using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
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

using mycalltruck.Admin.Class.Extensions;

namespace mycalltruck.Admin
{
    public partial class FrmTrade_Client_Add : Form
    {
        DESCrypt m_crypt = null;
        Int64 iPrice = 0;
        public FrmTrade_Client_Add()
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");
        }

       

        private void FrmTrade_Add_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.Clients' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.clientsTableAdapter.Fill(this.cMDataSet.Clients);

            string Todate = DateTime.Now.ToString("yyyy/MM/01");
         //   this.driversInfoTableAdapter.Fill(this.cMDataSet.DriversInfo, LocalUser.Instance.LogInInfomation.ClientId);
            this.driversTableAdapter.Fill(this.cMDataSet.Drivers);
            this.trades1TableAdapter.Fill(this.cMDataSet.Trades1);
            //dtp_BeginDate.Value = DateTime.Parse(Todate).AddMonths(-1);
            //dtp_EndDate.Value = DateTime.Parse(Todate).AddDays(-1);
            dtp_RequestDate.Value = DateTime.Now;
            
            _InitCmb();

            btn_InfoSearch_Click(null, null);

            cmb_HasAcc.SelectedIndex = 1;

        }
        private void _InitCmb()
        {



            //var BankSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BankGubun").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_PayBankName.DataSource = BankSourceDataSource;
            //cmb_PayBankName.DisplayMember = "Name";
            //cmb_PayBankName.ValueMember = "Value";

            Dictionary<string, string> PayBank = new Dictionary<string, string>();

         


            //PayBank.Add(" ", "은행선택");
            //PayBank.Add("003", "기업은행");
            //PayBank.Add("004", "국민은행");
            //PayBank.Add("011", "농협");
            //PayBank.Add("012", "단위농협");
            //PayBank.Add("020", "우리은행");
            //PayBank.Add("031", "대구은행");
            //PayBank.Add("005", "외한은행");
            //PayBank.Add("023", "SC제일은행");
            //PayBank.Add("032", "부산은행");
            //PayBank.Add("045", "새마을");
            //PayBank.Add("027", "한국시티은행");
            //PayBank.Add("034", "광주은행");
            //PayBank.Add("039", "경남은행");
            //PayBank.Add("007", "수협");
            //PayBank.Add("048", "신협");
            //PayBank.Add("037", "전북은행");
            //PayBank.Add("035", "제주은행");
            //PayBank.Add("064", "산림조합");
            //PayBank.Add("071", "우체국");
            //PayBank.Add("081", "하나은행");
            //PayBank.Add("088", "신한은행");
            //PayBank.Add("209", "동양종금증권");
            //PayBank.Add("243", "한국투자증권");
            //PayBank.Add("240", "삼성증권");
            //PayBank.Add("230", "미래에셋");
            //PayBank.Add("247", "우리투자증권");
            //PayBank.Add("218", "현대증권");
            //PayBank.Add("266", "SK증권");
            //PayBank.Add("278", "신한금융투자");
            //PayBank.Add("262", "하이증권");
            //PayBank.Add("263", "HMC증권");
            //PayBank.Add("267", "대신증권");
            //PayBank.Add("270", "하나대투증권");       
            //PayBank.Add("279", "동부증권");
            //PayBank.Add("280", "유진증권");
            //PayBank.Add("287", "메리츠증권");
            //PayBank.Add("291", "신영증권");
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

            cmb_PayBankName.DataSource = new BindingSource(PayBank, null);
            cmb_PayBankName.DisplayMember = "Value";
            cmb_PayBankName.ValueMember = "Key";

            Dictionary<bool, string> HasACCList = new Dictionary<bool, string>();

            HasACCList.Add(false, "현금");
            HasACCList.Add(true, "카드");
            cmb_HasAcc.DataSource = new BindingSource(HasACCList, null);
            cmb_HasAcc.DisplayMember = "Value";
            cmb_HasAcc.ValueMember = "Key";
         

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {





                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "역발행", 1), "역발행 추가 성공");


                string Todate = DateTime.Now.ToString("yyyy/MM/01");
               
                txt_Item.Text = "";

                dtp_RequestDate.Value = DateTime.Now;

                
                txt_Price.Text = "";
                txt_VAT.Text = "";
                chk_Vat.Checked = false;
                txt_Amount.Text = "";
                cmb_PayBankName.SelectedIndex = 0;
                txt_PayAccountNo.Text = "";

                txt_PayInputName.Text = "";
                cmb_HasAcc.SelectedIndex = 1;



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

            }


            if (txt_Price.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }

            if(cmb_PayBankName.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_PayBankName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }


            if (txt_PayAccountNo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PayAccountNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }


            if (txt_PayInputName.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PayInputName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
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

                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        //"INSERT Trades (RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, UseTax )Values(@RequestDate, @BeginDate, @EndDate, @DriverName, @Item, @Price, @VAT, @Amount, @PayState, @PayDate, @PayBankName, @PayBankCode, @PayAccountNo, @PayInputName, @DriverId, @ClientId, @UseTax)";

                     "INSERT INTO SalesManage " +
 " ( RequestDate, SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat, UseTax, Price, Amount, CreateDate" +
                    ",HasACC, CardPayGubun,CustomerId,ClientId,PayAccountNo,PayBankCode,PayBankName,PayInputName,SetYN,PayState)" +
                     " VALUES( @RequestDate, @SangHo, @BizNo, @Ceo, @Uptae, @Upjong, @AddressState, @AddressCity, @AddressDetail, @Email, @ContRactName, @MobileNo, @Item, @UnitPrice, @Num, @Vat, @UseTax" +
                    " , @Price, @Amount, @CreateDate,@HasACC, @CardPayGubun,@CustomerId,@ClientId,@PayAccountNo,@PayBankCode,@PayBankName,@PayInputName,@SetYN,@PayState)";

                    var Query = cMDataSet.Clients.Where(c=> c.ClientId ==int.Parse(txt_CustomerId.Text)).ToArray();

                    cmd.Parameters.AddWithValue("@RequestDate", dtp_RequestDate.Value);
                    //cmd.Parameters.AddWithValue("@BeginDate", dtp_BeginDate.Value);
                    //cmd.Parameters.AddWithValue("@EndDate", dtp_EndDate.Value);
                    cmd.Parameters.AddWithValue("@SangHo",txt_SangHo.Text);
                    cmd.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                    cmd.Parameters.AddWithValue("@Ceo", txt_CEO.Text);
                    cmd.Parameters.AddWithValue("@Uptae", Query.First().Uptae);
                    cmd.Parameters.AddWithValue("@Upjong", Query.First().Upjong);
                    cmd.Parameters.AddWithValue("@AddressState", Query.First().AddressState);
                    cmd.Parameters.AddWithValue("@AddressCity", Query.First().AddressCity);
                    cmd.Parameters.AddWithValue("@AddressDetail", Query.First().AddressDetail);
                    cmd.Parameters.AddWithValue("@Email", Query.First().Email);
                    cmd.Parameters.AddWithValue("@ContRactName", Query.First().CEO);
                    cmd.Parameters.AddWithValue("@MobileNo", Query.First().MobileNo);
                    cmd.Parameters.AddWithValue("@Item", txt_Item.Text);
                    cmd.Parameters.AddWithValue("@UnitPrice", txt_Price.Text);
                    cmd.Parameters.AddWithValue("@Num", 1);
                    cmd.Parameters.AddWithValue("@Vat", txt_VAT.Text);
                    if (chk_Vat.Checked == true)
                    {
                        cmd.Parameters.AddWithValue("@UseTax", true);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@UseTax", false);
                    }
                    cmd.Parameters.AddWithValue("@Price", txt_Price.Text);

                    cmd.Parameters.AddWithValue("@Amount", txt_Amount.Text);
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);

                    if (cmb_HasAcc.SelectedIndex == 0)
                    {
                        cmd.Parameters.AddWithValue("@HasACC", false);
                        cmd.Parameters.AddWithValue("@CardPayGubun", "N");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@HasACC", true);
                        cmd.Parameters.AddWithValue("@CardPayGubun", "C");
                    }

                    //cmd.Parameters.AddWithValue("@CustomerId", txt_CustomerId.Text);
                    //cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInfomation.ClientId);

                    cmd.Parameters.AddWithValue("@CustomerId", LocalUser.Instance.LogInInformation.ClientId);
                    cmd.Parameters.AddWithValue("@ClientId", txt_CustomerId.Text);


                    cmd.Parameters.AddWithValue("@PayAccountNo", txt_PayAccountNo.Text);
                    cmd.Parameters.AddWithValue("@PayBankCode", cmb_PayBankName.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PayBankName", cmb_PayBankName.Text);
                    cmd.Parameters.AddWithValue("@PayInputName", txt_PayInputName.Text);
                    cmd.Parameters.AddWithValue("@SetYN", 0);

                    cmd.Parameters.AddWithValue("@PayState", 2);
                    
                 

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
            if (!string.IsNullOrEmpty(txt_Price.Text))
            {

                if (chk_Vat.Checked == true)
                {
                    txt_VAT.Text = Math.Truncate((Int64.Parse(txt_Price.Text) * 0.1)).ToString();
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }
                else
                {
                    txt_VAT.Text = "0";
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }
              

            }
        }

        private void chk_Vat_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Price.Text))
            {

                if (chk_Vat.Checked == true)
                {
                    txt_VAT.Text = Math.Truncate((Int64.Parse(txt_Price.Text) * 0.1)).ToString();
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }
                else
                {
                    txt_VAT.Text = "0";
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }


            }
        }

      

        private void btn_InfoSearch_Click(object sender, EventArgs e)
        {
            this.clientsTableAdapter.Fill(this.cMDataSet.Clients);
          
            
            try
            {
                if (String.IsNullOrEmpty(txt_InfoSearch.Text))
                {

                    clientsBindingSource.Filter = string.Format("PAYLOGISYN = 1 AND ClientId <> {0}", LocalUser.Instance.LogInInformation.ClientId);
                }
                else
                {

                    clientsBindingSource.Filter = string.Format("(Name Like  '%{0}%' or BizNo Like  '%{0}%' or Ceo Like  '%{0}%') AND PAYLOGISYN = 1 AND ClientId <> {1}", txt_InfoSearch.Text,LocalUser.Instance.LogInInformation.ClientId);

                }

            }
            catch
            {
            }
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
            string title = string.Empty;
            byte[] ieExcel;

            string fileString = string.Empty;


            fileString = "협력업체일괄역발행양식_" + DateTime.Now.ToString("yyyyMMdd");
            title = "sheet1";
            ieExcel = Properties.Resources.협력업체일괄역발행양식;


            pnProgress.Visible = true;
            bar.Value = 0;
            Thread t = new Thread(new ThreadStart(() =>
            {

                newDGV1.ExportExistExcel2(title, fileString, bar, true, ieExcel, 3, LocalUser.Instance.PersonalOption.TRADE);
                pnProgress.Invoke(new Action(() => pnProgress.Visible = false));





            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void btnExcelInsert_Click(object sender, EventArgs e)
        {

            EXCELINSERT_Trade_Client _Form = new EXCELINSERT_Trade_Client();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );


         
        }

        private void clientsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (clientsBindingSource.Current == null) return;



            var Selected = ((DataRowView)clientsBindingSource.Current).Row as CMDataSet.ClientsRow;


            if (Selected != null)
            {


                lbl_CustomerName.Text = Selected.Name;
                txt_CustomerId.Text = Selected.ClientId.ToString();
                txt_BizNo.Text = Selected.BizNo;
                txt_SangHo.Text = Selected.Name;

                txt_CEO.Text = Selected.CEO;

               // txt_PayAccountNo.Text = Selected.CMSAccountNo;
                txt_PayInputName.Text = Selected.CMSOwner;
                cmb_PayBankName.SelectedValue = Selected.CMSBankCode;

                string tempString = string.Empty;
                tempString = Selected.CMSAccountNo;
                try
                {
                    byte[] data = Convert.FromBase64String(tempString);


                    txt_PayAccountNo.Text = m_crypt.Decrypt(tempString).Replace("\0", "");
                }
                catch
                {
                    txt_PayAccountNo.Text = tempString;
                }


            }
        }
    }
}
