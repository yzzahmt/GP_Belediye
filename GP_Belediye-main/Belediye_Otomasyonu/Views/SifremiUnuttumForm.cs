using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class SifremiUnuttumForm : Form
    {
        private string uretilenKod = "";

        public SifremiUnuttumForm()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Load += (s, e) => OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Controls.Clear();
            this.Text = "Şifremi Unuttum";
            this.Size = new Size(500, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = UiTheme.GradientTop;
            this.Font = UiTheme.UiFont;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(this.Width, this.Height),
                    Color.FromArgb(20, 25, 60), Color.FromArgb(40, 50, 90)))
                    e.Graphics.FillRectangle(br, this.ClientRectangle);
            };

            var kart = new Panel { Location = new Point(40, 30), Size = new Size(400, 380), BackColor = Color.Transparent };
            kart.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(25, 255, 255, 255)))
                    FillRounded(e.Graphics, br, kart.ClientRectangle, 12);
                using (var pen = new Pen(Color.FromArgb(40, 255, 255, 255), 1))
                    DrawRounded(e.Graphics, pen, new Rectangle(0, 0, kart.Width-1, kart.Height-1), 12);
            };

            var lblIcon = new Label { Text = "🔒", Font = new Font("Segoe UI", 32f), ForeColor = UiTheme.Accent, AutoSize = true, Location = new Point(170, 14) };
            var lblTitle = new Label { Text = "Şifrenizi Mi Unuttunuz?", Font = new Font("Segoe UI", 14f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(90, 74) };
            var lblDesc = new Label { Text = "Hesabınızı kurtarmak için kayıtlı\nTC Kimlik ve E-Posta adresinizi girin.", Font = UiTheme.SmallFont, ForeColor = Color.FromArgb(170, 200, 240), AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Location = new Point(90, 106) };

            var lblTC = new Label { Text = "TC Kimlik No", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(180, 210, 240), AutoSize = true, Location = new Point(38, 160) };
            var txtTc = new TextBox { Location = new Point(38, 180), Width = 324, Height = 36, Font = UiTheme.UiFont, BackColor = Color.FromArgb(15, 25, 65), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, MaxLength = 11 };

            var lblEmail = new Label { Text = "E-Posta Adresi", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(180, 210, 240), AutoSize = true, Location = new Point(38, 230) };
            var txtEmail = new TextBox { Location = new Point(38, 250), Width = 324, Height = 36, Font = UiTheme.UiFont, BackColor = Color.FromArgb(15, 25, 65), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            var btnGonder = new Button { Text = "  Sıfırlama Kodu Gönder", Location = new Point(38, 310), Width = 210, Height = 44 };
            UiTheme.AnaEylemButonu(btnGonder);

            var btnKapat = new Button { Text = "İptal", Location = new Point(260, 310), Width = 102, Height = 44, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(20, 255,255,255), ForeColor = Color.FromArgb(180, 210, 240), Font = UiTheme.UiFont, Cursor = Cursors.Hand };
            btnKapat.FlatAppearance.BorderColor = Color.FromArgb(40, 255,255,255); btnKapat.FlatAppearance.BorderSize = 1;

            btnKapat.Click += (s, e) => this.Close();

            btnGonder.Click += (s, e) => {
                if (txtTc.Text.Trim().Length != 11) { MessageBox.Show("TC kimlik 11 haneli olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { MessageBox.Show("Geçerli bir e-posta girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                btnGonder.Text = "Gönderiliyor...";
                btnGonder.Enabled = false;
                Application.DoEvents();

                try {
                    uretilenKod = new Random().Next(100000, 999999).ToString();
                    SmtpClient istemci = new SmtpClient("smtp.gmail.com", 587) { EnableSsl = true, Credentials = new NetworkCredential("uskudarbelediyesi51@gmail.com", "vfngrlfhzdfvxvbh") };
                    var mesaj = new MailMessage { From = new MailAddress("uskudarbelediyesi51@gmail.com", "Üsküdar Belediyesi Otomasyon"), Subject = "Güvenlik Kodu - Şifre Sıfırlama İşlemi", Body = $"Sayın kullanıcımız,\n\nŞifre sıfırlama işleminiz için tek kullanımlık güvenlik kodunuz: {uretilenKod}\n\nLütfen bu kodu kimseyle paylaşmayınız." };
                    mesaj.To.Add(txtEmail.Text.Trim());
                    istemci.Send(mesaj);
                    
                    MessageBox.Show("Güvenlik kodu e-posta adresinize gönderildi! Gelen kutunuzu veya Spam klasörünü kontrol edin.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var yeniSayfa = new YeniSifreForm(uretilenKod, txtTc.Text.Trim(), txtEmail.Text.Trim());
                    this.Hide(); yeniSayfa.ShowDialog(); this.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Mail gönderilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } finally {
                    btnGonder.Text = "  Sıfırlama Kodu Gönder";
                    btnGonder.Enabled = true;
                }
            };

            kart.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblDesc, lblTC, txtTc, lblEmail, txtEmail, btnGonder, btnKapat });
            this.Controls.Add(kart);
        }

        private void FillRounded(Graphics g, Brush b, Rectangle r, int rad) {
            int d = rad*2; using (var p = new GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.FillPath(b,p); }
        }
        private void DrawRounded(Graphics g, Pen pen, Rectangle r, int rad) {
            int d = rad*2; using (var p = new GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.DrawPath(pen,p); }
        }

        // Stublar
        private void SifremiUnuttumForm_Load(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void btnIptal_Click(object sender, EventArgs e) { }
    }
}
