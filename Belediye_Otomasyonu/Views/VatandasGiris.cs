using System;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class VatandasGiris : Form
    {
        public VatandasGiris()
        {
            InitializeComponent();
            this.Resize += VatandasGiris_Resize;
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void Geri_btn_Click(object sender, EventArgs e)
        {
            İlkGiris i2 = new İlkGiris();
            i2.Show();
            this.Hide();
        }

        private void VatandasGiris_Load(object sender, EventArgs e)
        {
            OrtalaYerlesim();
            UiTheme.AnaEylemButonu(btnGirisYap);
            UiTheme.IkincilButon(Geri_btn);
            UiTheme.IkincilButon(btnSifremiUnuttum);
        }

        private void btnSifremiUnuttum_Click(object sender, EventArgs e)
        {
            using (var f = new SifremiUnuttumForm())
                f.ShowDialog(this);
        }

        private void VatandasGiris_Resize(object sender, EventArgs e)
        {
            OrtalaYerlesim();
        }

        private void OrtalaYerlesim()
        {
            pictureBox1.Left = System.Math.Max(0, (ClientSize.Width - pictureBox1.Width) / 2);
            tabloAlan.Left = System.Math.Max(16, (ClientSize.Width - tabloAlan.Width) / 2);
        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            try
            {
                if (BelediyeDbServisi.VatandasGirisDogrula(txtTC.Text, textSifre.Text))
                {
                    var vh = new VatandasHomeScreen(txtTC.Text.Trim());
                    vh.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("TC kimlik veya şifre hatalı. Kayıt olduysanız kayıtta kullandığınız bilgilerle girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı hatası: " + ex.Message, "Veritabanı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}
