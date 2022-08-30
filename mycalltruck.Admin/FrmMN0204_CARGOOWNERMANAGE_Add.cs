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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Net;
using mycalltruck.Admin.Class;

namespace mycalltruck.Admin
{
    public partial class FrmMN0204_CARGOOWNERMANAGE_Add : Form
    {
        DESCrypt m_crypt = null;
        String Sgubun = "";

        public FrmMN0204_CARGOOWNERMANAGE_Add()
        {
            InitializeComponent();
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            m_crypt = new DESCrypt("12345678");
        }
        private void FrmMN0204_CARGOOWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            this.clientsTableAdapter.Fill(cmDataSet.Clients);
            var dealersTableAdapter = new CMDataSetTableAdapters.DealersTableAdapter();
            dealersTableAdapter.Fill(cmDataSet.Dealers);
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            _InitCmb();
            ClientsCode_Add();
            txt_Name.Focus();
            cmb_BizType.SelectedIndex = 2;
            cmb_status.SelectedIndex = 0;
            dtp_ServiceDate.Value = DateTime.Now;
            cmb_YN.SelectedIndex = 0;
            cmb_PG.SelectedIndex = 1;
        }

        private void ClientsCode_Add()
        {
            this.clientsTableAdapter.Fill(cmDataSet.Clients);
            var Client_code = cmDataSet.Clients.Where(c => c.Code != "999999").Select(c => new { c.Code }).OrderByDescending(c => c.Code).ToArray();
            if (Client_code.Count() > 0)
            {
                var ClientCode = 100001;
                var ClientCodeCandidate = cmDataSet.Clients.Where(c => c.Code != "999999").OrderBy(c => c.Code).Select(c => c.Code).ToArray();
                while (true)
                {
                    if (!ClientCodeCandidate.Any(c => c == ClientCode.ToString()))
                    {
                        break;
                    }
                    ClientCode++;
                }
                txt_Code.Text = ClientCode.ToString();
            }
            else
            {
                txt_Code.Text = "100001";
            }
        }

        private void SaveClear()
        {
            ClientsCode_Add();
            txt_Name.Text = "";
            txt_BizNo.Text = "";
            txt_CEO.Text = "";
            txt_Uptae.Text = "";
            txt_Upjong.Text = "";
            txt_Email.Text = "";
            txt_LoginId.Text = "";
            txt_Password.Text = "";
            cmb_Admin.SelectedIndex = 0;
            txt_MobileNo.Text = "";
            txt_PhoneNo.Text = "";
            txt_FaxNo.Text = "";
            //cmb_AddressCity.SelectedIndex = 0;
            //cmb_AddressState.SelectedIndex = 0;
            //txt_Addr.Text = "";
            txt_Zip.Text = "";
            txt_State.Text = "";
            txt_City.Text = "";
            txt_Street.Text = "";

            cmb_BizType.SelectedIndex = 2;
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            cmb_Bank.SelectedIndex = 0;
            txt_AccountNo.Text = "";
            txt_AccountOwner.Text = "";
            cmb_status.SelectedIndex = 0;
            dtp_ServiceDate.Value = DateTime.Now;
            cmb_PG.SelectedIndex = 1;
            cmb_YN.SelectedIndex = 0;
            txt_CEOBirth.Text = "";
            txt_Code.Focus();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Sgubun = "Add";
            _UpdateDB();
        }

