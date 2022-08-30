namespace mycalltruck.Admin
{
    partial class FrmMN0204_CARGOOWNERMANAGE_Add
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
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txt_Zip = new System.Windows.Forms.TextBox();
            this.btnFindZip = new System.Windows.Forms.Button();
            this.txt_City = new System.Windows.Forms.TextBox();
            this.txt_State = new System.Windows.Forms.TextBox();
            this.txt_Street = new System.Windows.Forms.TextBox();
            this.panel16 = new System.Windows.Forms.Panel();
            this.label54 = new System.Windows.Forms.Label();
            this.txt_CEOBirth = new System.Windows.Forms.MaskedTextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.cmb_status = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cmb_Admin = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Upjong = new System.Windows.Forms.TextBox();
            this.txt_Uptae = new System.Windows.Forms.TextBox();
            this.txt_CEO = new System.Windows.Forms.TextBox();
            this.txt_BizNo = new System.Windows.Forms.MaskedTextBox();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.txt_Code = new System.Windows.Forms.TextBox();
            this.txt_Email = new System.Windows.Forms.TextBox();
            this.txt_PhoneNo = new System.Windows.Forms.MaskedTextBox();
            this.txt_MobileNo = new System.Windows.Forms.MaskedTextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.txt_LoginId = new System.Windows.Forms.TextBox();
            this.cmb_BizType = new System.Windows.Forms.ComboBox();
            this.txt_AccountOwner = new System.Windows.Forms.TextBox();
            this.txt_CreateDate = new System.Windows.Forms.TextBox();
            this.txt_AccountNo = new System.Windows.Forms.TextBox();
            this.cmb_Bank = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_LGD_MID = new System.Windows.Forms.TextBox();
            this.cmb_PG = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cmb_YN = new System.Windows.Forms.ComboBox();
            this.txt_FaxNo = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtp_ServiceDate = new System.Windows.Forms.DateTimePicker();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.txt_Result = new System.Windows.Forms.TextBox();
            this.cmDataSet = new mycalltruck.Admin.CMDataSet();
            this.clientsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ClientsTableAdapter();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(910, 247);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 23;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddClose.Location = new System.Drawing.Point(810, 247);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 23);
            this.btnAddClose.TabIndex = 22;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기(F6)";
            this.btnAddClose.UseVisualStyleBackColor = true;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(707, 247);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 23);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "등록후추가(F5)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(1015, 241);
            this.groupBox2.TabIndex = 13;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel16, 5, 4);
            this.tableLayoutPanel2.Controls.Add(this.label22, 4, 7);
            this.tableLayoutPanel2.Controls.Add(this.cmb_status, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label21, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.cmb_Admin, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.label11, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.label9, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label13, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label8, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Upjong, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Uptae, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_CEO, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_BizNo, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Name, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Code, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Email, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_PhoneNo, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.txt_MobileNo, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.txt_Password, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_LoginId, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cmb_BizType, 3, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_AccountOwner, 5, 6);
            this.tableLayoutPanel2.Controls.Add(this.txt_CreateDate, 5, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_AccountNo, 3, 6);
            this.tableLayoutPanel2.Controls.Add(this.cmb_Bank, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label15, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.label19, 4, 6);
            this.tableLayoutPanel2.Controls.Add(this.label14, 4, 5);
            this.tableLayoutPanel2.Controls.Add(this.label20, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 5, 7);
            this.tableLayoutPanel2.Controls.Add(this.label23, 2, 7);
            this.tableLayoutPanel2.Controls.Add(this.cmb_YN, 3, 7);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2853F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2853F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2853F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2853F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2853F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28816F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1013, 239);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel3, 5);
            this.panel3.Controls.Add(this.txt_Zip);
            this.panel3.Controls.Add(this.btnFindZip);
            this.panel3.Controls.Add(this.txt_City);
            this.panel3.Controls.Add(this.txt_State);
            this.panel3.Controls.Add(this.txt_Street);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(121, 61);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(891, 30);
            this.panel3.TabIndex = 413;
            // 
            // txt_Zip
            // 
            this.txt_Zip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Zip.Location = new System.Drawing.Point(4, 5);
            this.txt_Zip.MaxLength = 20;
            this.txt_Zip.Name = "txt_Zip";
            this.txt_Zip.ReadOnly = true;
            this.txt_Zip.Size = new System.Drawing.Size(66, 21);
            this.txt_Zip.TabIndex = 17;
            this.txt_Zip.TabStop = false;
            // 
            // btnFindZip
            // 
            this.btnFindZip.Location = new System.Drawing.Point(601, 4);
            this.btnFindZip.Name = "btnFindZip";
            this.btnFindZip.Size = new System.Drawing.Size(108, 23);
            this.btnFindZip.TabIndex = 16;
            this.btnFindZip.TabStop = false;
            this.btnFindZip.Text = "우편번호 검색";
            this.btnFindZip.UseVisualStyleBackColor = false;
            this.btnFindZip.Click += new System.EventHandler(this.btnFindZip_Click);
            // 
            // txt_City
            // 
            this.txt_City.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_City.Location = new System.Drawing.Point(179, 5);
            this.txt_City.MaxLength = 20;
            this.txt_City.Name = "txt_City";
            this.txt_City.ReadOnly = true;
            this.txt_City.Size = new System.Drawing.Size(99, 21);
            this.txt_City.TabIndex = 15;
            this.txt_City.TabStop = false;
            // 
            // txt_State
            // 
            this.txt_State.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_State.Location = new System.Drawing.Point(76, 5);
            this.txt_State.MaxLength = 20;
            this.txt_State.Name = "txt_State";
            this.txt_State.ReadOnly = true;
            this.txt_State.Size = new System.Drawing.Size(99, 21);
            this.txt_State.TabIndex = 14;
            this.txt_State.TabStop = false;
            // 
            // txt_Street
            // 
            this.txt_Street.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Street.Location = new System.Drawing.Point(284, 5);
            this.txt_Street.MaxLength = 100;
            this.txt_Street.Name = "txt_Street";
            this.txt_Street.Size = new System.Drawing.Size(311, 21);
            this.txt_Street.TabIndex = 11;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.label54);
            this.panel16.Controls.Add(this.txt_CEOBirth);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel16.Location = new System.Drawing.Point(686, 121);
            this.panel16.Margin = new System.Windows.Forms.Padding(0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(326, 30);
            this.panel16.TabIndex = 406;
            this.panel16.Visible = false;
            // 
            // label54
            // 
            this.label54.ForeColor = System.Drawing.Color.Red;
            this.label54.Location = new System.Drawing.Point(60, 1);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(70, 23);
            this.label54.TabIndex = 56;
            this.label54.Text = "(YYMMDD)";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_CEOBirth
            // 
            this.txt_CEOBirth.Location = new System.Drawing.Point(4, 2);
            this.txt_CEOBirth.Mask = "009090";
            this.txt_CEOBirth.Name = "txt_CEOBirth";
            this.txt_CEOBirth.Size = new System.Drawing.Size(50, 21);
            this.txt_CEOBirth.TabIndex = 55;
            // 
            // label22
            // 
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Location = new System.Drawing.Point(564, 212);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(119, 26);
            this.label22.TabIndex = 402;
            this.label22.Text = "PG社/가맹점ID :";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_status
            // 
            this.cmb_status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_status.FormattingEnabled = true;
            this.cmb_status.ItemHeight = 12;
            this.cmb_status.Location = new System.Drawing.Point(124, 215);
            this.cmb_status.Name = "cmb_status";
            this.cmb_status.Size = new System.Drawing.Size(153, 20);
            this.cmb_status.TabIndex = 53;
            // 
            // label21
            // 
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Location = new System.Drawing.Point(4, 212);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(114, 26);
            this.label21.TabIndex = 52;
            this.label21.Text = "서비스유형 :";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_Admin
            // 
            this.cmb_Admin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Admin.FormattingEnabled = true;
            this.cmb_Admin.Location = new System.Drawing.Point(689, 94);
            this.cmb_Admin.Name = "cmb_Admin";
            this.cmb_Admin.Size = new System.Drawing.Size(108, 20);
            this.cmb_Admin.TabIndex = 50;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(564, 121);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 30);
            this.label11.TabIndex = 42;
            this.label11.Text = "대표자 생년월일(*) :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label11.Visible = false;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(284, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 30);
            this.label9.TabIndex = 41;
            this.label9.Text = "전화번호(*) :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(4, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 30);
            this.label3.TabIndex = 39;
            this.label3.Text = "아이디(*) :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 30);
            this.label5.TabIndex = 36;
            this.label5.Text = "주소(*) :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(4, 1);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(114, 30);
            this.label18.TabIndex = 34;
            this.label18.Text = "운송사코드(*) :";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(4, 151);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(114, 30);
            this.label16.TabIndex = 15;
            this.label16.Text = "e-메일 :";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(284, 91);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 30);
            this.label13.TabIndex = 12;
            this.label13.Text = "비밀번호(*) :";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(4, 31);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(114, 30);
            this.label12.TabIndex = 11;
            this.label12.Text = "대표자(*) :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(4, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 30);
            this.label10.TabIndex = 9;
            this.label10.Text = "핸드폰번호 :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(564, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 30);
            this.label8.TabIndex = 7;
            this.label8.Text = "종목(*) :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(564, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 30);
            this.label7.TabIndex = 6;
            this.label7.Text = "사업자번호(*) :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(564, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "영업담당자 :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(284, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "업태(*) :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(284, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "운송사명(*) :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Upjong
            // 
            this.txt_Upjong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Upjong.Location = new System.Drawing.Point(689, 34);
            this.txt_Upjong.MaxLength = 30;
            this.txt_Upjong.Name = "txt_Upjong";
            this.txt_Upjong.Size = new System.Drawing.Size(154, 21);
            this.txt_Upjong.TabIndex = 5;
            // 
            // txt_Uptae
            // 
            this.txt_Uptae.Location = new System.Drawing.Point(404, 34);
            this.txt_Uptae.MaxLength = 30;
            this.txt_Uptae.Name = "txt_Uptae";
            this.txt_Uptae.Size = new System.Drawing.Size(154, 21);
            this.txt_Uptae.TabIndex = 4;
            // 
            // txt_CEO
            // 
            this.txt_CEO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CEO.Location = new System.Drawing.Point(124, 34);
            this.txt_CEO.MaxLength = 20;
            this.txt_CEO.Name = "txt_CEO";
            this.txt_CEO.Size = new System.Drawing.Size(154, 21);
            this.txt_CEO.TabIndex = 3;
            // 
            // txt_BizNo
            // 
            this.txt_BizNo.Location = new System.Drawing.Point(689, 4);
            this.txt_BizNo.Mask = "999-99-99999";
            this.txt_BizNo.Name = "txt_BizNo";
            this.txt_BizNo.Size = new System.Drawing.Size(87, 21);
            this.txt_BizNo.TabIndex = 2;
            // 
            // txt_Name
            // 
            this.txt_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Name.Location = new System.Drawing.Point(404, 4);
            this.txt_Name.MaxLength = 30;
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(154, 21);
            this.txt_Name.TabIndex = 1;
            // 
            // txt_Code
            // 
            this.txt_Code.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Code.Location = new System.Drawing.Point(124, 4);
            this.txt_Code.Name = "txt_Code";
            this.txt_Code.ReadOnly = true;
            this.txt_Code.Size = new System.Drawing.Size(154, 21);
            this.txt_Code.TabIndex = 0;
            // 
            // txt_Email
            // 
            this.txt_Email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Email.Location = new System.Drawing.Point(124, 154);
            this.txt_Email.MaxLength = 50;
            this.txt_Email.Name = "txt_Email";
            this.txt_Email.Size = new System.Drawing.Size(154, 21);
            this.txt_Email.TabIndex = 16;
            // 
            // txt_PhoneNo
            // 
            this.txt_PhoneNo.Location = new System.Drawing.Point(404, 124);
            this.txt_PhoneNo.Mask = "999-0009-0000";
            this.txt_PhoneNo.Name = "txt_PhoneNo";
            this.txt_PhoneNo.Size = new System.Drawing.Size(111, 21);
            this.txt_PhoneNo.TabIndex = 11;
            // 
            // txt_MobileNo
            // 
            this.txt_MobileNo.Location = new System.Drawing.Point(124, 124);
            this.txt_MobileNo.Mask = "999-0009-0000";
            this.txt_MobileNo.Name = "txt_MobileNo";
            this.txt_MobileNo.Size = new System.Drawing.Size(111, 21);
            this.txt_MobileNo.TabIndex = 10;
            // 
            // txt_Password
            // 
            this.txt_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Password.Location = new System.Drawing.Point(404, 94);
            this.txt_Password.MaxLength = 10;
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Size = new System.Drawing.Size(154, 21);
            this.txt_Password.TabIndex = 8;
            // 
            // txt_LoginId
            // 
            this.txt_LoginId.Location = new System.Drawing.Point(124, 94);
            this.txt_LoginId.MaxLength = 10;
            this.txt_LoginId.Name = "txt_LoginId";
            this.txt_LoginId.Size = new System.Drawing.Size(154, 21);
            this.txt_LoginId.TabIndex = 7;
            // 
            // cmb_BizType
            // 
            this.cmb_BizType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_BizType.FormattingEnabled = true;
            this.cmb_BizType.Location = new System.Drawing.Point(404, 154);
            this.cmb_BizType.Name = "cmb_BizType";
            this.cmb_BizType.Size = new System.Drawing.Size(108, 20);
            this.cmb_BizType.TabIndex = 17;
            // 
            // txt_AccountOwner
            // 
            this.txt_AccountOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_AccountOwner.Location = new System.Drawing.Point(689, 184);
            this.txt_AccountOwner.MaxLength = 20;
            this.txt_AccountOwner.Name = "txt_AccountOwner";
            this.txt_AccountOwner.Size = new System.Drawing.Size(154, 21);
            this.txt_AccountOwner.TabIndex = 20;
            // 
            // txt_CreateDate
            // 
            this.txt_CreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CreateDate.Location = new System.Drawing.Point(689, 154);
            this.txt_CreateDate.Name = "txt_CreateDate";
            this.txt_CreateDate.ReadOnly = true;
            this.txt_CreateDate.Size = new System.Drawing.Size(154, 21);
            this.txt_CreateDate.TabIndex = 27;
            this.txt_CreateDate.TabStop = false;
            // 
            // txt_AccountNo
            // 
            this.txt_AccountNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_AccountNo.Location = new System.Drawing.Point(404, 184);
            this.txt_AccountNo.MaxLength = 20;
            this.txt_AccountNo.Name = "txt_AccountNo";
            this.txt_AccountNo.Size = new System.Drawing.Size(154, 21);
            this.txt_AccountNo.TabIndex = 19;
            // 
            // cmb_Bank
            // 
            this.cmb_Bank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Bank.FormattingEnabled = true;
            this.cmb_Bank.ItemHeight = 12;
            this.cmb_Bank.Location = new System.Drawing.Point(124, 184);
            this.cmb_Bank.Name = "cmb_Bank";
            this.cmb_Bank.Size = new System.Drawing.Size(153, 20);
            this.cmb_Bank.TabIndex = 18;
            // 
            // label15
            // 
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(284, 151);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 30);
            this.label15.TabIndex = 14;
            this.label15.Text = "화주구분 :";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(564, 181);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(119, 31);
            this.label19.TabIndex = 39;
            this.label19.Text = "예금주 :";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(564, 151);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(119, 30);
            this.label14.TabIndex = 13;
            this.label14.Text = "등록일자 :";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Location = new System.Drawing.Point(284, 181);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(114, 31);
            this.label20.TabIndex = 49;
            this.label20.Text = "계좌번호 :";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(4, 181);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(114, 31);
            this.label17.TabIndex = 46;
            this.label17.Text = "은행명 :";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_LGD_MID);
            this.panel1.Controls.Add(this.cmb_PG);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(686, 212);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(326, 26);
            this.panel1.TabIndex = 403;
            // 
            // txt_LGD_MID
            // 
            this.txt_LGD_MID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_LGD_MID.Location = new System.Drawing.Point(92, 3);
            this.txt_LGD_MID.MaxLength = 20;
            this.txt_LGD_MID.Name = "txt_LGD_MID";
            this.txt_LGD_MID.ReadOnly = true;
            this.txt_LGD_MID.Size = new System.Drawing.Size(154, 21);
            this.txt_LGD_MID.TabIndex = 55;
            // 
            // cmb_PG
            // 
            this.cmb_PG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_PG.FormattingEnabled = true;
            this.cmb_PG.ItemHeight = 12;
            this.cmb_PG.Location = new System.Drawing.Point(4, 3);
            this.cmb_PG.Name = "cmb_PG";
            this.cmb_PG.Size = new System.Drawing.Size(86, 20);
            this.cmb_PG.TabIndex = 54;
            this.cmb_PG.SelectedIndexChanged += new System.EventHandler(this.cmb_PG_SelectedIndexChanged);
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(284, 212);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(114, 26);
            this.label23.TabIndex = 404;
            this.label23.Text = "차세로 SVC :";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_YN
            // 
            this.cmb_YN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_YN.FormattingEnabled = true;
            this.cmb_YN.ItemHeight = 12;
            this.cmb_YN.Location = new System.Drawing.Point(404, 215);
            this.cmb_YN.Name = "cmb_YN";
            this.cmb_YN.Size = new System.Drawing.Size(153, 20);
            this.cmb_YN.TabIndex = 405;
            // 
            // txt_FaxNo
            // 
            this.txt_FaxNo.Location = new System.Drawing.Point(586, 245);
            this.txt_FaxNo.Mask = "999-0009-0000";
            this.txt_FaxNo.Name = "txt_FaxNo";
            this.txt_FaxNo.Size = new System.Drawing.Size(111, 21);
            this.txt_FaxNo.TabIndex = 12;
            this.txt_FaxNo.Visible = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(273, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 26);
            this.label6.TabIndex = 51;
            this.label6.Text = "과금시작일 :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Visible = false;
            // 
            // dtp_ServiceDate
            // 
            this.dtp_ServiceDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_ServiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_ServiceDate.Location = new System.Drawing.Point(390, 246);
            this.dtp_ServiceDate.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.dtp_ServiceDate.Name = "dtp_ServiceDate";
            this.dtp_ServiceDate.Size = new System.Drawing.Size(97, 21);
            this.dtp_ServiceDate.TabIndex = 401;
            this.dtp_ServiceDate.TabStop = false;
            this.dtp_ServiceDate.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.dtp_ServiceDate.Visible = false;
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // txt_Result
            // 
            this.txt_Result.Location = new System.Drawing.Point(179, 246);
            this.txt_Result.Name = "txt_Result";
            this.txt_Result.Size = new System.Drawing.Size(100, 21);
            this.txt_Result.TabIndex = 402;
            this.txt_Result.Visible = false;
            // 
            // cmDataSet
            // 
            this.cmDataSet.DataSetName = "CMDataSet";
            this.cmDataSet.EnforceConstraints = false;
            this.cmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // clientsTableAdapter
            // 
            this.clientsTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMN0204_CARGOOWNERMANAGE_Add
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1015, 272);
            this.Controls.Add(this.txt_Result);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dtp_ServiceDate);
            this.Controls.Add(this.txt_FaxNo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMN0204_CARGOOWNERMANAGE_Add";
            this.Text = "운송주선관리 추가";
            this.Load += new System.EventHandler(this.FrmMN0204_CARGOOWNERMANAGE_Add_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMN0204_CARGOOWNERMANAGE_Add_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Upjong;
        private System.Windows.Forms.TextBox txt_Uptae;
        private System.Windows.Forms.TextBox txt_CEO;
        private System.Windows.Forms.MaskedTextBox txt_BizNo;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.TextBox txt_Code;
        private System.Windows.Forms.TextBox txt_LoginId;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.TextBox txt_Email;
        private System.Windows.Forms.ComboBox cmb_BizType;
        private System.Windows.Forms.TextBox txt_CreateDate;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.MaskedTextBox txt_MobileNo;
        private System.Windows.Forms.MaskedTextBox txt_PhoneNo;
        private System.Windows.Forms.MaskedTextBox txt_FaxNo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cmb_Bank;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txt_AccountNo;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txt_AccountOwner;
        private System.Windows.Forms.ComboBox cmb_Admin;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmb_status;
        private System.Windows.Forms.DateTimePicker dtp_ServiceDate;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_LGD_MID;
        private System.Windows.Forms.ComboBox cmb_PG;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cmb_YN;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.MaskedTextBox txt_CEOBirth;
        private System.Windows.Forms.TextBox txt_Result;
        private CMDataSet cmDataSet;
        private CMDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txt_Zip;
        private System.Windows.Forms.Button btnFindZip;
        private System.Windows.Forms.TextBox txt_City;
        private System.Windows.Forms.TextBox txt_State;
        private System.Windows.Forms.TextBox txt_Street;
    }
}