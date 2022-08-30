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
using OfficeOpenXml;
using System.Diagnostics;
using mycalltruck.Admin.Class.DataSet;
using System.Data.Common;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT2 : Form
    {
        string FileName = string.Empty;
        public EXCELINSERT2()
        {
            InitializeComponent();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            fpiS_CONTTableAdapter.Fill(fpisDataSet.FPIS_CONT);
            LocalUser.Instance.LogInInformation.LoadClient();
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
                const int TestIndex = 1;
                while (true)
                {
                    var TestText = _Sheet.Cells[ExcelRowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;
                    int _RowIndex = newDGV1.Rows.Add();
                    for (int i = 0; i < 22; i++)
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
                    IDX = newDGV1[0, i].Value.ToString().Trim(),
                    Date = newDGV1[1, i].Value.ToString().Trim(),
                    Item = newDGV1[2, i].Value.ToString().Trim(),
                    Customer = newDGV1[3, i].Value.ToString().Trim(),
                    BizNo = newDGV1[4, i].Value.ToString().Trim(),
                    Start = newDGV1[5, i].Value.ToString().Trim(),
                    Stop = newDGV1[6, i].Value.ToString().Trim(),
                    Target = newDGV1[7, i].Value.ToString().Trim(),
                    CarNo = newDGV1[8, i].Value.ToString().Trim(),
                    Code = newDGV1[9, i].Value.ToString().Trim(),
                    Client = newDGV1[10, i].Value.ToString().Trim(),
                    CarType = newDGV1[11, i].Value.ToString().Trim(),
                    ClientPriceString = newDGV1[12, i].Value.ToString().Trim(),
                    PriceString = newDGV1[13, i].Value.ToString().Trim().Replace(",", "").Trim(),
                    AddedPrice = newDGV1[14, i].Value.ToString().Replace(",", "").Trim(),
                    AddedCharge = newDGV1[15, i].Value.ToString().Replace(",", "").Trim(),
                    RequestPrice = newDGV1[16, i].Value.ToString().Replace(",", "").Trim(),
                    EndPrice = newDGV1[17, i].Value.ToString().Replace(",", "").Trim(),
                    LastPrice = newDGV1[18, i].Value.ToString().Replace(",", "").Trim(),
                    EndClient = newDGV1[19, i].Value.ToString().Trim(),
                    Memo = newDGV1[20, i].Value.ToString().Trim(),
                    Type1 = newDGV1[21, i].Value.ToString().Trim(),
                };
                if (String.IsNullOrEmpty(nModel.PriceString) || nModel.PriceString == "-")
                    nModel.PriceString = "0";
                if (String.IsNullOrEmpty(nModel.ClientPriceString) || nModel.ClientPriceString == "-")
                    nModel.ClientPriceString = "0";
                #region Idx
                if (_ModelList.Any(c => c.IDX == nModel.IDX))
                {
                    _AppendError(nModel, "IDX 중복");
                }
                #endregion
                #region 화주
                //사업자등록번호 없으면 에러
                if (String.IsNullOrEmpty(nModel.BizNo) || nModel.BizNo.Length != 10)
                {
                    _AppendError(nModel, "화물사업자번호");
                }
                //있으면  찾기
                else
                {
                    nModel.BizNo = nModel.BizNo.Replace("-", "");
                    nModel.BizNo = nModel.BizNo.Substring(0, 3) + "-" + nModel.BizNo.Substring(3, 2) + "-" + nModel.BizNo.Substring(5);
                    if (!TempCustomerDictionary.ContainsKey(nModel.BizNo))
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                var _CommandText = "SELECT CustomerId FROM Customers Where BizNo = @BizNo AND ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@BizNo", nModel.BizNo);
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
                                    TempCustomerDictionary.Add(nModel.BizNo, Convert.ToInt32(o));
                                }
                                else
                                {
                                    using (SqlCommand _InsertCommand = _Connection.CreateCommand())
                                    {
                                        _InsertCommand.CommandText =
                                            @"INSERT INTO Customers (Code,BizNo,SangHo,CreateTime,ClientId) output inserted.CustomerId
                                            SELECT cast(cast(isnull(max(code),'1000') as int)+1 as nvarchar), @BizNo, @SangHo, GETDATE(), @ClientId
                                            From Customers
                                            where ClientId = @ClientId";
                                        _InsertCommand.Parameters.AddWithValue("@BizNo", nModel.BizNo);
                                        _InsertCommand.Parameters.AddWithValue("@SangHo", nModel.Customer);
                                        _InsertCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                        TempCustomerDictionary.Add(nModel.BizNo, Convert.ToInt32(_InsertCommand.ExecuteScalar()));
                                    }
                                }
                            }
                            _Connection.Close();
                        }
                    }
                    nModel.CustomerId = TempCustomerDictionary[nModel.BizNo];
                    if (nModel.CustomerId == 0)
                    {
                        _AppendError(nModel, "화물수탁정보 사업자번호 없음");
                    }
                    else
                    {
                        var CONT_FROM = nModel.Date.Substring(0,4) + "/01/01";
                        var CONT_TO = nModel.Date.Substring(0, 4) + "/12/31";

                        nModel.FPIS_ID = 0;
                        if (fpisDataSet.FPIS_CONT.Any(c => c.CustomerId == nModel.CustomerId))
                            nModel.FPIS_ID = fpisDataSet.FPIS_CONT.First(c => c.CustomerId == nModel.CustomerId).FPIS_ID;
                        else
                        {
                            var nFPIS_CONT = fpisDataSet.FPIS_CONT.NewFPIS_CONTRow();
                            nFPIS_CONT.CL_COMP_GUBUN = "1";
                            nFPIS_CONT.CL_COMP_NM = nModel.Customer;
                            nFPIS_CONT.CL_COMP_CORP_NUM = "";
                            nFPIS_CONT.CL_P_TEL = "";
                            nFPIS_CONT.CONT_GROUP = DateTime.Now.ToString("yyMMddhhmmss");
                            nFPIS_CONT.CONT_AGENCY_USR_MST_KEY = "";
                            nFPIS_CONT.CONT_FROM = CONT_FROM;
                            nFPIS_CONT.CONT_TO = CONT_TO;
                            nFPIS_CONT.CONT_ITEM = "99";
                            nFPIS_CONT.CONT_GOODS_FORM = "99";
                            nFPIS_CONT.CONT_GOODS_UNIT = "99";
                            nFPIS_CONT.CONT_GOODS_CNT = "0";
                            nFPIS_CONT.CONT_START_ADDR = "";
                            nFPIS_CONT.CONT_END_ADDR = "";
                            nFPIS_CONT.CONT_START_ADDR1 = "";
                            nFPIS_CONT.CONT_END_ADDR1 = "";
                            nFPIS_CONT.CONT_DEPOSIT = "0";
                            nFPIS_CONT.CONT_MANG_TYPE = "N";
                            nFPIS_CONT.CREATE_DATE = DateTime.Now;
                            nFPIS_CONT.CliendId = LocalUser.Instance.LogInInformation.ClientId.ToString();
                            nFPIS_CONT.CONT_YN = false;
                            nFPIS_CONT.ONE_GUBUN = "";
                            nFPIS_CONT.CustomerId = nModel.CustomerId;
                            nFPIS_CONT.CL_COMP_BSNS_NUM = nModel.BizNo;
                            fpisDataSet.FPIS_CONT.AddFPIS_CONTRow(nFPIS_CONT);
                            fpiS_CONTTableAdapter.Update(nFPIS_CONT);


                            nModel.FPIS_ID = nFPIS_CONT.FPIS_ID;
                        }
                    }
                }
                #endregion
                #region Date
                DateTime tDate = DateTime.Now;
                if (!DateTime.TryParseExact(nModel.Date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out tDate)                    )
                {
                    _AppendError(nModel, "일자");
                }
                else
                {
                    nModel.StartTime = tDate;
                    nModel.StopTime = tDate;
                }
                #endregion
                #region 운송료
                decimal tPrice = 0;
                if (!decimal.TryParse(nModel.ClientPriceString.Replace(",", ""), out tPrice))
                {
                    _AppendError(nModel, "화주운송료숫자");
                }
                else
                {
                    nModel.ClientPrice = tPrice;
                }
                if (!decimal.TryParse(nModel.PriceString.Replace(",", ""), out tPrice))
                {
                    _AppendError(nModel, "배차운송료숫자");
                }
                else
                {
                    nModel.Price = tPrice;
                }
                #endregion
                #region 차량
                if (String.IsNullOrWhiteSpace(nModel.CarNo) || nModel.CarNo.Trim().Length < 5)
                {
                    _AppendError(nModel, "차량번호");
                }
                else
                {
                    if(!TempDriverDictionary.ContainsKey(nModel.CarNo))
                    {
                        var Driver = mDriverRepository.GetDriver(nModel.CarNo);
                        if (Driver == null)
                            Driver = mDriverRepository.CreateDriver_FPIS(nModel.CarNo);
                        TempDriverDictionary.Add(nModel.CarNo, Driver);
                    }
                    nModel.Driver = TempDriverDictionary[nModel.CarNo];
                }
                #endregion
                #region 출발지 
                if (String.IsNullOrWhiteSpace(nModel.Start))
                {
                    _AppendError(nModel, "상차지 미입력");
                }
                #endregion
                #region 도착지 
                if (String.IsNullOrWhiteSpace(nModel.Stop))
                {
                    _AppendError(nModel, "하차지 미입력");
                }
                #endregion
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
            }
            else
            {
                label4.Text = FCount.ToString() + " 건";
                label5.Text = (FCount - errCount).ToString() + " 건";
                label6.Text = errCount.ToString() + " 건";
                label7.Visible = true;
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
            int ERROR_Index = 23;
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

        private async void  btn_Update_Click(object sender, EventArgs e)
        {
            if (_ModelList.Count == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }
            var TotalCount = 0;
            var SuccessCount = 0;
            this.Cursor = Cursors.WaitCursor;
            BarProgress.Value = TotalCount;
            BarProgress.Maximum = _ModelList.Count;
            PnProgress.Visible = true;
            Task.Factory.StartNew(() =>
            {
                foreach (Model mModel in _ModelList)
                {
                    TotalCount++;
                    Invoke(new Action(() =>
                    {
                        lblProgress.Text = $"{TotalCount.ToString("N0")}/{_ModelList.Count.ToString("N0")}";
                        BarProgress.Value = TotalCount;
                    }));
                    if (!String.IsNullOrEmpty(mModel.Error))
                        continue;

                    var StopTime = mModel.StartTime;
                    var StartTime = mModel.StopTime;
                    var Price = mModel.EndPrice;
                    var ClientPrice = mModel.RequestPrice;
                    var Item = mModel.Item;
                    var ClientId = LocalUser.Instance.LogInInformation.ClientId;
                    var CarNo = mModel.CarNo;
                    var car = mModel.Driver;
                    var CarType = car.CarType;
                    var Driver = car.CarYear;
                    var DriverPhoneNo = car.MobileNo;
                    var DriverCarModel = car.Name;
                    var DriverId = car.DriverId;
                    var CarSize = car.CarSize;
                    var ItemSize = "";
                    if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == CarSize))
                    {
                        ItemSize = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == CarSize).Name;
                    }
                    var StartState = mModel.Start;
                    var StartCity = "";
                    var StopState = mModel.Stop;
                    var StopCity = "";
                    var CarCount = 1;
                    var AcceptTime = DateTime.Now;
                    var OrderPhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;

                    try
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            var Query = @"INSERT INTO Orders
           (StartTime,StartState,StartCity,Item,CreateTime,ClientId,StartDate,CarType,CarSize,StopState,StopCity,CarCount,AcceptTime,Driver,DriverPhoneNo,DriverCarModel,Remark,OrderStatus,DriverId,OrderPhoneNo,StopTime,ItemSize,Price,ClientPrice,DriverCarNo,CustomerId,SourceType,Agubun,Wgubun,Fpis_id)
     VALUES(@StartTime,@StartState,@StartCity,@Item,@CreateTime,@ClientId,@StartDate,@CarType,@CarSize,@StopState,@StopCity,@CarCount,@AcceptTime,@Driver,@DriverPhoneNo
,@DriverCarModel,@Remark,@OrderStatus,@DriverId,@OrderPhoneNo,@StopTime,@ItemSize,@Price,@ClientPrice,@DriverCarNo,@CustomerId,@SourceType,@Agubun,@Wgubun,@FPIS_id)";

                            var Command = cn.CreateCommand();
                            Command.CommandText = Query;

                            Command.Parameters.AddWithValue("@StartTime", StartTime);
                            Command.Parameters.AddWithValue("@StartState", StartState);
                            Command.Parameters.AddWithValue("@StartCity", StartCity);
                            Command.Parameters.AddWithValue("@Item", Item);
                            Command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                            Command.Parameters.AddWithValue("@ClientId", ClientId);
                            Command.Parameters.AddWithValue("@StartDate", StartTime);
                            Command.Parameters.AddWithValue("@CarType", CarType);
                            Command.Parameters.AddWithValue("@CarSize", CarSize);
                            Command.Parameters.AddWithValue("@StopState", StopState);
                            Command.Parameters.AddWithValue("@StopCity", StopCity);
                            Command.Parameters.AddWithValue("@CarCount", CarCount);
                            Command.Parameters.AddWithValue("@AcceptTime", AcceptTime);
                            Command.Parameters.AddWithValue("@Driver", Driver);
                            Command.Parameters.AddWithValue("@DriverPhoneNo", DriverPhoneNo);
                            Command.Parameters.AddWithValue("@DriverCarModel", DriverCarModel);
                            Command.Parameters.AddWithValue("@Remark", "");

                            Command.Parameters.AddWithValue("@OrderStatus", 3);
                            Command.Parameters.AddWithValue("@DriverId", DriverId);

                            Command.Parameters.AddWithValue("@OrderPhoneNo", OrderPhoneNo);
                            Command.Parameters.AddWithValue("@StopTime", StopTime);

                            Command.Parameters.AddWithValue("@ItemSize", ItemSize);
                            Command.Parameters.AddWithValue("@Price", Price);
                            Command.Parameters.AddWithValue("@ClientPrice", ClientPrice);


                            Command.Parameters.AddWithValue("@DriverCarNo", CarNo);

                            Command.Parameters.AddWithValue("@CustomerId", mModel.CustomerId);
                            Command.Parameters.AddWithValue("@SourceType", 0);
                            Command.Parameters.AddWithValue("@Agubun", 3);
                            Command.Parameters.AddWithValue("@Wgubun", "PC");
                            Command.Parameters.AddWithValue("@FPIS_id", mModel.FPIS_ID);
                            Command.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                    catch (Exception _Exception)
                    {
                    }
                    SuccessCount++;
                }
                Invoke(new Action(() =>
                {
                    PnProgress.Visible = false;
                    var ErrorCount = TotalCount - SuccessCount;
                    label4.Text = TotalCount.ToString() + " 건";
                    label5.Text = SuccessCount.ToString() + " 건";
                    label6.Text = ErrorCount.ToString() + " 건";
                    EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
                    _Form.Owner = this;
                    _Form.StartPosition = FormStartPosition.CenterParent;
                    _Form.ShowDialog();
                    this.Close();
                }));
            });
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
            public string IDX { get; set; }
            public string Date { get; set; }
            public string Item { get; set; }
            public string Customer { get; set; }
            public string BizNo { get; set; }
            public string Start { get; set; }
            public string Stop { get; set; }
            public string Target { get; set; }
            public string CarNo { get; set; }
            public string Code { get; set; }
            public string Client { get; set; }
            public string CarType { get; set; }
            public string ClientPriceString { get; set; }
            public string PriceString { get; set; }
            public string AddedPrice { get; set; }
            public string AddedCharge { get; set; }
            public string RequestPrice { get; set; }
            public string EndPrice { get; set; }
            public string LastPrice { get; set; }
            public string EndClient { get; set; }
            public string Memo { get; set; }
            public string Type1 { get; set; }
            public string Error { get; set; }
            public int CustomerId { get; set; }
            public DriverRepository.Driver Driver { get; set; }
            public int FPIS_ID { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime StopTime { get; set; }
            public decimal ClientPrice { get; set; }
            public decimal Price { get; set; }
        }
    }
}
