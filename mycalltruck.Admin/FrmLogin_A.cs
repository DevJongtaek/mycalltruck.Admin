using mycalltruck.Admin.Class.Common;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmLogin_A : Form
    {
        public String AuthKey { get; set; }
        public bool Accepted { get; set; }

        public FrmLogin_A()
        {
            InitializeComponent();
        }

        private void btn_Accept_Click(object sender, EventArgs e)
        {
            if(LocalUser.Instance.LogInInformation.ClientUserId == 0)
            {
                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT ClientId From Clients Where LoginId = @LoginId AND Password = @Password";
                    Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                    Command.Parameters.AddWithValue("@Password", txt_Password.Text);
                    Connection.Open();
                    var o = Command.ExecuteScalar();
                    if (o != null && o is Int32)
                    {
                        var ClientId = (int)o;
                        var UpdateCommand = Connection.CreateCommand();
                        var authKey = Guid.NewGuid().ToString();
                        UpdateCommand.CommandText = "Update Clients Set AuthKey = @AuthKey Where ClientId = @ClientId";
                        UpdateCommand.Parameters.AddWithValue("@ClientId", ClientId);
                        UpdateCommand.Parameters.AddWithValue("@AuthKey", authKey);
                        UpdateCommand.ExecuteNonQuery();
                        AuthKey = authKey;
                        Accepted = true;
                    }
                    Connection.Close();
                }
            }
            else
            {
                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT ClientId From ClientUsers Where LoginId = @LoginId AND Password = @Password";
                    Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                    Command.Parameters.AddWithValue("@Password", txt_Password.Text);
                    Connection.Open();
                    var o = Command.ExecuteScalar();
                    if (o != null && o is Int32)
                    {
                        var ClientId = (int)o;
                        var UpdateCommand = Connection.CreateCommand();
                        var authKey = Guid.NewGuid().ToString();
                        UpdateCommand.CommandText = "Update Clients Set AuthKey = @AuthKey Where ClientId = @ClientId";
                        UpdateCommand.Parameters.AddWithValue("@ClientId", ClientId);
                        UpdateCommand.Parameters.AddWithValue("@AuthKey", authKey);
                        UpdateCommand.ExecuteNonQuery();
                        AuthKey = authKey;
                        Accepted = true;
                    }
                    Connection.Close();
                }
            }
            if (Accepted)
                Close();
            else
            {
                lblError.ForeColor = Color.Red;
                lblError.Text = "비밀번호가 틀립니다. 확인 후, 다시 입력바랍니다.";
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txt_Password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                btn_Accept_Click(null, null);
        }
    }
}
