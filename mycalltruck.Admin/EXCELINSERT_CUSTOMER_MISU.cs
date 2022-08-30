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

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_CUSTOMER_MISU : Form
    {
        string FileName = string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();
        public EXCELINSERT_CUSTOMER_MISU()
        {
            InitializeComponent();
            InitializeStorage();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            cmb_Savegubun.SelectedIndex = 0;
            this.staticOptionsTableAdapter.Fill(cMDataSet.StaticOptions);
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
            CodeCompareTable.Clear();
            BizNoCompareTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (var _CodeCommand = _Connection.CreateCommand())
                using (var _BizNoCommand = _Connection.CreateCommand())
                {
                    _CodeCommand.CommandText = "SELECT Code FROM Customers WHERE ClientId = @ClientId";
                    _BizNoCommand.CommandText = "SELECT BizNo FROM Customers WHERE ClientId = @ClientId";
                    _CodeCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _BizNoCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _CodeCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            CodeCompareTable.Add(_Reader.GetString(0));
                        }
                    }
                    using (SqlDataReader _Reader = _BizNoCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            BizNoCompareTable.Add(_Reader.GetString(0).Replace("-",""));
                        }
                    }
                }
                _Connection.Close();
            }
            CodeCompareTable.Sort();
        }

        #region UPDATE
        private void ImportExcel()
        {
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);
            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.CUSTOMER;
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
                        SName = _Sheet.Cells[RowIndex, 1].Text,
                        SBiz_NO = _Sheet.Cells[RowIndex, 2].Text,
                        SMisu = _Sheet.Cells[RowIndex, 3].Text.Replace(",", ""),
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
            int ERROR_Index = 5;
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
                    if (row.SBiz_NO != "")
                    {
                        FCount++;
                       
                        //사업자등록번호 없으면 에러
                        if (String.IsNullOrEmpty(row.SBiz_NO) || !Regex.IsMatch(row.SBiz_NO.Replace("-",""), @"^\d{10}$"))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 사업자번호 형식 또는 빈값";
                            }
                            else
                            {
                                ErrorText += "사업자번호 형식 또는 빈값";

                            }
                            RowErrorCount++;

                        }
                        else
                        {
                            if(!BizNoCompareTable.Contains(row.SBiz_NO.Replace("-","")))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 해당사업자번호 없음 ";
                                }
                                else
                                {
                                    ErrorText += "해당사업자번호 없음";

                                }
                                RowErrorCount++;
                            }
                            else
                            {
                                BizNoCompareTable.Add(row.SBiz_NO);
                            }
                        }
                        //상호 없으면 에러
                        if (String.IsNullOrEmpty(row.SName))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 상호 빈값";
                            }
                            else
                            {
                                ErrorText += "상호 빈값";

                            }
                            RowErrorCount++;
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
                $@"Update [Customers] SET Misu = @Misu,Mizi = @Mizi,MStartDate = getdate(),MStart = @Mizi WHERE CustomerId  = @CustomerId  AND ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
            if (DataCount == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }
            InitCompareTable();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = UpdateQuery;
                   
                    //_Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Misu", SqlDbType.Decimal);
                    _Command.Parameters.Add("@Mizi", SqlDbType.Decimal);

                    _Command.Parameters.Add("@CustomerId", SqlDbType.Int);
                    _Command.Parameters.Add("@ClientId", SqlDbType.Int);


                    foreach (Model row in _CoreList)
                    {
                        InitCompareTable();
                        if (row.SBiz_NO != "" && row.Error == "")
                        {
                           
                            string _BizNo = row.SBiz_NO.Replace("-", "");
                            _BizNo = _BizNo.Substring(0, 3) + "-" + _BizNo.Substring(3, 2) + "-" + _BizNo.Substring(5);
                            //   _Command.Parameters["@BizNo"].Value = _BizNo;

                            CustomerRepository mCustomerRepository = new CustomerRepository();


                            var _CustomerId = mCustomerRepository.GetCustomerId(row.SBiz_NO);
                         
                            _Command.Parameters["@CustomerId"].Value = _CustomerId;
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
         
            private String _SBiz_NO = "";
            private String _SName = "";
           
            private String _SMizi = "";
            private String _SMisu = "";
            private String _Error = "";

         
            public string SBiz_NO
            {
                get
                {
                    return _SBiz_NO;
                }

                set
                {
                    SetField(ref _SBiz_NO, value);
                }
            }
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
