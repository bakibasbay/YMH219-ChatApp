using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatApp.Shared.Protocol;

// ChatMessage <-> tek satır JSON dönüşümü.
// JSON içindeki satır sonları "\n" olarak kaçışlandığı için her mesaj tam olarak bir satırdır;
// böylece akıştan ReadLine ile mesaj mesaj okunabilir.
public static class MessageSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // Türkçe karakterler ç gibi kaçışlanmadan okunabilir şekilde yazılsın
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter() },
        WriteIndented = false
    };

    public static string Serialize(ChatMessage message) =>
        JsonSerializer.Serialize(message, Options);

    // Bozuk veya tanınmayan satırda null döner (bağlantıyı düşürmek yerine satır atlanır)
    public static ChatMessage? Deserialize(string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        try
        {
            return JsonSerializer.Deserialize<ChatMessage>(line, Options);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
