namespace Belediye_Otomasyonu.Views
{
    partial class VatandasGiris
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
            this.Geri_btn = new System.Windows.Forms.Button();
            this.tabloAlan = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textSifre = new System.Windows.Forms.TextBox();
            this.lblSifremiUnuttum = new System.Windows.Forms.LinkLabel();
            this.btnGirisYap = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabloAlan.SuspendLayout();
            this.SuspendLayout();
            //
            // pictureBox1
            //
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Location = new System.Drawing.Point(295, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(210, 176);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            //
            // Geri_btn
            //
            this.Geri_btn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.Geri_btn.BackColor = global::Belediye_Otomasyonu.UiTheme.PrimaryDark;
            this.Geri_btn.FlatAppearance.BorderSize = 0;
            this.Geri_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Geri_btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Geri_btn.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.Geri_btn.Location = new System.Drawing.Point(16, 16);
            this.Geri_btn.Name = "Geri_btn";
            this.Geri_btn.Size = new System.Drawing.Size(100, 30);
            this.Geri_btn.TabIndex = 11;
            this.Geri_btn.Text = "Geri Dön";
            this.Geri_btn.UseVisualStyleBackColor = false;
            this.Geri_btn.Click += new System.EventHandler(this.Geri_btn_Click);
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
            this.tabloAlan.Controls.Add(this.textBox2, 1, 1);
            this.tabloAlan.Controls.Add(this.label2, 0, 2);
            this.tabloAlan.Controls.Add(this.txtTC, 1, 2);
            this.tabloAlan.Controls.Add(this.label4, 0, 3);
            this.tabloAlan.Controls.Add(this.textSifre, 1, 3);
            this.tabloAlan.Controls.Add(this.lblSifremiUnuttum, 0, 4);
            this.tabloAlan.Controls.Add(this.btnGirisYap, 1, 5);
            this.tabloAlan.Location = new System.Drawing.Point(190, 210);
            this.tabloAlan.Name = "tabloAlan";
            this.tabloAlan.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.tabloAlan.RowCount = 6;
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tabloAlan.SetColumnSpan(this.lblSifremiUnuttum, 2);
            this.tabloAlan.Size = new System.Drawing.Size(420, 250);
            this.tabloAlan.TabIndex = 12;
            //
            // label1
            //
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(91, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ad";
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
            this.label3.Location = new System.Drawing.Point(75, 47);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Soyad";
            //
            // textBox2
            //
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(123, 44);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(294, 20);
            this.textBox2.TabIndex = 2;
            //
            // label2
            //
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(91, 83);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "TC";
            //
            // txtTC
            //
            this.txtTC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTC.Location = new System.Drawing.Point(123, 80);
            this.txtTC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTC.MaxLength = 11;
            this.txtTC.Name = "txtTC";
            this.txtTC.Size = new System.Drawing.Size(294, 20);
            this.txtTC.TabIndex = 3;
            this.txtTC.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            //
            // label4
            //
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(84, 119);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Şifre";
            //
            // textSifre
            //
            this.textSifre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textSifre.Location = new System.Drawing.Point(123, 116);
            this.textSifre.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textSifre.Name = "textSifre";
            this.textSifre.PasswordChar = '●';
            this.textSifre.Size = new System.Drawing.Size(294, 20);
            this.textSifre.TabIndex = 4;
            this.textSifre.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            //
            //
            // lblSifremiUnuttum
            //
            this.lblSifremiUnuttum.AutoSize = true;
            this.lblSifremiUnuttum.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSifremiUnuttum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSifremiUnuttum.Location = new System.Drawing.Point(320, 160);
            this.lblSifremiUnuttum.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblSifremiUnuttum.Name = "lblSifremiUnuttum";
            this.lblSifremiUnuttum.Size = new System.Drawing.Size(95, 15);
            this.lblSifremiUnuttum.TabIndex = 9;
            this.lblSifremiUnuttum.TabStop = true;
            this.lblSifremiUnuttum.Text = "Şifremi unuttum";
            this.lblSifremiUnuttum.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblSifremiUnuttum_LinkClicked);
            //
            // btnGirisYap
            //
            this.btnGirisYap.BackColor = global::Belediye_Otomasyonu.UiTheme.Primary;
            this.btnGirisYap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGirisYap.FlatAppearance.BorderSize = 0;
            this.btnGirisYap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGirisYap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGirisYap.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.btnGirisYap.Location = new System.Drawing.Point(123, 192);
            this.btnGirisYap.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.btnGirisYap.Name = "btnGirisYap";
            this.btnGirisYap.Size = new System.Drawing.Size(294, 52);
            this.btnGirisYap.TabIndex = 10;
            this.btnGirisYap.Text = "Giriş Yap";
            this.btnGirisYap.UseVisualStyleBackColor = false;
            this.btnGirisYap.Click += new System.EventHandler(this.btnGirisYap_Click);
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = global::Belediye_Otomasyonu.UiTheme.Surface;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabloAlan);
            this.Controls.Add(this.Geri_btn);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "VatandasGiris";
            this.Text = "Vatandaş girişi";
            this.Load += new System.EventHandler(this.VatandasGiris_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabloAlan.ResumeLayout(false);
            this.tabloAlan.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtTC;
        private System.Windows.Forms.TextBox textSifre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lblSifremiUnuttum;
        private System.Windows.Forms.Button btnGirisYap;
        private System.Windows.Forms.Button Geri_btn;
        private System.Windows.Forms.TableLayoutPanel tabloAlan;
    }
}
