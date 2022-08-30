namespace mycalltruck.Admin
{
    partial class FrmMN0303_CARGOFPIS_Add_Default
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_CL_P_TEL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.txt_CONT_DEPOSIT = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dtp_CONT_TO = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.dtp_CONT_FROM = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btn_Customer = new System.Windows.Forms.Button();
            this.txt_CL_COMP_NM = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_CL_COMP_BSNS_NUM = new System.Windows.Forms.TextBox();
            this.txt_CreateDate = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.cmDataSet = new mycalltruck.Admin.CMDataSet();
            this.fpiS_CONTTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.FPIS_CONTTableAdapter();
            this.btn_DriverExcel = new System.Windows.Forms.Button();
            this.btnExcelImport = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.tableLayoutPanel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(749, 201);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.txt_CL_P_TEL, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.panel12, 1, 4);
            this.tableLayoutPanel5.Controls.Add(this.panel7, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.label14, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.label11, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.panel8, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.txt_CL_COMP_BSNS_NUM, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.txt_CreateDate, 1, 5);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 7;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(749, 201);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // txt_CL_P_TEL
            // 
            this.txt_CL_P_TEL.Location = new System.Drawing.Point(103, 63);
            this.txt_CL_P_TEL.Name = "txt_CL_P_TEL";
            this.txt_CL_P_TEL.ReadOnly = true;
            this.txt_CL_P_TEL.Size = new System.Drawing.Size(141, 21);
            this.txt_CL_P_TEL.TabIndex = 48;
            this.txt_CL_P_TEL.TabStop = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(3, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 30);
            this.label1.TabIndex = 47;
            this.label1.Text = "연락처 :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(3, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 30);
            this.label9.TabIndex = 45;
            this.label9.Text = "계약금액 :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.label19);
            this.panel12.Controls.Add(this.txt_CONT_DEPOSIT);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(100, 120);
            this.panel12.Margin = new System.Windows.Forms.Padding(0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(649, 30);
            this.panel12.TabIndex = 44;
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(163, 9);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 12);
            this.label19.TabIndex = 48;
            this.label19.Text = "원";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_CONT_DEPOSIT
            // 
            this.txt_CONT_DEPOSIT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CONT_DEPOSIT.Location = new System.Drawing.Point(3, 3);
            this.txt_CONT_DEPOSIT.MaxLength = 11;
            this.txt_CONT_DEPOSIT.Name = "txt_CONT_DEPOSIT";
            this.txt_CONT_DEPOSIT.Size = new System.Drawing.Size(154, 21);
            this.txt_CONT_DEPOSIT.TabIndex = 3;
            this.txt_CONT_DEPOSIT.Enter += new System.EventHandler(this.txt_CONT_DEPOSIT_Enter);
            this.txt_CONT_DEPOSIT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_CONT_DEPOSIT_KeyPress);
            this.txt_CONT_DEPOSIT.Leave += new System.EventHandler(this.txt_CONT_DEPOSIT_Leave);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.dtp_CONT_TO);
            this.panel7.Controls.Add(this.label8);
            this.panel7.Controls.Add(this.dtp_CONT_FROM);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(100, 90);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(649, 30);
            this.panel7.TabIndex = 1;
            // 
            // dtp_CONT_TO
            // 
            this.dtp_CONT_TO.CustomFormat = "yyyy/MM/dd";
            this.dtp_CONT_TO.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_CONT_TO.Location = new System.Drawing.Point(135, 4);
            this.dtp_CONT_TO.Name = "dtp_CONT_TO";
            this.dtp_CONT_TO.Size = new System.Drawing.Size(117, 21);
            this.dtp_CONT_TO.TabIndex = 2;
            this.dtp_CONT_TO.TabStop = false;
            this.dtp_CONT_TO.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.dtp_CONT_TO.Enter += new System.EventHandler(this.Control_Enter);
            this.dtp_CONT_TO.Leave += new System.EventHandler(this.dtp_CONT_FROM_Leave);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(120, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 12);
            this.label8.TabIndex = 49;
            this.label8.Text = "~";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtp_CONT_FROM
            // 
            this.dtp_CONT_FROM.CustomFormat = "yyyy/MM/dd";
            this.dtp_CONT_FROM.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_CONT_FROM.Location = new System.Drawing.Point(4, 4);
            this.dtp_CONT_FROM.Name = "dtp_CONT_FROM";
            this.dtp_CONT_FROM.Size = new System.Drawing.Size(117, 21);
            this.dtp_CONT_FROM.TabIndex = 1;
            this.dtp_CONT_FROM.TabStop = false;
            this.dtp_CONT_FROM.Value = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.dtp_CONT_FROM.Enter += new System.EventHandler(this.Control_Enter);
            this.dtp_CONT_FROM.Leave += new System.EventHandler(this.dtp_CONT_FROM_Leave);
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.ForeColor = System.Drawing.Color.Blue;
            this.label14.Location = new System.Drawing.Point(3, 90);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(94, 30);
            this.label14.TabIndex = 39;
            this.label14.Text = "계약기간 :";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 150);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 30);
            this.label11.TabIndex = 15;
            this.label11.Text = "등록일자 :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btn_Customer);
            this.panel8.Controls.Add(this.txt_CL_COMP_NM);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(100, 0);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(649, 30);
            this.panel8.TabIndex = 1;
            this.panel8.TabStop = true;
            // 
            // btn_Customer
            // 
            this.btn_Customer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Customer.Location = new System.Drawing.Point(144, 4);
            this.btn_Customer.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btn_Customer.Name = "btn_Customer";
            this.btn_Customer.Size = new System.Drawing.Size(45, 23);
            this.btn_Customer.TabIndex = 10;
            this.btn_Customer.TabStop = false;
            this.btn_Customer.Text = "검색";
            this.btn_Customer.UseVisualStyleBackColor = true;
            this.btn_Customer.Click += new System.EventHandler(this.btn_Customer_Click);
            // 
            // txt_CL_COMP_NM
            // 
            this.txt_CL_COMP_NM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CL_COMP_NM.Location = new System.Drawing.Point(3, 4);
            this.txt_CL_COMP_NM.MaxLength = 25;
            this.txt_CL_COMP_NM.Name = "txt_CL_COMP_NM";
            this.txt_CL_COMP_NM.ReadOnly = true;
            this.txt_CL_COMP_NM.Size = new System.Drawing.Size(141, 21);
            this.txt_CL_COMP_NM.TabIndex = 1;
            this.txt_CL_COMP_NM.TabStop = false;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(3, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 30);
            this.label6.TabIndex = 6;
            this.label6.Text = "사업자번호 :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "상호/성명 :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_CL_COMP_BSNS_NUM
            // 
            this.txt_CL_COMP_BSNS_NUM.Location = new System.Drawing.Point(103, 33);
            this.txt_CL_COMP_BSNS_NUM.Name = "txt_CL_COMP_BSNS_NUM";
            this.txt_CL_COMP_BSNS_NUM.ReadOnly = true;
            this.txt_CL_COMP_BSNS_NUM.Size = new System.Drawing.Size(141, 21);
            this.txt_CL_COMP_BSNS_NUM.TabIndex = 2;
            this.txt_CL_COMP_BSNS_NUM.TabStop = false;
            // 
            // txt_CreateDate
            // 
            this.txt_CreateDate.Location = new System.Drawing.Point(103, 153);
            this.txt_CreateDate.MaxLength = 10;
            this.txt_CreateDate.Name = "txt_CreateDate";
            this.txt_CreateDate.ReadOnly = true;
            this.txt_CreateDate.Size = new System.Drawing.Size(154, 21);
            this.txt_CreateDate.TabIndex = 46;
            this.txt_CreateDate.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.Location = new System.Drawing.Point(651, 209);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddClose.Location = new System.Drawing.Point(551, 209);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 23);
            this.btnAddClose.TabIndex = 11;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기(F6)";
            this.btnAddClose.UseVisualStyleBackColor = true;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(448, 209);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 23);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "등록후추가(F5)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // cmDataSet
            // 
            this.cmDataSet.DataSetName = "CMDataSet";
            this.cmDataSet.EnforceConstraints = false;
            this.cmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fpiS_CONTTableAdapter
            // 
            this.fpiS_CONTTableAdapter.ClearBeforeFill = true;
            // 
            // btn_DriverExcel
            // 
            this.btn_DriverExcel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_DriverExcel.Location = new System.Drawing.Point(104, 209);
            this.btn_DriverExcel.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btn_DriverExcel.Name = "btn_DriverExcel";
            this.btn_DriverExcel.Size = new System.Drawing.Size(96, 23);
            this.btn_DriverExcel.TabIndex = 48;
            this.btn_DriverExcel.TabStop = false;
            this.btn_DriverExcel.Text = "엑셀양식";
            this.btn_DriverExcel.UseVisualStyleBackColor = true;
            this.btn_DriverExcel.Click += new System.EventHandler(this.btn_DriverExcel_Click);
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExcelImport.Location = new System.Drawing.Point(5, 209);
            this.btnExcelImport.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(96, 23);
            this.btnExcelImport.TabIndex = 47;
            this.btnExcelImport.TabStop = false;
            this.btnExcelImport.Text = "엑셀 일괄등록";
            this.btnExcelImport.UseVisualStyleBackColor = true;
            this.btnExcelImport.Click += new System.EventHandler(this.btnExcelImport_Click);
            // 
            // FrmMN0303_CARGOFPIS_Add_Default
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(749, 236);
            this.Controls.Add(this.btn_DriverExcel);
            this.Controls.Add(this.btnExcelImport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "FrmMN0303_CARGOFPIS_Add_Default";
            this.Text = "의뢰자/계약정보 추가";
            this.Load += new System.EventHandler(this.FrmMN0303_CARGOFPIS_Add_Default_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btn_Customer;
        private System.Windows.Forms.TextBox txt_CL_COMP_NM;
        private System.Windows.Forms.TextBox txt_CL_COMP_BSNS_NUM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.DateTimePicker dtp_CONT_TO;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtp_CONT_FROM;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txt_CONT_DEPOSIT;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_CreateDate;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private CMDataSet cmDataSet;
        private CMDataSetTableAdapters.FPIS_CONTTableAdapter fpiS_CONTTableAdapter;
        private System.Windows.Forms.Button btn_DriverExcel;
        private System.Windows.Forms.Button btnExcelImport;
        private System.Windows.Forms.TextBox txt_CL_P_TEL;
        private System.Windows.Forms.Label label1;
    }
}