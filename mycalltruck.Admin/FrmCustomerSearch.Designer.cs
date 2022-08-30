namespace mycalltruck.Admin
{
    partial class FrmCustomerSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel17 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_new = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid1 = new mycalltruck.Admin.NewDGV();
            this.sangHoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uptaeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.upjongDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressStateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressCityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressDetailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chargeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobileNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizGubunDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resgisterNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.salesGubunDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phoneNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.faxNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zipcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.customersTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.CustomersTableAdapter();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 262);
            this.panel1.TabIndex = 1;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(429, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.label1);
            this.panel17.Controls.Add(this.btn_new);
            this.panel17.Controls.Add(this.btn_Search);
            this.panel17.Controls.Add(this.txt_Search);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel17.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Margin = new System.Windows.Forms.Padding(0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(429, 43);
            this.panel17.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "선택 :";
            // 
            // btn_new
            // 
            this.btn_new.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_new.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_new.FlatAppearance.BorderSize = 0;
            this.btn_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_new.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_new.ForeColor = System.Drawing.Color.White;
            this.btn_new.Location = new System.Drawing.Point(356, 7);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(66, 27);
            this.btn_new.TabIndex = 62;
            this.btn_new.Text = "초기화";
            this.btn_new.UseVisualStyleBackColor = false;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Search.FlatAppearance.BorderSize = 0;
            this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Search.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(297, 7);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(53, 27);
            this.btn_Search.TabIndex = 39;
            this.btn_Search.Text = "검 색";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Location = new System.Drawing.Point(145, 7);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(146, 25);
            this.txt_Search.TabIndex = 1;
            this.txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 43);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(429, 219);
            this.panel2.TabIndex = 2;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.grid1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.grid1.AutoGenerateColumns = false;
            this.grid1.BackgroundColor = System.Drawing.Color.White;
            this.grid1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sangHoDataGridViewTextBoxColumn,
            this.bizNoDataGridViewTextBoxColumn,
            this.ceoDataGridViewTextBoxColumn,
            this.uptaeDataGridViewTextBoxColumn,
            this.upjongDataGridViewTextBoxColumn,
            this.addressStateDataGridViewTextBoxColumn,
            this.addressCityDataGridViewTextBoxColumn,
            this.addressDetailDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.chargeNameDataGridViewTextBoxColumn,
            this.mobileNoDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.customerIdDataGridViewTextBoxColumn,
            this.bizGubunDataGridViewTextBoxColumn,
            this.resgisterNoDataGridViewTextBoxColumn,
            this.salesGubunDataGridViewTextBoxColumn,
            this.phoneNoDataGridViewTextBoxColumn,
            this.faxNoDataGridViewTextBoxColumn,
            this.createTimeDataGridViewTextBoxColumn,
            this.clientIdDataGridViewTextBoxColumn,
            this.Zipcode});
            this.grid1.DataSource = this.customersBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid1.DefaultCellStyle = dataGridViewCellStyle6;
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
            this.grid1.Size = new System.Drawing.Size(429, 219);
            this.grid1.TabIndex = 0;
            // 
            // sangHoDataGridViewTextBoxColumn
            // 
            this.sangHoDataGridViewTextBoxColumn.DataPropertyName = "SangHo";
            this.sangHoDataGridViewTextBoxColumn.HeaderText = "거래처명";
            this.sangHoDataGridViewTextBoxColumn.Name = "sangHoDataGridViewTextBoxColumn";
            this.sangHoDataGridViewTextBoxColumn.ReadOnly = true;
            this.sangHoDataGridViewTextBoxColumn.Width = 170;
            // 
            // bizNoDataGridViewTextBoxColumn
            // 
            this.bizNoDataGridViewTextBoxColumn.DataPropertyName = "BizNo";
            this.bizNoDataGridViewTextBoxColumn.HeaderText = "사업자번호";
            this.bizNoDataGridViewTextBoxColumn.Name = "bizNoDataGridViewTextBoxColumn";
            this.bizNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.bizNoDataGridViewTextBoxColumn.Width = 130;
            // 
            // ceoDataGridViewTextBoxColumn
            // 
            this.ceoDataGridViewTextBoxColumn.DataPropertyName = "Ceo";
            this.ceoDataGridViewTextBoxColumn.HeaderText = "대표자";
            this.ceoDataGridViewTextBoxColumn.Name = "ceoDataGridViewTextBoxColumn";
            this.ceoDataGridViewTextBoxColumn.ReadOnly = true;
            this.ceoDataGridViewTextBoxColumn.Width = 110;
            // 
            // uptaeDataGridViewTextBoxColumn
            // 
            this.uptaeDataGridViewTextBoxColumn.DataPropertyName = "Uptae";
            this.uptaeDataGridViewTextBoxColumn.HeaderText = "Uptae";
            this.uptaeDataGridViewTextBoxColumn.Name = "uptaeDataGridViewTextBoxColumn";
            this.uptaeDataGridViewTextBoxColumn.ReadOnly = true;
            this.uptaeDataGridViewTextBoxColumn.Visible = false;
            // 
            // upjongDataGridViewTextBoxColumn
            // 
            this.upjongDataGridViewTextBoxColumn.DataPropertyName = "Upjong";
            this.upjongDataGridViewTextBoxColumn.HeaderText = "Upjong";
            this.upjongDataGridViewTextBoxColumn.Name = "upjongDataGridViewTextBoxColumn";
            this.upjongDataGridViewTextBoxColumn.ReadOnly = true;
            this.upjongDataGridViewTextBoxColumn.Visible = false;
            // 
            // addressStateDataGridViewTextBoxColumn
            // 
            this.addressStateDataGridViewTextBoxColumn.DataPropertyName = "AddressState";
            this.addressStateDataGridViewTextBoxColumn.HeaderText = "AddressState";
            this.addressStateDataGridViewTextBoxColumn.Name = "addressStateDataGridViewTextBoxColumn";
            this.addressStateDataGridViewTextBoxColumn.ReadOnly = true;
            this.addressStateDataGridViewTextBoxColumn.Visible = false;
            // 
            // addressCityDataGridViewTextBoxColumn
            // 
            this.addressCityDataGridViewTextBoxColumn.DataPropertyName = "AddressCity";
            this.addressCityDataGridViewTextBoxColumn.HeaderText = "AddressCity";
            this.addressCityDataGridViewTextBoxColumn.Name = "addressCityDataGridViewTextBoxColumn";
            this.addressCityDataGridViewTextBoxColumn.ReadOnly = true;
            this.addressCityDataGridViewTextBoxColumn.Visible = false;
            // 
            // addressDetailDataGridViewTextBoxColumn
            // 
            this.addressDetailDataGridViewTextBoxColumn.DataPropertyName = "AddressDetail";
            this.addressDetailDataGridViewTextBoxColumn.HeaderText = "AddressDetail";
            this.addressDetailDataGridViewTextBoxColumn.Name = "addressDetailDataGridViewTextBoxColumn";
            this.addressDetailDataGridViewTextBoxColumn.ReadOnly = true;
            this.addressDetailDataGridViewTextBoxColumn.Visible = false;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "Email";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            this.emailDataGridViewTextBoxColumn.ReadOnly = true;
            this.emailDataGridViewTextBoxColumn.Visible = false;
            // 
            // chargeNameDataGridViewTextBoxColumn
            // 
            this.chargeNameDataGridViewTextBoxColumn.DataPropertyName = "ChargeName";
            this.chargeNameDataGridViewTextBoxColumn.HeaderText = "ChargeName";
            this.chargeNameDataGridViewTextBoxColumn.Name = "chargeNameDataGridViewTextBoxColumn";
            this.chargeNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.chargeNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // mobileNoDataGridViewTextBoxColumn
            // 
            this.mobileNoDataGridViewTextBoxColumn.DataPropertyName = "MobileNo";
            this.mobileNoDataGridViewTextBoxColumn.HeaderText = "MobileNo";
            this.mobileNoDataGridViewTextBoxColumn.Name = "mobileNoDataGridViewTextBoxColumn";
            this.mobileNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.mobileNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            this.codeDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerIdDataGridViewTextBoxColumn
            // 
            this.customerIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.HeaderText = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.Name = "customerIdDataGridViewTextBoxColumn";
            this.customerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // bizGubunDataGridViewTextBoxColumn
            // 
            this.bizGubunDataGridViewTextBoxColumn.DataPropertyName = "BizGubun";
            this.bizGubunDataGridViewTextBoxColumn.HeaderText = "BizGubun";
            this.bizGubunDataGridViewTextBoxColumn.Name = "bizGubunDataGridViewTextBoxColumn";
            this.bizGubunDataGridViewTextBoxColumn.ReadOnly = true;
            this.bizGubunDataGridViewTextBoxColumn.Visible = false;
            // 
            // resgisterNoDataGridViewTextBoxColumn
            // 
            this.resgisterNoDataGridViewTextBoxColumn.DataPropertyName = "ResgisterNo";
            this.resgisterNoDataGridViewTextBoxColumn.HeaderText = "ResgisterNo";
            this.resgisterNoDataGridViewTextBoxColumn.Name = "resgisterNoDataGridViewTextBoxColumn";
            this.resgisterNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.resgisterNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // salesGubunDataGridViewTextBoxColumn
            // 
            this.salesGubunDataGridViewTextBoxColumn.DataPropertyName = "SalesGubun";
            this.salesGubunDataGridViewTextBoxColumn.HeaderText = "전표구분";
            this.salesGubunDataGridViewTextBoxColumn.Name = "salesGubunDataGridViewTextBoxColumn";
            this.salesGubunDataGridViewTextBoxColumn.ReadOnly = true;
            this.salesGubunDataGridViewTextBoxColumn.Visible = false;
            // 
            // phoneNoDataGridViewTextBoxColumn
            // 
            this.phoneNoDataGridViewTextBoxColumn.DataPropertyName = "PhoneNo";
            this.phoneNoDataGridViewTextBoxColumn.HeaderText = "PhoneNo";
            this.phoneNoDataGridViewTextBoxColumn.Name = "phoneNoDataGridViewTextBoxColumn";
            this.phoneNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.phoneNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // faxNoDataGridViewTextBoxColumn
            // 
            this.faxNoDataGridViewTextBoxColumn.DataPropertyName = "FaxNo";
            this.faxNoDataGridViewTextBoxColumn.HeaderText = "FaxNo";
            this.faxNoDataGridViewTextBoxColumn.Name = "faxNoDataGridViewTextBoxColumn";
            this.faxNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.faxNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // createTimeDataGridViewTextBoxColumn
            // 
            this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "CreateTime";
            this.createTimeDataGridViewTextBoxColumn.HeaderText = "CreateTime";
            this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
            this.createTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.createTimeDataGridViewTextBoxColumn.Visible = false;
            // 
            // clientIdDataGridViewTextBoxColumn
            // 
            this.clientIdDataGridViewTextBoxColumn.DataPropertyName = "ClientId";
            this.clientIdDataGridViewTextBoxColumn.HeaderText = "ClientId";
            this.clientIdDataGridViewTextBoxColumn.Name = "clientIdDataGridViewTextBoxColumn";
            this.clientIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.clientIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // Zipcode
            // 
            this.Zipcode.DataPropertyName = "Zipcode";
            this.Zipcode.HeaderText = "Zipcode";
            this.Zipcode.Name = "Zipcode";
            this.Zipcode.ReadOnly = true;
            this.Zipcode.Visible = false;
            // 
            // customersBindingSource
            // 
            this.customersBindingSource.DataMember = "Customers";
            this.customersBindingSource.DataSource = this.cMDataSet;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customersTableAdapter
            // 
            this.customersTableAdapter.ClearBeforeFill = true;
            // 
            // FrmCustomerSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(429, 262);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCustomerSearch";
            this.Text = "일반 거래처";
            this.Load += new System.EventHandler(this.FrmCustomerSearch_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Panel panel2;
        private CMDataSet cMDataSet;
        private System.Windows.Forms.BindingSource customersBindingSource;
        private CMDataSetTableAdapters.CustomersTableAdapter customersTableAdapter;
        public NewDGV grid1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sangHoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uptaeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn upjongDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addressStateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addressCityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addressDetailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chargeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobileNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizGubunDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resgisterNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn salesGubunDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn phoneNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn faxNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zipcode;
    }
}