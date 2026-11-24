using System.Net.Sockets;
using ChatApp.Shared;
using ChatApp.Shared.Protocol;

namespace ChatApp.Server.Network;

// Sunucuya bağlı tek bir istemciyi temsil eder: soket, okuma/yazma akışları ve istemci bilgileri
public sealed class ClientConnection : IDisposable
{
    private readonly TcpClient _tcpClient;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;
    // Aynı anda birden fazla görev yazarsa satırlar karışmasın diye yazma kilidi
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private int _messageCount;
    private bool _disposed;

    public ClientConnection(TcpClient tcpClient)
    {
        _tcpClient = tcpClient;
        _tcpClient.NoDelay = true;

        var stream = tcpClient.GetStream();
        _reader = new StreamReader(stream, ChatConstants.Encoding);
        _writer = new StreamWriter(stream, ChatConstants.Encoding) { AutoFlush = false };

        Id = Guid.NewGuid();
        RemoteEndPoint = tcpClient.Client.RemoteEndPoint?.ToString() ?? "bilinmiyor";
        ConnectedAt = DateTime.Now;
    }

    public Guid Id { get; }
    public string RemoteEndPoint { get; }
    public DateTime ConnectedAt { get; }

    // Katılma isteği kabul edilene kadar boştur
    public string UserName { get; set; } = string.Empty;
    public bool IsJoined => UserName.Length > 0;

    // İstemcinin gönderdiği sohbet / özel mesaj sayısı
    public int MessageCount => Volatile.Read(ref _messageCount);

    public void IncrementMessageCount() => Interlocked.Increment(ref _messageCount);

    // Günlükte gösterilecek kısa ad: "Ali (127.0.0.1:52344)"
    public string DisplayName => IsJoined ? $"{UserName} ({RemoteEndPoint})" : RemoteEndPoint;

    // Tek bir mesajı JSON satırı olarak gönderir
    public async Task SendAsync(ChatMessage message, CancellationToken token = default)
    {
        if (_disposed)
            return;

        string line = MessageSerializer.Serialize(message);
        await _writeLock.WaitAsync(token).ConfigureAwait(false);
        try
        {
            await _writer.WriteLineAsync(line.AsMemory(), token).ConfigureAwait(false);
            await _writer.FlushAsync(token).ConfigureAwait(false);
        }
        finally
        {
            _writeLock.Release();
        }
    }

    // Bir sonraki mesajı okur; bağlantı kapandıysa null döner
    public async Task<ChatMessage?> ReadMessageAsync(CancellationToken token)
    {
        while (true)
        {
            string? line = await _reader.ReadLineAsync(token).ConfigureAwait(false);
            if (line is null)
                return null; // karşı taraf bağlantıyı kapattı

            var message = MessageSerializer.Deserialize(line);
            if (message is not null)
                return message;
            // Bozuk satırlar atlanır
        }
    }

    // Asenkron okuma döngüsü: bağlantı kapanana veya iptal edilene kadar gelen her mesajı işleyiciye verir.
    // İşleyici false dönerse döngü sonlanır (ör. istemci Leave gönderdiğinde).
    public async Task ReadLoopAsync(Func<ChatMessage, Task<bool>> handler, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var message = await ReadMessageAsync(token).ConfigureAwait(false);
            if (message is null)
                break;

            if (!await handler(message).ConfigureAwait(false))
                break;
        }
    }

    // Soketi kapatır; bekleyen okuma işlemi hata ile sonlanır
    public void Close()
    {
        try
        {
            _tcpClient.Client.Shutdown(SocketShutdown.Both);
        }
        catch (Exception)
        {
            // Soket zaten kapanmış olabilir
        }

        _tcpClient.Close();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        Close();
        _reader.Dispose();
        try
        {
            _writer.Dispose();
        }
        catch (Exception)
        {
            // Kapalı akışa boşaltma hatası önemsiz
        }

        _writeLock.Dispose();
    }
}
