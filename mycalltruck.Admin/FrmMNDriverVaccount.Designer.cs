namespace mycalltruck.Admin
{
    partial class FrmMNDriverVaccount
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.cmbSSearch = new System.Windows.Forms.ComboBox();
            this.cmbSMonth = new System.Windows.Forms.ComboBox();
            this.txtSText = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp_Edate = new System.Windows.Forms.DateTimePicker();
            this.dtp_Sdate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.grid1 = new mycalltruck.Admin.NewDGV();
            this.rowNUMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chargeGubunDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accFunctionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lGDTIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lGDRESPCODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lGDRESPMSGDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverPointsListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pointsDataSet = new mycalltruck.Admin.DataSets.PointsDataSet();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAmout = new System.Windows.Forms.Label();
            this.lblCancle = new System.Windows.Forms.Label();
            this.lblApprove = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.driverPointsListTableAdapter = new mycalltruck.Admin.DataSets.PointsDataSetTableAdapters.DriverPointsListTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driverPointsListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsDataSet)).BeginInit();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Location = new System.Drawing.Point(661, 6);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 27);
            this.btnExcel.TabIndex = 66;
            this.btnExcel.TabStop = false;
            this.btnExcel.Tag = "Write";
            this.btnExcel.Text = "내보내기";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btn_Inew
            // 
            this.btn_Inew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Inew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Inew.FlatAppearance.BorderSize = 0;
            this.btn_Inew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Inew.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Inew.ForeColor = System.Drawing.Color.White;
            this.btn_Inew.Location = new System.Drawing.Point(1247, 7);
            this.btn_Inew.Name = "btn_Inew";
            this.btn_Inew.Size = new System.Drawing.Size(59, 27);
            this.btn_Inew.TabIndex = 56;
            this.btn_Inew.Text = "초기화";
            this.btn_Inew.UseVisualStyleBackColor = false;
            this.btn_Inew.Click += new System.EventHandler(this.btn_Inew_Click);
            // 
            // cmbSSearch
            // 
            this.cmbSSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSSearch.FormattingEnabled = true;
            this.cmbSSearch.Items.AddRange(new object[] {
            ""});
            this.cmbSSearch.Location = new System.Drawing.Point(1004, 8);
            this.cmbSSearch.Name = "cmbSSearch";
            this.cmbSSearch.Size = new System.Drawing.Size(80, 25);
            this.cmbSSearch.TabIndex = 54;
            // 
            // cmbSMonth
            // 
            this.cmbSMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSMonth.FormattingEnabled = true;
            this.cmbSMonth.Location = new System.Drawing.Point(740, 7);
            this.cmbSMonth.Name = "cmbSMonth";
            this.cmbSMonth.Size = new System.Drawing.Size(80, 25);
            this.cmbSMonth.TabIndex = 51;
            this.cmbSMonth.SelectedIndexChanged += new System.EventHandler(this.cmbSMonth_SelectedIndexChanged);
            // 
            // txtSText
            // 
            this.txtSText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSText.Location = new System.Drawing.Point(1092, 8);
            this.txtSText.Name = "txtSText";
            this.txtSText.Size = new System.Drawing.Size(100, 25);
            this.txtSText.TabIndex = 50;
            this.txtSText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSText_KeyUp);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(4, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(136, 19);
            this.label11.TabIndex = 48;
            this.label11.Text = "차주(카드) 충전내역";
            // 
            // dtp_Edate
            // 
            this.dtp_Edate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Edate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Edate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Edate.Location = new System.Drawing.Point(915, 7);
            this.dtp_Edate.Name = "dtp_Edate";
            this.dtp_Edate.Size = new System.Drawing.Size(83, 25);
            this.dtp_Edate.TabIndex = 46;
            // 
            // dtp_Sdate
            // 
            this.dtp_Sdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Sdate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Sdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Sdate.Location = new System.Drawing.Point(826, 7);
            this.dtp_Sdate.Name = "dtp_Sdate";
            this.dtp_Sdate.Size = new System.Drawing.Size(83, 25);
            this.dtp_Sdate.TabIndex = 45;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(1194, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(47, 27);
            this.btnSearch.TabIndex = 43;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.grid1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.AutoGenerateColumns = false;
            this.grid1.BackgroundColor = System.Drawing.Color.White;
            this.grid1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rowNUMDataGridViewTextBoxColumn,
            this.cDateDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.loginIdDataGridViewTextBoxColumn,
            this.carNoDataGridViewTextBoxColumn,
            this.carYearDataGridViewTextBoxColumn,
            this.cEODataGridViewTextBoxColumn,
            this.chargeGubunDataGridViewTextBoxColumn,
            this.accFunctionDataGridViewTextBoxColumn,
            this.lGDTIDDataGridViewTextBoxColumn,
            this.cardNoDataGridViewTextBoxColumn,
            this.lGDRESPCODEDataGridViewTextBoxColumn,
            this.lGDRESPMSGDataGridViewTextBoxColumn});
            this.grid1.DataSource = this.driverPointsListBindingSource;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid1.DefaultCellStyle = dataGridViewCellStyle9;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.GridColor = System.Drawing.Color.White;
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Margin = new System.Windows.Forms.Padding(0);
            this.grid1.MultiSelect = false;
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grid1.RowTemplate.Height = 23;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(1311, 540);
            this.grid1.TabIndex = 2;
            this.grid1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid1_CellFormatting);
            // 
            // rowNUMDataGridViewTextBoxColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rowNUMDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.rowNUMDataGridViewTextBoxColumn.HeaderText = "번호";
            this.rowNUMDataGridViewTextBoxColumn.Name = "rowNUMDataGridViewTextBoxColumn";
            this.rowNUMDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rowNUMDataGridViewTextBoxColumn.Width = 41;
            // 
            // cDateDataGridViewTextBoxColumn
            // 
            this.cDateDataGridViewTextBoxColumn.DataPropertyName = "CDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.cDateDataGridViewTextBoxColumn.HeaderText = "일시";
            this.cDateDataGridViewTextBoxColumn.Name = "cDateDataGridViewTextBoxColumn";
            this.cDateDataGridViewTextBoxColumn.Width = 170;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.amountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.amountDataGridViewTextBoxColumn.HeaderText = "금액";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            // 
            // loginIdDataGridViewTextBoxColumn
            // 
            this.loginIdDataGridViewTextBoxColumn.DataPropertyName = "LoginId";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.loginIdDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.loginIdDataGridViewTextBoxColumn.HeaderText = "아이디";
            this.loginIdDataGridViewTextBoxColumn.Name = "loginIdDataGridViewTextBoxColumn";
            // 
            // carNoDataGridViewTextBoxColumn
            // 
            this.carNoDataGridViewTextBoxColumn.DataPropertyName = "CarNo";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.carNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.carNoDataGridViewTextBoxColumn.HeaderText = "차량번호";
            this.carNoDataGridViewTextBoxColumn.Name = "carNoDataGridViewTextBoxColumn";
            // 
            // carYearDataGridViewTextBoxColumn
            // 
            this.carYearDataGridViewTextBoxColumn.DataPropertyName = "CarYear";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.carYearDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.carYearDataGridViewTextBoxColumn.HeaderText = "차주명";
            this.carYearDataGridViewTextBoxColumn.Name = "carYearDataGridViewTextBoxColumn";
            // 
            // cEODataGridViewTextBoxColumn
            // 
            this.cEODataGridViewTextBoxColumn.DataPropertyName = "CEO";
            this.cEODataGridViewTextBoxColumn.HeaderText = "CEO";
            this.cEODataGridViewTextBoxColumn.Name = "cEODataGridViewTextBoxColumn";
            this.cEODataGridViewTextBoxColumn.Visible = false;
            // 
            // chargeGubunDataGridViewTextBoxColumn
            // 
            this.chargeGubunDataGridViewTextBoxColumn.DataPropertyName = "ChargeGubun";
            this.chargeGubunDataGridViewTextBoxColumn.HeaderText = "충전수단";
            this.chargeGubunDataGridViewTextBoxColumn.Name = "chargeGubunDataGridViewTextBoxColumn";
            this.chargeGubunDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // accFunctionDataGridViewTextBoxColumn
            // 
            this.accFunctionDataGridViewTextBoxColumn.DataPropertyName = "AccFunction";
            this.accFunctionDataGridViewTextBoxColumn.HeaderText = "유형";
            this.accFunctionDataGridViewTextBoxColumn.Name = "accFunctionDataGridViewTextBoxColumn";
            this.accFunctionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lGDTIDDataGridViewTextBoxColumn
            // 
            this.lGDTIDDataGridViewTextBoxColumn.DataPropertyName = "LGD_TID";
            this.lGDTIDDataGridViewTextBoxColumn.HeaderText = "거래번호";
            this.lGDTIDDataGridViewTextBoxColumn.Name = "lGDTIDDataGridViewTextBoxColumn";
            this.lGDTIDDataGridViewTextBoxColumn.Width = 140;
            // 
            // cardNoDataGridViewTextBoxColumn
            // 
            this.cardNoDataGridViewTextBoxColumn.DataPropertyName = "CardNo";
            this.cardNoDataGridViewTextBoxColumn.HeaderText = "카드번호";
            this.cardNoDataGridViewTextBoxColumn.Name = "cardNoDataGridViewTextBoxColumn";
            this.cardNoDataGridViewTextBoxColumn.Width = 140;
            // 
            // lGDRESPCODEDataGridViewTextBoxColumn
            // 
            this.lGDRESPCODEDataGridViewTextBoxColumn.DataPropertyName = "LGD_RESPCODE";
            this.lGDRESPCODEDataGridViewTextBoxColumn.HeaderText = "코드";
            this.lGDRESPCODEDataGridViewTextBoxColumn.Name = "lGDRESPCODEDataGridViewTextBoxColumn";
            // 
            // lGDRESPMSGDataGridViewTextBoxColumn
            // 
            this.lGDRESPMSGDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lGDRESPMSGDataGridViewTextBoxColumn.DataPropertyName = "LGD_RESPMSG";
            this.lGDRESPMSGDataGridViewTextBoxColumn.HeaderText = "응답메세지";
            this.lGDRESPMSGDataGridViewTextBoxColumn.Name = "lGDRESPMSGDataGridViewTextBoxColumn";
            this.lGDRESPMSGDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // driverPointsListBindingSource
            // 
            this.driverPointsListBindingSource.DataMember = "DriverPointsList";
            this.driverPointsListBindingSource.DataSource = this.pointsDataSet;
            // 
            // pointsDataSet
            // 
            this.pointsDataSet.DataSetName = "PointsDataSet";
            this.pointsDataSet.EnforceConstraints = false;
            this.pointsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.grid1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1311, 540);
            this.panel3.TabIndex = 22;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1317, 614);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.lblAmout, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCancle, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblApprove, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 589);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1317, 25);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // lblAmout
            // 
            this.lblAmout.BackColor = System.Drawing.Color.White;
            this.lblAmout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAmout.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblAmout.Location = new System.Drawing.Point(1096, 1);
            this.lblAmout.Margin = new System.Windows.Forms.Padding(0);
            this.lblAmout.Name = "lblAmout";
            this.lblAmout.Size = new System.Drawing.Size(220, 23);
            this.lblAmout.TabIndex = 10;
            this.lblAmout.Text = "0";
            this.lblAmout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCancle
            // 
            this.lblCancle.BackColor = System.Drawing.Color.White;
            this.lblCancle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCancle.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblCancle.Location = new System.Drawing.Point(658, 1);
            this.lblCancle.Margin = new System.Windows.Forms.Padding(0);
            this.lblCancle.Name = "lblCancle";
            this.lblCancle.Size = new System.Drawing.Size(218, 23);
            this.lblCancle.TabIndex = 9;
            this.lblCancle.Text = "0";
            this.lblCancle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblApprove
            // 
            this.lblApprove.BackColor = System.Drawing.Color.White;
            this.lblApprove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApprove.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblApprove.Location = new System.Drawing.Point(220, 1);
            this.lblApprove.Margin = new System.Windows.Forms.Padding(0);
            this.lblApprove.Name = "lblApprove";
            this.lblApprove.Size = new System.Drawing.Size(218, 23);
            this.lblApprove.TabIndex = 8;
            this.lblApprove.Text = "0";
            this.lblApprove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "승인합계";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(439, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "취소합계";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(877, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "입금액";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.btn_Inew);
            this.panel1.Controls.Add(this.cmbSSearch);
            this.panel1.Controls.Add(this.cmbSMonth);
            this.panel1.Controls.Add(this.txtSText);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.dtp_Edate);
            this.panel1.Controls.Add(this.dtp_Sdate);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1317, 43);
            this.panel1.TabIndex = 21;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1319, 616);
            this.panel2.TabIndex = 3;
            // 
            // driverPointsListTableAdapter
            // 
            this.driverPointsListTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMNDriverVaccount
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1319, 616);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmMNDriverVaccount";
            this.Text = "차주(카드) 충전내역)";
            this.Load += new System.EventHandler(this.FrmMNDriverVaccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driverPointsListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsDataSet)).EndInit();
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExcel;
        public System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.ComboBox cmbSSearch;
        private System.Windows.Forms.ComboBox cmbSMonth;
        public System.Windows.Forms.TextBox txtSText;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp_Edate;
        private System.Windows.Forms.DateTimePicker dtp_Sdate;
        public System.Windows.Forms.Button btnSearch;
        public NewDGV grid1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblAmout;
        private System.Windows.Forms.Label lblCancle;
        private System.Windows.Forms.Label lblApprove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DataSets.PointsDataSet pointsDataSet;
        private System.Windows.Forms.BindingSource driverPointsListBindingSource;
        private DataSets.PointsDataSetTableAdapters.DriverPointsListTableAdapter driverPointsListTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNUMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chargeGubunDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accFunctionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lGDTIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lGDRESPCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lGDRESPMSGDataGridViewTextBoxColumn;
    }
}