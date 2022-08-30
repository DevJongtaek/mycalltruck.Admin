using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmSMS : Form
    {
        int SMSCOUNT = 0;
        string _SendGubun = "";
        public FrmSMS(int SmsCnt)
        {
            InitializeComponent();

            SMSCOUNT = SmsCnt;
            label3.Text = SMSCOUNT.ToString() + " 건";
            //label4.Text = "(" + (SMSCOUNT * 20).ToString() + "원 차감 )";
        }
        public FrmSMS(string SmsContent,string SendGubun)
        {
            InitializeComponent();
            _SendGubun = SendGubun;
            label1.Text = $"선택하신 {_SendGubun}에게 문자를 전송 합니다.";
            SMSCOUNT = 1;
            label3.Text = SMSCOUNT.ToString() + " 건";
            txt_SMS.Text = SmsContent+"\r\n"+ "※.세금계산서(인수증) 발행은 http://m.cardpay.kr/Link?Id=0 눌러주세요.";
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
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
            int _maxLength = 720;
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

                    label6.Text = string.Format("글자수 : {0}/720", _byte.Length);
                }
                else
                    label6.Text = "글자수 : 0/720";
              

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
            txt_SMS.MaxLength = 720;   
        }
    }
}
