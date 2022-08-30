using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
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
    public partial class FrmMN301_Call_Popup : Form
    {

        private int CustomerId = 0;
        private string OriginalPhoneNo = "";
        private int CallId = 0;

        public FrmMN301_Call_Popup(string PhoneNo, string ClientPhoneNo, DateTime CTime)
        {
            OriginalPhoneNo = PhoneNo;
            String Target = "";
            String Div = "";
            int DriverId = 0;
            InitializeComponent();
            CallTime.Text = CTime.ToString("yyyy-MM-dd HH:mm:ss");
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT CustomerId, Sangho, AddressState, AddressCity FROM Customers WHERE ClientId = {LocalUser.Instance.LogInInformation.ClientId} AND (REPLACE(PhoneNo, '-', '') = '{PhoneNo}' OR REPLACE(MobileNo, '-', '') = '{PhoneNo}')";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            CustomerId = _Reader.GetInt32(0);
                            Target = _Reader.GetString(1);
                            Div = "[화주]";
                            CallTarget.Text = Div + Target;
                            CallAddress.Text = String.Join(" ", _Reader.GetString(2), _Reader.GetString(3));
                            ShowImage.Enabled = true;
                            InputOrder.Enabled = true;
                            return;
                        }
                    }
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT DriverId, Name FROM Drivers WHERE REPLACE(PhoneNo, '-', '') = '{PhoneNo}' OR REPLACE(MobileNo, '-', '') = '{PhoneNo}'";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            DriverId = _Reader.GetInt32(0);
                            Target = _Reader.GetString(1);
                            Div = "[차주]";
                            CallTarget.Text = Div + Target;
                            return;
                        }
                    }
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Name FROM Contracts WHERE REPLACE(PhoneNo, '-', '') = '{PhoneNo}'";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            Target = _Reader.GetString(0);
                            Div = "";
                            CallTarget.Text = Div + Target;
                            return;
                        }
                    }
                }
            });
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                    "INSERT INTO Calls (OriginalPhoneNo, Target, Div, CustomerId, DriverId, CTime, ClientId, ClientPhoneNo, Memo, LoginId) OUTPUT Inserted.CallId VALUES (@OriginalPhoneNo, @Target, @Div, @CustomerId, @DriverId, @CTime, @ClientId, @ClientPhoneNo, N'', @LoginId)";
                    _Command.Parameters.AddWithValue("@OriginalPhoneNo", OriginalPhoneNo);
                    _Command.Parameters.AddWithValue("@Target", Target);
                    _Command.Parameters.AddWithValue("@Div", Div);
                    _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    _Command.Parameters.AddWithValue("@CTime", CTime);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@ClientPhoneNo", ClientPhoneNo);
                    _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                    CallId = Convert.ToInt32(_Command.ExecuteScalar());
                }
            });
            string _S = PhoneNo;
            if (_S.StartsWith("1"))
            {
                if (_S.Length > 4)
                {
                    _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
                }
                else if (_S.Length > 8)
                {
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
            else if (_S.StartsWith("01"))
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
            CallPhoneNo.Text = _S;
        }

        private void FrmMN301_Call_Popup_Load(object sender, EventArgs e)
        {

        }

        private void ShowImage_Click(object sender, EventArgs e)
        {
            if(CustomerId > 0)
            FrmMDI.Dialog_CustomerImage_Instance.LoadCutomer(CustomerId);
        }

        private void InputOrder_Click(object sender, EventArgs e)
        {
            FrmMDI.LoadCustomerFromCall(CustomerId);
        }

        private void MemoUpdate_Click(object sender, EventArgs e)
        {
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "UPDATE Calls SET Memo = @Memo WHERE CallId = @CallId";
                    _Command.Parameters.AddWithValue("@Memo", Memo.Text);
                    _Command.Parameters.AddWithValue("@CallId", CallId);
                    _Command.ExecuteNonQuery();
                }
            });
        }

        private void CallRe_Click(object sender, EventArgs e)
        {
            CallHelper.Instance.Call(OriginalPhoneNo);
        }
    }
}
