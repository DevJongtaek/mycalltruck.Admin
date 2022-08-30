namespace mycalltruck.Admin
{
    partial class EXCELINSERT_DRIVER2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cMDataSet = new mycalltruck.Admin.CMDataSet();
            this.label1 = new System.Windows.Forms.Label();
            this.newDGV1 = new mycalltruck.Admin.NewDGV();
            this.btn_Close = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.btn_Test = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.staticOptionsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.StaticOptionsTableAdapter();
            this.btn_Info = new System.Windows.Forms.Button();
            this.driversTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.DriversTableAdapter();
            this.clientsTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.ClientsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.SuspendLayout();
            // 
            // cMDataSet
            // 
            this.cMDataSet.DataSetName = "CMDataSet";
            this.cMDataSet.EnforceConstraints = false;
            this.cMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "의뢰건수";
            // 
            // newDGV1
            // 
            this.newDGV1.AllowUserToAddRows = false;
            this.newDGV1.AllowUserToDeleteRows = false;
            this.newDGV1.AllowUserToOrderColumns = true;
            this.newDGV1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.newDGV1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.newDGV1.BackgroundColor = System.Drawing.Color.White;
            this.newDGV1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.newDGV1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newDGV1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.newDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.newDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newDGV1.DefaultCellStyle = dataGridViewCellStyle3;
            this.newDGV1.GridColor = System.Drawing.Color.White;
            this.newDGV1.Location = new System.Drawing.Point(282, 13);
            this.newDGV1.Margin = new System.Windows.Forms.Padding(0);
            this.newDGV1.Name = "newDGV1";
            this.newDGV1.ReadOnly = true;
            this.newDGV1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.newDGV1.RowTemplate.Height = 23;
            this.newDGV1.Size = new System.Drawing.Size(708, 188);
            this.newDGV1.TabIndex = 14;
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(134, 189);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 17;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(233, 24);
            this.label7.TabIndex = 10;
            this.label7.Text = "데이터 검증에 실패 하였습니다.\r\n\"보기\"에서 수정한 후 , 다시 검증하십시오.\r\n";
            this.label7.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_OK);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(11, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 141);
            this.panel1.TabIndex = 16;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(164, 77);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 9;
            this.btn_OK.Text = "보기";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "데이터 검증실패";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.MediumBlue;
            this.label6.Location = new System.Drawing.Point(131, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "데이터 검증성공";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.MediumBlue;
            this.label5.Location = new System.Drawing.Point(131, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.MediumBlue;
            this.label4.Location = new System.Drawing.Point(131, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "0";
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // btn_Test
            // 
            this.btn_Test.Location = new System.Drawing.Point(41, 189);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(75, 23);
            this.btn_Test.TabIndex = 15;
            this.btn_Test.Text = "검증";
            this.btn_Test.UseVisualStyleBackColor = true;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "이미지 파일(*jpg,*bmp,*.png,*.gif)|*jpg;*bmp;*.png;*.gif|모든 파일|*.*";
            // 
            // staticOptionsTableAdapter
            // 
            this.staticOptionsTableAdapter.ClearBeforeFill = true;
            // 
            // btn_Info
            // 
            this.btn_Info.Location = new System.Drawing.Point(11, 13);
            this.btn_Info.Name = "btn_Info";
            this.btn_Info.Size = new System.Drawing.Size(111, 23);
            this.btn_Info.TabIndex = 13;
            this.btn_Info.Text = "엑셀파일불러오기";
            this.btn_Info.UseVisualStyleBackColor = true;
            this.btn_Info.Click += new System.EventHandler(this.btn_Info_Click);
            // 
            // driversTableAdapter
            // 
            this.driversTableAdapter.ClearBeforeFill = true;
            // 
            // clientsTableAdapter
            // 
            this.clientsTableAdapter.ClearBeforeFill = true;
            // 
            // EXCELINSERT_DRIVER2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1000, 223);
            this.Controls.Add(this.newDGV1);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_Test);
            this.Controls.Add(this.btn_Info);
            this.Name = "EXCELINSERT_DRIVER2";
            this.Text = "MID일괄등록";
            this.Load += new System.EventHandler(this.EXCELINSERT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDGV1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CMDataSet cMDataSet;
        private System.Windows.Forms.Label label1;
        private NewDGV newDGV1;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.Button btn_Test;
        private System.Windows.Forms.Button btn_Info;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private CMDataSetTableAdapters.StaticOptionsTableAdapter staticOptionsTableAdapter;
        private CMDataSetTableAdapters.DriversTableAdapter driversTableAdapter;
        private CMDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter;

    }
}