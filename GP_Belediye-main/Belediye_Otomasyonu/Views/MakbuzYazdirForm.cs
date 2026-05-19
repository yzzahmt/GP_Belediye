using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public class MakbuzYazdirForm : Form
    {
        private readonly int _borcId;
        private Panel _pnlA4;
        private Label _lblTarih;
        private Label _lblNo;
        private Label _lblBaslik;
        private Label _lblVatandas;
        private Label _lblTC;
        private Label _lblAciklama;
        private Label _lblMiktar;
        private Label _lblMiktarYazi;
        private Label _lblBarkod;
        private Label _lblVezne;
        private Button _btnPrint;
        private Button _btnClose;
        private PrintDocument _printDoc;

        public MakbuzYazdirForm(int borcId)
        {
            _borcId = borcId;
            InitializeComponent();
            YukleMakbuzDetay();
        }

        private void InitializeComponent()
        {
            this.Text = "Belediye Vezne Tahsilat Makbuzu Önizleme";
            this.Size = new Size(720, 850);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Print Document
            _printDoc = new PrintDocument();
            _printDoc.PrintPage += PrintDoc_PrintPage;

            // Top control bar
            var pnlControls = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White };
            pnlControls.Paint += (s, e) => {
                using (var p = new Pen(Color.FromArgb(220, 220, 220)))
                    e.Graphics.DrawLine(p, 0, 59, pnlControls.Width, 59);
            };

            _btnPrint = new Button { Text = "🖨️ Resmi Makbuzu Yazdır", Width = 180, Height = 38, Location = new Point(15, 11) };
            UiTheme.BasariButon(_btnPrint);
            _btnPrint.Click += BtnPrint_Click;

            _btnClose = new Button { Text = "Kapat", Width = 100, Height = 38, Location = new Point(205, 11) };
            UiTheme.IkincilButon(_btnClose);
            _btnClose.Click += (s, e) => this.Close();

            pnlControls.Controls.AddRange(new Control[] { _btnPrint, _btnClose });
            this.Controls.Add(pnlControls);

            // A4 Paper panel emulation
            _pnlA4 = new Panel
            {
                Size = new Size(595, 700), // A4 ratio approximation
                Location = new Point(55, 80),
                BackColor = Color.White,
                Padding = new Padding(45)
            };
            _pnlA4.Paint += (s, e) => {
                using (var p = new Pen(Color.FromArgb(180, 180, 180), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, _pnlA4.Width - 1, _pnlA4.Height - 1);
            };
            this.Controls.Add(_pnlA4);

            // A4 Contents
            var lblHeader = new Label
            {
                Text = "T.C.\nBELEDİYE BAŞKANLIĞI\nMali Hizmetler Dairesi Başkanlığı (Gelirler Şefliği)",
                Font = new Font("Times New Roman", 12f, FontStyle.Bold),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Top,
                Height = 60,
                ForeColor = Color.Black
            };
            _pnlA4.Controls.Add(lblHeader);

            var pnlMeta = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.Transparent };
            _lblNo = new Label { Text = "Makbuz No: MAK-", Font = new Font("Times New Roman", 10f), Location = new Point(5, 10), AutoSize = true, ForeColor = Color.Black };
            _lblTarih = new Label { Text = "Tahsilat Tarihi: ", Font = new Font("Times New Roman", 10f), Location = new Point(320, 10), AutoSize = true, ForeColor = Color.Black };
            pnlMeta.Controls.AddRange(new Control[] { _lblNo, _lblTarih });
            _pnlA4.Controls.Add(pnlMeta);

            var pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 10 };
            _pnlA4.Controls.Add(pnlSpacer);

            _lblBaslik = new Label
            {
                Text = "VEZNE TAHSİLAT MAKBUZU",
                Font = new Font("Times New Roman", 11f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30,
                ForeColor = Color.Black
            };
            _pnlA4.Controls.Add(_lblBaslik);

            // Grid layout for info
            var pnlInfo = new Panel { Dock = DockStyle.Top, Height = 180, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.Transparent };
            _pnlA4.Controls.Add(pnlInfo);

            int infoY = 15;
            _lblVatandas = new Label { Text = "Mükellef Ad Soyad: ", Font = new Font("Times New Roman", 10f, FontStyle.Bold), Location = new Point(15, infoY), Width = 450, ForeColor = Color.Black };
            pnlInfo.Controls.Add(_lblVatandas);
            infoY += 25;

            _lblTC = new Label { Text = "T.C. Kimlik No: ", Font = new Font("Times New Roman", 10f), Location = new Point(15, infoY), Width = 450, ForeColor = Color.Black };
            pnlInfo.Controls.Add(_lblTC);
            infoY += 25;

            _lblAciklama = new Label { Text = "Açıklama / Gelir Türü: ", Font = new Font("Times New Roman", 10f), Location = new Point(15, infoY), Width = 450, Height = 36, ForeColor = Color.Black };
            pnlInfo.Controls.Add(_lblAciklama);
            infoY += 40;

            _lblMiktar = new Label { Text = "Tahsil Edilen Tutar: ", Font = new Font("Times New Roman", 11f, FontStyle.Bold), Location = new Point(15, infoY), Width = 450, ForeColor = Color.Black };
            pnlInfo.Controls.Add(_lblMiktar);
            infoY += 25;

            _lblMiktarYazi = new Label { Text = "Yalnız: ", Font = new Font("Times New Roman", 9.5f, FontStyle.Italic), Location = new Point(15, infoY), Width = 450, ForeColor = Color.Black };
            pnlInfo.Controls.Add(_lblMiktarYazi);

            var pnlSpacer2 = new Panel { Dock = DockStyle.Top, Height = 30 };
            _pnlA4.Controls.Add(pnlSpacer2);

            // Bottom section
            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 180, BackColor = Color.Transparent };
            _pnlA4.Controls.Add(pnlBottom);

            _lblBarkod = new Label
            {
                Text = "||||||||||||||||||||||||||||||\n[ E-Belediye Güvenli Tahsilat Barkodu ]",
                Font = new Font("Courier New", 9f),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 20),
                Width = 240,
                Height = 40,
                ForeColor = Color.Black
            };
            pnlBottom.Controls.Add(_lblBarkod);

            _lblVezne = new Label
            {
                Text = "Tahsil Eden Vezne\nE-Vezne İşlemi / Sanal POS\n\nKaşe / İmza",
                Font = new Font("Times New Roman", 10f),
                TextAlign = ContentAlignment.TopCenter,
                Location = new Point(300, 20),
                Width = 180,
                Height = 80,
                ForeColor = Color.Black
            };
            pnlBottom.Controls.Add(_lblVezne);

            var lblNotice = new Label
            {
                Text = "Bu makbuz elektronik ortamda üretilmiş olup, 5070 Sayılı Elektronik İmza Kanunu uyarınca ıslak imza yerine geçer. Aslı gibidir.",
                Font = new Font("Times New Roman", 8f, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 35,
                ForeColor = Color.DarkGray
            };
            pnlBottom.Controls.Add(lblNotice);
        }

        private void YukleMakbuzDetay()
        {
            try
            {
                var dt = BelediyeDbServisi.BorcDetayGetir(_borcId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    _lblNo.Text = "Makbuz No: MAK-" + r["Id"].ToString().PadLeft(6, '0');
                    _lblTarih.Text = "Tahsilat Tarihi: " + (r["OdenmeTarihi"] != DBNull.Value ? Convert.ToDateTime(r["OdenmeTarihi"]).ToString("dd.MM.yyyy HH:mm") : "-");
                    _lblVatandas.Text = "Mükellef Ad Soyad: " + r["VatandasAdi"].ToString();
                    _lblTC.Text = "T.C. Kimlik No: " + r["VatandasTC"].ToString();
                    _lblAciklama.Text = "Açıklama / Gelir Türü: " + r["Aciklama"].ToString();
                    decimal miktar = Convert.ToDecimal(r["Miktar"]);
                    _lblMiktar.Text = "Tahsil Edilen Tutar: " + miktar.ToString("N2") + " ₺";
                    _lblMiktarYazi.Text = "Yalnız: " + SayiyiYaziyaCevir(miktar);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Makbuz detayları yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            var printDlg = new PrintDialog { Document = _printDoc };
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                _printDoc.Print();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            using (var bmp = new Bitmap(_pnlA4.Width, _pnlA4.Height))
            {
                _pnlA4.DrawToBitmap(bmp, new Rectangle(0, 0, _pnlA4.Width, _pnlA4.Height));
                e.Graphics.DrawImage(bmp, new Point(100, 100)); // Margins
            }
        }

        // Helper to convert decimal numbers to official Turkish text amount format
        private string SayiyiYaziyaCevir(decimal tutar)
        {
            try
            {
                string sTutar = tutar.ToString("F2"); // E.g. "123.45"
                string[] parts = sTutar.Split('.');
                long lira = Convert.ToInt64(parts[0]);
                int kurus = Convert.ToInt32(parts[1]);

                string liraYazi = SayiCevir(lira) + " Lira";
                string kurusYazi = kurus > 0 ? ", " + SayiCevir(kurus) + " Kuruş" : "";
                
                return liraYazi + kurusYazi + " #";
            }
            catch
            {
                return tutar.ToString("N2") + " Lira";
            }
        }

        private string SayiCevir(long sayi)
        {
            if (sayi == 0) return "Sıfır";

            string[] birler = { "", "Bir", "İki", "Üç", "Dört", "Beş", "Altı", "Yedi", "Sekiz", "Dokuz" };
            string[] onlar = { "", "On", "Yirmi", "Otuz", "Kırk", "Elli", "Atmış", "Yetmiş", "Seksen", "Doksan" };
            string[] binler = { "", "Bin", "Milyon", "Milyar" };

            string yazi = "";
            int grup = 0;

            while (sayi > 0)
            {
                int ucluk = (int)(sayi % 1000);
                sayi /= 1000;

                if (ucluk > 0)
                {
                    int y = ucluk / 100;
                    int o = (ucluk % 100) / 10;
                    int b = ucluk % 10;

                    string uclukYazi = "";
                    if (y > 0) uclukYazi += (y > 1 ? birler[y] : "") + "Yüz";
                    uclukYazi += onlar[o];
                    uclukYazi += birler[b];

                    // "Bir Bin" to "Bin" correction
                    if (grup == 1 && uclukYazi == "Bir") uclukYazi = "";

                    yazi = uclukYazi + binler[grup] + yazi;
                }
                grup++;
            }
            return yazi;
        }
    }
}
