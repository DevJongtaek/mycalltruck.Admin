namespace mycalltruck.Admin
{
    partial class FrmMNSTATS5
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.sTATS5BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_Edate = new System.Windows.Forms.DateTimePicker();
            this.dtp_Sdate = new System.Windows.Forms.DateTimePicker();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Print = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.sTATS5TableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.STATS5TableAdapter();
            this.idx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revenueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderPerCentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTATS5BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToDeleteRows = false;
            this.newDGV1.AllowUserToOrderColumns = true;
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.newDGV1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.newDGV1.AutoGenerateColumns = false;
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
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idx,
            this.payDateDataGridViewTextBoxColumn,
            this.orderCountDataGridViewTextBoxColumn,
            this.clientPriceDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.revenueDataGridViewTextBoxColumn,
            this.orderPerCentDataGridViewTextBoxColumn,
            this.clientidDataGridViewTextBoxColumn,
            this.SumAmount});
            this.newDGV1.DataSource = this.sTATS5BindingSource;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle11;
            this.newDGV1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(0, 0);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.MultiSelect = false;
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.ReadOnly = true;
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newDGV1.Size = new System.Drawing.Size(1012, 578);
            this.newDGV1.TabIndex = 48;
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            // 
            // sTATS5BindingSource
            // 
            this.sTATS5BindingSource.DataMember = "STATS5";
            this.sTATS5BindingSource.DataSource = this.cMDataSet;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(577, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 12);
            this.label1.TabIndex = 47;
            this.label1.Text = "결제일 :";
            // 
            // dtp_Edate
            // 
            this.dtp_Edate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Edate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Edate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Edate.Location = new System.Drawing.Point(721, 9);
            this.dtp_Edate.Name = "dtp_Edate";
            this.dtp_Edate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Edate.TabIndex = 46;
            // 
            // dtp_Sdate
            // 
            this.dtp_Sdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Sdate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Sdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Sdate.Location = new System.Drawing.Point(632, 9);
            this.dtp_Sdate.Name = "dtp_Sdate";
            this.dtp_Sdate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Sdate.TabIndex = 45;
            // 
            // btn_Inew
            // 
            this.btn_Inew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Inew.Location = new System.Drawing.Point(933, 7);
            this.btn_Inew.Name = "btn_Inew";
            this.btn_Inew.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Inew.Size = new System.Drawing.Size(76, 23);
            this.btn_Inew.TabIndex = 44;
            this.btn_Inew.Text = "초 기 화";
            this.btn_Inew.UseVisualStyleBackColor = true;
            this.btn_Inew.Click += new System.EventHandler(this.btn_Inew_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.Location = new System.Drawing.Point(807, 8);
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
            this.panel2.Controls.Add(this.newDGV1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1012, 578);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Print);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtp_Edate);
            this.panel1.Controls.Add(this.dtp_Sdate);
            this.panel1.Controls.Add(this.btn_Inew);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 38);
            this.panel1.TabIndex = 2;
            // 
            // btn_Print
            // 
            this.btn_Print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Print.Location = new System.Drawing.Point(871, 8);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Print.Size = new System.Drawing.Size(56, 23);
            this.btn_Print.TabIndex = 53;
            this.btn_Print.Text = "인 쇄";
            this.btn_Print.UseVisualStyleBackColor = true;
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label11.Location = new System.Drawing.Point(12, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(168, 16);
            this.label11.TabIndex = 52;
            this.label11.Text = "[일자별 수익성분석]";
            // 
            // sTATS5TableAdapter
            // 
            this.sTATS5TableAdapter.ClearBeforeFill = true;
            // 
            // idx
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.idx.DefaultCellStyle = dataGridViewCellStyle3;
            this.idx.HeaderText = "번호";
            this.idx.Name = "idx";
            this.idx.ReadOnly = true;
            this.idx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.idx.Width = 46;
            // 
            // payDateDataGridViewTextBoxColumn
            // 
            this.payDateDataGridViewTextBoxColumn.DataPropertyName = "PayDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.payDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.payDateDataGridViewTextBoxColumn.HeaderText = "결제일";
            this.payDateDataGridViewTextBoxColumn.Name = "payDateDataGridViewTextBoxColumn";
            this.payDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.payDateDataGridViewTextBoxColumn.Width = 120;
            // 
            // orderCountDataGridViewTextBoxColumn
            // 
            this.orderCountDataGridViewTextBoxColumn.DataPropertyName = "orderCount";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.orderCountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.orderCountDataGridViewTextBoxColumn.HeaderText = "배차건수";
            this.orderCountDataGridViewTextBoxColumn.Name = "orderCountDataGridViewTextBoxColumn";
            this.orderCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // clientPriceDataGridViewTextBoxColumn
            // 
            this.clientPriceDataGridViewTextBoxColumn.DataPropertyName = "ClientPrice";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.clientPriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.clientPriceDataGridViewTextBoxColumn.HeaderText = "화주운송료";
            this.clientPriceDataGridViewTextBoxColumn.Name = "clientPriceDataGridViewTextBoxColumn";
            this.clientPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.clientPriceDataGridViewTextBoxColumn.Width = 130;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.priceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.priceDataGridViewTextBoxColumn.HeaderText = "배차운송료";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.ReadOnly = true;
            this.priceDataGridViewTextBoxColumn.Width = 130;
            // 
            // revenueDataGridViewTextBoxColumn
            // 
            this.revenueDataGridViewTextBoxColumn.DataPropertyName = "Revenue";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.revenueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.revenueDataGridViewTextBoxColumn.HeaderText = "수익금";
            this.revenueDataGridViewTextBoxColumn.Name = "revenueDataGridViewTextBoxColumn";
            this.revenueDataGridViewTextBoxColumn.ReadOnly = true;
            this.revenueDataGridViewTextBoxColumn.Width = 130;
            // 
            // orderPerCentDataGridViewTextBoxColumn
            // 
            this.orderPerCentDataGridViewTextBoxColumn.DataPropertyName = "orderPerCent";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.orderPerCentDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.orderPerCentDataGridViewTextBoxColumn.HeaderText = "수익율(%)";
            this.orderPerCentDataGridViewTextBoxColumn.Name = "orderPerCentDataGridViewTextBoxColumn";
            this.orderPerCentDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderPerCentDataGridViewTextBoxColumn.Width = 130;
            // 
            // clientidDataGridViewTextBoxColumn
            // 
            this.clientidDataGridViewTextBoxColumn.DataPropertyName = "clientid";
            this.clientidDataGridViewTextBoxColumn.HeaderText = "clientid";
            this.clientidDataGridViewTextBoxColumn.Name = "clientidDataGridViewTextBoxColumn";
            this.clientidDataGridViewTextBoxColumn.ReadOnly = true;
            this.clientidDataGridViewTextBoxColumn.Visible = false;
            // 
            // SumAmount
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SumAmount.DefaultCellStyle = dataGridViewCellStyle10;
            this.SumAmount.HeaderText = "비고";
            this.SumAmount.Name = "SumAmount";
            this.SumAmount.ReadOnly = true;
            this.SumAmount.Width = 215;
            // 
            // FrmMNSTATS5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1012, 616);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmMNSTATS5";
            this.Text = "일자별 수익성 분석";
            this.Load += new System.EventHandler(this.FrmMNSTATS5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTATS5BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NewDGV newDGV1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_Edate;
        private System.Windows.Forms.DateTimePicker dtp_Sdate;
        private System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource sTATS5BindingSource;
        private CMDataSet cMDataSet;
        private CMDataSetTableAdapters.STATS5TableAdapter sTATS5TableAdapter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_Print;
        private System.Windows.Forms.DataGridViewTextBoxColumn idx;
        private System.Windows.Forms.DataGridViewTextBoxColumn payDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revenueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderPerCentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumAmount;
    }
}