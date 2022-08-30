using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmBank : Form
    {
        DriverRepository mDriverRepository = new DriverRepository();
        int _DriverId = 0;
        int _TradeId = 0;
        DESCrypt m_crypt = null;
        public FrmBank(int DriverId,int TradeId)
        {
            InitializeComponent();

            m_crypt = new DESCrypt("12345678");
            _DriverId = DriverId;
            _TradeId = TradeId;



        }
        
        private void FrmBank_Load(object sender, EventArgs e)
        {
            _InitCmb();
            var _DriverSelect = mDriverRepository.NoGetDriver(_DriverId);
            //txt_CarNo.Text = 
            cmb_PayBankName.SelectedValue = _DriverSelect.PayBankCode;
            txt_CarNo.Text = _DriverSelect.CarNo;
            txt_CEO.Text = _DriverSelect.CEO;


            string tempString = string.Empty;
            tempString = _DriverSelect.PayAccountNo;
            try
            {
                txt_PayAccountNo.Text = m_crypt.Decrypt(tempString);
               
            }
            catch
            {
                txt_PayAccountNo.Text = tempString;
            }


            txt_PayAccountNo.Text = _DriverSelect.PayAccountNo;
            txt_PayInputName.Text = _DriverSelect.PayInputName;
        }

        private void _InitCmb()
        {

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
            cmb_PayBankName.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string Account_Name = txt_PayInputName.Text;
            string Mid = "edubill";
            string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_PayBankName.SelectedValue.ToString(), txt_PayAccountNo.Text, txt_PayInputName.Text, cmb_PayBankName.Text, Mid);

            //  Task.Factory.StartNew(() => {
            WebClient mWebClient = new WebClient();
            try
            {
               
                var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                // var r = mWebClient.DownloadString(new Uri("http://localhost/Pay/AccCert" + Parameter));
                if (r == null || r.ToLower() != "true")
                {
                    var _Result2 = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));
                    //var _Result2 = mWebClient.DownloadString(new Uri("http://localhost/Pay/AccCertFirst" + Parameter));
                    _Result2 = HttpUtility.UrlDecode(_Result2);
                    if (_Result2 == "오류")
                    {
                        MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        
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
                        }




                    }

                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                
                return;


            }
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                //using (SqlCommand _Command = _Connection.CreateCommand())
                //{
                //    _Command.CommandText =
                //        " UPDATE Drivers " +
                //        " SET " +
                //        "   PayBankName = @PayBankName" +
                //        " , PayBankCode = @PayBankCode" +
                //        " , PayAccountNo = @PayAccountNo" +
                //        " , PayInputName = @PayInputName" +
                     
                //        " WHERE DriverId = @DriverId ";
              
                //    _Command.Parameters.AddWithValue("@PayBankName", cmb_PayBankName.Text);
                //    _Command.Parameters.AddWithValue("@PayBankCode", cmb_PayBankName.SelectedValue);
                //    _Command.Parameters.AddWithValue("@PayAccountNo", txt_PayAccountNo.Text);
                //    _Command.Parameters.AddWithValue("@PayInputName", Account_Name);
                 
                //    _Command.Parameters.AddWithValue("@DriverId", _DriverId);
                //    _Command.ExecuteNonQuery();



                //}

                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        " UPDATE Trades " +
                        " SET " +
                        "   PayBankName = @PayBankName" +
                        " , PayBankCode = @PayBankCode" +
                        " , PayAccountNo = @PayAccountNo" +
                        " , PayInputName = @PayInputName" +

                        " WHERE TradeId = @TradeId ";

                    _Command.Parameters.AddWithValue("@PayBankName", cmb_PayBankName.Text);
                    _Command.Parameters.AddWithValue("@PayBankCode", cmb_PayBankName.SelectedValue);
                    _Command.Parameters.AddWithValue("@PayAccountNo", txt_PayAccountNo.Text);
                    _Command.Parameters.AddWithValue("@PayInputName", Account_Name);

                    _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                    _Command.ExecuteNonQuery();



                }
                _Connection.Close();
            }


            try
            {
                MessageBox.Show("수정되었습니다");
                    
                this.Close();
            }
            catch
            {

            }
        }
    }
}
