namespace Belediye_Otomasyonu.Views
{
    partial class SifremiUnuttumForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Label lblTc;
        private System.Windows.Forms.TextBox txtTc;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblYeni;
        private System.Windows.Forms.TextBox txtYeni;
        private System.Windows.Forms.Label lblTekrar;
        private System.Windows.Forms.TextBox txtYeniTekrar;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBaslik = new System.Windows.Forms.Label();
            this.lblTc = new System.Windows.Forms.Label();
            this.txtTc = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblYeni = new System.Windows.Forms.Label();
            this.txtYeni = new System.Windows.Forms.TextBox();
            this.lblTekrar = new System.Windows.Forms.Label();
            this.txtYeniTekrar = new System.Windows.Forms.TextBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.Location = new System.Drawing.Point(24, 20);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(280, 21);
            this.lblBaslik.Text = "Şifremi unuttum (vatandaş hesabı)";
            this.lblTc.AutoSize = true;
            this.lblTc.Location = new System.Drawing.Point(24, 60);
            this.lblTc.Name = "lblTc";
            this.lblTc.Size = new System.Drawing.Size(21, 13);
            this.lblTc.Text = "TC";
            this.txtTc.Location = new System.Drawing.Point(24, 76);
            this.txtTc.MaxLength = 11;
            this.txtTc.Name = "txtTc";
            this.txtTc.Size = new System.Drawing.Size(320, 20);
            this.txtTc.TabIndex = 0;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(24, 106);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(44, 13);
            this.lblEmail.Text = "E-posta";
            this.txtEmail.Location = new System.Drawing.Point(24, 122);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(320, 20);
            this.txtEmail.TabIndex = 1;
            this.lblYeni.AutoSize = true;
            this.lblYeni.Location = new System.Drawing.Point(24, 152);
            this.lblYeni.Name = "lblYeni";
            this.lblYeni.Size = new System.Drawing.Size(55, 13);
            this.lblYeni.Text = "Yeni şifre";
            this.txtYeni.Location = new System.Drawing.Point(24, 168);
            this.txtYeni.Name = "txtYeni";
            this.txtYeni.PasswordChar = '●';
            this.txtYeni.Size = new System.Drawing.Size(320, 20);
            this.txtYeni.TabIndex = 2;
            this.lblTekrar.AutoSize = true;
            this.lblTekrar.Location = new System.Drawing.Point(24, 198);
            this.lblTekrar.Name = "lblTekrar";
            this.lblTekrar.Size = new System.Drawing.Size(73, 13);
            this.lblTekrar.Text = "Şifre tekrarı";
            this.txtYeniTekrar.Location = new System.Drawing.Point(24, 214);
            this.txtYeniTekrar.Name = "txtYeniTekrar";
            this.txtYeniTekrar.PasswordChar = '●';
            this.txtYeniTekrar.Size = new System.Drawing.Size(320, 20);
            this.txtYeniTekrar.TabIndex = 3;
            this.btnKaydet.Location = new System.Drawing.Point(168, 256);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(176, 36);
            this.btnKaydet.TabIndex = 4;
            this.btnKaydet.Text = "Şifreyi güncelle";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            this.btnIptal.Location = new System.Drawing.Point(24, 256);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(120, 36);
            this.btnIptal.TabIndex = 5;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = false;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 321);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.txtYeniTekrar);
            this.Controls.Add(this.lblTekrar);
            this.Controls.Add(this.txtYeni);
            this.Controls.Add(this.lblYeni);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtTc);
            this.Controls.Add(this.lblTc);
            this.Controls.Add(this.lblBaslik);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SifremiUnuttumForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Şifremi unuttum";
            this.Load += new System.EventHandler(this.SifremiUnuttumForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
