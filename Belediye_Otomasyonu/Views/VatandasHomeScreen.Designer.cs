namespace Belediye_Otomasyonu.Views
{
    partial class VatandasHomeScreen
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Label lblHosgeldin;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelOzet;
        private System.Windows.Forms.Label lblOzetBaslik;
        private System.Windows.Forms.Label lblOzetSatir1;
        private System.Windows.Forms.Label lblOzetSatir2;
        private System.Windows.Forms.Label lblAciklama;
        private System.Windows.Forms.TableLayoutPanel panelAltCubuk;
        private System.Windows.Forms.Button btnOzetYenile;
        private System.Windows.Forms.Button btnCikis;

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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.lblHosgeldin = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblAciklama = new System.Windows.Forms.Label();
            this.panelAltCubuk = new System.Windows.Forms.TableLayoutPanel();
            this.btnOzetYenile = new System.Windows.Forms.Button();
            this.btnCikis = new System.Windows.Forms.Button();
            this.panelOzet = new System.Windows.Forms.Panel();
            this.lblOzetSatir2 = new System.Windows.Forms.Label();
            this.lblOzetSatir1 = new System.Windows.Forms.Label();
            this.lblOzetBaslik = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelAltCubuk.SuspendLayout();
            this.panelOzet.SuspendLayout();
            this.SuspendLayout();
            //
            // panelHeader
            //
            this.panelHeader.Controls.Add(this.lblHosgeldin);
            this.panelHeader.Controls.Add(this.lblBaslik);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(24, 20, 24, 16);
            this.panelHeader.Size = new System.Drawing.Size(720, 100);
            this.panelHeader.TabIndex = 0;
            //
            // lblBaslik
            //
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblBaslik.Location = new System.Drawing.Point(24, 20);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(200, 30);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "Vatandaş hizmetleri";
            //
            // lblHosgeldin
            //
            this.lblHosgeldin.AutoSize = true;
            this.lblHosgeldin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblHosgeldin.Location = new System.Drawing.Point(24, 56);
            this.lblHosgeldin.Name = "lblHosgeldin";
            this.lblHosgeldin.Size = new System.Drawing.Size(75, 19);
            this.lblHosgeldin.TabIndex = 1;
            this.lblHosgeldin.Text = "Hoş geldiniz";
            //
            // panelContent
            //
            this.panelContent.Controls.Add(this.panelAltCubuk);
            this.panelContent.Controls.Add(this.panelOzet);
            this.panelContent.Controls.Add(this.lblAciklama);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 100);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.panelContent.Size = new System.Drawing.Size(720, 312);
            this.panelContent.TabIndex = 1;
            //
            // panelOzet
            //
            this.panelOzet.Controls.Add(this.lblOzetSatir2);
            this.panelOzet.Controls.Add(this.lblOzetSatir1);
            this.panelOzet.Controls.Add(this.lblOzetBaslik);
            this.panelOzet.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOzet.Location = new System.Drawing.Point(24, 16);
            this.panelOzet.Name = "panelOzet";
            this.panelOzet.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.panelOzet.Size = new System.Drawing.Size(672, 88);
            this.panelOzet.TabIndex = 2;
            //
            // lblOzetBaslik
            //
            this.lblOzetBaslik.AutoSize = true;
            this.lblOzetBaslik.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblOzetBaslik.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.lblOzetBaslik.Location = new System.Drawing.Point(0, 0);
            this.lblOzetBaslik.Name = "lblOzetBaslik";
            this.lblOzetBaslik.Size = new System.Drawing.Size(91, 20);
            this.lblOzetBaslik.TabIndex = 0;
            this.lblOzetBaslik.Text = "Genel özet";
            //
            // lblOzetSatir1
            //
            this.lblOzetSatir1.AutoSize = true;
            this.lblOzetSatir1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblOzetSatir1.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblOzetSatir1.Location = new System.Drawing.Point(0, 28);
            this.lblOzetSatir1.Name = "lblOzetSatir1";
            this.lblOzetSatir1.Size = new System.Drawing.Size(120, 17);
            this.lblOzetSatir1.TabIndex = 1;
            this.lblOzetSatir1.Text = "Kayıtlı vatandaş: —";
            //
            // lblOzetSatir2
            //
            this.lblOzetSatir2.AutoSize = true;
            this.lblOzetSatir2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblOzetSatir2.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblOzetSatir2.Location = new System.Drawing.Point(0, 52);
            this.lblOzetSatir2.Name = "lblOzetSatir2";
            this.lblOzetSatir2.Size = new System.Drawing.Size(108, 17);
            this.lblOzetSatir2.TabIndex = 2;
            this.lblOzetSatir2.Text = "Toplam başvuru: —";
            //
            // lblAciklama
            //
            this.lblAciklama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAciklama.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAciklama.Location = new System.Drawing.Point(24, 104);
            this.lblAciklama.Name = "lblAciklama";
            this.lblAciklama.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblAciklama.Size = new System.Drawing.Size(672, 160);
            this.lblAciklama.TabIndex = 1;
            this.lblAciklama.Text = "Başvurularınız ve duyurular bu ekrandan yönetilebilecek. Aşağıdaki özet tüm belediye verisine aittir.";
            //
            // panelAltCubuk
            //
            this.panelAltCubuk.ColumnCount = 3;
            this.panelAltCubuk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelAltCubuk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.panelAltCubuk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.panelAltCubuk.Controls.Add(this.btnOzetYenile, 1, 0);
            this.panelAltCubuk.Controls.Add(this.btnCikis, 2, 0);
            this.panelAltCubuk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAltCubuk.Location = new System.Drawing.Point(24, 264);
            this.panelAltCubuk.Name = "panelAltCubuk";
            this.panelAltCubuk.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.panelAltCubuk.RowCount = 1;
            this.panelAltCubuk.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelAltCubuk.Size = new System.Drawing.Size(672, 56);
            this.panelAltCubuk.TabIndex = 0;
            //
            // btnOzetYenile
            //
            this.btnOzetYenile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOzetYenile.Location = new System.Drawing.Point(443, 11);
            this.btnOzetYenile.Margin = new System.Windows.Forms.Padding(8, 3, 6, 3);
            this.btnOzetYenile.Name = "btnOzetYenile";
            this.btnOzetYenile.Size = new System.Drawing.Size(102, 26);
            this.btnOzetYenile.TabIndex = 0;
            this.btnOzetYenile.Text = "Yenile";
            this.btnOzetYenile.UseVisualStyleBackColor = false;
            this.btnOzetYenile.Click += new System.EventHandler(this.btnOzetYenile_Click);
            //
            // btnCikis
            //
            this.btnCikis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCikis.Location = new System.Drawing.Point(559, 11);
            this.btnCikis.Margin = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(110, 26);
            this.btnCikis.TabIndex = 1;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            //
            // VatandasHomeScreen
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.ClientSize = new System.Drawing.Size(720, 412);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHeader);
            this.MinimumSize = new System.Drawing.Size(560, 380);
            this.Name = "VatandasHomeScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Belediye — vatandaş paneli";
            this.Load += new System.EventHandler(this.VatandasHomeScreen_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelAltCubuk.ResumeLayout(false);
            this.panelOzet.ResumeLayout(false);
            this.panelOzet.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
