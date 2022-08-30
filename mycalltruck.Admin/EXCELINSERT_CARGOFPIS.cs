using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OfficeOpenXml;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


using mycalltruck.Admin.Class.Extensions;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_CARGOFPIS : Form
    {
        string FileName = string.Empty;
        DateTime O_SCONT_FROM, O_SCONT_TO;
        Int64 O_Deposit = 0;
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        string ERROR=string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        string SPayBankName = string.Empty;
        string S_date = string.Empty;
        string E_date = string.Empty;
        private List<String> BizNoCompareTable = new List<String>();
        private List<String> SangHoCompareTable = new List<String>();
        
        public EXCELINSERT_CARGOFPIS()
        {
            InitializeComponent();
            InitializeStorage();
        }
        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            customersTableAdapter.Fill(this.cMDataSet.Customers);
            cmb_Savegubun.SelectedIndex = 0;
           
            InitCompareTable();
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
            ExportExcel();
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btn_Update_Click(object sender, EventArgs e)
        {
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

        #region UPDATE
        private void ImportExcel()
        {
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.FPISTRU);

            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.FPISTRU;
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
                if (_Excel.Workbook.Worksheets.Count < 1)
                {
                    MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                _CoreList.Clear();
                DataCount = 0;
                var _Sheet = _Excel.Workbook.Worksheets[1];
                const int TestIndex = 2;
                int RowIndex = 2;
                while (true)
                {
                    var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;

                    var Added = new Model
                    {
                        S_Idx = _Sheet.Cells[RowIndex, 1].Text,
                        SCL_COMP_NM = _Sheet.Cells[RowIndex, 2].Text,
                        SCL_COMP_BSNS_NUM = _Sheet.Cells[RowIndex, 3].Text,
                        SCONT_FROM = _Sheet.Cells[RowIndex, 4].Text,
                        SCONT_TO = _Sheet.Cells[RowIndex, 5].Text,
                        SCONT_DEPOSIT = _Sheet.Cells[RowIndex, 6].Text,

                    };
                    _CoreList.Add(Added);
                    RowIndex++;
                }
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
            if (_Excel.Workbook.Worksheets.Count < 1)
            {
                MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            int RowIndex = 2;
            int ERROR_Index = 7;
            for (int i = 0; i < _CoreList.Count; i++)
            {
                var _Model = _CoreList[i];
                _Sheet.Cells[RowIndex, ERROR_Index].Value = _Model.Error;
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
               // InitCompareTable();
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
                        //사업자번호 없으면 에러
                        if (String.IsNullOrEmpty(row.SCL_COMP_BSNS_NUM) || Regex.IsMatch(row.SCL_COMP_BSNS_NUM, "^.d{10}$"))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 사업자번호";
                            }
                            else
                            {
                                ErrorText = "사업자번호";
                            }
                            RowErrorCount++;
                        }
                        else
                        {
                            if (!BizNoCompareTable.Contains(row.SCL_COMP_BSNS_NUM))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "| 미 등록된 사업자번호";
                                }
                                else
                                {
                                    ErrorText = "미 등록된 사업자번호";
                                }
                                RowErrorCount++;
                            }
                        }
                        //상호 없으면 에러
                        if (String.IsNullOrEmpty(row.SCL_COMP_NM))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 상호";
                            }
                            else
                            {
                                ErrorText += "상호";

                            }
                            RowErrorCount++;
                        }
                        

                        //핸드폰번호
                        if (String.IsNullOrEmpty(row.SCONT_FROM))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 계약시작일자";
                            }
                            else
                            {
                                ErrorText += "계약시작일자";

                            }
                            RowErrorCount++;
                        }
                        else
                        {
                           // string D_date = string.Empty;
                            //  string D_dateString = row.Cells[4].Value.ToString().Replace(".0", "");

                            if (row.SCONT_FROM.Length > 0)
                            {
                                if (row.SCONT_FROM.Length == 8)
                                {
                                    S_date = row.SCONT_FROM.Substring(0, 4) + "-" + row.SCONT_FROM.Substring(4, 2) + "-" + row.SCONT_FROM.Substring(6, 2);
                                }
                                else if (row.SCONT_FROM.Length == 10)
                                {
                                    S_date = row.SCONT_FROM.Substring(0, 4) + "-" + row.SCONT_FROM.Substring(5, 2) + "-" + row.SCONT_FROM.Substring(8, 2);
                                }
                            }



                            if (!DateTime.TryParse(S_date, out O_SCONT_FROM))
                            {
                               
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "  | 계약시작일자 ";
                                }
                                else
                                {
                                    ErrorText = " 계약시작일자";
                                }
                               
                                RowErrorCount++;



                                // return;
                            }
                        }
                        //입사일자
                        if (String.IsNullOrEmpty(row.SCONT_TO))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 계약종료일자";
                            }
                            else
                            {
                                ErrorText = "계약종료일자";

                            }
                            RowErrorCount++;
                        }
                        else
                        {
                           // string D_date = string.Empty;
                            //  string D_dateString = row.Cells[4].Value.ToString().Replace(".0", "");

                            if (row.SCONT_TO.Length > 0)
                            {
                                if (row.SCONT_TO.Length == 8)
                                {
                                    E_date = row.SCONT_TO.Substring(0, 4) + "-" + row.SCONT_TO.Substring(4, 2) + "-" + row.SCONT_TO.Substring(6, 2);
                                }
                                else if (row.SCONT_TO.Length == 10)
                                {
                                    E_date = row.SCONT_TO.Substring(0, 4) + "-" + row.SCONT_TO.Substring(5, 2) + "-" + row.SCONT_TO.Substring(8, 2);
                                }
                            }



                            if (!DateTime.TryParse(E_date, out O_SCONT_TO))
                            {
                                
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "| 계약종료일자 ";
                                }
                                else
                                {
                                    ErrorText = " 계약종료일자";
                                }
                                
                                RowErrorCount++;



                                // return;
                            }

                        }
                      
                        if (!ErrorText.Contains("계약시작일자") || !ErrorText.Contains("계약종료일자"))
                        {

                            if (O_SCONT_TO < O_SCONT_FROM)
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "| 계약기간은 시작일이 종료일 보다 나중일 수 없습니다. ";
                                }
                                else
                                {
                                    ErrorText = " 계약기간은 시작일이 종료일 보다 나중일 수 없습니다.";
                                }

                                RowErrorCount++;
                            }
          
                        }


                        //계약금액
                        if (String.IsNullOrEmpty(row.SCONT_DEPOSIT))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 계약금액";
                            }
                            else
                            {
                                ErrorText = "계약금액";

                            }
                            RowErrorCount++;
                        }

                        else
                        {

                            if (!Int64.TryParse(row.SCONT_DEPOSIT.Replace(",", ""), out O_Deposit))
                            {

                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "| 계약금액";
                                }
                                else
                                {
                                    ErrorText = "계약금액";

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
                @"INSERT INTO [dbo].[FPIS_CONT](CL_COMP_GUBUN, CL_COMP_NM, CL_COMP_CORP_NUM, CL_COMP_BSNS_NUM, CL_P_TEL, CONT_GROUP,  CONT_FROM, CONT_TO, CONT_ITEM, CONT_GOODS_FORM, CONT_GOODS_UNIT, CONT_GOODS_CNT, CONT_START_ADDR, CONT_END_ADDR, CONT_START_ADDR1, CONT_END_ADDR1, CONT_DEPOSIT, CONT_MANG_TYPE, CliendId, CREATE_DATE, CONT_YN, ONE_GUBUN,CustomerId)
                VALUES(@CL_COMP_GUBUN, @CL_COMP_NM, @CL_COMP_CORP_NUM, @CL_COMP_BSNS_NUM, @CL_P_TEL, @CONT_GROUP,  @CONT_FROM, @CONT_TO, @CONT_ITEM, @CONT_GOODS_FORM, @CONT_GOODS_UNIT, @CONT_GOODS_CNT, @CONT_START_ADDR, @CONT_END_ADDR, @CONT_START_ADDR1, @CONT_END_ADDR1, @CONT_DEPOSIT, @CONT_MANG_TYPE, @CliendId,getdate(), @CONT_YN, @ONE_GUBUN,@CustomerId)";
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
                    _Command.Parameters.Add("@CL_COMP_GUBUN", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CL_COMP_NM", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CL_COMP_CORP_NUM", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CL_COMP_BSNS_NUM", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CL_P_TEL", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_GROUP", SqlDbType.NVarChar);

                  //  _Command.Parameters.Add("@CONT_AGENCY_USR_MST_KEY", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_FROM", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_TO", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_ITEM", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_GOODS_FORM", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_GOODS_UNIT", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_GOODS_CNT", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_START_ADDR", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_END_ADDR", SqlDbType.NVarChar);


                    _Command.Parameters.Add("@CONT_START_ADDR1", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_END_ADDR1", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_DEPOSIT", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_MANG_TYPE", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CliendId", SqlDbType.NVarChar);
                  //  _Command.Parameters.Add("@CREATE_DATE", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CONT_YN", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ONE_GUBUN", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CustomerId", SqlDbType.Int);




                    foreach (Model row in _CoreList)
                    {
                        if (row.S_Idx != "" && row.Error == "")
                        {
                            string SCL_COMP_BSNS_NUM = row.SCL_COMP_BSNS_NUM;
                            _Command.Parameters["@CL_COMP_GUBUN"].Value = "1";
                            //   _Command.Parameters["@CL_COMP_NM"].Value = row.SCL_COMP_NM;

                            var Query = cMDataSet.Customers.Where(c => c.BizNo.Replace("-", "") == SCL_COMP_BSNS_NUM.Replace("-", "") && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();
                            if (Query.Any())
                            {
                                _Command.Parameters["@CL_COMP_NM"].Value = Query.First().SangHo;
                                _Command.Parameters["@CustomerId"].Value = Query.First().CustomerId;
                                var _S = Query.First().PhoneNo.Replace("-", "").Replace(" ", "");
                                if (_S.StartsWith("02"))
                                {
                                    if (_S.Length > 2)
                                    {
                                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                                    }
                                    if (_S.Length > 6)
                                    {
                                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                                    }
                                    if (_S.Length > 11)
                                    {
                                        _S = _S.Replace("-", "");
                                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                                    }
                                }
                                else
                                {
                                    if (_S.Length > 3)
                                    {
                                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                                    }
                                    if (_S.Length > 7)
                                    {
                                        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                                    }
                                    if (_S.Length > 12)
                                    {
                                        _S = _S.Replace("-", "");
                                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                                    }
                                }
                               // _Txt.Text = _S;
                            


                                _Command.Parameters["@CL_P_TEL"].Value = _S;
                            }
                            else
                            {
                                _Command.Parameters["@CL_COMP_NM"].Value = row.SCL_COMP_NM;
                                _Command.Parameters["@CL_P_TEL"].Value = "";
                            }
                            _Command.Parameters["@CL_COMP_CORP_NUM"].Value = "";

                            SCL_COMP_BSNS_NUM = row.SCL_COMP_BSNS_NUM.Replace("-", "");
                            SCL_COMP_BSNS_NUM = SCL_COMP_BSNS_NUM.Substring(0, 3) + "-" + SCL_COMP_BSNS_NUM.Substring(3, 2) + "-" + SCL_COMP_BSNS_NUM.Substring(5);


                            _Command.Parameters["@CL_COMP_BSNS_NUM"].Value = SCL_COMP_BSNS_NUM;
                            
                            _Command.Parameters["@CONT_GROUP"].Value = DateTime.Now.ToString("yyMMddhhmmss");

                            string SCONT_FROM = row.SCONT_FROM.ToString().Trim().Replace(".0", "");

                            string SCONT_TO = row.SCONT_TO.ToString().Trim().Replace(".0", "");
                            string SCONT_TO_C = SCONT_TO.Substring(0, 4) + "/" + SCONT_TO.Substring(4, 2) + "/" + SCONT_TO.Substring(6, 2);
                            string SCONT_FROM_C = SCONT_FROM.Substring(0, 4) + "/" + SCONT_FROM.Substring(4, 2) + "/" + SCONT_FROM.Substring(6, 2);

                            _Command.Parameters["@CONT_FROM"].Value = SCONT_FROM_C;
                            _Command.Parameters["@CONT_TO"].Value = SCONT_TO_C;
                            _Command.Parameters["@CONT_ITEM"].Value = "99";
                            _Command.Parameters["@CONT_GOODS_FORM"].Value = "99";
                            _Command.Parameters["@CONT_GOODS_UNIT"].Value = "99";
                            _Command.Parameters["@CONT_GOODS_CNT"].Value = "0";
                            _Command.Parameters["@CONT_START_ADDR"].Value = "";
                            _Command.Parameters["@CONT_END_ADDR"].Value = "";
                            _Command.Parameters["@CONT_START_ADDR1"].Value = "";
                            _Command.Parameters["@CONT_END_ADDR1"].Value = "";

                            _Command.Parameters["@CONT_DEPOSIT"].Value = row.SCONT_DEPOSIT.Replace(",","");

                            _Command.Parameters["@CONT_MANG_TYPE"].Value = "N";

                            _Command.Parameters["@CliendId"].Value = LocalUser.Instance.LogInInformation.LoginId;
                            _Command.Parameters["@CONT_YN"].Value = false;
                            _Command.Parameters["@ONE_GUBUN"].Value = "0";


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

            private String _SCL_COMP_NM = "";

            private String _SCL_COMP_BSNS_NUM = "";
            private String _SCONT_FROM = "";
            private String _SCONT_TO = "";
            private String _SCONT_DEPOSIT = "";
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
            public string SCL_COMP_NM
            {
                get
                {
                    return _SCL_COMP_NM;
                }

                set
                {
                    SetField(ref _SCL_COMP_NM, value);
                }
            }
            public string SCL_COMP_BSNS_NUM
            {
                get
                {
                    return _SCL_COMP_BSNS_NUM;
                }

                set
                {
                    SetField(ref _SCL_COMP_BSNS_NUM, value);
                }
            }
            public string  SCONT_FROM
            {
                get
                {
                    return _SCONT_FROM;
                }

                set
                {
                    SetField(ref _SCONT_FROM, value);
                }
            }
            public string SCONT_TO
            {
                get
                {
                    return _SCONT_TO;
                }

                set
                {
                    SetField(ref _SCONT_TO, value);
                }
            }
            public string SCONT_DEPOSIT
            {
                get
                {
                    return _SCONT_DEPOSIT;
                }

                set
                {
                    SetField(ref _SCONT_DEPOSIT, value);
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
        BindingList<Model> _CoreList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _CoreList;
        }
        private void InitCompareTable()
        {
            BizNoCompareTable.Clear();
            SangHoCompareTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (var _BizNoCommand = _Connection.CreateCommand())
                {
                    _BizNoCommand.CommandText = "SELECT BizNo,SangHo FROM Customers WHERE ClientId = @ClientId";
                    _BizNoCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _BizNoCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            BizNoCompareTable.Add(_Reader.GetString(0).Replace("-", ""));
                        }

                        while (_Reader.Read())
                        {
                            SangHoCompareTable.Add(_Reader.GetString(1).Replace("-", ""));
                        }
                    }
                }
                _Connection.Close();
            }
        }

        #endregion

    }
}
