using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using Belediye_Otomasyonu.Models;

namespace Belediye_Otomasyonu.Views
{
    /// <summary>
    /// Belediyedeki personelin gelen başvuruları inceleyip atama yapmasını,
    /// öncelik derecesi belirlemesini ve durumu güncellemesini sağlayan form.
    /// </summary>
    public class PersonelForm : Form
    {
        private DataGridView dgvBasvurular;
        private TextBox txtSeciliID;
        private TextBox txtSeciliAd;
        private TextBox txtSeciliKategori;
        private RichTextBox rtbSeciliAciklama;
        private ComboBox cmbPersonel;
        private ComboBox cmbOncelik;
        private TextBox txtNot;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        public PersonelForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Personel İşlem Formunun görsel arayüz elemanlarını oluşturur ve düzenler.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "T.C. Göksu Belediyesi - Personel Atama & Görev Yönetimi";
            this.Size = new Size(1100, 680);
            this.MinimumSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.AutoScroll = true;

            // Arka plan rengi
            this.Paint += Form_Paint;

            // Ana Layout
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70f)); // Header
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f)); // Status

            this.Controls.Add(mainLayout);

            // 1. Üst Panel
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(160, 18, 30, 49)
            };
            Label lblLogo = new Label
            {
                Text = "🏛️  GÖKSU BELEDİYESİ",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.Gold,
                Location = new Point(20, 18),
                AutoSize = true
            };
            Label lblSubTitle = new Label
            {
                Text = "PERSONEL İŞLEM VE GÖREV ATAMA KONTROLÜ",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(280, 24),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblSubTitle);
            mainLayout.Controls.Add(pnlHeader, 0, 0);

            // 2. Orta Bölüm (Sol: Başvurular Grid, Sağ: Atama Paneli)
            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(12),
                BackColor = Color.Transparent
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55f));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f));
            mainLayout.Controls.Add(contentLayout, 0, 1);

            // Sol Panel: Başvuru Listesi
            Panel pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 255, 255, 255),
                Padding = new Padding(12),
                Margin = new Padding(6)
            };
            pnlLeft.Paint += PanelBorder_Paint;

            TableLayoutPanel gridLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            gridLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            gridLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            Label lblGridTitle = new Label
            {
                Text = "📥 Gelen Tüm Başvuru ve Şikayet Talepleri",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 40, 80),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            gridLayout.Controls.Add(lblGridTitle, 0, 0);

            dgvBasvurular = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(220, 225, 230),
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9f)
            };
            dgvBasvurular.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 50, 90);
            dgvBasvurular.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBasvurular.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvBasvurular.EnableHeadersVisualStyles = false;
            dgvBasvurular.RowTemplate.Height = 35;
            dgvBasvurular.SelectionChanged += DgvBasvurular_SelectionChanged;
            dgvBasvurular.CellFormatting += DgvBasvurular_CellFormatting;

            gridLayout.Controls.Add(dgvBasvurular, 0, 1);
            pnlLeft.Controls.Add(gridLayout);
            contentLayout.Controls.Add(pnlLeft, 0, 0);

            // Sağ Panel: Detay ve Görev Atama Formu
            Panel pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 255, 255, 255),
                Padding = new Padding(16),
                Margin = new Padding(6),
                AutoScroll = true
            };
            pnlRight.Paint += PanelBorder_Paint;

            TableLayoutPanel assignLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 13,
                BackColor = Color.Transparent
            };

            for (int i = 0; i < 13; i++)
            {
                if (i == 7) // Açıklama Readonly
                    assignLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70f));
                else if (i == 11) // Not
                    assignLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45f));
                else if (i == 12) // Buton Grubu
                    assignLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100f));
                else if (i % 2 == 0) // Label satırları
                    assignLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
                else
                    assignLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));
            }

            int idx = 1;

            // Başvuru ID
            assignLayout.Controls.Add(new Label { Text = "Seçili Başvuru ID", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) }, 0, 0);
            txtSeciliID = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Segoe UI", 9f), TabIndex = idx++ };
            assignLayout.Controls.Add(txtSeciliID, 0, 1);

            // Ad Soyad
            assignLayout.Controls.Add(new Label { Text = "Başvuru Sahibi", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) }, 0, 2);
            txtSeciliAd = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Segoe UI", 9f), TabIndex = idx++ };
            assignLayout.Controls.Add(txtSeciliAd, 0, 3);

            // Kategori
            assignLayout.Controls.Add(new Label { Text = "Kategori", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) }, 0, 4);
            txtSeciliKategori = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Segoe UI", 9f), TabIndex = idx++ };
            assignLayout.Controls.Add(txtSeciliKategori, 0, 5);

            // Açıklama
            assignLayout.Controls.Add(new Label { Text = "Açıklama Detayı", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) }, 0, 6);
            rtbSeciliAciklama = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Segoe UI", 9f), TabIndex = idx++ };
            assignLayout.Controls.Add(rtbSeciliAciklama, 0, 7);

            // Personel Seç
            assignLayout.Controls.Add(new Label { Text = "Atanacak Personel", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) }, 0, 8);
            cmbPersonel = new ComboBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9f), DropDownStyle = ComboBoxStyle.DropDownList, TabIndex = idx++ };
            // Personel combobox listesini doldur
            foreach (var pers in SharedData.Personeller)
            {
                cmbPersonel.Items.Add(pers.AdSoyad);
            }
            if (cmbPersonel.Items.Count > 0) cmbPersonel.SelectedIndex = 0;
            assignLayout.Controls.Add(cmbPersonel, 0, 9);

            // Öncelik
            assignLayout.Controls.Add(new Label { Text = "Öncelik Derecesi", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) }, 0, 10);
            cmbOncelik = new ComboBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9f), DropDownStyle = ComboBoxStyle.DropDownList, TabIndex = idx++ };
            cmbOncelik.Items.AddRange(new[] { "Düşük", "Normal", "Yüksek", "Acil" });
            cmbOncelik.SelectedIndex = 1;
            assignLayout.Controls.Add(cmbOncelik, 0, 11);

            // Not Ekle
            assignLayout.Controls.Add(new Label { Text = "Atama Notu / İşlem Notu", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) }, 0, 10);
            // Satır çakışması olmaması için 11. satır not yazısı, 12. satır not girdisidir
            txtNot = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9f), TabIndex = idx++ };
            assignLayout.Controls.Add(txtNot, 0, 11);

            // Butonlar Layout (TableLayoutPanel ile buton çakışmaları tamamen önlenir)
            TableLayoutPanel btnLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };
            btnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            btnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            btnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            btnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            Button btnAta = new Button
            {
                Text = "👤 Personel Ata",
                Dock = DockStyle.Fill,
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = idx++
            };
            btnAta.FlatAppearance.BorderSize = 0;
            btnAta.Click += BtnAta_Click;

            Button btnGoreveAl = new Button
            {
                Text = "⚡ Göreve Al",
                Dock = DockStyle.Fill,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = idx++
            };
            btnGoreveAl.FlatAppearance.BorderSize = 0;
            btnGoreveAl.Click += BtnGoreveAl_Click;

            Button btnTamamla = new Button
            {
                Text = "✓ Tamamlandı",
                Dock = DockStyle.Fill,
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = idx++
            };
            btnTamamla.FlatAppearance.BorderSize = 0;
            btnTamamla.Click += BtnTamamla_Click;

            Button btnReddet = new Button
            {
                Text = "✕ Reddet",
                Dock = DockStyle.Fill,
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = idx++
            };
            btnReddet.FlatAppearance.BorderSize = 0;
            btnReddet.Click += BtnReddet_Click;

            // Butonları 2x2 grid yapısına yerleştir
            btnLayout.Controls.Add(btnAta, 0, 0);
            btnLayout.Controls.Add(btnGoreveAl, 1, 0);
            btnLayout.Controls.Add(btnTamamla, 0, 1);
            btnLayout.Controls.Add(btnReddet, 1, 1);

            assignLayout.Controls.Add(btnLayout, 0, 12);
            pnlRight.Controls.Add(assignLayout);
            contentLayout.Controls.Add(pnlRight, 1, 0);

            // 3. Status Bar
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel { Text = "Hazır. Atama yapmak veya durum değiştirmek için listeden başvuru seçin." };
            statusStrip.Items.Add(lblStatus);
            mainLayout.Controls.Add(statusStrip, 0, 2);

            YenileGrid();
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(20, 60, 120),
                Color.FromArgb(100, 160, 220),
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            DrawBackground(e.Graphics);
        }

        private void DrawBackground(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(Color.FromArgb(25, 255, 255, 255)))
            {
                g.FillPolygon(brush, new Point[] {
                    new Point(0, this.Height),
                    new Point(this.Width / 5, this.Height - 120),
                    new Point(this.Width / 3, this.Height)
                });
                g.FillPolygon(brush, new Point[] {
                    new Point(this.Width / 2, this.Height),
                    new Point(3 * this.Width / 4, this.Height - 160),
                    new Point(this.Width, this.Height)
                });
            }
        }

        private void PanelBorder_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            if (p != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(120, 255, 255, 255), 1.5f))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            }
        }

        /// <summary>
        /// Grid satırlarını başvuruların güncel durumuna göre renklendirir.
        /// </summary>
        private void DgvBasvurular_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvBasvurular.Rows.Count)
            {
                var row = dgvBasvurular.Rows[e.RowIndex];
                var cellDurum = row.Cells["Durum"];
                if (cellDurum != null && cellDurum.Value != null)
                {
                    string durum = cellDurum.Value.ToString();
                    if (durum == "Bekliyor")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 230); // Açık Sarı
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 90, 0);
                    }
                    else if (durum == "Atandı")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(224, 240, 255); // Açık Mavi
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 50, 140);
                    }
                    else if (durum == "Tamamlandı")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(224, 255, 224); // Açık Yeşil
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 100, 0);
                    }
                    else if (durum == "Reddedildi")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224); // Açık Kırmızı
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(140, 0, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Seçim değiştiğinde sağ taraftaki form alanlarını doldurur.
        /// </summary>
        private void DgvBasvurular_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBasvurular.SelectedRows.Count > 0)
            {
                var row = dgvBasvurular.SelectedRows[0];
                var basvuru = row.DataBoundItem as Basvuru;
                if (basvuru != null)
                {
                    txtSeciliID.Text = basvuru.BasvuruID;
                    txtSeciliAd.Text = basvuru.AdSoyad;
                    txtSeciliKategori.Text = basvuru.Kategori;
                    rtbSeciliAciklama.Text = basvuru.Aciklama;
                    txtNot.Text = basvuru.AtanmaNotari;

                    if (!string.IsNullOrEmpty(basvuru.Oncelik) && cmbOncelik.Items.Contains(basvuru.Oncelik))
                        cmbOncelik.SelectedItem = basvuru.Oncelik;
                    else
                        cmbOncelik.SelectedIndex = 1; // Normal

                    if (!string.IsNullOrEmpty(basvuru.AtananPersonel))
                        cmbPersonel.Text = basvuru.AtananPersonel;
                    else
                        cmbPersonel.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Seçili başvuruyu combobox'tan seçilen personele atar.
        /// </summary>
        private void BtnAta_Click(object sender, EventArgs e)
        {
            GuncelleSeciliBasvuru("Atandı", cmbPersonel.SelectedItem?.ToString());
        }

        /// <summary>
        /// Seçili görevi form üzerinde çalışan varsayılan personele atar.
        /// </summary>
        private void BtnGoreveAl_Click(object sender, EventArgs e)
        {
            GuncelleSeciliBasvuru("Atandı", "Sistem Personeli");
        }

        /// <summary>
        /// Seçili başvuruyu tamamlandı olarak işaretler.
        /// </summary>
        private void BtnTamamla_Click(object sender, EventArgs e)
        {
            GuncelleSeciliBasvuru("Tamamlandı", null);
        }

        /// <summary>
        /// Seçili başvuruyu reddeder.
        /// </summary>
        private void BtnReddet_Click(object sender, EventArgs e)
        {
            GuncelleSeciliBasvuru("Reddedildi", null);
        }

        /// <summary>
        /// Seçili başvuru kaydını durum, atanan personel ve not detayları ile günceller.
        /// </summary>
        private void GuncelleSeciliBasvuru(string yeniDurum, string personel)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSeciliID.Text))
                {
                    MessageBox.Show("Lütfen işlem yapmak istediğiniz başvuruyu listeden seçin.", "Seçim Yok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string id = txtSeciliID.Text;
                var basvuru = SharedData.Basvurular.FirstOrDefault(b => b.BasvuruID == id);

                if (basvuru != null)
                {
                    basvuru.Durum = yeniDurum;
                    if (personel != null)
                    {
                        basvuru.AtananPersonel = personel;
                        basvuru.AtanmaTarihi = DateTime.Now;

                        // İlgili personelin atanmış iş sayısını artır
                        var p = SharedData.Personeller.FirstOrDefault(x => x.AdSoyad == personel);
                        if (p != null) p.AtananIsCount++;
                    }

                    basvuru.Oncelik = cmbOncelik.SelectedItem?.ToString() ?? "Normal";
                    basvuru.AtanmaNotari = txtNot.Text.Trim();

                    lblStatus.Text = $"{id} numaralı başvurunun durumu '{yeniDurum}' olarak güncellendi.";
                    MessageBox.Show($"Başvuru başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    YenileGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İşlem sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void YenileGrid()
        {
            dgvBasvurular.DataSource = null;
            dgvBasvurular.DataSource = SharedData.Basvurular;

            if (dgvBasvurular.Columns.Count > 0)
            {
                dgvBasvurular.Columns["BasvuruID"].HeaderText = "Başvuru ID";
                dgvBasvurular.Columns["AdSoyad"].HeaderText = "Ad Soyad";
                dgvBasvurular.Columns["Kategori"].HeaderText = "Kategori";
                dgvBasvurular.Columns["Durum"].HeaderText = "Durum";
                dgvBasvurular.Columns["BasvuruTarihi"].HeaderText = "Tarih";

                // Diğer kolonları gizle
                foreach (DataGridViewColumn col in dgvBasvurular.Columns)
                {
                    if (col.Name != "BasvuruID" && col.Name != "AdSoyad" && col.Name != "Kategori" && col.Name != "Durum" && col.Name != "BasvuruTarihi")
                    {
                        col.Visible = false;
                    }
                }
            }
        }
    }
}