        private int _UpdateDB()
        {
            this.clientsTableAdapter.Fill(cmDataSet.Clients);

            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
            else if (cmDataSet.Clients.Where(c => c.Code == txt_Code.Text).Count() > 0)
            {

                MessageBox.Show("코드가 중복되었습니다.!!", "코드 입력 오류");
                err.SetError(txt_Code, "코드가 중복되었습니다.!!");
                return -1;

            }

            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }
            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                return -1;
            }

            else if (cmDataSet.Clients.Where(c => c.BizNo == txt_BizNo.Text).Count() > 0)
            {

                MessageBox.Show("사업자 번호가 중복되었습니다.!!", "사업자 번호 입력 오류");
                err.SetError(txt_BizNo, "사업자 번호가 중복되었습니다.!!");
                return -1;

            }

            if (txt_CEO.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_Uptae.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_Upjong.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (string.IsNullOrWhiteSpace(txt_Zip.Text) || string.IsNullOrWhiteSpace(txt_Street.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Street, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

               
            }
            
          
            

            if (txt_LoginId.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_LoginId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            else
            {
                //var Query1 = "Select Count(*) From ClientUsers Where LoginId = @LoginId";
                //var Query2 =
                //    "Select Count(*) From Clients Where LoginId = @LoginId";
                var Query1 = "SELECT COUNT(*) FROM " +
                 "(Select LoginId From ClientUsers " +
                 " union " +
                 " Select LoginId From clients " +
                 " union " +
                 " Select LoginId From Customers " +
                 " union " +
                 " Select LoginId From CustomerAddPhone) as a" +
                 " Where LoginId = @LoginId ";


                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }
                    //if (!IsDuplicated)
                    //{
                    //    SqlCommand cmd2 = cn.CreateCommand();
                    //    cmd2.CommandText = Query2;
                    //    cmd2.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    //    if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
                    //    {
                    //        IsDuplicated = true;
                    //    }
                    //}
                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show("아이디가 중복되었습니다.!!", "아이디 입력 오류");
                    err.SetError(txt_LoginId, "아이디가 중복되었습니다.!!");
                    txt_LoginId.Clear();
                    txt_LoginId.Focus();
                    return -1;
                }


            }

            if (txt_Password.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (txt_PhoneNo.Text.Replace(" ","").Replace("-","") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

           


            //if (cmb_PG.SelectedIndex == 1)
            //{

            //    if (String.IsNullOrEmpty(txt_LGD_MID.Text.Trim()))
            //    {
            //        MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //        err.SetError(txt_LGD_MID, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //        return -1;
            //    }
            //}


            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Code") iName = "코드";
                if (code == "Name") iName = "상호";
                if (code == "BizNo") iName = "사업자 번호";
                if (code == "CEO") iName = "대표자명";
                if (code == "Addr") iName = "주소";
                if (code == "Uptae") iName = "업태";
                if (code == "Upjong") iName = "업종";
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }

        private void MWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
            }
            else if (e.Error != null)
            {
            }
            else
            {
                if (e.Result != null && e.Result.ToLower() == "true")
                {
                     _AddClient();
                }
                else
                {
                    MessageBox.Show("계좌인증 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }


        public CMDataSet.ClientsRow CurrentCode = null;
        private void _AddClient()
        {
            CMDataSet.ClientsRow row = cmDataSet.Clients.NewClientsRow();
            CurrentCode = row;

            row.Code = txt_Code.Text;
            row.Name = txt_Name.Text;
            row.BizNo = txt_BizNo.Text;
            row.CEO = txt_CEO.Text;
            row.Uptae = txt_Uptae.Text;
            row.Upjong = txt_Upjong.Text;

            row.AddressState = txt_State.Text;
            row.AddressCity = txt_City.Text;
            row.AddressDetail = txt_Street.Text;
            row.ZipCode = txt_Zip.Text;

            row.LoginId = txt_LoginId.Text;
            row.Password = txt_Password.Text;
            row.ShopID = txt_LoginId.Text;
            row.ShopPW = txt_Password.Text;

            if (cmb_Admin.SelectedValue != null)
                row.DealerId = int.Parse(cmb_Admin.SelectedValue.ToString());
            row.Admin = cmb_Admin.Text;
            row.MobileNo = txt_MobileNo.Text;
            row.Email = txt_Email.Text;
            if (txt_MobileNo.Text.Length < 12)
            {
                row.MobileNo = "";
            }
            else
            {
                row.MobileNo = txt_MobileNo.Text;
            }

            if (txt_PhoneNo.Text.Length < 8)
            {
                row.PhoneNo = "";
            }
            else
            {
                row.PhoneNo = txt_PhoneNo.Text;
            }
            if (txt_FaxNo.Text.Length < 11)
            {
                row.FaxNo = "";
            }
            else
            {
                row.FaxNo = txt_FaxNo.Text;
            }

            row.CreateDate = DateTime.Now;
            row.BizType = int.Parse(cmb_BizType.SelectedValue.ToString());
            row.RouteType = 0;
            row.InsuranceType = 0;
            row.CarType = 0;
            row.CarSize = 0;
            row.CarState = 0;
            row.CarCity = 0;

            row.Status = int.Parse(cmb_status.SelectedValue.ToString());
            row.ServiceDate = dtp_ServiceDate.Text;
            if (cmb_Bank.SelectedValue.ToString() != "0")
            {
                row.CMSBankName = cmb_Bank.Text;
                row.CMSBankCode = cmb_Bank.SelectedValue.ToString();
            }

            string tempString = string.Empty;
            //DES 암호화
            tempString = m_crypt.Encrypt(txt_AccountNo.Text);
            row.CMSAccountNo = tempString;
            row.CMSOwner = txt_AccountOwner.Text;

            //if (cmb_PG.SelectedIndex == 1)
            //{
            //    row.LGD_MID = txt_LGD_MID.Text.Trim();
            //}
            if(String.IsNullOrWhiteSpace(txt_LGD_MID.Text))
            {
                row.LGD_MID = "";
            }
            row.PGGubun=  int.Parse(cmb_PG.SelectedValue.ToString());
            row.PAYLOGISYN = int.Parse(cmb_YN.SelectedValue.ToString());
            row.CEOBirth = txt_CEOBirth.Text;
            row.ExcelType = 3;
            row.ContType = 1;
            row.DriverType = 1;
            row.OrderType = 1;
            row.NoticeDriver = 2;
            row.NoticeCnt = 0;
            row.X = 0;
            row.Y = 0;
            row.AllowFPIS = false;
            row.AllowFPIS_In = false;
            row.AllowOrder = false;
            row.AllowSMS = false;
            row.AllowTax = false;
            row.AllowSub = false;
            cmDataSet.Clients.AddClientsRow(row);
            try
            {

                clientsTableAdapter.Update(row);

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "운송사", 1), "운송사 정보 추가 성공");

                this.clientsTableAdapter.Fill(cmDataSet.Clients);
                object ClientAccId = 0;
                int ClientId = cmDataSet.Clients.Where(c => c.BizNo == txt_BizNo.Text).First().ClientId;

                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "Insert Into ClientAccs (ClientId, AccCheck, AccBank, AccLimite, AccCurrent, RegDate) output INSERTED.ClientAccId Values (@ClientId, @AccCheck, @AccBank, @AccLimite, @AccCurrent, @RegDate)";
                    Command.Parameters.AddWithValue("@ClientId", ClientId);
                    Command.Parameters.AddWithValue("@AccCheck", "체크 안함");
                    Command.Parameters.AddWithValue("@AccBank", "우리카드");
                    Command.Parameters.AddWithValue("@AccLimite", 0);
                    Command.Parameters.AddWithValue("@AccCurrent", 0);
                    Command.Parameters.AddWithValue("@RegDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    Connection.Open();
                    ClientAccId = Command.ExecuteScalar();
                    Connection.Close();
                }

                if (Sgubun == "Add")
                {
                    SaveClear();
                }
                else
                {
                    Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("화주 정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
      
        public bool IsSuccess = false;
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            Sgubun = "Close";
            _UpdateDB();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _InitCmb()
        {
            var BizTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ClientType"  && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_BizType.DataSource = BizTypeDataSource;
            cmb_BizType.DisplayMember = "Name";
            cmb_BizType.ValueMember = "value";

            var PGgubunDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "PGgubun" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_PG.DataSource = PGgubunDataSource;
            cmb_PG.DisplayMember = "Name";
            cmb_PG.ValueMember = "value";

            //var CarstateDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.State }).Distinct().ToArray();
            //cmb_AddressState.DataSource = CarstateDataSource;
            //cmb_AddressState.DisplayMember = "State";
            //cmb_AddressState.ValueMember = "State";


            //var CarCityDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.City }).Distinct().ToArray();
            //cmb_AddressCity.DataSource = CarCityDataSource;
            //cmb_AddressCity.DisplayMember = "City";
            //cmb_AddressCity.ValueMember = "City";

            var DealersDataSource = cmDataSet.Dealers.Where(c => c.Status == 2).Select(c => new { c.Name, c.Code, c.DealerId }).OrderBy(c => c.DealerId).ToArray();
            cmb_Admin.DataSource = DealersDataSource;
            cmb_Admin.DisplayMember = "Name";
            cmb_Admin.ValueMember = "DealerId";

            cmb_Admin.SelectedIndex = 1;


          
            //cmb_Admin.DisplayMember = "Text";
            //cmb_Admin.ValueMember = "Value";
            //cmb_Admin.DataSource = Filter.Dealer.DealerList;
            //cmb_Admin.SelectedIndex = 1;

            var SatusDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_status.DataSource = SatusDataSource;
            cmb_status.DisplayMember = "Name";
            cmb_status.ValueMember = "value";

            var PaylogisYNDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "PaylogisYN" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_YN.DataSource = PaylogisYNDataSource;
            cmb_YN.DisplayMember = "Name";
            cmb_YN.ValueMember = "value";

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

            cmb_Bank.DataSource = new BindingSource(PayBank, null);
            cmb_Bank.DisplayMember = "Value";
            cmb_Bank.ValueMember = "Key";
        }
        private void cmb_AddressState_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //cmb_AddressCity.Enabled = true;
            //cmb_AddressCity.DataSource = null;
            //var CarCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            //cmb_AddressCity.DataSource = CarCityDataSource;

            //cmb_AddressCity.DisplayMember = "City";
            //cmb_AddressCity.ValueMember = "City";
        }

        private void FrmMN0204_CARGOOWNERMANAGE_Add_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                btnAdd_Click(null, null);
            else if (e.KeyCode == Keys.F6)
                btnAddClose_Click(null, null);
        }


        private void cmb_PG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_PG.SelectedIndex == 1)
            {
                txt_LGD_MID.ReadOnly = false;
            }
            else
            {
                txt_LGD_MID.ReadOnly = true;
                txt_LGD_MID.Text = "";
            }
        }

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                txt_Zip.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_State.Text = ss[0];
                txt_City.Text = ss[1];
                txt_Street.Text = String.Join(" ", ss.Skip(2));
            }
        }
    }
}
