using System.Net.Sockets;
using ChatApp.Shared;
using ChatApp.Shared.Protocol;

namespace ChatApp.Client.Network;

// Sunucuya bağlanan, mesaj gönderen ve gelen mesajları arka planda dinleyen istemci.
// Arayüzden bağımsızdır; olaylar arka plan iş parçacığından tetiklenir.
public sealed class ChatClient : IDisposable
{
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private TcpClient? _tcpClient;
    private StreamReader? _reader;
    private StreamWriter? _writer;
    private CancellationTokenSource? _listenCts;
    private Task? _listenTask;
    private volatile bool _disconnectRequested;
    private string? _serverLeaveReason;

    public bool IsConnected { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string ServerAddress { get; private set; } = string.Empty;

    // Sohbet, özel, sistem ve duyuru mesajları
    public event EventHandler<ChatMessage>? MessageReceived;
    // Çevrimiçi kullanıcı listesi değişti
    public event EventHandler<IReadOnlyList<string>>? UserListChanged;
    // Bağlantı sona erdi (kullanıcı kesti, sunucu kapandı veya bağlantı koptu)
    public event EventHandler<DisconnectedEventArgs>? Disconnected;

    // Sunucuya bağlanır ve katılma isteği gönderir; başarısızlıkta ChatClientException fırlatır
    public async Task ConnectAsync(string host, int port, string userName, CancellationToken token = default)
    {
        if (IsConnected)
            throw new InvalidOperationException("Zaten bağlı.");

        host = host.Trim();
        userName = userName.Trim();
        if (host.Length == 0)
            throw new ChatClientException("Eksik bilgi", "Sunucu IP adresi boş olamaz.");

        string? nameError = UserNameValidator.Validate(userName);
        if (nameError is not null)
            throw new ChatClientException("Geçersiz kullanıcı adı", nameError, isJoinRejected: true);

        var tcpClient = new TcpClient { NoDelay = true };
        try
        {
            // 1) TCP bağlantısı (en fazla ConnectTimeout kadar beklenir)
            using (var timeout = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                timeout.CancelAfter(ChatConstants.ConnectTimeout);
                try
                {
                    await tcpClient.ConnectAsync(host, port, timeout.Token);
                }
                catch (OperationCanceledException) when (!token.IsCancellationRequested)
                {
                    throw new ChatClientException("Bağlantı zaman aşımı",
                        $"{host}:{port} adresine {ChatConstants.ConnectTimeout.TotalSeconds:0} saniye içinde bağlanılamadı.\n" +
                        "IP adresini, portu ve sunucu bilgisayarın güvenlik duvarı ayarlarını kontrol edin.");
                }
            }

            var stream = tcpClient.GetStream();
            var reader = new StreamReader(stream, ChatConstants.Encoding);
            var writer = new StreamWriter(stream, ChatConstants.Encoding) { AutoFlush = false };

            // 2) Katılma isteği ve sunucunun cevabı
            await WriteLineAsync(writer, ChatMessage.Join(userName), token);
            ChatMessage reply = await ReadJoinReplyAsync(reader, token);

            if (reply.Type == MessageType.JoinRejected)
                throw new ChatClientException("Katılma reddedildi", reply.Content, isJoinRejected: true);

            // 3) Bağlantı hazır: dinleme döngüsü başlatılır
            _tcpClient = tcpClient;
            _reader = reader;
            _writer = writer;
            _disconnectRequested = false;
            _serverLeaveReason = null;
            UserName = userName;
            ServerAddress = $"{host}:{port}";
            IsConnected = true;

            // Karşılama mesajı, dinleme başlamadan önce bildirilir (sıra karışmasın)
            MessageReceived?.Invoke(this, ChatMessage.System(reply.Content));

            // Dinleme döngüsü arka plan iş parçacığında çalışır; arayüz donmaz
            _listenCts = new CancellationTokenSource();
            var listenToken = _listenCts.Token;
            _listenTask = Task.Run(() => ListenLoopAsync(listenToken));
        }
        catch (ChatClientException)
        {
            tcpClient.Dispose();
            throw;
        }
        catch (OperationCanceledException)
        {
            tcpClient.Dispose();
            throw;
        }
        catch (SocketException ex)
        {
            tcpClient.Dispose();
            throw new ChatClientException("Sunucuya bağlanılamadı", DescribeSocketError(ex, host, port), inner: ex);
        }
        catch (IOException ex)
        {
            tcpClient.Dispose();
            throw new ChatClientException("Bağlantı hatası",
                "Sunucu ile bağlantı kurulurken bağlantı koptu. Lütfen tekrar deneyin.", inner: ex);
        }
    }

    // Katılma isteğine gelen cevabı bekler (JoinAccepted veya JoinRejected)
    private static async Task<ChatMessage> ReadJoinReplyAsync(StreamReader reader, CancellationToken token)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);
        timeout.CancelAfter(ChatConstants.JoinTimeout);
        try
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync(timeout.Token);
                if (line is null)
                    throw new ChatClientException("Bağlantı hatası", "Sunucu katılma isteğine cevap vermeden bağlantıyı kapattı.");

