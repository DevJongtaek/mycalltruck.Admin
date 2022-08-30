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
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets.ClientDataSetTableAdapters;

using static mycalltruck.Admin.DataSets.ClientDataSet;
using System.Web;

namespace mycalltruck.Admin
{
    public partial class FrmMNRegister : Form
    {
        DESCrypt m_crypt = null;
        String Sgubun = "";

        public FrmMNRegister()
        {
            InitializeComponent();
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            m_crypt = new DESCrypt("12345678");
        }
        private void FrmMN0204_CARGOOWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            this.clientsTableAdapter.Fill(clientDataSet.Clients);
            var dealersTableAdapter = new DealersTableAdapter();
            dealersTableAdapter.Fill(clientDataSet.Dealers);
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            _InitCmb();
            ClientsCode_Add();
          //  InitCustomerTable();
            txt_Name.Focus();
            //cmb_BizType.SelectedIndex = 2;
            cmb_status.SelectedIndex = 0;
            //dtp_ServiceDate.Value = DateTime.Now;
           // cmb_YN.SelectedIndex = 0;
            //cmb_PG.SelectedIndex = 1;
        }

        private void ClientsCode_Add()
        {
            this.clientsTableAdapter.Fill(clientDataSet.Clients);
            var Client_code = clientDataSet.Clients.Where(c => c.Code != "999999").Select(c => new { c.Code }).OrderByDescending(c => c.Code).ToArray();
            if (Client_code.Count() > 0)
            {
                var ClientCode = 100001;
                var ClientCodeCandidate = clientDataSet.Clients.Where(c => c.Code != "999999").OrderBy(c => c.Code).Select(c => c.Code).ToArray();
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
        class CustomerViewModel
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string PhoneNo { get; set; }
            public string Code { get; set; }
            public string BizNo { get; set; }
            // public int CClientId { get; set; }
        }
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>();
        private void InitCustomerTable()
        {
            _CustomerViewModelList.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo,Customers.Code,Customers.BizNo FROM Customers" +
                        " WHERE ClientId = @ClientId  Order by Code DESC";
                    commnad.Parameters.AddWithValue("@ClientId", txt_ClientId.Text);
                    var dataReader = commnad.ExecuteReader();
                    while (dataReader.Read())
                    {
                        _CustomerViewModelList.Add(
                          new CustomerViewModel
                          {
                              CustomerId = dataReader.GetInt32(0),
                              Name = dataReader.GetStringN(1),
                              PhoneNo = dataReader.GetStringN(2),
                              Code = dataReader.GetStringN(3),
                              BizNo = dataReader.GetStringN(4),
                              // CClientId = dataReader.GetInt32(4),
                          });
                    }
                }
                connection.Close();
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
           // txt_FaxNo.Text = "";
            //cmb_AddressCity.SelectedIndex = 0;
            //cmb_AddressState.SelectedIndex = 0;
            //txt_Addr.Text = "";
            txt_Zip.Text = "";
            txt_State.Text = "";
            txt_City.Text = "";
            txt_Street.Text = "";

           // cmb_BizType.SelectedIndex = 2;
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
           // cmb_Bank.SelectedIndex = 0;
            //txt_AccountNo.Text = "";
            //txt_AccountOwner.Text = "";
            cmb_status.SelectedIndex = 0;
           // dtp_ServiceDate.Value = DateTime.Now;
            //cmb_PG.SelectedIndex = 1;
            //cmb_YN.SelectedIndex = 0;
           
            txt_Code.Focus();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Sgubun = "Add";
            if(txt_Gubun.Text == "0")
            {
                _UpdateDB();
            }
            else if (txt_Gubun.Text == "1")
            {

                if (txt_BizNo.Text.Length != 12)
                {
                    MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                    err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                    return ;
                }
                
                if (txt_Name.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;

                }
                if (txt_CEO.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (txt_Uptae.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (txt_Upjong.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (string.IsNullOrWhiteSpace(txt_Zip.Text) || string.IsNullOrWhiteSpace(txt_Street.Text))
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Street, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;


                }
                if (txt_Password.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }

                if (txt_PhoneNo.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (txt_LoginId.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_LoginId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                else
                {

                    //var Query1 = "Select Count(*) From ClientUsers Where LoginId = @LoginId And CliendId != @ClientId";
                    //var Query2 =
                    //    "Select Count(*) From Clients Where LoginId = @LoginId And CliendId != @ClientId";
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
                        cmd1.Parameters.AddWithValue("@ClientId", txt_ClientId.Text);
                        if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                        {
                            IsDuplicated = true;
                        }
                        //if (!IsDuplicated)
                        //{
                        //    SqlCommand cmd2 = cn.CreateCommand();
                        //    cmd2.CommandText = Query2;
                        //    cmd2.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                        //    cmd2.Parameters.AddWithValue("@ClientId", txt_ClientId.Text);
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
                        return ;
                    }
                }



                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "UPDATE Clients SET Name = @Name " +
                        ", CEO = @CEO" +
                        ", AddressState = @AddressState" +
                        ", AddressCity = @AddressCity" +
                        ", AddressDetail = @AddressDetail" +
                        ", ZipCode = @Zipcode" +
                        ", LoginId  = @LoginId" +
                        ", Password = @Password" +
                        ", MobileNo = @MobileNo" +
                        ", DealerId = @DealerId "+
                        ", Admin = @Admin " +
                        " WHERE Code = @Code";
                    _Command.Parameters.AddWithValue("@Code", txt_Code.Text);
                    _Command.Parameters.AddWithValue("@Name", txt_Name.Text);
                    _Command.Parameters.AddWithValue("@AddressState", txt_State);
                    _Command.Parameters.AddWithValue("@AddressCity", txt_City.Text);
                    _Command.Parameters.AddWithValue("@AddressDetail", txt_Street.Text);
                    _Command.Parameters.AddWithValue("@ZipCode", txt_Zip.Text);

                    _Command.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    _Command.Parameters.AddWithValue("@Password", txt_Password.Text);
                    _Command.Parameters.AddWithValue("@MobileNo", txt_MobileNo.Text);
                    
                    _Command.Parameters.AddWithValue("@DealerId", int.Parse(cmb_Admin.SelectedValue.ToString()));
                    _Command.Parameters.AddWithValue("@Admin", cmb_Admin.Text);


                    _Command.ExecuteNonQuery();
                }
            }
           
        }

        private int _UpdateDB()
        {
            this.clientsTableAdapter.Fill(clientDataSet.Clients);

            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
            else if (clientDataSet.Clients.Where(c => c.Code == txt_Code.Text).Count() > 0)
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

            else if (clientDataSet.Clients.Where(c => c.BizNo == txt_BizNo.Text).Count() > 0)
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

            //if (cmbVBankName.SelectedIndex == 0)
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(cmbVBankName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;
            //}

            if (txt_MobileNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            else if(!Regex.IsMatch(txt_MobileNo.Text.Replace("-", ""), @"^01[0,1,6,7,8,9]\d{3,4}\d{4}"))
            {
                MessageBox.Show("핸드폰번호 형식!!", "핸드폰번호 형식 오류");
                err.SetError(txt_MobileNo, "핸드폰번호가 올바르지않습니다.!");
             
                return -1;
            }

            if (txt_Email.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Email, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            else if(!Regex.Match(txt_Email.Text, @"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$").Success)
            {
                MessageBox.Show("이메일 형식!!", "이메일 형식 오류");
                err.SetError(txt_Email, "이메일형식이 올바르지않습니다.!");

                return -1;
            }

            if (cmb_PayBankName.SelectedIndex < 1)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_PayBankName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (txt_PayAccountNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PayAccountNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_AccountOwner.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AccountOwner, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            string Account_Name = txt_AccountOwner.Text;
            string Mid = "edubill";
            string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_PayBankName.SelectedValue.ToString(), txt_PayAccountNo.Text, txt_AccountOwner.Text, cmb_PayBankName.Text, Mid);

            WebClient mWebClient = new WebClient();
            try
            {
                //                    var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                if (r == null || r.ToLower() != "true")
                {
                    var _Result2 = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));
                    _Result2 = HttpUtility.UrlDecode(_Result2);
                    if (_Result2 == "오류")
                    {
                        MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //Invoke(new Action(() => pnProgress.Visible = false));
                        //Invoke(new Action(() => MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                        return -1;
                    }
                    else
                    {
                        if (MessageBox.Show($"{cmb_PayBankName.Text} / {txt_PayAccountNo.Text} / {_Result2}\r\n위 계좌가 맞습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            return -1;
                        }
                        else
                        {
                            Account_Name = _Result2;

                            txt_AccountOwner.Text = Account_Name;
                        }




                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               
                return -1;
            }



            try
            {
                FrmLogin_C f = new FrmLogin_C(txt_Code.Text,txt_MobileNo.Text,txt_Name.Text);
                f.ShowDialog();
                if (f.Accepted)
                {
                    _AddClient();
                }
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

      


        public ClientDataSet.ClientsRow CurrentCode = null;
        private void _AddClient()
        {
          


            ClientDataSet.ClientsRow row = clientDataSet.Clients.NewClientsRow();
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

          
            row.FaxNo = "";
            row.CreateDate = DateTime.Now;
            row.BizType = 3;
            //row.RouteType = 0;
            //row.InsuranceType = 0;
            //row.CarType = 0;
            //row.CarSize = 0;
            //row.CarState = 0;
            //row.CarCity = 0;

            row.Status = 1;
            row.ServiceDate = DateTime.Now.ToString("yyyy-MM-dd");
           
            row.LGD_MID = "";

            //row.PGGubun = 2;
            //row.PAYLOGISYN = 1;
           // row.CEOBirth = txt_CEOBirth.Text;
            row.ExcelType = 3;
            row.ContType = 1;
            row.DriverType = 1;
            row.OrderType = 1;
            row.NoticeDriver = 2;
            row.NoticeCnt = 0;
            //row.X = 0;
            //row.Y = 0;
            row.AllowFPIS = false;
            row.AllowFPIS_In = false;
            row.AllowOrder = true;
            row.AllowSMS = false;
            row.AllowTax = false;
            row.AllowSub = false;

            row.HideAddTrade = false;
            row.HideAddSales = false;
            row.SignLocation = "LU";
            row.HideMyCarOrder = false;
            row.OrderGubun = 0;
            row.SmsYn = false;
            row.CMSBankCode = cmb_PayBankName.SelectedValue.ToString();
            row.CMSBankName = cmb_PayBankName.Text;

            row.CMSAccountNo = txt_PayAccountNo.Text;
            row.CMSOwner = txt_AccountOwner.Text;
            row.StatsGubun = 0;
            row.KakaoPriceGubun = 1;
            row.CargoApiYn = true;
            row.CarBankYn = false;
            clientDataSet.Clients.AddClientsRow(row);



            try
            {

                clientsTableAdapter.Update(row);

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "운송사", 1), "운송사 정보 추가 성공");

                this.clientsTableAdapter.Fill(clientDataSet.Clients);
                object ClientAccId = 0;
                int ClientId = clientDataSet.Clients.Where(c => c.BizNo == txt_BizNo.Text).First().ClientId;

                txt_ClientId.Text = ClientId.ToString();
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


                //웹회원가입정보가 있는지 없는지 확인
                var Query1 = "Select Count(*) From ClientManage Where ClientCode = @ClientCode ";
                
               
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@ClientCode", txt_Code.Text);

                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {

                        SqlCommand UpCommand = cn.CreateCommand();
                        UpCommand.CommandText = "Update ClientManage Set " +
                            " BizNo1 = @BizNo1" +
                            ", BIzNo2 =@BIzNo2" +
                            ", BIzNo3 =@BIzNo3" +
                            ", Name = @Name" +
                            ", CEO = @CEO" +
                            ", Addr1 = @Addr1" +
                            ", Addr2 = @Addr2" +
                            ", AddrDetail = @AddrDetail " +
                            ", ZipCode = @ZipCode " +
                            ", CMobileNo = @CMobileNo" +
                            ", ClientPhoneNo = @ClientPhoneNo" +
                            ", CPassword = @CPassword"+
                            ", CLoginId = @CLoginId" +
                            " Where ClientCode = @ClientCode";
                        string[] BizNo = txt_BizNo.Text.Split('-');
                        UpCommand.Parameters.AddWithValue("@BizNo1", BizNo[0]);
                        UpCommand.Parameters.AddWithValue("@BizNo2", BizNo[1]);
                        UpCommand.Parameters.AddWithValue("@BizNo3", BizNo[2]);
                        UpCommand.Parameters.AddWithValue("@Name", txt_Name.Text);
                        UpCommand.Parameters.AddWithValue("@CEO", txt_CEO.Text);
                        UpCommand.Parameters.AddWithValue("@Addr1", txt_State.Text);
                        UpCommand.Parameters.AddWithValue("@Addr2",txt_City.Text);
                        UpCommand.Parameters.AddWithValue("@AddrDetail", txt_Street.Text);
                        UpCommand.Parameters.AddWithValue("@ZipCode", txt_Zip.Text);
                        UpCommand.Parameters.AddWithValue("@CMobileNo", txt_MobileNo.Text.Replace("-", ""));
                        UpCommand.Parameters.AddWithValue("@ClientPhoneNo", txt_PhoneNo.Text.Replace("-", ""));
                        UpCommand.Parameters.AddWithValue("@CPassword", txt_Password.Text);
                        UpCommand.Parameters.AddWithValue("@CLoginId", txt_LoginId.Text);
                        UpCommand.Parameters.AddWithValue("@ClientCode", txt_Code.Text);

                        UpCommand.ExecuteNonQuery();

                        try
                        {
                            InitCustomerTable();
                            string CustomerCode = "1001";
                            if (_CustomerViewModelList.Any())
                            {
                                CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                            }
                            else
                            {
                                CustomerCode = "1001";
                            }

                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1218198192").Count() == 0)
                            {

                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                        @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1 @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, 0, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "121-81-98192");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }
                            InitCustomerTable();
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1268664275").Count() == 0)
                            {
                                if (_CustomerViewModelList.Any())
                                {
                                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                                }
                                else
                                {
                                    CustomerCode = "1001";
                                }
                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                         @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1 @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, 0, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "126-86-64275");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }

                            InitCustomerTable();
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1238645376").Count() == 0)
                            {
                                if (_CustomerViewModelList.Any())
                                {
                                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                                }
                                else
                                {
                                    CustomerCode = "1001";
                                }
                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                          @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1 @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, 0, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "123-86-45376");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }
                           
                            InitCustomerTable();
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1131183754").Count() == 0)
                            {
                                if (_CustomerViewModelList.Any())
                                {
                                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                                }
                                else
                                {
                                    CustomerCode = "1001";
                                }
                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                         @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1  @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, 0, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "113-11-83754");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }

                            InitCustomerTable();
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == txt_BizNo.Text.Replace("-","")).Count() == 0)
                            {
                                if (_CustomerViewModelList.Any())
                                {
                                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                                }
                                else
                                {
                                    CustomerCode = "1001";
                                }
                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                         @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)" +
                                        $" VALUES(@Code  ,'{txt_BizNo.Text}','{txt_Name.Text}','{txt_CEO.Text}', '{txt_Uptae.Text}', '{txt_Upjong.Text}','{txt_State.Text}', '{txt_City.Text}', '{txt_Street.Text}', 4, '', 3, '{txt_Email.Text}', '{txt_PhoneNo.Text}', '', '{txt_CEO.Text}', '{txt_MobileNo.Text}',getdate(), @ClientId, '{txt_Zip.Text}', NULL, 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, '', 0, 0, 0, getdate(), 0)";
                                    
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        catch
                        {

                        }

                    }


                    else
                    {
                        var DealerQ = clientDataSet.Dealers.Where(c => c.DealerId == int.Parse(cmb_Admin.SelectedValue.ToString())).ToArray();
                        SqlCommand INCommand = cn.CreateCommand();
                        INCommand.CommandText = "INSERT ClientManage (BizNo1,BizNo2,BizNo3,Name,CEO,Addr1,Addr2,AddrDetail,ZipCode,CMobileNo,ClientPhoneNo,CPassword,CLoginId,ClientCode,DealerId,DealerMobileNo,DealerName,CardState,CardStateName)" +
                            "VALUES(@BizNo1,@BizNo2,@BizNo3,@Name,@CEO,@Addr1,@Addr2,@AddrDetail,@ZipCode,@CMobileNo,@ClientPhoneNo,@CPassword,@CLoginId,@ClientCode,@DealerId,@DealerMobileNo,@DealerName,1,'등록') ";
                           
                        string[] BizNo = txt_BizNo.Text.Split('-');
                        INCommand.Parameters.AddWithValue("@BizNo1", BizNo[0]);
                        INCommand.Parameters.AddWithValue("@BizNo2", BizNo[1]);
                        INCommand.Parameters.AddWithValue("@BizNo3", BizNo[2]);
                        INCommand.Parameters.AddWithValue("@Name", txt_Name.Text);
                        INCommand.Parameters.AddWithValue("@CEO", txt_CEO.Text);
                        INCommand.Parameters.AddWithValue("@Addr1", txt_State.Text);
                        INCommand.Parameters.AddWithValue("@Addr2", txt_City.Text);
                        INCommand.Parameters.AddWithValue("@AddrDetail", txt_Street.Text);
                        INCommand.Parameters.AddWithValue("@ZipCode", txt_Zip.Text);
                        INCommand.Parameters.AddWithValue("@CMobileNo", txt_MobileNo.Text.Replace("-", ""));
                        INCommand.Parameters.AddWithValue("@ClientPhoneNo", txt_PhoneNo.Text.Replace("-", ""));
                        INCommand.Parameters.AddWithValue("@CPassword", txt_Password.Text);
                        INCommand.Parameters.AddWithValue("@CLoginId", txt_LoginId.Text);
                        INCommand.Parameters.AddWithValue("@ClientCode", txt_Code.Text);
                        INCommand.Parameters.AddWithValue("@DealerId", int.Parse(cmb_Admin.SelectedValue.ToString()));
                        INCommand.Parameters.AddWithValue("@DealerMobileNo", DealerQ.First().MobileNo);
                        INCommand.Parameters.AddWithValue("@DealerName", DealerQ.First().Name);

                        INCommand.ExecuteNonQuery();

                        try
                        {
                            //var CustomersDataTable = new BaseDataSet.CustomersDataTable();
                            //var _Repository = new CustomerRepository();


                            //_Repository.FindbyBizNo(CustomersDataTable, "121-81-98192");
                            //var _Customer = CustomersDataTable[0];
                            //var CustomerId = _Customer.CustomerId;
                            InitCustomerTable();
                            string CustomerCode = "1001";
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1218198192").Count() == 0)
                            {

                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                        @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1 @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "121-81-98192");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }
                            InitCustomerTable();
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1268664275").Count() == 0)
                            {
                                if (_CustomerViewModelList.Any())
                                {
                                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                                }
                                else
                                {
                                    CustomerCode = "1001";
                                }
                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                         @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1 @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "126-86-64275");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }

                            InitCustomerTable();
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1238645376").Count() == 0)
                            {
                                if (_CustomerViewModelList.Any())
                                {
                                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                                }
                                else
                                {
                                    CustomerCode = "1001";
                                }
                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                          @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1 @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "123-86-45376");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }

                            InitCustomerTable();
                            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == "1131183754").Count() == 0)
                            {
                                if (_CustomerViewModelList.Any())
                                {
                                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                                }
                                else
                                {
                                    CustomerCode = "1001";
                                }
                                using (SqlCommand DriverInstanceCommand = cn.CreateCommand())
                                {
                                    DriverInstanceCommand.CommandText =
                                         @"Insert into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)
                                          select TOP 1  @Code  ,BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo,CreateTime, @ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart from customers where BizNo = @BizNo";
                                    DriverInstanceCommand.Parameters.AddWithValue("@BizNo", "113-11-83754");
                                    DriverInstanceCommand.Parameters.AddWithValue("@Code", CustomerCode);
                                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                    DriverInstanceCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        catch
                        {

                        }

                    }

                        cn.Close();
                    }



