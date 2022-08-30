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
using System.Threading;

namespace mycalltruck.Admin
{
    public partial class FrmTradeNew2 : Form
    {
        DateTime _BeginDate = DateTime.Now;
        DateTime _EndDate = DateTime.Now;
        DESCrypt m_crypt = null;
        bool AllowTaxBool = false;
        bool CustomerPay = false;
        DriverRepository mDriverRepository = new DriverRepository();
        
        public String _CarNo { get; set; }
        #region ACTION
        public FrmTradeNew2()
        {
            InitializeComponent();
            InitializeStorage();

            dtp_To.Value = DateTime.Now.Date;
            dtp_From.Value = DateTime.Now.Date.AddMonths(-1);
            m_crypt = new DESCrypt("12345678");

            _InitCmb();
            cmb_Search.SelectedIndex = 0;

            if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                lbl_Tax.Visible = false;
                rdb_Tax0.Visible = false;
                rdb_Tax1.Visible = false;
            }
        }

        private void FrmTradeNew2_Load(object sender, EventArgs e)
        {
            ClientLoad();
            DriverRepository mDriverRepository = new DriverRepository();
           // DataLoad();
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
            Dictionary<string, string> Smonth = new Dictionary<string, string>
            {

                { "당월", "당월" },
                { "전월", "전월" },
                { "지정", "지정" }
            };

            cmbSMonth.DataSource = new BindingSource(Smonth, null);
            cmbSMonth.DisplayMember = "Value";
            cmbSMonth.ValueMember = "Key";

            cmbSMonth.SelectedIndex = 0;
        }

