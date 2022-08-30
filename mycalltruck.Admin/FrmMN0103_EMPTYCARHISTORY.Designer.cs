namespace mycalltruck.Admin
{
    partial class FrmMN0103_EMPTYCARHISTORY
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnButtons = new System.Windows.Forms.Panel();
            this.bPanel1 = new System.Windows.Forms.Panel();
            this.btn_New = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_CarNo = new System.Windows.Forms.TextBox();
            this.cmb_DriverSearchType = new System.Windows.Forms.ComboBox();
            this.cmb_NotificationGroup = new System.Windows.Forms.ComboBox();
            this.cmb_CarSize = new System.Windows.Forms.ComboBox();
            this.cmb_CarType = new System.Windows.Forms.ComboBox();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.pnFill = new System.Windows.Forms.Panel();
            this.pnGrid = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new mycalltruck.Admin.NewDGV();
            this.SeqColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarSizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsGroupColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DriverNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarYearColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParkColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RouteTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhoneNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemarkColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientAddressTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ClientAddressTableAdapter();
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnButtons.SuspendLayout();
            this.bPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnFill.SuspendLayout();
            this.pnGrid.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnButtons, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnFill, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1012, 616);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // pnButtons
            // 
            this.pnButtons.Controls.Add(this.bPanel1);
            this.pnButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnButtons.Location = new System.Drawing.Point(0, 0);
            this.pnButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new System.Drawing.Size(1012, 61);
            this.pnButtons.TabIndex = 0;
            // 
            // bPanel1
            // 
            this.bPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bPanel1.Controls.Add(this.btn_New);
            this.bPanel1.Controls.Add(this.btnClose);
            this.bPanel1.Controls.Add(this.btn_Search);
            this.bPanel1.Controls.Add(this.label9);
            this.bPanel1.Controls.Add(this.panel2);
            this.bPanel1.Controls.Add(this.panel1);
            this.bPanel1.Controls.Add(this.label6);
            this.bPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bPanel1.Location = new System.Drawing.Point(0, 0);
            this.bPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.bPanel1.Name = "bPanel1";
            this.bPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.bPanel1.Size = new System.Drawing.Size(1012, 61);
            this.bPanel1.TabIndex = 0;
            // 
            // btn_New
            // 
            this.btn_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_New.Location = new System.Drawing.Point(907, 11);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(55, 23);
            this.btn_New.TabIndex = 52;
            this.btn_New.Text = "초기화";
            this.btn_New.UseVisualStyleBackColor = true;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(961, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.btnClose.Size = new System.Drawing.Size(48, 23);
            this.btnClose.TabIndex = 51;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.Location = new System.Drawing.Point(855, 11);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(52, 23);
            this.btn_Search.TabIndex = 50;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(189, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 12);
            this.label9.TabIndex = 48;
            this.label9.Text = "예약:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Location = new System.Drawing.Point(256, 40);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2);
            this.panel2.Size = new System.Drawing.Size(752, 18);
            this.panel2.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(328, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 12);
            this.label2.TabIndex = 43;
            this.label2.Text = "[실기간 공차조회]";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(433, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 44;
            this.label3.Text = "와";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(450, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "[공차예고]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(515, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 46;
            this.label5.Text = "에서 예약";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(572, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 47;
            this.label7.Text = "체크";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Right;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(601, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 12);
            this.label8.TabIndex = 48;
            this.label8.Text = "한 건이 여기에 나타납니다";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_CarNo);
            this.panel1.Controls.Add(this.cmb_DriverSearchType);
            this.panel1.Controls.Add(this.cmb_NotificationGroup);
            this.panel1.Controls.Add(this.cmb_CarSize);
            this.panel1.Controls.Add(this.cmb_CarType);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpStart);
            this.panel1.Location = new System.Drawing.Point(225, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(626, 28);
            this.panel1.TabIndex = 44;
            // 
            // txt_CarNo
            // 
            this.txt_CarNo.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt_CarNo.Location = new System.Drawing.Point(519, 0);
            this.txt_CarNo.Name = "txt_CarNo";
            this.txt_CarNo.Size = new System.Drawing.Size(100, 21);
            this.txt_CarNo.TabIndex = 2;
            this.txt_CarNo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_CarNo_KeyUp);
            // 
            // cmb_DriverSearchType
            // 
            this.cmb_DriverSearchType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmb_DriverSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_DriverSearchType.FormattingEnabled = true;
            this.cmb_DriverSearchType.Location = new System.Drawing.Point(411, 0);
            this.cmb_DriverSearchType.Name = "cmb_DriverSearchType";
            this.cmb_DriverSearchType.Size = new System.Drawing.Size(108, 20);
            this.cmb_DriverSearchType.TabIndex = 1;
            // 
            // cmb_NotificationGroup
            // 
            this.cmb_NotificationGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmb_NotificationGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_NotificationGroup.FormattingEnabled = true;
            this.cmb_NotificationGroup.Location = new System.Drawing.Point(331, 0);
            this.cmb_NotificationGroup.Name = "cmb_NotificationGroup";
            this.cmb_NotificationGroup.Size = new System.Drawing.Size(80, 20);
            this.cmb_NotificationGroup.TabIndex = 6;
            // 
            // cmb_CarSize
            // 
            this.cmb_CarSize.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmb_CarSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_CarSize.FormattingEnabled = true;
            this.cmb_CarSize.Location = new System.Drawing.Point(256, 0);
            this.cmb_CarSize.Name = "cmb_CarSize";
            this.cmb_CarSize.Size = new System.Drawing.Size(75, 20);
            this.cmb_CarSize.TabIndex = 9;
            // 
            // cmb_CarType
            // 
            this.cmb_CarType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmb_CarType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_CarType.FormattingEnabled = true;
            this.cmb_CarType.Location = new System.Drawing.Point(184, 0);
            this.cmb_CarType.Name = "cmb_CarType";
            this.cmb_CarType.Size = new System.Drawing.Size(72, 20);
            this.cmb_CarType.TabIndex = 398;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy/MM/dd";
            this.dtpEnd.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(99, 0);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(85, 21);
            this.dtpEnd.TabIndex = 400;
            this.dtpEnd.TabStop = false;
            this.dtpEnd.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(85, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 28);
            this.label1.TabIndex = 401;
            this.label1.Text = "~";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy/MM/dd";
            this.dtpStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(0, 0);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(85, 21);
            this.dtpStart.TabIndex = 399;
            this.dtpStart.TabStop = false;
            this.dtpStart.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(4, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 16);
            this.label6.TabIndex = 18;
            this.label6.Text = "[배차예약 내역]";
            // 
            // pnFill
            // 
            this.pnFill.Controls.Add(this.pnGrid);
            this.pnFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnFill.Location = new System.Drawing.Point(0, 61);
            this.pnFill.Margin = new System.Windows.Forms.Padding(0);
            this.pnFill.Name = "pnFill";
            this.pnFill.Size = new System.Drawing.Size(1012, 555);
            this.pnFill.TabIndex = 1;
            // 
            // pnGrid
            // 
            this.pnGrid.Controls.Add(this.panel8);
            this.pnGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGrid.Location = new System.Drawing.Point(0, 0);
            this.pnGrid.Margin = new System.Windows.Forms.Padding(0);
            this.pnGrid.Name = "pnGrid";
            this.pnGrid.Size = new System.Drawing.Size(1012, 555);
            this.pnGrid.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.dataGridView1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1012, 555);
            this.panel8.TabIndex = 1;
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeqColumn,
            this.CreateTimeColumn,
            this.CarTypeColumn,
            this.CarSizeColumn,
            this.IsGroupColumn,
            this.DriverNameColumn,
            this.CarYearColumn,
            this.CarNoColumn,
            this.ParkColumn,
            this.RouteTypeColumn,
            this.Column1,
            this.PhoneNoColumn,
            this.RemarkColumn});
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1012, 555);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            // 
            // SeqColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SeqColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.SeqColumn.HeaderText = "번호";
            this.SeqColumn.Name = "SeqColumn";
            this.SeqColumn.ReadOnly = true;
            this.SeqColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SeqColumn.Width = 60;
            // 
            // CreateTimeColumn
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CreateTimeColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.CreateTimeColumn.HeaderText = "예약지정일시";
            this.CreateTimeColumn.Name = "CreateTimeColumn";
            this.CreateTimeColumn.ReadOnly = true;
            this.CreateTimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CreateTimeColumn.Width = 110;
            // 
            // CarTypeColumn
            // 
            this.CarTypeColumn.DataPropertyName = "CarType";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.CarTypeColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.CarTypeColumn.HeaderText = "차종";
            this.CarTypeColumn.Name = "CarTypeColumn";
            this.CarTypeColumn.ReadOnly = true;
            this.CarTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CarTypeColumn.Width = 60;
            // 
            // CarSizeColumn
            // 
            this.CarSizeColumn.DataPropertyName = "CarSize";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CarSizeColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.CarSizeColumn.HeaderText = "톤수(t)";
            this.CarSizeColumn.Name = "CarSizeColumn";
            this.CarSizeColumn.ReadOnly = true;
            this.CarSizeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CarSizeColumn.Width = 60;
            // 
            // IsGroupColumn
            // 
            this.IsGroupColumn.DataPropertyName = "GroupName";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IsGroupColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.IsGroupColumn.HeaderText = "그룹";
            this.IsGroupColumn.Name = "IsGroupColumn";
            this.IsGroupColumn.ReadOnly = true;
            this.IsGroupColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IsGroupColumn.Width = 60;
            // 
            // DriverNameColumn
            // 
            this.DriverNameColumn.DataPropertyName = "DriverName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.DriverNameColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.DriverNameColumn.HeaderText = "상호";
            this.DriverNameColumn.Name = "DriverNameColumn";
            this.DriverNameColumn.ReadOnly = true;
            this.DriverNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DriverNameColumn.Width = 150;
            // 
            // CarYearColumn
            // 
            this.CarYearColumn.DataPropertyName = "CarYear";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CarYearColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.CarYearColumn.HeaderText = "기사명";
            this.CarYearColumn.Name = "CarYearColumn";
            this.CarYearColumn.ReadOnly = true;
            this.CarYearColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CarNoColumn
            // 
            this.CarNoColumn.DataPropertyName = "CarNo";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.CarNoColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.CarNoColumn.HeaderText = "차량번호";
            this.CarNoColumn.Name = "CarNoColumn";
            this.CarNoColumn.ReadOnly = true;
            this.CarNoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ParkColumn
            // 
            this.ParkColumn.DataPropertyName = "Park";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ParkColumn.DefaultCellStyle = dataGridViewCellStyle11;
            this.ParkColumn.HeaderText = "차고지";
            this.ParkColumn.Name = "ParkColumn";
            this.ParkColumn.ReadOnly = true;
            this.ParkColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ParkColumn.Width = 200;
            // 
            // RouteTypeColumn
            // 
            this.RouteTypeColumn.DataPropertyName = "RouteType";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.RouteTypeColumn.DefaultCellStyle = dataGridViewCellStyle12;
            this.RouteTypeColumn.HeaderText = "운행노선";
            this.RouteTypeColumn.Name = "RouteTypeColumn";
            this.RouteTypeColumn.ReadOnly = true;
            this.RouteTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RouteTypeColumn.Width = 80;
            // 
            // Column1
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle13;
            this.Column1.HeaderText = "아이디";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // PhoneNoColumn
            // 
            this.PhoneNoColumn.DataPropertyName = "DriverMobileNo";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PhoneNoColumn.DefaultCellStyle = dataGridViewCellStyle14;
            this.PhoneNoColumn.HeaderText = "핸드폰번호";
            this.PhoneNoColumn.Name = "PhoneNoColumn";
            this.PhoneNoColumn.ReadOnly = true;
            this.PhoneNoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RemarkColumn
            // 
            this.RemarkColumn.DataPropertyName = "Remark";
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle15.Padding = new System.Windows.Forms.Padding(1);
            this.RemarkColumn.DefaultCellStyle = dataGridViewCellStyle15;
            this.RemarkColumn.HeaderText = "Memo";
            this.RemarkColumn.Name = "RemarkColumn";
            this.RemarkColumn.ReadOnly = true;
            this.RemarkColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RemarkColumn.Width = 300;
            // 
            // clientAddressTableAdapter
            // 
            this.clientAddressTableAdapter.ClearBeforeFill = true;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FrmMN0103_EMPTYCARHISTORY
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1012, 616);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmMN0103_EMPTYCARHISTORY";
            this.Text = "공차예고";
            this.Load += new System.EventHandler(this.FrmMN0103_EMPTYCARHISTORY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnButtons.ResumeLayout(false);
            this.bPanel1.ResumeLayout(false);
            this.bPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnFill.ResumeLayout(false);
            this.pnGrid.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnButtons;
        private System.Windows.Forms.Panel bPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnFill;
        private System.Windows.Forms.Panel pnGrid;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmb_CarSize;
        private System.Windows.Forms.ComboBox cmb_CarType;
        private System.Windows.Forms.ComboBox cmb_NotificationGroup;
        private System.Windows.Forms.TextBox txt_CarNo;
        private System.Windows.Forms.ComboBox cmb_DriverSearchType;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private NewDGV dataGridView1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_New;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarSizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsGroupColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DriverNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarYearColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParkColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RouteTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhoneNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemarkColumn;
        private CMDataSetTableAdapters.ClientAddressTableAdapter clientAddressTableAdapter;
        private CMDataSet cMDataSet;

    }
}