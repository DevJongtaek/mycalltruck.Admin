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
    public partial class FrmTaxNew : Form
    {
        public int[] OrderIds { get; set; }
        
        string SSdate, SEdate, SSearchText, OrderIdsList;
        int SSGubun;
        public FrmTaxNew(int[] iOrderIds,string Sdate,string Edate,int SGubun,string SearchText)
        {
            OrderIds = iOrderIds;
            SSdate = Sdate;
            SEdate = Edate;
            SSGubun = SGubun;
            SSearchText = SearchText;
            OrderIdsList = string.Join(",", OrderIds.Select(c => "" + c + "").ToArray());
            InitializeComponent();
            this.sTATS7ListTableAdapter.Fill(this.CMDataSet.STATS7List, SSdate, SEdate, LocalUser.Instance.LogInInformation.ClientId);
            //foreach (var OrderId in OrderIds)
            //{
            //    this.sTATS7ListTableAdapter.Fill(this.CMDataSet.STATS7List, SSdate, SEdate, LocalUser.Instance.LogInInformation.ClientId);
            //   // taxTableAdapter1.Fill(cmDataSet1.Tax, TradeId);
            //}
        }

        public void PrintClient1()
        {
            //string filter = string.Format("CL_COMP_BSNS_NUM Like  '%{0}%' AND Num in ({1}) ", SSearchText, OrderIdsList);
            string filter = string.Format("Num in ({0}) ", OrderIdsList);
            BindingSource tempBindingSource = new BindingSource(STATS7ListBindingSource.Filter = filter, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("STATS7List", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.TaxNew.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
        }
        public void PrintClient()
        {
            string filter = string.Format("Num in ({0}) ",  OrderIdsList);
            BindingSource tempBindingSource = new BindingSource(STATS7ListBindingSource.Filter = filter, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("STATS7List", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.TaxNew.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
        }
        public void PrintClient2()
        {
            // string filter = string.Format("CL_COMP_NM Like  '%{0}%' AND Num in ({1}) ", SSearchText, OrderIdsList);
            string filter = string.Format("Num in ({0}) ", OrderIdsList);

            BindingSource tempBindingSource = new BindingSource(STATS7ListBindingSource.Filter = filter, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("STATS7List", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.TaxNew.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
        }

        private void FrmTax_Load(object sender, EventArgs e)
        {
            
            
        }
    }
}
