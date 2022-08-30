namespace mycalltruck.Admin
{
    partial class UCAccount01
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CustomerSelect = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DriverSelect = new System.Windows.Forms.ListView();
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.Div = new System.Windows.Forms.ComboBox();
            this.Input = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp_Edate = new System.Windows.Forms.DateTimePicker();
            this.dtp_Sdate = new System.Windows.Forms.DateTimePicker();
            this.btn_Search = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ParentDataGrid = new mycalltruck.Admin.NewDGV();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelDataGrid = new mycalltruck.Admin.NewDGV();
            this.ColumnOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRequestAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPayedAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHasImage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHasParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIssueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPayedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Misu = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.PaySum = new System.Windows.Forms.TextBox();
            this.PaySumLabel = new System.Windows.Forms.Label();
            this.RequestSum = new System.Windows.Forms.TextBox();
            this.RequestSumLable = new System.Windows.Forms.Label();
            this.stats1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.stats1TableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.Stats1TableAdapter();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParentDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stats1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomerSelect
            // 
            this.CustomerSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomerSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.CustomerSelect.FullRowSelect = true;
            this.CustomerSelect.GridLines = true;
            this.CustomerSelect.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.CustomerSelect.Location = new System.Drawing.Point(411, 34);
            this.CustomerSelect.MultiSelect = false;
            this.CustomerSelect.Name = "CustomerSelect";
            this.CustomerSelect.Size = new System.Drawing.Size(387, 133);
            this.CustomerSelect.TabIndex = 22;
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
            // DriverSelect
            // 
            this.DriverSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DriverSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15});
            this.DriverSelect.FullRowSelect = true;
            this.DriverSelect.GridLines = true;
            this.DriverSelect.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.DriverSelect.Location = new System.Drawing.Point(358, 34);
            this.DriverSelect.MultiSelect = false;
            this.DriverSelect.Name = "DriverSelect";
            this.DriverSelect.Size = new System.Drawing.Size(440, 133);
            this.DriverSelect.TabIndex = 23;
            this.DriverSelect.UseCompatibleStateImageBehavior = false;
            this.DriverSelect.View = System.Windows.Forms.View.Details;
            this.DriverSelect.Visible = false;
            this.DriverSelect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DriverSelect_KeyPress);
            this.DriverSelect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DriverSelect_MouseClick);
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "";
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "";
            this.columnHeader12.Width = 40;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Width = 100;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Width = 100;
            // 
            // columnHeader15
            // 
            this.columnHeader15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader15.Width = 70;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Div);
            this.panel1.Controls.Add(this.Input);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.dtp_Edate);
            this.panel1.Controls.Add(this.dtp_Sdate);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 38);
            this.panel1.TabIndex = 20;
            // 
            // Div
            // 
            this.Div.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Div.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Div.FormattingEnabled = true;
            this.Div.Items.AddRange(new object[] {
            "거래처",
            "차량"});
            this.Div.Location = new System.Drawing.Point(374, 8);
            this.Div.Name = "Div";
            this.Div.Size = new System.Drawing.Size(80, 20);
            this.Div.TabIndex = 51;
            // 
            // Input
            // 
            this.Input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Input.Location = new System.Drawing.Point(460, 8);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(100, 21);
            this.Input.TabIndex = 50;
            this.Input.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Input_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label11.Location = new System.Drawing.Point(12, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 16);
            this.label11.TabIndex = 48;
            this.label11.Text = "[정산관리]";
            // 
            // dtp_Edate
            // 
            this.dtp_Edate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Edate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Edate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Edate.Location = new System.Drawing.Point(655, 8);
            this.dtp_Edate.Name = "dtp_Edate";
            this.dtp_Edate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Edate.TabIndex = 46;
            // 
            // dtp_Sdate
            // 
            this.dtp_Sdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Sdate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Sdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Sdate.Location = new System.Drawing.Point(566, 8);
            this.dtp_Sdate.Name = "dtp_Sdate";
            this.dtp_Sdate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Sdate.TabIndex = 45;
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.Location = new System.Drawing.Point(744, 7);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Search.Size = new System.Drawing.Size(56, 23);
            this.btn_Search.TabIndex = 43;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(806, 543);
            this.panel2.TabIndex = 21;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ParentDataGrid, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ModelDataGrid, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(806, 543);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ParentDataGrid
            // 
            this.ParentDataGrid.AllowUserToAddRows = false;
            this.ParentDataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.ParentDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ParentDataGrid.BackgroundColor = System.Drawing.Color.White;
            this.ParentDataGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ParentDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ParentDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ParentDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ParentDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ParentDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDate,
            this.ColumnAmount});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ParentDataGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.ParentDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParentDataGrid.GridColor = System.Drawing.Color.White;
            this.ParentDataGrid.Location = new System.Drawing.Point(0, 0);
            this.ParentDataGrid.Margin = new System.Windows.Forms.Padding(0);
            this.ParentDataGrid.MultiSelect = false;
            this.ParentDataGrid.Name = "ParentDataGrid";
            this.ParentDataGrid.RowHeadersVisible = false;
            this.ParentDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ParentDataGrid.RowTemplate.Height = 23;
            this.ParentDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ParentDataGrid.Size = new System.Drawing.Size(250, 503);
            this.ParentDataGrid.TabIndex = 0;
            this.ParentDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ParentDataGrid_CellDoubleClick);
            // 
            // ColumnDate
            // 
            this.ColumnDate.DataPropertyName = "Date";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "yyyy-MM-dd";
            this.ColumnDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnDate.HeaderText = "일자";
            this.ColumnDate.Name = "ColumnDate";
            this.ColumnDate.ReadOnly = true;
            this.ColumnDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnAmount
            // 
            this.ColumnAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnAmount.DataPropertyName = "Amount";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            this.ColumnAmount.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnAmount.HeaderText = "발행금액";
            this.ColumnAmount.Name = "ColumnAmount";
            this.ColumnAmount.ReadOnly = true;
            this.ColumnAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ModelDataGrid
            // 
            this.ModelDataGrid.AllowUserToAddRows = false;
            this.ModelDataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.ModelDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.ModelDataGrid.BackgroundColor = System.Drawing.Color.White;
            this.ModelDataGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ModelDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModelDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ModelDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.ModelDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ModelDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnOrderDate,
            this.ColumnRequestAmount,
            this.ColumnPayedAmount,
            this.ColumnHasImage,
            this.ColumnHasParent,
            this.ColumnIssueDate,
            this.ColumnPayedDate});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ModelDataGrid.DefaultCellStyle = dataGridViewCellStyle15;
            this.ModelDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModelDataGrid.GridColor = System.Drawing.Color.White;
            this.ModelDataGrid.Location = new System.Drawing.Point(254, 0);
            this.ModelDataGrid.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.ModelDataGrid.MultiSelect = false;
            this.ModelDataGrid.Name = "ModelDataGrid";
            this.ModelDataGrid.RowHeadersVisible = false;
            this.ModelDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ModelDataGrid.RowTemplate.Height = 23;
            this.ModelDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ModelDataGrid.Size = new System.Drawing.Size(552, 503);
            this.ModelDataGrid.TabIndex = 1;
            this.ModelDataGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ModelDataGrid_CellFormatting);
            // 
            // ColumnOrderDate
            // 
            this.ColumnOrderDate.DataPropertyName = "OrderDate";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Format = "yyyy-MM-dd";
            this.ColumnOrderDate.DefaultCellStyle = dataGridViewCellStyle8;
            this.ColumnOrderDate.HeaderText = "배차일자";
            this.ColumnOrderDate.Name = "ColumnOrderDate";
            this.ColumnOrderDate.ReadOnly = true;
            this.ColumnOrderDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnRequestAmount
            // 
            this.ColumnRequestAmount.DataPropertyName = "RequestAmount";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            this.ColumnRequestAmount.DefaultCellStyle = dataGridViewCellStyle9;
            this.ColumnRequestAmount.HeaderText = "청구금액";
            this.ColumnRequestAmount.Name = "ColumnRequestAmount";
            this.ColumnRequestAmount.ReadOnly = true;
            this.ColumnRequestAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnPayedAmount
            // 
            this.ColumnPayedAmount.DataPropertyName = "PayedAmount";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N0";
            this.ColumnPayedAmount.DefaultCellStyle = dataGridViewCellStyle10;
            this.ColumnPayedAmount.HeaderText = "결제금액";
            this.ColumnPayedAmount.Name = "ColumnPayedAmount";
            this.ColumnPayedAmount.ReadOnly = true;
            this.ColumnPayedAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnHasImage
            // 
            this.ColumnHasImage.DataPropertyName = "HasImage";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHasImage.DefaultCellStyle = dataGridViewCellStyle11;
            this.ColumnHasImage.HeaderText = "인수증";
            this.ColumnHasImage.Name = "ColumnHasImage";
            this.ColumnHasImage.ReadOnly = true;
            this.ColumnHasImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnHasParent
            // 
            this.ColumnHasParent.DataPropertyName = "HasParent";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHasParent.DefaultCellStyle = dataGridViewCellStyle12;
            this.ColumnHasParent.HeaderText = "계산서발행";
            this.ColumnHasParent.Name = "ColumnHasParent";
            this.ColumnHasParent.ReadOnly = true;
            this.ColumnHasParent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnIssueDate
            // 
            this.ColumnIssueDate.DataPropertyName = "IssuedDate";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Format = "yyyy-MM-dd";
            this.ColumnIssueDate.DefaultCellStyle = dataGridViewCellStyle13;
            this.ColumnIssueDate.HeaderText = "계산서발행일자";
            this.ColumnIssueDate.Name = "ColumnIssueDate";
            this.ColumnIssueDate.ReadOnly = true;
            this.ColumnIssueDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnIssueDate.Width = 120;
            // 
            // ColumnPayedDate
            // 
            this.ColumnPayedDate.DataPropertyName = "PayedDate";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.Format = "yyyy-MM-dd";
            this.ColumnPayedDate.DefaultCellStyle = dataGridViewCellStyle14;
            this.ColumnPayedDate.HeaderText = "결제일자";
            this.ColumnPayedDate.Name = "ColumnPayedDate";
            this.ColumnPayedDate.ReadOnly = true;
            this.ColumnPayedDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnPayedDate.Width = 250;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Misu);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 506);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(244, 34);
            this.panel3.TabIndex = 2;
            // 
            // Misu
            // 
            this.Misu.Location = new System.Drawing.Point(109, 6);
            this.Misu.Name = "Misu";
            this.Misu.ReadOnly = true;
            this.Misu.Size = new System.Drawing.Size(132, 21);
            this.Misu.TabIndex = 1;
            this.Misu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "미수/미지급금";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.PaySum);
            this.panel5.Controls.Add(this.PaySumLabel);
            this.panel5.Controls.Add(this.RequestSum);
            this.panel5.Controls.Add(this.RequestSumLable);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(253, 506);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(550, 34);
            this.panel5.TabIndex = 3;
            // 
            // PaySum
            // 
            this.PaySum.Location = new System.Drawing.Point(352, 6);
            this.PaySum.Name = "PaySum";
            this.PaySum.ReadOnly = true;
            this.PaySum.Size = new System.Drawing.Size(132, 21);
            this.PaySum.TabIndex = 5;
            this.PaySum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // PaySumLabel
            // 
            this.PaySumLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PaySumLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaySumLabel.Location = new System.Drawing.Point(246, 5);
            this.PaySumLabel.Name = "PaySumLabel";
            this.PaySumLabel.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.PaySumLabel.Size = new System.Drawing.Size(100, 23);
            this.PaySumLabel.TabIndex = 4;
            this.PaySumLabel.Text = "결제금액 소계";
            this.PaySumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RequestSum
            // 
            this.RequestSum.Location = new System.Drawing.Point(109, 6);
            this.RequestSum.Name = "RequestSum";
            this.RequestSum.ReadOnly = true;
            this.RequestSum.Size = new System.Drawing.Size(132, 21);
            this.RequestSum.TabIndex = 3;
            this.RequestSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RequestSumLable
            // 
            this.RequestSumLable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RequestSumLable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RequestSumLable.Location = new System.Drawing.Point(3, 5);
            this.RequestSumLable.Name = "RequestSumLable";
            this.RequestSumLable.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.RequestSumLable.Size = new System.Drawing.Size(100, 23);
            this.RequestSumLable.TabIndex = 2;
            this.RequestSumLable.Text = "청구금액 소계";
            this.RequestSumLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stats1BindingSource
            // 
            this.stats1BindingSource.DataMember = "Stats1";
            this.stats1BindingSource.DataSource = this.cMDataSet;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // stats1TableAdapter
            // 
            this.stats1TableAdapter.ClearBeforeFill = true;
            // 
            // UCAccount01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.CustomerSelect);
            this.Controls.Add(this.DriverSelect);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UCAccount01";
            this.Size = new System.Drawing.Size(806, 581);
            this.Load += new System.EventHandler(this.UCAcoount01_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ParentDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stats1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView CustomerSelect;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ListView DriverSelect;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox Div;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp_Edate;
        private System.Windows.Forms.DateTimePicker dtp_Sdate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAmount;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox Misu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox PaySum;
        private System.Windows.Forms.Label PaySumLabel;
        private System.Windows.Forms.TextBox RequestSum;
        private System.Windows.Forms.Label RequestSumLable;
        private System.Windows.Forms.BindingSource stats1BindingSource;
        private CMDataSet cMDataSet;
        private CMDataSetTableAdapters.Stats1TableAdapter stats1TableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRequestAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPayedAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHasImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHasParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIssueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPayedDate;
        public System.Windows.Forms.TextBox Input;
        public System.Windows.Forms.Button btn_Search;
        public NewDGV ParentDataGrid;
        public NewDGV ModelDataGrid;
    }
}
