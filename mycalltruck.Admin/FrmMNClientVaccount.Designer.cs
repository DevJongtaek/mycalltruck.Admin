namespace mycalltruck.Admin
{
    partial class FrmMNClientVaccount
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grid1 = new mycalltruck.Admin.NewDGV();
            this.rowNUMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vBankNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vAccountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEtaxWriteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEtaxCancle = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clientPointsListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pointsDataSet = new mycalltruck.Admin.DataSets.PointsDataSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnEtax = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.cmbSSearch = new System.Windows.Forms.ComboBox();
            this.cmbSMonth = new System.Windows.Forms.ComboBox();
            this.txtSText = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp_Edate = new System.Windows.Forms.DateTimePicker();
            this.dtp_Sdate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.clientPointsListTableAdapter = new mycalltruck.Admin.DataSets.PointsDataSetTableAdapters.ClientPointsListTableAdapter();
            this.clientDataSet = new mycalltruck.Admin.DataSets.ClientDataSet();
            this.clientsTableAdapter = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.ClientsTableAdapter();
            this.baseDataSet = new mycalltruck.Admin.DataSets.BaseDataSet();
            this.AdminInfoesTableAdapter = new mycalltruck.Admin.DataSets.BaseDataSetTableAdapters.AdminInfoesTableAdapter();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientPointsListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).BeginInit();
            this.SuspendLayout();
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
            this.panel2.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1317, 614);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.grid1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1311, 565);
            this.panel3.TabIndex = 22;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            dataGridViewCellStyle28.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle28.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.Color.White;
            this.grid1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle28;
            this.grid1.AutoGenerateColumns = false;
            this.grid1.BackgroundColor = System.Drawing.Color.White;
            this.grid1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle29;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rowNUMDataGridViewTextBoxColumn,
            this.cDateDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.bizNoDataGridViewTextBoxColumn,
            this.cEODataGridViewTextBoxColumn,
            this.vBankNameDataGridViewTextBoxColumn,
            this.vAccountDataGridViewTextBoxColumn,
            this.ColumnEtaxWriteDate,
            this.ColumnEtaxCancle});
            this.grid1.DataSource = this.clientPointsListBindingSource;
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle36.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle36.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle36.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle36.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle36.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid1.DefaultCellStyle = dataGridViewCellStyle36;
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
            this.grid1.Size = new System.Drawing.Size(1311, 565);
            this.grid1.TabIndex = 2;
            this.grid1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid1_CellContentClick);
            this.grid1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid1_CellFormatting);
            this.grid1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid1_CellPainting);
            // 
            // rowNUMDataGridViewTextBoxColumn
            // 
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rowNUMDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle30;
            this.rowNUMDataGridViewTextBoxColumn.HeaderText = "번호";
            this.rowNUMDataGridViewTextBoxColumn.Name = "rowNUMDataGridViewTextBoxColumn";
            this.rowNUMDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rowNUMDataGridViewTextBoxColumn.Width = 41;
            // 
            // cDateDataGridViewTextBoxColumn
            // 
            this.cDateDataGridViewTextBoxColumn.DataPropertyName = "CDate";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle31;
            this.cDateDataGridViewTextBoxColumn.HeaderText = "일시";
            this.cDateDataGridViewTextBoxColumn.Name = "cDateDataGridViewTextBoxColumn";
            this.cDateDataGridViewTextBoxColumn.Width = 200;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle32.Format = "N0";
            dataGridViewCellStyle32.NullValue = null;
            this.amountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle32;
            this.amountDataGridViewTextBoxColumn.HeaderText = "금액";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.codeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle33;
            this.codeDataGridViewTextBoxColumn.HeaderText = "코드";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "상호";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 150;
            // 
            // bizNoDataGridViewTextBoxColumn
            // 
            this.bizNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.bizNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle34;
            this.bizNoDataGridViewTextBoxColumn.HeaderText = "사업자번호";
            this.bizNoDataGridViewTextBoxColumn.Name = "bizNoDataGridViewTextBoxColumn";
            this.bizNoDataGridViewTextBoxColumn.Width = 150;
            // 
            // cEODataGridViewTextBoxColumn
            // 
            this.cEODataGridViewTextBoxColumn.DataPropertyName = "CEO";
            this.cEODataGridViewTextBoxColumn.HeaderText = "대표자";
            this.cEODataGridViewTextBoxColumn.Name = "cEODataGridViewTextBoxColumn";
            // 
            // vBankNameDataGridViewTextBoxColumn
            // 
            this.vBankNameDataGridViewTextBoxColumn.DataPropertyName = "VBankName";
            this.vBankNameDataGridViewTextBoxColumn.HeaderText = "은행명";
            this.vBankNameDataGridViewTextBoxColumn.Name = "vBankNameDataGridViewTextBoxColumn";
            // 
            // vAccountDataGridViewTextBoxColumn
            // 
            this.vAccountDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.vAccountDataGridViewTextBoxColumn.DataPropertyName = "VAccount";
            this.vAccountDataGridViewTextBoxColumn.HeaderText = "가상계좌번호";
            this.vAccountDataGridViewTextBoxColumn.Name = "vAccountDataGridViewTextBoxColumn";
            // 
            // ColumnEtaxWriteDate
            // 
            this.ColumnEtaxWriteDate.DataPropertyName = "EtaxWriteDate";
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnEtaxWriteDate.DefaultCellStyle = dataGridViewCellStyle35;
            this.ColumnEtaxWriteDate.HeaderText = "계산서발행일";
            this.ColumnEtaxWriteDate.Name = "ColumnEtaxWriteDate";
            this.ColumnEtaxWriteDate.Width = 110;
            // 
            // ColumnEtaxCancle
            // 
            this.ColumnEtaxCancle.HeaderText = "발행취소";
            this.ColumnEtaxCancle.Name = "ColumnEtaxCancle";
            this.ColumnEtaxCancle.Text = "발행취소";
            this.ColumnEtaxCancle.UseColumnTextForButtonValue = true;
            // 
            // clientPointsListBindingSource
            // 
            this.clientPointsListBindingSource.DataMember = "ClientPointsList";
            this.clientPointsListBindingSource.DataSource = this.pointsDataSet;
            // 
            // pointsDataSet
            // 
            this.pointsDataSet.DataSetName = "PointsDataSet";
            this.pointsDataSet.EnforceConstraints = false;
            this.pointsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Controls.Add(this.btnEtax);
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
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1317, 43);
            this.panel1.TabIndex = 21;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(330, 11);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(78, 20);
            this.webBrowser1.TabIndex = 106;
            this.webBrowser1.Url = new System.Uri("http://222.231.9.253/NiceEncoding.asp", System.UriKind.Absolute);
            this.webBrowser1.Visible = false;
            // 
            // btnEtax
            // 
            this.btnEtax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEtax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnEtax.FlatAppearance.BorderSize = 0;
            this.btnEtax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEtax.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEtax.ForeColor = System.Drawing.Color.White;
            this.btnEtax.Location = new System.Drawing.Point(553, 7);
            this.btnEtax.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnEtax.Name = "btnEtax";
            this.btnEtax.Size = new System.Drawing.Size(102, 27);
            this.btnEtax.TabIndex = 88;
            this.btnEtax.TabStop = false;
            this.btnEtax.Text = "전자세금발행";
            this.btnEtax.UseVisualStyleBackColor = false;
            this.btnEtax.Click += new System.EventHandler(this.btnEtax_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Location = new System.Drawing.Point(659, 7);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(69, 27);
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
            this.btn_Inew.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Inew.ForeColor = System.Drawing.Color.White;
            this.btn_Inew.Location = new System.Drawing.Point(1247, 8);
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
            this.cmbSSearch.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbSSearch.FormattingEnabled = true;
            this.cmbSSearch.Items.AddRange(new object[] {
            ""});
            this.cmbSSearch.Location = new System.Drawing.Point(1004, 9);
            this.cmbSSearch.Name = "cmbSSearch";
            this.cmbSSearch.Size = new System.Drawing.Size(80, 25);
            this.cmbSSearch.TabIndex = 54;
            // 
            // cmbSMonth
            // 
            this.cmbSMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSMonth.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbSMonth.FormattingEnabled = true;
            this.cmbSMonth.Location = new System.Drawing.Point(734, 8);
            this.cmbSMonth.Name = "cmbSMonth";
            this.cmbSMonth.Size = new System.Drawing.Size(80, 25);
            this.cmbSMonth.TabIndex = 51;
            this.cmbSMonth.SelectedIndexChanged += new System.EventHandler(this.cmbSMonth_SelectedIndexChanged);
            // 
            // txtSText
            // 
            this.txtSText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSText.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtSText.Location = new System.Drawing.Point(1092, 9);
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
            this.label11.Size = new System.Drawing.Size(178, 19);
            this.label11.TabIndex = 48;
            this.label11.Text = "주선사(가상계좌) 충전내역";
            // 
            // dtp_Edate
            // 
            this.dtp_Edate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Edate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Edate.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dtp_Edate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Edate.Location = new System.Drawing.Point(909, 8);
            this.dtp_Edate.Name = "dtp_Edate";
            this.dtp_Edate.Size = new System.Drawing.Size(90, 25);
            this.dtp_Edate.TabIndex = 46;
            // 
            // dtp_Sdate
            // 
            this.dtp_Sdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Sdate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Sdate.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dtp_Sdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Sdate.Location = new System.Drawing.Point(820, 8);
            this.dtp_Sdate.Name = "dtp_Sdate";
            this.dtp_Sdate.Size = new System.Drawing.Size(90, 25);
            this.dtp_Sdate.TabIndex = 45;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(1194, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(47, 27);
            this.btnSearch.TabIndex = 43;
            this.btnSearch.Text = "조 회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // clientPointsListTableAdapter
            // 
            this.clientPointsListTableAdapter.ClearBeforeFill = true;
            // 
            // clientDataSet
            // 
            this.clientDataSet.DataSetName = "ClientDataSet";
            this.clientDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // clientsTableAdapter
            // 
            this.clientsTableAdapter.ClearBeforeFill = true;
            // 
            // baseDataSet
            // 
            this.baseDataSet.DataSetName = "BaseDataSet";
            this.baseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // AdminInfoesTableAdapter
            // 
            this.AdminInfoesTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMNClientVaccount
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1319, 616);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "FrmMNClientVaccount";
            this.Text = "주선사(가상계좌) 충전내역";
            this.Load += new System.EventHandler(this.FrmMNClientVaccount_Load);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientPointsListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel3;
        public NewDGV grid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExcel;
        public System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.ComboBox cmbSSearch;
        private System.Windows.Forms.ComboBox cmbSMonth;
        public System.Windows.Forms.TextBox txtSText;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp_Edate;
        private System.Windows.Forms.DateTimePicker dtp_Sdate;
        public System.Windows.Forms.Button btnSearch;
        private DataSets.PointsDataSet pointsDataSet;
        private System.Windows.Forms.BindingSource clientPointsListBindingSource;
        private DataSets.PointsDataSetTableAdapters.ClientPointsListTableAdapter clientPointsListTableAdapter;
        private System.Windows.Forms.Button btnEtax;
        private DataSets.ClientDataSet clientDataSet;
        private DataSets.ClientDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNUMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vBankNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vAccountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEtaxWriteDate;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnEtaxCancle;
        private DataSets.BaseDataSet baseDataSet;
        private DataSets.BaseDataSetTableAdapters.AdminInfoesTableAdapter AdminInfoesTableAdapter;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}