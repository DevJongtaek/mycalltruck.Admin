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
    public partial class EXCELINSERT_CUSTOMER : Form
    {
        string FileName = string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();
        public EXCELINSERT_CUSTOMER()
        {
            InitializeComponent();
            InitializeStorage();
            SetCustomerList();
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
        class CustomerModel
        {
            public int CustomerId { get; set; }
            public string SangHo { get; set; }
            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }
            public int PointMethod { get; set; }
            public int Fee { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }
            public string BizNo { get; set; }
            public string CustomerManager { get; set; }
            public int BizGubun { get; set; }
        }
        List<CustomerModel> CustomerModelList = new List<CustomerModel>();
        private void SetCustomerList()
        {
            CustomerModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo, Customers.PointMethod, Customers.Fee, Customers.AddressState, Customers.AddressCity, Customers.AddressDetail, Customers.BizNo,Customers.MobileNo,ISNULL(CustomerManager.ManagerName,''),BizGubun FROM Customers " +
                        $" LEFT  JOIN CustomerManager ON Customers.CustomerManagerId = CustomerManager.ManagerId" +

                        $" WHERE Customers.BizNo <> '000-00-00000' AND Customers.ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            CustomerModelList.Add(new CustomerModel
                            {
                                CustomerId = _Reader.GetInt32(0),
                                SangHo = _Reader.GetStringN(1),
                                PhoneNo = _Reader.GetStringN(2),
                                PointMethod = _Reader.GetInt32Z(3),
                                Fee = _Reader.GetInt32Z(4),
                                AddressState = _Reader.GetStringN(5),
                                AddressCity = _Reader.GetStringN(6),
                                AddressDetail = _Reader.GetStringN(7),
                                BizNo = _Reader.GetStringN(8),
                                MobileNo = _Reader.GetStringN(9),
                                CustomerManager = _Reader.GetString(10),
                                BizGubun = _Reader.GetInt32(11),
                            });
                        }
                    }
                }
                _Connection.Close();
            }
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
                        S_Idx = _Sheet.Cells[RowIndex, 1].Text,
                        SBiz_NO = _Sheet.Cells[RowIndex, 2].Text,
                        SName = _Sheet.Cells[RowIndex, 3].Text,
                        SCeo = _Sheet.Cells[RowIndex, 4].Text,
                        SUptae = _Sheet.Cells[RowIndex, 5].Text,
                        SUpjong = _Sheet.Cells[RowIndex, 6].Text,
                        SZip = _Sheet.Cells[RowIndex, 7].Text,
                        SState = _Sheet.Cells[RowIndex, 8].Text,
                        SCity = _Sheet.Cells[RowIndex, 9].Text,
                        SStreet = _Sheet.Cells[RowIndex, 10].Text,

                        SCustomerGubun = _Sheet.Cells[RowIndex, 11].Text,

                        SRegisterNo = _Sheet.Cells[RowIndex, 12].Text,
                        SSalesGubun = _Sheet.Cells[RowIndex, 13].Text,
                        SEmail = _Sheet.Cells[RowIndex,14].Text,
                        SPhoneNo = _Sheet.Cells[RowIndex, 15].Text,
                        SCharge = _Sheet.Cells[RowIndex, 16].Text,
                        SMobileNo = _Sheet.Cells[RowIndex, 17].Text,
                        SMisu = _Sheet.Cells[RowIndex, 18].Text,
                        SMizi = _Sheet.Cells[RowIndex, 19].Text,
                        SRemark = _Sheet.Cells[RowIndex, 20].Text,
                    };
                    _CoreList.Add(Added);
                    RowIndex++;

                    if(RowIndex > 5000)
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
            int ERROR_Index = 21;
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
                        if (String.IsNullOrEmpty(row.SCustomerGubun))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 구분 빈값";
                            }
                            else
                            {
                                ErrorText += "구분 빈값";

                            }
                            RowErrorCount++;

                        }
                        else
                        {
                            if (row.SCustomerGubun == "화주" || row.SCustomerGubun == "운송" || row.SCustomerGubun == "주선" || row.SCustomerGubun == "위탁사" || row.SCustomerGubun == "도착지" || row.SCustomerGubun == "기타")
                            {

                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 구분 오류";
                                }
                                else
                                {
                                    ErrorText += "구분 오류";

                                }
                                RowErrorCount++;
                            }



                        }

                        if (row.SCustomerGubun == "도착지")
                        {
                            SetCustomerList();
                            Int64 BizNo = 9999900000;

                            var Query = CustomerModelList.Where(c => c.BizGubun == 5).OrderByDescending(c => c.BizNo.Replace("-", "")).ToArray();

                            if (Query.Any())
                            {
                                BizNo = Convert.ToInt64(Query.First().BizNo.Replace("-", "")) + Convert.ToInt64(row.S_Idx);

                            }
                            else
                            {
                                BizNo = BizNo + Convert.ToInt64(row.S_Idx);
                            }

                            row.SBiz_NO = BizNo.ToString();

                            if (String.IsNullOrWhiteSpace(row.SCeo))
                            {
                                row.SCeo = ".";
                            }
                            if (String.IsNullOrWhiteSpace(row.SUptae))
                            {
                                row.SUptae = ".";
                            }

                            if (String.IsNullOrWhiteSpace(row.SUpjong))
                            {
                                row.SUpjong = ".";
                            }
                            if (String.IsNullOrWhiteSpace(row.SEmail))
                            {
                                row.SEmail = "111@naver.com";
                            }
                            if (String.IsNullOrWhiteSpace(row.SMobileNo))
                            {
                                row.SMobileNo = "010-0000-0000";
                            }
                            if (String.IsNullOrWhiteSpace(row.SZip))
                            {
                                row.SZip = "00000";
                            }
                            row.SMisu = "0";
                            row.SMizi = "0";
                            row.SSalesGubun = "3";
                        }

                        #region
                        //사업자등록번호 없으면 에러
                        if (String.IsNullOrEmpty(row.SBiz_NO) || !Regex.IsMatch(row.SBiz_NO.Replace("-", ""), @"^\d{10}$"))
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
                            if (BizNoCompareTable.Contains(row.SBiz_NO.Replace("-", "")))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 사업자번호 중복";
                                }
                                else
                                {
                                    ErrorText += "사업자번호 중복";

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
                                ErrorText += " | 종목 빈값";
                            }
                            else
                            {
                                ErrorText += "종목 빈값";

                            }
                            RowErrorCount++;
                        }


                        if (String.IsNullOrEmpty(row.SZip))
                        {
                            row.SZip = "";

                        }


                        //시도
                        if (String.IsNullOrEmpty(row.SState))
                        {
                            row.SState = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 시도 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "시도 빈값";

                            //}
                            //RowErrorCount++;
                        }

                        //시군구
                        if (String.IsNullOrEmpty(row.SCity))
                        {
                            row.SCity = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 시군구 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "시군구 빈값";

                            //}
                            //RowErrorCount++;
                        }


                        //상세주소
                        if (String.IsNullOrEmpty(row.SStreet))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 상세주소 빈값";
                            }
                            else
                            {
                                ErrorText += "상세주소 빈값";

                            }
                            RowErrorCount++;
                        }
                        #region 거래처구분
                        if (row.SSalesGubun != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "SalesGubun" && c.Value == IntNull.Parse(row.SSalesGubun)).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 거래처구분 코드 오류.";
                                }
                                else
                                {
                                    ErrorText += "거래처구분 코드 오류.";

                                }
                                RowErrorCount++;
                            }
                        }
                        else
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 거래처구분 빈값.";
                            }
                            else
                            {
                                ErrorText += "거래처구분 빈값.";

                            }
                            RowErrorCount++;
                        }
                        #endregion
                        //전화번호
                        if (String.IsNullOrEmpty(row.SPhoneNo))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 전화번호 빈값 ";
                            }
                            else
                            {
                                ErrorText += "전화번호 빈값";

                            }
                            RowErrorCount++;
                        }

                        if (String.IsNullOrWhiteSpace(row.SEmail))
                        {
                            row.SEmail = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | E-메일 ";
                            //}
                            //else
                            //{
                            //    ErrorText += "E-메일";

                            //}
                            //RowErrorCount++;

                        }
                        //else if(!Regex.Match(row.SEmail, @"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$").Success)
                        //{
                        //    row.SEmail = "";
                        //}

                        if (!String.IsNullOrEmpty(row.SMobileNo))
                        {
                           
                            //if (!Regex.Match(row.SMobileNo.Replace("-",""), @"01[016789]\d{3,4}\d{4,4}").Success)
                            //{
                            //    if (RowErrorCount > 0)
                            //    {
                            //        ErrorText += " | 핸드폰번호 형식 ";
                            //    }
                            //    else
                            //    {
                            //        ErrorText += "핸드폰번호 형식";

                            //    }

                            //    RowErrorCount++;
                            //}
                        }

                        if (String.IsNullOrEmpty(row.SMisu))
                        {
                            row.SMisu = "0";
                        }
                        else if (!String.IsNullOrEmpty(row.SMisu))
                        {

                            if (!Int64.TryParse(row.SMisu.Replace(",", ""), out long _Smisu))
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
                            row.SMizi = "0";
                        }
                        else if (!String.IsNullOrEmpty(row.SMizi))
                        {

                            if (!Int64.TryParse(row.SMizi.Replace(",", ""), out long _SMizi))
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
                        #endregion








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
            btn_Info.Enabled = false;
            btn_Test.Enabled = false;
            btn_OK.Enabled = false;
            cmb_Savegubun.Enabled = false;
            btn_Update.Enabled = false;
            btn_Close.Enabled = false;
            pnProgress.Visible = true;

            bar.Value = 0;
            bar.Maximum = _CoreList.Count;

            Task.Factory.StartNew(() =>
            {

                foreach (Model row in _CoreList)
                {
                    int _BizGubun = 0;
                    var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NCustomerGubun" && c.Name == row.SCustomerGubun).ToArray();
                    if(Query.Any())
                    {
                        _BizGubun = Query.First().Value;
                    }
                   

                    bar.Invoke(new Action(() =>
                    {
                        bar.Value++;
                    }));


                    InitCompareTable();
                    if (BizNoCompareTable.Contains(row.SBiz_NO.Replace("-", "")))
                    {
                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "Update Customers" +
                                " SET SangHo = @SangHo" +
                                ",Ceo = @Ceo" +
                                ",Uptae = @Uptae" +
                                ",Upjong=@Upjong" +
                                ",AddressState = @AddressState,AddressCity = @AddressCity" +
                                ",AddressDetail = @AddressDetail" +
                                ",BizGubun = @BizGubun" +
                                ",ResgisterNo = @ResgisterNo" +
                                ",SalesGubun = @SalesGubun" +
                                ",Email=@Email" +
                                ",PhoneNo = @PhoneNo" +
                                ",FaxNo= N''" +
                                ",ChargeName=@ChargeName" +
                                ",MobileNo = @MobileNo" +
                                //",CreateTime = @getdate()" +

                                ",Zipcode =@Zipcode" +
                                ",SubClientId=@SubClientId" +
                                ",ClientUserId=@ClientUserId" +
                                ",Misu=@Misu" +
                                ",Mizi = Mizi " +
                                ",Remark = @Remark" +
                                " WHERE REPLACE(BizNo,'-','') = @BizNo ";
                                _Command.Parameters.AddWithValue("@SangHo", row.SName);
                                _Command.Parameters.AddWithValue("@Ceo", row.SCeo);
                                _Command.Parameters.AddWithValue("@Uptae", row.SUptae);
                                _Command.Parameters.AddWithValue("@Upjong", row.SUpjong);
                                _Command.Parameters.AddWithValue("@AddressState", row.SState);
                                _Command.Parameters.AddWithValue("@AddressCity", row.SCity);
                                _Command.Parameters.AddWithValue("@AddressDetail", row.SStreet);
                                _Command.Parameters.AddWithValue("@Zipcode", row.SZip);
                                _Command.Parameters.AddWithValue("@ResgisterNo", row.SRegisterNo);
                                _Command.Parameters.AddWithValue("@SalesGubun", int.Parse(row.SSalesGubun));
                                _Command.Parameters.AddWithValue("@Email", row.SEmail);
                                _Command.Parameters.AddWithValue("@PhoneNo", row.SPhoneNo.Replace("-", ""));
                                _Command.Parameters.AddWithValue("@ChargeName", row.SCharge);
                                _Command.Parameters.AddWithValue("@MobileNo", row.SPhoneNo.Replace("-", ""));
                                _Command.Parameters.AddWithValue("@SubClientId", DBNull.Value);
                                _Command.Parameters.AddWithValue("@ClientUserId", DBNull.Value);
                                
                                _Command.Parameters.AddWithValue("@BizGubun", _BizGubun);


                                decimal _Misu = Convert.ToDecimal(row.SMisu);
                                decimal _Mizi = Convert.ToDecimal(row.SMizi);
                                _Command.Parameters.AddWithValue("@Misu", _Misu);
                                _Command.Parameters.AddWithValue("@Mizi", _Mizi);

                                _Command.Parameters.AddWithValue("@Remark", row.SRemark);


                                _Command.Parameters.AddWithValue("@BizNo", row.SBiz_NO.Replace("-", ""));




                                _Command.ExecuteNonQuery();
                            }
                            CustomerRepository mCustomerRepository = new CustomerRepository();

                            var _CustomerId = mCustomerRepository.GetCustomerId(row.SBiz_NO);
                            using (SqlCommand _DICommand = _Connection.CreateCommand())
                            {
                                _DICommand.CommandText = "Update Customers SET Misu = @Misu,Mizi = @Mizi,MStartDate = getdate(),MStart = @Mizi WHERE CustomerId = @CustomerId AND ClientId = @ClientId ";
                                decimal _Misu = Convert.ToDecimal(row.SMisu);
                                decimal _Mizi = Convert.ToDecimal(row.SMizi);
                                _DICommand.Parameters.AddWithValue("@Misu", _Misu);
                                _DICommand.Parameters.AddWithValue("@Mizi", _Mizi);
                                _DICommand.Parameters.AddWithValue("@CustomerId", _CustomerId);
                                _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            }
                        });
                    }
                    else
                    {
                        InitCompareTable();

                        int _CustomerId = 0;
                        var InsertQuery =
                       @"INSERT INTO [dbo].[Customers](Code,BizNo,SangHo,Ceo,Uptae,Upjong,AddressState,AddressCity,AddressDetail,BizGubun,ResgisterNo,SalesGubun,Email,PhoneNo,FaxNo,ChargeName,MobileNo,CreateTime,ClientId,Zipcode,SubClientId,ClientUserId,Misu,Mizi,MStartDate,Mstart,Remark)
                VALUES(@Code,@BizNo,@SangHo,@Ceo,@Uptae,@Upjong,@AddressState,@AddressCity,@AddressDetail,@BizGubun,@ResgisterNo,@SalesGubun,@Email,@PhoneNo,N'',@ChargeName,@MobileNo,GetDate(),@ClientId,@Zipcode,@SubClientId,@ClientUserId,@Misu,@Mizi,getdate(),@Mizi,@Remark)" +
                     "  SELECT @@IDENTITY ";


                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = InsertQuery;
                                _Command.Parameters.Add("@Code", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@SangHo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@Ceo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@Uptae", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@Upjong", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@AddressState", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@AddressCity", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@AddressDetail", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@ResgisterNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@SalesGubun", SqlDbType.Int);
                                _Command.Parameters.Add("@Email", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@PhoneNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@ChargeName", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@MobileNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                                _Command.Parameters.Add("@Zipcode", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@SubClientId", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@ClientUserId", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@Misu", SqlDbType.Decimal);
                                _Command.Parameters.Add("@Mizi", SqlDbType.Decimal);
                                _Command.Parameters.Add("@Remark", SqlDbType.NText);
                                _Command.Parameters.Add("@BizGubun", SqlDbType.Int);




                                if (row.S_Idx != "" && row.Error == "")
                                {
                                    int CustomerCode = 1001;
                                    while (true)
                                    {
                                        if (!CodeCompareTable.Any(c => c == CustomerCode.ToString()))
                                        {
                                            break;
                                        }
                                        CustomerCode++;
                                    }
                                    _Command.Parameters["@Code"].Value = CustomerCode.ToString();
                                    string _BizNo = row.SBiz_NO.Replace("-", "");
                                    _BizNo = _BizNo.Substring(0, 3) + "-" + _BizNo.Substring(3, 2) + "-" + _BizNo.Substring(5);
                                    _Command.Parameters["@BizNo"].Value = _BizNo;
                                    _Command.Parameters["@SangHo"].Value = row.SName;
                                    _Command.Parameters["@Ceo"].Value = row.SCeo;
                                    _Command.Parameters["@Uptae"].Value = row.SUptae;
                                    _Command.Parameters["@Upjong"].Value = row.SUpjong;
                                    _Command.Parameters["@AddressState"].Value = row.SState;
                                    _Command.Parameters["@AddressCity"].Value = row.SCity;
                                    _Command.Parameters["@AddressDetail"].Value = row.SStreet;
                                    _Command.Parameters["@Zipcode"].Value = row.SZip;
                                    _Command.Parameters["@ResgisterNo"].Value = row.SRegisterNo;
                                    _Command.Parameters["@SalesGubun"].Value = int.Parse(row.SSalesGubun);
                                    _Command.Parameters["@Email"].Value = row.SEmail;
                                    String _PhoneNo = row.SPhoneNo.Replace("-", "");
                                    if (_PhoneNo.StartsWith("02"))
                                    {
                                        if (_PhoneNo.Length == 9)
                                        {
                                            _PhoneNo = _PhoneNo.Substring(0, 2) + "-" + _PhoneNo.Substring(2, 3) + "-" + _PhoneNo.Substring(5);
                                        }
                                        else if (_PhoneNo.Length == 10)
                                        {
                                            _PhoneNo = _PhoneNo.Substring(0, 2) + "-" + _PhoneNo.Substring(2, 4) + "-" + _PhoneNo.Substring(6);
                                        }
                                    }
                                    else
                                    {
                                        if (_PhoneNo.Length == 10)
                                        {
                                            _PhoneNo = _PhoneNo.Substring(0, 3) + "-" + _PhoneNo.Substring(3, 3) + "-" + _PhoneNo.Substring(6);
                                        }
                                        else if (_PhoneNo.Length == 11)
                                        {
                                            _PhoneNo = _PhoneNo.Substring(0, 3) + "-" + _PhoneNo.Substring(3, 4) + "-" + _PhoneNo.Substring(7);
                                        }
                                    }
                                    _Command.Parameters["@PhoneNo"].Value = _PhoneNo;
                                    _Command.Parameters["@ChargeName"].Value = row.SCharge;
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
                                    _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                                    _Command.Parameters["@SubClientId"].Value = DBNull.Value;
                                    _Command.Parameters["@ClientUserId"].Value = DBNull.Value;
                                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                                    {
                                        _Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                                        if (LocalUser.Instance.LogInInformation.IsAgent)
                                        {
                                            _Command.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                                        }
                                    }
                                    decimal _Misu = Convert.ToDecimal(row.SMisu);
                                    decimal _Mizi = Convert.ToDecimal(row.SMizi);
                                    _Command.Parameters["@Misu"].Value = _Misu;
                                    _Command.Parameters["@Mizi"].Value = _Mizi;
                                    _Command.Parameters["@Remark"].Value = row.SRemark;
                                    _Command.Parameters["@BizGubun"].Value = _BizGubun;
                                    Object O = _Command.ExecuteScalar();
                                    if (O != null)
                                        _CustomerId = Convert.ToInt32(O);
                                   // _Command.ExecuteNonQuery();
                                }

                            }


                            //using (SqlCommand _DICommand = _Connection.CreateCommand())
                            //{
                            //    if (row.S_Idx != "" && row.Error == "")
                            //    {
                            //        decimal _Misu = Convert.ToDecimal(row.SMisu);
                            //        decimal _Mizi = Convert.ToDecimal(row.SMizi);
                            //        _DICommand.CommandText = @"INSERT INTO Customers (CustomerId, ClientId,Misu,Mizi,MStartDate,Mstart) VALUES (@CustomerId, @ClientId,@Misu,@Mizi,getdate(),@Mizi)";

                            //        _DICommand.Parameters.AddWithValue("@Misu", _Misu);
                            //        _DICommand.Parameters.AddWithValue("@Mizi", _Mizi);
                            //        _DICommand.Parameters.AddWithValue("@CustomerId", _CustomerId);
                            //        _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            //        _DICommand.ExecuteNonQuery();
                            //    }
                            //}
                            _Connection.Close();
                        }
                    }
                }


                Invoke(new Action(() =>
                {
                    EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
                    _Form.Owner = this;
                    _Form.StartPosition = FormStartPosition.CenterParent;
                    _Form.ShowDialog();
                    this.Close();

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
        #endregion

        #region STORAGE
        class Model : INotifyPropertyChanged
        {
            private String _S_Idx = "";
            private String _SBiz_NO = "";
            private String _SName = "";
            private String _SCeo = "";
            private String _SUptae = "";
            private String _SUpjong = "";
            private String _SZip = "";
            private String _SState = "";
            private String _SCity = "";
            private String _SStreet = "";
            private String _SRegisterNo = "";
            private String _SSalesGubun = "";
            private String _SPhoneNo = "";
            private String _SCharge = "";
            private String _SMobileNo = "";
            private String _SEmail = "";
            private String _SMizi = "";
            private String _SMisu = "";
            private string _SRemark = "";
            private String _Error = "";
            private String _SCustomerGubun = "";
            
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
            public string SZip
            {
                get
                {
                    return _SZip;
                }

                set
                {
                    SetField(ref _SZip, value);
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
            public string SCustomerGubun
            {
                get
                {
                    return _SCustomerGubun;
                }

                set
                {
                    SetField(ref _SCustomerGubun, value);
                }
            }
            


            public string SRegisterNo
            {
                get
                {
                    return _SRegisterNo;
                }

                set
                {
                    SetField(ref _SRegisterNo, value);
                }
            }
            public string SSalesGubun
            {
                get
                {
                    return _SSalesGubun;
                }

                set
                {
                    SetField(ref _SSalesGubun, value);
                }
            }
            public string SPhoneNo
            {
                get
                {
                    return _SPhoneNo;
                }

                set
                {
                    SetField(ref _SPhoneNo, value);
                }
            }
            public string SCharge
            {
                get
                {
                    return _SCharge;
                }

                set
                {
                    SetField(ref _SCharge, value);
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
            public string SEmail
            {
                get
                {
                    return _SEmail;
                }

                set
                {
                    SetField(ref _SEmail, value);
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


            public string SRemark
            {
                get
                {
                    return _SRemark;
                }

                set
                {
                    SetField(ref _SRemark, value);
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
