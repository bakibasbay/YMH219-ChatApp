using System.Text;

namespace ChatApp.Shared;

// Sunucu ve istemcinin ortak kullandığı sabit değerler
public static class ChatConstants
{
    public const int DefaultPort = 5000;
    public const string DefaultHost = "127.0.0.1";

    public const int MaxUserNameLength = 20;
    public const int MaxMessageLength = 1000;

    // Bağlanma ve katılma isteği için bekleme süreleri
    public static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(5);
    public static readonly TimeSpan JoinTimeout = TimeSpan.FromSeconds(10);

    // Akışlarda BOM'suz UTF-8 kullanılır (ilk satırın başına görünmez karakter eklenmesin)
    public static readonly Encoding Encoding = new UTF8Encoding(false);
}
