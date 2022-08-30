namespace mycalltruck.Admin
{
    partial class FrmMN0213_PRICEMANAGE_Header
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LayoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_ClientSearch = new System.Windows.Forms.TextBox();
            this.nmRate = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.cmbClient = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnAction = new System.Windows.Forms.Panel();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_Next = new System.Windows.Forms.Button();
            this.headerGridView = new mycalltruck.Admin.NewDGV();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColOrigin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColState = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColCity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LayoutRoot.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmRate)).BeginInit();
            this.pnAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutRoot
            // 
            this.LayoutRoot.ColumnCount = 1;
            this.LayoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutRoot.Controls.Add(this.panel2, 0, 1);
            this.LayoutRoot.Controls.Add(this.panel1, 0, 0);
            this.LayoutRoot.Controls.Add(this.pnAction, 0, 6);
            this.LayoutRoot.Controls.Add(this.headerGridView, 0, 4);
            this.LayoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutRoot.Location = new System.Drawing.Point(0, 0);
            this.LayoutRoot.Name = "LayoutRoot";
            this.LayoutRoot.RowCount = 7;
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutRoot.Size = new System.Drawing.Size(531, 508);
            this.LayoutRoot.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 157);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(531, 18);
            this.panel2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "탁송업체 운송료 주소 수정";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.txt_ClientSearch);
            this.panel1.Controls.Add(this.nmRate);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbCustomer);
            this.panel1.Controls.Add(this.cmbClient);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(531, 157);
            this.panel1.TabIndex = 3;
            // 
            // txt_ClientSearch
            // 
            this.txt_ClientSearch.Location = new System.Drawing.Point(3, 27);
            this.txt_ClientSearch.Name = "txt_ClientSearch";
            this.txt_ClientSearch.Size = new System.Drawing.Size(330, 21);
            this.txt_ClientSearch.TabIndex = 9;
            this.txt_ClientSearch.TextChanged += new System.EventHandler(this.txt_ClientSearch_TextChanged);
            this.txt_ClientSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_ClientSearch_KeyDown);
            // 
            // nmRate
            // 
            this.nmRate.Location = new System.Drawing.Point(133, 128);
            this.nmRate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.nmRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmRate.Name = "nmRate";
            this.nmRate.Size = new System.Drawing.Size(60, 21);
            this.nmRate.TabIndex = 7;
            this.nmRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmRate.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(3, 131);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "차주운송료 비율(%)";
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbCustomer.DisplayMember = "Value";
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCustomer.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.ItemHeight = 12;
            this.cmbCustomer.Location = new System.Drawing.Point(5, 98);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(328, 20);
            this.cmbCustomer.TabIndex = 5;
            this.cmbCustomer.ValueMember = "Key";
            // 
            // cmbClient
            // 
            this.cmbClient.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbClient.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbClient.FormattingEnabled = true;
            this.cmbClient.ItemHeight = 12;
            this.cmbClient.Location = new System.Drawing.Point(5, 54);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(328, 20);
            this.cmbClient.TabIndex = 4;
            this.cmbClient.SelectedIndexChanged += new System.EventHandler(this.cmbClient_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(3, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "화주 정보";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "운송사 선택";
            // 
            // pnAction
            // 
            this.pnAction.AutoSize = true;
            this.pnAction.Controls.Add(this.btn_Close);
            this.pnAction.Controls.Add(this.btn_Next);
            this.pnAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAction.Location = new System.Drawing.Point(0, 479);
            this.pnAction.Margin = new System.Windows.Forms.Padding(0);
            this.pnAction.Name = "pnAction";
            this.pnAction.Size = new System.Drawing.Size(531, 29);
            this.pnAction.TabIndex = 1;
            // 
            // btn_Close
            // 
            this.btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Close.Location = new System.Drawing.Point(372, 3);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 1;
            this.btn_Close.TabStop = false;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Next.Location = new System.Drawing.Point(453, 3);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(75, 23);
            this.btn_Next.TabIndex = 0;
            this.btn_Next.TabStop = false;
            this.btn_Next.Text = "계속";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // headerGridView
            // 
            this.headerGridView.AllowUserToAddRows = false;
            this.headerGridView.AllowUserToDeleteRows = false;
            this.headerGridView.AllowUserToResizeColumns = false;
            this.headerGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.headerGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.headerGridView.BackgroundColor = System.Drawing.Color.White;
            this.headerGridView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.headerGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.headerGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.headerGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.headerGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.headerGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.ColOrigin,
            this.ColState,
            this.ColCity});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.headerGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.headerGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerGridView.GridColor = System.Drawing.Color.White;
            this.headerGridView.Location = new System.Drawing.Point(0, 175);
            this.headerGridView.Margin = new System.Windows.Forms.Padding(0);
            this.headerGridView.Name = "headerGridView";
            this.headerGridView.RowHeadersVisible = false;
            this.headerGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.headerGridView.RowTemplate.Height = 23;
            this.headerGridView.Size = new System.Drawing.Size(531, 304);
            this.headerGridView.TabIndex = 2;
            this.headerGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.headerGridView_CellEndEdit);
            this.headerGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.headerGridView_CellFormatting);
            this.headerGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.headerGridView_CellPainting);
            this.headerGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.headerGridView_DataBindingComplete);
            this.headerGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.headerGridView_EditingControlShowing);
            // 
            // colNumber
            // 
            this.colNumber.Frozen = true;
            this.colNumber.HeaderText = "번호";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.Width = 65;
            // 
            // ColOrigin
            // 
            this.ColOrigin.DataPropertyName = "ExcelText";
            this.ColOrigin.Frozen = true;
            this.ColOrigin.HeaderText = "엑셀 텍스트";
            this.ColOrigin.Name = "ColOrigin";
            this.ColOrigin.ReadOnly = true;
            this.ColOrigin.Width = 200;
            // 
            // ColState
            // 
            this.ColState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColState.Frozen = true;
            this.ColState.HeaderText = "시/도";
            this.ColState.Name = "ColState";
            this.ColState.Width = 120;
            // 
            // ColCity
            // 
            this.ColCity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColCity.Frozen = true;
            this.ColCity.HeaderText = "시/군/구";
            this.ColCity.Name = "ColCity";
            this.ColCity.Width = 120;
            // 
            // FrmMN0213_PRICEMANAGE_Header
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(531, 508);
            this.Controls.Add(this.LayoutRoot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMN0213_PRICEMANAGE_Header";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "탁송업체 운송료 등록";
            this.LayoutRoot.ResumeLayout(false);
            this.LayoutRoot.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmRate)).EndInit();
            this.pnAction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutRoot;
        private System.Windows.Forms.Panel pnAction;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Button btn_Next;
        private NewDGV headerGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbClient;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColOrigin;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColState;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColCity;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.NumericUpDown nmRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_ClientSearch;
    }
}