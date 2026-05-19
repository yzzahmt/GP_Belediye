using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Belediye_Otomasyonu;

namespace Belediye_Otomasyonu.Views
{
    public partial class İlkGiris : Form
    {
        private System.Windows.Forms.Timer _saatTimer;

        public İlkGiris()
        {
            InitializeComponent();
            this.Load += İlkGiris_Load;
            this.Resize += (s, e) => { this.Invalidate(); };
        }

        private void İlkGiris_Load(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.Text = "Belediye Otomasyonu — Giriş";
            this.Size = new Size(860, 600);
            this.MinimumSize = new Size(780, 540);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UiTheme.GradientTop;
            this.Font = UiTheme.UiFont;

            // Gradient arka plan
            this.Paint += (s, ee) =>
            {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(this.Width, this.Height),
                    UiTheme.GradientTop, UiTheme.GradientBottom))
                {
                    ee.Graphics.FillRectangle(br, this.ClientRectangle);
                }
                // Dekoratif daireler (arka plan süsü)
                using (var br2 = new SolidBrush(Color.FromArgb(12, 255, 255, 255)))
                {
                    ee.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    ee.Graphics.FillEllipse(br2, -80, -80, 300, 300);
                    ee.Graphics.FillEllipse(br2, this.Width - 150, this.Height - 150, 280, 280);
                    ee.Graphics.FillEllipse(br2, this.Width - 100, -60, 200, 200);
                }
            };

