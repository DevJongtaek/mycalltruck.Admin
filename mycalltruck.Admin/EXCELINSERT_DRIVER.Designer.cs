namespace mycalltruck.Admin
{
    partial class EXCELINSERT_DRIVER
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
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.label1 = new System.Windows.Forms.Label();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSBiz_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSUptae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSUpjong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCeo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCeoBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSMobileNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSPhoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSFaxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSStreet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSBizGubun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSRouteType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSInsurance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarGubun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSPayBankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSPayAccountNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSInputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarstate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarcity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSCarStreet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSfpisCartype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Close = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.btn_Test = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.staticOptionsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.StaticOptionsTableAdapter();
            this.btn_Info = new System.Windows.Forms.Button();
            this.driversTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DriversTableAdapter();
            this.btn_Update = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cmb_Savegubun = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.SuspendLayout();
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "의뢰건수";
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
            this.colNumber,
            this.ColumnSBiz_NO,
            this.ColumnSName,
            this.ColumnSUptae,
            this.ColumnSUpjong,
            this.ColumnSCeo,
            this.ColumnSCeoBirth,
            this.ColumnSMobileNo,
            this.ColumnSPhoneNo,
            this.ColumnSFaxNo,
            this.ColumnSEmail,
            this.ColumnSState,
            this.ColumnSCity,
            this.ColumnSStreet,
            this.ColumnSBizGubun,
            this.ColumnSRouteType,
            this.ColumnSInsurance,
            this.ColumnSCarNo,
            this.ColumnSCarType,
            this.ColumnSCarSize,
            this.ColumnSCarGubun,
            this.ColumnSCarYear,
            this.ColumnSPayBankName,
            this.ColumnSPayAccountNo,
            this.ColumnSInputName,
            this.ColumnSCarstate,
            this.ColumnSCarcity,
            this.ColumnSCarStreet,
            this.ColumnSfpisCartype,
            this.ColumnError});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle3;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(282, 13);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.ReadOnly = true;
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.Size = new System.Drawing.Size(708, 254);
            this.newDGV1.TabIndex = 14;
            // 
            // colNumber
            // 
            this.colNumber.DataPropertyName = "S_Idx";
            this.colNumber.HeaderText = "IDX";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNumber.Width = 60;
            // 
            // ColumnSBiz_NO
            // 
            this.ColumnSBiz_NO.DataPropertyName = "SBiz_NO";
            this.ColumnSBiz_NO.HeaderText = "사업자번호";
            this.ColumnSBiz_NO.Name = "ColumnSBiz_NO";
            this.ColumnSBiz_NO.ReadOnly = true;
            this.ColumnSBiz_NO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSName
            // 
            this.ColumnSName.DataPropertyName = "SName";
            this.ColumnSName.HeaderText = "상호";
            this.ColumnSName.Name = "ColumnSName";
            this.ColumnSName.ReadOnly = true;
            this.ColumnSName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSUptae
            // 
            this.ColumnSUptae.DataPropertyName = "SUptae";
            this.ColumnSUptae.HeaderText = "업태";
            this.ColumnSUptae.Name = "ColumnSUptae";
            this.ColumnSUptae.ReadOnly = true;
            this.ColumnSUptae.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSUpjong
            // 
            this.ColumnSUpjong.DataPropertyName = "SUpjong";
            this.ColumnSUpjong.HeaderText = "종목";
            this.ColumnSUpjong.Name = "ColumnSUpjong";
            this.ColumnSUpjong.ReadOnly = true;
            this.ColumnSUpjong.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCeo
            // 
            this.ColumnSCeo.DataPropertyName = "SCeo";
            this.ColumnSCeo.HeaderText = "성명";
            this.ColumnSCeo.Name = "ColumnSCeo";
            this.ColumnSCeo.ReadOnly = true;
            this.ColumnSCeo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCeoBirth
            // 
            this.ColumnSCeoBirth.DataPropertyName = "SCeoBirth";
            this.ColumnSCeoBirth.HeaderText = "생년월일";
            this.ColumnSCeoBirth.Name = "ColumnSCeoBirth";
            this.ColumnSCeoBirth.ReadOnly = true;
            this.ColumnSCeoBirth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSMobileNo
            // 
            this.ColumnSMobileNo.DataPropertyName = "SMobileNo";
            this.ColumnSMobileNo.HeaderText = "핸드폰번호";
            this.ColumnSMobileNo.Name = "ColumnSMobileNo";
            this.ColumnSMobileNo.ReadOnly = true;
            this.ColumnSMobileNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSPhoneNo
            // 
            this.ColumnSPhoneNo.DataPropertyName = "SPhoneNo";
            this.ColumnSPhoneNo.HeaderText = "전화번호";
            this.ColumnSPhoneNo.Name = "ColumnSPhoneNo";
            this.ColumnSPhoneNo.ReadOnly = true;
            this.ColumnSPhoneNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSFaxNo
            // 
            this.ColumnSFaxNo.DataPropertyName = "SFaxNo";
            this.ColumnSFaxNo.HeaderText = "팩스번호";
            this.ColumnSFaxNo.Name = "ColumnSFaxNo";
            this.ColumnSFaxNo.ReadOnly = true;
            this.ColumnSFaxNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSEmail
            // 
            this.ColumnSEmail.DataPropertyName = "SEmail";
            this.ColumnSEmail.HeaderText = "이메일";
            this.ColumnSEmail.Name = "ColumnSEmail";
            this.ColumnSEmail.ReadOnly = true;
            this.ColumnSEmail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSState
            // 
            this.ColumnSState.DataPropertyName = "SState";
            this.ColumnSState.HeaderText = "시도";
            this.ColumnSState.Name = "ColumnSState";
            this.ColumnSState.ReadOnly = true;
            this.ColumnSState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCity
            // 
            this.ColumnSCity.DataPropertyName = "SCity";
            this.ColumnSCity.HeaderText = "시군구";
            this.ColumnSCity.Name = "ColumnSCity";
            this.ColumnSCity.ReadOnly = true;
            this.ColumnSCity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSStreet
            // 
            this.ColumnSStreet.DataPropertyName = "SStreet";
            this.ColumnSStreet.HeaderText = "상세주소";
            this.ColumnSStreet.Name = "ColumnSStreet";
            this.ColumnSStreet.ReadOnly = true;
            this.ColumnSStreet.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSBizGubun
            // 
            this.ColumnSBizGubun.DataPropertyName = "SBizGubun";
            this.ColumnSBizGubun.HeaderText = "사업자구분";
            this.ColumnSBizGubun.Name = "ColumnSBizGubun";
            this.ColumnSBizGubun.ReadOnly = true;
            this.ColumnSBizGubun.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSRouteType
            // 
            this.ColumnSRouteType.DataPropertyName = "SRouteType";
            this.ColumnSRouteType.HeaderText = "운행노선";
            this.ColumnSRouteType.Name = "ColumnSRouteType";
            this.ColumnSRouteType.ReadOnly = true;
            this.ColumnSRouteType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSInsurance
            // 
            this.ColumnSInsurance.DataPropertyName = "SInsurance";
            this.ColumnSInsurance.HeaderText = "적재물보험";
            this.ColumnSInsurance.Name = "ColumnSInsurance";
            this.ColumnSInsurance.ReadOnly = true;
            this.ColumnSInsurance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarNo
            // 
            this.ColumnSCarNo.DataPropertyName = "SCarNo";
            this.ColumnSCarNo.HeaderText = "차량번호";
            this.ColumnSCarNo.Name = "ColumnSCarNo";
            this.ColumnSCarNo.ReadOnly = true;
            this.ColumnSCarNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarType
            // 
            this.ColumnSCarType.DataPropertyName = "SCarType";
            this.ColumnSCarType.HeaderText = "차종";
            this.ColumnSCarType.Name = "ColumnSCarType";
            this.ColumnSCarType.ReadOnly = true;
            this.ColumnSCarType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarSize
            // 
            this.ColumnSCarSize.DataPropertyName = "SCarSize";
            this.ColumnSCarSize.HeaderText = "톤수(t)";
            this.ColumnSCarSize.Name = "ColumnSCarSize";
            this.ColumnSCarSize.ReadOnly = true;
            this.ColumnSCarSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarGubun
            // 
            this.ColumnSCarGubun.DataPropertyName = "SCarGubun";
            this.ColumnSCarGubun.HeaderText = "구분";
            this.ColumnSCarGubun.Name = "ColumnSCarGubun";
            this.ColumnSCarGubun.ReadOnly = true;
            this.ColumnSCarGubun.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarYear
            // 
            this.ColumnSCarYear.DataPropertyName = "SCarYear";
            this.ColumnSCarYear.HeaderText = "기사명";
            this.ColumnSCarYear.Name = "ColumnSCarYear";
            this.ColumnSCarYear.ReadOnly = true;
            this.ColumnSCarYear.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSPayBankName
            // 
            this.ColumnSPayBankName.DataPropertyName = "SPayBankName";
            this.ColumnSPayBankName.HeaderText = "은행";
            this.ColumnSPayBankName.Name = "ColumnSPayBankName";
            this.ColumnSPayBankName.ReadOnly = true;
            this.ColumnSPayBankName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSPayAccountNo
            // 
            this.ColumnSPayAccountNo.DataPropertyName = "SPayAccountNo";
            this.ColumnSPayAccountNo.HeaderText = "계좌번호";
            this.ColumnSPayAccountNo.Name = "ColumnSPayAccountNo";
            this.ColumnSPayAccountNo.ReadOnly = true;
            this.ColumnSPayAccountNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSInputName
            // 
            this.ColumnSInputName.DataPropertyName = "SInputName";
            this.ColumnSInputName.HeaderText = "예금주";
            this.ColumnSInputName.Name = "ColumnSInputName";
            this.ColumnSInputName.ReadOnly = true;
            this.ColumnSInputName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarstate
            // 
            this.ColumnSCarstate.DataPropertyName = "SCarstate";
            this.ColumnSCarstate.HeaderText = "시도";
            this.ColumnSCarstate.Name = "ColumnSCarstate";
            this.ColumnSCarstate.ReadOnly = true;
            this.ColumnSCarstate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarcity
            // 
            this.ColumnSCarcity.DataPropertyName = "SCarcity";
            this.ColumnSCarcity.HeaderText = "시군구";
            this.ColumnSCarcity.Name = "ColumnSCarcity";
            this.ColumnSCarcity.ReadOnly = true;
            this.ColumnSCarcity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSCarStreet
            // 
            this.ColumnSCarStreet.DataPropertyName = "SCarStreet";
            this.ColumnSCarStreet.HeaderText = "읍면동";
            this.ColumnSCarStreet.Name = "ColumnSCarStreet";
            this.ColumnSCarStreet.ReadOnly = true;
            this.ColumnSCarStreet.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSfpisCartype
            // 
            this.ColumnSfpisCartype.DataPropertyName = "SfpisCartype";
            this.ColumnSfpisCartype.HeaderText = "국토부차종";
            this.ColumnSfpisCartype.Name = "ColumnSfpisCartype";
            this.ColumnSfpisCartype.ReadOnly = true;
            this.ColumnSfpisCartype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnError
            // 
            this.ColumnError.DataPropertyName = "Error";
            this.ColumnError.HeaderText = "ERROR";
            this.ColumnError.Name = "ColumnError";
            this.ColumnError.ReadOnly = true;
            this.ColumnError.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Close.FlatAppearance.BorderSize = 0;
            this.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Close.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Close.ForeColor = System.Drawing.Color.White;
            this.btn_Close.Location = new System.Drawing.Point(134, 240);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 27);
            this.btn_Close.TabIndex = 17;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(235, 30);
            this.label7.TabIndex = 10;
            this.label7.Text = "데이터 검증에 실패 하였습니다.\r\n\"보기\"에서 수정한 후 , 다시 검증하십시오.\r\n";
            this.label7.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_OK);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(11, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 141);
            this.panel1.TabIndex = 16;
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_OK.FlatAppearance.BorderSize = 0;
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.Location = new System.Drawing.Point(164, 77);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 27);
            this.btn_OK.TabIndex = 9;
            this.btn_OK.Text = "보기";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "데이터 검증실패";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.MediumBlue;
            this.label6.Location = new System.Drawing.Point(131, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "데이터 검증성공";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.MediumBlue;
            this.label5.Location = new System.Drawing.Point(131, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.MediumBlue;
            this.label4.Location = new System.Drawing.Point(131, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "0";
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // btn_Test
            // 
            this.btn_Test.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Test.FlatAppearance.BorderSize = 0;
            this.btn_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Test.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Test.ForeColor = System.Drawing.Color.White;
            this.btn_Test.Location = new System.Drawing.Point(144, 12);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(104, 27);
            this.btn_Test.TabIndex = 15;
            this.btn_Test.Text = "데이터 검증";
            this.btn_Test.UseVisualStyleBackColor = false;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // staticOptionsTableAdapter
            // 
            this.staticOptionsTableAdapter.ClearBeforeFill = true;
            // 
            // btn_Info
            // 
            this.btn_Info.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Info.FlatAppearance.BorderSize = 0;
            this.btn_Info.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Info.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Info.ForeColor = System.Drawing.Color.White;
            this.btn_Info.Location = new System.Drawing.Point(11, 13);
            this.btn_Info.Name = "btn_Info";
            this.btn_Info.Size = new System.Drawing.Size(127, 27);
            this.btn_Info.TabIndex = 13;
            this.btn_Info.Text = "엑셀파일불러오기";
            this.btn_Info.UseVisualStyleBackColor = false;
            this.btn_Info.Click += new System.EventHandler(this.btn_Info_Click);
            // 
            // driversTableAdapter
            // 
            this.driversTableAdapter.ClearBeforeFill = true;
            // 
            // btn_Update
            // 
            this.btn_Update.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Update.Enabled = false;
            this.btn_Update.FlatAppearance.BorderSize = 0;
            this.btn_Update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Update.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Update.ForeColor = System.Drawing.Color.White;
            this.btn_Update.Location = new System.Drawing.Point(43, 240);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(79, 27);
            this.btn_Update.TabIndex = 18;
            this.btn_Update.Text = "일괄등록";
            this.btn_Update.UseVisualStyleBackColor = false;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 194);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 15);
            this.label8.TabIndex = 19;
            this.label8.Text = "저장할 데이터 선택";
            // 
            // cmb_Savegubun
            // 
            this.cmb_Savegubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Savegubun.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmb_Savegubun.FormattingEnabled = true;
            this.cmb_Savegubun.Items.AddRange(new object[] {
            "검증 성공 건 만 저장",
            "저장 안 함"});
            this.cmb_Savegubun.Location = new System.Drawing.Point(12, 210);
            this.cmb_Savegubun.Name = "cmb_Savegubun";
            this.cmb_Savegubun.Size = new System.Drawing.Size(241, 25);
            this.cmb_Savegubun.TabIndex = 20;
            this.cmb_Savegubun.SelectedIndexChanged += new System.EventHandler(this.cmb_Savegubun_SelectedIndexChanged);
            // 
            // EXCELINSERT_DRIVER
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1000, 281);
            this.Controls.Add(this.cmb_Savegubun);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.newDGV1);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_Test);
            this.Controls.Add(this.btn_Info);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "EXCELINSERT_DRIVER";
            this.Text = "차량일괄등록_표준형";
            this.Load += new System.EventHandler(this.EXCELINSERT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CMDataSet cMDataSet;
        private System.Windows.Forms.Label label1;
        private NewDGV newDGV1;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.Button btn_Test;
        private System.Windows.Forms.Button btn_Info;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private CMDataSetTableAdapters.StaticOptionsTableAdapter staticOptionsTableAdapter;
        private CMDataSetTableAdapters.DriversTableAdapter driversTableAdapter;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.ComboBox cmb_Savegubun;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSBiz_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSUptae;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSUpjong;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCeo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCeoBirth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSMobileNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSPhoneNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSFaxNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSStreet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSBizGubun;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSRouteType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSInsurance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarGubun;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSPayBankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSPayAccountNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSInputName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarstate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarcity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSCarStreet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSfpisCartype;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnError;
    }
}