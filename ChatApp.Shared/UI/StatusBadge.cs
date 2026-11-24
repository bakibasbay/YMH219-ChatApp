using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace ChatApp.Shared.UI;

// Bağlantı / sunucu durumunu renkli bir hap (pill) içinde nokta ve yazıyla gösterir.
// Bağlanma gibi ara durumlarda nokta yanıp söner.
public class StatusBadge : Control
{
    private readonly System.Windows.Forms.Timer _blinkTimer;
    private Color _statusColor = Theme.Idle;
    private bool _blinkOn = true;

    public StatusBadge()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.SupportsTransparentBackColor, true);
        _blinkTimer = new System.Windows.Forms.Timer { Interval = 450 };
        _blinkTimer.Tick += (_, _) =>
        {
            _blinkOn = !_blinkOn;
            Invalidate();
        };
    }

    [Category("Görünüm")]
    public Color StatusColor
    {
        get => _statusColor;
        set
        {
            _statusColor = value;
            Invalidate();
        }
    }

    // Ara durum (ör. "Bağlanıyor…") için yanıp sönme
    [Category("Davranış")]
    [DefaultValue(false)]
    public bool Blinking
    {
        get => _blinkTimer.Enabled;
        set
        {
            _blinkOn = true;
            _blinkTimer.Enabled = value;
            Invalidate();
        }
    }

    // Durumu tek çağrıda günceller
    public void SetStatus(string text, Color color, bool blinking = false)
    {
        Text = text;
        _statusColor = color;
        Blinking = blinking;
        AccessibleName = text;
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? BackColor);

        // Zemin rengi koyu başlıkta da açık kartta da okunacak şekilde durum renginin saydam tonu
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var path = Theme.RoundedRect(rect, rect.Height / 2))
        {
            using var fill = new SolidBrush(Color.FromArgb(40, _statusColor));
            using var pen = new Pen(Color.FromArgb(190, _statusColor), 1.5f);
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        int dot = Math.Max(8, Height / 3);
        var dotRect = new Rectangle(12, (Height - dot) / 2, dot, dot);
        using (var halo = new SolidBrush(Color.FromArgb(60, _statusColor)))
            g.FillEllipse(halo, Rectangle.Inflate(dotRect, 3, 3));
        using (var brush = new SolidBrush(_blinkOn ? _statusColor : Color.FromArgb(70, _statusColor)))
            g.FillEllipse(brush, dotRect);

        var textRect = new Rectangle(dotRect.Right + 9, 0, Width - dotRect.Right - 16, Height);
        TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _blinkTimer.Stop();
            _blinkTimer.Dispose();
        }

        base.Dispose(disposing);
    }
}
