using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class VatandasHomeScreen : Form
    {
        private readonly string _tcKimlik;
        private string _adSoyad = "";
        private Button _aktifBtn;
        private Panel _pnlIcerik;

        public VatandasHomeScreen(string tcKimlik)
        {
            _tcKimlik = tcKimlik ?? "";
            InitializeComponent();
            OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            // Eski kontrolleri temizle
            this.Controls.Clear();
            this.Text = "Belediye Vatandaş Portalı";
            this.MinimumSize = new Size(900, 600);
            this.Size = new Size(1050, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            UiTheme.FormDizayn(this);

            // İçerik paneli
            _pnlIcerik = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            this.Controls.Add(_pnlIcerik);

            // Sidebar
            var sidebar = OlusturSidebar();
            this.Controls.Add(sidebar);
        }

        private Panel OlusturSidebar()
        {
            var sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = UiTheme.SidebarBg
            };

            // Logo
            var pnlLogo = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = UiTheme.PrimaryDark };
            var sepLogo = new Panel { Dock = DockStyle.Bottom, Height = 2, BackColor = UiTheme.Accent };
            var lblLogo = new Label
            {
                Text = "🏛  Belediye\nVatandaş Portalı",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlLogo.Controls.Add(lblLogo);
            pnlLogo.Controls.Add(sepLogo);
            sidebar.Controls.Add(pnlLogo);

            // Kullanıcı alanı
            var pnlUser = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.FromArgb(22, 38, 72), Padding = new Padding(20, 12, 20, 12) };
            BelediyeDbServisi.TryGetKullaniciDisplayName(_tcKimlik, out _adSoyad);
            var lblUser = new Label
            {
                Text = $"👤  {(_adSoyad.Length > 0 ? _adSoyad : _tcKimlik)}\n    Vatandaş",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 200, 230),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlUser.Controls.Add(lblUser);
            sidebar.Controls.Add(pnlUser);

            // Çıkış
            var bCikis = new Button { Text = "  ⏻  Çıkış Yap", Dock = DockStyle.Bottom, Height = 50 };
            UiTheme.SidebarButon(bCikis);
            bCikis.ForeColor = Color.FromArgb(220, 100, 100);
            bCikis.Click += (s, e) => { new İlkGiris().Show(); Close(); };
            sidebar.Controls.Add(bCikis);

            var sep = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(40, 60, 100) };
            sidebar.Controls.Add(sep);

            // Profil
            Button bProfil = SidebarBtn("  👤  Profilim");
            bProfil.Click += (s, e) => { SidebarSec(bProfil); GosterProfil(); };
            sidebar.Controls.Add(bProfil);

            // Duyurular
            Button bDuyuru = SidebarBtn("  📌  Duyurular");
            bDuyuru.Click += (s, e) => { SidebarSec(bDuyuru); GosterDuyurular(); };
            sidebar.Controls.Add(bDuyuru);

            // Başvurularım
            Button bBasvuru = SidebarBtn("  📋  Başvurularım");
            bBasvuru.Click += (s, e) => { SidebarSec(bBasvuru); GosterBasvurular(); };
            sidebar.Controls.Add(bBasvuru);

            // Ana Sayfa
            Button bAna = SidebarBtn("  🏠  Ana Sayfa");
            UiTheme.SidebarButon(bAna, secili: true);
            bAna.Click += (s, e) => { SidebarSec(bAna); GosterAnaSayfa(); };
            sidebar.Controls.Add(bAna);
            _aktifBtn = bAna;

            return sidebar;
        }

        private Button SidebarBtn(string text)
        {
            var b = new Button { Text = text, Dock = DockStyle.Top, Height = 48 };
            UiTheme.SidebarButon(b);
            return b;
        }

        private void SidebarSec(Button b)
        {
            if (_aktifBtn != null) UiTheme.SidebarButon(_aktifBtn);
            UiTheme.SidebarButon(b, secili: true);
            _aktifBtn = b;
        }

        // ── Load ─────────────────────────────────────────────────────────────
        private void VatandasHomeScreen_Load(object sender, EventArgs e) => GosterAnaSayfa();

        // ── ANA SAYFA ─────────────────────────────────────────────────────────
        private void GosterAnaSayfa()
        {
            _pnlIcerik.Controls.Clear();

            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };

            // Header
            var hdr = UiTheme.OlusturHeaderPanel(
                "🏠  Hoş Geldiniz" + (_adSoyad.Length > 0 ? $", {_adSoyad}" : ""),
                "Belediye Vatandaş Portalı — Tüm hizmetlere buradan ulaşabilirsiniz.");
            hdr.Dock = DockStyle.Top;
            pnl.Controls.Add(hdr);

            // İçerik
            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 20, 24, 20) };

            // İstatistik kartları
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 120,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(0, 0, 0, 16)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));

            BelediyeDbServisi.YukleGenelSayilar(out int toplamBasvuru, out int kayitliVatandas);
            int benimBasvuru = BelediyeDbServisi.VatandasBasvurulariniGetir(_tcKimlik).Rows.Count;

            tbl.Controls.Add(StatKart("Kayıtlı Vatandaş", kayitliVatandas.ToString(), UiTheme.Primary), 0, 0);
            tbl.Controls.Add(StatKart("Toplam Başvuru", toplamBasvuru.ToString(), UiTheme.Accent), 1, 0);
            tbl.Controls.Add(StatKart("Başvurularım", benimBasvuru.ToString(), UiTheme.Success), 2, 0);
            pnlBody.Controls.Add(tbl);

            // Son duyurular başlık
            var lblDuy = new Label
            {
                Text = "📌  Son Duyurular",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = UiTheme.TextPrimary,
                AutoSize = true,
                Location = new Point(0, 130)
            };
            pnlBody.Controls.Add(lblDuy);

            // Duyuru listesi
            var dgvDuy = new DataGridView { Location = new Point(0, 160), Height = 200, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            dgvDuy.Width = _pnlIcerik.Width - 280;
            UiTheme.DataGridStil(dgvDuy);
            dgvDuy.AutoGenerateColumns = false;
            dgvDuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "YayinTarihi", HeaderText = "Tarih", Width = 140 });
            dgvDuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Baslik", HeaderText = "Başlık", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            try { dgvDuy.DataSource = BelediyeDbServisi.DuyurulariGetir(); } catch { }
            pnlBody.Controls.Add(dgvDuy);

            // Hızlı başvuru butonu
            var btnHizli = new Button { Text = "  + Yeni Başvuru Aç", Location = new Point(0, 380), Width = 200, Height = 42 };
            UiTheme.AnaEylemButonu(btnHizli);
            btnHizli.Click += (s, e) => { SidebarSec(_aktifBtn); GosterBasvurular(acDialog: true); };
            pnlBody.Controls.Add(btnHizli);

            // WinForms dock kuralı: Fill önce, Top sonra eklenir
            pnl.Controls.Add(pnlBody); // Fill
            pnl.Controls.Add(hdr);     // Top
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── BAŞVURULARIM ──────────────────────────────────────────────────────
        private void GosterBasvurular(bool acDialog = false)
        {
            _pnlIcerik.Controls.Clear();

            var pnl = new Panel { Dock = DockStyle.Fill };
            var hdr = UiTheme.OlusturHeaderPanel("📋  Başvurularım", "Mevcut başvurularınız ve yeni başvuru oluşturma");

            // Alt butonlar
            var pnlBot = new Panel { Dock = DockStyle.Bottom, Height = 58, BackColor = Color.White, Padding = new Padding(16, 8, 16, 8) };
            var sep = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            var btnYeniBasvuru = new Button { Text = "  + Yeni Başvuru Aç", Dock = DockStyle.Left, Width = 190 };
            UiTheme.AnaEylemButonu(btnYeniBasvuru);
            var btnYenile = new Button { Text = "Yenile", Dock = DockStyle.Right, Width = 100 };
            UiTheme.IkincilButon(btnYenile);
            pnlBot.Controls.AddRange(new Control[] { btnYeniBasvuru, btnYenile, sep });
            pnl.Controls.Add(pnlBot);

            // Grid
            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "No", Width = 55 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Kategori", HeaderText = "Kategori", Width = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Konu", HeaderText = "Konu / Açıklama", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KayitTarihi", HeaderText = "Tarih", Width = 140 });

            // Durum rengi
            dgv.CellFormatting += (s, e) =>
            {
                if (dgv.Columns[e.ColumnIndex].DataPropertyName == "Durum" && e.Value != null)
                {
                    e.CellStyle.ForeColor = UiTheme.DurumRengi(e.Value.ToString());
                    e.CellStyle.Font = UiTheme.UiFontBold;
                }
            };

            Action yukle = () =>
            {
                try { dgv.DataSource = BelediyeDbServisi.VatandasBasvurulariniGetir(_tcKimlik); }
                catch (Exception ex) { MessageBox.Show("Başvurular yüklenemedi: " + ex.Message); }
            };
            yukle();

            btnYenile.Click += (s, e) => yukle();
            btnYeniBasvuru.Click += (s, e) => YeniBasvuruDialog(yukle);

            // WinForms dock kuralı: Bottom/Fill önce, Top sonra
            pnl.Controls.Add(dgv);     // Fill
            pnl.Controls.Add(hdr);     // Top — sonra eklenir, önce işlenir
            _pnlIcerik.Controls.Add(pnl);

            if (acDialog) YeniBasvuruDialog(yukle);
        }

        private void YeniBasvuruDialog(Action sonraYukle)
        {
            var frm = new Form
            {
                Text = "Yeni Başvuru",
                Size = new Size(540, 480),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = UiTheme.Surface,
                Font = UiTheme.UiFont
            };

            // Footer (Bottom) — ÖNCE ekle
            var pnlF = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.White };
            var sepF = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            var btnGon = new Button { Text = "Başvuruyu Gönder", Dock = DockStyle.Right, Width = 180 };
            UiTheme.AnaEylemButonu(btnGon);
            var btnIp = new Button { Text = "İptal", Dock = DockStyle.Right, Width = 100 };
            UiTheme.IkincilButon(btnIp);
            btnIp.Click += (s, e) => frm.Close();
            pnlF.Controls.AddRange(new Control[] { btnGon, btnIp, sepF });
            frm.Controls.Add(pnlF); // 1. Bottom

            // Body — TableLayoutPanel ile düzgün hücreler
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Padding = new Padding(24, 16, 24, 8),
                BackColor = UiTheme.Surface
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));  // lblKat
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));  // cmbKat
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 14f));  // boşluk
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));  // lblKon
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));  // txtKon
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));  // açıklama (kalan)

            var lblKat = new Label { Text = "Kategori", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            var cmbKat = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = UiTheme.UiFont };
            cmbKat.Items.AddRange(new[] { "İmar & Yapı", "Sosyal Yardım", "Şikayet", "Temizlik & Çevre", "Ulaşım", "Su & Altyapı", "Vergi & Ruhsat", "Evlilik & Nüfus", "Diğer" });
            cmbKat.SelectedIndex = 0;

            var lblKon = new Label { Text = "Başvuru Konusu", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            var txtKon = new TextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, MaxLength = 200 };

            // Açıklama satırı — içinde label + RTB için iç panel
            var pnlAc = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var lblAc = new Label { Text = "Açıklama (isteğe bağlı)", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, AutoSize = true, Location = new Point(0, 4) };
            var rtbAc = new RichTextBox { Location = new Point(0, 28), Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom };
            rtbAc.Width  = 460;
            rtbAc.Height = 110;
            pnlAc.Controls.Add(lblAc);
            pnlAc.Controls.Add(rtbAc);

            tbl.Controls.Add(lblKat, 0, 0);
            tbl.Controls.Add(cmbKat, 0, 1);
            tbl.Controls.Add(new Label(), 0, 2); // boşluk
            tbl.Controls.Add(lblKon, 0, 3);
            tbl.Controls.Add(txtKon, 0, 4);
            tbl.Controls.Add(pnlAc,  0, 5);

            frm.Controls.Add(tbl);  // 2. Fill

            // Header — SON ekle (döck işleminde önce işlenir = üstte görünür)
            var hdr = UiTheme.OlusturHeaderPanel("📋  Yeni Başvuru Oluştur", "Talebinizi aşağıdaki formu doldurarak iletin.");
            frm.Controls.Add(hdr); // 3. Top

            btnGon.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtKon.Text))
                {
                    MessageBox.Show("Başvuru konusu boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKon.Focus(); return;
                }
                string hata = BelediyeDbServisi.VatandasBasvuruEkle(_tcKimlik, txtKon.Text, cmbKat.SelectedItem?.ToString(), rtbAc.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                MessageBox.Show("Başvurunuz alınmıştır. Takip edebilirsiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frm.Close();
                sonraYukle?.Invoke();
            };

            frm.ShowDialog(this);
        }

        // ── DUYURULAR ─────────────────────────────────────────────────────────
        private void GosterDuyurular()
        {
            _pnlIcerik.Controls.Clear();

            var pnl = new Panel { Dock = DockStyle.Fill };
            var hdr = UiTheme.OlusturHeaderPanel("📌  Duyurular", "Belediyemizden güncel duyuru ve haberler");
            hdr.Dock = DockStyle.Top;
            pnl.Controls.Add(hdr);

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "YayinTarihi", HeaderText = "Yayın Tarihi", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Baslik", HeaderText = "Başlık", Width = 280 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Icerik", HeaderText = "İçerik", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            try { dgv.DataSource = BelediyeDbServisi.DuyurulariGetir(); }
            catch (Exception ex) { MessageBox.Show("Duyurular yüklenemedi: " + ex.Message); }

            // Detay paneli
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 280, BackColor = Color.White, Padding = new Padding(16) };
            var lblDetayBas = new Label { Text = "Duyuru İçeriği", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 28 };
            var rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            pnlDetay.Controls.Add(rtbDetay);
            pnlDetay.Controls.Add(lblDetayBas);
            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv)
                {
                    rtbDetay.Text = drv["Icerik"]?.ToString() ?? "";
                    lblDetayBas.Text = drv["Baslik"]?.ToString() ?? "Duyuru İçeriği";
                }
            };

            pnl.Controls.Add(dgv);
            pnl.Controls.Add(pnlDetay);
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── PROFİL ────────────────────────────────────────────────────────────
        private void GosterProfil()
        {
            _pnlIcerik.Controls.Clear();

            var pnl = new Panel { Dock = DockStyle.Fill };
            var hdr = UiTheme.OlusturHeaderPanel("👤  Profilim", "Kişisel bilgileriniz (salt okunur)");
            hdr.Dock = DockStyle.Top;
            pnl.Controls.Add(hdr);

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40, 30, 40, 30) };

            // Profil kartı
            var kart = new Panel
            {
                BackColor = Color.White,
                Location = new Point(0, 0),
                Size = new Size(440, 300),
                Padding = new Padding(28)
            };
            kart.Paint += (s, e) =>
            {
                e.Graphics.FillRectangle(new SolidBrush(UiTheme.Primary), 0, 0, 5, kart.Height);
                using (var pen = new Pen(UiTheme.BorderCard, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, kart.Width - 1, kart.Height - 1);
            };

            int y = 16;
            kart.Controls.Add(ProfilSatirPanel("Ad Soyad", _adSoyad, ref y));
            kart.Controls.Add(ProfilSatirPanel("TC Kimlik No", _tcKimlik, ref y));

            // Veritabanından ek bilgi çek
            try
            {
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT e_Mail, KullaniciAdi FROM kullanicilar WHERE tc=@tc LIMIT 1", con))
                {
                    cmd.Parameters.AddWithValue("@tc", _tcKimlik);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            kart.Controls.Add(ProfilSatirPanel("E-Posta", r.IsDBNull(0) ? "-" : r.GetString(0), ref y));
                            kart.Controls.Add(ProfilSatirPanel("Kullanıcı Adı", r.IsDBNull(1) ? "-" : r.GetString(1), ref y));
                        }
                    }
                }
            }
            catch { }

            var lblNot = new Label
            {
                Text = "ℹ  Bilgilerinizi güncellemek için belediyemizle iletişime geçin.",
                Font = UiTheme.SmallFont,
                ForeColor = UiTheme.TextMuted,
                AutoSize = false,
                Size = new Size(440, 36),
                Location = new Point(0, y + 10)
            };
            pnlBody.Controls.Add(kart);
            pnlBody.Controls.Add(lblNot);

            pnl.Controls.Add(pnlBody);
            _pnlIcerik.Controls.Add(pnl);
        }


        private Panel ProfilSatirPanel(string etiket, string deger, ref int y)
        {
            var pnlSatir = new Panel { Location = new Point(20, y), Size = new Size(390, 46) };
            pnlSatir.Controls.Add(new Label { Text = etiket, Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, AutoSize = true, Location = new Point(0, 0) });
            pnlSatir.Controls.Add(new Label { Text = deger, Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, AutoSize = true, Location = new Point(0, 18) });
            pnlSatir.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = UiTheme.BorderSubtle });
            y += 54;
            return pnlSatir;
        }

        // ── YARDIMCI ─────────────────────────────────────────────────────────
        private Panel StatKart(string baslik, string deger, Color renk)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(6) };
            p.Paint += (s, e) => e.Graphics.FillRectangle(new SolidBrush(renk), 0, 0, 4, p.Height);
            p.Controls.Add(new Label { Text = baslik, Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, AutoSize = true, Location = new Point(14, 14) });
            p.Controls.Add(new Label { Text = deger, Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = renk, AutoSize = true, Location = new Point(14, 32) });
            return p;
        }
    }
}
