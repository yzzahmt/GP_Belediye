namespace Belediye_Otomasyonu.Views
{
    partial class YeniSifreForm
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
            this.txtGelenKod = new System.Windows.Forms.TextBox();
            this.btnDogrula = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtGelenKod
            // 
            this.txtGelenKod.Location = new System.Drawing.Point(97, 137);
            this.txtGelenKod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGelenKod.Name = "txtGelenKod";
            this.txtGelenKod.Size = new System.Drawing.Size(122, 30);
            this.txtGelenKod.TabIndex = 0;
            // 
            // btnDogrula
            // 
            this.btnDogrula.Location = new System.Drawing.Point(97, 223);
            this.btnDogrula.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDogrula.Name = "btnDogrula";
            this.btnDogrula.Size = new System.Drawing.Size(123, 59);
            this.btnDogrula.TabIndex = 1;
            this.btnDogrula.Text = "Doğrula";
            this.btnDogrula.UseVisualStyleBackColor = true;
            this.btnDogrula.Click += new System.EventHandler(this.btnDogrula_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Lütfen mailinize gelen 6 haneli kodu giriniz.";
            // 
            // YeniSifreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 335);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDogrula);
            this.Controls.Add(this.txtGelenKod);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "YeniSifreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "YeniSifreForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGelenKod;
        private System.Windows.Forms.Button btnDogrula;
        private System.Windows.Forms.Label label1;
    }
}