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
using System.IO;
using OfficeOpenXml;
using System.Diagnostics;
using OfficeOpenXml.Style;

namespace mycalltruck.Admin
{
    public partial class UCNSTATS2 : UserControl
    {
        public UCNSTATS2()
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



            Dictionary<string, string> SpayLocation = new Dictionary<string, string>
            {
                { "전체", "전체" },
                { "수금", "수금" },
                { "미수금", "미수금" },
               
            };


            cmbSDiv.DataSource = new BindingSource(SpayLocation, null);
            cmbSDiv.DisplayMember = "Value";
            cmbSDiv.ValueMember = "Key";

            cmbSDiv.SelectedIndex = 0;


            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {
                { "전체", "전체" },
                //{ "배차일시", "배차일시" },
                { "사업자번호", "사업자번호" },
                { "상호명", "상호명" },
            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


        }
        private String GetSelectCommand()
        {
            //            return @" SELECT  SalesId, SM.SangHo, RequestDate, Amount, 0 AS AcceptCount, Issue, ISNULL(IssueState, '') AS IssueState, CASE WHEN IssueState = '발행' THEN CONVERT(varchar(10), IssueDate, 111) 
            //               ELSE '' END AS IssueDate, InputPrice1, ISNULL(CONVERT(varchar(10), InputPrice1Date, 111), '') AS InputPrice1Date, InputPrice2, ISNULL(CONVERT(varchar(10), InputPrice2Date, 111), '') 
            //               AS InputPrice2Date, InputPrice3, ISNULL(CONVERT(varchar(10), InputPrice3Date, 111), '') AS InputPrice3Date, SM.BizNo,SM.ClientId ,Customers.MStartDate, Customers.MStart,Customers.Mizi,Customers.Misu
            //FROM     SalesManage AS SM left JOIN Customers ON SM.CustomerId = Customers.CustomerId";
            return @"SELECT  SM.SalesId, SM.SangHo, SM.RequestDate, SM.Amount, 0 AS AcceptCount, SM.Issue, ISNULL(SM.IssueState, '') AS IssueState, CASE WHEN IssueState = '발행' THEN CONVERT(varchar(10), 
               IssueDate, 111) ELSE '' END AS IssueDate, SM.InputPrice1, ISNULL(CONVERT(varchar(10), SM.InputPrice1Date, 111), '') AS InputPrice1Date, SM.InputPrice2, ISNULL(CONVERT(varchar(10), 
               SM.InputPrice2Date, 111), '') AS InputPrice2Date, SM.InputPrice3, ISNULL(CONVERT(varchar(10), SM.InputPrice3Date, 111), '') AS InputPrice3Date, SM.BizNo, SM.ClientId, 
               isnull(Customers.MStartDate,getdate()) as MStartDate ,isnull(Customers.MStart,0) as MStart, isnull(Customers.Mizi,0) as Mizi, isnull(Customers.Misu,0) as Misu
                ,SM.IssueLoginId, SM.IssueGubun
FROM     SalesManage AS SM LEFT OUTER JOIN
               Customers ON SM.CustomerId = Customers.CustomerId ";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS2.Clear();
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                    


                    WhereStringList.Add("CONVERT(VARCHAR(10),RequestDate,111) >= @Sdate AND CONVERT(VARCHAR(10),RequestDate,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate",dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);


                    if(cmbSDiv.SelectedIndex == 1)
                    {
                        WhereStringList.Add("SM.Amount = SM.InputPrice1+ SM.InputPrice2 + SM.InputPrice3 ");
                       
                    }
                    else if (cmbSDiv.SelectedIndex == 2)
                    {
                        WhereStringList.Add("SM.Amount != SM.InputPrice1+ SM.InputPrice2 + SM.InputPrice3 ");
                    }


