using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.Common
{
   public class ExcelOpen : IDisposable
    {

        //Private Value
        OleDbConnection cn;
        //Static Method
        private static OleDbConnection CreateConnection(string fileName)
        {
            if (fileName.Substring(fileName.Length - 3, 3).ToLower() == "xls") ;
            else
                throw new Exception("확장자가 xls인 엑셀파일만 사용 할 수 있습니다.");

            OleDbConnectionStringBuilder ConnectionBuilder = new OleDbConnectionStringBuilder();
            ConnectionBuilder.DataSource = fileName;
            ConnectionBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
            ConnectionBuilder.Add("Extended Properties", "Excel 8.0");
            return new OleDbConnection(ConnectionBuilder.ToString());
        }
        //Constructor
        public ExcelOpen(string fileName)
        {
            cn = CreateConnection(fileName);
        }
        //Method
        public DataTable Open()
        {
            DataTable r = new DataTable("Sheet1");
            string sqlString = "SELECT * FROM [Sheet1$]";
            OleDbCommand selectCmd = new OleDbCommand(sqlString, cn);
            cn.Open();
            r.Load(selectCmd.ExecuteReader());
            cn.Close();
            return r;
        }
        //IDispose
        public void Dispose()
        {
            cn = null;
        }

    }
}
