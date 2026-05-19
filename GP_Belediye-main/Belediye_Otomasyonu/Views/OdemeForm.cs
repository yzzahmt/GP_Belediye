using System;
using System.Drawing;
using System.Windows.Forms;

namespace Belediye_Otomasyonu.Views
{
    public class OdemeForm : Form
    {
        private readonly decimal _miktar;
        private readonly string _aciklama;

        private Label _lblMiktar;
        private TextBox _txtKartNo;
        private TextBox _txtAdSoyad;
        private TextBox _txtSkt;
        private TextBox _txtCvv;
        private Button _btnOde;
        private Button _btnIptal;
        private ProgressBar _progOde;
        private Label _lblDurum;
        private Timer _payTimer;
        private int _tickCount = 0;

        public OdemeForm(string aciklama, decimal miktar)
        {
            _aciklama = aciklama;
            _miktar = miktar;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Güvenli Ödeme Geçidi (Simülasyon)";
            this.Size = new Size(420, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 250);

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(15, 34, 64)
            };
            var lblTitle = new Label
            {
                Text = "💳 Belediye Tahsilat Servisi",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 22),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24)
            };
            this.Controls.Add(pnlBody);

            int y = 10;

            var lblAcik = new Label
            {
                Text = "Ödeme Detayı: " + _aciklama,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 110, 120),
                Location = new Point(24, y),
                Width = 360,
                Height = 36
            };
            pnlBody.Controls.Add(lblAcik);
            y += 40;

            _lblMiktar = new Label
            {
                Text = "Ödenecek Tutar: " + _miktar.ToString("N2") + " ₺",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 117, 89), // Success color
                Location = new Point(24, y),
                Width = 360,
                AutoSize = true
            };
            pnlBody.Controls.Add(_lblMiktar);
            y += 40;

            // Card Number
            var lblKartNo = new Label
            {
                Text = "Kart Numarası",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 80, 95),
                Location = new Point(24, y),
                Width = 360,
                Height = 18
            };
            pnlBody.Controls.Add(lblKartNo);
            y += 20;

            _txtKartNo = new TextBox
            {
                Location = new Point(24, y),
                Width = 350,
                Font = new Font("Segoe UI", 11f),
                MaxLength = 19,
                BorderStyle = BorderStyle.FixedSingle
            };
            _txtKartNo.TextChanged += TxtKartNo_TextChanged;
            pnlBody.Controls.Add(_txtKartNo);
            y += 35;

            // Ad Soyad
            var lblAd = new Label
            {
                Text = "Kart Sahibi Ad Soyad",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 80, 95),
                Location = new Point(24, y),
                Width = 360,
                Height = 18
            };
            pnlBody.Controls.Add(lblAd);
            y += 20;

            _txtAdSoyad = new TextBox
            {
                Location = new Point(24, y),
                Width = 350,
                Font = new Font("Segoe UI", 11f),
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlBody.Controls.Add(_txtAdSoyad);
            y += 35;

            // SKT ve CVV yan yana
            var lblSkt = new Label
            {
                Text = "Son Kul. Tar. (AA/YY)",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 80, 95),
                Location = new Point(24, y),
                Width = 160,
                Height = 18
            };
            pnlBody.Controls.Add(lblSkt);

            var lblCvv = new Label
            {
                Text = "CVV (CVC)",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 80, 95),
                Location = new Point(200, y),
                Width = 160,
                Height = 18
            };
            pnlBody.Controls.Add(lblCvv);
            y += 20;

            _txtSkt = new TextBox
            {
                Location = new Point(24, y),
                Width = 150,
                Font = new Font("Segoe UI", 11f),
                MaxLength = 5,
                BorderStyle = BorderStyle.FixedSingle
            };
            _txtSkt.TextChanged += TxtSkt_TextChanged;
            pnlBody.Controls.Add(_txtSkt);

            _txtCvv = new TextBox
            {
                Location = new Point(200, y),
                Width = 174,
                Font = new Font("Segoe UI", 11f),
                MaxLength = 3,
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlBody.Controls.Add(_txtCvv);
            y += 45;

            // Progress bar and loading state
            _progOde = new ProgressBar
            {
                Location = new Point(24, y),
                Width = 350,
                Height = 16,
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };
            pnlBody.Controls.Add(_progOde);

            _lblDurum = new Label
            {
                Text = "Güvenli ödeme bağlantısı kuruluyor...",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 110, 120),
                Location = new Point(24, y + 20),
                Width = 350,
                Height = 18,
                Visible = false
            };
            pnlBody.Controls.Add(_lblDurum);
            y += 40;

            // Buttons
            _btnOde = new Button
            {
                Text = "Ödemeyi Tamamla (" + _miktar.ToString("N2") + " ₺)",
                Location = new Point(24, y),
                Width = 220,
                Height = 38
            };
            UiTheme.BasariButon(_btnOde);
            _btnOde.Click += BtnOde_Click;
            pnlBody.Controls.Add(_btnOde);

            _btnIptal = new Button
            {
                Text = "İptal",
                Location = new Point(260, y),
                Width = 114,
                Height = 38
            };
            UiTheme.IkincilButon(_btnIptal);
            _btnIptal.Click += (s, e) => this.Close();
            pnlBody.Controls.Add(_btnIptal);

            _payTimer = new Timer { Interval = 1000 };
            _payTimer.Tick += PayTimer_Tick;
        }

        private void TxtKartNo_TextChanged(object sender, EventArgs e)
        {
            // Simple formatting for card number digits (e.g. 1234-5678-...)
            string raw = _txtKartNo.Text.Replace("-", "").Replace(" ", "");
            if (raw.Length > 0 && raw.Length % 4 == 0 && _txtKartNo.Text.Length < 19 && !raw.EndsWith("-"))
            {
                _txtKartNo.Text += "-";
                _txtKartNo.SelectionStart = _txtKartNo.Text.Length;
            }
        }

        private void TxtSkt_TextChanged(object sender, EventArgs e)
        {
            // Simple formatting for expiry MM/YY
            string raw = _txtSkt.Text.Replace("/", "");
            if (raw.Length == 2 && _txtSkt.Text.Length < 5 && !_txtSkt.Text.EndsWith("/"))
            {
                _txtSkt.Text += "/";
                _txtSkt.SelectionStart = _txtSkt.Text.Length;
            }
        }

        private void BtnOde_Click(object sender, EventArgs e)
        {
            string kart = _txtKartNo.Text.Replace("-", "").Trim();
            string ad = _txtAdSoyad.Text.Trim();
            string skt = _txtSkt.Text.Trim();
            string cvv = _txtCvv.Text.Trim();

            if (kart.Length < 16 || ad.Length < 5 || skt.Length < 5 || cvv.Length < 3)
            {
                MessageBox.Show("Lütfen kredi kartı bilgilerini eksiksiz ve doğru doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Start simulated process
            _btnOde.Enabled = false;
            _btnIptal.Enabled = false;
            _txtKartNo.Enabled = false;
            _txtAdSoyad.Enabled = false;
            _txtSkt.Enabled = false;
            _txtCvv.Enabled = false;

            _progOde.Visible = true;
            _lblDurum.Visible = true;
            _tickCount = 0;
            _payTimer.Start();
        }

        private void PayTimer_Tick(object sender, EventArgs e)
        {
            _tickCount++;
            if (_tickCount == 1)
            {
                _lblDurum.Text = "Banka ödeme onayı bekleniyor...";
            }
            else if (_tickCount == 2)
            {
                _lblDurum.Text = "Ödeme onaylandı. Makbuz üretiliyor...";
            }
            else if (_tickCount == 3)
            {
                _payTimer.Stop();
                MessageBox.Show("Ödemeniz başarıyla tahsil edilmiştir. Makbuzunuzu yazdırabilirsiniz.", "Başarılı Ödeme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
