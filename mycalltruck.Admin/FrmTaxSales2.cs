using Microsoft.Reporting.WinForms;
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
    public partial class FrmTaxSales2 : Form
    {
        public int _SaleId;
        public string _HAmount;
        public string _Gubun;

        public FrmTaxSales2(int SaleId,string HAmount, string Gubun)
        {
            _SaleId = SaleId;
            _HAmount = HAmount;
            _Gubun = Gubun;

            InitializeComponent();
            WindowState = FormWindowState.Maximized;

        }
        public void PrintClient()
        {
            if (_Gubun == "N")
            {
                salesReport2TableAdapter.Fill(salesDataSet.SalesReport2, _SaleId);
                BindingSource tempBindingSource = new BindingSource(bindingSource, "");
                Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
                ("DataSet1", tempBindingSource);

                reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesList2.rdlc";

                reportViewer1.LocalReport.DataSources.Add(_Source);
                reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", _HAmount));
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            else
            {
                salesReport3TableAdapter.Fill(salesDataSet.SalesReport3, _SaleId);
                bindingSource.DataMember = "SalesReport3";
                BindingSource tempBindingSource = new BindingSource(bindingSource, "");
                Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
                ("DataSet1", tempBindingSource);

                reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.SalesList3.rdlc";

                reportViewer1.LocalReport.DataSources.Add(_Source);
                reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", _HAmount));
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
           
         
            


            reportViewer1.RefreshReport();
        }
       

        private void FrmTax_Load(object sender, EventArgs e)
        {
           
              

        }
    }
}
