namespace mycalltruck.Admin
{
    partial class FrmMN0216_ORDERITEM_ADD
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
            this.label18 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ItemCode = new System.Windows.Forms.TextBox();
            this.ItemName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_DriverExcel = new System.Windows.Forms.Button();
            this.btnExcelImport = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.CreateDate = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Remark = new System.Windows.Forms.TextBox();
            this.btnAddClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.orderItemDataSet = new mycalltruck.Admin.DataSets.OrderItemDataSet();
            this.orderItemTableAdapter = new mycalltruck.Admin.DataSets.OrderItemDataSetTableAdapters.OrderItemsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orderItemDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label18.Location = new System.Drawing.Point(5, 5);
            this.label18.Margin = new System.Windows.Forms.Padding(4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 25);
            this.label18.TabIndex = 34;
            this.label18.Text = "코드";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(5, 104);
            this.label8.Margin = new System.Windows.Forms.Padding(4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "비고";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ItemCode
            // 
            this.ItemCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ItemCode.Enabled = false;
            this.ItemCode.Location = new System.Drawing.Point(92, 4);
            this.ItemCode.Name = "ItemCode";
            this.ItemCode.ReadOnly = true;
            this.ItemCode.Size = new System.Drawing.Size(154, 25);
            this.ItemCode.TabIndex = 35;
            this.ItemCode.TabStop = false;
            // 
            // ItemName
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.ItemName, 2);
            this.ItemName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemName.Location = new System.Drawing.Point(92, 37);
            this.ItemName.MaxLength = 30;
            this.ItemName.Name = "ItemName";
            this.ItemName.Size = new System.Drawing.Size(274, 25);
            this.ItemName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(5, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 25);
            this.label5.TabIndex = 36;
            this.label5.Text = "품목명";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(5, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 25);
            this.label3.TabIndex = 39;
            this.label3.Text = "등록일자";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_DriverExcel
            // 
            this.btn_DriverExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btn_DriverExcel.FlatAppearance.BorderSize = 0;
            this.btn_DriverExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DriverExcel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btn_DriverExcel.ForeColor = System.Drawing.Color.White;
            this.btn_DriverExcel.Location = new System.Drawing.Point(116, 153);
            this.btn_DriverExcel.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btn_DriverExcel.Name = "btn_DriverExcel";
            this.btn_DriverExcel.Size = new System.Drawing.Size(96, 27);
            this.btn_DriverExcel.TabIndex = 60;
            this.btn_DriverExcel.TabStop = false;
            this.btn_DriverExcel.Text = "엑셀양식";
            this.btn_DriverExcel.UseVisualStyleBackColor = false;
            this.btn_DriverExcel.Visible = false;
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnExcelImport.FlatAppearance.BorderSize = 0;
            this.btnExcelImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcelImport.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnExcelImport.ForeColor = System.Drawing.Color.White;
            this.btnExcelImport.Location = new System.Drawing.Point(1, 153);
            this.btnExcelImport.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(110, 27);
            this.btnExcelImport.TabIndex = 59;
            this.btnExcelImport.TabStop = false;
            this.btnExcelImport.Text = "엑셀일괄등록";
            this.btnExcelImport.UseVisualStyleBackColor = false;
            this.btnExcelImport.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(313, 153);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 27);
            this.btnAdd.TabIndex = 56;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "등록후추가(F5)";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(516, 153);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 27);
            this.btnClose.TabIndex = 57;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CreateDate
            // 
            this.CreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CreateDate.Location = new System.Drawing.Point(92, 70);
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.ReadOnly = true;
            this.CreateDate.Size = new System.Drawing.Size(154, 25);
            this.CreateDate.TabIndex = 27;
            this.CreateDate.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ItemCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.ItemName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.CreateDate, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.Remark, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("맑은 고딕", 10F);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(616, 143);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // Remark
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.Remark, 2);
            this.Remark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Remark.Location = new System.Drawing.Point(92, 103);
            this.Remark.MaxLength = 30;
            this.Remark.Name = "Remark";
            this.Remark.Size = new System.Drawing.Size(274, 25);
            this.Remark.TabIndex = 40;
            // 
            // btnAddClose
            // 
            this.btnAddClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnAddClose.FlatAppearance.BorderSize = 0;
            this.btnAddClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddClose.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddClose.ForeColor = System.Drawing.Color.White;
            this.btnAddClose.Location = new System.Drawing.Point(416, 153);
            this.btnAddClose.Margin = new System.Windows.Forms.Padding(7, 5, 0, 5);
            this.btnAddClose.Name = "btnAddClose";
            this.btnAddClose.Size = new System.Drawing.Size(96, 27);
            this.btnAddClose.TabIndex = 58;
            this.btnAddClose.TabStop = false;
            this.btnAddClose.Text = "등록후닫기(F6)";
            this.btnAddClose.UseVisualStyleBackColor = false;
            this.btnAddClose.Click += new System.EventHandler(this.btnAddClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(618, 145);
            this.groupBox2.TabIndex = 55;
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(209, 57);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 62;
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
            // orderItemDataSet
            // 
            this.orderItemDataSet.DataSetName = "OrderItemDataSet";
            this.orderItemDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // orderItemTableAdapter
            // 
            this.orderItemTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMN0216_ORDERITEM_ADD
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(618, 189);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.btn_DriverExcel);
            this.Controls.Add(this.btnExcelImport);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddClose);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmMN0216_ORDERITEM_ADD";
            this.Text = "품목코드 추가";
            this.Load += new System.EventHandler(this.FrmMN0216_ORDERITEM_ADD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.pnProgress.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.orderItemDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.Button btn_DriverExcel;
        private System.Windows.Forms.Button btnExcelImport;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnAddClose;
        private System.Windows.Forms.Panel groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ItemCode;
        private System.Windows.Forms.TextBox ItemName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CreateDate;
        private System.Windows.Forms.TextBox Remark;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
        private DataSets.OrderItemDataSet orderItemDataSet;
        private DataSets.OrderItemDataSetTableAdapters.OrderItemsTableAdapter orderItemTableAdapter;
    }
}