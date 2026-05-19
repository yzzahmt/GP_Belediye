namespace Belediye_Otomasyonu.Views
{
    partial class BasvuruYonetimForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvBasvurular;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnDurumDegistir;
        private System.Windows.Forms.ComboBox cmbDurum;
        private System.Windows.Forms.Button btnKapat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvBasvurular = new System.Windows.Forms.DataGridView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnDurumDegistir = new System.Windows.Forms.Button();
            this.cmbDurum = new System.Windows.Forms.ComboBox();
            this.btnKapat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBasvurular)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(16);
            this.panelTop.Size = new System.Drawing.Size(800, 60);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTitle.Location = new System.Drawing.Point(16, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Başvuru Yönetimi";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.cmbDurum);
            this.panelBottom.Controls.Add(this.btnDurumDegistir);
            this.panelBottom.Controls.Add(this.btnKapat);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 390);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(16);
            this.panelBottom.Size = new System.Drawing.Size(800, 60);
            this.panelBottom.TabIndex = 2;
            // 
            // cmbDurum
            // 
            this.cmbDurum.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbDurum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDurum.Location = new System.Drawing.Point(16, 16);
            this.cmbDurum.Name = "cmbDurum";
            this.cmbDurum.Size = new System.Drawing.Size(120, 23);
            this.cmbDurum.TabIndex = 2;
            // 
            // btnDurumDegistir
            // 
            this.btnDurumDegistir.Location = new System.Drawing.Point(146, 14);
            this.btnDurumDegistir.Name = "btnDurumDegistir";
            this.btnDurumDegistir.Size = new System.Drawing.Size(140, 28);
            this.btnDurumDegistir.TabIndex = 0;
            this.btnDurumDegistir.Text = "Seçili Durumu Uygula";
            this.btnDurumDegistir.UseVisualStyleBackColor = true;
            this.btnDurumDegistir.Click += new System.EventHandler(this.btnDurumDegistir_Click);
            // 
            // btnKapat
            // 
            this.btnKapat.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnKapat.Location = new System.Drawing.Point(684, 16);
            this.btnKapat.Name = "btnKapat";
            this.btnKapat.Size = new System.Drawing.Size(100, 28);
            this.btnKapat.TabIndex = 1;
            this.btnKapat.Text = "Kapat";
            this.btnKapat.UseVisualStyleBackColor = true;
            this.btnKapat.Click += new System.EventHandler(this.btnKapat_Click);
            // 
            // dgvBasvurular
            // 
            this.dgvBasvurular.AllowUserToAddRows = false;
            this.dgvBasvurular.AllowUserToDeleteRows = false;
            this.dgvBasvurular.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBasvurular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBasvurular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBasvurular.Location = new System.Drawing.Point(0, 60);
            this.dgvBasvurular.Name = "dgvBasvurular";
            this.dgvBasvurular.ReadOnly = true;
            this.dgvBasvurular.RowHeadersVisible = false;
            this.dgvBasvurular.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBasvurular.Size = new System.Drawing.Size(800, 330);
            this.dgvBasvurular.TabIndex = 1;
            // 
            // BasvuruYonetimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvBasvurular);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "BasvuruYonetimForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Başvuru Yönetimi";
            this.Load += new System.EventHandler(this.BasvuruYonetimForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBasvurular)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
