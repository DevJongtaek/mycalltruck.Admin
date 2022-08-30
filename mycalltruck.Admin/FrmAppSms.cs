using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;

using mycalltruck.Admin.UI;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmAppSms : Form
    {
        int SMSCOUNT = 0;
        string Now = DateTime.Now.Date.ToString("yyyy-MM-dd").Replace("-", "/");
        string SMSLastText = "";
        string SmsBaseText = "";
        string SmsMiddleText = "";
        string SmsBaseViewText = "";
        string Remark = "";
        int DriverId = 0;
        string DefaultText = "";
        string SMSGubun, SMSText;
        string SMSWriteText = "";

        DataSets.AppSMSDataSetTableAdapters.em_mmt_tranTableAdapter em_mmt_tranTableAdapter = new DataSets.AppSMSDataSetTableAdapters.em_mmt_tranTableAdapter();
        DataSets.AppSMSDataSetTableAdapters.em_smt_tranTableAdapter em_smt_tranTableAdapter = new DataSets.AppSMSDataSetTableAdapters.em_smt_tranTableAdapter();

        public FrmAppSms()
        {
            InitializeComponent();

            InitClientTable();
            InitDriverTable();
            InitAppSMSTable();
            InitAppSMSMessageTable();

            dtpStart.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtpEnd.Value = DateTime.Now;

            cmb_Search.SelectedIndex = 0;

            clientSmsCountTableAdapter.Fill(clientDataSet.ClientSmsCount);



        }
        private void FrmAppSms_Load(object sender, EventArgs e)
        {
            DefaultText = "안녕하십니까?" +
                   "\r\n" +
                   "당 社에서는 배차와는 무관하게 모든 \"전자세금계산서\" 및 \"전자인수증\"을 스마트폰으로 발행(주선/운송/화주)할 수 있는 어플(차세로)을 개발하여 시행함을 알려드립니다." +
                    "\r\n\r\n" +
                   "■ 발행(수금) 내역 체계적 통합관리" +
                   "\r\n" +
                   "■ 종이를 전자로 대체, 비용 절감" +
                   "\r\n" +
                   "■ 우체국에 내방 불필요,  시간 절약" +
                   "\r\n" +
                   "■ 차주 공인인증서 불필요"+
                     "\r\n" +
                       "\r\n" +
                    "★ \"차세로\" 어플설치" +
                    "\r\n" +
                    "1.아래 버튼을 클릭하여 \"어플설치\"" +
                     "\r\n" +
                    "☞ 클릭: https://play.google.com/store/apps/details?id=edubill.m.newcardpay.kr"+
                     "\r\n" +
                     "2.설치된 어플에서 \"회원가입\" " +
                    "\r\n" +
                    //" ※. 처음 가입 시, 3,000포인트 무료충전." +
                    //"\r\n" +
                    "3.가입 후, 당일부터 \"사용\" 가능" +
                    "\r\n" +
                    " ※. 사용료는 1,100원/건 차감." +
                     "\r\n\r\n" +
                    "차세로 고객센터" +
                    "\r\n" +
                    "☎ 02 - 853 - 5111";






            Search();

            txtSMSText();
            txtSmsWriteText();

            this.tablesTableAdapter.Fill(baseDataSet.TABLES);

            //txtSmsWrite.BackColor = Color.LightGray;
            //txtSmsWrite.ForeColor = Color.Gray;
            //txtSmsWrite.ReadOnly = true;
            txtMobileNo.Focus();

            var Query = clientDataSet.ClientSmsCount.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.SendGubun == "App설치");
            if (Query.Any())
            {
                lblCount.Text = Query.Where(c => c.CreateDate.Date == DateTime.Now.Date).Sum(c => c.DailyCnt).ToString() + "/500";

                if (Query.Where(c => c.CreateDate.Date == DateTime.Now.Date).Sum(c => c.DailyCnt) >= 500)
                {
                    MessageBox.Show("오늘 하루 전송할 수 있는 \r\n발송 건(500건)이 초과 되었습니다.\r\n익일에 전송 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnOk.Enabled = false;
                }
                else
                {
                    btnOk.Enabled = true;

                }
                btnOk.Enabled = true;
            }
            else
            {
                lblCount.Text = "0/500";
                btnOk.Enabled = true;

            }

            

        }
        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string PhoneNo { get; set; }
        }
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
        }
        class AppSMSViewModel
        {
            public int Idx { get; set; }
            public string MobileNo { get; set; }
            public string Name { get; set; }
            public int ClientId { get; set; }
            public DateTime CreateDate { get; set; }
            public string SMSYN { get; set; }
            public string SMSSendDate { get; set; }
        }

        class APPSMSMessageViewModel
        {
            public int Idx { get; set; }

            public int ClientId { get; set; }

            public string Message { get; set; }
        }


        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();
        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();
        private List<AppSMSViewModel> _AppSMSTable = new List<AppSMSViewModel>();
        private List<APPSMSMessageViewModel> _APPSMSMessageTable = new List<APPSMSMessageViewModel>();

        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name, LoginId,ISNULL(PhoneNo,00-0000-0000)PhoneNo FROM Clients ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientTable.Add(
                              new ClientViewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                                  PhoneNo = dataReader.GetString(4),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,MobileNo,CarNo,CarYear FROM Drivers ";

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
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private void InitAppSMSTable()
        {
            _AppSMSTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT idx, MobileNo,ISNULL(Name,'') AS Name ,ISNULL(ClientId,0) AS ClientId, CreateDate, SMSYN, ISNULL(SMSSendDate,'') SMSSendDate FROM AppSMS ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _AppSMSTable.Add(
                              new AppSMSViewModel
                              {
                                  Idx = dataReader.GetInt32(0),
                                  MobileNo = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  ClientId = dataReader.GetInt32(3),
                                  CreateDate = dataReader.GetDateTime(4),
                                  SMSYN = dataReader.GetString(5),
                                  SMSSendDate = dataReader.GetString(6),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private void InitAppSMSMessageTable()
        {
            _APPSMSMessageTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT idx, ClientId, Message FROM APPSMSMessage ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _APPSMSMessageTable.Add(
                              new APPSMSMessageViewModel
                              {
                                  Idx = dataReader.GetInt32(0),

                                  ClientId = dataReader.GetInt32(1),
                                  Message = dataReader.GetString(2),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        private void txtSMSText()
        {

            //  var Client = _ClientTable.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
            var Query = _APPSMSMessageTable.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();
            //해당운송사의 메세지가 있을때

            if (Query.Any())
            {
                txtSmsDefault.Text = Query.First().Message;
            }
            //없을때
            else
            {
                txtSmsDefault.Text = DefaultText;
            }

        }

        private void txtSmsWriteText()
        {
            //var Client = _ClientTable.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);

            //SmsBaseText = "★ \"차세로\" 어플설치" +              
            //    "\r\n" +
            //    "1.아래 버튼을 클릭하여 \"어플설치\""+
            //     "\r\n" +
            //    "☞ 클릭: https://play.google.com/store/apps/details?id=edubill.m.newcardpay.kr";

            //SmsBaseViewText = "★ \"차세로\" 어플설치" +
            //    "\r\n" +
            //    "1.아래 버튼을 클릭하여 \"어플설치\"" +
            //     "\r\n" +
            //    "☞ 클릭: https://play.google.com/store/apps/details?id=edubill.m.newcardpay.kr";

            
            //SmsMiddleText =
                //"2.설치된 어플에서 \"회원가입\" " +
                //"\r\n" +
                //" ※. 처음 가입 시, 3,000포인트 무료충전." +
                //"\r\n" +
                //"3.가입 후, 당일부터 \"사용\" 가능" +
                //"\r\n" +
                //" ※. 사용료는 500원/건 차감." +
                // "\r\n\r\n" +
                //"차세로 고객센터" +
                //"\r\n" +
                //"☎ 02 - 853 - 5111";
                

            //if (rdo_LMS.Checked)
            //{
            //    SMSWriteText = SmsBaseViewText + "\r\n\r\n" + SmsMiddleText + "\r\n\r\n";
            //}
            //else
            //{
            //    SMSWriteText = SmsBaseText;
            //}
            SMSWriteText = DefaultText;



        }

        BindingList<(int idx, String MobileNo, String Name, int ClientId)> DataListSource = new BindingList<(int, string, string, int)>();

        public static string EncryptSHA512(string Data)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        string SmsResult;

        private void btnOk_Click(object sender, EventArgs e)
        {
            Remark = "";
            int successcount = 0;
            int failcount = 0;
            string SVC_RT;
            string SVC_MSG;
            List<string> _SVC_RT = new List<string>();

            int Count = 0;
            int TotalCount = 0;
            var Query = clientDataSet.ClientSmsCount.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.SendGubun == "App설치");
            //보낼수있는건수 500-Query
            int SendUseCount = 500 - Query.Where(c => c.CreateDate.Date == DateTime.Now.Date).Sum(c => c.DailyCnt);

            foreach (DataGridViewRow Row in mDataGridView.Rows.Cast<DataGridViewRow>().Where(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
            {
                TotalCount++;
            }
            if (TotalCount == 0)
            {
                MessageBox.Show("전송할 건을 선택하십시오", "APP 설치문자");
                return;
            }
            else if (TotalCount > SendUseCount)
            {
                MessageBox.Show("당일 보낼수 있는 문자메세지는 " + SendUseCount + " 건 입니다.\r\n" + SendUseCount + " 건 만 전송됩니다. ", "APP 설치문자");

            }
            //당일 전송 건 체크
            InitAppSMSTable();


            SMSText = SMSWriteText;

            //DialogAppSms _DialogAppSms = new DialogAppSms(SMSGubun, SMSText,"", TotalCount,"", "");
            //_DialogAppSms.Owner = this;
            //_DialogAppSms.StartPosition = FormStartPosition.CenterParent;


            pnProgress.Visible = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Data.Connection((_Connection) =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "UPDATE AppSMS SET SMSYN = 'Y',Remark = @Remark ,SMSSendDate = '" + Now + "',CreateDate = getdate(),DriverId = @DriverId WHERE idx = @idx";
                            _Command.Parameters.Add("@idx", SqlDbType.Int);
                            _Command.Parameters.Add("@Remark", SqlDbType.NVarChar);
                            _Command.Parameters.Add("@DriverId", SqlDbType.Int);


                            foreach (DataGridViewRow Row in mDataGridView.Rows.Cast<DataGridViewRow>().Where(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                            {
                                if (SendUseCount == Count)
                                {
                                    break;
                                }

                                (int idx, String MobileNo, String Name, int ClientId) = DataListSource[Row.Index];
                                _Command.Parameters["@idx"].Value = idx;

                                // if (_Command.ExecuteNonQuery() > 0)

                                if (Regex.IsMatch(MobileNo.Replace("-", ""), @"^01[0,1,6,7,8,9]\d{3,4}\d{4}$"))
                                {
                                    string Message = "";

                                    //MMS일때

                                    Message = $"{SMSWriteText}";


                                    if (chk_OverLap.Checked)
                                    {
                                        var APPQ = _AppSMSTable.Where(c => c.CreateDate.ToString("d") == DateTime.Now.ToString("d") && c.SMSYN == "Y" && c.MobileNo == MobileNo).ToArray();
                                        var APPU = _DriverTable.Where(c => c.AppUse == true && c.MobileNo.Replace("-", "") == MobileNo).ToArray();
                                        // //차세로 어플 설치 건이면 전송 안함.
                                        if (APPU.Count() > 0)
                                        {
                                            Remark = "어플설치";
                                            DriverId = APPU.First().DriverId;

                                        }
                                        //당일 발송건 전송안함
                                        else if (APPQ.Any())
                                        {
                                            Remark = "당일발송";
                                            
                                        }
                                        else
                                        {
                                            if (MobileNo.Replace("-", "").Length >= 10)
                                            {

                                                string sha512password = EncryptSHA512(CallHelper.Instance.Password);




                                                string Parameter = "";
                                                Parameter = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", CallHelper.Instance.Id, sha512password, MobileNo.Replace("-", ""), Message);




                                                JObject response = null;

                                                var uriBuilder = new UriBuilder("https://centrex.uplus.co.kr/RestApi/smssend");






                                                uriBuilder.Query = Parameter;
                                                Uri finalUrl = uriBuilder.Uri;


                                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);



                                                request.Method = "POST";
                                                request.ContentType = "text/json;";
                                                request.ContentLength = 0;

                                                request.Headers.Add("header-staff-api", "value");

                                                var httpResponse = (HttpWebResponse)request.GetResponse();

                                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                                {
                                                    response = JObject.Parse(streamReader.ReadToEnd());

                                                    SVC_RT = response["SVC_RT"].ToString();
                                                    SVC_MSG = response["SVC_MSG"].ToString();

                                                    if (SVC_RT == "0000")
                                                    {
                                                        SmsResult = "성공";
                                                        successcount++;
                                                    }
                                                    else
                                                    {
                                                        SmsResult = "실패";
                                                        failcount++;
                                                        _SVC_RT.Add(SVC_RT);
                                                    }

                                                }



                                                using (SqlCommand _Command2 = _Connection.CreateCommand())
                                                {
                                                    _Command2.CommandText =
                                                    "INSERT INTO CallSms (CTime, OriginalPhoneNo, SmsResult, ResultMessage, ClientId,LoginId,CustomerId,DriverId,MSG) VALUES (@CTime, @OriginalPhoneNo, @SmsResult, @ResultMessage, @ClientId,@LoginId,@CustomerId,@DriverId,@Msg)";

                                                    _Command2.Parameters.AddWithValue("@CTime", DateTime.Now);
                                                    _Command2.Parameters.AddWithValue("@OriginalPhoneNo", MobileNo.Replace("-", ""));
                                                    _Command2.Parameters.AddWithValue("@SmsResult", SmsResult);
                                                    _Command2.Parameters.AddWithValue("@ResultMessage", SVC_MSG);
                                                    _Command2.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                                    _Command2.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                                                    _Command2.Parameters.AddWithValue("@CustomerId", 0);
                                                    _Command2.Parameters.AddWithValue("@DriverId", DriverId);
                                                    _Command2.Parameters.AddWithValue("@Msg", Message);
                                                    _Command2.ExecuteNonQuery();

                                                }


                                            }


                                        }
                                    }
                                    else
                                    {
                                        if (MobileNo.Replace("-", "").Length >= 10)
                                        {

                                            string sha512password = EncryptSHA512(CallHelper.Instance.Password);




                                            string Parameter = "";
                                            Parameter = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", CallHelper.Instance.Id, sha512password, MobileNo.Replace("-", ""), Message);




                                            JObject response = null;

                                            var uriBuilder = new UriBuilder("https://centrex.uplus.co.kr/RestApi/smssend");


                                            uriBuilder.Query = Parameter;
                                            Uri finalUrl = uriBuilder.Uri;


                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);



                                            request.Method = "POST";
                                            request.ContentType = "text/json;";
                                            request.ContentLength = 0;

                                            request.Headers.Add("header-staff-api", "value");

                                            var httpResponse = (HttpWebResponse)request.GetResponse();

                                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                            {
                                                response = JObject.Parse(streamReader.ReadToEnd());

                                                SVC_RT = response["SVC_RT"].ToString();
                                                SVC_MSG = response["SVC_MSG"].ToString();

                                                if (SVC_RT == "0000")
                                                {
                                                    SmsResult = "성공";
                                                    successcount++;
                                                }
                                                else
                                                {
                                                    SmsResult = "실패";
                                                    failcount++;
                                                    _SVC_RT.Add(SVC_RT);
                                                }

                                            }



                                            using (SqlCommand _Command2 = _Connection.CreateCommand())
                                            {
                                                _Command2.CommandText =
                                                "INSERT INTO CallSms (CTime, OriginalPhoneNo, SmsResult, ResultMessage, ClientId,LoginId,CustomerId,DriverId,MSG) VALUES (@CTime, @OriginalPhoneNo, @SmsResult, @ResultMessage, @ClientId,@LoginId,@CustomerId,@DriverId,@Msg)";

                                                _Command2.Parameters.AddWithValue("@CTime", DateTime.Now);
                                                _Command2.Parameters.AddWithValue("@OriginalPhoneNo", MobileNo.Replace("-", ""));
                                                _Command2.Parameters.AddWithValue("@SmsResult", SmsResult);
                                                _Command2.Parameters.AddWithValue("@ResultMessage", SVC_MSG);
                                                _Command2.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                                _Command2.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                                                _Command2.Parameters.AddWithValue("@CustomerId", 0);
                                                _Command2.Parameters.AddWithValue("@DriverId", DriverId);
                                                _Command2.Parameters.AddWithValue("@Msg", Message);
                                                _Command2.ExecuteNonQuery();

                                            }


                                        }

                                    }
                                }
                                _Command.Parameters["@Remark"].Value = Remark;
                                _Command.Parameters["@DriverId"].Value = DriverId;
                                _Command.ExecuteNonQuery();




                            }


                        }



                    });




                    try
                    {
                        if (Remark == "어플설치")
                        {
                            Invoke(new Action(() => MessageBox.Show("어플이 설치된 차량입니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));
                        }
                        else if (Remark == "당일발송")
                        {
                            Invoke(new Action(() => MessageBox.Show("당일발송된 전화번호입니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));


                        }
                        else
                        {
                            if (_SVC_RT.Contains("1004"))
                            {
                                Invoke(new Action(() => MessageBox.Show("전화번호(LG U+)로 로그인에 실패 했습니다.\r\n\r\n“통화내역” 메뉴에서 “전화번호 설정”을 클릭,\r\n“비밀번호 초기화”를 한 후에\r\n다시 사용하시면 됩니다.", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information)));

                            }


                            Invoke(new Action(() => MessageBox.Show("SMS 전송 \r\n\r\n성공 : '" + successcount + "'건\r\n실패 : '" + failcount + "'건", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));
                        }




                    }
                    catch
                    {

                        Invoke(new Action(() => MessageBox.Show("연결된 인터넷전화(LG U+)가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));


                        ;

                    }





                }
                catch (Exception)
                {

                }
                Invoke(new Action(() => pnProgress.Visible = false));
            });






        }

        private void txt_SMS_KeyUp(object sender, KeyEventArgs e)
        {



        }

        private void txt_SMS_TextChanged(object sender, EventArgs e)
        {
            #region 텍스트 길이 체크
            try
            {
                //char[] msg_chars = this.txt_SMS.Text.ToCharArray();
                //int len = 0;
                //foreach (char msg_char in msg_chars)
                //{
                //    if (char.IsDigit(msg_char) || char.IsWhiteSpace(msg_char) || char.IsUpper(msg_char) || char.IsLower(msg_char))
                //    {
                //        len++;
                //    }
                //    else
                //    {
                //        len += 2;
                //    }
                //}
                //  this.label6.Text ="입력글자수 :"+ len.ToString() + "/80";
                //if (len > 80)
                //{
                //    MessageBox.Show("80 byte를 초과하여 입력할 수 없습니다.");
                //    this.txt_SMS.Text = this.txt_SMS.Text.Remove(txt_SMS.Text.Length - 1);
                //}
            }
            catch (Exception ex)
            {
                // MessageManager.ShowMessage(MessageType.Error, "오류가 발생했습니다.", ex, false);
            }
            #endregion
        }
        private void Search()
        {
            InitAppSMSTable();
            DataListSource.Clear();
            mDataGridView.AutoGenerateColumns = false;
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        "SELECT idx, MobileNo, ISNULL(Name,''), ClientId " +
                        "FROM AppSMS WHERE ClientId = @ClientId AND SMSYN = 'N' AND (SMSSendDate is null or SMSSendDate != '" + Now + "')" +
                        "ORDER BY idx DESC";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            DataListSource.Add((_Reader.GetInt32(0), _Reader.GetString(1), _Reader.GetString(2), _Reader.GetInt32(3)));
                        }
                    }
                }
            });
            mDataGridView.DataSource = DataListSource;


            clientSmsCountTableAdapter.Fill(clientDataSet.ClientSmsCount);
            var Query = clientDataSet.ClientSmsCount.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.SendGubun == "App설치");
            if (Query.Any())
            {
                lblCount.Text = Query.Where(c => c.CreateDate.Date == DateTime.Now.Date).Sum(c => c.DailyCnt).ToString() + "/500";

                if (Query.Where(c => c.CreateDate.Date == DateTime.Now.Date).Sum(c => c.DailyCnt) >= 500)
                {
                    MessageBox.Show("오늘 하루 전송할 수 있는 \r\n발송 건(500건)이 초과 되었습니다.\r\n익일에 전송 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnOk.Enabled = false;
                }
                else
                {
                    btnOk.Enabled = true;

                }

                btnOk.Enabled = true;
            }
           


            // DataListSource.ResetBindings();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public bool IsSuccess = false;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                //MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "추가", 1), " 추가 성공");
                IsSuccess = true;

                txtMobileNo.Text = "";
                Search();


            }
            else
            {

            }
        }
        private int _UpdateDB()
        {
            err.Clear();

            if (txtMobileNo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txtMobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            else
            {


                if (!Regex.IsMatch(txtMobileNo.Text.Replace("-", ""), @"^01[0,1,6,7,8,9]\d{3,4}\d{4}$"))
                {
                    MessageBox.Show("핸드폰번호 체계가 맞지않습니다.!!");
                    err.SetError(txtMobileNo, "핸드폰번호 체계가 맞지않습니다.!!");
                    return -1;
                }
                ////문자를 보냈을경우
                //if (_AppSMSTable.Where(c => c.CreateDate.ToString("d") ==DateTime.Now.ToString("d") && c.SMSYN=="Y" ).Any(c => c.MobileNo == txtMobileNo.Text))
                //{
                //    MessageBox.Show("당일 발송된 번호입니다.!!");
                //    err.SetError(txtMobileNo, "당일 발송된 번호입니다.!!");
                //    return -1;
                //}
                //문자는 안보냈지만 등록한 경우
                if (_AppSMSTable.Where(c => c.CreateDate.ToString("d") == DateTime.Now.ToString("d") && c.SMSYN == "N").Any(c => c.MobileNo == txtMobileNo.Text))
                {
                    MessageBox.Show("등록된 번호입니다.!!");
                    err.SetError(txtMobileNo, "등록된 번호입니다.!!");
                    return -1;
                }
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
                if (code == "MobileNo") iName = "핸드폰번호";


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
                        "INSERT AppSms (MobileNo,ClientId)Values(@MobileNo,@ClientId)";
                    cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

            }
            catch
            {
                MessageBox.Show("추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }

        private void mDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            (int idx, String MobileNo, String Name, int ClientId) = DataListSource[e.RowIndex];
            if (e.ColumnIndex == ColumnNo.Index)
            {
                e.Value = (mDataGridView.RowCount - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == ColumnMobileNo.Index)
            {
                if (MobileNo.Length == 11)
                {
                    e.Value = MobileNo.Substring(0, 3) + "-" + MobileNo.Substring(3, 4) + "-" + MobileNo.Substring(7, 4);
                }
                else if (MobileNo.Length == 10)
                {
                    e.Value = MobileNo.Substring(0, 3) + "-" + MobileNo.Substring(3, 3) + "-" + MobileNo.Substring(6, 4);
                }
                else
                {
                    e.Value = MobileNo;
                }
            }


            else if (e.ColumnIndex == ColumnName.Index)
                e.Value = Name;
            else if (e.ColumnIndex == ColumnClientId.Index)
                e.Value = ClientId;

        }

        private void AllSelect_Click(object sender, EventArgs e)
        {

            if (AllSelect.CheckState == CheckState.Checked)
            {
                foreach (DataGridViewRow Row in mDataGridView.Rows)
                {
                    Row.Cells[ColumnSelect.Index].Value = true;
                }
            }
            else if (AllSelect.CheckState == CheckState.Unchecked)
            {
                foreach (DataGridViewRow Row in mDataGridView.Rows)
                {
                    Row.Cells[ColumnSelect.Index].Value = false;
                }
            }
        }

        private void btnSaveForm_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog
            {
                DefaultExt = "xlsx",
                FileName = "APP설치문자입력.xlsx"
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(d.FileName, Properties.Resources.APP설치문자입력);
                Process.Start(d.FileName);
            }
        }

        private void ImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog
            {
                DefaultExt = "xlsx",
                FileName = "APP설치문자입력.xlsx"
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                int TotalCount = 0;
                int AddedCount = 0;
                Task SqlTask = new Task(() =>
                {
                    List<(string MobileNo, string Name)> ExcelList = new List<(string MobileNo, string Name)>();
                    ExcelPackage _Excel = null;
                    try
                    {
                        _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                        return;
                    }
                    if (_Excel.Workbook.Worksheets.Count < 1)
                    {
                        MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                        return;
                    }
                    var _Sheet = _Excel.Workbook.Worksheets[1];
                    int RowIndex = 2;
                    while (true)
                    {
                        var TestText = _Sheet.Cells[RowIndex, 1].Text;


                        if (String.IsNullOrEmpty(TestText))
                            break;
                       


                     
                        if (!Regex.IsMatch(_Sheet.Cells[RowIndex, 1].Text.Replace("-", ""), @"^01[0,1,6,7,8,9]\d{3,4}\d{4}$"))
                        {
                            continue;
                        }

                        if (!DataListSource.Any(c => c.MobileNo == _Sheet.Cells[RowIndex, 1].Text.Trim()))
                        {
                            ExcelList.Add((_Sheet.Cells[RowIndex, 1].Text.Trim(), _Sheet.Cells[RowIndex, 2].Text.Trim()));
                            AddedCount++;
                        }
                        TotalCount++;
                        RowIndex++;


                    }
                    Data.Connection((_Connection) =>
                    {
                        DateTime Start = DateTime.Now;
                        using (SqlTransaction _Transaction = _Connection.BeginTransaction())
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.Transaction = _Transaction;
                                _Command.CommandText
                                = "INSERT INTO AppSMS (MobileNo, Name,ClientId) VALUES (@MobileNo, @Name,@ClientId)";
                                _Command.Parameters.Add("@MobileNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@Name", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                                foreach (var (MobileNo, Name) in ExcelList)
                                {
                                    _Command.Parameters["@MobileNo"].Value = MobileNo;
                                    _Command.Parameters["@Name"].Value = Name;
                                    _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                                    _Command.ExecuteNonQuery();
                                }
                            }
                            _Transaction.Commit();
                        }
                        Debug.WriteLine((DateTime.Now - Start).TotalSeconds);
                    });
                    Invoke(new Action(() =>
                    {
                        //MessageBox.Show($"{TotalCount}건의 정보 중, 중복 정보를 제외한 {AddedCount}건의 정보를 입력하였습니다.");
                        MessageBox.Show($"입력건수 : {TotalCount} 건\r\n성공건수 : {AddedCount} 건\r\n실패건수 : {TotalCount - AddedCount}건", "등록내역", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Search();
                        this.Cursor = Cursors.Arrow;
                    }));

                });
                this.Cursor = Cursors.WaitCursor;
                SqlTask.Start();
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("선택된 핸드폰번호를 삭제 하시겠습니까?", "APP설치문자", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {


                int Count = 0;
                Data.Connection((_Connection) =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "DELETE FROM AppSMS WHERE idx = @idx";
                        _Command.Parameters.Add("@idx", SqlDbType.Int);

                        foreach (DataGridViewRow Row in mDataGridView.Rows.Cast<DataGridViewRow>().Where(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                        {

                            (int idx, String MobileNo, String Name, int ClientId) = DataListSource[Row.Index];
                            _Command.Parameters["@idx"].Value = idx;

                            if (_Command.ExecuteNonQuery() > 0)
                                Count++;
                        }


                    }
                });
                MessageBox.Show($"{Count}건의 정보가 삭제되었습니다.", "APP설치문자");
                Search();
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private string GetSelectCommand()
        {
            int sMonth = Convert.ToInt32(dtpStart.Text.Substring(5, 2));
            int lMonth = Convert.ToInt32(dtpEnd.Text.Substring(5, 2));

            int mMonth = lMonth - sMonth;

            

            String SelectCommandText =
                    @" SELECT date_client_req,recipient_num,'' AS Name, CASE WHEN mt_report_code_ib = '1000' then '성공' ELSE c.rslt_pname  end as mt_report_code_ib ,a.mt_refkey   " +
                    " FROM em_mmt_tran AS a " +
                    " join em_resultcode c on a.mt_report_code_ib = c.rslt_code  WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
            for (int i = sMonth; i <= lMonth; i++)
            {
                var Query = baseDataSet.TABLES.Where(c => c.TABLE_NAME == "em_mmt_log_2018" + String.Format("{0:00}", i)).Count();

                if (Query > 0)
                {
                    SelectCommandText += " union all" +
                    " SELECT  date_client_req,recipient_num,'' AS Name, CASE WHEN mt_report_code_ib = '1000' then '성공' ELSE c.rslt_pname  end as mt_report_code_ib ,a.mt_refkey   FROM em_mmt_log_2018" + String.Format("{0:00}", i) + " AS a " +
                    "  join em_resultcode c on a.mt_report_code_ib = c.rslt_code  WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
                }


            }
            SelectCommandText +=
                    @" UNION  all" +
                    " SELECT  date_client_req,recipient_num,'' AS Name, CASE WHEN mt_report_code_ib = '1000' then '성공' ELSE c.rslt_pname  end as mt_report_code_ib ,a.mt_refkey   " +
                    " FROM em_smt_tran AS a " +
                    " join em_resultcode c on a.mt_report_code_ib = c.rslt_code  WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
            for (int i = sMonth; i <= lMonth; i++)
            {
                var Query = baseDataSet.TABLES.Where(c => c.TABLE_NAME == "em_smt_log_2018" + String.Format("{0:00}", i)).Count();

                if (Query > 0)
                {
                    SelectCommandText += " union  all" +
                " SELECT  date_client_req,recipient_num,'' AS Name, CASE WHEN mt_report_code_ib = '1000' then '성공' ELSE c.rslt_pname  end as mt_report_code_ib ,a.mt_refkey   FROM em_smt_log_2018" + String.Format("{0:00}", i) + " AS a " +

                " join em_resultcode c on a.mt_report_code_ib = c.rslt_code  WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
                }


            }

            SelectCommandText += " UNION  all" +
                 " SELECT  A.CreateDate,a.MobileNo,a.Name,a.Remark collate Korean_Wansung_CI_AS ,a.mt_refkey  FROM (SELECT idx,CreateDate  ,MobileNo collate Korean_Wansung_CI_AS MobileNo,isnull(Name , '') Name,Remark,ClientId mt_refkey FROM AppSMS WHERE isnull(Remark,'') ! = '' ) as a  WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";

            return SelectCommandText;
        }
        private void DataLoad()
        {
            appSMSDataSet.AppSMSSearch.Clear();
            //clientDataSet.SubClients.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

                    // WhereStringList.Add("mt_refkey = @ClientId");
                    //  _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    // 2. 단어 검색
                    if (cmb_Search.SelectedIndex > 0)
                    {
                        switch (cmb_Search.Text)
                        {
                            case "성공":
                                WhereStringList.Add(" mt_report_code_ib = '성공'");
                                break;
                            case "실패":
                                WhereStringList.Add(" mt_report_code_ib != '성공'");
                                break;

                            default:
                                break;
                        }

                    }
                    SelectCommandText = "SELECT  date_client_req,recipient_num, Name ,mt_report_code_ib ,mt_refkey FROM (" + SelectCommandText + ") as C";
                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText + " order by date_client_req desc";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        appSMSDataSet.AppSMSSearch.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as AppSMSDataSet.AppSMSSearchRow;
            if (Selected == null)
                return;
            if (e.ColumnIndex == ColumnNo1.Index)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == dateclientreqDataGridViewTextBoxColumn.Index)
            {
                try
                {

                    e.Value = Selected.date_client_req.ToString("yyyy-MM-dd HH:mm");

                }
                catch (Exception)
                {
                    e.Value = "";
                }
            }
            else if (e.ColumnIndex == nameDataGridViewTextBoxColumn.Index)
            {
              
                if (Selected.mt_report_code_ib == "어플설치")
                {
                    var Query = _DriverTable.Where(c => c.MobileNo.Replace("-", "") == Selected.recipient_num).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().CarYear + "/" + Query.First().CarNo;
                    }
                }
            }

            else if (e.ColumnIndex == recipientnumDataGridViewTextBoxColumn.Index)
            {

                if (Selected.recipient_num.Length == 11)
                {
                    e.Value = Selected.recipient_num.Substring(0, 3) + "-" + Selected.recipient_num.Substring(3, 4) + "-" + Selected.recipient_num.Substring(7, 4);
                }
                else if (Selected.recipient_num.Length == 10)
                {
                    e.Value = Selected.recipient_num.Substring(0, 3) + "-" + Selected.recipient_num.Substring(3, 3) + "-" + Selected.recipient_num.Substring(6, 4);
                }
                else
                {
                    e.Value = Selected.recipient_num;
                }
            }
            else if (e.ColumnIndex == mtreportcodeibDataGridViewTextBoxColumn.Index)
            {
                if (Selected.mt_report_code_ib == "성공")
                {



                }
                else
                {
                    e.Value = "실패(" + e.Value + ")";
                    newDGV1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);
        }

        private void txtMobileNo_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txtMobileNo.Text);
            if (!ismatch)
            {
                // MessageBox.Show("숫자만 입력해 주세요.");


            }
        }

        private void txtMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void rdo_LMS_CheckedChanged(object sender, EventArgs e)
        {
            ////LMS
            //if (rdo_LMS.Checked)
            //{
            //    txtSmsDefault.ReadOnly = false;
               
            //    txtSMSText();
            //    txtSmsWriteText();
            //    btnSmsSave.Enabled = true;
            //}
            ////SMS
            //else
            //{

               
            //    txtSmsDefault.ReadOnly = true;
            //    txtSMSText();
            //    txtSmsWriteText();
            //    btnSmsSave.Enabled = false;
            //}
        }

        private void txtSmsDefault_KeyUp(object sender, KeyEventArgs e)
        {
            txtSmsDefault.MaxLength = 720;
        }

        private void txtSmsDefault_KeyDown(object sender, KeyEventArgs e)
        {
            txtSmsDefault.MaxLength = 720;
        }
        private bool _isTextChanged = false;
        private void txtSmsDefault_TextChanged(object sender, EventArgs e)
        {
            int _maxLength = 720;
            string _tempVal = txtSmsDefault.Text;
            Byte[] _byte;

            if (!_isTextChanged)
            {
                _isTextChanged = true;

                if (txtSmsDefault.Text.Length > 0)
                {
                    _byte = Encoding.GetEncoding("euc-kr").GetBytes(txtSmsDefault.Text);

                    if (_byte.Length > _maxLength)
                    {
                        MessageBox.Show(string.Format(" * 최대 {0}byte를 초과할 수 없습니다.", _maxLength));

                        while (_byte.Length > _maxLength)
                        {
                            _tempVal = _tempVal.Substring(0, _tempVal.Length - 1);
                            _byte = Encoding.GetEncoding("euc-kr").GetBytes(_tempVal);
                        }

                        txtSmsDefault.Text = _tempVal;
                        txtSmsDefault.SelectionStart = txtSmsDefault.Text.Length;
                        txtSmsDefault.Focus();
                    }

                    label6.Text = string.Format("글자수 : {0}/720", _byte.Length);
                }
                else
                    label6.Text = "글자수 : 0/720";


                _isTextChanged = false;
            }
        }

        private void btnSmsSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {

                _Connection.Open();

                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "DELETE APPSMSMessage Where ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.ExecuteNonQuery();
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "INSERT APPSMSMessage (ClientId,Message)Values(@ClientId,@Message)";
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                    _Command.Parameters.Add("@Message", SqlDbType.Text);

                    _Command.Parameters["@Message"].Value = txtSmsDefault.Text;
                    _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                    //_Command.Parameters.AddWithValue("@Message", txtSmsDefault.Text);
                    //_Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.ExecuteNonQuery();
                }
                _Connection.Close();
            }

            try
            {
                MessageBox.Show("메세지가 저장되었습니다.");

            }
            catch
            {

            }
        }
    }
}
