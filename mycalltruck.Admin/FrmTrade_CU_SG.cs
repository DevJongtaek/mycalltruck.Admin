using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Class.DataSet;

namespace mycalltruck.Admin
{
    public partial class FrmTrade_CU_SG : Form
    {
        int intTradeIds = 0;
        public FrmTrade_CU_SG()
        {
            InitializeComponent();
            cmb_PayState.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            cmb_Date.SelectedIndex = 0;
            cmb_AllowAcc.SelectedIndex = 0;
            cmb_LGD_Last_Function.SelectedIndex = 0;
            dtp_From.Value = DateTime.Now.AddMonths(-2);
            dtp_To.Value = DateTime.Now;
            _InitCmb();
            // 운송사
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btnUpdate.Enabled = true;
                label10.Visible = true;
                chk_PayState.Visible = true;
                LocalUser.Instance.LogInInformation.LoadClient();
            }
            // 관리자
            else
            {
                btnUpdate.Enabled = false;
                btnCurrentDelete.Enabled = false;
                label10.Visible = false;
                chk_PayState.Visible = false;
                btn_New.Visible = true;
                btn_New.Enabled = false;
            }
        }

        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        private void _InitCmb()
        {
            Dictionary<string, string> PayBank = new Dictionary<string, string>();
            PayBank.Add(" ", "은행선택");
            PayBank.Add("003", "기업은행");
            PayBank.Add("005", "외한은행");
            PayBank.Add("004", "국민은행");
            PayBank.Add("011", "농협");
            PayBank.Add("020", "우리은행");
            PayBank.Add("088", "신한은행");
            PayBank.Add("023", "SC제일");
            PayBank.Add("027", "씨티은행");
            PayBank.Add("032", "부산은행");
            PayBank.Add("039", "경남은행");
            PayBank.Add("031", "대구은행");
            PayBank.Add("071", "우체국");
            PayBank.Add("034", "광주은행");
            PayBank.Add("007", "수협");
            PayBank.Add("022", "상업은행");
            PayBank.Add("030", "대동은행");
            PayBank.Add("033", "충청은행");
            PayBank.Add("035", "제주은행");
            PayBank.Add("036", "경기은행");
            PayBank.Add("037", "전북은행");
            PayBank.Add("038", "강원은행");
            PayBank.Add("040", "충북은행");
            PayBank.Add("081", "하나은행");
            PayBank.Add("082", "보람은행");
            PayBank.Add("002", "산업은행");
            PayBank.Add("045", "새마을금고");
            PayBank.Add("054", "HSBC은행");
            PayBank.Add("048", "신협");
            PayBank.Add("090", "카카오뱅크");
            PayBank.Add("089", "케이뱅크");
            PayBank.Add("S0", "유안타증권");
            PayBank.Add("S1", "미래에셋");
            PayBank.Add("S2", "신한금융투자");
            PayBank.Add("S3", "삼성증권");
            PayBank.Add("S6", "한국투자증권");
            PayBank.Add("SG", "한화증권");

            Dictionary<string, string> HasACCList = new Dictionary<string, string>();
            HasACCList.Add("0", "현금");
            HasACCList.Add("1", "카드");
            cmb_HasAcc.DataSource = new BindingSource(HasACCList, null);
            cmb_HasAcc.DisplayMember = "Value";
            cmb_HasAcc.ValueMember = "Key";

        }
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void txt_MobileNo_TextChanged(object sender, EventArgs e)
        {

        }


