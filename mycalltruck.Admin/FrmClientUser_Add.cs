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
    public partial class FrmClientUser_Add : Form
    {
        public FrmClientUser_Add()
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
                //var Query1 = "Select Count(*) From ClientUsers Where LoginId = @LoginId";
                //var Query2 =
                //    "Select Count(*) From Clients Where LoginId = @LoginId";

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
                int? _SubClientId = null;
                if (cmbSubClientId.SelectedIndex > 0)
                    _SubClientId = (int)cmbSubClientId.SelectedValue;
                clientUsersTableAdapter.Insert(txt_LoginId.Text, txt_Password.Text, LocalUser.Instance.LogInInformation.ClientId, txt_Part.Text, txt_Name.Text, txt_Rank.Text, txt_Email.Text, txt_PhoneNo.Text, txt_MobileNo.Text, chk_AllowWrite.Checked, DateTime.Now, _SubClientId, IsAgent.Checked, false,(int)cmb_Customer.SelectedValue, cmb_Customer.Text);
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
            if (cmbSubClientId.Items.Count > 0)
                cmbSubClientId.SelectedIndex = 0;
            chk_AllowWrite.Checked = true;
            IsAgent.Checked = false;
        }

        private void FrmClientUser_Add_Load(object sender, EventArgs e)
        {
            Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
            SubClientIdDictionary.Add(0, "본점");
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _AllowSubCommand = _Connection.CreateCommand())
                {
                    _AllowSubCommand.CommandText = "SELECT AllowSub FROM Clients WHERE ClientId = @ClientId";
                    _AllowSubCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var _AllowSub = (bool)_AllowSubCommand.ExecuteScalar();
                    if (_AllowSub)
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "SELECT Name, SubClientId FROM SubClients WHERE ClientId = @ClientId";
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            using (SqlDataReader _Reader = _Command.ExecuteReader())
                            {
                                while (_Reader.Read())
                                {
                                    SubClientIdDictionary.Add(_Reader.GetInt32(1), _Reader.GetString(0));
                                }
                            }
                        }
                        cmbSubClientId.DataSource = SubClientIdDictionary.ToList();
                        cmbSubClientId.ValueMember = "Key";
                        cmbSubClientId.DisplayMember = "Value";
                    }
                    else
                    {
                        lblSubClientId.Visible = false;
                        cmbSubClientId.Visible = false;
                        lbl_IsAgent.Visible = false;
                        IsAgent.Visible = false;
                    }
                }

                _Connection.Close();
            }


            Dictionary<int, string> DCustomer = new Dictionary<int, string>();

            customersNewTableAdapter.Fill(clientDataSet.CustomersNew);

            var cmbCustomerMIdDataSource = clientDataSet.CustomersNew.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.SangHo).ToArray();


            DCustomer.Add(0, "전체");


            foreach (var item in cmbCustomerMIdDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }


            cmb_Customer.DataSource = new BindingSource(DCustomer, null);
            cmb_Customer.DisplayMember = "Value";
            cmb_Customer.ValueMember = "Key";
            cmb_Customer.SelectedValue = 0;
        }

        private void cmbSubClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubClientId.SelectedIndex == 0)
            {
                IsAgent.Checked = false;
                IsAgent.Enabled = false;
            }
            else
            {
                IsAgent.Enabled = true;
            }
        }

        private void IsAgent_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAgent.Checked)
            {
                chk_AllowWrite.Checked = true;
                chk_AllowWrite.Enabled = false;
            }
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
