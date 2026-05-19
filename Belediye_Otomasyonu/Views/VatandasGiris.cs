using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public partial class VatandasGiris : Form
    {
        public VatandasGiris()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Load += (s, e) => OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Controls.Clear();
            this.Text = "Vatandas Girisi";
            this.Size = new Size(820, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UiTheme.GradientTop;
            this.Font = UiTheme.UiFont;
            this.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(this.Width, this.Height), UiTheme.GradientTop, UiTheme.GradientBottom))
                    e.Graphics.FillRectangle(br, this.ClientRectangle);
                using (var br2 = new SolidBrush(Color.FromArgb(10, 255, 255, 255))) {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(br2, -60, -60, 260, 260);
                    e.Graphics.FillEllipse(br2, this.Width - 180, this.Height - 180, 280, 280);
                }
            };

            // Sol panel - dekoratif
            var pnlSol = new Panel { Dock = DockStyle.Left, Width = 320, BackColor = Color.Transparent };
            pnlSol.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(60, UiTheme.Accent), 2))
                    e.Graphics.DrawLine(pen, pnlSol.Width - 1, 60, pnlSol.Width - 1, pnlSol.Height - 60);
            };
            var lblAmb  = new Label { Text = "🏛", Font = new Font("Segoe UI", 48f), ForeColor = UiTheme.Accent, AutoSize = true, Location = new Point(60, 120) };
            var lblBas  = new Label { Text = "Vatandaş\nGirişi", Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(48, 210) };
            var lblAlt  = new Label { Text = "Belediye Portalına\nhoş geldiniz.", Font = UiTheme.UiFont, ForeColor = Color.FromArgb(170, 200, 235), AutoSize = true, Location = new Point(50, 310) };
            pnlSol.Controls.AddRange(new Control[] { lblAmb, lblBas, lblAlt });
            this.Controls.Add(pnlSol);

            // Sag panel - kart
            var pnlSag = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(36, 0, 36, 0) };

            var kart = new Panel { Location = new Point(26, 60), Size = new Size(360, 390), BackColor = Color.Transparent };
            kart.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                    FillRounded(e.Graphics, br, kart.ClientRectangle, 14);
                using (var pen = new Pen(Color.FromArgb(40, UiTheme.Accent), 1))
                    DrawRounded(e.Graphics, pen, new Rectangle(0, 0, kart.Width - 1, kart.Height - 1), 14);
            };

            var lblTitle = new Label { Text = "Giriş Yap", Font = new Font("Segoe UI", 16f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(28, 22) };

            var lblTC = new Label { Text = "TC Kimlik No", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(180, 210, 240), AutoSize = true, Location = new Point(28, 68) };
            var txtTC = new TextBox { Location = new Point(28, 88), Width = 304, Height = 36, Font = UiTheme.UiFont, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, MaxLength = 11 };
            StilleTextBox(txtTC);

            var lblSifre = new Label { Text = "Şifre", Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(180, 210, 240), AutoSize = true, Location = new Point(28, 140) };
            var txtSifre = new TextBox { Location = new Point(28, 160), Width = 304, Height = 36, Font = UiTheme.UiFont, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, UseSystemPasswordChar = true };
            StilleTextBox(txtSifre);

            var btnGiris = new Button { Text = "Giriş Yap", Location = new Point(28, 220), Width = 304, Height = 46 };
            UiTheme.AnaEylemButonu(btnGiris);
            btnGiris.BackColor = UiTheme.Primary;

            var lnkSifre = new LinkLabel { Text = "Şifremi Unuttum", Location = new Point(28, 278), AutoSize = true, Font = UiTheme.SmallFont };
            lnkSifre.LinkColor = Color.FromArgb(150, 190, 240);
            lnkSifre.LinkClicked += (s, e) => { using (var f = new SifremiUnuttumForm()) f.ShowDialog(this); };

            var btnGeri = new Button { Text = "← Geri", Location = new Point(28, 316), Width = 140, Height = 40 };
            UiTheme.IkincilButon(btnGeri);
            btnGeri.BackColor = Color.FromArgb(25, 255, 255, 255);
            btnGeri.ForeColor = Color.FromArgb(180, 210, 240);
            btnGeri.FlatAppearance.BorderColor = Color.FromArgb(60, 255, 255, 255);
            btnGeri.Click += (s, e) => { new İlkGiris().Show(); this.Hide(); };

            btnGiris.Click += (s, e) => {
                try {
                    if (BelediyeDbServisi.VatandasGirisDogrula(txtTC.Text, txtSifre.Text)) {
                        new VatandasHomeScreen(txtTC.Text.Trim()).Show(); this.Hide();
                    } else {
                        MessageBox.Show("TC kimlik veya şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            kart.Controls.AddRange(new Control[] { lblTitle, lblTC, txtTC, lblSifre, txtSifre, btnGiris, lnkSifre, btnGeri });
            pnlSag.Controls.Add(kart);
            this.Controls.Add(pnlSag);
            this.Controls.Add(pnlSol);
        }

        private void StilleTextBox(TextBox t) {
            UiTheme.TextBoxKaranlikStil(t);
        }

        private void FillRounded(Graphics g, Brush b, Rectangle r, int rad) {
            int d = rad * 2;
            using (var p = new System.Drawing.Drawing2D.GraphicsPath()) {
                p.AddArc(r.X, r.Y, d, d, 180, 90);
                p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                p.CloseFigure(); g.FillPath(b, p);
            }
        }
        private void DrawRounded(Graphics g, Pen pen, Rectangle r, int rad) {
            int d = rad * 2;
            using (var p = new System.Drawing.Drawing2D.GraphicsPath()) {
                p.AddArc(r.X, r.Y, d, d, 180, 90);
                p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                p.CloseFigure(); g.DrawPath(pen, p);
            }
        }

        // Eski designer referansları için stub
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void Geri_btn_Click(object sender, EventArgs e) { }
        private void lblSifremiUnuttum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { }
        private void btnGirisYap_Click(object sender, EventArgs e) { }
        private void VatandasGiris_Load(object sender, EventArgs e) { }
    }
}
