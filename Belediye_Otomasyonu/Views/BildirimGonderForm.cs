using System;
using System.Drawing;
using System.Windows.Forms;
using Belediye_Otomasyonu.Services;

namespace Belediye_Otomasyonu.Views
{
    public class BildirimGonderForm : Form
    {
        private readonly string _oturumKadi;
        private ComboBox cmbAlici;
        private TextBox txtBaslik;
        private RichTextBox rtbIcerik;
        private Label lblCharCount;
        private Button btnGonder;

        public BildirimGonderForm(string oturumKadi)
        {
            _oturumKadi = oturumKadi ?? "";
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "Bildirim Gönder — Belediye Yönetim Sistemi";
            this.Size = new Size(580, 520);
            this.MinimumSize = new Size(500, 460);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = UiTheme.Surface;
            this.Font = UiTheme.UiFont;

            // ── 1. Footer (Bottom) — ÖNCE ekle ──────────────────────────────
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.White };
            var sepFooter = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UiTheme.BorderSubtle };
            btnGonder = new Button { Text = "  📤  Gönder", Dock = DockStyle.Right, Width = 150 };
            UiTheme.AccentButon(btnGonder);
            btnGonder.Click += BtnGonder_Click;
            var btnIptal = new Button { Text = "İptal", Dock = DockStyle.Right, Width = 100 };
            UiTheme.IkincilButon(btnIptal);
            btnIptal.Click += (s, e) => this.Close();
            pnlFooter.Controls.AddRange(new Control[] { btnGonder, btnIptal, sepFooter });
            this.Controls.Add(pnlFooter); // 1. Bottom

            // ── 2. Body — TableLayoutPanel (Fill) ────────────────────────────
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7,
                Padding = new Padding(24, 16, 24, 8),
                BackColor = UiTheme.Surface
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));  // lblAlici
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));  // cmbAlici
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 16f));  // boşluk
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));  // lblBaslik
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));  // txtBaslik
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));  // lblIcerik
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));  // rtbIcerik + charcount

            // Alıcı
            var lblAlici = new Label { Text = "Alıcı", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            cmbAlici = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = UiTheme.UiFont, BackColor = Color.White, FlatStyle = FlatStyle.Flat };
            cmbAlici.Items.Add("— Tüm Personeller —");
            try
            {
                var dt = BelediyeDbServisi.PersonelListesiAdiyla();
                foreach (System.Data.DataRow row in dt.Rows)
                    cmbAlici.Items.Add(new PersonelItem(row["KullaniciAdi"].ToString(), row["AdSoyad"].ToString()));
            }
            catch { }
            cmbAlici.SelectedIndex = 0;

            // Başlık
            var lblBaslik = new Label { Text = "Bildirim Başlığı", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            txtBaslik = new TextBox { Dock = DockStyle.Fill, Font = UiTheme.UiFont, BorderStyle = BorderStyle.FixedSingle, MaxLength = 200 };

            // İçerik
            var lblIcerik = new Label { Text = "Bildirim İçeriği", Font = UiTheme.UiFontBold, ForeColor = UiTheme.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };

            // İçerik + karakter sayacı için iç panel
            var pnlIcerik = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Surface };
            rtbIcerik = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = UiTheme.UiFont,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };
            lblCharCount = new Label
            {
                Text = "0 karakter",
                Font = UiTheme.SmallFont,
                ForeColor = UiTheme.TextMuted,
                Dock = DockStyle.Bottom,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft
            };
            rtbIcerik.TextChanged += (s, e) => lblCharCount.Text = rtbIcerik.Text.Length + " karakter";
            pnlIcerik.Controls.Add(rtbIcerik);     // Fill önce
            pnlIcerik.Controls.Add(lblCharCount);  // Bottom sonra

            // TableLayoutPanel'e ekle
            tbl.Controls.Add(lblAlici,  0, 0);
            tbl.Controls.Add(cmbAlici,  0, 1);
            tbl.Controls.Add(new Label { Dock = DockStyle.Fill }, 0, 2); // boşluk satırı
            tbl.Controls.Add(lblBaslik, 0, 3);
            tbl.Controls.Add(txtBaslik, 0, 4);
            tbl.Controls.Add(lblIcerik, 0, 5);
            tbl.Controls.Add(pnlIcerik, 0, 6);

            this.Controls.Add(tbl); // 2. Fill

            // ── 3. Header (Top) — SON ekle ──────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = UiTheme.HeaderBg };
            var sepHeader = new Panel { Dock = DockStyle.Bottom, Height = 3, BackColor = UiTheme.Accent };
            var lblHdr = new Label { Text = "📢  Bildirim Gönder", Font = new Font("Segoe UI", 14f, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(24, 14) };
            var lblSub = new Label { Text = "Tüm personellere veya belirli bir personele bildirim gönderin.", Font = UiTheme.SmallFont, ForeColor = Color.FromArgb(180, 200, 230), AutoSize = true, Location = new Point(25, 42) };
            pnlHeader.Controls.AddRange(new Control[] { lblHdr, lblSub, sepHeader });
            this.Controls.Add(pnlHeader); // 3. Top
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {
            string baslik = txtBaslik.Text.Trim();
            string icerik = rtbIcerik.Text.Trim();

            if (string.IsNullOrEmpty(baslik))
            {
                MessageBox.Show("Bildirim başlığı boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBaslik.Focus(); return;
            }
            if (string.IsNullOrEmpty(icerik))
            {
                MessageBox.Show("Bildirim içeriği boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbIcerik.Focus(); return;
            }

            string alici = null;
            if (cmbAlici.SelectedIndex > 0 && cmbAlici.SelectedItem is PersonelItem pi)
                alici = pi.KullaniciAdi;

            string hata = BelediyeDbServisi.BildirimGonder(_oturumKadi, alici, baslik, icerik);
            if (hata != null) { MessageBox.Show("Gönderim hatası: " + hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            string hedef = alici == null ? "tüm personellere" : $"'{alici}' adlı personele";
            MessageBox.Show($"Bildirim {hedef} başarıyla gönderildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private class PersonelItem
        {
            public string KullaniciAdi { get; }
            public string AdSoyad { get; }
            public PersonelItem(string k, string a) { KullaniciAdi = k; AdSoyad = a; }
            public override string ToString() => $"{AdSoyad} ({KullaniciAdi})";
        }
    }
}
