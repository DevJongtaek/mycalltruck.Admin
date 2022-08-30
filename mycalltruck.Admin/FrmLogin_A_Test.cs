using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmLogin_A_Test : Form
    {
        public String AuthKey { get; set; }
        public bool Accepted { get; set; }

        public FrmLogin_A_Test()
        {
            InitializeComponent();
        }

        private void btn_Accept_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                if (txt_Password.Text == "16610102")
                {
                    SqlCommand Command = Connection.CreateCommand();

                    Connection.Open();


                    var ClientId = (int)21;
                    var UpdateCommand = Connection.CreateCommand();
                    var authKey = Guid.NewGuid().ToString();
                    UpdateCommand.CommandText = "Update Clients Set AuthKey = @AuthKey Where ClientId = @ClientId";
                    UpdateCommand.Parameters.AddWithValue("@ClientId", ClientId);
                    UpdateCommand.Parameters.AddWithValue("@AuthKey", authKey);
                    UpdateCommand.ExecuteNonQuery();
                    AuthKey = authKey;
                    Accepted = true;

                    Connection.Close();
                }
            }
            if (Accepted)
                Close();
            else
                // Info.Text = "비밀번호가 틀립니다. 올바른 비밀번호를 입력해 주십시오.";
                lblError.ForeColor = Color.Red;
                lblError.Text = "비밀번호가 틀립니다. 확인 후, 다시 입력바랍니다.";

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