                //List<(int VAccountPoolId, String VBankName, String VAccount)> VAccountPoolList = new List<(int, string, string)>();
                //Data.Connection((_Connection) =>
                //{
                   

                //    ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                //    {
                //        ClientId = ClientId,
                //        CDate = DateTime.Now,
                //        Amount = 30000,
                //        MethodType = "회원가입",
                //        Remark = "회원가입",
                //    });
                //    ShareOrderDataSet.Instance.SaveChanges();
                //});

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
                MessageBox.Show("회원가입 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
      
        public bool IsSuccess = false;
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            Sgubun = "Close";


          if(txt_Gubun.Text == "0")
            {


                _UpdateDB();
            }
            else if (txt_Gubun.Text == "1")
            {

                if (txt_BizNo.Text.Length != 12)
                {
                    MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                    err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                    return ;
                }
                
                if (txt_Name.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;

                }
                if (txt_CEO.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (txt_Uptae.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (txt_Upjong.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (string.IsNullOrWhiteSpace(txt_Zip.Text) || string.IsNullOrWhiteSpace(txt_Street.Text))
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Street, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;


                }
             
                if (txt_LoginId.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_LoginId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }
                else
                {

                    //var Query1 = "Select Count(*) From ClientUsers Where LoginId = @LoginId And Clientid != @ClientId";
                    //var Query2 =
                    //    "Select Count(*) From Clients Where LoginId = @LoginId And Clientid != @ClientId";
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
                        cmd1.Parameters.AddWithValue("@ClientId", txt_ClientId.Text);
                        if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                        {
                            IsDuplicated = true;
                        }
                        //if (!IsDuplicated)
                        //{
                        //    SqlCommand cmd2 = cn.CreateCommand();
                        //    cmd2.CommandText = Query2;
                        //    cmd2.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                        //    cmd2.Parameters.AddWithValue("@ClientId", txt_ClientId.Text);
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
                        return;
                    }
                }

                if (txt_Password.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }
                if (txt_MobileNo.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }

                if (txt_PhoneNo.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }

                //if(cmbVBankName.SelectedIndex == 0)
                //{
                //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                //    err.SetError(cmbVBankName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                //    return;
                //}

                if (txt_Email.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Email, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
                if (cmb_PayBankName.SelectedIndex < 1)
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(cmb_PayBankName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;                  
                }

                if (txt_PayAccountNo.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_PayAccountNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }
                if (txt_AccountOwner.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_AccountOwner, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }


                string Account_Name = txt_AccountOwner.Text;
                string Mid = "edubill";
                string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_PayBankName.SelectedValue.ToString(), txt_PayAccountNo.Text, txt_AccountOwner.Text, cmb_PayBankName.Text, Mid);

                WebClient mWebClient = new WebClient();
                try
                {
                    //                    var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                    var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                    if (r == null || r.ToLower() != "true")
                    {
                        var _Result2 = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));
                        _Result2 = HttpUtility.UrlDecode(_Result2);
                        if (_Result2 == "오류")
                        {
                            MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            //Invoke(new Action(() => pnProgress.Visible = false));
                            //Invoke(new Action(() => MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                            return;
                        }
                        else
                        {
                            if (MessageBox.Show($"{cmb_PayBankName.Text} / {txt_PayAccountNo.Text} / {_Result2}\r\n위 계좌가 맞습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            {
                                return;
                            }
                            else
                            {
                                Account_Name = _Result2;

                                txt_AccountOwner.Text = Account_Name;
                            }




                        }

                    }
                }
                catch (Exception)
                {
                    //Invoke(new Action(() => pnProgress.Visible = false));
                    //Invoke(new Action(() => MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));

                    MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }



                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "UPDATE Clients SET Name = @Name " +
                        ", CEO = @CEO" +
                        ", AddressState = @AddressState" +
                        ", AddressCity = @AddressCity" +
                        ", AddressDetail = @AddressDetail" +
                        ", ZipCode = @Zipcode" +
                        ", LoginId  = @LoginId" +
                        ", Password = @Password" +
                        ", MobileNo = @MobileNo" +
                        ", DealerId = @DealerId " +
                        ", Email = @Email " +
                        ", Admin = @Admin " +
                        ", status = 1" +
                        ", CargoApiYn = 1" +
                        " WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", txt_ClientId.Text);

                    _Command.Parameters.AddWithValue("@Name", txt_Name.Text);
                    _Command.Parameters.AddWithValue("@CEO", txt_CEO.Text);
                    _Command.Parameters.AddWithValue("@AddressState", txt_State.Text);
                    _Command.Parameters.AddWithValue("@AddressCity", txt_City.Text);
                    _Command.Parameters.AddWithValue("@AddressDetail", txt_Street.Text);
                    _Command.Parameters.AddWithValue("@ZipCode", txt_Zip.Text);

                    _Command.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    _Command.Parameters.AddWithValue("@Password", txt_Password.Text);
                    _Command.Parameters.AddWithValue("@MobileNo", txt_MobileNo.Text);
                    _Command.Parameters.AddWithValue("@Email", txt_Email.Text);

                    _Command.Parameters.AddWithValue("@DealerId", int.Parse(cmb_Admin.SelectedValue.ToString()));
                    _Command.Parameters.AddWithValue("@Admin", cmb_Admin.Text);


                    _Command.ExecuteNonQuery();
                }

