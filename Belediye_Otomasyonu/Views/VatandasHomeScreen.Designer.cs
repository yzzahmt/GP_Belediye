namespace Belediye_Otomasyonu.Views
{
    partial class VatandasHomeScreen
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 680);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "VatandasHomeScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Belediye Vatandaş Portalı";
            this.Load += new System.EventHandler(this.VatandasHomeScreen_Load);
            this.ResumeLayout(false);
        }
    }
}
