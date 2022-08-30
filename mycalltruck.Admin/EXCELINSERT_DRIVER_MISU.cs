using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_DRIVER_MISU : Form
    {
        string FileName = string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();
        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();
        public EXCELINSERT_DRIVER_MISU()
        {
            InitializeComponent();
            InitializeStorage();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            cmb_Savegubun.SelectedIndex = 0;
            this.staticOptionsTableAdapter.Fill(cMDataSet.StaticOptions);
          //  InitCompareTable();
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
            //InitCompareTable();
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
        //    CodeCompareTable.Clear();
        //    BizNoCompareTable.Clear();
        //    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //    {
        //        _Connection.Open();
        //        using (var _CodeCommand = _Connection.CreateCommand())
        //        using (var _BizNoCommand = _Connection.CreateCommand())
        //        {
        //            _CodeCommand.CommandText = "SELECT Code FROM Customers WHERE ClientId = @ClientId";
        //            _BizNoCommand.CommandText = "SELECT BizNo FROM Customers WHERE ClientId = @ClientId";
        //            _CodeCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
        //            _BizNoCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
        //            using (SqlDataReader _Reader = _CodeCommand.ExecuteReader())
        //            {
        //                while (_Reader.Read())
        //                {
        //                    CodeCompareTable.Add(_Reader.GetString(0));
        //                }
        //            }
        //            using (SqlDataReader _Reader = _BizNoCommand.ExecuteReader())
        //            {
        //                while (_Reader.Read())
        //                {
        //                    BizNoCompareTable.Add(_Reader.GetString(0).Replace("-",""));
        //                }
        //            }
        //        }
        //        _Connection.Close();
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
                        SCarNo = _Sheet.Cells[RowIndex, 1].Text,
                        SName = _Sheet.Cells[RowIndex, 2].Text,
                        //SBizNo = _Sheet.Cells[RowIndex, 3].Text,
                        SMisu = _Sheet.Cells[RowIndex, 3].Text.Replace(",",""),
                        SMizi = _Sheet.Cells[RowIndex, 4].Text.Replace(",", ""),

                    };
                    _CoreList.Add(Added);
                    RowIndex++;

                    if (RowIndex > 5000)
                    {
                        MessageBox.Show("한번에 5,000건 이상 불러올 수 없습니다.");
                        break;
                    }
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
            int ERROR_Index = 6;
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
                DriverRepository mDriverRepository = new DriverRepository();
                foreach (Model row in _CoreList)
                {
                    bar.Invoke(new Action(() => {
                        bar.Value++;
                    }));
                    RowErrorCount = 0;
                    String ErrorText = "";
                    if (row.SCarNo != "")
                    {
                        FCount++;

                        //상호 없으면 에러
                        if (String.IsNullOrEmpty(row.SCarNo))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 차량번호 빈값";
                            }
                            else
                            {
                                ErrorText += "차량번호 빈값";

                            }
                            RowErrorCount++;
                        }
                        else
                        {
                            var IsMyCar = mDriverRepository.IsMyCar(row.SCarNo);
                            if (!IsMyCar)
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록되지 않은 차량";
                                }
                                else
                                {
                                    ErrorText += "등록되지 않은 차량";

                                }
                                RowErrorCount++;


                            }
                            //var IsAnotherCar = mDriverRepository.IsAnotherCar(row.SCarNo);
                            //if (IsAnotherCar)
                            //{
                            //    continue;
                            //}

                        }



                        if (String.IsNullOrEmpty(row.SMisu))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 미수금 빈값 ";
                            }
                            else
                            {
                                ErrorText += "미수금 빈값";

                            }
                            RowErrorCount++;
                        }
                        else if(!String.IsNullOrEmpty(row.SMisu))
                        {

                            if (!Int64.TryParse(row.SMisu, out long _Smisu))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "  | 미수금 숫자입력 ";
                                }
                                else
                                {
                                    ErrorText = " 미수금 숫자입력";
                                }

                                RowErrorCount++;

                            }

                        }



                        if (String.IsNullOrEmpty(row.SMizi))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 미지급금 빈값 ";
                            }
                            else
                            {
                                ErrorText += "미지급금 빈값";

                            }
                            RowErrorCount++;
                        }
                        else if (!String.IsNullOrEmpty(row.SMizi))
                        {

                            if (!Int64.TryParse(row.SMizi, out long _SMizi))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += "  | 미지급금 숫자입력 ";
                                }
                                else
                                {
                                    ErrorText = " 미지급금 숫자입력";
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

            var UpdateQuery =
                " IF EXISTS (select * from DriverInstances where DriverId = @DriverId AND ClientId = @ClientId)" +
                " BEGIN " +
                " Update DriverInstances SET Misu = @Misu,Mizi = @Mizi,MStartDate = getdate(),MStart = @Mizi WHERE DriverId = @DriverId AND ClientId = @ClientId " +
                " END " +
                " ELSE " +
                " BEGIN " +
                " INSERT INTO DriverInstances(DriverId, ClientId, Misu, Mizi, MStartDate, Mstart)VALUES(@DriverId, @ClientId, @Misu, @Mizi, getdate(), @Mizi)" +
                " END ";

            //var UpdateQuery =
            //    $@"Update [Drivers] SET Misu = @Misu,Mizi = @Mizi,MStartDate = getdate(),MStart = @Mizi WHERE CarNo  = @CarNo AND ServiceState <> 5 ";
            if (DataCount == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }
           // InitCompareTable();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = UpdateQuery;

                   // _Command.Parameters.Add("@DriverInstanceId", SqlDbType.Int);
                    //_Command.Parameters.Add("@CarNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Misu", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Mizi", SqlDbType.Decimal);

                    _Command.Parameters.Add("@DriverId", SqlDbType.Int);
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                    

                    foreach (Model row in _CoreList)
                    {
                     
                        if (row.SCarNo != "" && row.Error == "")
                        {
                            DriverRepository mDriverRepository = new DriverRepository();

                          
                            var _DriverId = mDriverRepository.GetDriverId(row.SCarNo);
                            //var Query = ShareOrderDataSet.DriverInstances.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.DriverId == _DriverId).ToArray();
                            
                           
                           // _Command.Parameters["@DriverInstanceId"].Value = Query.First().DriverInstanceId;
                            _Command.Parameters["@DriverId"].Value = _DriverId;
                            _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;

                            decimal _Misu = Convert.ToDecimal(row.SMisu);
                            decimal _Mizi = Convert.ToDecimal(row.SMizi);
                            _Command.Parameters["@Misu"].Value = _Misu;
                            _Command.Parameters["@Mizi"].Value = _Mizi;
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

            private String _SCarNo = "";
            //private String _SBizNo = "";
            private String _SName = "";
           
            private String _SMizi = "";
            private String _SMisu = "";
            private String _Error = "";

         
            public string SCarNo
            {
                get
                {
                    return _SCarNo;
                }

                set
                {
                    SetField(ref _SCarNo, value);
                }
            }
            //public string SBizNo
            //{
            //    get
            //    {
            //        return _SBizNo;
            //    }

            //    set
            //    {
            //        SetField(ref _SBizNo, value);
            //    }
            //}
            public string SName
            {
                get
                {
                    return _SName;
                }

                set
                {
                    SetField(ref _SName, value);
                }
            }
         
            public string SMizi
            {
                get
                {
                    return _SMizi;
                }

                set
                {
                    SetField(ref _SMizi, value);
                }
            }
            public string SMisu
            {
                get
                {
                    return _SMisu;
                }

                set
                {
                    SetField(ref _SMisu, value);
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
