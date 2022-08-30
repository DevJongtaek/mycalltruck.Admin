using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using System.Runtime.CompilerServices;
using System.Net.Sockets;
using System.IO;
using OfficeOpenXml;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;

namespace mycalltruck.Admin
{
    public partial class Dialog_BankUpload : Form
    {
        string FileName = string.Empty;
        string SFileName = string.Empty;
        int DataCount = 0;
        string Idx = string.Empty;
        string transperDate = "";
        string SStatus = "";
        string szConn = "";
        string sheet1 = "";
        public Dialog_BankUpload()
        {
            InitializeComponent();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);

            Dictionary<string, string> PayBank = new Dictionary<string, string>();

            PayBank.Add("국민은행", "국민은행");
            PayBank.Add("우리은행", "우리은행");
            PayBank.Add("신한은행", "신한은행");
            PayBank.Add("기업은행", "기업은행");
            PayBank.Add("하나은행", "하나은행");
            PayBank.Add("농협", "농협");

            cmb_Bank.DataSource = new BindingSource(PayBank, null);
            cmb_Bank.DisplayMember = "Value";
            cmb_Bank.ValueMember = "Key";


            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
          
        }

        public TradeDataSet.TradesRow[] Trades { get; set; }

      
        private TradeDataSet.TradesRow[] AcceptTrades { get; set; }
        List<TradeDataSet.TradesRow> Accepted = new List<TradeDataSet.TradesRow>();

        //public BankDataSet.BankUploadMasterRow[] BankUploadMaster { get; set; }
        //List<BankDataSet.BankUploadMasterRow> BmAccepted = new List<BankDataSet.BankUploadMasterRow>();
        //public BankDataSet.BankUploadDetailRow[] BankUploadDetail { get; set; }
        //List<BankDataSet.BankUploadDetailRow> BdAccepted = new List<BankDataSet.BankUploadDetailRow>();
        private int CurrentStep = 1;
        private bool IsLoadClientAccs = false;
        private bool IsLoadStep2 = false;
        private bool IsLoadStep3 = false;
        private bool IsLoadStep4 = false;
        private bool NeedCheckCustomerAccLimite = false;
        private int Step5Total = 0;
        private int Step5Done = 0;
        private Int64 Step5TotalMoney = 0;
        private int Step5Success = 0;
        private Int64 Step5SuccessMoney = 0;
        private int Step5Failed = 0;
        private Int64 Step5FailedMoney = 0;
        private bool IsClosed = false;
        private int BUMId = 0;


        //Parameter


