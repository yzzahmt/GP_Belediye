using System;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class PersonelGiris : Form
    {
        public PersonelGiris()
        {
            InitializeComponent();
            UiTheme.FormDizayn(this);
            this.Resize += PersonelGiris_Resize;
            OrtalaYerlesim();
            UiTheme.AnaEylemButonu(btnGirisPrsn);
            UiTheme.IkincilButon(button1);
            UiTheme.IkincilButon(btnSifremiUnuttum);
            UiTheme.IkincilButon(btnYoneticiGiris);
        }

        private void btnSifremiUnuttum_Click(object sender, EventArgs e)
        {
            using (var f = new SifremiUnuttumForm())
                f.ShowDialog(this);
        }

        private void PersonelGiris_Resize(object sender, EventArgs e)
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
            İlkGiris i2 = new İlkGiris();
            this.Hide();
            i2.ShowDialog();
            this.Close();
        }

        private void btnGirisPrsn_Click(object sender, EventArgs e)
        {
            try
            {
                var ka = textBox1.Text.Trim();

                if (BelediyeDbServisi.PersonelGirisDogrula(ka, txtSifre.Text))
                {
                    MessageBox.Show("Personel girişi başarılı. Sisteme yönlendiriliyorsunuz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var p = new PersonelHomeScreen(ka, isYonetici: false);
                    this.Hide();
                    p.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı. Lütfen tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnYoneticiGiris_Click(object sender, EventArgs e)
        {
            var yg = new YoneticiGiris();
            this.Hide();
            yg.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
    }
}