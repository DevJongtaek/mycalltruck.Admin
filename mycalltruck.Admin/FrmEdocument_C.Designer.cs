namespace mycalltruck.Admin
{
    partial class FrmEdocument_C
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
            this.pdfViewer1 = new PdfiumViewer.PdfViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRotateRight = new System.Windows.Forms.Button();
            this.btnRotateLeft = new System.Windows.Forms.Button();
            this.btnFitBest = new System.Windows.Forms.Button();
            this.btnFitWidth = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tradeDataSet1 = new mycalltruck.Admin.DataSets.TradeDataSet();
            this.tradesTableAdapter1 = new mycalltruck.Admin.DataSets.TradeDataSetTableAdapters.TradesTableAdapter();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tradeDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // pdfViewer1
            // 
            this.pdfViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer1.Location = new System.Drawing.Point(3, 46);
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.Size = new System.Drawing.Size(794, 369);
            this.pdfViewer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 456);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pdfViewer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 456);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblKeyword);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnRotateRight);
            this.panel2.Controls.Add(this.btnRotateLeft);
            this.panel2.Controls.Add(this.btnFitBest);
            this.panel2.Controls.Add(this.btnFitWidth);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 43);
            this.panel2.TabIndex = 2;
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblKeyword.ForeColor = System.Drawing.Color.Blue;
            this.lblKeyword.Location = new System.Drawing.Point(202, 11);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(0, 12);
            this.lblKeyword.TabIndex = 9;
            this.lblKeyword.Tag = "";
            this.lblKeyword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(13, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 19);
            this.label2.TabIndex = 8;
            this.label2.Tag = "";
            this.label2.Text = "전자문서(LG-CNS) Keyword :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRotateRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnRotateRight.FlatAppearance.BorderSize = 0;
            this.btnRotateRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRotateRight.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRotateRight.ForeColor = System.Drawing.Color.White;
            this.btnRotateRight.Location = new System.Drawing.Point(679, 6);
            this.btnRotateRight.Margin = new System.Windows.Forms.Padding(0);
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(115, 27);
            this.btnRotateRight.TabIndex = 7;
            this.btnRotateRight.Text = "오른쪽으로 회전";
            this.btnRotateRight.UseVisualStyleBackColor = false;
            this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRotateLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnRotateLeft.FlatAppearance.BorderSize = 0;
            this.btnRotateLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRotateLeft.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRotateLeft.ForeColor = System.Drawing.Color.White;
            this.btnRotateLeft.Location = new System.Drawing.Point(574, 6);
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(100, 27);
            this.btnRotateLeft.TabIndex = 6;
            this.btnRotateLeft.Text = "왼쪽으로 회전";
            this.btnRotateLeft.UseVisualStyleBackColor = false;
            this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
            // 
            // btnFitBest
            // 
            this.btnFitBest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFitBest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnFitBest.FlatAppearance.BorderSize = 0;
            this.btnFitBest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFitBest.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFitBest.ForeColor = System.Drawing.Color.White;
            this.btnFitBest.Location = new System.Drawing.Point(469, 6);
            this.btnFitBest.Name = "btnFitBest";
            this.btnFitBest.Size = new System.Drawing.Size(100, 27);
            this.btnFitBest.TabIndex = 5;
            this.btnFitBest.Text = "화면에맞추기";
            this.btnFitBest.UseVisualStyleBackColor = false;
            this.btnFitBest.Click += new System.EventHandler(this.btnFitBest_Click);
            // 
            // btnFitWidth
            // 
            this.btnFitWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFitWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            this.btnFitWidth.FlatAppearance.BorderSize = 0;
            this.btnFitWidth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFitWidth.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFitWidth.ForeColor = System.Drawing.Color.White;
            this.btnFitWidth.Location = new System.Drawing.Point(364, 6);
            this.btnFitWidth.Name = "btnFitWidth";
            this.btnFitWidth.Size = new System.Drawing.Size(100, 27);
            this.btnFitWidth.TabIndex = 3;
            this.btnFitWidth.Text = "너비에맞추기";
            this.btnFitWidth.UseVisualStyleBackColor = false;
            this.btnFitWidth.Click += new System.EventHandler(this.btnFitWidth_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 424);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(794, 29);
            this.panel3.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(794, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "※. 위 “전자인수증”은 “전자문서 기본법(2018. 4. 25)” 제5조 2항에 의거, 원본과 동일한 효력을 갖음.\r\n※. 화주는 “www.ch" +
    "asero.co.kr” 홈페이지에서 해당 “전자인수증”을 직접 조회/출력 하실 수 있음.  \r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tradeDataSet1
            // 
            this.tradeDataSet1.DataSetName = "TradeDataSet";
            this.tradeDataSet1.EnforceConstraints = false;
            this.tradeDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tradesTableAdapter1
            // 
            this.tradesTableAdapter1.ClearBeforeFill = true;
            // 
            // FrmEdocument_C
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 456);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "FrmEdocument_C";
            this.Text = "인수증";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEdocument_FormClosing);
            this.Load += new System.EventHandler(this.FrmEdocument_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tradeDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PdfiumViewer.PdfViewer pdfViewer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnFitWidth;
        private System.Windows.Forms.Button btnFitBest;
        private System.Windows.Forms.Button btnRotateLeft;
        private System.Windows.Forms.Button btnRotateRight;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.Label label2;
        private DataSets.TradeDataSet tradeDataSet1;
        private DataSets.TradeDataSetTableAdapters.TradesTableAdapter tradesTableAdapter1;
    }
}