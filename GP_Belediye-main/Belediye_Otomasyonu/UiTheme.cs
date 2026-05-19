using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Belediye_Otomasyonu
{
    public static class UiTheme
    {
        // ── Kurumsal Renk Paleti — Premium ──────────────────────────────────────
        public static readonly Color Primary      = Color.FromArgb(20, 42, 100);   // Derin Modern Lacivert
        public static readonly Color PrimaryDark  = Color.FromArgb(10, 20, 56);     // Gece Mavisi
        public static readonly Color PrimaryLight = Color.FromArgb(38, 75, 172);   // Açık Lacivert
        public static readonly Color Accent       = Color.FromArgb(235, 185, 45);  // Canlı Altın
        public static readonly Color AccentDark   = Color.FromArgb(185, 140, 20);  // Koyu Altın
        public static readonly Color AccentLight  = Color.FromArgb(255, 220, 105); // Parlak Altın
        public static readonly Color Secondary    = Color.FromArgb(30, 85, 200);   // Parlak Mavi

        public static readonly Color GradientTop    = Color.FromArgb(8, 15, 45);
        public static readonly Color GradientBottom = Color.FromArgb(20, 42, 98);

        public static readonly Color SidebarBg       = Color.FromArgb(10, 18, 52);
        public static readonly Color SidebarSelected = Color.FromArgb(25, 50, 120);
        public static readonly Color SidebarHover    = Color.FromArgb(16, 32, 80);
        public static readonly Color SidebarText     = Color.FromArgb(170, 195, 235);
        public static readonly Color SidebarTextSel  = Color.FromArgb(235, 185, 45);
        public static readonly Color SidebarAccent   = Color.FromArgb(235, 185, 45);

        public static readonly Color Surface        = Color.FromArgb(242, 245, 252);
        public static readonly Color SurfaceDark    = Color.FromArgb(226, 232, 245);
        public static readonly Color CardBackground = Color.White;
        public static readonly Color HeaderBg       = Color.FromArgb(10, 22, 64);

        public static readonly Color TextPrimary   = Color.FromArgb(16, 26, 68);
        public static readonly Color TextMuted     = Color.FromArgb(94, 114, 150);
        public static readonly Color TextOnPrimary = Color.White;
        public static readonly Color TextOnDark    = Color.FromArgb(205, 220, 245);
        public static readonly Color BorderSubtle  = Color.FromArgb(210, 222, 242);
        public static readonly Color BorderCard    = Color.FromArgb(218, 230, 248);
        public static readonly Color BorderAccent  = Color.FromArgb(235, 185, 45);

        public static readonly Color Success  = Color.FromArgb(16, 172, 86);
        public static readonly Color Warning  = Color.FromArgb(240, 140, 0);
        public static readonly Color Danger   = Color.FromArgb(226, 45, 45);
        public static readonly Color Info     = Color.FromArgb(0, 135, 235);

        public static readonly Color RowAlt   = Color.FromArgb(248, 250, 255);

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

        // ── Custom Rounded Button Drawing ────────────────────────────────────────
        public static void YuvarlakButon(Button b, Color arkaPlan, Color yaziRengi, int radius = 6)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(120, AnaButonYukseklik);
            if (b.Height < AnaButonYukseklik) b.Height = AnaButonYukseklik;

            b.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color parentColor = Color.White;
                if (b.Parent != null)
                {
                    if (b.Parent.BackColor == Color.Transparent && b.Parent.Parent != null)
                        parentColor = b.Parent.Parent.BackColor;
                    else
                        parentColor = b.Parent.BackColor;
                }
                
                using (var brushBg = new SolidBrush(parentColor))
                    e.Graphics.FillRectangle(brushBg, b.ClientRectangle);

                bool isHover = b.ClientRectangle.Contains(b.PointToClient(Cursor.Position));
                Color drawColor = b.Enabled ? (isHover ? ControlPaint.Light(arkaPlan, 0.15f) : arkaPlan) : Color.FromArgb(200, 200, 200);

                using (var path = new GraphicsPath())
                {
                    int d = radius * 2;
                    var r = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
                    path.AddArc(r.X, r.Y, d, d, 180, 90);
                    path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                    path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                    path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();

                    using (var br = new SolidBrush(drawColor))
                        e.Graphics.FillPath(br, path);
                }

                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, b.ClientRectangle, yaziRengi, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            };

            b.MouseEnter += (s, e) => b.Invalidate();
            b.MouseLeave += (s, e) => b.Invalidate();
        }

        public static void YuvarlakButonBorder(Button b, Color arkaPlan, Color yaziRengi, Color borderColor, int radius = 6)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(IkincilButonGenislik, IkincilButonYukseklik);
            if (b.Width < IkincilButonGenislik) b.Width = IkincilButonGenislik;
            if (b.Height < IkincilButonYukseklik) b.Height = IkincilButonYukseklik;

            b.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color parentColor = Color.White;
                if (b.Parent != null)
                {
                    if (b.Parent.BackColor == Color.Transparent && b.Parent.Parent != null)
                        parentColor = b.Parent.Parent.BackColor;
                    else
                        parentColor = b.Parent.BackColor;
                }
                
                using (var brushBg = new SolidBrush(parentColor))
                    e.Graphics.FillRectangle(brushBg, b.ClientRectangle);

                bool isHover = b.ClientRectangle.Contains(b.PointToClient(Cursor.Position));
                Color drawColor = b.Enabled ? (isHover ? Color.FromArgb(245, 248, 255) : arkaPlan) : Color.FromArgb(230, 230, 230);

                using (var path = new GraphicsPath())
                {
                    int d = radius * 2;
                    var r = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
                    path.AddArc(r.X, r.Y, d, d, 180, 90);
                    path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                    path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                    path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();

                    using (var br = new SolidBrush(drawColor))
                        e.Graphics.FillPath(br, path);

                    using (var pen = new Pen(borderColor, 1))
                        e.Graphics.DrawPath(pen, path);
                }

                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, b.ClientRectangle, yaziRengi, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            };

            b.MouseEnter += (s, e) => b.Invalidate();
            b.MouseLeave += (s, e) => b.Invalidate();
        }

        // ── Ana Eylem Butonları ──────────────────────────────────────────────────
        public static void AnaEylemButonu(Button b)
        {
            b.Font = UiFontBold;
            YuvarlakButon(b, Primary, TextOnPrimary, 6);
        }

        public static void IkincilButon(Button b)
        {
            b.Font = UiFontBold;
            YuvarlakButonBorder(b, Color.White, TextPrimary, BorderSubtle, 6);
        }

        public static void AccentButon(Button b)
        {
            b.Font = UiFontBold;
            YuvarlakButon(b, Accent, PrimaryDark, 6);
        }

        public static void TehlikeButon(Button b)
        {
            b.Font = UiFontBold;
            YuvarlakButon(b, Danger, Color.White, 6);
        }

        public static void BasariButon(Button b)
        {
            b.Font = UiFontBold;
            YuvarlakButon(b, Success, Color.White, 6);
        }

        public static void HeroButon(Button b, Color bgColor, string emoji = "")
        {
            b.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            YuvarlakButon(b, bgColor, Color.White, 8);
        }

        // ── Textbox & Combobox Styling Helpers ────────────────────────────────────
        public static void TextBoxKaranlikStil(TextBox txt)
        {
            txt.BackColor = Color.FromArgb(18, 35, 75);
            txt.ForeColor = Color.White;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = UiFont;
            
            txt.GotFocus += (s, e) => {
                txt.BackColor = Color.FromArgb(24, 46, 96);
            };
            txt.LostFocus += (s, e) => {
                txt.BackColor = Color.FromArgb(18, 35, 75);
            };
        }

        public static void TextBoxAydinlikStil(TextBox txt)
        {
            txt.BackColor = Color.White;
            txt.ForeColor = TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = UiFont;
            
            txt.GotFocus += (s, e) => {
                txt.BackColor = Color.FromArgb(250, 252, 255);
            };
            txt.LostFocus += (s, e) => {
                txt.BackColor = Color.White;
            };
        }

        public static void ComboBoxStil(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Font = UiFont;
            cmb.BackColor = Color.White;
            cmb.ForeColor = TextPrimary;
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

        // ── Sidebar User Panel ──────────────────────────────────────────────────
        public static void SidebarUserPaneli(Panel p, string kullanici, string rol)
        {
            p.BackColor = Color.Transparent;
            p.Controls.Clear();

            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Draw a beautiful separator line
                using (var pen = new Pen(Color.FromArgb(30, 255, 255, 255), 1))
                {
                    e.Graphics.DrawLine(pen, 16, p.Height - 1, p.Width - 16, p.Height - 1);
                }
            };

            // Avatar placeholder circle
            var pnlAvatar = new Panel {
                Size = new Size(38, 38),
                Location = new Point(18, 16),
                BackColor = Color.Transparent
            };
            pnlAvatar.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(30, Accent)))
                    e.Graphics.FillEllipse(br, 0, 0, 37, 37);
                using (var pen = new Pen(Accent, 1))
                    e.Graphics.DrawEllipse(pen, 0, 0, 37, 37);
                
                string init = "👤";
                if (!string.IsNullOrEmpty(kullanici) && kullanici.Length > 0)
                {
                    init = kullanici.Substring(0, 1).ToUpper();
                }
                TextRenderer.DrawText(e.Graphics, init, new Font("Segoe UI", 11f, FontStyle.Bold), new Rectangle(0,0,38,38), Accent, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            p.Controls.Add(pnlAvatar);

            var lblName = new Label {
                Text = kullanici,
                Font = UiFontBold,
                ForeColor = Color.White,
                Location = new Point(66, 14),
                AutoSize = true
            };
            var lblRole = new Label {
                Text = rol,
                Font = SmallFont,
                ForeColor = Color.FromArgb(160, 185, 220),
                Location = new Point(66, 33),
                AutoSize = true
            };
            p.Controls.Add(lblName);
            p.Controls.Add(lblRole);
        }

        // ── İstatistik Kartı ─────────────────────────────────────────────────────
        public static Panel OlusturStatKart(string baslik, string deger, Color renk, string ikon = "")
        {
            var pnl = new Panel
            {
                BackColor = Color.Transparent,
                Margin = new Padding(8),
                Dock = DockStyle.Fill,
                Height = 100
            };

            pnl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                Color parentColor = Surface;
                if (pnl.Parent != null) parentColor = pnl.Parent.BackColor;
                using (var brushBg = new SolidBrush(parentColor))
                    e.Graphics.FillRectangle(brushBg, pnl.ClientRectangle);

                var r = new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1);
                int rad = 8;
                int d = rad * 2;
                
                using (var path = new GraphicsPath())
                {
                    path.AddArc(r.X, r.Y, d, d, 180, 90);
                    path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                    path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                    path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();

                    using (var br = new SolidBrush(Color.White))
                        e.Graphics.FillPath(br, path);

                    using (var br2 = new SolidBrush(Color.FromArgb(12, renk)))
                        e.Graphics.FillPath(br2, path);

                    using (var pen = new Pen(Color.FromArgb(40, renk), 1))
                        e.Graphics.DrawPath(pen, path);
                }

                // Left accent bar
                using (var pathBar = new GraphicsPath())
                {
                    int radBar = 4;
                    int dBar = radBar * 2;
                    var rBar = new Rectangle(0, 0, 6, pnl.Height - 1);
                    pathBar.AddArc(rBar.X, rBar.Y, dBar, dBar, 180, 90);
                    pathBar.AddLine(6, 0, 6, pnl.Height - 1);
                    pathBar.AddArc(rBar.X, rBar.Bottom - dBar, dBar, dBar, 90, 90);
                    pathBar.CloseFigure();
                    using (var brBar = new SolidBrush(renk))
                        e.Graphics.FillPath(brBar, pathBar);
                }
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
                    Location = new Point(-1, -1)
                };
                lblIkon.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                pnl.Controls.Add(lblIkon);
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
                Text = "  " + (durum ?? "Beklemede").ToUpper() + "  ",
                Font = SmallBold,
                ForeColor = renk,
                BackColor = Color.Transparent,
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lbl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int rad = 4;
                int d = rad * 2;
                var r = new Rectangle(0, 0, lbl.Width - 1, lbl.Height - 1);
                using (var path = new GraphicsPath())
                {
                    path.AddArc(r.X, r.Y, d, d, 180, 90);
                    path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                    path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                    path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();
                    using (var br = new SolidBrush(Color.FromArgb(32, renk)))
                        e.Graphics.FillPath(br, path);
                    using (var pen = new Pen(Color.FromArgb(80, renk), 1))
                        e.Graphics.DrawPath(pen, path);
                }
                TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font, lbl.ClientRectangle, renk, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            return lbl;
        }

        // ── Arama Kutusu ─────────────────────────────────────────────────────────
        public static Panel OlusturAramaKutusu(out TextBox txtArama, string placeholder = "🔍  Ara...")
        {
            var pnl = new Panel { Height = 40, BackColor = Color.Transparent, Padding = new Padding(12, 10, 12, 10) };
            var txt = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = UiFont,
                ForeColor = TextMuted,
                BackColor = Color.White,
                Text = placeholder
            };
            
            pnl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                Color parentColor = Surface;
                if (pnl.Parent != null) parentColor = pnl.Parent.BackColor;
                using (var brushBg = new SolidBrush(parentColor))
                    e.Graphics.FillRectangle(brushBg, pnl.ClientRectangle);

                var r = new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1);
                int rad = 6;
                int d = rad * 2;
                using (var path = new GraphicsPath())
                {
                    path.AddArc(r.X, r.Y, d, d, 180, 90);
                    path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                    path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                    path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();
                    using (var br = new SolidBrush(Color.White))
                        e.Graphics.FillPath(br, path);
                    using (var pen = new Pen(BorderSubtle, 1))
                        e.Graphics.DrawPath(pen, path);
                }
            };
            
            txt.GotFocus  += (s, e) => { if (txt.Text == placeholder) { txt.Text = ""; txt.ForeColor = TextPrimary; } };
            txt.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txt.Text)) { txt.Text = placeholder; txt.ForeColor = TextMuted; } };
            
            txt.Location = new Point(12, 11);
            txt.Width = pnl.Width - 24;
            pnl.Layout += (s, e) => {
                txt.Location = new Point(12, 11);
                txt.Width = pnl.Width - 24;
            };

            pnl.Controls.Add(txt);
            txtArama = txt;
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
