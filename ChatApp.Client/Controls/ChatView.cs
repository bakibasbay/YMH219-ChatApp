using System.ComponentModel;
using System.Drawing.Drawing2D;
using ChatApp.Shared.Protocol;
using ChatApp.Shared.UI;

namespace ChatApp.Client.Controls;

// Mesajları baloncuk şeklinde çizen, kaydırılabilir sohbet alanı (owner-draw).
// Kendi mesajları sağda, diğerleri solda; sistem mesajları ortada gri; özel mesajlar farklı renkte.
public class ChatView : Control
{
    private const int Pad = 16;          // kenar boşluğu
    private const int BubblePadX = 12;   // baloncuk iç yatay boşluk
    private const int BubblePadY = 8;    // baloncuk iç dikey boşluk
    private const int AvatarSize = 30;
    private const int AvatarGap = 8;
    private const int ItemSpacing = 10;  // mesajlar arası boşluk
    private const int Radius = 14;

    private const TextFormatFlags WrapFlags =
        TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;
    private const TextFormatFlags LineFlags =
        TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;

    private readonly List<ChatItem> _items = new();
    private readonly VScrollBar _scrollBar = new() { Dock = DockStyle.Right, Visible = false };
    private readonly Font _textFont = new("Segoe UI", 10.5F);
    private readonly Font _headerFont = new("Segoe UI Semibold", 9F, FontStyle.Bold);
    private readonly Font _timeFont = new("Segoe UI", 8F);
    private readonly Font _systemFont = new("Segoe UI", 9F);
    private readonly Font _avatarFont = new("Segoe UI Semibold", 10F, FontStyle.Bold);
    private int _contentHeight;

