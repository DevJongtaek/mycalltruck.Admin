using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using mycalltruck.Admin.Class;
using static mycalltruck.Admin.CMDataSet;
using System.Threading;
using System.IO;
using OfficeOpenXml;
using System.Net;
using mycalltruck.Admin.Class.DataSet;
using System.Text.RegularExpressions;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using mycalltruck.Admin.Class.XML;
using mycalltruck.Admin.CMDataSetTableAdapters;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301Default : Form
    {
        private bool ValidateIgnore = false;
        private bool IsCurrentNull = false;
        //ShareOrderDataSet ShareOrderDataSet = null;
        bool HasLoaded = false;
        Action LoadAction = null;
        int _MNSTOrderId = 0;
        string _MCreateTime = "";
        string _MGubun = string.Empty;
        bool HideMyCarOrder = false;
        //0.용차
        //1.지입
        int OrderGubun = 0;
        int _KakaoPriceGubun = 2;

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
                    
                    CopyToApplication.Enabled = false;
                    btnOrderAfterAdd.Enabled = false;
                    btnExcel.Enabled = false;
                    tableLayoutPanel9.Enabled = false;
                    grid1.ReadOnly = true;
                  
                    break;
            }
        }

        public FrmMN0301Default(string _mGubun = "", int _mNSTOrderId = 0,string _mCreateTime = "")
        {


            //this.ShareOrderDataSet = ShareOrderDataSet;
            InitializeComponent();
            _MNSTOrderId = _mNSTOrderId;
            _MCreateTime = _mCreateTime;
            _MGubun = _mGubun;
            SearchTextFilter.SelectedIndex = 0;
            SearchDriverTrade.SelectedIndex = 0;
            SearchCustomerTax.SelectedIndex = 0;
            SelectApplication.SelectedIndex = 0;
            cmbSMonth.SelectedIndex = 0;
            cmbRequest.SelectedIndex = 4;
            MouseClick += NControl_MouseClick;
            DateFilterBegin.Value = DateTime.Now.Date;
            DateFilterEnd.Value = DateTime.Now.Date;
            dtpRequestDate.Value = DateTime.Now;
            SetMouseClick(this);
            SetStaticOptions();
            SetDriverList();
            SetCustomerList();
            InitDriverTable();
            InitCustomerStartTable();
            InitClientTable();

            //grid1
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);
            //requestday.DefaultCellStyle.ForeColor = Color.Blue;
            //requestday.DefaultCellStyle.SelectionForeColor = Color.Blue;

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                if (LocalUser.Instance.LogInInformation.Client.CustomerPay)
                    Price3.ReadOnly = true;
            }
            
                //SelectApplication.Visible = true;
                //CopyToApplication.Visible = true;

            

            //DateFilterBegin.ValueChanged += (c, d) =>
            //{
            //    DateTime eDate = DateFilterEnd.Value.Date;
            //    DateTime sDate = DateFilterBegin.Value.AddMonths(3).Date;


            //    if (eDate >= sDate)
            //    {
            //        MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return;
            //    }
            //    _Sort = false;
            //    _Search();
            //};
            //DateFilterEnd.ValueChanged += (c, d) =>
            //{
            //    if (DateFilterEnd.Value.AddMonths(-3).Date >= DateFilterBegin.Value.Date)
            //    {
            //        MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return;
            //    }
            //    _Sort = false;
            //    _Search();
            //};
            InitClientListView();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }

        private void FrmMN0301_Load(object sender, EventArgs e)
        {
            //rdoWe.TabStop = false;
            //rdoJusun.TabStop = false;

            LocalUser.Instance.LogInInformation.LoadClient();
            HideMyCarOrder = LocalUser.Instance.LogInInformation.Client.HideMyCarOrder;
            OrderGubun = LocalUser.Instance.LogInInformation.Client.OrderGubun;
            
            _KakaoPriceGubun = LocalUser.Instance.LogInInformation.Client.KakaoPriceGubun;
            if (HideMyCarOrder == false)
            {
                ShareSearch.Enabled = false;
                chkHideMyCarOrder.Checked = false;
            }
            else
            {
                chkHideMyCarOrder.Checked = true;
                ShareSearch.Enabled = true;
            }

           
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

        }
        BindingList<ModelDefalut> _CoreList = new BindingList<ModelDefalut>();
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
                        _CoreList.Add(new ModelDefalut
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

        }


        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _DriverTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,MobileNo,CarNo,CarYear,CarSize,CarType,Password FROM Drivers ";

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
        }


        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name, LoginId, isnull(PhoneNo,''),isnull(MobileNo,'')  FROM Clients ";
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
                    commnad.CommandText = "SELECT  idx, StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,SGubun FROM CustomerStartManage ";

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
            NewOrder_Click(null, null);
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
            PayLocation.SelectedIndex = 0;

            UnitType.DisplayMember = "Name";
            UnitType.ValueMember = "Value";
            UnitType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Unit").ToList());
            UnitType.SelectedIndex = 0;



            cmb_CarSize.DisplayMember = "Name";
            cmb_CarSize.ValueMember = "Value";
            cmb_CarSize.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").ToList());
            cmb_CarSize.SelectedValue = 0;

            cmb_CarType.DisplayMember = "Name";
            cmb_CarType.ValueMember = "Value";
            cmb_CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").ToList());
            cmb_CarType.SelectedValue = 0;

            DriverGrade.DisplayMember = "Name";
            DriverGrade.ValueMember = "Value";
            DriverGrade.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "DriverGrade").ToList());
            DriverGrade.SelectedValue = 0;
            //_SetStaticOption(SharedItemLength, "SharedItemLength");




            initReferralId();



        }

        private void initReferralId()
        {
            Dictionary<int, string> DCustomer = new Dictionary<int, string>();
            customersNewTableAdapter.Fill(clientDataSet.CustomersNew);
            //  customersTableAdapter.Fill(clientDataSet.CustomersNew);
            var cmbCustomerMIdDataSource = clientDataSet.CustomersNew.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.SangHo).ToArray();

            DCustomer.Add(0, "위탁사");


            foreach (var item in cmbCustomerMIdDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }


            cmb_ReferralId.DataSource = new BindingSource(DCustomer, null);
            cmb_ReferralId.DisplayMember = "Value";
            cmb_ReferralId.ValueMember = "Key";
            cmb_ReferralId.SelectedValue = 0;
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
                    _Command.CommandText = $"SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo, Customers.PointMethod, Customers.Fee, Customers.AddressState, Customers.AddressCity, Customers.AddressDetail, Customers.BizNo,Customers.MobileNo,ISNULL(CustomerManager.ManagerName,''),BizGubun FROM Customers " +
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
                    _Command.CommandText = $"SELECT Distinct Drivers.DriverId, CEO, Name, CarNo, MobileNo, CarType, CarSize, CarYear,CandidateId" +
                       // $", dbo._GetDriverPoint(Drivers.DriverId) as DriverPoint" +
                        $", 0 as DriverPoint " +
                        $" FROM Drivers JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId WHERE Drivers.ServiceState != 5 ";
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

        private void InitClientListView()
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                List<ClientListViewModelDefault> ClientDataSource = new List<ClientListViewModelDefault>();
                ClientDataSource.Add(new ClientListViewModelDefault { ClientId = 0, Name = "전체" });
                ClientDataSource.Add(new ClientListViewModelDefault { ClientId = LocalUser.Instance.LogInInformation.ClientId, Name = LocalUser.Instance.LogInInformation.Client.Name });

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
                                ClientDataSource.Add(new ClientListViewModelDefault { ClientId = _Reader.GetInt32(0), Name = _Reader.GetString(1) });
                            }
                        }
                    }
                });
                ClientListView.DisplayMember = "Name";
                ClientListView.ValueMember = "ClientId";
                ClientListView.DataSource = ClientDataSource;
                ClientListView.SelectedIndex = 1;


                if(LocalUser.Instance.LogInInformation.AllowSub  == true)
                {
                    ClientListView.Visible = true;

                }
                else
                {
                    ClientListView.Visible = false;
                }
                //if (ClientDataSource.Count == 1)
                //{
                //    ClientListView.Visible = false;
                //    // ColumnClientId.Visible = false;
                //}
                //else
                //{
                //    ClientListView.Visible = true;
                //    // ColumnClientId.Visible = true;
                //}




                ClientListView.SelectedIndexChanged += (p, q) => {
                    _Sort = false;
                    _Search();
                };
            }
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
        private void _Search(int OrderState = 0)
        {
            var Now = DateFilterBegin.Value.Date;
            var PNow = DateTime.Now.AddMonths(-3).AddDays(1).Date;
            var Tommorow = DateFilterEnd.Value.AddDays(1).Date;
            var ClientId = (int)ClientListView.SelectedValue;
            grid1.AutoGenerateColumns = false;
            ValidateIgnore = true;
            List<Order> DataSource = new List<Order>();
            List<Order> DataSource2 = new List<Order>();
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                if (ClientId != 0)
                {
                    switch (OrderState)
                    {
                        //전체
                        case 0:
                            if (_Sort == false)
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId  && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();
                                    }
                                }
                                else if(_SortGubun == "Time")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId  && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                                    }
                                }
                                else if (_SortGubun == "Start")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();
                                    }
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();
                                    }
                                }
                                
                                _Sort = true;

                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                    }

                                }
                                else if (_SortGubun == "Time")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                                    }

                                }
                                else if (_SortGubun == "Start")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();
                                    }

                                }
                                else if (_SortGubun == "Stop")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder && c.OrderStatus != 3)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();
                                    }

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


                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();

                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();

                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();

                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();

                                }
                                _Sort = true;
                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == false && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();
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

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();
                                }
                                _Sort = true;
                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.ClientId == ClientId) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();
                                }
                                _Sort = false;

                            }
                            break;
                        //예약
                        case 4:
                            if (_Sort == false)
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId  && c.CreateTime >= Tommorow).OrderByDescending(c => c.Customer).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderByDescending(c => c.Customer).ToList();
                                    }
                                }
                                else if (_SortGubun == "Time")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderByDescending(c => c.CreateTime).ToList();
                                    }
                                }
                                else if (_SortGubun == "Start")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.StartName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderByDescending(c => c.StartName).ToList();
                                    }
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.StopName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderByDescending(c => c.StopName).ToList();
                                    }
                                }
                                _Sort = true;
                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.Customer).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderBy(c => c.Customer).ToList();
                                    }
                                }

                                else if (_SortGubun == "Time")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId  && c.CreateTime >= Tommorow).OrderBy(c => c.CreateTime).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderBy(c => c.CreateTime).ToList();
                                    }
                                }
                                else if (_SortGubun == "Start")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.StartName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderBy(c => c.StartName).ToList();
                                    }
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.StopName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && (c.ClientId == ClientId || (c.ClientId != ClientId && c.MyCarOrder == HideMyCarOrder)) && c.CreateTime >= Tommorow).OrderBy(c => c.StopName).ToList();
                                    }
                                }
                                _Sort = false;
                            }
                            break;
                        //예약/접수/대기/완료/공유/취소
                        case 5:
                            if (_Sort == false)
                            {
                                if (_SortGubun == "ClientId")
                                {

                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList();

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList();

                                }
                                else if (_SortGubun == "Time")
                                {
                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList();
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList();

                                }
                                else if (_SortGubun == "Start")
                                {
                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList();
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList();

                                }
                                else if (_SortGubun == "Stop")
                                {
                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList();

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderByDescending(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList();
                                }
                                _Sort = true;
                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList();
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList();

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList();


                                }
                                else if (_SortGubun == "Start")
                                {
                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList();

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList();


                                }
                                else if (_SortGubun == "Stop")
                                {
                                    //DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList();

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.ClientId == ClientId && c.CreateTime >= Tommorow).OrderBy(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 2) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.ClientId == ClientId && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList();


                                }
                                _Sort = false;
                            }
                            break;
                        case 6:
                            DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderId == _MNSTOrderId).ToList();
                            //  GridIndex = ;


                            break;
                        case 7:
                            if (_Sort == false)
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();

                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();

                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();

                                }
                                _Sort = true;
                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    if (HideMyCarOrder == false)
                                    {

                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                    }
                                }
                                else if (_SortGubun == "Time")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder  && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                                    }
                                }
                                else if (_SortGubun == "Start")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();
                                    }
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    if (HideMyCarOrder == false)
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();
                                    }
                                    else
                                    {
                                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();
                                    }
                                }
                                _Sort = false;

                            }
                            break;
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

                        case 9:
                            DateTime _SMCreateTime = Convert.ToDateTime(_MCreateTime).Date;
                            DateTime _EMCreateTime = Convert.ToDateTime(_MCreateTime).AddDays(1).Date;

                            DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.CreateTime >= _SMCreateTime && c.CreateTime < _EMCreateTime).ToList();
                            break;

                    }
                }
                else
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
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderByDescending(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();

                                }
                                _Sort = true;
                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.Customer).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.CreateTime).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();


                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.StartName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();


                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == false && c.CreateTime >= Tommorow).OrderBy(c => c.StopName).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 3) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 1) && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList().Union(DataSource.Concat(ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => (c.OrderStatus == 0) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName))).ToList();

                                    DataSource = DataSource.Where(c => ClientDataSourceList.Contains((int)c.ClientId)).ToList();


                                }
                                _Sort = false;
                            }
                            break;
                        case 6:
                            DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderId == _MNSTOrderId).ToList();
                            //  GridIndex = ;


                            break;
                        //공유목록
                        case 7:
                            if (_Sort == false)
                            {
                                if (_SortGubun == "ClientId")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.Customer).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.CreateTime).ToList();

                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StartName).ToList();

                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && c.MyCarOrder == HideMyCarOrder && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderByDescending(c => c.StopName).ToList();

                                }
                                _Sort = true;
                            }
                            else
                            {
                                if (_SortGubun == "ClientId")
                                {

                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.Customer).ToList();
                                }
                                else if (_SortGubun == "Time")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.CreateTime).ToList();
                                }
                                else if (_SortGubun == "Start")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StartName).ToList();
                                }
                                else if (_SortGubun == "Stop")
                                {
                                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 1 && ((c.ClientId == ClientId && c.MyCarOrder == HideMyCarOrder) || (c.ClientId != ClientId && c.MyCarOrder == true)) && c.CreateTime >= Now && c.CreateTime < Tommorow).OrderBy(c => c.StopName).ToList();
                                }
                                _Sort = false;

                            }
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
                }
                if (!String.IsNullOrEmpty(SearchText.Text))
                {
                    switch (SearchTextFilter.SelectedIndex)
                    {
                        case 1:
                            DataSource = DataSource.Where(c => c.Driver != null && c.DriverModel.Name.Contains(SearchText.Text)).ToList();
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

                if(SearchDriverTrade.SelectedIndex > 0)
                {

                    switch (SearchDriverTrade.SelectedIndex)
                    {
                        case 1:
                            DataSource = DataSource.Where(c => c.TradeModel != null ).ToList();
                            break;
                        case 2:
                            DataSource = DataSource.Where(c => c.TradeModel == null).ToList();
                            break;
                      

                    }
                }


                if (SearchCustomerTax.SelectedIndex > 0)
                {

                    switch (SearchCustomerTax.SelectedIndex)
                    {
                        case 1:
                            DataSource = DataSource.Where(c => c.SalesManageId != null).ToList();
                            break;
                        case 2:
                            DataSource = DataSource.Where(c => c.SalesManageId == null).ToList();
                            break;


                    }
                }
                if (chkMyOrder.Checked)
                {
                    DataSource = DataSource.Where(c => c.OrdersLoginId == LocalUser.Instance.LogInInformation.LoginId || c.OrdersAcceptId == LocalUser.Instance.LogInInformation.LoginId).ToList();
                }


                DataListSource.DataSource = DataSource;
            }
            //ShareOrderDataSet = ShareOrderDataSet.Instance;
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

                //공유
                else if (Model.OrderStatus == 1 && Model.MyCarOrder == true)
                {
                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Blue;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Blue;
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

                    //for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    //{
                    //    grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Black;
                    //    grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Black;
                    //}
                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                    {
                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Black;
                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Black;
                    }

                    DriverRepository mDriverRepository = new DriverRepository();
                    if (Model.DriverId != 0)
                    {
                       // DriverRepository.Driver Query = null;
                         var Query = mDriverRepository.GetDriver((int)Model.DriverId);

                        if (Query == null)
                            return;


                        if (String.IsNullOrEmpty(Query.BizNo) || String.IsNullOrEmpty(Query.Name) || String.IsNullOrEmpty(Query.CEO) || String.IsNullOrEmpty(Query.Uptae) || String.IsNullOrEmpty(Query.Upjong) || Query.Name == "." || Query.CEO == "." || Query.Upjong == "." || Query.Uptae == "." || String.IsNullOrEmpty(Query.Addresstate) || String.IsNullOrEmpty(Query.AddressCity) || String.IsNullOrEmpty(Query.AddressDetail) || String.IsNullOrEmpty(Query.PayBankName) || String.IsNullOrEmpty(Query.PayAccountNo) || String.IsNullOrEmpty(Query.PayInputName))
                        {
                            for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                            {
                                grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Green;
                                grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Green;
                            }

                            if (Query.BizNo.Length > 0)
                            {
                                if (Query.BizNo.Substring(0, 3) == "000" || Query.BizNo.Substring(0, 3) == "999")
                                {
                                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                                    {
                                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Green;
                                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Green;
                                    }
                                }
                            }

                            // e.CellStyle.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (Query.BizNo.Length > 0)
                            {
                                if (Query.BizNo.Substring(0, 3) == "000" || Query.BizNo.Substring(0, 3) == "999")
                                {
                                    for (int _ColumnIndex = 0; _ColumnIndex < grid1.ColumnCount; _ColumnIndex++)
                                    {
                                        grid1[_ColumnIndex, RowIndex].Style.ForeColor = Color.Green;
                                        grid1[_ColumnIndex, RowIndex].Style.SelectionForeColor = Color.Green;
                                    }
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
                    }


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

        private void SetMouseClick(Control mControl)
        {
            foreach (Control nControl in mControl.Controls)
            {
                if (nControl == StartSelect || nControl == StopSelect
                    || nControl == CustomerSelectContainer || nControl == DriverSelectContainer)
                {
                    continue;
                }
                nControl.MouseClick += NControl_MouseClick;
                SetMouseClick(nControl);
            }
        }

        private void NControl_MouseClick(object sender, MouseEventArgs e)
        {
            StartSelect.Visible = false;
            StopSelect.Visible = false;
            CustomerSelectContainer.Visible = false;
            DriverSelectContainer.Visible = false;
            //JusunSelect.Visible = false;
        }

        private void Customer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataListSource.Current != null)
            {
                var Current = DataListSource.Current as Order;
                if (Current.Agubun == 4)
                    return;
            }


            if (Customer.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    CustomerSelect.Items.Clear();
                    int ItemIndex = 0;
                    SetCustomerList();
                    foreach (var _Customer in CustomerModelList.Where(c => c.SangHo.ToLower().Contains(Customer.Text.ToLower()) || c.PhoneNo.Replace("-", "").Contains(Customer.Text.Replace("-", ""))))
                    {
                        CustomerSelect.Items.Add(_Customer.SangHo);
                        CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.PhoneNo);
                        CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.AddressState + " " + _Customer.AddressCity);
                        CustomerSelect.Items[ItemIndex].Tag = _Customer.CustomerId;
                        ItemIndex++;
                    }
                    //if (CustomerSelect.Items.Count == 0)
                    //{
                    //    SetCustomerList();
                    //    foreach (var _Customer in CustomerModelList.Where(c => c.SangHo.Contains(Customer.Text) || c.PhoneNo.Replace("-", "").Contains(Customer.Text.Replace("-", ""))))
                    //    {
                    //        CustomerSelect.Items.Add(_Customer.SangHo);
                    //        CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.PhoneNo);
                    //        CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.AddressState + " " + _Customer.AddressCity);
                    //        CustomerSelect.Items[ItemIndex].Tag = _Customer.CustomerId;
                    //        ItemIndex++;
                    //    }
                    //}
                    if (CustomerSelect.Items.Count > 0)
                    {
                        NewSangHo.Clear();

                        CustomerSelect.Items[0].Selected = true;

                        CustomerSelect.Focus();
                    }
                    else
                    {
                        NewSangHo.Text = Customer.Text;

                        Customer.Clear();
                        Customer.Tag = null;

                        
                    }

                    CustomerSelectContainer.Visible = true;
                    CustomerSelect.Focus();
                }
            }
            else
            {
                if (e.KeyChar == 13)
                {

                    CustomerSelect.Items.Clear();
                    NewSangHo.Clear();
                    NewCMobileNo.Clear();
                    NewCPhoneNo.Clear();
                    CustomerSelectContainer.Visible = true;
                    CustomerSelect.Focus();

                }
            }


        }

        private void CustomerSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (CustomerSelect.SelectedItems.Count > 0)
                {
                    _CustomerSelected();
                }
            }
        }

        private void CustomerSelect_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in CustomerSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _CustomerSelected();
                    }
                }
            }
        }
        int GridIndex = 0;
        private void _CustomerSelected()
        {
            Customer.Clear();
            var _Customer = CustomerModelList.Find(c => c.CustomerId == (int)CustomerSelect.SelectedItems[0].Tag);

            Customer.Text = _Customer.SangHo;
            OrderPhoneNo.Text = _Customer.PhoneNo;
            Customer.Tag = _Customer.CustomerId;
            CustomerManager.Text = _Customer.CustomerManager;
          
            if (_Customer.Fee > 0)
            {
                decimal.TryParse(Price1.Text.Replace(",", ""), out decimal _Price);
                Price3.Text = (_Price * 0.01m * _Customer.Fee).ToString("N0");
            }
            CustomerSelectContainer.Visible = false;

            //상차지정보 추가
            InitCustomerStartTable();

            var _CustomerStartCount = _CustomerStartTable.Where(c => c.CustomerId == _Customer.CustomerId && c.SGubun == "S").ToArray();
            var _CustomerStopCount = _CustomerStartTable.Where(c => c.CustomerId == _Customer.CustomerId && c.SGubun == "E").ToArray();
            if (_CustomerStartCount.Count() > 0 || _CustomerStopCount.Count() > 0)
            {
                if (_CustomerStartCount.Count() > 0)
                {
                    FrmCustomerStartList f = new FrmCustomerStartList();
                    f.Search(_Customer.CustomerId);
                    f.StartPosition = FormStartPosition.CenterParent;

                    f.ShowDialog();


                    if (!String.IsNullOrEmpty(f.StartState) && !String.IsNullOrEmpty(f.StartCity))
                    {

                        if (f.StartGubun == "본사")
                        {
                            OrderPhoneNo.Text = f.StartPhoneNo;
                            //StartPhoneNo.Text = f.StartPhoneNo;
                        }
                        else
                        {
                            OrderPhoneNo.Text = _Customer.PhoneNo;
                            //StartPhoneNo.Text = f.StartPhoneNo;
                        }

                        StartState.Text = f.StartState;
                        StartCity.Text = f.StartCity;
                        StartStreet.Text = f.StartStreet;

                        Start.Text = f.StartDetail;
                        if (Start.Text.Length > 30)
                        {
                            Start.Text = Start.Text.Substring(0, 29);
                        }
                        StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;


                    }
                }

                if (_CustomerStopCount.Count() > 0)
                {
                    FrmCustomerStoptList fe = new FrmCustomerStoptList();
                    fe.Search(_Customer.CustomerId);
                    fe.StartPosition = FormStartPosition.CenterParent;

                    fe.ShowDialog();

                    if (!String.IsNullOrEmpty(fe.StartState) && !String.IsNullOrEmpty(fe.StartCity))
                    {

                        if (fe.StartGubun == "본사")
                        {

                            //StopPhoneNo.Text = fe.StartPhoneNo;
                        }
                        else
                        {

                           // StopPhoneNo.Text = fe.StartPhoneNo;
                        }

                        StopState.Text = fe.StartState;
                        StopCity.Text = fe.StartCity;
                        StopStreet.Text = fe.StartStreet;

                        Stop.Text = fe.StartDetail;
                        if (Stop.Text.Length > 30)
                        {
                            Stop.Text = Stop.Text.Substring(0, 29);
                        }
                        StopName.Text = _AddressStateParse(StopState.Text) + " " + StopCity.Text;


                    }

                }

                   

            }
            else
            {
                if (!String.IsNullOrEmpty(_Customer.AddressState) && String.IsNullOrEmpty(StartState.Text))
                {
                    StartState.Text = _Customer.AddressState;
                    StartCity.Text = _Customer.AddressCity;
                    StartStreet.Text = "";
                    Start.Text = _Customer.AddressDetail;
                    if (Start.Text.Length > 30)
                    {
                        Start.Text =  Start.Text.Substring(0, 29);
                    }
                    StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                    OrderPhoneNo.Text = _Customer.PhoneNo;
                    //StartPhoneNo.Text = _Customer.PhoneNo;

                }
            }

            //Customer.Focus();
            Start.Focus();
            Start.SelectAll();
           



            if (grid1.CurrentCell == null)
            {
                return;
            }
            var Current = DataListSource.Current as Order;

            if (grid1.CurrentCell != null)
            {
                if (Current.SalesManageId == null)
                {
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "Update Orders SET Customer = @Customer,CustomerId = @CustomerId,OrderPhoneNo = @OrderPhoneNo   WHERE OrderId = @OrderId "; ;
                            _Command.Parameters.AddWithValue("@Customer", Customer.Text);
                            _Command.Parameters.AddWithValue("@CustomerId", Customer.Tag);
                            _Command.Parameters.AddWithValue("@OrderPhoneNo", OrderPhoneNo.Text);
                            _Command.Parameters.AddWithValue("@OrderId", Current.OrderId);
                           // _Command.Parameters.AddWithValue("@StartPhoneNo", StartPhoneNo.Text);
                            _Command.ExecuteNonQuery();
                        }
                    });

                    if (grid1.RowCount > 0)
                    {
                        GridIndex = DataListSource.Position;
                        _Sort = false;
                        _Search();
                        grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];

                        DataListSource_CurrentChanged(null, null);
                    }
                    else
                    {
                        _Sort = false;
                        _Search();
                    }

                }
                else
                {
                    MessageBox.Show("화주 계산서 발행 건은 수정 할 수 있습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;

                }
                // MessageBox.Show("화주정보 가 수정되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //_Search();
            }
        }

        private void Driver_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Driver.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    //if (LocalUser.Instance.LogInInformation.Client.NeedCustomerId && Price1.Text == "0")
                    //{
                    //    MessageBox.Show("배차를 입력하기 전, 먼저 운송료를 설정하여 주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    return;
                    //}
                 
                    DriverSelect.Items.Clear();
                    int ItemIndex = 0;
                    SetDriverList();
                    //var _D = DriverModelList.Where(c => c.CarNo.Contains(Driver.Text) || c.CarYear.Contains(Driver.Text) || c.MobileNo.Replace("-", "").Contains(Driver.Text.Replace("-", ""))).ToArray();
                    var _D = DriverModelList.Where(c => c.CarNo.Contains(Driver.Text) || c.CarYear.Contains(Driver.Text)).ToArray();

                    if (_D.Any())
                    {
                        foreach (var _Driver in _D)
                        {
                            DriverSelect.Items.Add(_Driver.CarYear);
                            
                            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == _Driver.CarType))
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == _Driver.CarType).Name);
                            }
                            else
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add("");
                            }
                            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == _Driver.CarSize))
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == _Driver.CarSize).Name);
                            }
                            else
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add("");
                            }

                            DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.MobileNo);
                            DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.CarNo);

                            if (DriverPoint.Checked)
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.DriverPoint.ToString("N0"));
                                Decimal.TryParse(Price3.Text.Replace(",", ""), out Decimal _Fee);
                                if (_Driver.DriverPoint < _Fee)
                                {
                                    DriverSelect.Items[ItemIndex].ForeColor = Color.Red;
                                }
                            }

                            DriverSelect.Items[ItemIndex].Tag = _Driver.DriverId;
                            ItemIndex++;
                        }
                    }
                    if (DriverSelect.Items.Count == 0)
                    {
                        SetDriverList();
                        foreach (var _Driver in DriverModelList.Where(c => c.CarYear.Contains(Driver.Text) || c.CarNo.Contains(Driver.Text)))
                        {
                            DriverSelect.Items.Add(_Driver.CarYear);
                            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == _Driver.CarType))
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == _Driver.CarType).Name);
                            }
                            else
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add("");
                            }
                            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == _Driver.CarSize))
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == _Driver.CarSize).Name);
                            }
                            else
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add("");
                            }

                            DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.MobileNo);
                            DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.CarNo);
                            DriverSelect.Items[ItemIndex].Tag = _Driver.DriverId;
                            ItemIndex++;
                        }
                    }
                    if (DriverSelect.Items.Count > 0)
                    {
                        DriverSelect.Items[0].Selected = true;

                        var _Driver = DriverModelList.Find(c => c.DriverId == (int)DriverSelect.Items[0].Tag);

                        NewCarYear.Text = _Driver.CarYear;
                        NewMobileNo.Text = _Driver.MobileNo;
                        NewCarNo.Text = _Driver.CarNo;
                        cmb_CarSize.SelectedValue = _Driver.CarSize;
                        cmb_CarType.SelectedValue = _Driver.CarType;
                    }
                    else
                    {
                        NewCarNo.Clear();
                        NewMobileNo.Clear();
                        NewCarYear.Clear();
                        cmb_CarSize.SelectedIndex = 0;
                        cmb_CarType.SelectedIndex = 0;
                    }
                    DriverSelectContainer.Visible = true;
                    DriverSelect.Focus();
                }
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    if (LocalUser.Instance.LogInInformation.Client.NeedCustomerId && Price1.Text == "0")
                    {
                        MessageBox.Show("배차를 입력하기 전, 먼저 운송료를 설정하여 주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    DriverSelect.Items.Clear();
                    NewCarNo.Clear();
                    NewMobileNo.Clear();
                    NewCarYear.Clear();
                    cmb_CarSize.SelectedIndex = 0;
                    cmb_CarType.SelectedIndex = 0;
                    DriverSelectContainer.Visible = true;
                    DriverSelect.Focus();
                }
            }
        }

        private void DriverSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (DriverSelect.SelectedItems.Count > 0)
                {
                    _DriverSelected();
                }
            }
        }

        private void DriverSelect_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    foreach (ListViewItem nListViewItem in DriverSelect.Items)
            //    {
            //        if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
            //        {
            //            _DriverSelected();
            //        }
            //    }
            //}
        }

        private void _DriverSelected()
        {

            if (PayLocation.SelectedIndex == 2 || PayLocation.SelectedIndex == 3 || PayLocation.SelectedIndex == 1)
            {
                if (cmb_ReferralId.SelectedIndex == 0)
                {
                    //MessageBox.Show("운송비가 선/착불,선/착불+인수증일 경우 위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cmb_ReferralId.Focus();
                    return;
                }
            }

            var _Driver = DriverModelList.Find(c => c.DriverId == (int)DriverSelect.SelectedItems[0].Tag);
            if (DriverPoint.Checked)
            {
                decimal.TryParse(Price6.Text.Replace(",", ""), out decimal _Fee);
                if (_Driver.DriverPoint < _Fee)
                {
                    if (MessageBox.Show("선택한 기사의 충전금이 부족합니다. 그래도 배차 하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                }
            }

            

            Driver.Clear();
            Driver.Tag = _Driver;
            DriverName.Text = _Driver.CarYear;
            DriverPhoneNo.Text = _Driver.MobileNo;
            DriverCarNo.Text = _Driver.CarNo;

            if (grid1.CurrentCell != null && DataListSource.Current is Order Current && Current.DriverId == null && Current.TradeId == null && (Current.OrderStatus == 1 || Current.OrderStatus == 2))
            {
                Current.DriverId = _Driver.DriverId;

                Current.Driver = _Driver.CarYear;
                Current.DriverCarModel = _Driver.Name;
                Current.DriverCarNo = _Driver.CarNo;
                Current.DriverPhoneNo = _Driver.MobileNo;
                Current.OrderStatus = 3;
                Current.TradePrice = 0;
                Current.SalesPrice = 0;
                Current.AlterPrice = 0;
                Current.StartPrice = 0;
                Current.StopPrice = 0;
                Current.DriverPrice = 0;
                Current.Price = 0;
                Current.ClientPrice = 0;

                // Current.ClientId = LocalUser.Instance.LogInInformation.ClientId;

                if (PayLocation.SelectedIndex == 2)
                {
                    Current.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                    Current.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                    Current.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));
                    Current.Price = Current.StartPrice.Value + Current.StopPrice.Value;
                    Current.ClientPrice = Current.StartPrice.Value + Current.StopPrice.Value - Current.DriverPrice;
                }
                else if (PayLocation.SelectedIndex == 1)
                {
                    Current.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                    Current.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                    Current.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));
                    Current.Price = Current.TradePrice.Value;
                    Current.ClientPrice = Current.SalesPrice;
                }
                 //인수증+선착불
                else if (PayLocation.SelectedIndex == 3)
                {
                    Current.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                    Current.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                    Current.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));

                    Current.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                    Current.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                    Current.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));

                    Current.Price = Current.TradePrice.Value; //Current.StartPrice.Value + Current.StopPrice.Value + Current.TradePrice.Value;
                    Current.ClientPrice = Current.SalesPrice; //Current.StartPrice.Value + Current.StopPrice.Value - Current.DriverPrice + Current.SalesPrice;

                }

                Current.AcceptTime = DateTime.Now;
                Current.Wgubun = "PC";
                //if (rdoJusun.Checked)
                //{
                //    if (!string.IsNullOrEmpty(txtJusun.Text))
                //    {
                //        Current.ReferralId = (int)txtJusun.Tag;
                //    }
                //    else
                //    {
                //        Current.ReferralId = 0;
                //    }

                //}
                //else
                //{
                //    Current.ReferralId = (int)cmb_ReferralId.SelectedValue;
                //}

                Current.ReferralId = (int)cmb_ReferralId.SelectedValue;
                Current.DriverGrade = (int)DriverGrade.SelectedValue;
                if (DriverPoint.Checked)
                {
                    Current.DriverPoint = Current.DriverPrice;
                }

                //if(StartMulti.Checked)
                //{
                //    Current.StartMulti = true;
                //}
                //else
                //{
                //    Current.StartMulti = false;
                //}


                //if (StopMulti.Checked)
                //{
                //    Current.StopMulti = true;
                //}
                //else
                //{
                //    Current.StopMulti = false;
                //}


                //Current.StartMulti = false;
                //Current.StopMulti = false;


                Current.CustomerManager = CustomerManager.Text;

                Current.OrdersAcceptId = LocalUser.Instance.LogInInformation.LoginId;


                Current.OrderClientId = LocalUser.Instance.LogInInformation.ClientId;
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
                    ShareOrderDataSet.SaveChanges();
                }
                Data.Connection((_Connection) =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "UPDATE Drivers SET CarYear = @CarYear,MobileNo = @MobileNo,CarType = @CarType,CarSize = @CarSize  WHERE DriverId = @DriverId";
                        _Command.Parameters.AddWithValue("@DriverId", Current.DriverId);
                        _Command.Parameters.AddWithValue("@CarYear", NewCarYear.Text);
                        _Command.Parameters.AddWithValue("@MobileNo", NewMobileNo.Text);
                        // _Command.Parameters.AddWithValue("@CarNo", NewCarNo.Text);
                        _Command.Parameters.AddWithValue("@CarType", (int)cmb_CarType.SelectedValue);
                        _Command.Parameters.AddWithValue("@CarSize", (int)cmb_CarSize.SelectedValue);

                        _Command.ExecuteNonQuery();
                    }
                    _Connection.Close();

                });


                _Sort = false;
                _Search();

                DataListSource.ResetBindings(false);
                SetRowBackgroundColor();
            }
            DriverSelectContainer.Visible = false;
        }

        private void Start_KeyPress(object sender, KeyPressEventArgs e)
        {
           

            if (Start.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    StartSelect.Items.Clear();
                    int ItemIndex = 0;

                    //_SearchAddress(Start.Text);

                    //foreach (var _Address in _CoreList.Where(c => c.Jibun.Contains(Start.Text)))
                    //{
                    //    var ss = _Address.Jibun.Split(' ');

                    //    var _ss = ss[0] + ' ' + ss[1] + ' ' + ss[2];


                    //    StartSelect.Items.Add(ss[0]);
                    //    StartSelect.Items[ItemIndex].SubItems.Add(ss[1]);
                    //    StartSelect.Items[ItemIndex].SubItems.Add(ss[2]);
                    //    ItemIndex++;
                    //}

                    foreach (var _Address in SingleDataSet.Instance.AddressReferences.Where(c => c.State.Contains(Start.Text) || c.City.Contains(Start.Text) || c.Street.Contains(Start.Text)))
                    {
                        StartSelect.Items.Add(_Address.State);
                        StartSelect.Items[ItemIndex].SubItems.Add(_Address.City);
                        StartSelect.Items[ItemIndex].SubItems.Add(_Address.Street);
                        ItemIndex++;
                    }
                    if (StartSelect.Items.Count > 0)
                    {
                        StartSelect.Items[0].Selected = true;
                        StartSelect.Visible = true;
                        StartSelect.Focus();
                    }
                    else
                    {
                        Start.Clear();
                    }
                }
            }
        }

        private void StartSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (StartSelect.SelectedItems.Count > 0)
                {
                    _StartSelected();
                }
            }
        }

        private void StartSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in StartSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _StartSelected();
                    }
                }
            }
        }

        private void _StartSelected()
        {
            Start.Clear();
            StartState.Text = StartSelect.SelectedItems[0].Text;
            StartCity.Text = StartSelect.SelectedItems[0].SubItems[1].Text;
            StartStreet.Text = StartSelect.SelectedItems[0].SubItems[2].Text;

            StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;


            StartSelect.Visible = false;
            Start.Focus();


        }

        private void Stop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Stop.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    StopSelect.Items.Clear();
                    int ItemIndex = 0;
                    foreach (var _Address in SingleDataSet.Instance.AddressReferences.Where(c => c.State.Contains(Stop.Text) || c.City.Contains(Stop.Text) || c.Street.Contains(Stop.Text)))
                    {
                        StopSelect.Items.Add(_Address.State);
                        StopSelect.Items[ItemIndex].SubItems.Add(_Address.City);
                        StopSelect.Items[ItemIndex].SubItems.Add(_Address.Street);
                        ItemIndex++;
                    }
                    if (StopSelect.Items.Count > 0)
                    {
                        StopSelect.Items[0].Selected = true;
                        StopSelect.Visible = true;
                        StopSelect.Focus();
                    }
                    else
                    {
                        Stop.Clear();
                    }
                }
            }
        }

        private void StopSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (StopSelect.SelectedItems.Count > 0)
                {
                    _StopSelected();
                }
            }
        }

        private void StopSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in StopSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _StopSelected();
                    }
                }
            }
        }

        private void _StopSelected()
        {
            Stop.Clear();
            StopState.Text = StopSelect.SelectedItems[0].Text;
            StopCity.Text = StopSelect.SelectedItems[0].SubItems[1].Text;
            StopStreet.Text = StopSelect.SelectedItems[0].SubItems[2].Text;
            StopName.Text = _AddressStateParse(StopState.Text) + " " + StopCity.Text;
            StopSelect.Visible = false;
            Stop.Focus();
        }

        private void StartInfo_CheckedChanged(object sender, EventArgs e)
        {
            //var _StartInfo = sender as CheckBox;
            //if (_StartInfo.Checked)
            //{
            //    if (_StartInfo != StartInfo1)
            //        StartInfo1.Checked = false;
            //    if (_StartInfo != StartInfo2)
            //        StartInfo2.Checked = false;
            //    if (_StartInfo != StartInfo3)
            //        StartInfo3.Checked = false;
            //    if (_StartInfo != StartInfo4)
            //        StartInfo4.Checked = false;
            //    if (_StartInfo != StartInfo5)
            //        StartInfo5.Checked = false;
            //}
            //SetRemark();
        }

        private void StopInfo_CheckedChanged(object sender, EventArgs e)
        {
            //var _StopInfo = sender as CheckBox;
            //if (_StopInfo.Checked)
            //{
            //    if (_StopInfo != StopInfo1)
            //        StopInfo1.Checked = false;
            //    if (_StopInfo != StopInfo2)
            //        StopInfo2.Checked = false;
            //    if (_StopInfo != StopInfo3)
            //        StopInfo3.Checked = false;
            //    if (_StopInfo != StopInfo4)
            //        StopInfo4.Checked = false;
            //    if (_StopInfo != StopInfo5)
            //        StopInfo5.Checked = false;
            //}
            //SetRemark();
        }

        private void NotShared_CheckedChanged(object sender, EventArgs e)
        {
            //if (NotShared.Checked)
            //{
            //    Shared.Checked = false;
            //    SharedItemLength.SelectedIndex = 0;
            //    SharedItemSize.SelectedIndex = 0;
            //    SharedItemLength.Enabled = false;
            //    SharedItemSize.Enabled = false;
            //}
            //else
            //{
            //    if (Shared.Checked == false)
            //    {
            //        NotShared.Checked = true;
            //    }
            //}
            //SetRemark();
        }

        private void Shared_CheckedChanged(object sender, EventArgs e)
        {
            //if (Shared.Checked)
            //{
            //    NotShared.Checked = false;
            //    SharedItemLength.Enabled = true;
            //    SharedItemSize.Enabled = true;
            //}
            //else
            //{
            //    if (NotShared.Checked == false)
            //    {
            //        Shared.Checked = true;
            //    }
            //    else
            //    {
            //        SharedItemLength.SelectedIndex = 0;
            //        SharedItemSize.SelectedIndex = 0;
            //        SharedItemLength.Enabled = false;
            //        SharedItemSize.Enabled = false;
            //    }
            //}
            //SetRemark();
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
            //if (StartTimeType1.Checked == false)
            //{
            //    //if(StartTimeType2.Checked == false && StartTimeType3.Checked == false && String.IsNullOrEmpty(StartTimeType4.Text))
            //    //{
            //    //    StartTimeType1.Checked = true;
            //    //}
            //}
            //else
            //{
            //    StartTimeType2.Checked = false;
            //    StartTimeType3.Checked = false;
            //    StartTimeType4.Clear();
            //    StartTimeHour1.SelectedIndex = 0;
            //    StartTimeHour1.Enabled = false;
            //    StartTimeHour2.SelectedIndex = 0;
            //    StartTimeHour2.Enabled = false;
            //    StartTimeHalf.Checked = false;
            //    StartTimeHalf.Enabled = false;

            //    StartTimeETC.Checked = false;
            //    StartTimeETC.Enabled = false;
               
            //}
            //SetRemark();
        }

        private void StartTimeType2_CheckedChanged(object sender, EventArgs e)
        {
            //if (StartTimeType2.Checked == false)
            //{
            //    //if (StartTimeType1.Checked == false && StartTimeType3.Checked == false && String.IsNullOrEmpty(StartTimeType4.Text))
            //    //{
            //    //    StartTimeType2.Checked = true;
            //    //}
            //}
            //else
            //{
            //    StartTimeType1.Checked = false;
            //    StartTimeType3.Checked = false;
            //    StartTimeType4.Clear();
            //    StartTimeHour1.Enabled = true;
            //    StartTimeHour2.Enabled = true;
            //    StartTimeHalf.Enabled = true;
            //    StartTimeETC.Enabled = true;
            //}
            //SetRemark();
        }

        private void StartTimeType3_CheckedChanged(object sender, EventArgs e)
        {
            //if (StartTimeType3.Checked == false)
            //{
            //    //if (StartTimeType1.Checked == false && StartTimeType2.Checked == false && String.IsNullOrEmpty(StartTimeType4.Text))
            //    //{
            //    //    StartTimeType3.Checked = true;
            //    //}
            //}
            //else
            //{
            //    StartTimeType1.Checked = false;
            //    StartTimeType2.Checked = false;
            //    StartTimeType4.Clear();
            //    StartTimeHour1.Enabled = true;
            //    StartTimeHour2.Enabled = true;
            //    StartTimeHalf.Enabled = true;
            //    StartTimeETC.Enabled = true;
            //}
            //SetRemark();
        }

        private void StartTimeType4_TextChanged(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(StartTimeType4.Text))
            //{
            //    StartTimeType1.Checked = false;
            //    StartTimeType2.Checked = false;
            //    StartTimeType3.Checked = false;
            //    StartTimeHour1.Enabled = true;
            //    StartTimeHour2.Enabled = true;
            //    StartTimeHalf.Enabled = true;
            //    StartTimeETC.Enabled = true;
            //}
            //SetRemark();
        }

        private void StartTimeHour1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //StartTimeHour2.SelectedIndex = 0;
            //SetRemark();
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
            //StartInfo1.Checked = false;
            //StartInfo2.Checked = false;
            //StartInfo3.Checked = false;
            //StartInfo4.Checked = false;
            //StartInfo5.Checked = false;
            //StartTimeType1.Checked = false;
            //StartTimeType2.Checked = false;
            //StartTimeType3.Checked = false;
            //StartTimeType4.Clear();
            //StartTimeHour1.SelectedIndex = 0;
            //StartTimeHour2.SelectedIndex = 0;
            //StartTimeHalf.Checked = false;
            //StartTimeHour1.Enabled = false;
            //StartTimeHour2.Enabled = false;
            //StartTimeHalf.Enabled = false;

            //StartTimeETC.Checked = false;
            //StartTimeETC.Enabled = false;
            //StopDateHelper.SelectedIndex = 0;
            SetRemark();
        }

        private void StopTimeType1_CheckedChanged(object sender, EventArgs e)
        {
            //if (StopTimeType1.Checked)
            //{
            //    StopTimeType2.Checked = false;
            //    StopTimeType3.Checked = false;
            //    StopTimeType4.Checked = false;
            //    StopTimeType5.Clear();
            //    StopTimeHour1.Enabled = true;
            //    StopTimeHour2.Enabled = true;
            //    StopTimeHalf.Enabled = true;
            //    StopDateHelper.SelectedIndex = 1;
            //}
           
            SetRemark();
        }

        private void StopTimeType2_CheckedChanged(object sender, EventArgs e)
        {
            //if (StopTimeType2.Checked)
            //{
            //    StopTimeType1.Checked = false;
            //    StopTimeType3.Checked = false;
            //    StopTimeType4.Checked = false;
            //    StopTimeType5.Clear();
            //    StopTimeHour1.Enabled = true;
            //    StopTimeHour2.Enabled = true;
            //    StopTimeHalf.Enabled = true;
            //    StopDateHelper.SelectedIndex = 2;
            //}
            SetRemark();
        }

        private void StopTimeType3_CheckedChanged(object sender, EventArgs e)
        {
            //if (StopTimeType3.Checked)
            //{
            //    StopTimeType2.Checked = false;
            //    StopTimeType1.Checked = false;
            //    StopTimeType4.Checked = false;
            //    StopTimeType5.Clear();
            //    StopTimeHour1.Enabled = true;
            //    StopTimeHour2.Enabled = true;
            //    StopTimeHalf.Enabled = true;
            //    StopDateHelper.SelectedIndex = 3;
            //}
            //SetRemark();
        }

        private void StopTimeType4_CheckedChanged(object sender, EventArgs e)
        {
            //if (StopTimeType4.Checked)
            //{
            //    StopTimeType2.Checked = false;
            //    StopTimeType3.Checked = false;
            //    StopTimeType1.Checked = false;
            //    StopTimeType5.Clear();
            //    StopTimeHour1.SelectedIndex = 0;
            //    StopTimeHour2.SelectedIndex = 0;
            //    StopTimeHalf.Checked = false;
            //    StopTimeHour1.Enabled = false;
            //    StopTimeHour2.Enabled = false;
            //    StopTimeHalf.Enabled = false;
            //    StopDateHelper.SelectedIndex = 4;
            //}
            //SetRemark();
        }

        private void StopTimeType5_TextChanged(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(StopTimeType5.Text))
            //{
            //    StopTimeType1.Checked = false;
            //    StopTimeType2.Checked = false;
            //    StopTimeType3.Checked = false;
            //    StopTimeType4.Checked = false;
            //    StopTimeHour1.Enabled = true;
            //    StopTimeHour2.Enabled = true;
            //    StopTimeHalf.Enabled = true;
               
            //}
            //SetRemark();
        }

        private void StopTimeHour1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //StopTimeHour2.SelectedIndex = 0;
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
            //StopInfo1.Checked = false;
            //StopInfo2.Checked = false;
            //StopInfo3.Checked = false;
            //StopInfo4.Checked = false;
            //StopInfo5.Checked = false;
            //StopTimeType1.Checked = false;
            //StopTimeType2.Checked = false;
            //StopTimeType3.Checked = false;
            //StopTimeType4.Checked = false;
            //StopTimeType5.Clear();
            //StopTimeHour1.SelectedIndex = 0;
            //StopTimeHour2.SelectedIndex = 0;
            //StopTimeHalf.Checked = false;
            //StopTimeHour1.Enabled = false;
            //StopTimeHour2.Enabled = false;
            //StopTimeHalf.Enabled = false;
            //SetRemark();
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
            

            var mEtc = "";
            
            var _StopDateHelper = "";
            if (StopDateHelper.SelectedIndex != 0)
            {
               
                switch (StopDateHelper.SelectedIndex)
                {
                    case 1:
                        _StopDateHelper = "당착 ";
                        break;

                    case 2:
                        _StopDateHelper = "내일착 ";
                        break;
                    case 3:
                        _StopDateHelper = "월착 ";
                        break;
                    case 4:
                        _StopDateHelper = "당착/내착 ";
                        break;

                }


               
            }

            mEtc += _StopDateHelper + Item.Text;
           
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
            Driver.Enabled = true;

            btnDisable();

            Customer.Focus();
           // UpdateOrder.Enabled = false;
            // Customer.Enabled = true;
        }

        private void _NewOrder()
        {
            lblAOId.Text = "";
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
            Price1.Text = "0";
            Price2.Text = "0";
            Price3.Text = "0";
            Price4.Text = "0";
            Price5.Text = "0";
            Price6.Text = "0";
            Item.Text = "";
            ItemSize.Text = "0";
            ItemSizeInclude.Checked = false;
            //NotShared.Checked = true;
            //Emergency.Checked = false;
            //Round.Checked = false;
            //Reservation.Checked = false;
            //StartInfo1.Checked = false;
            //StartInfo2.Checked = false;
            //StartInfo3.Checked = false;
            //StartInfo4.Checked = false;
            //StartInfo5.Checked = false;
           // ClearStarTime_Click(null, null);
            ClearStopTime_Click(null, null);
            Customer.Clear();
            Customer.Tag = null;
            OrderPhoneNo.Clear();
            //StopPhoneNo.Clear();
            Driver.Clear();
            Driver.Tag = null;
            DriverPhoneNo.Text = "";
            DriverCarNo.Text = "";
            DriverPoint.Checked = false;
            HasTrade.Checked = false;
            HasSales.Checked = false;
            //StartMemo.Clear();
            //StopMemo.Clear();
            RequestMemo.Clear();
            CustomerManager.Clear();
            cmb_ReferralId.SelectedIndex = 0;
            DriverGrade.SelectedIndex = 0;
            //StartPhoneNo.Clear();
            //txtJusun.Clear();
            DriverName.Text = "";
            chkMyCarOrder.Checked = false;
            PayLocation.Enabled = true;
            Customer.Enabled = true;
            btnPrev.Enabled = true;
            //StartMulti.Checked = false;
            //StopMulti.Checked = false;

            UnitItem.Clear();
            UnitType.SelectedIndex = 0;




        }

        private void InsertOrder_Click(object sender, EventArgs e)
        {
            

            if (String.IsNullOrEmpty(StartState.Text) || String.IsNullOrEmpty(StopState.Text)
             || String.IsNullOrEmpty(Item.Text) )
            {
               
                //MessageBox.Show("상하차지 운송료/추가정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("먼저 화물정보를 입력 하십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
           
            if (PayLocation.SelectedIndex == 0)
            {
                MessageBox.Show("지불방식은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                PayLocation.Focus();
                return;
            }
            if(String.IsNullOrEmpty(Customer.Text) || Customer.Tag == null)
            {
                MessageBox.Show("화주정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Customer.Focus();
                return;
            }

            //if (PayLocation.SelectedIndex == 2 || PayLocation.SelectedIndex == 3)
            //{
            //    if (cmb_ReferralId.SelectedIndex == 0)
            //    {
            //        MessageBox.Show("운송비가 선/착불,선/착불+인수증일 경우 위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        cmb_ReferralId.Focus();
            //        return;
            //    }
            //}
            if (LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                if (Customer.Tag == null)
                {
                    MessageBox.Show("화주명을 선택하여 주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                var _Customer = CustomerModelList.Find(c => c.CustomerId == (int)Customer.Tag);
                var _Price = int.Parse(Price1.Text.Replace(",", ""));
                var _Fee = (decimal)(_Price * _Customer.Fee * 0.01m);
                Price3.Text = _Fee.ToString("N0");
                Price2.Text = (_Price + _Fee).ToString("N0");
            }
            int _ReferralId = 0;
            //if (rdoJusun.Checked)
            //{
            //    if (string.IsNullOrEmpty(txtJusun.Text))
            //    {
            //        _ReferralId = 0;
            //    }
            //    else
            //    {
            //        _ReferralId = (int)txtJusun.Tag;
            //    }
            //}
            //else
            //{
                _ReferralId = (int)cmb_ReferralId.SelectedValue;
            //}


            int _DriverGrade = 0;
            _DriverGrade = (int)DriverGrade.SelectedValue;


            bool _MyCarOrder = false;
            if (chkMyCarOrder.Checked)
            {
                _MyCarOrder = true;

            }
            else
            {
                _MyCarOrder = false;

            }

            bool _StartMulti = false;
            bool _StopMulti = false;
            //if (StartMulti.Checked)
            //{
            //    _StartMulti = true;
            //}
            //else
            //{
                _StartMulti = false;
            //}
            //if (StopMulti.Checked)
            //{
            //    _StopMulti = true;
            //}
            //else
            //{
                _StopMulti = false;
            //}
            var _CreateTime = DateTime.Now;
            int CarCountValue = Math.Max(int.Parse(CarCount.Text), 1);
            for (int i = 0; i < CarCountValue; i++)
            {
                if (ChkRequestDate.Checked)
                {
                    _CreateTime = Convert.ToDateTime(dtpRequestDate.Value.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                }
                else
                {
                    _CreateTime = DateTime.Now;
                }

                Order nOrder = new Order
                {
                    //CreateTime = Convert.ToDateTime(dtpRequestDate.Value.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")),
                    CreateTime = _CreateTime,
                    //CreateTime = DateTime.Now,
                    ClientId = LocalUser.Instance.LogInInformation.ClientId,
                    StartState = StartState.Text,
                    StartCity = StartCity.Text,
                    StartStreet = StartStreet.Text,
                    StartDetail = Start.Text,
                    StopState = StopState.Text,
                    StopCity = StopCity.Text,
                    StopStreet = StopStreet.Text,
                    StopDetail = Stop.Text,
                    CarSize = (int)CarSize.SelectedValue,
                    CarType = (int)CarType.SelectedValue,
                    StopDateHelper = (int)StopDateHelper.SelectedValue,
                    CarCount = 1,
                    PayLocation = (int)PayLocation.SelectedValue,
                    Item = Item.Text,
                    Remark = Remark.Text,
                    ItemSize = ItemSize.Text,
                    ItemSizeInclude = ItemSizeInclude.Checked,
                  //  IsShared = false,
                    SharedItemLength = 0,
                    SharedItemSize = 0,
                   // Emergency = false,
                   // Round = false,
                   // Reservation = false,
                    Customer = Customer.Text,
                    OrderPhoneNo = OrderPhoneNo.Text,
                    //StopPhoneNo = "",
                    //StartPhoneNo = "",
                    StartDate = DateTime.Now.Date,
                    StartTime = DateTime.Now.Date,
                    StopTime = DateTime.Now.Date,
                    // AccountMemo = AccountMemo.Text,
                    StartName = StartName.Text,
                    StopName = StopName.Text,
                   // StartMemo = "",//StartMemo.Text,
                   // StopMemo = "",//StopMemo.Text,
                    RequestMemo = RequestMemo.Text,

                    ReferralId = _ReferralId,
                    DriverGrade =_DriverGrade,
                    MyCarOrder = _MyCarOrder,
                    StartMulti = _StartMulti,
                    OrdersLoginId = LocalUser.Instance.LogInInformation.LoginId,
                    ReferralAccountYN = "N",

                    CustomerManager = CustomerManager.Text,
                    StopMulti = _StopMulti,
                    UnitItem = UnitItem.Text,
                    UnitType = (int)UnitType.SelectedValue,
                };
                nOrder.TradePrice = 0;
                nOrder.SalesPrice = 0;
                nOrder.AlterPrice = 0;
                nOrder.StartPrice = 0;
                nOrder.StopPrice = 0;
                nOrder.DriverPrice = 0;
                nOrder.Price = 0;
                nOrder.ClientPrice = 0;
                //선착불
                if (PayLocation.SelectedIndex == 2)
                {
                    nOrder.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                    nOrder.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                    nOrder.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));
                    nOrder.Price = nOrder.StartPrice.Value + nOrder.StopPrice.Value;
                    nOrder.ClientPrice = nOrder.StartPrice.Value + nOrder.StopPrice.Value - nOrder.DriverPrice;
                }
                //인수증
                else if (PayLocation.SelectedIndex == 1)
                {
                    nOrder.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                    nOrder.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                    nOrder.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));
                    nOrder.Price = nOrder.TradePrice.Value;
                    nOrder.ClientPrice = nOrder.SalesPrice;
                }
                //인수증+선착불
                else if (PayLocation.SelectedIndex == 3)
                {
                    nOrder.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                    nOrder.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                    nOrder.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));

                    nOrder.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                    nOrder.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                    nOrder.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));

                    nOrder.Price = nOrder.StartPrice.Value + nOrder.StopPrice.Value + nOrder.TradePrice.Value;
                    nOrder.ClientPrice = nOrder.StartPrice.Value + nOrder.StopPrice.Value - nOrder.DriverPrice + nOrder.SalesPrice;

                }


                string StartTime = "";
               
                if (String.IsNullOrEmpty(nOrder.OrderPhoneNo))
                {
                    nOrder.OrderPhoneNo = "";
                }
                if (Customer.Tag != null)
                {
                    if (CustomerModelList.Where(c => c.CustomerId == (int)Customer.Tag).Any())
                    {
                        nOrder.CustomerId = (int)Customer.Tag;
                        nOrder.PointMethod = CustomerModelList.Find(c => c.CustomerId == nOrder.CustomerId).PointMethod;
                    }
                    else
                    {
                        nOrder.CustomerId = null;
                     
                      
                    }
                }
                if (DriverPoint.Checked)
                    nOrder.DriverPoint = 0;
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    ShareOrderDataSet.Orders.Add(nOrder);
                    ShareOrderDataSet.SaveChanges();
                }
                if (nOrder.CustomerId == null && !String.IsNullOrEmpty(nOrder.OrderPhoneNo) && !String.IsNullOrEmpty(nOrder.Customer))
                {
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "SELECT ContractId FROM Contracts WHERE PhoneNo = @PhoneNo";
                            _Command.Parameters.AddWithValue("@PhoneNo", nOrder.OrderPhoneNo);
                            if (_Command.ExecuteScalar() != null)
                                return;
                        }
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "INSERT INTO Contracts (ClientId, PhoneNo, Name) VALUES (@ClientId, @PhoneNo, @Name)";
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            _Command.Parameters.AddWithValue("@PhoneNo", nOrder.OrderPhoneNo);
                            _Command.Parameters.AddWithValue("@Name", nOrder.Customer);
                            _Command.ExecuteNonQuery();
                        }
                    });
                }
                if (Driver.Tag != null)
                {
                    //직배송
                    var _Driver = Driver.Tag as DriverModel;
                    nOrder.DriverId = _Driver.DriverId;
                    nOrder.Driver = _Driver.CarYear;
                    nOrder.DriverCarModel = _Driver.Name;
                    nOrder.DriverCarNo = _Driver.CarNo;
                    nOrder.DriverPhoneNo = _Driver.MobileNo;
                    nOrder.OrderStatus = 3;
                    nOrder.AcceptTime = DateTime.Now;
                    nOrder.Wgubun = "PC";
                    nOrder.OrdersAcceptId = LocalUser.Instance.LogInInformation.LoginId;
                    if (DriverPoint.Checked)
                    {
                        nOrder.DriverPoint = nOrder.ClientPrice - nOrder.Price;
                    }
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        ShareOrderDataSet.Entry(nOrder).State = System.Data.Entity.EntityState.Modified;
                        ShareOrderDataSet.SaveChanges();
                    }
                  
                }
                else
                {
                    if (LocalUser.Instance.LogInInformation.NoticeDriver == 1)
                    {
                        //카카오톡배차
                        #region 카카오톡배차
                        var _NoticeDriver = 0;
                        var _NoticeCnt = 0;
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT NoticeDriver, NoticeCnt FROM Clients WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (_Reader.Read())
                                    {
                                        _NoticeDriver = _Reader.GetInt32(0);
                                        _NoticeCnt = _Reader.GetInt32(1);
                                    }
                                }
                            }
                            _Connection.Close();
                        }
                        if (_NoticeDriver == 1)
                        {

                            if (!LocalUser.Instance.LogInInformation.IsAdmin)
                            {
                                var iCarType = CarType.SelectedValue.ToString();
                                var iCarSize = CarSize.SelectedValue.ToString();
                                var iOrderStatus = 1;
                                if (iOrderStatus == 1)
                                {
                                    List<string> DriverMobileNoList = new List<string>();

                                    if (DriverMobileNoList.Count < _NoticeCnt)
                                    {
                                        int DriverIdCnt = _NoticeCnt - DriverMobileNoList.Count;
                                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                        {
                                            cn.Open();
                                            var Commad = cn.CreateCommand();
                                            Commad.CommandText = " SELECT Top " + _NoticeCnt + " NoticeIdx ,MobileNo" +
                                            " FROM NOTICEDRIVER " +
                                            " WHERE CarType = @CarType AND CarSize >= @CarSize AND ClientId = @ClientId" +
                                            " ORDER BY NEWID()";

                                            Commad.Parameters.AddWithValue("@DriverIdCnt", DriverIdCnt);
                                            Commad.Parameters.AddWithValue("@CarType", iCarType);
                                            Commad.Parameters.AddWithValue("@CarSize", iCarSize);
                                            Commad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                            var Reader = Commad.ExecuteReader();
                                            while (Reader.Read())
                                            {

                                                var DriverId = Reader.GetInt32(0);
                                                var MobileNo = Reader.GetString(1);

                                                DriverMobileNoList.Add(MobileNo);
                                            }
                                            cn.Close();
                                        }

                                    }

                                    if (DriverMobileNoList.Count > 0)
                                    {

                                        var StartTimeText = "지금";
                                        //if (StartTimeType2.Checked)
                                        //{
                                        //    StartTimeText = "오늘";
                                        //    if (StartTimeHour1.SelectedIndex > 0)
                                        //        StartTimeText += $" {StartTimeHour1.Text}";
                                        //    if (StartTimeHour2.SelectedIndex > 0)
                                        //        StartTimeText += $" {StartTimeHour2.Text}";
                                        //    if (StartTimeHalf.Checked)
                                        //        StartTimeText += " 30분";
                                        //    if (StartTimeETC.Checked)
                                        //    {
                                        //        StartTimeText += "이후";
                                        //    }

                                        //}
                                        //else if (StartTimeType3.Checked)
                                        //{
                                        //    StartTimeText = "내일";
                                        //    if (StartTimeHour1.SelectedIndex > 0)
                                        //        StartTimeText += $" {StartTimeHour1.Text}";
                                        //    if (StartTimeHour2.SelectedIndex > 0)
                                        //        StartTimeText += $" {StartTimeHour2.Text}";
                                        //    if (StartTimeHalf.Checked)
                                        //        StartTimeText += " 30분";
                                        //    if (StartTimeETC.Checked)
                                        //    {
                                        //        StartTimeText += "이후";
                                        //    }

                                        //}
                                        //else if (!String.IsNullOrEmpty(StartTimeType4.Text))
                                        //{
                                        //    StartTimeText = $"{StartTimeType4.Text}일";
                                        //    if (StartTimeHour1.SelectedIndex > 0)
                                        //        StartTimeText += $" {StartTimeHour1.Text}";
                                        //    if (StartTimeHour2.SelectedIndex > 0)
                                        //        StartTimeText += $" {StartTimeHour2.Text}";
                                        //    if (StartTimeHalf.Checked)
                                        //        StartTimeText += " 30분";
                                        //    if (StartTimeETC.Checked)
                                        //    {
                                        //        StartTimeText += "이후";
                                        //    }

                                        //}
                                        
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            _Sort = false;
            _Search();
            _NewOrder();
            SetRowBackgroundColor();

            if (ChkRequestDate.Checked)
            {
                dtpRequestDate.Value = _CreateTime;
            }
        }

        private void StopDateHelper_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void CarSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] SmallCarTypeValues = new int[]{
                2, 3,74
            };
            int[] MediumCarTypeValues = new int[]{
                0,1,4,5,6,8,9,10,14,16,18,20,21,60,61,62,22,63,64,65,66,67,27,68,69,33,34,70,71,72,73
            };
            int[] FiveCarTypeValues = new int[]{
                0,1,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,72,73
            };
            int[] LargeCarTypeValues = new int[]{
                0,1,4,5,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,72,73
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
            else if (CarSizeValue > 7)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && LargeCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 0;
            }
            //SharedItemSize.DisplayMember = "Name";
            //SharedItemSize.ValueMember = "Value";
            double Number = ((CarSize.SelectedItem as StaticOptionsRow)?.Number) ?? 0;
           // SharedItemSize.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "SharedItemSize" && c.Number <= Number).OrderBy(c => c.Seq).ToList());
        }

        private void UpdateOrder_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current == null)
            {
                return;
            }

            var Current = DataListSource.Current as Order;
            if (Current.ClientId != LocalUser.Instance.LogInInformation.ClientId)
            {
                MessageBox.Show("타 회사의 화물은 수정 할 수 있습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (String.IsNullOrEmpty(Customer.Text) || Customer.Tag == null)
            {
                MessageBox.Show("화주정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Customer.Focus();
                return;
            }


            if (String.IsNullOrEmpty(StartState.Text) || String.IsNullOrEmpty(StopState.Text)
     || String.IsNullOrEmpty(Price1.Text) || String.IsNullOrEmpty(Item.Text))
            {
                MessageBox.Show("상하차지 운송료/추가정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (PayLocation.SelectedIndex == 0)
            {
                MessageBox.Show("지불방식은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                PayLocation.Focus();
                return;
            }

            if (String.IsNullOrEmpty(CarSize.Text))
            {
                MessageBox.Show("톤수는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                CarSize.Focus();
                return;

            }

            if (String.IsNullOrEmpty(CarType.Text))
            {
                MessageBox.Show("차종은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                CarType.Focus();
                return;

            }
            if (Current.DriverId > 0)
            {
                if (PayLocation.SelectedIndex == 2 || PayLocation.SelectedIndex == 3 || PayLocation.SelectedIndex == 1)
                {
                    if (cmb_ReferralId.SelectedIndex == 0)
                    {
                        //MessageBox.Show("운송비가 선/착불,선/착불+인수증일 경우 위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        MessageBox.Show("위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cmb_ReferralId.Focus();
                        return;
                    }
                }
            }

            

            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                Order uOrder = ShareOrderDataSet.Orders.Find(Current.OrderId);

               
                
                if(uOrder.OrderStatus != 3)
                {
                    if (uOrder.OrderStatus == 0 || Current.OrderStatus == 1)
                        uOrder.OrderStatus = 1;
                }
                else
                {
                    uOrder.OrderStatus = 3;
                }

                uOrder.CreateTime = Current.CreateTime;
                uOrder.ClientId = Current.ClientId;
                uOrder.StartState = StartState.Text;
                uOrder.StartCity = StartCity.Text;
                uOrder.StartStreet = StartStreet.Text;
                uOrder.StartDetail = Start.Text;
                uOrder.StopState = StopState.Text;
                uOrder.StopCity = StopCity.Text;
                uOrder.StopStreet = StopStreet.Text;
                uOrder.StopDetail = Stop.Text;
                uOrder.CarSize = (int)CarSize.SelectedValue;
                uOrder.CarType = (int)CarType.SelectedValue;
                uOrder.StopDateHelper = (int)StopDateHelper.SelectedValue;
                uOrder.CarCount = int.Parse(CarCount.Text);
                uOrder.PayLocation = (int)PayLocation.SelectedValue;
              
                uOrder.Item = Item.Text;
                uOrder.Remark = Remark.Text;
                uOrder.ItemSize = ItemSize.Text;
                uOrder.ItemSizeInclude = ItemSizeInclude.Checked;
               
                uOrder.SharedItemLength = 0;
                uOrder.SharedItemSize = 0;
               
                uOrder.Customer = Customer.Text;
                uOrder.OrderPhoneNo = OrderPhoneNo.Text;
          
                uOrder.StartDate = DateTime.Now.Date;
                uOrder.StartTime = DateTime.Now.Date;
                uOrder.StopTime = DateTime.Now.Date;
                //  uOrder.AccountMemo = AccountMemo.Text;
                uOrder.TradePrice = 0;
                uOrder.SalesPrice = 0;
                uOrder.AlterPrice = 0;
                uOrder.StartPrice = 0;
                uOrder.StopPrice = 0;
                uOrder.DriverPrice = 0;
                uOrder.Price = 0;
                uOrder.ClientPrice = 0;
                if (PayLocation.SelectedIndex == 2)
                {
                    uOrder.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                    uOrder.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                    uOrder.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));
                    uOrder.Price = uOrder.StartPrice.Value + uOrder.StopPrice.Value;
                    uOrder.ClientPrice = uOrder.StartPrice.Value + uOrder.StopPrice.Value - uOrder.DriverPrice;
                }
                else if (PayLocation.SelectedIndex == 1)
                {
                    uOrder.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                    uOrder.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                    uOrder.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));
                    uOrder.Price = uOrder.TradePrice.Value;
                    uOrder.ClientPrice = uOrder.SalesPrice;
                }
                else if (PayLocation.SelectedIndex == 3)
                {
                    uOrder.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                    uOrder.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                    uOrder.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));

                    uOrder.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                    uOrder.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                    uOrder.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));

                    uOrder.Price = uOrder.TradePrice.Value;//uOrder.StartPrice.Value + uOrder.StopPrice.Value + uOrder.TradePrice.Value;
                    uOrder.ClientPrice = uOrder.SalesPrice; //uOrder.StartPrice.Value + uOrder.StopPrice.Value - uOrder.DriverPrice + uOrder.SalesPrice;
                }
              
                if (Customer.Tag != null && String.IsNullOrEmpty(Customer.Text) && uOrder.OrderStatus == 1)
                {
                    uOrder.CustomerId = (int)Customer.Tag;
                    if (CustomerModelList.Any(c => c.CustomerId == uOrder.CustomerId))
                        uOrder.PointMethod = CustomerModelList.Find(c => c.CustomerId == uOrder.CustomerId).PointMethod;
                }
                if (String.IsNullOrEmpty(Customer.Text))
                {
                    uOrder.CustomerId = null;
                    uOrder.OrderPhoneNo = "";
                    uOrder.Customer = "";

                }
                if (String.IsNullOrEmpty(uOrder.OrderPhoneNo))
                   
                    uOrder.OrderPhoneNo = "";
                if (DriverPoint.Checked)
                    uOrder.DriverPoint = 0;
                else
                    uOrder.DriverPoint = null;
               
                if (ChkRequestDate.Checked)
                {
                    if (Current.CreateTime.Date != dtpRequestDate.Value.Date)
                    {
                        uOrder.CreateTime = Convert.ToDateTime(dtpRequestDate.Value.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                        if (uOrder.OrderStatus == 3)
                        {
                            uOrder.AcceptTime = Convert.ToDateTime(dtpRequestDate.Value.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                        }
                    }
                    else
                    {
                        uOrder.CreateTime = DateTime.Now;
                        if (uOrder.OrderStatus == 3)
                        {
                            uOrder.AcceptTime = DateTime.Now;
                        }
                    }
                }
               

                uOrder.StartName = StartName.Text;
                uOrder.StopName = StopName.Text;

                uOrder.RequestMemo = RequestMemo.Text;
                if (chkMyCarOrder.Checked)
                {
                    uOrder.MyCarOrder = true;

                }
                else
                {
                    uOrder.MyCarOrder = false;
                }
               
                uOrder.ReferralId = (int)cmb_ReferralId.SelectedValue;
                uOrder.DriverGrade = (int)DriverGrade.SelectedValue;
              


                uOrder.CustomerManager = CustomerManager.Text;
                 
                //uOrder.OrdersLoginId = LocalUser.Instance.LogInInformation.LoginId;

                uOrder.UnitItem = UnitItem.Text;
                uOrder.UnitType = (int)UnitType.SelectedValue;

                //uOrder.ReferralId = (int)cmb_ReferralId.SelectedValue;


                ShareOrderDataSet.Entry(uOrder).State = System.Data.Entity.EntityState.Modified;
                ShareOrderDataSet.SaveChanges();
                ShareOrderDataSet.OrderUpdateLogs.Add(new OrderUpdateLog
                {
                    UpdateTime = DateTime.Now,
                    LoginId = LocalUser.Instance.LogInInformation.LoginId,
                    CarSize = CarSize.Text,
                    CarType = CarType.Text,
                    PayLocation = PayLocation.Text,
                    Amount = Price2.Text,
                    Remark = Remark.Text,
                    ClientId = LocalUser.Instance.LogInInformation.ClientId,
                    OrderId = uOrder.OrderId,
                    Customer = Customer.Text
                });
                ShareOrderDataSet.SaveChanges();
            }
            _Sort = false;
            _Search();
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
                else if (e.ColumnIndex == ColumnStart.Index)
                {
                    // e.Value = String.Join(" ", new string[] { Current.StartState, Current.StartCity, Current.StartStreet, Current.StartDetail }.Where(c => !String.IsNullOrEmpty(c)));
                    e.Value = Current.StartName;
                }
                else if (e.ColumnIndex == ColumnStop.Index)
                {
                    //e.Value = String.Join(" ", new string[] { Current.StopState, Current.StopCity, Current.StopStreet, Current.StopDetail }.Where(c => !String.IsNullOrEmpty(c)));
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
                            if (Current.MyCarOrder == true)
                            {
                                if (Current.CreateTime.Date > DateTime.Now.Date)
                                {
                                    e.Value = "공유/예약";
                                }
                                else
                                {

                                    e.Value = "공유";
                                }
                            }
                            else
                            {
                                if (Current.CreateTime.Date > DateTime.Now.Date)
                                {
                                    e.Value = "예약";
                                }
                                else
                                {
                                    

                                    e.Value = "접수";
                                }
                            }
                            break;
                        case 2:
                            e.Value = "대기";
                            break;
                        case 3:
                            if (Current.MyCarOrder == true)
                            {
                                if (Current.OrderClientId == 0)
                                {
                                    e.Value = "완료";
                                }
                                else
                                {
                                    if (Current.OrderClientId == LocalUser.Instance.LogInInformation.ClientId)
                                    {
                                        e.Value = "완료";
                                    }
                                    else if (Current.OrderClientId != LocalUser.Instance.LogInInformation.ClientId)
                                    {
                                        e.Value = "공유완료";
                                    }
                                }
                            }
                            else
                            {
                                e.Value = "완료";
                            }

                            break;
                    }
                }

                else if (e.ColumnIndex == ColumnImageA.Index)
                {
                    var Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableButtonCell;
                    if (Current.ImageA == null)
                    {
                        Cell.Enabled = false;
                        Cell.ReadOnly = true;
                    }
                    else
                    {
                        Cell.Enabled = true;
                        Cell.ReadOnly = false;
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
                else if (e.ColumnIndex == ColumnTradeDate.Index)
                {
                    try
                    {
                        if (Current.TradeModel != null)
                        {
                            e.Value = Current.TradeModel.RequestDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                    catch
                    {
                        e.Value = "";

                    }
                }
                else if (e.ColumnIndex == ColumnClientId.Index)
                {
                 

                    if (Current.MyCarOrder == true && Current.ClientId != LocalUser.Instance.LogInInformation.ClientId)
                    {
                        var _ClientsList = _ClientTable.Where(c => c.ClientId == Current.ClientId).First();
                        e.Value = _ClientsList.Name;
                    }
                    else if(Current.MyCarOrder == true && Current.ClientId == LocalUser.Instance.LogInInformation.ClientId)
                    {
                        e.Value = Current.Customer;
                    }
                    else
                    {
                        e.Value = Current.Customer;
                    }

                }
                else if(e.ColumnIndex == ColumnSalesPrice.Index)
                {

                    if (Current.MyCarOrder == true && Current.ClientId != LocalUser.Instance.LogInInformation.ClientId)
                    {
                      
                        e.Value = (Current.TradePrice ?? 0).ToString("N0");
                    }
                    else if (Current.MyCarOrder == true && Current.ClientId == LocalUser.Instance.LogInInformation.ClientId)
                    {
                        e.Value = e.Value;
                    }
                    else
                    {
                        e.Value = e.Value;


                    }
                   

                }
                else if (e.ColumnIndex == ColumnTradePrice.Index)
                {

                    if (Current.MyCarOrder == true && Current.ClientId != LocalUser.Instance.LogInInformation.ClientId)
                    {

                        e.Value = "0";
                    }
                    else if (Current.MyCarOrder == true && Current.ClientId == LocalUser.Instance.LogInInformation.ClientId)
                    {
                        e.Value = e.Value;
                    }
                    else
                    {
                        e.Value = e.Value;


                    }


                }
                else if (e.ColumnIndex == ColumnDriverBizNo.Index)
                {
                    if (Current.DriverId > 0)
                    {
                        var drivers = _DriverTable.Where(c => c.DriverId == Current.DriverId).ToArray();

                        if (drivers.Any())
                        {
                            e.Value = drivers.First().BizNo;
                        }
                    }



                }
                else if (e.ColumnIndex == ColumnPayDate.Index)
                {
                    if (Current.TradeModel != null && Current.TradeModel.PayDate != null && Current.TradeModel.PayState == 1)
                    {
                        e.Value = Current.TradeModel.PayDate.Value.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        e.Value = "";
                    }
                }
                //else if (e.ColumnIndex == btnSms.Index)
                //{



                //}

                else if (e.ColumnIndex == lMsCntCol.Index)
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.Value = Current.LMSCnt.ToString() + "/" + Current.LMSCustomerCnt.ToString();

                }
                else if(e.ColumnIndex == Column_DriverCarNo.Index)
                {
                    //DriverRepository mDriverRepository = new DriverRepository();
                    //if (Current.OrderStatus == 3 && Current.DriverId != 0)
                    //{
                    //    var Query = mDriverRepository.GetDriver(Current.DriverCarNo);


                    //    if (String.IsNullOrEmpty(Query.BizNo) || Query.BizNo.Substring(0, 3) == "000" || Query.BizNo.Substring(0, 3) == "999" || String.IsNullOrEmpty(Query.Name) || String.IsNullOrEmpty(Query.CEO) || String.IsNullOrEmpty(Query.Uptae) || String.IsNullOrEmpty(Query.Upjong) || Query.Name == "." || Query.CEO == "." || Query.Upjong == "." || Query.Uptae == "." || String.IsNullOrEmpty(Query.Addresstate) || String.IsNullOrEmpty(Query.AddressCity) || String.IsNullOrEmpty(Query.AddressDetail) || String.IsNullOrEmpty(Query.PayBankName) || String.IsNullOrEmpty(Query.PayAccountNo) || String.IsNullOrEmpty(Query.PayInputName))
                    //    {
                    //        e.CellStyle.ForeColor = Color.Red;
                    //    }
                    //}

                }

                else if (e.ColumnIndex == SalesRequestDate.Index)
                {
                    string R = "";
                    if (Current.SalesManageId != null)
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                    @"SELECT  IssueDate FROM    SalesManage
                                    WHERE SalesId = @SalesManageId AND ISSUE = 1 ";
                                _Command.Parameters.AddWithValue("@SalesManageId", Current.SalesManageId);
                                object O = _Command.ExecuteScalar();
                                if (O != null)
                                    if (O != null)
                                    {
                                        R = Convert.ToDateTime(O).ToString("d").Replace("-", "/");
                                    }
                                    else
                                    {
                                        R = "";
                                    }
                            }
                            _Connection.Close();
                        }

                        e.Value = R;
                    }
                    else
                    {
                        e.Value = "";

                    }


                }

                else if (e.ColumnIndex == SalesPayDate.Index)
                {
                    string R = "";
                    if (Current.SalesManageId != null)
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                    @"SELECT  PayDate FROM    SalesManage
                                    WHERE SalesId = @SalesManageId AND Paystate = 1 ";
                                _Command.Parameters.AddWithValue("@SalesManageId", Current.SalesManageId);
                                object O = _Command.ExecuteScalar();
                                if (O != null)
                                {
                                    R = Convert.ToDateTime(O).ToString("d").Replace("-", "/");
                                }
                                else
                                {
                                    R = "";
                                }
                            }
                            _Connection.Close();
                        }

                        e.Value = R;
                    }
                    else
                    {
                        e.Value = "";

                    }

                }

                else if(e.ColumnIndex == ColumnOrdersLoginId.Index)
                {
                    string R = "";
                 
                    if (!String.IsNullOrEmpty(Current.OrdersLoginId))
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                    @"SELECT  CEO FROM  Clients
                                    WHERE LoginId = @LoginId  ";
                                _Command.Parameters.AddWithValue("@LoginId", Current.OrdersLoginId);
                                object O = _Command.ExecuteScalar();
                               
                                    if (O != null)
                                    {
                                        R = O.ToString();
                                    }
                                    else
                                    {
                                        _Command.CommandText =
                                          @"SELECT  Name FROM  ClientUsers
                                            WHERE LoginId = @LoginId  ";
                                       

                                        object U = _Command.ExecuteScalar();

                                        if (U != null)
                                        {
                                            R = U.ToString();
                                        }
                                    }
                            }
                            _Connection.Close();
                        }

                        e.Value = R;
                    }
                    else
                    {
                        e.Value = "";

                    }
                }
                else if (e.ColumnIndex == ColumnOrdersAcceptId.Index)
                {
                    string R = "";

                    if (!String.IsNullOrEmpty(Current.OrdersAcceptId))
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                    @"SELECT  CEO FROM  Clients
                                    WHERE LoginId = @LoginId  ";
                                _Command.Parameters.AddWithValue("@LoginId", Current.OrdersAcceptId);
                                object O = _Command.ExecuteScalar();
                               
                                    if (O != null)
                                    {
                                        R = O.ToString();
                                    }
                                    else
                                    {
                                        _Command.CommandText =
                                          @"SELECT  Name FROM  ClientUsers
                                            WHERE LoginId = @LoginId  ";
                                       

                                        object U = _Command.ExecuteScalar();

                                        if (U != null)
                                        {
                                            R = U.ToString();
                                        }
                                    }
                            }
                            _Connection.Close();
                        }

                        e.Value = R;
                    }
                    else
                    {
                        e.Value = "";

                    }
                }

                else if (e.ColumnIndex == ColumnDriverSangHo.Index)
                {
                    if (Current.DriverId > 0)
                    {
                        var drivers = _DriverTable.Where(c => c.DriverId == Current.DriverId).ToArray();

                        if (drivers.Any())
                        {
                            e.Value = drivers.First().Name;
                        }
                    }
                }
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {

            //DateFilterBegin.ValueChanged += (c, d) =>
            //{
            //    DateTime eDate = DateFilterEnd.Value.Date;
            //    DateTime sDate = DateFilterBegin.Value.AddMonths(3).Date;


            //    if (eDate >= sDate)
            //    {
            //        MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return;
            //    }
            //    _Sort = false;
            //    _Search();
            //};
            //DateFilterEnd.ValueChanged += (c, d) =>
            //{
            //    if (DateFilterEnd.Value.AddMonths(-3).Date >= DateFilterBegin.Value.Date)
            //    {
            //        MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return;
            //    }
            //    _Sort = false;
            //    _Search();


            //};
            //_Sort = false;
            //_Search();

            DateTime eDate = DateFilterEnd.Value.Date;
            DateTime sDate = DateFilterBegin.Value.AddMonths(3).Date;


            //if (eDate >= sDate)
            //{
            //    MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //} 


            if (DateFilterEnd.Value.AddMonths(-3).Date >= DateFilterBegin.Value.Date || eDate >= sDate)
            { 
                MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            _Sort = false;
            _Search();
        }

        private void CancelOrder_Click(object sender, EventArgs e)
        {
           
            if (DataListSource.Current != null)
            {
                GridIndex = DataListSource.Position;
                _Sort = false;
                _Search();
                grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];
                DataListSource_CurrentChanged(null, null);
                var Current = DataListSource.Current as Order;

                if (Current.OrderStatus != 3)
                    return;
                if (Current.ClientId != LocalUser.Instance.LogInInformation.ClientId && Current.OrderClientId != LocalUser.Instance.LogInInformation.ClientId)
                {
                    MessageBox.Show("타 회사의 화물은 수정 할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (Current.OrderClientId != 0)
                {
                    if (Current.OrderClientId != LocalUser.Instance.LogInInformation.ClientId)
                    {
                        MessageBox.Show("타운송사에서 배차한 공유 화물은 배차 취소 할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                if (String.IsNullOrEmpty(StartState.Text) || String.IsNullOrEmpty(StopState.Text)
  || String.IsNullOrEmpty(Price1.Text) || String.IsNullOrEmpty(Item.Text))
                {
                    MessageBox.Show("배차 취소할 정보를 선택하십시오", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }


                //if (Current.TradeId != null || Current.SalesManageId != null)
                if (Current.TradeId != null)
                {
                    //if (Current.TradeId != null && Current.SalesManageId != null)
                    if (Current.TradeId != null )
                    {
                        MessageBox.Show("세금계산서가 발행된 배차는 취소할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        string R = "";
                        #region 화주세금계산서
                        //if (Current.SalesManageId != null)
                        //{
                        //    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        //    {
                        //        _Connection.Open();
                        //        using (SqlCommand _Command = _Connection.CreateCommand())
                        //        {
                        //            _Command.CommandText =
                        //                @"SELECT  IssueDate FROM    SalesManage
                        //            WHERE SalesId = @SalesManageId AND ISSUE = 1 ";
                        //            _Command.Parameters.AddWithValue("@SalesManageId", Current.SalesManageId);
                        //            object O = _Command.ExecuteScalar();
                        //            if (O != null)
                        //                if (O != null)
                        //                {
                        //                    R = Convert.ToDateTime(O).ToString("d").Replace("-", "/");
                        //                }
                        //                else
                        //                {
                        //                    R = "";
                        //                }
                        //        }
                        //        _Connection.Close();
                        //    }

                        //    if (R == "")
                        //    {
                        //        MessageBox.Show("매출전표가 작성된 배차는 취소할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("세금계산서가 발행된 배차는 취소할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //    }
                        //}
                        #endregion

                        if (Current.TradeId != null)
                        {

                            MessageBox.Show("세금계산서가 발행된 배차는 취소할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    return;
                }
                Current.DriverModel = null;
                Current.DriverId = null;
                Current.OrderStatus = 1;
                Current.AcceptTime = null;
                Current.Driver = String.Empty;
                Current.DriverCarModel = String.Empty;
                Current.DriverCarNo = String.Empty;
                Current.DriverPhoneNo = String.Empty;
                Current.DriverPoint = null;
                Current.Wgubun = "";
                Current.OrdersAcceptId = "";

                bool _MyOrder = false;
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    if (Current.FOrderId != 0)
                    {
                        var Query = ShareOrderDataSet.Orders.Where(c => c.OrderId == Current.FOrderId).ToArray();
                        if (Query.Any())
                        {
                           
                                if (Query.First().ClientId == LocalUser.Instance.LogInInformation.ClientId)
                                {
                                    _MyOrder = true;
                                }
                                else
                                {
                                    _MyOrder = false;
                                }
                           
                        }
                    }
                    else
                    {
                        _MyOrder = true;
                    }

                }
                //타사접수 / 배차 자차
                if (Current.MyCarOrder == true && !_MyOrder)
                {
                    //접수건은 원래대로
                    Data.Connection((_Connection) =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "UPDATE Orders SET DriverId = NULL,OrderStatus = 1,AcceptTime = NULL,Driver = '',DriverCarModel = '',DriverCarNo ='',DriverPhoneNo = '',DriverPoint = NULL, Wgubun = '',OrderClientId = 0,ForderId =0  WHERE OrderId = @FOrderId";
                            _Command.Parameters.AddWithValue("@FOrderId", Current.FOrderId);
                            _Command.ExecuteNonQuery();
                        }
                        _Connection.Close();

                    });

                    
                    //자차배차건은삭제
                    Data.Connection((_Connection) =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "DELETE Orders WHERE OrderId = @OrderId";
                            _Command.Parameters.AddWithValue("@OrderId", Current.OrderId);
                            _Command.ExecuteNonQuery();
                        }
                        _Connection.Close();

                    });
                    _Sort = false;
                    _Search();

                }

                if (Current.ClientId == LocalUser.Instance.LogInInformation.ClientId && _MyOrder)
                {
                    Current.FOrderId = 0;
                    Current.OrderClientId = 0;
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
                        ShareOrderDataSet.SaveChanges();
                    }
                }
                DataListSource.ResetBindings(false);
            }
            SetRowBackgroundColor();
        }
        private void btnEnable()
        {

            UpdateOrder.Enabled = true;
            CopyOrder.Enabled = true;
            DeleteOrder.Enabled = true;
            CancelOrder.Enabled = true;
            btnDel.Enabled = true;


        }

        private void btnDisable()
        {

            UpdateOrder.Enabled = false;
            CopyOrder.Enabled = false;
            DeleteOrder.Enabled = false;
            CancelOrder.Enabled = false;
            btnDel.Enabled = false;


        }
        private void DataListSource_CurrentChanged(object sender, EventArgs e)
        {
            MethodProcess = true;
            _NewOrder();
            if (DataListSource.Current != null)
            {
                btnEnable();

                var Current = DataListSource.Current as Order;
                ModelToView(Current);
                if (Current.Agubun == 4)
                {
                    Customer.ReadOnly = true;
                }
                else
                {
                    Customer.ReadOnly = false;
                }

               

                //차주세금계산서 발급받은경우
                if (Current.TradeId != null)
                {
                  
                    //지불금액
                    if (Current.OrderStatus == 3)
                    {
                        Price1.Enabled = false;
                    }
                    else
                    {
                        Price1.Enabled = true;
                    }
                }

                if(Current.SalesManageId != null)
                {
                   
                    //지불금액
                    if (Current.OrderStatus == 3)
                    {
                        Price2.Enabled = false;
                    }
                    else
                    {
                        Price2.Enabled = true;
                    }
                }

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
            string R = "";
            string RA = "";
            if (!String.IsNullOrEmpty(Current.OrdersLoginId))
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                            @"SELECT  CEO FROM  Clients
                                    WHERE LoginId = @LoginId  ";
                        _Command.Parameters.AddWithValue("@LoginId", Current.OrdersLoginId);
                        object O = _Command.ExecuteScalar();

                        if (O != null)
                        {
                            R = O.ToString();
                        }
                        else
                        {
                            _Command.CommandText =
                              @"SELECT  Name FROM  ClientUsers
                                            WHERE LoginId = @LoginId  ";


                            object U = _Command.ExecuteScalar();

                            if (U != null)
                            {
                                R = U.ToString();
                            }
                        }
                    }
                    _Connection.Close();
                }

               
            }
            if (!String.IsNullOrEmpty(Current.OrdersAcceptId))
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                            @"SELECT  CEO FROM  Clients
                                    WHERE LoginId = @LoginId  ";
                        _Command.Parameters.AddWithValue("@LoginId", Current.OrdersAcceptId);
                        object O = _Command.ExecuteScalar();

                        if (O != null)
                        {
                            RA = O.ToString();
                        }
                        else
                        {
                            _Command.CommandText =
                              @"SELECT  Name FROM  ClientUsers
                                            WHERE LoginId = @LoginId  ";
                           // _Command.Parameters.AddWithValue("@LoginId", Current.OrdersAcceptId);

                            object U = _Command.ExecuteScalar();

                            if (U != null)
                            {
                                RA = U.ToString();
                            }
                        }
                    }
                    _Connection.Close();
                }

               
            }

            lblAOId.Text = R + " / " + RA;
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
            Price1.Text = (Current.TradePrice ?? 0).ToString("N0");
            Price2.Text = (Current.SalesPrice ?? 0).ToString("N0");
            Price3.Text = (Current.AlterPrice ?? 0).ToString("N0");
            Price4.Text = (Current.StartPrice ?? 0).ToString("N0");
            Price5.Text = (Current.StopPrice ?? 0).ToString("N0");
            Price6.Text = (Current.DriverPrice ?? 0).ToString("N0");

            Item.Text = Current.Item;
            Remark.Text = Current.Remark;

            //if (Current.IsShared)
            //{
            //    Shared.Checked = true;
            //    SharedItemLength.SelectedValue = Current.SharedItemLength ?? 0;
            //    SharedItemSize.SelectedValue = Current.SharedItemSize ?? 0;
            //}
            //else
            //{
            //    NotShared.Checked = true;
            //    SharedItemLength.SelectedIndex = 0;
            //    SharedItemSize.SelectedIndex = 0;
            //}
            //Emergency.Checked = Current.Emergency == true;
            //Round.Checked = Current.Round == true;
            //Reservation.Checked = Current.Reservation == true;

            //ClearStarTime_Click(null, null);
            //if (Current.StartInfo == "지게차")
            //{
            //    StartInfo1.Checked = true;
            //}
            //else if (Current.StartInfo == "수작업")
            //{
            //    StartInfo2.Checked = true;
            //}
            //else if (Current.StartInfo == "호이스트")
            //{
            //    StartInfo3.Checked = true;
            //}
            //else if (Current.StartInfo == "크레인")
            //{
            //    StartInfo4.Checked = true;
            //}
            //else if (Current.StartInfo == "컨베이어")
            //{
            //    StartInfo5.Checked = true;
            //}
            //if (Current.StartTimeType == 1)
            //{
            //    StartTimeType1.Checked = true;
            //}
            //else if (Current.StartTimeType == 2)
            //{
            //    StartTimeType2.Checked = true;
            //    if (Current.StartTimeHour > 0)
            //    {
            //        if (Current.StartTimeHour > 12)
            //        {
            //            StartTimeHour1.SelectedIndex = 2;
            //            StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value - 12;
            //        }
            //        else
            //        {
            //            StartTimeHour1.SelectedIndex = 1;
            //            StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value;
            //        }
            //        StartTimeHalf.Checked = Current.StartTimeHalf == true;
            //        StartTimeETC.Checked = Current.StartTimeETC == true;
            //    }
            //}
            //else if (Current.StartTimeType == 3)
            //{
            //    StartTimeType3.Checked = true;
            //    if (Current.StartTimeHour > 0)
            //    {
            //        if (Current.StartTimeHour > 12)
            //        {
            //            StartTimeHour1.SelectedIndex = 2;
            //            StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value - 12;
            //        }
            //        else
            //        {
            //            StartTimeHour1.SelectedIndex = 1;
            //            StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value;
            //        }
            //        StartTimeHalf.Checked = Current.StartTimeHalf == true;
            //        StartTimeETC.Checked = Current.StartTimeETC == true;
            //    }
            //}
            //else if (Current.StartTimeType > 1000)
            //{
            //    StartTimeType4.Text = (Current.StartTimeType - 1000).ToString();
            //    if (Current.StartTimeHour > 0)
            //    {
            //        if (Current.StartTimeHour > 12)
            //        {
            //            StartTimeHour1.SelectedIndex = 2;
            //            StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value - 12;
            //        }
            //        else
            //        {
            //            StartTimeHour1.SelectedIndex = 1;
            //            StartTimeHour2.SelectedIndex = Current.StartTimeHour.Value;
            //        }
            //        StartTimeHalf.Checked = Current.StartTimeHalf == true;
            //        StartTimeETC.Checked = Current.StartTimeETC == true;
            //    }
            //}

            ClearStopTime_Click(null, null);
            //if (Current.StopInfo == "지게차")
            //{
            //    StopInfo1.Checked = true;
            //}
            //else if (Current.StopInfo == "수작업")
            //{
            //    StopInfo2.Checked = true;
            //}
            //else if (Current.StopInfo == "호이스트")
            //{
            //    StopInfo3.Checked = true;
            //}
            //else if (Current.StopInfo == "크레인")
            //{
            //    StopInfo4.Checked = true;
            //}
            //else if (Current.StopInfo == "컨베이어")
            //{
            //    StopInfo5.Checked = true;
            //}
            //if (Current.StopTimeType == 1)
            //{
            //    StopTimeType1.Checked = true;
            //    if (Current.StopTimeHour > 0)
            //    {
            //        if (Current.StopTimeHour > 12)
            //        {
            //            StopTimeHour1.SelectedIndex = 2;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
            //        }
            //        else
            //        {
            //            StopTimeHour1.SelectedIndex = 1;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
            //        }
            //        StopTimeHalf.Checked = Current.StopTimeHalf == true;
            //    }
            //}
            //else if (Current.StopTimeType == 2)
            //{
            //    StopTimeType2.Checked = true;
            //    if (Current.StopTimeHour > 0)
            //    {
            //        if (Current.StopTimeHour > 12)
            //        {
            //            StopTimeHour1.SelectedIndex = 2;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
            //        }
            //        else
            //        {
            //            StopTimeHour1.SelectedIndex = 1;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
            //        }
            //        StopTimeHalf.Checked = Current.StopTimeHalf == true;
            //    }
            //}
            //else if (Current.StopTimeType == 3)
            //{
            //    StopTimeType3.Checked = true;
            //    if (Current.StopTimeHour > 0)
            //    {
            //        if (Current.StopTimeHour > 12)
            //        {
            //            StopTimeHour1.SelectedIndex = 2;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
            //        }
            //        else
            //        {
            //            StopTimeHour1.SelectedIndex = 1;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
            //        }
            //        StopTimeHalf.Checked = Current.StopTimeHalf == true;
            //    }
            //}
            //else if (Current.StopTimeType == 4)
            //{
            //    StopTimeType4.Checked = true;
            //}
            //else if (Current.StopTimeType > 1000)
            //{

            //    StopTimeType5.Text = (Current.StopTimeType - 1000).ToString();
            //    if (Current.StopTimeHour > 0)
            //    {
            //        if (Current.StopTimeHour > 12)
            //        {
            //            StopTimeHour1.SelectedIndex = 2;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value - 12;
            //        }
            //        else
            //        {
            //            StopTimeHour1.SelectedIndex = 1;
            //            StopTimeHour2.SelectedIndex = Current.StopTimeHour.Value;
            //        }
            //        StopTimeHalf.Checked = Current.StopTimeHalf == true;
            //    }
            //}



            OrderPhoneNo.Text = Current.OrderPhoneNo;
            //StopPhoneNo.Text = Current.StopPhoneNo;
            //StartPhoneNo.Text = Current.StartPhoneNo;
            ItemSizeInclude.Checked = Current.ItemSizeInclude == true;
            ItemSize.Text = Current.ItemSize;

            DriverName.Text = Current.Driver;
            DriverPhoneNo.Text = Current.DriverPhoneNo;
            DriverCarNo.Text = Current.DriverCarNo;

            if (Current.OrderStatus == 1 ||  Current.OrderStatus == 2)
            {
                Driver.Enabled = true;
                // Customer.Enabled = true;
            }
            else
            {
                Driver.Enabled = false;
                // Customer.Enabled = false;
            }
            HasTrade.Checked = Current.TradeId != null;
            HasSales.Checked = Current.SalesManageId != null;
            DriverPoint.Checked = Current.DriverPoint != null;

            StartName.Text = Current.StartName;
            StopName.Text = Current.StopName;
            //StartMemo.Text = Current.StartMemo;
            //StopMemo.Text = Current.StopMemo;

            RequestMemo.Text = Current.RequestMemo;
            //if (Current.ReferralId)
            DateTime Now = DateTime.Now.AddDays(1).Date;
            DateTime Tommorow = DateTime.Now.AddYears(-1).Date;

            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                string _Grade = "0";
                var _Q = ShareOrderDataSet.Orders.Where(c => c.DriverId == Current.DriverId && c.CreateTime >= Tommorow && c.CreateTime < Now && (c.DriverGrade != 0 && c.DriverGrade != null));
                if(_Q.Any())
                {
                    _Grade = _Q.Average(c => c.DriverGrade ?? 0).ToString("N0");
                }



                lblGrade.Text = "★ : " + _Grade;

            }





          
            if (Current.DriverGrade == null)
            {
                DriverGrade.SelectedValue = 0;

            }
            else
            {
                DriverGrade.SelectedValue = Current.DriverGrade;
            }


            SetCustomerList();
            var Query = CustomerModelList.Where(c => c.CustomerId == Current.ReferralId).ToArray();
            if (Query.Any())
            {
                if (Query.First().BizGubun == 3)
                {
                    //rdoJusun.Checked = true;
                    //txtJusun.Tag = Current.ReferralId;
                    //txtJusun.Text = Query.First().SangHo;
                    //txtJusun.Visible = true;
                    //cmb_ReferralId.Visible = false;
                }
                else
                {
                   // rdoWe.Checked = true;
                    cmb_ReferralId.Visible = true;
                    cmb_ReferralId.SelectedValue = Current.ReferralId;

                   // txtJusun.Visible = false;
                }

            }
            else
            {
                //rdoWe.Checked = true;
                cmb_ReferralId.Visible = true;


               // txtJusun.Visible = false;
            }
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
               
                    Price2.Text = (Current.TradePrice ?? 0).ToString("N0");
                    Price1.Text = "0";
               

                //배차 완료
                if (Current.OrderStatus == 3)
                {
                    //접수운송사ID //타사에서 배차
                    if (_MyCarClientId != 0)
                    {
                        var _ClientsList = _ClientTable.Where(c => c.ClientId == _MyCarClientId).First();
                        Customer.Text = _ClientsList.Name;
                        OrderPhoneNo.Text = _ClientsList.PhoneNo;
                       
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


          
           

            CustomerManager.Text = Current.CustomerManager;

            dtpRequestDate.Value = Current.CreateTime;

            UnitItem.Text = Current.UnitItem;
            UnitType.SelectedValue = Current.UnitType;
          
        }

        private void CopyOrder_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current != null)
            {
                var Current = DataListSource.Current as Order;
                if (Current.ClientId != LocalUser.Instance.LogInInformation.ClientId)
                {
                    MessageBox.Show("타 회사의 화물은 수정 할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (Current.OrderStatus == 0 || Current.OrderStatus ==2)
                {
                    Current.OrderStatus = 1;
                    UpdateOrder_Click(null, null);
                }
            }
            //DataList.CurrentCell = null;
            SetRowBackgroundColor();
        }

        private void AllSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
            SearchDriverTrade.SelectedIndex = 0;
            SearchCustomerTax.SelectedIndex = 0;
            SearchText.Clear();
            _Sort = false;
            _Search();
        }

        private void OrderSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
            SearchDriverTrade.SelectedIndex = 0;
            SearchCustomerTax.SelectedIndex = 0;
            SearchText.Clear();
            _Sort = false;
            _Search(1);
        }

        private void AcceptSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
            SearchDriverTrade.SelectedIndex = 0;
            SearchCustomerTax.SelectedIndex = 0;
            SearchText.Clear();
            _Sort = false;
            _Search(3);
        }

        private void DeleteOrder_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current == null)
                return;
            var Current = DataListSource.Current as Order;
            if (Current.ClientId != LocalUser.Instance.LogInInformation.ClientId)
            {
                MessageBox.Show("타 회사의 화물은 수정 할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (Current.OrderStatus != 1 && Current.OrderStatus != 3 && Current.TradeId != null)
            {
                MessageBox.Show("상태가 [접수]인 건만 화물 취소 할 수 있습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (Current.TradeId != null || Current.SalesManageId != null)
            {
                MessageBox.Show("세금계산서가 발행된 화물은 취소할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            if (String.IsNullOrEmpty(StartState.Text) || String.IsNullOrEmpty(StopState.Text)
   || String.IsNullOrEmpty(Price1.Text) || String.IsNullOrEmpty(Item.Text))
            {
                MessageBox.Show("화물 취소할 정보를 선택하십시오", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            if (Current.DriverId != null)
            {
                MessageBox.Show("배차 완료 된 건입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;


                //if (MessageBox.Show("배차 완료 된 건입니다. [배차 취소] 와 함께 [화물 취소]를 같이 진행하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                //    return;
                //Current.DriverModel = null;
                //Current.DriverId = null;
                //Current.AcceptTime = null;
                //Current.Driver = String.Empty;
                //Current.DriverCarModel = String.Empty;
                //Current.DriverCarNo = String.Empty;
                //Current.DriverPhoneNo = String.Empty;
                //Current.Wgubun = "";
            }
            Current.OrderStatus = 0;
            if(Current.MyCarOrder == true)
            {
                Current.MyCarOrder = false;
            }
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
                ShareOrderDataSet.SaveChanges();
            }
            DataListSource.ResetBindings(false);
            SetRowBackgroundColor();
        }

        private void btnOrderAfterAdd_Click(object sender, EventArgs e)
        {


            //FrmMN0303_CARGOFPIS_Add3 _Form = new FrmMN0303_CARGOFPIS_Add3
            //{
            //    Owner = this,
            //    StartPosition = FormStartPosition.CenterParent
            //};
            //if (_Form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    initReferralId();
            //}

          

            FrmMN0301Add _Form = new FrmMN0301Add
            {
                Owner = this,
                StartPosition = FormStartPosition.CenterParent
            };
            if (_Form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                initReferralId();
            }


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

            if(CurrentOrder == null)
            {
                return;
            }
            if (e.ColumnIndex == ColumnImageA.Index)
            {
                if (CurrentOrder.ImageA != null)
                {
                    List<String> Urls = new List<string>
                {
                    $"http://m.cardpay.kr/ImageFromAdmin/GetImage?ImageReferenceId={CurrentOrder.ImageA}"
                };
                    FormImagesDefault f = new FormImagesDefault(Urls.ToArray());
                    f.Show();


                }


            }
            else if (e.ColumnIndex == btnSms.Index)
            {
                var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

                if (_ClientPoint < 10)
                {

                    MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                    FrmMDI.LoadForm("FrmMN0204", "Point");
                    return;
                }

                if (CurrentOrder.OrderStatus != 3)
                    return;

                if (CurrentOrder.PayLocation == 5)
                    return;

                //var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

                //if(_ClientPoint < 15)
                //{
                //    MessageBox.Show("충전금이 부족합니다. 충전 후, 이용하시기 바랍니다.\r\n회원정보로 이동합니다.\r\n회원정보 이동후 포인트 내역 탭의 가상계좌번호를 확인후 충전 바랍니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                //    FrmMN0204_CARGOOWNERMANAGE _Form = new FrmMN0204_CARGOOWNERMANAGE();
                //    _Form.MdiParent = this.MdiParent;

                //    _Form.Show();
                   
                //    this.Close();

                //    return;
                //}


                string _PriceLms = "";
                String Message = "";
                //선/착불
                if (PayLocation.SelectedIndex == 2)
                {
                    //if (Price5.Text != "0")

                    //    _PriceLms = Price5.Text;
                    //else
                    //    _PriceLms = Price4.Text;
                    // _PriceLms = (CurrentOrder.StartPrice + CurrentOrder.StopPrice ?? 0).ToString("N0");
                    _PriceLms = " 선불 : " + (CurrentOrder.StartPrice ?? 0).ToString("N0") + "원 / " + " 착불 : " + (CurrentOrder.StopPrice ?? 0).ToString("N0") + "원";

                }
                else if (PayLocation.SelectedIndex == 1)
                {
                    _PriceLms = Price1.Text + "원";
                }
                else if (PayLocation.SelectedIndex == 3)
                {
                    _PriceLms = " 선불 : " + (CurrentOrder.StartPrice ?? 0).ToString("N0") + "원 / " + " 착불 : " + (CurrentOrder.StopPrice ?? 0).ToString("N0") + "원 "+" 청구 : "+Price1.Text + "원";
                }
                if (CurrentOrder.OrderStatus != 3 || String.IsNullOrEmpty(CurrentOrder.StartState) || String.IsNullOrEmpty(CurrentOrder.StopState))
                {
                    MessageBox.Show("상하차지가 입력되지 않았습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }


                #region 문자메세지
                //if (String.IsNullOrEmpty(StartMemo.Text))
                //{
                //    #region 문자메세지 2018-10-11
                //    Message = $"■ 상차지 - {CurrentOrder.StartTime.ToString("yyyy-MM-dd hh:mm").Replace("-", "/")}\r\n" +
                //           $"- {StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}\r\n" +

                //           $"- {StartPhoneNo.Text}\r\n" +

                //            $"- 화주({Customer.Text})\r\n" +
                //            $"\r\n" +


                //           $"■ 하차지\r\n" +
                //          $"- {StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}\r\n" +
                //           $"- {StopPhoneNo.Text}\r\n" +
                //           $"\r\n" +

                //           $"■ 화물정보\r\n" +
                //           $"- 운임 : {_PriceLms}\r\n" +
                //           $"- {PayLocation.Text}/{Remark.Text}\r\n" +
                //           $"\r\n" +



                //         $"■ 주선사 \r\n" +
                //         $"상호 :{LocalUser.Instance.LogInInformation.Client.Name} \r\n" +
                //         $"전화문의 : ☏{LocalUser.Instance.LogInInformation.Client.PhoneNo}\r\n" +
                //         $"세금계산서/인수증 관련사항은 위 전화로 연락바랍니다. \r\n" +
                //         $"\r\n" +



                //         $"■ 전자인수증(※사진촬영첨부)\r\n" +
                //         $"  전자세금계산서 어플 발행\r\n\r\n" +
                //         $"＂차세로＂비 회원(어플설치)\r\n" +
                //         $" 아래클릭\r\n" +
                //         $"https://play.google.com/store/apps/details?id=edubill.m.newcardpay.kr \r\n" +
                //         $"\r\n" +

                //          $"※. 기존회원은 바탕화면에서 ＂차세로＂ 어플을 실행하여 계산서를 발행 하십시오." +


                //         $"\r\n\r\n" +
                //         $"이용해 주셔서 감사 합니다. ";
                //    #endregion

                //}
                //else

                //{

                //    #region 문자메세지 2018-10-11
                //    Message = $"■ 상차지 - {CurrentOrder.StartTime.ToString("yyyy-MM-dd hh:mm").Replace("-", "/")}\r\n" +
                //           $"- {StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}\r\n" +
                //           $"- {StartMemo.Text} \r\n" +
                //           $"- {StartPhoneNo.Text}\r\n" +

                //            $"- 화주({Customer.Text})\r\n" +
                //            $"\r\n" +


                //           $"■ 하차지\r\n" +
                //          $"- {StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}\r\n" +
                //           $"- {StopPhoneNo.Text}\r\n" +
                //           $"\r\n" +

                //           $"■ 화물정보\r\n" +
                //           $"- 운임 : {_PriceLms}\r\n" +
                //           $"- {PayLocation.Text}/{Remark.Text}\r\n" +
                //           $"\r\n" +



                //          $"■ 주선사 \r\n" +
                //         $"상호 :{LocalUser.Instance.LogInInformation.Client.Name} \r\n" +
                //         $"전화문의 : ☏{LocalUser.Instance.LogInInformation.Client.PhoneNo}\r\n" +
                //         $"세금계산서/인수증 관련사항은 위 전화로 연락바랍니다. \r\n" +
                //         $"\r\n" +



                //         $"■ 전자인수증(※사진촬영첨부)\r\n" +
                //         $"  전자세금계산서 어플 발행\r\n\r\n" +
                //         $"＂차세로＂비 회원(어플설치)\r\n" +
                //         $" 아래클릭\r\n" +
                //         $"https://play.google.com/store/apps/details?id=edubill.m.newcardpay.kr \r\n" +
                //         $"\r\n" +

                //          $"※. 기존회원은 바탕화면에서 ＂차세로＂ 어플을 실행하여 계산서를 발행 하십시오." +


                //         $"\r\n\r\n" +
                //         $"이용해 주셔서 감사 합니다. ";
                //    #endregion


                //}
                #endregion

                #region 알림톡




                InitDriverTable();
                var LoginId = _DriverTable.Where(c => c.CarNo == DriverCarNo.Text);
                string ClientPhoneNo = "";
                if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {
                    var mClientUsesAdapter = new ClientUsersTableAdapter();
                    var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);
                    if (mTable.Any(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId))
                    {
                        var Q = mTable.Where(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId).First().PhoneNo;
                        if (!string.IsNullOrWhiteSpace(Q.Replace("-", "")))
                        {
                            ClientPhoneNo = Q;
                        }

                        else
                        {
                            ClientPhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                        }
                    }
                }
                else
                {
                    ClientPhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                }
                string _StartPhoneNo = "";
                string _StartMemo = "";

                string _StopPhoneNo = "";
                string _StopMemo = "";
              
                    _StartPhoneNo = " ";

             
                    _StartMemo = " ";

                
                    _StopPhoneNo = " ";

                
                    _StopMemo = " ";

                
                Message = $"{DriverName.Text} 님께서 배차 신청하신 {DriverCarNo.Text} 차량으로 배차처리가 완료되었습니다. 안전운전하시기 바랍니다." + "\r\n" +
                              //  $"\r\n" +
                              "■ 상차지" + "\r\n" +

                              $"{CurrentOrder.CreateTime.ToString("yyyy-MM-dd HH:mm").Replace("-", "/")}" + "\r\n" +
                              $"{StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}" + "\r\n" +

                              $"{Customer.Text} / {_StartPhoneNo}" + "\r\n" +
                              $"{_StartMemo}" + "\r\n" +
                               "■ 하차지" + "\r\n" +
                              $"{StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}" + "\r\n" +
                              $"{_StopPhoneNo}" + "\r\n" +
                              $"{_StopMemo}" + "\r\n" +
                               "■ 화물정보" + "\r\n" +
                              $"인수증후불 : {Price1.Text}원" + "\r\n" +
                              $"선/착불 : {(CurrentOrder.StartPrice ?? 0).ToString("N0")}원 / {(CurrentOrder.StopPrice ?? 0).ToString("N0")}원" + "\r\n" +
                              $"{Remark.Text}" + "\r\n" +
                               "■ 운송/주선사" + "\r\n" +
                              $"{LocalUser.Instance.LogInInformation.ClientName} / {ClientPhoneNo}" + "\r\n" +
                              $"\r\n" +
                              $"배송 후, 아래 \"전자세금계산서 발행\" 버튼을 눌러, \"차세로\" 어플을 설치하여 \"전자세금계산서\" 및 \"전자인수증\"을 발행해 주십시오." + "\r\n" +
                              $"※ 문의전화 : 1661-6090";


                #endregion

                DialogAppSms _DialogAppSms = new DialogAppSms("알림톡", Message, "", 1, DriverPhoneNo.Text.Replace("-", ""), "차주", (int)CurrentOrder.DriverId);

                if (_DialogAppSms.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var Table = new BizMsgDataSet.BIZ_MSGDataTable();

                        var Row = Table.NewBIZ_MSGRow();



                        Row.MSG_TYPE = 6;
                        Row.CMID = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + CurrentOrder.DriverId.ToString();
                        Row.REQUEST_TIME = DateTime.Now;
                        Row.SEND_TIME = DateTime.Now;
                        Row.DEST_PHONE = _DialogAppSms.cmbLMSMobileNo.Text.Replace("-", "");
                        Row.SEND_PHONE = "01084301200";
                        Row.MSG_BODY = Message;
                        Row.TEMPLATE_CODE = "bizp_2021082512574318902367241";
                        Row.SENDER_KEY = "1f68131ed852323c07f08acccc94e5d88719df62";
                        Row.NATION_CODE = "82";
                        Row.STATUS = 0;
                        Row.ATTACHED_FILE = "button.json";
                        Table.AddBIZ_MSGRow(Row);


                        var Adapter = new DataSets.BizMsgDataSetTableAdapters.BIZ_MSGTableAdapter();
                        Adapter.Update(Table);

                        Data.Connection((_Connection) =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "Update Orders SET LMSCnt = LMSCnt+1 WHERE OrderId = @OrderId";
                                _Command.Parameters.AddWithValue("@OrderId", CurrentOrder.OrderId);
                                _Command.ExecuteNonQuery();
                            }
                            _Connection.Close();

                        });



                        Data.Connection((_Connection) =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "INSERT ClientSmsCount (DailyCnt, ClientId, Date, CreateDate, UpdateDate,SMSGubun,sendgubun)Values(@DailyCnt,@ClientId,getdate(),getdate(),getdate(),@SMSGubun,@sendgubun)";
                                _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                                _Command.Parameters.Add("@DailyCnt", SqlDbType.Int);
                                _Command.Parameters.Add("@SMSGubun", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@sendgubun", SqlDbType.NVarChar);
                                _Command.Parameters["@SMSGubun"].Value = "LMS";

                                _Command.Parameters["@DailyCnt"].Value = 1;
                                _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                                _Command.Parameters["@sendgubun"].Value = "배차";
                                _Command.ExecuteNonQuery();
                            }
                            _Connection.Close();

                        });

                        ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                        {
                            ClientId = LocalUser.Instance.LogInInformation.ClientId,
                            CDate = DateTime.Now,
                            Amount = -10,
                            MethodType = "알림톡",
                            Remark = $"{DriverName.Text} ({DriverCarNo.Text})",
                        });
                        ShareOrderDataSet.Instance.SaveChanges();


                        MessageBox.Show("알림톡 전송이 완료 되었습니다.", "알림톡전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _Sort = false;
                        _Search();
                    }
                    catch (Exception)
                    {

                    }

                }



            }

            else if (e.ColumnIndex == CustomerSms.Index)
            {
                var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

                if (_ClientPoint < 10)
                {

                    MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                    FrmMDI.LoadForm("FrmMN0204", "Point");
                    return;
                }

                if (CurrentOrder.OrderStatus != 3 || String.IsNullOrEmpty(CurrentOrder.Customer))
                    return;
                if (CurrentOrder.PayLocation == 5)
                    return;

                //var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

                //if (_ClientPoint < 15)
                //{
                //    MessageBox.Show("충전금이 부족합니다. 충전 후, 이용하시기 바랍니다.\r\n회원정보로 이동합니다.\r\n회원정보 이동후 포인트 내역 탭의 가상계좌번호를 확인후 충전 바랍니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                //    FrmMN0204_CARGOOWNERMANAGE _Form = new FrmMN0204_CARGOOWNERMANAGE();
                //    _Form.MdiParent = this.MdiParent;

                //    _Form.Show();

                //    this.Close();

                //    return;
                //}



                string _PriceLms = "";
                String Message = "";
                //선/착불
                if (PayLocation.SelectedIndex == 2)
                {
                    //if (Price5.Text != "0")

                    //    _PriceLms = Price5.Text;
                    //else
                    //    _PriceLms = Price4.Text;
                    _PriceLms = "0" ;

                }
                else
                {
                    _PriceLms = (CurrentOrder.SalesPrice ?? 0).ToString("N0");
                }

                if(_KakaoPriceGubun==2)
                {
                    _PriceLms = "0";
                }
                if (CurrentOrder.OrderStatus != 3 || String.IsNullOrEmpty(CurrentOrder.StartState) || String.IsNullOrEmpty(CurrentOrder.StopState) || String.IsNullOrEmpty(CurrentOrder.Driver) || String.IsNullOrEmpty(CurrentOrder.DriverPhoneNo) || String.IsNullOrEmpty(CurrentOrder.DriverCarNo))
                {
                    MessageBox.Show("상하차지가 입력되지 않았습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                #region
                //if (string.IsNullOrEmpty(StartMemo.Text))
                //{
                //    Message = $"■ 상차지 - {CurrentOrder.StartTime.ToString("yyyy-MM-dd hh:mm").Replace("-", "/")}\r\n" +
                //           $"- {StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}\r\n" +

                //           $"- {StartPhoneNo.Text}\r\n" +

                //           $"- 화주({Customer.Text})\r\n" +
                //           // $"- {OrderPhoneNo.Text}\r\n" +

                //           $"■ 하차지\r\n" +
                //           $"- {StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}\r\n" +
                //           $"- {StopPhoneNo.Text}\r\n" +

                //           $"■ 화물정보\r\n" +
                //           $"- 운임 : {_PriceLms}원\r\n" +
                //           $"- {PayLocation.Text}/{Remark.Text}\r\n" +

                //           $"■ 배차정보\r\n" +
                //           $"- 기사명 : {CurrentOrder.Driver}\r\n" +
                //           $"- 핸드폰번호 : {CurrentOrder.DriverPhoneNo}\r\n" +
                //           $"- 차량번호 : {CurrentOrder.DriverCarNo}\r\n" +
                //           $"\r\n" +


                //           $"■ 주선사 ☏{LocalUser.Instance.LogInInformation.Client.PhoneNo}\r\n" +

                //           $"{LocalUser.Instance.LogInInformation.Client.Name}\r\n" +
                //           $"☎ {LocalUser.Instance.LogInInformation.Client.PhoneNo}\r\n\r\n" +
                //           //$"-----------------------------------\r\n" +

                //           $"\r\n\r\n" +
                //           $"이용해 주셔서 감사 합니다. ";
                //}
                //else
                //{
                //    Message = $"■ 상차지 - {CurrentOrder.StartTime.ToString("yyyy-MM-dd hh:mm").Replace("-", "/")}\r\n" +
                //            $"- {StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}\r\n" +
                //             $"- {StartMemo.Text} \r\n" +
                //            $"- {StartPhoneNo.Text}\r\n" +

                //            $"- 화주({Customer.Text})\r\n" +
                //            // $"- {OrderPhoneNo.Text}\r\n" +

                //            $"■ 하차지\r\n" +
                //            $"- {StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}\r\n" +
                //            $"- {StopPhoneNo.Text}\r\n" +

                //            $"■ 화물정보\r\n" +
                //            $"- 운임 : {_PriceLms}원\r\n" +
                //            $"- {PayLocation.Text}/{Remark.Text}\r\n" +

                //            $"■ 배차정보\r\n" +
                //            $"- 기사명 : {CurrentOrder.Driver}\r\n" +
                //            $"- 핸드폰번호 : {CurrentOrder.DriverPhoneNo}\r\n" +
                //            $"- 차량번호 : {CurrentOrder.DriverCarNo}\r\n" +
                //            $"\r\n" +


                //            $"■ 주선사 ☏{LocalUser.Instance.LogInInformation.Client.PhoneNo}\r\n" +

                //            $"{LocalUser.Instance.LogInInformation.Client.Name}\r\n" +
                //            $"☎ {LocalUser.Instance.LogInInformation.Client.PhoneNo}\r\n\r\n" +
                //            //$"-----------------------------------\r\n" +

                //            $"\r\n\r\n" +
                //            $"이용해 주셔서 감사 합니다. ";
                //}
                #endregion

                #region 알림톡
                string ClientPhoneNo = "";
                if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {
                    var mClientUsesAdapter = new ClientUsersTableAdapter();
                    var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);
                    if (mTable.Any(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId))
                    {
                        var Q = mTable.Where(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId).First().PhoneNo;
                        if (!string.IsNullOrWhiteSpace(Q.Replace("-", "")))
                        {
                            ClientPhoneNo = Q;
                        }
                        else
                        {
                            ClientPhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                        }
                    }
                }
                else
                {
                    ClientPhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                }

                string _StartPhoneNo = "";
                string _StartMemo = "";

                string _StopPhoneNo = "";
                string _StopMemo = "";
               
                    _StartPhoneNo = " ";

               
                    _StartMemo = " ";

                
                    _StopPhoneNo = " ";

               
                    _StopMemo = " ";

               
                Message = $"{Customer.Text} 님께서 의뢰하신 배차 완료내역 입니다." + "\r\n" +
                            "■ 상차지" + "\r\n" +
                            $"{CurrentOrder.CreateTime.ToString("yyyy-MM-dd HH:mm").Replace("-", "/")}" + "\r\n" +
                            $"{StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}" + "\r\n" +
                            $"{_StartPhoneNo}" + "\r\n" +
                            $"{_StartMemo}" + "\r\n" +
                            $"■ 하차지" + "\r\n" +
                            $"{StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}" + "\r\n" +
                            $"{ _StopPhoneNo}" + "\r\n" +
                            $"{_StopMemo}" + "\r\n" +
                            "■ 화물정보" + "\r\n" +
                            $"인수증후불 : {_PriceLms}원" + "\r\n" +
                            $"선/착불 : {(CurrentOrder.StartPrice ?? 0).ToString("N0")}원 / {(CurrentOrder.StopPrice ?? 0).ToString("N0")}원" + "\r\n" +
                            $"{Remark.Text}" + "\r\n" +
                            "■ 운송/주선사" + "\r\n" +
                            $"{LocalUser.Instance.LogInInformation.ClientName} / {ClientPhoneNo}" + "\r\n" +
                            "■ 배차차량정보" + "\r\n" +
                            $"기사명 : {DriverName.Text}" + "\r\n" +
                            $"차량번호 : {DriverCarNo.Text}" + "\r\n" +
                            $"연락처 : {DriverPhoneNo.Text}" + "\r\n\r\n" +

                            "차주님께서 어플로 발행한 \"전자세금계산서\" 및 \"전자인수증\"은 www.chasero.co.kr 홈페이지에서 직접 조회해 보실 수 있습니다.";




                //Message = $"{Customer.Text} 님께서 의뢰하신 화물배차 내역입니다." + "\r\n\r\n" +

                //                          "■ 상차지 " + "\r\n" +
                //                          $"- {CurrentOrder.CreateTime.ToString("yyyy-MM-dd HH:mm").Replace("-", "/")}" + "\r\n" +
                //                          $"- {StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}" + "\r\n" +
                //                          $"- {StartMemo.Text}" + "\r\n" +
                //                          $"- {StartPhoneNo.Text}" + "\r\n" +
                //                          $"- 화주:{Customer.Text}" + "\r\n\r\n" +

                //                         "■ 하차지 " + "\r\n" +
                //                         $"- {StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}" + "\r\n" +
                //                         $"- {StopPhoneNo.Text} / {StopMemo.Text}" + "\r\n\r\n" +

                //                        "■ 화물정보 " + "\r\n" +
                //                        $"- 운임: {_PriceLms} 원" + "\r\n" +
                //                        //$"- 운임: 0 원" + "\r\n" +
                //                        $"- 선불: {(CurrentOrder.StartPrice ?? 0).ToString("N0")} 원" + "\r\n" +
                //                        $"- 착불: {(CurrentOrder.StopPrice ?? 0).ToString("N0")} 원" + "\r\n" +
                //                        $"- {Remark.Text}" + "\r\n\r\n" +


                //                        "■ 배차정보 " + "\r\n" +
                //                        $"- 기사명: {DriverName.Text}" + "\r\n" +
                //                        $"- 차량번호: {DriverCarNo.Text}" + "\r\n" +
                //                        $"- 핸드폰번호: {DriverPhoneNo.Text}" + "\r\n\r\n" +

                //                       "■ 주선사 " + "\r\n" +
                //                       $"- 상호 : {LocalUser.Instance.LogInInformation.ClientName}" + "\r\n" +
                //                        //$"- 전화 : {LocalUser.Instance.LogInInformation.Client.PhoneNo}" + "\r\n\r\n" +
                //                        $"- 전화 : {ClientPhoneNo}" + "\r\n\r\n" +

                //                       "기사님께서 전자인수증으로 전송한 경우, " + "\r\n" +
                //                       "www.chasero.co.kr 홈페이지에서 확인하실 수 있습니다.\r\n\r\n" +
                //                       "이용해 주셔서 감사 합니다.";

                #endregion

                var _CustomerMobileNo = "";
                if (Customer.Tag == null)
                {
                    if (CurrentOrder.FOrderId != 0)
                    {
                        var Query = _ClientTable.Where(c => c.Name == Customer.Text).ToArray();
                        if (Query.Any())
                        {
                            _CustomerMobileNo = Query.First().MobileNo;
                        }
                        else
                        {
                            MessageBox.Show($"화주 핸드폰번호가 없습니다.", "알림톡전송", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;

                        }
                    }
                    else
                    {
                        _CustomerMobileNo = CustomerModelList.Find(c => c.SangHo == Customer.Text).MobileNo;
                    }
                }
                else
                {

                    _CustomerMobileNo = CustomerModelList.Find(c => c.CustomerId == (int)Customer.Tag).MobileNo;
                }

                if (String.IsNullOrEmpty(_CustomerMobileNo.Replace("-", "")))
                {
                    MessageBox.Show($"화주 핸드폰번호가 없습니다.", "알림톡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogAppSms _DialogAppSms = new DialogAppSms("알림톡", Message, "", 1, _CustomerMobileNo.Replace("-", ""), "화주");

                if (_DialogAppSms.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var Table = new BizMsgDataSet.BIZ_MSGDataTable();

                        var Row = Table.NewBIZ_MSGRow();


                        Row.MSG_TYPE = 6;
                        Row.CMID = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + CurrentOrder.CustomerId.ToString();
                        Row.REQUEST_TIME = DateTime.Now;
                        Row.SEND_TIME = DateTime.Now;
                        Row.DEST_PHONE = _DialogAppSms.LMSMobileNo.Text.Replace("-", "");
                        Row.SEND_PHONE = "01084301200";
                        Row.MSG_BODY = Message;
                        Row.TEMPLATE_CODE = "bizp_2021082513024818902613242";
                        Row.SENDER_KEY = "1f68131ed852323c07f08acccc94e5d88719df62";
                        Row.NATION_CODE = "82";
                        Row.STATUS = 0;



                        // Row.ATTACHED_FILE = "button.json";

                        Table.AddBIZ_MSGRow(Row);


                        var Adapter = new DataSets.BizMsgDataSetTableAdapters.BIZ_MSGTableAdapter();
                        Adapter.Update(Table);



                        Data.Connection((_Connection) =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "Update Orders SET LMSCustomerCnt = LMSCustomerCnt+1 WHERE OrderId = @OrderId";
                                _Command.Parameters.AddWithValue("@OrderId", CurrentOrder.OrderId);
                                _Command.ExecuteNonQuery();
                            }
                            _Connection.Close();

                        });



                        Data.Connection((_Connection) =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "INSERT ClientSmsCount (DailyCnt, ClientId, Date, CreateDate, UpdateDate,SMSGubun,sendgubun)Values(@DailyCnt,@ClientId,getdate(),getdate(),getdate(),@SMSGubun,@sendgubun)";
                                _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                                _Command.Parameters.Add("@DailyCnt", SqlDbType.Int);
                                _Command.Parameters.Add("@SMSGubun", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@sendgubun", SqlDbType.NVarChar);
                                _Command.Parameters["@SMSGubun"].Value = "LMS";

                                _Command.Parameters["@DailyCnt"].Value = 1;
                                _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                                _Command.Parameters["@sendgubun"].Value = "배차";
                                _Command.ExecuteNonQuery();
                            }
                            _Connection.Close();

                        });


                        var Query = CustomerModelList.Where(c => c.CustomerId == CurrentOrder.CustomerId).ToArray();
                        ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                        {
                            ClientId = LocalUser.Instance.LogInInformation.ClientId,
                            CDate = DateTime.Now,
                            Amount = -10,
                            MethodType = "알림톡",
                            Remark = $"{Customer.Text} ({Query.First().BizNo.Replace("-", "")})",
                        });
                        ShareOrderDataSet.Instance.SaveChanges();

                        MessageBox.Show($"알림톡 전송이 완료 되었습니다.", "알림톡전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _Sort = false;
                        _Search();
                    }
                    catch (Exception)
                    {

                    }

                }




            }

            //else if (e.ColumnIndex == Column1.Index)
            //{
            //    if (LocalUser.Instance.LogInInformation.ClientId != 21)
            //    {
            //        return;
            //    }
            //    if (CurrentOrder == null)
            //        return;

            //    if (CurrentOrder.OrderStatus != 3 || String.IsNullOrEmpty(CurrentOrder.DriverPhoneNo))
            //        return;

            //    string _PriceLms = "";
            //    String Message = "";
            //    //선/착불
            //    if (PayLocation.SelectedIndex == 2)
            //    {
            //        //if (Price5.Text != "0")

            //        //    _PriceLms = Price5.Text;
            //        //else
            //        //    _PriceLms = Price4.Text;
            //        _PriceLms = (CurrentOrder.StartPrice + CurrentOrder.StopPrice ?? 0).ToString("N0");

            //    }
            //    else
            //    {
            //        _PriceLms = (CurrentOrder.SalesPrice ?? 0).ToString("N0");
            //    }

            //    var Table = new CMDataSet.MSG_DATADataTable();

            //    var Row = Table.NewMSG_DATARow();


            //    Row.SENDER_KEY = "29533bce5a502550b740c7090e00b8353fc462ae";
            //    Row.PHONE = DriverPhoneNo.Text.Replace("-", "");
            //    Row.TMPL_CD = "K18-0005";
            //    Row.SEND_MSG = "배차를 위한 화물정보 입니다." + "\r\n" +
            //                    "배차를 받으시려면 하단 배차받기" + "\r\n" +
            //                    "버튼을 클릭 하십시오." + "\r\n" +


            //                    " * 상차 : " + CurrentOrder.StartTime.ToString("yyyy-MM-dd hh:mm").Replace("-", "/") + " " + "\r\n" +
            //                    " * 출발 : " + StartState.Text + " " + StartCity.Text + " " + "\r\n" +
            //                    " * 도착 : " + StopState.Text + " " + StopCity.Text + " " + "\r\n" +
            //                    " * 차종 : " + CarType.Text + "" + "\r\n" +
            //                    " * 톤수 : " + CarSize.Text + "" + "\r\n" +
            //                    " * 화물 : " + Item.Text + "" + "\r\n" +
            //                    " * 금액 : " + Price1.Text + "" + "\r\n\r\n" +

            //                    " * 운송회사 : " + LocalUser.Instance.LogInInformation.ClientName + "" + "\r\n" +
            //                    " * 문의전화 : " + LocalUser.Instance.LogInInformation.Client.PhoneNo + "";

            //    #region 화주
            //    //Row.SEND_MSG = $"{Customer.Text} 님께서 의뢰하신 화물배차 내역입니다." + "\n\n" +

            //    //               "■ 상차지 " + "\n" +
            //    //               $"- {CurrentOrder.StartTime.ToString("yyyy-MM-dd hh:mm").Replace("-", "/")}" + "\n" +
            //    //               $"- {StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}" + "\n" +
            //    //               $"- {StartMemo.Text}" + "\n" +
            //    //               $"- {StartPhoneNo.Text}" + "\n" +
            //    //               $"- 화주:{Customer.Text}" + "\n\n" +

            //    //              "■ 하차지 " + "\n" +
            //    //              $"- {StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}" + "\n" +
            //    //              $"- {StopPhoneNo.Text}" + "\n\n" +

            //    //             "■ 화물정보 " + "\n" +
            //    //             $"- 운임: {_PriceLms} 원" + "\n" +
            //    //             $"- 선불: 0 원" + "\n" +
            //    //             $"- 착불: 0 원" + "\n" +
            //    //             $"- {Remark.Text}" + "\n\n" +


            //    //             "■ 배차정보 " + "\n" +
            //    //             $"- 기사명: {DriverName.Text}" + "\n" +
            //    //             $"- 차량번호: {DriverCarNo.Text}" + "\n" +
            //    //             $"- 핸드폰번호: {DriverPhoneNo.Text}" + "\n\n" +

            //    //            "■ 주선사 " + "\n" +
            //    //            $"- 상호 : {LocalUser.Instance.LogInInformation.ClientName}" + "\n" +
            //    //            $"- 전화 : {LocalUser.Instance.LogInInformation.Client.PhoneNo}" + "\n\n" +

            //    //            "기사님께서 전자인수증으로 전송한 경우, " + "\n" +
            //    //            "www.chasero.co.kr 홈페이지에서 확인하실 수 있습니다.\n\n" +
            //    //            "이용해 주셔서 감사 합니다.";
            //    #endregion

            //    #region 차주
            //    //InitDriverTable();
            //    //var LoginId = _DriverTable.Where(c => c.CarNo == DriverCarNo.Text);
            //    //Row.SEND_MSG = $"{Driver.Text} 님,{DriverCarNo.Text} 차량의 배차내역입니다. 안전 운전 하시기 바랍니다." + "\n\n" +

            //    //               "■ 상차지 " + "\n" +
            //    //               $"- {CurrentOrder.StartTime.ToString("yyyy-MM-dd hh:mm").Replace("-", "/")}" + "\n" +
            //    //               $"- {StartState.Text} {StartCity.Text} {StartStreet.Text} {Start.Text}" + "\n" +
            //    //               $"- {StartMemo.Text}" + "\n" +
            //    //               $"- {StartPhoneNo.Text}" + "\n" +
            //    //               $"- 화주:{Customer.Text}" + "\n\n" +

            //    //              "■ 하차지 " + "\n" +
            //    //              $"- {StopState.Text} {StopCity.Text} {StopStreet.Text} {Stop.Text}" + "\n" +
            //    //              $"- {StopPhoneNo.Text}" + "\n\n" +

            //    //             "■ 화물정보 " + "\n" +
            //    //             $"- 운임: {Price1.Text} 원" + "\n" +
            //    //             $"- 선불: {CurrentOrder.StartPrice} 원" + "\n" +
            //    //             $"- 착불: {CurrentOrder.StopPrice} 원" + "\n" +
            //    //             $"- {Remark.Text}" + "\n\n" +


            //    //            "■ 운송/주선사 " + "\n" +
            //    //            $"- 상호 : {LocalUser.Instance.LogInInformation.ClientName}" + "\n" +
            //    //            $"- 전화 : {LocalUser.Instance.LogInInformation.Client.PhoneNo}" + "\n" +
            //    //            $"※세금계산서/인수증 등 배차관련 문의사항은 위 운송/주선사로 연락 바랍니다." + "\n\n" +

            //    //            $"■ {"차세로"} 세금계산서 발행어플 " + "\n" +
            //    //            $"- 아이디:{ LoginId.First().LoginId }"+"\n" +
            //    //            $"- 비밀번호:{ LoginId.First().Password }" + "\n\n" +

            //    //            "이용해 주셔서 감사 합니다.\n\n"+
            //    //            "※.세금계산서(인수증) 발행은 아래 버튼을 눌러주세요.\n\n";
            //    #endregion

            //    Row.REQ_DATE = DateTime.Now;
            //    Row.CUR_STATE = "0";
            //    Row.SMS_TYPE = "N";
            //    Row.ATTACHMENT_TYPE = "button";
            //    Row.ATTACHMENT_NAME = "배차받기";
            //    Row.ATTACHMENT_URL = $"http://m.cardpay.kr/Link?Id={CurrentOrder.OrderId}";
            //    Table.AddMSG_DATARow(Row);
                

            //    var Adapter = new CMDataSetTableAdapters.MSG_DATATableAdapter();
            //    Adapter.Update(Table);
            //}
        }

        private void CopyToApplication_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current == null)
                return;

            if (MessageBox.Show($"화주명 : {Customer.Text}\r\n상차지 : {StartName.Text}\r\n하차지 : {StopName.Text}\r\n화물명 : {Item.Text}\r\n운송비구분 : {PayLocation.Text}\r\n\r\n{SelectApplication.Text}으로\r\n{""}콜패스(자동입력)하시겠습니까?", "콜패스 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            //24시 구버전
            if (SelectApplication.SelectedIndex == 0)
            {
                Cursor = Cursors.WaitCursor;
                SendMessageToAnotherApplication SendTextToAnotherApplication = new SendMessageToAnotherApplication("DFreighter24", new string[] { "TfrmMain" });
                SendTextToAnotherApplication.FindControlList();
                //신규
                SendTextToAnotherApplication.TBitBtnClick(17);
                Thread.Sleep(100);

                //상차지
                _SearchAddress(StartStreet.Text);
                if (_CoreList.Any())
                {
                  
                    string _StartState = "";
                    string _StartCity = "";
                    string _StartStreet = "";
                    if (_CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).Any())
                    {
                        var ss = _CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                        _StartState = ss[0];
                        _StartCity = ss[1];
                        _StartStreet = ss[2];

                    }
                    else
                    {
                        var ss = _CoreList.Where(c => c.Address.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                        _StartState = ss[0];
                        _StartCity = ss[1];
                        _StartStreet = ss[2];

                    }
                    SendTextToAnotherApplication.SetText(15, _AddressStateParse(_StartState));
                    SendTextToAnotherApplication.SetText(9, _AddressCityParse(_StartCity));
                    SendTextToAnotherApplication.SetText(7, _StartStreet);
                    SendTextToAnotherApplication.SetText(14, _AddressCityParse(_StartCity) + " " + _StartStreet);

                    if (string.IsNullOrEmpty(StartStreet.Text))
                    {
                        SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                        SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                        SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                        SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
                    }
                }
                else
                {

                    SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                    SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                    SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                    SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
                }

                //하차지
                _SearchAddress(StopStreet.Text);
                if (_CoreList.Any())
                {
                    //var ss = _CoreList.Where(c=> c.Jibun.Contains(StopStreet.Text)).First().Jibun.Split(' ');
                    //string _StopState = ss[0];
                    //string _StopCity = ss[1];
                    //string _StopStreet = ss[2];

                    //SendTextToAnotherApplication.SetText(13, _AddressStateParse(_StopState));
                    //SendTextToAnotherApplication.SetText(8, _AddressCityParse(_StopCity));
                    //SendTextToAnotherApplication.SetText(6, _StopStreet);
                    //SendTextToAnotherApplication.SetText(12, _AddressCityParse(_StopCity) + " " + _StopStreet);
                    string _StopState = "";
                    string _StopCity = "";
                    string _StopStreet = "";
                    if (_CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).Any())
                    {
                        var ss = _CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).First().Jibun.Split(' ');

                        _StopState = ss[0];
                        _StopCity = ss[1];
                        _StopStreet = ss[2];

                    }
                    else
                    {
                        var ss = _CoreList.Where(c => c.Address.Contains(StopStreet.Text)).First().Jibun.Split(' ');
                        _StopState = ss[0];
                        _StopCity = ss[1];
                        _StopStreet = ss[2];

                    }
                    SendTextToAnotherApplication.SetText(13, _AddressStateParse(_StopState));
                    SendTextToAnotherApplication.SetText(8, _AddressCityParse(_StopCity));
                    SendTextToAnotherApplication.SetText(6, _StopStreet);
                    SendTextToAnotherApplication.SetText(12, _AddressCityParse(_StopCity) + " " + _StopStreet);
                    if (string.IsNullOrEmpty(StopStreet.Text))
                    {
                        SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                        SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                        SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                        SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);
                    }
                }
                else
                {
                    SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                    SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                    SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                    SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);

                }


                //대수
                SendTextToAnotherApplication.SetText(11, CarCount.Text);
                //추가정보
                //SendTextToAnotherApplication.SetText(10, Item.Text);
                ////상차일시
                //SendTextToAnotherApplication.SetText(5, StartTimeType4.Text);
                ////하차일시
                //SendTextToAnotherApplication.SetText(4, StopTimeType5.Text);
                //의뢰자명
                SendTextToAnotherApplication.SetText(3, LocalUser.Instance.LogInInformation.Client.Name);
                //전화번호
                SendTextToAnotherApplication.SetText(1, LocalUser.Instance.LogInInformation.Client.PhoneNo);
                //전화번호
               // SendTextToAnotherApplication.SetText(2, StopPhoneNo.Text);
                //의뢰자 사업자번호
                //String CustomerBizNo = String.Empty;
                //if (Customer.Tag is Int32 CustomerId)
                //{
                //    if (CustomerModelList.Any(c => c.CustomerId == CustomerId))
                //    {
                //        CustomerBizNo = CustomerModelList.First(c => c.CustomerId == CustomerId).BizNo;
                //    }
                //}
                SendTextToAnotherApplication.SetText(0, LocalUser.Instance.LogInInformation.Client.BizNo);
                //톤수
                SendTextToAnotherApplication.SetText(16, ItemSize.Text);
                //SendTextToAnotherApplication.SetText(10, ItemSize.Text);


              


                //운송료,수수료,합계
                //인수증
                if (PayLocation.SelectedIndex == 1)
                {
                    //운송료
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(2, Price1.Text.Replace(",", ""));
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(1, "0");
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(0, Price1.Text.Replace(",", ""));
                }
                //선착불
                else if (PayLocation.SelectedIndex == 2)
                {

                    //합계
                    Int64 Price = Convert.ToInt64(Price4.Text.Replace(",", "")) + Convert.ToInt64(Price5.Text.Replace(",", ""));
                    //운송료
                    Int64 _Price = Price - Convert.ToInt64(Price6.Text.Replace(",", ""));
                    //운송료
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(2, _Price.ToString());
                    //수수료
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price6.Text.Replace(",", ""));

                    //합계
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(0, Price.ToString());



                    //Int64 Price = Convert.ToInt64(Price4.Text.Replace(",", "")) + Convert.ToInt64(Price5.Text.Replace(",", ""));
                    ////운송료
                    //SendTextToAnotherApplication.SetTextbyTwNumEdit(2, Price.ToString());
                    ////수수료
                    //SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price6.Text.Replace(",", ""));
                    ////합계
                    //Int64 PriceSum = Price + Convert.ToInt64(Price6.Text.Replace(",", ""));

                    //SendTextToAnotherApplication.SetTextbyTwNumEdit(0, PriceSum.ToString());
                }

                //도착
                SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(8, StopDateHelper.SelectedIndex);

                //상차일시
                //if (StartTimeType1.Checked)
                //    SendTextToAnotherApplication.SetCheck(10);
                //if (StartTimeType2.Checked)
                //    SendTextToAnotherApplication.SetCheck(13);
                //if (StartTimeType3.Checked)
                //    SendTextToAnotherApplication.SetCheck(12);
                //if (StartTimeHalf.Checked)
                //    SendTextToAnotherApplication.SetCheck(11);
                ////하차일시
                //if (StopTimeType1.Checked)
                //    SendTextToAnotherApplication.SetCheck(4);
                //if (StopTimeType2.Checked)
                //    SendTextToAnotherApplication.SetCheck(3);
                //if (StopTimeType3.Checked)
                //    SendTextToAnotherApplication.SetCheck(1);
                //if (StopTimeType4.Checked)
                //    SendTextToAnotherApplication.SetCheck(0);
                //if (StopTimeHalf.Checked)
                //    SendTextToAnotherApplication.SetCheck(2);
                ////상차방법
                //if (StartInfo1.Checked)
                //    SendTextToAnotherApplication.SetCheck(18);
                //if (StartInfo2.Checked)
                //    SendTextToAnotherApplication.SetCheck(17);
                //if (StartInfo3.Checked)
                //    SendTextToAnotherApplication.SetCheck(16);
                //if (StartInfo4.Checked)
                //    SendTextToAnotherApplication.SetCheck(15);
                //if (StartInfo5.Checked)
                //    SendTextToAnotherApplication.SetCheck(14);
                ////하차방법
                //if (StopInfo1.Checked)
                //    SendTextToAnotherApplication.SetCheck(9);
                //if (StopInfo2.Checked)
                //    SendTextToAnotherApplication.SetCheck(8);
                //if (StopInfo3.Checked)
                //    SendTextToAnotherApplication.SetCheck(7);
                //if (StopInfo4.Checked)
                //    SendTextToAnotherApplication.SetCheck(6);
                //if (StopInfo5.Checked)
                //    SendTextToAnotherApplication.SetCheck(5);
                ////예약,왕복,긴급
                //if (Reservation.Checked)
                //    SendTextToAnotherApplication.SetCheck(19);
                //if (Round.Checked)
                //    SendTextToAnotherApplication.SetCheck(20);
                //if (Emergency.Checked)
                //    SendTextToAnotherApplication.SetCheck(21);
                ////독차,혼적
                //if (NotShared.Checked)
                //    SendTextToAnotherApplication.SetCheck(22);
                //if (Shared.Checked)
                //    SendTextToAnotherApplication.SetCheck(23);
                //~톤 이하
                if (ItemSizeInclude.Checked)
                    SendTextToAnotherApplication.SetCheck(24);
                Thread.Sleep(1000);
                //톤수,차종
                SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBoxByKey(0, CarSize.SelectedIndex + 1, CarSize.Items.Count);
                // SendTextToAnotherApplication.SetText(0, CarSize.Text);


                Thread.Sleep(1000);
                if (CarType.SelectedIndex > 3)
                {
                    SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(1, CarType.SelectedIndex - 1);
                }
                else
                {
                    SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(1, CarType.SelectedIndex);
                }


                ////하차일시
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(3, StopTimeHour1.SelectedIndex);
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(2, StopTimeHour2.SelectedIndex);
                ////상차일시
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(5, StartTimeHour1.SelectedIndex);
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(4, StartTimeHour2.SelectedIndex);
                //if (Shared.Checked)
                //{
                //    //혼적1
                //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(2, SharedItemLength.Text, SharedItemLength.Items.Count);
                //    //혼적2
                //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(1, SharedItemSize.Text, SharedItemSize.Items.Count);
                //}
                //운송비구분
                if (PayLocation.SelectedIndex == 1 || PayLocation.SelectedIndex == 2)
                {
                    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(3, PayLocation.Text, PayLocation.Items.Count);
                }
                //추가정보
                SendTextToAnotherApplication.SetTextWithReturnKey(10, Item.Text);
                // SendTextToAnotherApplication.SetTextWithReturnKey(11, Item.Text);
                Cursor = Cursors.Arrow;
            }
            else if (SelectApplication.SelectedIndex == 1)
            {
                Cursor = Cursors.WaitCursor;
                SendMessageToAnotherApplication SendTextToAnotherApplication = new SendMessageToAnotherApplication("Cargo24", new string[] { "TfrmCargoMain" });
                SendTextToAnotherApplication.FindControlList();
                //신규
                SendTextToAnotherApplication.TBitBtnClick(17);
                Thread.Sleep(100);

                //상차지
                _SearchAddress(StartStreet.Text);
                if (_CoreList.Any())
                {
                    string _StartState = "";
                    string _StartCity = "";
                    string _StartStreet = "";
                    if (_CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).Any())
                    {
                        var ss = _CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                        _StartState = ss[0];
                        _StartCity = ss[1];
                        _StartStreet = ss[2];

                    }
                    else
                    {
                        var ss = _CoreList.Where(c => c.Address.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                        _StartState = ss[0];
                        _StartCity = ss[1];
                        _StartStreet = ss[2];

                    }
                    SendTextToAnotherApplication.SetText(15, _AddressStateParse(_StartState));
                    SendTextToAnotherApplication.SetText(9, _AddressCityParse(_StartCity));
                    SendTextToAnotherApplication.SetText(7, _StartStreet);
                    SendTextToAnotherApplication.SetText(14, _AddressCityParse(_StartCity) + " " + _StartStreet);



                    if (string.IsNullOrEmpty(StartStreet.Text))
                    {
                        SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                        SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                        SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                        SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
                    }
                }
                else
                {

                    SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                    SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                    SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                    SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
                }

                //하차지
                _SearchAddress(StopStreet.Text);
                if (_CoreList.Any())
                {

                    string _StopState = "";
                    string _StopCity = "";
                    string _StopStreet = "";
                    if (_CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).Any())
                    {
                        var ss = _CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).First().Jibun.Split(' ');

                        _StopState = ss[0];
                        _StopCity = ss[1];
                        _StopStreet = ss[2];

                    }
                    else
                    {
                        var ss = _CoreList.Where(c => c.Address.Contains(StopStreet.Text)).First().Jibun.Split(' ');
                        _StopState = ss[0];
                        _StopCity = ss[1];
                        _StopStreet = ss[2];

                    }

                    SendTextToAnotherApplication.SetText(13, _AddressStateParse(_StopState));
                    SendTextToAnotherApplication.SetText(8, _AddressCityParse(_StopCity));
                    SendTextToAnotherApplication.SetText(6, _StopStreet);
                    SendTextToAnotherApplication.SetText(12, _AddressCityParse(_StopCity) + " " + _StopStreet);

                    if (string.IsNullOrEmpty(StopStreet.Text))
                    {
                        SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                        SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                        SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                        SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);
                    }
                }
                else
                {
                    SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                    SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                    SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                    SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);

                }


                //대수
                SendTextToAnotherApplication.SetText(11, CarCount.Text);
                //추가정보
                //SendTextToAnotherApplication.SetText(10, Item.Text);
                ////상차일시
                //SendTextToAnotherApplication.SetText(5, StartTimeType4.Text);
                ////하차일시
                //SendTextToAnotherApplication.SetText(4, StopTimeType5.Text);
                //의뢰자명
                SendTextToAnotherApplication.SetText(3, LocalUser.Instance.LogInInformation.ClientName);
                //전화번호
                SendTextToAnotherApplication.SetText(1, LocalUser.Instance.LogInInformation.Client.PhoneNo);

                //주선사 사업자번호
                SendTextToAnotherApplication.SetText(0, LocalUser.Instance.LogInInformation.Client.BizNo);

                //전화번호
                // SendTextToAnotherApplication.SetText(2, StopPhoneNo.Text);
                //의뢰자 사업자번호
                //String CustomerBizNo = String.Empty;
                //if (Customer.Tag is Int32 CustomerId)
                //{
                //    if (CustomerModelList.Any(c => c.CustomerId == CustomerId))
                //    {
                //        CustomerBizNo = CustomerModelList.First(c => c.CustomerId == CustomerId).BizNo;
                //    }
                //}
                //  SendTextToAnotherApplication.SetText(0, CustomerBizNo);
                //톤수
                SendTextToAnotherApplication.SetText(16, ItemSize.Text);
                //SendTextToAnotherApplication.SetText(10, ItemSize.Text);

                //운송료,수수료,합계
                //인수증
                if (PayLocation.SelectedIndex == 1)
                {
                    //운송료
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(0, Price1.Text.Replace(",", ""));
                    //수수료
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(2, "0");
                    //합계
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price1.Text.Replace(",", ""));

                }
                //선착불
                else if (PayLocation.SelectedIndex == 2)
                {
                    //합계
                    Int64 Price = Convert.ToInt64(Price4.Text.Replace(",", "")) + Convert.ToInt64(Price5.Text.Replace(",", ""));
                    //운송료
                    Int64 _Price = Price - Convert.ToInt64(Price6.Text.Replace(",", ""));
                    //운송료
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(0, _Price.ToString());
                    //수수료
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(2, Price6.Text.Replace(",", ""));

                    //합계
                    SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price.ToString());
                    //Int64 PriceSum = Price + Convert.ToInt64(Price.ToString());


                }

                //도착
                SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(8, StopDateHelper.SelectedIndex);

                ////상차일시
                //if (StartTimeType1.Checked)
                //    SendTextToAnotherApplication.SetCheck(10);
                //if (StartTimeType2.Checked)
                //    SendTextToAnotherApplication.SetCheck(13);
                //if (StartTimeType3.Checked)
                //    SendTextToAnotherApplication.SetCheck(12);
                //if (StartTimeHalf.Checked)
                //    SendTextToAnotherApplication.SetCheck(11);
                ////하차일시
                //if (StopTimeType1.Checked)
                //    SendTextToAnotherApplication.SetCheck(4);
                //if (StopTimeType2.Checked)
                //    SendTextToAnotherApplication.SetCheck(3);
                //if (StopTimeType3.Checked)
                //    SendTextToAnotherApplication.SetCheck(1);
                //if (StopTimeType4.Checked)
                //    SendTextToAnotherApplication.SetCheck(0);
                //if (StopTimeHalf.Checked)
                //    SendTextToAnotherApplication.SetCheck(2);
                ////상차방법
                //if (StartInfo1.Checked)
                //    SendTextToAnotherApplication.SetCheck(18);
                //if (StartInfo2.Checked)
                //    SendTextToAnotherApplication.SetCheck(17);
                //if (StartInfo3.Checked)
                //    SendTextToAnotherApplication.SetCheck(16);
                //if (StartInfo4.Checked)
                //    SendTextToAnotherApplication.SetCheck(15);
                //if (StartInfo5.Checked)
                //    SendTextToAnotherApplication.SetCheck(14);
                ////하차방법
                //if (StopInfo1.Checked)
                //    SendTextToAnotherApplication.SetCheck(9);
                //if (StopInfo2.Checked)
                //    SendTextToAnotherApplication.SetCheck(8);
                //if (StopInfo3.Checked)
                //    SendTextToAnotherApplication.SetCheck(7);
                //if (StopInfo4.Checked)
                //    SendTextToAnotherApplication.SetCheck(6);
                //if (StopInfo5.Checked)
                //    SendTextToAnotherApplication.SetCheck(5);
                ////예약,왕복,긴급
                //if (Reservation.Checked)
                //    SendTextToAnotherApplication.SetCheck(19);
                //if (Round.Checked)
                //    SendTextToAnotherApplication.SetCheck(20);
                //if (Emergency.Checked)
                //    SendTextToAnotherApplication.SetCheck(21);
                ////독차,혼적
                //if (NotShared.Checked)
                //    SendTextToAnotherApplication.SetCheck(22);
                //if (Shared.Checked)
                //    SendTextToAnotherApplication.SetCheck(23);
                //~톤 이하
                if (ItemSizeInclude.Checked)
                    SendTextToAnotherApplication.SetCheck(24);



                Thread.Sleep(1000);


                //톤수,차종
                //switch(CarSize.Text)
                // {
                //     case "0.3":
                //         SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, 2);

                //         break;
                //     case "0.5":
                //         SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, 3);

                //         break;
                // }
                // SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, CarSize.SelectedIndex + 2);
                SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBoxByKey(1, CarSize.SelectedIndex + 1, CarSize.Items.Count + 1);

                // SendTextToAnotherApplication.SetText(0, CarSize.Text);


                Thread.Sleep(1000);
                //if (CarType.SelectedIndex > 3)
                //{
                //    SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, CarType.SelectedIndex - 1);
                //}
                //else
                //{
                //    SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, CarType.SelectedIndex);
                //}
                if (CarType.Text == "트레일러" || CarType.Text == "카고+윙바디")
                {
                    SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, "차종확인", CarType.Items.Count + 2);

                }
                else
                {


                    switch (CarSize.Text)
                    {
                        //31

                        case "1":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;

                        case "1.4":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "2.5":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "3.5":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;

                        case "5":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "8":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "11":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "14":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "15":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "18":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;
                        case "25":
                            SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                            break;

                    }
                }


                ////하차일시
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(3, StopTimeHour1.SelectedIndex);
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(2, StopTimeHour2.SelectedIndex);
                ////상차일시
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(5, StartTimeHour1.SelectedIndex);
                //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(4, StartTimeHour2.SelectedIndex);
                //if (Shared.Checked)
                //{
                //    //혼적1
                //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(2, SharedItemLength.Text, SharedItemLength.Items.Count);
                //    //혼적2
                //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(1, SharedItemSize.Text, SharedItemSize.Items.Count);
                //}
                //운송비구분
                if (PayLocation.SelectedIndex == 1 || PayLocation.SelectedIndex == 2)
                {
                    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(3, PayLocation.Text, PayLocation.Items.Count);
                }
                //추가정보
                SendTextToAnotherApplication.SetTextWithReturnKey(10, Item.Text);
              
              
                  Cursor = Cursors.Arrow;
               
            }

            //화물맨 빽통2014
            else if (SelectApplication.SelectedIndex == 2)
            {
                Cursor = Cursors.WaitCursor;
                SendMessageToAnotherApplication SendTextToAnotherApplication = new SendMessageToAnotherApplication("PRJ_100TONG", "WindowsForms10.Window.8.app", true);
                SendTextToAnotherApplication.FindControlList();
                //신규
                SendTextToAnotherApplication.MainKeyPressF4();
                Thread.Sleep(100);
                //상차지
                //SendTextToAnotherApplication.WindowsForms10STATICSetText(95, 126, _AddressStateParse(StartState.Text));
                //SendTextToAnotherApplication.WindowsForms10EDITSetText(184, 125, (_AddressCityParse(StartCity.Text) + "  " + StartStreet.Text).Trim());
                ////하차지
                //SendTextToAnotherApplication.WindowsForms10STATICSetText(95, 151, _AddressStateParse(StopState.Text));
                //SendTextToAnotherApplication.WindowsForms10EDITSetText(184, 150, (_AddressCityParse(StopCity.Text) + "  " + StopStreet.Text).Trim());
                //대수
                SendTextToAnotherApplication.WindowsForms10EDITSetText(394, 175, CarCount.Text);
                ////추가정보
                SendTextToAnotherApplication.WindowsForms10EDITSetText(94, 200, Item.Text);
                //전화번호
                //  SendTextToAnotherApplication.WindowsForms10EDITSetText(94, 100, OrderPhoneNo.Text);
                //톤수
                SendTextToAnotherApplication.WindowsForms10EDITSetText(184, 175, ItemSize.Text);

                if (PayLocation.SelectedIndex == 1)
                {
                    //운송료,수수료,합계
                    SendTextToAnotherApplication.WindowsForms10EDITSetText(94, 225, Price1.Text);
                    SendTextToAnotherApplication.WindowsForms10EDITSetText(253, 225, "0");
                    SendTextToAnotherApplication.WindowsForms10EDITSetText(412, 225, Price1.Text);
                }
                //선착불
                else if (PayLocation.SelectedIndex == 2)
                {
                    Int64 Price = Convert.ToInt64(Price4.Text.Replace(",", "")) + Convert.ToInt64(Price5.Text.Replace(",", ""));
                    Int64 _Price = Price - Convert.ToInt64(Price6.Text.Replace(",", ""));

                    SendTextToAnotherApplication.WindowsForms10EDITSetText(94, 225, _Price.ToString());
                    SendTextToAnotherApplication.WindowsForms10EDITSetText(253, 225, Price6.Text);
                    //합계

                    SendTextToAnotherApplication.WindowsForms10EDITSetText(412, 225, Price.ToString());



                }


                ////상차방법
                //if (StartInfo1.Checked) //지게차
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(940, 99);
                //if (StartInfo2.Checked) //수작업
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(595, 99);
                //if (StartInfo3.Checked) //호이스트
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(871, 99);
                //if (StartInfo4.Checked) //크레인
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(733, 99);
                //if (StartInfo5.Checked) //컨베이어
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(802, 99);
                ////하차방법
                //if (StopInfo1.Checked) //지게차
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(940, 149);
                //if (StopInfo2.Checked) //수작업
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(595, 149);
                //if (StopInfo3.Checked) //호이스트
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(871, 149);
                //if (StopInfo4.Checked) //크레인
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(733, 149);
                //if (StopInfo5.Checked) //컨베이어
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(802, 149);
                //톤수,차종
                switch (CarSize.Text)
                {
                    case "0.3":
                    case "0.5":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 12);
                        break;
                    case "1":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 0);
                        break;
                    case "1.4":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 1);
                        break;
                    case "2.5":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 2);
                        break;
                    case "3.5":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 3);
                        break;
                    case "5":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 4);
                        break;
                    case "8":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 7);
                        break;
                    case "11":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 8);
                        break;
                    case "14":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 9);
                        break;
                    case "18":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 10);
                        break;
                    case "25":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 11);
                        break;
                    default:
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(95, 175, 13);
                        break;
                }
                switch (CarType.Text)
                {
                    case "카고":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 1);
                        break;
                    case "윙바디":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 2);
                        break;
                    case "카고+윙바디":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 3);
                        break;
                    case "트레일러":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 4);
                        break;
                    case "탑":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 6);
                        break;
                    case "다마스":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 7);
                        break;
                    case "라보":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 8);
                        break;
                    case "냉장탑":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 10);
                        break;
                    case "냉동탑":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 11);
                        break;
                    default:
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(245, 175, 0);
                        break;
                }
                ////상차일시
                //if (StartTimeType1.Checked || StartTimeType2.Checked)
                //{
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(594, 124);
                //    Thread.Sleep(500);
                //}
                //else if (StartTimeType3.Checked || !String.IsNullOrEmpty(StartTimeType4.Text))
                //{
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(634, 124);
                //    Thread.Sleep(500);
                //}
                //if (!String.IsNullOrEmpty(StartTimeType4.Text) && int.TryParse(StartTimeType4.Text, out int StartDay) && ((StartDay >= DateTime.Now.Day && StartDay <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)) || (StartDay < DateTime.Now.Day && StartDay <= DateTime.DaysInMonth(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month))))
                //{
                //    var Date = DateTime.Now;
                //    if (StartDay >= DateTime.Now.Day)
                //        Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, StartDay);
                //    else
                //        Date = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, StartDay);
                //    SendTextToAnotherApplication.WindowsForms10SysDateTimePick32SetText(676, 175, Date.ToString("yyyy-MM-dd"));
                //}
                ////하차일시
                //if (StopTimeType1.Checked || StopTimeType4.Checked)
                //{
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(594, 174);
                //    Thread.Sleep(500);
                //}
                //else if (StopTimeType2.Checked || StopTimeType3.Checked || !String.IsNullOrEmpty(StopTimeType5.Text))
                //{
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(634, 174);
                //    Thread.Sleep(500);
                //}
                //if (StopTimeType3.Checked)
                //{
                //    var Date = DateTime.Now.Date.AddDays(7);
                //    switch (Date.DayOfWeek)
                //    {
                //        case DayOfWeek.Tuesday:
                //            Date = Date.AddDays(-1);
                //            break;
                //        case DayOfWeek.Wednesday:
                //            Date = Date.AddDays(-2);
                //            break;
                //        case DayOfWeek.Thursday:
                //            Date = Date.AddDays(-3);
                //            break;
                //        case DayOfWeek.Friday:
                //            Date = Date.AddDays(-4);
                //            break;
                //        case DayOfWeek.Saturday:
                //            Date = Date.AddDays(-5);
                //            break;
                //        case DayOfWeek.Sunday:
                //            Date = Date.AddDays(-6);
                //            break;
                //        default:
                //            break;
                //    }
                //    SendTextToAnotherApplication.WindowsForms10SysDateTimePick32SetText(676, 175, Date.ToString("yyyy-MM-dd"));
                //}
                //else if (!String.IsNullOrEmpty(StopTimeType5.Text) && int.TryParse(StopTimeType5.Text, out int StopDay) && ((StopDay >= DateTime.Now.Day && StopDay <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)) || (StopDay < DateTime.Now.Day && StopDay <= DateTime.DaysInMonth(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month))))
                //{
                //    var Date = DateTime.Now;
                //    if (StopDay >= DateTime.Now.AddDays(1).Day)
                //        Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, StopDay);
                //    else
                //        Date = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, StopDay);
                //    SendTextToAnotherApplication.WindowsForms10SysDateTimePick32SetText(676, 175, Date.ToString("yyyy-MM-dd"));
                //}
                ////상차일시
                //if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
                //{
                //    int Hour = StartTimeHour2.SelectedIndex;
                //    if (StartTimeHour2.SelectedIndex == 12)
                //        Hour = 0;
                //    if (StartTimeHour1.SelectedIndex == 2)
                //        Hour += 12;
                //    SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(774, 124, Hour);
                //    if (StartTimeHalf.Checked)
                //    {
                //        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(824, 124, 3);
                //    }
                //}
                ////하차일시
                //if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                //{
                //    int Hour = StopTimeHour2.SelectedIndex;
                //    if (StopTimeHour2.SelectedIndex == 12)
                //        Hour = 0;
                //    if (StopTimeHour1.SelectedIndex == 2)
                //        Hour += 12;
                //    SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(774, 174, Hour);
                //    if (StopTimeHalf.Checked)
                //    {
                //        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(824, 174, 3);
                //    }
                //}
                ////혼적
                //if (Shared.Checked)
                //{
                //    SendTextToAnotherApplication.WindowsForms10BUTTONClick(448, 176);
                //}
                //운송비구분
                switch (PayLocation.Text)
                {
                    case "인수증":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(434, 200, 4);
                        break;
                    case "수수료확인":
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(434, 200, 5);
                        break;

                    default:
                        SendTextToAnotherApplication.SetSelectedIndexbyWindowsForms10COMBOBOX(434, 200, 0);
                        break;
                }
                Cursor = Cursors.Arrow;

            }

            //원콜
            else if (SelectApplication.SelectedIndex == 3)
            {
                Cursor = Cursors.WaitCursor;

                SendMessageToAnotherApplication SendTextToAnotherApplication = new SendMessageToAnotherApplication("OneCallShipper", "WindowsForms10.Window.8.app", true);
                SendTextToAnotherApplication.FindControlList();

                SendTextToAnotherApplication.MainKeyPressF1();
                

                //신규
                //   SendTextToAnotherApplication.MainKeyPressF1();
                Thread.Sleep(100);
                //상차지
                SendTextToAnotherApplication.WindowsForms10EDITSetText(221, 185, "테스트");

                // SendTextToAnotherApplication.WindowsForms10EDITSetText(474, 372, "테스트");
                Cursor = Cursors.Arrow;

            }
            //화물마당
            //else if (SelectApplication.SelectedIndex == 4)
            //{
            //    Cursor = Cursors.WaitCursor;

            //    SendMessageToAnotherApplication SendTextToAnotherApplication = new SendMessageToAnotherApplication("OneCallShipper", "WindowsForms10.Window.8.app", true);
            //    SendTextToAnotherApplication.FindControlList();

            //    SendTextToAnotherApplication.MainKeyPressF1();


            //    //신규
            //    //   SendTextToAnotherApplication.MainKeyPressF1();
            //    Thread.Sleep(100);
            //    //상차지
            //    SendTextToAnotherApplication.WindowsForms10EDITSetText(221, 185, "테스트");

            //    // SendTextToAnotherApplication.WindowsForms10EDITSetText(474, 372, "테스트");
            //    Cursor = Cursors.Arrow;

            //}
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
            if (_Value.Length > 2)
                R = _Value.Substring(0, _Value.Length - 1);
            return R;
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
            Price1.SelectAll();
        }

        private void Fee_Enter(object sender, EventArgs e)
        {
            Number_Enter(sender, e);
            Price3.SelectAll();
        }

        private void Price_Click(object sender, EventArgs e)
        {
            Price_Enter(sender, e);
        }

        private void Fee_Click(object sender, EventArgs e)
        {
            Fee_Enter(sender, e);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = "배차관리_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.카드페이_화물_배차_내역))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var ColumnIndexMap = new Dictionary<int, int>();
            var ColumnIndex = 0;
            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { ColumnImageA,CustomerSms,btnSms }.Contains(grid1.Columns[i])))
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }
            for (int i = 0; i < grid1.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = grid1[ColumnIndexMap[j], i].FormattedValue?.ToString();
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
        string SVC_RT;
        string SVC_MSG;
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

        private void PayLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //선착불
            if (PayLocation.SelectedIndex == 2)
            {
              
                Price4.TabIndex = 9;
                Price5.TabIndex = 10;
                Price6.TabIndex = 11;
                if (Price1.Enabled)
                {
                    Price1.Text = "0";
                    Price1.Enabled = false;
                }
                if (Price2.Enabled)
                {
                    Price2.Text = "0";
                    Price2.Enabled = false;

                   
                }
                if (Price3.Enabled)
                {
                    Price3.Text = "0";
                    Price3.Enabled = false;
                   
                }

                //주선공유일때 보관금 입력
                if (chkMyCarOrder.Checked == true)
                {
                    Price3.Enabled = true;
                }
                else
                {
                    Price3.Enabled = false;
                }


                if (!Price4.Enabled)
                {
                    Price4.Text = "0";
                    Price4.Enabled = true;
                }
                if (!Price5.Enabled)
                {
                    Price5.Text = "0";
                    Price5.Enabled = true;
                }
                if (!Price6.Enabled)
                {
                    Price6.Text = "0";
                    Price6.Enabled = true;
                }


            }
            else if (PayLocation.SelectedIndex == 1)
            {

              
                if (!Price1.Enabled)
                {
                    Price1.Text = "0";
                    Price1.Enabled = true;
                }
                if (!Price2.Enabled)
                {
                    Price2.Text = "0";
                    Price2.Enabled = true;
                }
                if (!Price3.Enabled)
                {
                    Price3.Text = "0";
                    Price3.Enabled = true;
                }
                if (Price4.Enabled)
                {
                    Price4.Text = "0";
                    Price4.Enabled = false;
                }
                if (Price5.Enabled)
                {
                    Price5.Text = "0";
                    Price5.Enabled = false;
                }
                if (Price6.Visible)
                {
                    Price6.Text = "0";
                    Price6.Enabled = false;
                }
            }
            else if (PayLocation.SelectedIndex == 3)
            {
                Price1.Text = "0";
                Price1.Enabled = true;

                Price2.Text = "0";
                Price2.Enabled = true;

                Price3.Text = "0";
                Price3.Enabled = true;

                Price4.Text = "0";
                Price4.Enabled = true;

                Price5.Text = "0";
                Price5.Enabled = true;

                Price6.Text = "0";
                Price6.Enabled = true;
             
            }
            
            else
            {

                Price1.Text = "0";
                Price1.Enabled = false;

                Price2.Text = "0";
                Price2.Enabled = false;

                Price3.Text = "0";
                Price3.Enabled = false;

                Price4.Text = "0";
                Price4.Enabled = false;

                Price5.Text = "0";
                Price5.Enabled = false;

                Price6.Text = "0";
                Price6.Enabled = false;

            }

            //if (PayLocation.SelectedIndex == 1)
            //{
            //    panel20.Enabled = false;
            //    panel21.Enabled = false;
            //}
            //else
            //{
            //    panel20.Enabled = true;
            //    panel21.Enabled = true;
            //}
        }

        private void Price2_Enter(object sender, EventArgs e)
        {
            Number_Enter(sender, e);
            Price2.SelectAll();
        }

        private void Price2_Click(object sender, EventArgs e)
        {
            Price2_Enter(sender, e);
        }

        private void NewDriver_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(NewCarYear.Text) || String.IsNullOrWhiteSpace(NewMobileNo.Text) || String.IsNullOrWhiteSpace(NewCarNo.Text)
                || !Regex.Match(NewMobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success || NewCarNo.Text.Length < 4 || !int.TryParse(NewCarNo.Text.Substring(NewCarNo.Text.Length - 4), out int t))
            {
                MessageBox.Show("차주이름/핸드폰번호/차량번호를 모두 채우셔야 합니다.");
                return;
            }
            int DriverId = _NewDriver();
            if (DriverId == 0)
            {
                MessageBox.Show("이미 가입되어 있는 차량번호입니다. 확인 부탁드립니다.");
                return;
            }


            DriverSelect.Items.Insert(0, NewCarYear.Text);
            DriverSelect.Items[0].SubItems.Add("");
            DriverSelect.Items[0].SubItems.Add("");
            DriverSelect.Items[0].SubItems.Add(NewMobileNo.Text);
            DriverSelect.Items[0].SubItems.Add(NewCarNo.Text);
            if (DriverPoint.Checked)
            {
                DriverSelect.Items[0].ForeColor = Color.Red;
            }
            DriverSelect.Items[0].Tag = DriverId;
            DriverSelect.Items[0].Selected = true;
            NewCarNo.Clear();
            NewMobileNo.Clear();
            NewCarYear.Clear();
            cmb_CarSize.SelectedIndex = 0;
            cmb_CarType.SelectedIndex = 0;
        }

        private void NewDriverAndAccept_Click(object sender, EventArgs e)
        {

            if (DriverSelect.SelectedItems.Count > 0)
            {
                _DriverSelected();
            }
            else
            {
                if (String.IsNullOrWhiteSpace(NewCarYear.Text) || String.IsNullOrWhiteSpace(NewMobileNo.Text) || String.IsNullOrWhiteSpace(NewCarNo.Text)
               || !Regex.Match(NewMobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success || NewCarNo.Text.Length < 4 || !int.TryParse(NewCarNo.Text.Substring(NewCarNo.Text.Length - 4), out int t))
                {
                    MessageBox.Show("차주이름/핸드폰번호/차량번호를 모두 채우셔야 합니다.");
                    return;
                }
                int DriverId = _NewDriver();
                if (DriverId == 0)
                {

                    DriverId = DriverModelList.Find(c => c.CarNo == NewCarNo.Text).DriverId;
                    //MessageBox.Show("이미 가입되어 있는 차량번호입니다. 확인 부탁드립니다.");
                    //return;
                }
                else
                {
                    //List<(int VAccountPoolId, String VBankName, String VAccount)> VAccountPoolList = new List<(int, string, string)>();
                    //Data.Connection((_Connection) =>
                    //{


                    //    ShareOrderDataSet.Instance.DriverPoints.Add(new DriverPoint
                    //    {
                    //        DriverId = DriverId,
                    //        CDate = DateTime.Now,
                    //        Amount = 3000,
                    //        OrderId = 0,
                    //        ClientId = DriverModelList.Find(c => c.DriverId == DriverId).CandidateId,
                    //        Remark = "무료충전 3000",
                    //        PointItem = "기본 "
                    //    });
                    //    ShareOrderDataSet.Instance.SaveChanges();
                    //});
                }

                if (PayLocation.SelectedIndex == 2 || PayLocation.SelectedIndex == 3 || PayLocation.SelectedIndex == 1)
                {
                    if (cmb_ReferralId.SelectedIndex == 0)
                    {
                        //MessageBox.Show("운송비가 선/착불,선/착불+인수증일 경우 위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        MessageBox.Show("위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cmb_ReferralId.Focus();
                        return;
                    }
                }


                var _Driver = DriverModelList.Find(c => c.DriverId == DriverId);
                if (DriverPoint.Checked)
                {
                    decimal.TryParse(Price3.Text.Replace(",", ""), out decimal _Fee);
                    if (_Driver.DriverPoint < _Fee)
                    {
                        if (MessageBox.Show("선택한 기사의 충전금이 부족합니다. 그래도 배차 하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            return;
                    }
                }

                Driver.Clear();
                Driver.Tag = _Driver;
                DriverName.Text = _Driver.CarYear;
                DriverPhoneNo.Text = _Driver.MobileNo;
                DriverCarNo.Text = _Driver.CarNo;

                if (grid1.CurrentCell != null && DataListSource.Current is Order Current && Current.DriverId == null && Current.TradeId == null && (Current.OrderStatus == 1 || Current.OrderStatus == 2))
                {
                    Current.DriverId = _Driver.DriverId;

                    Current.Driver = _Driver.CarYear;
                    Current.DriverCarModel = _Driver.Name;
                    Current.DriverCarNo = _Driver.CarNo;
                    Current.DriverPhoneNo = _Driver.MobileNo;
                    Current.OrderStatus = 3;
                    //Current.Price = int.Parse(Price1.Text.Replace(",", ""));
                    //Current.ClientPrice = int.Parse(Price2.Text.Replace(",", ""));

                    Current.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                    Current.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                    Current.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));
                    Current.Price = Current.TradePrice.Value;
                    Current.ClientPrice = Current.SalesPrice;

                    Current.AcceptTime = DateTime.Now;
                    Current.Wgubun = "PC";

                    Current.ClientId = LocalUser.Instance.LogInInformation.ClientId;
                    Current.OrdersAcceptId = LocalUser.Instance.LogInInformation.LoginId;
                    if (DriverPoint.Checked)
                    {
                        Current.DriverPoint = Current.ClientPrice - Current.Price;
                    }
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
                        ShareOrderDataSet.SaveChanges();
                    }
                    DataListSource.ResetBindings(false);
                    SetRowBackgroundColor();
                }
                DriverSelectContainer.Visible = false;
                NewCarNo.Clear();
                NewMobileNo.Clear();
                NewCarYear.Clear();
                cmb_CarSize.SelectedIndex = 0;
                cmb_CarType.SelectedIndex = 0;

            }



            //if (String.IsNullOrWhiteSpace(NewCarYear.Text) || String.IsNullOrWhiteSpace(NewMobileNo.Text) || String.IsNullOrWhiteSpace(NewCarNo.Text)
            //    || !Regex.Match(NewMobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success || NewCarNo.Text.Length < 4 || !int.TryParse(NewCarNo.Text.Substring(NewCarNo.Text.Length - 4), out int t))
            //{
            //    MessageBox.Show("차주이름/핸드폰번호/차량번호를 모두 채우셔야 합니다.");
            //    return;
            //}
            //int DriverId = _NewDriver();
            //if (DriverId == 0)
            //{

            //    DriverId = DriverModelList.Find(c => c.CarNo == NewCarNo.Text).DriverId;

            //}
            //else
            //{

            //    Data.Connection((_Connection) =>
            //    {


            //        ShareOrderDataSet.Instance.DriverPoints.Add(new DriverPoint
            //        {
            //            DriverId = DriverId,
            //            CDate = DateTime.Now,
            //            Amount = 3000,
            //            OrderId = 0,
            //            ClientId = DriverModelList.Find(c => c.DriverId == DriverId).CandidateId,
            //            Remark = "무료충전 3000",
            //            PointItem = "기본 "
            //        });
            //        ShareOrderDataSet.Instance.SaveChanges();
            //    });
            //}

            //if (PayLocation.SelectedIndex == 2 || PayLocation.SelectedIndex == 3 || PayLocation.SelectedIndex == 1)
            //{
            //    if (cmb_ReferralId.SelectedIndex == 0)
            //    {
            //        //MessageBox.Show("운송비가 선/착불,선/착불+인수증일 경우 위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        MessageBox.Show("위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        cmb_ReferralId.Focus();
            //        return;
            //    }
            //}


            //var _Driver = DriverModelList.Find(c => c.DriverId == DriverId);
            //if (DriverPoint.Checked)
            //{
            //    decimal.TryParse(Price3.Text.Replace(",", ""), out decimal _Fee);
            //    if (_Driver.DriverPoint < _Fee)
            //    {
            //        if (MessageBox.Show("선택한 기사의 충전금이 부족합니다. 그래도 배차 하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //            return;
            //    }
            //}

            //Driver.Clear();
            //Driver.Tag = _Driver;
            //DriverName.Text = _Driver.CarYear;
            //DriverPhoneNo.Text = _Driver.MobileNo;
            //DriverCarNo.Text = _Driver.CarNo;

            //if (grid1.CurrentCell != null && DataListSource.Current is Order Current && Current.DriverId == null && Current.TradeId == null && (Current.OrderStatus == 1 || Current.OrderStatus == 2))
            //{
            //    Current.DriverId = _Driver.DriverId;

            //    Current.Driver = _Driver.CarYear;
            //    Current.DriverCarModel = _Driver.Name;
            //    Current.DriverCarNo = _Driver.CarNo;
            //    Current.DriverPhoneNo = _Driver.MobileNo;
            //    Current.OrderStatus = 3;
            //    //Current.Price = int.Parse(Price1.Text.Replace(",", ""));
            //    //Current.ClientPrice = int.Parse(Price2.Text.Replace(",", ""));

            //    Current.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
            //    Current.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
            //    Current.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));
            //    Current.Price = Current.TradePrice.Value;
            //    Current.ClientPrice = Current.SalesPrice;

            //    Current.AcceptTime = DateTime.Now;
            //    Current.Wgubun = "PC";

            //    Current.ClientId = LocalUser.Instance.LogInInformation.ClientId;
            //    Current.OrdersAcceptId = LocalUser.Instance.LogInInformation.LoginId;
            //    if (DriverPoint.Checked)
            //    {
            //        Current.DriverPoint = Current.ClientPrice - Current.Price;
            //    }
            //    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            //    {
            //        ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
            //        ShareOrderDataSet.SaveChanges();
            //    }
            //    DataListSource.ResetBindings(false);
            //    SetRowBackgroundColor();
            //}
            //DriverSelectContainer.Visible = false;
            //NewCarNo.Clear();
            //NewMobileNo.Clear();
            //NewCarYear.Clear();
            //cmb_CarSize.SelectedIndex = 0;
            //cmb_CarType.SelectedIndex = 0;
        }

        private int _NewDriver()
        {
            DriverRepository mDriverRepository = new DriverRepository();
            if (mDriverRepository.GetDriver(NewCarNo.Text) == null)
            {
                int DriverId = mDriverRepository.CreateSmartDriver(NewCarYear.Text, NewMobileNo.Text, NewCarNo.Text,(int)cmb_CarSize.SelectedValue,(int)cmb_CarType.SelectedValue, out String LoginId, out String Password, 0);
                DriverModelList.Add(new DriverModel
                {
                    DriverId = DriverId,
                    CarNo = NewCarNo.Text,
                    MobileNo = NewMobileNo.Text,
                    CarYear = NewCarYear.Text,
                    Name = NewCarYear.Text,
                    CarSize =(int)cmb_CarSize.SelectedValue,
                    CarType =(int)cmb_CarType.SelectedValue,
                    DriverPoint = 0,
                });
                //String Message = $"차세로에 차량등록이 완료되었습니다.\r\n" +
                //    $"아래 절차에 따라 추가정보 입력, 회원가입을 하시기 바랍니다.\r\n" +
                //    $"회원에 가입하시면..\r\n" +
                //    $"전국화물조회,배차받기,내역조회,인수증,세금계산서발행,운송료조기수금 등의 업무를 차주 어플에서 편리하게 하실 수 있습니다.\r\n" +
                //    $"\r\n" +
                //    $"1. 차주 어플설치\r\n" +
                //    $"'Play스토어'에서 '차세로' 검색 후, 설치\r\n" +
                //    $"https://goo.gl/PD8MXj\r\n" +
                //    $"\r\n" +
                //    $"2. 로그인\r\n" +
                //    $"아이디: {LoginId}\r\n" +
                //    $"비밀번호: {Password}\r\n" +
                //    $"\r\n" +
                //    $"3. 정보입력\r\n" +
                //    $"위 아이디로 로그인 하신 후,\r\n" +
                //    $"①'내정보' 탭의 '기본사항'의 해당 정보 추가입력..\r\n" +
                //    $"②하단 이용약관 동의..\r\n" +
                //    $"③'첨부서류'의 '사업자등록증'과 '통장'을 촬영하여 등록..\r\n" +
                //    $"\r\n" +
                //    $"수고 하셨습니다.\r\n" +
                //    $"이제 회원가입이 완료되었습니다.\r\n" +
                //    $"\r\n" +
                //    $"회사명: {LocalUser.Instance.LogInInformation.Client.Name}\r\n" +
                //    $"문의처: {LocalUser.Instance.LogInInformation.Client.PhoneNo}\r\n";
                //em_mmt_tranTableAdapter.Insert(DateTime.Now, "차세로에 차량등록이 완료되었습니다.", Message, "028535111", "3", NewMobileNo.Text.Replace("-", ""),LocalUser.Instance.LogInInformation.ClientId.ToString());
                return DriverId;
            }
            return 0;
        }

        private int _NewCustomer()
        {
            CustomerRepository mCustomerRepository = new CustomerRepository();

            int CustomerId = mCustomerRepository._Insert(NewSangHo.Text, NewCMobileNo.Text, NewCPhoneNo.Text);
            CustomerModelList.Add(new CustomerModel
            {
                CustomerId = CustomerId,
                SangHo = NewSangHo.Text,
                PhoneNo = NewCPhoneNo.Text,
                MobileNo = NewMobileNo.Text,
            });


            return CustomerId;
        }

        private void NewMobileNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;
        }

        private void NewMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void NewMobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(NewMobileNo.Text))
            {
                var _S = NewMobileNo.Text.Replace("-", "").Replace(" ", "");
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
                NewMobileNo.Text = _S;
            }
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

        private void NewCMobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(NewCMobileNo.Text))
            {
                var _S = NewCMobileNo.Text.Replace("-", "").Replace(" ", "");
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
                NewCMobileNo.Text = _S;
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




            if (e.ColumnIndex == btnSms.Index)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Current != null)
                {
                    try
                    {
                        if (Current.OrderStatus != 3)
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

                        if (Current.PayLocation == 5)
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

            if (e.ColumnIndex == CustomerSms.Index)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Current != null)
                {
                    try
                    {
                        if (Current.OrderStatus != 3 || String.IsNullOrEmpty(Current.Customer))
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

                        if (Current.PayLocation == 5)
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

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current == null)
                return;
            var Current = DataListSource.Current as Order;
            if (Current.ClientId != LocalUser.Instance.LogInInformation.ClientId)
            {
                MessageBox.Show("타 회사의 화물은 수정 할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (String.IsNullOrEmpty(StartState.Text) || String.IsNullOrEmpty(StopState.Text)
 || String.IsNullOrEmpty(Price1.Text) || String.IsNullOrEmpty(Item.Text))
            {
                MessageBox.Show("삭제할 정보를 선택하십시오", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (Current.OrderStatus != 0)
            {
                MessageBox.Show("상태가 [취소]인 건만 삭제 할 수 있습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (Current.DriverId != null)
            {
                MessageBox.Show("상태가 [취소]인 건만 삭제 할 수 있습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
                //if (MessageBox.Show("배차 완료 된 건입니다. [배차 취소] 와 함께 [화물 취소]를 같이 진행하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                //    return;
                //Current.DriverModel = null;
                //Current.DriverId = null;
                //Current.AcceptTime = null;
                //Current.Driver = String.Empty;
                //Current.DriverCarModel = String.Empty;
                //Current.DriverCarNo = String.Empty;
                //Current.DriverPhoneNo = String.Empty;
                //Current.Wgubun = "";
            }
            //Current.OrderStatus = 0;
            //using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            //{
            //    ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Deleted;
            //    ShareOrderDataSet.SaveChanges();
            //}
            if (MessageBox.Show("삭제 하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "DELETE FROM Orders WHERE OrderId = @OrderId";
                    _Command.Parameters.AddWithValue("@OrderId", Current.OrderId);
                    _Command.ExecuteNonQuery();
                }


            });
            try
            {
                MessageBox.Show("삭제 완료되었습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Sort = false;
                _Search();
                //DataListSource.ResetBindings(true);
                //SetRowBackgroundColor();
            }
            catch
            {
                _Sort = false;
                _Search();
                //DataListSource.ResetBindings(true);
                //SetRowBackgroundColor();

            }
        }

        private void NewCustomer_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(NewSangHo.Text) || String.IsNullOrWhiteSpace(NewCMobileNo.Text) || String.IsNullOrWhiteSpace(NewCPhoneNo.Text)
                || !Regex.Match(NewCMobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success || NewCPhoneNo.Text.Length < 9)
            {
                MessageBox.Show("상호/핸드폰번호/전화번호를 모두 채우셔야 합니다.");
                return;
            }
            int CustomerId = _NewCustomer();

            try
            {
                //CustomerRepository mCustomerRepository = new CustomerRepository();
                //mCustomerRepository.ConnectCustomer(CustomerId);
            }
            catch (Exception ex)
            {
                //pnProgress.Visible = false;
                MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            CustomerSelect.Items.Insert(0, NewSangHo.Text);
            CustomerSelect.Items[0].SubItems.Add(NewCPhoneNo.Text);
            CustomerSelect.Items[0].SubItems.Add("");

            if (DriverPoint.Checked)
            {
                CustomerSelect.Items[0].ForeColor = Color.Red;
            }
            CustomerSelect.Items[0].Tag = CustomerId;
            CustomerSelect.Items[0].Selected = true;

            NewSangHo.Clear();
            NewCPhoneNo.Clear();
            NewCMobileNo.Clear();

        }
        bool MethodProcess = false;
        private void cmb_ReferralId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grid1.CurrentCell == null)
            {
                return;
            }


            if (PayLocation.SelectedIndex == 2 || PayLocation.SelectedIndex == 3 || PayLocation.SelectedIndex == 1)
            {
                if (cmb_ReferralId.SelectedIndex == 0)
                {
                    //MessageBox.Show("운송비가 선/착불,선/착불+인수증일 경우 위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("위탁사는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cmb_ReferralId.Focus();
                    return;
                }
            }


            var Current = DataListSource.Current as Order;

            
            if (grid1.CurrentCell != null)
            {
                if (!MethodProcess)
                {

                    Current.ReferralId = (int)cmb_ReferralId.SelectedValue;

                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
                        ShareOrderDataSet.SaveChanges();
                    }
                    DataListSource.ResetBindings(false);
                }

            }


        }

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

        private void rdoJusun_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdoJusun.Checked)
            //{
            //    txtJusun.Clear();
            //    txtJusun.Visible = true;
            //    cmb_ReferralId.Visible = false;
            //}
            //else
            //{
            //    txtJusun.Visible = false;
            //    cmb_ReferralId.Visible = true;
            //}
        }

        private void txtJusun_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (DataListSource.Current != null)
            //{
            //    var Current = DataListSource.Current as Order;
            //    if (Current.Agubun == 4)
            //        return;
            //}
            //if (txtJusun.Text.Length > 1)
            //{

            //    if (e.KeyChar == 13)
            //    {
            //        JusunSelect.Items.Clear();
            //        int ItemIndex = 0;
            //        SetCustomerList();
            //        foreach (var _Customer in CustomerModelList.Where(c => c.SangHo.Contains(txtJusun.Text) || c.PhoneNo.Replace("-", "").Contains(txtJusun.Text.Replace("-", ""))).Where(c => c.BizGubun == 3))
            //        {
            //            JusunSelect.Items.Add(_Customer.SangHo);
            //            JusunSelect.Items[ItemIndex].SubItems.Add(_Customer.PhoneNo);
            //            JusunSelect.Items[ItemIndex].SubItems.Add(_Customer.AddressState + " " + _Customer.AddressCity);
            //            JusunSelect.Items[ItemIndex].Tag = _Customer.CustomerId;
            //            ItemIndex++;
            //        }

            //        if (JusunSelect.Items.Count > 0)
            //        {
            //            JusunSelect.Visible = true;
            //            JusunSelect.Items[0].Selected = true;

            //            JusunSelect.Focus();
            //        }
            //        else
            //        {
            //            JusunSelect.Visible = false;
            //            txtJusun.Clear();
            //            txtJusun.Tag = null;
            //        }

            //        //JusunSelect.Visible = true;
            //        //JusunSelect.Focus();
            //    }
            //}
            //else
            //{
            //    if (e.KeyChar == 13)
            //    {

            //        JusunSelect.Items.Clear();
            //        //JusunSelect.Visible = true;
            //        //JusunSelect.Focus();

            //    }
            //}
        }

        private void txtJusun_Leave(object sender, EventArgs e)
        {
            //if (txtJusun.Tag != null)
            //{
            //    if (CustomerModelList.Any(c => c.CustomerId == (int)txtJusun.Tag))
            //    {
            //        txtJusun.Text = CustomerModelList.Find(c => c.CustomerId == (int)txtJusun.Tag).SangHo;
            //    }
            //}
        }

        private void JusunSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    if (JusunSelect.SelectedItems.Count > 0)
            //    {
            //        _JusunSelected();
            //    }
            //}
        }
        private void _JusunSelected()
        {
            //txtJusun.Clear();
            //var _Jusun = CustomerModelList.Find(c => c.CustomerId == (int)JusunSelect.SelectedItems[0].Tag);

            //txtJusun.Text = _Jusun.SangHo;

            //txtJusun.Tag = _Jusun.CustomerId;

            //JusunSelect.Visible = false;
            //txtJusun.Focus();


            //if (grid1.CurrentCell == null)
            //{
            //    return;
            //}

            //var Current = DataListSource.Current as Order;

            //if (grid1.CurrentCell != null)
            //{
            //    if (!MethodProcess)
            //    {

            //        Current.ReferralId = (int)txtJusun.Tag;

            //        using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            //        {
            //            ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
            //            ShareOrderDataSet.SaveChanges();
            //        }
            //        DataListSource.ResetBindings(false);
            //    }

            //}
        }

        private void JusunSelect_MouseClick(object sender, MouseEventArgs e)
        {

            //if (e.Button == MouseButtons.Left)
            //{
            //    foreach (ListViewItem nListViewItem in JusunSelect.Items)
            //    {
            //        if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
            //        {
            //            _JusunSelected();
            //        }
            //    }
            //}
        }

        private void cmbRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbRequest.SelectedIndex)
            {
                //내일
                case 0:
                    dtpRequestDate.Value = DateTime.Now.AddDays(1);
                    break;
                //모래
                case 1:
                    dtpRequestDate.Value = DateTime.Now.AddDays(2);
                    break;
                //3일후
                case 2:
                    dtpRequestDate.Value = DateTime.Now.AddDays(3);
                    break;
                //일주일후
                case 3:
                    dtpRequestDate.Value = DateTime.Now.AddDays(7);
                    break;
                //직접지정
                case 4:
                    dtpRequestDate.Value = DateTime.Now;
                    break;

            }
        }

        private void btnAfterList_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
            SearchDriverTrade.SelectedIndex = 0;
            SearchCustomerTax.SelectedIndex = 0;
            SearchText.Clear();
            _Sort = false;
            _Search(4);
        }

        private void OrderAcceptSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
            SearchDriverTrade.SelectedIndex = 0;
            SearchCustomerTax.SelectedIndex = 0;
            SearchText.Clear();
            _Sort = false;
            _Search(5);
        }

        private void ShareSearch_Click(object sender, EventArgs e)
        {
            SearchTextFilter.SelectedIndex = 0;
            SearchDriverTrade.SelectedIndex = 0;
            SearchCustomerTax.SelectedIndex = 0;
            SearchText.Clear();
            _Sort = false;
            _Search(7);
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
            if (e.ColumnIndex == ColumnTime.Index)
            {
                _SortGubun = "Time";
                _Search();

            }
            else if (e.ColumnIndex == ColumnClientId.Index)
            {
                _SortGubun = "ClientId";
                _Search();
            }
            else if (e.ColumnIndex == ColumnStart.Index)
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

        private void Driver_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                NewCarYear.Clear();
                NewMobileNo.Clear();
                NewCarNo.Clear();
                cmb_CarSize.SelectedIndex = 0;
                cmb_CarType.SelectedIndex = 0;

                if (Clipboard.GetData(DataFormats.Text) == null)
                {
                    return;
                }
                var clipboard = Clipboard.GetData(DataFormats.Text).ToString();

                string[] splited2 = clipboard.Replace(",","").Split(' ');
                if (splited2.Length == 3)
                {
                    string _CarYear = splited2[0];
                    if (Regex.Match(splited2[1], @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success)
                    {
                        NewCarNo.Text = splited2[2];
                        NewMobileNo.Text = splited2[1];

                        NewCarYear.Text = _CarYear;
                    }
                    else
                    {
                        NewCarNo.Text = splited2[1];
                        NewMobileNo.Text = splited2[2];

                        NewCarYear.Text = _CarYear;

                      
                    }
                    NewDriverAndAccept.Focus();
                    //  MessageBox.Show(_CarYear);


                }
                DriverSelect.Items.Clear();
                DriverSelectContainer.Visible = true;
                Driver.Clear();
                //DriverSelect.Focus();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (Customer.Tag != null)
            {


                FrmMN0301PrevPopup f = new FrmMN0301PrevPopup("W",Customer.Text);
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

                if(DataSource.Any())
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
                    Price1.Text = (DataSource.First().TradePrice ?? 0).ToString("N0");
                    Price2.Text = (DataSource.First().SalesPrice ?? 0).ToString("N0");
                    Price3.Text = (DataSource.First().AlterPrice ?? 0).ToString("N0");
                    Price4.Text = (DataSource.First().StartPrice ?? 0).ToString("N0");
                    Price5.Text = (DataSource.First().StopPrice ?? 0).ToString("N0");
                    Price6.Text = (DataSource.First().DriverPrice ?? 0).ToString("N0");

                    Item.Text = DataSource.First().Item;
                    Remark.Text = DataSource.First().Remark;


                    //if (DataSource.First().IsShared)
                    //{
                    //    Shared.Checked = true;
                    //    SharedItemLength.SelectedValue = DataSource.First().SharedItemLength ?? 0;
                    //    SharedItemSize.SelectedValue = DataSource.First().SharedItemSize ?? 0;
                    //}
                    //else
                    //{
                    //    NotShared.Checked = true;
                    //    SharedItemLength.SelectedIndex = 0;
                    //    SharedItemSize.SelectedIndex = 0;
                    //}


                    //Emergency.Checked = DataSource.First().Emergency == true;
                    //Round.Checked = DataSource.First().Round == true;
                    //Reservation.Checked = DataSource.First().Reservation == true;

                   // ClearStarTime_Click(null, null);



                    //if (DataSource.First().StartInfo == "지게차")
                    //{
                    //    StartInfo1.Checked = true;
                    //}
                    //else if (DataSource.First().StartInfo == "수작업")
                    //{
                    //    StartInfo2.Checked = true;
                    //}
                    //else if (DataSource.First().StartInfo == "호이스트")
                    //{
                    //    StartInfo3.Checked = true;
                    //}
                    //else if (DataSource.First().StartInfo == "크레인")
                    //{
                    //    StartInfo4.Checked = true;
                    //}
                    //else if (DataSource.First().StartInfo == "컨베이어")
                    //{
                    //    StartInfo5.Checked = true;
                    //}


                    //if (DataSource.First().StartTimeType == 1)
                    //{
                    //    StartTimeType1.Checked = true;
                    //}
                    //else if (DataSource.First().StartTimeType == 2)
                    //{
                    //    StartTimeType2.Checked = true;
                    //    if (DataSource.First().StartTimeHour > 0)
                    //    {
                    //        if (DataSource.First().StartTimeHour > 12)
                    //        {
                    //            StartTimeHour1.SelectedIndex = 2;
                    //            StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value - 12;
                    //        }
                    //        else
                    //        {
                    //            StartTimeHour1.SelectedIndex = 1;
                    //            StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value;
                    //        }
                    //        StartTimeHalf.Checked = DataSource.First().StartTimeHalf == true;
                    //        StartTimeETC.Checked = DataSource.First().StartTimeETC == true;
                    //    }
                    //}
                    //else if (DataSource.First().StartTimeType == 3)
                    //{
                    //    StartTimeType3.Checked = true;
                    //    if (DataSource.First().StartTimeHour > 0)
                    //    {
                    //        if (DataSource.First().StartTimeHour > 12)
                    //        {
                    //            StartTimeHour1.SelectedIndex = 2;
                    //            StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value - 12;
                    //        }
                    //        else
                    //        {
                    //            StartTimeHour1.SelectedIndex = 1;
                    //            StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value;
                    //        }
                    //        StartTimeHalf.Checked = DataSource.First().StartTimeHalf == true;
                    //        StartTimeETC.Checked = DataSource.First().StartTimeETC == true;
                    //    }
                    //}
                    //else if (DataSource.First().StartTimeType > 1000)
                    //{
                    //    StartTimeType4.Text = (DataSource.First().StartTimeType - 1000).ToString();
                    //    if (DataSource.First().StartTimeHour > 0)
                    //    {
                    //        if (DataSource.First().StartTimeHour > 12)
                    //        {
                    //            StartTimeHour1.SelectedIndex = 2;
                    //            StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value - 12;
                    //        }
                    //        else
                    //        {
                    //            StartTimeHour1.SelectedIndex = 1;
                    //            StartTimeHour2.SelectedIndex = DataSource.First().StartTimeHour.Value;
                    //        }
                    //        StartTimeHalf.Checked = DataSource.First().StartTimeHalf == true;
                    //        StartTimeETC.Checked = DataSource.First().StartTimeETC == true;
                    //    }
                    //}

                    ClearStopTime_Click(null, null);
                    //if (DataSource.First().StopInfo == "지게차")
                    //{
                    //    StopInfo1.Checked = true;
                    //}
                    //else if (DataSource.First().StopInfo == "수작업")
                    //{
                    //    StopInfo2.Checked = true;
                    //}
                    //else if (DataSource.First().StopInfo == "호이스트")
                    //{
                    //    StopInfo3.Checked = true;
                    //}
                    //else if (DataSource.First().StopInfo == "크레인")
                    //{
                    //    StopInfo4.Checked = true;
                    //}
                    //else if (DataSource.First().StopInfo == "컨베이어")
                    //{
                    //    StopInfo5.Checked = true;
                    //}
                    //if (DataSource.First().StopTimeType == 1)
                    //{
                    //    StopTimeType1.Checked = true;
                    //    if (DataSource.First().StopTimeHour > 0)
                    //    {
                    //        if (DataSource.First().StopTimeHour > 12)
                    //        {
                    //            StopTimeHour1.SelectedIndex = 2;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                    //        }
                    //        else
                    //        {
                    //            StopTimeHour1.SelectedIndex = 1;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                    //        }
                    //        StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                    //    }
                    //}
                    //else if (DataSource.First().StopTimeType == 2)
                    //{
                    //    StopTimeType2.Checked = true;
                    //    if (DataSource.First().StopTimeHour > 0)
                    //    {
                    //        if (DataSource.First().StopTimeHour > 12)
                    //        {
                    //            StopTimeHour1.SelectedIndex = 2;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                    //        }
                    //        else
                    //        {
                    //            StopTimeHour1.SelectedIndex = 1;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                    //        }
                    //        StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                    //    }
                    //}
                    //else if (DataSource.First().StopTimeType == 3)
                    //{
                    //    StopTimeType3.Checked = true;
                    //    if (DataSource.First().StopTimeHour > 0)
                    //    {
                    //        if (DataSource.First().StopTimeHour > 12)
                    //        {
                    //            StopTimeHour1.SelectedIndex = 2;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                    //        }
                    //        else
                    //        {
                    //            StopTimeHour1.SelectedIndex = 1;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                    //        }
                    //        StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                    //    }
                    //}
                    //else if (DataSource.First().StopTimeType == 4)
                    //{
                    //    StopTimeType4.Checked = true;
                    //}
                    //else if (DataSource.First().StopTimeType > 1000)
                    //{
                    //    StopTimeType4.Text = (DataSource.First().StopTimeType - 1000).ToString();
                    //    if (DataSource.First().StopTimeHour > 0)
                    //    {
                    //        if (DataSource.First().StopTimeHour > 12)
                    //        {
                    //            StopTimeHour1.SelectedIndex = 2;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value - 12;
                    //        }
                    //        else
                    //        {
                    //            StopTimeHour1.SelectedIndex = 1;
                    //            StopTimeHour2.SelectedIndex = DataSource.First().StopTimeHour.Value;
                    //        }
                    //        StopTimeHalf.Checked = DataSource.First().StopTimeHalf == true;
                    //    }
                    //}



                    OrderPhoneNo.Text = DataSource.First().OrderPhoneNo;
                    //StopPhoneNo.Text = DataSource.First().StopPhoneNo;
                    //StartPhoneNo.Text = DataSource.First().StartPhoneNo;
                    ItemSizeInclude.Checked = DataSource.First().ItemSizeInclude == true;
                    ItemSize.Text = DataSource.First().ItemSize;

                    DriverName.Text = DataSource.First().Driver;
                    DriverPhoneNo.Text = DataSource.First().DriverPhoneNo;
                    DriverCarNo.Text = DataSource.First().DriverCarNo;

                    if (DataSource.First().OrderStatus == 1 || DataSource.First().OrderStatus == 0 || DataSource.First().OrderStatus == 2)
                    {
                        Driver.Enabled = true;
                        // Customer.Enabled = true;
                    }
                    else
                    {
                        Driver.Enabled = false;
                        // Customer.Enabled = false;
                    }
                    HasTrade.Checked = DataSource.First().TradeId != null;
                    HasSales.Checked = DataSource.First().SalesManageId != null;
                    DriverPoint.Checked = DataSource.First().DriverPoint != null;

                    StartName.Text = DataSource.First().StartName;
                    StopName.Text = DataSource.First().StopName;
                    //StartMemo.Text = DataSource.First().StartMemo;
                    //StopMemo.Text = DataSource.First().StopMemo;

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
                    var Query = CustomerModelList.Where(c => c.CustomerId == DataSource.First().ReferralId).ToArray();
                    if (Query.Any())
                    {
                        if (Query.First().BizGubun == 3)
                        {
                            //rdoJusun.Checked = true;
                            //txtJusun.Tag = DataSource.First().ReferralId;
                            //txtJusun.Text = Query.First().SangHo;
                            //txtJusun.Visible = true;
                            cmb_ReferralId.Visible = false;
                        }
                        else
                        {
                            //rdoWe.Checked = true;
                            cmb_ReferralId.Visible = true;
                            cmb_ReferralId.SelectedValue = DataSource.First().ReferralId;

                            //txtJusun.Visible = false;
                        }

                    }
                    else
                    {
                        //rdoWe.Checked = true;
                        cmb_ReferralId.Visible = true;


                        //txtJusun.Visible = false;
                    }
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


                    //if (DataSource.First().StartMulti == true)
                    //{
                    //    StartMulti.Checked = true;
                    //}
                    //else
                    //{
                    //    StartMulti.Checked = false;
                    //}

                    //if (DataSource.First().StopMulti == true)
                    //{
                    //    StopMulti.Checked = true;
                    //}
                    //else
                    //{
                    //    StopMulti.Checked = false;
                    //}

                    CustomerManager.Text = DataSource.First().CustomerManager;

                    dtpRequestDate.Value = DateTime.Now;// DataSource.First().CreateTime;
                }

            }

        }

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            FindZipNew f = new Admin.FindZipNew();
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
                    if(Start.Text.Length > 30)
                    {
                        Start.Text = Start.Text.Substring(0, 29);
                    }
                    

                    StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
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
                    Start.Focus();
                }
            }
        }

        private void btnFindZip2_Click(object sender, EventArgs e)
        {
            FindZipNew f = new Admin.FindZipNew();
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
                    Stop.Focus();
                }
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


            PayLocation_SelectedIndexChanged(null, null);

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

            private void btnPause_Click(object sender, EventArgs e)
        {
            
            if (grid1.CurrentCell != null)
            {
               
                var Current = DataListSource.Current as Order;

                if (Current.OrderStatus != 1)
                    return;

                if (Current.OrderStatus == 1)

                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "Update Orders SET OrderStatus = 2 WHERE OrderId = @OrderId "; ;

                            _Command.Parameters.AddWithValue("@OrderId", Current.OrderId);

                            _Command.ExecuteNonQuery();
                        }
                    });

               
            }
            else {

                if (String.IsNullOrEmpty(StartState.Text) || String.IsNullOrEmpty(StopState.Text)
             || String.IsNullOrEmpty(Item.Text))
                {
                    MessageBox.Show("상하차지 운송료/추가정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (String.IsNullOrEmpty(Customer.Text) || Customer.Tag == null)
                {
                    MessageBox.Show("화주정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Customer.Focus();
                    return;
                }
                if (PayLocation.SelectedIndex == 0)
                {
                    MessageBox.Show("지불방식은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    PayLocation.Focus();
                    return;
                }

                if (LocalUser.Instance.LogInInformation.Client.CustomerPay)
                {
                    if (Customer.Tag == null)
                    {
                        MessageBox.Show("화주명을 선택하여 주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    var _Customer = CustomerModelList.Find(c => c.CustomerId == (int)Customer.Tag);
                    var _Price = int.Parse(Price1.Text.Replace(",", ""));
                    var _Fee = (decimal)(_Price * _Customer.Fee * 0.01m);
                    Price3.Text = _Fee.ToString("N0");
                    Price2.Text = (_Price + _Fee).ToString("N0");
                }
                int _ReferralId = 0;
                //if (rdoJusun.Checked)
                //{
                //    if (string.IsNullOrEmpty(txtJusun.Text))
                //    {
                //        _ReferralId = 0;
                //    }
                //    else
                //    {
                //        _ReferralId = (int)txtJusun.Tag;
                //    }
                //}
                //else
                //{
                    _ReferralId = (int)cmb_ReferralId.SelectedValue;
                //}
                int _DriverGrade = 0;
                _DriverGrade = (int)DriverGrade.SelectedValue;
                bool _MyCarOrder = false;
                if (chkMyCarOrder.Checked)
                {
                    _MyCarOrder = true;

                }
                else
                {
                    _MyCarOrder = false;

                }
                int CarCountValue = Math.Max(int.Parse(CarCount.Text), 1);
                for (int i = 0; i < CarCountValue; i++)
                {
                    Order nOrder = new Order
                    {
                        //CreateTime = Convert.ToDateTime(dtpRequestDate.Value.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")),
                        CreateTime = DateTime.Now,
                        //CreateTime = DateTime.Now,
                        ClientId = LocalUser.Instance.LogInInformation.ClientId,
                        StartState = StartState.Text,
                        StartCity = StartCity.Text,
                        StartStreet = StartStreet.Text,
                        StartDetail = Start.Text,
                        StopState = StopState.Text,
                        StopCity = StopCity.Text,
                        StopStreet = StopStreet.Text,
                        StopDetail = Stop.Text,
                        CarSize = (int)CarSize.SelectedValue,
                        CarType = (int)CarType.SelectedValue,
                        StopDateHelper = (int)StopDateHelper.SelectedValue,
                        CarCount = 1,
                        PayLocation = (int)PayLocation.SelectedValue,
                        Item = Item.Text,
                        Remark = Remark.Text,
                        ItemSize = ItemSize.Text,
                        ItemSizeInclude = ItemSizeInclude.Checked,
                        //IsShared = Shared.Checked,
                        //SharedItemLength = (int)SharedItemLength.SelectedValue,
                        //SharedItemSize = (int)SharedItemSize.SelectedValue,
                        //Emergency = Emergency.Checked,
                        //Round = Round.Checked,
                        //Reservation = Reservation.Checked,
                        Customer = Customer.Text,
                        OrderPhoneNo = OrderPhoneNo.Text,
                        //StopPhoneNo = StopPhoneNo.Text,
                        //StartPhoneNo = StartPhoneNo.Text,
                        StartDate = DateTime.Now.Date,
                        StartTime = DateTime.Now.Date,
                        StopTime = DateTime.Now.Date,
                        // AccountMemo = AccountMemo.Text,
                        StartName = StartName.Text,
                        StopName = StopName.Text,
                        //StartMemo = StartMemo.Text,
                        //StopMemo = StopMemo.Text,
                        RequestMemo = RequestMemo.Text,

                        ReferralId = _ReferralId,
                        DriverGrade = _DriverGrade,
                        MyCarOrder = _MyCarOrder,
                        OrderStatus = 2,
                    };
                    nOrder.TradePrice = 0;
                    nOrder.SalesPrice = 0;
                    nOrder.AlterPrice = 0;
                    nOrder.StartPrice = 0;
                    nOrder.StopPrice = 0;
                    nOrder.DriverPrice = 0;
                    nOrder.Price = 0;
                    nOrder.ClientPrice = 0;
                    //선착불
                    if (PayLocation.SelectedIndex == 2)
                    {
                        nOrder.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                        nOrder.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                        nOrder.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));
                        nOrder.Price = nOrder.StartPrice.Value + nOrder.StopPrice.Value;
                        nOrder.ClientPrice = nOrder.StartPrice.Value + nOrder.StopPrice.Value - nOrder.DriverPrice;
                    }
                    //인수증
                    else if (PayLocation.SelectedIndex == 1)
                    {
                        nOrder.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                        nOrder.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                        nOrder.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));
                        nOrder.Price = nOrder.TradePrice.Value;
                        nOrder.ClientPrice = nOrder.SalesPrice;
                    }
                    //인수증+선착불
                    else if (PayLocation.SelectedIndex == 3)
                    {
                        nOrder.StartPrice = int.Parse(Price4.Text.Replace(",", ""));
                        nOrder.StopPrice = int.Parse(Price5.Text.Replace(",", ""));
                        nOrder.DriverPrice = int.Parse(Price6.Text.Replace(",", ""));

                        nOrder.TradePrice = int.Parse(Price1.Text.Replace(",", ""));
                        nOrder.SalesPrice = int.Parse(Price2.Text.Replace(",", ""));
                        nOrder.AlterPrice = int.Parse(Price3.Text.Replace(",", ""));

                        nOrder.Price = nOrder.TradePrice.Value; //nOrder.StartPrice.Value + nOrder.StopPrice.Value + nOrder.TradePrice.Value;
                        nOrder.ClientPrice = nOrder.SalesPrice; //nOrder.StartPrice.Value + nOrder.StopPrice.Value - nOrder.DriverPrice + nOrder.SalesPrice;

                    }


                    //if (StartInfo1.Checked)
                    //    nOrder.StartInfo = StartInfo1.Text;
                    //else if (StartInfo2.Checked)
                    //    nOrder.StartInfo = StartInfo2.Text;
                    //else if (StartInfo3.Checked)
                    //    nOrder.StartInfo = StartInfo3.Text;
                    //else if (StartInfo4.Checked)
                    //    nOrder.StartInfo = StartInfo4.Text;
                    //else if (StartInfo5.Checked)
                    //    nOrder.StartInfo = StartInfo5.Text;
                    //string StartTime = "";
                    //if (StartTimeType1.Checked)
                    //{
                    //    nOrder.StartTimeType = 1;
                    //    nOrder.StartDate = DateTime.Now.Date;
                    //    nOrder.StartTime = DateTime.Now;
                    //    StartTime = "지금상/";
                    //}
                    //else if (StartTimeType2.Checked)
                    //{
                    //    nOrder.StartTimeType = 2;
                    //    nOrder.StartDate = DateTime.Now.Date;
                    //    nOrder.StartTime = DateTime.Now.Date;
                    //    if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
                    //    {
                    //        nOrder.StartTimeHour = StartTimeHour2.SelectedIndex + (StartTimeHour1.SelectedIndex - 1) * 12;
                    //        nOrder.StartTime = nOrder.StartTime.AddHours(nOrder.StartTimeHour.Value);
                    //        nOrder.StartTimeHalf = StartTimeHalf.Checked;
                    //        nOrder.StartTimeETC = StartTimeETC.Checked;
                    //        if (StartTimeHalf.Checked)
                    //        {
                    //            nOrder.StartTime = nOrder.StartTime.AddMinutes(30);
                    //        }
                    //    }
                    //    StartTime = "당상/";
                    //}
                    //else if (StartTimeType3.Checked)
                    //{
                    //    nOrder.StartTimeType = 3;
                    //    nOrder.StartDate = DateTime.Now.Date;
                    //    nOrder.StartTime = DateTime.Now.Date;
                    //    if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
                    //    {
                    //        nOrder.StartTimeHour = StartTimeHour2.SelectedIndex + (StartTimeHour1.SelectedIndex - 1) * 12;
                    //        nOrder.StartTime = nOrder.StartTime.AddHours(nOrder.StartTimeHour.Value);
                    //        nOrder.StartTimeHalf = StartTimeHalf.Checked;
                    //        nOrder.StartTimeETC = StartTimeETC.Checked;
                    //        if (StartTimeHalf.Checked)
                    //        {
                    //            nOrder.StartTime = nOrder.StartTime.AddMinutes(30);
                    //        }
                    //    }
                    //    StartTime = "내상/";
                    //}
                    //else if (!String.IsNullOrEmpty(StartTimeType4.Text))
                    //{
                    //    nOrder.StartTimeType = 1000 + int.Parse(StartTimeType4.Text);
                    //    nOrder.StartDate = DateTime.Now.Date;
                    //    nOrder.StartTime = DateTime.Now.Date;
                    //    if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
                    //    {
                    //        nOrder.StartTimeHour = StartTimeHour2.SelectedIndex + (StartTimeHour1.SelectedIndex - 1) * 12;
                    //        nOrder.StartTime = nOrder.StartTime.AddHours(nOrder.StartTimeHour.Value);
                    //        nOrder.StartTimeHalf = StartTimeHalf.Checked;
                    //        nOrder.StartTimeETC = StartTimeETC.Checked;
                    //        if (StartTimeHalf.Checked)
                    //        {
                    //            nOrder.StartTime = nOrder.StartTime.AddMinutes(30);
                    //        }
                    //    }
                    //    StartTime = $"{StartTimeType4.Text}일상/";
                    //}
                    //if (StopInfo1.Checked)
                    //    nOrder.StopInfo = StopInfo1.Text;
                    //else if (StopInfo2.Checked)
                    //    nOrder.StopInfo = StopInfo2.Text;
                    //else if (StopInfo3.Checked)
                    //    nOrder.StopInfo = StopInfo3.Text;
                    //else if (StopInfo4.Checked)
                    //    nOrder.StopInfo = StopInfo4.Text;
                    //else if (StopInfo5.Checked)
                    //    nOrder.StopInfo = StopInfo5.Text;
                    //string StopTime = "";
                    //if (StopTimeType1.Checked)
                    //{
                    //    nOrder.StopTimeType = 1;
                    //    nOrder.StopTime = DateTime.Now.Date;
                    //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                    //    {
                    //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
                    //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
                    //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
                    //        if (StopTimeHalf.Checked)
                    //        {
                    //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
                    //        }
                    //    }
                    //    StopTime = "당착";
                    //}
                    //else if (StopTimeType2.Checked)
                    //{
                    //    nOrder.StopTimeType = 2;
                    //    nOrder.StopTime = DateTime.Now.Date;
                    //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                    //    {
                    //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
                    //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
                    //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
                    //        if (StopTimeHalf.Checked)
                    //        {
                    //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
                    //        }
                    //    }
                    //    StopTime = "내착";
                    //}
                    //else if (StopTimeType3.Checked)
                    //{
                    //    nOrder.StopTimeType = 3;
                    //    nOrder.StopTime = DateTime.Now.Date;
                    //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                    //    {
                    //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
                    //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
                    //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
                    //        if (StopTimeHalf.Checked)
                    //        {
                    //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
                    //        }
                    //    }
                    //    StopTime = "월착";
                    //}
                    //else if (StopTimeType4.Checked)
                    //{
                    //    nOrder.StopTimeType = 4;
                    //    nOrder.StopTime = DateTime.Now.Date;
                    //    StopTime = "당착/내착";
                    //}
                    //else if (!String.IsNullOrEmpty(StopTimeType5.Text))
                    //{
                    //    nOrder.StopTimeType = 1000 + int.Parse(StopTimeType5.Text);
                    //    nOrder.StopTime = DateTime.Now.Date;
                    //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
                    //    {
                    //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
                    //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
                    //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
                    //        if (StopTimeHalf.Checked)
                    //        {
                    //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
                    //        }
                    //    }
                    //    StopTime = $"{StopTimeType5.Text}일착";
                    //}
                    if (String.IsNullOrEmpty(nOrder.OrderPhoneNo))
                    {
                        //string PhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo.Replace("-", "");
                        //nOrder.OrderPhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                        //  string PhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo.Replace("-", "");
                        nOrder.OrderPhoneNo = "";
                    }
                    if (Customer.Tag != null)
                    {
                        if (CustomerModelList.Where(c => c.CustomerId == (int)Customer.Tag).Any())
                        {
                            nOrder.CustomerId = (int)Customer.Tag;
                            nOrder.PointMethod = CustomerModelList.Find(c => c.CustomerId == nOrder.CustomerId).PointMethod;
                        }
                        else
                        {
                            nOrder.CustomerId = null;


                        }
                    }
                    if (DriverPoint.Checked)
                        nOrder.DriverPoint = 0;

                    //if(StartMulti.Checked)
                    //{
                    //    nOrder.StartMulti = true;
                    //}
                    //else
                    //{
                    //    nOrder.StartMulti = false;
                    //}

                    //if (StopMulti.Checked)
                    //{
                    //    nOrder.StopMulti = true;
                    //}
                    //else
                    //{
                    //    nOrder.StopMulti = false;
                    //}
                    nOrder.CustomerManager = CustomerManager.Text;
                    nOrder.OrdersLoginId = LocalUser.Instance.LogInInformation.LoginId;
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        ShareOrderDataSet.Orders.Add(nOrder);
                        ShareOrderDataSet.SaveChanges();
                    }
                    if (nOrder.CustomerId == null && !String.IsNullOrEmpty(nOrder.OrderPhoneNo) && !String.IsNullOrEmpty(nOrder.Customer))
                    {
                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT ContractId FROM Contracts WHERE PhoneNo = @PhoneNo";
                                _Command.Parameters.AddWithValue("@PhoneNo", nOrder.OrderPhoneNo);
                                if (_Command.ExecuteScalar() != null)
                                    return;
                            }
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "INSERT INTO Contracts (ClientId, PhoneNo, Name) VALUES (@ClientId, @PhoneNo, @Name)";
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                _Command.Parameters.AddWithValue("@PhoneNo", nOrder.OrderPhoneNo);
                                _Command.Parameters.AddWithValue("@Name", nOrder.Customer);
                                _Command.ExecuteNonQuery();
                            }
                        });
                    }
                    if (Driver.Tag != null)
                    {
                        //직배송
                        var _Driver = Driver.Tag as DriverModel;
                        nOrder.DriverId = _Driver.DriverId;
                        nOrder.Driver = _Driver.CarYear;
                        nOrder.DriverCarModel = _Driver.Name;
                        nOrder.DriverCarNo = _Driver.CarNo;
                        nOrder.DriverPhoneNo = _Driver.MobileNo;
                        nOrder.OrderStatus = 3;
                        nOrder.AcceptTime = DateTime.Now;
                        nOrder.Wgubun = "PC";
                        nOrder.OrdersAcceptId = LocalUser.Instance.LogInInformation.LoginId;
                        if (DriverPoint.Checked)
                        {
                            nOrder.DriverPoint = nOrder.ClientPrice - nOrder.Price;
                        }
                        using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                        {
                            ShareOrderDataSet.Entry(nOrder).State = System.Data.Entity.EntityState.Modified;
                            ShareOrderDataSet.SaveChanges();
                        }

                    }
                    else
                    {

                    }
                }
            }
            _Sort = false;
            _Search();
            _NewOrder();
            SetRowBackgroundColor();

        }

        private void rdb_Car_CheckedChanged(object sender, EventArgs e)
        {
            
            
          
            if(rdb_Car.Checked)
            {
                FrmMDI frmMDI = new FrmMDI();
                FrmMN0301 _Form = new FrmMN0301();
                _Form.MdiParent = this.MdiParent;

                _Form.Show();

                this.Close();
            }
            else if (rdb_Client.Checked)
            {
                FrmMDI frmMDI = new FrmMDI();
                FrmMN0301G _Form = new FrmMN0301G();
                
                _Form.MdiParent = this.MdiParent;

                _Form.Show();

                this.Close();
            }




          
        }

        private void CopyToApplication_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "자동입력";
            // this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.CopyToApplication, "① 화물정보를 입력 \r\n" +
                "② 타 사 배차 프로그램을 선택한 후,\r\n" +
                "③ “자동입력” 버튼을 누른다.");
        }

        private void SelectApplication_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "자동입력";
            // this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.SelectApplication, "① 화물정보를 입력 \r\n" +
                "② 타 사 배차 프로그램을 선택한 후,\r\n" +
                "③ “자동입력” 버튼을 누른다.");
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

            //bool _StartInfo1 = false;
            //bool _StartInfo2 = false;
            //bool _StartInfo3 = false;
            //bool _StartInfo4 = false;
            //bool _StartInfo5 = false;

            //if (StartInfo1.Checked)
            //{
            //    _StartInfo1 = true;
            //}
            //if (StartInfo2.Checked)
            //{
            //    _StartInfo2 = true;
            //}
            //if (StartInfo3.Checked)
            //{
            //    _StartInfo3 = true;
            //}
            //if (StartInfo4.Checked)
            //{
            //    _StartInfo4 = true;
            //}
            //if (StartInfo5.Checked)
            //{
            //    _StartInfo5 = true;
            //}

            //bool _StopInfo1 = false;
            //bool _StopInfo2 = false;
            //bool _StopInfo3 = false;
            //bool _StopInfo4 = false;
            //bool _StopInfo5 = false;


            //if (StopInfo1.Checked)
            //{
            //    _StopInfo1 = true;
            //}
            //if (StopInfo2.Checked)
            //{
            //    _StopInfo2 = true;
            //}
            //if (StopInfo3.Checked)
            //{
            //    _StopInfo3 = true;
            //}
            //if (StopInfo4.Checked)
            //{
            //    _StopInfo4 = true;
            //}
            //if (StopInfo5.Checked)
            //{
            //    _StopInfo5 = true;
            //}
            //bool _StartTimeType2 = false;
            //bool _StartTimeType3 = false;
           // var _StartTimeType4 = StartTimeType4.Text;
            //if (StartTimeType2.Checked)
            //{
            //    _StartTimeType2 = true;
            //}
            //if (StartTimeType3.Checked)
            //{
            //    _StartTimeType3 = true;
            //}
            //var _StartTimeHour1 = StartTimeHour1.Text;
            //var _StartTimeHour2 = StartTimeHour2.Text;
            //bool _StartTimeHalf = false;

            //if(StartTimeHalf.Checked)
            //{
            //    _StartTimeHalf = true;
            //}


            //bool _StopTimeType1 = false;
            //bool _StopTimeType2 = false;
            //var _StopTimeType5 = StopTimeType5.Text;
            //if (StopTimeType1.Checked)
            //{
            //    _StopTimeType1 = true;
            //}
            //if (StopTimeType2.Checked)
            //{
            //    _StopTimeType2 = true;
            //}
            //var _StopTimeHour1 = StopTimeHour1.Text;
            //var _StopTimeHour2 = StopTimeHour2.Text;
            //bool _StopTimeHalf = false;

            //if (StopTimeHalf.Checked)
            //{
            //    _StopTimeHalf = true;
            //}

            //var _StartPhoneNo = StartPhoneNo.Text;
            //var _StopPhoneNo = StopPhoneNo.Text;

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


            //StartInfo1.Checked = _StopInfo1;
            //StartInfo2.Checked = _StopInfo2;
            //StartInfo3.Checked = _StopInfo3;
            //StartInfo4.Checked = _StopInfo4;
            //StartInfo5.Checked = _StopInfo5;

            //StopInfo1.Checked = _StartInfo1;
            //StopInfo2.Checked = _StartInfo2;
            //StopInfo3.Checked = _StartInfo3;
            //StopInfo4.Checked = _StartInfo4;
            //StopInfo5.Checked = _StartInfo5;

            //StartTimeType2.Checked = _StopTimeType1;
            //StartTimeType3.Checked = _StopTimeType2;
            //StartTimeType4.Text = _StopTimeType5;
            //StartTimeHour1.Text = _StopTimeHour1;
            //StartTimeHour2.Text = _StopTimeHour2;
            //StartTimeHalf.Checked = _StopTimeHalf;
            //StartPhoneNo.Text = _StopPhoneNo;

            //StopTimeType1.Checked = _StartTimeType2;
            //StopTimeType2.Checked = _StartTimeType3;
            //StopTimeType5.Text = _StartTimeType4;
            //StopTimeHour1.Text = _StartTimeHour1;
            //StopTimeHour2.Text = _StartTimeHour2;
            //StopTimeHalf.Checked = _StartTimeHalf;
            //StopPhoneNo.Text = _StartPhoneNo;


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
                ColumnNumber,
                ColumnOrderStatus,
                ColumnTime,
               ColumnAcceptTime,
               // ColumnStartTime,
                ColumnClientId,
                btnSms,
                CustomerSms,
              //  Column1,
                lMsCntCol,
                ColumnStart,
                ColumnStop,
                ColumnItem,
                ColumnSalesPrice,
                ColumnTradePrice,
                ColumnAlterPrice,
                ColumnStartPrice,
                ColumnStopPrice,
                ColumnDriverPrice,
                ColumnPayLocation,
                ColumnCarSize,
                ColumnCarType,
                ColumnShared,
                ColumnItemInfo,
                ColumnWgubun,
                ColumnDriverSangHo,
                
                Column_DriverName,
                Column_DriverPhoneNo,
                Column_DriverCarNo,
                ColumnDriverCarSize,
                ColumnDriverCarType,
                ColumnDriverBizNo,
                ColumnTradeDate,
                //ColumnStopTime,
                ColumnPayDate,
                //ColumnImageA,
                SalesRequestDate,
                SalesPayDate

                );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);
        }

        private void StartTimeETC_CheckedChanged(object sender, EventArgs e)
        {
            SetRemark();
        }

        private void btnMyCarOrder_Click(object sender, EventArgs e)
        {
            //if (chkMyCarOrder.Checked)
            //{
            //    chkMyCarOrder.Checked = false;
                
            //}
            //else
            //{
                chkMyCarOrder.Checked = true;
            //}

            PayLocation_SelectedIndexChanged(null, null);
            if (DataListSource.Current != null)
            {
                var Current = DataListSource.Current as Order;
                if (Current.OrderStatus == 0 || Current.OrderStatus == 2)
                {
                    Current.OrderStatus = 1;
                   
                }
                UpdateOrder_Click(null, null);
            }
        }

        private void chkHideMyCarOrder_CheckedChanged(object sender, EventArgs e)
        {
          
            if (chkHideMyCarOrder.Checked == true)
            {
                HideMyCarOrder = true;
                
                ShareSearch.Enabled = true;
            }
            else
            {
                HideMyCarOrder = false;
                ShareSearch.Enabled = false;
            }


            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET HideMyCarOrder = @HideMyCarOrder" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@HideMyCarOrder", HideMyCarOrder);
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
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
                //전일
                case 1:
                    DateFilterBegin.Value = DateTime.Now.AddDays(-1);
                    DateFilterEnd.Value = DateTime.Now.AddDays(-1);
                    break;
                //금주
                case 2:
                    DateFilterBegin.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    DateFilterEnd.Value = DateTime.Now;
                    break;
                //금월
                case 3:
                    DateFilterBegin.Text = DateTime.Now.ToString("yyyy/MM/01");
                    DateFilterEnd.Value = DateTime.Now;
                    break;
                //전월
                case 4:
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
                panel1.Height = 320;
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
                if (!String.IsNullOrEmpty(Current.StartState)  && !String.IsNullOrEmpty(Current.StopState))
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
                    // System.Diagnostics.Process ie = System.Diagnostics.Process.Start("IExplore.exe", url);


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
            if (DriverSelect.SelectedItems.Count > 0)
            {
                var _Driver = DriverModelList.Find(c => c.DriverId == (int)DriverSelect.SelectedItems[0].Tag);

                if(_Driver != null)
                {
                    _Select(_Driver.CarNo);
                }
            }
        }
        private void _Select(string _DCarNo)
        {
            DriverSelectContainer.Visible = false;

            _CarNo = _DCarNo;



            FrmMN0203_CAROWNERMANAGE_FAULT_Add2 _Form = new FrmMN0203_CAROWNERMANAGE_FAULT_Add2(_CarNo);


            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;

            if (_Form.ShowDialog() == DialogResult.OK)
            {
                 
               
                Driver_KeyPress(null,new KeyPressEventArgs((char)13));
               
            }
            else
            {
                Driver_KeyPress(null, new KeyPressEventArgs((char)13));

            }


            //DialogResult = DialogResult.OK;
            //Close();
        }
        private void DriverSelect_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            foreach (ListViewItem nListViewItem in DriverSelect.Items)
            {
                if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                {
                    _DriverSelected();
                }
            }

        }

        private void DriverGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grid1.CurrentCell == null)
            {
                return;
            }





            var Current = DataListSource.Current as Order;


            if (grid1.CurrentCell != null)
            {
                if (!MethodProcess)
                {

                    Current.DriverGrade = (int)DriverGrade.SelectedValue;

                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        ShareOrderDataSet.Entry(Current).State = System.Data.Entity.EntityState.Modified;
                        ShareOrderDataSet.SaveChanges();
                    }
                    DataListSource.ResetBindings(false);
                }

            }
        }

        private void CallPass24OldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current == null)
                return;

            if (MessageBox.Show($"화주명 : {Customer.Text}\r\n상차지 : {StartName.Text}\r\n하차지 : {StopName.Text}\r\n화물명 : {Item.Text}\r\n운송비구분 : {PayLocation.Text}\r\n\r\n{SelectApplication.Text}으로\r\n{""}콜패스(자동입력)하시겠습니까?", "콜패스 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            //24시 구버전

            Cursor = Cursors.WaitCursor;
            SendMessageToAnotherApplication SendTextToAnotherApplication = new SendMessageToAnotherApplication("DFreighter24", new string[] { "TfrmMain" });
            SendTextToAnotherApplication.FindControlList();
            //신규
            SendTextToAnotherApplication.TBitBtnClick(17);
            Thread.Sleep(100);

            //상차지
            _SearchAddress(StartStreet.Text);
            if (_CoreList.Any())
            {

                string _StartState = "";
                string _StartCity = "";
                string _StartStreet = "";
                if (_CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).Any())
                {
                    var ss = _CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                    _StartState = ss[0];
                    _StartCity = ss[1];
                    _StartStreet = ss[2];

                }
                else
                {
                    var ss = _CoreList.Where(c => c.Address.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                    _StartState = ss[0];
                    _StartCity = ss[1];
                    _StartStreet = ss[2];

                }
                SendTextToAnotherApplication.SetText(15, _AddressStateParse(_StartState));
                SendTextToAnotherApplication.SetText(9, _AddressCityParse(_StartCity));
                SendTextToAnotherApplication.SetText(7, _StartStreet);
                SendTextToAnotherApplication.SetText(14, _AddressCityParse(_StartCity) + " " + _StartStreet);

                if (string.IsNullOrEmpty(StartStreet.Text))
                {
                    SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                    SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                    SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                    SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
                }
            }
            else
            {

                SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
            }

            //하차지
            _SearchAddress(StopStreet.Text);
            if (_CoreList.Any())
            {
                //var ss = _CoreList.Where(c=> c.Jibun.Contains(StopStreet.Text)).First().Jibun.Split(' ');
                //string _StopState = ss[0];
                //string _StopCity = ss[1];
                //string _StopStreet = ss[2];

                //SendTextToAnotherApplication.SetText(13, _AddressStateParse(_StopState));
                //SendTextToAnotherApplication.SetText(8, _AddressCityParse(_StopCity));
                //SendTextToAnotherApplication.SetText(6, _StopStreet);
                //SendTextToAnotherApplication.SetText(12, _AddressCityParse(_StopCity) + " " + _StopStreet);
                string _StopState = "";
                string _StopCity = "";
                string _StopStreet = "";
                if (_CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).Any())
                {
                    var ss = _CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).First().Jibun.Split(' ');

                    _StopState = ss[0];
                    _StopCity = ss[1];
                    _StopStreet = ss[2];

                }
                else
                {
                    var ss = _CoreList.Where(c => c.Address.Contains(StopStreet.Text)).First().Jibun.Split(' ');
                    _StopState = ss[0];
                    _StopCity = ss[1];
                    _StopStreet = ss[2];

                }
                SendTextToAnotherApplication.SetText(13, _AddressStateParse(_StopState));
                SendTextToAnotherApplication.SetText(8, _AddressCityParse(_StopCity));
                SendTextToAnotherApplication.SetText(6, _StopStreet);
                SendTextToAnotherApplication.SetText(12, _AddressCityParse(_StopCity) + " " + _StopStreet);
                if (string.IsNullOrEmpty(StopStreet.Text))
                {
                    SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                    SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                    SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                    SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);
                }
            }
            else
            {
                SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);

            }


            //대수
            SendTextToAnotherApplication.SetText(11, CarCount.Text);
            //추가정보
            //SendTextToAnotherApplication.SetText(10, Item.Text);
            ////상차일시
            //SendTextToAnotherApplication.SetText(5, StartTimeType4.Text);
            ////하차일시
            //SendTextToAnotherApplication.SetText(4, StopTimeType5.Text);
            //의뢰자명
            SendTextToAnotherApplication.SetText(3, LocalUser.Instance.LogInInformation.Client.Name);
            //전화번호
            SendTextToAnotherApplication.SetText(1, LocalUser.Instance.LogInInformation.Client.PhoneNo);
            //전화번호
            // SendTextToAnotherApplication.SetText(2, StopPhoneNo.Text);
            //의뢰자 사업자번호
            //String CustomerBizNo = String.Empty;
            //if (Customer.Tag is Int32 CustomerId)
            //{
            //    if (CustomerModelList.Any(c => c.CustomerId == CustomerId))
            //    {
            //        CustomerBizNo = CustomerModelList.First(c => c.CustomerId == CustomerId).BizNo;
            //    }
            //}
            SendTextToAnotherApplication.SetText(0, LocalUser.Instance.LogInInformation.Client.BizNo);
            //톤수
            SendTextToAnotherApplication.SetText(16, ItemSize.Text);
            //SendTextToAnotherApplication.SetText(10, ItemSize.Text);





            //운송료,수수료,합계
            //인수증
            if (PayLocation.SelectedIndex == 1)
            {
                //운송료
                SendTextToAnotherApplication.SetTextbyTwNumEdit(2, Price1.Text.Replace(",", ""));
                SendTextToAnotherApplication.SetTextbyTwNumEdit(1, "0");
                SendTextToAnotherApplication.SetTextbyTwNumEdit(0, Price1.Text.Replace(",", ""));
            }
            //선착불
            else if (PayLocation.SelectedIndex == 2)
            {

                //합계
                Int64 Price = Convert.ToInt64(Price4.Text.Replace(",", "")) + Convert.ToInt64(Price5.Text.Replace(",", ""));
                //운송료
                Int64 _Price = Price - Convert.ToInt64(Price6.Text.Replace(",", ""));
                //운송료
                SendTextToAnotherApplication.SetTextbyTwNumEdit(2, _Price.ToString());
                //수수료
                SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price6.Text.Replace(",", ""));

                //합계
                SendTextToAnotherApplication.SetTextbyTwNumEdit(0, Price.ToString());



                //Int64 Price = Convert.ToInt64(Price4.Text.Replace(",", "")) + Convert.ToInt64(Price5.Text.Replace(",", ""));
                ////운송료
                //SendTextToAnotherApplication.SetTextbyTwNumEdit(2, Price.ToString());
                ////수수료
                //SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price6.Text.Replace(",", ""));
                ////합계
                //Int64 PriceSum = Price + Convert.ToInt64(Price6.Text.Replace(",", ""));

                //SendTextToAnotherApplication.SetTextbyTwNumEdit(0, PriceSum.ToString());
            }

            //도착
            SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(8, StopDateHelper.SelectedIndex);

            //상차일시
            //if (StartTimeType1.Checked)
            //    SendTextToAnotherApplication.SetCheck(10);
            //if (StartTimeType2.Checked)
            //    SendTextToAnotherApplication.SetCheck(13);
            //if (StartTimeType3.Checked)
            //    SendTextToAnotherApplication.SetCheck(12);
            //if (StartTimeHalf.Checked)
            //    SendTextToAnotherApplication.SetCheck(11);
            ////하차일시
            //if (StopTimeType1.Checked)
            //    SendTextToAnotherApplication.SetCheck(4);
            //if (StopTimeType2.Checked)
            //    SendTextToAnotherApplication.SetCheck(3);
            //if (StopTimeType3.Checked)
            //    SendTextToAnotherApplication.SetCheck(1);
            //if (StopTimeType4.Checked)
            //    SendTextToAnotherApplication.SetCheck(0);
            //if (StopTimeHalf.Checked)
            //    SendTextToAnotherApplication.SetCheck(2);
            ////상차방법
            //if (StartInfo1.Checked)
            //    SendTextToAnotherApplication.SetCheck(18);
            //if (StartInfo2.Checked)
            //    SendTextToAnotherApplication.SetCheck(17);
            //if (StartInfo3.Checked)
            //    SendTextToAnotherApplication.SetCheck(16);
            //if (StartInfo4.Checked)
            //    SendTextToAnotherApplication.SetCheck(15);
            //if (StartInfo5.Checked)
            //    SendTextToAnotherApplication.SetCheck(14);
            ////하차방법
            //if (StopInfo1.Checked)
            //    SendTextToAnotherApplication.SetCheck(9);
            //if (StopInfo2.Checked)
            //    SendTextToAnotherApplication.SetCheck(8);
            //if (StopInfo3.Checked)
            //    SendTextToAnotherApplication.SetCheck(7);
            //if (StopInfo4.Checked)
            //    SendTextToAnotherApplication.SetCheck(6);
            //if (StopInfo5.Checked)
            //    SendTextToAnotherApplication.SetCheck(5);
            ////예약,왕복,긴급
            //if (Reservation.Checked)
            //    SendTextToAnotherApplication.SetCheck(19);
            //if (Round.Checked)
            //    SendTextToAnotherApplication.SetCheck(20);
            //if (Emergency.Checked)
            //    SendTextToAnotherApplication.SetCheck(21);
            ////독차,혼적
            //if (NotShared.Checked)
            //    SendTextToAnotherApplication.SetCheck(22);
            //if (Shared.Checked)
            //    SendTextToAnotherApplication.SetCheck(23);
            //~톤 이하
            if (ItemSizeInclude.Checked)
                SendTextToAnotherApplication.SetCheck(24);
            Thread.Sleep(1000);
            //톤수,차종
            SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBoxByKey(0, CarSize.SelectedIndex + 1, CarSize.Items.Count);
            // SendTextToAnotherApplication.SetText(0, CarSize.Text);


            Thread.Sleep(1000);
            if (CarType.SelectedIndex > 3)
            {
                SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(1, CarType.SelectedIndex - 1);
            }
            else
            {
                SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(1, CarType.SelectedIndex);
            }


            ////하차일시
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(3, StopTimeHour1.SelectedIndex);
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(2, StopTimeHour2.SelectedIndex);
            ////상차일시
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(5, StartTimeHour1.SelectedIndex);
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(4, StartTimeHour2.SelectedIndex);
            //if (Shared.Checked)
            //{
            //    //혼적1
            //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(2, SharedItemLength.Text, SharedItemLength.Items.Count);
            //    //혼적2
            //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(1, SharedItemSize.Text, SharedItemSize.Items.Count);
            //}
            //운송비구분
            if (PayLocation.SelectedIndex == 1 || PayLocation.SelectedIndex == 2)
            {
                SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(3, PayLocation.Text, PayLocation.Items.Count);
            }
            //추가정보
            SendTextToAnotherApplication.SetTextWithReturnKey(10, Item.Text);
            // SendTextToAnotherApplication.SetTextWithReturnKey(11, Item.Text);
            Cursor = Cursors.Arrow;

        }

        private void CallPass24NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataListSource.Current == null)
                return;

            if (MessageBox.Show($"화주명 : {Customer.Text}\r\n상차지 : {StartName.Text}\r\n하차지 : {StopName.Text}\r\n화물명 : {Item.Text}\r\n운송비구분 : {PayLocation.Text}\r\n\r\n{SelectApplication.Text}으로\r\n{""}콜패스(자동입력)하시겠습니까?", "콜패스 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;


            Cursor = Cursors.WaitCursor;
            SendMessageToAnotherApplication SendTextToAnotherApplication = new SendMessageToAnotherApplication("Cargo24", new string[] { "TfrmCargoMain" });
            SendTextToAnotherApplication.FindControlList();
            //신규
            SendTextToAnotherApplication.TBitBtnClick(17);
            Thread.Sleep(100);

            //상차지
            _SearchAddress(StartStreet.Text);
            if (_CoreList.Any())
            {
                string _StartState = "";
                string _StartCity = "";
                string _StartStreet = "";
                if (_CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).Any())
                {
                    var ss = _CoreList.Where(c => c.Jibun.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                    _StartState = ss[0];
                    _StartCity = ss[1];
                    _StartStreet = ss[2];

                }
                else
                {
                    var ss = _CoreList.Where(c => c.Address.Contains(StartStreet.Text)).First().Jibun.Split(' ');


                    _StartState = ss[0];
                    _StartCity = ss[1];
                    _StartStreet = ss[2];

                }
                SendTextToAnotherApplication.SetText(15, _AddressStateParse(_StartState));
                SendTextToAnotherApplication.SetText(9, _AddressCityParse(_StartCity));
                SendTextToAnotherApplication.SetText(7, _StartStreet);
                SendTextToAnotherApplication.SetText(14, _AddressCityParse(_StartCity) + " " + _StartStreet);



                if (string.IsNullOrEmpty(StartStreet.Text))
                {
                    SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                    SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                    SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                    SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
                }
            }
            else
            {

                SendTextToAnotherApplication.SetText(15, _AddressStateParse(StartState.Text));
                SendTextToAnotherApplication.SetText(9, _AddressCityParse(StartCity.Text));
                SendTextToAnotherApplication.SetText(7, StartStreet.Text);
                SendTextToAnotherApplication.SetText(14, _AddressCityParse(StartCity.Text) + " " + StartStreet.Text);
            }

            //하차지
            _SearchAddress(StopStreet.Text);
            if (_CoreList.Any())
            {

                string _StopState = "";
                string _StopCity = "";
                string _StopStreet = "";
                if (_CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).Any())
                {
                    var ss = _CoreList.Where(c => c.Jibun.Contains(StopStreet.Text)).First().Jibun.Split(' ');

                    _StopState = ss[0];
                    _StopCity = ss[1];
                    _StopStreet = ss[2];

                }
                else
                {
                    var ss = _CoreList.Where(c => c.Address.Contains(StopStreet.Text)).First().Jibun.Split(' ');
                    _StopState = ss[0];
                    _StopCity = ss[1];
                    _StopStreet = ss[2];

                }

                SendTextToAnotherApplication.SetText(13, _AddressStateParse(_StopState));
                SendTextToAnotherApplication.SetText(8, _AddressCityParse(_StopCity));
                SendTextToAnotherApplication.SetText(6, _StopStreet);
                SendTextToAnotherApplication.SetText(12, _AddressCityParse(_StopCity) + " " + _StopStreet);

                if (string.IsNullOrEmpty(StopStreet.Text))
                {
                    SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                    SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                    SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                    SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);
                }
            }
            else
            {
                SendTextToAnotherApplication.SetText(13, _AddressStateParse(StopState.Text));
                SendTextToAnotherApplication.SetText(8, _AddressCityParse(StopCity.Text));
                SendTextToAnotherApplication.SetText(6, StopStreet.Text);
                SendTextToAnotherApplication.SetText(12, _AddressCityParse(StopCity.Text) + " " + StopStreet.Text);

            }


            //대수
            SendTextToAnotherApplication.SetText(11, CarCount.Text);
            //추가정보
            //SendTextToAnotherApplication.SetText(10, Item.Text);
            ////상차일시
            //SendTextToAnotherApplication.SetText(5, StartTimeType4.Text);
            ////하차일시
            //SendTextToAnotherApplication.SetText(4, StopTimeType5.Text);
            //의뢰자명
            SendTextToAnotherApplication.SetText(3, LocalUser.Instance.LogInInformation.ClientName);
            //전화번호
            SendTextToAnotherApplication.SetText(1, LocalUser.Instance.LogInInformation.Client.PhoneNo);

            //주선사 사업자번호
            SendTextToAnotherApplication.SetText(0, LocalUser.Instance.LogInInformation.Client.BizNo);

            //전화번호
            // SendTextToAnotherApplication.SetText(2, StopPhoneNo.Text);
            //의뢰자 사업자번호
            //String CustomerBizNo = String.Empty;
            //if (Customer.Tag is Int32 CustomerId)
            //{
            //    if (CustomerModelList.Any(c => c.CustomerId == CustomerId))
            //    {
            //        CustomerBizNo = CustomerModelList.First(c => c.CustomerId == CustomerId).BizNo;
            //    }
            //}
            //  SendTextToAnotherApplication.SetText(0, CustomerBizNo);
            //톤수
            SendTextToAnotherApplication.SetText(16, ItemSize.Text);
            //SendTextToAnotherApplication.SetText(10, ItemSize.Text);

            //운송료,수수료,합계
            //인수증
            if (PayLocation.SelectedIndex == 1)
            {
                //운송료
                SendTextToAnotherApplication.SetTextbyTwNumEdit(0, Price1.Text.Replace(",", ""));
                //수수료
                SendTextToAnotherApplication.SetTextbyTwNumEdit(2, "0");
                //합계
                SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price1.Text.Replace(",", ""));

            }
            //선착불
            else if (PayLocation.SelectedIndex == 2)
            {
                //합계
                Int64 Price = Convert.ToInt64(Price4.Text.Replace(",", "")) + Convert.ToInt64(Price5.Text.Replace(",", ""));
                //운송료
                Int64 _Price = Price - Convert.ToInt64(Price6.Text.Replace(",", ""));
                //운송료
                SendTextToAnotherApplication.SetTextbyTwNumEdit(0, _Price.ToString());
                //수수료
                SendTextToAnotherApplication.SetTextbyTwNumEdit(2, Price6.Text.Replace(",", ""));

                //합계
                SendTextToAnotherApplication.SetTextbyTwNumEdit(1, Price.ToString());
                //Int64 PriceSum = Price + Convert.ToInt64(Price.ToString());


            }

            //도착
            SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(8, StopDateHelper.SelectedIndex);

            ////상차일시
            //if (StartTimeType1.Checked)
            //    SendTextToAnotherApplication.SetCheck(10);
            //if (StartTimeType2.Checked)
            //    SendTextToAnotherApplication.SetCheck(13);
            //if (StartTimeType3.Checked)
            //    SendTextToAnotherApplication.SetCheck(12);
            //if (StartTimeHalf.Checked)
            //    SendTextToAnotherApplication.SetCheck(11);
            ////하차일시
            //if (StopTimeType1.Checked)
            //    SendTextToAnotherApplication.SetCheck(4);
            //if (StopTimeType2.Checked)
            //    SendTextToAnotherApplication.SetCheck(3);
            //if (StopTimeType3.Checked)
            //    SendTextToAnotherApplication.SetCheck(1);
            //if (StopTimeType4.Checked)
            //    SendTextToAnotherApplication.SetCheck(0);
            //if (StopTimeHalf.Checked)
            //    SendTextToAnotherApplication.SetCheck(2);
            ////상차방법
            //if (StartInfo1.Checked)
            //    SendTextToAnotherApplication.SetCheck(18);
            //if (StartInfo2.Checked)
            //    SendTextToAnotherApplication.SetCheck(17);
            //if (StartInfo3.Checked)
            //    SendTextToAnotherApplication.SetCheck(16);
            //if (StartInfo4.Checked)
            //    SendTextToAnotherApplication.SetCheck(15);
            //if (StartInfo5.Checked)
            //    SendTextToAnotherApplication.SetCheck(14);
            ////하차방법
            //if (StopInfo1.Checked)
            //    SendTextToAnotherApplication.SetCheck(9);
            //if (StopInfo2.Checked)
            //    SendTextToAnotherApplication.SetCheck(8);
            //if (StopInfo3.Checked)
            //    SendTextToAnotherApplication.SetCheck(7);
            //if (StopInfo4.Checked)
            //    SendTextToAnotherApplication.SetCheck(6);
            //if (StopInfo5.Checked)
            //    SendTextToAnotherApplication.SetCheck(5);
            ////예약,왕복,긴급
            //if (Reservation.Checked)
            //    SendTextToAnotherApplication.SetCheck(19);
            //if (Round.Checked)
            //    SendTextToAnotherApplication.SetCheck(20);
            //if (Emergency.Checked)
            //    SendTextToAnotherApplication.SetCheck(21);
            ////독차,혼적
            //if (NotShared.Checked)
            //    SendTextToAnotherApplication.SetCheck(22);
            //if (Shared.Checked)
            //    SendTextToAnotherApplication.SetCheck(23);
            //~톤 이하
            if (ItemSizeInclude.Checked)
                SendTextToAnotherApplication.SetCheck(24);



            Thread.Sleep(1000);


            //톤수,차종
            //switch(CarSize.Text)
            // {
            //     case "0.3":
            //         SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, 2);

            //         break;
            //     case "0.5":
            //         SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, 3);

            //         break;
            // }
            // SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, CarSize.SelectedIndex + 2);
            SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBoxByKey(1, CarSize.SelectedIndex + 1, CarSize.Items.Count + 1);

            // SendTextToAnotherApplication.SetText(0, CarSize.Text);


            Thread.Sleep(1000);
            //if (CarType.SelectedIndex > 3)
            //{
            //    SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, CarType.SelectedIndex - 1);
            //}
            //else
            //{
            //    SendTextToAnotherApplication.SetSelectedIndexbyTRzComboBox(0, CarType.SelectedIndex);
            //}
            if (CarType.Text == "트레일러" || CarType.Text == "카고+윙바디")
            {
                SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, "차종확인", CarType.Items.Count + 2);

            }
            else
            {


                switch (CarSize.Text)
                {
                    //31

                    case "1":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;

                    case "1.4":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "2.5":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "3.5":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;

                    case "5":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "8":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "11":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "14":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "15":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "18":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;
                    case "25":
                        SendTextToAnotherApplication.SetSelectedIndexbyTRzCombo(0, CarType.Text, CarType.Items.Count + 2);
                        break;

                }
            }


            ////하차일시
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(3, StopTimeHour1.SelectedIndex);
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(2, StopTimeHour2.SelectedIndex);
            ////상차일시
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(5, StartTimeHour1.SelectedIndex);
            //SendTextToAnotherApplication.SetSelectedIndexbyTComboBox(4, StartTimeHour2.SelectedIndex);
            //if (Shared.Checked)
            //{
            //    //혼적1
            //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(2, SharedItemLength.Text, SharedItemLength.Items.Count);
            //    //혼적2
            //    SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(1, SharedItemSize.Text, SharedItemSize.Items.Count);
            //}
            //운송비구분
            if (PayLocation.SelectedIndex == 1 || PayLocation.SelectedIndex == 2)
            {
                SendTextToAnotherApplication.SetSelectedIndexbyTwCombo(3, PayLocation.Text, PayLocation.Items.Count);
            }
            //추가정보
            SendTextToAnotherApplication.SetTextWithReturnKey(10, Item.Text);


            Cursor = Cursors.Arrow;
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

        private void DriverSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DriverSelect.SelectedItems.Count > 0)
            {
                var _Driver = DriverModelList.Find(c => c.DriverId == (int)DriverSelect.SelectedItems[0].Tag);

                NewCarYear.Text = _Driver.CarYear;
                NewMobileNo.Text = _Driver.MobileNo;
                NewCarNo.Text = _Driver.CarNo;
                cmb_CarSize.SelectedValue = _Driver.CarSize;
                cmb_CarType.SelectedValue = _Driver.CarType;
            }
        }
    }

    //public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    //{
    //    public DataGridViewDisableButtonColumn()
    //    {
    //        this.CellTemplate = new DataGridViewDisableButtonCell();
    //    }
    //}

    //public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    //{
    //    private bool enabledValue;
    //    public bool Enabled
    //    {
    //        get
    //        {
    //            return enabledValue;
    //        }
    //        set
    //        {
    //            enabledValue = value;
    //        }
    //    }

    //    // Override the Clone method so that the Enabled property is copied.
    //    public override object Clone()
    //    {
    //        DataGridViewDisableButtonCell cell =
    //            (DataGridViewDisableButtonCell)base.Clone();
    //        cell.Enabled = this.Enabled;
    //        return cell;
    //    }

    //    // By default, enable the button cell.
    //    public DataGridViewDisableButtonCell()
    //    {
    //        this.enabledValue = true;
    //    }

    //    protected override void Paint(Graphics graphics,
    //        Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
    //        DataGridViewElementStates elementState, object value,
    //        object formattedValue, string errorText,
    //        DataGridViewCellStyle cellStyle,
    //        DataGridViewAdvancedBorderStyle advancedBorderStyle,
    //        DataGridViewPaintParts paintParts)
    //    {
    //        // The button cell is disabled, so paint the border,  
    //        // background, and disabled button for the cell.
    //        if (!this.enabledValue)
    //        {
    //            // Draw the cell background, if specified.
    //            if ((paintParts & DataGridViewPaintParts.Background) ==
    //                DataGridViewPaintParts.Background)
    //            {
    //                SolidBrush cellBackground =
    //                    new SolidBrush(cellStyle.BackColor);
    //                graphics.FillRectangle(cellBackground, cellBounds);
    //                cellBackground.Dispose();
    //            }

    //            // Draw the cell borders, if specified.
    //            if ((paintParts & DataGridViewPaintParts.Border) ==
    //                DataGridViewPaintParts.Border)
    //            {
    //                PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
    //                    advancedBorderStyle);
    //            }

    //            // Calculate the area in which to draw the button.
    //            Rectangle buttonArea = cellBounds;
    //            Rectangle buttonAdjustment =
    //                this.BorderWidths(advancedBorderStyle);
    //            buttonArea.X += buttonAdjustment.X;
    //            buttonArea.Y += buttonAdjustment.Y;
    //            buttonArea.Height -= buttonAdjustment.Height;
    //            buttonArea.Width -= buttonAdjustment.Width;

    //            // Draw the disabled button.                
    //            ButtonRenderer.DrawButton(graphics, buttonArea, PushButtonState.Disabled);

    //            // Draw the disabled button text. 
    //            if (this.FormattedValue is String)
    //            {
    //                TextRenderer.DrawText(graphics,
    //                    (string)this.FormattedValue,
    //                    this.DataGridView.Font,
    //                    buttonArea, SystemColors.GrayText);
    //            }
    //        }
    //        else
    //        {
    //            // The button cell is enabled, so let the base class 
    //            // handle the painting.
    //            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
    //                elementState, value, formattedValue, errorText,
    //                cellStyle, advancedBorderStyle, paintParts);
    //        }
    //    }
    //}
    class ClientListViewModelDefault
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
    }
    class ModelDefalut
    {
        public String Zip { get; set; }
        public String Address { get; set; }
        public String Jibun { get; set; }
    }
   
}
