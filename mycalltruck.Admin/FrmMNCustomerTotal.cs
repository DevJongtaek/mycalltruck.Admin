using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMNCustomerTotal : Form
    {
        public FrmMNCustomerTotal()
        {
            InitializeComponent();
        }

        private void FrmMNCustomerTotal_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.FPIS_CONT_Total' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            string Sdate = string.Empty;
            string Edate = string.Empty;

            Sdate = DateTime.Now.ToString("yyyy") + "-01-01";
            Edate = DateTime.Now.ToString("yyyy-MM-dd");
            this.fPIS_CONT_TotalTableAdapter.Fill(this.cMDataSet.FPIS_CONT_Total, Sdate.Replace("-", "/"), Edate.Replace("-", "/"), LocalUser.Instance.LogInInformation.ClientId);
            this.fpiS_CONTTableAdapter.Fill(this.cMDataSet.FPIS_CONT);
            this.clientUsersTableAdapter.FillForAdmin(this.cMDataSet.ClientUsers);

            _Search();

            label1.Text = DateTime.Now.ToString("yyyy") + "/01/01" + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
        }
        string I_TRU_DEPOSIT;

        private void _Search()
        {
            string _FilterString = string.Empty;

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                LocalUser.Instance.LogInInformation.LoadClient();
                List<string> VisibleOrderIds = new List<string>();

                if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {
                    var Query = cMDataSet.ClientUsers.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
                    if (Query.Any())
                    {

                        foreach (var item in Query)
                        {
                            VisibleOrderIds.Add("'" + item.LoginId + "'");
                        }


                    }


                    if (_FilterString != string.Empty)
                    {
                        if (VisibleOrderIds.Count == 0)
                        {
                            _FilterString += " AND Cliendid = '" + LocalUser.Instance.LogInInformation.Client.LoginId + "'";
                        }

                        else
                        {
                            _FilterString += " AND (Cliendid = '" + LocalUser.Instance.LogInInformation.Client.LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))";
                        }
                    }
                    else
                    {
                        if (VisibleOrderIds.Count == 0)
                        {
                            _FilterString += " Cliendid = '" + LocalUser.Instance.LogInInformation.Client.LoginId + "'";
                        }

                        else
                        {
                            _FilterString += " (Cliendid = '" + LocalUser.Instance.LogInInformation.Client.LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))";

                        }
                    }

                }
                else
                {
                    var Query = cMDataSet.ClientUsers.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
                    if (Query.Any())
                    {

                        foreach (var item in Query)
                        {
                            VisibleOrderIds.Add("'" + item.LoginId + "'");
                        }


                    }


                    if (_FilterString != string.Empty)
                    {
                        if (VisibleOrderIds.Count == 0)
                        {
                            _FilterString += " AND Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'";
                        }
                        else
                        {
                            _FilterString += " AND (Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))";
                        }
                    }
                    else
                    {

                        if (VisibleOrderIds.Count == 0)
                        {
                            _FilterString += " Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'";
                        }

                        else
                        {
                            _FilterString += " (Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))";
                        }
                    }

                }
            }
            try
            {
                fPISCONTTotalBindingSource.Filter = _FilterString;
             

            }
            catch
            {
                //btn_Search_Click(null, null);
            }
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
            }
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {

                e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));


            }
            else if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {

                e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));


            }
            else if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {

                e.Value = e.Value + "%";


            }
        }
    }
}
