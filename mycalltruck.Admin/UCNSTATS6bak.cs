using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Extensions;
using System.Diagnostics;
using mycalltruck.Admin.Class.DataSet;

namespace mycalltruck.Admin
{
    public partial class UCNSTATS6bak : UserControl
    {
        String Gubun = string.Empty;
        int iidx = 0;
        public static OrderDataSet.OrdersRow Order { get; set; }
        public UCNSTATS6bak()
        {
            InitializeComponent();
        }

        private void UCNSTATS1_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
            dtp_Edate.Value = DateTime.Now;
            dtpOrderSdate.Value = DateTime.Now;
            dtpOrderEdate.Value = DateTime.Now;
            InitCmb();
            SetCustomerList();
          //  InitDriverTable();
            LoadTable();

          
            rdoDriver.Checked = true;
            
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

        private void InitDriverTable()
        {

            baseDataSet.Drivers.Clear();

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {

                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount, Drivers.VBankName,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                ISNULL(DriverInstances.SubClientId, 0) AS SubClientId, ISNULL(DriverInstances.ClientUserId, 0) AS ClientUserId, ISNULL(DriverInstances.GroupName, '0') AS GroupName,
                                ISNULL(DriverInstances.CarGubun, 0) AS CarGubun, ISNULL(DriverInstances.RequestFrom, N'') AS RequestFrom, ISNULL(DriverInstances.RequestTo, N'') AS RequestTo,Drivers.DealerId,Drivers.Misu,Drivers.Mizi
                            FROM    Drivers
                           LEFT JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId";

                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += " Order by Drivers.CreateDate DESC";
                    }
                    else
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += " Order by DriverInstances.DriverInstanceId DESC";
                    }

                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        baseDataSet.Drivers.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        Dictionary<int, string> DCustomer = new Dictionary<int, string>();
        Dictionary<int, string> DCustomerI = new Dictionary<int, string>();
        Dictionary<int, string> DCustomerJ = new Dictionary<int, string>();
        
        Dictionary<int, string> DDriver = new Dictionary<int, string>();
        List<Order> DataSource = new List<Order>();
        List<Order> JDataSource = new List<Order>();
        List<Order> WDataSource = new List<Order>();
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

            cmbSMonth.SelectedIndex = 5;



            customersNewTableAdapter.Fill(clientDataSet.CustomersNew);
            var cmbCustomerMIdDataSource = clientDataSet.CustomersNew.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();

            DCustomer.Add(0, "전체");


            foreach (var item in cmbCustomerMIdDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }


            cmbSReferralId.DataSource = new BindingSource(DCustomer, null);
            cmbSReferralId.DisplayMember = "Value";
            cmbSReferralId.ValueMember = "Key";
            cmbSReferralId.SelectedValue = 0;


            //var _Sdate = dtpOrderSdate.Value.Date;
            //var _Edate = dtpOrderEdate.Value.Date.AddDays(1);
            //var cmbCustomerMIdDataSource2 = clientDataSet.CustomersNew.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();
            //using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            //{
            //    WDataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && c.ReferralId != 0 && c.CreateTime >= _Sdate && c.CreateTime < _Edate).ToList();
            //}


            // if (WDataSource.Any())
            //{
            //    foreach (var item in WDataSource.GroupBy(c=> c.ReferralId))
            //    {
            //        var Query = cmbCustomerMIdDataSource2.Where(c => c.CustomerId == item.First().ReferralId);
            //        if (Query.Any())
            //        {
            //            DCustomerI.Add((int)item.First().ReferralId, Query.First().SangHo);
            //        }
            //    }

            //    if (DCustomerI.Any())
            //    {

            //        cmbReferralId.DataSource = new BindingSource(DCustomerI, null);
            //        cmbReferralId.DisplayMember = "Value";
            //        cmbReferralId.ValueMember = "Key";
            //        cmbReferralId.SelectedIndex = 0;
            //    }
            //}

