using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class PersonelHomeScreen : Form
    {
        private readonly string _oturumKullaniciAdi;
        private readonly bool _isYonetici;

        // Sidebar buton referansları (aktif seçim için)
        private Button _aktifSidebarBtn;
        // İçerik paneli
        private Panel _pnlIcerik;
        // Bildirim rozet etiketi
        private Label _lblBildirimRozet;

        public PersonelHomeScreen() : this("", false) { }

        public PersonelHomeScreen(string oturumKullaniciAdi, bool isYonetici = false)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            _isYonetici = isYonetici;
            InitializeComponent();
            OlusturArayuz();
        }

        // ── Ana Arayüz ───────────────────────────────────────────────────────
        private void OlusturArayuz()
        {
            tableUstBar.Visible = false;

            // İçerik alanı
            _pnlIcerik = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Surface
            };
            this.Controls.Remove(panelChart);
            this.Controls.Remove(tableStats);
            _pnlIcerik.Controls.Add(panelChart);
            _pnlIcerik.Controls.Add(tableStats);
            this.Controls.Add(_pnlIcerik);

            // Sidebar
            var pnlSidebar = OlusturSidebar();
            this.Controls.Add(pnlSidebar);

            UiTheme.FormDizayn(this);
        }

        // ── Sidebar Oluştur ──────────────────────────────────────────────────
        private Panel OlusturSidebar()
        {
            var sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = UiTheme.SidebarBg
            };

            // Logo alanı
            var pnlLogo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = UiTheme.PrimaryDark,
                Padding = new Padding(20, 20, 20, 0)
            };
            var sepLogo = new Panel { Dock = DockStyle.Bottom, Height = 2, BackColor = UiTheme.Accent };
            var lblLogo = new Label
            {
                Text = "🏛  Belediye\nOtomasyonu",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 0, 0)
            };
            pnlLogo.Controls.Add(lblLogo);
            pnlLogo.Controls.Add(sepLogo);
            sidebar.Controls.Add(pnlLogo);

            // Kullanıcı bilgi alanı
            var pnlUser = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(22, 38, 72),
                Padding = new Padding(20, 12, 20, 12)
            };
            string rol = _isYonetici ? "Yönetici" : "Personel";
            var lblUser = new Label
            {
                Text = $"👤  {_oturumKullaniciAdi}\n    {rol}",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 200, 230),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlUser.Controls.Add(lblUser);
            sidebar.Controls.Add(pnlUser);

            // Çıkış butonu (en alta)
            var bCikis = new Button { Text = "  ⏻  Çıkış Yap", Dock = DockStyle.Bottom, Height = 50 };
            UiTheme.SidebarButon(bCikis);
            bCikis.ForeColor = Color.FromArgb(220, 100, 100);
            bCikis.Click += (s, e) => { var g = new İlkGiris(); g.Show(); Close(); };
            sidebar.Controls.Add(bCikis);

            // Separator
            var sep = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(40, 60, 100) };
            sidebar.Controls.Add(sep);

            // Yönetici menü öğeleri
            if (_isYonetici)
            {
                // Bildirim Gönder
                var bBildirimGonder = new Button { Text = "  📢  Bildirim Gönder", Dock = DockStyle.Top, Height = 48 };
                UiTheme.SidebarButon(bBildirimGonder);
                bBildirimGonder.Click += (s, e) =>
                {
                    using (var f = new BildirimGonderForm(_oturumKullaniciAdi))
                        f.ShowDialog(this);
                };
                sidebar.Controls.Add(bBildirimGonder);

                // Duyuru Ekle
                var bDuyuru = new Button { Text = "  📌  Duyuru Ekle", Dock = DockStyle.Top, Height = 48 };
                UiTheme.SidebarButon(bDuyuru);
                bDuyuru.Click += (s, e) => GosterDuyuruEkleDialog();
                sidebar.Controls.Add(bDuyuru);

                // Personel Yönetimi
                var bPersonel = new Button { Text = "  👥  Personel Yönetimi", Dock = DockStyle.Top, Height = 48 };
                UiTheme.SidebarButon(bPersonel);
                bPersonel.Click += (s, e) =>
                {
                    using (var f = new PersonelYonetimForm(_oturumKullaniciAdi))
                        f.ShowDialog(this);
                    YukleDashboardVerileri();
                };
                sidebar.Controls.Add(bPersonel);

                // Vatandaş Yönetimi
                var bVatandas = new Button { Text = "  🧑‍💼  Vatandaş Yönetimi", Dock = DockStyle.Top, Height = 48 };
                UiTheme.SidebarButon(bVatandas);
                bVatandas.Click += (s, e) =>
                {
                    using (var f = new VatandasYonetimForm(_oturumKullaniciAdi))
                        f.ShowDialog(this);
                    YukleDashboardVerileri();
                };
                sidebar.Controls.Add(bVatandas);
            }

            // Başvuru Yönetimi (herkes)
            var bBasvuru = new Button { Text = "  📋  Başvuru Yönetimi", Dock = DockStyle.Top, Height = 48 };
            UiTheme.SidebarButon(bBasvuru);
            bBasvuru.Click += (s, e) =>
            {
                using (var f = new BasvuruYonetimForm(_oturumKullaniciAdi))
                    f.ShowDialog(this);
                YukleDashboardVerileri();
            };
            sidebar.Controls.Add(bBasvuru);

            // Bildirimler (herkes)
            var bBildirim = new Button { Text = "  🔔  Bildirimler", Dock = DockStyle.Top, Height = 48 };
            UiTheme.SidebarButon(bBildirim);
            bBildirim.Click += (s, e) =>
            {
                SidebarSecimDegistir(bBildirim);
                GosterBildirimlerPaneli();
            };
            // Rozet
            _lblBildirimRozet = new Label
            {
                AutoSize = false,
                Size = new Size(22, 22),
                BackColor = UiTheme.Danger,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(180, 13),
                Visible = false
            };
            bBildirim.Controls.Add(_lblBildirimRozet);
            sidebar.Controls.Add(bBildirim);

            // Ana Sayfa
            var bAnaSayfa = new Button { Text = "  🏠  Ana Sayfa", Dock = DockStyle.Top, Height = 48 };
            UiTheme.SidebarButon(bAnaSayfa, secili: true);
            bAnaSayfa.Click += (s, e) =>
            {
                SidebarSecimDegistir(bAnaSayfa);
                GosterDashboard();
            };
            sidebar.Controls.Add(bAnaSayfa);
            _aktifSidebarBtn = bAnaSayfa;

            GuncelleBildirimRozeti();
            return sidebar;
        }

        private void SidebarSecimDegistir(Button yeni)
        {
            if (_aktifSidebarBtn != null)
                UiTheme.SidebarButon(_aktifSidebarBtn, secili: false);
            UiTheme.SidebarButon(yeni, secili: true);
            _aktifSidebarBtn = yeni;
        }

        // ── Dashboard ────────────────────────────────────────────────────────
        private void GosterDashboard()
        {
            _pnlIcerik.Controls.Clear();
            _pnlIcerik.Controls.Add(panelChart);
            _pnlIcerik.Controls.Add(tableStats);
            YukleDashboardVerileri();
        }

        // ── Bildirimler Paneli ────────────────────────────────────────────────
        private void GosterBildirimlerPaneli()
        {
            _pnlIcerik.Controls.Clear();
            BelediyeDbServisi.BildirimleriOkunduIsaretle(_oturumKullaniciAdi);
            GuncelleBildirimRozeti();

            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GonderimTarihi", HeaderText = "Tarih", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GonderenKadi", HeaderText = "Gönderen", Width = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Baslik", HeaderText = "Başlık", Width = 220 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Icerik", HeaderText = "İçerik", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            try { dgv.DataSource = BelediyeDbServisi.BildirimleriGetir(_oturumKullaniciAdi); }
            catch (Exception ex) { MessageBox.Show("Bildirimler yüklenemedi: " + ex.Message); }

            // WinForms docking: Fill önce ekle, Top sonra ekle (ters işlem sırası)
            pnl.Controls.Add(dgv);   // Fill — önce ekle

            var header = UiTheme.OlusturHeaderPanel("🔔  Bildirimler", "Yöneticilerden gelen bildirimler");
            header.Dock = DockStyle.Top;
            pnl.Controls.Add(header); // Top — sonra ekle (önce işlenir, alanı alır)

            _pnlIcerik.Controls.Add(pnl);
        }

        private void GuncelleBildirimRozeti()
        {
            try
            {
                int n = BelediyeDbServisi.OkunmamisBildirimSayisi(_oturumKullaniciAdi);
                if (_lblBildirimRozet != null)
                {
                    _lblBildirimRozet.Visible = n > 0;
                    _lblBildirimRozet.Text = n > 9 ? "9+" : n.ToString();
                }
            }
            catch { }
        }

        // ── Duyuru Ekleme Dialog ──────────────────────────────────────────────
        private void GosterDuyuruEkleDialog()
        {
            var frm = new Form
            {
                Text = "Duyuru Ekle",
                Size = new Size(520, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = UiTheme.Surface,
                Font = UiTheme.UiFont
            };

            // 1. Footer — ÖNCE ekle
            var pnlF = new Panel { Dock = DockStyle.Bottom, Height = 58, BackColor = Color.White };
            var sepF  = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            var btnKaydet = new Button { Text = "  Yayınla", Dock = DockStyle.Right, Width = 120 };
            UiTheme.AccentButon(btnKaydet);
            var btnKapat = new Button { Text = "İptal", Dock = DockStyle.Right, Width = 100 };
            UiTheme.IkincilButon(btnKapat);
            btnKapat.Click += (s, e) => frm.Close();
            pnlF.Controls.AddRange(new Control[] { btnKaydet, btnKapat, sepF });
            frm.Controls.Add(pnlF);

            // 2. Body — TableLayoutPanel (Fill)
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(24, 16, 24, 8),
                BackColor = UiTheme.Surface
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var lblB = new Label { Text = "Başlık", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            var txtB = new TextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, MaxLength = 200 };
            var lblI = new Label { Text = "İçerik", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            var rtbI = new RichTextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };

            tbl.Controls.Add(lblB, 0, 0);
            tbl.Controls.Add(txtB, 0, 1);
            tbl.Controls.Add(lblI, 0, 2);
            tbl.Controls.Add(rtbI, 0, 3);
            frm.Controls.Add(tbl);

            // 3. Header — SON ekle (WinForms'ta son eklenen Top önce işlenir)
            frm.Controls.Add(UiTheme.OlusturHeaderPanel("📌  Yeni Duyuru Ekle"));

            btnKaydet.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtB.Text))
                { MessageBox.Show("Başlık boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtB.Focus(); return; }
                var hata = BelediyeDbServisi.DuyuruEkle(txtB.Text, rtbI.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                MessageBox.Show("Duyuru yayınlandı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frm.Close();
            };

            frm.ShowDialog(this);
        }

        // ── Load ve Veriler ───────────────────────────────────────────────────
        private void PersonelHomeScreen_Load(object sender, EventArgs e)
        {
            // Kart renkleri
            lblChartTitle.ForeColor = UiTheme.TextPrimary;
            lblCapVatandas.ForeColor  = UiTheme.TextMuted;
            lblCapPersonel.ForeColor  = UiTheme.TextMuted;
            lblCapBasvuru.ForeColor   = UiTheme.TextMuted;
            lblKayitliVatandas.ForeColor = UiTheme.Primary;
            lblPersonelSayisi.ForeColor  = UiTheme.Primary;
            lblToplamBasvuru.ForeColor   = UiTheme.Accent;
            lblKayitliVatandas.Font = UiTheme.StatValueFont;
            lblPersonelSayisi.Font  = UiTheme.StatValueFont;
            lblToplamBasvuru.Font   = UiTheme.StatValueFont;
            cardVatandas.BackColor = UiTheme.CardBackground;
            cardPersonel.BackColor = UiTheme.CardBackground;
            cardBasvuru.BackColor  = UiTheme.CardBackground;
            // Kart border sol çizgisi
            cardVatandas.Paint += (s, ea) => ea.Graphics.FillRectangle(new SolidBrush(UiTheme.Primary), 0, 0, 4, cardVatandas.Height);
            cardPersonel.Paint += (s, ea) => ea.Graphics.FillRectangle(new SolidBrush(UiTheme.Secondary), 0, 0, 4, cardPersonel.Height);
            cardBasvuru.Paint  += (s, ea) => ea.Graphics.FillRectangle(new SolidBrush(UiTheme.Accent), 0, 0, 4, cardBasvuru.Height);

            this.Text = _isYonetici
                ? $"Belediye Yönetim Sistemi — Yönetici: {_oturumKullaniciAdi}"
                : $"Belediye Yönetim Sistemi — Personel: {_oturumKullaniciAdi}";

            YukleDashboardVerileri();
        }

        private void btnYenile_Click(object sender, EventArgs e) => YukleDashboardVerileri();

        private void YukleDashboardVerileri()
        {
            try
            {
                var oz = BelediyeDbServisi.YukleDashboardOzet();
                lblKayitliVatandas.Text = oz.KayitliVatandasSayisi.ToString();
                lblPersonelSayisi.Text  = oz.PersonelSayisi.ToString();
                lblToplamBasvuru.Text   = oz.ToplamBasvuru.ToString();
                GuncelleGrafik(oz);
                lblChartTitle.Text = "Başvuru Durumları — Anlık Özet";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Özet yüklenemedi. MySQL ve `belediye` veritabanı hazır mı?\n\n" + ex.Message,
                    "Veritabanı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GuncelleGrafik(DashboardOzet oz)
        {
            try
            {
                cartesianChart1.Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Başvuru Adedi",
                        Values = new ChartValues<int> { oz.Beklemede, oz.Islemde, oz.Tamamlandi },
                        Fill = new System.Windows.Media.SolidColorBrush(
                            System.Windows.Media.Color.FromRgb(26, 45, 90))
                    }
                };
                cartesianChart1.AxisX.Clear();
                cartesianChart1.AxisX.Add(new Axis { Labels = new[] { "Beklemede", "İşlemde", "Tamamlandı" } });
                cartesianChart1.AxisY.Clear();
                cartesianChart1.AxisY.Add(new Axis { Title = "Adet", MinValue = 0 });
                cartesianChart1.LegendLocation = LegendLocation.Top;
            }
            catch (Exception ex)
            {
                lblChartTitle.Text = "Grafik gösterilemedi: " + ex.Message;
            }
        }

        private void btnPersonelYonetim_Click(object sender, EventArgs e)
        {
            using (var f = new PersonelYonetimForm(_oturumKullaniciAdi))
                f.ShowDialog(this);
            YukleDashboardVerileri();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            var g = new İlkGiris();
            g.Show();
            Close();
        }
    }
}
