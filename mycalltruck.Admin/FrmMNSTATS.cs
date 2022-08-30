
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using mycalltruck.Admin.DataSets;
using System.Runtime.CompilerServices;
using mycalltruck.Admin.Class.DataSet;
using Popbill.Taxinvoice;
using Popbill;
using mycalltruck.Admin.CMDataSetTableAdapters;
using mycalltruck.Admin.Class;

namespace mycalltruck.Admin
{
    public partial class FrmMNSTATS : Form
    { DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
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

                   
                    //grid2.ReadOnly = true;
                    break;
            }


        }

        UCNSTATS2 ucNSTATS2 = new UCNSTATS2();
        UCNSTATS3 ucNSTATS3 = new UCNSTATS3();
        UCNSTATS4 ucNSTATS4 = new UCNSTATS4();
        UCNSTATS5 ucNSTATS5 = new UCNSTATS5();
        UCNSTATS6 ucNSTATS6 = new UCNSTATS6();
        UCNSTATS7 ucNSTATS7 = new UCNSTATS7();
        UCNSTATS8 ucNSTATS8 = new UCNSTATS8();
        DESCrypt m_crypt = null;
        bool AllowTaxBool = false;
        Int32 _ServiceState = 0;
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;


        public FrmMNSTATS()
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }
        private void FrmMNSTATS_Load(object sender, EventArgs e)
        {
            NSTAT2();

            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;
        }


     
        int _SOrderId = 0;
       

        void Mn0301()
        {

        }

        private void NSTAT2()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS2);
            ucNSTATS2.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS2.Location = new System.Drawing.Point(0, 0);
            ucNSTATS2.Name = "ucNSTATS2";
            ucNSTATS2.Size = new System.Drawing.Size(804, 579);
            ucNSTATS2.TabIndex = 0;
            ucNSTATS2.btn_Inew_Click(null, null);
        }

        private void NSTAT3()
        {
            
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS3);
            ucNSTATS3.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS3.Location = new System.Drawing.Point(0, 0);
            ucNSTATS3.Name = "ucNSTATS3";
            ucNSTATS3.Size = new System.Drawing.Size(804, 579);
            ucNSTATS3.TabIndex = 0;
            ucNSTATS3.btn_Inew_Click(null, null);


            ucNSTATS3.ModelDataGrid.CellClick += new DataGridViewCellEventHandler(model_Click);
           //ucNSTATS3.수수료확인취소ToolStripMenuItem_Click
          
        }
        List<int> OrderIds = new List<int>();
        List<int> OrderIdsNo = new List<int>();
        List<string> OrderAcceptTime = new List<string>();

        int DriverCountNo = 0;
        
        void model_Click(object sender, DataGridViewCellEventArgs e)
        {
            OrderIds.Clear();
            OrderIdsNo.Clear();
          

            if (e.RowIndex < 0)
                return;
            int DriverPrice = 0;
            int DriverCount = 0;
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();
            if (ucNSTATS3.ModelDataGrid.SelectedCells.Count > 0)
            {

                foreach (DataGridViewCell item in ucNSTATS3.ModelDataGrid.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }


                
               

                
                foreach (DataGridViewRow dRow in deleteRows)
                {

                    if (dRow.Cells["ReferralAccountYN"].Value.ToString() == "N")
                    {
                        DriverPrice += int.Parse(dRow.Cells["driverPriceDataGridViewTextBoxColumn"].Value.ToString());
                        DriverCount++;
                        OrderIds.Add(int.Parse(dRow.Cells["OrderId"].Value.ToString()));

                      
                    }
                    else
                    {
                        DriverCountNo++;
                        OrderIdsNo.Add(int.Parse(dRow.Cells["OrderId"].Value.ToString()));
                    }
                   
                }

                lblCount.Text = DriverCount.ToString("N0");
                lblSSum.Text = DriverPrice.ToString("N0");

            }

            if (ucNSTATS3.cmbSReferralId.SelectedIndex == 0)
            {
                lblSangHo.Text = "";
                lblCount.Text = "0";
                lblSSum.Text = "0";
                dtpPayDate.Value = DateTime.Now;

                if (DriverCount > 1)
                {
                    MessageBox.Show("위의 조회 조건에서 먼저 위탁사를 선택해야 합니다.", "위탁사 수수료정산", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                lblSangHo.Text = ucNSTATS3.cmbSReferralId.Text;
                lblSangHo.Tag = ucNSTATS3.cmbSReferralId.SelectedValue;
            }


            



        }

    
        private void NSTAT4()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS4);
            ucNSTATS4.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS4.Location = new System.Drawing.Point(0, 0);
            ucNSTATS4.Name = "ucNSTATS4";
            ucNSTATS4.Size = new System.Drawing.Size(804, 579);
            ucNSTATS4.TabIndex = 0;
            ucNSTATS4.btn_Inew_Click(null, null);

            ucNSTATS4.ModelDataGrid.CellClick += new DataGridViewCellEventHandler(Taxmodel_Click);
        }
        //List<int> OrderIds = new List<int>();
        //List<int> OrderIdsNo = new List<int>();

        int TaxCountNo = 0;

        void Taxmodel_Click(object sender, DataGridViewCellEventArgs e)
        {
            OrderIds.Clear();
            OrderIdsNo.Clear();
            OrderAcceptTime.Clear();

            if (e.RowIndex < 0)
                return;
            int DriverPrice = 0;
            int TaxCount = 0;
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();
            if (ucNSTATS4.ModelDataGrid.SelectedCells.Count > 0)
            {

                foreach (DataGridViewCell item in ucNSTATS4.ModelDataGrid.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }






                foreach (DataGridViewRow dRow in deleteRows)
                {

                    if (dRow.Cells["TaxTradeId"].Value.ToString() == "0")
                    {
                        var a =dRow.Cells["tSPriceDataGridViewTextBoxColumn"].Value.ToString().Replace(".00", "");
                        DriverPrice += int.Parse(a);
                        TaxCount++;
                        OrderIds.Add(int.Parse(dRow.Cells["OrderId"].Value.ToString()));
                        var _OrderAcceptTime = DateTime.Parse(dRow.Cells["acceptTimeDataGridViewTextBoxColumn"].Value.ToString());

                        if (!OrderAcceptTime.Contains(_OrderAcceptTime.ToString("yyyyMM")))
                        {
                            OrderAcceptTime.Add(_OrderAcceptTime.ToString("yyyyMM"));
                        }
                    }
                    else
                    {
                        TaxCountNo++;
                        OrderIdsNo.Add(int.Parse(dRow.Cells["OrderId"].Value.ToString()));
                    }

                }

               
                

            }

            //var Sdate = ucNSTATS4.dtp_Sdate.Value.Year + "/" + ucNSTATS4.dtp_Sdate.Value.Month;
            //var Edate = ucNSTATS4.dtp_Edate.Value.Year + "/" + ucNSTATS4.dtp_Edate.Value.Month;

           


            if (OrderAcceptTime.Count > 1)
            {

                dtpRequestDate.Value = DateTime.Now;

                if (TaxCount > 1)
                {
                    MessageBox.Show("같은 년월 만 선택하여 발행할 수 있습니다.", "차주 세금계산서 발행", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    
                    lblYear.Text = ucNSTATS4.dtp_Sdate.Value.Date.ToString("yyyyMM");
                }

            }
            else
            {
                if(OrderIds.Count == 0)
                {
                    return;
                }
                DataLoad();

                lblYear.Text = ucNSTATS4.dtp_Sdate.Value.Date.ToString("yyyyMM");
                lblTaxCount.Text = _ModelList.Count.ToString() +" 건";
                dtpRequestDate.Value = DateTime.Now;
            }






        }

        private void NSTAT5()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS5);
            ucNSTATS5.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS5.Location = new System.Drawing.Point(0, 0);
            ucNSTATS5.Name = "ucNSTATS5";
            ucNSTATS5.Size = new System.Drawing.Size(804, 579);
            ucNSTATS5.TabIndex = 0;
            ucNSTATS5.btn_Inew_Click(null, null);
        }

        private void NSTAT6()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS6);
            ucNSTATS6.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS6.Location = new System.Drawing.Point(0, 0);
            ucNSTATS6.Name = "ucNSTATS6";
            ucNSTATS6.Size = new System.Drawing.Size(804, 579);
            ucNSTATS6.TabIndex = 0;
            ucNSTATS6.btn_Inew_Click(null, null);
        }
        private void NSTAT7()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS7);
            ucNSTATS7.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS7.Location = new System.Drawing.Point(0, 0);
            ucNSTATS7.Name = "ucNSTATS7";
            ucNSTATS7.Size = new System.Drawing.Size(804, 579);
            ucNSTATS7.TabIndex = 0;
            ucNSTATS7.btn_Inew_Click(null, null);
        }
        private void NSTAT8()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS8);
            ucNSTATS8.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS8.Location = new System.Drawing.Point(0, 0);
            ucNSTATS8.Name = "ucNSTATS8";
            ucNSTATS8.Size = new System.Drawing.Size(804, 579);
            ucNSTATS8.TabIndex = 0;
            ucNSTATS8.btn_Inew_Click(null, null);
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

            public string CEO { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }

            public string Upjong { get; set; }
            public string Uptae { get; set; }
            public string Email { get; set; }
            public string PhoneNo { get; set; }





        }


        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _DriverTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,ISNULL(MobileNo,N'')as MobileNo,CarNo,ISNULL(CarYear,'N')CarYear,CarSize,CarType,Password,isnull(CEO,N'') as CEO,isnull(AddressState,N'')as AddressState,ISNULL(AddressCity,N'')as AddressCity,ISNULL(AddressDetail,N'')as AddressDetail,ISNULL(Upjong,N'')as Upjong,ISNULL(Uptae,N'')as Uptae,ISNULL(Email,N'')as Email,ISNULL(PhoneNo,N'')as PhoneNo FROM Drivers ";

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
                                  CEO = dataReader.GetString(13),

                                  AddressState = dataReader.GetString(14),
                                  AddressCity = dataReader.GetString(15),
                                  AddressDetail = dataReader.GetString(16),

                                  Upjong = dataReader.GetString(17),
                                  Uptae = dataReader.GetString(18),
                                  Email = dataReader.GetString(19),
                                  PhoneNo = dataReader.GetString(20),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (treeView1.SelectedNode.Name == "NSTAT1")
            //{
            //    NSTAT1();
            //}

            //else 
            if (treeView1.SelectedNode.Name == "NSTAT2")
            {
                
                panel5.Visible = false;
                label4.Visible = false;
                panel7.Visible = false;
                NSTAT2();
            }

            else if (treeView1.SelectedNode.Name == "NSTAT3")
            {
                label4.Text = "선택 건 수수료정산";
                panel5.Visible = true;
                label4.Visible = true;
                panel7.Visible = false;
                
                NSTAT3();
            }
            else if (treeView1.SelectedNode.Name == "NSTAT4")
            {
                label4.Text = "선택 건 계산서발행";
                panel5.Visible = false;
                label4.Visible = true;
                panel7.Visible = true;

                NSTAT4();
            }
            else if (treeView1.SelectedNode.Name == "NSTAT5")
            {
                panel5.Visible = false;
                label4.Visible = false;
                panel7.Visible = false;
                NSTAT5();
            }
            else if (treeView1.SelectedNode.Name == "NSTAT6")
            {
                panel5.Visible = false;
                label4.Visible = false;
                panel7.Visible = false;
                NSTAT6();
            }
            else if (treeView1.SelectedNode.Name == "NSTAT7")
            {
                panel5.Visible = false;
                label4.Visible = false;
                panel7.Visible = false;
                NSTAT7();
            }

            else if (treeView1.SelectedNode.Name == "NSTAT8")
            {
                panel5.Visible = false;
                label4.Visible = false;
                panel7.Visible = false;
                NSTAT8();
            }
            #region

            #endregion

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                int _ReferralAccountId = 0;
                _Connection.Open();

                if (!String.IsNullOrEmpty(lblSangHo.Text) && lblCount.Text.Replace(",", "") != "0" || lblSSum.Text.Replace(",", "") != "0")
                {
                    
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "INSERT ReferralAccount ( PayDate, CustomerId,Amount, OutAmount, ClientId,OrderSdate,OrderEdate,DriverId,ReferralCount)Values( @PayDate, @CustomerId,@Amount, @OutAmount, @ClientId,@OrderSdate,@OrderEdate,@DriverId,@ReferralCount)" +
                            " SELECT @@Identity";
                        _Command.Parameters.Add("@PayDate", SqlDbType.DateTime);
                        _Command.Parameters.Add("@CustomerId", SqlDbType.Int);
                        _Command.Parameters.Add("@Amount", SqlDbType.Decimal);
                        _Command.Parameters.Add("@OutAmount", SqlDbType.Decimal);
                        _Command.Parameters.Add("@ClientId", SqlDbType.Int);

                        _Command.Parameters.Add("@OrderSdate", SqlDbType.DateTime);
                        _Command.Parameters.Add("@OrderEdate", SqlDbType.DateTime);
                        _Command.Parameters.Add("@DriverId", SqlDbType.Int);
                        _Command.Parameters.Add("@ReferralCount", SqlDbType.Int);

                        _Command.Parameters["@PayDate"].Value = dtpPayDate.Value;
                        _Command.Parameters["@OrderSdate"].Value = ucNSTATS3.dtp_Sdate.Value;
                        _Command.Parameters["@OrderEdate"].Value = ucNSTATS3.dtp_Edate.Value;


                        _Command.Parameters["@CustomerId"].Value = ucNSTATS3.cmbSReferralId.SelectedValue;
                        _Command.Parameters["@DriverId"].Value = 0;

                        _Command.Parameters["@ReferralCount"].Value = lblCount.Text.Replace(",", "");
                        _Command.Parameters["@Amount"].Value = lblSSum.Text;


                        Int64 _OutAmount = 0;


                        _Command.Parameters["@OutAmount"].Value = _OutAmount;


                        _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;


                        _ReferralAccountId = Convert.ToInt32(_Command.ExecuteScalar());


                        try
                        {
                            MessageBox.Show("저장되었습니다.", "수수료 정산", MessageBoxButtons.OK);


                            lblSangHo.Text = "";
                            lblCount.Text = "0";
                            lblSSum.Text = "0";
                            dtpPayDate.Value = DateTime.Now;

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

                    MessageBox.Show("저장할 건을 선택하세요", "위탁사 수수료정산", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }

                _Connection.Close();

                ucNSTATS3.btnSearch_Click(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblCount.Text = "0";
            lblSangHo.Text = "";
            lblSSum.Text = "0";
            dtpPayDate.Value = DateTime.Now;

            ucNSTATS3.btnSearch_Click(null, null);

        }

        public class Model : INotifyPropertyChanged
        {
           
            public int DriverId { get; set; }
            public int Count { get; set; }
            public decimal TSPrice { get { return _TSPrice; } set { SetField(ref _TSPrice, value); } }
            //public decimal ClientPrice { get { return _ClientPrice; } set { SetField(ref _ClientPrice, value); } }
            public decimal VAT { get { return _VAT; } set { SetField(ref _VAT, value); } }
           // public decimal Fee { get { return _Fee; } set { SetField(ref _Fee, value); } }
            //public decimal Fee_VAT { get { return _Fee_VAT; } set { SetField(ref _Fee_VAT, value); } }
            public decimal Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            public string CarNo { get; set; }
            public string CarYear { get; set; }


            public string DriverBizNo { get; set; }
            public string DriverEmail { get; set; }
            public string DriverSangHo { get; set; }
            public string DriverUptae { get; set; }
            public string DriverUpjong { get; set; }
            public string DriverCEO { get; set; }
            public string ClientEmail { get; set; }
            public string ClientBizNo { get; set; }
            public int ClientId { get; set; }

      
          
           
            public int OrderId { get; set; }
            public DateTime AcceptTime { get; set; }
        

            private decimal _TSPrice = 0;
          
            private decimal _VAT = 0;
         
            private decimal _Amount = 0;
           
            public List<int> OrderIdList { get; set; } = new List<int>();
            //public bool IsRefenece { get; set; }
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
        private void DataLoad()
        {
            _ModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    List<String> WhereStringList = new List<string>();
                    _Command.CommandText = " SELECT " +
                     "  Orders.OrderId " +
                     " ,Orders.AcceptTime " +
                     " ,CASE WHEN Orders.PayLocation IS NULL THEN Trades.Amount WHEN Orders.PayLocation = 5 THEN  0 else ISNULL(Orders.SalesPrice, 0) - ISNULL(Orders.TradePrice, 0) end as TSPrice " +
                     " ,Orders.DriverId " +
                     " ,Orders.Driver as DriverName " +
                     " ,Drivers.BizNo as DriverBizNo " +
                     " ,ISNULL(Drivers.Email,N'') as DriverEmail " +
                     " ,Drivers.Name as DriverSangHo " +
                     " ,Drivers.Uptae as DriverUptae " +
                     " ,Drivers.Upjong as DriverUpjong " +
                     " ,Drivers.CEO as DriverCEO " +
                     " ,Clients.Email as ClientEmail " +
                     " ,Clients.BizNo as ClientBizNo " +
                     " ,Clients.ClientId as ClientId " +
                     " FROM Trades " +
                     " JOIN Orders ON Orders.TradeId = Trades.TradeId AND(Orders.PayLocation  in (1, 4, 5)) AND Orders.OrderStatus = 3 " +
                     " JOIN Drivers ON Trades.DriverId = Drivers.DriverId " +
                     " LEFT JOIN DriverInstances ON Trades.DriverId = DriverInstances.DriverId AND DriverInstances.ClientId = Trades.ClientId " +
                     "join Clients on Trades.ClientId = Clients.ClientId ";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    _Command.CommandText += Environment.NewLine + " AND Drivers.ServiceState != 5 ";

                    _Command.CommandText += Environment.NewLine + " AND  Orderid in(" + String.Join(", ", OrderIds) + ") ";

                    _Command.CommandText += Environment.NewLine + "  Order By Orders.CreateTime Desc";

                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var _DriverId = _Reader.GetInt32Z(3);

                            var _TSPrice = _Reader.GetDecimal(2);

                            var _OrderId = _Reader.GetInt32(0);
                            Model _Model = null;

                            if (rdoSplit.Checked == true)
                            {
                                _Model = new Model()
                                {
                                    OrderId = _OrderId,
                                    DriverId = _DriverId,
                                    CarYear = _Reader.GetString(4),
                                    AcceptTime = _Reader.GetDateTime(1),
                                    DriverBizNo = _Reader.GetString(5),
                                    DriverEmail = _Reader.GetString(6),
                                    DriverSangHo = _Reader.GetString(7),
                                    DriverUptae = _Reader.GetString(8),
                                    DriverUpjong = _Reader.GetString(9),
                                    DriverCEO = _Reader.GetString(10),
                                    ClientEmail = _Reader.GetString(11),
                                    ClientBizNo = _Reader.GetString(12),
                                    ClientId = _Reader.GetInt32Z(13),

                                };
                                _ModelList.Add(_Model);
                            }
                            else
                            {
                                _Model = _ModelList.FirstOrDefault(c => c.DriverId == _DriverId);
                                if (_Model == null)
                                {
                                    _Model = new Model()
                                    {
                                        OrderId = _OrderId,
                                        DriverId = _DriverId,
                                        CarYear = _Reader.GetString(4),
                                        AcceptTime = _Reader.GetDateTime(1),
                                        DriverBizNo = _Reader.GetString(5),
                                        DriverEmail = _Reader.GetString(6),
                                        DriverSangHo = _Reader.GetString(7),
                                        DriverUptae = _Reader.GetString(8),
                                        DriverUpjong = _Reader.GetString(9),
                                        DriverCEO = _Reader.GetString(10),
                                        ClientEmail = _Reader.GetString(11),
                                        ClientBizNo = _Reader.GetString(12),
                                        ClientId = _Reader.GetInt32Z(13),
                                    };
                                    _ModelList.Add(_Model);
                                }
                            }
                            _Model.Count++;
                            _Model.TSPrice += _TSPrice;

                            _Model.OrderIdList.Add(_OrderId);
                        }
                    }
                }
                _Connection.Close();
            }
        }
        //세금계산서발행
        private void btn_AddTax_Click(object sender, EventArgs e)
        {

            if (_ModelList.Count > 0)
            {
                var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

                if (_ClientPoint < 150)
                {

                    MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                    FrmMDI.LoadForm("FrmMN0204", "Point");
                    return;
                }


                if (MessageBox.Show($"총 발행건수 : { _ModelList.Count.ToString()} 건\r\n전자세금계산서를 정말 발행하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _AddClient();
                }
                else
                {
                    return;
                }

            }

        }
        int _TradeId = 0;
        private void _AddClient()
        {
            try
            {
                if (_ModelList.Count == 0)
                    return;

                bar.Value = 0;
                bar.Maximum = _ModelList.Count();
                bar.Visible = true;
                pnProgress.Visible = true;
                int _Success = 0;
                int _Fail = 0;
                Thread t = new Thread(new ThreadStart(() =>
                {
                   
                    foreach (var _model in _ModelList)
                    {
                        bar.Invoke(new Action(() => {
                            bar.Value++;
                        }));

                        decimal _Price = 0;
                        decimal _Vat = 0;
                        decimal _Amount = 0;
                        if (decimal.TryParse(_model.TSPrice.ToString().Replace(",", ""), out _Amount))
                        {
                            _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                            _Price = _Amount - _Vat;
                          
                        }


                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand _DriverCommand = cn.CreateCommand();
                            _DriverCommand.CommandText = "SELECT ISNULL(PayBankName,N''), ISNULL(PayBankCode,N''), ISNULL(PayAccountNo,N''), ISNULL(PayInputName,N'') FROM Drivers WHERE DriverId = @DriverId";
                            _DriverCommand.Parameters.AddWithValue("@DriverId", _model.DriverId);
                            var PayBankName = "";
                            var PayBankCode = "";
                            var PayAccountNo = "";
                            var PayInputName = "";
                            using (var _Reader = _DriverCommand.ExecuteReader())
                            {
                                if (_Reader.Read())
                                {
                                    PayBankName = _Reader.GetStringN(0);
                                    PayBankCode = _Reader.GetStringN(1);
                                    PayAccountNo = _Reader.GetStringN(2);
                                    PayInputName = _Reader.GetStringN(3);
                                    try
                                    {
                                        try
                                        {
                                            PayAccountNo = m_crypt.Decrypt(PayAccountNo).Replace("\0", string.Empty).Trim();
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                " INSERT Trades (RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, UseTax, HasAcc, AllowAcc, SubClientId, ClientUserId,HasETax,trusteeMgtKey,SourceType,TransportDate,AcceptCount,Splits,EtaxUserId,EtaxUserName)Values(@RequestDate, @BeginDate, @EndDate, @DriverName, @Item, @Price, @VAT, @Amount, @PayState, @PayDate, @PayBankName, @PayBankCode, @PayAccountNo, @PayInputName, @DriverId, @ClientId, @UseTax, @HasAcc, @AllowAcc, @SubClientId, @ClientUserId,@HasETax,@trusteeMgtKey,@SourceType,@TransportDate,@AcceptCount,@Splits,@EtaxUserId,@EtaxUserName)" +
                                " SELECT @@IDENTITY ";
                            cmd.Parameters.AddWithValue("@RequestDate", dtpRequestDate.Value);
                            cmd.Parameters.AddWithValue("@BeginDate", dtpRequestDate.Value);
                            cmd.Parameters.AddWithValue("@EndDate", dtpRequestDate.Value);
                            cmd.Parameters.AddWithValue("@DriverName", _model.CarYear);
                            cmd.Parameters.AddWithValue("@Item", "수수료");
                            cmd.Parameters.AddWithValue("@Price", _Price.ToString().Replace(",", ""));
                            cmd.Parameters.AddWithValue("@VAT", _Vat.ToString().Replace(",", ""));
                            cmd.Parameters.AddWithValue("@Amount", _model.TSPrice);
                            cmd.Parameters.AddWithValue("@PayState", 2);
                            cmd.Parameters.AddWithValue("@PayDate", dtpRequestDate.Value);
                            cmd.Parameters.AddWithValue("@PayBankName", PayBankName);


                            cmd.Parameters.AddWithValue("@PayBankCode", PayBankCode);
                            cmd.Parameters.AddWithValue("@PayAccountNo", PayAccountNo);
                            cmd.Parameters.AddWithValue("@PayInputName", PayInputName);


                            cmd.Parameters.AddWithValue("@DriverId", _model.DriverId);
                            cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                            cmd.Parameters.AddWithValue("@SubClientId", DBNull.Value);
                            cmd.Parameters.AddWithValue("@ClientUserId", DBNull.Value);
                            cmd.Parameters.AddWithValue("@TransportDate", _model.AcceptTime);
                            cmd.Parameters.AddWithValue("@AcceptCount", _model.OrderIdList.Count());

                            //제공이 아닐때
                            if (_ServiceState != 1)
                            {
                                //현금
                                cmd.Parameters.AddWithValue("@HasAcc", 0);
                            }
                            else
                            {
                                //카드
                                cmd.Parameters.AddWithValue("@HasAcc", 1);

                            }

                            if (LocalUser.Instance.LogInInformation.IsSubClient)
                            {
                                cmd.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                                if (LocalUser.Instance.LogInInformation.IsAgent)
                                {
                                    cmd.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                                }
                            }
                            cmd.Parameters.AddWithValue("@UseTax", true);

                          
                            cmd.Parameters.AddWithValue("@AllowAcc", false);
                            cmd.Parameters.AddWithValue("@HasETax", true);
                            cmd.Parameters.AddWithValue("@trusteeMgtKey", "t" + _model.DriverId + DateTime.Now.ToString("yyyyMMddHHmmss"));
                            //}
                            cmd.Parameters.AddWithValue("@SourceType", 0);
                            if(rdoSplit.Checked)
                            {
                                cmd.Parameters.AddWithValue("@Splits", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Splits", 0);
                            }

                            cmd.Parameters.AddWithValue("@EtaxUserId", LocalUser.Instance.LogInInformation.LoginId);

                            if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.ClientUserId > 0)
                            {
                                var mClientUsesAdapter = new ClientUsersTableAdapter();
                                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);
                                if (mTable.Any(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId))
                                {
                                    cmd.Parameters.AddWithValue("@EtaxUserName", mTable.First().Name);
                                }
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@EtaxUserName", LocalUser.Instance.LogInInformation.Client.CEO);
                            }

                          
                            

                            //   cmd.ExecuteNonQuery();
                            Object O = cmd.ExecuteScalar();
                            if (O != null)
                                _TradeId = Convert.ToInt32(O);
                            if (LocalUser.Instance.LogInInformation.Client.EtaxGubun == "P")
                            {

                                if (SendInvoice(_model.DriverId, _TradeId, LocalUser.Instance.LogInInformation.ClientId))
                                {
                                    SqlCommand _MiziCommand = cn.CreateCommand();

                                    _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                                    _MiziCommand.Parameters.AddWithValue("@Driverid", _model.DriverId);
                                    _MiziCommand.Parameters.AddWithValue("@Mizi", _model.TSPrice.ToString().Replace(",", ""));
                                    _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                    _MiziCommand.ExecuteNonQuery();



                                    SqlCommand _TaxTrade = cn.CreateCommand();
                                    if (_model.OrderIdList.Count > 1)
                                    {
                                        _TaxTrade.CommandText = "UPDATE Orders   SET TaxTradeId =  @TaxTradeId WHERE Orderid in(" + String.Join(",", _model.OrderIdList) + ")  ";
                                        _TaxTrade.Parameters.AddWithValue("@TaxTradeId", _TradeId);

                                    }
                                    else
                                    {
                                        _TaxTrade.CommandText = "UPDATE Orders   SET TaxTradeId =  @TaxTradeId WHERE OrderId = @OrderId ";
                                        _TaxTrade.Parameters.AddWithValue("@TaxTradeId", _TradeId);
                                        _TaxTrade.Parameters.AddWithValue("@OrderId", _model.OrderId);
                                    }
                                    _TaxTrade.ExecuteNonQuery();




                                    var _Driver = _DriverTable.Where(c => c.DriverId == Convert.ToInt32(_model.DriverId));
                                    ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                                    {
                                        ClientId = LocalUser.Instance.LogInInformation.ClientId,
                                        CDate = DateTime.Now,
                                        Amount = -110,
                                        MethodType = "전자 세금계산서",
                                        Remark = $"{_model.CarYear} ({_Driver.First().CarNo})",
                                    });
                                    ShareOrderDataSet.Instance.SaveChanges();

                                    _Success++;
                                }
                                else
                                {

                                    SqlCommand Deletecmd = cn.CreateCommand();
                                    Deletecmd.CommandText = " DELETE Trades WHERE TradeId = @TradeId ";

                                    Deletecmd.Parameters.AddWithValue("@TradeId", _TradeId);
                                    Deletecmd.ExecuteNonQuery();
                                    _Fail++;
                                }
                            }
                            else
                            {
                                if (SendNice(_model.DriverId, _TradeId, LocalUser.Instance.LogInInformation.ClientId))
                                {
                                    SqlCommand _MiziCommand = cn.CreateCommand();

                                    

                                    _MiziCommand.CommandText = "UPDATE DriverInstances   SET Mizi =Mizi +  @Mizi WHERE DriverId = @Driverid AND ClientId = @ClientId";
                                    _MiziCommand.Parameters.AddWithValue("@Driverid", _model.DriverId);
                                    _MiziCommand.Parameters.AddWithValue("@Mizi", _model.TSPrice.ToString().Replace(",", ""));
                                    _MiziCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);




                                    _MiziCommand.ExecuteNonQuery();


                                    SqlCommand _BillNoCommand = cn.CreateCommand();

                                    _BillNoCommand.CommandText = "UPDATE Trades   SET BillNo = @BillNo WHERE TradeId = @TradeId ";
                                    _BillNoCommand.Parameters.AddWithValue("@BillNo", billNo);
                                    _BillNoCommand.Parameters.AddWithValue("@TradeId", _TradeId);


                                    _BillNoCommand.ExecuteNonQuery();





                                    var _Driver = _DriverTable.Where(c => c.DriverId == Convert.ToInt32(_model.DriverId));
                                    ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                                    {
                                        ClientId = LocalUser.Instance.LogInInformation.ClientId,
                                        CDate = DateTime.Now,
                                        Amount = -110,
                                        MethodType = "전자 세금계산서",
                                        Remark = $"{_model.CarYear} ({_Driver.First().CarNo})",
                                    });
                                    ShareOrderDataSet.Instance.SaveChanges();


                                }
                                else
                                {

                                    SqlCommand Deletecmd = cn.CreateCommand();
                                    Deletecmd.CommandText = " DELETE Trades WHERE TradeId = @TradeId ";

                                    Deletecmd.Parameters.AddWithValue("@TradeId", _TradeId);
                                    Deletecmd.ExecuteNonQuery();
                                    MessageBox.Show("세금계산서 추가 실패하였습니다.\r\n" + errMsg, Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                                }

                            }
                            //}
                            cn.Close();

                        }

                    }
                    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

                    pnProgress.Invoke(new Action(() => MessageBox.Show($"발행성공 건 : {_Success} 건\r\n발행실패 건 : {_Fail} 건\r\n\r\n 전자세금계산서 발행이 완료 되었습니다.\r\n", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    pnProgress.Invoke(new Action(() => ucNSTATS4.btnSearch_Click(null, null)));


                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("세금계산서 추가 실패하였습니다.\r\n" + ex.Message.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
        private string Memo = "▶운송/주선사用 '차세로' PC프로그램 안내 운송/주선사에서는 http://www.chasero.co.kr 접속하여 프로그램을 설치하시면, 화물차주가 발행한 '전자세금계산서' 및 '첨부화일(인수증/송장 등)'을 조회해 보실 수 있습니다. 문의: 1661 - 6090 ";
        private bool SendInvoice(int driverId, int tradeId, int clientId)
        {
            //LoadTable();
            this.tradesTableAdapter.Fill(this.tradeDataSet.Trades);
            clientsTableAdapter.Fill(clientDataSet.Clients);
            InitDriverTable();
            var _Driver = _DriverTable.Where(c => c.DriverId == driverId);
            var _Client = clientDataSet.Clients.FirstOrDefault(c => c.ClientId == clientId);

            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            bool forceIssue = false;        // 지연발행 강제여부
            var TPrice = _Trade.Price;
            var TAmount = _Trade.Amount;
            var TVat = _Trade.VAT;

            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();


            taxinvoice.writeDate = dtpRequestDate.Value.ToString("yyyyMMdd");                      //필수, 기재상 작성일자
            taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
            taxinvoice.issueType = "위수탁";                        //필수, {정발행, 역발행, 위수탁}
            taxinvoice.purposeType = "청구";                        //필수, {영수, 청구}
            taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
            taxinvoice.taxType = "과세";                            //필수, {과세, 영세, 면세}

            taxinvoice.invoicerCorpNum = _Driver.First().BizNo.Replace("-", "");             //공급자 사업자번호
            //taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
            taxinvoice.invoicerCorpName = _Driver.First().Name;
            taxinvoice.invoicerMgtKey = tradeId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoicerCEOName = _Driver.First().CEO;
            taxinvoice.invoicerAddr = _Driver.First().AddressState + " " + _Driver.First().AddressCity + " " + _Driver.First().AddressDetail;
            taxinvoice.invoicerBizClass = _Driver.First().Upjong;
            taxinvoice.invoicerBizType = _Driver.First().Uptae;
            taxinvoice.invoicerContactName = _Driver.First().CEO;
            taxinvoice.invoicerEmail = _Driver.First().Email;
            taxinvoice.invoicerTEL = _Driver.First().PhoneNo;
            taxinvoice.invoicerHP = _Driver.First().MobileNo;
            taxinvoice.invoicerSMSSendYN = false;                    //정발행시(공급자->공급받는자) 문자발송기능 사용시 활용

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            taxinvoice.invoiceeType = "사업자";                     //공급받는자 구분, {사업자, 개인, 외국인}
            taxinvoice.invoiceeCorpNum = _Client.BizNo.Replace("-", "");              //공급받는자 사업자번호
            taxinvoice.invoiceeCorpName = _Client.Name;
            // taxinvoice.invoiceeMgtKey = "";                         //역발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoiceeCEOName = _Client.CEO;
            taxinvoice.invoiceeAddr = _Client.AddressState + " " + _Client.AddressCity + " " + _Client.AddressDetail;
            taxinvoice.invoiceeBizClass = _Client.Upjong;
            taxinvoice.invoiceeBizType = _Client.Uptae;
            taxinvoice.invoiceeTEL1 = _Client.MobileNo;
            taxinvoice.invoiceeContactName1 = _Client.CEO;
            taxinvoice.invoiceeEmail1 = _Client.Email;
            taxinvoice.invoiceeHP1 = _Client.MobileNo;
            taxinvoice.invoiceeSMSSendYN = false;                   //역발행시(공급받는자->공급자) 문자발송기능 사용시 활용

            taxinvoice.supplyCostTotal = TPrice.ToString().Replace(".00", "");                  //필수 공급가액 합계"
            taxinvoice.taxTotal = TVat.ToString().Replace(".00", "");                      //필수 세액 합계
            taxinvoice.totalAmount = TAmount.ToString().Replace(".00", "");                  //필수 합계금액.  공급가액 + 세액

            taxinvoice.modifyCode = null;                           //수정세금계산서 작성시 1~6까지 선택기재.
            taxinvoice.originalTaxinvoiceKey = "";                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
            taxinvoice.serialNum = "1";
            taxinvoice.cash = "";                                   //현금
            taxinvoice.chkBill = "";                                //수표
            taxinvoice.note = "";                                   //어음
            taxinvoice.credit = "";                                 //외상미수금
            taxinvoice.remark1 = _Trade.TransportDate.ToString("d").Replace("-", "/") + " " + _Trade.StartState + " → " + _Trade.StopState + " ( ☎ " + _Driver.First().MobileNo + " )" + " (" + _Trade.PayBankName + "/" + _Trade.PayInputName + "/" + _Trade.PayAccountNo.Replace("\0", "") + ")";
            taxinvoice.remark2 = "";
            taxinvoice.remark3 = "";
            taxinvoice.kwon = 1;
            taxinvoice.ho = 1;

            taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
            taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

            string CtrusteeMgtKey = _Trade.trusteeMgtKey;
            #region 수탁자
            //수탁자 문서관리번호
            taxinvoice.trusteeMgtKey = CtrusteeMgtKey; //_Trade.trusteeMgtKey;
            //사업자번호
            taxinvoice.trusteeCorpNum = "1148654906";
            //상호
            taxinvoice.trusteeCorpName = "(주)에듀빌시스템";
            //대표자
            taxinvoice.trusteeCEOName = "박양우";

            //주소
            taxinvoice.trusteeAddr = "서울특별시 금천구 디지털로9길 65, 408호(가산동,백상스타타워1차)";

            //업태
            taxinvoice.trusteeBizType = "";

            //업종
            taxinvoice.trusteeBizClass = "";


            //담당자성명
            taxinvoice.trusteeContactName = "박양우";

            #endregion


            taxinvoice.detailList = new List<TaxinvoiceDetail>();

            TaxinvoiceDetail detail = new TaxinvoiceDetail();

            detail.serialNum = 1;                                   //일련번호
            detail.purchaseDT = dtpRequestDate.Value.ToString("yyyyMMdd"); //DateTime.Now.ToString("yyyyMMdd");                         //거래일자
            detail.itemName = "차주수수료(" + _Driver.First().CarNo + ")";
            detail.spec = "";
            detail.qty = "1";                                       //수량
            detail.unitCost = TPrice.ToString().Replace(".00", ""); ;                             //단가
            detail.supplyCost = TPrice.ToString().Replace(".00", ""); ;                           //공급가액
            detail.tax = TVat.ToString().Replace(".00", ""); ;                                   //세액
            detail.remark = "";

            taxinvoice.detailList.Add(detail);

            detail = new TaxinvoiceDetail();

            try
            {
                Response response = taxinvoiceService.RegistIssue("1148654906", taxinvoice, forceIssue, Memo);


                return true;



            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");


                // MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return false;
            }
        }

        string billNo = "";
        string errMsg = "";
        private bool SendNice(int driverId, int tradeId, int clientId)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            errMsg = "";
            billNo = "";
            
            LocalUser.Instance.LogInInformation.LoadClient();
            var _Driver = _DriverTable.Where(c => c.DriverId == driverId);
            var _Client = clientDataSet.Clients.FirstOrDefault(c => c.ClientId == clientId);
            var _Admininfo = baseDataSet.AdminInfoes.FirstOrDefault();

            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            bool forceIssue = false;        // 지연발행 강제여부
            var TPrice = _Trade.Price;
            var TAmount = _Trade.Amount;
            var TVat = _Trade.VAT;


            var _Clients = LocalUser.Instance.LogInInformation.Client;
            //   var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";


            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "dpebqlf54906**" }).ToString();
            string Tdtp_Date = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");


            string _purposeType = "02";

            // TaxInvoiceErrorDic.Clear();




            string DutyNum = string.Empty;
            string DescriptionText = "";
            int i = 0;

            DescriptionText = "▶운송 / 주선사用 '차세로' PC프로그램 안내 운송 / 주선사에서는 http://www.chasero.co.kr 접속하여 프로그램을 설치하시면, 화물차주가 발행한 '전자세금계산서' 및 '첨부화일(인수증/송장 등)'을 조회해 보실 수 있습니다. 문의: 1661 - 6090";


            decimal _Price = 0;
            decimal _Vat = 0;
            decimal _Amont = 0;
            _Price = _Trade.Price;
            _Vat = _Trade.VAT;
            _Amont = _Trade.Amount;


            string dtiXml = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            " xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            "   <TaxInvoiceDocument>\r\n" +
            $"       <TypeCode>0103</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            $"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            $"      <IssueDateTime>{Tdtp_Date.Replace("/", "")}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            $"      <AmendmentStatusCode></AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            $"      <PurposeCode>{_purposeType}</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            $"      <OriginalIssueID></OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            $"  </TaxInvoiceDocument>\r\n" +
            $"  <TaxInvoiceTradeSettlement>\r\n" +
            $"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            $"          <ID>{_Driver.First().BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Driver.First().Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            $"          <NameText>{_Driver.First().Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Driver.First().Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Driver.First().CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <URICommunication>{_Driver.First().Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Driver.First().AddressState + " " + _Driver.First().AddressCity + " " + _Driver.First().AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
            $"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
            $"          <ID>{_Client.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Client.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
            $"          <NameText>{_Client.Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Client.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Client.CEO}</NameText>	<!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <PrimaryDefinedContact>\r\n" +
            $"              <PersonNameText>{_Client.CEO}</PersonNameText>	<!-- 담당자명 -->\r\n" +
            $"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <TelephoneCommunication>{_Client.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
            $"              <URICommunication>{_Client.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </PrimaryDefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Client.AddressState + " " + _Client.AddressCity + " " + _Client.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +
            $"      <BrokerParty> <!-- 수탁자정보 --> \r\n" +
            $"          <ID>{_Admininfo.BIzNo.Replace("-", "")}</ID>     <!-- 사업자번호 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>         <!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +

            $"          <NameText>{_Admininfo.CustName}</NameText>         <!-- 상호명 -->\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Admininfo.ownerName}</NameText>       <!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Admininfo.addr1 + " " + _Admininfo.addr2}</LineOneText>          <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"          <TypeCode>{_Admininfo.BizCond}</TypeCode>  <!-- 업태 -->\r\n" +
            $"          <ClassificationCode>{_Admininfo.bizItem}</ClassificationCode>   <!-- 종목 -->\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <DepartmentNameText>물류팀</DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <PersonNameText>{_Admininfo.rsbmName}</PersonNameText>      <!-- 담당자명 -->\r\n" +
            $"              <TelephoneCommunication>{_Admininfo.hpNo}</TelephoneCommunication>           <!-- 전화번호 -->\r\n" +
            $"             	<URICommunication>{_Admininfo.email}</URICommunication>  <!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"      </BrokerParty> <!-- 수탁자정보 끝 -->\r\n" +
            $"          <SpecifiedPaymentMeans>\r\n" +
            $"              <TypeCode>10</TypeCode> <!-- 10:현금, 20:수표, 30:어음, 40:외상미수금 -->\r\n" +
            $"              <PaidAmount>{Convert.ToInt64(_Amont)}</PaidAmount> <!-- 부가세가 포함된 금액 -->\r\n" +
            $"          </SpecifiedPaymentMeans>\r\n" +
            $"          <SpecifiedMonetarySummation>\r\n" +
            $"              <ChargeTotalAmount>{Convert.ToInt64(_Price)}</ChargeTotalAmount>	<!-- 총 공급가액 -->\r\n" +
            $"              <TaxTotalAmount>{Convert.ToInt64(_Vat)}</TaxTotalAmount>	<!-- 총 세액 -->\r\n" +
            $"              <GrandTotalAmount>{Convert.ToInt64(_Amont)}</GrandTotalAmount>	<!-- 총액(공급가액+세액) -->\r\n" +
            $"          </SpecifiedMonetarySummation>\r\n" +
            $"  </TaxInvoiceTradeSettlement>\r\n";

            dtiXml += "" +
              $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
              $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
              $"      <InvoiceAmount>{Convert.ToInt64(_Price)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
              $"      <ChargeableUnitQuantity>1</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
              $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
              $"      <NameText>{"차주수수료(" + _Driver.First().CarNo + ")"}</NameText>  <!-- 품목명 -->\r\n" +
              $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
              $"          <TotalTax>\r\n" +
              $"              <CalculatedAmount>{Convert.ToInt64(_Vat)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
              $"          </TotalTax>\r\n" +
              $"          <UnitPrice>\r\n" +
              $"              <UnitAmount>{Convert.ToInt64(_Price)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
              $"          </UnitPrice>\r\n" +
              $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
              $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";


            dtiXml += "" +
            "</TaxInvoice>";










            //try
            //{
            //    webBrowser1.Url = new Uri("http://222.231.9.253/NiceEncoding.asp");
            //    //frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.frnNo }).ToString();
            //    //userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.userid }).ToString();
            //    passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            //  //  Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.Linkcd }).ToString();


            //    //MessageBox.Show(Encode.ToString());

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.Write(ex.Message);

            //}



            // var Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Clients.frnNo, _Clients.userid, _Clients.passwd, certPw, dtiXml, "Y", "N", "", "6");
            var Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "", "3");

            try
            {
                var ResultList = Result.Split('/');

                var retVal = ResultList[0];
                errMsg = ResultList[1];
                billNo = ResultList[2];
                var gnlPoint = ResultList[3];
                var outbnsPoint = ResultList[4];
                var totPoint = ResultList[5];
                //return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

                if (retVal == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {

            }




            try
            {
                //Response response = taxinvoiceService.RegistIssue("1148654906", taxinvoice, forceIssue, Memo);


                return true;



            }
            catch (PopbillException ex)
            {

                return false;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            lblYear.Text = "";
            lblTaxCount.Text = "0 건";
            dtpRequestDate.Value = DateTime.Now;

            ucNSTATS4.btnSearch_Click(null, null);
        }

        private void rdoSplit_CheckedChanged(object sender, EventArgs e)
        {

            DataLoad();
            lblTaxCount.Text = _ModelList.Count.ToString() + " 건";

        }
    }
}
