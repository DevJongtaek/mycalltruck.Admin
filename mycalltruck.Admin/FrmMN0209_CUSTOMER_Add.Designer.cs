namespace mycalltruck.Admin
{
    partial class FrmMN0209_CUSTOMER_Add
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
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.lbl_Password = new System.Windows.Forms.Label();
            this.txt_LoginId = new System.Windows.Forms.TextBox();
            this.lbl_LoginId = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnCustomerExist = new System.Windows.Forms.Button();
            this.txt_BizNo = new System.Windows.Forms.TextBox();
            this.cmbCustomerMId = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnFindZip = new System.Windows.Forms.Button();
            this.txt_Zip = new System.Windows.Forms.TextBox();
            this.txt_City = new System.Windows.Forms.TextBox();
            this.txt_State = new System.Windows.Forms.TextBox();
            this.txt_Street = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_Upjong = new System.Windows.Forms.TextBox();
            this.txt_Uptae = new System.Windows.Forms.TextBox();
            this.txt_CEO = new System.Windows.Forms.TextBox();
            this.txt_Code = new System.Windows.Forms.TextBox();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_Gubun = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_RegisterNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_Email = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_FaxNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_CreateDate = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_MobileNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_ChargeName = new System.Windows.Forms.TextBox();
            this.txt_PhoneNo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmb_SalesGubun = new System.Windows.Forms.ComboBox();
            this.cmb_EndDay = new System.Windows.Forms.ComboBox();
            this.lbl_EndDay = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.nudSalesDay = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btn_DriverExcel = new System.Windows.Forms.Button();
            this.btnExcelImport = new System.Windows.Forms.Button();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.cmDataSet = new mycalltruck.Admin.CMDataSet();
            this.customersTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.CustomersTableAdapter();
            this.customerManagerDataSet = new mycalltruck.Admin.DataSets.CustomerManagerDataSet();
            this.customerManagerTableAdapter = new mycalltruck.Admin.DataSets.CustomerManagerDataSetTableAdapters.CustomerManagerTableAdapter();
            this.btnMisuExcel = new System.Windows.Forms.Button();
            this.btnMisuExcelUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSalesDay)).BeginInit();
            this.pnProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerManagerDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(1193, 267);
            this.groupBox2.TabIndex = 28;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.txt_Password, 3, 7);
            this.tableLayoutPanel2.Controls.Add(this.lbl_Password, 2, 7);
            this.tableLayoutPanel2.Controls.Add(this.txt_LoginId, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.lbl_LoginId, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbCustomerMId, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label17, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Upjong, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Uptae, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_CEO, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_Code, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_Name, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmb_Gubun, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_RegisterNo, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.label9, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txt_Email, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label4, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label10, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.txt_FaxNo, 5, 4);
            this.tableLayoutPanel2.Controls.Add(this.label14, 4, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_CreateDate, 5, 5);
            this.tableLayoutPanel2.Controls.Add(this.label13, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_MobileNo, 3, 5);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_ChargeName, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.txt_PhoneNo, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 6, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel10, 3, 6);
            this.tableLayoutPanel2.Controls.Add(this.label11, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.label5, 6, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1191, 265);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // txt_Password
            // 
            this.txt_Password.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Password.Location = new System.Drawing.Point(340, 236);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txt_Password.MaxLength = 11;
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.ReadOnly = true;
            this.txt_Password.Size = new System.Drawing.Size(154, 25);
            this.txt_Password.TabIndex = 67;
            // 
            // lbl_Password
            // 
            this.lbl_Password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.lbl_Password.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Password.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_Password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lbl_Password.Location = new System.Drawing.Point(253, 236);
            this.lbl_Password.Margin = new System.Windows.Forms.Padding(4);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(80, 24);
            this.lbl_Password.TabIndex = 66;
            this.lbl_Password.Text = "비밀번호";
            this.lbl_Password.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_LoginId
            // 
            this.txt_LoginId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_LoginId.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_LoginId.Location = new System.Drawing.Point(92, 235);
            this.txt_LoginId.MaxLength = 11;
            this.txt_LoginId.Name = "txt_LoginId";
            this.txt_LoginId.ReadOnly = true;
            this.txt_LoginId.Size = new System.Drawing.Size(154, 25);
            this.txt_LoginId.TabIndex = 65;
            // 
            // lbl_LoginId
            // 
            this.lbl_LoginId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.lbl_LoginId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_LoginId.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_LoginId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lbl_LoginId.Location = new System.Drawing.Point(5, 236);
            this.lbl_LoginId.Margin = new System.Windows.Forms.Padding(4);
            this.lbl_LoginId.Name = "lbl_LoginId";
            this.lbl_LoginId.Size = new System.Drawing.Size(80, 24);
            this.lbl_LoginId.TabIndex = 64;
            this.lbl_LoginId.Text = "접속아이디";
            this.lbl_LoginId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnCustomerExist);
            this.panel4.Controls.Add(this.txt_BizNo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(337, 1);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(180, 33);
            this.panel4.TabIndex = 1;
            this.panel4.TabStop = true;
            // 
            // btnCustomerExist
            // 
            this.btnCustomerExist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCustomerExist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnCustomerExist.FlatAppearance.BorderSize = 0;
            this.btnCustomerExist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerExist.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCustomerExist.ForeColor = System.Drawing.Color.White;
            this.btnCustomerExist.Location = new System.Drawing.Point(138, 3);
            this.btnCustomerExist.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnCustomerExist.Name = "btnCustomerExist";
            this.btnCustomerExist.Size = new System.Drawing.Size(40, 27);
            this.btnCustomerExist.TabIndex = 50;
            this.btnCustomerExist.TabStop = false;
            this.btnCustomerExist.Text = "확인";
            this.btnCustomerExist.UseVisualStyleBackColor = false;
            this.btnCustomerExist.EnabledChanged += new System.EventHandler(this.Button_EnabledChanged);
            this.btnCustomerExist.Click += new System.EventHandler(this.btnCustomerExist_Click);
            this.btnCustomerExist.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnCustomerExist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // txt_BizNo
            // 
            this.txt_BizNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_BizNo.Location = new System.Drawing.Point(3, 4);
            this.txt_BizNo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_BizNo.MaxLength = 10;
            this.txt_BizNo.Name = "txt_BizNo";
            this.txt_BizNo.Size = new System.Drawing.Size(131, 25);
            this.txt_BizNo.TabIndex = 1;
            this.txt_BizNo.TextChanged += new System.EventHandler(this.txt_BizNo_TextChanged);
            this.txt_BizNo.Enter += new System.EventHandler(this.txt_BizNo_Enter);
            this.txt_BizNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_BizNo_KeyPress);
            this.txt_BizNo.Leave += new System.EventHandler(this.txt_BizNo_Leave);
            // 
            // cmbCustomerMId
            // 
            this.cmbCustomerMId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCustomerMId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerMId.Enabled = false;
            this.cmbCustomerMId.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbCustomerMId.FormattingEnabled = true;
            this.cmbCustomerMId.Location = new System.Drawing.Point(92, 204);
            this.cmbCustomerMId.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cmbCustomerMId.Name = "cmbCustomerMId";
            this.cmbCustomerMId.Size = new System.Drawing.Size(154, 25);
            this.cmbCustomerMId.TabIndex = 61;
            this.cmbCustomerMId.TabStop = false;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label19.Location = new System.Drawing.Point(5, 203);
            this.label19.Margin = new System.Windows.Forms.Padding(4);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 25);
            this.label19.TabIndex = 60;
            this.label19.Text = "화주담당자";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel3, 5);
            this.panel3.Controls.Add(this.btnFindZip);
            this.panel3.Controls.Add(this.txt_Zip);
            this.panel3.Controls.Add(this.txt_City);
            this.panel3.Controls.Add(this.txt_State);
            this.panel3.Controls.Add(this.txt_Street);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(89, 67);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(736, 33);
            this.panel3.TabIndex = 6;
            // 
            // btnFindZip
            // 
            this.btnFindZip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194)))));
            this.btnFindZip.Enabled = false;
            this.btnFindZip.FlatAppearance.BorderSize = 0;
            this.btnFindZip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindZip.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnFindZip.ForeColor = System.Drawing.Color.White;
            this.btnFindZip.Location = new System.Drawing.Point(601, 2);
            this.btnFindZip.Name = "btnFindZip";
            this.btnFindZip.Size = new System.Drawing.Size(107, 29);
            this.btnFindZip.TabIndex = 50;
            this.btnFindZip.Text = "우편번호검색";
            this.btnFindZip.UseVisualStyleBackColor = false;
            this.btnFindZip.EnabledChanged += new System.EventHandler(this.Button_EnabledChanged);
            this.btnFindZip.Click += new System.EventHandler(this.btnFindZip_Click);
            this.btnFindZip.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnFindZip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // txt_Zip
            // 
            this.txt_Zip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Zip.BackColor = System.Drawing.SystemColors.Control;
            this.txt_Zip.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Zip.Location = new System.Drawing.Point(4, 5);
            this.txt_Zip.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_Zip.MaxLength = 20;
            this.txt_Zip.Name = "txt_Zip";
            this.txt_Zip.ReadOnly = true;
            this.txt_Zip.Size = new System.Drawing.Size(66, 25);
            this.txt_Zip.TabIndex = 17;
            this.txt_Zip.TabStop = false;
            // 
            // txt_City
            // 
            this.txt_City.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_City.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_City.Location = new System.Drawing.Point(179, 5);
            this.txt_City.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_City.MaxLength = 20;
            this.txt_City.Name = "txt_City";
            this.txt_City.ReadOnly = true;
            this.txt_City.Size = new System.Drawing.Size(99, 25);
            this.txt_City.TabIndex = 15;
            this.txt_City.TabStop = false;
            // 
            // txt_State
            // 
            this.txt_State.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_State.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_State.Location = new System.Drawing.Point(76, 5);
            this.txt_State.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_State.MaxLength = 20;
            this.txt_State.Name = "txt_State";
            this.txt_State.ReadOnly = true;
            this.txt_State.Size = new System.Drawing.Size(99, 25);
            this.txt_State.TabIndex = 14;
            this.txt_State.TabStop = false;
            // 
            // txt_Street
            // 
            this.txt_Street.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Street.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Street.Location = new System.Drawing.Point(284, 5);
            this.txt_Street.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_Street.MaxLength = 100;
            this.txt_Street.Name = "txt_Street";
            this.txt_Street.ReadOnly = true;
            this.txt_Street.Size = new System.Drawing.Size(311, 25);
            this.txt_Street.TabIndex = 6;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label17.Location = new System.Drawing.Point(253, 5);
            this.label17.Margin = new System.Windows.Forms.Padding(4);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 25);
            this.label17.TabIndex = 35;
            this.label17.Text = "사업자번호";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label18.Location = new System.Drawing.Point(5, 5);
            this.label18.Margin = new System.Windows.Forms.Padding(4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 25);
            this.label18.TabIndex = 34;
            this.label18.Text = "거래처코드";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label12.Location = new System.Drawing.Point(5, 38);
            this.label12.Margin = new System.Windows.Forms.Padding(4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 25);
            this.label12.TabIndex = 11;
            this.label12.Text = "대표자";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label8.Location = new System.Drawing.Point(521, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "업종";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label2.Location = new System.Drawing.Point(253, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "업태";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label7.Location = new System.Drawing.Point(521, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 25);
            this.label7.TabIndex = 0;
            this.label7.Text = "성명/상호";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Upjong
            // 
            this.txt_Upjong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Upjong.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Upjong.Location = new System.Drawing.Point(608, 39);
            this.txt_Upjong.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_Upjong.MaxLength = 15;
            this.txt_Upjong.Name = "txt_Upjong";
            this.txt_Upjong.ReadOnly = true;
            this.txt_Upjong.Size = new System.Drawing.Size(154, 25);
            this.txt_Upjong.TabIndex = 5;
            this.txt_Upjong.Enter += new System.EventHandler(this.Control_Enter);
            this.txt_Upjong.Leave += new System.EventHandler(this.txt_Upjong_Leave);
            // 
            // txt_Uptae
            // 
            this.txt_Uptae.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Uptae.Location = new System.Drawing.Point(340, 39);
            this.txt_Uptae.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_Uptae.MaxLength = 15;
            this.txt_Uptae.Name = "txt_Uptae";
            this.txt_Uptae.ReadOnly = true;
            this.txt_Uptae.Size = new System.Drawing.Size(154, 25);
            this.txt_Uptae.TabIndex = 4;
            this.txt_Uptae.Enter += new System.EventHandler(this.Control_Enter);
            this.txt_Uptae.Leave += new System.EventHandler(this.txt_Uptae_Leave);
            // 
            // txt_CEO
            // 
            this.txt_CEO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CEO.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_CEO.Location = new System.Drawing.Point(92, 39);
            this.txt_CEO.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_CEO.MaxLength = 15;
            this.txt_CEO.Name = "txt_CEO";
            this.txt_CEO.ReadOnly = true;
            this.txt_CEO.Size = new System.Drawing.Size(154, 25);
            this.txt_CEO.TabIndex = 3;
            this.txt_CEO.Enter += new System.EventHandler(this.Control_Enter);
            this.txt_CEO.Leave += new System.EventHandler(this.txt_CEO_Leave);
            // 
            // txt_Code
            // 
            this.txt_Code.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Code.Enabled = false;
            this.txt_Code.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Code.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txt_Code.Location = new System.Drawing.Point(92, 6);
            this.txt_Code.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_Code.MaxLength = 6;
            this.txt_Code.Name = "txt_Code";
            this.txt_Code.ReadOnly = true;
            this.txt_Code.Size = new System.Drawing.Size(154, 25);
            this.txt_Code.TabIndex = 1;
            this.txt_Code.TabStop = false;
            // 
            // txt_Name
            // 
            this.txt_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Name.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Name.Location = new System.Drawing.Point(608, 6);
            this.txt_Name.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_Name.MaxLength = 30;
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.ReadOnly = true;
            this.txt_Name.Size = new System.Drawing.Size(154, 25);
            this.txt_Name.TabIndex = 2;
            this.txt_Name.Enter += new System.EventHandler(this.Control_Enter);
            this.txt_Name.Leave += new System.EventHandler(this.txt_Name_Leave);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label6.Location = new System.Drawing.Point(5, 71);
            this.label6.Margin = new System.Windows.Forms.Padding(4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "주소";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmb_Gubun
            // 
            this.cmb_Gubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Gubun.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmb_Gubun.FormattingEnabled = true;
            this.cmb_Gubun.Location = new System.Drawing.Point(92, 105);
            this.cmb_Gubun.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cmb_Gubun.Name = "cmb_Gubun";
            this.cmb_Gubun.Size = new System.Drawing.Size(154, 25);
            this.cmb_Gubun.TabIndex = 7;
            this.cmb_Gubun.TabStop = false;
            this.cmb_Gubun.SelectedIndexChanged += new System.EventHandler(this.cmb_Gubun_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label15.Location = new System.Drawing.Point(5, 104);
            this.label15.Margin = new System.Windows.Forms.Padding(4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 25);
            this.label15.TabIndex = 14;
            this.label15.Text = "거래구분";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label3.Location = new System.Drawing.Point(253, 104);
            this.label3.Margin = new System.Windows.Forms.Padding(4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 25);
            this.label3.TabIndex = 38;
            this.label3.Text = "법인등록번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_RegisterNo
            // 
            this.txt_RegisterNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_RegisterNo.Location = new System.Drawing.Point(340, 105);
            this.txt_RegisterNo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_RegisterNo.MaxLength = 13;
            this.txt_RegisterNo.Name = "txt_RegisterNo";
            this.txt_RegisterNo.ReadOnly = true;
            this.txt_RegisterNo.Size = new System.Drawing.Size(111, 25);
            this.txt_RegisterNo.TabIndex = 7;
            this.txt_RegisterNo.Enter += new System.EventHandler(this.txt_RegisterNo_Enter);
            this.txt_RegisterNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_RegisterNo_KeyPress);
            this.txt_RegisterNo.Leave += new System.EventHandler(this.txt_RegisterNo_Leave);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label9.Location = new System.Drawing.Point(521, 104);
            this.label9.Margin = new System.Windows.Forms.Padding(4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 25);
            this.label9.TabIndex = 40;
            this.label9.Text = "전표구분";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label16.Location = new System.Drawing.Point(5, 137);
            this.label16.Margin = new System.Windows.Forms.Padding(4);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 25);
            this.label16.TabIndex = 15;
            this.label16.Text = "e-메일";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Email
            // 
            this.txt_Email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Email.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_Email.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txt_Email.Location = new System.Drawing.Point(92, 138);
            this.txt_Email.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_Email.MaxLength = 50;
            this.txt_Email.Name = "txt_Email";
            this.txt_Email.ReadOnly = true;
            this.txt_Email.Size = new System.Drawing.Size(154, 25);
            this.txt_Email.TabIndex = 8;
            this.txt_Email.Enter += new System.EventHandler(this.Control_Enter);
            this.txt_Email.Leave += new System.EventHandler(this.txt_Email_Leave);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label4.Location = new System.Drawing.Point(253, 137);
            this.label4.Margin = new System.Windows.Forms.Padding(4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "전화번호";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label10.Location = new System.Drawing.Point(521, 137);
            this.label10.Margin = new System.Windows.Forms.Padding(4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 25);
            this.label10.TabIndex = 9;
            this.label10.Text = "팩스번호";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_FaxNo
            // 
            this.txt_FaxNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_FaxNo.Location = new System.Drawing.Point(608, 138);
            this.txt_FaxNo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_FaxNo.MaxLength = 11;
            this.txt_FaxNo.Name = "txt_FaxNo";
            this.txt_FaxNo.ReadOnly = true;
            this.txt_FaxNo.Size = new System.Drawing.Size(111, 25);
            this.txt_FaxNo.TabIndex = 10;
            this.txt_FaxNo.Enter += new System.EventHandler(this.txt_FaxNo_Enter);
            this.txt_FaxNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_FaxNo_KeyPress);
            this.txt_FaxNo.Leave += new System.EventHandler(this.txt_FaxNo_Leave);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label14.Location = new System.Drawing.Point(521, 170);
            this.label14.Margin = new System.Windows.Forms.Padding(4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 25);
            this.label14.TabIndex = 13;
            this.label14.Text = "등록일자";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_CreateDate
            // 
            this.txt_CreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CreateDate.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_CreateDate.Location = new System.Drawing.Point(608, 171);
            this.txt_CreateDate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_CreateDate.Name = "txt_CreateDate";
            this.txt_CreateDate.ReadOnly = true;
            this.txt_CreateDate.Size = new System.Drawing.Size(154, 25);
            this.txt_CreateDate.TabIndex = 27;
            this.txt_CreateDate.TabStop = false;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(141)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label13.Location = new System.Drawing.Point(253, 170);
            this.label13.Margin = new System.Windows.Forms.Padding(4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 25);
            this.label13.TabIndex = 12;
            this.label13.Text = "핸드폰번호";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_MobileNo
            // 
            this.txt_MobileNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_MobileNo.Location = new System.Drawing.Point(340, 171);
            this.txt_MobileNo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_MobileNo.MaxLength = 13;
            this.txt_MobileNo.Name = "txt_MobileNo";
            this.txt_MobileNo.ReadOnly = true;
            this.txt_MobileNo.Size = new System.Drawing.Size(111, 25);
            this.txt_MobileNo.TabIndex = 12;
            this.txt_MobileNo.Enter += new System.EventHandler(this.txt_MobileNo_Enter);
            this.txt_MobileNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_MobileNo_KeyPress);
            this.txt_MobileNo.Leave += new System.EventHandler(this.txt_MobileNo_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label1.Location = new System.Drawing.Point(5, 170);
            this.label1.Margin = new System.Windows.Forms.Padding(4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 42;
            this.label1.Text = "담당자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_ChargeName
            // 
            this.txt_ChargeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_ChargeName.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_ChargeName.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txt_ChargeName.Location = new System.Drawing.Point(92, 171);
            this.txt_ChargeName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_ChargeName.MaxLength = 50;
            this.txt_ChargeName.Name = "txt_ChargeName";
            this.txt_ChargeName.ReadOnly = true;
            this.txt_ChargeName.Size = new System.Drawing.Size(154, 25);
            this.txt_ChargeName.TabIndex = 11;
            // 
            // txt_PhoneNo
            // 
            this.txt_PhoneNo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txt_PhoneNo.Location = new System.Drawing.Point(340, 138);
            this.txt_PhoneNo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_PhoneNo.MaxLength = 13;
            this.txt_PhoneNo.Name = "txt_PhoneNo";
            this.txt_PhoneNo.ReadOnly = true;
            this.txt_PhoneNo.Size = new System.Drawing.Size(111, 25);
            this.txt_PhoneNo.TabIndex = 9;
            this.txt_PhoneNo.Enter += new System.EventHandler(this.txt_PhoneNo_Enter);
            this.txt_PhoneNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_PhoneNo_KeyPress);
            this.txt_PhoneNo.Leave += new System.EventHandler(this.txt_PhoneNo_Leave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmb_SalesGubun);
            this.panel1.Controls.Add(this.cmb_EndDay);
            this.panel1.Controls.Add(this.lbl_EndDay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(605, 100);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 33);
            this.panel1.TabIndex = 43;
            // 
            // cmb_SalesGubun
            // 
            this.cmb_SalesGubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_SalesGubun.Enabled = false;
            this.cmb_SalesGubun.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmb_SalesGubun.FormattingEnabled = true;
            this.cmb_SalesGubun.Location = new System.Drawing.Point(3, 4);
            this.cmb_SalesGubun.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cmb_SalesGubun.Name = "cmb_SalesGubun";
            this.cmb_SalesGubun.Size = new System.Drawing.Size(111, 25);
            this.cmb_SalesGubun.TabIndex = 9;
            this.cmb_SalesGubun.TabStop = false;
            this.cmb_SalesGubun.SelectedIndexChanged += new System.EventHandler(this.cmb_SalesGubun_SelectedIndexChanged);
            this.cmb_SalesGubun.Click += new System.EventHandler(this.Control_Enter);
            // 
            // cmb_EndDay
            // 
            this.cmb_EndDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_EndDay.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmb_EndDay.FormattingEnabled = true;
            this.cmb_EndDay.Location = new System.Drawing.Point(167, 4);
            this.cmb_EndDay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cmb_EndDay.Name = "cmb_EndDay";
            this.cmb_EndDay.Size = new System.Drawing.Size(50, 25);
            this.cmb_EndDay.TabIndex = 45;
            this.cmb_EndDay.TabStop = false;
            // 
            // lbl_EndDay
            // 
            this.lbl_EndDay.Location = new System.Drawing.Point(122, 4);
            this.lbl_EndDay.Name = "lbl_EndDay";
            this.lbl_EndDay.Size = new System.Drawing.Size(41, 25);
            this.lbl_EndDay.TabIndex = 44;
            this.lbl_EndDay.Text = "마감일";
            this.lbl_EndDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtRemark);
            this.panel2.Location = new System.Drawing.Point(825, 34);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.tableLayoutPanel2.SetRowSpan(this.panel2, 7);
            this.panel2.Size = new System.Drawing.Size(366, 227);
            this.panel2.TabIndex = 45;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemark.Location = new System.Drawing.Point(0, 0);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ReadOnly = true;
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(366, 227);
            this.txtRemark.TabIndex = 1;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.label23);
            this.panel10.Controls.Add(this.nudSalesDay);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(340, 202);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(174, 27);
            this.panel10.TabIndex = 62;
            // 
            // label23
            // 
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.label23.Location = new System.Drawing.Point(64, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(110, 27);
            this.label23.TabIndex = 47;
            this.label23.Text = "일";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label23.Visible = false;
            // 
            // nudSalesDay
            // 
            this.nudSalesDay.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudSalesDay.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.nudSalesDay.Location = new System.Drawing.Point(0, 0);
            this.nudSalesDay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.nudSalesDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudSalesDay.Name = "nudSalesDay";
            this.nudSalesDay.Size = new System.Drawing.Size(64, 25);
            this.nudSalesDay.TabIndex = 0;
            this.nudSalesDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(253, 203);
            this.label11.Margin = new System.Windows.Forms.Padding(4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 25);
            this.label11.TabIndex = 63;
            this.label11.Text = "정기발행일";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label5.Location = new System.Drawing.Point(828, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 25);
            this.label5.TabIndex = 44;
            this.label5.Text = "화주메모";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1104, 276);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 29);
            this.btnClose.TabIndex = 30;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194)))));
            this.btnAddClose.Enabled = false;
            this.btnAddClose.FlatAppearance.BorderSize = 0;
            this.btnAddClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddClose.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddClose.ForeColor = System.Drawing.Color.White;
            this.btnAddClose.Location = new System.Drawing.Point(1004, 276);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(90, 29);
            this.btnAddClose.TabIndex = 31;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기";
            this.btnAddClose.UseVisualStyleBackColor = false;
            this.btnAddClose.EnabledChanged += new System.EventHandler(this.Button_EnabledChanged);
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            this.btnAddClose.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnAddClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194)))));
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(901, 276);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 29);
            this.btnAdd.TabIndex = 29;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "등록후추가";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.EnabledChanged += new System.EventHandler(this.Button_EnabledChanged);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnAdd.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnAdd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // btn_DriverExcel
            // 
            this.btn_DriverExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_DriverExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_DriverExcel.FlatAppearance.BorderSize = 0;
            this.btn_DriverExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DriverExcel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btn_DriverExcel.ForeColor = System.Drawing.Color.White;
            this.btn_DriverExcel.Location = new System.Drawing.Point(121, 276);
            this.btn_DriverExcel.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btn_DriverExcel.Name = "btn_DriverExcel";
            this.btn_DriverExcel.Size = new System.Drawing.Size(80, 29);
            this.btn_DriverExcel.TabIndex = 46;
            this.btn_DriverExcel.TabStop = false;
            this.btn_DriverExcel.Text = "양식";
            this.btn_DriverExcel.UseVisualStyleBackColor = false;
            this.btn_DriverExcel.Click += new System.EventHandler(this.btn_DriverExcel_Click);
            this.btn_DriverExcel.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btn_DriverExcel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcelImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnExcelImport.FlatAppearance.BorderSize = 0;
            this.btnExcelImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcelImport.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnExcelImport.ForeColor = System.Drawing.Color.White;
            this.btnExcelImport.Location = new System.Drawing.Point(9, 276);
            this.btnExcelImport.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(107, 29);
            this.btnExcelImport.TabIndex = 45;
            this.btnExcelImport.TabStop = false;
            this.btnExcelImport.Text = "엑셀 일괄등록";
            this.btnExcelImport.UseVisualStyleBackColor = false;
            this.btnExcelImport.Click += new System.EventHandler(this.btnExcelImport_Click);
            this.btnExcelImport.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnExcelImport.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(407, 85);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 47;
            this.pnProgress.Visible = false;
            // 
            // label66
            // 
            this.label66.BackColor = System.Drawing.Color.Transparent;
            this.label66.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label66.Location = new System.Drawing.Point(10, 10);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(180, 21);
            this.label66.TabIndex = 3;
            this.label66.Text = "잠시만 기다려 주십시오.";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bar
            // 
            this.bar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar.Location = new System.Drawing.Point(10, 31);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(180, 23);
            this.bar.Step = 1;
            this.bar.TabIndex = 2;
            // 
            // cmDataSet
            // 
            this.cmDataSet.DataSetName = "CMDataSet";
            this.cmDataSet.EnforceConstraints = false;
            this.cmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customersTableAdapter
            // 
            this.customersTableAdapter.ClearBeforeFill = true;
            // 
            // customerManagerDataSet
            // 
            this.customerManagerDataSet.DataSetName = "CustomerManagerDataSet";
            this.customerManagerDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customerManagerTableAdapter
            // 
            this.customerManagerTableAdapter.ClearBeforeFill = true;
            // 
            // btnMisuExcel
            // 
            this.btnMisuExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMisuExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnMisuExcel.FlatAppearance.BorderSize = 0;
            this.btnMisuExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMisuExcel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnMisuExcel.ForeColor = System.Drawing.Color.White;
            this.btnMisuExcel.Location = new System.Drawing.Point(399, 276);
            this.btnMisuExcel.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnMisuExcel.Name = "btnMisuExcel";
            this.btnMisuExcel.Size = new System.Drawing.Size(80, 29);
            this.btnMisuExcel.TabIndex = 49;
            this.btnMisuExcel.TabStop = false;
            this.btnMisuExcel.Text = "양식";
            this.btnMisuExcel.UseVisualStyleBackColor = false;
            this.btnMisuExcel.Click += new System.EventHandler(this.btnMisuExcel_Click);
            this.btnMisuExcel.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnMisuExcel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // btnMisuExcelUp
            // 
            this.btnMisuExcelUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMisuExcelUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnMisuExcelUp.FlatAppearance.BorderSize = 0;
            this.btnMisuExcelUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMisuExcelUp.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnMisuExcelUp.ForeColor = System.Drawing.Color.White;
            this.btnMisuExcelUp.Location = new System.Drawing.Point(287, 276);
            this.btnMisuExcelUp.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnMisuExcelUp.Name = "btnMisuExcelUp";
            this.btnMisuExcelUp.Size = new System.Drawing.Size(107, 29);
            this.btnMisuExcelUp.TabIndex = 48;
            this.btnMisuExcelUp.TabStop = false;
            this.btnMisuExcelUp.Text = "미수 일괄등록";
            this.btnMisuExcelUp.UseVisualStyleBackColor = false;
            this.btnMisuExcelUp.Click += new System.EventHandler(this.btnMisuExcelUp_Click);
            this.btnMisuExcelUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            this.btnMisuExcelUp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_MouseMove);
            // 
            // FrmMN0209_CUSTOMER_Add
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1193, 314);
            this.Controls.Add(this.btnMisuExcel);
            this.Controls.Add(this.btnMisuExcelUp);
            this.Controls.Add(this.btn_DriverExcel);
            this.Controls.Add(this.btnExcelImport);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.pnProgress);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FrmMN0209_CUSTOMER_Add";
            this.Text = "거래처 추가";
            this.Load += new System.EventHandler(this.FrmMN0209_CUSTOMER_Add_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSalesDay)).EndInit();
            this.pnProgress.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerManagerDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private CMDataSet cmDataSet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_Upjong;
        private System.Windows.Forms.TextBox txt_Uptae;
        private System.Windows.Forms.TextBox txt_CEO;
        private System.Windows.Forms.TextBox txt_Code;
        private System.Windows.Forms.TextBox txt_BizNo;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmb_Gubun;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_RegisterNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmb_SalesGubun;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_Email;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_FaxNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_CreateDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_MobileNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_ChargeName;
        private CMDataSetTableAdapters.CustomersTableAdapter customersTableAdapter;
        private System.Windows.Forms.Button btn_DriverExcel;
        private System.Windows.Forms.Button btnExcelImport;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txt_Zip;
        private System.Windows.Forms.TextBox txt_City;
        private System.Windows.Forms.TextBox txt_State;
        private System.Windows.Forms.TextBox txt_Street;
        private System.Windows.Forms.TextBox txt_PhoneNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_EndDay;
        private System.Windows.Forms.ComboBox cmb_EndDay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cmbCustomerMId;
        private DataSets.CustomerManagerDataSet customerManagerDataSet;
        private DataSets.CustomerManagerDataSetTableAdapters.CustomerManagerTableAdapter customerManagerTableAdapter;
        private System.Windows.Forms.Button btnMisuExcel;
        private System.Windows.Forms.Button btnMisuExcelUp;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnCustomerExist;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown nudSalesDay;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnFindZip;
        private System.Windows.Forms.Label lbl_LoginId;
        private System.Windows.Forms.TextBox txt_LoginId;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.TextBox txt_Password;
    }
}