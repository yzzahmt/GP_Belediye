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

        public VatandasHomeScreen(string tcKimlik)
        {
            _tcKimlik = tcKimlik ?? "";
            InitializeComponent();
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

            _pnlIcerik = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            this.Controls.Add(_pnlIcerik);

            var sidebar = OlusturSidebar();
            this.Controls.Add(sidebar);
        }

        private Panel OlusturSidebar()
        {
            var sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = UiTheme.SidebarBg };
            sidebar.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(0, sidebar.Height),
                    UiTheme.SidebarBg, Color.FromArgb(12, 25, 65)))
                    e.Graphics.FillRectangle(br, sidebar.ClientRectangle);
            };

            // Logo
            var pnlLogo = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.Transparent };
            pnlLogo.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(pnlLogo.Width, 0),
                    UiTheme.PrimaryDark, Color.FromArgb(18, 45, 95)))
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
            sidebar.Controls.Add(pnlLogo);

            // Kullanici
            BelediyeDbServisi.TryGetKullaniciDisplayName(_tcKimlik, out _adSoyad);
            var pnlUser = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.Transparent };
            var lblUser = new Label {
                Text = "  " + (_adSoyad.Length > 0 ? _adSoyad : _tcKimlik) + "\n  Vatandas",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(160, 190, 230),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlUser.Controls.Add(lblUser);
            sidebar.Controls.Add(pnlUser);

            // Cikis
            var bCikis = new Button { Text = "  Cikis Yap", Dock = DockStyle.Bottom, Height = 50 };
            UiTheme.SidebarButon(bCikis);
            bCikis.ForeColor = Color.FromArgb(220, 90, 90);
            bCikis.Click += (s, e) => { new İlkGiris().Show(); Close(); };
            sidebar.Controls.Add(bCikis);
            sidebar.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(30, 60, 100) });

            // Menu butonlar
            Button bProfil  = SidebarBtn("    Profilim");
            Button bDuyuru  = SidebarBtn("    Duyurular");
            Button bBasvuru = SidebarBtn("    Basvurularim");
            Button bDilekce = SidebarBtn("    Dilekce Yaz");
            Button bAna     = SidebarBtn("    Ana Sayfa");

            bAna.Click     += (s, e) => { SidebarSec(bAna);     GosterAnaSayfa(); };
            bDilekce.Click += (s, e) => { SidebarSec(bDilekce); GosterDilekce(); };
            bBasvuru.Click += (s, e) => { SidebarSec(bBasvuru); GosterBasvurular(); };
            bDuyuru.Click  += (s, e) => { SidebarSec(bDuyuru);  GosterDuyurular(); };
            bProfil.Click  += (s, e) => { SidebarSec(bProfil);  GosterProfil(); };

            UiTheme.SidebarButon(bAna, secili: true);
            _aktifBtn = bAna;

            sidebar.Controls.Add(bProfil);
            sidebar.Controls.Add(bDuyuru);
            sidebar.Controls.Add(bBasvuru);
            sidebar.Controls.Add(bDilekce);
            sidebar.Controls.Add(bAna);
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
            btnHizli.Click += (s, e) => GosterDilekce();
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

            pnl.Controls.Add(pnlBody);
            pnl.Controls.Add(hdr);
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
            cmbKat.Items.AddRange(new[] { "Imar & Yapi", "Sosyal Yardim", "Sikayet", "Temizlik & Cevre", "Ulasim", "Su & Altyapi", "Vergi & Ruhsat", "Evlilik & Nufus", "Diger" });
            cmbKat.SelectedIndex = 0;

            var txtKon = new TextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, MaxLength = 200 };
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
                GosterBasvurular();
            };
            pnlBtn.Controls.AddRange(new Control[] { btnGon, btnIp });

            pnlBody.Controls.Add(tbl);
            pnl.Controls.Add(pnlBtn);
            pnl.Controls.Add(pnlBody);
            pnl.Controls.Add(hdr);
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
            pnl.Controls.Add(pnlBot);

            // Detay paneli sag taraf
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 290, BackColor = Color.White, Padding = new Padding(14) };
            pnlDetay.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetay.Height);
            };
            var lblDetayBas = new Label { Text = "Basvuru Detayi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            var rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            pnlDetay.Controls.Add(rtbDetay);
            pnlDetay.Controls.Add(lblDetayBas);
            pnl.Controls.Add(pnlDetay);

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

            pnl.Controls.Add(dgv);
            pnl.Controls.Add(hdr);
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

            pnl.Controls.Add(dgv);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(hdr);
            _pnlIcerik.Controls.Add(pnl);
        }

        // ── PROFIL ────────────────────────────────────────────────────────
        private void GosterProfil()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("  Profilim", "Kisisel bilgileriniz");

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40, 30, 40, 30), BackColor = UiTheme.Surface };
            var kart = new Panel { BackColor = Color.White, Location = new Point(0, 0), Size = new Size(480, 320), Padding = new Padding(28) };
            kart.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0, 0), new Point(5, 0), UiTheme.Primary, UiTheme.PrimaryLight))
                    e.Graphics.FillRectangle(br, 0, 0, 5, kart.Height);
                using (var pen = new Pen(UiTheme.BorderCard, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, kart.Width - 1, kart.Height - 1);
            };

            int y = 16;
            kart.Controls.Add(ProfilSatir("Ad Soyad", _adSoyad, ref y));
            kart.Controls.Add(ProfilSatir("TC Kimlik No", _tcKimlik, ref y));

            try {
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT e_Mail, KullaniciAdi FROM kullanicilar WHERE tc=@tc LIMIT 1", con)) {
                    cmd.Parameters.AddWithValue("@tc", _tcKimlik);
                    con.Open();
                    using (var r = cmd.ExecuteReader()) {
                        if (r.Read()) {
                            kart.Controls.Add(ProfilSatir("E-Posta", r.IsDBNull(0) ? "-" : r.GetString(0), ref y));
                            kart.Controls.Add(ProfilSatir("Kullanici Adi", r.IsDBNull(1) ? "-" : r.GetString(1), ref y));
                        }
                    }
                }
            } catch { }

            pnlBody.Controls.Add(kart);
            pnl.Controls.Add(pnlBody);
            pnl.Controls.Add(hdr);
            _pnlIcerik.Controls.Add(pnl);
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
    }
}
