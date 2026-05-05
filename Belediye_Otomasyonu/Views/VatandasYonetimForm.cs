using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class VatandasYonetimForm : Form
    {
        private readonly string _oturumKullaniciAdi;
        private Panel _pnlDuzenle;
        private TextBox _txtAd, _txtSoyad, _txtEmail;
        private Label _lblSeciliTC;
        private int _seciliId = -1;

        public VatandasYonetimForm(string oturumKullaniciAdi)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            InitializeComponent();
            UiTheme.FormDizayn(this);
            OlusturDuzenlePanel();
            OlusturHeader();
            UiTheme.TehlikeButon(btnSil);
            UiTheme.IkincilButon(btnKapat);
        }

        private void OlusturHeader()
        {
            panelTop.BackColor = UiTheme.HeaderBg;
            panelTop.Height = 85;

            var sep = new Panel { Dock = DockStyle.Bottom, Height = 3, BackColor = UiTheme.Accent };
            panelTop.Controls.Add(sep);

            lblTitle.Text = "🧑‍💼  Vatandaş Yönetimi";
            lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            var lblAlt = new Label
            {
                Text = "Vatandaş listesi — Ad, Soyad ve E-posta bilgileri düzenlenebilir. TC kimlik değiştirilemez.",
                Font = UiTheme.SmallFont,
                ForeColor = Color.FromArgb(180, 200, 230),
                AutoSize = true,
                Location = new Point(17, 46)
            };
            panelTop.Controls.Add(lblAlt);
        }

        private void OlusturDuzenlePanel()
        {
            _pnlDuzenle = new Panel
            {
                Dock = DockStyle.Right,
                Width = 280,
                BackColor = Color.White,
                Padding = new Padding(20),
                Visible = false
            };

            var sep = new Panel { Dock = DockStyle.Left, Width = 1, BackColor = UiTheme.BorderSubtle };

            var lblBaslik = new Label
            {
                Text = "Vatandaş Bilgilerini Düzenle",
                Font = UiTheme.UiFontBold,
                ForeColor = UiTheme.Primary,
                Dock = DockStyle.Top,
                Height = 36,
                TextAlign = ContentAlignment.BottomLeft
            };
            var lblTCSub = new Label { Text = "TC Kimlik (değiştirilemez)", Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 22, TextAlign = ContentAlignment.BottomLeft };
            _lblSeciliTC = new Label { Text = "—", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 28 };
            var sepTC = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            var lblAdSub = new Label { Text = "Ad", Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 20, TextAlign = ContentAlignment.BottomLeft };
            _txtAd = new TextBox { Dock = DockStyle.Top, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle, Height = 28, MaxLength = 50 };

            var lblSoyadSub = new Label { Text = "Soyad", Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 24, TextAlign = ContentAlignment.BottomLeft };
            _txtSoyad = new TextBox { Dock = DockStyle.Top, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle, Height = 28, MaxLength = 50 };

            var lblEmailSub = new Label { Text = "E-Posta", Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 24, TextAlign = ContentAlignment.BottomLeft };
            _txtEmail = new TextBox { Dock = DockStyle.Top, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle, Height = 28, MaxLength = 100 };

            var btnKaydet = new Button { Text = "  💾  Değişiklikleri Kaydet", Dock = DockStyle.Bottom, Height = 42 };
            UiTheme.AnaEylemButonu(btnKaydet);
            btnKaydet.Click += BtnKaydet_Click;

            var btnIptal = new Button { Text = "İptal", Dock = DockStyle.Bottom, Height = 36 };
            UiTheme.IkincilButon(btnIptal);
            btnIptal.Click += (s, e) => { _pnlDuzenle.Visible = false; _seciliId = -1; };

            // Ters sırayla ekle (DockStyle.Top ile doğru sıra için)
            _pnlDuzenle.Controls.Add(btnKaydet);
            _pnlDuzenle.Controls.Add(btnIptal);
            _pnlDuzenle.Controls.Add(_txtEmail);
            _pnlDuzenle.Controls.Add(lblEmailSub);
            _pnlDuzenle.Controls.Add(_txtSoyad);
            _pnlDuzenle.Controls.Add(lblSoyadSub);
            _pnlDuzenle.Controls.Add(_txtAd);
            _pnlDuzenle.Controls.Add(lblAdSub);
            _pnlDuzenle.Controls.Add(sepTC);
            _pnlDuzenle.Controls.Add(_lblSeciliTC);
            _pnlDuzenle.Controls.Add(lblTCSub);
            _pnlDuzenle.Controls.Add(lblBaslik);
            _pnlDuzenle.Controls.Add(sep);

            this.Controls.Add(_pnlDuzenle);
        }

        private void VatandasYonetimForm_Load(object sender, EventArgs e)
        {
            dgvVatandas.BringToFront();
            UiTheme.DataGridStil(dgvVatandas);
            dgvVatandas.AutoGenerateColumns = false;
            dgvVatandas.Columns.Clear();
            dgvVatandas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 55 });
            dgvVatandas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KullaniciAdi", HeaderText = "Kullanıcı Adı", Width = 130 });
            dgvVatandas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ad", HeaderText = "Ad", Width = 110 });
            dgvVatandas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "soyad", HeaderText = "Soyad", Width = 110 });
            dgvVatandas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "tc", HeaderText = "TC Kimlik", Width = 110 });
            dgvVatandas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "e_Mail", HeaderText = "E-Posta", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            // Düzenleme butonu kolonu
            var colDuzenle = new DataGridViewButtonColumn
            {
                HeaderText = "İşlem",
                Text = "Düzenle",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };
            dgvVatandas.Columns.Add(colDuzenle);

            dgvVatandas.CellClick += DgvVatandas_CellClick;

            panelBottom.BackColor = Color.White;
            var sep = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            panelBottom.Controls.Add(sep);

            DoldurListe();
        }

        private void DgvVatandas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            // Düzenle butonu kolonu (son kolon)
            if (e.ColumnIndex == dgvVatandas.Columns.Count - 1)
            {
                var drv = dgvVatandas.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (drv == null) return;
                _seciliId = Convert.ToInt32(drv["Id"]);
                _lblSeciliTC.Text = drv["tc"]?.ToString() ?? "";
                _txtAd.Text = drv["ad"]?.ToString() ?? "";
                _txtSoyad.Text = drv["soyad"]?.ToString() ?? "";
                _txtEmail.Text = drv["e_Mail"]?.ToString() ?? "";
                _pnlDuzenle.Visible = true;
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (_seciliId < 0) return;

            if (string.IsNullOrWhiteSpace(_txtAd.Text) || string.IsNullOrWhiteSpace(_txtSoyad.Text))
            {
                MessageBox.Show("Ad ve Soyad boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hata = BelediyeDbServisi.PersonelVatandasBilgiGuncelle(_seciliId, _txtAd.Text, _txtSoyad.Text, _txtEmail.Text);
            if (hata != null)
            {
                MessageBox.Show("Güncelleme hatası: " + hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Vatandaş bilgileri güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _pnlDuzenle.Visible = false;
            _seciliId = -1;
            DoldurListe();
        }

        private void DoldurListe()
        {
            try { dgvVatandas.DataSource = BelediyeDbServisi.VatandasListesiGetir(); }
            catch (Exception ex) { MessageBox.Show("Liste yüklenemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvVatandas.CurrentRow == null)
            {
                MessageBox.Show("Lütfen silmek istediğiniz vatandaşı listeden seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var drv = dgvVatandas.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;

            int id = Convert.ToInt32(drv["Id"]);
            string adSoyad = drv["ad"] + " " + drv["soyad"];

            if (MessageBox.Show($"{adSoyad} isimli vatandaşı sistemden tamamen silmek istediğinize emin misiniz?\n\nBu işlem geri alınamaz.", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var hata = BelediyeDbServisi.VatandasSil(id);
                if (hata != null) MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else { _pnlDuzenle.Visible = false; DoldurListe(); }
            }
        }

        private void btnKapat_Click(object sender, EventArgs e) => this.Close();
    }
}
