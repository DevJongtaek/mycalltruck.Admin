namespace mycalltruck.Admin
{
    partial class FrmMN0301_SMS_List
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMN0301_SMS_List));
            this.Root = new System.Windows.Forms.TableLayoutPanel();
            this.Toolbox = new System.Windows.Forms.TableLayoutPanel();
            this.Title = new System.Windows.Forms.Label();
            this.DoSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_Centrix = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.cmbCallGubun = new System.Windows.Forms.ComboBox();
            this.cmb_Gubun = new System.Windows.Forms.ComboBox();
            this.CTimeEFilter = new System.Windows.Forms.DateTimePicker();
            this.CTimeSFilter = new System.Windows.Forms.DateTimePicker();
            this.DataListContainer = new System.Windows.Forms.Panel();
            this.DataList = new mycalltruck.Admin.NewDGV();
            this.ColumnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDiv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCarNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnOriginalPhoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBtnSms = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnSmsResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Root.SuspendLayout();
            this.Toolbox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.DataListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataList)).BeginInit();
            this.SuspendLayout();
            // 
            // Root
            // 
            this.Root.ColumnCount = 1;
            this.Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Root.Controls.Add(this.Toolbox, 0, 0);
            this.Root.Controls.Add(this.DataListContainer, 0, 1);
            this.Root.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Root.Location = new System.Drawing.Point(0, 0);
            this.Root.Margin = new System.Windows.Forms.Padding(0);
            this.Root.Name = "Root";
            this.Root.RowCount = 2;
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Root.Size = new System.Drawing.Size(1319, 616);
            this.Root.TabIndex = 0;
            // 
            // Toolbox
            // 
            this.Toolbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.Toolbox.ColumnCount = 3;
            this.Toolbox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Toolbox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1100F));
            this.Toolbox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.Toolbox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Toolbox.Controls.Add(this.Title, 0, 0);
            this.Toolbox.Controls.Add(this.DoSearch, 2, 0);
            this.Toolbox.Controls.Add(this.panel1, 1, 0);
            this.Toolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Toolbox.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Toolbox.Location = new System.Drawing.Point(0, 0);
            this.Toolbox.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbox.Name = "Toolbox";
            this.Toolbox.RowCount = 1;
            this.Toolbox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Toolbox.Size = new System.Drawing.Size(1319, 43);
            this.Toolbox.TabIndex = 1;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Title.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.Title.Location = new System.Drawing.Point(5, 5);
            this.Title.Margin = new System.Windows.Forms.Padding(5);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(129, 33);
            this.Title.TabIndex = 3;
            this.Title.Text = "문자전송내역";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DoSearch
            // 
            this.DoSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DoSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.DoSearch.FlatAppearance.BorderSize = 0;
            this.DoSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DoSearch.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.DoSearch.ForeColor = System.Drawing.Color.White;
            this.DoSearch.Location = new System.Drawing.Point(1244, 8);
            this.DoSearch.Margin = new System.Windows.Forms.Padding(5);
            this.DoSearch.Name = "DoSearch";
            this.DoSearch.Size = new System.Drawing.Size(70, 27);
            this.DoSearch.TabIndex = 2;
            this.DoSearch.Text = "검색";
            this.DoSearch.UseVisualStyleBackColor = false;
            this.DoSearch.Click += new System.EventHandler(this.DoSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btn_Centrix);
            this.panel1.Controls.Add(this.txt_Search);
            this.panel1.Controls.Add(this.cmbSearch);
            this.panel1.Controls.Add(this.cmbCallGubun);
            this.panel1.Controls.Add(this.cmb_Gubun);
            this.panel1.Controls.Add(this.CTimeEFilter);
            this.panel1.Controls.Add(this.CTimeSFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(139, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1100, 43);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(489, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(22, 22);
            this.panel2.TabIndex = 55;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::mycalltruck.Admin.Properties.Resources.icon_question;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 22);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btn_Centrix
            // 
            this.btn_Centrix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Centrix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Centrix.FlatAppearance.BorderSize = 0;
            this.btn_Centrix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Centrix.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Centrix.ForeColor = System.Drawing.Color.White;
            this.btn_Centrix.Location = new System.Drawing.Point(319, 8);
            this.btn_Centrix.Name = "btn_Centrix";
            this.btn_Centrix.Size = new System.Drawing.Size(164, 27);
            this.btn_Centrix.TabIndex = 54;
            this.btn_Centrix.TabStop = false;
            this.btn_Centrix.Text = "전화번호 설정 (LGU+)";
            this.btn_Centrix.UseVisualStyleBackColor = false;
            this.btn_Centrix.Click += new System.EventHandler(this.btn_Centrix_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Search.Location = new System.Drawing.Point(998, 10);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(99, 23);
            this.txt_Search.TabIndex = 44;
            this.txt_Search.TabStop = false;
            this.txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyUp);
            // 
            // cmbSearch
            // 
            this.cmbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Items.AddRange(new object[] {
            "전체",
            "상호/이름",
            "차량번호",
            "핸드폰번호"});
            this.cmbSearch.Location = new System.Drawing.Point(885, 10);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(107, 23);
            this.cmbSearch.TabIndex = 43;
            this.cmbSearch.TabStop = false;
            // 
            // cmbCallGubun
            // 
            this.cmbCallGubun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCallGubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCallGubun.FormattingEnabled = true;
            this.cmbCallGubun.Items.AddRange(new object[] {
            "전체",
            "성공",
            "실패"});
            this.cmbCallGubun.Location = new System.Drawing.Point(788, 10);
            this.cmbCallGubun.Name = "cmbCallGubun";
            this.cmbCallGubun.Size = new System.Drawing.Size(91, 23);
            this.cmbCallGubun.TabIndex = 42;
            this.cmbCallGubun.TabStop = false;
            // 
            // cmb_Gubun
            // 
            this.cmb_Gubun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Gubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Gubun.FormattingEnabled = true;
            this.cmb_Gubun.Items.AddRange(new object[] {
            "전체",
            "거래처",
            "차주"});
            this.cmb_Gubun.Location = new System.Drawing.Point(707, 10);
            this.cmb_Gubun.Name = "cmb_Gubun";
            this.cmb_Gubun.Size = new System.Drawing.Size(78, 23);
            this.cmb_Gubun.TabIndex = 41;
            this.cmb_Gubun.TabStop = false;
            // 
            // CTimeEFilter
            // 
            this.CTimeEFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTimeEFilter.CustomFormat = "yyyy-MM-dd";
            this.CTimeEFilter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CTimeEFilter.Location = new System.Drawing.Point(611, 10);
            this.CTimeEFilter.Margin = new System.Windows.Forms.Padding(5);
            this.CTimeEFilter.Name = "CTimeEFilter";
            this.CTimeEFilter.Size = new System.Drawing.Size(89, 23);
            this.CTimeEFilter.TabIndex = 2;
            // 
            // CTimeSFilter
            // 
            this.CTimeSFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTimeSFilter.CustomFormat = "yyyy-MM-dd";
            this.CTimeSFilter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CTimeSFilter.Location = new System.Drawing.Point(519, 10);
            this.CTimeSFilter.Margin = new System.Windows.Forms.Padding(5);
            this.CTimeSFilter.Name = "CTimeSFilter";
            this.CTimeSFilter.Size = new System.Drawing.Size(89, 23);
            this.CTimeSFilter.TabIndex = 1;
            // 
            // DataListContainer
            // 
            this.DataListContainer.Controls.Add(this.DataList);
            this.DataListContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataListContainer.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.DataListContainer.Location = new System.Drawing.Point(0, 43);
            this.DataListContainer.Margin = new System.Windows.Forms.Padding(0);
            this.DataListContainer.Name = "DataListContainer";
            this.DataListContainer.Size = new System.Drawing.Size(1319, 573);
            this.DataListContainer.TabIndex = 2;
            // 
            // DataList
            // 
            this.DataList.AllowUserToAddRows = false;
            this.DataList.AllowUserToDeleteRows = false;
            this.DataList.AllowUserToResizeColumns = false;
            this.DataList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataList.BackgroundColor = System.Drawing.Color.White;
            this.DataList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DataList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNo,
            this.ColumnCTime,
            this.ColumnDiv,
            this.ColumnTarget,
            this.ColumnCarNo,
            this.ColumnOriginalPhoneNo,
            this.ColumnMessage,
            this.ColumnBtnSms,
            this.ColumnSmsResult});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataList.DefaultCellStyle = dataGridViewCellStyle9;
            this.DataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataList.GridColor = System.Drawing.Color.White;
            this.DataList.Location = new System.Drawing.Point(0, 0);
            this.DataList.Margin = new System.Windows.Forms.Padding(5);
            this.DataList.MultiSelect = false;
            this.DataList.Name = "DataList";
            this.DataList.RowHeadersVisible = false;
            this.DataList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.DataList.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.DataList.RowTemplate.Height = 23;
            this.DataList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataList.Size = new System.Drawing.Size(1319, 573);
            this.DataList.TabIndex = 3;
            this.DataList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataList_CellContentClick);
            this.DataList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataList_CellFormatting);
            this.DataList.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataList_CellPainting);
            // 
            // ColumnNo
            // 
            this.ColumnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnNo.Frozen = true;
            this.ColumnNo.HeaderText = "번호";
            this.ColumnNo.Name = "ColumnNo";
            this.ColumnNo.ReadOnly = true;
            this.ColumnNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnNo.Width = 60;
            // 
            // ColumnCTime
            // 
            this.ColumnCTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnCTime.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnCTime.Frozen = true;
            this.ColumnCTime.HeaderText = "전송일시";
            this.ColumnCTime.Name = "ColumnCTime";
            this.ColumnCTime.ReadOnly = true;
            this.ColumnCTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnCTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnCTime.Width = 140;
            // 
            // ColumnDiv
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnDiv.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnDiv.HeaderText = "구분";
            this.ColumnDiv.Name = "ColumnDiv";
            this.ColumnDiv.ReadOnly = true;
            this.ColumnDiv.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDiv.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDiv.Width = 60;
            // 
            // ColumnTarget
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnTarget.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnTarget.HeaderText = "상호/이름";
            this.ColumnTarget.Name = "ColumnTarget";
            this.ColumnTarget.ReadOnly = true;
            this.ColumnTarget.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnTarget.Width = 120;
            // 
            // ColumnCarNo
            // 
            this.ColumnCarNo.HeaderText = "차량번호";
            this.ColumnCarNo.Name = "ColumnCarNo";
            // 
            // ColumnOriginalPhoneNo
            // 
            this.ColumnOriginalPhoneNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnOriginalPhoneNo.DefaultCellStyle = dataGridViewCellStyle7;
            this.ColumnOriginalPhoneNo.HeaderText = "핸드폰번호";
            this.ColumnOriginalPhoneNo.Name = "ColumnOriginalPhoneNo";
            this.ColumnOriginalPhoneNo.ReadOnly = true;
            this.ColumnOriginalPhoneNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnOriginalPhoneNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnOriginalPhoneNo.Width = 120;
            // 
            // ColumnMessage
            // 
            this.ColumnMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnMessage.HeaderText = "메세지내용";
            this.ColumnMessage.Name = "ColumnMessage";
            this.ColumnMessage.ReadOnly = true;
            // 
            // ColumnBtnSms
            // 
            this.ColumnBtnSms.HeaderText = "문자";
            this.ColumnBtnSms.Name = "ColumnBtnSms";
            this.ColumnBtnSms.Text = "보기";
            this.ColumnBtnSms.UseColumnTextForButtonValue = true;
            this.ColumnBtnSms.Width = 50;
            // 
            // ColumnSmsResult
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnSmsResult.DefaultCellStyle = dataGridViewCellStyle8;
            this.ColumnSmsResult.HeaderText = "결과";
            this.ColumnSmsResult.Name = "ColumnSmsResult";
            this.ColumnSmsResult.Width = 80;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "통화";
            this.dataGridViewImageColumn1.Image = global::mycalltruck.Admin.Properties.Resources.CallRe;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.Width = 70;
            // 
            // dataGridViewImageColumn2
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle11.NullValue")));
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewImageColumn2.HeaderText = "전화";
            this.dataGridViewImageColumn2.Image = global::mycalltruck.Admin.Properties.Resources.Call;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.Width = 70;
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.HeaderText = "SMS";
            this.dataGridViewImageColumn3.Image = global::mycalltruck.Admin.Properties.Resources.CallSms;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn3.Width = 70;
            // 
            // dataGridViewImageColumn4
            // 
            this.dataGridViewImageColumn4.HeaderText = "메모";
            this.dataGridViewImageColumn4.Image = global::mycalltruck.Admin.Properties.Resources.CallMemo;
            this.dataGridViewImageColumn4.Name = "dataGridViewImageColumn4";
            this.dataGridViewImageColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn4.Width = 70;
            // 
            // FrmMN0301_SMS_List
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1319, 616);
            this.Controls.Add(this.Root);
            this.Name = "FrmMN0301_SMS_List";
            this.Text = "문자전송내역";
            this.Root.ResumeLayout(false);
            this.Toolbox.ResumeLayout(false);
            this.Toolbox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.DataListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Root;
        private System.Windows.Forms.TableLayoutPanel Toolbox;
        private System.Windows.Forms.DateTimePicker CTimeSFilter;
        private System.Windows.Forms.Panel DataListContainer;
        private NewDGV DataList;
        private System.Windows.Forms.Button DoSearch;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker CTimeEFilter;
        public System.Windows.Forms.ComboBox cmb_Gubun;
        public System.Windows.Forms.ComboBox cmbCallGubun;
        public System.Windows.Forms.ComboBox cmbSearch;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDiv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCarNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOriginalPhoneNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMessage;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBtnSms;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSmsResult;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_Centrix;
    }
}