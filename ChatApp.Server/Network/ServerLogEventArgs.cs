namespace ChatApp.Server.Network;

// Olay günlüğündeki kaydın türü (günlükte renklendirme için kullanılır)
public enum ServerLogKind
{
    Info,
    Connected,
    Disconnected,
    Message,
    Private,
    Warning,
    Error
}

// Sunucunun arayüze bildirdiği tek bir günlük kaydı
public sealed class ServerLogEventArgs : EventArgs
{
    public ServerLogEventArgs(ServerLogKind kind, string text)
    {
        Kind = kind;
        Text = text;
        Time = DateTime.Now;
    }

    public DateTime Time { get; }
    public ServerLogKind Kind { get; }
    public string Text { get; }
}
