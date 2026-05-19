namespace Belediye_Otomasyonu
{
    partial class Kayit_Screen
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
            this.Soyad_txt = new System.Windows.Forms.MaskedTextBox();
            this.Ad_txt = new System.Windows.Forms.MaskedTextBox();
            this.kayit_lbl = new System.Windows.Forms.Label();
            this.Tc_txt = new System.Windows.Forms.MaskedTextBox();
            this.Email_txt = new System.Windows.Forms.MaskedTextBox();
            this.KullaniciAdi_txt = new System.Windows.Forms.MaskedTextBox();
            this.Sifre_txt = new System.Windows.Forms.MaskedTextBox();
            this.btnGeri = new System.Windows.Forms.Button();
            this.kayitOl_btn = new System.Windows.Forms.Button();
            this.Name_lbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Soyad_txt
            // 
            this.Soyad_txt.Location = new System.Drawing.Point(116, 144);
            this.Soyad_txt.Name = "Soyad_txt";
            this.Soyad_txt.Size = new System.Drawing.Size(240, 20);
            this.Soyad_txt.TabIndex = 0;
            this.Soyad_txt.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.Soyad_txt_MaskInputRejected);
            // 
            // Ad_txt
            // 
            this.Ad_txt.Location = new System.Drawing.Point(116, 95);
            this.Ad_txt.Name = "Ad_txt";
            this.Ad_txt.Size = new System.Drawing.Size(240, 20);
            this.Ad_txt.TabIndex = 1;
            this.Ad_txt.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.Ad_txt_MaskInputRejected);
            // 
            // kayit_lbl
            // 
            this.kayit_lbl.BackColor = System.Drawing.Color.Transparent;
            this.kayit_lbl.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kayit_lbl.ForeColor = System.Drawing.Color.LightGreen;
            this.kayit_lbl.Location = new System.Drawing.Point(118, 48);
            this.kayit_lbl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 0);
            this.kayit_lbl.MaximumSize = new System.Drawing.Size(500, 50);
            this.kayit_lbl.MinimumSize = new System.Drawing.Size(150, 35);
            this.kayit_lbl.Name = "kayit_lbl";
            this.kayit_lbl.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.kayit_lbl.Size = new System.Drawing.Size(214, 50);
            this.kayit_lbl.TabIndex = 3;
            this.kayit_lbl.Text = "Kayıt Ol";
            // 
            // Tc_txt
            // 
            this.Tc_txt.Location = new System.Drawing.Point(116, 193);
            this.Tc_txt.Name = "Tc_txt";
            this.Tc_txt.Size = new System.Drawing.Size(240, 20);
            this.Tc_txt.TabIndex = 4;
            this.Tc_txt.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.Tc_txt_MaskInputRejected);
            // 
            // Email_txt
            // 
            this.Email_txt.Location = new System.Drawing.Point(116, 239);
            this.Email_txt.Name = "Email_txt";
            this.Email_txt.Size = new System.Drawing.Size(240, 20);
            this.Email_txt.TabIndex = 5;
            this.Email_txt.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.Email_txt_MaskInputRejected);
            // 
            // KullaniciAdi_txt
            // 
            this.KullaniciAdi_txt.Location = new System.Drawing.Point(116, 288);
            this.KullaniciAdi_txt.Name = "KullaniciAdi_txt";
            this.KullaniciAdi_txt.Size = new System.Drawing.Size(240, 20);
            this.KullaniciAdi_txt.TabIndex = 6;
            this.KullaniciAdi_txt.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.KullaniciAdi_txt_MaskInputRejected);
            // 
            // Sifre_txt
            // 
            this.Sifre_txt.Location = new System.Drawing.Point(116, 332);
            this.Sifre_txt.Name = "Sifre_txt";
            this.Sifre_txt.Size = new System.Drawing.Size(240, 20);
            this.Sifre_txt.TabIndex = 7;
            this.Sifre_txt.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.Sifre_txt_MaskInputRejected);
            // 
            // btnGeri
            // 
            this.btnGeri.FlatAppearance.BorderSize = 0;
            this.btnGeri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGeri.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGeri.Location = new System.Drawing.Point(16, 12);
            this.btnGeri.Name = "btnGeri";
            this.btnGeri.Size = new System.Drawing.Size(100, 28);
            this.btnGeri.TabIndex = 15;
            this.btnGeri.Text = "Geri";
            this.btnGeri.UseVisualStyleBackColor = false;
            this.btnGeri.Click += new System.EventHandler(this.btnGeri_Click);
            // 
            // kayitOl_btn
            // 
            this.kayitOl_btn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.kayitOl_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kayitOl_btn.Location = new System.Drawing.Point(116, 377);
            this.kayitOl_btn.Name = "kayitOl_btn";
            this.kayitOl_btn.Size = new System.Drawing.Size(240, 23);
            this.kayitOl_btn.TabIndex = 8;
            this.kayitOl_btn.Text = "Kayıt Ol";
            this.kayitOl_btn.UseVisualStyleBackColor = false;
            this.kayitOl_btn.Click += new System.EventHandler(this.kayitOl_btn_Click);
            // 
            // Name_lbl
            // 
            this.Name_lbl.BackColor = System.Drawing.Color.Transparent;
            this.Name_lbl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Name_lbl.Location = new System.Drawing.Point(29, 98);
            this.Name_lbl.Name = "Name_lbl";
            this.Name_lbl.Size = new System.Drawing.Size(61, 20);
            this.Name_lbl.TabIndex = 9;
            this.Name_lbl.Text = "Ad";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(29, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Soyad";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Location = new System.Drawing.Point(29, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Tc";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Location = new System.Drawing.Point(29, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "E Posta";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Location = new System.Drawing.Point(29, 291);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Kullanıcı Adı";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Location = new System.Drawing.Point(29, 335);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Şifre";
            // 
            // Kayit_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = global::Belediye_Otomasyonu.UiTheme.Surface;
            this.ClientSize = new System.Drawing.Size(485, 450);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Name_lbl);
            this.Controls.Add(this.kayitOl_btn);
            this.Controls.Add(this.Sifre_txt);
            this.Controls.Add(this.KullaniciAdi_txt);
            this.Controls.Add(this.Email_txt);
            this.Controls.Add(this.Tc_txt);
            this.Controls.Add(this.kayit_lbl);
            this.Controls.Add(this.Ad_txt);
            this.Controls.Add(this.Soyad_txt);
            this.Controls.Add(this.btnGeri);
            this.Name = "Kayit_Screen";
            this.Text = "Kayit_Screen";
            this.Load += new System.EventHandler(this.Kayit_Screen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox Soyad_txt;
        private System.Windows.Forms.MaskedTextBox Ad_txt;
        private System.Windows.Forms.Label kayit_lbl;
        private System.Windows.Forms.MaskedTextBox Tc_txt;
        private System.Windows.Forms.MaskedTextBox Email_txt;
        private System.Windows.Forms.MaskedTextBox KullaniciAdi_txt;
        private System.Windows.Forms.MaskedTextBox Sifre_txt;
        private System.Windows.Forms.Button btnGeri;
        private System.Windows.Forms.Button kayitOl_btn;
        private System.Windows.Forms.Label Name_lbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