        #region UPDATE
        private void _Update()
        {
            var _Models = _ModelList.Where(c => c.Selected && !c.IsRefenece && (c.BizNo != "" && c.PayAccountNo != ""));
            if (_Models.Count() == 0)
            {
                MessageBox.Show("등록할 건을 먼저 선택하여 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var _RequestDate = DateTime.Now;
            var _CEO = "";
            var _Uptae = "";
            var _Upjong = "";
            var _Email = "";
            var _Name = "";
            var _BizNo = "";
            var _Address = "";


            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        _Command.CommandText = "SELECT CEO, Uptae, Upjong, Email, Name, BizNo, AddressState + ' ' + AddressCity + ' ' + AddressDetail FROM SubClients WHERE SubClientId = @SubClientId";
                        _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                    }
                    else
                    {
                        _Command.CommandText = "SELECT CEO, Uptae, Upjong, Email, Name, BizNo, AddressState + ' ' + AddressCity + ' ' + AddressDetail FROM CLIENTS WHERE ClientId = @ClientId";
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    }
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        if (_Reader.Read())
                        {
                            _CEO = _Reader.GetStringN(0);
                            _Uptae = _Reader.GetStringN(1);
                            _Upjong = _Reader.GetStringN(2);
                            _Email = _Reader.GetStringN(3);
                            _Name = _Reader.GetStringN(4);
                            _BizNo = _Reader.GetStringN(5);
                            _Address = _Reader.GetStringN(6);
                        }
                    }
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                using (SqlCommand _DriverCommand = _Connection.CreateCommand())
                using (SqlCommand _OrderCommand = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"INSERT INTO Trades (DriverId,ClientId,BeginDate,EndDate,Item,Price,VAT,Amount,PayBankName,PayBankCode,PayAccountNo,PayInputName,CEO,Uptae,Upjong,Email,Name,BizNo,Address,PayState,PayDate,RequestDate,UseTax,HasETax,AllowAcc,HasAcc,CustomerAccId,SourceType,AcceptCount,SubClientId,ClientUserId,PointMethod,Fee,Fee_VAT,TransportDate,StartState,StopState) OUTPUT INSERTED.TradeId
                        VALUES (@DriverId,@ClientId,@BeginDate,@EndDate,@Item,@Price,@VAT,@Amount,@PayBankName,@PayBankCode,@PayAccountNo,@PayInputName,@CEO,@Uptae,@Upjong,@Email,@Name,@BizNo,@Address,2,GETDATE(),@RequestDate,1,0,0,@HasAcc,0,0,@AcceptCount,@SubClientId,@ClientUserId,@PointMethod,@Fee,@Fee_VAT,@TransportDate,@StartState,@StopState)";
                    _OrderCommand.CommandText = "UPDATE Orders SET TradeId = @TradeId WHERE OrderId = @OrderId";
                    _OrderCommand.Parameters.Add("@TradeId", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@OrderId", SqlDbType.Int);

                    _DriverCommand.CommandText = "UPDATE DriverInstances SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                    _DriverCommand.Parameters.Add("@Driverid", SqlDbType.Int);
                    _DriverCommand.Parameters.Add("@Mizi", SqlDbType.Decimal);
                    _DriverCommand.Parameters.Add("@ClientId", SqlDbType.Int);

                    _Command.Parameters.Add("@DriverId", SqlDbType.Int);
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                    _Command.Parameters.Add("@Item", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@BeginDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@EndDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@Price", SqlDbType.Decimal);
                    _Command.Parameters.Add("@VAT", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Amount", SqlDbType.Decimal);
                    _Command.Parameters.Add("@PayBankName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayBankCode", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayAccountNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayInputName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@DriverName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CEO", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Uptae", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Upjong", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Email", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Name", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Address", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@RequestDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@HasAcc", SqlDbType.Int);
                    _Command.Parameters.Add("@AcceptCount", SqlDbType.Int);
                    _Command.Parameters.Add("@SubClientId", SqlDbType.Int);
                    _Command.Parameters.Add("@ClientUserId", SqlDbType.Int);
                    //_Command.Parameters.Add("@CustomerId", SqlDbType.Int);
                    _Command.Parameters.Add("@PointMethod", SqlDbType.Int);
                    _Command.Parameters.Add("@Fee", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Fee_VAT", SqlDbType.Decimal);
                    _Command.Parameters.Add("@TransportDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@StartState", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@StopState", SqlDbType.NVarChar);

                    Dictionary<int, DriverRepository.Driver> TempDriverDictionary = new Dictionary<int, DriverRepository.Driver>();

                    if(rdoSplit.Checked)
                    {
                        foreach (var _Model in _Models)
                        {
                            DriverRepository.Driver _Driver = null;
                            if (TempDriverDictionary.ContainsKey(_Model.DriverId))
                            {
                                _Driver = TempDriverDictionary[_Model.DriverId];
                            }
                            else
                            {
                                _Driver = mDriverRepository.NoGetDriver(_Model.DriverId);
                                if (_Driver == null)
                                    continue;
                                else
                                {
                                    TempDriverDictionary.Add(_Model.DriverId, _Driver);
                                }
                            }
                            String PayAccountNo = m_crypt.Decrypt(_Driver.PayAccountNo).Replace("\0", string.Empty).Trim();
                            _Command.Parameters["@DriverId"].Value = _Model.DriverId;
                            _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                            _Command.Parameters["@Item"].Value = "운송료집계";
                            _Command.Parameters["@BeginDate"].Value = _BeginDate;
                            _Command.Parameters["@EndDate"].Value = _EndDate;
                            _Command.Parameters["@Price"].Value = _Model.Price;
                            _Command.Parameters["@VAT"].Value = _Model.VAT;
                            _Command.Parameters["@Amount"].Value = _Model.Amount;
                            _Command.Parameters["@PayBankName"].Value = _Driver.PayBankName;
                            _Command.Parameters["@PayBankCode"].Value = _Driver.PayBankCode;
                            _Command.Parameters["@PayAccountNo"].Value = PayAccountNo;
                            _Command.Parameters["@PayInputName"].Value = _Driver.PayInputName;
                            _Command.Parameters["@DriverName"].Value = _Driver.Name;
                            _Command.Parameters["@CEO"].Value = _CEO;
                            _Command.Parameters["@Uptae"].Value = _Uptae;
                            _Command.Parameters["@Upjong"].Value = _Upjong;
                            _Command.Parameters["@Email"].Value = _Email;
                            _Command.Parameters["@Name"].Value = _Name;
                            _Command.Parameters["@BizNo"].Value = _BizNo;
                            _Command.Parameters["@Address"].Value = _Address;
                            _Command.Parameters["@RequestDate"].Value = DateTime.Now;
                            _Command.Parameters["@AcceptCount"].Value = _Model.Count;
                            _Command.Parameters["@SubClientId"].Value = DBNull.Value;
                            _Command.Parameters["@ClientUserId"].Value = DBNull.Value;
                            _Command.Parameters["@PointMethod"].Value = _Model.PointMethod;
                            _Command.Parameters["@Fee"].Value = _Model.Fee;
                            _Command.Parameters["@Fee_VAT"].Value = _Model.Fee_VAT;
                            _Command.Parameters["@TransportDate"].Value = _Model.OrderDate;
                            _Command.Parameters["@StartState"].Value = _Model.StartName;
                            _Command.Parameters["@StopState"].Value = _Model.StopName;


                            //제공이 아닐때
                            if (_Driver.ServiceState != 1)
                            {
                                //현금
                                _Command.Parameters["@HasAcc"].Value = 0;
                            }
                            else
                            {
                                //카드
                                _Command.Parameters["@HasAcc"].Value = 1;

                            }

                            if (LocalUser.Instance.LogInInformation.IsSubClient)
                            {
                                _Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                                if (LocalUser.Instance.LogInInformation.IsAgent)
                                {
                                    _Command.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                                }
                            }
                            //_Command.Parameters["@CustomerId"].Value = _Model.First().CustomerId;
                            var _TradeId = Convert.ToInt32(_Command.ExecuteScalar());
                            //foreach (var _ModelItem in _Model)
                            //{
                            //    foreach (var _OrderId in _ModelItem.OrderIdList)
                            //    {
                            //        _OrderCommand.Parameters["@TradeId"].Value = _TradeId;
                            //        _OrderCommand.Parameters["@OrderId"].Value = _OrderId;
                            //        _OrderCommand.ExecuteNonQuery();
                            //    }

                            //    _DriverCommand.Parameters["@Mizi"].Value = _ModelItem.Amount;
                            //    _DriverCommand.Parameters["@DriverId"].Value = _ModelItem.DriverId;
                            //    _DriverCommand.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                            //    _DriverCommand.ExecuteNonQuery();


                            //}

                            _OrderCommand.Parameters["@TradeId"].Value = _TradeId;
                            _OrderCommand.Parameters["@OrderId"].Value = _Model.OrderId;
                            _OrderCommand.ExecuteNonQuery();


                            _DriverCommand.Parameters["@Mizi"].Value = _Model.Amount;
                            _DriverCommand.Parameters["@DriverId"].Value = _Model.DriverId;
                            _DriverCommand.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                            _DriverCommand.ExecuteNonQuery();


                        }
                    }
                    else
                    {
                        foreach (var _Model in _Models.GroupBy(c => c.DriverId))
                        {
                            DriverRepository.Driver _Driver = null;
                            if (TempDriverDictionary.ContainsKey(_Model.Key))
                            {
                                _Driver = TempDriverDictionary[_Model.Key];
                            }
                            else
                            {
                                _Driver = mDriverRepository.NoGetDriver(_Model.Key);
                                if (_Driver == null)
                                    continue;
                                else
                                {
                                    TempDriverDictionary.Add(_Model.Key, _Driver);
                                }
                            }
                            String PayAccountNo = m_crypt.Decrypt(_Driver.PayAccountNo).Replace("\0", string.Empty).Trim();
                            _Command.Parameters["@DriverId"].Value = _Model.Key;
                            _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                            _Command.Parameters["@Item"].Value = "운송료집계";
                            _Command.Parameters["@BeginDate"].Value = _BeginDate;
                            _Command.Parameters["@EndDate"].Value = _EndDate;
                            _Command.Parameters["@Price"].Value = _Model.Sum(c => c.Price);
                            _Command.Parameters["@VAT"].Value = _Model.Sum(c => c.VAT);
                            _Command.Parameters["@Amount"].Value = _Model.Sum(c => c.Amount);
                            _Command.Parameters["@PayBankName"].Value = _Driver.PayBankName;
                            _Command.Parameters["@PayBankCode"].Value = _Driver.PayBankCode;
                            _Command.Parameters["@PayAccountNo"].Value = PayAccountNo;
                            _Command.Parameters["@PayInputName"].Value = _Driver.PayInputName;
                            _Command.Parameters["@DriverName"].Value = _Driver.Name;
                            _Command.Parameters["@CEO"].Value = _CEO;
                            _Command.Parameters["@Uptae"].Value = _Uptae;
                            _Command.Parameters["@Upjong"].Value = _Upjong;
                            _Command.Parameters["@Email"].Value = _Email;
                            _Command.Parameters["@Name"].Value = _Name;
                            _Command.Parameters["@BizNo"].Value = _BizNo;
                            _Command.Parameters["@Address"].Value = _Address;
                            _Command.Parameters["@RequestDate"].Value = DateTime.Now;
                            _Command.Parameters["@AcceptCount"].Value = _Model.Sum(c => c.Count);
                            _Command.Parameters["@SubClientId"].Value = DBNull.Value;
                            _Command.Parameters["@ClientUserId"].Value = DBNull.Value;
                            _Command.Parameters["@PointMethod"].Value = _Model.First().PointMethod;
                            _Command.Parameters["@Fee"].Value = _Model.Sum(c => c.Fee);
                            _Command.Parameters["@Fee_VAT"].Value = _Model.Sum(c => c.Fee_VAT);
                            _Command.Parameters["@TransportDate"].Value = _Model.First().OrderDate;
                            _Command.Parameters["@StartState"].Value = _Model.First().StartName;
                            _Command.Parameters["@StopState"].Value = _Model.First().StopName;


                            //제공이 아닐때
                            if (_Driver.ServiceState != 1)
                            {
                                //현금
                                _Command.Parameters["@HasAcc"].Value = 0;
                            }
                            else
                            {
                                //카드
                                _Command.Parameters["@HasAcc"].Value = 1;

                            }

                            if (LocalUser.Instance.LogInInformation.IsSubClient)
                            {
                                _Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                                if (LocalUser.Instance.LogInInformation.IsAgent)
                                {
                                    _Command.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                                }
                            }
                            //_Command.Parameters["@CustomerId"].Value = _Model.First().CustomerId;
                            var _TradeId = Convert.ToInt32(_Command.ExecuteScalar());
                            foreach (var _ModelItem in _Model)
                            {
                                foreach (var _OrderId in _ModelItem.OrderIdList)
                                {
                                    _OrderCommand.Parameters["@TradeId"].Value = _TradeId;
                                    _OrderCommand.Parameters["@OrderId"].Value = _OrderId;
                                    _OrderCommand.ExecuteNonQuery();
                                }

                                _DriverCommand.Parameters["@Mizi"].Value = _ModelItem.Amount;
                                _DriverCommand.Parameters["@DriverId"].Value = _ModelItem.DriverId;
                                _DriverCommand.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                                _DriverCommand.ExecuteNonQuery();


                            }


                        }
                    }
                    
                }
                _Connection.Close();
            }

            try
            {

                DataLoad();
                //Close();
            }
            catch
            {


            }
        }
        #endregion

        #region STORAGE
        class Model : INotifyPropertyChanged
        {
            public int DriverId { get; set; }
            public int Count { get; set; }
            public decimal Price { get { return _Price; } set { SetField(ref _Price, value); } }
            public decimal ClientPrice { get { return _ClientPrice; } set { SetField(ref _ClientPrice, value); } }
            public decimal VAT { get { return _VAT; } set { SetField(ref _VAT, value); } }
            public decimal Fee { get { return _Fee; } set { SetField(ref _Fee, value); } }
            public decimal Fee_VAT { get { return _Fee_VAT; } set { SetField(ref _Fee_VAT, value); } }
            public decimal Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            public string LoginId { get; set; }
            public string CarNo { get; set; }
            public string CarYear { get; set; }
            public string BizNo { get; set; }
            public string PayAccountNo { get; set; }
            public string StartName { get; set; }
            public string StopName { get; set; }
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public string PayBankName { get; set; }
            public string PayBankCode { get; set; }
            public string PayInputName { get; set; }
            public string Name { get; set; }
            public string Ceo { get; set; }

            private decimal _Price = 0;
            private decimal _ClientPrice = 0;
            private decimal _VAT = 0;
            private decimal _Fee = 0;
            private decimal _Fee_VAT = 0;
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

            public List<int> OrderIdList { get; set; } = new List<int>();
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
            AllSelect.Checked = false;
            _ModelList.Clear();




            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    List<String> WhereStringList = new List<string>();


                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            _Command.CommandText = "SELECT Orders.DriverId, ISNULL(TradePrice, 0), OrderId, PointMethod, Orders.CustomerId, ISNULL(SalesPrice, 0), LoginId, CarNo, CarYear, Orders.PayLocation ,Orders.Customer,Orders.CreateTime,ISNULL(Drivers.BizNo,'') as BizNo ,ISNULL(Drivers.PayAccountNo,'') as PayAccountNo,isnull(Orders.StartName,'') AS StartName ,ISNULL(Orders.StopName,'') as StopName,ISNULL(Drivers.Name,'') as Name ,ISNULL(Drivers.CEO,'') as CEO FROM Orders JOIN Drivers ON Orders.DriverId = Drivers.DriverId WHERE CreateTime >= @Begin AND CreateTime < @End AND Orders.ClientUserId = @ClientUserId AND TradeId IS NULL AND OrderStatus = 3 AND Orders.DriverId IS NOT NULL AND ISNULL(TradePrice, 0) > 0 ";
                            _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        }
                        else
                        {
                            _Command.CommandText = "SELECT Orders.DriverId, ISNULL(TradePrice, 0), OrderId, PointMethod, Orders.CustomerId, ISNULL(SalesPrice, 0), LoginId, CarNo, CarYear, Orders.PayLocation,Orders.Customer ,Orders.CreateTime,ISNULL(Drivers.BizNo,'') as BizNo ,ISNULL(Drivers.PayAccountNo,'') as PayAccountNo,isnull(Orders.StartName,'') AS StartName ,ISNULL(Orders.StopName,'') as StopName,ISNULL(Drivers.Name,'') as Name ,ISNULL(Drivers.CEO,'') as CEO FROM Orders JOIN Drivers ON Orders.DriverId = Drivers.DriverId WHERE CreateTime >= @Begin AND CreateTime < @End AND Orders.SubClientId = @SubClientId AND TradeId IS NULL AND OrderStatus = 3 AND Orders.DriverId IS NOT NULL AND ISNULL(TradePrice, 0) > 0";
                            _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        }
                    }
                    else
                    {
                        _Command.CommandText = "SELECT Orders.DriverId, ISNULL(TradePrice, 0), OrderId, PointMethod, Orders.CustomerId, ISNULL(SalesPrice, 0), LoginId, CarNo, CarYear, Orders.PayLocation,Orders.Customer ,Orders.CreateTime ,ISNULL(Drivers.BizNo,'') as BizNo ,ISNULL(Drivers.PayAccountNo,'') as PayAccountNo,isnull(Orders.StartName,'') AS StartName ,ISNULL(Orders.StopName,'') as StopName,ISNULL(Drivers.PayBankName,'') as PayBankName,ISNULL(Drivers.PayBankCode,'') as PayBankCode,ISNULL(Drivers.PayInputName,'') as PayInputName,ISNULL(Drivers.Name,'') as Name ,ISNULL(Drivers.CEO,'') as CEO FROM Orders JOIN Drivers ON Orders.DriverId = Drivers.DriverId WHERE CreateTime >= @Begin AND CreateTime < @End AND Orders.ClientId = @ClientId " +
                            "AND TradeId IS NULL " +
                            "AND OrderStatus = 3 AND Orders.DriverId IS NOT NULL AND ISNULL(TradePrice, 0) > 0 ";
                        // "AND Drivers.ServiceState <> 5 ";
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    }

                    //_Command.CommandText += Environment.NewLine + " AND Drivers.ServiceState != 5 ";



                    if (cmb_Search.Text == "상호")
                    {
                        //Drivers.Name
                        _Command.CommandText += Environment.NewLine + " AND Drivers.Name LIKE '%'+ @Customer + '%' Order By Orders.CreateTime Desc";
                        //_Command.CommandText += Environment.NewLine + " AND Orders.Customer LIKE '%'+ @Customer + '%' Order By Orders.CreateTime Desc";
                        _Command.Parameters.AddWithValue("@Customer", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "차주명")
                    {
                        _Command.CommandText += Environment.NewLine + " AND CarYear LIKE '%'+ @CarYear + '%' Order By Orders.CreateTime Desc";
                        _Command.Parameters.AddWithValue("@CarYear", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "차량번호")
                    {
                        _Command.CommandText += Environment.NewLine + " AND CarNo LIKE '%'+ @CarNo + '%' Order By Orders.CreateTime Desc";
                        _Command.Parameters.AddWithValue("@CarNo", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "핸드폰번호")
                    {
                        _Command.CommandText += Environment.NewLine + " AND REPLACE(Drivers.MobileNo,'-','') LIKE '%'+ @MobileNo + '%' Order By Orders.CreateTime Desc";
                        _Command.Parameters.AddWithValue("@MobileNo", txt_Search.Text.Replace("-", ""));
                    }
                    //else if (cmb_Search.Text == "화주명")
                    //{
                    //    _Command.CommandText += Environment.NewLine + " AND Orders.Customer LIKE '%'+ @Customer + '%' Order By Orders.CreateTime Desc";
                    //    _Command.Parameters.AddWithValue("@Customer", txt_Search.Text.Replace("-", ""));
                    //}


                    else
                    {
                        _Command.CommandText += Environment.NewLine + "  Order By Orders.CreateTime Desc";

                    }

                    _Command.Parameters.AddWithValue("@Begin", dtp_From.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtp_To.Value.Date.AddDays(1));



                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var _DriverId = _Reader.GetInt32Z(0);

                            var _Price = _Reader.GetDecimal(1);
                            var _ClientPrice = _Reader.GetDecimal(5);

                            var _OrderId = _Reader.GetInt32(2);
                            Model _Model = null;

                            string tempString = string.Empty;
                            tempString = _Reader.GetString(13);
                            string _PayAccountNo = "";
                            try
                            {
                                _PayAccountNo = m_crypt.Decrypt(tempString);

                            }
                            catch
                            {
                                _PayAccountNo = tempString;

                            }


                            if (CustomerPay || rdoSplit.Checked == true)
                            {
                                _Model = new Model()
                                {
                                    DriverId = _DriverId,
                                    PointMethod = _Reader.GetInt32Z(3),
                                    CustomerId = _Reader.GetInt32Z(4),
                                    LoginId = _Reader.GetString(6),
                                    CarNo = _Reader.GetString(7),
                                    CarYear = _Reader.GetString(8),
                                    OrderDate = _Reader.GetDateTime(11),
                                    BizNo = _Reader.GetString(12),
                                    PayAccountNo = _PayAccountNo.Replace("\0", ""),
                                    StartName = _Reader.GetString(14),
                                    StopName = _Reader.GetString(15),
                                    OrderId = _Reader.GetInt32(2),
                                    PayBankName = _Reader.GetString(16),
                                    PayBankCode = _Reader.GetString(17),
                                    PayInputName = _Reader.GetString(18),
                                    Name = _Reader.GetString(19),
                                    Ceo = _Reader.GetString(20),
                                };
                                _ModelList.Add(_Model);
                            }
                            else
                            {
                                var _PayLocation = _Reader.GetInt32Z(9);
                                if (_PayLocation == 2)
                                    continue;
                                _Model = _ModelList.FirstOrDefault(c => c.DriverId == _DriverId);
                                if (_Model == null)
                                {
                                    _Model = new Model()
                                    {
                                        DriverId = _DriverId,
                                        LoginId = _Reader.GetString(6),
                                        CarNo = _Reader.GetString(7),
                                        CarYear = _Reader.GetString(8),
                                        OrderDate = _Reader.GetDateTime(11),
                                        BizNo = _Reader.GetString(12),
                                        PayAccountNo = _PayAccountNo.Replace("\0", ""),
                                        StartName = _Reader.GetString(14),
                                        StopName = _Reader.GetString(15),
                                        PayBankName = _Reader.GetString(16),
                                        PayBankCode = _Reader.GetString(17),
                                        PayInputName = _Reader.GetString(18),
                                        Name = _Reader.GetString(19),
                                        Ceo = _Reader.GetString(20),
                                    };
                                    _ModelList.Add(_Model);
                                }
                            }
                            _Model.Count++;
                            _Model.Price += _Price;
                            _Model.ClientPrice += _ClientPrice;
                            _Model.OrderIdList.Add(_OrderId);
                        }
                    }
                }
                _Connection.Close();
            }

          
            foreach (var _Model in _ModelList)
            {
                if (AllowTaxBool == false)
                {
                    _Model.VAT = Math.Floor(_Model.Price * 0.1m);
                    _Model.Amount = _Model.Price + _Model.VAT;
                    _Model.Fee = _Model.ClientPrice - _Model.Price;
                    _Model.Fee_VAT = Math.Floor(_Model.Fee * 0.1m);
                }
                else
                {
                    _Model.Amount = _Model.Price;
                    _Model.VAT = Math.Floor(_Model.Amount - (_Model.Price / 1.1m));
                    _Model.Price = _Model.Amount - _Model.VAT;
                    var _FeeAmount = _Model.ClientPrice - _Model.Price;
                    _Model.Fee = Math.Floor(_FeeAmount / 1.1m);
                    _Model.Fee_VAT = _FeeAmount - _Model.Fee;
                }
            }
            _ModelList.Add(new Model
            {
                DriverId = 0,
                IsRefenece = true,
            });
            _BeginDate = dtp_From.Value;
            _EndDate = dtp_To.Value;

        
        }
        #endregion

        private void ClientLoad()
        {
            LocalUser.Instance.LogInInformation.LoadClient();
            AllowTaxBool = LocalUser.Instance.LogInInformation.Client.AllowTax;
            CustomerPay = LocalUser.Instance.LogInInformation.Client.CustomerPay;
            if (AllowTaxBool == false)
            {
                rdb_Tax0.Checked = true;
            }
            else
            {
                rdb_Tax1.Checked = true;
            }
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
            else if (e.ColumnIndex == ColumnCount.Index)
            {
                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    e.Value = "";
                }
            }
            else if (e.ColumnIndex == ColumnSelect.Index)
            {
                var Selected = _ModelList[e.RowIndex];

                var _DriverSelect = mDriverRepository.NoGetDriver(Selected.DriverId);

                var _Driver = mDriverRepository.NoIsMyCar(Selected.DriverId);

                bool _UptaeB = false;
                bool _UpjongB = false;
                bool _AddressB = false;
                bool _BizNo = false;

                if (_DriverSelect != null)
                {
                    if (!String.IsNullOrEmpty(_DriverSelect.Uptae))
                    {
                        _UptaeB = true;
                    }
                    if (!String.IsNullOrEmpty(_DriverSelect.Upjong))
                    {
                        _UpjongB = true;
                    }
                    if (!String.IsNullOrEmpty(_DriverSelect.Addresstate) && !String.IsNullOrEmpty(_DriverSelect.AddressCity) && !String.IsNullOrEmpty(_DriverSelect.AddressDetail))
                    {
                        _AddressB = true;
                    }

                    if (!String.IsNullOrEmpty(_DriverSelect.BizNo))
                    {
                        //if (_DriverSelect.BizNo.Replace("-", "").Contains("999"))
                        //{
                        //    _BizNo = false;
                        //}
                        //else
                        //{
                        //    _BizNo = true;
                        //}
                        _BizNo = true;
                    }


                }


                //if (String.IsNullOrEmpty(Selected.PayAccountNo)|| String.IsNullOrEmpty(Selected.BizNo) || Selected.BizNo.Contains("999") || Selected.BizNo.Contains("000")|| String.IsNullOrEmpty(Selected.Name) || String.IsNullOrEmpty(Selected.PayInputName)|| String.IsNullOrEmpty(Selected.PayBankName) || String.IsNullOrEmpty(Selected.PayBankCode) || String.IsNullOrEmpty(Selected.Ceo)|| )
                //{



                if (_BizNo == false || String.IsNullOrEmpty(Selected.Name) || String.IsNullOrEmpty(Selected.Ceo) || _UptaeB == false || _UpjongB == false || _AddressB == false || Selected.PayAccountNo == "" || Selected.PayBankName == "" || Selected.PayBankCode == "" || Selected.PayInputName == "" || _Driver == false)
                {
                    var _Cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = false;
                    _Cell.ReadOnly = true;
                }
                else
                {
                    var _Cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = true;
                    _Cell.ReadOnly = false;
                }


                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    if (e.RowIndex != 0)
                    {
                        ((DataGridViewDisableCheckBoxCell)newDGV1[e.ColumnIndex, e.RowIndex]).CheckBoxVisible = false;
                    }
                }
            }
            else if(e.ColumnIndex == RequestDate.Index)
            {
                if (e.RowIndex == newDGV1.RowCount - 1)
                {

                }
                else
                {
                    e.Value = DateTime.Now.ToString("d").Replace("-", "/");
                }

                //e.Value = DateTime.Now.ToString("d").Replace("-", "/");
            }
            else if (e.ColumnIndex == OrderDate.Index)
            {
                if (e.RowIndex == newDGV1.RowCount - 1)
                {

                }
                else
                {
                    e.Value = ((DateTime)e.Value).ToString("d");
                }
              //  e.Value = ((DateTime)e.Value).ToString("d");
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
           

            foreach (var _Model in _ModelList.Where(c=> c.BizNo != "" && c.PayAccountNo != "" && c.PayBankName != "" && c.PayBankCode != "" && c.PayInputName != ""))
            {
                _Model.Selected = AllSelect.Checked;
            }
            SetReferenceModel();
        }

        private void SetReferenceModel()
        {
            var _Models = _ModelList.Where(c => c.Selected && !c.IsRefenece && (c.BizNo != "" && c.PayAccountNo != "" && c.PayAccountNo != "" && c.PayBankName != "" && c.PayBankCode != "" && c.PayInputName != ""));

            var _Models2 = _ModelList.Where(c => c.Selected  && (c.BizNo != "" && c.PayAccountNo != "" && c.PayAccountNo != "" && c.PayBankName != "" && c.PayBankCode != "" && c.PayInputName != ""));


            var _RModel = _ModelList.Where(c => c.BizNo != "" && c.PayAccountNo != "" && c.PayAccountNo != "" && c.PayBankName != "" && c.PayBankCode != "" && c.PayInputName != "").FirstOrDefault(c => c.IsRefenece);
            if (_RModel != null)
            {
                _RModel.Price = _Models.Sum(c => c.Price);
                _RModel.VAT = _Models.Sum(c => c.VAT);
                _RModel.Amount = _Models.Sum(c => c.Amount);


                lblcnt.Text = _Models.Count().ToString("N0");

                lblPrice.Text = _Models.Sum(c => c.Price).ToString("N0");
                lblVat.Text = _Models.Sum(c => c.VAT).ToString("N0");
                lblAmout.Text = _Models.Sum(c => c.Amount).ToString("N0");

            }

          
           
        }

        private void newDGV1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Tax0.Checked)
            {
                AllowTaxBool = false;
            }
            else
            {
                AllowTaxBool = true;
            }
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

