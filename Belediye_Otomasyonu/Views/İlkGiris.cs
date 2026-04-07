using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Belediye_Otomasyonu.Views
{
    public partial class İlkGiris : Form
    {
        public İlkGiris()
        {
            InitializeComponent();
        }

        private void ilkVtnds_btn_Click(object sender, EventArgs e)
        {
            VatandasGiris vatandasGiris = new VatandasGiris();
            vatandasGiris.Show();
            this.Hide();

        }

        private void ilkKyt_btn_Click(object sender, EventArgs e)
        {
            Kayit_Screen kayitEkran = new Kayit_Screen();
            kayitEkran.Show();
            this.Hide();
        }

        private void ilkPrsnl_btn_Click(object sender, EventArgs e)
        {
            PersonelGiris personelGiris = new PersonelGiris();
            personelGiris.Show();
            this.Hide();
        }
    }
}
