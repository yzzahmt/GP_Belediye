using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private Button _btnBasvurularim;
        private Button _btnAnaSayfa;
        private Button _btnDilekceYaz;
        private Button _btnBorclar;

        public VatandasHomeScreen(string tcKimlik)
        {
            _tcKimlik = tcKimlik ?? "";
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Controls.Clear();
            this.Text = "Belediye Vatandas Portali";
            this.MinimumSize = new Size(950, 620);
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UiTheme.Surface;
            this.Font = UiTheme.UiFont;

            var sidebar = OlusturSidebar();
            this.Controls.Add(sidebar);

            _pnlIcerik = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            this.Controls.Add(_pnlIcerik);
            _pnlIcerik.SendToBack();
        }

        private Panel OlusturSidebar()
        {
            var sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = UiTheme.SidebarBg };
            sidebar.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(0, sidebar.Height),
                    UiTheme.SidebarBg, Color.FromArgb(17, 28, 59)))
                    e.Graphics.FillRectangle(br, sidebar.ClientRectangle);
            };

            // Logo
            var pnlLogo = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.Transparent };
            pnlLogo.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(pnlLogo.Width, 0),
                    UiTheme.PrimaryDark, Color.FromArgb(28, 37, 65)))
                    e.Graphics.FillRectangle(br, pnlLogo.ClientRectangle);
            };
            var sepLogo = new Panel { Dock = DockStyle.Bottom, Height = 3, BackColor = UiTheme.Accent };
            var lblLogo = new Label {
                Text = "  Belediye\nVatandas Portali",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0)
            };
            pnlLogo.Controls.Add(lblLogo);
            pnlLogo.Controls.Add(sepLogo);

            // Kullanici
            BelediyeDbServisi.TryGetKullaniciDisplayName(_tcKimlik, out _adSoyad);
            var pnlUser = new Panel { Dock = DockStyle.Top, Height = 72 };
            UiTheme.SidebarUserPaneli(pnlUser, _adSoyad.Length > 0 ? _adSoyad : _tcKimlik, "Vatandaş");

            // Cikis
            var bCikis = new Button { Text = "  Cikis Yap", Dock = DockStyle.Bottom, Height = 50 };
            UiTheme.SidebarButon(bCikis);
            bCikis.ForeColor = Color.FromArgb(220, 90, 90);
            bCikis.Click += (s, e) => { new İlkGiris().Show(); Close(); };
            var sepBottom = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(30, 41, 59) };

            // Menu butonlar
            Button bProfil  = SidebarBtn("    Profilim");
            Button bDuyuru  = SidebarBtn("    Duyurular");
            Button bBildirim = SidebarBtn("    Bildirimlerim");
            _btnBasvurularim = SidebarBtn("    Basvurularim");
            _btnDilekceYaz = SidebarBtn("    Dilekce Yaz");
            _btnBorclar = SidebarBtn("    Borç Ödeme");
            _btnAnaSayfa = SidebarBtn("    Ana Sayfa");

            _btnAnaSayfa.Click     += (s, e) => { SidebarSec(_btnAnaSayfa);     GosterAnaSayfa(); };
            _btnDilekceYaz.Click += (s, e) => { SidebarSec(_btnDilekceYaz); GosterDilekce(); };
            _btnBasvurularim.Click += (s, e) => { SidebarSec(_btnBasvurularim); GosterBasvurular(); };
            _btnBorclar.Click += (s, e) => { SidebarSec(_btnBorclar); GosterBorclar(); };
            bBildirim.Click += (s, e) => { SidebarSec(bBildirim); GosterBildirimlerim(); };
            bDuyuru.Click  += (s, e) => { SidebarSec(bDuyuru);  GosterDuyurular(); };
            bProfil.Click  += (s, e) => { SidebarSec(bProfil);  GosterProfil(); };

            UiTheme.SidebarButon(_btnAnaSayfa, secili: true);
            _aktifBtn = _btnAnaSayfa;

            // Menu Panel (scrollable area for buttons)
            var pnlMenu = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.Transparent };

            // Stack correctly:
            sidebar.Controls.Add(pnlUser);
            sidebar.Controls.Add(pnlLogo);
            sidebar.Controls.Add(sepBottom);
            sidebar.Controls.Add(bCikis);
            sidebar.Controls.Add(pnlMenu);

            pnlMenu.Controls.Add(bProfil);
            pnlMenu.Controls.Add(bDuyuru);
            pnlMenu.Controls.Add(bBildirim);
            pnlMenu.Controls.Add(_btnBasvurularim);
            pnlMenu.Controls.Add(_btnDilekceYaz);
            pnlMenu.Controls.Add(_btnBorclar);
            pnlMenu.Controls.Add(_btnAnaSayfa);
            return sidebar;
        }

        private Button SidebarBtn(string text)
        {
            var b = new Button { Text = text, Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(b);
            return b;
        }

        private void SidebarSec(Button b)
        {
            if (_aktifBtn != null) UiTheme.SidebarButon(_aktifBtn);
            UiTheme.SidebarButon(b, secili: true);
            _aktifBtn = b;
        }

        private void VatandasHomeScreen_Load(object sender, EventArgs e) => GosterAnaSayfa();

        // ── ANA SAYFA ──────────────────────────────────────────────────────
        private void GosterAnaSayfa()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel(
                "  Hos Geldiniz" + (_adSoyad.Length > 0 ? ", " + _adSoyad : ""),
                "Belediye Vatandas Portali - Tum hizmetlere buradan ulasabilirsiniz.");

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 20, 24, 20), BackColor = UiTheme.Surface };

            BelediyeDbServisi.YukleGenelSayilar(out int toplamBasvuru, out int kayitliVatandas);
            int benimBasvuru = BelediyeDbServisi.VatandasBasvurulariniGetir(_tcKimlik).Rows.Count;
            int duyuruSayisi = 0;
            try { duyuruSayisi = BelediyeDbServisi.DuyurulariGetir().Rows.Count; } catch { }

            var tbl = new TableLayoutPanel { Dock = DockStyle.Top, Height = 115, ColumnCount = 4, RowCount = 1, Padding = new Padding(0, 0, 0, 12) };
            for (int i = 0; i < 4; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tbl.Controls.Add(UiTheme.OlusturStatKart("Kayitli Vatandas", kayitliVatandas.ToString(), UiTheme.Primary), 0, 0);
            tbl.Controls.Add(UiTheme.OlusturStatKart("Toplam Basvuru", toplamBasvuru.ToString(), UiTheme.Accent), 1, 0);
            tbl.Controls.Add(UiTheme.OlusturStatKart("Basvurularim", benimBasvuru.ToString(), UiTheme.Success), 2, 0);
            tbl.Controls.Add(UiTheme.OlusturStatKart("Aktif Duyuru", duyuruSayisi.ToString(), UiTheme.Info), 3, 0);
            pnlBody.Controls.Add(tbl);

            var pnlHizli = new Panel { Location = new Point(0, 125), Height = 56, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top, BackColor = UiTheme.Surface };
            var btnHizli = new Button { Text = "  + Yeni Dilekce / Basvuru Ac", Height = 44, Width = 250, Location = new Point(0, 6) };
            UiTheme.AnaEylemButonu(btnHizli);
            btnHizli.Click += (s, e) => { SidebarSec(_btnDilekceYaz); GosterDilekce(); };
            pnlHizli.Controls.Add(btnHizli);
            pnlBody.Controls.Add(pnlHizli);

            var lblDuy = new Label { Text = "  Son Duyurular", Font = UiTheme.LargeFont, ForeColor = UiTheme.TextPrimary, AutoSize = true, Location = new Point(0, 190) };
            pnlBody.Controls.Add(lblDuy);

            var dgvDuy = new DataGridView { Location = new Point(0, 222), Height = 220, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            dgvDuy.Width = _pnlIcerik.ClientSize.Width - 260;
            UiTheme.DataGridStil(dgvDuy);
            dgvDuy.AutoGenerateColumns = false;
            dgvDuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "YayinTarihi", HeaderText = "Tarih", Width = 150 });
            dgvDuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Baslik", HeaderText = "Baslik", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            try { dgvDuy.DataSource = BelediyeDbServisi.DuyurulariGetir(); } catch { }
            pnlBody.Controls.Add(dgvDuy);

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlBody);
            hdr.BringToFront();
            pnlBody.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── DILEKCE YAZ ────────────────────────────────────────────────────
        private void GosterDilekce()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("  Dilekce / Basvuru Yaz", "Yeni basvuru olusturun - Personel tarafindan incelenecektir");

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30, 20, 30, 0), BackColor = UiTheme.Surface };

            var tbl = new TableLayoutPanel { Dock = DockStyle.Top, Height = 380, ColumnCount = 1, RowCount = 7, BackColor = UiTheme.Surface };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 12f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var cmbKat = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = UiTheme.UiFont };
            UiTheme.ComboBoxStil(cmbKat);
            try
            {
                var dtKat = BelediyeDbServisi.KategorileriGetir();
                foreach (DataRow row in dtKat.Rows)
                {
                    if (row["KategoriAdi"] != null && row["KategoriAdi"] != DBNull.Value)
                        cmbKat.Items.Add(row["KategoriAdi"].ToString());
                }
            }
            catch { }
            if (cmbKat.Items.Count == 0)
            {
                cmbKat.Items.AddRange(new[] { "Imar & Yapi", "Sosyal Yardim", "Sikayet", "Temizlik & Cevre", "Ulasim", "Su & Altyapi", "Vergi & Ruhsat", "Evlilik & Nufus", "Diger" });
            }
            cmbKat.SelectedIndex = 0;

            var txtKon = new TextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, MaxLength = 200 };
            UiTheme.TextBoxAydinlikStil(txtKon);
            var rtbAc  = new RichTextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };

            tbl.Controls.Add(new Label { Text = "Kategori", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill }, 0, 0);
            tbl.Controls.Add(cmbKat, 0, 1);
            tbl.Controls.Add(new Label(), 0, 2);
            tbl.Controls.Add(new Label { Text = "Basvuru Konusu *", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill }, 0, 3);
            tbl.Controls.Add(txtKon, 0, 4);
            tbl.Controls.Add(new Label { Text = "Aciklama (istatege bagli)", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill }, 0, 5);
            tbl.Controls.Add(rtbAc, 0, 6);

            var pnlBtn = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.White, Padding = new Padding(0, 10, 0, 10) };
            pnlBtn.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle });
            var btnGon = new Button { Text = "  Dilekceyi Gonder", Dock = DockStyle.Right, Width = 200 };
            UiTheme.AnaEylemButonu(btnGon);
            var btnIp = new Button { Text = "Iptal", Dock = DockStyle.Right, Width = 110 };
            UiTheme.IkincilButon(btnIp);
            btnIp.Click += (s, e) => GosterAnaSayfa();
            btnGon.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtKon.Text)) {
                    MessageBox.Show("Konu bos birakilamaz.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKon.Focus(); return;
                }
                var hata = BelediyeDbServisi.VatandasBasvuruEkle(_tcKimlik, txtKon.Text, cmbKat.SelectedItem?.ToString(), rtbAc.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_tcKimlik, "Vatandas", "Basvuru gonderildi: " + txtKon.Text);
                MessageBox.Show("Basvurunuz alinmistir!", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SidebarSec(_btnBasvurularim);
                GosterBasvurular();
            };
            pnlBtn.Controls.AddRange(new Control[] { btnGon, btnIp });

            pnlBody.Controls.Add(tbl);
            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlBtn);
            pnl.Controls.Add(pnlBody);
            hdr.BringToFront();
            pnlBtn.BringToFront();
            pnlBody.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── BASVURULARIM ───────────────────────────────────────────────────
        private void GosterBasvurular()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("  Basvurularim", "Mevcut basvurulariniz, durumlar ve personel notlari");

            var pnlBot = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.White, Padding = new Padding(16, 10, 16, 10) };
            pnlBot.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle });
            var btnYeni = new Button { Text = "  + Yeni Basvuru", Dock = DockStyle.Left, Width = 190 };
            UiTheme.AnaEylemButonu(btnYeni);
            var btnYenile = new Button { Text = "Yenile", Dock = DockStyle.Right, Width = 100 };
            UiTheme.IkincilButon(btnYenile);
            pnlBot.Controls.AddRange(new Control[] { btnYeni, btnYenile });
            // pnlBot will be added at the end in the correct docking order

            // Detay paneli sag taraf
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 290, BackColor = Color.White, Padding = new Padding(14) };
            pnlDetay.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetay.Height);
            };
            var lblDetayBas = new Label { Text = "Basvuru Detayi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            int seciliBasvuruId = -1;
            var btnYazdir = new Button { Text = "🖨️ Dilekçeyi Yazdır", Dock = DockStyle.Bottom, Height = 40, Visible = false };
            UiTheme.AccentButon(btnYazdir);
            btnYazdir.Click += (s, e) => {
                if (seciliBasvuruId > 0)
                {
                    using (var f = new DilekceYazdirForm(seciliBasvuruId))
                        f.ShowDialog(this);
                }
            };
            var rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };

            pnlDetay.Controls.Add(rtbDetay);
            pnlDetay.Controls.Add(btnYazdir);
            pnlDetay.Controls.Add(lblDetayBas);
            // pnlDetay will be added at the end in the correct docking order

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "No", Width = 55 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Kategori", HeaderText = "Kategori", Width = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Konu", HeaderText = "Konu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KayitTarihi", HeaderText = "Tarih", Width = 130 });

            dgv.CellFormatting += (s, e) => {
                if (e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].DataPropertyName == "Durum" && e.Value != null) {
                    e.CellStyle.ForeColor = UiTheme.DurumRengi(e.Value.ToString());
                    e.CellStyle.Font = UiTheme.UiFontBold;
                }
            };

            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv) {
                    int id = Convert.ToInt32(drv["Id"]);
                    seciliBasvuruId = id;
                    btnYazdir.Visible = true;
                    var dt = BelediyeDbServisi.BasvuruDetayiGetir(id);
                    if (dt.Rows.Count > 0) {
                        var r = dt.Rows[0];
                        lblDetayBas.Text = r["Konu"]?.ToString();
                        string notlar = "";
                        var dtNot = BelediyeDbServisi.BasvuruNotlariniGetir(id);
                        foreach (DataRow nr in dtNot.Rows)
                            notlar += "[" + nr["EklenmeTarihi"] + "] " + nr["PersonelAdi"] + ":\n" + nr["Not"] + "\n\n";
                        rtbDetay.Text =
                            "Durum: " + r["Durum"] +
                            "\nKategori: " + r["Kategori"] +
                            "\nTarih: " + r["KayitTarihi"] +
                            "\n\nAciklama:\n" + r["Aciklama"] +
                            "\n\n--- Personel Notlari ---\n" +
                            (string.IsNullOrEmpty(notlar) ? "Henuz not eklenmedi." : notlar);
                    }
                }
            };

            Action yukle = () => {
                try { dgv.DataSource = BelediyeDbServisi.VatandasBasvurulariniGetir(_tcKimlik); }
                catch (Exception ex) { MessageBox.Show("Yuklenemedi: " + ex.Message); }
            };
            yukle();
            btnYenile.Click += (s, e) => yukle();
            btnYeni.Click   += (s, e) => GosterDilekce();

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlBot);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(dgv);
            hdr.BringToFront();
            pnlBot.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── DUYURULAR ──────────────────────────────────────────────────────
        private void GosterDuyurular()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("  Duyurular", "Belediyemizden guncel duyuru ve haberler");

            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 300, BackColor = Color.White, Padding = new Padding(16) };
            var lblDetayBas = new Label { Text = "Duyuru Icerigi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 28 };
            var rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            pnlDetay.Controls.Add(rtbDetay);
            pnlDetay.Controls.Add(lblDetayBas);

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "YayinTarihi", HeaderText = "Tarih", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Baslik", HeaderText = "Baslik", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            try { dgv.DataSource = BelediyeDbServisi.DuyurulariGetir(); } catch { }
            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv) {
                    rtbDetay.Text = drv["Icerik"]?.ToString();
                    lblDetayBas.Text = drv["Baslik"]?.ToString();
                }
            };

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(dgv);
            hdr.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── BILDIRIMLERIM ──────────────────────────────────────────────────
        private void GosterBildirimlerim()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("  Bildirimlerim", "Belediyeden şahsınıza veya tüm vatandaşlara gönderilen mesajlar");

            // Control panel
            var pnlControl = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.White, Padding = new Padding(8) };
            var btnTemizle = new Button { Text = "🗑️ Tüm Bildirimleri Temizle", Dock = DockStyle.Left, Width = 220 };
            UiTheme.TehlikeButon(btnTemizle);
            pnlControl.Controls.Add(btnTemizle);
            // pnlControl will be added at the end in the correct docking order

            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 300, BackColor = Color.White, Padding = new Padding(16) };
            var lblDetayBas = new Label { Text = "Bildirim İçeriği", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 28 };
            var rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            
            int seciliBildirimId = -1;
            var btnSil = new Button { Text = "❌ Bu Bildirimi Sil", Dock = DockStyle.Bottom, Height = 36, Visible = false };
            UiTheme.TehlikeButon(btnSil);
            
            pnlDetay.Controls.Add(rtbDetay);
            pnlDetay.Controls.Add(btnSil);
            pnlDetay.Controls.Add(lblDetayBas);

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GonderimTarihi", HeaderText = "Tarih", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Baslik", HeaderText = "Başlık", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GonderenKadi", HeaderText = "Gönderen", Width = 110 });

            Action yukle = () => {
                try {
                    // Try to load notifications. Citizen username is kadi or TC.
                    dgv.DataSource = BelediyeDbServisi.BildirimleriGetir(_tcKimlik);
                } catch { }
            };
            yukle();

            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv) {
                    seciliBildirimId = Convert.ToInt32(drv["Id"]);
                    rtbDetay.Text = drv["Icerik"]?.ToString();
                    lblDetayBas.Text = drv["Baslik"]?.ToString();
                    btnSil.Visible = true;
                }
            };

            btnSil.Click += (s, e) => {
                if (seciliBildirimId > 0) {
                    var err = BelediyeDbServisi.BildirimSil(seciliBildirimId);
                    if (err != null) MessageBox.Show(err);
                    else {
                        btnSil.Visible = false;
                        rtbDetay.Clear();
                        lblDetayBas.Text = "Bildirim İçeriği";
                        yukle();
                    }
                }
            };

            btnTemizle.Click += (s, e) => {
                if (MessageBox.Show("Tüm bildirimlerinizi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    var err = BelediyeDbServisi.TumBildirimleriTemizle(_tcKimlik);
                    if (err != null) MessageBox.Show(err);
                    else {
                        btnSil.Visible = false;
                        rtbDetay.Clear();
                        lblDetayBas.Text = "Bildirim İçeriği";
                        yukle();
                    }
                }
            };

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlControl);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(dgv);
            hdr.BringToFront();
            pnlControl.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── PROFIL ────────────────────────────────────────────────────────
        private void GosterProfil()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("  Profilim", "Kisisel bilgilerinizi guncelleyin ve şifrenizi değiştirin");

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40, 20, 40, 20), BackColor = UiTheme.Surface };
            
            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            pnlBody.Controls.Add(scroll);

            var kart = new Panel { BackColor = Color.White, Location = new Point(10, 10), Size = new Size(540, 520), Padding = new Padding(24) };
            kart.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0, 0), new Point(5, 0), UiTheme.Primary, UiTheme.PrimaryLight))
                    e.Graphics.FillRectangle(br, 0, 0, 5, kart.Height);
                using (var pen = new Pen(UiTheme.BorderCard, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, kart.Width - 1, kart.Height - 1);
            };
            scroll.Controls.Add(kart);

            // Fetch data
            string ad = "", soyad = "", email = "", kadi = "", telefon = "", adres = "";
            var dt = BelediyeDbServisi.VatandasTamProfilGetir(_tcKimlik);
            if (dt != null && dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                ad = r["ad"]?.ToString() ?? "";
                soyad = r["soyad"]?.ToString() ?? "";
                email = r["e_Mail"]?.ToString() ?? "";
                kadi = r["KullaniciAdi"]?.ToString() ?? "";
                telefon = r["telefon"]?.ToString() ?? "";
                adres = r["adres"]?.ToString() ?? "";
            }

            int py = 15;
            TextBox txtAd = ProfilInput(kart, "Ad", ad, ref py);
            TextBox txtSoyad = ProfilInput(kart, "Soyad", soyad, ref py);
            TextBox txtEmail = ProfilInput(kart, "E-Posta", email, ref py);
            TextBox txtKadi = ProfilInput(kart, "Kullanıcı Adı", kadi, ref py);
            TextBox txtTel = ProfilInput(kart, "Telefon", telefon, ref py);
            TextBox txtAdr = ProfilInput(kart, "Adres", adres, ref py, isMultiline: true);
            TextBox txtSifre = ProfilInput(kart, "Yeni Şifre (Değiştirmek istemiyorsanız boş bırakın)", "", ref py, isPassword: true);

            var btnKaydet = new Button { Text = "💾 Değişiklikleri Kaydet", Location = new Point(20, py + 10), Width = 500, Height = 44 };
            UiTheme.BasariButon(btnKaydet);
            kart.Controls.Add(btnKaydet);
            kart.Height = py + 75;

            btnKaydet.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text) || string.IsNullOrWhiteSpace(txtKadi.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Ad, Soyad, E-Posta ve Kullanıcı Adı alanları zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var err = BelediyeDbServisi.VatandasTamProfilGuncelle(_tcKimlik, txtAd.Text, txtSoyad.Text, txtEmail.Text, txtKadi.Text, txtTel.Text, txtAdr.Text, txtSifre.Text);
                if (err != null)
                {
                    MessageBox.Show("Hata: " + err, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    BelediyeDbServisi.SistemLoguEkle(txtKadi.Text, "Vatandas", "Profil bilgilerini güncelledi.");
                    MessageBox.Show("Profil bilgileriniz başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BelediyeDbServisi.TryGetKullaniciDisplayName(_tcKimlik, out _adSoyad);
                    GosterProfil();
                }
            };

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlBody);
            hdr.BringToFront();
            pnlBody.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }

        private TextBox ProfilInput(Panel parent, string labelText, string val, ref int y, bool isMultiline = false, bool isPassword = false)
        {
            var lbl = new Label { Text = labelText, Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Location = new Point(20, y), Width = 500, Height = 18 };
            parent.Controls.Add(lbl);
            y += 20;

            int height = isMultiline ? 60 : 30;
            var txt = new TextBox
            {
                Text = val,
                Location = new Point(20, y),
                Width = 500,
                Height = height,
                Multiline = isMultiline
            };
            UiTheme.TextBoxAydinlikStil(txt);
            if (isPassword) txt.UseSystemPasswordChar = true;
            parent.Controls.Add(txt);
            y += height + 15;
            return txt;
        }

        private Panel ProfilSatir(string etiket, string deger, ref int y)
        {
            var p = new Panel { Location = new Point(20, y), Size = new Size(420, 48) };
            p.Controls.Add(new Label { Text = etiket, Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, AutoSize = true, Location = new Point(0, 0) });
            p.Controls.Add(new Label { Text = deger, Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, AutoSize = true, Location = new Point(0, 18) });
            p.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = UiTheme.BorderSubtle });
            y += 56;
            return p;
        }

        // ── BORÇ VE VERGİ ÖDEME ──────────────────────────────────────────────
        private void GosterBorclar()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("  Borç ve Vergi Tahsilatı", "Belediyemize ait borçlarınızı ve faturalarınızı güvenle ödeyin");

            // Alt buton alanı
            var pnlBot = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.White, Padding = new Padding(16, 10, 16, 10) };
            pnlBot.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle });
            var btnYenile = new Button { Text = "Yenile", Dock = DockStyle.Left, Width = 100 };
            UiTheme.IkincilButon(btnYenile);
            pnlBot.Controls.Add(btnYenile);

            // Detay paneli sag taraf
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 290, BackColor = Color.White, Padding = new Padding(14) };
            pnlDetay.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetay.Height);
            };
            var lblDetayBas = new Label { Text = "Ödeme Detayı", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            int seciliBorcId = -1;
            decimal seciliBorcMiktar = 0;
            string seciliBorcAcik = "";

            var btnOde = new Button { Text = "💳 Borç Öde", Dock = DockStyle.Bottom, Height = 40, Visible = false };
            UiTheme.AnaEylemButonu(btnOde);

            var btnMakbuz = new Button { Text = "🖨️ Makbuz Yazdır", Dock = DockStyle.Bottom, Height = 40, Visible = false };
            UiTheme.AccentButon(btnMakbuz);
            
            var rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };

            pnlDetay.Controls.Add(rtbDetay);
            pnlDetay.Controls.Add(btnOde);
            pnlDetay.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 10 }); // Spacer
            pnlDetay.Controls.Add(btnMakbuz);
            pnlDetay.Controls.Add(lblDetayBas);

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "No", Width = 55 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Aciklama", HeaderText = "Borç / Fatura Türü", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Miktar", HeaderText = "Tutar (₺)", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SonOdemeTarihi", HeaderText = "Son Ödeme", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 100 });

            dgv.CellFormatting += (s, e) => {
                if (e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].DataPropertyName == "Durum" && e.Value != null) {
                    if (e.Value.ToString() == "Odendi")
                    {
                        e.CellStyle.ForeColor = UiTheme.Success;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = UiTheme.Danger;
                    }
                    e.CellStyle.Font = UiTheme.UiFontBold;
                }
            };

            Action yukle = () => {
                try { dgv.DataSource = BelediyeDbServisi.VatandasBorclariniGetir(_tcKimlik); }
                catch (Exception ex) { MessageBox.Show("Veriler yüklenemedi: " + ex.Message); }
                btnOde.Visible = false;
                btnMakbuz.Visible = false;
                rtbDetay.Clear();
                lblDetayBas.Text = "Ödeme Detayı";
            };

            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv) {
                    int id = Convert.ToInt32(drv["Id"]);
                    seciliBorcId = id;
                    seciliBorcMiktar = Convert.ToDecimal(drv["Miktar"]);
                    seciliBorcAcik = drv["Aciklama"]?.ToString();
                    string durum = drv["Durum"]?.ToString();

                    lblDetayBas.Text = seciliBorcAcik;
                    
                    if (durum == "Odenmedi")
                    {
                        btnOde.Visible = true;
                        btnMakbuz.Visible = false;
                        rtbDetay.Text = "Borç Türü: " + seciliBorcAcik +
                                        "\nToplam Tutar: " + seciliBorcMiktar.ToString("N2") + " ₺" +
                                        "\nSon Ödeme Tarihi: " + Convert.ToDateTime(drv["SonOdemeTarihi"]).ToString("dd.MM.yyyy") +
                                        "\n\nDurum: ÖDENMEDİ" +
                                        "\n\nÖdeme yapmak için alttaki butona tıklayın.";
                    }
                    else
                    {
                        btnOde.Visible = false;
                        btnMakbuz.Visible = true;
                        rtbDetay.Text = "Borç Türü: " + seciliBorcAcik +
                                        "\nÖdenen Tutar: " + seciliBorcMiktar.ToString("N2") + " ₺" +
                                        "\nÖdeme Tarihi: " + (drv["OdenmeTarihi"] != DBNull.Value ? Convert.ToDateTime(drv["OdenmeTarihi"]).ToString("dd.MM.yyyy HH:mm") : "-") +
                                        "\n\nDurum: ÖDENDİ (Tahsil Edildi)" +
                                        "\n\nResmi makbuzu görüntülemek ve yazdırmak için alttaki butona tıklayın.";
                    }
                }
            };

            btnOde.Click += (s, e) => {
                if (seciliBorcId > 0)
                {
                    using (var f = new OdemeForm(seciliBorcAcik, seciliBorcMiktar))
                    {
                        if (f.ShowDialog(this) == DialogResult.OK)
                        {
                            BelediyeDbServisi.BorcOde(seciliBorcId);
                            BelediyeDbServisi.SistemLoguEkle(_tcKimlik, "Vatandas", "Borç Ödendi: " + seciliBorcAcik + " (" + seciliBorcMiktar.ToString("N2") + " ₺)");
                            yukle();
                        }
                    }
                }
            };

            btnMakbuz.Click += (s, e) => {
                if (seciliBorcId > 0)
                {
                    using (var f = new MakbuzYazdirForm(seciliBorcId))
                    {
                        f.ShowDialog(this);
                    }
                }
            };

            btnYenile.Click += (s, e) => yukle();
            yukle();

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlBot);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(dgv);
            hdr.BringToFront();
            pnlBot.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }
    }
}
