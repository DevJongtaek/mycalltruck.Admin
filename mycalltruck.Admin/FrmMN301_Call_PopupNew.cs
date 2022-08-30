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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN301_Call_PopupNew : Form
    {

        private int CustomerId = 0;
        private string OriginalPhoneNo = "";
        public int CallId = 0;
        string _ClientPhoneNo = "";
        String Target = "";
        String Div = "";
        String Ceo = "";
        int DriverId = 0;
        public FrmMN301_Call_PopupNew(string PhoneNo, string ClientPhoneNo, DateTime CTime,String Gubun)
        {
            OriginalPhoneNo = PhoneNo;
           
            _ClientPhoneNo = ClientPhoneNo;
            InitializeComponent();
            this.Height = 206;
            //CallTime.Text = CTime.ToString("yyyy-MM-dd HH:mm:ss");
            lblTime.Text = CTime.ToString("yyyy-MM-dd tt HH:mm");
            panel6.Visible = false;
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    //_Command.CommandText = $"SELECT CustomerId, Sangho, AddressState, AddressCity,CEO,AddressDetail FROM Customers  WHERE ClientId = {LocalUser.Instance.LogInInformation.ClientId} AND (REPLACE(PhoneNo, '-', '') = '{PhoneNo}' OR REPLACE(MobileNo, '-', '') = '{PhoneNo}')";
                    _Command.CommandText = $"SELECT customers.CustomerId, customers.Sangho, customers.AddressState, customers.AddressCity,customers.CEO,customers.AddressDetail FROM Customers LEFT JOIN CustomerAddPhone ON customers.CustomerId = CustomerAddPhone.CustomerId  WHERE customers.ClientId = {LocalUser.Instance.LogInInformation.ClientId} AND (REPLACE(customers.PhoneNo, '-', '') = '{PhoneNo}' OR REPLACE(customers.MobileNo, '-', '') = '{PhoneNo}' OR REPLACE(CustomerAddPhone.AddPhoneNo, '-', '') = '{PhoneNo}')";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            CustomerId = _Reader.GetInt32(0);
                            Target = _Reader.GetString(1);
                            Ceo = _Reader.GetString(4);
                            Div = "거래처";
                           

                            lblTarget.Text = "수신전화 / 거래처";


                            //CallTarget.Text = Div + Target;
                            CallTarget.Text = "- "+Target;

                            CallAddress.Text = "- " + String.Join(" ", _Reader.GetString(2), _Reader.GetString(3), _Reader.GetString(5));
                            CallName.Text = "- " + Ceo;
                            label1.Text = "";
                            //ShowImage.Enabled = true;
                            //InputOrder.Enabled = true;
                            return;
                        }
                    }
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Drivers.DriverId, Drivers.Name, Drivers.AddressState, Drivers.AddressCity, Drivers.CarYear,Drivers.AddressDetail,Drivers.CarNo,a.Name as CarType, b.Name +'톤' as CarSize, isnull(CarInfo,N'') FROM Drivers join StaticOptions as a on drivers.CarType = a.Value and a.Div = 'CarType' join StaticOptions as b on drivers.CarSize = b.Value and b.Div = 'CarSize' WHERE REPLACE(Drivers.PhoneNo, '-', '') = '{PhoneNo}' OR REPLACE(Drivers.MobileNo, '-', '') = '{PhoneNo}'";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            DriverId = _Reader.GetInt32(0);
                            Target = _Reader.GetString(1);
                            Ceo = _Reader.GetString(4);

                            Div = "차주";
                           // CallTarget.Text = Div + Target;
                           CallTarget.Text = "- " + Target;

                            //CallAddress.Text = "- " + String.Join(" ", _Reader.GetString(2), _Reader.GetString(3), _Reader.GetString(5));
                            CallAddress.Text = "- " + _Reader.GetString(6) + " / " + Ceo;
                            CallName.Text = "- " + _Reader.GetString(7) + " / " + _Reader.GetString(8) + " / " + _Reader.GetString(9);
                            lblTarget.Text = "수신전화 / 차주";
                            label1.Text = "";
                            return;
                        }
                    }
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Name FROM Contracts WHERE REPLACE(PhoneNo, '-', '') = '{PhoneNo}'";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            Target = _Reader.GetString(0);
                            Div = "";

                            lblTarget.Text = "수신전화 / 거래처";
                            CallTarget.Text = "- " + Target;
                            label1.Text = "";
                            //  CallTarget.Text = Div + Target;
                            return;
                        }
                    }
                }

               


            });
            if (Gubun == "Call")
            {
                Data.Connection((_Connection) =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                        "INSERT INTO Calls (OriginalPhoneNo, Target, Div, CustomerId, DriverId, CTime, ClientId, ClientPhoneNo, Memo, LoginId,Gubun) OUTPUT Inserted.CallId VALUES (@OriginalPhoneNo, @Target, @Div, @CustomerId, @DriverId, @CTime, @ClientId, @ClientPhoneNo, N'', @LoginId,'부재중')";
                        _Command.Parameters.AddWithValue("@OriginalPhoneNo", OriginalPhoneNo);
                        _Command.Parameters.AddWithValue("@Target", Target.Replace("- ",""));
                        _Command.Parameters.AddWithValue("@Div", Div);
                        _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        _Command.Parameters.AddWithValue("@CTime", CTime);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.Parameters.AddWithValue("@ClientPhoneNo", ClientPhoneNo);
                        _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                        CallId = Convert.ToInt32(_Command.ExecuteScalar());
                    }
                });
            }

            //if (Gubun == "CallReceive")
            //{
            //    Data.Connection((_Connection) =>
            //    {
            //        using (SqlCommand _Command = _Connection.CreateCommand())
            //        {
            //            _Command.CommandText =
            //            "Update Calls SET Gubun = @Gubun WHERE CallId = @CallId";
            //            _Command.Parameters.AddWithValue("@CallId", CallId);
                        
            //            _Command.Parameters.AddWithValue("@Gubun","수신");
            //            CallId = Convert.ToInt32(_Command.ExecuteScalar());
            //        }
            //    });
            //}
            string _S = PhoneNo;
            if (_S.StartsWith("1"))
            {
                if (_S.Length > 4)
                {
                    _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
                }
                else if (_S.Length > 8)
                {
                    _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                }
            }
            else if (_S.StartsWith("02"))
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
            else if (_S.StartsWith("01"))
            {
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 7)
                {
                    _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                }
                if (_S.Length > 12)
                {
                    _S = _S.Replace("-", "");
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
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
                    _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                }
                if (_S.Length > 12)
                {
                    _S = _S.Replace("-", "");
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                }
            }
            CallPhoneNo.Text = _S;
        }

        private void FrmMN301_Call_Popup_Load(object sender, EventArgs e)
        {
           
        }

        //의뢰물건
        private void ShowImage_Click(object sender, EventArgs e)
        {
            if(CustomerId > 0)
            FrmMDI.Dialog_CustomerImage_Instance.LoadCutomer(CustomerId);
        }

        //화물입력
        private void InputOrder_Click(object sender, EventArgs e)
        {
            FrmMDI.LoadCustomerFromCall(CustomerId);
        }

        //메모저장
        private void MemoUpdate_Click(object sender, EventArgs e)
        {
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "UPDATE Calls SET Memo = @Memo WHERE CallId = @CallId";
                    _Command.Parameters.AddWithValue("@Memo", Memo.Text);
                    _Command.Parameters.AddWithValue("@CallId", CallId);
                    _Command.ExecuteNonQuery();
                }
            });
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        //통화하기
        private void pictureBox12_MouseDown(object sender, MouseEventArgs e)
        {
            CallHelper.Instance.Call(OriginalPhoneNo);
        }

        public void SendSMS()
        {
            pictureBox1_MouseDown(null, null);
        }

        public void SendMemo(string _Memo = "",int  _CallId = 0)
        {
            CallId = _CallId;
            pictureBox2_MouseDown(null, null);

            if (!String.IsNullOrEmpty(_Memo))
            {
                Memo.Text = _Memo;
            }
            else
            {
                Memo.Text = "";
            }
        }

        private bool isFold = true;
        //문자보내기
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (lblTitle.Text == "SMS" || lblTitle.Text == "")
            {

                if (isFold)
                {
                    panel6.Visible = true;
                    this.Height = 350;
                    isFold = false;
                }
                else
                {
                    panel6.Visible = false;
                    this.Height = 206;
                    isFold = true;

                }
                this.label6.Text = "글자수 :0/720";
                
            }
            label2.Text = "";
            Memo.Clear();
            lblTitle.Text = "SMS";
         //   btnSave.Text = " 보내기 ";
            btnSave.Image = Properties.Resources.btn_send;

        }
        //메모
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (lblTitle.Text == "MEMO" || lblTitle.Text == "")
            {
                if (isFold)
                {

                    panel6.Visible = true;
                    this.Height = 350;
                    isFold = false;
                }
                else
                {
                    panel6.Visible = false;
                    this.Height = 206;
                    isFold = true;
                }

                this.label6.Text = "글자수 :0/720";
            }
            label2.Text = "";
            Memo.Clear();
            lblTitle.Text = "MEMO";
          //  btnSave.Text = " 저 장 ";
            btnSave.Image = Properties.Resources.btn_save;
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
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            string SVC_RT;
            string SVC_MSG;
            List<string> _SVC_RT = new List<string>();
            string SmsResult;

            if (lblTitle.Text == "SMS")
            {
                // CallHelper.Instance.SendSMS(OriginalPhoneNo, Memo.Text,CustomerId,DriverId);
                if (!LocalUser.Instance.LogInInformation.Client.HideAddTrade && !LocalUser.Instance.LogInInformation.Client.SmsYn)
                {
                    MessageBox.Show("본 서비스의 \"CRM 연동\"과 \"문자 전송\"은 \r\nLG U+ 인터넷전화기를 통해서만 가능 합니다.\r\n귀 사는\r\nLG U+ 미 사용자이므로 사용하실 수 없습니다.\r\n\r\nLG U+인터넷전화 신청\r\n문의 : ☎ 1833-2363  엑티브아이티㈜", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
              
                if (OriginalPhoneNo.Replace("-", "").Length >= 10)
                {

                    string sha512password = EncryptSHA512(CallHelper.Instance.Password);




                    string Parameter = "";
                    Parameter = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", CallHelper.Instance.Id, sha512password, OriginalPhoneNo.Replace("-", ""), Memo.Text);




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
                            //successcount++;
                        }
                        else
                        {
                            SmsResult = "실패";
                            _SVC_RT.Add(SVC_RT);
                            //failcount++;
                        }

                    }


                    Data.Connection((_Connection) =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                            "INSERT INTO CallSms (CTime, OriginalPhoneNo, SmsResult, ResultMessage, ClientId,LoginId,CustomerId,DriverId,MSG) VALUES (@CTime, @OriginalPhoneNo, @SmsResult, @ResultMessage, @ClientId,@LoginId,@CustomerId,@DriverId,@Msg)";

                            _Command.Parameters.AddWithValue("@CTime", DateTime.Now);
                            _Command.Parameters.AddWithValue("@OriginalPhoneNo", OriginalPhoneNo.Replace("-", ""));
                            _Command.Parameters.AddWithValue("@SmsResult", SmsResult);
                            _Command.Parameters.AddWithValue("@ResultMessage", SVC_MSG);
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                            _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                            _Command.Parameters.AddWithValue("@DriverId", DriverId);
                            _Command.Parameters.AddWithValue("@Msg", Memo.Text);
                            _Command.ExecuteNonQuery();
                           
                        }
                    });



                }
                if (_SVC_RT.Contains("1004"))
                {
                    MessageBox.Show("전화번호(LG U+)로 로그인에 실패 했습니다.\r\n\r\n“통화내역” 메뉴에서 “전화번호 설정”을 클릭,\r\n“비밀번호 초기화”를 한 후에\r\n다시 사용하시면 됩니다.", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                label2.Text = "문자 전송완료";


            }
            else if(lblTitle.Text == "MEMO")
            {
                Data.Connection((_Connection) =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "UPDATE Calls SET Memo = @Memo WHERE CallId = @CallId";
                        _Command.Parameters.AddWithValue("@Memo", Memo.Text);
                        _Command.Parameters.AddWithValue("@CallId", CallId);
                        _Command.ExecuteNonQuery();
                    }
                });

                label2.Text = "메모 저장완료";
            }
        }
        private bool _isTextChanged = false;
        private void Memo_TextChanged(object sender, EventArgs e)
        {
            #region 텍스트 길이 체크
            try
            {
                int _maxLength = 720;
                string _tempVal = Memo.Text;
                Byte[] _byte;

                if (!_isTextChanged)
                {
                    _isTextChanged = true;

                    if (Memo.Text.Length > 0)
                    {
                        _byte = Encoding.GetEncoding("euc-kr").GetBytes(Memo.Text);

                        if (_byte.Length > _maxLength)
                        {
                            MessageBox.Show(string.Format(" * 최대 {0}byte를 초과할 수 없습니다.", _maxLength));

                            while (_byte.Length > _maxLength)
                            {
                                _tempVal = _tempVal.Substring(0, _tempVal.Length - 1);
                                _byte = Encoding.GetEncoding("euc-kr").GetBytes(_tempVal);
                            }

                            Memo.Text = _tempVal;
                            Memo.SelectionStart = Memo.Text.Length;
                            Memo.Focus();
                        }

                        label6.Text = string.Format("글자수 : {0}/720", _byte.Length);
                    }
                    else
                        label6.Text = "글자수 : 0/720";


                    _isTextChanged = false;
                }
            }
            catch (Exception ex)
            {
                // MessageManager.ShowMessage(MessageType.Error, "오류가 발생했습니다.", ex, false);
            }
            #endregion

        }
        public void SendCall()
        {
            pictureBox12_MouseClick(null, null);
        }
        private void pictureBox12_MouseClick(object sender, MouseEventArgs e)
        {
            lblTarget.Text = "발신전화";

            CallHelper.Instance.Call(OriginalPhoneNo);


            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                    "INSERT INTO Calls (OriginalPhoneNo, Target, Div, CustomerId, DriverId, CTime, ClientId, ClientPhoneNo, Memo, LoginId,Gubun) OUTPUT Inserted.CallId VALUES (@OriginalPhoneNo, @Target, @Div, @CustomerId, @DriverId, @CTime, @ClientId, @ClientPhoneNo, N'', @LoginId,'발신')";
                    _Command.Parameters.AddWithValue("@OriginalPhoneNo", OriginalPhoneNo);
                    _Command.Parameters.AddWithValue("@Target", CallTarget.Text.Replace("- ", ""));
                    _Command.Parameters.AddWithValue("@Div",Div);
                    _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    _Command.Parameters.AddWithValue("@CTime", DateTime.Now);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@ClientPhoneNo", _ClientPhoneNo);
                    _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                    CallId = Convert.ToInt32(_Command.ExecuteScalar());
                }
            });
        }

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            FrmMN301_Call_PopupSetting f = new FrmMN301_Call_PopupSetting();

            
            f.Show();

            
        }

        bool isMove = false;
        Point fpt;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            fpt = new Point(e.X, e.Y);
        }

       

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMove &&(e.Button & MouseButtons.Left)==MouseButtons.Left)
            {
                Location = new Point(this.Left - (fpt.X - e.X),this.Top - (fpt.Y - e.Y));
            }
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if(Div =="거래처" && CustomerId != 0)
            {

                this.Close();
                FrmMDI.LoadForm("FrmMN0301","Customer", CustomerId);

            }
        }
    }
}
