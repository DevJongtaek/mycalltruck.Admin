using CedaPdfMakerDll;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using Newtonsoft.Json.Linq;
using Popbill;
using Popbill.Taxinvoice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMNTest : Form
    {
      

        string _Sdate = "";
        string _Edate = "";
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;
        DESCrypt m_crypt = null;
        public FrmMNTest()
        {
            m_crypt = new DESCrypt("12345678");
            InitializeComponent();
          
        }

        private void FrmMNTest_Load(object sender, EventArgs e)
        {
            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();

                using (SqlCommand _UpdateCommand2 = _Connection.CreateCommand())
                {



                    _UpdateCommand2.CommandText = "UPDATE TestResult SET testyn = 'N',TestResult = 0 ";

                    _UpdateCommand2.ExecuteNonQuery();

                }
                _Connection.Close();



            }

            LocalUser.Instance.LogInInformation.LoadClient();
            InitializeStorage();
            GetModels();
        }
        #region VIEW
        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var _Model = _BindingList[e.RowIndex];




            if (e.ColumnIndex == colTestContents.Index)
            {
                e.Value = _Model.TestContents1; //+ " : " + _Model.TestContents2 + "   " + _Model.TestContents3 + " : " + _Model.TestContents4 + "  " + _Model.TestContentsRemark;
            }
            else if (e.ColumnIndex == colTestResult.Index)
            {
                if (_Model.TestYn == "Y")
                {
                    if ((bool?)_Model.TestResult == true)
                        e.Value = "성공";
                    else
                        e.Value = "실패";
                }
                else
                {
                    e.Value = "";
                }

            }

            



        }
        #endregion
        #region Storage
        class Model : INotifyPropertyChanged
        {
            private int _TestIdx  ;
            private string _TestTile;
            private string _TestContents1;
            private string _TestContents2;
            private string _TestContents3;
            private string _TestContents4;
            private string _TestContentsRemark;
            private bool? _TestResult;
            private string _BtnYn;
            private string _TestYn;

            public String SMID { get; set; }


            public int TestIdx
            {
                get
                {
                    return _TestIdx;
                }

                set
                {
                    SetField(ref _TestIdx, value);
                }
            }

            public string TestTile
            {
                get
                {
                    return _TestTile;
                }

                set
                {
                    SetField(ref _TestTile, value);
                }
            }

            public string TestContents1
            {
                get
                {
                    return _TestContents1;
                }

                set
                {
                    SetField(ref _TestContents1, value);
                }
            }

            public string TestContents2
            {
                get
                {
                    return _TestContents2;
                }

                set
                {
                    SetField(ref _TestContents2, value);
                }
            }

            public string TestContents3
            {
                get
                {
                    return _TestContents3;
                }

                set
                {
                    SetField(ref _TestContents3, value);
                }
            }

            public string TestContents4
            {
                get
                {
                    return _TestContents4;
                }

                set
                {
                    SetField(ref _TestContents4, value);
                }
            }
            public string TestContentsRemark
            {
                get
                {
                    return _TestContentsRemark;
                }

                set
                {
                    SetField(ref _TestContentsRemark, value);
                }
            }
            


            public bool? TestResult
            {
                get
                {
                    return _TestResult;
                }

                set
                {
                    SetField(ref _TestResult, value);
                }
            }

            
            public string BtnYn
            {
                get
                {
                    return _BtnYn;
                }

                set
                {
                    SetField(ref _BtnYn, value);
                }
            }
            public string TestYn
            {
                get
                {
                    return _TestYn;
                }

                set
                {
                    SetField(ref _TestYn, value);
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

        }
        BindingList<Model> _BindingList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _BindingList;
        }
        #endregion
        #region UPDATE
        private void GetModels()
        {
            _BindingList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _TradeCommand = _Connection.CreateCommand())
                {
                   
                    _TradeCommand.CommandText = "select TestIdx, TestTile, TestContents1, TestContents2, TestContents3, TestContents4, TestContentsRemark, TestResult,BtnYn,TestYn from TestResult";
                    using (SqlDataReader _Reader = _TradeCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var TestIdx = _Reader.GetInt32(0);
                            //if (_BindingList.Any(c => c.DriverId == DriverId))
                            //    continue;
                            var Added = new Model
                            {
                                TestIdx = TestIdx,
                                TestTile = _Reader.GetStringN(1),
                                TestContents1 = _Reader.GetStringN(2),
                                TestContents2 = _Reader.GetStringN(3),
                                TestContents3 = _Reader.GetStringN(4),
                                TestContents4 = _Reader.GetStringN(5),
                                TestContentsRemark = _Reader.GetStringN(6),
                                TestResult = _Reader.GetBoolean(7),
                                BtnYn = _Reader.GetStringN(8),
                                TestYn = _Reader.GetStringN(9),

                            };
                            _BindingList.Add(Added);
                        }
                    }
                }
                _Connection.Close();
            }
        }
        #endregion

        private void newDGV1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var _Model = _BindingList[e.RowIndex];


            if (e.ColumnIndex == colCode.Index)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (_Model != null)
                {
                    try
                    {
                        if (_Model.BtnYn == "N")
                        {
                            if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                            {
                                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                                else
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                            }
                            e.Handled = true;
                        }
                    }
                    catch (Exception)
                    {
                        if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                        {
                            if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                            else
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                        }
                        e.Handled = true;
                    }
                }
            }
        }

        int Error = 0;
        int Success = 0;
        string Gubun = "";
        string _staxType = "과세";
        string _invoicerMgtKey = "";
        private Dictionary<int, String> TaxInvoiceErrorDic = new Dictionary<int, string>();
        private bool SendInvoice()
        {
            Success = 0;
            decimal _Amount = 1004;
            decimal _Price = (_Amount / 1.1m);
            decimal _VAT = _Amount - _Price;

            
            #region 전자세금계산서발행 
            clientsTableAdapter.Fill(clientDataSet.Clients);
            var Query2 = clientDataSet.Clients.FindByClientId(21);
            var Query = clientDataSet.Clients.FindByClientId(21);



            string Tdtp_Date = DateTime.Now.ToString("yyyy-MM-dd");


            string _purposeType = "영수";

            _invoicerMgtKey = "Test" + Query2.ClientId + "-" + DateTime.Now.ToString("yyMMddhhmmss");
            TaxInvoiceErrorDic.Clear();






            string DutyNum = string.Empty;
           // int i = 0;



            bool forceIssue = false;        // 지연발행 강제여부
           // String memo = "";  // 즉시발행 메모 

            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();


            taxinvoice.writeDate = Tdtp_Date.Replace("-", "");                      //필수, 기재상 작성일자
            taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
            taxinvoice.issueType = "정발행";                        //필수, {정발행, 역발행, 위수탁}
            taxinvoice.purposeType = _purposeType;                        //필수, {영수, 청구}
            taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
            taxinvoice.taxType = _staxType;                            //필수, {과세, 영세, 면세}

            taxinvoice.invoicerCorpNum = Query2.BizNo.Replace("-", "");              //공급자 사업자번호
            taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
            taxinvoice.invoicerCorpName = Query2.Name;
            taxinvoice.invoicerMgtKey = _invoicerMgtKey;           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoicerCEOName = Query2.CEO;
            taxinvoice.invoicerAddr = Query2.AddressState + " " + Query2.AddressCity + " " + Query2.AddressDetail;
            taxinvoice.invoicerBizClass = Query2.Upjong;
            taxinvoice.invoicerBizType = Query2.Uptae;
            taxinvoice.invoicerContactName = Query2.CEO;
            taxinvoice.invoicerEmail = Query2.Email;
            taxinvoice.invoicerTEL = Query2.PhoneNo;
            taxinvoice.invoicerHP = Query2.MobileNo;
            taxinvoice.invoicerSMSSendYN = false;                    //정발행시(공급자->공급받는자) 문자발송기능 사용시 활용

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            taxinvoice.invoiceeType = "사업자";                     //공급받는자 구분, {사업자, 개인, 외국인}
            taxinvoice.invoiceeCorpNum = Query.BizNo.Replace("-", "");              //공급받는자 사업자번호
            taxinvoice.invoiceeCorpName = Query.Name;
            taxinvoice.invoiceeMgtKey = "";                         //역발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoiceeCEOName = Query.CEO;
            taxinvoice.invoiceeAddr = Query.AddressState + " " + Query.AddressCity + " " + Query.AddressDetail;
            taxinvoice.invoiceeBizClass = Query.Upjong.ToString();
            taxinvoice.invoiceeBizType = Query.Uptae.ToString();




            taxinvoice.invoiceeContactName1 = Query.CEO;
            taxinvoice.invoiceeEmail1 = Query.Email;
            taxinvoice.invoiceeHP1 = Query.MobileNo;
            taxinvoice.invoiceeTEL1 = Query.PhoneNo;


            taxinvoice.invoiceeSMSSendYN = false;                   //역발행시(공급받는자->공급자) 문자발송기능 사용시 활용

            taxinvoice.supplyCostTotal = Convert.ToUInt64(_Price).ToString();                 //필수 공급가액 합계
            taxinvoice.taxTotal = Convert.ToUInt64(_VAT).ToString();                          //필수 세액 합계
            taxinvoice.totalAmount = Convert.ToUInt64(_Amount).ToString();                     //필수 합계금액.  공급가액 + 세액

            taxinvoice.modifyCode = null;                           //수정세금계산서 작성시 1~6까지 선택기재.
            taxinvoice.originalTaxinvoiceKey = "";                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
            taxinvoice.serialNum = "1";
            taxinvoice.cash = "";                                   //현금
            taxinvoice.chkBill = "";                                //수표
            taxinvoice.note = "";                                   //어음
            taxinvoice.credit = "";                                 //외상미수금
            taxinvoice.remark1 = "";
            

            taxinvoice.remark2 = "";
            taxinvoice.remark3 = "";
            taxinvoice.kwon = 1;
            taxinvoice.ho = 1;

            taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
            taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

            taxinvoice.detailList = new List<TaxinvoiceDetail>();

            TaxinvoiceDetail detail = new TaxinvoiceDetail();


            detail.serialNum = 1;                                   //일련번호
                                                                    // detail.purchaseDT = item["CREATE_DATE1"].ToString().Replace("/", "");                         //거래일자
            detail.purchaseDT = Tdtp_Date.Replace("-", "");                         //거래일자
            detail.itemName = "주선사 충전금";
            detail.spec = "";
            detail.qty = "1";                                       //수량
            detail.unitCost = Convert.ToUInt64(_Price).ToString();                     //단가
            detail.supplyCost = Convert.ToUInt64(_Price).ToString();                          //공급가액
            detail.tax = Convert.ToUInt64(_VAT).ToString();                                   //세액
            detail.remark = "";



            taxinvoice.detailList.Add(detail);




            detail = new TaxinvoiceDetail();

            try
            {
                Response response = taxinvoiceService.RegistIssue(Query2.BizNo.Replace("-", ""), taxinvoice, forceIssue, "");

                try
                {
                    
                    Success++;
                   
                }
                catch (PopbillException ex)
                {
                  

                    Error++;
                }
            }
            catch (PopbillException ex)
            {
               // TaxInvoiceErrorDic.Add(_Model.ClientPointId, "[ " + ex.code.ToString() + " ] " + ex.Message);

                Error++;
            }


            #endregion

            if(Success > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
             
        }


        private bool SendCancelIssue(string _PinvoicerMgtKey)
        {

            try
            {
             

                Response response = taxinvoiceService.CancelIssue("1131183754", MgtKeyType.SELL, _PinvoicerMgtKey, "", "edubill365");

                return true;
            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);

                return false;

            }
        }

        private bool SendDelete(string _DinvoicerMgtKey)
        {

            try
            {
                clientsTableAdapter.Fill(clientDataSet.Clients);
                var Query = clientDataSet.Clients.Where(c => c.ClientId == 21);


               
                Response response = taxinvoiceService.Delete("1131183754", MgtKeyType.SELL, _DinvoicerMgtKey, "edubill365");


                try
                {
                    //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    //{
                    //    cn.Open();
                    //    SqlCommand cmd = cn.CreateCommand();
                    //    cmd.CommandText =
                    //           $"UPDATE ClientPoints SET EtaxWriteDate = @EtaxWriteDate, Issue = 0, IssueState = '취소'   WHERE    ClientPointId ={clientPointId}";
                    //    cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);

                    //    cmd.ExecuteNonQuery();





                    //    cn.Close();


                    //}
                    //MessageBox.Show("전자세금계산서가 삭제 되었습니다.");
                    ////Success++;
                    //btnSearch_Click(null, null);
                    return true;
                }

                catch (PopbillException ext)
                {
                    // MessageBox.Show(ext.code.ToString() + "\r\n" + ext.Message);
                    return false;
                }



            }
            catch (PopbillException ex)
            {

                //MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return false;
            }




        }

        public static string EncryptSHA512(string Data)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        string SmsResult = "";


        private void newDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var _Model = _BindingList[e.RowIndex];




            if (e.ColumnIndex == colCode.Index)
            {
                if (_Model != null)
                {
                    if (_Model.TestTile == "신용카드승인")
                    {


                        pnProgress.Visible = true;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();
                                    using (SqlCommand _AuthKeyCommand = _Connection.CreateCommand())
                                    using (SqlCommand _UpdateCommand = _Connection.CreateCommand())
                                    {
                                        _AuthKeyCommand.CommandText = "UPDATE Drivers SET AuthKey = @AuthKey WHERE DriverId = @DriverId";
                                        _AuthKeyCommand.Parameters.Add("@AuthKey", SqlDbType.NVarChar);
                                        _AuthKeyCommand.Parameters.Add("@DriverId", SqlDbType.Int);
                                        _UpdateCommand.CommandText = "UPDATE Drivers SET AuthKey = N'', TESTError = @Error WHERE DriverId = @DriverId";
                                        _UpdateCommand.Parameters.Add("@Error", SqlDbType.NVarChar);
                                        _UpdateCommand.Parameters.Add("@DriverId", SqlDbType.Int);

                                        _UpdateCommand.CommandText = "UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE TestIdx = 1 " +
                                        " UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE TestIdx = 2";

                                        _UpdateCommand.Parameters.Add("@TestResult", SqlDbType.Bit);



                                        var _AuthKey = Guid.NewGuid().ToString().Substring(0, 10);
                                        var _DriverId = 261;
                                        var _CardNo = "5584204000616426";
                                        var _CardDate = "2106";
                                        var _CardPan = "52";
                                        var _MID = "m83754001";
                                        _AuthKeyCommand.Parameters["@AuthKey"].Value = _AuthKey;
                                        _AuthKeyCommand.Parameters["@DriverId"].Value = _DriverId;
                                        _AuthKeyCommand.ExecuteNonQuery();
                                        WebClient mWebClient = new WebClient();
                                        mWebClient.Encoding = Encoding.UTF8;
                                        String _AccountNo = "99310101157930";
                                        string Parameter = "?sPrameter=" + String.Join("^", new object[] { _DriverId, _AuthKey, _CardNo, _CardDate, _CardPan, _MID, _AccountNo });
                                        var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/CardTEST" + Parameter));

                                        // var r = mWebClient.DownloadString(new Uri("http://localhost/Pay/CardTEST" + Parameter));
                                        bool _r = false;
                                        if (bool.TryParse(r, out _r))
                                        {
                                            if (_r)
                                            {
                                                _Model.TestResult = true;
                                                //_Model.Error = "";
                                            }
                                            else
                                            {
                                                _Model.TestResult = false;
                                                // _Model.Error = "알 수 없는 오류로 테스트를 실패하였습니다.";
                                            }
                                        }
                                        else
                                        {
                                            _Model.TestResult = false;
                                            //_Model.Error = r;
                                        }
                                        _UpdateCommand.Parameters["@Error"].Value = "";
                                        _UpdateCommand.Parameters["@DriverId"].Value = _DriverId;
                                        // _UpdateCommand.ExecuteNonQuery();

                                        _UpdateCommand.Parameters["@TestResult"].Value = _Model.TestResult;
                                        //_UpdateCommand.Parameters["@TestIdx"].Value = _Model.TestIdx;
                                        _UpdateCommand.ExecuteNonQuery();

                                    }
                                    _Connection.Close();
                                }
                                Invoke(new Action(() => MessageBox.Show("승인테스트를 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));

                            }
                            catch (Exception ex)
                            {

                                Invoke(new Action(() => MessageBox.Show("승인테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));
                            }
                            Invoke(new Action(() => pnProgress.Visible = false));
                        });
                    }

                    else if (_Model.TestTile == "전자세금계산서발행")
                    {

                        pnProgress.Visible = true;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                if (SendInvoice())
                                {
                                    if (SendCancelIssue(_invoicerMgtKey))
                                    {

                                        if (SendDelete(_invoicerMgtKey))
                                        {
                                            try
                                            {
                                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                                {
                                                    cn.Open();
                                                    SqlCommand cmd = cn.CreateCommand();
                                                    cmd.CommandText =
                                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =3 " +
                                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =4";
                                                    cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                                    cmd.Parameters.AddWithValue("@TestResult", 1);
                                                    cmd.ExecuteNonQuery();





                                                    cn.Close();

                                                }
                                            }
                                            catch
                                            {
                                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                                {
                                                    cn.Open();
                                                    SqlCommand cmd = cn.CreateCommand();
                                                    cmd.CommandText =
                                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =3 " +
                                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =4";
                                                    cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                                    cmd.Parameters.AddWithValue("@TestResult", 0);

                                                    cmd.ExecuteNonQuery();





                                                    cn.Close();

                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                            {
                                                cn.Open();
                                                SqlCommand cmd = cn.CreateCommand();
                                                cmd.CommandText =
                                                       $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =3 " +
                                                       $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =4";
                                                cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@TestResult", 0);

                                                cmd.ExecuteNonQuery();





                                                cn.Close();

                                            }
                                        }
                                    }
                                    else
                                    {
                                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                        {
                                            cn.Open();
                                            SqlCommand cmd = cn.CreateCommand();
                                            cmd.CommandText =
                                                     $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =3 " +
                                                       $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =4";
                                            cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@TestResult", 0);

                                            cmd.ExecuteNonQuery();





                                            cn.Close();

                                        }
                                    }
                                }
                                else
                                {
                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                                 $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =3 " +
                                                   $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =4";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 0);

                                        cmd.ExecuteNonQuery();





                                        cn.Close();

                                    }
                                }

                                Invoke(new Action(() => MessageBox.Show("발행테스트를 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));
                            }

                            catch (Exception ex)
                            {

                                Invoke(new Action(() => MessageBox.Show("발행테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));
                            }
                            Invoke(new Action(() => pnProgress.Visible = false));

                        });



                    }

                    else if (_Model.TestTile == "전자인수증 생성")
                    {
                        int _TradeId = 0;
                        pnProgress.Visible = true;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                              
                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();
                                    using (SqlCommand _NewCommand = _Connection.CreateCommand())
                                    {
                                        _NewCommand.CommandText = "INSERT Trades (RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, trusteeMgtKey, EtaxCanCelYN, ExcelExportYN, TransportDate, StartState, StopState, PdfFileName, AipId, DocId, DeleteYn, UpdateDateTime, Splits, EtaxUserId, EtaxUserName, TaxGubun, TaxLoginId)" +
                                            " SELECT RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, trusteeMgtKey, EtaxCanCelYN, ExcelExportYN, TransportDate, StartState, StopState, PdfFileName, AipId, DocId, DeleteYn, UpdateDateTime, Splits, EtaxUserId, EtaxUserName, TaxGubun, TaxLoginId FROM TradesTest  " +
                                            " SELECT @@Identity";


                                        _TradeId = Convert.ToInt32(_NewCommand.ExecuteScalar());


                                        try
                                        {
                                            SqlCommand cmd = _Connection.CreateCommand();
                                            cmd.CommandText =
                                                   " INSERT INTO DocuTable(TradeId,FileDirectory,Image1Name,Status,SignLocation) " +
                                                    "Values(@TradeId,@FilePath,@Image1,'_10_ready','LU')";
                                            cmd.Parameters.AddWithValue("@TradeId", _TradeId);
                                            cmd.Parameters.AddWithValue("@FilePath", @"D:\Cardpay-Data\Tax_Images\" + "3713");
                                            cmd.Parameters.AddWithValue("@Image1", "3713_20190613154007307.jpg");
                                            cmd.ExecuteNonQuery();


                                        }
                                        catch
                                        {

                                        }
                                    }
                                    _Connection.Close();
                                }



                                using (CedaPdfMakerDll.CedaPdfMaker lCedaPdfMaker = new CedaPdfMakerDll.CedaPdfMaker())
                                {
                                    lCedaPdfMaker.MakePdf(_TradeId.ToString());

                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =5 " +
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =6";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 1);
                                        cmd.ExecuteNonQuery();


                                        cmd.CommandText =
                                         "delete DocuTable WHERE TradeId = @TradeId ";
                                        cmd.Parameters.AddWithValue("@TradeId", _TradeId);

                                        cmd.ExecuteNonQuery();




                                        cn.Close();

                                    }
                                }
                                Invoke(new Action(() => MessageBox.Show("승인테스트를 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));



                            }

                            catch (Exception ex)
                            {
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =5 " +
                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =6";
                                    cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@TestResult", 0);
                                    cmd.ExecuteNonQuery();


                                    cmd.CommandText =
                                           "delete DocuTable WHERE TradeId = @TradeId ";
                                    cmd.Parameters.AddWithValue("@TradeId", _TradeId);

                                    cmd.ExecuteNonQuery();



                                    cn.Close();

                                }
                                Invoke(new Action(() => MessageBox.Show("발행테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));
                            }
                            Invoke(new Action(() => pnProgress.Visible = false));

                        });
                      
                    }
                    else if (_Model.TestTile == "알림톡 전송")
                    {
                       // int _TradeId = 0;
                        string _MobleNo = "01044402689";
                        pnProgress.Visible = true;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                int rIdx = 0;
                                var AuthKey = new Random().Next(1001, 9999).ToString();
                                string rClientCode = "100001";

                                Data.Connection(_Connection =>
                                {
                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = $"INSERT INTO  registerauth ( AuthKey, ClientCode, MobileNo) VALUES( @AuthKey, @ClientCode, @MobileNo) SELECT @@identity";
                                        _Command.Parameters.AddWithValue("@AuthKey", AuthKey);
                                        _Command.Parameters.AddWithValue("@ClientCode", "100001");
                                        _Command.Parameters.AddWithValue("@MobileNo", "010-4440-2689");
                                        //_Command.ExecuteNonQuery();

                                        Object O = _Command.ExecuteScalar();
                                        if (O != null)
                                            rIdx = Convert.ToInt32(O);
                                    }
                                });


                                try
                                {


                                    #region


                                    string Message = $"[배차정산관리 - 차세로]" +
                                         "\r\n" +
                                         $"테스트 님의 인증번호는 {AuthKey} 입니다.";

                                    var Table = new DataSets.BizMsgDataSet.BIZ_MSGDataTable();

                                    var Row = Table.NewBIZ_MSGRow();


                                    Row.MSG_TYPE = 6;
                                    Row.CMID = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + rClientCode;
                                    Row.REQUEST_TIME = DateTime.Now;
                                    Row.SEND_TIME = DateTime.Now;
                                    Row.DEST_PHONE = _MobleNo;
                                    Row.SEND_PHONE = "01044402689";
                                    Row.MSG_BODY = Message;
                                    Row.TEMPLATE_CODE = "bizp_2019071817115516111080047";
                                    Row.SENDER_KEY = "1f68131ed852323c07f08acccc94e5d88719df62";
                                    Row.NATION_CODE = "82";
                                    Row.STATUS = 0;
                                    Row.RE_TYPE = "SMS";
                                    Row.RE_BODY = "";
                                    Table.AddBIZ_MSGRow(Row);


                                    var Adapter = new DataSets.BizMsgDataSetTableAdapters.BIZ_MSGTableAdapter();
                                    Adapter.Update(Table);




                                    ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                                    {
                                        ClientId = 21,
                                        CDate = DateTime.Now,
                                        Amount = -10,
                                        MethodType = "알림톡테스트",
                                        Remark = $"에듀빌 (1131183754)",
                                    });
                                    ShareOrderDataSet.Instance.SaveChanges();


                                   // MessageBox.Show("인증번호 요청이 발송 되었습니다. 시간 안에 인증번호를 확인하시기 바랍니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.None);



                                    #endregion




                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =7 ";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 1);
                                        cmd.ExecuteNonQuery();





                                        cn.Close();

                                    }




                                    Invoke(new Action(() => MessageBox.Show("테스트를 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                                    Invoke(new Action(() => InitializeStorage()));
                                    Invoke(new Action(() => GetModels()));

                                }
                                catch
                                {
                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =7 ";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 0);
                                        cmd.ExecuteNonQuery();




                                        cn.Close();

                                    }
                                }

                            }

                            catch (Exception ex)
                            {
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =7 ";
                                    cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@TestResult", 0);
                                    cmd.ExecuteNonQuery();




                                    cn.Close();

                                }
                                Invoke(new Action(() => MessageBox.Show("테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                            }
                            Invoke(new Action(() => pnProgress.Visible = false));

                        });

                    }


                    else if (_Model.TestTile == "문자 전송")
                    {
                        string SVC_RT = "";
                        string SVC_MSG = "";
                        string MobileNo = "010-4440-2689";

                        pnProgress.Visible = true;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {

                                string sha512password = EncryptSHA512("edu1234!@");




                                string Parameter2 = "";
                                Parameter2 = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", "07086806908", sha512password, MobileNo.Replace("-", ""), $"문자전송테스트입니다.");




                                JObject response = null;

                                var uriBuilder = new UriBuilder("https://centrex.uplus.co.kr/RestApi/smssend");






                                uriBuilder.Query = Parameter2;
                                Uri finalUrl = uriBuilder.Uri;


                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);



                                request.Method = "POST";
                                request.ContentType = "text/json;";
                                request.ContentLength = 0;

                                request.Headers.Add("header-staff-api", "value");

                                var httpResponse = (HttpWebResponse)request.GetResponse();

                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    response = JObject.Parse(streamReader.ReadToEnd());

                                    SVC_RT = response["SVC_RT"].ToString();
                                    SVC_MSG = response["SVC_MSG"].ToString();

                                    if (SVC_RT == "0000")
                                    {
                                        SmsResult = "성공";
                                        
                                    }
                                    else
                                    {
                                        SmsResult = "실패";
                                        
                                        
                                    }

                                }

                                if(SmsResult =="성공")
                                {
                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =8 ";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 1);
                                        cmd.ExecuteNonQuery();




                                        cn.Close();

                                    }



                                    Invoke(new Action(() => MessageBox.Show("테스트를 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                                    Invoke(new Action(() => InitializeStorage()));
                                    Invoke(new Action(() => GetModels()));

                                }
                                else
                                {
                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =8 ";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 0);
                                        cmd.ExecuteNonQuery();




                                        cn.Close();

                                        Invoke(new Action(() => MessageBox.Show("테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                        Invoke(new Action(() => InitializeStorage()));
                                        Invoke(new Action(() => GetModels()));

                                    }
                                }

                            }

                            catch (Exception ex)
                            {
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =8 ";
                                    cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@TestResult", 0);
                                    cmd.ExecuteNonQuery();




                                    cn.Close();

                                }




                                Invoke(new Action(() => MessageBox.Show("테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));
                            }
                            Invoke(new Action(() => pnProgress.Visible = false));

                        });

                    }


                    else if (_Model.TestTile == "e-mail 전송")
                    {
                        int MailSuccess = 0;
                        int MailFail = 0;

                        pnProgress.Visible = true;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                try
                                {
                                    string fileString = "test.xlsx";

                                    var sFile = $"http://222.231.9.253:8080/Newchasero/StatsUpload/" + fileString;
                                    var sFileName = fileString;
                                    string sName, sEmail, sMobile, sNote1, sClientName, sgigan, scount, sAmount;
                                    sClientName = "에듀빌";
                                    sgigan = "- 기간 : " + DateTime.Now.ToString("yyyy-MM-dd") + "~" + DateTime.Now.ToString("yyyy-MM-dd");
                                    scount = "- 건수 : " + "1" + " 건";

                                    //var Query = salesDataSet.SalesReport.ToArray();
                                    //if (Query.Any())
                                    //{
                                    //    sName = Query.First().SangHo;

                                    //    sEmail = txt_Email.Text;
                                    //    sMobile = Query.First().PhoneNo;
                                    //    sNote1 = Query.First().RequestMemo;
                                    //}
                                    //else
                                    //{
                                    // var Q = salesDataSet.SalesManage.Where(c => c.SalesId == SalesId).ToArray();
                                    sName = "에듀빌";
                                    sEmail = "s4861404@naver.com";
                                    sMobile = "010-4440-2689";
                                    sNote1 = "";

                                    //}
                                    sAmount = "- 청구금액 : " + "1004" + " 원(VAT별도)";





                                    #region
                                    string Parameter = "";


                                    StringBuilder postParams = new StringBuilder();
                                    postParams.Append($"sName={sName}");
                                    postParams.Append($"&sEmail={sEmail}");
                                    postParams.Append($"&sMobile={sMobile}");
                                    postParams.Append($"&sFile_url={sFile}");
                                    postParams.Append($"&sFileName={sFileName}");
                                    postParams.Append($"&sClientName={sClientName}");
                                    postParams.Append($"&sgigan={sgigan}");
                                    postParams.Append($"&scount={scount}");
                                    postParams.Append($"&sAmount={sAmount}");



                                    JObject response = null;

                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://cardpay.kr/directsend_mail_change_word_asp_api.asp?" + postParams);



                                    request.Method = "GET";
                                    request.ContentType = "pplication/json;charset=utf-8;";
                                    request.ContentLength = 0;

                                    request.Headers.Add("header-staff-api", "value");

                                    var httpResponse = (HttpWebResponse)request.GetResponse();

                                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                    {
                                        response = JObject.Parse(streamReader.ReadToEnd());



                                    }
                                    #endregion


                                    if (response["status"].ToString() == "0")
                                    {

                                        MailSuccess++;
                                      
                                    }
                                    else
                                    {

                                        MailFail++;
                                    }

                                }
                                catch
                                {

                                    MailFail++;

                                }

                                if (MailSuccess > 0)
                                {
                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =9 ";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 1);
                                        cmd.ExecuteNonQuery();




                                        cn.Close();

                                    }



                                    Invoke(new Action(() => MessageBox.Show("테스트를 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                                    Invoke(new Action(() => InitializeStorage()));
                                    Invoke(new Action(() => GetModels()));
                                }
                                else
                                {
                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                               $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx =9 ";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@TestResult", 0);
                                        cmd.ExecuteNonQuery();




                                        cn.Close();

                                    }




                                    Invoke(new Action(() => MessageBox.Show("테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                    Invoke(new Action(() => InitializeStorage()));
                                    Invoke(new Action(() => GetModels()));

                                }

                            }

                            catch (Exception ex)
                            {
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                           $" UPDATE TestResult SET TestResult = @TestResult,TestYn='Y'  WHERE    Testidx = 9 ";
                                    cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@TestResult", 0);
                                    cmd.ExecuteNonQuery();




                                    cn.Close();

                                }




                                Invoke(new Action(() => MessageBox.Show("테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                Invoke(new Action(() => InitializeStorage()));
                                Invoke(new Action(() => GetModels()));
                            }
                            Invoke(new Action(() => pnProgress.Visible = false));

                        });

                    }


                }

            }
        }
    }
}
