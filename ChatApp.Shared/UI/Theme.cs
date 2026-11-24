using System.Drawing.Drawing2D;

namespace ChatApp.Shared.UI;

// Sunucu ve istemci pencerelerinde kullanılan ortak renkler ve çizim yardımcıları
public static class Theme
{
    public static readonly Color Background = Color.FromArgb(241, 245, 249);
    public static readonly Color Surface = Color.White;
    public static readonly Color SurfaceAlt = Color.FromArgb(248, 250, 252);
    public static readonly Color Header = Color.FromArgb(15, 23, 42);
    public static readonly Color HeaderHover = Color.FromArgb(30, 41, 59);
    public static readonly Color Border = Color.FromArgb(226, 232, 240);
    public static readonly Color Text = Color.FromArgb(30, 41, 59);
    public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
    public static readonly Color TextOnDark = Color.FromArgb(203, 213, 225);

    public static readonly Color Primary = Color.FromArgb(79, 70, 229);
    public static readonly Color PrimaryLight = Color.FromArgb(224, 231, 255);
    public static readonly Color Success = Color.FromArgb(16, 185, 129);
    public static readonly Color Warning = Color.FromArgb(245, 158, 11);
    public static readonly Color Danger = Color.FromArgb(239, 68, 68);
    public static readonly Color Idle = Color.FromArgb(148, 163, 184);

    public static readonly Color DisabledBack = Color.FromArgb(226, 232, 240);
    public static readonly Color DisabledText = Color.FromArgb(148, 163, 184);

    // Sohbet baloncukları
    public static readonly Color OwnBubble = Primary;
    public static readonly Color OtherBubble = Color.White;
    public static readonly Color PrivateOwnBubble = Color.FromArgb(217, 119, 6);
    public static readonly Color PrivateOtherBubble = Color.FromArgb(254, 243, 199);
    public static readonly Color PrivateBorder = Color.FromArgb(252, 211, 77);
    public static readonly Color PrivateText = Color.FromArgb(146, 64, 14);
    public static readonly Color AnnouncementBack = Color.FromArgb(254, 226, 226);
    public static readonly Color AnnouncementText = Color.FromArgb(153, 27, 27);

    // Kullanıcı adından renk üretmek için kullanılan palet (avatar ve isim rengi)
    private static readonly Color[] UserPalette =
    {
        Color.FromArgb(14, 165, 233), Color.FromArgb(16, 185, 129), Color.FromArgb(236, 72, 153),
        Color.FromArgb(245, 158, 11), Color.FromArgb(139, 92, 246), Color.FromArgb(20, 184, 166),
        Color.FromArgb(239, 68, 68), Color.FromArgb(99, 102, 241)
    };

    // Aynı kullanıcı adı her pencerede aynı rengi alır (string.GetHashCode süreçler arası sabit değildir)
    public static Color ColorForUser(string userName)
    {
        int hash = 17;
        foreach (char c in userName.ToLowerInvariant())
            hash = unchecked(hash * 31 + c);
        return UserPalette[(hash & 0x7fffffff) % UserPalette.Length];
    }

    // Rengi verilen oranda koyulaştırır (üzerine gelme / basma efekti için)
    public static Color Darken(Color color, float amount) => Color.FromArgb(
        color.A,
        (int)(color.R * (1 - amount)),
        (int)(color.G * (1 - amount)),
        (int)(color.B * (1 - amount)));

    // Köşeleri yuvarlatılmış dikdörtgen yolu oluşturur
    public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int d = Math.Max(1, Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height)));
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    // Baş harfli yuvarlak avatar çizer
    public static void DrawAvatar(Graphics g, Rectangle bounds, string userName, Font font)
    {
        var old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var brush = new SolidBrush(ColorForUser(userName)))
            g.FillEllipse(brush, bounds);
        g.SmoothingMode = old;

        string initial = userName.Length > 0 ? userName.Substring(0, 1).ToUpper() : "?";
        TextRenderer.DrawText(g, initial, font, bounds, Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }
}
