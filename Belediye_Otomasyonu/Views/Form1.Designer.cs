namespace Belediye_Otomasyonu
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Belediye_lbl = new System.Windows.Forms.Label();
            this.Vatandas_btn = new System.Windows.Forms.Button();
            this.Personel_btn = new System.Windows.Forms.Button();
            this.Kayit_btn = new System.Windows.Forms.Button();
            this.Sifre_txt = new System.Windows.Forms.TextBox();
            this.KullaniciAdi_txt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Belediye_lbl
            // 
            this.Belediye_lbl.Font = new System.Drawing.Font("Arial Black", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Belediye_lbl.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.Belediye_lbl.Location = new System.Drawing.Point(88, 52);
            this.Belediye_lbl.MaximumSize = new System.Drawing.Size(500, 500);
            this.Belediye_lbl.MinimumSize = new System.Drawing.Size(5, 5);
            this.Belediye_lbl.Name = "Belediye_lbl";
            this.Belediye_lbl.Size = new System.Drawing.Size(398, 64);
            this.Belediye_lbl.TabIndex = 2;
            this.Belediye_lbl.Text = "Avcılar Belediyesi";
            this.Belediye_lbl.Click += new System.EventHandler(this.Belediye_lbl_Click);
            // 
            // Vatandas_btn
            // 
            this.Vatandas_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Vatandas_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Vatandas_btn.Location = new System.Drawing.Point(304, 246);
            this.Vatandas_btn.Name = "Vatandas_btn";
            this.Vatandas_btn.Size = new System.Drawing.Size(138, 42);
            this.Vatandas_btn.TabIndex = 3;
            this.Vatandas_btn.Text = "Vatandaş Girişi";
            this.Vatandas_btn.UseVisualStyleBackColor = false;
            this.Vatandas_btn.Click += new System.EventHandler(this.Vatandas_btn_Click);
            // 
            // Personel_btn
            // 
            this.Personel_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Personel_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Personel_btn.Location = new System.Drawing.Point(117, 246);
            this.Personel_btn.Name = "Personel_btn";
            this.Personel_btn.Size = new System.Drawing.Size(138, 42);
            this.Personel_btn.TabIndex = 4;
            this.Personel_btn.Text = "Personel Girişi";
            this.Personel_btn.UseVisualStyleBackColor = false;
            this.Personel_btn.Click += new System.EventHandler(this.Personel_btn_Click);
            // 
            // Kayit_btn
            // 
            this.Kayit_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Kayit_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Kayit_btn.Location = new System.Drawing.Point(212, 326);
            this.Kayit_btn.Name = "Kayit_btn";
            this.Kayit_btn.Size = new System.Drawing.Size(138, 42);
            this.Kayit_btn.TabIndex = 5;
            this.Kayit_btn.Text = "Vatandaş Kayıt Ol";
            this.Kayit_btn.UseVisualStyleBackColor = false;
            this.Kayit_btn.Click += new System.EventHandler(this.Kayit_btn_Click);
            // 
            // Sifre_txt
            // 
            this.Sifre_txt.BackColor = System.Drawing.SystemColors.Info;
            this.Sifre_txt.Font = new System.Drawing.Font("Ink Free", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Sifre_txt.Location = new System.Drawing.Point(117, 182);
            this.Sifre_txt.Name = "Sifre_txt";
            this.Sifre_txt.Size = new System.Drawing.Size(325, 21);
            this.Sifre_txt.TabIndex = 7;
            this.Sifre_txt.Text = "Lütfen Şifrenizi Giriniz";
            this.Sifre_txt.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // KullaniciAdi_txt
            // 
            this.KullaniciAdi_txt.BackColor = System.Drawing.SystemColors.Info;
            this.KullaniciAdi_txt.Font = new System.Drawing.Font("Ink Free", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.KullaniciAdi_txt.Location = new System.Drawing.Point(117, 143);
            this.KullaniciAdi_txt.Name = "KullaniciAdi_txt";
            this.KullaniciAdi_txt.Size = new System.Drawing.Size(325, 21);
            this.KullaniciAdi_txt.TabIndex = 8;
            this.KullaniciAdi_txt.Text = "Kullanıcı Adı:";
            this.KullaniciAdi_txt.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(531, 450);
            this.Controls.Add(this.KullaniciAdi_txt);
            this.Controls.Add(this.Sifre_txt);
            this.Controls.Add(this.Kayit_btn);
            this.Controls.Add(this.Personel_btn);
            this.Controls.Add(this.Vatandas_btn);
            this.Controls.Add(this.Belediye_lbl);
            this.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.Name = "Form1";
            this.Text = "Giris_Form";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label Belediye_lbl;
        private System.Windows.Forms.Button Vatandas_btn;
        private System.Windows.Forms.Button Personel_btn;
        private System.Windows.Forms.Button Kayit_btn;
        private System.Windows.Forms.TextBox Sifre_txt;
        private System.Windows.Forms.TextBox KullaniciAdi_txt;
    }
}

