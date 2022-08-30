namespace mycalltruck.Admin
{
    partial class FrmLogin_A_Test
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
            this.Info = new System.Windows.Forms.Label();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_Accept = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Info
            // 
            this.Info.AutoSize = true;
            this.Info.Location = new System.Drawing.Point(9, 15);
            this.Info.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(313, 12);
            this.Info.TabIndex = 0;
            this.Info.Text = "안전한 결제를 위해 비밀번호를 다시 한 번 입력해주세요.";
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(9, 36);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(3, 15, 3, 6);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.PasswordChar = '*';
            this.txt_Password.Size = new System.Drawing.Size(325, 21);
            this.txt_Password.TabIndex = 1;
            this.txt_Password.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Password_KeyUp);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(259, 83);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_Accept
            // 
            this.btn_Accept.Location = new System.Drawing.Point(178, 83);
            this.btn_Accept.Name = "btn_Accept";
            this.btn_Accept.Size = new System.Drawing.Size(75, 23);
            this.btn_Accept.TabIndex = 3;
            this.btn_Accept.Text = "확인";
            this.btn_Accept.UseVisualStyleBackColor = true;
            this.btn_Accept.Click += new System.EventHandler(this.btn_Accept_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(15, 65);
            this.lblError.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 12);
            this.lblError.TabIndex = 4;
            // 
            // FrmLogin_A
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(343, 111);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btn_Accept);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.Info);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmLogin_A";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "비밀번호 재확인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Info;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Button btn_Accept;
        private System.Windows.Forms.Label lblError;
    }
}