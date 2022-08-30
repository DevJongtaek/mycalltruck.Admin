using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmTaxSales : Form
    {
        public int _SaleId;
        public string _HAmount;
      
        public FrmTaxSales(int SaleId,string HAmount)
        {
            _SaleId = SaleId;
            _HAmount = HAmount;
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

        }
        public void PrintClientDefault()
        {
            salesReportTableAdapter.Fill(salesDataSet.SalesReport, _SaleId);
            BindingSource tempBindingSource = new BindingSource(salesReportBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesList.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", _HAmount));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
           

            reportViewer1.RefreshReport();
        }
        public void PrintClientDefault1()
        {
            salesReportTableAdapter.Fill(salesDataSet.SalesReport, _SaleId);
            BindingSource tempBindingSource = new BindingSource(salesReportBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesList-1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", _HAmount));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
        }
        public void PrintClientRemark()
        {
            salesReportTableAdapter.Fill(salesDataSet.SalesReport, _SaleId);
            BindingSource tempBindingSource = new BindingSource(salesReportBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesListRemark.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", _HAmount));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
        }
        public void PrintClientRemark1()
        {
            salesReportTableAdapter.Fill(salesDataSet.SalesReport, _SaleId);
            BindingSource tempBindingSource = new BindingSource(salesReportBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesListRemark-1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", _HAmount));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
        }
        public void PrintClientDriver()
        {

            salesReportTableAdapter.Fill(salesDataSet.SalesReport, _SaleId);
            BindingSource tempBindingSource = new BindingSource(salesReportBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesListRemarkDriver.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", _HAmount));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
        }
        private void FrmTax_Load(object sender, EventArgs e)
        {
           
              

        }
    }
}
