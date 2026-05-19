using System;
using System.Windows.Forms;
using Belediye_Otomasyonu;

namespace Belediye_Otomasyonu.Views
{
    public partial class YeniSifreForm : Form
    {
        string dogruKod = "";
        string gelenTc = "";
        string gelenEmail = "";

        public YeniSifreForm(string kod, string tc, string email)
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            dogruKod = kod;
            gelenTc = tc;
            gelenEmail = email;
        }

        private void YeniSifreForm_Load(object sender, EventArgs e)
        {
            this.BackColor = UiTheme.Surface;
            UiTheme.AnaEylemButonu(btnDogrula);
        }

        // İşte butonun gerçekte bağlı olduğu yer burası! Kodları buraya taşıdım.
        private void btnDogrula_Click_1(object sender, EventArgs e)
        {
            // Trim() kullanarak sağdaki soldaki gizli boşlukları siliyoruz
            if (txtGelenKod.Text.Trim() == dogruKod)
            {
                MessageBox.Show("Doğrulama başarılı! Şifre güncelleme ekranına geçiliyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. sayfayı (Şifre Güncelleme) açıyoruz
                SifreGuncelleForm guncellemeSayfasi = new SifreGuncelleForm(gelenTc, gelenEmail);
                this.Hide();
                guncellemeSayfasi.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Girdiğiniz kod hatalı. Lütfen tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
