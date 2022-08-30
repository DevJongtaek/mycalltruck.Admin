using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.DataSet
{
    public class DriverRepository
    {
        public void Select(DataTable DriverTable, List<String> _WhereStringList = null)
        {
            List<String> WhereStringList = new List<string>();
            DriverTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {

                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        _Command.CommandText =
                            @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount, Drivers.VBankName,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                ISNULL(DriverInstances.SubClientId, 0) AS SubClientId, ISNULL(DriverInstances.ClientUserId, 0) AS ClientUserId, ISNULL(DriverInstances.GroupName, '0') AS GroupName,
                                ISNULL(DriverInstances.CarGubun, 0) AS CarGubun, ISNULL(DriverInstances.RequestFrom, N'') AS RequestFrom, ISNULL(DriverInstances.RequestTo, N'') AS RequestTo,Drivers.DealerId,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                           JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId";
                        if (LocalUser.Instance.LogInInformation.Client.HasPoint)
                        {
                            _Command.CommandText =
                                @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount, Drivers.VBankName,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                ISNULL(DriverInstances.SubClientId, 0) AS SubClientId, ISNULL(DriverInstances.ClientUserId, 0) AS ClientUserId, ISNULL(DriverInstances.GroupName, '0') AS GroupName
                                , ISNULL(DriverInstances.CarGubun, 0) AS CarGubun, ISNULL(DriverInstances.RequestFrom, N'') AS RequestFrom, ISNULL(DriverInstances.RequestTo, N'') AS RequestTo
                                , ISNULL(DriverPoints.Point, 0) AS POINT,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                            JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            LEFT JOIN (SELECT SUM(Amount) AS Point, DriverId, ClientId FROM DriverPoints GROUP BY Driverid, ClientId ) DriverPoints ON DriverInstances.DriverId = DriverPoints.DriverId AND DriverInstances.ClientId = DriverPoints.ClientId";
                        }
                        WhereStringList.Add("DriverInstances.ClientId = " + LocalUser.Instance.LogInInformation.ClientId.ToString() + " AND Drivers.ServiceState <> 5");
                        
                    }
                    else
                    {
                        _Command.CommandText =
                            @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount, Drivers.VBankName,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                0 AS SubClientId, 0 AS ClientUserId, '0' AS GroupName, 0 AS CarGubun, '' AS RequestFrom, '' AS RequestTo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                            JOIN Clients ON Drivers.CandidateId = Clients.ClientId
	                        JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId AND DriverInstances.ClientId = clients.ClientId";
                    }
                    if (_WhereStringList != null)
                        WhereStringList.AddRange(_WhereStringList);
                    if(WhereStringList.Count > 0)
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += ("WHERE "+ String.Join(" AND ", WhereStringList));
                    }
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += " Order by Drivers.CreateDate DESC";
                    }
                    else
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += " Order by DriverInstances.DriverInstanceId DESC";
                    }

                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        DriverTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        public void Select_SG(DataTable DriverTable, List<String> _WhereStringList = null)
        {
            List<String> WhereStringList = new List<string>
            {
                "Drivers.ServiceType = 2"
            };
            DriverTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {

                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        _Command.CommandText =
                            @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                ISNULL(DriverInstances.SubClientId, 0) AS SubClientId, ISNULL(DriverInstances.ClientUserId, 0) AS ClientUserId, ISNULL(DriverInstances.GroupName, '0') AS GroupName,
                                ISNULL(DriverInstances.CarGubun, 0) AS CarGubun, ISNULL(DriverInstances.RequestFrom, N'') AS RequestFrom, ISNULL(DriverInstances.RequestTo, N'') AS RequestTo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                          LEFT  JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId";
                        if (LocalUser.Instance.LogInInformation.Client.HasPoint)
                        {
                            _Command.CommandText =
                                @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                ISNULL(DriverInstances.SubClientId, 0) AS SubClientId, ISNULL(DriverInstances.ClientUserId, 0) AS ClientUserId, ISNULL(DriverInstances.GroupName, '0') AS GroupName
                                , ISNULL(DriverInstances.CarGubun, 0) AS CarGubun, ISNULL(DriverInstances.RequestFrom, N'') AS RequestFrom, ISNULL(DriverInstances.RequestTo, N'') AS RequestTo
                                , ISNULL(DriverPoints.Point, 0) AS POINT,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                         LEFT   JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            LEFT JOIN (SELECT SUM(Amount) AS Point, DriverId, ClientId FROM DriverPoints GROUP BY Driverid, ClientId ) DriverPoints ON DriverInstances.DriverId = DriverPoints.DriverId AND DriverInstances.ClientId = DriverPoints.ClientId";
                        }
                        WhereStringList.Add("DriverInstances.ClientId = " + LocalUser.Instance.LogInInformation.ClientId.ToString() + " AND Drivers.ServiceState <> 5");

                    }
                    else
                    {
                        _Command.CommandText =
                            @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                0 AS SubClientId, 0 AS ClientUserId, '0' AS GroupName, 0 AS CarGubun, '' AS RequestFrom, '' AS RequestTo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                            JOIN Clients ON Drivers.CandidateId = Clients.ClientId
                            LEFT   JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId";
                    }
                    if (_WhereStringList != null)
                        WhereStringList.AddRange(_WhereStringList);
                    if (WhereStringList.Count > 0)
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += ("WHERE " + String.Join(" AND ", WhereStringList));
                    }
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += " Order by Drivers.CreateDate DESC";
                    }
                    else
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += " Order by DriverInstances.DriverInstanceId DESC";
                    }

                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        DriverTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        public void Select_Self(DataTable DriverTable)
        {
            List<String> WhereStringList = new List<string>
            {
                "Drivers.ServiceType = 3"
            };
            DriverTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {

                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        _Command.CommandText =
                            @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed, Drivers.CarInfo, Drivers.Memo,

                                0 AS SubClientId, 0 AS ClientUserId, '0' AS GroupName,
                                0 AS CarGubun, N'' AS RequestFrom, N'' AS RequestTo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers  LEFT   JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId ";
                        WhereStringList.Add("Drivers.CandidateId = " + LocalUser.Instance.LogInInformation.ClientId.ToString());

                    }
                    if (WhereStringList.Count > 0)
                    {
                        _Command.CommandText += Environment.NewLine;
                        _Command.CommandText += ("WHERE " + String.Join(" AND ", WhereStringList));
                    }

                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        DriverTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        public void FindbyCarNo(DataTable DriverTable, String CarNo)
        {
            CarNo = CarNo.Replace(" ", "").Trim();
            DriverTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarGubun, Drivers.RequestFrom, Drivers.RequestTo, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed,

                                Drivers.SubClientId, Drivers.ClientUserId, Drivers.CarInfo, Drivers.Memo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                            LEFT   JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            WHERE Drivers.CarNo = @CarNo AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@CarNo", CarNo);
                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        DriverTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        public void SelectOne(DataTable DriverTable, int DriverId)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {

                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        _Command.CommandText =
                            @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed,

                                ISNULL(DriverInstances.SubClientId, 0) AS SubClientId, ISNULL(DriverInstances.ClientUserId, 0) AS ClientUserId, ISNULL(DriverInstances.GroupName, '0') AS GroupName
                                , ISNULL(DriverInstances.CarGubun, 0) AS CarGubun, ISNULL(DriverInstances.RequestFrom, N'') AS RequestFrom, ISNULL(DriverInstances.RequestTo, N'') AS RequestTo
                                , Drivers.CarInfo, Drivers.Memo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                          LEFT  JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            WHERE Drivers.DriverId = @DriverId AND Drivers.ServiceState <> 5";
                        if (LocalUser.Instance.LogInInformation.Client.HasPoint)
                        {
                            _Command.CommandText =
                                @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed,

                                ISNULL(DriverInstances.SubClientId, 0) AS SubClientId, ISNULL(DriverInstances.ClientUserId, 0) AS ClientUserId, ISNULL(DriverInstances.GroupName, '0') AS GroupName
                                , ISNULL(DriverInstances.CarGubun, 0) AS CarGubun, ISNULL(DriverInstances.RequestFrom, N'') AS RequestFrom, ISNULL(DriverInstances.RequestTo, N'') AS RequestTo
                                , ISNULL(DriverPoints.Point, 0) AS POINT
                                , Drivers.CarInfo, Drivers.Memo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                          LEFT  JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            LEFT JOIN (SELECT SUM(Amount) AS Point, DriverId, ClientId FROM DriverPoints GROUP BY Driverid, ClientId ) DriverPoints ON DriverInstances.DriverId = DriverPoints.DriverId AND DriverInstances.ClientId = DriverPoints.ClientId
                             WHERE Drivers.DriverId = @DriverId AND Drivers.ServiceState <> 5 AND DriverInstances.ClientId = @ClientId";
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        }
                    }
                    else
                    {
                        _Command.CommandText =
                            @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarGubun, Drivers.RequestFrom, Drivers.RequestTo, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg,

                                0 AS SubClientId, 0 AS ClientUserId, '0' AS GroupName, 0 AS CarGubun, '' AS RequestFrom, '' AS RequestTo
                                , 0 AS POINT
                                , Drivers.CarInfo, Drivers.Memo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                            JOIN Clients ON Drivers.CandidateId = Clients.ClientId
                            LEFT   JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            WHERE Drivers.DriverId = @DriverId";
                    }
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        DriverTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        public void SelectRemoteOne(DataTable DriverTable, int DriverId)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId, 

                                Drivers.Code, Drivers.BizNo, Drivers.Name, Drivers.CEO, Drivers.Uptae, Drivers.Upjong, Drivers.Email, Drivers.LoginId, Drivers.Password, 
                                Drivers.MobileNo, Drivers.PhoneNo, Drivers.FaxNo, Drivers.CreateDate, Drivers.CEOBirth, Drivers.VAccount,
                                Drivers.Zipcode, Drivers.AddressState, Drivers.AddressCity, Drivers.AddressDetail, Drivers.ServiceState, Drivers.RequestDate, 
                                Drivers.PayBankCode, Drivers.PayBankName, Drivers.PayAccountNo, Drivers.PayInputName, 

                                Drivers.BizType, Drivers.RouteType, Drivers.InsuranceType, Drivers.UsePayNow, 
                                Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.CarGubun, Drivers.RequestFrom, Drivers.RequestTo, Drivers.CarYear, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet, Drivers.FpisCarType, Drivers.CandidateId, 

                                Drivers.InsuranceName, Drivers.InsuranceNum, Drivers.InsuranceFrom, Drivers.InsuranceTo, Drivers.InsuranceDate, Drivers.InsuranceMoney, Drivers.InsuranceCarYear, Drivers.InsuranceNowDate, Drivers.InsuranceNextDate, 
                                Drivers.InsuranceShcard, Drivers.InsuranceKbCard, Drivers.InsuranceWrCard, Drivers.ServicePrice, Drivers.useTax, Drivers.DTGUse, Drivers.DTGPrice, Drivers.AccountUse, Drivers.AccountPrice, Drivers.FPISUse, Drivers.FPISPrice, Drivers.MyCallUSe, Drivers.MyCallPrice, Drivers.OTGUse, Drivers.OTGPrice, 

                                Drivers.HasBizPaper, Drivers.HasCarPaper, Drivers.HasCarReg, Drivers.IsAgreed,

                                0 AS SubClientId, 0 AS ClientUserId, '0' AS GroupName, 0 AS CarGubun, '' AS RequestFrom, '' AS RequestTo
                                , Drivers.CarInfo, Drivers.Memo,Drivers.DealerId,ISNULL(DriverInstances.Misu,0) AS Misu,ISNULL(DriverInstances.Mizi,0) AS Mizi ,ISNULL(Drivers.AppUse, 0) AS AppUse,Drivers.HasCarEtc
                            FROM    Drivers
                            JOIN Clients ON Drivers.CandidateId = Clients.ClientId
                            LEFT   JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            WHERE Drivers.DriverId = @DriverId";
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    using (IDataReader _Reader = _Command.ExecuteReader())
                    {
                        DriverTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        public bool IsMyCar(String CarNo)
        {
            CarNo = CarNo.Replace(" ", "").Trim();
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId 
                            FROM    Drivers
                            JOIN    DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            WHERE Drivers.CarNo = @CarNo AND DriverInstances.ClientId = @ClientId AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@CarNo", CarNo);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public bool IsMyCar(int DriverId)
        {
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId 
                            FROM    Drivers
                            JOIN    DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            WHERE Drivers.DriverId = @DriverId AND DriverInstances.ClientId = @ClientId AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }
        public bool NoIsMyCar(int DriverId)
        {
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId 
                            FROM    Drivers
                            JOIN    DriverInstances ON Drivers.DriverId = DriverInstances.DriverId
                            WHERE Drivers.DriverId = @DriverId AND DriverInstances.ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public bool IsAnotherCar(String CarNo)
        {
            CarNo = CarNo.Replace(" ", "").Trim();
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId 
                            FROM    Drivers
                            WHERE Drivers.CarNo = @CarNo AND Drivers.CandidateId <> @ClientId AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@CarNo", CarNo);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public bool IsAnotherCar(int DriverId)
        {
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId 
                            FROM    Drivers
                            WHERE Drivers.DriverId = @DriverId AND Drivers.CandidateId <> @ClientId AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }
        
        public int GetDriverId(String CarNo)
        {
            CarNo = CarNo.Replace(" ", "").Trim();
            int DriverId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId 
                            FROM    Drivers
                            WHERE Drivers.CarNo = @CarNo AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@CarNo", CarNo);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        DriverId = Convert.ToInt32(O);
                }
                _Connection.Close();
            }
            return DriverId;
        }

       
        public Driver GetDriver(String CarNo)
        {
            CarNo = CarNo.Replace(" ", "").Trim();
            Driver _Driver = null;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId ,
                                Drivers.CarNo ,
                                Drivers.CarType ,
                                Drivers.CarSize ,
                                Drivers.CarYear ,
                                Drivers.MobileNo,
                                Drivers.Name,
                                Drivers.BizNo,
                                Drivers.CEO
                            FROM    Drivers
                            WHERE Drivers.CarNo = @CarNo AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@CarNo", CarNo);
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            _Driver = new Driver
                            {
                                DriverId = _Reader.GetInt32(0),
                                CarNo = _Reader.GetString(1),
                                CarType = _Reader.GetInt32(2),
                                CarSize = _Reader.GetInt32(3),
                                CarYear = _Reader.GetString(4),
                                MobileNo = _Reader.GetString(5),
                                Name = _Reader.GetString(6),
                                BizNo = _Reader.GetString(7),
                                CEO = _Reader.GetString(8),
                            };
                        }
                    }
                }
                _Connection.Close();
            }
            return _Driver;
        }

        public Driver GetDriver(int DriverId)
        {
            Driver _Driver = null;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId ,
                                Drivers.CarNo ,
                                Drivers.CarType ,
                                Drivers.CarSize ,
                                Drivers.CarYear ,
                                Drivers.MobileNo,
                                ISNULL(Drivers.Name,''),
                                ISNULL(Drivers.BizNo,''),
                                ISNULL(Drivers.PayAccountNo,'') ,
                                ISNULL(Drivers.PayBankName,''),
                                ISNULL(Drivers.PayBankCode,''),
                                ISNULL(Drivers.PayInputName,''),
                                ISNULL(Drivers.CEO,''),
                                Drivers.ServiceState,
                                ISNULL(Drivers.VAccount,''),
                                ISNULL(Drivers.VBankName,''),
                                ISNULL(Drivers.UpTae,''),
                                ISNULL(Drivers.Upjong,''),
                                ISNULL(Drivers.AddressState,''),
                                ISNULL(Drivers.AddressCity,''),
                                ISNULL(Drivers.AddressDetail,'')
                            FROM    Drivers
                            WHERE Drivers.DriverId = @DriverId AND Drivers.ServiceState <> 5";
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            _Driver = new Driver
                            {
                                DriverId = _Reader.GetInt32(0),
                                CarNo = _Reader.GetString(1),
                                CarType = _Reader.GetInt32(2),
                                CarSize = _Reader.GetInt32(3),
                                CarYear = _Reader.GetString(4),
                                MobileNo = _Reader.GetString(5),
                                Name = _Reader.GetString(6),
                                BizNo = _Reader.GetString(7),
                                PayAccountNo = _Reader.GetString(8),
                                PayBankName = _Reader.GetString(9),
                                PayBankCode = _Reader.GetString(10),
                                PayInputName = _Reader.GetString(11),
                                CEO = _Reader.GetString(12),
                                ServiceState = _Reader.GetInt32(13),
                                VAccount = _Reader.GetString(14),
                                VBankName = _Reader.GetString(15),

                                Uptae = _Reader.GetString(16),
                                Upjong = _Reader.GetString(17),
                                Addresstate = _Reader.GetString(18),
                                AddressCity = _Reader.GetString(19),
                                AddressDetail = _Reader.GetString(20),
                            };
                        }
                    }
                }
                _Connection.Close();
            }
            return _Driver;
        }

        public Driver NoGetDriver(int DriverId)
        {
            Driver _Driver = null;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                Drivers.DriverId ,
                                Drivers.CarNo ,
                                Drivers.CarType ,
                                Drivers.CarSize ,
                                Drivers.CarYear ,
                                Drivers.MobileNo,
                                ISNULL(Drivers.Name,''),
                                ISNULL(Drivers.BizNo,''),
                                ISNULL(Drivers.PayAccountNo,'') ,
                                ISNULL(Drivers.PayBankName,''),
                                ISNULL(Drivers.PayBankCode,''),
                                ISNULL(Drivers.PayInputName,''),
                                ISNULL(Drivers.CEO,''),
                                Drivers.ServiceState,
                                ISNULL(Drivers.VAccount,''),
                                ISNULL(Drivers.VBankName,''),
                                ISNULL(Drivers.UpTae,''),
                                ISNULL(Drivers.Upjong,''),
                                ISNULL(Drivers.AddressState,''),
                                ISNULL(Drivers.AddressCity,''),
                                ISNULL(Drivers.AddressDetail,'')
                            FROM    Drivers
                            WHERE Drivers.DriverId = @DriverId ";
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            _Driver = new Driver
                            {
                                DriverId = _Reader.GetInt32(0),
                                CarNo = _Reader.GetString(1),
                                CarType = _Reader.GetInt32(2),
                                CarSize = _Reader.GetInt32(3),
                                CarYear = _Reader.GetString(4),
                                MobileNo = _Reader.GetString(5),
                                Name = _Reader.GetString(6),
                                BizNo = _Reader.GetString(7),
                                PayAccountNo = _Reader.GetString(8),
                                PayBankName = _Reader.GetString(9),
                                PayBankCode = _Reader.GetString(10),
                                PayInputName = _Reader.GetString(11),
                                CEO = _Reader.GetString(12),
                                ServiceState = _Reader.GetInt32(13),
                                VAccount = _Reader.GetString(14),
                                VBankName = _Reader.GetString(15),

                                Uptae = _Reader.GetString(16),
                                Upjong = _Reader.GetString(17),
                                Addresstate = _Reader.GetString(18),
                                AddressCity = _Reader.GetString(19),
                                AddressDetail = _Reader.GetString(20),
                            };
                        }
                    }
                }
                _Connection.Close();
            }
            return _Driver;
        }

        public int CreateDriver(String BizNo, String Name, String CEO, String Uptae, String Upjong, String CEOBirth,
            String Zipcode, String AddressState, String AddressCity, String AddressDetail, String MobileNo,
            String PayBankCode, String PayBankName, String PayAccountNo, String PayInputName, String CarNo,
            int CarType, int CarSize, String CarInfo, int DealerId,int Misu,int Mizi)
        {
            String Code = "100001";
            String LoginId = "";
            String Password = MobileNo.Substring(MobileNo.Length - 4);
            CarNo = CarNo.Replace(" ", "").Trim();
            int DriverId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand CodeCommand = _Connection.CreateCommand())
                {
                    CodeCommand.CommandText = "SELECT MAX(Code) FROM Drivers";
                    Object O = CodeCommand.ExecuteScalar();
                    if (O != null)
                    {
                        Code = (int.Parse(O.ToString()) + 1).ToString();
                    }
                }
                // 같은 사업자번호로 999명이 넘어갈경우, 다른 사업자번호 사용
                int tBizNo = 0;
                if (BizNo.Length > 5)
                    //                    tBizNo = int.Parse(BizNo.Substring(BizNo.Length - 5));
                    tBizNo = int.Parse(BizNo.Replace("-","").Substring(BizNo.Replace("-", "").Length - 5));
                else
                    tBizNo = int.Parse(MobileNo.Substring(MobileNo.Length - 4));
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT LoginId FROM Drivers WHERE RIGHT(BizNo, 5) = @BizNo";
                    _Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                    while (true)
                    {
                        _Command.Parameters["@BizNo"].Value = tBizNo.ToString("00000");
                        List<String> _LoginIdCompareList = new List<string>();
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            while (_Reader.Read())
                            {
                                _LoginIdCompareList.Add(_Reader.GetString(0));
                            }
                        }
                        for (int tLoginId = 1; tLoginId < 1000; tLoginId++)
                        {
                            if (!_LoginIdCompareList.Contains($"m{tBizNo:00000}{tLoginId:000}"))
                            {
                                LoginId = $"m{tBizNo:00000}{tLoginId:000}";
                                break;
                            }
                        }
                        if (!String.IsNullOrEmpty(LoginId))
                            break;
                        tBizNo++;
                    }
                }
                using (SqlCommand DriverInsertCommand = _Connection.CreateCommand())
                {
                    DriverInsertCommand.CommandText =
                        @"INSERT INTO Drivers
                            (Code, LoginId, Password, BizNo, Name, CEO, Uptae, Upjong, CEOBirth, Zipcode, AddressState, AddressCity, AddressDetail, MobileNo,
                            PayBankCode, PayBankName, PayAccountNo, PayInputName, CarNo, CarType, CarSize, CarInfo,
                            CreateDate, CandidateId, CarYear,DealerId,Misu,Mizi,Mstart) OUTPUT INSERTED.DriverId
                            Values
                            (@Code, @LoginId, @Password, @BizNo, @Name, @CEO, @Uptae, @Upjong, @CEOBirth, @Zipcode, @AddressState, @AddressCity, @AddressDetail, @MobileNo,
                            @PayBankCode, @PayBankName, @PayAccountNo, @PayInputName, @CarNo, @CarType, @CarSize, @CarInfo,
                            GETDATE(), @ClientId, @CEO,@DealerId,@Misu,@Mizi,@Mstart)";
                    DriverInsertCommand.Parameters.AddWithValue("@Code", Code);
                    DriverInsertCommand.Parameters.AddWithValue("@LoginId", LoginId);
                    DriverInsertCommand.Parameters.AddWithValue("@Password", Password);
                    DriverInsertCommand.Parameters.AddWithValue("@BizNo", BizNo);
                    DriverInsertCommand.Parameters.AddWithValue("@Name", Name);
                    DriverInsertCommand.Parameters.AddWithValue("@CEO", CEO);
                    DriverInsertCommand.Parameters.AddWithValue("@Uptae", Uptae);
                    DriverInsertCommand.Parameters.AddWithValue("@Upjong", Upjong);
                    DriverInsertCommand.Parameters.AddWithValue("@CEOBirth", CEOBirth);
                    DriverInsertCommand.Parameters.AddWithValue("@Zipcode", Zipcode);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressState", AddressState);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressCity", AddressCity);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressDetail", AddressDetail);
                    DriverInsertCommand.Parameters.AddWithValue("@MobileNo", MobileNo);
                    DriverInsertCommand.Parameters.AddWithValue("@PayBankCode", PayBankCode);
                    DriverInsertCommand.Parameters.AddWithValue("@PayBankName", PayBankName);
                    DriverInsertCommand.Parameters.AddWithValue("@PayAccountNo", PayAccountNo);
                    DriverInsertCommand.Parameters.AddWithValue("@PayInputName", PayInputName);
                    DriverInsertCommand.Parameters.AddWithValue("@CarNo", CarNo);
                    DriverInsertCommand.Parameters.AddWithValue("@CarType", CarType);
                    DriverInsertCommand.Parameters.AddWithValue("@CarSize", CarSize);
                    DriverInsertCommand.Parameters.AddWithValue("@CarInfo", CarInfo);
                    DriverInsertCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    DriverInsertCommand.Parameters.AddWithValue("@DealerId", DealerId);
                    DriverInsertCommand.Parameters.AddWithValue("@Misu", Misu);
                    DriverInsertCommand.Parameters.AddWithValue("@Mizi", Mizi);
                    DriverInsertCommand.Parameters.AddWithValue("@Mstart", Mizi);
                    Object O = DriverInsertCommand.ExecuteScalar();
                    if (O != null)
                        DriverId = Convert.ToInt32(O);
                }
                if (DriverId > 0)
                {
                    using (SqlCommand DriverInstanceCommand = _Connection.CreateCommand())
                    {
                        DriverInstanceCommand.CommandText =
                            @"INSERT INTO DriverInstances (DriverId, ClientId,Misu,Mizi,MStartDate,Mstart) VALUES (@DriverId, @ClientId,@Misu,@Mizi,getdate(),@Mizi)";
                        DriverInstanceCommand.Parameters.AddWithValue("@DriverId", DriverId);
                        DriverInstanceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        DriverInstanceCommand.Parameters.AddWithValue("@Misu", Misu);
                        DriverInstanceCommand.Parameters.AddWithValue("@Mizi", Mizi);
                        DriverInstanceCommand.ExecuteNonQuery();
                    }
                    //using (SqlCommand _VAccountSelectCommand = _Connection.CreateCommand())
                    //using (SqlCommand _VAccountUpdateCommand = _Connection.CreateCommand())
                    //{
                    //    _VAccountSelectCommand.CommandText = "SELECT VAccountPoolId, VAccount FROM VAccountPools WHERE ClientId IS NULL AND DriverId IS NULL";
                    //    using (SqlDataReader _Reader = _VAccountSelectCommand.ExecuteReader(CommandBehavior.SingleRow))
                    //    {
                    //        if (_Reader.Read())
                    //        {
                    //            var VAccountPoolId = _Reader.GetInt32(0);
                    //            var VAccount = _Reader.GetString(1);
                    //            _Reader.Close();
                    //            _VAccountUpdateCommand.CommandText = String.Format("UPDATE VAccountPools SET DriverId = {0} WHERE VAccountPoolId = {1}", DriverId, VAccountPoolId);
                    //            _VAccountUpdateCommand.ExecuteNonQuery();
                    //            _VAccountUpdateCommand.CommandText = String.Format("UPDATE Drivers SET VAccount = {0} WHERE DriverId = {1}", VAccount, DriverId);
                    //            _VAccountUpdateCommand.ExecuteNonQuery();
                    //        }
                    //    }

                    //}
                }
                _Connection.Close();
            }
            return DriverId;
        }

        public int CreateDriver_SG(String BizNo, String Name, String CEO, String Uptae, String Upjong, String CEOBirth,
    String Zipcode, String AddressState, String AddressCity, String AddressDetail, String MobileNo, String PhoneNo, String CarNo, String FaxNo, String Email)
        {
            CarNo = CarNo.Replace("-", "").Replace(" ", "").Trim(); 
            String Code = "100001";
            String LoginId = "";
            String Password = CarNo.Substring(CarNo.Length - 5);
            int DriverId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand CodeCommand = _Connection.CreateCommand())
                {
                    CodeCommand.CommandText = "SELECT MAX(Code) FROM Drivers";
                    Object O = CodeCommand.ExecuteScalar();
                    if (O != null)
                    {
                        Code = (int.Parse(O.ToString()) + 1).ToString();
                    }
                }
                int tBizNo = 0;
                if (BizNo.Length > 5)
                    tBizNo = int.Parse(BizNo.Substring(BizNo.Length - 5));
                else
                    tBizNo = int.Parse(MobileNo.Substring(MobileNo.Length - 4));
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT LoginId FROM Drivers WHERE RIGHT(BizNo, 5) = @BizNo";
                    _Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                    while (true)
                    {
                        _Command.Parameters["@BizNo"].Value = tBizNo.ToString("00000");
                        List<String> _LoginIdCompareList = new List<string>();
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            while (_Reader.Read())
                            {
                                _LoginIdCompareList.Add(_Reader.GetString(0));
                            }
                        }
                        for (int tLoginId = 1; tLoginId < 1000; tLoginId++)
                        {
                            if (!_LoginIdCompareList.Contains($"m{tBizNo:00000}{tLoginId:000}"))
                            {
                                LoginId = $"m{tBizNo:00000}{tLoginId:000}";
                                break;
                            }
                        }
                        if (!String.IsNullOrEmpty(LoginId))
                            break;
                        tBizNo++;
                    }
                }
                using (SqlCommand DriverInsertCommand = _Connection.CreateCommand())
                {
                    DriverInsertCommand.CommandText =
                        @"INSERT INTO Drivers
                            (Code, LoginId, Password, BizNo, Name, CEO, Uptae, Upjong, CEOBirth, Zipcode, AddressState, AddressCity, AddressDetail, MobileNo, PhoneNo, FaxNo, Email,
                            PayBankCode, PayBankName, PayAccountNo, PayInputName, CarNo, CarType, CarSize, CarInfo, CarYear,
                            CreateDate, CandidateId, ServiceType) OUTPUT INSERTED.DriverId
                            Values
                            (@Code, @LoginId, @Password, @BizNo, @Name, @CEO, @Uptae, @Upjong, @CEOBirth, @Zipcode, @AddressState, @AddressCity, @AddressDetail, @MobileNo, @PhoneNo, @FaxNo, @Email,
                            '', '', '', '', @CarNo, 0, 0, '', '',
                            GETDATE(), @ClientId, 2)";
                    DriverInsertCommand.Parameters.AddWithValue("@Code", Code);
                    DriverInsertCommand.Parameters.AddWithValue("@LoginId", LoginId);
                    DriverInsertCommand.Parameters.AddWithValue("@Password", Password);
                    DriverInsertCommand.Parameters.AddWithValue("@BizNo", BizNo);
                    DriverInsertCommand.Parameters.AddWithValue("@Name", Name);
                    DriverInsertCommand.Parameters.AddWithValue("@CEO", CEO);
                    DriverInsertCommand.Parameters.AddWithValue("@Uptae", Uptae);
                    DriverInsertCommand.Parameters.AddWithValue("@Upjong", Upjong);
                    DriverInsertCommand.Parameters.AddWithValue("@CEOBirth", CEOBirth);
                    DriverInsertCommand.Parameters.AddWithValue("@Zipcode", Zipcode);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressState", AddressState);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressCity", AddressCity);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressDetail", AddressDetail);
                    DriverInsertCommand.Parameters.AddWithValue("@MobileNo", MobileNo);
                    DriverInsertCommand.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                    DriverInsertCommand.Parameters.AddWithValue("@FaxNo", FaxNo);
                    DriverInsertCommand.Parameters.AddWithValue("@Email", Email);
                    DriverInsertCommand.Parameters.AddWithValue("@CarNo", CarNo);
                    DriverInsertCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    Object O = DriverInsertCommand.ExecuteScalar();
                    if (O != null)
                        DriverId = Convert.ToInt32(O);
                }
                if (DriverId > 0)
                {
                    using (SqlCommand DriverInstanceCommand = _Connection.CreateCommand())
                    {
                        DriverInstanceCommand.CommandText =
                            @"INSERT INTO DriverInstances (DriverId, ClientId) VALUES (@DriverId, @ClientId)";
                        DriverInstanceCommand.Parameters.AddWithValue("@DriverId", DriverId);
                        DriverInstanceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        DriverInstanceCommand.ExecuteNonQuery();
                    }
                }
                _Connection.Close();
            }
            return DriverId;
        }

        public Driver CreateDriver_FPIS(String CarNo)
        {
            String Code = "";
            int DriverId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand DriverInsertCommand = _Connection.CreateCommand())
                {
                    DriverInsertCommand.CommandText =
                        @"INSERT INTO Drivers
                            (Code, LoginId, Password, BizNo, Name, CEO, Uptae, Upjong, CEOBirth, Zipcode, AddressState, AddressCity, AddressDetail, MobileNo, PhoneNo, FaxNo, Email,
                            PayBankCode, PayBankName, PayAccountNo, PayInputName, CarNo, CarType, CarSize, CarInfo, CarYear,
                            CreateDate, CandidateId, ServiceType) OUTPUT INSERTED.DriverId
                            Values
                            (@Code, @LoginId, @Password, @BizNo, @Name, @CEO, @Uptae, @Upjong, @CEOBirth, @Zipcode, @AddressState, @AddressCity, @AddressDetail, @MobileNo, @PhoneNo, @FaxNo, @Email,
                            '', '', '', '', @CarNo, 0, 0, '', '',
                            GETDATE(), @ClientId, 2)";
                    DriverInsertCommand.Parameters.AddWithValue("@Code", Code);
                    DriverInsertCommand.Parameters.AddWithValue("@LoginId", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Password", "");
                    DriverInsertCommand.Parameters.AddWithValue("@BizNo", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Name", "");
                    DriverInsertCommand.Parameters.AddWithValue("@CEO", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Uptae", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Upjong", "");
                    DriverInsertCommand.Parameters.AddWithValue("@CEOBirth", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Zipcode", "");
                    DriverInsertCommand.Parameters.AddWithValue("@AddressState", "");
                    DriverInsertCommand.Parameters.AddWithValue("@AddressCity", "");
                    DriverInsertCommand.Parameters.AddWithValue("@AddressDetail", "");
                    DriverInsertCommand.Parameters.AddWithValue("@MobileNo", "");
                    DriverInsertCommand.Parameters.AddWithValue("@PhoneNo", "");
                    DriverInsertCommand.Parameters.AddWithValue("@FaxNo", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Email", "");
                    DriverInsertCommand.Parameters.AddWithValue("@CarNo", CarNo);
                    DriverInsertCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    Object O = DriverInsertCommand.ExecuteScalar();
                    if (O != null)
                        DriverId = Convert.ToInt32(O);
                }
                if (DriverId > 0)
                {
                    using (SqlCommand DriverInstanceCommand = _Connection.CreateCommand())
                    {
                        DriverInstanceCommand.CommandText =
                            @"INSERT INTO DriverInstances (DriverId, ClientId) VALUES (@DriverId, @ClientId)";
                        DriverInstanceCommand.Parameters.AddWithValue("@DriverId", DriverId);
                        DriverInstanceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        DriverInstanceCommand.ExecuteNonQuery();
                    }
                }
                _Connection.Close();
            }
            return new Driver
            {
                DriverId = DriverId,
                CarType = 0,
                CarSize = 0,
                CarYear = "",
                MobileNo = "",
                Name = "",
            };
        }

        public int CreateSmartDriver(String CarYear, String MobileNo, String CarNo,int CarSize, int CarType, out String LoginId, out String Password,int DealerId, string BizNo = "")
        {
            String Code = "100001";
            LoginId = "";
            Password = MobileNo.Substring(MobileNo.Length - 4);
            CarNo = CarNo.Replace(" ", "").Trim();
            int DriverId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand CodeCommand = _Connection.CreateCommand())
                {
                    CodeCommand.CommandText = "SELECT MAX(Code) FROM Drivers";
                    Object O = CodeCommand.ExecuteScalar();
                    if (O != null)
                    {
                        Code = (int.Parse(O.ToString()) + 1).ToString();
                    }
                }
                // 같은 사업자번호로 999명이 넘어갈경우, 다른 사업자번호 사용
                int tBizNo = int.Parse(CarNo.Substring(CarNo.Length - 4));
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT LoginId FROM Drivers";
                    _Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                    while (true)
                    {
                        _Command.Parameters["@BizNo"].Value = tBizNo.ToString("00000");
                        List<String> _LoginIdCompareList = new List<string>();
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            while (_Reader.Read())
                            {
                                _LoginIdCompareList.Add(_Reader.GetString(0));
                            }
                        }
                        for (int tLoginId = 1; tLoginId < 1000; tLoginId++)
                        {
                            if (!_LoginIdCompareList.Contains($"m{tBizNo:00000}{tLoginId:000}"))
                            {
                                LoginId = $"m{tBizNo:00000}{tLoginId:000}";
                                break;
                            }
                        }
                        if (!String.IsNullOrEmpty(LoginId))
                            break;
                        tBizNo++;
                    }
                }
                using (SqlCommand DriverInsertCommand = _Connection.CreateCommand())
                {
                    

                    DriverInsertCommand.CommandText =
                        @"INSERT INTO Drivers
                            (Code, LoginId, Password, BizNo, Name, CEO, Uptae, Upjong, CEOBirth, Zipcode, AddressState, AddressCity, AddressDetail, MobileNo,
                            PayBankCode, PayBankName, PayAccountNo, PayInputName, CarNo, CarType, CarSize, CarInfo,
                            CreateDate, CandidateId, CarYear,DealerId) OUTPUT INSERTED.DriverId
                            Values
                            (@Code, @LoginId, @Password, @BizNo, @Name, @CEO, @Uptae, @Upjong, @CEOBirth, @Zipcode, @AddressState, @AddressCity, @AddressDetail, @MobileNo,
                            @PayBankCode, @PayBankName, @PayAccountNo, @PayInputName, @CarNo, @CarType, @CarSize, @CarInfo,
                            GETDATE(), @ClientId, @CarYear,@DealerId)";
                    DriverInsertCommand.Parameters.AddWithValue("@Code", Code);
                    DriverInsertCommand.Parameters.AddWithValue("@LoginId", LoginId);
                    DriverInsertCommand.Parameters.AddWithValue("@Password", Password);
                    DriverInsertCommand.Parameters.AddWithValue("@BizNo", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Name", "");
                    DriverInsertCommand.Parameters.AddWithValue("@CEO", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Uptae", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Upjong", "");
                    DriverInsertCommand.Parameters.AddWithValue("@CEOBirth", "");
                    DriverInsertCommand.Parameters.AddWithValue("@Zipcode", "");
                    DriverInsertCommand.Parameters.AddWithValue("@AddressState", "");
                    DriverInsertCommand.Parameters.AddWithValue("@AddressCity", "");
                    DriverInsertCommand.Parameters.AddWithValue("@AddressDetail", "");
                    DriverInsertCommand.Parameters.AddWithValue("@MobileNo", MobileNo);
                    DriverInsertCommand.Parameters.AddWithValue("@PayBankCode", "");
                    DriverInsertCommand.Parameters.AddWithValue("@PayBankName", "");
                    DriverInsertCommand.Parameters.AddWithValue("@PayAccountNo", "");
                    DriverInsertCommand.Parameters.AddWithValue("@PayInputName", "");
                    DriverInsertCommand.Parameters.AddWithValue("@CarNo", CarNo);
                    DriverInsertCommand.Parameters.AddWithValue("@CarType", CarType);
                    DriverInsertCommand.Parameters.AddWithValue("@CarSize", CarSize);
                    DriverInsertCommand.Parameters.AddWithValue("@CarInfo", "");
                    DriverInsertCommand.Parameters.AddWithValue("@CarYear", CarYear);

                    DriverInsertCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    DriverInsertCommand.Parameters.AddWithValue("@DealerId", DealerId);
                    Object O = DriverInsertCommand.ExecuteScalar();
                    if (O != null)
                        DriverId = Convert.ToInt32(O);
                }
                if (DriverId > 0)
                {
                    using (SqlCommand DriverInstanceCommand = _Connection.CreateCommand())
                    {
                        DriverInstanceCommand.CommandText =
                            @"INSERT INTO DriverInstances (DriverId, ClientId) VALUES (@DriverId, @ClientId)";
                        DriverInstanceCommand.Parameters.AddWithValue("@DriverId", DriverId);
                        DriverInstanceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        DriverInstanceCommand.ExecuteNonQuery();
                    }
                    //using (SqlCommand _VAccountSelectCommand = _Connection.CreateCommand())
                    //using (SqlCommand _VAccountUpdateCommand = _Connection.CreateCommand())
                    //{
                    //    _VAccountSelectCommand.CommandText = "SELECT VAccountPoolId, VAccount FROM VAccountPools WHERE ClientId IS NULL AND DriverId IS NULL";
                    //    using (SqlDataReader _Reader = _VAccountSelectCommand.ExecuteReader(CommandBehavior.SingleRow))
                    //    {
                    //        if (_Reader.Read())
                    //        {
                    //            var VAccountPoolId = _Reader.GetInt32(0);
                    //            var VAccount = _Reader.GetString(1);
                    //            _Reader.Close();
                    //            _VAccountUpdateCommand.CommandText = String.Format("UPDATE VAccountPools SET DriverId = {0} WHERE VAccountPoolId = {1}", DriverId, VAccountPoolId);
                    //            _VAccountUpdateCommand.ExecuteNonQuery();
                    //            _VAccountUpdateCommand.CommandText = String.Format("UPDATE Drivers SET VAccount = {0} WHERE DriverId = {1}", VAccount, DriverId);
                    //            _VAccountUpdateCommand.ExecuteNonQuery();
                    //        }
                    //    }

                    //}
                }
                _Connection.Close();
            }
            return DriverId;
        }

        public bool HasSelf()
        {
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                DriverId 
                            FROM    Drivers
                            WHERE Drivers.ServiceType = 3 AND CandidateId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public int GetDriverId_Self()
        {
            int R = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                DriverId 
                            FROM    Drivers
                            WHERE Drivers.ServiceType = 3 AND CandidateId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = Convert.ToInt32(O);
                }
                _Connection.Close();
            }
            return R;
        }

        public bool HasBizPaper_Self()
        {
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                HasBizPaper 
                            FROM    Drivers
                            WHERE Drivers.ServiceType = 3 AND CandidateId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public bool HasCarPaper_Self()
        {
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT    
                                HasCarPaper 
                            FROM    Drivers
                            WHERE Drivers.ServiceType = 3 AND CandidateId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public int CreateDriver_Self()
        {
            String Code = "100001";
            String LoginId = "edv" + LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", "");
            String Password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            
            int DriverId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand CodeCommand = _Connection.CreateCommand())
                {
                    CodeCommand.CommandText = "SELECT MAX(Code) FROM Drivers";
                    Object O = CodeCommand.ExecuteScalar();
                    if (O != null)
                    {
                        Code = (int.Parse(O.ToString()) + 1).ToString();
                    }
                }
                using (SqlCommand DriverInsertCommand = _Connection.CreateCommand())
                {
                    DriverInsertCommand.CommandText =
                        @"INSERT INTO Drivers
                            (Code, LoginId, Password, BizNo, Name, CEO, Uptae, Upjong, CEOBirth, Zipcode, AddressState, AddressCity, AddressDetail, MobileNo, PhoneNo, FaxNo, 
                            PayBankCode, PayBankName, PayAccountNo, PayInputName, CarNo, CarType, CarSize, CarInfo, CarYear,
                            CreateDate, CandidateId, ServiceType) OUTPUT INSERTED.DriverId
                        Values
                            (@Code, @LoginId, @Password, @BizNo, @Name, @CEO, @Uptae, @Upjong, @CEOBirth, @Zipcode, @AddressState, @AddressCity, @AddressDetail, @MobileNo, @PhoneNo, @FaxNo, 
                            @PayBankCode, @PayBankName, @PayAccountNo, @PayInputName, @CarNo, 0, 0, '', '',
                            GETDATE(), @ClientId, 3)";
                    DriverInsertCommand.Parameters.AddWithValue("@Code", Code);
                    DriverInsertCommand.Parameters.AddWithValue("@LoginId", LoginId);
                    DriverInsertCommand.Parameters.AddWithValue("@Password", Password);
                    DriverInsertCommand.Parameters.AddWithValue("@BizNo", LocalUser.Instance.LogInInformation.Client.BizNo);
                    DriverInsertCommand.Parameters.AddWithValue("@Name", LocalUser.Instance.LogInInformation.Client.Name);
                    DriverInsertCommand.Parameters.AddWithValue("@CEO", LocalUser.Instance.LogInInformation.Client.CEO);
                    DriverInsertCommand.Parameters.AddWithValue("@Uptae", LocalUser.Instance.LogInInformation.Client.Uptae);
                    DriverInsertCommand.Parameters.AddWithValue("@Upjong", LocalUser.Instance.LogInInformation.Client.Upjong);
                    DriverInsertCommand.Parameters.AddWithValue("@CEOBirth", "000000");
                    DriverInsertCommand.Parameters.AddWithValue("@Zipcode", LocalUser.Instance.LogInInformation.Client.ZipCode);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressState", LocalUser.Instance.LogInInformation.Client.AddressState);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressCity", LocalUser.Instance.LogInInformation.Client.AddressCity);
                    DriverInsertCommand.Parameters.AddWithValue("@AddressDetail", LocalUser.Instance.LogInInformation.Client.AddressDetail);
                    DriverInsertCommand.Parameters.AddWithValue("@MobileNo", LocalUser.Instance.LogInInformation.Client.MobileNo);
                    DriverInsertCommand.Parameters.AddWithValue("@PhoneNo", LocalUser.Instance.LogInInformation.Client.PhoneNo);
                    DriverInsertCommand.Parameters.AddWithValue("@FaxNo", LocalUser.Instance.LogInInformation.Client.FaxNo);
                    DriverInsertCommand.Parameters.AddWithValue("@PayBankCode", LocalUser.Instance.LogInInformation.Client.CMSBankCode);
                    DriverInsertCommand.Parameters.AddWithValue("@PayBankName", LocalUser.Instance.LogInInformation.Client.CMSBankName);
                    DriverInsertCommand.Parameters.AddWithValue("@PayAccountNo", LocalUser.Instance.LogInInformation.Client.CMSAccountNo);
                    DriverInsertCommand.Parameters.AddWithValue("@PayInputName", LocalUser.Instance.LogInInformation.Client.CMSOwner);
                    DriverInsertCommand.Parameters.AddWithValue("@CarNo", LoginId);
                    DriverInsertCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    
                    Object O = DriverInsertCommand.ExecuteScalar();
                    if (O != null)
                        DriverId = Convert.ToInt32(O);
                }
                _Connection.Close();
            }
            return DriverId;
        }

        public void ConnectDriver(int DriverId)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand DriverInstanceCommand = _Connection.CreateCommand())
                {
                    DriverInstanceCommand.CommandText =
                        @"INSERT INTO DriverInstances (DriverId, ClientId, CarGubun, RequestFrom, RequestTo) VALUES (@DriverId, @ClientId, 3, CONVERT(NVARCHAR(10),GETDATE(),126), CONVERT(NVARCHAR(10),GETDATE(),126))";
                    DriverInstanceCommand.Parameters.AddWithValue("@DriverId", DriverId);
                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    DriverInstanceCommand.ExecuteNonQuery();
                }
                _Connection.Close();
            }
        }

        public int ConnectDriver(String CarNo)
        {
            CarNo = CarNo.Replace(" ", "").Trim();
            int DriverId = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand CarNoCommand = _Connection.CreateCommand())
                using (SqlCommand DeleteDriverInstance = _Connection.CreateCommand())
                using (SqlCommand DriverInstanceCommand = _Connection.CreateCommand())
                {
                    CarNoCommand.CommandText =
                        @"SELECT DriverId FROM Drivers WHERE CarNo = @CarNo AND ServiceState <> 5";
                    CarNoCommand.Parameters.AddWithValue("@CarNo", CarNo);
                    DriverId = Convert.ToInt32(CarNoCommand.ExecuteScalar());


                    DeleteDriverInstance.CommandText =
                      @"DELETE DriverInstances WHERE DriverId = @DriverId AND ClientId = @ClientId";
                    DeleteDriverInstance.Parameters.AddWithValue("@DriverId", DriverId);
                    DeleteDriverInstance.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    DeleteDriverInstance.ExecuteNonQuery();

                    DriverInstanceCommand.CommandText =
                        @"INSERT INTO DriverInstances (DriverId, ClientId, CarGubun, RequestFrom, RequestTo) VALUES (@DriverId, @ClientId, 3, CONVERT(NVARCHAR(10),GETDATE(),126), CONVERT(NVARCHAR(10),GETDATE(),126))";
                    DriverInstanceCommand.Parameters.AddWithValue("@DriverId", DriverId);
                    DriverInstanceCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    DriverInstanceCommand.ExecuteNonQuery();
                }
                _Connection.Close();
            }
            return DriverId;
        }

        public void UpdateDriver(DataRow Row, int DriverId)
        {
            var _IsAnotherCar = IsAnotherCar(DriverId);
            List<String> DriverProperties = new List<string>(new string[]{
                "BizNo",
                "Name",
                "CEO",
                "Uptae",
                "Upjong",
                "Email",
                "Password",
                "MobileNo",
                "PhoneNo",
                "FaxNo",
                "ZipCode",
                "AddressState",
                "AddressCity",
                "AddressStreet",
                "AddressDetail",
                "ServiceState",
                "PayBankName",
                "PayBankCode",
                "PayAccountNo",
                "PayInputName",
                "BizType",
                "RouteType",
                "InsuranceType",
                "UsePayNow",
                "CarNo",
                "CarType",
                "CarSize",
                "CarYear",
                "ParkState",
                "ParkCity",
                "ParkStreet",
                "FPIS_CarType",
                "InsuranceName",
                "InsuranceNum",
                "InsuranceFrom",
                "InsuranceTo",
                "InsuranceDate",
                "InsuranceMoney",
                "InsuranceCarYear",
                "InsuranceNowDate",
                "InsuranceNextDate",
                "InsuranceShcard",
                "InsuranceKbCard",
                "InsuranceWrCard",
                "ServicePrice",
                "useTax",
                "DTGUse",
                "DTGPrice",
                "AccountUse",
                "AccountPrice",
                "FPISUse",
                "FPISPrice",
                "MyCallUSe",
                "MycallPrice",
                "OTGUSe",
                "OTGPrice",
                "CarInfo",
                "CEOBirth",
                "Memo",
                "DealerId",
                "Vaccount",
                "Misu",
                "Mizi",
            });
            List<String> DriverInstanceProperties = new List<string>(new String[] {
                "GroupName",
                "CarGubun",
                "RequestFrom",
                "RequestTo",
            });
            List<String> DriverUpdateList = new List<string>();
            List<String> DriverInstanceUpdateList = new List<string>();
            //if (LocalUser.Instance.LogInInformation.IsAdmin || !_IsAnotherCar)
            {
                foreach (var Property in DriverProperties)
                {
                    if (Row.Table.Columns.Contains(Property))
                    {
                        if (!Object.Equals(Row[Property], Row[Property, DataRowVersion.Original]))
                        {
                            DriverUpdateList.Add(String.Format("{0} = '{1}'", Property, Row[Property].ToString()));
                        }
                    }
                }
            }
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                foreach (var Property in DriverInstanceProperties)
                {
                    if (Row.Table.Columns.Contains(Property))
                    {
                        if (!Object.Equals(Row[Property], Row[Property, DataRowVersion.Original]))
                        {
                            DriverInstanceUpdateList.Add(String.Format("{0} = '{1}'", Property, Row[Property].ToString()));
                        }
                    }
                }
            }
            if (DriverUpdateList.Any() || DriverInstanceUpdateList.Any())
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    if (DriverUpdateList.Any())
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                                @"UPDATE Drivers" + Environment.NewLine +
                                "SET " + String.Join(", ", DriverUpdateList) + Environment.NewLine +
                                "WHERE DriverId = @DriverId";
                            _Command.Parameters.AddWithValue("@DriverId", DriverId);
                            _Command.ExecuteNonQuery();
                        }
                    }
                    if (DriverInstanceUpdateList.Any())
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
                                @"UPDATE DriverInstances" + Environment.NewLine +
                                "SET " + String.Join(", ", DriverInstanceUpdateList) + Environment.NewLine +
                                "WHERE DriverId = @DriverId AND ClientId = @ClientId";
                            _Command.Parameters.AddWithValue("@DriverId", DriverId);
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            _Command.ExecuteNonQuery();
                        }
                    }
                    _Connection.Close();
                }
            }
        }

        public void UpdateDriverSetBizPaper(int DriverId)
        {
            var _IsAnotherCar = IsAnotherCar(DriverId);
            //if (!_IsAnotherCar)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                            @"UPDATE Drivers" + Environment.NewLine +
                            "SET HasBizPaper = 1" + Environment.NewLine +
                            "WHERE DriverId = @DriverId";
                        _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        _Command.ExecuteNonQuery();
                    }
                    _Connection.Close();
                }
            }
        }

        public void UpdateDriverSetCarPaper(int DriverId)
        {
            var _IsAnotherCar = IsAnotherCar(DriverId);
            //if (!_IsAnotherCar)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                            @"UPDATE Drivers" + Environment.NewLine +
                            "SET HasCarPaper = 1" + Environment.NewLine +
                            "WHERE DriverId = @DriverId";
                        _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        _Command.ExecuteNonQuery();
                    }
                    _Connection.Close();
                }
            }
        }


        public bool AllowDelete(int DriverId)
        {
            bool R = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT Drivers.DriverId
                        FROM Drivers
                        LEFT JOIN (SELECT DriverId FROM Orders GROUP BY DriverId) as _Orders ON Drivers.DriverId = _Orders.DriverId
                        LEFT JOIN (SELECT DriverId FROM Trades GROUP BY DriverId) as _Trades ON Drivers.DriverId = _Trades.DriverId
                        WHERE 
                        --ServiceState <> 1 AND ServiceState <> 6  AND ServiceState <> 7 AND _Orders.DriverId IS NULL AND _Trades.DriverId IS NULL
                        ServiceState <> 5  AND( _Orders.DriverId IS NOT NULL OR  _Trades.DriverId IS NOT  NULL)
                        AND Drivers.DriverId = @DriverId";
                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        R = true;
                }
                _Connection.Close();
            }
            return R;
        }

        public void DeleteDriver(int DriverId)
        {
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                var _AllowDeleteA = AllowDelete(DriverId);

                Data.Connection(_Connection =>
                {
                   

                    if (_AllowDeleteA)
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "UPDATE Drivers SET ServiceState = 5 WHERE DriverId = @DriverId";
                            _Command.Parameters.AddWithValue("@DriverId", DriverId);
                            _Command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        var _ServiceState = 0;
                        using (SqlCommand _CarService = _Connection.CreateCommand())
                        {

                            _CarService.CommandText =
                             @"SELECT ServiceState FROM Drivers WHERE DriverId = @DriverId";
                            _CarService.Parameters.AddWithValue("@DriverId", DriverId);
                            _ServiceState = Convert.ToInt32(_CarService.ExecuteScalar());
                        }
                        if (_ServiceState == 3)
                        {

                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                var Query = GetDriver(DriverId);

                                //if (!String.IsNullOrEmpty(Query.VBankName) || !String.IsNullOrEmpty(Query.VAccount))
                                //{
                                //    var bank_cd = "";
                                //    switch (Query.VBankName)
                                //    {
                                //        case "기업":
                                //            bank_cd = "003";
                                //            break;
                                //        case "국민":
                                //            bank_cd = "004";
                                //            break;
                                //        case "농협":
                                //            bank_cd = "011";
                                //            break;
                                //        case "우리":
                                //            bank_cd = "020";
                                //            break;
                                //        case "신한":
                                //            bank_cd = "088";
                                //            break;

                                //    }

                                //    _Command.CommandText = "DELETE FROM vacs_vact WHERE acct_no = @acct_no AND bank_cd = @bank_cd";
                                //    _Command.Parameters.AddWithValue("@acct_no", Query.VAccount);
                                //    _Command.Parameters.AddWithValue("@bank_cd", bank_cd);
                                //    _Command.ExecuteNonQuery();
                                //}
                            }

                            //등록이면 삭제
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "DELETE FROM Drivers WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }

                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "DELETE FROM DriverInstances WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "UPDATE Drivers SET ServiceState = 5 WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "DELETE FROM DriverInstances WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }
                        }

                       
                    }

                });
                return;
            }


            var _IsMyCar = IsMyCar(DriverId);
            var _IsAnotherCar = IsAnotherCar(DriverId);

            var _AllowDelete = AllowDelete(DriverId);
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                if (_IsMyCar && !_IsAnotherCar)
                {

                    if (_AllowDelete)
                    {
                        //using (SqlCommand _Command = _Connection.CreateCommand())
                        //{
                        //    _Command.CommandText = "DELETE FROM Drivers WHERE DriverId = @DriverId";
                        //    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        //    _Command.ExecuteNonQuery();
                        //}
                        //using (SqlCommand _Command = _Connection.CreateCommand())
                        //{
                        //    _Command.CommandText = "DELETE FROM DriverInstances WHERE DriverId = @DriverId";
                        //    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        //    _Command.ExecuteNonQuery();
                        //}
                    }
                    else
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            var Query = GetDriver(DriverId);

                            if (!String.IsNullOrEmpty(Query.VBankName) || !String.IsNullOrEmpty(Query.VAccount))
                            {
                                var bank_cd = "";
                                switch (Query.VBankName)
                                {
                                    case "기업":
                                        bank_cd = "003";
                                        break;
                                    case "국민":
                                        bank_cd = "004";
                                        break;
                                    case "농협":
                                        bank_cd = "011";
                                        break;
                                    case "우리":
                                        bank_cd = "020";
                                        break;
                                    case "신한":
                                        bank_cd = "088";
                                        break;

                                }

                                _Command.CommandText = "DELETE FROM vacs_vact WHERE acct_no = @acct_no AND bank_cd = @bank_cd";
                                _Command.Parameters.AddWithValue("@acct_no", Query.VAccount);
                                _Command.Parameters.AddWithValue("@bank_cd", bank_cd);
                                _Command.ExecuteNonQuery();
                            }
                        }


                        var _ServiceState1 = 0;
                        using (SqlCommand _CarService = _Connection.CreateCommand())
                        {

                            _CarService.CommandText =
                             @"SELECT ServiceState FROM Drivers WHERE DriverId = @DriverId";
                            _CarService.Parameters.AddWithValue("@DriverId", DriverId);
                            _ServiceState1 = Convert.ToInt32(_CarService.ExecuteScalar());
                        }
                        if (_ServiceState1 == 3)
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "DELETE FROM Drivers WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }

                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "DELETE FROM DriverInstances WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }

                            
                        }
                        else
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "UPDATE Drivers SET ServiceState = 5 WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "DELETE FROM DriverInstances WHERE DriverId = @DriverId";
                                _Command.Parameters.AddWithValue("@DriverId", DriverId);
                                _Command.ExecuteNonQuery();
                            }
                        }
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "UPDATE VAccountPools SET DriverId = NULL WHERE DriverId = @DriverId AND ClientId = @ClientId";
                            _Command.Parameters.AddWithValue("@DriverId", DriverId);
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            _Command.ExecuteNonQuery();
                        }


                        

                        //using (SqlCommand _Command = _Connection.CreateCommand())
                        //{
                        //    _Command.CommandText = "UPDATE Drivers SET ServiceState = 5 WHERE DriverId = @DriverId";
                        //    _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        //    _Command.ExecuteNonQuery();
                        //}
                    }
                    
                }
                else
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "DELETE FROM DriverInstances WHERE DriverId = @DriverId AND ClientId = @ClientId";
                        _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                }
                _Connection.Close();
            }
        }

        public void AllDelete(int ClientId)
        {
            ///환경설정 메뉴의 데이터삭제 기능이며, 모두 삭제 하지 않고, 해지처리한다.
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "DELETE FROM DriverInstances WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", ClientId);
                    _Command.ExecuteNonQuery();
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "UPDATE Drivers SET ServiceState = 5 WHERE CandidateId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", ClientId);
                    _Command.ExecuteNonQuery();
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "UPDATE VAccountPools SET DriverId = NULL WHERE ClientId = @ClientId ";
                    _Command.Parameters.AddWithValue("@ClientId", ClientId);
                    _Command.ExecuteNonQuery();
                }
                _Connection.Close();
            }
        }

        public bool HasUnregistrations()
        {
            bool _R = false;

            DataTable _T = new DataTable();
            List<String> _WhereStringList = new List<string>
            {
                "Drivers.ServiceState = 3",
                "Drivers.IsAgreed = 0",
                "Drivers.HasBizPaper = 1 AND Drivers.HasCarPaper = 1",
            };
            Select(_T, _WhereStringList);
            _R = _T.Rows.Count > 0;
            return _R;
        }

        public class Driver
        {
            public int DriverId { get; set; }
            public string CEO { get; set; }
            public string CarNo { get; set; }
            public int CarType { get; set; }
            public int CarSize { get; set; }
            public string CarYear { get; set; }
            public string MobileNo { get; set; }
            public string Name { get; set; }
            public string BizNo { get; set; }
            public string PayAccountNo { get; set; }
            public string PayBankName { get; set; }
            public string PayBankCode { get; set; }
            public string PayInputName { get; set; }
            public int ServiceState { get; set; }
            public string VAccount { get; set; }
            public string VBankName { get; set; }

            public string Uptae { get; set; }
            public string Upjong { get; set; }
            public string Addresstate { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }
        }
    }
}
