using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0203_CAROWNERMANAGE_FAULT_Add_SG : Form
    {
        DESCrypt m_crypt = null;
        bool ValidateStart = false;
        int DriverId = 0;
        public FrmMN0203_CAROWNERMANAGE_FAULT_Add_SG()
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");
        }
        private void FrmMN0203_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            ValidateStart = true;
            _InitCmb();
            txt_CarNo.Focus();
            ValidateStart = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _UpdateDB(() => {
                txt_CarNo.Clear();
                txt_Name.Clear();
                txt_CEO.Clear();
                txt_Uptae.Clear();
                txt_Upjong.Clear();
                txt_CEOBirth.Clear();
                txt_Zip.Clear();
                txt_State.Clear();
                txt_City.Clear();
                txt_Street.Clear();
                txt_MobileNo.Clear();
                txt_PhoneNo.Clear();
                txt_Email.Clear();
                txt_Name.ReadOnly = true;
                txt_CEO.ReadOnly = true;
                txt_Uptae.ReadOnly = true;
                txt_Upjong.ReadOnly = true;
                txt_CEOBirth.ReadOnly = true;
                txt_Zip.ReadOnly = true;
                txt_State.ReadOnly = true;
                txt_City.ReadOnly = true;
                txt_Street.ReadOnly = true;
                txt_MobileNo.ReadOnly = true;
                txt_PhoneNo.ReadOnly = true;
                txt_Email.ReadOnly = true;

                btnFindZip.Enabled = false;
                btnAdd.Enabled = false;
                btnAddClose.Enabled = false;

                DriverId = 0;

                txt_CarNo.Focus();
            });
        }
        public bool IsSuccess = false;
        public CMDataSet.DriversRow CurrentCode = null;
        public CMDataSet.DriverPapersRow DCurrentCode = null;

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            _UpdateDB(() => Close());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _UpdateDB(Action Done)
        {
            //
            bool HasError = false;
            List<String> ErrorArray = new List<string>();
            ClearError();
            DriverRepository mDriverRepository = new DriverRepository();
            if (DriverId > 0)
            {
                try
                {
                    mDriverRepository.ConnectDriver(DriverId);
                }
                catch (Exception ex)
                {
                    pnProgress.Visible = false;
                    MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                pnProgress.Visible = false;
                Done();
                return;
            }
            if (String.IsNullOrWhiteSpace(txt_Name.Text))
            {
                var Error = "상호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_Name, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_CEO.Text))
            {
                var Error = "대표자를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_CEO, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Uptae.Text))
            {
                var Error = "업태를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_Uptae, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Upjong.Text))
            {
                var Error = "업종을 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_Upjong, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Zip.Text))
            {
                var Error = "우편번호 검색을 해주세요.";
                ErrorArray.Add(Error);
                SetError(btnFindZip, Error);
                HasError = true;
            }
            if (txt_CEOBirth.Text.Length != 6 || txt_CEOBirth.Text.Contains(" "))
            {
                var Error = "개인사업자는 생년월일을 법인사업자는 법인번호 앞 6자리를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(btnFindZip, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_CarNo.Text))
            {
                var Error = "사업자번호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_CarNo, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_MobileNo.Text))
            {
                var Error = "핸드폰번호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_MobileNo, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Email.Text))
            {
                var Error = "이메일을 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_Email, Error);
                HasError = true;
            }
            if (HasError)
            {
                return;
            }
            pnProgress.Visible = true;
            string SName = txt_Name.Text.Trim();
            string SUptae = txt_Uptae.Text.Trim();
            string SUpjong = txt_Upjong.Text.Trim();
            string SCeo = txt_CEO.Text.Trim();
            string SCeoBirth = txt_CEOBirth.Text;
            string SMobileNo = txt_MobileNo.Text.Trim();
            string SPhoneNo = txt_PhoneNo.Text.Trim();
            string SState = txt_State.Text.Trim();
            string SCity = txt_City.Text.Trim();
            string SStreet = txt_Street.Text.Trim();
            string SZip = txt_Zip.Text.Trim();
            string SCarNo = txt_CarNo.Text.Trim();
            String SBiz_NO = SCarNo.Substring(0, 3) + "-" + SCarNo.Substring(3, 2) + "-" + SCarNo.Substring(5, 5);
            string SCarYear = SCeo;
            string SCarstate = SState;
            string SCarcity = SCity;
            string SCarStreet = SStreet;
            mDriverRepository.CreateDriver_SG(
                SBiz_NO, SName, SCeo, SUptae, SUpjong, SCeoBirth, SZip, SState, SCity, SStreet, SMobileNo, SPhoneNo, SBiz_NO.Replace("-",""), "", txt_Email.Text);
            if (Done != null)
                Done();
        }

        private void _InitCmb()
        {
        }
       
        private void btn_DriverExcel_Click(object sender, EventArgs e)
        {
            string title = string.Empty;
            byte[] ieExcel;

            string fileString = string.Empty;

            int _DriverType = 0;
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _ClientsCommand = _Connection.CreateCommand())
                    {
                        _ClientsCommand.CommandText = "SELECT DriverType FROM Clients WHERE ClientId = @ClientId";
                        _ClientsCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        var o = _ClientsCommand.ExecuteScalar();
                        if (o == null || !int.TryParse(o.ToString(), out _DriverType))
                        {
                            MessageBox.Show("항목을 가져오는 중 오류가 발생하였습니다. 잠시 후에 다시 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    _Connection.Close();
                }
            }
            if (_DriverType == 1)
            {
                fileString = "차량관리입력양식(기본)_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                ieExcel = Properties.Resources.차량관리입력양식_기본_;
            }
            else
            {
                fileString = "차량관리입력양식(표준).xlsx";
                ieExcel = Properties.Resources.차량관리입력양식_표준__XLSX;
            }

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {

                di.Create();
            }
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                    MessageBox.Show("엑셀 양식을 열 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
            }
            try
            {
                File.WriteAllBytes(FileName, ieExcel);
                System.Diagnostics.Process.Start(FileName);
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 양식을 열 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
        }
        private string getValueFrom2DArray(object[,] array, int index)
        {
            int intD = array.GetUpperBound(1);
            try
            {
                return array[index, intD].ToString();
            }
            catch { return string.Empty; }
        }
        //private void btnExcelImport_Click(object sender, EventArgs e)
        //{
        //    DataGridViewColumn[] requiereds = new DataGridViewColumn[]{
                   
        //        codeDataGridViewTextBoxColumn,
        //        bizNoDataGridViewTextBoxColumn,
        //        nameDataGridViewTextBoxColumn,
        //        cEODataGridViewTextBoxColumn,
        //        uptaeDataGridViewTextBoxColumn,
        //        upjongDataGridViewTextBoxColumn,
        //         mobileNoDataGridViewTextBoxColumn,

        //        addressStateDataGridViewTextBoxColumn,
        //        addressCityDataGridViewTextBoxColumn,
        //        addressDetailDataGridViewTextBoxColumn,
               
               
                
               
        //        carNoDataGridViewTextBoxColumn,
        //        carTypeDataGridViewTextBoxColumn,
        //        carSizeDataGridViewTextBoxColumn,
        //        Gubun,
        //        carYearDataGridViewTextBoxColumn,
        //     parkStateDataGridViewTextBoxColumn,
             
        //     parkCityDataGridViewTextBoxColumn,
        //     parkStreetDataGridViewTextBoxColumn};
        //    try
        //    {
        //        Dictionary<DataGridViewColumn, object[,]> dic = new Dictionary<DataGridViewColumn, object[,]>();
        //        dic.Add(codeDataGridViewTextBoxColumn, null);
        //        dic.Add(bizNoDataGridViewTextBoxColumn, null);
        //        dic.Add(nameDataGridViewTextBoxColumn, null);
        //        dic.Add(cEODataGridViewTextBoxColumn, null);
        //        dic.Add(uptaeDataGridViewTextBoxColumn, null);
        //        dic.Add(upjongDataGridViewTextBoxColumn, null);
        //        dic.Add(mobileNoDataGridViewTextBoxColumn, null);




        //        dic.Add(addressStateDataGridViewTextBoxColumn, null);
        //        dic.Add(addressCityDataGridViewTextBoxColumn, null);
        //        dic.Add(addressDetailDataGridViewTextBoxColumn, null);

        //        dic.Add(emailDataGridViewTextBoxColumn, null);
        //        dic.Add(phoneNoDataGridViewTextBoxColumn, null);
        //        dic.Add(faxNoDataGridViewTextBoxColumn, null);


        //        // dic.Add(passwordDataGridViewTextBoxColumn, null);
        //        dic.Add(bizTypeDataGridViewTextBoxColumn, null);
        //        dic.Add(RouteType, null);
        //        dic.Add(insuranceTypeDataGridViewTextBoxColumn, null);

        //        dic.Add(carNoDataGridViewTextBoxColumn, null);
        //        dic.Add(carTypeDataGridViewTextBoxColumn, null);
        //        dic.Add(carSizeDataGridViewTextBoxColumn, null);
        //        dic.Add(Gubun, null);
        //        dic.Add(carYearDataGridViewTextBoxColumn, null);

        //        dic.Add(parkStateDataGridViewTextBoxColumn, null);
        //        dic.Add(parkCityDataGridViewTextBoxColumn, null);
        //        dic.Add(parkStreetDataGridViewTextBoxColumn, null);
        //        //  dic.Add(loginIdDataGridViewTextBoxColumn, null);
        //        dic.Add(CandidateId, null);
        //        dic.Add(FpisCarType, null);

        //        FrmExcel f = new FrmExcel("차량관리", requiereds, ref dic);
        //        #region t
        //        Thread t = new Thread(new ThreadStart(() =>
        //        {
        //            f.Invoke(new Action(() =>
        //            {
        //                f.btnClose.Enabled = false;
        //                f.btnNext.Enabled = false;
        //                f.btnPrevious.Enabled = false;
        //            }));
        //            int i = 1;
        //            string lbl1 = f.lblStep3_1.Text;
        //            string lbl2 = f.lblStep3_2.Text;
        //            DateTime o = DateTime.Now;
        //            decimal d = 0;
        //            int iSum = 0, iSkp = 0, iNew = 0;
        //            CMDataSet cMDataSet = new CMDataSet();
        //            var driversTableAdapter = new CMDataSetTableAdapters.DriversTableAdapter();
        //            while (true)
        //            {
        //                string sCode;
        //                if (CodeCompareTable.Count > 0)
        //                {
        //                    var DriverCode = 100001;
        //                    var DriverCodeCandidate = CodeCompareTable.OrderBy(c => c);
        //                    while (true)
        //                    {
        //                        if (!DriverCodeCandidate.Any(c => c == DriverCode.ToString()))
        //                        {
        //                            break;
        //                        }
        //                        DriverCode++;
        //                    }
        //                    sCode = DriverCode.ToString();
        //                }
        //                else
        //                {

        //                    sCode = "100001";
        //                }


        //                string sbizNo = getValueFrom2DArray(dic[bizNoDataGridViewTextBoxColumn], i);
        //                if (sbizNo == "사업자번호")
        //                {
        //                    i++;
        //                    continue;

        //                }
        //                if (sbizNo == "") break;
        //                string sName = getValueFrom2DArray(dic[nameDataGridViewTextBoxColumn], i);
        //                string sCeo = getValueFrom2DArray(dic[cEODataGridViewTextBoxColumn], i);
        //                string sUptae = getValueFrom2DArray(dic[uptaeDataGridViewTextBoxColumn], i);
        //                string sUpjong = getValueFrom2DArray(dic[upjongDataGridViewTextBoxColumn], i);
        //                string smobileNo = getValueFrom2DArray(dic[mobileNoDataGridViewTextBoxColumn], i);

        //                string S_BizNo = string.Empty;
        //                string BiznoId = string.Empty;
        //                string sPassword = string.Empty;

        //                if (sbizNo.Length == 10)
        //                {
        //                    S_BizNo = sbizNo.Substring(5, 5);
        //                }
        //                else if(sbizNo.Length == 12)
        //                {
        //                    S_BizNo = sbizNo.Substring(7, 5);
        //                }
        //                //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //                //{
        //                //    SqlCommand selectCmd = new SqlCommand(
        //                //        @"SELECT Count(*) FROM Drivers Where right(BizNo,5) = @Bizno ", cn);
        //                //    selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
        //                //    // selectCmd.Parameters.Add(new SqlParameter("@UserPassword", txtPassword.Text));
        //                //    cn.Open();
        //                //    object o = selectCmd.ExecuteScalar();
        //                //    if (o != null)
        //                //        r = Convert.ToInt16(o);
        //                //    cn.Close();
        //                //}


        //                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
        //                {
        //                    cn.Open();

        //                    SqlCommand selectCmd = new SqlCommand(
        //                        @"SELECT top 1 convert(int,right(LoginId,3)+1) as LoginId FROM Drivers Where right(BizNo,5) = @Bizno order by loginid desc", cn);
        //                    selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
        //                    var Reader = selectCmd.ExecuteReader();
        //                    while (Reader.Read())
        //                    {

        //                        BiznoId = Reader["LoginId"].ToString();

        //                    }
        //                    if (String.IsNullOrEmpty(BiznoId))
        //                    {
        //                        BiznoId = "";

        //                    }
        //                    cn.Close();
        //                }
        //                string BizNo = string.Empty;
        //                if (BiznoId == "")
        //                {
        //                    BizNo = "001";

        //                }
        //                else if (BiznoId.Length == 2)
        //                {
        //                    BizNo = "0" + BiznoId;
        //                }
        //                else if (BiznoId.Length == 1)
        //                {
        //                    BizNo = "00" + BiznoId;
        //                }
        //                else if (BiznoId.Length == 3)
        //                {
        //                    BizNo = BiznoId;
        //                }


        //                string sLoginId = "m" + sbizNo.Substring(5, 5) + BizNo;

        //                if (smobileNo.Length > 10)
        //                {
        //                    sPassword = smobileNo.Substring(smobileNo.Length - 4, 4);
        //                }





        //                //if (Decimal.TryParse(sAmt.Replace(",", ""), out oAmt) == false)
        //                //{
        //                //    i++;
        //                //    continue;
        //                //}
        //                //나머지 값들 넣어보고
        //                string semail = getValueFrom2DArray(dic[emailDataGridViewTextBoxColumn], i);
        //                string sphoneNo = getValueFrom2DArray(dic[phoneNoDataGridViewTextBoxColumn], i);
        //                string sfaxNo = getValueFrom2DArray(dic[faxNoDataGridViewTextBoxColumn], i);


        //                DateTime sCreateDate = DateTime.Now;

        //                string saddressState = getValueFrom2DArray(dic[addressStateDataGridViewTextBoxColumn], i);
        //                string saddressCity = getValueFrom2DArray(dic[addressCityDataGridViewTextBoxColumn], i);
        //                string saddressDetail = getValueFrom2DArray(dic[addressDetailDataGridViewTextBoxColumn], i);

        //                string sbizType = getValueFrom2DArray(dic[bizTypeDataGridViewTextBoxColumn], i);
        //                string sRouteType = getValueFrom2DArray(dic[RouteType], i);
        //                string sinsuranceType = getValueFrom2DArray(dic[insuranceTypeDataGridViewTextBoxColumn], i);
        //                string sUsePayNow = "0";


        //                string scarNo = getValueFrom2DArray(dic[carNoDataGridViewTextBoxColumn], i);
        //                string scarType = getValueFrom2DArray(dic[carTypeDataGridViewTextBoxColumn], i);
        //                string scarSize = getValueFrom2DArray(dic[carSizeDataGridViewTextBoxColumn], i);
        //                string sGubun = getValueFrom2DArray(dic[Gubun], i);

        //                string scarYear = getValueFrom2DArray(dic[carYearDataGridViewTextBoxColumn], i);


        //                string sparkState = getValueFrom2DArray(dic[parkStateDataGridViewTextBoxColumn], i);
        //                string sparkCity = getValueFrom2DArray(dic[parkCityDataGridViewTextBoxColumn], i);
        //                string sparkStreet = getValueFrom2DArray(dic[parkStreetDataGridViewTextBoxColumn], i);
        //                int sCandidateId = LocalUser.Instance.LogInInformation.ClientId;
        //                string sFpisCartype = getValueFrom2DArray(dic[FpisCarType], i);
        //                iSum++;


        //                //이제 추가하고
        //                try
        //                {

        //                    CMDataSet.DriversRow iRow = cMDataSet.Drivers.NewDriversRow();
        //                    iRow.Code = sCode;
        //                    iRow.Name = sName;
        //                    iRow.BizNo = sbizNo.Replace("-", "");
        //                    iRow.CEO = sCeo;
        //                    iRow.Uptae = sUptae;
        //                    iRow.Upjong = sUpjong;
        //                    iRow.Email = semail;
        //                    iRow.LoginId = sLoginId;
        //                    iRow.Password = sPassword;

        //                    if (smobileNo.Length < 10)
        //                    {
        //                        iRow.MobileNo = "";
        //                    }
        //                    else
        //                    {
        //                        iRow.MobileNo = smobileNo;
        //                    }


        //                    if (sphoneNo.Length < 9)
        //                    {
        //                        iRow.PhoneNo = "";
        //                    }
        //                    else
        //                    {
        //                        iRow.PhoneNo = sphoneNo;
        //                    }

        //                    if (sfaxNo.Length < 11)
        //                    {
        //                        iRow.FaxNo = "";
        //                    }

        //                    else
        //                    {
        //                        iRow.FaxNo = sfaxNo;
        //                    }

        //                    iRow.CreateDate = sCreateDate;

        //                    iRow.AddressState = saddressState;
        //                    iRow.AddressCity = saddressCity;
        //                    iRow.AddressDetail = saddressDetail;

        //                    //if (String.IsNullOrEmpty(sbizType))
        //                    //{
        //                    //    iRow.BizType = 1;
        //                    //}
        //                    //else
        //                    //{
        //                    //    iRow.BizType = int.Parse(sbizType);
        //                    //}

        //                    var QBizType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BizType" && c.Name == scarType).ToArray();

        //                    if (QBizType.Any())
        //                    {
        //                        iRow.BizType = QBizType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.BizType = 1;
        //                    }
        //                    //if (String.IsNullOrEmpty(sRouteType))
        //                    //{
        //                    //    iRow.RouteType = 1;
        //                    //}
        //                    //else
        //                    //{
        //                    //    iRow.RouteType = int.Parse(sRouteType);
        //                    //}

        //                    var QRouteType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "RouteType" && c.Name == scarType).ToArray();

        //                    if (QRouteType.Any())
        //                    {
        //                        iRow.RouteType = QRouteType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.RouteType = 1;
        //                    }

        //                    var QInsuranceType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InsuranceType" && c.Name == scarType).ToArray();

        //                    if (QInsuranceType.Any())
        //                    {
        //                        iRow.InsuranceType = QInsuranceType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.InsuranceType = 1;
        //                    }
        //                    //if (String.IsNullOrEmpty(sinsuranceType))
        //                    //{
        //                    //    iRow.InsuranceType = 0;
        //                    //}

        //                    //else
        //                    //{
        //                    //    iRow.InsuranceType = int.Parse(sinsuranceType);

        //                    //}
        //                    iRow.CarNo = scarNo.Replace(" ", "");
        //                    var QcarType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && c.Name == scarType).ToArray();

        //                    if (QcarType.Any())
        //                    {
        //                        iRow.CarType = QcarType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.CarType = 5;
        //                    }

        //                    var QcarSize = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Name == scarSize).ToArray();
        //                    if (QcarSize.Any())
        //                    {
        //                        iRow.CarSize = QcarSize.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.CarSize = 1;
        //                    }
        //                    var Qgubun = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarGubun" && c.Name == sGubun).ToArray();
        //                    if (Qgubun.Any())
        //                    {
        //                        iRow.CarGubun = Qgubun.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.CarGubun = 1;
        //                    }
        //                    iRow.CarYear = scarYear;
        //                    iRow.ParkState = sparkState;
        //                    iRow.ParkCity = sparkCity;
        //                    iRow.ParkStreet = sparkStreet;

        //                    iRow.AccountOwner = "";
        //                    iRow.AccountRegNo = "";
        //                    iRow.BankName = "";
        //                    iRow.AccountNo = "";
        //                    iRow.AccountExtra = "";
        //                    if (String.IsNullOrEmpty(sUsePayNow))
        //                    {
        //                        iRow.UsePayNow = 2;
        //                    }
        //                    else
        //                    {
        //                        iRow.UsePayNow = int.Parse(sUsePayNow);
        //                    }
        //                    iRow.ClientBizType = 0;
        //                    //     row.CandidateId = int.Parse(cmb_CandidateGubun.SelectedValue.ToString());
        //                    iRow.CandidateId = LocalUser.Instance.LogInInformation.ClientId;

        //                    iRow.Car_ContRact = false;


        //                    iRow.AccountUse = false;
        //                    iRow.DTGUse = true;
        //                    iRow.FPISUse = true;
        //                    iRow.MyCallUSe = true;
        //                    iRow.OTGUse = false;
        //                    iRow.ServicePrice = "5500";
        //                    iRow.useTax = true;
        //                    iRow.OTGPrice = 0;
        //                    iRow.AccountPrice = 0;
        //                    iRow.FPISPrice = 0;
        //                    iRow.MyCallPrice = 0;
        //                    iRow.DTGPrice = 5000;
                           
        //                    var QFPisCarType = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "FPisCarType" && c.Name == sFpisCartype).ToArray();
        //                    if (QFPisCarType.Any())
        //                    {
        //                        iRow.FpisCarType = QFPisCarType.First().Value;
        //                    }
        //                    else
        //                    {
        //                        iRow.FpisCarType = 1;
        //                    }

        //                    //driversTableAdapter.Insert(sbizNo, sName, sCeo, sUptae, sUpjong, saddressState, saddressCity, smobileNo, sPassword, sphoneNo, sphoneNo, semail, int.Parse(sbizType), int.Parse(sRouteType), int.Parse(sinsuranceType), sCreateDate, scarNo, int.Parse(scarType), int.Parse(scarSize), scarYear, sparkState, sparkCity, "", "", "", "", "", "", "",int.Parse(null), sCode, sparkStreet, 0, sLoginId, saddressDetail, sCandidateId, 0);

        //                    cMDataSet.Drivers.AddDriversRow(iRow);

        //                    //TODO
        //                    driversTableAdapter.Update(iRow);

        //                    iNew++;

        //                }
        //                catch { iSkp++; }
        //                try
        //                {
        //                    f.Invoke(new Action(() =>
        //                    {
        //                        f.pBar.Value++;
        //                        f.lblProgress.Text = string.Format("{0}/{1}", f.pBar.Value, f.pBar.Maximum);
        //                    }));
        //                }
        //                catch { }
        //                i++;
        //            }
        //            f.Invoke(new Action(() =>
        //            {
        //                f.picStep3_1.Image = Properties.Resources.Done;
        //                f.picStep3_2.Width = 32;
        //                f.lblStep3_1.Text = lbl1;
        //                f.Cursor = Cursors.WaitCursor;
        //            }));
        //            f.Invoke(new Action(() =>
        //            {
        //                f.picStep3_2.Image = Properties.Resources.Done;
        //                f.lblStep3_2.Text = lbl2;
        //                f.Cursor = Cursors.Arrow;
        //                f.lblSum.Text = iSum.ToString() + "건";
        //                f.lblSkip.Text = iSkp.ToString() + "건";
        //                f.lblWrite.Text = (iSum - iSkp - iNew).ToString() + "건";
        //                f.lblNew.Text = iNew.ToString() + "건";
        //                f.tableLayoutPanel5.Visible = true;
        //                f.btnNext.Text = "완료";
        //                f.DicDone = false;
        //            }));
        //            f.Invoke(new Action(() =>
        //            {
        //                f.btnClose.Enabled = true;
        //                f.btnNext.Enabled = true;
        //                f.btnPrevious.Enabled = true;
        //            }));
        //        }));
        //        t.SetApartmentState(ApartmentState.STA);
        //        #endregion

        //        f.btnNext.Click += new EventHandler((object bsender, EventArgs be) =>
        //        {
        //            try
        //            {
        //                if (f.IsClose) return;
        //                if (f.CurrentIndex != 2) return;
        //                while (f.DicDone == false)
        //                    Thread.Sleep(100);
        //                t.Start();
        //            }
        //            catch { }
        //        });
        //        f.ShowDialog();
        //       // this.Invoke(new Action(() => btn_Search_Click(null, null)));
        //    }
        //    catch
        //    {
        //        (this as Form).ThisMessageBox("엑셀 불러오기가 실패하였습니다.");
        //    }

        //}

        private void btnExcelImport_Click_1(object sender, EventArgs e)
        {
            EXCELINSERT_DRIVER_DEFAULT_SG _Form = new EXCELINSERT_DRIVER_DEFAULT_SG();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if(!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                txt_Zip.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_State.Text = ss[0];
                txt_City.Text = ss[1];
                txt_Street.Text = String.Join(" ", ss.Skip(2));
            }
        }

        private void Control_Enter(object sender, EventArgs e)
        {
            RemoveError(sender as Control);
            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
        }

        private void txt_Name_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_Name.Text))
            {
                var Error = "차주 상호를 입력해주세요.";
                SetError(txt_Name, Error);
            }
        }

        private void txt_CEO_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_CEO.Text))
            {
                var Error = "대표자를 입력해주세요.";
                SetError(txt_CEO, Error);
            }
        }

        private void txt_Uptae_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_Uptae.Text))
            {
                var Error = "업태를 입력해주세요.";
                SetError(txt_Uptae, Error);
            }
        }

        private void txt_Upjong_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_Upjong.Text))
            {
                var Error = "업종을 입력해주세요.";
                SetError(txt_Upjong, Error);
            }
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_MobileNo.Text))
            {
                var _S = txt_MobileNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 7)
                {
                    _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                }
                if (_S.Length > 12)
                {
                    _S = _S.Replace("-", "");
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                }
                txt_MobileNo.Text = _S;
            }
        }

        private void txt_CarNo_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_CarNo.Text))
            {
                var Error = "차량번호를 입력해주세요.";
                SetError(txt_CarNo, Error);
            }
        }

        class ErrorModel
        {
            public ErrorModel(Control iControl)
            {
                _Color = iControl.BackColor;
                _Control = iControl;
            }
            public Control _Control { get; set; }
            public Color _Color { get; set; }
        }

        private List<ErrorModel> _ErrorModelList = new List<ErrorModel>();

        private void SetError(Control _Control, String _ErrorText)
        {
            if (_Control is TextBox && ((TextBox)_Control).ReadOnly)
                return;

            if (!_ErrorModelList.Any(c => c._Control == _Control))
            {
                _ErrorModelList.Add(new ErrorModel(_Control));
                _Control.BackColor = Color.FromArgb(0xff, 0xff, 0xf9, 0xc4);
                if (!(_Control.Tag is ToolTip))
                {
                    var _ToolTip = new ToolTip();
                    _ToolTip.ShowAlways = true;
                    _ToolTip.AutomaticDelay = 0;
                    _ToolTip.InitialDelay = 0;
                    _ToolTip.ReshowDelay = 0;
                    _ToolTip.ForeColor = Color.Red;
                    _ToolTip.BackColor = Color.White;
                    _Control.Tag = new ToolTip();
                }
                ((ToolTip)_Control.Tag).SetToolTip(_Control, _ErrorText);
                ((ToolTip)_Control.Tag).Show(_ErrorText, _Control);
            }
        }

        private void ClearError()
        {
            foreach (var _Model in _ErrorModelList)
            {
                _Model._Control.BackColor = _Model._Color;
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            _ErrorModelList.Clear();
        }

        private void RemoveError(Control _Control)
        {
            if(_ErrorModelList.Any(c=>c._Control == _Control))
            {
                var _Model = _ErrorModelList.First(c => c._Control == _Control);
                _Model._Control.BackColor = _Model._Color;
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
                _ErrorModelList.Remove(_Model);
            }
        }

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            Control_Enter(sender, e);
            //txt_MobileNo.Text = txt_MobileNo.Text.Replace("-", "");
            //txt_MobileNo.SelectionStart = txt_MobileNo.Text.Length;

            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;


        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }


        private void txt_CEOBirth_Enter(object sender, EventArgs e)
        {
            Control_Enter(sender, e);
        }

        private void txt_CEOBirth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_CEOBirth_Leave(object sender, EventArgs e)
        {
            if (!Regex.Match(txt_CEOBirth.Text, @"^\d{6}").Success)
            {
                var Error = "법인번호(생년월일)은 필수값입니다.";
                SetError(txt_CEOBirth, Error);
                return;
            }
        }

        private void btnCarNoExist_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_CarNo.Text))
            {
                MessageBox.Show("사업자번호를 입력한 후 확인을 눌려주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var DriverDataTable = new BaseDataSet.DriversDataTable();
            var _Repository = new DriverRepository();
            var IsMyCar = _Repository.IsMyCar(txt_CarNo.Text);
            if (IsMyCar)
            {
                MessageBox.Show("이미 등록되어 있는 매입처입니다.", "카드페이", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            _Repository.FindbyCarNo(DriverDataTable, txt_CarNo.Text);
            ValidateStart = true;
            if(DriverDataTable.Rows.Count == 0)
            {
                if(DriverId != 0)
                {
                    txt_Name.Clear();
                    txt_CEO.Clear();
                    txt_Uptae.Clear();
                    txt_Upjong.Clear();
                    txt_CEOBirth.Clear();
                    txt_Zip.Clear();
                    txt_State.Clear();
                    txt_City.Clear();
                    txt_Street.Clear();
                    txt_MobileNo.Clear();
                    txt_PhoneNo.Clear();
                }
                txt_Name.ReadOnly = false;
                txt_CEO.ReadOnly = false;
                txt_Uptae.ReadOnly = false;
                txt_Upjong.ReadOnly = false;
                txt_CEOBirth.ReadOnly = false;
                txt_Zip.ReadOnly = false;
                txt_State.ReadOnly = false;
                txt_City.ReadOnly = false;
                txt_Street.ReadOnly = false;
                btnFindZip.Enabled = true;
                txt_MobileNo.ReadOnly = false;
                txt_PhoneNo.ReadOnly = false;
                txt_Email.ReadOnly = false;
                DriverId = 0;
            } else
            {
                var _Driver = DriverDataTable[0];
                txt_Name.Text = _Driver.Name;
                txt_CEO.Text = _Driver.CEO;
                txt_Uptae.Text = _Driver.Uptae;
                txt_Upjong.Text = _Driver.Upjong;
                txt_CEOBirth.Text = _Driver.CEOBirth;
                txt_Zip.Text = _Driver.Zipcode;
                txt_State.Text = _Driver.AddressState;
                txt_City.Text = _Driver.AddressCity;
                txt_Street.Text = _Driver.AddressDetail;
                txt_CarNo.Text = _Driver.BizNo;
                btnFindZip.Enabled = false;
                txt_MobileNo.Text = _Driver.MobileNo;
                txt_PhoneNo.Text = _Driver.PhoneNo;

                txt_Name.ReadOnly = true;
                txt_CEO.ReadOnly = true;
                txt_Uptae.ReadOnly = true;
                txt_Upjong.ReadOnly = true;
                txt_CEOBirth.ReadOnly = true;
                txt_Zip.ReadOnly = true;
                txt_State.ReadOnly = true;
                txt_City.ReadOnly = true;
                txt_Street.ReadOnly = true;
                txt_MobileNo.ReadOnly = true;
                txt_PhoneNo.ReadOnly = true;
                txt_Email.ReadOnly = true;

                DriverId = _Driver.DriverId;
            }
            btnAdd.Enabled = true;
            btnAddClose.Enabled = true;
            ValidateStart = false;
        }

        private void txt_CarNo_TextChanged(object sender, EventArgs e)
        {
            txt_Name.ReadOnly = true;
            txt_CEO.ReadOnly = true;
            txt_Uptae.ReadOnly = true;
            txt_Upjong.ReadOnly = true;
            txt_CEOBirth.ReadOnly = true;
            txt_Zip.ReadOnly = true;
            txt_State.ReadOnly = true;
            txt_City.ReadOnly = true;
            txt_Street.ReadOnly = true;
            txt_MobileNo.ReadOnly = true;
            txt_PhoneNo.ReadOnly = true;
            btnFindZip.Enabled = false;
            btnAdd.Enabled = false;
            btnAddClose.Enabled = false;
        }

        private void txt_CarInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
                {
                    if (_S.Length > 2)
                    {
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                    }
                    if (_S.Length > 6)
                    {
                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                    }
                    if (_S.Length > 11)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                    }
                }
                else
                {
                    if (_S.Length > 3)
                    {
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                    }
                    if (_S.Length > 7)
                    {
                        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                _Txt.Text = _S;
            }
        }
    }
}
