using System.Drawing;
using System.Windows.Forms;

namespace Belediye_Otomasyonu
{
    public static class UiTheme
    {
        public static readonly Color Primary = Color.FromArgb(0, 94, 115);
        public static readonly Color PrimaryDark = Color.FromArgb(0, 68, 88);
        public static readonly Color Surface = Color.FromArgb(245, 246, 248);
        public static readonly Color CardBackground = Color.White;
        public static readonly Color TextPrimary = Color.FromArgb(33, 37, 41);
        public static readonly Color TextMuted = Color.FromArgb(108, 117, 125);
        public static readonly Color TextOnPrimary = Color.White;
        public static readonly Color BorderSubtle = Color.FromArgb(222, 226, 230);

        public const int AnaButonYukseklik = 36;
        public const int IkincilButonYukseklik = 30;
        public const int IkincilButonGenislik = 100;

        public static Font UiFont { get; } = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static Font UiFontBold { get; } = new Font("Segoe UI", 10f, FontStyle.Bold);
        public static Font TitleFont { get; } = new Font("Segoe UI", 16f, FontStyle.Bold);
        public static Font StatValueFont { get; } = new Font("Segoe UI", 22f, FontStyle.Bold);

        public static void AnaEylemButonu(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Primary;
            b.ForeColor = TextOnPrimary;
            b.Font = UiFontBold;
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(100, AnaButonYukseklik);
            if (b.Height < AnaButonYukseklik)
                b.Height = AnaButonYukseklik;
        }

        public static void IkincilButon(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = PrimaryDark;
            b.ForeColor = TextOnPrimary;
            b.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            b.UseVisualStyleBackColor = false;
            b.MinimumSize = new Size(IkincilButonGenislik, IkincilButonYukseklik);
            if (b.Width < IkincilButonGenislik)
                b.Width = IkincilButonGenislik;
            if (b.Height < IkincilButonYukseklik)
                b.Height = IkincilButonYukseklik;
        }
    }
}
