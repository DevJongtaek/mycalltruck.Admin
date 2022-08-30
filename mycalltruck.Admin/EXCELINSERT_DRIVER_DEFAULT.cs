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
    public partial class EXCELINSERT_DRIVER_DEFAULT : Form
    {
        string FileName = string.Empty;
        int FCount = 0;
        int errCount = 0;
        int DataCount = 0;
        string SPayBankName = string.Empty;
        DESCrypt m_crypt = null;
        public EXCELINSERT_DRIVER_DEFAULT()
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
                //const int TestIndex = 2;
                const int TestIndex = 12;
                int RowIndex = 3;
                while (true)
                {
                    var TestText = _Sheet.Cells[RowIndex, TestIndex].Text;
                    if (String.IsNullOrEmpty(TestText))
                        break;

                    var Added = new Model
                    {
                        S_Idx = _Sheet.Cells[RowIndex, 1].Text.Trim(),
                        SBiz_NO = _Sheet.Cells[RowIndex, 2].Text.Trim(),
                        SName = _Sheet.Cells[RowIndex, 3].Text.Trim(),
                        SUptae = _Sheet.Cells[RowIndex, 4].Text.Trim(),
                        SUpjong = _Sheet.Cells[RowIndex, 5].Text.Trim(),
                        SZip = _Sheet.Cells[RowIndex, 6].Text.Trim(),
                        SState = _Sheet.Cells[RowIndex, 7].Text.Trim(),
                        SCity = _Sheet.Cells[RowIndex, 8].Text.Trim(),
                        SStreet = _Sheet.Cells[RowIndex, 9].Text.Trim(),
                        SCeo = _Sheet.Cells[RowIndex, 10].Text.Trim(),
                        SMobileNo = _Sheet.Cells[RowIndex, 11].Text.Trim(),
                        SCarNo = _Sheet.Cells[RowIndex, 12].Text.Trim(),
                        SCarType = _Sheet.Cells[RowIndex, 13].Text.Trim(),
                        SCarSize = _Sheet.Cells[RowIndex, 14].Text.Trim(),

                        SCarInfo = _Sheet.Cells[RowIndex, 15].Text.Trim(),

                        SPayBankName = _Sheet.Cells[RowIndex, 16].Text.Trim(),
                        SPayAccountNo = _Sheet.Cells[RowIndex, 17].Text.Replace("-","").Trim(),
                        SInputName = _Sheet.Cells[RowIndex, 18].Text.Trim(),
                      //  SVaccount = _Sheet.Cells[RowIndex, 18].Text.Trim(),
                        SMisu = _Sheet.Cells[RowIndex, 19].Text.Trim(),
                        SMizi = _Sheet.Cells[RowIndex, 20].Text.Trim(),
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
            int RowIndex = 3;
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
                        if (String.IsNullOrEmpty(row.SCarNo))
                        {
                            ErrorText += "차량 번호 없음";
                            errCount++;
                            row.Error = ErrorText;
                            continue;
                        }
                        else
                        {
                            var IsMyCar = mDriverRepository.IsMyCar(row.SCarNo);
                            if (IsMyCar)
                            {
                                ErrorText += "등록된 차량";
                                errCount++;
                                row.Error = ErrorText;
                                continue;
                            }
                            else
                            {
                                var IsAnotherCar = mDriverRepository.IsAnotherCar(row.SCarNo);
                                if (IsAnotherCar)
                                {
                                    continue;
                                }
                            }
                           
                        }
                        if (InnerCarNo.Contains(row.SCarNo))
                        {
                            ErrorText += "동일 엑셀 속에 중복된 차량번호";
                        }
                        InnerCarNo.Add(row.SCarNo);
                        //사업자등록번호 없으면 에러
                        if (String.IsNullOrEmpty(row.SBiz_NO) || row.SBiz_NO.Replace("-","").Length != 10)
                        {
                            if (String.IsNullOrEmpty(row.SBiz_NO))
                            {
                                row.SBiz_NO = "";
                            }
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 사업자번호 이상";
                            //}
                            //else
                            //{
                            //    ErrorText += "사업자번호 이상";

                            //}
                            //RowErrorCount++;
                        }
                        //상호 없으면 에러
                        if (String.IsNullOrEmpty(row.SName))
                        {
                            row.SName = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 상호 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "상호 빈값";

                            //}
                            //RowErrorCount++;
                        }
                        //업태
                        if (String.IsNullOrEmpty(row.SUptae))
                        {

                            row.SUptae = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 업태 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "업태 빈값";

                            //}
                            //RowErrorCount++;
                        }
                        //종목
                        if (String.IsNullOrEmpty(row.SUpjong))
                        {
                            row.SUpjong = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 종목 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "종목 빈값";

                            //}
                            //RowErrorCount++;
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
                        //핸드폰번호
                        if (!Regex.IsMatch(row.SMobileNo.Replace("-",""), @"^01[0,1,6,7,8,9]\d{3,4}\d{4}"))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 핸드폰번호 이상 ";
                            }
                            else
                            {
                                ErrorText += "핸드폰번호 이상";

                            }
                            RowErrorCount++;
                        }
                        //우편번호
                        if (!Regex.IsMatch(row.SZip, @"^\d{5}$"))
                        {
                            if (String.IsNullOrEmpty(row.SZip))
                            {
                                row.SZip = "";
                            }
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 우편번호 이상";
                            //}
                            //else
                            //{
                            //    ErrorText += "우편번호 이상";

                            //}
                            //RowErrorCount++;
                        }                        //시도
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
                        //else
                        //{
                        //    var StateQuery = SingleDataSet.Instance.AddressReferences.Where(c => c.State == row.SState).ToArray();

                        //    if (StateQuery.Any())
                        //    {

                        //    }
                        //    else
                        //    {
                        //        if (RowErrorCount > 0)
                        //        {
                        //            ErrorText += " | 시도";
                        //        }
                        //        else
                        //        {
                        //            ErrorText += "시도";

                        //        }
                        //        RowErrorCount++;

                        //    }

                        //}
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
                        //else
                        //{
                        //    var StateQuery = SingleDataSet.Instance.AddressReferences.Where(c => c.City == row.SCity).ToArray();

                        //    if (StateQuery.Any())
                        //    {

                        //    }
                        //    else
                        //    {
                        //        if (RowErrorCount > 0)
                        //        {
                        //            ErrorText += " | 시군구";
                        //        }
                        //        else
                        //        {
                        //            ErrorText += "시군구";

                        //        }
                        //        RowErrorCount++;

                        //    }

                        //}
                        //상세주소
                        if (String.IsNullOrEmpty(row.SStreet))
                        {
                            row.SStreet = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 상세주소 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "상세주소 빈값";

                            //}
                            //RowErrorCount++;
                        }
                        //차종
                        //int _CarType = 0;
                        //bool _CarTypeValid = int.TryParse(row.SCarType, out _CarType) && _CarType > 0 && SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == _CarType);
                        if (String.IsNullOrEmpty(row.SCarType))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 차종코드 빈값";
                            }
                            else
                            {
                                ErrorText += "차종코드 빈값";

                            }
                            RowErrorCount++;
                        }
                        //톤수
                        //int _CarSize = 0;
                        //bool _CarSizeValid = int.TryParse(row.SCarSize, out _CarSize) && _CarSize > 0 && SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == _CarSize);
                        if (String.IsNullOrEmpty(row.SCarSize))
                        {
                            if (RowErrorCount > 0)
                            {
                                ErrorText += " | 톤수코드 빈값";
                            }
                            else
                            {
                                ErrorText += "톤수코드 빈값";

                            }
                            RowErrorCount++;
                        }
                        //은행
                        if (String.IsNullOrEmpty(row.SPayBankName))
                        {
                            row.SPayBankName = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 은행 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "은행 빈값";

                            //}
                            //RowErrorCount++;
                        }
                        //계좌번호
                        if (String.IsNullOrEmpty(row.SPayAccountNo))
                        {
                            row.SPayAccountNo = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 계좌번호 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "계좌번호 빈값";

                            //}
                            //RowErrorCount++;
                        }
                        if (String.IsNullOrEmpty(row.SInputName))
                        {
                            row.SInputName = "";
                            //if (RowErrorCount > 0)
                            //{
                            //    ErrorText += " | 예금주 빈값";
                            //}
                            //else
                            //{
                            //    ErrorText += "예금주 빈값";

                            //}
                            //RowErrorCount++;
                        }
                        #region 차종
                        if (row.SCarType != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && (c.Value == IntNull.Parse(row.SCarType) ||c.Name == row.SCarType) ).ToArray();
                            if (Query.Any())
                            {
                                row.SCarType = Query.First().Value.ToString();
                            }
                            else
                            {
                                row.SCarType = "99";
                                //if (RowErrorCount > 0)
                                //{
                                //    ErrorText += " | 등록된 차종 없음.";
                                //}
                                //else
                                //{
                                //    ErrorText += "등록된  차종 없음.";

                                //}
                                //RowErrorCount++;
                            }


                        }
                        if(String.IsNullOrEmpty(row.SMisu))
                        {
                            row.SMisu = "0";

                        }
                        if (String.IsNullOrEmpty(row.SMizi))
                        {
                            row.SMizi = "0";

                        }
                        #endregion
                        #region 톤수
                        if (row.SCarSize != "")
                        {
                            var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && (c.Value == IntNull.Parse(row.SCarSize) || c.Number == double.Parse(row.SCarSize ))).ToArray();
                            if (Query.Any())
                            {
                                row.SCarSize = Query.First().Value.ToString();
                            }
                            else
                            {
                                row.SCarSize = "99";
                                //if (RowErrorCount > 0)
                                //{
                                //    ErrorText += " | 등록된 톤수 없음.";
                                //}
                                //else
                                //{
                                //    ErrorText += "등록된  톤수 없음.";

                                //}
                                //RowErrorCount++;
                            }
                        }
                        #endregion
                        #region 은행명
                        if (row.SPayBankName != "")
                        {

                            //var BankName = Filter.Bank.BankList.Where(c => c.Value == row.SPayBankName).ToArray();
                            //if (!BankName.Any())
                            //{
                            //    if (RowErrorCount > 0)
                            //    {
                            //        ErrorText += " | 등록된 은행 없음.";
                            //    }
                            //    else
                            //    {
                            //        ErrorText += "등록된  은행 없음.";

                            //    }
                            //    RowErrorCount++;
                            //}
                        }
                        #endregion

                        if (RowErrorCount == 0 && row.SPayBankName != "" && row.SPayAccountNo != "" && row.SInputName != "")
                        {
                            string Parameter = "";
                            WebClient mWebClient = new WebClient();
                            string Mid = "edubill";
                            Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3}&MID={4}", row.SPayBankName, row.SPayAccountNo, row.SInputName, SPayBankName, Mid);
                            try
                            {
                                var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                                if (r == null || r.ToLower() != "true")
                                {
                                    if (RowErrorCount > 0)
                                    {
                                        ErrorText += " | 계좌인증 실패";
                                    }
                                    else
                                    {
                                        ErrorText += "계좌인증 실패";
                                    }
                                    RowErrorCount++;
                                }
                            }
                            catch (Exception)
                            {
                                Invoke(new Action(() => MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                                return;
                            }
                        }


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
                        else
                        {
                            S_BizNo = "12345";
                        }



                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();

                            SqlCommand selectCmd = new SqlCommand(
                                @"SELECT top 1 convert(int,right(LoginId,3)+1) as LoginId FROM Drivers Where right(BizNo,5) = @Bizno and Loginid != 'test' order by loginid desc", cn);
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


            List<String> InnerCarNo = new List<string>();
            Task.Factory.StartNew(() => {
                DriverRepository mDriverRepository = new DriverRepository();
                BaseDataSet.DriversRow DriverRow = baseDataSet.Drivers.NewDriversRow();
                foreach (Model row in _CoreList)
                {
                    bar.Invoke(new Action(() => {
                        bar.Value++;
                    }));

                    if (row.S_Idx != "" && row.Error == "")
                    {
                        string S_Idx = row.S_Idx.Trim();
                        string SBiz_NO = row.SBiz_NO.Trim();
                        string SName = row.SName.Trim();
                        string SUptae = row.SUptae.Trim();
                        string SUpjong = row.SUpjong.Trim();
                        string SCeo = row.SCeo.Trim();
                        string SCeoBirth = "000000";
                        string SMobileNo = row.SMobileNo.Trim();
                        string SState = row.SState.Trim();
                        string SCity = row.SCity.Trim();
                        string SStreet = row.SStreet.Trim();
                        string SZip = row.SZip.Trim();
                        string SCarNo = row.SCarNo.Trim();
                        string SCarType = row.SCarType.Trim();
                        string SCarSize = row.SCarSize.Trim();
                        string SCarYear = SCeo;
                        string SPayBankName = row.SPayBankName.Trim();
                        string SPayAccountNo = row.SPayAccountNo.Trim();
                        string SInputName = row.SInputName.Trim();
                        string SVaccount = row.SVaccount.Trim();
                        string SMisu = row.SMisu.Trim().Replace(",", "");
                        string SMizi = row.SMizi.Trim().Replace(",", "");
                        string sCarInfo = row.SCarInfo.Trim();

                        if (!String.IsNullOrEmpty(SBiz_NO) && SBiz_NO.Replace("-", "").Length  == 10)
                        {
                            SBiz_NO = SBiz_NO.Replace("-", "");
                            SBiz_NO = SBiz_NO.Substring(0, 3) + "-" + SBiz_NO.Substring(3, 2) + "-" + SBiz_NO.Substring(5);
                        }
                        else
                        {
                            SBiz_NO = "111-11-11111";
                        }



                        SMobileNo = SMobileNo.Replace("-", "");
                        if (SMobileNo.Length == 10)
                        {
                            SMobileNo = SMobileNo.Substring(0, 3) + "-" + SMobileNo.Substring(3, 3) + "-" + SMobileNo.Substring(6);
                        }
                        else if (SMobileNo.Length == 11)
                        {
                            SMobileNo = SMobileNo.Substring(0, 3) + "-" + SMobileNo.Substring(3, 4) + "-" + SMobileNo.Substring(7);
                        }
                        var IsAnotherCar = mDriverRepository.IsAnotherCar(SCarNo);
                        if (IsAnotherCar)
                        {
                            mDriverRepository.ConnectDriver(SCarNo);
                        }
                        else
                        {
                            var IsMyCar = mDriverRepository.IsMyCar(row.SCarNo);
                            var _DriverId = mDriverRepository.GetDriverId(row.SCarNo);
                            if (IsMyCar)
                            {

                            }
                            else
                            {


                                var BankName = Filter.Bank.BankList.Where(c => c.Value == SPayBankName).First();
                                int DriverId = mDriverRepository.CreateDriver(SBiz_NO, SName, SCeo, SUptae, SUpjong, SCeoBirth,
                                     SZip, SState, SCity, SStreet, SMobileNo, BankName.Value, BankName.Text, SPayAccountNo, SInputName, SCarNo,
                                     int.Parse(SCarType), int.Parse(SCarSize), sCarInfo, 0, int.Parse(SMisu), int.Parse(SMizi));
                            }
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
            private String _SUptae = "";
            private String _SUpjong = "";
            private String _SCeo = "";
            private String _SMobileNo = "";
            private String _SState = "";
            private String _SCity = "";
            private String _SStreet = "";
            private String _SZip = "";
            private String _SCarNo = "";
            private String _SCarType = "";
            private String _SCarSize = "";
            private String _SCarInfo = "";


            private String _SPayBankName = "";
            private String _SPayAccountNo = "";
            private String _SInputName = "";
            private String _SVaccount = "";
            private String _SMisu = "";
            private String _SMizi = "";
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

            public string SCarInfo
            {
                get
                {
                    return _SCarInfo;
                }

                set
                {
                    SetField(ref _SCarInfo, value);
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
            public string SVaccount
            {
                get
                {
                    return _SVaccount;
                }

                set
                {
                    SetField(ref _SVaccount, value);
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
