namespace mycalltruck.Admin
{
    partial class FrmAPPSMSLIST
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmb_SmsState = new System.Windows.Forms.ComboBox();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.btn_Export = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.appSMSUseListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.appSMSDataSet = new mycalltruck.Admin.DataSets.AppSMSDataSet();
            this.appSMSUseListTableAdapter = new mycalltruck.Admin.DataSets.AppSMSDataSetTableAdapters.AppSMSUseListTableAdapter();
            this.baseDataSet = new mycalltruck.Admin.DataSets.BaseDataSet();
            this.tablesTableAdapter = new mycalltruck.Admin.DataSets.BaseDataSetTableAdapters.TABLESTableAdapter();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.ColumnNo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateclientreqDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobileNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smsgubun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smsResultDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smsErrorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mtrefkeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appSMSUseListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appSMSDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmb_SmsState);
            this.panel1.Controls.Add(this.txt_Search);
            this.panel1.Controls.Add(this.cmb_Search);
            this.panel1.Controls.Add(this.btn_Export);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Controls.Add(this.dtpStart);
            this.panel1.Controls.Add(this.btn_Inew);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 43);
            this.panel1.TabIndex = 49;
            // 
            // cmb_SmsState
            // 
            this.cmb_SmsState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_SmsState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_SmsState.FormattingEnabled = true;
            this.cmb_SmsState.Items.AddRange(new object[] {
            "전송결과",
            "성공",
            "실패(어플설치)",
            "실패(당일중복전송)",
            "실패(전체)"});
            this.cmb_SmsState.Location = new System.Drawing.Point(572, 7);
            this.cmb_SmsState.Name = "cmb_SmsState";
            this.cmb_SmsState.Size = new System.Drawing.Size(118, 25);
            this.cmb_SmsState.TabIndex = 59;
            this.cmb_SmsState.TabStop = false;
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Search.Location = new System.Drawing.Point(778, 7);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(87, 25);
            this.txt_Search.TabIndex = 58;
            this.txt_Search.TabStop = false;
            // 
            // cmb_Search
            // 
            this.cmb_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Items.AddRange(new object[] {
            "전체",
            "운송사코드",
            "운송사명",
            "차주아이디",
            "차주명",
            "핸드폰번호",
            "차량번호"});
            this.cmb_Search.Location = new System.Drawing.Point(694, 7);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(80, 25);
            this.cmb_Search.TabIndex = 57;
            this.cmb_Search.TabStop = false;
            // 
            // btn_Export
            // 
            this.btn_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Export.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Export.FlatAppearance.BorderSize = 0;
            this.btn_Export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Export.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Export.ForeColor = System.Drawing.Color.White;
            this.btn_Export.Location = new System.Drawing.Point(313, 7);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(75, 27);
            this.btn_Export.TabIndex = 56;
            this.btn_Export.Text = "내려받기";
            this.btn_Export.UseVisualStyleBackColor = false;
            this.btn_Export.Visible = false;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(4, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(154, 19);
            this.label11.TabIndex = 48;
            this.label11.Text = "APP설치문자 전송내역";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.CustomFormat = "yyyy/MM/dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(483, 7);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(86, 25);
            this.dtpEnd.TabIndex = 46;
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStart.CustomFormat = "yyyy/MM/dd";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(394, 7);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(86, 25);
            this.dtpStart.TabIndex = 45;
            // 
            // btn_Inew
            // 
            this.btn_Inew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Inew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Inew.FlatAppearance.BorderSize = 0;
            this.btn_Inew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Inew.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Inew.ForeColor = System.Drawing.Color.White;
            this.btn_Inew.Location = new System.Drawing.Point(933, 7);
            this.btn_Inew.Name = "btn_Inew";
            this.btn_Inew.Size = new System.Drawing.Size(76, 27);
            this.btn_Inew.TabIndex = 44;
            this.btn_Inew.Text = "초 기 화";
            this.btn_Inew.UseVisualStyleBackColor = false;
            this.btn_Inew.Click += new System.EventHandler(this.btn_Inew_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Search.FlatAppearance.BorderSize = 0;
            this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Search.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(871, 7);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(56, 27);
            this.btn_Search.TabIndex = 43;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // appSMSUseListBindingSource
            // 
            this.appSMSUseListBindingSource.DataMember = "AppSMSUseList";
            this.appSMSUseListBindingSource.DataSource = this.appSMSDataSet;
            // 
            // appSMSDataSet
            // 
            this.appSMSDataSet.DataSetName = "AppSMSDataSet";
            this.appSMSDataSet.EnforceConstraints = false;
            this.appSMSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // appSMSUseListTableAdapter
            // 
            this.appSMSUseListTableAdapter.ClearBeforeFill = true;
            // 
            // baseDataSet
            // 
            this.baseDataSet.DataSetName = "BaseDataSet";
            this.baseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tablesTableAdapter
            // 
            this.tablesTableAdapter.ClearBeforeFill = true;
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNo1,
            this.dateclientreqDataGridViewTextBoxColumn,
            this.clientCodeDataGridViewTextBoxColumn,
            this.clientNameDataGridViewTextBoxColumn,
            this.loginIdDataGridViewTextBoxColumn,
            this.driverNameDataGridViewTextBoxColumn,
            this.mobileNoDataGridViewTextBoxColumn,
            this.carNoDataGridViewTextBoxColumn,
            this.smsgubun,
            this.smsResultDataGridViewTextBoxColumn,
            this.smsErrorDataGridViewTextBoxColumn,
            this.mtrefkeyDataGridViewTextBoxColumn});
            this.newDGV1.DataSource = this.appSMSUseListBindingSource;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle10;
            this.newDGV1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(0, 43);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.MultiSelect = false;
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newDGV1.Size = new System.Drawing.Size(1012, 573);
            this.newDGV1.TabIndex = 50;
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            // 
            // ColumnNo1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnNo1.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnNo1.HeaderText = "번호";
            this.ColumnNo1.Name = "ColumnNo1";
            this.ColumnNo1.ReadOnly = true;
            this.ColumnNo1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnNo1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnNo1.Width = 60;
            // 
            // dateclientreqDataGridViewTextBoxColumn
            // 
            this.dateclientreqDataGridViewTextBoxColumn.DataPropertyName = "date_client_req";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dateclientreqDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.dateclientreqDataGridViewTextBoxColumn.HeaderText = "전송일시";
            this.dateclientreqDataGridViewTextBoxColumn.Name = "dateclientreqDataGridViewTextBoxColumn";
            this.dateclientreqDataGridViewTextBoxColumn.Width = 135;
            // 
            // clientCodeDataGridViewTextBoxColumn
            // 
            this.clientCodeDataGridViewTextBoxColumn.DataPropertyName = "ClientCode";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.clientCodeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.clientCodeDataGridViewTextBoxColumn.HeaderText = "운송사코드";
            this.clientCodeDataGridViewTextBoxColumn.Name = "clientCodeDataGridViewTextBoxColumn";
            // 
            // clientNameDataGridViewTextBoxColumn
            // 
            this.clientNameDataGridViewTextBoxColumn.DataPropertyName = "ClientName";
            this.clientNameDataGridViewTextBoxColumn.HeaderText = "운송사명";
            this.clientNameDataGridViewTextBoxColumn.Name = "clientNameDataGridViewTextBoxColumn";
            // 
            // loginIdDataGridViewTextBoxColumn
            // 
            this.loginIdDataGridViewTextBoxColumn.DataPropertyName = "LoginId";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.loginIdDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.loginIdDataGridViewTextBoxColumn.HeaderText = "차주아이디";
            this.loginIdDataGridViewTextBoxColumn.Name = "loginIdDataGridViewTextBoxColumn";
            // 
            // driverNameDataGridViewTextBoxColumn
            // 
            this.driverNameDataGridViewTextBoxColumn.DataPropertyName = "DriverName";
            this.driverNameDataGridViewTextBoxColumn.HeaderText = "차주명";
            this.driverNameDataGridViewTextBoxColumn.Name = "driverNameDataGridViewTextBoxColumn";
            // 
            // mobileNoDataGridViewTextBoxColumn
            // 
            this.mobileNoDataGridViewTextBoxColumn.DataPropertyName = "MobileNo";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mobileNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.mobileNoDataGridViewTextBoxColumn.HeaderText = "핸드폰번호";
            this.mobileNoDataGridViewTextBoxColumn.Name = "mobileNoDataGridViewTextBoxColumn";
            this.mobileNoDataGridViewTextBoxColumn.Width = 120;
            // 
            // carNoDataGridViewTextBoxColumn
            // 
            this.carNoDataGridViewTextBoxColumn.DataPropertyName = "CarNo";
            this.carNoDataGridViewTextBoxColumn.HeaderText = "차량번호";
            this.carNoDataGridViewTextBoxColumn.Name = "carNoDataGridViewTextBoxColumn";
            // 
            // smsgubun
            // 
            this.smsgubun.DataPropertyName = "smsgubun";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smsgubun.DefaultCellStyle = dataGridViewCellStyle8;
            this.smsgubun.HeaderText = "문자구분";
            this.smsgubun.Name = "smsgubun";
            // 
            // smsResultDataGridViewTextBoxColumn
            // 
            this.smsResultDataGridViewTextBoxColumn.DataPropertyName = "SmsResult";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smsResultDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.smsResultDataGridViewTextBoxColumn.HeaderText = "결과";
            this.smsResultDataGridViewTextBoxColumn.Name = "smsResultDataGridViewTextBoxColumn";
            this.smsResultDataGridViewTextBoxColumn.ReadOnly = true;
            this.smsResultDataGridViewTextBoxColumn.Width = 60;
            // 
            // smsErrorDataGridViewTextBoxColumn
            // 
            this.smsErrorDataGridViewTextBoxColumn.DataPropertyName = "SmsError";
            this.smsErrorDataGridViewTextBoxColumn.HeaderText = "전송실패내역";
            this.smsErrorDataGridViewTextBoxColumn.Name = "smsErrorDataGridViewTextBoxColumn";
            this.smsErrorDataGridViewTextBoxColumn.ReadOnly = true;
            this.smsErrorDataGridViewTextBoxColumn.Width = 300;
            // 
            // mtrefkeyDataGridViewTextBoxColumn
            // 
            this.mtrefkeyDataGridViewTextBoxColumn.DataPropertyName = "mt_refkey";
            this.mtrefkeyDataGridViewTextBoxColumn.HeaderText = "mt_refkey";
            this.mtrefkeyDataGridViewTextBoxColumn.Name = "mtrefkeyDataGridViewTextBoxColumn";
            this.mtrefkeyDataGridViewTextBoxColumn.Visible = false;
            // 
            // FrmAPPSMSLIST
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1012, 616);
            this.Controls.Add(this.newDGV1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmAPPSMSLIST";
            this.Text = "APP설치 전송문자 사용내역";
            this.Load += new System.EventHandler(this.FRMMNUSELIST_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appSMSUseListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appSMSDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Button btn_Export;
        public System.Windows.Forms.ComboBox cmb_SmsState;
        private System.Windows.Forms.TextBox txt_Search;
        public System.Windows.Forms.ComboBox cmb_Search;
        private NewDGV newDGV1;
        private DataSets.AppSMSDataSet appSMSDataSet;
        private System.Windows.Forms.BindingSource appSMSUseListBindingSource;
        private DataSets.AppSMSDataSetTableAdapters.AppSMSUseListTableAdapter appSMSUseListTableAdapter;
        private DataSets.BaseDataSet baseDataSet;
        private DataSets.BaseDataSetTableAdapters.TABLESTableAdapter tablesTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateclientreqDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobileNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn smsgubun;
        private System.Windows.Forms.DataGridViewTextBoxColumn smsResultDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn smsErrorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mtrefkeyDataGridViewTextBoxColumn;
    }
}