using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private System.Windows.Forms.Timer _chatTimer;
        private string _seciliSohbetKadi = "";

        public PersonelHomeScreen() : this("", false) { }

        public PersonelHomeScreen(string oturumKullaniciAdi, bool isYonetici = false)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            _isYonetici = isYonetici;
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            OlusturArayuz();
        }

        // Ana Arayüz
        private void OlusturArayuz()
        {
            this.Controls.Clear();

            // Sidebar
            var pnlSidebar = OlusturSidebar();
            this.Controls.Add(pnlSidebar);

            // İçerik alanı
            _pnlIcerik = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Surface
            };
            _pnlIcerik.Controls.Add(panelChart);
            _pnlIcerik.Controls.Add(tableStats);

            this.Controls.Add(_pnlIcerik);

            UiTheme.FormDizayn(this);
        }

        // Sidebar Oluştur
        private Panel OlusturSidebar()
        {
            var sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = UiTheme.SidebarBg };
            sidebar.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(0, sidebar.Height), UiTheme.SidebarBg, Color.FromArgb(17, 28, 59)))
                    e.Graphics.FillRectangle(br, sidebar.ClientRectangle);
            };

            var pnlLogo = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.Transparent };
            pnlLogo.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(pnlLogo.Width,0), UiTheme.PrimaryDark, Color.FromArgb(28, 37, 65)))
                    e.Graphics.FillRectangle(br, pnlLogo.ClientRectangle);
            };
            var sepLogo = new Panel { Dock = DockStyle.Bottom, Height = 3, BackColor = UiTheme.Accent };
            var lblLogo = new Label
            {
                Text = "  Belediye\nOtomasyonu",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0)
            };
            pnlLogo.Controls.Add(lblLogo);
            pnlLogo.Controls.Add(sepLogo);

            // Kullanıcı bilgi alanı
            var pnlUser = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70
            };
            string rol = _isYonetici ? "Yönetici" : "Personel";
            UiTheme.SidebarUserPaneli(pnlUser, _oturumKullaniciAdi, rol);

            // Cikis butonu
            var bCikis = new Button { Text = "  🚪  Çıkış Yap", Dock = DockStyle.Bottom, Height = 50 };
            UiTheme.SidebarButon(bCikis);
            bCikis.ForeColor = Color.FromArgb(220, 100, 100);
            bCikis.Click += (s, e) => { var g = new İlkGiris(); g.Show(); Close(); };

            var sep = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(30, 41, 59) };

            // Menu Panel (scrollable area for buttons)
            var pnlMenu = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.Transparent };

            // Stack controls in correct order to avoid overlapping:
            sidebar.Controls.Add(pnlUser);
            sidebar.Controls.Add(pnlLogo);
            sidebar.Controls.Add(sep);
            sidebar.Controls.Add(bCikis);
            sidebar.Controls.Add(pnlMenu);

            if (_isYonetici)
            {
                var bBildirimGonder = new Button { Text = "  📢  Bildirim Gönder", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bBildirimGonder);
                bBildirimGonder.Click += (s, e) => { using (var f = new BildirimGonderForm(_oturumKullaniciAdi)) f.ShowDialog(this); };
                pnlMenu.Controls.Add(bBildirimGonder);

                var bDuyuru = new Button { Text = "  📌  Duyuru Ekle", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bDuyuru);
                bDuyuru.Click += (s, e) => GosterDuyuruEkleDialog();
                pnlMenu.Controls.Add(bDuyuru);

                var bPersonel = new Button { Text = "  👥  Personel Yönetimi", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bPersonel);
                bPersonel.Click += (s, e) => { using (var f = new PersonelYonetimForm(_oturumKullaniciAdi)) f.ShowDialog(this); YukleDashboardVerileri(); };
                pnlMenu.Controls.Add(bPersonel);

                var bAnaliz = new Button { Text = "  📊  Analiz ve Raporlar", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bAnaliz);
                bAnaliz.Click += (s, e) => { SidebarSecimDegistir(bAnaliz); GosterAnalizRaporlar(); };
                pnlMenu.Controls.Add(bAnaliz);

                var bLoglar = new Button { Text = "  📜  Sistem Günlüğü", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bLoglar);
                bLoglar.Click += (s, e) => { SidebarSecimDegistir(bLoglar); GosterSistemGunlugu(); };
                pnlMenu.Controls.Add(bLoglar);

                var bFinans = new Button { Text = "  💳  Finans ve Tahsilat", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bFinans);
                bFinans.Click += (s, e) => { SidebarSecimDegistir(bFinans); GosterFinansYonetim(); };
                pnlMenu.Controls.Add(bFinans);

                var bVatandas = new Button { Text = "  🧑‍💼  Vatandaş Yönetimi", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bVatandas);
                bVatandas.Click += (s, e) => { using (var f = new VatandasYonetimForm(_oturumKullaniciAdi)) f.ShowDialog(this); YukleDashboardVerileri(); };
                pnlMenu.Controls.Add(bVatandas);
            }

            var bDilekce = new Button { Text = "  📨  Gelen Dilekce/Basvurular", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bDilekce);
            bDilekce.Click += (s, e) => { SidebarSecimDegistir(bDilekce); GosterGelenDilekceleri(); };
            pnlMenu.Controls.Add(bDilekce);

            var bBasvuru = new Button { Text = "  📋  Basvuru Yönetimi", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bBasvuru);
            bBasvuru.Click += (s, e) => { using (var f = new BasvuruYonetimForm(_oturumKullaniciAdi)) f.ShowDialog(this); YukleDashboardVerileri(); };
            pnlMenu.Controls.Add(bBasvuru);

            var bBildirim = new Button { Text = "  🔔  Bildirimler", Dock = DockStyle.Top, Height = 50 };
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
            pnlMenu.Controls.Add(bBildirim);

            var bGorevler = new Button { Text = "  📝  Görevlerim & İş Takibi", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bGorevler);
            bGorevler.Click += (s, e) =>
            {
                SidebarSecimDegistir(bGorevler);
                GosterPersonelGorevleri();
            };
            pnlMenu.Controls.Add(bGorevler);

            var bChat = new Button { Text = "  💬  Belediye İçi İletişim", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bChat);
            bChat.Click += (s, e) =>
            {
                SidebarSecimDegistir(bChat);
                GosterBelediyeChat();
            };
            pnlMenu.Controls.Add(bChat);

            var bProfil = new Button { Text = "  👤  Profilim", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bProfil);
            bProfil.Click += (s, e) =>
            {
                SidebarSecimDegistir(bProfil);
                GosterProfil();
            };
            pnlMenu.Controls.Add(bProfil);

            // Ana Sayfa
            var bAnaSayfa = new Button { Text = "  🏠  Ana Sayfa", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bAnaSayfa, secili: true);
            bAnaSayfa.Click += (s, e) =>
            {
                SidebarSecimDegistir(bAnaSayfa);
                GosterDashboard();
            };
            pnlMenu.Controls.Add(bAnaSayfa);
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

        // Dashboard
        private void GosterDashboard()
        {
            _pnlIcerik.Controls.Clear();
            _pnlIcerik.Controls.Add(panelChart);
            _pnlIcerik.Controls.Add(tableStats);
            YukleDashboardVerileri();
        }

        // Bildirimler Paneli
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

            pnl.Controls.Add(dgv);

            var header = UiTheme.OlusturHeaderPanel("🔔  Bildirimler", "Yöneticilerden gelen bildirimler");
            header.Dock = DockStyle.Top;
            pnl.Controls.Add(header);
            header.BringToFront();
            dgv.SendToBack();

            _pnlIcerik.Controls.Add(pnl);
        }

        // Gelen Dilekçeler Paneli
        private void GosterGelenDilekceleri()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };

            // Filtre araçları
            var pnlFiltre = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = Color.White, Padding = new Padding(10, 8, 10, 8) };
            pnlFiltre.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, pnlFiltre.Height-1, pnlFiltre.Width, pnlFiltre.Height-1);
            };
            var cmbDur = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Location = new Point(10, 10), Font = UiTheme.UiFont };
            UiTheme.ComboBoxStil(cmbDur);
            cmbDur.Items.AddRange(new[] { "Tümü", "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbDur.SelectedIndex = 0;
            var cmbKat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, Location = new Point(150, 10), Font = UiTheme.UiFont };
            UiTheme.ComboBoxStil(cmbKat);
            cmbKat.Items.AddRange(new[] { "Tümü", "Imar & Yapi", "Sosyal Yardim", "Sikayet", "Temizlik", "Ulasim", "Su & Altyapi", "Vergi & Ruhsat", "Evlilik & Nufus", "Diger" });
            cmbKat.SelectedIndex = 0;
            var txtAra = new TextBox { Width = 180, Location = new Point(310, 10), Font = UiTheme.UiFont, ForeColor = UiTheme.TextMuted, Text = "Ara..." };
            UiTheme.TextBoxAydinlikStil(txtAra);
            txtAra.GotFocus  += (s, e) => { if (txtAra.Text == "Ara...") { txtAra.Text = ""; txtAra.ForeColor = UiTheme.TextPrimary; } };
            txtAra.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txtAra.Text)) { txtAra.Text = "Ara..."; txtAra.ForeColor = UiTheme.TextMuted; } };
            var btnFiltre = new Button { Text = "Filtrele", Location = new Point(500, 10), Width = 90, Height = 32 };
            UiTheme.KucukYuvarlakButon(btnFiltre, UiTheme.Primary, Color.White, 4);
            var btnYenile2 = new Button { Text = "Yenile", Location = new Point(600, 10), Width = 80, Height = 32 };
            UiTheme.KucukIkincilButon(btnYenile2);
            pnlFiltre.Controls.AddRange(new Control[] { cmbDur, cmbKat, txtAra, btnFiltre, btnYenile2 });

            // Detay paneli sağ
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 310, BackColor = Color.White, Padding = new Padding(14), AutoScroll = true };
            pnlDetay.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle))
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetay.Height);
            };
            var lblDetBas = new Label { Text = "Basvuru Detayi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            var rtbDet = new RichTextBox { Height = 140, Dock = DockStyle.Top, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            var lblNot = new Label { Text = "Not Ekle:", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Top, Height = 24 };
            var txtNot = new TextBox { Dock = DockStyle.Top, Height = 50, Multiline = true, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };
            UiTheme.TextBoxAydinlikStil(txtNot);
            var btnNotEkle = new Button { Text = "  Not Ekle", Dock = DockStyle.Top, Height = 32 };
            UiTheme.BasariButon(btnNotEkle);

            var pnlDurBtn = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = Color.White };
            var cmbYeniDur = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 135, Location = new Point(0, 6), Font = UiTheme.UiFont };
            UiTheme.ComboBoxStil(cmbYeniDur);
            cmbYeniDur.Items.AddRange(new[] { "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbYeniDur.SelectedIndex = 0;
            var btnDurGun = new Button { Text = "Durumu Guncelle", Location = new Point(143, 4), Width = 138, Height = 32 };
            UiTheme.AccentButon(btnDurGun);
            pnlDurBtn.Controls.AddRange(new Control[] { cmbYeniDur, btnDurGun });

            var pnlSpace = new Panel { Dock = DockStyle.Top, Height = 6, BackColor = Color.White };

            // Kolon 4: Personel Ata (Yalnızca yöneticilere gösterilir)
            ComboBox cmbAtanDep = null;
            ComboBox cmbAtanPers = null;
            Button btnAtaYap = null;
            Panel pnlAtama = null;
            int seciliId = -1;

            if (_isYonetici)
            {
                pnlAtama = new Panel { Dock = DockStyle.Top, Height = 140, BackColor = Color.White };
                var lblAtaBas = new Label { Text = "Personel Ata:", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Location = new Point(0, 2), Height = 22, Width = 280 };
                cmbAtanDep = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, 26), Width = 280, Font = UiTheme.UiFont };
                UiTheme.ComboBoxStil(cmbAtanDep);
                cmbAtanPers = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, 56), Width = 280, Font = UiTheme.UiFont };
                UiTheme.ComboBoxStil(cmbAtanPers);
                btnAtaYap = new Button { Text = "Personel Ata", Location = new Point(0, 86), Width = 280, Height = 32 };
                UiTheme.AccentButon(btnAtaYap);

                pnlAtama.Controls.AddRange(new Control[] { lblAtaBas, cmbAtanDep, cmbAtanPers, btnAtaYap });

                // Departman değişince personelleri getir
                cmbAtanDep.SelectedIndexChanged += (s, e) => {
                    cmbAtanPers.DataSource = null;
                    cmbAtanPers.Items.Clear();
                    var depStr = cmbAtanDep.SelectedItem?.ToString();
                    var dtPers = BelediyeDbServisi.DepartmanPersonelleriniGetir(depStr);
                    cmbAtanPers.DataSource = dtPers;
                    cmbAtanPers.DisplayMember = "AdSoyad";
                    cmbAtanPers.ValueMember = "KullaniciAdi";
                };

                // Departman doldurma
                Action doldurDep = () => {
                    cmbAtanDep.Items.Clear();
                    cmbAtanDep.Items.Add("(Tümü)");
                    var dtDep = BelediyeDbServisi.DepartmanlariGetir();
                    foreach (DataRow row in dtDep.Rows)
                    {
                        if (row["Departman"] != null && row["Departman"] != DBNull.Value)
                            cmbAtanDep.Items.Add(row["Departman"].ToString());
                    }
                    cmbAtanDep.SelectedIndex = 0;
                };
                doldurDep();
            }

            btnNotEkle.Click += (s, e) => {
                if (seciliId < 0 || string.IsNullOrWhiteSpace(txtNot.Text)) { MessageBox.Show("Önce başvuru seçin ve not yazın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                var hata = BelediyeDbServisi.BasvuruNotEkle(seciliId, _oturumKullaniciAdi, txtNot.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, _isYonetici ? "Yonetici" : "Personel", "Not eklendi, Basvuru #" + seciliId);
                MessageBox.Show("Not eklendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNot.Clear();
            };
            btnDurGun.Click += (s, e) => {
                if (seciliId < 0) { MessageBox.Show("Önce başvuru seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string yD = cmbYeniDur.SelectedItem.ToString();
                var hata = BelediyeDbServisi.BasvuruDurumGuncelle(seciliId, yD);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, _isYonetici ? "Yonetici" : "Personel", "Basvuru #" + seciliId + " durumu: " + yD);
                MessageBox.Show("Durum güncellendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            var pnlSpace2 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.White };

            var btnYazdir = new Button { Text = "🖨️ Dilekçe/Rapor Yazdır", Dock = DockStyle.Top, Height = 36 };
            UiTheme.AccentButon(btnYazdir);
            btnYazdir.Click += (s, e) => {
                if (seciliId > 0)
                {
                    using (var f = new DilekceYazdirForm(seciliId))
                        f.ShowDialog(this);
                }
            };

            pnlDetay.Controls.Add(lblDetBas);
            pnlDetay.Controls.Add(rtbDet);
            if (_isYonetici && pnlAtama != null)
            {
                pnlDetay.Controls.Add(pnlAtama);
                pnlDetay.Controls.Add(pnlSpace);
            }
            pnlDetay.Controls.Add(pnlDurBtn);
            pnlDetay.Controls.Add(lblNot);
            pnlDetay.Controls.Add(txtNot);
            pnlDetay.Controls.Add(btnNotEkle);
            pnlDetay.Controls.Add(pnlSpace2);
            pnlDetay.Controls.Add(btnYazdir);

            // Grid
            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "No", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VatandasAdi", HeaderText = "Vatandaş", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Kategori", HeaderText = "Kategori", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Konu", HeaderText = "Konu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 105 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KayitTarihi", HeaderText = "Tarih", Width = 120 });
            dgv.CellFormatting += (s, e) => {
                if (e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].DataPropertyName == "Durum" && e.Value != null) {
                    e.CellStyle.ForeColor = UiTheme.DurumRengi(e.Value.ToString());
                    e.CellStyle.Font = UiTheme.UiFontBold;
                }
            };
            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv) {
                    seciliId = Convert.ToInt32(drv["Id"]);
                    cmbYeniDur.SelectedItem = drv["Durum"]?.ToString() ?? "Beklemede";
                    lblDetBas.Text = drv["Konu"]?.ToString();
                    string notlar = "";
                    var dtNot = BelediyeDbServisi.BasvuruNotlariniGetir(seciliId);
                    foreach (DataRow nr in dtNot.Rows)
                        notlar += "[" + nr["EklenmeTarihi"] + "] " + nr["PersonelAdi"] + ":\n" + nr["Not"] + "\n\n";

                    string atananDetay = "";
                    if (drv["AtananDepartman"] != DBNull.Value && !string.IsNullOrWhiteSpace(drv["AtananDepartman"]?.ToString()))
                        atananDetay = "Atanan Bölüm: " + drv["AtananDepartman"] + "\n";
                    if (drv["AtananPersonelAdi"] != DBNull.Value && !string.IsNullOrWhiteSpace(drv["AtananPersonelAdi"]?.ToString()))
                        atananDetay += "Atanan Personel: " + drv["AtananPersonelAdi"] + "\n";

                    rtbDet.Text = "Vatandas: " + drv["VatandasAdi"] + "\nTC: " + drv["VatandasTC"] +
                        "\nKategori: " + drv["Kategori"] + "\nTarih: " + drv["KayitTarihi"] + "\n" + atananDetay +
                        "\n\nNotlar:\n" + (string.IsNullOrEmpty(notlar) ? "Henüz not yok." : notlar);

                    if (_isYonetici && cmbAtanDep != null && cmbAtanPers != null)
                    {
                        string curDep = drv["AtananDepartman"]?.ToString();
                        string curPers = drv["AtananPersonelKadi"]?.ToString();
                        if (!string.IsNullOrEmpty(curDep) && cmbAtanDep.Items.Contains(curDep))
                            cmbAtanDep.SelectedItem = curDep;
                        else
                            cmbAtanDep.SelectedIndex = 0;

                        if (!string.IsNullOrEmpty(curPers))
                            cmbAtanPers.SelectedValue = curPers;
                    }
                }
            };

            Action yukle = () => {
                string dur = cmbDur.SelectedItem?.ToString();
                string kat = cmbKat.SelectedItem?.ToString();
                string ara = txtAra.Text == "Ara..." ? null : txtAra.Text;
                string persFilter = _isYonetici ? null : _oturumKullaniciAdi;
                try { dgv.DataSource = BelediyeDbServisi.BasvuruListesiDetayliGetir(kat, dur, ara, persFilter); }
                catch (Exception ex) { MessageBox.Show("Yüklenemedi: " + ex.Message); }
            };
            yukle();
            btnFiltre.Click  += (s, e) => yukle();
            btnYenile2.Click += (s, e) => yukle();

            if (_isYonetici && btnAtaYap != null)
            {
                btnAtaYap.Click += (s, e) => {
                    if (seciliId < 0) { MessageBox.Show("Önce başvuru seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    if (cmbAtanPers.SelectedValue == null) { MessageBox.Show("Lütfen atanacak personeli seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    string depStr = cmbAtanDep.SelectedItem?.ToString();
                    if (depStr == "(Tümü)") depStr = "";
                    string persKadi = cmbAtanPers.SelectedValue.ToString();

                    var hata = BelediyeDbServisi.BasvuruPersonelAta(seciliId, depStr, persKadi, _oturumKullaniciAdi);
                    if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, "Yonetici", "Başvuru #" + seciliId + " atandı: " + persKadi);
                    MessageBox.Show("Başvuru başarıyla atandı ve personele bildirim gönderildi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    yukle();
                };
            }

            var hdr2 = UiTheme.OlusturHeaderPanel("    Gelen Dilekçe ve Başvurular", "Vatandaşlardan gelen tüm başvurular - not ekle, durum değiştir");
            pnl.Controls.Add(hdr2);
            pnl.Controls.Add(pnlFiltre);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(dgv);
            hdr2.BringToFront();
            pnlFiltre.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();
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

        // Duyuru Ekleme Dialog
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

            var pnlF = new Panel { Dock = DockStyle.Bottom, Height = 58, BackColor = Color.White };
            var sepF  = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            var btnKaydet = new Button { Text = "  Yayınla", Dock = DockStyle.Right, Width = 120 };
            UiTheme.AccentButon(btnKaydet);
            var btnKapat = new Button { Text = "İptal", Dock = DockStyle.Right, Width = 100 };
            UiTheme.IkincilButon(btnKapat);
            btnKapat.Click += (s, e) => frm.Close();
            pnlF.Controls.AddRange(new Control[] { btnKaydet, btnKapat, sepF });
            frm.Controls.Add(pnlF);

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
            UiTheme.TextBoxAydinlikStil(txtB);
            var lblI = new Label { Text = "İçerik", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            var rtbI = new RichTextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };

            tbl.Controls.Add(lblB, 0, 0);
            tbl.Controls.Add(txtB, 0, 1);
            tbl.Controls.Add(lblI, 0, 2);
            tbl.Controls.Add(rtbI, 0, 3);
            frm.Controls.Add(tbl);

            frm.Controls.Add(UiTheme.OlusturHeaderPanel("📌 Yeni Duyuru Ekle"));
            tbl.BringToFront();

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

        // Load ve Veriler
        private void PersonelHomeScreen_Load(object sender, EventArgs e)
        {
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

        // ── Analiz ve Raporlar Sekmesi ───────────────────────────────────────
        private void GosterAnalizRaporlar()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };

            var hdr = UiTheme.OlusturHeaderPanel("📊 Analiz ve Raporlar", "Departman ve personel başarı oranları, talep istatistikleri");
            pnl.Controls.Add(hdr);

            // Export panel
            var pnlTools = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(10) };
            var btnExport = new Button { Text = "📥 Raporları Dışa Aktar (CSV)", Dock = DockStyle.Left, Width = 220 };
            UiTheme.BasariButon(btnExport);
            pnlTools.Controls.Add(btnExport);
            pnl.Controls.Add(pnlTools);

            var split = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(14),
                BackColor = UiTheme.Surface
            };
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            // Sol Panel: Departman Raporu
            var pnlDep = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12), Margin = new Padding(0, 0, 8, 0) };
            var lblDep = new Label { Text = "Departman Workload & Başarı Oranları", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            var dgvDep = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgvDep);
            pnlDep.Controls.Add(dgvDep);
            pnlDep.Controls.Add(lblDep);
            split.Controls.Add(pnlDep, 0, 0);

            // Sağ Panel: Personel Raporu
            var pnlPers = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12), Margin = new Padding(8, 0, 0, 0) };
            var lblPers = new Label { Text = "Personel Performans Analizi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            var dgvPers = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgvPers);
            pnlPers.Controls.Add(dgvPers);
            pnlPers.Controls.Add(lblPers);
            split.Controls.Add(pnlPers, 1, 0);

            pnl.Controls.Add(split);
            hdr.BringToFront();
            pnlTools.BringToFront();
            split.SendToBack();

            Action yukle = () =>
            {
                dgvDep.DataSource = BelediyeDbServisi.DepartmanBazliPerformansRaporu();
                dgvPers.DataSource = BelediyeDbServisi.PersonelBazliPerformansRaporu();
            };
            yukle();

            btnExport.Click += (s, e) =>
            {
                try
                {
                    using (var sfd = new SaveFileDialog { Filter = "CSV Files|*.csv", FileName = "Belediye_Performans_Raporu.csv" })
                    {
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            using (var sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                            {
                                sw.WriteLine("DEPARTMAN BAZLI PERFORMANS RAPORU");
                                sw.WriteLine("Departman;ToplamTalep;Tamamlanan;Islemdeki;Bekleyen;BasariOrani(%)");
                                var dt1 = (DataTable)dgvDep.DataSource;
                                foreach (DataRow row in dt1.Rows)
                                {
                                    sw.WriteLine($"{row["Departman"]};{row["ToplamTalep"]};{row["Tamamlanan"]};{row["Islemdeki"]};{row["Bekleyen"]};{row["BasariOrani"]}");
                                }
                                sw.WriteLine();
                                sw.WriteLine("PERSONEL BAZLI PERFORMANS RAPORU");
                                sw.WriteLine("Personel;Departman;ToplamTalep;Tamamlanan;BitirmeOrani(%)");
                                var dt2 = (DataTable)dgvPers.DataSource;
                                foreach (DataRow row in dt2.Rows)
                                {
                                    sw.WriteLine($"{row["Personel"]};{row["Departman"]};{row["ToplamTalep"]};{row["Tamamlanan"]};{row["BitirmeOrani"]}");
                                }
                            }
                            MessageBox.Show("Raporlar başarıyla dışa aktarıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            };

            _pnlIcerik.Controls.Add(pnl);
        }

        // ── Sistem Günlüğü (Audit Logs) Sekmesi ──────────────────────────────
        private void GosterSistemGunlugu()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };

            var hdr = UiTheme.OlusturHeaderPanel("📜 Sistem Günlüğü (Audit Logs)", "Veritabanı üzerinde gerçekleşen tüm kritik sistem hareketleri");
            pnl.Controls.Add(hdr);

            // Filtre Barı
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(10, 8, 10, 8) };
            var txtAra = new TextBox { Width = 180, Location = new Point(10, 14), Font = UiTheme.UiFont };
            UiTheme.TextBoxAydinlikStil(txtAra);
            txtAra.Text = "Ara...";
            txtAra.GotFocus += (s, e) => { if (txtAra.Text == "Ara...") txtAra.Text = ""; };
            txtAra.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txtAra.Text)) txtAra.Text = "Ara..."; };

            var btnAra = new Button { Text = "🔍 Filtrele", Location = new Point(200, 10), Width = 90, Height = 32 };
            UiTheme.AnaEylemButonu(btnAra);

            var btnTemizle = new Button { Text = "🗑️ Günlüğü Temizle", Location = new Point(300, 10), Width = 150, Height = 32 };
            UiTheme.TehlikeButon(btnTemizle);

            var btnExport = new Button { Text = "📥 Dışa Aktar", Location = new Point(460, 10), Width = 110, Height = 32 };
            UiTheme.IkincilButon(btnExport);

            pnlFilter.Controls.AddRange(new Control[] { txtAra, btnAra, btnTemizle, btnExport });
            pnl.Controls.Add(pnlFilter);

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Log ID", Width = 70 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KullaniciAdi", HeaderText = "Kullanıcı", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Rol", HeaderText = "Rol", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Islem", HeaderText = "İşlem / Hareket", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tarih", HeaderText = "Tarih", Width = 160 });

            pnl.Controls.Add(dgv);
            hdr.BringToFront();
            pnlFilter.BringToFront();
            dgv.SendToBack();

            Action yukle = () =>
            {
                string ara = txtAra.Text == "Ara..." ? "" : txtAra.Text;
                dgv.DataSource = BelediyeDbServisi.SistemLoglariGetir(ara);
            };
            yukle();

            btnAra.Click += (s, e) => yukle();
            btnTemizle.Click += (s, e) =>
            {
                if (MessageBox.Show("Tüm sistem loglarını kalıcı olarak silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var err = BelediyeDbServisi.SistemLoglariniTemizle();
                    if (err != null) MessageBox.Show(err);
                    else {
                        BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, "Yonetici", "Sistem loglarını sıfırladı.");
                        yukle();
                    }
                }
            };
            btnExport.Click += (s, e) =>
            {
                try
                {
                    using (var sfd = new SaveFileDialog { Filter = "CSV Files|*.csv", FileName = "Belediye_Sistem_Loglari.csv" })
                    {
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            using (var sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                            {
                                sw.WriteLine("LogID;Kullanici;Rol;Islem;Tarih");
                                var dt = (DataTable)dgv.DataSource;
                                foreach (DataRow row in dt.Rows)
                                {
                                    sw.WriteLine($"{row["Id"]};{row["KullaniciAdi"]};{row["Rol"]};{row["Islem"]};{row["Tarih"]}");
                                }
                            }
                            MessageBox.Show("Sistem logları başarıyla dışa aktarıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            };

            _pnlIcerik.Controls.Add(pnl);
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

        // ── FİNANS VE TAHSİLAT YÖNETİMİ ──────────────────────────────────────
        private void GosterFinansYonetim()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("    Finans ve Tahsilat Yönetimi", "Mükelleflerin borçlarını yönetin, yeni vergiler tanımlayın ve tahsilat istatistiklerini izleyin.");

            // 1. Stats Bar
            var pnlStats = new Panel { Dock = DockStyle.Top, Height = 90, Padding = new Padding(12, 10, 12, 10), BackColor = UiTheme.Surface };
            var tbl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            pnlStats.Controls.Add(tbl);

            // 2. Filter Bar
            var pnlFiltre = new Panel { Dock = DockStyle.Top, Height = 56, Padding = new Padding(12), BackColor = Color.White };
            pnlFiltre.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, 55, pnlFiltre.Width, 55);
            };
            var txtAra = new TextBox { Width = 180, Location = new Point(12, 13), Font = UiTheme.UiFont };
            UiTheme.TextBoxAydinlikStil(txtAra);
            var cmbDur = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120, Location = new Point(202, 13), Font = UiTheme.UiFont };
            UiTheme.ComboBoxStil(cmbDur);
            cmbDur.Items.AddRange(new[] { "Tümü", "Ödenenler", "Ödenmeyenler" });
            cmbDur.SelectedIndex = 0;
            var btnYenile = new Button { Text = "Yenile", Width = 80, Location = new Point(332, 11) };
            UiTheme.IkincilButon(btnYenile);

            pnlFiltre.Controls.AddRange(new Control[] { txtAra, cmbDur, btnYenile });

            // 3. Right Panel: Add Debt / Details
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 310, BackColor = Color.White, Padding = new Padding(14), AutoScroll = true };
            pnlDetay.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetay.Height);
            };

            var lblFormBas = new Label { Text = "Yeni Borç / Fatura Tanımla", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 25 };
            
            var lblTc = new Label { Text = "Vatandaş T.C. Kimlik:", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 18 };
            var txtTc = new TextBox { Dock = DockStyle.Top, Font = UiTheme.UiFont, MaxLength = 11, BorderStyle = BorderStyle.FixedSingle };
            UiTheme.TextBoxAydinlikStil(txtTc);
            
            var lblAcik = new Label { Text = "Açıklama / Gelir Kalemi:", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 18 };
            var txtAcik = new TextBox { Dock = DockStyle.Top, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };
            UiTheme.TextBoxAydinlikStil(txtAcik);
            
            var lblMiktar = new Label { Text = "Miktar (₺):", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 18 };
            var txtMiktar = new TextBox { Dock = DockStyle.Top, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };
            UiTheme.TextBoxAydinlikStil(txtMiktar);

            var lblTarih = new Label { Text = "Son Ödeme Tarihi:", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 18 };
            var dtpTarih = new DateTimePicker { Dock = DockStyle.Top, Font = UiTheme.UiFont, Format = DateTimePickerFormat.Short };

            var btnKaydet = new Button { Text = "💾 Borç Kaydet", Dock = DockStyle.Top, Height = 36 };
            UiTheme.BasariButon(btnKaydet);

            var pnlSep1 = new Panel { Dock = DockStyle.Top, Height = 20 };
            var pnlSep2 = new Panel { Dock = DockStyle.Top, Height = 10 };
            var pnlSep3 = new Panel { Dock = DockStyle.Top, Height = 10 };
            var pnlSep4 = new Panel { Dock = DockStyle.Top, Height = 10 };
            var pnlSep5 = new Panel { Dock = DockStyle.Top, Height = 10 };
            var pnlSep6 = new Panel { Dock = DockStyle.Top, Height = 25 };

            var lblMakbuzBas = new Label { Text = "Ödeme Detayı ve Makbuz", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 25 };
            var btnMakbuz = new Button { Text = "🖨️ Makbuz Yazdır", Dock = DockStyle.Top, Height = 36, Visible = false };
            UiTheme.AccentButon(btnMakbuz);

            pnlDetay.Controls.Add(btnMakbuz);
            pnlDetay.Controls.Add(lblMakbuzBas);
            pnlDetay.Controls.Add(pnlSep6);
            pnlDetay.Controls.Add(btnKaydet);
            pnlDetay.Controls.Add(pnlSep5);
            pnlDetay.Controls.Add(dtpTarih);
            pnlDetay.Controls.Add(lblTarih);
            pnlDetay.Controls.Add(pnlSep4);
            pnlDetay.Controls.Add(txtMiktar);
            pnlDetay.Controls.Add(lblMiktar);
            pnlDetay.Controls.Add(pnlSep3);
            pnlDetay.Controls.Add(txtAcik);
            pnlDetay.Controls.Add(lblAcik);
            pnlDetay.Controls.Add(pnlSep2);
            pnlDetay.Controls.Add(txtTc);
            pnlDetay.Controls.Add(lblTc);
            pnlDetay.Controls.Add(pnlSep1);
            pnlDetay.Controls.Add(lblFormBas);

            // 4. Grid
            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "No", Width = 55 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VatandasTC", HeaderText = "T.C. Kimlik", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VatandasAdi", HeaderText = "Mükellef", Width = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Aciklama", HeaderText = "Gelir Kalemi / Açıklama", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Miktar", HeaderText = "Tutar (₺)", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SonOdemeTarihi", HeaderText = "Son Ödeme", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 90 });

            dgv.CellFormatting += (s, e) => {
                if (e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].DataPropertyName == "Durum" && e.Value != null) {
                    if (e.Value.ToString() == "Odendi")
                        e.CellStyle.ForeColor = UiTheme.Success;
                    else
                        e.CellStyle.ForeColor = UiTheme.Danger;
                    e.CellStyle.Font = UiTheme.UiFontBold;
                }
            };

            int seciliBorcId = -1;

            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv) {
                    seciliBorcId = Convert.ToInt32(drv["Id"]);
                    string durum = drv["Durum"]?.ToString();
                    btnMakbuz.Visible = (durum == "Odendi");
                    lblMakbuzBas.Text = "Ödeme Detayı (#" + seciliBorcId + ")";
                }
                else
                {
                    btnMakbuz.Visible = false;
                    lblMakbuzBas.Text = "Ödeme Detayı ve Makbuz";
                }
            };

            tbl.Controls.Add(UiTheme.OlusturStatKart("Toplam Tahsilat (Gelir)", "0.00 ₺", UiTheme.Success), 0, 0);
            tbl.Controls.Add(UiTheme.OlusturStatKart("Bekleyen Belediye Alacağı", "0.00 ₺", UiTheme.Info), 1, 0);

            // Fetch the generated cards to dynamically update their values
            Action guncelleStatKartlari = () => {
                BelediyeDbServisi.FinansOzetiGetir(out decimal tahsilat, out decimal alacak);
                // Update table card label values
                foreach (Control c in tbl.Controls)
                {
                    if (c is Panel p)
                    {
                        foreach (Control cc in p.Controls)
                        {
                            if (cc is Label lbl && lbl.Name == "lblVal")
                            {
                                if (p.AccessibleDescription == "Toplam Tahsilat (Gelir)")
                                    lbl.Text = tahsilat.ToString("N2") + " ₺";
                                else
                                    lbl.Text = alacak.ToString("N2") + " ₺";
                            }
                        }
                    }
                }
            };

            // Set accessible descriptions for identification
            foreach (Control c in tbl.Controls)
            {
                if (c is Panel p)
                {
                    foreach (Control cc in p.Controls)
                    {
                        if (cc is Label lbl && lbl.Font.Size > 10)
                        {
                            lbl.Name = "lblVal";
                        }
                        if (cc is Label lblT && lblT.Font.Size <= 10)
                        {
                            p.AccessibleDescription = lblT.Text;
                        }
                    }
                }
            }

            Action yukleGrid = () => {
                try {
                    var dt = BelediyeDbServisi.TumBorclariGetirDetayli();
                    if (dt != null)
                    {
                        string filtre = "";
                        if (cmbDur.SelectedIndex == 1) filtre = "Durum='Odendi'";
                        else if (cmbDur.SelectedIndex == 2) filtre = "Durum='Odenmedi'";

                        string ara = txtAra.Text.Trim().Replace("'", "''");
                        if (!string.IsNullOrEmpty(ara))
                        {
                            string araFiltre = "(VatandasTC LIKE '%" + ara + "%' OR VatandasAdi LIKE '%" + ara + "%')";
                            if (!string.IsNullOrEmpty(filtre)) filtre += " AND " + araFiltre;
                            else filtre = araFiltre;
                        }

                        dt.DefaultView.RowFilter = filtre;
                        dgv.DataSource = dt;
                    }
                }
                catch { }
            };

            Action yenile = () => {
                yukleGrid();
                guncelleStatKartlari();
                txtTc.Clear();
                txtAcik.Clear();
                txtMiktar.Clear();
                dtpTarih.Value = DateTime.Today.AddDays(30);
            };

            btnYenile.Click += (s, e) => yenile();
            txtAra.TextChanged += (s, e) => yukleGrid();
            cmbDur.SelectedIndexChanged += (s, e) => yukleGrid();

            btnKaydet.Click += (s, e) => {
                string tc = txtTc.Text.Trim();
                string aciklama = txtAcik.Text.Trim();
                string miktarStr = txtMiktar.Text.Trim();
                DateTime tarihVal = dtpTarih.Value;

                if (tc.Length != 11 || string.IsNullOrEmpty(aciklama) || !decimal.TryParse(miktarStr, out decimal miktar) || miktar <= 0)
                {
                    MessageBox.Show("Lütfen TC Kimlik (11 haneli), Gelir Açıklaması ve geçerli bir Miktar giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var err = BelediyeDbServisi.BorcEkle(tc, aciklama, miktar, tarihVal);
                if (err != null)
                {
                    MessageBox.Show("Borç tanımlanamadı: " + err, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, "Yonetici", "Mükellefe borç tanımlandı: " + tc + " - " + aciklama + " (" + miktar.ToString("N2") + " ₺)");
                    MessageBox.Show("Borç kaydı başarıyla oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    yenile();
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

            yenile();

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlStats);
            pnl.Controls.Add(pnlFiltre);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(dgv);
            hdr.BringToFront();
            pnlStats.BringToFront();
            pnlFiltre.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();

            _pnlIcerik.Controls.Add(pnl);
        }

        // ── GÖREVLERİM VE İŞ TAKİBİ ──────────────────────────────────────────
        private void GosterPersonelGorevleri()
        {
            if (_chatTimer != null)
            {
                _chatTimer.Stop();
                _chatTimer.Dispose();
                _chatTimer = null;
            }

            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("📋 Görevlerim & İş Takibi", "Size atanan görevleri yönetin veya personellere yeni işler atayın.");

            // 1. Stats Bar
            var pnlStats = new Panel { Dock = DockStyle.Top, Height = 90, Padding = new Padding(12, 10, 12, 10), BackColor = UiTheme.Surface };
            var tbl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1 };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            pnlStats.Controls.Add(tbl);

            var stat1 = UiTheme.OlusturStatKart("Bekleyen Görevler", "0", UiTheme.Danger);
            var stat2 = UiTheme.OlusturStatKart("Devam Eden İşler", "0", UiTheme.Info);
            var stat3 = UiTheme.OlusturStatKart("Tamamlanan Görevler", "0", UiTheme.Success);
            tbl.Controls.Add(stat1, 0, 0);
            tbl.Controls.Add(stat2, 1, 0);
            tbl.Controls.Add(stat3, 2, 0);

            // Set AccessibleDescription for identifying panels
            stat1.AccessibleDescription = "Bekleyen Görevler";
            stat2.AccessibleDescription = "Devam Eden İşler";
            stat3.AccessibleDescription = "Tamamlanan Görevler";

            foreach (Control c in tbl.Controls)
            {
                if (c is Panel p)
                {
                    foreach (Control cc in p.Controls)
                    {
                        if (cc is Label lblVal && lblVal.Font.Size > 12)
                        {
                            lblVal.Name = "lblVal";
                        }
                    }
                }
            }

            // Helper to update stats labels
            Action<string, string, string> guncelleStats = (bStr, dStr, tStr) =>
            {
                foreach (Control c in tbl.Controls)
                {
                    if (c is Panel p)
                    {
                        foreach (Control cc in p.Controls)
                        {
                            if (cc is Label lblVal && lblVal.Name == "lblVal")
                            {
                                if (p.AccessibleDescription == "Bekleyen Görevler") lblVal.Text = bStr;
                                else if (p.AccessibleDescription == "Devam Eden İşler") lblVal.Text = dStr;
                                else if (p.AccessibleDescription == "Tamamlanan Görevler") lblVal.Text = tStr;
                            }
                        }
                    }
                }
            };

            // 2. Main Grid
            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 55 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Baslik", HeaderText = "Görev Başlığı", Width = 180 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AtananPersonelKadi", HeaderText = "Atanan Personel", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VerenKadi", HeaderText = "Görev Veren", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Oncelik", HeaderText = "Öncelik", Width = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OlusturmaTarihi", HeaderText = "Oluşturma Tarihi", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgv.CellFormatting += (s, e) => {
                if (e.ColumnIndex >= 0 && e.Value != null)
                {
                    string colName = dgv.Columns[e.ColumnIndex].DataPropertyName;
                    if (colName == "Oncelik")
                    {
                        string val = e.Value.ToString();
                        if (val == "Yüksek") e.CellStyle.ForeColor = UiTheme.Danger;
                        else if (val == "Orta") e.CellStyle.ForeColor = UiTheme.Warning;
                        else e.CellStyle.ForeColor = UiTheme.TextMuted;
                        e.CellStyle.Font = UiTheme.UiFontBold;
                    }
                    else if (colName == "Durum")
                    {
                        string val = e.Value.ToString();
                        if (val == "Tamamlandi") e.CellStyle.ForeColor = UiTheme.Success;
                        else if (val == "DevamEdiyor") e.CellStyle.ForeColor = UiTheme.Info;
                        else e.CellStyle.ForeColor = Color.DarkGray;
                        e.CellStyle.Font = UiTheme.UiFontBold;
                    }
                }
            };

            // 3. Right Sidebar Details Panel
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 320, BackColor = Color.White, Padding = new Padding(14), AutoScroll = true };
            pnlDetay.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetay.Height);
            };

            var lblGorevBas = new Label { Text = "Görev Detayları", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 25 };
            var rtbAciklama = new RichTextBox { Dock = DockStyle.Top, Height = 100, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.FromArgb(248, 250, 252) };
            
            var pnlSep1 = new Panel { Dock = DockStyle.Top, Height = 10 };
            
            // Staff controls
            var btnBaslat = new Button { Text = "▶️ Görevi Başlat", Dock = DockStyle.Top, Height = 36 };
            UiTheme.AccentButon(btnBaslat);
            var pnlSep2 = new Panel { Dock = DockStyle.Top, Height = 8 };
            var btnTamamla = new Button { Text = "✔️ Görevi Tamamla", Dock = DockStyle.Top, Height = 36 };
            UiTheme.BasariButon(btnTamamla);

            int seciliGorevId = -1;
            
            // Manager controls
            Panel pnlYoneticiForm = null;
            Button btnSil = null;

            if (_isYonetici)
            {
                btnSil = new Button { Text = "🗑️ Görevi Sil", Dock = DockStyle.Top, Height = 36 };
                UiTheme.TehlikeButon(btnSil);

                pnlYoneticiForm = new Panel { Dock = DockStyle.Top, Height = 340, BackColor = Color.White };
                var lblYeniBas = new Label { Text = "Yeni Görev Tanımla", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Secondary, Location = new Point(0, 15), Height = 25, Width = 280 };
                
                var lblT = new Label { Text = "Görev Başlığı:", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Location = new Point(0, 45), Height = 18, Width = 280 };
                var txtBaslik = new TextBox { Location = new Point(0, 63), Width = 280, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };
                UiTheme.TextBoxAydinlikStil(txtBaslik);

                var lblA = new Label { Text = "Görev Açıklaması:", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Location = new Point(0, 93), Height = 18, Width = 280 };
                var rtbA = new RichTextBox { Location = new Point(0, 111), Width = 280, Height = 60, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };

                var lblP = new Label { Text = "Personel Seç:", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Location = new Point(0, 181), Height = 18, Width = 280 };
                var cmbPers = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, 199), Width = 280, Font = UiTheme.UiFont };
                UiTheme.ComboBoxStil(cmbPers);

                var lblPr = new Label { Text = "Öncelik:", Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Location = new Point(0, 229), Height = 18, Width = 280 };
                var cmbPr = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, 247), Width = 280, Font = UiTheme.UiFont };
                UiTheme.ComboBoxStil(cmbPr);
                cmbPr.Items.AddRange(new[] { "Yüksek", "Orta", "Düşük" });
                cmbPr.SelectedIndex = 1;

                var btnKaydet = new Button { Text = "➕ Görev Ata", Location = new Point(0, 285), Width = 280, Height = 36 };
                UiTheme.BasariButon(btnKaydet);

                pnlYoneticiForm.Controls.AddRange(new Control[] { lblYeniBas, lblT, txtBaslik, lblA, rtbA, lblP, cmbPers, lblPr, cmbPr, btnKaydet });

                // Load users
                try
                {
                    var dtUsers = BelediyeDbServisi.TumKullanicilarListesiGetir("");
                    cmbPers.DataSource = dtUsers;
                    cmbPers.DisplayMember = "AdSoyad";
                    cmbPers.ValueMember = "KullaniciAdi";
                }
                catch { }

                btnKaydet.Click += (s, e) =>
                {
                    string bStr = txtBaslik.Text.Trim();
                    string aStr = rtbA.Text.Trim();
                    string target = cmbPers.SelectedValue?.ToString();
                    string priority = cmbPr.SelectedItem?.ToString();

                    if (string.IsNullOrEmpty(bStr) || string.IsNullOrEmpty(target))
                    {
                        MessageBox.Show("Lütfen Başlık girin ve Personel seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var err = BelediyeDbServisi.PersonelGoreviEkle(bStr, aStr, target, _oturumKullaniciAdi, priority);
                    if (err != null) MessageBox.Show(err);
                    else
                    {
                        MessageBox.Show("Görev başarıyla atandı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtBaslik.Clear();
                        rtbA.Clear();
                        // Refresh grid
                        dgv.DataSource = BelediyeDbServisi.PersonelGorevleriGetir(_oturumKullaniciAdi, _isYonetici);
                        UpdateStatsGorev(dgv, guncelleStats);
                    }
                };
            }

            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv)
                {
                    seciliGorevId = Convert.ToInt32(drv["Id"]);
                    lblGorevBas.Text = drv["Baslik"]?.ToString();
                    rtbAciklama.Text = drv["Aciklama"]?.ToString();
                    string durum = drv["Durum"]?.ToString();

                    btnBaslat.Enabled = (durum == "Baslanmadi");
                    btnTamamla.Enabled = (durum != "Tamamlandi");
                }
                else
                {
                    seciliGorevId = -1;
                    lblGorevBas.Text = "Görev Seçilmedi";
                    rtbAciklama.Clear();
                    btnBaslat.Enabled = false;
                    btnTamamla.Enabled = false;
                }
            };

            btnBaslat.Click += (s, e) => {
                if (seciliGorevId <= 0) return;
                var err = BelediyeDbServisi.PersonelGorevDurumuGuncelle(seciliGorevId, "DevamEdiyor");
                if (err != null) MessageBox.Show(err);
                else
                {
                    dgv.DataSource = BelediyeDbServisi.PersonelGorevleriGetir(_oturumKullaniciAdi, _isYonetici);
                    UpdateStatsGorev(dgv, guncelleStats);
                }
            };

            btnTamamla.Click += (s, e) => {
                if (seciliGorevId <= 0) return;
                var err = BelediyeDbServisi.PersonelGorevDurumuGuncelle(seciliGorevId, "Tamamlandi");
                if (err != null) MessageBox.Show(err);
                else
                {
                    dgv.DataSource = BelediyeDbServisi.PersonelGorevleriGetir(_oturumKullaniciAdi, _isYonetici);
                    UpdateStatsGorev(dgv, guncelleStats);
                }
            };

            if (_isYonetici && btnSil != null)
            {
                btnSil.Click += (s, e) => {
                    if (seciliGorevId <= 0) return;
                    if (MessageBox.Show("Seçili görevi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        var err = BelediyeDbServisi.PersonelGorevSil(seciliGorevId);
                        if (err != null) MessageBox.Show(err);
                        else
                        {
                            dgv.DataSource = BelediyeDbServisi.PersonelGorevleriGetir(_oturumKullaniciAdi, _isYonetici);
                            UpdateStatsGorev(dgv, guncelleStats);
                        }
                    }
                };
            }

            pnlDetay.Controls.Add(lblGorevBas);
            pnlDetay.Controls.Add(rtbAciklama);
            pnlDetay.Controls.Add(pnlSep1);
            pnlDetay.Controls.Add(btnBaslat);
            pnlDetay.Controls.Add(pnlSep2);
            pnlDetay.Controls.Add(btnTamamla);
            if (_isYonetici)
            {
                var pnlSep3 = new Panel { Dock = DockStyle.Top, Height = 8 };
                pnlDetay.Controls.Add(pnlSep3);
                pnlDetay.Controls.Add(btnSil);
                pnlDetay.Controls.Add(pnlYoneticiForm);
            }

            dgv.DataSource = BelediyeDbServisi.PersonelGorevleriGetir(_oturumKullaniciAdi, _isYonetici);
            UpdateStatsGorev(dgv, guncelleStats);

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlStats);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(dgv);

            hdr.BringToFront();
            pnlStats.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();

            _pnlIcerik.Controls.Add(pnl);
        }

        private void UpdateStatsGorev(DataGridView dgv, Action<string, string, string> updateAction)
        {
            int b = 0, d = 0, t = 0;
            if (dgv.DataSource is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string status = row["Durum"]?.ToString();
                    if (status == "Baslanmadi") b++;
                    else if (status == "DevamEdiyor") d++;
                    else if (status == "Tamamlandi") t++;
                }
            }
            updateAction(b.ToString(), d.ToString(), t.ToString());
        }

        // ── BELEDİYE İÇİ İLETİŞİM ─────────────────────────────────────────────
        private void GosterBelediyeChat()
        {
            if (_chatTimer != null)
            {
                _chatTimer.Stop();
                _chatTimer.Dispose();
                _chatTimer = null;
            }

            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            var hdr = UiTheme.OlusturHeaderPanel("💬 Belediye İçi İletişim", "Çalışma arkadaşlarınızla doğrudan mesajlaşın veya duyuru panosuna yazın.");

            var split = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = UiTheme.Surface
            };
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Left Side: Contacts
            var pnlContacts = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };
            pnlContacts.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, pnlContacts.Width - 1, 0, pnlContacts.Width - 1, pnlContacts.Height);
            };

            var lblContBas = new Label { Text = "Sohbet Listesi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            var lstContacts = new ListBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, BorderStyle = BorderStyle.None, ItemHeight = 35, DrawMode = DrawMode.OwnerDrawFixed };
            pnlContacts.Controls.Add(lstContacts);
            pnlContacts.Controls.Add(lblContBas);

            // Right Side: Chat Board
            var pnlChatMain = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface, Padding = new Padding(12) };
            
            var pnlChatHdr = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Color.White, Padding = new Padding(12) };
            pnlChatHdr.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, pnlChatHdr.Height - 1, pnlChatHdr.Width, pnlChatHdr.Height - 1);
            };
            var lblSeciliKullanici = new Label { Text = "Konuşma seçilmedi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Left, AutoSize = true };
            pnlChatHdr.Controls.Add(lblSeciliKullanici);

            var pnlInput = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.White, Padding = new Padding(8) };
            pnlInput.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, 0, pnlInput.Width, 0);
            };

            var txtMesaj = new TextBox { Width = 380, Location = new Point(10, 18), Font = UiTheme.UiFont };
            UiTheme.TextBoxAydinlikStil(txtMesaj);
            var btnGonder = new Button { Text = "Gönder ✉️", Location = new Point(400, 14), Width = 100, Height = 32 };
            UiTheme.AnaEylemButonu(btnGonder);
            pnlInput.Controls.AddRange(new Control[] { txtMesaj, btnGonder });

            var chatScroll = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(10),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            pnlChatMain.Controls.Add(chatScroll);
            pnlChatMain.Controls.Add(pnlChatHdr);
            pnlChatMain.Controls.Add(pnlInput);

            split.Controls.Add(pnlContacts, 0, 0);
            split.Controls.Add(pnlChatMain, 1, 0);

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(split);

            hdr.BringToFront();
            split.SendToBack();

            // Populate contacts
            lstContacts.Items.Clear();
            lstContacts.Items.Add(new ChatContact { Username = "GENEL", AdSoyad = "📢 Genel Duyuru Panosu", Detay = "Tüm Belediye" });
            try
            {
                var dtC = BelediyeDbServisi.TumKullanicilarListesiGetir(_oturumKullaniciAdi);
                foreach (DataRow row in dtC.Rows)
                {
                    lstContacts.Items.Add(new ChatContact
                    {
                        Username = row["KullaniciAdi"]?.ToString(),
                        AdSoyad = row["AdSoyad"]?.ToString(),
                        Detay = row["Detay"]?.ToString() + " (" + row["Rol"]?.ToString() + ")"
                    });
                }
            }
            catch { }

            // Owner Draw ListBox for Contacts to look extremely premium!
            lstContacts.DrawItem += (s, e) => {
                if (e.Index < 0) return;
                e.DrawBackground();
                var contact = (ChatContact)lstContacts.Items[e.Index];
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                using (var brushText = new SolidBrush(isSelected ? Color.White : UiTheme.TextPrimary))
                using (var brushSub = new SolidBrush(isSelected ? Color.FromArgb(210, 225, 245) : UiTheme.TextMuted))
                {
                    e.Graphics.DrawString(contact.AdSoyad, UiTheme.UiFontBold, brushText, e.Bounds.X + 8, e.Bounds.Y + 4);
                    e.Graphics.DrawString(contact.Detay, new Font("Segoe UI", 7.5f), brushSub, e.Bounds.X + 8, e.Bounds.Y + 20);
                }
                
                // Draw a separator line
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                {
                    e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
                }
                e.DrawFocusRectangle();
            };

            Action yukleMesajlar = () => {
                if (string.IsNullOrEmpty(_seciliSohbetKadi)) return;
                try
                {
                    var dtMsg = BelediyeDbServisi.MesajlariGetir(_oturumKullaniciAdi, _seciliSohbetKadi);
                    
                    // Don't recreate if message count is same to avoid flashing/jerkiness
                    if (chatScroll.Controls.Count == dtMsg.Rows.Count)
                    {
                        // Check if last message matches, if so, return
                        if (dtMsg.Rows.Count > 0)
                        {
                            var lastRow = dtMsg.Rows[dtMsg.Rows.Count - 1];
                            string lastText = lastRow["Mesaj"]?.ToString();
                            // Just a quick check
                            var lastPanel = chatScroll.Controls[chatScroll.Controls.Count - 1] as Panel;
                            if (lastPanel != null && lastPanel.Controls.Count > 0)
                            {
                                var innerPanel = lastPanel.Controls[0] as Panel;
                                if (innerPanel != null && innerPanel.Controls.Count > 0 && innerPanel.Controls[0] is Label lbl && lbl.Text == lastText)
                                    return;
                            }
                        }
                    }

                    chatScroll.Controls.Clear();
                    chatScroll.SuspendLayout();
                    foreach (DataRow row in dtMsg.Rows)
                    {
                        string sender = row["GonderenKadi"]?.ToString();
                        string msg = row["Mesaj"]?.ToString();
                        DateTime time = Convert.ToDateTime(row["Tarih"]);
                        bool isMe = sender == _oturumKullaniciAdi;

                        EkleMesajBalonu(chatScroll, sender, msg, time, isMe);
                    }
                    chatScroll.ResumeLayout();
                    chatScroll.PerformLayout();

                    // Scroll to bottom
                    if (chatScroll.Controls.Count > 0)
                    {
                        chatScroll.ScrollControlIntoView(chatScroll.Controls[chatScroll.Controls.Count - 1]);
                    }
                }
                catch { }
            };

            lstContacts.SelectedIndexChanged += (s, e) => {
                if (lstContacts.SelectedItem is ChatContact cc)
                {
                    _seciliSohbetKadi = cc.Username;
                    lblSeciliKullanici.Text = "Sohbet: " + cc.AdSoyad;
                    yukleMesajlar();
                }
            };

            // Set size of message text box dynamically
            pnlInput.Resize += (s, e) => {
                txtMesaj.Width = pnlInput.Width - btnGonder.Width - 30;
                btnGonder.Location = new Point(txtMesaj.Right + 10, btnGonder.Location.Y);
            };

            Action gonderAction = () => {
                string msg = txtMesaj.Text.Trim();
                if (string.IsNullOrEmpty(msg) || string.IsNullOrEmpty(_seciliSohbetKadi)) return;

                var err = BelediyeDbServisi.MesajGonder(_oturumKullaniciAdi, _seciliSohbetKadi, msg);
                if (err != null) MessageBox.Show(err);
                else
                {
                    txtMesaj.Clear();
                    yukleMesajlar();
                }
            };

            btnGonder.Click += (s, e) => gonderAction();
            txtMesaj.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    gonderAction();
                }
            };

            // Start timer for real-time polling
            _chatTimer = new System.Windows.Forms.Timer { Interval = 3000 }; // 3 seconds
            _chatTimer.Tick += (s, e) => {
                if (this.Visible && _aktifSidebarBtn != null && _aktifSidebarBtn.Text.Contains("İletişim"))
                {
                    yukleMesajlar();
                }
            };
            _chatTimer.Start();

            // Select General board by default
            if (lstContacts.Items.Count > 0)
            {
                lstContacts.SelectedIndex = 0;
            }

            _pnlIcerik.Controls.Add(pnl);
        }

        private void EkleMesajBalonu(FlowLayoutPanel pnl, string gonderen, string mesaj, DateTime tarih, bool isMe)
        {
            var pMsg = new Panel
            {
                Width = pnl.Width - 30,
                Height = 65,
                Margin = new Padding(3, 5, 3, 5),
                BackColor = Color.Transparent
            };

            var pBubble = new Panel
            {
                Width = (int)(pMsg.Width * 0.75),
                Height = 55,
                BackColor = isMe ? Color.FromArgb(220, 235, 252) : Color.FromArgb(243, 244, 246),
                Padding = new Padding(10),
                Location = new Point(isMe ? pMsg.Width - (int)(pMsg.Width * 0.75) - 5 : 5, 5)
            };

            pBubble.Paint += (s, e) =>
            {
                using (var pen = new Pen(isMe ? Color.FromArgb(170, 205, 240) : Color.FromArgb(220, 220, 220), 1))
                {
                    var rect = new Rectangle(0, 0, pBubble.Width - 1, pBubble.Height - 1);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            var lblText = new Label
            {
                Text = mesaj,
                Font = UiTheme.UiFont,
                ForeColor = Color.FromArgb(30, 41, 59),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft
            };

            var lblMeta = new Label
            {
                Text = (isMe ? "Ben" : gonderen) + " • " + tarih.ToString("HH:mm"),
                Font = new Font("Segoe UI", 7.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Bottom,
                Height = 15,
                TextAlign = isMe ? ContentAlignment.BottomRight : ContentAlignment.BottomLeft
            };

            pBubble.Controls.Add(lblText);
            pBubble.Controls.Add(lblMeta);
            pMsg.Controls.Add(pBubble);
            pnl.Controls.Add(pMsg);
            
            using (var g = pMsg.CreateGraphics())
            {
                var size = g.MeasureString(mesaj, lblText.Font, lblText.Width);
                if (size.Height > 30)
                {
                    pBubble.Height = (int)size.Height + 25;
                    pMsg.Height = pBubble.Height + 10;
                }
            }
        }

        private class ChatContact
        {
            public string Username { get; set; }
            public string AdSoyad { get; set; }
            public string Detay { get; set; }
        }

        // ── PROFİLİM ─────────────────────────────────────────────────────────
        private void GosterProfil()
        {
            if (_chatTimer != null) { _chatTimer.Stop(); _chatTimer.Dispose(); _chatTimer = null; }

            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            string rolLabel = _isYonetici ? "Yönetici" : "Personel";
            var hdr = UiTheme.OlusturHeaderPanel("  👤  Profilim", rolLabel + " bilgilerinizi güncelleyin ve şifrenizi değiştirin");

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40, 20, 40, 20), BackColor = UiTheme.Surface };
            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            pnlBody.Controls.Add(scroll);

            var kart = new Panel { BackColor = Color.White, Location = new Point(10, 10), Size = new Size(560, 540), Padding = new Padding(24) };
            kart.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0, 0), new Point(5, 0), UiTheme.Primary, UiTheme.PrimaryLight))
                    e.Graphics.FillRectangle(br, 0, 0, 5, kart.Height);
                using (var pen = new Pen(UiTheme.BorderCard, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, kart.Width - 1, kart.Height - 1);
            };
            scroll.Controls.Add(kart);

            // Veri çek
            string ad = "", soyad = "", email = "", telefon = "", adres = "", ekBilgi = "";
            DataTable dt = _isYonetici
                ? BelediyeDbServisi.YoneticiTamProfilGetir(_oturumKullaniciAdi)
                : BelediyeDbServisi.PersonelTamProfilGetir(_oturumKullaniciAdi);

            if (dt != null && dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                ad      = r["Ad"]?.ToString() ?? "";
                soyad   = r["Soyad"]?.ToString() ?? "";
                email   = r["Email"]?.ToString() ?? "";
                telefon = r["Telefon"]?.ToString() ?? "";
                adres   = r["Adres"]?.ToString() ?? "";
                ekBilgi = _isYonetici
                    ? (r["Unvan"]?.ToString() ?? "")
                    : (r["Departman"]?.ToString() ?? "");
            }

            int py = 15;
            TextBox txtAd      = ProfilInputPers(kart, "Ad *", ad, ref py);
            TextBox txtSoyad   = ProfilInputPers(kart, "Soyad *", soyad, ref py);
            TextBox txtEmail   = ProfilInputPers(kart, "E-Posta", email, ref py);
            string ekLabel     = _isYonetici ? "Unvan" : "Departman";
            TextBox txtEkBilgi = ProfilInputPers(kart, ekLabel + " (salt okunur)", ekBilgi, ref py, readOnly: true);
            TextBox txtTel     = ProfilInputPers(kart, "Telefon", telefon, ref py);
            TextBox txtAdr     = ProfilInputPers(kart, "Adres", adres, ref py, isMultiline: true);
            TextBox txtSifre   = ProfilInputPers(kart, "Yeni Şifre (boş bırakırsanız değişmez)", "", ref py, isPassword: true);

            var btnKaydet = new Button { Text = "💾 Değişiklikleri Kaydet", Location = new Point(20, py + 10), Width = 520, Height = 44 };
            UiTheme.BasariButon(btnKaydet);
            kart.Controls.Add(btnKaydet);
            kart.Height = py + 75;

            btnKaydet.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text))
                {
                    MessageBox.Show("Ad ve Soyad alanları zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string err = _isYonetici
                    ? BelediyeDbServisi.YoneticiTamProfilGuncelle(_oturumKullaniciAdi, txtAd.Text, txtSoyad.Text, txtEmail.Text, txtTel.Text, txtAdr.Text, txtSifre.Text)
                    : BelediyeDbServisi.PersonelTamProfilGuncelle(_oturumKullaniciAdi, txtAd.Text, txtSoyad.Text, txtEmail.Text, txtTel.Text, txtAdr.Text, txtSifre.Text);

                if (err != null)
                {
                    MessageBox.Show("Hata: " + err, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, rolLabel, "Profil bilgilerini güncelledi.");
                    MessageBox.Show("Profil bilgileriniz başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GosterProfil(); // Sayfayı yenile
                }
            };

            pnl.Controls.Add(hdr);
            pnl.Controls.Add(pnlBody);
            hdr.BringToFront();
            pnlBody.SendToBack();
            _pnlIcerik.Controls.Add(pnl);
        }

        private TextBox ProfilInputPers(Panel parent, string labelText, string val, ref int y,
            bool isMultiline = false, bool isPassword = false, bool readOnly = false)
        {
            var lbl = new Label { Text = labelText, Font = UiTheme.SmallBold, ForeColor = UiTheme.TextMuted, Location = new Point(20, y), Width = 520, Height = 18 };
            parent.Controls.Add(lbl);
            y += 20;

            int height = isMultiline ? 60 : 30;
            var txt = new TextBox
            {
                Text = val,
                Location = new Point(20, y),
                Width = 520,
                Height = height,
                Multiline = isMultiline,
                ReadOnly = readOnly
            };
            UiTheme.TextBoxAydinlikStil(txt);
            if (isPassword) txt.UseSystemPasswordChar = true;
            if (readOnly) txt.BackColor = Color.FromArgb(242, 244, 248);
            parent.Controls.Add(txt);
            y += height + 15;
            return txt;
        }
    }
}
