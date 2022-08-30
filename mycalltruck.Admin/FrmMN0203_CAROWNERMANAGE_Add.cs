using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.Net;

using System.Security;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace mycalltruck.Admin
{
    public partial class FrmMN0203_CAROWNERMANAGE_Add : Form
    {
         DESCrypt m_crypt = null;

         string BtnGubun = "";

        bool Account_Result = false;
        public FrmMN0203_CAROWNERMANAGE_Add()
        {
            InitializeComponent();

            _InitCmb();

              m_crypt = new DESCrypt("12345678");
            
            cmb_BizType.SelectedIndex = 1;
            cmb_UsePayNow.SelectedIndex = 2;
            cmb_RouteType.SelectedIndex = 1;
           // 
            cmb_UsePayNow.Enabled = false;

           
        }
        private void FrmMN0203_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            this.candidateTableAdapter.Fill(this.cMDataSet.Candidate);
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            txt_Code.Text = "100001";
            DriversCode_Add();
            cmb_PayBankName.SelectedIndex = 0;
            DriverAddCode_Add();
            txt_Name.Focus();
            this.driversTableAdapter.Fill(cMDataSet.Drivers);
        }

       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            BtnGubun = "Add";
            //ValidationAccount();
            if (cmb_PayBankName.SelectedIndex != 0 && !String.IsNullOrEmpty(txt_PayAccountNo.Text) && !String.IsNullOrEmpty(txt_AccountOwner.Text))
            {
                ValidationAccount();
            }
            else
            {
                _UpdateDB();
            }


                //if (_UpdateDB() > 0)
                //{



                //    if (txt_MobileNo.Text.Replace("-", "").Length >= 10)
                //    {
                //        em_mmt_tranTableAdapter.Insert(DateTime.Now, LocalUser.Instance.LogInInfomation.UserName + "에서 보내는 메시지 입니다", LocalUser.Instance.LogInInfomation.UserName + "에서 보내는 메시지 입니다" + "\n" + "당 사로 보내는 (전자)세금계산서는 아래 절차에 따라 앱을 설치하시고,그 앱으로 발행하여 주시기 바랍니다." + "\n" + "① 앱 설치방법" + "\n" + "  -핸드폰(안드로이드)" + "\n" + "  “Play 스토어”에서 “카드페이” 단어로 검색 설치 " + "\n" + "  -핸드폰(아이폰)" + "\n" + "  “ m.cardpay.kr ” 직접 접속 " + "\n" + "② 로그인" + "\n" + "  - 아이디 : " + txt_LoignId.Text + "" + "\n" + "  - 비밀번호 : " + txt_Password.Text + "" + "\n" + "③ 회원정보 수정입력 ( 로그인 후, 안내 참조 )" + "\n" + "④ 첫 화면의 “세금계산서” 버튼 클릭" + "\n" + "-----------------------------------" + "\n" + "⑤ “청구” 탭 버튼 클릭" + "\n" + "⑥ 화면상단 청구할 “운송사”를 선택한다." + "\n" + "⑦ 해당 정보를 입력하여 세금계산서를 전송한다. " + "\n" + "※콜센터 전화번호 : 070-8680-6908", "028535111", "3", txt_MobileNo.Text.Replace("-", ""));
                //    }

                //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "차주", 1), "차주정보 추가 성공");



                //    DriversCode_Add();
                //    //txt_Code.Text = "";
                //    txt_Name.Text = "";
                //    txt_BizNo.Text = "";
                //    txt_CEO.Text = "";
                //    txt_Uptae.Text = "";
                //    txt_Upjong.Text = "";
                //    txt_Email.Text = "";
                //    //txt_LoignId.Text = "";
                //    txt_Password.Text = "";
                //    txt_MobileNo.Text = "";
                //    txt_PhoneNo.Text = "";
                //    txt_FaxNo.Text = "";
                //    txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
                //    cmb_AddressState.SelectedIndex = 0;
                //    cmb_AddressCity.SelectedIndex = 0;
                //    txt_Addr.Text = "";



                //    cmb_BizType.SelectedIndex = 0;
                //    cmb_RouteType.SelectedIndex = 0;
                //    cmb_InsuranceType.SelectedIndex = 0;
                //    txt_CarNo.Text = "";
                //    cmb_CarType.SelectedIndex = 0;
                //    cmb_CarSize.SelectedIndex = 0;
                //    //  cmb_CarYear.SelectedIndex = 0;
                //    txt_CarYear.Text = "";
                //    cmb_ParkState.SelectedIndex = 0;
                //    cmb_ParkCity.SelectedIndex = 0;
                //    cmb_ParkStreet.SelectedIndex = 0;

                //    cmb_CarGubun.SelectedIndex = 0;
                //    cmb_FPIS_CarType.SelectedIndex = 0;
                //    txt_MID.Text = "";

                //    cmb_PayBankName.SelectedIndex = 0;
                //    txt_PayAccountNo.Text = "";
                //    txt_AccountOwner.Text = "";
                //    txt_CEOBirth.Text = "";

                //}

                //txt_Code.Focus();
            
        }
        public bool IsSuccess = false;
        public CMDataSet.DriversRow CurrentCode = null;
        public CMDataSet.DriverPapersRow DCurrentCode = null;

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            _UpdateDB();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void DriversCode_Add()
        {
            this.driversTableAdapter.Fill(this.cMDataSet.Drivers);


            var Driver_code = cMDataSet.Drivers.Select(c => new { c.Code }).OrderByDescending(c => c.Code).ToArray();

            if (Driver_code.Count() > 0)
            {
                var DriverCode = 100001;
                var DriverCodeCandidate = cMDataSet.Drivers.OrderBy(c => c.Code).Select(c => c.Code).ToArray();
                while (true)
                {
                    if (!DriverCodeCandidate.Any(c => c == DriverCode.ToString()))
                    {
                        break;
                    }
                    DriverCode++;
                }
                txt_Code.Text = DriverCode.ToString();
            }
            else
            {

                txt_Code.Text = "100001";
            }

        }
        private void DriverAddCode_Add()
        {
            var DriverAdd_code = cMDataSet.DriverAdd.Select(c => new { c.DriverAddCode }).OrderByDescending(c => c.DriverAddCode).ToArray();



            if (DriverAdd_code.Count() > 0)
            {
                var DriverAddCodeCode = 1001;
                var DriverAddCandidate = cMDataSet.DriverAdd.OrderBy(c => c.DriverAddCode).Select(c => c.DriverAddCode).ToArray();
                while (true)
                {
                    if (!DriverAddCandidate.Any(c => c == DriverAddCodeCode.ToString()))
                    {
                        break;
                    }
                    DriverAddCodeCode++;
                }
                txt_DriverAddCode.Text = DriverAddCodeCode.ToString();
            }
            else
            {

                txt_DriverAddCode.Text = "1001";
            }



        }
        private void DriverId_Add()
        {
            int r = 0;
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand selectCmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM Clients ", cn);
                // selectCmd.Parameters.Add(new SqlParameter("@UserID", txtID.Text));
                // selectCmd.Parameters.Add(new SqlParameter("@UserPassword", txtPassword.Text));
                cn.Open();
                object o = selectCmd.ExecuteScalar();
                if (o != null)
                    r = Convert.ToInt16(o);
                cn.Close();
            }

        }
        private int _UpdateDB()
        {
            err.Clear();

             if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
           
            if (txt_BizNo.Text.Length != 12 || txt_BizNo.Text.Contains(" "))
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");
                tabControl1.SelectTab(0);
                return -1;
            }

            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;

            }
            if (txt_CEO.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }
            if (txt_Uptae.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }
            if (txt_Upjong.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }

            if (txt_MobileNo.Text.Length < 10)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }
            if (txt_LoignId.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_LoignId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }
            if (txt_Password.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }

            if(txt_Addr.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Addr, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }

            if (cmb_PayBankName.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_PayBankName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }

            if (txt_PayAccountNo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PayAccountNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }
            if (txt_AccountOwner.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AccountOwner, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(0);
                return -1;
            }

            if (txt_CarNo.Text == "")
            {
                MessageBox.Show("차량번호 항목이 누락되었습니다. 확인해 주십시오.","필수항목누락");
                err.SetError(txt_CarNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(1);
                return -1;
            }
            else if (cMDataSet.Drivers.Where(c => c.CarNo == txt_CarNo.Text.Trim().Replace(" ","")).Count() > 0)
            {
                MessageBox.Show("차량번호가 중복되었습니다.!!", "차량번호 입력 오류");
                err.SetError(txt_CarNo, "차량번호가 중복되었습니다.!!");
                tabControl1.SelectTab(1);
                return -1;
            }

            if (cmb_CarType.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_CarType, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(1);
                return -1;
            }

            if (cmb_CarSize.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_CarSize, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(1);
                return -1;
            }

            if (cmb_CarGubun.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_CarGubun, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(1);
                return -1;
            }

            if (txt_CarYear.Text == "")
            {
                MessageBox.Show("기사명 항목이 누락되었습니다. 확인해 주십시오.", "필수항목누락");
                err.SetError(txt_CarYear, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(1);
                return -1;
            }

            if (cmb_FPIS_CarType.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_FPIS_CarType, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                tabControl1.SelectTab(1);
                return -1;
            }

            try
            {
                ValidationAccount();
                return 1;

            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Code") iName = "코드";
                if (code == "Name") iName = "상호";
                if (code == "ID_Code") iName = "사업자 번호";
                if (code == "CEO_Name") iName = "대표자명";
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

        private void ValidationAccount()
        {
            string Parameter = "";


            WebClient mWebClient = new WebClient();
            string Mid = "edubill";
            Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_PayBankName.SelectedValue.ToString(), txt_PayAccountNo.Text, txt_AccountOwner.Text, cmb_PayBankName.Text, Mid);
            mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
            mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
        }
        private void MWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null && e.Result != null && e.Result.ToLower() == "true")
            {
                Account_Result = true;
                _AddClient();
            }
            else
            {
                Account_Result = false;
                MessageBox.Show("계좌인증 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void _AddClient()
        {
            CMDataSet.DriversRow row = cMDataSet.Drivers.NewDriversRow();
            CurrentCode = row;

            row.Code = txt_Code.Text;
            row.Name = txt_Name.Text;
            row.BizNo = txt_BizNo.Text.Replace("-", "");
            row.CEO = txt_CEO.Text;
            row.Uptae = txt_Uptae.Text;
            row.Upjong = txt_Upjong.Text;
            row.Email = txt_Email.Text;
            row.LoginId = txt_LoignId.Text;
            row.Password = txt_Password.Text;

            if (txt_MobileNo.Text.Length < 12)
            {
                row.MobileNo = "";
            }
            else
            {
                row.MobileNo = txt_MobileNo.Text;
            }

            if (txt_PhoneNo.Text.Length < 12)
            {
                row.PhoneNo = "";
            }
            else
            {
                row.PhoneNo = txt_PhoneNo.Text;
            }
            if (txt_FaxNo.Text.Length < 12)
            {
                row.FaxNo = "";
            }

            else
            {
                row.FaxNo = txt_FaxNo.Text;
            }
            row.CreateDate = DateTime.Parse(txt_CreateDate.Text);
            row.AddressState = cmb_AddressState.SelectedValue.ToString();
            row.AddressCity = cmb_AddressCity.SelectedValue.ToString();
            row.AddressDetail = txt_Addr.Text;
            row.BizType = int.Parse(cmb_BizType.SelectedValue.ToString());
            row.RouteType = int.Parse(cmb_RouteType.SelectedValue.ToString());
            row.InsuranceType = int.Parse(cmb_InsuranceType.SelectedValue.ToString());
            row.CarNo = txt_CarNo.Text.Trim().Replace(" ", "");
            row.CarType = int.Parse(cmb_CarType.SelectedValue.ToString());
            row.CarSize = int.Parse(cmb_CarSize.SelectedValue.ToString());
            row.CarGubun = int.Parse(cmb_CarGubun.SelectedValue.ToString());
            if (txt_DriverMobileNo.Text.Length < 12)
            {
                row.DriverMobileNo = "";
            }
            else
            {
                row.DriverMobileNo = txt_DriverMobileNo.Text;
            }
            row.CarYear = txt_CarYear.Text;
            row.ParkState = cmb_ParkState.SelectedValue.ToString();
            row.ParkCity = cmb_ParkCity.SelectedValue.ToString();
            row.ParkStreet = cmb_ParkStreet.SelectedValue.ToString();
            if (cmb_CarGubun.SelectedIndex == 3)
            {
                row.Car_ContRact = true;
                row.RequestFrom = dtp_RequestFrom.Text;
                row.RequestTo = dtp_RequestTo.Text;
            }
            else
            {
                row.Car_ContRact = false;
            }
            row.AccountOwner = "";
            row.AccountRegNo = "";
            row.BankName = "";
            row.AccountNo = "";
            row.AccountExtra = "";
            row.UsePayNow = int.Parse(cmb_UsePayNow.SelectedValue.ToString());
            row.ClientBizType = 0;
            row.CandidateId = LocalUser.Instance.LogInInformation.ClientId;
            if (LocalUser.Instance.LogInInformation.IsSubClient)
                row.SubClientId = LocalUser.Instance.LogInInformation.SubClientId;
            else
                row.SetSubClientIdNull();
            row.FpisCarType = int.Parse(cmb_FPIS_CarType.SelectedValue.ToString());
            row.AccountUse = false;
            row.DTGUse = true;
            row.FPISUse = true;
            row.MyCallUSe = true;
            row.ServicePrice = "5500";
            row.useTax = true;
            row.OTGUse = false;
            row.OTGPrice = 0;
            row.AccountPrice = 0;
            row.FPISPrice = 0;
            row.MyCallPrice = 0;
            row.DTGPrice = 5000;
            row.MID = txt_MID.Text.Trim();

            string tempString = string.Empty;
            //DES 암호화
            tempString = m_crypt.Encrypt(txt_PayAccountNo.Text);
            row.PayAccountNo = tempString;



            //  row.PayAccountNo = txt_PayAccountNo.Text;
            row.PayBankCode = cmb_PayBankName.SelectedValue.ToString();
            row.PayBankName = cmb_PayBankName.Text;
            row.PayInputName = txt_AccountOwner.Text;


            row.LG_MertKeyYn = false;
            row.ServiceState = 3;
            cMDataSet.Drivers.AddDriversRow(row);
            try
            {
                driversTableAdapter.Update(row);
                if (BtnGubun == "Add")
                {
                    if (txt_MobileNo.Text.Replace("-", "").Length >= 10)
                    {
                        em_mmt_tranTableAdapter.Insert(DateTime.Now, LocalUser.Instance.LogInInformation.ClientName, LocalUser.Instance.LogInInformation.ClientName + "에서 보내는 메시지 입니다" + "\n" + "당 사로 보내는 (전자)세금계산서는 아래 절차에 따라 앱을 설치하시고,그 앱으로 발행하여 주시기 바랍니다." + "\n" + "① 앱 설치방법" + "\n" + "  -핸드폰(안드로이드)" + "\n" + "  “Play 스토어”에서 “차세로” 단어로 검색 설치 " + "\n" + "  -핸드폰(아이폰)" + "\n" + "  “ m.cardpay.kr ” 직접 접속 " + "\n" + "② 로그인" + "\n" + "  - 아이디 : " + txt_LoignId.Text + "" + "\n" + "  - 비밀번호 : " + txt_Password.Text + "" + "\n" + "-----------------------------------" + "\n" + "③ 회원정보 수정입력 ( 로그인 후, 안내 참조 )" + "\n" + "④ 첫 화면의 “세금계산서” 버튼 클릭" + "\n" + "⑤ “청구” 탭 버튼 클릭" + "\n" + "⑥ 화면상단 청구할 “운송사”를 선택한다." + "\n" + "⑦ 해당 정보를 입력하여 세금계산서를 전송한다. " + "\n" + "※콜센터 전화번호 : 070-8680-6908", "028535111", "3", txt_MobileNo.Text.Replace("-", ""));
                    }
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "차주", 1), "차주정보 추가 성공");



                    DriversCode_Add();
                    //txt_Code.Text = "";
                    txt_Name.Text = "";
                    txt_BizNo.Text = "";
                    txt_CEO.Text = "";
                    txt_Uptae.Text = "";
                    txt_Upjong.Text = "";
                    txt_Email.Text = "";
                    //txt_LoignId.Text = "";
                    txt_Password.Text = "";
                    txt_MobileNo.Text = "";
                    txt_PhoneNo.Text = "";
                    txt_FaxNo.Text = "";
                    txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
                    cmb_AddressState.SelectedIndex = 0;
                    cmb_AddressCity.SelectedIndex = 0;
                    txt_Addr.Text = "";



                    cmb_BizType.SelectedIndex = 0;
                    cmb_RouteType.SelectedIndex = 0;
                    cmb_InsuranceType.SelectedIndex = 0;
                    txt_CarNo.Text = "";
                    cmb_CarType.SelectedIndex = 0;
                    cmb_CarSize.SelectedIndex = 0;
                    //  cmb_CarYear.SelectedIndex = 0;
                    txt_CarYear.Text = "";
                    cmb_ParkState.SelectedIndex = 0;
                    cmb_ParkCity.SelectedIndex = 0;
                    cmb_ParkStreet.SelectedIndex = 0;

                    cmb_CarGubun.SelectedIndex = 0;
                    cmb_FPIS_CarType.SelectedIndex = 0;
                    txt_MID.Text = "";

                    cmb_PayBankName.SelectedIndex = 0;
                    txt_PayAccountNo.Text = "";
                    txt_AccountOwner.Text = "";



                    txt_Code.Focus();
                }
                else
                {
                    if (txt_MobileNo.Text.Replace("-", "").Length >= 10)
                    {
                        em_mmt_tranTableAdapter.Insert(DateTime.Now, LocalUser.Instance.LogInInformation.ClientName, LocalUser.Instance.LogInInformation.ClientName + "에서 보내는 메시지 입니다" + "\n" + "당 사로 보내는 (전자)세금계산서는 아래 절차에 따라 앱을 설치하시고,그 앱으로 발행하여 주시기 바랍니다." + "\n" + "① 앱 설치방법" + "\n" + "  -핸드폰(안드로이드)" + "\n" + "  “Play 스토어”에서 “차세로” 단어로 검색 설치 " + "\n" + "  -핸드폰(아이폰)" + "\n" + "  “ m.cardpay.kr ” 직접 접속 " + "\n" + "② 로그인" + "\n" + "  - 아이디 : " + txt_LoignId.Text + "" + "\n" + "  - 비밀번호 : " + txt_Password.Text + "" + "\n" + "-----------------------------------" + "\n" + "③ 회원정보 수정입력 ( 로그인 후, 안내 참조 )" + "\n" + "④ 첫 화면의 “세금계산서” 버튼 클릭" + "\n" + "⑤ “청구” 탭 버튼 클릭" + "\n" + "⑥ 화면상단 청구할 “운송사”를 선택한다." + "\n" + "⑦ 해당 정보를 입력하여 세금계산서를 전송한다. " + "\n" + "※콜센터 전화번호 : 070-8680-6908", "028535111", "3", txt_MobileNo.Text.Replace("-", ""));
                    }

                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "차주", 1), "차주정보 추가 성공");

                }


            }
            catch
            {
                MessageBox.Show("차주추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }



        }

        private void _InitCmb()
        {


            var ParkStateDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.State }).Distinct().ToArray();
            cmb_ParkState.DataSource = ParkStateDataSource;
            cmb_ParkState.DisplayMember = "State";
            cmb_ParkState.ValueMember = "State";


            var AddressStateDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.State }).Distinct().ToArray();
            cmb_AddressState.DataSource = AddressStateDataSource;
            cmb_AddressState.DisplayMember = "State";
            cmb_AddressState.ValueMember = "State";




            var ParkCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            cmb_ParkCity.DataSource = ParkCityDataSource;
            cmb_ParkCity.DisplayMember = "City";
            cmb_ParkCity.ValueMember = "City";

            var AddressCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            cmb_AddressCity.DataSource = AddressCityDataSource;
            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";

            //   .Where(c => c.City == cmb_ParkCity.SelectedValue.ToString()).Where(c => c.Street != "").Select(c => new { c.Street }).Distinct().ToArray();
            var ParkStreetDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()).Where(c => c.City == cmb_ParkCity.SelectedValue.ToString()).Where(c => c.Street != "") select new { a.Street }).Distinct().ToArray();
            cmb_ParkStreet.DataSource = ParkStreetDataSource;
            cmb_ParkStreet.DisplayMember = "Street";
            cmb_ParkStreet.ValueMember = "Street";

            var BizTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BizType").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_BizType.DataSource = BizTypeDataSource;
            cmb_BizType.DisplayMember = "Name";
            cmb_BizType.ValueMember = "Value";

            var RouteTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "RouteType").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_RouteType.DataSource = RouteTypeDataSource;
            cmb_RouteType.DisplayMember = "Name";
            cmb_RouteType.ValueMember = "Value";


            var InsuranceTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InsuranceType").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_InsuranceType.DataSource = InsuranceTypeDataSource;
            cmb_InsuranceType.DisplayMember = "Name";
            cmb_InsuranceType.ValueMember = "Value";


            var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarType.DataSource = CarTypeDataSource;
            cmb_CarType.DisplayMember = "Name";
            cmb_CarType.ValueMember = "Value";


            var CarSizeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarSize.DataSource = CarSizeDataSource;
            cmb_CarSize.DisplayMember = "Name";
            cmb_CarSize.ValueMember = "Value";



            //var CandidateGubunDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ClientType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_CandidateGubun.DataSource = CandidateGubunDataSource;
            //cmb_CandidateGubun.DisplayMember = "Name";
            //cmb_CandidateGubun.ValueMember = "Value";


            //var CandidateDataSource = SingleDataSet.Instance.Clients.Select(c => new { c.ClientId, c.Name }).OrderBy(c => c.ClientId).ToArray();
            //cmb_Candidate.DataSource = CandidateDataSource;
            //cmb_Candidate.DisplayMember = "Name";
            //cmb_Candidate.ValueMember = "ClientId";


            //cmb_CandidateGubun.DataSource = candidateBindingSource;

            //cmb_CandidateGubun.DisplayMember = "name";
            //cmb_CandidateGubun.ValueMember = "id";
            //cmb_CandidateGubun.Tag = "Gubun";

            var UsePayNowDatasource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "UsePay").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_UsePayNow.DataSource = UsePayNowDatasource;
            cmb_UsePayNow.DisplayMember = "Name";
            cmb_UsePayNow.ValueMember = "Value";

            var CarGubunDatasource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarGubun").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CarGubun.DataSource = CarGubunDatasource;
            cmb_CarGubun.DisplayMember = "Name";
            cmb_CarGubun.ValueMember = "Value";

            var FPISCarGubunDatasource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "FPisCarType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_FPIS_CarType.DataSource = FPISCarGubunDatasource;
            cmb_FPIS_CarType.DisplayMember = "Name";
            cmb_FPIS_CarType.ValueMember = "Value";

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



            cmb_PayBankName.DataSource = new BindingSource(PayBank, null);
            cmb_PayBankName.DisplayMember = "Value";
            cmb_PayBankName.ValueMember = "Key";

            cmb_PayBankName.SelectedValue = " ";

             

        }
       


        private void txt_Code_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txt_Code.Text);
            if (!ismatch)
            {
                MessageBox.Show("숫자만 입력해 주세요.");
            }



        }
     

        private void cmb_ParkState_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_ParkCity.Enabled = true;




            var ParkCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            cmb_ParkCity.DataSource = ParkCityDataSource;
            cmb_ParkCity.DisplayMember = "City";
            cmb_ParkCity.ValueMember = "City";
        }

        private void cmb_ParkCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_ParkStreet.Enabled = true;
            var ParkStreetDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()).Where(c => c.City == cmb_ParkCity.SelectedValue.ToString()).Where(c => c.Street != "").Select(c => new { c.Street }).Distinct().ToArray();
            cmb_ParkStreet.DataSource = ParkStreetDataSource;
            cmb_ParkStreet.DisplayMember = "Street";
            cmb_ParkStreet.ValueMember = "Street";


          
        }

        private void cmb_AddressState_SelectedIndexChanged(object sender, EventArgs e)
        {

            cmb_AddressCity.Enabled = true;

            cmb_AddressCity.DataSource = null;
            var CarCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            cmb_AddressCity.DataSource = CarCityDataSource;

            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";


            //cmb_AddressCity.Enabled = true;
            //var AddressCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            //cmb_AddressCity.DataSource = AddressCityDataSource;
            //cmb_AddressCity.DisplayMember = "City";
            //cmb_AddressCity.ValueMember = "City";

            

        }

      

        private void cmb_CandidateGubun_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //cmb_Candidate.Enabled = true;
            //var CandidateDataSource = SingleDataSet.Instance.Clients.Where(c => c.BizType == int.Parse(cmb_CandidateGubun.SelectedValue.ToString())).Select(c => new { c.ClientId, c.Name }).OrderBy(c => c.ClientId).ToArray();
            //cmb_Candidate.DataSource = CandidateDataSource;
            //cmb_Candidate.DisplayMember = "Name";
            //cmb_Candidate.ValueMember = "ClientId";
        }

        private void btn_BizPaperAdd_Click(object sender, EventArgs e)
        {
            //dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //if (dlgOpen.ShowDialog() != DialogResult.OK) return;
            //txt_BizPaper.Text = dlgOpen.FileName;
            //pic_BizPaper.ImageLocation = dlgOpen.FileName;
        }

        private void btn_CarPaperAdd_Click(object sender, EventArgs e)
        {
            //dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //if (dlgOpen.ShowDialog() != DialogResult.OK) return;
            //txt_CarPaper.Text = dlgOpen.FileName;
            //pic_CarPaper.ImageLocation = dlgOpen.FileName;
        }

        private void btn_InsuPaperAdd_Click(object sender, EventArgs e)
        {
            //dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //if (dlgOpen.ShowDialog() != DialogResult.OK) return;
            //txt_InsuPaper.Text = dlgOpen.FileName;
            //pic_InsuPaper.ImageLocation = dlgOpen.FileName;
        }

        private void btn_InsuPaperDelete_Click(object sender, EventArgs e)
        {
            //txt_InsuPaper.Text = "";
            //pic_InsuPaper.ImageLocation = "";
        }

        private void btn_BizPaperDelete_Click(object sender, EventArgs e)
        {
            //txt_BizPaper.Text = "";
            //pic_BizPaper.ImageLocation = "";
        }

        private void btn_CarPaperDelete_Click(object sender, EventArgs e)
        {
            //txt_CarPaper.Text = "";
            //pic_CarPaper.ImageLocation = "";
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

            S_BizNo = txt_BizNo.Text.Substring(7, 5);




            var Driver_Loginid = cMDataSet.Drivers.Where(c => c.BizNo.Replace("-", "").Substring(5, 5) == txt_BizNo.Text.Replace("-", "").Substring(5, 5)).Where(c => c.LoginId != "test").Select(c => new { c.LoginId }).OrderByDescending(c => c.LoginId).ToArray();
            if (Driver_Loginid.Count() > 0)
            {
                var DriverInt = 1;
                var DriverIndCandi = cMDataSet.Drivers.Where(c => c.BizNo.Replace("-", "").Substring(5, 5) == txt_BizNo.Text.Replace("-", "").Substring(5, 5)).Where(c => c.LoginId != "test").Select(c => int.Parse(c.LoginId.Substring(6, 3))).ToArray();

                while (true)
                {
                    if (!DriverIndCandi.Any(c => c == int.Parse(DriverInt.ToString())))
                    {
                        break;
                    }
                    DriverInt++;
                }
                BiznoId = DriverInt.ToString();

            }
            else
            {
                BiznoId = "";

            }
            string BizNo = string.Empty;
            if (BiznoId == "")
            {
                BizNo = "001";

            }
            else if (BiznoId.Length == 2)
            {
                BizNo = "0" + BiznoId;
            }
            else if (BiznoId.Length == 1)
            {
                BizNo = "00" + BiznoId;
            }
            else if (BiznoId.Length == 3)
            {
                BizNo = BiznoId;
            }



            txt_LoignId.Text = "m" + txt_BizNo.Text.Substring(7, 5) + BizNo;





            if (cMDataSet.Drivers.Where(c => c.BizNo.Replace("-", "") == txt_BizNo.Text.Replace("-", "")).Count() > 0)
            {
                var Driver_code = cMDataSet.Drivers.Where(c => c.BizNo.Replace("-", "") == txt_BizNo.Text.Replace("-", "")).Select(c => new { c.Code, c.Name, c.CEO, c.Uptae, c.Upjong, c.AddressState, c.AddressCity, c.AddressDetail, c.LoginId }).OrderBy(c => c.LoginId).First();

                txt_Name.Text = Driver_code.Name;
                txt_CEO.Text = Driver_code.CEO;
                txt_Upjong.Text = Driver_code.Upjong;
                txt_Uptae.Text = Driver_code.Uptae;
                cmb_AddressState.SelectedValue = Driver_code.AddressState;
              

               

                cmb_AddressCity.SelectedValue = Driver_code.AddressCity;

                cmb_ParkState.SelectedValue = Driver_code.AddressState;
                cmb_ParkCity.SelectedValue = Driver_code.AddressCity;
               
                txt_Addr.Text = Driver_code.AddressDetail;
            }
            else
            {
                txt_Name.Text = string.Empty;
                txt_CEO.Text = string.Empty;
                txt_Upjong.Text = string.Empty;
                txt_Uptae.Text = string.Empty;
              
                cmb_AddressState.SelectedIndex = 0;
                cmb_AddressCity.SelectedIndex = 0;
                txt_Addr.Text = string.Empty;
            }



        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            int i = 0;
            i = txt_MobileNo.Text.Length;
            
            //txt_Password.Text = "m" + txt_MobileNo.Text.Replace("-","").Replace(" ","");

            if (txt_MobileNo.Text.Length > 11)
            {
                txt_Password.Text = txt_MobileNo.Text.Substring(txt_MobileNo.Text.Length - 4, 4);
            }
        }

        private void cmb_InsuranceType_SelectedValueChanged(object sender, EventArgs e)
        {
            //if (cmb_InsuranceType.SelectedValue.ToString() == "1")
            //{
            //    pn_Insurance.Visible = true;
            //}
            //else
            //{
            //    pn_Insurance.Visible = false;
            //}
        }

        private void cmb_AddressCity_SelectedIndexChanged(object sender, EventArgs e)
        {
           


            cmb_ParkState.SelectedValue = cmb_AddressState.SelectedValue;
            //cmb_ParkCity.SelectedValue = cmb_AddressCity.SelectedValue;
        }

        private void cmb_CarGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_CarGubun.SelectedIndex == 3)
            {
                //chk_Cont.Checked = true;
                //chk_Cont.Enabled = false;
              //  panel6.Visible = true;
                panel6.Visible = true;
            }
            else
            {
                //if (chk_Cont.Checked == true)
                //{
                //    chk_Cont.Checked = true;
                //}
                //else
                //{
                //    chk_Cont.Checked = false;
                //}
                //chk_Cont.Enabled = true;
                panel6.Visible = false;
              //  panel6.Enabled = false;
            }
        }

        private void btn_driverInfo_Click(object sender, EventArgs e)
        {
            FrmDriverSearch _frmDriverSearch = new FrmDriverSearch();
            _frmDriverSearch.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmDriverSearch.grid1.SelectedCells.Count == 0) return;
                if (_frmDriverSearch.grid1.SelectedCells[0].RowIndex < 0) return;

                txt_CarYear.Text = _frmDriverSearch.grid1[1, _frmDriverSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                //   txt_DriverMobileNo.Text = _frmDriverSearch.grid1[2, _frmDriverSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();


                _frmDriverSearch.Close();
            });
            _frmDriverSearch.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmDriverSearch.grid1.SelectedCells.Count == 0) return;
                if (_frmDriverSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                txt_CarYear.Text = _frmDriverSearch.grid1[1, _frmDriverSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

               
                _frmDriverSearch.Close();
            });






            _frmDriverSearch.Owner = this;
            _frmDriverSearch.StartPosition = FormStartPosition.CenterParent;
            _frmDriverSearch.ShowDialog();
        }

        private void btn_DriverExcel_Click(object sender, EventArgs e)
        {
            string title = string.Empty;
            byte[] ieExcel;

            string fileString = string.Empty;
           
            clientsTableAdapter.Fill(this.cMDataSet.Clients);
            var Query = cMDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();
            if (Query.First().DriverType == 1)
            {
                fileString = "차량관리입력양식(기본)_" + DateTime.Now.ToString("yyyyMMdd");
                title = "sheet1";
                ieExcel = Properties.Resources.차량관리입력양식_기본__Blank;
            }
            else
            {
                fileString = "차량관리입력양식(표준)_" + DateTime.Now.ToString("yyyyMMdd");
                title = "sheet1";
                ieExcel = Properties.Resources.차량관리입력양식_표준__XLSX;
            }



            pnProgress.Visible = true;
            bar.Value = 0;
            Thread t = new Thread(new ThreadStart(() =>
            {


                dataGridView1.ExportExistExcel2_xlsx(title, fileString, bar, true, ieExcel, 3, LocalUser.Instance.PersonalOption.DRIVER);
                pnProgress.Invoke(new Action(() => pnProgress.Visible = false));





            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        private string getValueFrom2DArray(object[,] array, int index)
        {
            int intD = array.GetUpperBound(1);
            try
            {
                return array[index, intD].ToString();
            }
            catch { return string.Empty; }
        }
        //private void btnExcelImport_Click(object sender, EventArgs e)
        //{
        //    DataGridViewColumn[] requiereds = new DataGridViewColumn[]{
                   
        //        codeDataGridViewTextBoxColumn,
        //        bizNoDataGridViewTextBoxColumn,
        //        nameDataGridViewTextBoxColumn,
        //        cEODataGridViewTextBoxColumn,
        //        uptaeDataGridViewTextBoxColumn,
        //        upjongDataGridViewTextBoxColumn,
        //         mobileNoDataGridViewTextBoxColumn,

        //        addressStateDataGridViewTextBoxColumn,
        //        addressCityDataGridViewTextBoxColumn,
        //        addressDetailDataGridViewTextBoxColumn,
               
               
                
               
        //        carNoDataGridViewTextBoxColumn,
        //        carTypeDataGridViewTextBoxColumn,
        //        carSizeDataGridViewTextBoxColumn,
        //        Gubun,
        //        carYearDataGridViewTextBoxColumn,
        //     parkStateDataGridViewTextBoxColumn,
             
        //     parkCityDataGridViewTextBoxColumn,
        //     parkStreetDataGridViewTextBoxColumn};
        //    try
        //    {
        //        Dictionary<DataGridViewColumn, object[,]> dic = new Dictionary<DataGridViewColumn, object[,]>();
        //        dic.Add(codeDataGridViewTextBoxColumn, null);
        //        dic.Add(bizNoDataGridViewTextBoxColumn, null);
        //        dic.Add(nameDataGridViewTextBoxColumn, null);
        //        dic.Add(cEODataGridViewTextBoxColumn, null);
        //        dic.Add(uptaeDataGridViewTextBoxColumn, null);
        //        dic.Add(upjongDataGridViewTextBoxColumn, null);
        //        dic.Add(mobileNoDataGridViewTextBoxColumn, null);




        //        dic.Add(addressStateDataGridViewTextBoxColumn, null);
        //        dic.Add(addressCityDataGridViewTextBoxColumn, null);
        //        dic.Add(addressDetailDataGridViewTextBoxColumn, null);

        //        dic.Add(emailDataGridViewTextBoxColumn, null);
        //        dic.Add(phoneNoDataGridViewTextBoxColumn, null);
        //        dic.Add(faxNoDataGridViewTextBoxColumn, null);


        //        // dic.Add(passwordDataGridViewTextBoxColumn, null);
        //        dic.Add(bizTypeDataGridViewTextBoxColumn, null);
        //        dic.Add(RouteType, null);
        //        dic.Add(insuranceTypeDataGridViewTextBoxColumn, null);

        //        dic.Add(carNoDataGridViewTextBoxColumn, null);
        //        dic.Add(carTypeDataGridViewTextBoxColumn, null);
        //        dic.Add(carSizeDataGridViewTextBoxColumn, null);
        //        dic.Add(Gubun, null);
        //        dic.Add(carYearDataGridViewTextBoxColumn, null);

        //        dic.Add(parkStateDataGridViewTextBoxColumn, null);
        //        dic.Add(parkCityDataGridViewTextBoxColumn, null);
        //        dic.Add(parkStreetDataGridViewTextBoxColumn, null);
        //        //  dic.Add(loginIdDataGridViewTextBoxColumn, null);
        //        dic.Add(CandidateId, null);
        //        dic.Add(FpisCarType, null);

        //        FrmExcel f = new FrmExcel("차량관리", requiereds, ref dic);
        //        #region t
        //        Thread t = new Thread(new ThreadStart(() =>
        //        {
        //            f.Invoke(new Action(() =>
        //            {
        //                f.btnClose.Enabled = false;
        //                f.btnNext.Enabled = false;
        //                f.btnPrevious.Enabled = false;
        //            }));
        //            int i = 1;
        //            CMDataSet.DriversRow editRow;
        //            string lbl1 = f.lblStep3_1.Text;
        //            string lbl2 = f.lblStep3_2.Text;
        //            DateTime o = DateTime.Now;
        //            decimal d = 0;
        //            int iSum = 0, iSkp = 0, iNew = 0;
        //            while (true)
        //            {


        //                //클라이언트코드에 값이 없으면 끝이난다.
        //                //   if (dic[codeDataGridViewTextBoxColumn].GetUpperBound(0) < i) break;
        //                decimal oAmt;
        //                //필수항목인 클라이언트 코드와, 입금액을 조사하여 부족하면 진행하지 않는다.
        //                // string sAmt = getValueFrom2DArray(dic[AmtMoneyColumn], i);
        //                //  string sCode = getValueFrom2DArray(dic[codeDataGridViewTextBoxColumn], i);

        //                string sCode;
        //                var Driver_code = cMDataSet.Drivers.Select(c => new { c.Code }).OrderByDescending(c => c.Code).ToArray();
        //                if (Driver_code.Count() > 0)
        //                {
        //                    var DriverCode = 100001;
        //                    var DriverCodeCandidate = cMDataSet.Drivers.OrderBy(c => c.Code).Select(c => c.Code).ToArray();
        //                    while (true)
        //                    {
        //                        if (!DriverCodeCandidate.Any(c => c == DriverCode.ToString()))
        //                        {
        //                            break;
        //                        }
        //                        DriverCode++;
        //                    }
        //                    sCode = DriverCode.ToString();
        //                }
        //                else
        //                {

        //                    sCode = "100001";
        //                }


        //                string sbizNo = getValueFrom2DArray(dic[bizNoDataGridViewTextBoxColumn], i);
        //                if (sbizNo == "사업자번호")
        //                {
        //                    i++;
        //                    continue;

        //                }
        //                if (sbizNo == "") break;
        //                string sName = getValueFrom2DArray(dic[nameDataGridViewTextBoxColumn], i);
        //                string sCeo = getValueFrom2DArray(dic[cEODataGridViewTextBoxColumn], i);
        //                string sUptae = getValueFrom2DArray(dic[uptaeDataGridViewTextBoxColumn], i);
        //                string sUpjong = getValueFrom2DArray(dic[upjongDataGridViewTextBoxColumn], i);
        //                string smobileNo = getValueFrom2DArray(dic[mobileNoDataGridViewTextBoxColumn], i);

        //                string S_BizNo = string.Empty;
        //                string BiznoId = string.Empty;
        //                string sPassword = string.Empty;

        //                if (sbizNo.Length == 10)
        //                {
        //                    S_BizNo = sbizNo.Substring(5, 5);
        //                }
        //                else if(sbizNo.Length == 12)
        //                {
        //                    S_BizNo = sbizNo.Substring(7, 5);
        //                }
        //                //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //                //{
        //                //    SqlCommand selectCmd = new SqlCommand(
        //                //        @"SELECT Count(*) FROM Drivers Where right(BizNo,5) = @Bizno ", cn);
        //                //    selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
        //                //    // selectCmd.Parameters.Add(new SqlParameter("@UserPassword", txtPassword.Text));
        //                //    cn.Open();
        //                //    object o = selectCmd.ExecuteScalar();
        //                //    if (o != null)
        //                //        r = Convert.ToInt16(o);
        //                //    cn.Close();
        //                //}


        //                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //                {
        //                    cn.Open();

        //                    SqlCommand selectCmd = new SqlCommand(
        //                        @"SELECT top 1 convert(int,right(LoginId,3)+1) as LoginId FROM Drivers Where right(BizNo,5) = @Bizno order by loginid desc", cn);
        //                    selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
        //                    var Reader = selectCmd.ExecuteReader();
        //                    while (Reader.Read())
        //                    {

        //                        BiznoId = Reader["LoginId"].ToString();

        //                    }
        //                    if (String.IsNullOrEmpty(BiznoId))
        //                    {
        //                        BiznoId = "";

        //                    }
        //                    cn.Close();
        //                }
        //                string BizNo = string.Empty;
        //                if (BiznoId == "")
        //                {
        //                    BizNo = "001";

        //                }
        //                else if (BiznoId.Length == 2)
        //                {
        //                    BizNo = "0" + BiznoId;
        //                }
        //                else if (BiznoId.Length == 1)
        //                {
        //                    BizNo = "00" + BiznoId;
        //                }
        //                else if (BiznoId.Length == 3)
        //                {
        //                    BizNo = BiznoId;
        //                }


        //                string sLoginId = "m" + sbizNo.Substring(5, 5) + BizNo;

        //                if (smobileNo.Length > 10)
        //                {
        //                    sPassword = smobileNo.Substring(smobileNo.Length - 4, 4);
        //                }





        //                //if (Decimal.TryParse(sAmt.Replace(",", ""), out oAmt) == false)
        //                //{
        //                //    i++;
        //                //    continue;
        //                //}
        //                //나머지 값들 넣어보고
        //                string semail = getValueFrom2DArray(dic[emailDataGridViewTextBoxColumn], i);
        //                string sphoneNo = getValueFrom2DArray(dic[phoneNoDataGridViewTextBoxColumn], i);
        //                string sfaxNo = getValueFrom2DArray(dic[faxNoDataGridViewTextBoxColumn], i);


        //                DateTime sCreateDate = DateTime.Now;

        //                string saddressState = getValueFrom2DArray(dic[addressStateDataGridViewTextBoxColumn], i);
        //                string saddressCity = getValueFrom2DArray(dic[addressCityDataGridViewTextBoxColumn], i);
        //                string saddressDetail = getValueFrom2DArray(dic[addressDetailDataGridViewTextBoxColumn], i);

        //                string sbizType = getValueFrom2DArray(dic[bizTypeDataGridViewTextBoxColumn], i);
        //                string sRouteType = getValueFrom2DArray(dic[RouteType], i);
        //                string sinsuranceType = getValueFrom2DArray(dic[insuranceTypeDataGridViewTextBoxColumn], i);
        //                string sUsePayNow = "0";


        //                string scarNo = getValueFrom2DArray(dic[carNoDataGridViewTextBoxColumn], i);
        //                string scarType = getValueFrom2DArray(dic[carTypeDataGridViewTextBoxColumn], i);
        //                string scarSize = getValueFrom2DArray(dic[carSizeDataGridViewTextBoxColumn], i);
        //                string sGubun = getValueFrom2DArray(dic[Gubun], i);

        //                string scarYear = getValueFrom2DArray(dic[carYearDataGridViewTextBoxColumn], i);


        //                string sparkState = getValueFrom2DArray(dic[parkStateDataGridViewTextBoxColumn], i);
        //                string sparkCity = getValueFrom2DArray(dic[parkCityDataGridViewTextBoxColumn], i);
        //                string sparkStreet = getValueFrom2DArray(dic[parkStreetDataGridViewTextBoxColumn], i);
        //                int sCandidateId = LocalUser.Instance.LogInInformation.ClientId;
        //                string sFpisCartype = getValueFrom2DArray(dic[FpisCarType], i);
        //                iSum++;


        //                //이제 추가하고
        //                try
        //                {


        //                    CMDataSet.DriversRow iRow = cMDataSet.Drivers.NewDriversRow();
        //                    iRow.Code = sCode;
        //                    iRow.Name = sName;
        //                    iRow.BizNo = sbizNo.Replace("-", "");
        //                    iRow.CEO = sCeo;
        //                    iRow.Uptae = sUptae;
        //                    iRow.Upjong = sUpjong;
        //                    iRow.Email = semail;
        //                    iRow.LoginId = sLoginId;
        //                    iRow.Password = sPassword;

        //                    if (smobileNo.Length < 10)
        //                    {
        //                        iRow.MobileNo = "";
        //                    }
        //                    else
        //                    {
        //                        iRow.MobileNo = smobileNo;
        //                    }


        //                    if (sphoneNo.Length < 9)
        //                    {
        //                        iRow.PhoneNo = "";
        //                    }
        //                    else
        //                    {
        //                        iRow.PhoneNo = sphoneNo;
        //                    }

        //                    if (sfaxNo.Length < 11)
        //                    {
        //                        iRow.FaxNo = "";
        //                    }

        //                    else
        //                    {
        //                        iRow.FaxNo = sfaxNo;
        //                    }

        //                    iRow.CreateDate = sCreateDate;

        //                    iRow.AddressState = saddressState;
        //                    iRow.AddressCity = saddressCity;
        //                    iRow.AddressDetail = saddressDetail;

        //                    //if (String.IsNullOrEmpty(sbizType))
        //                    //{
        //                    //    iRow.BizType = 1;
        //                    //}
        //                    //else
        //                    //{
        //                    //    iRow.BizType = int.Parse(sbizType);
        //                    //}

        //                    var QBizType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BizType" && c.Name == scarType).ToArray();

        //                    if (QBizType.Any())
        //                    {
        //                        iRow.BizType = QBizType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.BizType = 1;
        //                    }
        //                    //if (String.IsNullOrEmpty(sRouteType))
        //                    //{
        //                    //    iRow.RouteType = 1;
        //                    //}
        //                    //else
        //                    //{
        //                    //    iRow.RouteType = int.Parse(sRouteType);
        //                    //}

        //                    var QRouteType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "RouteType" && c.Name == scarType).ToArray();

        //                    if (QRouteType.Any())
        //                    {
        //                        iRow.RouteType = QRouteType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.RouteType = 1;
        //                    }

        //                    var QInsuranceType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InsuranceType" && c.Name == scarType).ToArray();

        //                    if (QInsuranceType.Any())
        //                    {
        //                        iRow.InsuranceType = QInsuranceType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.InsuranceType = 1;
        //                    }
        //                    //if (String.IsNullOrEmpty(sinsuranceType))
        //                    //{
        //                    //    iRow.InsuranceType = 0;
        //                    //}

        //                    //else
        //                    //{
        //                    //    iRow.InsuranceType = int.Parse(sinsuranceType);

        //                    //}
        //                    iRow.CarNo = scarNo.Replace(" ", "");
        //                    var QcarType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && c.Name == scarType).ToArray();

        //                    if (QcarType.Any())
        //                    {
        //                        iRow.CarType = QcarType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.CarType = 5;
        //                    }

        //                    var QcarSize = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Name == scarSize).ToArray();
        //                    if (QcarSize.Any())
        //                    {
        //                        iRow.CarSize = QcarSize.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.CarSize = 1;
        //                    }
        //                    var Qgubun = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarGubun" && c.Name == sGubun).ToArray();
        //                    if (Qgubun.Any())
        //                    {
        //                        iRow.CarGubun = Qgubun.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.CarGubun = 1;
        //                    }
        //                    iRow.CarYear = scarYear;
        //                    iRow.ParkState = sparkState;
        //                    iRow.ParkCity = sparkCity;
        //                    iRow.ParkStreet = sparkStreet;

        //                    iRow.AccountOwner = "";
        //                    iRow.AccountRegNo = "";
        //                    iRow.BankName = "";
        //                    iRow.AccountNo = "";
        //                    iRow.AccountExtra = "";
        //                    if (String.IsNullOrEmpty(sUsePayNow))
        //                    {
        //                        iRow.UsePayNow = 2;
        //                    }
        //                    else
        //                    {
        //                        iRow.UsePayNow = int.Parse(sUsePayNow);
        //                    }
        //                    iRow.ClientBizType = 0;
        //                    //     row.CandidateId = int.Parse(cmb_CandidateGubun.SelectedValue.ToString());
        //                    iRow.CandidateId = LocalUser.Instance.LogInInformation.ClientId;

        //                    iRow.Car_ContRact = false;


        //                    iRow.AccountUse = false;
        //                    iRow.DTGUse = true;
        //                    iRow.FPISUse = true;
        //                    iRow.MyCallUSe = true;
        //                    iRow.OTGUse = false;
        //                    iRow.ServicePrice = "5500";
        //                    iRow.useTax = true;
        //                    iRow.OTGPrice = 0;
        //                    iRow.AccountPrice = 0;
        //                    iRow.FPISPrice = 0;
        //                    iRow.MyCallPrice = 0;
        //                    iRow.DTGPrice = 5000;
                           
        //                    var QFPisCarType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "FPisCarType" && c.Name == sFpisCartype).ToArray();
        //                    if (QFPisCarType.Any())
        //                    {
        //                        iRow.FpisCarType = QFPisCarType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.FpisCarType = 1;
        //                    }

        //                    //driversTableAdapter.Insert(sbizNo, sName, sCeo, sUptae, sUpjong, saddressState, saddressCity, smobileNo, sPassword, sphoneNo, sphoneNo, semail, int.Parse(sbizType), int.Parse(sRouteType), int.Parse(sinsuranceType), sCreateDate, scarNo, int.Parse(scarType), int.Parse(scarSize), scarYear, sparkState, sparkCity, "", "", "", "", "", "", "",int.Parse(null), sCode, sparkStreet, 0, sLoginId, saddressDetail, sCandidateId, 0);

        //                    cMDataSet.Drivers.AddDriversRow(iRow);

        //                    driversTableAdapter.Update(iRow);

        //                    iNew++;

        //                }
        //                catch { iSkp++; }
        //                try
        //                {
        //                    f.Invoke(new Action(() =>
        //                    {
        //                        f.pBar.Value++;
        //                        f.lblProgress.Text = string.Format("{0}/{1}", f.pBar.Value, f.pBar.Maximum);
        //                    }));
        //                }
        //                catch { }
        //                i++;
        //            }
        //            f.Invoke(new Action(() =>
        //            {
        //                f.picStep3_1.Image = Properties.Resources.Done;
        //                f.picStep3_2.Width = 32;
        //                f.lblStep3_1.Text = lbl1;
        //                f.Cursor = Cursors.WaitCursor;
        //            }));
        //            f.Invoke(new Action(() =>
        //            {
        //                f.picStep3_2.Image = Properties.Resources.Done;
        //                f.lblStep3_2.Text = lbl2;
        //                f.Cursor = Cursors.Arrow;
        //                f.lblSum.Text = iSum.ToString() + "건";
        //                f.lblSkip.Text = iSkp.ToString() + "건";
        //                f.lblWrite.Text = (iSum - iSkp - iNew).ToString() + "건";
        //                f.lblNew.Text = iNew.ToString() + "건";
        //                f.tableLayoutPanel5.Visible = true;
        //                f.btnNext.Text = "완료";
        //                f.DicDone = false;
        //            }));
        //            f.Invoke(new Action(() =>
        //            {
        //                f.btnClose.Enabled = true;
        //                f.btnNext.Enabled = true;
        //                f.btnPrevious.Enabled = true;
        //            }));
        //        }));
        //        t.SetApartmentState(ApartmentState.STA);
        //        #endregion

        //        f.btnNext.Click += new EventHandler((object bsender, EventArgs be) =>
        //        {
        //            try
        //            {
        //                if (f.IsClose) return;
        //                if (f.CurrentIndex != 2) return;
        //                while (f.DicDone == false)
        //                    Thread.Sleep(100);
        //                t.Start();
        //            }
        //            catch { }
        //        });
        //        f.ShowDialog();
        //       // this.Invoke(new Action(() => btn_Search_Click(null, null)));
        //    }
        //    catch
        //    {
        //        (this as Form).ThisMessageBox("엑셀 불러오기가 실패하였습니다.");
        //    }

        //}

        private void btnExcelImport_Click_1(object sender, EventArgs e)
        {



            EXCELINSERT_DRIVER _Form = new EXCELINSERT_DRIVER();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );

        }



    }
}
