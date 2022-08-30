using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.IO;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.DataSets;
using Popbill.Taxinvoice;
using Popbill;
using mycalltruck.Admin.Class;

namespace mycalltruck.Admin
{
    public partial class FrmTrade_Add : Form
    {
        Int64 iPrice = 0;
        DESCrypt m_crypt = null;
        bool AllowTaxBool = false;
        Int32 _ServiceState = 0;
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;
        #region 전자세금계산서NiceDNR
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
        //string linkId = "edubillsys";
        #endregion
        public FrmTrade_Add()
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");
        }



        private void FrmTrade_Add_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            dtp_BeginDate.Value = DateTime.Parse(Todate).AddMonths(-1);
            dtp_EndDate.Value = DateTime.Parse(Todate).AddDays(-1);
            dtp_RequestDate.Value = DateTime.Now;


            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;

            _InitCmb();
            ClientLoad();
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(baseDataSet.Drivers);

            InitDriverTable();

            clientsTableAdapter.Fill(clientDataSet.Clients);
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);

            btn_InfoSearch_Click(null, null);

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

        private void _InitCmb()
        {
            Dictionary<string, string> PayBank = new Dictionary<string, string>();
            PayBank.Add(" ", "은행선택");
            PayBank.Add("003", "기업은행");
            PayBank.Add("005", "외한은행");
            PayBank.Add("004", "국민은행");
            PayBank.Add("011", "농협");
            PayBank.Add("020", "우리은행");
            PayBank.Add("088", "신한은행");
            PayBank.Add("023", "SC제일");
            PayBank.Add("027", "씨티은행");
            PayBank.Add("032", "부산은행");
            PayBank.Add("039", "경남은행");
            PayBank.Add("031", "대구은행");
            PayBank.Add("071", "우체국");
            PayBank.Add("034", "광주은행");
            PayBank.Add("007", "수협");
            PayBank.Add("022", "상업은행");
            PayBank.Add("030", "대동은행");
            PayBank.Add("033", "충청은행");
            PayBank.Add("035", "제주은행");
            PayBank.Add("036", "경기은행");
            PayBank.Add("037", "전북은행");
            PayBank.Add("038", "강원은행");
            PayBank.Add("040", "충북은행");
            PayBank.Add("081", "하나은행");
            PayBank.Add("082", "보람은행");
            PayBank.Add("002", "산업은행");
            PayBank.Add("045", "새마을금고");
            PayBank.Add("054", "HSBC은행");
            PayBank.Add("048", "신협");
            PayBank.Add("090", "카카오뱅크");
            PayBank.Add("089", "케이뱅크");

            PayBank.Add("S0", "유안타증권");
            PayBank.Add("S1", "미래에셋");
            PayBank.Add("S2", "신한금융투자");
            PayBank.Add("S3", "삼성증권");
            PayBank.Add("S6", "한국투자증권");
            PayBank.Add("SG", "한화증권");

        }

        private void ClientLoad()
        {


            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT AllowTax FROM Clients";

                    _Command.CommandText += Environment.NewLine + "WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);




                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {

                        while (_Reader.Read())
                        {
                            AllowTaxBool = _Reader.GetBooleanZ(0);
                        }


                    }
                }
                _Connection.Close();
            }

            if (AllowTaxBool == false)
            {

                rdb_Tax0.Checked = true;
            }
            else
            {
                rdb_Tax1.Checked = true;

            }
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
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() == 1)
            {
                if (ETaxN.Checked)
                {
                    MessageBox.Show("배차 외 결제건이 등록되었습니다.", "역발행");
                }
                else
                {
                    MessageBox.Show("역발행 전자세금계산서가 발행되었습니다.", "역발행");
                }
                string Todate = DateTime.Now.ToString("yyyy/MM/01");
                dtp_BeginDate.Value = DateTime.Parse(Todate).AddMonths(-1);
                dtp_EndDate.Value = DateTime.Parse(Todate).AddDays(-1);
                txt_Item.Text = "";
                dtp_RequestDate.Value = DateTime.Now;
                txt_Price.Text = "";
                txt_VAT.Text = "";
                ETaxN.Checked = true;
                if (AllowTaxBool == false)
                {
                    rdb_Tax0.Checked = true;
                }
                else
                {
                    rdb_Tax0.Checked = false;
                }
                txt_Amount.Text = "";
            }
            txt_Item.Focus();
        }

        private int _UpdateDB()
        {
            err.Clear();

            if (txt_Item.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }


            if (txt_Price.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }
            var _d = 0m;
            if (!decimal.TryParse(txt_Price.Text.Replace(",", ""), out _d) || !decimal.TryParse(txt_VAT.Text.Replace(",", ""), out _d) || !decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _d))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }
            else
            {
                decimal _Pirce = Convert.ToDecimal(txt_Price.Text.Replace(",", ""));
                if (_Pirce > 10000000)
                {
                    MessageBox.Show("10,000,000 원 까지만 입력 가능합니다.", "매입관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    err.SetError(txt_Price, "10,000,000 원 까지만 입력 가능합니다.");
                    return -1;
                }

            }

            try
            {
                if (ETaxY.Checked)
                {
                    var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

                    if (_ClientPoint < 150)
                    {

                        MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                        FrmMDI.LoadForm("FrmMN0204", "Point");
                        return 0;
                    }
                   


                    var Query = _DriverTable.Where(c => c.DriverId ==Convert.ToInt32(txt_DriverId.Text));
                    if (Query.Any())
                    {


                        if (MessageBox.Show($"상호 : {Query.First().Name} ({Query.First().CarNo})\r\n" +
                            $"기사 : {Query.First().CarYear}\r\n" +
                            $"금액 : {txt_Amount.Text}\r\n\r\n" +
                            $"역발행 전자세금계산서를 정말 발행하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            _AddClient();
                        }
                        else
                        {
                            return 0;
                        }
                    }
                        
                    

                    
                }
                else
                {
                    _AddClient();
                }
                if (!String.IsNullOrEmpty(errMsg))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Item") iName = "청구항목";
                if (code == "Price") iName = "청구금액";
                if (code == "PayBankName") iName = "은행명";
                if (code == "PayAccountNo") iName = "계좌번호";
                if (code == "PayInputName") iName = "예금주";

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        public CMDataSet.TradesRow CurrentCode = null;
        int _TradeId = 0;
        private void _AddClient()
        {
            try
            {
                if (driversBindingSource.Current == null)
                    return;
                var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
                if (Selected == null)
                    return;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand _DriverCommand = cn.CreateCommand();
                    _DriverCommand.CommandText = "SELECT PayBankName, PayBankCode, PayAccountNo, PayInputName FROM Drivers WHERE DriverId = @DriverId";
                    _DriverCommand.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                    var PayBankName = "";
                    var PayBankCode = "";
                    var PayAccountNo = "";
                    var PayInputName = "";
                    using (var _Reader = _DriverCommand.ExecuteReader())
                    {
                        if (_Reader.Read())
                        {
                            PayBankName = _Reader.GetStringN(0);
                            PayBankCode = _Reader.GetStringN(1);
                            PayAccountNo = _Reader.GetStringN(2);
                            PayInputName = _Reader.GetStringN(3);
                            try
                            {
                                try
                                {
                                    PayAccountNo = m_crypt.Decrypt(PayAccountNo).Replace("\0", string.Empty).Trim();
                                }
                                catch
                                {
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        " INSERT Trades (RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, UseTax, HasAcc, AllowAcc, SubClientId, ClientUserId,HasETax,trusteeMgtKey,SourceType)Values(@RequestDate, @BeginDate, @EndDate, @DriverName, @Item, @Price, @VAT, @Amount, @PayState, @PayDate, @PayBankName, @PayBankCode, @PayAccountNo, @PayInputName, @DriverId, @ClientId, @UseTax, @HasAcc, @AllowAcc, @SubClientId, @ClientUserId,@HasETax,@trusteeMgtKey,@SourceType)" +
                        " SELECT @@IDENTITY ";
                    cmd.Parameters.AddWithValue("@RequestDate", dtp_RequestDate.Value);
                    cmd.Parameters.AddWithValue("@BeginDate", dtp_BeginDate.Value);
                    cmd.Parameters.AddWithValue("@EndDate", dtp_EndDate.Value);
                    cmd.Parameters.AddWithValue("@DriverName", label83.Text);
                    cmd.Parameters.AddWithValue("@Item", txt_Item.Text);
                    cmd.Parameters.AddWithValue("@Price", txt_Price.Text.Replace(",", ""));
                    cmd.Parameters.AddWithValue("@VAT", txt_VAT.Text.Replace(",", ""));
                    cmd.Parameters.AddWithValue("@Amount", txt_Amount.Text.Replace(",", ""));
                    cmd.Parameters.AddWithValue("@PayState", 2);
                    cmd.Parameters.AddWithValue("@PayDate", dtp_RequestDate.Value);
                    cmd.Parameters.AddWithValue("@PayBankName", PayBankName);


                    cmd.Parameters.AddWithValue("@PayBankCode", PayBankCode);
                    cmd.Parameters.AddWithValue("@PayAccountNo", PayAccountNo);
                    cmd.Parameters.AddWithValue("@PayInputName", PayInputName);


                    cmd.Parameters.AddWithValue("@DriverId", txt_DriverId.Text);
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    
                    cmd.Parameters.AddWithValue("@SubClientId", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClientUserId", DBNull.Value);

                    //제공이 아닐때
                    if (_ServiceState != 1)
                    {
                        //현금
                        cmd.Parameters.AddWithValue("@HasAcc", 0);
                    }
                    else
                    {
                        //카드
                        cmd.Parameters.AddWithValue("@HasAcc",1);

                    }
                   
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        cmd.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            cmd.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                        }
                    }
                    cmd.Parameters.AddWithValue("@UseTax", true);

                    if (ETaxN.Checked)
                    {
                        cmd.Parameters.AddWithValue("@AllowAcc", true);
                        cmd.Parameters.AddWithValue("@HasETax", false);
                        cmd.Parameters.AddWithValue("@trusteeMgtKey", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@AllowAcc", false);
                        cmd.Parameters.AddWithValue("@HasETax", true);
                        cmd.Parameters.AddWithValue("@trusteeMgtKey", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@trusteeMgtKey", "t" + txt_DriverId.Text + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    }
                    cmd.Parameters.AddWithValue("@SourceType", 0);





                    //   cmd.ExecuteNonQuery();
                    Object O = cmd.ExecuteScalar();
                    if (O != null)
                        _TradeId = Convert.ToInt32(O);


                    if (ETaxN.Checked)
                    {
                        SqlCommand _MiziCommand = cn.CreateCommand();

                        _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                        _MiziCommand.Parameters.AddWithValue("@Driverid", txt_DriverId.Text);
                        _MiziCommand.Parameters.AddWithValue("@Mizi", txt_Amount.Text.Replace(",", ""));
                        _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                        _MiziCommand.ExecuteNonQuery();
                    }
                    else
                    {

                        if (LocalUser.Instance.LogInInformation.Client.EtaxGubun == "P")
                        {

                            if (SendInvoice(Convert.ToInt32(txt_DriverId.Text), _TradeId, LocalUser.Instance.LogInInformation.ClientId))
                            {
                                SqlCommand _MiziCommand = cn.CreateCommand();

                                _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                                _MiziCommand.Parameters.AddWithValue("@Driverid", txt_DriverId.Text);
                                _MiziCommand.Parameters.AddWithValue("@Mizi", txt_Amount.Text.Replace(",", ""));
                                _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                _MiziCommand.ExecuteNonQuery();


                                var _Driver = _DriverTable.Where(c => c.DriverId == Convert.ToInt32(txt_DriverId.Text));
                                ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                                {
                                    ClientId = LocalUser.Instance.LogInInformation.ClientId,
                                    CDate = DateTime.Now,
                                    Amount = -110,
                                    MethodType = "전자 세금계산서",
                                    Remark = $"{txt_DriverName.Text} ({_Driver.First().CarNo})",
                                });
                                ShareOrderDataSet.Instance.SaveChanges();


                            }
                            else
                            {

                                SqlCommand Deletecmd = cn.CreateCommand();
                                Deletecmd.CommandText = " DELETE Trades WHERE TradeId = @TradeId ";

                                Deletecmd.Parameters.AddWithValue("@TradeId", _TradeId);
                                Deletecmd.ExecuteNonQuery();

                            }
                        }
                        else
                        {
                            if (SendNice(Convert.ToInt32(txt_DriverId.Text), _TradeId, LocalUser.Instance.LogInInformation.ClientId))
                            {
                                SqlCommand _MiziCommand = cn.CreateCommand();

                                _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                                _MiziCommand.Parameters.AddWithValue("@Driverid", txt_DriverId.Text);
                                _MiziCommand.Parameters.AddWithValue("@Mizi", txt_Amount.Text.Replace(",", ""));
                                _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                _MiziCommand.ExecuteNonQuery();


                                SqlCommand _BillNoCommand = cn.CreateCommand();

                                _BillNoCommand.CommandText = "UPDATE Trades   SET BillNo = @BillNo WHERE TradeId = @TradeId ";
                                _BillNoCommand.Parameters.AddWithValue("@BillNo", billNo);
                                _BillNoCommand.Parameters.AddWithValue("@TradeId", _TradeId);
                             

                                _BillNoCommand.ExecuteNonQuery();


                                


                                var _Driver = _DriverTable.Where(c => c.DriverId == Convert.ToInt32(txt_DriverId.Text));
                                ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                                {
                                    ClientId = LocalUser.Instance.LogInInformation.ClientId,
                                    CDate = DateTime.Now,
                                    Amount = -110,
                                    MethodType = "전자 세금계산서",
                                    Remark = $"{txt_DriverName.Text} ({_Driver.First().CarNo})",
                                });
                                ShareOrderDataSet.Instance.SaveChanges();


                            }
                            else
                            {

                                SqlCommand Deletecmd = cn.CreateCommand();
                                Deletecmd.CommandText = " DELETE Trades WHERE TradeId = @TradeId ";

                                Deletecmd.Parameters.AddWithValue("@TradeId", _TradeId);
                                Deletecmd.ExecuteNonQuery();
                                MessageBox.Show("세금계산서 추가 실패하였습니다.\r\n" + errMsg, Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                              
                            }

                        }
                    }
                    cn.Close();

                }
            }
            catch(Exception ex)
            {
                
            }

        }
        private string Memo = "▶운송/주선사用 '차세로' PC프로그램 안내 운송/주선사에서는 http://www.chasero.co.kr 접속하여 프로그램을 설치하시면, 화물차주가 발행한 '전자세금계산서' 및 '첨부화일(인수증/송장 등)'을 조회해 보실 수 있습니다. 문의: 1661 - 6090 ";
        private bool SendInvoice(int driverId, int tradeId, int clientId)
        {
            LoadTable();

            var _Driver = _DriverTable.Where(c => c.DriverId == driverId);
            var _Client = clientDataSet.Clients.FirstOrDefault(c => c.ClientId == clientId);

            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            bool forceIssue = false;        // 지연발행 강제여부
            var TPrice = _Trade.Price;
            var TAmount = _Trade.Amount;
            var TVat = _Trade.VAT;

            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();


            taxinvoice.writeDate = dtp_RequestDate.Text.Replace("/", ""); // DateTime.Now.ToString("yyyyMMdd");                      //필수, 기재상 작성일자
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
            taxinvoice.remark1 = _Trade.TransportDate.ToString("d").Replace("-", "/") + " " + _Trade.StartState + " → " + _Trade.StopState + " ( ☎ " + _Driver.First().MobileNo + " )" + " (" + _Trade.PayBankName + "/" + _Trade.PayInputName + "/" + _Trade.PayAccountNo.Replace("\0", "") + ")";
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
            detail.purchaseDT = dtp_RequestDate.Text.Replace("/", "");//DateTime.Now.ToString("yyyyMMdd");                         //거래일자
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
        string errMsg ="";
        string billNo = "";
        private bool SendNice(int driverId, int tradeId, int clientId)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            errMsg = "";
            billNo = "";
            LoadTable();
            LocalUser.Instance.LogInInformation.LoadClient();
            var _Driver = _DriverTable.Where(c => c.DriverId == driverId);
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
            string Tdtp_Date = dtp_RequestDate.Text;//DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");


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
              $"      <NameText>{txt_Item.Text}</NameText>  <!-- 품목명 -->\r\n" +
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
            var Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "","3");

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
        public bool IsSuccess = false;

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() == 1)
            {
                if (ETaxN.Checked)
                {
                    MessageBox.Show("배차 외 결제건이 등록되었습니다.", "역발행");
                }
                else
                {
                    MessageBox.Show("역발행 전자세금계산서가 발행되었습니다.", "역발행");
                }
                IsSuccess = true;
                Close();
            }
            else 
            {
                Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txt_Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == 45))
            {
                e.Handled = true;
            }
        }

        private void txt_Price_Leave(object sender, EventArgs e)
        {
            if (AllowTaxBool == false)
            {
                decimal _Price = 0;
                decimal _Vat = 0;
                decimal _Amount = 0;
                if (decimal.TryParse(txt_Price.Text.Replace(",", ""), out _Price))
                {
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }
        }

        private void btn_InfoSearch_Click(object sender, EventArgs e)
        {
            dataGridView2.AutoGenerateColumns = false;
            driversBindingSource.Filter = $"ServiceState <> 5 AND (Name LIKE '%{txt_InfoSearch.Text}%' OR CarNo LIKE '%{txt_InfoSearch.Text}%' OR CarYear LIKE '%{txt_InfoSearch.Text}%')";
            dataGridView2.ClearSelection();
        }

        private void btn_InfoSearch_Clear_Click(object sender, EventArgs e)
        {
            txt_InfoSearch.Text = "";
            btn_InfoSearch_Click(null, null);
        }

        private void txt_InfoSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_InfoSearch_Click(null, null);
        }

        private void btnExcelDown_Click(object sender, EventArgs e)
        {
            string fileString = "지입차일괄역발행양식_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.지입차일괄역발행양식;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.TRADE);

            if (di.Exists == false)
            {
                di.Create();
            }
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            try
            {
                if (file.Exists)
                {
                    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    file.Delete();
                }
                File.WriteAllBytes(FileName, ieExcel);
            }
            catch
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 이름의 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            System.Diagnostics.Process.Start(FileName);
        }

        private void btnExcelInsert_Click(object sender, EventArgs e)
        {

            EXCELINSERT_Trade _Form = new EXCELINSERT_Trade();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void txt_Price_Enter(object sender, EventArgs e)
        {
            txt_Price.Text = txt_Price.Text.Replace(",", "");
        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            txt_Price.Text = "";
            txt_VAT.Text = "";
            txt_Amount.Text = "";
            if (rdb_Tax0.Checked)
            {
                txt_Amount.ReadOnly = true;
                txt_Price.ReadOnly = false;
                lbl_Price.ForeColor = Color.Blue;
                lbl_Amt.ForeColor = Color.Black;
                AllowTaxBool = false;
                txt_Price.Focus();
            }
            else
            {
                txt_Amount.ReadOnly = false;
                txt_Price.ReadOnly = true;
                lbl_Price.ForeColor = Color.Black;
                lbl_Amt.ForeColor = Color.Blue;
                AllowTaxBool = true;
                txt_Amount.Focus();
            }
        }

        private void txt_Amount_Leave(object sender, EventArgs e)
        {
            if (AllowTaxBool == true)
            {
                decimal _Price = 0;
                decimal _Vat = 0;
                decimal _Amount = 0;
                if (decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _Amount))
                {
                    _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }
        }

        private void txt_Amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Amount_Enter(object sender, EventArgs e)
        {
            txt_Amount.Text = txt_Amount.Text.Replace(",", "");
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            if (driversBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null)
                return;
            label83.Text = Selected.Name;
            txt_DriverId.Text = Selected.DriverId.ToString();
            _ServiceState = Selected.ServiceState;
            txt_BizNo.Text = Selected.BizNo;
            txt_Ceo.Text = Selected.Name;
            txt_DriverName.Text = Selected.CarYear;
        }

        private void txt_BizNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void AllowAcc_CheckedChanged(object sender, EventArgs e)
        {
          //  AllowAcc.Checked = true;
        }

        private void ETaxN_CheckedChanged(object sender, EventArgs e)
        {
            if(ETaxN.Checked)
            {
                btnAddClose.Text = "등록후닫기(F6)";
            }
            else
            {
                btnAddClose.Text = "전자세금발행(F6)";
            }
        }
    }
}
