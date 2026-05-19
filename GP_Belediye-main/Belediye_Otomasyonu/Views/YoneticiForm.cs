using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using Belediye_Otomasyonu.Models;

namespace Belediye_Otomasyonu.Views
{
    /// <summary>
    /// Belediye yöneticisinin tüm sistemi izlediği, başvuruları filtreleyip
    /// performans istatistiklerini kontrol ettiği form.
    /// </summary>
    public class YoneticiForm : Form
    {
        private Label lblTotalCount;
        private Label lblPendingCount;
        private Label lblCompletedCount;
        private Label lblRejectedCount;

        private DataGridView dgvAllBasvurular;
        private DataGridView dgvPersonelPerformans;
        private ComboBox cmbFiltreKategori;
        private ComboBox cmbFiltreDurum;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        public YoneticiForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Yönetici Formundaki tüm dashboard, filtre ve listeleme kontrol elemanlarını oluşturur.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "T.C. Göksu Belediyesi - Yönetici Dashboard Kontrol Paneli";
            this.Size = new Size(1150, 720);
            this.MinimumSize = new Size(1050, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.AutoScroll = true;

            // Arka plan degrade
            this.Paint += Form_Paint;

            // Ana layout (Header, Summary Cards, Content, Status)
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70f));  // Header
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110f)); // Summary Cards
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));  // Main Panel
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));   // Status

            this.Controls.Add(mainLayout);

            // 1. Üst Panel (Header)
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(160, 18, 30, 49)
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
                Text = "YÖNETİM BİLGİ SİSTEMİ & DASHBOARD",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(280, 24),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblSubTitle);
            mainLayout.Controls.Add(pnlHeader, 0, 0);

            // 2. Özet Kartları Paneli (4 adet yan yana kart)
            TableLayoutPanel cardsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(12, 6, 12, 6),
                BackColor = Color.Transparent
            };
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            // Kart 1: Toplam Başvuru
            Panel pnlCard1 = OlusturOzetKart("TOPLAM BAŞVURU", out lblTotalCount, Color.DodgerBlue);
            cardsLayout.Controls.Add(pnlCard1, 0, 0);

            // Kart 2: Bekleyenler
            Panel pnlCard2 = OlusturOzetKart("BEKLEYEN BAŞVURU", out lblPendingCount, Color.Orange);
            cardsLayout.Controls.Add(pnlCard2, 1, 0);

            // Kart 3: Tamamlananlar
            Panel pnlCard3 = OlusturOzetKart("TAMAMLANAN", out lblCompletedCount, Color.ForestGreen);
            cardsLayout.Controls.Add(pnlCard3, 2, 0);

            // Kart 4: Reddedilenler
            Panel pnlCard4 = OlusturOzetKart("REDDEDİLEN", out lblRejectedCount, Color.Crimson);
            cardsLayout.Controls.Add(pnlCard4, 3, 0);

            mainLayout.Controls.Add(cardsLayout, 0, 1);

            // 3. Ana İçerik Bölümü (Sol: Filtre ve Liste, Sağ: Personel Performans)
            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(12),
                BackColor = Color.Transparent
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
            mainLayout.Controls.Add(contentLayout, 0, 2);

            // Sol Panel: Filtreler ve Dilekçe Listesi
            Panel pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 255, 255, 255),
                Padding = new Padding(12),
                Margin = new Padding(6)
            };
            pnlLeft.Paint += PanelBorder_Paint;

            TableLayoutPanel listLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            listLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45f));  // Filtre Barı
            listLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Grid
            listLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));  // Butonlar

            // Filtre Barı
            FlowLayoutPanel pnlFilterBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent
            };
            
            pnlFilterBar.Controls.Add(new Label { Text = "Kategori: ", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 60), Margin = new Padding(0, 8, 4, 0) });
            cmbFiltreKategori = new ComboBox { Width = 110, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), Margin = new Padding(0, 4, 12, 0) };
            cmbFiltreKategori.Items.AddRange(new[] { "Tümü", "Altyapı", "Park-Bahçe", "Temizlik", "Su", "Elektrik", "Diğer" });
            cmbFiltreKategori.SelectedIndex = 0;
            cmbFiltreKategori.SelectedIndexChanged += Filtrele;
            pnlFilterBar.Controls.Add(cmbFiltreKategori);

            pnlFilterBar.Controls.Add(new Label { Text = "Durum: ", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 60), Margin = new Padding(0, 8, 4, 0) });
            cmbFiltreDurum = new ComboBox { Width = 110, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), Margin = new Padding(0, 4, 12, 0) };
            cmbFiltreDurum.Items.AddRange(new[] { "Tümü", "Bekliyor", "Atandı", "Tamamlandı", "Reddedildi" });
            cmbFiltreDurum.SelectedIndex = 0;
            cmbFiltreDurum.SelectedIndexChanged += Filtrele;
            pnlFilterBar.Controls.Add(cmbFiltreDurum);

            Button btnYenile = new Button
            {
                Text = "🔄 Yenile",
                Size = new Size(80, 28),
                BackColor = Color.SlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Margin = new Padding(0, 2, 0, 0)
            };
            btnYenile.FlatAppearance.BorderSize = 0;
            btnYenile.Click += BtnYenile_Click;
            pnlFilterBar.Controls.Add(btnYenile);

            listLayout.Controls.Add(pnlFilterBar, 0, 0);

            // DataGridView
            dgvAllBasvurular = new DataGridView
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
                Font = new Font("Segoe UI", 9f)
            };
            dgvAllBasvurular.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 50, 90);
            dgvAllBasvurular.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAllBasvurular.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvAllBasvurular.EnableHeadersVisualStyles = false;
            dgvAllBasvurular.RowTemplate.Height = 35;
            listLayout.Controls.Add(dgvAllBasvurular, 0, 1);

            // Alt Butonlar
            FlowLayoutPanel pnlActionButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };

            Button btnRapor = new Button
            {
                Text = "📥 Excel/PDF Rapor Al",
                Size = new Size(160, 36),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRapor.FlatAppearance.BorderSize = 0;
            btnRapor.Click += BtnRapor_Click;

            Button btnPersYonet = new Button
            {
                Text = "👥 Personel Yönet",
                Size = new Size(150, 36),
                BackColor = Color.DarkSlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPersYonet.FlatAppearance.BorderSize = 0;
            btnPersYonet.Click += BtnPersYonet_Click;

            Button btnAyarlar = new Button
            {
                Text = "⚙️ Ayarlar",
                Size = new Size(100, 36),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAyarlar.FlatAppearance.BorderSize = 0;
            btnAyarlar.Click += BtnAyarlar_Click;

            pnlActionButtons.Controls.Add(btnRapor);
            pnlActionButtons.Controls.Add(btnPersYonet);
            pnlActionButtons.Controls.Add(btnAyarlar);

            listLayout.Controls.Add(pnlActionButtons, 0, 2);

            pnlLeft.Controls.Add(listLayout);
            contentLayout.Controls.Add(pnlLeft, 0, 0);

            // Sağ Panel: Personel Performans Listesi
            Panel pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 255, 255, 255),
                Padding = new Padding(12),
                Margin = new Padding(6)
            };
            pnlRight.Paint += PanelBorder_Paint;

            TableLayoutPanel perfLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            perfLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            perfLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            Label lblPerfTitle = new Label
            {
                Text = "📊 Personel Performans & Yük Dağılımı",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 40, 80),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            perfLayout.Controls.Add(lblPerfTitle, 0, 0);

            dgvPersonelPerformans = new DataGridView
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
                AutoSizeColumnsMode = dgvPersonelPerformans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9f)
            };
            dgvPersonelPerformans.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 50, 90);
            dgvPersonelPerformans.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPersonelPerformans.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvPersonelPerformans.EnableHeadersVisualStyles = false;
            dgvPersonelPerformans.RowTemplate.Height = 35;

            perfLayout.Controls.Add(dgvPersonelPerformans, 0, 1);
            pnlRight.Controls.Add(perfLayout);
            contentLayout.Controls.Add(pnlRight, 1, 0);

            // 4. Status Bar
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel { Text = "Dashboard hazır. Güncel veriler listelendi." };
            statusStrip.Items.Add(lblStatus);
            mainLayout.Controls.Add(statusStrip, 0, 3);

            // Verileri yükle
            YenileDashboard();
        }

        private Panel OlusturOzetKart(string baslik, out Label lblSayi, Color seritRengi)
        {
            Panel p = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 255, 255, 255),
                Padding = new Padding(12),
                Margin = new Padding(6)
            };
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Sol renkli şerit
                using (var brush = new SolidBrush(seritRengi))
                    e.Graphics.FillRectangle(brush, 0, 0, 6, p.Height);
                // Çevre sınır çizgisi
                using (var pen = new Pen(Color.FromArgb(50, seritRengi), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };

            Label lblBas = new Label
            {
                Text = baslik,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.SlateGray,
                Location = new Point(14, 10),
                AutoSize = true
            };
            lblSayi = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 50, 80),
                Location = new Point(14, 30),
                AutoSize = true
            };

            p.Controls.Add(lblBas);
            p.Controls.Add(lblSayi);
            return p;
        }

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

        private void DrawBackground(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(Color.FromArgb(25, 255, 255, 255)))
            {
                g.FillPolygon(brush, new Point[] {
                    new Point(0, this.Height),
                    new Point(this.Width / 6, this.Height - 150),
                    new Point(this.Width / 3, this.Height)
                });
                g.FillPolygon(brush, new Point[] {
                    new Point(this.Width / 2, this.Height),
                    new Point(3 * this.Width / 5, this.Height - 130),
                    new Point(this.Width, this.Height)
                });
            }
        }

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

        private void YenileDashboard()
        {
            // İstatistikleri hesapla ve etiketlere ata
            int total = SharedData.Basvurular.Count;
            int pending = SharedData.Basvurular.Count(b => b.Durum == "Bekliyor");
            int completed = SharedData.Basvurular.Count(b => b.Durum == "Tamamlandı");
            int rejected = SharedData.Basvurular.Count(b => b.Durum == "Reddedildi");

            lblTotalCount.Text = total.ToString();
            lblPendingCount.Text = pending.ToString();
            lblCompletedCount.Text = completed.ToString();
            lblRejectedCount.Text = rejected.ToString();

            // Gridi filtreye göre yenile
            Filtrele(null, null);

            // Personel performansını yenile
            dgvPersonelPerformans.DataSource = null;
            dgvPersonelPerformans.DataSource = SharedData.Personeller;

            if (dgvPersonelPerformans.Columns.Count > 0)
            {
                dgvPersonelPerformans.Columns["AdSoyad"].HeaderText = "Personel Ad Soyad";
                dgvPersonelPerformans.Columns["Departman"].HeaderText = "Departman";
                dgvPersonelPerformans.Columns["AtananIsCount"].HeaderText = "Bitirilen/Atanan İş";

                if (dgvPersonelPerformans.Columns["Ad"] != null) dgvPersonelPerformans.Columns["Ad"].Visible = false;
                if (dgvPersonelPerformans.Columns["Soyad"] != null) dgvPersonelPerformans.Columns["Soyad"].Visible = false;
                if (dgvPersonelPerformans.Columns["Unvan"] != null) dgvPersonelPerformans.Columns["Unvan"].Visible = false;
            }
        }

        private void Filtrele(object sender, EventArgs e)
        {
            string kat = cmbFiltreKategori.SelectedItem?.ToString() ?? "Tümü";
            string dur = cmbFiltreDurum.SelectedItem?.ToString() ?? "Tümü";

            var list = SharedData.Basvurular.AsQueryable();

            if (kat != "Tümü") list = list.Where(b => b.Kategori == kat);
            if (dur != "Tümü") list = list.Where(b => b.Durum == dur);

            dgvAllBasvurular.DataSource = null;
            dgvAllBasvurular.DataSource = list.ToList();

            if (dgvAllBasvurular.Columns.Count > 0)
            {
                dgvAllBasvurular.Columns["BasvuruID"].HeaderText = "ID";
                dgvAllBasvurular.Columns["AdSoyad"].HeaderText = "Başvuru Sahibi";
                dgvAllBasvurular.Columns["Kategori"].HeaderText = "Kategori";
                dgvAllBasvurular.Columns["Durum"].HeaderText = "Durum";
                dgvAllBasvurular.Columns["BasvuruTarihi"].HeaderText = "Tarih";
                dgvAllBasvurular.Columns["Oncelik"].HeaderText = "Öncelik";

                // Gereksiz kolonları gizle
                foreach (DataGridViewColumn col in dgvAllBasvurular.Columns)
                {
                    if (col.Name != "BasvuruID" && col.Name != "AdSoyad" && col.Name != "Kategori" && col.Name != "Durum" && col.Name != "BasvuruTarihi" && col.Name != "Oncelik")
                    {
                        col.Visible = false;
                    }
                }
            }
        }

        private void BtnYenile_Click(object sender, EventArgs e)
        {
            YenileDashboard();
            lblStatus.Text = "Veriler başarıyla yenilendi.";
        }

        private void BtnRapor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Rapor hazırlama motoru çalıştırıldı.\nTüm veriler 'C:\\Raporlar\\Basvuru_Raporu.xlsx' dosyasına aktarıldı (Simüle edildi).", "Rapor Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnPersYonet_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Personel Yönetim Modülü açılıyor (Simüle edildi).", "Personel Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAyarlar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sistem Ayarları Modülü açılıyor (Simüle edildi).", "Ayarlar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
