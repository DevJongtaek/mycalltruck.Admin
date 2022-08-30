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
using mycalltruck.Admin.UI;
using Popbill.Taxinvoice;
using Popbill;
using OfficeOpenXml.Style;

namespace mycalltruck.Admin
{
    public partial class UCNSTATS8 : UserControl
    {
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;
        private const string CRLF = "\r\n";
        string TinfoNtsresult = "";
        string TinfoWriteDate = "";
        string TinfoitemKey = "";
        string TinfostateCode = "";
        string TinfostateName = "";

        string memo = "";
        public UCNSTATS8()
        {
            InitializeComponent();
        }

        private void UCNSTATS1_Load(object sender, EventArgs e)
        {
            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;

            dtp_Sdate.Value = DateTime.Now;
            dtp_Edate.Value = DateTime.Now;
            InitCmb();
            LoadTable();
        }
        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();
        class DriverViewModel
        {
            public int DriverId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string BizNo { get; set; }
            public int candidateid { get; set; }
            public bool AppUse { get; set; }
            public string MobileNo { get; set; }
            public string CarNo { get; set; }
            public string CarYear { get; set; }
            public int CarSize { get; set; }
            public int CarType { get; set; }
            public string Password { get; set; }

            public string CEO { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }

            public string Upjong { get; set; }
            public string Uptae { get; set; }
            public string Email { get; set; }
            public string PhoneNo { get; set; }





        }


        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _DriverTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,ISNULL(MobileNo,N'')as MobileNo,CarNo,ISNULL(CarYear,'N')CarYear,CarSize,CarType,Password,isnull(CEO,N'') as CEO,isnull(AddressState,N'')as AddressState,ISNULL(AddressCity,N'')as AddressCity,ISNULL(AddressDetail,N'')as AddressDetail,ISNULL(Upjong,N'')as Upjong,ISNULL(Uptae,N'')as Uptae,ISNULL(Email,N'')as Email,ISNULL(PhoneNo,N'')as PhoneNo FROM Drivers ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _DriverTable.Add(
                              new DriverViewModel
                              {
                                  DriverId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                                  BizNo = dataReader.GetString(4),
                                  candidateid = dataReader.GetInt32(5),
                                  AppUse = dataReader.GetBoolean(6),
                                  MobileNo = dataReader.GetString(7),
                                  CarNo = dataReader.GetString(8),
                                  CarYear = dataReader.GetString(9),
                                  CarSize = dataReader.GetInt32(10),
                                  CarType = dataReader.GetInt32(11),
                                  Password = dataReader.GetString(12),
                                  CEO = dataReader.GetString(13),

                                  AddressState = dataReader.GetString(14),
                                  AddressCity = dataReader.GetString(15),
                                  AddressDetail = dataReader.GetString(16),

                                  Upjong = dataReader.GetString(17),
                                  Uptae = dataReader.GetString(18),
                                  Email = dataReader.GetString(19),
                                  PhoneNo = dataReader.GetString(20),
                              });
                        }
                    }
                }
                connection.Close();
            }
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

            Dictionary<string, string> SsDate = new Dictionary<string, string>
            {
                { "작성일", "작성일" },
                { "발행일", "발행일" },
                
            };

            cmb_Date.DataSource = new BindingSource(SsDate, null);
            cmb_Date.DisplayMember = "Value";
            cmb_Date.ValueMember = "Key";

            cmb_Date.SelectedIndex = 0;
            



            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {
                { "전체", "전체" },

                { "사업자번호", "사업자번호" },
                { "상호", "상호" },
                 { "차량번호", "차량번호" },
                { "발행자", "발행자" },
            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


        }
        private String GetSelectCommand()
        {
          
            return @"SELECT distinct trades.TradeId,  Drivers.BizNo,Drivers.Name,Drivers.CarNo,Trades.RequestDate,AcceptCount, Trades.Splits,Trades.Amount
                   ,substring(right(Trades.trusteeMgtKey,14),1,4)+'-'+substring(right(Trades.trusteeMgtKey,14),5,2)+'-'+substring(right(Trades.trusteeMgtKey,14),7,2) as ETaxDate
                    ,Trades.EtaxUserName ,Trades.EtaxCanCelYN ,Orders.DriverId, Trades.trusteeMgtKey FROM Trades
                    JOIN Drivers
                    ON Trades.DriverId = Drivers.DriverId
                    JOIN Orders
                    ON Trades.TradeId = Orders.TaxTradeId and Trades.EtaxCanCelYN = 'N' ";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS8.Clear();
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                    String _DateColumn = "";
                    if (cmb_Date.SelectedIndex == 0)
                    {
                        _DateColumn = "Trades.RequestDate";
                    }
                    else if (cmb_Date.SelectedIndex == 1)
                    {

                        _DateColumn = " substring(right(Trades.trusteeMgtKey,14),1,4)+'-'+substring(right(Trades.trusteeMgtKey,14),5,2)+'-'+substring(right(Trades.trusteeMgtKey,14),7,2) ";

                    }
                  

                    WhereStringList.Add($"{_DateColumn} >= @Begin AND {_DateColumn} < @End ");
                    _Command.Parameters.AddWithValue("@Begin", dtp_Sdate.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtp_Edate.Value.Date.AddDays(1));


                 



                    WhereStringList.Add("Trades.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                   // WhereStringList.Add("Orders.OrderStatus = 3 ");
                    

                    if (cmbSSearch.Text == "사업자번호")
                    {
                        WhereStringList.Add(string.Format("Replace(Drivers.BizNo,'-','') Like '%{0}%'", txtSText.Text.Replace("-","")));
                    }
                    else if (cmbSSearch.Text == "상호")
                    {
                        WhereStringList.Add(string.Format("Drivers.Name Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("Drivers.CarNo Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "발행자")
                    {
                        WhereStringList.Add(string.Format("Trades.EtaxUserName Like  '%{0}%'", txtSText.Text));
                    }

               

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += $" Order by { _DateColumn}  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS8.Load(_Reader);

                    }
                }

                
                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS8.ToArray();

            if (Query.Any())
            {
                if(cmbSSearch.SelectedIndex == 0)
                {
                    lblMStartDate.Text = "";
                }
                else
                {
                   // lblMStartDate.Text = Query.First().MStartDate.ToString("d") + "/" + Query.First().MStart.ToString("N0");
                }
                //lblTradePrice.Text = Query.Sum(c => c.Amount).ToString("N0");
                
                lblIssuePrice.Text = Query.Sum(c => c.Amount).ToString("N0");
                //lblSTPrice.Text = (Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3)).ToString("N0");

                //세금계산서 발행금액중 미수금액
              //  var _Misu = Query.Where(c =>  c.IssueDate != "" &&  DateTime.Parse(c.IssueDate) >= Query.First().MStartDate ).ToArray();

                //var _totalPrice = Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3);
                //var _inputPrice = Query.Where(c => c.IssueDate != "" && DateTime.Parse(c.IssueDate).Date >= Query.First().MStartDate.Date).Sum(c => c.Amount) - _totalPrice;


                if (cmbSSearch.SelectedIndex == 0)
                {
                   // lblmisu.Text = (Query.Where(c => c.IssueDate != "").Sum(c => c.Amount) - (Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3))).ToString("N0");
                }
                else
                {
                    ////미수시작일이 없으면 : 발행금액 - 수금액 = 현재미수금
                    //if(Query.First().MStart == 0)
                    //{
                    //    lblmisu.Text = (Query.Where(c => c.IssueDate != "").Sum(c => c.Amount) - _totalPrice).ToString("N0");
                    //}
                    ////(미수시작금액+ 발행금액) - 수금액 = 현재미수금 (단, 미수시작일이후발행금액만 계산함)
                    //else
                    //{
                    //    lblmisu.Text = (Query.First().MStart + _inputPrice).ToString("N0");
                    //}
                    
                }


             
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

            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS8Row;


            if (ModelDataGrid.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (ModelDataGrid.Rows.Count - e.RowIndex).ToString("N0");
            }

            if (ModelDataGrid.Columns[e.ColumnIndex] == ColumnSplits)
            {
                if(Selected.Splits)
                {
                    e.Value = "개별";
                }
                else
                {
                    e.Value = "합산";
                }
                
            }

            //if (ModelDataGrid.Columns[e.ColumnIndex] == acceptCountDataGridViewTextBoxColumn)
            //{
            //    int acceptCount = 0;
            //    try
            //    {
            //        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //        {
            //            cn.Open();
            //            var Command1 = cn.CreateCommand();
            //            Command1.CommandText = "SELECT count(*) FROM Orders where SalesManageId = " + Selected.SalesId + "";
            //            var o1 = Command1.ExecuteScalar();
            //            if (o1 != null)
            //            {
            //                acceptCount += Convert.ToInt32(o1);
            //            }
            //            cn.Close();
            //        }
            //        e.Value = acceptCount.ToString("N0");
            //    }
            //    catch
            //    {
            //        e.Value = 0;
            //    }
            //}

            //if (ModelDataGrid.Columns[e.ColumnIndex] == inputPrice2DateDataGridViewTextBoxColumn)
            //{
            //    if (Selected.InputPrice2 == 0)
            //    {
            //        e.Value = "";
            //    }
            //    else
            //    {
            //        e.Value = Selected.InputPrice2Date;
            //    }
            //}
            //if (ModelDataGrid.Columns[e.ColumnIndex] == inputPrice3DateDataGridViewTextBoxColumn)
            //{
            //    if (Selected.InputPrice3 == 0)
            //    {
            //        e.Value = "";
            //    }
            //    else
            //    {
            //        e.Value = Selected.InputPrice3Date;
            //    }
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
        String _Sender = "";
        bool MethodProccess = false;
        private void NSTATS8BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            nSTATSDataSet.NSTATS4.Clear();

            if (nSTATS8BindingSource.Current == null)

                return;


            if (((DataRowView)nSTATS8BindingSource.Current).Row is NSTATSDataSet.NSTATS8Row Selected)
            {


                MethodProccess = true;
                //  _Sender = "salesManageBindingSource";
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {

                        _Command.CommandText = @" SELECT 

                   ISNULL(CONVERT(varchar, Orders.CreateTime, 111), N'') as AcceptTime
				   ,Drivers.DriverId
                    ,Drivers.CarNo as DriverCarNo
                    ,Drivers.CarYear as Driver
                    ,ISNULL(Orders.StartName, '') StartName
                    ,ISNULL(Orders.StopName, '')StopName
                    ,ISNULL(Orders.Item, '')Item
                    ,ISNULL(Orders.Customer, '') as Customer
				 
                    ,ISNULL(Orders.ReferralId, 0) as ReferralId
                    ,ISNULL(Customers.SangHo, '') as SangHo
					,ISNULL(Orders.PayLocation, 0) as PayLocation
					
                    , CASE
                    WHEN Orders.PayLocation IS NULL THEN ''

                    WHEN Orders.PayLocation = 1 THEN  '인수증'

                    WHEN Orders.PayLocation = 4 THEN  '인수증+선/착불'

                    WHEN Orders.PayLocation = 5 THEN  '지입배차'

                    END AS PayLocationName

					,CASE WHEN Orders.PayLocation IS NULL THEN '' WHEN Orders.PayLocation = 5 THEN  '' ELSE '포함' END AS PayLocationP
					, Trades.RequestDate as TRequestDate
                    ,   ISNULL(Orders.TradePrice, 0)  TradePrice
                    ,  CASE WHEN Orders.PayLocation IS NULL THEN Trades.Amount else ISNULL(Orders.SalesPrice, 0) END SalesPrice

                     , CASE WHEN Orders.PayLocation IS NULL THEN Trades.Amount WHEN Orders.PayLocation = 5 THEN  0 else ISNULL(Orders.SalesPrice, 0) - ISNULL(Orders.TradePrice, 0) end as TSPrice
                    ,Case WHEN Trades.PayState = 1  then Convert(varchar(10),Trades.PayDate,111) else '' end as PayDate
                    ,Case WHEN Trades.PayState = 1 AND Trades.HasAcc = 1 then '카드' else '현금' end as PayText
                    ,'' as ClientName
                    ,Case WHEN Trades.PayState = 1  then Trades.Amount else 0 end as Amount
                    ,isnull(DriverInstances.MStartDate, getdate()) as MStartDate, ISNULL(DriverInstances.MStart, 0) as MStart,ISNULL(DriverInstances.Mizi, 0) AS Mizi, ISNULL(DriverInstances.Misu, 0) as Misu
                    ,orders.OrderId
					,trades.TradeId
                    , Trades.HasETax, Trades.AllowAcc, Trades.SourceType
                    ,ISNULL(Orders.TaxTradeId, 0) as TaxTradeId

                    FROM Trades

                    left JOIN Orders ON Orders.TradeId = Trades.TradeId AND(Orders.PayLocation  in (1, 4, 5)) AND Orders.OrderStatus = 3

                    JOIN Drivers ON Trades.DriverId = Drivers.DriverId

                    LEFT JOIN Customers ON Orders.ReferralId = Customers.CustomerId

                    LEFT JOIN DriverInstances ON Trades.DriverId = DriverInstances.DriverId AND DriverInstances.ClientId = Trades.ClientId WHERE Orders.TaxTradeId = @TradeId";

                        _Command.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            nSTATSDataSet.NSTATS4.Load(_Reader);


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

            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS4Row;
            decimal _Price = Selected.TradePrice;
            decimal _Vat = 0;
            decimal _Amount = 0;

            _Vat = Math.Floor(Selected.TradePrice * 0.1m);
            _Amount = (_Price + _Vat);

            if (newDGV1.Columns[e.ColumnIndex] == dataGridViewTextBoxColumn1)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0");
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
            else if (e.ColumnIndex == ColumnTaxTradeId.Index)
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

        public void btn_Inew_Click(object sender, EventArgs e)
        {
            if (cmbSMonth.Items.Count > 0)
            {
                cmbSMonth.SelectedIndex = 3;
            }
            
            dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
            dtp_Edate.Value = DateTime.Now;
            cmb_Date.SelectedIndex = 0;
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

            var fileString = "차주수수료발행내역_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
        //    var fileString = "차주수수료계산서발행내역" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
        //    using (MemoryStream _Stream = new MemoryStream(Properties.Resources.차주수수료계산서발행내역_Blank))
        //    {
        //        _Stream.Seek(0, SeekOrigin.Begin);
        //        _Excel = new ExcelPackage(_Stream);
        //    }
        //    var _Sheet = _Excel.Workbook.Worksheets[1];
        //    var RowIndex = 2;
        //    for (int i = 0; i < ModelDataGrid.RowCount; i++)
        //    {
        //        _Sheet.Cells[RowIndex, 1].Value = ModelDataGrid[rowNUMDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 2].Value = ModelDataGrid[bizNoDataGridViewTextBoxColumn1.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 3].Value = ModelDataGrid[nameDataGridViewTextBoxColumn1.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 4].Value = ModelDataGrid[carNoDataGridViewTextBoxColumn1.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 5].Value = ModelDataGrid[requestDateDataGridViewTextBoxColumn1.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 6].Value = ModelDataGrid[ColumnSplits.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 7].Value = ModelDataGrid[acceptCountDataGridViewTextBoxColumn1.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 8].Value = ModelDataGrid[amountDataGridViewTextBoxColumn2.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 9].Value = ModelDataGrid[eTaxDateDataGridViewTextBoxColumn1.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 10].Value = ModelDataGrid[etaxUserNameDataGridViewTextBoxColumn1.Index, i].FormattedValue;


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

        private void ModelDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)ModelDataGrid.SelectedRows[0].DataBoundItem).Row as NSTATSDataSet.NSTATS8Row;
            if (ModelDataGrid.Columns[e.ColumnIndex] == ColumnTax && e.RowIndex >= 0)
            {
                if (Selected != null)
                {
                    if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "차수 수수료계산서", 1), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {

                        #region
                        TaxinvoiceInfo taxinvoiceInfo = taxinvoiceService.GetInfo("1148654906", MgtKeyType.TRUSTEE, Selected.trusteeMgtKey);

                        try
                        {
                            string tmp = null;

                            tmp += "itemKey : " + taxinvoiceInfo.itemKey + CRLF;
                            tmp += "taxType : " + taxinvoiceInfo.taxType + CRLF;
                            tmp += "writeDate : " + taxinvoiceInfo.writeDate + CRLF;
                            tmp += "regDT : " + taxinvoiceInfo.regDT + CRLF;

                            tmp += "invoicerCorpName : " + taxinvoiceInfo.invoicerCorpName + CRLF;
                            tmp += "invoicerCorpNum : " + taxinvoiceInfo.invoicerCorpNum + CRLF;
                            tmp += "invoicerMgtKey : " + taxinvoiceInfo.invoicerMgtKey + CRLF;
                            tmp += "invoicerPrintYN : " + taxinvoiceInfo.invoicerPrintYN + CRLF;
                            tmp += "invoiceeCorpName : " + taxinvoiceInfo.invoiceeCorpName + CRLF;
                            tmp += "invoiceeCorpNum : " + taxinvoiceInfo.invoiceeCorpNum + CRLF;
                            tmp += "invoiceeMgtKey : " + taxinvoiceInfo.invoiceeMgtKey + CRLF;
                            tmp += "invoiceePrintYN : " + taxinvoiceInfo.invoiceePrintYN + CRLF;
                            tmp += "closeDownState : " + taxinvoiceInfo.closeDownState + CRLF;
                            tmp += "closeDownStateDate : " + taxinvoiceInfo.closeDownStateDate + CRLF;
                            tmp += "interOPYN : " + taxinvoiceInfo.interOPYN + CRLF;

                            tmp += "supplyCostTotal : " + taxinvoiceInfo.supplyCostTotal + CRLF;
                            tmp += "taxTotal : " + taxinvoiceInfo.taxTotal + CRLF;
                            tmp += "purposeType : " + taxinvoiceInfo.purposeType + CRLF;
                            tmp += "modifyCode : " + taxinvoiceInfo.modifyCode.ToString() + CRLF;
                            tmp += "issueType : " + taxinvoiceInfo.issueType + CRLF;

                            tmp += "issueDT : " + taxinvoiceInfo.issueDT + CRLF;
                            tmp += "preIssueDT : " + taxinvoiceInfo.preIssueDT + CRLF;

                            tmp += "stateCode : " + taxinvoiceInfo.stateCode.ToString() + CRLF;
                            tmp += "stateDT : " + taxinvoiceInfo.stateDT + CRLF;

                            tmp += "openYN : " + taxinvoiceInfo.openYN.ToString() + CRLF;
                            tmp += "openDT : " + taxinvoiceInfo.openDT + CRLF;
                            tmp += "ntsresult : " + taxinvoiceInfo.ntsresult + CRLF; //국세청전송결과
                            tmp += "ntsconfirmNum : " + taxinvoiceInfo.ntsconfirmNum + CRLF; //국세청승인번호
                            tmp += "ntssendDT : " + taxinvoiceInfo.ntssendDT + CRLF; //
                            tmp += "ntsresultDT : " + taxinvoiceInfo.ntsresultDT + CRLF;
                            tmp += "ntssendErrCode : " + taxinvoiceInfo.ntssendErrCode + CRLF;
                            tmp += "stateMemo : " + taxinvoiceInfo.stateMemo;
                            // MessageBox.Show(tmp, "문서 상태/요약 정보 조회");

                            if (!string.IsNullOrEmpty(taxinvoiceInfo.ntsresult))
                            {
                                TinfoNtsresult = taxinvoiceInfo.ntsresult;
                            }
                            TinfoWriteDate = taxinvoiceInfo.writeDate;
                            TinfoitemKey = taxinvoiceInfo.itemKey;
                            TinfostateCode = taxinvoiceInfo.stateCode.ToString();

                            switch (TinfostateCode)
                            {
                                case "300":
                                    TinfostateName = "발행완료";
                                    break;
                                case "310":
                                    TinfostateName = "발행완료";
                                    break;
                                case "301":
                                    TinfostateName = "전송중";
                                    break;
                                case "302":
                                    TinfostateName = "전송중";
                                    break;
                                case "303":
                                    TinfostateName = "전송중";
                                    break;
                                case "311":
                                    TinfostateName = "전송중";
                                    break;
                                case "312":
                                    TinfostateName = "전송중";
                                    break;
                                case "313":
                                    TinfostateName = "전송중";
                                    break;
                                case "304":
                                    TinfostateName = "전송성공";
                                    break;
                                case "314":
                                    TinfostateName = "전송성공";
                                    break;
                                case "305":
                                    TinfostateName = "전송실패";
                                    break;
                                case "315":
                                    TinfostateName = "전송실패";
                                    break;

                            }
                        }

                        catch (PopbillException ex)
                        {
                            MessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                                            "응답메시지(message) : " + ex.Message, "문서 상태/요약 정보 조회");
                        }
                        #endregion


                        //당일일경우 삭제
                        if (Selected.RequestDate.Date == DateTime.Now.Date)
                        {

                            try
                            {
                                if (TinfostateName == "발행완료")
                                {
                                    if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 당일 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        SendCancelIssue(Selected.TradeId);
                                    }
                                }
                                else if (TinfostateName == "전송성공")
                                {
                                    if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        SendMinusInvoice(Selected.DriverId, Selected.TradeId, LocalUser.Instance.LogInInformation.ClientId, Selected.trusteeMgtKey);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }

                        }
                        else
                        {
                            if (TinfostateName == "발행완료")
                            {
                                if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        SendCancelIssue(Selected.TradeId);

                                        //SendMinusInvoice(Selected.DriverId, Selected.TradeId, Selected.ClientId, Selected.trusteeMgtKey);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }
                                }
                            }
                            else if (TinfostateName == "전송성공")
                            {
                                if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        SendMinusInvoice(Selected.DriverId, Selected.TradeId, LocalUser.Instance.LogInInformation.ClientId , Selected.trusteeMgtKey);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }



                    }
                }

            }
        }


        private void SendCancelIssue(int tradeId)
        {
            try
            {
                this.tradesTableAdapter.Fill(this.tradeDataSet.Trades);
                //clientsTableAdapter.Fill(clientDataSet.Clients);
                //InitDriverTable();

                var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
                Response response = taxinvoiceService.CancelIssue("1148654906", MgtKeyType.TRUSTEE, _Trade.trusteeMgtKey, memo, "edubillsys");

                SendDelete(tradeId);
            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);

                return;

            }
        }

        private void SendDelete(int tradeId)
        {

            try
            {
                this.tradesTableAdapter.Fill(this.tradeDataSet.Trades);
                clientsTableAdapter.Fill(clientDataSet.Clients);
                InitDriverTable();

                var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
                Response response = taxinvoiceService.Delete("1148654906", MgtKeyType.TRUSTEE, _Trade.trusteeMgtKey, "edubillsys");


                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "Delete Trades  WHERE TradeId = @TradeId";
                    cmd.Parameters.AddWithValue("@TradeId", tradeId);
                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                      " UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";
                    // cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                     " UPDATE Orders SET TaxTradeId = null  WHERE TaxTradeId = @TradeId";

                    cmd.ExecuteNonQuery();

                    // cmd.CommandText =
                    //" UPDATE Drivers SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId";
                    using (SqlCommand _DICommand = cn.CreateCommand())
                    {
                        _DICommand.CommandText =
                        " UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId ";
                        _DICommand.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                        _DICommand.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                        _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _DICommand.ExecuteNonQuery();
                    }

                    // cmd.ExecuteNonQuery();

                    if (!String.IsNullOrEmpty(_Trade.PdfFileName))
                    {
                        if (!String.IsNullOrEmpty(_Trade.AipId))
                        {
                            SqlCommand cmdDocument = cn.CreateCommand();
                            cmdDocument.CommandText =
                                " INSERT INTO DocuTable(TradeId,PdfFileName,AipId,DocId,Status,RequestDateTime) " +
                                " Values(@TradeId,@PdfFileName,@AipId,@DocId,@Status,getdate())";
                            cmdDocument.Parameters.AddWithValue("@Status", "_40_ready");
                            cmdDocument.Parameters.AddWithValue("@TradeId", tradeId);
                            cmdDocument.Parameters.AddWithValue("@PdfFileName", _Trade.PdfFileName);
                            cmdDocument.Parameters.AddWithValue("@AipId", _Trade.AipId);
                            cmdDocument.Parameters.AddWithValue("@DocId", _Trade.DocId);
                            cmdDocument.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand cmdDocument = cn.CreateCommand();
                            cmdDocument.CommandText =
                                "Delete DocuTable  WHERE TradeId = @TradeId";
                            cmdDocument.Parameters.AddWithValue("@TradeId", tradeId);
                            cmdDocument.ExecuteNonQuery();

                        }
                    }
                    cn.Close();
                }

                MessageBox.Show("삭제 되었습니다.");
                btnSearch_Click(null, null);
            }
            catch (PopbillException ex)
            {

                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return;
            }




        }

        private void SendMinusInvoice(int driverId, int tradeId, int clientId, string MgtKEy)
        {
            InitDriverTable();
            this.tradesTableAdapter.Fill(this.tradeDataSet.Trades);
            clientsTableAdapter.Fill(clientDataSet.Clients);
            
            //var _Driver = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == driverId);
            var _Driver = _DriverTable.Where(c => c.DriverId == driverId);
            var _Client = clientDataSet.Clients.FirstOrDefault(c => c.ClientId == clientId);
            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            bool forceIssue = false;        // 지연발행 강제여부
            var TPrice = _Trade.Price * -1;
            var TAmount = _Trade.Amount * -1;
            var TVat = _Trade.VAT * -1;



            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();


            taxinvoice.writeDate = TinfoWriteDate;//DateTime.Now.ToString("yyyyMMdd");                      //필수, 기재상 작성일자
            taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
            taxinvoice.issueType = "위수탁";                        //필수, {정발행, 역발행, 위수탁}
            taxinvoice.purposeType = "청구";                        //필수, {영수, 청구}
            taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
            taxinvoice.taxType = "과세";                            //필수, {과세, 영세, 면세}

            taxinvoice.invoicerCorpNum = _Driver.First().BizNo.Replace("-", "");             //공급자 사업자번호
            taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
            taxinvoice.invoicerCorpName = _Driver.First().Name;
            taxinvoice.invoicerMgtKey = tradeId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoicerCEOName = _Driver.First().CEO;
            taxinvoice.invoicerAddr = _Driver.First().AddressState + " " + _Driver.First().AddressCity + " " + _Driver.First().AddressDetail;
            taxinvoice.invoicerBizClass = _Driver.First().Upjong;
            taxinvoice.invoicerBizType = _Driver.First().Uptae;
            taxinvoice.invoicerContactName = _Driver.First().CEO;
            taxinvoice.invoicerEmail = _Driver.First().Email;
            taxinvoice.invoicerTEL = _Driver.First().PhoneNo;
            taxinvoice.invoicerHP = _Driver.First().MobileNo;
            taxinvoice.invoicerSMSSendYN = false;                    //정발행시(공급자->공급받는자) 문자발송기능 사용시 활용

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            taxinvoice.invoiceeType = "사업자";                     //공급받는자 구분, {사업자, 개인, 외국인}
            taxinvoice.invoiceeCorpNum = _Client.BizNo.Replace("-", "");              //공급받는자 사업자번호
            taxinvoice.invoiceeCorpName = _Client.Name;
            // taxinvoice.invoiceeMgtKey = "";                         //역발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoiceeCEOName = _Client.CEO;
            taxinvoice.invoiceeAddr = _Client.AddressState + " " + _Client.AddressCity + " " + _Client.AddressDetail;
            taxinvoice.invoiceeBizClass = _Client.Upjong;
            taxinvoice.invoiceeBizType = _Client.Uptae;
            taxinvoice.invoiceeTEL1 = _Client.MobileNo;
            taxinvoice.invoiceeContactName1 = _Client.CEO;
            taxinvoice.invoiceeEmail1 = _Client.Email;
            taxinvoice.invoiceeHP1 = _Client.MobileNo;
            taxinvoice.invoiceeSMSSendYN = false;                   //역발행시(공급받는자->공급자) 문자발송기능 사용시 활용

            taxinvoice.supplyCostTotal = TPrice.ToString().Replace(".00", "");                  //필수 공급가액 합계"
            taxinvoice.taxTotal = TVat.ToString().Replace(".00", "");                      //필수 세액 합계
            taxinvoice.totalAmount = TAmount.ToString().Replace(".00", "");                  //필수 합계금액.  공급가액 + 세액

            taxinvoice.modifyCode = 4;                           //수정세금계산서 작성시 1~6까지 선택기재.
            taxinvoice.originalTaxinvoiceKey = TinfoitemKey;                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
            taxinvoice.serialNum = "1";
            taxinvoice.cash = "";                                   //현금
            taxinvoice.chkBill = "";                                //수표
            taxinvoice.note = "";                                   //어음
            taxinvoice.credit = "";                                 //외상미수금
            taxinvoice.remark1 = _Trade.TransportDate.ToString("d").Replace("-", "/") + " " + _Trade.StartState + " → " + _Trade.StopState + " ( ☎ " + _Driver.First().MobileNo + " )";
            taxinvoice.remark2 = "";
            taxinvoice.remark3 = "";
            taxinvoice.kwon = 1;
            taxinvoice.ho = 1;

            taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
            taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

            string CtrusteeMgtKey = "t" + _Trade.TradeId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            #region 수탁자
            //수탁자 문서관리번호
            taxinvoice.trusteeMgtKey = CtrusteeMgtKey; //_Trade.trusteeMgtKey;
            //사업자번호
            taxinvoice.trusteeCorpNum = "1148654906";
            //상호
            taxinvoice.trusteeCorpName = "(주)에듀빌시스템";
            //대표자
            taxinvoice.trusteeCEOName = "박양우";

            //주소
            taxinvoice.trusteeAddr = "서울특별시 금천구 디지털로9길 65, 408호(가산동,백상스타타워1차)";

            //업태
            taxinvoice.trusteeBizType = "";

            //업종
            taxinvoice.trusteeBizClass = "";


            //담당자성명
            taxinvoice.trusteeContactName = "박양우";

            #endregion


            taxinvoice.detailList = new List<TaxinvoiceDetail>();

            TaxinvoiceDetail detail = new TaxinvoiceDetail();

            detail.serialNum = 1;                                   //일련번호
            detail.purchaseDT = DateTime.Now.ToString("yyyyMMdd");                         //거래일자
            detail.itemName = "화물운송료(" + _Driver.First().CarNo + ")";
            detail.spec = "";
            detail.qty = "1";                                       //수량
            detail.unitCost = TPrice.ToString().Replace(".00", ""); ;                             //단가
            detail.supplyCost = TPrice.ToString().Replace(".00", ""); ;                           //공급가액
            detail.tax = TVat.ToString().Replace(".00", ""); ;                                   //세액
            detail.remark = "";

            taxinvoice.detailList.Add(detail);

            detail = new TaxinvoiceDetail();

            try
            {
                Response response = taxinvoiceService.RegistIssue("1148654906", taxinvoice, forceIssue, memo);
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {

                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "INSERT INTO  Trades" +
                        "(RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, trusteeMgtKey, EtaxCanCelYN, ExcelExportYN, TransportDate, StartState, StopState)  " +
                        " SELECT getdate(), BeginDate, EndDate, DriverName, Item, Price *-1, VAT*-1, Amount*-1, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, '" + CtrusteeMgtKey + "', 'Y', ExcelExportYN, TransportDate, StartState, StopState FROM Trades" +
                        " WHERE TradeId = @TradeId" +
                        " UPDATE Trades SET EtaxCanCelYN = 'Y' " +
                        " WHERE TradeId = @TradeId";



                    cmd.Parameters.AddWithValue("@TradeId", tradeId);
                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                   " UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";

                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                    " UPDATE Orders SET TaxTradeId = null  WHERE TaxTradeId = @TradeId";

                    cmd.ExecuteNonQuery();


                    // cmd.CommandText =
                    //" UPDATE Drivers SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId";
                    // cmd.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                    // cmd.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                    // cmd.ExecuteNonQuery();
                    using (SqlCommand _DICommand = cn.CreateCommand())
                    {
                        _DICommand.CommandText =
                        " UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId ";
                        _DICommand.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                        _DICommand.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                        _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _DICommand.ExecuteNonQuery();
                    }

                }

                MessageBox.Show("발행 취소 되었습니다.");

                btnSearch_Click(null, null);


            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return;
            }
        }
    }
}
