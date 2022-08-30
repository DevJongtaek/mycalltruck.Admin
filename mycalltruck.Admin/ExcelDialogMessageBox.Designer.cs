namespace mycalltruck.Admin
{
    partial class ExcelDialogMessageBox
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn1 = new System.Windows.Forms.Button();
            this.btnTax = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 63);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 63);
            this.label1.TabIndex = 1;
            this.label1.Text = "내보내기 “양식”을 선택해 주십시오.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn1
            // 
            this.btn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn1.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btn1.FlatAppearance.BorderSize = 0;
            this.btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn1.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btn1.ForeColor = System.Drawing.Color.White;
            this.btn1.Location = new System.Drawing.Point(34, 73);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(84, 27);
            this.btn1.TabIndex = 1;
            this.btn1.Text = "전체정보";
            this.btn1.UseVisualStyleBackColor = false;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btnTax
            // 
            this.btnTax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnTax.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnTax.FlatAppearance.BorderSize = 0;
            this.btnTax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTax.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnTax.ForeColor = System.Drawing.Color.White;
            this.btnTax.Location = new System.Drawing.Point(152, 73);
            this.btnTax.Name = "btnTax";
            this.btnTax.Size = new System.Drawing.Size(84, 27);
            this.btnTax.TabIndex = 2;
            this.btnTax.Text = "배차정보";
            this.btnTax.UseVisualStyleBackColor = false;
            this.btnTax.Click += new System.EventHandler(this.btnTax_Click);
            // 
            // ExcelDialogMessageBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(275, 110);
            this.Controls.Add(this.btnTax);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExcelDialogMessageBox";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "내보내기 선택";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrintDialogMessageBox_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btnTax;
    }
}