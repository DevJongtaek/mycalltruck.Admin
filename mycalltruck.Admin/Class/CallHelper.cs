using AxLGUBASEOPENAPILib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;
using mycalltruck.Admin.Class.Common;
using System.Security.Cryptography;

namespace mycalltruck.Admin.Class
{
    class CallHelper : IDisposable
    {
        private FrmMDI Form = null;
        private bool _IsLogined = false;
        public bool IsLogined { get { return _IsLogined; } set {
                if(_IsLogined != value)
                {
                    _IsLogined = value;
                    if(lblRecord != null)
                    {
                        var _Text = _IsLogined ? "자동녹음 사용 중입니다." : "자동녹음 기능이 꺼졌습니다.";
                        if (lblRecord.InvokeRequired)
                            lblRecord.Invoke(new Action(() => lblRecord.Text = _Text));
                        else
                            lblRecord.Text = _Text;
                    }
                }
            } }
        public bool IsDisposed { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public static CallHelper Instance = null;
        public Action OnLogined = null;
        public Action OnLogouted = null;
        private AxLGUBaseOpenApi _Instance = null;
        private Label lblRecord = null;
        int _CustomerIdS = 0;
        int _DriverIdS = 0;
        string _Msg = "";
        public CallHelper(FrmMDI _Form)
        {
            Form = _Form;
        }
        public void Start()
        {
            var ConfigFile = _GetConfigFile();
            if (File.Exists(ConfigFile))
            {
                try
                {
                    var Json = File.ReadAllText(ConfigFile, Encoding.UTF8);
                    var jObject = (JObject)JsonConvert.DeserializeObject(Json);
                    if (jObject.GetValue("Id") != null && jObject.GetValue("Password") != null)
                    {
                        Id = jObject.GetValue("Id").ToString();
                        Password = jObject.GetValue("Password").ToString();
                        _Instance.LoginServer(Id, Password, "");
                    }
                }
                catch (Exception)
                {
                }
            }
            GetRecord();
        }
        public  void Initialize(AxLGUBaseOpenApi instance, Label ilblRecord)
        {
            _Instance = instance;
            _Instance.SendChannelListEvent += _Instance_SendChannelListEvent;
            _Instance.SendCommandResultEvent += _Instance_SendCommandResultEvent;
            _Instance.SendLoginResultEvent += _Instance_SendLoginResultEvent;
            _Instance.SendRingEvent += _Instance_SendRingEvent;
            lblRecord = ilblRecord;
        }

        public void _Instance_SendLoginResultEvent(object sender, _DLGUBaseOpenApiEvents_SendLoginResultEventEvent e)
        {
          LoginResultEventArgs E = new LoginResultEventArgs(e.bstrLoginResult);
            IsLogined = E.STATUS == LoginResultStatus.Normal;
            if (IsLogined)
            {
                var Json = JsonConvert.SerializeObject(new
                {
                    Id = Id,
                    Password = Password
                });
                File.WriteAllText(_GetConfigFile(), Json, Encoding.UTF8);

              //  MessageBox.Show("로그인");
                OnLogined?.Invoke();

            }
            else
            {
                //Id = "";
                //Password = "";
                //var Json = JsonConvert.SerializeObject(new
                //{
                //    Id = Id,
                //    Password = Password
                //});
                //File.WriteAllText(_GetConfigFile(), Json, Encoding.UTF8);
                OnLogouted?.Invoke();

                //MessageBox.Show("로그아웃");
            }
        }

        private void _Instance_SendCommandResultEvent(object sender, _DLGUBaseOpenApiEvents_SendCommandResultEventEvent e)
        {

            Debug.WriteLine(e.bstrCommandResult);

            var _Result = e.bstrCommandResult.Split('|');
            string OriginalPhoneNo2 = "";
            string SmsResult = "";

            if (_Result.Length > 0)
            {


                if (_Result[1].Contains("SENDSMS"))
                {
                    OriginalPhoneNo2 = _Result[2].Replace(":", "");
                    if (_Result.Length > 3)
                    {
                        if (_Result[3].Contains("SUCCESS"))
                        {
                            SmsResult = "성공";
                        }
                        else
                        {
                            SmsResult = "실패";
                        }
                    }
                    else
                    {
                        _Result[3] = "Fail";
                        SmsResult = "실패";

                    }




                    Data.Connection((_Connection) =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                            "INSERT INTO CallSms (CTime, OriginalPhoneNo, SmsResult, ResultMessage, ClientId,LoginId,CustomerId,DriverId,MSG) VALUES (@CTime, @OriginalPhoneNo, @SmsResult, @ResultMessage, @ClientId,@LoginId,@CustomerId,@DriverId,@Msg)";

                            _Command.Parameters.AddWithValue("@CTime", DateTime.Now);
                            _Command.Parameters.AddWithValue("@OriginalPhoneNo", OriginalPhoneNo2);
                            _Command.Parameters.AddWithValue("@SmsResult", SmsResult);
                            _Command.Parameters.AddWithValue("@ResultMessage", _Result[3]);
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                            _Command.Parameters.AddWithValue("@CustomerId", _CustomerIdS);
                            _Command.Parameters.AddWithValue("@DriverId", _DriverIdS);
                            _Command.Parameters.AddWithValue("@Msg", _Msg);
                            _Command.ExecuteNonQuery();
                            //CallId = Convert.ToInt32(_Command.ExecuteScalar());
                        }
                    });


                }
            }

        }
        private void _Instance_SendRingEvent(object sender, _DLGUBaseOpenApiEvents_SendRingEventEvent e)
        {
            //이벤트명
            string EventName = string.Empty;
            //수,발신구분 0:수신,1:발신
            var ISDIAL = string.Empty;
            //수신채널
            var CHANNEL = string.Empty;
            //발신채널
            var RECHANNEL = string.Empty;
            //인입번호
            var INEXTEN = string.Empty;
            //나의 내선번호
            var AGENT = string.Empty;
            //
            var ATXFER = string.Empty;
            //상대방발신번호
            var CALLERID = string.Empty;
            //시스템 UNIQUEID
            var UNIQUEID = string.Empty;
            var bstrRingEvent = e.bstrRingEvent;

            var s = bstrRingEvent.Split('|');
            foreach (var KeyAndValue in s)
            {
                if (KeyAndValue.Contains(':'))
                {
                    var Key = KeyAndValue.Split(':')[0];
                    var Value = KeyAndValue.Split(':')[1];
                    if (Key == "ISDIAL")
                    {
                        ISDIAL = Value;
                    }
                    else if (Key == "CHANNEL")
                    {
                        CHANNEL = Value;
                    }
                    else if (Key == "RECHANNEL")
                    {
                        RECHANNEL = Value.Replace("-", ""); ;
                    }
                    else if (Key == "INEXTEN")
                    {
                        INEXTEN = Value.Replace("-", ""); ;
                    }
                    else if (Key == "AGENT")
                    {
                        AGENT = Value.Replace("-", ""); ;
                    }
                    else if (Key == "ATXFER")
                    {
                        ATXFER = Value;
                    }
                    else if (Key == "CALLERID")
                    {
                        CALLERID = Value;
                    }
                    else if (Key == "UNIQUEID")
                    {
                        UNIQUEID = Value;
                    }
                }
            }

            if(ISDIAL == "0")
            {
                if (LocalUser.Instance.LogInInformation.Client.AllowOrder)
                {
                    Form.Invoke(new Action(() =>
                    {
                       
                        FrmMN301_Call_PopupNew f = new FrmMN301_Call_PopupNew(CALLERID, INEXTEN, DateTime.Now, "Call");
                        f.Show();
                        if(f.CallId != 0)
                        {
                            _CallId = f.CallId;
                        }
                       
                    }));
                    return;
                }
            }
        }
        private int _CallId = 0;
        private void _Instance_SendChannelListEvent(object sender, _DLGUBaseOpenApiEvents_SendChannelListEventEvent e)
        {
            ChannelListEventArgs E = new ChannelListEventArgs(e.bstrChannelList);
            if (E.CALLER1ID.Length < 8 || (long.TryParse(E.CALLER1ID, out long v1) && long.TryParse(Id, out long v2) && v1 == v2))
                E.ISDIAL = true;
            if (!LocalUser.Instance.LogInInformation.IsAdmin && !E.ISDIAL)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (LocalUser.Instance.LogInInformation.Client.AllowOrder)
                        {
                            Form.Invoke(new Action(() =>
                            {
                                //FrmMN301_Call_Popup f = new FrmMN301_Call_Popup(E.CALLER1ID, E.CALLER2ID, E.CTime);
                                // FrmMN301_Call_PopupNew f = new FrmMN301_Call_PopupNew(E.CALLER1ID, E.CALLER2ID, E.CTime, "CallReceive");
                                Data.Connection((_Connection) =>
                                {
                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText =
                                        "Update Calls SET Gubun = @Gubun WHERE CallId = @CallId";
                                        _Command.Parameters.AddWithValue("@CallId", _CallId);

                                        _Command.Parameters.AddWithValue("@Gubun", "수신");
                                        _CallId = Convert.ToInt32(_Command.ExecuteScalar());
                                    }
                                });

                            }));
                            return;
                        }