        private void FrmTrade_Load(object sender, EventArgs e)
        {
            try
            {
                DriverRepository mDriverRepository = new DriverRepository();
                mDriverRepository.Select(baseDataSet.Drivers);
                btn_Search_Click(null, null);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        string fileString = string.Empty;
        string title = string.Empty;
        string FolderPath = string.Empty;



        int GridIndex = 0;
        private void btnUpdate_Click(object sender, EventArgs e)
        {

            err.Clear();

            if (txt_Item.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }


            if (txt_Price.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;

            }
            var _d = 0m;
            if (!decimal.TryParse(txt_Price.Text.Replace(",", ""), out _d) || !decimal.TryParse(txt_VAT.Text.Replace(",", ""), out _d) || !decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _d))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;

            }


            var PayState = 2;

            if (chk_PayState.Checked == true)
            {
                PayState = 1;
            }

            if (tradesBindingSource.Current != null)
            {
                var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    Selected.PayState = PayState;
                    tradesBindingSource.EndEdit();
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText =
                            "UPDATE Trades SET  RequestDate = @RequestDate" +
                            ", BeginDate = @BeginDate" +
                            ", EndDate = @EndDate" +
                            ", DriverName = @DriverName" +
                            ", Item = @Item " +
                            ", Price = @Price " +
                            ", VAT = @VAT " +
                            ", Amount = @Amount" +
                            ", ClientId = @ClientId" +
                            ", UseTax = @UseTax" +
                            ", HasAcc = @HasAcc " +
                            " WHERE TradeId = @TradeId";

                        cmd.Parameters.AddWithValue("@RequestDate", dtp_RequestDate.Value);
                        cmd.Parameters.AddWithValue("@BeginDate", dtp_BeginDate.Value);
                        cmd.Parameters.AddWithValue("@EndDate", dtp_EndDate.Value);
                        cmd.Parameters.AddWithValue("@DriverName", label83.Text);
                        cmd.Parameters.AddWithValue("@Item", txt_Item.Text);
                        cmd.Parameters.AddWithValue("@Price", txt_Price.Text.Replace(",", ""));
                        cmd.Parameters.AddWithValue("@VAT", txt_VAT.Text.Replace(",", ""));
                        cmd.Parameters.AddWithValue("@Amount", txt_Amount.Text.Replace(",", ""));
                        cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        cmd.Parameters.AddWithValue("@UseTax", true);
                        if (cmb_HasAcc.SelectedIndex == 0)
                        {
                            cmd.Parameters.AddWithValue("@HasAcc", false);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@HasAcc", true);
                        }
                        cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "세금계산서", 1), "세금계산서 수정 성공");
            if (newDGV1.RowCount > 1)
            {
                GridIndex = tradesBindingSource.Position;
                btn_Search_Click(null, null);
                newDGV1.CurrentCell = newDGV1.Rows[GridIndex].Cells[0];
            }
            else
            {
                btn_Search_Click(null, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_PayState.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            dtp_From.Value = DateTime.Now.AddMonths(-1);
            dtp_To.Value = DateTime.Now;
            txt_Search.Clear();

            cmb_Date.SelectedIndex = 0;
            cmb_AllowAcc.SelectedIndex = 0;
            cmb_LGD_Last_Function.SelectedIndex = 0;

            btn_Search_Click(null, null);
        }
        bool MethodProcess = false;
        object Old = null;
        private void tradesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            MethodProcess = true;
            if (tradesBindingSource.Current == null)
            {
                chk_PayState.Checked = false;
            }
            else
            {
                if(Old != null && (Old as DataRow).RowState == DataRowState.Modified)
                {
                    (Old as DataRow).RejectChanges();
                }
                var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
                Old = Selected;
                if (Selected != null)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "SELECT OrderId, AcceptTime, ISNULL(Customers.SangHo, N'') AS Customer, Price, Item, StartState, StartCity, StopState, StopCity FROM ORDERS LEFT JOIN CUSTOMERS ON ORDERS.CustomerId = CUSTOMERS.CustomerId WHERE TradeId = @TradeId";
                            _Command.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                            using (SqlDataReader _Reader = _Command.ExecuteReader())
                            {
                                var _Table = new DataTable();
                                _Table.Load(_Reader);
                                ordersBindingSource.DataSource = _Table;
                            }
                        }
                        _Connection.Close();
                    }
                    var _Driver = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == Selected.DriverId);
                    rdb_Tax0.Checked = true;
                    txt_Price.ReadOnly = false;
                    txt_Amount.ReadOnly = true;
                    // 상태에 따라서 수정/삭제 가능 여부 변경
                    // 0. 카드는 기본정보를 수정할 수 없다.
                    // 1. 결제 건은 수정/삭제가 안된다.
                    // 2. 현금은 결제여부와 결제일자 변경이 가능하다.
                    // 3. 결제여부와 결제일자는 수정버튼을 누르지 않고 변경하도록 하자.
                    // 4. 외부계산서 체크는 결제여부와 관련없이 언제나 변경가능(수정버튼 누르지 않고 수정)
                    if (Selected.HasAcc)
                    {
                        chk_PayState.Enabled = false;
                        dtp_PayDate.Enabled = false;
                        btnUpdatePayDate.Enabled = false;
                        cmb_HasAcc.SelectedIndex = 1;
                    }
                    else
                    {
                        chk_PayState.Enabled = true;
                        dtp_PayDate.Enabled = true;
                        btnUpdatePayDate.Enabled = true;
                        cmb_HasAcc.SelectedIndex = 0;
                    }
                    if (Selected.PayState == 1)
                    {
                        btnCurrentDelete.Enabled = false;
                        btnUpdate.Enabled = false;

                        chk_PayState.Checked = true;
                        lbl_PayDate.Visible = true;
                        dtp_PayDate.Visible = true;
                        btnUpdatePayDate.Visible = true;

                        dtp_BeginDate.Enabled = false;
                        dtp_EndDate.Enabled = false;
                        txt_Item.Enabled = false;
                        dtp_RequestDate.Enabled = false;
                        txt_Price.Enabled = false;
                        rdb_Tax0.Enabled = false;
                        rdb_Tax1.Enabled = false;
                        txt_Amount.Enabled = false;
                        cmb_HasAcc.Enabled = false;
                    }
                    else
                    {
                        btnCurrentDelete.Enabled = true;
                        btnUpdate.Enabled = true;

                        chk_PayState.Checked = false;
                        lbl_PayDate.Visible = false;
                        dtp_PayDate.Visible = false;
                        btnUpdatePayDate.Visible = false;

                        dtp_BeginDate.Enabled = true;
                        dtp_EndDate.Enabled = true;
                        txt_Item.Enabled = true;
                        dtp_RequestDate.Enabled = true;
                        txt_Price.Enabled = true;
                        rdb_Tax0.Enabled = true;
                        rdb_Tax1.Enabled = true;
                        txt_Amount.Enabled = true;
                        cmb_HasAcc.Enabled = true;
                    }

                    label83.Text = Selected.DriverName;
                    txt_Price.Text = ((long)Selected.Price).ToString("N0");
                    txt_VAT.Text = ((long)Selected.VAT).ToString("N0");
                    txt_Amount.Text = ((long)Selected.Amount).ToString("N0");
                    dtp_PayDate.Value = Selected.PayDate;
                    intTradeIds = Selected.TradeId;
                    //acceptInfoesBindingSource.Filter = " TradeId = '" + intTradeIds + "'";
                    newDGV2Binding(Selected.TradeId);
                    if(Selected.SourceType == 1)
                    {
                        txt_Price.ReadOnly = true;
                    }
                    else
                    {
                        txt_Price.ReadOnly = false;
                    }
                }
                else
                {
                    chk_PayState.Checked = false;
                }
            }

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btnCurrentDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }

            MethodProcess = false;
        }
        private int UpdateDB()
        {
            return 0;
            Int64 sPrice = 0;
            Int64 Price = 0;
            int _rows = 0;
            try
            {
                #region 업데이트
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    var Command1 = cn.CreateCommand();
                    Command1.CommandText = "SELECT convert(bigint,Sum(Price))  FROM AcceptInfoes where Tradeid = '" + intTradeIds + "'";
                    var o1 = Command1.ExecuteScalar();
                    if (o1 != null)
                    {
                        sPrice = Int64.Parse(o1.ToString());
                    }
                    cn.Close();

                    Price = sPrice;
                    Int64 VAT = Price / 10;

                    Int64 Amount = Price + VAT;


                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();



                    cmd.CommandText =
                       "UPDATE Trades SET Price = @Price, VAT = @VAT,Amount = @Amount WHERE Tradeid = @Tradeid";



                    cmd.Parameters.AddWithValue("@Tradeid", intTradeIds);
                    cmd.Parameters.AddWithValue("@Price", Price);
                    cmd.Parameters.AddWithValue("@VAT", VAT);
                    cmd.Parameters.AddWithValue("@Amount", Amount);
                    cmd.ExecuteNonQuery();
                    cn.Close();


                }
                #endregion
            }
            catch (DBConcurrencyException)
            {
                MessageBox.Show("다른 사용자에 의해서 해당 데이터가 변경되었습니다. 작업이 취소됩니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //if (LocalUser.Instance.LogInInfomation.UseItemJisa)
                //    SingleDataSet.Instance.TblItemTableAdapter.FillJisa(SingleDataSet.Instance.TblItem, LocalUser.Instance.LogInInfomation.JisaCode);
                //else
                //    SingleDataSet.Instance.TblItemTableAdapter.Fill(SingleDataSet.Instance.TblItem);
                _rows = 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "상품정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }
        private void chk_PayState_CheckedChanged(object sender, EventArgs e)
        {
            if (tradesBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            if (Selected == null)
                return;
            if (Selected != null)
            {
                if (Selected.PayState != 1)
                {
                    dtp_PayDate.Value = DateTime.Now;
                }
            }
            if(!MethodProcess)
            {
                Selected.RejectChanges();
                if (Selected.HasAcc)
                    return;
                var PayState = 2;
                if (chk_PayState.Checked == true)
                {
                    PayState = 1;
                }
                // 여기서 데이터 적용한다.
                var TradeId = Selected.TradeId;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "UPDATE Trades SET  PayState = @PayState " +
                        "WHERE TradeId = @TradeId AND HasAcc = 0";
                    cmd.Parameters.AddWithValue("@PayState", PayState);
                    cmd.Parameters.AddWithValue("@TradeId", TradeId);
                    int _R = cmd.ExecuteNonQuery();
                    if (_R > 0)
                    {
                        if(PayState == 1)
                        {
                            cmd.CommandText =
                                @"UPDATE Orders
                            SET DriverPayDate = CONVERT(NVARCHAR(10),GETDATE(),126)
                                ,DriverPayPrice = @DriverPayPrice
                                ,DriverPayVAT = @DriverPayVAT
                                ,UseCardPay = 0
                                ,DriverPoint = null
                            WHERE TradeId = @TradeId";
                            cmd.Parameters.AddWithValue("@DriverPayPrice", Selected.Price);
                            cmd.Parameters.AddWithValue("@DriverPayVAT", Selected.VAT);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText =
                                @"UPDATE Orders
                            SET DriverPayDate = null
                                ,DriverPayPrice = null
                                ,DriverPayVAT = null
                                ,UseCardPay = null
                                ,DriverPoint = null
                            WHERE TradeId = @TradeId";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    cn.Close();
                }
                LoadTableOne(TradeId);
            }
        }

        private String GetSelectCommand()
        {
            return @"SELECT  TradeId, Trades.RequestDate, BeginDate, EndDate, 
                Item, Price, VAT, Amount, PayState, PayDate, Trades.PayBankName, Trades.PayBankCode, Trades.PayAccountNo, Trades.PayInputName, 
                Trades.DriverId, Trades.ClientId, Trades.UseTax, LGD_OID, 
                LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, AllowAcc, HasAcc, ClientAccId, 
                SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, 
                Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, 
                HasETax, SourceType,
                Drivers.LoginId AS DriverLoginId, Drivers.CarNo AS DriverCarNo, Drivers.BizNo AS DriverBizNo, Drivers.Name AS DriverName, 
                Drivers.ServiceState, Drivers.MID, 
                Clients.Code AS ClientCode, Clients.Name AS ClientName, Trades.AcceptCount, Trades.SubClientId, Trades.ClientUserId,
Trades.BizNo, Trades.Name
                FROM     Trades
                JOIN Drivers ON Trades.DriverId = Drivers.DriverId
                JOIN Clients ON Trades.ClientId = Clients.ClientId ";
        }



        private void LoadTable()
        {
            tradeDataSet.Trades.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();
                    WhereStringList.Add("Drivers.ServiceType = 3");
                    // 1. 본점/지사
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            WhereStringList.Add("Trades.ClientUserId = @ClientUserId");
                            _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        }
                        else
                        {
                            WhereStringList.Add("Trades.SubClientId = @SubClientId");
                            _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        }
                    }
                    else if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        WhereStringList.Add("Trades.ClientId = @ClientId");
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    }
                    // 2. 조회 기간
                    String _DateColumn = cmb_Date.SelectedIndex == 0 ? "Trades.RequestDate" : "Trades.PayDate";
                    WhereStringList.Add($"{_DateColumn} >= @Begin AND {_DateColumn} < @End");
                    _Command.Parameters.AddWithValue("@Begin", dtp_From.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtp_To.Value.Date.AddDays(1));
                    // 
                    if (cmb_Search.Text == "사업자번호")
                    {
                        WhereStringList.Add(string.Format("REPLACE(BizNo,N'-',N'') Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "상호")
                    {
                        WhereStringList.Add(string.Format("Name Like  '%{0}%'", txt_Search.Text));
                    }


                    if (cmb_Date.SelectedIndex == 1)
                    {
                        WhereStringList.Add("PayState = 1");

                    }
                    else
                    {
                        if (cmb_PayState.Text == "결제")
                        {
                            WhereStringList.Add("PayState = 1");
                        }

                        else if (cmb_PayState.Text == "미결제")
                        {
                            WhereStringList.Add("PayState = 2");
                        }
                    }
                    if (cmb_AllowAcc.Text == "현금")
                    {
                        WhereStringList.Add("HasAcc = 0");
                    }
                    else if (cmb_AllowAcc.Text == "카드")
                    {
                        WhereStringList.Add("HasAcc = 1");
                    }
                    if (cmb_LGD_Last_Function.Text == "승인")
                    {
                        WhereStringList.Add("LGD_Last_Function = '승인'");
                    }
                    else if (cmb_LGD_Last_Function.Text == "취소")
                    {
                        WhereStringList.Add("LGD_Last_Function = '취소'");
                    }
                    else if (cmb_LGD_Last_Function.Text == "미결")
                    {
                        WhereStringList.Add("ISNULL(LGD_Last_Function, '') = ''");
                    }


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString ;
                    }
                    _Command.CommandText = SelectCommandText ;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        tradeDataSet.Trades.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        private void LoadTableOne(int TradeId)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();
                    List<String> WhereStringList = new List<string>();
                    WhereStringList.Add("Trades.TradeId = @TradeId");
                    _Command.Parameters.AddWithValue("@TradeId", TradeId);
                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        tradeDataSet.Trades.Load(_Reader);

                        //tradeDataSet.Trades.OrderByDescending(c => c.TradeId);
                    }
                }
                _Connection.Close();
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            if (newDGV1.SelectedCells.Count > 0)
            {
                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "세금계산", 1), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                tradesBindingSource.EndEdit();
                if (tradesBindingSource.Current != null)
                {
                    if (((DataRowView)tradesBindingSource.Current).Row is TradeDataSet.TradesRow Selected)
                    {
                        tradesBindingSource.EndEdit();
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                "Delete Trades  WHERE TradeId = @TradeId";
                            cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText =
                                "UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";
                            cmd.ExecuteNonQuery();

                            cn.Close();
                        }
                    }
                }
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "세금계산서", 1), "세금계산서 삭제 성공");
                btn_Search_Click(null, null);
            }
        }

        private void txt_ClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            DriverRepository mDriverRepository = new DriverRepository();
            if (!mDriverRepository.HasSelf())
            {
                MessageBox.Show("이 서비스는 화주가 당사에 카드로 결제할 수 있는 기능입니다." + Environment.NewLine +
                    "우선, 당 사가 P/G 가맹점 등록이 되어야 합니다." + Environment.NewLine +
                    "가맹점 등록(당사) 문의: 1661-6090", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            FrmTrade_Add_CU_SG _Form = new FrmTrade_Add_CU_SG();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void driversInfoBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void txt_Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Price_Leave(object sender, EventArgs e)
        {
            if (rdb_Tax0.Checked)
            {
                decimal _Price = 0;
                decimal _Vat = 0;
                decimal _Amount = 0;
                if (decimal.TryParse(txt_Price.Text.Replace(",", ""), out _Price))
                {
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }
            else if (rdb_Tax2.Checked)
            {
                decimal _Price = 0;
                if (decimal.TryParse(txt_Price.Text.Replace(",", ""), out _Price))
                {
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = "0";
                    txt_Amount.Text = _Price.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tradesBindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
        {

        }

        private void newDGV1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tradesBindingSource.Current == null)
                return;
            if (e.RowIndex < 0)
                return;

            var _Trade = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                var _TradePrice = 0m;
                foreach (var rowView in ordersBindingSource)
                {
                    var _Row = ((DataRowView)rowView).Row;
                    var _OrderId = Convert.ToInt32(_Row["OrderId"]);
                    var _Price = Convert.ToDecimal(_Row["Price"]);
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "UPDATE Orders SET Price = @Price WHERE OrderId = @OrderId";
                        _Command.Parameters.AddWithValue("@Price", _Price);
                        _Command.Parameters.AddWithValue("@OrderId", _OrderId);
                        _Command.ExecuteNonQuery();
                    }
                    _TradePrice += _Price;
                }
                _Trade.Price = _TradePrice;
                decimal _Vat = Math.Floor(_TradePrice * 0.1m);
                txt_Price.Text = _TradePrice.ToString("N0");
                txt_VAT.Text = _Vat.ToString("N0");
                var _Amount = (_TradePrice + _Vat);
                txt_Amount.Text = _Amount.ToString("N0");
                _Trade.VAT = _Vat;
                _Trade.Amount = _Amount;
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "UPDATE Trades SET Price = @Price, VAT = @VAT, Amount = @Amount WHERE TradeId = @TradeId";
                    _Command.Parameters.AddWithValue("@Price", _Trade.Price);
                    _Command.Parameters.AddWithValue("@VAT", _Trade.VAT);
                    _Command.Parameters.AddWithValue("@Amount", _Trade.Amount);
                    _Command.Parameters.AddWithValue("@TradeId", _Trade.TradeId);
                    _Command.ExecuteNonQuery();
                }
                _Connection.Close();
            }
            
        }

        private void newDGV2Binding(int iTradeID)
        {
            DataTable mDataTable = new DataTable();

            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT AccLogId, AccFunction, AccLogs.BankName, CardNo, AccLogs.TradeId, AccLogs.CreateDate, LGD_TID, LGD_MID,AccLogs.LGD_OID, LGD_AMOUNT, LGD_RESPCODE, LGD_RESPMSG,drivers.MID FROM AccLogs join trades on AccLogs.TradeId = trades.TradeId join drivers on trades.driverid = drivers.DriverId Where AccLogs.TradeId = @TradeId Order by AccLogs.CreateDate DESC";
                Command.Parameters.AddWithValue("@TradeId", iTradeID);
                Connection.Open();
                mDataTable.Load(Command.ExecuteReader());
                Connection.Close();
            }

            newDGV2.AutoGenerateColumns = false;
            newDGV2.DataSource = mDataTable;

        }



        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (newDGV2.RowCount - e.RowIndex).ToString("N0");
            }
        }

        private void TaxSumSearch()
        {
            var Filters = new List<string>();

            var FilterString = "";

            if (String.IsNullOrEmpty(txt_Search.Text))
            {
                FilterString = "";
            }
            else
            {
                if (cmb_Search.Text == "사업자번호")
                {
                    FilterString = string.Format("BizNo Like  '%{0}%'", txt_Search.Text);
                }
                else if (cmb_Search.Text == "상호")
                {
                    FilterString = string.Format("Name Like  '%{0}%'", txt_Search.Text);
                }
            }

            if (!String.IsNullOrEmpty(FilterString))
                Filters.Add(FilterString);

            var PayStateFilterString = "";
            if (cmb_PayState.Text == "결제")
            {
                PayStateFilterString = "PayState = 1";
            }
            else if (cmb_PayState.Text == "미결제")
            {
                PayStateFilterString = "PayState = 2";
            }

            if (!String.IsNullOrEmpty(PayStateFilterString))
                Filters.Add(PayStateFilterString);

            var AllowAccFilterString = "";

            if (cmb_AllowAcc.Text == "현금")
            {
                AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') = ''";
            }
            else if (cmb_AllowAcc.Text == "카드")
            {
                AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') <> ''";
            }


            if (!String.IsNullOrEmpty(AllowAccFilterString))
                Filters.Add(AllowAccFilterString);

            var LGD_Last_FunctionFilterString = "";
            if (cmb_LGD_Last_Function.Text == "승인")
            {
                LGD_Last_FunctionFilterString = "LGD_Last_Function = '승인'";
            }
            else if (cmb_LGD_Last_Function.Text == "취소")
            {
                LGD_Last_FunctionFilterString = "LGD_Last_Function = '취소'";
            }
            else if (cmb_LGD_Last_Function.Text == "미결")
            {
                LGD_Last_FunctionFilterString = "ISNULL(LGD_Last_Function, '') = ''";
            }

            if (!String.IsNullOrEmpty(LGD_Last_FunctionFilterString))
                Filters.Add(LGD_Last_FunctionFilterString);


            var MIDFilterString = "";



            if (!String.IsNullOrEmpty(MIDFilterString))
                Filters.Add(MIDFilterString);

            if (Filters.Count == 0)
                tradesBindingSource.Filter = "";
            else
                tradesBindingSource.Filter = String.Join(" AND ", Filters);
        }

        private void cmb_HasAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_HasAcc.SelectedIndex == 1)
            {
                chk_PayState.Enabled = false;
            }
            else
            {
                chk_PayState.Enabled = true;
            }
            if (tradesBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            if (Selected == null)
                return;
            if (cmb_HasAcc.SelectedIndex == 1)
            {
                Selected.HasAcc = true;
            }
            else
            {
                Selected.HasAcc = false;
            }
            if (!MethodProcess)
            {
                //Selected.RejectChanges();
                // 여기서 데이터 적용한다.
                var TradeId = Selected.TradeId;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "UPDATE Trades SET  HasAcc = @HasAcc " +
                        "WHERE TradeId = @TradeId AND PayState <> 1";
                    cmd.Parameters.AddWithValue("@HasAcc", Selected.HasAcc);
                    cmd.Parameters.AddWithValue("@TradeId", TradeId);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                LoadTableOne(TradeId);
            }
        }

        private void rdb_Car_CheckedChanged(object sender, EventArgs e)
        {


            FrmMDI frmMDI = new FrmMDI();




            FrmTrade_Client _Form = new FrmTrade_Client();
            _Form.MdiParent = this.MdiParent;

            _Form.Show();

            this.Close();
        }

        private void rdb_Client_CheckedChanged(object sender, EventArgs e)
        {
            //FrmMDI frmMDI = new FrmMDI();

            //frmMDI.Menu_Click("mnu0209", null);
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (dataGridView3.Columns[e.ColumnIndex] == Memo)
            //{
            //    e.Value = "화물운송료";
            //}
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_DriverId_TextChanged(object sender, EventArgs e)
        {


        }

        private void chkAllSelectTax_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                var cell = newDGV1[ColumnSelect.Index, i] as DataGridViewDisableCheckBoxCell;
                if (cell.Enabled)
                    newDGV1[ColumnSelect.Index, i].Value = chkAllSelectTax.Checked;
            }

        }

        private void txt_Price_Enter(object sender, EventArgs e)
        {
            txt_Price.Text = txt_Price.Text.Replace(",", "");
        }

        private void btn_New2_Click(object sender, EventArgs e)
        {
            FrmTradeNew2 _Form = new FrmTradeNew2();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Tax0.Checked)
            {
                txt_Amount.ReadOnly = true;
                txt_Price.ReadOnly = false;
                lbl_Price.ForeColor = Color.Blue;
                lbl_Amt.ForeColor = Color.Black;
                txt_Price.Focus();
            }
            else if (rdb_Tax1.Checked)
            {
                txt_Amount.ReadOnly = false;
                txt_Price.ReadOnly = true;
                lbl_Price.ForeColor = Color.Black;
                lbl_Amt.ForeColor = Color.Blue;
                txt_Amount.Focus();
            }
            else if (rdb_Tax2.Checked)
            {
                txt_Amount.ReadOnly = true;
                txt_Price.ReadOnly = false;
                lbl_Price.ForeColor = Color.Blue;
                lbl_Amt.ForeColor = Color.Black;
                txt_Amount.Text = txt_Price.Text;
                txt_VAT.Text = "0";
                txt_Price.Focus();
            }
        }

        private void txt_Amount_Enter(object sender, EventArgs e)
        {
            var AllowTaxBool = !rdb_Tax0.Checked;
            if (AllowTaxBool == true)
            {
                decimal _Price = 0;
                decimal _Vat = 0;
                decimal _Amount = 0;
                if (decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _Amount))
                {

                    _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }
        }

        private void txt_Amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Amount_Leave(object sender, EventArgs e)
        {
            if (rdb_Tax1.Checked)
            {
                decimal _Price = 0;
                decimal _Vat = 0;
                decimal _Amount = 0;
                if (decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _Amount))
                {
                    _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }


            txt_Amount.Text = txt_Amount.Text.Replace(",", "");
        }

        private void btnUpdatePayDate_Click(object sender, EventArgs e)
        {
            if (tradesBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            if (Selected == null)
                return;
            if (!Selected.HasAcc || Selected.PayState == 1)
            {
                // 여기서 데이터 적용한다.
                var TradeId = Selected.TradeId;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "UPDATE Trades SET  PayDate = @PayDate " +
                        "WHERE TradeId = @TradeId";
                    cmd.Parameters.AddWithValue("@PayDate", dtp_PayDate.Value);
                    cmd.Parameters.AddWithValue("@TradeId", TradeId);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                LoadTableOne(TradeId);
            }
        }

        private void Btn_Stat8_Click(object sender, EventArgs e)
        {
            FrmMNSTATS8 f = new FrmMNSTATS8();
            f.ShowDialog();
        }

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                var cell = newDGV1[ColumnSelect.Index, i] as DataGridViewDisableCheckBoxCell;
                if (cell.Enabled)
                    newDGV1[ColumnSelect.Index, i].Value = chkAllSelect.Checked;
            }
        }

        private void newDGV1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect1 = newDGV1.GetColumnDisplayRectangle(ColumnSelect.Index, true);
            chkAllSelect.Location = new Point(rect1.Location.X + 2, rect1.Location.Y + 4);
            if (rect1.Width == 0)
                chkAllSelect.Visible = false;
            else
                chkAllSelect.Visible = true;

        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
            if (Selected == null)
                return;

            if (e.ColumnIndex == ColumnSelect.Index)
            {
                if (Selected.PayState != 1 || !Selected.HasAcc)
                {
                    var _Cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = false;
                    _Cell.ReadOnly = true;
                }
            }
            else if (e.ColumnIndex == ColumnPayState.Index)
            {
                // 결제된것은 무조건 결제완료
                if (Selected.PayState == 1)
                {
                    e.Value = "결제완료";
                    e.CellStyle.ForeColor = Color.Gray;
                }
                else
                {
                    if (Selected.HasAcc)
                    {
                        if (Selected.ServiceState != 1)
                        {
                            e.Value = "심사중";
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else if (Selected.ServiceState == 1 && Selected.LGD_Last_Function != "승인")
                        {
                            e.Value = "결제가능";
                            e.CellStyle.ForeColor = Color.Blue;
                        }
                    }
                    else
                    {
                        e.Value = "";
                    }
                }
            }
            else if (e.ColumnIndex == ColumnNo.Index)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == ColumnHasAcc.Index)
            {
                if (Selected.HasAcc)
                {
                    e.Value = "카드";
                    newDGV1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
                else
                {
                    e.Value = "현금";
                    newDGV1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }
            else if (e.ColumnIndex == ColumnPayDate.Index)
            {
                try
                {
                    if (Selected.HasAcc && Selected.PayState == 1 && Selected.LGD_Last_Function != "")
                    {
                        e.Value = Selected.LGD_Last_Date.ToString("yyyy-MM-dd");
                    }
                    else if (Selected.PayState == 1)
                    {
                        e.Value = Selected.PayDate.ToString("yyyy-MM-dd");
                    }
                }
                catch (Exception)
                {
                    e.Value = "";
                }
            }
            else if (e.ColumnIndex == ColumnLGFunction.Index)
            {
                if (Selected.HasAcc)
                {
                    if (Selected.LGD_Last_Function == "승인")
                    {
                        e.Value = "승인";
                        e.CellStyle.ForeColor = Color.Blue;
                    }
                    else if (Selected.LGD_Last_Function == "취소")
                    {
                        e.Value = "취소";
                        e.CellStyle.ForeColor = Color.Red;
                    }
                }
                else
                {
                    if (Selected.PayState == 1)
                    {
                        e.Value = "현금";
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.Value = "";
                    }
                }
            }


        }

        private void newDGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnSelect.Index)
            {
                var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected == null)
                    return;
                if (Selected.PayState != 1)
                    return;

                DataGridViewCheckBoxCell _Cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                _Cell.Value = _Cell.Value == null || !((bool)_Cell.Value);
                newDGV1.RefreshEdit();
                newDGV1.NotifyCurrentCellDirty(true);
            }
        }

        private void newDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnSelect.Index)
            {
                newDGV1.RefreshEdit();
            }
        }

        private void btn_CancelAcc_Click(object sender, EventArgs e)
        {
            var Datas = new List<TradeDataSet.TradesRow>();
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                var _Cell = newDGV1[ColumnSelect.Index, i] as DataGridViewDisableCheckBoxCell;
                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                    //Datas.Add(((DataRowView)dataGridView1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow);
                    var _Row = ((DataRowView)newDGV1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (_Row.ServiceState == 1 && _Row.PayDate.ToString("d").Replace("-", "/") == DateTime.Now.ToString("d").Replace("-", "/"))
                        Datas.Add(_Row);

                }
            }
            if (Datas.Any())
            {
                FrmLogin_B f = new FrmLogin_B();
                f.ShowDialog();
                if (f.Accepted)
                {
                    Dialog_CancelAcc_CU d = new Dialog_CancelAcc_CU();
                    d.Trades = Datas.ToArray();
                    d.AuthKey = f.AuthKey;
                    d.ShowDialog();
                    btn_Search_Click(null, null);
                }
            }
            else
            {
                MessageBox.Show("먼저 취소할 항목들을 선택하여 주십시오.");
            }
        }
    }
}
