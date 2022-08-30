namespace mycalltruck.Admin
{
    partial class FRMMNNOTICEDRIVER_ADD
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.btn_DriverExcel = new System.Windows.Forms.Button();
            this.btnExcelImport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb_CarSize = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CreateDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_CarType = new System.Windows.Forms.ComboBox();
            this.txt_MobileNo = new System.Windows.Forms.MaskedTextBox();
            this.cmb_AddressCity = new System.Windows.Forms.ComboBox();
            this.cmb_AddressState = new System.Windows.Forms.ComboBox();
            this.txt_ClientCode = new System.Windows.Forms.TextBox();
            this.txt_ClientName = new System.Windows.Forms.TextBox();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cmDataSet = new mycalltruck.Admin.CMDataSet();
            this.noticedriverTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.NOTICEDRIVERTableAdapter();
            this.clientsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ClientsTableAdapter();
            this.dataGridView1 = new mycalltruck.Admin.NewDGV();
            this.driverchk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rowNUMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gubun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.pnProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // btn_DriverExcel
            // 
            this.btn_DriverExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DriverExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_DriverExcel.FlatAppearance.BorderSize = 0;
            this.btn_DriverExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DriverExcel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_DriverExcel.ForeColor = System.Drawing.Color.White;
            this.btn_DriverExcel.Location = new System.Drawing.Point(106, 150);
            this.btn_DriverExcel.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btn_DriverExcel.Name = "btn_DriverExcel";
            this.btn_DriverExcel.Size = new System.Drawing.Size(96, 27);
            this.btn_DriverExcel.TabIndex = 61;
            this.btn_DriverExcel.TabStop = false;
            this.btn_DriverExcel.Text = "엑셀양식";
            this.btn_DriverExcel.UseVisualStyleBackColor = false;
            this.btn_DriverExcel.Click += new System.EventHandler(this.btn_DriverExcel_Click);
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcelImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnExcelImport.FlatAppearance.BorderSize = 0;
            this.btnExcelImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcelImport.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcelImport.ForeColor = System.Drawing.Color.White;
            this.btnExcelImport.Location = new System.Drawing.Point(7, 150);
            this.btnExcelImport.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(96, 27);
            this.btnExcelImport.TabIndex = 60;
            this.btnExcelImport.TabStop = false;
            this.btnExcelImport.Text = "엑셀일괄등록";
            this.btnExcelImport.UseVisualStyleBackColor = false;
            this.btnExcelImport.Click += new System.EventHandler(this.btnExcelImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(914, 150);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 27);
            this.btnClose.TabIndex = 57;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.groupBox2.Size = new System.Drawing.Size(1015, 145);
            this.groupBox2.TabIndex = 55;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmb_CarSize, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label7, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txt_CreateDate, 5, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmb_CarType, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.txt_MobileNo, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmb_AddressCity, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmb_AddressState, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_ClientCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_ClientName, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1013, 143);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label5.Location = new System.Drawing.Point(253, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 25);
            this.label5.TabIndex = 48;
            this.label5.Text = "운송사명";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label4.Location = new System.Drawing.Point(5, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 25);
            this.label4.TabIndex = 47;
            this.label4.Text = "운송사코드";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label2.Location = new System.Drawing.Point(5, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "시도";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_CarSize
            // 
            this.cmb_CarSize.DisplayMember = "StaticOptionId";
            this.cmb_CarSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_CarSize.FormattingEnabled = true;
            this.cmb_CarSize.Location = new System.Drawing.Point(92, 70);
            this.cmb_CarSize.Name = "cmb_CarSize";
            this.cmb_CarSize.Size = new System.Drawing.Size(154, 25);
            this.cmb_CarSize.TabIndex = 46;
            this.cmb_CarSize.TabStop = false;
            this.cmb_CarSize.ValueMember = "StaticOptionId";
            this.cmb_CarSize.SelectedIndexChanged += new System.EventHandler(this.cmb_CarSize_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label7.Location = new System.Drawing.Point(253, 38);
            this.label7.Margin = new System.Windows.Forms.Padding(4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "시군구";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label8.Location = new System.Drawing.Point(501, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "핸드폰번호";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label9.Location = new System.Drawing.Point(253, 71);
            this.label9.Margin = new System.Windows.Forms.Padding(4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 25);
            this.label9.TabIndex = 41;
            this.label9.Text = "차종";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label1.Location = new System.Drawing.Point(5, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 42;
            this.label1.Text = "톤수";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_CreateDate
            // 
            this.txt_CreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CreateDate.Location = new System.Drawing.Point(588, 70);
            this.txt_CreateDate.Name = "txt_CreateDate";
            this.txt_CreateDate.ReadOnly = true;
            this.txt_CreateDate.Size = new System.Drawing.Size(154, 25);
            this.txt_CreateDate.TabIndex = 27;
            this.txt_CreateDate.TabStop = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label3.Location = new System.Drawing.Point(501, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 25);
            this.label3.TabIndex = 39;
            this.label3.Text = "등록일자";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_CarType
            // 
            this.cmb_CarType.DisplayMember = "StaticOptionId";
            this.cmb_CarType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_CarType.FormattingEnabled = true;
            this.cmb_CarType.Items.AddRange(new object[] {
            "라보",
            "다마스",
            "카고",
            "윙",
            "일반탑",
            "축",
            "리프트",
            "냉장",
            "냉동",
            "냉동/냉장"});
            this.cmb_CarType.Location = new System.Drawing.Point(340, 70);
            this.cmb_CarType.Name = "cmb_CarType";
            this.cmb_CarType.Size = new System.Drawing.Size(154, 25);
            this.cmb_CarType.TabIndex = 45;
            this.cmb_CarType.TabStop = false;
            this.cmb_CarType.ValueMember = "StaticOptionId";
            // 
            // txt_MobileNo
            // 
            this.txt_MobileNo.Location = new System.Drawing.Point(588, 37);
            this.txt_MobileNo.Mask = "999-0009-0000";
            this.txt_MobileNo.Name = "txt_MobileNo";
            this.txt_MobileNo.Size = new System.Drawing.Size(111, 25);
            this.txt_MobileNo.TabIndex = 4;
            // 
            // cmb_AddressCity
            // 
            this.cmb_AddressCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_AddressCity.Enabled = false;
            this.cmb_AddressCity.FormattingEnabled = true;
            this.cmb_AddressCity.Location = new System.Drawing.Point(340, 37);
            this.cmb_AddressCity.Name = "cmb_AddressCity";
            this.cmb_AddressCity.Size = new System.Drawing.Size(154, 25);
            this.cmb_AddressCity.TabIndex = 44;
            this.cmb_AddressCity.TabStop = false;
            // 
            // cmb_AddressState
            // 
            this.cmb_AddressState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_AddressState.FormattingEnabled = true;
            this.cmb_AddressState.Location = new System.Drawing.Point(92, 37);
            this.cmb_AddressState.Name = "cmb_AddressState";
            this.cmb_AddressState.Size = new System.Drawing.Size(154, 25);
            this.cmb_AddressState.TabIndex = 43;
            this.cmb_AddressState.TabStop = false;
            this.cmb_AddressState.SelectedIndexChanged += new System.EventHandler(this.cmb_AddressState_SelectedIndexChanged);
            // 
            // txt_ClientCode
            // 
            this.txt_ClientCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_ClientCode.Location = new System.Drawing.Point(92, 4);
            this.txt_ClientCode.Name = "txt_ClientCode";
            this.txt_ClientCode.ReadOnly = true;
            this.txt_ClientCode.Size = new System.Drawing.Size(154, 25);
            this.txt_ClientCode.TabIndex = 49;
            this.txt_ClientCode.TabStop = false;
            // 
            // txt_ClientName
            // 
            this.txt_ClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_ClientName.Location = new System.Drawing.Point(340, 4);
            this.txt_ClientName.Name = "txt_ClientName";
            this.txt_ClientName.ReadOnly = true;
            this.txt_ClientName.Size = new System.Drawing.Size(154, 25);
            this.txt_ClientName.TabIndex = 50;
            this.txt_ClientName.TabStop = false;
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnAddClose.FlatAppearance.BorderSize = 0;
            this.btnAddClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddClose.ForeColor = System.Drawing.Color.White;
            this.btnAddClose.Location = new System.Drawing.Point(814, 150);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 27);
            this.btnAddClose.TabIndex = 58;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기(F6)";
            this.btnAddClose.UseVisualStyleBackColor = false;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(711, 150);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 27);
            this.btnAdd.TabIndex = 56;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "등록후추가(F5)";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cmDataSet
            // 
            this.cmDataSet.DataSetName = "CMDataSet";
            this.cmDataSet.EnforceConstraints = false;
            this.cmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // noticedriverTableAdapter
            // 
            this.noticedriverTableAdapter.ClearBeforeFill = true;
            // 
            // clientsTableAdapter
            // 
            this.clientsTableAdapter.ClearBeforeFill = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.driverchk,
            this.rowNUMDataGridViewTextBoxColumn,
            this.ClientCode,
            this.ClientName,
            this.Gubun,
            this.Column2});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(226, 153);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(215, 22);
            this.dataGridView1.TabIndex = 62;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.Visible = false;
            // 
            // driverchk
            // 
            this.driverchk.DataPropertyName = "CheckBox";
            this.driverchk.HeaderText = "";
            this.driverchk.Name = "driverchk";
            this.driverchk.Width = 20;
            // 
            // rowNUMDataGridViewTextBoxColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rowNUMDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.rowNUMDataGridViewTextBoxColumn.HeaderText = "번호";
            this.rowNUMDataGridViewTextBoxColumn.Name = "rowNUMDataGridViewTextBoxColumn";
            this.rowNUMDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rowNUMDataGridViewTextBoxColumn.Width = 46;
            // 
            // ClientCode
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ClientCode.DefaultCellStyle = dataGridViewCellStyle4;
            this.ClientCode.HeaderText = "운송사코드";
            this.ClientCode.Name = "ClientCode";
            this.ClientCode.Width = 88;
            // 
            // ClientName
            // 
            this.ClientName.HeaderText = "운송사명";
            this.ClientName.Name = "ClientName";
            this.ClientName.Width = 150;
            // 
            // Gubun
            // 
            this.Gubun.HeaderText = "구분";
            this.Gubun.Name = "Gubun";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "장기";
            this.Column2.Name = "Column2";
            this.Column2.Visible = false;
            this.Column2.Width = 50;
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.pnProgress.Location = new System.Drawing.Point(407, 57);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 65;
            this.pnProgress.Visible = false;
            // 
            // label66
            // 
            this.label66.BackColor = System.Drawing.Color.Transparent;
            this.label66.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label66.Location = new System.Drawing.Point(10, 10);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(180, 21);
            this.label66.TabIndex = 3;
            this.label66.Text = "잠시만 기다려 주십시오.";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bar
            // 
            this.bar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar.Location = new System.Drawing.Point(10, 31);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(180, 23);
            this.bar.Step = 1;
            this.bar.TabIndex = 2;
            // 
            // FRMMNNOTICEDRIVER_ADD
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1015, 184);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_DriverExcel);
            this.Controls.Add(this.btnExcelImport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "FRMMNNOTICEDRIVER_ADD";
            this.Text = "카톡배차 차량관리추가";
            this.Load += new System.EventHandler(this.FRMMNNOTICEDRIVER_ADD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.pnProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CMDataSet cmDataSet;
        private System.Windows.Forms.ErrorProvider err;
        private NewDGV dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn driverchk;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNUMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gubun;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.Button btn_DriverExcel;
        private System.Windows.Forms.Button btnExcelImport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel groupBox2;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox txt_MobileNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CreateDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_AddressState;
        private System.Windows.Forms.ComboBox cmb_AddressCity;
        private System.Windows.Forms.ComboBox cmb_CarType;
        private System.Windows.Forms.ComboBox cmb_CarSize;
        private CMDataSetTableAdapters.NOTICEDRIVERTableAdapter noticedriverTableAdapter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_ClientCode;
        private System.Windows.Forms.TextBox txt_ClientName;
        private CMDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
    }
}