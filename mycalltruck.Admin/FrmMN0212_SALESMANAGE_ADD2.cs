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
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class FrmMN0212_SALESMANAGE_ADD2 : Form
    {
        DateTime _BeginDate = DateTime.Now;
        DateTime _EndDate = DateTime.Now;
        bool AllowTaxBool = false;
        #region ACTION


        public FrmMN0212_SALESMANAGE_ADD2()
        {
            InitializeComponent();
            InitializeStorage();

            InitCmb();
            cmbSMonth.SelectedIndex = 4;

            dtp_From.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtp_To.Value = DateTime.Now.Date;

            if (LocalUser.Instance.LogInInformation.ClientId != 290)
            {
                //ColumnEndDay.Visible = false;
            }
        }

        public void InitCmb()
        {
            Dictionary<string, string> Smonth = new Dictionary<string, string>
            {
                { "당일", "당일" },
                { "전일", "전일" },
                { "금주", "금주" },
                { "금월", "금월" },
                { "전월", "전월" },
                { "지정", "지정" }
            };

            cmbSMonth.DataSource = new BindingSource(Smonth, null);
            cmbSMonth.DisplayMember = "Value";
            cmbSMonth.ValueMember = "Key";

            cmbSMonth.SelectedIndex = 0;




        }

        private void FrmTradeNew2_Load(object sender, EventArgs e)
        {
            CustomerLoad();
            SetMouseClick(this);
            DataLoad();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            _Update();
            Close();
        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        #endregion

        #region UPDATE
        private void _Update()
        {
            var _Models = _ModelList.Where(c => c.Selected && !c.IsRefenece);
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
                using (SqlCommand _OrderCommand = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"INSERT INTO SalesManage ( RequestDate, SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat, UseTax, Price, Amount, CreateDate,  ClientId, Issue, HasACC,  SetYN, PayState,   CardPayGubun, CustomerId, ClientAccId,  ZipCode,BeginDate,EndDate,SourceType,SubClientId,Taxgubun) OUTPUT INSERTED.SalesId
                        VALUES (Getdate(), @SangHo, @BizNo, @Ceo, @Uptae, @Upjong, @AddressState, @AddressCity, @AddressDetail, @Email, @ContRactName, @MobileNo, @Item, @UnitPrice, @Num, @Vat, @UseTax, @Price, @Amount, Getdate(), @ClientId, 0, 0,  0, 2,   'N', @CustomerId, 0,  @ZipCode,@BeginDate,@EndDate,1,@SubClientId,@Taxgubun)";
                    _OrderCommand.CommandText = "UPDATE Orders SET SalesManageId = @SalesManageId WHERE OrderId = @OrderId";
                    _OrderCommand.Parameters.Add("@SalesManageId", SqlDbType.Int);
                    _OrderCommand.Parameters.Add("@OrderId", SqlDbType.Int);


                    _Command.Parameters.Add("@SangHo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Ceo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Uptae", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Upjong", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AddressState", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AddressCity", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AddressDetail", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Email", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ContRactName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@MobileNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Item", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@UnitPrice", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Num", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Vat", SqlDbType.Decimal);
                    _Command.Parameters.Add("@UseTax", SqlDbType.Bit);
                    _Command.Parameters.Add("@Taxgubun", SqlDbType.Int);
                    _Command.Parameters.Add("@Price", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Amount", SqlDbType.Decimal);
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                    //_Command.Parameters.Add("@PayBankName", SqlDbType.NVarChar);
                    //_Command.Parameters.Add("@PayBankCode", SqlDbType.NVarChar);
                    //_Command.Parameters.Add("@PayAccountNo", SqlDbType.NVarChar);
                    //_Command.Parameters.Add("@PayInputName", SqlDbType.NVarChar);

                    _Command.Parameters.Add("@CustomerId", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ZipCode", SqlDbType.NVarChar);

                    _Command.Parameters.Add("@BeginDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@EndDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@SubClientId", SqlDbType.Int);


                    int _CustomerId = 0;
                    int _UseTax = 1;
                    int _Taxgubun = 0;
                    if (cmb_Search.SelectedIndex == 0)
                    {
                        _CustomerId =(int)txt_Search.Tag;
                    }
                    else
                    {
                        _CustomerId = (int)cmb_Search.SelectedValue;
                    }


                    if (rdb_Tax0.Checked)
                    {
                        _UseTax = 1;
                        _Taxgubun = 0;
                    }
                    else
                    {
                        _UseTax = 0;
                        _Taxgubun = 1;
                    }

                    var _Customer = _CustomerViewModelList.FirstOrDefault(c => c.CustomerId == _CustomerId);
                    if (_Customer != null)
                    {

                        _Command.Parameters["@SangHo"].Value = _Customer.SangHo;
                        _Command.Parameters["@BizNo"].Value = _Customer.BizNo;
                        _Command.Parameters["@Ceo"].Value = _Customer.Ceo;
                        _Command.Parameters["@Uptae"].Value = _Customer.Uptae;
                        _Command.Parameters["@Upjong"].Value = _Customer.Upjong;
                        _Command.Parameters["@AddressState"].Value = _Customer.AddressState;
                        _Command.Parameters["@AddressCity"].Value = _Customer.AddressCity;
                        _Command.Parameters["@AddressDetail"].Value = _Customer.AddressDetail;
                        _Command.Parameters["@Email"].Value = _Customer.Email;
                        _Command.Parameters["@ContRactName"].Value = _Customer.ChargeName;
                        _Command.Parameters["@MobileNo"].Value = _Customer.PhoneNo;
                        _Command.Parameters["@Item"].Value = "운송료집계";
                        //_Command.Parameters["@UnitPrice"].Value = _Model.Price;
                        _Command.Parameters["@UnitPrice"].Value = _Models.Sum(c => c.Price);

                        _Command.Parameters["@Num"].Value = 1;
                        //_Command.Parameters["@Vat"].Value = _Model.VAT;
                        _Command.Parameters["@Vat"].Value = _Models.Sum(c => c.VAT);
                        _Command.Parameters["@UseTax"].Value = _UseTax;

                        _Command.Parameters["@Taxgubun"].Value = _Taxgubun;

                        _Command.Parameters["@Price"].Value = _Models.Sum(c => c.Price);
                        _Command.Parameters["@Amount"].Value = _Models.Sum(c => c.Amount);
                        _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                        _Command.Parameters["@CustomerId"].Value = (int)cmb_Search.SelectedValue;
                        _Command.Parameters["@ZipCode"].Value = _Customer.Zipcode;
                        _Command.Parameters["@BeginDate"].Value = _BeginDate;
                        _Command.Parameters["@EndDate"].Value = _EndDate;
                        if (LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            _Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                        }
                        else
                        {
                            _Command.Parameters["@SubClientId"].Value = DBNull.Value;
                        }
                        var _SalesManageId = Convert.ToInt32(_Command.ExecuteScalar());
                        //foreach (var _OrderId in _Model.OrderIdList)
                        //{
                        //    _OrderCommand.Parameters["@SalesManageId"].Value = _SalesManageId;
                        //    _OrderCommand.Parameters["@OrderId"].Value = _OrderId;
                        //    _OrderCommand.ExecuteNonQuery();
                        //}
                        foreach (var _OrderId in _Models)
                        {
                            _OrderCommand.Parameters["@SalesManageId"].Value = _SalesManageId;
                            _OrderCommand.Parameters["@OrderId"].Value = _OrderId.OrderId;
                            _OrderCommand.ExecuteNonQuery();
                        }

                    }
                    //}
                }
                _Connection.Close();
            }
        }
        #endregion

        #region STORAGE

        private List<OrderViewModel> _OrderTable = new List<OrderViewModel>();
        class OrderViewModel
        {
            public int OrderId { get; set; }
            public DateTime AcceptTime { get; set; }
            public string StartName { get; set; }
            public string StopName { get; set; }
            public string Item { get; set; }
            public int PayLocation { get; set; }
            public string DriverCarNo { get; set; }

        }


        private void InitOrderTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT OrderId,AcceptTime, ISNULL(StartName,''), ISNULL(StopName,''), Item,PayLocation, DriverCarNo  FROM Orders WHERE AcceptTime IS NOT NULL";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _OrderTable.Add(
                              new OrderViewModel
                              {
                                  OrderId = dataReader.GetInt32(0),
                                  AcceptTime = dataReader.GetDateTime(1),
                                  StartName = dataReader.GetString(2),
                                  StopName = dataReader.GetString(3),
                                  Item = dataReader.GetString(4),
                                  PayLocation = dataReader.GetInt32(5),
                                  DriverCarNo = dataReader.GetString(6),
                                
                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        class Model : INotifyPropertyChanged
        {
            public int OrderId { get; set; }
            public int CustomerId { get; set; }
            public int Count { get; set; }
            public decimal Price { get { return _Price; } set { SetField(ref _Price, value); } }
            public decimal VAT { get { return _VAT; } set { SetField(ref _VAT, value); } }
            public decimal Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            private decimal _Price = 0;
            private decimal _VAT = 0;
            private decimal _Amount = 0;
            private bool _Selected = false;

            public string AcceptTime { get; set; }
            public string StartName { get; set; }
            public string StopName { get; set; }
            public string Item { get; set; }
            public string PayLocation { get; set; }
            public string DriverCarNo { get; set; }
            public string CustomerTeam { get; set; }

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
        class CustomerViewModel
        {
            public int CustomerId { get; set; }
           
            public string BizNo { get; set; }
            public string SangHo { get; set; }
            public string Ceo { get; set; }
            public string Uptae { get; set; }
            public string Upjong { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }

            public string Email { get; set; }

            public string ChargeName { get; set; }
            public string PhoneNo { get; set; }
            public string Zipcode { get; set; }
            public int EndDay { get; set; }
            public string MobileNo { get; set; }


        }
        private BindingList<Model> _ModelList = new BindingList<Model>();
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>();
        private void InitializeStorage()
        {
             newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _ModelList;
        }
        private void DataLoad()
        {
            string CommandText = "";
            _ModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    CommandText = "SELECT Orders.CustomerId, ISNULL(Orders.SalesPrice,0)+ISNULL(Orders.AlterPrice,0), Orders.OrderId" +
                        " ,Orders.AcceptTime,isnull(Orders.StartName,''),isnull(Orders.StopName,''),Orders.Item" +
                        " ,Orders.PayLocation,isnull(Orders.DriverCarNo,''),ISNULL(Orders.ClientPrice,0), isnull(CustomerTeams.TeamName,'본사') FROM Orders" +
                        " JOIN Customers ON Orders.CustomerId = Customers.CustomerId" +
                        " LEFT JOIN CustomerTeams ON Orders.CustomerTeam = CustomerTeams.CustomerTeamId";
                        
                        
                        //" AND Orders.OrderStatus = 3 AND Orders.CustomerId IS NOT NULL " +
                        //" AND ISNULL(Orders.SalesPrice,0) +ISNULL(Orders.AlterPrice,0)  > 0 " +


                        //" AND (PayLocation = 1 or PayLocation = 5 or PayLocation = 4)";
                        //" Order by  Orders.CreateTime ";
                   
                 


                    String SelectCommandText = CommandText;

                    List<String> WhereStringList = new List<string>();


                    WhereStringList.Add(" Orders.CreateTime >= @Begin AND Orders.CreateTime < @End ");
                    _Command.Parameters.AddWithValue("@Begin", dtp_From.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtp_To.Value.Date.AddDays(1));

                    WhereStringList.Add(" Orders.ClientId = @ClientId AND Orders.SalesManageId IS NULL ");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    WhereStringList.Add(" Orders.OrderStatus = 3 ");
                    WhereStringList.Add(" Orders.CustomerId IS NOT NULL ");
                    WhereStringList.Add(" ISNULL(Orders.SalesPrice,0) +ISNULL(Orders.AlterPrice,0)  > 0 ");
                    WhereStringList.Add(" (PayLocation = 1 or PayLocation = 5 or PayLocation = 4) ");
                    if (cmb_Search.Items.Count > 0)
                    {
                       
                        if (!string.IsNullOrEmpty(txt_Search.Text))
                        {
                            if (txt_Search.Tag != null)
                            {
                                WhereStringList.Add("Customers.CustomerId =  @CustomerId ");
                                _Command.Parameters.AddWithValue("@CustomerId", (int)txt_Search.Tag);



                            }
                            else
                            {
                                WhereStringList.Add("Customers.CustomerId =  @CustomerId ");
                              
                                _Command.Parameters.AddWithValue("@CustomerId", cmb_Search.SelectedValue);
                            }
                        }
                        else
                        {
                            WhereStringList.Add("Customers.CustomerId =  @CustomerId ");
                            _Command.Parameters.AddWithValue("@CustomerId", cmb_Search.SelectedValue);
                        }
                    }
                    else
                    {
                        WhereStringList.Add("Customers.CustomerId =  @CustomerId ");
                        _Command.Parameters.AddWithValue("@CustomerId", 0);
                    }

                 


                    if (cmbTeam.SelectedIndex > 0)
                    {
                        WhereStringList.Add("Orders.CustomerTeam = @CustomerTeam ");
                        _Command.Parameters.AddWithValue("@CustomerTeam", cmbTeam.SelectedValue.ToString());
                    }

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by  Orders.CreateTime ";

                    _Command.CommandText = SelectCommandText;



                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var _CustomerId = _Reader.GetInt32Z(0);
                            var _Price = _Reader.GetDecimal(1);
                            var _OrderId = _Reader.GetInt32(2);
                            var _AcceptTime = _Reader.GetDateTime(3);
                            var _StartName = _Reader.GetString(4);
                            var _StopName = _Reader.GetString(5);
                            var _Item = _Reader.GetString(6);
                            var _PayLocation = "인수증";
                            var _DriverCarNo = _Reader.GetString(8);
                            var _CustomerTeam = _Reader.GetString(10);

                            var _Model = _ModelList.FirstOrDefault(c => c.OrderId == _OrderId);
                            if (_Model == null)
                            {
                                _Model = new Model();
                                _Model.OrderId = _OrderId;
                                _Model.AcceptTime = _AcceptTime.ToString("d");
                                _Model.StartName = _StartName;
                                _Model.StopName = _StopName;
                                _Model.Item = _Item;
                                _Model.PayLocation = _PayLocation;
                                _Model.DriverCarNo = _DriverCarNo;
                                _Model.CustomerTeam = _CustomerTeam;
                                _ModelList.Add(_Model);
                            }
                            _Model.Count++;
                            _Model.Price += _Price;
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
                    decimal _Price = _Model.Price;
                    decimal _Vat = Math.Floor(_Price * 0.1m);
                    decimal _Amount = (_Price + _Vat);
                    _Model.VAT = _Vat;
                    _Model.Amount = _Amount;
                   
                }
                else
                {
                    _Model.Amount = _Model.Price;
                    _Model.VAT = Math.Floor(_Model.Amount - (_Model.Price / 1.1m));
                    _Model.Price = _Model.Amount - _Model.VAT;
                }
            }
            _ModelList.Add(new Model
            {
                OrderId = 0,
              //  CustomerId = 0,
                IsRefenece = true,
            });
            _BeginDate = dtp_From.Value;
            _EndDate = dtp_To.Value;
        }
        private void CustomerLoad(int iCustomerId = 0)
        {
            _CustomerViewModelList.Clear();
            if (iCustomerId > 0 && _CustomerViewModelList.Any(c => c.CustomerId == iCustomerId))
                return;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT Customers.CustomerId,  Customers.BizNo, Customers.SangHo, Customers.Ceo, Customers.Uptae, Customers.Upjong, Customers.AddressState, Customers.AddressCity, Customers.AddressDetail,  Customers.Email,  Customers.PhoneNo, Customers.ChargeName,  Customers.Zipcode,Customers.EndDay,Customers.ClientId,ISNULL(Customers.MobileNo,'') FROM Customers  ";
                    if (iCustomerId == 0)
                    {
                        _Command.CommandText += Environment.NewLine + "WHERE ClientId = @ClientId ";
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    }
                    else
                    {
                        _Command.CommandText += Environment.NewLine + "WHERE CustomerId = @CustomerId ";
                        _Command.Parameters.AddWithValue("@CustomerId", iCustomerId);
                    }
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var _CustomerId = _Reader.GetInt32Z(0);
                            var _BizNo = _Reader.GetStringN(1);
                            var _SangHo = _Reader.GetStringN(2);
                            var _Ceo = _Reader.GetStringN(3);
                            var _Uptae = _Reader.GetStringN(4);
                            var _Upjong = _Reader.GetStringN(5);
                            var _AddressState = _Reader.GetStringN(6);
                            var _AddressCity = _Reader.GetStringN(7);
                            var _AddressDetail = _Reader.GetStringN(8);
                            var _Email = _Reader.GetStringN(9);
                            var _PhoneNo = _Reader.GetStringN(10);
                            var _ChargeName = _Reader.GetStringN(11);
                            var _Zipcode = _Reader.GetStringN(12);
                            var _EndDay = _Reader.GetInt32(13);
                            var _MobileNo = _Reader.GetStringN(15);

                            _CustomerViewModelList.Add(new CustomerViewModel
                            {
                                CustomerId = _CustomerId,
                                BizNo = _BizNo,
                                SangHo = _SangHo,
                                Ceo = _Ceo,
                                Uptae = _Uptae,
                                Upjong = _Upjong,
                                AddressState = _AddressState,
                                AddressCity = _AddressCity,
                                AddressDetail = _AddressDetail,
                                Email = _Email,
                                PhoneNo = _PhoneNo,
                                ChargeName = _ChargeName,
                                Zipcode = _Zipcode,
                                EndDay = _EndDay,
                                MobileNo  = _MobileNo,

                            });
                        }
                    }
                }
                _Connection.Close();
            }
        }
        #endregion


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
            //else if (e.ColumnIndex == ColumnCount.Index)
            //{
            //    if (e.RowIndex == newDGV1.RowCount - 1)
            //    {
            //        e.Value = "";
            //    }
            //}
            else if (e.ColumnIndex == ColumnSelect.Index)
            {
                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    ((UI.DataGridViewDisableCheckBoxCell)newDGV1[e.ColumnIndex, e.RowIndex]).CheckBoxVisible = false;
                }
            }
            
            else if (e.ColumnIndex == ColumnAcceptDate.Index )
            {

                //if (e.Value == null)
                //{ return; }
                //var _OrderId = (int)e.Value;
                //if (_OrderId == 0)
                //{
                //    e.Value = "";
                //    return;
                //}
               
               // InitOrderTable();
                //var _Order = _OrderTable.FirstOrDefault(c => c.OrderId == _OrderId);
                //if(_Order == null)
                //{
                //    e.Value = "";
                //}
                //else
                //{
                //    //if (e.ColumnIndex == ColumnAcceptDate.Index)
                //    //    e.Value = _Order.AcceptTime.ToString("d");
                //    //else 
                    
                //    //if (e.ColumnIndex == ColumnStartName.Index)
                //    //    e.Value = _Order.StartName;
                //    //else 
                //    //if (e.ColumnIndex == ColumnStopName.Index)
                //    //    e.Value = _Order.StopName;
                //    //else 
                    
                //    //if (e.ColumnIndex == ColumnItem.Index)
                //    //    e.Value = _Order.Item;
                //    //else if (e.ColumnIndex == ColumnPayLocation.Index)
                //    //    e.Value = "인수증";
                //    //else if (e.ColumnIndex == ColumnDriverCarNo.Index)
                //    //    e.Value = _Order.DriverCarNo;
                //}
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
            var _Models = _ModelList.Where(c => c.Selected && !c.IsRefenece);
            var _RModel = _ModelList.FirstOrDefault(c => c.IsRefenece);
            if (_RModel != null)
            {
                _RModel.Price = _Models.Sum(c => c.Price);
                _RModel.VAT = _Models.Sum(c => c.VAT);
                _RModel.Amount = _Models.Sum(c => c.Amount);
            }
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

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (txt_Search.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    CustomerSelect.Items.Clear();
                    int ItemIndex = 0;
                    //CustomerLoad();
                    //foreach (var _Customer in _CustomerViewModelList.Where(c => c.SangHo.Contains(txt_Search.Text)))
                    //{
                    //    CustomerSelect.Items.Add(_Customer.SangHo);
                    //    CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.PhoneNo);
                    //    CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.AddressState + " " + _Customer.AddressCity);
                    //    CustomerSelect.Items[ItemIndex].Tag = _Customer.CustomerId;
                    //    ItemIndex++;
                    //}

                    List<Order> DataSource = new List<Order>();
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        DateTime _Edate = dtp_To.Value.AddDays(1);

                        DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("CustomerModel").Where(c => c.OrderStatus == 3 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CreateTime >= dtp_From.Value && c.CreateTime < _Edate && c.SalesManageId == null && (c.PayLocation == 1 || c.PayLocation == 4 || c.PayLocation == 5) && c.SalesPrice + c.AlterPrice > 0).OrderByDescending(c => c.CreateTime).ToList();


                    }

                      
                    foreach (var item in DataSource.GroupBy(c => new { c.CustomerId }).Select(c => c.First()).ToList().Where(c=> c.Customer.Contains(txt_Search.Text)))
                    {
                     
                        CustomerSelect.Items.Add(item.CustomerModel.SangHo);
                        CustomerSelect.Items[ItemIndex].SubItems.Add(item.CustomerModel.PhoneNo);
                        CustomerSelect.Items[ItemIndex].SubItems.Add(item.CustomerModel.AddressState + " " + item.CustomerModel.AddressCity);
                        CustomerSelect.Items[ItemIndex].Tag = item.CustomerId;
                        ItemIndex++;
                    }



                    if (CustomerSelect.Items.Count > 0)
                    {

                        CustomerSelect.Items[0].Selected = true;

                        CustomerSelect.Focus();
                    }
                    else
                    {

                        txt_Search.Clear();
                        txt_Search.Tag = null;
                    }

                    CustomerSelect.Visible = true;
                }
            }
            else
            {
                if (e.KeyChar == 13)
                {

                    CustomerSelect.Items.Clear();
                  
                    CustomerSelect.Visible = true;
                    CustomerSelect.Focus();

                }
            }
        }
        private void SetMouseClick(Control mControl)
        {
            foreach (Control nControl in mControl.Controls)
            {
                if (nControl == CustomerSelect)
                {
                    continue;
                }
                nControl.MouseClick += NControl_MouseClick;
                SetMouseClick(nControl);
            }
        }

        private void NControl_MouseClick(object sender, MouseEventArgs e)
        {
            CustomerSelect.Visible = false;
            
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

        private void _CustomerSelected()
        {

            txt_Search.Clear();
            cmb_Search.SelectedIndex = 0;
            var _Customer = _CustomerViewModelList.Find(c => c.CustomerId == (int)CustomerSelect.SelectedItems[0].Tag);

            txt_Search.Text = _Customer.SangHo;
            txt_Search.Tag = _Customer.CustomerId;


            CustomerSelect.Visible = false;
            txt_Search.Focus();


            if (!string.IsNullOrEmpty(txt_Search.Tag.ToString()))
            {
                cmbTeamSearchBinding(Convert.ToInt32(txt_Search.Tag.ToString()));

            }
            else
            {
                cmbTeamSearchBinding(0);
            }

            btn_Search_Click(null, null);
        }
        private void cmbSearchBinding()
        {
            Dictionary<int, string> DCustomerI = new Dictionary<int, string>();

            DCustomerI.Clear();
            cmb_Search.DataSource = null;

            List<Order> DataSource = new List<Order>();
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                DateTime _Sdate = dtp_From.Value.Date;
                DateTime _Edate = dtp_To.Value.AddDays(1).Date;

                DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("CustomerModel").Where(c => c.OrderStatus == 3 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CreateTime >= _Sdate && c.CreateTime < _Edate && c.SalesManageId == null && (c.PayLocation == 1 || c.PayLocation == 4 || c.PayLocation == 5) && c.SalesPrice + c.AlterPrice > 0).OrderByDescending(c => c.CreateTime).ToList();


            }
            DCustomerI.Add(0, "선택");
            foreach (var item in DataSource.GroupBy(c=> new { c.CustomerId}).Select(c=>c.First()).ToList().OrderBy(c=> c.Customer))
            {
                try
                {
                    if (item.CustomerModel.SalesDay > 0)
                    {
                        DCustomerI.Add((int)item.CustomerId, item.CustomerModel.SangHo + "(" + item.CustomerModel.SalesDay + "일)");
                    }
                    else
                    {
                        DCustomerI.Add((int)item.CustomerId, item.CustomerModel.SangHo);
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (DCustomerI.Any())
            {

                cmb_Search.DataSource = new BindingSource(DCustomerI, null);
                cmb_Search.DisplayMember = "Value";
                cmb_Search.ValueMember = "Key";
                cmb_Search.SelectedIndex = 0;
              
            }
           
          
           // DataLoad();
           
        }
        private void dtp_From_ValueChanged(object sender, EventArgs e)
        {
            cmbSearchBinding();
        }

        private void dtp_To_ValueChanged(object sender, EventArgs e)
        {
            cmbSearchBinding();
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Search.Text = "";
            txt_Search.Tag = null;


            if (cmb_Search.SelectedIndex > 0)
            {
                cmbTeamSearchBinding(Convert.ToInt32(cmb_Search.SelectedValue.ToString()));

            }
            else
            {
                cmbTeamSearchBinding(0);
            }


        }
        private void cmbTeamSearchBinding(int _CustomerId)
        {
            Dictionary<int, string> DCustomer = new Dictionary<int, string>();

            customerTeamsTableAdapter.Fill(customerUserDataSet.CustomerTeams, LocalUser.Instance.LogInInformation.ClientId);


            var cmbCustomerTeamataSource = customerUserDataSet.CustomerTeams.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == _CustomerId).Select(c => new { c.TeamName, c.CustomerTeamId }).OrderBy(c => c.CustomerTeamId).ToArray();

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
        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMonth.SelectedIndex)
            {
                //당일
                case 0:
                    dtp_From.Value = DateTime.Now;
                    dtp_To.Value = DateTime.Now;
                    break;
                //전일
                case 1:
                    dtp_From.Value = DateTime.Now.AddDays(-1);
                    dtp_To.Value = DateTime.Now;
                    break;
                //금주
                case 2:
                    dtp_From.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    dtp_To.Value = DateTime.Now;
                    break;
                //금월
                case 3:
                    dtp_From.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_To.Value = DateTime.Now;
                    break;
                //전월
                case 4:
                    dtp_From.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_To.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 5:
                    dtp_From.Value = DateTime.Now;
                    dtp_To.Value = DateTime.Now;
                    break;
            }
        }
    }
}
