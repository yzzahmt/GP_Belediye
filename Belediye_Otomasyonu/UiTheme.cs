using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Belediye_Otomasyonu
{
    public static class UiTheme
    {
        // ── Kurumsal Renk Paleti ──────────────────────────────────────────────
        public static readonly Color Primary      = Color.FromArgb(26, 45, 90);   // Lacivert
        public static readonly Color PrimaryDark  = Color.FromArgb(15, 28, 60);   // Koyu Lacivert
        public static readonly Color PrimaryLight = Color.FromArgb(40, 70, 140);  // Açık Lacivert
        public static readonly Color Accent       = Color.FromArgb(201, 168, 76); // Altın Sarısı
        public static readonly Color AccentDark   = Color.FromArgb(160, 130, 50); // Koyu Altın
        public static readonly Color Secondary    = Color.FromArgb(45, 90, 160);  // Mavi

        public static readonly Color SidebarBg        = Color.FromArgb(15, 28, 60);
        public static readonly Color SidebarSelected  = Color.FromArgb(26, 45, 90);
        public static readonly Color SidebarText      = Color.FromArgb(185, 200, 220);
        public static readonly Color SidebarTextHover = Color.White;
        public static readonly Color SidebarAccent    = Color.FromArgb(201, 168, 76);

        public static readonly Color Surface        = Color.FromArgb(240, 243, 248);
        public static readonly Color CardBackground = Color.White;
        public static readonly Color HeaderBg       = Color.FromArgb(26, 45, 90);

        public static readonly Color TextPrimary  = Color.FromArgb(22, 33, 55);
        public static readonly Color TextMuted    = Color.FromArgb(100, 116, 139);
        public static readonly Color TextOnPrimary= Color.White;
        public static readonly Color BorderSubtle = Color.FromArgb(210, 218, 230);
        public static readonly Color BorderCard   = Color.FromArgb(220, 228, 238);

        public static readonly Color Success  = Color.FromArgb(22, 163, 74);
        public static readonly Color Warning  = Color.FromArgb(217, 119, 6);
        public static readonly Color Danger   = Color.FromArgb(185, 28, 28);
        public static readonly Color Info     = Color.FromArgb(2, 132, 199);

        public static readonly Color RowAlt   = Color.FromArgb(247, 250, 255);

        // ── Dimensions ───────────────────────────────────────────────────────
        public const int AnaButonYukseklik    = 42;
        public const int IkincilButonYukseklik= 38;
        public const int IkincilButonGenislik = 120;

        // ── Fonts ─────────────────────────────────────────────────────────────
        public static Font UiFont       { get; } = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static Font UiFontBold   { get; } = new Font("Segoe UI", 10f, FontStyle.Bold);
        public static Font TitleFont    { get; } = new Font("Segoe UI", 17f, FontStyle.Bold);
        public static Font SubtitleFont { get; } = new Font("Segoe UI", 12f, FontStyle.Regular);
        public static Font SmallFont    { get; } = new Font("Segoe UI", 8.5f, FontStyle.Regular);
        public static Font StatValueFont{ get; } = new Font("Segoe UI", 26f, FontStyle.Bold);
        public static Font SidebarFont  { get; } = new Font("Segoe UI", 10.5f, FontStyle.Regular);

        // ── Buton Stilleri ───────────────────────────────────────────────────
        public static void AnaEylemButonu(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
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
            b.BackColor = Danger;
            b.ForeColor = Color.White;
            b.Font = UiFontBold;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(120, AnaButonYukseklik);
            if (b.Height < AnaButonYukseklik) b.Height = AnaButonYukseklik;
        }

        public static void SidebarButon(Button b, bool secili = false)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = SidebarSelected;
            b.BackColor = secili ? SidebarSelected : SidebarBg;
            b.ForeColor = secili ? SidebarAccent : SidebarText;
            b.Font = SidebarFont;
            b.Cursor = Cursors.Hand;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(20, 0, 0, 0);
            b.UseVisualStyleBackColor = false;
        }

        // ── DataGridView Stili ───────────────────────────────────────────────
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
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.Font = UiFont;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 220, 245);
            dgv.DefaultCellStyle.SelectionForeColor = PrimaryDark;
            dgv.DefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            dgv.RowTemplate.Height = 34;
        }

        // ── Form Dizayn ──────────────────────────────────────────────────────
        public static void FormDizayn(Form f)
        {
            f.BackColor = Surface;
            f.Font = UiFont;
            f.ForeColor = TextPrimary;
        }

        // ── Header Panel Oluştur ─────────────────────────────────────────────
        public static Panel OlusturHeaderPanel(string baslik, string altYazi = null)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = altYazi != null ? 90 : 66,
                BackColor = HeaderBg,
                Padding = new Padding(24, 14, 24, 14)
            };

            var lblBaslik = new Label
            {
                Text = baslik,
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(24, 14)
            };
            pnl.Controls.Add(lblBaslik);

            if (altYazi != null)
            {
                var lblAlt = new Label
                {
                    Text = altYazi,
                    Font = SmallFont,
                    ForeColor = Color.FromArgb(180, 200, 230),
                    AutoSize = true,
                    Location = new Point(25, 42)
                };
                pnl.Controls.Add(lblAlt);
            }

            // Altın çizgi
            var sep = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 3,
                BackColor = Accent
            };
            pnl.Controls.Add(sep);

            return pnl;
        }

        // ── Kart Panel ───────────────────────────────────────────────────────
        public static Panel OlusturKartPanel(string baslik, string deger, string renk = null)
        {
            Color kart = renkAl(renk);
            var pnl = new Panel
            {
                BackColor = CardBackground,
                Margin = new Padding(6),
                Padding = new Padding(16)
            };

            var solSerit = new Panel
            {
                Dock = DockStyle.Left,
                Width = 4,
                BackColor = kart
            };

            var lblCap = new Label
            {
                Text = baslik,
                Font = SmallFont,
                ForeColor = TextMuted,
                AutoSize = true,
                Location = new Point(20, 14)
            };

            var lblDeger = new Label
            {
                Text = deger,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = kart,
                AutoSize = true,
                Location = new Point(20, 32)
            };

            pnl.Controls.Add(solSerit);
            pnl.Controls.Add(lblCap);
            pnl.Controls.Add(lblDeger);
            return pnl;
        }

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

        // ── Durum Badge ─────────────────────────────────────────────────────
        public static Color DurumRengi(string durum)
        {
            if (durum == null) return TextMuted;
            switch (durum.ToLowerInvariant())
            {
                case "tamamlandi":
                case "tamamlandı": return Success;
                case "islemde":
                case "işlemde":    return Info;
                case "reddedildi": return Danger;
                default:           return Warning;
            }
        }
    }
}
