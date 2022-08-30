using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;

namespace CedaCommonDll
{
    public sealed class MsSqlUtility : IDisposable
    {
        private string _ServerName { get; }
        private string _ServerIp { get; }
        private string _DbName { get; }
        private string _UserId { get; }
        private string _UserPassword { get; }

        private SqlConnection _Connection { get; set; }
        private SqlTransaction _Transaction { get; set; }
        private List<SqlParameter> _ParameterList { get; set; }

        public MsSqlUtility(string pServerName, string pServerIp, string pDbName, string pUserId, string pUserPassword)
        {
            try
            {
                this._ServerName = pServerName;
                this._ServerIp = pServerIp;
                this._DbName = pDbName;
                this._UserId = pUserId;
                this._UserPassword = pUserPassword;
                this.openDB();
            }
            catch (Exception) { throw; }
        }

        ~MsSqlUtility()
        {
            try { this.Dispose(true); }
            catch (Exception) { throw; }
        }

        public void Dispose()
        {
            try { this.Dispose(false); }
            catch (Exception) { throw; }
        }

        private void Dispose(bool pFinalize)
        {
            try
            {
                if (pFinalize != true) { GC.SuppressFinalize(this); }
                this.closeDB();
            }
            catch (Exception) { throw; }
        }

        private void openDB()
        {
            try
            {
                if (this._Connection != null && this._Connection.State.Equals(ConnectionState.Open) == true) { return; }
                string lConnectString = string.Concat("Data Source=", this._ServerIp, ";User ID=", this._UserId, ";Password=", this._UserPassword, ";Initial Catalog=", this._DbName, ";Persist Security Info=true;Connection Timeout=3");
                this.closeDB();
                this._Connection = new SqlConnection(lConnectString);
                this._Connection.Open();
            }
            catch (Exception) { throw; }
        }

        private void closeDB()
        {
            try
            {
                this.clearParameterList();
                this.closeTransaction();
                if (this._Connection != null)
                {
                    if (this._Connection.State != ConnectionState.Closed) { this._Connection.Close(); }
                    this._Connection.Dispose(); this._Connection = null;
                }
            }
            catch (Exception) { throw; }
        }

        public void beginTransaction()
        {
            try { this._Transaction = this._Connection.BeginTransaction(IsolationLevel.Serializable); }
            catch (Exception) { throw; }
        }

        public void commitTransaction()
        {
            try
            {
                if (this._Transaction != null) { this._Transaction.Commit(); }
                this.closeTransaction();
            }
            catch (Exception) { throw; }
        }

        public void rollbackTransaction()
        {
            try
            {
                if (this._Transaction != null) { this._Transaction.Rollback(); }
                this.closeTransaction();
            }
            catch (Exception) { throw; }
        }

        private void closeTransaction()
        {
            try
            {
                if (this._Transaction != null)
                {
                    if (this._Transaction.Connection != null) { this._Transaction.Rollback(); }
                    this._Transaction.Dispose(); this._Transaction = null;
                }
            }
            catch (Exception) { throw; }
        }
        
        public SqlDataReader selectDB(string pQueryString)
        {
            try
            {
                this.printDebug(pQueryString);
                using (SqlCommand lSqlCommand = new SqlCommand(pQueryString, this._Connection))
                {
                    if (this._Transaction != null) { lSqlCommand.Transaction = this._Transaction; }
                    if (this._ParameterList != null && this._ParameterList.Count > 0) { lSqlCommand.Parameters.AddRange(this._ParameterList.ToArray()); }
                    return lSqlCommand.ExecuteReader();
                }
            }
            catch (Exception) { throw; }
            finally { this.clearParameterList(); }
        }

