namespace mycalltruck.Admin
{
    partial class FrmCustomerStoptList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.ColumnNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGubun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startPhoneNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAction = new System.Windows.Forms.DataGridViewButtonColumn();
            this.idxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startStateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startCityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startStreetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDetailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerStopManageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.customerStartManageDataSet = new mycalltruck.Admin.DataSets.CustomerStartManageDataSet();
            this.customerStopManageTableAdapter = new mycalltruck.Admin.DataSets.CustomerStartManageDataSetTableAdapters.CustomerStopManageTableAdapter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerStopManageBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerStartManageDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.newDGV1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(780, 357);
            this.panel1.TabIndex = 0;
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
            this.newDGV1.AutoGenerateColumns = false;
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
            this.ColumnNumber,
            this.startNameDataGridViewTextBoxColumn,
            this.ColumnStart,
            this.ColumnGubun,
            this.startPhoneNoDataGridViewTextBoxColumn,
            this.ColumnAction,
            this.idxDataGridViewTextBoxColumn,
            this.startStateDataGridViewTextBoxColumn,
            this.startCityDataGridViewTextBoxColumn,
            this.startStreetDataGridViewTextBoxColumn,
            this.startDetailDataGridViewTextBoxColumn,
            this.clientIdDataGridViewTextBoxColumn,
            this.customerIdDataGridViewTextBoxColumn,
            this.createTimeDataGridViewTextBoxColumn});
            this.newDGV1.DataSource = this.customerStopManageBindingSource;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle5;
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
            this.newDGV1.Size = new System.Drawing.Size(780, 357);
            this.newDGV1.TabIndex = 5;
            this.newDGV1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newDGV1_CellContentClick);
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            this.newDGV1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.newDGV1_KeyDown);
            this.newDGV1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.newDGV1_KeyPress);
            // 
            // ColumnNumber
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnNumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnNumber.HeaderText = "번호";
            this.ColumnNumber.Name = "ColumnNumber";
            this.ColumnNumber.ReadOnly = true;
            this.ColumnNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnNumber.Width = 41;
            // 
            // startNameDataGridViewTextBoxColumn
            // 
            this.startNameDataGridViewTextBoxColumn.DataPropertyName = "StartName";
            this.startNameDataGridViewTextBoxColumn.HeaderText = "상호";
            this.startNameDataGridViewTextBoxColumn.Name = "startNameDataGridViewTextBoxColumn";
            this.startNameDataGridViewTextBoxColumn.Width = 130;
            // 
            // ColumnStart
            // 
            this.ColumnStart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnStart.HeaderText = "하차지주소";
            this.ColumnStart.Name = "ColumnStart";
            // 
            // ColumnGubun
            // 
            this.ColumnGubun.DataPropertyName = "Gubun";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnGubun.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnGubun.HeaderText = "구분";
            this.ColumnGubun.Name = "ColumnGubun";
            this.ColumnGubun.ReadOnly = true;
            this.ColumnGubun.Width = 80;
            // 
            // startPhoneNoDataGridViewTextBoxColumn
            // 
            this.startPhoneNoDataGridViewTextBoxColumn.DataPropertyName = "StartPhoneNo";
            this.startPhoneNoDataGridViewTextBoxColumn.HeaderText = "하차지 전화번호";
            this.startPhoneNoDataGridViewTextBoxColumn.Name = "startPhoneNoDataGridViewTextBoxColumn";
            this.startPhoneNoDataGridViewTextBoxColumn.Width = 120;
            // 
            // ColumnAction
            // 
            this.ColumnAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColumnAction.HeaderText = "";
            this.ColumnAction.Name = "ColumnAction";
            this.ColumnAction.Text = "선택";
            this.ColumnAction.UseColumnTextForButtonValue = true;
            this.ColumnAction.Width = 70;
            // 
            // idxDataGridViewTextBoxColumn
            // 
            this.idxDataGridViewTextBoxColumn.DataPropertyName = "idx";
            this.idxDataGridViewTextBoxColumn.HeaderText = "idx";
            this.idxDataGridViewTextBoxColumn.Name = "idxDataGridViewTextBoxColumn";
            this.idxDataGridViewTextBoxColumn.ReadOnly = true;
            this.idxDataGridViewTextBoxColumn.Visible = false;
            // 
            // startStateDataGridViewTextBoxColumn
            // 
            this.startStateDataGridViewTextBoxColumn.DataPropertyName = "StartState";
            this.startStateDataGridViewTextBoxColumn.HeaderText = "StartState";
            this.startStateDataGridViewTextBoxColumn.Name = "startStateDataGridViewTextBoxColumn";
            this.startStateDataGridViewTextBoxColumn.Visible = false;
            // 
            // startCityDataGridViewTextBoxColumn
            // 
            this.startCityDataGridViewTextBoxColumn.DataPropertyName = "StartCity";
            this.startCityDataGridViewTextBoxColumn.HeaderText = "StartCity";
            this.startCityDataGridViewTextBoxColumn.Name = "startCityDataGridViewTextBoxColumn";
            this.startCityDataGridViewTextBoxColumn.Visible = false;
            // 
            // startStreetDataGridViewTextBoxColumn
            // 
            this.startStreetDataGridViewTextBoxColumn.DataPropertyName = "StartStreet";
            this.startStreetDataGridViewTextBoxColumn.HeaderText = "StartStreet";
            this.startStreetDataGridViewTextBoxColumn.Name = "startStreetDataGridViewTextBoxColumn";
            this.startStreetDataGridViewTextBoxColumn.Visible = false;
            // 
            // startDetailDataGridViewTextBoxColumn
            // 
            this.startDetailDataGridViewTextBoxColumn.DataPropertyName = "StartDetail";
            this.startDetailDataGridViewTextBoxColumn.HeaderText = "StartDetail";
            this.startDetailDataGridViewTextBoxColumn.Name = "startDetailDataGridViewTextBoxColumn";
            this.startDetailDataGridViewTextBoxColumn.Visible = false;
            // 
            // clientIdDataGridViewTextBoxColumn
            // 
            this.clientIdDataGridViewTextBoxColumn.DataPropertyName = "ClientId";
            this.clientIdDataGridViewTextBoxColumn.HeaderText = "ClientId";
            this.clientIdDataGridViewTextBoxColumn.Name = "clientIdDataGridViewTextBoxColumn";
            this.clientIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerIdDataGridViewTextBoxColumn
            // 
            this.customerIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.HeaderText = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.Name = "customerIdDataGridViewTextBoxColumn";
            this.customerIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // createTimeDataGridViewTextBoxColumn
            // 
            this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "CreateTime";
            this.createTimeDataGridViewTextBoxColumn.HeaderText = "CreateTime";
            this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
            this.createTimeDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerStopManageBindingSource
            // 
            this.customerStopManageBindingSource.DataMember = "CustomerStopManage";
            this.customerStopManageBindingSource.DataSource = this.customerStartManageDataSet;
            // 
            // customerStartManageDataSet
            // 
            this.customerStartManageDataSet.DataSetName = "CustomerStartManageDataSet";
            this.customerStartManageDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customerStopManageTableAdapter
            // 
            this.customerStopManageTableAdapter.ClearBeforeFill = true;
            // 
            // FrmCustomerStoptList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(780, 357);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmCustomerStoptList";
            this.Text = "하차지정보";
            this.Load += new System.EventHandler(this.FrmCustomerStartList_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerStopManageBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerStartManageDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private NewDGV newDGV1;
        private DataSets.CustomerStartManageDataSet customerStartManageDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn startNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGubun;
        private System.Windows.Forms.DataGridViewTextBoxColumn startPhoneNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn idxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startStateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startCityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startStreetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDetailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource customerStopManageBindingSource;
        private DataSets.CustomerStartManageDataSetTableAdapters.CustomerStopManageTableAdapter customerStopManageTableAdapter;
    }
}