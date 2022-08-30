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
    public partial class FrmNStatistics : Form
    {
        BindingSource _BindingSource = new BindingSource();
        public FrmNStatistics()
        {
            InitializeComponent();
        }

        public void NSTATS1(string dtpStart, string dtpEnd,string jogun,  BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.NSTATS1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("Gigan", dtpStart +'-'+dtpEnd));
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("Jogun", jogun));
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("PrintDate", DateTime.Now.ToString("D")));
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName","발행  : "+ LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }

        public void NSTATS5(string dtpStart, string dtpEnd, string jogun, BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.NSTATS5.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("Gigan", dtpStart + '-' + dtpEnd));
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("Jogun", jogun));
            
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName",  LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }
        public void NSTATS7(string dtpStart, string dtpEnd, BindingSource iBindingSource)
        {
            BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("DataSet1", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.NSTATS7.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("Gigan", dtpStart + '-' + dtpEnd));
            

            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        }
        private void FrmNStatistics_Load(object sender, EventArgs e)
        {

        }

        //public void STATS2(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        //{
        //    BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
        //    Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
        //    ("Stats2", tempBindingSource);
        //    reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS2.rdlc";
        //    reportViewer1.LocalReport.DataSources.Add(_Source);
        //    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
        //    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        //}

        //public void STATS3(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource,string Filter,string FilterText)
        //{
        //    BindingSource tempBindingSource = new BindingSource(iBindingSource, "");

        //    Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
        //    ("Stats3", tempBindingSource);
        //    reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS3.rdlc";
        //    reportViewer1.LocalReport.DataSources.Add(_Source);
        //    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
        //    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        //}
        //public void STATS4(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        //{
        //    BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
        //    Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
        //    ("Stats4", tempBindingSource);
        //    reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS4.rdlc";
        //    reportViewer1.LocalReport.DataSources.Add(_Source);
        //    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
        //    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        //}
        //public void STATS5(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        //{
        //    BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
        //    Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
        //    ("Stats5", tempBindingSource);
        //    reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS5.rdlc";
        //    reportViewer1.LocalReport.DataSources.Add(_Source);
        //    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
        //    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        //}
        //public void STATS6(string dtpStart, string dtpEnd, string order, BindingSource iBindingSource)
        //{
        //    BindingSource tempBindingSource = new BindingSource(iBindingSource, "");
        //    Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
        //    ("Stats6", tempBindingSource);
        //    reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Report.STATS6.rdlc";
        //    reportViewer1.LocalReport.DataSources.Add(_Source);
        //    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ClientName", LocalUser.Instance.LogInInformation.ClientName));
        //    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

        //}
    }
}
