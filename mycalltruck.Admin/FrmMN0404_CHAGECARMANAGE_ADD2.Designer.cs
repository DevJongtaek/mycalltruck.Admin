namespace mycalltruck.Admin
{
    partial class FrmMN0404_CHAGECARMANAGE_ADD2
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.chk_UseTax2 = new System.Windows.Forms.CheckBox();
            this.txt_Tax2 = new System.Windows.Forms.TextBox();
            this.txt_UnitPrice2 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_Num = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmb_Gubun2 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dtp_ApplyDate2 = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.cmb_ChargeAccountId = new System.Windows.Forms.ComboBox();
            this.txt_Remark = new System.Windows.Forms.TextBox();
            this.cmb_CardName = new System.Windows.Forms.ComboBox();
            this.driversCardBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txt_CardNo = new System.Windows.Forms.MaskedTextBox();
            this.txt_Amount2 = new System.Windows.Forms.TextBox();
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
            this.tableLayoutPanel3.SuspendLayout();
            this.panel6.SuspendLayout();
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
            this.newDGV1.MultiSelect = false;
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
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(365, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel2.SetRowSpan(this.panel1, 7);
            this.panel1.Size = new System.Drawing.Size(647, 208);
            this.panel1.TabIndex = 45;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 291F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label17, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.label15, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.label14, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.panel6, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.txt_UnitPrice2, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.label16, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txt_Num, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label10, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmb_Gubun2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label12, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.dtp_ApplyDate2, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.label13, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmb_ChargeAccountId, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txt_Remark, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmb_CardName, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.txt_CardNo, 3, 5);
            this.tableLayoutPanel3.Controls.Add(this.txt_Amount2, 3, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel3.RowCount = 8;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(647, 208);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(4, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 28);
            this.label9.TabIndex = 80;
            this.label9.Text = "세액 :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(329, 113);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(79, 28);
            this.label17.TabIndex = 78;
            this.label17.Text = "카드번호 :";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(4, 113);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(102, 28);
            this.label15.TabIndex = 76;
            this.label15.Text = "카드사명 :";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(329, 85);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 28);
            this.label14.TabIndex = 75;
            this.label14.Text = "합계금액 :";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.chk_UseTax2);
            this.panel6.Controls.Add(this.txt_Tax2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(109, 85);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(217, 28);
            this.panel6.TabIndex = 73;
            this.panel6.TabStop = true;
            // 
            // chk_UseTax2
            // 
            this.chk_UseTax2.AutoSize = true;
            this.chk_UseTax2.Location = new System.Drawing.Point(161, 7);
            this.chk_UseTax2.Name = "chk_UseTax2";
            this.chk_UseTax2.Size = new System.Drawing.Size(48, 16);
            this.chk_UseTax2.TabIndex = 1;
            this.chk_UseTax2.Text = "VAT";
            this.chk_UseTax2.UseVisualStyleBackColor = true;
            this.chk_UseTax2.CheckedChanged += new System.EventHandler(this.chk_UseTax2_CheckedChanged);
            // 
            // txt_Tax2
            // 
            this.txt_Tax2.Location = new System.Drawing.Point(3, 4);
            this.txt_Tax2.Name = "txt_Tax2";
            this.txt_Tax2.ReadOnly = true;
            this.txt_Tax2.Size = new System.Drawing.Size(154, 21);
            this.txt_Tax2.TabIndex = 76;
            // 
            // txt_UnitPrice2
            // 
            this.txt_UnitPrice2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_UnitPrice2.Location = new System.Drawing.Point(414, 60);
            this.txt_UnitPrice2.MaxLength = 10;
            this.txt_UnitPrice2.Name = "txt_UnitPrice2";
            this.txt_UnitPrice2.Size = new System.Drawing.Size(154, 21);
            this.txt_UnitPrice2.TabIndex = 55;
            this.txt_UnitPrice2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_UnitPrice2_KeyPress);
            this.txt_UnitPrice2.Leave += new System.EventHandler(this.txt_UnitPrice2_Leave);
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(329, 57);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 28);
            this.label16.TabIndex = 72;
            this.label16.Text = "단가 :";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(4, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 28);
            this.label6.TabIndex = 70;
            this.label6.Text = "수량 :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Num
            // 
            this.txt_Num.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Num.Location = new System.Drawing.Point(112, 60);
            this.txt_Num.MaxLength = 5;
            this.txt_Num.Name = "txt_Num";
            this.txt_Num.Size = new System.Drawing.Size(154, 21);
            this.txt_Num.TabIndex = 54;
            this.txt_Num.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Num_KeyPress);
            this.txt_Num.Leave += new System.EventHandler(this.txt_Num_Leave);
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(329, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 28);
            this.label10.TabIndex = 57;
            this.label10.Text = "적요 :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(4, 1);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(102, 28);
            this.label11.TabIndex = 34;
            this.label11.Text = "전표구분 :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_Gubun2
            // 
            this.cmb_Gubun2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Gubun2.FormattingEnabled = true;
            this.cmb_Gubun2.Location = new System.Drawing.Point(112, 4);
            this.cmb_Gubun2.Name = "cmb_Gubun2";
            this.cmb_Gubun2.Size = new System.Drawing.Size(154, 20);
            this.cmb_Gubun2.TabIndex = 50;
            this.cmb_Gubun2.SelectedIndexChanged += new System.EventHandler(this.cmb_Gubun2_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(329, 1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 28);
            this.label12.TabIndex = 6;
            this.label12.Text = "적용일자 :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtp_ApplyDate2
            // 
            this.dtp_ApplyDate2.CustomFormat = "yyyy/MM/dd";
            this.dtp_ApplyDate2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_ApplyDate2.Location = new System.Drawing.Point(414, 4);
            this.dtp_ApplyDate2.Name = "dtp_ApplyDate2";
            this.dtp_ApplyDate2.Size = new System.Drawing.Size(154, 21);
            this.dtp_ApplyDate2.TabIndex = 51;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(4, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 28);
            this.label13.TabIndex = 51;
            this.label13.Text = "계정과목 :";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_ChargeAccountId
            // 
            this.cmb_ChargeAccountId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_ChargeAccountId.FormattingEnabled = true;
            this.cmb_ChargeAccountId.Location = new System.Drawing.Point(112, 32);
            this.cmb_ChargeAccountId.Name = "cmb_ChargeAccountId";
            this.cmb_ChargeAccountId.Size = new System.Drawing.Size(154, 20);
            this.cmb_ChargeAccountId.TabIndex = 52;
            // 
            // txt_Remark
            // 
            this.txt_Remark.Location = new System.Drawing.Point(414, 32);
            this.txt_Remark.MaxLength = 50;
            this.txt_Remark.Name = "txt_Remark";
            this.txt_Remark.Size = new System.Drawing.Size(154, 21);
            this.txt_Remark.TabIndex = 53;
            // 
            // cmb_CardName
            // 
            this.cmb_CardName.DataSource = this.driversCardBindingSource;
            this.cmb_CardName.DisplayMember = "CardName";
            this.cmb_CardName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_CardName.FormattingEnabled = true;
            this.cmb_CardName.Location = new System.Drawing.Point(112, 116);
            this.cmb_CardName.Name = "cmb_CardName";
            this.cmb_CardName.Size = new System.Drawing.Size(154, 20);
            this.cmb_CardName.TabIndex = 77;
            this.cmb_CardName.ValueMember = "CardNo";
            this.cmb_CardName.SelectedIndexChanged += new System.EventHandler(this.cmb_CardName_SelectedIndexChanged);
            // 
            // driversCardBindingSource
            // 
            this.driversCardBindingSource.DataMember = "DriversCard";
            this.driversCardBindingSource.DataSource = this.cMDataSet;
            // 
            // txt_CardNo
            // 
            this.txt_CardNo.Location = new System.Drawing.Point(414, 116);
            this.txt_CardNo.Mask = "9999-9999-9999-9999";
            this.txt_CardNo.Name = "txt_CardNo";
            this.txt_CardNo.ReadOnly = true;
            this.txt_CardNo.Size = new System.Drawing.Size(154, 21);
            this.txt_CardNo.TabIndex = 79;
            // 
            // txt_Amount2
            // 
            this.txt_Amount2.Location = new System.Drawing.Point(414, 88);
            this.txt_Amount2.Name = "txt_Amount2";
            this.txt_Amount2.ReadOnly = true;
            this.txt_Amount2.Size = new System.Drawing.Size(154, 21);
            this.txt_Amount2.TabIndex = 81;
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
            // FrmMN0404_CHAGECARMANAGE_ADD2
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
            this.Name = "FrmMN0404_CHAGECARMANAGE_ADD2";
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
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox chk_UseTax2;
        private System.Windows.Forms.TextBox txt_Tax2;
        private System.Windows.Forms.TextBox txt_UnitPrice2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_Num;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmb_Gubun2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtp_ApplyDate2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmb_ChargeAccountId;
        private System.Windows.Forms.TextBox txt_Remark;
        private System.Windows.Forms.ComboBox cmb_CardName;
        private System.Windows.Forms.MaskedTextBox txt_CardNo;
        private System.Windows.Forms.TextBox txt_Amount2;
    }
}