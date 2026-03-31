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
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace Belediye_Otomasyonu
{
    public partial class Kayit_Screen : Form
    {
        string connection = "Server=localhost;Database=belediye;Uid=root;Pwd=;";
        MySqlConnection con;
        public Kayit_Screen()
        {
            InitializeComponent();
            con = new MySqlConnection(connection);
        }
        private void Kayit_Screen_Load(object sender, EventArgs e)
        {
        }
        private void kayitOl_btn_Click(object sender, EventArgs e)
        { //Veritabanı:

            if(Tc_txt.Text.Length != 11) // TC kimlik Numarası Kontrolü
            {

                MessageBox.Show("TC Kimlik numaranız 11 haneli olmalıdır!!!!!");
                return;
            }
            string emailKontrol = @"^[ @""^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email_txt.Text, emailKontrol)) //E mail doğrulaması
            {
                MessageBox.Show("Geçerli Bir E-Mail Adresi Giriniz!!!!!");
            }
            try
            {
                con.Open();
                string sorgu = "INSERT INTO kullanicilar (ad, e_Mail, KullaniciAdi, sifre, soyad, tc) VALUES ('"
                    + Ad_txt.Text + "','"
                    + Soyad_txt.Text + "','"
                    + Tc_txt.Text + "','"
                    + Email_txt.Text + "','"
                    + KullaniciAdi_txt.Text + "','"
                    + Sifre_txt.Text + "')";
                MySqlCommand cmd = new MySqlCommand(sorgu, con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Kayıt Başarılı!!!");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
      


        private void Ad_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void Soyad_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void Tc_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void Email_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void KullaniciAdi_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void Sifre_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}