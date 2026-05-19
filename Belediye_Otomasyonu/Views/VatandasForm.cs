using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using Belediye_Otomasyonu.Models;

namespace Belediye_Otomasyonu.Views
{
    /// <summary>
    /// Vatandaşların belediyeye yeni başvurular göndermesini ve eski başvurularını izlemesini sağlayan form.
    /// </summary>
    public class VatandasForm : Form
    {
        private TextBox txtAdSoyad;
        private TextBox txtTCNo;
        private TextBox txtTelefon;
        private ComboBox cmbKategori;
        private RichTextBox rtbAciklama;
        private DataGridView dgvBasvurular;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        public VatandasForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Vatandaş Formunun tüm görsel kontrollerini ve yerleşim yapısını oluşturur.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "T.C. Göksu Belediyesi - Vatandaş Başvuru Paneli";
            this.Size = new Size(1000, 650);
            this.MinimumSize = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.AutoScroll = true;

            // Form Paint olayı ile gradient arka plan çizilir
            this.Paint += Form_Paint;

            // Main TableLayoutPanel: Tüm formu dikeyde böler (Header, Content, Status)
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70f)); // Header
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f)); // StatusStrip
            this.Controls.Add(mainLayout);

            // 1. Üst Başlık ve Logo Paneli
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(160, 18, 30, 49) // Yarı şeffaf koyu mavi
            };
            Label lblLogo = new Label
            {
                Text = "🏛️  GÖKSU BELEDİYESİ",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.Gold,
                Location = new Point(20, 18),
                AutoSize = true
            };
            Label lblSubTitle = new Label
            {
                Text = "VATANDAŞ BAŞVURU VE DİLEKÇE HİZMETLERİ",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(280, 24),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblSubTitle);
            mainLayout.Controls.Add(pnlHeader, 0, 0);

            // 2. Orta İçerik Alanı (Yatayda ikiye bölünür: Sol form, Sağ liste)
            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(12),
                BackColor = Color.Transparent
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f)); // Sol form paneli
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55f)); // Sağ listeleme paneli
            mainLayout.Controls.Add(contentLayout, 0, 1);

            // Sol Panel: Giriş Formu (Yarı Şeffaf)
            Panel pnlForm = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 255, 255, 255),
                Padding = new Padding(16),
                Margin = new Padding(6)
            };
            pnlForm.Paint += PanelBorder_Paint;

            TableLayoutPanel inputLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 12,
                BackColor = Color.Transparent
            };
            // Label ve Input satır stilleri
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0) // Label satırları
                    inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
                else if (i == 9) // Açıklama alanı
                    inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100f));
                else if (i == 11) // Butonlar alanı
                    inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));
                else // Textbox satırları
                    inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            }

            int tabIndex = 1;

            // Ad Soyad
            inputLayout.Controls.Add(new Label { Text = "Ad Soyad *", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 60), Dock = DockStyle.Fill }, 0, 0);
            txtAdSoyad = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f), TabIndex = tabIndex++ };
            inputLayout.Controls.Add(txtAdSoyad, 0, 1);

            // TC Kimlik No
            inputLayout.Controls.Add(new Label { Text = "T.C. Kimlik Numarası * (11 Hane)", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 60), Dock = DockStyle.Fill }, 0, 2);
            txtTCNo = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f), MaxLength = 11, TabIndex = tabIndex++ };
            txtTCNo.KeyPress += TxtTCNo_KeyPress;
            inputLayout.Controls.Add(txtTCNo, 0, 3);

            // Telefon
            inputLayout.Controls.Add(new Label { Text = "İletişim Telefonu *", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 60), Dock = DockStyle.Fill }, 0, 4);
            txtTelefon = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f), MaxLength = 15, TabIndex = tabIndex++ };
            inputLayout.Controls.Add(txtTelefon, 0, 5);

            // Kategori
            inputLayout.Controls.Add(new Label { Text = "Başvuru Kategorisi *", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 60), Dock = DockStyle.Fill }, 0, 6);
            cmbKategori = new ComboBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f), DropDownStyle = ComboBoxStyle.DropDownList, TabIndex = tabIndex++ };
            cmbKategori.Items.AddRange(new[] { "Altyapı", "Park-Bahçe", "Temizlik", "Su", "Elektrik", "Diğer" });
            cmbKategori.SelectedIndex = 0;
            inputLayout.Controls.Add(cmbKategori, 0, 7);

            // Açıklama
            inputLayout.Controls.Add(new Label { Text = "Açıklama / Detaylı Talep *", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 60), Dock = DockStyle.Fill }, 0, 8);
            rtbAciklama = new RichTextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f), TabIndex = tabIndex++ };
            inputLayout.Controls.Add(rtbAciklama, 0, 9);

            // Butonlar Alanı
            FlowLayoutPanel pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };

            Button btnBasvur = new Button
            {
                Text = "🚀 Başvur",
                Size = new Size(110, 36),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = tabIndex++
            };
            btnBasvur.FlatAppearance.BorderSize = 0;
            btnBasvur.Click += BtnBasvur_Click;

            Button btnTemizle = new Button
            {
                Text = "🧹 Temizle",
                Size = new Size(110, 36),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = tabIndex++
            };
            btnTemizle.FlatAppearance.BorderSize = 0;
            btnTemizle.Click += BtnTemizle_Click;

            Button btnSorgula = new Button
            {
                Text = "🔍 Başvurularım",
                Size = new Size(140, 36),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = tabIndex++
            };
            btnSorgula.FlatAppearance.BorderSize = 0;
            btnSorgula.Click += BtnSorgula_Click;

            pnlButtons.Controls.Add(btnBasvur);
            pnlButtons.Controls.Add(btnTemizle);
            pnlButtons.Controls.Add(btnSorgula);
            inputLayout.Controls.Add(pnlButtons, 0, 11);

            pnlForm.Controls.Add(inputLayout);
            contentLayout.Controls.Add(pnlForm, 0, 0);

            // Sağ Panel: Başvuru Takip Listesi
            Panel pnlList = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 255, 255, 255),
                Padding = new Padding(12),
                Margin = new Padding(6)
            };
            pnlList.Paint += PanelBorder_Paint;

            TableLayoutPanel listLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            listLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            listLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            Label lblListHeader = new Label
            {
                Text = "📋 T.C. Kimlik Numaranıza Ait Başvurular",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 40, 80),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            listLayout.Controls.Add(lblListHeader, 0, 0);

            dgvBasvurular = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(220, 225, 230),
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9f),
                TabIndex = tabIndex++
            };
            dgvBasvurular.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 50, 90);
            dgvBasvurular.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBasvurular.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvBasvurular.EnableHeadersVisualStyles = false;
            dgvBasvurular.RowTemplate.Height = 35;
            listLayout.Controls.Add(dgvBasvurular, 0, 1);

            pnlList.Controls.Add(listLayout);
            contentLayout.Controls.Add(pnlList, 1, 0);

            // 3. Durum Çubuğu (StatusStrip)
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel { Text = "Sistem hazır. Yeni bir dilekçe veya başvuru oluşturabilirsiniz." };
            statusStrip.Items.Add(lblStatus);
            mainLayout.Controls.Add(statusStrip, 0, 2);

            // İlk veriyi listele
            YenileGrid();
        }

        /// <summary>
        /// Formun arka planına degrade geçiş ve belediye silüeti çizer.
        /// </summary>
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(20, 60, 120),
                Color.FromArgb(100, 160, 220),
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            DrawBackground(e.Graphics);
        }

        /// <summary>
        /// Formun arka planına soyut geometrik tepeler ve silüet kısımları çizer.
        /// </summary>
        private void DrawBackground(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(Color.FromArgb(25, 255, 255, 255)))
            {
                g.FillPolygon(brush, new Point[] {
                    new Point(0, this.Height),
                    new Point(this.Width / 4, this.Height - 140),
                    new Point(this.Width / 2, this.Height)
                });
                g.FillPolygon(brush, new Point[] {
                    new Point(this.Width / 3, this.Height),
                    new Point(2 * this.Width / 3, this.Height - 190),
                    new Point(this.Width, this.Height)
                });
            }
        }

        /// <summary>
        /// Yarı şeffaf panellere modern ince bir sınır çizgisi çizer.
        /// </summary>
        private void PanelBorder_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            if (p != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(120, 255, 255, 255), 1.5f))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            }
        }

        /// <summary>
        /// T.C. Kimlik numarasına sadece rakam girilmesini sağlar.
        /// </summary>
        private void TxtTCNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Yeni başvuru kaydını doğrular ve listeye kaydeder.
        /// </summary>
        private void BtnBasvur_Click(object sender, EventArgs e)
        {
            try
            {
                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(txtAdSoyad.Text) || 
                    string.IsNullOrWhiteSpace(txtTCNo.Text) || 
                    string.IsNullOrWhiteSpace(txtTelefon.Text) || 
                    string.IsNullOrWhiteSpace(rtbAciklama.Text))
                {
                    MessageBox.Show("Lütfen yıldızlı (*) tüm alanları doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // TC doğrulama
                if (txtTCNo.Text.Length != 11)
                {
                    MessageBox.Show("T.C. Kimlik numarası tam olarak 11 haneden oluşmalıdır.", "Geçersiz T.C.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Başvuru ID Oluşturma (BEL + Yıl + 5 haneli sıra no)
                int year = DateTime.Now.Year;
                int sequence = SharedData.Basvurular.Count + 1;
                string newId = $"BEL{year}-{sequence:D5}";

                // Yeni başvuru modeli
                Basvuru basvuru = new Basvuru
                {
                    BasvuruID = newId,
                    AdSoyad = txtAdSoyad.Text.Trim(),
                    TCNo = txtTCNo.Text.Trim(),
                    Telefon = txtTelefon.Text.Trim(),
                    Kategori = cmbKategori.SelectedItem.ToString(),
                    Aciklama = rtbAciklama.Text.Trim(),
                    Durum = "Bekliyor",
                    BasvuruTarihi = DateTime.Now
                };

                // Belleğe ekle
                SharedData.Basvurular.Add(basvuru);

                lblStatus.Text = $"{newId} numaralı başvurunuz başarıyla oluşturuldu.";
                MessageBox.Show($"Başvurunuz başarıyla alınmıştır.\nBaşvuru Numarası: {newId}", "Başvuru Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                TemizleForm();
                YenileGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Başvuru sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Form giriş kutularındaki tüm verileri temizler.
        /// </summary>
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TemizleForm();
        }

        /// <summary>
        /// Girilen T.C. Kimlik numarasına ait eski başvuruları grid üzerinde listeler.
        /// </summary>
        private void BtnSorgula_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTCNo.Text))
            {
                MessageBox.Show("Sorgulama yapmak için lütfen T.C. Kimlik numaranızı yazınız.", "T.C. Eksik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tc = txtTCNo.Text.Trim();
            var citizenList = SharedData.Basvurular.Where(b => b.TCNo == tc).ToList();

            if (citizenList.Count == 0)
            {
                MessageBox.Show("Bu T.C. Kimlik numarasına ait bir başvuru kaydı bulunamadı.", "Kayıt Yok", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dgvBasvurular.DataSource = citizenList;
            FormatGridColumns();
        }

        private void TemizleForm()
        {
            txtAdSoyad.Clear();
            txtTCNo.Clear();
            txtTelefon.Clear();
            rtbAciklama.Clear();
            cmbKategori.SelectedIndex = 0;
            lblStatus.Text = "Form temizlendi. Yeni veri girişi yapabilirsiniz.";
        }

        private void YenileGrid()
        {
            dgvBasvurular.DataSource = null;
            dgvBasvurular.DataSource = SharedData.Basvurular;
            FormatGridColumns();
        }

        private void FormatGridColumns()
        {
            if (dgvBasvurular.Columns.Count > 0)
            {
                // Kolon başlıklarını Türkçe ve düzgün yap
                dgvBasvurular.Columns["BasvuruID"].HeaderText = "Başvuru No";
                dgvBasvurular.Columns["AdSoyad"].HeaderText = "Ad Soyad";
                dgvBasvurular.Columns["Kategori"].HeaderText = "Kategori";
                dgvBasvurular.Columns["BasvuruTarihi"].HeaderText = "Tarih";
                dgvBasvurular.Columns["Durum"].HeaderText = "Durum";

                // Diğer görünmesini istemediğimiz kolonları gizle
                if (dgvBasvurular.Columns["TCNo"] != null) dgvBasvurular.Columns["TCNo"].Visible = false;
                if (dgvBasvurular.Columns["Telefon"] != null) dgvBasvurular.Columns["Telefon"].Visible = false;
                if (dgvBasvurular.Columns["Aciklama"] != null) dgvBasvurular.Columns["Aciklama"].Visible = false;
                if (dgvBasvurular.Columns["AtananPersonel"] != null) dgvBasvurular.Columns["AtananPersonel"].Visible = false;
                if (dgvBasvurular.Columns["AtanmaNotari"] != null) dgvBasvurular.Columns["AtanmaNotari"].Visible = false;
                if (dgvBasvurular.Columns["AtanmaTarihi"] != null) dgvBasvurular.Columns["AtanmaTarihi"].Visible = false;
                if (dgvBasvurular.Columns["Oncelik"] != null) dgvBasvurular.Columns["Oncelik"].Visible = false;
            }
        }
    }
}
