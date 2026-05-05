using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Belediye_Otomasyonu;
using Belediye_Otomasyonu.Models;
using Belediye_Otomasyonu.Controllers;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu
{
    public partial class Kayit_Screen : Form
    {
        public Kayit_Screen()
        {
            InitializeComponent();
            this.Load += (s, e) => OlusturArayuz();
        }

        private void OlusturArayuz()
        {
            this.Controls.Clear();
            this.Text = "Kayit Ol";
            this.Size = new Size(900, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UiTheme.GradientTop;
            this.Font = UiTheme.UiFont;
            this.Paint += (s, e) => {
                using (var br = new LinearGradientBrush(new Point(0,0), new Point(this.Width, this.Height),
                    Color.FromArgb(8, 20, 60), Color.FromArgb(15, 40, 90)))
                    e.Graphics.FillRectangle(br, this.ClientRectangle);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br2 = new SolidBrush(Color.FromArgb(10, 255, 255, 255))) {
                    e.Graphics.FillEllipse(br2, -80, -80, 300, 300);
                    e.Graphics.FillEllipse(br2, this.Width - 200, this.Height - 200, 300, 300);
                }
            };

            // Sol bilgi paneli
            var pnlSol = new Panel { Dock = DockStyle.Left, Width = 280, BackColor = Color.Transparent };
            pnlSol.Paint += (s, e) => {
                using (var pen = new Pen(Color.FromArgb(50, UiTheme.Accent), 2))
                    e.Graphics.DrawLine(pen, pnlSol.Width-1, 60, pnlSol.Width-1, pnlSol.Height-60);
            };
            pnlSol.Controls.Add(new Label { Text = "✏️", Font = new Font("Segoe UI", 44f), ForeColor = UiTheme.Accent, AutoSize = true, Location = new Point(50, 100) });
            pnlSol.Controls.Add(new Label { Text = "Yeni\nHesap Oluştur", Font = new Font("Segoe UI", 20f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(36, 190) });
            pnlSol.Controls.Add(new Label { Text = "Belediye portalına\nücretsiz kayıt olun.", Font = UiTheme.UiFont, ForeColor = Color.FromArgb(160, 195, 235), AutoSize = true, Location = new Point(38, 300) });
            this.Controls.Add(pnlSol);

            // Sağ kart
            var pnlSag = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var kart = new Panel { Location = new Point(20, 30), Size = new Size(560, 530), BackColor = Color.Transparent };
            kart.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                    FillRounded(e.Graphics, br, kart.ClientRectangle, 14);
                using (var pen = new Pen(Color.FromArgb(40, UiTheme.Accent), 1))
                    DrawRounded(e.Graphics, pen, new Rectangle(0, 0, kart.Width-1, kart.Height-1), 14);
            };

            kart.Controls.Add(new Label { Text = "Kayıt Bilgileri", Font = new Font("Segoe UI", 15f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(24, 18) });

            // 2 sütun grid ile alanlar
            var tbl = new TableLayoutPanel { Location = new Point(20, 52), Size = new Size(520, 380), ColumnCount = 2, RowCount = 4, BackColor = Color.Transparent };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int i = 0; i < 4; i++) tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));

            var txtAd     = AlanEkle(tbl, "Ad",              0, 0);
            var txtSoyad  = AlanEkle(tbl, "Soyad",           1, 0);
            var txtTC     = AlanEkle(tbl, "TC Kimlik No",     0, 1); txtTC.MaxLength = 11;
            var txtEmail  = AlanEkle(tbl, "E-Posta",          1, 1);
            var txtKadi   = AlanEkle(tbl, "Kullanıcı Adı",   0, 2);
            var txtSifre  = AlanEkle(tbl, "Şifre",           1, 2); txtSifre.UseSystemPasswordChar = true;
            var txtSifre2 = AlanEkle(tbl, "Şifre Tekrar",    0, 3); txtSifre2.UseSystemPasswordChar = true;

            kart.Controls.Add(tbl);

            var btnKayit = new Button { Text = "  Kayıt Ol", Location = new Point(20, 448), Width = 250, Height = 46 };
            UiTheme.AnaEylemButonu(btnKayit);

            var btnGeri = new Button { Text = "← Geri", Location = new Point(280, 448), Width = 260, Height = 46 };
            btnGeri.FlatStyle = FlatStyle.Flat;
            btnGeri.BackColor = Color.FromArgb(20, 255,255,255);
            btnGeri.ForeColor = Color.FromArgb(170, 205, 240);
            btnGeri.FlatAppearance.BorderColor = Color.FromArgb(50, 255,255,255);
            btnGeri.FlatAppearance.BorderSize = 1;
            btnGeri.Font = UiTheme.UiFontBold;
            btnGeri.Cursor = Cursors.Hand;
            btnGeri.Click += (s, e) => { new Views.İlkGiris().Show(); this.Hide(); };

            btnKayit.Click += (s, e) => {
                if (txtTC.Text.Length != 11) { MessageBox.Show("TC Kimlik 11 haneli olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { MessageBox.Show("Geçerli bir e-posta girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (txtSifre.Text != txtSifre2.Text) { MessageBox.Show("Şifreler eşleşmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (BelediyeDbServisi.KullaniciEmailZatenKayitli(txtEmail.Text)) { MessageBox.Show("Bu e-posta zaten kayıtlı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                var k = new Kullanici { Ad = txtAd.Text, Soyad = txtSoyad.Text, Tc = txtTC.Text, Email = txtEmail.Text, KullaniciAdi = txtKadi.Text, Sifre = txtSifre.Text };
                if (new KullaniciController().KullaniciEkle(k)) { MessageBox.Show("Kayıt başarılı! Giriş yapabilirsiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); this.Close(); }
                else MessageBox.Show("Kayıt sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            kart.Controls.AddRange(new Control[] { btnKayit, btnGeri });
            pnlSag.Controls.Add(kart);
            this.Controls.Add(pnlSag);
            this.Controls.Add(pnlSol);
        }

        private TextBox AlanEkle(TableLayoutPanel tbl, string etiket, int col, int row)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(6, 4, 6, 4) };
            pnl.Controls.Add(new Label { Text = etiket, Font = UiTheme.SmallBold, ForeColor = Color.FromArgb(165, 200, 240), AutoSize = true, Location = new Point(0, 2) });
            var txt = new TextBox { Location = new Point(0, 22), Width = pnl.Width - 12, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top, Height = 36, Font = UiTheme.UiFont, BackColor = Color.FromArgb(18, 35, 75), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            pnl.Controls.Add(txt);
            tbl.Controls.Add(pnl, col, row);
            return txt;
        }

        private void FillRounded(Graphics g, Brush b, Rectangle r, int rad) {
            int d = rad*2; using (var p = new GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.FillPath(b,p); }
        }
        private void DrawRounded(Graphics g, Pen pen, Rectangle r, int rad) {
            int d = rad*2; using (var p = new GraphicsPath()) { p.AddArc(r.X,r.Y,d,d,180,90); p.AddArc(r.Right-d,r.Y,d,d,270,90); p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90); p.AddArc(r.X,r.Bottom-d,d,d,90,90); p.CloseFigure(); g.DrawPath(pen,p); }
        }
        // Stub'lar
        private void Ad_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Soyad_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Tc_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Email_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void KullaniciAdi_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void Sifre_txt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        
        private void btnGeri_Click(object sender, EventArgs e) { }
        private void kayitOl_btn_Click(object sender, EventArgs e) { }
        private void Kayit_Screen_Load(object sender, EventArgs e) { }
    }
}