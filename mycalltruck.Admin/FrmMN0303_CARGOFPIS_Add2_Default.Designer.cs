namespace mycalltruck.Admin
{
    partial class FrmMN0303_CARGOFPIS_Add2_Default
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lbl_ReMoney = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_SignMoney = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_name = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.panel28 = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.txt_TRU_DEPOSIT = new System.Windows.Forms.TextBox();
            this.panel27 = new System.Windows.Forms.Panel();
            this.txt_TRU_COMP_BSNS_NUM = new System.Windows.Forms.MaskedTextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.cmb_TRU_MANG_TYPE = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_TRU_CONT_TO = new System.Windows.Forms.DateTimePicker();
            this.dtp_TRU_CONT_FROM = new System.Windows.Forms.DateTimePicker();
            this.txt_TRU_COMP_NM = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtEndZip = new System.Windows.Forms.TextBox();
            this.txtStartZip = new System.Windows.Forms.TextBox();
            this.clientUsersTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ClientUsersTableAdapter();
            this.cmDataSet = new mycalltruck.Admin.CMDataSet();
            this.fpiS_TRUTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.FPIS_TRUTableAdapter();
            this.fpiS_CONTTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.FPIS_CONTTableAdapter();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.panel28.SuspendLayout();
            this.panel27.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(652, 208);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 15;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddClose.Location = new System.Drawing.Point(555, 208);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 23);
            this.btnAddClose.TabIndex = 14;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기(F6)";
            this.btnAddClose.UseVisualStyleBackColor = true;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(456, 208);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 23);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "등록후추가(F5)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Location = new System.Drawing.Point(0, -1);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(749, 203);
            this.groupBox2.TabIndex = 10;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(1, 1);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(747, 201);
            this.panel4.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.12903F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.12903F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.12903F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.6129F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(747, 201);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label3);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 64);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(747, 32);
            this.panel8.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "■ 화물위탁(주는)정보";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.lbl_ReMoney);
            this.panel7.Controls.Add(this.label10);
            this.panel7.Controls.Add(this.lbl_SignMoney);
            this.panel7.Controls.Add(this.label8);
            this.panel7.Controls.Add(this.lbl_Date);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.lbl_name);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 35);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(741, 26);
            this.panel7.TabIndex = 2;
            // 
            // lbl_ReMoney
            // 
            this.lbl_ReMoney.AutoSize = true;
            this.lbl_ReMoney.ForeColor = System.Drawing.Color.Red;
            this.lbl_ReMoney.Location = new System.Drawing.Point(631, 4);
            this.lbl_ReMoney.Name = "lbl_ReMoney";
            this.lbl_ReMoney.Size = new System.Drawing.Size(11, 12);
            this.lbl_ReMoney.TabIndex = 7;
            this.lbl_ReMoney.Text = "-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(556, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "미운송금액 : ";
            // 
            // lbl_SignMoney
            // 
            this.lbl_SignMoney.AutoSize = true;
            this.lbl_SignMoney.ForeColor = System.Drawing.Color.Red;
            this.lbl_SignMoney.Location = new System.Drawing.Point(452, 4);
            this.lbl_SignMoney.Name = "lbl_SignMoney";
            this.lbl_SignMoney.Size = new System.Drawing.Size(11, 12);
            this.lbl_SignMoney.TabIndex = 5;
            this.lbl_SignMoney.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(390, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "계약금액 : ";
            // 
            // lbl_Date
            // 
            this.lbl_Date.AutoSize = true;
            this.lbl_Date.ForeColor = System.Drawing.Color.Red;
            this.lbl_Date.Location = new System.Drawing.Point(285, 4);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(11, 12);
            this.lbl_Date.TabIndex = 3;
            this.lbl_Date.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(230, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "계약일 : ";
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.ForeColor = System.Drawing.Color.Red;
            this.lbl_name.Location = new System.Drawing.Point(87, 5);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(11, 12);
            this.lbl_name.TabIndex = 1;
            this.lbl_name.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "상호/사업자 : ";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel10);
            this.panel1.Location = new System.Drawing.Point(3, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(741, 99);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 4;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 305F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.Controls.Add(this.txt_TRU_COMP_NM, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.panel27, 3, 0);
            this.tableLayoutPanel10.Controls.Add(this.label41, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.label30, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.label26, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.panel28, 3, 1);
            this.tableLayoutPanel10.Controls.Add(this.label29, 2, 1);
            this.tableLayoutPanel10.Controls.Add(this.label39, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.cmb_TRU_MANG_TYPE, 1, 2);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel10.RowCount = 3;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66669F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(739, 98);
            this.tableLayoutPanel10.TabIndex = 4;
            // 
            // panel28
            // 
            this.panel28.Controls.Add(this.label23);
            this.panel28.Controls.Add(this.txt_TRU_DEPOSIT);
            this.panel28.Location = new System.Drawing.Point(441, 33);
            this.panel28.Margin = new System.Windows.Forms.Padding(0);
            this.panel28.Name = "panel28";
            this.panel28.Size = new System.Drawing.Size(305, 31);
            this.panel28.TabIndex = 8;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(168, 9);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(27, 12);
            this.label23.TabIndex = 8;
            this.label23.Text = "(원)";
            // 
            // txt_TRU_DEPOSIT
            // 
            this.txt_TRU_DEPOSIT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_TRU_DEPOSIT.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txt_TRU_DEPOSIT.Location = new System.Drawing.Point(4, 4);
            this.txt_TRU_DEPOSIT.MaxLength = 10;
            this.txt_TRU_DEPOSIT.Name = "txt_TRU_DEPOSIT";
            this.txt_TRU_DEPOSIT.Size = new System.Drawing.Size(146, 21);
            this.txt_TRU_DEPOSIT.TabIndex = 3;
            // 
            // panel27
            // 
            this.panel27.Controls.Add(this.txt_TRU_COMP_BSNS_NUM);
            this.panel27.Controls.Add(this.label20);
            this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel27.Location = new System.Drawing.Point(441, 1);
            this.panel27.Margin = new System.Windows.Forms.Padding(0);
            this.panel27.Name = "panel27";
            this.panel27.Size = new System.Drawing.Size(305, 32);
            this.panel27.TabIndex = 2;
            // 
            // txt_TRU_COMP_BSNS_NUM
            // 
            this.txt_TRU_COMP_BSNS_NUM.Location = new System.Drawing.Point(4, 4);
            this.txt_TRU_COMP_BSNS_NUM.Mask = "999-99-99999";
            this.txt_TRU_COMP_BSNS_NUM.Name = "txt_TRU_COMP_BSNS_NUM";
            this.txt_TRU_COMP_BSNS_NUM.Size = new System.Drawing.Size(146, 21);
            this.txt_TRU_COMP_BSNS_NUM.TabIndex = 2;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(151, 7);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(57, 12);
            this.label20.TabIndex = 8;
            this.label20.Text = "(\'-\' 없이)";
            // 
            // label26
            // 
            this.label26.ForeColor = System.Drawing.Color.MediumBlue;
            this.label26.Location = new System.Drawing.Point(4, 33);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(114, 31);
            this.label26.TabIndex = 15;
            this.label26.Text = "계약기간 :";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label29
            // 
            this.label29.ForeColor = System.Drawing.Color.MediumBlue;
            this.label29.Location = new System.Drawing.Point(324, 33);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(114, 31);
            this.label29.TabIndex = 12;
            this.label29.Text = "위탁계약금액 :";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            this.label30.ForeColor = System.Drawing.Color.MediumBlue;
            this.label30.Location = new System.Drawing.Point(4, 1);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(114, 31);
            this.label30.TabIndex = 11;
            this.label30.Text = "상호/사업자 :";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label39
            // 
            this.label39.ForeColor = System.Drawing.Color.MediumBlue;
            this.label39.Location = new System.Drawing.Point(4, 64);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(114, 31);
            this.label39.TabIndex = 2;
            this.label39.Text = "정보망 이용 :";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label41
            // 
            this.label41.ForeColor = System.Drawing.Color.MediumBlue;
            this.label41.Location = new System.Drawing.Point(324, 1);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(114, 31);
            this.label41.TabIndex = 0;
            this.label41.Text = "사업자등록번호 :";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_TRU_MANG_TYPE
            // 
            this.cmb_TRU_MANG_TYPE.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmb_TRU_MANG_TYPE.DisplayMember = "StaticOptionId";
            this.cmb_TRU_MANG_TYPE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_TRU_MANG_TYPE.FormattingEnabled = true;
            this.cmb_TRU_MANG_TYPE.ItemHeight = 12;
            this.cmb_TRU_MANG_TYPE.Location = new System.Drawing.Point(124, 70);
            this.cmb_TRU_MANG_TYPE.Name = "cmb_TRU_MANG_TYPE";
            this.cmb_TRU_MANG_TYPE.Size = new System.Drawing.Size(194, 20);
            this.cmb_TRU_MANG_TYPE.TabIndex = 6;
            this.cmb_TRU_MANG_TYPE.TabStop = false;
            this.cmb_TRU_MANG_TYPE.ValueMember = "StaticOptionId";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.dtp_TRU_CONT_TO);
            this.panel2.Controls.Add(this.dtp_TRU_CONT_FROM);
            this.panel2.Location = new System.Drawing.Point(121, 33);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 31);
            this.panel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 408;
            this.label1.Text = "~";
            // 
            // dtp_TRU_CONT_TO
            // 
            this.dtp_TRU_CONT_TO.CustomFormat = "yyyy/MM/dd";
            this.dtp_TRU_CONT_TO.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_TRU_CONT_TO.Location = new System.Drawing.Point(100, 4);
            this.dtp_TRU_CONT_TO.Name = "dtp_TRU_CONT_TO";
            this.dtp_TRU_CONT_TO.Size = new System.Drawing.Size(83, 21);
            this.dtp_TRU_CONT_TO.TabIndex = 2;
            this.dtp_TRU_CONT_TO.TabStop = false;
            this.dtp_TRU_CONT_TO.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            // 
            // dtp_TRU_CONT_FROM
            // 
            this.dtp_TRU_CONT_FROM.CustomFormat = "yyyy/MM/dd";
            this.dtp_TRU_CONT_FROM.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_TRU_CONT_FROM.Location = new System.Drawing.Point(4, 4);
            this.dtp_TRU_CONT_FROM.Name = "dtp_TRU_CONT_FROM";
            this.dtp_TRU_CONT_FROM.Size = new System.Drawing.Size(83, 21);
            this.dtp_TRU_CONT_FROM.TabIndex = 1;
            this.dtp_TRU_CONT_FROM.TabStop = false;
            this.dtp_TRU_CONT_FROM.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            // 
            // txt_TRU_COMP_NM
            // 
            this.txt_TRU_COMP_NM.Location = new System.Drawing.Point(124, 4);
            this.txt_TRU_COMP_NM.MaxLength = 15;
            this.txt_TRU_COMP_NM.Name = "txt_TRU_COMP_NM";
            this.txt_TRU_COMP_NM.Size = new System.Drawing.Size(180, 21);
            this.txt_TRU_COMP_NM.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(747, 32);
            this.panel6.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "■ 화물수탁(받은)정보";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtEndZip);
            this.panel5.Controls.Add(this.txtStartZip);
            this.panel5.Location = new System.Drawing.Point(4, 207);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(377, 18);
            this.panel5.TabIndex = 48;
            this.panel5.Visible = false;
            // 
            // txtEndZip
            // 
            this.txtEndZip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtEndZip.Location = new System.Drawing.Point(257, -1);
            this.txtEndZip.Name = "txtEndZip";
            this.txtEndZip.Size = new System.Drawing.Size(117, 21);
            this.txtEndZip.TabIndex = 39;
            // 
            // txtStartZip
            // 
            this.txtStartZip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtStartZip.Location = new System.Drawing.Point(4, -1);
            this.txtStartZip.Name = "txtStartZip";
            this.txtStartZip.Size = new System.Drawing.Size(121, 21);
            this.txtStartZip.TabIndex = 38;
            // 
            // clientUsersTableAdapter
            // 
            this.clientUsersTableAdapter.ClearBeforeFill = true;
            // 
            // cmDataSet
            // 
            this.cmDataSet.DataSetName = "CMDataSet";
            this.cmDataSet.EnforceConstraints = false;
            this.cmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fpiS_TRUTableAdapter
            // 
            this.fpiS_TRUTableAdapter.ClearBeforeFill = true;
            // 
            // fpiS_CONTTableAdapter
            // 
            this.fpiS_CONTTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMN0303_CARGOFPIS_Add2_Default
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(749, 236);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMN0303_CARGOFPIS_Add2_Default";
            this.ShowIcon = false;
            this.Text = "위탁정보 추가";
            this.Load += new System.EventHandler(this.FrmMN0207_CAROWNERMANAGE_Add_Load);
            this.groupBox2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            this.panel27.ResumeLayout(false);
            this.panel27.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtEndZip;
        private System.Windows.Forms.TextBox txtStartZip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Panel panel28;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txt_TRU_DEPOSIT;
        private System.Windows.Forms.Panel panel27;
        private System.Windows.Forms.MaskedTextBox txt_TRU_COMP_BSNS_NUM;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.ComboBox cmb_TRU_MANG_TYPE;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_TRU_CONT_TO;
        private System.Windows.Forms.DateTimePicker dtp_TRU_CONT_FROM;
        private System.Windows.Forms.TextBox txt_TRU_COMP_NM;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lbl_ReMoney;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_SignMoney;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label2;
        private CMDataSetTableAdapters.ClientUsersTableAdapter clientUsersTableAdapter;
        private CMDataSet cmDataSet;
        private CMDataSetTableAdapters.FPIS_TRUTableAdapter fpiS_TRUTableAdapter;
        private CMDataSetTableAdapters.FPIS_CONTTableAdapter fpiS_CONTTableAdapter;
    }
}