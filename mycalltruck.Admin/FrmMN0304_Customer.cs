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
using mycalltruck.Admin.CMDataSetTableAdapters;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using OfficeOpenXml.Style;

namespace mycalltruck.Admin
{
    public partial class FrmMN0304_Customer : Form
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


        public FrmMN0304_Customer()
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
            // TODO: 이 코드는 데이터를 'nSTATSDataSet.NSTATS1CUSTOMER' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.nSTATS1CUSTOMERTableAdapter.Fill(this.nSTATSDataSet.NSTATS1CUSTOMER);
            //NSTAT1();

            dtp_Sdate.Value = DateTime.Now.AddDays(-1);
            dtp_Edate.Value = DateTime.Now;
            InitCmb();
            grid1.Controls.Add(dtp);
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.Visible = false;

            //if(LocalUser.Instance.LogInInformation.CustomerUserId != 0)
            //{
            //    cmbTeam.Enabled = false;
            //}
            //else
            //{
            //    cmbTeam.Enabled = true;
            //}

            btn_Inew_Click(null, null);



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
                { "결제구분", "결제구분" },
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
                //{ "거래처명", "거래처명" },
                { "차량번호", "차량번호" },
                { "차주명", "차주명" },
                { "핸드폰번호", "핸드폰번호" }

            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


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


        }
        private String GetSelectCommand()
        {
//            return @"SELECT  Orders.CreateTime, ISNULL(Orders.Customer, '') AS CustomerName, Orders.DriverCarNo, Orders.Driver, Orders.DriverPhoneNo, ISNULL(Orders.StartName, '') AS StartName, 
//               ISNULL(Orders.StartMemo, '') AS StartMemo, ISNULL(Orders.StopName, '') AS StopName, ISNULL(Orders.StopMemo, '') AS StopMemo, Orders.Item, Orders.ItemSize, Orders.CarCount, 
//               Orders.CarSize, CarSize.Name AS CarSizeName, Orders.CarType, CarType.Name AS CarTypeName, Orders.PayLocation, PayLocation.Name AS PayLocationName, Orders.IsShared, 
//               CONVERT(varchar(10), Trades.RequestDate, 111) AS TradesRequestDate, ISNULL(Customers.SangHo, '') AS ReferralName, ISNULL(Orders.StartPrice, 0) AS StartPrice, 
//               ISNULL(Orders.StopPrice, 0) AS StopPrice, ISNULL(Orders.DriverPrice, 0) AS DriverPrice, ISNULL(Orders.TradePrice, 0) AS TradePrice, ISNULL(Orders.SalesPrice, 0) AS SalesPrice, 
//               ISNULL(Orders.SalesPrice, 0) - ISNULL(Orders.TradePrice, 0) AS STPrice, CASE ISNULL(Orders.DriverPrice, 0) WHEN 0 THEN ISNULL(Orders.TradePrice, 0) - ISNULL(Orders.SalesPrice, 0) 
//               ELSE ISNULL(Orders.DriverPrice, 0) END AS SonIk, ISNULL(Orders.AlterPrice, 0) AS AlterPrice, 0 AS SunAMount, ISNULL(Orders.RequestMemo, '') AS RequestMemo, Clients.CEO, 
//               Orders.ClientId, Orders.OrderId, Orders.DriverId, Orders.UnitItem, Orders.UnitType, CASE WHEN Trades.PayState = 1 THEN CONVERT(varchar, Trades.PayDate, 111) ELSE '' END AS PayDate, 
//               ISNULL(Orders.SalesManageId, 0) AS SalesManageId,StartTime,StopTime,ISNULL(Trades.PdfFileName,'') as PdfFileName,Trades.TradeId
//FROM     Orders INNER JOIN
//               StaticOptions AS CarSize ON Orders.CarSize = CarSize.Value AND CarSize.Div = 'CarSize' INNER JOIN
//               StaticOptions AS CarType ON Orders.CarType = CarType.Value AND CarType.Div = 'CarType' INNER JOIN
//               StaticOptions AS PayLocation ON Orders.PayLocation = PayLocation.Value AND PayLocation.Div = 'PayLocation' LEFT JOIN
//               Trades ON Orders.TradeId = Trades.TradeId LEFT OUTER JOIN
//               Customers ON Orders.ReferralId = Customers.CustomerId AND Customers.BizGubun = 4 INNER JOIN
//               Clients ON Orders.ClientId = Clients.ClientId INNER JOIN
//               Drivers ON Orders.DriverId = Drivers.DriverId LEFT OUTER JOIN
//               SalesManage ON Orders.SalesManageId = SalesManage.SalesId";


            return @"SELECT  Orders.CreateTime, ISNULL(Orders.Customer, '') AS CustomerName, Orders.DriverCarNo, Orders.Driver, Orders.DriverPhoneNo, ISNULL(Orders.StartName, '') AS StartName, 
               ISNULL(Orders.StartMemo, '') AS StartMemo, ISNULL(Orders.StopName, '') AS StopName, ISNULL(Orders.StopMemo, '') AS StopMemo, Orders.Item, Orders.ItemSize, Orders.CarCount, 
               Orders.CarSize, CarSize.Name AS CarSizeName, Orders.CarType, CarType.Name AS CarTypeName, Orders.PayLocation, PayLocation.Name AS PayLocationName, Orders.IsShared, 
               CONVERT(varchar(10), Trades.RequestDate, 111) AS TradesRequestDate, ISNULL(Customers.SangHo, '') AS ReferralName, ISNULL(Orders.StartPrice, 0) AS StartPrice, 
               ISNULL(Orders.StopPrice, 0) AS StopPrice, ISNULL(Orders.DriverPrice, 0) AS DriverPrice, ISNULL(Orders.TradePrice, 0) AS TradePrice, ISNULL(Orders.SalesPrice, 0) AS SalesPrice, 
               ISNULL(Orders.SalesPrice, 0) - ISNULL(Orders.TradePrice, 0) AS STPrice, CASE ISNULL(Orders.DriverPrice, 0) WHEN 0 THEN ISNULL(Orders.TradePrice, 0) - ISNULL(Orders.SalesPrice, 0) 
               ELSE ISNULL(Orders.DriverPrice, 0) END AS SonIk, ISNULL(Orders.AlterPrice, 0) AS AlterPrice, 0 AS SunAMount, ISNULL(Orders.RequestMemo, '') AS RequestMemo, Clients.CEO, 
               Orders.ClientId, Orders.OrderId, Orders.DriverId, Orders.UnitItem, Orders.UnitType, CASE WHEN Trades.PayState = 1 THEN CONVERT(varchar, Trades.PayDate, 111) ELSE '' END AS PayDate, 
               ISNULL(Orders.SalesManageId, 0) AS SalesManageId, Orders.StartTime, Orders.StopTime, ISNULL(Trades.PdfFileName, '') AS PdfFileName, Trades.TradeId, Orders.CustomerTeam, 
               Orders.OrderLine, Orders.OrderProduct, Orders.OrderTurn, Orders.OrderWidth, Orders.OrderLength, Orders.OrderHeight
FROM     Orders INNER JOIN
               StaticOptions AS CarSize ON Orders.CarSize = CarSize.Value AND CarSize.Div = 'CarSize' INNER JOIN
               StaticOptions AS CarType ON Orders.CarType = CarType.Value AND CarType.Div = 'CarType' INNER JOIN
               StaticOptions AS PayLocation ON Orders.PayLocation = PayLocation.Value AND PayLocation.Div = 'PayLocation' LEFT JOIN
               Trades ON Orders.TradeId = Trades.TradeId LEFT OUTER JOIN
               Customers ON Orders.ReferralId = Customers.CustomerId AND Customers.BizGubun = 4 INNER JOIN
               Clients ON Orders.ClientId = Clients.ClientId INNER JOIN
               Drivers ON Orders.DriverId = Drivers.DriverId LEFT OUTER JOIN
               SalesManage ON Orders.SalesManageId = SalesManage.SalesId";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS1CUSTOMER.Clear();

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();




                    if (cmbSPayLocation.SelectedIndex > 0)
                    {
                        WhereStringList.Add("PayLocation.Name = @PayLocation ");
                        _Command.Parameters.AddWithValue("@PayLocation", cmbSPayLocation.Text);
                    }


                    WhereStringList.Add("Orders.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    WhereStringList.Add(string.Format("Orders.CustomerId  = '{0}'", LocalUser.Instance.LogInInformation.CustomerId));




                    WhereStringList.Add("CONVERT(VARCHAR(10),orders.CreateTime,111) >= @Sdate AND CONVERT(VARCHAR(10),orders.CreateTime,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate", dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    WhereStringList.Add("Orders.OrderStatus = 3 ");

                    if(LocalUser.Instance.LogInInformation.CustomerUserId != 0)
                    {
                        WhereStringList.Add(string.Format("Orders.CustomerTeam  = '{0}'", LocalUser.Instance.LogInInformation.CustomerTeamId));
                    }
                    else
                    {
                        if(cmbTeam.SelectedIndex > 0)
                        {
                            WhereStringList.Add(string.Format("Orders.CustomerTeam  = '{0}'", (int)cmbTeam.SelectedValue));
                        }
                        
                    }


                     if (cmbSSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("DriverCarNo Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "차주명")
                    {
                        WhereStringList.Add(string.Format("Driver Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "핸드폰번호")
                    {
                        WhereStringList.Add(string.Format("DriverPhoneNo Like  '%{0}%'", txtSText.Text));
                    }


                    //if(!LocalUser.Instance.LogInInformation.IsClient)
                    //{
                    //    WhereStringList.Add(string.Format("Orders.CustomerId  = '{0}'", LocalUser.Instance.LogInInformation.CustomerId));
                    //}
                    



                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by orders.CreateTime  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS1CUSTOMER.Load(_Reader);


                        if (grid1.RowCount > 0)
                        {
                            // GridIndex = ModelDataGrid.Position;
                            //  _Search();
                            grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];

                            nSTATS1CUSTOMERBindingSource_CurrentChanged(null, null);
                            
                        }


                    }
                }


                _Connection.Close();


            }

            var Query = nSTATSDataSet.NSTATS1CUSTOMER.ToArray();

            if (Query.Any())
            {
                lblSalesPrice.Text = Query.Sum(c => c.SalesPrice).ToString("N0");
              //  lblTradePrice.Text = Query.Sum(c => c.TradePrice).ToString("N0");
              //  lblSTPrice.Text = (Query.Sum(c => c.SalesPrice) - Query.Sum(c => c.TradePrice)).ToString("N0");
                lblAlterPrice.Text = Query.Sum(c => c.AlterPrice).ToString("N0");
                lblStartPrice.Text = (Query.Sum(c => c.StartPrice)+ Query.Sum(c => c.StopPrice)).ToString("N0");
                lblTotal.Text = (Query.Sum(c => c.SalesPrice) + Query.Sum(c => c.AlterPrice)).ToString("N0");
                //lblDriverPrice.Text = Query.Sum(c => c.DriverPrice).ToString("N0");
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

            //jogun = "위탁사 : " + cmbSReferralId.Text + "  운임조건 : " + cmbSPayLocation.Text;

            f.NSTATS1(dtp_Sdate.Text, dtp_Edate.Text, jogun, nSTATS1CUSTOMERBindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();



        }

        private void grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1CUSTOMERRow;


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

            //else if (e.ColumnIndex == itemSizeDataGridViewTextBoxColumn.Index)
            //{
            //    DriverRepository mDriverRepository = new DriverRepository();
            //    if (Selected.DriverId != 0)
            //    {

            //        var Query = mDriverRepository.NoGetDriver((int)Selected.DriverId);


            //        if (Query.CarSize != 0 )
            //        {
            //            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == Query.CarSize))
            //                e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == Query.CarSize).Name;
            //        }


            //    }
            //}
            else if(e.ColumnIndex == TotalPriceColumns.Index)
            {
                e.Value = (Selected.AlterPrice + Selected.SalesPrice).ToString("N0");
            }
            else if (e.ColumnIndex == carSizeNameDataGridViewTextBoxColumn1.Index)
            {
               // e.Value = Selected.UnitItem;
            }

            else if (e.ColumnIndex == carTypeNameDataGridViewTextBoxColumn1.Index)
            {
                //if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "Unit" && c.Value == Selected.UnitType))
                //    e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "Unit" && c.Value == Selected.UnitType).Name;
               
            }

            else if(e.ColumnIndex == startTimeDataGridViewTextBoxColumn.Index)
            {
                e.Value = Selected.StartTime.ToString("yyyy-MM-dd HH:mm");

            }
            else if(e.ColumnIndex == stopTimeDataGridViewTextBoxColumn.Index)
            {

                e.Value = Selected.StopTime.ToString("yyyy-MM-dd HH:mm");
            }
            else if (e.ColumnIndex == ColumnCustomerTeam.Index)
            {
                if (Selected.CustomerTeam == 0)
                {
                    e.Value = "본사";
                }
                else
                {

                    customerTeamsTableAdapter.Fill(customerUserDataSet.CustomerTeams, LocalUser.Instance.LogInInformation.ClientId);


                    var _Q = customerUserDataSet.CustomerTeams.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == LocalUser.Instance.LogInInformation.CustomerId && c.CustomerTeamId == Selected.CustomerTeam).ToArray();

                    if (_Q.Any())
                    {
                        e.Value = _Q.First().TeamName;
                    }
                    else
                    {

                    }



                }

                

                //e.Value = Selected.StopTime.ToString("yyyy-MM-dd HH:mm");
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
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1CUSTOMERRow;


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

       

        private void grid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1CUSTOMERRow;
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
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            if (di.Exists == false)
            {

                di.Create();
            }
            ExcelPackage _Excel = null;

            var fileString = "배차일보" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;


                }


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
            //cmbCustomerSearchBinding();
            //cmbSearchBinding();
        }

        private void dtp_Edate_ValueChanged(object sender, EventArgs e)
        {
            //cmbCustomerSearchBinding();
            //cmbSearchBinding();
        }

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }


        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,
              rowNUMDataGridViewTextBoxColumn
              ,acceptTimeDataGridViewTextBoxColumn
              ,startNameDataGridViewTextBoxColumn
              ,startTimeDataGridViewTextBoxColumn
              ,stopNameDataGridViewTextBoxColumn
              ,stopTimeDataGridViewTextBoxColumn
              ,ColumnCustomerTeam
              ,carTypeNameDataGridViewTextBoxColumn1
              ,carSizeNameDataGridViewTextBoxColumn1
              ,driverDataGridViewTextBoxColumn
              ,driverCarNoDataGridViewTextBoxColumn
              ,driverPhoneNoDataGridViewTextBoxColumn
              ,payLocationNameDataGridViewTextBoxColumn
              ,salesPriceDataGridViewTextBoxColumn
              ,alterPriceDataGridViewTextBoxColumn
              ,TotalPriceColumns
              ,pdfFileNameDataGridViewTextBoxColumn
              ,requestMemoDataGridViewTextBoxColumn

        


           );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);

        }

        private void nSTATS1CUSTOMERBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (nSTATS1CUSTOMERBindingSource.Current == null)
            {
                return;
            }
            else
            {
                var Selected = ((DataRowView)nSTATS1CUSTOMERBindingSource.Current).Row as NSTATSDataSet.NSTATS1CUSTOMERRow;

                GridIndex = nSTATS1CUSTOMERBindingSource.Position;
                _SOrderId = Selected.OrderId;
            }
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (grid1.Columns[e.ColumnIndex] == pdfFileNameDataGridViewTextBoxColumn)
            {
                var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1CUSTOMERRow;
                if (Selected != null)
                {
                    if (!String.IsNullOrEmpty(Selected.PdfFileName))
                    {
                        tradesTableAdapter1.Fill(this.tradeDataSet1.Trades);
                        
                        //bindingSource1.DataSource = tradeDataSet1.Trades.Where(c => c.TradeId == Selected.TradeId);

                       // int _Tradeid = tradeDataSet1.Trades.Where(c => c.TradeId == Selected.TradeId).FirstOrDefault().TradeId;



                        FrmEdocument_C f = new FrmEdocument_C(Selected.TradeId, Selected.PdfFileName);
                        f.ShowDialog();

                    }
                    //else if (!String.IsNullOrEmpty(Selected.Image1) && Selected.HasETax == false)
                    //{


                    //    FormImages f = new FormImages(Selected);
                    //    f.Owner = this;
                    //    f.StartPosition = FormStartPosition.CenterParent;
                    //    f.ShowDialog();

                    //    btnSearch_Click(null, null);

                    //}


                }
            }
        }

        private void grid1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)


                return;
            if (grid1.Columns[e.ColumnIndex] == pdfFileNameDataGridViewTextBoxColumn)
            {
                var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS1CUSTOMERRow;

                if (Selected != null)
                {
                    try
                    {
                        if (String.IsNullOrEmpty(Selected.PdfFileName))
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

        private void button3_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);


            _FormStyle.WriteFormStyle(this, grid1);

            MessageBox.Show("저장하였습니다.");
        }
    }
}
