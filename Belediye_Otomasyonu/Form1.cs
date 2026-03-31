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

namespace Belediye_Otomasyonu
{
    public partial class Form1 : Form
    {
        String connection = "Server=localhost;Database=belediye;Uid=root;Pwd=;";
        MySqlConnection con;

        public Form1()
        {
            InitializeComponent();
            con = new MySqlConnection(connection);
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
            con.Open();
            string sorgu = "SELECT * FROM kullanicilar WHERE Kullaniciadi='" + KullaniciAdi_txt.Text + "' AND sifre='" + Sifre_txt.Text + "'";
            MySqlCommand cmd = new MySqlCommand(sorgu, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Home_Screen HomeScreen = new Home_Screen();
                HomeScreen.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı veya şifre hatalı");

            }
            con.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
 }

