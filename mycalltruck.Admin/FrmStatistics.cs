using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmStatistics : Form
    {
        BindingSource _BindingSource = new BindingSource();
        public FrmStatistics()
        {
            InitializeComponent();
        }

        public void STATS1(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Stats1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }

        public void STATS2(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Stats2", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS2.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }

        public void STATS3(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource,string Filter,string FilterText)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
           
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Stats3", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS3.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }
        public void STATS4(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Stats4", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS4.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }
        public void STATS5(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Stats5", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS5.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }
        public void STATS6(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Stats6", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS6.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }
    }
}
