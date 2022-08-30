namespace mycalltruck.Admin
{
    partial class SendMailDialogMessageBox
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
            this.btn1 = new System.Windows.Forms.Button();
            this.btnTax = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn1
            // 
            this.btn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn1.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btn1.FlatAppearance.BorderSize = 0;
            this.btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn1.ForeColor = System.Drawing.Color.White;
            this.btn1.Location = new System.Drawing.Point(30, 9);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(84, 27);
            this.btn1.TabIndex = 1;
            this.btn1.Text = "거래명세서";
            this.btn1.UseVisualStyleBackColor = false;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btnTax
            // 
            this.btnTax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnTax.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnTax.FlatAppearance.BorderSize = 0;
            this.btnTax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTax.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTax.ForeColor = System.Drawing.Color.White;
            this.btnTax.Location = new System.Drawing.Point(148, 9);
            this.btnTax.Name = "btnTax";
            this.btnTax.Size = new System.Drawing.Size(111, 27);
            this.btnTax.TabIndex = 2;
            this.btnTax.Text = "Nice세금계산서";
            this.btnTax.UseVisualStyleBackColor = false;
            this.btnTax.Click += new System.EventHandler(this.btnTax_Click);
            // 
            // SendMailDialogMessageBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(275, 48);
            this.Controls.Add(this.btnTax);
            this.Controls.Add(this.btn1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendMailDialogMessageBox";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "메일전송";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SendMailDialogMessageBox_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btnTax;
    }
}