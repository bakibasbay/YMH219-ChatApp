using ChatApp.Server.Network;
using ChatApp.Shared.UI;

namespace ChatApp.Server;

// Sunucu penceresi: yalnızca arayüz işleri yapar, ağ işlemleri ChatServer sınıfındadır
public partial class MainForm : Form
{
    private readonly ChatServer _server = new();
    private readonly Label _lblEmptyClients = new();
    private bool _closingAfterStop;

    public MainForm()
    {
        InitializeComponent();

        // İstemci yokken listenin ortasında gösterilen bilgi yazısı
        _lblEmptyClients.ForeColor = Theme.TextMuted;
        _lblEmptyClients.BackColor = Color.White;
        _lblEmptyClients.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
        _lblEmptyClients.TextAlign = ContentAlignment.MiddleCenter;
        lvClients.Controls.Add(_lblEmptyClients);

        // Sunucu olayları arka plan iş parçacığından gelir; arayüze Invoke ile aktarılır
        _server.Log += (_, e) => RunOnUi(() => AppendLog(e));
        _server.ClientJoined += (_, c) => RunOnUi(() => AddClientRow(c));
        _server.ClientLeft += (_, c) => RunOnUi(() => RemoveClientRow(c));
        _server.ClientUpdated += (_, c) => RunOnUi(() => UpdateClientRow(c));

        UpdateUiState();
        ResizeColumns();
    }

    // ---------- Sunucuyu başlatma / durdurma ----------

    private void btnStart_Click(object? sender, EventArgs e)
    {
        try
        {
            _server.Start((int)numPort.Value);
        }
        catch (InvalidOperationException ex)
        {
            AppendLog(new ServerLogEventArgs(ServerLogKind.Error, ex.Message.Replace('\n', ' ')));
            MessageBox.Show(this, ex.Message, "Sunucu başlatılamadı", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        UpdateUiState();
    }

    private async void btnStop_Click(object? sender, EventArgs e) => await StopServerAsync();

    private async Task StopServerAsync()
    {
        btnStop.Enabled = false;
        statusBadge.SetStatus("Durduruluyor…", Theme.Warning, blinking: true);
        try
        {
            await _server.StopAsync();
        }
        catch (Exception ex)
        {
            AppendLog(new ServerLogEventArgs(ServerLogKind.Error, "Durdurma sırasında hata: " + ex.Message));
        }

        lvClients.Items.Clear();
        UpdateUiState();
    }

    // Pencere kapanırken sunucu düzgünce durdurulur (istemcilere bildirim gider)
    private async void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_closingAfterStop || !_server.IsRunning)
            return;

        e.Cancel = true;
        Enabled = false;
        await StopServerAsync();
        _closingAfterStop = true;
        Close();
    }

    // ---------- Duyuru ----------

    private async void btnAnnounce_Click(object? sender, EventArgs e) => await SendAnnouncementAsync();

