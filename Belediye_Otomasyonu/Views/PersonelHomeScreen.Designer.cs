namespace Belediye_Otomasyonu.Views
{
    partial class PersonelHomeScreen
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tableUstBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnPersonelYonetim;
        private System.Windows.Forms.Button btnYenile;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.TableLayoutPanel tableStats;
        private System.Windows.Forms.Panel cardVatandas;
        private System.Windows.Forms.Label lblCapVatandas;
        private System.Windows.Forms.Label lblKayitliVatandas;
        private System.Windows.Forms.Panel cardPersonel;
        private System.Windows.Forms.Label lblCapPersonel;
        private System.Windows.Forms.Label lblPersonelSayisi;
        private System.Windows.Forms.Panel cardBasvuru;
        private System.Windows.Forms.Label lblCapBasvuru;
        private System.Windows.Forms.Label lblToplamBasvuru;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.Label lblChartTitle;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableUstBar = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnPersonelYonetim = new System.Windows.Forms.Button();
            this.btnYenile = new System.Windows.Forms.Button();
            this.btnCikis = new System.Windows.Forms.Button();
            this.tableStats = new System.Windows.Forms.TableLayoutPanel();
            this.cardVatandas = new System.Windows.Forms.Panel();
            this.lblKayitliVatandas = new System.Windows.Forms.Label();
            this.lblCapVatandas = new System.Windows.Forms.Label();
            this.cardPersonel = new System.Windows.Forms.Panel();
            this.lblPersonelSayisi = new System.Windows.Forms.Label();
            this.lblCapPersonel = new System.Windows.Forms.Label();
            this.cardBasvuru = new System.Windows.Forms.Panel();
            this.lblToplamBasvuru = new System.Windows.Forms.Label();
            this.lblCapBasvuru = new System.Windows.Forms.Label();
            this.panelChart = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.tableUstBar.SuspendLayout();
            this.tableStats.SuspendLayout();
            this.cardVatandas.SuspendLayout();
            this.cardPersonel.SuspendLayout();
            this.cardBasvuru.SuspendLayout();
            this.panelChart.SuspendLayout();
            this.SuspendLayout();
            //
            // tableUstBar
            //
            this.tableUstBar.ColumnCount = 4;
            this.tableUstBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableUstBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableUstBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableUstBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableUstBar.Controls.Add(this.lblTitle, 0, 0);
            this.tableUstBar.Controls.Add(this.btnPersonelYonetim, 1, 0);
            this.tableUstBar.Controls.Add(this.btnYenile, 2, 0);
            this.tableUstBar.Controls.Add(this.btnCikis, 3, 0);
            this.tableUstBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableUstBar.Location = new System.Drawing.Point(0, 0);
            this.tableUstBar.Name = "tableUstBar";
            this.tableUstBar.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.tableUstBar.RowCount = 1;
            this.tableUstBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableUstBar.Size = new System.Drawing.Size(984, 56);
            this.tableUstBar.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(16, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(720, 36);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Personel paneli — özet";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnPersonelYonetim
            //
            this.btnPersonelYonetim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPersonelYonetim.FlatAppearance.BorderSize = 0;
            this.btnPersonelYonetim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPersonelYonetim.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnPersonelYonetim.Location = new System.Drawing.Point(736, 10);
            this.btnPersonelYonetim.Margin = new System.Windows.Forms.Padding(8, 0, 6, 0);
            this.btnPersonelYonetim.Name = "btnPersonelYonetim";
            this.btnPersonelYonetim.Size = new System.Drawing.Size(188, 36);
            this.btnPersonelYonetim.TabIndex = 1;
            this.btnPersonelYonetim.Text = "Personel kullanıcıları";
            this.btnPersonelYonetim.UseVisualStyleBackColor = false;
            this.btnPersonelYonetim.Click += new System.EventHandler(this.btnPersonelYonetim_Click);
            //
            // btnYenile
            //
            this.btnYenile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnYenile.Location = new System.Drawing.Point(744, 10);
            this.btnYenile.Margin = new System.Windows.Forms.Padding(8, 0, 6, 0);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(102, 36);
            this.btnYenile.TabIndex = 2;
            this.btnYenile.Text = "Yenile";
            this.btnYenile.UseVisualStyleBackColor = false;
            this.btnYenile.Click += new System.EventHandler(this.btnYenile_Click);
            //
            // btnCikis
            //
            this.btnCikis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCikis.Location = new System.Drawing.Point(858, 10);
            this.btnCikis.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(110, 36);
            this.btnCikis.TabIndex = 3;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            //
            // tableStats
            //
            this.tableStats.ColumnCount = 3;
            this.tableStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableStats.Controls.Add(this.cardVatandas, 0, 0);
            this.tableStats.Controls.Add(this.cardPersonel, 1, 0);
            this.tableStats.Controls.Add(this.cardBasvuru, 2, 0);
            this.tableStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableStats.Location = new System.Drawing.Point(0, 56);
            this.tableStats.Name = "tableStats";
            this.tableStats.Padding = new System.Windows.Forms.Padding(16, 16, 16, 8);
            this.tableStats.RowCount = 1;
            this.tableStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableStats.Size = new System.Drawing.Size(984, 132);
            this.tableStats.TabIndex = 1;
            //
            // cardVatandas
            //
            this.cardVatandas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardVatandas.Controls.Add(this.lblKayitliVatandas);
            this.cardVatandas.Controls.Add(this.lblCapVatandas);
            this.cardVatandas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardVatandas.Location = new System.Drawing.Point(27, 27);
            this.cardVatandas.Margin = new System.Windows.Forms.Padding(8);
            this.cardVatandas.Name = "cardVatandas";
            this.cardVatandas.Padding = new System.Windows.Forms.Padding(12);
            this.cardVatandas.Size = new System.Drawing.Size(295, 82);
            this.cardVatandas.TabIndex = 0;
            //
            // lblKayitliVatandas
            //
            this.lblKayitliVatandas.AutoSize = true;
            this.lblKayitliVatandas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblKayitliVatandas.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKayitliVatandas.Location = new System.Drawing.Point(12, 40);
            this.lblKayitliVatandas.Name = "lblKayitliVatandas";
            this.lblKayitliVatandas.Size = new System.Drawing.Size(33, 37);
            this.lblKayitliVatandas.TabIndex = 1;
            this.lblKayitliVatandas.Text = "0";
            //
            // lblCapVatandas
            //
            this.lblCapVatandas.AutoSize = true;
            this.lblCapVatandas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCapVatandas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCapVatandas.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCapVatandas.Location = new System.Drawing.Point(12, 12);
            this.lblCapVatandas.Name = "lblCapVatandas";
            this.lblCapVatandas.Size = new System.Drawing.Size(95, 15);
            this.lblCapVatandas.TabIndex = 0;
            this.lblCapVatandas.Text = "Kayıtlı vatandaş";
            //
            // cardPersonel
            //
            this.cardPersonel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardPersonel.Controls.Add(this.lblPersonelSayisi);
            this.cardPersonel.Controls.Add(this.lblCapPersonel);
            this.cardPersonel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPersonel.Location = new System.Drawing.Point(338, 27);
            this.cardPersonel.Margin = new System.Windows.Forms.Padding(8);
            this.cardPersonel.Name = "cardPersonel";
            this.cardPersonel.Padding = new System.Windows.Forms.Padding(12);
            this.cardPersonel.Size = new System.Drawing.Size(295, 82);
            this.cardPersonel.TabIndex = 1;
            //
            // lblPersonelSayisi
            //
            this.lblPersonelSayisi.AutoSize = true;
            this.lblPersonelSayisi.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPersonelSayisi.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPersonelSayisi.Location = new System.Drawing.Point(12, 40);
            this.lblPersonelSayisi.Name = "lblPersonelSayisi";
            this.lblPersonelSayisi.Size = new System.Drawing.Size(33, 37);
            this.lblPersonelSayisi.TabIndex = 1;
            this.lblPersonelSayisi.Text = "0";
            //
            // lblCapPersonel
            //
            this.lblCapPersonel.AutoSize = true;
            this.lblCapPersonel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCapPersonel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCapPersonel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCapPersonel.Location = new System.Drawing.Point(12, 12);
            this.lblCapPersonel.Name = "lblCapPersonel";
            this.lblCapPersonel.Size = new System.Drawing.Size(53, 15);
            this.lblCapPersonel.TabIndex = 0;
            this.lblCapPersonel.Text = "Personel";
            //
            // cardBasvuru
            //
            this.cardBasvuru.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardBasvuru.Controls.Add(this.lblToplamBasvuru);
            this.cardBasvuru.Controls.Add(this.lblCapBasvuru);
            this.cardBasvuru.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardBasvuru.Location = new System.Drawing.Point(649, 27);
            this.cardBasvuru.Margin = new System.Windows.Forms.Padding(8);
            this.cardBasvuru.Name = "cardBasvuru";
            this.cardBasvuru.Padding = new System.Windows.Forms.Padding(12);
            this.cardBasvuru.Size = new System.Drawing.Size(295, 82);
            this.cardBasvuru.TabIndex = 2;
            //
            // lblToplamBasvuru
            //
            this.lblToplamBasvuru.AutoSize = true;
            this.lblToplamBasvuru.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblToplamBasvuru.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblToplamBasvuru.Location = new System.Drawing.Point(12, 40);
            this.lblToplamBasvuru.Name = "lblToplamBasvuru";
            this.lblToplamBasvuru.Size = new System.Drawing.Size(33, 37);
            this.lblToplamBasvuru.TabIndex = 1;
            this.lblToplamBasvuru.Text = "0";
            //
            // lblCapBasvuru
            //
            this.lblCapBasvuru.AutoSize = true;
            this.lblCapBasvuru.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCapBasvuru.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCapBasvuru.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCapBasvuru.Location = new System.Drawing.Point(12, 12);
            this.lblCapBasvuru.Name = "lblCapBasvuru";
            this.lblCapBasvuru.Size = new System.Drawing.Size(88, 15);
            this.lblCapBasvuru.TabIndex = 0;
            this.lblCapBasvuru.Text = "Toplam başvuru";
            //
            // panelChart
            //
            this.panelChart.Controls.Add(this.cartesianChart1);
            this.panelChart.Controls.Add(this.lblChartTitle);
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.Location = new System.Drawing.Point(0, 188);
            this.panelChart.Name = "panelChart";
            this.panelChart.Padding = new System.Windows.Forms.Padding(16, 8, 16, 16);
            this.panelChart.Size = new System.Drawing.Size(984, 373);
            this.panelChart.TabIndex = 2;
            //
            // lblChartTitle
            //
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblChartTitle.Location = new System.Drawing.Point(16, 8);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblChartTitle.Size = new System.Drawing.Size(234, 29);
            this.lblChartTitle.TabIndex = 1;
            this.lblChartTitle.Text = "Başvuru durumları (özet)";
            //
            // cartesianChart1
            //
            this.cartesianChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart1.Location = new System.Drawing.Point(16, 37);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(952, 320);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            //
            // PersonelHomeScreen
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.panelChart);
            this.Controls.Add(this.tableStats);
            this.Controls.Add(this.tableUstBar);
            this.MinimumSize = new System.Drawing.Size(800, 480);
            this.Name = "PersonelHomeScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Belediye — personel paneli";
            this.Load += new System.EventHandler(this.PersonelHomeScreen_Load);
            this.tableUstBar.ResumeLayout(false);
            this.tableUstBar.PerformLayout();
            this.tableStats.ResumeLayout(false);
            this.cardVatandas.ResumeLayout(false);
            this.cardVatandas.PerformLayout();
            this.cardPersonel.ResumeLayout(false);
            this.cardPersonel.PerformLayout();
            this.cardBasvuru.ResumeLayout(false);
            this.cardBasvuru.PerformLayout();
            this.panelChart.ResumeLayout(false);
            this.panelChart.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