        private String GetSelectCommand()
        {
            return @"SELECT  TradeId, Trades.RequestDate, BeginDate, EndDate, 
                Item, Price, VAT, Amount, PayState, PayDate, Trades.PayBankName, Trades.PayBankCode, Trades.PayAccountNo, Trades.PayInputName, 
                Trades.DriverId, Trades.ClientId, Trades.UseTax, LGD_OID, 
                LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, AllowAcc, HasAcc, ClientAccId, 
                SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, 
                Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, 
                HasETax, SourceType,
                Drivers.LoginId AS DriverLoginId, Drivers.CarNo AS DriverCarNo, Drivers.BizNo AS DriverBizNo, Drivers.Name AS DriverName, 
                Drivers.ServiceState, Drivers.MID, 
                Clients.Code AS ClientCode, Clients.Name AS ClientName, Trades.AcceptCount, Trades.SubClientId, Trades.ClientUserId
                ,(SELECT ISNULL(GroupName,'미설정') FROM  DriverInstances WHERE DriverId = Trades.DriverId and ClientId = Trades.ClientId) as GroupName
                ,Trades.ExcelExportYN,ISNULL(Trades.EtaxCanCelYN,'N') AS EtaxCanCelYN,TransportDate,StartState,StopState
                FROM     Trades
                JOIN Drivers ON Trades.DriverId = Drivers.DriverId
                JOIN Clients ON Trades.ClientId = Clients.ClientId 
                "
              ;


        }
        private void LoadTable()
        {
            tradeDataSet.Trades.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();
                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        WhereStringList.Add("Trades.ClientId = @ClientId");
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        
                    }
                   

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        tradeDataSet.Trades.Load(_Reader);


                    }
                }
                _Connection.Close();
            }
        }

        private void btn_ShowDenyList_Click(object sender, EventArgs e)
        {
           // Step2DenyList.Visible = true;
        }


      
      

        private void ShowStep4()
        {
            LoadTable();

               CurrentStep = 4;

           
            pnStep4.Visible = true;
            pnStep5.Visible = false;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[4].Height = 100F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[5].Height = 0F;

           
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Bold);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Regular);
          
            lbl_Step4.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step5.ForeColor = Color.FromArgb(137, 206, 250);
          
            bar_Step4.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step5.BackColor = Color.FromArgb(19, 124, 192);
            btn_Pre.Enabled = true;
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            btn_Next.Text = "다음";
            




            if (!IsLoadStep4)
            {
              


                IsLoadStep4 = true;


            }

           


            if (newDGV2.RowCount > 0)
            {
                btn_Next.Enabled = true;
            }



        }

       

        private void ShowStep5()
        {
            CurrentStep = 5;

           
            pnStep3.Visible = false;
            pnStep4.Visible = false;
            pnStep5.Visible = true;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[4].Height = 0F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[5].Height = 100F;

           
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Regular);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Bold);
           
            lbl_Step4.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step5.ForeColor = Color.FromKnownColor(KnownColor.White);
          
            bar_Step4.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step5.BackColor = Color.FromKnownColor(KnownColor.White);
            btn_Pre.Enabled = true;
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            
            AcceptTrades = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as TradeDataSet.TradesRow).ToArray();
            Step5Total = AcceptTrades.Length;
            Step5Progress.Maximum = Step5Total;
            Step5Progress.Value = Step5Done;

            Step5Display(true);
            
            Step5Work();
        }
        private void ExcelExportBasic()
        {
            
            string SPayBankName = "";
            string SPayAccountNo = "";
            string SAmount = "";
            string SAccountName = "";
            int STradeId = 0;
            switch (cmb_Bank.Text)
            {
                case "국민은행":
                   
                    _CoreList.Clear();
                    List<DataGridViewRow> dataRows = new List<DataGridViewRow>();
                    if(newDGV2.RowCount > 0)
                    {
                        MasterSave();
                    }
                    else
                    {
                        return;
                    }
                    foreach (DataGridViewRow row in newDGV2.Rows)
                    {
                       
                        

                        Idx = row.Cells[0].Value.ToString();
                        SStatus = row.Cells[7].Value.ToString().Trim();

                        if (Idx != "" && SStatus == "정상")
                        {
                            Step5Total++;

                            SPayBankName = row.Cells[2].Value.ToString().Trim();
                            SPayAccountNo = row.Cells[3].Value.ToString().Trim().Replace("-", "");
                            SAmount = row.Cells[4].Value.ToString().Trim().Replace(",", "");
                            SAccountName = row.Cells[5].Value.ToString().Trim().Replace(",", "");


                            var Query = tradeDataSet.Trades.Where(c => c.ExcelExportYN == transperDate && c.PayBankName.Contains(SPayBankName) && c.PayAccountNo.Replace("-", "").Replace("\0", "").Trim() == SPayAccountNo && Convert.ToInt32(c.Amount).ToString().Replace(",", "") == SAmount  && c.EtaxCanCelYN == "N" && c.PayState == 2);



                            Step5TotalMoney += Convert.ToInt64(SAmount);







                            if (Query.Count() > 0)
                            {


                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();


                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "UPDATE Trades SET PayDate = @PayDate ,PayState = 1 ,HasAcc = 0 WHERE TradeId = @TradeId";
                                        _Command.Parameters.AddWithValue("@PayDate", DateTime.Now);
                                        _Command.Parameters.AddWithValue("@TradeId", Query.First().TradeId);
                                        _Command.ExecuteNonQuery();

                                        try
                                        {
                                            Step5Success++;
                                            Step5SuccessMoney += Convert.ToInt64(SAmount);


                                            #region 엑셀데이터 DB입력
                                            BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                            Row.BUMId = BUMId;
                                            Row.ExcelIdx = Idx;
                                            Row.BankName = SPayBankName;
                                            Row.BankAccount = SPayAccountNo;
                                            Row.BankAccountName = SAccountName;
                                            Row.Amount = Convert.ToInt64(SAmount);
                                            Row.Result = "성공";
                                            Row.Remark = "";

                                            bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                            bankUploadDetailTableAdapter.Update(Row);
                                            #endregion


                                        }
                                        catch
                                        {

                                            continue;
                                        }
                                    }


                                    using (SqlCommand cmdDriver = _Connection.CreateCommand())
                                    {


                                        cmdDriver.CommandText =
                                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                                        cmdDriver.Parameters.AddWithValue("@Mizi", Query.First().Amount);
                                        cmdDriver.Parameters.AddWithValue("@DriverId", Query.First().DriverId);
                                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        cmdDriver.ExecuteNonQuery();
                                    }


                                    _Connection.Close();
                                }
                            }
                            else
                            {
                                var Added = new Model
                                {
                                    IDX = Idx,
                                    PayBankName = SPayBankName,
                                    PayAccountNo = SPayAccountNo.ToString(),
                                    PayAmount = SAmount,
                                    Remark = "해당데이터 없음"

                                };

                                _CoreList.Add(Added);

                                Step5FailedMoney += Convert.ToInt64(SAmount);

                                Step5Failed++;

                                //
                                #region 엑셀데이터 DB입력
                                BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                Row.BUMId = BUMId;
                                Row.ExcelIdx = Idx;
                                Row.BankName = SPayBankName;
                                Row.BankAccount = SPayAccountNo;
                                Row.BankAccountName = SAccountName;
                                Row.Amount = Convert.ToInt64(SAmount);
                                Row.Result = "실패";
                                Row.Remark = "해당데이터 없음";

                                bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                bankUploadDetailTableAdapter.Update(Row);
                                #endregion
                            }
                        }
                    }


                    MasterUpdate();




                    break;

                case "우리은행":
                    _CoreList.Clear();
                    dataRows = new List<DataGridViewRow>();
                    if (newDGV2.RowCount > 0)
                    {
                        MasterSave();
                    }
                    else
                    {
                        return;
                    }
                    foreach (DataGridViewRow row in newDGV2.Rows)
                    {


                        Idx = row.Cells[0].Value.ToString();
                        SStatus = row.Cells[1].Value.ToString().Trim();
                        transperDate = row.Cells[2].Value.ToString().Trim().Replace(".", "/");

                        if (Idx != "" && SStatus == "정상")
                        {
                            Step5Total++;
                            SPayBankName = row.Cells[4].Value.ToString().Trim().Substring(0, 2);
                            SPayAccountNo = row.Cells[5].Value.ToString().Trim().Replace("-", "");
                            SAmount = row.Cells[8].Value.ToString().Trim().Replace(",", "");

                            var Query = tradeDataSet.Trades.Where(c => c.ExcelExportYN == transperDate && c.PayBankName.Contains(SPayBankName) && c.PayAccountNo.Replace("-", "").Replace("\0", "").Trim() == SPayAccountNo && Convert.ToInt32(c.Amount).ToString().Replace(",", "") == SAmount  && c.EtaxCanCelYN == "N" && c.PayState == 2);



                            Step5TotalMoney += Convert.ToInt64(SAmount);







                            if (Query.Count() > 0)
                            {


                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();


                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "UPDATE Trades SET PayDate = @PayDate ,PayState = 1,HasAcc = 0 WHERE TradeId = @TradeId";
                                        _Command.Parameters.AddWithValue("@PayDate", DateTime.Now);
                                        _Command.Parameters.AddWithValue("@TradeId", Query.First().TradeId);
                                        _Command.ExecuteNonQuery();

                                     

                                        try
                                        {
                                            Step5Success++;
                                            Step5SuccessMoney += Convert.ToInt64(SAmount);


                                            #region 엑셀데이터 DB입력
                                            BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                            Row.BUMId = BUMId;
                                            Row.ExcelIdx = Idx;
                                            Row.BankName = SPayBankName;
                                            Row.BankAccount = SPayAccountNo;
                                            Row.BankAccountName = SAccountName;
                                            Row.Amount = Convert.ToInt64(SAmount);
                                            Row.Result = "성공";
                                            Row.Remark = "";

                                            bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                            bankUploadDetailTableAdapter.Update(Row);
                                            #endregion


                                        }
                                        catch
                                        {

                                            continue;
                                        }
                                    }

                                    using (SqlCommand cmdDriver = _Connection.CreateCommand())
                                    {


                                        cmdDriver.CommandText =
                                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                                        cmdDriver.Parameters.AddWithValue("@Mizi", Query.First().Amount);
                                        cmdDriver.Parameters.AddWithValue("@DriverId", Query.First().DriverId);
                                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        cmdDriver.ExecuteNonQuery();
                                    }

                                    _Connection.Close();
                                }
                            }
                            else
                            {
                                var Added = new Model
                                {
                                    IDX = Idx,
                                    PayBankName = SPayBankName,
                                    PayAccountNo = SPayAccountNo.ToString(),
                                    PayAmount = SAmount,
                                    Remark = "해당데이터 없음"

                                };

                                _CoreList.Add(Added);

                                Step5FailedMoney += Convert.ToInt64(SAmount);

                                Step5Failed++;

                                //
                                #region 엑셀데이터 DB입력
                                BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                Row.BUMId = BUMId;
                                Row.ExcelIdx = Idx;
                                Row.BankName = SPayBankName;
                                Row.BankAccount = SPayAccountNo;
                                Row.BankAccountName = SAccountName;
                                Row.Amount = Convert.ToInt64(SAmount);
                                Row.Result = "실패";
                                Row.Remark = "해당데이터 없음";

                                bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                bankUploadDetailTableAdapter.Update(Row);
                                #endregion
                            }
                        }
                    }




                    MasterUpdate();


                    break;
                case "하나은행":
                    _CoreList.Clear();
                    dataRows = new List<DataGridViewRow>();
                    if (newDGV2.RowCount > 0)
                    {
                        MasterSave();
                    }
                    else
                    {
                        return;
                    }
                    foreach (DataGridViewRow row in newDGV2.Rows)
                    {


                        Idx = row.Cells[0].Value.ToString();
                        SStatus = row.Cells[1].Value.ToString().Trim();

                        transperDate = sheet1.Substring(sheet1.Length - 9).Replace("$", "");

                        if (Idx != "" && SStatus == "완료")
                        {
                            Step5Total++;

                            SPayBankName = row.Cells[2].Value.ToString().Trim().Substring(0, 2);
                            SPayAccountNo = row.Cells[3].Value.ToString().Trim().Replace("-", "");
                            SAmount = row.Cells[4].Value.ToString().Trim().Replace(",", "");



                            //foreach (var trade in Trades)
                            //{
                            //    string tAmount = Convert.ToInt32(trade.Amount).ToString().Replace(",", "");

                            //    if (trade.ExcelExportYN == transperDate && trade.PayBankName.Contains(SPayBankName) && trade.PayAccountNo.Replace("-", "").Replace("\0", "") == SPayAccountNo && tAmount == SAmount && trade.HasAcc == false && trade.HasETax && trade.EtaxCanCelYN == "N" && trade.PayState == 2)
                            //        Accepted.Add(trade);
                            //}


                            var Query = tradeDataSet.Trades.Where(c => c.ExcelExportYN.Replace("/","") == transperDate && c.PayBankName.Contains(SPayBankName) && c.PayAccountNo.Replace("-", "").Replace("\0", "").Trim() == SPayAccountNo && Convert.ToInt32(c.Amount).ToString().Replace(",", "") == SAmount  && c.EtaxCanCelYN == "N" && c.PayState == 2);



                            Step5TotalMoney += Convert.ToInt64(SAmount);







                            if (Query.Count() > 0)
                            {


                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();


                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "UPDATE Trades SET PayDate = @PayDate ,PayState = 1,HasAcc = 0 WHERE TradeId = @TradeId";
                                        _Command.Parameters.AddWithValue("@PayDate", DateTime.Now);
                                        _Command.Parameters.AddWithValue("@TradeId", Query.First().TradeId);
                                        _Command.ExecuteNonQuery();

                                       

                                        try
                                        {
                                            Step5Success++;
                                            Step5SuccessMoney += Convert.ToInt64(SAmount);


                                            #region 엑셀데이터 DB입력
                                            BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                            Row.BUMId = BUMId;
                                            Row.ExcelIdx = Idx;
                                            Row.BankName = SPayBankName;
                                            Row.BankAccount = SPayAccountNo;
                                            Row.BankAccountName = SAccountName;
                                            Row.Amount = Convert.ToInt64(SAmount);
                                            Row.Result = "성공";
                                            Row.Remark = "";

                                            bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                            bankUploadDetailTableAdapter.Update(Row);
                                            #endregion


                                        }
                                        catch
                                        {

                                            continue;
                                        }
                                    }

                                    using (SqlCommand cmdDriver = _Connection.CreateCommand())
                                    {


                                        cmdDriver.CommandText =
                                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                                        cmdDriver.Parameters.AddWithValue("@Mizi", Query.First().Amount);
                                        cmdDriver.Parameters.AddWithValue("@DriverId", Query.First().DriverId);
                                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        cmdDriver.ExecuteNonQuery();
                                    }

                                    _Connection.Close();
                                }
                            }
                            else
                            {
                                var Added = new Model
                                {
                                    IDX = Idx,
                                    PayBankName = SPayBankName,
                                    PayAccountNo = SPayAccountNo.ToString(),
                                    PayAmount = SAmount,
                                    Remark = "해당데이터 없음"

                                };

                                _CoreList.Add(Added);

                                Step5FailedMoney += Convert.ToInt64(SAmount);

                                Step5Failed++;

                                //
                                #region 엑셀데이터 DB입력
                                BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                Row.BUMId = BUMId;
                                Row.ExcelIdx = Idx;
                                Row.BankName = SPayBankName;
                                Row.BankAccount = SPayAccountNo;
                                Row.BankAccountName = SAccountName;
                                Row.Amount = Convert.ToInt64(SAmount);
                                Row.Result = "실패";
                                Row.Remark = "해당데이터 없음";

                                bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                bankUploadDetailTableAdapter.Update(Row);
                                #endregion
                            }
                        }
                    }
            
                    MasterUpdate();
                    break;
                case "기업은행":
                    _CoreList.Clear();
                    dataRows = new List<DataGridViewRow>();
                    if (newDGV2.RowCount > 0)
                    {
                        MasterSave();
                    }
                    else
                    {
                        return;
                    }
                    foreach (DataGridViewRow row in newDGV2.Rows)
                    {


                        Idx = row.Cells[9].Value.ToString();
                        SStatus = row.Cells[0].Value.ToString().Trim();

                        transperDate = sheet1.Substring(sheet1.Length - 9).Replace("$", "");

                        if (Idx != "" && SStatus == "정상")
                        {
                            Step5Total++;
                            SPayBankName = row.Cells[1].Value.ToString().Trim();
                             SPayAccountNo = row.Cells[2].Value.ToString().Trim().Replace("-", "");
                             SAmount = row.Cells[3].Value.ToString().Trim().Replace(",", "");



                            var Query = tradeDataSet.Trades.Where(c => c.ExcelExportYN.Replace("/","") == transperDate && c.PayBankName.Contains(SPayBankName) && c.PayAccountNo.Replace("-", "").Replace("\0", "").Trim() == SPayAccountNo && Convert.ToInt32(c.Amount).ToString().Replace(",", "") == SAmount  && c.EtaxCanCelYN == "N" && c.PayState == 2);



                            Step5TotalMoney += Convert.ToInt64(SAmount);







                            if (Query.Count() > 0)
                            {


                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();


                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "UPDATE Trades SET PayDate = @PayDate ,PayState = 1 ,HasAcc = 0 WHERE TradeId = @TradeId";
                                        _Command.Parameters.AddWithValue("@PayDate", DateTime.Now);
                                        _Command.Parameters.AddWithValue("@TradeId", Query.First().TradeId);
                                        _Command.ExecuteNonQuery();
                                        try
                                        {
                                            Step5Success++;
                                            Step5SuccessMoney += Convert.ToInt64(SAmount);


                                            #region 엑셀데이터 DB입력
                                            BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                            Row.BUMId = BUMId;
                                            Row.ExcelIdx = Idx;
                                            Row.BankName = SPayBankName;
                                            Row.BankAccount = SPayAccountNo;
                                            Row.BankAccountName = SAccountName;
                                            Row.Amount = Convert.ToInt64(SAmount);
                                            Row.Result = "성공";
                                            Row.Remark = "";

                                            bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                            bankUploadDetailTableAdapter.Update(Row);
                                            #endregion


                                        }
                                        catch
                                        {

                                            continue;
                                        }
                                    }

                                    using (SqlCommand cmdDriver = _Connection.CreateCommand())
                                    {


                                        cmdDriver.CommandText =
                                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                                        cmdDriver.Parameters.AddWithValue("@Mizi", Query.First().Amount);
                                        cmdDriver.Parameters.AddWithValue("@DriverId", Query.First().DriverId);
                                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        cmdDriver.ExecuteNonQuery();
                                    }

                                    _Connection.Close();
                                }
                            }
                            else
                            {
                                var Added = new Model
                                {
                                    IDX = Idx,
                                    PayBankName = SPayBankName,
                                    PayAccountNo = SPayAccountNo.ToString(),
                                    PayAmount = SAmount,
                                    Remark = "해당데이터 없음"

                                };

                                _CoreList.Add(Added);

                                Step5FailedMoney += Convert.ToInt64(SAmount);

                                Step5Failed++;

                                //
                                #region 엑셀데이터 DB입력
                                BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                Row.BUMId = BUMId;
                                Row.ExcelIdx = Idx;
                                Row.BankName = SPayBankName;
                                Row.BankAccount = SPayAccountNo;
                                Row.BankAccountName = SAccountName;
                                Row.Amount = Convert.ToInt64(SAmount);
                                Row.Result = "실패";
                                Row.Remark = "해당데이터 없음";

                                bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                bankUploadDetailTableAdapter.Update(Row);
                                #endregion
                            }
                        }
                    }
                    MasterUpdate();
                    break;

                case "농협":
                    _CoreList.Clear();
                    dataRows = new List<DataGridViewRow>();
                    if (newDGV2.RowCount > 0)
                    {
                        MasterSave();
                    }
                    else
                    {
                        return;
                    }
                    foreach (DataGridViewRow row in newDGV2.Rows)
                    {


                        Idx = row.Cells[0].Value.ToString();
                        SStatus = row.Cells[7].Value.ToString().Trim();

                        transperDate = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");

                        if (Idx != "" && SStatus == "정상입금")
                        {
                            Step5Total++;
                            SPayBankName = row.Cells[1].Value.ToString().Trim();
                            SPayAccountNo = row.Cells[2].Value.ToString().Trim().Replace("-", "");
                            SAmount = row.Cells[3].Value.ToString().Trim().Replace(",", "");



                            var Query = tradeDataSet.Trades.Where(c => c.ExcelExportYN == transperDate && c.PayBankName.Contains(SPayBankName) && c.PayAccountNo.Replace("-", "").Replace("\0", "") == SPayAccountNo && Convert.ToInt32(c.Amount).ToString().Replace(",", "") == SAmount  && c.EtaxCanCelYN == "N" && c.PayState == 2);



                            Step5TotalMoney += Convert.ToInt64(SAmount);







                            if (Query.Count() > 0)
                            {


                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();


                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "UPDATE Trades SET PayDate = @PayDate ,PayState = 1 ,HasAcc = 0 WHERE TradeId = @TradeId";
                                        _Command.Parameters.AddWithValue("@PayDate", DateTime.Now);
                                        _Command.Parameters.AddWithValue("@TradeId", Query.First().TradeId);
                                        _Command.ExecuteNonQuery();
                                        try
                                        {
                                            Step5Success++;
                                            Step5SuccessMoney += Convert.ToInt64(SAmount);


                                            #region 엑셀데이터 DB입력
                                            BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                            Row.BUMId = BUMId;
                                            Row.ExcelIdx = Idx;
                                            Row.BankName = SPayBankName;
                                            Row.BankAccount = SPayAccountNo;
                                            Row.BankAccountName = SAccountName;
                                            Row.Amount = Convert.ToInt64(SAmount);
                                            Row.Result = "성공";
                                            Row.Remark = "";

                                            bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                            bankUploadDetailTableAdapter.Update(Row);
                                            #endregion


                                        }
                                        catch
                                        {

                                            continue;
                                        }
                                    }

                                    using (SqlCommand cmdDriver = _Connection.CreateCommand())
                                    {


                                        cmdDriver.CommandText =
                                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                                        cmdDriver.Parameters.AddWithValue("@Mizi", Query.First().Amount);
                                        cmdDriver.Parameters.AddWithValue("@DriverId", Query.First().DriverId);
                                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        cmdDriver.ExecuteNonQuery();
                                    }

                                    _Connection.Close();
                                }
                            }
                            else
                            {
                                var Added = new Model
                                {
                                    IDX = Idx,
                                    PayBankName = SPayBankName,
                                    PayAccountNo = SPayAccountNo.ToString(),
                                    PayAmount = SAmount,
                                    Remark = "해당데이터 없음"

                                };

                                _CoreList.Add(Added);

                                Step5FailedMoney += Convert.ToInt64(SAmount);

                                Step5Failed++;

                                //
                                #region 엑셀데이터 DB입력
                                BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                Row.BUMId = BUMId;
                                Row.ExcelIdx = Idx;
                                Row.BankName = SPayBankName;
                                Row.BankAccount = SPayAccountNo;
                                Row.BankAccountName = SAccountName;
                                Row.Amount = Convert.ToInt64(SAmount);
                                Row.Result = "실패";
                                Row.Remark = "해당데이터 없음";

                                bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                bankUploadDetailTableAdapter.Update(Row);
                                #endregion
                            }
                        }
                    }
                    MasterUpdate();
                    break;


                case "신한은행":
                    _CoreList.Clear();
                    dataRows = new List<DataGridViewRow>();
                    if (newDGV2.RowCount > 0)
                    {
                        MasterSave();
                    }
                    else
                    {
                        return;
                    }
                    foreach (DataGridViewRow row in newDGV2.Rows)
                    {
                        int i = 0;
                        i++;
                        Idx = i.ToString();
                        SStatus = row.Cells[0].Value.ToString().Trim();

                        transperDate = row.Cells[1].Value.ToString().Trim().Replace(".","/");

                        if (SStatus == "처리완료")
                        {
                            Step5Total++;

                            SPayBankName = row.Cells[5].Value.ToString().Trim().Substring(0,2);

                            //if(SPayBankName =="KE")
                            //{
                            //    SPayBankName = "하나";
                            //}
                            //if(SPayBankName =="신용")
                            //{
                            //    SPayBankName = "신협";
                            //}
                            SPayAccountNo = row.Cells[6].Value.ToString().Trim().Replace("-", "");
                            SAmount = row.Cells[9].Value.ToString().Trim().Replace(",", "");

                            STradeId = Convert.ToInt32(row.Cells[17].Value.ToString());

                            //var Query = tradeDataSet.Trades.Where(c => c.ExcelExportYN == transperDate && c.PayBankName.Contains(SPayBankName) && c.PayAccountNo.Replace("-", "").Replace("\0", "").Trim() == SPayAccountNo && Convert.ToInt32(c.Amount).ToString().Replace(",", "") == SAmount  && c.EtaxCanCelYN == "N" && c.PayState == 2 && c.TradeId == STradeId);

                            var Query = tradeDataSet.Trades.Where(c =>  c.PayAccountNo.Replace("-", "").Replace("\0", "").Trim() == SPayAccountNo && Convert.ToInt32(c.Amount).ToString().Replace(",", "") == SAmount && c.EtaxCanCelYN == "N" && c.PayState == 2 && c.TradeId == STradeId);


                            Step5TotalMoney += Convert.ToInt64(SAmount);







                            if (Query.Count() > 0)
                            {


                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();


                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "UPDATE Trades SET PayDate = @PayDate ,PayState = 1 ,HasAcc = 0 WHERE TradeId = @TradeId";
                                        _Command.Parameters.AddWithValue("@PayDate", DateTime.Now);
                                        _Command.Parameters.AddWithValue("@TradeId", Query.First().TradeId);
                                        _Command.ExecuteNonQuery();
                                        try
                                        {
                                            Step5Success++;
                                            Step5SuccessMoney += Convert.ToInt64(SAmount);


                                            #region 엑셀데이터 DB입력
                                            BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                            Row.BUMId = BUMId;
                                            Row.ExcelIdx = Idx;
                                            Row.BankName = SPayBankName;
                                            Row.BankAccount = SPayAccountNo;
                                            Row.BankAccountName = SAccountName;
                                            Row.Amount = Convert.ToInt64(SAmount);
                                            Row.Result = "성공";
                                            Row.Remark = "";

                                            bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                            bankUploadDetailTableAdapter.Update(Row);
                                            #endregion


                                        }
                                        catch
                                        {

                                            continue;
                                        }
                                    }
                                    using (SqlCommand cmdDriver = _Connection.CreateCommand())
                                    {


                                        cmdDriver.CommandText =
                                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                                        cmdDriver.Parameters.AddWithValue("@Mizi", Query.First().Amount);
                                        cmdDriver.Parameters.AddWithValue("@DriverId", Query.First().DriverId);
                                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        cmdDriver.ExecuteNonQuery();
                                    }
                                    _Connection.Close();
                                }
                            }
                            else
                            {
                                var Added = new Model
                                {
                                    IDX = Idx,
                                    PayBankName = SPayBankName,
                                    PayAccountNo = SPayAccountNo.ToString(),
                                    PayAmount = SAmount,
                                    Remark = "해당데이터 없음"

                                };

                                _CoreList.Add(Added);

                                Step5FailedMoney += Convert.ToInt64(SAmount);

                                Step5Failed++;

                                //
                                #region 엑셀데이터 DB입력
                                BankDataSet.BankUploadDetailRow Row = bankDataSet.BankUploadDetail.NewBankUploadDetailRow();
                                Row.BUMId = BUMId;
                                Row.ExcelIdx = Idx;
                                Row.BankName = SPayBankName;
                                Row.BankAccount = SPayAccountNo;
                                Row.BankAccountName = SAccountName;
                                Row.Amount = Convert.ToInt64(SAmount);
                                Row.Result = "실패";
                                Row.Remark = "해당데이터 없음";

                                bankDataSet.BankUploadDetail.AddBankUploadDetailRow(Row);
                                bankUploadDetailTableAdapter.Update(Row);
                                #endregion
                            }
                        }
                    }
                    MasterUpdate();
                    break;
            }


            try
            {

                Step5Complete();


            }
            catch (Exception ex)
            {

                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }

        }

        private void MasterSave()
        {
            
             
            //업로드파일저장
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "INSERT INTO BankUploadMaster " +
                        "(RequestDate, FileName, TotalCount, SuccessCount, FailedCount, TotalAmount, SuccessAmount, FailAmount, ClientId)" +
                        "VALUES" +
                        "(@RequestDate, @FileName, @TotalCount, @SuccessCount, @FailedCount, @TotalAmount, @SuccessAmount, @FailAmount, @ClientId)" +
                        "  SELECT @@IDENTITY ";
                    _Command.Parameters.AddWithValue("@RequestDate", DateTime.Now);
                    _Command.Parameters.AddWithValue("@FileName", SFileName);
                    _Command.Parameters.AddWithValue("@TotalCount", newDGV2.RowCount);
                    _Command.Parameters.AddWithValue("@SuccessCount", 0);
                    _Command.Parameters.AddWithValue("@FailedCount", 0);
                    _Command.Parameters.AddWithValue("@TotalAmount", 0);
                    _Command.Parameters.AddWithValue("@SuccessAmount", 0);
                    _Command.Parameters.AddWithValue("@FailAmount", 0);
             
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    // _Command.ExecuteNonQuery();
                    BUMId = Convert.ToInt32(_Command.ExecuteScalar());


                }

                _Connection.Close();
            }
        }

        private void MasterUpdate()
        {

            //업로드파일저장
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "UPDATE BankUploadMaster " +
                       " SET SuccessCount = @SuccessCount" +
                       " ,FailedCount = @FailedCount" +
                       " ,TotalAmount = @TotalAmount" +
                       " ,SuccessAmount = @SuccessAmount" +
                       ", FailAmount = @FailAmount" +
                       ",TotalCount = @TotalCount" +
                       " WHERE BUMId = @BUMId";

                    _Command.Parameters.AddWithValue("@TotalCount", Step5Total);
                    _Command.Parameters.AddWithValue("@SuccessCount", Step5Success);
                    _Command.Parameters.AddWithValue("@FailedCount", Step5Failed);
                    _Command.Parameters.AddWithValue("@TotalAmount", Step5TotalMoney);
                    _Command.Parameters.AddWithValue("@SuccessAmount", Step5SuccessMoney);
                    _Command.Parameters.AddWithValue("@FailAmount", Step5FailedMoney);
                    _Command.Parameters.AddWithValue("@BUMId", BUMId);
                     _Command.ExecuteNonQuery();
                 

                }

                _Connection.Close();
            }
        }

        private void Step5Work()
        {
            if (IsClosed)
                return;

            try
            {
                ExcelExportBasic();
                //Step5Complete();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           
        }
        


        private void Step5Complete()
        {
            Step5Info.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            Step5Info.Font = new Font(Step5Info.Font, FontStyle.Bold);
            Step5Info.Text = "적용 완료 되었습니다.";
          
            Step5TotalCount.Text = Step5Total.ToString();
            Step5SuccessCount.Text = Step5Success.ToString(); ;
            Step5FailedCount.Text = Step5Failed.ToString();
            btn_Next.Text = "완료";
            btn_Close.Enabled = false;
            btn_Next.Enabled = true;
        }

        BindingList<BankExport> AccLogsItemBindingList = new BindingList<BankExport>();
        private void Step5Display(bool first, int _TradeId = 0)
        {


            newDGV1.DataSource = null;

            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _CoreList;
            Step5Progress.Value = Step5Done;
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 4)
            {

                if (MessageBox.Show("" + cmb_Bank.Text + " 일괄 지급이체 처리결과를 적용 하시겠습니까? ", "대량이체 결과 적용", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    ShowStep5();

            }
            else if (CurrentStep == 5)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btn_Pre_Click(object sender, EventArgs e)
        {
            ShowStep4();

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Step3List_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (Step3List.RowCount - e.RowIndex).ToString("N0");
            }
        }

        private void Dialog_AcceptAcc_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsClosed = true;
        }
        class DenyedItem
        {
            public String DriverLoginId { get; set; }
            public String DriverCarNo { get; set; }
            public String DriverName { get; set; }

            //바로결제신청
            public String CheckHasAcc { get; set; }

            public String CheckSetYN { get; set; }
            public String MID { get; set; }
        }

        class BankExport : INotifyPropertyChanged
        {
            private int _Id;
            private String _DriverLoginId;
            private String _DriverCarNo;
            private String _DriverName;
            private String _Amount;
            private String _BankName;
            private String _PayAccountNo;
            private String _BankCode;
            private String _Remark;
            //

            public int Id { get { return _Id; } set { SetField(ref _Id, value); } }
            public string DriverLoginId { get { return _DriverLoginId; } set { SetField(ref _DriverLoginId, value); } }
            public string DriverCarNo { get { return _DriverCarNo; } set { SetField(ref _DriverCarNo, value); } }
            public string DriverName { get { return _DriverName; } set { SetField(ref _DriverName, value); } }
            public string Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            public string BankName { get { return _BankName; } set { SetField(ref _BankName, value); } }
            public string PayAccountNo { get { return _PayAccountNo; } set { SetField(ref _PayAccountNo, value); } }
            public string BankCode { get { return _BankCode; } set { SetField(ref _BankCode, value); } }
            public string Remark { get { return _Remark; } set { SetField(ref _Remark, value); } }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }

      

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (newDGV1.RowCount - e.RowIndex).ToString("N0");
            }
        }


        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;
        private void btnExcelExport_Click(object sender, EventArgs e)
        {

            fileString = cmb_Bank.Text + string.Format("실패내역") + DateTime.Now.ToString("yyyyMMdd");
            title = "실패내역";

            ieExcel = Properties.Resources.실패결과;





            if (newDGV1.RowCount == 0)
            {
                MessageBox.Show("내보낼 실패내역 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (newDGV1.RowCount > 0)
            {
                pnProgress.Visible = true;
                bar.Value = 0;
                Thread t = new Thread(new ThreadStart(() =>
                {
                    newDGV1.ExportExistExcel2(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.MYBANKNEW);
                    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

               

                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            else
            {
                MessageBox.Show("엑셀로 내보낼 실패내역 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }





        private void chkCardNumSave_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkCardNumSave.Checked)
            //{


            //    //LocalUser.Instance.LogInInfomation.CardNumSave = true;
            //    //LocalUser.Instance.LogInInfomation.CardNum = Step4CardNo.Text;
            //    //LocalUser.Instance.Write();

            //}
            //else
            //{
            //    Step4CardNo.Text = "";
            //    //LocalUser.Instance.LogInInfomation.CardNumSave = false;
            //    //LocalUser.Instance.LogInInfomation.CardNum = "";
            //    //LocalUser.Instance.Write();

            //}
        }

        private void Dialog_AcceptAcc_Load(object sender, EventArgs e)
        {
            ShowStep4();
        }



        private void Step4CardNo1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Step4CardNo1.Text.Length == 4)
            //{
            //    Step4CardNo2.Text = "";
            //    Step4CardNo2.Focus();

            //}
        }
        private void Step4CardNo2_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Step4CardNo2.Text.Length == 4)
            //{
            //    Step4CardNo3.Text = "";
            //    Step4CardNo3.Focus();

            //}
        }
        private void Step4CardNo3_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Step4CardNo3.Text.Length == 4)
            //{
            //    Step4CardNo4.Text = "";
            //    Step4CardNo4.Focus();

            //}
        }

        private void Step4CardNo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void Step4CardNo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void Step4CardNo4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void Step4CardNo3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void btn_Info_Click(object sender, EventArgs e)
        {
            ImportExcel();
        }

        #region STORAGE
        class Model
        {
            public String IDX { get; set; }
            public String PayBankName { get; set; }
            public String PayAccountNo { get; set; }
            public String PayAmount { get; set; }
            public String PayInputName { get; set; }
            public String Remark { get; set; }
        }
        BindingList<Model> _CoreList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _CoreList;
        }
        #endregion

        #region UPDATE
        private void ImportExcel()
        {
           
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYBANKNEW);

            if (di.Exists == false)
            {
                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.MYBANKNEW;
            d.Filter = "Excel2003-2007 (*.xls)|*.xls|Excel통합문서 (*.xlsx)|*.xlsx";
            
            d.FilterIndex = 1;

            //if (cmb_Bank.Text == "신한은행")
            //{
            //    MessageBox.Show(cmb_Bank.Text + " 데이터 적용 준비중입니다.");

            //}

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
              
                FileName = d.FileName;
                SFileName = Path.GetFileName(FileName);


                switch (cmb_Bank.Text)
                {
                    case "국민은행":
                        // OLEDB를 이용한 엑셀 연결
                        szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=NO'";
                        //string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES;ImportMixedTypes=Text;IMEX=1'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        try
                        {
                            conn.Open();



                            var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                            sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME").Replace("'", "");

                            //거래완료일시 읽기
                            OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}{1}]", sheet1, "A2:A2"), conn);

                            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);

                            DataTable dTable = new DataTable();
                            adpt.Fill(dTable);

                            // dTable에 추출된 내용을 String으로 변환
                            foreach (DataRow row in dTable.Rows)
                            {
                                foreach (DataColumn Col in dTable.Columns)
                                {
                                    transperDate = row[Col].ToString().Substring(9, 10).Replace(".", "/");
                                }
                            }


                            conn.Close();



                        }
                        catch (Exception e)
                        {
                            conn.Close();

                            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.\r해당은행 양식과 틀립니다.", this.Text, MessageBoxButtons.OK);
                        }

                        szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        conn = new OleDbConnection(szConn);
                        try
                        {

                            conn.Open();

                            var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                            sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME").Replace("'", "");

                            // 엑셀로부터 데이타 읽기
                            OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}{1}]", sheet1, "A8:Q"), conn);



                            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                            DataSet ds = new DataSet();
                            adpt.Fill(ds);

                            DataTable dTable = new DataTable();
                            adpt.Fill(dTable);


                            newDGV2.DataSource = dTable;


                            conn.Close();

                            newDGV2.AutoResizeColumns();
                           

                            if (newDGV2.RowCount > 0)
                            {
                                btn_Next.Enabled = true;
                            }

                        }
                        catch (Exception e)
                        {
                            conn.Close();

                            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.\r해당은행 양식과 틀립니다.", this.Text, MessageBoxButtons.OK);
                        }
                        break;
                    case "우리은행":

                        szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        conn = new OleDbConnection(szConn);
                        try
                        {

                            conn.Open();

                            var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                            sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME").Replace("'", "");

                            // 엑셀로부터 데이타 읽기
                            OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}{1}]", sheet1, "A2:P"), conn);



                            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                            DataSet ds = new DataSet();
                            adpt.Fill(ds);

                            DataTable dTable = new DataTable();
                            adpt.Fill(dTable);


                            newDGV2.DataSource = dTable;


                            conn.Close();
                            newDGV2.AutoResizeColumns();
                            if (newDGV2.RowCount > 0)
                            {
                                btn_Next.Enabled = true;
                            }

                        }
                        catch (Exception e)
                        {
                            conn.Close();

                            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.\r해당은행 양식과 틀립니다.", this.Text, MessageBoxButtons.OK);
                        }
                        break;

                    case "하나은행":

                        szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        conn = new OleDbConnection(szConn);
                        try
                        {

                            conn.Open();

                            var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                            sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME").Replace("'", "");

                            // 엑셀로부터 데이타 읽기
                            OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}{1}]", sheet1, "A2:M"), conn);



                            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                            DataSet ds = new DataSet();
                            adpt.Fill(ds);

                            DataTable dTable = new DataTable();
                            adpt.Fill(dTable);


                            newDGV2.DataSource = dTable;


                            conn.Close();
                            newDGV2.AutoResizeColumns();
                            if (newDGV2.RowCount > 0)
                            {
                                btn_Next.Enabled = true;
                            }

                        }
                        catch (Exception e)
                        {
                            conn.Close();

                            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.\r해당은행 양식과 틀립니다.", this.Text, MessageBoxButtons.OK);
                        }
                        break;

                    case "기업은행":

                        szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=NO'";
                        conn = new OleDbConnection(szConn);
                        try
                        {

                            conn.Open();

                            var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                            sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME").Replace("'", "");

                            // 엑셀로부터 데이타 읽기
                            OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}{1}]", sheet1, "A1:K"), conn);



                            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                            DataSet ds = new DataSet();
                            adpt.Fill(ds);

                            DataTable dTable = new DataTable();
                            adpt.Fill(dTable);


                            newDGV2.DataSource = dTable;


                            conn.Close();
                            newDGV2.AutoResizeColumns();
                            if (newDGV2.RowCount > 0)
                            {
                                btn_Next.Enabled = true;
                            }

                        }
                        catch (Exception e)
                        {
                            conn.Close();

                            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.\r해당은행 양식과 틀립니다.", this.Text, MessageBoxButtons.OK);
                        }
                        break;
                    case "농협":

                        szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        conn = new OleDbConnection(szConn);
                        try
                        {

                            conn.Open();

                            var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                            sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME").Replace("'", "");

                            // 엑셀로부터 데이타 읽기
                            OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}{1}]", sheet1, "B2:K"), conn);



                            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                            DataSet ds = new DataSet();
                            adpt.Fill(ds);

                            DataTable dTable = new DataTable();
                            adpt.Fill(dTable);


                            newDGV2.DataSource = dTable;


                            conn.Close();
                            newDGV2.AutoResizeColumns();
                            if (newDGV2.RowCount > 0)
                            {
                                btn_Next.Enabled = true;
                            }

                        }
                        catch (Exception e)
                        {
                            conn.Close();

                            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.\r해당은행 양식과 틀립니다.", this.Text, MessageBoxButtons.OK);
                        }
                        break;

                    case "신한은행":

                        szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        conn = new OleDbConnection(szConn);
                        try
                        {

                            conn.Open();

                            var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                            sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME").Replace("'", "");

                            // 엑셀로부터 데이타 읽기
                            OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}{1}]", sheet1, "A1:S"), conn);



                            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                            DataSet ds = new DataSet();
                            adpt.Fill(ds);

                            DataTable dTable = new DataTable();
                            adpt.Fill(dTable);


                            newDGV2.DataSource = dTable;


                            conn.Close();
                            newDGV2.AutoResizeColumns();
                            if (newDGV2.RowCount > 0)
                            {
                                btn_Next.Enabled = true;
                            }

                        }
                        catch (Exception e)
                        {
                            conn.Close();

                            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.\r해당은행 양식과 틀립니다.", this.Text, MessageBoxButtons.OK);
                        }
                        break;

                }



               

            }


           
        }
      
       
        
        #endregion
    }

}
