using System;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class SifreGuncelleForm : Form
    {
        string tc = "";
        string email = "";

        public SifreGuncelleForm(string gelenTc, string gelenEmail)
        {
            InitializeComponent();
            tc = gelenTc;
            email = gelenEmail;
        }

        private void SifreGuncelleForm_Load(object sender, EventArgs e)
        {
            this.BackColor = UiTheme.Surface;
            UiTheme.AnaEylemButonu(btnKaydet); // Buton tasarımını uyguluyoruz
        }

        // İŞTE KABLO BURAYA BAĞLANDI! Bütün işi bu kısım yapacak.
        private void btnKaydet_Click_1(object sender, EventArgs e)
        {
            string yeniSifre = txtYeni.Text.Trim();
            string yeniSifreTekrar = txtYeniTekrar.Text.Trim();

            if (yeniSifre == yeniSifreTekrar && yeniSifre.Length >= 3)
            {
                try
                {
                    if (BelediyeDbServisi.SifremiUnuttumGuncelle(tc, email, yeniSifre))
                    {
                        MessageBox.Show("Şifreniz başarıyla güncellendi! Giriş ekranına yönlendiriliyorsunuz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // --- ÜST KATMANA (ASIL GİRİŞ EKRANINA) GEÇİŞ ---
                        PersonelGiris girisEkrani = new PersonelGiris();
                        this.Hide();        // Bu şifre sayfasını gizle
                        girisEkrani.ShowDialog(); // Ana giriş sayfasını aç
                        this.Close();       // İşlem bitince arka planda şifre sayfasını tamamen kapat
                    }
                    else
                    {
                        MessageBox.Show("Bilgiler doğrulanamadı, şifre güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Hata");
                }
            }
            else
            {
                MessageBox.Show("Şifreler uyuşmuyor veya çok kısa (en az 3 karakter)!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}