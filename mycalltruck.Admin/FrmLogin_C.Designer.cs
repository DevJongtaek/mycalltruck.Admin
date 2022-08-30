namespace mycalltruck.Admin
{
    partial class FrmLogin_C
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
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_Accept = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.MobileNo = new System.Windows.Forms.Label();
            this.SendSMS = new System.Windows.Forms.Button();
            this.Key = new System.Windows.Forms.TextBox();
            this.Timer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.clientsTableAdapter = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.ClientsTableAdapter();
            this.clientDataSet = new mycalltruck.Admin.DataSets.ClientDataSet();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // Info
            // 
            this.Info.AutoSize = true;
            this.Info.Location = new System.Drawing.Point(9, 15);
            this.Info.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(107, 15);
            this.Info.TabIndex = 0;
            this.Info.Text = "관리자 핸드폰번호";
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Close.FlatAppearance.BorderSize = 0;
            this.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Close.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Close.ForeColor = System.Drawing.Color.White;
            this.btn_Close.Location = new System.Drawing.Point(259, 99);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 27);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_Accept
            // 
            this.btn_Accept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Accept.Enabled = false;
            this.btn_Accept.FlatAppearance.BorderSize = 0;
            this.btn_Accept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Accept.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Accept.ForeColor = System.Drawing.Color.White;
            this.btn_Accept.Location = new System.Drawing.Point(178, 99);
            this.btn_Accept.Name = "btn_Accept";
            this.btn_Accept.Size = new System.Drawing.Size(75, 27);
            this.btn_Accept.TabIndex = 3;
            this.btn_Accept.Text = "확인";
            this.btn_Accept.UseVisualStyleBackColor = false;
            this.btn_Accept.Click += new System.EventHandler(this.btn_Accept_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(15, 83);
            this.lblError.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 15);
            this.lblError.TabIndex = 4;
            // 
            // MobileNo
            // 
            this.MobileNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.MobileNo.Location = new System.Drawing.Point(9, 36);
            this.MobileNo.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.MobileNo.Name = "MobileNo";
            this.MobileNo.Size = new System.Drawing.Size(117, 23);
            this.MobileNo.TabIndex = 5;
            // 
            // SendSMS
            // 
            this.SendSMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.SendSMS.FlatAppearance.BorderSize = 0;
            this.SendSMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendSMS.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SendSMS.ForeColor = System.Drawing.Color.White;
            this.SendSMS.Location = new System.Drawing.Point(119, 35);
            this.SendSMS.Name = "SendSMS";
            this.SendSMS.Size = new System.Drawing.Size(100, 27);
            this.SendSMS.TabIndex = 6;
            this.SendSMS.Text = "인증번호 요청";
            this.SendSMS.UseVisualStyleBackColor = false;
            this.SendSMS.Click += new System.EventHandler(this.SendSMS_Click);
            // 
            // Key
            // 
            this.Key.Enabled = false;
            this.Key.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Key.Location = new System.Drawing.Point(11, 64);
            this.Key.MaxLength = 4;
            this.Key.Name = "Key";
            this.Key.Size = new System.Drawing.Size(51, 25);
            this.Key.TabIndex = 7;
            this.Key.TextChanged += new System.EventHandler(this.Key_TextChanged);
            this.Key.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Key_KeyPress);
            // 
            // Timer
            // 
            this.Timer.Enabled = false;
            this.Timer.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Timer.Location = new System.Drawing.Point(288, 64);
            this.Timer.Name = "Timer";
            this.Timer.Size = new System.Drawing.Size(46, 25);
            this.Timer.TabIndex = 8;
            this.Timer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Timer.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "인증번호를 입력해 주십시오.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(120, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "(\"인증번호 요청\" 클릭 하세요.)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "[3:00]";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clientsTableAdapter
            // 
            this.clientsTableAdapter.ClearBeforeFill = true;
            // 
            // clientDataSet
            // 
            this.clientDataSet.DataSetName = "ClientDataSet";
            this.clientDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(11, 104);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "인증번호가 틀립니다.";
            this.label4.Visible = false;
            // 
            // FrmLogin_C
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(343, 134);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Timer);
            this.Controls.Add(this.Key);
            this.Controls.Add(this.SendSMS);
            this.Controls.Add(this.MobileNo);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btn_Accept);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.Info);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmLogin_C";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "관리자 모바일 인증";
            this.Load += new System.EventHandler(this.FrmLogin_B_Load);
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Info;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Button btn_Accept;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label MobileNo;
        private System.Windows.Forms.Button SendSMS;
        private System.Windows.Forms.TextBox Key;
        private System.Windows.Forms.TextBox Timer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DataSets.ClientDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;
        private DataSets.ClientDataSet clientDataSet;
        private System.Windows.Forms.Label label4;
    }
}