    private async void txtAnnouncement_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        await SendAnnouncementAsync();
    }

    private async Task SendAnnouncementAsync()
    {
        string text = txtAnnouncement.Text.Trim();
        if (text.Length == 0 || !_server.IsRunning)
            return;

        txtAnnouncement.Clear();
        await _server.AnnounceAsync(text);
    }

    private void txtAnnouncement_TextChanged(object? sender, EventArgs e) => UpdateUiState();

    // ---------- Arayüz durumu ----------

    private void UpdateUiState()
    {
        bool running = _server.IsRunning;

        btnStart.Enabled = !running;
        btnStop.Enabled = running;
        numPort.Enabled = !running;
        txtAnnouncement.Enabled = running;
        btnAnnounce.Enabled = running && txtAnnouncement.Text.Trim().Length > 0;

        if (running)
        {
            statusBadge.SetStatus($"Çalışıyor – port {_server.Port}", Theme.Success);
            lblStatusMessage.Text = $"Sunucu {_server.Port} numaralı portu dinliyor. İstemciler bu bilgisayarın IP adresi ve port ile bağlanabilir.";
            Text = $"Chat Sunucusu – Çalışıyor (port {_server.Port})";
        }
        else
        {
            statusBadge.SetStatus("Durdu", Theme.Danger);
            lblStatusMessage.Text = "Sunucu durduruldu. Başlatmak için bir port seçip \"Sunucuyu Başlat\"a tıklayın.";
            Text = "Chat Sunucusu – Durdu";
        }

        UpdateClientCount();
    }

    private void UpdateClientCount()
    {
        int count = lvClients.Items.Count;
        lblClientCount.Text = $"Bağlı istemci: {count}";
        _lblEmptyClients.Visible = count == 0;
        _lblEmptyClients.Text = _server.IsRunning ? "Henüz bağlı istemci yok" : "Sunucu çalışmıyor";
    }

    // ---------- Bağlı istemciler listesi ----------

    private void AddClientRow(ClientConnection client)
    {
        string key = client.Id.ToString();
        if (lvClients.Items.ContainsKey(key))
            return;

        var item = new ListViewItem(client.UserName) { Name = key, Tag = client };
        item.SubItems.Add(client.RemoteEndPoint);
        item.SubItems.Add(client.ConnectedAt.ToString("dd.MM.yyyy HH:mm:ss"));
        item.SubItems.Add(client.MessageCount.ToString());
        lvClients.Items.Add(item);
        UpdateClientCount();
    }

    private void RemoveClientRow(ClientConnection client)
    {
        lvClients.Items.RemoveByKey(client.Id.ToString());
        UpdateClientCount();
    }

    private void UpdateClientRow(ClientConnection client)
    {
        var items = lvClients.Items.Find(client.Id.ToString(), false);
        if (items.Length > 0)
            items[0].SubItems[3].Text = client.MessageCount.ToString();
    }

    // Sütunlar liste genişliğine göre orantılı dağıtılır
    private void ResizeColumns()
    {
        int width = lvClients.ClientSize.Width;
        if (width <= 0)
            return;

        colEndPoint.Width = (int)(width * 0.27);
        colConnectedAt.Width = (int)(width * 0.31);
        colMessageCount.Width = (int)(width * 0.16);
        colUserName.Width = width - colEndPoint.Width - colConnectedAt.Width - colMessageCount.Width;

        _lblEmptyClients.Bounds = new Rectangle(0, 40, width, Math.Max(40, lvClients.ClientSize.Height - 40));
    }

    private void lvClients_Resize(object? sender, EventArgs e) => ResizeColumns();

    // Düz, açık renkli sütun başlıkları
    private void lvClients_DrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
    {
        using (var back = new SolidBrush(Theme.SurfaceAlt))
            e.Graphics.FillRectangle(back, e.Bounds);
        using (var line = new Pen(Theme.Border))
            e.Graphics.DrawLine(line, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

        var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                    (e.Header?.TextAlign == HorizontalAlignment.Center ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Left);
        var textBounds = Rectangle.Inflate(e.Bounds, -8, 0);
        using var font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        TextRenderer.DrawText(e.Graphics, e.Header?.Text, font, textBounds, Theme.TextMuted, flags);
    }

    private void lvClients_DrawItem(object? sender, DrawListViewItemEventArgs e)
    {
        // Satır çizimi DrawSubItem içinde yapılır
    }

    // Satırlar: seçili satır vurgulu, ilk sütunda renkli avatar + kullanıcı adı
    private void lvClients_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
    {
        if (e.Item is null || e.SubItem is null)
            return;

        var g = e.Graphics;
        Color back = e.Item.Selected ? Theme.PrimaryLight : Color.White;
        using (var brush = new SolidBrush(back))
            g.FillRectangle(brush, e.Bounds);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

        var textBounds = Rectangle.Inflate(e.Bounds, -8, 0);
        if (e.ColumnIndex == 0)
        {
            int size = 24;
            var avatar = new Rectangle(e.Bounds.X + 8, e.Bounds.Y + (e.Bounds.Height - size) / 2, size, size);
            using var avatarFont = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            Theme.DrawAvatar(g, avatar, e.Item.Text, avatarFont);
            textBounds = new Rectangle(avatar.Right + 8, e.Bounds.Y, e.Bounds.Right - avatar.Right - 12, e.Bounds.Height);
            using var bold = new Font(lvClients.Font, FontStyle.Bold);
            TextRenderer.DrawText(g, e.SubItem.Text, bold, textBounds, Theme.Text,
                TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            return;
        }

        var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                    (e.Header?.TextAlign == HorizontalAlignment.Center ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Left);
        TextRenderer.DrawText(g, e.SubItem.Text, lvClients.Font, textBounds,
            e.ColumnIndex == 3 ? Theme.Primary : Theme.Text, flags);
    }

    // ---------- Olay günlüğü ----------

    private void AppendLog(ServerLogEventArgs entry)
    {
        var (symbol, color) = entry.Kind switch
        {
            ServerLogKind.Connected => ("●", Theme.Success),
            ServerLogKind.Disconnected => ("○", Theme.Warning),
            ServerLogKind.Message => ("›", Theme.Text),
            ServerLogKind.Private => ("◆", Theme.PrivateOwnBubble),
            ServerLogKind.Warning => ("!", Theme.Warning),
            ServerLogKind.Error => ("✖", Theme.Danger),
            _ => ("ℹ", Theme.Primary)
        };

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.SelectionLength = 0;

        rtbLog.SelectionColor = Theme.TextMuted;
        rtbLog.AppendText($"[{entry.Time:HH:mm:ss}]  ");
        rtbLog.SelectionColor = color;
        rtbLog.AppendText(symbol + "  ");
        rtbLog.SelectionColor = entry.Kind == ServerLogKind.Message ? Theme.Text : color;
        rtbLog.AppendText(entry.Text + Environment.NewLine);

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.ScrollToCaret();
    }

    private void btnClearLog_Click(object? sender, EventArgs e) => rtbLog.Clear();

    // Arka plan iş parçacığından gelen işi arayüz iş parçacığında çalıştırır
    private void RunOnUi(Action action)
    {
        if (IsDisposed || !IsHandleCreated)
            return;

        try
        {
            if (InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }
        catch (InvalidOperationException)
        {
            // Pencere kapanırken gelen olaylar yok sayılır
        }
    }
}
