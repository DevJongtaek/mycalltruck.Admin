using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static mycalltruck.Admin.CMDataSet;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301G : Form
    {
        private bool ValidateIgnore = false;
        private bool IsCurrentNull = false;
        bool HasLoaded = false;
       
        int GridIndex = 0;

        DateTimePicker dtp = new DateTimePicker();
        ContextMenuStrip m = new ContextMenuStrip();


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

                    NewOrderG.Enabled = false;
                    SaveOrderG.Enabled = false;
                    UpOrderG.Enabled = false;

                    DeleteOrderG.Enabled = false;


                    DataList.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }


        //Action LoadAction = null;
        //0.용차
        //1.지입
        int OrderGubun = 0;
        public FrmMN0301G()
        {
            InitializeComponent();

           
          
            DateFilterBegin.Value = DateTime.Now.Date;
            DateFilterEnd.Value = DateTime.Now.Date;
            dtpRequestDate.Value = DateTime.Now;
            SearChOrderGubun.SelectedIndex = 0;
            SearchTextFilter.SelectedIndex = 0;

            SetCustomerList();
            SetStaticOptions();
            SetDriverList();
          
            InitCustomerStartTable();
            SearChDateGubun.SelectedIndex = 0;
            m.Items.Add("배차일자수정");
            m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
            dtp.CloseUp += new EventHandler(dtp_CloseUp);
            dtp.ValueChanged += new EventHandler(dtp_OnTextChange);

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }



            DateFilterBegin.ValueChanged += (c, d) =>
            { 
                _Search();
            };
            DateFilterEnd.ValueChanged += (c, d) =>
            {
                _Search();
            };
            
        }

        private void FrmMN0301G_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'orderDataSet.OrderGs' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.orderGsTableAdapter.Fill(this.orderDataSet.OrderGs);
            LocalUser.Instance.LogInInformation.LoadClient();

            OrderGubun = LocalUser.Instance.LogInInformation.Client.OrderGubun;
            DataList.Controls.Add(dtp);
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.Visible = false;
            SetMouseClick(this);
            _Search();

            HasLoaded = true;

            



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
            public string Ceo { get; set; }
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
                    _Command.CommandText = $"SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo, Customers.PointMethod, Customers.Fee, Customers.AddressState, Customers.AddressCity, Customers.AddressDetail, Customers.BizNo,Customers.MobileNo,ISNULL(CustomerManager.ManagerName,''),BizGubun,ISNULL(Customers.Ceo,N'') FROM Customers " +
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
                                Ceo = _Reader.GetString(12),
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
                    _Command.CommandText = $"SELECT Distinct Drivers.DriverId, CEO, Name, CarNo, MobileNo, CarType, CarSize, CarYear" +               
                        $" FROM Drivers JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId WHERE Drivers.ServiceState != 5 AND DriverInstances.ClientId = {LocalUser.Instance.LogInInformation.ClientId} ";
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

                            });
                        }
                    }
                }
                _Connection.Close();
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




        }
        private void InitCustomerStartTable()
        {
            _CustomerStartTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT  idx, StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime FROM CustomerStartManage ";

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

                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        private void CustomerStartData(int SCustomerId)
        {

            customerStartManageDataSet.CustomerStartManage.Clear();

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText =

                      @"SELECT  idx, StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,Gubun FROM(
SELECT 0 as idx,AddressState as StartState, AddressCity as StartCity,'' as StartStreet, AddressDetail as StartDetail,AddressState +' '+ AddressCity as  StartName,PhoneNo as StartPhoneNo,ClientId,CustomerId,CreateTime,'본사' as Gubun FROM Customers    
UNION  
SELECT  idx, StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,'추가' as Gubun FROM CustomerStartManage)A
                        where CustomerId = @CustomerId
                       
                        order by CreateTime DESC";


                _Command.Parameters.AddWithValue("@CustomerId", SCustomerId);
                using (SqlDataReader _Reader = _Command.ExecuteReader())
                {
                    customerStartManageDataSet.CustomerStartManage.Load(_Reader);
                }
            }
        }

        private void _SetStaticOption(ComboBox mComboBox, String Div)
        {
            mComboBox.DisplayMember = "Name";
            mComboBox.ValueMember = "Value";
            mComboBox.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == Div).ToList());
            mComboBox.SelectedIndex = 0;
        }

        private void SetMouseClick(Control mControl)
        {
            foreach (Control nControl in mControl.Controls)
            {
                if (nControl == StopCustomerSelect || nControl == Stop1stCustomerSelect || nControl == Stop2stCustomerSelect || nControl == Stop3stCustomerSelect || nControl == Stop4stCustomerSelect)
                {
                    continue;
                }
                nControl.MouseClick += NControl_MouseClick;
                SetMouseClick(nControl);
            }
        }

        private void NControl_MouseClick(object sender, MouseEventArgs e)
        {
            StopCustomerSelect.Visible = false;
            Stop1stCustomerSelect.Visible = false;
            Stop2stCustomerSelect.Visible = false;
            Stop3stCustomerSelect.Visible = false;
            Stop4stCustomerSelect.Visible = false;

        }


        private void initCmbCustomer()
        {
            Dictionary<int, string> DCustomer = new Dictionary<int, string>();
          
            var CustomerDataSource = CustomerModelList.Where(c => c.BizGubun == 1).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).OrderBy(c=> c.SangHo).ToArray();

            DCustomer.Add(0,"화주선택");

            foreach (var item in CustomerDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }

            Customer.DataSource = new BindingSource(DCustomer, null);
            Customer.DisplayMember = "Value";
            Customer.ValueMember = "Key";
            Customer.SelectedValue = 0;


            Dictionary<int, string> DStartCid = new Dictionary<int, string>();
            var StartCidDataSource = CustomerModelList.Where(c => c.BizGubun == 5 ).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();

            DStartCid.Add(0, "도착지");

            foreach (var item in StartCidDataSource)
            {
                DStartCid.Add(item.CustomerId, item.SangHo);
            }

            //StartCid.DataSource = new BindingSource(DStartCid, null);
            //StartCid.DisplayMember = "Value";
            //StartCid.ValueMember = "Key";
            //StartCid.SelectedValue = 0;


            //StopCustomerId.DataSource = new BindingSource(DStartCid, null);
            //StopCustomerId.DisplayMember = "Value";
            //StopCustomerId.ValueMember = "Key";
            //StopCustomerId.SelectedValue = 0;

            //Stop1stCustomer.DataSource = new BindingSource(DStartCid, null);
            //Stop1stCustomer.DisplayMember = "Value";
            //Stop1stCustomer.ValueMember = "Key";
            //Stop1stCustomer.SelectedValue = 0;

            //Stop2stCustomer.DataSource = new BindingSource(DStartCid, null);
            //Stop2stCustomer.DisplayMember = "Value";
            //Stop2stCustomer.ValueMember = "Key";
            //Stop2stCustomer.SelectedValue = 0;

            //Stop3stCustomer.DataSource = new BindingSource(DStartCid, null);
            //Stop3stCustomer.DisplayMember = "Value";
            //Stop3stCustomer.ValueMember = "Key";
            //Stop3stCustomer.SelectedValue = 0;

            //Stop4stCustomer.DataSource = new BindingSource(DStartCid, null);
            //Stop4stCustomer.DisplayMember = "Value";
            //Stop4stCustomer.ValueMember = "Key";
            //Stop4stCustomer.SelectedValue = 0;




        }
        private void SetStaticOptions()
        {
            //CarSize.DisplayMember = "Name";
            //CarSize.ValueMember = "Value";
            //CarType.DisplayMember = "Name";
            //CarType.ValueMember = "Value";
            //CarSize.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value != 0 && c.Value != 99).ToList());
            //CarSize.SelectedValue = 1;
            _SetStaticOption(StopGubun, "StopGubun");

            _SetStaticOption(StopDayGubun, "StopTimeType");

            _SetStaticOption(StartInGubun, "RunGubun");

            //PayLocation.DisplayMember = "Name";
            //PayLocation.ValueMember = "Value";
            //PayLocation.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "PayLocation" && c.Value != 3).ToList());
            //PayLocation.SelectedIndex = 0;

            Dictionary<int, String> DriverIdDataSource = new Dictionary<int, string>();
            DriverIdDataSource.Add(0, "선택");
            BaseDataSet.DriversDataTable T = new BaseDataSet.DriversDataTable();
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(T);
            foreach (var driverRow in T.OrderBy(c => c.CarYear))
            {
                //DriverIdDataSource.Add(driverRow.DriverId, $"{driverRow.Name}[{driverRow.CarYear}] ({driverRow.CarNo})");
                DriverIdDataSource.Add(driverRow.DriverId, driverRow.CarYear);
            }
            Driver.DataSource = DriverIdDataSource.ToList();
            Driver.ValueMember = "Key";
            Driver.DisplayMember = "Value";
            Driver.SelectedIndex = 0;

            initCmbCustomer();



        }

        private void _Search()
        {
            ValidateIgnore = true;
            DataList.AutoGenerateColumns = false;
            var Now = DateFilterBegin.Value.Date;
            
            var Tommorow = DateFilterEnd.Value.AddDays(1).Date;
            List<OrderG> DataSource = new List<OrderG>();

            using (ShareOrderGDataSet ShareOrderGDataSet = new ShareOrderGDataSet())
            {
                DataSource = ShareOrderGDataSet.OrderGs.Include("DriverModel").Where(c=> c.ClientId == LocalUser.Instance.LogInInformation.ClientId).OrderByDescending(c => c.CreateTime).ToList();

                switch(SearChOrderGubun.SelectedIndex)
                {
                    case 1:
                        DataSource = DataSource.Where(c => c.StopDateYN == false || c.Stop1stDateYN == false || c.Stop2stDateYN == false || c.Stop3stDateYN == false || c.Stop4stDateYN == false ).ToList();
                        break;

                    case 2:
                        DataSource = DataSource.Where(c => c.StopDateYN == true &&  c.Stop1stDateYN == true && c.Stop2stDateYN == true && c.Stop3stDateYN == true && c.Stop4stDateYN == true ).ToList();

                        break;

                }

                if (!String.IsNullOrEmpty(SearchText.Text))
                {
                    switch (SearchTextFilter.SelectedIndex)
                    {
                        
                        case 1:
                            DataSource = DataSource.Where(c => c.OrderGid != 0 && c.OrderGid.ToString().Contains(SearchText.Text)).ToList();
                            break;
                        case 2:
                            DataSource = DataSource.Where(c => c.CustomerName != null && c.CustomerName.Contains(SearchText.Text)).ToList();
                            break;
                        case 3:
                            DataSource = DataSource.Where(c => c.StartCAddress != null && c.StartCAddress.Contains(SearchText.Text)).ToList();
                            break;
                        case 4:
                            DataSource = DataSource.Where(c => c.StopAddress != null && c.StopAddress.Contains(SearchText.Text)).ToList();
                            break;
                        case 5:
                            DataSource = DataSource.Where(c => c.DriverName != null && c.DriverName.Contains(SearchText.Text)).ToList();
                            break;
                        case 6:
                            DataSource = DataSource.Where(c => c.DriverRemark != null && c.DriverRemark.Contains(SearchText.Text)).ToList();
                            break;
                       
                    }
                }

                switch (SearChDateGubun.SelectedIndex)
                {
                    case 0:
                        DataSource = DataSource.Where(c => c.CreateTime >= Now && c.CreateTime < Tommorow).ToList();
                        break;

                    case 1:
                        DataSource = DataSource.Where(c => c.StartDate >= Now && c.StartDate < Tommorow).ToList();

                        break;

                }
              
                orderGsBindingSource.DataSource = DataSource;
            }

            ValidateIgnore = false;


            NewOrderG_Click(null, null);



        }
        bool MethodProcess = false;

        private void orderGsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

            MethodProcess = true;
            _NewOrder();
            if (orderGsBindingSource.Current != null)
            {
                var Current = orderGsBindingSource.Current as OrderG;
                ModelToView(Current);

                if (DataList.RowCount > 0)
                {
                    GridIndex = orderGsBindingSource.Position;
                  //  DataList.CurrentCell = DataList.Rows[GridIndex].Cells[0];
                }

                IsCurrentNull = false;


            }
            MethodProcess = false;
        }
       

        private void ModelToView(OrderG Current)
        {
            try
            {
                Customer.SelectedValue = Current.CustomerId;
                CustomerRemark.Text = Current.CustomerRemark;

                StartCid.SelectedText = Current.StartCname;
                StartCAddress.Text = Current.StartCAddress;

                StartInDate.Value = Current.StartInDate;

               
                    if (Current.StartInTimeHour > 12)
                    {
                        StartInTimeGubun.SelectedIndex = 1;
                        StartInTimeHour.SelectedIndex = Current.StartInTimeHour - 13;
                    }
                    else
                    {
                        StartInTimeGubun.SelectedIndex = 0;
                        StartInTimeHour.SelectedIndex = Current.StartInTimeHour-1;
                    }
                    StartInTimeMinute.Checked = Current.StartInTimeMinute == true;
               


                //StartInTimeGubun.SelectedValue = Current.StartInTimeGubun;
                //StartInTimeHour.SelectedIndex = Current.StartInTimeHour - 1;
                //StartInTimeMinute.Checked = Current.StartInTimeMinute;
                StartInGubun.SelectedValue = Current.StartInGubun;

                StartDate.Value = Current.StartDate;

               
                    if (Current.StartTimeHour > 12)
                    {
                        StartTimeGubun.SelectedIndex = 1;
                        StartTimeHour.SelectedIndex = Current.StartTimeHour - 13;
                    }
                    else
                    {
                        StartTimeGubun.SelectedIndex = 0 ;
                        StartTimeHour.SelectedIndex = Current.StartTimeHour-1;
                    }
                    StartTimeMinute.Checked = Current.StartTimeMinute == true;
                

                //StartTimeGubun.SelectedValue = Current.StartTimeGubun;
                //StartTimeHour.SelectedIndex = Current.StartTimeHour - 1;
                //StartTimeMinute.Checked = Current.StartTimeMinute;

                StopDayGubun.SelectedValue = Current.StopDayGubun;
                StopGubun.SelectedValue = Current.StopGubun;

                Remark.Text = Current.Remark;

                Driver.SelectedValue = Current.DriverId;
                DriverRemark.Text = Current.DriverRemark;

                StopCustomerId.Tag = Current.StopCustomerId;
                StopCustomerId.Text = Current.StopCustomerName;
                StopAddress.Text = Current.StopAddress;
                StopPlt.Text = Current.StopPlt.ToString("N0");
                StopBox.Text = Current.StopBox.ToString("N0");
                if (Current.StopDateYN == true)
                {
                    StopDatetime.Text = Current.StopDateTime.ToString("MM-dd HH:mm");
                }
                else
                {
                    StopDatetime.Text = "";
                }
                
                Stop1stCustomer.Tag = Current.Stop1stCustomerId;
                Stop1stCustomer.Text = Current.Stop1stCustomerName;
                if (Current.Stop1stCustomerId == 0)
                {
                    Stop1stCustomer.Tag = null;
                    Stop1stCustomer.Text = "";
                }

                Stop1stAddress.Text = Current.Stop1stAddress;
                Stop1stPlt.Text = Current.Stop1stPlt.ToString("N0");
                Stop1stBox.Text = Current.Stop1stBox.ToString("N0");

                if (Current.Stop1stDateYN == true)
                {
                    Stop1stDatetime.Text = Current.Stop1stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {

                    Stop1stDatetime.Text = "";
                }

                Stop2stCustomer.Tag = Current.Stop2stCustomerId;
                Stop2stCustomer.Text = Current.Stop2stCustomerName;
                if (Current.Stop2stCustomerId == 0)
                {
                    Stop2stCustomer.Tag = null;
                    Stop2stCustomer.Text = "";
                }
                Stop2stAddress.Text = Current.Stop2stAddress;
                Stop2stPlt.Text = Current.Stop2stPlt.ToString("N0");
                Stop2stBox.Text = Current.Stop2stBox.ToString("N0");

                if (Current.Stop2stDateYN == true)
                {
                    Stop2stDatetime.Text = Current.Stop2stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {

                    Stop2stDatetime.Text = "";
                }
                Stop3stCustomer.Tag = Current.Stop3stCustomerId;
                Stop3stCustomer.Text = Current.Stop3stCustomerName;

                if (Current.Stop3stCustomerId == 0)
                {
                    Stop3stCustomer.Tag = null;
                    Stop3stCustomer.Text = "";
                }

                Stop3stAddress.Text = Current.Stop3stAddress;
                Stop3stPlt.Text = Current.Stop3stPlt.ToString("N0");
                Stop3stBox.Text = Current.Stop3stBox.ToString("N0");

                if (Current.Stop3stDateYN == true)
                {
                    Stop3stDatetime.Text = Current.Stop3stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {

                    Stop3stDatetime.Text = "";
                }
                Stop4stCustomer.Tag = Current.Stop4stCustomerId;
                Stop4stCustomer.Text = Current.Stop3stCustomerName;
                if (Current.Stop4stCustomerId == 0)
                {
                    Stop4stCustomer.Tag = null;
                    Stop4stCustomer.Text = "";
                }
                Stop4stAddress.Text = Current.Stop4stAddress;
                Stop4stPlt.Text = Current.Stop4stPlt.ToString("N0");
                Stop4stBox.Text = Current.Stop4stBox.ToString("N0");

                if (Current.Stop4stDateYN == true)
                {
                    Stop4stDatetime.Text = Current.Stop4stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {

                    Stop4stDatetime.Text = "";
                }
                StopRemark.Text = Current.StopRemark;

                dtpRequestDate.Value = Current.CreateTime;
                pltSum.Text = Current.StopPlt.ToString("N0");
                BoxSum.Text = Current.StopBox.ToString("N0");


                if (Current.StartDateYN == true)
                {
                    StartDatetime.Text = Current.StartDateTime.ToString("MM-dd HH:mm");
                }
                else
                {
                    StartDatetime.Text = "";
                }

                if (Current.Start1stDateYN == true)
                {
                    Start1stDatetime.Text = Current.Start1stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {
                    Start1stDatetime.Text = "";
                }

                if (Current.Start2stDateYN == true)
                {
                    Start2stDatetime.Text = Current.Start2stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {
                    Start2stDatetime.Text = "";
                }

                if (Current.Start3stDateYN == true)
                {
                    Start3stDatetime.Text = Current.Start3stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {
                    Start2stDatetime.Text = "";
                }

                if (Current.Start4stDateYN == true)
                {
                    Start4stDatetime.Text = Current.Start4stDatetime.ToString("MM-dd HH:mm");
                }
                else
                {
                    Start4stDatetime.Text = "";
                }
            }
            catch
            {

            }
        }
        private void NewOrderG_Click(object sender, EventArgs e)
        {
            DataList.CurrentCell = null;
            IsCurrentNull = true;
            _NewOrder();
            // Driver.Enabled = true;

        }

        private void _NewOrder()
        {

            Customer.SelectedIndex = 0;
            CustomerRemark.Text = "";

            //StartCid.SelectedIndex = 0;
            StartCAddress.Clear();

            StartInDate.Value = DateTime.Now;
            StartInTimeGubun.SelectedIndex = 0;
            StartInTimeHour.SelectedIndex = 0;
            StartInTimeMinute.Checked = false;
            StartInGubun.SelectedIndex = 0;

            StartDate.Value = DateTime.Now;
            StartTimeGubun.SelectedIndex = 0;
            StartTimeHour.SelectedIndex = 0;
            StartTimeMinute.Checked = false;

            StopDayGubun.SelectedIndex = 0;
            StopGubun.SelectedIndex = 0;

            Remark.Clear();

            Driver.SelectedIndex = 0;
            DriverRemark.Text = "";


            StopCustomerId.Text = "";
            StopCustomerId.Tag = null;
            StopAddress.Clear();
            StopPlt.Text = "0";
            StopBox.Text = "0";
            StopDatetime.Text = "";

            Stop1stCustomer.Text = "";
            Stop1stCustomer.Tag = null;
            Stop1stAddress.Clear();
            Stop1stPlt.Text = "0";
            Stop1stBox.Text = "0";
            Stop1stDatetime.Text = "";

            Stop2stCustomer.Text = "";
            Stop2stCustomer.Tag = null;

            Stop2stAddress.Clear();
            Stop2stPlt.Text = "0";
            Stop2stBox.Text = "0";
            Stop2stDatetime.Text = "";

            Stop3stCustomer.Text = "";
            Stop3stCustomer.Tag = null;
            
            Stop3stAddress.Clear();
            Stop3stPlt.Text = "0";
            Stop3stBox.Text = "0";
            Stop3stDatetime.Text = "";

            Stop4stCustomer.Text = "";
            Stop4stCustomer.Tag = null;

            
            Stop4stAddress.Clear();
            Stop4stPlt.Text = "0";
            Stop4stBox.Text = "0";
            Stop4stDatetime.Text = "";

            pltSum.Text = "";
            BoxSum.Text = "";

            StartDatetime.Text = "";
            Start1stDatetime.Text = "";
            Start2stDatetime.Text = "";
            Start3stDatetime.Text = "";
            Start4stDatetime.Text = "";

            StopRemark.Clear();



        }

        private void SaveOrderG_Click(object sender, EventArgs e)
        {
            //화주정보
            if(Customer.SelectedIndex == 0)
            {
                MessageBox.Show("화주정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Customer.Focus();
                return;
            }

            //화주정보
            if (StartCid.SelectedIndex == 0)
            {
                MessageBox.Show("출발지는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StartCid.Focus();
                return;
            }

            if (StartInGubun.SelectedIndex == 0)
            {
                MessageBox.Show("운행구분은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StartInGubun.Focus();
                return;
            }

            if (StopDayGubun.SelectedIndex == 0)
            {
                MessageBox.Show("도착일구분은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopDayGubun.Focus();
                return;
            }

            //기사명
            if (Driver.SelectedIndex == 0)
            {
                MessageBox.Show("기사명은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Driver.Focus();
                return;
            }

            if (StopCustomerId.Tag == null)
            {
                MessageBox.Show("도착지 상호는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopCustomerId.Focus();
                return;
            }

            if (String.IsNullOrEmpty(StopAddress.Text))
            {
                MessageBox.Show("도착지 지역은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopAddress.Focus();
                return;
            }
            if (String.IsNullOrEmpty(StopPlt.Text))
            {
                MessageBox.Show("도착지 PLT 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopPlt.Focus();
                return;
            }
            if (String.IsNullOrEmpty(StopBox.Text))
            {
                MessageBox.Show("도착지 BOX 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopBox.Focus();
                return;
            }
            if(Stop1stCustomer.Tag != null)
            {
                if(String.IsNullOrEmpty(Stop1stAddress.Text) || String.IsNullOrEmpty(Stop1stPlt.Text) || String.IsNullOrEmpty(Stop1stBox.Text))
                {
                    MessageBox.Show("1차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    
                    return;
                }
            }

            if (Stop2stCustomer.Tag != null)
            {
                if (String.IsNullOrEmpty(Stop2stAddress.Text) || String.IsNullOrEmpty(Stop2stPlt.Text) || String.IsNullOrEmpty(Stop2stBox.Text))
                {
                    MessageBox.Show("2차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
            }


            if (Stop3stCustomer.Tag != null)
            {
                if (String.IsNullOrEmpty(Stop3stAddress.Text) || String.IsNullOrEmpty(Stop3stPlt.Text) || String.IsNullOrEmpty(Stop3stBox.Text))
                {
                    MessageBox.Show("3차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
            }


            if (Stop4stCustomer.Tag != null)
            {
                if (String.IsNullOrEmpty(Stop4stAddress.Text) || String.IsNullOrEmpty(Stop4stPlt.Text) || String.IsNullOrEmpty(Stop4stBox.Text))
                {
                    MessageBox.Show("3차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
            }

            OrderG nOrderG = new OrderG();

           
            nOrderG.CustomerId = (int)Customer.SelectedValue;
            nOrderG.CustomerName = Customer.Text;
            nOrderG.CustomerRemark = CustomerRemark.Text;

            nOrderG.StartCId = (int)Customer.SelectedValue;
            nOrderG.StartCname = StartCid.Text;
            nOrderG.StartCAddress = StartCAddress.Text;

            nOrderG.StartDate = StartDate.Value.Date;
            nOrderG.StartTimeGubun = StartTimeGubun.Text;

            nOrderG.ClientId = LocalUser.Instance.LogInInformation.ClientId;


            nOrderG.StartInDate = StartInDate.Value.Date;
            nOrderG.StartInTimeGubun = StartInTimeGubun.Text;
            nOrderG.StartInTimeHour = StartInTimeHour.SelectedIndex+1 + (StartInTimeGubun.SelectedIndex) * 12;
            nOrderG.StartInDate = nOrderG.StartInDate.AddHours(nOrderG.StartInTimeHour);
            nOrderG.StartInTimeMinute = StartInTimeMinute.Checked;
            if (StartInTimeMinute.Checked)
            {
                nOrderG.StartInDate = nOrderG.StartInDate.AddMinutes(30);
            }
            nOrderG.StartInGubun =(int)StartInGubun.SelectedValue;

            nOrderG.StartDate = StartDate.Value.Date;
            nOrderG.StartTimeGubun = StartTimeGubun.Text;
            nOrderG.StartTimeHour =StartTimeHour.SelectedIndex + 1 + (StartTimeGubun.SelectedIndex) * 12;
            nOrderG.StartDate = nOrderG.StartDate.AddHours(nOrderG.StartTimeHour);
            nOrderG.StartTimeMinute = StartTimeMinute.Checked;
            if (StartTimeMinute.Checked)
            {
                nOrderG.StartDate = nOrderG.StartDate.AddMinutes(30);
            }


            nOrderG.StopDayGubun = (int)StopDayGubun.SelectedValue;
            nOrderG.StopGubun = (int)StopGubun.SelectedValue;
            nOrderG.Remark = Remark.Text;
            nOrderG.DriverId = (int)Driver.SelectedValue;

            nOrderG.DriverName = Driver.Text;
            nOrderG.DriverRemark = DriverRemark.Text;

            nOrderG.StopCustomerId = (int)StopCustomerId.Tag;
            nOrderG.StopCustomerName = StopCustomerId.Text;
            nOrderG.StopAddress = StopAddress.Text;
            nOrderG.StopPlt = int.Parse(StopPlt.Text.Replace(",", ""));
            nOrderG.StopBox = int.Parse(StopBox.Text.Replace(",", ""));
            nOrderG.StopDateTime = DateTime.Now;
            nOrderG.StopDateYN = false;

            if (Stop1stCustomer.Tag != null)
            {
                nOrderG.Stop1stCustomerId = (int)Stop1stCustomer.Tag;
                nOrderG.Stop1stCustomerName = Stop1stCustomer.Text;
            }
            nOrderG.Stop1stAddress = Stop1stAddress.Text;
            nOrderG.Stop1stPlt = int.Parse(Stop1stPlt.Text.Replace(",", ""));
            nOrderG.Stop1stBox = int.Parse(Stop1stBox.Text.Replace(",", ""));
            nOrderG.Stop1stDatetime = DateTime.Now;
            nOrderG.Stop1stDateYN = false;

            if (Stop2stCustomer.Tag != null)
            {
                nOrderG.Stop2stCustomerId = (int)Stop2stCustomer.Tag;
                nOrderG.Stop2stCustomerName = Stop2stCustomer.Text;
            }
            nOrderG.Stop2stAddress = Stop2stAddress.Text;
            nOrderG.Stop2stPlt = int.Parse(Stop2stPlt.Text.Replace(",", ""));
            nOrderG.Stop2stBox = int.Parse(Stop2stBox.Text.Replace(",", ""));
            nOrderG.Stop2stDatetime = DateTime.Now;
            nOrderG.Stop2stDateYN = false;

            if (Stop3stCustomer.Tag != null)
            {
                nOrderG.Stop3stCustomerId = (int)Stop3stCustomer.Tag;
                nOrderG.Stop3stCustomerName = Stop3stCustomer.Text;
            }
            nOrderG.Stop3stAddress = Stop3stAddress.Text;
            nOrderG.Stop3stPlt = int.Parse(Stop3stPlt.Text.Replace(",", ""));
            nOrderG.Stop3stBox = int.Parse(Stop3stBox.Text.Replace(",", ""));
            nOrderG.Stop3stDatetime = DateTime.Now;
            nOrderG.Stop3stDateYN = false;

            if (Stop4stCustomer.Tag != null)
            {
                nOrderG.Stop4stCustomerId = (int)Stop4stCustomer.Tag;
                nOrderG.Stop4stCustomerName = Stop4stCustomer.Text;
            }
            nOrderG.Stop4stAddress = Stop4stAddress.Text;
            nOrderG.Stop4stPlt = int.Parse(Stop4stPlt.Text.Replace(",", ""));
            nOrderG.Stop4stBox = int.Parse(Stop4stBox.Text.Replace(",", ""));
            nOrderG.Stop4stDatetime = DateTime.Now;
            nOrderG.Stop4stDateYN = false;

            nOrderG.StopRemark = StopRemark.Text;

            nOrderG.CreateTime = DateTime.Now;

            nOrderG.StartDateTime = DateTime.Now;
            nOrderG.StartDateYN = false;
            nOrderG.Start1stDatetime = DateTime.Now;
            nOrderG.Start1stDateYN = false;
            nOrderG.Start2stDatetime = DateTime.Now;
            nOrderG.Start2stDateYN = false;
            nOrderG.Start3stDatetime = DateTime.Now;
            nOrderG.Start3stDateYN = false;
            nOrderG.Start4stDatetime = DateTime.Now;
            nOrderG.Start4stDateYN = false;

            using (ShareOrderGDataSet ShareOrderGDataSet = new ShareOrderGDataSet())
            {
                ShareOrderGDataSet.OrderGs.Add(nOrderG);
                ShareOrderGDataSet.SaveChanges();
                //ShareOrderGDataSet.Entry(nOrderG).State = System.Data.Entity.EntityState.Modified;
                //ShareOrderGDataSet.SaveChanges();
            }

        
            
            _Search();
            _NewOrder();
           // SetRowBackgroundColor();
        }

        private void UpOrderG_Click(object sender, EventArgs e)
        {
            if (orderGsBindingSource.Current == null)
            {
                return;
            }
            var Current = orderGsBindingSource.Current as OrderG;
            //화주정보
            if (Customer.SelectedIndex == 0)
            {
                MessageBox.Show("화주정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Customer.Focus();
                return;
            }

            //화주정보
            if (StartCid.SelectedIndex == 0)
            {
                MessageBox.Show("상차정보 상호는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StartCid.Focus();
                return;
            }

            if (StopDayGubun.SelectedIndex == 0)
            {
                MessageBox.Show("도착일구분은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopDayGubun.Focus();
                return;
            }

            //기사명
            if (Driver.SelectedIndex == 0)
            {
                MessageBox.Show("기사명은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Driver.Focus();
                return;
            }

            if (string.IsNullOrEmpty(StopCustomerId.Text))
            {
                MessageBox.Show("도착지 상호는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopCustomerId.Focus();
                return;
            }

            if (String.IsNullOrEmpty(StopAddress.Text))
            {
                MessageBox.Show("도착지 지역은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopAddress.Focus();
                return;
            }
            if (String.IsNullOrEmpty(StopPlt.Text))
            {
                MessageBox.Show("도착지 PLT 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopPlt.Focus();
                return;
            }
            if (String.IsNullOrEmpty(StopBox.Text))
            {
                MessageBox.Show("도착지 BOX 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                StopBox.Focus();
                return;
            }
            if (Stop1stCustomer.Tag != null  && Stop1stCustomer.Tag.ToString() != "0")
            {
                if (String.IsNullOrEmpty(Stop1stAddress.Text) || String.IsNullOrEmpty(Stop1stPlt.Text) || String.IsNullOrEmpty(Stop1stBox.Text))
                {
                    MessageBox.Show("1차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
            }

            if (Stop2stCustomer.Tag != null && Stop2stCustomer.Tag.ToString() != "0")
            {
                if (String.IsNullOrEmpty(Stop2stAddress.Text) || String.IsNullOrEmpty(Stop2stPlt.Text) || String.IsNullOrEmpty(Stop2stBox.Text))
                {
                    MessageBox.Show("2차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
            }


            if (Stop3stCustomer.Tag != null && Stop3stCustomer.Tag.ToString() != "0")
            {
                if (String.IsNullOrEmpty(Stop3stAddress.Text) || String.IsNullOrEmpty(Stop3stPlt.Text) || String.IsNullOrEmpty(Stop3stBox.Text))
                {
                    MessageBox.Show("3차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
            }


            if (Stop4stCustomer.Tag != null && Stop4stCustomer.Tag.ToString() != "0")
            {
                if (String.IsNullOrEmpty(Stop4stAddress.Text) || String.IsNullOrEmpty(Stop4stPlt.Text) || String.IsNullOrEmpty(Stop4stBox.Text))
                {
                    MessageBox.Show("3차경유지 나머지정보를 입력하세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
            }
            using (ShareOrderGDataSet ShareOrderGDataSet = new ShareOrderGDataSet())
            {
                OrderG uOrderG = ShareOrderGDataSet.OrderGs.Find(Current.OrderGid);



                uOrderG.CustomerId = (int)Customer.SelectedValue;
                uOrderG.CustomerName = Customer.Text;
                uOrderG.CustomerRemark = CustomerRemark.Text;

                uOrderG.StartCId = (int)Customer.SelectedValue;
                uOrderG.StartCname = StartCid.Text;
                uOrderG.StartCAddress = StartCAddress.Text;

                uOrderG.StartDate = StartDate.Value.Date;
                uOrderG.StartTimeGubun = StartTimeGubun.Text;

                //uOrderG.ClientId = LocalUser.Instance.LogInInformation.ClientId;


                uOrderG.StartInDate = StartInDate.Value.Date;
                uOrderG.StartInTimeGubun = StartInTimeGubun.Text;
                uOrderG.StartInTimeHour = StartInTimeHour.SelectedIndex + 1 + (StartInTimeGubun.SelectedIndex) * 12;
                uOrderG.StartInDate = uOrderG.StartInDate.AddHours(uOrderG.StartInTimeHour);
                uOrderG.StartInTimeMinute = StartInTimeMinute.Checked;
                if (StartInTimeMinute.Checked)
                {
                    uOrderG.StartInDate = uOrderG.StartInDate.AddMinutes(30);
                }
                uOrderG.StartInGubun = (int)StartInGubun.SelectedValue;

                uOrderG.StartDate = StartDate.Value.Date;
                uOrderG.StartTimeGubun = StartTimeGubun.Text;
                uOrderG.StartTimeHour = StartTimeHour.SelectedIndex + 1 + (StartTimeGubun.SelectedIndex) * 12;
                uOrderG.StartDate = uOrderG.StartDate.AddHours(uOrderG.StartTimeHour);
                uOrderG.StartTimeMinute = StartTimeMinute.Checked;
                if (StartTimeMinute.Checked)
                {
                    uOrderG.StartDate = uOrderG.StartDate.AddMinutes(30);
                }


                uOrderG.StopDayGubun = (int)StopDayGubun.SelectedValue;
                uOrderG.StopGubun = (int)StopGubun.SelectedValue;
                uOrderG.Remark = Remark.Text;
                uOrderG.DriverId = (int)Driver.SelectedValue;

                uOrderG.DriverName = Driver.Text;
                uOrderG.DriverRemark = DriverRemark.Text;

                uOrderG.StopCustomerId = (int)StopCustomerId.Tag;
                uOrderG.StopCustomerName = StopCustomerId.Text;
                uOrderG.StopAddress = StopAddress.Text;
                uOrderG.StopPlt = int.Parse(StopPlt.Text.Replace(",", ""));
                uOrderG.StopBox = int.Parse(StopBox.Text.Replace(",", ""));
                //nOrderG.StopDateTime = DateTime.Now;
                //nOrderG.StopDateYN = false;

                if (Stop1stCustomer.Tag != null)
                {
                    uOrderG.Stop1stCustomerId = (int)Stop1stCustomer.Tag;
                    uOrderG.Stop1stCustomerName = Stop1stCustomer.Text;
                }
                uOrderG.Stop1stAddress = Stop1stAddress.Text;
                uOrderG.Stop1stPlt = int.Parse(Stop1stPlt.Text.Replace(",", ""));
                uOrderG.Stop1stBox = int.Parse(Stop1stBox.Text.Replace(",", ""));
                //nOrderG.Stop1stDatetime = DateTime.Now;
                //nOrderG.Stop1stDateYN = false;
                if (Stop2stCustomer.Tag != null)
                {
                    uOrderG.Stop2stCustomerId = (int)Stop2stCustomer.Tag;
                    uOrderG.Stop2stCustomerName = Stop2stCustomer.Text;
                }
                uOrderG.Stop2stAddress = Stop2stAddress.Text;
                uOrderG.Stop2stPlt = int.Parse(Stop2stPlt.Text.Replace(",", ""));
                uOrderG.Stop2stBox = int.Parse(Stop2stBox.Text.Replace(",", ""));
                //nOrderG.Stop2stDatetime = DateTime.Now;
                //nOrderG.Stop2stDateYN = false;
                if (Stop3stCustomer.Tag != null)
                {
                    uOrderG.Stop3stCustomerId = (int)Stop3stCustomer.Tag;
                    uOrderG.Stop3stCustomerName = Stop3stCustomer.Text;
                }
                uOrderG.Stop3stAddress = Stop3stAddress.Text;
                uOrderG.Stop3stPlt = int.Parse(Stop3stPlt.Text.Replace(",", ""));
                uOrderG.Stop3stBox = int.Parse(Stop3stBox.Text.Replace(",", ""));
                //nOrderG.Stop3stDatetime = DateTime.Now;
                //nOrderG.Stop3stDateYN = false;
                if (Stop4stCustomer.Tag != null)
                {
                    uOrderG.Stop4stCustomerId = (int)Stop4stCustomer.Tag;
                    uOrderG.Stop4stCustomerName = Stop4stCustomer.Text;
                }
                uOrderG.Stop4stAddress = Stop4stAddress.Text;
                uOrderG.Stop4stPlt = int.Parse(Stop4stPlt.Text.Replace(",", ""));
                uOrderG.Stop4stBox = int.Parse(Stop4stBox.Text.Replace(",", ""));
                //uOrderG.Stop4stDatetime = DateTime.Now;
                //nOrderG.Stop4stDateYN = false;

                uOrderG.StopRemark = StopRemark.Text;

                //nOrderG.CreateTime = DateTime.Now;



                ShareOrderGDataSet.Entry(uOrderG).State = System.Data.Entity.EntityState.Modified;
                ShareOrderGDataSet.SaveChanges();
              

                ShareOrderGDataSet.SaveChanges();
            }
            
            _Search();

        }

        private void DeleteOrderG_Click(object sender, EventArgs e)
        {
            if (orderGsBindingSource.Current == null)
                return;
            var Current = orderGsBindingSource.Current as OrderG;

            if (MessageBox.Show("삭제 하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "DELETE FROM OrderGs WHERE OrderGid = @OrderId";
                    _Command.Parameters.AddWithValue("@OrderId", Current.OrderGid);
                    _Command.ExecuteNonQuery();
                }


            });
            try
            {
                MessageBox.Show("삭제 완료되었습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _Search();

            }
            catch
            {

                _Search();

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = "배차관리_지입_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.카드페이_지입_화물_배차_내역))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var ColumnIndexMap = new Dictionary<int, int>();
            var ColumnIndex = 0;
            for (int i = 0; i < DataList.ColumnCount; i++)
            {
                if (DataList.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }
            for (int i = 0; i < DataList.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = DataList[ColumnIndexMap[j], i].FormattedValue?.ToString();
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

        private void Search_Click(object sender, EventArgs e)
        {
            _Search();
        }


        //화주정보 
        private void Customer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Customer.SelectedIndex == 0)
            {
                CustomerRemark.Text = "";
                if (StartCid.Items.Count > 0)
                {
                    StartCid.SelectedIndex = 0;
                    StartCid.Enabled = false;
                }

            }
            else
            {
                var Query = CustomerModelList.Where(c => c.CustomerId == (int)Customer.SelectedValue).ToArray();

                if (Query.Any())
                {
                    CustomerRemark.Text = Query.First().AddressState + " " + Query.First().AddressCity + " / " + Query.First().Ceo + " / " + "☎ "+ Query.First().PhoneNo ;
                }

                StartAddress((int)Customer.SelectedValue);
                StartCid.Enabled = true;
            }
        }




        private  void StartAddress(int CustomerId)
        {
            

            InitCustomerStartTable();

            var Query = _CustomerStartTable.Where(c => c.CustomerId == CustomerId).ToArray();

            if (Query.Any())
            {

                Dictionary<string, string> StartAddress = new Dictionary<string, string>();
               

                CustomerStartData(CustomerId);

               
                var StartCidDataSource = customerStartManageDataSet.CustomerStartManage.Select(c => new { c.StartName, c.CustomerId, c.idx }).OrderBy(c => c.idx).ToArray();

                StartAddress.Add("0000", "출발지");

                foreach (var item in StartCidDataSource)
                {
                    StartAddress.Add(item.idx.ToString(), item.StartName);
                }

                StartCid.DataSource = new BindingSource(StartAddress, null);
                StartCid.DisplayMember = "Value";
                StartCid.ValueMember = "Key";
                StartCid.SelectedIndex = 1;
            }
            else
            {
                Dictionary<string, string> StartAddress = new Dictionary<string, string>();

                var Query2 = CustomerModelList.Where(c => c.CustomerId == CustomerId).Select(c => new { c.AddressState, c.AddressCity, c.AddressDetail, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();

                StartAddress.Add("0000", "출발지");

                foreach (var item in Query2)
                {
                    StartAddress.Add(item.CustomerId.ToString(), item.AddressState + " " + item.AddressCity);
                }

             


                StartCid.DataSource = new BindingSource(StartAddress, null);
                StartCid.DisplayMember = "Value";
                StartCid.ValueMember = "Key";
                StartCid.SelectedIndex = 1;
            }
        }


        //상차정보 출발지
        private void StartCid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StartCid.SelectedIndex == 0)
            {
                StartCAddress.Text = "";
                // StartCid.SelectedIndex = 0;
            }
            else
            {
                if (StartCid.Items.Count > 0)
                {
                    if (StartCid.Items.Count == 2)
                    {
                        var Query = CustomerModelList.Where(c => c.CustomerId == (int)Customer.SelectedValue).ToArray();

                        if (Query.Any())
                        {
                            StartCAddress.Text = Query.First().AddressState + " " + Query.First().AddressCity + " " + Query.First().AddressDetail;
                        }
                    }
                    else
                    {
                        if (StartCid.SelectedValue.ToString() == "0000")
                        {
                            var Query = CustomerModelList.Where(c => c.CustomerId == (int)Customer.SelectedValue).ToArray();

                            if (Query.Any())
                            {
                                StartCAddress.Text = Query.First().AddressState + " " + Query.First().AddressCity + " " + Query.First().AddressDetail;
                            }
                        }
                        else
                        {
                            var Query2 = customerStartManageDataSet.CustomerStartManage.Where(c => c.idx.ToString() == StartCid.SelectedValue.ToString()).ToArray();

                            if (Query2.Any())
                            {
                                StartCAddress.Text = Query2.First().StartState + " " + Query2.First().StartCity + " " + Query2.First().StartDetail;
                            }

                        }
                    }
                }
            }
        }

        private void Driver_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Query = DriverModelList.Where(c => c.DriverId == (int)Driver.SelectedValue).ToArray();

            if (Query.Any())
            {
                var SizeName = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == Query.First().CarSize && c.Div == "CarSize").Select(c => new { c.Name }).ToArray();

               

                var TypeName = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == Query.First().CarType && c.Div == "CarType" && c.Value == Query.First().CarType).Select(c => new { c.Name }).ToArray();


                DriverRemark.Text = Query.First().CarNo + " / " + TypeName.First().Name + " / " + SizeName.First().Name + " / " + "☎ " + Query.First().MobileNo;
            }
        }

        //private void StopCustomerId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(StopCustomerId.Text))
        //    {
        //        StopAddress.Text = "";
        //    }
        //    else

        //    {
        //        var Query = CustomerModelList.Where(c => c.CustomerId == (int)StopCustomerId.SelectedValue).ToArray();

        //        if (Query.Any())
        //        {
        //            StopAddress.Text = Query.First().AddressState + " " + Query.First().AddressCity;
        //        }
        //    }
        //}

        //private void Stop1stCustomer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Stop1stCustomer.Tag == null))
        //    {
        //        Stop1stAddress.Text = "";
        //    }
        //    else

        //    {
        //        var Query = CustomerModelList.Where(c => c.CustomerId == (int)Stop1stCustomer.SelectedValue).ToArray();

        //        if (Query.Any())
        //        {
        //            Stop1stAddress.Text = Query.First().AddressState + " " + Query.First().AddressCity;
        //        }
        //    }
        //}

        //private void Stop2stCustomer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Stop2stCustomer.Tag == null)
        //    {
        //        Stop2stAddress.Text = "";
        //    }
        //    else

        //    {
        //        var Query = CustomerModelList.Where(c => c.CustomerId == (int)Stop2stCustomer.SelectedValue).ToArray();

        //        if (Query.Any())
        //        {
        //            Stop2stAddress.Text = Query.First().AddressState + " " + Query.First().AddressCity;
        //        }
        //    }
        //}

        //private void Stop3stCustomer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Stop3stCustomer.Tag == null)
        //    {
        //        Stop3stAddress.Text = "";
        //    }
        //    else

        //    {
        //        var Query = CustomerModelList.Where(c => c.CustomerId == (int)Stop3stCustomer.SelectedValue).ToArray();

        //        if (Query.Any())
        //        {
        //            Stop3stAddress.Text = Query.First().AddressState + " " + Query.First().AddressCity;
        //        }
        //    }
        //}

        //private void Stop4stCustomer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Stop4stCustomer.SelectedIndex == 0)
        //    {
        //        Stop4stAddress.Text = "";
        //    }
        //    else

        //    {
        //        var Query = CustomerModelList.Where(c => c.CustomerId == (int)Stop4stCustomer.SelectedValue).ToArray();

        //        if (Query.Any())
        //        {
        //            Stop4stAddress.Text = Query.First().AddressState + " " + Query.First().AddressCity;
        //        }
        //    }
        //}

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

                Number.Text = "0";

            }
            else
            {

                Number.Text = int.Parse(Number.Text.Replace(",", "")).ToString("N0");

            }
            //int _Stop1stPlt = 0;
            //int _Stop2stPlt = 0;
            //int _Stop3stPlt = 0;
            //int _Stop4stPlt = 0;

            //_Stop1stPlt = Convert.ToInt32(Stop1stPlt.Text);
            //_Stop2stPlt = Convert.ToInt32(Stop2stPlt.Text);
            //_Stop3stPlt = Convert.ToInt32(Stop3stPlt.Text);
            //_Stop4stPlt = Convert.ToInt32(Stop4stPlt.Text);

            //int _Stop1stBox = 0;
            //int _Stop2stBox = 0;
            //int _Stop3stBox = 0;
            //int _Stop4stBox = 0;


            //_Stop1stBox = Convert.ToInt32(Stop1stBox.Text);
            //_Stop2stBox = Convert.ToInt32(Stop2stBox.Text);
            //_Stop3stBox = Convert.ToInt32(Stop3stBox.Text);
            //_Stop4stBox = Convert.ToInt32(Stop4stBox.Text);


            //pltSum.Text = (_Stop1stPlt + _Stop2stPlt + _Stop3stPlt + _Stop4stPlt).ToString("N0");
            //BoxSum.Text = (_Stop1stBox + _Stop2stBox + _Stop3stBox + _Stop4stBox).ToString("N0");
        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
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
        private void DataList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (IsCurrentNull)
            {
                orderGsBindingSource_CurrentChanged(null, null);
                
            }
        }

        private void DataList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < orderGsBindingSource.Count)
            {
                var Current = orderGsBindingSource[e.RowIndex] as OrderG;

                if (e.ColumnIndex == ColumnNum.Index)
                {
                    DataList[e.ColumnIndex, e.RowIndex].Value = (DataList.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
                }


                if (e.ColumnIndex == orderGidDataGridViewTextBoxColumn.Index)
                {
                    //String OrderGNo = Current.OrderGid.ToString("00000000");
                    //OrderGNo = OrderGNo.Substring(OrderGNo.Length - 8);
                    //OrderGNo = OrderGNo.Substring(0, 4) + "-" + OrderGNo.Substring(4);
                    e.Value = Current.OrderGid.ToString("00000000");
                }
                else if (e.ColumnIndex == startDateDataGridViewTextBoxColumn.Index)
                {
                   
                    e.Value = Current.StartDate.ToString("d").Replace("-", "/");
                }
                else if (e.ColumnIndex == CreateTimeDataGridViewTextBoxColumn.Index)
                {
                    
                    e.Value = Current.CreateTime.ToString("d").Replace("-", "/");
                }
                else if (e.ColumnIndex == ColumnsStatus.Index)
                {
                    if(Current.StopDateYN == true && Current.Stop1stDateYN == true && Current.Stop2stDateYN == true && Current.Stop3stDateYN == true && Current.Stop4stDateYN == true )
                    {
                        e.Value = "완료";
                    }
                    else
                    {
                        e.Value = "운행중";
                    }
                }

                else if (e.ColumnIndex == ColumnsCarSize.Index)
                {

                    if (DriverModelList.Where(c => c.DriverId == Current.DriverId).Any())
                    {

                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == DriverModelList.First().CarSize))
                        {
                            e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == DriverModelList.First().CarSize).Name;
                        }
                    }
                }
                else if (e.ColumnIndex == ColumnsCarType.Index)
                {

                    if (DriverModelList.Where(c => c.DriverId == Current.DriverId).Any())
                    {

                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == DriverModelList.First().CarType))
                        {
                            e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == DriverModelList.First().CarType).Name;
                        }
                    }


                }
                else if (e.ColumnIndex == ColumnsCarNo.Index)
                {
                    if (DriverModelList.Where(c => c.DriverId == Current.DriverId).Any())
                    {
                        e.Value = DriverModelList.First().CarNo;
                    }
                }

              

              
            }
        }

        private void rdb_Client_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Car.Checked)
            {


                FrmMDI frmMDI = new FrmMDI();

                FrmMN0301 _Form = new FrmMN0301();
                _Form.MdiParent = this.MdiParent;

                _Form.Show();

                this.Close();

            }
            //else if(rdo_Defalut.Checked)
            //{
            //    FrmMDI frmMDI = new FrmMDI();

            //    FrmMN0301Default _Form = new FrmMN0301Default();
            //    _Form.MdiParent = this.MdiParent;

            //    _Form.Show();

            //    this.Close();
            //}

            else if (rdb_Ban.Checked)
            {
                FrmMDI frmMDI = new FrmMDI();

                FrmMN0301B _Form = new FrmMN0301B();
                _Form.MdiParent = this.MdiParent;

                _Form.Show();

                this.Close();
            }
        }

        private void SearchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                _Search();
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (orderGsBindingSource.Current == null)
            {
                return;
            }
            dtp.Value = (DateTime)DataList.Rows[GridIndex].Cells[3].Value;
            Rectangle Rectangle = DataList.GetCellDisplayRectangle(3, GridIndex, true);
            dtp.Size = new Size(Rectangle.Width, Rectangle.Height);
            dtp.Location = new Point(Rectangle.X, Rectangle.Y);


            dtp.Visible = true;

        }
        int _OrderGid = 0;
        private void dtp_OnTextChange(object sender, EventArgs e)
        {
          
            DataList.Rows[GridIndex].Cells[3].Value = dtp.Text.ToString();


            _OrderGid = Convert.ToInt32(DataList.Rows[GridIndex].Cells[1].Value.ToString().Replace("-",""));

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                    "Update OrderGs SET StartDate = @StartDate WHERE OrderGid = @OrderGid";

                cmd.Parameters.AddWithValue("@OrderGid", _OrderGid);
                cmd.Parameters.AddWithValue("@StartDate", dtp.Value);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

            Search_Click(null,null);
            //MessageBox.Show(_OrderGid.ToString());
        }
        void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }

        private void DataList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            var Current = orderGsBindingSource[e.RowIndex] as OrderG;
            //var Selected = ((DataRowView)DataList.Rows[e.RowIndex].DataBoundItem).Row as OrderDataSet.OrderGsRow;


            if (e.Button == MouseButtons.Right)
            {


                if (e.ColumnIndex == startDateDataGridViewTextBoxColumn.Index)
                {

                    

                    Rectangle Rectangle = DataList.GetCellDisplayRectangle(e.ColumnIndex, GridIndex, true);
                    m.Show(DataList, new Point(Rectangle.X, Rectangle.Y));


                }
            }
            else
            {

                if (e.ColumnIndex == startDateDataGridViewTextBoxColumn.Index)
                {
                    startDateDataGridViewTextBoxColumn.ReadOnly = true;
                }

                dtp.Visible = false;
            }
        }

        private void StopCustomerId_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == 13)
            {
                StopCustomerSelect.Items.Clear();
                int ItemIndex = 0;


                    var CustomerDataSource = CustomerModelList.Where(c => c.BizGubun == 5).Select(c => new { c.SangHo, c.CustomerId,c.PhoneNo,c.AddressState,c.AddressCity ,c.AddressDetail}).OrderBy(c => c.CustomerId).ToArray();


                


                foreach (var item in CustomerDataSource.Where(c => c.SangHo.Contains(StopCustomerId.Text)).ToList())
                {

                    StopCustomerSelect.Items.Add(item.SangHo);
                    StopCustomerSelect.Items[ItemIndex].SubItems.Add(item.PhoneNo);
                    StopCustomerSelect.Items[ItemIndex].SubItems.Add(item.AddressState + " " + item.AddressCity + " " + item.AddressDetail);
                    StopCustomerSelect.Items[ItemIndex].Tag = item.CustomerId;
                    ItemIndex++;
                }



                if (StopCustomerSelect.Items.Count > 0)
                {

                    StopCustomerSelect.Items[0].Selected = true;
                    StopCustomerSelect.Visible = true;
                    StopCustomerSelect.Focus();
                }
                else
                {

                    StopCustomerId.Clear();
                    StopCustomerId.Tag = null;
                }

                //StopCustomerSelect.Visible = true;
            }
          
        }


        private void Stop1stCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Stop1stCustomerSelect.Items.Clear();
                int ItemIndex = 0;


                var CustomerDataSource = CustomerModelList.Where(c => c.BizGubun == 5).Select(c => new { c.SangHo, c.CustomerId, c.PhoneNo, c.AddressState, c.AddressCity, c.AddressDetail }).OrderBy(c => c.CustomerId).ToArray();

                foreach (var item in CustomerDataSource.Where(c => c.SangHo.Contains(Stop1stCustomer.Text)).ToList())
                {

                    Stop1stCustomerSelect.Items.Add(item.SangHo);
                    Stop1stCustomerSelect.Items[ItemIndex].SubItems.Add(item.PhoneNo);
                    Stop1stCustomerSelect.Items[ItemIndex].SubItems.Add(item.AddressState + " " + item.AddressCity + " " + item.AddressDetail);
                    Stop1stCustomerSelect.Items[ItemIndex].Tag = item.CustomerId;
                    ItemIndex++;
                }



                if (Stop1stCustomerSelect.Items.Count > 0)
                {

                    Stop1stCustomerSelect.Items[0].Selected = true;
                    Stop1stCustomerSelect.Visible = true;
                    Stop1stCustomerSelect.Focus();
                }
                else
                {

                    Stop1stCustomer.Clear();
                    Stop1stCustomer.Tag = null;
                }

               
            }
        }

        private void Stop2stCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Stop2stCustomerSelect.Items.Clear();
                int ItemIndex = 0;


                var CustomerDataSource = CustomerModelList.Where(c => c.BizGubun == 5).Select(c => new { c.SangHo, c.CustomerId, c.PhoneNo, c.AddressState, c.AddressCity, c.AddressDetail }).OrderBy(c => c.CustomerId).ToArray();

                foreach (var item in CustomerDataSource.Where(c => c.SangHo.Contains(Stop2stCustomer.Text)).ToList())
                {

                    Stop2stCustomerSelect.Items.Add(item.SangHo);
                    Stop2stCustomerSelect.Items[ItemIndex].SubItems.Add(item.PhoneNo);
                    Stop2stCustomerSelect.Items[ItemIndex].SubItems.Add(item.AddressState + " " + item.AddressCity + " " + item.AddressDetail);
                    Stop2stCustomerSelect.Items[ItemIndex].Tag = item.CustomerId;
                    ItemIndex++;
                }



                if (Stop2stCustomerSelect.Items.Count > 0)
                {

                    Stop2stCustomerSelect.Items[0].Selected = true;
                    Stop2stCustomerSelect.Visible = true;
                    Stop2stCustomerSelect.Focus();
                }
                else
                {

                    Stop2stCustomer.Clear();
                    Stop2stCustomer.Tag = null;
                }

                
            }
        }

        private void Stop3stCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Stop3stCustomerSelect.Items.Clear();
                int ItemIndex = 0;


                var CustomerDataSource = CustomerModelList.Where(c => c.BizGubun == 5).Select(c => new { c.SangHo, c.CustomerId, c.PhoneNo, c.AddressState, c.AddressCity, c.AddressDetail }).OrderBy(c => c.CustomerId).ToArray();

                foreach (var item in CustomerDataSource.Where(c => c.SangHo.Contains(Stop3stCustomer.Text)).ToList())
                {

                    Stop3stCustomerSelect.Items.Add(item.SangHo);
                    Stop3stCustomerSelect.Items[ItemIndex].SubItems.Add(item.PhoneNo);
                    Stop3stCustomerSelect.Items[ItemIndex].SubItems.Add(item.AddressState + " " + item.AddressCity + " " + item.AddressDetail);
                    Stop3stCustomerSelect.Items[ItemIndex].Tag = item.CustomerId;
                    ItemIndex++;
                }



                if (Stop3stCustomerSelect.Items.Count > 0)
                {

                    Stop3stCustomerSelect.Items[0].Selected = true;
                    Stop3stCustomerSelect.Visible = true;
                    Stop3stCustomerSelect.Focus();
                }
                else
                {

                    Stop3stCustomer.Clear();
                    Stop3stCustomer.Tag = null;
                }

                
            }
        }

        private void Stop4stCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Stop4stCustomerSelect.Items.Clear();
                int ItemIndex = 0;


                var CustomerDataSource = CustomerModelList.Where(c => c.BizGubun == 5).Select(c => new { c.SangHo, c.CustomerId, c.PhoneNo, c.AddressState, c.AddressCity, c.AddressDetail }).OrderBy(c => c.CustomerId).ToArray();

                foreach (var item in CustomerDataSource.Where(c => c.SangHo.Contains(Stop4stCustomer.Text)).ToList())
                {

                    Stop4stCustomerSelect.Items.Add(item.SangHo);
                    Stop4stCustomerSelect.Items[ItemIndex].SubItems.Add(item.PhoneNo);
                    Stop4stCustomerSelect.Items[ItemIndex].SubItems.Add(item.AddressState + " " + item.AddressCity + " " + item.AddressDetail);
                    Stop4stCustomerSelect.Items[ItemIndex].Tag = item.CustomerId;
                    ItemIndex++;
                }



                if (Stop4stCustomerSelect.Items.Count > 0)
                {

                    Stop4stCustomerSelect.Items[0].Selected = true;
                    Stop4stCustomerSelect.Visible = true;
                    Stop4stCustomerSelect.Focus();
                }
                else
                {

                    Stop4stCustomer.Clear();
                    Stop4stCustomer.Tag = null;
                }

                
            }
        }


       

        private void _CustomerSelected(int Gubun)
        {
            int _Gubun = 0;

            _Gubun = Gubun;

            switch(_Gubun)
            {
                case 0:
                    StopCustomerId.Clear();

                    var _Customer = CustomerModelList.Find(c => c.CustomerId == (int)StopCustomerSelect.SelectedItems[0].Tag);

                    StopCustomerId.Text = _Customer.SangHo;
                    StopCustomerId.Tag = _Customer.CustomerId;
                    StopAddress.Text = _Customer.AddressState + " " + _Customer.AddressCity + " " + _Customer.AddressDetail;

                    StopCustomerSelect.Visible = false;
                    StopCustomerId.Focus();
                    break;

                case 1:
                    Stop1stCustomer.Clear();

                    var _Customer1 = CustomerModelList.Find(c => c.CustomerId == (int)Stop1stCustomerSelect.SelectedItems[0].Tag);

                    Stop1stCustomer.Text = _Customer1.SangHo;
                    Stop1stCustomer.Tag = _Customer1.CustomerId;
                    Stop1stAddress.Text = _Customer1.AddressState + " " + _Customer1.AddressCity + " " + _Customer1.AddressDetail;

                    Stop1stCustomerSelect.Visible = false;
                    Stop1stCustomer.Focus();
                    break;

                case 2:
                    Stop2stCustomer.Clear();

                    var _Customer2 = CustomerModelList.Find(c => c.CustomerId == (int)Stop2stCustomerSelect.SelectedItems[0].Tag);

                    Stop2stCustomer.Text = _Customer2.SangHo;
                    Stop2stCustomer.Tag = _Customer2.CustomerId;
                    Stop2stAddress.Text = _Customer2.AddressState + " " + _Customer2.AddressCity + " " + _Customer2.AddressDetail;

                    Stop2stCustomerSelect.Visible = false;
                    Stop2stCustomer.Focus();
                    break;


                case 3:
                    Stop3stCustomer.Clear();

                    var _Customer3 = CustomerModelList.Find(c => c.CustomerId == (int)Stop3stCustomerSelect.SelectedItems[0].Tag);

                    Stop3stCustomer.Text = _Customer3.SangHo;
                    Stop3stCustomer.Tag = _Customer3.CustomerId;
                    Stop3stAddress.Text = _Customer3.AddressState + " " + _Customer3.AddressCity + " " + _Customer3.AddressDetail;

                    Stop3stCustomerSelect.Visible = false;
                    Stop3stCustomer.Focus();
                    break;

                case 4:
                    Stop4stCustomer.Clear();

                    var _Customer4 = CustomerModelList.Find(c => c.CustomerId == (int)Stop4stCustomerSelect.SelectedItems[0].Tag);

                    Stop4stCustomer.Text = _Customer4.SangHo;
                    Stop4stCustomer.Tag = _Customer4.CustomerId;
                    Stop4stAddress.Text = _Customer4.AddressState + " " + _Customer4.AddressCity + " " + _Customer4.AddressDetail;

                    Stop4stCustomerSelect.Visible = false;
                    Stop4stCustomer.Focus();
                    break;
            }

                

           

            
        }


        private void StopCustomerSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (StopCustomerSelect.SelectedItems.Count > 0)
                {
                    _CustomerSelected(0);
                }
            }
        }

        private void StopCustomerSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in StopCustomerSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _CustomerSelected(0);
                    }
                }
            }
        }


        private void Stop1stCustomerSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (Stop1stCustomerSelect.SelectedItems.Count > 0)
                {
                    _CustomerSelected(1);
                }
            }
        }

        private void Stop1stCustomerSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in Stop1stCustomerSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _CustomerSelected(1);
                    }
                }
            }
        }

        private void Stop2stCustomerSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (Stop2stCustomerSelect.SelectedItems.Count > 0)
                {
                    _CustomerSelected(2);
                }
            }
        }

        private void Stop2stCustomerSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in Stop2stCustomerSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _CustomerSelected(2);
                    }
                }
            }
        }

        private void Stop3stCustomerSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (Stop2stCustomerSelect.SelectedItems.Count > 0)
                {
                    _CustomerSelected(3);
                }
            }
        }

        private void Stop3stCustomerSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in Stop2stCustomerSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _CustomerSelected(3);
                    }
                }
            }
        }

        private void Stop4stCustomerSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (Stop2stCustomerSelect.SelectedItems.Count > 0)
                {
                    _CustomerSelected(4);
                }
            }
        }

        private void Stop4stCustomerSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in Stop2stCustomerSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _CustomerSelected(4);
                    }
                }
            }
        }

        
    }
}
