using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301_CARGOACCEPT_Add : Form
    {
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
        LocationHelper mLocationHelper = new LocationHelper();
        Int64 iPrice = 0;
        Int64 iClientPrice = 0;
        Int64 iCarCount = 0;
        
        public FrmMN0301_CARGOACCEPT_Add()
        {
            foreach (var item in SingleDataSet.Instance.AddressReferences)
            {
                if (String.IsNullOrEmpty(item.State) || String.IsNullOrEmpty(item.City) || String.IsNullOrEmpty(item.Street))
                    continue;
                if (!AddressList.Any(c => c.State == item.State && c.City == item.City))
                {
                    AddressList.Add(new AddressViewModel
                    {
                        State = item.State,
                        City = item.City,
                    });
                }
            }
            InitializeComponent();
            Image1.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image2.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image3.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image4.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image5.Image = Properties.Resources.ic_photo_white_48dp_2x;
            ImageName1.Text = "";
            ImageName2.Text = "";
            ImageName3.Text = "";
            ImageName4.Text = "";
            ImageName5.Text = "";

            _InitCmb();
            Location.SelectedIndex = 2;


            StartTime.Value = DateTime.Now;
            StopTime.Value = DateTime.Now;

            StartTimeHour.SelectedValue = DateTime.Now.Hour;
            StopTimeHour.SelectedValue = DateTime.Now.AddHours(1).Hour;

            cmb_Radius.SelectedValue = 30;

            CarCount.SelectedIndex = 0;
            if(!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                ClientPrice.Enabled = false;
            }
            InitCustomerTable();
        }

        public void ShowFromCustomerId(int CustomerId)
        {
            var Index = _CustomerViewModelList.FindIndex(c => c.CustomerId == CustomerId);
            dataGridView2.CurrentCell = dataGridView2[0, Index];
            Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "화물접수내역", int.Parse(CarCount.Text)), "화물접수내역 추가 성공");
                StartTime.Value = DateTime.Now;
                StartTimeHour.SelectedValue = DateTime.Now.Hour;
                StartTimeMinute.SelectedIndex = 0;
                StopTime.Value = DateTime.Now;
                StopTimeHour.SelectedValue = DateTime.Now.AddHours(1).Hour;
                StopTimeMinute.SelectedIndex = 0;
                var clientAddressTableAdapter = new CMDataSetTableAdapters.ClientAddressTableAdapter();
                var client = clientAddressTableAdapter.GetData().Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();
                if (client.Any())
                {
                    StartState.SelectedValue = client.First().AddressState;
                    StartCity.SelectedValue = client.First().AddressCity;
                    StopState.SelectedValue = client.First().AddressState;
                    StopCity.SelectedValue = client.First().AddressCity;

                }
                else
                {
                    StartState.SelectedValue = "서울특별시";
                    StartCity.SelectedValue = "종로구";
                    StopState.SelectedValue = "서울특별시";
                    StopCity.SelectedValue = "종로구";
                }
                Item.Clear();
                IsShared.SelectedIndex = 0;
                CarType.SelectedIndex = 0;
                CarSize.SelectedIndex = 0;
                CarCount.SelectedIndex = 0;
                ItemSize.Text = "0";
                CarSize.DataSource = Filter.Order.CarSizeList;
                Location.SelectedIndex = 2;
                Price.Clear();
                ClientPrice.Clear();
            }
        }
        public bool IsSuccess = false;
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "화물접수내역 ", int.Parse(CarCount.Text)), "화물접수내역 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }
        private int _UpdateDB()
        {
            err.Clear();
            StartTime.Value = StartTime.Value.Date.AddHours((int)StartTimeHour.SelectedValue).AddMinutes((int)StartTimeMinute.SelectedValue);
            StopTime.Value = StopTime.Value.Date.AddHours((int)StopTimeHour.SelectedValue).AddMinutes((int)StopTimeMinute.SelectedValue);

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("해당하는 화물 의뢰자가 없습니다.");
                return -1;
            }

            if (StopTime.Value <= StartTime.Value)
            {
                MessageBox.Show("도착지 날짜는 반드시 출발지 이후로 설정하셔야 합니다.");
                return -1;
            }

            if (String.IsNullOrEmpty(Item.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (CarType.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(CarType, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (CarSize.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(CarSize, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (String.IsNullOrEmpty(Price.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            else if (!Int64.TryParse(Price.Text.Replace(",", ""), out iPrice))
            {
                MessageBox.Show("차주운송료를 숫자로 입력해 주십시오.");
                return -1;
            }

            if(!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                var _Customer = _CustomerViewModelList[dataGridView2.SelectedRows[0].Index];
                decimal _Fee = (decimal)_Customer.Fee;
                _Fee += 100;
                _Fee *= 0.01m;
                decimal _d =(decimal)iPrice;
                iClientPrice = (long)Math.Floor(_d * _Fee);
                ClientPrice.Text = iClientPrice.ToString("N0");
            }
            else
            {
                if (String.IsNullOrEmpty(ClientPrice.Text))
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }
                else if (!Int64.TryParse(ClientPrice.Text.Replace(",", ""), out iClientPrice))
                {
                    MessageBox.Show("화주운송료를 숫자로 입력해 주십시오.");
                    return -1;
                }
            }

            if (!Int64.TryParse(CarCount.Text, out iCarCount))
            {
                MessageBox.Show("차량대수를  숫자로 입력해 주십시오.");
                return -1;
            }

            try
            {
                _AddClient();
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        string PhoneNo = "";
        private void FrmMN0207_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT AddressState, AddressCity, PhoneNo FROM Clients WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            StartState.SelectedValue = _Reader.GetStringN(0);
                            StartCity.SelectedValue = _Reader.GetStringN(1);
                            StopState.SelectedValue = _Reader.GetStringN(0);
                            StopCity.SelectedValue = _Reader.GetStringN(1);
                            PhoneNo = _Reader.GetStringN(2);
                        }
                    }
                }
                _Connection.Close();
            }

            ChangeInfo();
        }

        private void _InitCmb()
        {

            StartTimeHour.DisplayMember = "Text";
            StartTimeHour.ValueMember = "Value";
            StartTimeHour.DataSource = Filter.Order.HourList;


            StartTimeMinute.DisplayMember = "Text";
            StartTimeMinute.ValueMember = "Value";
            StartTimeMinute.DataSource = Filter.Order.MinuteList;


            StopTimeHour.DisplayMember = "Text";
            StopTimeHour.ValueMember = "Value";
            StopTimeHour.DataSource = Filter.Order.HourList;


            StopTimeMinute.DisplayMember = "Text";
            StopTimeMinute.ValueMember = "Value";
            StopTimeMinute.DataSource = Filter.Order.MinuteList;


            var StartStateDataSource = (from a in AddressList select new { a.State }).Distinct().ToArray();
            StartState.DataSource = StartStateDataSource;
            StartState.DisplayMember = "State";
            StartState.ValueMember = "State";

            var EndStateDataSource = (from a in AddressList select new { a.State }).Distinct().ToArray();
            StopState.DataSource = EndStateDataSource;
            StopState.DisplayMember = "State";
            StopState.ValueMember = "State";


            var StartCityDataSource = AddressList.Where(c => c.State == StartState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            StartCity.DataSource = StartCityDataSource;
            StartCity.DisplayMember = "City";
            StartCity.ValueMember = "City";

            var EndCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == StopState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            StopCity.DataSource = EndCityDataSource;
            StopCity.DisplayMember = "City";
            StopCity.ValueMember = "City";

            IsShared.DisplayMember = "Text";
            IsShared.ValueMember = "BoolValue";
            IsShared.DataSource = Filter.Order.IsSharedList;

            CarType.DisplayMember = "Text";
            CarType.ValueMember = "Value";
            CarType.DataSource = Filter.Order.CarTypeList;


            CarSize.DisplayMember = "Text";
            CarSize.ValueMember = "Value";
            CarSize.DataSource = Filter.Order.CarSizeList;

            Location.DisplayMember = "Text";
            Location.ValueMember = "Value";
            Location.DataSource = Filter.Order.LocationList;


            cmb_NFilterType.DisplayMember = "Text";
            cmb_NFilterType.ValueMember = "Value";
            cmb_NFilterType.DataSource = Filter.Order.FilterTypeList;

            cmb_Radius.DisplayMember = "Text";
            cmb_Radius.ValueMember = "Value";
            cmb_Radius.DataSource = Filter.Order.RadiusList;



            Dictionary<string, string> NotificationGroup = new Dictionary<string, string>();
            NotificationGroup.Add("0", "그룹전체");
            NotificationGroup.Add("A", "A 그룹");
            NotificationGroup.Add("B", "B 그룹");
            NotificationGroup.Add("C", "C 그룹");

            cmb_NotificationGroup.DataSource = new BindingSource(NotificationGroup, null);
            cmb_NotificationGroup.DisplayMember = "Value";
            cmb_NotificationGroup.ValueMember = "Key";


        }

        private void cmb_StartState_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartCity.Enabled = true;
            var StartCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == StartState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            StartCity.DataSource = StartCityDataSource;
            StartCity.DisplayMember = "City";
            StartCity.ValueMember = "City";
        }

        private void cmb_EndState_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopCity.Enabled = true;
            var EndCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == StopState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            StopCity.DataSource = EndCityDataSource;
            StopCity.DisplayMember = "City";
            StopCity.ValueMember = "City";
        }

        private void cmb_NFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {

            ChangeInfo();
            cmb_Radius.SelectedValue = 30;
        }

        private void ChangeInfo()
        {
            labelE.Text = "[" + CarType.Text + "]";
            labelF.Text = "[" + CarSize.Text + "] 이상 ";

            if (cmb_NFilterType.SelectedIndex == 0)
            {
                cmb_NFilterType.Visible = true;
                label9.Visible = true;
                label10.Visible = false;
                cmb_Radius.Visible = true;

                cmb_NotificationGroup.Visible = false;

                string ClientAddr;

                ClientAddr = StartState.Text + " " + StartCity.Text;




                labelA.Visible = true;
                labelB.Visible = true;
                labelC.Visible = false;
                labelD.Visible = false;





                labelA.Text = "[" + ClientAddr + "]";
                labelB.Text = " 반경 " + cmb_Radius.SelectedValue + "km 내 공차로 알림전송 합니다.";
                labelB.ForeColor = Color.Red;





            }

            else if (cmb_NFilterType.SelectedIndex == 1)
            {
                cmb_NFilterType.Visible = true;
                cmb_Radius.Visible = false;
                label9.Visible = true;
                label10.Visible = false;
                cmb_NotificationGroup.Visible = true;

                string GroupName = string.Empty;
                if (cmb_NotificationGroup.SelectedIndex == 0)
                {
                    GroupName = "전체 ";
                }
                else
                {
                    GroupName = cmb_NotificationGroup.SelectedValue.ToString();
                }

                // label1.Text = "[" + LocalUser.Instance.LogInInfomation.UserName + "] " + GroupName + " 그룹 공차를 조회 합니다.";

                labelA.Visible = true;
                labelB.Visible = true;
                labelC.Visible = false;
                labelD.Visible = false;

                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    labelA.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";

                    if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                    {
                        labelA.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";
                    }
                }
                else
                {
                    labelA.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";

                }


                // labelA.Text = "[" + LocalUser.Instance.LogInInfomation.UserName + "] ";
                labelB.Text = GroupName + " 그룹 공차로 알림전송 합니다.";
                labelB.ForeColor = Color.Red;


            }
            else if (cmb_NFilterType.SelectedIndex == 2)
            {

                cmb_NFilterType.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                cmb_Radius.Visible = true;

                cmb_NotificationGroup.Visible = true;

                //  label1.Text = "[" + cmb_ParkState.SelectedValue + " " + cmb_ParkCity.SelectedValue + " " + cmb_ParkStreet.SelectedValue + "]" + " 반경 " + cmb_Radius.SelectedValue + "km 내 공차를 조회 합니다.";

                labelA.Visible = true;
                labelB.Visible = true;
                labelC.Visible = true;
                labelD.Visible = true;

                string GroupName = string.Empty;
                if (cmb_NotificationGroup.SelectedIndex == 0)
                {
                    GroupName = "전체 ";
                }
                else
                {
                    GroupName = cmb_NotificationGroup.SelectedValue.ToString();
                }
                string ClientAddr;

                ClientAddr = StartState.Text + " " + StartCity.Text;

                // labelA.Text = "[" + LocalUser.Instance.LogInInfomation.UserName + "] ";

                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    labelA.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";
                }
                else
                {
                    labelA.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";
                }

                labelB.Text = GroupName + "그룹과 ";
                labelB.ForeColor = Color.Red;
                labelC.Text = "[" + ClientAddr + "]";
                labelD.Text = " 반경 " + cmb_Radius.SelectedValue + "Km 내 공차로 알림전송 합니다.";
                labelD.ForeColor = Color.Red;
            }
        }

        private void cmb_Radius_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void cmb_NotificationGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void _AddClient()
        {

            StartTime.Value = StartTime.Value.Date.AddHours((int)StartTimeHour.SelectedValue).AddMinutes((int)StartTimeMinute.SelectedValue);
            StopTime.Value = StopTime.Value.Date.AddHours((int)StopTimeHour.SelectedValue).AddMinutes((int)StopTimeMinute.SelectedValue);
            var SelectedCustomer = _CustomerViewModelList[dataGridView2.SelectedRows[0].Index];
            int _CustomerId = SelectedCustomer.CustomerId;
            int _CustomerSubClientId = SelectedCustomer.SubClientId;
            int _PointMethod = SelectedCustomer.PointMethod;

            //좌표 가져오기
            var Address = SingleDataSet.Instance.AddressReferences.First(c => c.State == StartState.Text && c.City == StartCity.Text);
            var rPoint = mLocationHelper.GetLocactionFromAddress(Address);
            //Order 추가
            int OrderId = 0;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();

                //반복
                if (iCarCount < 1)
                    iCarCount = 1;
                for (int i = 0; i < iCarCount; i++)
                {
                    var Query = @"INSERT INTO Orders
               (StartTime, StartState, StartCity, Price, ClientPrice, Item, CreateTime, ClientId, StartDate, IsShared, CarType, CarSize, StopState, StopCity, PayLocation, CarCount, AcceptTime, Driver, DriverPhoneNo, 
               DriverCarModel, Remark, StartStreet, StopStreet, NotificationFilterType, NotificationRadius, NotificationGroupName, OrderStatus, DriverId, X, Y, OrderPhoneNo, NotificationState, 
               NotificationCity, NotificationStreet, NotificationX, NotificationY, StopTime, ItemSize,Wgubun,SourceType,CustomerId,PointMethod,Agubun,SubClientId,ClientUserId)
                VALUES  (@StartTime,@StartState,@StartCity,@Price, @ClientPrice,@Item,@CreateTime,@ClientId,@StartDate,@IsShared,@CarType,@CarSize,@StopState,@StopCity
                ,@PayLocation,@CarCount,@AcceptTime,@Driver,@DriverPhoneNo,@DriverCarModel,@Remark,@StartStreet,@StopStreet,@NotificationFilterType,
                @NotificationRadius,@NotificationGroupName,@OrderStatus,@DriverId,@X,@Y,@OrderPhoneNo,@NotificationState,@NotificationCity
                ,@NotificationStreet,@NotificationX,@NotificationY,@StopTime,@ItemSize,'',1,@CustomerId,@PointMethod,2,@SubClientId,@ClientUserId)
                SELECT @@IDENTITY";
                    var Command = cn.CreateCommand();
                    Command.CommandText = Query;


                    Command.Parameters.AddWithValue("@StartTime", StartTime.Value);
                    Command.Parameters.AddWithValue("@StartState", StartState.Text);
                    Command.Parameters.AddWithValue("@StartCity", StartCity.Text);
                    Command.Parameters.AddWithValue("@Price", iPrice);
                    Command.Parameters.AddWithValue("@ClientPrice", iClientPrice);
                    Command.Parameters.AddWithValue("@Item", Item.Text);
                    Command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                    Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    Command.Parameters.AddWithValue("@StartDate", StartTime.Value.Date);
                    Command.Parameters.AddWithValue("@IsShared", (bool)IsShared.SelectedValue);
                    Command.Parameters.AddWithValue("@CarType", (int)CarType.SelectedValue);
                    Command.Parameters.AddWithValue("@CarSize", (int)CarSize.SelectedValue);

                    Command.Parameters.AddWithValue("@StopState", StopState.Text);
                    Command.Parameters.AddWithValue("@StopCity", StopCity.Text);
                    Command.Parameters.AddWithValue("@PayLocation", (int)Location.SelectedValue);
                    Command.Parameters.AddWithValue("@CarCount", 1);
                    Command.Parameters.AddWithValue("@AcceptTime", DBNull.Value);
                    Command.Parameters.AddWithValue("@Driver", "");
                    Command.Parameters.AddWithValue("@DriverPhoneNo", "");
                    Command.Parameters.AddWithValue("@DriverCarModel", "");
                    Command.Parameters.AddWithValue("@Remark", "");
                    Command.Parameters.AddWithValue("@StartStreet", StartStreet.Text);
                    Command.Parameters.AddWithValue("@StopStreet", StopStreet.Text);
                    Command.Parameters.AddWithValue("@NotificationFilterType", (int)cmb_NFilterType.SelectedValue);
                    Command.Parameters.AddWithValue("@NotificationRadius", (int)cmb_Radius.SelectedValue);
                    #region


                    Command.Parameters.AddWithValue("@NotificationGroupName", cmb_NotificationGroup.SelectedValue.ToString());
                    Command.Parameters.AddWithValue("@OrderStatus",1);
                    Command.Parameters.AddWithValue("@DriverId", DBNull.Value);
                    Command.Parameters.AddWithValue("@X", rPoint.X);
                    Command.Parameters.AddWithValue("@Y", rPoint.Y);
                    Command.Parameters.AddWithValue("@OrderPhoneNo", PhoneNo);
                    Command.Parameters.AddWithValue("@NotificationState", "");
                    Command.Parameters.AddWithValue("@NotificationCity", "");
                    Command.Parameters.AddWithValue("@NotificationStreet", "");
                    Command.Parameters.AddWithValue("@NotificationX", rPoint.X);
                    Command.Parameters.AddWithValue("@NotificationY", rPoint.Y);
                    Command.Parameters.AddWithValue("@StopTime", StopTime.Value);

                    Command.Parameters.AddWithValue("@ItemSize", "");
                    Command.Parameters.AddWithValue("@CustomerId", _CustomerId);
                    Command.Parameters.AddWithValue("@PointMethod", _PointMethod);
                    Command.Parameters.AddWithValue("@SubClientId", DBNull.Value);
                    Command.Parameters.AddWithValue("@ClientUserId", DBNull.Value);
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            Command.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                        }
                    }
                    #endregion

                    OrderId = Convert.ToInt32(Command.ExecuteScalar());
                }


                cn.Close(); 
            }

            //Notification 추가
            #region Notification 추가
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                var iNotificationFilterType = (int)cmb_NFilterType.SelectedValue;
                var iNotificationRadius = (int)cmb_Radius.SelectedValue;
                var iOrderStatus = 1;
                var DistanceLimite = iNotificationRadius;
                var iNotificationGroupName = cmb_NotificationGroup.SelectedValue.ToString();
                var iStartState = StartState.Text;
                var iStartCity = StartCity.Text;
                var iCarType = CarType.SelectedValue.ToString();
                var iCarSize = CarSize.SelectedValue.ToString();


                var iStopState = StopState.Text;
                var iStopCity = StopCity.Text;
                var iPrice = Price.Text;


                if (iOrderStatus == 1)
                {
                    List<int> DriverIdList = new List<int>();
                    //행정구역 +- 10Km
                    if (iNotificationFilterType == 1 || iNotificationFilterType == 4)
                    {
                        var x1 = rPoint.X;
                        var y1 = rPoint.Y;
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();

                            var Query =
                                @"SELECT PreOrders.DriverId, X, Y
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            WHERE PreOrders.StopTime > GetDate() AND PreOrders.IsPreview = 0
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            ORDER BY ABS(@x1 - PreOrders.X) + ABS(@y1 - PreOrders.Y)";
                            var Commad = cn.CreateCommand();
                            Commad.CommandText = Query;
                            Commad.Parameters.AddWithValue("@x1", x1);
                            Commad.Parameters.AddWithValue("@y1", y1);

                            Commad.Parameters.AddWithValue("@CarType", iCarType);
                            Commad.Parameters.AddWithValue("@CarSize", iCarSize);

                            var Reader = Commad.ExecuteReader();
                            while (Reader.Read())
                            {
                                var X = Reader.GetDouble(1);
                                var Y = Reader.GetDouble(2);
                                var DriverId = Reader.GetInt32(0);

                                var Distance = mLocationHelper.DistanceFromGPS(X, Y, x1, y1);
                                if (Distance > DistanceLimite)
                                    break;
                                DriverIdList.Add(DriverId);
                            }
                            cn.Close();
                        }
                    }
                    //그룹
                    if (iNotificationFilterType == 3 || iNotificationFilterType == 4)
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();

                            var Query =
                                @"SELECT PreOrders.DriverId
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            JOIN DriverGroups ON Drivers.DriverId = DriverGroups.DriverId
                            WHERE StopTime > GetDate() AND IsPreview = 0
                            AND DriverGroups.ClientId = @ClientId
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            AND (@iNotificationGroupName = '0' OR DriverGroups.NAME = @iNotificationGroupName)";
                            var Commad = cn.CreateCommand();
                            Commad.CommandText = Query;
                            Commad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            Commad.Parameters.AddWithValue("@iNotificationGroupName", iNotificationGroupName);

                            Commad.Parameters.AddWithValue("@CarType", iCarType);
                            Commad.Parameters.AddWithValue("@CarSize", iCarSize);


                            var Reader = Commad.ExecuteReader();
                            while (Reader.Read())
                            {
                                var DriverId = Reader.GetInt32(0);
                                DriverIdList.Add(DriverId);
                            }
                            cn.Close();
                        }
                    }
                    if (DriverIdList.Count > 0)
                    {
                        var Table = new CMDataSet.NotificationsDataTable();
                        foreach (var driverId in DriverIdList.Distinct())
                        {
                            var Row = Table.NewNotificationsRow();
                            Row.FilterType = iNotificationFilterType;
                            Row.State = iStartState;
                            Row.City = iStartCity;
                            Row.Street = "";
                            Row.Radius = iNotificationRadius;
                            Row.GroupName = iNotificationGroupName;
                            Row.IsChecked = false;
                            Row.CreateTime = DateTime.Now;
                            Row.DriverId = driverId;
                            Row.ClientId = LocalUser.Instance.LogInInformation.ClientId;
                            Row.OrderId = OrderId;
                            Table.AddNotificationsRow(Row);
                        }
                        var Adapter = new CMDataSetTableAdapters.NotificationsTableAdapter();
                        Adapter.Update(Table);


                    }
                }
            }
            #endregion

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


                            var Table = new CMDataSet.MSG_DATADataTable();
                            foreach (var phoneNo in DriverMobileNoList.Distinct())
                            {
                                var Row = Table.NewMSG_DATARow();


                                Row.SENDER_KEY = "045bc76bbd0af10b869e9f7b388bb73d4870a3d8";
                                Row.PHONE = phoneNo.Replace("-", "");
                                Row.TMPL_CD = "K18-0005";
                                Row.SEND_MSG = "배차를 위한 화물정보 입니다." + "\n" +
                                                "배차를 받으시려면 하단 배차받기" + "\n" +
                                                "버튼을 클릭 하십시오." + "\n\n" +

                                                " * 상차 : " + StartTime.Text + " " + StartTimeHour.Text.Replace("시", "") + ":" + StartTimeMinute.Text.Replace("분", "") + " " + "\n" +
                                                " * 출발 : " + StartState.Text + " " + StartCity.Text + " " + "\n" +
                                                " * 도착 : " + StopState.Text + " " + StopCity.Text + " " + "\n" +
                                                " * 차종 : " + CarType.Text + "" + "\n" +
                                                " * 톤수 : " + CarSize.Text + "" + "\n" +
                                                " * 화물 : " + Item.Text + "" + "\n" +
                                                " * 금액 : " + Price.Text + "" + "\n\n" +

                                                " * 운송회사 : " + LocalUser.Instance.LogInInformation.ClientName + "" + "\n" +
                                                " * 문의전화 : " + PhoneNo + "";

                                Row.REQ_DATE = DateTime.Now;
                                Row.CUR_STATE = "0";
                                Row.SMS_TYPE = "N";
                                Row.ATTACHMENT_TYPE = "button";
                                Row.ATTACHMENT_NAME = "배차받기";
                                Row.ATTACHMENT_URL = $"http://m.cardpay.kr/Link?Id={OrderId}";
                                Table.AddMSG_DATARow(Row);


                            }

                            var Adapter = new CMDataSetTableAdapters.MSG_DATATableAdapter();
                            Adapter.Update(Table);
                        }
                    }
                }
            }
            #endregion
        }

        private void CarSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void CarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void Price_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
        }

        private void Price_Leave(object sender, EventArgs e)
        {
            decimal _d = 0;
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
            if (decimal.TryParse(((TextBox)sender).Text, out _d))
            {
                ((TextBox)sender).Text = _d.ToString("N0");
            }
        }

        private void Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        #region CUSTOMER
        class CustomerViewModel
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string PhoneNo { get; set; }
            public int PointMethod { get; set; }
            public int SubClientId { get; set; }
            public int Image1 { get; set; }
            public int Image2 { get; set; }
            public int Image3 { get; set; }
            public int Image4 { get; set; }
            public int Image5 { get; set; }
            public int Fee { get; set; }
        }
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>();
        private void InitCustomerTable()
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    var _CommandText = "SELECT CustomerId, SangHo, PhoneNo, ISNULL(PointMethod,0) AS PointMethod, ISNULL(SubClientId, 0) AS SubClientId, ISNULL(Image1, 0) AS Image1, ISNULL(Image2, 0) AS Image2, ISNULL(Image3, 0) AS Image3, ISNULL(Image4, 0) AS Image4, ISNULL(Image5, 0) AS Image5, ISNULL(Fee, 8) as Fee FROM Customers WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            _CommandText += " AND ClientUserId = @ClientUserId";
                            _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        }
                        else
                        {
                            _CommandText += " AND SubClientId = @SubClientId";
                            _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        }
                    }
                    _Command.CommandText = _CommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _CustomerViewModelList.Add(
                              new CustomerViewModel
                              {
                                  CustomerId = _Reader.GetInt32(0),
                                  Name = _Reader.GetStringN(1),
                                  PhoneNo = _Reader.GetStringN(2),
                                  PointMethod = _Reader.GetInt32(3),
                                  SubClientId = _Reader.GetInt32(4),
                                  Image1 = _Reader.GetInt32Z(5),
                                  Image2 = _Reader.GetInt32Z(6),
                                  Image3 = _Reader.GetInt32Z(7),
                                  Image4 = _Reader.GetInt32Z(8),
                                  Image5 = _Reader.GetInt32Z(9),
                                  Fee = _Reader.GetInt32Z(10),
                              });
                        }
                    }
                }
                _Connection.Close();
            }
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = _CustomerViewModelList;
        }
        #endregion

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            lbl_Customer.Text = "";
            if (dataGridView2.CurrentCell == null)
            {
                return;
            }
            if(dataGridView2.CurrentCell.RowIndex < 0)
            {
                return;
            }
            var SelectedCustomer = _CustomerViewModelList[dataGridView2.CurrentCell.RowIndex];
            lbl_Customer.Text = SelectedCustomer.Name;
            Image1.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image2.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image3.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image4.Image = Properties.Resources.ic_photo_white_48dp_2x;
            Image5.Image = Properties.Resources.ic_photo_white_48dp_2x;
            ImageName1.Text = "";
            ImageName2.Text = "";
            ImageName3.Text = "";
            ImageName4.Text = "";
            ImageName5.Text = "";
            if(SelectedCustomer.Image1 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + SelectedCustomer.Image1.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName1.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image1.Image = new Bitmap(_Stream);
                        }
                    }
                }
            }
            if (SelectedCustomer.Image2 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + SelectedCustomer.Image2.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName2.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image2.Image = new Bitmap(_Stream);
                        }
                    }
                }
            }
            if (SelectedCustomer.Image3 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + SelectedCustomer.Image3.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName3.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image3.Image = new Bitmap(_Stream);
                        }
                    }
                }
            }
            if (SelectedCustomer.Image4 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + SelectedCustomer.Image4.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName4.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image4.Image = new Bitmap(_Stream);
                        }
                    }
                }
            }
            if (SelectedCustomer.Image5 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + SelectedCustomer.Image5.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName5.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image5.Image = new Bitmap(_Stream);
                        }
                    }
                }
            }
        }

        private void StartCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void ItemSize_Leave(object sender, EventArgs e)
        {
            var d = 0d;
            if(double.TryParse(ItemSize.Text, out d))
            {
                CarSize.DataSource = Filter.Order.CarSizeList.Where(c=>c.Number > d / 1.1d || c.Number == 0).ToList();
            }
            else
            {
                CarSize.DataSource = Filter.Order.CarSizeList;
            }
        }
    }
}