                    WhereStringList.Add("SM.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                   // WhereStringList.Add("Orders.OrderStatus = 3 ");
                    

                    if (cmbSSearch.Text == "사업자번호")
                    {
                        WhereStringList.Add(string.Format("SM.BizNo Like '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "상호명")
                    {
                        WhereStringList.Add(string.Format("SM.SangHo Like  '%{0}%'", txtSText.Text));
                    }


                    WhereStringList.Add("IssueState = '발행'");

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by RequestDate  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS2.Load(_Reader);

                    }
                }

                
                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS2.ToArray();

            if (Query.Any())
            {
                if(cmbSSearch.SelectedIndex == 0)
                {
                    lblMStartDate.Text = "";
                }
                else
                {
                    lblMStartDate.Text = Query.First().MStartDate.ToString("d") + "/" + Query.First().MStart.ToString("N0");
                }
                //lblTradePrice.Text = Query.Sum(c => c.Amount).ToString("N0");
                
                lblIssuePrice.Text = Query.Where(c => c.IssueDate != "").Sum(c => c.Amount).ToString("N0");
                lblSTPrice.Text = (Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3)).ToString("N0");

                //세금계산서 발행금액중 미수금액
              //  var _Misu = Query.Where(c =>  c.IssueDate != "" &&  DateTime.Parse(c.IssueDate) >= Query.First().MStartDate ).ToArray();

                var _totalPrice = Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3);
                var _inputPrice = Query.Where(c => c.IssueDate != "" && DateTime.Parse(c.IssueDate).Date >= Query.First().MStartDate.Date).Sum(c => c.Amount) - _totalPrice;


                if (cmbSSearch.SelectedIndex == 0)
                {
                    lblmisu.Text = (Query.Where(c => c.IssueDate != "").Sum(c => c.Amount) - (Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3))).ToString("N0");
                }
                else
                {
                    //미수시작일이 없으면 : 발행금액 - 수금액 = 현재미수금
                    if(Query.First().MStart == 0)
                    {
                        lblmisu.Text = (Query.Where(c => c.IssueDate != "").Sum(c => c.Amount) - _totalPrice).ToString("N0");
                    }
                    //(미수시작금액+ 발행금액) - 수금액 = 현재미수금 (단, 미수시작일이후발행금액만 계산함)
                    else
                    {
                        lblmisu.Text = (Query.First().MStart + _inputPrice).ToString("N0");
                    }
                    
                }


                //lblAlterPrice.Text = (Query.Sum(c => c.Amount) - (Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3))).ToString("N0");
            }
        
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

      
      

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS2Row;


            if (ModelDataGrid.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (ModelDataGrid.Rows.Count - e.RowIndex).ToString("N0");
            }

            if (ModelDataGrid.Columns[e.ColumnIndex] == requestDateDataGridViewTextBoxColumn)
            {
                e.Value = Selected.RequestDate.ToString("d").Replace("-", "/");
            }

