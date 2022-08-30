using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmCustomerUser_Add : Form
    {
        public FrmCustomerUser_Add()
        {
            InitializeComponent();
            txt_CreateTime.Text = DateTime.Now.ToString("d");
        }
        public FrmCustomerUser_Add(int CustomerId)
        {
            InitializeComponent();
            txt_CreateTime.Text = DateTime.Now.ToString("d");
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "아이디 추가", 1), "아이디 추가 성공");
                Clear();
            }
            txt_Part.Focus();
        }

        private int _UpdateDB()
        {
            err.Clear();
            if (String.IsNullOrEmpty(txt_Part.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Part, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                txt_Part.Focus();
                return -1;

            }

            if (String.IsNullOrEmpty(txt_Name.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                txt_Name.Focus();
                return -1;

            }

            if (String.IsNullOrEmpty(txt_LoginId.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_LoginId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                txt_LoginId.Focus();
                return -1;
            }
            else if (LocalUser.Instance.LogInInformation.LoginId == txt_LoginId.Text)
            {
                MessageBox.Show("아이디가 중복되었습니다.!!", "아이디 입력 오류");
                err.SetError(txt_LoginId, "아이디가 중복되었습니다.!!");
                txt_LoginId.Clear();
                txt_LoginId.Focus();
                return -1;
            }
            else
            {
                //var Query1 = "Select Count(*) From CustomerAddPhone Where LoginId = @LoginId";
                //var Query2 =
                //    "Select Count(*) From Customers Where LoginId = @LoginId";
                var Query1 = "SELECT COUNT(*) FROM " +
                 "(Select LoginId From ClientUsers " +
                 " union " +
                 " Select LoginId From clients " +
                 " union " +
                 " Select LoginId From Customers " +
                 " union " +
                 " Select LoginId From CustomerAddPhone) as a" +
                 " Where LoginId = @LoginId ";


                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }
                    //if (!IsDuplicated)
                    //{
                    //    SqlCommand cmd2 = cn.CreateCommand();
                    //    cmd2.CommandText = Query2;
                    //    cmd2.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    //    if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
                    //    {
                    //        IsDuplicated = true;
                    //    }
                    //}
                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show("아이디가 중복되었습니다.!!", "아이디 입력 오류");
                    err.SetError(txt_LoginId, "아이디가 중복되었습니다.!!");
                    txt_LoginId.Clear();
                    txt_LoginId.Focus();
                    return -1;
                }
            }
            if (String.IsNullOrEmpty(txt_Password.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                txt_Password.Focus();
                return -1;
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
                if (code == "Part") iName = "부서명";
                if (code == "Name") iName = "사용자명";
                if (code == "ID") iName = "아이디";
                if (code == "Password") iName = "비밀번호";

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }
        }

        private void _AddClient()
        {
            try
            {

                customerAddPhoneTableAdapter.Insert(txt_Part.Text, txt_Name.Text, txt_PhoneNo.Text, LocalUser.Instance.LogInInformation.ClientId, LocalUser.Instance.LogInInformation.CustomerId, DateTime.Now, "", txt_LoginId.Text, txt_Password.Text, txt_Rank.Text, txt_Email.Text, txt_MobileNo.Text,0);
                    
                    //(txt_LoginId.Text, txt_Password.Text, LocalUser.Instance.LogInInformation.CustomerId, LocalUser.Instance.LogInInformation.ClientId, txt_Part.Text, txt_Name.Text, txt_Rank.Text, txt_Email.Text, txt_PhoneNo.Text, txt_MobileNo.Text, DateTime.Now);
            }
            catch
            {
                MessageBox.Show("아이디 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }
        public bool IsSuccess = false;
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "아이디 추가", 1), "아이디 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Clear()
        {
            txt_Part.Clear();
            txt_LoginId.Clear();
            txt_PhoneNo.Clear();
            txt_Name.Clear();
            txt_Password.Clear();
            txt_MobileNo.Clear();
            txt_Rank.Clear();
            txt_Email.Clear();

            txt_CreateTime.Text = DateTime.Now.ToString("d");
          
        }

        private void FrmClientUser_Add_Load(object sender, EventArgs e)
        {
          
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "").Trim();
            _Txt.SelectionStart = _Txt.Text.Length;
        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_PhoneNo.Text))
            {
                var _S = txt_PhoneNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("1"))
                {
                    if (_S.Length > 4)
                    {
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
                    }
                    if (_S.Length > 8)
                    {
                        _S = _S.Replace("-", "");
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
                txt_PhoneNo.Text = _S;
            }
        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "").Trim();
            _Txt.SelectionStart = _Txt.Text.Length;
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_MobileNo.Text))
            {
                var _S = txt_MobileNo.Text.Replace("-", "").Replace(" ", "");
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
                txt_MobileNo.Text = _S;
            }
        }
    }
}
