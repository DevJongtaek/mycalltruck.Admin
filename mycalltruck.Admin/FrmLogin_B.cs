using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmLogin_B : Form
    {
        public String AuthKey { get; set; }
        public bool Accepted { get; set; }
        public DateTime TimerStart { get; set; }
        private Timer mTimer = new System.Windows.Forms.Timer();

        public FrmLogin_B()
        {
            InitializeComponent();

            
        }

        private void btn_Accept_Click(object sender, EventArgs e)
        {
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT ClientId FROM Clients WHERE ClientId = @ClientId AND AuthKey = @AuthKey";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@AuthKey", Key.Text);
                    Object o = _Command.ExecuteScalar();
                    if (o != null)
                    {
                        using (SqlCommand UpdateCommand = _Connection.CreateCommand())
                        {
                            var authKey = Guid.NewGuid().ToString();
                            UpdateCommand.CommandText = "Update Clients Set AuthKey = @AuthKey Where ClientId = @ClientId";
                            UpdateCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            UpdateCommand.Parameters.AddWithValue("@AuthKey", authKey);
                            UpdateCommand.ExecuteNonQuery();
                            AuthKey = authKey;
                            Accepted = true;
                            Close();
                        }
                    }
                    else
                    {
                        label4.Visible = true;
                    }
                }
            });
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
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
        string SmsResult = "";
        private void SendSMS_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
          


            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"UPDATE Clients SET AuthKey = @AuthKey WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@AuthKey", new Random().Next(1001, 9999).ToString());
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.ExecuteNonQuery();
                }
            });
           
            try
            {


                #region
                clientsTableAdapter.Fill(clientDataSet.Clients);
                var _Clients = clientDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

                //string sha512password = EncryptSHA512("edu1234!@");




                //string Parameter2 = "";
                //Parameter2 = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", "07086806908", sha512password, MobileNo.Text.Replace("-", ""), $"[차세로]인증코드는 {_Clients.First().AuthKey}입니다.");




                //JObject response = null;

                //var uriBuilder = new UriBuilder("https://centrex.uplus.co.kr/RestApi/smssend");






                //uriBuilder.Query = Parameter2;
                //Uri finalUrl = uriBuilder.Uri;


                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);



                //request.Method = "POST";
                //request.ContentType = "text/json;";
                //request.ContentLength = 0;

                //request.Headers.Add("header-staff-api", "value");

                //var httpResponse = (HttpWebResponse)request.GetResponse();

                //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //{
                //    response = JObject.Parse(streamReader.ReadToEnd());

                //    SVC_RT = response["SVC_RT"].ToString();
                //    SVC_MSG = response["SVC_MSG"].ToString();

                //    if (SVC_RT == "0000")
                //    {
                //        SmsResult = "성공";
                //        successcount++;
                //    }
                //    else
                //    {
                //        SmsResult = "실패";
                //        failcount++;
                //        _SVC_RT.Add(SVC_RT);
                //    }

                //}

                try
                {
                    string Message = $"[배차정산관리 - 차세로]" +
                         "\r\n" +
                         $"{LocalUser.Instance.LogInInformation.ClientName} 님의 인증번호는 {_Clients.First().AuthKey} 입니다.";
                   
                    var Table = new BizMsgDataSet.BIZ_MSGDataTable();

                    var Row = Table.NewBIZ_MSGRow();


                    Row.MSG_TYPE = 6;
                    Row.CMID = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + LocalUser.Instance.LogInInformation.ClientId;
                    Row.REQUEST_TIME = DateTime.Now;
                    Row.SEND_TIME = DateTime.Now;
                    Row.DEST_PHONE = MobileNo.Text.Replace("-", "");
                    Row.SEND_PHONE = "01084301200";
                    Row.MSG_BODY = Message;
                    Row.TEMPLATE_CODE = "bizp_2019071817115516111080047";
                    Row.SENDER_KEY = "1f68131ed852323c07f08acccc94e5d88719df62";
                    Row.NATION_CODE = "82";
                    Row.STATUS = 0;
                    Row.RE_TYPE = "SMS";
                    Row.RE_BODY = "";
                      

                    Table.AddBIZ_MSGRow(Row);


                    var Adapter = new DataSets.BizMsgDataSetTableAdapters.BIZ_MSGTableAdapter();
                    Adapter.Update(Table);



                    //Data.Connection((_Connection) =>
                    //{
                    //    using (SqlCommand _Command = _Connection.CreateCommand())
                    //    {
                    //        _Command.CommandText = "Update Orders SET LMSCustomerCnt = LMSCustomerCnt+1 WHERE OrderId = @OrderId";
                    //        _Command.Parameters.AddWithValue("@OrderId", CurrentOrder.OrderId);
                    //        _Command.ExecuteNonQuery();
                    //    }
                    //    _Connection.Close();

                    //});



                    Data.Connection((_Connection) =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "INSERT ClientSmsCount (DailyCnt, ClientId, Date, CreateDate, UpdateDate,SMSGubun,sendgubun)Values(@DailyCnt,@ClientId,getdate(),getdate(),getdate(),@SMSGubun,@sendgubun)";
                            _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                            _Command.Parameters.Add("@DailyCnt", SqlDbType.Int);
                            _Command.Parameters.Add("@SMSGubun", SqlDbType.NVarChar);
                            _Command.Parameters.Add("@sendgubun", SqlDbType.NVarChar);
                            _Command.Parameters["@SMSGubun"].Value = "LMS";

                            _Command.Parameters["@DailyCnt"].Value = 1;
                            _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                            _Command.Parameters["@sendgubun"].Value = "배차";
                            _Command.ExecuteNonQuery();
                        }
                        _Connection.Close();

                    });


                    LocalUser.Instance.LogInInformation.LoadClient();
                    ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                    {
                        ClientId = 21,
                        CDate = DateTime.Now,
                        Amount = -10,
                        MethodType = "알림톡",
                        Remark = $"{LocalUser.Instance.LogInInformation.Client.Name} ({LocalUser.Instance.LogInInformation.Client.BizNo})",
                    });
                    ShareOrderDataSet.Instance.SaveChanges();

                    //MessageBox.Show($"알림톡 전송이 완료 되었습니다.", "알림톡전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("인증번호 요청이 발송 되었습니다. 시간 안에 인증번호를 확인하시기 바랍니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.None);
                    TimerStart = DateTime.Now;
                    Key.Enabled = true;
                    mTimer.Interval = 1000;
                    mTimer.Tick += MTimer_Tick;
                    Timer.Text = "[3:00]";

                    label3.Text = "[3:00]";
                    mTimer.Start();

                }
                catch (Exception)
                {

                }


                #endregion
                //if (SmsResult == "성공")
                //{

             
                //}
                //else
                //{
                //    MessageBox.Show("인증번호 요청이 실패하였습니다.\r\n아래 전화번호로 문의 바랍니다.\r\n문의: 1661-6090", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //}

            }
            catch (Exception)
            {
                MessageBox.Show("인증번호 요청이 실패하였습니다.\r\n아래 전화번호로 문의 바랍니다.\r\n문의: 1661-6090", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void MTimer_Tick(object sender, EventArgs e)
        {
            var Time = (TimerStart.AddMinutes(3) - DateTime.Now);
            if(Time.TotalSeconds > 0)
            {
                Timer.Text = $"[{Time.TotalMinutes.ToString("N0")}:{Time.Seconds.ToString("00")}]";
                label3.Text = $"[{Time.TotalMinutes.ToString("N0")}:{Time.Seconds.ToString("00")}]";
            }
            else
            {
                Key.Enabled = false;
                mTimer.Stop();
            }
        }

        private void FrmLogin_B_Load(object sender, EventArgs e)
        {
            LocalUser.Instance.LogInInformation.LoadClient();
            string t = LocalUser.Instance.LogInInformation.Client.MobileNo.Replace("-", "");
            if(t.Length == 12)
            {
                t = t.Substring(0, 4) + "-" + t.Substring(4, 4) + "-" + t.Substring(8);
            }
            else if(t.Length == 11)
            {
                t = t.Substring(0, 3) + "-" + t.Substring(3, 4) + "-" + t.Substring(7);
            }
            else if (t.Length == 10)
            {
                t = t.Substring(0, 3) + "-" + t.Substring(3, 3) + "-" + t.Substring(6);
            }
            MobileNo.Text = t;
        }

        private void Key_TextChanged(object sender, EventArgs e)
        {
            if (Key.Text.Length == 4)
                btn_Accept.Enabled = true;
            else
                btn_Accept.Enabled = false;
        }

        private void Key_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
    }
}
