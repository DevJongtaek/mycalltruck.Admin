using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.UI;
using Popbill.Taxinvoice;
using mycalltruck.Admin.DataSets;
using Popbill;
using System.Threading;
using mycalltruck.Admin.Class;

namespace mycalltruck.Admin
{
    public partial class FrmClientVaccountEtax : Form
    {
        DateTime _BeginDate = DateTime.Now;
        DateTime _EndDate = DateTime.Now;
        DESCrypt m_crypt = null;
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
        string _Sdate = "";
        string _Edate = "";
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;


        public PointsDataSet.ClientPointsListRow[] Cpoints { get; set; }


        public String _CarNo { get; set; }
        #region ACTION
        public FrmClientVaccountEtax(string Sdate, string Edate)
        {
            InitializeComponent();
            InitializeStorage();
            _Sdate = Sdate;
            _Edate = Edate;

            m_crypt = new DESCrypt("12345678");

            _InitCmb();
            cmb_Search.SelectedIndex = 0;


        }

        private void FrmTradeNew2_Load(object sender, EventArgs e)
        {
            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;


            ClientLoad();
            DriverRepository mDriverRepository = new DriverRepository();
            DataLoad();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            _Update();
        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        #endregion
        private void _InitCmb()
        {

        }

        #region UPDATE
        private void _Update()
        {
            SendInvoiceNice();

            //SendInvoice();
           
        }
        #endregion
        int Error = 0;
        int Success = 0;
        string Gubun = "";
        string _staxType = "과세";
        private Dictionary<int, String> TaxInvoiceErrorDic = new Dictionary<int, string>();
        private void SendInvoice()
        {
            var _Models = _ModelList.Where(c => c.Selected && !c.IsRefenece);
            if (_Models.Count() == 0)
            {
                MessageBox.Show("등록할 건을 먼저 선택하여 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            Success = 0;
            Error = 0;

            foreach (var _Model in _Models)
            {
                #region 전자세금계산서발행 
                clientsTableAdapter.Fill(clientDataSet.Clients);
                var Query2 = clientDataSet.Clients.FindByClientId(377);
                var Query = clientDataSet.Clients.FindByClientId(_Model.ClientId);
                


                string Tdtp_Date = DateTime.Now.ToString("yyyy-MM-dd");


                string _purposeType = "영수";


                TaxInvoiceErrorDic.Clear();






                string DutyNum = string.Empty;
                int i = 0;



                bool forceIssue = false;        // 지연발행 강제여부
                String memo = "";  // 즉시발행 메모 

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
                taxinvoice.invoicerMgtKey = "S" + _Model.ClientPointId.ToString() + "-" + DateTime.Now.ToString("yyMMddhhmmss");           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
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

                taxinvoice.supplyCostTotal = Convert.ToUInt64(_Model.Price).ToString();                 //필수 공급가액 합계
                taxinvoice.taxTotal = Convert.ToUInt64(_Model.VAT).ToString();                          //필수 세액 합계
                taxinvoice.totalAmount = Convert.ToUInt64(_Model.Amount).ToString();                     //필수 합계금액.  공급가액 + 세액

                taxinvoice.modifyCode = null;                           //수정세금계산서 작성시 1~6까지 선택기재.
                taxinvoice.originalTaxinvoiceKey = "";                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
                taxinvoice.serialNum = "1";
                taxinvoice.cash = "";                                   //현금
                taxinvoice.chkBill = "";                                //수표
                taxinvoice.note = "";                                   //어음
                taxinvoice.credit = "";                                 //외상미수금
                taxinvoice.remark1 = "";
                //LocalUser.Instance.LogInInformation.LoadClient();
                //var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
                //if (_HideAccountNo == 0)
                //{
                //    taxinvoice.remark1 = Query.CMSBankName + "/" + Query.CMSOwner + "/" + Query.CMSAccountNo;
                //}
                //else
                //{
                //    taxinvoice.remark1 = Query.CMSBankName + "/" + Query.CMSOwner;

                //}
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
                detail.unitCost = Convert.ToUInt64(_Model.Price).ToString();                     //단가
                detail.supplyCost = Convert.ToUInt64(_Model.Price).ToString();                          //공급가액
                detail.tax = Convert.ToUInt64(_Model.VAT).ToString();                                   //세액
                detail.remark = "";



                taxinvoice.detailList.Add(detail);




                detail = new TaxinvoiceDetail();

                try
                {
                    Response response = taxinvoiceService.RegistIssue(Query2.BizNo.Replace("-", ""), taxinvoice, forceIssue, "");

                    try
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                               "UPDATE ClientPoints SET EtaxWriteDate = @EtaxWriteDate, Issue = 1  , IssueState = '발행', invoicerMgtKey = @invoicerMgtKey,EtaxCanCelYN='N' WHERE    ClientPointId =" + _Model.ClientPointId.ToString() + " ";
                            cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);

                            cmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);


                            cmd.ExecuteNonQuery();




                            cn.Close();
                        }
                        Success++;
                        //ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                        //{
                        //    ClientId = 21,
                        //    CDate = DateTime.Now,
                        //    Amount = -150,
                        //    MethodType = "전자 세금계산서",
                        //    Remark = $"{taxinvoice.invoiceeCorpName} ({taxinvoice.invoiceeCorpNum})",
                        //});
                        //ShareOrderDataSet.Instance.SaveChanges();
                    }
                    catch (PopbillException ex)
                    {
                        TaxInvoiceErrorDic.Add(_Model.ClientPointId, "[ " + ex.code.ToString() + " ] " + ex.Message);

                        Error++;
                    }
                }
                catch (PopbillException ex)
                {
                    TaxInvoiceErrorDic.Add(_Model.ClientPointId, "[ " + ex.code.ToString() + " ] " + ex.Message);

                    Error++;
                }


                #endregion
            }


