namespace mycalltruck.Admin
{
    partial class UCAccount04
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnRequestAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPayedAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSalesPayedAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTradePayedAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.PaySum = new System.Windows.Forms.TextBox();
            this.PaySumLabel = new System.Windows.Forms.Label();
            this.RequestSum = new System.Windows.Forms.TextBox();
            this.RequestSumLable = new System.Windows.Forms.Label();
            this.ColumnMisuRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stats1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.stats1TableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.Stats1TableAdapter();
            this.CustomerSelect = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnMisu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelDataGrid = new mycalltruck.Admin.NewDGV();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnOrderCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAcceptedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAcceptedRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNotIssuedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNotIssuedAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Div = new System.Windows.Forms.ComboBox();
            this.Input = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp_Edate = new System.Windows.Forms.DateTimePicker();
            this.dtp_Sdate = new System.Windows.Forms.DateTimePicker();
            this.btn_Search = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stats1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "";
            this.columnHeader8.Width = 100;
            // 
            // ColumnRequestAmount
            // 
            this.ColumnRequestAmount.DataPropertyName = "IssuedAmount";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            this.ColumnRequestAmount.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnRequestAmount.HeaderText = "청구운임";
            this.ColumnRequestAmount.Name = "ColumnRequestAmount";
            this.ColumnRequestAmount.ReadOnly = true;
            this.ColumnRequestAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnPayedAmount
            // 
            this.ColumnPayedAmount.DataPropertyName = "DriverAmount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            this.ColumnPayedAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnPayedAmount.HeaderText = "지불운임";
            this.ColumnPayedAmount.Name = "ColumnPayedAmount";
            this.ColumnPayedAmount.ReadOnly = true;
            this.ColumnPayedAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSalesPayedAmount
            // 
            this.ColumnSalesPayedAmount.DataPropertyName = "SalesPayedAmount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            this.ColumnSalesPayedAmount.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnSalesPayedAmount.HeaderText = "입금금액";
            this.ColumnSalesPayedAmount.Name = "ColumnSalesPayedAmount";
            this.ColumnSalesPayedAmount.ReadOnly = true;
            this.ColumnSalesPayedAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnTradePayedAmount
            // 
            this.ColumnTradePayedAmount.DataPropertyName = "TradePayedAmount";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            this.ColumnTradePayedAmount.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnTradePayedAmount.HeaderText = "출금금액";
            this.ColumnTradePayedAmount.Name = "ColumnTradePayedAmount";
            this.ColumnTradePayedAmount.ReadOnly = true;
            this.ColumnTradePayedAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.PaySum);
            this.panel4.Controls.Add(this.PaySumLabel);
            this.panel4.Controls.Add(this.RequestSum);
            this.panel4.Controls.Add(this.RequestSumLable);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 468);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(796, 1);
            this.panel4.TabIndex = 3;
            this.panel4.Visible = false;
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
            this.PaySumLabel.Text = "출금액 소계";
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
            this.RequestSumLable.Text = "입금액 소계";
            this.RequestSumLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColumnMisuRate
            // 
            this.ColumnMisuRate.DataPropertyName = "FeeRate";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.ColumnMisuRate.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnMisuRate.HeaderText = "손익률";
            this.ColumnMisuRate.Name = "ColumnMisuRate";
            this.ColumnMisuRate.ReadOnly = true;
            this.ColumnMisuRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.CustomerSelect.Location = new System.Drawing.Point(410, 34);
            this.CustomerSelect.MultiSelect = false;
            this.CustomerSelect.Name = "CustomerSelect";
            this.CustomerSelect.Size = new System.Drawing.Size(387, 133);
            this.CustomerSelect.TabIndex = 21;
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
            // columnHeader9
            // 
            this.columnHeader9.Text = "";
            this.columnHeader9.Width = 150;
            // 
            // ColumnMisu
            // 
            this.ColumnMisu.DataPropertyName = "FeeAmount";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.ColumnMisu.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnMisu.HeaderText = "손익";
            this.ColumnMisu.Name = "ColumnMisu";
            this.ColumnMisu.ReadOnly = true;
            this.ColumnMisu.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ModelDataGrid
            // 
            this.ModelDataGrid.AllowUserToAddRows = false;
            this.ModelDataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            this.ModelDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.ModelDataGrid.BackgroundColor = System.Drawing.Color.White;
            this.ModelDataGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ModelDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModelDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ModelDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.ModelDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ModelDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnOrderDate,
            this.ColumnOrderCount,
            this.ColumnAcceptedCount,
            this.ColumnAcceptedRate,
            this.ColumnNotIssuedCount,
            this.ColumnNotIssuedAmount,
            this.ColumnMisu,
            this.ColumnMisuRate,
            this.ColumnRequestAmount,
            this.ColumnPayedAmount,
            this.ColumnSalesPayedAmount,
            this.ColumnTradePayedAmount});
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
            this.ModelDataGrid.Location = new System.Drawing.Point(4, 0);
            this.ModelDataGrid.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.ModelDataGrid.MultiSelect = false;
            this.ModelDataGrid.Name = "ModelDataGrid";
            this.ModelDataGrid.RowHeadersVisible = false;
            this.ModelDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ModelDataGrid.RowTemplate.Height = 23;
            this.ModelDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ModelDataGrid.Size = new System.Drawing.Size(798, 465);
            this.ModelDataGrid.TabIndex = 1;
            this.ModelDataGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ModelDataGrid_CellFormatting);
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            this.ColumnName.Frozen = true;
            this.ColumnName.HeaderText = "거래처";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnName.Width = 120;
            // 
            // ColumnOrderDate
            // 
            this.ColumnOrderDate.DataPropertyName = "Date";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Format = "yyyy-MM-dd";
            this.ColumnOrderDate.DefaultCellStyle = dataGridViewCellStyle9;
            this.ColumnOrderDate.HeaderText = "거래일자";
            this.ColumnOrderDate.Name = "ColumnOrderDate";
            this.ColumnOrderDate.ReadOnly = true;
            this.ColumnOrderDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnOrderCount
            // 
            this.ColumnOrderCount.DataPropertyName = "OrderCount";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N0";
            this.ColumnOrderCount.DefaultCellStyle = dataGridViewCellStyle10;
            this.ColumnOrderCount.HeaderText = "총배차";
            this.ColumnOrderCount.Name = "ColumnOrderCount";
            this.ColumnOrderCount.ReadOnly = true;
            this.ColumnOrderCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnAcceptedCount
            // 
            this.ColumnAcceptedCount.DataPropertyName = "AcceptedCount";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N0";
            this.ColumnAcceptedCount.DefaultCellStyle = dataGridViewCellStyle11;
            this.ColumnAcceptedCount.HeaderText = "용차";
            this.ColumnAcceptedCount.Name = "ColumnAcceptedCount";
            this.ColumnAcceptedCount.ReadOnly = true;
            this.ColumnAcceptedCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnAcceptedRate
            // 
            this.ColumnAcceptedRate.DataPropertyName = "AcceptedRate";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            this.ColumnAcceptedRate.DefaultCellStyle = dataGridViewCellStyle12;
            this.ColumnAcceptedRate.HeaderText = "용차율";
            this.ColumnAcceptedRate.Name = "ColumnAcceptedRate";
            this.ColumnAcceptedRate.ReadOnly = true;
            this.ColumnAcceptedRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnNotIssuedCount
            // 
            this.ColumnNotIssuedCount.DataPropertyName = "NotIssuedCount";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "N0";
            this.ColumnNotIssuedCount.DefaultCellStyle = dataGridViewCellStyle13;
            this.ColumnNotIssuedCount.HeaderText = "미청구건수";
            this.ColumnNotIssuedCount.Name = "ColumnNotIssuedCount";
            this.ColumnNotIssuedCount.ReadOnly = true;
            this.ColumnNotIssuedCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnNotIssuedAmount
            // 
            this.ColumnNotIssuedAmount.DataPropertyName = "NotIssuedAmount";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N0";
            this.ColumnNotIssuedAmount.DefaultCellStyle = dataGridViewCellStyle14;
            this.ColumnNotIssuedAmount.HeaderText = "미청구금액";
            this.ColumnNotIssuedAmount.Name = "ColumnNotIssuedAmount";
            this.ColumnNotIssuedAmount.ReadOnly = true;
            this.ColumnNotIssuedAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.panel1.Size = new System.Drawing.Size(802, 38);
            this.panel1.TabIndex = 19;
            // 
            // Div
            // 
            this.Div.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Div.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Div.FormattingEnabled = true;
            this.Div.Items.AddRange(new object[] {
            "일단위",
            "월단위"});
            this.Div.Location = new System.Drawing.Point(370, 8);
            this.Div.Name = "Div";
            this.Div.Size = new System.Drawing.Size(80, 20);
            this.Div.TabIndex = 51;
            // 
            // Input
            // 
            this.Input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Input.Location = new System.Drawing.Point(456, 8);
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
            this.label11.Size = new System.Drawing.Size(134, 16);
            this.label11.TabIndex = 48;
            this.label11.Text = "[거래처별 실적]";
            // 
            // dtp_Edate
            // 
            this.dtp_Edate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Edate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Edate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Edate.Location = new System.Drawing.Point(651, 8);
            this.dtp_Edate.Name = "dtp_Edate";
            this.dtp_Edate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Edate.TabIndex = 46;
            // 
            // dtp_Sdate
            // 
            this.dtp_Sdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Sdate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Sdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Sdate.Location = new System.Drawing.Point(562, 8);
            this.dtp_Sdate.Name = "dtp_Sdate";
            this.dtp_Sdate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Sdate.TabIndex = 45;
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.Location = new System.Drawing.Point(740, 7);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Search.Size = new System.Drawing.Size(56, 23);
            this.btn_Search.TabIndex = 43;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ModelDataGrid, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(802, 465);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(802, 465);
            this.panel2.TabIndex = 20;
            // 
            // UCAccount04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.CustomerSelect);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UCAccount04";
            this.Size = new System.Drawing.Size(802, 503);
            this.Load += new System.EventHandler(this.UCAccount04_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stats1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRequestAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPayedAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSalesPayedAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTradePayedAmount;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox PaySum;
        private System.Windows.Forms.Label PaySumLabel;
        private System.Windows.Forms.TextBox RequestSum;
        private System.Windows.Forms.Label RequestSumLable;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisuRate;
        private System.Windows.Forms.BindingSource stats1BindingSource;
        private CMDataSet cMDataSet;
        private CMDataSetTableAdapters.Stats1TableAdapter stats1TableAdapter;
        private System.Windows.Forms.ListView CustomerSelect;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisu;
        private NewDGV ModelDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOrderCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAcceptedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAcceptedRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNotIssuedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNotIssuedAmount;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox Div;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp_Edate;
        private System.Windows.Forms.DateTimePicker dtp_Sdate;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
    }
}