            var _Sdate = dtpOrderSdate.Value.Date;
            var _Edate = dtpOrderEdate.Value.Date.AddDays(1);
            var cmbCustomerMIdDataSource2 = clientDataSet.CustomersNew.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {


                WDataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && (c.PayLocation == 2 || c.PayLocation == 4) && c.OrderStatus == 3 && c.ReferralId != 0 && c.CreateTime >= _Sdate && c.CreateTime < _Edate && (c.ReferralAccountYN == "N" || c.ReferralAccountYN == "")).ToList();
            }
            if (WDataSource.Any())
            {
                DCustomerI.Clear();

                foreach (var item in WDataSource.GroupBy(c => c.ReferralId))
                {
                    var Query = cmbCustomerMIdDataSource2.Where(c => c.CustomerId == item.First().ReferralId);
                    if (Query.Any())
                    {
                        if (item.Sum(c => c.DriverPrice ?? 0) > 0)
                        {
                            DCustomerI.Add((int)item.First().ReferralId, Query.First().SangHo);
                        }
                    }
                }

                if (DCustomerI.Any())
                {

                    cmbReferralId.DataSource = new BindingSource(DCustomerI, null);
                    cmbReferralId.DisplayMember = "Value";
                    cmbReferralId.ValueMember = "Key";
                    cmbReferralId.SelectedIndex = 0;
                }
            }
            else
            {
                // DCustomerI.Clear();
                cmbReferralId.DataSource = null;
            }



            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                JDataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && c.CustomerModel.BizGubun == 3
                && c.CreateTime >= _Sdate && c.CreateTime < _Edate).ToList();
            }

            if(JDataSource.Any())
            {
                foreach (var item in JDataSource.GroupBy(c=> c.CustomerId))
                {
                    DCustomerJ.Add((int)item.First().CustomerId, item.First().Customer);
                }

                if (DCustomerJ.Any())
                {

                    cmbJusun.DataSource = new BindingSource(DCustomerJ, null);
                    cmbJusun.DisplayMember = "Value";
                    cmbJusun.ValueMember = "Key";
                    cmbJusun.SelectedIndex = 0;
                }
            }

           


            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                DataSource =  ShareOrderDataSet.Orders.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.FOrderId != 0 && c.OrderStatus == 3).ToList();
            }
            
            if (DataSource.Any())
            {
                foreach (var item in DataSource)
                {
                    DDriver.Add((int)item.DriverId, item.Driver);
                }

                if(DDriver.Any())
                {
                    cmbDriver.DataSource = new BindingSource(DDriver, null);
                    cmbDriver.DisplayMember = "Value";
                    cmbDriver.ValueMember = "Key";
                    cmbDriver.SelectedIndex = 0;
                }
            }



            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {
                { "전체", "전체" },
                //{ "배차일시", "배차일시" },
                { "화주(상호)", "화주(상호)" },
                { "화주(사업자번호)", "화주(사업자번호)" },
                { "차주(상호)", "차주(상호)" },
                { "차주(사업자번호)", "차주(사업자번호)" },
                 { "차주(차량번호)", "차주(차량번호)" }

            };


            //cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            //cmbSSearch.DisplayMember = "Value";
            //cmbSSearch.ValueMember = "Key";

            //cmbSSearch.SelectedIndex = 0;


        }
        private String GetSelectCommand()
        {
            return @"SELECT  ReferralAccount.idx, ReferralAccount.PayDate, ReferralAccount.CustomerId,CASE WHEN ReferralAccount.DriverId  = 0 THEN  Customers.SangHo ELSE Drivers.CarYear END as SangHo, ReferralAccount.OutAmount, ReferralAccount.ClientId, ReferralAccount.OrderSdate, 
               ReferralAccount.OrderEdate, ReferralAccount.Amount
FROM     ReferralAccount left JOIN
               Customers ON ReferralAccount.CustomerId = Customers.CustomerId
			   left join Drivers
			   ON ReferralAccount.DriverId = Drivers.DriverId";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS6.Clear();
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                    if (cmbSReferralId.SelectedIndex > 0)
                    {
                        WhereStringList.Add("Customers.SangHo = @ReferralName ");
                        _Command.Parameters.AddWithValue("@ReferralName", cmbSReferralId.Text);
                    }


                  


                    WhereStringList.Add("ReferralAccount.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    WhereStringList.Add("CONVERT(VARCHAR(10),PayDate,111) >= @Sdate AND CONVERT(VARCHAR(10),PayDate,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate",dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    
                    




                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by PayDate  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS6.Load(_Reader);

                    }
                }

                
                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS6.ToArray();
            
            

            if (Query.Any())
            {
                lblAmount.Text = Query.Sum(c => c.Amount).ToString("N0");
                lblOutAmount.Text = Query.Sum(c => c.OutAmount).ToString("N0");
            }
           

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

       

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS6Row;


            if (ModelDataGrid.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (ModelDataGrid.Rows.Count - e.RowIndex).ToString("N0");
            }

            else if (e.ColumnIndex == payDateDataGridViewTextBoxColumn.Index)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
            else if (e.ColumnIndex == Amount.Index)
            {
                e.Value = Selected.Amount;
            }

            else if(e.ColumnIndex == Column2.Index)
            {
                e.Value = Selected.OrderSdate.ToString("d") + " - " + Selected.OrderEdate.ToString("d");

            }
        }

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMonth.SelectedIndex)
            {
                //당일
                case 0:
                    dtp_Sdate.Value = DateTime.Now;
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전일
                case 1:
                    dtp_Sdate.Value = DateTime.Now.AddDays(-1);
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //금주
                case 2:
                    dtp_Sdate.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //금월
                case 3:
                    dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전월
                case 4:
                    dtp_Sdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 5:
                    dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                    dtp_Edate.Value = DateTime.Now;
                    break;
            }
        }

        private void txtSText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            LoadTable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(Gubun))
            {              

                MessageBox.Show("새로 등록하시려면 신규 버튼을 클릭하세요","위탁사 수수료 입금관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (rdoJusun.Checked)
            {
                if (cmbJusun.Items.Count == 0)
                {
                    MessageBox.Show("주선사를 선택하세요", "위탁사 수수료 입금관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if(rdoDriver.Checked)
            {
                if (cmbDriver.Items.Count == 0)
                {
                    MessageBox.Show("차주를 선택하세요", "위탁사 수수료 입금관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                if (cmbReferralId.Items.Count == 0)
                {
                    MessageBox.Show("거래처 관리에서 위탁사를 등록후 사용하세요", "위탁사 수수료 입금관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            
            

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                int _ReferralAccountId = 0;
                _Connection.Open();

                if (Gubun == "New")
                {
                    //if(String.IsNullOrEmpty(txtAmount.Text))
                    //{
                    //    MessageBox.Show("수금액을 입력하세요", "위탁사입금내역", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "INSERT ReferralAccount ( PayDate, CustomerId,Amount, OutAmount, ClientId,OrderSdate,OrderEdate,DriverId)Values( @PayDate, @CustomerId,@Amount, @OutAmount, @ClientId,@OrderSdate,@OrderEdate,@DriverId)" +
                            " SELECT @@Identity";
                        _Command.Parameters.Add("@PayDate", SqlDbType.DateTime);
                        _Command.Parameters.Add("@CustomerId", SqlDbType.Int);
                        _Command.Parameters.Add("@Amount", SqlDbType.Decimal);
                        _Command.Parameters.Add("@OutAmount", SqlDbType.Decimal);
                        _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                        
                        _Command.Parameters.Add("@OrderSdate", SqlDbType.DateTime);
                        _Command.Parameters.Add("@OrderEdate", SqlDbType.DateTime);
                        _Command.Parameters.Add("@DriverId", SqlDbType.Int);

                        _Command.Parameters["@PayDate"].Value = dtpPayDate.Value;
                        _Command.Parameters["@OrderSdate"].Value = dtpOrderSdate.Value;
                        _Command.Parameters["@OrderEdate"].Value = dtpOrderEdate.Value;



                        if (rdoJusun.Checked)
                        {
                            _Command.Parameters["@CustomerId"].Value = cmbJusun.SelectedValue;
                            _Command.Parameters["@DriverId"].Value = 0;
                        }
                        else if (rdoDriver.Checked)
                        {
                            _Command.Parameters["@CustomerId"].Value = 0;
                            _Command.Parameters["@DriverId"].Value = cmbDriver.SelectedValue;
                        }
                        else
                        {
                            _Command.Parameters["@CustomerId"].Value = cmbReferralId.SelectedValue;
                            _Command.Parameters["@DriverId"].Value = 0;
                        }
                       
                        _Command.Parameters["@Amount"].Value = txtSSum.Text;


                        Int64 _OutAmount = 0;
                        if (String.IsNullOrEmpty(txtOutAmount.Text))
                        {
                            _OutAmount = 0;
                        }
                        else
                        {
                            _OutAmount = Convert.ToInt64(txtOutAmount.Text);
                        }

                        _Command.Parameters["@OutAmount"].Value = _OutAmount;


                        _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;

                        //  _Command.ExecuteNonQuery();
                        _ReferralAccountId = Convert.ToInt32(_Command.ExecuteScalar());


                        try
                        {

                            MessageBox.Show("저장되었습니다.");
                           
                        }
                        catch
                        {

                        }
                    }

                    if (OrderIds.Count > 0)
                    {

                        using (SqlCommand _OrderCommand = _Connection.CreateCommand())
                        {

                            _OrderCommand.CommandText = "Update Orders SET ReferralAccountYN = 'Y',ReferralAccountId = @ReferralAccountId WHERE Orderid in(" + String.Join(",", OrderIds) + ") AND (PayLocation = 2 or PayLocation = 4) ";

                            _OrderCommand.Parameters.AddWithValue("ReferralAccountId", _ReferralAccountId);

                            _OrderCommand.ExecuteNonQuery();


                         
                        }
                    }

                }
                else
                {
                    //if (nSTATS6BindingSource.Current == null)
                    //    return;


                    //if (iidx == 0)
                    //    return;

                    //using (SqlCommand _UpCommand = _Connection.CreateCommand())
                    //{
                    //    _UpCommand.CommandText = "Update ReferralAccount" +
                    //        " SET  PayDate = @PayDate" +
                    //        ", CustomerId = @CustomerId" +
                    //        ", Amount = @Amount" +
                    //        ", OutAmount = @OutAmount" +
                    //        ", OrderSdate = @OrderSdate" +
                    //        ", OrderEdate = @OrderEdate" +
                    //        " WHERE idx = @idx";
                    //    _UpCommand.Parameters.Add("@PayDate", SqlDbType.DateTime);
                    //    _UpCommand.Parameters.Add("@CustomerId", SqlDbType.Int);
                    //    _UpCommand.Parameters.Add("@Amount", SqlDbType.Decimal);
                    //    _UpCommand.Parameters.Add("@OutAmount", SqlDbType.Int);
                    //    _UpCommand.Parameters.Add("@ClientId", SqlDbType.Int);
                    //    _UpCommand.Parameters.Add("@idx", SqlDbType.Int);
                    //    _UpCommand.Parameters.Add("@OrderSdate", SqlDbType.DateTime);
                    //    _UpCommand.Parameters.Add("@OrderEdate", SqlDbType.DateTime);



                     

                    //    _UpCommand.Parameters["@idx"].Value = iidx;

                    //    _UpCommand.Parameters["@PayDate"].Value = dtpPayDate.Value;
                    //    _UpCommand.Parameters["@OrderSdate"].Value = dtpOrderSdate.Value;
                    //    _UpCommand.Parameters["@OrderEdate"].Value = dtpOrderEdate.Value;
                    //    if (rdoJusun.Checked)
                    //    {
                    //        _UpCommand.Parameters["@CustomerId"].Value = txtJusun.Tag;
                    //    }
                    //    else
                    //    {
                    //        _UpCommand.Parameters["@CustomerId"].Value = cmbReferralId.SelectedValue;
                    //    }

                    //    Int64 _Amount = 0;
                    //    _Amount = Convert.ToInt64(txtSSum.Text.Replace(",",""));

                    //    _UpCommand.Parameters["@Amount"].Value = _Amount;



                    //    Int64 _OutAmount = 0;
                    //    if (String.IsNullOrEmpty(txtOutAmount.Text))
                    //    {
                    //        _OutAmount = 0;
                    //    }
                    //    else
                    //    {
                    //        _OutAmount = Convert.ToInt64(txtOutAmount.Text);
                    //    }

                    //    _UpCommand.Parameters["@OutAmount"].Value = _OutAmount;


                    //    _UpCommand.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;

                    //    _UpCommand.ExecuteNonQuery();

                    //    try
                    //    {

                    //        MessageBox.Show("수정되었습니다.");
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}

                }

                _Connection.Close();
            }

            LoadTable();
            Ssum();

        }

        private void nSTATS6BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (nSTATS6BindingSource.Current == null)
                return;


            if (((DataRowView)nSTATS6BindingSource.Current).Row is NSTATSDataSet.NSTATS6Row Selected)
            {
                iidx = Selected.idx;
            }

              
            }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Gubun = "New";

            if (cmbReferralId.Items.Count > 0)
            {
                cmbReferralId.SelectedIndex = 0;
            }
            
            dtpPayDate.Value = DateTime.Now;
            //txtAmount.Text = "";
            txtOutAmount.Text = "0";
            btnSave.Text = "저장";
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txtOutAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (nSTATS6BindingSource.Current == null)
                return;


            if (MessageBox.Show("삭제 하시겠습니까?", "위탁사 수수료 입금관리", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {


                Data.Connection(_Connection =>
                {
                    

                    using (SqlCommand _OrderCommand = _Connection.CreateCommand())
                    {

                        _OrderCommand.CommandText = "Update Orders SET ReferralAccountYN = 'N',ReferralAccountId = 0 WHERE ReferralAccountId = @ReferralAccountId ";

                        _OrderCommand.Parameters.AddWithValue("ReferralAccountId", iidx);

                        _OrderCommand.ExecuteNonQuery();



                    }

                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "DELETE  ReferralAccount WHERE idx = @idx";
                        _Command.Parameters.AddWithValue("@idx", iidx);
                        _Command.ExecuteNonQuery();
                    }


                });



                try
                {

                    MessageBox.Show("삭제되었습니다.");
                    LoadTable();
                    Ssum();
                }
                catch
                {

                }
            }
        }

        private void rdoJusun_CheckedChanged(object sender, EventArgs e)
        {
            //if(rdoJusun.Checked)
            //{
            //    lblWe.Text = "주선사";
            //    txtJusun.Visible = true;
            //    cmbReferralId.Visible = false;
            //}
            //else
            //{
            //    lblWe.Text = "위탁사";
            //    txtJusun.Visible = false;
            //    cmbReferralId.Visible = true;
            //}


        }

        private void txtJusun_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (nSTATS6BindingSource.Current != null)
            //{
            //    var Current = nSTATS6BindingSource.Current as Order;
               
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


            //Ssum();
        }

        private void JusunSelect_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in JusunSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _JusunSelected();
                    }
                }
            }
        }

        public void btn_Inew_Click(object sender, EventArgs e)
        {
            cmbSMonth.SelectedIndex = 5;
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
            dtp_Edate.Value = DateTime.Now;
            cmbSReferralId.SelectedIndex = 0;
            btnSearch_Click(null, null);
        }

        List<int> OrderIds = new List<int>();
        private void Ssum()
        {
            if (rdoJusun.Checked)
            {
                if (cmbJusun.SelectedValue == null)
                {
                    return;
                }
            }
            else if(rdoDriver.Checked)
            {
                if (cmbDriver.SelectedValue == null)
                {
                    return;
                }
            }
            else
            {
            
              

                if (cmbReferralId.SelectedValue == null)
                {
                    return;
                }
            }


           List <Order> DataSource = new List<Order>();
            int _CustomerId = 0 ;
            int _DriverId = 0;
            if (rdoJusun.Checked)
            {
                if (cmbJusun.SelectedValue != null)
                {
                    try
                    {
                        _CustomerId = (int)cmbJusun.SelectedValue;
                    }
                    catch
                    {
                        _CustomerId = Convert.ToInt32(DCustomerJ.First().Key.ToString());
                    }
                }
            }
            else if(rdoDriver.Checked)
            {
                if (cmbDriver.SelectedValue != null)
                {
                    try
                    {
                        _DriverId = (int)cmbDriver.SelectedValue;
                    }
                    catch
                    {
                        _DriverId = Convert.ToInt32(DDriver.First().Key.ToString());
                    }
                }
            }
            else
            {
                if (cmbReferralId.SelectedValue != null )
                {
                    try
                    {
                        _CustomerId = (int)cmbReferralId.SelectedValue;
                    }
                    catch
                    {
                        _CustomerId = Convert.ToInt32(DCustomerI.First().Key.ToString());
                    }
                }
            }



           
          


            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                
                var Edate = dtpOrderEdate.Value.AddDays(1).Date;
                if (rdoDriver.Checked)
                {
                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 &&c.FOrderId!= 0 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CreateTime >= dtpOrderSdate.Value.Date && c.CreateTime < Edate && c.DriverId == _DriverId && c.ReferralAccountYN == "N").OrderByDescending(c => c.CreateTime).ToList();
                }
                else
                {
                    DataSource = ShareOrderDataSet.Orders.Include("DriverModel").Include("TradeModel").Where(c => c.OrderStatus == 3 && (c.PayLocation == 2 || c.PayLocation == 4) && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CreateTime >= dtpOrderSdate.Value.Date && c.CreateTime < Edate && c.ReferralId == _CustomerId && c.ReferralAccountYN == "N").OrderByDescending(c => c.CreateTime).ToList();
                }
           
            }

            OrderIds.Clear();
            if (DataSource.Any())
            {

                foreach (var _Orderid in DataSource)
                {
                    OrderIds.Add(_Orderid.OrderId);
                }

                txtSSum.Text = DataSource.Sum(c => c.DriverPrice ?? 0).ToString("N0");

            }
            else
            {
                txtSSum.Text = "0";
            }

            txtOutAmount.Text = "0";


        }

        private void ReferralIdChange()
        {
            int _CustomerId = 0;
            if (cmbReferralId.SelectedValue != null)
            {
                try
                {
                    _CustomerId = (int)cmbReferralId.SelectedValue;
                }
                catch
                {
                    _CustomerId = Convert.ToInt32(DCustomerI.First().Key.ToString());
                }
            }


          
            Ssum();
        }

        private void DriverIdChange()
        {
            int _DriverrId = 0;
            if (cmbDriver.SelectedValue != null)
            {
                try
                {
                    _DriverrId = (int)cmbDriver.SelectedValue;
                }
                catch
                {
                    _DriverrId = Convert.ToInt32(DDriver.First().Key.ToString());
                }
            }



            Ssum();
        }
        private void dtpOrderEdate_ValueChanged(object sender, EventArgs e)
        {
            var _Sdate = dtpOrderSdate.Value.Date;
            var _Edate = dtpOrderEdate.Value.Date.AddDays(1);
            var cmbCustomerMIdDataSource2 = clientDataSet.CustomersNew.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                //WDataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && c.ReferralId != 0 && c.CreateTime >= _Sdate && c.CreateTime < _Edate).ToList();

                WDataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && (c.PayLocation == 2 || c.PayLocation == 4) && c.OrderStatus == 3 && c.ReferralId != 0 && c.CreateTime >= _Sdate && c.CreateTime < _Edate && (c.ReferralAccountYN == "N" || c.ReferralAccountYN == "")).ToList();
            }
            if (WDataSource.Any())
            {
                DCustomerI.Clear();

                foreach (var item in WDataSource.GroupBy(c => c.ReferralId))
                {
                    var Query = cmbCustomerMIdDataSource2.Where(c => c.CustomerId == item.First().ReferralId);
                    if (Query.Any())
                    {
                        // DCustomerI.Add((int)item.First().ReferralId, Query.First().SangHo);
                        if (item.Sum(c => c.DriverPrice ?? 0) > 0)
                        {
                            DCustomerI.Add((int)item.First().ReferralId, Query.First().SangHo);
                        }
                    }
                }

                if (DCustomerI.Any())
                {

                    cmbReferralId.DataSource = new BindingSource(DCustomerI, null);
                    cmbReferralId.DisplayMember = "Value";
                    cmbReferralId.ValueMember = "Key";
                    cmbReferralId.SelectedIndex = 0;
                }
            }
            else
            {
                // DCustomerI.Clear();
                cmbReferralId.DataSource = null;
            }

            Ssum();

        }

        private void dtpOrderSdate_ValueChanged(object sender, EventArgs e)
        {

            var _Sdate = dtpOrderSdate.Value.Date;
            var _Edate = dtpOrderEdate.Value.Date.AddDays(1);
            var cmbCustomerMIdDataSource2 = clientDataSet.CustomersNew.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
               

                WDataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && (c.PayLocation == 2 || c.PayLocation == 4) && c.OrderStatus == 3 && c.ReferralId != 0 && c.CreateTime >= _Sdate && c.CreateTime < _Edate && (c.ReferralAccountYN =="N" || c.ReferralAccountYN == "")).ToList();
            }
            if (WDataSource.Any())
            {
                DCustomerI.Clear();

                foreach (var item in WDataSource.GroupBy(c => c.ReferralId))
                {
                    var Query = cmbCustomerMIdDataSource2.Where(c => c.CustomerId == item.First().ReferralId);
                    if (Query.Any())
                    {
                        if (item.Sum(c => c.DriverPrice ?? 0) > 0)
                        {
                            DCustomerI.Add((int)item.First().ReferralId, Query.First().SangHo);
                        }
                    }
                }

                if (DCustomerI.Any())
                {

                    cmbReferralId.DataSource = new BindingSource(DCustomerI, null);
                    cmbReferralId.DisplayMember = "Value";
                    cmbReferralId.ValueMember = "Key";
                    cmbReferralId.SelectedIndex = 0;
                }
            }
            else
            {
               // DCustomerI.Clear();
                cmbReferralId.DataSource = null;
            }







            Ssum();
        }

        private void cmbReferralId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReferralIdChange();
          //  Ssum();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372111212");
        }
        private void rdoChk()
        {
            if (rdoJusun.Checked)
            {
                lblWe.Text = "거래처선택";
               
                cmbJusun.Visible = true;
                txtSSum.Text = "0";
                cmbReferralId.Visible = false;
                cmbDriver.Visible = false;
            }
            else if (rdoDriver.Checked)
            {
                lblWe.Text = "거래처선택";
                txtSSum.Text = "0";
                cmbDriver.Visible = true;
                cmbJusun.Visible = false;
                cmbReferralId.Visible = false;

            }
            else
            {
                lblWe.Text = "거래처선택";
                cmbJusun.Visible = false;
                cmbReferralId.Visible = true;
                cmbDriver.Visible = false;


            }

            Ssum();
        }
        private void rdoWe_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();
        }

        private void rdoJusun_CheckedChanged_1(object sender, EventArgs e)
        {
            rdoChk();
        }

        private void rdoDriver_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();
        }
    }
}
