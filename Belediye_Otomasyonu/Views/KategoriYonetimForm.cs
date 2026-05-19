using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class KategoriYonetimForm : Form
    {
        private readonly string _oturumKullaniciAdi;
        private Panel _pnlEkle;
        private TextBox _txtKategoriAdi;

        public KategoriYonetimForm(string oturumKullaniciAdi)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            InitializeComponent();
            UiTheme.FormDizayn(this);
            OlusturEklePanel();
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

            lblTitle.Text = "🏷️  Kategori Yönetimi";
            lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            var lblAlt = new Label
            {
                Text = "Sistemdeki başvuru kategorilerini listeleyin, yeni kategori ekleyin veya silin.",
                Font = UiTheme.SmallFont,
                ForeColor = Color.FromArgb(180, 200, 230),
                AutoSize = true,
                Location = new Point(17, 46)
            };
            panelTop.Controls.Add(lblAlt);
        }

        private void OlusturEklePanel()
        {
            _pnlEkle = new Panel
            {
                Dock = DockStyle.Right,
                Width = 280,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var sep = new Panel { Dock = DockStyle.Left, Width = 1, BackColor = UiTheme.BorderSubtle };

            var lblBaslik = new Label
            {
                Text = "Yeni Kategori Ekle",
                Font = UiTheme.UiFontBold,
                ForeColor = UiTheme.Primary,
                Dock = DockStyle.Top,
                Height = 36,
                TextAlign = ContentAlignment.BottomLeft
            };

            var lblKatSub = new Label { Text = "Kategori Adı", Font = UiTheme.SmallFont, ForeColor = UiTheme.TextMuted, Dock = DockStyle.Top, Height = 28, TextAlign = ContentAlignment.BottomLeft };
            _txtKategoriAdi = new TextBox { Dock = DockStyle.Top, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle, Height = 28, MaxLength = 100 };

            var btnEkle = new Button { Text = "  ➕  Kategori Ekle", Dock = DockStyle.Bottom, Height = 42 };
            UiTheme.AnaEylemButonu(btnEkle);
            btnEkle.Click += BtnEkle_Click;

            // Ters sırayla ekle (DockStyle.Top ile doğru sıra için)
            _pnlEkle.Controls.Add(btnEkle);
            _pnlEkle.Controls.Add(_txtKategoriAdi);
            _pnlEkle.Controls.Add(lblKatSub);
            _pnlEkle.Controls.Add(lblBaslik);
            _pnlEkle.Controls.Add(sep);

            this.Controls.Add(_pnlEkle);
        }

        private void KategoriYonetimForm_Load(object sender, EventArgs e)
        {
            dgvKategori.BringToFront();
            UiTheme.DataGridStil(dgvKategori);
            dgvKategori.AutoGenerateColumns = false;
            dgvKategori.Columns.Clear();
            dgvKategori.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 70 });
            dgvKategori.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KategoriAdi", HeaderText = "Kategori Adı", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            panelBottom.BackColor = Color.White;
            var sep = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            panelBottom.Controls.Add(sep);

            DoldurListe();
        }

        private void DoldurListe()
        {
            try
            {
                dgvKategori.DataSource = BelediyeDbServisi.KategorileriGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori listesi yüklenemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtKategoriAdi.Text))
            {
                MessageBox.Show("Kategori adı boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hata = BelediyeDbServisi.KategoriEkle(_txtKategoriAdi.Text);
            if (hata != null)
            {
                MessageBox.Show("Ekleme hatası: " + hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Yeni kategori başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _txtKategoriAdi.Clear();
            DoldurListe();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvKategori.CurrentRow == null)
            {
                MessageBox.Show("Lütfen silmek istediğiniz kategoriyi listeden seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var drv = dgvKategori.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;

            int id = Convert.ToInt32(drv["Id"]);
            string kategoriAdi = drv["KategoriAdi"]?.ToString() ?? "";

            if (MessageBox.Show($"'{kategoriAdi}' kategorisini silmek istediğinize emin misiniz?\n\nBu işlem mevcut başvurulardaki kategori isimlerini etkilemez ancak vatandaşlar artık bu kategoriyi seçemez.", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var hata = BelediyeDbServisi.KategoriSil(id);
                if (hata != null) MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else { DoldurListe(); }
            }
        }

        private void btnKapat_Click(object sender, EventArgs e) => this.Close();
    }
}
