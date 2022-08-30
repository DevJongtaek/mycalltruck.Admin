namespace mycalltruck.Admin
{
    partial class FrmCargoZip
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cargoZipcodeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.cargoZipcodeTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.CargoZipcodeTableAdapter();
            this.grid1 = new mycalltruck.Admin.NewDGV();
            this.zipcodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addrDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sidoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siGunGuDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eupmyunDongDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gubunDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cargoZipcodeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(434, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(67, 23);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txt_Search.Location = new System.Drawing.Point(314, 12);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(114, 21);
            this.txt_Search.TabIndex = 21;
            this.txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "검색조건: ";
            // 
            // cmb_Search
            // 
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Location = new System.Drawing.Point(108, 12);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(121, 20);
            this.cmb_Search.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "검색어 입력: ";
            // 
            // cargoZipcodeBindingSource
            // 
            this.cargoZipcodeBindingSource.DataMember = "CargoZipcode";
            this.cargoZipcodeBindingSource.DataSource = this.cMDataSet;
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cargoZipcodeTableAdapter
            // 
            this.cargoZipcodeTableAdapter.ClearBeforeFill = true;
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
            this.zipcodeDataGridViewTextBoxColumn,
            this.addrDataGridViewTextBoxColumn,
            this.Column1,
            this.sidoDataGridViewTextBoxColumn,
            this.siGunGuDataGridViewTextBoxColumn,
            this.eupmyunDongDataGridViewTextBoxColumn,
            this.gubunDataGridViewTextBoxColumn});
            this.grid1.DataSource = this.cargoZipcodeBindingSource;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid1.DefaultCellStyle = dataGridViewCellStyle5;
            this.grid1.GridColor = System.Drawing.Color.White;
            this.grid1.Location = new System.Drawing.Point(9, 46);
            this.grid1.Margin = new System.Windows.Forms.Padding(0);
            this.grid1.Name = "grid1";
            this.grid1.ReadOnly = true;
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grid1.RowTemplate.Height = 23;
            this.grid1.Size = new System.Drawing.Size(501, 269);
            this.grid1.TabIndex = 23;
            // 
            // zipcodeDataGridViewTextBoxColumn
            // 
            this.zipcodeDataGridViewTextBoxColumn.DataPropertyName = "Zipcode";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.zipcodeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.zipcodeDataGridViewTextBoxColumn.HeaderText = "우편번호";
            this.zipcodeDataGridViewTextBoxColumn.Name = "zipcodeDataGridViewTextBoxColumn";
            this.zipcodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // addrDataGridViewTextBoxColumn
            // 
            this.addrDataGridViewTextBoxColumn.DataPropertyName = "Addr";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.addrDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.addrDataGridViewTextBoxColumn.HeaderText = "주소";
            this.addrDataGridViewTextBoxColumn.Name = "addrDataGridViewTextBoxColumn";
            this.addrDataGridViewTextBoxColumn.ReadOnly = true;
            this.addrDataGridViewTextBoxColumn.Width = 350;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Idx";
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // sidoDataGridViewTextBoxColumn
            // 
            this.sidoDataGridViewTextBoxColumn.DataPropertyName = "Sido";
            this.sidoDataGridViewTextBoxColumn.HeaderText = "Sido";
            this.sidoDataGridViewTextBoxColumn.Name = "sidoDataGridViewTextBoxColumn";
            this.sidoDataGridViewTextBoxColumn.ReadOnly = true;
            this.sidoDataGridViewTextBoxColumn.Visible = false;
            // 
            // siGunGuDataGridViewTextBoxColumn
            // 
            this.siGunGuDataGridViewTextBoxColumn.DataPropertyName = "SiGunGu";
            this.siGunGuDataGridViewTextBoxColumn.HeaderText = "SiGunGu";
            this.siGunGuDataGridViewTextBoxColumn.Name = "siGunGuDataGridViewTextBoxColumn";
            this.siGunGuDataGridViewTextBoxColumn.ReadOnly = true;
            this.siGunGuDataGridViewTextBoxColumn.Visible = false;
            // 
            // eupmyunDongDataGridViewTextBoxColumn
            // 
            this.eupmyunDongDataGridViewTextBoxColumn.DataPropertyName = "EupmyunDong";
            this.eupmyunDongDataGridViewTextBoxColumn.HeaderText = "EupmyunDong";
            this.eupmyunDongDataGridViewTextBoxColumn.Name = "eupmyunDongDataGridViewTextBoxColumn";
            this.eupmyunDongDataGridViewTextBoxColumn.ReadOnly = true;
            this.eupmyunDongDataGridViewTextBoxColumn.Visible = false;
            // 
            // gubunDataGridViewTextBoxColumn
            // 
            this.gubunDataGridViewTextBoxColumn.DataPropertyName = "Gubun";
            this.gubunDataGridViewTextBoxColumn.HeaderText = "Gubun";
            this.gubunDataGridViewTextBoxColumn.Name = "gubunDataGridViewTextBoxColumn";
            this.gubunDataGridViewTextBoxColumn.ReadOnly = true;
            this.gubunDataGridViewTextBoxColumn.Visible = false;
            // 
            // FrmCargoZip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(517, 320);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmb_Search);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txt_Search);
            this.Name = "FrmCargoZip";
            this.Text = "FPIS주소검색";
            this.Load += new System.EventHandler(this.FrmCargoZip_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cargoZipcodeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_Search;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource cargoZipcodeBindingSource;
        private CMDataSet cMDataSet;
        private CMDataSetTableAdapters.CargoZipcodeTableAdapter cargoZipcodeTableAdapter;
        public NewDGV grid1;
        private System.Windows.Forms.DataGridViewTextBoxColumn zipcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addrDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sidoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siGunGuDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eupmyunDongDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gubunDataGridViewTextBoxColumn;
    }
}