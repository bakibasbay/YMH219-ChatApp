namespace ChatApp.Client.Network;

// Bağlantı sona erdiğinde nedenini ve kullanıcının isteğiyle olup olmadığını taşır
public sealed class DisconnectedEventArgs : EventArgs
{
    public DisconnectedEventArgs(string reason, bool byUser)
    {
        Reason = reason;
        ByUser = byUser;
    }

    public string Reason { get; }
    public bool ByUser { get; }
}
