using System;
using System.Windows.Forms;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class BasvuruYonetimForm : Form
    {
        private readonly string _oturumKullaniciAdi;

        public BasvuruYonetimForm(string oturumKullaniciAdi)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            InitializeComponent();
            UiTheme.FormDizayn(this);
            UiTheme.AnaEylemButonu(btnDurumDegistir);
            UiTheme.IkincilButon(btnKapat);
        }

        private void BasvuruYonetimForm_Load(object sender, EventArgs e)
        {
            dgvBasvurular.AutoGenerateColumns = false;
            dgvBasvurular.Columns.Clear();
            dgvBasvurular.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 40 });
            dgvBasvurular.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Konu", HeaderText = "Başvuru Konusu", Width = 250 });
            dgvBasvurular.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum", Width = 100 });
            dgvBasvurular.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KayitTarihi", HeaderText = "Tarih", Width = 120 });
            dgvBasvurular.ReadOnly = true;
            dgvBasvurular.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBasvurular.MultiSelect = false;
            
            cmbDurum.Items.Add("Beklemede");
            cmbDurum.Items.Add("İşlemde");
            cmbDurum.Items.Add("Tamamlandı");
            cmbDurum.SelectedIndex = 0;

            DoldurListe();
        }

        private void DoldurListe()
        {
            try
            {
                dgvBasvurular.DataSource = BelediyeDbServisi.BasvuruListesiGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Liste yüklenemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDurumDegistir_Click(object sender, EventArgs e)
        {
            if (dgvBasvurular.CurrentRow == null)
            {
                MessageBox.Show("Lütfen durumunu değiştirmek istediğiniz başvuruyu listeden seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var drv = dgvBasvurular.CurrentRow.DataBoundItem as System.Data.DataRowView;
            if (drv == null) return;

            int id = Convert.ToInt32(drv["Id"]);
            string yeniDurum = cmbDurum.SelectedItem.ToString();
            
            // Eğer "İşlemde" seçildiyse veritabanındaki isimlendirme olan "Islemde", "Tamamlandı" ise "Tamamlandi" olarak gönderilmeli
            if (yeniDurum == "İşlemde") yeniDurum = "Islemde";
            if (yeniDurum == "Tamamlandı") yeniDurum = "Tamamlandi";

            var hata = BelediyeDbServisi.BasvuruDurumGuncelle(id, yeniDurum);
            if (hata != null)
                MessageBox.Show(hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                MessageBox.Show("Başvuru durumu başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DoldurListe();
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
