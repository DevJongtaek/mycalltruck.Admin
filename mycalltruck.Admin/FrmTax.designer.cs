namespace mycalltruck.Admin
{
    partial class FrmTax
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.cmDataSet1 = new mycalltruck.Admin.CMDataSet();
            this.taxTableAdapter1 = new mycalltruck.Admin.CMDataSetTableAdapters.TaxTableAdapter();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tradeDataSet1 = new mycalltruck.Admin.DataSets.TradeDataSet();
            this.taxOutTableAdapter1 = new mycalltruck.Admin.DataSets.TradeDataSetTableAdapters.TaxOutTableAdapter();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradeDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.AutoSize = true;
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Tax.rdlc";
            this.reportViewer1.LocalReport.ReportPath = "";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1014, 566);
            this.reportViewer1.TabIndex = 0;
            // 
            // cmDataSet1
            // 
            this.cmDataSet1.DataSetName = "CMDataSet";
            this.cmDataSet1.EnforceConstraints = false;
            this.cmDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // taxTableAdapter1
            // 
            this.taxTableAdapter1.ClearBeforeFill = true;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Tax";
            this.bindingSource1.DataSource = this.cmDataSet1;
            // 
            // tradeDataSet1
            // 
            this.tradeDataSet1.DataSetName = "TradeDataSet";
            this.tradeDataSet1.EnforceConstraints = false;
            this.tradeDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // taxOutTableAdapter1
            // 
            this.taxOutTableAdapter1.ClearBeforeFill = true;
            // 
            // bindingSource2
            // 
            this.bindingSource2.DataMember = "TaxOut";
            this.bindingSource2.DataSource = this.tradeDataSet1;
            // 
            // FrmTax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1014, 566);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmTax";
            this.Text = "세금계산서 인쇄";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmTax_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradeDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private CMDataSet cmDataSet1;
        private CMDataSetTableAdapters.TaxTableAdapter taxTableAdapter1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DataSets.TradeDataSet tradeDataSet1;
        private DataSets.TradeDataSetTableAdapters.TaxOutTableAdapter taxOutTableAdapter1;
        private System.Windows.Forms.BindingSource bindingSource2;
    }
}