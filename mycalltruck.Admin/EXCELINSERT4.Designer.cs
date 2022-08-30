namespace mycalltruck.Admin
{
    partial class EXCELINSERT4
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.btn_Info = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_Close = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkFilterError = new System.Windows.Forms.CheckBox();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblErrorCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblValidCount = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.btn_Update = new System.Windows.Forms.Button();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCarNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBizN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCarType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCarSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStopTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartState = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStartCity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStartStreet = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStopState = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStopCity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStopStreet = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colClientPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colETC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btn_Close);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btn_Update);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12);
            this.panel1.Size = new System.Drawing.Size(272, 368);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbCustomer);
            this.panel3.Controls.Add(this.btn_Info);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(15, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(242, 89);
            this.panel3.TabIndex = 18;
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(47, 31);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(190, 20);
            this.cmbCustomer.TabIndex = 19;
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbCustomer_SelectedIndexChanged);
            // 
            // btn_Info
            // 
            this.btn_Info.Location = new System.Drawing.Point(47, 57);
            this.btn_Info.Name = "btn_Info";
            this.btn_Info.Size = new System.Drawing.Size(111, 23);
            this.btn_Info.TabIndex = 13;
            this.btn_Info.Text = "엑셀파일불러오기";
            this.btn_Info.UseVisualStyleBackColor = true;
            this.btn_Info.Click += new System.EventHandler(this.btn_Info_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "화주 :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(4, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "탁송 업체 선택";
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(178, 326);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(79, 23);
            this.btn_Close.TabIndex = 16;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkFilterError);
            this.panel2.Controls.Add(this.lblErrorMessage);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.lblErrorCount);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lblValidCount);
            this.panel2.Controls.Add(this.lblTotalCount);
            this.panel2.Location = new System.Drawing.Point(15, 111);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(242, 147);
            this.panel2.TabIndex = 15;
            // 
            // chkFilterError
            // 
            this.chkFilterError.AutoSize = true;
            this.chkFilterError.Location = new System.Drawing.Point(175, 81);
            this.chkFilterError.Name = "chkFilterError";
            this.chkFilterError.Size = new System.Drawing.Size(48, 16);
            this.chkFilterError.TabIndex = 11;
            this.chkFilterError.Text = "보기";
            this.chkFilterError.UseVisualStyleBackColor = true;
            this.chkFilterError.CheckedChanged += new System.EventHandler(this.chkFilterError_CheckedChanged);
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Crimson;
            this.lblErrorMessage.Location = new System.Drawing.Point(4, 110);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(233, 24);
            this.lblErrorMessage.TabIndex = 10;
            this.lblErrorMessage.Text = "** 데이터 검증 실패를 제외한 성공 건만 등록됩니다.";
            this.lblErrorMessage.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "의뢰건수";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "데이터 검증실패";
            // 
            // lblErrorCount
            // 
            this.lblErrorCount.AutoSize = true;
            this.lblErrorCount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblErrorCount.ForeColor = System.Drawing.Color.Red;
            this.lblErrorCount.Location = new System.Drawing.Point(142, 82);
            this.lblErrorCount.Name = "lblErrorCount";
            this.lblErrorCount.Size = new System.Drawing.Size(12, 12);
            this.lblErrorCount.TabIndex = 8;
            this.lblErrorCount.Text = "0";
            this.lblErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "데이터 검증성공";
            // 
            // lblValidCount
            // 
            this.lblValidCount.AutoSize = true;
            this.lblValidCount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblValidCount.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblValidCount.Location = new System.Drawing.Point(142, 46);
            this.lblValidCount.Name = "lblValidCount";
            this.lblValidCount.Size = new System.Drawing.Size(12, 12);
            this.lblValidCount.TabIndex = 7;
            this.lblValidCount.Text = "0";
            this.lblValidCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTotalCount.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblTotalCount.Location = new System.Drawing.Point(142, 10);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(12, 12);
            this.lblTotalCount.TabIndex = 6;
            this.lblTotalCount.Text = "0";
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_Update
            // 
            this.btn_Update.Enabled = false;
            this.btn_Update.Location = new System.Drawing.Point(93, 326);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(79, 23);
            this.btn_Update.TabIndex = 17;
            this.btn_Update.Text = "일괄등록";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colCarNo,
            this.colName,
            this.colBizN,
            this.colPhoneNo,
            this.colCarType,
            this.colCarSize,
            this.colStartTime,
            this.colStopTime,
            this.colStartState,
            this.colStartCity,
            this.colStartStreet,
            this.colStopState,
            this.colStopCity,
            this.colStopStreet,
            this.colClientPrice,
            this.colPrice,
            this.colETC,
            this.colError});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle10;
            this.newDGV1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(272, 0);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.Size = new System.Drawing.Size(728, 368);
            this.newDGV1.TabIndex = 3;
            this.newDGV1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.newDGV1_CellEndEdit);
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            this.newDGV1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.newDGV1_DataError);
            this.newDGV1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.newDGV1_EditingControlShowing);
            // 
            // colNumber
            // 
            this.colNumber.HeaderText = "번호";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNumber.Width = 55;
            // 
            // colCarNo
            // 
            this.colCarNo.DataPropertyName = "CarNo";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colCarNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.colCarNo.HeaderText = "차량번호";
            this.colCarNo.Name = "colCarNo";
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "기사명";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colBizN
            // 
            this.colBizN.DataPropertyName = "BizNo";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colBizN.DefaultCellStyle = dataGridViewCellStyle4;
            this.colBizN.HeaderText = "사업자번호";
            this.colBizN.Name = "colBizN";
            this.colBizN.ReadOnly = true;
            // 
            // colPhoneNo
            // 
            this.colPhoneNo.DataPropertyName = "PhoneNo";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPhoneNo.DefaultCellStyle = dataGridViewCellStyle5;
            this.colPhoneNo.HeaderText = "핸드폰번호";
            this.colPhoneNo.Name = "colPhoneNo";
            this.colPhoneNo.ReadOnly = true;
            // 
            // colCarType
            // 
            this.colCarType.DataPropertyName = "CarType";
            this.colCarType.HeaderText = "차종";
            this.colCarType.Name = "colCarType";
            this.colCarType.ReadOnly = true;
            this.colCarType.Width = 65;
            // 
            // colCarSize
            // 
            this.colCarSize.DataPropertyName = "CarSize";
            this.colCarSize.HeaderText = "톤수";
            this.colCarSize.Name = "colCarSize";
            this.colCarSize.ReadOnly = true;
            this.colCarSize.Width = 65;
            // 
            // colStartTime
            // 
            this.colStartTime.DataPropertyName = "StartTime";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "yyyy-MM-dd";
            this.colStartTime.DefaultCellStyle = dataGridViewCellStyle6;
            this.colStartTime.HeaderText = "출발일";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.ReadOnly = true;
            // 
            // colStopTime
            // 
            this.colStopTime.DataPropertyName = "StopTime";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "yyyy-MM-dd";
            this.colStopTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.colStopTime.HeaderText = "도착일";
            this.colStopTime.Name = "colStopTime";
            this.colStopTime.ReadOnly = true;
            // 
            // colStartState
            // 
            this.colStartState.DataPropertyName = "StartState";
            this.colStartState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colStartState.HeaderText = "출발지";
            this.colStartState.Name = "colStartState";
            // 
            // colStartCity
            // 
            this.colStartCity.DataPropertyName = "StartCity";
            this.colStartCity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colStartCity.HeaderText = "출발지";
            this.colStartCity.Name = "colStartCity";
            // 
            // colStartStreet
            // 
            this.colStartStreet.DataPropertyName = "StartStreet";
            this.colStartStreet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colStartStreet.HeaderText = "출발지";
            this.colStartStreet.Name = "colStartStreet";
            this.colStartStreet.Visible = false;
            // 
            // colStopState
            // 
            this.colStopState.DataPropertyName = "StopState";
            this.colStopState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colStopState.HeaderText = "도착지";
            this.colStopState.Name = "colStopState";
            // 
            // colStopCity
            // 
            this.colStopCity.DataPropertyName = "StopCity";
            this.colStopCity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colStopCity.HeaderText = "도착지";
            this.colStopCity.Name = "colStopCity";
            // 
            // colStopStreet
            // 
            this.colStopStreet.DataPropertyName = "StopStreet";
            this.colStopStreet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colStopStreet.HeaderText = "도착지";
            this.colStopStreet.Name = "colStopStreet";
            this.colStopStreet.Visible = false;
            // 
            // colClientPrice
            // 
            this.colClientPrice.DataPropertyName = "ClientPrice";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            this.colClientPrice.DefaultCellStyle = dataGridViewCellStyle8;
            this.colClientPrice.HeaderText = "화주운송료";
            this.colClientPrice.Name = "colClientPrice";
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "Price";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            this.colPrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.colPrice.HeaderText = "배차운송료";
            this.colPrice.Name = "colPrice";
            // 
            // colETC
            // 
            this.colETC.DataPropertyName = "ETC";
            this.colETC.HeaderText = "특이사항";
            this.colETC.Name = "colETC";
            // 
            // colError
            // 
            this.colError.DataPropertyName = "Error";
            this.colError.HeaderText = "오류";
            this.colError.Name = "colError";
            this.colError.ReadOnly = true;
            // 
            // EXCELINSERT4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1000, 368);
            this.Controls.Add(this.newDGV1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EXCELINSERT4";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "운송완료 건 일괄등록";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblErrorMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblErrorCount;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lblValidCount;
        public System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Button btn_Info;
        private NewDGV newDGV1;
        private System.Windows.Forms.CheckBox chkFilterError;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCarNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBizN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoneNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCarType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCarSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStopTime;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStartState;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStartCity;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStartStreet;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStopState;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStopCity;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStopStreet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colETC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colError;
    }
}