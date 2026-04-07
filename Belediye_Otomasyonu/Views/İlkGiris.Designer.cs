namespace Belediye_Otomasyonu.Views
{
    partial class İlkGiris
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ilkVtnds_btn = new System.Windows.Forms.Button();
            this.ilkPrsnl_btn = new System.Windows.Forms.Button();
            this.ilkKyt_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Belediye_Otomasyonu.Properties.Resources.images;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(246, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(213, 192);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ilkVtnds_btn
            // 
            this.ilkVtnds_btn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ilkVtnds_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ilkVtnds_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ilkVtnds_btn.Font = new System.Drawing.Font("MV Boli", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ilkVtnds_btn.Location = new System.Drawing.Point(12, 321);
            this.ilkVtnds_btn.Name = "ilkVtnds_btn";
            this.ilkVtnds_btn.Size = new System.Drawing.Size(198, 69);
            this.ilkVtnds_btn.TabIndex = 1;
            this.ilkVtnds_btn.Text = "Vatandaş Girişi";
            this.ilkVtnds_btn.UseVisualStyleBackColor = false;
            this.ilkVtnds_btn.Click += new System.EventHandler(this.ilkVtnds_btn_Click);
            // 
            // ilkPrsnl_btn
            // 
            this.ilkPrsnl_btn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ilkPrsnl_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ilkPrsnl_btn.Font = new System.Drawing.Font("Segoe Print", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ilkPrsnl_btn.Location = new System.Drawing.Point(246, 321);
            this.ilkPrsnl_btn.Name = "ilkPrsnl_btn";
            this.ilkPrsnl_btn.Size = new System.Drawing.Size(198, 69);
            this.ilkPrsnl_btn.TabIndex = 2;
            this.ilkPrsnl_btn.Text = "Personel Gİrişi";
            this.ilkPrsnl_btn.UseVisualStyleBackColor = false;
            this.ilkPrsnl_btn.Click += new System.EventHandler(this.ilkPrsnl_btn_Click);
            // 
            // ilkKyt_btn
            // 
            this.ilkKyt_btn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ilkKyt_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ilkKyt_btn.Font = new System.Drawing.Font("Segoe Print", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ilkKyt_btn.Location = new System.Drawing.Point(477, 321);
            this.ilkKyt_btn.Name = "ilkKyt_btn";
            this.ilkKyt_btn.Size = new System.Drawing.Size(198, 69);
            this.ilkKyt_btn.TabIndex = 3;
            this.ilkKyt_btn.Text = "Kayıt Ol";
            this.ilkKyt_btn.UseVisualStyleBackColor = false;
            this.ilkKyt_btn.Click += new System.EventHandler(this.ilkKyt_btn_Click);
            // 
            // İlkGiris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Belediye_Otomasyonu.Properties.Resources.turkey_istanbul_wallpaper_preview;
            this.ClientSize = new System.Drawing.Size(727, 412);
            this.Controls.Add(this.ilkKyt_btn);
            this.Controls.Add(this.ilkPrsnl_btn);
            this.Controls.Add(this.ilkVtnds_btn);
            this.Controls.Add(this.pictureBox1);
            this.Name = "İlkGiris";
            this.Text = "İlkGiris";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ilkVtnds_btn;
        private System.Windows.Forms.Button ilkPrsnl_btn;
        private System.Windows.Forms.Button ilkKyt_btn;
    }
}