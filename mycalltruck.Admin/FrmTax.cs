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
    public partial class FrmTax : Form
    {
        public int[] TradeIds { get; set; }
        public FrmTax(int[] iTradeIds,String Gubun = "")
        {
            TradeIds = iTradeIds;
            InitializeComponent();
            taxTableAdapter1.ClearBeforeFill = false;
            taxOutTableAdapter1.ClearBeforeFill = false;

            if (Gubun=="New")
            {
                foreach (var TradeId in TradeIds)
                {
                    taxOutTableAdapter1.Fill(tradeDataSet1.TaxOut, TradeId);
                }
            }
            else
            {
                if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
                {
                    foreach (var TradeId in TradeIds)
                    {
                        taxTableAdapter1.FillByCustomerPay(cmDataSet1.Tax, TradeId);
                    }
                }
                else
                {
                    foreach (var TradeId in TradeIds)
                    {
                        taxTableAdapter1.Fill(cmDataSet1.Tax, TradeId);
                    }
                }

            }
            WindowState = FormWindowState.Maximized;
            
        }

        public void PrintClient()
        {
            BindingSource tempBindingSource = new BindingSource(bindingSource1, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Tax", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Tax.Client.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            



            reportViewer1.RefreshReport();
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;


        }
        public void PrintOut()
        {
            BindingSource tempBindingSource = new BindingSource(bindingSource2, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("TaxOut", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Tax.Out.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


          

            reportViewer1.RefreshReport();
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;


        }

        public void PrintDriver()
        {
            BindingSource tempBindingSource = new BindingSource(bindingSource1, "");
            Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource
            ("Tax", tempBindingSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "mycalltruck.Admin.Tax.Driver.rdlc";
            reportViewer1.LocalReport.DataSources.Add(_Source);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


            reportViewer1.RefreshReport();
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;
        }

        private void FrmTax_Load(object sender, EventArgs e)
        {
            var _Location = this.Location;
            var _Height = this.Height;
            WindowState = FormWindowState.Maximized;
            this.Left = _Location.X;
            this.Top = _Location.Y;
            Height = _Height;
            Width = (int)(_Height * 0.8d);
           
        }
    }
}
