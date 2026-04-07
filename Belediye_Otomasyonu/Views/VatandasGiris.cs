using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Belediye_Otomasyonu.Views
{
    public partial class VatandasGiris : Form
    {
        public VatandasGiris()
        {
            
            string conString = "Server=.; Database=Belediye; Integrated Security=True;";
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Geri_btn_Click(object sender, EventArgs e)
        {
            İlkGiris i2 = new İlkGiris();
            i2.Show();
            this.Hide();
        }

        private void VatandasGiris_Load(object sender, EventArgs e)
        {

        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection(conString))
            {
                try
                {
                    baglanti.Open();
                    
                    string sorgu = "SELECT * FROM Vatandaslar WHERE TCKimlik=@p1 AND Sifre=@p2";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);

                    komut.Parameters.AddWithValue("@p1", txtTC.Text);
                    komut.Parameters.AddWithValue("@p2", txtSifre.Text); 

                    SqlDataReader oku = komut.ExecuteReader();

                    if (oku.Read()) 
                    {
                        MessageBox.Show("Giriş Başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Diğer forma geçiş
                        VatandasHomeScreen vh = new VatandasHomeScreen();
                        vh.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("TC Kimlik No veya Şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bağlantı hatası: " + ex.Message);
                }
            }
        }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
