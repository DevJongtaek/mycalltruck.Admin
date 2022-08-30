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
    public partial class UCNSTATS4 : UserControl
    {
        public UCNSTATS4()
        {
            InitializeComponent();
        }

        private void UCNSTATS1_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
            dtp_Edate.Value = DateTime.Now;
            InitCmb();
          //  LoadTable();
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

            cmbSMonth.SelectedIndex = 5;







            Dictionary<string, string> SpayLocation = new Dictionary<string, string>
            {
                { "전체", "전체" },
                { "지급", "지급" },
                { "미지급", "미지급" },

            };


            cmbSDiv.DataSource = new BindingSource(SpayLocation, null);
            cmbSDiv.DisplayMember = "Value";
            cmbSDiv.ValueMember = "Key";


            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {

                 { "전체", "전체" },
                { "차량번호", "차량번호" },
                { "차주명", "차주명" },


            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


            Dictionary<string, string> Dsearch = new Dictionary<string, string>
            {

                 { "배차일", "배차일" },
                { "접수일", "접수일" },
                { "결제일", "결제일" },


            };


            cmbDataGubun.DataSource = new BindingSource(Dsearch, null);
            cmbDataGubun.DisplayMember = "Value";
            cmbDataGubun.ValueMember = "Key";


            cmbDataGubun.SelectedIndex = 0;

            Dictionary<string, string> DTax = new Dictionary<string, string>
            {

                { "전체", "전체" },
                { "계산서발행", "계산서발행" },
                { "계산서미발행", "계산서미발행" },


            };

            cmbSTax.DataSource = new BindingSource(DTax, null);
            cmbSTax.DisplayMember = "Value";
            cmbSTax.ValueMember = "Key";


        }
        private String GetSelectCommand()
        {
            return @"     
				   SELECT 
				   ISNULL(CONVERT(varchar,Orders.CreateTime,111),N'') as AcceptTime
				   ,Drivers.DriverId
                    ,Drivers.CarNo	as DriverCarNo
                    ,Drivers.CarYear as Driver
                    ,ISNULL(Orders.StartName,'') StartName
                    ,ISNULL(Orders.StopName,'')StopName
                    ,ISNULL(Orders.Item,'')Item
                    ,ISNULL(Orders.Customer,'')as Customer
				 
                    ,ISNULL(Orders.ReferralId,0) as ReferralId
                    ,ISNULL(Customers.SangHo,'')as SangHo
					,ISNULL(Orders.PayLocation,0) as PayLocation
					
                    , CASE 
					WHEN Orders.PayLocation IS NULL THEN '' 
					WHEN Orders.PayLocation = 1 THEN  '인수증' 
					WHEN Orders.PayLocation = 4 THEN  '인수증+선/착불' 
					WHEN Orders.PayLocation = 5 THEN  '지입배차' 
				    END AS PayLocationName

					,CASE WHEN Orders.PayLocation IS NULL THEN '' WHEN Orders.PayLocation  = 5 THEN  '' ELSE '포함' END AS PayLocationP
					, Trades.RequestDate as TRequestDate
                    ,   ISNULL(Orders.TradePrice,0)  TradePrice
                    ,  CASE WHEN Orders.PayLocation IS NULL THEN Trades.Amount else ISNULL(Orders.SalesPrice,0) END SalesPrice
                   
                    ,CASE WHEN Orders.PayLocation IS NULL THEN Trades.Amount WHEN Orders.PayLocation  = 5 THEN  0 else ISNULL(Orders.SalesPrice,0) - ISNULL(Orders.TradePrice,0) end as  TSPrice
                    ,Case WHEN Trades.PayState = 1  then Convert(varchar(10),Trades.PayDate,111) else '' end as PayDate
                    ,Case WHEN Trades.PayState = 1 AND  Trades.HasAcc = 1 then '카드' else '현금' end as PayText
                    ,'' as ClientName
                    ,Case WHEN Trades.PayState = 1  then Trades.Amount else 0 end as Amount
                    ,isnull(DriverInstances.MStartDate,getdate()) as MStartDate, ISNULL(DriverInstances.MStart,0) as MStart,ISNULL(DriverInstances.Mizi,0) AS Mizi,ISNULL(DriverInstances.Misu,0) as Misu
                    ,orders.OrderId
					,trades.TradeId
                    , Trades.HasETax, Trades.AllowAcc, Trades.SourceType
                    ,ISNULL(Orders.TaxTradeId,0) as TaxTradeId
                    , Drivers.BizNo, Drivers.ServiceState
				    FROM Trades
					left JOIN Orders ON Orders.TradeId = Trades.TradeId AND (Orders.PayLocation  in(1,4,5)) AND Orders.OrderStatus = 3 
				    JOIN Drivers ON Trades.DriverId  = Drivers.DriverId
					LEFT JOIN Customers ON Orders.ReferralId = Customers.CustomerId
					LEFT JOIN DriverInstances ON Trades.DriverId = DriverInstances.DriverId AND DriverInstances.ClientId = Trades.ClientId";
           


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS4.Clear();
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                 

                    if (cmbSDiv.SelectedIndex == 1)
                    {
                        WhereStringList.Add("Trades.PayState = 1 ");
                      
                    }
                    else if(cmbSDiv.SelectedIndex == 2)
                    {
                        WhereStringList.Add("Trades.PayState != 1 ");

                    }

                    if(cmbSTax.SelectedIndex ==1)
                    {
                        WhereStringList.Add("ISNULL(Orders.TaxTradeId,0) != 0 ");
                    }
                    else if (cmbSTax.SelectedIndex == 2)
                    {
                        WhereStringList.Add("ISNULL(Orders.TaxTradeId,0) = 0 ");
                    }
                    WhereStringList.Add("Trades.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    switch(cmbDataGubun.Text)
                    {
                        case "배차일":
                            WhereStringList.Add("CONVERT(VARCHAR(10),Orders.CreateTime,111) >= @Sdate AND CONVERT(VARCHAR(10),Orders.CreateTime,111) < @Edate");
                            
                            break;

                        case "접수일":
                            WhereStringList.Add("CONVERT(VARCHAR(10),Trades.RequestDate,111) >= @Sdate AND CONVERT(VARCHAR(10),Trades.RequestDate,111) < @Edate");
                            break;

                        case "결제일":
                            WhereStringList.Add("CONVERT(VARCHAR(10),Trades.PayDate,111) >= @Sdate AND CONVERT(VARCHAR(10),Trades.PayDate,111) < @Edate");
                            WhereStringList.Add("Trades.PayState = 1");
                            break;
                    }
                   
                    var Sdate = dtp_Sdate.Value.Date;
                    var Edate = dtp_Edate.Value.Date.AddDays(1);
                    _Command.Parameters.AddWithValue("@Sdate", Sdate);
                    _Command.Parameters.AddWithValue("@edate", Edate);

                   // WhereStringList.Add("Orders.OrderStatus = 3 ");
                    

                    if (cmbSSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("Drivers.CarNo Like '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "차주명")
                    {
                        WhereStringList.Add(string.Format("Drivers.Caryear Like  '%{0}%'", txtSText.Text));
                    }

                    WhereStringList.Add("trades.EtaxCanCelYN != 'Y'");


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by Trades.RequestDate  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS4.Load(_Reader);

                    }
                }

                
                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS4.ToArray();

           

            if (Query.Any())
            {
                if (cmbSSearch.SelectedIndex == 0)
                {
                    lblMStartDate.Text = "";
                }
                else
                {
                    lblMStartDate.Text = Query.First().MStartDate.ToString("d") + "/" + Query.First().MStart.ToString("N0");
                }

                decimal _Price = Query.Sum(c => c.TradePrice);
                decimal _Vat = 0;
                decimal _Amount = 0;

                _Vat = Math.Floor(_Price * 0.1m);
                _Amount = (_Price + _Vat);

                //지불합계
                lblTradePrice.Text = _Amount.ToString("N0");

                lblTSPrice.Text = Query.Sum(c=> c.TSPrice).ToString("N0");   // (Query.Where(c => c.PayLocation != 5).Sum(c => c.SalesPrice) - Query.Where(c => c.PayLocation != 5).Sum(c => c.TradePrice)).ToString("N0");

                decimal _PayPrice = 0;

                var _PayPriceQ = Query.GroupBy(c => c.TradeId).ToArray();
               
                foreach (var _Pay in Query.GroupBy(c => new { c.TradeId }).ToArray())
                {
                    _PayPrice += _Pay.First().Amount;
                }


                lblPayPrice.Text = _PayPrice.ToString("N0");
                var _Mizi =  Query.Where(c => c.PayDate == "" && c.TRequestDate.Date >= Query.First().MStartDate.Date).ToArray();

                decimal _MPrice = _Mizi.Sum(c => c.TradePrice);
                decimal _MVat = 0;
                decimal _MAmount = 0;

                _MVat = Math.Floor(_MPrice * 0.1m);
                _MAmount = (_MPrice + _MVat);

                if (cmbSSearch.SelectedIndex == 0)
                {
                    var _AMizi = _Mizi.Where(c => c.PayDate == "").Sum(c => c.TradePrice);
                    decimal _AMVat = 0;
                    decimal _AMAmount = 0;

                    _AMVat = Math.Floor(_AMizi * 0.1m);
                    _AMAmount = (_AMizi + _AMVat);


                    lblmizi.Text = _AMAmount.ToString("N0"); //_MAmount.ToString("N0");
                }
                else
                {
                    lblmizi.Text = (Query.First().MStart + _MAmount).ToString("N0");
                }
            }
            else
            {

                lblTradePrice.Text = "-";
                lblTSPrice.Text = "-";
                lblPayPrice.Text = "-";


                lblmizi.Text = "-";

            }

            //if (Query.First().MStart == "S")
            //{

            //    lblmisu.Text = (Query.First().Mizi + _Mizi.Sum(c => c.SalesPrice)).ToString("N0");
            //}
            //else
            //{
            //    //lblmisu.Text = (Query.Sum(c => c.SalesPrice) - Query.Sum(c => c.Amount)).ToString("N0");
            //    lblmisu.Text = _Mizi.Sum(c => c.SalesPrice).ToString("N0");

            //}




        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

    

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS4Row;
            decimal _Price = Selected.TradePrice;
            decimal _Vat = 0;
            decimal _Amount = 0;

            _Vat = Math.Floor(Selected.TradePrice * 0.1m);
            _Amount = (_Price + _Vat);

            if (ModelDataGrid.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (ModelDataGrid.Rows.Count - e.RowIndex).ToString("N0");
            }
            
            //else if (e.ColumnIndex == acceptTimeDataGridViewTextBoxColumn.Index)
            //{
            //    e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            //}
            else if (e.ColumnIndex == tRequestDateDataGridViewTextBoxColumn.Index)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }

            else if (e.ColumnIndex == clientNameDataGridViewTextBoxColumn.Index)
            {
                e.Value = LocalUser.Instance.LogInInformation.Client.CEO;
            }

            else if (e.ColumnIndex == TradeVat.Index)
            {


                e.Value = _Vat.ToString("N0"); ;
            }
            else if (e.ColumnIndex == TradeAmount.Index)
            {


                e.Value = _Amount.ToString("N0"); ;
            }
            else if(e.ColumnIndex== ColumnTaxTradeId.Index)
            {
                if (Selected.TaxTradeId == 0)
                {
                    e.Value = "";
                }
                else
                {
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        var uOrder = ShareOrderDataSet.Trades.Where(c => c.TradeId == Selected.TaxTradeId);

                        e.Value = uOrder.First().RequestDate.ToString("d");
                    }

                }
            }
            else if (e.ColumnIndex == ColumnsEtaxGubun.Index)
            {
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    var uOrder = ShareOrderDataSet.Orders.Where(c => c.TradeId == Selected.TradeId).Count();

                    if (uOrder > 0)
                    {
                        e.Value = "외부";
                    }

                    if (Selected.AllowAcc)
                    {

                        if (uOrder > 0)
                        {
                            e.Value = "외부";
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                    else
                    {
                        if (Selected.HasETax)
                        {
                            e.Value = "전자";
                            if (Selected.SourceType == 0)
                            {
                                e.Value += "(역발행)";

                            }
                        }


                    }
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
                //지정
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
            cmbSMonth.SelectedIndex = 5;
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
            dtp_Edate.Value = DateTime.Now;
            cmbSDiv.SelectedIndex = 0;
            cmbSSearch.SelectedIndex = 0;
            cmbDataGubun.SelectedIndex = 0;
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

            var fileString = "차주정산내역_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
        //    var fileString = "매입차주정산" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
        //    using (MemoryStream _Stream = new MemoryStream(Properties.Resources.매입차주정산양식_Blank))
        //    {
        //        _Stream.Seek(0, SeekOrigin.Begin);
        //        _Excel = new ExcelPackage(_Stream);
        //    }
        //    var _Sheet = _Excel.Workbook.Worksheets[1];
        //    var RowIndex = 2;
        //    for (int i = 0; i < ModelDataGrid.RowCount; i++)
        //    {
        //        _Sheet.Cells[RowIndex, 1].Value = ModelDataGrid[rowNUMDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 2].Value = ModelDataGrid[acceptTimeDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 3].Value = ModelDataGrid[driverCarNoDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 4].Value = ModelDataGrid[driverDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 5].Value = ModelDataGrid[startNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 6].Value = ModelDataGrid[stopNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 7].Value = ModelDataGrid[itemDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 8].Value = ModelDataGrid[customerDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 9].Value = ModelDataGrid[sangHoDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 10].Value = ModelDataGrid[payLocationNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 11].Value = ModelDataGrid[payLocationPDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 12].Value = ModelDataGrid[tRequestDateDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 13].Value = ModelDataGrid[salesPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 14].Value = ModelDataGrid[TradeVat.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 15].Value = ModelDataGrid[TradeAmount.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 16].Value = ModelDataGrid[tSPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 17].Value = ModelDataGrid[payDateDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 18].Value = ModelDataGrid[ColumnTaxTradeId.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 19].Value = ModelDataGrid[payTextDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 20].Value = ModelDataGrid[clientNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

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

        private void ModelDataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            


            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS4Row;
          
            if (Selected.ServiceState == 5)
            {
                ModelDataGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                ModelDataGrid.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;
            }
        }
    }
}
