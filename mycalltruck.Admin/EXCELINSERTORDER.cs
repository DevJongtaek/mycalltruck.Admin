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

namespace mycalltruck.Admin
{
   
    public partial class EXCELINSERTORDER : Form
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
                    //_Command.CommandText += Environment.NewLine + "WHERE CandidateId = @ClientId";
                    //_Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
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
            public string CustomerManager { get; set; }
            public int BizGubun { get; set; }
            // public int CClientId { get; set; }
        }
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>();
        private void InitCustomerTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo,Customers.Code,ISNULL(CustomerManager.ManagerName,''),BizGubun FROM Customers" +
                          " LEFT  JOIN CustomerManager ON Customers.CustomerManagerId = CustomerManager.ManagerId" +
                        " WHERE Customers.ClientId = @ClientId  Order by Customers.Code DESC";
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
                              CustomerManager = dataReader.GetStringN(4),
                              BizGubun  = dataReader.GetInt32(5),
                              // CClientId = dataReader.GetInt32(4),
                          });
                    }
                }
                connection.Close();
            }
        }
        #endregion
        
        string FileName = string.Empty;
      

        public EXCELINSERTORDER()
        {
            InitializeComponent();
        }

     


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            cmb_Savegubun.SelectedIndex = 0;
            LoadLocalDriver();
            InitCustomerTable();
            fpiS_CONTTableAdapter.Fill(fpisDataSet.FPIS_CONT);
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText = "SELECT Name, HeaderRowIndex, Match, UniqueKey, SkipRowCount FROM ExcelInfoes";
                using (SqlDataReader _Reader = _Command.ExecuteReader())
                {
                    while (_Reader.Read())
                    {
                        ExcelModelList.Add(new ExcelInfoModel
                        {
                            Name = _Reader.GetString(0),
                            HeaderRowIndex = _Reader.GetInt32(1),
                            Match = _Reader.GetString(2),
                            UniqueKey = _Reader.GetStringN(3),
                            SkipRowCount = _Reader.GetInt32Z(4),
                        });
                    }
                }
                _Connection.Close();
            }
            //cmbSelectExcel.Items.Add("회원가입 배차정보 입력양식-기본");
            //foreach (var excelModel in ExcelModelList)
            //{
            //    cmbSelectExcel.Items.Add(excelModel.Name);
            //}
            //cmbSelectExcel.SelectedIndex = ExcelIndex;
            //if (ExcelIndex > 0)
            //    cmbSelectExcel.Enabled = false;
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

                int ExcelRowIndex = 3;
                const int TestIndex = 1;
                while (true)
                {
                    var TestText = _Sheet.Cells[ExcelRowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;
                    int _RowIndex = newDGV1.Rows.Add();
                    for (int i = 0; i < 28; i++)
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

        private String ParseSourceExpression(Dictionary<ExcelModel, List<String>> ExcelCellMap, int _ValueIndex, ExpressionModel _ExpressionModel)
        {
            String R = "";
            var _Value = "";
            var _Expression = _ExpressionModel.TargetExpression;
            while (_Expression.IndexOf("{{") > -1)
            {
                var _Index = _Expression.IndexOf("{{");
                var _EndIndex = _Expression.IndexOf("}}", _Index);
                var _HeaderWord = _Expression.Substring(_Index, _EndIndex - _Index + 2);
                var _SourceColumnIndex = ParseHeader(_HeaderWord);
                if(!String.IsNullOrEmpty(_SourceColumnIndex.ColumnHeader))
                {
                    var _KeyValue = ExcelCellMap.FirstOrDefault(c => c.Key.ColumnHeader == _SourceColumnIndex.ColumnHeader);
                    if(_KeyValue.Value != null && _KeyValue.Value.Count > _ValueIndex)
                    {
                        _Value = _KeyValue.Value[_ValueIndex];
                    }

                }
                if (!String.IsNullOrEmpty(_SourceColumnIndex.ColumnIndex))
                {
                    var _KeyValue = ExcelCellMap.FirstOrDefault(c => c.Key.ColumnIndex == _SourceColumnIndex.ColumnIndex);
                    if (_KeyValue.Value != null && _KeyValue.Value.Count > _ValueIndex)
                    {
                        _Value = _KeyValue.Value[_ValueIndex];
                    }
                }
                _Expression = _Replace(_Expression, _HeaderWord, _Value);
            }
            while (_Expression.IndexOf("[[") > -1)
            {
                var _Index = _Expression.IndexOf("[[");
                var _EndIndex = _Expression.IndexOf("]]", _Index);
                var _HeaderWord = _Expression.Substring(_Index, _EndIndex - _Index + 2);
                var _SourceColumnIndex = ParseHeader(_HeaderWord);
                if (!String.IsNullOrEmpty(_SourceColumnIndex.ColumnHeader))
                {
                    var _KeyValue = ExcelCellMap.FirstOrDefault(c => c.Key.ColumnHeader == _SourceColumnIndex.ColumnHeader);
                    if (_KeyValue.Value != null && _KeyValue.Value.Count > _ValueIndex)
                    {
                        _Value = _KeyValue.Value[_ValueIndex];
                    }

                }
                if (!String.IsNullOrEmpty(_SourceColumnIndex.ColumnIndex))
                {
                    var _KeyValue = ExcelCellMap.FirstOrDefault(c => c.Key.ColumnIndex == _SourceColumnIndex.ColumnIndex);
                    if (_KeyValue.Value != null && _KeyValue.Value.Count > _ValueIndex)
                    {
                        _Value = _KeyValue.Value[_ValueIndex];
                    }

                }
                _Expression = _Replace(_Expression, _HeaderWord, _Value);
            }
            if (_ExpressionModel.IsCheck)
            {
                if (String.IsNullOrEmpty(_Expression))
                    _Expression = (true).ToString();
                else
                {
                    bool _R = false;
                    if (_Expression.Contains("=") && _Expression.Split('=').Length > 1)
                    {
                        _R = _Expression.Split('=')[0].Trim() == _Expression.Split('=')[1].Trim();
                    }
                    _Expression = _R.ToString();
                }
            }
            if (_ExpressionModel.IsDate)
            {
                _Expression = _Expression.Replace("오전", DateTime.Now.ToString("yyyyMMdd"));
                _Expression = _Expression.Replace("오후", DateTime.Now.ToString("yyyyMMdd"));
                _Expression = _Expression.Replace("오늘", DateTime.Now.ToString("yyyyMMdd"));

                if (!string.IsNullOrEmpty(_Expression))
                {
                    _Expression = DateTime.Now.Year + _Expression.Replace("-", "").Substring(4, 4);
                }
            }
            if (_ExpressionModel.IsOnlyNumber)
            {
                _Expression = String.Join("", _Expression.Where(c => c >= '0' && c <= '9'));
            }
            if (_ExpressionModel.IsNumber)
            {
                _Expression = _Expression.Replace(",", "").Trim();
                try
                {
                    var _Interpreter = new Interpreter();
                    _Expression = _Interpreter.Eval(_Expression).ToString();
                }
                catch (Exception)
                {
                    _Expression = "";
                }
            }
            if (!String.IsNullOrEmpty(_ExpressionModel.Reference))
            {
                var _Reference = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == _ExpressionModel.Reference).ToArray();
                if(_Reference.Any(c=> _Expression.StartsWith(c.Name) || (c.Number.ToString() == _Expression)))
                {
                    _Expression = _Reference.First(c => _Expression.StartsWith(c.Name) || (c.Number.ToString() == _Expression)).Value.ToString();
                }
                else
                {
                    _Expression = "99";
                }
            }
            if(_ExpressionModel.AddressIndex > 0)
            {
                if(_ExpressionModel.AddressIndex == 1)
                {
                    var _T = _AddressStateParse(_Expression);
                    if (!String.IsNullOrEmpty(_T))
                    {
                        _Expression = _T;
                    }
                    else
                    {
                        if (SingleDataSet.Instance.AddressReferences.Any(c => c.State.StartsWith(_Expression)))
                        {
                            _Expression = SingleDataSet.Instance.AddressReferences.First(c => c.State.StartsWith(_Expression)).State;
                        }
                        else
                        {

                            //_Expression = "";

                        }
                    }
                }
                else if(_ExpressionModel.AddressIndex == 2)
                {
                    bool _IsParsed = false;
                    var _Splited = _Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(_Splited.Length > 1)
                    {
                        var _State = _Splited[0];
                        var _T = _AddressStateParse(_State);
                        if (!String.IsNullOrEmpty(_T))
                            _State = _T;
                        if (SingleDataSet.Instance.AddressReferences.Any(c => c.State.StartsWith(_State)))
                        {
                            var _AddressList = SingleDataSet.Instance.AddressReferences.Where(c => c.State.StartsWith(_State));
                            _Splited = _Expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            //if(_Splited.Length > 1)
                            //{
                            //    foreach (var _Address in _AddressList)
                            //    {
                            //        var _CitySplited = _Address.City.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            //        if(_CitySplited.Length == 2)
                            //        {
                            //            if(_CitySplited[0].Contains(_Splited[0]) && _CitySplited[1].Contains(_Splited[1]))
                            //            {
                            //                _IsParsed = true;
                            //                _Expression = _Address.City;
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                            //if (!_IsParsed)
                            //{
                            //    if (_AddressList.Any(c => c.City.Contains(_Splited[0]))){
                            //        _IsParsed = true;
                            //        _Expression = _AddressList.First(c => c.City.Contains(_Splited[0])).City;
                            //    }
                            //}
                        }
                    }
                   // if (!_IsParsed)
                        //_Expression = "";
                }
            }
            R = _Expression;
            return R;
        }

        private ExcelModel ParseHeader(String HeaderWord)
        {
            var _HeaderText = HeaderWord.Substring(2, HeaderWord.Length - 4);
            if (HeaderWord.StartsWith("{{"))
            {
                return new ExcelModel { ColumnHeader = _HeaderText };
            }
            else if (HeaderWord.StartsWith("[["))
            {
                return new ExcelModel { ColumnIndex = _HeaderText };
            }
            return new ExcelModel();
        }

        private string _Replace(String _Source, String _Expression, String _Value)
        {
            var _Splited = _Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            _Source = _Source.Replace(_Expression + "[4~]", _Splited.Length > 3 ? String.Join(" ", _Splited.Skip(3)) : "");
            _Source = _Source.Replace(_Expression + "[3~]", _Splited.Length > 2 ? String.Join(" ", _Splited.Skip(2)) : "");
            _Source = _Source.Replace(_Expression + "[2~]", _Splited.Length > 1 ? String.Join(" ", _Splited.Skip(1)) : "");
            _Source = _Source.Replace(_Expression + "[1~]", _Splited.Length > 0 ? String.Join(" ", _Splited.Skip(0)) : "");
            _Source = _Source.Replace(_Expression + "[5]", _Splited.Length > 4 ? _Splited[4] : "");
            _Source = _Source.Replace(_Expression + "[4]", _Splited.Length > 3 ? _Splited[3] : "");
            _Source = _Source.Replace(_Expression + "[3]", _Splited.Length > 2 ? _Splited[2] : "");
            _Source = _Source.Replace(_Expression + "[2]", _Splited.Length > 1 ? _Splited[1] : "");
            _Source = _Source.Replace(_Expression + "[1]", _Splited.Length > 0 ? _Splited[0] : "");
            _Source = _Source.Replace(_Expression, _Value);
            return _Source;
        }

        private string _AddressStateParse(String _Value)
        {
            string R = "";
            switch (_Value)
            {
                case "강원도":
                    R = "강원";
                    break;
                case "경기도":
                    R = "경기";
                    break;
                case "경상남도":
                    R = "경남";
                    break;
                case "경상북도":
                    R = "경북";
                    break;
                case "광주광역시":
                    R = "광주";
                    break;
                case "대구광역시":
                    R = "대구";
                    break;
                case "대전광역시":
                    R = "대전";
                    break;
                case "부산광역시":
                    R = "부산";
                    break;
                case "서울특별시":
                    R = "서울";
                    break;
                case "세종특별자치시":
                    R = "세종";
                    break;
                case "울산광역시":
                    R = "울산";
                    break;
                case "인천광역시":
                    R = "인천";
                    break;
                case "전라남도":
                    R = "전남";
                    break;
                case "전라북도":
                    R = "전북";
                    break;
                case "제주특별자치도":
                    R = "제주";
                    break;
                case "충청남도":
                    R = "충남";
                    break;
                case "충청북도":
                    R = "충북";
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
            Dictionary<String, int> TempCustomerTeamDictionary = new Dictionary<string, int>();
            Dictionary<String, DriverRepository.Driver> TempDriverDictionary = new Dictionary<string, DriverRepository.Driver>();
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                var _ReferralName = "";
                var _ReferralId = 0;
                var _Code = "";
                if (newDGV1[26, i].Value == null)
                {


                    var Query = _CustomerViewModelList.Where(c => c.Name.Contains("화물맨")).ToArray();
                    //var Query = _CustomerViewModelList.Where(c => c.Name.Contains("화물맨")).ToArray();
                    if (Query.Any())
                    {
                        _ReferralId = Query.First().CustomerId;
                        _Code = Query.First().Code;
                    }
                    else
                    {
                        _Code = "1001";



                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                    @"INSERT INTO [dbo].[Customers]
                                            ([Code] ,[BizNo] ,[SangHo] ,[Ceo] ,[Uptae] ,[Upjong] ,[AddressState] ,[AddressCity] ,[AddressDetail]
                                            ,[BizGubun] ,[ResgisterNo] ,[SalesGubun] ,[Email] ,[PhoneNo] ,[FaxNo] ,[ChargeName] ,[MobileNo] ,[CreateTime]
                                            ,[ClientId] ,[Zipcode] ,[SubClientId] ,[EndDay] ,[ClientUserId] ,[PointMethod]
                                            ,[Image1] ,[Image2] ,[Image3] ,[Image4] ,[Image5] ,[DriverId] ,[Fee]) OUTPUT INSERTED.CustomerId
                                        VALUES
                                            (@Code ,'121-81-98192' ,'(주)화물맨' ,'임영묵' ,'주선' ,'화물정보제공' ,'경기도' ,'광주시' ,'장지1길 90 (역동)'
                                            ,4 ,'' ,2 ,'iahafree@naver.com' ,'031-588-0424' ,'031-798-2416' ,'' ,'' ,GETDATE()
                                            ,@ClientId ,'12777' ,NULL ,'0' ,0 ,1
                                            ,NULL ,NULL ,NULL ,NULL ,NULL ,0 ,0)  SELECT @@IDENTITY ";

                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                _Command.Parameters.AddWithValue("@Code", _Code);
                                Object o = _Command.ExecuteScalar();
                                if (o != null)
                                {
                                    _ReferralId = Convert.ToInt32(o.ToString());
                                }
                            }
                            _Connection.Close();
                        }

                    }


                    newDGV1[25, i].Value = _Code;
                }
                else
                {
                    _Code = newDGV1[26, i].Value.ToString();
                }
               
                var nModel = new Model
                {
                    Idx = newDGV1[0, i].Value.ToString().Trim(),
                    CustomerName = newDGV1[1, i].Value.ToString().Trim(),
                    CustomerBizNo = newDGV1[2, i].Value.ToString().Trim().Replace("-", "").Replace(".0", ""),
                    CustomerTeam = newDGV1[3, i].Value.ToString().Trim(),

                    StartTime = newDGV1[4, i].Value.ToString().Trim(),
                    StopTime = newDGV1[5, i].Value.ToString().Trim(),
                    StartState = newDGV1[6, i].Value.ToString().Trim(),
                    StartCity = newDGV1[7, i].Value.ToString().Trim(),
                    StartStreet = newDGV1[8, i].Value.ToString().Trim(),
                    Start = newDGV1[9, i].Value.ToString().Trim(),
                    StopState = newDGV1[10, i].Value.ToString().Trim(),
                    StopCity = newDGV1[11, i].Value.ToString().Trim(),
                    StopStreet = newDGV1[12, i].Value.ToString().Trim(),
                    Stop = newDGV1[13, i].Value.ToString().Trim(),
                    Item = newDGV1[14, i].Value.ToString().Trim(),

                    ItemCarType = newDGV1[15, i].Value.ToString().Trim(),
                    ItemCarSize = newDGV1[16, i].Value.ToString().Trim(),
                    ItemSize = newDGV1[17, i].Value.ToString().Trim(),

                    PayLocation = newDGV1[18, i].Value.ToString().Trim(),


                    //화주운송료
                    SalesPrice  = newDGV1[19, i].Value.ToString().Replace(",", "").Trim(),
                    //배차운송료
                    TradePrice = newDGV1[20, i].Value.ToString().Replace(",", "").Trim(),

                    AlterPrice = newDGV1[21, i].Value.ToString().Replace(",", "").Trim(),

                    StartPrice = newDGV1[22, i].Value.ToString().Replace(",", "").Trim(),
                    StopPrice = newDGV1[23, i].Value.ToString().Replace(",", "").Trim(),
                    DriverPrice = newDGV1[24, i].Value.ToString().Replace(",", "").Trim(),
                   
                    Etc = newDGV1[25, i].Value.ToString().Trim(),
                   
                    ReferralId = _Code,
                    UniqueKey = newDGV1[ColumnUniqueKey.Index, i].Value?.ToString().Trim(),

                };
                if (String.IsNullOrEmpty(nModel.Price))
                    nModel.Price = "0";
                if (String.IsNullOrEmpty(nModel.ClientPrice))
                    nModel.ClientPrice = "0";
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
                #region 화주
                //사업자등록번호 없으면 에러
                if (String.IsNullOrEmpty(nModel.CustomerBizNo) || nModel.CustomerBizNo.Length != 10)
                {
                    _AppendError(nModel, "화물사업자번호");
                }
                //있으면  찾기
                else
                {
                    if (!TempCustomerDictionary.ContainsKey(nModel.CustomerBizNo))
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                var _CommandText = "SELECT CustomerId FROM Customers Where BizNo = @BizNo AND ClientId = @ClientId ";
                                var CustomerBizNo = nModel.CustomerBizNo.Substring(0, 3) + "-" + nModel.CustomerBizNo.Substring(3, 2) + "-" + nModel.CustomerBizNo.Substring(5);
                                _Command.Parameters.AddWithValue("@BizNo", CustomerBizNo);
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                if (LocalUser.Instance.LogInInformation.IsSubClient)
                                {
                                    if (LocalUser.Instance.LogInInformation.IsAgent)
                                    {
                                        _CommandText += " AND ClientUserId = @ClientUserId";
                                        _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                                    }
                                    else
                                    {
                                        _CommandText += " AND SubClientId = @SubClientId";
                                        _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                                    }
                                }
                                _Command.CommandText = _CommandText;
                                var o = _Command.ExecuteScalar();
                                if (o != null)
                                {
                                    TempCustomerDictionary.Add(nModel.CustomerBizNo, Convert.ToInt32(o));
                                }
                            }
                            _Connection.Close();
                        }
                        if (nModel.CustomerBizNo == "0000000000" && !TempCustomerDictionary.ContainsKey(nModel.CustomerBizNo))
                        {
                            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                _Connection.Open();
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText =
                                        @"INSERT INTO [dbo].[Customers]
                                            ([Code] ,[BizNo] ,[SangHo] ,[Ceo] ,[Uptae] ,[Upjong] ,[AddressState] ,[AddressCity] ,[AddressDetail]
                                            ,[BizGubun] ,[ResgisterNo] ,[SalesGubun] ,[Email] ,[PhoneNo] ,[FaxNo] ,[ChargeName] ,[MobileNo] ,[CreateTime]
                                            ,[ClientId] ,[Zipcode] ,[SubClientId] ,[EndDay] ,[ClientUserId] ,[PointMethod]
                                            ,[Image1] ,[Image2] ,[Image3] ,[Image4] ,[Image5] ,[DriverId] ,[Fee]) OUTPUT INSERTED.CustomerId
                                        VALUES
                                            ('0000' ,'000-00-00000' ,'외부시스템 연동 가상 거래처' ,'' ,'' ,'' ,'' ,'' ,''
                                            ,1 ,'' ,1 ,'' ,'' ,'' ,'' ,'' ,GETDATE()
                                            ,@ClientId ,'' ,NULL ,'' ,NULL ,NULL
                                            ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL)";
                                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                    Object o = _Command.ExecuteScalar();
                                    if (o != null)
                                    {
                                        TempCustomerDictionary.Add(nModel.CustomerBizNo, Convert.ToInt32(o));
                                    }
                                }
                                _Connection.Close();
                            }
                        }
                    }
                    if (TempCustomerDictionary.ContainsKey(nModel.CustomerBizNo))
                        nModel.CustomerId = TempCustomerDictionary[nModel.CustomerBizNo];
                    if (nModel.CustomerId == 0)
                    {
                        _AppendError(nModel, "화주 사업자번호 없음");
                    }

                    if(string.IsNullOrEmpty(nModel.CustomerTeam))
                    {
                        nModel.CustomerTeam = "0";
                    }
                    else
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                var _CommandText = "SELECT CustomerTeamId FROM CustomerTeams Where CustomerId = @CustomerId AND ClientId = @ClientId  AND TeamName = @TeamName";
                                var CustomerBizNo = nModel.CustomerBizNo.Substring(0, 3) + "-" + nModel.CustomerBizNo.Substring(3, 2) + "-" + nModel.CustomerBizNo.Substring(5);
                                _Command.Parameters.AddWithValue("@CustomerId", nModel.CustomerId);
                                _Command.Parameters.AddWithValue("@TeamName", nModel.CustomerTeam);
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                              
                                _Command.CommandText = _CommandText;
                                var o = _Command.ExecuteScalar();
                                if (o != null)
                                {
                                    nModel.CustomerTeam = o.ToString();
                                }
                                else
                                {
                                    _AppendError(nModel, "부서");

                                }
                            }
                            _Connection.Close();
                        }
                    }
                }
                #endregion
                #region Date
                DateTime tDate = DateTime.Now;
                if (String.IsNullOrEmpty(nModel.StartTime))
                    nModel.StartTime = DateTime.Now.ToString("yyyyMMdd");
                if (String.IsNullOrEmpty(nModel.StopTime))
                    nModel.StopTime = DateTime.Now.ToString("yyyyMMdd");

                if (DateTime.TryParseExact(nModel.StartTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out tDate)
                    && DateTime.TryParseExact(nModel.StopTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out tDate))
                {
                    if (DateTime.ParseExact(nModel.StartTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None).Date > DateTime.ParseExact(nModel.StopTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None).Date)
                    {
                        _AppendError(nModel, "도착일은 출발일 이후로 입력");
                    }
                }
                else if (!DateTime.TryParseExact(nModel.StartTime,"yyyyMMdd",null, System.Globalization.DateTimeStyles.None, out tDate))
                {
                    _AppendError(nModel, "출발일");
                }
                else if (!DateTime.TryParseExact(nModel.StopTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out tDate))
                {
                    _AppendError(nModel, "도착일");
                }
                #endregion
                #region 운송료
                decimal tPrice = 0;
                if (!decimal.TryParse(nModel.ClientPrice.Replace(",",""), out tPrice))
                {
                    _AppendError(nModel, "청구운임숫자");
                }
                if (!decimal.TryParse(nModel.Price.Replace(",", ""), out tPrice))
                {
                    _AppendError(nModel, "지불운임숫자");
                }
                if (!decimal.TryParse(nModel.AlterPrice.Replace(",", ""), out tPrice))
                {
                    _AppendError(nModel, "추가운송료숫자");
                }
                if (!decimal.TryParse(nModel.StartPrice.Replace(",", ""), out tPrice))
                {
                    _AppendError(nModel, "선불운임숫자");
                }
                if (!decimal.TryParse(nModel.StopPrice.Replace(",", ""), out tPrice))
                {
                    _AppendError(nModel, "착불운임숫자");
                }
                if (!decimal.TryParse(nModel.DriverPrice.Replace(",", ""), out tPrice))
                {
                    _AppendError(nModel, "수수료숫자");
                }
                #endregion

                #region 출발지 
                if (!String.IsNullOrWhiteSpace(nModel.StartState))
                {
                    var Query =  SingleDataSet.Instance.AddressReferences.Where(c => c.State == nModel.StartState).ToArray();
                    if (Query.Any())
                    {
                        nModel.StartState = Query.First().State;
                    }
                    else
                    {
                        _AppendError(nModel, "상차지(시도)");

                    }

                }
                else
                {
                    nModel.StartState = LocalUser.Instance.LogInInformation.Client.AddressState;
                }
                if (!String.IsNullOrWhiteSpace(nModel.StartCity))
                {
                    var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.City == nModel.StartCity && c.State == nModel.StartState).ToArray();
                    if (Query.Any())
                    {
                        nModel.StartCity = Query.First().City;
                    }
                    else
                    {
                        _AppendError(nModel, "상차지(시군구)");

                    }
                }
                else
                {
                    nModel.StartCity = "";
                }

                if (!String.IsNullOrWhiteSpace(nModel.StartStreet))
                {
                    var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.City == nModel.StartCity && c.State == nModel.StartState && c.Street == nModel.StartStreet).ToArray();
                    if (Query.Any())
                    {
                        nModel.StartStreet = Query.First().Street;
                    }
                    else
                    {
                        _AppendError(nModel, "상차지(읍면동)");

                    }
                }
                else
                {
                    nModel.StartStreet = "";
                }
                #endregion
                #region 도착지 
                if (!String.IsNullOrWhiteSpace(nModel.StopState))
                {
                    var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.State == nModel.StopState).ToArray();
                    if (Query.Any())
                    {
                        nModel.StopState = Query.First().State;
                    }
                    else
                    {
                        _AppendError(nModel, "하차지(시도)");

                    }
                }
                else
                {
                    nModel.StopState = LocalUser.Instance.LogInInformation.Client.AddressState;
                }
                if (!String.IsNullOrWhiteSpace(nModel.StopCity))
                {
                    var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.City == nModel.StopCity && c.State == nModel.StopState).ToArray();
                    if (Query.Any())
                    {
                        nModel.StopCity = Query.First().City;
                    }
                    else
                    {
                        _AppendError(nModel, "하차지(시군구)");

                    }
                }
                else
                {
                    nModel.StopCity ="";
                }

                if (!String.IsNullOrWhiteSpace(nModel.StopStreet))
                {
                    var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.City == nModel.StopCity && c.State == nModel.StopState && c.Street == nModel.StopStreet).ToArray();
                    if (Query.Any())
                    {
                        nModel.StopStreet = Query.First().Street;
                    }
                    else
                    {
                        _AppendError(nModel, "하차지(읍면동)");

                    }
                }
                else
                {
                    nModel.StopStreet = "";
                }
                #endregion

                #region 톤수코드(화물)
                if (String.IsNullOrWhiteSpace(nModel.ItemCarSize))
                {
                    nModel.ItemCarSize ="1";
                }
                #endregion
                #region 차종코드(화물)
                if (String.IsNullOrWhiteSpace(nModel.ItemCarType))
                {
                    nModel.ItemCarType = "1";
                }
                #endregion
                
              

                if(String.IsNullOrWhiteSpace(nModel.ReferralId))
                {
                    _AppendError(nModel, "위탁사");

                }
               else 
                {
                    var Query = _CustomerViewModelList.Where(c => c.Code == nModel.ReferralId && c.BizGubun == 4).ToArray();

                    if(Query.Any())
                    {

                    }
                    else
                    {
                        _AppendError(nModel, "위탁사 없음");
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
            int RowIndex = 3;
            int ERROR_Index = 28;
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
            if (_ModelList.Count(c => String.IsNullOrEmpty(c.Error)) == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }

            if (cmb_Savegubun.SelectedIndex == 0)
            {
                DriverRepository mDriverRepository = new DriverRepository();
                foreach (var _Model in _ModelList.Where(c => String.IsNullOrEmpty(c.Error)))
                {
                    
                    //Order 추가
                    var nOrder = orderDataSet.Orders.NewOrdersRow();
                    
                    nOrder.ClientId = LocalUser.Instance.LogInInformation.ClientId;
                    nOrder.SetSubClientIdNull();
                    nOrder.SetClientUserIdNull();
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        nOrder.SubClientId = LocalUser.Instance.LogInInformation.SubClientId;
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            nOrder.ClientUserId = LocalUser.Instance.LogInInformation.ClientUserId;
                        }
                    }

                    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        connection.Open();
                        using (SqlCommand commnad = connection.CreateCommand())
                        {
                            commnad.CommandText = "SELECT Top 1 FPIS_ID FROM FPIS_CONT WHERE CustomerId = @CustomerId order by FPIS_ID DESC";
                            commnad.Parameters.AddWithValue("@CustomerId", _Model.CustomerId);
                            var dataReader = commnad.ExecuteReader();
                            while (dataReader.Read())
                            {

                                FPIS_ID = dataReader.GetInt32(0);
                            }
                        }
                        connection.Close();
                    }

                    nOrder.FPIS_ID = FPIS_ID;
                    nOrder.CustomerId = _Model.CustomerId;
                    nOrder.Customer = _Model.CustomerName;
                    nOrder.CustomerTeam = Convert.ToInt32(_Model.CustomerTeam);

                    if (DateTime.Now.Minute < 30)
                    {
                        nOrder.StartTime = DateTime.ParseExact(_Model.StartTime + DateTime.Now.ToString("HHmm"), "yyyyMMddHHmm", null);
                    }
                    else
                    {
                        nOrder.StartTime = DateTime.ParseExact(_Model.StartTime + DateTime.Now.AddHours(1).ToString("HHmm"), "yyyyMMddHHmm", null);
                        
                    }


                    //nOrder.StartTime = DateTime.ParseExact(_Model.StartTime, "yyyyMMdd", null);
                    nOrder.StartDate = DateTime.ParseExact(_Model.StartTime, "yyyyMMdd", null);
                    //nOrder.StopTime = DateTime.ParseExact(_Model.StopTime, "yyyyMMdd", null);

                    nOrder.StopTime = DateTime.ParseExact(_Model.StopTime + DateTime.Now.AddHours(6).ToString("HHmm"), "yyyyMMddHHmm", null);


                    nOrder.StartState = _Model.StartState;

                    var _Start = _Model.StartCity + " " + _Model.StartStreet + " " + _Model.Start;//nOrder.StartDetail;
                    var StartS = _Start.Replace("  "," ").Split(' ');

                    if (StartS.Length > 1)
                    {
                        nOrder.StartCity = StartS[0];
                        nOrder.StartStreet = StartS[1];
                        nOrder.StartDetail = String.Join(" ", StartS.Skip(2));
                    }
                    else
                    {
                        nOrder.StartCity = _Model.StartCity;
                        nOrder.StartStreet = _Model.StartStreet;
                        nOrder.StartDetail = _Model.Start;
                    }



                    
                    //nOrder.StartCity = _Model.StartCity;
                    //nOrder.StartStreet = _Model.StartStreet;
                    //nOrder.StartDetail = _Model.Start;
                    nOrder.StopState = _Model.StopState;


                    var _Stop = _Model.StopCity + " " + _Model.StopStreet + " " + _Model.Stop; // nOrder.StopDetail;
                    var StopS = _Stop.Replace("  ", " ").Split(' ');

                    if (StopS.Length > 1)
                    {
                        nOrder.StopCity = StopS[0];
                        nOrder.StopStreet = StopS[1];
                        nOrder.StopDetail = String.Join(" ", StopS.Skip(2));
                    }
                    else
                    {
                        nOrder.StopCity = _Model.StopCity;
                        nOrder.StopStreet = _Model.StopStreet;
                        nOrder.StopDetail = _Model.Stop;
                    }



                    //nOrder.StopCity = _Model.StopCity;
                    //nOrder.StopStreet = _Model.StopStreet;
                    //nOrder.StopDetail = _Model.Stop;
                    //nOrder.StartStreet = "";
                    //nOrder.StopStreet = "";
                    // nOrder.Item = "화물운송료";

                    nOrder.ItemSize = _Model.ItemSize.Replace("톤","");
                    nOrder.AcceptTime = DateTime.ParseExact(_Model.StartTime, "yyyyMMdd", null);
                    //nOrder.Price = Decimal.Parse(_Model.Price.Replace(",", ""));
                    //nOrder.ClientPrice = Decimal.Parse(_Model.ClientPrice.Replace(",", ""));
                    //nOrder.TradePrice = nOrder.Price;
                    //nOrder.SalesPrice = nOrder.SalesPrice;
                    
                    nOrder.CarCount = 1;
                    nOrder.Etc = _Model.Etc;
                    nOrder.IsShared = false;

                    int.TryParse(_Model.ItemCarType, out int _ItemCarType);
                    int.TryParse(_Model.ItemCarSize, out int _ItemCarSize);
                    if (!SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == _ItemCarType))
                        _ItemCarType = 1;
                    if (!SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == _ItemCarSize))
                        _ItemCarSize = 1;


                    nOrder.CarType = _ItemCarType;
                    nOrder.CarSize = _ItemCarSize;
                    if (nOrder.CarType == 0)
                        nOrder.CarType = _Model.Driver.CarType;
                    if (nOrder.CarSize == 0)
                        nOrder.CarSize = _Model.Driver.CarSize;
                    nOrder.PayLocation =Convert.ToInt32(_Model.PayLocation);
                    nOrder.OrderStatus = 1;
                    nOrder.SourceType = 0;
                    nOrder.Agubun = 3;
                    if (String.IsNullOrEmpty(_Model.Wgubun))
                        nOrder.Wgubun = "PC";
                    else
                        nOrder.Wgubun = _Model.Wgubun;
                    
                    nOrder.CreateTime = nOrder.StartTime;//DateTime.ParseExact(_Model.StartTime, "yyyyMMddhhmm", null);
                    //nOrder.StartStreet = "";
                    //nOrder.StopStreet = ""; 
                    nOrder.UniqueKey = _Model.UniqueKey;

                    int _Index, _EndIndex;
                    string _HeaderWord = "" ;


                    if (_Model.Etc.Contains("["))
                    {
                         _Index = _Model.Etc.IndexOf("[");
                         _EndIndex = _Model.Etc.IndexOf("]", _Index);
                         _HeaderWord = _Model.Etc.Substring(_Index);
                    }

                    if (_HeaderWord == "[인수증]")
                    {
                        nOrder.PayLocation = 1;

                        //배차운송료  / 지불운임
                        nOrder.Price = Decimal.Parse(_Model.SalesPrice.Replace(",", ""));
                        //화주운송료 /청구운임
                        nOrder.ClientPrice = Decimal.Parse(_Model.TradePrice.Replace(",", ""));
                        //지불운임
                        nOrder.TradePrice = nOrder.Price;
                        //청구운임
                        nOrder.SalesPrice = nOrder.SalesPrice;
                        //수수료
                        nOrder.AlterPrice = nOrder.ClientPrice - nOrder.Price;

                        nOrder.StartPrice = 0;
                        //착불
                        nOrder.StopPrice = 0;
                        nOrder.DriverPrice = 0;


                    }
                    //선착불
                    else
                    {

                        {
                            

                            ////배차운송료  / 지불운임
                            nOrder.Price = Decimal.Parse(_Model.Price.Replace(",", ""));
                           

                            nOrder.TradePrice = 0;
                            nOrder.SalesPrice = 0;
                            nOrder.AlterPrice = 0;
                            nOrder.StartPrice = 0;
                            nOrder.StopPrice = 0;
                            nOrder.DriverPrice = 0;
                            nOrder.Price = 0;
                            nOrder.ClientPrice = 0;
                            //선착불
                            if (_Model.PayLocation == "2")
                            {
                                nOrder.PayLocation = 2;
                                nOrder.StartPrice = int.Parse(_Model.StartPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.StopPrice = int.Parse(_Model.StopPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.DriverPrice = int.Parse(_Model.DriverPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.Price = nOrder.StartPrice + nOrder.StopPrice;
                                nOrder.ClientPrice = nOrder.StartPrice + nOrder.StopPrice - nOrder.DriverPrice;
                            }
                            //인수증
                            else if (_Model.PayLocation == "1")
                            {
                                nOrder.PayLocation = 1;

                                //지불운임
                                nOrder.TradePrice = int.Parse(_Model.TradePrice.Replace(",", "").Replace("-", "0"));

                                //청구운임
                                nOrder.SalesPrice = int.Parse(_Model.SalesPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.AlterPrice = int.Parse(_Model.AlterPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.Price = nOrder.TradePrice;
                                nOrder.ClientPrice = nOrder.SalesPrice;
                            }
                            //인수증+선착불
                            else if (_Model.PayLocation == "4")
                            {
                                nOrder.PayLocation = 4;
                                nOrder.StartPrice = int.Parse(_Model.StartPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.StopPrice = int.Parse(_Model.StopPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.DriverPrice = int.Parse(_Model.DriverPrice.Replace(",", "").Replace("-", "0"));

                                nOrder.TradePrice = int.Parse(_Model.TradePrice.Replace(",", "").Replace("-", "0"));
                                nOrder.SalesPrice = int.Parse(_Model.SalesPrice.Replace(",", "").Replace("-", "0"));
                                nOrder.AlterPrice = int.Parse(_Model.AlterPrice.Replace(",", "").Replace("-", "0"));

                                nOrder.Price = nOrder.TradePrice; //nOrder.StartPrice.Value + nOrder.StopPrice.Value + nOrder.TradePrice.Value;
                                nOrder.ClientPrice = nOrder.SalesPrice; //nOrder.StartPrice.Value + nOrder.StopPrice.Value - nOrder.DriverPrice + nOrder.SalesPrice;

                            }
                            else
                            {

                            }
                        }




                    }

                 

                    if(string.IsNullOrEmpty(_Model.Item) && string.IsNullOrEmpty(_Model.Etc))
                    {
                        nOrder.Item = "화물";
                        nOrder.Remark = "화물";
                    }
                    else if (string.IsNullOrEmpty(_Model.Item) && !string.IsNullOrEmpty(_Model.Etc))
                    {
                        nOrder.Item = "화물";
                        nOrder.Remark = "화물"+" " + _Model.Etc;
                    }
                    else if (!string.IsNullOrEmpty(_Model.Item) && string.IsNullOrEmpty(_Model.Etc))
                    {
                        nOrder.Item = _Model.Item;
                        nOrder.Remark = _Model.Item;
                    }
                    else
                    {
                        nOrder.Item = _Model.Item;
                        nOrder.Remark = _Model.Item + " " + _Model.Etc;
                    }
                    nOrder.RequestMemo = _Model.Etc;

                    nOrder.StartName = _AddressStateParse(_Model.StartState) + " " + nOrder.StartCity;
                    nOrder.StopName = _AddressStateParse(_Model.StopState) + " " + nOrder.StopCity;
                    nOrder.AddressGubun = "S";
                    InitCustomerTable();

                    int _ReferralId = 0;

                    String _Code = "";
                    

                   
                    
                        if (String.IsNullOrEmpty(_Model.ReferralId))
                        {



                            var Query = _CustomerViewModelList.Where(c => c.Name.Contains("차세로")).ToArray();


                            if (Query.Any())
                            {
                                _ReferralId = Query.First().CustomerId;
                                _Code = (Convert.ToInt64(_CustomerViewModelList.First().Code) + 1).ToString();

                            }
                        }
                        else
                        {
                            var _Query = _CustomerViewModelList.Where(c => c.Code == _Model.ReferralId && c.BizGubun == 4).ToArray();

                            if(_Query.Any())
                            {
                                _ReferralId = _Query.First().CustomerId;
                            }
                        }

                    
                    var _CQuery = _CustomerViewModelList.Where(c => c.CustomerId == nOrder.CustomerId).ToArray();
                    nOrder.OrderPhoneNo = _CQuery.First().PhoneNo;
                    nOrder.CustomerManager = _CQuery.First().CustomerManager;

                    nOrder.ReferralId = _ReferralId;
                    if(String.IsNullOrEmpty(nOrder.Item))
                    {
                        nOrder.Item = "화물";
                    }

                    //nOrder.OrdersAcceptId = LocalUser.Instance.LogInInformation.LoginId;
                    nOrder.OrdersLoginId = LocalUser.Instance.LogInInformation.LoginId;

                    orderDataSet.Orders.AddOrdersRow(nOrder);
                }
                ordersTableAdapter.Update(orderDataSet.Orders);

                //EXCELORDERRESULT _Form = new EXCELORDERRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""), orderDataSet.Orders);
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
            public String CustomerName { get; set; }
            public String CustomerBizNo { get; set; }
            public String CustomerTeam { get; set; }
            public String StartTime { get; set; }
            public String StopTime { get; set; }
            public String StartState { get; set; }
            public String StartCity { get; set; }
            public String StartStreet { get; set; }
            public String Start { get; set; }
            public String StopState { get; set; }
            public String StopCity { get; set; }
            public String StopStreet { get; set; }
            public String Stop { get; set; }
            public String Item { get; set; }
            public String ItemSize { get; set; }
            public String ItemCarType { get; set; }
            public String ItemCarSize { get; set; }

            public String ClientPrice { get; set; }
            public String Price { get; set; }

            public String DriverPrice { get; set; }
            public String StopPrice { get; set; }

            public String StartPrice { get; set; }
            public String AlterPrice { get; set; }
            public String SalesPrice { get; set; }
            public String TradePrice { get; set; }


            public String Etc { get; set; }
            //public String DriverCarNo { get; set; }
            //public String DriverName { get; set; }
            //public String DriverBizNo { get; set; }
            //public String DriverMobileNo { get; set; }
            //public String DriverCarType { get; set; }
            //public String DriverCarSize { get; set; }
            public String ReferralId { get; set; }
            public String Error { get; set; } = "";
            public int CarType { get; set; }
            public int CarSize { get; set; }
            //public int DriverId { get; set; }
            public int CustomerId { get; set; }
            public DriverRepository.Driver Driver { get; set; }
            public String Wgubun { get; set; }
            public String UniqueKey { get; set; }
            public String PayLocation { get; set; }
            public String StartName { get; set; }
            public String StopName { get; set; }
            public int FPiS_ID { get; set; }
            

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

        private void button1_Click(object sender, EventArgs e)
        {
            string fileString = "화물정보일괄등록_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.Excel_Order_Import;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            try
            {
                if (file.Exists)
                {
                    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    file.Delete();
                }
                File.WriteAllBytes(FileName, ieExcel);
            }
            catch
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 이름의 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            System.Diagnostics.Process.Start(FileName);
        }
    }
}
