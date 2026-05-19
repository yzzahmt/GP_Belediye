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

        // Sidebar buton referanslar� (aktif se�im i�in)
        private Button _aktifSidebarBtn;
        // ��erik paneli
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

        // �� Ana Aray�z �������������������������������������������������������
        private void OlusturArayuz()
        {
            tableUstBar.Visible = false;

            // ��erik alan�
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

        // �� Sidebar Olu�tur ��������������������������������������������������
        private Panel OlusturSidebar()
        {
            var sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = UiTheme.SidebarBg };
            sidebar.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(0, sidebar.Height), UiTheme.SidebarBg, Color.FromArgb(12,25,65)))
                    e.Graphics.FillRectangle(br, sidebar.ClientRectangle);
            };

            var pnlLogo = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.Transparent };
            pnlLogo.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(pnlLogo.Width,0), UiTheme.PrimaryDark, Color.FromArgb(18,45,95)))
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
            sidebar.Controls.Add(pnlLogo);

            // Kullan�c� bilgi alan�
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
            // Cikis butonu
            var bCikis = new Button { Text = "  ??  ��k�� Yap", Dock = DockStyle.Bottom, Height = 50 };
            UiTheme.SidebarButon(bCikis);
            bCikis.ForeColor = Color.FromArgb(220, 100, 100);
            bCikis.Click += (s, e) => { var g = new İlkGiris(); g.Show(); Close(); };
            sidebar.Controls.Add(bCikis);

            var sep = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(40, 60, 100) };
            sidebar.Controls.Add(sep);

            if (_isYonetici)
            {
                var bBildirimGonder = new Button { Text = "  📢  Bildirim Gönder", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bBildirimGonder);
                bBildirimGonder.Click += (s, e) => { using (var f = new BildirimGonderForm(_oturumKullaniciAdi)) f.ShowDialog(this); };
                sidebar.Controls.Add(bBildirimGonder);

                var bDuyuru = new Button { Text = "  📌  Duyuru Ekle", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bDuyuru);
                bDuyuru.Click += (s, e) => GosterDuyuruEkleDialog();
                sidebar.Controls.Add(bDuyuru);

                var bPersonel = new Button { Text = "  👥  Personel Yönetimi", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bPersonel);
                bPersonel.Click += (s, e) => { using (var f = new PersonelYonetimForm(_oturumKullaniciAdi)) f.ShowDialog(this); YukleDashboardVerileri(); };
                sidebar.Controls.Add(bPersonel);

                var bVatandas = new Button { Text = "  🧑‍💼  Vatandaş Yönetimi", Dock = DockStyle.Top, Height = 50 };
                UiTheme.SidebarButon(bVatandas);
                bVatandas.Click += (s, e) => { using (var f = new VatandasYonetimForm(_oturumKullaniciAdi)) f.ShowDialog(this); YukleDashboardVerileri(); };
                sidebar.Controls.Add(bVatandas);
            }

            var bDilekce = new Button { Text = "  📨  Gelen Dilekce/Basvurular", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bDilekce);
            bDilekce.Click += (s, e) => { SidebarSecimDegistir(bDilekce); GosterGelenDilekceleri(); };
            sidebar.Controls.Add(bDilekce);

            var bBasvuru = new Button { Text = "  📋  Basvuru Yönetimi", Dock = DockStyle.Top, Height = 50 };
            UiTheme.SidebarButon(bBasvuru);
            bBasvuru.Click += (s, e) => { using (var f = new BasvuruYonetimForm(_oturumKullaniciAdi)) f.ShowDialog(this); YukleDashboardVerileri(); };
            sidebar.Controls.Add(bBasvuru);

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
            sidebar.Controls.Add(bBildirim);

            // Ana Sayfa
            var bAnaSayfa = new Button { Text = "  🏠  Ana Sayfa", Dock = DockStyle.Top, Height = 50 };
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

        // �� Dashboard ��������������������������������������������������������
        private void GosterDashboard()
        {
            _pnlIcerik.Controls.Clear();
            _pnlIcerik.Controls.Add(panelChart);
            _pnlIcerik.Controls.Add(tableStats);
            YukleDashboardVerileri();
        }

        // �� Bildirimler Paneli ������������������������������������������������
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

            // WinForms docking: Fill �nce ekle, Top sonra ekle (ters i�lem s�ras�)
            pnl.Controls.Add(dgv);   // Fill � �nce ekle

            var header = UiTheme.OlusturHeaderPanel("??  Bildirimler", "Y�neticilerden gelen bildirimler");
            header.Dock = DockStyle.Top;
            pnl.Controls.Add(header); // Top � sonra ekle (�nce i�lenir, alan� al�r)

            _pnlIcerik.Controls.Add(pnl);
        }

        // �� Gelen Dilek�eler Paneli ������������������������������������������
        private void GosterGelenDilekceleri()
        {
            _pnlIcerik.Controls.Clear();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };

            // Filtre araclari
            var pnlFiltre = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = Color.White, Padding = new Padding(10, 8, 10, 8) };
            pnlFiltre.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle, 1))
                    e.Graphics.DrawLine(pen, 0, pnlFiltre.Height-1, pnlFiltre.Width, pnlFiltre.Height-1);
            };
            var cmbDur = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Location = new Point(10, 10), Font = UiTheme.UiFont };
            cmbDur.Items.AddRange(new[] { "Tum�", "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbDur.SelectedIndex = 0;
            var cmbKat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, Location = new Point(150, 10), Font = UiTheme.UiFont };
            cmbKat.Items.AddRange(new[] { "Tum�", "Imar & Yapi", "Sosyal Yardim", "Sikayet", "Temizlik", "Ulasim", "Su & Altyapi", "Vergi & Ruhsat", "Evlilik & Nufus", "Diger" });
            cmbKat.SelectedIndex = 0;
            var txtAra = new TextBox { Width = 180, Location = new Point(310, 10), Font = UiTheme.UiFont, ForeColor = UiTheme.TextMuted, Text = "Ara..." };
            txtAra.GotFocus  += (s, e) => { if (txtAra.Text == "Ara...") { txtAra.Text = ""; txtAra.ForeColor = UiTheme.TextPrimary; } };
            txtAra.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txtAra.Text)) { txtAra.Text = "Ara..."; txtAra.ForeColor = UiTheme.TextMuted; } };
            var btnFiltre = new Button { Text = "Filtrele", Location = new Point(500, 8), Width = 90, Height = 32 };
            UiTheme.AnaEylemButonu(btnFiltre);
            var btnYenile2 = new Button { Text = "Yenile", Location = new Point(600, 8), Width = 80, Height = 32 };
            UiTheme.IkincilButon(btnYenile2);
            pnlFiltre.Controls.AddRange(new Control[] { cmbDur, cmbKat, txtAra, btnFiltre, btnYenile2 });

            // Detay paneli sag
            var pnlDetay = new Panel { Dock = DockStyle.Right, Width = 310, BackColor = Color.White, Padding = new Padding(14) };
            pnlDetay.Paint += (s, e) => {
                using (var pen = new Pen(UiTheme.BorderSubtle))
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetay.Height);
            };
            var lblDetBas = new Label { Text = "Basvuru Detayi", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 30 };
            var rtbDet = new RichTextBox { Height = 200, Dock = DockStyle.Top, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            var lblNot = new Label { Text = "Not Ekle:", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Top, Height = 24 };
            var txtNot = new TextBox { Dock = DockStyle.Top, Height = 70, Multiline = true, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle };
            var pnlDurBtn = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
            var cmbYeniDur = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 135, Location = new Point(0, 6), Font = UiTheme.UiFont };
            cmbYeniDur.Items.AddRange(new[] { "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbYeniDur.SelectedIndex = 0;
            var btnDurGun = new Button { Text = "Durumu Guncelle", Location = new Point(143, 4), Width = 155, Height = 36 };
            UiTheme.AccentButon(btnDurGun);
            var btnNotEkle = new Button { Text = "  Not Ekle", Dock = DockStyle.Top, Height = 38 };
            UiTheme.BasariButon(btnNotEkle);
            pnlDurBtn.Controls.AddRange(new Control[] { cmbYeniDur, btnDurGun });

            int seciliId = -1;
            btnNotEkle.Click += (s, e) => {
                if (seciliId < 0 || string.IsNullOrWhiteSpace(txtNot.Text)) { MessageBox.Show("Once basvuru secin ve not yazin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                var hata = BelediyeDbServisi.BasvuruNotEkle(seciliId, _oturumKullaniciAdi, txtNot.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, _isYonetici?"Yonetici":"Personel", "Not eklendi, Basvuru #" + seciliId);
                MessageBox.Show("Not eklendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNot.Clear();
            };
            btnDurGun.Click += (s, e) => {
                if (seciliId < 0) { MessageBox.Show("Once basvuru secin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string yD = cmbYeniDur.SelectedItem.ToString();
                var hata = BelediyeDbServisi.BasvuruDurumGuncelle(seciliId, yD);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, _isYonetici?"Yonetici":"Personel", "Basvuru #" + seciliId + " durumu: " + yD);
                MessageBox.Show("Durum guncellendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            pnlDetay.Controls.Add(btnNotEkle);
            pnlDetay.Controls.Add(txtNot);
            pnlDetay.Controls.Add(lblNot);
            pnlDetay.Controls.Add(pnlDurBtn);
            pnlDetay.Controls.Add(rtbDet);
            pnlDetay.Controls.Add(lblDetBas);

            // Grid
            var dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "No", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VatandasAdi", HeaderText = "Vatandas", Width = 140 });
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
                    rtbDet.Text = "Vatandas: " + drv["VatandasAdi"] + "\nTC: " + drv["VatandasTC"] +
                        "\nKategori: " + drv["Kategori"] + "\nTarih: " + drv["KayitTarihi"] +
                        "\n\nNotlar:\n" + (string.IsNullOrEmpty(notlar) ? "Henuz not yok." : notlar);
                }
            };

            Action yukle = () => {
                string dur = cmbDur.SelectedItem?.ToString();
                string kat = cmbKat.SelectedItem?.ToString();
                string ara = txtAra.Text == "Ara..." ? null : txtAra.Text;
                try { dgv.DataSource = BelediyeDbServisi.BasvuruListesiDetayliGetir(kat, dur, ara); }
                catch (Exception ex) { MessageBox.Show("Yuklenemedi: " + ex.Message); }
            };
            yukle();
            btnFiltre.Click  += (s, e) => yukle();
            btnYenile2.Click += (s, e) => yukle();

            pnl.Controls.Add(dgv);
            pnl.Controls.Add(pnlDetay);
            pnl.Controls.Add(pnlFiltre);
            var hdr2 = UiTheme.OlusturHeaderPanel("    Gelen Dilekce ve Basvurular", "Vatandaslardan gelen tum basvurular - not ekle, durum degistir");
            pnl.Controls.Add(hdr2);
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

        // �� Duyuru Ekleme Dialog ����������������������������������������������
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

            // 1. Footer � �NCE ekle
            var pnlF = new Panel { Dock = DockStyle.Bottom, Height = 58, BackColor = Color.White };
            var sepF  = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            var btnKaydet = new Button { Text = "  Yay�nla", Dock = DockStyle.Right, Width = 120 };
            UiTheme.AccentButon(btnKaydet);
            var btnKapat = new Button { Text = "İptal", Dock = DockStyle.Right, Width = 100 };
            UiTheme.IkincilButon(btnKapat);
            btnKapat.Click += (s, e) => frm.Close();
            pnlF.Controls.AddRange(new Control[] { btnKaydet, btnKapat, sepF });
            frm.Controls.Add(pnlF);

            // 2. Body � TableLayoutPanel (Fill)
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

            // 3. Header � SON ekle (WinForms'ta son eklenen Top �nce i�lenir)
            frm.Controls.Add(UiTheme.OlusturHeaderPanel("📌 Yeni Duyuru Ekle"));
            tbl.BringToFront();

            btnKaydet.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtB.Text))
                { MessageBox.Show("Ba�l�k bo� b�rak�lamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtB.Focus(); return; }
                var hata = BelediyeDbServisi.DuyuruEkle(txtB.Text, rtbI.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                MessageBox.Show("Duyuru yay�nland�.", "Ba�ar�l�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frm.Close();
            };

            frm.ShowDialog(this);
        }

        // �� Load ve Veriler ���������������������������������������������������
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
            // Kart border sol �izgisi
            cardVatandas.Paint += (s, ea) => ea.Graphics.FillRectangle(new SolidBrush(UiTheme.Primary), 0, 0, 4, cardVatandas.Height);
            cardPersonel.Paint += (s, ea) => ea.Graphics.FillRectangle(new SolidBrush(UiTheme.Secondary), 0, 0, 4, cardPersonel.Height);
            cardBasvuru.Paint  += (s, ea) => ea.Graphics.FillRectangle(new SolidBrush(UiTheme.Accent), 0, 0, 4, cardBasvuru.Height);

            this.Text = _isYonetici
                ? $"Belediye Y�netim Sistemi � Y�netici: {_oturumKullaniciAdi}"
                : $"Belediye Y�netim Sistemi � Personel: {_oturumKullaniciAdi}";

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
                lblChartTitle.Text = "Ba�vuru Durumlar� � Anl�k �zet";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "�zet y�klenemedi. MySQL ve `belediye` veritaban� haz�r m�?\n\n" + ex.Message,
                    "Veritaban�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

