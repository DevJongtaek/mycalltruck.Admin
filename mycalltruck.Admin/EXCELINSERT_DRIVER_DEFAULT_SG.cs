using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using OfficeOpenXml;
using mycalltruck.Admin.Class.DataSet;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_DRIVER_DEFAULT_SG : Form
    {
        string FileName = string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        string SPayBankName = string.Empty;
        DESCrypt m_crypt = null;
        public EXCELINSERT_DRIVER_DEFAULT_SG()
        {
            InitializeComponent();
            InitializeStorage();
            m_crypt = new DESCrypt("12345678");
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            cmb_Savegubun.SelectedIndex = 0;
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
                        S_Idx = _Sheet.Cells[RowIndex, 1].Text.Trim(),
                        SName = _Sheet.Cells[RowIndex, 3].Text.Trim(),
                        SBiz_NO = _Sheet.Cells[RowIndex, 4].Text.Trim(),
                        SCeo = _Sheet.Cells[RowIndex, 5].Text.Trim(),
                        SUptae = _Sheet.Cells[RowIndex, 6].Text.Trim(),
                        SUpjong = _Sheet.Cells[RowIndex, 7].Text.Trim(),
                        SState = _Sheet.Cells[RowIndex, 9].Text.Trim(),
                        SFaxNo = _Sheet.Cells[RowIndex, 10].Text.Trim(),
                    };
                    if (!String.IsNullOrEmpty(Added.SState))
                    {
                        var Splited = Added.SState.Split(' ').Where(c=>!String.IsNullOrEmpty(c)).ToArray();
                        Added.SState = Splited[0];
                        if (Splited.Length > 1)
                            Added.SCity = Splited[1];
                        if (Splited.Length > 2)
                            Added.SStreet = String.Join(" ", Splited.Skip(2));
                    }
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
            int ERROR_Index = 13;
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
            List<String> InnerCarNo = new List<string>();
            Task.Factory.StartNew(() => {
                FCount = 0;
                errCount = 0;
                int RowErrorCount = 0;
                DriverRepository mDriverRepository = new DriverRepository();
                foreach (Model row in _CoreList)
                {
                    bar.Invoke(new Action(()=> {
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
                        // 차량번호 조회 후, 타사 등록 차량이면 나머지 필드 검사 안함.
                        if (String.IsNullOrEmpty(row.SBiz_NO))
                        {
                            ErrorText += "사업자등록번호 없음";
                            errCount++;
                            row.Error = ErrorText;
                            continue;
                        }
                        else
                        {
                            var IsMyCar = mDriverRepository.IsMyCar(row.SBiz_NO.Replace("-",""));
                            if (IsMyCar)
                            {
                                ErrorText += "등록된 매입처";
                                errCount++;
                                row.Error = ErrorText;
                                continue;
                            }
                            var IsAnotherCar = mDriverRepository.IsAnotherCar(row.SBiz_NO.Replace("-", ""));
                            if(IsAnotherCar)
                            {
                                continue;
                            }
                        }
                        if (InnerCarNo.Contains(row.SBiz_NO.Replace("-", "")))
                        {
                            ErrorText += "동일 엑셀 속에 중복된 사업자등록번호";
                        }
                        InnerCarNo.Add(row.SBiz_NO.Replace("-", ""));
                        //사업자등록번호 없으면 에러
                        if (String.IsNullOrEmpty(row.SBiz_NO) || row.SBiz_NO.Replace("-","").Length != 10)
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 사업자번호 이상";
                            }
                            else
                            {
                                ErrorText += "사업자번호 이상";

                            }
                            RowErrorCount++;

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
                        //업태
                        if (String.IsNullOrEmpty(row.SUptae))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 업태 빈값";
                            }
                            else
                            {
                                ErrorText += "업태 빈값";

                            }
                            RowErrorCount++;
                        }
                        //종목
                        if (String.IsNullOrEmpty(row.SUpjong))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 업종 빈값";
                            }
                            else
                            {
                                ErrorText += "업종 빈값";

                            }
                            RowErrorCount++;
                        }
                        //대표자
                        if (String.IsNullOrEmpty(row.SCeo))
                        {

                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 대표자 빈값";
                            }
                            else
                            {
                                ErrorText += "대표자 빈값";

                            }
                            RowErrorCount++;
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
            if (DataCount == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }
            DriverRepository mDriverRepository = new DriverRepository();
            foreach (Model row in _CoreList)
            {
                if (row.S_Idx != "" && row.Error == "")
                {
                    string S_Idx = row.S_Idx.Trim();
                    string SBiz_NO = row.SBiz_NO.Trim();
                    string SName = row.SName.Trim();
                    string SCeo = row.SCeo.Trim();
                    string SUptae = row.SUptae.Trim();
                    string SUpjong = row.SUpjong.Trim();
                    string SCeoBirth = "000000";
                    string SZip = "";
                    string SState = row.SState.Trim();
                    string SCity = row.SCity.Trim();
                    string SStreet = row.SStreet.Trim();
                    string SMobileNo = "";
                    string SPhoneNo = "";
                    string SFaxNo = row.SFaxNo.Trim();
                    string SCarNo = row.SBiz_NO.Replace("-", "").Trim();

                    SBiz_NO = SBiz_NO.Replace("-", "");
                    SBiz_NO = SBiz_NO.Substring(0, 3) + "-" + SBiz_NO.Substring(3, 2) + "-" + SBiz_NO.Substring(5);

                    var IsAnotherCar = mDriverRepository.IsAnotherCar(SCarNo);
                    if (IsAnotherCar)
                    {
                        mDriverRepository.ConnectDriver(SCarNo);
                    }
                    else
                    {
                       int DriverId =  mDriverRepository.CreateDriver_SG(SBiz_NO, SName, SCeo, SUptae, SUpjong, SCeoBirth,
                            SZip, SState, SCity, SStreet, SMobileNo, SPhoneNo, SCarNo, SFaxNo, "");
                    }
                }
            }
            EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
            this.Close();
        }
        #endregion

        #region STORAGE
        class Model : INotifyPropertyChanged
        {
            private String _S_Idx = "";
            private String _SName = "";
            private String _SBiz_NO = "";
            private String _SCeo = "";
            private String _SUptae = "";
            private String _SUpjong = "";
            private String _SState = "";
            private String _SCity = "";
            private String _SStreet = "";
            private String _SFaxNo = "";
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
            public string SCeo
            {
                get
                {
                    return _SCeo;
                }

                set
                {
                    SetField(ref _SCeo, value);
                }
            }
            public string SUptae
            {
                get
                {
                    return _SUptae;
                }

                set
                {
                    SetField(ref _SUptae, value);
                }
            }
            public string SUpjong
            {
                get
                {
                    return _SUpjong;
                }

                set
                {
                    SetField(ref _SUpjong, value);
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
            public string SStreet
            {
                get
                {
                    return _SStreet;
                }

                set
                {
                    SetField(ref _SStreet, value);
                }
            }
            public string SFaxNo
            {
                get
                {
                    return _SFaxNo;
                }

                set
                {
                    SetField(ref _SFaxNo, value);
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
