namespace mycalltruck.Admin
{
    partial class FrmMN301_Call_Popup
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
            this.Root = new System.Windows.Forms.TableLayoutPanel();
            this.MemoUpdate = new System.Windows.Forms.Button();
            this.CallPhoneNo = new System.Windows.Forms.Label();
            this.CallTime = new System.Windows.Forms.Label();
            this.CallTarget = new System.Windows.Forms.Label();
            this.CallAddress = new System.Windows.Forms.Label();
            this.Memo = new System.Windows.Forms.TextBox();
            this.CallRe = new System.Windows.Forms.Button();
            this.ShowImage = new System.Windows.Forms.Button();
            this.InputOrder = new System.Windows.Forms.Button();
            this.Root.SuspendLayout();
            this.SuspendLayout();
            // 
            // Root
            // 
            this.Root.BackColor = System.Drawing.Color.Transparent;
            this.Root.ColumnCount = 4;
            this.Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Root.Controls.Add(this.MemoUpdate, 0, 5);
            this.Root.Controls.Add(this.CallPhoneNo, 0, 0);
            this.Root.Controls.Add(this.CallTime, 0, 3);
            this.Root.Controls.Add(this.CallTarget, 0, 1);
            this.Root.Controls.Add(this.CallAddress, 0, 2);
            this.Root.Controls.Add(this.Memo, 0, 4);
            this.Root.Controls.Add(this.CallRe, 1, 5);
            this.Root.Controls.Add(this.ShowImage, 2, 5);
            this.Root.Controls.Add(this.InputOrder, 3, 5);
            this.Root.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Root.Location = new System.Drawing.Point(0, 0);
            this.Root.Margin = new System.Windows.Forms.Padding(0);
            this.Root.Name = "Root";
            this.Root.RowCount = 6;
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Root.Size = new System.Drawing.Size(284, 301);
            this.Root.TabIndex = 1;
            // 
            // MemoUpdate
            // 
            this.MemoUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MemoUpdate.Location = new System.Drawing.Point(0, 272);
            this.MemoUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.MemoUpdate.Name = "MemoUpdate";
            this.MemoUpdate.Size = new System.Drawing.Size(71, 29);
            this.MemoUpdate.TabIndex = 10;
            this.MemoUpdate.Text = "메모저장";
            this.MemoUpdate.UseVisualStyleBackColor = true;
            this.MemoUpdate.Click += new System.EventHandler(this.MemoUpdate_Click);
            // 
            // CallPhoneNo
            // 
            this.CallPhoneNo.BackColor = System.Drawing.Color.Sienna;
            this.Root.SetColumnSpan(this.CallPhoneNo, 4);
            this.CallPhoneNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CallPhoneNo.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CallPhoneNo.ForeColor = System.Drawing.Color.White;
            this.CallPhoneNo.Location = new System.Drawing.Point(0, 0);
            this.CallPhoneNo.Margin = new System.Windows.Forms.Padding(0);
            this.CallPhoneNo.Name = "CallPhoneNo";
            this.CallPhoneNo.Size = new System.Drawing.Size(284, 44);
            this.CallPhoneNo.TabIndex = 0;
            this.CallPhoneNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CallTime
            // 
            this.Root.SetColumnSpan(this.CallTime, 4);
            this.CallTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.CallTime.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CallTime.ForeColor = System.Drawing.Color.White;
            this.CallTime.Location = new System.Drawing.Point(5, 126);
            this.CallTime.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.CallTime.Name = "CallTime";
            this.CallTime.Size = new System.Drawing.Size(274, 25);
            this.CallTime.TabIndex = 1;
            this.CallTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CallTarget
            // 
            this.Root.SetColumnSpan(this.CallTarget, 4);
            this.CallTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CallTarget.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CallTarget.ForeColor = System.Drawing.Color.Yellow;
            this.CallTarget.Location = new System.Drawing.Point(5, 50);
            this.CallTarget.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.CallTarget.Name = "CallTarget";
            this.CallTarget.Size = new System.Drawing.Size(274, 26);
            this.CallTarget.TabIndex = 2;
            this.CallTarget.Text = "-";
            this.CallTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CallAddress
            // 
            this.Root.SetColumnSpan(this.CallAddress, 4);
            this.CallAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CallAddress.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CallAddress.ForeColor = System.Drawing.Color.White;
            this.CallAddress.Location = new System.Drawing.Point(5, 88);
            this.CallAddress.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.CallAddress.Name = "CallAddress";
            this.CallAddress.Size = new System.Drawing.Size(274, 26);
            this.CallAddress.TabIndex = 4;
            this.CallAddress.Text = "-";
            this.CallAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Memo
            // 
            this.Root.SetColumnSpan(this.Memo, 4);
            this.Memo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Memo.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Memo.Location = new System.Drawing.Point(5, 164);
            this.Memo.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Memo.MaxLength = 400;
            this.Memo.Multiline = true;
            this.Memo.Name = "Memo";
            this.Memo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Memo.Size = new System.Drawing.Size(274, 102);
            this.Memo.TabIndex = 6;
            // 
            // CallRe
            // 
            this.CallRe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CallRe.Location = new System.Drawing.Point(71, 272);
            this.CallRe.Margin = new System.Windows.Forms.Padding(0);
            this.CallRe.Name = "CallRe";
            this.CallRe.Size = new System.Drawing.Size(71, 29);
            this.CallRe.TabIndex = 7;
            this.CallRe.Text = "통화하기";
            this.CallRe.UseVisualStyleBackColor = true;
            this.CallRe.Click += new System.EventHandler(this.CallRe_Click);
            // 
            // ShowImage
            // 
            this.ShowImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowImage.Enabled = false;
            this.ShowImage.Location = new System.Drawing.Point(142, 272);
            this.ShowImage.Margin = new System.Windows.Forms.Padding(0);
            this.ShowImage.Name = "ShowImage";
            this.ShowImage.Size = new System.Drawing.Size(71, 29);
            this.ShowImage.TabIndex = 8;
            this.ShowImage.Text = "의뢰물건";
            this.ShowImage.UseVisualStyleBackColor = true;
            this.ShowImage.Click += new System.EventHandler(this.ShowImage_Click);
            // 
            // InputOrder
            // 
            this.InputOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputOrder.Enabled = false;
            this.InputOrder.Location = new System.Drawing.Point(213, 272);
            this.InputOrder.Margin = new System.Windows.Forms.Padding(0);
            this.InputOrder.Name = "InputOrder";
            this.InputOrder.Size = new System.Drawing.Size(71, 29);
            this.InputOrder.TabIndex = 9;
            this.InputOrder.Text = "화물입력";
            this.InputOrder.UseVisualStyleBackColor = true;
            this.InputOrder.Click += new System.EventHandler(this.InputOrder_Click);
            // 
            // FrmMN301_Call_Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::mycalltruck.Admin.Properties.Resources.CallPopupBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(284, 301);
            this.Controls.Add(this.Root);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FrmMN301_Call_Popup";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "차세로 - 전화 수신";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmMN301_Call_Popup_Load);
            this.Root.ResumeLayout(false);
            this.Root.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Root;
        private System.Windows.Forms.Label CallPhoneNo;
        private System.Windows.Forms.Label CallTime;
        private System.Windows.Forms.Label CallAddress;
        private System.Windows.Forms.TextBox Memo;
        private System.Windows.Forms.Button MemoUpdate;
        private System.Windows.Forms.Button CallRe;
        private System.Windows.Forms.Button ShowImage;
        private System.Windows.Forms.Button InputOrder;
        private System.Windows.Forms.Label CallTarget;
    }
}