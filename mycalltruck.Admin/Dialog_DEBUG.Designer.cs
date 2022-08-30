namespace mycalltruck.Admin
{
    partial class Dialog_DEBUG
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
            this.CARDPAY = new System.Windows.Forms.Button();
            this.CARDPAY_HY = new System.Windows.Forms.Button();
            this.Btn_Beta = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CARDPAY
            // 
            this.CARDPAY.Location = new System.Drawing.Point(12, 12);
            this.CARDPAY.Name = "CARDPAY";
            this.CARDPAY.Size = new System.Drawing.Size(260, 23);
            this.CARDPAY.TabIndex = 0;
            this.CARDPAY.Text = "CARDPAY";
            this.CARDPAY.UseVisualStyleBackColor = true;
            this.CARDPAY.Click += new System.EventHandler(this.CARDPAY_Click);
            // 
            // CARDPAY_HY
            // 
            this.CARDPAY_HY.Location = new System.Drawing.Point(12, 41);
            this.CARDPAY_HY.Name = "CARDPAY_HY";
            this.CARDPAY_HY.Size = new System.Drawing.Size(260, 23);
            this.CARDPAY_HY.TabIndex = 1;
            this.CARDPAY_HY.Text = "CARDPAY_HY";
            this.CARDPAY_HY.UseVisualStyleBackColor = true;
            this.CARDPAY_HY.Click += new System.EventHandler(this.CARDPAY_HY_Click);
            // 
            // Btn_Beta
            // 
            this.Btn_Beta.Location = new System.Drawing.Point(12, 70);
            this.Btn_Beta.Name = "Btn_Beta";
            this.Btn_Beta.Size = new System.Drawing.Size(260, 23);
            this.Btn_Beta.TabIndex = 2;
            this.Btn_Beta.Text = "BETA";
            this.Btn_Beta.UseVisualStyleBackColor = true;
            this.Btn_Beta.Click += new System.EventHandler(this.Btn_Beta_Click);
            // 
            // Dialog_DEBUG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Btn_Beta);
            this.Controls.Add(this.CARDPAY_HY);
            this.Controls.Add(this.CARDPAY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Dialog_DEBUG";
            this.Text = "Dialog_DEBUG";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CARDPAY;
        private System.Windows.Forms.Button CARDPAY_HY;
        private System.Windows.Forms.Button Btn_Beta;
    }
}