    public ChatView()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        BackColor = Theme.SurfaceAlt;
        _scrollBar.ValueChanged += (_, _) => Invalidate();
        Controls.Add(_scrollBar);
    }

    // Mesajın sağda mı solda mı çizileceğini belirlemek için oturumdaki kullanıcı adı
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentUser { get; set; } = string.Empty;

    [Category("Görünüm")]
    [DefaultValue("")]
    public string EmptyText { get; set; } = string.Empty;

    public int MessageCount => _items.Count;

    // Yeni mesaj ekler; kullanıcı en alttaysa veya mesaj kendisininse en alta kaydırır
    public void AddMessage(ChatMessage message)
    {
        bool stickToBottom = IsAtBottom;
        var item = new ChatItem(message, KindOf(message));
        LayoutItem(item, LayoutWidth);

        item.Top = _items.Count == 0 ? Pad : _items[^1].Top + _items[^1].Height + ItemSpacing;
        _items.Add(item);
        _contentHeight = item.Top + item.Height + Pad;

        bool own = item.Kind is BubbleKind.Own or BubbleKind.OwnPrivate;
        UpdateScrollBar(stickToBottom || own);
        Invalidate();
    }

    public void ClearMessages()
    {
        _items.Clear();
        _contentHeight = 0;
        UpdateScrollBar(true);
        Invalidate();
    }

    private BubbleKind KindOf(ChatMessage m)
    {
        bool mine = string.Equals(m.Sender, CurrentUser, StringComparison.OrdinalIgnoreCase);
        return m.Type switch
        {
            MessageType.Chat => mine ? BubbleKind.Own : BubbleKind.Other,
            MessageType.Private => mine ? BubbleKind.OwnPrivate : BubbleKind.OtherPrivate,
            MessageType.System when m.IsAnnouncement => BubbleKind.Announcement,
            _ => BubbleKind.System
        };
    }

    // Kaydırma çubuğu görünse de görünmese de aynı genişlik kullanılır (yerleşim zıplamasın)
    private int LayoutWidth => Math.Max(200, ClientSize.Width - SystemInformation.VerticalScrollBarWidth);

    // ---------- Yerleşim hesapları ----------

    private void LayoutItem(ChatItem item, int width)
    {
        var m = item.Message;
        item.Time = m.Timestamp.ToString("HH:mm");

        switch (item.Kind)
        {
            case BubbleKind.System:
                LayoutSystem(item, width);
                break;
            case BubbleKind.Announcement:
                LayoutAnnouncement(item, width);
                break;
            default:
                LayoutBubble(item, width);
                break;
        }
    }

    // Ortada gri hap: "Ali sohbete katıldı · 14:03"
    private void LayoutSystem(ChatItem item, int width)
    {
        item.Text = $"{item.Message.Content}   ·   {item.Time}";
        int maxText = width - 2 * Pad - 60;
        var size = TextRenderer.MeasureText(item.Text, _systemFont, new Size(maxText, int.MaxValue), WrapFlags);

        int w = size.Width + 28;
        int h = size.Height + 10;
        item.Bubble = new Rectangle((width - w) / 2, 0, w, h);
        item.TextRect = new Rectangle(item.Bubble.X + 14, 5, size.Width + 1, size.Height);
        item.Height = h;
    }

    // Ortada kırmızımsı kutu: sunucu duyurusu
    private void LayoutAnnouncement(ChatItem item, int width)
    {
        item.Header = "SUNUCU DUYURUSU";
        item.Text = item.Message.Content;
        int maxText = (int)(width * 0.7) - 2 * BubblePadX;

        var header = TextRenderer.MeasureText(item.Header + "  ·  " + item.Time, _headerFont, Size.Empty, LineFlags);
        var text = TextRenderer.MeasureText(item.Text, _textFont, new Size(maxText, int.MaxValue), WrapFlags);
        int inner = Math.Min(maxText, Math.Max(header.Width, text.Width));

        int w = inner + 2 * BubblePadX;
        int h = BubblePadY + header.Height + 4 + text.Height + BubblePadY;
        item.Bubble = new Rectangle((width - w) / 2, 0, w, h);
        item.HeaderRect = new Rectangle(item.Bubble.X + BubblePadX, BubblePadY, inner, header.Height);
        item.TextRect = new Rectangle(item.Bubble.X + BubblePadX, item.HeaderRect.Bottom + 4, inner, text.Height);
        item.Height = h;
    }

    // Kullanıcı mesajı baloncuğu: başlık (gönderen / özel bilgisi), metin ve saat
    private void LayoutBubble(ChatItem item, int width)
    {
        var m = item.Message;
        bool own = item.Kind is BubbleKind.Own or BubbleKind.OwnPrivate;

        item.Header = item.Kind switch
        {
            BubbleKind.Other => m.Sender,
            BubbleKind.OtherPrivate => $"{m.Sender}  ·  size özel",
            BubbleKind.OwnPrivate => $"Özel  →  {m.Recipient}",
            _ => null
        };
        item.Text = m.Content;

        int maxBubble = (int)((width - 2 * Pad - AvatarSize - AvatarGap) * 0.7);
        int maxText = Math.Max(80, maxBubble - 2 * BubblePadX);

        var text = TextRenderer.MeasureText(item.Text, _textFont, new Size(maxText, int.MaxValue), WrapFlags);
        var time = TextRenderer.MeasureText(item.Time, _timeFont, Size.Empty, LineFlags);
        var header = item.Header is null ? Size.Empty : TextRenderer.MeasureText(item.Header, _headerFont, Size.Empty, LineFlags);

        int inner = Math.Min(maxText, Math.Max(text.Width, Math.Max(header.Width, time.Width)));
        int headerHeight = item.Header is null ? 0 : header.Height + 3;
        int h = BubblePadY + headerHeight + text.Height + 3 + time.Height + BubblePadY - 2;
        int w = inner + 2 * BubblePadX;

        int x = own ? width - Pad - w : Pad + AvatarSize + AvatarGap;
        item.Bubble = new Rectangle(x, 0, w, h);
        item.HeaderRect = new Rectangle(x + BubblePadX, BubblePadY, inner, header.Height);
        item.TextRect = new Rectangle(x + BubblePadX, BubblePadY + headerHeight, inner, text.Height);
        item.TimeRect = new Rectangle(x + BubblePadX, item.TextRect.Bottom + 3, inner, time.Height);
        // Avatar baloncuğun alt hizasında, yalnızca başkalarının mesajlarında
        item.Avatar = own ? Rectangle.Empty : new Rectangle(Pad, h - AvatarSize, AvatarSize, AvatarSize);
        item.Height = h;
    }

    private void RelayoutAll()
    {
        int width = LayoutWidth;
        int top = Pad;
        foreach (var item in _items)
        {
            LayoutItem(item, width);
            item.Top = top;
            top += item.Height + ItemSpacing;
        }

        _contentHeight = _items.Count == 0 ? 0 : top - ItemSpacing + Pad;
    }

    // ---------- Kaydırma ----------

    private int MaxScroll => Math.Max(0, _scrollBar.Maximum - _scrollBar.LargeChange + 1);

    private bool IsAtBottom => !_scrollBar.Visible || _scrollBar.Value >= MaxScroll - 4;

    private void UpdateScrollBar(bool scrollToBottom)
    {
        int viewHeight = ClientSize.Height;
        if (_contentHeight <= viewHeight || viewHeight <= 0)
        {
            _scrollBar.Visible = false;
            _scrollBar.Value = 0;
            return;
        }

        int keep = _scrollBar.Value;
        _scrollBar.Minimum = 0;
        _scrollBar.Maximum = _contentHeight - 1;
        _scrollBar.LargeChange = viewHeight;
        _scrollBar.SmallChange = 40;
        _scrollBar.Visible = true;
        _scrollBar.Value = scrollToBottom ? MaxScroll : Math.Min(keep, MaxScroll);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (!_scrollBar.Visible)
            return;

        int value = _scrollBar.Value - e.Delta / 120 * 60;
        _scrollBar.Value = Math.Clamp(value, 0, MaxScroll);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Focus(); // tekerlek ve klavye ile kaydırma için odak alınır
        base.OnMouseDown(e);
    }

    protected override bool IsInputKey(Keys keyData) =>
        keyData is Keys.Up or Keys.Down or Keys.PageUp or Keys.PageDown || base.IsInputKey(keyData);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (!_scrollBar.Visible)
            return;

        int delta = e.KeyCode switch
        {
            Keys.Up => -40,
            Keys.Down => 40,
            Keys.PageUp => -ClientSize.Height,
            Keys.PageDown => ClientSize.Height,
            Keys.Home => -_contentHeight,
            Keys.End => _contentHeight,
            _ => 0
        };
        _scrollBar.Value = Math.Clamp(_scrollBar.Value + delta, 0, MaxScroll);
    }

    protected override void OnResize(EventArgs e)
    {
        bool atBottom = IsAtBottom;
        base.OnResize(e);
        RelayoutAll();
        UpdateScrollBar(atBottom);
        Invalidate();
    }

    // ---------- Çizim ----------

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(BackColor);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_items.Count == 0)
        {
            var area = new Rectangle(0, 0, LayoutWidth, ClientSize.Height);
            TextRenderer.DrawText(g, EmptyText, _systemFont, area, Theme.TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            return;
        }

        int offset = _scrollBar.Visible ? _scrollBar.Value : 0;
        foreach (var item in _items)
        {
            int y = item.Top - offset;
            if (y + item.Height < e.ClipRectangle.Top || y > e.ClipRectangle.Bottom)
                continue;
            DrawItem(g, item, y);
        }
    }

    private void DrawItem(Graphics g, ChatItem item, int y)
    {
        var bubble = Shift(item.Bubble, y);

        switch (item.Kind)
        {
            case BubbleKind.System:
                FillShape(g, bubble, bubble.Height / 2, Color.FromArgb(226, 232, 240), Color.Empty);
                TextRenderer.DrawText(g, item.Text, _systemFont, Shift(item.TextRect, y), Theme.TextMuted, WrapFlags);
                return;

            case BubbleKind.Announcement:
                FillShape(g, bubble, 10, Theme.AnnouncementBack, Color.FromArgb(252, 165, 165));
                TextRenderer.DrawText(g, item.Header + "  ·  " + item.Time, _headerFont, Shift(item.HeaderRect, y), Theme.Danger, LineFlags);
                TextRenderer.DrawText(g, item.Text, _textFont, Shift(item.TextRect, y), Theme.AnnouncementText, WrapFlags);
                return;
        }

        // Renkler: baloncuk zemini, kenar, başlık, metin, saat
        var (back, border, headerColor, textColor, timeColor) = item.Kind switch
        {
            BubbleKind.Own => (Theme.OwnBubble, Color.Empty, Color.White, Color.White, Color.FromArgb(199, 210, 254)),
            BubbleKind.OwnPrivate => (Theme.PrivateOwnBubble, Color.Empty, Color.FromArgb(254, 243, 199), Color.White, Color.FromArgb(254, 215, 170)),
            BubbleKind.OtherPrivate => (Theme.PrivateOtherBubble, Theme.PrivateBorder, Theme.PrivateText, Theme.Text, Theme.PrivateText),
            _ => (Theme.OtherBubble, Theme.Border, Theme.ColorForUser(item.Message.Sender), Theme.Text, Theme.TextMuted)
        };

        bool own = item.Kind is BubbleKind.Own or BubbleKind.OwnPrivate;
        // Gönderen tarafındaki alt köşe daha az yuvarlak: konuşma balonu görünümü
        using (var path = BubblePath(bubble, own))
        {
            using var brush = new SolidBrush(back);
            g.FillPath(brush, path);
            if (border != Color.Empty)
            {
                using var pen = new Pen(border);
                g.DrawPath(pen, path);
            }
        }

        if (!item.Avatar.IsEmpty)
            Theme.DrawAvatar(g, Shift(item.Avatar, y), item.Message.Sender, _avatarFont);

        if (item.Header is not null)
            TextRenderer.DrawText(g, item.Header, _headerFont, Shift(item.HeaderRect, y), headerColor, LineFlags);

        TextRenderer.DrawText(g, item.Text, _textFont, Shift(item.TextRect, y), textColor, WrapFlags);
        TextRenderer.DrawText(g, item.Time, _timeFont, Shift(item.TimeRect, y), timeColor, LineFlags | TextFormatFlags.Right);
    }

    private static Rectangle Shift(Rectangle r, int y) => new(r.X, r.Y + y, r.Width, r.Height);

    private static void FillShape(Graphics g, Rectangle r, int radius, Color back, Color border)
    {
        using var path = Theme.RoundedRect(r, radius);
        using var brush = new SolidBrush(back);
        g.FillPath(brush, path);
        if (border != Color.Empty)
        {
            using var pen = new Pen(border);
            g.DrawPath(pen, path);
        }
    }

    // Köşe yarıçapları farklı yuvarlak dikdörtgen (kendi mesajında sağ alt, diğerlerinde sol alt köşe sivri)
    private static GraphicsPath BubblePath(Rectangle r, bool own)
    {
        int big = Math.Min(Radius * 2, Math.Min(r.Width, r.Height));
        int small = Math.Min(8, big);
        int tl = big, tr = big, br = own ? small : big, bl = own ? big : small;

        var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, tl, tl, 180, 90);
        path.AddArc(r.Right - tr, r.Y, tr, tr, 270, 90);
        path.AddArc(r.Right - br, r.Bottom - br, br, br, 0, 90);
        path.AddArc(r.X, r.Bottom - bl, bl, bl, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _textFont.Dispose();
            _headerFont.Dispose();
            _timeFont.Dispose();
            _systemFont.Dispose();
            _avatarFont.Dispose();
        }

        base.Dispose(disposing);
    }

    // Baloncuk türleri
    private enum BubbleKind
    {
        Own,
        Other,
        OwnPrivate,
        OtherPrivate,
        System,
        Announcement
    }

    // Tek bir mesajın çizim bilgileri (koordinatlar öğenin üst kenarına göredir)
    private sealed class ChatItem
    {
        public ChatItem(ChatMessage message, BubbleKind kind)
        {
            Message = message;
            Kind = kind;
        }

        public ChatMessage Message { get; }
        public BubbleKind Kind { get; }
        public int Top { get; set; }
        public int Height { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? Header { get; set; }
        public string Time { get; set; } = string.Empty;
        public Rectangle Bubble { get; set; }
        public Rectangle HeaderRect { get; set; }
        public Rectangle TextRect { get; set; }
        public Rectangle TimeRect { get; set; }
        public Rectangle Avatar { get; set; }
    }
}
