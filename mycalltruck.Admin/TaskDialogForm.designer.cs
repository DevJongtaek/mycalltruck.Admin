namespace mycalltruck.Admin
{
    partial class TaskDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskDialogForm));
            this.pnHead = new System.Windows.Forms.Panel();
            this.lblHeadTex = new System.Windows.Forms.Label();
            this.pnContent = new System.Windows.Forms.Panel();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.pnLabels = new System.Windows.Forms.Panel();
            this.lblContent3 = new System.Windows.Forms.Label();
            this.lblContent2 = new System.Windows.Forms.Label();
            this.lblContent1 = new System.Windows.Forms.Label();
            this.pnButtons = new System.Windows.Forms.Panel();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnHead.SuspendLayout();
            this.pnContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.pnLabels.SuspendLayout();
            this.pnButtons.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnHead
            // 
            this.pnHead.BackColor = System.Drawing.Color.White;
            this.pnHead.Controls.Add(this.lblHeadTex);
            this.pnHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHead.Location = new System.Drawing.Point(0, 0);
            this.pnHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnHead.Name = "pnHead";
            this.pnHead.Padding = new System.Windows.Forms.Padding(10);
            this.pnHead.Size = new System.Drawing.Size(452, 40);
            this.pnHead.TabIndex = 0;
            // 
            // lblHeadTex
            // 
            this.lblHeadTex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeadTex.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblHeadTex.Location = new System.Drawing.Point(10, 10);
            this.lblHeadTex.Name = "lblHeadTex";
            this.lblHeadTex.Size = new System.Drawing.Size(432, 20);
            this.lblHeadTex.TabIndex = 0;
            this.lblHeadTex.Text = "이 파일을 열거나 저장하시겠습니까?";
            this.lblHeadTex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnContent
            // 
            this.pnContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnContent.BackColor = System.Drawing.Color.White;
            this.pnContent.Controls.Add(this.picIcon);
            this.pnContent.Controls.Add(this.pnLabels);
            this.pnContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnContent.Location = new System.Drawing.Point(0, 40);
            this.pnContent.Margin = new System.Windows.Forms.Padding(0);
            this.pnContent.Name = "pnContent";
            this.pnContent.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnContent.Size = new System.Drawing.Size(452, 72);
            this.pnContent.TabIndex = 1;
            // 
            // picIcon
            // 
            this.picIcon.ErrorImage = ((System.Drawing.Image)(resources.GetObject("picIcon.ErrorImage")));
            this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
            this.picIcon.Location = new System.Drawing.Point(41, 14);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(26, 26);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picIcon.TabIndex = 1;
            this.picIcon.TabStop = false;
            // 
            // pnLabels
            // 
            this.pnLabels.Controls.Add(this.lblContent3);
            this.pnLabels.Controls.Add(this.lblContent2);
            this.pnLabels.Controls.Add(this.lblContent1);
            this.pnLabels.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnLabels.Location = new System.Drawing.Point(104, 0);
            this.pnLabels.Name = "pnLabels";
            this.pnLabels.Size = new System.Drawing.Size(338, 62);
            this.pnLabels.TabIndex = 0;
            // 
            // lblContent3
            // 
            this.lblContent3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblContent3.Location = new System.Drawing.Point(0, 40);
            this.lblContent3.Margin = new System.Windows.Forms.Padding(0);
            this.lblContent3.Name = "lblContent3";
            this.lblContent3.Size = new System.Drawing.Size(338, 20);
            this.lblContent3.TabIndex = 2;
            this.lblContent3.Text = "게시자 : 카드페이";
            this.lblContent3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContent2
            // 
            this.lblContent2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblContent2.Location = new System.Drawing.Point(0, 20);
            this.lblContent2.Margin = new System.Windows.Forms.Padding(0);
            this.lblContent2.Name = "lblContent2";
            this.lblContent2.Size = new System.Drawing.Size(338, 20);
            this.lblContent2.TabIndex = 1;
            this.lblContent2.Text = "유형 : Microsoft Office Excel 97 - 2003 통합문서(.xls)";
            this.lblContent2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContent1
            // 
            this.lblContent1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblContent1.Location = new System.Drawing.Point(0, 0);
            this.lblContent1.Margin = new System.Windows.Forms.Padding(0);
            this.lblContent1.Name = "lblContent1";
            this.lblContent1.Size = new System.Drawing.Size(338, 20);
            this.lblContent1.TabIndex = 0;
            this.lblContent1.Text = "이름 : 상품정보.xls";
            this.lblContent1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnButtons
            // 
            this.pnButtons.BackColor = System.Drawing.Color.White;
            this.pnButtons.Controls.Add(this.btnOpen);
            this.pnButtons.Controls.Add(this.btnSave);
            this.pnButtons.Controls.Add(this.btnCancel);
            this.pnButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnButtons.Location = new System.Drawing.Point(0, 112);
            this.pnButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Padding = new System.Windows.Forms.Padding(10);
            this.pnButtons.Size = new System.Drawing.Size(452, 43);
            this.pnButtons.TabIndex = 2;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(158, 10);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(90, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "열기";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(255, 10);
            this.btnSave.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(352, 10);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 1);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 156);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(452, 1);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 157);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10);
            this.panel3.Size = new System.Drawing.Size(452, 60);
            this.panel3.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Location = new System.Drawing.Point(61, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(381, 40);
            this.label6.TabIndex = 2;
            this.label6.Text = "열기를 선택하시면, 새 엑셀 어플리케이션이 실행된 후 해당 정보를 엑셀 문서로 편집할 수 있으며, 저장을 선택하시는 경우 파일 위치 선택 창이 떠" +
    "서, 선택하신 위치로 내보내진 엑셀 파일이 자동 저장 됩니다.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // TaskDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(452, 218);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnButtons);
            this.Controls.Add(this.pnContent);
            this.Controls.Add(this.pnHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskDialogForm";
            this.Text = "TaskDialogForm";
            this.pnHead.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.pnContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.pnLabels.ResumeLayout(false);
            this.pnButtons.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnHead;
        private System.Windows.Forms.Label lblHeadTex;
        private System.Windows.Forms.Panel pnContent;
        private System.Windows.Forms.Panel pnLabels;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label lblContent3;
        private System.Windows.Forms.Label lblContent2;
        private System.Windows.Forms.Label lblContent1;
        private System.Windows.Forms.Panel pnButtons;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}