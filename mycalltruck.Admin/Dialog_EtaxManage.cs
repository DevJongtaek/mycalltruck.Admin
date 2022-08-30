using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using Popbill;
using Popbill.Taxinvoice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class Dialog_EtaxManage : Form
    {
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;

        string memo = "";
        int Error = 0;
        int Success = 0;
        string Gubun = "";
        string _staxType = "과세";
        public Dialog_EtaxManage()
        {
            InitializeComponent();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);

            salesManageDetailTableAdapter.Fill(this.salesDataSet.SalesManageDetail);


            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
            SetCustomerList();

        }
        public Dialog_EtaxManage(string _rdopurposeType1,string _taxType,string _Gubun)
        {
            InitializeComponent();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);

            salesManageDetailTableAdapter.Fill(this.salesDataSet.SalesManageDetail);


            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
            SetCustomerList();

            if(_rdopurposeType1 == "청구")
            {
                rdopurposeType1.Checked = true;
            }
            else
            {
                rdopurposeType2.Checked = true;
            }
            _staxType = _taxType;
            Gubun = _Gubun;


        }
        public SalesDataSet.SalesManageRow[] Sales { get; set; }
        private SalesDataSet.SalesManageRow[] AcceptSales { get; set; }

        private int CurrentStep = 1;
        private bool IsLoadClientAccs = false;
        private bool IsLoadStep2 = false;
        private bool IsLoadStep3 = false;
        private bool IsLoadStep4 = false;
        private bool NeedCheckCustomerAccLimite = false;
        private int Step5Tatal = 0;
        private int Step5Done = 0;

        private bool IsClosed = false;
        //Parameter
        public String AuthKey { get; set; }
        private int ClientAccId = 0;
        private string CardNo = "";
        private string CardDate = "";
        private string CardPan = "";
        private string UserType = "";
        private string BizNo = "";
        private int PGGubun = 0;
        private string _purposeType = "청구";

        private void btn_ShowDenyList_Click(object sender, EventArgs e)
        {
            //Step2DenyList.Visible = true;
        }

        class CustomerModel
        {
            public int CustomerId { get; set; }
            public string SangHo { get; set; }
            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }
            public int PointMethod { get; set; }
            public int Fee { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }
            public string BizNo { get; set; }
            public string CustomerManager { get; set; }
            public string ChargeName { get; set; }
            public string Email { get; set; }
            public string CEO { get; set; }
            public int BizGubun { get; set; }
        }
        List<CustomerModel> CustomerModelList = new List<CustomerModel>();
        private void SetCustomerList()
        {
            CustomerModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo, Customers.PointMethod, Customers.Fee, Customers.AddressState, Customers.AddressCity, Customers.AddressDetail, Customers.BizNo,Customers.MobileNo,ISNULL(CustomerManager.ManagerName,''),BizGubun,Customers.ChargeName,Email,CEO FROM Customers " +
                        $" LEFT  JOIN CustomerManager ON Customers.CustomerManagerId = CustomerManager.ManagerId" +

                        $" WHERE Customers.BizNo <> '000-00-00000' AND Customers.ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            CustomerModelList.Add(new CustomerModel
                            {
                                CustomerId = _Reader.GetInt32(0),
                                SangHo = _Reader.GetStringN(1),
                                PhoneNo = _Reader.GetStringN(2),
                                PointMethod = _Reader.GetInt32Z(3),
                                Fee = _Reader.GetInt32Z(4),
                                AddressState = _Reader.GetStringN(5),
                                AddressCity = _Reader.GetStringN(6),
                                AddressDetail = _Reader.GetStringN(7),
                                BizNo = _Reader.GetStringN(8),
                                MobileNo = _Reader.GetStringN(9),
                                CustomerManager = _Reader.GetString(10),
                                BizGubun = _Reader.GetInt32(11),
                                ChargeName = _Reader.GetString(12),
                                Email = _Reader.GetString(13),
                                CEO = _Reader.GetString(14)
                            });
                        }
                    }
                }
                _Connection.Close();
            }
        }
        private void ShowStep3()
        {
            List<SalesDataSet.SalesManageRow> Denyed = new List<SalesDataSet.SalesManageRow>();
            List<SalesDataSet.SalesManageRow> Accepted = new List<SalesDataSet.SalesManageRow>();
            foreach (var sale in Sales)
            {
               


                if (sale.Amount == 0)
                {
                    Denyed.Add(sale);
                    continue;
                }
            


                Accepted.Add(sale);
            }

            AcceptSales = Accepted.ToArray();



            CurrentStep = 3;


            pnStep3.Visible = true;
            pnStep4.Visible = false;
            pnStep5.Visible = false;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[3].Height = 100F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[4].Height = 0F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[5].Height = 0F;

            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Bold);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Regular);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Regular);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step3.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step4.ForeColor = Color.FromArgb(137, 206, 250);
            lbl_Step5.ForeColor = Color.FromArgb(137, 206, 250);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step3.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step4.BackColor = Color.FromArgb(19, 124, 192);
            bar_Step5.BackColor = Color.FromArgb(19, 124, 192);
            btn_Pre.Enabled = true;
            btn_Close.Enabled = true;
            btn_Next.Enabled = true;
            btn_ShowDenyList.Visible = false;
            if (AcceptSales.Count() > 0)
            {

                if (!IsLoadStep3)
                {
                    BindingList<SalesDataSet.SalesManageRow> AcceptedItemBindingList = new BindingList<SalesDataSet.SalesManageRow>(AcceptSales);
                    Step3List.AutoGenerateColumns = false;
                    Step3List.DataSource = AcceptedItemBindingList;
                    Step3List.CellContentClick += (sender, e) =>
                    {
                        Step3List.EndEdit();
                        if (Step3List.Rows.Cast<DataGridViewRow>().Any(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true))
                        {
                            btn_Next.Enabled = true;


                        }
                        else
                        {
                            btn_Next.Enabled = false;
                        }

                        AcceptSales = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as SalesDataSet.SalesManageRow).ToArray();


                        lbl_TotalMoney.Text = " ■ 총 이체금액 : " + AcceptSales.Sum(c => c.Amount).ToString("N0") + " 원";
                        lblEtaxCnt.Text = AcceptSales.Count().ToString("N0") + " 건";


                    };
                    Step3AllSelect.CheckedChanged += (sender, e) =>
                    {
                        List<string> Totalmoney = new List<string>();
                        if (Step3AllSelect.Checked)
                        {
                            for (int i = 0; i < Step3List.RowCount; i++)
                            {
                                Step3List[Step3Checkbox.Index, i].Value = true;

                            }

                            AcceptSales = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as SalesDataSet.SalesManageRow).ToArray();


                            lbl_TotalMoney.Text = " ■ 총 결제금액 : " + AcceptSales.Sum(c => c.Amount).ToString("N0") + " 원";
                            lblEtaxCnt.Text = AcceptSales.Count().ToString("N0") + " 건";

                            btn_Next.Enabled = true;

                        }
                        else
                        {
                            for (int i = 0; i < Step3List.RowCount; i++)
                            {
                                Step3List[Step3Checkbox.Index, i].Value = false;

                            }
                            lblEtaxCnt.Text = AcceptSales.Count().ToString("N0") + " 건";
                            lbl_TotalMoney.Text = " ■ 총 결제금액 : " + "0";

                            btn_Next.Enabled = false;

                        }
                    };
                    Step3AllSelect.Checked = true;
                    IsLoadStep3 = true;
                }
            }
            else
            {
                btn_Next.Enabled = false;

            }
        }


        private void ShowStep4()
        {
            CurrentStep = 4;


            pnStep3.Visible = false;
            pnStep4.Visible = true;
            pnStep5.Visible = false;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[4].Height = 100F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[5].Height = 0F;

            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Regular);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Bold);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Regular);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step3.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step4.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step5.ForeColor = Color.FromArgb(137, 206, 250);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step3.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step4.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step5.BackColor = Color.FromArgb(19, 124, 192);
            btn_Pre.Enabled = true;
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            btn_ShowDenyList.Visible = false;




            if (!IsLoadStep4)
            {



                IsLoadStep4 = true;




                dtpEtaxDate.Value = DateTime.Now;
                txtItem.Text = "화물운송료";
               


            }

            btn_Next.Enabled = true;





        }

        private void Step4_TextChanged(object sender, EventArgs e)
        {

        }

        private void ShowStep5()
        {
            CurrentStep = 5;


            pnStep3.Visible = false;
            pnStep4.Visible = false;
            pnStep5.Visible = true;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[4].Height = 0F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[5].Height = 100F;

            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Regular);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Regular);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Bold);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step3.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step4.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step5.ForeColor = Color.FromKnownColor(KnownColor.White);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step3.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step4.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step5.BackColor = Color.FromKnownColor(KnownColor.White);
            btn_Pre.Enabled = false;
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            btn_ShowDenyList.Visible = false;
            AcceptSales = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as SalesDataSet.SalesManageRow).ToArray();
            Step5Tatal = AcceptSales.Length;
        

            Step5Display(true);

            Step5Work();
        }
      

        private void Step5Work()
        {
            if (IsClosed)
                return;

            try
            {

                Step5Etax();




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private Dictionary<int, String> TaxInvoiceErrorDic = new Dictionary<int, string>();
        private void Step5Etax()
        {
            #region 전자세금계산서발행 

            var Query = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
          
          
            string Tdtp_Date = dtpEtaxDate.Text;

            if (rdopurposeType1.Checked)
            {
                _purposeType = "청구";
            }
            else
            {
                _purposeType = "영수";
            }

            TaxInvoiceErrorDic.Clear();

            bar.Value = 0;
            bar.Maximum = AcceptSales.Count();
            bar.Visible = true;
            pnProgress.Visible = true;


            Thread t = new Thread(new ThreadStart(() =>
            {

                string DutyNum = string.Empty;
                int i = 0;

                Success = 0;
                Error = 0;

                foreach (var item in AcceptSales)
                {


                    bool forceIssue = false;        // 지연발행 강제여부
                    String memo = "";  // 즉시발행 메모 

                    // 세금계산서 정보 객체 
                    Taxinvoice taxinvoice = new Taxinvoice();


                    taxinvoice.writeDate = Tdtp_Date.Replace("/", "");                      //필수, 기재상 작성일자
                    taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
                    taxinvoice.issueType = "정발행";                        //필수, {정발행, 역발행, 위수탁}
                    taxinvoice.purposeType = _purposeType;                        //필수, {영수, 청구}
                    taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
                    taxinvoice.taxType = _staxType;                            //필수, {과세, 영세, 면세}

                    taxinvoice.invoicerCorpNum = Query.BizNo.Replace("-", "");              //공급자 사업자번호
                    taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
                    taxinvoice.invoicerCorpName = Query.Name;
                    taxinvoice.invoicerMgtKey = "S" + item["SalesId"].ToString() + "-" + DateTime.Now.ToString("yyMMddhhmmss");           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
                    taxinvoice.invoicerCEOName = Query.CEO;
                    taxinvoice.invoicerAddr = Query.AddressState + " " + Query.AddressCity + " " + Query.AddressDetail;
                    taxinvoice.invoicerBizClass = Query.Upjong;
                    taxinvoice.invoicerBizType = Query.Uptae;
                    taxinvoice.invoicerContactName = Query.CEO;
                    taxinvoice.invoicerEmail = Query.Email;
                    taxinvoice.invoicerTEL = Query.PhoneNo;
                    taxinvoice.invoicerHP = Query.MobileNo;
                    taxinvoice.invoicerSMSSendYN = false;                    //정발행시(공급자->공급받는자) 문자발송기능 사용시 활용

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    taxinvoice.invoiceeType = "사업자";                     //공급받는자 구분, {사업자, 개인, 외국인}
                    taxinvoice.invoiceeCorpNum = item["BizNo"].ToString().Replace("-", "");              //공급받는자 사업자번호
                    taxinvoice.invoiceeCorpName = item["SangHo"].ToString();
                    taxinvoice.invoiceeMgtKey = "";                         //역발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
                    taxinvoice.invoiceeCEOName = item["Ceo"].ToString();
                    taxinvoice.invoiceeAddr = item["AddressState"].ToString() + " " + item["AddressCity"].ToString() + " " + item["AddressDetail"].ToString();
                    taxinvoice.invoiceeBizClass = item["Upjong"].ToString();
                    taxinvoice.invoiceeBizType = item["Uptae"].ToString();
                    
                   
                    
                    var _Customer = CustomerModelList.Where(c => c.CustomerId == item.CustomerId).ToArray();
                    if (_Customer.Any())
                    {
                        if (String.IsNullOrEmpty(_Customer.First().ChargeName))
                        {
                            taxinvoice.invoiceeContactName1 = _Customer.First().CEO;
                            taxinvoice.invoiceeEmail1 = _Customer.First().Email;
                            taxinvoice.invoiceeHP1 = _Customer.First().MobileNo;
                            taxinvoice.invoiceeTEL1 = _Customer.First().MobileNo;
                        }
                        else
                        {
                            taxinvoice.invoiceeContactName1 = _Customer.First().ChargeName;
                            taxinvoice.invoiceeEmail1 = _Customer.First().Email;
                            taxinvoice.invoiceeHP1 = _Customer.First().MobileNo;
                            taxinvoice.invoiceeTEL1 = _Customer.First().MobileNo;
                        }


                    }
                    else
                    {
                        taxinvoice.invoiceeContactName1 = item.ContRactName;
                        taxinvoice.invoiceeEmail1 = item["Email"].ToString();
                        taxinvoice.invoiceeHP1 = item["MobileNo"].ToString();
                        taxinvoice.invoiceeTEL1 = item["MobileNo"].ToString();
                    }
                  
                    taxinvoice.invoiceeSMSSendYN = false;                   //역발행시(공급받는자->공급자) 문자발송기능 사용시 활용

                    taxinvoice.supplyCostTotal = Convert.ToUInt64(item.Price).ToString();                  //필수 공급가액 합계"
                    taxinvoice.taxTotal = Convert.ToUInt64(item.Vat).ToString();                          //필수 세액 합계
                    taxinvoice.totalAmount = Convert.ToUInt64(item.Amount).ToString();                     //필수 합계금액.  공급가액 + 세액

                    taxinvoice.modifyCode = null;                           //수정세금계산서 작성시 1~6까지 선택기재.
                    taxinvoice.originalTaxinvoiceKey = "";                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
                    taxinvoice.serialNum = "1";
                    taxinvoice.cash = "";                                   //현금
                    taxinvoice.chkBill = "";                                //수표
                    taxinvoice.note = "";                                   //어음
                    taxinvoice.credit = "";                                 //외상미수금
                                                                            //taxinvoice.remark1 = "";
                    LocalUser.Instance.LogInInformation.LoadClient();
                   var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
                    if (_HideAccountNo == 0)
                    {
                        taxinvoice.remark1 = Query.CMSBankName + "/" + Query.CMSOwner + "/" + Query.CMSAccountNo;
                    }
                    else
                    {
                        taxinvoice.remark1 = Query.CMSBankName + "/" + Query.CMSOwner;

                    }
                    taxinvoice.remark2 = "";
                    taxinvoice.remark3 = "";
                    taxinvoice.kwon = 1;
                    taxinvoice.ho = 1;

                    taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
                    taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

                    taxinvoice.detailList = new List<TaxinvoiceDetail>();

                    TaxinvoiceDetail detail = new TaxinvoiceDetail();

                    //
                    var DetailQuery1 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName1)&& c.SalesId == item.SalesId).ToArray();
                    if (DetailQuery1.Any())
                    {
                        detail.serialNum = 1;                                   //일련번호
                                                                                // detail.purchaseDT = item["CREATE_DATE1"].ToString().Replace("/", "");                         //거래일자
                        detail.purchaseDT = Tdtp_Date.Replace("/", "");                         //거래일자
                        detail.itemName = DetailQuery1.First().itemName1;
                        detail.spec = "";
                        detail.qty = DetailQuery1.First().qty1;                               //수량
                        detail.unitCost = Convert.ToUInt64(DetailQuery1.First().unitCost1).ToString();                     //단가
                        detail.supplyCost = Convert.ToUInt64(DetailQuery1.First().supplyCost1).ToString();                          //공급가액
                        detail.tax = Convert.ToUInt64(DetailQuery1.First().tax1).ToString();                                   //세액
                        detail.remark = txtRemark2.Text ;
                       
                    }
                    else
                    {
                        detail.serialNum = 1;                                   //일련번호
                                                                                // detail.purchaseDT = item["CREATE_DATE1"].ToString().Replace("/", "");                         //거래일자
                        detail.purchaseDT = Tdtp_Date.Replace("/", "");                         //거래일자
                        detail.itemName = txtItem.Text;
                        detail.spec = "";
                        detail.qty = "1";                                       //수량
                        detail.unitCost = Convert.ToUInt64(item.Price).ToString();                     //단가
                        detail.supplyCost = Convert.ToUInt64(item.Price).ToString();                          //공급가액
                        detail.tax = Convert.ToUInt64(item.Vat).ToString();                                   //세액
                        detail.remark = txtRemark2.Text;
                    }
                    

                    taxinvoice.detailList.Add(detail);

                    var DetailQuery2 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName2) && c.SalesId == item.SalesId).ToArray();

                    if(DetailQuery2.Any())
                    {
                        detail = new TaxinvoiceDetail();

                        detail.serialNum = 2;                                   //일련번호
                                                                                // detail.purchaseDT = item["CREATE_DATE1"].ToString().Replace("/", "");                         //거래일자
                        detail.purchaseDT = Tdtp_Date.Replace("/", "");                         //거래일자
                        detail.itemName = DetailQuery2.First().itemName2;
                        detail.spec = "";
                        detail.qty = DetailQuery2.First().qty2;                                    //수량
                        detail.unitCost = Convert.ToUInt64(DetailQuery2.First().unitCost2).ToString();                     //단가
                        detail.supplyCost = Convert.ToUInt64(DetailQuery2.First().supplyCost2).ToString();                          //공급가액
                        detail.tax = Convert.ToUInt64(DetailQuery2.First().tax2).ToString();

                        taxinvoice.detailList.Add(detail);

                    }


                    var DetailQuery3 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName3) && c.SalesId == item.SalesId).ToArray();

                    if (DetailQuery3.Any())
                    {
                        detail = new TaxinvoiceDetail();

                        detail.serialNum = 3;                                   //일련번호
                                                                                // detail.purchaseDT = item["CREATE_DATE1"].ToString().Replace("/", "");                         //거래일자
                        detail.purchaseDT = Tdtp_Date.Replace("/", "");                         //거래일자
                        detail.itemName = DetailQuery3.First().itemName3;
                        detail.spec = "";
                        detail.qty = DetailQuery3.First().qty3;                                    //수량
                        detail.unitCost = Convert.ToUInt64(DetailQuery3.First().unitCost3).ToString();                     //단가
                        detail.supplyCost = Convert.ToUInt64(DetailQuery3.First().supplyCost3).ToString();                          //공급가액
                        detail.tax = Convert.ToUInt64(DetailQuery3.First().tax3).ToString();

                        taxinvoice.detailList.Add(detail);

                    }

                    var DetailQuery4 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName4) && c.SalesId == item.SalesId).ToArray();

                    if (DetailQuery4.Any())
                    {
                        detail = new TaxinvoiceDetail();

                        detail.serialNum = 4;                                   //일련번호
                                                                                // detail.purchaseDT = item["CREATE_DATE1"].ToString().Replace("/", "");                         //거래일자
                        detail.purchaseDT = Tdtp_Date.Replace("/", "");                         //거래일자
                        detail.itemName = DetailQuery4.First().itemName4;
                        detail.spec = "";
                        detail.qty = DetailQuery4.First().qty4;                                    //수량
                        detail.unitCost = Convert.ToUInt64(DetailQuery4.First().unitCost4).ToString();                     //단가
                        detail.supplyCost = Convert.ToUInt64(DetailQuery4.First().supplyCost4).ToString();                          //공급가액
                        detail.tax = Convert.ToUInt64(DetailQuery4.First().tax4).ToString();

                        taxinvoice.detailList.Add(detail);

                    }


                    detail = new TaxinvoiceDetail();

                    try
                    {
                        Response response = taxinvoiceService.RegistIssue(Query.BizNo.Replace("-", ""), taxinvoice, forceIssue, memo);

                        try
                        {
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                SqlCommand cmd = cn.CreateCommand();
                                cmd.CommandText =
                                   "UPDATE SalesManage SET IssueDate = @IssueDate ,RequestDate = @RequestDate, Issue = 1  , IssueState = '발행', HasAcc = 1,invoicerMgtKey = @invoicerMgtKey,IssueLoginId = @IssueLoginId, IssueGubun = @IssueGubun  WHERE    SalesId =" + item["SalesId"].ToString() + " ";
                                cmd.Parameters.AddWithValue("@IssueDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@RequestDate", dtpEtaxDate.Value);
                                cmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);
                                cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
                                cmd.Parameters.AddWithValue("@IssueGubun", "차세로");
                                cmd.ExecuteNonQuery();

                                SqlCommand Customercmd = cn.CreateCommand();
                                Customercmd.CommandText =
                                   " UPDATE Customers " +
                                   " SET Misu = Misu+ @Misu WHERE CustomerId = @CustomerId  AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                                Customercmd.Parameters.AddWithValue("@Misu", item.Amount);
                                Customercmd.Parameters.AddWithValue("@CustomerId", item.CustomerId);

                                Customercmd.ExecuteNonQuery();


                                cn.Close();
                            }
                            Success++;
                            ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                            {
                                ClientId = LocalUser.Instance.LogInInformation.ClientId,
                                CDate = DateTime.Now,
                                Amount = -150,
                                MethodType = "전자 세금계산서",
                                Remark = $"{taxinvoice.invoiceeCorpName} ({taxinvoice.invoiceeCorpNum})",
                            });
                            ShareOrderDataSet.Instance.SaveChanges();
                        }
                        catch (PopbillException ex)
                        {
                            TaxInvoiceErrorDic.Add(item.SalesId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                            //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                            Error++;
                        }
                    }
                    catch (PopbillException ex)
                    {
                        TaxInvoiceErrorDic.Add(item.SalesId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                        //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                        Error++;
                    }
                }

                pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

                pnProgress.Invoke(new Action(() => Step5Complete()));


            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
           
           
            #endregion

        }

        private void Step5Complete()
        {
            Step5Info.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            Step5Info.Font = new Font(Step5Info.Font, FontStyle.Bold);
            //Step5Info.Text = "내보내기 완료 되었습니다.";
           
            if(TaxInvoiceErrorDic.Count > 0)
            {
                Step5Info.Text = "오류메세지를 확인해주세요.";
                newDGV1.FirstDisplayedScrollingColumnIndex = 7;
            }
            else
            {
                Step5Info.Text = "전자세금계산서 발행이 완료 되었습니다.";
            }

                 
            btn_Next.Text = "완료";
            btn_Close.Enabled = false;
            btn_Next.Enabled = true;
        }

        BindingList<BankExport> AccLogsItemBindingList = new BindingList<BankExport>();
        BindingList<BankExportNew> BankExportNewBindingList = new BindingList<BankExportNew>();
        private void Step5Display(bool first, int _TradeId = 0)
        {
            newDGV1.DataSource = null;

            foreach (var sale in AcceptSales)
            {
                var Added = new BankExportNew
                {
                    Id = sale.SalesId,
                    SangHo =sale.SangHo,
                    BizNo =sale.BizNo,
                    Ceo = sale.Ceo,
                    Price = sale.Price.ToString("N0"),
                    Vat = sale.Vat.ToString("N0"),
                    Amount = sale.Amount.ToString("N0"),
                    Item = txtItem.Text,
                    IssueDate = dtpEtaxDate.Text,
                 

                };
                BankExportNewBindingList.Add(Added);
            }
          //  Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptTrades.Count());

            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = BankExportNewBindingList;


            //Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptSales.Count());

            //newDGV1.AutoGenerateColumns = false;
            //newDGV1.DataSource = AccLogsItemBindingList;
           // Step5Progress.Value = Step5Done;




        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 2)
                ShowStep3();
            else if (CurrentStep == 3)
                if (AcceptSales.Count() == 0)
                {
                    MessageBox.Show("전사세금계산서 발행할 항목들을 선택하여 주십시오.");
                }
                else
                {
                    ShowStep4();
                }
            else if (CurrentStep == 4)
            {

                //if (MessageBox.Show("" + cmb_Bank.Text + " 데이터를 생성 하시겠습니까?\r※ “C:\\차세로\\대량이체” 폴더에 자동저장\r※ “Excel 97-2003 통합문서” 로 저장 됨.", "대량이체 확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                ShowStep5();

            }
            else if (CurrentStep == 5)
                Close();
        }

        private void btn_Pre_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 4)
                ShowStep3();

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Gubun))
            {
                var _AcceptSales = AcceptSales.ToArray();

                foreach (var item in AcceptSales)
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText =
                            "Delete SalesManage  WHERE SalesId = @SalesId";

                        cmd.CommandText +=
                          " Delete SalesManageDetail  WHERE SalesId = @SalesId";

                        cmd.Parameters.AddWithValue("@SalesId", item.SalesId);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            Close();
        }

        private void Step3List_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (Step3List.RowCount - e.RowIndex).ToString("N0");
            }
        }

        private void Dialog_AcceptAcc_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Step5Info.Text == "전자세금계산서 발행이 완료 되었습니다.")
            {


            }
            else
            {
                if (!String.IsNullOrEmpty(Gubun))
                {
                    var _AcceptSales = AcceptSales.ToArray();

                    foreach (var item in AcceptSales)
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                "Delete SalesManage  WHERE SalesId = @SalesId";

                            cmd.CommandText +=
                              " Delete SalesManageDetail  WHERE SalesId = @SalesId";

                            cmd.Parameters.AddWithValue("@SalesId", item.SalesId);
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                }
            }
            IsClosed = true;
        }


        class BankExport : INotifyPropertyChanged
        {
            private int _Id;
            private String _SangHo;
            private String _BizNo;
            private String _Ceo;
            private String _Price;
            private String _Vat;
            private String _Amount;
            private String _Item;
            private String _IssueDate;

            //

            public int Id { get { return _Id; } set { SetField(ref _Id, value); } }
            public string SangHo { get { return _SangHo; } set { SetField(ref _SangHo, value); } }
            public string BizNo { get { return _BizNo; } set { SetField(ref _BizNo, value); } }
            public string Ceo { get { return _Ceo; } set { SetField(ref _Ceo, value); } }
            public string Price { get { return _Price; } set { SetField(ref _Price, value); } }
            public string Vat { get { return _Vat; } set { SetField(ref _Vat, value); } }
            public string Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            public string Item { get { return _Item; } set { SetField(ref _Item, value); } }
            public string IssueDate { get { return _IssueDate; } set { SetField(ref _IssueDate, value); } }

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

        class BankExportNew : INotifyPropertyChanged
        {
            private int _Id;
            private String _SangHo;
            private String _BizNo;
            private String _Ceo;
            private String _Price;
            private String _Vat;
            private String _Amount;
            private String _Item;
            private String _IssueDate;

            //

            public int Id { get { return _Id; } set { SetField(ref _Id, value); } }
            public string SangHo { get { return _SangHo; } set { SetField(ref _SangHo, value); } }
            public string BizNo { get { return _BizNo; } set { SetField(ref _BizNo, value); } }
            public string Ceo { get { return _Ceo; } set { SetField(ref _Ceo, value); } }
            public string Price { get { return _Price; } set { SetField(ref _Price, value); } }
            public string Vat { get { return _Vat; } set { SetField(ref _Vat, value); } }
            public string Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            public string Item { get { return _Item; } set { SetField(ref _Item, value); } }
            public string IssueDate { get { return _IssueDate; } set { SetField(ref _IssueDate, value); } }

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


       

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;
            //if (e.ColumnIndex == 0)
            //{
            //    e.Value = (newDGV1.RowCount - e.RowIndex).ToString("N0");
            //}
            //if (e.ColumnIndex == Step3IssueDate.Index)
            //{

            //}
        }


        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;
      



        private void Dialog_AcceptAcc_Load(object sender, EventArgs e)
        {
            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;


            ShowStep3();
        }





       

        private void btnPopbill_Click(object sender, EventArgs e)
        {
            LocalUser.Instance.LogInInformation.LoadClient();
            if (!String.IsNullOrEmpty(LocalUser.Instance.LogInInformation.Client.ShopID) && !String.IsNullOrEmpty(LocalUser.Instance.LogInInformation.Client.ShopPW))
            {
                FrmPopbill f = new Admin.FrmPopbill();
                f.Show();
            }
            else
            {
                MessageBox.Show("전자세금관리에 필요한 팝빌 어카운트 정보가 존재하지 않습니다. 확인을 누르면 관련 화면으로 이동합니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                FrmMDI.LoadForm("FrmMN0204_CARGOOWNERMANAGE", "");
            }
        }

        private void newDGV1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
           // var Selected = BankExportNewBindingList[newDGV1.SelectedRows[0].Index];
           var Selected = BankExportNewBindingList[e.RowIndex];
            if (e.ColumnIndex == 1)
            {
                
                e.Value = (newDGV1.RowCount - e.RowIndex).ToString("N0");
            }
            if (e.ColumnIndex == ErrorMessage.Index)
            {
                if (TaxInvoiceErrorDic.ContainsKey(Selected.Id))
                {
                    e.Value = TaxInvoiceErrorDic[Selected.Id];
                }
                else
                {
                    e.Value = "";
                }
            }
        }

        private void Step3AllSelect_CheckedChanged(object sender, EventArgs e)
        {
           
          
        }
    }

}
