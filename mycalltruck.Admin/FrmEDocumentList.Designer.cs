namespace mycalltruck.Admin
{
    partial class FrmEDocumentList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.ColumnNo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestDateTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileDirectoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.image1NameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signLocationNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signLocationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pdfFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aipIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateDateTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errMsgDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docuTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.eDocumnetDataSet = new mycalltruck.Admin.DataSets.EDocumnetDataSet();
            this.docuTableTableAdapter = new mycalltruck.Admin.DataSets.EDocumnetDataSetTableAdapters.DocuTableTableAdapter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docuTableBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eDocumnetDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_Search);
            this.panel1.Controls.Add(this.cmb_Search);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Controls.Add(this.dtpStart);
            this.panel1.Controls.Add(this.btn_Inew);
            this.panel1.Controls.Add(this.btn_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 43);
            this.panel1.TabIndex = 49;
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Search.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Search.Location = new System.Drawing.Point(775, 7);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(87, 25);
            this.txt_Search.TabIndex = 58;
            this.txt_Search.TabStop = false;
            // 
            // cmb_Search
            // 
            this.cmb_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Search.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Items.AddRange(new object[] {
            "전체"});
            this.cmb_Search.Location = new System.Drawing.Point(691, 7);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(80, 25);
            this.cmb_Search.TabIndex = 57;
            this.cmb_Search.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(12, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 19);
            this.label11.TabIndex = 48;
            this.label11.Text = "전자인수증 전송현황";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.CustomFormat = "yyyy/MM/dd";
            this.dtpEnd.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(602, 7);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(85, 25);
            this.dtpEnd.TabIndex = 46;
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStart.CustomFormat = "yyyy/MM/dd";
            this.dtpStart.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(513, 7);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(85, 25);
            this.dtpStart.TabIndex = 45;
            // 
            // btn_Inew
            // 
            this.btn_Inew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Inew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Inew.FlatAppearance.BorderSize = 0;
            this.btn_Inew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Inew.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Inew.ForeColor = System.Drawing.Color.White;
            this.btn_Inew.Location = new System.Drawing.Point(930, 7);
            this.btn_Inew.Name = "btn_Inew";
            this.btn_Inew.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
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
            this.btn_Search.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(868, 7);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(56, 27);
            this.btn_Search.TabIndex = 43;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToDeleteRows = false;
            this.newDGV1.AllowUserToOrderColumns = true;
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle25.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle25.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.Color.White;
            this.newDGV1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle25;
            this.newDGV1.AutoGenerateColumns = false;
            this.newDGV1.BackgroundColor = System.Drawing.Color.White;
            this.newDGV1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.newDGV1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newDGV1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNo1,
            this.tradeIdDataGridViewTextBoxColumn,
            this.requestDateTimeDataGridViewTextBoxColumn,
            this.fileDirectoryDataGridViewTextBoxColumn,
            this.image1NameDataGridViewTextBoxColumn,
            this.signLocationNameDataGridViewTextBoxColumn,
            this.signLocationDataGridViewTextBoxColumn,
            this.pdfFileNameDataGridViewTextBoxColumn,
            this.aipIdDataGridViewTextBoxColumn,
            this.docIdDataGridViewTextBoxColumn,
            this.statusNameDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.updateDateTimeDataGridViewTextBoxColumn,
            this.errMsgDataGridViewTextBoxColumn});
            this.newDGV1.DataSource = this.docuTableBindingSource;
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle32.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle32.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle32.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle32.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle32;
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
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnNo1.DefaultCellStyle = dataGridViewCellStyle27;
            this.ColumnNo1.HeaderText = "번호";
            this.ColumnNo1.Name = "ColumnNo1";
            this.ColumnNo1.ReadOnly = true;
            this.ColumnNo1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnNo1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnNo1.Width = 60;
            // 
            // tradeIdDataGridViewTextBoxColumn
            // 
            this.tradeIdDataGridViewTextBoxColumn.DataPropertyName = "TradeId";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.tradeIdDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle28;
            this.tradeIdDataGridViewTextBoxColumn.HeaderText = "거래번호";
            this.tradeIdDataGridViewTextBoxColumn.Name = "tradeIdDataGridViewTextBoxColumn";
            // 
            // requestDateTimeDataGridViewTextBoxColumn
            // 
            this.requestDateTimeDataGridViewTextBoxColumn.DataPropertyName = "RequestDateTime";
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.requestDateTimeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle29;
            this.requestDateTimeDataGridViewTextBoxColumn.HeaderText = "요청일시";
            this.requestDateTimeDataGridViewTextBoxColumn.Name = "requestDateTimeDataGridViewTextBoxColumn";
            this.requestDateTimeDataGridViewTextBoxColumn.Width = 130;
            // 
            // fileDirectoryDataGridViewTextBoxColumn
            // 
            this.fileDirectoryDataGridViewTextBoxColumn.DataPropertyName = "FileDirectory";
            this.fileDirectoryDataGridViewTextBoxColumn.HeaderText = "폴더";
            this.fileDirectoryDataGridViewTextBoxColumn.Name = "fileDirectoryDataGridViewTextBoxColumn";
            this.fileDirectoryDataGridViewTextBoxColumn.Width = 240;
            // 
            // image1NameDataGridViewTextBoxColumn
            // 
            this.image1NameDataGridViewTextBoxColumn.DataPropertyName = "Image1Name";
            this.image1NameDataGridViewTextBoxColumn.HeaderText = "파일명1";
            this.image1NameDataGridViewTextBoxColumn.Name = "image1NameDataGridViewTextBoxColumn";
            // 
            // signLocationNameDataGridViewTextBoxColumn
            // 
            this.signLocationNameDataGridViewTextBoxColumn.DataPropertyName = "SignLocationName";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.signLocationNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle30;
            this.signLocationNameDataGridViewTextBoxColumn.HeaderText = "위치";
            this.signLocationNameDataGridViewTextBoxColumn.Name = "signLocationNameDataGridViewTextBoxColumn";
            this.signLocationNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // signLocationDataGridViewTextBoxColumn
            // 
            this.signLocationDataGridViewTextBoxColumn.DataPropertyName = "SignLocation";
            this.signLocationDataGridViewTextBoxColumn.HeaderText = "SignLocation";
            this.signLocationDataGridViewTextBoxColumn.Name = "signLocationDataGridViewTextBoxColumn";
            this.signLocationDataGridViewTextBoxColumn.Visible = false;
            // 
            // pdfFileNameDataGridViewTextBoxColumn
            // 
            this.pdfFileNameDataGridViewTextBoxColumn.DataPropertyName = "PdfFileName";
            this.pdfFileNameDataGridViewTextBoxColumn.HeaderText = "PDF명";
            this.pdfFileNameDataGridViewTextBoxColumn.Name = "pdfFileNameDataGridViewTextBoxColumn";
            this.pdfFileNameDataGridViewTextBoxColumn.Width = 130;
            // 
            // aipIdDataGridViewTextBoxColumn
            // 
            this.aipIdDataGridViewTextBoxColumn.DataPropertyName = "AipId";
            this.aipIdDataGridViewTextBoxColumn.HeaderText = "AipId";
            this.aipIdDataGridViewTextBoxColumn.Name = "aipIdDataGridViewTextBoxColumn";
            // 
            // docIdDataGridViewTextBoxColumn
            // 
            this.docIdDataGridViewTextBoxColumn.DataPropertyName = "DocId";
            this.docIdDataGridViewTextBoxColumn.HeaderText = "DocId";
            this.docIdDataGridViewTextBoxColumn.Name = "docIdDataGridViewTextBoxColumn";
            // 
            // statusNameDataGridViewTextBoxColumn
            // 
            this.statusNameDataGridViewTextBoxColumn.DataPropertyName = "StatusName";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.statusNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle31;
            this.statusNameDataGridViewTextBoxColumn.HeaderText = "상태";
            this.statusNameDataGridViewTextBoxColumn.Name = "statusNameDataGridViewTextBoxColumn";
            this.statusNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.Visible = false;
            // 
            // updateDateTimeDataGridViewTextBoxColumn
            // 
            this.updateDateTimeDataGridViewTextBoxColumn.DataPropertyName = "UpdateDateTime";
            this.updateDateTimeDataGridViewTextBoxColumn.HeaderText = "수정일시";
            this.updateDateTimeDataGridViewTextBoxColumn.Name = "updateDateTimeDataGridViewTextBoxColumn";
            this.updateDateTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.updateDateTimeDataGridViewTextBoxColumn.Width = 130;
            // 
            // errMsgDataGridViewTextBoxColumn
            // 
            this.errMsgDataGridViewTextBoxColumn.DataPropertyName = "ErrMsg";
            this.errMsgDataGridViewTextBoxColumn.HeaderText = "오류메세지";
            this.errMsgDataGridViewTextBoxColumn.Name = "errMsgDataGridViewTextBoxColumn";
            this.errMsgDataGridViewTextBoxColumn.Width = 300;
            // 
            // docuTableBindingSource
            // 
            this.docuTableBindingSource.DataMember = "DocuTable";
            this.docuTableBindingSource.DataSource = this.eDocumnetDataSet;
            // 
            // eDocumnetDataSet
            // 
            this.eDocumnetDataSet.DataSetName = "EDocumnetDataSet";
            this.eDocumnetDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // docuTableTableAdapter
            // 
            this.docuTableTableAdapter.ClearBeforeFill = true;
            // 
            // FrmEDocumentList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1012, 616);
            this.Controls.Add(this.newDGV1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmEDocumentList";
            this.Text = "전자인수증 전송현황";
            this.Load += new System.EventHandler(this.FRMMNUSELIST_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docuTableBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eDocumnetDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.TextBox txt_Search;
        public System.Windows.Forms.ComboBox cmb_Search;
        private NewDGV newDGV1;
        private DataSets.EDocumnetDataSet eDocumnetDataSet;
        private System.Windows.Forms.BindingSource docuTableBindingSource;
        private DataSets.EDocumnetDataSetTableAdapters.DocuTableTableAdapter docuTableTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestDateTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileDirectoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn image1NameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signLocationNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signLocationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pdfFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aipIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn docIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn updateDateTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errMsgDataGridViewTextBoxColumn;
    }
}