                        //using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        //{
                        //    _Connection.Open();
                        //    SqlCommand _Command = _Connection.CreateCommand();
                        //    _Command.CommandText = String.Format("SELECT CustomerId FROM Customers WHERE (REPLACE(PhoneNo, '-', '') LIKE '%{0}%' OR REPLACE(MobileNo, '-', '') LIKE '%{0}%' OR REPLACE(PhoneNo, '-', '') LIKE '%{1}%' OR REPLACE(MobileNo, '-', '') LIKE '%{1}%') AND ClientId = {2}", E.CALLER1ID, E.CALLER2ID, LocalUser.Instance.LogInInformation.ClientId);
                        //    var o = _Command.ExecuteScalar();
                        //    if (o != null)
                        //    {
                        //        var _CustomerId = Convert.ToInt32(o);
                        //        Form.Invoke(new Action(() =>
                        //        {
                        //            FrmMN0301_CARGOACCEPT_Add f = new FrmMN0301_CARGOACCEPT_Add();
                        //            f.ShowFromCustomerId(_CustomerId);
                        //        }));
                        //    }
                        //}
                    }
                    catch (Exception)
                    {
                    }
                });
            }
            StartRecrod();
        }
        public void Login(String _Id, String _Password)
        {
            try
            {
                _Instance.DisconnectServer();
                //var Json = JsonConvert.SerializeObject(new
                //{
                //    Id = _Id,
                //    Password = _Password
                //});
                //File.WriteAllText(_GetConfigFile(), Json, Encoding.UTF8);

                //Start();
                IsLogined = false;
                Id = _Id;
                Password = _Password;
                _Instance.LoginServer(Id, Password, "");
            }
            catch(Exception ex)
            {

            }

           
        }

     

        public void SendSMS(String OriginalPhoneNo, String Msg,int CustomerId, int DriverId)
        {
            _CustomerIdS = CustomerId;
            _DriverIdS = DriverId;
            _Msg = Msg;
            var type = "0";//예약:1
            var typeinfo = "0";//예약시 예약시간 년월일 시:분:00
            var peers = OriginalPhoneNo;
            var msg = Msg.Replace("\r\n"," ");//\n\r없이


            _Instance.SendSMS(type, typeinfo, peers, msg);
           

        }
        public void StartRecrod()
        {
            if (IsLogined)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    if (_Instance.InvokeRequired)
                    {
                        _Instance.Invoke(new Action(() => _Instance.StartRecord()));
                    }
                    else
                    {
                        _Instance.StartRecord();
                    }
                });
            }
        }
        private Task GetRecord()
        {
            DateTime Last = DateTime.Now;
            return Task.Factory.StartNew(() => {
                while (!IsDisposed)
                {
                    try
                    {
                        Thread.Sleep(100);
                        if (IsLogined && (DateTime.Now - Last).TotalSeconds > 60)
                        {
                            WebClient mWebClient = new WebClient();
                            var LoginPage = mWebClient.DownloadString("http://centrex.uplus.co.kr/premium/");
                            var Cookie = mWebClient.ResponseHeaders["Set-Cookie"].Split(';')[0];
                            var token = Regex.Match(LoginPage, "name=\"token\"([^>]*)>").Groups[1].Value.Replace("value=", "").Replace("\"", "").Replace(" ", "");
                            mWebClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                            mWebClient.Headers.Add("Referer: http://centrex.uplus.co.kr/premium/");
                            mWebClient.Headers.Add("Accept-Language: ko-KR");
                            mWebClient.Headers.Add("Content-Type: application/x-www-form-urlencoded;charset=UTF-8");
                            mWebClient.Headers.Add("Host: centrex.uplus.co.kr");
                            mWebClient.Headers.Add(String.Format("Cookie: {0}", Cookie));
                            var Response = mWebClient.UploadString("http://centrex.uplus.co.kr/premium/PHP/web_login.php", String.Format("token={0}&id={1}&pass={2}", token, Id, Password));
                            if (Response.Contains("http://centrex.uplus.co.kr/premium/backoffice/"))
                                IsLogined = true;
                            else
                                IsLogined = false;
                            if (IsLogined)
                            {
                                Thread.Sleep(1 * 1 * 1000);
                                var ListUrl = "http://centrex.uplus.co.kr/premium/backoffice/record_list.html";
                                var r = mWebClient.DownloadData(ListUrl);
                                string ListResponse = Encoding.Default.GetString(r);
                                if (ListResponse.Contains("사용 권한이 없습니다."))
                                {
                                    IsLogined = false;
                                    continue;
                                }
                                int startIndex = 0;
                                while (true)
                                {
                                    startIndex = ListResponse.IndexOf("'../PHP/recorddownload.html?filename=", startIndex + 1);
                                    if (startIndex == -1)
                                        break;
                                    int endIndex = ListResponse.IndexOf("'", startIndex + 1);
                                    if (endIndex == -1)
                                        break;
                                    string iUrl = ListResponse.Substring(startIndex, endIndex - startIndex).Replace("'","");
                                    var splited = iUrl.Split(new char[] { '?', '&' });
                                    string prefix="", proxyip = "", naesun = "", dFileName = "";
                                    foreach (var item in splited)
                                    {
                                        if (item.Contains('='))
                                        {
                                            var splited2 = item.Split('=');
                                            if (splited2.Length == 2)
                                            {
                                                if (splited2[0] == "prefix")
                                                {
                                                    prefix = splited2[1];
                                                }
                                                if (splited2[0] == "proxyip")
                                                {
                                                    proxyip = splited2[1];
                                                }
                                                if (splited2[0] == "naesun")
                                                {
                                                    naesun = splited2[1];
                                                }
                                                if (splited2[0] == "filename")
                                                {
                                                    dFileName = splited2[1];
                                                }
                                            }
                                        }
                                    }

                                        string fileName = GetFileName(iUrl.Replace("../PHP/recorddownload.html?filename=", ""));
                                    iUrl = "http://centrex.uplus.co.kr/premium" + iUrl.Substring(2);
                                    var _Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CardPay");
                                    if (Directory.Exists(_Directory) == false)
                                        Directory.CreateDirectory(_Directory);
                                    var DateString = fileName.Split('_')[1];
                                    var _Date = DateTime.ParseExact(DateString, "yyyyMMddHHmmss", null);
                                    _Directory = Path.Combine(_Directory, _Date.ToString("yyyy-MM-dd"));
                                    if (Directory.Exists(_Directory) == false)
                                        Directory.CreateDirectory(_Directory);
                                    var file = Path.Combine(_Directory, String.Format("{0}", fileName));
                                    mWebClient.DownloadFile(iUrl, file);
                                    //Delete
                                    var sendData = string.Format("prefix={0}&proxyip={1}&chk%5B%5D={2}%7C{3}", prefix, proxyip, naesun, dFileName);
                                    mWebClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                                    var r111 =mWebClient.UploadString("http://centrex.uplus.co.kr/premium/PHP/deleteRecordFile.php", sendData);
                                    Thread.Sleep(1 * 1 * 1000);
                                }
                            }
                            Last = DateTime.Now;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }
        private string GetFileName(string url)
        {
            var endIndex = url.IndexOf("&");
            if (endIndex < 0)
                throw new UriFormatException();
            return url.Substring(0, endIndex);
        }
        public string _GetConfigFile()
        {
            var ConfigFile = "";
            ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cardpay");
            if (!Directory.Exists(ConfigFile))
                Directory.CreateDirectory(ConfigFile);
            ConfigFile = Path.Combine(ConfigFile, "Call.Config");
            return ConfigFile;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public void Call(string OriginalPhoneNo)
        {
            
            _Instance.Click2Call(OriginalPhoneNo, "", "");
        }

        class ChannelListEventArgs
        {
            public ChannelListEventArgs(String bstrChannelList)
            {
                var s = bstrChannelList.Split('|');
                foreach (var KeyAndValue in s)
                {
                    if (KeyAndValue.Contains(':'))
                    {
                        var Key = KeyAndValue.Split(':')[0];
                        var Value = KeyAndValue.Split(':')[1];
                        if (Key == "CHANNEL1")
                        {
                            CHANNEL1 = Value;
                        }
                        else if (Key == "CHANNEL2")
                        {
                            CHANNEL2 = Value;
                        }
                        else if (Key == "CALLER1ID")
                        {
                            CALLER1ID = Value.Replace("-", ""); ;
                        }
                        else if (Key == "CALLER2ID")
                        {
                            CALLER2ID = Value.Replace("-", ""); ;
                        }
                        else if (Key == "INEXTEN")
                        {
                            INEXTEN = Value.Replace("-", ""); ;
                        }
                        else if (Key == "UNIQUEID1")
                        {
                            UNIQUEID1 = Value;
                        }
                        else if (Key == "UNIQUEID2")
                        {
                            UNIQUEID2 = Value;
                        }
                    }
                }
                CTime = DateTime.Now;
                ISDIAL = false;
            }

            /// <summary>
            /// 발신채널
            /// </summary>
            public String CHANNEL1 { get; private set; }
            /// <summary>
            /// 수신채널
            /// </summary>
            public String CHANNEL2 { get; private set; }
            /// <summary>
            /// 발신채널 번호
            /// </summary>
            public String CALLER1ID { get; private set; }
            /// <summary>
            /// 수신채널 번호
            /// </summary>
            public String CALLER2ID { get; private set; }
            /// <summary>
            /// 돌려준 번호
            /// </summary>
            public String INEXTEN { get; private set; }
            /// <summary>
            /// 발신채널의 UNIQUEID
            /// </summary>
            public String UNIQUEID1 { get; private set; }
            /// <summary>
            /// 수신채널의 UNIQUEID
            /// </summary>
            public String UNIQUEID2 { get; private set; }
            /// <summary>
            /// 발생시각
            /// </summary>
            public DateTime CTime { get; private set; }
            /// <summary>
            /// 발신여부
            /// </summary>
            public bool ISDIAL { get; set; }
        }
        class LoginResultEventArgs 
        {
            public LoginResultEventArgs(LoginResultStatus iSTATUS, string iEXTEN, string iCALLERID, string iMSG)
            {
                STATUS = iSTATUS;
                EXTEN = iEXTEN;
                CALLERID = iCALLERID;
                MSG = iMSG;
                CTime = DateTime.Now;
            }

            public LoginResultEventArgs(String bstrLoginResult)
            {
                var s = bstrLoginResult.Split('|');
                foreach (var KeyAndValue in s)
                {
                    if (KeyAndValue.Contains(':'))
                    {
                        var Key = KeyAndValue.Split(':')[0];
                        var Value = KeyAndValue.Split(':')[1];
                        if (Key == "STATUS")
                        {
                            STATUS = (LoginResultStatus)int.Parse(Value);
                        }
                        else if (Key == "EXTEN")
                        {
                            EXTEN = Value.Replace("-", ""); ;
                        }
                        else if (Key == "CALLERID")
                        {
                            CALLERID = Value.Replace("-", ""); ;
                        }
                        else if (Key == "MSG")
                        {
                            MSG = Value;
                        }
                    }
                }
                CTime = DateTime.Now;
            }
            //public void smssend(string apiUrl, string jsonParameter)
            //{

            //    string uri = "https://centrex.uplus.co.kr/RestApi/smssend";
            //    string requestJson = "someJsonRequestString";
            //    WebClient webClient = new WebClient();
            //    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            //    webClient.Encoding = UTF8Encoding.UTF8;
            //    string responseJSON = webClient.UploadString(uri, requestJson);


            //}
           
            /// <summary>
            /// 로그인결과값
            /// </summary>
            public LoginResultStatus STATUS { get; private set; }
            /// <summary>
            /// 내선번호
            /// </summary>
            public string EXTEN { get; private set; }
            /// <summary>
            /// 발신자번호
            /// </summary>
            public string CALLERID { get; private set; }
            /// <summary>
            /// 오류메시지 혹은 내선,발신번호
            /// </summary>
            public string MSG { get; private set; }
            /// <summary>
            /// 발생시각
            /// </summary>
            public DateTime CTime { get; private set; }
        }
        enum LoginResultStatus
        {
            Normal = 1,
            NotFound = -1,
            PasswdErr = -2,
            TimeOut = -3,
        }

    }
}
