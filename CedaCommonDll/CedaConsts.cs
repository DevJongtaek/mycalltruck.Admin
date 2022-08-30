using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace CedaCommonDll
{
    public class CedaConsts
    {
        public enum SIGN_LOCATION_CODE { LU, LL, RU, RL }   // LU : 좌상단, LL : 좌하단, RU : 우상단, RL : 우하단
        public enum STATUS_CODE { _10_ready, _10_prog, _10_error, _20_ready, _20_prog, _20_error, _30_ready, _30_prog, _30_error, _40_ready, _40_prog, _40_error }      // 10 : Pdf 생성, 20 : TimeStamp, 30 : 공전소전송, 40 : 공전소삭제

#if (DEBUG)
        public static string _SERVER_NAME { get; } = "WIN-VG93UA3L1DG";
        public static string _SERVER_IP { get; } = "222.231.9.253,2899";
        public static string _DB_NAME { get; } = "Truck";
        public static string _DB_UID { get; } = "edubillsys";
        public static string _DB_PWD { get; } = "edubillsysdb2202#$";
        public static string _TSA_URL { get; } = "http://203.247.128.200:8015/TSA/tsa.jsp";
#else
        public static string _SERVER_NAME { get; } = "WIN-VG93UA3L1DG";
        public static string _SERVER_IP { get; } = "222.231.9.253,2899";
        public static string _DB_NAME { get; } = "Truck";
        public static string _DB_UID { get; } = "edubillsys";
        public static string _DB_PWD { get; } = "edubillsysdb2202#$";
        public static string _TSA_URL { get; } = "http://203.247.128.200:8015/TSA/tsa.jsp";
#endif

    public static MsSqlUtility GetDBclass()
        {
            try { return new MsSqlUtility(_SERVER_NAME, _SERVER_IP, _DB_NAME, _DB_UID, _DB_PWD); }
            catch (Exception) { throw; }
        }

        public static List<string> GetTargetList(CedaConsts.STATUS_CODE pSourceStatus, CedaConsts.STATUS_CODE pTargetStatue, string pTradeId, int pThreadCount, int pThreadSeq)
        {
            try
            {
                List<string> lErrorList = null;
                using (MsSqlUtility lDbUtil = CedaConsts.GetDBclass())
                {
                    lDbUtil.beginTransaction();
                    try
                    {
                        lDbUtil.clearParameterList();
                        StringBuilder lSqlString = new StringBuilder();
                        lSqlString.AppendLine(" SELECT TradeId ");
                        lSqlString.AppendLine(" FROM DocuTable WITH (UPDLOCK) ");
                        lSqlString.AppendLine(" WHERE Status = @pSourceStatus ");
                        lDbUtil.addSqlParameter("@pSourceStatus", pSourceStatus.ToString());
                        if (string.IsNullOrWhiteSpace(pTradeId) != true)
                        {
                            lSqlString.AppendLine(" AND   TradeId = @pTradeId ");
                            lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                        }
                        if (pThreadCount > 1)
                        {
                            lSqlString.AppendLine(" AND   TradeId % @pThreadCount = @pThreadSeq ");
                            lDbUtil.addSqlParameter("@pThreadCount", pThreadCount);
                            lDbUtil.addSqlParameter("@pThreadSeq", pThreadSeq == pThreadCount ? 0 : pThreadSeq);
                        }
                        using (SqlDataReader AdoRs = lDbUtil.selectDB(lSqlString.ToString()))
                        {
                            if (AdoRs != null)
                            {
                                lErrorList = new List<string>();
                                while (AdoRs.Read() == true) { lErrorList.Add(lDbUtil.getDbValue(AdoRs["TradeId"])); }
                                AdoRs.Close();
                            }
                        }
                        lDbUtil.clearParameterList();
                        lSqlString.Clear();
                        lSqlString.AppendLine(" UPDATE DocuTable ");
                        lSqlString.AppendLine(" SET Status = @pTargetStatue, ");
                        lDbUtil.addSqlParameter("@pTargetStatue", pTargetStatue.ToString());
                        lSqlString.AppendLine("     ErrMsg = @pErrorMessage, ");
                        lDbUtil.addSqlParameter("@pErrorMessage", DBNull.Value);
                        lSqlString.AppendLine("     UpdateDateTime = GETDATE() ");
                        lSqlString.AppendLine(" WHERE Status = @pSourceStatus ");
                        lDbUtil.addSqlParameter("@pSourceStatus", pSourceStatus.ToString());
                        if (string.IsNullOrWhiteSpace(pTradeId) != true)
                        {
                            lSqlString.AppendLine(" AND   TradeId = @pTradeId ");
                            lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                        }
                        if (pThreadCount > 1)
                        {
                            lSqlString.AppendLine(" AND   TradeId % @pThreadCount = @pThreadSeq ");
                            lDbUtil.addSqlParameter("@pThreadCount", pThreadCount);
                            lDbUtil.addSqlParameter("@pThreadSeq", pThreadSeq == pThreadCount ? 0 : pThreadSeq);
                        }
                        lDbUtil.executeDB(lSqlString.ToString());
                        lDbUtil.commitTransaction();
                    }
                    catch (Exception) { lDbUtil.rollbackTransaction(); throw; }
                }
                return lErrorList;
            }
            catch (Exception) { throw; }
        }

        public static void SaveStatusData(string pTradeId, string pStatusCode, string pErrorMessage)
        {
            try
            {
                using (MsSqlUtility lDbUtil = CedaConsts.GetDBclass())
                {
                    lDbUtil.clearParameterList();
                    StringBuilder lSqlString = new StringBuilder();
                    lSqlString.AppendLine(" UPDATE DocuTable ");
                    lSqlString.AppendLine(" SET Status = @pStatusCode, ");
                    lDbUtil.addSqlParameter("@pStatusCode", pStatusCode);
                    lSqlString.AppendLine("     ErrMsg = @pErrorMessage, ");
                    if (string.IsNullOrWhiteSpace(pErrorMessage) == true) { lDbUtil.addSqlParameter("@pErrorMessage", DBNull.Value); }
                    else { lDbUtil.addSqlParameter("@pErrorMessage", pErrorMessage); }
                    lSqlString.AppendLine("     UpdateDateTime = GETDATE() ");
                    lSqlString.AppendLine(" WHERE TradeId = @pTradeId ");
                    lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                    lDbUtil.executeDB(lSqlString.ToString());
                }
            }
            catch (Exception) { throw; }
        }

        public static void SaveLog(string pLogText)
        {
            try
            {
                string lDir = "./logs";
                if (Directory.Exists(lDir) != true) { Directory.CreateDirectory(lDir); }
                using (FileStream lFileStream = File.Open(string.Concat(lDir, "/", DateTime.Now.ToString("yyyyMMdd"), ".log"), FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    using (StreamWriter lStreamWriter = new StreamWriter(lFileStream))
                    {
                        lStreamWriter.Write(string.Concat("[", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "]", pLogText, Environment.NewLine, Environment.NewLine));
                        lStreamWriter.Flush();
                        lStreamWriter.Close();
                    }
                    lFileStream.Close();
                }
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }
        }
    }
}
