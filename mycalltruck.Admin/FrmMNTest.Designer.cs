namespace mycalltruck.Admin
{
    partial class FrmMNTest
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.colTestTile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTestContents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTestResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColTestContents1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTestContents2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTestContents3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTestContents4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestContentsRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.clientDataSet = new mycalltruck.Admin.DataSets.ClientDataSet();
            this.clientsTableAdapter = new mycalltruck.Admin.DataSets.ClientDataSetTableAdapters.ClientsTableAdapter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            this.pnProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.newDGV1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToDeleteRows = false;
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            this.newDGV1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.newDGV1.BackgroundColor = System.Drawing.Color.White;
            this.newDGV1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.newDGV1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newDGV1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTestTile,
            this.colTestContents,
            this.colTestResult,
            this.colCode,
            this.ColTestContents1,
            this.ColTestContents2,
            this.ColTestContents3,
            this.ColTestContents4,
            this.TestContentsRemark});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle12;
            this.newDGV1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(0, 0);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.MultiSelect = false;
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.ReadOnly = true;
            this.newDGV1.RowHeadersVisible = false;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newDGV1.Size = new System.Drawing.Size(800, 450);
            this.newDGV1.TabIndex = 1;
            this.newDGV1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newDGV1_CellContentClick);
            this.newDGV1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newDGV1_CellFormatting);
            this.newDGV1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.newDGV1_CellPainting);
            // 
            // colTestTile
            // 
            this.colTestTile.DataPropertyName = "TestTile";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colTestTile.DefaultCellStyle = dataGridViewCellStyle11;
            this.colTestTile.HeaderText = "테스트항목";
            this.colTestTile.Name = "colTestTile";
            this.colTestTile.ReadOnly = true;
            this.colTestTile.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTestTile.Width = 120;
            // 
            // colTestContents
            // 
            this.colTestContents.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTestContents.HeaderText = "세부내역";
            this.colTestContents.Name = "colTestContents";
            this.colTestContents.ReadOnly = true;
            this.colTestContents.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTestResult
            // 
            this.colTestResult.DataPropertyName = "TestResult";
            this.colTestResult.HeaderText = "결과";
            this.colTestResult.Name = "colTestResult";
            this.colTestResult.ReadOnly = true;
            this.colTestResult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "실행";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCode.Text = "실행";
            this.colCode.UseColumnTextForButtonValue = true;
            this.colCode.Width = 80;
            // 
            // ColTestContents1
            // 
            this.ColTestContents1.HeaderText = "ColTestContents1";
            this.ColTestContents1.Name = "ColTestContents1";
            this.ColTestContents1.ReadOnly = true;
            this.ColTestContents1.Visible = false;
            // 
            // ColTestContents2
            // 
            this.ColTestContents2.HeaderText = "ColTestContents2";
            this.ColTestContents2.Name = "ColTestContents2";
            this.ColTestContents2.ReadOnly = true;
            this.ColTestContents2.Visible = false;
            // 
            // ColTestContents3
            // 
            this.ColTestContents3.HeaderText = "ColTestContents3";
            this.ColTestContents3.Name = "ColTestContents3";
            this.ColTestContents3.ReadOnly = true;
            this.ColTestContents3.Visible = false;
            // 
            // ColTestContents4
            // 
            this.ColTestContents4.HeaderText = "ColTestContents4";
            this.ColTestContents4.Name = "ColTestContents4";
            this.ColTestContents4.ReadOnly = true;
            this.ColTestContents4.Visible = false;
            // 
            // TestContentsRemark
            // 
            this.TestContentsRemark.HeaderText = "TestContentsRemark";
            this.TestContentsRemark.Name = "TestContentsRemark";
            this.TestContentsRemark.ReadOnly = true;
            this.TestContentsRemark.Visible = false;
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(300, 193);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 99;
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
            this.bar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.bar.TabIndex = 2;
            // 
            // clientDataSet
            // 
            this.clientDataSet.DataSetName = "ClientDataSet";
            this.clientDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // clientsTableAdapter
            // 
            this.clientsTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMNTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMNTest";
            this.Text = "연동모듈 테스트";
            this.Load += new System.EventHandler(this.FrmMNTest_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            this.pnProgress.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private NewDGV newDGV1;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTestTile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTestContents;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTestResult;
        private System.Windows.Forms.DataGridViewButtonColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTestContents1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTestContents2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTestContents3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTestContents4;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestContentsRemark;
        private DataSets.ClientDataSet clientDataSet;
        private DataSets.ClientDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;
    }
}