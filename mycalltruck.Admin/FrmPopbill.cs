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
    public partial class FrmPopbill : Form
    {
        public FrmPopbill()
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
                    _Command.CommandText = "SELECT ShopID, ShopPW FROM Clients WHERE ClientId = @ClientId AND ISNULL(ShopID,N'') <> N'' AND ISNULL(ShopPW,N'') <> N''";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            var PostString = $"LoginID={_Reader.GetString(0)}&LoginPWD={_Reader.GetString(1)}";
                            var PostData = Encoding.UTF8.GetBytes(PostString);
                            Web.Navigate("https://www.popbill.com/Member/Login", null, PostData, "Content-Type: application/x-www-form-urlencoded");
                            Cursor = Cursors.Arrow;
                            return;
                        }
                    }
                }
                _Connection.Close();
            }
            Web.Navigate("https://www.popbill.com/");
            Cursor = Cursors.Arrow;
        }
    }
}
