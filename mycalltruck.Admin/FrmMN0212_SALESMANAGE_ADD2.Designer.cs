namespace mycalltruck.Admin
{
    partial class FrmMN0212_SALESMANAGE_ADD2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbTeam = new System.Windows.Forms.ComboBox();
            this.cmbSMonth = new System.Windows.Forms.ComboBox();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdb_Tax1 = new System.Windows.Forms.RadioButton();
            this.rdb_Tax0 = new System.Windows.Forms.RadioButton();
            this.btn_Search = new System.Windows.Forms.Button();
            this.dtp_To = new System.Windows.Forms.DateTimePicker();
            this.dtp_From = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AllSelect = new System.Windows.Forms.CheckBox();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.ColumnSelect = new mycalltruck.Admin.UI.DataGridViewDisableCheckBoxColumn();
            this.ColumnNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCustomerTeam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAcceptDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStopName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPayLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDriverCarNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerSelect = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.customersTableAdapter = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.CustomersTableAdapter();
            this.clientDataSet = new mycalltruck.Admin.DataSets.ClientDataSet();
            this.panel3 = new System.Windows.Forms.Panel();
            this.customerTeamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.customerTeamsTableAdapter = new mycalltruck.Admin.DataSets.CustomerUserDataSetTableAdapters.CustomerTeamsTableAdapter();
            this.customerUserDataSet = new mycalltruck.Admin.DataSets.CustomerUserDataSet();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerTeamsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerUserDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbTeam);
            this.panel1.Controls.Add(this.cmbSMonth);
            this.panel1.Controls.Add(this.cmb_Search);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_Search);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.rdb_Tax1);
            this.panel1.Controls.Add(this.rdb_Tax0);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Controls.Add(this.dtp_To);
            this.panel1.Controls.Add(this.dtp_From);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1073, 39);
            this.panel1.TabIndex = 0;
            // 
            // cmbTeam
            // 
            this.cmbTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeam.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbTeam.FormattingEnabled = true;
            this.cmbTeam.Location = new System.Drawing.Point(586, 7);
            this.cmbTeam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbTeam.Name = "cmbTeam";
            this.cmbTeam.Size = new System.Drawing.Size(105, 25);
            this.cmbTeam.TabIndex = 92;
            // 
            // cmbSMonth
            // 
            this.cmbSMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSMonth.FormattingEnabled = true;
            this.cmbSMonth.Location = new System.Drawing.Point(6, 8);
            this.cmbSMonth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbSMonth.Name = "cmbSMonth";
            this.cmbSMonth.Size = new System.Drawing.Size(80, 23);
            this.cmbSMonth.TabIndex = 56;
            this.cmbSMonth.SelectedIndexChanged += new System.EventHandler(this.cmbSMonth_SelectedIndexChanged);
            // 
            // cmb_Search
            // 
            this.cmb_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Location = new System.Drawing.Point(329, 7);
            this.cmb_Search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(160, 23);
            this.cmb_Search.TabIndex = 55;
            this.cmb_Search.TabStop = false;
            this.cmb_Search.SelectedIndexChanged += new System.EventHandler(this.cmb_Search_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.label3.Location = new System.Drawing.Point(271, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 19);
            this.label3.TabIndex = 54;
            this.label3.Text = "거래처 :";
            // 
            // txt_Search
            // 
            this.txt_Search.Location = new System.Drawing.Point(495, 7);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(87, 23);
            this.txt_Search.TabIndex = 53;
            this.txt_Search.TabStop = false;
            this.txt_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Search_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(922, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 52;
            this.label2.Text = "VAT :";
            // 
            // rdb_Tax1
            // 
            this.rdb_Tax1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Tax1.AutoSize = true;
            this.rdb_Tax1.Location = new System.Drawing.Point(1012, 10);
            this.rdb_Tax1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdb_Tax1.Name = "rdb_Tax1";
            this.rdb_Tax1.Size = new System.Drawing.Size(49, 19);
            this.rdb_Tax1.TabIndex = 51;
            this.rdb_Tax1.Text = "포함";
            this.rdb_Tax1.UseVisualStyleBackColor = true;
            // 
            // rdb_Tax0
            // 
            this.rdb_Tax0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Tax0.AutoSize = true;
            this.rdb_Tax0.Checked = true;
            this.rdb_Tax0.Location = new System.Drawing.Point(963, 10);
            this.rdb_Tax0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdb_Tax0.Name = "rdb_Tax0";
            this.rdb_Tax0.Size = new System.Drawing.Size(49, 19);
            this.rdb_Tax0.TabIndex = 50;
            this.rdb_Tax0.TabStop = true;
            this.rdb_Tax0.Text = "별도";
            this.rdb_Tax0.UseVisualStyleBackColor = true;
            this.rdb_Tax0.CheckedChanged += new System.EventHandler(this.rdb_Tax0_CheckedChanged);
            // 
            // btn_Search
            // 
            this.btn_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Search.FlatAppearance.BorderSize = 0;
            this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Search.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(701, 6);
            this.btn_Search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(56, 27);
            this.btn_Search.TabIndex = 46;
            this.btn_Search.TabStop = false;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // dtp_To
            // 
            this.dtp_To.CustomFormat = "yyyy/MM/dd";
            this.dtp_To.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_To.Location = new System.Drawing.Point(182, 8);
            this.dtp_To.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtp_To.Name = "dtp_To";
            this.dtp_To.Size = new System.Drawing.Size(86, 23);
            this.dtp_To.TabIndex = 45;
            this.dtp_To.TabStop = false;
            this.dtp_To.ValueChanged += new System.EventHandler(this.dtp_To_ValueChanged);
            // 
            // dtp_From
            // 
            this.dtp_From.CustomFormat = "yyyy/MM/dd";
            this.dtp_From.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_From.Location = new System.Drawing.Point(92, 8);
            this.dtp_From.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtp_From.Name = "dtp_From";
            this.dtp_From.Size = new System.Drawing.Size(86, 23);
            this.dtp_From.TabIndex = 44;
            this.dtp_From.TabStop = false;
            this.dtp_From.ValueChanged += new System.EventHandler(this.dtp_From_ValueChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.AllSelect);
            this.panel2.Controls.Add(this.newDGV1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1073, 274);
            this.panel2.TabIndex = 1;
            // 
            // AllSelect
            // 
            this.AllSelect.AutoSize = true;
            this.AllSelect.Location = new System.Drawing.Point(45, 4);
            this.AllSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AllSelect.Name = "AllSelect";
            this.AllSelect.Size = new System.Drawing.Size(15, 14);
            this.AllSelect.TabIndex = 2;
            this.AllSelect.UseVisualStyleBackColor = true;
            this.AllSelect.CheckedChanged += new System.EventHandler(this.AllSelect_CheckedChanged);
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 9F);
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSelect,
            this.ColumnNumber,
            this.ColumnCustomerTeam,
            this.ColumnAcceptDate,
            this.ColumnStartName,
            this.ColumnStopName,
            this.ColumnItem,
            this.ColumnPayLocation,
            this.ColumnDriverCarNo,
            this.Column6,
            this.Column7,
            this.Column8});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle9;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(0, 0);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.MultiSelect = false;
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.newDGV1.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newDGV1.Size = new System.Drawing.Size(1073, 271);
            this.newDGV1.TabIndex = 0;
            this.newDGV1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newDGV1_CellContentClick);
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            // 
            // ColumnSelect
            // 
            this.ColumnSelect.DataPropertyName = "Selected";
            this.ColumnSelect.HeaderText = "선택";
            this.ColumnSelect.Name = "ColumnSelect";
            this.ColumnSelect.ReadOnly = true;
            this.ColumnSelect.Width = 60;
            // 
            // ColumnNumber
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.ColumnNumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnNumber.HeaderText = "번호";
            this.ColumnNumber.Name = "ColumnNumber";
            this.ColumnNumber.ReadOnly = true;
            this.ColumnNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnNumber.Width = 50;
            // 
            // ColumnCustomerTeam
            // 
            this.ColumnCustomerTeam.DataPropertyName = "CustomerTeam";
            this.ColumnCustomerTeam.HeaderText = "부서";
            this.ColumnCustomerTeam.Name = "ColumnCustomerTeam";
            // 
            // ColumnAcceptDate
            // 
            this.ColumnAcceptDate.DataPropertyName = "AcceptTime";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = null;
            this.ColumnAcceptDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnAcceptDate.HeaderText = "배차일자";
            this.ColumnAcceptDate.Name = "ColumnAcceptDate";
            this.ColumnAcceptDate.ReadOnly = true;
            this.ColumnAcceptDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnAcceptDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnStartName
            // 
            this.ColumnStartName.DataPropertyName = "StartName";
            this.ColumnStartName.HeaderText = "상차지명";
            this.ColumnStartName.Name = "ColumnStartName";
            this.ColumnStartName.ReadOnly = true;
            this.ColumnStartName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnStopName
            // 
            this.ColumnStopName.DataPropertyName = "StopName";
            this.ColumnStopName.HeaderText = "하차지명";
            this.ColumnStopName.Name = "ColumnStopName";
            this.ColumnStopName.ReadOnly = true;
            // 
            // ColumnItem
            // 
            this.ColumnItem.DataPropertyName = "Item";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "N0";
            this.ColumnItem.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnItem.HeaderText = "품명";
            this.ColumnItem.Name = "ColumnItem";
            this.ColumnItem.ReadOnly = true;
            this.ColumnItem.Width = 70;
            // 
            // ColumnPayLocation
            // 
            this.ColumnPayLocation.DataPropertyName = "PayLocation";
            this.ColumnPayLocation.HeaderText = "구분";
            this.ColumnPayLocation.Name = "ColumnPayLocation";
            this.ColumnPayLocation.Width = 70;
            // 
            // ColumnDriverCarNo
            // 
            this.ColumnDriverCarNo.DataPropertyName = "DriverCarNo";
            this.ColumnDriverCarNo.HeaderText = "차량번호";
            this.ColumnDriverCarNo.Name = "ColumnDriverCarNo";
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "Price";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.Column6.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column6.HeaderText = "금액";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "VAT";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            this.Column7.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column7.HeaderText = "부가세";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "Amount";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            this.Column8.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column8.HeaderText = "합계";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 120;
            // 
            // CustomerSelect
            // 
            this.CustomerSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.CustomerSelect.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.CustomerSelect.FullRowSelect = true;
            this.CustomerSelect.GridLines = true;
            this.CustomerSelect.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.CustomerSelect.HideSelection = false;
            this.CustomerSelect.Location = new System.Drawing.Point(495, 32);
            this.CustomerSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CustomerSelect.MultiSelect = false;
            this.CustomerSelect.Name = "CustomerSelect";
            this.CustomerSelect.Size = new System.Drawing.Size(434, 165);
            this.CustomerSelect.TabIndex = 55;
            this.CustomerSelect.UseCompatibleStateImageBehavior = false;
            this.CustomerSelect.View = System.Windows.Forms.View.Details;
            this.CustomerSelect.Visible = false;
            this.CustomerSelect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CustomerSelect_KeyPress);
            this.CustomerSelect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CustomerSelect_MouseClick);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "";
            this.columnHeader9.Width = 150;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(963, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 6, 0, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 27);
            this.btnClose.TabIndex = 37;
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
            this.btnAddClose.Location = new System.Drawing.Point(858, 4);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 6, 0, 6);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 27);
            this.btnAddClose.TabIndex = 38;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기";
            this.btnAddClose.UseVisualStyleBackColor = false;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // customersTableAdapter
            // 
            this.customersTableAdapter.ClearBeforeFill = true;
            // 
            // clientDataSet
            // 
            this.clientDataSet.DataSetName = "ClientDataSet";
            this.clientDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAddClose);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 313);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1073, 39);
            this.panel3.TabIndex = 56;
            // 
            // customerTeamsBindingSource
            // 
            this.customerTeamsBindingSource.DataMember = "CustomerTeams";
            // 
            // customerTeamsTableAdapter
            // 
            this.customerTeamsTableAdapter.ClearBeforeFill = true;
            // 
            // customerUserDataSet
            // 
            this.customerUserDataSet.DataSetName = "CustomerUserDataSet";
            this.customerUserDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FrmMN0212_SALESMANAGE_ADD2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1073, 352);
            this.Controls.Add(this.CustomerSelect);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMN0212_SALESMANAGE_ADD2";
            this.Text = "배차 건 집계추가";
            this.Load += new System.EventHandler(this.FrmTradeNew2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customerTeamsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerUserDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.DateTimePicker dtp_To;
        private System.Windows.Forms.DateTimePicker dtp_From;
        private System.Windows.Forms.Button btn_Search;
        private NewDGV newDGV1;
        private System.Windows.Forms.CheckBox AllSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdb_Tax1;
        private System.Windows.Forms.RadioButton rdb_Tax0;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.ListView CustomerSelect;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        public System.Windows.Forms.ComboBox cmb_Search;
        private DataSets.ClientDataSetTableAdapters.CustomersTableAdapter customersTableAdapter;
        private DataSets.ClientDataSet clientDataSet;
        private System.Windows.Forms.ComboBox cmbSMonth;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cmbTeam;
        private System.Windows.Forms.BindingSource customerTeamsBindingSource;
        private DataSets.CustomerUserDataSetTableAdapters.CustomerTeamsTableAdapter customerTeamsTableAdapter;
        private DataSets.CustomerUserDataSet customerUserDataSet;
        private UI.DataGridViewDisableCheckBoxColumn ColumnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCustomerTeam;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAcceptDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStopName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPayLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDriverCarNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}