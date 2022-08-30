namespace mycalltruck.Admin
{
    partial class FrmTaxSales2
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
            this.salesDataSet = new mycalltruck.Admin.DataSets.SalesDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.salesReport2TableAdapter = new mycalltruck.Admin.DataSets.SalesDataSetTableAdapters.SalesReport2TableAdapter();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.salesReport3TableAdapter = new mycalltruck.Admin.DataSets.SalesDataSetTableAdapters.SalesReport3TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.salesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // salesDataSet
            // 
            this.salesDataSet.DataSetName = "SalesDataSet";
            this.salesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesList2.rdlc";
            this.reportViewer1.LocalReport.ReportPath = "";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1014, 566);
            this.reportViewer1.TabIndex = 0;
            // 
            // salesReport2TableAdapter
            // 
            this.salesReport2TableAdapter.ClearBeforeFill = true;
            // 
            // bindingSource
            // 
            this.bindingSource.DataMember = "SalesReport2";
            this.bindingSource.DataSource = this.salesDataSet;
            // 
            // salesReport3TableAdapter
            // 
            this.salesReport3TableAdapter.ClearBeforeFill = true;
            // 
            // FrmTaxSales2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1014, 566);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmTaxSales2";
            this.Text = "거래명세서 인쇄";
            this.Load += new System.EventHandler(this.FrmTax_Load);
            ((System.ComponentModel.ISupportInitialize)(this.salesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DataSets.SalesDataSet salesDataSet;
        private DataSets.SalesDataSetTableAdapters.SalesReport2TableAdapter salesReport2TableAdapter;
        private System.Windows.Forms.BindingSource bindingSource;
        private DataSets.SalesDataSetTableAdapters.SalesReport3TableAdapter salesReport3TableAdapter;
    }
}