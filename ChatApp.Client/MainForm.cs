using ChatApp.Client.Network;
using ChatApp.Client.Services;
using ChatApp.Shared;
using ChatApp.Shared.Protocol;
using ChatApp.Shared.UI;

namespace ChatApp.Client;

// İstemci penceresi: yalnızca arayüz işleri yapar, ağ işlemleri ChatClient sınıfındadır
public partial class MainForm : Form
{
    private enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected
    }

    private static readonly Color PrivateInputBack = Color.FromArgb(255, 251, 235);

    private readonly ChatClient _client = new();
    private readonly HistoryService _history = new();
    private ConnectionState _state = ConnectionState.Disconnected;
    private CancellationTokenSource? _connectCts;
    private string? _privateTarget;   // özel mesaj modundaysa hedef kullanıcı
    private bool _closing;
    private bool _closingAfterDisconnect;

    public MainForm()
    {
        InitializeComponent();
        txtHost.Text = ChatConstants.DefaultHost;
        numPort.Value = ChatConstants.DefaultPort;
        txtUserName.MaxLength = ChatConstants.MaxUserNameLength;
        txtMessage.MaxLength = ChatConstants.MaxMessageLength;

        // İstemci olayları arka plan iş parçacığından gelir; arayüze Invoke ile aktarılır
        _client.MessageReceived += (_, m) => RunOnUi(() => ShowMessage(m));
        _client.UserListChanged += (_, users) => RunOnUi(() => UpdateUsers(users));
        _client.Disconnected += (_, e) => RunOnUi(() => OnDisconnected(e));

        UpdateUiState();
    }

    private void MainForm_Shown(object? sender, EventArgs e) => txtUserName.Focus();

    // ---------- Bağlanma / bağlantıyı kesme ----------

    private async void btnConnect_Click(object? sender, EventArgs e) => await ConnectAsync();

    private async void txtUserName_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        await ConnectAsync();
    }

    private async Task ConnectAsync()
    {
        if (_state != ConnectionState.Disconnected)
            return;

        string host = txtHost.Text.Trim();
        string userName = txtUserName.Text.Trim();

        // Sunucuya gitmeden önce yerel kontroller (anında geri bildirim)
        if (host.Length == 0)
        {
            ShowWarning("Sunucu IP adresini girin (aynı bilgisayar için 127.0.0.1).", "Eksik bilgi");
            txtHost.Focus();
            return;
        }

        string? nameError = UserNameValidator.Validate(userName);
        if (nameError is not null)
        {
            ShowWarning(nameError, "Geçersiz kullanıcı adı");
            txtUserName.Focus();
            txtUserName.SelectAll();
            return;
        }

        int port = (int)numPort.Value;
        chatView.CurrentUser = userName;
        lstUsers.CurrentUser = userName;
        SetState(ConnectionState.Connecting);
        SetStatusMessage($"{host}:{port} adresine bağlanılıyor…");

        _connectCts = new CancellationTokenSource();
        try
        {
            await _client.ConnectAsync(host, port, userName, _connectCts.Token);
            SetState(ConnectionState.Connected);
            SetStatusMessage($"{host}:{port} sunucusuna \"{userName}\" olarak bağlandınız.");
            txtMessage.Focus();
        }
        catch (ChatClientException ex)
        {
            SetState(ConnectionState.Disconnected);
            SetStatusMessage(ex.Title + ": " + ex.Message.Split('\n')[0]);
            MessageBox.Show(this, ex.Message, ex.Title, MessageBoxButtons.OK,
                ex.IsJoinRejected ? MessageBoxIcon.Warning : MessageBoxIcon.Error);

            if (ex.IsJoinRejected)
            {
                txtUserName.Focus();
                txtUserName.SelectAll();
            }
        }
        catch (OperationCanceledException)
        {
            SetState(ConnectionState.Disconnected);
            SetStatusMessage("Bağlanma işlemi iptal edildi.");
        }
        finally
        {
            _connectCts.Dispose();
            _connectCts = null;
        }
    }

    private async void btnDisconnect_Click(object? sender, EventArgs e)
    {
        if (_state == ConnectionState.Connecting)
        {
            _connectCts?.Cancel();
            return;
        }

        btnDisconnect.Enabled = false;
        await _client.DisconnectAsync();
    }

    // Bağlantı sona erdi: arayüz "bağlı değil" durumuna döner, beklenmedik kopmada kullanıcı uyarılır
    private void OnDisconnected(DisconnectedEventArgs e)
    {
        SetState(ConnectionState.Disconnected);
        ExitPrivateMode();
        lstUsers.Items.Clear();
        lblUsersTitle.Text = "Çevrimiçi (0)";

        ShowMessage(ChatMessage.System(e.ByUser ? "Sohbetten ayrıldınız." : "Bağlantı kesildi: " + e.Reason));
        SetStatusMessage(e.ByUser ? "Bağlantı kesildi. Tekrar bağlanmak için \"Bağlan\"a tıklayın." : "Bağlantı koptu: " + e.Reason);

        if (!e.ByUser && !_closing)
        {
            MessageBox.Show(this, e.Reason + "\n\nYeniden bağlanmak için \"Bağlan\" butonunu kullanabilirsiniz.",
                "Bağlantı kesildi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // Pencere kapanırken sunucuya ayrılma bildirimi gönderilir
    private async void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _closing = true;
        if (_state == ConnectionState.Connecting)
            _connectCts?.Cancel();

        if (_closingAfterDisconnect || _state != ConnectionState.Connected)
            return;

        e.Cancel = true;
        Enabled = false;
        await _client.DisconnectAsync();
        _closingAfterDisconnect = true;
        Close();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _client.Dispose();
        base.OnFormClosed(e);
    }

    // ---------- Mesaj gönderme ----------

    private async void btnSend_Click(object? sender, EventArgs e) => await SendMessageAsync();

    private async void txtMessage_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape && _privateTarget is not null)
        {
            e.SuppressKeyPress = true;
            ExitPrivateMode();
            return;
        }

        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true; // "ding" sesi çıkmasın
        await SendMessageAsync();
    }

    private async Task SendMessageAsync()
    {
        string text = txtMessage.Text.Trim();
        if (text.Length == 0 || _state != ConnectionState.Connected)
            return;

        try
        {
            if (_privateTarget is not null)
                await _client.SendPrivateAsync(_privateTarget, text);
            else
                await _client.SendChatAsync(text);

            txtMessage.Clear();
        }
        catch (ChatClientException ex)
        {
            // Bağlantı koptuysa ayrıca Disconnected olayı da arayüzü günceller
            SetStatusMessage(ex.Title + ": " + ex.Message);
        }

        if (txtMessage.Enabled)
            txtMessage.Focus();
    }

    private void txtMessage_TextChanged(object? sender, EventArgs e) =>
        btnSend.Enabled = _state == ConnectionState.Connected && txtMessage.Text.Trim().Length > 0;

    // Gelen (ve sunucudan geri dönen kendi) mesajları ekrana ve geçmişe ekler
    private void ShowMessage(ChatMessage message)
    {
        chatView.AddMessage(message);
        _history.Add(message);
        btnSaveHistory.Enabled = btnClearChat.Enabled = true;
    }

    // ---------- Çevrimiçi kullanıcılar ve özel mesaj ----------

    private void UpdateUsers(IReadOnlyList<string> users)
    {
        lstUsers.SetUsers(users);
        lblUsersTitle.Text = $"Çevrimiçi ({users.Count})";

        // Özel mesaj hedefi ayrıldıysa mod kapatılır
        if (_privateTarget is not null &&
            !users.Any(u => string.Equals(u, _privateTarget, StringComparison.OrdinalIgnoreCase)))
        {
            SetStatusMessage($"{_privateTarget} sohbetten ayrıldığı için özel mesaj modu kapatıldı.");
            ExitPrivateMode();
        }
    }

    private void lstUsers_DoubleClick(object? sender, EventArgs e) => EnterPrivateMode(lstUsers.SelectedUser);

    private void lstUsers_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        EnterPrivateMode(lstUsers.SelectedUser);
    }

    private void EnterPrivateMode(string? user)
    {
        if (user is null || _state != ConnectionState.Connected)
            return;

        if (lstUsers.IsSelf(user))
        {
            SetStatusMessage("Kendinize özel mesaj gönderemezsiniz; listeden başka bir kullanıcı seçin.");
            return;
        }

        _privateTarget = user;
        lstUsers.PrivateTarget = user;
        lstUsers.Invalidate();

        lblPrivateMode.Text = $"@{TurkishText.Dative(user)} özel mesaj yazıyorsunuz – mesajı yalnızca {user} görecek.  (Esc: herkese yaz)";
        pnlPrivateBanner.Visible = true;
        pnlMessageBox.BackColor = txtMessage.BackColor = PrivateInputBack;
        btnSend.Text = "Özel Gönder";
        UpdatePlaceholder();
        txtMessage.Focus();
    }

    private void ExitPrivateMode()
    {
        _privateTarget = null;
        lstUsers.PrivateTarget = null;
        lstUsers.Invalidate();

        pnlPrivateBanner.Visible = false;
        pnlMessageBox.BackColor = txtMessage.BackColor = Color.White;
        pnlMessageBox.Invalidate();
        btnSend.Text = "Gönder";
        UpdatePlaceholder();
    }

    private void btnCancelPrivate_Click(object? sender, EventArgs e)
    {
        ExitPrivateMode();
        txtMessage.Focus();
    }

    // ---------- Geçmiş ----------

    private void btnSaveHistory_Click(object? sender, EventArgs e)
    {
        if (_history.Count == 0)
            return;

        using var dialog = new SaveFileDialog
        {
            Title = "Sohbet geçmişini kaydet",
            Filter = "Metin dosyası (*.txt)|*.txt",
            DefaultExt = "txt",
            FileName = HistoryService.SuggestFileName()
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            string user = _client.UserName.Length > 0 ? _client.UserName : txtUserName.Text.Trim();
            _history.SaveToFile(dialog.FileName, user, _client.ServerAddress);
            SetStatusMessage($"Sohbet geçmişi kaydedildi ({_history.Count} mesaj): {dialog.FileName}");
        }
        catch (IOException ex)
        {
            MessageBox.Show(this, ex.Message, "Kaydetme hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClearChat_Click(object? sender, EventArgs e)
    {
        var answer = MessageBox.Show(this,
            "Ekrandaki mesajlar temizlensin mi?\nKaydedilmemiş sohbet geçmişi de silinir.",
            "Sohbeti temizle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes)
            return;

        chatView.ClearMessages();
        _history.Clear();
        btnSaveHistory.Enabled = btnClearChat.Enabled = false;
    }

    // ---------- Arayüz durumu ----------

    private void SetState(ConnectionState state)
    {
        _state = state;
        UpdateUiState();
    }

    private void UpdateUiState()
    {
        bool disconnected = _state == ConnectionState.Disconnected;
        bool connected = _state == ConnectionState.Connected;

        txtHost.Enabled = numPort.Enabled = txtUserName.Enabled = disconnected;
        btnConnect.Enabled = disconnected;
        btnDisconnect.Enabled = !disconnected;
        btnDisconnect.Text = _state == ConnectionState.Connecting ? "İptal" : "Bağlantıyı Kes";

        // Bağlı değilken mesaj yazma ve gönderme pasif
        txtMessage.Enabled = connected;
        btnSend.Enabled = connected && txtMessage.Text.Trim().Length > 0;
        lstUsers.Enabled = connected;
        btnSaveHistory.Enabled = btnClearChat.Enabled = _history.Count > 0;
        UpdatePlaceholder();

        switch (_state)
        {
            case ConnectionState.Connected:
                statusBadge.SetStatus($"Bağlı – {_client.UserName}", Theme.Success);
                Text = $"Chat İstemcisi – {_client.UserName} ({_client.ServerAddress})";
                break;
            case ConnectionState.Connecting:
                statusBadge.SetStatus("Bağlanıyor…", Theme.Warning, blinking: true);
                Text = "Chat İstemcisi – Bağlanıyor…";
                break;
            default:
                statusBadge.SetStatus("Bağlı değil", Theme.Danger);
                Text = "Chat İstemcisi – Bağlı değil";
                break;
        }
    }

    private void UpdatePlaceholder()
    {
        txtMessage.PlaceholderText = _state != ConnectionState.Connected
            ? "Mesaj göndermek için önce sunucuya bağlanın"
            : _privateTarget is not null
                ? $"@{TurkishText.Dative(_privateTarget)} özel mesaj yazın ve Enter'a basın…"
                : "Herkese mesaj yazın ve Enter'a basın…";
    }

    private void SetStatusMessage(string text) => lblStatusMessage.Text = text;

    private void ShowWarning(string text, string title)
    {
        SetStatusMessage(text);
        MessageBox.Show(this, text, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

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
