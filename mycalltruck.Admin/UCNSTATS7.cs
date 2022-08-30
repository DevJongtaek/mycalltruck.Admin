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
    public partial class UCNSTATS7 : UserControl
    {
        public UCNSTATS7()
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


          








        }
        private String GetSelectCommand()
        {
            return @"SELECT CONVERT(varchar(10), Orders.CreateTime,111) as CreateTime
                      ,SUM( CASE WHEN Orders.PayLocation = 1 THEN 1  ELSE 0 END ) AS Paylocation1  
                     ,SUM( CASE WHEN Orders.PayLocation = 2 THEN 1 ELSE 0 END ) AS Paylocation2
                     ,SUM( CASE WHEN Orders.PayLocation = 4 THEN 1 ELSE 0 END ) AS Paylocation4  
                     ,SUM( CASE WHEN Orders.PayLocation = 5 THEN 1 ELSE 0 END ) AS Paylocation5
                      ,SUM(Orders.TradePrice)as TradesAmount
                     ,SUM(Orders.SalesPrice )as SalesAmount
                     ,SUM(ISNULL(DriverPrice,0))as ReferralAccountAmount
                     FROM Orders
                     LEFT JOIN Trades ON Orders.TradeId = Trades.TradeId
                     LEFT JOIN SalesManage ON Orders.SalesManageId = SalesManage.SalesId
                     LEFT JOIN ReferralAccount ON ORders.ReferralAccountId =  ReferralAccount.idx
                    ";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS7.Clear();
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();



                    WhereStringList.Add("Orders.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    WhereStringList.Add("CONVERT(VARCHAR(10),Orders.CreateTime,111) >= @Sdate AND CONVERT(VARCHAR(10),Orders.CreateTime,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate",dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    
                    

                  


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " GROUP BY CONVERT(varchar(10), Orders.CreateTime,111)  ORDER BY CONVERT(varchar(10), Orders.CreateTime,111) DESC ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS7.Load(_Reader);

                    }
                }

                
                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS7.ToArray();

            if (Query.Any())
            {
                lblPaylocation1.Text = Query.Sum(c=> c.Paylocation1).ToString();
                lblPaylocation2.Text = Query.Sum(c => c.Paylocation2).ToString();
                lblPaylocation4.Text = Query.Sum(c => c.Paylocation4).ToString();
                lblPaylocation5.Text = Query.Sum(c => c.Paylocation5).ToString();

                lblOrderCount.Text = (Query.Sum(c => c.Paylocation1) + Query.Sum(c => c.Paylocation2) + Query.Sum(c => c.Paylocation4) + Query.Sum(c => c.Paylocation5)).ToString();

                lblTradeAmount.Text = Query.Sum(c => c.TradesAmount).ToString("N0");
                lblSalesAmount.Text = Query.Sum(c => c.SalesAmount).ToString("N0");
                lblRAmount.Text = Query.Sum(c => c.ReferralAccountAmount).ToString("N0");

              
            }
            
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

           // jogun = "위탁사 : " + cmbSReferralId.Text + "  " + cmbSSearch.Text + txtSText.Text;

            f.NSTATS7(dtp_Sdate.Text, dtp_Edate.Text,nSTATS7BindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();



        }

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS7Row;


            if (ModelDataGrid.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (ModelDataGrid.Rows.Count - e.RowIndex).ToString("N0");
            }

            
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
            
          
            btnSearch_Click(null, null);

        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372111212");
        }
        string _SCreateTime = "";
        private void ModelDataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //if (e.RowIndex < 0)
            //    return;
            //var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS7Row;
            //_SCreateTime = Selected.CreateTime;

            //FrmMDI.LoadForm("FrmMN0301", "Order", 0,_SCreateTime);
        }

        private void ModelDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS7Row;
            _SCreateTime = Selected.CreateTime;

            FrmMDI.LoadForm("FrmMN0301", "Order", 0, _SCreateTime);
        }
    
    }
}
