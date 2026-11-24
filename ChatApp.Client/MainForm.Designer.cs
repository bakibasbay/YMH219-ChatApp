namespace ChatApp.Client
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.statusBadge = new ChatApp.Shared.UI.StatusBadge();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.cardConnection = new ChatApp.Shared.UI.CardPanel();
            this.btnDisconnect = new ChatApp.Shared.UI.ModernButton();
            this.btnConnect = new ChatApp.Shared.UI.ModernButton();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.cardChat = new ChatApp.Shared.UI.CardPanel();
            this.chatView = new ChatApp.Client.Controls.ChatView();
            this.pnlPrivateBanner = new System.Windows.Forms.Panel();
            this.lblPrivateMode = new System.Windows.Forms.Label();
            this.btnCancelPrivate = new ChatApp.Shared.UI.ModernButton();
            this.pnlCompose = new System.Windows.Forms.Panel();
            this.pnlMessageBox = new ChatApp.Shared.UI.CardPanel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new ChatApp.Shared.UI.ModernButton();
            this.pnlChatHeader = new System.Windows.Forms.Panel();
            this.btnClearChat = new ChatApp.Shared.UI.ModernButton();
            this.btnSaveHistory = new ChatApp.Shared.UI.ModernButton();
            this.lblChatTitle = new System.Windows.Forms.Label();
            this.cardUsers = new ChatApp.Shared.UI.CardPanel();
            this.lstUsers = new ChatApp.Client.Controls.UserListBox();
            this.lblUsersHint = new System.Windows.Forms.Label();
            this.lblUsersTitle = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlHeader.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.cardConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.cardChat.SuspendLayout();
            this.pnlPrivateBanner.SuspendLayout();
            this.pnlCompose.SuspendLayout();
            this.pnlMessageBox.SuspendLayout();
            this.pnlChatHeader.SuspendLayout();
            this.cardUsers.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlHeader.Controls.Add(this.statusBadge);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1104, 76);
            this.pnlHeader.TabIndex = 0;
            //
            // statusBadge
            //
            this.statusBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusBadge.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.statusBadge.ForeColor = System.Drawing.Color.White;
            this.statusBadge.Location = new System.Drawing.Point(824, 20);
            this.statusBadge.Name = "statusBadge";
            this.statusBadge.Size = new System.Drawing.Size(260, 36);
            this.statusBadge.TabIndex = 2;
            this.statusBadge.Text = "Bağlı değil";
            //
            // lblSubtitle
            //
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSubtitle.Location = new System.Drawing.Point(22, 44);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(300, 17);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "YMH 219 · TCP istemci-sunucu sohbet uygulaması";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chat İstemcisi";
            //
            // tlpMain
            //
            this.tlpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.tlpMain.Controls.Add(this.cardConnection, 0, 0);
            this.tlpMain.Controls.Add(this.cardChat, 0, 1);
            this.tlpMain.Controls.Add(this.cardUsers, 1, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 76);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(12);
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(1104, 623);
            this.tlpMain.TabIndex = 1;
            //
            // cardConnection
            //
            this.cardConnection.BackColor = System.Drawing.Color.White;
            this.tlpMain.SetColumnSpan(this.cardConnection, 2);
            this.cardConnection.Controls.Add(this.btnDisconnect);
            this.cardConnection.Controls.Add(this.btnConnect);
            this.cardConnection.Controls.Add(this.txtUserName);
            this.cardConnection.Controls.Add(this.lblUserName);
            this.cardConnection.Controls.Add(this.numPort);
            this.cardConnection.Controls.Add(this.lblPort);
            this.cardConnection.Controls.Add(this.txtHost);
            this.cardConnection.Controls.Add(this.lblHost);
            this.cardConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardConnection.Location = new System.Drawing.Point(16, 16);
            this.cardConnection.Margin = new System.Windows.Forms.Padding(4);
            this.cardConnection.Name = "cardConnection";
            this.cardConnection.Size = new System.Drawing.Size(1072, 72);
            this.cardConnection.TabIndex = 0;
            //
            // btnDisconnect
            //
            this.btnDisconnect.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnDisconnect.Location = new System.Drawing.Point(810, 17);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(150, 38);
            this.btnDisconnect.TabIndex = 7;
            this.btnDisconnect.Text = "Bağlantıyı Kes";
            this.btnDisconnect.Variant = ChatApp.Shared.UI.ButtonVariant.Danger;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            //
            // btnConnect
            //
            this.btnConnect.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnConnect.Location = new System.Drawing.Point(684, 17);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(116, 38);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Bağlan";
            this.btnConnect.Variant = ChatApp.Shared.UI.ButtonVariant.Primary;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            //
            // txtUserName
            //
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUserName.Location = new System.Drawing.Point(500, 21);
            this.txtUserName.MaxLength = 20;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.PlaceholderText = "ör. Ali";
            this.txtUserName.Size = new System.Drawing.Size(164, 29);
            this.txtUserName.TabIndex = 5;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyDown);
            //
            // lblUserName
            //
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblUserName.Location = new System.Drawing.Point(398, 26);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(96, 19);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "Kullanıcı adı";
            //
            // numPort
            //
            this.numPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPort.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numPort.Location = new System.Drawing.Point(296, 21);
            this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(84, 29);
            this.numPort.TabIndex = 3;
            this.numPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            //
            // lblPort
            //
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblPort.Location = new System.Drawing.Point(254, 26);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(37, 19);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            //
            // txtHost
            //
            this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtHost.Location = new System.Drawing.Point(100, 21);
            this.txtHost.MaxLength = 100;
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(140, 29);
            this.txtHost.TabIndex = 1;
            this.txtHost.Text = "127.0.0.1";
            //
            // lblHost
            //
            this.lblHost.AutoSize = true;
            this.lblHost.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblHost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblHost.Location = new System.Drawing.Point(20, 26);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(74, 19);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Sunucu IP";
            //
            // cardChat
            //
            this.cardChat.BackColor = System.Drawing.Color.White;
            this.cardChat.Controls.Add(this.chatView);
            this.cardChat.Controls.Add(this.pnlPrivateBanner);
            this.cardChat.Controls.Add(this.pnlCompose);
            this.cardChat.Controls.Add(this.pnlChatHeader);
            this.cardChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardChat.Location = new System.Drawing.Point(16, 96);
            this.cardChat.Margin = new System.Windows.Forms.Padding(4);
            this.cardChat.Name = "cardChat";
            this.cardChat.Padding = new System.Windows.Forms.Padding(14, 6, 14, 14);
            this.cardChat.Size = new System.Drawing.Size(794, 511);
            this.cardChat.TabIndex = 1;
            //
            // chatView
            //
            this.chatView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.chatView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatView.EmptyText = "Henüz mesaj yok.\nSunucuya bağlanıp sohbete başlayın.";
            this.chatView.Location = new System.Drawing.Point(14, 50);
            this.chatView.Name = "chatView";
            this.chatView.Size = new System.Drawing.Size(766, 357);
            this.chatView.TabIndex = 1;
            //
            // pnlPrivateBanner
            //
            this.pnlPrivateBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(243)))), ((int)(((byte)(199)))));
            this.pnlPrivateBanner.Controls.Add(this.lblPrivateMode);
            this.pnlPrivateBanner.Controls.Add(this.btnCancelPrivate);
            this.pnlPrivateBanner.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPrivateBanner.Location = new System.Drawing.Point(14, 407);
            this.pnlPrivateBanner.Name = "pnlPrivateBanner";
            this.pnlPrivateBanner.Padding = new System.Windows.Forms.Padding(12, 4, 6, 4);
            this.pnlPrivateBanner.Size = new System.Drawing.Size(766, 34);
            this.pnlPrivateBanner.TabIndex = 2;
            this.pnlPrivateBanner.Visible = false;
            //
            // lblPrivateMode
            //
            this.lblPrivateMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPrivateMode.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPrivateMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(64)))), ((int)(((byte)(14)))));
            this.lblPrivateMode.Location = new System.Drawing.Point(12, 4);
            this.lblPrivateMode.Name = "lblPrivateMode";
            this.lblPrivateMode.Size = new System.Drawing.Size(618, 26);
            this.lblPrivateMode.TabIndex = 0;
            this.lblPrivateMode.Text = "Özel mesaj modu";
            this.lblPrivateMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnCancelPrivate
            //
            this.btnCancelPrivate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancelPrivate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancelPrivate.Location = new System.Drawing.Point(630, 4);
            this.btnCancelPrivate.Name = "btnCancelPrivate";
            this.btnCancelPrivate.Size = new System.Drawing.Size(130, 26);
            this.btnCancelPrivate.TabIndex = 1;
            this.btnCancelPrivate.Text = "✕  Herkese yaz";
            this.btnCancelPrivate.Click += new System.EventHandler(this.btnCancelPrivate_Click);
            //
            // pnlCompose
            //
            this.pnlCompose.BackColor = System.Drawing.Color.White;
            this.pnlCompose.Controls.Add(this.pnlMessageBox);
            this.pnlCompose.Controls.Add(this.btnSend);
            this.pnlCompose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCompose.Location = new System.Drawing.Point(14, 441);
            this.pnlCompose.Name = "pnlCompose";
            this.pnlCompose.Size = new System.Drawing.Size(766, 56);
            this.pnlCompose.TabIndex = 3;
            //
            // pnlMessageBox
            //
            this.pnlMessageBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMessageBox.BackColor = System.Drawing.Color.White;
            this.pnlMessageBox.Controls.Add(this.txtMessage);
            this.pnlMessageBox.CornerRadius = 10;
            this.pnlMessageBox.Location = new System.Drawing.Point(0, 14);
            this.pnlMessageBox.Name = "pnlMessageBox";
            this.pnlMessageBox.Size = new System.Drawing.Size(640, 40);
            this.pnlMessageBox.TabIndex = 0;
            //
            // txtMessage
            //
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMessage.Location = new System.Drawing.Point(12, 10);
            this.txtMessage.MaxLength = 1000;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.PlaceholderText = "Mesajınızı yazın ve Enter'a basın…";
            this.txtMessage.Size = new System.Drawing.Size(616, 20);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            //
            // btnSend
            //
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSend.Location = new System.Drawing.Point(652, 14);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(114, 40);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Gönder";
            this.btnSend.Variant = ChatApp.Shared.UI.ButtonVariant.Primary;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            //
            // pnlChatHeader
            //
            this.pnlChatHeader.BackColor = System.Drawing.Color.White;
            this.pnlChatHeader.Controls.Add(this.btnClearChat);
            this.pnlChatHeader.Controls.Add(this.btnSaveHistory);
            this.pnlChatHeader.Controls.Add(this.lblChatTitle);
            this.pnlChatHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChatHeader.Location = new System.Drawing.Point(14, 6);
            this.pnlChatHeader.Name = "pnlChatHeader";
            this.pnlChatHeader.Size = new System.Drawing.Size(766, 44);
            this.pnlChatHeader.TabIndex = 0;
            //
            // btnClearChat
            //
            this.btnClearChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearChat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClearChat.Location = new System.Drawing.Point(476, 7);
            this.btnClearChat.Name = "btnClearChat";
            this.btnClearChat.Size = new System.Drawing.Size(90, 30);
            this.btnClearChat.TabIndex = 1;
            this.btnClearChat.Text = "Temizle";
            this.btnClearChat.Click += new System.EventHandler(this.btnClearChat_Click);
            //
            // btnSaveHistory
            //
            this.btnSaveHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveHistory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSaveHistory.Location = new System.Drawing.Point(576, 7);
            this.btnSaveHistory.Name = "btnSaveHistory";
            this.btnSaveHistory.Size = new System.Drawing.Size(180, 30);
            this.btnSaveHistory.TabIndex = 2;
            this.btnSaveHistory.Text = "Geçmişi Kaydet (.txt)";
            this.btnSaveHistory.Click += new System.EventHandler(this.btnSaveHistory_Click);
            //
            // lblChatTitle
            //
            this.lblChatTitle.AutoSize = true;
            this.lblChatTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblChatTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblChatTitle.Location = new System.Drawing.Point(0, 11);
            this.lblChatTitle.Name = "lblChatTitle";
            this.lblChatTitle.Size = new System.Drawing.Size(104, 21);
            this.lblChatTitle.TabIndex = 0;
            this.lblChatTitle.Text = "Sohbet Odası";
            //
            // cardUsers
            //
            this.cardUsers.BackColor = System.Drawing.Color.White;
            this.cardUsers.Controls.Add(this.lstUsers);
            this.cardUsers.Controls.Add(this.lblUsersHint);
            this.cardUsers.Controls.Add(this.lblUsersTitle);
            this.cardUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardUsers.Location = new System.Drawing.Point(818, 96);
            this.cardUsers.Margin = new System.Windows.Forms.Padding(4);
            this.cardUsers.Name = "cardUsers";
            this.cardUsers.Padding = new System.Windows.Forms.Padding(8, 6, 8, 10);
            this.cardUsers.Size = new System.Drawing.Size(262, 511);
            this.cardUsers.TabIndex = 2;
            //
            // lstUsers
            //
            this.lstUsers.BackColor = System.Drawing.Color.White;
            this.lstUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstUsers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstUsers.Location = new System.Drawing.Point(8, 86);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(246, 415);
            this.lstUsers.TabIndex = 2;
            this.lstUsers.DoubleClick += new System.EventHandler(this.lstUsers_DoubleClick);
            this.lstUsers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstUsers_KeyDown);
            //
            // lblUsersHint
            //
            this.lblUsersHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUsersHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblUsersHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblUsersHint.Location = new System.Drawing.Point(8, 50);
            this.lblUsersHint.Name = "lblUsersHint";
            this.lblUsersHint.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblUsersHint.Size = new System.Drawing.Size(246, 36);
            this.lblUsersHint.TabIndex = 1;
            this.lblUsersHint.Text = "Özel mesaj göndermek için bir kullanıcıya çift tıklayın.";
            //
            // lblUsersTitle
            //
            this.lblUsersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUsersTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblUsersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblUsersTitle.Location = new System.Drawing.Point(8, 6);
            this.lblUsersTitle.Name = "lblUsersTitle";
            this.lblUsersTitle.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblUsersTitle.Size = new System.Drawing.Size(246, 44);
            this.lblUsersTitle.TabIndex = 0;
            this.lblUsersTitle.Text = "Çevrimiçi (0)";
            this.lblUsersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // statusStrip
            //
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusMessage});
            this.statusStrip.Location = new System.Drawing.Point(0, 699);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1104, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            //
            // lblStatusMessage
            //
            this.lblStatusMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(1089, 17);
            this.lblStatusMessage.Spring = true;
            this.lblStatusMessage.Text = "Bağlı değil.";
            this.lblStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1104, 721);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 560);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat İstemcisi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.cardConnection.ResumeLayout(false);
            this.cardConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.cardChat.ResumeLayout(false);
            this.pnlPrivateBanner.ResumeLayout(false);
            this.pnlCompose.ResumeLayout(false);
            this.pnlMessageBox.ResumeLayout(false);
            this.pnlMessageBox.PerformLayout();
            this.pnlChatHeader.ResumeLayout(false);
            this.pnlChatHeader.PerformLayout();
            this.cardUsers.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private ChatApp.Shared.UI.StatusBadge statusBadge;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private ChatApp.Shared.UI.CardPanel cardConnection;
        private ChatApp.Shared.UI.ModernButton btnDisconnect;
        private ChatApp.Shared.UI.ModernButton btnConnect;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private ChatApp.Shared.UI.CardPanel cardChat;
        private ChatApp.Client.Controls.ChatView chatView;
        private System.Windows.Forms.Panel pnlPrivateBanner;
        private System.Windows.Forms.Label lblPrivateMode;
        private ChatApp.Shared.UI.ModernButton btnCancelPrivate;
        private System.Windows.Forms.Panel pnlCompose;
        private ChatApp.Shared.UI.CardPanel pnlMessageBox;
        private System.Windows.Forms.TextBox txtMessage;
        private ChatApp.Shared.UI.ModernButton btnSend;
        private System.Windows.Forms.Panel pnlChatHeader;
        private ChatApp.Shared.UI.ModernButton btnClearChat;
        private ChatApp.Shared.UI.ModernButton btnSaveHistory;
        private System.Windows.Forms.Label lblChatTitle;
        private ChatApp.Shared.UI.CardPanel cardUsers;
        private ChatApp.Client.Controls.UserListBox lstUsers;
        private System.Windows.Forms.Label lblUsersHint;
        private System.Windows.Forms.Label lblUsersTitle;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusMessage;
    }
}
