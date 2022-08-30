namespace mycalltruck.Admin
{
    partial class FrmTrade_Add_CU_SG
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label83 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView2 = new mycalltruck.Admin.NewDGV();
            this.bizNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sangHoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.clientDataSet = new mycalltruck.Admin.DataSets.ClientDataSet();
            this.panel17 = new System.Windows.Forms.Panel();
            this.btn_InfoSearch_Clear = new System.Windows.Forms.Button();
            this.btn_InfoSearch = new System.Windows.Forms.Button();
            this.txt_InfoSearch = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_Amt = new System.Windows.Forms.Label();
            this.lbl_Price = new System.Windows.Forms.Label();
            this.txt_Price = new System.Windows.Forms.TextBox();
            this.txt_Item = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdb_Tax2 = new System.Windows.Forms.RadioButton();
            this.rdb_Tax1 = new System.Windows.Forms.RadioButton();
            this.rdb_Tax0 = new System.Windows.Forms.RadioButton();
            this.txt_VAT = new System.Windows.Forms.TextBox();
            this.dtp_RequestDate = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.dtp_EndDate = new System.Windows.Forms.DateTimePicker();
            this.dtp_BeginDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_BizNo = new System.Windows.Forms.TextBox();
            this.txt_Ceo = new System.Windows.Forms.TextBox();
            this.txt_Amount = new System.Windows.Forms.TextBox();
            this.baseDataSet = new mycalltruck.Admin.DataSets.BaseDataSet();
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.customersTableAdapter = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.CustomersTableAdapter();
            this.tableAdapterManager = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).BeginInit();
            this.panel17.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.pnProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(918, 237);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 34;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddClose.Location = new System.Drawing.Point(818, 237);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 23);
            this.btnAddClose.TabIndex = 35;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기(F6)";
            this.btnAddClose.UseVisualStyleBackColor = true;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(715, 237);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 23);
            this.btnAdd.TabIndex = 33;
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
            this.groupBox2.Size = new System.Drawing.Size(1015, 232);
            this.groupBox2.TabIndex = 32;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 256F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 228F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label8, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.lbl_Amt, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.lbl_Price, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_Price, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_Item, 5, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.dtp_RequestDate, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.label12, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_BizNo, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Ceo, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Amount, 5, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28816F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1013, 230);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 4);
            this.panel1.Controls.Add(this.label83);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(367, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(7);
            this.panel1.Size = new System.Drawing.Size(643, 28);
            this.panel1.TabIndex = 42;
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Dock = System.Windows.Forms.DockStyle.Right;
            this.label83.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label83.ForeColor = System.Drawing.Color.Red;
            this.label83.Location = new System.Drawing.Point(622, 7);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(12, 12);
            this.label83.TabIndex = 2;
            this.label83.Text = "-";
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
            this.panel4.Size = new System.Drawing.Size(364, 228);
            this.panel4.TabIndex = 43;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.dataGridView2, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.panel17, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(364, 228);
            this.tableLayoutPanel7.TabIndex = 3;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToOrderColumns = true;
            this.dataGridView2.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bizNoDataGridViewTextBoxColumn,
            this.sangHoDataGridViewTextBoxColumn,
            this.ceoDataGridViewTextBoxColumn});
            this.dataGridView2.DataSource = this.customersBindingSource;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.GridColor = System.Drawing.Color.White;
            this.dataGridView2.Location = new System.Drawing.Point(0, 30);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(364, 198);
            this.dataGridView2.TabIndex = 4;
            this.dataGridView2.TabStop = false;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dataGridView2.CurrentCellChanged += new System.EventHandler(this.dataGridView2_CurrentCellChanged);
            // 
            // bizNoDataGridViewTextBoxColumn
            // 
            this.bizNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.bizNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.bizNoDataGridViewTextBoxColumn.HeaderText = "사업자번호";
            this.bizNoDataGridViewTextBoxColumn.Name = "bizNoDataGridViewTextBoxColumn";
            this.bizNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.bizNoDataGridViewTextBoxColumn.Width = 120;
            // 
            // sangHoDataGridViewTextBoxColumn
            // 
            this.sangHoDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sangHoDataGridViewTextBoxColumn.DataPropertyName = "SangHo";
            this.sangHoDataGridViewTextBoxColumn.HeaderText = "상호";
            this.sangHoDataGridViewTextBoxColumn.Name = "sangHoDataGridViewTextBoxColumn";
            this.sangHoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ceoDataGridViewTextBoxColumn
            // 
            this.ceoDataGridViewTextBoxColumn.DataPropertyName = "Ceo";
            this.ceoDataGridViewTextBoxColumn.HeaderText = "대표자";
            this.ceoDataGridViewTextBoxColumn.Name = "ceoDataGridViewTextBoxColumn";
            this.ceoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customersBindingSource
            // 
            this.customersBindingSource.DataMember = "Customers";
            this.customersBindingSource.DataSource = this.clientDataSet;
            // 
            // clientDataSet
            // 
            this.clientDataSet.DataSetName = "ClientDataSet";
            this.clientDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // btn_InfoSearch_Clear
            // 
            this.btn_InfoSearch_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_InfoSearch_Clear.Location = new System.Drawing.Point(308, 3);
            this.btn_InfoSearch_Clear.Name = "btn_InfoSearch_Clear";
            this.btn_InfoSearch_Clear.Size = new System.Drawing.Size(53, 23);
            this.btn_InfoSearch_Clear.TabIndex = 40;
            this.btn_InfoSearch_Clear.TabStop = false;
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
            this.btn_InfoSearch.TabStop = false;
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
            this.txt_InfoSearch.TabStop = false;
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
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(368, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 32);
            this.label3.TabIndex = 2;
            this.label3.Text = "부가세 :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(368, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 32);
            this.label8.TabIndex = 7;
            this.label8.Text = "청구일 :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Amt
            // 
            this.lbl_Amt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Amt.Location = new System.Drawing.Point(695, 129);
            this.lbl_Amt.Name = "lbl_Amt";
            this.lbl_Amt.Size = new System.Drawing.Size(93, 32);
            this.lbl_Amt.TabIndex = 8;
            this.lbl_Amt.Text = "합계금액 :";
            this.lbl_Amt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Price
            // 
            this.lbl_Price.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Price.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Price.Location = new System.Drawing.Point(695, 97);
            this.lbl_Price.Name = "lbl_Price";
            this.lbl_Price.Size = new System.Drawing.Size(93, 32);
            this.lbl_Price.TabIndex = 15;
            this.lbl_Price.Text = "청구금액 :";
            this.lbl_Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Price
            // 
            this.txt_Price.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Price.Location = new System.Drawing.Point(794, 100);
            this.txt_Price.MaxLength = 10;
            this.txt_Price.Name = "txt_Price";
            this.txt_Price.Size = new System.Drawing.Size(154, 21);
            this.txt_Price.TabIndex = 5;
            this.txt_Price.Enter += new System.EventHandler(this.txt_Price_Enter);
            this.txt_Price.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Price_KeyPress);
            this.txt_Price.Leave += new System.EventHandler(this.txt_Price_Leave);
            // 
            // txt_Item
            // 
            this.txt_Item.Location = new System.Drawing.Point(794, 68);
            this.txt_Item.MaxLength = 50;
            this.txt_Item.Name = "txt_Item";
            this.txt_Item.Size = new System.Drawing.Size(154, 21);
            this.txt_Item.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(695, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "청구항목 :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdb_Tax2);
            this.panel3.Controls.Add(this.rdb_Tax1);
            this.panel3.Controls.Add(this.rdb_Tax0);
            this.panel3.Controls.Add(this.txt_VAT);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(464, 129);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(228, 32);
            this.panel3.TabIndex = 5;
            // 
            // rdb_Tax2
            // 
            this.rdb_Tax2.AutoSize = true;
            this.rdb_Tax2.Location = new System.Drawing.Point(180, 8);
            this.rdb_Tax2.Name = "rdb_Tax2";
            this.rdb_Tax2.Size = new System.Drawing.Size(47, 16);
            this.rdb_Tax2.TabIndex = 55;
            this.rdb_Tax2.Text = "면세";
            this.rdb_Tax2.UseVisualStyleBackColor = true;
            this.rdb_Tax2.CheckedChanged += new System.EventHandler(this.rdb_Tax0_CheckedChanged);
            // 
            // rdb_Tax1
            // 
            this.rdb_Tax1.AutoSize = true;
            this.rdb_Tax1.Location = new System.Drawing.Point(127, 8);
            this.rdb_Tax1.Name = "rdb_Tax1";
            this.rdb_Tax1.Size = new System.Drawing.Size(47, 16);
            this.rdb_Tax1.TabIndex = 54;
            this.rdb_Tax1.Text = "포함";
            this.rdb_Tax1.UseVisualStyleBackColor = true;
            this.rdb_Tax1.CheckedChanged += new System.EventHandler(this.rdb_Tax0_CheckedChanged);
            // 
            // rdb_Tax0
            // 
            this.rdb_Tax0.AutoSize = true;
            this.rdb_Tax0.Checked = true;
            this.rdb_Tax0.Location = new System.Drawing.Point(74, 8);
            this.rdb_Tax0.Name = "rdb_Tax0";
            this.rdb_Tax0.Size = new System.Drawing.Size(47, 16);
            this.rdb_Tax0.TabIndex = 53;
            this.rdb_Tax0.TabStop = true;
            this.rdb_Tax0.Text = "별도";
            this.rdb_Tax0.UseVisualStyleBackColor = true;
            this.rdb_Tax0.CheckedChanged += new System.EventHandler(this.rdb_Tax0_CheckedChanged);
            // 
            // txt_VAT
            // 
            this.txt_VAT.Location = new System.Drawing.Point(2, 6);
            this.txt_VAT.MaxLength = 10;
            this.txt_VAT.Name = "txt_VAT";
            this.txt_VAT.ReadOnly = true;
            this.txt_VAT.Size = new System.Drawing.Size(67, 21);
            this.txt_VAT.TabIndex = 52;
            this.txt_VAT.TabStop = false;
            // 
            // dtp_RequestDate
            // 
            this.dtp_RequestDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_RequestDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_RequestDate.Location = new System.Drawing.Point(467, 100);
            this.dtp_RequestDate.Name = "dtp_RequestDate";
            this.dtp_RequestDate.Size = new System.Drawing.Size(100, 21);
            this.dtp_RequestDate.TabIndex = 4;
            this.dtp_RequestDate.TabStop = false;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(368, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 32);
            this.label12.TabIndex = 11;
            this.label12.Text = "운송기간 :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.dtp_EndDate);
            this.panel2.Controls.Add(this.dtp_BeginDate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(464, 65);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 32);
            this.panel2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(106, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 12);
            this.label6.TabIndex = 38;
            this.label6.Text = "~";
            // 
            // dtp_EndDate
            // 
            this.dtp_EndDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_EndDate.Location = new System.Drawing.Point(123, 4);
            this.dtp_EndDate.Name = "dtp_EndDate";
            this.dtp_EndDate.Size = new System.Drawing.Size(100, 21);
            this.dtp_EndDate.TabIndex = 2;
            this.dtp_EndDate.TabStop = false;
            // 
            // dtp_BeginDate
            // 
            this.dtp_BeginDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_BeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_BeginDate.Location = new System.Drawing.Point(5, 4);
            this.dtp_BeginDate.Name = "dtp_BeginDate";
            this.dtp_BeginDate.Size = new System.Drawing.Size(100, 21);
            this.dtp_BeginDate.TabIndex = 1;
            this.dtp_BeginDate.TabStop = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(368, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 32);
            this.label1.TabIndex = 46;
            this.label1.Text = "사업자번호 :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(695, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 32);
            this.label7.TabIndex = 47;
            this.label7.Text = "상호 :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_BizNo
            // 
            this.txt_BizNo.Location = new System.Drawing.Point(467, 36);
            this.txt_BizNo.Name = "txt_BizNo";
            this.txt_BizNo.ReadOnly = true;
            this.txt_BizNo.Size = new System.Drawing.Size(154, 21);
            this.txt_BizNo.TabIndex = 48;
            this.txt_BizNo.TabStop = false;
            this.txt_BizNo.TextChanged += new System.EventHandler(this.txt_BizNo_TextChanged);
            // 
            // txt_Ceo
            // 
            this.txt_Ceo.Location = new System.Drawing.Point(794, 36);
            this.txt_Ceo.Name = "txt_Ceo";
            this.txt_Ceo.ReadOnly = true;
            this.txt_Ceo.Size = new System.Drawing.Size(154, 21);
            this.txt_Ceo.TabIndex = 49;
            this.txt_Ceo.TabStop = false;
            // 
            // txt_Amount
            // 
            this.txt_Amount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Amount.Location = new System.Drawing.Point(794, 132);
            this.txt_Amount.MaxLength = 10;
            this.txt_Amount.Name = "txt_Amount";
            this.txt_Amount.ReadOnly = true;
            this.txt_Amount.Size = new System.Drawing.Size(154, 21);
            this.txt_Amount.TabIndex = 5;
            this.txt_Amount.TabStop = false;
            this.txt_Amount.Enter += new System.EventHandler(this.txt_Amount_Enter);
            this.txt_Amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Amount_KeyPress);
            this.txt_Amount.Leave += new System.EventHandler(this.txt_Amount_Leave);
            // 
            // baseDataSet
            // 
            this.baseDataSet.DataSetName = "BaseDataSet";
            this.baseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(407, 112);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 49;
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
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // customersTableAdapter
            // 
            this.customersTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.ClientSmsCountTableAdapter = null;
            this.tableAdapterManager.ClientsTableAdapter = null;
            this.tableAdapterManager.CustomersTableAdapter = this.customersTableAdapter;
            this.tableAdapterManager.SubClientsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // FrmTrade_Add_CU_SG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1015, 266);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox2);
            this.Name = "FrmTrade_Add_CU_SG";
            this.Text = "청구관리 추가";
            this.Load += new System.EventHandler(this.FrmTrade_Add_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).EndInit();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.pnProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lbl_Price;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbl_Amt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Price;
        private System.Windows.Forms.TextBox txt_Item;
        private System.Windows.Forms.TextBox txt_Amount;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtp_EndDate;
        private System.Windows.Forms.DateTimePicker dtp_BeginDate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dtp_RequestDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private NewDGV dataGridView2;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Button btn_InfoSearch_Clear;
        private System.Windows.Forms.Button btn_InfoSearch;
        private System.Windows.Forms.TextBox txt_InfoSearch;
        private System.Windows.Forms.Label label22;
        private CMDataSet cMDataSet;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_BizNo;
        private System.Windows.Forms.TextBox txt_Ceo;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private DataSets.BaseDataSet baseDataSet;
        private DataSets.ClientDataSet clientDataSet;
        private System.Windows.Forms.BindingSource customersBindingSource;
        private DataSets.ClientDataSetTableAdapters.CustomersTableAdapter customersTableAdapter;
        private DataSets.ClientDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sangHoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceoDataGridViewTextBoxColumn;
        private System.Windows.Forms.RadioButton rdb_Tax2;
        private System.Windows.Forms.RadioButton rdb_Tax1;
        private System.Windows.Forms.RadioButton rdb_Tax0;
        private System.Windows.Forms.TextBox txt_VAT;
    }
}