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
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnKodGonder = new System.Windows.Forms.Button();
            this.txtDogrulamaKodu = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblBaslik
            // 
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.Location = new System.Drawing.Point(32, 25);
            this.lblBaslik.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(340, 28);
            this.lblBaslik.TabIndex = 10;
            this.lblBaslik.Text = "Şifremi unuttum (vatandaş hesabı)";
            // 
            // lblTc
            // 
            this.lblTc.AutoSize = true;
            this.lblTc.Location = new System.Drawing.Point(32, 74);
            this.lblTc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTc.Name = "lblTc";
            this.lblTc.Size = new System.Drawing.Size(25, 16);
            this.lblTc.TabIndex = 9;
            this.lblTc.Text = "TC";
            // 
            // txtTc
            // 
            this.txtTc.Location = new System.Drawing.Point(32, 94);
            this.txtTc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTc.MaxLength = 11;
            this.txtTc.Name = "txtTc";
            this.txtTc.Size = new System.Drawing.Size(425, 22);
            this.txtTc.TabIndex = 0;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(32, 130);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(54, 16);
            this.lblEmail.TabIndex = 8;
            this.lblEmail.Text = "E-posta";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(32, 150);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(425, 22);
            this.txtEmail.TabIndex = 1;
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(37, 212);
            this.btnIptal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(160, 56);
            this.btnIptal.TabIndex = 5;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = false;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // btnKodGonder
            // 
            this.btnKodGonder.Location = new System.Drawing.Point(297, 212);
            this.btnKodGonder.Name = "btnKodGonder";
            this.btnKodGonder.Size = new System.Drawing.Size(138, 56);
            this.btnKodGonder.TabIndex = 11;
            this.btnKodGonder.Text = "Kod Gönder";
            this.btnKodGonder.UseVisualStyleBackColor = true;
            this.btnKodGonder.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtDogrulamaKodu
            // 
            this.txtDogrulamaKodu.Location = new System.Drawing.Point(326, 229);
            this.txtDogrulamaKodu.Name = "txtDogrulamaKodu";
            this.txtDogrulamaKodu.Size = new System.Drawing.Size(100, 22);
            this.txtDogrulamaKodu.TabIndex = 12;
            this.txtDogrulamaKodu.Visible = false;
            // 
            // SifremiUnuttumForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 331);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtTc);
            this.Controls.Add(this.lblTc);
            this.Controls.Add(this.lblBaslik);
            this.Controls.Add(this.btnKodGonder);
            this.Controls.Add(this.txtDogrulamaKodu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SifremiUnuttumForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Şifremi unuttum";
            this.Load += new System.EventHandler(this.SifremiUnuttumForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnKodGonder;
        private System.Windows.Forms.TextBox txtDogrulamaKodu;
    }
}
