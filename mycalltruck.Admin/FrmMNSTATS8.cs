using Microsoft.Reporting.WinForms;
using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMNSTATS8 : Form
    {
        ReportParameter CardSum = null;
        ReportParameter DateTitle = null;

        public FrmMNSTATS8()
        {
            InitializeComponent();
        }

        private void FrmMNSTATS1_Load(object sender, EventArgs e)
        {
            var Now = DateTime.Now;
            if(DateTime.Now.Month > 3 && DateTime.Now.Month < 7 )
            {
                dtp_Sdate.Value = new DateTime(Now.Year, 1, 1);
                dtp_Edate.Value = new DateTime(Now.Year, 4, 1).AddSeconds(-1);
            }
            else if (DateTime.Now.Month > 6 && DateTime.Now.Month < 10)
            {
                dtp_Sdate.Value = new DateTime(Now.Year, 4, 1);
                dtp_Edate.Value = new DateTime(Now.Year, 7, 1).AddSeconds(-1);
            }
            else if (DateTime.Now.Month > 9)
            {
                dtp_Sdate.Value = new DateTime(Now.Year, 7, 1);
                dtp_Edate.Value = new DateTime(Now.Year, 10, 1).AddSeconds(-1);
            }
            else
            {
                dtp_Sdate.Value = new DateTime(Now.Year-1, 10, 1);
                dtp_Edate.Value = new DateTime(Now.Year, 1, 1).AddSeconds(-1);
            }

            mViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            mViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
            LocalUser.Instance.LogInInformation.LoadClient();
            CardSum = new ReportParameter("CardSum", "0");
            DateTitle = new ReportParameter("DateTitle", "0");
            var Address = String.Join(" ",new String[] { LocalUser.Instance.LogInInformation.Client.AddressState, LocalUser.Instance.LogInInformation.Client.AddressCity, LocalUser.Instance.LogInInformation.Client.AddressDetail });
            mViewer.LocalReport.SetParameters(new ReportParameter("ClientName", LocalUser.Instance.LogInInformation.Client.Name));
            mViewer.LocalReport.SetParameters(new ReportParameter("ClientCEO", LocalUser.Instance.LogInInformation.Client.CEO));
            mViewer.LocalReport.SetParameters(new ReportParameter("ClientAddress", Address));
            mViewer.LocalReport.SetParameters(new ReportParameter("ClientBizNo", LocalUser.Instance.LogInInformation.Client.BizNo));
            mViewer.LocalReport.SetParameters(CardSum);
            mViewer.LocalReport.SetParameters(DateTitle);
            Search();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            DateTitle.Values[0] = String.Format("{0}년 ( {1:00} 월 {2:00} 일 ~ {3:00} 월 {4:00} 일 )", dtp_Sdate.Value.Year, dtp_Sdate.Value.Month, dtp_Sdate.Value.Day, dtp_Edate.Value.Month, dtp_Edate.Value.Day);
            using (SqlConnection mConnection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                mConnection.Open();
                SqlCommand mCommand = mConnection.CreateCommand();
                mCommand.CommandText = "SELECT SUM(Amount) FROM Trades WHERE ClientId = @ClientId AND PayState = 1 AND HasAcc = 1 AND PayDate >= @Start AND PayDate <= @End";
                mCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                mCommand.Parameters.AddWithValue("@Start", dtp_Sdate.Value);
                mCommand.Parameters.AddWithValue("@End", dtp_Edate.Value);
                var o = mCommand.ExecuteScalar();
                if (o != null && !(o is DBNull))
                    CardSum.Values[0] = Convert.ToInt32(mCommand.ExecuteScalar()).ToString("N0");
                else
                    CardSum.Values[0] = "0";
            }
            mViewer.LocalReport.SetParameters(CardSum);
            mViewer.LocalReport.SetParameters(DateTitle);
            mViewer.RefreshReport();
        }

    }
}
