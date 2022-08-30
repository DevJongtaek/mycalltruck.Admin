using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class DialogAppSms : Form
    {
        string sMSGubun = "";
        string sMsText = "";
        string sMsText2 = "";
        int totalCount = 0;
        string _MobileNo = "";
        string _Gubun = "";
        int _DriverId = 0;
        public DialogAppSms(string SMSGubun,string SMSText, string SMSText2, int TotalCount,String MobileNo,String Gubun,int DriverId = 0)
        {
            InitializeComponent();
            sMSGubun = SMSGubun;
            sMsText = SMSText;
            sMsText2 = SMSText2;
            totalCount = TotalCount;

            _MobileNo = MobileNo;
            _Gubun = Gubun;
            _DriverId = DriverId;

          
        }
        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();
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
            public int CarSize { get; set; }
            public int CarType { get; set; }
            public string Password { get; set; }

            public string CEO { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }

            public string Upjong { get; set; }
            public string Uptae { get; set; }
            public string Email { get; set; }
            public string PhoneNo { get; set; }





        }


        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _DriverTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,ISNULL(MobileNo,N'')as MobileNo,CarNo,ISNULL(CarYear,'N')CarYear,CarSize,CarType,Password,isnull(CEO,N'') as CEO,isnull(AddressState,N'')as AddressState,ISNULL(AddressCity,N'')as AddressCity,ISNULL(AddressDetail,N'')as AddressDetail,ISNULL(Upjong,N'')as Upjong,ISNULL(Uptae,N'')as Uptae,ISNULL(Email,N'')as Email,ISNULL(PhoneNo,N'')as PhoneNo FROM Drivers ";

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
                                  CarSize = dataReader.GetInt32(10),
                                  CarType = dataReader.GetInt32(11),
                                  Password = dataReader.GetString(12),
                                  CEO = dataReader.GetString(13),

                                  AddressState = dataReader.GetString(14),
                                  AddressCity = dataReader.GetString(15),
                                  AddressDetail = dataReader.GetString(16),

                                  Upjong = dataReader.GetString(17),
                                  Uptae = dataReader.GetString(18),
                                  Email = dataReader.GetString(19),
                                  PhoneNo = dataReader.GetString(20),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private void initDriverMobileNo()
        {
            InitDriverTable();
            Dictionary<string, string> DCustomer = new Dictionary<string, string>();

            DCustomer.Clear();
            var DriverMobileList = _DriverTable.Where(c => c.DriverId == _DriverId).ToArray();

            if (DriverMobileList.Any())
            {

                DCustomer.Add(DriverMobileList.First().MobileNo, DriverMobileList.First().MobileNo);
                if (!String.IsNullOrEmpty(DriverMobileList.First().PhoneNo))
                {
                    if (!DCustomer.Keys.Contains(DriverMobileList.First().PhoneNo.ToString()))
                    {
                        DCustomer.Add(DriverMobileList.First().PhoneNo, DriverMobileList.First().PhoneNo);
                    }
                }



                cmbLMSMobileNo.DataSource = new BindingSource(DCustomer, null);
                cmbLMSMobileNo.DisplayMember = "Value";
                cmbLMSMobileNo.ValueMember = "Key";
                cmbLMSMobileNo.SelectedIndex = 0;
            }
        }

        private void DialogAppSms_Load(object sender, EventArgs e)
        {
            txtSmsDefault.Text = sMsText + sMsText2;
            lblSmsGubun.Text = sMSGubun;
            lblTotalCount.Text = totalCount + "건";

            if (_Gubun == "화주")
            {
                lblGubun.Text = "■ 화주문자 ■ ";
                LMSMobileNo.Text = _MobileNo;
                LMSMobileNo_Leave(null, null);
                lblText.Visible = true;
                LMSMobileNo.Visible = true;
                cmbLMSMobileNo.Visible = false;
            }
            else
            {
                initDriverMobileNo();


                LMSMobileNo.Visible = false;
                cmbLMSMobileNo.Visible = true;
                if (_Gubun == "차주")
                {
                    lblGubun.Text = "■ 차주문자 ■ ";
                    
                   // LMSMobileNo.Text = _MobileNo;
                   // LMSMobileNo_Leave(null, null);
                    lblText.Visible = true;
                   // LMSMobileNo.Visible = true;
                }
                else

                {
                    lblGubun.Text = "■ 문자전송 ■ ";
                    lblText.Visible = false;
                    //LMSMobileNo.Visible = false;
                }



            }
           

            

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

            if (_ClientPoint < 10)
            {
                MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                FrmMDI.LoadForm("FrmMN0204", "Point");
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                if (_Gubun == "화주")
                {
                    if (!Regex.Match(LMSMobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success && !Regex.Match(cmbLMSMobileNo.Text, @"^01[0,1,6,7,8,9]\d{3,4}\d{4}").Success)
                    {
                        MessageBox.Show("올바른 핸드폰번호를 입력 하세요.");
                        this.DialogResult = DialogResult.Cancel;

                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else if (_Gubun == "차주")
                {
                    if (!Regex.Match(cmbLMSMobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success && !Regex.Match(cmbLMSMobileNo.Text, @"^01[0,1,6,7,8,9]\d{3,4}\d{4}").Success)
                    {
                        MessageBox.Show("올바른 핸드폰번호를 입력 하세요.");
                        this.DialogResult = DialogResult.Cancel;

                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                }

            }


          

            
        }

        private void LMSMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void LMSMobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(LMSMobileNo.Text))
            {
                var _S = LMSMobileNo.Text.Replace("-", "").Replace(" ", "");
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
                LMSMobileNo.Text = _S;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (!LocalUser.Instance.LogInInformation.Client.HideAddTrade && !LocalUser.Instance.LogInInformation.Client.SmsYn)
            {
                MessageBox.Show("본 서비스의 “CRM 연동”과 “문자 전송”은 \r\nLG U+ 인터넷전화기를 통해서만 가능 합니다.\r\n귀 사는\r\nLG U+ 미 사용자이므로 사용하실 수 없습니다.\r\n\r\nLG U+인터넷전화 신청\r\n문의 : ☎ 1833-2363  엑티브아이티㈜", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            string _MobileNo = "";
            if(_Gubun =="차주" )
            {
                _MobileNo = cmbLMSMobileNo.Text;
            }
            else
            {
                _MobileNo = LMSMobileNo.Text;
            }

            string _SmsText = "";
            if(_Gubun =="화주")
            {
                _SmsText = sMsText.Replace("\r\n\r\n", "\r\n");
            }
            else
            {
                _SmsText = sMsText.Replace("버튼을 눌러","주소(URL)을 눌러").Replace("\r\n\r\n", "\r\n") + "\r\n\r\n▶계산서발행 인수증보내기◀\r\n ( 아래 URL을 눌러주세요 )\r\n\r\nhttp://m.cardpay.kr/Link?id=0";

            }

            FrmSMSNew frmSMSNew = new FrmSMSNew(_SmsText, _Gubun, _MobileNo, _DriverId);

            frmSMSNew.Show();
        }
    }
}
