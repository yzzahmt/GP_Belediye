using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public class DilekceYazdirForm : Form
    {
        private readonly int _basvuruId;
        private Panel _pnlA4;
        private Label _lblTarih;
        private Label _lblNo;
        private Label _lblBaslik;
        private Label _lblVatandas;
        private Label _lblKategori;
        private RichTextBox _rtbIcerik;
        private Label _lblImza;
        private Button _btnPrint;
        private Button _btnClose;
        private PrintDocument _printDoc;

        public DilekceYazdirForm(int basvuruId)
        {
            _basvuruId = basvuruId;
            InitializeComponent();
            YukleBasvuruDetay();
        }

        private void InitializeComponent()
        {
            this.Text = "Resmi Dilekçe Önizleme ve Yazdırma";
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

            _btnPrint = new Button { Text = "🖨️ Resmi Dilekçe Yazdır", Width = 180, Height = 38, Location = new Point(15, 11) };
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
                Text = "T.C.\nBELEDİYE BAŞKANLIĞI\nYazı İşleri ve Kararlar Dairesi Başkanlığı",
                Font = new Font("Times New Roman", 12f, FontStyle.Bold),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Top,
                Height = 60,
                ForeColor = Color.Black
            };
            _pnlA4.Controls.Add(lblHeader);

            var pnlMeta = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.Transparent };
            _lblNo = new Label { Text = "Sayı: BŞV-", Font = new Font("Times New Roman", 10f), Location = new Point(5, 10), AutoSize = true, ForeColor = Color.Black };
            _lblTarih = new Label { Text = "Tarih: ", Font = new Font("Times New Roman", 10f), Location = new Point(360, 10), AutoSize = true, ForeColor = Color.Black };
            pnlMeta.Controls.AddRange(new Control[] { _lblNo, _lblTarih });
            _pnlA4.Controls.Add(pnlMeta);

            var pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 20 };
            _pnlA4.Controls.Add(pnlSpacer);

            _lblBaslik = new Label
            {
                Text = "DİLEKÇE VE BAŞVURU FORMU",
                Font = new Font("Times New Roman", 11f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30,
                ForeColor = Color.Black
            };
            _pnlA4.Controls.Add(_lblBaslik);

            _lblKategori = new Label
            {
                Text = "Konu Grubu: ",
                Font = new Font("Times New Roman", 10f, FontStyle.Italic),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = Color.Black
            };
            _pnlA4.Controls.Add(_lblKategori);

            _lblVatandas = new Label
            {
                Text = "Başvuru Sahibi: ",
                Font = new Font("Times New Roman", 10f, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = Color.Black
            };
            _pnlA4.Controls.Add(_lblVatandas);

            var pnlSpacer2 = new Panel { Dock = DockStyle.Top, Height = 15 };
            _pnlA4.Controls.Add(pnlSpacer2);

            _rtbIcerik = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Times New Roman", 11f),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                ScrollBars = RichTextBoxScrollBars.None
            };
            _pnlA4.Controls.Add(_rtbIcerik);

            _lblImza = new Label
            {
                Text = "Ad Soyad / İmza\n\n__________________",
                Font = new Font("Times New Roman", 10f),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Bottom,
                Height = 70,
                ForeColor = Color.Black
            };
            _pnlA4.Controls.Add(_lblImza);
        }

        private void YukleBasvuruDetay()
        {
            try
            {
                var dt = BelediyeDbServisi.BasvuruDetayiGetir(_basvuruId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    _lblNo.Text = "Sayı: BŞV-" + r["Id"];
                    _lblTarih.Text = "Tarih: " + Convert.ToDateTime(r["KayitTarihi"]).ToString("dd.MM.yyyy");
                    _lblBaslik.Text = r["Konu"].ToString().ToUpper();
                    _lblKategori.Text = "Konu Grubu: " + r["Kategori"];
                    _lblVatandas.Text = "Başvuru Sahibi: " + r["VatandasAdi"] + " (TC: " + r["VatandasTC"] + ")";
                    _rtbIcerik.Text = r["Aciklama"].ToString();
                    _lblImza.Text = r["VatandasAdi"] + "\nİmza\n\n__________________";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dilekçe detayları yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Simple canvas printing of the A4 panel
            using (var bmp = new Bitmap(_pnlA4.Width, _pnlA4.Height))
            {
                _pnlA4.DrawToBitmap(bmp, new Rectangle(0, 0, _pnlA4.Width, _pnlA4.Height));
                e.Graphics.DrawImage(bmp, new Point(100, 100)); // Margins
            }
        }
    }
}
