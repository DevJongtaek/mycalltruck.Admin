using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.DataSet
{
    class CustomerRepository
    {
        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();
        private List<String> SanghoCompareTable = new List<String>();
        private void InitCompareTable()
        {
            CodeCompareTable.Clear();
            BizNoCompareTable.Clear();
            SanghoCompareTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (var _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT Code,BizNo,Sangho FROM Customers WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            CodeCompareTable.Add(_Reader.GetString(0));
                            BizNoCompareTable.Add(_Reader.GetString(1));
                            SanghoCompareTable.Add(_Reader.GetString(2));
                        }
                    }
                }
                _Connection.Close();
            }
            CodeCompareTable.Sort();
        }
        public int _Insert(String Name, String MobileNo, String PhoneNo)
        {

            InitCompareTable();


            int CustomerId = 0;

            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    Int64 _iBizNo = 1111110001;
                    string _BizNo = "";
                    while (true)
                    {
                        if (!BizNoCompareTable.Any(c => Convert.ToInt64(c.Replace("-","")) == _iBizNo))
                        {
                            break;
                        }
                        _iBizNo++;
                    }

                    _BizNo = _iBizNo.ToString().Substring(0, 3) + "-" + _iBizNo.ToString().Substring(3, 2) + "-" + _iBizNo.ToString().Substring(5, 5);

                    _Command.CommandText = @"INSERT INTO Customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Zipcode, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, SubClientId, EndDay, ClientUserId, LoginId, Password,Remark,CustomerManagerId,Mstart,PointMethod)
                        VALUES (@Code, @BizNo, @SangHo, @Ceo, @Uptae, @Upjong, @AddressState, @AddressCity, @AddressDetail, @Zipcode, @BizGubun, @ResgisterNo, @SalesGubun, @Email, @PhoneNo, @FaxNo, @ChargeName, @MobileNo, @CreateTime, @ClientId, @SubClientId, @EndDay, @ClientUserId, @LoginId, @Password,@Remark,@CustomerManagerId,0,1) SELECT @@IDENTITY";

                    int CustomerCode = 1001;
                    while (true)
                    {
                        if (!CodeCompareTable.Any(c => c == CustomerCode.ToString()))
                        {
                            break;
                        }
                        CustomerCode++;
                    }

                    _Command.Parameters.AddWithValue("@Code", CustomerCode);
                    _Command.Parameters.AddWithValue("@BizNo", _BizNo);
                    _Command.Parameters.AddWithValue("@SangHo", Name);
                    _Command.Parameters.AddWithValue("@Ceo", "대표자");
                    _Command.Parameters.AddWithValue("@Uptae", ".");
                    _Command.Parameters.AddWithValue("@Upjong", ".");

                    _Command.Parameters.AddWithValue("@AddressState", "");
                    _Command.Parameters.AddWithValue("@AddressCity", "");
                    _Command.Parameters.AddWithValue("@AddressDetail", "");
                    _Command.Parameters.AddWithValue("@Zipcode", "00000");

                    _Command.Parameters.AddWithValue("@BizGubun", 1);
                    _Command.Parameters.AddWithValue("@ResgisterNo", "");
                    _Command.Parameters.AddWithValue("@SalesGubun", 1);
                    _Command.Parameters.AddWithValue("@Email", "");
                    _Command.Parameters.AddWithValue("@PhoneNo", PhoneNo);

                    _Command.Parameters.AddWithValue("@FaxNo", "");
                    _Command.Parameters.AddWithValue("@ChargeName", "");

                    _Command.Parameters.AddWithValue("@MobileNo", MobileNo);
                    _Command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                    

                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    _Command.Parameters.AddWithValue("@SubClientId", DBNull.Value);
                    _Command.Parameters.AddWithValue("@EndDay", 1);
                    _Command.Parameters.AddWithValue("@ClientUserId", DBNull.Value);
                    _Command.Parameters.AddWithValue("@LoginId", DBNull.Value);
                    _Command.Parameters.AddWithValue("@Password", DBNull.Value);
                    _Command.Parameters.AddWithValue("@Remark","");
                    _Command.Parameters.AddWithValue("@CustomerManagerId", 0);



                    Object O = _Command.ExecuteScalar();
                    if (O != null)
                        CustomerId = Convert.ToInt32(O);

                }
            });

            return CustomerId;
        }

        public bool IsMyCustomer(String BizNo)
        {
            BizNo = BizNo.Replace("-", "").Trim();
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                         //@"SELECT    
                         //        Customers.CustomerId 
                         //    FROM    Customers
                         //    LEFT JOIN    CustomerInstances ON Customers.CustomerId = CustomerInstances.CustomerId
                         //    WHERE REPLACE(Customers.BizNo,'-','') = @BizNo AND CustomerInstances.ClientId = @ClientId ";
                         @"SELECT    
                                Customers.CustomerId 
                            FROM    Customers
                         
                            WHERE REPLACE(Customers.BizNo,'-','') = @BizNo AND Customers.ClientId = @ClientId ";

                    _Command.Parameters.AddWithValue("@BizNo", BizNo);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public void FindbyBizNo(DataTable CustomerTable, String BizNo)
        {
            BizNo = BizNo.Replace("-", "").Trim();
            CustomerTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT CustomerId, Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, DriverId, Fee, LoginId, Password, Remark, CustomerManagerId, Misu, Mizi, MStartDate, MStart FROM Customers
                            WHERE REPLACE(Customers.BizNo,'-','') = @BizNo  AND ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@BizNo", BizNo);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        CustomerTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        //public void ConnectCustomer(int CustomerId)
        //{
        //    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //    {
        //        _Connection.Open();
        //        using (SqlCommand DriverInstanceCommand = _Connection.CreateCommand())
        //        {
        //            DriverInstanceCommand.CommandText =
        //                @"INSERT INTO CustomerInstances (CustomerId, ClientId) VALUES (@CustomerId, @ClientId)";
        //            DriverInstanceCommand.Parameters.AddWithValue("@CustomerId", CustomerId);
        //            DriverInstanceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
        //            DriverInstanceCommand.ExecuteNonQuery();
        //        }
        //        _Connection.Close();
        //    }
        //}

        public int GetCustomerId(String BizNo)
        {
            BizNo = BizNo.Replace("-", "").Trim();
            int CustomerId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                       //@"SELECT    
                       //          Customers.CustomerId 
                       //      FROM    Customers
                       //      LEFT JOIN    CustomerInstances ON Customers.CustomerId = CustomerInstances.CustomerId
                       //      WHERE REPLACE(Customers.BizNo,'-','') = @BizNo AND Customers.ClientId = @ClientId ";
                       @"SELECT    
                                Customers.CustomerId 
                            FROM    Customers
                           
                            WHERE REPLACE(Customers.BizNo,'-','') = @BizNo AND Customers.ClientId = @ClientId ";
                    _Command.Parameters.AddWithValue("@BizNo", BizNo);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        CustomerId = Convert.ToInt32(O);
                }
                _Connection.Close();
            }
            return CustomerId;
        }
    }
}
