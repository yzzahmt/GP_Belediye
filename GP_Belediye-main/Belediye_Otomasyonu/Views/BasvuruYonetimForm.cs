using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class BasvuruYonetimForm : Form
    {
        private readonly string _oturumKullaniciAdi;
        private DataGridView dgv;
        private Panel pnlDetay;
        private Label lblDetBas;
        private RichTextBox rtbDetay;
        private int _seciliId = -1;

        public BasvuruYonetimForm(string oturumKullaniciAdi)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Controls.Clear();
            bool isYonetici = BelediyeDbServisi.YoneticiMi(_oturumKullaniciAdi);

            this.Text = "Başvuru Yönetimi";
            this.Size = new Size(1220, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            UiTheme.FormDizayn(this);

            // Header
            var hdr = UiTheme.OlusturHeaderPanel("  📋  Başvuru Yönetimi", "Tüm başvuruları inceleyin, durum değiştirin, not ekleyin");
            this.Controls.Add(hdr);

            // Filtre barı
            var pnlFiltre = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = Color.White };
            pnlFiltre.Paint += (s, e) => { using (var pen = new Pen(UiTheme.BorderSubtle)) e.Graphics.DrawLine(pen, 0, pnlFiltre.Height-1, pnlFiltre.Width, pnlFiltre.Height-1); };
            var cmbDurum = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Location = new Point(12, 12), Font = UiTheme.UiFont };
            UiTheme.ComboBoxStil(cmbDurum);
            cmbDurum.Items.AddRange(new[] { "Tümü", "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbDurum.SelectedIndex = 0;
            var cmbKat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, Location = new Point(152, 12), Font = UiTheme.UiFont };
            UiTheme.ComboBoxStil(cmbKat);
            cmbKat.Items.AddRange(new[] { "Tümü", "Imar & Yapi", "Sosyal Yardim", "Sikayet", "Temizlik", "Ulasim", "Su & Altyapi", "Vergi & Ruhsat", "Evlilik & Nufus", "Diger" });
            cmbKat.SelectedIndex = 0;
            var txtAra = new TextBox { Width = 200, Location = new Point(314, 12), Font = UiTheme.UiFont, ForeColor = UiTheme.TextMuted, Text = "  Ara..." };
            txtAra.GotFocus  += (s, e) => { if (txtAra.Text.Trim() == "Ara...") { txtAra.Text = ""; txtAra.ForeColor = UiTheme.TextPrimary; } };
            txtAra.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txtAra.Text)) { txtAra.Text = "  Ara..."; txtAra.ForeColor = UiTheme.TextMuted; } };
            var btnFiltre = new Button { Text = "  Filtrele", Location = new Point(524, 10), Width = 100, Height = 32 };
            UiTheme.KucukYuvarlakButon(btnFiltre, UiTheme.Primary, Color.White, 4);
            var btnYenile = new Button { Text = "Yenile", Location = new Point(634, 10), Width = 90, Height = 32 };
            UiTheme.KucukIkincilButon(btnYenile);
            var btnKapat = new Button { Text = "Kapat", Location = new Point(1120, 10), Width = 90, Height = 32, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            UiTheme.KucukIkincilButon(btnKapat);
            btnKapat.Click += (s, e) => this.Close();
            pnlFiltre.Controls.AddRange(new Control[] { cmbDurum, cmbKat, txtAra, btnFiltre, btnYenile, btnKapat });
            this.Controls.Add(pnlFiltre);

            // Detay paneli (alt)
            pnlDetay = new Panel { Dock = DockStyle.Bottom, Height = 240, BackColor = Color.White, Padding = new Padding(14), Visible = false };
            pnlDetay.Paint += (s, e) => { using (var pen = new Pen(UiTheme.BorderSubtle)) e.Graphics.DrawLine(pen, 0, 0, pnlDetay.Width, 0); };

            // Kolon 3: Not Ekle (En sağ)
            var pnlNot = new Panel { Dock = DockStyle.Right, Width = 300, Padding = new Padding(20,0,0,0) };
            var lblNotBas = new Label { Text = "Not Ekle:", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Top, Height = 26 };
            var txtNot = new TextBox { Dock = DockStyle.Top, Height = 75, Multiline = true, BorderStyle = BorderStyle.FixedSingle, Font = UiTheme.UiFont };
            var btnNotEkle = new Button { Text = "Not Ekle", Dock = DockStyle.Top, Height = 36 };
            UiTheme.BasariButon(btnNotEkle);
            pnlNot.Controls.Add(btnNotEkle);
            pnlNot.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8 });
            pnlNot.Controls.Add(txtNot);
            pnlNot.Controls.Add(lblNotBas);
            pnlDetay.Controls.Add(pnlNot);

            // Kolon 2: Durum Güncelle (Orta-Sağ)
            var pnlDurum = new Panel { Dock = DockStyle.Right, Width = 220, Padding = new Padding(20,0,0,0) };
            var lblDurBas = new Label { Text = "Durum Değiştir:", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Top, Height = 26 };
            var cmbYeniDur = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top, Font = UiTheme.UiFont, Height = 32 };
            UiTheme.ComboBoxStil(cmbYeniDur);
            cmbYeniDur.Items.AddRange(new[] { "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbYeniDur.SelectedIndex = 0;
            var btnDurGun = new Button { Text = "Durumu Güncelle", Dock = DockStyle.Top, Height = 36 };
            UiTheme.AccentButon(btnDurGun);
            pnlDurum.Controls.Add(btnDurGun);
            pnlDurum.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8 });
            pnlDurum.Controls.Add(cmbYeniDur);
            pnlDurum.Controls.Add(lblDurBas);
            pnlDetay.Controls.Add(pnlDurum);

            // Kolon 4: Personel Ata (Personel ve Yönetici için)
            ComboBox cmbAtanDep = null;
            ComboBox cmbAtanPers = null;
            {
                var pnlAtama = new Panel { Dock = DockStyle.Right, Width = 260, Padding = new Padding(20,0,0,0) };
                var lblAtaBas = new Label { Text = "Personel Ata:", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Top, Height = 26 };
                cmbAtanDep = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top, Font = UiTheme.UiFont, Height = 32 };
                UiTheme.ComboBoxStil(cmbAtanDep);
                var pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 6 };
                cmbAtanPers = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top, Font = UiTheme.UiFont, Height = 32 };
                UiTheme.ComboBoxStil(cmbAtanPers);
                var pnlSpacer2 = new Panel { Dock = DockStyle.Top, Height = 8 };
                var btnAtaYap = new Button { Text = "✅ Personel Ata", Dock = DockStyle.Top, Height = 36 };
                UiTheme.AccentButon(btnAtaYap);

                pnlAtama.Controls.Add(btnAtaYap);
                pnlAtama.Controls.Add(pnlSpacer2);
                pnlAtama.Controls.Add(cmbAtanPers);
                pnlAtama.Controls.Add(pnlSpacer);
                pnlAtama.Controls.Add(cmbAtanDep);
                pnlAtama.Controls.Add(lblAtaBas);
                pnlDetay.Controls.Add(pnlAtama);

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

                btnAtaYap.Click += (s, e) => {
                    if (_seciliId < 0) { MessageBox.Show("Önce bir başvuru seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    if (cmbAtanPers.SelectedValue == null) { MessageBox.Show("Lütfen atanacak personeli seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    string depStr = cmbAtanDep.SelectedItem?.ToString();
                    if (depStr == "(Tümü)") depStr = "";
                    string persKadi = cmbAtanPers.SelectedValue.ToString();

                    var hata = BelediyeDbServisi.BasvuruPersonelAta(_seciliId, depStr, persKadi, _oturumKullaniciAdi);
                    if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    string rolLog = isYonetici ? "Yonetici" : "Personel";
                    BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, rolLog, "Başvuru #" + _seciliId + " atandı: " + persKadi);
                    MessageBox.Show("Başvuru başarıyla atandı ve personele bildirim gönderildi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Yukle(cmbDurum, cmbKat, txtAra);
                };
            }

            // Kolon 1: Başvuru Detayı (Sol, Fill)
            var pnlDet = new Panel { Dock = DockStyle.Fill };
            pnlDet.Width = 400;
            lblDetBas = new Label { Text = "Başvuru Detayı", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 26 };
            rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            
            var btnKapatDetay = new Button { Text = "✖", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(370, 2), Width = 26, Height = 26, FlatStyle = FlatStyle.Flat, ForeColor = UiTheme.Danger, Font = UiTheme.SmallBold };
            btnKapatDetay.FlatAppearance.BorderSize = 0;
            btnKapatDetay.Cursor = Cursors.Hand;
            btnKapatDetay.Click += (s, e) => { pnlDetay.Visible = false; };
            
            pnlDet.Controls.Add(btnKapatDetay);
            pnlDet.Controls.Add(rtbDetay);
            pnlDet.Controls.Add(lblDetBas);
            btnKapatDetay.BringToFront();
            pnlDetay.Controls.Add(pnlDet);

            btnDurGun.Click += (s, e) => {
                if (_seciliId < 0) { MessageBox.Show("Önce bir başvuru seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                var hata = BelediyeDbServisi.BasvuruDurumGuncelle(_seciliId, cmbYeniDur.SelectedItem.ToString());
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, isYonetici ? "Yonetici" : "Personel", "Başvuru #" + _seciliId + " durumu: " + cmbYeniDur.SelectedItem);
                MessageBox.Show("Durum güncellendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Yukle(cmbDurum, cmbKat, txtAra);
            };
            btnNotEkle.Click += (s, e) => {
                if (_seciliId < 0 || string.IsNullOrWhiteSpace(txtNot.Text)) { MessageBox.Show("Başvuru seçin ve not yazın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                var hata = BelediyeDbServisi.BasvuruNotEkle(_seciliId, _oturumKullaniciAdi, txtNot.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, isYonetici ? "Yonetici" : "Personel", "Not eklendi, Basvuru #" + _seciliId);
                MessageBox.Show("Not eklendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNot.Clear();
            };

            this.Controls.Add(pnlDetay);

            // Grid
            dgv = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.DataGridStil(dgv);
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "No", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VatandasAdi", HeaderText = "Vatandaş", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Kategori", HeaderText = "Kategori", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Konu", HeaderText = "Konu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KayitTarihi", HeaderText = "Tarih", Width = 120 });
            dgv.CellFormatting += (s, e) => {
                if (e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].DataPropertyName == "Durum" && e.Value != null) {
                    e.CellStyle.ForeColor = UiTheme.DurumRengi(e.Value.ToString());
                    e.CellStyle.Font = UiTheme.UiFontBold;
                }
            };
            dgv.SelectionChanged += (s, e) => {
                if (dgv.CurrentRow?.DataBoundItem is DataRowView drv) {
                    pnlDetay.Visible = true;
                    _seciliId = Convert.ToInt32(drv["Id"]);
                    cmbYeniDur.SelectedItem = drv["Durum"]?.ToString() ?? "Beklemede";
                    lblDetBas.Text = drv["Konu"]?.ToString();
                    var dtNot = BelediyeDbServisi.BasvuruNotlariniGetir(_seciliId);
                    string notlar = "";
                    foreach (DataRow nr in dtNot.Rows)
                        notlar += "[" + nr["EklenmeTarihi"] + "] " + nr["PersonelAdi"] + ":\n" + nr["Not"] + "\n\n";

                    string atananDetay = "";
                    if (drv["AtananDepartman"] != DBNull.Value && !string.IsNullOrWhiteSpace(drv["AtananDepartman"]?.ToString()))
                        atananDetay = "Atanan Bölüm: " + drv["AtananDepartman"] + "\n";
                    if (drv["AtananPersonelAdi"] != DBNull.Value && !string.IsNullOrWhiteSpace(drv["AtananPersonelAdi"]?.ToString()))
                        atananDetay += "Atanan Personel: " + drv["AtananPersonelAdi"] + "\n";

                    rtbDetay.Text = "Vatandaş: " + drv["VatandasAdi"] + "\nKategori: " + drv["Kategori"] +
                        "\nTarih: " + drv["KayitTarihi"] + "\n" + atananDetay + "\n" + (string.IsNullOrEmpty(notlar) ? "Not yok." : "Notlar:\n" + notlar);

                    if (cmbAtanDep != null && cmbAtanPers != null)
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
            this.Controls.Add(dgv);

            hdr.BringToFront();
            pnlFiltre.BringToFront();
            pnlDetay.BringToFront();
            dgv.SendToBack();

            Action yukleAc = () => Yukle(cmbDurum, cmbKat, txtAra);
            btnFiltre.Click  += (s, e) => yukleAc();
            btnYenile.Click  += (s, e) => yukleAc();
            this.Load += (s, e) => yukleAc();
        }

        private void Yukle(ComboBox cmbDur, ComboBox cmbKat, TextBox txtAra)
        {
            try {
                string dur = cmbDur.SelectedItem?.ToString();
                string kat = cmbKat.SelectedItem?.ToString();
                string ara = txtAra.Text.Trim() == "Ara..." ? null : txtAra.Text.Trim();
                string persFilter = BelediyeDbServisi.YoneticiMi(_oturumKullaniciAdi) ? null : _oturumKullaniciAdi;
                dgv.DataSource = BelediyeDbServisi.BasvuruListesiDetayliGetir(kat, dur, ara, persFilter);
            } catch (Exception ex) { MessageBox.Show("Yüklenemedi: " + ex.Message); }
        }

        private void BasvuruYonetimForm_Load(object sender, EventArgs e) { }
        private void btnDurumDegistir_Click(object sender, EventArgs e) { }
        private void btnKapat_Click(object sender, EventArgs e) => this.Close();
    }
}
