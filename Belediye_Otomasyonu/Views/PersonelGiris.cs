using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class PersonelGiris : Form
    {
        public PersonelGiris()
        {
            InitializeComponent();
            this.Load += (s, e) => OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Controls.Clear();
            this.Text = "Personel Girisi";
            this.Size = new Size(820, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UiTheme.GradientTop;
            this.Font = UiTheme.UiFont;
            this.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(this.Width, this.Height),
                    Color.FromArgb(8, 18, 55), Color.FromArgb(20, 45, 100)))
                    e.Graphics.FillRectangle(br, this.ClientRectangle);
                using (var br2 = new SolidBrush(Color.FromArgb(10, 255, 255, 255))) {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(br2, this.Width - 200, -80, 280, 280);
                    e.Graphics.FillEllipse(br2, -60, this.Height - 180, 260, 260);
                }
            };

            // Sol dekoratif
            var pnlSol = new Panel { Dock = DockStyle.Left, Width = 300, BackColor = Color.Transparent };
            pnlSol.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(60, UiTheme.Accent), 2))
                    e.Graphics.DrawLine(pen, pnlSol.Width - 1, 60, pnlSol.Width - 1, pnlSol.Height - 60);
            };
            pnlSol.Controls.Add(new Label { Text = "👔", Font = new Font("Segoe UI", 48f), ForeColor = UiTheme.Accent, AutoSize = true, Location = new Point(55, 110) });
            pnlSol.Controls.Add(new Label { Text = "Personel\nGirişi", Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(44, 200) });
            pnlSol.Controls.Add(new Label { Text = "Belediye çalışanları\niçin yönetim paneli.", Font = UiTheme.UiFont, ForeColor = Color.FromArgb(160, 195, 235), AutoSize = true, Location = new Point(46, 310) });
            this.Controls.Add(pnlSol);

            // Sağ kart
            var pnlSag = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var kart = new Panel { Location = new Point(26, 40), Size = new Size(364, 460), BackColor = Color.Transparent };
            kart.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                    FillRounded(e.Graphics, br, kart.ClientRectangle, 14);
                using (var pen = new Pen(Color.FromArgb(40, UiTheme.Accent), 1))
                    DrawRounded(e.Graphics, pen, new Rectangle(0, 0, kart.Width-1, kart.Height-1), 14);
            };

            kart.Controls.Add(new Label { Text = "Personel Girişi", Font = new Font("Segoe UI", 16f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(28, 20) });

            var lblKadi  = new Label { Text = "Kullanıcı Adı", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(170, 205, 240), AutoSize = true, Location = new Point(28, 64) };
            var txtKadi  = Txt(28, 84);

            var lblSifre = new Label { Text = "Şifre", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(170, 205, 240), AutoSize = true, Location = new Point(28, 136) };
            var txtSifre = Txt(28, 156); txtSifre.UseSystemPasswordChar = true;

            var btnGiris = new Button { Text = "  Giriş Yap", Location = new Point(28, 214), Width = 308, Height = 46 };
            UiTheme.AnaEylemButonu(btnGiris);

            var sep = new Label { Text = "──────────────────────────────", Font = new Font("Segoe UI", 7f), ForeColor = Color.FromArgb(60, 255,255,255), AutoSize = true, Location = new Point(28, 272) };

            var lnkSifre = new LinkLabel { Text = "Şifremi Unuttum", Location = new Point(28, 306), AutoSize = true, Font = UiTheme.SmallFont };
            lnkSifre.LinkColor = Color.FromArgb(150, 190, 240);
            lnkSifre.LinkClicked += (s, e) => { using (var f = new SifremiUnuttumForm()) f.ShowDialog(this); };

            var btnGeri = DarkBtn("← Geri", 206, 296, 130);
            btnGeri.Click += (s, e) => { new İlkGiris().Show(); this.Hide(); };

            var sep2 = new Panel { Location = new Point(28, 350), Size = new Size(308, 1), BackColor = Color.FromArgb(40, 255, 255, 255) };

            var lblYon = new Label { Text = "Yönetici misiniz?", Font = UiTheme.SmallFont, ForeColor = Color.FromArgb(140, 180, 220), AutoSize = true, Location = new Point(28, 364) };
            var btnYon = new Button { Text = "  Yönetici Girişi →", Location = new Point(28, 384), Width = 308, Height = 44 };
            btnYon.FlatStyle = FlatStyle.Flat;
            btnYon.BackColor = Color.FromArgb(25, UiTheme.Accent);
            btnYon.ForeColor = UiTheme.Accent;
            btnYon.FlatAppearance.BorderColor = Color.FromArgb(80, UiTheme.Accent);
            btnYon.FlatAppearance.BorderSize = 1;
            btnYon.Font = UiTheme.UiFontBold;
            btnYon.Cursor = Cursors.Hand;
            btnYon.Click += (s, e) => { var yg = new YoneticiGiris(); this.Hide(); yg.ShowDialog(); this.Close(); };

            btnGiris.Click += (s, e) => {
                try {
                    if (BelediyeDbServisi.PersonelGirisDogrula(txtKadi.Text.Trim(), txtSifre.Text)) {
                        var p = new PersonelHomeScreen(txtKadi.Text.Trim(), false);
                        this.Hide(); p.ShowDialog(); this.Close();
                    } else { MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                } catch (Exception ex) { MessageBox.Show("Bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            kart.Controls.AddRange(new Control[] { lblKadi, txtKadi, lblSifre, txtSifre, btnGiris, sep, lnkSifre, btnGeri, sep2, lblYon, btnYon });
            pnlSag.Controls.Add(kart);
            this.Controls.Add(pnlSag);
            this.Controls.Add(pnlSol);
        }

        private TextBox Txt(int x, int y) {
            var t = new TextBox { Location = new Point(x, y), Width = 308, Height = 36, Font = UiTheme.UiFont, BackColor = Color.FromArgb(18, 35, 75), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            return t;
        }
        private Button DarkBtn(string text, int x, int y, int w) {
            var b = new Button { Text = text, Location = new Point(x, y), Width = w, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(20, 255,255,255), ForeColor = Color.FromArgb(170, 205, 240), Font = UiTheme.UiFont, Cursor = Cursors.Hand };
            b.FlatAppearance.BorderColor = Color.FromArgb(40, 255,255,255); b.FlatAppearance.BorderSize = 1; return b;
        }
        private void FillRounded(Graphics g, Brush b, Rectangle r, int rad) {
            int d = rad*2; using (var p = new System.Drawing.Drawing2D.GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.FillPath(b,p); }
        }
        private void DrawRounded(Graphics g, Pen pen, Rectangle r, int rad) {
            int d = rad*2; using (var p = new System.Drawing.Drawing2D.GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.DrawPath(pen,p); }
        }
        // Eski stub'lar
        private void btnSifremiUnuttum_Click(object sender, EventArgs e) { }
        private void btnGirisPrsn_Click(object sender, EventArgs e) { }
        private void btnYoneticiGiris_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
    }
}