using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalltruck.Admin.Class.Common
{
    public class OleExcel
    {
        private static OleDbConnection CreateConnection(string ExcelPath)
        {
            OleDbConnectionStringBuilder ConnectionBuilder = new OleDbConnectionStringBuilder();
            ConnectionBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
            ConnectionBuilder.DataSource = ExcelPath;
            ConnectionBuilder.Add("Extended Properties", "Excel 8.0");
            return new OleDbConnection(ConnectionBuilder.ToString());
        }
        public static bool Export(DataTable Table, string ExcelPath, string SheetName)
        {
            DataTable Result = new DataTable();
            using (OleDbConnection Connection = CreateConnection(ExcelPath))
            {
                Connection.Open();
                OleDbCommand TempCmd = Connection.CreateCommand();
                TempCmd.CommandText = CreateInsertQuery(Table, TempCmd.Parameters, SheetName);
                foreach (DataRow Row in Table.Rows)
                {
                    for (int i = 0; i < TempCmd.Parameters.Count; i++)
                        TempCmd.Parameters[i].Value = Row[i];
                    try
                    {
                        TempCmd.ExecuteScalar();
                    }
                    catch { }
                }
                Connection.Close();
            }
            return true;
        }
        public static bool Export(Object[,] formmatedValues, string ExcelPath, string Title, System.Windows.Forms.ProgressBar bar)
        {
            if (formmatedValues.Length > 0)
            {
                DataTable Result = new DataTable();
                using (OleDbConnection Connection = CreateConnection(ExcelPath))
                {
                    Connection.Open();
                    OleDbCommand TempCmd = Connection.CreateCommand();
                    TempCmd.CommandText = CreateInsertQuery(formmatedValues.GetLength(1), TempCmd.Parameters, "Sheet1");
                    for (int i = 0; i < formmatedValues.GetLength(0); i++)
                    {
                        for (int j = 0; j < formmatedValues.GetLength(1); j++)
                        {
                            object o = formmatedValues[i, j];
                            //if (o is DateTime || o is string)
                            //    TempCmd.Parameters[j].Value = "'" + formmatedValues[i, j];
                            //else
                            TempCmd.Parameters[j].Value = formmatedValues[i, j];
                            if (bar.InvokeRequired)
                            {
                                bar.Invoke(new Action(() => bar.PerformStep()));
                            }
                            else
                                bar.PerformStep();
                        }
                        try
                        {
                            TempCmd.ExecuteScalar();
                        }
                        catch { }
                    }
                    Connection.Close();
                }
            }
            return true;
        }
        //public static bool Export(DataTable Table, string ExcelPath, string SheetName, string DeleteCol, string DeleteValue)
        //{
        //    DataTable Result = new DataTable();
        //    using (OleDbConnection Connection = CreateConnection(ExcelPath))
        //    {
        //        Connection.Open();
        //        OleDbCommand TempCmd = Connection.CreateCommand();
        //        TempCmd.CommandText = CreateInsertQuery(Table, TempCmd.Parameters, SheetName);
        //        foreach (DataRow Row in Table.Rows)
        //        {
        //            for (int i = 0; i < TempCmd.Parameters.Count; i++)
        //                TempCmd.Parameters[i].Value = Row[i];
        //            try
        //            {
        //                TempCmd.ExecuteScalar();
        //            }
        //            catch { }
        //        }
        //        TempCmd.CommandText = CreateDeleteQuery(SheetName, DeleteCol, DeleteValue);
        //        TempCmd.ExecuteNonQuery();
        //        Connection.Close();
        //    }
        //    return true;
        //}
        public static DataTable Import(string ExcelPath, string SheetName)
        {
            DataTable Result = new DataTable();
            using (OleDbConnection Connection = CreateConnection(ExcelPath))
            {
                Connection.Open();
                OleDbCommand SelectCommand = Connection.CreateCommand();
                SelectCommand.CommandText = "SELECT * FROM [" + SheetName + "$]";
                Result.Load(SelectCommand.ExecuteReader());
                Connection.Close();
            }
            return Result;
        }
        public static string CreateTableQury(DataTable Table, string SheetName)
        {
            string Query = "CREATE TABLE [" + SheetName + "] (";
            for (int i = 0; i < Table.Columns.Count; i++)
            {
                Query += Table.Columns[i].ColumnName + " text";
                if (i < Table.Columns.Count - 1) Query += ", ";
                else Query += ")";
            }
            return Query;
        }

        public static string CreateInsertQuery(DataTable Table, OleDbParameterCollection Parameters, string SheetName)
        {
            string Query = @"INSERT INTO [" + SheetName + "$] VALUES (";
            for (int i = 0; i < Table.Columns.Count; i++)
            {
                Query += "@Param" + i.ToString();
                if (i < Table.Columns.Count - 1) Query += ", ";
                else Query += ")";
                OleDbParameter Parameter = new OleDbParameter("@Param" + i.ToString(), DbType.String);
                Parameters.Add(Parameter);
            }
            return Query;
        }
        public static string CreateInsertQuery(int arrayCount, OleDbParameterCollection Parameters, string SheetName)
        {
            string Query = @"INSERT INTO [" + SheetName + "$] VALUES (";
            for (int i = 0; i < arrayCount; i++)
            {
                Query += "@Param" + i.ToString();
                if (i < arrayCount - 1) Query += ", ";
                else Query += ")";
                OleDbParameter Parameter = new OleDbParameter("@Param" + i.ToString(), DbType.String);
                Parameters.Add(Parameter);
            }
            return Query;
        }
        public static string CreateDeleteQuery(string SheetName, String ColName, String Value)
        {
            string Query = String.Format(@"DELETE FROM [{0}$] WHERE {1}='{2}'", SheetName, ColName, Value);
            return Query;
        }
    }
}
