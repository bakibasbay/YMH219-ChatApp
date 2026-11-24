namespace ChatApp.Server
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
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.statusBadge = new ChatApp.Shared.UI.StatusBadge();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.cardSettings = new ChatApp.Shared.UI.CardPanel();
            this.lblClientCount = new System.Windows.Forms.Label();
            this.btnStop = new ChatApp.Shared.UI.ModernButton();
            this.btnStart = new ChatApp.Shared.UI.ModernButton();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.cardClients = new ChatApp.Shared.UI.CardPanel();
            this.lvClients = new System.Windows.Forms.ListView();
            this.colUserName = new System.Windows.Forms.ColumnHeader();
            this.colEndPoint = new System.Windows.Forms.ColumnHeader();
            this.colConnectedAt = new System.Windows.Forms.ColumnHeader();
            this.colMessageCount = new System.Windows.Forms.ColumnHeader();
            this.imlRowHeight = new System.Windows.Forms.ImageList(this.components);
            this.lblClientsTitle = new System.Windows.Forms.Label();
            this.cardLog = new ChatApp.Shared.UI.CardPanel();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.pnlLogHeader = new System.Windows.Forms.Panel();
            this.btnClearLog = new ChatApp.Shared.UI.ModernButton();
            this.lblLogTitle = new System.Windows.Forms.Label();
            this.cardAnnounce = new ChatApp.Shared.UI.CardPanel();
            this.txtAnnouncement = new System.Windows.Forms.TextBox();
            this.btnAnnounce = new ChatApp.Shared.UI.ModernButton();
            this.lblAnnounce = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlHeader.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.cardSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.cardClients.SuspendLayout();
            this.cardLog.SuspendLayout();
            this.pnlLogHeader.SuspendLayout();
            this.cardAnnounce.SuspendLayout();
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
            this.pnlHeader.Size = new System.Drawing.Size(1064, 76);
            this.pnlHeader.TabIndex = 0;
            //
            // statusBadge
            //
            this.statusBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusBadge.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.statusBadge.ForeColor = System.Drawing.Color.White;
            this.statusBadge.Location = new System.Drawing.Point(800, 20);
            this.statusBadge.Name = "statusBadge";
            this.statusBadge.Size = new System.Drawing.Size(244, 36);
            this.statusBadge.TabIndex = 2;
            this.statusBadge.Text = "Durdu";
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
            this.lblTitle.Text = "Chat Sunucusu";
            //
            // tlpMain
            //
            this.tlpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpMain.Controls.Add(this.cardSettings, 0, 0);
            this.tlpMain.Controls.Add(this.cardClients, 0, 1);
            this.tlpMain.Controls.Add(this.cardLog, 1, 1);
            this.tlpMain.Controls.Add(this.cardAnnounce, 0, 2);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 76);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(12);
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tlpMain.Size = new System.Drawing.Size(1064, 563);
            this.tlpMain.TabIndex = 1;
            //
            // cardSettings
            //
            this.cardSettings.BackColor = System.Drawing.Color.White;
            this.tlpMain.SetColumnSpan(this.cardSettings, 2);
            this.cardSettings.Controls.Add(this.lblClientCount);
            this.cardSettings.Controls.Add(this.btnStop);
            this.cardSettings.Controls.Add(this.btnStart);
            this.cardSettings.Controls.Add(this.numPort);
            this.cardSettings.Controls.Add(this.lblPort);
            this.cardSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardSettings.Location = new System.Drawing.Point(16, 16);
            this.cardSettings.Margin = new System.Windows.Forms.Padding(4);
            this.cardSettings.Name = "cardSettings";
            this.cardSettings.Size = new System.Drawing.Size(1032, 68);
            this.cardSettings.TabIndex = 0;
            //
            // lblClientCount
            //
            this.lblClientCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClientCount.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblClientCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblClientCount.Location = new System.Drawing.Point(752, 16);
            this.lblClientCount.Name = "lblClientCount";
            this.lblClientCount.Size = new System.Drawing.Size(260, 36);
            this.lblClientCount.TabIndex = 4;
            this.lblClientCount.Text = "Bağlı istemci: 0";
            this.lblClientCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // btnStop
            //
            this.btnStop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnStop.Location = new System.Drawing.Point(358, 16);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(150, 36);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "■  Sunucuyu Durdur";
            this.btnStop.Variant = ChatApp.Shared.UI.ButtonVariant.Danger;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            //
            // btnStart
            //
            this.btnStart.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(196, 16);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 36);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "▶  Sunucuyu Başlat";
            this.btnStart.Variant = ChatApp.Shared.UI.ButtonVariant.Success;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            //
            // numPort
            //
            this.numPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPort.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numPort.Location = new System.Drawing.Point(70, 19);
            this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(106, 29);
            this.numPort.TabIndex = 1;
            this.numPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            //
            // lblPort
            //
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblPort.Location = new System.Drawing.Point(20, 24);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(37, 19);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Port";
            //
            // cardClients
            //
            this.cardClients.BackColor = System.Drawing.Color.White;
            this.cardClients.Controls.Add(this.lvClients);
            this.cardClients.Controls.Add(this.lblClientsTitle);
            this.cardClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardClients.Location = new System.Drawing.Point(16, 92);
            this.cardClients.Margin = new System.Windows.Forms.Padding(4);
            this.cardClients.Name = "cardClients";
            this.cardClients.Padding = new System.Windows.Forms.Padding(14, 8, 14, 14);
            this.cardClients.Size = new System.Drawing.Size(556, 371);
            this.cardClients.TabIndex = 1;
            //
            // lvClients
            //
            this.lvClients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUserName,
            this.colEndPoint,
            this.colConnectedAt,
            this.colMessageCount});
            this.lvClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvClients.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lvClients.FullRowSelect = true;
            this.lvClients.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvClients.Location = new System.Drawing.Point(14, 44);
            this.lvClients.MultiSelect = false;
            this.lvClients.Name = "lvClients";
            this.lvClients.OwnerDraw = true;
            this.lvClients.Size = new System.Drawing.Size(528, 313);
            this.lvClients.SmallImageList = this.imlRowHeight;
            this.lvClients.TabIndex = 1;
            this.lvClients.UseCompatibleStateImageBehavior = false;
            this.lvClients.View = System.Windows.Forms.View.Details;
            this.lvClients.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvClients_DrawColumnHeader);
            this.lvClients.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvClients_DrawItem);
            this.lvClients.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvClients_DrawSubItem);
            this.lvClients.Resize += new System.EventHandler(this.lvClients_Resize);
            //
            // colUserName
            //
            this.colUserName.Text = "Kullanıcı adı";
            this.colUserName.Width = 160;
            //
            // colEndPoint
            //
            this.colEndPoint.Text = "IP:Port";
            this.colEndPoint.Width = 140;
            //
            // colConnectedAt
            //
            this.colConnectedAt.Text = "Bağlanma zamanı";
            this.colConnectedAt.Width = 150;
            //
            // colMessageCount
            //
            this.colMessageCount.Text = "Mesaj sayısı";
            this.colMessageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colMessageCount.Width = 90;
            //
            // imlRowHeight
            //
            this.imlRowHeight.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlRowHeight.ImageSize = new System.Drawing.Size(1, 34);
            this.imlRowHeight.TransparentColor = System.Drawing.Color.Transparent;
            //
            // lblClientsTitle
            //
            this.lblClientsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblClientsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblClientsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblClientsTitle.Location = new System.Drawing.Point(14, 8);
            this.lblClientsTitle.Name = "lblClientsTitle";
            this.lblClientsTitle.Size = new System.Drawing.Size(528, 36);
            this.lblClientsTitle.TabIndex = 0;
            this.lblClientsTitle.Text = "Bağlı İstemciler";
            this.lblClientsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cardLog
            //
            this.cardLog.BackColor = System.Drawing.Color.White;
            this.cardLog.Controls.Add(this.rtbLog);
            this.cardLog.Controls.Add(this.pnlLogHeader);
            this.cardLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardLog.Location = new System.Drawing.Point(580, 92);
            this.cardLog.Margin = new System.Windows.Forms.Padding(4);
            this.cardLog.Name = "cardLog";
            this.cardLog.Padding = new System.Windows.Forms.Padding(14, 8, 14, 14);
            this.cardLog.Size = new System.Drawing.Size(468, 371);
            this.cardLog.TabIndex = 2;
            //
            // rtbLog
            //
            this.rtbLog.BackColor = System.Drawing.Color.White;
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.DetectUrls = false;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rtbLog.Location = new System.Drawing.Point(14, 44);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(440, 313);
            this.rtbLog.TabIndex = 1;
            this.rtbLog.Text = "";
            //
            // pnlLogHeader
            //
            this.pnlLogHeader.BackColor = System.Drawing.Color.White;
            this.pnlLogHeader.Controls.Add(this.btnClearLog);
            this.pnlLogHeader.Controls.Add(this.lblLogTitle);
            this.pnlLogHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogHeader.Location = new System.Drawing.Point(14, 8);
            this.pnlLogHeader.Name = "pnlLogHeader";
            this.pnlLogHeader.Size = new System.Drawing.Size(440, 36);
            this.pnlLogHeader.TabIndex = 0;
            //
            // btnClearLog
            //
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClearLog.Location = new System.Drawing.Point(350, 4);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(90, 28);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Temizle";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            //
            // lblLogTitle
            //
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblLogTitle.Location = new System.Drawing.Point(0, 7);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(110, 21);
            this.lblLogTitle.TabIndex = 0;
            this.lblLogTitle.Text = "Olay Günlüğü";
            //
            // cardAnnounce
            //
            this.cardAnnounce.BackColor = System.Drawing.Color.White;
            this.tlpMain.SetColumnSpan(this.cardAnnounce, 2);
            this.cardAnnounce.Controls.Add(this.txtAnnouncement);
            this.cardAnnounce.Controls.Add(this.btnAnnounce);
            this.cardAnnounce.Controls.Add(this.lblAnnounce);
            this.cardAnnounce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardAnnounce.Location = new System.Drawing.Point(16, 471);
            this.cardAnnounce.Margin = new System.Windows.Forms.Padding(4);
            this.cardAnnounce.Name = "cardAnnounce";
            this.cardAnnounce.Size = new System.Drawing.Size(1032, 68);
            this.cardAnnounce.TabIndex = 3;
            //
            // txtAnnouncement
            //
            this.txtAnnouncement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAnnouncement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAnnouncement.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtAnnouncement.Location = new System.Drawing.Point(150, 19);
            this.txtAnnouncement.MaxLength = 1000;
            this.txtAnnouncement.Name = "txtAnnouncement";
            this.txtAnnouncement.PlaceholderText = "Tüm istemcilere gönderilecek duyuruyu yazın ve Enter'a basın…";
            this.txtAnnouncement.Size = new System.Drawing.Size(708, 27);
            this.txtAnnouncement.TabIndex = 1;
            this.txtAnnouncement.TextChanged += new System.EventHandler(this.txtAnnouncement_TextChanged);
            this.txtAnnouncement.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAnnouncement_KeyDown);
            //
            // btnAnnounce
            //
            this.btnAnnounce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnnounce.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAnnounce.Location = new System.Drawing.Point(872, 16);
            this.btnAnnounce.Name = "btnAnnounce";
            this.btnAnnounce.Size = new System.Drawing.Size(140, 36);
            this.btnAnnounce.TabIndex = 2;
            this.btnAnnounce.Text = "Duyuru Gönder";
            this.btnAnnounce.Variant = ChatApp.Shared.UI.ButtonVariant.Primary;
            this.btnAnnounce.Click += new System.EventHandler(this.btnAnnounce_Click);
            //
            // lblAnnounce
            //
            this.lblAnnounce.AutoSize = true;
            this.lblAnnounce.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAnnounce.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblAnnounce.Location = new System.Drawing.Point(20, 24);
            this.lblAnnounce.Name = "lblAnnounce";
            this.lblAnnounce.Size = new System.Drawing.Size(120, 19);
            this.lblAnnounce.TabIndex = 0;
            this.lblAnnounce.Text = "Sunucu duyurusu";
            //
            // statusStrip
            //
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusMessage});
            this.statusStrip.Location = new System.Drawing.Point(0, 639);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1064, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            //
            // lblStatusMessage
            //
            this.lblStatusMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(1049, 17);
            this.lblStatusMessage.Spring = true;
            this.lblStatusMessage.Text = "Sunucu durduruldu. Başlatmak için bir port seçip \"Sunucuyu Başlat\"a tıklayın.";
            this.lblStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1064, 661);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(860, 560);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Sunucusu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.cardSettings.ResumeLayout(false);
            this.cardSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.cardClients.ResumeLayout(false);
            this.cardLog.ResumeLayout(false);
            this.pnlLogHeader.ResumeLayout(false);
            this.pnlLogHeader.PerformLayout();
            this.cardAnnounce.ResumeLayout(false);
            this.cardAnnounce.PerformLayout();
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
        private ChatApp.Shared.UI.CardPanel cardSettings;
        private System.Windows.Forms.Label lblClientCount;
        private ChatApp.Shared.UI.ModernButton btnStop;
        private ChatApp.Shared.UI.ModernButton btnStart;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPort;
        private ChatApp.Shared.UI.CardPanel cardClients;
        private System.Windows.Forms.ListView lvClients;
        private System.Windows.Forms.ColumnHeader colUserName;
        private System.Windows.Forms.ColumnHeader colEndPoint;
        private System.Windows.Forms.ColumnHeader colConnectedAt;
        private System.Windows.Forms.ColumnHeader colMessageCount;
        private System.Windows.Forms.ImageList imlRowHeight;
        private System.Windows.Forms.Label lblClientsTitle;
        private ChatApp.Shared.UI.CardPanel cardLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Panel pnlLogHeader;
        private ChatApp.Shared.UI.ModernButton btnClearLog;
        private System.Windows.Forms.Label lblLogTitle;
        private ChatApp.Shared.UI.CardPanel cardAnnounce;
        private System.Windows.Forms.TextBox txtAnnouncement;
        private ChatApp.Shared.UI.ModernButton btnAnnounce;
        private System.Windows.Forms.Label lblAnnounce;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusMessage;
    }
}
