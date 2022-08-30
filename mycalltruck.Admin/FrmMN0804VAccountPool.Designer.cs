namespace mycalltruck.Admin
{
    partial class FrmMN0804VAccountPool
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle61 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle62 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle70 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle63 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle64 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle65 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle66 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle67 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle68 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle69 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Delete = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.SaveForm = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.ImportExcel = new System.Windows.Forms.Button();
            this.AllSelect = new System.Windows.Forms.CheckBox();
            this.mDataGridView = new mycalltruck.Admin.NewDGV();
            this.ColumnSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVAccountNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVBankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClientCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDriverCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDriverName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Delete);
            this.panel1.Controls.Add(this.Clear);
            this.panel1.Controls.Add(this.SaveForm);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.ImportExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 43);
            this.panel1.TabIndex = 1;
            // 
            // Delete
            // 
            this.Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Delete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.Delete.FlatAppearance.BorderSize = 0;
            this.Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Delete.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Delete.ForeColor = System.Drawing.Color.White;
            this.Delete.Location = new System.Drawing.Point(632, 9);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(75, 27);
            this.Delete.TabIndex = 51;
            this.Delete.Text = "삭제";
            this.Delete.UseVisualStyleBackColor = false;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Clear
            // 
            this.Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Clear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.Clear.FlatAppearance.BorderSize = 0;
            this.Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Clear.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Clear.ForeColor = System.Drawing.Color.White;
            this.Clear.Location = new System.Drawing.Point(713, 9);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(75, 27);
            this.Clear.TabIndex = 50;
            this.Clear.Text = "해제";
            this.Clear.UseVisualStyleBackColor = false;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // SaveForm
            // 
            this.SaveForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.SaveForm.FlatAppearance.BorderSize = 0;
            this.SaveForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveForm.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SaveForm.ForeColor = System.Drawing.Color.White;
            this.SaveForm.Location = new System.Drawing.Point(794, 9);
            this.SaveForm.Name = "SaveForm";
            this.SaveForm.Size = new System.Drawing.Size(100, 27);
            this.SaveForm.TabIndex = 49;
            this.SaveForm.Text = "양식 내려받기";
            this.SaveForm.UseVisualStyleBackColor = false;
            this.SaveForm.Click += new System.EventHandler(this.SaveForm_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(4, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 17);
            this.label11.TabIndex = 48;
            this.label11.Text = "가상계좌 관리";
            // 
            // ImportExcel
            // 
            this.ImportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.ImportExcel.FlatAppearance.BorderSize = 0;
            this.ImportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportExcel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ImportExcel.ForeColor = System.Drawing.Color.White;
            this.ImportExcel.Location = new System.Drawing.Point(900, 9);
            this.ImportExcel.Name = "ImportExcel";
            this.ImportExcel.Size = new System.Drawing.Size(100, 27);
            this.ImportExcel.TabIndex = 43;
            this.ImportExcel.Text = "엑셀 불러오기";
            this.ImportExcel.UseVisualStyleBackColor = false;
            this.ImportExcel.Click += new System.EventHandler(this.ImportExcel_Click);
            // 
            // AllSelect
            // 
            this.AllSelect.AutoSize = true;
            this.AllSelect.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.AllSelect.Location = new System.Drawing.Point(12, 44);
            this.AllSelect.Name = "AllSelect";
            this.AllSelect.Size = new System.Drawing.Size(50, 19);
            this.AllSelect.TabIndex = 3;
            this.AllSelect.Text = "선택";
            this.AllSelect.UseVisualStyleBackColor = false;
            this.AllSelect.Click += new System.EventHandler(this.AllSelect_Click);
            // 
            // mDataGridView
            // 
            this.mDataGridView.AllowUserToAddRows = false;
            this.mDataGridView.AllowUserToDeleteRows = false;
            this.mDataGridView.AllowUserToOrderColumns = true;
            this.mDataGridView.AllowUserToResizeColumns = false;
            this.mDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle61.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle61.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle61.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle61.SelectionForeColor = System.Drawing.Color.White;
            this.mDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle61;
            this.mDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.mDataGridView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle62.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle62.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle62.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle62.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle62.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle62.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle62.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle62;
            this.mDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSelect,
            this.ColumnNo,
            this.ColumnVAccountNo,
            this.ColumnVBankName,
            this.ColumnClientCode,
            this.ColumnClientName,
            this.ColumnDriverCode,
            this.ColumnDriverName});
            dataGridViewCellStyle70.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle70.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle70.Font = new System.Drawing.Font("맑은 고딕", 9F);
            dataGridViewCellStyle70.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle70.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle70.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle70.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mDataGridView.DefaultCellStyle = dataGridViewCellStyle70;
            this.mDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mDataGridView.GridColor = System.Drawing.Color.White;
            this.mDataGridView.Location = new System.Drawing.Point(0, 43);
            this.mDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.mDataGridView.MultiSelect = false;
            this.mDataGridView.Name = "mDataGridView";
            this.mDataGridView.RowHeadersVisible = false;
            this.mDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.mDataGridView.RowTemplate.Height = 23;
            this.mDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mDataGridView.Size = new System.Drawing.Size(1012, 573);
            this.mDataGridView.TabIndex = 2;
            this.mDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mDataGridView_CellContentClick);
            this.mDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.mDataGridView_CellFormatting);
            // 
            // ColumnSelect
            // 
            this.ColumnSelect.FalseValue = false;
            this.ColumnSelect.HeaderText = "";
            this.ColumnSelect.Name = "ColumnSelect";
            this.ColumnSelect.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnSelect.TrueValue = true;
            this.ColumnSelect.Width = 70;
            // 
            // ColumnNo
            // 
            dataGridViewCellStyle63.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnNo.DefaultCellStyle = dataGridViewCellStyle63;
            this.ColumnNo.HeaderText = "번호";
            this.ColumnNo.Name = "ColumnNo";
            this.ColumnNo.ReadOnly = true;
            this.ColumnNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnNo.Width = 60;
            // 
            // ColumnVAccountNo
            // 
            this.ColumnVAccountNo.DataPropertyName = "VAccountNo";
            dataGridViewCellStyle64.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnVAccountNo.DefaultCellStyle = dataGridViewCellStyle64;
            this.ColumnVAccountNo.HeaderText = "가상계좌번호";
            this.ColumnVAccountNo.Name = "ColumnVAccountNo";
            this.ColumnVAccountNo.ReadOnly = true;
            this.ColumnVAccountNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnVAccountNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnVAccountNo.Width = 120;
            // 
            // ColumnVBankName
            // 
            this.ColumnVBankName.DataPropertyName = "VBankName";
            dataGridViewCellStyle65.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnVBankName.DefaultCellStyle = dataGridViewCellStyle65;
            this.ColumnVBankName.HeaderText = "은행명";
            this.ColumnVBankName.Name = "ColumnVBankName";
            this.ColumnVBankName.ReadOnly = true;
            this.ColumnVBankName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnVBankName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnVBankName.Width = 60;
            // 
            // ColumnClientCode
            // 
            this.ColumnClientCode.DataPropertyName = "ClientCode";
            dataGridViewCellStyle66.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnClientCode.DefaultCellStyle = dataGridViewCellStyle66;
            this.ColumnClientCode.HeaderText = "운송사코드";
            this.ColumnClientCode.Name = "ColumnClientCode";
            this.ColumnClientCode.ReadOnly = true;
            this.ColumnClientCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnClientCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnClientName
            // 
            this.ColumnClientName.DataPropertyName = "ClientName";
            dataGridViewCellStyle67.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnClientName.DefaultCellStyle = dataGridViewCellStyle67;
            this.ColumnClientName.HeaderText = "운송사명";
            this.ColumnClientName.Name = "ColumnClientName";
            this.ColumnClientName.ReadOnly = true;
            this.ColumnClientName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnClientName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnClientName.Width = 120;
            // 
            // ColumnDriverCode
            // 
            this.ColumnDriverCode.DataPropertyName = "DriverCode";
            dataGridViewCellStyle68.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnDriverCode.DefaultCellStyle = dataGridViewCellStyle68;
            this.ColumnDriverCode.HeaderText = "기사코드";
            this.ColumnDriverCode.Name = "ColumnDriverCode";
            this.ColumnDriverCode.ReadOnly = true;
            this.ColumnDriverCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDriverCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnDriverName
            // 
            this.ColumnDriverName.DataPropertyName = "DriverName";
            dataGridViewCellStyle69.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnDriverName.DefaultCellStyle = dataGridViewCellStyle69;
            this.ColumnDriverName.HeaderText = "기사명";
            this.ColumnDriverName.Name = "ColumnDriverName";
            this.ColumnDriverName.ReadOnly = true;
            this.ColumnDriverName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDriverName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDriverName.Width = 120;
            // 
            // FrmMN0804VAccountPool
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1012, 616);
            this.Controls.Add(this.AllSelect);
            this.Controls.Add(this.mDataGridView);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmMN0804VAccountPool";
            this.Text = "가상계좌 관리";
            this.Load += new System.EventHandler(this.FrmMN0804VAccountPool_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button ImportExcel;
        private NewDGV mDataGridView;
        private System.Windows.Forms.Button SaveForm;
        private System.Windows.Forms.CheckBox AllSelect;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVAccountNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVBankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClientCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDriverCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDriverName;
    }
}