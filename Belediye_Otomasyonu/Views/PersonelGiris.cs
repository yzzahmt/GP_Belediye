using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient; 

namespace Belediye_Otomasyonu.Views
{
    public partial class PersonelGiris : Form
    {
       
        string connectionString = "Server=.; Database=BelediyeDB; Integrated Security=True;";

        public PersonelGiris()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            İlkGiris i2 = new İlkGiris();
            i2.Show();
            this.Hide();
        }

        private void btnGirisPrsn_Click(object sender, EventArgs e)
        {
            
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    
                    string sorgu = "SELECT * FROM Personeller WHERE KullaniciAdi=@p1 AND Sifre=@p2";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    
                  
                    komut.Parameters.AddWithValue("@p1", textBox1.Text); 
                    komut.Parameters.AddWithValue("@p2", txtSifre.Text);

                    SqlDataReader oku = komut.ExecuteReader();

                    if (oku.Read()) 
                    {
                        MessageBox.Show("Personel Girişi Başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PersonelHomeScreen P1 = new PersonelHomeScreen();
                        P1.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bağlantı hatası: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}