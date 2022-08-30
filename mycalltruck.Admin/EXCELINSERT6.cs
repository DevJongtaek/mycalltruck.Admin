using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;
using mycalltruck.Admin.Class.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class;
using OfficeOpenXml;
using Newtonsoft.Json;
using DynamicExpresso;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
   
    public partial class EXCELINSERT6 : Form
    {
        int FPIS_ID;
        #region DRIVER / CUSTOMER
        class DriverViewModel
        {
            public String Name { get; set; }
            public String CarYear { get; set; }
            public String BizNo { get; set; }
            public int CarGubun { get; set; }
            public String CarNo { get; set; }
            public int CarType { get; set; }
            public String MobileNo { get; set; }
            public int DriverId { get; set; }
            public String LoginId { get; set; }
        }
        private List<DriverViewModel> _DriverViewModelList = new List<DriverViewModel>();
        private void LoadLocalDriver(bool Force = false)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT Drivers.Name, CarYear,BizNo, CarGubun, CarNo, CarType, MobileNo, DriverId
                        FROM Drivers";
                    _Command.CommandText += Environment.NewLine + "WHERE CandidateId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _DriverViewModelList.Add(new DriverViewModel
                            {
                                Name = _Reader.GetStringN(0),
                                CarYear = _Reader.GetStringN(1),
                                BizNo = _Reader.GetStringN(2),
                                CarGubun = _Reader.GetInt32Z(3),
                                CarNo = _Reader.GetStringN(4),
                                CarType = _Reader.GetInt32Z(5),
                                MobileNo = _Reader.GetStringN(6),
                                DriverId = _Reader.GetInt32Z(7),
                            });
                        }
                    }
                }
                _Connection.Close();
            }
        }
        class CustomerViewModel
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string PhoneNo { get; set; }
            public string Code { get; set; }
        }
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>();
        private void InitCustomerTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT Customers.CustomerId, SangHo, PhoneNo,Code FROM Customers Customers WHERE ClientId = @ClientId Order by Code DESC";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var dataReader = commnad.ExecuteReader();
                    while (dataReader.Read())
                    {
                        _CustomerViewModelList.Add(
                          new CustomerViewModel
                          {
                              CustomerId = dataReader.GetInt32(0),
                              Name = dataReader.GetStringN(1),
                              PhoneNo = dataReader.GetStringN(2),
                              Code = dataReader.GetStringN(3),
                          });
                    }
                }
                connection.Close();
            }
        }
        #endregion
        
        string FileName = string.Empty;
        int ExcelIndex = 0;

        public EXCELINSERT6()
        {
            InitializeComponent();
        }

        public EXCELINSERT6(int iExcelIndex)
        {
            InitializeComponent();
            ExcelIndex = iExcelIndex;
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            cmb_Savegubun.SelectedIndex = 0;
            LoadLocalDriver();
            InitCustomerTable();
            fpiS_CONTTableAdapter.Fill(fpisDataSet.FPIS_CONT);
          
        }

        private void ExcelTest()
        {
            newDGV1.Rows.Clear();
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);
            if (di.Exists == false)
            {
                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.MYCAR;
            d.DefaultExt = "xlsx";
            d.Filter = "엑셀 (*.xlsx)|*.xlsx";
            d.FilterIndex = 0;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
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
                var _Sheet = _Excel.Workbook.Worksheets[1];

                int ExcelRowIndex = 2;
                const int TestIndex = 2;
                while (true)
                {
                    var TestText = _Sheet.Cells[ExcelRowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;
                    int _RowIndex = newDGV1.Rows.Add();
                    for (int i = 0; i < 10; i++)
                    {
                        int ExcelColumnIndex = i + 1;
                        newDGV1[i, _RowIndex].Value = _Sheet.Cells[ExcelRowIndex, ExcelColumnIndex].Text;
                    }
                    ExcelRowIndex++;

                    if (ExcelRowIndex > 5000)
                    {
                        MessageBox.Show("한번에 5,000건 이상 불러올 수 없습니다.");
                        break;
                    }
                }
                btn_OK.Enabled = true;

                FileName = d.FileName;
            }
        }

      


      

        private string _AddressStateParse(String _Value)
        {
            string R = "";
            switch (_Value)
            {
                case "충북":
                    R = "충청북도";
                    break;
                case "충남":
                    R = "충청남도";
                    break;
                case "전북":
                    R = "전라북도";
                    break;
                case "전남":
                    R = "전라남도";
                    break;
                case "경북":
                    R = "경상북도";
                    break;
                case "경남":
                    R = "경상남도";
                    break;
                default:
                    break;
            }
            return R;
        }

        private void btn_Info_Click(object sender, EventArgs e)
        {
            ExcelTest();
        }

        List<Model> _ModelList = new List<Model>();
        private void btn_Test_Click(object sender, EventArgs e)
        {
            if (newDGV1.Rows.Count == 0)
            {
                MessageBox.Show("검증할 데이터가 없습니다.");
                return;
            }
           

            _ModelList.Clear();
            DriverRepository mDriverRepository = new DriverRepository();
            Dictionary<String, int> TempCustomerDictionary = new Dictionary<string, int>();
            Dictionary<String, DriverRepository.Driver> TempDriverDictionary = new Dictionary<string, DriverRepository.Driver>();
            for (int i = 0; i < newDGV1.RowCount; i++)
            {


                var nModel = new Model
                {
                    Idx = newDGV1[0, i].Value.ToString().Trim(),
                    OrderNo = newDGV1[1, i].Value.ToString().Trim(),
                    CustomerName = newDGV1[2, i].Value.ToString().Trim(),
                    SubClientId = newDGV1[3, i].Value.ToString().Trim().Replace("-", "").Replace(".0", ""),
                    StartName = newDGV1[4, i].Value.ToString().Trim(),
                    StopName = newDGV1[5, i].Value.ToString().Trim(),

                    
                    DriverCarNo = newDGV1[6, i].Value.ToString().Replace(",", "").Trim(),
                    DriverName = newDGV1[7, i].Value.ToString().Trim(),
                    //배차운송료
                    AcceptTime = newDGV1[8, i].Value.ToString().Replace(",", "").Trim(),
                    SalesPrice = newDGV1[9, i].Value.ToString().Replace(",", "").Trim(),
                 
                   
                    UniqueKey = newDGV1[ColumnUniqueKey.Index, i].Value?.ToString().Trim(),

                };
                if (String.IsNullOrEmpty(nModel.SalesPrice))
                    nModel.SalesPrice = "0";
               
                #region UniqueKey
                if (!String.IsNullOrEmpty(nModel.UniqueKey))
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = $"SELECT OrderId FROM Orders WHERE OrderStatus <> 0 AND UniqueKey = '{nModel.UniqueKey}'";
                            if(_Command.ExecuteScalar() != null)
                            {
                                _AppendError(nModel, "화물 중복 등록");
                            }
                        }
                        _Connection.Close();
                    }
                }
                #endregion
                #region Idx
                if (_ModelList.Any(c => c.Idx == nModel.Idx))
                {
                    _AppendError(nModel, "IDX 중복");
                }
                #endregion
                if(String.IsNullOrEmpty(nModel.OrderNo))
                {
                    _AppendError(nModel, "화물번호 필수");
                }
                else 
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = $"SELECT OrderId FROM Orders WHERE OrderId = '{nModel.OrderNo.Replace("-","")}'";
                            if (_Command.ExecuteScalar() == null)
                            {
                                _AppendError(nModel, "해당 배차정보 없음");
                            }
                        }
                        _Connection.Close();
                    }

                    
                }
                decimal tPrice = 0;
                DateTime tDate = DateTime.Now;
                if (String.IsNullOrEmpty(nModel.AcceptTime))
                {
                    _AppendError(nModel, "운송일");
                }
                else if (!DateTime.TryParseExact(nModel.AcceptTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out tDate))
                {
                    _AppendError(nModel, "운송일 형식");
                }

                if(nModel.SalesPrice == "0")
                {
                    _AppendError(nModel, "청구금액");
                }
                else if (!string.IsNullOrEmpty(nModel.SalesPrice))
                {
                    if (!decimal.TryParse(nModel.SalesPrice.Replace(",", ""), out tPrice))
                    {
                        _AppendError(nModel, "청구금액숫자");
                    }
                }




                _ModelList.Add(nModel);
                newDGV1[ColumnError.Index, i].Value = nModel.Error;
            }
            int FCount = _ModelList.Count;
            int errCount = _ModelList.Count(c => !String.IsNullOrEmpty(c.Error));
            if (errCount == 0)
            {
                label4.Text = FCount.ToString() + " 건";
                label5.Text = (FCount - errCount).ToString() + " 건";
                label6.Text = errCount.ToString() + " 건";
                label7.Visible = false;
               // btn_Update.Enabled = true;
            }
            else
            {
                label4.Text = FCount.ToString() + " 건";
                label5.Text = (FCount - errCount).ToString() + " 건";
                label6.Text = errCount.ToString() + " 건";
                label7.Visible = true;
                //btn_Update.Enabled = false;
            }

            btn_Update.Enabled = true;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (newDGV1.Rows.Count == 0)
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
            int ERROR_Index = 11;
            for (int i = 0; i < _ModelList.Count; i++)
            {
                var _Model = _ModelList[i];
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

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            var UpdateQuery =
              $@"Update [Orders] SET AcceptTime = @AcceptTime,SalesPrice = @SalesPrice WHERE OrderId  = @OrderId AND SalesManageId is null  AND ClientId = {LocalUser.Instance.LogInInformation.ClientId}";

            if (_ModelList.Count(c => String.IsNullOrEmpty(c.Error)) == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }

            if (cmb_Savegubun.SelectedIndex == 0)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = UpdateQuery;

                        //_Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                        _Command.Parameters.Add("@SalesPrice", SqlDbType.Decimal);
                        _Command.Parameters.Add("@AcceptTime", SqlDbType.DateTime);

                        _Command.Parameters.Add("@OrderId", SqlDbType.Int);



                        foreach (var _Model in _ModelList.Where(c => String.IsNullOrEmpty(c.Error)))
                        {
                            DateTime tDate = DateTime.Now;
                            string _OrderId = _Model.OrderNo.Replace("-", "");
                            decimal _SalesPrice = Convert.ToDecimal(_Model.SalesPrice);
                            DateTime.TryParseExact(_Model.AcceptTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out tDate);

                            _Command.Parameters["@OrderId"].Value = _OrderId;
                            _Command.Parameters["@AcceptTime"].Value = tDate;
                            _Command.Parameters["@SalesPrice"].Value = _SalesPrice;




                            _Command.ExecuteNonQuery();

                        }
                    }
                    _Connection.Close();

                    foreach (var _Model in _ModelList.Where(c => String.IsNullOrEmpty(c.Error)))
                    {



                    }
                }
            

                EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
                _Form.Owner = this;
                _Form.StartPosition = FormStartPosition.CenterParent;
                _Form.ShowDialog();
            }
            this.Close();
        }

        private void _AppendError(Model _Model, String ErrorMessage)
        {
            if (String.IsNullOrWhiteSpace(_Model.Error))
                _Model.Error = ErrorMessage;
            else
                _Model.Error += $", {ErrorMessage}";
        }

        class Model
        {
            public String Idx { get; set; }
            public String OrderNo { get; set; }
            public String SubClientId { get; set; }
            public String StartName { get; set; }
            public String StopTime { get; set; }
            public String StopName { get; set; }
            public String DriverCarNo { get; set; }
            public String AcceptTime { get; set; }
            public String SalesPrice { get; set; }
            public String DriverName { get; set; }
            public String CustomerName { get; set; }
            //public String Price { get; set; }
            //public String Item { get; set; }
            //public String Etc { get; set; }
            //public String DriverCarNo { get; set; }
            //public String DriverName { get; set; }
            //public String DriverBizNo { get; set; }
            //public String DriverMobileNo { get; set; }
            //public String DriverCarType { get; set; }
            //public String DriverCarSize { get; set; }
            public String Error { get; set; } = "";
            //public int CarType { get; set; }
            //public int CarSize { get; set; }
            //public int DriverId { get; set; }
            //public int CustomerId { get; set; }
            public DriverRepository.Driver Driver { get; set; }
          //  public String Wgubun { get; set; }
            public String UniqueKey { get; set; }
           // public String PayLocation { get; set; }

          //  public int FPiS_ID { get; set; }
        }

        List<ExcelInfoModel> ExcelModelList = new List<ExcelInfoModel>();

        class ExcelInfoModel
        {
            public String Name { get; set; }
            public int HeaderRowIndex { get; set; }
            public String Match { get; set; }
            public String UniqueKey { get; set; }
            public int SkipRowCount { get; set; }
        }

        class ExpressionModel
        {
            public String TargetProperty { get; set; }
            public String TargetExpression { get; set; }
            public String TargetFileColumnIndex { get; set; }
            public bool IsReadOnly { get; set; }
            public bool IsNumber { get; set; }
            public bool IsDate { get; set; }
            public bool IsOnlyNumber { get; set; }
            public bool IsRequire { get; set; }
            public bool IsCheck { get; set; }
            public String Reference { get; set; }
            public int AddressIndex { get; set; }
        }

        class ExcelModel
        {
            public String ColumnHeader { get; set; }
            public String ColumnIndex { get; set; }
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
    }
}
