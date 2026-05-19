using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Belediye_Otomasyonu.Views;

namespace Belediye_Otomasyonu
{
    public partial class Form1 : Form
    {
        MySqlConnection con;

        public Form1()
        {
            InitializeComponent();
            con = new MySqlConnection(DatabaseConfig.MySqlBelediye);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Belediye_lbl_Click(object sender, EventArgs e)
        {
            Belediye_lbl.Font = new Font("Arial", 48, FontStyle.Bold);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Personel_btn_Click(object sender, EventArgs e)
        {

        }

        private void Kayit_btn_Click(object sender, EventArgs e)
        {
            Kayit_Screen newForm = new Kayit_Screen();
            newForm.Show();


        }

        private void Vatandas_btn_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                using (var cmd = new MySqlCommand(
                    "SELECT tc FROM kullanicilar WHERE KullaniciAdi=@kadi AND sifre=@sifre LIMIT 1", con))
                {
                    cmd.Parameters.AddWithValue("@kadi", KullaniciAdi_txt.Text);
                    cmd.Parameters.AddWithValue("@sifre", Sifre_txt.Text);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string tc = dr.GetString(0);
                            var homeScreen = new VatandasHomeScreen(tc);
                            homeScreen.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                        }
                    }
                }
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
 }

