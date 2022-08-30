namespace mycalltruck.Admin
{
    partial class UCNSTATS5
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAlterPrice = new System.Windows.Forms.Label();
            this.lblAOSum = new System.Windows.Forms.Label();
            this.lblOutSum = new System.Windows.Forms.Label();
            this.lblInputSum = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ModelDataGrid = new mycalltruck.Admin.NewDGV();
            this.rowNUMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gubunDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sangHoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aOAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nSTATS5BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nSTATSDataSet = new mycalltruck.Admin.DataSets.NSTATSDataSet();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.cmbSSearch = new System.Windows.Forms.ComboBox();
            this.cmbSReferralId = new System.Windows.Forms.ComboBox();
            this.cmbSMonth = new System.Windows.Forms.ComboBox();
            this.txtSText = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp_Edate = new System.Windows.Forms.DateTimePicker();
            this.dtp_Sdate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.clientDataSet = new mycalltruck.Admin.DataSets.ClientDataSet();
            this.customersTableAdapter = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.CustomersTableAdapter();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.nSTATS5TableAdapter = new mycalltruck.Admin.DataSets.NSTATSDataSetTableAdapters.NSTATS5TableAdapter();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSTATS5BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSTATSDataSet)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panel4.Location = new System.Drawing.Point(0, 556);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1100, 25);
            this.panel4.TabIndex = 23;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.lblAlterPrice, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblAOSum, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblOutSum, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblInputSum, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 6, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1100, 25);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblAlterPrice
            // 
            this.lblAlterPrice.BackColor = System.Drawing.Color.White;
            this.lblAlterPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlterPrice.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblAlterPrice.Location = new System.Drawing.Point(960, 1);
            this.lblAlterPrice.Margin = new System.Windows.Forms.Padding(0);
            this.lblAlterPrice.Name = "lblAlterPrice";
            this.lblAlterPrice.Size = new System.Drawing.Size(139, 23);
            this.lblAlterPrice.TabIndex = 11;
            this.lblAlterPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAOSum
            // 
            this.lblAOSum.BackColor = System.Drawing.Color.White;
            this.lblAOSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAOSum.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblAOSum.Location = new System.Drawing.Point(686, 1);
            this.lblAOSum.Margin = new System.Windows.Forms.Padding(0);
            this.lblAOSum.Name = "lblAOSum";
            this.lblAOSum.Size = new System.Drawing.Size(136, 23);
            this.lblAOSum.TabIndex = 10;
            this.lblAOSum.Text = "0";
            this.lblAOSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutSum
            // 
            this.lblOutSum.BackColor = System.Drawing.Color.White;
            this.lblOutSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutSum.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblOutSum.Location = new System.Drawing.Point(412, 1);
            this.lblOutSum.Margin = new System.Windows.Forms.Padding(0);
            this.lblOutSum.Name = "lblOutSum";
            this.lblOutSum.Size = new System.Drawing.Size(136, 23);
            this.lblOutSum.TabIndex = 9;
            this.lblOutSum.Text = "0";
            this.lblOutSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInputSum
            // 
            this.lblInputSum.BackColor = System.Drawing.Color.White;
            this.lblInputSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInputSum.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblInputSum.Location = new System.Drawing.Point(138, 1);
            this.lblInputSum.Margin = new System.Windows.Forms.Padding(0);
            this.lblInputSum.Name = "lblInputSum";
            this.lblInputSum.Size = new System.Drawing.Size(136, 23);
            this.lblInputSum.TabIndex = 8;
            this.lblInputSum.Text = "0";
            this.lblInputSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "입금액";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(275, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "출금액";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(549, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "입금-출금";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(823, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "비고";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.ModelDataGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panel3.Location = new System.Drawing.Point(3, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1094, 507);
            this.panel3.TabIndex = 22;
            // 
            // ModelDataGrid
            // 
            this.ModelDataGrid.AllowUserToAddRows = false;
            this.ModelDataGrid.AllowUserToDeleteRows = false;
            this.ModelDataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.ModelDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ModelDataGrid.AutoGenerateColumns = false;
            this.ModelDataGrid.BackgroundColor = System.Drawing.Color.White;
            this.ModelDataGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ModelDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModelDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ModelDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ModelDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ModelDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rowNUMDataGridViewTextBoxColumn,
            this.payDateDataGridViewTextBoxColumn,
            this.gubunDataGridViewTextBoxColumn,
            this.sangHoDataGridViewTextBoxColumn,
            this.bizNoDataGridViewTextBoxColumn,
            this.carNoDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.OutAmount,
            this.aOAmountDataGridViewTextBoxColumn,
            this.Column1});
            this.ModelDataGrid.DataSource = this.nSTATS5BindingSource;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ModelDataGrid.DefaultCellStyle = dataGridViewCellStyle11;
            this.ModelDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModelDataGrid.GridColor = System.Drawing.Color.White;
            this.ModelDataGrid.Location = new System.Drawing.Point(0, 0);
            this.ModelDataGrid.Margin = new System.Windows.Forms.Padding(0);
            this.ModelDataGrid.MultiSelect = false;
            this.ModelDataGrid.Name = "ModelDataGrid";
            this.ModelDataGrid.ReadOnly = true;
            this.ModelDataGrid.RowHeadersVisible = false;
            this.ModelDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.ModelDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.ModelDataGrid.RowTemplate.Height = 23;
            this.ModelDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ModelDataGrid.Size = new System.Drawing.Size(1094, 507);
            this.ModelDataGrid.TabIndex = 2;
            this.ModelDataGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ModelDataGrid_CellFormatting);
            // 
            // rowNUMDataGridViewTextBoxColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rowNUMDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.rowNUMDataGridViewTextBoxColumn.HeaderText = "번호";
            this.rowNUMDataGridViewTextBoxColumn.Name = "rowNUMDataGridViewTextBoxColumn";
            this.rowNUMDataGridViewTextBoxColumn.ReadOnly = true;
            this.rowNUMDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rowNUMDataGridViewTextBoxColumn.Width = 41;
            // 
            // payDateDataGridViewTextBoxColumn
            // 
            this.payDateDataGridViewTextBoxColumn.DataPropertyName = "PayDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "yyyy-MM-dd HH:mm:ss";
            this.payDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.payDateDataGridViewTextBoxColumn.HeaderText = "거래일";
            this.payDateDataGridViewTextBoxColumn.Name = "payDateDataGridViewTextBoxColumn";
            this.payDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.payDateDataGridViewTextBoxColumn.Width = 150;
            // 
            // gubunDataGridViewTextBoxColumn
            // 
            this.gubunDataGridViewTextBoxColumn.DataPropertyName = "Gubun";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gubunDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.gubunDataGridViewTextBoxColumn.HeaderText = "구분";
            this.gubunDataGridViewTextBoxColumn.Name = "gubunDataGridViewTextBoxColumn";
            this.gubunDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sangHoDataGridViewTextBoxColumn
            // 
            this.sangHoDataGridViewTextBoxColumn.DataPropertyName = "SangHo";
            this.sangHoDataGridViewTextBoxColumn.HeaderText = "상호";
            this.sangHoDataGridViewTextBoxColumn.Name = "sangHoDataGridViewTextBoxColumn";
            this.sangHoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bizNoDataGridViewTextBoxColumn
            // 
            this.bizNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.bizNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.bizNoDataGridViewTextBoxColumn.HeaderText = "사업자번호";
            this.bizNoDataGridViewTextBoxColumn.Name = "bizNoDataGridViewTextBoxColumn";
            this.bizNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // carNoDataGridViewTextBoxColumn
            // 
            this.carNoDataGridViewTextBoxColumn.DataPropertyName = "CarNo";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.carNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.carNoDataGridViewTextBoxColumn.HeaderText = "차량번호";
            this.carNoDataGridViewTextBoxColumn.Name = "carNoDataGridViewTextBoxColumn";
            this.carNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.amountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.amountDataGridViewTextBoxColumn.HeaderText = "입금";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // OutAmount
            // 
            this.OutAmount.DataPropertyName = "OutAmount";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            dataGridViewCellStyle9.NullValue = null;
            this.OutAmount.DefaultCellStyle = dataGridViewCellStyle9;
            this.OutAmount.HeaderText = "출금";
            this.OutAmount.Name = "OutAmount";
            this.OutAmount.ReadOnly = true;
            // 
            // aOAmountDataGridViewTextBoxColumn
            // 
            this.aOAmountDataGridViewTextBoxColumn.DataPropertyName = "AOAmount";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N0";
            dataGridViewCellStyle10.NullValue = null;
            this.aOAmountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.aOAmountDataGridViewTextBoxColumn.HeaderText = "입금-출금";
            this.aOAmountDataGridViewTextBoxColumn.Name = "aOAmountDataGridViewTextBoxColumn";
            this.aOAmountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "비고";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 230;
            // 
            // nSTATS5BindingSource
            // 
            this.nSTATS5BindingSource.DataMember = "NSTATS5";
            this.nSTATS5BindingSource.DataSource = this.nSTATSDataSet;
            // 
            // nSTATSDataSet
            // 
            this.nSTATSDataSet.DataSetName = "NSTATSDataSet";
            this.nSTATSDataSet.EnforceConstraints = false;
            this.nSTATSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.panel2.Controls.Add(this.btn_Inew);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Controls.Add(this.cmbSSearch);
            this.panel2.Controls.Add(this.cmbSReferralId);
            this.panel2.Controls.Add(this.cmbSMonth);
            this.panel2.Controls.Add(this.txtSText);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.dtp_Edate);
            this.panel2.Controls.Add(this.dtp_Sdate);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1100, 43);
            this.panel2.TabIndex = 21;
            // 
            // btn_Inew
            // 
            this.btn_Inew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Inew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Inew.FlatAppearance.BorderSize = 0;
            this.btn_Inew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Inew.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Inew.ForeColor = System.Drawing.Color.White;
            this.btn_Inew.Location = new System.Drawing.Point(1031, 7);
            this.btn_Inew.Name = "btn_Inew";
            this.btn_Inew.Size = new System.Drawing.Size(65, 27);
            this.btn_Inew.TabIndex = 59;
            this.btn_Inew.Text = "초기화";
            this.btn_Inew.UseVisualStyleBackColor = false;
            this.btn_Inew.Click += new System.EventHandler(this.btn_Inew_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(977, 7);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(50, 27);
            this.btnPrint.TabIndex = 55;
            this.btnPrint.Text = "인 쇄";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // cmbSSearch
            // 
            this.cmbSSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSSearch.FormattingEnabled = true;
            this.cmbSSearch.Items.AddRange(new object[] {
            ""});
            this.cmbSSearch.Location = new System.Drawing.Point(730, 9);
            this.cmbSSearch.Name = "cmbSSearch";
            this.cmbSSearch.Size = new System.Drawing.Size(80, 25);
            this.cmbSSearch.TabIndex = 54;
            // 
            // cmbSReferralId
            // 
            this.cmbSReferralId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSReferralId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSReferralId.FormattingEnabled = true;
            this.cmbSReferralId.Location = new System.Drawing.Point(277, 8);
            this.cmbSReferralId.Name = "cmbSReferralId";
            this.cmbSReferralId.Size = new System.Drawing.Size(80, 25);
            this.cmbSReferralId.TabIndex = 52;
            this.cmbSReferralId.Visible = false;
            // 
            // cmbSMonth
            // 
            this.cmbSMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSMonth.FormattingEnabled = true;
            this.cmbSMonth.Location = new System.Drawing.Point(466, 9);
            this.cmbSMonth.Name = "cmbSMonth";
            this.cmbSMonth.Size = new System.Drawing.Size(80, 25);
            this.cmbSMonth.TabIndex = 51;
            this.cmbSMonth.SelectedIndexChanged += new System.EventHandler(this.cmbSMonth_SelectedIndexChanged);
            // 
            // txtSText
            // 
            this.txtSText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSText.Location = new System.Drawing.Point(816, 8);
            this.txtSText.Name = "txtSText";
            this.txtSText.Size = new System.Drawing.Size(100, 25);
            this.txtSText.TabIndex = 50;
            this.txtSText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSText_KeyUp);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(5, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 19);
            this.label11.TabIndex = 48;
            this.label11.Text = "통합정산 내역서";
            // 
            // dtp_Edate
            // 
            this.dtp_Edate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Edate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Edate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Edate.Location = new System.Drawing.Point(641, 9);
            this.dtp_Edate.Name = "dtp_Edate";
            this.dtp_Edate.Size = new System.Drawing.Size(85, 25);
            this.dtp_Edate.TabIndex = 46;
            // 
            // dtp_Sdate
            // 
            this.dtp_Sdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Sdate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Sdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Sdate.Location = new System.Drawing.Point(552, 9);
            this.dtp_Sdate.Name = "dtp_Sdate";
            this.dtp_Sdate.Size = new System.Drawing.Size(85, 25);
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
            this.btnSearch.Location = new System.Drawing.Point(923, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 27);
            this.btnSearch.TabIndex = 43;
            this.btnSearch.Text = "조 회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // clientDataSet
            // 
            this.clientDataSet.DataSetName = "ClientDataSet";
            this.clientDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customersTableAdapter
            // 
            this.customersTableAdapter.ClearBeforeFill = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1100, 581);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // nSTATS5TableAdapter
            // 
            this.nSTATS5TableAdapter.ClearBeforeFill = true;
            // 
            // UCNSTATS5
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "UCNSTATS5";
            this.Size = new System.Drawing.Size(1100, 581);
            this.Load += new System.EventHandler(this.UCNSTATS1_Load);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSTATS5BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSTATSDataSet)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbSMonth;
        public System.Windows.Forms.TextBox txtSText;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp_Edate;
        private System.Windows.Forms.DateTimePicker dtp_Sdate;
        public System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ComboBox cmbSSearch;
        private System.Windows.Forms.ComboBox cmbSReferralId;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        public NewDGV ModelDataGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAlterPrice;
        private System.Windows.Forms.Label lblAOSum;
        private System.Windows.Forms.Label lblOutSum;
        private System.Windows.Forms.Label lblInputSum;
        private DataSets.ClientDataSet clientDataSet;
        private DataSets.ClientDataSetTableAdapters.CustomersTableAdapter customersTableAdapter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.BindingSource nSTATS5BindingSource;
        private DataSets.NSTATSDataSet nSTATSDataSet;
        private DataSets.NSTATSDataSetTableAdapters.NSTATS5TableAdapter nSTATS5TableAdapter;
        public System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNUMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn payDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gubunDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sangHoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn aOAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}
