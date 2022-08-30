using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmSMSNew : Form
    {
        int SMSCOUNT = 0;
        string _SendGubun = "";
        string _MobileNo = "";
        int _DriverId;
        string _SmsContent = "";
        public FrmSMSNew(int SmsCnt)
        {
            InitializeComponent();

            SMSCOUNT = SmsCnt;
            label3.Text = SMSCOUNT.ToString() + " 건";
            //label4.Text = "(" + (SMSCOUNT * 20).ToString() + "원 차감 )";
        }
        public FrmSMSNew(string SmsContent,string SendGubun,string MobileNo,int DriverId)
        {
            InitializeComponent();
            _SendGubun = SendGubun;
            lblPhoneNo.Text = MobileNo;
            _DriverId = DriverId;
            label1.Text = $"선택하신 {_SendGubun}에게 문자를 전송 합니다.";
            SMSCOUNT = 1;
            label3.Text = SMSCOUNT.ToString() + " 건";
            _SmsContent = SmsContent;
           // txt_SMS.Focus();
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

        string SmsResult;
        private void btnOk_Click(object sender, EventArgs e)
        {
            int successcount = 0;
            int failcount = 0;
            string SVC_RT;
            string SVC_MSG;
            List<string> _SVC_RT = new List<string>();
            // this.DialogResult = DialogResult.OK;
            Thread t = new Thread(new ThreadStart(() =>
            {
               
                    if (lblPhoneNo.Text.Replace("-", "").Length >= 10)
                    {

                        string sha512password = EncryptSHA512(CallHelper.Instance.Password);




                        string Parameter = "";
                        Parameter = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", CallHelper.Instance.Id, sha512password, lblPhoneNo.Text.Replace("-", ""), txt_SMS.Text.Replace("&", "n"));




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


                        Data.Connection((_Connection) =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                "INSERT INTO CallSms (CTime, OriginalPhoneNo, SmsResult, ResultMessage, ClientId,LoginId,CustomerId,DriverId,MSG) VALUES (@CTime, @OriginalPhoneNo, @SmsResult, @ResultMessage, @ClientId,@LoginId,@CustomerId,@DriverId,@Msg)";

                                _Command.Parameters.AddWithValue("@CTime", DateTime.Now);
                                _Command.Parameters.AddWithValue("@OriginalPhoneNo", lblPhoneNo.Text.Replace("-", ""));
                                _Command.Parameters.AddWithValue("@SmsResult", SmsResult);
                                _Command.Parameters.AddWithValue("@ResultMessage", SVC_MSG);
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                                _Command.Parameters.AddWithValue("@CustomerId", 0);
                                _Command.Parameters.AddWithValue("@DriverId", _DriverId);
                                _Command.Parameters.AddWithValue("@Msg", txt_SMS.Text);
                                _Command.ExecuteNonQuery();
                                //CallId = Convert.ToInt32(_Command.ExecuteScalar());
                            }
                        });

                    }

                try
                {



                    if (_SVC_RT.Contains("1004"))
                    {
                        MessageBox.Show("전화번호(LG U+)로 로그인에 실패 했습니다.\r\n\r\n“통화내역” 메뉴에서 “전화번호 설정”을 클릭,\r\n“비밀번호 초기화”를 한 후에\r\n다시 사용하시면 됩니다.", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return;
                    }

                    MessageBox.Show("SMS 전송 \r\n\r\n성공 : '" + successcount + "'건\r\n실패 : '" + failcount + "'건", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);



                }
                catch { MessageBox.Show("연결된 인터넷전화(LG U+)가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);  }


                pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
               // pnProgress.Invoke(new Action(() => btn_Search_Click(null, null)));
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void txt_SMS_KeyUp(object sender, KeyEventArgs e)
        {
            //Encoding enc = Encoding.GetEncoding(0);

            //int length = enc.GetByteCount(txt_SMS.Text);

            //if (length > 80)
            //{
            //    txt_SMS.Text = txt_SMS.Text.Substring(0, txt_SMS.Text.Length - 1);
            //    e.SuppressKeyPress = true;                // 키 입력 취소
            //    txt_SMS.SelectionStart = txt_SMS.Text.Length; //맨 끝유지
            //}


        }
        private bool _isTextChanged = false;
        

        private void txt_SMS_TextChanged(object sender, EventArgs e)
        {
            int _maxLength = 710;
            string _tempVal = txt_SMS.Text;
            Byte[] _byte;

            if (!_isTextChanged)
            {
                _isTextChanged = true;

                if (txt_SMS.Text.Length > 0)
                {
                    _byte = Encoding.GetEncoding("euc-kr").GetBytes(txt_SMS.Text);

                    if (_byte.Length > _maxLength)
                    {
                        MessageBox.Show(string.Format(" * 최대 {0}byte를 초과할 수 없습니다.", _maxLength));

                        while (_byte.Length > _maxLength)
                        {
                            _tempVal = _tempVal.Substring(0, _tempVal.Length - 1);
                            _byte = Encoding.GetEncoding("euc-kr").GetBytes(_tempVal);
                        }

                        txt_SMS.Text = _tempVal;
                        txt_SMS.SelectionStart = txt_SMS.Text.Length;
                        txt_SMS.Focus();
                    }

                    label6.Text = string.Format("글자수 : {0}/710", _byte.Length);
                }
                else
                    label6.Text = "글자수 : 0/710";
              

                _isTextChanged = false;
            }


            //#region 텍스트 길이 체크
            //try
            //{
            //    char[] msg_chars = this.txt_SMS.Text.ToCharArray();
            //    int len = 0;
               
            //    len = Encoding.Default.GetBytes(txt_SMS.Text).Length;

                
            //    this.label6.Text ="글자수 : "+ len.ToString() + "/720";
            //    if (len > 720)
            //    {

            //        MessageBox.Show("720 byte를 초과하여 입력할 수 없습니다.");
            //       //this.txt_SMS.Text = this.txt_SMS.Text.Remove(txt_SMS.Text.Length - );
                   
            //    }
            //}
            //catch (Exception ex)
            //{
            //   // MessageManager.ShowMessage(MessageType.Error, "오류가 발생했습니다.", ex, false);
            //}
            //#endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txt_SMS_KeyDown(object sender, KeyEventArgs e)
        {
            txt_SMS.MaxLength = 710;   
        }

        private void FrmSMSNew_Load(object sender, EventArgs e)
        {
            txt_SMS.Text = _SmsContent;
            txt_SMS.Select(0, 0);
            
        }
    }
}
