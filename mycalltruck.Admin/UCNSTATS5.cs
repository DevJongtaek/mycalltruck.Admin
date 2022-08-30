using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using System.Diagnostics;

namespace mycalltruck.Admin
{
    public partial class UCNSTATS5 : UserControl
    {
        public UCNSTATS5()
        {
            InitializeComponent();
        }

        private void UCNSTATS1_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now;
            dtp_Edate.Value = DateTime.Now;
            InitCmb();
            LoadTable();
        }

        public void InitCmb()
        {
            Dictionary<string, string> Smonth = new Dictionary<string, string>
            {
                { "당일", "당일" },
                { "전일", "전일" },
                { "금주", "금주" },
                { "금월", "금월" },
                { "전월", "전월" },
                { "지정", "지정" }
            };

            cmbSMonth.DataSource = new BindingSource(Smonth, null);
            cmbSMonth.DisplayMember = "Value";
            cmbSMonth.ValueMember = "Key";

            cmbSMonth.SelectedIndex = 3;


            Dictionary<int, string> DCustomer = new Dictionary<int, string>();
            customersTableAdapter.FillBy(clientDataSet.Customers, LocalUser.Instance.LogInInformation.ClientId);
            var cmbCustomerMIdDataSource = clientDataSet.Customers.Where(c => c.BizGubun == 4).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();

            DCustomer.Add(0, "전체");


            foreach (var item in cmbCustomerMIdDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }


            cmbSReferralId.DataSource = new BindingSource(DCustomer, null);
            cmbSReferralId.DisplayMember = "Value";
            cmbSReferralId.ValueMember = "Key";
            cmbSReferralId.SelectedValue = 0;







            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {
                { "전체", "전체" },
                //{ "배차일시", "배차일시" },
                { "화주(상호)", "화주(상호)" },
                { "화주(사업자번호)", "화주(사업자번호)" },
                { "차주(상호)", "차주(상호)" },
                { "차주(사업자번호)", "차주(사업자번호)" },
                 { "차주(차량번호)", "차주(차량번호)" }

            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


        }
        private String GetSelectCommand()
        {
            return @"SELECT  PayDate, Gubun, SangHo, BizNo, CarNo, Amount, AOAmount, ClientId, ReferralId, ReferralName,OutAmount
FROM     (SELECT  SalesManage.PayDate, '화주' AS Gubun, SalesManage.SangHo, SalesManage.BizNo, '' AS CarNo, SalesManage.Amount, 0 AS OutAmount, SalesManage.Amount - 0 AS AOAmount, 
                               SalesManage.ClientId,0 as ReferralId, '' AS ReferralName
                FROM     SalesManage INNER JOIN
                               Orders ON SalesManage.SalesId = Orders.SalesManageId LEFT OUTER JOIN
                               Customers ON Orders.ReferralId = Customers.CustomerId
                WHERE  (SalesManage.PayState = 1)
                group by SalesManage.PayDate,SalesManage.SangHo, SalesManage.BizNo, SalesManage.Amount,SalesManage.ClientId
                UNION ALL
                SELECT  Trades.PayDate, '차주' AS Gubun, Drivers.Name, Drivers.BizNo, Drivers.CarNo, 0 AS Expr1, Trades.Amount, 0 - Trades.Amount AS Expr2, Trades.ClientId, 0 AS Expr3, 
                               '' AS Expr4
                FROM     Trades INNER JOIN
                               Drivers ON Trades.DriverId = Drivers.DriverId
                WHERE  (Trades.PayState = 1)) AS a";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS5.Clear();
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                    //if (cmbSReferralId.SelectedIndex > 0)
                    //{
                    //    WhereStringList.Add("ReferralName = @ReferralName ");
                    //    _Command.Parameters.AddWithValue("@ReferralName", cmbSReferralId.Text);
                    //}


                  


                    WhereStringList.Add("ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    WhereStringList.Add("CONVERT(VARCHAR(10),A.PayDate,111) >= @Sdate AND CONVERT(VARCHAR(10),A.PayDate,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate",dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    
                    

                    if (cmbSSearch.Text == "화주(상호)")
                    {
                        if (!String.IsNullOrEmpty(txtSText.Text))
                        {
                            WhereStringList.Add("Gubun='화주'");
                            WhereStringList.Add(string.Format("SangHo Like '%{0}%'", txtSText.Text));
                        }
                    }
                    else if (cmbSSearch.Text == "화주(사업자번호)")
                    {
                        if (!String.IsNullOrEmpty(txtSText.Text))
                        {
                            WhereStringList.Add("Gubun='화주'");
                            WhereStringList.Add(string.Format("BizNo Like  '%{0}%'", txtSText.Text));
                        }
                    }
                    else if (cmbSSearch.Text == "차주(상호)")
                    {
                        if (!String.IsNullOrEmpty(txtSText.Text))
                        {
                            WhereStringList.Add("Gubun='차주'");
                            WhereStringList.Add(string.Format("SangHo Like  '%{0}%'", txtSText.Text));
                        }
                    }
                    else if (cmbSSearch.Text == "차주(사업자번호)")
                    {
                        if (!String.IsNullOrEmpty(txtSText.Text))
                        {
                            WhereStringList.Add("Gubun='차주'");
                            WhereStringList.Add(string.Format("BizNo Like  '%{0}%'", txtSText.Text));
                        }
                    }
                    else if (cmbSSearch.Text == "차주(차량번호)")
                    {
                        if (!String.IsNullOrEmpty(txtSText.Text))
                        {
                            WhereStringList.Add("Gubun='차주'");
                            WhereStringList.Add(string.Format("CarNo Like  '%{0}%'", txtSText.Text));
                        }
                    }



                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by PayDate  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS5.Load(_Reader);

                    }
                }

                
                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS5.ToArray();

            if (Query.Any())
            {
                lblInputSum.Text = Query.Sum(c => c.Amount).ToString("N0");
                lblOutSum.Text = Query.Sum(c => c.OutAmount).ToString("N0");
                lblAOSum.Text = Query.Sum(c => c.AOAmount).ToString("N0");
            }
            //lblTradePrice.Text = Query.Sum(c => c.TradePrice).ToString("N0");
            //lblSalesPrice.Text = Query.Sum(c => c.SalesPrice).ToString("N0");
            //lblSTPrice.Text = (Query.Sum(c => c.TradePrice) -  Query.Sum(c => c.SalesPrice)).ToString("N0");
            //lblAlterPrice.Text = Query.Sum(c => c.AlterPrice).ToString("N0");
            //lblStartPrice.Text = Query.Sum(c => c.StartPrice).ToString("N0");
            //lblStopPrice.Text = Query.Sum(c => c.StopPrice).ToString("N0");
            //lblDriverPrice.Text = Query.Sum(c => c.DriverPrice).ToString("N0");
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ModelDataGrid.RowCount < 1)
            {
                MessageBox.Show("출력할 내용이 없습니다. 먼저 데이터를 조회하신 후 출력 버튼을 눌려주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            FrmNStatistics f = new FrmNStatistics();
            
            string jogun = string.Empty;

            jogun = "위탁사 : " + cmbSReferralId.Text + "  " + cmbSSearch.Text + txtSText.Text;

            f.NSTATS5(dtp_Sdate.Text, dtp_Edate.Text, jogun, nSTATS5BindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();


            
        }

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS5Row;


            if (ModelDataGrid.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (ModelDataGrid.Rows.Count - e.RowIndex).ToString("N0");
            }

            //else if (e.ColumnIndex == acceptTimeDataGridViewTextBoxColumn.Index)
            //{
            //    e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            //}
        }

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMonth.SelectedIndex)
            {
                //당일
                case 0:
                    dtp_Sdate.Value = DateTime.Now;
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전일
                case 1:
                    dtp_Sdate.Value = DateTime.Now.AddDays(-1);
                    dtp_Edate.Value = DateTime.Now.AddDays(-1);
                    break;
                //금주
                case 2:
                    dtp_Sdate.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //금월
                case 3:
                    dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전월
                case 4:
                    dtp_Sdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 5:
                    dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                    dtp_Edate.Value = DateTime.Now;
                    break;
            }
        }

        private void txtSText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            LoadTable();
        }

        public void btn_Inew_Click(object sender, EventArgs e)
        {
            cmbSMonth.SelectedIndex = 3;
            dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
            dtp_Edate.Value = DateTime.Now;
            cmbSReferralId.SelectedIndex = 0;
            cmbSSearch.SelectedIndex = 0;
            txtSText.Clear();
            btnSearch_Click(null, null);

        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372111212");
        }
    }
}
