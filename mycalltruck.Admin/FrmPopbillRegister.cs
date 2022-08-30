using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;

namespace mycalltruck.Admin
{
    public partial class FrmPopbillRegister : Form
    {
        public FrmPopbillRegister()
        {
            InitializeComponent();
        }

        private void FrmPopbill_Load(object sender, EventArgs e)
        {
           
            Cursor = Cursors.WaitCursor;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT LOGINID, Password FROM Clients WHERE ClientId = @ClientId ";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            this.Text += $"    ■링크아이디 : EDUBILLL   ■아이디 : {_Reader.GetString(0)}   ■비밀번호 : {_Reader.GetString(1)}";
                            Web.Navigate("https://www.popbill.com/Member/Type");
                            Cursor = Cursors.Arrow;
                            return;
                        }
                    }
                }
                _Connection.Close();
            }
            Web.Navigate("https://www.popbill.com/Member/Type");
            Cursor = Cursors.Arrow;
        }
    }
}
