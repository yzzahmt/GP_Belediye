using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class PersonelYonetimForm : Form
    {
        private readonly string _oturumKullaniciAdi;

        public PersonelYonetimForm() : this("")
        {
        }

        public PersonelYonetimForm(string oturumKullaniciAdi)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            InitializeComponent();
        }

        private void PersonelYonetimForm_Load(object sender, EventArgs e)
        {
            BackColor = UiTheme.Surface;
            UiTheme.AnaEylemButonu(btnListeYenile);
            UiTheme.AnaEylemButonu(btnVatandasYetki);
            UiTheme.AnaEylemButonu(btnYeniKaydet);
            UiTheme.IkincilButon(btnDurumDegistir);
            UiTheme.IkincilButon(btnKapat);
            dgvPersonel.AutoGenerateColumns = false;
            dgvPersonel.Columns.Clear();
            dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Id", Width = 40 });
            dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KullaniciAdi", HeaderText = "Kullanıcı adı", Width = 140 });
            dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Ad", HeaderText = "Ad", Width = 120 });
            dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Soyad", HeaderText = "Soyad", Width = 120 });
            dgvPersonel.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "Aktif", HeaderText = "Aktif", Width = 50 });
            dgvPersonel.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "Yonetici", HeaderText = "Yönetici", Width = 60 });
            dgvPersonel.ReadOnly = true;
            dgvPersonel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersonel.MultiSelect = false;
            DoldurListe();
        }

        private void DoldurListe()
        {
            try
            {
                dgvPersonel.DataSource = BelediyeDbServisi.PersonelListesiGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Liste yüklenemedi: " + ex.Message, "Veritabanı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnListeYenile_Click(object sender, EventArgs e)
        {
            DoldurListe();
        }

        private void btnVatandasYetki_Click(object sender, EventArgs e)
        {
            DataTable dt;
            try
            {
                dt = BelediyeDbServisi.KullanicilarPersoneleUygunGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Kayıtlı vatandaşlar arasında henüz personele eklenmemiş uygun kullanıcı yok (hepsi zaten personel olabilir).", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var f = new Form())
            {
                f.Text = "Vatandaşa personel yetkisi ver";
                f.Size = new Size(420, 140);
                f.StartPosition = FormStartPosition.CenterParent;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                var cb = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Dock = DockStyle.Top,
                    Height = 28,
                    DataSource = dt,
                    DisplayMember = "KullaniciAdi",
                    ValueMember = "KullaniciAdi"
                };
                var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Height = 40 };
                var iptal = new Button { Text = "İptal", DialogResult = DialogResult.Cancel, AutoSize = true };
                var tamam = new Button { Text = "Personele ekle", DialogResult = DialogResult.OK, AutoSize = true };
                panel.Controls.Add(iptal);
                panel.Controls.Add(tamam);
                f.Controls.Add(panel);
                f.Controls.Add(cb);
                f.CancelButton = iptal;
                f.AcceptButton = tamam;

                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var drv = cb.SelectedItem as DataRowView;
                if (drv == null)
                    return;
                var kadi = drv["KullaniciAdi"].ToString();
                var hata = BelediyeDbServisi.KullaniciyiPersoneleAktar(kadi);
                if (hata != null)
                    MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    MessageBox.Show("Kullanıcı personele eklendi. Artık personel girişi ile girebilir.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DoldurListe();
                }
            }
        }

        private void btnYeniKaydet_Click(object sender, EventArgs e)
        {
            var hata = BelediyeDbServisi.PersonelEkle(
                txtYeniKadi.Text,
                txtYeniSifre.Text,
                txtYeniAd.Text,
                txtYeniSoyad.Text,
                chkYeniYonetici.Checked);
            if (hata != null)
                MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                MessageBox.Show("Yeni personel kaydı oluşturuldu.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtYeniKadi.Clear();
                txtYeniSifre.Clear();
                txtYeniAd.Clear();
                txtYeniSoyad.Clear();
                chkYeniYonetici.Checked = false;
                DoldurListe();
            }
        }

        private void btnDurumDegistir_Click(object sender, EventArgs e)
        {
            if (dgvPersonel.CurrentRow == null)
            {
                MessageBox.Show("Önce listeden bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var drv = dgvPersonel.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null)
                return;

            int id = Convert.ToInt32(drv["Id"]);
            bool simdiAktif = Convert.ToInt32(drv["Aktif"]) != 0;
            var yeni = !simdiAktif;

            var msg = yeni
                ? "Seçili personeli aktifleştirmek istiyor musunuz?"
                : "Seçili personeli pasifleştirmek istiyor musunuz? Giriş yapamaz.";

            if (MessageBox.Show(msg, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var hata = BelediyeDbServisi.PersonelAktifDegistir(id, yeni, _oturumKullaniciAdi);
            if (hata != null)
                MessageBox.Show(hata, "İşlem yapılamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                DoldurListe();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
