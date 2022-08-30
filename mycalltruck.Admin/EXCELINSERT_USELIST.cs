using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
//using OfficeOpenXml;
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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_USELIST : Form
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
        public EXCELINSERT_USELIST()
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
            cmb_Savegubun.SelectedIndex = 0;

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

            public string CarYear { get; set; }
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
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid,CarYear FROM Drivers WHERE ServiceState = 1";
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
                                  CarYear = dataReader.GetString(6),
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
            DataTest();


        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

            //if (newDGV1.Rows.Count == 0)
            //{
            //    MessageBox.Show("확인할 데이터가 없습니다.");
            //    return;
            //}
            //System.Diagnostics.Process.Start(FileName);

            //newDGV1.DataSource = "";
            //label4.Text = "0";
            //label5.Text = "0";
            //label6.Text = "0";

            ExportExcel();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            InitCompareTable();
            UpdateDb();
        }

        private void cmb_Savegubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Savegubun.SelectedIndex == 0)
            {
                btn_Update.Enabled = true;
            }
            else
            {
                btn_Update.Enabled = false;
            }
        }

        private void InitCompareTable()
        {
            LgdOidCompareTable.Clear();
           
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {

                connection.Open();
                using (SqlCommand _AccountListCommand = connection.CreateCommand())
                {
                    _AccountListCommand.CommandText = "SELECT LGD_OID,PayState FROM AccountList ";
                    using (SqlDataReader _Reader = _AccountListCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            LgdOidCompareTable.Add(new UseListModel
                            {
                                ULGD_OID = _Reader.GetStringN(0),
                                UPayState = _Reader.GetStringN(1),

                            });
                        }
                    }
                }
            }

            //CodeCompareTable.Sort();
        }




        #region UPDATE
        //private void ImportExcel()
        //{
        //    label7.Visible = false;
        //    OpenFileDialog d = new OpenFileDialog();
        //    DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.TAX);

        //    if (di.Exists == false)
        //    {

        //        di.Create();
        //    }
        //    d.InitialDirectory = LocalUser.Instance.PersonalOption.TAX;
        //    d.Filter = "Excel통합문서 (*.xlsx)|*.xlsx";
        //    d.FilterIndex = 1;
        //    if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        FileName = d.FileName;

        //        ExcelPackage _Excel = null;
        //        try
        //        {
        //            _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
        //            return;
        //        }
        //        //if (_Excel.Workbook.Worksheets.Count < 1)
        //        //{
        //        //    MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
        //        //    return;
        //        //}
        //        _CoreList.Clear();
        //        DataCount = 0;
        //        var _Sheet = _Excel.Workbook.Worksheets[1];
        //        const int TestIndex = 3;
        //        int RowIndex = 3;
        //        while (true)
        //        {
        //            var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
        //            if (String.IsNullOrEmpty(TestText))
        //                break;

        //            var Added = new Model
        //            {
        //                S_Idx = _Sheet.Cells[RowIndex, 1].Text,
        //                PayDate = _Sheet.Cells[RowIndex, 9].Text,
        //                LastDate = _Sheet.Cells[RowIndex, 2].Text,

        //                LGD_MID = _Sheet.Cells[RowIndex, 4].Text,
        //                PayState = _Sheet.Cells[RowIndex, 6].Text,
        //                Amount = _Sheet.Cells[RowIndex, 11].Text,
        //                VAT = _Sheet.Cells[RowIndex, 12].Text,
        //                PayAmount = _Sheet.Cells[RowIndex, 13].Text,
        //                ApproveNum = _Sheet.Cells[RowIndex, 14].Text,
        //                LGD_OID = _Sheet.Cells[RowIndex, 10].Text,



        //            };
        //            _CoreList.Add(Added);
        //            RowIndex++;
        //        }
        //        label4.Text = "0";
        //        label5.Text = "0";
        //        label6.Text = "0";
        //    }
        //}
        private void ImportExcel()
        {
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.TAX);

            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.TAX;
            d.Filter = "Excel2003-2007 (*.xls)|*.xls";
            d.FilterIndex = 1;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = d.FileName;
                _CoreList.Clear();
                DataCount = 0;


                String Sheet_name;
                int RowIndex = 2;
                const int TestIndex = 3;

                //ExcelPackage _Excel = null;
                try
                {
                   
                    if (Path.GetExtension(FileName) == ".xls")
                    {
                        HSSFWorkbook wb;
                        HSSFSheet sh;

                        // _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));

                        using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                        {
                            wb = new HSSFWorkbook(fs);

                            Sheet_name = wb.GetSheetAt(0).SheetName;  //get first sheet name
                        }


                        _CoreList.Clear();
                        DataCount = 0;
                        sh = (HSSFSheet)wb.GetSheet(Sheet_name);


                        while (true)
                        {
                            var TestText = sh.GetRow(RowIndex).GetCell(TestIndex).StringCellValue;

                            if (String.IsNullOrEmpty(TestText))
                                break;

                            DataFormatter formatter = new DataFormatter();

                            var Added = new Model
                            {

                                S_Idx = formatter.FormatCellValue(sh.GetRow(RowIndex).GetCell(0)),
                                PayDate = sh.GetRow(RowIndex).GetCell(8).StringCellValue,
                                LastDate = sh.GetRow(RowIndex).GetCell(1).StringCellValue,

                                LGD_MID = sh.GetRow(RowIndex).GetCell(3).StringCellValue,
                                PayState = sh.GetRow(RowIndex).GetCell(5).StringCellValue,
                                Amount = sh.GetRow(RowIndex).GetCell(10).NumericCellValue.ToString(),
                                VAT = sh.GetRow(RowIndex).GetCell(11).NumericCellValue.ToString(),
                                PayAmount = sh.GetRow(RowIndex).GetCell(12).NumericCellValue.ToString(),
                                ApproveNum = sh.GetRow(RowIndex).GetCell(13).StringCellValue,
                                LGD_OID = sh.GetRow(RowIndex).GetCell(9).StringCellValue,



                            };
                            _CoreList.Add(Added);
                            RowIndex++;
                        }

                    }
                    //else if (Path.GetExtension(FileName) == ".xlsx")
                    //{
                    //    XSSFWorkbook wb;
                    //    XSSFSheet sh;

                    //    // _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));

                    //    using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    //    {
                    //        wb = new XSSFWorkbook(fs);

                    //        Sheet_name = wb.GetSheetAt(0).SheetName;  //get first sheet name
                    //    }


                    //    _CoreList.Clear();
                    //    DataCount = 0;
                    //    sh = (XSSFSheet)wb.GetSheet(Sheet_name);


                    //    while (true)
                    //    {
                    //        var TestText = sh.GetRow(RowIndex).GetCell(TestIndex).StringCellValue;

                    //        if (String.IsNullOrEmpty(TestText))
                    //            break;

                    //        DataFormatter formatter = new DataFormatter();

                    //        var Added = new Model
                    //        {

                    //            S_Idx = formatter.FormatCellValue(sh.GetRow(RowIndex).GetCell(0)),
                    //            PayDate = sh.GetRow(RowIndex).GetCell(8).StringCellValue,
                    //            LastDate = sh.GetRow(RowIndex).GetCell(1).StringCellValue,

                    //            LGD_MID = sh.GetRow(RowIndex).GetCell(3).StringCellValue,
                    //            PayState = sh.GetRow(RowIndex).GetCell(5).StringCellValue,
                    //            Amount = sh.GetRow(RowIndex).GetCell(10).NumericCellValue.ToString(),
                    //            VAT = sh.GetRow(RowIndex).GetCell(11).NumericCellValue.ToString(),
                    //            PayAmount = sh.GetRow(RowIndex).GetCell(12).NumericCellValue.ToString(),
                    //            ApproveNum = sh.GetRow(RowIndex).GetCell(13).StringCellValue,
                    //            LGD_OID = sh.GetRow(RowIndex).GetCell(9).StringCellValue,



                    //        };
                    //        _CoreList.Add(Added);
                    //        RowIndex++;
                    //    }
                    //}
                    label4.Text = "0";
                    label5.Text = "0";
                    label6.Text = "0";


                }
                catch (Exception ex)
                {
                    
                     MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }

               // _CoreList.Clear();
               // DataCount = 0;
                // var _Sheet = _Excel.Workbook.Worksheets[1];
                //const int TestIndex = 3;
                //int RowIndex = 3;
                //sh = (HSSFSheet)wb.GetSheet(Sheet_name);
                //while (true)
                //{
                //    var TestText = sh.GetRow(RowIndex).GetCell(TestIndex);
                //    if (TestText == null)
                //        break;

                //}
                //while (true)
                //{
                //    var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
                //    if (String.IsNullOrEmpty(TestText))
                //        break;

                //    var Added = new Model
                //    {
                //        S_Idx = _Sheet.Cells[RowIndex, 1].Text,
                //        PayDate = _Sheet.Cells[RowIndex, 9].Text,
                //        LastDate = _Sheet.Cells[RowIndex, 2].Text,

                //        LGD_MID = _Sheet.Cells[RowIndex, 4].Text,
                //        PayState = _Sheet.Cells[RowIndex, 6].Text,
                //        Amount = _Sheet.Cells[RowIndex, 11].Text,
                //        VAT = _Sheet.Cells[RowIndex, 12].Text,
                //        PayAmount = _Sheet.Cells[RowIndex, 13].Text,
                //        ApproveNum = _Sheet.Cells[RowIndex, 14].Text,
                //        LGD_OID = _Sheet.Cells[RowIndex, 10].Text,



                //    };
                //    _CoreList.Add(Added);
                //    RowIndex++;
                //}
                label4.Text = "0";
                label5.Text = "0";
                label6.Text = "0";
            }
        }
        private void ExportExcel()
        {
            if (_CoreList.Count == 0)
            {
                MessageBox.Show("확인할 데이터가 없습니다.");
                return;
            }
            //ExcelPackage _Excel = null;

           
            String Sheet_name;
            try
            {
                if (Path.GetExtension(FileName) == ".xls")
                {
                    HSSFWorkbook wb;
                    HSSFSheet sh;
                    using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        wb = new HSSFWorkbook(fs);
                       // Sheet_name = wb.GetSheetAt(0).SheetName;
                    }

                    sh = (HSSFSheet)wb.GetSheetAt(0);
                  

                    int RowIndex = 2;
                    int ERROR_Index = 19;
                    sh.GetRow(1).CreateCell(19);
                    sh.GetRow(1).GetCell(19).CellStyle.FillBackgroundColor = IndexedColors.Grey25Percent.Index;
                    sh.GetRow(1).GetCell(ERROR_Index).SetCellValue("Error");
                    for (int i = 0; i < _CoreList.Count; i++)
                    {
                        var _Model = _CoreList[i];
                        sh.GetRow(RowIndex).CreateCell(19);
                        sh.GetRow(RowIndex).GetCell(ERROR_Index).SetCellValue(_Model.Error);

                        RowIndex++;
                    }

                    using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        wb.Write(fs);
                        fs.Close();
                    }


                }

                //else if (Path.GetExtension(FileName) == ".xlsx")
                //{
                //    XSSFWorkbook wb;
                //    XSSFSheet sh;
                //    using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                //    {
                //        wb = new XSSFWorkbook(fs);
                //        // Sheet_name = wb.GetSheetAt(0).SheetName;
                //    }

                //    sh = (XSSFSheet)wb.GetSheetAt(0);


                //    int RowIndex = 2;
                //    int ERROR_Index = 19;
                //    sh.GetRow(1).CreateCell(19);
                //    sh.GetRow(1).GetCell(19).CellStyle.FillBackgroundColor = IndexedColors.Grey25Percent.Index;
                //    sh.GetRow(1).GetCell(ERROR_Index).SetCellValue("Error");
                //    for (int i = 0; i < _CoreList.Count; i++)
                //    {
                //        var _Model = _CoreList[i];
                //        sh.GetRow(RowIndex).CreateCell(19);
                //        sh.GetRow(RowIndex).GetCell(ERROR_Index).SetCellValue(_Model.Error);

                //        RowIndex++;
                //    }

                //    using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                //    {
                //        wb.Write(fs);
                //        fs.Close();
                //    }


                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }



            //try
            //{
            //   // _Excel.Save();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
            //    return;
            //}
            System.Diagnostics.Process.Start(FileName);
        }
        private void DataTest()
        {
            if (_CoreList.Count == 0)
            {
                MessageBox.Show("검증할 데이터가 없습니다.");
                return;

            }
            btn_Info.Enabled = false;
            btn_Test.Enabled = false;
            btn_OK.Enabled = false;
            cmb_Savegubun.Enabled = false;
            btn_Update.Enabled = false;
            btn_Close.Enabled = false;
            pnProgress.Visible = true;
            bar.Value = 0;
            bar.Maximum = _CoreList.Count;
            Task.Factory.StartNew(() => {
                InitCompareTable();
                FCount = 0;
                errCount = 0;
                int RowErrorCount = 0;
                foreach (Model row in _CoreList)
                {
                    bar.Invoke(new Action(() => {
                        bar.Value++;
                    }));
                    RowErrorCount = 0;
                    String ErrorText = "";
                    if (row.S_Idx != "")
                    {
                        FCount++;
                        if (String.IsNullOrEmpty(row.S_Idx))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | IDX 빈값";
                            }
                            else
                            {
                                ErrorText += "IDX 빈값";

                            }
                            RowErrorCount++;
                        }
                        //결제일자
                        if (String.IsNullOrEmpty(row.PayDate))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 결제일자";
                            }
                            else
                            {
                                ErrorText = "결제일자";

                            }
                            RowErrorCount++;

                        }
                       

                        //상점아이디
                        if (String.IsNullOrEmpty(row.LGD_MID))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 상점ID";
                            }
                            else
                            {
                                ErrorText = "상점ID";

                            }
                            RowErrorCount++;

                        }
                        
                        //결제상태
                        if (String.IsNullOrEmpty(row.PayState))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 결제상태";
                            }
                            else
                            {
                                ErrorText += "결제상태";

                            }
                            RowErrorCount++;
                        }
                       


                        //금액
                        if (String.IsNullOrEmpty(row.Amount))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 금액";
                            }
                            else
                            {
                                ErrorText = "금액";

                            }
                            RowErrorCount++;
                        }
                       

                        //수수료
                        if (String.IsNullOrEmpty(row.VAT))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 수수료";
                            }
                            else
                            {
                                ErrorText = "수수료";

                            }
                            RowErrorCount++;
                        }

                        //지급액
                        if (String.IsNullOrEmpty(row.PayAmount))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 지급액";
                            }
                            else
                            {
                                ErrorText = "지급액";

                            }
                            RowErrorCount++;
                        }

                        //승인번호
                        if (String.IsNullOrEmpty(row.ApproveNum))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 승인번호";
                            }
                            else
                            {
                                ErrorText = "승인번호";

                            }
                            RowErrorCount++;
                        }


                        //주문번호
                        if (String.IsNullOrEmpty(row.LGD_OID))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 주문번호";
                            }
                            else
                            {
                                ErrorText = "주문번호";

                            }
                            RowErrorCount++;
                        }
                        else
                        {



                            if (LgdOidCompareTable.Any(c => c.ULGD_OID == row.LGD_OID && c.UPayState == row.PayState))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 주문번호 중복";
                                }
                                else
                                {
                                    ErrorText += "주문번호 중복";

                                }
                                RowErrorCount++;
                            }




                        }



                    }
                    if (RowErrorCount > 0)
                    {
                        errCount++;
                    }

                    row.Error = ErrorText;
                }
                DataCount = FCount - errCount;
                Invoke(new Action(() =>
                {
                    if (errCount == 0)
                    {
                        label4.Text = FCount.ToString() + " 건";
                        label5.Text = (FCount - errCount).ToString() + " 건";
                        label6.Text = errCount.ToString() + " 건";

                        label7.Visible = false;
                        btn_Update.Enabled = true;
                    }
                    else
                    {

                        label4.Text = FCount.ToString() + " 건";
                        label5.Text = (FCount - errCount).ToString() + " 건";
                        label6.Text = errCount.ToString() + " 건";

                        label7.Visible = true;
                    }
                    btn_Info.Enabled = true;
                    btn_Test.Enabled = true;
                    btn_OK.Enabled = true;
                    cmb_Savegubun.Enabled = true;
                    if (cmb_Savegubun.SelectedIndex == 0)
                    {
                        btn_Update.Enabled = true;
                    }
                    else
                    {
                        btn_Update.Enabled = false;
                    }
                    btn_Close.Enabled = true;
                    pnProgress.Visible = false;
                }));
            });
        }
        private void UpdateDb()
        {
            var InsertQuery =
                @"INSERT INTO AccountList (PayDate ,ClientName,ClientId,ClientCode,DriverName,DriverId,LGD_MID,PayState,Amount,VAT,PayAmount,ApproveNum,LGD_OID,CreateDate, LastDate)
                VALUES (@PayDate ,@ClientName, @ClientId, @ClientCode, @DriverName,@DriverId, @LGD_MID, @PayState, @Amount, @VAT,@PayAmount, @ApproveNum, @LGD_OID,getdate(), @LastDate )";
            if (DataCount == 0)
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
                    _Command.Parameters.Add("@LastDate", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@LGD_OID", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ClientCode", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ClientName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);


                    _Command.Parameters.Add("@LGD_MID", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@DriverName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@DriverId", SqlDbType.Int);

                   
                    _Command.Parameters.Add("@PayState", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Amount", SqlDbType.Decimal);
                    _Command.Parameters.Add("@VAT", SqlDbType.Decimal);
                    _Command.Parameters.Add("@PayAmount", SqlDbType.Decimal);
                    _Command.Parameters.Add("@ApproveNum", SqlDbType.NVarChar);
                   
                   





                    foreach (Model row in _CoreList)
                    {
                        if (row.S_Idx != "" && row.Error == "")
                        {


                            _Command.Parameters["@PayDate"].Value = row.PayDate;
                            _Command.Parameters["@LastDate"].Value = row.LastDate;
                            _Command.Parameters["@LGD_OID"].Value = row.LGD_OID;


                            var sClientcode = row.LGD_OID.Split('-')[0];

                            _Command.Parameters["@ClientCode"].Value = sClientcode;

                            var Client = _ClientTable.Where(c => c.Code == sClientcode);

                            if (Client.Any())
                            {

                                _Command.Parameters["@ClientName"].Value = Client.First().Name; ;
                                _Command.Parameters["@ClientId"].Value = (int)Client.First().ClientId;
                            }
                            else
                            {
                                _Command.Parameters["@ClientName"].Value = "";
                                _Command.Parameters["@ClientId"].Value = DBNull.Value;

                            }

                            _Command.Parameters["@LGD_MID"].Value = row.LGD_MID;
                            var Driver = _DriverTable.Where(c => c.LoginId == row.LGD_MID);


                            if (Driver.Any())
                            {
                                _Command.Parameters["@DriverName"].Value = Driver.First().CarYear;
                                _Command.Parameters["@DriverId"].Value = (int)Driver.First().DriverId;
                            }
                            else
                            {
                                _Command.Parameters["@DriverName"].Value = "";
                                _Command.Parameters["@DriverId"].Value = DBNull.Value;

                            }
                            _Command.Parameters["@PayState"].Value = row.PayState;
                            _Command.Parameters["@Amount"].Value =Convert.ToDecimal(row.Amount);
                            _Command.Parameters["@VAT"].Value = Convert.ToDecimal(row.VAT);
                            _Command.Parameters["@PayAmount"].Value = Convert.ToDecimal(row.PayAmount);
                            _Command.Parameters["@ApproveNum"].Value = row.ApproveNum;

                            _Command.ExecuteNonQuery();
                        }
                    }
                }
                _Connection.Close();
            }

            EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
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
            private String _S_Idx = "";

            private String _PayDate = "";
            private String _LastDate = "";

            private String _ClientName = "";
            private int _ClientId ;
            private String _ClientCode = "";
            private String _DriverName = "";

            private int _DriverId;
            private String _LGD_MID = "";
            private String _PayState = "";
            private string _Amount ;
            private string _VAT ;
            private string _PayAmount ;
            private String _ApproveNum = "";
            private String _LGD_OID = "";

            private DateTime _CreateDate;

            private String _Error = "";

            public string S_Idx
            {
                get
                {
                    return _S_Idx;
                }

                set
                {
                    SetField(ref _S_Idx, value);
                }
            }
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
            public string LastDate
            {
                get
                {
                    return _LastDate;
                }

                set
                {
                    SetField(ref _LastDate, value);
                }
            }
            public string  ClientName
            {
                get
                {
                    return _ClientName;
                }

                set
                {
                    SetField(ref _ClientName, value);
                }
            }
            public int ClientId
            {
                get
                {
                    return _ClientId;
                }

                set
                {
                    SetField(ref _ClientId, value);
                }
            }
            public string ClientCode
            {
                get
                {
                    return _ClientCode;
                }

                set
                {
                    SetField(ref _ClientCode, value);
                }
            }

            public string DriverName
            {
                get
                {
                    return _DriverName;
                }

                set
                {
                    SetField(ref _DriverName, value);
                }
            }

            public int DriverId
            {
                get
                {
                    return _DriverId;
                }

                set
                {
                    SetField(ref _DriverId, value);
                }
            }


            public string LGD_MID
            {
                get
                {
                    return _LGD_MID;
                }

                set
                {
                    SetField(ref _LGD_MID, value);
                }
            }

            public string PayState
            {
                get
                {
                    return _PayState;
                }

                set
                {
                    SetField(ref _PayState, value);
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

            public string PayAmount
            {
                get
                {
                    return _PayAmount;
                }

                set
                {
                    SetField(ref _PayAmount, value);
                }
            }

            public string ApproveNum
            {
                get
                {
                    return _ApproveNum;
                }

                set
                {
                    SetField(ref _ApproveNum, value);
                }
            }

            public string LGD_OID
            {
                get
                {
                    return _LGD_OID;
                }

                set
                {
                    SetField(ref _LGD_OID, value);
                }
            }

            public DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    SetField(ref _CreateDate, value);
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
