namespace mycalltruck.Admin
{
    partial class FrmTrade_Client_Add
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_CustomerName = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView2 = new mycalltruck.Admin.NewDGV();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.panel17 = new System.Windows.Forms.Panel();
            this.btn_InfoSearch_Clear = new System.Windows.Forms.Button();
            this.btn_InfoSearch = new System.Windows.Forms.Button();
            this.txt_InfoSearch = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txt_PayInputName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmb_PayBankName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_PayAccountNo = new System.Windows.Forms.TextBox();
            this.txt_Amount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_Price = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chk_Vat = new System.Windows.Forms.CheckBox();
            this.txt_VAT = new System.Windows.Forms.TextBox();
            this.dtp_RequestDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_BizNo = new System.Windows.Forms.TextBox();
            this.txt_SangHo = new System.Windows.Forms.TextBox();
            this.txt_Item = new System.Windows.Forms.TextBox();
            this.txt_CEO = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_HasAcc = new System.Windows.Forms.Label();
            this.cmb_HasAcc = new System.Windows.Forms.ComboBox();
            this.txt_CustomerId = new System.Windows.Forms.TextBox();
            this.tradesTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.TradesTableAdapter();
            this.btnExcelInsert = new System.Windows.Forms.Button();
            this.btnExcelDown = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.trades1TableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.Trades1TableAdapter();
            this.driversTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DriversTableAdapter();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.clientsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ClientsTableAdapter();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.label66 = new System.Windows.Forms.Label();
            this.pnProgress = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.panel17.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
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
            this.tableLayoutPanel2.Controls.Add(this.label10, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_PayInputName, 3, 6);
            this.tableLayoutPanel2.Controls.Add(this.label13, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.cmb_PayBankName, 3, 5);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label8, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label4, 4, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_PayAccountNo, 5, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_Amount, 5, 4);
            this.tableLayoutPanel2.Controls.Add(this.label9, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.label16, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_Price, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.dtp_RequestDate, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_BizNo, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_SangHo, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Item, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.txt_CEO, 5, 2);
            this.tableLayoutPanel2.Controls.Add(this.label5, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.lbl_HasAcc, 4, 6);
            this.tableLayoutPanel2.Controls.Add(this.cmb_HasAcc, 5, 6);
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
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(368, 193);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 36);
            this.label10.TabIndex = 50;
            this.label10.Text = "예금주(*) :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 4);
            this.panel1.Controls.Add(this.lbl_CustomerName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(367, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(7);
            this.panel1.Size = new System.Drawing.Size(643, 28);
            this.panel1.TabIndex = 42;
            // 
            // lbl_CustomerName
            // 
            this.lbl_CustomerName.AutoSize = true;
            this.lbl_CustomerName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbl_CustomerName.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_CustomerName.ForeColor = System.Drawing.Color.Red;
            this.lbl_CustomerName.Location = new System.Drawing.Point(622, 7);
            this.lbl_CustomerName.Name = "lbl_CustomerName";
            this.lbl_CustomerName.Size = new System.Drawing.Size(12, 12);
            this.lbl_CustomerName.TabIndex = 2;
            this.lbl_CustomerName.Text = "-";
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
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.carNoDataGridViewTextBoxColumn,
            this.carYearDataGridViewTextBoxColumn,
            this.Column1});
            this.dataGridView2.DataSource = this.clientsBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle6;
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
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "상호";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 130;
            // 
            // carNoDataGridViewTextBoxColumn
            // 
            this.carNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            this.carNoDataGridViewTextBoxColumn.HeaderText = "사업자등록번호";
            this.carNoDataGridViewTextBoxColumn.Name = "carNoDataGridViewTextBoxColumn";
            this.carNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.carNoDataGridViewTextBoxColumn.Width = 130;
            // 
            // carYearDataGridViewTextBoxColumn
            // 
            this.carYearDataGridViewTextBoxColumn.DataPropertyName = "CEO";
            this.carYearDataGridViewTextBoxColumn.HeaderText = "대표자";
            this.carYearDataGridViewTextBoxColumn.Name = "carYearDataGridViewTextBoxColumn";
            this.carYearDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ClientId";
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // clientsBindingSource
            // 
            this.clientsBindingSource.DataMember = "Clients";
            this.clientsBindingSource.DataSource = this.cMDataSet;
            this.clientsBindingSource.CurrentChanged += new System.EventHandler(this.clientsBindingSource_CurrentChanged);
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // txt_PayInputName
            // 
            this.txt_PayInputName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_PayInputName.Location = new System.Drawing.Point(467, 196);
            this.txt_PayInputName.MaxLength = 20;
            this.txt_PayInputName.Name = "txt_PayInputName";
            this.txt_PayInputName.Size = new System.Drawing.Size(164, 21);
            this.txt_PayInputName.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(368, 161);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 32);
            this.label13.TabIndex = 12;
            this.label13.Text = "입금 은행명(*) :";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_PayBankName
            // 
            this.cmb_PayBankName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_PayBankName.FormattingEnabled = true;
            this.cmb_PayBankName.Location = new System.Drawing.Point(467, 164);
            this.cmb_PayBankName.Name = "cmb_PayBankName";
            this.cmb_PayBankName.Size = new System.Drawing.Size(154, 20);
            this.cmb_PayBankName.TabIndex = 6;
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
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(695, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 32);
            this.label4.TabIndex = 12;
            this.label4.Text = "계좌번호(*) :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_PayAccountNo
            // 
            this.txt_PayAccountNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_PayAccountNo.Location = new System.Drawing.Point(794, 164);
            this.txt_PayAccountNo.MaxLength = 20;
            this.txt_PayAccountNo.Name = "txt_PayAccountNo";
            this.txt_PayAccountNo.Size = new System.Drawing.Size(154, 21);
            this.txt_PayAccountNo.TabIndex = 7;
            // 
            // txt_Amount
            // 
            this.txt_Amount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Amount.Location = new System.Drawing.Point(794, 132);
            this.txt_Amount.Name = "txt_Amount";
            this.txt_Amount.ReadOnly = true;
            this.txt_Amount.Size = new System.Drawing.Size(154, 21);
            this.txt_Amount.TabIndex = 5;
            this.txt_Amount.TabStop = false;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(695, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 32);
            this.label9.TabIndex = 8;
            this.label9.Text = "합계금액 :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(695, 97);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 32);
            this.label16.TabIndex = 15;
            this.label16.Text = "청구금액(*) :";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Price
            // 
            this.txt_Price.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Price.Location = new System.Drawing.Point(794, 100);
            this.txt_Price.MaxLength = 10;
            this.txt_Price.Name = "txt_Price";
            this.txt_Price.Size = new System.Drawing.Size(154, 21);
            this.txt_Price.TabIndex = 4;
            this.txt_Price.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Price_KeyPress);
            this.txt_Price.Leave += new System.EventHandler(this.txt_Price_Leave);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(368, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "청구항목(*) :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chk_Vat);
            this.panel3.Controls.Add(this.txt_VAT);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(464, 129);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(228, 32);
            this.panel3.TabIndex = 5;
            // 
            // chk_Vat
            // 
            this.chk_Vat.AutoSize = true;
            this.chk_Vat.Location = new System.Drawing.Point(106, 6);
            this.chk_Vat.Name = "chk_Vat";
            this.chk_Vat.Size = new System.Drawing.Size(48, 16);
            this.chk_Vat.TabIndex = 1;
            this.chk_Vat.Text = "VAT";
            this.chk_Vat.UseVisualStyleBackColor = true;
            this.chk_Vat.CheckedChanged += new System.EventHandler(this.chk_Vat_CheckedChanged);
            // 
            // txt_VAT
            // 
            this.txt_VAT.Location = new System.Drawing.Point(3, 3);
            this.txt_VAT.Name = "txt_VAT";
            this.txt_VAT.ReadOnly = true;
            this.txt_VAT.Size = new System.Drawing.Size(100, 21);
            this.txt_VAT.TabIndex = 7;
            this.txt_VAT.TabStop = false;
            // 
            // dtp_RequestDate
            // 
            this.dtp_RequestDate.CustomFormat = "yyyy/MM/dd";
            this.dtp_RequestDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_RequestDate.Location = new System.Drawing.Point(467, 100);
            this.dtp_RequestDate.Name = "dtp_RequestDate";
            this.dtp_RequestDate.Size = new System.Drawing.Size(100, 21);
            this.dtp_RequestDate.TabIndex = 3;
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
            // 
            // txt_SangHo
            // 
            this.txt_SangHo.Location = new System.Drawing.Point(794, 36);
            this.txt_SangHo.Name = "txt_SangHo";
            this.txt_SangHo.ReadOnly = true;
            this.txt_SangHo.Size = new System.Drawing.Size(154, 21);
            this.txt_SangHo.TabIndex = 49;
            this.txt_SangHo.TabStop = false;
            // 
            // txt_Item
            // 
            this.txt_Item.Location = new System.Drawing.Point(467, 68);
            this.txt_Item.MaxLength = 50;
            this.txt_Item.Name = "txt_Item";
            this.txt_Item.Size = new System.Drawing.Size(154, 21);
            this.txt_Item.TabIndex = 2;
            // 
            // txt_CEO
            // 
            this.txt_CEO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CEO.Location = new System.Drawing.Point(794, 68);
            this.txt_CEO.Name = "txt_CEO";
            this.txt_CEO.ReadOnly = true;
            this.txt_CEO.Size = new System.Drawing.Size(154, 21);
            this.txt_CEO.TabIndex = 51;
            this.txt_CEO.TabStop = false;
            this.txt_CEO.Visible = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(695, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 32);
            this.label5.TabIndex = 45;
            this.label5.Text = "대표자 :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Visible = false;
            // 
            // lbl_HasAcc
            // 
            this.lbl_HasAcc.Location = new System.Drawing.Point(695, 193);
            this.lbl_HasAcc.Name = "lbl_HasAcc";
            this.lbl_HasAcc.Size = new System.Drawing.Size(93, 25);
            this.lbl_HasAcc.TabIndex = 90;
            this.lbl_HasAcc.Text = "결제요청";
            this.lbl_HasAcc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_HasAcc
            // 
            this.cmb_HasAcc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_HasAcc.FormattingEnabled = true;
            this.cmb_HasAcc.Location = new System.Drawing.Point(794, 196);
            this.cmb_HasAcc.Name = "cmb_HasAcc";
            this.cmb_HasAcc.Size = new System.Drawing.Size(154, 20);
            this.cmb_HasAcc.TabIndex = 91;
            // 
            // txt_CustomerId
            // 
            this.txt_CustomerId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CustomerId.Location = new System.Drawing.Point(150, 240);
            this.txt_CustomerId.Name = "txt_CustomerId";
            this.txt_CustomerId.Size = new System.Drawing.Size(48, 21);
            this.txt_CustomerId.TabIndex = 40;
            // 
            // tradesTableAdapter
            // 
            this.tradesTableAdapter.ClearBeforeFill = true;
            // 
            // btnExcelInsert
            // 
            this.btnExcelInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcelInsert.Location = new System.Drawing.Point(2, 238);
            this.btnExcelInsert.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnExcelInsert.Name = "btnExcelInsert";
            this.btnExcelInsert.Size = new System.Drawing.Size(131, 23);
            this.btnExcelInsert.TabIndex = 51;
            this.btnExcelInsert.TabStop = false;
            this.btnExcelInsert.Text = "협력업체 일괄 역발행";
            this.btnExcelInsert.UseVisualStyleBackColor = true;
            this.btnExcelInsert.Click += new System.EventHandler(this.btnExcelInsert_Click);
            // 
            // btnExcelDown
            // 
            this.btnExcelDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcelDown.Location = new System.Drawing.Point(136, 238);
            this.btnExcelDown.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnExcelDown.Name = "btnExcelDown";
            this.btnExcelDown.Size = new System.Drawing.Size(80, 23);
            this.btnExcelDown.TabIndex = 52;
            this.btnExcelDown.TabStop = false;
            this.btnExcelDown.Text = "양식";
            this.btnExcelDown.UseVisualStyleBackColor = true;
            this.btnExcelDown.Click += new System.EventHandler(this.btnExcelDown_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // trades1TableAdapter
            // 
            this.trades1TableAdapter.ClearBeforeFill = true;
            // 
            // driversTableAdapter
            // 
            this.driversTableAdapter.ClearBeforeFill = true;
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.newDGV1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle3;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(238, 244);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.MultiSelect = false;
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.Size = new System.Drawing.Size(42, 16);
            this.newDGV1.TabIndex = 50;
            this.newDGV1.Visible = false;
            // 
            // clientsTableAdapter
            // 
            this.clientsTableAdapter.ClearBeforeFill = true;
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
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(407, 101);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 49;
            this.pnProgress.Visible = false;
            // 
            // FrmTrade_Client_Add
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1015, 266);
            this.Controls.Add(this.btnExcelDown);
            this.Controls.Add(this.newDGV1);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.txt_CustomerId);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnExcelInsert);
            this.Name = "FrmTrade_Client_Add";
            this.Text = "매입관리(운송사) 역발행";
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
            ((System.ComponentModel.ISupportInitialize)(this.clientsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            this.pnProgress.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_VAT;
        private System.Windows.Forms.TextBox txt_Price;
        private System.Windows.Forms.TextBox txt_Item;
        private System.Windows.Forms.TextBox txt_Amount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_PayAccountNo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chk_Vat;
        private System.Windows.Forms.TextBox txt_CustomerId;
        private System.Windows.Forms.DateTimePicker dtp_RequestDate;
        private System.Windows.Forms.ComboBox cmb_PayBankName;
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
        private System.Windows.Forms.Label lbl_CustomerName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_PayInputName;
        private CMDataSetTableAdapters.TradesTableAdapter tradesTableAdapter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_BizNo;
        private System.Windows.Forms.TextBox txt_SangHo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_CEO;
        private NewDGV newDGV1;
        private System.Windows.Forms.Button btnExcelInsert;
        private System.Windows.Forms.Button btnExcelDown;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private CMDataSetTableAdapters.Trades1TableAdapter trades1TableAdapter;
        private CMDataSetTableAdapters.DriversTableAdapter driversTableAdapter;
        private System.Windows.Forms.BindingSource clientsBindingSource;
        private CMDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Label lbl_HasAcc;
        private System.Windows.Forms.ComboBox cmb_HasAcc;

    }
}