            MessageBox.Show($"발행성공 건 : {Success} 건\r\n발행실패 건 : {Error} 건\r\n\r\n 전자세금계산서 발행이 완료 되었습니다.\r\n", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }
        private string _purposeType = "01";

        private void SendInvoiceNice()
        {

            var _Models = _ModelList.Where(c => c.Selected && !c.IsRefenece);
            if (_Models.Count() == 0)
            {
                MessageBox.Show("등록할 건을 먼저 선택하여 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            Success = 0;
            Error = 0;

            #region 전자세금계산서발행 
            clientsTableAdapter.Fill(clientDataSet.Clients);
            //var Query2 = clientDataSet.Clients.FindByClientId(377);
            //var Query = clientDataSet.Clients.FindByClientId(_Model.ClientId);


            var Query = clientDataSet.Clients.FindByClientId(377);
           
            //   var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";


            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.NPKIpasswd }).ToString();
            string Tdtp_Date = dtpEtaxDate.Text;


            _purposeType = "01";
          

            TaxInvoiceErrorDic.Clear();

            bar.Value = 0;
            bar.Maximum = _Models.Count();
            bar.Visible = true;
            pnProgress.Visible = true;


            Thread t = new Thread(new ThreadStart(() =>
            {

                string DutyNum = string.Empty;
                string DescriptionText = "";
                int i = 0;

                Success = 0;
                Error = 0;
                //var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
                //if (_HideAccountNo == 0)
                //{
                //    DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner + "/" + _Clients.CMSAccountNo;
                //}
                //else
                //{
                //    DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner;

                //}
                foreach (var _Model in _Models)
                {
                    var _Clients = clientDataSet.Clients.FindByClientId(_Model.ClientId);
                    decimal _Price = 0;
                    decimal _Vat = 0;
                    decimal _Amont = 0;
                    string _TypeCode = "0101";
                    _Price = _Model.Price;
                    _Vat = _Model.VAT;
                    
                    _Amont = _Price + _Vat;
                    string dtiXml = "" +
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                    "<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                    " xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
                    "   <TaxInvoiceDocument>\r\n" +
                    $"       <TypeCode>{_TypeCode}</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
                    $"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
                    $"      <IssueDateTime>{Tdtp_Date.Replace("/", "")}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
                    $"      <AmendmentStatusCode></AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
                    $"      <PurposeCode>{_purposeType}</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
                    $"      <OriginalIssueID></OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
                    $"  </TaxInvoiceDocument>\r\n" +
                    $"  <TaxInvoiceTradeSettlement>\r\n" +
                    $"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
                    $"          <ID>{Query.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
                    $"          <TypeCode>{Query.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
                    $"          <NameText>{Query.Name}</NameText>	<!-- 상호명 -->\r\n" +
                    $"          <ClassificationCode>{Query.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
                    $"          <SpecifiedOrganization>\r\n" +
                    $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
                    $"          </SpecifiedOrganization>\r\n" +
                    $"          <SpecifiedPerson>\r\n" +
                    $"              <NameText>{Query.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
                    $"          </SpecifiedPerson>\r\n" +
                    $"          <DefinedContact>\r\n" +
                    $"              <PersonNameText>{Query.CEO}</PersonNameText> <!-- 담당자명 -->\r\n" +
                    $"              <TelephoneCommunication>{Query.MobileNo}</TelephoneCommunication> <!-- 전화번호 -->\r\n" +
                    $"              <URICommunication>{Query.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
                    $"          </DefinedContact>\r\n" +
                    $"          <SpecifiedAddress>\r\n" +
                    $"              <LineOneText>{Query.AddressState + " " + Query.AddressCity + " " + Query.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
                    $"          </SpecifiedAddress>\r\n" +
                    $"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
                    $"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
                    $"          <ID>{_Clients.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
                    $"          <TypeCode>{_Clients.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
                    $"          <NameText>{_Clients.Name}</NameText>	<!-- 상호명 -->\r\n" +
                    $"          <ClassificationCode>{_Clients.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
                    $"          <SpecifiedOrganization>\r\n" +
                    $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
                    $"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
                    $"          </SpecifiedOrganization>\r\n" +
                    $"          <SpecifiedPerson>\r\n" +
                    $"              <NameText>{_Clients.CEO}</NameText>	<!--  대표자명 -->\r\n" +
                    $"          </SpecifiedPerson>\r\n" +
                    $"          <PrimaryDefinedContact>\r\n" +
                    $"              <PersonNameText>{_Clients.CEO}</PersonNameText>	<!-- 담당자명 -->\r\n" +
                    $"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
                    $"              <TelephoneCommunication>{_Clients.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
                    $"              <URICommunication>{_Clients.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
                    $"          </PrimaryDefinedContact>\r\n" +
                    $"          <SpecifiedAddress>\r\n" +
                    $"              <LineOneText>{_Clients.AddressState + " " + _Clients.AddressCity + " " + _Clients.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
                    $"          </SpecifiedAddress>\r\n" +
                    $"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +
                    
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
                          $"      <NameText>주선사 충전금</NameText>  <!-- 품목명 -->\r\n" +
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











                    var Result = "";



                    Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, Query.frnNo, Query.userid, Query.passwd, certPw, dtiXml, "Y", "N", "", "3");
                    
                  

                    try
                    {
                        var ResultList = Result.Split('/');

                        var retVal = ResultList[0];
                        var errMsg = ResultList[1];
                        var billNo = ResultList[2];
                        var gnlPoint = ResultList[3];
                        var outbnsPoint = ResultList[4];
                        var totPoint = ResultList[5];
                        //return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

                        if (retVal == "0")
                        {
                            //승인번호 DB반영

                            try
                            {


                                try
                                {
                                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = cn.CreateCommand();
                                        cmd.CommandText =
                                           "UPDATE ClientPoints SET EtaxWriteDate = @EtaxWriteDate, Issue = 1  , IssueState = '발행', invoicerMgtKey = @invoicerMgtKey,EtaxCanCelYN='N' WHERE    ClientPointId =" + _Model.ClientPointId.ToString() + " ";
                                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);

                                        cmd.Parameters.AddWithValue("@invoicerMgtKey", billNo);


                                        cmd.ExecuteNonQuery();

                                        cn.Close();
                                    }
                                    Success++;
                            
                                }
                                catch (Exception ex)
                                {
                                    TaxInvoiceErrorDic.Add(_Model.ClientPointId,  ex.Message);
                                    //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                                    Error++;
                                }
                            }
                            catch (Exception ex)
                            {
                                TaxInvoiceErrorDic.Add(_Model.ClientPointId, ex.Message);
                                //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                                Error++;
                            }
                        }
                        else
                        {
                            TaxInvoiceErrorDic.Add(_Model.ClientPointId, "[ " + retVal + " ] " + errMsg);
                            //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                            Error++;
                        }

                    }
                    catch
                    {

                    }
                }


                pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

              


            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();


            #endregion

            MessageBox.Show($"전자세금계산서 발행이 완료 되었습니다.\r\n", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }

        #region STORAGE
        class Model : INotifyPropertyChanged
        {
            public int ClientPointId { get; set; }
            public DateTime CDate { get; set; }
            public DateTime EtaxWriteDate { get; set; }
            public string BizNo { get; set; }
            public string Name { get; set; }
            public string Ceo { get; set; }
            public int Count { get; set; }
            public decimal Price { get { return _Price; } set { SetField(ref _Price, value); } }
            public decimal VAT { get { return _VAT; } set { SetField(ref _VAT, value); } }
            public decimal Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }

            public int ClientId { get; set; }



            private decimal _Price = 0;

            private decimal _VAT = 0;

            private decimal _Amount = 0;
            private bool _Selected = false;
            public bool Selected
            {
                get { return _Selected; }
                set
                {
                    SetField(ref _Selected, value);
                }
            }

            public List<int> ClietPointIdList { get; set; } = new List<int>();
            public bool IsRefenece { get; set; }
            public int PointMethod { get; set; }
            public int CustomerId { get; set; }
            public int PayLocation { get; set; }

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
        private BindingList<Model> _ModelList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _ModelList;
        }
        private void DataLoad()
        {
            var _clientpointid = Cpoints.Select(c => c.ClientPointId).ToArray();
            AllSelect.Checked = false;
            _ModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    List<String> WhereStringList = new List<string>();


                    _Command.CommandText = "SELECT CP.ClientPointId,CP.CDate, C.BizNo,C.Name,C.CEO, Convert(decimal, CP.Amount / 1.1) as Price,Convert(decimal, CP.Amount - CP.Amount / 1.1) as Vat,CP.Amount,C.ClientId  FROM     ClientPoints AS CP INNER JOIN Clients AS C ON CP.ClientId = C.ClientId WHERE CP.Remark = '가상계좌 입금' AND ISNULL(Issue,0) != 1 AND  CONVERT(VARCHAR(10),CP.CDate,111) >= @Sdate AND CONVERT(VARCHAR(10),CP.CDate,111) <= @Edate";


                    _Command.Parameters.AddWithValue("@Sdate", _Sdate);
                    _Command.Parameters.AddWithValue("@Edate", _Edate);





                    //  _Command.Parameters.AddWithValue("@ClientPointId", _Edate);




                    if (cmb_Search.Text == "사업자번호")
                    {
                        _Command.CommandText += Environment.NewLine + " AND Replace(C.BizNo,'-','') LIKE '%'+ @BizNo + '%' ";
                        _Command.Parameters.AddWithValue("@BizNo", txt_Search.Text.Replace("-", ""));
                    }
                    else if (cmb_Search.Text == "상호")
                    {
                        _Command.CommandText += Environment.NewLine + " AND C.Name LIKE '%'+ @Name + '%' ";
                        _Command.Parameters.AddWithValue("@Name", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "대표자")
                    {
                        _Command.CommandText += Environment.NewLine + " AND C.CEO LIKE '%'+ @CEO + '%' ";
                        _Command.Parameters.AddWithValue("@CEO", txt_Search.Text);
                    }

                    string filter = String.Format(" AND ClientPointId IN ('{0}'", _clientpointid[0]);
                    for (int i = 1; i < _clientpointid.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", _clientpointid[i]);
                    }
                    filter += ")";
                    //FilterString = filter
                    _Command.CommandText += Environment.NewLine + filter;

                    _Command.CommandText += Environment.NewLine + "  Order By CP.ClientPointId Desc";







                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var _ClientPointId = _Reader.GetInt32Z(0);
                            var _ClientId = _Reader.GetInt32Z(8);
                            var _Amount = _Reader.GetDecimal(7);

                            Model _Model = null;


                            _Model = new Model()
                            {
                                ClientPointId = _ClientPointId,
                                CDate = _Reader.GetDateTime(1),
                                BizNo = _Reader.GetString(2),
                                Name = _Reader.GetString(3),
                                Ceo = _Reader.GetString(4),
                                ClientId = _Reader.GetInt32Z(8),

                            };
                            _ModelList.Add(_Model);

                            _Model.Count++;
                            _Model.Amount += _Amount;

                            _Model.ClietPointIdList.Add(_ClientPointId);
                        }
                    }
                }
                _Connection.Close();
            }
            foreach (var _Model in _ModelList)
            {

                _Model.Amount = _Model.Amount;
                _Model.Price = (_Model.Amount / 1.1m);
                _Model.VAT = _Model.Amount - _Model.Price;


            }
            _ModelList.Add(new Model
            {
                ClientPointId = 0,
                IsRefenece = true,
            });


            //_BeginDate = dtp_From.Value;
            //_EndDate = dtp_To.Value;
        }
        #endregion

     
    
        private void ClientLoad()
        {
            LocalUser.Instance.LogInInformation.LoadClient();
          
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnNumber.Index)
            {
               
                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    e.Value = "합계";
                   
                    
                }
                else
                {
                    e.Value = e.RowIndex + 1;
                }
            }
          
            else if (e.ColumnIndex == ColumnSelect.Index)
            {
                var Selected = _ModelList[e.RowIndex];

              

                bool _UptaeB = false;
                bool _UpjongB = false;
                bool _AddressB = false;
                bool _BizNo = false;

               






                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    ((DataGridViewDisableCheckBoxCell)newDGV1[e.ColumnIndex, e.RowIndex]).CheckBoxVisible = false;
                }
            }
           

            if (e.RowIndex == newDGV1.RowCount - 1)
            {
                newDGV1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black;
            }

        }

        private void newDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _ModelList.Count - 1)
                return;
            if (e.ColumnIndex == ColumnSelect.Index)
            {
              
                _ModelList[e.RowIndex].Selected = !_ModelList[e.RowIndex].Selected;
                SetReferenceModel();
            }
        }

        private void AllSelect_CheckedChanged(object sender, EventArgs e)
        {
           

            foreach (var _Model in _ModelList)
            {
                _Model.Selected = AllSelect.Checked;
            }
            SetReferenceModel();
        }

        private void SetReferenceModel()
        {
            var _Models = _ModelList.Where(c => c.Selected && !c.IsRefenece );
            var _RModel = _ModelList.FirstOrDefault(c => c.IsRefenece);
            if (_RModel != null)
            {
                _RModel.Price = _Models.Sum(c => c.Price);
                _RModel.VAT = _Models.Sum(c => c.VAT);
                _RModel.Amount = _Models.Sum(c => c.Amount);
            }
        }

        private void newDGV1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdb_Tax0.Checked)
            //{
            //    AllowTaxBool = false;
            //}
            //else
            //{
            //    AllowTaxBool = true;
            //}
            DataLoad();
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void newDGV1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var _Row = _ModelList[e.RowIndex];

         

            bool _UptaeB = false;
            bool _UpjongB = false;
            bool _AddressB = false;
            bool _BizNo = false;

          
        }

        private void newDGV1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var _Row = _ModelList[e.RowIndex];

            if (e.RowIndex < 0)
                return;
       

            
        }
       

        private void rdoSplit_CheckedChanged(object sender, EventArgs e)
        {
            DataLoad();
        }

       
    }
}
