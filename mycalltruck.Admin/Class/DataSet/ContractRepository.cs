using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.DataSet
{
    class ContractRepository
    {
        private void _Insert(String Name, String PhoneNo)
        {
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "INSERT INTO (ClientId, PhoneNo, Name) VALUES (@ClientId, @PhoneNo, @Name)";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                    _Command.Parameters.AddWithValue("@Name", Name);
                    _Command.ExecuteNonQuery();
                }
            });
        }


    }
}
