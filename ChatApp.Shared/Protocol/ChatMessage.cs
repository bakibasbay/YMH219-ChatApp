using System.Text.Json.Serialization;

namespace ChatApp.Shared.Protocol;

// Ağ üzerinden gönderilen tek bir mesaj. Her mesaj tek satırlık JSON olarak yazılır.
public class ChatMessage
{
    // Sunucunun kendi adına gönderdiği mesajlarda kullanılan gönderen adı
    public const string ServerName = "Sunucu";

    public MessageType Type { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string? Recipient { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;

    // Yalnızca UserList mesajında dolu olur: çevrimiçi kullanıcı adları
    public List<string>? Users { get; set; }

    // Sık kullanılan mesajlar için kısa oluşturucu metotlar
    public static ChatMessage Join(string userName) =>
        new() { Type = MessageType.Join, Sender = userName };

    public static ChatMessage JoinAccepted(string userName) =>
        new() { Type = MessageType.JoinAccepted, Sender = ServerName, Recipient = userName, Content = "Sohbete hoş geldin, " + userName + "!" };

    public static ChatMessage JoinRejected(string reason) =>
        new() { Type = MessageType.JoinRejected, Sender = ServerName, Content = reason };

    public static ChatMessage Chat(string sender, string content) =>
        new() { Type = MessageType.Chat, Sender = sender, Content = content };

    public static ChatMessage Private(string sender, string recipient, string content) =>
        new() { Type = MessageType.Private, Sender = sender, Recipient = recipient, Content = content };

    public static ChatMessage UserListOf(IEnumerable<string> users) =>
        new() { Type = MessageType.UserList, Sender = ServerName, Users = users.ToList() };

    public static ChatMessage System(string content) =>
        new() { Type = MessageType.System, Sender = ServerName, Content = content };

    public static ChatMessage Leave(string sender, string reason = "") =>
        new() { Type = MessageType.Leave, Sender = sender, Content = reason };

    // Duyurularda Alıcı alanı "*" (herkes) olur; sıradan sistem bildirimlerinde boştur
    public const string AnnouncementTag = "*";

    [JsonIgnore]
    public bool IsAnnouncement => Type == MessageType.System && Recipient == AnnouncementTag;

    public static ChatMessage Announcement(string content) =>
        new() { Type = MessageType.System, Sender = ServerName, Recipient = AnnouncementTag, Content = content };

    public override string ToString() => $"[{Timestamp:HH:mm:ss}] {Type} {Sender}: {Content}";
}
