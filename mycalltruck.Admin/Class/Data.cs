using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class
{
    class Data
    {
        public static void Connection(Action<SqlConnection> Job)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                Job(_Connection);
                _Connection.Close();
            }
        }
    }
}
