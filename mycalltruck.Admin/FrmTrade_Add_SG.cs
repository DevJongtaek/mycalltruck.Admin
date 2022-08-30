using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmTrade_Add_SG : Form
    {
        Int64 iPrice = 0;
        DESCrypt m_crypt = null;
        bool AllowTaxBool = false;
        public FrmTrade_Add_SG()
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");
        }



        private void FrmTrade_Add_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            dtp_BeginDate.Value = DateTime.Parse(Todate).AddMonths(-1);
            dtp_EndDate.Value = DateTime.Parse(Todate).AddDays(-1);
            dtp_RequestDate.Value = DateTime.Now;

            _InitCmb();
            ClientLoad();
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select_SG(baseDataSet.Drivers);
        }
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

        }

        private void ClientLoad()
        {


            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT AllowTax FROM Clients";

                    _Command.CommandText += Environment.NewLine + "WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);




                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {

                        while (_Reader.Read())
                        {
                            AllowTaxBool = _Reader.GetBooleanZ(0);
                        }


                    }
                }
                _Connection.Close();
            }

            if (AllowTaxBool == false)
            {

                rdb_Tax0.Checked = true;
            }
            else
            {
                rdb_Tax1.Checked = true;

            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "역발행", 1), "역발행 추가 성공");
                string Todate = DateTime.Now.ToString("yyyy/MM/01");
                dtp_BeginDate.Value = DateTime.Parse(Todate).AddMonths(-1);
                dtp_EndDate.Value = DateTime.Parse(Todate).AddDays(-1);
                txt_Item.Text = "";
                dtp_RequestDate.Value = DateTime.Now;
                txt_Price.Text = "";
                txt_VAT.Text = "";
                if (AllowTaxBool == false)
                {
                    rdb_Tax0.Checked = true;
                }
                else
                {
                    rdb_Tax0.Checked = false;
                }
                txt_Amount.Text = "";
            }
            txt_Item.Focus();
        }

        private int _UpdateDB()
        {
            err.Clear();

            if (txt_Item.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }


            if (txt_Price.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }


            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Item") iName = "청구항목";
                if (code == "Price") iName = "청구금액";
                if (code == "PayBankName") iName = "은행명";
                if (code == "PayAccountNo") iName = "계좌번호";
                if (code == "PayInputName") iName = "예금주";

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        public CMDataSet.TradesRow CurrentCode = null;
        private void _AddClient()
        {
            try
            {
                if (driversBindingSource.Current == null)
                    return;
                var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
                if (Selected == null)
                    return;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand _DriverCommand = cn.CreateCommand();
                    _DriverCommand.CommandText = "SELECT PayBankName, PayBankCode, PayAccountNo, PayInputName FROM Drivers WHERE DriverId = @DriverId";
                    _DriverCommand.Parameters.AddWithValue("@DriverId", Selected.DriverId);
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
                        "INSERT Trades (RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, UseTax, HasAcc, AllowAcc, SubClientId, ClientUserId)Values(@RequestDate, @BeginDate, @EndDate, @DriverName, @Item, @Price, @VAT, @Amount, @PayState, @PayDate, @PayBankName, @PayBankCode, @PayAccountNo, @PayInputName, @DriverId, @ClientId, @UseTax, 1, @AllowAcc, @SubClientId, @ClientUserId)";
                    cmd.Parameters.AddWithValue("@RequestDate", dtp_RequestDate.Value);
                    cmd.Parameters.AddWithValue("@BeginDate", dtp_BeginDate.Value);
                    cmd.Parameters.AddWithValue("@EndDate", dtp_EndDate.Value);
                    cmd.Parameters.AddWithValue("@DriverName", label83.Text);
                    cmd.Parameters.AddWithValue("@Item", txt_Item.Text);
                    cmd.Parameters.AddWithValue("@Price", txt_Price.Text.Replace(",", ""));
                    cmd.Parameters.AddWithValue("@VAT", txt_VAT.Text.Replace(",", ""));
                    cmd.Parameters.AddWithValue("@Amount", txt_Amount.Text.Replace(",", ""));
                    cmd.Parameters.AddWithValue("@PayState", 2);
                    cmd.Parameters.AddWithValue("@PayDate", dtp_RequestDate.Value);
                    cmd.Parameters.AddWithValue("@PayBankName", PayBankName);


                    cmd.Parameters.AddWithValue("@PayBankCode", PayBankCode);
                    cmd.Parameters.AddWithValue("@PayAccountNo", PayAccountNo);
                    cmd.Parameters.AddWithValue("@PayInputName", PayInputName);


                    cmd.Parameters.AddWithValue("@DriverId", txt_DriverId.Text);
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    cmd.Parameters.AddWithValue("@AllowAcc", true);
                    cmd.Parameters.AddWithValue("@SubClientId", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClientUserId", DBNull.Value);
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        cmd.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            cmd.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                        }
                    }
                    cmd.Parameters.AddWithValue("@UseTax", true);

                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            catch
            {
                MessageBox.Show("세금계산서 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
        public bool IsSuccess = false;

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "세금계산서", 1), "세금계산서 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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

        private void btn_InfoSearch_Click(object sender, EventArgs e)
        {
            dataGridView2.AutoGenerateColumns = false;
            driversBindingSource.Filter = $"Name LIKE '%{txt_InfoSearch.Text}%' OR CarNo LIKE '%{txt_InfoSearch.Text}%' OR CarYear LIKE '%{txt_InfoSearch.Text}%'";
            dataGridView2.ClearSelection();
        }

        private void btn_InfoSearch_Clear_Click(object sender, EventArgs e)
        {
            txt_InfoSearch.Text = "";
            btn_InfoSearch_Click(null, null);
        }

        private void txt_InfoSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_InfoSearch_Click(null, null);
        }

        private void btnExcelDown_Click(object sender, EventArgs e)
        {
            string fileString = "지입차일괄역발행양식_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.지입차일괄역발행양식;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.TRADE);

            if (di.Exists == false)
            {
                di.Create();
            }
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            try
            {
                if (file.Exists)
                {
                    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    file.Delete();
                }
                File.WriteAllBytes(FileName, ieExcel);
            }
            catch
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 이름의 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            System.Diagnostics.Process.Start(FileName);
        }

        private void btnExcelInsert_Click(object sender, EventArgs e)
        {

            EXCELINSERT_Trade _Form = new EXCELINSERT_Trade();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void txt_Price_Enter(object sender, EventArgs e)
        {
            txt_Price.Text = txt_Price.Text.Replace(",", "");
        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            txt_Price.Text = "";
            txt_VAT.Text = "";
            txt_Amount.Text = "";
            if (rdb_Tax0.Checked)
            {
                txt_Amount.ReadOnly = true;
                txt_Price.ReadOnly = false;
                lbl_Price.ForeColor = Color.Blue;
                lbl_Amt.ForeColor = Color.Black;
                AllowTaxBool = false;
                txt_Price.Focus();
            }
            else if(rdb_Tax1.Checked)
            {
                txt_Amount.ReadOnly = false;
                txt_Price.ReadOnly = true;
                lbl_Price.ForeColor = Color.Black;
                lbl_Amt.ForeColor = Color.Blue;
                AllowTaxBool = true;
                txt_Amount.Focus();
            }
            else if (rdb_Tax2.Checked)
            {
                txt_Amount.ReadOnly = true;
                txt_Price.ReadOnly = false;
                lbl_Price.ForeColor = Color.Blue;
                lbl_Amt.ForeColor = Color.Black;
                txt_VAT.Text = "0";
                AllowTaxBool = false;
                txt_Price.Focus();
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
        }

        private void txt_Amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Amount_Enter(object sender, EventArgs e)
        {
            txt_Amount.Text = txt_Amount.Text.Replace(",", "");
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            if (driversBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null)
                return;
            label83.Text = Selected.Name;
            txt_DriverId.Text = Selected.DriverId.ToString();
            txt_BizNo.Text = Selected.BizNo;
            txt_Ceo.Text = Selected.Name;
        }

        private void txt_BizNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
