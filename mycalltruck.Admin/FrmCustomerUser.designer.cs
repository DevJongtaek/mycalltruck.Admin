namespace mycalltruck.Admin
{
    partial class FrmCustomerUser
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnButtons = new System.Windows.Forms.Panel();
            this.bPanel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_New = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCurrentDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.ClientUserId = new System.Windows.Forms.NumericUpDown();
            this.pnFill = new System.Windows.Forms.Panel();
            this.pnGrid = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new mycalltruck.Admin.NewDGV();
            this.clientUserIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addTeamDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addPhoneNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobilePhoneNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rankDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerAddPhoneBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.customerStartManageDataSet = new mycalltruck.Admin.DataSets.CustomerStartManageDataSet();
            this.pnSearch = new System.Windows.Forms.Panel();
            this.txt_ClientSearch = new System.Windows.Forms.TextBox();
            this.cmb_ClientSerach = new System.Windows.Forms.ComboBox();
            this.btn_Inew = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.cmb_Search = new System.Windows.Forms.ComboBox();
            this.pnDetail = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CreateTime = new System.Windows.Forms.TextBox();
            this.txt_Email = new System.Windows.Forms.TextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.txt_LoginId = new System.Windows.Forms.TextBox();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.txt_Part = new System.Windows.Forms.TextBox();
            this.txt_Rank = new System.Windows.Forms.TextBox();
            this.txt_PhoneNo = new System.Windows.Forms.TextBox();
            this.txt_MobileNo = new System.Windows.Forms.TextBox();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableAdapterManager = new mycalltruck.Admin.CMDataSetTableAdapters.TableAdapterManager();
            this.customerAddPhoneTableAdapter = new mycalltruck.Admin.DataSets.CustomerStartManageDataSetTableAdapters.CustomerAddPhoneTableAdapter();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnButtons.SuspendLayout();
            this.bPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientUserId)).BeginInit();
            this.pnFill.SuspendLayout();
            this.pnGrid.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerAddPhoneBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerStartManageDataSet)).BeginInit();
            this.pnSearch.SuspendLayout();
            this.pnDetail.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnButtons, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnFill, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1319, 616);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // pnButtons
            // 
            this.pnButtons.Controls.Add(this.bPanel1);
            this.pnButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnButtons.Location = new System.Drawing.Point(0, 0);
            this.pnButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new System.Drawing.Size(1319, 43);
            this.pnButtons.TabIndex = 0;
            // 
            // bPanel1
            // 
            this.bPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bPanel1.Controls.Add(this.label11);
            this.bPanel1.Controls.Add(this.btn_New);
            this.bPanel1.Controls.Add(this.btnClose);
            this.bPanel1.Controls.Add(this.btnCurrentDelete);
            this.bPanel1.Controls.Add(this.btnUpdate);
            this.bPanel1.Controls.Add(this.ClientUserId);
            this.bPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bPanel1.Location = new System.Drawing.Point(0, 0);
            this.bPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.bPanel1.Name = "bPanel1";
            this.bPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.bPanel1.Size = new System.Drawing.Size(1319, 43);
            this.bPanel1.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(4, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 19);
            this.label11.TabIndex = 17;
            this.label11.Text = "거래처아이디관리";
            // 
            // btn_New
            // 
            this.btn_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_New.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_New.FlatAppearance.BorderSize = 0;
            this.btn_New.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_New.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_New.ForeColor = System.Drawing.Color.White;
            this.btn_New.Location = new System.Drawing.Point(990, 7);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(77, 27);
            this.btn_New.TabIndex = 14;
            this.btn_New.Tag = "Write";
            this.btn_New.Text = "추 가";
            this.btn_New.UseVisualStyleBackColor = false;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1238, 7);
            this.btnClose.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 27);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCurrentDelete
            // 
            this.btnCurrentDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCurrentDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnCurrentDelete.FlatAppearance.BorderSize = 0;
            this.btnCurrentDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCurrentDelete.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCurrentDelete.ForeColor = System.Drawing.Color.White;
            this.btnCurrentDelete.Location = new System.Drawing.Point(1156, 7);
            this.btnCurrentDelete.Name = "btnCurrentDelete";
            this.btnCurrentDelete.Size = new System.Drawing.Size(77, 27);
            this.btnCurrentDelete.TabIndex = 12;
            this.btnCurrentDelete.Tag = "Write";
            this.btnCurrentDelete.Text = "삭 제";
            this.btnCurrentDelete.UseVisualStyleBackColor = false;
            this.btnCurrentDelete.Click += new System.EventHandler(this.btnCurrentDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(1073, 7);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(77, 27);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Tag = "Write";
            this.btnUpdate.Text = "수 정";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // ClientUserId
            // 
            this.ClientUserId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientUserId.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientUserId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ClientUserId.ForeColor = System.Drawing.Color.Gainsboro;
            this.ClientUserId.Location = new System.Drawing.Point(1003, 7);
            this.ClientUserId.Margin = new System.Windows.Forms.Padding(0);
            this.ClientUserId.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.ClientUserId.Name = "ClientUserId";
            this.ClientUserId.Size = new System.Drawing.Size(1, 23);
            this.ClientUserId.TabIndex = 37;
            // 
            // pnFill
            // 
            this.pnFill.Controls.Add(this.pnGrid);
            this.pnFill.Controls.Add(this.pnDetail);
            this.pnFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnFill.Location = new System.Drawing.Point(0, 43);
            this.pnFill.Margin = new System.Windows.Forms.Padding(0);
            this.pnFill.Name = "pnFill";
            this.pnFill.Size = new System.Drawing.Size(1319, 573);
            this.pnFill.TabIndex = 1;
            // 
            // pnGrid
            // 
            this.pnGrid.Controls.Add(this.panel8);
            this.pnGrid.Controls.Add(this.pnSearch);
            this.pnGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGrid.Location = new System.Drawing.Point(0, 219);
            this.pnGrid.Margin = new System.Windows.Forms.Padding(0);
            this.pnGrid.Name = "pnGrid";
            this.pnGrid.Size = new System.Drawing.Size(1319, 354);
            this.pnGrid.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.dataGridView1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 45);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1319, 309);
            this.panel8.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clientUserIdDataGridViewTextBoxColumn,
            this.idxDataGridViewTextBoxColumn,
            this.addTeamDataGridViewTextBoxColumn,
            this.addNameDataGridViewTextBoxColumn,
            this.loginIdDataGridViewTextBoxColumn,
            this.passwordDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.addPhoneNoDataGridViewTextBoxColumn,
            this.mobilePhoneNoDataGridViewTextBoxColumn,
            this.clientIdDataGridViewTextBoxColumn,
            this.customerIdDataGridViewTextBoxColumn,
            this.createTimeDataGridViewTextBoxColumn,
            this.remarkDataGridViewTextBoxColumn,
            this.rankDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.customerAddPhoneBindingSource;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1319, 309);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // clientUserIdDataGridViewTextBoxColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "N0";
            this.clientUserIdDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.clientUserIdDataGridViewTextBoxColumn.HeaderText = "번호";
            this.clientUserIdDataGridViewTextBoxColumn.Name = "clientUserIdDataGridViewTextBoxColumn";
            this.clientUserIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.clientUserIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clientUserIdDataGridViewTextBoxColumn.Width = 60;
            // 
            // idxDataGridViewTextBoxColumn
            // 
            this.idxDataGridViewTextBoxColumn.DataPropertyName = "idx";
            this.idxDataGridViewTextBoxColumn.HeaderText = "idx";
            this.idxDataGridViewTextBoxColumn.Name = "idxDataGridViewTextBoxColumn";
            this.idxDataGridViewTextBoxColumn.ReadOnly = true;
            this.idxDataGridViewTextBoxColumn.Visible = false;
            // 
            // addTeamDataGridViewTextBoxColumn
            // 
            this.addTeamDataGridViewTextBoxColumn.DataPropertyName = "AddTeam";
            this.addTeamDataGridViewTextBoxColumn.HeaderText = "부서";
            this.addTeamDataGridViewTextBoxColumn.Name = "addTeamDataGridViewTextBoxColumn";
            this.addTeamDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // addNameDataGridViewTextBoxColumn
            // 
            this.addNameDataGridViewTextBoxColumn.DataPropertyName = "AddName";
            this.addNameDataGridViewTextBoxColumn.HeaderText = "사용자명";
            this.addNameDataGridViewTextBoxColumn.Name = "addNameDataGridViewTextBoxColumn";
            this.addNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // loginIdDataGridViewTextBoxColumn
            // 
            this.loginIdDataGridViewTextBoxColumn.DataPropertyName = "LoginId";
            this.loginIdDataGridViewTextBoxColumn.HeaderText = "아이디";
            this.loginIdDataGridViewTextBoxColumn.Name = "loginIdDataGridViewTextBoxColumn";
            this.loginIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // passwordDataGridViewTextBoxColumn
            // 
            this.passwordDataGridViewTextBoxColumn.DataPropertyName = "Password";
            this.passwordDataGridViewTextBoxColumn.HeaderText = "비밀번호";
            this.passwordDataGridViewTextBoxColumn.Name = "passwordDataGridViewTextBoxColumn";
            this.passwordDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "이메일";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            this.emailDataGridViewTextBoxColumn.ReadOnly = true;
            this.emailDataGridViewTextBoxColumn.Width = 140;
            // 
            // addPhoneNoDataGridViewTextBoxColumn
            // 
            this.addPhoneNoDataGridViewTextBoxColumn.DataPropertyName = "AddPhoneNo";
            this.addPhoneNoDataGridViewTextBoxColumn.HeaderText = "전화번호";
            this.addPhoneNoDataGridViewTextBoxColumn.Name = "addPhoneNoDataGridViewTextBoxColumn";
            this.addPhoneNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.addPhoneNoDataGridViewTextBoxColumn.Width = 130;
            // 
            // mobilePhoneNoDataGridViewTextBoxColumn
            // 
            this.mobilePhoneNoDataGridViewTextBoxColumn.DataPropertyName = "MobilePhoneNo";
            this.mobilePhoneNoDataGridViewTextBoxColumn.HeaderText = "핸드폰번호";
            this.mobilePhoneNoDataGridViewTextBoxColumn.Name = "mobilePhoneNoDataGridViewTextBoxColumn";
            this.mobilePhoneNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.mobilePhoneNoDataGridViewTextBoxColumn.Width = 130;
            // 
            // clientIdDataGridViewTextBoxColumn
            // 
            this.clientIdDataGridViewTextBoxColumn.DataPropertyName = "ClientId";
            this.clientIdDataGridViewTextBoxColumn.HeaderText = "ClientId";
            this.clientIdDataGridViewTextBoxColumn.Name = "clientIdDataGridViewTextBoxColumn";
            this.clientIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.clientIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerIdDataGridViewTextBoxColumn
            // 
            this.customerIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.HeaderText = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.Name = "customerIdDataGridViewTextBoxColumn";
            this.customerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // createTimeDataGridViewTextBoxColumn
            // 
            this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "CreateTime";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "d";
            this.createTimeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.createTimeDataGridViewTextBoxColumn.HeaderText = "등록일";
            this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
            this.createTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // remarkDataGridViewTextBoxColumn
            // 
            this.remarkDataGridViewTextBoxColumn.DataPropertyName = "Remark";
            this.remarkDataGridViewTextBoxColumn.HeaderText = "Remark";
            this.remarkDataGridViewTextBoxColumn.Name = "remarkDataGridViewTextBoxColumn";
            this.remarkDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarkDataGridViewTextBoxColumn.Visible = false;
            // 
            // rankDataGridViewTextBoxColumn
            // 
            this.rankDataGridViewTextBoxColumn.DataPropertyName = "Rank";
            this.rankDataGridViewTextBoxColumn.HeaderText = "Rank";
            this.rankDataGridViewTextBoxColumn.Name = "rankDataGridViewTextBoxColumn";
            this.rankDataGridViewTextBoxColumn.ReadOnly = true;
            this.rankDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerAddPhoneBindingSource
            // 
            this.customerAddPhoneBindingSource.DataMember = "CustomerAddPhone";
            this.customerAddPhoneBindingSource.DataSource = this.customerStartManageDataSet;
            this.customerAddPhoneBindingSource.CurrentChanged += new System.EventHandler(this.customerAddPhoneBindingSource_CurrentChanged);
            // 
            // customerStartManageDataSet
            // 
            this.customerStartManageDataSet.DataSetName = "CustomerStartManageDataSet";
            this.customerStartManageDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnSearch
            // 
            this.pnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.pnSearch.Controls.Add(this.txt_ClientSearch);
            this.pnSearch.Controls.Add(this.cmb_ClientSerach);
            this.pnSearch.Controls.Add(this.btn_Inew);
            this.pnSearch.Controls.Add(this.btn_Search);
            this.pnSearch.Controls.Add(this.txt_Search);
            this.pnSearch.Controls.Add(this.cmb_Search);
            this.pnSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.pnSearch.Location = new System.Drawing.Point(0, 0);
            this.pnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.pnSearch.Name = "pnSearch";
            this.pnSearch.Size = new System.Drawing.Size(1319, 45);
            this.pnSearch.TabIndex = 0;
            // 
            // txt_ClientSearch
            // 
            this.txt_ClientSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_ClientSearch.Location = new System.Drawing.Point(832, 10);
            this.txt_ClientSearch.Name = "txt_ClientSearch";
            this.txt_ClientSearch.Size = new System.Drawing.Size(87, 25);
            this.txt_ClientSearch.TabIndex = 41;
            this.txt_ClientSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_ClientSearch_KeyUp);
            // 
            // cmb_ClientSerach
            // 
            this.cmb_ClientSerach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_ClientSerach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_ClientSerach.FormattingEnabled = true;
            this.cmb_ClientSerach.Items.AddRange(new object[] {
            "전체",
            "운송사코드",
            "운송사명"});
            this.cmb_ClientSerach.Location = new System.Drawing.Point(706, 10);
            this.cmb_ClientSerach.Name = "cmb_ClientSerach";
            this.cmb_ClientSerach.Size = new System.Drawing.Size(121, 25);
            this.cmb_ClientSerach.TabIndex = 40;
            // 
            // btn_Inew
            // 
            this.btn_Inew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Inew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Inew.FlatAppearance.BorderSize = 0;
            this.btn_Inew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Inew.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Inew.ForeColor = System.Drawing.Color.White;
            this.btn_Inew.Location = new System.Drawing.Point(1234, 9);
            this.btn_Inew.Name = "btn_Inew";
            this.btn_Inew.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Inew.Size = new System.Drawing.Size(77, 27);
            this.btn_Inew.TabIndex = 39;
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
            this.btn_Search.Location = new System.Drawing.Point(1152, 9);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btn_Search.Size = new System.Drawing.Size(77, 27);
            this.btn_Search.TabIndex = 38;
            this.btn_Search.Text = "조 회";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Search.Location = new System.Drawing.Point(1050, 10);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(97, 25);
            this.txt_Search.TabIndex = 34;
            this.txt_Search.TextChanged += new System.EventHandler(this.txt_Search_TextChanged);
            this.txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyUp);
            // 
            // cmb_Search
            // 
            this.cmb_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Search.FormattingEnabled = true;
            this.cmb_Search.Items.AddRange(new object[] {
            "전체",
            "사용자명",
            "아이디"});
            this.cmb_Search.Location = new System.Drawing.Point(924, 10);
            this.cmb_Search.Name = "cmb_Search";
            this.cmb_Search.Size = new System.Drawing.Size(121, 25);
            this.cmb_Search.TabIndex = 33;
            this.cmb_Search.SelectedIndexChanged += new System.EventHandler(this.cmb_Search_SelectedIndexChanged);
            // 
            // pnDetail
            // 
            this.pnDetail.BackColor = System.Drawing.Color.Silver;
            this.pnDetail.Controls.Add(this.groupBox2);
            this.pnDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDetail.Location = new System.Drawing.Point(0, 0);
            this.pnDetail.Margin = new System.Windows.Forms.Padding(0);
            this.pnDetail.Name = "pnDetail";
            this.pnDetail.Size = new System.Drawing.Size(1319, 219);
            this.pnDetail.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(1319, 219);
            this.groupBox2.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_CreateTime, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_Email, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Password, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_LoginId, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Name, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Part, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Rank, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_PhoneNo, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txt_MobileNo, 3, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1317, 217);
            this.tableLayoutPanel2.TabIndex = 2;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label18.Location = new System.Drawing.Point(5, 5);
            this.label18.Margin = new System.Windows.Forms.Padding(4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 25);
            this.label18.TabIndex = 34;
            this.label18.Text = "부서명";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label16.Location = new System.Drawing.Point(5, 71);
            this.label16.Margin = new System.Windows.Forms.Padding(4);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 25);
            this.label16.TabIndex = 15;
            this.label16.Text = "전화번호";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label13.Location = new System.Drawing.Point(5, 104);
            this.label13.Margin = new System.Windows.Forms.Padding(4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 25);
            this.label13.TabIndex = 12;
            this.label13.Text = "등록일";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label12.Location = new System.Drawing.Point(5, 38);
            this.label12.Margin = new System.Windows.Forms.Padding(4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 25);
            this.label12.TabIndex = 11;
            this.label12.Text = "아이디";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label8.Location = new System.Drawing.Point(501, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "이메일";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label7.Location = new System.Drawing.Point(501, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "직위";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label3.Location = new System.Drawing.Point(253, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "핸드폰번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label2.Location = new System.Drawing.Point(253, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "비밀번호";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label1.Location = new System.Drawing.Point(253, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "담당자명";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_CreateTime
            // 
            this.txt_CreateTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CreateTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "CreateTime", true));
            this.txt_CreateTime.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_CreateTime.Location = new System.Drawing.Point(92, 103);
            this.txt_CreateTime.Name = "txt_CreateTime";
            this.txt_CreateTime.ReadOnly = true;
            this.txt_CreateTime.Size = new System.Drawing.Size(154, 25);
            this.txt_CreateTime.TabIndex = 9;
            this.txt_CreateTime.TabStop = false;
            // 
            // txt_Email
            // 
            this.txt_Email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Email.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "Email", true));
            this.txt_Email.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Email.Location = new System.Drawing.Point(588, 37);
            this.txt_Email.MaxLength = 50;
            this.txt_Email.Name = "txt_Email";
            this.txt_Email.Size = new System.Drawing.Size(154, 25);
            this.txt_Email.TabIndex = 5;
            // 
            // txt_Password
            // 
            this.txt_Password.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "Password", true));
            this.txt_Password.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Password.Location = new System.Drawing.Point(340, 37);
            this.txt_Password.MaxLength = 15;
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Size = new System.Drawing.Size(154, 25);
            this.txt_Password.TabIndex = 4;
            // 
            // txt_LoginId
            // 
            this.txt_LoginId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_LoginId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "LoginId", true));
            this.txt_LoginId.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_LoginId.Location = new System.Drawing.Point(92, 37);
            this.txt_LoginId.Name = "txt_LoginId";
            this.txt_LoginId.ReadOnly = true;
            this.txt_LoginId.Size = new System.Drawing.Size(154, 25);
            this.txt_LoginId.TabIndex = 3;
            this.txt_LoginId.TabStop = false;
            // 
            // txt_Name
            // 
            this.txt_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Name.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "AddName", true));
            this.txt_Name.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Name.Location = new System.Drawing.Point(340, 4);
            this.txt_Name.MaxLength = 15;
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(154, 25);
            this.txt_Name.TabIndex = 2;
            // 
            // txt_Part
            // 
            this.txt_Part.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Part.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "AddTeam", true));
            this.txt_Part.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Part.Location = new System.Drawing.Point(92, 4);
            this.txt_Part.MaxLength = 15;
            this.txt_Part.Name = "txt_Part";
            this.txt_Part.Size = new System.Drawing.Size(154, 25);
            this.txt_Part.TabIndex = 1;
            // 
            // txt_Rank
            // 
            this.txt_Rank.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Rank.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "Rank", true));
            this.txt_Rank.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Rank.Location = new System.Drawing.Point(588, 4);
            this.txt_Rank.MaxLength = 15;
            this.txt_Rank.Name = "txt_Rank";
            this.txt_Rank.Size = new System.Drawing.Size(154, 25);
            this.txt_Rank.TabIndex = 3;
            // 
            // txt_PhoneNo
            // 
            this.txt_PhoneNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "AddPhoneNo", true));
            this.txt_PhoneNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_PhoneNo.Location = new System.Drawing.Point(92, 70);
            this.txt_PhoneNo.Name = "txt_PhoneNo";
            this.txt_PhoneNo.Size = new System.Drawing.Size(154, 25);
            this.txt_PhoneNo.TabIndex = 6;
            this.txt_PhoneNo.Enter += new System.EventHandler(this.txt_PhoneNo_Enter);
            this.txt_PhoneNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_PhoneNo_KeyPress);
            this.txt_PhoneNo.Leave += new System.EventHandler(this.txt_PhoneNo_Leave);
            // 
            // txt_MobileNo
            // 
            this.txt_MobileNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAddPhoneBindingSource, "MobilePhoneNo", true));
            this.txt_MobileNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_MobileNo.Location = new System.Drawing.Point(340, 70);
            this.txt_MobileNo.Name = "txt_MobileNo";
            this.txt_MobileNo.Size = new System.Drawing.Size(154, 25);
            this.txt_MobileNo.TabIndex = 7;
            this.txt_MobileNo.Enter += new System.EventHandler(this.txt_MobileNo_Enter);
            this.txt_MobileNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_MobileNo_KeyPress);
            this.txt_MobileNo.Leave += new System.EventHandler(this.txt_MobileNo_Leave);
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AcceptInfoesTableAdapter = null;
            this.tableAdapterManager.AccLogsTableAdapter = null;
            this.tableAdapterManager.AccountOptionsTableAdapter = null;
            this.tableAdapterManager.AddressReferencesTableAdapter = null;
            this.tableAdapterManager.AndroidTimeTableAdapter = null;
            this.tableAdapterManager.AndroidUpdateInfoesTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CardPayASTableAdapter = null;
            this.tableAdapterManager.ChargeAccountsTableAdapter = null;
            this.tableAdapterManager.ChargeManageTableAdapter = null;
            this.tableAdapterManager.Charges_ClientsTableAdapter = null;
            this.tableAdapterManager.Charges1TableAdapter = null;
            this.tableAdapterManager.ChargesTableAdapter = null;
            this.tableAdapterManager.ClientAccsTableAdapter = null;
            this.tableAdapterManager.ClientAddressTableAdapter = null;
            this.tableAdapterManager.ClientsTableAdapter = null;
            this.tableAdapterManager.ClientUsersTableAdapter = null;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.CustomerAccsTableAdapter = null;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.DealersTableAdapter = null;
            this.tableAdapterManager.DriverAddTableAdapter = null;
            this.tableAdapterManager.DriverGroupsTableAdapter = null;
            this.tableAdapterManager.DriverPapersTableAdapter = null;
            this.tableAdapterManager.DRIVERPAYTableAdapter = null;
            this.tableAdapterManager.Drivers_CarTableAdapter = null;
            this.tableAdapterManager.Drivers_ExcelTableAdapter = null;
            this.tableAdapterManager.Drivers1TableAdapter = null;
            this.tableAdapterManager.DriversTableAdapter = null;
            this.tableAdapterManager.em_mmt_tranTableAdapter = null;
            this.tableAdapterManager.em_tranTableAdapter = null;
            this.tableAdapterManager.FPIS_CARTableAdapter = null;
            this.tableAdapterManager.FPIS_CONTTableAdapter = null;
            this.tableAdapterManager.FPIS_FILE_CARTableAdapter = null;
            this.tableAdapterManager.FPIS_FILETableAdapter = null;
            this.tableAdapterManager.FPIS_TRUTableAdapter = null;
            this.tableAdapterManager.FPISOptionsTableAdapter = null;
            this.tableAdapterManager.LoanInfoesTableAdapter = null;
            this.tableAdapterManager.MSG_DATATableAdapter = null;
            this.tableAdapterManager.NewZipcodeTableAdapter = null;
            this.tableAdapterManager.NOTICEDRIVERTableAdapter = null;
            this.tableAdapterManager.NotificationsTableAdapter = null;
            this.tableAdapterManager.OrdersTableAdapter = null;
            this.tableAdapterManager.SalesManageTableAdapter = null;
            this.tableAdapterManager.TB_FPIS_MODULETableAdapter = null;
            this.tableAdapterManager.TR_DUTYDETAILTableAdapter = null;
            this.tableAdapterManager.TR_DUTYMASTERTableAdapter = null;
            this.tableAdapterManager.Trades1TableAdapter = null;
            this.tableAdapterManager.UpdateOrder = mycalltruck.Admin.CMDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // customerAddPhoneTableAdapter
            // 
            this.customerAddPhoneTableAdapter.ClearBeforeFill = true;
            // 
            // FrmCustomerUser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1319, 616);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmCustomerUser";
            this.Text = "아이디관리";
            this.Load += new System.EventHandler(this.FrmClientUser_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnButtons.ResumeLayout(false);
            this.bPanel1.ResumeLayout(false);
            this.bPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientUserId)).EndInit();
            this.pnFill.ResumeLayout(false);
            this.pnGrid.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerAddPhoneBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerStartManageDataSet)).EndInit();
            this.pnSearch.ResumeLayout(false);
            this.pnSearch.PerformLayout();
            this.pnDetail.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnButtons;
        private System.Windows.Forms.Panel bPanel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_New;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCurrentDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Panel pnFill;
        private System.Windows.Forms.Panel pnGrid;
        private System.Windows.Forms.Panel panel8;
        private NewDGV dataGridView1;
        private System.Windows.Forms.Panel pnSearch;
        private System.Windows.Forms.Button btn_Inew;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.TextBox txt_Search;
        public System.Windows.Forms.ComboBox cmb_Search;
        private System.Windows.Forms.Panel pnDetail;
        private System.Windows.Forms.Panel groupBox2;
        private CMDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CreateTime;
        private System.Windows.Forms.TextBox txt_Email;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.TextBox txt_LoginId;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.TextBox txt_Part;
        private System.Windows.Forms.TextBox txt_Rank;
        private System.Windows.Forms.NumericUpDown ClientUserId;
        private System.Windows.Forms.TextBox txt_PhoneNo;
        private System.Windows.Forms.TextBox txt_MobileNo;
        private System.Windows.Forms.TextBox txt_ClientSearch;
        public System.Windows.Forms.ComboBox cmb_ClientSerach;
        private DataSets.CustomerStartManageDataSet customerStartManageDataSet;
        private System.Windows.Forms.BindingSource customerAddPhoneBindingSource;
        private DataSets.CustomerStartManageDataSetTableAdapters.CustomerAddPhoneTableAdapter customerAddPhoneTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientUserIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addTeamDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addPhoneNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobilePhoneNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rankDataGridViewTextBoxColumn;
    }
}