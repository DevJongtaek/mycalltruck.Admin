using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0204_CARGOOWNERMANAGE : Form
    {
        bool Account_Result = false;
        bool ClientsBindingSource_CurrentChanging = false;
        DESCrypt m_crypt = null;
        string Up_Status = string.Empty;
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
        int GridIndex = 0;
        ShareOrderDataSet ShareOrderDataSet = ShareOrderDataSet.Instance;
        List<ClientPoint> PointDataSource = new List<ClientPoint>();
        string _Gubun = "";

        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
            auth = this.GetAuth();
            switch (auth)
            {
                case MenuAuth.None:
                    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                    return;
                case MenuAuth.Read:

                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                    RegisterDriver.Enabled = false;



                    dataGridView1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }


        public FrmMN0204_CARGOOWNERMANAGE(string _sGubun = "")
        {
            _Gubun = _sGubun;


            m_crypt = new DESCrypt("12345678");

            foreach (var item in SingleDataSet.Instance.AddressReferences)
            {
                if (String.IsNullOrEmpty(item.State) || String.IsNullOrEmpty(item.City) || String.IsNullOrEmpty(item.Street))
                    continue;
                if (!AddressList.Any(c => c.State == item.State && c.City == item.City && c.Street == item.Street))
                {
                    AddressList.Add(new AddressViewModel
                    {
                        State = item.State,
                        City = item.City,
                        Street = item.Street,
                    });
                }
            }

            InitDealerTable();
            InitializeComponent();

            //clientsBindingSource.DataSource = SingleDataSet.Instance;
            //clientsBindingSource.DataMember = "Clients";
            //clientsTableAdapter.Fill(SingleDataSet.Instance.Clients);

            _InitCmbSearch();
            _InitCmb();

            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                Point.ReadOnly = false;
                ChangePoint.Visible = true;
            }
            else
            {
                ChildBizNo.ReadOnly = true;
                ChildBizNoSearch.Enabled = false;
                ChildAdd.Enabled = false;
                ChildDelete.Enabled = false;
            }
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            

            //if(Gubun =="Point")
            // {
            //     MessageBox.Show("포인트 내역 탭으로 이동하여 , 가상계좌번호를 확인후 충전 바랍니다.");
            // }
        }

        private void FrmMN0204_CARGOOWNERMANAGE_Load(object sender, EventArgs e)
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                btn_Excel.Visible = false;
                UpdateVAccount.Visible = false;
            }
            else
            {
                dataGridView1.Columns[1].Visible = true;
                dataGridView1.Columns[2].Visible = true;
                btn_Excel.Visible = true;
                UpdateVAccount.Visible = true;
            }

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_New.Enabled = false;
                btnCurrentDelete.Enabled = false;
                cmb_BizType.Enabled = false;
                txt_BizNo.ReadOnly = true;
                cmb_Admin.Enabled = false;
                cmb_status.Enabled = false;
                txt_CEOBirth.ReadOnly = true;
                panel6.Enabled = false;
                ShopID.Enabled = true;
                ShopPW.Enabled = true;
            }
            else
            {
                btn_New.Enabled = true;
                btnCurrentDelete.Enabled = true;
                cmb_BizType.Enabled = true;
                txt_BizNo.ReadOnly = false;
                cmb_Admin.Enabled = true;
                cmb_status.Enabled = true;
                txt_CEOBirth.ReadOnly = false;
                panel6.Enabled = true;
            }
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }

            btn_Search_Click(null, null);
            if (_Gubun == "Point")
            {
                clientsBindingSource_CurrentChanged(null, null);
                tabControl1.SelectedTab = BasePage;

                tabControl1.SelectedTab = PointPage;
                tabControl1_SelectedIndexChanged(null, null);
            }
            else
            {
                tabControl1.SelectedTab = BasePage;
            }

        }
        class DealerViewModel
        {
            public int DealerId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public int BizName { get; set; }
            public int Status { get; set; }
        }
        private List<DealerViewModel> _DealerTable = new List<DealerViewModel>();

        private void InitDealerTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DealerId, Code, Name, BizName,Status FROM Dealers ";
                    //commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _DealerTable.Add(
                              new DealerViewModel
                              {
                                  DealerId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  BizName = dataReader.GetInt32(3),
                                  Status = dataReader.GetInt32(4),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        private void _InitCmb()
        {
            var BizTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ClientType" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_BizType.DataSource = BizTypeDataSource;
            cmb_BizType.DisplayMember = "Name";
            cmb_BizType.ValueMember = "value";

            bizTypeDataGridViewTextBoxColumn.DataSource = BizTypeDataSource;
            bizTypeDataGridViewTextBoxColumn.DisplayMember = "Name";
            bizTypeDataGridViewTextBoxColumn.ValueMember = "value";

            Dictionary<string, string> PayBank = new Dictionary<string, string>();
            PayBank.Add("", "은행선택");
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

            cmb_Bank.DataSource = new BindingSource(PayBank, null);
            cmb_Bank.DisplayMember = "Value";
            cmb_Bank.ValueMember = "Key";

            cmb_Bank1.DataSource = new BindingSource(PayBank, null);
            cmb_Bank1.DisplayMember = "Value";
            cmb_Bank1.ValueMember = "Key";
            cmb_Bank1.SelectedIndex = 0;

            cmb_Bank2.DataSource = new BindingSource(PayBank, null);
            cmb_Bank2.DisplayMember = "Value";
            cmb_Bank2.ValueMember = "Key";
            cmb_Bank2.SelectedIndex = 0;

            cmb_Bank3.DataSource = new BindingSource(PayBank, null);
            cmb_Bank3.DisplayMember = "Value";
            cmb_Bank3.ValueMember = "Key";
            cmb_Bank3.SelectedIndex = 0;

            cmb_Bank4.DataSource = new BindingSource(PayBank, null);
            cmb_Bank4.DisplayMember = "Value";
            cmb_Bank4.ValueMember = "Key";
            cmb_Bank4.SelectedIndex = 0;


            Dictionary<string, string> EtaxGubun = new Dictionary<string, string>();
            EtaxGubun.Add("P", "팝빌");
            EtaxGubun.Add("N", "나이스DNR");
            cmbEtaxGubun.DataSource = new BindingSource(EtaxGubun, null);
            cmbEtaxGubun.DisplayMember = "Value";
            cmbEtaxGubun.ValueMember = "Key";
            cmbEtaxGubun.SelectedIndex = 0;
            


            //var DealersDataSource = _DealerTable.Select(c => new { c.Name, c.Code, c.DealerId }).OrderBy(c => c.DealerId).ToArray();
            //cmb_Admin.DataSource = DealersDataSource;
            //cmb_Admin.DisplayMember = "Name";
            //cmb_Admin.ValueMember = "DealerId";

            var DealersDataSource = _DealerTable.Where(c => c.Status == 2).Select(c => new { c.Name, c.Code, c.DealerId }).OrderBy(c => c.DealerId).ToArray();
            cmb_Admin.DataSource = DealersDataSource;
            cmb_Admin.DisplayMember = "Name";
            cmb_Admin.ValueMember = "DealerId";

           // cmb_Admin.SelectedIndex = 1;


            //var DealersDataSource = _DealerTable.Where(c => c.Status == 2).Select(c => new { c.Name, c.Code, c.DealerId }).OrderBy(c => c.DealerId).ToArray();
            //cmb_Admin.DataSource = DealersDataSource;
            //cmb_Admin.DisplayMember = "Name";
            //cmb_Admin.ValueMember = "DealerId";

            //cmb_Admin.SelectedIndex = 1;

            var SatusDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_status.DataSource = SatusDataSource;
            cmb_status.DisplayMember = "Name";
            cmb_status.ValueMember = "value";

            var NoticeDriverDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NoticeDriver" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_NoticeDriver.DataSource = NoticeDriverDataSource;
            cmb_NoticeDriver.DisplayMember = "Name";
            cmb_NoticeDriver.ValueMember = "value";

            var StatusDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_statusI.DataSource = StatusDataSource;
            cmb_statusI.DisplayMember = "Name";
            cmb_statusI.ValueMember = "value";

            var ClientTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ClientType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_BizTypeI.DataSource = ClientTypeDataSource;
            cmb_BizTypeI.DisplayMember = "Name";
            cmb_BizTypeI.ValueMember = "value";

            var ExcelTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ExcelType" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();

            cmb_ExcelType.DataSource = ExcelTypeDataSource;
            cmb_ExcelType.DisplayMember = "Name";
            cmb_ExcelType.ValueMember = "value";

            var InTypeSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InType" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_DriverType.DataSource = InTypeSourceDataSource;
            cmb_DriverType.DisplayMember = "Name";
            cmb_DriverType.ValueMember = "value";

            var InTypeSourceDataSource2 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InType" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_ContType.DataSource = InTypeSourceDataSource2;
            cmb_ContType.DisplayMember = "Name";
            cmb_ContType.ValueMember = "value";

            var InTypeSourceDataSource3 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InType" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_OrderType.DataSource = InTypeSourceDataSource3;
            cmb_OrderType.DisplayMember = "Name";
            cmb_OrderType.ValueMember = "value";


           
            var KakaoPriceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "KakaoPrice" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmbKakaoPrice.DataSource = KakaoPriceDataSource;
            cmbKakaoPrice.DisplayMember = "Name";
            cmbKakaoPrice.ValueMember = "value";


            Dictionary<bool, string> AllowFPIS_In = new Dictionary<bool, string>();
            AllowFPIS_In.Add(true, "사용한다");
            AllowFPIS_In.Add(false, "사용안한다");
            cmb_AllowFPIS_In.DataSource = new BindingSource(AllowFPIS_In, null);
            cmb_AllowFPIS_In.DisplayMember = "Value";
            cmb_AllowFPIS_In.ValueMember = "Key";

            Dictionary<bool, string> AllowSMS = new Dictionary<bool, string>();
            AllowSMS.Add(true, "사용한다");
            AllowSMS.Add(false, "사용안한다");
            cmb_AllowSMS.DataSource = new BindingSource(AllowSMS, null);
            cmb_AllowSMS.DisplayMember = "Value";
            cmb_AllowSMS.ValueMember = "Key";

            Dictionary<bool, string> AllowOrder = new Dictionary<bool, string>();
            AllowOrder.Add(true, "사용한다");
            AllowOrder.Add(false, "사용안한다");
            cmbAllowOrder.DataSource = new BindingSource(AllowOrder, null);
            cmbAllowOrder.DisplayMember = "Value";
            cmbAllowOrder.ValueMember = "Key";

            Dictionary<bool, string> AllowFPIS = new Dictionary<bool, string>();
            AllowFPIS.Add(true, "사용한다");
            AllowFPIS.Add(false, "사용안한다");
            cmbAllowFPIS.DataSource = new BindingSource(AllowFPIS, null);
            cmbAllowFPIS.DisplayMember = "Value";
            cmbAllowFPIS.ValueMember = "Key";

            Dictionary<bool, string> AllowSub = new Dictionary<bool, string>();
            AllowSub.Add(true, "사용한다");
            AllowSub.Add(false, "사용안한다");
            cmbAllowSub.DataSource = new BindingSource(AllowSub, null);
            cmbAllowSub.DisplayMember = "Value";
            cmbAllowSub.ValueMember = "Key";

            Dictionary<bool, string> AllowMultiCustomer = new Dictionary<bool, string>();
            AllowMultiCustomer.Add(true, "사업자번호 중복 허용");
            AllowMultiCustomer.Add(false, "사업자번호 중복 불가");
            cmbAllowMultiCustomer.DataSource = new BindingSource(AllowMultiCustomer, null);
            cmbAllowMultiCustomer.DisplayMember = "Value";
            cmbAllowMultiCustomer.ValueMember = "Key";

            Dictionary<bool, string> HasPoint = new Dictionary<bool, string>();
            HasPoint.Add(true, "포인트 사용");
            HasPoint.Add(false, "포인트 사용하지 않음");
            cmbHasPoint.DataSource = new BindingSource(HasPoint, null);
            cmbHasPoint.DisplayMember = "Value";
            cmbHasPoint.ValueMember = "Key";

            Dictionary<bool, string> NeedCustomerId = new Dictionary<bool, string>();
            NeedCustomerId.Add(true, "화주 선택 필수");
            NeedCustomerId.Add(false, "화주 선택 필수 아님");
            cmbNeedCustomerId.DataSource = new BindingSource(NeedCustomerId, null);
            cmbNeedCustomerId.DisplayMember = "Value";
            cmbNeedCustomerId.ValueMember = "Key";

            Dictionary<bool, string> IsHolderShop = new Dictionary<bool, string>();
            IsHolderShop.Add(true, "사용한다");
            IsHolderShop.Add(false, "사용안한다");
            cmbIsHolderShop.DataSource = new BindingSource(IsHolderShop, null);
            cmbIsHolderShop.DisplayMember = "Value";
            cmbIsHolderShop.ValueMember = "Key";

            Dictionary<bool, string> HideCustomer = new Dictionary<bool, string>();
            HideCustomer.Add(true, "숨긴다");
            HideCustomer.Add(false, "보인다");
            cmbHideCustomer.DataSource = new BindingSource(HideCustomer, null);
            cmbHideCustomer.DisplayMember = "Value";
            cmbHideCustomer.ValueMember = "Key";


            Dictionary<bool, string> IsHideAddtrade = new Dictionary<bool, string>();
            IsHideAddtrade.Add(true, "사용한다");
            IsHideAddtrade.Add(false, "사용안한다");
            cmbHideAddtrade.DataSource = new BindingSource(IsHideAddtrade, null);
            cmbHideAddtrade.DisplayMember = "Value";
            cmbHideAddtrade.ValueMember = "Key";

            Dictionary<bool, string> IsHideAddSales = new Dictionary<bool, string>();
            IsHideAddSales.Add(true, "사용한다");
            IsHideAddSales.Add(false, "사용안한다");
            cmbHideAddSales.DataSource = new BindingSource(IsHideAddSales, null);
            cmbHideAddSales.DisplayMember = "Value";
            cmbHideAddSales.ValueMember = "Key";


            Dictionary<string, string> IsSignLocation = new Dictionary<string, string>();
            IsSignLocation.Add("LU", "좌측상단");
            IsSignLocation.Add("LL", "촤측하단");
            IsSignLocation.Add("RU", "우측상단");
            IsSignLocation.Add("RL", "우측하단");
            cmbSignLocation.DataSource = new BindingSource(IsSignLocation, null);
            cmbSignLocation.DisplayMember = "Value";
            cmbSignLocation.ValueMember = "Key";

        

        }

        private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
        private void _InitCmbSearch()
        {
            DataGridViewColumn[] cols = new DataGridViewColumn[]{codeDataGridViewTextBoxColumn,nameDataGridViewTextBoxColumn,bizNoDataGridViewTextBoxColumn,loginIdDataGridViewTextBoxColumn,cEODataGridViewTextBoxColumn,LGD_MID
             };
            cmb_Search.Items.Clear();
            DicSearch.Clear();
            cmb_Search.Items.Add("전체");

            foreach (var item in cols)
            {

                cmb_Search.Items.Add(item.HeaderText);
                if (item.DataPropertyName == null || item.DataPropertyName == "")
                {
                    //DicSearch.Add(item.HeaderText, "'" + item.Name);
                }
                else
                {
                    DicSearch.Add(item.HeaderText, item.DataPropertyName);
                }
            }
            //cmb_Search.Items.Add("가맹점-ID");
            if (cmb_Search.Items.Count > 0) cmb_Search.SelectedIndex = 0;








        }
        private void _Search()
        {
            clientDataSet.Clients.Clear();
            var _SelectQuery =
                @"SELECT  
                    ClientId, PhoneNo, Name, BizNo, CEO, Uptae, Upjong, Email, LoginId, Password, MobileNo, FaxNo, CreateDate, ZipCode, Status, AddressDetail, ShopID, ShopPW, Code, AddressState, 
                    AddressCity, DealerId, Admin, ServiceDate, LGD_MID, ExcelType, ContType, OrderType, DriverType, NoticeDriver, NoticeCnt, AllowFPIS_In, AllowSMS, AllowOrder, AllowFPIS, AllowTax, 
                    Stamp, AllowSub, AllowMultiCustomer, HasPoint, NeedCustomerId, BizType, CMSOwner, CMSBankName, CMSBankCode, CMSAccountNo, IsHolderShop, UseDriverPoint, HideCustomer,HideAddTrade,HideAddSales,SignLocation,HideMyCarOrder,OrderGubun,VBankName,VAccount, CMSOwner1, CMSBankName1, CMSBankCode1, CMSAccountNo1, CMSOwner2, CMSBankName2, CMSBankCode2, CMSAccountNo2, CMSOwner3, CMSBankName3, CMSBankCode3, CMSAccountNo3, CMSOwner4, CMSBankName4, CMSBankCode4, CMSAccountNo4,StatsGubun,KakaoPriceGubun,smsyn,AuthKey,CargoApiYn,CargoLoginId,linkCd, frnNo, userid, passwd, 
               NPKIpasswd ,EtaxGubun ,NPKIpasswdEnc, Call24LoginId, Call24SecretKey, Call24EncKey, Call24IVKey
                FROM Clients";

            string _FilterString = txt_Search.Text;
            string _cmbSearchString = cmb_Search.Text;
            string _StatusSearchString = string.Empty;
            string _StatusSearchString2 = string.Empty;

            int Sstatus = (int)cmb_statusI.SelectedValue;
            int Sbiztype = (int)cmb_BizTypeI.SelectedValue;

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                List<String> WhereList = new List<string>();
                if (Sstatus > 0)
                {
                    WhereList.Add($"Status = {Sstatus}");
                }
                if (Sbiztype > 0)
                {
                    WhereList.Add($"BizType = {Sbiztype}");
                }
                //if (cmb_Search.SelectedIndex > 0 && txt_Search.Text.Length > 0)
                //{

                //    WhereList.Add($"{DicSearch[cmb_Search.Text]} LIKE '%{txt_Search.Text.Replace("-","")}%'");
                //}
                if (cmb_Search.SelectedIndex > 0)
                {
                    switch (cmb_Search.Text)
                    {
                        case "운송사코드":
                            WhereList.Add("Code LIKE '%" + txt_Search.Text + "%'");
                            break;
                        case "운송사명":
                            WhereList.Add("Name LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "사업자번호":
                            WhereList.Add("REPLACE(BizNo,'-','') LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "아이디":
                            WhereList.Add("LoginId LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "대표자":
                            WhereList.Add("Ceo LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "가맹점-ID":
                            WhereList.Add("LGD_MID LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                       
                        default:
                            break;
                    }
                }
                if (WhereList.Count > 0)    
                {
                    _SelectQuery += Environment.NewLine
                        + "WHERE " + String.Join(" AND ", WhereList);

                   
                }
            }
            else
            {
                _SelectQuery += Environment.NewLine
                    + $@"WHERE ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
            }

            _SelectQuery += Environment.NewLine + "ORDER BY ClientId DESC";

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = _SelectQuery;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        clientDataSet.Clients.Clear();
                        clientDataSet.Clients.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }
        string Account_Name = "";
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Selected == null)
                return;

            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");
                return;
            }
            else
            {
                var Query1 =
                       "Select Count(*) From Clients Where BizNo = @BizNo AND ClientId != @ClientId";
                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                    cmd1.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }
                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show("사업자번호가 중복되었습니다.!!. 다른사업자 사업자번호를 입력해주십시오.");
                    err.SetError(txt_BizNo, "사업자번호가 중복되었습니다!!");
                    return;
                }
            }
            if (txt_CEO.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Uptae.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Upjong.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_Password.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_PhoneNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }


         

            Up_Status = "Update";
            int _rows = UpdateDB(Up_Status);
        }

        private void MWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
            }
            else if (e.Error != null)
            {
            }
            else
            {
                if (e.Result != null && e.Result.ToLower() == "true")
                {
                    Account_Result = true;

                    Up_Status = "Update";
                    int _rows = UpdateDB(Up_Status);
                }
                else
                {
                    Account_Result = false;


                    //Up_Status = "Update";
                    //int _rows = UpdateDB(Up_Status);


                    MessageBox.Show("계좌인증 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // _UpdateDB();
                }
            }
        }
        private int UpdateDB(string Status)
        {
            int _rows = 0;
            try
            {
                clientsBindingSource.EndEdit();
                var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
                Row.Admin = cmb_Admin.Text;

                //Row.CMSBankName = cmb_Bank.Text;

                //Row.CMSBankName1 = cmb_Bank1.Text;
                //Row.CMSBankName2 = cmb_Bank2.Text;
                //Row.CMSBankName3 = cmb_Bank3.Text;
                //Row.CMSBankName4 = cmb_Bank4.Text;
                if (rdb_Tax0.Checked)
                {
                    Row.AllowTax = false;
                }
                else
                {
                    Row.AllowTax = true;
                }

                string NPKIpasswdEnc = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] {textBox1.Text}).ToString();
                if (Status == "Update")
                {
                    string _Status = "";
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "SELECT Status FROM Clients WHERE ClientId = @ClientId";
                            _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                            using (SqlDataReader _Reader = _Command.ExecuteReader())
                            {
                                if (_Reader.Read())
                                {
                                    _Status = _Reader.GetInt32(0).ToString();

                                }
                            }
                        }
                    });

                    
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                             @"UPDATE Clients
                            SET Name = @Name
                            , BizNo = @BizNo, CEO = @CEO, Uptae = @Uptae, Upjong = @Upjong, ZipCode = @ZipCode
                            , AddressState = @AddressState, AddressCity = @AddressCity, AddressDetail = @AddressDetail, Password = @Password, DealerId = @DealerId
                            , MobileNo = @MobileNo, PhoneNo = @PhoneNo, Email = @Email, BizType = @BizType, Status = @Status
                            , ShopID = @ShopID, ShopPW = @ShopPW,CargoLoginId=@CargoLoginId,NPKIpasswd = @NPKIpasswd,NPKIpasswdEnc = @NPKIpasswdEnc,Call24LoginId = @Call24LoginId,Call24SecretKey = @Call24SecretKey,Call24EncKey = @Call24EncKey,Call24IVKey = @Call24IVKey
                            
                            WHERE ClientId = @ClientId";
                           
                            _Command.Parameters.AddWithValue("@Name", Row.Name);
                            _Command.Parameters.AddWithValue("@BizNo", Row.BizNo);
                            _Command.Parameters.AddWithValue("@CEO", Row.CEO);
                            _Command.Parameters.AddWithValue("@Uptae", Row.Uptae);
                            _Command.Parameters.AddWithValue("@Upjong", Row.Upjong);
                            _Command.Parameters.AddWithValue("@ZipCode", Row.ZipCode);
                            _Command.Parameters.AddWithValue("@AddressState", Row.AddressState);
                            _Command.Parameters.AddWithValue("@AddressCity", Row.AddressCity);
                            _Command.Parameters.AddWithValue("@AddressDetail", Row.AddressDetail);
                            _Command.Parameters.AddWithValue("@Password", Row.Password);
                            _Command.Parameters.AddWithValue("@DealerId", Row.DealerId);
                            _Command.Parameters.AddWithValue("@MobileNo", Row.MobileNo);
                            _Command.Parameters.AddWithValue("@PhoneNo", Row.PhoneNo);
                            //_Command.Parameters.AddWithValue("@Name", Row.CEOBirth);
                            _Command.Parameters.AddWithValue("@Email", Row.Email);
                            _Command.Parameters.AddWithValue("@BizType", Row.BizType);
                            _Command.Parameters.AddWithValue("@Status", Row.Status);
                            _Command.Parameters.AddWithValue("@ShopID", Row.ShopID);
                            _Command.Parameters.AddWithValue("@ShopPW", Row.ShopPW);
                          

                            _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                            _Command.Parameters.AddWithValue("@CargoLoginId", Row.CargoLoginId);
                            _Command.Parameters.AddWithValue("@NPKIpasswd", Row.NPKIpasswd);
                            _Command.Parameters.AddWithValue("@NPKIpasswdEnc", NPKIpasswdEnc);

                            _Command.Parameters.AddWithValue("@Call24LoginId", Row.Call24LoginId);
                            _Command.Parameters.AddWithValue("@Call24SecretKey", Row.Call24SecretKey);
                            _Command.Parameters.AddWithValue("@Call24EncKey", Row.Call24EncKey);
                            _Command.Parameters.AddWithValue("@Call24IVKey", Row.Call24IVKey);

                            _Command.ExecuteNonQuery();
                        }
                    });

                    using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        SqlCommand Command = Connection.CreateCommand();
                        Command.CommandText = "Update ClientManage Set Name = @Name,CEO =@CEO,Addr1 =@Addr1,Addr2=@Addr2,AddrDetail=@AddrDetail,CPassword=@CPassword,CMobileNo=@CMobileNo,ClientPhoneNo = @ClientPhoneNo Where ClientCode = @ClientCode";
                        Command.Parameters.AddWithValue("@Name", txt_Name.Text);
                        Command.Parameters.AddWithValue("@CEO", txt_CEO.Text);
                        Command.Parameters.AddWithValue("@Addr1", Row.AddressState);
                        Command.Parameters.AddWithValue("@Addr2", Row.AddressCity);
                        Command.Parameters.AddWithValue("@AddrDetail", Row.AddressDetail);
                        Command.Parameters.AddWithValue("@CPassword", txt_Password.Text);
                        Command.Parameters.AddWithValue("@CMobileNo", txt_MobileNo.Text.Replace("-", ""));
                        Command.Parameters.AddWithValue("@ClientPhoneNo", txt_PhoneNo.Text.Replace("-", ""));
                        Command.Parameters.AddWithValue("@ClientCode", txt_Code.Text);
                        Connection.Open();
                        Command.ExecuteNonQuery();
                        Connection.Close();
                    }


                    if (cmb_status.Text == "제공" && _Status != "2")
                    {
                        if (!String.IsNullOrEmpty(txt_MobileNo.Text.Replace("-", "")))
                        {
                            if (Regex.Match(txt_MobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success)
                            {
                                DataSets.AppSMSDataSetTableAdapters.em_smt_tranTableAdapter em_smt_tranTableAdapter = new DataSets.AppSMSDataSetTableAdapters.em_smt_tranTableAdapter();
                                em_smt_tranTableAdapter.Insert(DateTime.Now, $"{txt_Name.Text} 님의 차세로 회원가입이 완료 되었습니다.\r\n문의 : 1661-6090", "028535111", "0", txt_MobileNo.Text.Replace("-", ""), "0");
                            }
                        }

                        if (String.IsNullOrEmpty(Row.VAccount))
                        {

                            //가상계좌할당
                            List<(int VAccountPoolId, String VBankName, String VAccount)> VAccountPoolList = new List<(int, string, string)>();
                            Data.Connection((_Connection) =>
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "SELECT [VAccountPoolId], [VBankName], [VAccount] FROM [VAccountPools] WHERE VBankName = @VBankName AND  [ClientId] IS NULL AND [DriverId] IS NULL";
                                    var _cmbbank = "국민";
                                    if (cmb_Bank.SelectedValue == null)
                                    {
                                        _cmbbank = "국민";
                                        _Command.Parameters.AddWithValue("@VBankName", "국민");

                                    }
                                    else if (cmb_Bank.SelectedValue.ToString().Contains("국민") || cmb_Bank.SelectedValue.ToString().Contains("기업") || cmb_Bank.SelectedValue.ToString().Contains("신한") || cmb_Bank.SelectedValue.ToString().Contains("농협") || cmb_Bank.SelectedValue.ToString().Contains("우리"))
                                    {
                                        _Command.Parameters.AddWithValue("@VBankName", cmb_Bank.SelectedValue.ToString().Substring(0, 2));
                                    }
                                    else
                                    {
                                        _Command.Parameters.AddWithValue("@VBankName", "국민");
                                    }

                                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                                    {
                                        while (_Reader.Read())
                                        {
                                            VAccountPoolList.Add((_Reader.GetInt32(0), _Reader.GetString(1), _Reader.GetString(2)));
                                        }
                                    }
                                }

                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "UPDATE Clients SET [VBankName] = @VBankName, [VAccount] = @VAccount WHERE ClientId = @ClientId;UPDATE VAccountPools SET ClientId = @ClientId WHERE VAccountPoolId = @VAccountPoolId;";
                                    _Command.Parameters.Add(new SqlParameter("@VBankName", SqlDbType.NVarChar));
                                    _Command.Parameters.Add(new SqlParameter("@VAccount", SqlDbType.NVarChar));
                                    _Command.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                                    _Command.Parameters.Add(new SqlParameter("@VAccountPoolId", SqlDbType.Int));

                                    var _VBankName = VAccountPoolList.First().VBankName;
                                    var _VAccount = VAccountPoolList.First().VAccount;
                                    _Command.Parameters["@VBankName"].Value = _VBankName;
                                    _Command.Parameters["@VAccount"].Value = _VAccount;
                                    _Command.Parameters["@ClientId"].Value = Row.ClientId;
                                    _Command.Parameters["@VAccountPoolId"].Value = VAccountPoolList.First().VAccountPoolId;
                                    _Command.ExecuteNonQuery();

                                }
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "INSERT INTO VACS_VACT " +
                                                "(ORG_CD,bank_cd,acct_no,cmf_nm,acct_st,reg_il,open_il,tramt_cond,trmc_cond,trbegin_il,trend_il,trbegin_si,trend_si,seq_no)" +
                                                "VALUES('31000745',@bank_cd,@acct_no,'(주)에듀빌시스템','1','19990101','20180601','0','1','19990101','99991231','000000','000000',0)";

                                    var bank_cd = "";
                                    switch (VAccountPoolList.First().VBankName)
                                    {

                                        case "기업":
                                            bank_cd = "003";
                                            break;
                                        case "국민":
                                            bank_cd = "004";
                                            break;
                                        case "농협":
                                            bank_cd = "011";
                                            break;
                                        case "우리":
                                            bank_cd = "020";
                                            break;
                                        case "신한":
                                            bank_cd = "088";
                                            break;
                                        default:
                                            bank_cd = "004";
                                            break;
                                    }

                                    _Command.Parameters.AddWithValue("@bank_cd", bank_cd);
                                    _Command.Parameters.AddWithValue("@acct_no", VAccountPoolList.First().VAccount);

                                    _Command.ExecuteNonQuery();

                                }


                            });
                        }
                    }


                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "회원사", 1), "회원사정보 수정 성공");

                    if (dataGridView1.RowCount > 1)
                    {
                        GridIndex = clientsBindingSource.Position;
                        btn_Search_Click(null, null);
                        dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                    }
                    else
                    {
                        btn_Search_Click(null, null);
                    }
                }
                else if (Status == "Delete")
                {

                    var _TradeCount = 0;
                    var _DriverCnt = 0;
                    var _CustomersCnt = 0;
                    var _OrdersCnt = 0;
                    var _DriverGroupsCnt = 0;
                    var _SalesManageCnt = 0;
                    var _ClientUserCnt = 0;
                    var _DriverAddCnt = 0;
                    if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
                    {
                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*) FROM trades WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _TradeCount = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*)  FROM drivers WHERE CandidateId = @ClientId AND ServiceState != 5";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _DriverCnt = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*)  FROM Customers WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _CustomersCnt = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*)  FROM Orders WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _OrdersCnt = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*)  FROM DriverGroups WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _DriverGroupsCnt = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*)  FROM SalesManage WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _SalesManageCnt = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*)  FROM ClientUsers WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _ClientUserCnt = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT COUNT(*)  FROM DriverAdd WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _DriverAddCnt = _Reader.GetInt32(0);

                                    }
                                }
                            }
                        });



                    }
                    if (_TradeCount > 0 || _DriverCnt > 0 || _CustomersCnt > 0 || _OrdersCnt > 0 || _DriverGroupsCnt > 0 || _SalesManageCnt > 0 || _ClientUserCnt > 0 || _DriverAddCnt > 0)
                    {
                        MessageBox.Show("사용중인 데이터가 존재하므로\n이 이 회원사는 삭제할 수 없습니다.", "회원사 삭제 실패");


                    }
                    else
                    {
                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _PCommand = _Connection.CreateCommand())
                            {
                                _PCommand.CommandText =
                                @"DELETE ClientPoints WHERE ClientId = @ClientId";
                                _PCommand.Parameters.AddWithValue("@ClientId", Row.ClientId);
                                _PCommand.ExecuteNonQuery();
                            }


                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                @"DELETE Clients WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                                _Command.ExecuteNonQuery();
                            }
                        });

                        MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "회원사", 1), "회원사 삭제 성공");
                       
                    }


                    btn_Search_Click(null, null);


                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "회원사정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }
        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문2, txt_Name.Text, 0), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;


                

                UpdateDB("Delete");
            }
            //MessageBox.Show("현제 회원삭제 기능은 제공되지 않고 있습니다. 대신 상태를 [보류]로 설정하고 저장 버튼을 눌려주십시오.", "카드페이", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //return;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void cmb_AddressState_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void btn_New_Click(object sender, EventArgs e)
        {

            FrmMN0204_CARGOOWNERMANAGE_Add _Form = new FrmMN0204_CARGOOWNERMANAGE_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();

            btn_Search_Click(null, null);

        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            cmb_BizTypeI.SelectedIndex = 0;
            cmb_statusI.SelectedIndex = 0;
            _Search();
        }

        private void clientsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            label11.Text = string.Empty;
            if (clientsBindingSource.Current == null)
                return;
            ClientsBindingSource_CurrentChanging = true;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                cmb_Admin.SelectedValue = Selected.DealerId;
                cmb_DriverType.SelectedValue = Selected.DriverType;
                cmb_ContType.SelectedValue = Selected.ContType;
                cmb_OrderType.SelectedValue = Selected.OrderType;

                cmb_AllowFPIS_In.SelectedValue = Selected.AllowFPIS_In;
                cmb_AllowSMS.SelectedValue = Selected.AllowSMS;
                cmbAllowOrder.SelectedValue = Selected.AllowOrder;
                cmbAllowFPIS.SelectedValue = Selected.AllowFPIS;
                cmbAllowSub.SelectedValue = Selected.AllowSub;
                cmbAllowMultiCustomer.SelectedValue = Selected.AllowMultiCustomer;
                cmbHasPoint.SelectedValue = Selected.HasPoint;
                cmbNeedCustomerId.SelectedValue = Selected.NeedCustomerId;
                cmbIsHolderShop.SelectedValue = Selected.IsHolderShop;
                cmbHideCustomer.SelectedValue = Selected.HideCustomer;
                cmbHideAddtrade.SelectedValue = Selected.HideAddTrade;
                cmbHideAddSales.SelectedValue = Selected.HideAddSales;
                cmbSignLocation.SelectedValue = Selected.SignLocation;
                cmbKakaoPrice.SelectedValue = Selected.KakaoPriceGubun;

                cmbEtaxGubun.SelectedValue = Selected.EtaxGubun;



                if (Selected.SmsYn == true)
                {
                    chkSms.Checked = true;

                }
                else
                {
                    chkSms.Checked = false;
                }
                if (Selected.AllowTax == false)
                {
                    rdb_Tax0.Checked = true;
                }
                else
                {
                    rdb_Tax1.Checked = true;

                }
                //사용한다
                if (Selected.HideAddTrade == true)
                {
                    chkSms.Checked = true;
                    chkSms.Enabled = false;
                }
                else
                {
                    chkSms.Enabled = true;
                }

                if(Selected.CargoApiYn == true)
                {
                    rdoCargoApiY.Checked = true;
                }
                else
                {
                    rdoCargoApiN.Checked = true;
                }

                if (!string.IsNullOrEmpty(Selected.userid) && !string.IsNullOrEmpty(Selected.passwd))
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }

                if (Selected.CarBankYn == true)
                {
                    rdoCarBankY.Checked = true;
                }
                else
                {
                    rdoCarBankN.Checked = true;
                }




                ChildBizNo.Clear();
                ChildCode.Tag = null;
                ChildCode.Clear();
                ChildName.Clear();
                ChildCeo.Clear();

                _FillChildDataView();
            }

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                if (tabControl1.SelectedTab == ImagePage || tabControl1.SelectedTab == PointPage)
                {
                    tabControl1.SelectedTab = BasePage;
                }
            }
            ClientsBindingSource_CurrentChanging = false;
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void cmb_AddressCity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //cmb_AddressStreet.Enabled = true;


            //var CarStreetDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Where(c => c.City == cmb_AddressCity.SelectedValue.ToString()).Where(c => c.Street != "").Select(c => new { c.Street }).Distinct().ToArray();
            //cmb_AddressStreet.DataSource = CarStreetDataSource;
            //cmb_AddressStreet.DisplayMember = "Street";
            //cmb_AddressStreet.ValueMember = "Street";
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == dataGridViewTextBoxColumn3)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.ClientsRow;
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == Selected.Status && c.Div == "Status");

                if (Query.Any())
                {

                    e.Value = Query.First().Name;
                }
                else
                {
                    e.Value = " ";
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == dataGridViewTextBoxColumn1)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.ClientsRow;
                e.Value = Selected.AddressState + " " + Selected.AddressCity + " " + Selected.AddressDetail;
            }

            else if (dataGridView1.Columns[e.ColumnIndex] == adminDataGridViewTextBoxColumn)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                }

                else
                {
                    var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.ClientsRow;

                    var Query = _DealerTable.Where(c => c.DealerId == Selected.DealerId).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().Name;
                    }
                    else { e.Value = " "; }
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == bizTypeDataGridViewTextBoxColumn)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                }
            }

        }

        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;


        private void btn_Excel_Click(object sender, EventArgs e)
        {
            //fileString = string.Format("회원정보EXCEL");
            //title = "회원정보";

            //ieExcel = Properties.Resources.회원정보1;





            //if (dataGridView1.RowCount == 0)
            //{
            //    MessageBox.Show("내보낼 회원정보 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            //if (dataGridView1.RowCount > 0)
            //{
            //    pnProgress.Visible = true;
            //    bar.Value = 0;
            //    Thread t = new Thread(new ThreadStart(() =>
            //    {
            //        dataGridView1.ExportExistExcel2_xlsx(title, fileString, bar, true, ieExcel, 2, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            //        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

            //        //mycalltruck.Admin.Class.Extensions.Excel_Epplus.Ep_FileExport(dataGridView1, title, fileString, bar, true, ieExcel, 2);
            //        //pnProgress.Invoke(new Action(() => pnProgress.Visible = false));


            //    }));
            //    t.SetApartmentState(ApartmentState.STA);
            //    t.Start();
            //}
            //else
            //{
            //    MessageBox.Show("엑셀로 내보낼 회원정보 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
            //        Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            ExcelExport();

        }
        private void ExcelExport()
        {
            int Sstatus = (int)cmb_statusI.SelectedValue;

            int Sbiztype = (int)cmb_BizTypeI.SelectedValue;


            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {

                di.Create();
            }
            var fileString = "회원정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }


            List<ExcelModel> _List = new List<ExcelModel>();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                //  using (SqlCommand _ExtraCommand = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT
                                a.Code,a.Name,a.Status,a.BizNo,a.Ceo,a.PGGubun,a.LGD_MID,a.Email,a.LoginId,a.Password,a.PhoneNo,a.MobileNo,Convert(varchar,a.CreateDate,111) as CreateDate,a.BizType,b.Name,a.Zipcode,a.AddressState +' '+ a.AddressCity + ' ' +a. AddressDetail as Address


                        FROM     Clients AS a join Dealers as b  ON a.Dealerid = b.DealerId
                        WHERE  (a.BizType = @BizType OR
                       @BizType = 0) AND (a.Status = @Status OR
                       @Status = 0) AND (@SearchType = 0) OR
                       (@SearchType = 1) AND (a.Code LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 2) AND (a.Name LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 3) AND (REPLACE(a.BizNo, '-', '') LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 4) AND (a.LoginId LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 5) AND (a.CEO LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 6) AND (ISNULL(a.LGD_MID, N'') LIKE '%' + @SearchText + '%')
                        ORDER BY a.ClientId DESC";
                    _Command.Parameters.AddWithValue("@BizType", Sbiztype);
                    _Command.Parameters.AddWithValue("@Status", Sstatus);

                    _Command.Parameters.AddWithValue("@SearchType", cmb_Search.SelectedIndex.ToString());
                    _Command.Parameters.AddWithValue("@SearchText", txt_Search.Text);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        _List.AddRange(_GetDataForExcelExport(_Reader));
                    }

                }
                _Connection.Close();
            }
            if (_List.Count == 0)
            {
                MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.ClientList))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            for (int i = 0; i < _List.Count; i++)
            {
                var _Model = _List[i];
                _Sheet.Cells[RowIndex, 1].Value = (_List.Count - i).ToString();
                _Sheet.Cells[RowIndex, 2].Value = _Model.SCdoe;
                _Sheet.Cells[RowIndex, 3].Value = _Model.SName;
                _Sheet.Cells[RowIndex, 4].Value = _Model.SSerivice;
                _Sheet.Cells[RowIndex, 5].Value = _Model.SBizNo;
                _Sheet.Cells[RowIndex, 6].Value = _Model.SCeo;
                _Sheet.Cells[RowIndex, 7].Value = _Model.SPG;
                _Sheet.Cells[RowIndex, 8].Value = _Model.SPgId;
                _Sheet.Cells[RowIndex, 9].Value = _Model.SEmail;
                _Sheet.Cells[RowIndex, 10].Value = _Model.SLoginId;
                _Sheet.Cells[RowIndex, 11].Value = _Model.SPassWord;
                _Sheet.Cells[RowIndex, 12].Value = _Model.SMobileNo;
                _Sheet.Cells[RowIndex, 13].Value = _Model.SPhoneNo;
                _Sheet.Cells[RowIndex, 14].Value = _Model.SCreateDate;
                _Sheet.Cells[RowIndex, 15].Value = _Model.SBizGubun;
                _Sheet.Cells[RowIndex, 16].Value = _Model.SDealerName;
                _Sheet.Cells[RowIndex, 17].Value = _Model.SZipCode;
                _Sheet.Cells[RowIndex, 18].Value = _Model.SAddress;

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
        private List<ExcelModel> _GetDataForExcelExport(SqlDataReader _Reader)
        {
            List<ExcelModel> r = new List<ExcelModel>();
            while (_Reader.Read())
            {
                //SingleDataSet.Instance.StaticOptions
                r.Add(new ExcelModel
                {
                    // S_Idx = _Reader.GetStringN(0),
                    SCdoe = _Reader.GetStringN(0),
                    SName = _Reader.GetStringN(1),
                    SSerivice = GetTextFromStaticOption("Status", _Reader.GetInt32Z(2)),
                    SBizNo = _Reader.GetStringN(3),
                    SCeo = _Reader.GetStringN(4),
                    SPG = GetTextFromStaticOption("PGgubun", _Reader.GetInt32Z(5)),
                    SPgId = _Reader.GetStringN(6),
                    SEmail = _Reader.GetStringN(7),
                    SLoginId = _Reader.GetStringN(8),
                    SPassWord = _Reader.GetStringN(9),
                    SMobileNo = _Reader.GetStringN(10),
                    SPhoneNo = _Reader.GetStringN(11),
                    SCreateDate = _Reader.GetStringN(12),
                    SBizGubun = GetTextFromStaticOption("ClientType", _Reader.GetInt32Z(13)),
                    SDealerName = _Reader.GetStringN(14),
                    SZipCode = _Reader.GetStringN(15),
                    SAddress = _Reader.GetStringN(16),

                });
            }
            return r;
        }
        private String GetTextFromStaticOption(String Div, int Value)
        {
            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == Div && c.Value == Value))
            {
                return SingleDataSet.Instance.StaticOptions.First(c => c.Div == Div && c.Value == Value).Name;
            }
            else
            {
                return "";
            }
        }

        class ExcelModel : INotifyPropertyChanged
        {
            private String _S_Idx = "";
            private String _SCdoe = "";
            private String _SName = "";
            private String _SSerivice = "";
            private String _SBizNo = "";
            private String _SCeo = "";
            private String _SPG = "";
            private String _SPgId = "";
            private String _SEmail = "";
            private String _SLoginId = "";
            private String _SPassWord = "";
            private String _SMobileNo = "";
            private String _SPhoneNo = "";
            private String _SCreateDate = "";
            private String _SBizGubun = "";
            private String _SDealerName = "";
            private String _SZipCode = "";
            private String _SAddress = "";

            public string S_Idx
            {
                get
                {
                    return _S_Idx;
                }

                set
                {
                    SetField(ref _S_Idx, value);
                }
            }

            public string SCdoe
            {
                get
                {
                    return _SCdoe;
                }

                set
                {
                    SetField(ref _SCdoe, value);
                }
            }

            public string SName
            {
                get
                {
                    return _SName;
                }

                set
                {
                    SetField(ref _SName, value);
                }
            }

            public string SSerivice
            {
                get
                {
                    return _SSerivice;
                }

                set
                {
                    SetField(ref _SSerivice, value);
                }
            }

            public string SBizNo
            {
                get
                {
                    return _SBizNo;
                }

                set
                {
                    SetField(ref _SBizNo, value);
                }
            }

            public string SCeo
            {
                get
                {
                    return _SCeo;
                }

                set
                {
                    SetField(ref _SCeo, value);
                }
            }

            public string SPG
            {
                get
                {
                    return _SPG;
                }

                set
                {
                    SetField(ref _SPG, value);
                }
            }
            public string SPgId
            {
                get
                {
                    return _SPgId;
                }

                set
                {
                    SetField(ref _SPgId, value);
                }
            }

            public string SEmail
            {
                get
                {
                    return _SEmail;
                }

                set
                {
                    SetField(ref _SEmail, value);
                }
            }


            public string SLoginId
            {
                get
                {
                    return _SLoginId;
                }

                set
                {
                    SetField(ref _SLoginId, value);
                }
            }
            public string SPassWord
            {
                get
                {
                    return _SPassWord;
                }

                set
                {
                    SetField(ref _SPassWord, value);
                }
            }
            public string SMobileNo
            {
                get
                {
                    return _SMobileNo;
                }

                set
                {
                    SetField(ref _SMobileNo, value);
                }
            }
            public string SPhoneNo
            {
                get
                {
                    return _SPhoneNo;
                }

                set
                {
                    SetField(ref _SPhoneNo, value);
                }
            }

            public string SCreateDate
            {
                get
                {
                    return _SCreateDate;
                }

                set
                {
                    SetField(ref _SCreateDate, value);
                }
            }






            public string SBizGubun
            {
                get
                {
                    return _SBizGubun;
                }

                set
                {
                    SetField(ref _SBizGubun, value);
                }
            }

            public string SDealerName
            {
                get
                {
                    return _SDealerName;
                }

                set
                {
                    SetField(ref _SDealerName, value);
                }
            }
            public string SZipCode
            {
                get
                {
                    return _SZipCode;
                }

                set
                {
                    SetField(ref _SZipCode, value);
                }
            }
            public string SAddress
            {
                get
                {
                    return _SAddress;
                }

                set
                {
                    SetField(ref _SAddress, value);
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

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                txt_Zip.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_State.Text = ss[0];
                txt_City.Text = ss[1];
                txt_Street.Text = String.Join(" ", ss.Skip(2));
            }
        }


        void ContTypeChanged(object sender, EventArgs e)
        {
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.ContType == (int)cmb_ContType.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET ContType = @ContType" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@ContType", int.Parse(cmb_ContType.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.ContType = (int)cmb_ContType.SelectedValue;
        }

        void DriverTypeChanged(object sender, EventArgs e)
        {
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.DriverType == (int)cmb_DriverType.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET DriverType = @DriverType" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@DriverType", int.Parse(cmb_DriverType.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.DriverType = (int)cmb_DriverType.SelectedValue;
        }

        void OrderTypeChanged(object sender, EventArgs e)
        {
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.OrderType == (int)cmb_OrderType.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET OrderType = @OrderType" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@OrderType", int.Parse(cmb_OrderType.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.OrderType = (int)cmb_OrderType.SelectedValue;
        }
        private void cmb_ContType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContTypeChanged(null, null);
        }

        private void cmb_DriverType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriverTypeChanged(null, null);
        }

        private void cmb_OrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            OrderTypeChanged(null, null);
        }

        private void cmb_AllowFPIS_In_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmb_AllowFPIS_In.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowFPIS_In == (bool)cmb_AllowFPIS_In.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowFPIS_In = @AllowFPIS_In" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@AllowFPIS_In", (bool)cmb_AllowFPIS_In.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.AllowFPIS_In = (bool)cmb_AllowFPIS_In.SelectedValue;
        }

        private void cmb_AllowSMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmb_AllowSMS.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowSMS == (bool)cmb_AllowSMS.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowSMS = @AllowSMS" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@AllowSMS", (bool)cmb_AllowSMS.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.AllowSMS = (bool)cmb_AllowSMS.SelectedValue;
        }

        private void cmbAllowOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbAllowOrder.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowOrder == (bool)cmbAllowOrder.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowOrder = @AllowOrder" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@AllowOrder", (bool)cmbAllowOrder.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.AllowOrder = (bool)cmbAllowOrder.SelectedValue;
        }

        private void cmbAllowFPIS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbAllowFPIS.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowFPIS == (bool)cmbAllowFPIS.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowFPIS = @AllowFPIS" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@AllowFPIS", (bool)cmbAllowFPIS.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.AllowFPIS = (bool)cmbAllowFPIS.SelectedValue;
        }

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            txt_MobileNo.Text = txt_MobileNo.Text.Replace(",", "");
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_MobileNo.Text))
            {
                var _S = txt_MobileNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 7)
                {
                    _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                }
                if (_S.Length > 12)
                {
                    _S = _S.Replace("-", "");
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                }
                txt_MobileNo.Text = _S;
            }
        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            txt_PhoneNo.Text = txt_PhoneNo.Text.Replace(",", "");
        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_PhoneNo.Text))
            {
                var _S = txt_PhoneNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("1"))
                {
                    if (_S.Length > 4)
                    {
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
                    }
                    else if (_S.Length > 8)
                    {
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                    }
                }
                else if (_S.StartsWith("02"))
                {
                    if (_S.Length > 2)
                    {
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                    }
                    if (_S.Length > 6)
                    {
                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                    }
                    if (_S.Length > 11)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                    }
                }
                else
                {
                    if (_S.Length > 3)
                    {
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                    }
                    if (_S.Length > 7)
                    {
                        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                txt_PhoneNo.Text = _S;
            }
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void StampUpload_Click(object sender, EventArgs e)
        {

        }

        private void StampDelete_Click(object sender, EventArgs e)
        {

        }

        private void StampView_Click(object sender, EventArgs e)
        {

        }

        private void cmbAllowSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbAllowSub.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowSub == (bool)cmbAllowSub.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowSub = @AllowSub" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@AllowSub", (bool)cmbAllowSub.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.AllowSub = (bool)cmbAllowSub.SelectedValue;
        }

        private void cmbAllowMultiCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmb_AllowSMS.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowMultiCustomer == (bool)cmbAllowMultiCustomer.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowMultiCustomer = @AllowMultiCustomer" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@AllowMultiCustomer", (bool)cmbAllowMultiCustomer.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.AllowMultiCustomer = (bool)cmbAllowMultiCustomer.SelectedValue;
        }

        private void cmbHasPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbHasPoint.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.HasPoint == (bool)cmbHasPoint.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET HasPoint = @HasPoint" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@HasPoint", (bool)cmbHasPoint.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.HasPoint = (bool)cmbHasPoint.SelectedValue;
        }

        private void cmbNeedCustomerId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmb_AllowSMS.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.NeedCustomerId == (bool)cmbNeedCustomerId.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET NeedCustomerId = @NeedCustomerId" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@NeedCustomerId", (bool)cmbNeedCustomerId.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.NeedCustomerId = (bool)cmbNeedCustomerId.SelectedValue;
        }

        private void cmbIsHolderShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbIsHolderShop.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.IsHolderShop == (bool)cmbIsHolderShop.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET IsHolderShop = @IsHolderShop" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@IsHolderShop", (bool)cmbIsHolderShop.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.IsHolderShop = (bool)cmbIsHolderShop.SelectedValue;
        }

        private void RegisterDriver_Click(object sender, EventArgs e)
        {
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                MessageBox.Show("관리자 계정으로는 할 수 없는 동작입니다. 해당 회원의 계정으로 로그인 한 후 진행하여주십시오.");
                return;
            }
            DriverRepository mDriverRepository = new DriverRepository();
            if (mDriverRepository.HasSelf())
            {
                MessageBox.Show("이미 등록되었습니다.");
                return;
            }
            if (clientsBindingSource.Current == null || cmbIsHolderShop.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (String.IsNullOrEmpty(cmb_Bank.SelectedValue?.ToString()) || String.IsNullOrEmpty(txt_AccountNo.Text) || String.IsNullOrEmpty(txt_AccountOwner.Text))
            {
                MessageBox.Show("먼저 계좌정보를 등록하여 주십시오.");
                return;
            }
            string Mid = "edubill";
            string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_Bank.SelectedValue.ToString(), txt_AccountNo.Text, txt_AccountOwner.Text, cmb_Bank.Text, Mid);
            WebClient mWebClient = new WebClient();
            try
            {
                var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                if (r == null || r.ToLower() != "true")
                {
                    MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();

                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                       "UPDATE Clients SET CMSOwner = @CMSOwner, CMSBankName = @CMSBankName, CMSBankCode = @CMSBankCode, CMSAccountNo = @CMSAccountNo" +
                       " WHERE Clientid = @ClientId";
                    _Command.Parameters.AddWithValue("@CMSOwner", txt_AccountOwner.Text);
                    _Command.Parameters.AddWithValue("@CMSBankName", cmb_Bank.Text);
                    _Command.Parameters.AddWithValue("@CMSBankCode", cmb_Bank.SelectedValue.ToString());
                    _Command.Parameters.AddWithValue("@CMSAccountNo", txt_AccountNo.Text);
                    _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                    _Command.ExecuteNonQuery();
                }
                _Connection.Close();
            }
            LocalUser.Instance.LogInInformation.LoadClient();
            mDriverRepository.CreateDriver_Self();
            MessageBox.Show("상점 등록이 되었습니다.");
        }

        private void btn_BizPaperAdd_Click(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            int DriverId = 0;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                DriverRepository mDriverRepository = new DriverRepository();
                if (!mDriverRepository.HasSelf())
                {
                    MessageBox.Show("LG 등록을 먼저 진행해주십시오.");
                    tabControl1.SelectedTab = BasePage;
                    return;
                }
                DriverId = mDriverRepository.GetDriverId_Self();

                dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (dlgOpen.ShowDialog() != DialogResult.OK) return;
                Bitmap _b = null;
                try
                {
                    var b = File.ReadAllBytes(dlgOpen.FileName);
                    MemoryStream ms = new MemoryStream();
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                try
                {
                    MemoryStream mm = new MemoryStream();
                    _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = new byte[mm.Length];
                    mm.Seek(0, SeekOrigin.Begin);
                    mm.Read(bytes, 0, bytes.Length);
                    mm.Close();

                    AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                    i.DriverId = DriverId;
                    i.ImageData64String = System.Convert.ToBase64String(bytes);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetBizPaper"));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string parsedContent = JsonConvert.SerializeObject(i);
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] b = encoding.GetBytes(parsedContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(b, 0, b.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                txt_BizPaper.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
                MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_BizPaperDelete_Click(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            int DriverId = 0;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                DriverRepository mDriverRepository = new DriverRepository();
                if (!mDriverRepository.HasSelf())
                {
                    MessageBox.Show("LG 등록을 먼저 진행해주십시오.");
                    tabControl1.SelectedTab = BasePage;
                    return;
                }
                DriverId = mDriverRepository.GetDriverId_Self();
                if (MessageBox.Show("정말 이미지를 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                try
                {
                    string sqlString = "UPDATE Drivers Set HasBizPaper = 0 WHERE DriverId = @DriverId";
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        SqlCommand updateCmd = new SqlCommand(sqlString, cn);
                        updateCmd.Parameters.Add(new SqlParameter("@DriverId", DriverId));
                        cn.Open();
                        updateCmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 삭제 하는 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                txt_BizPaper.Text = "";
            }
        }

        private void btn_BizPaperShow_Click(object sender, EventArgs e)
        {
            _ShowBizPaper();
        }

        private void pic_BizPaper_Click(object sender, EventArgs e)
        {
            _ShowBizPaper();
        }

        private void _ShowBizPaper()
        {
            if (clientsBindingSource.Current == null)
                return;
            int DriverId = 0;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                DriverRepository mDriverRepository = new DriverRepository();
                if (!mDriverRepository.HasSelf())
                {
                    MessageBox.Show("LG 등록을 먼저 진행해주십시오.");
                    tabControl1.SelectedTab = BasePage;
                    return;
                }
                DriverId = mDriverRepository.GetDriverId_Self();

                Bitmap _b = null;
                PrintDocument pDoc = new PrintDocument();
                PageSettings ps = new PageSettings();
                ps.Margins = new Margins(10, 10, 10, 10);
                pDoc.DefaultPageSettings = ps;
                PrintPreviewDialog ppDoc = new PrintPreviewDialog();

                ppDoc.ClientSize = new System.Drawing.Size(500, 500);
                ppDoc.UseAntiAlias = true;
                pDoc.PrintPage += new PrintPageEventHandler((_sender, _e) =>
                {
                    if (_b.Width > _b.Height)
                    {
                        _b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    var width = _b.Width;
                    var height = _b.Height;
                    if (_b.Width > _e.MarginBounds.Width)
                    {
                        width = _e.MarginBounds.Width;
                        height = height * _e.MarginBounds.Width / _b.Width;
                    }
                    else if (_b.Height > _e.MarginBounds.Height)
                    {
                        height = _e.MarginBounds.Height;
                        width = width * _e.MarginBounds.Height / _b.Height;
                    }
                    _e.Graphics.DrawImage(_b, 0, 0, width, height);

                });

                ppDoc.Document = pDoc;
                ((Form)ppDoc).WindowState = FormWindowState.Maximized;
                ((Form)ppDoc).TopMost = true;
                Task.Factory.StartNew(() =>
                {
                    WebClient mWebClient = new WebClient();
                    try
                    {
                        var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetBizPaper?DriverId=" + DriverId.ToString());
                        MemoryStream ms = new MemoryStream();
                        ms.Write(b, 0, b.Length);
                        ms.Position = 0;
                        _b = new Bitmap(ms);
                    }
                    catch (Exception)
                    {
                        Invoke(new Action(() => MessageBox.Show("이미지를 가져오는 중 오류가 발생하였습니다. 잠시 후 다시 이용바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                        return;
                    }
                    Invoke(new Action(() => ppDoc.Show()));
                });
            }
        }

        private void btn_CarPaperAdd_Click(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            int DriverId = 0;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                DriverRepository mDriverRepository = new DriverRepository();
                if (!mDriverRepository.HasSelf())
                {
                    MessageBox.Show("LG 등록을 먼저 진행해주십시오.");
                    tabControl1.SelectedTab = BasePage;
                    return;
                }
                DriverId = mDriverRepository.GetDriverId_Self();

                dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (dlgOpen.ShowDialog() != DialogResult.OK) return;
                Bitmap _b = null;
                try
                {
                    var b = File.ReadAllBytes(dlgOpen.FileName);
                    MemoryStream ms = new MemoryStream();
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                try
                {
                    MemoryStream mm = new MemoryStream();
                    _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = new byte[mm.Length];
                    mm.Seek(0, SeekOrigin.Begin);
                    mm.Read(bytes, 0, bytes.Length);
                    mm.Close();

                    AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                    i.DriverId = DriverId;
                    i.ImageData64String = System.Convert.ToBase64String(bytes);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetCarPaper"));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string parsedContent = JsonConvert.SerializeObject(i);
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] b = encoding.GetBytes(parsedContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(b, 0, b.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                txt_CarPaper.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
                MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_CarPaperDelete_Click(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            int DriverId = 0;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                DriverRepository mDriverRepository = new DriverRepository();
                if (!mDriverRepository.HasSelf())
                {
                    MessageBox.Show("LG 등록을 먼저 진행해주십시오.");
                    tabControl1.SelectedTab = BasePage;
                    return;
                }
                DriverId = mDriverRepository.GetDriverId_Self();

                if (MessageBox.Show("정말 이미지를 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                try
                {
                    string sqlString = "UPDATE Drivers Set HasCarPaper = 0 WHERE DriverId = @DriverId";
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        SqlCommand updateCmd = new SqlCommand(sqlString, cn);
                        updateCmd.Parameters.Add(new SqlParameter("@DriverId", DriverId));
                        cn.Open();
                        updateCmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 삭제 하는 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                txt_CarPaper.Text = "";
            }
        }

        private void btn_CarPaperShow_Click(object sender, EventArgs e)
        {
            _ShowCarPaper();
        }

        private void pic_CarPaper_Click(object sender, EventArgs e)
        {
            _ShowCarPaper();
        }

        private void _ShowCarPaper()
        {
            if (clientsBindingSource.Current == null)
                return;
            int DriverId = 0;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                DriverRepository mDriverRepository = new DriverRepository();
                if (!mDriverRepository.HasSelf())
                {
                    MessageBox.Show("LG 등록을 먼저 진행해주십시오.");
                    tabControl1.SelectedTab = BasePage;
                    return;
                }
                DriverId = mDriverRepository.GetDriverId_Self();

                Bitmap _b = null;
                PrintDocument pDoc = new PrintDocument();
                PageSettings ps = new PageSettings();
                ps.Margins = new Margins(10, 10, 10, 10);
                pDoc.DefaultPageSettings = ps;
                PrintPreviewDialog ppDoc = new PrintPreviewDialog();

                ppDoc.ClientSize = new System.Drawing.Size(500, 500);
                ppDoc.UseAntiAlias = true;
                pDoc.PrintPage += new PrintPageEventHandler((_sender, _e) =>
                {
                    if (_b.Width > _b.Height)
                    {
                        _b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    var width = _b.Width;
                    var height = _b.Height;
                    if (_b.Width > _e.MarginBounds.Width)
                    {
                        width = _e.MarginBounds.Width;
                        height = height * _e.MarginBounds.Width / _b.Width;
                    }
                    else if (_b.Height > _e.MarginBounds.Height)
                    {
                        height = _e.MarginBounds.Height;
                        width = width * _e.MarginBounds.Height / _b.Height;
                    }
                    _e.Graphics.DrawImage(_b, 0, 0, width, height);

                });

                ppDoc.Document = pDoc;
                ((Form)ppDoc).WindowState = FormWindowState.Maximized;
                ((Form)ppDoc).TopMost = true;
                Task.Factory.StartNew(() =>
                {
                    WebClient mWebClient = new WebClient();
                    try
                    {
                        var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarPaper?DriverId=" + DriverId.ToString());
                        MemoryStream ms = new MemoryStream();
                        ms.Write(b, 0, b.Length);
                        ms.Position = 0;
                        _b = new Bitmap(ms);
                    }
                    catch (Exception)
                    {
                        Invoke(new Action(() => MessageBox.Show("이미지를 가져오는 중 오류가 발생하였습니다. 잠시 후 다시 이용바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                        return;
                    }
                    Invoke(new Action(() => ppDoc.Show()));
                });
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = true;
            if (tabControl1.SelectedTab == ImagePage)
            {
                if (clientsBindingSource.Current == null)
                    return;
                if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
                {
                    DriverRepository mDriverRepository = new DriverRepository();
                    if (!mDriverRepository.HasSelf())
                    {
                        MessageBox.Show("LG 등록을 먼저 진행해주십시오.");
                        tabControl1.SelectedTab = BasePage;
                        return;
                    }
                    pic_BizPaper.Image = Properties.Resources.ic_photo_white_48dp_2x;
                    pic_CarPaper.Image = Properties.Resources.ic_photo_white_48dp_2x;
                    pic_BizPaper.Cursor = Cursors.Arrow;
                    pic_CarPaper.Cursor = Cursors.Arrow;
                    var DriverId = mDriverRepository.GetDriverId_Self();
                    if (mDriverRepository.HasBizPaper_Self())
                    {
                        using (WebClient mWebClient = new WebClient())
                        {
                            var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetBizPaper?DriverId=" + DriverId.ToString());
                            MemoryStream ms = new MemoryStream();
                            ms.Write(b, 0, b.Length);
                            ms.Position = 0;
                            pic_BizPaper.Image = new Bitmap(ms);
                        }
                        pic_BizPaper.Cursor = Cursors.Hand;
                    }
                    if (mDriverRepository.HasCarPaper_Self())
                    {
                        using (WebClient mWebClient = new WebClient())
                        {
                            var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarPaper?DriverId=" + DriverId.ToString());
                            MemoryStream ms = new MemoryStream();
                            ms.Write(b, 0, b.Length);
                            ms.Position = 0;
                            pic_CarPaper.Image = new Bitmap(ms);
                        }
                        pic_CarPaper.Cursor = Cursors.Hand;
                    }
                }
            }
            else if(tabControl1.SelectedTab == PointPage)
            {
                if (clientsBindingSource.Current == null)
                    return;
                if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
                {
                    PointDataSource = ShareOrderDataSet.ClientPoints.Where(c => c.ClientId == Selected.ClientId).OrderByDescending(c=>c.ClientPointId).ToList();
                    Point.Text = "0";
                    DataListPoint.AutoGenerateColumns = false;
                    DataListPoint.DataSource = PointDataSource;
                    if (PointDataSource.Count > 0)
                    {
                        Point.Text = PointDataSource.Sum(c => c.Amount).ToString("N0");
                    }
                    if (!String.IsNullOrEmpty(Selected.VAccount))
                    {
                        VAccountInfo.Text = $"가상계좌번호 : {Selected.VAccount} ({Selected.VBankName})";
                    }
                    else
                    {
                        VAccountInfo.Text = "";
                    }
                }
            }
            else if (tabControl1.SelectedTab == BankPage)
            {
                if (clientsBindingSource.Current == null)
                    return;

                btnUpdate.Enabled = false;
                if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
                {
                    if (!String.IsNullOrEmpty(cmb_Bank.SelectedValue?.ToString()) && !String.IsNullOrEmpty(txt_AccountNo.Text) && !String.IsNullOrEmpty(txt_AccountOwner.Text))
                    {
                        BankSave.Visible = false;
                        BankDelete.Visible = false;
                    }
                    else if (String.IsNullOrEmpty(txt_AccountNo.Text) && String.IsNullOrEmpty(txt_AccountOwner.Text))
                    {
                        BankSave.Visible = true;
                        BankDelete.Visible = true;

                    }

               
                }
            }

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }

        private void UpdateVAccount_Click(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                List<(int VAccountPoolId, String VBankName, String VAccount)> VAccountPoolList = new List<(int, string, string)>();
                Queue<int> DriverIdQueue = new Queue<int>();
                int DriverCount = 0;
                int VAccountCount = 0;
                Data.Connection((_Connection) =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT [VAccountPoolId], [VBankName], [VAccount] FROM [VAccountPools] WHERE [ClientId] IS NULL AND [DriverId] IS NULL";
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            while (_Reader.Read())
                            {
                                VAccountPoolList.Add((_Reader.GetInt32(0), _Reader.GetString(1), _Reader.GetString(2)));
                            }
                        }
                    }
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT [Drivers].[DriverId] FROM [Drivers] JOIN [DriverInstances] ON [Drivers].[DriverId] = [DriverInstances].[DriverId] WHERE [DriverInstances].[ClientId] = @ClientId AND [VAccount] IS NULL";
                        _Command.Parameters.AddWithValue("@ClientId", Selected.ClientId);
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            while (_Reader.Read())
                            {
                                DriverIdQueue.Enqueue(_Reader.GetInt32(0));
                            }
                        }
                    }
                    DriverCount = DriverIdQueue.Count;
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "UPDATE Drivers SET [VBankName] = @VBankName, [VAccount] = @VAccount WHERE DriverId = @DriverId;UPDATE VAccountPools SET DriverId = @DriverId WHERE VAccountPoolId = @VAccountPoolId;";
                        _Command.Parameters.Add(new SqlParameter("@VBankName", SqlDbType.NVarChar));
                        _Command.Parameters.Add(new SqlParameter("@VAccount", SqlDbType.NVarChar));
                        _Command.Parameters.Add(new SqlParameter("@DriverId", SqlDbType.Int));
                        _Command.Parameters.Add(new SqlParameter("@VAccountPoolId", SqlDbType.Int));
                        foreach (var VAccountPool in VAccountPoolList)
                        {
                            if (DriverIdQueue.Count == 0)
                                break;
                            int DriverId = DriverIdQueue.Dequeue();
                            _Command.Parameters["@VBankName"].Value = VAccountPool.VBankName;
                            _Command.Parameters["@VAccount"].Value = VAccountPool.VAccount;
                            _Command.Parameters["@DriverId"].Value = DriverId;
                            _Command.Parameters["@VAccountPoolId"].Value = VAccountPool.VAccountPoolId;
                            _Command.ExecuteNonQuery();
                            VAccountCount++;
                        }
                    }
                });
                if (DriverCount == VAccountCount)
                {
                    MessageBox.Show($"대상 기사 {DriverCount}건 중 {VAccountCount}건의 기사항목에 가상계좌가 부여되었습니다.", "차세로");
                }
                else
                {
                    MessageBox.Show($"대상 기사 {DriverCount}건 중 {VAccountCount}건의 기사항목에 가상계좌가 부여되었습니다. 가상계좌가 충분하지 않습니다. 확인 부탁드립니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChildBizNoSearch_Click(object sender, EventArgs e)
        {
            ChildCode.Tag = null;
            ChildCode.Clear();
            ChildName.Clear();
            ChildCeo.Clear();
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT ClientId, Code, Name, CEO FROM Clients WHERE REPLACE(BizNo, '-','') = @BizNo";
                        _Command.Parameters.AddWithValue("@BizNo", ChildBizNo.Text);
                        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                ChildCode.Tag = _Reader.GetInt32(0);
                                ChildCode.Text = _Reader.GetString(1);
                                ChildName.Text = _Reader.GetString(2);
                                ChildCeo.Text = _Reader.GetString(3);
                            }
                        }
                    }
                });

            }
        }

        private void ChildAdd_Click(object sender, EventArgs e)
        {
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                if(ChildCode.Tag != null)
                {
                    int ChildClientId = (int)ChildCode.Tag;
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "SELECT ClientInstanceId FROM ClientInstances WHERE ParentClientId = @ParentClientId AND ChildClientId = @ChildClientId";
                            _Command.Parameters.AddWithValue("@ParentClientId", Selected.ClientId);
                            _Command.Parameters.AddWithValue("@ChildClientId", ChildClientId);
                            using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                if (_Reader.Read())
                                {
                                    MessageBox.Show("그룹에는 같은 회원이 포함될 수 없습니다. 확인 부탁드립니다.","회원정보관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                            }
                        }
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "INSERT INTO ClientInstances (ParentClientId, ChildClientId, CreateTime) VALUES (@ParentClientId, @ChildClientId, GETDATE())";
                            _Command.Parameters.AddWithValue("@ParentClientId", Selected.ClientId);
                            _Command.Parameters.AddWithValue("@ChildClientId", ChildClientId);
                            _Command.ExecuteNonQuery();
                        }
                    });

                    ChildBizNo.Clear();
                    ChildCode.Tag = null;
                    ChildCode.Clear();
                    ChildName.Clear();
                    ChildCeo.Clear();

                    _FillChildDataView();
                }
            }
        }

        private void ChildDelete_Click(object sender, EventArgs e)
        {
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                if (ChildDataView.SelectedRows.Count == 0)
                    return;
                int ClientInstanceId = (int) ChildDataView.SelectedRows[0].Tag;
                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "DELETE FROM ClientInstances WHERE ClientInstanceId = @ClientInstanceId";
                        _Command.Parameters.AddWithValue("@ClientInstanceId", ClientInstanceId);
                        _Command.ExecuteNonQuery();
                    }
                });
                _FillChildDataView();
            }
        }

        private void ChildDataView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ChildColumnNo.Index)
            {
                e.Value = (ChildDataView.RowCount - e.RowIndex).ToString("N0");
            }
        }

        private void _FillChildDataView()
        {
            ChildDataView.Rows.Clear();
            if (((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected)
            {
                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT ClientInstanceId, BizNo, Code, Name, CEO FROM ClientInstances JOIN Clients ON ClientInstances.ChildClientId = Clients.ClientId WHERE ClientInstances.ParentClientId = @ParentClientId";
                        _Command.Parameters.AddWithValue("@ParentClientId", Selected.ClientId);
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            while (_Reader.Read())
                            {
                                var newRow = ChildDataView.Rows[ChildDataView.Rows.Add()];
                                newRow.Tag = _Reader.GetInt32(0);
                                newRow.Cells[1].Value = _Reader.GetString(1);
                                newRow.Cells[2].Value = _Reader.GetString(2);
                                newRow.Cells[3].Value = _Reader.GetString(3);
                                newRow.Cells[4].Value = _Reader.GetString(4);
                            }
                        }
                    }
                });
            }
        }

        private void ChildBizNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void DataListPoint_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnPointNo.Index)
                e.Value = DataListPoint.RowCount - e.RowIndex;
            else if (e.ColumnIndex == ColumnSum.Index)
            {
                e.Value = PointDataSource.Skip(e.RowIndex).Sum(c => c.Amount);
            }

        }

        private void ChangePoint_Click(object sender, EventArgs e)
        {
            if(clientsBindingSource.Current != null && ((DataRowView)clientsBindingSource.Current).Row is ClientDataSet.ClientsRow Selected && DataListPoint.DataSource != null && DataListPoint.DataSource is List<ClientPoint> PointDataSource)
            {
                var Amount = Decimal.Parse(Point.Text.Replace(",", "")) - PointDataSource.Sum(c => c.Amount);
                ShareOrderDataSet.ClientPoints.Add(new ClientPoint
                {
                    ClientId = Selected.ClientId,
                    Amount = Amount,
                    CDate = DateTime.Now,
                    MethodType = "관리자 변경",
                    Remark = Point.Text,
                });
                ShareOrderDataSet.SaveChanges();
                PointDataSource = ShareOrderDataSet.ClientPoints.Where(c => c.ClientId == Selected.ClientId).OrderByDescending(c => c.ClientPointId).ToList();
                Point.Text = "0";
                DataListPoint.AutoGenerateColumns = false;
                DataListPoint.DataSource = PointDataSource;
                if (PointDataSource.Count > 0)
                {
                    Point.Text = PointDataSource.Sum(c => c.Amount).ToString("N0");
                }
            }
        }

        private void Point_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void Point_Enter(object sender, EventArgs e)
        {
            Point.Text = Point.Text.Replace(",", "");
        }

        private void Point_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Point.Text))
                Point.Text = "0";
            else
                Point.Text = Decimal.Parse(Point.Text).ToString("N0");
        }


        private void cmbHideCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbHideCustomer.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.HideCustomer == (bool)cmbHideCustomer.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET HideCustomer = @HideCustomer" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@HideCustomer", (bool)cmbHideCustomer.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.HideCustomer = (bool)cmbHideCustomer.SelectedValue;
        }

        private void cmb_ExcelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmb_ExcelType.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.ExcelType == (int)cmb_ExcelType.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET ExcelType = @ExcelType" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@ExcelType", (int)cmb_ExcelType.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.ExcelType = (int)cmb_ExcelType.SelectedValue;

        }

        private void cmb_NoticeDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmb_NoticeDriver.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.NoticeDriver == (int)cmb_NoticeDriver.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET NoticeDriver = @NoticeDriver" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@NoticeDriver", (int)cmb_NoticeDriver.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.NoticeDriver = (int)cmb_NoticeDriver.SelectedValue;

        }

        private void NoticeCntUpdate_Click(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.NoticeCnt == (int)NoticeCnt.Value)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET NoticeCnt = @NoticeCnt" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@NoticeCnt", (int)NoticeCnt.Value);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.NoticeCnt = (int)NoticeCnt.Value;
        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowTax == false)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowTax = 0" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            if (rdb_Tax0.Checked)
            {
                Row.AllowTax = false;
            }
            else
            {
                Row.AllowTax = true;
            }
        }

        private void rdb_Tax1_CheckedChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.AllowTax == true)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET AllowTax = 1" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            if (rdb_Tax0.Checked)
            {
                Row.AllowTax = false;
            }
            else
            {
                Row.AllowTax = true;
            }
        }

        private void cmbHideAddtrade_SelectedIndexChanged(object sender, EventArgs e)
        {
           


            if (clientsBindingSource.Current == null || cmbHideAddtrade.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.HideAddTrade == (bool)cmbHideAddtrade.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET HideAddTrade = @HideAddTrade" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@HideAddTrade", (bool)cmbHideAddtrade.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.HideAddTrade = (bool)cmbHideAddtrade.SelectedValue;
            //사용한다
            if (cmbHideAddtrade.SelectedIndex == 0)
            {
                chkSms.Checked = true;
                chkSms.Enabled = false;
            }
            else
            {
                chkSms.Checked = false;
                chkSms.Enabled = true;
            }

        }

        private void cmbHideAddSales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbHideAddSales.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.HideAddSales == (bool)cmbHideAddSales.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET HideAddSales = @HideAddSales" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@HideAddSales", (bool)cmbHideAddSales.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.HideAddSales = (bool)cmbHideAddSales.SelectedValue;
        }

        private void cmbSignLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null || cmbSignLocation.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.SignLocation == cmbSignLocation.SelectedValue.ToString())
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET SignLocation = @SignLocation" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@SignLocation", cmbSignLocation.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.SignLocation = cmbSignLocation.SelectedValue.ToString();
        }
        private void BankSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmb_Bank.SelectedValue?.ToString()) && !String.IsNullOrEmpty(txt_AccountNo.Text) && !String.IsNullOrEmpty(txt_AccountOwner.Text))
            {
                string Mid = "edubill";
                string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_Bank.SelectedValue.ToString(), txt_AccountNo.Text, txt_AccountOwner.Text, cmb_Bank.Text, Mid);
                WebClient mWebClient = new WebClient();
                try
                {

                    var _Result = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));


                    _Result = HttpUtility.UrlDecode(_Result);
                    if (_Result == "오류")
                    {
                        throw new Exception("올바른 계좌가 아닙니다.");
                    }
                    else
                    {
                        Account_Name = _Result;
                        txt_AccountOwner.Text = Account_Name;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    cmb_Bank.SelectedIndex = 0;
                    txt_AccountNo.Text = "";
                    txt_AccountOwner.Text = "";
                    return;
                }
            }
            clientsBindingSource.EndEdit();
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            Row.Admin = cmb_Admin.Text;

            Row.CMSBankName = cmb_Bank.Text;

            try
            {

                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName = @CMSBankName, CMSBankCode = @CMSBankCode, CMSAccountNo = @CMSAccountNo , CMSOwner = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", Row.CMSBankName);
                        _Command.Parameters.AddWithValue("@CMSBankCode", Row.CMSBankCode);
                        _Command.Parameters.AddWithValue("@CMSAccountNo", Row.CMSAccountNo);
                        _Command.Parameters.AddWithValue("@CMSOwner", Row.CMSOwner);


                        _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("저장 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {

            }
        }
        private void BankSave1_Click(object sender, EventArgs e)
        {
            

            if (!String.IsNullOrEmpty(cmb_Bank1.SelectedValue?.ToString()) && !String.IsNullOrEmpty(txt_AccountNo1.Text) && !String.IsNullOrEmpty(txt_AccountOwner1.Text))
            {
                string Mid = "edubill";
                string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_Bank1.SelectedValue.ToString(), txt_AccountNo1.Text, txt_AccountOwner1.Text, cmb_Bank1.Text, Mid);
                WebClient mWebClient = new WebClient();
                try
                {

                    var _Result = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));


                    _Result = HttpUtility.UrlDecode(_Result);
                    if (_Result == "오류")
                    {
                        throw new Exception("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.");
                    }
                    else
                    {
                        Account_Name = _Result;
                        txt_AccountOwner1.Text = Account_Name;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    cmb_Bank1.SelectedIndex = 0;
                    txt_AccountNo1.Text = "";
                    txt_AccountOwner1.Text = "";
                    return;
                }
            }

            try
            {
                clientsBindingSource.EndEdit();
                var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
                Row.Admin = cmb_Admin.Text;

                Row.CMSBankName1 = cmb_Bank1.Text;



                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName1 = @CMSBankName, CMSBankCode1 = @CMSBankCode, CMSAccountNo1 = @CMSAccountNo , CMSOwner1 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", Row.CMSBankName1);
                        _Command.Parameters.AddWithValue("@CMSBankCode", Row.CMSBankCode1);
                        _Command.Parameters.AddWithValue("@CMSAccountNo", Row.CMSAccountNo1);
                        _Command.Parameters.AddWithValue("@CMSOwner", Row.CMSOwner1);


                        _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("저장 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch { }

        }

        private void BankSave2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmb_Bank2.SelectedValue?.ToString()) && !String.IsNullOrEmpty(txt_AccountNo2.Text) && !String.IsNullOrEmpty(txt_AccountOwner2.Text))
            {
                string Mid = "edubill";
                string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_Bank2.SelectedValue.ToString(), txt_AccountNo2.Text, txt_AccountOwner2.Text, cmb_Bank2.Text, Mid);
                WebClient mWebClient = new WebClient();
                try
                {

                    var _Result = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));


                    _Result = HttpUtility.UrlDecode(_Result);
                    if (_Result == "오류")
                    {
                        throw new Exception("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.");
                    }
                    else
                    {
                        Account_Name = _Result;
                        txt_AccountOwner2.Text = Account_Name;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    cmb_Bank2.SelectedIndex = 0;
                    txt_AccountNo2.Text = "";
                    txt_AccountOwner2.Text = "";

                    return;
                }
            }

            try
            {
                clientsBindingSource.EndEdit();
                var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
                Row.Admin = cmb_Admin.Text;

                Row.CMSBankName2 = cmb_Bank2.Text;



                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName2 = @CMSBankName, CMSBankCode2 = @CMSBankCode, CMSAccountNo2 = @CMSAccountNo , CMSOwner2 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", Row.CMSBankName2);
                        _Command.Parameters.AddWithValue("@CMSBankCode", Row.CMSBankCode2);
                        _Command.Parameters.AddWithValue("@CMSAccountNo", Row.CMSAccountNo2);
                        _Command.Parameters.AddWithValue("@CMSOwner", Row.CMSOwner2);


                        _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("저장 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch { }
        }

        private void BankSave3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmb_Bank3.SelectedValue?.ToString()) && !String.IsNullOrEmpty(txt_AccountNo3.Text) && !String.IsNullOrEmpty(txt_AccountOwner3.Text))
            {
                string Mid = "edubill";
                string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_Bank3.SelectedValue.ToString(), txt_AccountNo3.Text, txt_AccountOwner3.Text, cmb_Bank3.Text, Mid);
                WebClient mWebClient = new WebClient();
                try
                {

                    var _Result = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));


                    _Result = HttpUtility.UrlDecode(_Result);
                    if (_Result == "오류")
                    {
                        throw new Exception("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.");
                    }
                    else
                    {
                        Account_Name = _Result;
                        txt_AccountOwner3.Text = Account_Name;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cmb_Bank3.SelectedIndex = 0;
                    txt_AccountNo3.Text = "";
                    txt_AccountOwner3.Text = "";

                    return;
                }
            }

            try
            {
                clientsBindingSource.EndEdit();
                var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
                Row.Admin = cmb_Admin.Text;

                Row.CMSBankName3= cmb_Bank3.Text;



                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName3 = @CMSBankName, CMSBankCode3 = @CMSBankCode, CMSAccountNo3 = @CMSAccountNo , CMSOwner3 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", Row.CMSBankName3);
                        _Command.Parameters.AddWithValue("@CMSBankCode", Row.CMSBankCode3);
                        _Command.Parameters.AddWithValue("@CMSAccountNo", Row.CMSAccountNo3);
                        _Command.Parameters.AddWithValue("@CMSOwner", Row.CMSOwner3);


                        _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("저장 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch { }
        }
        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }
        private void BankSave4_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmb_Bank4.SelectedValue?.ToString()) && !String.IsNullOrEmpty(txt_AccountNo4.Text) && !String.IsNullOrEmpty(txt_AccountOwner4.Text))
            {
                string Mid = "edubill";
                string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_Bank4.SelectedValue.ToString(), txt_AccountNo4.Text, txt_AccountOwner3.Text, cmb_Bank4.Text, Mid);
                WebClient mWebClient = new WebClient();
                try
                {

                    var _Result = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));


                    _Result = HttpUtility.UrlDecode(_Result);
                    if (_Result == "오류")
                    {
                        throw new Exception("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.");
                    }
                    else
                    {
                        Account_Name = _Result;
                        txt_AccountOwner4.Text = Account_Name;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("입력하신 계좌정보 가 틀립니다.확인 후, 다시 입력해 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cmb_Bank4.SelectedIndex = 0;
                    txt_AccountNo4.Text = "";
                    txt_AccountOwner4.Text = "";
                    return;
                }

                try
                {
                    clientsBindingSource.EndEdit();
                    var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
                    Row.Admin = cmb_Admin.Text;

                    Row.CMSBankName4 = cmb_Bank4.Text;



                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                             @"UPDATE Clients
                            SET  CMSBankName3 = @CMSBankName, CMSBankCode3 = @CMSBankCode, CMSAccountNo3 = @CMSAccountNo , CMSOwner3 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                            _Command.Parameters.AddWithValue("@CMSBankName", Row.CMSBankName4);
                            _Command.Parameters.AddWithValue("@CMSBankCode", Row.CMSBankCode4);
                            _Command.Parameters.AddWithValue("@CMSAccountNo", Row.CMSAccountNo4);
                            _Command.Parameters.AddWithValue("@CMSOwner", Row.CMSOwner4);


                            _Command.Parameters.AddWithValue("@ClientId", Row.ClientId);
                            _Command.ExecuteNonQuery();
                        }
                    });

                    MessageBox.Show("저장 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                catch { }
            }


        }

        private void BankDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"삭제겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {

                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName = @CMSBankName, CMSBankCode = @CMSBankCode, CMSAccountNo = @CMSAccountNo , CMSOwner = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", "");
                        _Command.Parameters.AddWithValue("@CMSBankCode", "");
                        _Command.Parameters.AddWithValue("@CMSAccountNo", DBNull.Value);
                        _Command.Parameters.AddWithValue("@CMSOwner", DBNull.Value);


                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("삭제 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);

                
            }
            catch
            {

            }

            btn_Search_Click(null, null);
        }

        private void BankDelete1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"삭제겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {

                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName1 = @CMSBankName, CMSBankCode1 = @CMSBankCode, CMSAccountNo1 = @CMSAccountNo , CMSOwner1 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", "");
                        _Command.Parameters.AddWithValue("@CMSBankCode", "");
                        _Command.Parameters.AddWithValue("@CMSAccountNo", DBNull.Value);
                        _Command.Parameters.AddWithValue("@CMSOwner", DBNull.Value);


                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("삭제 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {

            }
            btn_Search_Click(null, null);
        }

        private void BankDelete2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"삭제겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {

                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName2 = @CMSBankName, CMSBankCode2 = @CMSBankCode, CMSAccountNo2 = @CMSAccountNo , CMSOwner2 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", "");
                        _Command.Parameters.AddWithValue("@CMSBankCode", "");
                        _Command.Parameters.AddWithValue("@CMSAccountNo", DBNull.Value);
                        _Command.Parameters.AddWithValue("@CMSOwner", DBNull.Value);


                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("삭제 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {

            }
            btn_Search_Click(null, null);
        }

        private void BankDelete3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"삭제겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {

                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName3 = @CMSBankName, CMSBankCode3 = @CMSBankCode, CMSAccountNo3 = @CMSAccountNo , CMSOwner3 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", "");
                        _Command.Parameters.AddWithValue("@CMSBankCode", "");
                        _Command.Parameters.AddWithValue("@CMSAccountNo", DBNull.Value);
                        _Command.Parameters.AddWithValue("@CMSOwner", DBNull.Value);


                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("삭제 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {

            }
            btn_Search_Click(null, null);
        }

        private void BankDelete4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"삭제겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {

                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                         @"UPDATE Clients
                            SET  CMSBankName4 = @CMSBankName, CMSBankCode4 = @CMSBankCode, CMSAccountNo4 = @CMSAccountNo , CMSOwner4 = @CMSOwner 
                            
                            WHERE ClientId = @ClientId";


                        _Command.Parameters.AddWithValue("@CMSBankName", "");
                        _Command.Parameters.AddWithValue("@CMSBankCode", "");
                        _Command.Parameters.AddWithValue("@CMSAccountNo", DBNull.Value);
                        _Command.Parameters.AddWithValue("@CMSOwner", DBNull.Value);


                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });

                MessageBox.Show("삭제 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {

            }
            btn_Search_Click(null, null);
        }
        void KakaoPriceChanged(object sender, EventArgs e)
        {
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.KakaoPriceGubun == (int)cmbKakaoPrice.SelectedValue)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET KakaoPriceGubun = @KakaoPriceGubun" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@KakaoPriceGubun", int.Parse(cmbKakaoPrice.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.KakaoPriceGubun = (int)cmbKakaoPrice.SelectedValue;
 
        }
        private void cmbKakaoPrice_SelectedIndexChanged(object sender, EventArgs e)
        {


            KakaoPriceChanged(null,null);


        }

        private void chkSms_CheckedChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null )
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.SmsYn == (bool)chkSms.Checked)
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET SmsYn = @SmsYn" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@SmsYn", (bool)chkSms.Checked);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.SmsYn = (bool)chkSms.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            //if (Row.CargoApiYn == false)
            //    return;
            bool _CargoApiYn = false;
            if (rdoCargoApiY.Checked)
            {
                _CargoApiYn = true;
            }
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET CargoApiYn = @CargoApiYn" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@CargoApiYn", _CargoApiYn);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }




            if (_CargoApiYn == true)
            {
                Row.CargoApiYn = true;
            }
            else
            {
                Row.CargoApiYn = false;
            }
        }

        private void cmbEtaxGubun_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (clientsBindingSource.Current == null || cmbEtaxGubun.SelectedValue == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            if (Row.EtaxGubun == cmbEtaxGubun.SelectedValue.ToString())
                return;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET EtaxGubun = @EtaxGubun" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@EtaxGubun", cmbEtaxGubun.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            Row.EtaxGubun = cmbEtaxGubun.SelectedValue.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
            var _Clients = LocalUser.Instance.LogInInformation.Client;

            //MessageBox.Show("공인인증서를 등록하셔야 합니다. 공인인증 등록 페이지로 이동합니다.\r\n인증서 등록후 회원정보에 공인인증서 비밀번호를 입력하세요.");
            // MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);


            var Query = baseDataSet.AdminInfoes.FirstOrDefault();
            string url = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do?";
            if (Query.IsTest != true)
            {
                url = "http://www.nicedata.co.kr/ti/TI_80101.do?";
            }


            string frnNo = "";
            string userid = "";
            string passwd = "";
            string Linkcd = "";

            try
            {
                frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.frnNo }).ToString();
                userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.userid }).ToString();
                passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.passwd }).ToString();
                Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "EDB" }).ToString();


                //MessageBox.Show(Encode.ToString());

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);

            }

            string value = $"frnNo={frnNo}&userId={userid}&passwd={passwd}&linkCd={Linkcd}&retUrl=";

            //FrmNice f = new FrmNice(value, url);
            //f.ShowDialog();

            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Internet Explorer\\iexplore.exe", url + value);
          
        }

        private void rdoCarBankN_CheckedChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null)
                return;
            var Row = ((DataRowView)clientsBindingSource.Current).Row as ClientDataSet.ClientsRow;
            if (Row == null)
                return;
            //if (Row.CargoApiYn == false)
            //    return;
            bool _CarBankYn = false;
            if (rdoCarBankY.Checked)
            {
                _CarBankYn = true;
            }
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET CarBankYn = @CarBankYn" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@CarBankYn", _CarBankYn);
                cmd.Parameters.AddWithValue("@ClientId", Row.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }




            if (_CarBankYn == true)
            {
                Row.CarBankYn = true;
            }
            else
            {
                Row.CarBankYn = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label11.Text = string.Empty;
           var aa =  CRNRequest.call(txt_BizNo.Text.Replace("-",""));

            label11.Text = aa;


            //if(aa.Contains("폐업"))
            //{
            //    MessageBox.Show("폐업상태입니다");
            //}
            //else
            //{
            //    MessageBox.Show(aa);
            //}
            
        }
    }
}
