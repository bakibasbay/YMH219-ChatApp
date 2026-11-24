namespace ChatApp.Client.Network;

// Kullanıcıya doğrudan gösterilebilecek bağlantı / gönderim hatası
public sealed class ChatClientException : Exception
{
    public ChatClientException(string title, string message, bool isJoinRejected = false, Exception? inner = null)
        : base(message, inner)
    {
        Title = title;
        IsJoinRejected = isJoinRejected;
    }

    public string Title { get; }

    // Sunucu kullanıcı adını reddettiyse true (form kullanıcı adı kutusuna odaklanır)
    public bool IsJoinRejected { get; }
}
