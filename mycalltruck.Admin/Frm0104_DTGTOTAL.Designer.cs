namespace mycalltruck.Admin
{
    partial class Frm0104_DTGTOTAL
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobileNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driversBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.baseDataSet = new mycalltruck.Admin.DataSets.BaseDataSet();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pn_4_month = new System.Windows.Forms.Panel();
            this.lbl_Month4 = new System.Windows.Forms.Label();
            this.pn_3_month = new System.Windows.Forms.Panel();
            this.lbl_Month3 = new System.Windows.Forms.Label();
            this.pn_2_month = new System.Windows.Forms.Panel();
            this.lbl_Month2 = new System.Windows.Forms.Label();
            this.pn_1_month = new System.Windows.Forms.Panel();
            this.lbl_Month1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtgLogTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DTGLogTableAdapter();
            this.driverGroupsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DriverGroupsTableAdapter();
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.driversTableAdapter = new mycalltruck.Admin.DataSets.BaseDataSetTableAdapters.DriversTableAdapter();
            this.tableAdapterManager = new mycalltruck.Admin.DataSets.BaseDataSetTableAdapters.TableAdapterManager();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 510);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(773, 510);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.newDGV1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(380, 504);
            this.panel2.TabIndex = 0;
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToDeleteRows = false;
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
            this.Num,
            this.carYearDataGridViewTextBoxColumn,
            this.carNoDataGridViewTextBoxColumn,
            this.mobileNoDataGridViewTextBoxColumn});
            this.newDGV1.DataSource = this.driversBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle3;
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
            this.newDGV1.Size = new System.Drawing.Size(380, 504);
            this.newDGV1.TabIndex = 2;
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            // 
            // Num
            // 
            this.Num.HeaderText = "번호";
            this.Num.Name = "Num";
            this.Num.ReadOnly = true;
            this.Num.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Num.Width = 50;
            // 
            // carYearDataGridViewTextBoxColumn
            // 
            this.carYearDataGridViewTextBoxColumn.DataPropertyName = "CarYear";
            this.carYearDataGridViewTextBoxColumn.HeaderText = "기사명";
            this.carYearDataGridViewTextBoxColumn.Name = "carYearDataGridViewTextBoxColumn";
            this.carYearDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // carNoDataGridViewTextBoxColumn
            // 
            this.carNoDataGridViewTextBoxColumn.DataPropertyName = "CarNo";
            this.carNoDataGridViewTextBoxColumn.HeaderText = "차량번호";
            this.carNoDataGridViewTextBoxColumn.Name = "carNoDataGridViewTextBoxColumn";
            this.carNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mobileNoDataGridViewTextBoxColumn
            // 
            this.mobileNoDataGridViewTextBoxColumn.DataPropertyName = "MobileNo";
            this.mobileNoDataGridViewTextBoxColumn.HeaderText = "핸드폰번호";
            this.mobileNoDataGridViewTextBoxColumn.Name = "mobileNoDataGridViewTextBoxColumn";
            this.mobileNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // driversBindingSource
            // 
            this.driversBindingSource.DataMember = "Drivers";
            this.driversBindingSource.DataSource = this.baseDataSet;
            // 
            // baseDataSet
            // 
            this.baseDataSet.DataSetName = "BaseDataSet";
            this.baseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(389, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(381, 504);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.pn_4_month);
            this.panel5.Controls.Add(this.lbl_Month4);
            this.panel5.Controls.Add(this.pn_3_month);
            this.panel5.Controls.Add(this.lbl_Month3);
            this.panel5.Controls.Add(this.pn_2_month);
            this.panel5.Controls.Add(this.lbl_Month2);
            this.panel5.Controls.Add(this.pn_1_month);
            this.panel5.Controls.Add(this.lbl_Month1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 34);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(381, 470);
            this.panel5.TabIndex = 1;
            // 
            // pn_4_month
            // 
            this.pn_4_month.Dock = System.Windows.Forms.DockStyle.Top;
            this.pn_4_month.Location = new System.Drawing.Point(0, 360);
            this.pn_4_month.Name = "pn_4_month";
            this.pn_4_month.Size = new System.Drawing.Size(381, 100);
            this.pn_4_month.TabIndex = 7;
            // 
            // lbl_Month4
            // 
            this.lbl_Month4.AutoSize = true;
            this.lbl_Month4.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Month4.Location = new System.Drawing.Point(0, 345);
            this.lbl_Month4.Name = "lbl_Month4";
            this.lbl_Month4.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.lbl_Month4.Size = new System.Drawing.Size(68, 15);
            this.lbl_Month4.TabIndex = 6;
            this.lbl_Month4.Text = "2014年11月";
            // 
            // pn_3_month
            // 
            this.pn_3_month.Dock = System.Windows.Forms.DockStyle.Top;
            this.pn_3_month.Location = new System.Drawing.Point(0, 245);
            this.pn_3_month.Name = "pn_3_month";
            this.pn_3_month.Size = new System.Drawing.Size(381, 100);
            this.pn_3_month.TabIndex = 5;
            // 
            // lbl_Month3
            // 
            this.lbl_Month3.AutoSize = true;
            this.lbl_Month3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Month3.Location = new System.Drawing.Point(0, 230);
            this.lbl_Month3.Name = "lbl_Month3";
            this.lbl_Month3.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.lbl_Month3.Size = new System.Drawing.Size(68, 15);
            this.lbl_Month3.TabIndex = 4;
            this.lbl_Month3.Text = "2014年11月";
            // 
            // pn_2_month
            // 
            this.pn_2_month.Dock = System.Windows.Forms.DockStyle.Top;
            this.pn_2_month.Location = new System.Drawing.Point(0, 130);
            this.pn_2_month.Name = "pn_2_month";
            this.pn_2_month.Size = new System.Drawing.Size(381, 100);
            this.pn_2_month.TabIndex = 3;
            // 
            // lbl_Month2
            // 
            this.lbl_Month2.AutoSize = true;
            this.lbl_Month2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Month2.Location = new System.Drawing.Point(0, 115);
            this.lbl_Month2.Name = "lbl_Month2";
            this.lbl_Month2.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.lbl_Month2.Size = new System.Drawing.Size(68, 15);
            this.lbl_Month2.TabIndex = 2;
            this.lbl_Month2.Text = "2014年11月";
            // 
            // pn_1_month
            // 
            this.pn_1_month.Dock = System.Windows.Forms.DockStyle.Top;
            this.pn_1_month.Location = new System.Drawing.Point(0, 15);
            this.pn_1_month.Name = "pn_1_month";
            this.pn_1_month.Size = new System.Drawing.Size(381, 100);
            this.pn_1_month.TabIndex = 1;
            // 
            // lbl_Month1
            // 
            this.lbl_Month1.AutoSize = true;
            this.lbl_Month1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Month1.Location = new System.Drawing.Point(0, 0);
            this.lbl_Month1.Name = "lbl_Month1";
            this.lbl_Month1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.lbl_Month1.Size = new System.Drawing.Size(68, 15);
            this.lbl_Month1.TabIndex = 0;
            this.lbl_Month1.Text = "2014年11月";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dtpStart);
            this.panel4.Controls.Add(this.dtpEnd);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(381, 34);
            this.panel4.TabIndex = 0;
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStart.CustomFormat = "yyyy/MM/dd";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(159, 7);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(98, 21);
            this.dtpStart.TabIndex = 408;
            this.dtpStart.TabStop = false;
            this.dtpStart.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.CustomFormat = "yyyy/MM/dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(275, 7);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(99, 21);
            this.dtpEnd.TabIndex = 409;
            this.dtpEnd.TabStop = false;
            this.dtpEnd.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtpEnd_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(257, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 28);
            this.label2.TabIndex = 410;
            this.label2.Text = "~";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "기간(최대3개월) :";
            // 
            // dtgLogTableAdapter
            // 
            this.dtgLogTableAdapter.ClearBeforeFill = true;
            // 
            // driverGroupsTableAdapter
            // 
            this.driverGroupsTableAdapter.ClearBeforeFill = true;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // driversTableAdapter
            // 
            this.driversTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AdminInfoesTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.DriverInstancesTableAdapter = null;
            this.tableAdapterManager.DriverPapersTableAdapter = null;
            this.tableAdapterManager.DriverPointsTableAdapter = null;
            this.tableAdapterManager.MenuListTableAdapter = null;
            this.tableAdapterManager.StaticOptionsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = mycalltruck.Admin.DataSets.BaseDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.UserAuthorityTableAdapter = null;
            // 
            // Frm0104_DTGTOTAL
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(773, 510);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm0104_DTGTOTAL";
            this.Text = "기사별 집계";
            this.Load += new System.EventHandler(this.Frm0104_DTGTOTAL_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label2;
        private NewDGV newDGV1;
        private System.Windows.Forms.Label lbl_Month1;
        private System.Windows.Forms.Panel pn_4_month;
        private System.Windows.Forms.Label lbl_Month4;
        private System.Windows.Forms.Panel pn_3_month;
        private System.Windows.Forms.Label lbl_Month3;
        private System.Windows.Forms.Panel pn_2_month;
        private System.Windows.Forms.Label lbl_Month2;
        private System.Windows.Forms.Panel pn_1_month;
        private CMDataSetTableAdapters.DTGLogTableAdapter dtgLogTableAdapter;
        private CMDataSetTableAdapters.DriverGroupsTableAdapter driverGroupsTableAdapter;
        private CMDataSet cMDataSet;
        private DataSets.BaseDataSet baseDataSet;
        private System.Windows.Forms.BindingSource driversBindingSource;
        private DataSets.BaseDataSetTableAdapters.DriversTableAdapter driversTableAdapter;
        private DataSets.BaseDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn Num;
        private System.Windows.Forms.DataGridViewTextBoxColumn carYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobileNoDataGridViewTextBoxColumn;
    }
}