using mycalltruck.Admin.Class;
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
    public partial class FrmClientRegisterUser_Add : Form
    {
        public FrmClientRegisterUser_Add()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "아이디 추가", 1), "아이디 추가 성공");
                Clear();
            }
        }

        private int _UpdateDB()
        {
            err.Clear();


            if (String.IsNullOrEmpty(txt_LoginId.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_LoginId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                txt_LoginId.Focus();
                return -1;
            }
            else if (LocalUser.Instance.LogInInformation.Client.LoginId == txt_LoginId.Text )
            {
                MessageBox.Show("아이디가 중복되었습니다. 아이디가 동일한 계정이 있습니다.", "아이디 입력 오류");
                err.SetError(txt_LoginId, "아이디가 중복되었습니다.!!");
                txt_LoginId.Clear();
                txt_LoginId.Focus();
                return -1;
            }
            else
            {
                //var Query1 = "Select Count(*) From ClientUsers Where LoginId = @LoginId ";//AND Password = @Password";
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
                   // cmd1.Parameters.AddWithValue("@Password", txt_Password.Text);
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }
                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show("아이디가 중복되었습니다. 아이디/비밀번호가 동일한 계정이 있습니다.", "아이디 입력 오류");
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
                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "INSERT INTO ClientUsers (LoginId, Password, ClientId, AllowWrite, CreateTime, IsRegister) " +
                        "VALUES (@LoginId, @Password, @ClientId, 0, GETDATE(), 1) ";
                        _Command.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                        _Command.Parameters.AddWithValue("@Password", txt_Password.Text);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                });
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
            txt_LoginId.Clear();
        }

        private void FrmClientRegisterUser_Add_Load(object sender, EventArgs e)
        {
            txt_LoginId.Text = LocalUser.Instance.LogInInformation.Client.Name.Replace(" ", "");
        }
    }
}
