using ChatApp.Shared.Protocol;

namespace ChatApp.Shared;

// Kullanıcı adı kuralları: hem istemcide (gönderilmeden önce) hem sunucuda (güvenlik için) kontrol edilir
public static class UserNameValidator
{
    // Geçerliyse null, değilse kullanıcıya gösterilecek hata mesajını döner
    public static string? Validate(string? userName)
    {
        string name = userName?.Trim() ?? string.Empty;

        if (name.Length == 0)
            return "Kullanıcı adı boş olamaz.";

        if (name.Length > ChatConstants.MaxUserNameLength)
            return $"Kullanıcı adı en fazla {ChatConstants.MaxUserNameLength} karakter olabilir.";

        // Harf, rakam ve _ - . dışındaki karakterlere (boşluk dahil) izin verilmez
        if (!name.All(c => char.IsLetterOrDigit(c) || c is '_' or '-' or '.'))
            return "Kullanıcı adında yalnızca harf, rakam ve _ - . karakterleri kullanılabilir (boşluk olmadan).";

        if (string.Equals(name, ChatMessage.ServerName, StringComparison.OrdinalIgnoreCase))
            return $"\"{ChatMessage.ServerName}\" adı sisteme ayrılmıştır, başka bir ad seçin.";

        return null;
    }
}
