namespace Belediye_Otomasyonu.Views
{
    partial class YoneticiGiris
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabloAlan = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.btnGirisYonetici = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabloAlan.SuspendLayout();
            this.SuspendLayout();
            //
            // pictureBox1
            //
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Location = new System.Drawing.Point(264, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(221, 200);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            //
            // button1
            //
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.button1.BackColor = global::Belediye_Otomasyonu.UiTheme.PrimaryDark;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.button1.Location = new System.Drawing.Point(16, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 9;
            this.button1.Text = "Geri Dön";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // tabloAlan
            //
            this.tabloAlan.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tabloAlan.ColumnCount = 2;
            this.tabloAlan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tabloAlan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tabloAlan.Controls.Add(this.label1, 0, 0);
            this.tabloAlan.Controls.Add(this.textBox1, 1, 0);
            this.tabloAlan.Controls.Add(this.label3, 0, 1);
            this.tabloAlan.Controls.Add(this.txtSifre, 1, 1);
            this.tabloAlan.Controls.Add(this.btnGirisYonetici, 1, 2);
            this.tabloAlan.Location = new System.Drawing.Point(190, 236);
            this.tabloAlan.Name = "tabloAlan";
            this.tabloAlan.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.tabloAlan.RowCount = 3;
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tabloAlan.Size = new System.Drawing.Size(420, 128);
            this.tabloAlan.TabIndex = 10;
            //
            // label1
            //
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(49, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kullanıcı adı";
            //
            // textBox1
            //
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(123, 8);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(294, 20);
            this.textBox1.TabIndex = 1;
            //
            // label3
            //
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(84, 47);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Şifre";
            //
            // txtSifre
            //
            this.txtSifre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSifre.Location = new System.Drawing.Point(123, 44);
            this.txtSifre.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '●';
            this.txtSifre.Size = new System.Drawing.Size(294, 20);
            this.txtSifre.TabIndex = 2;
            //
            // btnGirisYonetici
            //
            this.btnGirisYonetici.BackColor = global::Belediye_Otomasyonu.UiTheme.Primary;
            this.btnGirisYonetici.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGirisYonetici.FlatAppearance.BorderSize = 0;
            this.btnGirisYonetici.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGirisYonetici.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGirisYonetici.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.btnGirisYonetici.Location = new System.Drawing.Point(123, 84);
            this.btnGirisYonetici.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.btnGirisYonetici.Name = "btnGirisYonetici";
            this.btnGirisYonetici.Size = new System.Drawing.Size(294, 40);
            this.btnGirisYonetici.TabIndex = 4;
            this.btnGirisYonetici.Text = "Yönetici Girişi Yap";
            this.btnGirisYonetici.UseVisualStyleBackColor = false;
            this.btnGirisYonetici.Click += new System.EventHandler(this.btnGirisYonetici_Click);
            //
            // YoneticiGiris
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = global::Belediye_Otomasyonu.UiTheme.Surface;
            this.ClientSize = new System.Drawing.Size(749, 430);
            this.Controls.Add(this.tabloAlan);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "YoneticiGiris";
            this.Text = "Yönetici Girişi";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabloAlan.ResumeLayout(false);
            this.tabloAlan.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGirisYonetici;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tabloAlan;
    }
}
