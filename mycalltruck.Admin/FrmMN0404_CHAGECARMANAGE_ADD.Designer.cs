namespace mycalltruck.Admin
{
    partial class FrmMN0404_CHAGECARMANAGE_ADD
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_InfoSearch_Clear = new System.Windows.Forms.Button();
            this.btn_InfoSearch = new System.Windows.Forms.Button();
            this.txt_InfoSearch = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.panel17 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driversInfo2BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cmb_Gubun = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtp_ApplyDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CustomerName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Item = new System.Windows.Forms.TextBox();
            this.txt_UnitPrice = new System.Windows.Forms.TextBox();
            this.chk_IssueGubun = new System.Windows.Forms.CheckBox();
            this.txt_BizNo = new System.Windows.Forms.MaskedTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chk_UseTax = new System.Windows.Forms.CheckBox();
            this.txt_Amount = new System.Windows.Forms.TextBox();
            this.driversCardBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txt_ChargeGubun = new System.Windows.Forms.TextBox();
            this.txt_DriverId = new System.Windows.Forms.TextBox();
            this.txt_Tax = new System.Windows.Forms.TextBox();
            this.chargesTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ChargesTableAdapter();
            this.chargeAccountsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ChargeAccountsTableAdapter();
            this.driversCardTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DriversCardTableAdapter();
            this.driversInfo2TableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DriversInfo2TableAdapter();
            this.panel17.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversInfo2BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.driversCardBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_InfoSearch_Clear
            // 
            this.btn_InfoSearch_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_InfoSearch_Clear.Location = new System.Drawing.Point(308, 3);
            this.btn_InfoSearch_Clear.Name = "btn_InfoSearch_Clear";
            this.btn_InfoSearch_Clear.Size = new System.Drawing.Size(53, 23);
            this.btn_InfoSearch_Clear.TabIndex = 40;
            this.btn_InfoSearch_Clear.Text = "초기화";
            this.btn_InfoSearch_Clear.UseVisualStyleBackColor = true;
            this.btn_InfoSearch_Clear.Click += new System.EventHandler(this.btn_InfoSearch_Clear_Click);
            // 
            // btn_InfoSearch
            // 
            this.btn_InfoSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_InfoSearch.Location = new System.Drawing.Point(256, 3);
            this.btn_InfoSearch.Name = "btn_InfoSearch";
            this.btn_InfoSearch.Size = new System.Drawing.Size(53, 23);
            this.btn_InfoSearch.TabIndex = 39;
            this.btn_InfoSearch.Text = "검 색";
            this.btn_InfoSearch.UseVisualStyleBackColor = true;
            this.btn_InfoSearch.Click += new System.EventHandler(this.btn_InfoSearch_Click);
            // 
            // txt_InfoSearch
            // 
            this.txt_InfoSearch.Location = new System.Drawing.Point(76, 4);
            this.txt_InfoSearch.Name = "txt_InfoSearch";
            this.txt_InfoSearch.Size = new System.Drawing.Size(174, 21);
            this.txt_InfoSearch.TabIndex = 1;
            this.txt_InfoSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_InfoSearch_KeyUp);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(29, 8);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(41, 12);
            this.label22.TabIndex = 0;
            this.label22.Text = "선택 : ";
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.btn_InfoSearch_Clear);
            this.panel17.Controls.Add(this.btn_InfoSearch);
            this.panel17.Controls.Add(this.txt_InfoSearch);
            this.panel17.Controls.Add(this.label22);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Margin = new System.Windows.Forms.Padding(0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(364, 30);
            this.panel17.TabIndex = 0;
            // 
            // panel4
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel4, 2);
            this.panel4.Controls.Add(this.tableLayoutPanel7);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(1, 1);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.tableLayoutPanel2.SetRowSpan(this.panel4, 7);
            this.panel4.Size = new System.Drawing.Size(364, 208);
            this.panel4.TabIndex = 44;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.newDGV1, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.panel17, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(364, 208);
            this.tableLayoutPanel7.TabIndex = 3;
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToDeleteRows = false;
            this.newDGV1.AllowUserToOrderColumns = true;
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.newDGV1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.newDGV1.AutoGenerateColumns = false;
            this.newDGV1.BackgroundColor = System.Drawing.Color.White;
            this.newDGV1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.newDGV1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newDGV1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.carNoDataGridViewTextBoxColumn,
            this.CarYear,
            this.driverIdDataGridViewTextBoxColumn,
            this.bizNoDataGridViewTextBoxColumn,
            this.cEODataGridViewTextBoxColumn});
            this.newDGV1.DataSource = this.driversInfo2BindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle3;
            this.newDGV1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(0, 30);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.ReadOnly = true;
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newDGV1.Size = new System.Drawing.Size(364, 178);
            this.newDGV1.TabIndex = 6;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "상호";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // carNoDataGridViewTextBoxColumn
            // 
            this.carNoDataGridViewTextBoxColumn.DataPropertyName = "CarNo";
            this.carNoDataGridViewTextBoxColumn.HeaderText = "차량번호";
            this.carNoDataGridViewTextBoxColumn.Name = "carNoDataGridViewTextBoxColumn";
            this.carNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // CarYear
            // 
            this.CarYear.DataPropertyName = "CarYear";
            this.CarYear.HeaderText = "기사명";
            this.CarYear.Name = "CarYear";
            this.CarYear.ReadOnly = true;
            // 
            // driverIdDataGridViewTextBoxColumn
            // 
            this.driverIdDataGridViewTextBoxColumn.DataPropertyName = "DriverId";
            this.driverIdDataGridViewTextBoxColumn.HeaderText = "DriverId";
            this.driverIdDataGridViewTextBoxColumn.Name = "driverIdDataGridViewTextBoxColumn";
            this.driverIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // bizNoDataGridViewTextBoxColumn
            // 
            this.bizNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            this.bizNoDataGridViewTextBoxColumn.HeaderText = "BizNo";
            this.bizNoDataGridViewTextBoxColumn.Name = "bizNoDataGridViewTextBoxColumn";
            this.bizNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.bizNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // cEODataGridViewTextBoxColumn
            // 
            this.cEODataGridViewTextBoxColumn.DataPropertyName = "CEO";
            this.cEODataGridViewTextBoxColumn.HeaderText = "CEO";
            this.cEODataGridViewTextBoxColumn.Name = "cEODataGridViewTextBoxColumn";
            this.cEODataGridViewTextBoxColumn.ReadOnly = true;
            this.cEODataGridViewTextBoxColumn.Visible = false;
            // 
            // driversInfo2BindingSource
            // 
            this.driversInfo2BindingSource.DataMember = "DriversInfo2";
            this.driversInfo2BindingSource.DataSource = this.cMDataSet;
            this.driversInfo2BindingSource.CurrentChanged += new System.EventHandler(this.driversInfo2BindingSource_CurrentChanged);
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(918, 220);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 49;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddClose.Location = new System.Drawing.Point(818, 220);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 23);
            this.btnAddClose.TabIndex = 50;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기(F6)";
            this.btnAddClose.UseVisualStyleBackColor = true;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(715, 220);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 23);
            this.btnAdd.TabIndex = 48;
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
            this.groupBox2.Size = new System.Drawing.Size(1015, 212);
            this.groupBox2.TabIndex = 47;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 256F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1013, 210);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(365, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel2.SetRowSpan(this.panel1, 7);
            this.panel1.Size = new System.Drawing.Size(647, 208);
            this.panel1.TabIndex = 45;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 194F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 291F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmb_Gubun, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtp_ApplyDate, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txt_CustomerName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txt_Item, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txt_UnitPrice, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.chk_IssueGubun, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txt_BizNo, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 3, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(647, 208);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(4, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 28);
            this.label8.TabIndex = 61;
            this.label8.Text = "발행구분 :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 28);
            this.label5.TabIndex = 58;
            this.label5.Text = "공급가액 :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(306, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 28);
            this.label2.TabIndex = 57;
            this.label2.Text = "사업자번호 :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(4, 1);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(102, 28);
            this.label18.TabIndex = 34;
            this.label18.Text = "전표구분 :";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_Gubun
            // 
            this.cmb_Gubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Gubun.FormattingEnabled = true;
            this.cmb_Gubun.Location = new System.Drawing.Point(112, 4);
            this.cmb_Gubun.Name = "cmb_Gubun";
            this.cmb_Gubun.Size = new System.Drawing.Size(154, 20);
            this.cmb_Gubun.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(306, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 28);
            this.label7.TabIndex = 6;
            this.label7.Text = "발행일자 :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtp_ApplyDate
            // 
            this.dtp_ApplyDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_ApplyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_ApplyDate.Location = new System.Drawing.Point(414, 4);
            this.dtp_ApplyDate.Name = "dtp_ApplyDate";
            this.dtp_ApplyDate.Size = new System.Drawing.Size(154, 21);
            this.dtp_ApplyDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 28);
            this.label1.TabIndex = 51;
            this.label1.Text = "거래처명 :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_CustomerName
            // 
            this.txt_CustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CustomerName.Location = new System.Drawing.Point(112, 32);
            this.txt_CustomerName.MaxLength = 5;
            this.txt_CustomerName.Name = "txt_CustomerName";
            this.txt_CustomerName.Size = new System.Drawing.Size(154, 21);
            this.txt_CustomerName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 28);
            this.label4.TabIndex = 56;
            this.label4.Text = "품명 :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(306, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "합계 :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Item
            // 
            this.txt_Item.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Item.Location = new System.Drawing.Point(112, 60);
            this.txt_Item.MaxLength = 10;
            this.txt_Item.Name = "txt_Item";
            this.txt_Item.Size = new System.Drawing.Size(154, 21);
            this.txt_Item.TabIndex = 5;
            // 
            // txt_UnitPrice
            // 
            this.txt_UnitPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_UnitPrice.Location = new System.Drawing.Point(112, 88);
            this.txt_UnitPrice.MaxLength = 10;
            this.txt_UnitPrice.Name = "txt_UnitPrice";
            this.txt_UnitPrice.Size = new System.Drawing.Size(154, 21);
            this.txt_UnitPrice.TabIndex = 6;
            this.txt_UnitPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_UnitPrice_KeyPress);
            this.txt_UnitPrice.Leave += new System.EventHandler(this.txt_UnitPrice_Leave);
            // 
            // chk_IssueGubun
            // 
            this.chk_IssueGubun.AutoSize = true;
            this.chk_IssueGubun.Location = new System.Drawing.Point(112, 116);
            this.chk_IssueGubun.Name = "chk_IssueGubun";
            this.chk_IssueGubun.Size = new System.Drawing.Size(156, 16);
            this.chk_IssueGubun.TabIndex = 7;
            this.chk_IssueGubun.Text = " 전자세금계산서 발행 건";
            this.chk_IssueGubun.UseVisualStyleBackColor = true;
            // 
            // txt_BizNo
            // 
            this.txt_BizNo.Location = new System.Drawing.Point(414, 32);
            this.txt_BizNo.Mask = "999-99-99999";
            this.txt_BizNo.Name = "txt_BizNo";
            this.txt_BizNo.Size = new System.Drawing.Size(154, 21);
            this.txt_BizNo.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chk_UseTax);
            this.panel3.Controls.Add(this.txt_Amount);
            this.panel3.Location = new System.Drawing.Point(411, 85);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(225, 28);
            this.panel3.TabIndex = 66;
            this.panel3.TabStop = true;
            // 
            // chk_UseTax
            // 
            this.chk_UseTax.AutoSize = true;
            this.chk_UseTax.Location = new System.Drawing.Point(161, 7);
            this.chk_UseTax.Name = "chk_UseTax";
            this.chk_UseTax.Size = new System.Drawing.Size(48, 16);
            this.chk_UseTax.TabIndex = 1;
            this.chk_UseTax.Text = "VAT";
            this.chk_UseTax.UseVisualStyleBackColor = true;
            this.chk_UseTax.CheckedChanged += new System.EventHandler(this.chk_UseTax_CheckedChanged);
            // 
            // txt_Amount
            // 
            this.txt_Amount.Location = new System.Drawing.Point(3, 3);
            this.txt_Amount.Name = "txt_Amount";
            this.txt_Amount.ReadOnly = true;
            this.txt_Amount.Size = new System.Drawing.Size(154, 21);
            this.txt_Amount.TabIndex = 7;
            // 
            // driversCardBindingSource
            // 
            this.driversCardBindingSource.DataMember = "DriversCard";
            this.driversCardBindingSource.DataSource = this.cMDataSet;
            // 
            // txt_ChargeGubun
            // 
            this.txt_ChargeGubun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_ChargeGubun.Location = new System.Drawing.Point(134, 220);
            this.txt_ChargeGubun.Name = "txt_ChargeGubun";
            this.txt_ChargeGubun.Size = new System.Drawing.Size(102, 21);
            this.txt_ChargeGubun.TabIndex = 52;
            this.txt_ChargeGubun.Visible = false;
            // 
            // txt_DriverId
            // 
            this.txt_DriverId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_DriverId.Location = new System.Drawing.Point(12, 220);
            this.txt_DriverId.Name = "txt_DriverId";
            this.txt_DriverId.Size = new System.Drawing.Size(102, 21);
            this.txt_DriverId.TabIndex = 51;
            this.txt_DriverId.Visible = false;
            // 
            // txt_Tax
            // 
            this.txt_Tax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Tax.Location = new System.Drawing.Point(258, 216);
            this.txt_Tax.Name = "txt_Tax";
            this.txt_Tax.Size = new System.Drawing.Size(102, 21);
            this.txt_Tax.TabIndex = 54;
            this.txt_Tax.Visible = false;
            // 
            // chargesTableAdapter
            // 
            this.chargesTableAdapter.ClearBeforeFill = true;
            // 
            // chargeAccountsTableAdapter
            // 
            this.chargeAccountsTableAdapter.ClearBeforeFill = true;
            // 
            // driversCardTableAdapter
            // 
            this.driversCardTableAdapter.ClearBeforeFill = true;
            // 
            // driversInfo2TableAdapter
            // 
            this.driversInfo2TableAdapter.ClearBeforeFill = true;
            // 
            // FrmMN0404_CHAGECARMANAGE_ADD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1015, 244);
            this.Controls.Add(this.txt_Tax);
            this.Controls.Add(this.txt_ChargeGubun);
            this.Controls.Add(this.txt_DriverId);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox2);
            this.Name = "FrmMN0404_CHAGECARMANAGE_ADD";
            this.Text = "경비출납 추가";
            this.Load += new System.EventHandler(this.FrmMN0404_CHAGECARMANAGE_ADD_Load);
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversInfo2BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.driversCardBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CMDataSet cMDataSet;
        private System.Windows.Forms.Button btn_InfoSearch_Clear;
        private System.Windows.Forms.Button btn_InfoSearch;
        private System.Windows.Forms.TextBox txt_InfoSearch;
        private System.Windows.Forms.Label label22;
        private CMDataSetTableAdapters.ChargesTableAdapter chargesTableAdapter;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private NewDGV newDGV1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarYear;
        private CMDataSetTableAdapters.ChargeAccountsTableAdapter chargeAccountsTableAdapter;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEODataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox txt_ChargeGubun;
        private System.Windows.Forms.TextBox txt_DriverId;
        private System.Windows.Forms.TextBox txt_Tax;
        private CMDataSetTableAdapters.DriversCardTableAdapter driversCardTableAdapter;
        private System.Windows.Forms.BindingSource driversCardBindingSource;
        private System.Windows.Forms.BindingSource driversInfo2BindingSource;
        private CMDataSetTableAdapters.DriversInfo2TableAdapter driversInfo2TableAdapter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cmb_Gubun;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtp_ApplyDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CustomerName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Item;
        private System.Windows.Forms.TextBox txt_UnitPrice;
        private System.Windows.Forms.CheckBox chk_IssueGubun;
        private System.Windows.Forms.MaskedTextBox txt_BizNo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chk_UseTax;
        private System.Windows.Forms.TextBox txt_Amount;
    }
}