                var message = MessageSerializer.Deserialize(line);
                if (message?.Type is MessageType.JoinAccepted or MessageType.JoinRejected)
                    return message;
            }
        }
        catch (OperationCanceledException) when (!token.IsCancellationRequested)
        {
            throw new ChatClientException("Cevap yok",
                "Bağlanılan adreste bir sunucu var ancak sohbet sunucusu gibi cevap vermiyor. Port numarasını kontrol edin.");
        }
    }

    // Herkese açık mesaj gönderir
    public Task SendChatAsync(string text) =>
        SendAsync(ChatMessage.Chat(UserName, PrepareContent(text)));

    // Yalnızca alıcının göreceği özel mesaj gönderir
    public Task SendPrivateAsync(string recipient, string text) =>
        SendAsync(ChatMessage.Private(UserName, recipient, PrepareContent(text)));

    private static string PrepareContent(string text)
    {
        text = text.Trim();
        if (text.Length == 0)
            throw new ChatClientException("Boş mesaj", "Boş mesaj gönderilemez.");
        if (text.Length > ChatConstants.MaxMessageLength)
            throw new ChatClientException("Mesaj çok uzun", $"Mesaj en fazla {ChatConstants.MaxMessageLength} karakter olabilir.");
        return text;
    }

    private async Task SendAsync(ChatMessage message)
    {
        var writer = _writer;
        if (!IsConnected || writer is null)
            throw new ChatClientException("Bağlı değil", "Mesaj göndermek için önce sunucuya bağlanın.");

        try
        {
            await _writeLock.WaitAsync();
            try
            {
                await WriteLineAsync(writer, message, CancellationToken.None);
            }
            finally
            {
                _writeLock.Release();
            }
        }
        catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException)
        {
            // Yazılamıyorsa bağlantı kopmuştur: soket kapatılır, dinleme döngüsü Disconnected olayını tetikler
            _tcpClient?.Close();
            throw new ChatClientException("Mesaj gönderilemedi", "Sunucu ile bağlantı koptu, mesaj iletilemedi.", inner: ex);
        }
    }

    private static async Task WriteLineAsync(StreamWriter writer, ChatMessage message, CancellationToken token)
    {
        await writer.WriteLineAsync(MessageSerializer.Serialize(message).AsMemory(), token);
        await writer.FlushAsync(token);
    }

    // Kullanıcının isteğiyle bağlantıyı kapatır; sunucuya ayrılma bildirimi gönderilir
    public async Task DisconnectAsync()
    {
        if (!IsConnected)
            return;

        _disconnectRequested = true;
        try
        {
            var writer = _writer;
            if (writer is not null)
            {
                using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(1));
                await _writeLock.WaitAsync(timeout.Token);
                try
                {
                    await WriteLineAsync(writer, ChatMessage.Leave(UserName), timeout.Token);
                }
                finally
                {
                    _writeLock.Release();
                }
            }
        }
        catch (Exception)
        {
            // Bağlantı zaten kopmuş olabilir; kapatmaya devam edilir
        }

        _listenCts?.Cancel();
        _tcpClient?.Close();

        if (_listenTask is not null)
            await _listenTask;
    }

    // Arka planda sunucudan gelen satırları okur ve ilgili olayları tetikler
    private async Task ListenLoopAsync(CancellationToken token)
    {
        string reason = "Sunucu bağlantıyı kapattı.";
        try
        {
            while (!token.IsCancellationRequested)
            {
                string? line = await _reader!.ReadLineAsync(token).ConfigureAwait(false);
                if (line is null)
                    break; // sunucu bağlantıyı kapattı

                var message = MessageSerializer.Deserialize(line);
                if (message is null)
                    continue;

                switch (message.Type)
                {
                    case MessageType.UserList:
                        UserListChanged?.Invoke(this, message.Users ?? new List<string>());
                        break;

                    case MessageType.Leave when message.Sender == ChatMessage.ServerName:
                        // Sunucu kapanıyor: sebep saklanır, bağlantı kapanınca bildirilir
                        _serverLeaveReason = message.Content;
                        return;

                    default:
                        MessageReceived?.Invoke(this, message);
                        break;
                }
            }
        }
        catch (Exception ex) when (ex is OperationCanceledException or IOException or SocketException or ObjectDisposedException)
        {
            if (!_disconnectRequested)
                reason = "Sunucu ile bağlantı beklenmedik şekilde koptu.";
        }
        finally
        {
            if (_serverLeaveReason is { Length: > 0 })
                reason = _serverLeaveReason;
            if (_disconnectRequested)
                reason = "Bağlantı kesildi.";

            CloseConnection();
            Disconnected?.Invoke(this, new DisconnectedEventArgs(reason, _disconnectRequested));
        }
    }

    private void CloseConnection()
    {
        IsConnected = false;
        try
        {
            _tcpClient?.Close();
        }
        catch (Exception)
        {
            // Kapatma sırasında oluşan hatalar önemsiz
        }

        _reader?.Dispose();
        _tcpClient?.Dispose();
        _listenCts?.Dispose();
        _tcpClient = null;
        _reader = null;
        _writer = null;
        _listenCts = null;
    }

    // Soket hata kodlarını anlaşılır Türkçe mesajlara çevirir
    private static string DescribeSocketError(SocketException ex, string host, int port) => ex.SocketErrorCode switch
    {
        SocketError.ConnectionRefused =>
            $"{host}:{port} adresinde çalışan bir sohbet sunucusu bulunamadı (bağlantı reddedildi).\n" +
            "Sunucunun başlatıldığından, IP adresi ve portun doğru olduğundan emin olun.",
        SocketError.HostNotFound or SocketError.NoData or SocketError.TryAgain =>
            $"\"{host}\" adresi bulunamadı. IP adresini kontrol edin (ör. 127.0.0.1 veya 192.168.1.20).",
        SocketError.TimedOut or SocketError.HostUnreachable or SocketError.NetworkUnreachable =>
            $"{host}:{port} adresine ulaşılamadı. Bilgisayarların aynı ağda olduğundan ve güvenlik duvarının izin verdiğinden emin olun.",
        SocketError.AddressNotAvailable =>
            $"\"{host}\" bağlanılabilecek geçerli bir adres değil.",
        _ => $"Bağlantı kurulamadı: {ex.Message}"
    };

    public void Dispose()
    {
        if (IsConnected)
        {
            _disconnectRequested = true;
            _listenCts?.Cancel();
            _tcpClient?.Close();
        }

        _writeLock.Dispose();
    }
}
