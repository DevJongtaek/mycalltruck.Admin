using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.XML;
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
//using mycalltruck.Admin.CMDataSetTableAdapters;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using OfficeOpenXml.Style;
using mycalltruck.Admin.DataSets.ClientDataSetTableAdapters;

namespace mycalltruck.Admin
{
    public partial class FrmMN0304_OverView : Form
    {


        // UCNSTATS1 ucNSTATS1 = new UCNSTATS1();
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

                   


                    grid1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }

        int GridIndex = 0;
        int _OrderId = 0;
        public int _SOrderId = 0;
        DateTimePicker dtp = new DateTimePicker();
        ContextMenuStrip m = new ContextMenuStrip();


        public FrmMN0304_OverView()
        {
            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);


            m.Items.Add("배차일자수정");
            m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
            dtp.CloseUp += new EventHandler(dtp_CloseUp);
            dtp.ValueChanged += new EventHandler(dtp_OnTextChange);

        }

        private void FrmMN0304_Load(object sender, EventArgs e)
        {
            //NSTAT1();

            dtp_Sdate.Value = DateTime.Now.AddDays(-1);
            dtp_Edate.Value = DateTime.Now;
            InitCmb();
            //InitCustomerAndTeamTable();
            grid1.Controls.Add(dtp);
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.Visible = false;
            btn_Inew_Click(null, null);



        }
        private List<CustomerAndTeamViewModel> _CustomerTeamTable = new List<CustomerAndTeamViewModel>();
        private void InitCustomerAndTeamTable(int _CustomerId)
        {
            _CustomerTeamTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT CustomerTeamId as  idx, CustomerId ,TeamName " +
                        " FROM CustomerTeams " +
                        " WHERE(ClientId = @ClientId) AND CustomerId = @CustomerId" +

                        " ORDER BY CreateDate DESC";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    commnad.Parameters.AddWithValue("@CustomerId", _CustomerId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _CustomerTeamTable.Add(
                              new CustomerAndTeamViewModel
                              {
                                  TeamId = dataReader.GetInt32(0),
                                  CustomerId = dataReader.GetInt32(1),

                                  Part = dataReader.GetString(2),


                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        private void cmbSearchBinding()
        {
            Dictionary<int, string> DCustomerI = new Dictionary<int, string>();

            DCustomerI.Clear();
            cmbSReferralId.DataSource = null;
            customersTableAdapter.FillBy(clientDataSet.Customers, LocalUser.Instance.LogInInformation.ClientId);
            var cmbCustomerMIdDataSource2 = clientDataSet.Customers.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();
            List<Order> DataSource = new List<Order>();
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                DateTime _Sdate = dtp_Sdate.Value.Date;
                DateTime _Edate = dtp_Edate.Value.AddDays(1).Date;


                DataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && c.ReferralId != 0 && c.CreateTime >= _Sdate && c.CreateTime < _Edate && (c.ReferralAccountYN == "N" || c.ReferralAccountYN == "")).ToList();


            }
            DCustomerI.Clear();
            DCustomerI.Add(0, "위탁사");
            if (DataSource.Any())
            {


                foreach (var item in DataSource.GroupBy(c => c.ReferralId))
                {
                    var Query = cmbCustomerMIdDataSource2.Where(c => c.CustomerId == item.First().ReferralId);
                    if (Query.Any())
                    {

                        DCustomerI.Add((int)item.First().ReferralId, Query.First().SangHo);

                    }
                }

                if (DCustomerI.Any())
                {

                    cmbSReferralId.DataSource = new BindingSource(DCustomerI, null);
                    cmbSReferralId.DisplayMember = "Value";
                    cmbSReferralId.ValueMember = "Key";
                    cmbSReferralId.SelectedIndex = 0;
                }
            }
            else
            {
                // DCustomerI.Clear();
                cmbSReferralId.DataSource = null;
            }




            // DataLoad();

        }

        private void cmbCustomerSearchBinding()
{
            //Dictionary<int, string> DCustomerS = new Dictionary<int, string>();
            //DCustomerS.Clear();
            //cmbCustomers.DataSource = null;
            //customersTableAdapter.FillBy(clientDataSet.Customers, LocalUser.Instance.LogInInformation.ClientId);
            //var cmbCustomerSDataSource = clientDataSet.Customers.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();
            //List<Order> customerDataSource = new List<Order>();
            //using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            //{
            //    DateTime _Sdate = dtp_Sdate.Value.Date;
            //    DateTime _Edate = dtp_Edate.Value.AddDays(1).Date;


            //    customerDataSource = ShareOrderDataSet.Orders.Include("CustomerModel").Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && c.ReferralId != 0 && c.CreateTime >= _Sdate && c.CreateTime < _Edate && (c.ReferralAccountYN == "N" || c.ReferralAccountYN == "")).ToList();


            //}
            //DCustomerS.Clear();
            //DCustomerS.Add(0, "거래처");
            //if (customerDataSource.Any())
            //{


            //    foreach (var item in customerDataSource.GroupBy(c => c.CustomerId))
            //    {
            //        var Query = cmbCustomerSDataSource.Where(c => c.CustomerId == item.First().CustomerId);
            //        if (Query.Any())
            //        {

            //            DCustomerS.Add((int)item.First().CustomerId, Query.First().SangHo);

            //        }
            //    }

            //    if (DCustomerS.Any())
            //    {

            //        cmbCustomers.DataSource = new BindingSource(DCustomerS, null);
            //        cmbCustomers.DisplayMember = "Value";
            //        cmbCustomers.ValueMember = "Key";
            //        cmbCustomers.SelectedIndex = 0;
            //    }
            //}
            //else
            //{
            //    // DCustomerI.Clear();
            //    cmbCustomers.DataSource = null;
            //}
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


            //Dictionary<int, string> DCustomer = new Dictionary<int, string>();
            //customersTableAdapter.FillBy(clientDataSet.Customers, LocalUser.Instance.LogInInformation.ClientId);
            //var cmbCustomerMIdDataSource = clientDataSet.Customers.Where(c => c.BizGubun == 4).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();

            //DCustomer.Add(0, "전체");


            //foreach (var item in cmbCustomerMIdDataSource)
            //{
            //    DCustomer.Add(item.CustomerId, item.SangHo);
            //}


            //cmbSReferralId.DataSource = new BindingSource(DCustomer, null);
            //cmbSReferralId.DisplayMember = "Value";
            //cmbSReferralId.ValueMember = "Key";
            //cmbSReferralId.SelectedValue = 0;






            Dictionary<string, string> SpayLocation = new Dictionary<string, string>
            {
                { "전체", "전체" },
                { "인수증", "인수증" },
                { "선/착불", "선/착불" },
                { "수수료확인", "수수료확인" }
            };


            cmbSPayLocation.DataSource = new BindingSource(SpayLocation, null);
            cmbSPayLocation.DisplayMember = "Value";
            cmbSPayLocation.ValueMember = "Key";

            cmbSPayLocation.SelectedIndex = 0;


            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {
                { "전체", "전체" },
                //{ "배차일시", "배차일시" },
                { "거래처명", "거래처명" },
                { "차량번호", "차량번호" },
                { "기사명", "기사명" },
                { "핸드폰번호", "핸드폰번호" },
                { "화주담당자", "화주담당자" }

            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


        }
        private String GetSelectCommand()
        {
            return @"SELECT 
                    Orders.CreateTime
                    ,ISNULL(Orders.Customer,'') as CustomerName
                    ,Orders.DriverCarNo
                    ,Orders.Driver
                    ,Orders.DriverPhoneNo
                    ,ISNULL(Orders.StartName,'') as StartName
                    ,ISNULL(Orders.StartMemo,'') as StartMemo
                    ,ISNULL(Orders.StopName,'') as StopName
                    ,ISNULL(Orders.StopMemo,'') as StopMemo
                    ,Orders.Item
                    ,Orders.ItemSize
                    ,Orders.CarCount
                    ,Orders.CarSize
                    ,CarSize.Name as CarSizeName
                    ,Orders.CarType
                    ,CarType.Name  as CarTypeName
                    ,Orders.PayLocation
                    ,PayLocation.Name as PayLocationName
                    ,Orders.IsShared
                    ,ISNULL(Convert(varchar(10),Trades.RequestDate,111),'') as TradesRequestDate
                    ,ISNULL(Customers.SangHo,'') AS ReferralName --위탁사명
                    ,ISNULL(Orders.StartPrice,0) AS StartPrice --선불운임
                    ,ISNULL(Orders.StopPrice,0) AS StopPrice --착불운임
                    ,ISNULL(Orders.DriverPrice,0) as DriverPrice -- 수수료
                    ,ISNULL(Orders.TradePrice,0) as TradePrice -- 지불운임
                    ,ISNULL(Orders.SalesPrice,0) as SalesPrice -- 청구
                    ,ISNULL(Orders.SalesPrice,0) - ISNULL(Orders.TradePrice,0) as STPrice --후불이익
                    , CASE  ISNULL(Orders.DriverPrice,0) WHEN 0 then ISNULL(Orders.SalesPrice,0) - ISNULL(Orders.TradePrice,0) ELSE ISNULL(Orders.DriverPrice,0) END AS SonIk
                    ,ISNULL(Orders.AlterPrice,0)  AS AlterPrice -- 보관금
                    ,0 AS SunAMount
                    ,ISNULL(Orders.RequestMemo,'') as RequestMemo
                    --,case when orders.OrdersLoginId IS NULL THEN  Clients.CEO else OrdersLoginId END as CEO
                    ,case when orders.OrdersAcceptId IS NULL THEN  '' else OrdersAcceptId END as CEO
                    ,Orders.ClientId
                    ,Orders.OrderId
                    ,Orders.DriverId
                    ,Orders.UnitItem
                    ,Orders.UnitType
                    ,CASE WHEN Trades.PayState = 1 THEN Convert(varchar,Trades.PayDate,111) ELSE '' END as PayDate
                    ,ISNULL(Orders.SalesManageId,0) as SalesManageId
                    ,ISNULL(Orders.CustomerTeam,0) as CustomerTeam
                    ,ISNULL(Orders.CustomerManager,'') as CustomerManager
                    FROM 
                    Orders
                    JOIN StaticOptions AS CarSize 
                    ON Orders.CarSize = CarSize.Value
                    AND CarSize.Div = 'CarSize'
                    JOIN StaticOptions AS CarType 
                    ON Orders.CarType = CarType.Value
                    AND CarType.Div = 'CarType'
                    JOIN StaticOptions AS PayLocation 
                    ON Orders.PayLocation = PayLocation.Value
                    AND PayLocation.Div = 'PayLocation'
                    LEFT JOIN Trades ON Orders.TradeId = Trades.TradeId
                    LEFT JOIN Customers ON Orders.ReferralId = Customers.CustomerId AND Customers.BizGubun = 4
                    
                    JOIN Clients ON Orders.ClientId = Clients.ClientId
                    JOIN Drivers ON Orders.DriverId = Drivers.DriverId
                    LEFT JOIN SalesManage ON Orders.SalesManageId = SalesManage.SalesId";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS1.Clear();


            var mClientUsesAdapter = new ClientUsersTableAdapter();
            var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);


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


                    if (mTable.Any())
                    {
                        if (mTable.First().CustomerId > 0)
                        {
                            WhereStringList.Add("Orders.CustomerId = @CustomerId ");
                            _Command.Parameters.AddWithValue("@CustomerId", mTable.First().CustomerId);
                        }
                        if (mTable.First().CustomerTeam > 0)
                        {

                            WhereStringList.Add("Orders.CustomerTeam = @CustomerTeam ");
                            _Command.Parameters.AddWithValue("@CustomerTeam", mTable.First().CustomerTeam);
                        }
                    }


                    if (cmbSPayLocation.SelectedIndex > 0)
                    {
                        WhereStringList.Add("PayLocation.Name = @PayLocation ");
                        _Command.Parameters.AddWithValue("@PayLocation", cmbSPayLocation.Text);
                    }


                    WhereStringList.Add("Orders.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    WhereStringList.Add("CONVERT(VARCHAR(10),orders.CreateTime,111) >= @Sdate AND CONVERT(VARCHAR(10),orders.CreateTime,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate", dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    WhereStringList.Add("Orders.OrderStatus = 3 ");


                    if (cmbSSearch.Text == "거래처명")
                    {
                        WhereStringList.Add(string.Format("Orders.Customer Like '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("DriverCarNo Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "기사명")
                    {
                        WhereStringList.Add(string.Format("Driver Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "핸드폰번호")
                    {
                        WhereStringList.Add(string.Format("DriverPhoneNo Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "화주담당자")
                    {
                        WhereStringList.Add(string.Format("CustomerManager Like  '%{0}%'", txtSText.Text));
                    }

                    if (!LocalUser.Instance.LogInInformation.IsClient)
                    {
                        WhereStringList.Add(string.Format("Orders.CustomerId  = '{0}'", LocalUser.Instance.LogInInformation.CustomerId));
                    }
                    



                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by orders.CreateTime  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS1.Load(_Reader);


                        if (grid1.RowCount > 0)
                        {
                            // GridIndex = ModelDataGrid.Position;
                            //  _Search();
                            grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];

                            nSTATS1BindingSource_CurrentChanged(null, null);
                        }


                    }
                }


                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS1.ToArray();

            if (Query.Any())
            {
                lblSalesPrice.Text = Query.Sum(c => c.SalesPrice).ToString("N0");
                lblTradePrice.Text = Query.Sum(c => c.TradePrice).ToString("N0");
                lblSTPrice.Text = (Query.Sum(c => c.SalesPrice) - Query.Sum(c => c.TradePrice)).ToString("N0");
                lblAlterPrice.Text = Query.Sum(c => c.AlterPrice).ToString("N0");
                lblStartPrice.Text = Query.Sum(c => c.StartPrice).ToString("N0");
                lblStopPrice.Text = Query.Sum(c => c.StopPrice).ToString("N0");
                lblDriverPrice.Text = Query.Sum(c => c.DriverPrice).ToString("N0");
            }
        }



        private void NSTAT1()
        {

            //this.panel2.Controls.Clear();
            //this.panel2.Controls.Add(ucNSTATS1);
            //ucNSTATS1.Dock = System.Windows.Forms.DockStyle.Fill;
            //ucNSTATS1.Location = new System.Drawing.Point(0, 0);
            //ucNSTATS1.Name = "ucNSTATS1";
            //ucNSTATS1.Size = new System.Drawing.Size(804, 579);
            //ucNSTATS1.TabIndex = 0;
            //ucNSTATS1.btn_Inew_Click(null, null);



           

            //ucNSTATS1.grid1.CellDoubleClick += new DataGridViewCellEventHandler(model_DoubleClick);
          

        }

        void model_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           

           



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (grid1.RowCount < 1)
            {
                MessageBox.Show("출력할 내용이 없습니다. 먼저 데이터를 조회하신 후 출력 버튼을 눌려주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            FrmNStatistics f = new FrmNStatistics();
            //string order = string.Empty;
            //if (ModelDataGrid.SortedColumn != null)
            //{
            //    order = ModelDataGrid.SortedColumn.DataPropertyName;
            //}
            string jogun = string.Empty;

            jogun = "위탁사 : " + cmbSReferralId.Text + "  운임조건 : " + cmbSPayLocation.Text;

            f.NSTATS1(dtp_Sdate.Text, dtp_Edate.Text, jogun, nSTATS1BindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();



        }

        private void grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1Row;


            if (grid1.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (grid1.Rows.Count - e.RowIndex).ToString("N0");
            }

            else if (e.ColumnIndex == acceptTimeDataGridViewTextBoxColumn.Index)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
            else if (e.ColumnIndex == isSharedDataGridViewTextBoxColumn.Index)
            {
                if (Selected.IsShared == true)
                {
                    e.Value = "혼적";
                }
                else
                {
                    e.Value = "독차";
                }

            }

            else if(e.ColumnIndex == ColumnCustomerTeam.Index)
            {
                string R = "";


                if (Selected.CustomerTeam != 0 && Selected.CustomerTeam != null)
                {

                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                                @"SELECT  TeamName FROM    CustomerTeams 
                                    WHERE CustomerTeamId = @CustomerTeamId";
                            _Command.Parameters.AddWithValue("@CustomerTeamId", Selected.CustomerTeam);
                            object O = _Command.ExecuteScalar();
                            if (O != null)
                            {
                                R = O.ToString();
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
                    e.Value = "본사";

                }

            }
            else if (e.ColumnIndex == cEODataGridViewTextBoxColumn.Index)
            {
                string R = "";

                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                            @"SELECT  CEO FROM  Clients
                                    WHERE LoginId = @LoginId  ";
                        _Command.Parameters.AddWithValue("@LoginId", e.Value.ToString());
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
                //var mClientUsesAdapter = new ClientUsersTableAdapter();
                //var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);

                //if (mTable.Any(c => c.LoginId == e.Value.ToString()))
                //{
                //    e.Value = mTable.Where(c => c.LoginId == e.Value.ToString()).First().Name;
                //}
                //else
                //{
                //    e.Value = _clients "";
                //}


            }

            else if (e.ColumnIndex == itemSizeDataGridViewTextBoxColumn.Index)
            {
                //DriverRepository mDriverRepository = new DriverRepository();
                //if (Selected.DriverId != 0)
                //{
                    
                //    var Query = mDriverRepository.NoGetDriver((int)Selected.DriverId);


                //    if (Query.CarSize != 0 )
                //    {
                //        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == Query.CarSize))
                //            e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == Query.CarSize).Name;
                //    }
                    

                //}
            }
            else if (e.ColumnIndex == carSizeNameDataGridViewTextBoxColumn.Index)
            {
                e.Value = Selected.UnitItem;
            }

            else if (e.ColumnIndex == carTypeNameDataGridViewTextBoxColumn.Index)
            {
                if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "Unit" && c.Value == Selected.UnitType))
                    e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "Unit" && c.Value == Selected.UnitType).Name;
               
            }
            else if(e.ColumnIndex == PayDateDataGridViewTextBoxColumn.Index)
            {
                string R = "";

                
                if (Selected.SalesManageId != 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                                @"SELECT  PayDate FROM    SalesManage
                                    WHERE SalesId = @SalesManageId AND Paystate = 1 ";
                            _Command.Parameters.AddWithValue("@SalesManageId", Selected.SalesManageId);
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
                    dtp_Edate.Value = DateTime.Now.AddDays(-1);
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

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmbSMonth.SelectedIndex = 0;
            dtp_Sdate.Value = DateTime.Now;
            dtp_Edate.Value = DateTime.Now;

            cmbSPayLocation.SelectedIndex = 0;
            cmbSSearch.SelectedIndex = 0;
            txtSText.Clear();
            btnSearch_Click(null, null);
        }

        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            //  GridIndex = nSTATS1BindingSource.Position;

            //ModelDataGrid.CurrentCell = ModelDataGrid.Rows[GridIndex].Cells[1];


            grid1.Rows[GridIndex].Cells[1].Value = dtp.Text.ToString();


            _OrderId = (int)grid1.Rows[GridIndex].Cells[33].Value;

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                    "Update Orders SET CreateTime = @CreateTime WHERE OrderId = @OrderId";

                cmd.Parameters.AddWithValue("@OrderId", _OrderId);
                cmd.Parameters.AddWithValue("@CreateTime", dtp.Value);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

            // btnSearch_Click(null, null);
            // MessageBox.Show(OrderId.ToString());
        }
        void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }

        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //   GridIndex = nSTATS1BindingSource.Position;

            //   ModelDataGrid.Rows[GridIndex].Cells[1].Value = dtp.Text.ToString();


            dtp.Value = (DateTime)grid1.Rows[GridIndex].Cells[1].Value;
            Rectangle Rectangle = grid1.GetCellDisplayRectangle(1, GridIndex, true);
            dtp.Size = new Size(Rectangle.Width, Rectangle.Height);
            dtp.Location = new Point(Rectangle.X, Rectangle.Y);





            dtp.Visible = true;


        }

        private void grid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1Row;


            if (e.Button == MouseButtons.Right)
            {


                if (e.ColumnIndex == acceptTimeDataGridViewTextBoxColumn.Index)
                {



                    Rectangle Rectangle = grid1.GetCellDisplayRectangle(e.ColumnIndex, GridIndex, true);
                    m.Show(grid1, new Point(Rectangle.X, Rectangle.Y));


                }
            }
            else
            {

                if (e.ColumnIndex == acceptTimeDataGridViewTextBoxColumn.Index)
                {
                    acceptTimeDataGridViewTextBoxColumn.ReadOnly = true;
                }

                dtp.Visible = false;
            }
        }

        private void nSTATS1BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (nSTATS1BindingSource.Current == null)
            {
                return;
            }
            else
            {
                var Selected = ((DataRowView)nSTATS1BindingSource.Current).Row as NSTATSDataSet.NSTATS1Row;

                GridIndex = nSTATS1BindingSource.Position;
                _SOrderId = Selected.OrderId;
            }
        }

        private void grid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1Row;
            _SOrderId = Selected.OrderId;


           

            if (_SOrderId == 0)
                return;

            FrmMDI.LoadForm("FrmMN0301","Order", _SOrderId);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {


            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;


            //var fileString = "배차일보" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //FileInfo file = new FileInfo(FileName);
            //if (file.Exists)
            //{
            //    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        return;
            //}
            //using (MemoryStream _Stream = new MemoryStream(Properties.Resources.Order_Default))
            //{
            //    _Stream.Seek(0, SeekOrigin.Begin);
            //    _Excel = new ExcelPackage(_Stream);
            //}
            //var _Sheet = _Excel.Workbook.Worksheets[1];
            //var RowIndex = 2;
            //var ColumnIndexMap = new Dictionary<int, int>();
            //var ColumnIndex = 0;
            //for (int i = 0; i < grid1.ColumnCount; i++)
            //{
            //    if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { ColumnImageA, CustomerSms, btnSms }.Contains(grid1.Columns[i])))
            //    {
            //        ColumnIndexMap.Add(ColumnIndex, i);
            //        ColumnIndex++;
            //    }
            //}
            //for (int i = 0; i < grid1.RowCount; i++)
            //{
            //    for (int j = 0; j < ColumnIndexMap.Count; j++)
            //    {

            //        _Sheet.Cells[RowIndex, j + 1].Value = grid1[ColumnIndexMap[j], i].FormattedValue?.ToString();
            //    }
            //    RowIndex++;
            //}
            //try
            //{
            //    _Excel.SaveAs(file);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
            //    return;
            //}

            //System.Diagnostics.Process.Start(FileName);

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
           



            //DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            //if (di.Exists == false)
            //{

            //    di.Create();
            //}
            //var fileString = "배차일보" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //FileInfo file = new FileInfo(FileName);
            //if (file.Exists)
            //{
            //    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //    {
            //        return;


            //    }


            //}

            //ExcelPackage _Excel = null;
            //using (MemoryStream _Stream = new MemoryStream(Properties.Resources.배차일보_Blank))
            //{
            //    _Stream.Seek(0, SeekOrigin.Begin);
            //    _Excel = new ExcelPackage(_Stream);
            //}
            //var _Sheet = _Excel.Workbook.Worksheets[1];
            //var RowIndex = 2;
            //for (int i = 0; i < grid1.RowCount; i++)
            //{
            //    _Sheet.Cells[RowIndex, 1].Value = grid1[rowNUMDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 2].Value = grid1[acceptTimeDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 3].Value = grid1[customerNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 4].Value = grid1[driverCarNoDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 5].Value = grid1[driverDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 6].Value = grid1[driverPhoneNoDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 7].Value = grid1[startNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 8].Value = grid1[stopNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

            //    _Sheet.Cells[RowIndex, 9].Value = grid1[itemDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 10].Value = grid1[itemSizeDataGridViewTextBoxColumn.Index, i].FormattedValue;

            //    _Sheet.Cells[RowIndex, 11].Value = grid1[carCountDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 12].Value = grid1[carSizeNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

            //    _Sheet.Cells[RowIndex, 13].Value = grid1[carTypeNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 14].Value = grid1[payLocationNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 15].Value = grid1[isSharedDataGridViewTextBoxColumn.Index, i].FormattedValue;

            //    _Sheet.Cells[RowIndex, 16].Value = grid1[tradesRequestDateDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 17].Value = grid1[referralNameDataGridViewTextBoxColumn.Index, i].FormattedValue;


            //    _Sheet.Cells[RowIndex, 18].Value = grid1[startPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 19].Value = grid1[stopPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 20].Value = grid1[driverPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 21].Value = grid1[tradePriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 22].Value = grid1[salesPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;

            //    _Sheet.Cells[RowIndex, 23].Value = grid1[sTPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 24].Value = grid1[sonIkDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 25].Value = grid1[alterPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;


            //    _Sheet.Cells[RowIndex, 26].Value = grid1[sunAMountDataGridViewTextBoxColumn.Index, i].FormattedValue;

            //    _Sheet.Cells[RowIndex, 27].Value = grid1[requestMemoDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 28].Value = grid1[cEODataGridViewTextBoxColumn.Index, i].FormattedValue;

            //    RowIndex++;
            //}

            //try
            //{
            //    _Excel.SaveAs(file);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
            //    return;
            //}
            //System.Diagnostics.Process.Start(FileName);
        }
        public void Button_MouseMove(object sender, MouseEventArgs e)
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


        public void Button_MouseLeave(object sender, EventArgs e)
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

        private void dtp_Sdate_ValueChanged(object sender, EventArgs e)
        {
            cmbCustomerSearchBinding();
            cmbSearchBinding();
        }

        private void dtp_Edate_ValueChanged(object sender, EventArgs e)
        {
            cmbCustomerSearchBinding();
            cmbSearchBinding();
        }

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }
        class CustomerAndTeamViewModel
        {
            public int CustomerId { get; set; }
            public int TeamId { get; set; }
            public string Part { get; set; }

        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,
              rowNUMDataGridViewTextBoxColumn
          , acceptTimeDataGridViewTextBoxColumn
          , customerNameDataGridViewTextBoxColumn
          , ColumnCustomerTeam
          , driverCarNoDataGridViewTextBoxColumn
          , driverDataGridViewTextBoxColumn
          , driverPhoneNoDataGridViewTextBoxColumn
          , startNameDataGridViewTextBoxColumn
          //, startMemoDataGridViewTextBoxColumn
          , stopNameDataGridViewTextBoxColumn
          //, stopMemoDataGridViewTextBoxColumn
          , itemDataGridViewTextBoxColumn
          , itemSizeDataGridViewTextBoxColumn
          , carCountDataGridViewTextBoxColumn
         // , carSizeDataGridViewTextBoxColumn
          , carSizeNameDataGridViewTextBoxColumn
         // , carTypeDataGridViewTextBoxColumn
          , carTypeNameDataGridViewTextBoxColumn
         // , payLocationDataGridViewTextBoxColumn
          , payLocationNameDataGridViewTextBoxColumn
          , isSharedDataGridViewTextBoxColumn
          , tradesRequestDateDataGridViewTextBoxColumn
          , referralNameDataGridViewTextBoxColumn
          , startPriceDataGridViewTextBoxColumn
          , stopPriceDataGridViewTextBoxColumn
          , driverPriceDataGridViewTextBoxColumn
          , tradePriceDataGridViewTextBoxColumn
          , salesPriceDataGridViewTextBoxColumn
          , sTPriceDataGridViewTextBoxColumn
          , sonIkDataGridViewTextBoxColumn
          , alterPriceDataGridViewTextBoxColumn
          , sunAMountDataGridViewTextBoxColumn
          , requestMemoDataGridViewTextBoxColumn
          , cEODataGridViewTextBoxColumn
          , PayDateDataGridViewTextBoxColumn
          ,ColumnsCustomerManager
           // , OrderId




           );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);


            _FormStyle.WriteFormStyle(this, grid1);

            MessageBox.Show("저장하였습니다.");
        }
    }
}
