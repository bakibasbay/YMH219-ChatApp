using System.Text;
using ChatApp.Shared.Protocol;

namespace ChatApp.Client.Services;

// Ekranda gösterilen mesajları tutar ve sohbet geçmişini .txt dosyasına kaydeder
public sealed class HistoryService
{
    private readonly List<ChatMessage> _messages = new();

    public int Count => _messages.Count;

    public void Add(ChatMessage message) => _messages.Add(message);

    public void Clear() => _messages.Clear();

    // Kaydetme penceresinde önerilen dosya adı
    public static string SuggestFileName() => $"sohbet_gecmisi_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

    // Geçmişi UTF-8 metin dosyasına yazar; hata durumunda anlaşılır mesajla IOException fırlatır
    public void SaveToFile(string path, string userName, string serverAddress)
    {
        var sb = new StringBuilder();
        sb.AppendLine("YMH219 Chat Uygulaması – Sohbet Geçmişi");
        sb.AppendLine($"Kullanıcı   : {(userName.Length > 0 ? userName : "-")}");
        sb.AppendLine($"Sunucu      : {(serverAddress.Length > 0 ? serverAddress : "-")}");
        sb.AppendLine($"Kaydedilme  : {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        sb.AppendLine($"Mesaj sayısı: {_messages.Count}");
        sb.AppendLine(new string('-', 60));

        foreach (var message in _messages)
            sb.AppendLine(Format(message));

        try
        {
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new IOException("Bu konuma yazma izniniz yok. Farklı bir klasör seçin.", ex);
        }
        catch (IOException ex)
        {
            throw new IOException("Dosya kaydedilemedi (dosya başka bir programda açık olabilir): " + ex.Message, ex);
        }
    }

    // Tek bir mesajın dosyadaki satır biçimi
    public static string Format(ChatMessage message)
    {
        string time = $"[{message.Timestamp:dd.MM.yyyy HH:mm:ss}]";
        string content = message.Content.Replace("\r", "").Replace("\n", " ");

        return message.Type switch
        {
            MessageType.Chat => $"{time} {message.Sender}: {content}",
            MessageType.Private => $"{time} (Özel) {message.Sender} → {message.Recipient}: {content}",
            MessageType.System when message.IsAnnouncement => $"{time} [Sunucu duyurusu] {content}",
            _ => $"{time} * {content}"
        };
    }
}
