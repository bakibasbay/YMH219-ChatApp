using System.Globalization;
using System.Net;
using System.Net.Sockets;
using ChatApp.Shared;
using ChatApp.Shared.Protocol;

namespace ChatApp.Server.Network;

// TCP sohbet sunucusu: istemcileri kabul eder, katılma isteklerini doğrular,
// mesajları herkese veya özel olarak iletir. Arayüzden bağımsızdır; durumu olaylarla bildirir.
public sealed class ChatServer : IDisposable
{
    // Kullanıcı adları büyük/küçük harf duyarsız karşılaştırılır (Türkçe kurallarıyla: "ali" = "Ali")
    private static readonly StringComparer NameComparer =
        StringComparer.Create(CultureInfo.GetCultureInfo("tr-TR"), ignoreCase: true);

    private readonly object _sync = new();
    private readonly List<ClientConnection> _connections = new(); // tüm açık bağlantılar (katılmamış olanlar dahil)
    private readonly List<ClientConnection> _clients = new();     // sohbete katılmış istemciler
    private readonly HashSet<string> _takenNames = new(NameComparer);
    private readonly List<Task> _handlerTasks = new();

    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private Task? _acceptTask;
    private volatile bool _stopping;

    public bool IsRunning { get; private set; }
    public int Port { get; private set; }

    public event EventHandler<ServerLogEventArgs>? Log;
    public event EventHandler<ClientConnection>? ClientJoined;
    public event EventHandler<ClientConnection>? ClientLeft;
    public event EventHandler<ClientConnection>? ClientUpdated;

    public int ClientCount
    {
        get
        {
            lock (_sync)
                return _clients.Count;
        }
    }

    // Sunucuyu verilen portta başlatır; tüm ağ arayüzlerinden (yerel ağ dahil) bağlantı kabul eder
    public void Start(int port)
    {
        if (IsRunning)
            throw new InvalidOperationException("Sunucu zaten çalışıyor.");

        var listener = new TcpListener(IPAddress.Any, port);
        try
        {
            listener.Start();
        }
        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
        {
            throw new InvalidOperationException(
                $"{port} numaralı port başka bir uygulama (veya açık kalmış başka bir sunucu) tarafından kullanılıyor.\nFarklı bir port seçin.", ex);
        }
        catch (SocketException ex)
        {
            throw new InvalidOperationException("Sunucu başlatılamadı: " + ex.Message, ex);
        }

        _listener = listener;
        _cts = new CancellationTokenSource();
        _stopping = false;
        Port = port;
        IsRunning = true;

        OnLog(ServerLogKind.Info, $"Sunucu başlatıldı, port {port} dinleniyor.");
        string addresses = string.Join(", ", GetLocalIPv4Addresses());
        if (addresses.Length > 0)
            OnLog(ServerLogKind.Info, $"Aynı ağdaki istemciler şu adreslerle bağlanabilir: {addresses}");

        _acceptTask = AcceptLoopAsync(_cts.Token);
    }

