namespace mycalltruck.Admin
{
    partial class FrmTaxSales
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
            this.salesReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.salesDataSet = new mycalltruck.Admin.DataSets.SalesDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.salesReportTableAdapter = new mycalltruck.Admin.DataSets.SalesDataSetTableAdapters.SalesReportTableAdapter();
            this.salesReport2TableAdapter = new mycalltruck.Admin.DataSets.SalesDataSetTableAdapters.SalesReport2TableAdapter();
            this.salesReportBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.salesReportBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesReportBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // salesReportBindingSource
            // 
            this.salesReportBindingSource.DataMember = "SalesReport";
            this.salesReportBindingSource.DataSource = this.salesDataSet;
            // 
            // salesDataSet
            // 
            this.salesDataSet.DataSetName = "SalesDataSet";
            this.salesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesList.rdlc";
            this.reportViewer1.LocalReport.ReportPath = "";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1014, 566);
            this.reportViewer1.TabIndex = 0;
            // 
            // salesReportTableAdapter
            // 
            this.salesReportTableAdapter.ClearBeforeFill = true;
            // 
            // salesReport2TableAdapter
            // 
            this.salesReport2TableAdapter.ClearBeforeFill = true;
            // 
            // salesReportBindingSource2
            // 
            this.salesReportBindingSource2.DataMember = "SalesReport2";
            this.salesReportBindingSource2.DataSource = this.salesDataSet;
            // 
            // FrmTaxSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1014, 566);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmTaxSales";
            this.Text = "거래명세서 인쇄";
            this.Load += new System.EventHandler(this.FrmTax_Load);
            ((System.ComponentModel.ISupportInitialize)(this.salesReportBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesReportBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource salesReportBindingSource;
        private DataSets.SalesDataSet salesDataSet;
        private DataSets.SalesDataSetTableAdapters.SalesReportTableAdapter salesReportTableAdapter;
        private DataSets.SalesDataSetTableAdapters.SalesReport2TableAdapter salesReport2TableAdapter;
        private System.Windows.Forms.BindingSource salesReportBindingSource2;
    }
}