            var _Driver = mDriverRepository.NoIsMyCar(_Row.DriverId);

            var _DriverSelect = mDriverRepository.NoGetDriver(_Row.DriverId);

            bool _UptaeB = false;
            bool _UpjongB = false;
            bool _AddressB = false;
            bool _BizNo = false;

            if (_DriverSelect != null)
            {
                if (!String.IsNullOrEmpty(_DriverSelect.Uptae))
                {
                    _UptaeB = true;
                }
                if (!String.IsNullOrEmpty(_DriverSelect.Upjong))
                {
                    _UpjongB = true;
                }
                if (!String.IsNullOrEmpty(_DriverSelect.Addresstate) && !String.IsNullOrEmpty(_DriverSelect.AddressCity) && !String.IsNullOrEmpty(_DriverSelect.AddressDetail))
                {
                    _AddressB = true;
                }

                if (!String.IsNullOrEmpty(_DriverSelect.BizNo))
                {
                    //if (_DriverSelect.BizNo.Replace("-", "").Contains("999"))
                    //{
                    //    _BizNo = false;
                    //}
                    //else
                    //{
                    //    _BizNo = true;
                    //}
                    _BizNo = true;
                }
                

            }


            if (_BizNo == false || String.IsNullOrEmpty(_Row.Name) || String.IsNullOrEmpty(_Row.Ceo) || _UptaeB == false || _UpjongB == false || _AddressB == false || _Row.PayAccountNo == "" || _Row.PayBankName == "" || _Row.PayBankCode == "" || _Row.PayInputName == "" || _Driver == false)
            {

                newDGV1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                newDGV1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;


            }
        }

        private void newDGV1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;


            var _Row = _ModelList[e.RowIndex];
            
           
            if (e.ColumnIndex != 0)
            {
                _Select(_Row.CarNo);
            }

            
        }
        private void _Select(string _DCarNo)
        {

            _CarNo = _DCarNo;

          

            FrmMN0203_CAROWNERMANAGE_FAULT_Add2 _Form = new FrmMN0203_CAROWNERMANAGE_FAULT_Add2(_CarNo);


            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
          
            if (_Form.ShowDialog() == DialogResult.OK)
            {
                _ModelList.Clear();
                //btn_Search_Click(null, null);
                DataLoad();
            }
            else
            {
                _ModelList.Clear();
                DataLoad();
                //btn_Search_Click(null, null);
            }


            //DialogResult = DialogResult.OK;
            //Close();
        }

        private void rdoSplit_CheckedChanged(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            switch (cmbSMonth.SelectedIndex)
            {

                //당월
                case 0:
                    dtp_From.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_To.Value = DateTime.Now;
                    break;
                //전월
                case 1:
                    dtp_From.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_To.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 2:
                    dtp_From.Value = DateTime.Now;
                    dtp_To.Value = DateTime.Now;
                    break;
            }
        }
    }
}
