using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_PayExcel : Form
    {
     
        string FileName = string.Empty;
     
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        string ERROR=string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        string SPayBankName = string.Empty;
        private List<UseListModel> LgdOidCompareTable = new List<UseListModel>();
        
        int RowErrorCount = 0;
        public EXCELINSERT_PayExcel()
        {
            InitializeComponent();
            InitializeStorage();
            InitClientTable();
            InitDriverTable();
        }

        DataSets.BaseDataSet  baseDataSet = new DataSets.BaseDataSet();
        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
          //  cmb_Savegubun.SelectedIndex = 0;

            accountListTableAdapter.Fill(useListDataSet.AccountList);
            //var driversTableAdapter = new DataSets.BaseDataSetTableAdapters.DriversTableAdapter();
            //driversTableAdapter.Fill(baseDataSet.Drivers);
          
        }


        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
        }

        class DriverViewModel
        {
            public int DriverId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string BizNo { get; set; }
            public int candidateid { get; set; }
        }
        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();
        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();

        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name, LoginId FROM Clients ";
                    //commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientTable.Add(
                              new ClientViewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid FROM Drivers ";
                    //commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _DriverTable.Add(
                              new DriverViewModel
                              {
                                  DriverId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                                  BizNo = dataReader.GetString(4),
                                  candidateid = dataReader.GetInt32(5),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private void btn_Info_Click(object sender, EventArgs e)
        {
            ImportExcel();


        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
           // DataTest();


        }

        private void btn_OK_Click(object sender, EventArgs e)
        {


            ExportExcel();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
           // InitCompareTable();
            UpdateDb();
        }

       

        //private void InitCompareTable()
        //{
        //    LgdOidCompareTable.Clear();
           
        //    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //    {

        //        connection.Open();
        //        using (SqlCommand _AccountListCommand = connection.CreateCommand())
        //        {
        //            _AccountListCommand.CommandText = "SELECT LGD_OID,PayState FROM AccountList ";
        //            using (SqlDataReader _Reader = _AccountListCommand.ExecuteReader())
        //            {
        //                while (_Reader.Read())
        //                {
        //                    LgdOidCompareTable.Add(new UseListModel
        //                    {
        //                        ULGD_OID = _Reader.GetStringN(0),
        //                        UPayState = _Reader.GetStringN(1),

        //                    });
        //                }
        //            }
        //        }
        //    }

        //    //CodeCompareTable.Sort();
        //}




        #region UPDATE
        private void ImportExcel()
        {
           // label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            d.Filter = "Excel통합문서 (*.xlsx)|*.xlsx";
            d.FilterIndex = 1;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = d.FileName;

                ExcelPackage _Excel = null;
                try
                {
                    _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
                }
                catch (Exception)
                {
                    MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                //if (_Excel.Workbook.Worksheets.Count < 1)
                //{
                //    MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                //    return;
                //}
                _CoreList.Clear();
                DataCount = 0;
                var _Sheet = _Excel.Workbook.Worksheets[1];
                const int TestIndex =12;
                int RowIndex = 3;
                string dateTimeSalesDate = "";
                while (true)
                {
                    var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;

                    string sDate = _Sheet.Cells[RowIndex, 2].Value.ToString();
                    double date;
                   
                    if (sDate != " ")
                    {
                        date = double.Parse(sDate);

                        dateTimeSalesDate = DateTime.FromOADate(date).ToString("yyyy-MM-dd").Replace("-", "/");
                    }
                    else
                    {
                        dateTimeSalesDate = "";

                    }
                    var Added = new Model
                    {
                        // S_Idx = _Sheet.Cells[RowIndex, 1].Text,
                        PayDate = _Sheet.Cells[RowIndex, 1].Text,

                     

                        SalesDate = dateTimeSalesDate,

                        
                       // SalesDate = "",
                        ShopID = _Sheet.Cells[RowIndex, 3].Text,
                        PayGubun = _Sheet.Cells[RowIndex, 4].Text,
                        JMoney = _Sheet.Cells[RowIndex, 5].Text,

                        Amount = _Sheet.Cells[RowIndex, 6].Text,
                        Comm = _Sheet.Cells[RowIndex, 7].Text,

                        VAT = _Sheet.Cells[RowIndex, 8].Text,
                        CommSum = _Sheet.Cells[RowIndex, 9].Text,

                        PaySum = _Sheet.Cells[RowIndex, 11].Text,



                    };
                    _CoreList.Add(Added);
                    RowIndex++;
                }
                label4.Text = _CoreList.Count().ToString();
               // label5.Text = "0";
               // label6.Text = "0";
            }
        }
        private void ExportExcel()
        {
            if (_CoreList.Count == 0)
            {
                MessageBox.Show("확인할 데이터가 없습니다.");
                return;
            }
            ExcelPackage _Excel = null;
            try
            {
                _Excel = new ExcelPackage(new System.IO.FileInfo(FileName));
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            //if (_Excel.Workbook.Worksheets.Count < 1)
            //{
            //    MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
            //    return;
            //}
            var _Sheet = _Excel.Workbook.Worksheets[1];
            int RowIndex = 3;
           // int ERROR_Index = 20;
            for (int i = 0; i < _CoreList.Count; i++)
            {
                var _Model = _CoreList[i];
               // _Sheet.Cells[RowIndex, ERROR_Index].Value = _Model.Error;
                RowIndex++;
            }
            try
            {
                _Excel.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            System.Diagnostics.Process.Start(FileName);
        }
        //private void DataTest()
        //{
        //    if (_CoreList.Count == 0)
        //    {
        //        MessageBox.Show("검증할 데이터가 없습니다.");
        //        return;

        //    }
        //    btn_Info.Enabled = false;
        //    btn_Test.Enabled = false;
        //    btn_OK.Enabled = false;
        //    cmb_Savegubun.Enabled = false;
        //    btn_Update.Enabled = false;
        //    btn_Close.Enabled = false;
        //    pnProgress.Visible = true;
        //    bar.Value = 0;
        //    bar.Maximum = _CoreList.Count;
        //    Task.Factory.StartNew(() => {
        //        InitCompareTable();
        //        FCount = 0;
        //        errCount = 0;
        //        int RowErrorCount = 0;
        //        foreach (Model row in _CoreList)
        //        {
        //            bar.Invoke(new Action(() => {
        //                bar.Value++;
        //            }));
        //            RowErrorCount = 0;
        //            String ErrorText = "";
                    
        //                FCount++;
        //                if (String.IsNullOrEmpty(row.S_Idx))
        //                {
        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += " | IDX 빈값";
        //                    }
        //                    else
        //                    {
        //                        ErrorText += "IDX 빈값";

        //                    }
        //                    RowErrorCount++;
        //                }
        //                //결제일자
        //                if (String.IsNullOrEmpty(row.PayDate))
        //                {
        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += "| 결제일자";
        //                    }
        //                    else
        //                    {
        //                        ErrorText = "결제일자";

        //                    }
        //                    RowErrorCount++;

        //                }
                       

        //                //상점아이디
        //                if (String.IsNullOrEmpty(row.LGD_MID))
        //                {
        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += "| 상점ID";
        //                    }
        //                    else
        //                    {
        //                        ErrorText = "상점ID";

        //                    }
        //                    RowErrorCount++;

        //                }
                        
        //                //결제상태
        //                if (String.IsNullOrEmpty(row.PayState))
        //                {
        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += " | 결제상태";
        //                    }
        //                    else
        //                    {
        //                        ErrorText += "결제상태";

        //                    }
        //                    RowErrorCount++;
        //                }
                       


        //                //금액
        //                if (String.IsNullOrEmpty(row.Amount))
        //                {

        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += "| 금액";
        //                    }
        //                    else
        //                    {
        //                        ErrorText = "금액";

        //                    }
        //                    RowErrorCount++;
        //                }
                       

        //                //수수료
        //                if (String.IsNullOrEmpty(row.VAT))
        //                {

        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += "| 수수료";
        //                    }
        //                    else
        //                    {
        //                        ErrorText = "수수료";

        //                    }
        //                    RowErrorCount++;
        //                }

        //                //지급액
        //                if (String.IsNullOrEmpty(row.PayAmount))
        //                {

        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += "| 지급액";
        //                    }
        //                    else
        //                    {
        //                        ErrorText = "지급액";

        //                    }
        //                    RowErrorCount++;
        //                }

        //                //승인번호
        //                if (String.IsNullOrEmpty(row.ApproveNum))
        //                {

        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += "| 승인번호";
        //                    }
        //                    else
        //                    {
        //                        ErrorText = "승인번호";

        //                    }
        //                    RowErrorCount++;
        //                }


        //                //주문번호
        //                if (String.IsNullOrEmpty(row.LGD_OID))
        //                {

        //                    if (RowErrorCount > 0)
        //                    {
        //                        ErrorText += "| 주문번호";
        //                    }
        //                    else
        //                    {
        //                        ErrorText = "주문번호";

        //                    }
        //                    RowErrorCount++;
        //                }
        //                else
        //                {



        //                    if (LgdOidCompareTable.Any(c => c.ULGD_OID == row.LGD_OID && c.UPayState == row.PayState))
        //                    {
        //                        if (RowErrorCount > 0)
        //                        {
        //                            ErrorText += " | 주문번호 중복";
        //                        }
        //                        else
        //                        {
        //                            ErrorText += "주문번호 중복";

        //                        }
        //                        RowErrorCount++;
        //                    }




        //                }



                   
        //            //if (RowErrorCount > 0)
        //            //{
        //            //    errCount++;
        //            //}

        //            //row.Error = ErrorText;
        //        }
        //        DataCount = FCount - errCount;
        //        Invoke(new Action(() =>
        //        {
        //            if (errCount == 0)
        //            {
        //                label4.Text = FCount.ToString() + " 건";
        //                label5.Text = (FCount - errCount).ToString() + " 건";
        //                label6.Text = errCount.ToString() + " 건";

        //                label7.Visible = false;
        //                btn_Update.Enabled = true;
        //            }
        //            else
        //            {

        //                label4.Text = FCount.ToString() + " 건";
        //                label5.Text = (FCount - errCount).ToString() + " 건";
        //                label6.Text = errCount.ToString() + " 건";

        //                label7.Visible = true;
        //            }
        //            btn_Info.Enabled = true;
        //            btn_Test.Enabled = true;
        //            btn_OK.Enabled = true;
        //            cmb_Savegubun.Enabled = true;
        //            if (cmb_Savegubun.SelectedIndex == 0)
        //            {
        //                btn_Update.Enabled = true;
        //            }
        //            else
        //            {
        //                btn_Update.Enabled = false;
        //            }
        //            btn_Close.Enabled = true;
        //            pnProgress.Visible = false;
        //        }));
        //    });
        //}
        private void UpdateDb()
        {
            var InsertQuery =
                @"INSERT INTO PayExcel (PayDate, SalesDate, ShopID, DriverName, DriverBizNo, PayGubun, JMoney, Amount, Comm, Vat, CommSum, PaySum, ClientCode, ClientName)
                VALUES (@PayDate, @SalesDate, @ShopID, @DriverName, @DriverBizNo, @PayGubun, @JMoney, @Amount, @Comm, @Vat, @CommSum, @PaySum, @ClientCode, @ClientName )";

    //        var InsertQuery =
    //@"INSERT INTO AccountList (PayDate ,ClientName,ClientId,ClientCode,DriverName,DriverId,LGD_MID,PayState,Amount,VAT,PayAmount,ApproveNum,LGD_OID,CreateDate)
    //            VALUES (@PayDate ,@ClientName, @ClientId, @ClientCode, @DriverName,@DriverId, @LGD_MID, @PayState, @Amount, @VAT,@PayAmount, @ApproveNum, @LGD_OID,getdate() )";


            if (newDGV1.RowCount == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = InsertQuery;
                    
                    _Command.Parameters.Add("@PayDate", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@SalesDate", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ShopID", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@DriverName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@DriverBizNo", SqlDbType.NVarChar);


                    _Command.Parameters.Add("@PayGubun", SqlDbType.NVarChar);
                   

                   
                    _Command.Parameters.Add("@JMoney", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Amount", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Comm", SqlDbType.Decimal);
                    _Command.Parameters.Add("@VAT", SqlDbType.Decimal);
                  
                    _Command.Parameters.Add("@CommSum", SqlDbType.Decimal);
                    _Command.Parameters.Add("@PaySum", SqlDbType.Decimal);

                    _Command.Parameters.Add("@ClientCode", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ClientName", SqlDbType.NVarChar);






                    foreach (Model row in _CoreList)
                    {



                        _Command.Parameters["@PayDate"].Value = row.PayDate;
                        _Command.Parameters["@SalesDate"].Value = row.SalesDate;


                        //var sClientcode = row.LGD_OID.Split('-')[0];

                        //_Command.Parameters["@ClientCode"].Value = sClientcode;

                    

                        _Command.Parameters["@ShopID"].Value = row.ShopID;
                        var Driver = _DriverTable.Where(c => c.LoginId == row.ShopID);


                        if (Driver.Any())
                        {
                            _Command.Parameters["@DriverName"].Value = Driver.First().Name;
                            _Command.Parameters["@DriverBizNo"].Value = Driver.First().BizNo;
                        }
                        else
                        {
                            _Command.Parameters["@DriverName"].Value = "";
                            _Command.Parameters["@DriverBizNo"].Value = "";

                        }

                        var SClientid = _DriverTable.Where(c => c.LoginId == row.ShopID).First().candidateid;

                        var Client = _ClientTable.Where(c => c.ClientId == Convert.ToInt32(SClientid));

                        if (Client.Any())
                        {

                            _Command.Parameters["@ClientName"].Value = Client.First().Name;
                            _Command.Parameters["@ClientCode"].Value = Client.First().Code;
                        }
                        else
                        {
                            _Command.Parameters["@ClientName"].Value = "";
                            _Command.Parameters["@ClientCode"].Value = DBNull.Value;

                        }


                        _Command.Parameters["@PayGubun"].Value = row.PayGubun;

                        _Command.Parameters["@JMoney"].Value = Convert.ToDecimal(row.JMoney);
                        _Command.Parameters["@Amount"].Value = Convert.ToDecimal(row.Amount);
                        _Command.Parameters["@Comm"].Value = Convert.ToDecimal(row.Comm);
                        _Command.Parameters["@VAT"].Value = Convert.ToDecimal(row.VAT);
                        _Command.Parameters["@CommSum"].Value = Convert.ToDecimal(row.CommSum);
                        _Command.Parameters["@PaySum"].Value = Convert.ToDecimal(row.PaySum);

                        _Command.ExecuteNonQuery();

                    }
                }
                _Connection.Close();
            }

            EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label4.Text.Replace("건", ""));
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
            this.Close();
        }
        #endregion

        #region STORAGE
        class Model : INotifyPropertyChanged
        {
          //  private String _S_Idx = "";

            private String _PayDate = "";

            private String _SalesDate ;
            private String _ShopID ;
            private String _PayGubun = "";
            private String _JMoney = "";

           // private int _DriverId;
            private String _Amount = "";
            private String _Comm = "";
            private String _VAT = "" ;
            private String _CommSum ;
            private String _PaySum = "";
         

            private String _Error = "";

           
            public string PayDate
            {
                get
                {
                    return _PayDate;
                }

                set
                {
                    SetField(ref _PayDate, value);
                }
            }
            public String SalesDate
            {
                get
                {
                    return _SalesDate;
                }

                set
                {
                    SetField(ref _SalesDate, value);
                }
            }
            public string ShopID
            {
                get
                {
                    return _ShopID;
                }

                set
                {
                    SetField(ref _ShopID, value);
                }
            }
            public string PayGubun
            {
                get
                {
                    return _PayGubun;
                }

                set
                {
                    SetField(ref _PayGubun, value);
                }
            }

            public string JMoney
            {
                get
                {
                    return _JMoney;
                }

                set
                {
                    SetField(ref _JMoney, value);
                }
            }

          

            public string Amount
            {
                get
                {
                    return _Amount;
                }

                set
                {
                    SetField(ref _Amount, value);
                }
            }

            public string Comm
            {
                get
                {
                    return _Comm;
                }

                set
                {
                    SetField(ref _Comm, value);
                }
            }
            public string VAT
            {
                get
                {
                    return _VAT;
                }

                set
                {
                    SetField(ref _VAT, value);
                }
            }

            public string CommSum
            {
                get
                {
                    return _CommSum;
                }

                set
                {
                    SetField(ref _CommSum, value);
                }
            }

            public string PaySum
            {
                get
                {
                    return _PaySum;
                }

                set
                {
                    SetField(ref _PaySum, value);
                }
            }

           

          

            public string Error
            {
                get
                {
                    return _Error;
                }

                set
                {
                    SetField(ref _Error, value);
                }
            }

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

        private class UseListModel
        {
            public string UPayState { get; set; }

            public string ULGD_OID { get; set; }

        }
        BindingList<Model> _CoreList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _CoreList;
        }
        #endregion
    }
}
