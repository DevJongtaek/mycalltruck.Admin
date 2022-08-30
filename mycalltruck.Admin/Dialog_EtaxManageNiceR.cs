using mycalltruck.Admin.Class;
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
    public partial class Dialog_EtaxManageNiceR : Form
    {
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
       // string linkId = "edubillsys";


        string memo = "";
        int Error = 0;
        int Success = 0;
        string Gubun = "";
        string _staxType = "과세";
        string _TypeCode = "0103";
        public Dialog_EtaxManageNiceR()
        {
            InitializeComponent();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);

            salesManageDetailTableAdapter.Fill(this.salesDataSet.SalesManageDetail);

            AdminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
            SetCustomerList();

        }
        public Dialog_EtaxManageNiceR(string _rdopurposeType1,string _taxType,string _Gubun)
        {
            InitializeComponent();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);

            salesManageDetailTableAdapter.Fill(this.salesDataSet.SalesManageDetail);

            AdminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
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

            if (_staxType != "과세")
            {
                _TypeCode = "0105";
            }
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


                        lbl_TotalMoney.Text = " ■ 총 발행금액 : " + AcceptSales.Sum(c => c.Amount).ToString("N0") + " 원";
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


                            lbl_TotalMoney.Text = " ■ 총 발행금액 : " + AcceptSales.Sum(c => c.Amount).ToString("N0") + " 원";
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
                            lbl_TotalMoney.Text = " ■ 총 발행금액 : " + "0";

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
            var _Clients = LocalUser.Instance.LogInInformation.Client;
            var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";


            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "dpebqlf54906**" }).ToString();
            string Tdtp_Date = dtpEtaxDate.Text;

            if (rdopurposeType1.Checked)
            {
                _purposeType = "02";
            }
            else
            {
                _purposeType = "01";
            }

            TaxInvoiceErrorDic.Clear();

            bar.Value = 0;
            bar.Maximum = AcceptSales.Count();
            bar.Visible = true;
            pnProgress.Visible = true;


            Thread t = new Thread(new ThreadStart(() =>
            {

                string DutyNum = string.Empty;
                string DescriptionText = "";
                int i = 0;

                Success = 0;
                Error = 0;
                var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
                if (_HideAccountNo == 0)
                {
                    DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner + "/" + _Clients.CMSAccountNo;
                }
                else
                {
                    DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner;

                }
                foreach (var item in AcceptSales)
                {
                    decimal _Price = 0;
                    decimal _Vat = 0;
                    decimal _Amont = 0;
                    var DetailQuery1 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName1) && c.SalesId == item.SalesId).ToArray();
                    var DetailQuery2 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName2) && c.SalesId == item.SalesId).ToArray();
                    var DetailQuery3 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName3) && c.SalesId == item.SalesId).ToArray();
                    var DetailQuery4 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName4) && c.SalesId == item.SalesId).ToArray();
                    if(DetailQuery1.Any() && DetailQuery2.Any() && DetailQuery3.Any() && DetailQuery4.Any())
                    {
                        _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2 + DetailQuery3.First().unitCost3 + DetailQuery4.First().unitCost4;
                        _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2 + DetailQuery3.First().tax3 + DetailQuery4.First().tax4;
                    }
                    else if(DetailQuery1.Any() && DetailQuery2.Any() && DetailQuery3.Any() && !DetailQuery4.Any())
                    {
                        _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2 + DetailQuery3.First().unitCost3;
                        _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2 + DetailQuery3.First().tax3;
                    }
                    else if (DetailQuery1.Any() && DetailQuery2.Any() && !DetailQuery3.Any() && !DetailQuery4.Any())
                    {
                        _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2;
                        _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2;
                    }
                    else if (DetailQuery1.Any() && !DetailQuery2.Any() && !DetailQuery3.Any() && !DetailQuery4.Any())
                    {
                        _Price = DetailQuery1.First().unitCost1;
                        _Vat = DetailQuery1.First().tax1;
                    }
                    else
                    {
                        _Price = item.Price;
                        _Vat = item.Vat;
                    }

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
                    $"          <ID>{_Clients.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
                    $"          <TypeCode>{_Clients.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
                    $"          <NameText>{_Clients.Name}</NameText>	<!-- 상호명 -->\r\n" +
                    $"          <ClassificationCode>{_Clients.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
                    $"          <SpecifiedOrganization>\r\n" +
                    $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
                    $"          </SpecifiedOrganization>\r\n" +
                    $"          <SpecifiedPerson>\r\n" +
                    $"              <NameText>{_Clients.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
                    $"          </SpecifiedPerson>\r\n" +
                    $"          <DefinedContact>\r\n" +
                    $"              <PersonNameText>{_Clients.CEO}</PersonNameText> <!-- 담당자명 -->\r\n" +
                    $"              <TelephoneCommunication>{_Clients.MobileNo}</TelephoneCommunication> <!-- 전화번호 -->\r\n" +
                    $"              <URICommunication>{_Clients.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
                    $"          </DefinedContact>\r\n" +
                    $"          <SpecifiedAddress>\r\n" +
                    $"              <LineOneText>{_Clients.AddressState + " " + _Clients.AddressCity + " " + _Clients.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
                    $"          </SpecifiedAddress>\r\n" +
                    $"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
                    $"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
                    $"          <ID>{item.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
                    $"          <TypeCode>{item.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
                    $"          <NameText>{item.SangHo}</NameText>	<!-- 상호명 -->\r\n" +
                    $"          <ClassificationCode>{item.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
                    $"          <SpecifiedOrganization>\r\n" +
                    $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
                    $"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
                    $"          </SpecifiedOrganization>\r\n" +
                    $"          <SpecifiedPerson>\r\n" +
                    $"              <NameText>{item.Ceo}</NameText>	<!--  대표자명 -->\r\n" +
                    $"          </SpecifiedPerson>\r\n" +
                    $"          <PrimaryDefinedContact>\r\n" +
                    $"              <PersonNameText>{item.Ceo}</PersonNameText>	<!-- 담당자명 -->\r\n" +
                    $"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
                    $"              <TelephoneCommunication>{item.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
                    $"              <URICommunication>{item.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
                    $"          </PrimaryDefinedContact>\r\n" +
                    $"          <SpecifiedAddress>\r\n" +
                    $"              <LineOneText>{item.AddressState + " " + item.AddressCity + " " + item.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
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

                    if (DetailQuery1.Any())
                    {
                        dtiXml += "" +
                        $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                        $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                        $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery1.First().unitCost1)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                        $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery1.First().qty1)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                        $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                        $"      <NameText>{DetailQuery1.First().itemName1}</NameText>  <!-- 품목명 -->\r\n" +
                        $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                        $"          <TotalTax>\r\n" +
                        $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery1.First().tax1)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                        $"          </TotalTax>\r\n" +
                        $"          <UnitPrice>\r\n" +
                        $"              <UnitAmount>{Convert.ToInt64(DetailQuery1.First().supplyCost1)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                        $"          </UnitPrice>\r\n" +
                        $"          <DescriptionText>{txtRemark2.Text}</DescriptionText>       <!-- 품목비고 -->\r\n" +
                        $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
                    }
                    else
                    {
                        dtiXml += "" +
                          $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                          $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                          $"      <InvoiceAmount>{Convert.ToInt64(_Price)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                          $"      <ChargeableUnitQuantity>1</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                          $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                          $"      <NameText>{txtItem.Text}</NameText>  <!-- 품목명 -->\r\n" +
                          $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                          $"          <TotalTax>\r\n" +
                          $"              <CalculatedAmount>{Convert.ToInt64(_Vat)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                          $"          </TotalTax>\r\n" +
                          $"          <UnitPrice>\r\n" +
                          $"              <UnitAmount>{Convert.ToInt64(_Price)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                          $"          </UnitPrice>\r\n" +
                          $"          <DescriptionText>{txtRemark2.Text}</DescriptionText>       <!-- 품목비고 -->\r\n" +
                          $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";

                    }
                    if (DetailQuery2.Any())
                    {
                        dtiXml += "" +
                        $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                        $"      <SequenceNumeric>2</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                        $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery2.First().unitCost2)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                        $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery2.First().qty2)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                        $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                        $"      <NameText>{DetailQuery2.First().itemName2}</NameText>  <!-- 품목명 -->\r\n" +
                        $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                        $"          <TotalTax>\r\n" +
                        $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery2.First().tax2)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                        $"          </TotalTax>\r\n" +
                        $"          <UnitPrice>\r\n" +
                        $"              <UnitAmount>{Convert.ToInt64(DetailQuery2.First().supplyCost2)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                        $"          </UnitPrice>\r\n" +
                        $"          <DescriptionText>{txtRemark2.Text}</DescriptionText>       <!-- 품목비고 -->\r\n" +
                        $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
                    }

                    if (DetailQuery3.Any())
                    {
                        dtiXml += "" +
                        $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                        $"      <SequenceNumeric>3</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                        $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery3.First().unitCost3)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                        $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery3.First().qty3)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                        $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                        $"      <NameText>{DetailQuery3.First().itemName3}</NameText>  <!-- 품목명 -->\r\n" +
                        $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                        $"          <TotalTax>\r\n" +
                        $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery3.First().tax3)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                        $"          </TotalTax>\r\n" +
                        $"          <UnitPrice>\r\n" +
                        $"              <UnitAmount>{Convert.ToInt64(DetailQuery3.First().supplyCost3)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                        $"          </UnitPrice>\r\n" +
                        $"          <DescriptionText>{txtRemark2.Text}</DescriptionText>       <!-- 품목비고 -->\r\n" +
                        $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
                    }

                    if (DetailQuery4.Any())
                    {
                        dtiXml += "" +
                        $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                        $"      <SequenceNumeric>4</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                        $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery4.First().unitCost4)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                        $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery4.First().qty4)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                        $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                        $"      <NameText>{DetailQuery4.First().itemName4}</NameText>  <!-- 품목명 -->\r\n" +
                        $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                        $"          <TotalTax>\r\n" +
                        $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery4.First().tax4)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                        $"          </TotalTax>\r\n" +
                        $"          <UnitPrice>\r\n" +
                        $"              <UnitAmount>{Convert.ToInt64(DetailQuery4.First().supplyCost4)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                        $"          </UnitPrice>\r\n" +
                        $"          <DescriptionText>{txtRemark2.Text}</DescriptionText>       <!-- 품목비고 -->\r\n" +
                        $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
                    }
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


                    var Result = "";
                    // var Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Clients.frnNo, _Clients.userid, _Clients.passwd, certPw, dtiXml, "Y", "N", "", "6");
                    if (rdodelay.Checked)
                    {
                        Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "", "3");
                    }
                    else
                    {
                        Result = dTIServiceClass.makeAndPublishSign(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "");
                    }
                    

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
                                           "UPDATE SalesManage SET IssueDate = @IssueDate ,RequestDate = @RequestDate, Issue = 1  , IssueState = '발행', HasAcc = 1,billNo = @billNo,IssueLoginId = @IssueLoginId, IssueGubun = @IssueGubun,purposeType = @purposeType ,TypeCode = @TypeCode WHERE    SalesId =" + item.SalesId + " ";
                                        cmd.Parameters.AddWithValue("@IssueDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@RequestDate", dtpEtaxDate.Value);
                                        cmd.Parameters.AddWithValue("@billNo", billNo);
                                        cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
                                        cmd.Parameters.AddWithValue("@IssueGubun", "차세로");
                                        cmd.Parameters.AddWithValue("@purposeType", _purposeType);
                                        cmd.Parameters.AddWithValue("@TypeCode", "위수탁");
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
                                        Amount = -110,
                                        MethodType = "전자 세금계산서",
                                        //Remark = $"{taxinvoice.invoiceeCorpName} ({taxinvoice.invoiceeCorpNum})",
                                        Remark = $"{item.SangHo} ({item.BizNo.Replace("-","")})",
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
                        else
                        {
                            TaxInvoiceErrorDic.Add(item.SalesId, "[ " + retVal + " ] " + errMsg);
                            //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                            Error++;
                        }

                    }
                    catch
                    {

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
            ////전자세금계산서 모듈 초기화
            //taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            ////연동환경 설정값, 테스트용(true), 상업용(false)
            //taxinvoiceService.IsTest = false;


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
