﻿namespace mycalltruck.Admin
{
    partial class UCSTATS4
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.sTATS4TableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.STATS4TableAdapter();
            this.sTATS4BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.btn_Print = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_Edate = new System.Windows.Forms.DateTimePicker();
            this.dtp_Sdate = new System.Windows.Forms.DateTimePicker();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.btn_Search = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.idx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sangHoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.sTATS4BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sTATS4TableAdapter
            // 
            this.sTATS4TableAdapter.ClearBeforeFill = true;
            // 
            // sTATS4BindingSource
            // 
            this.sTATS4BindingSource.DataMember = "STATS4";
            this.sTATS4BindingSource.DataSource = this.cMDataSet;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btn_Print
            // 
            this.btn_Print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Print.Location = new System.Drawing.Point(661, 7);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Print.Size = new System.Drawing.Size(56, 23);
            this.btn_Print.TabIndex = 52;
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
            this.label11.TabIndex = 51;
            this.label11.Text = "[화주별 수금집계표]";
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Search.Location = new System.Drawing.Point(502, 8);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(87, 21);
            this.txt_Search.TabIndex = 49;
            this.txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyUp);
            // 
            // cmb_Search
            // 
            this.cmb_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Items.AddRange(new object[] {
            "전체",
            "사업자번호",
            "상호"});
            this.cmb_Search.Location = new System.Drawing.Point(398, 8);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(99, 20);
            this.cmb_Search.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 12);
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
            this.dtp_Edate.Location = new System.Drawing.Point(309, 8);
            this.dtp_Edate.Name = "dtp_Edate";
            this.dtp_Edate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Edate.TabIndex = 46;
            // 
            // dtp_Sdate
            // 
            this.dtp_Sdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Sdate.CustomFormat = "yyyy/MM/dd";
            this.dtp_Sdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Sdate.Location = new System.Drawing.Point(220, 8);
            this.dtp_Sdate.Name = "dtp_Sdate";
            this.dtp_Sdate.Size = new System.Drawing.Size(83, 21);
            this.dtp_Sdate.TabIndex = 45;
            // 
            // btn_Inew
            // 
            this.btn_Inew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Inew.Location = new System.Drawing.Point(723, 7);
            this.btn_Inew.Name = "btn_Inew";
            this.btn_Inew.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Inew.Size = new System.Drawing.Size(76, 23);
            this.btn_Inew.TabIndex = 44;
            this.btn_Inew.Text = "초 기 화";
            this.btn_Inew.UseVisualStyleBackColor = true;
            this.btn_Inew.Click += new System.EventHandler(this.btn_Inew_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.newDGV1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(802, 465);
            this.panel2.TabIndex = 3;
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
            this.bizNoDataGridViewTextBoxColumn,
            this.sangHoDataGridViewTextBoxColumn,
            this.payDateDataGridViewTextBoxColumn,
            this.amountTextBoxColumn,
            this.SumAmount});
            this.newDGV1.DataSource = this.sTATS4BindingSource;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.newDGV1.ReadOnly = true;
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newDGV1.Size = new System.Drawing.Size(802, 465);
            this.newDGV1.TabIndex = 49;
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.Location = new System.Drawing.Point(595, 7);
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
            this.panel1.Controls.Add(this.btn_Print);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txt_Search);
            this.panel1.Controls.Add(this.cmb_Search);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtp_Edate);
            this.panel1.Controls.Add(this.dtp_Sdate);
            this.panel1.Controls.Add(this.btn_Inew);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 38);
            this.panel1.TabIndex = 2;
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
            // bizNoDataGridViewTextBoxColumn
            // 
            this.bizNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.bizNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.bizNoDataGridViewTextBoxColumn.HeaderText = "사업자번호";
            this.bizNoDataGridViewTextBoxColumn.Name = "bizNoDataGridViewTextBoxColumn";
            this.bizNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.bizNoDataGridViewTextBoxColumn.Width = 150;
            // 
            // sangHoDataGridViewTextBoxColumn
            // 
            this.sangHoDataGridViewTextBoxColumn.DataPropertyName = "SangHo";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.sangHoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.sangHoDataGridViewTextBoxColumn.HeaderText = "상호";
            this.sangHoDataGridViewTextBoxColumn.Name = "sangHoDataGridViewTextBoxColumn";
            this.sangHoDataGridViewTextBoxColumn.ReadOnly = true;
            this.sangHoDataGridViewTextBoxColumn.Width = 150;
            // 
            // payDateDataGridViewTextBoxColumn
            // 
            this.payDateDataGridViewTextBoxColumn.DataPropertyName = "PayDate";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.payDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.payDateDataGridViewTextBoxColumn.HeaderText = "수금일";
            this.payDateDataGridViewTextBoxColumn.Name = "payDateDataGridViewTextBoxColumn";
            this.payDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.payDateDataGridViewTextBoxColumn.Width = 150;
            // 
            // amountTextBoxColumn
            // 
            this.amountTextBoxColumn.DataPropertyName = "Amount";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.amountTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.amountTextBoxColumn.HeaderText = "금액";
            this.amountTextBoxColumn.Name = "amountTextBoxColumn";
            this.amountTextBoxColumn.ReadOnly = true;
            this.amountTextBoxColumn.Width = 150;
            // 
            // SumAmount
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SumAmount.DefaultCellStyle = dataGridViewCellStyle8;
            this.SumAmount.HeaderText = "비고";
            this.SumAmount.Name = "SumAmount";
            this.SumAmount.ReadOnly = true;
            this.SumAmount.Width = 460;
            // 
            // UCSTATS4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UCSTATS4";
            this.Size = new System.Drawing.Size(802, 503);
            this.Load += new System.EventHandler(this.UCSTATS4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sTATS4BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CMDataSetTableAdapters.STATS4TableAdapter sTATS4TableAdapter;
        private System.Windows.Forms.BindingSource sTATS4BindingSource;
        private CMDataSet cMDataSet;
        private System.Windows.Forms.Button btn_Print;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_Search;
        public System.Windows.Forms.ComboBox cmb_Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_Edate;
        private System.Windows.Forms.DateTimePicker dtp_Sdate;
        private System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Panel panel1;
        private NewDGV newDGV1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idx;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sangHoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn payDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumAmount;
    }
}
