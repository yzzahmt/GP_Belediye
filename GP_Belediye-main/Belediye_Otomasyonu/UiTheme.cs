using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Belediye_Otomasyonu
{
    public static class UiTheme
    {
        // ── Kurumsal Renk Paleti — Premium ──────────────────────────────────────
        public static readonly Color Primary      = Color.FromArgb(15, 35, 90);    // Derin Lacivert
        public static readonly Color PrimaryDark  = Color.FromArgb(8, 16, 50);     // Gece Mavisi
        public static readonly Color PrimaryLight = Color.FromArgb(32, 65, 150);   // Açık Lacivert
        public static readonly Color Accent       = Color.FromArgb(212, 175, 55);  // Altın
        public static readonly Color AccentDark   = Color.FromArgb(165, 130, 25);  // Koyu Altın
        public static readonly Color AccentLight  = Color.FromArgb(255, 215, 100); // Parlak Altın
        public static readonly Color Secondary    = Color.FromArgb(25, 75, 175);   // Parlak Mavi

        public static readonly Color GradientTop    = Color.FromArgb(6, 12, 40);
        public static readonly Color GradientBottom = Color.FromArgb(15, 35, 85);

        public static readonly Color SidebarBg       = Color.FromArgb(7, 13, 42);
        public static readonly Color SidebarSelected = Color.FromArgb(18, 40, 95);
        public static readonly Color SidebarHover    = Color.FromArgb(12, 26, 68);
        public static readonly Color SidebarText     = Color.FromArgb(155, 178, 215);
        public static readonly Color SidebarTextSel  = Color.FromArgb(212, 175, 55);
        public static readonly Color SidebarAccent   = Color.FromArgb(212, 175, 55);

        public static readonly Color Surface        = Color.FromArgb(232, 238, 250);
        public static readonly Color SurfaceDark    = Color.FromArgb(218, 226, 242);
        public static readonly Color CardBackground = Color.White;
        public static readonly Color HeaderBg       = Color.FromArgb(8, 18, 55);

        public static readonly Color TextPrimary   = Color.FromArgb(12, 22, 58);
        public static readonly Color TextMuted     = Color.FromArgb(88, 108, 142);
        public static readonly Color TextOnPrimary = Color.White;
        public static readonly Color TextOnDark    = Color.FromArgb(195, 212, 240);
        public static readonly Color BorderSubtle  = Color.FromArgb(195, 210, 228);
        public static readonly Color BorderCard    = Color.FromArgb(208, 222, 240);
        public static readonly Color BorderAccent  = Color.FromArgb(212, 175, 55);

        public static readonly Color Success  = Color.FromArgb(14, 142, 66);
        public static readonly Color Warning  = Color.FromArgb(205, 105, 0);
        public static readonly Color Danger   = Color.FromArgb(180, 25, 25);
        public static readonly Color Info     = Color.FromArgb(2, 115, 195);

        public static readonly Color RowAlt   = Color.FromArgb(242, 247, 255);

        // ── Dimensions ──────────────────────────────────────────────────────────
        public const int AnaButonYukseklik     = 44;
        public const int IkincilButonYukseklik = 40;
        public const int IkincilButonGenislik  = 120;

        // ── Fonts ────────────────────────────────────────────────────────────────
        public static Font UiFont       { get; } = new Font("Segoe UI", 10f);
        public static Font UiFontBold   { get; } = new Font("Segoe UI", 10f, FontStyle.Bold);
        public static Font TitleFont    { get; } = new Font("Segoe UI", 18f, FontStyle.Bold);
        public static Font SubtitleFont { get; } = new Font("Segoe UI", 12f);
        public static Font SmallFont    { get; } = new Font("Segoe UI", 8.5f);
        public static Font SmallBold    { get; } = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        public static Font StatValueFont{ get; } = new Font("Segoe UI", 26f, FontStyle.Bold);
        public static Font SidebarFont  { get; } = new Font("Segoe UI", 10.5f);
        public static Font HeroFont     { get; } = new Font("Segoe UI", 28f, FontStyle.Bold);
        public static Font LargeFont    { get; } = new Font("Segoe UI", 14f, FontStyle.Bold);

        // ── Form Dizayn ─────────────────────────────────────────────────────────
        public static void FormDizayn(Form f)
        {
            f.BackColor = Surface;
            f.Font = UiFont;
            f.ForeColor = TextPrimary;
        }

        public static void GradientFormDizayn(Form f, Color? ust = null, Color? alt = null)
        {
            f.BackColor = ust ?? GradientTop;
            f.Font = UiFont;
            f.ForeColor = TextOnPrimary;
            f.Paint += (s, e) =>
            {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(0, f.Height),
                    ust ?? GradientTop, alt ?? GradientBottom))
                    e.Graphics.FillRectangle(br, f.ClientRectangle);
            };
        }

        // ── Sidebar ─────────────────────────────────────────────────────────────
        public static Panel OlusturGradientSidebar(int width = 250)
        {
            var sidebar = new Panel { Dock = DockStyle.Left, Width = width, BackColor = SidebarBg };
            sidebar.Paint += (s, e) =>
            {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(0, sidebar.Height),
                    SidebarBg, Color.FromArgb(10, 22, 60)))
                    e.Graphics.FillRectangle(br, sidebar.ClientRectangle);
            };
            return sidebar;
        }

        // ── Sidebar Buton ────────────────────────────────────────────────────────
        public static void SidebarButon(Button b, bool secili = false)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = SidebarHover;
            b.BackColor = secili ? SidebarSelected : SidebarBg;
            b.ForeColor = secili ? SidebarAccent : SidebarText;
            b.Font = SidebarFont;
            b.Cursor = Cursors.Hand;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(22, 0, 0, 0);
            b.UseVisualStyleBackColor = false;

            // Sol aksanlı şerit — Paint ile
            b.Paint -= SidebarBtnPaint; // önceki handler'ı temizle
            b.Tag = secili;
            b.Paint += SidebarBtnPaint;
        }

        private static void SidebarBtnPaint(object sender, PaintEventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;
            bool secili = b.Tag is bool s && s;
            if (secili)
            {
                using (var br = new SolidBrush(SidebarAccent))
                    e.Graphics.FillRectangle(br, 0, 0, 4, b.Height);
            }
        }

        // ── Ana Eylem Butonu ─────────────────────────────────────────────────────
        public static void AnaEylemButonu(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = PrimaryLight;
            b.BackColor = Primary;
            b.ForeColor = TextOnPrimary;
            b.Font = UiFontBold;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(120, AnaButonYukseklik);
            if (b.Height < AnaButonYukseklik) b.Height = AnaButonYukseklik;
        }

        public static void IkincilButon(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = BorderSubtle;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 240, 250);
            b.BackColor = Color.White;
            b.ForeColor = TextPrimary;
            b.Font = UiFontBold;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(IkincilButonGenislik, IkincilButonYukseklik);
            if (b.Width < IkincilButonGenislik) b.Width = IkincilButonGenislik;
            if (b.Height < IkincilButonYukseklik) b.Height = IkincilButonYukseklik;
        }

        public static void AccentButon(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = AccentLight;
            b.BackColor = Accent;
            b.ForeColor = PrimaryDark;
            b.Font = UiFontBold;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(120, AnaButonYukseklik);
            if (b.Height < AnaButonYukseklik) b.Height = AnaButonYukseklik;
        }

        public static void TehlikeButon(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 40, 40);
            b.BackColor = Danger;
            b.ForeColor = Color.White;
            b.Font = UiFontBold;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(120, AnaButonYukseklik);
            if (b.Height < AnaButonYukseklik) b.Height = AnaButonYukseklik;
        }

        public static void BasariButon(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(18, 168, 80);
            b.BackColor = Success;
            b.ForeColor = Color.White;
            b.Font = UiFontBold;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(120, AnaButonYukseklik);
            if (b.Height < AnaButonYukseklik) b.Height = AnaButonYukseklik;
        }

        public static void HeroButon(Button b, Color bgColor, string emoji = "")
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(bgColor, 0.15f);
            b.BackColor = bgColor;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.TextAlign = ContentAlignment.MiddleCenter;
        }

        // ── DataGridView Stili ───────────────────────────────────────────────────
        public static void DataGridStil(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = BorderSubtle;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.Font = UiFont;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(195, 218, 255);
            dgv.DefaultCellStyle.SelectionForeColor = PrimaryDark;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            dgv.RowTemplate.Height = 40;
        }

        // ── Header Panel ─────────────────────────────────────────────────────────
        public static Panel OlusturHeaderPanel(string baslik, string altYazi = null)
        {
            int yukseklik = altYazi != null ? 96 : 68;
            var pnl = new Panel { Dock = DockStyle.Top, Height = yukseklik, BackColor = HeaderBg };

            pnl.Paint += (s, e) =>
            {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(pnl.Width, 0),
                    HeaderBg, Color.FromArgb(18, 48, 118)))
                    e.Graphics.FillRectangle(br, pnl.ClientRectangle);
            };

            var lblBaslik = new Label
            {
                Text = baslik,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(26, 14)
            };
            pnl.Controls.Add(lblBaslik);

            if (altYazi != null)
            {
                var lblAlt = new Label
                {
                    Text = altYazi,
                    Font = SmallFont,
                    ForeColor = Color.FromArgb(165, 190, 230),
                    AutoSize = true,
                    Location = new Point(27, 44)
                };
                pnl.Controls.Add(lblAlt);
            }

            var sep = new Panel { Dock = DockStyle.Bottom, Height = 3 };
            sep.Paint += (ss, ee) =>
            {
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(sep.Width, 0),
                    AccentLight, Accent))
                    ee.Graphics.FillRectangle(br, sep.ClientRectangle);
            };
            pnl.Controls.Add(sep);

            return pnl;
        }

        // ── İstatistik Kartı ─────────────────────────────────────────────────────
        public static Panel OlusturStatKart(string baslik, string deger, Color renk, string ikon = "")
        {
            var pnl = new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(6),
                Dock = DockStyle.Fill
            };

            pnl.Paint += (s, e) =>
            {
                // Sol şerit
                using (var br = new LinearGradientBrush(
                    new Point(0, 0), new Point(0, pnl.Height),
                    renk, ControlPaint.Light(renk, 0.3f)))
                    e.Graphics.FillRectangle(br, 0, 0, 5, pnl.Height);

                // Çok hafif arka plan tonu
                using (var br2 = new SolidBrush(Color.FromArgb(10, renk)))
                    e.Graphics.FillRectangle(br2, 5, 0, pnl.Width - 5, pnl.Height);

                // Alt çizgi
                using (var pen = new Pen(Color.FromArgb(18, renk), 1))
                    e.Graphics.DrawLine(pen, 5, pnl.Height - 1, pnl.Width, pnl.Height - 1);
            };

            if (!string.IsNullOrEmpty(ikon))
            {
                var lblIkon = new Label
                {
                    Text = ikon,
                    Font = new Font("Segoe UI", 26f),
                    ForeColor = Color.FromArgb(45, renk),
                    AutoSize = false,
                    Size = new Size(56, 56),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(-1, -1) // sağda yer tutacak şekilde anchor ile yapılacak
                };
                lblIkon.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                pnl.Controls.Add(lblIkon);
                // sağa yasla
                pnl.Layout += (s, e2) => { if (lblIkon != null) lblIkon.Location = new Point(pnl.Width - 62, 8); };
            }

            pnl.Controls.Add(new Label
            {
                Text = baslik.ToUpperInvariant(),
                Font = SmallBold,
                ForeColor = TextMuted,
                AutoSize = true,
                Location = new Point(16, 14)
            });
            pnl.Controls.Add(new Label
            {
                Text = deger,
                Font = new Font("Segoe UI", 24f, FontStyle.Bold),
                ForeColor = renk,
                AutoSize = true,
                Location = new Point(16, 34)
            });

            return pnl;
        }

        // ── Kart Panel (eski uyumluluk) ───────────────────────────────────────────
        public static Panel OlusturKartPanel(string baslik, string deger, string renk = null)
        {
            Color kart = renkAl(renk);
            return OlusturStatKart(baslik, deger, kart);
        }

        // ── Durum Rengi ─────────────────────────────────────────────────────────
        public static Color DurumRengi(string durum)
        {
            if (durum == null) return TextMuted;
            switch (durum.ToLowerInvariant().Trim())
            {
                case "tamamlandi":
                case "tamamlandı": return Success;
                case "islemde":
                case "işlemde":    return Info;
                case "reddedildi": return Danger;
                default:           return Warning;
            }
        }

        public static string DurumEmoji(string durum)
        {
            if (durum == null) return "⏳";
            switch (durum.ToLowerInvariant().Trim())
            {
                case "tamamlandi":
                case "tamamlandı": return "✅";
                case "islemde":
                case "işlemde":    return "🔄";
                case "reddedildi": return "❌";
                default:           return "⏳";
            }
        }

        // ── Durum Etiketi ────────────────────────────────────────────────────────
        public static Label OlusturDurumEtiketi(string durum)
        {
            var renk = DurumRengi(durum);
            var lbl = new Label
            {
                Text = "  " + (durum ?? "Beklemede") + "  ",
                Font = SmallBold,
                ForeColor = Color.White,
                BackColor = renk,
                AutoSize = true,
                Padding = new Padding(4, 2, 4, 2)
            };
            return lbl;
        }

        // ── Arama Kutusu ─────────────────────────────────────────────────────────
        public static Panel OlusturAramaKutusu(out TextBox txtArama, string placeholder = "🔍  Ara...")
        {
            var pnl = new Panel { Height = 40, BackColor = Color.White };
            pnl.Paint += (s, e) =>
            {
                using (var pen = new Pen(BorderSubtle, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };
            txtArama = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = UiFont,
                ForeColor = TextMuted,
                BackColor = Color.White,
                Text = placeholder,
                Padding = new Padding(6, 0, 0, 0)
            };
            var t = txtArama;
            t.GotFocus  += (s, e) => { if (t.Text == placeholder) { t.Text = ""; t.ForeColor = TextPrimary; } };
            t.LostFocus += (s, e) => { if (string.IsNullOrEmpty(t.Text)) { t.Text = placeholder; t.ForeColor = TextMuted; } };
            pnl.Controls.Add(txtArama);
            return pnl;
        }

        // ── Yardımcılar ─────────────────────────────────────────────────────────
        private static Color renkAl(string renk)
        {
            switch (renk)
            {
                case "accent":  return Accent;
                case "success": return Success;
                case "warning": return Warning;
                case "danger":  return Danger;
                default:        return Primary;
            }
        }

        /// <summary>Filtre ComboBox değerinin gerçek bir filtre mi yoksa "Tümü" mü olduğunu kontrol eder.</summary>
        public static bool FiltrelerAktif(string deger)
        {
            if (string.IsNullOrWhiteSpace(deger)) return false;
            string d = deger.Trim().ToLowerInvariant();
            return d != "tümü" && d != "tumu" && d != "tüm" && d != "tum" && d != "hepsi";
        }
    }
}
