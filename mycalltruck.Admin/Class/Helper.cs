using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin.Class
{
    class Helper
    {
        private static Helper _Instance = new Helper();

        internal static Helper Instance
        {
            get { return Helper._Instance; }
        }

        public bool IsLogined { get; set; }

        private Helper()
        {

        }

        private Guid SessionId { get; set; }

        public void LockWirte(Form form)
        {
            var allControls = GetAll(form);
            foreach (var control in allControls)
            {
                if (control.Tag != null && control.Tag.ToString() == "Write")
                {
                    control.Enabled = false;
                }
            }
        }

        public IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl))
                                      .Concat(controls);
        }

        int AuthCount = 0;

        public bool IsAuthenticated()
        {
            bool r = false;
            var Query =
                "Select Count(*) From AccountInfoes Where SessinId = @SessinId AND ExprireDateTime > GetDate()";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = Query;
                cmd.Parameters.AddWithValue("@SessinId", LocalUser.Instance.LogInInformation.SessionId.ToString());
                var o = cmd.ExecuteScalar();
                int i = 0;
                if (o != null && int.TryParse(o.ToString(), out i) && i > 0)
                {
                    r = true;
                }
                cn.Close();
            }
            if (r)
            {
                AuthCount++;
                if(AuthCount > 5)
                {
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "UPDATE AccountInfoes SET ExprireDateTime = DateAdd(second, 20, GetDate()) WHERE SessinId = @SessinId";
                            _Command.Parameters.AddWithValue("@SessinId", LocalUser.Instance.LogInInformation.SessionId.ToString());
                            _Command.ExecuteNonQuery();
                        }
                    });
                    AuthCount = 0;
                }
            }
            return r;
        }

        public void Logoff()
        {
            if (!IsLogined)
                return;
            //var Query =
            //    "Delete From AccountInfoes Where SessinId = @SessinId";
            //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //{
            //    cn.Open();
            //    SqlCommand cmd = cn.CreateCommand();
            //    cmd.CommandText = Query;
            //    cmd.Parameters.AddWithValue("@SessinId", SessionId.ToString());
            //    var o = cmd.ExecuteNonQuery();
            //    cn.Close();
            //}

            var LoginQuery =
            @"Insert Into LoginLog (ClientId,ClientUserid, ClientName, LoginId, UserName, Gubun, LogDate)
                Values ( @ClientId,@ClientUserid, @ClientName, @LoginId,@UserName,@Gubun, GetDate())";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = LoginQuery;

                if (LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    cmd.Parameters.AddWithValue("@ClientId", 0);
                    cmd.Parameters.AddWithValue("@ClientUserId", 0);
                    cmd.Parameters.AddWithValue("@ClientName", "관리자");
                    cmd.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                    cmd.Parameters.AddWithValue("@UserName", "관리자");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    cmd.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                    cmd.Parameters.AddWithValue("@ClientName", LocalUser.Instance.LogInInformation.ClientName);
                    cmd.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                    cmd.Parameters.AddWithValue("@UserName", LocalUser.Instance.LogInInformation.ClientName);
                }
                cmd.Parameters.AddWithValue("@Gubun", "Logout");
                var o = cmd.ExecuteNonQuery();
                cn.Close();
            }
            IsLogined = false;
        }
        public void CustomerLogin()
        {
           
        }
        public void ClientLogin()
        {
            



            var LoginQuery =
               @"Insert Into LoginLog (ClientId,ClientUserid, ClientName, LoginId, UserName, Gubun, LogDate)
                Values ( @ClientId,@ClientUserid, @ClientName, @LoginId,@UserName,@Gubun, GetDate())";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = LoginQuery;
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                cmd.Parameters.AddWithValue("@ClientName", LocalUser.Instance.LogInInformation.ClientName);
                cmd.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                cmd.Parameters.AddWithValue("@UserName", LocalUser.Instance.LogInInformation.ClientName);
                cmd.Parameters.AddWithValue("@Gubun", "Login");
                var o = cmd.ExecuteNonQuery();
                cn.Close();



            }

          

            LocalUser.Instance.LogInInformation.LoadClient();
            var _Clients = LocalUser.Instance.LogInInformation.Client;

            string CustomerCode = "";
            InitCustomerTable();
            if (_CustomerViewModelList.Where(c => c.BizNo.Replace("-", "") == _Clients.BizNo.Replace("-", "")).Count() == 0)
            {
                if (_CustomerViewModelList.Any())
                {
                    CustomerCode = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();
                }
                else
                {
                    CustomerCode = "1001";
                }
                 var CustomersQuery = @"Insert Into customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart)" +
                    $" Values ( @Code  ,'{_Clients.BizNo}','{_Clients.Name}','{_Clients.CEO}', '{_Clients.Uptae}', '{_Clients.Upjong}','{_Clients.AddressState}', '{_Clients.AddressCity}', '{_Clients.AddressDetail}', 4, '', 3, '{_Clients.Email}', '{_Clients.PhoneNo}', '', '{_Clients.CEO}', '{_Clients.MobileNo}',getdate(), @ClientId, '{_Clients.ZipCode}', NULL, 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, '', 0, 0, 0, getdate(), 0)";
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = CustomersQuery;
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    cmd.Parameters.AddWithValue("@Code", CustomerCode);

                    var o = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }



         

            LocalUser.Instance.LogInInformation.SessionId = SessionId;
            IsLogined = true;
        }


        class CustomerViewModel
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string PhoneNo { get; set; }
            public string Code { get; set; }
            public string BizNo { get; set; }
            // public int CClientId { get; set; }
        }
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>();
        private void InitCustomerTable()
        {
            _CustomerViewModelList.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo,Customers.Code,Customers.BizNo FROM Customers" +
                        " WHERE ClientId = @ClientId  Order by Code DESC";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var dataReader = commnad.ExecuteReader();
                    while (dataReader.Read())
                    {
                        _CustomerViewModelList.Add(
                          new CustomerViewModel
                          {
                              CustomerId = dataReader.GetInt32(0),
                              Name = dataReader.GetStringN(1),
                              PhoneNo = dataReader.GetStringN(2),
                              Code = dataReader.GetStringN(3),
                              BizNo = dataReader.GetStringN(4),
                              // CClientId = dataReader.GetInt32(4),
                          });
                    }
                }
                connection.Close();
            }
        }
        public void AdminLogin()
        {
            //SessionId = Guid.NewGuid();
            //var Query =
            //    @"Insert Into AccountInfoes (ClientOrDriver, SessinId, LoginDateTime, ExprireDateTime, IsUser, UserId)
            //    Values (1, @SessinId, GetDate(), DateAdd(year, 1, GetDate()), 1, @UserId)";
            //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //{
            //    cn.Open();
            //    SqlCommand cmd = cn.CreateCommand();
            //    cmd.CommandText = Query;
            //    cmd.Parameters.AddWithValue("@SessinId", SessionId.ToString());
            //    cmd.Parameters.AddWithValue("@UserId", LocalUser.Instance.LogInInformation.UserId);
            //    var o = cmd.ExecuteNonQuery();
            //    cn.Close();
            //}


            var LoginQuery =
           @"Insert Into LoginLog (ClientId,ClientUserid, ClientName, LoginId, UserName, Gubun, LogDate)
                Values ( @ClientId,@ClientUserid, @ClientName, @LoginId,@UserName,@Gubun, GetDate())";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = LoginQuery;
                cmd.Parameters.AddWithValue("@ClientId", 0);
                cmd.Parameters.AddWithValue("@ClientUserId", 0);
                cmd.Parameters.AddWithValue("@ClientName", "관리자");
                cmd.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                cmd.Parameters.AddWithValue("@UserName", "관리자");
                cmd.Parameters.AddWithValue("@Gubun", "Login");
                var o = cmd.ExecuteNonQuery();
                cn.Close();
            }

            LocalUser.Instance.LogInInformation.SessionId = SessionId;
            IsLogined = true;
        }

        public bool IsAnotherLoginedForClient()
        {
            bool r = false;
            var Query =
                "Select Count(*) From AccountInfoes Where ClientId = @ClientId AND ClientUserId = @ClientUserId AND ExprireDateTime > GetDate()";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = Query;
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                var o = cmd.ExecuteScalar();
                if (o != null && int.TryParse(o.ToString(), out int i) && i > 0)
                {
                    r = true;
                }
                cn.Close();
            }
            return r;
        }

        public bool IsAnotherLoginedForAdmin()
        {
            bool r = false;
            return r;
        }

        public void ForceLogoutAnotherUserForClient()
        {
            var Query =
                "Delete From AccountInfoes Where ClientId = @ClientId AND ClientUserId = @ClientUserId";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = Query;
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                var o = cmd.ExecuteNonQuery();
                cn.Close();
            }

            var LoginQuery =
             @"Insert Into LoginLog (ClientId,ClientUserid, ClientName, LoginId, UserName, Gubun, LogDate)
                Values ( @ClientId,@ClientUserid, @ClientName, @LoginId,@UserName,@Gubun, GetDate())";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = LoginQuery;
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                cmd.Parameters.AddWithValue("@ClientName", LocalUser.Instance.LogInInformation.ClientName);
                cmd.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                cmd.Parameters.AddWithValue("@UserName", LocalUser.Instance.LogInInformation.ClientName);
                cmd.Parameters.AddWithValue("@Gubun", "Logout");
                var o = cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
