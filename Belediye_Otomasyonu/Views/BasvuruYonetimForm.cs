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
            OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Text = "Başvuru Yönetimi";
            this.Size = new Size(1000, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            UiTheme.FormDizayn(this);

            // Header
            var hdr = UiTheme.OlusturHeaderPanel("  📋  Başvuru Yönetimi", "Tüm başvuruları inceleyin, durum değiştirin, not ekleyin");
            this.Controls.Add(hdr);

            // Filtre barı
            var pnlFiltre = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = Color.White };
            pnlFiltre.Paint += (s, e) => { using (var pen = new Pen(UiTheme.BorderSubtle)) e.Graphics.DrawLine(pen, 0, pnlFiltre.Height-1, pnlFiltre.Width, pnlFiltre.Height-1); };
            var cmbDurum = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Location = new Point(12, 12), Font = UiTheme.UiFont };
            cmbDurum.Items.AddRange(new[] { "Tümü", "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbDurum.SelectedIndex = 0;
            var cmbKat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, Location = new Point(152, 12), Font = UiTheme.UiFont };
            cmbKat.Items.AddRange(new[] { "Tümü", "Imar & Yapi", "Sosyal Yardim", "Sikayet", "Temizlik", "Ulasim", "Su & Altyapi", "Vergi & Ruhsat", "Evlilik & Nufus", "Diger" });
            cmbKat.SelectedIndex = 0;
            var txtAra = new TextBox { Width = 200, Location = new Point(314, 12), Font = UiTheme.UiFont, ForeColor = UiTheme.TextMuted, Text = "  Ara..." };
            txtAra.GotFocus  += (s, e) => { if (txtAra.Text.Trim() == "Ara...") { txtAra.Text = ""; txtAra.ForeColor = UiTheme.TextPrimary; } };
            txtAra.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txtAra.Text)) { txtAra.Text = "  Ara..."; txtAra.ForeColor = UiTheme.TextMuted; } };
            var btnFiltre = new Button { Text = "  Filtrele", Location = new Point(524, 8), Width = 100, Height = 36 };
            UiTheme.AnaEylemButonu(btnFiltre);
            var btnYenile = new Button { Text = "Yenile", Location = new Point(634, 8), Width = 90, Height = 36 };
            UiTheme.IkincilButon(btnYenile);
            var btnKapat = new Button { Text = "Kapat", Location = new Point(900, 8), Width = 90, Height = 36, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            UiTheme.IkincilButon(btnKapat);
            btnKapat.Click += (s, e) => this.Close();
            pnlFiltre.Controls.AddRange(new Control[] { cmbDurum, cmbKat, txtAra, btnFiltre, btnYenile, btnKapat });
            this.Controls.Add(pnlFiltre);

            // Detay paneli (alt)
            pnlDetay = new Panel { Dock = DockStyle.Bottom, Height = 210, BackColor = Color.White, Padding = new Padding(14), Visible = false };
            pnlDetay.Paint += (s, e) => { using (var pen = new Pen(UiTheme.BorderSubtle)) e.Graphics.DrawLine(pen, 0, 0, pnlDetay.Width, 0); };
            
            var btnKapatDetay = new Button { Text = "✖", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(950, 8), Width = 30, Height = 30, FlatStyle = FlatStyle.Flat, ForeColor = UiTheme.Danger, Font = UiTheme.SmallBold };
            btnKapatDetay.FlatAppearance.BorderSize = 0;
            btnKapatDetay.Cursor = Cursors.Hand;
            btnKapatDetay.Click += (s, e) => { pnlDetay.Visible = false; };

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

            // Kolon 2: Durum Güncelle (Orta)
            var pnlDurum = new Panel { Dock = DockStyle.Right, Width = 250, Padding = new Padding(20,0,0,0) };
            var lblDurBas = new Label { Text = "Durum Değiştir:", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Top, Height = 26 };
            var cmbYeniDur = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top, Font = UiTheme.UiFont, Height = 32 };
            cmbYeniDur.Items.AddRange(new[] { "Beklemede", "Islemde", "Tamamlandi", "Reddedildi" });
            cmbYeniDur.SelectedIndex = 0;
            var btnDurGun = new Button { Text = "Durumu Güncelle", Dock = DockStyle.Top, Height = 36 };
            UiTheme.AccentButon(btnDurGun);
            pnlDurum.Controls.Add(btnDurGun);
            pnlDurum.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8 });
            pnlDurum.Controls.Add(cmbYeniDur);
            pnlDurum.Controls.Add(lblDurBas);
            pnlDetay.Controls.Add(pnlDurum);

            // Kolon 1: Başvuru Detayı (Sol, Fill)
            var pnlDet = new Panel { Dock = DockStyle.Fill };
            lblDetBas = new Label { Text = "Başvuru Detayı", Font = UiTheme.UiFontBold, ForeColor = UiTheme.Primary, Dock = DockStyle.Top, Height = 26 };
            rtbDetay = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, Font = UiTheme.UiFont, BackColor = Color.White };
            pnlDet.Controls.Add(rtbDetay);
            pnlDet.Controls.Add(lblDetBas);
            pnlDetay.Controls.Add(pnlDet);

            btnDurGun.Click += (s, e) => {
                if (_seciliId < 0) { MessageBox.Show("Önce bir başvuru seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                var hata = BelediyeDbServisi.BasvuruDurumGuncelle(_seciliId, cmbYeniDur.SelectedItem.ToString());
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                BelediyeDbServisi.SistemLoguEkle(_oturumKullaniciAdi, "Personel", "Başvuru #" + _seciliId + " durumu: " + cmbYeniDur.SelectedItem);
                MessageBox.Show("Durum güncellendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Yukle(cmbDurum, cmbKat, txtAra);
            };
            btnNotEkle.Click += (s, e) => {
                if (_seciliId < 0 || string.IsNullOrWhiteSpace(txtNot.Text)) { MessageBox.Show("Başvuru seçin ve not yazın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                var hata = BelediyeDbServisi.BasvuruNotEkle(_seciliId, _oturumKullaniciAdi, txtNot.Text);
                if (hata != null) { MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                MessageBox.Show("Not eklendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNot.Clear();
            };

            pnlDetay.Controls.Add(btnKapatDetay);
            btnKapatDetay.BringToFront();
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
                    rtbDetay.Text = "Vatandaş: " + drv["VatandasAdi"] + "\nKategori: " + drv["Kategori"] +
                        "\nTarih: " + drv["KayitTarihi"] + "\n\n" + (string.IsNullOrEmpty(notlar) ? "Not yok." : "Notlar:\n" + notlar);
                }
            };
            this.Controls.Add(dgv);

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
                dgv.DataSource = BelediyeDbServisi.BasvuruListesiDetayliGetir(kat, dur, ara);
            } catch (Exception ex) { MessageBox.Show("Yüklenemedi: " + ex.Message); }
        }

        private void BasvuruYonetimForm_Load(object sender, EventArgs e) { }
        private void btnDurumDegistir_Click(object sender, EventArgs e) { }
        private void btnKapat_Click(object sender, EventArgs e) => this.Close();
    }
}
