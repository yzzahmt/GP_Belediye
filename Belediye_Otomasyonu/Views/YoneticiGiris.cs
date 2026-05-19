using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class YoneticiGiris : Form
    {
        public YoneticiGiris()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Load += (s, e) => OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Controls.Clear();
            this.Text = "Yönetici Girisi";
            this.Size = new Size(820, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UiTheme.GradientTop;
            this.Font = UiTheme.UiFont;
            this.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(this.Width, this.Height),
                    Color.FromArgb(15, 10, 40), Color.FromArgb(35, 20, 80)))
                    e.Graphics.FillRectangle(br, this.ClientRectangle);
                using (var br2 = new SolidBrush(Color.FromArgb(10, 255, 255, 255))) {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(br2, -60, -80, 280, 280);
                    e.Graphics.FillEllipse(br2, this.Width - 220, this.Height - 180, 300, 300);
                }
            };

            // Sol dekoratif
            var pnlSol = new Panel { Dock = DockStyle.Left, Width = 300, BackColor = Color.Transparent };
            pnlSol.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(50, 180, 120, 255), 2))
                    e.Graphics.DrawLine(pen, pnlSol.Width - 1, 60, pnlSol.Width - 1, pnlSol.Height - 60);
            };
            pnlSol.Controls.Add(new Label { Text = "👑", Font = new Font("Segoe UI", 48f), ForeColor = Color.FromArgb(180, 120, 255), AutoSize = true, Location = new Point(55, 110) });
            pnlSol.Controls.Add(new Label { Text = "Yönetici\nGirişi", Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(44, 200) });
            pnlSol.Controls.Add(new Label { Text = "Sistem kontrol paneli\nve üst düzey yetki.", Font = UiTheme.UiFont, ForeColor = Color.FromArgb(200, 180, 230), AutoSize = true, Location = new Point(46, 310) });
            this.Controls.Add(pnlSol);

            // Sağ kart
            var pnlSag = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var kart = new Panel { Location = new Point(26, 50), Size = new Size(364, 430), BackColor = Color.Transparent };
            kart.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                    FillRounded(e.Graphics, br, kart.ClientRectangle, 14);
                using (var pen = new Pen(Color.FromArgb(60, 180, 120, 255), 1))
                    DrawRounded(e.Graphics, pen, new Rectangle(0, 0, kart.Width-1, kart.Height-1), 14);
            };

            kart.Controls.Add(new Label { Text = "Yönetici Girişi", Font = new Font("Segoe UI", 16f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(28, 22) });

            var lblKadi  = new Label { Text = "Kullanıcı Adı", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(200, 180, 230), AutoSize = true, Location = new Point(28, 74) };
            var txtKadi  = Txt(28, 94);

            var lblSifre = new Label { Text = "Şifre", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(200, 180, 230), AutoSize = true, Location = new Point(28, 146) };
            var txtSifre = Txt(28, 166); txtSifre.UseSystemPasswordChar = true;

            var btnGiris = new Button { Text = "  Giriş Yap", Location = new Point(28, 224), Width = 308, Height = 46 };
            UiTheme.YuvarlakButon(btnGiris, Color.FromArgb(120, 60, 220), Color.White, 6);

            var sep = new Label { Text = "──────────────────────────────", Font = new Font("Segoe UI", 7f), ForeColor = Color.FromArgb(60, 255,255,255), AutoSize = true, Location = new Point(28, 282) };

            var lnkSifre = new LinkLabel { Text = "Şifremi Unuttum", Location = new Point(28, 316), AutoSize = true, Font = UiTheme.SmallFont };
            lnkSifre.LinkColor = Color.FromArgb(180, 160, 240);
            lnkSifre.LinkClicked += (s, e) => { using (var f = new SifremiUnuttumForm()) f.ShowDialog(this); };

            var btnGeri = new Button { Text = "← Geri", Location = new Point(206, 306), Width = 130, Height = 40 };
            UiTheme.YuvarlakButonBorder(btnGeri, Color.Transparent, Color.FromArgb(200, 180, 230), Color.FromArgb(80, 255, 255, 255), 6);
            btnGeri.Click += (s, e) => { new PersonelGiris().Show(); this.Hide(); };

            btnGiris.Click += (s, e) => {
                try {
                    if (BelediyeDbServisi.YoneticiGirisDogrula(txtKadi.Text.Trim(), txtSifre.Text)) {
                        var p = new PersonelHomeScreen(txtKadi.Text.Trim(), true);
                        this.Hide(); p.ShowDialog(); this.Close();
                    } else { MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                } catch (Exception ex) { MessageBox.Show("Bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            kart.Controls.AddRange(new Control[] { lblKadi, txtKadi, lblSifre, txtSifre, btnGiris, sep, lnkSifre, btnGeri });
            pnlSag.Controls.Add(kart);
            this.Controls.Add(pnlSag);
            this.Controls.Add(pnlSol);
        }

        private TextBox Txt(int x, int y) {
            var t = new TextBox { Location = new Point(x, y), Width = 308, Height = 36 };
            UiTheme.TextBoxKaranlikStil(t);
            t.BackColor = Color.FromArgb(30, 20, 60);
            t.GotFocus += (s, e) => t.BackColor = Color.FromArgb(40, 25, 80);
            t.LostFocus += (s, e) => t.BackColor = Color.FromArgb(30, 20, 60);
            return t;
        }
        private void FillRounded(Graphics g, Brush b, Rectangle r, int rad) {
            int d = rad*2; using (var p = new GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.FillPath(b,p); }
        }
        private void DrawRounded(Graphics g, Pen pen, Rectangle r, int rad) {
            int d = rad*2; using (var p = new GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.DrawPath(pen,p); }
        }

        // Eski stublar
        private void YoneticiGiris_Resize(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void btnGirisYonetici_Click(object sender, EventArgs e) { }
    }
}
