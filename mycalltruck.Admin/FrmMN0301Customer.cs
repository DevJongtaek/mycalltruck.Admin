using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.XML;
using mycalltruck.Admin.CMDataSetTableAdapters;
using mycalltruck.Admin.DataSets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static mycalltruck.Admin.CMDataSet;
using System.Drawing.Text;
using OfficeOpenXml.Style;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301Customer : Form
    {

        private bool ValidateIgnore = false;
        private bool IsCurrentNull = false;
        //ShareOrderDataSet ShareOrderDataSet = null;
        bool HasLoaded = false;
        Action LoadAction = null;
        int _MNSTOrderId = 0;
        string _MCreateTime = "";
        string _MGubun = string.Empty;
        // bool HideMyCarOrder = false;
        string Key = "783AE1841E52783A";
        string _SHA256HASH = "";
        //0.용차
        //1.지입
        int OrderGubun = 0;
        int _KakaoPriceGubun = 2;
        string _ORDERCODE = "";
        System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();
        MenuAuth auth = MenuAuth.None;
        CargoApiClass cargoApiClass = new CargoApiClass();
        string code = "";
        string msg = "";
        string _AddressGubun = "";

        //string ApiLiveUrl = "http://cargo.api.labbgsoft.kr:8088";
        //string ApiTestUrl = "http://cargo.apitest.labbgsoft.kr:8080";
        string ApiUrl = "";
        string ApiGubun = "";
        string CargoId = "";
        //string Key = "783AE1841E52783A";
        //string _SHA256HASH = "";
        private void ApplyAuth()
        {
            //auth = this.GetAuth();
            //switch (auth)
            //{
            //    case MenuAuth.None:
            //        MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        Close();
            //        return;
            //    case MenuAuth.Read:

            //        //CopyToApplication.Enabled = false;
            //        btnOrderAfterAdd.Enabled = false;
            //        btnExcel.Enabled = false;
            //        tableLayoutPanel9.Enabled = false;
            //        grid1.ReadOnly = true;

            //        break;
            //}


        }

        public FrmMN0301Customer(string _mGubun = "", int _mNSTOrderId = 0, string _mCreateTime = "")
        {

            //this.ShareOrderDataSet = ShareOrderDataSet;
            InitializeComponent();




            _MNSTOrderId = _mNSTOrderId;
            _MCreateTime = _mCreateTime;
            _MGubun = _mGubun;
            SearchTextFilter.SelectedIndex = 0;
            //SearchDriverTrade.SelectedIndex = 0;
            //SearchCustomerTax.SelectedIndex = 0;
            //SelectApplication.SelectedIndex = 0;
            cmbSMonth.SelectedIndex = 0;
            
            DateFilterBegin.Value = DateTime.Now.Date;
            DateFilterEnd.Value = DateTime.Now.Date;
            SetStaticOptions();
            // SetDriverList();
            SetCustomerList();
            InitDriverTable();
            InitCustomerStartTable();
            InitClientTable();
            //itSubClientTable();
            //grid1
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);
            //requestday.DefaultCellStyle.ForeColor = Color.Blue;
            //requestday.DefaultCellStyle.SelectionForeColor = Color.Blue;

           

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            
        }


        private void FrmMN0301_Load(object sender, EventArgs e)
        {
            //PrivateFontCollection privateFonts = new PrivateFontCollection();


            ////폰트명이 아닌 폰트의 파일명을 적음
            //privateFonts.AddFontFile("NanumSquareR.ttf");


            ////24f 는 출력될 폰트 사이즈
            //Font font = new Font(privateFonts.Families[0], 11f);

            //Font = font;

            LocalUser.Instance.LogInInformation.LoadClient();
            // HideMyCarOrder = LocalUser.Instance.LogInInformation.Client.HideMyCarOrder;
            OrderGubun = LocalUser.Instance.LogInInformation.Client.OrderGubun;
            this.cargoApiURLTableAdapter.Fill(this.cargoDataSet.CargoApiURL);
            cargoApiAdressTableAdapter.Fill(cargoDataSet.CargoApiAdress);
            _KakaoPriceGubun = LocalUser.Instance.LogInInformation.Client.KakaoPriceGubun;
            //if (HideMyCarOrder == false)
            //{
            //    ShareSearch.Enabled = false;
            //    chkHideMyCarOrder.Checked = false;
            //}
            //else
            //{
            //    chkHideMyCarOrder.Checked = true;
            //    ShareSearch.Enabled = true;
            //}


            if (_MNSTOrderId != 0)
            {
                if (_MGubun == "Order")
                {
                    _Sort = false;
                    _Search(6);
                }


            }
            else
            {
                if (string.IsNullOrEmpty(_MCreateTime))
                {
                    _Sort = false;
                    _Search();
                }
                else
                {
                    _Sort = false;
                    _Search(9);
                }
            }
            HasLoaded = true;
            this.Start.GotFocus += OnFocus;
            this.Stop.GotFocus += OnFocus;
            this.Item.GotFocus += OnFocus;
            this.Customer.GotFocus += OnFocus;
            //this.CarSize.GotFocus += OnDropDown;
            //this.CarType.GotFocus += OnDropDown;
            //this.StopDateHelper.GotFocus += OnDropDown;
            //this.PayLocation.GotFocus += OnDropDown;


            if (LoadAction != null)
                LoadAction();


            if (_MGubun == "Customer" && _MNSTOrderId != 0)
            {
                LoadCustomer(_MNSTOrderId);
            }


            var CargoQuery = cargoDataSet.CargoApiURL.ToArray();

            ApiUrl = CargoQuery.Where(c => c.UseYN == "Y").First().ApiUrl;
            //CargoId = LocalUser.Instance.LogInInformation.Client.CargoLoginId;
            CargoId = LocalUser.Instance.LogInInformation.CargoLoginId;
            ApiGubun = CargoQuery.Where(c => c.UseYN == "Y").First().Gubun;
            //mTimer.Interval = 10 * 1000;
            //mTimer.Tick += MTimer_Tick;

        }

        private void MTimer_Tick(object sender, EventArgs e)
        {
            _Search();
        }
        BindingList<Model> _CoreList = new BindingList<Model>();
        private void _SearchAddress(string _Address)
        {
            if (String.IsNullOrEmpty(_Address))
                return;
            _CoreList.Clear();
            try
            {
                var Url = $"http://www.juso.go.kr/addrlink/addrLinkApi.do?resultType=json&countPerPage=100&keyword={_Address}&confmKey=U01TX0FVVEgyMDE2MTIyMjE2MTY0NjE3NTky";
                WebClient _WebClient = new WebClient();
                _WebClient.Encoding = Encoding.UTF8;
                var r = _WebClient.DownloadString(Url);
                dynamic _Array = JsonConvert.DeserializeObject(r);
                if (_Array.results != null)
                {
                    dynamic jusoArray = _Array.results.juso;
                    foreach (var juso in jusoArray)
                    {
                        var roadAddr = juso.roadAddr.ToString();
                        var zipNo = juso.zipNo.ToString();
                        var jibunAddr = juso.jibunAddr;
                        _CoreList.Add(new Model
                        {
                            Zip = zipNo,
                            Address = roadAddr,
                            Jibun = jibunAddr,
                        });
                    }
                }
            }
            catch (Exception)
            {

            }
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
            public string ServiceState { get; set; }


        }


        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _DriverTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,MobileNo,CarNo,CarYear,CarSize,CarType,Password,ServiceState FROM Drivers with (nolock)";

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
                                  ServiceState = dataReader.GetInt32(13).ToString()
                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();
        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }
            public string CEO { get; set; }
        }


        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name, LoginId, isnull(PhoneNo,''),isnull(MobileNo,''),ISNULL(CEO,'')  FROM Clients with (nolock)";
                    //commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientTable.Add(
                              new ClientViewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                                  PhoneNo = dataReader.GetString(4),
                                  MobileNo = dataReader.GetString(5),
                                  CEO = dataReader.GetString(6),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private List<CustomerStartViewModel> _CustomerStartTable = new List<CustomerStartViewModel>();
        class CustomerStartViewModel
        {
            public int idx { get; set; }
            public string StartState { get; set; }
            public string StartCity { get; set; }
            public string StartStreet { get; set; }
            public string StartDetail { get; set; }
            public string StartName { get; set; }
            public string StartPhoneNo { get; set; }
            public int ClientId { get; set; }
            public int CustomerId { get; set; }
            public DateTime CreateTime { get; set; }
            public string SGubun { get; set; }



        }
        private void InitCustomerStartTable()
        {
            _CustomerStartTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT  idx, StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,SGubun FROM CustomerStartManage with (nolock)";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _CustomerStartTable.Add(
                              new CustomerStartViewModel
                              {
                                  idx = dataReader.GetInt32(0),
                                  StartState = dataReader.GetString(1),
                                  StartCity = dataReader.GetString(2),
                                  StartStreet = dataReader.GetString(3),
                                  StartDetail = dataReader.GetString(4),
                                  StartName = dataReader.GetString(5),
                                  StartPhoneNo = dataReader.GetString(6),
                                  ClientId = dataReader.GetInt32(7),
                                  CustomerId = dataReader.GetInt32(8),
                                  CreateTime = dataReader.GetDateTime(9),
                                  SGubun = dataReader.GetString(10),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        public void LoadCustomer(int CustomerId)
        {
            if (HasLoaded)
                InnerLoadCustomer(CustomerId);
            else
            {
                LoadAction = new Action(() => InnerLoadCustomer(CustomerId));
            }
        }

        private void InnerLoadCustomer(int CustomerId)
        {
            
            Customer.Clear();

            var _Customer = CustomerModelList.Find(c => c.CustomerId == CustomerId);
            StartState.Text = _Customer.AddressState;
            StartCity.Text = _Customer.AddressCity;
            StartStreet.Text = "";
            Start.Text = _Customer.AddressDetail;
            Customer.Text = _Customer.SangHo;
            OrderPhoneNo.Text = _Customer.PhoneNo;
            Customer.Tag = CustomerId;
            CustomerManager.Text = _Customer.CustomerManager;
            StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
            //if (_Customer.PointMethod == 2)
            //    DriverPoint.Checked = true;
        }

        private void SetStaticOptions()
        {
            CarSize.DisplayMember = "Name";
            CarSize.ValueMember = "Value";
            CarType.DisplayMember = "Name";
            CarType.ValueMember = "Value";
            CarSize.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value != 0 && c.Value != 99).ToList());
            CarSize.SelectedValue = 1;
            _SetStaticOption(StopDateHelper, "StopDateHelper");
            // _SetStaticOption(PayLocation, "PayLocation");

            PayLocation.DisplayMember = "Name";
            PayLocation.ValueMember = "Value";
            PayLocation.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "PayLocation" && c.Value != 3).ToList());

            PayLocation.SelectedIndex = 1;

            UnitType.DisplayMember = "Name";
            UnitType.ValueMember = "Value";
            UnitType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Unit").ToList());
            UnitType.SelectedIndex = 0;



            //cmb_CarSize.DisplayMember = "Name";
            //cmb_CarSize.ValueMember = "Value";
            //cmb_CarSize.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").ToList());
            //cmb_CarSize.SelectedValue = 0;

            //cmb_CarType.DisplayMember = "Name";
            //cmb_CarType.ValueMember = "Value";
            //cmb_CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").ToList());
            //cmb_CarType.SelectedValue = 0;


            DriverGrade.DisplayMember = "Name";
            DriverGrade.ValueMember = "Value";
            DriverGrade.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "DriverGrade").ToList());
            DriverGrade.SelectedValue = 0;



            _SetStaticOption(SharedItemLength, "SharedItemLength");
            //_SetStaticOption(SharedItemSize, "SharedItemSize");


            Dictionary<int, string> DCustomer = new Dictionary<int, string>();

            customerTeamsTableAdapter.Fill(customerUserDataSet.CustomerTeams, LocalUser.Instance.LogInInformation.ClientId);

            if (LocalUser.Instance.LogInInformation.CustomerUserId == 0)
            {
                var cmbCustomerTeamataSource = customerUserDataSet.CustomerTeams.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == LocalUser.Instance.LogInInformation.CustomerId).Select(c => new { c.TeamName, c.CustomerTeamId }).OrderBy(c => c.CustomerTeamId).ToArray();

                DCustomer.Add(0, "부서검색");


                foreach (var item in cmbCustomerTeamataSource)
                {
                    DCustomer.Add(item.CustomerTeamId, item.TeamName);
                }


                cmbTeam.DataSource = new BindingSource(DCustomer, null);
                cmbTeam.DisplayMember = "Value";
                cmbTeam.ValueMember = "Key";

                cmbTeam.SelectedValue = 0;
            }
            else
            {
                var cmbCustomerTeamataSource = customerUserDataSet.CustomerTeams.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerTeamId == LocalUser.Instance.LogInInformation.CustomerTeamId).Select(c => new { c.TeamName, c.CustomerTeamId }).OrderBy(c => c.CustomerTeamId).ToArray();

                DCustomer.Add(0, "부서검색");


                foreach (var item in cmbCustomerTeamataSource)
                {
                    DCustomer.Add(item.CustomerTeamId, item.TeamName);
                }


                cmbTeam.DataSource = new BindingSource(DCustomer, null);
                cmbTeam.DisplayMember = "Value";
                cmbTeam.ValueMember = "Key";

                cmbTeam.SelectedValue = 0;
            }
           


           // initReferralId();



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
                    _Command.CommandText = $"SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo, Customers.PointMethod, Customers.Fee, Customers.AddressState, Customers.AddressCity, Customers.AddressDetail, Customers.BizNo,Customers.MobileNo,ISNULL(CustomerManager.ManagerName,''),BizGubun FROM Customers with (nolock)" +
                        $" LEFT  JOIN CustomerManager ON Customers.CustomerManagerId = CustomerManager.ManagerId" +

                        $" WHERE Customers.BizNo <> '000-00-00000' AND Customers.ClientId = {LocalUser.Instance.LogInInformation.ClientId} AND Customers.CustomerId = {LocalUser.Instance.LogInInformation.CustomerId}";
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
                            });
                        }
                    }
                }
                _Connection.Close();
            }
        }

        class DriverModel
        {
            public int DriverId { get; set; }
            public string Name { get; set; }
            public string CarNo { get; set; }
            public string MobileNo { get; set; }
            public string CarYear { get; set; }
            public int CarType { get; set; }
            public int CarSize { get; set; }
            public decimal DriverPoint { get; set; }
            public int CandidateId { get; set; }
        }
        List<DriverModel> DriverModelList = new List<DriverModel>();
        private void SetDriverList()
        {
            DriverModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Distinct Drivers.DriverId, ISNULL(CEO,''), ISNULL(Name,''), CarNo, MobileNo, CarType, CarSize, CarYear,CandidateId" +
                        $", 0 as DriverPoint " +
                        $" FROM Drivers with (nolock) JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId WHERE Drivers.ServiceState != 5 ";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            DriverModelList.Add(new DriverModel
                            {
                                DriverId = _Reader.GetInt32(0),
                                Name = _Reader.GetStringN(2),
                                CarNo = _Reader.GetStringN(3),
                                MobileNo = _Reader.GetStringN(4),
                                CarType = _Reader.GetInt32(5),
                                CarSize = _Reader.GetInt32(6),
                                CarYear = _Reader.GetStringN(7),
                                //DriverPoint = _Reader.GetDecimal(8),
                                DriverPoint = _Reader.GetInt32(8),
                                CandidateId = _Reader.GetInt32(9),
                            });
                        }
                    }
                }
                _Connection.Close();
            }
        }

        private void _SetStaticOption(ComboBox mComboBox, String Div)
        {
            mComboBox.DisplayMember = "Name";
            mComboBox.ValueMember = "Value";
            mComboBox.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == Div).ToList());
            mComboBox.SelectedIndex = 0;
        }

     
        List<int> ClientDataSourceList = new List<int>();
        private void ClientListViewList()
        {
            ClientDataSourceList.Clear();


            ClientDataSourceList.Add(LocalUser.Instance.LogInInformation.ClientId);

            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                    "SELECT ChildClientId, Name FROM ClientInstances JOIN Clients ON ClientInstances.ChildClientId = Clients.ClientId WHERE ClientInstances.ParentClientId = @ParentClientId";
                    _Command.Parameters.AddWithValue("@ParentClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            ClientDataSourceList.Add(_Reader.GetInt32(0));
                        }
                    }
                }
            });
        }

        class SubClientViewModel
        {
            public int SubClientId { get; set; }

            public string Name { get; set; }

        }
        private List<SubClientViewModel> _SubClientTable = new List<SubClientViewModel>();

        private void InitSubClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT Name, SubClientId FROM SubClients WHERE ClientId = @Clientid ";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _SubClientTable.Add(
                              new SubClientViewModel
                              {
                                  Name = dataReader.GetString(0),
                                  SubClientId = dataReader.GetInt32(1),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        class ClientAndUseriewModel
        {
            public int ClientId { get; set; }
            public string LoginId { get; set; }

            public string Name { get; set; }

        }

        class CustomerAndUseriewModel
        {
            public int CustomerId { get; set; }
            public string LoginId { get; set; }

            public string Name { get; set; }
            public int TeamId { get; set; }

        }
        private List<ClientAndUseriewModel> _ClientAndUserTable = new List<ClientAndUseriewModel>();
        private void InitClientAndUsersTable()
        {

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT  ClientUserId, LoginId, Password, ClientId, Part, Name,CreateTime" +
                        " FROM ClientUsers " +
                        " WHERE(ClientId = @Clientid) AND IsRegister != 1 " +
                        " UNION ALL " +
                        " SELECT ClientId, LoginId, Password, ClientId AS Expr1, '슈퍼관리자' AS Expr2, CEO,CreateDate " +
                        " FROM Clients " +
                        " WHERE(ClientId = @Clientid) " +
                        " ORDER BY CreateTime DESC";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientAndUserTable.Add(
                              new ClientAndUseriewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  LoginId = dataReader.GetString(1),
                                  Name = dataReader.GetString(5),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        private List<CustomerAndUseriewModel> _CustomerAndUserTable = new List<CustomerAndUseriewModel>();
        private void InitCustomerAndUsersTable()
        {

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT  idx, LoginId, Password, ClientId, AddTeam, AddName,CreateTime,ISNULL(TeamId,0) " +
                        " FROM CustomerAddPhone " +
                        " WHERE(idx = @CustomerId) " +
                        " UNION ALL " +
                        " SELECT CustomerId, LoginId, Password, ClientId AS Expr1, '슈퍼관리자' AS Expr2, CEO,CreateTime ,0 " +
                        " FROM Customers " +
                        " WHERE(CustomerId = @CustomerId) " +
                        " ORDER BY CreateTime DESC";
                    commnad.Parameters.AddWithValue("@CustomerId", LocalUser.Instance.LogInInformation.CustomerId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _CustomerAndUserTable.Add(
                              new CustomerAndUseriewModel
                              {
                                  CustomerId = dataReader.GetInt32(0),
                                  LoginId = dataReader.GetString(1),
                                  Name = dataReader.GetString(5),
                                   
                                  TeamId = dataReader.GetInt32(7),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        private void _Search(int OrderState = 0)
        {
            // SetDriverList();
            CargoStatus = "";
            InitSubClientTable();
            InitClientAndUsersTable();
            InitCustomerAndUsersTable();
            // InitDriverTable();
            var Now = DateFilterBegin.Value.Date;
            var PNow = DateTime.Now.AddMonths(-3).AddDays(1).Date;
            var Tommorow = DateFilterEnd.Value.AddDays(1).Date;

            grid1.AutoGenerateColumns = false;
            ValidateIgnore = true;
            List<Order> DataSource = new List<Order>();

            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {


                ClientListViewList();

                switch (OrderState)
                {
                    //전체
                    case 0:
                        if (_Sort == false)
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();



                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            _Sort = true;

                        }
                        else
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            _Sort = false;

                        }
                        break;
                    //접수
                    case 1:
                        if (_Sort == false)
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            _Sort = true;
                        }
                        else
                        {
                            if (_SortGubun == "ClientId")
                            {

                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            _Sort = false;

                        }
                        break;
                    //완료
                    case 3:
                        if (_Sort == false)
                        {
                            if (_SortGubun == "ClientId")
                            {

                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            _Sort = true;
                        }
                        else
                        {
                            if (_SortGubun == "ClientId")
                            {

                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            _Sort = false;

                        }
                        break;

                    case 4:
                        if (_Sort == false)
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderByDescending(c => c.Customer).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderByDescending(c => c.StartName).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderByDescending(c => c.StopName).ToList();
                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            _Sort = true;
                        }
                        else
                        {
                            if (_SortGubun == "ClientId")
                            {

                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderBy(c => c.Customer).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderBy(c => c.CreateTime).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderBy(c => c.StartName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.CreateTime >= Tommorow).OrderBy(c => c.StopName).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            _Sort = false;
                        }
                        break;
                    //예약/접수/완료/공유/취소
                    case 5:
                        if (_Sort == false)
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                            }
                            _Sort = true;
                        }
                        else
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();


                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();


                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList();

                                DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();


                            }
                            _Sort = false;
                        }
                        break;
                    case 6:
                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderId == _MNSTOrderId).ToList();
                        //  GridIndex = ;


                        break;

                    //
                    case 8:
                        if (_Sort == false)
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();
                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();
                            }
                            _Sort = true;
                        }
                        else
                        {
                            if (_SortGubun == "ClientId")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                            }
                            else if (_SortGubun == "Time")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                            }
                            else if (_SortGubun == "Start")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();

                            }
                            else if (_SortGubun == "Stop")
                            {
                                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CustomerId == (int)Customer.Tag && c.CreateTime >= PNow && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();

                            }
                            _Sort = false;
                        }

                        break;


                }

                if (!String.IsNullOrEmpty(SearchText.Text))
                {
                    switch (SearchTextFilter.SelectedIndex)
                    {
                        case 1:
                            DataSource = DataSource.Where(c => c.Driver != null && c.DriverModel != null && c.DriverModel.Name.Contains(SearchText.Text)).ToList();
                            break;
                        case 2:
                            DataSource = DataSource.Where(c => c.Driver != null && c.Driver.Contains(SearchText.Text)).ToList();
                            break;
                        case 3:
                            DataSource = DataSource.Where(c => c.DriverPhoneNo != null && c.DriverPhoneNo.Replace("-", "").Contains(SearchText.Text.Replace("-", ""))).ToList();
                            break;
                        case 4:
                            DataSource = DataSource.Where(c => c.DriverCarNo != null && c.DriverCarNo.Contains(SearchText.Text)).ToList();
                            break;
                        case 5:
                            DataSource = DataSource.Where(c => c.Customer != null && c.Customer.Contains(SearchText.Text)).ToList();
                            break;
                        case 6:
                            DataSource = DataSource.Where(c => c.OrderPhoneNo != null && c.OrderPhoneNo.Replace("-", "").Contains(SearchText.Text.Replace("-", ""))).ToList();
                            break;
                        case 7:
                            DataSource = DataSource.Where(c => c.Item != null && c.Item.Contains(SearchText.Text)).ToList();
                            break;
                        case 8:
                            DataSource = DataSource.Where(c => c.StartName != null && c.StartName.Contains(SearchText.Text)).ToList();
                            break;
                        case 9:
                            DataSource = DataSource.Where(c => c.StopName != null && c.StopName.Contains(SearchText.Text)).ToList();
                            break;


                    }
                }





               

                DataListSource.DataSource = DataSource.Where(C => C.CustomerId == LocalUser.Instance.LogInInformation.CustomerId).ToList();

                if (LocalUser.Instance.LogInInformation.CustomerUserId != 0)
                { DataListSource.DataSource = DataSource.Where(C => C.CustomerTeam == LocalUser.Instance.LogInInformation.CustomerTeamId).ToList(); }
                else
                {
                    if (cmbTeam.SelectedIndex > 0)
                    {
                        DataListSource.DataSource = DataSource.Where(C => C.CustomerTeam == (int)cmbTeam.SelectedValue).ToList();
                    }

                }

            }

            ValidateIgnore = false;
            if (OrderState != 6)
            {
                NewOrder_Click(null, null);
            }
            else
            {
                grid1.CurrentCell = grid1.Rows[0].Cells[0];

                DataListSource_CurrentChanged(null, null);
            }

            SetRowBackgroundColor();
        }

        private void SetRowBackgroundColor()
        {

            for (int RowIndex = 0; RowIndex < grid1.RowCount; RowIndex++)
            {
                var Model = DataListSource[RowIndex] as Order;
                //취소
                if (Model.OrderStatus == 0)
                {
                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Red;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Red;
                    }
                }



                //접수
                else if (Model.OrderStatus == 1 && Model.MyCarOrder == false && (Model.CreateTime.Date <= DateTime.Now.Date))
                {
                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Blue;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Blue;
                    }
                }
                //대기
                else if (Model.OrderStatus == 2 && Model.MyCarOrder == false && (Model.CreateTime.Date <= DateTime.Now.Date))
                {
                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Blue;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Blue;
                    }
                }
                //예약
                else if (Model.OrderStatus == 1 && Model.CreateTime.Date > DateTime.Now.Date)
                {

                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Green;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Green;
                    }


                }


                //완료
                else if (Model.OrderStatus == 3)
                {


                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Black;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Black;
                    }

                    #region 차량정보 올바르지않은것
                    //if (Model.DriverId != 0)
                    //{
                    //    if (Model.DriverModel == null)
                    //        return;



                    //    if (String.IsNullOrEmpty(Model.DriverModel.BizNo) || String.IsNullOrEmpty(Model.DriverModel.Name) || String.IsNullOrEmpty(Model.DriverModel.CEO) || String.IsNullOrEmpty(Model.DriverModel.Uptae) || String.IsNullOrEmpty(Model.DriverModel.Upjong) || Model.DriverModel.Name == "." || Model.DriverModel.CEO == "." || Model.DriverModel.Upjong == "." || Model.DriverModel.Uptae == "." || String.IsNullOrEmpty(Model.DriverModel.AddressState) || String.IsNullOrEmpty(Model.DriverModel.AddressCity) || String.IsNullOrEmpty(Model.DriverModel.AddressDetail) || String.IsNullOrEmpty(Model.DriverModel.PayBankName) || String.IsNullOrEmpty(Model.DriverModel.PayAccountNo) || String.IsNullOrEmpty(Model.DriverModel.PayInputName))
                    //    {
                    //        for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    //        {
                    //            grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Green;
                    //            grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Green;
                    //        }

                    //        if (Model.DriverModel.BizNo.Length > 0)
                    //        {
                    //            if (Model.DriverModel.BizNo.Substring(0, 3) == "000" || Model.DriverModel.BizNo.Substring(0, 3) == "999")
                    //            {
                    //                for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    //                {
                    //                    grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Green;
                    //                    grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Green;
                    //                }
                    //            }
                    //        }


                    //    }
                    //    else
                    //    {
                    //        if (Model.DriverModel.BizNo.Length > 0)
                    //        {
                    //            if (Model.DriverModel.BizNo.Substring(0, 3) == "000" || Model.DriverModel.BizNo.Substring(0, 3) == "999")
                    //            {
                    //                for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    //                {
                    //                    grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Green;
                    //                    grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Green;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    //                {
                    //                    grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Black;
                    //                    grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Black;
                    //                }
                    //            }
                    //        }

                    //    }
                    //}
                    #endregion

                }


                else
                {
                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Black;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Black;
                    }
                }
            }
        }

    
        int GridIndex = 0;
        


       

        private void StartInfo_CheckedChanged(object sender, EventArgs e)
        {
            var _StartInfo = sender as CheckBox;
            if (_StartInfo.Checked)
            {
                if (_StartInfo != StartInfo1)
                    StartInfo1.Checked = false;
                if (_StartInfo != StartInfo2)
                    StartInfo2.Checked = false;
                if (_StartInfo != StartInfo3)
                    StartInfo3.Checked = false;
                if (_StartInfo != StartInfo4)
                    StartInfo4.Checked = false;
                if (_StartInfo != StartInfo5)
                    StartInfo5.Checked = false;
            }
            SetRemark();
        }

        private void StopInfo_CheckedChanged(object sender, EventArgs e)
        {
            var _StopInfo = sender as CheckBox;
            if (_StopInfo.Checked)
            {
                if (_StopInfo != StopInfo1)
                    StopInfo1.Checked = false;
                if (_StopInfo != StopInfo2)
                    StopInfo2.Checked = false;
                if (_StopInfo != StopInfo3)
                    StopInfo3.Checked = false;
                if (_StopInfo != StopInfo4)
                    StopInfo4.Checked = false;
                if (_StopInfo != StopInfo5)
                    StopInfo5.Checked = false;
            }
            SetRemark();
        }

        private void NotShared_CheckedChanged(object sender, EventArgs e)
        {
            if (NotShared.Checked)
            {
                Shared.Checked = false;
                SharedItemLength.SelectedIndex = 0;
                SharedItemSize.SelectedIndex = 0;
                SharedItemLength.Enabled = false;
                SharedItemSize.Enabled = false;
            }
            else
            {
                if (Shared.Checked == false)
                {
                    NotShared.Checked = true;
                }
            }
            SetRemark();
        }

        private void Shared_CheckedChanged(object sender, EventArgs e)
        {
            if (Shared.Checked)
            {
                NotShared.Checked = false;
                SharedItemLength.Enabled = true;
                SharedItemSize.Enabled = true;
            }
            else
            {
                if (NotShared.Checked == false)
                {
                    Shared.Checked = true;
                }
                else
                {
                    SharedItemLength.SelectedIndex = 0;
                    SharedItemSize.SelectedIndex = 0;
                    SharedItemLength.Enabled = false;
                    SharedItemSize.Enabled = false;
                }
            }
            SetRemark();
        }

        private void Round_CheckedChanged(object sender, EventArgs e)
        {
            SetRemark();


            //if (Round.Checked)
            //{
            //    //if (!Stop.Text.Contains("왕복"))
            //    //{
            //    //    Stop.Text += " 왕복";
            //    //}
            //}
            //else
            //{
            //    //if (Stop.Text.EndsWith("왕복"))
            //    //{
            //    //    Stop.Text = Stop.Text.Substring(0, Stop.Text.Length - 2).Trim();
            //    //}
            //}
        }

        private void StartTimeType1_CheckedChanged(object sender, EventArgs e)
        {
            if (StartTimeType1.Checked == false)
            {
                //if(StartTimeType2.Checked == false && StartTimeType3.Checked == false && String.IsNullOrEmpty(StartTimeType4.Text))
                //{
                //    StartTimeType1.Checked = true;
                //}
            }
            else
            {
                StartTimeType2.Checked = false;
                StartTimeType3.Checked = false;
                StartTimeType4.Clear();
                StartTimeHour1.SelectedIndex = 0;
                StartTimeHour1.Enabled = false;
                StartTimeHour2.SelectedIndex = 0;
                StartTimeHour2.Enabled = false;
                StartTimeHalf.Checked = false;
                StartTimeHalf.Enabled = false;

                StartTimeETC.Checked = false;
                StartTimeETC.Enabled = false;

            }
            SetRemark();
        }

        private void StartTimeType2_CheckedChanged(object sender, EventArgs e)
        {
            if (StartTimeType2.Checked == false)
            {
                //if (StartTimeType1.Checked == false && StartTimeType3.Checked == false && String.IsNullOrEmpty(StartTimeType4.Text))
                //{
                //    StartTimeType2.Checked = true;
                //}
            }
            else
            {
                StartTimeType1.Checked = false;
                StartTimeType3.Checked = false;
                StartTimeType4.Clear();
                StartTimeHour1.Enabled = true;
                StartTimeHour2.Enabled = true;
                StartTimeHalf.Enabled = true;
                StartTimeETC.Enabled = true;
            }
            SetRemark();
        }

        private void StartTimeType3_CheckedChanged(object sender, EventArgs e)
        {
            if (StartTimeType3.Checked == false)
            {
                //if (StartTimeType1.Checked == false && StartTimeType2.Checked == false && String.IsNullOrEmpty(StartTimeType4.Text))
                //{
                //    StartTimeType3.Checked = true;
                //}
            }
            else
            {
                StartTimeType1.Checked = false;
                StartTimeType2.Checked = false;
                StartTimeType4.Clear();
                StartTimeHour1.Enabled = true;
                StartTimeHour2.Enabled = true;
                StartTimeHalf.Enabled = true;
                StartTimeETC.Enabled = true;
                //StopTimeType1.Checked = false;
            }
            SetRemark();
        }

        private void StartTimeType4_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(StartTimeType4.Text))
            {
                StartTimeType1.Checked = false;
                StartTimeType2.Checked = false;
                StartTimeType3.Checked = false;
                StartTimeHour1.Enabled = true;
                StartTimeHour2.Enabled = true;
                StartTimeHalf.Enabled = true;
                StartTimeETC.Enabled = true;
            }
            SetRemark();
        }

        private void StartTimeHour1_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartTimeHour2.SelectedIndex = 0;
            SetRemark();
        }

        private void StartTimeHour2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void StartTimeHalf_CheckedChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void ClearStarTime_Click(object sender, EventArgs e)
        {
            StartInfo1.Checked = false;
            StartInfo2.Checked = false;
            StartInfo3.Checked = false;
            StartInfo4.Checked = false;
            StartInfo5.Checked = false;
            StartTimeType1.Checked = false;
            StartTimeType2.Checked = false;
            StartTimeType3.Checked = false;
            StartTimeType4.Clear();
            StartTimeHour1.SelectedIndex = 0;
            StartTimeHour2.SelectedIndex = 0;
            StartTimeHalf.Checked = false;
            StartTimeHour1.Enabled = false;
            StartTimeHour2.Enabled = false;
            StartTimeHalf.Enabled = false;

            StartTimeETC.Checked = false;
            StartTimeETC.Enabled = false;
            StopDateHelper.SelectedIndex = 0;
            SetRemark();
        }

        private void StopTimeType1_CheckedChanged(object sender, EventArgs e)
        {
            if (StopTimeType1.Checked)
            {


                //StartTimeType3.Checked = false;
                //StartTimeType4.Clear();




                StopTimeType2.Checked = false;
                StopTimeType3.Checked = false;
                StopTimeType4.Checked = false;
                StopTimeType5.Clear();
                StopTimeHour1.Enabled = true;
                StopTimeHour2.Enabled = true;
                StopTimeHalf.Enabled = true;
                StopDateHelper.SelectedIndex = 1;
            }

            SetRemark();
        }

        private void StopTimeType2_CheckedChanged(object sender, EventArgs e)
        {
            if (StopTimeType2.Checked)
            {
                StopTimeType1.Checked = false;
                StopTimeType3.Checked = false;
                StopTimeType4.Checked = false;
                StopTimeType5.Clear();
                StopTimeHour1.Enabled = true;
                StopTimeHour2.Enabled = true;
                StopTimeHalf.Enabled = true;
                StopDateHelper.SelectedIndex = 2;
            }
            SetRemark();
        }

        private void StopTimeType3_CheckedChanged(object sender, EventArgs e)
        {
            if (StopTimeType3.Checked)
            {
                StopTimeType2.Checked = false;
                StopTimeType1.Checked = false;
                StopTimeType4.Checked = false;
                StopTimeType5.Clear();
                StopTimeHour1.Enabled = true;
                StopTimeHour2.Enabled = true;
                StopTimeHalf.Enabled = true;
                StopDateHelper.SelectedIndex = 3;
            }
            SetRemark();
        }

        private void StopTimeType4_CheckedChanged(object sender, EventArgs e)
        {
            if (StopTimeType4.Checked)
            {
                StopTimeType2.Checked = false;
                StopTimeType3.Checked = false;
                StopTimeType1.Checked = false;
                StopTimeType5.Clear();
                StopTimeHour1.SelectedIndex = 0;
                StopTimeHour2.SelectedIndex = 0;
                StopTimeHalf.Checked = false;
                StopTimeHour1.Enabled = false;
                StopTimeHour2.Enabled = false;
                StopTimeHalf.Enabled = false;
                StopDateHelper.SelectedIndex = 4;
            }
            SetRemark();
        }

        private void StopTimeType5_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(StopTimeType5.Text))
            {
                StopTimeType1.Checked = false;
                StopTimeType2.Checked = false;
                StopTimeType3.Checked = false;
                StopTimeType4.Checked = false;
                StopTimeHour1.Enabled = true;
                StopTimeHour2.Enabled = true;
                StopTimeHalf.Enabled = true;

            }
            SetRemark();
        }

        private void StopTimeHour1_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopTimeHour2.SelectedIndex = 0;
            SetRemark();
        }

        private void StopTimeHour2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void StopTimeHalf_CheckedChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void ClearStopTime_Click(object sender, EventArgs e)
        {
            StopInfo1.Checked = false;
            StopInfo2.Checked = false;
            StopInfo3.Checked = false;
            StopInfo4.Checked = false;
            StopInfo5.Checked = false;
            StopTimeType1.Checked = false;
            StopTimeType2.Checked = false;
            StopTimeType3.Checked = false;
            StopTimeType4.Checked = false;
            StopTimeType5.Clear();
            StopTimeHour1.SelectedIndex = 0;
            StopTimeHour2.SelectedIndex = 0;
            StopTimeHalf.Checked = false;
            StopTimeHour1.Enabled = false;
            StopTimeHour2.Enabled = false;
            StopTimeHalf.Enabled = false;
            SetRemark();
        }

        private void Price_TextChanged(object sender, EventArgs e)
        {
            //decimal nPrice = 0;
            //if (!String.IsNullOrEmpty(Price1.Text))
            //{
            //    nPrice = decimal.Parse(Price1.Text.Replace(",", ""));
            //}
            //if (Customer.Tag != null && CustomerModelList.Any(c => c.CustomerId == (int)Customer.Tag) && CustomerModelList.Find(c => c.CustomerId == (int)Customer.Tag).Fee > 0)
            //{
            //    Price3.Text = ((decimal)nPrice * 0.01m * (decimal)CustomerModelList.Find(c => c.CustomerId == (int)Customer.Tag).Fee).ToString("N0");
            //}
            //decimal nFee = 0;
            //if (!String.IsNullOrEmpty(Price3.Text))
            //{
            //    nFee = decimal.Parse(Price3.Text.Replace(",", ""));
            //}
            //Price2.Text = (nPrice + nFee).ToString("N0");
        }

        private void Fee_TextChanged(object sender, EventArgs e)
        {
            //decimal nPrice = 0;
            //if (!String.IsNullOrEmpty(Price1.Text))
            //{
            //    nPrice = decimal.Parse(Price1.Text.Replace(",", ""));
            //}
            //decimal nFee = 0;
            //if (!String.IsNullOrEmpty(Price3.Text))
            //{
            //    nFee = decimal.Parse(Price3.Text.Replace(",", ""));
            //}
            //Price2.Text = (nPrice + nFee).ToString("N0");
        }

        private void Item_TextChanged(object sender, EventArgs e)
        {
            //  Remark.Text += Item.Text;
            SetRemark();
        }

        private void ItemSize_TextChanged(object sender, EventArgs e)
        {
            if (ValidateIgnore)
                return;
            if (!String.IsNullOrEmpty(ItemSize.Text) && CarSize.SelectedValue != null)
            {
                var CarSizeValue = (int)CarSize.SelectedValue;
                var ItemSizeLimit = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == CarSizeValue).Number * 1.1d;
                if (double.Parse(ItemSize.Text.Replace(",", "")) > ItemSizeLimit)
                {
                    MessageBox.Show($"화물중량은 0보다 크거나 {CarSize.Text}톤까지 입력가능 합니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ItemSize.Clear();
                }
            }
        }

        private void SharedItemLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void SharedItemSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void Emergency_CheckedChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void Reservation_CheckedChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void SetRemark()
        {

            if (!StopTimeType1.Checked && !StopTimeType2.Checked && !StopTimeType3.Checked && !StopTimeType4.Checked)
            {
                StopDateHelper.SelectedIndex = 0;
            }

            var mEtc = "";

            mEtc += Item.Text + " ";


            if (StartTimeType1.Checked)
            {
                mEtc += "지금상 ";
            }
            else if (StartTimeType2.Checked)
            {
                mEtc += "당일";
                if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
                {
                    mEtc += StartTimeHour1.Text + StartTimeHour2.Text;
                    if (StartTimeHalf.Checked)
                    {
                        mEtc += "30분";
                    }

                    if (StartTimeETC.Checked)
                    {
                        mEtc += "이후";
                    }
                }
                mEtc += "상 ";
            }
            else if (StartTimeType3.Checked)
            {
                mEtc += "내일";
                if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
                {
                    mEtc += StartTimeHour1.Text + StartTimeHour2.Text;
                    if (StartTimeHalf.Checked)
                    {
                        mEtc += "30분";
                    }

                    if (StartTimeETC.Checked)
                    {
                        mEtc += "이후";
                    }
                }
                mEtc += "상 ";
            }
            else if (!String.IsNullOrEmpty(StartTimeType4.Text) && StartTimeType4.Text != "0")
            {
                mEtc += $"{StartTimeType4.Text}일";
                if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
                {
                    mEtc += StartTimeHour1.Text + StartTimeHour2.Text;
                    if (StartTimeHalf.Checked)
                    {
                        mEtc += "30분";
                    }

                    if (StartTimeETC.Checked)
                    {
                        mEtc += "이후";
                    }
                }
                mEtc += "상 ";
            }

            if (StopTimeType1.Checked)
            {
                mEtc += "당";
                if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                {
                    mEtc += StopTimeHour1.Text + StopTimeHour2.Text;
                    if (StopTimeHalf.Checked)
                    {
                        mEtc += "30분";
                    }
                }
                mEtc += "착 ";
            }
            else if (StopTimeType2.Checked)
            {
                mEtc += "내일";
                if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                {
                    mEtc += StopTimeHour1.Text + StopTimeHour2.Text;
                    if (StopTimeHalf.Checked)
                    {
                        mEtc += "30분";
                    }
                }
                mEtc += "착 ";
            }
            else if (StopTimeType3.Checked)
            {
                mEtc += "월";
                if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                {
                    mEtc += StopTimeHour1.Text + StopTimeHour2.Text;
                    if (StopTimeHalf.Checked)
                    {
                        mEtc += "30분";
                    }
                }
                mEtc += "착 ";
            }
            else if (StopTimeType4.Checked)
            {
                mEtc += "당착/내착 ";
            }
            else if (!String.IsNullOrEmpty(StopTimeType5.Text) && StopTimeType5.Text != "0")
            {
                mEtc += $"{StopTimeType5.Text}일";
                if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                {
                    mEtc += StopTimeHour1.Text + StopTimeHour2.Text;
                    if (StopTimeHalf.Checked)
                    {
                        mEtc += "30분";
                    }
                }
                mEtc += "착 ";
            }

            //mEtc += Item.Text + " ";

            if (StartInfo1.Checked || StartInfo2.Checked || StartInfo3.Checked || StartInfo4.Checked || StartInfo5.Checked
                || StopInfo1.Checked || StopInfo2.Checked || StopInfo3.Checked || StopInfo4.Checked || StopInfo5.Checked)
            {
                if (StartInfo1.Checked)
                {
                    mEtc += "지";
                }
                else if (StartInfo2.Checked)
                {
                    mEtc += "수";
                }
                else if (StartInfo3.Checked)
                {
                    mEtc += "호";
                }
                else if (StartInfo4.Checked)
                {
                    mEtc += "크";
                }
                else if (StartInfo5.Checked)
                {
                    mEtc += "컨";
                }
                mEtc += "/";
                if (StopInfo1.Checked)
                {
                    mEtc += "지";
                }
                else if (StopInfo2.Checked)
                {
                    mEtc += "수";
                }
                else if (StopInfo3.Checked)
                {
                    mEtc += "호";
                }
                else if (StopInfo4.Checked)
                {
                    mEtc += "크";
                }
                else if (StopInfo5.Checked)
                {
                    mEtc += "컨";
                }
            }

            if (Shared.Checked)
            {
                mEtc += "혼적";
                if (SharedItemLength.SelectedIndex > 0)
                    mEtc += SharedItemLength.Text;
                if (SharedItemSize.SelectedIndex > 0)
                    mEtc += SharedItemSize.Text;
                mEtc += " ";
            }
            if (Emergency.Checked)
            {
                mEtc += "긴급 ";
            }

            if (Round.Checked)
            {
                mEtc += "왕복 ";
            }
            if (Reservation.Checked)
            {
                mEtc += "예약 ";
            }

            mEtc = mEtc.Trim();
            Remark.Text = mEtc;
        }

        private void Number_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
            ((TextBox)sender).SelectAll();
        }

        private void Number_Leave(object sender, EventArgs e)
        {
            var Number = sender as TextBox;
            if (String.IsNullOrEmpty(Number.Text))
            {
                if (Number == CarCount)
                {
                    Number.Text = "1";
                }
                else
                {
                    Number.Text = "0";
                }
            }
            else
            {
                if (Number == ItemSize)
                {
                    double value = 0;
                    try
                    {
                        value = double.Parse(Number.Text.Replace(",", ""));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("화물 중량을 바르게 입력해주세요.", "차세로");
                        Number.Text = "0";
                    }
                    if (value == Math.Floor(value))
                    {
                        Number.Text = value.ToString("N0");
                    }
                    else
                    {
                        Number.Text = value.ToString();
                    }
                }
                else if (Number == StartTimeType4)
                {
                    int _Number = 0;
                    if (String.IsNullOrEmpty(Number.Text))
                    {
                        return;
                    }
                    _Number = int.Parse(Number.Text);
                    if (_Number > 31 || _Number < 1)
                    {
                        MessageBox.Show("상차일시를 바르게 입력해주세요.", "차세로");
                        Number.Focus();

                    }

                }
                else if (Number == StopTimeType5)
                {
                    int _Number = 0;
                    if (String.IsNullOrEmpty(Number.Text))
                    {
                        return;
                    }
                    _Number = int.Parse(Number.Text);
                    if (_Number > 31 || _Number < 1)
                    {
                        MessageBox.Show("하차일시를 바르게 입력해주세요.", "차세로");
                        Number.Focus();

                    }
                }

                else
                {
                    Number.Text = int.Parse(Number.Text.Replace(",", "")).ToString("N0");
                }
            }
        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',')
            {
                var Number = sender as TextBox;

                Number.Text += "000";

            }

        }

        private void NewOrder_Click(object sender, EventArgs e)
        {
            grid1.CurrentCell = null;
            IsCurrentNull = true;
            _NewOrder();
          
            PayLocation.SelectedIndex = 1;
            Customer.Focus();
            // UpdateOrder.Enabled = false;
            // Customer.Enabled = true;
        }

        private void _NewOrder()
        {
           
            StartState.Text = "";
            StartCity.Text = "";
            StartStreet.Text = "";
            Start.Clear();
            StartName.Clear();
            StopState.Text = "";
            StopCity.Text = "";
            StopStreet.Text = "";
            Stop.Clear();
            StopName.Clear();
            CarSize.SelectedValue = 1;
            StopDateHelper.SelectedIndex = 0;
            CarCount.Text = "1";
            PayLocation.SelectedIndex = 0;
          //  Price1.Text = "0";
            Price2.Text = "0";
            Price3.Text = "0";
            lblPrice4.Text = "0";
            TotalPrice.Text = "0";
           // Price6.Text = "0";
            Item.Text = "";
            ItemSize.Text = "0";
            ItemSizeInclude.Checked = false;
            NotShared.Checked = true;
            Emergency.Checked = false;
            Round.Checked = false;
            Reservation.Checked = false;
            StartInfo1.Checked = false;
            StartInfo2.Checked = false;
            StartInfo3.Checked = false;
            StartInfo4.Checked = false;
            StartInfo5.Checked = false;
            ClearStarTime_Click(null, null);
            ClearStopTime_Click(null, null);
            Customer.Clear();
            Customer.Tag = null;
            OrderPhoneNo.Clear();
            StopPhoneNo.Clear();
            //Driver.Clear();
            //Driver.Tag = null;
            //DriverPhoneNo.Text = "";
            //DriverCarNo.Text = "";
            DriverPoint.Checked = false;
            HasTrade.Checked = false;
            HasSales.Checked = false;
            StartMemo.Clear();
            StopMemo.Clear();
            RequestMemo.Clear();
            CustomerManager.Clear();
           // cmb_ReferralId.SelectedIndex = 0;
            DriverGrade.SelectedIndex = 0;
            StartPhoneNo.Clear();
            //txtJusun.Clear();
            //DriverName.Text = "";
            // chkMyCarOrder.Checked = false;
            PayLocation.Enabled = true;
            Customer.Enabled = true;
            btnPrev.Enabled = true;
            StartMulti.Checked = false;
            StopMulti.Checked = false;

            UnitItem.Clear();
            UnitType.SelectedIndex = 0;

            CustomerTeam.Text = "";


        }

      
        private void StopDateHelper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StopDateHelper.SelectedIndex == 0)
            {
                StopTimeType1.Checked = false;
                StopTimeType2.Checked = false;
                StopTimeType3.Checked = false;
                StopTimeType4.Checked = false;
                StopTimeType5.Text = "";
                StopTimeHour1.SelectedIndex = 0;
                StopTimeHour2.SelectedIndex = 0;
                StopTimeHalf.Checked = false;
                StopTimeHour1.Enabled = false;
                StopTimeHour2.Enabled = false;
                StopTimeHalf.Enabled = false;
            }
            else if (StopDateHelper.SelectedIndex == 1)
            {
                StopTimeType1.Checked = true;
            }
            else if (StopDateHelper.SelectedIndex == 2)
            {
                StopTimeType2.Checked = true;
            }
            else if (StopDateHelper.SelectedIndex == 3)
            {
                StopTimeType3.Checked = true;
            }
            else if (StopDateHelper.SelectedIndex == 4)
            {
                StopTimeType4.Checked = true;
            }
        }

        private void CarSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] SmallCarTypeValues = new int[]{
                2, 3,74,75
            };
            int[] MediumCarTypeValues = new int[]{
                0,1,4,5,6,8,9,10,14,16,18,20,21,60,61,62,22,63,64,65,66,67,27,68,69,33,34,70,71,72,73,75,76
            };
            int[] FiveCarTypeValues = new int[]{
                0,1,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,72,73,75,76
            };
            int[] LargeCarTypeValues = new int[]{
                0,1,4,5,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,72,73,75,76
            };

            int CarSizeValue = (int)(CarSize.SelectedValue ?? 0);
            if (CarSizeValue == 1)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 2;
            }
            else if (CarSizeValue == 2)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 3;
            }
            else if (CarSizeValue == 17)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 3;
            }
            else if (new int[] { 3, 4, 5, 6 }.Contains(CarSizeValue))
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && MediumCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 0;
            }
            else if (new int[] { 7 }.Contains(CarSizeValue))
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && FiveCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 0;
            }
            else if (CarSizeValue > 7 && CarSizeValue != 17)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && LargeCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 0;
            }
            SharedItemSize.DisplayMember = "Name";
            SharedItemSize.ValueMember = "Value";
            double Number = ((CarSize.SelectedItem as StaticOptionsRow)?.Number) ?? 0;
            SharedItemSize.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "SharedItemSize" && c.Number <= Number).OrderBy(c => c.Seq).ToList());
        }

      

        private void DataList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < DataListSource.Count)
            {
                var Current = DataListSource[e.RowIndex] as Order;

                if (e.ColumnIndex == ColumnNum.Index)
                {
                    grid1[e.ColumnIndex, e.RowIndex].Value = (grid1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
                }


                if (e.ColumnIndex == ColumnNumber.Index)
                {
                    String OrderNo = Current.OrderId.ToString("00000000");
                    OrderNo = OrderNo.Substring(OrderNo.Length - 8);
                    OrderNo = OrderNo.Substring(0, 4) + "-" + OrderNo.Substring(4);
                    e.Value = OrderNo;
                }
                //else if (e.ColumnIndex == ColumnSubClientId.Index)
                //{
                //    if (LocalUser.Instance.LogInInformation.AllowSub)
                //    {
                //        string R = "";
                //        if (!String.IsNullOrEmpty(Current.OrdersLoginId))
                //        {

                //            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                //            {
                //                _Connection.Open();
                //                using (SqlCommand _Command = _Connection.CreateCommand())
                //                {
                //                    _Command.CommandText =
                //                        @"SELECT  CEO FROM  Clients
                //                    WHERE LoginId = @LoginId  ";
                //                    _Command.Parameters.AddWithValue("@LoginId", Current.OrdersLoginId);
                //                    object O = _Command.ExecuteScalar();

                //                    if (O != null)
                //                    {
                //                        R = "본점";
                //                    }
                //                    else
                //                    {
                //                        _Command.CommandText =
                //                          @"SELECT  ISNULL(SubClients.Name,'본점')as NAME FROM  ClientUsers
                //                      LEFT join SubClients ON ClientUsers.SubClientId = SubClients.SubClientId
                //                            WHERE ClientUsers.LoginId = @LoginId  ";


                //                        object U = _Command.ExecuteScalar();

                //                        if (U != null)
                //                        {
                //                            R = U.ToString();
                //                        }
                //                    }
                //                }
                //                _Connection.Close();
                //            }

                //            e.Value = R;
                //        }
                //        else
                //        {
                //            e.Value = "";

                //        }
                //    }

                //}

                else if (e.ColumnIndex == CustomerTeamColumn.Index)
                {
                    if (Current.CustomerTeam != 0)
                    {
                        string R = "";
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                    @"SELECT TeamName FROM CustomerTeams WHERE CustomerTeamId = @CustomerTeamId  ";
                                _Command.Parameters.AddWithValue("@CustomerTeamId", Current.CustomerTeam);
                                object O = _Command.ExecuteScalar();

                                if (O != null)
                                {
                                    R = O.ToString();
                                }
                                else
                                {
                                    R = "본사";

                                }
                            }
                            _Connection.Close();
                        }

                        e.Value = R;
                    }
                    else
                    {
                        e.Value = "본사";

                    }
                }
                else if (e.ColumnIndex == ColumnStart.Index)
                {

                    e.Value = Current.StartName;
                }
                else if (e.ColumnIndex == ColumnStop.Index)
                {

                    e.Value = Current.StopName;
                }


                else if (e.ColumnIndex == ColumnOrderStatus.Index)
                {
                    switch (Current.OrderStatus)
                    {
                        case 0:
                            e.Value = "취소";
                            break;
                        case 1:
                            if (Current.CreateTime.Date > DateTime.Now.Date)
                            {
                                e.Value = "예약";
                            }
                            else
                            {
                                e.Value = "접수";
                            }

                            break;
                        case 2:
                            e.Value = "대기";
                            break;
                        case 3:
                            e.Value = "완료";


                            break;
                    }
                }


                else if (e.ColumnIndex == ColumnPayLocation.Index)
                {
                    if (Current.PayLocation != 0)
                    {
                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "PayLocation" && c.Value == Current.PayLocation))
                            e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "PayLocation" && c.Value == Current.PayLocation).Name;
                    }
                }
                else if (e.ColumnIndex == ColumnCarSize.Index)
                {
                    if (Current.CarSize != 0)
                    {
                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == Current.CarSize))
                            e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == Current.CarSize).Name;
                    }
                }
                else if (e.ColumnIndex == ColumnCarType.Index)
                {
                    if (Current.CarType != 0)
                    {
                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == Current.CarType))
                            e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == Current.CarType).Name;
                    }
                }
                else if (e.ColumnIndex == ColumnShared.Index)
                {
                    if (Current.IsShared)
                    {
                        e.Value = "혼적";
                    }
                    else
                    {
                        e.Value = "독차";
                    }
                }
                else if (e.ColumnIndex == ColumnItemInfo.Index)
                {
                    e.Value = "";
                    if (Current.IsShared)
                    {
                        List<String> ValueTextList = new List<string>();
                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "SharedItemLength" && c.Value == Current.SharedItemLength))
                            ValueTextList.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "SharedItemLength" && c.Value == Current.SharedItemLength).Name);
                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "SharedItemSize" && c.Value == Current.SharedItemSize))
                            ValueTextList.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "SharedItemSize" && c.Value == Current.SharedItemSize).Name);
                        e.Value = String.Join("/", ValueTextList);
                    }
                }
                else if (e.ColumnIndex == ColumnDriverCarSize.Index)
                {
                    if (Current.DriverId > 0)
                    {
                        var drivers = _DriverTable.Where(c => c.DriverId == Current.DriverId).ToArray();

                        if (drivers.Any())
                        {
                            if (drivers.First().CarSize != 0)
                            {
                                if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == drivers.First().CarSize))
                                    e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == drivers.First().CarSize).Name;
                            }

                        }


                    }


                }
                else if (e.ColumnIndex == ColumnDriverCarType.Index)
                {
                    if (Current.DriverId > 0)
                    {
                        var drivers = _DriverTable.Where(c => c.DriverId == Current.DriverId).ToArray();

                        if (drivers.Any())
                        {
                            if (drivers.First().CarType != 0)
                            {
                                if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == drivers.First().CarType))
                                    e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == drivers.First().CarType).Name;
                            }
                        }
                    }


                }




                else if (e.ColumnIndex == Column_DriverCarNo.Index)
                {


                }
                else if (e.ColumnIndex == ColumnTotalPrice.Index)
                {
                    e.Value = Current.SalesPrice + Current.StartPrice + Current.StopPrice + Current.AlterPrice;
                }

              
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {



            DateTime eDate = DateFilterEnd.Value.Date;
            DateTime sDate = DateFilterBegin.Value.AddMonths(3).Date;


            //if (eDate >= sDate)
            //{
            //    MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //} 

            _MNSTOrderId = 0;
            _MGubun = "";

            if (DateFilterEnd.Value.AddMonths(-3).Date >= DateFilterBegin.Value.Date || eDate >= sDate)
            {
                MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            _Sort = false;
            _Search();
        }

        
        
        


        private void DataListSource_CurrentChanged(object sender, EventArgs e)
        {

            MethodProcess = true;
            _NewOrder();
            if (DataListSource.Current != null)
            {
              //  btnEnable();

                var Current = DataListSource.Current as Order;

                ModelToView(Current);
                //_ORDERCODE = Current.OrderId.ToString();


                if (Current.Agubun == 4)
                {
                    Customer.ReadOnly = true;
                }
                else
                {
                    Customer.ReadOnly = false;
                }



                

                //if (Current.SalesManageId != null)
                //{

                //    //지불금액
                //    if (Current.OrderStatus == 3)
                //    {
                //        DefaultPrice.Enabled = false;
                //    }
                //    else
                //    {
                //        DefaultPrice.Enabled = true;
                //    }
                //}

                //차주세금계산서 화주세금계산서 둘중하나라고 발행되면 운송비구분 수정 안되게
                if (Current.SalesManageId == null && Current.TradeId == null)
                {
                    if (Current.PayLocation == 5) { PayLocation.Enabled = false; }
                    else
                    {

                        PayLocation.Enabled = true;
                    }
                }
                else
                {


                    PayLocation.Enabled = false;
                }

                //주선공유일때 보관금 입력
                if (Current.MyCarOrder == true)
                {

                    if (Price3.Enabled == false)
                    {
                        Price3.Enabled = true;
                    }

                }

                //주선공유배차
                if (Current.MyCarOrder == false && Current.OrderStatus == 3 && Current.FOrderId != 0)
                {

                    //보관금 수정불가
                    if (Price3.Enabled == true)
                    {
                        Price3.Enabled = false;
                    }

                }



                IsCurrentNull = false;


            }
            MethodProcess = false;
        }

        private void ModelToView(Order Current)
        {
          
            StartState.Text = Current.StartState;
            StartCity.Text = Current.StartCity;
            StartStreet.Text = Current.StartStreet;
            Start.Text = Current.StartDetail;

            StopState.Text = Current.StopState;
            StopCity.Text = Current.StopCity;
            StopStreet.Text = Current.StopStreet;
            Stop.Text = Current.StopDetail;

            CarSize.SelectedValue = Current.CarSize;
            CarType.SelectedValue = Current.CarType;
            if (Current.StopDateHelper == null)
                StopDateHelper.SelectedValue = 0;
            else
                StopDateHelper.SelectedValue = Current.StopDateHelper;
            CarCount.Text = Current.CarCount.ToString();
            PayLocation.SelectedValue = Current.PayLocation;
            //Price1.Text = (Current.TradePrice ?? 0).ToString("N0");
            Price2.Text = (Current.SalesPrice ?? 0).ToString("N0");
            Price3.Text = (Current.AlterPrice ?? 0).ToString("N0");
            //Price4.Text = (Current.StartPrice ?? 0).ToString("N0");
            //Price5.Text = (Current.StopPrice ?? 0).ToString("N0");
          
            
            lblPrice4.Text = (Current.StopPrice ?? 0 + Current.StartPrice ?? 0).ToString("N0");

            TotalPrice.Text = (Current.SalesPrice ?? 0+ Current.AlterPrice ?? 0 + Current.StopPrice ?? 0 + Current.StartPrice ?? 0).ToString("N0");
            //Price6.Text = (Current.DriverPrice ?? 0).ToString("N0");

            Item.Text = Current.Item;
            Remark.Text = Current.Remark;

            if (Current.IsShared)
            {
                Shared.Checked = true;
                SharedItemLength.SelectedValue = Current.SharedItemLength ?? 0;
                SharedItemSize.SelectedValue = Current.SharedItemSize ?? 0;
            }
            else
            {
                NotShared.Checked = true;
                SharedItemLength.SelectedIndex = 0;
                SharedItemSize.SelectedIndex = 0;
            }
            Emergency.Checked = Current.Emergency == true;
            Round.Checked = Current.Round == true;
            Reservation.Checked = Current.Reservation == true;

            ClearStarTime_Click(null, null);
            if (Current.StartInfo == "지게차")
            {
                StartInfo1.Checked = true;
            }
            else if (Current.StartInfo == "수작업")
            {
                StartInfo2.Checked = true;
            }
            else if (Current.StartInfo == "호이스트")
            {
                StartInfo3.Checked = true;
            }
            else if (Current.StartInfo == "크레인")
            {
                StartInfo4.Checked = true;
            }
            else if (Current.StartInfo == "컨베이어")
            {
                StartInfo5.Checked = true;
            }
            if (Current.StartTimeType == 1)
            {
                StartTimeType1.Checked = true;
            }
            else if (Current.StartTimeType == 2)
            {
                StartTimeType2.Checked = true;
                if (Current.StartTimeHour > 0)
                {
                    if (Current.StartTimeHour > 12)
                    {
                        StartTimeHour1.SelectedIndex = 2;
                        StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value - 12;
                    }
                    else
                    {
                        StartTimeHour1.SelectedIndex = 1;
                        StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value;
                    }
                    StartTimeHalf.Checked = Current.StartTimeHalf == true;
                    StartTimeETC.Checked = Current.StartTimeETC == true;
                }
            }
            else if (Current.StartTimeType == 3)
            {
                StartTimeType3.Checked = true;
                if (Current.StartTimeHour > 0)
                {
                    if (Current.StartTimeHour > 12)
                    {
                        StartTimeHour1.SelectedIndex = 2;
                        StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value - 12;
                    }
                    else
                    {
                        StartTimeHour1.SelectedIndex = 1;
                        StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value;
                    }
                    StartTimeHalf.Checked = Current.StartTimeHalf == true;
                    StartTimeETC.Checked = Current.StartTimeETC == true;
                }
            }
            else if (Current.StartTimeType > 1000)
            {
                StartTimeType4.Text = (Current.StartTimeType - 1000).ToString();
                if (Current.StartTimeHour > 0)
                {
                    if (Current.StartTimeHour > 12)
                    {
                        StartTimeHour1.SelectedIndex = 2;
                        StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value - 12;
                    }
                    else
                    {
                        StartTimeHour1.SelectedIndex = 1;
                        StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value;
                    }
                    StartTimeHalf.Checked = Current.StartTimeHalf == true;
                    StartTimeETC.Checked = Current.StartTimeETC == true;
                }
            }

            ClearStopTime_Click(null, null);
            if (Current.StopInfo == "지게차")
            {
                StopInfo1.Checked = true;
            }
            else if (Current.StopInfo == "수작업")
            {
                StopInfo2.Checked = true;
            }
            else if (Current.StopInfo == "호이스트")
            {
                StopInfo3.Checked = true;
            }
            else if (Current.StopInfo == "크레인")
            {
                StopInfo4.Checked = true;
            }
            else if (Current.StopInfo == "컨베이어")
            {
                StopInfo5.Checked = true;
            }
            if (Current.StopTimeType == 1)
            {
                StopTimeType1.Checked = true;
                if (Current.StopTimeHour > 0)
                {
                    if (Current.StopTimeHour > 12)
                    {
                        StopTimeHour1.SelectedIndex = 2;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
                    }
                    else
                    {
                        StopTimeHour1.SelectedIndex = 1;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
                    }
                    StopTimeHalf.Checked = Current.StopTimeHalf == true;
                }
            }
            else if (Current.StopTimeType == 2)
            {
                StopTimeType2.Checked = true;
                if (Current.StopTimeHour > 0)
                {
                    if (Current.StopTimeHour > 12)
                    {
                        StopTimeHour1.SelectedIndex = 2;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
                    }
                    else
                    {
                        StopTimeHour1.SelectedIndex = 1;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
                    }
                    StopTimeHalf.Checked = Current.StopTimeHalf == true;
                }
            }
            else if (Current.StopTimeType == 3)
            {
                StopTimeType3.Checked = true;
                if (Current.StopTimeHour > 0)
                {
                    if (Current.StopTimeHour > 12)
                    {
                        StopTimeHour1.SelectedIndex = 2;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
                    }
                    else
                    {
                        StopTimeHour1.SelectedIndex = 1;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
                    }
                    StopTimeHalf.Checked = Current.StopTimeHalf == true;
                }
            }
            else if (Current.StopTimeType == 4)
            {
                StopTimeType4.Checked = true;
            }
            else if (Current.StopTimeType > 1000)
            {

                StopTimeType5.Text = (Current.StopTimeType - 1000).ToString();
                if (Current.StopTimeHour > 0)
                {
                    if (Current.StopTimeHour > 12)
                    {
                        StopTimeHour1.SelectedIndex = 2;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
                    }
                    else
                    {
                        StopTimeHour1.SelectedIndex = 1;
                        StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
                    }
                    StopTimeHalf.Checked = Current.StopTimeHalf == true;
                }
            }



            OrderPhoneNo.Text = Current.OrderPhoneNo;
            StopPhoneNo.Text = Current.StopPhoneNo;
            StartPhoneNo.Text = Current.StartPhoneNo;
            ItemSizeInclude.Checked = Current.ItemSizeInclude == true;
            ItemSize.Text = Current.ItemSize;


            DriverName.Text = Current.Driver;
            DriverPhoneNo.Text = Current.DriverPhoneNo;
            DriverCarNo.Text = Current.DriverCarNo;


          
         

            HasTrade.Checked = Current.TradeId != null;
            HasSales.Checked = Current.SalesManageId != null;
            DriverPoint.Checked = Current.DriverPoint != null;

            StartName.Text = Current.StartName;
            StopName.Text = Current.StopName;
            StartMemo.Text = Current.StartMemo;
            StopMemo.Text = Current.StopMemo;

            //RequestMemo.Text = Current.RequestMemo;
            //if (Current.ReferralId)

            DateTime Now = DateTime.Now.AddDays(1).Date;
            DateTime Tommorow = DateTime.Now.AddYears(-1).Date;

            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                string _Grade = "0";
                var _Q = ShareOrderDataSet.Orders.Where(c => c.DriverId == Current.DriverId && c.CreateTime >= Tommorow && c.CreateTime < Now && (c.DriverGrade != 0 && c.DriverGrade != null));
                if (_Q.Any())
                {
                    _Grade = _Q.Average(c => c.DriverGrade ?? 0).ToString("N0");
                }



                //lblGrade.Text = "★ : " + _Grade;


            }





            SetCustomerList();
            if (Current.DriverGrade == null)
            {
                DriverGrade.SelectedValue = 0;

            }
            else
            {
                DriverGrade.SelectedValue = Current.DriverGrade;
            }



            //var Query = CustomerModelList.Where(c => c.CustomerId == Current.ReferralId).ToArray();
            //if (Query.Any())
            //{
            //    if (Query.First().BizGubun == 3)
            //    {
            //        //rdoJusun.Checked = true;
            //        //txtJusun.Tag = Current.ReferralId;
            //        //txtJusun.Text = Query.First().SangHo;
            //        //txtJusun.Visible = true;
            //        //cmb_ReferralId.Visible = false;
            //    }
            //    else
            //    {
            //        // rdoWe.Checked = true;
            //        cmb_ReferralId.Visible = true;
            //        cmb_ReferralId.SelectedValue = Current.ReferralId;

            //        // txtJusun.Visible = false;
            //    }

            //}
            //else
            //{
            //    //rdoWe.Checked = true;
            //    cmb_ReferralId.Visible = true;


            //    // txtJusun.Visible = false;
            //}
            if (Current.MyCarOrder == true)
            {
                chkMyCarOrder.Checked = true;
            }
            else
            {
                chkMyCarOrder.Checked = false;
            }

            //접수운송사,배차운송사 같으면 True 틀리면 False
            bool _MyOrder = false;
            //접수운송사ID
            int _MyCarClientId = 0;
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                //타사에서 배차했을때
                if (Current.FOrderId != 0)
                {
                    //타사공유배차정보
                    var Query2 = ShareOrderDataSet.Orders.Where(c => c.OrderId == Current.FOrderId).ToArray();


                    if (Query2.Any())
                    {
                        //배차운송사ID와 로그인한운송사 아이디가 같으면
                        if (Query2.First().ClientId == LocalUser.Instance.LogInInformation.ClientId)
                        {
                            //접수운송사와 배차운송사가 같음
                            _MyOrder = true;

                        }
                        else
                        {   //접수운송사와 배차운송사가 틀림
                            _MyOrder = false;
                        }

                        //배차한 운송사ID
                        _MyCarClientId = (int)Query2.First().ClientId;
                    }
                }
                //배차전 또는 내가 배차했을때
                else
                {
                    //접수운송사가 타사가 아니면 
                    if (Current.ClientId == LocalUser.Instance.LogInInformation.ClientId)
                    {
                        _MyOrder = true;
                    }
                    else
                    {
                        _MyOrder = false;

                    }

                }


            }
            //공유 /타사접수일때
            if (Current.MyCarOrder == true && !_MyOrder)
            {


                TotalPrice.Text = (Current.TradePrice ?? 0).ToString("N0");
                //Price1.Text = "0";


                //배차 완료
                if (Current.OrderStatus == 3)
                {
                    //접수운송사ID //타사에서 배차
                    if (_MyCarClientId != 0)
                    {
                        var _ClientsList = _ClientTable.Where(c => c.ClientId == _MyCarClientId).First();
                        Customer.Text = _ClientsList.Name;
                        OrderPhoneNo.Text = _ClientsList.PhoneNo;
                        CustomerTeam.Text = ""; 

                    }
                    //자차배차
                    else
                    {
                        if (Current.CustomerId != null && !string.IsNullOrEmpty(Current.Customer))
                        {
                            Customer.Tag = Current.CustomerId;
                            Customer.Text = Current.Customer;


                            
                        }
                        else
                        {
                            Customer.Tag = null;
                            Customer.Text = Current.Customer;


                        }

                      

                    }
                }
                //배차전
                else
                {
                    var _ClientsList = _ClientTable.Where(c => c.ClientId == Current.ClientId).First();
                    Customer.Text = _ClientsList.Name;
                    OrderPhoneNo.Text = _ClientsList.PhoneNo;


                    //if (Current.CustomerId != null && !string.IsNullOrEmpty(Current.Customer))
                    //{
                    //    Customer.Tag = Current.CustomerId;
                    //    Customer.Text = Current.Customer;
                    //}
                    //else
                    //{
                    //    Customer.Tag = null;
                    //    Customer.Text = Current.Customer;


                    //}

                }
                Customer.Enabled = false;
                OrderPhoneNo.Enabled = false;
                btnPrev.Enabled = false;
                CustomerManager.Enabled = false;


            }
            //로그인운송사접수
            if (Current.ClientId == LocalUser.Instance.LogInInformation.ClientId && _MyOrder)
            {
                if (Current.CustomerId != null && !string.IsNullOrEmpty(Current.Customer))
                {
                    Customer.Tag = Current.CustomerId;
                    Customer.Text = Current.Customer;
                }
                else
                {
                    Customer.Tag = null;
                    Customer.Text = Current.Customer;


                }

                Customer.Enabled = true;
                OrderPhoneNo.Enabled = true;
                btnPrev.Enabled = true;
                CustomerManager.Enabled = true;
            }
            if (Current.CustomerTeam != 0)
            {
                var cmbCustomerTeamataSource = customerUserDataSet.CustomerTeams.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerTeamId == Current.CustomerTeam).ToArray();

                if (cmbCustomerTeamataSource.Any())
                {
                    CustomerTeam.Text = cmbCustomerTeamataSource.First().TeamName;
                }
            }
            else
            {
                CustomerTeam.Text = "본사";
            }

            if (Current.StartMulti == true)
            {
                StartMulti.Checked = true;
            }
            else
            {
                StartMulti.Checked = false;
            }

            if (Current.StopMulti == true)
            {
                StopMulti.Checked = true;
            }
            else
            {
                StopMulti.Checked = false;
            }

            CustomerManager.Text = Current.CustomerManager;

            //dtpRequestDate.Value = Current.CreateTime;

            UnitItem.Text = Current.UnitItem;
            UnitType.SelectedValue = Current.UnitType;

            _AddressGubun = Current.AddressGubun;

        }

        

        private void AllSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
           
            SearchText.Clear();
            _Sort = false;
            _Search();
        }

        private void OrderSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
           
            SearchText.Clear();
            _Sort = false;
            _Search(1);
        }

        private void AcceptSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
           
            SearchText.Clear();
            _Sort = false;
            _Search(3);
        }

        

        

        private void btnOrderAfterAdd_Click(object sender, EventArgs e)
        {





            FrmMN0301Add _Form = new FrmMN0301Add
            {
                Owner = this,
                StartPosition = FormStartPosition.CenterParent
            };
            //if (_Form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    initReferralId();
            //}
            if (_Form.ShowDialog() == System.Windows.Forms.DialogResult.No)
            {
                Exceldefault();
            }
            else

            { }



            _Sort = false;
            _Search();
        }

        private void Customer_Leave(object sender, EventArgs e)
        {




            if (Customer.Tag != null && !String.IsNullOrWhiteSpace(Customer.Text))
            {
                if (CustomerModelList.Any(c => c.CustomerId == (int)Customer.Tag))
                {
                    Customer.Text = CustomerModelList.Find(c => c.CustomerId == (int)Customer.Tag).SangHo;
                }
            }
            else
            {
                Customer.Text = "";
                Customer.Tag = null;
                OrderPhoneNo.Clear();
                CustomerManager.Clear();

            }
        }

        private void PayLocation_DropDownClosed(object sender, EventArgs e)
        {
            int PayLocationPoint = 2;
            if ((int)PayLocation.SelectedValue == PayLocationPoint)
            {
                // DriverPoint.Checked = true;
            }
            else
            {
                //DriverPoint.Checked = false;
            }
            ////if (LocalUser.Instance.LogInInformation.Client.CustomerPay)
            ////    return;
            //if (DataListSource.Current == null || (DataListSource.Current is Order _Order && _Order.PayLocation != (int)PayLocation.SelectedValue))
            //{
            //    if ((int)PayLocation.SelectedValue == PayLocationPoint)
            //    {
            //        DriverPoint.Checked = true;
            //    }
            //    else
            //    {
            //        DriverPoint.Checked = false;
            //    }
            //}
        }

        private void DataList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            if (IsCurrentNull)
            {
                DataListSource_CurrentChanged(null, null);
            }
        }

        private void DataList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var CurrentOrder = DataListSource[e.RowIndex] as Order;

            if (CurrentOrder == null)
            {
                return;
            }

          

         


        }



        private string _AddressStateParse(String _Value)
        {
            string R = "";
            switch (_Value)
            {
                case "강원도":
                    R = "강원";
                    break;
                case "경기도":
                    R = "경기";
                    break;
                case "경상남도":
                    R = "경남";
                    break;
                case "경상북도":
                    R = "경북";
                    break;
                case "광주광역시":
                    R = "광주";
                    break;
                case "대구광역시":
                    R = "대구";
                    break;
                case "대전광역시":
                    R = "대전";
                    break;
                case "부산광역시":
                    R = "부산";
                    break;
                case "서울특별시":
                    R = "서울";
                    break;
                case "세종특별자치시":
                    R = "세종";
                    break;
                case "울산광역시":
                    R = "울산";
                    break;
                case "인천광역시":
                    R = "인천";
                    break;
                case "전라남도":
                    R = "전남";
                    break;
                case "전라북도":
                    R = "전북";
                    break;
                case "제주특별자치도":
                    R = "제주";
                    break;
                case "충청남도":
                    R = "충남";
                    break;
                case "충청북도":
                    R = "충북";
                    break;
                default:
                    break;
            }
            return R;
        }

        private string _AddressCityParse(String _Value)
        {
            string R = _Value;
            if (_Value == "제주시")
            {
                R = "제주시";
            }
            else
            {
                if (_Value.Length > 2)
                    R = _Value.Substring(0, _Value.Length - 1);
                //var _Values = _Value.Split(' ');

                //if (_Values.Length > 1)
                //{
                //    if (_Values[0].Length > 2)
                //        R = _Values[0].Substring(0, _Values[0].Length - 1)+" "+ _Values[1]


                //}
                //else
                //{
                //    if (_Value.Length > 2)
                //        R = _Value.Substring(0, _Value.Length - 1);
                //}


            }
            return R;

            //string R = _Value;
            //if (_Value.Length > 2)
            //    R = _Value.Substring(0, _Value.Length - 1);
            //return R;
        }

        private string _AddressCityParse24(String _Value)
        {
            string R = _Value;


            if (_Value.Length > 2)
                R = _Value.Substring(0, _Value.Length - 1);



            return R;

            //string R = _Value;
            //if (_Value.Length > 2)
            //    R = _Value.Substring(0, _Value.Length - 1);
            //return R;
        }


        private void SearchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                _Sort = false;
                _Search();
            }
        }

        private void Remark_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Price_Enter(object sender, EventArgs e)
        {
            Number_Enter(sender, e);
            //Price1.SelectAll();
        }

        private void Fee_Enter(object sender, EventArgs e)
        {
            Number_Enter(sender, e);
           // Price3.SelectAll();
        }

        private void Price_Click(object sender, EventArgs e)
        {
            Price_Enter(sender, e);
        }

        private void Fee_Click(object sender, EventArgs e)
        {
            Fee_Enter(sender, e);
        }

        public void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;


            

            var fileString = "배차관리전체정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible)
                {
                    ColumnHeaderMap.Add(grid1.Columns[i].HeaderText);
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
            for (int i = 0; i < grid1.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = grid1[ColumnIndexMap[j], i].FormattedValue?.ToString();

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
            //if (printDialog.ShowDialog() == DialogResult.Yes)
            //{
            //    var fileString = "배차관리전체정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //            return;
            //    }
            //    using (MemoryStream _Stream = new MemoryStream(Properties.Resources.Order_Default))
            //    {
            //        _Stream.Seek(0, SeekOrigin.Begin);
            //        _Excel = new ExcelPackage(_Stream);
            //    }
            //    var _Sheet = _Excel.Workbook.Worksheets[1];
            //    var RowIndex = 2;
            //    var ColumnIndexMap = new Dictionary<int, int>();
            //    var ColumnIndex = 0;
            //    for (int i = 0; i < grid1.ColumnCount; i++)
            //    {
            //        if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { ColumnImageA, CustomerSms, btnSms }.Contains(grid1.Columns[i])))
            //        {
            //            ColumnIndexMap.Add(ColumnIndex, i);
            //            ColumnIndex++;
            //        }
            //    }
            //    for (int i = 0; i < grid1.RowCount; i++)
            //    {
            //        for (int j = 0; j < ColumnIndexMap.Count; j++)
            //        {

            //            _Sheet.Cells[RowIndex, j + 1].Value = grid1[ColumnIndexMap[j], i].FormattedValue?.ToString();
            //        }
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
            //else if (printDialog.DialogResult == DialogResult.OK)
            //{
            //    var fileString = "배차정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //            return;
            //    }
            //    using (MemoryStream _Stream = new MemoryStream(Properties.Resources.Order_List))
            //    {
            //        _Stream.Seek(0, SeekOrigin.Begin);
            //        _Excel = new ExcelPackage(_Stream);
            //    }
            //    var _Sheet = _Excel.Workbook.Worksheets[1];
            //    var RowIndex = 2;



            //    for (int i = 0; i < grid1.RowCount; i++)
            //    {
            //        string sAcceptDate = "";
            //        var _AcceptDate = grid1[ColumnAcceptTime.Index, i].FormattedValue;
            //        if (!String.IsNullOrEmpty(_AcceptDate.ToString()))
            //        {
            //            sAcceptDate = Convert.ToDateTime(_AcceptDate).ToString("yyyyMMdd");

            //        }

            //        _Sheet.Cells[RowIndex, 1].Value = i + 1;
            //        _Sheet.Cells[RowIndex, 2].Value = grid1[ColumnNumber.Index, i].FormattedValue;
            //        _Sheet.Cells[RowIndex, 3].Value = grid1[ColumnSubClientId.Index, i].FormattedValue;
            //        _Sheet.Cells[RowIndex, 4].Value = grid1[ColumnStart.Index, i].FormattedValue;
            //        _Sheet.Cells[RowIndex, 5].Value = grid1[ColumnStop.Index, i].FormattedValue;
            //        _Sheet.Cells[RowIndex, 6].Value = grid1[Column_DriverCarNo.Index, i].FormattedValue;
            //        _Sheet.Cells[RowIndex, 7].Value = sAcceptDate;
            //        _Sheet.Cells[RowIndex, 8].Value = grid1[ColumnSalesPrice.Index, i].FormattedValue?.ToString().Replace(",", "");

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
            //else
            //{


            //}




        }

        public void Exceldefault()
        {

            //ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;


            var fileString = "배차정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.Order_List))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;



            for (int i = 0; i < grid1.RowCount; i++)
            {
                string sAcceptDate = "";
                //var _AcceptDate = grid1[ColumnAcceptTime.Index, i].FormattedValue;
                //if (!String.IsNullOrEmpty(_AcceptDate.ToString()))
                //{
                //    sAcceptDate = Convert.ToDateTime(_AcceptDate).ToString("yyyyMMdd");

                //}

                _Sheet.Cells[RowIndex, 1].Value = i + 1;
                _Sheet.Cells[RowIndex, 2].Value = grid1[ColumnNumber.Index, i].FormattedValue;
                //_Sheet.Cells[RowIndex, 3].Value = grid1[ColumnClientId.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 4].Value = grid1[CustomerTeamColumn.Index, i].FormattedValue;


                _Sheet.Cells[RowIndex, 5].Value = grid1[ColumnStart.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 6].Value = grid1[ColumnStop.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 7].Value = grid1[Column_DriverCarNo.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 8].Value = grid1[Column_DriverName.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 9].Value = sAcceptDate;
                _Sheet.Cells[RowIndex, 10].Value = grid1[ColumnSalesPrice.Index, i].FormattedValue?.ToString().Replace(",", "");

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

        string SmsResult;
        private void SMS_Click(object sender, EventArgs e)
        {

            string SVC_RT;
            string SVC_MSG;
            List<string> _SVC_RT = new List<string>();
            if (LocalUser.Instance.LogInInformation.IsAdmin)
                return;

            if (!LocalUser.Instance.LogInInformation.Client.HideAddTrade && !LocalUser.Instance.LogInInformation.Client.SmsYn)
            {
                MessageBox.Show("본 서비스의 “CRM 연동”과 “문자 전송”은 \r\nLG U+ 인터넷전화기를 통해서만 가능 합니다.\r\n귀 사는\r\nLG U+ 미 사용자이므로 사용하실 수 없습니다.\r\n\r\nLG U+인터넷전화 신청\r\n문의 : ☎ 1833-2363  엑티브아이티㈜", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            //bool AllowSMS = false;
            //using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //{
            //    _Connection.Open();
            //    using (SqlCommand _Command = _Connection.CreateCommand())
            //    {
            //        _Command.CommandText = "SELECT AllowSMS FROM Clients WHERE ClientId = @ClientId";
            //        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
            //        var o = _Command.ExecuteScalar();
            //        if (o != null && (bool)o == true)
            //            AllowSMS = true;
            //    }
            //    _Connection.Close();
            //}
            //if (!AllowSMS)
            //{
            //    MessageBox.Show("SMS 전송 서비스를 신청하지 않았습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            string SMSText;
            if (DataListSource.Current != null && DataListSource.Current is Order Current)
            {
                if (!String.IsNullOrEmpty(Current.DriverPhoneNo))
                {
                    FrmSMS _FrmSMS = new FrmSMS(1);
                    _FrmSMS.Owner = this;
                    _FrmSMS.StartPosition = FormStartPosition.CenterParent;
                    if (_FrmSMS.ShowDialog() == DialogResult.OK)
                    {
                        if (_FrmSMS.txt_SMS.Text == "") return;

                        _FrmSMS.Close();
                        SMSText = _FrmSMS.txt_SMS.Text;
                        //string Parameter = "";
                        //WebClient mWebClient = new WebClient();
                        //Parameter = String.Format(@"?SessionId={0}&PhoneNo={1}&Message={2}", LocalUser.Instance.LogInInformation.SessionId, Current.DriverPhoneNo, SMSText);
                        //try
                        //{
                        //    var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/SMS/Send" + Parameter));
                        //}
                        //catch (Exception)
                        //{
                        //}

                        if (Current.DriverPhoneNo.Replace("-", "").Length >= 10)
                        {

                            string sha512password = EncryptSHA512(CallHelper.Instance.Password);




                            string Parameter = "";
                            Parameter = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", CallHelper.Instance.Id, sha512password, Current.DriverPhoneNo.Replace("-", ""), SMSText);




                            JObject response = null;

                            var uriBuilder = new UriBuilder("https://centrex.uplus.co.kr/RestApi/smssend");






                            uriBuilder.Query = Parameter;
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
                                    _SVC_RT.Add(SVC_RT);
                                }

                            }


                            Data.Connection((_Connection) =>
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText =
                                    "INSERT INTO CallSms (CTime, OriginalPhoneNo, SmsResult, ResultMessage, ClientId,LoginId,CustomerId,DriverId,MSG) VALUES (@CTime, @OriginalPhoneNo, @SmsResult, @ResultMessage, @ClientId,@LoginId,@CustomerId,@DriverId,@Msg)";

                                    _Command.Parameters.AddWithValue("@CTime", DateTime.Now);
                                    _Command.Parameters.AddWithValue("@OriginalPhoneNo", Current.DriverPhoneNo.Replace("-", ""));
                                    _Command.Parameters.AddWithValue("@SmsResult", SmsResult);
                                    _Command.Parameters.AddWithValue("@ResultMessage", SVC_MSG);
                                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                    _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                                    _Command.Parameters.AddWithValue("@CustomerId", 0);
                                    _Command.Parameters.AddWithValue("@DriverId", Current.DriverId);
                                    _Command.Parameters.AddWithValue("@Msg", SMSText);
                                    _Command.ExecuteNonQuery();
                                    //CallId = Convert.ToInt32(_Command.ExecuteScalar());
                                }
                            });

                        }

                        if (_SVC_RT.Contains("1004"))
                        {
                            MessageBox.Show("전화번호(LG U+)로 로그인에 실패 했습니다.\r\n\r\n“통화내역” 메뉴에서 “전화번호 설정”을 클릭,\r\n“비밀번호 초기화”를 한 후에\r\n다시 사용하시면 됩니다.", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        MessageBox.Show("SMS 전송 '" + 1 + "' 건  " + SmsResult + " 하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    MessageBox.Show("배차 완료된 건을 선택해주세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void 수정내역조회ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current != null && DataListSource.Current is Order Current)
            {
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    if (ShareOrderDataSet.OrderUpdateLogs.Any(c => c.OrderId == Current.OrderId))
                    {
                        Dialog_OrderUpdatLog d = new Dialog_OrderUpdatLog(ShareOrderDataSet.OrderUpdateLogs.Where(c => c.OrderId == Current.OrderId).OrderByDescending(c => c.OrderUpdateLogId).ToArray());
                        d.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("수정 내역이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

      

     

        

       

      
        private void NewMobileNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;
        }

     
        private void StartPhoneNo_Enter(object sender, EventArgs e)
        {
            //var _Txt = ((System.Windows.Forms.TextBox)sender);
            //_Txt.Text = _Txt.Text.Replace("-", "");
            //_Txt.SelectionStart = _Txt.Text.Length;
        }

        private void StartPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void StartPhoneNo_Leave(object sender, EventArgs e)
        {
            //if (!String.IsNullOrWhiteSpace(StartPhoneNo.Text))
            //{
            //    var _S = StartPhoneNo.Text.Replace("-", "").Replace(" ", "");
            //    if (_S.Length > 3)
            //    {
            //        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
            //    }
            //    if (_S.Length > 7)
            //    {
            //        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
            //    }
            //    if (_S.Length > 12)
            //    {
            //        _S = _S.Replace("-", "");
            //        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
            //    }
            //    StartPhoneNo.Text = _S;
            //}

            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
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
                        if (_S.Length == 9)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                        else
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                _Txt.Text = _S;
            }
        }

        private void StopPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void StopPhoneNo_Leave(object sender, EventArgs e)
        {
            //if (!String.IsNullOrWhiteSpace(StopPhoneNo.Text))
            //{
            //    var _S = StopPhoneNo.Text.Replace("-", "").Replace(" ", "");
            //    if (_S.Length > 3)
            //    {
            //        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
            //    }
            //    if (_S.Length > 7)
            //    {
            //        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
            //    }
            //    if (_S.Length > 12)
            //    {
            //        _S = _S.Replace("-", "");
            //        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
            //    }
            //    StopPhoneNo.Text = _S;
            //}

            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
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

                        if (_S.Length == 9)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                        else
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                    }

                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                _Txt.Text = _S;
            }
        }


        private void NewCPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void NewCPhoneNo_Leave(object sender, EventArgs e)
        {
            //if (!String.IsNullOrWhiteSpace(NewCPhoneNo.Text))
            //{
            //    var _S = NewCPhoneNo.Text.Replace("-", "").Replace(" ", "");
            //    if (_S.Length > 3)
            //    {
            //        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
            //    }
            //    if (_S.Length > 7)
            //    {
            //        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
            //    }
            //    if (_S.Length > 12)
            //    {
            //        _S = _S.Replace("-", "");
            //        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
            //    }
            //    NewCPhoneNo.Text = _S;
            //}
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
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
                        if (_S.Length == 9)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                        else
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                _Txt.Text = _S;
            }
        }
        private void NewCMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        

        private void Number_MouseClick(object sender, MouseEventArgs e)
        {
            Number_Enter(sender, EventArgs.Empty);
        }

        private void Price6_TextChanged(object sender, EventArgs e)
        {
            //if (int.TryParse(Price6.Text, out int t) && t > 0)
            //    DriverPoint.Checked = true;
            //else
            //    DriverPoint.Checked = false;
        }

        private void DataList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Current = DataListSource[e.RowIndex] as Order;


        }

       

     

        private void btnAccountMemo_Click(object sender, EventArgs e)
        {
            

            if (Customer.Tag == null || String.IsNullOrEmpty(Customer.Text))
            {
                MessageBox.Show("화주를 선택하십시오.");

            }
            else
            {
                Dialog_OrderMemo dordermemo = new Dialog_OrderMemo((int)Customer.Tag);

                dordermemo.ShowDialog();

            }

        }
        bool MethodProcess = false;
      

        private void OrderPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void OrderPhoneNo_Leave(object sender, EventArgs e)
        {

            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
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
                        if (_S.Length == 9)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                        else
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                _Txt.Text = _S;
            }
        }





      

        private void btnAfterList_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
           
            SearchText.Clear();
            _Sort = false;
            _Search(4);
        }

        private void OrderAcceptSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
          
            SearchText.Clear();
            _Sort = false;
            _Search(5);
        }

        private void ShareSearch_Click(object sender, EventArgs e)
        {
            //SearchTextFilter.SelectedIndex = 0;
            //SearchDriverTrade.SelectedIndex = 0;
            //SearchCustomerTax.SelectedIndex = 0;
            //SearchText.Clear();
            //_Sort = false;
            //_Search(7);
        }
        private void OnFocus(object sender, EventArgs e)
        {
            ((TextBox)sender).ImeMode = ImeMode.Hangul;

        }

        private void MouseDropDown(object sender, EventArgs e)
        {



            ((ComboBox)sender).DroppedDown = true;
        }


        private void OnDropDown(object sender, EventArgs e)
        {

            ((ComboBox)sender).DroppedDown = true;
        }


        bool _Sort = false;
        string _SortGubun = "Time";
        private void DataList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _SortGubun = "Time";
            //var Current = DataListSource[e.RowIndex] as Order;
            if (e.ColumnIndex == ColumnStart.Index)
            {
                _SortGubun = "Start";
                _Search();
            }
            else if (e.ColumnIndex == ColumnStop.Index)
            {
                _SortGubun = "Stop";
                _Search();
            }
        }

        private void NewCarYear_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            //{
            //    NewCarYear.Clear();
            //    NewMobileNo.Clear();
            //    NewCarNo.Clear();
            //    if (Clipboard.GetData(DataFormats.Text) == null)
            //    {
            //        return;
            //    }
            //    var clipboard = Clipboard.GetData(DataFormats.Text).ToString();

            //    string[] splited2 = clipboard.Split(' ');
            //    if (splited2.Length == 3)
            //    {
            //        string _CarYear = splited2[0];
            //        //  MessageBox.Show(_CarYear);

            //        NewCarNo.Text = splited2[1];
            //        NewMobileNo.Text = splited2[2];
            //        NewCarYear.Clear();
            //        NewCarYear.Text = "";

            //        NewDriverAndAccept.Focus();
            //    }
            //}
        }

        private void NewCarYear_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

     
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (Customer.Tag != null)
            {


                FrmMN0301PrevPopup f = new FrmMN0301PrevPopup("W", Customer.Text);
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    NewOrder_Click(null, null);

                    ////DataList.CurrentCell = null;
                    ////IsCurrentNull = true;
                    //_NewOrder();
                    //Driver.Enabled = true;


                    int fOrderId = 0;
                    fOrderId = f._OrderId;

                    PrevViewOrder(fOrderId);

                    //DataList.CurrentCell = null;
                    //IsCurrentNull = true;

                }
            }
        }


        private void PrevViewOrder(int POrderId)
        {
            List<Order> DataSource = new List<Order>();

            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderId == POrderId).ToList();

                if (DataSource.Any())
                {
                    StartState.Text = DataSource.First().StartState;
                    StartState.Text = DataSource.First().StartState;
                    StartCity.Text = DataSource.First().StartCity;
                    StartStreet.Text = DataSource.First().StartStreet;
                    Start.Text = DataSource.First().StartDetail;

                    StopState.Text = DataSource.First().StopState;
                    StopCity.Text = DataSource.First().StopCity;
                    StopStreet.Text = DataSource.First().StopStreet;
                    Stop.Text = DataSource.First().StopDetail;

                    CarSize.SelectedValue = DataSource.First().CarSize;
                    CarType.SelectedValue = DataSource.First().CarType;
                    if (DataSource.First().StopDateHelper == null)
                        StopDateHelper.SelectedValue = 0;
                    else
                        StopDateHelper.SelectedValue = DataSource.First().StopDateHelper;
                    CarCount.Text = DataSource.First().CarCount.ToString();
                    PayLocation.SelectedValue = DataSource.First().PayLocation;
                    //Price1.Text = (DataSource.First().TradePrice ?? 0).ToString("N0");
                    Price2.Text = (DataSource.First().SalesPrice ?? 0).ToString("N0");
                    Price3.Text = (DataSource.First().AlterPrice ?? 0).ToString("N0");
                   
                    lblPrice4.Text = (DataSource.First().StopPrice ?? 0 + DataSource.First().StartPrice ?? 0).ToString("N0");
                    TotalPrice.Text = (DataSource.First().SalesPrice ?? 0 + DataSource.First().AlterPrice ?? 0 + DataSource.First().StopPrice ?? 0 + DataSource.First().StartPrice ?? 0).ToString("N0");




                    Item.Text = DataSource.First().Item;
                    Remark.Text = DataSource.First().Remark;


                    if (DataSource.First().IsShared)
                    {
                        Shared.Checked = true;
                        SharedItemLength.SelectedValue = DataSource.First().SharedItemLength ?? 0;
                        SharedItemSize.SelectedValue = DataSource.First().SharedItemSize ?? 0;
                    }
                    else
                    {
                        NotShared.Checked = true;
                        SharedItemLength.SelectedIndex = 0;
                        SharedItemSize.SelectedIndex = 0;
                    }


                    Emergency.Checked = DataSource.First().Emergency == true;
                    Round.Checked = DataSource.First().Round == true;
                    Reservation.Checked = DataSource.First().Reservation == true;

                    ClearStarTime_Click(null, null);



                    if (DataSource.First().StartInfo == "지게차")
                    {
                        StartInfo1.Checked = true;
                    }
                    else if (DataSource.First().StartInfo == "수작업")
                    {
                        StartInfo2.Checked = true;
                    }
                    else if (DataSource.First().StartInfo == "호이스트")
                    {
                        StartInfo3.Checked = true;
                    }
                    else if (DataSource.First().StartInfo == "크레인")
                    {
                        StartInfo4.Checked = true;
                    }
                    else if (DataSource.First().StartInfo == "컨베이어")
                    {
                        StartInfo5.Checked = true;
                    }


                    if (DataSource.First().StartTimeType == 1)
                    {
                        StartTimeType1.Checked = true;
                    }
                    else if (DataSource.First().StartTimeType == 2)
                    {
                        StartTimeType2.Checked = true;
                        if (DataSource.First().StartTimeHour > 0)
                        {
                            if (DataSource.First().StartTimeHour > 12)
                            {
                                StartTimeHour1.SelectedIndex = 2;
                                StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value - 12;
                            }
                            else
                            {
                                StartTimeHour1.SelectedIndex = 1;
                                StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value;
                            }
                            StartTimeHalf.Checked = DataSource.First().StartTimeHalf == true;
                            StartTimeETC.Checked = DataSource.First().StartTimeETC == true;
                        }
                    }
                    else if (DataSource.First().StartTimeType == 3)
                    {
                        StartTimeType3.Checked = true;
                        if (DataSource.First().StartTimeHour > 0)
                        {
                            if (DataSource.First().StartTimeHour > 12)
                            {
                                StartTimeHour1.SelectedIndex = 2;
                                StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value - 12;
                            }
                            else
                            {
                                StartTimeHour1.SelectedIndex = 1;
                                StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value;
                            }
                            StartTimeHalf.Checked = DataSource.First().StartTimeHalf == true;
                            StartTimeETC.Checked = DataSource.First().StartTimeETC == true;
                        }
                    }
                    else if (DataSource.First().StartTimeType > 1000)
                    {
                        StartTimeType4.Text = (DataSource.First().StartTimeType - 1000).ToString();
                        if (DataSource.First().StartTimeHour > 0)
                        {
                            if (DataSource.First().StartTimeHour > 12)
                            {
                                StartTimeHour1.SelectedIndex = 2;
                                StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value - 12;
                            }
                            else
                            {
                                StartTimeHour1.SelectedIndex = 1;
                                StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value;
                            }
                            StartTimeHalf.Checked = DataSource.First().StartTimeHalf == true;
                            StartTimeETC.Checked = DataSource.First().StartTimeETC == true;
                        }
                    }

                    ClearStopTime_Click(null, null);
                    if (DataSource.First().StopInfo == "지게차")
                    {
                        StopInfo1.Checked = true;
                    }
                    else if (DataSource.First().StopInfo == "수작업")
                    {
                        StopInfo2.Checked = true;
                    }
                    else if (DataSource.First().StopInfo == "호이스트")
                    {
                        StopInfo3.Checked = true;
                    }
                    else if (DataSource.First().StopInfo == "크레인")
                    {
                        StopInfo4.Checked = true;
                    }
                    else if (DataSource.First().StopInfo == "컨베이어")
                    {
                        StopInfo5.Checked = true;
                    }
                    if (DataSource.First().StopTimeType == 1)
                    {
                        StopTimeType1.Checked = true;
                        if (DataSource.First().StopTimeHour > 0)
                        {
                            if (DataSource.First().StopTimeHour > 12)
                            {
                                StopTimeHour1.SelectedIndex = 2;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                            }
                            else
                            {
                                StopTimeHour1.SelectedIndex = 1;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                            }
                            StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                        }
                    }
                    else if (DataSource.First().StopTimeType == 2)
                    {
                        StopTimeType2.Checked = true;
                        if (DataSource.First().StopTimeHour > 0)
                        {
                            if (DataSource.First().StopTimeHour > 12)
                            {
                                StopTimeHour1.SelectedIndex = 2;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                            }
                            else
                            {
                                StopTimeHour1.SelectedIndex = 1;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                            }
                            StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                        }
                    }
                    else if (DataSource.First().StopTimeType == 3)
                    {
                        StopTimeType3.Checked = true;
                        if (DataSource.First().StopTimeHour > 0)
                        {
                            if (DataSource.First().StopTimeHour > 12)
                            {
                                StopTimeHour1.SelectedIndex = 2;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                            }
                            else
                            {
                                StopTimeHour1.SelectedIndex = 1;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                            }
                            StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                        }
                    }
                    else if (DataSource.First().StopTimeType == 4)
                    {
                        StopTimeType4.Checked = true;
                    }
                    else if (DataSource.First().StopTimeType > 1000)
                    {
                        StopTimeType4.Text = (DataSource.First().StopTimeType - 1000).ToString();
                        if (DataSource.First().StopTimeHour > 0)
                        {
                            if (DataSource.First().StopTimeHour > 12)
                            {
                                StopTimeHour1.SelectedIndex = 2;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                            }
                            else
                            {
                                StopTimeHour1.SelectedIndex = 1;
                                StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                            }
                            StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                        }
                    }



                    OrderPhoneNo.Text = DataSource.First().OrderPhoneNo;
                    StopPhoneNo.Text = DataSource.First().StopPhoneNo;
                    StartPhoneNo.Text = DataSource.First().StartPhoneNo;
                    ItemSizeInclude.Checked = DataSource.First().ItemSizeInclude == true;
                    ItemSize.Text = DataSource.First().ItemSize;

                  

                  
                    HasTrade.Checked = DataSource.First().TradeId != null;
                    HasSales.Checked = DataSource.First().SalesManageId != null;
                    DriverPoint.Checked = DataSource.First().DriverPoint != null;

                    StartName.Text = DataSource.First().StartName;
                    StopName.Text = DataSource.First().StopName;
                    StartMemo.Text = DataSource.First().StartMemo;
                    StopMemo.Text = DataSource.First().StopMemo;

                    RequestMemo.Text = DataSource.First().RequestMemo;
                    //if (Current.ReferralId)
                    if (DataSource.First().DriverGrade == null)
                    {
                        DriverGrade.SelectedValue = 0;
                    }
                    else
                    {
                        DriverGrade.SelectedValue = DataSource.First().DriverGrade;
                    }

                    SetCustomerList();
                   
                    if (DataSource.First().MyCarOrder == true)
                    {
                        chkMyCarOrder.Checked = true;
                    }
                    else
                    {
                        chkMyCarOrder.Checked = false;
                    }

                    #region 공유
                    //접수운송사,배차운송사 같으면 True 틀리면 False
                    bool _MyOrder = false;
                    //접수운송사ID
                    int _MyCarClientId = 0;

                    //타사에서 배차했을때
                    if (DataSource.First().FOrderId != 0)
                    {
                        //타사공유배차정보
                        var Query2 = ShareOrderDataSet.Orders.Where(c => c.OrderId == DataSource.First().FOrderId).ToArray();


                        if (Query2.Any())
                        {
                            //배차운송사ID와 로그인한운송사 아이디가 같으면
                            if (Query2.First().ClientId == LocalUser.Instance.LogInInformation.ClientId)
                            {
                                //접수운송사와 배차운송사가 같음
                                _MyOrder = true;

                            }
                            else
                            {   //접수운송사와 배차운송사가 틀림
                                _MyOrder = false;
                            }

                            //배차한 운송사ID
                            _MyCarClientId = (int)Query2.First().ClientId;
                        }
                    }
                    //배차전 또는 내가 배차했을때
                    else
                    {
                        //접수운송사가 타사가 아니면 
                        if (DataSource.First().ClientId == LocalUser.Instance.LogInInformation.ClientId)
                        {
                            _MyOrder = true;
                        }
                        else
                        {
                            _MyOrder = false;

                        }

                    }



                    ////공유 /타사접수일때
                    //if (Current.MyCarOrder == true && !_MyOrder)
                    //{

                    //    Price2.Text = (Current.TradePrice ?? 0).ToString("N0");
                    //    Price1.Text = "0";


                    //    //배차 완료
                    //    if (Current.OrderStatus == 3)
                    //    {
                    //        //접수운송사ID //타사에서 배차
                    //        if (_MyCarClientId != 0)
                    //        {
                    //            var _ClientsList = _ClientTable.Where(c => c.ClientId == _MyCarClientId).First();
                    //            Customer.Text = _ClientsList.Name;
                    //            OrderPhoneNo.Text = _ClientsList.PhoneNo;

                    //        }
                    //        //자차배차
                    //        else
                    //        {
                    //            if (Current.CustomerId != null && !string.IsNullOrEmpty(Current.Customer))
                    //            {
                    //                Customer.Tag = Current.CustomerId;
                    //                Customer.Text = Current.Customer;
                    //            }
                    //            else
                    //            {
                    //                Customer.Tag = null;
                    //                Customer.Text = Current.Customer;


                    //            }

                    //        }
                    //    }
                    //    //배차전
                    //    else
                    //    {
                    //        var _ClientsList = _ClientTable.Where(c => c.ClientId == Current.ClientId).First();
                    //        Customer.Text = _ClientsList.Name;
                    //        OrderPhoneNo.Text = _ClientsList.PhoneNo;


                    //        //if (Current.CustomerId != null && !string.IsNullOrEmpty(Current.Customer))
                    //        //{
                    //        //    Customer.Tag = Current.CustomerId;
                    //        //    Customer.Text = Current.Customer;
                    //        //}
                    //        //else
                    //        //{
                    //        //    Customer.Tag = null;
                    //        //    Customer.Text = Current.Customer;


                    //        //}

                    //    }
                    //    Customer.Enabled = false;
                    //    OrderPhoneNo.Enabled = false;
                    //    btnPrev.Enabled = false;
                    //    CustomerManager.Enabled = false;


                    //}
                    //로그인운송사접수
                    if (DataSource.First().ClientId == LocalUser.Instance.LogInInformation.ClientId && _MyOrder)
                    {
                        if (DataSource.First().CustomerId != null && !string.IsNullOrEmpty(DataSource.First().Customer))
                        {
                            Customer.Tag = DataSource.First().CustomerId;
                            Customer.Text = DataSource.First().Customer;
                        }
                        else
                        {
                            Customer.Tag = null;
                            Customer.Text = DataSource.First().Customer;


                        }

                        Customer.Enabled = true;
                        OrderPhoneNo.Enabled = true;
                        btnPrev.Enabled = true;
                        CustomerManager.Enabled = true;
                    }

                    #endregion


                    if (DataSource.First().StartMulti == true)
                    {
                        StartMulti.Checked = true;
                    }
                    else
                    {
                        StartMulti.Checked = false;
                    }

                    if (DataSource.First().StopMulti == true)
                    {
                        StopMulti.Checked = true;
                    }
                    else
                    {
                        StopMulti.Checked = false;
                    }

                    CustomerManager.Text = DataSource.First().CustomerManager;



                    RequestMemo.Clear();
                    StartTimeType1.Checked = true;
                    StartTimeType1.Checked = false;

                    StartTimeType4.Clear();

                    StartTimeHour1.Enabled = false;
                    StartTimeHour2.Enabled = false;
                    StartTimeHalf.Enabled = false;
                    StartTimeETC.Enabled = false;
                    SetRemark();


                    StopTimeType1.Checked = true;
                    StopTimeType1.Checked = false;
                    //StopTimeType2.Checked = false;
                    //StopTimeType3.Checked = false;
                    //StopTimeType4.Checked = false;
                    StopTimeType5.Clear();
                    StopTimeHour1.Enabled = false;
                    StopTimeHour2.Enabled = false;
                    StopTimeHalf.Enabled = false;

                    SetRemark();


                }

            }

        }

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            FindZipNew f = new Admin.FindZipNew(Start.Text, "Order");
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                if (f.rdoRoad.Checked)
                {
                    // txt_Zip.Text = f.Zip;
                    var ss = f.Address.Split(' ');
                    StartState.Text = ss[0];
                    StartCity.Text = ss[1];
                    StartStreet.Text = ss[2];
                    Start.Text = String.Join(" ", ss.Skip(3));
                    if (Start.Text.Length > 30)
                    {
                        Start.Text = Start.Text.Substring(0, 29);
                    }


                    StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                    _AddressGubun = "R";
                    Start.Focus();
                }
                else if (f.rdoJibun.Checked)
                {
                    var ss = f.Jibun.Split(' ');
                    StartState.Text = ss[0];
                    StartCity.Text = ss[1];
                    StartStreet.Text = ss[2];
                    Start.Text = String.Join(" ", ss.Skip(3));

                    if (Start.Text.Length > 30)
                    {
                        Start.Text = Start.Text.Substring(0, 29);
                    }
                    StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                    _AddressGubun = "J";
                    Start.Focus();
                }
                //else if (f.rdoSimple.Checked)
                //{
                //    var ss = f.Jibun.Split(' ');
                //    StartState.Text = ss[0];
                //    StartCity.Text = ss[1];
                //    if (ss.Length > 2)
                //    {
                //        StartStreet.Text = ss[2];
                //        Start.Text = String.Join(" ", ss.Skip(3));
                //    }
                //    else
                //    {
                //        StartStreet.Text = "";
                //        Start.Text = "";
                //    }
                //    if (Start.Text.Length > 30)
                //    {
                //        Start.Text = Start.Text.Substring(0, 29);
                //    }
                //    StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                //    _AddressGubun = "S";
                //    Start.Focus();
                //}
            }
        }

        private void btnFindZip2_Click(object sender, EventArgs e)
        {
            FindZipNew f = new Admin.FindZipNew(Stop.Text, "Order");
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                if (f.rdoRoad.Checked)
                {
                    // txt_Zip.Text = f.Zip;
                    var ss = f.Address.Split(' ');
                    StopState.Text = ss[0];
                    StopCity.Text = ss[1];
                    StopStreet.Text = ss[2];
                    Stop.Text = String.Join(" ", ss.Skip(3));
                    if (Stop.Text.Length > 30)
                    {
                        Stop.Text = Stop.Text.Substring(0, 25);
                    }
                    StopName.Text = _AddressStateParse(StopState.Text) + " " + StopCity.Text;
                    _AddressGubun = "R";
                    Stop.Focus();
                }
                else if (f.rdoJibun.Checked)
                {
                    var ss = f.Jibun.Split(' ');
                    StopState.Text = ss[0];
                    StopCity.Text = ss[1];
                    StopStreet.Text = ss[2];
                    Stop.Text = String.Join(" ", ss.Skip(3));
                    if (Stop.Text.Length > 30)
                    {
                        Stop.Text = Stop.Text.Substring(0, 29);
                    }
                    StopName.Text = _AddressStateParse(StopState.Text) + " " + StopCity.Text;
                    _AddressGubun = "J";
                    Stop.Focus();
                }
                //else if (f.rdoSimple.Checked)
                //{
                //    var ss = f.Jibun.Split(' ');
                //    StopState.Text = ss[0];
                //    StopCity.Text = ss[1];
                //    if (ss.Length > 2)
                //    {
                //        StopStreet.Text = ss[2];
                //        Stop.Text = String.Join(" ", ss.Skip(3));
                //    }
                //    else
                //    {
                //        StartStreet.Text = "";
                //        Start.Text = "";
                //    }
                //    if (Stop.Text.Length > 30)
                //    {
                //        Stop.Text = Stop.Text.Substring(0, 29);
                //    }
                //    StopName.Text = _AddressStateParse(StopState.Text) + " " + StopCity.Text;
                //    _AddressGubun = "S";
                //    Stop.Focus();
                //}
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372110420");
        }

        private void CarSize_MouseClick(object sender, MouseEventArgs e)
        {


        }
        bool _MycarOrderSave = false;
        private void chkMyCarOrder_CheckedChanged(object sender, EventArgs e)
        {


          //  PayLocation_SelectedIndexChanged(null, null);

            var Current = DataListSource.Current as Order;


            if (grid1.CurrentCell != null)
            {
                if (!MethodProcess)
                {
                    if (chkMyCarOrder.Checked == true)
                    {
                        Current.MyCarOrder = true;
                    }
                    else
                    {
                        Current.MyCarOrder = false;
                    }

                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
                        ShareOrderDataSet.SaveChanges();
                    }
                    DataListSource.ResetBindings(false);

                    _Sort = false;
                    _Search();
                }

            }




        }

       

     

        private void addressRotate_Click(object sender, EventArgs e)
        {
            var _StartState = StartState.Text;
            var _StartCity = StartCity.Text;
            var _StartStreet = StartStreet.Text;
            var _Start = Start.Text;
            var _StartName = StartName.Text;


            var _StopState = StopState.Text;
            var _StopCity = StopCity.Text;
            var _StopStreet = StopStreet.Text;
            var _Stop = Stop.Text;
            var _StopName = StopName.Text;

            bool _StartInfo1 = false;
            bool _StartInfo2 = false;
            bool _StartInfo3 = false;
            bool _StartInfo4 = false;
            bool _StartInfo5 = false;

            if (StartInfo1.Checked)
            {
                _StartInfo1 = true;
            }
            if (StartInfo2.Checked)
            {
                _StartInfo2 = true;
            }
            if (StartInfo3.Checked)
            {
                _StartInfo3 = true;
            }
            if (StartInfo4.Checked)
            {
                _StartInfo4 = true;
            }
            if (StartInfo5.Checked)
            {
                _StartInfo5 = true;
            }

            bool _StopInfo1 = false;
            bool _StopInfo2 = false;
            bool _StopInfo3 = false;
            bool _StopInfo4 = false;
            bool _StopInfo5 = false;


            if (StopInfo1.Checked)
            {
                _StopInfo1 = true;
            }
            if (StopInfo2.Checked)
            {
                _StopInfo2 = true;
            }
            if (StopInfo3.Checked)
            {
                _StopInfo3 = true;
            }
            if (StopInfo4.Checked)
            {
                _StopInfo4 = true;
            }
            if (StopInfo5.Checked)
            {
                _StopInfo5 = true;
            }
            bool _StartTimeType2 = false;
            bool _StartTimeType3 = false;
            var _StartTimeType4 = StartTimeType4.Text;
            if (StartTimeType2.Checked)
            {
                _StartTimeType2 = true;
            }
            if (StartTimeType3.Checked)
            {
                _StartTimeType3 = true;
            }
            var _StartTimeHour1 = StartTimeHour1.Text;
            var _StartTimeHour2 = StartTimeHour2.Text;
            bool _StartTimeHalf = false;

            if (StartTimeHalf.Checked)
            {
                _StartTimeHalf = true;
            }


            bool _StopTimeType1 = false;
            bool _StopTimeType2 = false;
            var _StopTimeType5 = StopTimeType5.Text;
            if (StopTimeType1.Checked)
            {
                _StopTimeType1 = true;
            }
            if (StopTimeType2.Checked)
            {
                _StopTimeType2 = true;
            }
            var _StopTimeHour1 = StopTimeHour1.Text;
            var _StopTimeHour2 = StopTimeHour2.Text;
            bool _StopTimeHalf = false;

            if (StopTimeHalf.Checked)
            {
                _StopTimeHalf = true;
            }

            var _StartPhoneNo = StartPhoneNo.Text;
            var _StopPhoneNo = StopPhoneNo.Text;

            StartState.Text = _StopState;
            StartCity.Text = _StopCity;
            StartStreet.Text = _StopStreet;
            Start.Text = _Stop;
            StartName.Text = _StopName;

            StopState.Text = _StartState;
            StopCity.Text = _StartCity;
            StopStreet.Text = _StartStreet;
            Stop.Text = _Start;
            StopName.Text = _StartName;


            StartInfo1.Checked = _StopInfo1;
            StartInfo2.Checked = _StopInfo2;
            StartInfo3.Checked = _StopInfo3;
            StartInfo4.Checked = _StopInfo4;
            StartInfo5.Checked = _StopInfo5;

            StopInfo1.Checked = _StartInfo1;
            StopInfo2.Checked = _StartInfo2;
            StopInfo3.Checked = _StartInfo3;
            StopInfo4.Checked = _StartInfo4;
            StopInfo5.Checked = _StartInfo5;

            StartTimeType2.Checked = _StopTimeType1;
            StartTimeType3.Checked = _StopTimeType2;
            StartTimeType4.Text = _StopTimeType5;
            StartTimeHour1.Text = _StopTimeHour1;
            StartTimeHour2.Text = _StopTimeHour2;
            StartTimeHalf.Checked = _StopTimeHalf;
            StartPhoneNo.Text = _StopPhoneNo;

            StopTimeType1.Checked = _StartTimeType2;
            StopTimeType2.Checked = _StartTimeType3;
            StopTimeType5.Text = _StartTimeType4;
            StopTimeHour1.Text = _StartTimeHour1;
            StopTimeHour2.Text = _StartTimeHour2;
            StopTimeHalf.Checked = _StartTimeHalf;
            StopPhoneNo.Text = _StartPhoneNo;


        }

        private void chkMyOrder_CheckedChanged(object sender, EventArgs e)
        {


            _Sort = false;
            _Search();
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,
                ColumnNum,
                CustomerTeamColumn,
                ColumnOrderStatus,


                ColumnStart,
                ColumnStartTime,
                ColumnStop,
                ColumnStopTime,
                ColumnCarSize,
                ColumnCarType,

                ColumnSalesPrice,


                ColumnTotalPrice,


                 Column_DriverName,
                   Column_DriverCarNo,
                Column_DriverPhoneNo,
                ColumnDriverCarSize,
                ColumnDriverCarType




                );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);
        }

        private void StartTimeETC_CheckedChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

       
        private void chkHideMyCarOrder_CheckedChanged(object sender, EventArgs e)
        {

            //if (chkHideMyCarOrder.Checked == true)
            //{
            //    HideMyCarOrder = true;

            //    ShareSearch.Enabled = true;
            //}
            //else
            //{
            //    HideMyCarOrder = false;
            //    ShareSearch.Enabled = false;
            //}


            //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //{
            //    cn.Open();
            //    SqlCommand cmd = cn.CreateCommand();
            //    cmd.CommandText =
            //       "UPDATE Clients SET HideMyCarOrder = @HideMyCarOrder" +
            //       " WHERE Clientid = @ClientId";
            //    cmd.Parameters.AddWithValue("@HideMyCarOrder", HideMyCarOrder);
            //    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
            //    cmd.ExecuteNonQuery();
            //    cn.Close();
            //}
        }

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMonth.SelectedIndex)
            {
                //당일
                case 0:
                    DateFilterBegin.Value = DateTime.Now;
                    DateFilterEnd.Value = DateTime.Now;
                    break;
                //당일
                case 1:
                    DateFilterBegin.Value = DateTime.Now;
                    DateFilterEnd.Value = DateTime.Now;
                    break;
                //전일
                case 2:
                    DateFilterBegin.Value = DateTime.Now.AddDays(-1);
                    DateFilterEnd.Value = DateTime.Now.AddDays(-1);
                    break;
                //금주
                case 3:
                    DateFilterBegin.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    DateFilterEnd.Value = DateTime.Now;
                    break;
                //금월
                case 4:
                    DateFilterBegin.Text = DateTime.Now.ToString("yyyy/MM/01");
                    DateFilterEnd.Value = DateTime.Now;
                    break;
                //전월
                case 5:
                    DateFilterBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    DateFilterEnd.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                    //case 5:
                    //    dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                    //    dtp_Edate.Value = DateTime.Now;
                    //    break;
            }
        }
        private bool isFold = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (isFold)
            {
                button1.Text = "▲";
                panel1.Height = 330;
                isFold = false;

            }
            else
            {
                button1.Text = "▼";
                panel1.Height = 0;
                isFold = true;



            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (grid1.CurrentCell == null)
            {
                return;
            }
            var Current = DataListSource.Current as Order;


            if (Current != null)
            {
                if (!String.IsNullOrEmpty(Current.StartState) && !String.IsNullOrEmpty(Current.StopState))
                {
                    var sName = StartState.Text + " " + StartCity.Text + " " + StartStreet.Text;
                    var eName = StopState.Text + " " + StopCity.Text + " " + StopStreet.Text;

                    string url = $"http://map.daum.net/?sName={sName}&eName={eName}";
                    // System.Diagnostics.Process ie = System.Diagnostics.Process.Start("IExplore.exe", url);


                    System.Diagnostics.Process.Start(url);

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (grid1.CurrentCell == null)
            {
                return;
            }
            var Current = DataListSource.Current as Order;


            if (Current != null)
            {
                if (!String.IsNullOrEmpty(Current.StartState) && !String.IsNullOrEmpty(Current.StopState))
                {
                    var sName = StartState.Text + " " + StartCity.Text + " " + StartStreet.Text;
                    var eName = StopState.Text + " " + StopCity.Text + " " + StopStreet.Text;

                    string url = $"http://map.daum.net/?sName={sName}&eName={eName}";



                    System.Diagnostics.Process.Start(url);

                }
            }
            else
            {

                if (!String.IsNullOrEmpty(StartState.Text) && !String.IsNullOrEmpty(StopState.Text))
                {
                    var sName = StartState.Text + " " + StartCity.Text + " " + StartStreet.Text;
                    var eName = StopState.Text + " " + StopCity.Text + " " + StopStreet.Text;

                    string url = $"http://map.daum.net/?sName={sName}&eName={eName}";



                    System.Diagnostics.Process.Start(url);

                }
            }
        }

        private void DateFilterBegin_ValueChanged(object sender, EventArgs e)
        {

            //if (DateFilterEnd.Value.Date >= DateFilterBegin.Value.Date.AddMonths(3).Date)
            //{
            //    MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            //_Sort = false;
            //_Search();
        }

        private void DateFilterEnd_ValueChanged(object sender, EventArgs e)
        {
            //if (DateFilterEnd.Value.AddMonths(-3).Date >= DateFilterBegin.Value.Date)
            //{
            //    MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            //_Sort = false;
            //_Search();
        }
        string _CarNo = "";
        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (DriverSelect.SelectedItems.Count > 0)
            //{
            //    var _Driver = DriverModelList.Find(c => c.DriverId == (int)DriverSelect.SelectedItems[0].Tag);

            //    if (_Driver != null)
            //    {
            //        _Select(_Driver.CarNo);
            //    }
            //}
        }
      
        private void button3_Click(object sender, EventArgs e)
        {

            //  setCargoRegist();

            //cargoApiClass.get_address_search("광주");
        }
        /// <summary>
        /// 화물맨에 타업체 화물 등록합니다.
        /// 
        /// </summary>
        /// 
        string CargoStatus = "";

        public Color DisabledForeColor { get; private set; }

        private void setCargoRegist(string CargoGubun, int _Orderid = 0, int _CarCountValue = 0, int _i = 0)
        {
            string result;
            var _CargoAddrSearch = cargoDataSet.CargoApiAdress.ToArray();
            LocalUser.Instance.LogInInformation.LoadClient();
            //CargoId = LocalUser.Instance.LogInInformation.Client.CargoLoginId;
            CargoId = LocalUser.Instance.LogInInformation.CargoLoginId;
            if (CargoId == "")
            {
                MessageBox.Show("내정보에 화물맨 아이디를 등록하세요.");

                FrmMDI.LoadForm("FrmMN0204_CARGOOWNERMANAGE", "");
                return;
            }

            if (_Orderid != 0)
            {
                _ORDERCODE = _Orderid.ToString();
            }
            var _Carsize = CarSize.Text;
            string CarTypechange = "";
            string LOACITY = "";
            string LOACODE = "";
            string POIX = "";
            string POIY = "";

            string DOWCODE = "";
            string DOWCITY = "";
            string POIX_OUT = "";
            string POIY_OUT = "";
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                Int32 IOrderCode = Convert.ToInt32(_ORDERCODE);
                var CargoOrder = ShareOrderDataSet.Orders.Where(c => c.OrderId == IOrderCode).FirstOrDefault();

                #region 상차지
                string startaddr1 = cargoApiClass.Get_addr_search1(_AddressStateParse(CargoOrder.StartState), ApiUrl);
                //     string startaddr2 = "";
                string startaddr3 = "";
                string _StartStreet = "";
                string _StartDetail = "";
                string _startcity = "";


                if (_AddressGubun != "S")
                {
                    _StartStreet = cargoApiClass.roadaddr_trans(CargoOrder.StartState + " " + CargoOrder.StartCity + " " + CargoOrder.StartStreet + " " + CargoOrder.StartDetail);
                }
                if (string.IsNullOrEmpty(_StartStreet))
                {
                    var EditStartStreet = CargoOrder.StartStreet;
                    if (!string.IsNullOrEmpty(EditStartStreet))
                    {
                        if (EditStartStreet.Length > 2)
                        {
                            if (EditStartStreet == "동남구" && EditStartStreet == "서북구")
                            {

                            }
                            else
                            {
                                if (CargoOrder.StartStreet.Substring(CargoOrder.StartStreet.Length - 1, 1) == "시" || CargoOrder.StartStreet.Substring(CargoOrder.StartStreet.Length - 1, 1) == "구" || CargoOrder.StartStreet.Substring(CargoOrder.StartStreet.Length - 1, 1) == "읍")
                                {
                                    EditStartStreet = _AddressCityParse(EditStartStreet);
                                }
                            }
                        }
                    }

                    //화물맨주소검색
                    var _Q = _CargoAddrSearch.Where(c => c.SIDO + "" + c.GU + "" + c.GUN + "" + c.DONG == _AddressStateParse(CargoOrder.StartState) + "" + _AddressCityParse(CargoOrder.StartCity) + "" + EditStartStreet);

                    if (_Q.Any())
                    {
                        LOACODE = _Q.First().AREACODE;
                        POIX = _Q.First().POIX;
                        POIY = _Q.First().POIY;
                        LOACITY = _Q.First().SIDO + " " + _Q.First().GU + " " + _Q.First().GUN + " " + _Q.First().DONG;
                        LOACITY = LOACITY.Replace("  ", " ");
                    }
                    else
                    {

                        if (!String.IsNullOrEmpty(CargoOrder.StartState) && !String.IsNullOrEmpty(CargoOrder.StartCity))
                        {
                            CargoAddress cargoAddress = new CargoAddress(CargoOrder.StartState, CargoOrder.StartCity, CargoOrder.StartStreet);
                            cargoAddress.StartPosition = FormStartPosition.CenterParent;

                            if (cargoAddress.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                LOACODE = cargoAddress._AREACODE;
                                POIX = cargoAddress._POIX;
                                POIY = cargoAddress._POIY;


                                LOACITY = cargoAddress._SIDO + " " + cargoAddress._Gu_Gun + " " + cargoAddress._DONG;

                                LOACITY = LOACITY.Replace("  ", " ");
                            }
                            else
                            {
                                MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                            return;
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(_StartStreet))
                    {
                        var _StartCitys = _StartStreet.Split(' ');


                        //if (_StartCitys[0] == "제주시")
                        //{
                        //    _StartCitys[0] = "제주시시";
                        //}



                        if (_StartCitys.Length == 3)
                        {
                            //if (_StartCitys[0].Length > 2)
                            //{
                            //    _startcity = _StartCitys[0].Substring(0, _StartCitys[0].Trim().Length - 1) + _StartCitys[1];
                            //}
                            //else
                            //{
                            //    _startcity = _StartCitys[0].Trim() + _StartCitys[1];

                            //}
                            _startcity = _AddressCityParse(_StartCitys[0]) + _StartCitys[1];

                            _StartDetail = _StartCitys[2];
                        }
                        else
                        {
                            //if (_StartCitys[0].Length > 2)
                            //{
                            //    _startcity = _StartCitys[0].Substring(0, _StartCitys[0].Trim().Length - 1);
                            //}
                            //else
                            //{
                            //    _startcity = _StartCitys[0].Trim();

                            //}
                            _startcity = _AddressCityParse(_StartCitys[0]);
                            _StartDetail = _StartCitys[1];
                        }

                    }
                    else
                    {

                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                        return;


                    }


                    if (!String.IsNullOrEmpty(startaddr1))
                    {
                        if (_startcity.Contains("고양일산"))
                        {
                            _startcity = "고양일산";
                        }
                        startaddr3 = cargoApiClass.get_address_search(_AddressStateParse(CargoOrder.StartState) + "" + _startcity + "" + _StartDetail, _StartDetail, ApiUrl);

                    }
                    else
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }





                    if (String.IsNullOrEmpty(startaddr3))
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }
                    var ss = startaddr3.Split(',');

                    LOACODE = ss[0];
                    POIX = ss[1];
                    POIY = ss[2];


                    LOACITY = ss[3] + " " + ss[4] + " " + ss[5] + " " + ss[6];

                    LOACITY = LOACITY.Replace("  ", " ");
                }



                #endregion 출발지

                #region 하차지

                string stopaddr1 = cargoApiClass.Get_addr_search1(_AddressStateParse(CargoOrder.StopState), ApiUrl);
                //  string stopaddr2 = "";
                string stopaddr3 = "";
                string _Stopstreet = "";
                string _StopDetail = "";

                string _stopcity = "";

                if (_AddressGubun != "S")
                {
                    _Stopstreet = cargoApiClass.roadaddr_trans(CargoOrder.StopState + " " + CargoOrder.StopCity + " " + CargoOrder.StopStreet + " " + CargoOrder.StopDetail);
                }
                if (string.IsNullOrEmpty(_Stopstreet))
                {
                    var EditStopStreet = CargoOrder.StopStreet;
                    if (!string.IsNullOrEmpty(EditStopStreet))
                    {
                        if (EditStopStreet.Length > 2)
                        {
                            if (EditStopStreet == "동남구" && EditStopStreet == "서북구")
                            {

                            }
                            else
                            {
                                if (CargoOrder.StopStreet.Substring(CargoOrder.StopStreet.Length - 1, 1) == "시" || CargoOrder.StopStreet.Substring(CargoOrder.StopStreet.Length - 1, 1) == "구" || CargoOrder.StopStreet.Substring(CargoOrder.StopStreet.Length - 1, 1) == "읍")
                                {
                                    EditStopStreet = _AddressCityParse(EditStopStreet);
                                }
                            }
                        }
                    }


                    //화물맨주소검색
                    var _Q = _CargoAddrSearch.Where(c => c.SIDO + "" + c.GU + "" + c.GUN + "" + c.DONG == _AddressStateParse(CargoOrder.StopState) + "" + _AddressCityParse(CargoOrder.StopCity) + "" + EditStopStreet);

                    if (_Q.Any())
                    {
                        DOWCODE = _Q.First().AREACODE;
                        POIX_OUT = _Q.First().POIX;
                        POIY_OUT = _Q.First().POIY;
                        DOWCITY = _Q.First().SIDO + " " + _Q.First().GU + " " + _Q.First().GUN + " " + _Q.First().DONG;
                        DOWCITY = DOWCITY.Replace("  ", " ");
                    }

                    else
                    {
                        if (!String.IsNullOrEmpty(CargoOrder.StopState) && !String.IsNullOrEmpty(CargoOrder.StopCity))
                        {

                            CargoAddress cargoAddress = new CargoAddress(CargoOrder.StopState, CargoOrder.StopCity, CargoOrder.StopStreet);
                            cargoAddress.StartPosition = FormStartPosition.CenterParent;
                            if (cargoAddress.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                // _Stopstreet = cargoApiClass.roadaddr_trans(CargoOrder.StopState + " " + CargoOrder.StopCity + " " + cargoAddress._DONG);
                                DOWCODE = cargoAddress._AREACODE;
                                POIX_OUT = cargoAddress._POIX;
                                POIY_OUT = cargoAddress._POIY;


                                DOWCITY = cargoAddress._SIDO + " " + cargoAddress._Gu_Gun + " " + cargoAddress._DONG;

                                DOWCITY = DOWCITY.Replace("  ", " ");





                            }
                            else
                            {
                                MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                            return;
                        }
                    }


                }
                else
                {
                    if (!string.IsNullOrEmpty(_Stopstreet))
                    {
                        var _StopCitys = _Stopstreet.Split(' ');


                        //if (_StopCitys[0] == "제주시")
                        //{
                        //    _StopCitys[0] = "제주시시";
                        //}


                        if (_StopCitys.Length == 3)
                        {
                            //if (_StopCitys[0].Length > 2)
                            //{
                            //    _stopcity = _StopCitys[0].Substring(0, _StopCitys[0].Trim().Length - 1) + _StopCitys[1];
                            //}
                            //else
                            //{
                            //    _stopcity = _StopCitys[0].Trim() + _StopCitys[1];

                            // }

                            _stopcity = _AddressCityParse(_StopCitys[0]) + _StopCitys[1];


                            _StopDetail = _StopCitys[2];

                        }
                        else
                        {
                            //if (_StopCitys[0].Length > 2)
                            //{
                            //    _stopcity = _StopCitys[0].Substring(0, _StopCitys[0].Trim().Length - 1);
                            //}
                            //else
                            //{
                            //    _stopcity = _StopCitys[0].Trim();

                            //}

                            _stopcity = _AddressCityParse(_StopCitys[0]);

                            _StopDetail = _StopCitys[1];
                        }

                    }
                    else
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }


                    if (!String.IsNullOrEmpty(stopaddr1))
                    {
                        if (_stopcity.Contains("일산"))
                        {
                            _stopcity = "일산";
                        }
                        // startaddr2 = cargoApiClass.get_addr_search2(startaddr1, _startcity);
                        stopaddr3 = cargoApiClass.get_address_search(_AddressStateParse(CargoOrder.StopState) + "" + _stopcity + "" + _StopDetail, _StopDetail, ApiUrl);
                        // startaddr =  cargoApiClass.get_addr_searchname(startaddr2, _startcity, _AddressStateParse(CargoOrder.StartState), _StartStreet);
                    }
                    else
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }





                    if (String.IsNullOrEmpty(stopaddr3))
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }
                    var st = stopaddr3.Split(',');



                    DOWCODE = st[0];

                    POIX_OUT = st[1];
                    POIY_OUT = st[2];

                    DOWCITY = st[3] + " " + st[4] + " " + st[5] + " " + st[6];
                    DOWCITY = DOWCITY.Replace("  ", " ");
                }





                #endregion 하차지

                #region 선/착불
                if (CargoOrder.PayLocation == 2)
                {
                    if (CargoOrder.StartPrice != 0 && CargoOrder.StopPrice != 0)
                    {
                        MessageBox.Show($"\"화물맨\" 연동시에는\r\n선불금액 또는 착불금액 한가지만 입력하셔야 합니다.\r\n수정후 화물맨에 등록하시기바랍니다.", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }
                }
                #endregion

            }

            CarTypechange = CarType.Text;
            pnProgress.Visible = true;
            Task.Factory.StartNew(() =>
            {
                bar.Invoke(new Action(() =>
                {
                    bar.PerformStep();
                }));
                #region
                var MOVETYPELIST = cargoApiClass.Get_code_config("MOVETYPE", ApiUrl);

                var TONLIST = cargoApiClass.Get_code_config("TON", ApiUrl);

                var CARTYPELIST = cargoApiClass.Get_code_config("CARTYPE", ApiUrl);

                var ALONELIST = cargoApiClass.Get_code_config("ALONE", ApiUrl);

                var STATELIST = cargoApiClass.Get_code_config("STATE", ApiUrl);

                var PAYMETHODLIST = cargoApiClass.Get_code_config("PAYMETHOD", ApiUrl);


                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    Int32 IOrderCode = Convert.ToInt32(_ORDERCODE);
                    var CargoOrder = ShareOrderDataSet.Orders.Where(c => c.OrderId == IOrderCode).FirstOrDefault();


                    //주소찾기




                    _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + CargoApiClass.SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);







                    #region 상차지
                    //string startaddr1 = cargoApiClass.Get_addr_search1(_AddressStateParse(CargoOrder.StartState), ApiUrl);
                    ////     string startaddr2 = "";
                    //string startaddr3 = "";
                    //string _StartStreet = "";
                    //string _StartDetail = "";
                    //string _startcity = "";

                    //if (CargoOrder.StartCity == "제주시")
                    //{
                    //    CargoOrder.StartCity = "제주시시";
                    //}

                    //_StartStreet = cargoApiClass.roadaddr_trans(CargoOrder.StartState + " " + CargoOrder.StartCity + " " + CargoOrder.StartStreet + " " + CargoOrder.StartDetail);

                    //if (!string.IsNullOrEmpty(_StartStreet))
                    //{
                    //    var _StartCitys = _StartStreet.Split(' ');

                    //    if (_StartCitys.Length == 3)
                    //    {
                    //        if (_StartCitys[0].Length > 2)
                    //        {
                    //            _startcity = _StartCitys[0].Substring(0, _StartCitys[0].Trim().Length - 1) + _StartCitys[1];
                    //        }
                    //        else
                    //        {
                    //            _startcity = _StartCitys[0].Trim() + _StartCitys[1];

                    //        }
                    //        _StartDetail = _StartCitys[2];
                    //    }
                    //    else
                    //    {
                    //        if (_StartCitys[0].Length > 2)
                    //        {
                    //            _startcity = _StartCitys[0].Substring(0, _StartCitys[0].Trim().Length - 1);
                    //        }
                    //        else
                    //        {
                    //            _startcity = _StartCitys[0].Trim();

                    //        }
                    //        _StartDetail = _StartCitys[1];
                    //    }

                    //}
                    //else
                    //{
                    //    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);



                    //    Invoke(new Action(() => pnProgress.Visible = false));

                    //    Invoke(new Action(() => _Sort = false));
                    //    Invoke(new Action(() => _Search()));
                    //    Invoke(new Action(() => _NewOrder()));
                    //    Invoke(new Action(() => SetRowBackgroundColor()));
                    //    return;
                    //}


                    //if (!String.IsNullOrEmpty(startaddr1))
                    //{
                    //    // startaddr2 = cargoApiClass.get_addr_search2(startaddr1, _startcity);
                    //    startaddr3 = cargoApiClass.get_address_search(_AddressStateParse(CargoOrder.StartState) + "" + _startcity + "" + _StartDetail, _StartDetail, ApiUrl);
                    //    // startaddr =  cargoApiClass.get_addr_searchname(startaddr2, _startcity, _AddressStateParse(CargoOrder.StartState), _StartStreet);
                    //}
                    //else
                    //{
                    //    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);
                    //    Invoke(new Action(() => pnProgress.Visible = false));

                    //    Invoke(new Action(() => _Sort = false));
                    //    Invoke(new Action(() => _Search()));
                    //    Invoke(new Action(() => _NewOrder()));
                    //    Invoke(new Action(() => SetRowBackgroundColor()));
                    //    return;
                    //}





                    //if (String.IsNullOrEmpty(startaddr3))
                    //{
                    //    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);
                    //    Invoke(new Action(() => pnProgress.Visible = false));

                    //    Invoke(new Action(() => _Sort = false));
                    //    Invoke(new Action(() => _Search()));
                    //    Invoke(new Action(() => _NewOrder()));
                    //    Invoke(new Action(() => SetRowBackgroundColor()));
                    //    return;
                    //}
                    //var ss = startaddr3.Split(',');
                    //// var lc = startaddr.Split(',');
                    //var LOACODE = ss[0];
                    //var POIX = ss[1];
                    //var POIY = ss[2];

                    ////if (String.IsNullOrEmpty(startaddr))
                    ////{
                    ////    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);
                    ////    return;
                    ////}
                    //var LOACITY = ss[3] + " " + ss[4] + " " + ss[5] + " " + ss[6];

                    //LOACITY = LOACITY.Replace("  ", " ");
                    #endregion 상차지

                    #region 하차지

                    //string stopaddr1 = cargoApiClass.Get_addr_search1(_AddressStateParse(CargoOrder.StopState), ApiUrl);
                    ////  string stopaddr2 = "";
                    //string stopaddr3 = "";
                    //string _Stoptreet = "";
                    //string _StopDetail = "";

                    //string _stopcity = "";


                    //_Stoptreet = cargoApiClass.roadaddr_trans(CargoOrder.StopState + " " + CargoOrder.StopCity + " " + CargoOrder.StopStreet + " " + CargoOrder.StopDetail);






                    //if (!string.IsNullOrEmpty(_Stoptreet))
                    //{
                    //    var _StopCitys = _Stoptreet.Split(' ');


                    //    if (_StopCitys[0] == "제주시")
                    //    {
                    //        _StopCitys[0] = "제주시시";
                    //    }


                    //    if (_StopCitys.Length == 3)
                    //    {
                    //        if (_StopCitys[0].Length > 2)
                    //        {
                    //            _stopcity = _StopCitys[0].Substring(0, _StopCitys[0].Trim().Length - 1) + _StopCitys[1];
                    //        }
                    //        else
                    //        {
                    //            _stopcity = _StopCitys[0].Trim() + _StopCitys[1];

                    //        }
                    //        _StopDetail = _StopCitys[2];

                    //    }
                    //    else
                    //    {
                    //        if (_StopCitys[0].Length > 2)
                    //        {
                    //            _stopcity = _StopCitys[0].Substring(0, _StopCitys[0].Trim().Length - 1);
                    //        }
                    //        else
                    //        {
                    //            _stopcity = _StopCitys[0].Trim();

                    //        }
                    //        _StopDetail = _StopCitys[1];
                    //    }

                    //}
                    //else
                    //{
                    //    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                    //    Invoke(new Action(() => pnProgress.Visible = false));

                    //    Invoke(new Action(() => _Sort = false));
                    //    Invoke(new Action(() => _Search()));
                    //    Invoke(new Action(() => _NewOrder()));
                    //    Invoke(new Action(() => SetRowBackgroundColor()));
                    //    return;
                    //}


                    //if (!String.IsNullOrEmpty(stopaddr1))
                    //{
                    //    // startaddr2 = cargoApiClass.get_addr_search2(startaddr1, _startcity);
                    //    stopaddr3 = cargoApiClass.get_address_search(_AddressStateParse(CargoOrder.StopState) + "" + _stopcity + "" + _StopDetail, _StopDetail, ApiUrl);
                    //    // startaddr =  cargoApiClass.get_addr_searchname(startaddr2, _startcity, _AddressStateParse(CargoOrder.StartState), _StartStreet);
                    //}
                    //else
                    //{
                    //    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);
                    //    Invoke(new Action(() => pnProgress.Visible = false));

                    //    Invoke(new Action(() => _Sort = false));
                    //    Invoke(new Action(() => _Search()));
                    //    Invoke(new Action(() => _NewOrder()));
                    //    Invoke(new Action(() => SetRowBackgroundColor()));
                    //    return;
                    //}





                    //if (String.IsNullOrEmpty(stopaddr3))
                    //{
                    //    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);
                    //    Invoke(new Action(() => pnProgress.Visible = false));

                    //    Invoke(new Action(() => _Sort = false));
                    //    Invoke(new Action(() => _Search()));
                    //    Invoke(new Action(() => _NewOrder()));
                    //    Invoke(new Action(() => SetRowBackgroundColor()));
                    //    return;
                    //}
                    //var st = stopaddr3.Split(',');

                    //// var st = stopaddr3.Split(',');
                    //// var dc = stopaddr.Split(',');
                    ////if (string.IsNullOrEmpty(stopaddr3))
                    ////{
                    ////    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);
                    ////    return;
                    ////}

                    ////if (string.IsNullOrEmpty(stopaddr))
                    ////{
                    ////    MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);
                    ////    return;
                    ////}


                    //var DOWCODE = st[0];

                    //var POIX_OUT = st[1];
                    //var POIY_OUT = st[2];

                    //var DOWCITY = st[3] + " " + st[4] + " " + st[5] + " " + st[6];
                    //DOWCITY = DOWCITY.Replace("  ", " ");
                    #endregion 하차지
                    string CARTON = "";
                    //var _CARTON = TONLIST.Where(c => c.VALUE.Replace("톤", "") == CarSize.Text).ToArray();
                    decimal dCarsize = 0;
                    if (decimal.TryParse(_Carsize, out dCarsize))
                    {


                        if (dCarsize == 7.5m)
                        {
                            dCarsize = 8m;
                        }
                        else if (dCarsize == 15m)
                        {
                            dCarsize = 16m;
                        }
                        _Carsize = dCarsize.ToString() + "톤";

                        if (dCarsize < 1)
                        {
                            _Carsize = "1톤미만";
                        }
                    }


                    var _CARTON = TONLIST.Where(c => c.VALUE == _Carsize).ToArray();

                    if (_CARTON.Any())
                    {

                        CARTON = TONLIST.Where(c => c.VALUE == _Carsize).FirstOrDefault().KEY;

                    }
                    else
                    {

                        CARTON = TONLIST.Where(c => c.VALUE == "기타").FirstOrDefault().KEY;

                    }



                    if (CarTypechange == "카고+윙바디")
                    {
                        CarTypechange = "카고/윙바디";
                    }
                    else if (CarTypechange == "츄레라")
                    {
                        CarTypechange = "추레라";
                    }

                    var _CARTYPE = CARTYPELIST.Where(c => c.VALUE == CarTypechange).ToArray();
                    string CARTYPE = "";
                    if (_CARTYPE.Any())
                    {
                        CARTYPE = CARTYPELIST.Where(c => c.VALUE == CarTypechange).FirstOrDefault().KEY;



                    }
                    else
                    {
                        CARTYPE = CARTYPELIST.Where(c => c.VALUE == "카고").FirstOrDefault().KEY;
                    }

                    var LOADTYPE = "";
                    //var OWID = LocalUser.Instance.LogInInformation.Client.CargoLoginId;  //"화물맨";
                    var OWID = LocalUser.Instance.LogInInformation.CargoLoginId;  //"화물맨";
                    var PAYMENT = "";
                    string SATYPE = "";
                    string HATYPE = "";
                    string PAY = "";
                    string FEE = "";
                    if (CargoOrder.IsShared)
                    {
                        LOADTYPE = ALONELIST.Where(c => c.VALUE == "혼적").FirstOrDefault().KEY;
                    }
                    else
                    {
                        LOADTYPE = ALONELIST.Where(c => c.VALUE == "독차").FirstOrDefault().KEY;
                    }




                    switch (CargoOrder.PayLocation)
                    {
                        //인수증
                        case 1:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "인수증").FirstOrDefault().KEY;

                            break;
                        //선/착불
                        case 2:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "선불").FirstOrDefault().KEY;
                            break;
                        //인수증+선/착불
                        case 4:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "선불").FirstOrDefault().KEY;
                            break;
                        default:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "인수증").FirstOrDefault().KEY;
                            break;
                    }

                    if (String.IsNullOrEmpty(CargoOrder.StartInfo))
                    {
                        SATYPE = MOVETYPELIST.Where(c => c.VALUE == "일반").FirstOrDefault().KEY;
                    }
                    else
                    {
                        SATYPE = MOVETYPELIST.Where(c => c.VALUE == CargoOrder.StartInfo).FirstOrDefault().KEY;
                    }

                    if (String.IsNullOrEmpty(CargoOrder.StopInfo))
                    {
                        HATYPE = MOVETYPELIST.Where(c => c.VALUE == "일반").FirstOrDefault().KEY;
                    }
                    else
                    {
                        HATYPE = MOVETYPELIST.Where(c => c.VALUE == CargoOrder.StopInfo).FirstOrDefault().KEY;
                    }



                    string WEIGHT = "";
                    var CargoCarSize = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value == CargoOrder.CarSize).ToArray();


                    if (!String.IsNullOrEmpty(CargoOrder.ItemSize) && CargoOrder.ItemSize != "0")
                    {
                        WEIGHT = CargoOrder.ItemSize;

                    }
                    else
                    {
                        WEIGHT = (CargoCarSize.FirstOrDefault().Number * 1.1d).ToString("N1");
                    }


                    string url = "";
                    if (CargoGubun == "수정")
                    {
                        url = $"{ApiUrl}/service2/csr/set_cargo_update";
                    }
                    else
                    {
                        url = $"{ApiUrl}/service2/csr/set_cargo_regist";
                    }
                    if (PAYMENT == "payMethod01")
                    {
                        //착불
                        if (CargoOrder.StartPrice == 0)
                        {
                            //착불
                            PAYMENT = "payMethod03";
                            PAY = Convert.ToInt64(CargoOrder.StopPrice - CargoOrder.DriverPrice).ToString().Replace(",", "");
                        }
                        //선불
                        else if (CargoOrder.StopPrice == 0)
                        {

                            PAY = Convert.ToInt64(CargoOrder.StartPrice - CargoOrder.DriverPrice).ToString().Replace(",", "");
                        }
                        //선착불
                        else if (CargoOrder.StartPrice != 0 && CargoOrder.StopPrice != 0)
                        {
                            PAY = Convert.ToInt64(CargoOrder.StartPrice + CargoOrder.StopPrice - CargoOrder.DriverPrice).ToString().Replace(",", "");
                        }

                    }
                    //선/착불이 아닌경우
                    else
                    {
                        //PAY = Convert.ToInt64(CargoOrder.ClientPrice).ToString().Replace(",", "");
                        //지불운임/기사운임
                        PAY = Convert.ToInt64(CargoOrder.TradePrice + CargoOrder.AlterPrice).ToString().Replace(",", "");
                    }

                    string json = "{" +
                                    "\"COMCODE\":\"207\"," +
                                    "\"ORDERCODE\":\"" + CargoOrder.OrderId + "\"" +
                                    ",\"HASH\":\"" + _SHA256HASH + "\"" +
                                    ",\"LOADAY\":\"" + CargoOrder.StartTime.ToString("yyyy-MM-dd HH:mm") + "\"" +
                                    ",\"LOACITY\":\"" + LOACITY + "\"" +//+ CargoOrder.StartState + " " + CargoOrder.StartCity + " " + CargoOrder.StartStreet + "\"" +
                                    ",\"LOACODE\":\"" + LOACODE + "\"" +
                                    ",\"POIX\":\"" + POIX + "\"" +
                                    ",\"POIY\":\"" + POIY + "\"" +
                                    ",\"DOWDAY\":\"" + CargoOrder.StopTime.ToString("yyyy-MM-dd HH:mm") + "\"" +
                                    ",\"DOWCITY\":\"" + DOWCITY + "\"" +//+ CargoOrder.StopState + " " + CargoOrder.StopCity + " " + CargoOrder.StopStreet + "\"" +
                                    ",\"DOWCODE\":\"" + DOWCODE + "\"" +
                                    ",\"POIX_OUT\":\"" + POIX_OUT + "\"" +
                                    ",\"POIY_OUT\":\"" + POIY_OUT + "\"" +
                                    ",\"CARTON\":\"" + CARTON + "\"" +
                                    ",\"CARTYPE\":\"" + CARTYPE + "\"" +
                                    ",\"LOADTYPE\":\"" + LOADTYPE + "\"" +
                                    ",\"OWID\":\"" + OWID + "\"" +
                                    ",\"OWNAME\":\"" + LocalUser.Instance.LogInInformation.Client.Name + "\"" +
                                    ",\"PAYMENT\":\"" + PAYMENT + "\"" +
                                    ",\"PAY\":\"" + PAY + "\"" +
                                    ",\"FEE\":\"" + Convert.ToInt64(CargoOrder.DriverPrice).ToString().Replace(",", "") + "\"" +
                                    ",\"INFO\":\"" + CargoOrder.Remark + "\"" +
                                    ",\"ETC\":\"" + CargoOrder.Etc + "\"" +
                                    ",\"PHONE\":\"" + LocalUser.Instance.LogInInformation.Client.PhoneNo + "\"" +
                                    ",\"SATYPE\":\"" + SATYPE + "\"" +
                                    ",\"HATYPE\":\"" + HATYPE + "\"" +
                                    ",\"WEIGHT\":\"" + WEIGHT + "\"" +

                                  "}";
                    JObject response = null;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.ContentType = "application/json";
                        request.Method = "POST";

                        using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                        {
                            stream.Write(json);
                            stream.Flush();
                            stream.Close();
                        }

                        HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                        {

                            result = reader.ReadToEnd();


                            response = JObject.Parse(result);

                            msg = response["msg"].ToString();
                            code = response["code"].ToString();

                            if (code == "200")
                            {
                                Data.Connection(_Connection =>
                                {
                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "Update Orders SET CargoApiYN = 'Y' , CargoApiStatus = @CargoGubun WHERE OrderId = @OrderId ";

                                        _Command.Parameters.AddWithValue("@OrderId", CargoOrder.OrderId);
                                        _Command.Parameters.AddWithValue("@CargoGubun", CargoGubun);

                                        _Command.ExecuteNonQuery();
                                    }
                                });

                                if (String.IsNullOrEmpty(CargoStatus))
                                {
                                    //if (_CarCountValue != 0 && _i != 0)
                                    //{
                                    //    if (_CarCountValue == _i + 1)
                                    //    {
                                    //        MessageBox.Show($"\"화물맨\"에 {CargoGubun}되었습니다", "화물맨 연동", MessageBoxButtons.OK);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    MessageBox.Show($"\"화물맨\"에 {CargoGubun}되었습니다", "화물맨 연동", MessageBoxButtons.OK);

                                    //}
                                }



                            }
                            else
                            {
                                if (msg.Contains("ordercode"))
                                {
                                    MessageBox.Show("화물번호가 중복되었습니다");
                                }
                                else if (msg.Contains("loaday"))
                                {
                                    MessageBox.Show("상차시간이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("loacity"))
                                {
                                    MessageBox.Show("상차지가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("dowday"))
                                {
                                    MessageBox.Show("하차시간이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("dowcity"))
                                {
                                    MessageBox.Show("하차지가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("carton"))
                                {
                                    MessageBox.Show("차량톤수 올바르지 않습니다.");
                                }
                                else if (msg.Contains("cartype"))
                                {
                                    MessageBox.Show("차량타입 올바르지 않습니다.");
                                }
                                else if (msg.Contains("loadtype"))
                                {
                                    MessageBox.Show("혼적구분이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("ow_name"))
                                {
                                    MessageBox.Show("화주이름이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("payment"))
                                {
                                    MessageBox.Show("결제방식이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("pay"))
                                {
                                    MessageBox.Show("운임료가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("fee"))
                                {
                                    MessageBox.Show("수수료가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("info"))
                                {
                                    MessageBox.Show("화물정보가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("etc"))
                                {
                                    MessageBox.Show("비고가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("real_phone"))
                                {
                                    MessageBox.Show("핸드폰번호가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("SangType"))
                                {
                                    MessageBox.Show("상차방법이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("HaType"))
                                {
                                    MessageBox.Show("하차방법이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("LocationCode"))
                                {
                                    MessageBox.Show("지역코드가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("weight"))
                                {
                                    MessageBox.Show("화물실중량 값이 올바르지 않습니다.");
                                }
                                else
                                {
                                    MessageBox.Show(msg);
                                }

                            }



                        }


                    }
                    catch
                    {


                    }

                }
                #endregion


                Invoke(new Action(() => pnProgress.Visible = false));

                //Invoke(new Action(() => _Sort = false));
                //Invoke(new Action(() => _Search()));
                //Invoke(new Action(() => _NewOrder()));
                //Invoke(new Action(() => SetRowBackgroundColor()));


            });

        }

        private void button4_Click(object sender, EventArgs e)
        {






        }

       

        #region

        #endregion


        //화물맨에 취소하기
       


        //화물맨에 등록하기
        private void CargoRegistToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (DataListSource.Current != null && DataListSource.Current is Order Current)
            {
                if (Current.OrderStatus == 3)
                {
                    MessageBox.Show("배차 완료된 건 입니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (Current.CargoApiYN != "Y")
                    {

                        setCargoRegist("등록", Current.OrderId);


                    }
                }

            }

            //setCargoRegist("등록");

            //_Sort = false;
            //_Search();
            //_NewOrder();
            //SetRowBackgroundColor();
            //if (ChkRequestDate.Checked)
            //{
            //    dtpRequestDate.Value = _CreateTime;
            //}



            _Sort = false;
            _Search();


            SetRowBackgroundColor();
        }

       

     



        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    if (sender as Control != null)
            //    {
            //        Button pnShortCut = sender as Control as Button;
            //        if (pnShortCut.Enabled)
            //        {
            //            pnShortCut.BackColor = Color.Green;
            //        }
            //    }
            //}
            //catch { }
        }
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            //try
            //{
            //    if (sender as Control != null)
            //    {
            //        Button pnShortCut = sender as Control as Button;
            //        // pnShortCut.Invalidate();
            //        pnShortCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            //    }
            //}
            //catch { }

        }

        private void Button_MouseLeave1(object sender, EventArgs e)
        {
        //    try
        //    {
        //        if (sender as Control != null)
        //        {
        //            Button pnShortCut = sender as Control as Button;
        //            // pnShortCut.Invalidate();
        //            pnShortCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        //        }
        //    }
        //    catch { }

        }
        private void UpdateOrder_EnabledChanged(object sender, EventArgs e)
        {

        }


        void button_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UpdateOrder_EnabledChanged_1(object sender, EventArgs e)
        {

        }

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }

        private void CarSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                CarType.Focus();
            }
        }

        private void CarType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                ItemSize.Focus();
            }
        }

        private void ItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {

                PayLocation.Focus();
            }
        }

        private void PayLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == Convert.ToChar(Keys.Enter))
            //{
            //    //선착불
            //    if (PayLocation.SelectedIndex == 2)
            //    {
            //        Price4.Focus();
            //    }
            //    else if (PayLocation.SelectedIndex == 1)
            //    {
            //        Price2.Focus();
            //    }
            //    else if (PayLocation.SelectedIndex == 3)
            //    {
            //        Price2.Focus();
            //    }
            //}
        }

        private void Price2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == Convert.ToChar(Keys.Enter))
            //{
            //    Price1.Focus();
            //}
        }

        private void Price1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                Item.Focus();
            }
        }

        private void Price4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == Convert.ToChar(Keys.Enter))
            //{
            //    Price5.Focus();
            //}
        }

        private void Price6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                Item.Focus();
            }
        }

        private void Price5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                //Price6.Focus();
            }

        }
    }

    
   

}
