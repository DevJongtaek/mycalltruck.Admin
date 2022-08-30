using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;
using mycalltruck.Admin.Class.Common;
using System.Data.SqlClient;
using OfficeOpenXml;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.DataSet;
using Popbill.Taxinvoice;
using Popbill;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Class;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_Trade : Form
    {
        string FileName = string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        DESCrypt m_crypt = null;
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;
        #region 전자세금계산서NiceDNR
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
        #endregion
        public EXCELINSERT_Trade()
        {
            InitializeComponent();
            InitializeStorage();
            cmb_Savegubun.SelectedIndex = 0;
            m_crypt = new DESCrypt("12345678");
        }

        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;


            InitCompareTable();
          

            clientsTableAdapter.Fill(clientDataSet.Clients);

        }

        private void btn_Info_Click(object sender, EventArgs e)
        {
            ImportExcel();
        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            DataTest();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private String GetSelectCommand()
        {
            return @"SELECT distinct Trades.TradeId, Trades.RequestDate, BeginDate, EndDate, 
                trades.Item, Trades.Price, VAT, Amount, PayState, PayDate, Trades.PayBankName, Trades.PayBankCode, Trades.PayAccountNo, Trades.PayInputName, 
                Trades.DriverId, Trades.ClientId, Trades.UseTax, LGD_OID, 
                LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, AllowAcc, HasAcc, ClientAccId, 
                SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, 
                Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, 
                HasETax, Trades.SourceType,
                Drivers.LoginId AS DriverLoginId, Drivers.CarNo AS DriverCarNo, Drivers.BizNo AS DriverBizNo, Drivers.Name AS DriverName, 
                Drivers.ServiceState, Drivers.MID, 
                Clients.Code AS ClientCode, Clients.Name AS ClientName, Trades.AcceptCount, Trades.SubClientId, Trades.ClientUserId
                ,(SELECT ISNULL(GroupName,'미설정') FROM  DriverInstances WHERE DriverId = Trades.DriverId and ClientId = Trades.ClientId) as GroupName
                ,Trades.ExcelExportYN,ISNULL(Trades.EtaxCanCelYN,'N') AS EtaxCanCelYN,trusteeMgtKey,TransportDate,Trades.StartState,Trades.StopState,Trades.PdfFileName,Trades.AipId,Trades.DocId,Trades.DeleteYn,Trades.UpdateDateTime
				,Orders.TradeId AS OTradeId
                FROM     Trades
                JOIN Drivers ON Trades.DriverId = Drivers.DriverId
                JOIN Clients ON Trades.ClientId = Clients.ClientId 
			    LEFT JOIN Orders ON Trades.TradeId = Orders.TradeId ";


        }

        private void LoadTable()
        {
            tradeDataSet.Trades.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

                    WhereStringList.Add("Trades.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);




                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        tradeDataSet.Trades.Load(_Reader);


                    }
                }
                _Connection.Close();
            }


        }
        private void InitCompareTable()
        {
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(baseDataSet.Drivers);
        }

        #region STORAGE
        class Model
        {
            public String IDX { get; set; }
            public String CarNo { get; set; }
            public String Name { get; set; }
            public String BeginDate { get; set; }
            public String EndDate { get; set; }
            
            public String Item { get; set; }
            public String Date { get; set; }
            public String Price { get; set; }
            public String VAT { get; set; }
            public String ETAX { get; set; }
            public String Error { get; set; }
            public String PayBankName { get; set; }
            public String PayBankCode { get; set; }
            public String PayAccountNo { get; set; }
            public String PayInputName { get; set; }
        }
        BindingList<Model> _CoreList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _CoreList;
        }
        #endregion

        #region UPDATE
        private void ImportExcel()
        {
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.TRADE);

            if (di.Exists == false)
            {
                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.TRADE;
            d.Filter = "Excel통합문서 (*.xlsx)|*.xlsx";
            d.FilterIndex = 1;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = d.FileName;

                ExcelPackage _Excel = null;
                try
                {
                    _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
                }
                catch (Exception)
                {
                    MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                if (_Excel.Workbook.Worksheets.Count < 1)
                {
                    MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                _CoreList.Clear();
                DataCount = 0;
                var _Sheet = _Excel.Workbook.Worksheets[1];
                const int TestIndex = 2;
                int RowIndex = 2;
                while (true)
                {
                    var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;
                    var Added = new Model
                    {
                        IDX = _Sheet.Cells[RowIndex, 1].Text,
                        CarNo = _Sheet.Cells[RowIndex, 2].Text,
                        Name = _Sheet.Cells[RowIndex, 3].Text,
                        BeginDate = _Sheet.Cells[RowIndex, 4].Text,
                        EndDate = _Sheet.Cells[RowIndex, 5].Text,
                       
                        Item = _Sheet.Cells[RowIndex, 6].Text,
                        Date = _Sheet.Cells[RowIndex, 7].Text,
                        Price = _Sheet.Cells[RowIndex, 8].Text,
                        VAT = _Sheet.Cells[RowIndex, 9].Text,
                        ETAX = _Sheet.Cells[RowIndex,10].Text,
                    };

                    _CoreList.Add(Added);
                    RowIndex++;
                }
                label4.Text = "0";
                label5.Text = "0";
                label6.Text = "0";
            }
        }
        private void ExportExcel()
        {
            if (_CoreList.Count == 0)
            {
                MessageBox.Show("확인할 데이터가 없습니다.");
                return;
            }
            ExcelPackage _Excel = null;
            try
            {
                _Excel = new ExcelPackage(new System.IO.FileInfo(FileName));
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            if (_Excel.Workbook.Worksheets.Count < 1)
            {
                MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            int RowIndex = 2;
            int ERROR_Index = 11;
            for (int i = 0; i < _CoreList.Count; i++)
            {
                var _Model = _CoreList[i];
                _Sheet.Cells[RowIndex, ERROR_Index].Value = _Model.Error;
                RowIndex++;
            }
            try
            {
                _Excel.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            System.Diagnostics.Process.Start(FileName);
        }
        private void DataTest()
        {
            if (_CoreList.Count == 0)
            {
                MessageBox.Show("검증할 데이터가 없습니다.");
                return;

            }
            btn_Info.Enabled = false;
            btn_Update.Enabled = false;
            btn_OK.Enabled = false;
            cmb_Savegubun.Enabled = false;
            btn_Update.Enabled = false;
            btn_Close.Enabled = false;
            Task.Factory.StartNew(() => {
                FCount = 0;
                errCount = 0;
                int RowErrorCount = 0;
                foreach (Model row in _CoreList)
                {
                    RowErrorCount = 0;
                    String ErrorText = "";
                    if (row.IDX != "")
                    {
                        FCount++;
                        //사업자등록번호 없으면 에러
                        if (String.IsNullOrEmpty(row.CarNo) || row.CarNo.Length <4)
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 차량번호 형식 또는 빈값";
                            }
                            else
                            {
                                ErrorText += "차량번호 형식 또는 빈값";
                            }
                            RowErrorCount++;
                        }
                        else
                        {
                            if (!baseDataSet.Drivers.Any(c => c.CarNo.Replace(" ","") == row.CarNo))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 차량번호 일치하는 기사 없음";
                                }
                                else
                                {
                                    ErrorText += "차량번호 일치하는 기사 없음";
                                }
                                RowErrorCount++;
                            }
                            else
                            {
                                var _CarModel = baseDataSet.Drivers.First(c => c.CarNo.Replace(" ", "") == row.CarNo);
                                row.PayBankName = _CarModel.PayBankName;
                                row.PayBankCode = _CarModel.PayBankCode;
                                row.PayInputName = _CarModel.PayInputName;
                                row.PayAccountNo = m_crypt.Decrypt(_CarModel.PayAccountNo);
                            }
                        }
                        DateTime bDate;
                        if (String.IsNullOrEmpty(row.BeginDate) || !DateTime.TryParseExact(row.BeginDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out bDate))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 운송시작일 이상";
                            }
                            else
                            {
                                ErrorText += "운송시작일 이상";

                            }
                            RowErrorCount++;
                        }
                        DateTime eDate;
                        if (String.IsNullOrEmpty(row.EndDate) || !DateTime.TryParseExact(row.EndDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out eDate))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 운송종료일 이상";
                            }
                            else
                            {
                                ErrorText += "운송종료일 이상";

                            }
                            RowErrorCount++;
                        }


                        //기사명
                        //if (String.IsNullOrEmpty(row.Name))
                        //{
                        //    if (RowErrorCount > 0)
                        //    {
                        //        ErrorText += " | 기사명 빈값";
                        //    }
                        //    else
                        //    {
                        //        ErrorText += "기사명 빈값";

                        //    }
                        //    RowErrorCount++;
                        //}
                        //청구항목
                        if (String.IsNullOrEmpty(row.Item))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 청구항목 빈값";
                            }
                            else
                            {
                                ErrorText += "청구항목 빈값";

                            }
                            RowErrorCount++;
                        }
                        //청구일
                        DateTime oDate;
                        if (String.IsNullOrEmpty(row.Date) || !DateTime.TryParseExact(row.Date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out oDate))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 청구일 이상";
                            }
                            else
                            {
                                ErrorText += "청구일 이상";

                            }
                            RowErrorCount++;
                        }
                        //청구금액
                        decimal oNumber;
                        if (String.IsNullOrEmpty(row.Price) || !decimal.TryParse(row.Price, out oNumber))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 청구금액 이상";
                            }
                            else
                            {
                                ErrorText += "청구금액 이상";

                            }
                            RowErrorCount++;
                        }
                        //VAT
                        if (row.VAT != "1" && row.VAT != "2")
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | VAT 이상 ";
                            }
                            else
                            {
                                ErrorText += "VAT 이상";

                            }
                            RowErrorCount++;
                        }
                        //세금계산서
                        if (row.ETAX != "1" && row.ETAX != "0")
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 세금계산서 이상 ";
                            }
                            else
                            {
                                ErrorText += "세금계산서 이상";

                            }
                            RowErrorCount++;
                        }
                    }
                    if (RowErrorCount > 0)
                    {
                        errCount++;
                    }

                    row.Error = ErrorText;
                }
                DataCount = FCount - errCount;
                Invoke(new Action(() =>
                {
                    if (errCount == 0)
                    {
                        label4.Text = FCount.ToString() + " 건";
                        label5.Text = (FCount - errCount).ToString() + " 건";
                        label6.Text = errCount.ToString() + " 건";

                        label7.Visible = false;
                        btn_Update.Enabled = true;
                    }
                    else
                    {

                        label4.Text = FCount.ToString() + " 건";
                        label5.Text = (FCount - errCount).ToString() + " 건";
                        label6.Text = errCount.ToString() + " 건";

                        label7.Visible = true;
                    }
                    btn_Info.Enabled = true;
                    btn_Update.Enabled = true;
                    btn_OK.Enabled = true;
                    cmb_Savegubun.Enabled = true;
                    if (cmb_Savegubun.SelectedIndex == 0)
                    {
                        btn_Update.Enabled = true;
                    }
                    else
                    {
                        btn_Update.Enabled = false;
                    }
                    btn_Close.Enabled = true;
                }));
            });
        }
        int _TradeId = 0;
        private void UpdateDb()
        {
            
            string Todate = DateTime.Now.ToString("yyyy-MM-01");
            DateTime _BeginDate = DateTime.Parse(Todate).AddMonths(-1).Date;
            DateTime _EndDate = DateTime.Parse(Todate).AddDays(-1).Date;
            var InsertQuery =
                @"INSERT INTO [dbo].[Trades](DriverId,DriverName,RequestDate,Price,VAT,Amount,BeginDate,EndDate,PayDate,PayState,ClientId,UseTax,SetYN,Item, PayBankName, PayBankCode, PayAccountNo, PayInputName, HasAcc, AllowAcc,SubClientId,ClientUserId,trusteeMgtKey,HasETax)
                VALUES(@DriverId,@DriverName,@RequestDate,@Price,@VAT,@Amount,@BeginDate,@EndDate,@PayDate,2,@ClientId,@UseTax,0,@Item, @PayBankName, @PayBankCode, @PayAccountNo, @PayInputName, 1, @AllowAcc,@SubClientId,@ClientUserId,@trusteeMgtKey,@HasETax) SELECT @@Identity";
            if (DataCount == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = InsertQuery;
                    _Command.Parameters.Add("@DriverId", SqlDbType.Int);
                    _Command.Parameters.Add("@DriverName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@RequestDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@Price", SqlDbType.Decimal);
                    _Command.Parameters.Add("@VAT", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Amount", SqlDbType.Decimal);
                    _Command.Parameters.Add("@BeginDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@EndDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@PayDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                    _Command.Parameters.Add("@UseTax", SqlDbType.Bit);
                    _Command.Parameters.Add("@Item", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayBankName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayBankCode", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayAccountNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayInputName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@SubClientId", SqlDbType.Int);
                    _Command.Parameters.Add("@ClientUserId", SqlDbType.Int);
                    _Command.Parameters.Add("@AllowAcc", SqlDbType.Bit);
                    _Command.Parameters.Add("@trusteeMgtKey", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@HasETax", SqlDbType.Bit);
                    

                    foreach (Model row in _CoreList)
                    {
                        if (row.IDX != "" && row.Error == "")
                        {
                            int DriverId = baseDataSet.Drivers.First(c => c.CarNo.Replace(" ","") == row.CarNo).DriverId;
                          
                            bool UseTax = row.VAT == "1" ? false : true;

                            decimal _Price = 0;
                            decimal _VAT = 0;
                            decimal _Amount = 0;

                            //별도
                            if (UseTax == false)
                            {
                                _Price = decimal.Parse(row.Price);
                                _VAT = Math.Floor(_Price * 0.1m);
                                _Amount = (_Price + _VAT);
                            }
                            //포함
                            else
                            {
                                _Amount = decimal.Parse(row.Price);
                                _VAT = Math.Floor(_Amount - (decimal.Parse(row.Price) / 1.1m));
                                _Price = _Amount-_VAT;
                            }

                            _Command.Parameters["@DriverId"].Value = DriverId;
                            _Command.Parameters["@DriverName"].Value = row.Name;
                            _Command.Parameters["@RequestDate"].Value = DateTime.ParseExact(row.Date.Replace("-", ""), "yyyyMMdd", null);
                            _Command.Parameters["@Price"].Value = _Price;
                            _Command.Parameters["@VAT"].Value = _VAT;
                            _Command.Parameters["@Amount"].Value = _Amount;
                            _Command.Parameters["@BeginDate"].Value = DateTime.ParseExact(row.BeginDate.Replace("-", ""), "yyyyMMdd", null); ;
                            _Command.Parameters["@EndDate"].Value = DateTime.ParseExact(row.EndDate.Replace("-", ""), "yyyyMMdd", null); ;
                            _Command.Parameters["@PayDate"].Value = DateTime.Now.Date;
                            _Command.Parameters["@UseTax"].Value = true;
                            _Command.Parameters["@Item"].Value = row.Item;
                            _Command.Parameters["@PayBankName"].Value = row.PayBankName;
                            _Command.Parameters["@PayBankCode"].Value = row.PayBankCode;
                            _Command.Parameters["@PayAccountNo"].Value = row.PayAccountNo;
                            _Command.Parameters["@PayInputName"].Value = row.PayInputName;
                            _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                            _Command.Parameters["@SubClientId"].Value = DBNull.Value;
                            _Command.Parameters["@ClientUserId"].Value = DBNull.Value;
                            if (LocalUser.Instance.LogInInformation.IsSubClient)
                            {
                                _Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                                if (LocalUser.Instance.LogInInformation.IsAgent)
                                {
                                    _Command.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                                }
                            }
                            //안한다
                            if(row.ETAX == "0")
                            {
                                _Command.Parameters["@AllowAcc"].Value = true;
                                _Command.Parameters["@trusteeMgtKey"].Value = DBNull.Value;
                                _Command.Parameters["@HasETax"].Value = false;
                            }
                            else
                            {
                                _Command.Parameters["@AllowAcc"].Value = false;
                                _Command.Parameters["@trusteeMgtKey"].Value = "t" + DriverId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
                                _Command.Parameters["@HasETax"].Value = true;
                            }


                            Object O = _Command.ExecuteScalar();
                            if (O != null)
                                _TradeId = Convert.ToInt32(O);


                            if (row.ETAX == "0")
                            {
                                SqlCommand _MiziCommand = _Connection.CreateCommand();

                                _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                                _MiziCommand.Parameters.AddWithValue("@Driverid", DriverId);
                                _MiziCommand.Parameters.AddWithValue("@Mizi", _Amount.ToString().Replace(",", ""));
                                _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                _MiziCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                if (LocalUser.Instance.LogInInformation.Client.EtaxGubun == "P")
                                {
                                    if (SendInvoice(DriverId, _TradeId, LocalUser.Instance.LogInInformation.ClientId))
                                    {
                                        SqlCommand _MiziCommand = _Connection.CreateCommand();

                                        _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                                        _MiziCommand.Parameters.AddWithValue("@Driverid", DriverId);
                                        _MiziCommand.Parameters.AddWithValue("@Mizi", _Amount.ToString().Replace(",", ""));
                                        _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        _MiziCommand.ExecuteNonQuery();

                                        var _Driver = baseDataSet.Drivers.Where(c => c.DriverId == DriverId);
                                        ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                                        {
                                            ClientId = LocalUser.Instance.LogInInformation.ClientId,
                                            CDate = DateTime.Now,
                                            Amount = -110,
                                            MethodType = "전자 세금계산서",
                                            Remark = $"{_Driver.First().Name} ({_Driver.First().CarNo})",
                                        });
                                        ShareOrderDataSet.Instance.SaveChanges();
                                    }
                                    else
                                    {

                                        SqlCommand Deletecmd = _Connection.CreateCommand();
                                        Deletecmd.CommandText = " DELETE Trades WHERE TradeId = @TradeId ";

                                        Deletecmd.Parameters.AddWithValue("@TradeId", _TradeId);
                                        Deletecmd.ExecuteNonQuery();

                                    }
                                }
                                else
                                {
                                    if (SendNice(DriverId, _TradeId, LocalUser.Instance.LogInInformation.ClientId))
                                    {
                                        SqlCommand _MiziCommand = _Connection.CreateCommand();

                                        _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                                        _MiziCommand.Parameters.AddWithValue("@Driverid", DriverId);
                                        _MiziCommand.Parameters.AddWithValue("@Mizi", _Amount.ToString().Replace(",", ""));
                                        _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        _MiziCommand.ExecuteNonQuery();


                                        SqlCommand _BillNoCommand = _Connection.CreateCommand();

                                        _BillNoCommand.CommandText = "UPDATE Trades   SET BillNo = @BillNo,trusteeMgtKey = null WHERE TradeId = @TradeId ";
                                        _BillNoCommand.Parameters.AddWithValue("@BillNo", billNo);
                                        _BillNoCommand.Parameters.AddWithValue("@TradeId", _TradeId);


                                        _BillNoCommand.ExecuteNonQuery();





                                        var _Driver = baseDataSet.Drivers.Where(c => c.DriverId == DriverId);
                                        ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                                        {
                                            ClientId = LocalUser.Instance.LogInInformation.ClientId,
                                            CDate = DateTime.Now,
                                            Amount = -110,
                                            MethodType = "전자 세금계산서",
                                            Remark = $"{_Driver.First().Name} ({_Driver.First().CarNo})",
                                        });
                                        ShareOrderDataSet.Instance.SaveChanges();


                                    }
                                    else
                                    {

                                        SqlCommand Deletecmd = _Connection.CreateCommand();
                                        Deletecmd.CommandText = " DELETE Trades WHERE TradeId = @TradeId ";

                                        Deletecmd.Parameters.AddWithValue("@TradeId", _TradeId);
                                        Deletecmd.ExecuteNonQuery();


                                    }
                                }
                            }


                        }
                    }
                }
                _Connection.Close();
            }

            EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
            this.Close();
        }
        #endregion
        private string Memo = "▶운송/주선사用 '차세로' PC프로그램 안내 운송/주선사에서는 http://www.chasero.co.kr 접속하여 프로그램을 설치하시면, 화물차주가 발행한 '전자세금계산서' 및 '첨부화일(인수증/송장 등)'을 조회해 보실 수 있습니다. 문의: 1661 - 6090 ";
        private bool SendInvoice(int driverId, int tradeId, int clientId)
        {
            LoadTable();

            var _Driver = baseDataSet.Drivers.Where(c => c.DriverId == driverId);
            var _Client = clientDataSet.Clients.FirstOrDefault(c => c.ClientId == clientId);

            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            bool forceIssue = false;        // 지연발행 강제여부
            var TPrice = _Trade.Price;
            var TAmount = _Trade.Amount;
            var TVat = _Trade.VAT;

            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();


            taxinvoice.writeDate = DateTime.Now.ToString("yyyyMMdd");                      //필수, 기재상 작성일자
            taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
            taxinvoice.issueType = "위수탁";                        //필수, {정발행, 역발행, 위수탁}
            taxinvoice.purposeType = "청구";                        //필수, {영수, 청구}
            taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
            taxinvoice.taxType = "과세";                            //필수, {과세, 영세, 면세}

            taxinvoice.invoicerCorpNum = _Driver.First().BizNo.Replace("-", "");             //공급자 사업자번호
            //taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
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

            taxinvoice.modifyCode = null;                           //수정세금계산서 작성시 1~6까지 선택기재.
            taxinvoice.originalTaxinvoiceKey = "";                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
            taxinvoice.serialNum = "1";
            taxinvoice.cash = "";                                   //현금
            taxinvoice.chkBill = "";                                //수표
            taxinvoice.note = "";                                   //어음
            taxinvoice.credit = "";                                 //외상미수금
            if ((_Trade.TransportDate.ToString("d").Replace("-", "/") + " " + _Trade.StartState + "→" + _Trade.StopState + " ( ☎ " + _Driver.First().MobileNo + " )" + " (" + _Trade.PayBankName + "/" + _Trade.PayInputName + "/" + _Trade.PayAccountNo.Replace("\0", "") + ")").Length > 149)
            {
                taxinvoice.remark1 = (_Trade.TransportDate.ToString("d").Replace("-", "/") + " " + _Trade.StartState + "→" + _Trade.StopState + " ( ☎ " + _Driver.First().MobileNo + " )" + " (" + _Trade.PayBankName + "/" + _Trade.PayInputName + "/" + _Trade.PayAccountNo.Replace("\0", "") + ")").Substring(0, 149);
            }
            else
            {
                taxinvoice.remark1 = (_Trade.TransportDate.ToString("d").Replace("-", "/") + " " + _Trade.StartState + "→" + _Trade.StopState + " ( ☎ " + _Driver.First().MobileNo + " )" + " (" + _Trade.PayBankName + "/" + _Trade.PayInputName + "/" + _Trade.PayAccountNo.Replace("\0", "") + ")");
            }
            taxinvoice.remark2 = "";
            taxinvoice.remark3 = "";
            taxinvoice.kwon = 1;
            taxinvoice.ho = 1;

            taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
            taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

            string CtrusteeMgtKey = _Trade.trusteeMgtKey;
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
                Response response = taxinvoiceService.RegistIssue("1148654906", taxinvoice, forceIssue, Memo);


                return true;



            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");


                // MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return false;
            }
        }

        string billNo = "";
        string errMsg = "";
        private bool SendNice(int driverId, int tradeId, int clientId)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            errMsg = "";
            billNo = "";
            LoadTable();
            LocalUser.Instance.LogInInformation.LoadClient();
            var _Driver = baseDataSet.Drivers.Where(c => c.DriverId == driverId);
            var _Client = clientDataSet.Clients.FirstOrDefault(c => c.ClientId == clientId);
            var _Admininfo = baseDataSet.AdminInfoes.FirstOrDefault();

            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            bool forceIssue = false;        // 지연발행 강제여부
            var TPrice = _Trade.Price;
            var TAmount = _Trade.Amount;
            var TVat = _Trade.VAT;


            var _Clients = LocalUser.Instance.LogInInformation.Client;
            //   var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";


            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "dpebqlf54906**" }).ToString();
            string Tdtp_Date = _Trade.RequestDate.ToString("yyyy-MM-dd").Replace("-", "/");// dtp_RequestDate.Text;//DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");


            string _purposeType = "02";

            // TaxInvoiceErrorDic.Clear();




            string DutyNum = string.Empty;
            string DescriptionText = "";
            int i = 0;

            DescriptionText = "▶운송 / 주선사用 '차세로' PC프로그램 안내 운송 / 주선사에서는 http://www.chasero.co.kr 접속하여 프로그램을 설치하시면, 화물차주가 발행한 '전자세금계산서' 및 '첨부화일(인수증/송장 등)'을 조회해 보실 수 있습니다. 문의: 1661 - 6090";


            decimal _Price = 0;
            decimal _Vat = 0;
            decimal _Amont = 0;
            _Price = _Trade.Price;
            _Vat = _Trade.VAT;
            _Amont = _Trade.Amount;


            string dtiXml = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            " xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            "   <TaxInvoiceDocument>\r\n" +
            $"       <TypeCode>0103</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            $"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            $"      <IssueDateTime>{Tdtp_Date.Replace("/", "")}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            $"      <AmendmentStatusCode></AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            $"      <PurposeCode>{_purposeType}</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            $"      <OriginalIssueID></OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            $"  </TaxInvoiceDocument>\r\n" +
            $"  <TaxInvoiceTradeSettlement>\r\n" +
            $"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            $"          <ID>{_Driver.First().BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Driver.First().Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            $"          <NameText>{_Driver.First().Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Driver.First().Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Driver.First().CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <URICommunication>{_Driver.First().Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Driver.First().AddressState + " " + _Driver.First().AddressCity + " " + _Driver.First().AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
            $"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
            $"          <ID>{_Client.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Client.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
            $"          <NameText>{_Client.Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Client.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Client.CEO}</NameText>	<!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <PrimaryDefinedContact>\r\n" +
            $"              <PersonNameText>{_Client.CEO}</PersonNameText>	<!-- 담당자명 -->\r\n" +
            $"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <TelephoneCommunication>{_Client.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
            $"              <URICommunication>{_Client.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </PrimaryDefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Client.AddressState + " " + _Client.AddressCity + " " + _Client.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +
            $"      <BrokerParty> <!-- 수탁자정보 --> \r\n" +
            $"          <ID>{_Admininfo.BIzNo.Replace("-", "")}</ID>     <!-- 사업자번호 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>         <!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +

            $"          <NameText>{_Admininfo.CustName}</NameText>         <!-- 상호명 -->\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Admininfo.ownerName}</NameText>       <!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Admininfo.addr1 + " " + _Admininfo.addr2}</LineOneText>          <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"          <TypeCode>{_Admininfo.BizCond}</TypeCode>  <!-- 업태 -->\r\n" +
            $"          <ClassificationCode>{_Admininfo.bizItem}</ClassificationCode>   <!-- 종목 -->\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <DepartmentNameText>물류팀</DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <PersonNameText>{_Admininfo.rsbmName}</PersonNameText>      <!-- 담당자명 -->\r\n" +
            $"              <TelephoneCommunication>{_Admininfo.hpNo}</TelephoneCommunication>           <!-- 전화번호 -->\r\n" +
            $"             	<URICommunication>{_Admininfo.email}</URICommunication>  <!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"      </BrokerParty> <!-- 수탁자정보 끝 -->\r\n" +
            $"          <SpecifiedPaymentMeans>\r\n" +
            $"              <TypeCode>10</TypeCode> <!-- 10:현금, 20:수표, 30:어음, 40:외상미수금 -->\r\n" +
            $"              <PaidAmount>{Convert.ToInt64(_Amont)}</PaidAmount> <!-- 부가세가 포함된 금액 -->\r\n" +
            $"          </SpecifiedPaymentMeans>\r\n" +
            $"          <SpecifiedMonetarySummation>\r\n" +
            $"              <ChargeTotalAmount>{Convert.ToInt64(_Price)}</ChargeTotalAmount>	<!-- 총 공급가액 -->\r\n" +
            $"              <TaxTotalAmount>{Convert.ToInt64(_Vat)}</TaxTotalAmount>	<!-- 총 세액 -->\r\n" +
            $"              <GrandTotalAmount>{Convert.ToInt64(_Amont)}</GrandTotalAmount>	<!-- 총액(공급가액+세액) -->\r\n" +
            $"          </SpecifiedMonetarySummation>\r\n" +
            $"  </TaxInvoiceTradeSettlement>\r\n";

            dtiXml += "" +
              $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
              $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
              $"      <InvoiceAmount>{Convert.ToInt64(_Price)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
              $"      <ChargeableUnitQuantity>1</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
              $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
              //$"      <NameText>{"화물운송료(" + _Driver.First().CarNo + ")"}</NameText>  <!-- 품목명 -->\r\n" +
               $"      <NameText>{"화물운송료(" + _Trade.Item + ")"}</NameText>  <!-- 품목명 -->\r\n" +
              $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
              $"          <TotalTax>\r\n" +
              $"              <CalculatedAmount>{Convert.ToInt64(_Vat)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
              $"          </TotalTax>\r\n" +
              $"          <UnitPrice>\r\n" +
              $"              <UnitAmount>{Convert.ToInt64(_Price)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
              $"          </UnitPrice>\r\n" +
              $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
              $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";


            dtiXml += "" +
            "</TaxInvoice>";










            //try
            //{
            //    webBrowser1.Url = new Uri("http://222.231.9.253/NiceEncoding.asp");
            //    //frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.frnNo }).ToString();
            //    //userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.userid }).ToString();
            //    passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            //  //  Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.Linkcd }).ToString();


            //    //MessageBox.Show(Encode.ToString());

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.Write(ex.Message);

            //}



            // var Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Clients.frnNo, _Clients.userid, _Clients.passwd, certPw, dtiXml, "Y", "N", "", "6");
            var Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "", "3");

            try
            {
                var ResultList = Result.Split('/');

                var retVal = ResultList[0];
                errMsg = ResultList[1];
                billNo = ResultList[2];
                var gnlPoint = ResultList[3];
                var outbnsPoint = ResultList[4];
                var totPoint = ResultList[5];
                //return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

                if (retVal == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {

            }




            try
            {
                //Response response = taxinvoiceService.RegistIssue("1148654906", taxinvoice, forceIssue, Memo);


                return true;



            }
            catch (PopbillException ex)
            {

                return false;
            }
        }
        private void btn_Update_Click(object sender, EventArgs e)
        {
            UpdateDb();
        }
    }
}
