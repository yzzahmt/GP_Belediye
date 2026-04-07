using System;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class VatandasHomeScreen : Form
    {
        private readonly string _tcKimlik;

        public VatandasHomeScreen(string tcKimlik)
        {
            _tcKimlik = tcKimlik ?? "";
            InitializeComponent();
        }

        private void VatandasHomeScreen_Load(object sender, EventArgs e)
        {
            panelHeader.BackColor = UiTheme.Primary;
            lblBaslik.ForeColor = UiTheme.TextOnPrimary;
            lblBaslik.Font = UiTheme.TitleFont;
            lblHosgeldin.ForeColor = Color.FromArgb(220, 235, 240);
            lblOzetBaslik.ForeColor = UiTheme.TextPrimary;
            lblOzetSatir1.ForeColor = UiTheme.TextMuted;
            lblOzetSatir2.ForeColor = UiTheme.TextMuted;
            lblAciklama.ForeColor = UiTheme.TextPrimary;
            panelOzet.BackColor = UiTheme.Surface;
            UiTheme.AnaEylemButonu(btnOzetYenile);
            UiTheme.IkincilButon(btnCikis);

            if (BelediyeDbServisi.TryGetKullaniciDisplayName(_tcKimlik, out var ad))
                lblHosgeldin.Text = "Hoş geldiniz, " + ad;
            else
                lblHosgeldin.Text = "Hoş geldiniz.";

            YukleOzetSatirlari();
        }

        private void btnOzetYenile_Click(object sender, EventArgs e)
        {
            YukleOzetSatirlari();
        }

        private void YukleOzetSatirlari()
        {
            BelediyeDbServisi.YukleGenelSayilar(out int basvuru, out int vatandas);
            lblOzetSatir1.Text = "Kayıtlı vatandaş: " + vatandas;
            lblOzetSatir2.Text = "Toplam başvuru: " + basvuru;
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            var g = new İlkGiris();
            g.Show();
            Close();
        }
    }
}
