namespace mycalltruck.Admin
{
    partial class FrmMN0208_DRIVERADDMANAGE_ADD
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label18 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Code = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_CarInfo = new System.Windows.Forms.Button();
            this.txt_SangHo = new System.Windows.Forms.TextBox();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.txt_PhoneNo = new System.Windows.Forms.MaskedTextBox();
            this.txt_MobileNo = new System.Windows.Forms.MaskedTextBox();
            this.dtp_InDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtp_OutDate = new System.Windows.Forms.DateTimePicker();
            this.txt_CreateDate = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txt_DriverId = new System.Windows.Forms.TextBox();
            this.btn_DriverExcel = new System.Windows.Forms.Button();
            this.btnExcelImport = new System.Windows.Forms.Button();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.dataGridView1 = new mycalltruck.Admin.NewDGV();
            this.driverchk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rowNUMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gubun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cmDataSet = new mycalltruck.Admin.CMDataSet();
            this.driverAddTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DriverAddTableAdapter();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).BeginInit();
            this.SuspendLayout();
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
            this.groupBox2.TabIndex = 11;
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
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Code, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Name, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_PhoneNo, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_MobileNo, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtp_InDate, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.dtp_OutDate, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_CreateDate, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1013, 143);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label18.Location = new System.Drawing.Point(5, 5);
            this.label18.Margin = new System.Windows.Forms.Padding(4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 25);
            this.label18.TabIndex = 34;
            this.label18.Text = "기사코드";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label8.Location = new System.Drawing.Point(5, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "전화번호";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label7.Location = new System.Drawing.Point(501, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "면허증번호";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label2.Location = new System.Drawing.Point(253, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "기사명";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Code
            // 
            this.txt_Code.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Code.Enabled = false;
            this.txt_Code.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Code.Location = new System.Drawing.Point(92, 4);
            this.txt_Code.Name = "txt_Code";
            this.txt_Code.ReadOnly = true;
            this.txt_Code.Size = new System.Drawing.Size(154, 25);
            this.txt_Code.TabIndex = 35;
            this.txt_Code.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_CarInfo);
            this.panel1.Controls.Add(this.txt_SangHo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(585, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 33);
            this.panel1.TabIndex = 2;
            // 
            // btn_CarInfo
            // 
            this.btn_CarInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_CarInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_CarInfo.FlatAppearance.BorderSize = 0;
            this.btn_CarInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CarInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_CarInfo.ForeColor = System.Drawing.Color.White;
            this.btn_CarInfo.Location = new System.Drawing.Point(163, 3);
            this.btn_CarInfo.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btn_CarInfo.Name = "btn_CarInfo";
            this.btn_CarInfo.Size = new System.Drawing.Size(65, 27);
            this.btn_CarInfo.TabIndex = 2;
            this.btn_CarInfo.TabStop = false;
            this.btn_CarInfo.Text = "찾기";
            this.btn_CarInfo.UseVisualStyleBackColor = false;
            this.btn_CarInfo.Visible = false;
            this.btn_CarInfo.Click += new System.EventHandler(this.btn_CarInfo_Click);
            // 
            // txt_SangHo
            // 
            this.txt_SangHo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_SangHo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_SangHo.Location = new System.Drawing.Point(3, 3);
            this.txt_SangHo.MaxLength = 30;
            this.txt_SangHo.Name = "txt_SangHo";
            this.txt_SangHo.Size = new System.Drawing.Size(154, 25);
            this.txt_SangHo.TabIndex = 1;
            // 
            // txt_Name
            // 
            this.txt_Name.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Name.Location = new System.Drawing.Point(340, 4);
            this.txt_Name.MaxLength = 30;
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(154, 25);
            this.txt_Name.TabIndex = 1;
            // 
            // txt_PhoneNo
            // 
            this.txt_PhoneNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_PhoneNo.Location = new System.Drawing.Point(92, 37);
            this.txt_PhoneNo.Mask = "999-0009-0000";
            this.txt_PhoneNo.Name = "txt_PhoneNo";
            this.txt_PhoneNo.Size = new System.Drawing.Size(111, 25);
            this.txt_PhoneNo.TabIndex = 3;
            // 
            // txt_MobileNo
            // 
            this.txt_MobileNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_MobileNo.Location = new System.Drawing.Point(340, 37);
            this.txt_MobileNo.Mask = "999-0009-0000";
            this.txt_MobileNo.Name = "txt_MobileNo";
            this.txt_MobileNo.Size = new System.Drawing.Size(111, 25);
            this.txt_MobileNo.TabIndex = 4;
            // 
            // dtp_InDate
            // 
            this.dtp_InDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_InDate.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dtp_InDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_InDate.Location = new System.Drawing.Point(588, 37);
            this.dtp_InDate.Name = "dtp_InDate";
            this.dtp_InDate.Size = new System.Drawing.Size(111, 25);
            this.dtp_InDate.TabIndex = 5;
            this.dtp_InDate.TabStop = false;
            this.dtp_InDate.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label5.Location = new System.Drawing.Point(253, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 25);
            this.label5.TabIndex = 36;
            this.label5.Text = "핸드폰번호";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label9.Location = new System.Drawing.Point(501, 38);
            this.label9.Margin = new System.Windows.Forms.Padding(4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 25);
            this.label9.TabIndex = 41;
            this.label9.Text = "입사일자";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(5, 104);
            this.label11.Margin = new System.Windows.Forms.Padding(4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 25);
            this.label11.TabIndex = 42;
            this.label11.Text = "퇴사일자";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label11.Visible = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label3.Location = new System.Drawing.Point(5, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 25);
            this.label3.TabIndex = 39;
            this.label3.Text = "등록일자";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtp_OutDate
            // 
            this.dtp_OutDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_OutDate.Enabled = false;
            this.dtp_OutDate.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dtp_OutDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_OutDate.Location = new System.Drawing.Point(92, 103);
            this.dtp_OutDate.Name = "dtp_OutDate";
            this.dtp_OutDate.Size = new System.Drawing.Size(111, 25);
            this.dtp_OutDate.TabIndex = 6;
            this.dtp_OutDate.TabStop = false;
            this.dtp_OutDate.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.dtp_OutDate.Visible = false;
            // 
            // txt_CreateDate
            // 
            this.txt_CreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CreateDate.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_CreateDate.Location = new System.Drawing.Point(92, 70);
            this.txt_CreateDate.Name = "txt_CreateDate";
            this.txt_CreateDate.ReadOnly = true;
            this.txt_CreateDate.Size = new System.Drawing.Size(154, 25);
            this.txt_CreateDate.TabIndex = 27;
            this.txt_CreateDate.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(918, 150);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 27);
            this.btnClose.TabIndex = 29;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnAddClose.FlatAppearance.BorderSize = 0;
            this.btnAddClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddClose.ForeColor = System.Drawing.Color.White;
            this.btnAddClose.Location = new System.Drawing.Point(818, 150);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 27);
            this.btnAddClose.TabIndex = 30;
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
            this.btnAdd.Location = new System.Drawing.Point(715, 150);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 27);
            this.btnAdd.TabIndex = 28;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "등록후추가(F5)";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txt_DriverId);
            this.panel2.Location = new System.Drawing.Point(505, 149);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 23);
            this.panel2.TabIndex = 31;
            this.panel2.Visible = false;
            // 
            // txt_DriverId
            // 
            this.txt_DriverId.Location = new System.Drawing.Point(6, 0);
            this.txt_DriverId.Name = "txt_DriverId";
            this.txt_DriverId.Size = new System.Drawing.Size(100, 23);
            this.txt_DriverId.TabIndex = 0;
            // 
            // btn_DriverExcel
            // 
            this.btn_DriverExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DriverExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_DriverExcel.FlatAppearance.BorderSize = 0;
            this.btn_DriverExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DriverExcel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_DriverExcel.ForeColor = System.Drawing.Color.White;
            this.btn_DriverExcel.Location = new System.Drawing.Point(108, 150);
            this.btn_DriverExcel.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btn_DriverExcel.Name = "btn_DriverExcel";
            this.btn_DriverExcel.Size = new System.Drawing.Size(96, 27);
            this.btn_DriverExcel.TabIndex = 52;
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
            this.btnExcelImport.Location = new System.Drawing.Point(9, 150);
            this.btnExcelImport.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(96, 27);
            this.btnExcelImport.TabIndex = 51;
            this.btnExcelImport.TabStop = false;
            this.btnExcelImport.Text = "엑셀일괄등록";
            this.btnExcelImport.UseVisualStyleBackColor = false;
            this.btnExcelImport.Click += new System.EventHandler(this.btnExcelImport_Click);
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(407, 57);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 54;
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
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.driverchk,
            this.rowNUMDataGridViewTextBoxColumn,
            this.ClientCode,
            this.ClientName,
            this.Gubun,
            this.Column2});
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle20;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(226, 149);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(215, 22);
            this.dataGridView1.TabIndex = 53;
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
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rowNUMDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle18;
            this.rowNUMDataGridViewTextBoxColumn.HeaderText = "번호";
            this.rowNUMDataGridViewTextBoxColumn.Name = "rowNUMDataGridViewTextBoxColumn";
            this.rowNUMDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rowNUMDataGridViewTextBoxColumn.Width = 46;
            // 
            // ClientCode
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ClientCode.DefaultCellStyle = dataGridViewCellStyle19;
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
            // cmDataSet
            // 
            this.cmDataSet.DataSetName = "CMDataSet";
            this.cmDataSet.EnforceConstraints = false;
            this.cmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // driverAddTableAdapter
            // 
            this.driverAddTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMN0208_DRIVERADDMANAGE_ADD
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1015, 186);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_DriverExcel);
            this.Controls.Add(this.btnExcelImport);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmMN0208_DRIVERADDMANAGE_ADD";
            this.Text = "기사정보추가";
            this.Load += new System.EventHandler(this.FrmMN0208_DRIVERADDMANAGE_ADD_Load);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnProgress.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.TextBox txt_Code;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox txt_MobileNo;
        private System.Windows.Forms.TextBox txt_CreateDate;
        private System.Windows.Forms.DateTimePicker dtp_InDate;
        private System.Windows.Forms.DateTimePicker dtp_OutDate;
        private System.Windows.Forms.MaskedTextBox txt_PhoneNo;
        private CMDataSet cmDataSet;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_CarInfo;
        private System.Windows.Forms.TextBox txt_SangHo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txt_DriverId;
        private NewDGV dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn driverchk;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNUMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gubun;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.Button btn_DriverExcel;
        private System.Windows.Forms.Button btnExcelImport;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
        private CMDataSetTableAdapters.DriverAddTableAdapter driverAddTableAdapter;
    }
}