namespace mycalltruck.Admin
{
    partial class UCAccount06
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CustomerSelect = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.stats1TableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.Stats1TableAdapter();
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.stats1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PaySum = new System.Windows.Forms.TextBox();
            this.PaySumLabel = new System.Windows.Forms.Label();
            this.RequestSum = new System.Windows.Forms.TextBox();
            this.RequestSumLable = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ModelDataGrid = new mycalltruck.Admin.NewDGV();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Month = new System.Windows.Forms.ComboBox();
            this.Year = new System.Windows.Forms.NumericUpDown();
            this.Input = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_Search = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTotalMisu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMisu1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMisu2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMisu3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMisu4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMisu5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMisu6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stats1BindingSource)).BeginInit();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Year)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "";
            this.columnHeader9.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 120;
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
            this.CustomerSelect.Location = new System.Drawing.Point(408, 34);
            this.CustomerSelect.MultiSelect = false;
            this.CustomerSelect.Name = "CustomerSelect";
            this.CustomerSelect.Size = new System.Drawing.Size(387, 133);
            this.CustomerSelect.TabIndex = 21;
            this.CustomerSelect.UseCompatibleStateImageBehavior = false;
            this.CustomerSelect.View = System.Windows.Forms.View.Details;
            this.CustomerSelect.Visible = false;
            this.CustomerSelect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Input_KeyPress);
            this.CustomerSelect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CustomerSelect_MouseClick);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "";
            this.columnHeader8.Width = 100;
            // 
            // stats1TableAdapter
            // 
            this.stats1TableAdapter.ClearBeforeFill = true;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // stats1BindingSource
            // 
            this.stats1BindingSource.DataMember = "Stats1";
            this.stats1BindingSource.DataSource = this.cMDataSet;
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
            // ModelDataGrid
            // 
            this.ModelDataGrid.AllowUserToAddRows = false;
            this.ModelDataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.ModelDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ModelDataGrid.BackgroundColor = System.Drawing.Color.White;
            this.ModelDataGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ModelDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModelDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ModelDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ModelDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ModelDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnTotalMisu,
            this.ColumnMisu1,
            this.ColumnMisu2,
            this.ColumnMisu3,
            this.ColumnMisu4,
            this.ColumnMisu5,
            this.ColumnMisu6,
            this.Column1});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ModelDataGrid.DefaultCellStyle = dataGridViewCellStyle10;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(802, 465);
            this.panel2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(436, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 53;
            this.label1.Text = "거래처";
            // 
            // Month
            // 
            this.Month.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Month.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Month.FormattingEnabled = true;
            this.Month.Items.AddRange(new object[] {
            "01월",
            "02월",
            "03월",
            "04월",
            "05월",
            "06월",
            "07월",
            "08월",
            "09월",
            "10월",
            "11월",
            "12월"});
            this.Month.Location = new System.Drawing.Point(659, 8);
            this.Month.Name = "Month";
            this.Month.Size = new System.Drawing.Size(75, 20);
            this.Month.TabIndex = 52;
            // 
            // Year
            // 
            this.Year.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Year.Location = new System.Drawing.Point(589, 8);
            this.Year.Maximum = new decimal(new int[] {
            2099,
            0,
            0,
            0});
            this.Year.Minimum = new decimal(new int[] {
            2016,
            0,
            0,
            0});
            this.Year.Name = "Year";
            this.Year.Size = new System.Drawing.Size(64, 21);
            this.Year.TabIndex = 51;
            this.Year.Value = new decimal(new int[] {
            2018,
            0,
            0,
            0});
            // 
            // Input
            // 
            this.Input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Input.Location = new System.Drawing.Point(483, 8);
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
            this.label11.Size = new System.Drawing.Size(157, 16);
            this.label11.TabIndex = 48;
            this.label11.Text = "[월별 미수금 현황]";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Month);
            this.panel1.Controls.Add(this.Year);
            this.panel1.Controls.Add(this.Input);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 38);
            this.panel1.TabIndex = 19;
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
            // ColumnTotalMisu
            // 
            this.ColumnTotalMisu.DataPropertyName = "TotalMisu";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            this.ColumnTotalMisu.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnTotalMisu.HeaderText = "잔액";
            this.ColumnTotalMisu.Name = "ColumnTotalMisu";
            this.ColumnTotalMisu.ReadOnly = true;
            this.ColumnTotalMisu.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMisu1
            // 
            this.ColumnMisu1.DataPropertyName = "Misu1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            this.ColumnMisu1.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnMisu1.HeaderText = "";
            this.ColumnMisu1.Name = "ColumnMisu1";
            this.ColumnMisu1.ReadOnly = true;
            this.ColumnMisu1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMisu2
            // 
            this.ColumnMisu2.DataPropertyName = "Misu2";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            this.ColumnMisu2.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnMisu2.HeaderText = "";
            this.ColumnMisu2.Name = "ColumnMisu2";
            this.ColumnMisu2.ReadOnly = true;
            this.ColumnMisu2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMisu3
            // 
            this.ColumnMisu3.DataPropertyName = "Misu3";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.ColumnMisu3.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnMisu3.HeaderText = "";
            this.ColumnMisu3.Name = "ColumnMisu3";
            this.ColumnMisu3.ReadOnly = true;
            this.ColumnMisu3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMisu4
            // 
            this.ColumnMisu4.DataPropertyName = "Misu4";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            this.ColumnMisu4.DefaultCellStyle = dataGridViewCellStyle7;
            this.ColumnMisu4.HeaderText = "";
            this.ColumnMisu4.Name = "ColumnMisu4";
            this.ColumnMisu4.ReadOnly = true;
            this.ColumnMisu4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMisu5
            // 
            this.ColumnMisu5.DataPropertyName = "Misu5";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            this.ColumnMisu5.DefaultCellStyle = dataGridViewCellStyle8;
            this.ColumnMisu5.HeaderText = "";
            this.ColumnMisu5.Name = "ColumnMisu5";
            this.ColumnMisu5.ReadOnly = true;
            this.ColumnMisu5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMisu6
            // 
            this.ColumnMisu6.DataPropertyName = "Misu6";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            this.ColumnMisu6.DefaultCellStyle = dataGridViewCellStyle9;
            this.ColumnMisu6.HeaderText = "";
            this.ColumnMisu6.Name = "ColumnMisu6";
            this.ColumnMisu6.ReadOnly = true;
            this.ColumnMisu6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "비고";
            this.Column1.Name = "Column1";
            this.Column1.Width = 285;
            // 
            // UCAccount06
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.CustomerSelect);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UCAccount06";
            this.Size = new System.Drawing.Size(802, 503);
            this.Load += new System.EventHandler(this.UCAccount06_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stats1BindingSource)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ModelDataGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Year)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ListView CustomerSelect;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private CMDataSetTableAdapters.Stats1TableAdapter stats1TableAdapter;
        private CMDataSet cMDataSet;
        private System.Windows.Forms.BindingSource stats1BindingSource;
        private System.Windows.Forms.TextBox PaySum;
        private System.Windows.Forms.Label PaySumLabel;
        private System.Windows.Forms.TextBox RequestSum;
        private System.Windows.Forms.Label RequestSumLable;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private NewDGV ModelDataGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Month;
        private System.Windows.Forms.NumericUpDown Year;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTotalMisu;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisu1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisu2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisu3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisu4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisu5;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMisu6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}