        public long executeDB(string pQueryString)
        {
            try
            {
                this.printDebug(pQueryString);
                using (SqlCommand lSqlCommand = new SqlCommand(pQueryString, this._Connection))
                {
                    if (this._Transaction != null) { lSqlCommand.Transaction = this._Transaction; }
                    if (this._ParameterList != null && this._ParameterList.Count > 0) { lSqlCommand.Parameters.AddRange(this._ParameterList.ToArray()); }
                    return lSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }
            finally { this.clearParameterList(); }
        }

        public void addSqlParameter(string pParameterName, object pParameterValue)
        {
            try { this.addSqlParameter(new SqlParameter(pParameterName, pParameterValue)); }
            catch (Exception) { throw; }
        }

        public void addSqlParameter(string pParameterName, SqlDbType pColumnType, object pParameterValue)
        {
            try
            {
                SqlParameter lParameter = new SqlParameter(pParameterName, pColumnType);
                lParameter.SqlValue = pParameterValue;
                this.addSqlParameter(lParameter);
            }
            catch (Exception) { throw; }
        }

        public void addSqlParameter(string pParameterName, SqlDbType pColumnType, int pColumnSize, object pParameterValue)
        {
            try
            {
                SqlParameter lParameter = new SqlParameter(pParameterName, pColumnType, pColumnSize);
                lParameter.SqlValue = pParameterValue;
                this.addSqlParameter(lParameter);
            }
            catch (Exception) { throw; }
        }

        public void addSqlParameter(SqlParameter pParameter)
        {
            try
            {
                if (this._ParameterList == null) { this._ParameterList = new List<SqlParameter>(); }
                else { foreach (SqlParameter lParameter in this._ParameterList) { if (lParameter.ParameterName.Equals(pParameter.ParameterName) == true) { lParameter.Value = pParameter.Value; return; } } }
                this._ParameterList.Add(pParameter);
            }
            catch (Exception) { throw; }
        }

        public void clearParameterList()
        {
            try { if (this._ParameterList != null) { this._ParameterList.Clear(); this._ParameterList = null; } }
            catch (Exception) { throw; }
        }
        
        public string getDbValue(object objValue)
        {
            try
            {
                if (objValue == null) { return string.Empty; }
                if (objValue is bool) { return ((bool)objValue).ToString(); }
                if (objValue is sbyte) { return ((sbyte)objValue).ToString(); }
                if (objValue is byte) { return ((byte)objValue).ToString(); }
                if (objValue is short) { return ((short)objValue).ToString(); }
                if (objValue is ushort) { return ((ushort)objValue).ToString(); }
                if (objValue is int) { return ((int)objValue).ToString(); }
                if (objValue is uint) { return ((uint)objValue).ToString(); }
                if (objValue is long) { return ((long)objValue).ToString(); }
                if (objValue is ulong) { return ((ulong)objValue).ToString(); }
                if (objValue is float) { return ((float)objValue).ToString("0.#########"); }
                if (objValue is double) { return ((double)objValue).ToString("0.#########"); }
                if (objValue is decimal) { return ((decimal)objValue).ToString("0.#########"); }
                if (objValue is char) { return ((char)objValue).ToString(); }
                if (objValue is string) { return ((string)objValue).ToString(); }
                if (objValue is byte[]) { return new ConvertUtility().getHexStringFromBytes((byte[])objValue); }
                if (objValue is TimeSpan) { return ((TimeSpan)objValue).ToString(); }
                if (objValue is DateTime)
                {
                    if (((DateTime)objValue).ToString("HHmmssfff").Equals("000000000") == true) { return ((DateTime)objValue).ToString("yyyy-MM-dd"); }
                    else { return ((DateTime)objValue).ToString("yyyy-MM-dd HH:mm:ss"); }
                    //return ((DateTime)objValue).ToString("yyyyMMddHHmmss");
                }
                return objValue.ToString();
            }
            catch (Exception) { throw; }
        }

        public DateTime getDbDateTime()
        {
            try
            {
                DateTime lDateTime = DateTime.Today;
                string pQueryString = " SELECT GETDATE() ";
                using (SqlDataReader AdoRs = this.selectDB(pQueryString))
                {
                    if (AdoRs != null) { while (AdoRs.Read() == true) { lDateTime = (DateTime)AdoRs[0]; } }
                    AdoRs.Close();
                }
                return lDateTime;
            }
            catch (Exception) { throw; }
        }

        private void printDebug(string pQueryString)
        {
            try
            {
                Debug.WriteLine(pQueryString);
                if (this._ParameterList != null) { foreach (SqlParameter lParameter in this._ParameterList) { Debug.WriteLine(string.Concat(lParameter.ParameterName, " : ", lParameter.Value)); } }
                Debug.WriteLine(string.Empty);
            }
            catch (Exception) { throw; }
        }
    }
}