            if (ModelDataGrid.Columns[e.ColumnIndex] == acceptCountDataGridViewTextBoxColumn)
            {
                int acceptCount = 0;
                try
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        var Command1 = cn.CreateCommand();
                        Command1.CommandText = "SELECT count(*) FROM Orders where SalesManageId = " + Selected.SalesId + "";
                        var o1 = Command1.ExecuteScalar();
                        if (o1 != null)
                        {
                            acceptCount += Convert.ToInt32(o1);
                        }
                        cn.Close();
                    }
                    e.Value = acceptCount.ToString("N0");
                }
                catch
                {
                    e.Value = 0;
                }
            }

            if (ModelDataGrid.Columns[e.ColumnIndex] == inputPrice2DateDataGridViewTextBoxColumn)
            {
                if (Selected.InputPrice2 == 0)
                {
                    e.Value = "";
                }
                else
                {
                    e.Value = Selected.InputPrice2Date;
                }
            }
            if (ModelDataGrid.Columns[e.ColumnIndex] == inputPrice3DateDataGridViewTextBoxColumn)
            {
                if (Selected.InputPrice3 == 0)
                {
                    e.Value = "";
                }
                else
                {
                    e.Value = Selected.InputPrice3Date;
                }
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
        String _Sender = "";
        bool MethodProccess = false;
        private void nSTATS2BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            nSTATSDataSet.NSTATS2DETAIL.Clear();

            if (nSTATS2BindingSource.Current == null)

                return;


            if (((DataRowView)nSTATS2BindingSource.Current).Row is NSTATSDataSet.NSTATS2Row Selected)
            {
              

                MethodProccess = true;
              //  _Sender = "salesManageBindingSource";
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                      
                        _Command.CommandText = "SELECT  Orders.AcceptTime, Customers.SangHo, Orders.PayLocation, PayLocation.Name AS PayLocationName, Orders.DriverCarNo" +
                            ", ISNULL(Orders.StartName, '') AS StartName,    ISNULL(Orders.StopName, '') AS StopName, ISNULL(Orders.Item, '') AS Item, ISNULL(Orders.TradePrice, 0) AS TradePrice" +
                            ", ISNULL(Orders.SalesPrice, 0) AS SalesPrice,ISNULL(Orders.SalesPrice, 0) -ISNULL(Orders.TradePrice, 0) AS TSPrice, ISNULL(Orders.DriverPrice, 0) AS DriverPrice" +
                            ", CASE WHEN PayLocation = 1 THEN '포함' ELSE '미포함' END AS PayLocationUse, ISNULL(CONVERT(varchar(10), SalesManage.IssueDate, 111), '') AS IssueDate,  Orders.SalesManageId  " +
                            " FROM     Orders LEFT OUTER JOIN" +
                            " Customers ON Orders.ReferralId = Customers.CustomerId INNER JOIN" +
                            " StaticOptions AS PayLocation ON Orders.PayLocation = PayLocation.Value AND PayLocation.Div = 'PayLocation' " +
                            " LEFT OUTER JOIN SalesManage ON Orders.SalesManageId = SalesManage.SalesId WHERE SalesManageId = @SalesId";

                        _Command.Parameters.AddWithValue("@SalesId", Selected.SalesId);
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            nSTATSDataSet.NSTATS2DETAIL.Load(_Reader);

                         
                        }
                    }
                    _Connection.Close();
                }
              

               
            }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS2DETAILRow;


            if (newDGV1.Columns[e.ColumnIndex] == dataGridViewTextBoxColumn1)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0");
            }
            if (newDGV1.Columns[e.ColumnIndex] == acceptTimeDataGridViewTextBoxColumn)
            {
                if (e.Value != null)
                {
                    e.Value = Selected.AcceptTime.ToString("d").Replace("-", "/");
                }
            }
            if (newDGV1.Columns[e.ColumnIndex] == ClientNameD)
            {
                e.Value = LocalUser.Instance.LogInInformation.Client.CEO;
            }

        }

        public void btn_Inew_Click(object sender, EventArgs e)
        {
            cmbSMonth.SelectedIndex = 3;
            dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
            dtp_Edate.Value = DateTime.Now;
            cmbSDiv.SelectedIndex = 0;
            cmbSSearch.SelectedIndex = 0;
            txtSText.Clear();
            btnSearch_Click(null, null);


        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {

            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;

            var fileString = "거래처정산내역_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.CustomerOrderList))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var ColumnIndexMap = new Dictionary<int, int>();

            List<string> ColumnHeaderMap = new List<string>();
            var ColumnIndex = 0;
            for (int i = 0; i < ModelDataGrid.ColumnCount; i++)
            {
                if (ModelDataGrid.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < ModelDataGrid.ColumnCount; i++)
            {
                if (ModelDataGrid.Columns[i].Visible)
                {
                    ColumnHeaderMap.Add(ModelDataGrid.Columns[i].HeaderText);
                }

            }


            for (int i = 0; i < ColumnHeaderMap.Count; i++)
            {

                _Sheet.Cells[1, i + 1].Value = ColumnHeaderMap[i];

                _Sheet.Cells[RowIndex, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            }
            for (int i = 0; i < ModelDataGrid.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = ModelDataGrid[ColumnIndexMap[j], i].FormattedValue?.ToString();

                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    _Sheet.Cells[1, j + 1].AutoFitColumns();
                }

                RowIndex++;
            }


            try
            {

                _Excel.SaveAs(file);
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }

            System.Diagnostics.Process.Start(FileName);



           
        }
        //private void ExcelExportBasic()
        //{
        //    DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

        //    if (di.Exists == false)
        //    {

        //        di.Create();
        //    }
        //    var fileString = "매출화주정산" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
        //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
        //    FileInfo file = new FileInfo(FileName);
        //    if (file.Exists)
        //    {
        //        if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        //        {
        //            return;


        //        }


        //    }

        //    ExcelPackage _Excel = null;
        //    using (MemoryStream _Stream = new MemoryStream(Properties.Resources.매출화주정산양식_Blank))
        //    {
        //        _Stream.Seek(0, SeekOrigin.Begin);
        //        _Excel = new ExcelPackage(_Stream);
        //    }
        //    var _Sheet = _Excel.Workbook.Worksheets[1];
        //    var RowIndex = 2;
        //    for (int i = 0; i < ModelDataGrid.RowCount; i++)
        //    {
        //        _Sheet.Cells[RowIndex, 1].Value = ModelDataGrid[rowNUMDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 2].Value = ModelDataGrid[sangHoDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 3].Value = ModelDataGrid[requestDateDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 4].Value = ModelDataGrid[amountDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 5].Value = ModelDataGrid[acceptCountDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 6].Value = ModelDataGrid[issueDateDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 7].Value = ModelDataGrid[inputPrice1DateDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 8].Value = ModelDataGrid[inputPrice1DataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 9].Value = ModelDataGrid[inputPrice2DateDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 10].Value = ModelDataGrid[inputPrice2DataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 11].Value = ModelDataGrid[inputPrice3DateDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 12].Value = ModelDataGrid[inputPrice3DataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        RowIndex++;
        //    }

        //    try
        //    {
        //        _Excel.SaveAs(file);
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
        //        return;
        //    }
        //    System.Diagnostics.Process.Start(FileName);
        //}

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372111212");
        }
    }
}
