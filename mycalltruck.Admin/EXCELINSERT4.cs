using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;
using mycalltruck.Admin.Class.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using mycalltruck.Admin.Class.Extensions;
using System.Runtime.CompilerServices;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT4 : Form
    {
        public EXCELINSERT4()
        {
            InitializeComponent();
            InitializeCode();
            InitializeStore();
        }




        #region ACTION
        private void btn_Info_Click(object sender, EventArgs e)
        {
            ImportExcel();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            _Update();
        }

        private void newDGV1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var _Model = _BindingList[e.RowIndex];
            if (new int[] { colStartState.DisplayIndex, colStartCity.DisplayIndex, colStopState.DisplayIndex, colStopCity.DisplayIndex }.Contains(e.ColumnIndex))
            {
                SetPrices(_Model);
            }
            if (new int[] { colStartState.DisplayIndex, colStartCity.DisplayIndex, colStartStreet.DisplayIndex, colStopState.DisplayIndex, colStopCity.DisplayIndex, colStopStreet.DisplayIndex }.Contains(e.ColumnIndex))
            {
                var cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
                if (cell != null)
                {
                    var value = cell.Value == null ? "" : cell.Value.ToString();
                    cell.Items.Clear();
                    cell.Items.Add(value);
                    cell.Value = cell.Items[0];
                }
            }
            SetError(_Model);
        }

        private void chkFilterError_CheckedChanged(object sender, EventArgs e)
        {
            SetBindingList();
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPriceAllItem();
        }

        #endregion

        #region UPDATER
        int PayListCustomerId = 0;
        private void ImportExcel()
        {
            if (cmbCustomer.Items.Count == 0)
            {
                MessageBox.Show("화주 정보가 없습니다.", this.Text, MessageBoxButtons.OK);
                return;
            }

            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "xlsx";
            d.Filter = "엑셀 (*.xlsx)|*.xlsx";
            d.FilterIndex = 0;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.MYCAR;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExcelPackage _Excel = null;
                try
                {
                    _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
                }
                catch (Exception)
                {
                    MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                if (_Excel.Workbook.Worksheets.Count < 1)
                {
                    MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                _CoreList.Clear();
                chkFilterError.Checked = false;
                var _Sheet = _Excel.Workbook.Worksheets[1];
                const int TestIndex = 1;
                const int CarNoIndex = 5;
                const int ItemIndex = 9;
                const int DateIndex = 19;
                const int StartIndex = 11;
                const int EndIndex = 15;
                const int MemoIndex = 22;
                int RowIndex = 2;
                while (true)
                {
                    var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;

                    String CarNoText = _Sheet.Cells[RowIndex, CarNoIndex].Text;
                    String ItemText = _Sheet.Cells[RowIndex, ItemIndex].Text;
                    String DateText = _Sheet.Cells[RowIndex, DateIndex].Text;
                    String[] StartText = _Sheet.Cells[RowIndex, StartIndex].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    String[] EndText = _Sheet.Cells[RowIndex, EndIndex].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    String MemoText = _Sheet.Cells[RowIndex, MemoIndex].Text;
                    List<String> ErrorText = new List<string>();
                    var Added = new Model();
                    Added.CarNo = CarNoText;
                    Added.Item = ItemText;
                    Added.ExcelRowIndex = RowIndex;
                    var _CarModelQ = _CarModelList.Where(c => c.CarNo == CarNoText);
                    if (_CarModelQ.Any())
                    {
                        var _CarModel = _CarModelQ.First();
                        Added.DriverId = _CarModel.DriverId;
                        Added.CarNo = _CarModel.CarNo;
                        Added.Name = _CarModel.Name;
                        Added.BizNo = _CarModel.BizNo;
                        Added.PhoneNo = _CarModel.PhoneNo;
                        Added.CarType = _CarModel.CarType;
                        Added.CarSize = _CarModel.CarSize;
                        Added.DriverCarModel = _CarModel.DriverCarModel;
                    }
                    else
                    {
                        ErrorText.Add("차량번호를 확인할 수 없습니다.");
                    }
                    DateTime _Date = new DateTime();
                    if (DateTime.TryParse(DateText, out _Date))
                    {
                        Added.StartTime = _Date;
                        Added.StopTime = _Date;
                    }
                    else
                    {
                        ErrorText.Add("차량번호를 확인할 수 없습니다.");
                    }
                    if (StartText.Length > 0)
                    {
                        if (_AddressList.Any(c => c.State.StartsWith(StartText[0])))
                        {
                            Added.StartState = _AddressList.First(c => c.State.StartsWith(StartText[0])).State;
                            Added.IsStartStateValid = true;
                        }
                        else if (_AddressMatchList.Any(c => c.StateWord == StartText[0] && !c.IsAddressReference))
                        {
                            Added.StartState = _AddressMatchList.First(c => c.StateWord == StartText[0] && !c.IsAddressReference).StateTarget;
                            Added.IsStartStateValid = true;
                        }
                        else
                        {
                            Added.StartState = StartText[0];
                            ErrorText.Add("출발지를 확인 할 수 없습니다.");
                        }
                        if (StartText.Length > 1)
                        {
                            if (_AddressList.Any(c => c.State == Added.StartState && c.City.StartsWith(StartText[1])))
                            {
                                Added.StartCity = _AddressList.First(c => c.State == Added.StartState && c.City.StartsWith(StartText[1])).City;
                                Added.IsStartCityValid = true;
                            }
                            else if (_AddressMatchList.Any(c => c.CityWord == StartText[1] && !c.IsAddressReference))
                            {
                                Added.StartCity = _AddressMatchList.First(c => c.CityWord == StartText[1] && !c.IsAddressReference).StateTarget;
                                Added.IsStartCityValid = true;
                            }
                            else
                            {
                                Added.StartCity = StartText[1];
                            }
                            if (StartText.Length > 2)
                            {
                                Added.StartStreet = StartText[2];
                                Added.IsStartStreetValid = _AddressList.Any(c => c.State == Added.StartState && c.City == Added.StartCity && c.Street == Added.StartStreet);
                            }
                        }
                    }
                    else
                    {
                        ErrorText.Add("출발지를 확인 할 수 없습니다.");
                    }
                    if (EndText.Length > 0)
                    {
                        if (_AddressList.Any(c => c.State.StartsWith(EndText[0])))
                        {
                            Added.StopState = _AddressList.First(c => c.State.StartsWith(EndText[0])).State;
                            Added.IsStopStateValid = true;
                        }
                        else if (_AddressMatchList.Any(c => c.StateWord == EndText[0] && !c.IsAddressReference))
                        {
                            Added.StopState = _AddressMatchList.First(c => c.StateWord == EndText[0] && !c.IsAddressReference).StateTarget;
                            Added.IsStopStateValid = true;
                        }
                        else
                        {
                            Added.StopState = EndText[0];
                            ErrorText.Add("도착지를 확인 할 수 없습니다.");
                        }
                        if (EndText.Length > 1)
                        {
                            if (_AddressList.Any(c => c.State == Added.StopState && c.City.StartsWith(EndText[1])))
                            {
                                Added.StopCity = _AddressList.First(c => c.State == Added.StopState && c.City.StartsWith(EndText[1])).City;
                                Added.IsStopCityValid = true;
                            }
                            else if (_AddressMatchList.Any(c => c.CityWord == EndText[1] && !c.IsAddressReference))
                            {
                                Added.StopCity = _AddressMatchList.First(c => c.CityWord == EndText[1] && !c.IsAddressReference).StateTarget;
                                Added.IsStopCityValid = true;
                            }
                            else
                            {
                                Added.StopCity = EndText[1];
                            }
                            if (EndText.Length > 2)
                            {
                                Added.StopStreet = EndText[2];
                                Added.IsStopStreetValid = _AddressList.Any(c => c.State == Added.StopState && c.City == Added.StopCity && c.Street == Added.StopStreet);
                            }
                        }
                    }
                    else
                    {
                        ErrorText.Add("도착지를 확인 할 수 없습니다.");
                    }
                    colStartState.Items.Add(Added.StartState);
                    colStartCity.Items.Add(Added.StartCity);
                    colStartStreet.Items.Add(Added.StartStreet);
                    colStopState.Items.Add(Added.StopState);
                    colStopCity.Items.Add(Added.StopCity);
                    colStopStreet.Items.Add(Added.StopStreet);
                    Added.ETC = MemoText;
                    Added.Error = String.Join(" ", ErrorText);
                    _CoreList.Add(Added);
                    RowIndex++;
                }
                SetPriceAllItem();
                UpdateCount();
                SetBindingList();
            }
        }
        private void SetPrices(Model _Model)
        {
            _Model.Price = 0;
            _Model.ClientPrice = 0;
            var Q = _PriceReferenceList.Where(c => c.StartState == _Model.StartState);
            if (Q.Any())
            {
                if (Q.Any(c => c.StartCity == _Model.StartCity && c.EndState == _Model.StopState))
                {
                    Q = Q.Where(c => c.StartCity == _Model.StartCity);
                }
                if (Q.Any(c => c.EndState == _Model.StopState))
                {
                    Q = Q.Where(c => c.EndState == _Model.StopState);
                    if (Q.Any(c => c.EndCity == _Model.StopCity))
                    {
                        Q = Q.Where(c => c.EndCity == _Model.StopCity);
                    }
                    _Model.Price = Q.First().Price;
                    _Model.ClientPrice = Q.First().ClientPrice;
                }
                else
                {
                    return;
                }
            }
        }
        private void SetError(Model _Model)
        {
            List<String> ErrorText = new List<string>();
            var _CarModelQ = _CarModelList.Where(c => c.CarNo == _Model.CarNo);
            if (_CarModelQ.Any())
            {
                var _CarModel = _CarModelQ.First();
                _Model.DriverId = _CarModel.DriverId;
                _Model.Name = _CarModel.Name;
                _Model.BizNo = _CarModel.BizNo;
                _Model.PhoneNo = _CarModel.PhoneNo;
                _Model.CarType = _CarModel.CarType;
                _Model.CarSize = _CarModel.CarSize;
                _Model.DriverCarModel = _CarModel.DriverCarModel;
                
            }
            else
            {
                ErrorText.Add("차량번호를 확인할 수 없습니다.");
                _Model.DriverId = null;
                _Model.Name = "";
                _Model.BizNo = "";
                _Model.PhoneNo = "";
                _Model.CarType = 0;
                _Model.CarSize = 0;
                _Model.DriverCarModel = "";
            }
            if (!_AddressList.Any(c => c.State == _Model.StartState))
            {
                ErrorText.Add("출발지를 확인 할 수 없습니다.");
                _Model.IsStartStateValid = false;
                _Model.IsStartCityValid = false;
                _Model.IsStartStreetValid = false;
            }
            else
            {
                _Model.IsStartStateValid = true;
                _Model.IsStartCityValid = _AddressList.Any(c => c.State == _Model.StartState && c.City == _Model.StartCity);
                _Model.IsStartStreetValid = _AddressList.Any(c => c.State == _Model.StartState && c.City == _Model.StartCity && c.Street == _Model.StartStreet);
            }
            if (!_AddressList.Any(d => d.State == _Model.StopState))
            {
                ErrorText.Add("도착지를 확인 할 수 없습니다.");
                _Model.IsStopStateValid = false;
                _Model.IsStopCityValid = false;
                _Model.IsStopStreetValid = false;
            }
            else
            {
                _Model.IsStopStateValid = true;
                _Model.IsStopCityValid = _AddressList.Any(c => c.State == _Model.StopState && c.City == _Model.StopCity);
                _Model.IsStopStreetValid = _AddressList.Any(c => c.State == _Model.StopState && c.City == _Model.StopCity && c.Street == _Model.StopStreet);
            }
            if (_Model.Price == 0 || _Model.ClientPrice == 0)
            {
                ErrorText.Add("운송료가 0원 입니다.");
            }

            _Model.Error = String.Join(" ", ErrorText);
            UpdateCount();
            if (chkFilterError.Checked == true)
                SetBindingList();
        }
        private void UpdateCount()
        {
            lblTotalCount.Text = _CoreList.Count.ToString("N0");
            lblValidCount.Text = _CoreList.Count(c => String.IsNullOrEmpty(c.Error)).ToString("N0");
            lblErrorCount.Text = _CoreList.Count(c => !String.IsNullOrEmpty(c.Error)).ToString("N0");
            if (_CoreList.Any(c => String.IsNullOrEmpty(c.Error)))
                btn_Update.Enabled = true;
            else
                btn_Update.Enabled = false;
        }
        private void SetBindingList()
        {
            if (chkFilterError.Checked == false)
            {
                _BindingList.Clear();
                foreach (var _Model in _CoreList)
                {
                    _BindingList.Add(_Model);
                }
            }
            else
            {
                List<int> _RemoveIndexList = new List<int>();
                for (int i = 0; i < _BindingList.Count; i++)
                {
                    var _Model = _BindingList[i];
                    if (String.IsNullOrEmpty(_Model.Error))
                    {
                        _RemoveIndexList.Add(i);
                    }
                }
                _RemoveIndexList.Reverse();
                foreach (var Index in _RemoveIndexList)
                {
                    _BindingList.RemoveAt(Index);
                }
                foreach (var _Model in _CoreList.Where(c => !String.IsNullOrEmpty(c.Error)))
                {
                    if (!_BindingList.Contains(_Model))
                        _BindingList.Add(_Model);
                }
            }
        }
        private void _Update()
        {
            int CustomerId = ((BasicModel)cmbCustomer.SelectedItem).Id;
            var ClientName = "";
            var ClientNo = "";
            var PhoneNo = "";
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _ClientCommand = _Connection.CreateCommand())
                {
                    _ClientCommand.CommandText = "SELECT [Name], [BizNo], [PhoneNo] FROM [Clients] WHERE ClientId = @ClientId";
                    _ClientCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var _ClientReader = _ClientCommand.ExecuteReader();
                    if (!_ClientReader.Read())
                    {
                        throw new CustomException("데이터베이스 통신 중 오류가 발생하였습니다. 다시 한번 시도해 주시기 바랍니다.");
                    }
                    ClientName = _ClientReader.GetStringN(0);
                    ClientNo = _ClientReader.GetStringN(1);
                    PhoneNo = _ClientReader.GetStringN(2);
                }
                _Connection.Close();
            }
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _OrderCommand = _Connection.CreateCommand())
                {
                    _OrderCommand.CommandText =
                        @"INSERT INTO [Orders](StopTime, StartTime, StartDate, Price, ClientPrice, Etc, CarCount, Item, CreateTime
                        , ClientId, IsShared, RequestDateSC, DriverCarNo, CarType, CarSize, Driver, DriverPhoneNo, DriverCarModel, DriverId
                        , Remark, StartState, StartCity, StartStreet, StopState, StopCity, StopStreet, PayLocation, AcceptTime, NotificationFilterType
                        , NotificationState, NotificationCity, NotificationStreet, NotificationX, NotificationY, FPIS_F_DATE, Wgubun
                        , NotificationRadius, OrderStatus, X, Y, OrderPhoneNo, ItemSize, CustomerId, SubClientId, ClientUserId) output INSERTED.OrderId 
                        VALUES (@StopTime, @StartTime, @StartDate, @Price, @ClientPrice, @Etc, 1, @Item, @CreateTime
                        , @ClientId, 0, @RequestDateSC, @DriverCarNo, @CarType, @CarSize, @Driver, @DriverPhoneNo, @DriverCarModel, @DriverId
                        , @Remark, @StartState, @StartCity, N'', @StopState, @StopCity, N'', 0, @AcceptTime, 0
                        , N'', N'', N'', 0, 0, Null, N'PCALL'
                        , 0, 3, 0, 0, @OrderPhoneNo, '0', @CustomerId, @SubClientId, @ClientUserId)";
                    _OrderCommand.Parameters.Add("@StopTime", SqlDbType.DateTime);
                    _OrderCommand.Parameters.Add("@StartTime", SqlDbType.DateTime);
                    _OrderCommand.Parameters.Add("@StartDate", SqlDbType.DateTime);
                    _OrderCommand.Parameters.Add("@Price", SqlDbType.Decimal);
                    _OrderCommand.Parameters.Add("@ClientPrice", SqlDbType.Decimal);
                    _OrderCommand.Parameters.Add("@Etc", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@Item", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@CreateTime", SqlDbType.DateTime);
                    _OrderCommand.Parameters.Add("@ClientId", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@RequestDateSC", SqlDbType.DateTime);
                    _OrderCommand.Parameters.Add("@DriverCarNo", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@CarType", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@CarSize", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@Driver", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@DriverPhoneNo", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@DriverCarModel", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@DriverId", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@Remark", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@StartState", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@StartCity", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@StopState", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@StopCity", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@AcceptTime", SqlDbType.DateTime);
                    _OrderCommand.Parameters.Add("@OrderPhoneNo", SqlDbType.NVarChar);
                    _OrderCommand.Parameters.Add("@CustomerId", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@SubClientId", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@ClientUserId", SqlDbType.Int);

                    foreach (var _Model in _BindingList.Where(c => String.IsNullOrEmpty(c.Error)))
                    {
                        _OrderCommand.Parameters["@StopTime"].Value = _Model.StopTime.Date;
                        _OrderCommand.Parameters["@StartTime"].Value = _Model.StartTime.Date;
                        _OrderCommand.Parameters["@StartDate"].Value = _Model.StartTime.Date;
                        _OrderCommand.Parameters["@Price"].Value = _Model.Price;
                        _OrderCommand.Parameters["@ClientPrice"].Value = _Model.ClientPrice;
                        _OrderCommand.Parameters["@Etc"].Value = _Model.ETC;
                        _OrderCommand.Parameters["@Item"].Value = _Model.Item;
                        _OrderCommand.Parameters["@CreateTime"].Value = DateTime.Now;
                        _OrderCommand.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                        _OrderCommand.Parameters["@RequestDateSC"].Value = DateTime.Now;
                        _OrderCommand.Parameters["@DriverCarNo"].Value = _Model.CarNo;
                        _OrderCommand.Parameters["@CarType"].Value = _Model.CarType;
                        _OrderCommand.Parameters["@CarSize"].Value = _Model.CarSize;
                        _OrderCommand.Parameters["@DriverCarModel"].Value = _Model.DriverCarModel;
                        _OrderCommand.Parameters["@DriverPhoneNo"].Value = _Model.PhoneNo;
                        _OrderCommand.Parameters["@Driver"].Value = _Model.Name;
                        _OrderCommand.Parameters["@DriverId"].Value = _Model.DriverId;
                        _OrderCommand.Parameters["@Remark"].Value = _Model.BizNo;
                        _OrderCommand.Parameters["@StartState"].Value = _Model.StartState;
                        _OrderCommand.Parameters["@StartCity"].Value = _Model.StartCity;
                        _OrderCommand.Parameters["@StopState"].Value = _Model.StopState;
                        _OrderCommand.Parameters["@StopCity"].Value = _Model.StopCity;
                        _OrderCommand.Parameters["@AcceptTime"].Value = DateTime.Now;
                        _OrderCommand.Parameters["@OrderPhoneNo"].Value = PhoneNo;
                        _OrderCommand.Parameters["@CustomerId"].Value = CustomerId;
                        _OrderCommand.Parameters["@SubClientId"].Value = DBNull.Value;
                        _OrderCommand.Parameters["@ClientUserId"].Value = DBNull.Value;
                        if (LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            _OrderCommand.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                            if (LocalUser.Instance.LogInInformation.IsAgent)
                            {
                                _OrderCommand.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                            }
                        }
                        var _OrderId = (int)_OrderCommand.ExecuteScalar();

                    }
                }
                _Connection.Close();
            }
            MessageBox.Show("일괄등록 작업이 완료되었습니다.", Text);
            _CoreList.Clear();
            SetBindingList();
            UpdateCount();
            Close();
        }
        private void SetPriceAllItem()
        {
            if (cmbCustomer.SelectedIndex > -1)
            {
                int CustomerId = ((BasicModel)cmbCustomer.SelectedItem).Id;
                if (PayListCustomerId != CustomerId)
                {
                    PayListCustomerId = CustomerId;
                    _PriceReferenceList.Clear();
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _PriceCommand = _Connection.CreateCommand())
                        {
                            _PriceCommand.CommandText = "SELECT [Price], [ClientPrice], [StartState], [StartCity], [EndState], [EndCity] FROM [CustomerPriceReferences] JOIN CustomerPriceReferenceMasters ON CustomerPriceReferences.MasterId = CustomerPriceReferenceMasters.MasterId  WHERE CustomerPriceReferenceMasters.ClientId = @ClientId AND CustomerPriceReferenceMasters.CustomerId = @CustomerId";
                            _PriceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            _PriceCommand.Parameters.AddWithValue("@CustomerId", PayListCustomerId);
                            using ( SqlDataReader _Reader = _PriceCommand.ExecuteReader())
                            {
                                while (_Reader.Read())
                                {
                                    _PriceReferenceList.Add(new PriceReference
                                    {
                                        Price = _Reader.GetDecimal(0),
                                        ClientPrice = _Reader.GetDecimal(1),
                                        StartState = _Reader.GetStringN(2),
                                        StartCity = _Reader.GetStringN(3),
                                        EndState = _Reader.GetStringN(4),
                                        EndCity = _Reader.GetStringN(5),
                                    });
                                }
                            }
                        }
                        _Connection.Close();
                    }
                }
                foreach (var _Model in _CoreList)
                {
                    SetPrices(_Model);
                    SetError(_Model);
                }
            }
        }
        #endregion

        #region STORE
        private class Model : INotifyPropertyChanged
        {
            private int? _Driverid = null;
            private String _CarNo = "";
            private String _Name = "";
            private String _BizNo = "";
            private String _PhoneNo = "";
            private String _DriverCarModel = "";
            private int _CarType;
            private int _CarSize;
            private DateTime _StartTime;
            private DateTime _StopTime;
            private String _StartState = "";
            private String _StartCity = "";
            private String _StartStreet = "";
            private String _StopState = "";
            private String _StopCity = "";
            private String _StopStreet = "";
            private decimal _Price;
            private decimal _ClientPrice;
            private String _ETC = "";
            private String _Item = "";
            private String _Error = "";
            private int _ExcelRowIndex;

            public int? DriverId
            {
                get
                {
                    return _Driverid;
                }

                set
                {
                    _Driverid = value;
                }
            }

            public string CarNo
            {
                get
                {
                    return _CarNo;
                }

                set
                {
                    SetField(ref _CarNo, value);
                }
            }

            public string Name
            {
                get
                {
                    return _Name;
                }

                set
                {
                    SetField(ref _Name, value);
                }
            }

            public string BizNo
            {
                get
                {
                    return _BizNo;
                }

                set
                {
                    SetField(ref _BizNo, value);
                }
            }

            public string PhoneNo
            {
                get
                {
                    return _PhoneNo;
                }

                set
                {
                    SetField(ref _PhoneNo, value);
                }
            }

            public int CarType
            {
                get
                {
                    return _CarType;
                }

                set
                {
                    SetField(ref _CarType, value);
                }
            }

            public int CarSize
            {
                get
                {
                    return _CarSize;
                }

                set
                {
                    SetField(ref _CarSize, value);
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return _StartTime;
                }

                set
                {
                    SetField(ref _StartTime, value);
                }
            }

            public DateTime StopTime
            {
                get
                {
                    return _StopTime;
                }

                set
                {
                    SetField(ref _StopTime, value);
                }
            }

            public string StartState
            {
                get
                {
                    return _StartState;
                }

                set
                {
                    SetField(ref _StartState, value);
                }
            }

            public string StartCity
            {
                get
                {
                    return _StartCity;
                }

                set
                {
                    SetField(ref _StartCity, value);
                }
            }

            public string StartStreet
            {
                get
                {
                    return _StartStreet;
                }

                set
                {
                    SetField(ref _StartStreet, value);
                }
            }

            public string StopState
            {
                get
                {
                    return _StopState;
                }

                set
                {
                    SetField(ref _StopState, value);
                }
            }

            public string StopCity
            {
                get
                {
                    return _StopCity;
                }

                set
                {
                    SetField(ref _StopCity, value);
                }
            }

            public string StopStreet
            {
                get
                {
                    return _StopStreet;
                }

                set
                {
                    SetField(ref _StopStreet, value);
                }
            }

            public decimal Price
            {
                get
                {
                    return _Price;
                }

                set
                {
                    SetField(ref _Price, value);
                }
            }

            public decimal ClientPrice
            {
                get
                {
                    return _ClientPrice;
                }

                set
                {
                    SetField(ref _ClientPrice, value);
                }
            }

            public string ETC
            {
                get
                {
                    return _ETC;
                }

                set
                {
                    SetField(ref _ETC, value);
                }
            }

            public string Error
            {
                get
                {
                    return _Error;
                }

                set
                {
                    SetField(ref _Error, value);
                }
            }

            public string Item
            {
                get
                {
                    return _Item;
                }

                set
                {
                    SetField(ref _Item, value);
                }
            }

            public int ExcelRowIndex
            {
                get
                {
                    return _ExcelRowIndex;
                }

                set
                {
                    _ExcelRowIndex = value;
                }
            }

            public bool IsStartStateValid { get; set; }
            public bool IsStartCityValid { get; set; }
            public bool IsStartStreetValid { get; set; }
            public bool IsStopStateValid { get; set; }
            public bool IsStopCityValid { get; set; }
            public bool IsStopStreetValid { get; set; }

            public string DriverCarModel
            {
                get
                {
                    return _DriverCarModel;
                }

                set
                {
                    _DriverCarModel = value;
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
        private List<Model> _CoreList = new List<Model>();
        private BindingList<Model> _BindingList = new BindingList<Model>();
        private void InitializeStore()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _BindingList;
        }
        #endregion

        #region HELPER
        private class CarModel
        {
            public int DriverId { get; set; }
            public String CarNo { get; set; }
            public String Name { get; set; }
            public String BizNo { get; set; }
            public String PhoneNo { get; set; }
            public int CarType { get; set; }
            public int CarSize { get; set; }
            public String DriverCarModel { get; set; }
        }
        public class Address
        {
            public int Id { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
        }
        public class AddressMatch
        {
            public String StateWord { get; set; }
            public String StateTarget { get; set; }
            public String CityWord { get; set; }
            public String CityTarget { get; set; }
            public int? AddressReferenceId { get; set; }
            public bool IsAddressReference { get; set; }
        }
        public class PriceReference
        {
            public decimal Price { get; set; }
            public decimal ClientPrice { get; set; }
            public string StartState { get; set; }
            public string StartCity { get; set; }
            public string EndState { get; set; }
            public string EndCity { get; set; }
            public string StartText { get; set; }
            public string EndText { get; set; }
        }
        private List<BasicModel> _CarTypeList = new List<BasicModel>();
        private List<BasicModel> _CarSizeList = new List<BasicModel>();
        private List<BasicModel> _CustomerList = new List<BasicModel>();
        private List<CarModel> _CarModelList = new List<CarModel>();
        private List<Address> _AddressList = new List<Address>();
        private List<AddressMatch> _AddressMatchList = new List<AddressMatch>();
        private List<PriceReference> _PriceReferenceList = new List<PriceReference>();
        private void InitializeCode()
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _CarTypeCommand = _Connection.CreateCommand())
                {
                    _CarTypeCommand.CommandText = "SELECT Value, Name FROM [StaticOptions] WHERE Div = 'CarType' ORDER BY Seq";
                    using (SqlDataReader _Reader = _CarTypeCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _CarTypeList.Add(new BasicModel
                            {
                                Id = _Reader.GetInt32(0),
                                Text = _Reader.GetString(1),
                            });
                        }
                    }
                }
                using (SqlCommand _CarSizeCommand = _Connection.CreateCommand())
                {
                    _CarSizeCommand.CommandText = "SELECT Value, Name FROM [StaticOptions] WHERE Div = 'CarSize' ORDER BY Seq";
                    using (SqlDataReader _Reader = _CarSizeCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _CarSizeList.Add(new BasicModel
                            {
                                Id = _Reader.GetInt32(0),
                                Text = _Reader.GetString(1),
                            });
                        }
                    }
                }
                using (SqlCommand _CustomerPriceCommand = _Connection.CreateCommand())
                using (SqlCommand _CustomerCommand = _Connection.CreateCommand())
                {
                    List<int> DummyList = new List<int>();
                    _CustomerPriceCommand.CommandText = "SELECT ClientId, CustomerId FROM CustomerPriceReferenceMasters WHERE ClientId = @ClientId";
                    _CustomerPriceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _CustomerPriceCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            DummyList.Add(_Reader.GetInt32Z(1));
                        }
                    }
                    _CustomerCommand.CommandText = "SELECT CustomerId, Code, SangHo, [BizNo] FROM [Customers] WHERE @ClientId = ClientId";
                    _CustomerCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _CustomerCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            if(DummyList.Contains(_Reader.GetInt32(0)))
                            {
                                _CustomerList.Add(new BasicModel
                                {
                                    Id = _Reader.GetInt32(0),
                                    Text = $"{_Reader.GetStringN(2)}[{_Reader.GetStringN(1)}]",
                                    TextOption1 = $"{_Reader.GetStringN(3)}",
                                });
                            }
                        }
                    }
                }
                using (SqlCommand _ClientCommand = _Connection.CreateCommand())
                using (SqlCommand _ClientUserCommand = _Connection.CreateCommand())
                {
                    List<String> LoginIdList = new List<string>();
                    _ClientCommand.CommandText = "SELECT LoginId FROM [Clients] WHERE @ClientId = ClientId";
                    _ClientCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _ClientCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            LoginIdList.Add(_Reader.GetStringN(0));
                        }
                    }

                    _ClientUserCommand.CommandText = "SELECT LoginId FROM [ClientUsers] WHERE @ClientId = ClientId";
                    _ClientUserCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _ClientUserCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            LoginIdList.Add(_Reader.GetStringN(0));
                        }
                    }
                }
                using (SqlCommand _AddressCommand = _Connection.CreateCommand())
                {
                    _AddressCommand.CommandText = "SELECT AddressReferenceId, State, City, Street FROM AddressReferences";
                    using (SqlDataReader _Reader = _AddressCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _AddressList.Add(new Address
                            {
                                Id = _Reader.GetInt32(0),
                                State = _Reader.GetString(1),
                                City = _Reader.GetString(2),
                                Street = _Reader.GetString(3),
                            });
                        }
                    }
                }
                using (SqlCommand _AddressMatchCommand = _Connection.CreateCommand())
                {
                    _AddressMatchCommand.CommandText = "SELECT MatchId, StateWord, StateTarget, CityWord, CityTarget, AddressReferenceId, IsAddressReference FROM AddressMatches";
                    using (SqlDataReader _Reader = _AddressMatchCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _AddressMatchList.Add(new AddressMatch
                            {
                                StateWord = _Reader.GetStringN(1),
                                StateTarget = _Reader.GetStringN(2),
                                CityWord = _Reader.GetStringN(3),
                                CityTarget = _Reader.GetStringN(4),
                                AddressReferenceId = _Reader.GetInt32N(5),
                                IsAddressReference = _Reader.GetBoolean(6),
                            });
                        }
                    }
                }
                using (SqlCommand _DriverModelCommand = _Connection.CreateCommand())
                {
                    _DriverModelCommand.CommandText = "SELECT DriverId, [CarNo], [CarYear], [BizNo], [MobileNo], [CarType], [CarSize], [Name] FROM [Drivers] WHERE [CandidateId] = @ClientId";
                    _DriverModelCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _DriverModelCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            if (!_CarModelList.Any(c => c.CarNo == _Reader.GetStringN(1)))
                            {
                                _CarModelList.Add(new CarModel
                                {
                                    DriverId = _Reader.GetInt32(0),
                                    CarNo = _Reader.GetStringN(1),
                                    Name = _Reader.GetStringN(2),
                                    BizNo = _Reader.GetStringN(3),
                                    PhoneNo = _Reader.GetStringN(4),
                                    CarType = _Reader.GetInt32(5),
                                    CarSize = _Reader.GetInt32(6),
                                    DriverCarModel = _Reader.GetStringN(7),
                                });
                            }
                        }
                    }
                }
                _Connection.Close();
            }
            cmbCustomer.ValueMember = "Id";
            cmbCustomer.DisplayMember = "Text";
            cmbCustomer.DataSource = _CustomerList;
            if (cmbCustomer.Items.Count > 0)
            {
                cmbCustomer.SelectedIndex = 0;
            }
        }
        #endregion

        #region VIEW
        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == colNumber.DisplayIndex)
            {
                e.Value = (e.RowIndex + 1).ToString("N0");
            }
            else if (e.ColumnIndex == colCarType.DisplayIndex)
            {
                var _Model = _BindingList[e.RowIndex];
                if (_Model.DriverId > 0)
                    e.Value = _CarTypeList.FirstOrDefault(c => c.Id == ((int)e.Value)).Text;
                else
                    e.Value = "";
            }
            else if (e.ColumnIndex == colCarSize.DisplayIndex)
            {
                var _Model = _BindingList[e.RowIndex];
                if (_Model.DriverId > 0)
                    e.Value = _CarSizeList.FirstOrDefault(c => c.Id == ((int)e.Value)).Text;
                else
                    e.Value = "";
            }
            else if (e.ColumnIndex == colStartState.DisplayIndex || e.ColumnIndex == colStopState.DisplayIndex)
            {
                var _Value = e.Value == null ? "" : e.Value.ToString();
                PaintCellValid(e.CellStyle, _AddressList.Any(c => c.State == _Value));
            }
            else if (e.ColumnIndex == colStartCity.DisplayIndex
                || e.ColumnIndex == colStartStreet.DisplayIndex
                || e.ColumnIndex == colStopCity.DisplayIndex
                || e.ColumnIndex == colStopStreet.DisplayIndex)
            {
                var _Value = e.Value == null ? "" : e.Value.ToString();
                if (String.IsNullOrEmpty(_Value))
                {
                    PaintCellValid(e.CellStyle, true);
                }
                else
                {
                    var _Model = _BindingList[e.RowIndex];
                    if (e.ColumnIndex == colStartCity.DisplayIndex)
                    {
                        PaintCellValid(e.CellStyle, _AddressList.Any(c => c.State == _Model.StartState && c.City == _Model.StartCity));
                    }
                    else if (e.ColumnIndex == colStartStreet.DisplayIndex)
                    {
                        PaintCellValid(e.CellStyle, _AddressList.Any(c => c.State == _Model.StartState && c.City == _Model.StartCity && c.Street == _Model.StartStreet));
                    }
                    else if (e.ColumnIndex == colStopCity.DisplayIndex)
                    {
                        PaintCellValid(e.CellStyle, _AddressList.Any(c => c.State == _Model.StopState && c.City == _Model.StopCity));
                    }
                    else if (e.ColumnIndex == colStopStreet.DisplayIndex)
                    {
                        PaintCellValid(e.CellStyle, _AddressList.Any(c => c.State == _Model.StopState && c.City == _Model.StopCity && c.Street == _Model.StopStreet));
                    }
                }
            }
            else if (e.ColumnIndex == colCarNo.DisplayIndex)
            {
                var _Model = _BindingList[e.RowIndex];
                PaintCellValid(e.CellStyle, _Model.DriverId > 0);
            }
        }

        private void PaintCellValid(DataGridViewCellStyle _CellStyle, bool _IsValid)
        {
            if (_IsValid)
            {
                _CellStyle.ForeColor = newDGV1.DefaultCellStyle.ForeColor;
                _CellStyle.SelectionForeColor = newDGV1.DefaultCellStyle.SelectionForeColor;
            }
            else
            {
                _CellStyle.ForeColor = Color.OrangeRed;
                _CellStyle.SelectionForeColor = Color.OrangeRed;
            }
        }

        private void newDGV1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var cell = newDGV1.CurrentCell as DataGridViewComboBoxCell;
            if (cell == null || cell.RowIndex < 0)
                return;
            var _Model = _BindingList[cell.RowIndex];
            if (cell.ColumnIndex == colStartState.DisplayIndex | cell.ColumnIndex == colStopState.DisplayIndex)
            {
                var _ComboBox = (e.Control as ComboBox);
                var _State = cell.ColumnIndex == colStartState.DisplayIndex ? _Model.StartState : cell.ColumnIndex == colStopState.DisplayIndex ? _Model.StopState : "";
                _ComboBox.Items.Clear();
                bool _NeedValueAdd = true;
                foreach (var _Text in _AddressList.Select(c => c.State).Distinct())
                {
                    _ComboBox.Items.Add(_Text);
                    if (_Text == _State)
                    {
                        _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                        _NeedValueAdd = false;
                    }
                }
                if (_NeedValueAdd)
                {
                    _ComboBox.Items.Add(_State);
                    _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                }
            }
            else if (cell.ColumnIndex == colStartCity.DisplayIndex | cell.ColumnIndex == colStopCity.DisplayIndex)
            {
                var _ComboBox = (e.Control as ComboBox);
                var _State = cell.ColumnIndex == colStartCity.DisplayIndex ? _Model.StartState : cell.ColumnIndex == colStopCity.DisplayIndex ? _Model.StopState : "";
                var _City = cell.ColumnIndex == colStartCity.DisplayIndex ? _Model.StartCity : cell.ColumnIndex == colStopCity.DisplayIndex ? _Model.StopCity : "";
                _ComboBox.Items.Clear();
                bool _NeedValueAdd = true;
                _ComboBox.Items.Add("");
                if (String.IsNullOrEmpty(_City))
                {
                    _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                    _NeedValueAdd = false;
                }
                foreach (var _Text in _AddressList.Where(c => c.State == _State).Select(c => c.City).Distinct())
                {
                    _ComboBox.Items.Add(_Text);
                    if (_Text == _City)
                    {
                        _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                        _NeedValueAdd = false;
                    }
                }
                if (_NeedValueAdd)
                {
                    _ComboBox.Items.Add(_City);
                    _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                }
            }
            else if (cell.ColumnIndex == colStartStreet.DisplayIndex | cell.ColumnIndex == colStopStreet.DisplayIndex)
            {
                var _ComboBox = (e.Control as ComboBox);
                var _State = cell.ColumnIndex == colStartStreet.DisplayIndex ? _Model.StartState : cell.ColumnIndex == colStopStreet.DisplayIndex ? _Model.StopState : "";
                var _City = cell.ColumnIndex == colStartStreet.DisplayIndex ? _Model.StartCity : cell.ColumnIndex == colStopStreet.DisplayIndex ? _Model.StopCity : "";
                var _Street = cell.ColumnIndex == colStartStreet.DisplayIndex ? _Model.StartStreet : cell.ColumnIndex == colStopStreet.DisplayIndex ? _Model.StopStreet : "";
                _ComboBox.Items.Clear();
                bool _NeedValueAdd = true;
                _ComboBox.Items.Add("");
                if (String.IsNullOrEmpty(_Street))
                {
                    _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                    _NeedValueAdd = false;
                }
                foreach (var _Text in _AddressList.Where(c => c.State == _State && c.City == _City).Select(c => c.Street).Distinct())
                {
                    _ComboBox.Items.Add(_Text);
                    if (_Text == _Street)
                    {
                        _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                        _NeedValueAdd = false;
                    }
                }
                if (_NeedValueAdd)
                {
                    _ComboBox.Items.Add(_Street);
                    _ComboBox.SelectedIndex = _ComboBox.Items.Count - 1;
                }
            }
        }

        private void newDGV1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
        #endregion

    }
}