                try { MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "운송사", 1), "운송사 정보 수정 성공");
                    this.Close();
                }
                catch { }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _InitCmb()
        {

            var DealersDataSource = clientDataSet.Dealers.Where(c => c.Status == 2).Select(c => new { c.Name, c.Code, c.DealerId }).OrderBy(c => c.DealerId).ToArray();
            cmb_Admin.DataSource = DealersDataSource;
            cmb_Admin.DisplayMember = "Name";
            cmb_Admin.ValueMember = "DealerId";

            cmb_Admin.SelectedIndex = 0;

            //cmb_Admin.DisplayMember = "Text";
            //cmb_Admin.ValueMember = "Value";
            //cmb_Admin.DataSource = Filter.Dealer.DealerList;
            //cmb_Admin.SelectedIndex = 1;


            var SatusDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_status.DataSource = SatusDataSource;
            cmb_status.DisplayMember = "Name";
            cmb_status.ValueMember = "value";

            Dictionary<string, string> PayBank = new Dictionary<string, string>();

            PayBank.Add("은행선택", "은행선택");
            PayBank.Add("국민", "국민");
            PayBank.Add("기업", "기업");
            PayBank.Add("농협", "농협");
            PayBank.Add("신한", "신한");
            PayBank.Add("우리", "우리");
          
            cmbVBankName.DataSource = new BindingSource(PayBank, null);
            cmbVBankName.DisplayMember = "Value";
            cmbVBankName.ValueMember = "Key";
            cmbVBankName.SelectedIndex = 1;

            Dictionary<string, string> PayBank2 = new Dictionary<string, string>();

            PayBank2.Add(" ", "은행선택");
            PayBank2.Add("003", "기업은행");
            PayBank2.Add("005", "외한은행");
            PayBank2.Add("004", "국민은행");
            PayBank2.Add("011", "농협");
            PayBank2.Add("020", "우리은행");
            PayBank2.Add("088", "신한은행");
            PayBank2.Add("023", "SC제일");
            PayBank2.Add("027", "씨티은행");
            PayBank2.Add("032", "부산은행");
            PayBank2.Add("039", "경남은행");
            PayBank2.Add("031", "대구은행");
            PayBank2.Add("071", "우체국");
            PayBank2.Add("034", "광주은행");
            PayBank2.Add("007", "수협");
            PayBank2.Add("022", "상업은행");
            PayBank2.Add("030", "대동은행");
            PayBank2.Add("033", "충청은행");
            PayBank2.Add("035", "제주은행");
            PayBank2.Add("036", "경기은행");
            PayBank2.Add("037", "전북은행");
            PayBank2.Add("038", "강원은행");
            PayBank2.Add("040", "충북은행");
            PayBank2.Add("081", "하나은행");
            PayBank2.Add("082", "보람은행");
            PayBank2.Add("002", "산업은행");
            PayBank2.Add("045", "새마을금고");
            PayBank2.Add("052", "HSBC은행");
            PayBank2.Add("048", "신협");

            PayBank2.Add("090", "카카오뱅크");
            PayBank2.Add("089", "케이뱅크");

            PayBank2.Add("S0", "유안타증권");
            PayBank2.Add("S1", "미래에셋");
            PayBank2.Add("S2", "신한금융투자");
            PayBank2.Add("S3", "삼성증권");
            PayBank2.Add("S4", "한국투자증권");
            PayBank2.Add("S5", "한화증권");

            cmb_PayBankName.DataSource = new BindingSource(PayBank2, null);
            cmb_PayBankName.DisplayMember = "Value";
            cmb_PayBankName.ValueMember = "Key";
            cmb_PayBankName.SelectedIndex = 0;

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

        private void txt_BizNo_Leave(object sender, EventArgs e)
        {
            if (txt_BizNo.Text.Length != 12)
            {
                return;
            }
            int r = 0;
            string S_BizNo = string.Empty;
            string BiznoId = string.Empty;





            var clientSearch = clientDataSet.Clients.Where(c => c.BizNo.Replace("-", "") == txt_BizNo.Text.Replace("-", "")).ToArray();

            if (clientSearch.Where(c=> c.Status == 5).Count()  > 0)
            {
                MessageBox.Show("차주가\r\n세금계산서를 발행한 정보가 있습니다.\r\n확인해 보시고,\r\n귀 사 정보에 맞도록 수정하시기 바랍니다.","회원가입");
                var Client_Code = clientDataSet.Clients.Where(c => c.BizNo.Replace("-", "") == txt_BizNo.Text.Replace("-", "") && c.Status == 5).ToArray().First();

                //txt_BizNo.ReadOnly = true;
                txt_Name.Text = Client_Code.Name;
                txt_CEO.Text = Client_Code.CEO;
                txt_Upjong.Text = Client_Code.Upjong;
                txt_Uptae.Text = Client_Code.Uptae;
                if (String.IsNullOrEmpty(Client_Code.ZipCode))
                {
                    txt_Zip.Text = "00000";
                }
                else
                {
                    txt_Zip.Text = Client_Code.ZipCode;
                }
                txt_State.Text = Client_Code.AddressState;
                txt_City.Text = Client_Code.AddressCity;
                txt_Street.Text = Client_Code.AddressDetail;
                txt_LoginId.Text = Client_Code.LoginId;
                cmb_Admin.SelectedIndex = 0;
                txt_Code.Text = Client_Code.Code;
                txt_Gubun.Text = "1";
                txt_ClientId.Text = Client_Code.ClientId.ToString();
                txt_Email.Text = Client_Code.Email;
                txt_Password.Text = Client_Code.Password;
                if(!String.IsNullOrWhiteSpace(Client_Code.VBankName))
                {
                    cmbVBankName.SelectedValue = Client_Code.VBankName;

                    cmbVBankName.Enabled = false;
                }
                else
                {
                    cmbVBankName.SelectedIndex = 0;
                    cmbVBankName.Enabled = true;
                }
                
            }
            else if (clientSearch.Where(c => c.Status != 5).Count() > 0)
            {
                MessageBox.Show("이미 가입된 정보가 있습니다. 해당 아이디로 로그인 하세요.\r( 문의 : 1661-6090 )", "회원가입");

                this.Close();
            }
            else if (clientSearch.Count() == 0 )
            {
              //  txt_Password.Text = txt_BizNo.Text.Substring(7, 5);
                if (string.IsNullOrEmpty(txt_LoginId.Text))
                {
                    txt_LoginId.Text = string.Empty;
                }
                txt_BizNo.ReadOnly = false;

                if (string.IsNullOrEmpty(txt_Name.Text))
                {
                    txt_Name.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txt_CEO.Text))
                {

                    txt_CEO.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txt_Upjong.Text))
                {
                    txt_Upjong.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txt_Uptae.Text))
                {
                    txt_Uptae.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txt_Zip.Text))
                {
                    txt_Zip.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txt_State.Text))
                {
                    txt_State.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txt_City.Text))
                {
                    txt_City.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txt_Street.Text))
                {
                    txt_Street.Text = string.Empty;
                }
               
                cmb_Admin.SelectedIndex = 0;
                //txt_Code.Text = Client_Code.Code;

                //  txt_ClientId.Text = Client_Code.ClientId.ToString();
                if (string.IsNullOrEmpty(txt_Email.Text))
                {
                    txt_Email.Text = "";
                }
                if (string.IsNullOrEmpty(txt_Password.Text))
                {
                    txt_Password.Text = "";
                }
                txt_Gubun.Text = "0";
            }

        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
                {
                    if (_S.Length > 2)
                    {
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                    }
                    if (_S.Length > 6)
                    {
                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                    }
                    if (_S.Length > 11)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                    }
                }
                else
                {
                    if (_S.Length > 3)
                    {
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                    }
                    if (_S.Length > 7)
                    {
                        if (_S.Length == 9)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                        else
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                _Txt.Text = _S;
            }
        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
                {
                    if (_S.Length > 2)
                    {
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                    }
                    if (_S.Length > 6)
                    {
                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                    }
                    if (_S.Length > 11)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                    }
                }
                else
                {
                    if (_S.Length > 3)
                    {
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                    }
                    if (_S.Length > 7)
                    {
                        if (_S.Length == 9)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                        else
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                _Txt.Text = _S;
            }
        }
    }
}
