using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using OfficeOpenXml;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_DRIVER : Form
    {
        string FileName = string.Empty;
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        string ERROR=string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        private List<String> CarNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();
        public EXCELINSERT_DRIVER()
        {
            InitializeComponent();
            InitializeStorage();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
            cmb_Savegubun.SelectedIndex = 0;
        }

        private void ExcelTest()
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
                const int TestIndex = 2;
                int RowIndex = 3;
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
                        SUptae = _Sheet.Cells[RowIndex, 4].Text,
                        SUpjong = _Sheet.Cells[RowIndex, 5].Text,
                        SCeo = _Sheet.Cells[RowIndex, 6].Text,
                        SCeoBirth = _Sheet.Cells[RowIndex, 7].Text,
                        SMobileNo = _Sheet.Cells[RowIndex, 8].Text,
                        SPhoneNo = _Sheet.Cells[RowIndex, 9].Text,
                        SFaxNo = _Sheet.Cells[RowIndex, 10].Text,
                        SEmail = _Sheet.Cells[RowIndex, 11].Text,
                        SState = _Sheet.Cells[RowIndex, 12].Text,
                        SCity = _Sheet.Cells[RowIndex, 13].Text,
                        SStreet = _Sheet.Cells[RowIndex, 14].Text,
                        SBizGubun = _Sheet.Cells[RowIndex, 15].Text,
                        SRouteType = _Sheet.Cells[RowIndex, 16].Text,
                        SInsurance = _Sheet.Cells[RowIndex, 17].Text,
                        SCarNo = _Sheet.Cells[RowIndex, 18].Text,
                        SCarType = _Sheet.Cells[RowIndex, 19].Text,
                        SCarSize = _Sheet.Cells[RowIndex, 20].Text,
                        SCarGubun = _Sheet.Cells[RowIndex, 21].Text,
                        SCarYear = _Sheet.Cells[RowIndex, 22].Text,
                        SPayBankName = _Sheet.Cells[RowIndex, 23].Text,
                        SPayAccountNo = _Sheet.Cells[RowIndex, 24].Text,
                        SInputName = _Sheet.Cells[RowIndex, 25].Text,
                        SCarstate = _Sheet.Cells[RowIndex, 26].Text,
                        SCarcity = _Sheet.Cells[RowIndex, 27].Text,
                        SCarStreet = _Sheet.Cells[RowIndex, 28].Text,
                        SfpisCartype = _Sheet.Cells[RowIndex, 29].Text,
                    };
                    _CoreList.Add(Added);
                    RowIndex++;
                }
                label4.Text = "0";
                label5.Text = "0";
                label6.Text = "0";
            }
        }

        private void btn_Info_Click(object sender, EventArgs e)
        {
            ExcelTest();

        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            InitCompareTable();
            //bar.Value = 0;
            List<string> VisibleOrderIds = new List<string>();
            if (newDGV1.Rows.Count == 0)
            {
                MessageBox.Show("검증할 데이터가 없습니다.");
                return;

            }
            FCount = 0;
            errCount = 0;


           
            int RowErrorCount = 0;

          
            foreach (Model row in _CoreList)
            {
                RowErrorCount = 0;
                ErrorText = "";
                Idx = row.S_Idx;

                if (Idx != "")
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
                    //사업자등록번호 없으면 에러
                    else if (String.IsNullOrEmpty(row.SBiz_NO) || row.SBiz_NO.Length != 10)
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
                    //상호 없으면 에러
                    else if (String.IsNullOrEmpty(row.SName))
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
                    else if (String.IsNullOrEmpty(row.SUptae))
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
                    else if (String.IsNullOrEmpty(row.SUpjong))
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
                    //대표자
                    else if (String.IsNullOrEmpty(row.SCeo))
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
                    //대표자생년월일
                    else if (String.IsNullOrEmpty(row.SCeoBirth))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 대표자생년월일 빈값 ";
                        }
                        else
                        {
                            ErrorText += "대표자생년월일 빈값";

                        }
                        RowErrorCount++;
                    }
                    //핸드폰번호
                    else if (String.IsNullOrEmpty(row.SCeoBirth))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 핸드폰번호 빈값 ";
                        }
                        else
                        {
                            ErrorText += "핸드폰번호 빈값";

                        }
                        RowErrorCount++;
                    }
                    //시도
                    else if (String.IsNullOrEmpty(row.SState))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 시도 빈값";
                        }
                        else
                        {
                            ErrorText += "시도 빈값";

                        }
                        RowErrorCount++;
                    }
                    //시군구
                    else if (String.IsNullOrEmpty(row.SCity))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 시군구 빈값";
                        }
                        else
                        {
                            ErrorText += "시군구 빈값";

                        }
                        RowErrorCount++;
                    }
                    //상세주소
                    else if (String.IsNullOrEmpty(row.SUpjong))
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
                    //차량번호
                    else if (String.IsNullOrEmpty(row.SCarNo))
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
                    //차종
                    else if (String.IsNullOrEmpty(row.SCarType))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 차종 빈값";
                        }
                        else
                        {
                            ErrorText += "차종 빈값";

                        }
                        RowErrorCount++;
                    }
                    //톤수
                    else if (String.IsNullOrEmpty(row.SCarSize))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 톤수 빈값";
                        }
                        else
                        {
                            ErrorText += "톤수 빈값";

                        }
                        RowErrorCount++;
                    }
                    //구분
                    else if (String.IsNullOrEmpty(row.SCarGubun))
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
                    else if (String.IsNullOrEmpty(row.SCarYear))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 기사명 빈값";
                        }
                        else
                        {
                            ErrorText += "기사명 빈값";

                        }
                        RowErrorCount++;
                    }
                    //은행
                    else if (String.IsNullOrEmpty(row.SPayBankName))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 은행 빈값";
                        }
                        else
                        {
                            ErrorText += "은행 빈값";

                        }
                        RowErrorCount++;
                    }
                    //계좌번호
                    else if (String.IsNullOrEmpty(row.SPayAccountNo))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 계좌번호 빈값";
                        }
                        else
                        {
                            ErrorText += "계좌번호 빈값";

                        }
                        RowErrorCount++;
                    }
                    else if (String.IsNullOrEmpty(row.SInputName))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 예금주 빈값";
                        }
                        else
                        {
                            ErrorText += "예금주 빈값";

                        }
                        RowErrorCount++;
                    }
                    else if (String.IsNullOrEmpty(row.SCarstate))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 차고지(시도) 빈값.";
                        }
                        else
                        {
                            ErrorText += "차고지(시도) 빈값.";

                        }
                        RowErrorCount++;
                    }
                    //차종
                    else if (String.IsNullOrEmpty(row.SCarcity))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 차고지(시군구) 빈값";
                        }
                        else
                        {
                            ErrorText += "차고지(시군구) 빈값";

                        }
                        RowErrorCount++;
                    }
                    //차종
                    else if (String.IsNullOrEmpty(row.SCarStreet))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 차고지(읍면동) 빈값";
                        }
                        else
                        {
                            ErrorText += "차고지(읍면동) 빈값";

                        }
                        RowErrorCount++;
                    }
                    //차종
                    else if (String.IsNullOrEmpty(row.SfpisCartype))
                    {
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " | 국토부차종 빈값";
                        }
                        else
                        {
                            ErrorText += "국토부차종 빈값";

                        }
                        RowErrorCount++;
                    }
                    //있으면  찾기
                    else
                    {
                        #region 사업자구분

                        if (row.SBizGubun != "")
                        {

                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BizType" && c.Name == row.SBizGubun).ToArray();

                            if (Query.Any())
                            {

                              //  Row.BizType = Query.First().Value;
                            }

                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 사업자구분 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  사업자구분 없음.";

                                }
                                RowErrorCount++;
                            }
                        }
                        #endregion
                        #region 운행노선
                        if (row.SRouteType != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "RouteType" && c.Name == row.SRouteType).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 운행노선 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  운행노선 없음.";

                                }
                                RowErrorCount++;
                            }
                        }
                        #endregion
                        #region 적재물보험
                        if (row.SInsurance != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InsuranceType" && c.Name == row.SInsurance).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 적재물보험 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  적재물보험 없음.";

                                }
                                RowErrorCount++;
                            }
                        }
                        else
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 등록된 적재물보험 없음.";
                            }
                            else
                            {
                                ErrorText += "등록된  적재물보험 없음.";

                            }
                            RowErrorCount++;
                        }
                        #endregion
                        #region 차종
                        if (row.SCarType != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && c.Name == row.SCarType).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 차종 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  차종 없음.";

                                }
                                RowErrorCount++;
                            }


                        }
                        #endregion
                        #region 톤수
                        if (row.SCarSize != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Name == row.SCarSize).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 톤수 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  톤수 없음.";

                                }
                                RowErrorCount++;
                            }
                        }
                        #endregion
                        #region 구분
                        if (row.SCarGubun != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarGubun" && c.Name == row.SCarGubun).ToArray();
                            if (Query.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 구분 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  구분 없음.";

                                }
                                RowErrorCount++;
                            }
                        }
                        #endregion
                        #region 은행명
                        if (row.SPayBankName != "")
                        {

                            var BankName = Filter.Bank.BankList.Where(c => c.Text == row.SPayBankName).ToArray();
                            if (BankName.Any())
                            {
                            }
                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 은행 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  은행 없음.";

                                }
                                RowErrorCount++;
                            }
                        }
                        #endregion
                        #region 국토부차종
                        if (row.SfpisCartype != "")
                        {

                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "FPisCarType" && c.Name == row.SfpisCartype).ToArray();
                            if (Query.Any())
                            {
                            }

                            else
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 등록된 국토부차종 없음.";
                                }
                                else
                                {
                                    ErrorText += "등록된  국토부차종 없음.";

                                }
                                RowErrorCount++;
                            }
                        }
                        #endregion
                        if (!String.IsNullOrEmpty(row.SCarNo))
                        {
                            if (CarNoCompareTable.Any(c=>c == row.SCarNo.Replace(" " ,  "")))
                            {
                                if (RowErrorCount > 0)
                                {
                                    ErrorText += " | 차량번호 중복";
                                }
                                else
                                {
                                    ErrorText += "차량번호 중복";

                                }
                                RowErrorCount++;
                            }
                            else
                            {
                                CarNoCompareTable.Add(row.SCarNo.Replace(" ", ""));
                            }
                        }
                        string sCode;

                        if (CodeCompareTable.Any())
                        {
                            var DriverCode = 100001;
                            while (true)
                            {
                                if (!CodeCompareTable.Any(c => c == DriverCode.ToString()))
                                {
                                    break;
                                }
                                DriverCode++;
                            }
                            sCode = DriverCode.ToString();
                        }
                        else
                        {
                            sCode = "100001";
                        }
                        CodeCompareTable.Add(sCode);
                        CodeCompareTable.Sort();
                        string S_BizNo = string.Empty;
                        string BiznoId = string.Empty;
                        string sPassword = string.Empty;

                        if (row.SBiz_NO.Length == 10)
                        {
                            S_BizNo = row.SBiz_NO.Substring(5, 5);
                        }
                        else if (row.SBiz_NO.Length == 12)
                        {
                            S_BizNo = row.SBiz_NO.Substring(7, 5);
                        }



                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();

                            SqlCommand selectCmd = new SqlCommand(
                                @"SELECT top 1 convert(int,right(LoginId,3)+1) as LoginId FROM Drivers Where right(BizNo,5) = @Bizno order by loginid desc", cn);
                            selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
                            var Reader = selectCmd.ExecuteReader();
                            while (Reader.Read())
                            {

                                BiznoId = Reader["LoginId"].ToString();

                            }
                            if (String.IsNullOrEmpty(BiznoId))
                            {
                                BiznoId = "";

                            }
                            cn.Close();
                        }
                        string BizNo = string.Empty;
                        if (BiznoId == "")
                        {
                            BizNo = "001";

                        }
                        else if (BiznoId.Length == 2)
                        {
                            BizNo = "0" + BiznoId;
                        }
                        else if (BiznoId.Length == 1)
                        {
                            BizNo = "00" + BiznoId;
                        }
                        else if (BiznoId.Length == 3)
                        {
                            BizNo = BiznoId;
                        }


                        string sLoginId = "m" + S_BizNo + BizNo;

                        if (row.SMobileNo.Length > 10)
                        {
                            sPassword = row.SMobileNo.Substring(row.SMobileNo.Length - 4, 4);
                        }
                    }
                    if (RowErrorCount > 0)
                    {
                        errCount++;
                    }
                }
                row.Error = ErrorText;
            }
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
            DataCount = FCount - errCount;
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
            int ERROR_Index = 30;
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

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            var InsertQuery =
                @"INSERT INTO [dbo].[Drivers](BizType,RouteType,InsuranceType,CarType,CarSize,CarGubun,PayBankName,PayBankCode,PayInputName,PayAccountNo,FpisCarType,BizNo,Name,CEO,Uptae,Upjong,Email,MobileNo,PhoneNo,FaxNo,AddressState,AddressCity,AddressDetail,CEOBirth,CarNo,CarYear,ParkState,ParkCity,ParkStreet,Code,LoginId,[Password],CreateDate,AccountOwner,AccountRegNo,BankName,AccountNo,AccountExtra,UsePayNow,ClientBizType,CandidateId,Car_ContRact,AccountUse,DTGUse,FPISUse,MyCallUSe,OTGUse,ServicePrice,useTax,OTGPrice,AccountPrice,FPISPrice,MyCallPrice,DTGPrice,LG_MertKeyYn,ServiceState,RequestFrom,RequestTo,SubClientId)
                VALUES(@BizType,@RouteType,@InsuranceType,@CarType,@CarSize,@CarGubun,@PayBankName,@PayBankCode,@PayInputName,@PayAccountNo,@FpisCarType,@BizNo,@Name,@CEO,@Uptae,@Upjong,@Email,@MobileNo,@PhoneNo,@FaxNo,@AddressState,@AddressCity,@AddressDetail,@CEOBirth,@CarNo,@CarYear,@ParkState,@ParkCity,@ParkStreet,@Code,@LoginId,@Password,@CreateDate,@AccountOwner,@AccountRegNo,@BankName,@AccountNo,@AccountExtra,@UsePayNow,@ClientBizType,@CandidateId,@Car_ContRact,@AccountUse,@DTGUse,@FPISUse,@MyCallUSe,@OTGUse,@ServicePrice,@useTax,@OTGPrice,@AccountPrice,@FPISPrice,@MyCallPrice,@DTGPrice,@LG_MertKeyYn,@ServiceState,@RequestFrom,@RequestTo,@SubClientId)";


            InitCompareTable();
            FCount = 0;
            errCount = 0;
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
                    _Command.Parameters.Add("@BizType", SqlDbType.Int);
                    _Command.Parameters.Add("@RouteType", SqlDbType.Int);
                    _Command.Parameters.Add("@InsuranceType", SqlDbType.Int);
                    _Command.Parameters.Add("@CarType", SqlDbType.Int);
                    _Command.Parameters.Add("@CarSize", SqlDbType.Int);
                    _Command.Parameters.Add("@CarGubun", SqlDbType.Int);
                    _Command.Parameters.Add("@PayBankName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayBankCode", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayInputName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PayAccountNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@FpisCarType", SqlDbType.Int);
                    _Command.Parameters.Add("@BizNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Name", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CEO", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Uptae", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Upjong", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Email", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@MobileNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@PhoneNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@FaxNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AddressState", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AddressCity", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AddressDetail", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CEOBirth", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CarNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CarYear", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ParkState", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ParkCity", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@ParkStreet", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Code", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@LoginId", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@Password", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
                    _Command.Parameters.Add("@AccountOwner", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AccountRegNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@BankName", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AccountNo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@AccountExtra", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@UsePayNow", SqlDbType.Int);
                    _Command.Parameters.Add("@ClientBizType", SqlDbType.Int);
                    _Command.Parameters.Add("@CandidateId", SqlDbType.Int);
                    _Command.Parameters.Add("@Car_ContRact", SqlDbType.Bit);
                    _Command.Parameters.Add("@AccountUse", SqlDbType.Bit);
                    _Command.Parameters.Add("@DTGUse", SqlDbType.Bit);
                    _Command.Parameters.Add("@FPISUse", SqlDbType.Bit);
                    _Command.Parameters.Add("@MyCallUSe", SqlDbType.Bit);
                    _Command.Parameters.Add("@OTGUse", SqlDbType.Bit);
                    _Command.Parameters.Add("@ServicePrice", SqlDbType.Int);
                    _Command.Parameters.Add("@useTax", SqlDbType.Bit);
                    _Command.Parameters.Add("@OTGPrice", SqlDbType.Int);
                    _Command.Parameters.Add("@AccountPrice", SqlDbType.Int);
                    _Command.Parameters.Add("@FPISPrice", SqlDbType.Int);
                    _Command.Parameters.Add("@MyCallPrice", SqlDbType.Int);
                    _Command.Parameters.Add("@DTGPrice", SqlDbType.Int);
                    _Command.Parameters.Add("@LG_MertKeyYn", SqlDbType.Bit);
                    _Command.Parameters.Add("@ServiceState", SqlDbType.Int);
                    _Command.Parameters.Add("@RequestFrom", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@RequestTo", SqlDbType.NVarChar);
                    _Command.Parameters.Add("@SubClientId", SqlDbType.Int);

                    foreach (Model row in _CoreList)
                    {
                        if (row.S_Idx != "" && row.Error == "")
                        {
                            string S_Idx = row.S_Idx.Trim();
                            string SBiz_NO = row.SBiz_NO.Trim().Replace("-", "");
                            string SName = row.SName.Trim();
                            string SUptae = row.SUptae.Trim();
                            string SUpjong = row.SUpjong.Trim();
                            string SCeo = row.SCeo.Trim();
                            string SCeoBirth = row.SCeoBirth.Trim();
                            string SMobileNo = row.SMobileNo.Trim();
                            string SPhoneNo = row.SPhoneNo.Trim();
                            string SFaxNo = row.SFaxNo.Trim();
                            string SEmail = row.SEmail.Trim();
                            string SState = row.SState.Trim();
                            string SCity = row.SCity.Trim();
                            string SStreet = row.SStreet.Trim();
                            string SBizGubun = row.SBizGubun.Trim();
                            string SRouteType = row.SRouteType.Trim();
                            string SInsurance = row.SInsurance.Trim();
                            string SCarNo = row.SCarNo.Trim();
                            string SCarType = row.SCarType.Trim();
                            string SCarSize = row.SCarSize.Trim();
                            string SCarGubun = row.SCarGubun.Trim();
                            string SCarYear = row.SCarYear.Trim();
                            string SPayBankName = row.SPayBankName.Trim();
                            string SPayAccountNo = row.SPayAccountNo.Trim();
                            string SInputName = row.SInputName.Trim();
                            string SCarstate = row.SCarstate.Trim();
                            string SCarcity = row.SCarcity.Trim();
                            string SCarStreet = row.SCarStreet.Trim();
                            string SfpisCartype = row.SfpisCartype.Trim();

                            #region 사업자구분
                            if (SBizGubun != "")
                            {
                                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BizType" && c.Name == SBizGubun).ToArray();
                                if (Query.Any())
                                {
                                    _Command.Parameters["@BizType"].Value = Query.First().Value;
                                }
                            }

                            #endregion
                            #region 운행노선
                            if (SRouteType != "")
                            {
                                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "RouteType" && c.Name == SRouteType).ToArray();
                                if (Query.Any())
                                {
                                    _Command.Parameters["@RouteType"].Value = Query.First().Value;
                                }
                            }

                            #endregion
                            #region 적재물보험
                            if (SInsurance != "")
                            {
                                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InsuranceType" && c.Name == SInsurance).ToArray();
                                if (Query.Any())
                                {
                                    _Command.Parameters["@InsuranceType"].Value = Query.First().Value;
                                }
                            }

                            #endregion
                            #region 차종
                            if (SCarType != "")
                            {
                                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && c.Name == SCarType).ToArray();
                                if (Query.Any())
                                {
                                    _Command.Parameters["@CarType"].Value = Query.First().Value;
                                }
                            }

                            #endregion
                            #region 톤수
                            if (SCarSize != "")
                            {
                                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Name == SCarSize).ToArray();
                                if (Query.Any())
                                {
                                    _Command.Parameters["@CarSize"].Value = Query.First().Value;
                                }
                            }
                            #endregion

                            #region 구분
                            if (SCarGubun != "")
                            {
                                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarGubun" && c.Name == SCarGubun).ToArray();
                                if (Query.Any())
                                {
                                    _Command.Parameters["@CarGubun"].Value = Query.First().Value;
                                }
                            }
                            #endregion

                            #region 은행명

                            if (SPayBankName != "")
                            {

                                var BankName = Filter.Bank.BankList.Where(c => c.Text == SPayBankName).ToArray();
                                //  var BankCode = Filter.Bank.BankList.Where(c => c. == SPayBankName).ToArray();

                                if (BankName.Any())
                                {

                                    _Command.Parameters["@PayBankName"].Value = BankName.First().Text;
                                    _Command.Parameters["@PayBankCode"].Value = BankName.First().Value;
                                    _Command.Parameters["@PayInputName"].Value = SInputName;
                                    _Command.Parameters["@PayAccountNo"].Value = SPayAccountNo;
                                }
                            }
                            #endregion


                            #region 국토부차종

                            if (SfpisCartype != "")
                            {

                                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "FPisCarType" && c.Name == SfpisCartype).ToArray();

                                if (Query.Any())
                                {

                                    _Command.Parameters["@FpisCarType"].Value = Query.First().Value;
                                }




                            }

                            #endregion


                            _Command.Parameters["@BizNo"].Value = SBiz_NO;
                            _Command.Parameters["@Name"].Value = SName;
                            _Command.Parameters["@CEO"].Value = SCeo;
                            _Command.Parameters["@Uptae"].Value = SUptae;
                            _Command.Parameters["@Upjong"].Value = SUpjong;

                            _Command.Parameters["@Email"].Value = SEmail;

                            _Command.Parameters["@MobileNo"].Value = SMobileNo;

                            _Command.Parameters["@PhoneNo"].Value = SPhoneNo;
                            _Command.Parameters["@FaxNo"].Value = SFaxNo;
                            _Command.Parameters["@AddressState"].Value = SState;
                            _Command.Parameters["@AddressCity"].Value = SCity;
                            _Command.Parameters["@AddressDetail"].Value = SStreet;

                            _Command.Parameters["@CEOBirth"].Value = SCeoBirth;

                            if (!String.IsNullOrEmpty(SCarNo))
                            {

                                _Command.Parameters["@CarNo"].Value = SCarNo;

                            }



                            _Command.Parameters["@CarYear"].Value = SCarYear;
                            _Command.Parameters["@ParkState"].Value = SCarstate;
                            _Command.Parameters["@ParkCity"].Value = SCarcity;
                            _Command.Parameters["@ParkStreet"].Value = SStreet;



                            string sCode;

                            if (CodeCompareTable.Any())
                            {
                                var DriverCode = 100001;
                                while (true)
                                {
                                    if (!CodeCompareTable.Any(c => c == DriverCode.ToString()))
                                    {
                                        break;
                                    }
                                    DriverCode++;
                                }
                                sCode = DriverCode.ToString();
                            }
                            else
                            {

                                sCode = "100001";
                            }
                            CodeCompareTable.Add(sCode);
                            CodeCompareTable.Sort();

                            _Command.Parameters["@Code"].Value = sCode;


                            string S_BizNo = string.Empty;
                            string BiznoId = string.Empty;
                            string sPassword = string.Empty;

                            if (SBiz_NO.Length == 10)
                            {
                                S_BizNo = SBiz_NO.Substring(5, 5);
                            }
                            else if (SBiz_NO.Length == 12)
                            {
                                S_BizNo = SBiz_NO.Substring(7, 5);
                            }



                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();

                                SqlCommand selectCmd = new SqlCommand(
                                    @"SELECT top 1 convert(int,right(LoginId,3)+1) as LoginId FROM Drivers Where right(BizNo,5) = @Bizno order by loginid desc", cn);
                                selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
                                var Reader = selectCmd.ExecuteReader();
                                while (Reader.Read())
                                {

                                    BiznoId = Reader["LoginId"].ToString();

                                }
                                if (String.IsNullOrEmpty(BiznoId))
                                {
                                    BiznoId = "";

                                }
                                cn.Close();
                            }
                            string BizNo = string.Empty;
                            if (BiznoId == "")
                            {
                                BizNo = "001";

                            }
                            else if (BiznoId.Length == 2)
                            {
                                BizNo = "0" + BiznoId;
                            }
                            else if (BiznoId.Length == 1)
                            {
                                BizNo = "00" + BiznoId;
                            }
                            else if (BiznoId.Length == 3)
                            {
                                BizNo = BiznoId;
                            }


                            string sLoginId = "m" + SBiz_NO.Substring(5, 5) + BizNo;

                            if (SMobileNo.Length > 10)
                            {
                                sPassword = SMobileNo.Substring(SMobileNo.Length - 4, 4);
                            }


                            _Command.Parameters["@LoginId"].Value = sLoginId;
                            _Command.Parameters["@Password"].Value = sPassword;


                            DateTime sCreateDate = DateTime.Now;
                            string sUsePayNow = "0";
                            int sCandidateId = LocalUser.Instance.LogInInformation.ClientId;
                            _Command.Parameters["@CreateDate"].Value = sCreateDate;

                            _Command.Parameters["@AccountOwner"].Value = "";
                            _Command.Parameters["@AccountRegNo"].Value = "";
                            _Command.Parameters["@BankName"].Value = "";
                            _Command.Parameters["@AccountNo"].Value = "";
                            _Command.Parameters["@AccountExtra"].Value = "";
                            if (String.IsNullOrEmpty(sUsePayNow))
                            {
                                _Command.Parameters["@UsePayNow"].Value = 2;
                            }
                            else
                            {
                                _Command.Parameters["@UsePayNow"].Value = int.Parse(sUsePayNow);
                            }
                            _Command.Parameters["@ClientBizType"].Value = 0;
                            //     row.CandidateId = int.Parse(cmb_CandidateGubun.SelectedValue.ToString());
                            _Command.Parameters["@CandidateId"].Value = LocalUser.Instance.LogInInformation.ClientId;

                            _Command.Parameters["@Car_ContRact"].Value = false;


                            _Command.Parameters["@AccountUse"].Value = false;
                            _Command.Parameters["@DTGUse"].Value = true;
                            _Command.Parameters["@FPISUse"].Value = true;
                            _Command.Parameters["@MyCallUSe"].Value = true;
                            _Command.Parameters["@OTGUse"].Value = false;
                            _Command.Parameters["@ServicePrice"].Value = "5500";
                            _Command.Parameters["@useTax"].Value = true;
                            _Command.Parameters["@OTGPrice"].Value = 0;
                            _Command.Parameters["@AccountPrice"].Value = 0;
                            _Command.Parameters["@FPISPrice"].Value = 0;
                            _Command.Parameters["@MyCallPrice"].Value = 0;
                            _Command.Parameters["@DTGPrice"].Value = 5000;

                            _Command.Parameters["@LG_MertKeyYn"].Value = false;
                            _Command.Parameters["@ServiceState"].Value = 3;

                            if (SCarGubun == "단기용차")
                            {
                                _Command.Parameters["@Car_ContRact"].Value = true;
                                _Command.Parameters["@RequestFrom"].Value = DateTime.Now.ToString("yyyy -MM-dd").Replace("-", "/");
                                _Command.Parameters["@RequestTo"].Value = DateTime.Now.AddMonths(1).AddDays(1).ToString("yyyy -MM-dd").Replace("-", "/");



                            }
                            else
                            {
                                _Command.Parameters["@Car_ContRact"].Value = false;
                                _Command.Parameters["@RequestFrom"].Value = "";
                                _Command.Parameters["@RequestTo"].Value = "";
                            }
                            _Command.Parameters["@SubClientId"].Value = DBNull.Value;
                            if (LocalUser.Instance.LogInInformation.IsSubClient)
                                _Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;


                            _Command.ExecuteNonQuery();
                            FCount++;
                        }
                    }
                }
                _Connection.Close();
            }
            

            label4.Text = FCount.ToString() + " 건";
            label5.Text = (FCount - errCount).ToString() + " 건";
            label6.Text = errCount.ToString() + " 건";

            EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
            this.Close();
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
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT CarNo, Code FROM Drivers WHERE ServiceState <> 5";
                connection.Open();
                var dataReader = command.ExecuteReader( CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    CarNoCompareTable.Add(dataReader.GetString(0));
                    CodeCompareTable.Add(dataReader.GetString(1));
                }
            }
            CodeCompareTable.Sort();
        }

        #region STORAGE
        class Model : INotifyPropertyChanged
        {
            private String _S_Idx = "";
            private String _SBiz_NO = "";
            private String _SName = "";
            private String _SUptae = "";
            private String _SUpjong = "";
            private String _SCeo = "";
            private String _SCeoBirth = "";
            private String _SMobileNo = "";
            private String _SPhoneNo = "";
            private String _SFaxNo = "";
            private String _SEmail = "";
            private String _SState = "";
            private String _SCity = "";
            private String _SStreet = "";
            private String _SBizGubun = "";
            private String _SRouteType = "";
            private String _SInsurance = "";
            private String _SCarNo = "";
            private String _SCarType = "";
            private String _SCarSize = "";
            private String _SCarGubun = "";
            private String _SCarYear = "";
            private String _SPayBankName = "";
            private String _SPayAccountNo = "";
            private String _SInputName = "";
            private String _SCarstate = "";
            private String _SCarcity = "";
            private String _SCarStreet = "";
            private String _SfpisCartype = "";
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

            public string SCeoBirth
            {
                get
                {
                    return _SCeoBirth;
                }

                set
                {
                    SetField(ref _SCeoBirth, value);
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

            public string SBizGubun
            {
                get
                {
                    return _SBizGubun;
                }

                set
                {
                    SetField(ref _SBizGubun, value);
                }
            }

            public string SRouteType
            {
                get
                {
                    return _SRouteType;
                }

                set
                {
                    SetField(ref _SRouteType, value);
                }
            }

            public string SInsurance
            {
                get
                {
                    return _SInsurance;
                }

                set
                {
                    SetField(ref _SInsurance, value);
                }
            }

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

            public string SCarGubun
            {
                get
                {
                    return _SCarGubun;
                }

                set
                {
                    SetField(ref _SCarGubun, value);
                }
            }

            public string SCarYear
            {
                get
                {
                    return _SCarYear;
                }

                set
                {
                    SetField(ref _SCarYear, value);
                }
            }

            public string SPayBankName
            {
                get
                {
                    return _SPayBankName;
                }

                set
                {
                    SetField(ref _SPayBankName, value);
                }
            }

            public string SPayAccountNo
            {
                get
                {
                    return _SPayAccountNo;
                }

                set
                {
                    SetField(ref _SPayAccountNo, value);
                }
            }

            public string SInputName
            {
                get
                {
                    return _SInputName;
                }

                set
                {
                    SetField(ref _SInputName, value);
                }
            }

            public string SCarstate
            {
                get
                {
                    return _SCarstate;
                }

                set
                {
                    SetField(ref _SCarstate, value);
                }
            }

            public string SCarcity
            {
                get
                {
                    return _SCarcity;
                }

                set
                {
                    SetField(ref _SCarcity, value);
                }
            }

            public string SCarStreet
            {
                get
                {
                    return _SCarStreet;
                }

                set
                {
                    SetField(ref _SCarStreet, value);
                }
            }

            public string SfpisCartype
            {
                get
                {
                    return _SfpisCartype;
                }

                set
                {
                    SetField(ref _SfpisCartype, value);
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
