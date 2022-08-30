using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmLogin_C : Form
    {
        public String AuthKey { get; set; }
        public bool Accepted { get; set; }
        public DateTime TimerStart { get; set; }
        private Timer mTimer = new System.Windows.Forms.Timer();

        string rClientCode = "";
        string rMobileNo = "";
        string rName = "";
        int rIdx;
        public FrmLogin_C(string _ClientCode,string _MobileNo,string _Name)
        {
            InitializeComponent();
            rClientCode = _ClientCode;
            rMobileNo = _MobileNo;
            rName = _Name;
        }

        private void btn_Accept_Click(object sender, EventArgs e)
        {
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT RegisterAuthId FROM RegisterAuth WHERE ClientCode = @ClientCode AND AuthKey = @AuthKey";
                    _Command.Parameters.AddWithValue("@ClientCode", rClientCode);
                    _Command.Parameters.AddWithValue("@AuthKey", Key.Text);
                    Object o = _Command.ExecuteScalar();
                    if (o != null)
                    {
                        using (SqlCommand UpdateCommand = _Connection.CreateCommand())
                        {
                            var authKey = Guid.NewGuid().ToString();
                            UpdateCommand.CommandText = "Delete RegisterAuth  Where RegisterAuthId = @idx";
                            UpdateCommand.Parameters.AddWithValue("@idx", o.ToString());
                            //UpdateCommand.Parameters.AddWithValue("@AuthKey", authKey);
                            UpdateCommand.ExecuteNonQuery();
                            //AuthKey = authKey;
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

        private void SendSMS_Click(object sender, EventArgs e)
        {
            var AuthKey = new Random().Next(1001, 9999).ToString();
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"INSERT INTO  registerauth ( AuthKey, ClientCode, MobileNo) VALUES( @AuthKey, @ClientCode, @MobileNo) SELECT @@identity";
                    _Command.Parameters.AddWithValue("@AuthKey", AuthKey);
                    _Command.Parameters.AddWithValue("@ClientCode", rClientCode);
                    _Command.Parameters.AddWithValue("@MobileNo", rMobileNo);
                    //_Command.ExecuteNonQuery();

                    Object O = _Command.ExecuteScalar();
                    if (O != null)
                        rIdx = Convert.ToInt32(O);
                }
            });

           
            try
            {


                #region
                
                try
                {
                    string Message = $"[배차정산관리 - 차세로]" +
                         "\r\n" +
                         $"{rName} 님의 인증번호는 {AuthKey} 입니다.";

                    var Table = new BizMsgDataSet.BIZ_MSGDataTable();

                    var Row = Table.NewBIZ_MSGRow();


                    Row.MSG_TYPE = 6;
                    Row.CMID = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + rClientCode;
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




                    ShareOrderDataSet.Instance.ClientPoints.Add(new ClientPoint
                    {
                        ClientId = 21,
                        CDate = DateTime.Now,
                        Amount = -10,
                        MethodType = "알림톡",
                        Remark = $"에듀빌 (1131183754)",
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
            string t = rMobileNo.Replace("-", "");
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
