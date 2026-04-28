using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class SifremiUnuttumForm : Form
    {
        string uretilenKod = "";

        public SifremiUnuttumForm()
        {
            InitializeComponent();
        }

        private void SifremiUnuttumForm_Load(object sender, EventArgs e)
        {
            BackColor = UiTheme.Surface;
            UiTheme.IkincilButon(btnIptal);
        }

        private void button1_Click(object sender, EventArgs e)
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

            // 1. Rastgele 6 haneli kod üretiliyor
            Random rastgele = new Random();
            uretilenKod = rastgele.Next(100000, 999999).ToString();

            // 2. --- MAİL GÖNDERME İŞLEMİ BAŞLIYOR ---
            try
            {
                SmtpClient istemci = new SmtpClient("smtp.gmail.com", 587);
                istemci.EnableSsl = true;

                // KANZİ BURAYA DİKKAT: O 16 haneli şifreni aşağıdaki tırnak içine boşluksuz yapıştır!
                istemci.Credentials = new NetworkCredential("uskudarbelediyesi51@gmail.com", "vfngrlfhzdfvxvbh");

                MailMessage mesaj = new MailMessage();

                mesaj.From = new MailAddress("uskudarbelediyesi51@gmail.com", "Üsküdar Belediyesi Otomasyon");

                mesaj.To.Add(txtEmail.Text.Trim()); // Bu kısma dokunma, kullanıcının ekrana yazdığı adrese gönderecek
                mesaj.Subject = "Güvenlik Kodu - Şifre Sıfırlama İşlemi";
                mesaj.Body = $"Sayın kullanıcımız,\n\nŞifre sıfırlama işleminiz için tek kullanımlık güvenlik kodunuz: {uretilenKod}\n\nLütfen bu kodu kimseyle paylaşmayınız.";

                // Kargo yola çıkıyor!
                istemci.Send(mesaj);

                MessageBox.Show("Güvenlik kodu e-posta adresinize başarıyla gönderildi! Lütfen gelen kutunuzu (veya Spam klasörünü) kontrol edin.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. Mail gittiyse 2. sayfayı (Doğrulama ekranını) açıyoruz
                YeniSifreForm yeniSayfa = new YeniSifreForm(uretilenKod, txtTc.Text.Trim(), txtEmail.Text.Trim());
                this.Hide();
                yeniSayfa.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mail gönderilirken bir hata oluştu. İnternet bağlantınızı kontrol edin veya mail/şifrenin doğruluğundan emin olun.\nHata Detayı: " + ex.Message, "Mail Gönderilemedi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}