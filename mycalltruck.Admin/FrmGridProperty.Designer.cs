namespace mycalltruck.Admin
{
    partial class FrmGridProperty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGridProperty));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.lbxCol = new System.Windows.Forms.CheckedListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbForeColor = new System.Windows.Forms.ComboBox();
            this.cmbBackColor = new System.Windows.Forms.ComboBox();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.cmbFont = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnDefault);
            this.panel1.Controls.Add(this.lbxCol);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(200, 298);
            this.panel1.TabIndex = 0;
            // 
            // btnUp
            // 
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(164, 11);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 31);
            this.btnUp.TabIndex = 13;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(164, 57);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 31);
            this.btnDown.TabIndex = 14;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnDefault.FlatAppearance.BorderSize = 0;
            this.btnDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefault.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDefault.ForeColor = System.Drawing.Color.White;
            this.btnDefault.Location = new System.Drawing.Point(17, 263);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 27);
            this.btnDefault.TabIndex = 17;
            this.btnDefault.Text = "기본설정";
            this.btnDefault.UseVisualStyleBackColor = false;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // lbxCol
            // 
            this.lbxCol.FormattingEnabled = true;
            this.lbxCol.Location = new System.Drawing.Point(7, 11);
            this.lbxCol.Name = "lbxCol";
            this.lbxCol.ScrollAlwaysVisible = true;
            this.lbxCol.Size = new System.Drawing.Size(153, 244);
            this.lbxCol.TabIndex = 12;
            this.lbxCol.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbxCol_ItemCheck);
            this.lbxCol.SelectedIndexChanged += new System.EventHandler(this.lbxCol_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(98, 263);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "확인";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.cmbForeColor);
            this.groupBox1.Controls.Add(this.cmbBackColor);
            this.groupBox1.Controls.Add(this.chkBold);
            this.groupBox1.Controls.Add(this.cmbFont);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numFontSize);
            this.groupBox1.Controls.Add(this.txtPreview);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(164, 94);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(24, 104);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "열 속성 변경";
            this.groupBox1.Visible = false;
            // 
            // cmbForeColor
            // 
            this.cmbForeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbForeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbForeColor.FormattingEnabled = true;
            this.cmbForeColor.Items.AddRange(new object[] {
            "하얀색",
            "검정색",
            "빨강색",
            "주황색",
            "노랑색",
            "연한 녹색",
            "녹색",
            "연한 파랑색",
            "파랑색",
            "보라색"});
            this.cmbForeColor.Location = new System.Drawing.Point(96, 57);
            this.cmbForeColor.Name = "cmbForeColor";
            this.cmbForeColor.Size = new System.Drawing.Size(139, 24);
            this.cmbForeColor.TabIndex = 18;
            this.cmbForeColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbForeColor_DrawItem);
            this.cmbForeColor.SelectedIndexChanged += new System.EventHandler(this.cmbForeColor_SelectedIndexChanged);
            // 
            // cmbBackColor
            // 
            this.cmbBackColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBackColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBackColor.FormattingEnabled = true;
            this.cmbBackColor.Items.AddRange(new object[] {
            "하얀색",
            "검정색",
            "빨강색",
            "주황색",
            "노랑색",
            "연한 녹색",
            "녹색",
            "연한 파랑색",
            "파랑색",
            "보라색"});
            this.cmbBackColor.Location = new System.Drawing.Point(96, 30);
            this.cmbBackColor.Name = "cmbBackColor";
            this.cmbBackColor.Size = new System.Drawing.Size(139, 24);
            this.cmbBackColor.TabIndex = 17;
            this.cmbBackColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbBackColor_DrawItem);
            this.cmbBackColor.SelectedIndexChanged += new System.EventHandler(this.cmbBackColor_SelectedIndexChanged);
            // 
            // chkBold
            // 
            this.chkBold.AutoSize = true;
            this.chkBold.Location = new System.Drawing.Point(242, 86);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(62, 19);
            this.chkBold.TabIndex = 16;
            this.chkBold.Text = "진하게";
            this.chkBold.UseVisualStyleBackColor = true;
            // 
            // cmbFont
            // 
            this.cmbFont.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFont.FormattingEnabled = true;
            this.cmbFont.Location = new System.Drawing.Point(96, 83);
            this.cmbFont.Name = "cmbFont";
            this.cmbFont.Size = new System.Drawing.Size(139, 24);
            this.cmbFont.TabIndex = 15;
            this.cmbFont.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbFont_DrawItem);
            this.cmbFont.SelectedIndexChanged += new System.EventHandler(this.cmbFont_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 25);
            this.label6.TabIndex = 10;
            this.label6.Text = "텍스트 맞춤 :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Visible = false;
            // 
            // numFontSize
            // 
            this.numFontSize.Location = new System.Drawing.Point(96, 106);
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(68, 23);
            this.numFontSize.TabIndex = 9;
            this.numFontSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numFontSize.Visible = false;
            this.numFontSize.ValueChanged += new System.EventHandler(this.numFontSize_ValueChanged);
            // 
            // txtPreview
            // 
            this.txtPreview.Location = new System.Drawing.Point(96, 133);
            this.txtPreview.Multiline = true;
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.Size = new System.Drawing.Size(242, 96);
            this.txtPreview.TabIndex = 5;
            this.txtPreview.Text = "미리보기";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 25);
            this.label5.TabIndex = 4;
            this.label5.Text = "미리보기 :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "글꼴 크기 :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "글꼴 :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "글자색 :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "배경색 :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmGridProperty
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(200, 298);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGridProperty";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "보기설정";
            this.Load += new System.EventHandler(this.FrmGridProperty_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.CheckedListBox lbxCol;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbForeColor;
        private System.Windows.Forms.ComboBox cmbBackColor;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.ComboBox cmbFont;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numFontSize;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}