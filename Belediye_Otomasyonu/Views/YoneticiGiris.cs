using System;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class YoneticiGiris : Form
    {
        public YoneticiGiris()
        {
            InitializeComponent();
            UiTheme.FormDizayn(this);
            this.Resize += YoneticiGiris_Resize;
            OrtalaYerlesim();
            UiTheme.AnaEylemButonu(btnGirisYonetici);
            UiTheme.IkincilButon(button1);
        }

        private void YoneticiGiris_Resize(object sender, EventArgs e)
        {
            OrtalaYerlesim();
        }

        private void OrtalaYerlesim()
        {
            pictureBox1.Left = System.Math.Max(0, (ClientSize.Width - pictureBox1.Width) / 2);
            tabloAlan.Left = System.Math.Max(16, (ClientSize.Width - tabloAlan.Width) / 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PersonelGiris p = new PersonelGiris();
            this.Hide();
            p.ShowDialog();
            this.Close();
        }

        private void btnGirisYonetici_Click(object sender, EventArgs e)
        {
            try
            {
                var ka = textBox1.Text.Trim();

                if (BelediyeDbServisi.YoneticiGirisDogrula(ka, txtSifre.Text))
                {
                    MessageBox.Show("Yönetici girişi başarılı. Sisteme yönlendiriliyorsunuz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var p = new PersonelHomeScreen(ka, isYonetici: true);
                    this.Hide();
                    p.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı ya da yönetici yetkisi bulunmuyor.", "Erişim Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
