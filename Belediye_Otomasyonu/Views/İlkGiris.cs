using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Belediye_Otomasyonu;

namespace Belediye_Otomasyonu.Views
{
    public partial class İlkGiris : Form
    {
        public İlkGiris()
        {
            InitializeComponent();
            this.Load += İlkGiris_Load;
            this.Resize += İlkGiris_Resize;
            OrtalaLogo();
        }

        private void İlkGiris_Load(object sender, EventArgs e)
        {
            UiTheme.AnaEylemButonu(ilkVtnds_btn);
            UiTheme.AnaEylemButonu(ilkPrsnl_btn);
            UiTheme.AnaEylemButonu(ilkKyt_btn);
        }

        private void İlkGiris_Resize(object sender, EventArgs e)
        {
            OrtalaLogo();
        }

        private void OrtalaLogo()
        {
            pictureBox1.Left = Math.Max(0, (ClientSize.Width - pictureBox1.Width) / 2);
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
