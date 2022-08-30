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
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_DRIVER_SHARE : Form
    {
        string FileName = string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        string SPayBankName = string.Empty;
        DESCrypt m_crypt = null;
        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();
        public EXCELINSERT_DRIVER_SHARE()
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
                int RowIndex = 2;
                while (true)
                {
                    if (String.IsNullOrEmpty(_Sheet.Cells[RowIndex, 1].Text) )
                        break;

                    var Added = new Model
                    {
                        CarNo = _Sheet.Cells[RowIndex, 1].Text.Trim(),
                        MobileNo = _Sheet.Cells[RowIndex, 2].Text.Trim(),
                        CarYear = _Sheet.Cells[RowIndex, 3].Text.Trim(),
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
            int ERROR_Index = 4;
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
                DriverRepository mDriverRepository = new DriverRepository();
                foreach (Model row in _CoreList)
                {
                    bar.Invoke(new Action(() =>
                    {
                        bar.Value++;
                    }));
                    FCount++;
                    String ErrorText = "";
                    IQueryable<Driver> DriverQuery = ShareOrderDataSet.Drivers.Where(c => c.ServiceState != 5);

                    if (!String.IsNullOrWhiteSpace(row.CarNo))
                    {
                        DriverQuery = DriverQuery.Where(c => c.CarNo == row.CarNo);
                    }
                    //if (!String.IsNullOrWhiteSpace(row.MobileNo))
                    //{
                    //    DriverQuery = DriverQuery.Where(c => c.MobileNo == row.MobileNo);
                    //}
                    //if (!String.IsNullOrWhiteSpace(row.CarYear))
                    //{
                    //    DriverQuery = DriverQuery.Where(c => c.CarYear == row.CarYear);
                    //}

                    if (!DriverQuery.Any())
                    {
                        ErrorText = "해당 차량 없음";
                        errCount++;
                    }
                    else
                    {
                        int[] DriverIdArray = DriverQuery.Select(c => c.DriverId).ToArray();
                        List<int> DriverIdList = new List<int>(DriverIdArray);
                        foreach (var driverId in ShareOrderDataSet.DriverInstances.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && DriverIdArray.Contains(c.DriverId)).Select(c => c.DriverId))
                        {
                            if (DriverIdList.Contains(driverId))
                                DriverIdList.Remove(driverId);
                        }
                        if (DriverIdList.Any())
                        {
                            row.DriverIds = DriverIdList.ToArray();
                        }
                        else
                        {
                            ErrorText = "공유된 차량";
                            errCount++;
                        }
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

            var DriverIdArray = _CoreList.SelectMany(c => c.DriverIds).Distinct();

            foreach (var DriverId in DriverIdArray)
            {
                if(!ShareOrderDataSet.DriverInstances.Any(c=>c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.DriverId == DriverId))
                {
                    ShareOrderDataSet.DriverInstances.Add(new DriverInstance
                    {
                        ClientId = LocalUser.Instance.LogInInformation.ClientId,
                        DriverId = DriverId,
                    });
                    ShareOrderDataSet.SaveChanges();
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
            private String _CarYear = "";
            private String _MobileNo = "";
            private String _CarNo = "";
            private String _Error = "";

            public string CarYear
            {
                get
                {
                    return _CarYear;
                }

                set
                {
                    SetField(ref _CarYear, value);
                }
            }
            public string MobileNo
            {
                get
                {
                    return _MobileNo;
                }

                set
                {
                    SetField(ref _MobileNo, value);
                }
            }
            public string CarNo
            {
                get
                {
                    return _CarNo;
                }

                set
                {
                    SetField(ref _CarNo, value);
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

            public int[] DriverIds { get; set; } = new int[0];

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
