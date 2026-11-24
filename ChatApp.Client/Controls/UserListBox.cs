using System.ComponentModel;
using System.Drawing.Drawing2D;
using ChatApp.Shared.UI;

namespace ChatApp.Client.Controls;

// Çevrimiçi kullanıcılar listesi: avatar, ad, "sen" etiketi ve özel mesaj hedefi vurgusu
public class UserListBox : ListBox
{
    private readonly Font _nameFont = new("Segoe UI Semibold", 10F, FontStyle.Bold);
    private readonly Font _hintFont = new("Segoe UI", 8.5F);
    private readonly Font _avatarFont = new("Segoe UI Semibold", 10F, FontStyle.Bold);
    private int _hoverIndex = -1;

    public UserListBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 44;
        BorderStyle = BorderStyle.None;
        IntegralHeight = false;
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }

    // Oturumdaki kullanıcı ("sen" etiketi için)
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentUser { get; set; } = string.Empty;

    // Özel mesaj modundaki hedef kullanıcı (satırı turuncu vurgulanır)
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? PrivateTarget { get; set; }

    // Seçili satırdaki kullanıcı adı
    [Browsable(false)]
    public string? SelectedUser => SelectedItem as string;

    // Listeyi yeni kullanıcılarla değiştirir; kendi adı en üstte, diğerleri alfabetik
    public void SetUsers(IEnumerable<string> users)
    {
        string? selected = SelectedUser;
        var ordered = users
            .OrderBy(u => string.Equals(u, CurrentUser, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
            .ThenBy(u => u, StringComparer.CurrentCultureIgnoreCase)
            .ToArray();

        BeginUpdate();
        Items.Clear();
        Items.AddRange(ordered);
        if (selected is not null && Items.Contains(selected))
            SelectedItem = selected;
        EndUpdate();
    }

    public bool IsSelf(string user) => string.Equals(user, CurrentUser, StringComparison.OrdinalIgnoreCase);

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        var g = e.Graphics;
        var bounds = e.Bounds;
        using (var back = new SolidBrush(BackColor))
            g.FillRectangle(back, bounds);

        if (e.Index < 0 || e.Index >= Items.Count)
            return;

        string user = (string)Items[e.Index];
        bool self = IsSelf(user);
        bool isTarget = string.Equals(user, PrivateTarget, StringComparison.OrdinalIgnoreCase);
        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

        // Satır zemini: özel hedef > seçili > üzerinde
        Color rowColor = isTarget ? Theme.PrivateOtherBubble
                       : selected ? Theme.PrimaryLight
                       : e.Index == _hoverIndex ? Theme.SurfaceAlt
                       : Color.Empty;
        if (rowColor != Color.Empty)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var row = Rectangle.Inflate(bounds, -4, -2);
            using var path = Theme.RoundedRect(row, 8);
            using var brush = new SolidBrush(rowColor);
            g.FillPath(brush, path);
        }

        // Avatar ve çevrimiçi noktası
        var avatar = new Rectangle(bounds.X + 12, bounds.Y + (bounds.Height - 30) / 2, 30, 30);
        Theme.DrawAvatar(g, avatar, user, _avatarFont);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var dot = new Rectangle(avatar.Right - 9, avatar.Bottom - 9, 10, 10);
        using (var ring = new SolidBrush(BackColor))
            g.FillEllipse(ring, Rectangle.Inflate(dot, 2, 2));
        using (var green = new SolidBrush(Theme.Success))
            g.FillEllipse(green, dot);

        // Ad ve alt bilgi
        int textX = avatar.Right + 10;
        var nameRect = new Rectangle(textX, bounds.Y + 5, bounds.Right - textX - 8, 20);
        var hintRect = new Rectangle(textX, bounds.Y + 23, bounds.Right - textX - 8, 16);
        TextRenderer.DrawText(g, user, _nameFont, nameRect, Theme.Text,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

        string hint = self ? "sen" : isTarget ? "özel mesaj hedefi" : "çevrimiçi";
        Color hintColor = self ? Theme.Primary : isTarget ? Theme.PrivateText : Theme.TextMuted;
        TextRenderer.DrawText(g, hint, _hintFont, hintRect, hintColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    // Fare üzerindeki satır hafifçe vurgulanır
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int index = IndexFromPoint(e.Location);
        if (index != _hoverIndex)
        {
            _hoverIndex = index;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoverIndex = -1;
        Invalidate();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _nameFont.Dispose();
            _hintFont.Dispose();
            _avatarFont.Dispose();
        }

        base.Dispose(disposing);
    }
}
