using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using Belediye_Otomasyonu.Models;
using Belediye_Otomasyonu.Controllers;

namespace Belediye_Otomasyonu
{
    public partial class Kayit_Screen : Form
    {
        public Kayit_Screen()
        {
            InitializeComponent();
        }

        private void Kayit_Screen_Load(object sender, EventArgs e)
        {
        }

        private void kayitOl_btn_Click(object sender, EventArgs e)
        {
            
            if (Tc_txt.Text.Length != 11)
            {
                MessageBox.Show("TC Kimlik numaranız 11 haneli olmalıdır!!!!!");
                return; 
            }

            string emailKontrol = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; 
            if (!Regex.IsMatch(Email_txt.Text, emailKontrol))
            {
                MessageBox.Show("Geçerli Bir E-Mail Adresi Giriniz!!!!!");
                return; 
            }

            
            Kullanici yeniKullanici = new Kullanici();
            yeniKullanici.Ad = Ad_txt.Text;
            yeniKullanici.Soyad = Soyad_txt.Text;
            yeniKullanici.Tc = Tc_txt.Text;
            yeniKullanici.Email = Email_txt.Text;
            yeniKullanici.KullaniciAdi = KullaniciAdi_txt.Text;
            yeniKullanici.Sifre = Sifre_txt.Text;

           
            KullaniciController kontrolcu = new KullaniciController();
            bool basariliMi = kontrolcu.KullaniciEkle(yeniKullanici);

            
            if (basariliMi)
            {
                MessageBox.Show("Kayıt Başarılı!!!");
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Kayıt sırasında veritabanı kaynaklı bir hata oluştu!");
            }
        }

        
        private void Ad_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Soyad_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Tc_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Email_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void KullaniciAdi_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Sifre_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
    }
}