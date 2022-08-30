namespace mycalltruck.Admin
{
    partial class EXCELINSERT2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_Info = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.btn_Test = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Close = new System.Windows.Forms.Button();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.ColumnIdx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBizNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCarNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCarType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClientPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAddedPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAddedCharge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRequestPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEndPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLastPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEndClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMemo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Update = new System.Windows.Forms.Button();
            this.fpiS_CONTTableAdapter = new mycalltruck.Admin.DataSets.FPISDataSetTableAdapters.FPIS_CONTTableAdapter();
            this.fpisDataSet = new mycalltruck.Admin.DataSets.FPISDataSet();
            this.PnProgress = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.BarProgress = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpisDataSet)).BeginInit();
            this.PnProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Info
            // 
            this.btn_Info.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Info.FlatAppearance.BorderSize = 0;
            this.btn_Info.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Info.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Info.ForeColor = System.Drawing.Color.White;
            this.btn_Info.Location = new System.Drawing.Point(12, 12);
            this.btn_Info.Name = "btn_Info";
            this.btn_Info.Size = new System.Drawing.Size(120, 27);
            this.btn_Info.TabIndex = 0;
            this.btn_Info.Text = "엑셀파일불러오기";
            this.btn_Info.UseVisualStyleBackColor = false;
            this.btn_Info.Click += new System.EventHandler(this.btn_Info_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // btn_Test
            // 
            this.btn_Test.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Test.FlatAppearance.BorderSize = 0;
            this.btn_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Test.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Test.ForeColor = System.Drawing.Color.White;
            this.btn_Test.Location = new System.Drawing.Point(145, 12);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(109, 27);
            this.btn_Test.TabIndex = 2;
            this.btn_Test.Text = "데이터 검증";
            this.btn_Test.UseVisualStyleBackColor = false;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "데이터 검증실패";
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
            this.panel1.Location = new System.Drawing.Point(12, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 141);
            this.panel1.TabIndex = 10;
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
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Close.FlatAppearance.BorderSize = 0;
            this.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Close.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Close.ForeColor = System.Drawing.Color.White;
            this.btn_Close.Location = new System.Drawing.Point(135, 188);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 27);
            this.btn_Close.TabIndex = 11;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToDeleteRows = false;
            this.newDGV1.AllowUserToResizeColumns = false;
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            this.newDGV1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.newDGV1.BackgroundColor = System.Drawing.Color.White;
            this.newDGV1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.newDGV1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newDGV1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnIdx,
            this.ColumnDate,
            this.ColumnItem,
            this.ColumnCustomer,
            this.ColumnBizNo,
            this.ColumnStart,
            this.ColumnStop,
            this.ColumnTarget,
            this.ColumnCarNo,
            this.ColumnCode,
            this.ColumnClient,
            this.ColumnCarType,
            this.ColumnClientPrice,
            this.ColumnPrice,
            this.ColumnAddedPrice,
            this.ColumnAddedCharge,
            this.ColumnRequestPrice,
            this.ColumnEndPrice,
            this.ColumnLastPrice,
            this.ColumnEndClient,
            this.ColumnMemo,
            this.ColumnType1,
            this.ColumnError});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle18;
            this.newDGV1.Dock = System.Windows.Forms.DockStyle.Right;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(273, 0);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.MultiSelect = false;
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.ReadOnly = true;
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.Size = new System.Drawing.Size(727, 223);
            this.newDGV1.TabIndex = 1;
            // 
            // ColumnIdx
            // 
            this.ColumnIdx.HeaderText = "IDX";
            this.ColumnIdx.Name = "ColumnIdx";
            this.ColumnIdx.ReadOnly = true;
            this.ColumnIdx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnIdx.Width = 50;
            // 
            // ColumnDate
            // 
            this.ColumnDate.HeaderText = "일자";
            this.ColumnDate.Name = "ColumnDate";
            this.ColumnDate.ReadOnly = true;
            this.ColumnDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnItem
            // 
            this.ColumnItem.HeaderText = "항목";
            this.ColumnItem.Name = "ColumnItem";
            this.ColumnItem.ReadOnly = true;
            this.ColumnItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnCustomer
            // 
            this.ColumnCustomer.HeaderText = "청구처";
            this.ColumnCustomer.Name = "ColumnCustomer";
            this.ColumnCustomer.ReadOnly = true;
            this.ColumnCustomer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnBizNo
            // 
            this.ColumnBizNo.HeaderText = "사업자등록번호";
            this.ColumnBizNo.Name = "ColumnBizNo";
            this.ColumnBizNo.ReadOnly = true;
            this.ColumnBizNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnBizNo.Width = 120;
            // 
            // ColumnStart
            // 
            this.ColumnStart.HeaderText = "상차지";
            this.ColumnStart.Name = "ColumnStart";
            this.ColumnStart.ReadOnly = true;
            this.ColumnStart.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnStop
            // 
            this.ColumnStop.HeaderText = "지역";
            this.ColumnStop.Name = "ColumnStop";
            this.ColumnStop.ReadOnly = true;
            this.ColumnStop.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnTarget
            // 
            this.ColumnTarget.HeaderText = "입고업체명";
            this.ColumnTarget.Name = "ColumnTarget";
            this.ColumnTarget.ReadOnly = true;
            this.ColumnTarget.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnCarNo
            // 
            this.ColumnCarNo.HeaderText = "차량번호";
            this.ColumnCarNo.Name = "ColumnCarNo";
            this.ColumnCarNo.ReadOnly = true;
            this.ColumnCarNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnCode
            // 
            this.ColumnCode.HeaderText = "코드";
            this.ColumnCode.Name = "ColumnCode";
            this.ColumnCode.ReadOnly = true;
            this.ColumnCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnCode.Width = 80;
            // 
            // ColumnClient
            // 
            this.ColumnClient.HeaderText = "협력업체";
            this.ColumnClient.Name = "ColumnClient";
            this.ColumnClient.ReadOnly = true;
            this.ColumnClient.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnCarType
            // 
            this.ColumnCarType.HeaderText = "차종";
            this.ColumnCarType.Name = "ColumnCarType";
            this.ColumnCarType.ReadOnly = true;
            this.ColumnCarType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnCarType.Width = 80;
            // 
            // ColumnClientPrice
            // 
            this.ColumnClientPrice.HeaderText = "운임";
            this.ColumnClientPrice.Name = "ColumnClientPrice";
            this.ColumnClientPrice.ReadOnly = true;
            this.ColumnClientPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnClientPrice.Width = 120;
            // 
            // ColumnPrice
            // 
            this.ColumnPrice.HeaderText = "운송료";
            this.ColumnPrice.Name = "ColumnPrice";
            this.ColumnPrice.ReadOnly = true;
            this.ColumnPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnPrice.Width = 120;
            // 
            // ColumnAddedPrice
            // 
            this.ColumnAddedPrice.HeaderText = "추가청구액";
            this.ColumnAddedPrice.Name = "ColumnAddedPrice";
            this.ColumnAddedPrice.ReadOnly = true;
            this.ColumnAddedPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnAddedPrice.Width = 120;
            // 
            // ColumnAddedCharge
            // 
            this.ColumnAddedCharge.HeaderText = "추가지불액";
            this.ColumnAddedCharge.Name = "ColumnAddedCharge";
            this.ColumnAddedCharge.ReadOnly = true;
            this.ColumnAddedCharge.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnAddedCharge.Width = 120;
            // 
            // ColumnRequestPrice
            // 
            this.ColumnRequestPrice.HeaderText = "청구금액";
            this.ColumnRequestPrice.Name = "ColumnRequestPrice";
            this.ColumnRequestPrice.ReadOnly = true;
            this.ColumnRequestPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnRequestPrice.Width = 120;
            // 
            // ColumnEndPrice
            // 
            this.ColumnEndPrice.HeaderText = "지급금액";
            this.ColumnEndPrice.Name = "ColumnEndPrice";
            this.ColumnEndPrice.ReadOnly = true;
            this.ColumnEndPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnEndPrice.Width = 120;
            // 
            // ColumnLastPrice
            // 
            this.ColumnLastPrice.HeaderText = "차액";
            this.ColumnLastPrice.Name = "ColumnLastPrice";
            this.ColumnLastPrice.ReadOnly = true;
            this.ColumnLastPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnLastPrice.Width = 120;
            // 
            // ColumnEndClient
            // 
            this.ColumnEndClient.HeaderText = "지급처";
            this.ColumnEndClient.Name = "ColumnEndClient";
            this.ColumnEndClient.ReadOnly = true;
            this.ColumnEndClient.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMemo
            // 
            this.ColumnMemo.HeaderText = "비고";
            this.ColumnMemo.Name = "ColumnMemo";
            this.ColumnMemo.ReadOnly = true;
            this.ColumnMemo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnType1
            // 
            this.ColumnType1.HeaderText = "내동,냉장";
            this.ColumnType1.Name = "ColumnType1";
            this.ColumnType1.ReadOnly = true;
            this.ColumnType1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnError
            // 
            this.ColumnError.HeaderText = "ERROR";
            this.ColumnError.Name = "ColumnError";
            this.ColumnError.ReadOnly = true;
            this.ColumnError.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btn_Update
            // 
            this.btn_Update.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_Update.Enabled = false;
            this.btn_Update.FlatAppearance.BorderSize = 0;
            this.btn_Update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Update.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Update.ForeColor = System.Drawing.Color.White;
            this.btn_Update.Location = new System.Drawing.Point(50, 188);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(79, 27);
            this.btn_Update.TabIndex = 12;
            this.btn_Update.Text = "일괄등록";
            this.btn_Update.UseVisualStyleBackColor = false;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // fpiS_CONTTableAdapter
            // 
            this.fpiS_CONTTableAdapter.ClearBeforeFill = true;
            // 
            // fpisDataSet
            // 
            this.fpisDataSet.DataSetName = "FPISDataSet";
            this.fpisDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // PnProgress
            // 
            this.PnProgress.Controls.Add(this.lblProgress);
            this.PnProgress.Controls.Add(this.label8);
            this.PnProgress.Controls.Add(this.BarProgress);
            this.PnProgress.Location = new System.Drawing.Point(355, 84);
            this.PnProgress.Name = "PnProgress";
            this.PnProgress.Size = new System.Drawing.Size(337, 54);
            this.PnProgress.TabIndex = 13;
            this.PnProgress.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Location = new System.Drawing.Point(234, 12);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(100, 12);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "0/0";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "잠시만 기다려주세요";
            // 
            // BarProgress
            // 
            this.BarProgress.Location = new System.Drawing.Point(3, 27);
            this.BarProgress.Name = "BarProgress";
            this.BarProgress.Size = new System.Drawing.Size(331, 23);
            this.BarProgress.TabIndex = 0;
            // 
            // EXCELINSERT2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1000, 223);
            this.Controls.Add(this.PnProgress);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.newDGV1);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_Test);
            this.Controls.Add(this.btn_Info);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EXCELINSERT2";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "운송완료 건 일괄등록(외부양식)";
            this.Load += new System.EventHandler(this.EXCELINSERT_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpisDataSet)).EndInit();
            this.PnProgress.ResumeLayout(false);
            this.PnProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Info;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private NewDGV newDGV1;
        private System.Windows.Forms.Button btn_Test;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Update;
        private DataSets.FPISDataSetTableAdapters.FPIS_CONTTableAdapter fpiS_CONTTableAdapter;
        private DataSets.FPISDataSet fpisDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIdx;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBizNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStop;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCarNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCarType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClientPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAddedPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAddedCharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRequestPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEndPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLastPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEndClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMemo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnError;
        private System.Windows.Forms.Panel PnProgress;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar BarProgress;
    }
}