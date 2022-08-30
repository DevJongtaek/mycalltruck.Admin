namespace mycalltruck.Admin
{
    partial class FrmMNSTATSbak
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("정산관리");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("정산결과");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("미수/미지급금");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("거래처별실적");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("차량별실적");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("월별미수금현황");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("일자별 결제(차주) 집계표");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("일자별 수금(화주) 집계표");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("차주별 결제 집계표");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("화주별 수금 집계표");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("일자별(차주/화주) 수익성분석");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("화주별 수익성분성");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("화주별 거래명세서 출력");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("배차일보");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("매출(화주)정산");
            this.bPanel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnProgress = new System.Windows.Forms.Panel();
            this.label66 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.bPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // bPanel1
            // 
            this.bPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bPanel1.Controls.Add(this.label11);
            this.bPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bPanel1.Location = new System.Drawing.Point(0, 0);
            this.bPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.bPanel1.Name = "bPanel1";
            this.bPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.bPanel1.Size = new System.Drawing.Size(1012, 29);
            this.bPanel1.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label11.Location = new System.Drawing.Point(6, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(157, 16);
            this.label11.TabIndex = 17;
            this.label11.Text = "[통계 및 정산관리]";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(1012, 587);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1008, 583);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.treeView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(1, 1);
            this.panel3.Margin = new System.Windows.Forms.Padding(1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(198, 581);
            this.panel3.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Account01";
            treeNode1.Text = "정산관리";
            treeNode2.Name = "Account02";
            treeNode2.Text = "정산결과";
            treeNode3.Name = "Account03";
            treeNode3.Text = "미수/미지급금";
            treeNode4.Name = "Account04";
            treeNode4.Text = "거래처별실적";
            treeNode5.Name = "Account05";
            treeNode5.Text = "차량별실적";
            treeNode6.Name = "Account06";
            treeNode6.Text = "월별미수금현황";
            treeNode7.Name = "STATS1";
            treeNode7.Text = "일자별 결제(차주) 집계표";
            treeNode8.Name = "STATS2";
            treeNode8.Text = "일자별 수금(화주) 집계표";
            treeNode9.Name = "STATS3";
            treeNode9.Text = "차주별 결제 집계표";
            treeNode10.Name = "STATS4";
            treeNode10.Text = "화주별 수금 집계표";
            treeNode11.Name = "STATS5";
            treeNode11.Text = "일자별(차주/화주) 수익성분석";
            treeNode12.Name = "STATS6";
            treeNode12.Text = "화주별 수익성분성";
            treeNode13.Name = "STATS7";
            treeNode13.Text = "화주별 거래명세서 출력";
            treeNode14.Name = "NSTAT1";
            treeNode14.Text = "배차일보";
            treeNode15.Name = "NSTAT2";
            treeNode15.Text = "매출(화주)정산";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15});
            this.treeView1.Size = new System.Drawing.Size(196, 579);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(201, 1);
            this.panel2.Margin = new System.Windows.Forms.Padding(1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(806, 581);
            this.panel2.TabIndex = 0;
            // 
            // pnProgress
            // 
            this.pnProgress.Controls.Add(this.label66);
            this.pnProgress.Controls.Add(this.bar);
            this.pnProgress.Location = new System.Drawing.Point(406, 276);
            this.pnProgress.Name = "pnProgress";
            this.pnProgress.Padding = new System.Windows.Forms.Padding(10);
            this.pnProgress.Size = new System.Drawing.Size(200, 64);
            this.pnProgress.TabIndex = 38;
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
            // FrmMNSTATS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1012, 616);
            this.Controls.Add(this.pnProgress);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bPanel1);
            this.Name = "FrmMNSTATS";
            this.Text = "통계 및 정산관리";
            this.Load += new System.EventHandler(this.FrmMNFPIS_Load);
            this.bPanel1.ResumeLayout(false);
            this.bPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bPanel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnProgress;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ProgressBar bar;
       
        private System.Windows.Forms.TreeView treeView1;
    }
}