            OlusturIcerik();
        }

        private void OlusturIcerik()
        {
            // ── Sol dekoratif şerit ─────────────────────────────────────────
            var pnlSol = new Panel
            {
                Dock = DockStyle.Left,
                Width = 340,
                BackColor = Color.Transparent
            };
            pnlSol.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Altın dikey çizgi
                using (var pen = new Pen(Color.FromArgb(180, UiTheme.Accent), 2))
                    e.Graphics.DrawLine(pen, pnlSol.Width - 1, 60, pnlSol.Width - 1, pnlSol.Height - 60);
            };

            // Logo + başlık alanı
            var pnlLogo = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                Dock = DockStyle.Fill,
                Padding = new Padding(40, 0, 30, 0)
            };

            // Amblem / Simge
            var lblAmblem = new Label
            {
                Text = "🏛",
                Font = new Font("Segoe UI", 54f),
                ForeColor = UiTheme.Accent,
                AutoSize = true,
                Location = new Point(55, 120)
            };

            // Ana başlık
            var lblBaslik = new Label
            {
                Text = "Belediye\nOtomasyonu",
                Font = new Font("Segoe UI", 26f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(40, 215)
            };

            // Alt yazı
            var lblAlt = new Label
            {
                Text = "Dijital belediye hizmetleri\nplatformuna hoş geldiniz.",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(175, 200, 235),
                AutoSize = true,
                Location = new Point(42, 290)
            };

            // Altın çizgi süs
            var lblSus = new Label
            {
                Text = "━━━━━━━━━━━━━━━━━",
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(160, UiTheme.Accent),
                AutoSize = true,
                Location = new Point(42, 265)
            };

            // Saat/tarih
            var lblSaat = new Label
            {
                Text = DateTime.Now.ToString("dd MMMM yyyy  HH:mm", new System.Globalization.CultureInfo("tr-TR")),
                Font = UiTheme.SmallFont,
                ForeColor = Color.FromArgb(130, 165, 210),
                AutoSize = true,
                Location = new Point(42, 360)
            };

            _saatTimer = new System.Windows.Forms.Timer { Interval = 30000 };
            _saatTimer.Tick += (s, e) =>
                lblSaat.Text = DateTime.Now.ToString("dd MMMM yyyy  HH:mm", new System.Globalization.CultureInfo("tr-TR"));
            _saatTimer.Start();

            pnlLogo.Controls.AddRange(new Control[] { lblAmblem, lblBaslik, lblSus, lblAlt, lblSaat });
            pnlSol.Controls.Add(pnlLogo);
            this.Controls.Add(pnlSol);

            // ── Sağ bölüm — butonlar ────────────────────────────────────────
            var pnlSag = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(44, 0, 44, 0)
            };

            // Kart arka planı (cam efekti)
            var pnlKart = new Panel
            {
                BackColor = Color.FromArgb(18, 255, 255, 255),
                Location = new Point(30, 60),
                Size = new Size(376, 400)
            };
            pnlKart.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                    e.Graphics.FillRoundedRectangle(br, pnlKart.ClientRectangle, 16);
                using (var pen = new Pen(Color.FromArgb(35, UiTheme.Accent), 1))
                    e.Graphics.DrawRoundedRectangle(pen, new Rectangle(0, 0, pnlKart.Width - 1, pnlKart.Height - 1), 16);
            };

            var lblGiris = new Label
            {
                Text = "Giriş Seçin",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(28, 24)
            };
            var lblGirisAlt = new Label
            {
                Text = "Hesabınızla giriş yapın veya kayıt olun",
                Font = UiTheme.SmallFont,
                ForeColor = Color.FromArgb(155, 185, 220),
                AutoSize = true,
                Location = new Point(28, 52)
            };

            // Vatandaş Girişi Butonu
            var btnVatandas = OlusturGirisBtnu(
                "🧑  Vatandaş Girişi",
                "Portal üzerinden hizmetlere erişin",
                Color.FromArgb(25, 55, 120), 80);
            btnVatandas.Click += (s, e) => { new VatandasGiris().Show(); this.Hide(); };

            // Personel Girişi Butonu
            var btnPersonel = OlusturGirisBtnu(
                "👔  Personel Girişi",
                "Belediye çalışanları için panel",
                Color.FromArgb(18, 90, 160), 170);
            btnPersonel.Click += (s, e) => { new PersonelGiris().Show(); this.Hide(); };

            // Kayıt Ol Butonu
            var btnKayit = OlusturGirisBtnu(
                "✏️  Kayıt Ol",
                "Yeni vatandaş hesabı oluştur",
                Color.FromArgb(15, 110, 80), 260);
            btnKayit.Click += (s, e) => { new Kayit_Screen().Show(); this.Hide(); };

            pnlKart.Controls.AddRange(new Control[] { lblGiris, lblGirisAlt, btnVatandas, btnPersonel, btnKayit });
            pnlSag.Controls.Add(pnlKart);

            // Yönetici küçük link
            var btnYonetici = new Button
            {
                Text = "🔑  Yönetici Girişi",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(130, 170, 220),
                Font = UiTheme.SmallFont,
                Cursor = Cursors.Hand,
                AutoSize = true,
                Location = new Point(30, 472),
                UseVisualStyleBackColor = false
            };
            btnYonetici.FlatAppearance.BorderSize = 0;
            btnYonetici.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 255, 255, 255);
            btnYonetici.Click += (s, e) =>
            {
                var yg = new YoneticiGiris();
                yg.Show();
                this.Hide();
            };
            pnlSag.Controls.Add(btnYonetici);

            this.Controls.Add(pnlSag);
            this.Controls.Add(pnlSol);
        }

        private Panel OlusturGirisBtnu(string baslik, string altYazi, Color renk, int y)
        {
            var pnl = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(332, 78),
                BackColor = renk,
                Cursor = Cursors.Hand
            };
            pnl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(pnl.Width, 0),
                    renk, ControlPaint.Light(renk, 0.2f)))
                    e.Graphics.FillRoundedRectangle(br, pnl.ClientRectangle, 10);
                // Sol altın şerit
                using (var br2 = new SolidBrush(UiTheme.Accent))
                    e.Graphics.FillRectangle(br2, 0, 0, 4, pnl.Height);
            };

            var lblB = new Label
            {
                Text = baslik,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(18, 14),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            var lblA = new Label
            {
                Text = altYazi,
                Font = UiTheme.SmallFont,
                ForeColor = Color.FromArgb(190, 220, 255),
                AutoSize = true,
                Location = new Point(18, 42),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            var lblArr = new Label
            {
                Text = "›",
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, UiTheme.Accent),
                AutoSize = true,
                Location = new Point(296, 22),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            pnl.Controls.AddRange(new Control[] { lblB, lblA, lblArr });

            // Hover efekti
            MouseEventHandler hover = (s, e) => { pnl.Invalidate(); };
            Action hoverIn = () =>
            {
                pnl.BackColor = ControlPaint.Light(renk, 0.15f);
                pnl.Invalidate();
            };
            Action hoverOut = () =>
            {
                pnl.BackColor = renk;
                pnl.Invalidate();
            };

            foreach (Control c in new Control[] { pnl, lblB, lblA, lblArr })
            {
                c.MouseEnter += (s, e) => hoverIn();
                c.MouseLeave += (s, e) => hoverOut();
            }

            return pnl;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _saatTimer?.Stop();
            _saatTimer?.Dispose();
            base.OnFormClosed(e);
        }
    }

    // ── Graphics Extension ────────────────────────────────────────────────────
    internal static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (var path = RoundedRect(rect, radius))
                g.FillPath(brush, path);
        }

        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using (var path = RoundedRect(rect, radius))
                g.DrawPath(pen, path);
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
