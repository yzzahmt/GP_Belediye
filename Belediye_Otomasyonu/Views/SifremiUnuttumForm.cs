using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class SifremiUnuttumForm : Form
    {
        public SifremiUnuttumForm()
        {
            InitializeComponent();
        }

        private void SifremiUnuttumForm_Load(object sender, EventArgs e)
        {
            BackColor = UiTheme.Surface;
            UiTheme.IkincilButon(btnIptal);
            UiTheme.AnaEylemButonu(btnKaydet);
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (txtTc.Text.Trim().Length != 11)
            {
                MessageBox.Show("TC kimlik 11 haneli olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Geçerli bir e-posta girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtYeni.Text.Length < 3)
            {
                MessageBox.Show("Yeni şifre en az 3 karakter olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtYeni.Text != txtYeniTekrar.Text)
            {
                MessageBox.Show("Şifre tekrarı eşleşmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (BelediyeDbServisi.SifremiUnuttumGuncelle(txtTc.Text.Trim(), txtEmail.Text.Trim(), txtYeni.Text))
                {
                    MessageBox.Show("Şifreniz güncellendi. Giriş ekranından yeni şifrenizle giriş yapabilirsiniz.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("TC ve e-posta eşleşmedi veya kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem başarısız: " + ex.Message, "Veritabanı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