    // Sunucuyu durdurur: istemcilere bildirim gönderir, tüm bağlantıları ve dinleyiciyi kapatır
    public async Task StopAsync()
    {
        if (!IsRunning || _cts is null)
            return;

        _stopping = true;

        ClientConnection[] snapshot;
        lock (_sync)
            snapshot = _connections.ToArray();

        // Önce kapanış bildirimi gönderilir (en fazla 2 saniye beklenir), sonra döngüler iptal edilir
        using (var sendTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
        {
            var notice = ChatMessage.Leave(ChatMessage.ServerName, "Sunucu kapatıldı.");
            await Task.WhenAll(snapshot.Select(c => TrySendAsync(c, notice, sendTimeout.Token))).ConfigureAwait(false);
        }

        _cts.Cancel();
        _listener?.Stop();

        lock (_sync)
            snapshot = _connections.ToArray();
        foreach (var connection in snapshot)
            connection.Close();

        // Kabul döngüsü ve istemci görevlerinin bitmesi beklenir
        Task[] pending;
        lock (_sync)
            pending = _handlerTasks.Append(_acceptTask ?? Task.CompletedTask).ToArray();
        await Task.WhenAny(Task.WhenAll(pending), Task.Delay(TimeSpan.FromSeconds(3))).ConfigureAwait(false);

        lock (_sync)
        {
            _handlerTasks.Clear();
            _connections.Clear();
            _clients.Clear();
            _takenNames.Clear();
        }

        _cts.Dispose();
        _cts = null;
        _listener = null;
        IsRunning = false;
        OnLog(ServerLogKind.Info, "Sunucu durduruldu, tüm bağlantılar kapatıldı.");
    }

    // Sunucudan tüm istemcilere duyuru gönderir
    public async Task AnnounceAsync(string text)
    {
        if (!IsRunning)
            return;

        await BroadcastAsync(ChatMessage.Announcement(text)).ConfigureAwait(false);
        OnLog(ServerLogKind.Info, "Duyuru gönderildi: " + text);
    }

    public IReadOnlyList<ClientConnection> GetClients()
    {
        lock (_sync)
            return _clients.ToArray();
    }

    // Yeni bağlantıları kabul eden döngü; her istemci kendi görevinde işlenir
    private async Task AcceptLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            TcpClient tcpClient;
            try
            {
                tcpClient = await _listener!.AcceptTcpClientAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is OperationCanceledException or ObjectDisposedException or InvalidOperationException)
            {
                break; // dinleyici durduruldu
            }
            catch (SocketException ex)
            {
                if (token.IsCancellationRequested || _stopping)
                    break;
                OnLog(ServerLogKind.Error, "Bağlantı kabul edilemedi: " + ex.Message);
                continue;
            }

            var connection = new ClientConnection(tcpClient);
            OnLog(ServerLogKind.Info, $"Yeni bağlantı: {connection.RemoteEndPoint} (katılma isteği bekleniyor)");

            lock (_sync)
            {
                _connections.Add(connection);
                _handlerTasks.RemoveAll(t => t.IsCompleted);
                _handlerTasks.Add(HandleClientAsync(connection, token));
            }
        }
    }

    // Tek bir istemcinin yaşam döngüsü: katılma → mesaj döngüsü → ayrılma
    private async Task HandleClientAsync(ClientConnection connection, CancellationToken token)
    {
        await Task.Yield();
        try
        {
            if (!await HandshakeAsync(connection, token).ConfigureAwait(false))
                return;

            await connection.ReadLoopAsync(message => HandleMessageAsync(connection, message), token)
                .ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is OperationCanceledException or IOException or SocketException or ObjectDisposedException)
        {
            // Bağlantı koptu veya sunucu durduruluyor: normal kapanış yolu
        }
        catch (Exception ex)
        {
            OnLog(ServerLogKind.Error, $"{connection.DisplayName} işlenirken hata: {ex.Message}");
        }
        finally
        {
            await RemoveClientAsync(connection).ConfigureAwait(false);
        }
    }

    // İlk mesaj Join olmalı; kullanıcı adı doğrulanır ve benzersizliği kontrol edilir
    private async Task<bool> HandshakeAsync(ClientConnection connection, CancellationToken token)
    {
        ChatMessage? join;
        using (var timeout = CancellationTokenSource.CreateLinkedTokenSource(token))
        {
            timeout.CancelAfter(ChatConstants.JoinTimeout);
            try
            {
                join = await connection.ReadMessageAsync(timeout.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (!token.IsCancellationRequested)
            {
                OnLog(ServerLogKind.Warning, $"{connection.RemoteEndPoint} süresi içinde katılma isteği göndermedi, bağlantı kapatıldı.");
                return false;
            }
        }

        if (join is null)
            return false;

        if (join.Type != MessageType.Join)
        {
            OnLog(ServerLogKind.Warning, $"{connection.RemoteEndPoint} geçersiz ilk mesaj gönderdi ({join.Type}), bağlantı kapatıldı.");
            return false;
        }

        string name = join.Sender?.Trim() ?? string.Empty;
        string? error = UserNameValidator.Validate(name);
        if (error is null)
        {
            // Ad kontrolü ve ayırma tek kilit içinde yapılır; aynı anda gelen iki istek aynı adı alamaz
            lock (_sync)
            {
                if (!_takenNames.Add(name))
                    error = $"\"{name}\" kullanıcı adı şu anda sohbette kullanılıyor. Lütfen başka bir ad seçin.";
            }
        }

        if (error is not null)
        {
            await TrySendAsync(connection, ChatMessage.JoinRejected(error), token).ConfigureAwait(false);
            OnLog(ServerLogKind.Warning, $"Katılma reddedildi ({connection.RemoteEndPoint}, \"{name}\"): {error}");
            return false;
        }

        connection.UserName = name;
        await connection.SendAsync(ChatMessage.JoinAccepted(name), token).ConfigureAwait(false);

        // Kabul mesajı gittikten sonra yayın listesine eklenir; böylece istemci ilk olarak kabul mesajını alır
        lock (_sync)
            _clients.Add(connection);

        ClientJoined?.Invoke(this, connection);
        OnLog(ServerLogKind.Connected, $"{name} sohbete katıldı ({connection.RemoteEndPoint}).");

        await BroadcastAsync(ChatMessage.System($"{name} sohbete katıldı"), except: connection).ConfigureAwait(false);
        await BroadcastUserListAsync().ConfigureAwait(false);
        return true;
    }

    // Katılmış bir istemciden gelen mesajı işler; false dönerse okuma döngüsü biter
    private async Task<bool> HandleMessageAsync(ClientConnection sender, ChatMessage message)
    {
        switch (message.Type)
        {
            case MessageType.Chat:
                if (!await CheckContentAsync(sender, message.Content).ConfigureAwait(false))
                    return true;

                // Gönderen ve zaman sunucuda belirlenir (istemci başka biri adına yazamaz)
                var chat = ChatMessage.Chat(sender.UserName, message.Content.Trim());
                sender.IncrementMessageCount();
                ClientUpdated?.Invoke(this, sender);
                OnLog(ServerLogKind.Message, $"{sender.UserName}: {chat.Content}");
                await BroadcastAsync(chat).ConfigureAwait(false);
                return true;

            case MessageType.Private:
                await HandlePrivateAsync(sender, message).ConfigureAwait(false);
                return true;

            case MessageType.Leave:
                return false;

            default:
                OnLog(ServerLogKind.Warning, $"{sender.UserName} beklenmeyen mesaj türü gönderdi: {message.Type}");
                return true;
        }
    }

    // Özel mesaj yalnızca alıcıya iletilir; gönderene de kopyası döner
    private async Task HandlePrivateAsync(ClientConnection sender, ChatMessage message)
    {
        if (!await CheckContentAsync(sender, message.Content).ConfigureAwait(false))
            return;

        string recipientName = message.Recipient?.Trim() ?? string.Empty;
        ClientConnection? recipient;
        lock (_sync)
            recipient = _clients.FirstOrDefault(c => NameComparer.Equals(c.UserName, recipientName));

        if (recipient is null)
        {
            await TrySendAsync(sender, ChatMessage.System($"Özel mesaj gönderilemedi: \"{recipientName}\" çevrimiçi değil.")).ConfigureAwait(false);
            return;
        }

        if (recipient == sender)
        {
            await TrySendAsync(sender, ChatMessage.System("Kendinize özel mesaj gönderemezsiniz.")).ConfigureAwait(false);
            return;
        }

        var privateMessage = ChatMessage.Private(sender.UserName, recipient.UserName, message.Content.Trim());
        sender.IncrementMessageCount();
        ClientUpdated?.Invoke(this, sender);
        // Özel mesajın içeriği sunucu günlüğüne yazılmaz, yalnızca kimden kime gittiği görünür
        OnLog(ServerLogKind.Private, $"{sender.UserName} → {recipient.UserName} özel mesaj gönderdi.");

        await TrySendAsync(recipient, privateMessage).ConfigureAwait(false);
        await TrySendAsync(sender, privateMessage).ConfigureAwait(false);
    }

    // Boş veya çok uzun mesajlar iletilmez, gönderene bilgi verilir
    private async Task<bool> CheckContentAsync(ClientConnection sender, string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return false;

        if (content.Length > ChatConstants.MaxMessageLength)
        {
            await TrySendAsync(sender, ChatMessage.System(
                $"Mesaj iletilmedi: en fazla {ChatConstants.MaxMessageLength} karakter gönderilebilir.")).ConfigureAwait(false);
            return false;
        }

        return true;
    }

    // Bağlantıyı listelerden çıkarır, gerekirse diğerlerine ayrılma bildirimi gönderir
    private async Task RemoveClientAsync(ClientConnection connection)
    {
        bool wasJoined;
        lock (_sync)
        {
            _connections.Remove(connection);
            wasJoined = _clients.Remove(connection);
            if (connection.IsJoined)
                _takenNames.Remove(connection.UserName);
        }

        connection.Dispose();

        if (!wasJoined)
            return;

        ClientLeft?.Invoke(this, connection);

        if (_stopping)
        {
            OnLog(ServerLogKind.Disconnected, $"{connection.UserName} bağlantısı kapatıldı (sunucu durduruluyor).");
            return;
        }

        OnLog(ServerLogKind.Disconnected, $"{connection.UserName} sohbetten ayrıldı.");
        await BroadcastAsync(ChatMessage.System($"{connection.UserName} sohbetten ayrıldı")).ConfigureAwait(false);
        await BroadcastUserListAsync().ConfigureAwait(false);
    }

    private Task BroadcastUserListAsync()
    {
        string[] names;
        lock (_sync)
            names = _clients.Select(c => c.UserName).ToArray();
        return BroadcastAsync(ChatMessage.UserListOf(names));
    }

    // Mesajı katılmış tüm istemcilere (isteğe bağlı biri hariç) paralel gönderir
    private Task BroadcastAsync(ChatMessage message, ClientConnection? except = null)
    {
        ClientConnection[] targets;
        lock (_sync)
            targets = _clients.Where(c => c != except).ToArray();

        return Task.WhenAll(targets.Select(c => TrySendAsync(c, message)));
    }

    // Gönderim hatası tek istemciyi etkiler: bağlantı kapatılır, okuma döngüsü temizliği yapar
    private async Task TrySendAsync(ClientConnection connection, ChatMessage message, CancellationToken token = default)
    {
        try
        {
            await connection.SendAsync(message, token).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException or OperationCanceledException or InvalidOperationException)
        {
            if (!_stopping)
                OnLog(ServerLogKind.Error, $"{connection.DisplayName} adresine gönderilemedi, bağlantı kapatılıyor.");
            connection.Close();
        }
    }

    private void OnLog(ServerLogKind kind, string text) =>
        Log?.Invoke(this, new ServerLogEventArgs(kind, text));

    // Yerel ağdaki IPv4 adresleri (README'deki "başka bilgisayardan bağlanma" için günlükte gösterilir)
    private static IEnumerable<string> GetLocalIPv4Addresses()
    {
        try
        {
            return Dns.GetHostAddresses(Dns.GetHostName())
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a))
                .Select(a => a.ToString())
                .Distinct()
                .ToArray();
        }
        catch (SocketException)
        {
            return Array.Empty<string>();
        }
    }

    public void Dispose()
    {
        if (IsRunning)
            StopAsync().GetAwaiter().GetResult();
    }
}
