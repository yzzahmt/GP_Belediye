namespace Belediye_Otomasyonu.Views
{
    partial class PersonelGiris
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
            this.btnSifremiUnuttum = new System.Windows.Forms.Button();
            this.btnGirisPrsn = new System.Windows.Forms.Button();
            this.btnYoneticiGiris = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabloAlan.SuspendLayout();
            this.SuspendLayout();
            //
            // pictureBox1
            //
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            this.tabloAlan.Controls.Add(this.btnSifremiUnuttum, 0, 2);
            this.tabloAlan.Controls.Add(this.btnGirisPrsn, 1, 3);
            this.tabloAlan.Controls.Add(this.btnYoneticiGiris, 1, 4);
            this.tabloAlan.Location = new System.Drawing.Point(190, 236);
            this.tabloAlan.Name = "tabloAlan";
            this.tabloAlan.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.tabloAlan.RowCount = 5;
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tabloAlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tabloAlan.SetColumnSpan(this.btnSifremiUnuttum, 2);
            this.tabloAlan.Size = new System.Drawing.Size(420, 206);
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
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
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
            this.txtSifre.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            //
            // btnSifremiUnuttum
            //
            this.btnSifremiUnuttum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSifremiUnuttum.FlatAppearance.BorderSize = 0;
            this.btnSifremiUnuttum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSifremiUnuttum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSifremiUnuttum.Location = new System.Drawing.Point(3, 80);
            this.btnSifremiUnuttum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSifremiUnuttum.Name = "btnSifremiUnuttum";
            this.btnSifremiUnuttum.Size = new System.Drawing.Size(414, 26);
            this.btnSifremiUnuttum.TabIndex = 3;
            this.btnSifremiUnuttum.Text = "Şifremi unuttum";
            this.btnSifremiUnuttum.UseVisualStyleBackColor = false;
            this.btnSifremiUnuttum.Click += new System.EventHandler(this.btnSifremiUnuttum_Click);
            //
            // btnGirisPrsn
            //
            this.btnGirisPrsn.BackColor = global::Belediye_Otomasyonu.UiTheme.Primary;
            this.btnGirisPrsn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGirisPrsn.FlatAppearance.BorderSize = 0;
            this.btnGirisPrsn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGirisPrsn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGirisPrsn.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.btnGirisPrsn.Location = new System.Drawing.Point(123, 112);
            this.btnGirisPrsn.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.btnGirisPrsn.Name = "btnGirisPrsn";
            this.btnGirisPrsn.Size = new System.Drawing.Size(294, 40);
            this.btnGirisPrsn.TabIndex = 4;
            this.btnGirisPrsn.Text = "Giriş Yap";
            this.btnGirisPrsn.UseVisualStyleBackColor = false;
            this.btnGirisPrsn.Click += new System.EventHandler(this.btnGirisPrsn_Click);
            //
            // btnYoneticiGiris
            //
            this.btnYoneticiGiris.BackColor = global::Belediye_Otomasyonu.UiTheme.PrimaryDark;
            this.btnYoneticiGiris.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnYoneticiGiris.FlatAppearance.BorderSize = 0;
            this.btnYoneticiGiris.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYoneticiGiris.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYoneticiGiris.ForeColor = global::Belediye_Otomasyonu.UiTheme.TextOnPrimary;
            this.btnYoneticiGiris.Location = new System.Drawing.Point(123, 160);
            this.btnYoneticiGiris.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.btnYoneticiGiris.Name = "btnYoneticiGiris";
            this.btnYoneticiGiris.Size = new System.Drawing.Size(294, 40);
            this.btnYoneticiGiris.TabIndex = 5;
            this.btnYoneticiGiris.Text = "Yönetici Girişi";
            this.btnYoneticiGiris.UseVisualStyleBackColor = false;
            this.btnYoneticiGiris.Click += new System.EventHandler(this.btnYoneticiGiris_Click);
            //
            // PersonelGiris
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = global::Belediye_Otomasyonu.UiTheme.Surface;
            this.ClientSize = new System.Drawing.Size(749, 480);
            this.Controls.Add(this.tabloAlan);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "PersonelGiris";
            this.Text = "Personel girişi";
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
        private System.Windows.Forms.Button btnSifremiUnuttum;
        private System.Windows.Forms.Button btnGirisPrsn;
        private System.Windows.Forms.Button btnYoneticiGiris;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tabloAlan;
    }
}
