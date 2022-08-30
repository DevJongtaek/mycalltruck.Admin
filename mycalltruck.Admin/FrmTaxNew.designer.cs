namespace mycalltruck.Admin
{
    partial class FrmTaxNew
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.sTATS7ListTableAdapter = new mycalltruck.Admin.CMDataSetTableAdapters.STATS7ListTableAdapter();
            this.CMDataSet = new mycalltruck.Admin.CMDataSet();
            this.STATS7ListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.CMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STATS7ListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource2.Name = "STAT7List";
            reportDataSource2.Value = this.STATS7ListBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.TaxNew.rdlc";
            this.reportViewer1.LocalReport.ReportPath = "";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1014, 566);
            this.reportViewer1.TabIndex = 0;
            // 
            // sTATS7ListTableAdapter
            // 
            this.sTATS7ListTableAdapter.ClearBeforeFill = true;
            // 
            // CMDataSet
            // 
            this.CMDataSet.DataSetName = "CMDataSet";
            this.CMDataSet.EnforceConstraints = false;
            this.CMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // STATS7ListBindingSource
            // 
            this.STATS7ListBindingSource.DataMember = "STATS7List";
            this.STATS7ListBindingSource.DataSource = this.CMDataSet;
            // 
            // FrmTaxNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1014, 566);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmTaxNew";
            this.Text = "거래명세서 인쇄";
            this.Load += new System.EventHandler(this.FrmTax_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STATS7ListBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private CMDataSetTableAdapters.STATS7ListTableAdapter sTATS7ListTableAdapter;
        private System.Windows.Forms.BindingSource STATS7ListBindingSource;
        private CMDataSet CMDataSet;

    }
}