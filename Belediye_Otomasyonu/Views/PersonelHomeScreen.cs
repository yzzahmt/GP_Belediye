using System;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class PersonelHomeScreen : Form
    {
        private readonly string _oturumKullaniciAdi;

        public PersonelHomeScreen() : this("")
        {
        }

        public PersonelHomeScreen(string oturumKullaniciAdi)
        {
            _oturumKullaniciAdi = oturumKullaniciAdi ?? "";
            InitializeComponent();
        }

        private void PersonelHomeScreen_Load(object sender, EventArgs e)
        {
            tableUstBar.BackColor = UiTheme.Primary;
            lblTitle.ForeColor = UiTheme.TextOnPrimary;
            lblTitle.Font = UiTheme.TitleFont;
            UiTheme.AnaEylemButonu(btnPersonelYonetim);
            UiTheme.AnaEylemButonu(btnYenile);
            UiTheme.IkincilButon(btnCikis);
            btnPersonelYonetim.Visible = BelediyeDbServisi.PersonelYoneticiMi(_oturumKullaniciAdi);
            lblChartTitle.ForeColor = UiTheme.TextPrimary;
            lblCapVatandas.ForeColor = UiTheme.TextMuted;
            lblCapPersonel.ForeColor = UiTheme.TextMuted;
            lblCapBasvuru.ForeColor = UiTheme.TextMuted;
            lblKayitliVatandas.ForeColor = UiTheme.TextPrimary;
            lblPersonelSayisi.ForeColor = UiTheme.TextPrimary;
            lblToplamBasvuru.ForeColor = UiTheme.TextPrimary;
            lblKayitliVatandas.Font = UiTheme.StatValueFont;
            lblPersonelSayisi.Font = UiTheme.StatValueFont;
            lblToplamBasvuru.Font = UiTheme.StatValueFont;
            cardVatandas.BackColor = UiTheme.CardBackground;
            cardPersonel.BackColor = UiTheme.CardBackground;
            cardBasvuru.BackColor = UiTheme.CardBackground;

            YukleDashboardVerileri();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            YukleDashboardVerileri();
        }

        private void YukleDashboardVerileri()
        {
            try
            {
                var oz = BelediyeDbServisi.YukleDashboardOzet();
                lblKayitliVatandas.Text = oz.KayitliVatandasSayisi.ToString();
                lblPersonelSayisi.Text = oz.PersonelSayisi.ToString();
                lblToplamBasvuru.Text = oz.ToplamBasvuru.ToString();
                GuncelleGrafik(oz);
                lblChartTitle.Text = "Başvuru durumları (özet)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Özet yüklenemedi. MySQL ve `belediye` veritabanı hazır mı?\n\n" + ex.Message,
                    "Veritabanı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void GuncelleGrafik(DashboardOzet oz)
        {
            try
            {
                cartesianChart1.Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Adet",
                        Values = new ChartValues<int> { oz.Beklemede, oz.Islemde, oz.Tamamlandi }
                    }
                };
                cartesianChart1.AxisX.Clear();
                cartesianChart1.AxisX.Add(new Axis
                {
                    Labels = new[] { "Beklemede", "İşlemde", "Tamamlandı" }
                });
                cartesianChart1.AxisY.Clear();
                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Adet",
                    MinValue = 0
                });
                cartesianChart1.LegendLocation = LegendLocation.Top;
            }
            catch (Exception ex)
            {
                lblChartTitle.Text = "Grafik gösterilemedi: " + ex.Message;
            }
        }

        private void btnPersonelYonetim_Click(object sender, EventArgs e)
        {
            using (var f = new PersonelYonetimForm(_oturumKullaniciAdi))
            {
                f.ShowDialog(this);
            }
            YukleDashboardVerileri();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            var g = new İlkGiris();
            g.Show();
            Close();
        }
    }
}
