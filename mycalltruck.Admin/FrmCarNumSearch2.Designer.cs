namespace mycalltruck.Admin
{
    partial class FrmCarNumSearch2
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
            this.panel17 = new System.Windows.Forms.Panel();
            this.btn_new = new System.Windows.Forms.Button();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid1 = new mycalltruck.Admin.NewDGV();
            this.driversCarBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.drivers_CarTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.Drivers_CarTableAdapter();
            this.cEODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MobileNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversCarBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(363, 262);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(363, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.btn_new);
            this.panel17.Controls.Add(this.cmb_Search);
            this.panel17.Controls.Add(this.btn_Search);
            this.panel17.Controls.Add(this.txt_Search);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Margin = new System.Windows.Forms.Padding(0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(363, 30);
            this.panel17.TabIndex = 1;
            // 
            // btn_new
            // 
            this.btn_new.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_new.Location = new System.Drawing.Point(303, 3);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(53, 23);
            this.btn_new.TabIndex = 62;
            this.btn_new.Text = "초기화";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // cmb_Search
            // 
            this.cmb_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Items.AddRange(new object[] {
            "상호",
            "차량번호"});
            this.cmb_Search.Location = new System.Drawing.Point(3, 5);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(75, 20);
            this.cmb_Search.TabIndex = 61;
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.Location = new System.Drawing.Point(248, 3);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(53, 23);
            this.btn_Search.TabIndex = 39;
            this.btn_Search.Text = "검 색";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Location = new System.Drawing.Point(84, 4);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(146, 21);
            this.txt_Search.TabIndex = 1;
            this.txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(363, 232);
            this.panel2.TabIndex = 2;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.grid1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.AutoGenerateColumns = false;
            this.grid1.BackgroundColor = System.Drawing.Color.White;
            this.grid1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cEODataGridViewTextBoxColumn,
            this.carNoDataGridViewTextBoxColumn,
            this.CarYear,
            this.MobileNo,
            this.PhoneNo,
            this.driverIdDataGridViewTextBoxColumn,
            this.bizNoDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.Code});
            this.grid1.DataSource = this.driversCarBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid1.DefaultCellStyle = dataGridViewCellStyle3;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.GridColor = System.Drawing.Color.White;
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Margin = new System.Windows.Forms.Padding(0);
            this.grid1.Name = "grid1";
            this.grid1.ReadOnly = true;
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grid1.RowTemplate.Height = 23;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(363, 232);
            this.grid1.TabIndex = 0;
            // 
            // driversCarBindingSource
            // 
            this.driversCarBindingSource.DataMember = "Drivers_Car";
            this.driversCarBindingSource.DataSource = this.cMDataSet;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // drivers_CarTableAdapter
            // 
            this.drivers_CarTableAdapter.ClearBeforeFill = true;
            // 
            // cEODataGridViewTextBoxColumn
            // 
            this.cEODataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.cEODataGridViewTextBoxColumn.HeaderText = "상호";
            this.cEODataGridViewTextBoxColumn.Name = "cEODataGridViewTextBoxColumn";
            this.cEODataGridViewTextBoxColumn.ReadOnly = true;
            this.cEODataGridViewTextBoxColumn.Width = 180;
            // 
            // carNoDataGridViewTextBoxColumn
            // 
            this.carNoDataGridViewTextBoxColumn.DataPropertyName = "CarNo";
            this.carNoDataGridViewTextBoxColumn.HeaderText = "차량번호";
            this.carNoDataGridViewTextBoxColumn.Name = "carNoDataGridViewTextBoxColumn";
            this.carNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.carNoDataGridViewTextBoxColumn.Width = 140;
            // 
            // CarYear
            // 
            this.CarYear.DataPropertyName = "CarYear";
            this.CarYear.HeaderText = "기사명";
            this.CarYear.Name = "CarYear";
            this.CarYear.ReadOnly = true;
            this.CarYear.Visible = false;
            // 
            // MobileNo
            // 
            this.MobileNo.DataPropertyName = "MobileNo";
            this.MobileNo.HeaderText = "MobileNo";
            this.MobileNo.Name = "MobileNo";
            this.MobileNo.ReadOnly = true;
            this.MobileNo.Visible = false;
            // 
            // PhoneNo
            // 
            this.PhoneNo.DataPropertyName = "PhoneNo";
            this.PhoneNo.HeaderText = "PhoneNo";
            this.PhoneNo.Name = "PhoneNo";
            this.PhoneNo.ReadOnly = true;
            this.PhoneNo.Visible = false;
            // 
            // driverIdDataGridViewTextBoxColumn
            // 
            this.driverIdDataGridViewTextBoxColumn.DataPropertyName = "DriverId";
            this.driverIdDataGridViewTextBoxColumn.HeaderText = "DriverId";
            this.driverIdDataGridViewTextBoxColumn.Name = "driverIdDataGridViewTextBoxColumn";
            this.driverIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // bizNoDataGridViewTextBoxColumn
            // 
            this.bizNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            this.bizNoDataGridViewTextBoxColumn.HeaderText = "BizNo";
            this.bizNoDataGridViewTextBoxColumn.Name = "bizNoDataGridViewTextBoxColumn";
            this.bizNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.bizNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Visible = false;
            // 
            // Code
            // 
            this.Code.DataPropertyName = "Code";
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Visible = false;
            // 
            // FrmCarNumSearch2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(363, 262);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCarNumSearch2";
            this.ShowIcon = false;
            this.Text = "차량번호 검색";
            this.Load += new System.EventHandler(this.FrmCarNumSearch_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversCarBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.BindingSource driversCarBindingSource;
        private CMDataSet cMDataSet;
        private CMDataSetTableAdapters.Drivers_CarTableAdapter drivers_CarTableAdapter;
        public NewDGV grid1;
        public System.Windows.Forms.ComboBox cmb_Search;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn MobileNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhoneNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
    }
}