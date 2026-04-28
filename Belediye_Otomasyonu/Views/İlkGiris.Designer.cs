namespace Belediye_Otomasyonu.Views
{
    partial class İlkGiris
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelAlt = new System.Windows.Forms.Panel();
            this.tabloButonlar = new System.Windows.Forms.TableLayoutPanel();
            this.ilkVtnds_btn = new System.Windows.Forms.Button();
            this.ilkPrsnl_btn = new System.Windows.Forms.Button();
            this.ilkKyt_btn = new System.Windows.Forms.Button();
            this.panelAlt.SuspendLayout();
            this.tabloButonlar.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTitle.ForeColor = global::Belediye_Otomasyonu.UiTheme.Primary;
            this.lblTitle.Location = new System.Drawing.Point(190, 80);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(347, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Belediye Otomasyonu";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // panelAlt
            //
            this.panelAlt.Controls.Add(this.tabloButonlar);
            this.panelAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAlt.Location = new System.Drawing.Point(0, 316);
            this.panelAlt.Name = "panelAlt";
            this.panelAlt.Padding = new System.Windows.Forms.Padding(16, 8, 16, 16);
            this.panelAlt.Size = new System.Drawing.Size(727, 96);
            this.panelAlt.TabIndex = 1;
            //
            // tabloButonlar
            //
            this.tabloButonlar.ColumnCount = 3;
            this.tabloButonlar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tabloButonlar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tabloButonlar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tabloButonlar.Controls.Add(this.ilkVtnds_btn, 0, 0);
            this.tabloButonlar.Controls.Add(this.ilkPrsnl_btn, 1, 0);
            this.tabloButonlar.Controls.Add(this.ilkKyt_btn, 2, 0);
            this.tabloButonlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabloButonlar.Location = new System.Drawing.Point(16, 8);
            this.tabloButonlar.Name = "tabloButonlar";
            this.tabloButonlar.RowCount = 1;
            this.tabloButonlar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tabloButonlar.Size = new System.Drawing.Size(695, 72);
            this.tabloButonlar.TabIndex = 0;
            //
            // ilkVtnds_btn
            //
            this.ilkVtnds_btn.BackColor = global::Belediye_Otomasyonu.UiTheme.Primary;
            this.ilkVtnds_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilkVtnds_btn.FlatAppearance.BorderSize = 0;
            this.ilkVtnds_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ilkVtnds_btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ilkVtnds_btn.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.ilkVtnds_btn.Location = new System.Drawing.Point(8, 8);
            this.ilkVtnds_btn.Margin = new System.Windows.Forms.Padding(8);
            this.ilkVtnds_btn.Name = "ilkVtnds_btn";
            this.ilkVtnds_btn.Size = new System.Drawing.Size(215, 56);
            this.ilkVtnds_btn.TabIndex = 0;
            this.ilkVtnds_btn.Text = "Vatandaş Girişi";
            this.ilkVtnds_btn.UseVisualStyleBackColor = false;
            this.ilkVtnds_btn.Click += new System.EventHandler(this.ilkVtnds_btn_Click);
            //
            // ilkPrsnl_btn
            //
            this.ilkPrsnl_btn.BackColor = global::Belediye_Otomasyonu.UiTheme.Primary;
            this.ilkPrsnl_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilkPrsnl_btn.FlatAppearance.BorderSize = 0;
            this.ilkPrsnl_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ilkPrsnl_btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ilkPrsnl_btn.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.ilkPrsnl_btn.Location = new System.Drawing.Point(239, 8);
            this.ilkPrsnl_btn.Margin = new System.Windows.Forms.Padding(8);
            this.ilkPrsnl_btn.Name = "ilkPrsnl_btn";
            this.ilkPrsnl_btn.Size = new System.Drawing.Size(215, 56);
            this.ilkPrsnl_btn.TabIndex = 1;
            this.ilkPrsnl_btn.Text = "Personel Girişi";
            this.ilkPrsnl_btn.UseVisualStyleBackColor = false;
            this.ilkPrsnl_btn.Click += new System.EventHandler(this.ilkPrsnl_btn_Click);
            //
            // ilkKyt_btn
            //
            this.ilkKyt_btn.BackColor = global::Belediye_Otomasyonu.UiTheme.Primary;
            this.ilkKyt_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilkKyt_btn.FlatAppearance.BorderSize = 0;
            this.ilkKyt_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ilkKyt_btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ilkKyt_btn.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.ilkKyt_btn.Location = new System.Drawing.Point(470, 8);
            this.ilkKyt_btn.Margin = new System.Windows.Forms.Padding(8);
            this.ilkKyt_btn.Name = "ilkKyt_btn";
            this.ilkKyt_btn.Size = new System.Drawing.Size(217, 56);
            this.ilkKyt_btn.TabIndex = 2;
            this.ilkKyt_btn.Text = "Kayıt Ol";
            this.ilkKyt_btn.UseVisualStyleBackColor = false;
            this.ilkKyt_btn.Click += new System.EventHandler(this.ilkKyt_btn_Click);
            //
            // İlkGiris
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = global::Belediye_Otomasyonu.UiTheme.Surface;
            this.ClientSize = new System.Drawing.Size(727, 412);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panelAlt);
            this.Name = "İlkGiris";
            this.Text = "Belediye — giriş";
            this.panelAlt.ResumeLayout(false);
            this.tabloButonlar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelAlt;
        private System.Windows.Forms.TableLayoutPanel tabloButonlar;
        private System.Windows.Forms.Button ilkVtnds_btn;
        private System.Windows.Forms.Button ilkPrsnl_btn;
        private System.Windows.Forms.Button ilkKyt_btn;
    }
}
