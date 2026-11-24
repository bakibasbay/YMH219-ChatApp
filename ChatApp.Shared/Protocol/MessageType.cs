namespace ChatApp.Shared.Protocol;

// Sunucu ile istemci arasında taşınan mesajın türü
public enum MessageType
{
    Join,          // İstemci → Sunucu: kullanıcı adıyla katılma isteği
    JoinAccepted,  // Sunucu → İstemci: katılma kabul edildi
    JoinRejected,  // Sunucu → İstemci: katılma reddedildi (İçerik = sebep)
    Chat,          // Herkese açık sohbet mesajı
    Private,       // Yalnızca Alıcı'ya giden özel mesaj
    UserList,      // Sunucu → İstemci: çevrimiçi kullanıcı listesi
    System,        // Sunucu → İstemci: bilgi / duyuru mesajı
    Leave          // Ayrılma bildirimi (istemci çıkarken veya sunucu kapanırken)
}
