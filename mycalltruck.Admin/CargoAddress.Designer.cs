namespace mycalltruck.Admin
{
    partial class CargoAddress
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbl_Sido = new System.Windows.Forms.Label();
            this.lbl_GuGun = new System.Windows.Forms.Label();
            this.cmb_Dong = new System.Windows.Forms.ComboBox();
            this.cargoApiAdressTableAdapter = new mycalltruck.Admin.DataSets.CargoDataSetTableAdapters.CargoApiAdressTableAdapter();
            this.cargoDataSet = new mycalltruck.Admin.DataSets.CargoDataSet();
            this.cargoApiAdressNewTableAdapter = new mycalltruck.Admin.DataSets.CargoDataSetTableAdapters.CargoApiAdressNewTableAdapter();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cargoDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(547, 180);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.61354F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.38646F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 277F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Sido, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbl_GuGun, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmb_Dong, 3, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(547, 180);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(23, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(521, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "\"화물맨\" 프로그램과 아래 상/하차지 주소로 연동 합니다.\r\n해당 \"읍/명/동\"을 선택해 주십시오.\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 4);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 145);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(547, 43);
            this.panel2.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Gray;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(193, 6);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "취소";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Gray;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(274, 6);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbl_Sido
            // 
            this.lbl_Sido.AutoSize = true;
            this.lbl_Sido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Sido.Location = new System.Drawing.Point(23, 77);
            this.lbl_Sido.Name = "lbl_Sido";
            this.lbl_Sido.Size = new System.Drawing.Size(110, 35);
            this.lbl_Sido.TabIndex = 1;
            this.lbl_Sido.Text = "서울특별시";
            this.lbl_Sido.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_GuGun
            // 
            this.lbl_GuGun.AutoSize = true;
            this.lbl_GuGun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_GuGun.Location = new System.Drawing.Point(139, 77);
            this.lbl_GuGun.Name = "lbl_GuGun";
            this.lbl_GuGun.Size = new System.Drawing.Size(127, 35);
            this.lbl_GuGun.TabIndex = 2;
            this.lbl_GuGun.Text = "구로구";
            this.lbl_GuGun.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmb_Dong
            // 
            this.cmb_Dong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Dong.FormattingEnabled = true;
            this.cmb_Dong.Location = new System.Drawing.Point(272, 80);
            this.cmb_Dong.Name = "cmb_Dong";
            this.cmb_Dong.Size = new System.Drawing.Size(184, 20);
            this.cmb_Dong.TabIndex = 4;
            // 
            // cargoApiAdressTableAdapter
            // 
            this.cargoApiAdressTableAdapter.ClearBeforeFill = true;
            // 
            // cargoDataSet
            // 
            this.cargoDataSet.DataSetName = "CargoDataSet";
            this.cargoDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cargoApiAdressNewTableAdapter
            // 
            this.cargoApiAdressNewTableAdapter.ClearBeforeFill = true;
            // 
            // CargoAddress
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(547, 180);
            this.Controls.Add(this.panel1);
            this.Name = "CargoAddress";
            this.Text = "화물맨 주소연동";
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cargoDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Sido;
        private System.Windows.Forms.Label lbl_GuGun;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmb_Dong;
        private DataSets.CargoDataSetTableAdapters.CargoApiAdressTableAdapter cargoApiAdressTableAdapter;
        private DataSets.CargoDataSet cargoDataSet;
        private DataSets.CargoDataSetTableAdapters.CargoApiAdressNewTableAdapter cargoApiAdressNewTableAdapter;
    }
}