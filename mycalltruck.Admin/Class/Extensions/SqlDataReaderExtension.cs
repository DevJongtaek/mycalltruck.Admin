using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static int? GetInt32N(this SqlDataReader _Reader, int Index)
        {
            int? r = null;
            if (!_Reader.IsDBNull(Index))
                r = _Reader.GetInt32(Index);
            return r;
        }
        public static int GetInt32Z(this SqlDataReader _Reader, int Index)
        {
            int r = 0;
            if (!_Reader.IsDBNull(Index))
                r = _Reader.GetInt32(Index);
            return r;
        }
        public static string GetStringN(this SqlDataReader _Reader, int Index)
        {
            String r = "";
            if (!_Reader.IsDBNull(Index))
                r = _Reader.GetString(Index);
            return r;
        }
        public static bool GetBooleanZ(this SqlDataReader _Reader, int Index)
        {
            bool r = false;
            if (!_Reader.IsDBNull(Index))
                r = _Reader.GetBoolean(Index);
            return r;
        }
    }
}
