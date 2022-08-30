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
    public partial class EXCELINSERT_NOTICEDRIVER : Form
    {
        bool Account_Result = false;
        string FileName = string.Empty;
        DateTime O_StopDate,O_RequestDate;
        Int64 O_Price = 0;
        Int64 O_Deposit = 0;
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        string ERROR=string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        string SPayBankName = string.Empty;
        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();
        int RowErrorCount = 0;
        public EXCELINSERT_NOTICEDRIVER()
        {
            InitializeComponent();
            InitializeStorage();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            cmb_Savegubun.SelectedIndex = 0;
            noticedriverTableAdapter.Fill(cMDataSet.NOTICEDRIVER);
            this.staticOptionsTableAdapter.Fill(cMDataSet.StaticOptions);
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
           // InitCompareTable();
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

        //private void InitCompareTable()
        //{
        //    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //    {
        //        var command = connection.CreateCommand();
        //        command.CommandText = "SELECT Replace(BizNo,'-',''), Code FROM Customers WHERE Clientid = " + LocalUser.Instance.LogInInfomation.ClientId + "";
        //        connection.Open();
        //        var dataReader = command.ExecuteReader( CommandBehavior.CloseConnection);
        //        while (dataReader.Read())
        //        {
        //            BizNoCompareTable.Add(dataReader.GetString(0));
        //            CodeCompareTable.Add(dataReader.GetString(1));
        //        }
        //    }
        //    CodeCompareTable.Sort();
        //}
       



        #region UPDATE
        private void ImportExcel()
        {
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.DRIVER;
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
                const int TestIndex = 1;
                int RowIndex = 2;
                while (true)
                {
                    var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;

                    var Added = new Model
                    {
                        S_Idx = _Sheet.Cells[RowIndex, 1].Text,
                        SState = _Sheet.Cells[RowIndex, 2].Text,
                        SCity = _Sheet.Cells[RowIndex, 3].Text,
                        SMobileNo = _Sheet.Cells[RowIndex, 4].Text,
                        SCarType = _Sheet.Cells[RowIndex, 5].Text,
                        SCarSize = _Sheet.Cells[RowIndex, 5].Text,


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
                        //시도 없으면 에러
                        if (String.IsNullOrEmpty(row.SState))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 시도";
                            }
                            else
                            {
                                ErrorText = "시도";

                            }
                            RowErrorCount++;

                        }
                        else
                        {

                            var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.State == row.SState).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "| 시도";
                                }
                                else
                                {
                                    ErrorText = "시도";

                                }
                                RowErrorCount++;
                            }


                        }

                        //시군구 없으면 에러
                        if (String.IsNullOrEmpty(row.SCity))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 시군구";
                            }
                            else
                            {
                                ErrorText = "시군구";

                            }
                            RowErrorCount++;

                        }
                        else
                        {

                            var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.State == row.SState && c.City == row.SCity).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "| 시군구";
                                }
                                else
                                {
                                    ErrorText = "시군구";

                                }
                                RowErrorCount++;
                            }

                        }
                        //핸드폰번호 없으면 에러
                        if (String.IsNullOrEmpty(row.SMobileNo))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 핸드폰번호";
                            }
                            else
                            {
                                ErrorText += "핸드폰번호";

                            }
                            RowErrorCount++;
                        }
                        else
                        {
                            Regex Mobileregex = new Regex(@"01[016789]\d{3,4}\d{4,4}");

                            if (Mobileregex.IsMatch(row.SMobileNo))
                            {
                                
                                var Query = cMDataSet.NOTICEDRIVER.Where(c => c.MobileNo.Replace("-","") == row.SMobileNo).ToArray();
                                if (Query.Any())
                                {
                                    if (RowErrorCount > 0)
                                    {
                                        ErrorText += " | 핸드폰번호중복";
                                    }
                                    else
                                    {
                                        ErrorText += " 핸드폰번호중복";

                                    }
                                    RowErrorCount++;
                                }
                                
                            }
                            else
                            {




                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 핸드폰번호";
                                }
                                else
                                {
                                    ErrorText += "핸드폰번호";

                                }
                                RowErrorCount++;
                            }
                        }



                        //차종
                        if (String.IsNullOrEmpty(row.SCarType))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 차종";
                            }
                            else
                            {
                                ErrorText = "차종";

                            }
                            RowErrorCount++;
                        }
                        else
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && c.Value == IntNull.Parse(row.SCarType)).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 차종";
                                }
                                else
                                {
                                    ErrorText += " 차종";

                                }
                                RowErrorCount++;
                            }

                        }

                        //톤수
                        if (String.IsNullOrEmpty(row.SCarSize))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += "| 톤수";
                            }
                            else
                            {
                                ErrorText = "톤수";

                            }
                            RowErrorCount++;
                        }
                        else
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value == IntNull.Parse(row.SCarSize)).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 톤수";
                                }
                                else
                                {
                                    ErrorText += " 톤수";

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
                @"INSERT INTO [dbo].[NOTICEDRIVER](State, City, MobileNo, CarType, CarSize, ClientId, CreateDate)
                VALUES(@State, @City, @MobileNo, @CarType, @CarSize, @ClientId, getdate())";
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
                    _Command.Parameters.Add("@State", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@City", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@MobileNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CarType", SqlDbType.Int);
                    _Command.Parameters.Add("@CarSize", SqlDbType.Int);
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);



                    foreach (Model row in _CoreList)
                    {
                        if (row.S_Idx != "" && row.Error == "")
                        {


                            _Command.Parameters["@State"].Value = row.SState;
                            _Command.Parameters["@City"].Value = row.SCity;

                            String _MobileNo = row.SMobileNo.Replace("-", "");
                            if (_MobileNo.Length == 10)
                            {
                                _MobileNo = _MobileNo.Substring(0, 3) + "-" + _MobileNo.Substring(3, 3) + "-" + _MobileNo.Substring(6);
                            }
                            else if (_MobileNo.Length == 11)
                            {
                                _MobileNo = _MobileNo.Substring(0, 3) + "-" + _MobileNo.Substring(3, 4) + "-" + _MobileNo.Substring(7);
                            }
                            _Command.Parameters["@MobileNo"].Value = _MobileNo;

                            _Command.Parameters["@CarType"].Value = row.SCarType;
                            _Command.Parameters["@CarSize"].Value = row.SCarSize;

                            _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;

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

            private String _SState = "";

            private String _SCity = "";
            private String _SMobileNo = "";
            private String _SCarType = "";
            private String _SCarSize = "";
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
            public string SState
            {
                get
                {
                    return _SState;
                }

                set
                {
                    SetField(ref _SState, value);
                }
            }
            public string SCity
            {
                get
                {
                    return _SCity;
                }

                set
                {
                    SetField(ref _SCity, value);
                }
            }
            public string SMobileNo
            {
                get
                {
                    return _SMobileNo;
                }

                set
                {
                    SetField(ref _SMobileNo, value);
                }
            }
            public string SCarType
            {
                get
                {
                    return _SCarType;
                }

                set
                {
                    SetField(ref _SCarType, value);
                }
            }

            public string SCarSize
            {
                get
                {
                    return _SCarSize;
                }

                set
                {
                    SetField(ref _SCarSize, value);
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
        #endregion
    }
}
