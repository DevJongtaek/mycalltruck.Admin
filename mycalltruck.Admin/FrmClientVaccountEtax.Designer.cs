namespace mycalltruck.Admin
{
    partial class FrmClientVaccountEtax
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdb_Tax0 = new System.Windows.Forms.RadioButton();
            this.rdb_Tax1 = new System.Windows.Forms.RadioButton();
            this.rdoSplit = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.lbl_Tax = new System.Windows.Forms.Label();
            this.btn_Search = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AllSelect = new System.Windows.Forms.CheckBox();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.ColumnSelect = new mycalltruck.Admin.UI.DataGridViewDisableCheckBoxColumn();
            this.ColumnNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBizNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCeo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.pointsDataSet = new mycalltruck.Admin.DataSets.PointsDataSet();
            this.clientPointsListTableAdapter = new mycalltruck.Admin.DataSets.PointsDataSetTableAdapters.ClientPointsListTableAdapter();
            this.clientDataSet = new mycalltruck.Admin.DataSets.ClientDataSet();
            this.clientsTableAdapter = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.ClientsTableAdapter();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.dtpEtaxDate = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).BeginInit();
            this.pnProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpEtaxDate);
            this.panel1.Controls.Add(this.txt_Search);
            this.panel1.Controls.Add(this.cmb_Search);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.rdoSplit);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.lbl_Tax);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(758, 43);
            this.panel1.TabIndex = 0;
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Search.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Search.Location = new System.Drawing.Point(431, 8);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(87, 25);
            this.txt_Search.TabIndex = 54;
            this.txt_Search.TabStop = false;
            this.txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyUp);
            // 
            // cmb_Search
            // 
            this.cmb_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Search.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Items.AddRange(new object[] {
            "전체",
            "사업자번호",
            "상호",
            "대표자"});
            this.cmb_Search.Location = new System.Drawing.Point(295, 9);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(130, 25);
            this.cmb_Search.TabIndex = 53;
            this.cmb_Search.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.rdb_Tax0);
            this.panel3.Controls.Add(this.rdb_Tax1);
            this.panel3.Enabled = false;
            this.panel3.Location = new System.Drawing.Point(654, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(100, 31);
            this.panel3.TabIndex = 52;
            // 
            // rdb_Tax0
            // 
            this.rdb_Tax0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Tax0.AutoSize = true;
            this.rdb_Tax0.Enabled = false;
            this.rdb_Tax0.Location = new System.Drawing.Point(1, 8);
            this.rdb_Tax0.Name = "rdb_Tax0";
            this.rdb_Tax0.Size = new System.Drawing.Size(49, 19);
            this.rdb_Tax0.TabIndex = 47;
            this.rdb_Tax0.Text = "별도";
            this.rdb_Tax0.UseVisualStyleBackColor = true;
            this.rdb_Tax0.CheckedChanged += new System.EventHandler(this.rdb_Tax0_CheckedChanged);
            // 
            // rdb_Tax1
            // 
            this.rdb_Tax1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Tax1.AutoSize = true;
            this.rdb_Tax1.Checked = true;
            this.rdb_Tax1.Enabled = false;
            this.rdb_Tax1.Location = new System.Drawing.Point(50, 8);
            this.rdb_Tax1.Name = "rdb_Tax1";
            this.rdb_Tax1.Size = new System.Drawing.Size(49, 19);
            this.rdb_Tax1.TabIndex = 48;
            this.rdb_Tax1.TabStop = true;
            this.rdb_Tax1.Text = "포함";
            this.rdb_Tax1.UseVisualStyleBackColor = true;
            // 
            // rdoSplit
            // 
            this.rdoSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoSplit.AutoSize = true;
            this.rdoSplit.Checked = true;
            this.rdoSplit.Location = new System.Drawing.Point(228, 11);
            this.rdoSplit.Name = "rdoSplit";
            this.rdoSplit.Size = new System.Drawing.Size(61, 19);
            this.rdoSplit.TabIndex = 51;
            this.rdoSplit.TabStop = true;
            this.rdoSplit.Text = "개별건";
            this.rdoSplit.UseVisualStyleBackColor = true;
            this.rdoSplit.CheckedChanged += new System.EventHandler(this.rdoSplit_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(157, 11);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(73, 19);
            this.radioButton2.TabIndex = 50;
            this.radioButton2.Text = "개별합산";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // lbl_Tax
            // 
            this.lbl_Tax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Tax.AutoSize = true;
            this.lbl_Tax.Location = new System.Drawing.Point(611, 13);
            this.lbl_Tax.Name = "lbl_Tax";
            this.lbl_Tax.Size = new System.Drawing.Size(36, 15);
            this.lbl_Tax.TabIndex = 49;
            this.lbl_Tax.Text = "VAT :";
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Search.FlatAppearance.BorderSize = 0;
            this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Search.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(524, 8);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(48, 27);
            this.btn_Search.TabIndex = 46;
            this.btn_Search.TabStop = false;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.AllSelect);
            this.panel2.Controls.Add(this.newDGV1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(758, 213);
            this.panel2.TabIndex = 1;
            // 
            // AllSelect
            // 
            this.AllSelect.AutoSize = true;
            this.AllSelect.Location = new System.Drawing.Point(41, 3);
            this.AllSelect.Name = "AllSelect";
            this.AllSelect.Size = new System.Drawing.Size(15, 14);
            this.AllSelect.TabIndex = 1;
            this.AllSelect.UseVisualStyleBackColor = true;
            this.AllSelect.CheckedChanged += new System.EventHandler(this.AllSelect_CheckedChanged);
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
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
            this.ColumnBizNo,
            this.ColumnName,
            this.ColumnCeo,
            this.ColumnPrice,
            this.ColumnVAT,
            this.ColumnAmount});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle9;
            this.newDGV1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(0, 0);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.MultiSelect = false;
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newDGV1.Size = new System.Drawing.Size(758, 213);
            this.newDGV1.TabIndex = 0;
            this.newDGV1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newDGV1_CellContentClick);
            this.newDGV1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newDGV1_CellDoubleClick);
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            this.newDGV1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.newDGV1_CellPainting);
            this.newDGV1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.newDGV1_DataError);
            // 
            // ColumnSelect
            // 
            this.ColumnSelect.DataPropertyName = "Selected";
            this.ColumnSelect.FalseValue = "";
            this.ColumnSelect.HeaderText = "선택   ";
            this.ColumnSelect.Name = "ColumnSelect";
            this.ColumnSelect.ReadOnly = true;
            this.ColumnSelect.TrueValue = "";
            this.ColumnSelect.Width = 60;
            // 
            // ColumnNumber
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "N0";
            this.ColumnNumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnNumber.HeaderText = "번호";
            this.ColumnNumber.Name = "ColumnNumber";
            this.ColumnNumber.ReadOnly = true;
            this.ColumnNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnNumber.Width = 40;
            // 
            // ColumnBizNo
            // 
            this.ColumnBizNo.DataPropertyName = "BizNo";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnBizNo.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnBizNo.HeaderText = "사업자번호";
            this.ColumnBizNo.Name = "ColumnBizNo";
            this.ColumnBizNo.Width = 110;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            this.ColumnName.HeaderText = "상호";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.Width = 130;
            // 
            // ColumnCeo
            // 
            this.ColumnCeo.DataPropertyName = "Ceo";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnCeo.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnCeo.HeaderText = "대표자";
            this.ColumnCeo.Name = "ColumnCeo";
            this.ColumnCeo.Width = 80;
            // 
            // ColumnPrice
            // 
            this.ColumnPrice.DataPropertyName = "Price";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.ColumnPrice.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnPrice.HeaderText = "금액";
            this.ColumnPrice.Name = "ColumnPrice";
            this.ColumnPrice.ReadOnly = true;
            this.ColumnPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnPrice.Width = 90;
            // 
            // ColumnVAT
            // 
            this.ColumnVAT.DataPropertyName = "VAT";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            this.ColumnVAT.DefaultCellStyle = dataGridViewCellStyle7;
            this.ColumnVAT.HeaderText = "부가세";
            this.ColumnVAT.Name = "ColumnVAT";
            this.ColumnVAT.ReadOnly = true;
            this.ColumnVAT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnVAT.Width = 90;
            // 
            // ColumnAmount
            // 
            this.ColumnAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnAmount.DataPropertyName = "Amount";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            this.ColumnAmount.DefaultCellStyle = dataGridViewCellStyle8;
            this.ColumnAmount.HeaderText = "합계";
            this.ColumnAmount.Name = "ColumnAmount";
            this.ColumnAmount.ReadOnly = true;
            this.ColumnAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(655, 255);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
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
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnAddClose.FlatAppearance.BorderSize = 0;
            this.btnAddClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddClose.ForeColor = System.Drawing.Color.White;
            this.btnAddClose.Location = new System.Drawing.Point(555, 255);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 27);
            this.btnAddClose.TabIndex = 38;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "전자세금발행";
            this.btnAddClose.UseVisualStyleBackColor = false;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // pointsDataSet
            // 
            this.pointsDataSet.DataSetName = "PointsDataSet";
            this.pointsDataSet.EnforceConstraints = false;
            this.pointsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(331, 262);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(78, 20);
            this.webBrowser1.TabIndex = 108;
            this.webBrowser1.Url = new System.Uri("http://222.231.9.253/NiceEncoding.asp", System.UriKind.Absolute);
            this.webBrowser1.Visible = false;
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(279, 113);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 109;
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
            // dtpEtaxDate
            // 
            this.dtpEtaxDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEtaxDate.CustomFormat = "yyyy/MM/dd";
            this.dtpEtaxDate.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dtpEtaxDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEtaxDate.Location = new System.Drawing.Point(54, 7);
            this.dtpEtaxDate.Name = "dtpEtaxDate";
            this.dtpEtaxDate.Size = new System.Drawing.Size(86, 25);
            this.dtpEtaxDate.TabIndex = 55;
            this.dtpEtaxDate.TabStop = false;
            // 
            // FrmClientVaccountEtax
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(758, 290);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.MaximizeBox = false;
            this.Name = "FrmClientVaccountEtax";
            this.Text = "주선사 충전금 세금계산서 발행";
            this.Load += new System.EventHandler(this.FrmTradeNew2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).EndInit();
            this.pnProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btn_Search;
        private NewDGV newDGV1;
        private System.Windows.Forms.CheckBox AllSelect;
        private System.Windows.Forms.Label lbl_Tax;
        private System.Windows.Forms.RadioButton rdb_Tax1;
        private System.Windows.Forms.RadioButton rdb_Tax0;
        private System.Windows.Forms.RadioButton rdoSplit;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.ComboBox cmb_Search;
        private System.Windows.Forms.TextBox txt_Search;
        private UI.DataGridViewDisableCheckBoxColumn ColumnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBizNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCeo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAmount;
        private DataSets.PointsDataSet pointsDataSet;
        private DataSets.PointsDataSetTableAdapters.ClientPointsListTableAdapter clientPointsListTableAdapter;
        private DataSets.ClientDataSet clientDataSet;
        private DataSets.ClientDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.DateTimePicker dtpEtaxDate;
    }
}