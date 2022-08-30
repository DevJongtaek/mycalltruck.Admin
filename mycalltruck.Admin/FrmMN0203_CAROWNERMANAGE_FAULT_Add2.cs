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
using System.Web;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0203_CAROWNERMANAGE_FAULT_Add2 : Form
    {
        DESCrypt m_crypt = null;
        bool ValidateStart = false;
        int DriverId = 0;
        String _CarNo = "";
        public FrmMN0203_CAROWNERMANAGE_FAULT_Add2(string CarNo)
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");
            _CarNo = CarNo;
        }
        private void FrmMN0203_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            ValidateStart = true;
            _InitCmb();
            txt_CarNo.Focus();
            ValidateStart = false;

            txt_CarNo.Text = _CarNo;
            btnCarNoExist_Click(null, null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _UpdateDB(() => {
                txt_CarNo.Clear();
                txt_BizNo.Clear();
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
                cmb_PayBankName.SelectedIndex = 0;
                txt_PayAccountNo.Text = "";
                txt_AccountOwner.Clear();
                cmb_CarType.SelectedIndex = 0;
                cmb_CarSize.SelectedIndex = 0;

                txt_BizNo.ReadOnly = true;
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
                txt_PayAccountNo.ReadOnly = true;
                txt_AccountOwner.ReadOnly = true;

                cmb_PayBankName.Enabled = false;
                cmb_CarType.Enabled = false;
                cmb_CarSize.Enabled = false;

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
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void _UpdateDB(Action Done)
        {
            //
            bool HasError = false;
            List<String> ErrorArray = new List<string>();
            ClearError();

            var _Repository = new DriverRepository();
            var IsMyCar = _Repository.IsMyCar(txt_CarNo.Text);
            if (IsMyCar)
            {
               
            }
            else
            {
                _Repository.ConnectDriver(DriverId);
            }


            //if (DriverId > 0)
            //{
            //    try
            //    {
            //        DriverRepository mDriverRepository = new DriverRepository();
            //        mDriverRepository.ConnectDriver(DriverId);
            //    }
            //    catch (Exception ex)
            //    {
            //        pnProgress.Visible = false;
            //        MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return;
            //    }
            //    pnProgress.Visible = false;
            //    Done();
            //    return;
            //}
            if (!Regex.Match(txt_BizNo.Text, @"^\d{3}-\d{2}-\d{5}$").Success)
            {
                var Error = "바른 차주 사업자 번호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_BizNo, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Name.Text))
            {
                var Error = "차주 상호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_Name, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_CEO.Text))
            {
                //var Error = "대표자를 입력해주세요.";
                //ErrorArray.Add(Error);
                //SetError(txt_CEO, Error);
                //HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Uptae.Text))
            {
                //var Error = "업태를 입력해주세요.";
                //ErrorArray.Add(Error);
                //SetError(txt_Uptae, Error);
                //HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Upjong.Text))
            {
                //var Error = "업종을 입력해주세요.";
                //ErrorArray.Add(Error);
                //SetError(txt_Upjong, Error);
                //HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_Zip.Text))
            {
                //var Error = "우편번호 검색을 해주세요.";
                //ErrorArray.Add(Error);
                //SetError(btnFindZip, Error);
                //HasError = true;
            }
            if (txt_CEOBirth.Text.Length != 6 || txt_CEOBirth.Text.Contains(" "))
            {
             
                //var Error = "대표자 생년월일 6자리를 입력해주세요.";
                //ErrorArray.Add(Error);
                //SetError(btnFindZip, Error);
                //HasError = true;
            }
            if (!Regex.Match(txt_MobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}$").Success)
            {
                var Error = "핸드폰번호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_MobileNo, Error);
                HasError = true;
            }
            if (cmb_PayBankName.SelectedIndex < 1)
            {
                var Error = "은행명을 선택해주세요.";
                ErrorArray.Add(Error);
                SetError(cmb_PayBankName, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_PayAccountNo.Text))
            {
                var Error = "계좌번호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_PayAccountNo, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_AccountOwner.Text))
            {
                var Error = "예금주를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_AccountOwner, Error);
                HasError = true;
            }
            if (String.IsNullOrWhiteSpace(txt_CarNo.Text))
            {
                var Error = "차량번호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_CarNo, Error);
                HasError = true;
            }
            if (cmb_CarType.SelectedIndex < 1)
            {
                var Error = "차종을 선택해주세요.";
                ErrorArray.Add(Error);
                SetError(cmb_CarType, Error);
                HasError = true;
            }
            if (cmb_CarSize.SelectedIndex < 1)
            {
                var Error = "톤수를 선택해주세요.";
                ErrorArray.Add(Error);
                SetError(cmb_CarSize, Error);
                HasError = true;
            }
            if (HasError)
            {
                return;
            }
            pnProgress.Visible = true;
            string Mid = "edubill";
            string Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_PayBankName.SelectedValue.ToString(), txt_PayAccountNo.Text, txt_AccountOwner.Text, cmb_PayBankName.Text, Mid);

            string SBiz_NO = txt_BizNo.Text;
          
            string SName = txt_Name.Text.Trim();
            string SUptae = "";
            if (String.IsNullOrWhiteSpace(txt_Uptae.Text.Trim()))
            {
                SUptae = ".";
            }
            else
            {
                SUptae = txt_Uptae.Text;

            }
            string SUpjong = "";
            if (String.IsNullOrWhiteSpace(txt_Upjong.Text.Trim()))
            {
                SUpjong = ".";
            }
            else
            {
                SUpjong = txt_Upjong.Text;

            }
            string SCeo = txt_CEO.Text.Trim();
            if (String.IsNullOrWhiteSpace(txt_CEO.Text))
            {
                SCeo = ".";
            }
            else
            {
                SCeo = txt_CEO.Text;

            }
            string SCeoBirth = "";
            if (String.IsNullOrWhiteSpace(txt_CEOBirth.Text))
            {
                SCeoBirth = "000000";
            }
            else
            {
                SCeoBirth = txt_CEOBirth.Text;

            }
            string SMobileNo = txt_MobileNo.Text.Trim();
            string SState = "";
            if (String.IsNullOrWhiteSpace(txt_State.Text))
            {
                SState = ".";
            }
            else
            {
                SState = txt_State.Text;

            }
            string SCity = "";
            if (String.IsNullOrWhiteSpace(txt_City.Text))
            {
                SCity = ".";
            }
            else
            {
                SCity = txt_City.Text;

            }
            string SStreet = "";
            if (String.IsNullOrWhiteSpace(txt_Street.Text))
            {
                SStreet = ".";
            }
            else
            {
                SStreet = txt_Street.Text;
            }


            string SZip = "";
            if (String.IsNullOrWhiteSpace(txt_Zip.Text))
            {
                SZip = "00000";
            }
            else
            {
                SZip = txt_Zip.Text;
            }

            string SCarNo = txt_CarNo.Text.Trim();
            int _CarType = (int)cmb_CarType.SelectedValue;
            int _CarSize = (int)cmb_CarSize.SelectedValue;
            string SCarYear = SCeo;
            string _PayBankCode = cmb_PayBankName.SelectedValue.ToString();
            string SPayAccountNo = txt_PayAccountNo.Text.Trim();
            string SInputName = txt_AccountOwner.Text.Trim();
            string SCarstate = SState;
            string SCarcity = SCity;
            string SCarStreet = SStreet;
            string SCarInfo = txt_CarInfo.Text;
            string Account_Name = txt_AccountOwner.Text;
            int DealerId = int.Parse(cmb_Admin.SelectedValue.ToString());
            //  Task.Factory.StartNew(() => {
            WebClient mWebClient = new WebClient();
            try
            {
                //                    var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
               // var r = mWebClient.DownloadString(new Uri("http://localhost/Pay/AccCert" + Parameter));
                if (r == null || r.ToLower() != "true")
                {
                    var _Result2 = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));
                    //var _Result2 = mWebClient.DownloadString(new Uri("http://localhost/Pay/AccCertFirst" + Parameter));
                    _Result2 = HttpUtility.UrlDecode(_Result2);
                    if (_Result2 == "오류")
                    {
                        MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //Invoke(new Action(() => pnProgress.Visible = false));
                        //Invoke(new Action(() => MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                        return;
                    }
                    else
                    {
                        if (MessageBox.Show($"{cmb_PayBankName.Text} / {txt_PayAccountNo.Text} / {_Result2}\r\n위 계좌가 맞습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            return;
                        }
                        else
                        {
                            Account_Name = _Result2;
                        }




                    }

                    //Invoke(new Action(() => pnProgress.Visible=false));
                    //Invoke(new Action(() => MessageBox.Show("계좌인증이 실패하였습니다. 은행/계좌번호/예금주를 확인해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                    //return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //Invoke(new Action(() => pnProgress.Visible = false));
                //Invoke(new Action(() => MessageBox.Show("계좌인증 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                return;
            }

            try
            {
                DriverRepository mDriverRepository = new DriverRepository();
                
                var BankName = Filter.Bank.BankList.Where(c => c.Value == _PayBankCode).ToArray().First().Text;
              

                var DriverDataTable = new BaseDataSet.DriversDataTable();
               

                mDriverRepository.FindbyCarNo(DriverDataTable, txt_CarNo.Text);

                var _Driver = DriverDataTable[0];
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                            " UPDATE Drivers " +
                            " SET Zipcode = @ZipCode" +
                            " , AddressState = @AddressState" +
                            " , AddressCity = @AddressCity" +
                            " , AddressDetail = @AddressDetail" +
                            " , DealerId = @DealerId" +
                            " , PayBankName = @PayBankName" +
                            " , PayBankCode = @PayBankCode" +
                            " , PayAccountNo = @PayAccountNo" +
                            " , PayInputName = @PayInputName" +
                            " , MobileNo = @MobileNo" +
                            " , CarType = @CarType" +
                            " , CarSize = @CarSize" +
                            " , CarInfo = @CarInfo" +
                            " , CEOBirth = @CEOBirth" +
                            " , CEO = @CEO" +
                            " , Uptae = @Uptae" +
                            " , Upjong = @Upjong" +
                            " , Name = @Name" +
                            " , BizNo = @BizNo" +
                            " WHERE DriverId = @DriverId ";
                        _Command.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                        _Command.Parameters.AddWithValue("@Uptae", SUptae);
                        _Command.Parameters.AddWithValue("@Upjong", SUpjong);
                        _Command.Parameters.AddWithValue("@Name", txt_Name.Text);


                        _Command.Parameters.AddWithValue("@Zipcode", SZip);
                        _Command.Parameters.AddWithValue("@AddressState", SState);
                        _Command.Parameters.AddWithValue("@AddressCity", SCity);
                        _Command.Parameters.AddWithValue("@AddressDetail", SStreet);
                        _Command.Parameters.AddWithValue("@DealerId", cmb_Admin.SelectedValue);
                        _Command.Parameters.AddWithValue("@PayBankName", cmb_PayBankName.Text);
                        _Command.Parameters.AddWithValue("@PayBankCode", cmb_PayBankName.SelectedValue);
                        _Command.Parameters.AddWithValue("@PayAccountNo", txt_PayAccountNo.Text);
                        _Command.Parameters.AddWithValue("@PayInputName", Account_Name);
                        _Command.Parameters.AddWithValue("@MobileNo", txt_MobileNo.Text);
                        _Command.Parameters.AddWithValue("@CarType", cmb_CarType.SelectedValue);
                        _Command.Parameters.AddWithValue("@CarSize", cmb_CarSize.SelectedValue);
                        _Command.Parameters.AddWithValue("@CarInfo", txt_CarInfo.Text);
                        _Command.Parameters.AddWithValue("@CEOBirth", SCeoBirth);
                        _Command.Parameters.AddWithValue("@CEO", SCeo);
                        _Command.Parameters.AddWithValue("@DriverId", DriverId);
                        _Command.ExecuteNonQuery();
                    }
                }

              
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                //Invoke(new Action(() => pnProgress.Visible = false));
                //Invoke(new Action(() => MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                //return;
            }
            //Invoke(new Action(() => pnProgress.Visible = false));
            //Invoke(Done);
            //});

            Invoke(Done);

        }

        private void _InitCmb()
        {
            cmb_Admin.DisplayMember = "Text";
            cmb_Admin.ValueMember = "Value";
            cmb_Admin.DataSource = Filter.Dealer.DealerList;
            cmb_Admin.SelectedIndex = 1;


            var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarType.DataSource = CarTypeDataSource;
            cmb_CarType.DisplayMember = "Name";
            cmb_CarType.ValueMember = "Value";

            var CarSizeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarSize.DataSource = CarSizeDataSource;
            cmb_CarSize.DisplayMember = "Name";
            cmb_CarSize.ValueMember = "Value";

            Dictionary<string, string> PayBank = new Dictionary<string, string>();
            //PayBank.Add(" ", "은행선택");
            //PayBank.Add("003", "기업은행");
            //PayBank.Add("004", "국민은행");
            //PayBank.Add("011", "농협");
            //PayBank.Add("012", "단위농협");
            //PayBank.Add("020", "우리은행");
            //PayBank.Add("031", "대구은행");
            //PayBank.Add("005", "외한은행");
            //PayBank.Add("023", "SC제일은행");
            //PayBank.Add("032", "부산은행");
            //PayBank.Add("045", "새마을");
            //PayBank.Add("027", "한국시티은행");
            //PayBank.Add("034", "광주은행");
            //PayBank.Add("039", "경남은행");
            //PayBank.Add("007", "수협");
            //PayBank.Add("048", "신협");
            //PayBank.Add("037", "전북은행");
            //PayBank.Add("035", "제주은행");
            //PayBank.Add("064", "산림조합");
            //PayBank.Add("071", "우체국");
            //PayBank.Add("081", "하나은행");
            //PayBank.Add("088", "신한은행");
            //PayBank.Add("209", "동양종금증권");
            //PayBank.Add("243", "한국투자증권");
            //PayBank.Add("240", "삼성증권");
            //PayBank.Add("230", "미래에셋");
            //PayBank.Add("247", "우리투자증권");
            //PayBank.Add("218", "현대증권");
            //PayBank.Add("266", "SK증권");
            //PayBank.Add("278", "신한금융투자");
            //PayBank.Add("262", "하이증권");
            //PayBank.Add("263", "HMC증권");
            //PayBank.Add("267", "대신증권");
            //PayBank.Add("270", "하나대투증권");
            //PayBank.Add("279", "동부증권");
            //PayBank.Add("280", "유진증권");
            //PayBank.Add("287", "메리츠증권");
            //PayBank.Add("291", "신영증권");

            PayBank.Add(" ", "은행선택");
            PayBank.Add("003", "기업은행");
            PayBank.Add("005", "외한은행");
            PayBank.Add("004", "국민은행");
            PayBank.Add("011", "농협");
            PayBank.Add("020", "우리은행");
            PayBank.Add("088", "신한은행");
            PayBank.Add("023", "SC제일");
            PayBank.Add("027", "씨티은행");
            PayBank.Add("032", "부산은행");
            PayBank.Add("039", "경남은행");
            PayBank.Add("031", "대구은행");
            PayBank.Add("071", "우체국");
            PayBank.Add("034", "광주은행");
            PayBank.Add("007", "수협");
            PayBank.Add("022", "상업은행");
            PayBank.Add("030", "대동은행");
            PayBank.Add("033", "충청은행");
            PayBank.Add("035", "제주은행");
            PayBank.Add("036", "경기은행");
            PayBank.Add("037", "전북은행");
            PayBank.Add("038", "강원은행");
            PayBank.Add("040", "충북은행");
            PayBank.Add("081", "하나은행");
            PayBank.Add("082", "보람은행");
            PayBank.Add("002", "산업은행");
            PayBank.Add("045", "새마을금고");
            PayBank.Add("054", "HSBC은행");
            PayBank.Add("048", "신협");
            PayBank.Add("090", "카카오뱅크");
            PayBank.Add("089", "케이뱅크");
            PayBank.Add("S0", "유안타증권");
            PayBank.Add("S1", "미래에셋");
            PayBank.Add("S2", "신한금융투자");
            PayBank.Add("S3", "삼성증권");
            PayBank.Add("S6", "한국투자증권");
            PayBank.Add("SG", "한화증권");

            cmb_PayBankName.DataSource = new BindingSource(PayBank, null);
            cmb_PayBankName.DisplayMember = "Value";
            cmb_PayBankName.ValueMember = "Key";
            cmb_PayBankName.SelectedIndex = 0;
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
            //if (_DriverType == 1)
            //{
            //    EXCELINSERT_DRIVER_DEFAULT _Form = new EXCELINSERT_DRIVER_DEFAULT();
            //    _Form.Owner = this;
            //    _Form.StartPosition = FormStartPosition.CenterParent;
            //    _Form.ShowDialog(

            //        );
            //}

            //else
            //{
            //    EXCELINSERT_DRIVER _Form = new EXCELINSERT_DRIVER();
            //    _Form.Owner = this;
            //    _Form.StartPosition = FormStartPosition.CenterParent;
            //    _Form.ShowDialog(

            //        );
            //}
            using (EXCELINSERT_DRIVER_DEFAULT _Form = new EXCELINSERT_DRIVER_DEFAULT
            {
                Owner = this,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                _Form.ShowDialog();
            }
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

            if (!Regex.Match(txt_MobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success)
            {
                var Error = "핸드폰번호를 입력해주세요.";
                SetError(txt_MobileNo, Error);
                return;
            }

        }

        private void txt_PayAccountNo_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_PayAccountNo.Text))
            {
                var Error = "계좌번호를 입력해주세요.";
                SetError(txt_PayAccountNo, Error);
            }
        }

        private void txt_AccountOwner_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_AccountOwner.Text))
            {
                var Error = "예금주를 입력해주세요.";
                SetError(txt_AccountOwner, Error);
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

        private void cmb_PayBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ValidateStart)
                return;
            if (cmb_PayBankName.SelectedIndex < 1)
            {
                var Error = "은행명을 선택해주세요.";
                SetError(cmb_PayBankName, Error);
            }
            else
            {
                RemoveError(cmb_PayBankName);
            }
        }

        private void cmb_CarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ValidateStart)
                return;
            if (cmb_CarType.SelectedIndex < 1)
            {
                var Error = "차종을 선택해주세요.";
                SetError(cmb_CarType, Error);
            }
            else
            {
                RemoveError(cmb_CarType);
            }
        }

        private void cmb_CarSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ValidateStart)
                return;
            if (cmb_CarSize.SelectedIndex < 1)
            {
                var Error = "톤수를 선택해주세요.";
                SetError(cmb_CarSize, Error);
            }
            else
            {
                RemoveError(cmb_CarSize);
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

        private void txt_BizNo_Enter(object sender, EventArgs e)
        {
            Control_Enter(sender, e);
            txt_BizNo.Text = txt_BizNo.Text.Replace("-", "");
        }

        private void txt_BizNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_BizNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_BizNo.Text))
            {
                var _S = txt_BizNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 6)
                {
                    _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                }
                txt_BizNo.Text = _S;
            }
            if (!Regex.Match(txt_BizNo.Text, @"^\d{3}-\d{2}-\d{5}").Success)
            {
                var Error = "바른 차주 사업자 번호를 입력해주세요.";
                SetError(txt_BizNo, Error);
                return;
            }
            // 상호/업태/업종/대표자/주소 자동완성
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _BizNoCommand = _Connection.CreateCommand())
                {
                    _BizNoCommand.CommandText = "SELECT Name, Uptae, Upjong, CEO, AddressState, AddressCity, AddressDetail, Zipcode FROM Drivers WHERE BizNo = @BizNo AND serviceState <> 5";
                    _BizNoCommand.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                    using (SqlDataReader _Reader = _BizNoCommand.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            txt_Name.Text = _Reader.GetStringN(0);
                            txt_Uptae.Text = _Reader.GetStringN(1);
                            txt_Upjong.Text = _Reader.GetStringN(2);
                            txt_CEO.Text = _Reader.GetStringN(3);
                            txt_State.Text = _Reader.GetStringN(4);
                            txt_City.Text = _Reader.GetStringN(5);
                            txt_Street.Text = _Reader.GetStringN(6);
                            txt_Zip.Text = _Reader.GetStringN(7);
                        }
                    }
                }
                _Connection.Close();
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
                var Error = "대표자 생년월일은 필수값입니다.";
              //  var Error = "법인번호(생년월일)은 필수값입니다.";
                SetError(txt_CEOBirth, Error);
                return;
            }
        }

        private void btnCarNoExist_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_CarNo.Text))
            {
                MessageBox.Show("차량번호를 입력한 후 확인을 눌려주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var DriverDataTable = new BaseDataSet.DriversDataTable();
            var _Repository = new DriverRepository();

            var IsMyCar = _Repository.IsMyCar(txt_CarNo.Text);
            //if (IsMyCar)
            //{
            //    MessageBox.Show("이미 등록되어 있는 차량입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            _Repository.FindbyCarNo(DriverDataTable, txt_CarNo.Text);
            ValidateStart = true;
            if(DriverDataTable.Rows.Count == 0)
            {
                if(DriverId != 0)
                {
                    txt_BizNo.Clear();
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
                    cmb_PayBankName.SelectedIndex = 0;
                    txt_PayAccountNo.Text = "";
                    txt_AccountOwner.Clear();
                    cmb_CarType.SelectedIndex = 0;
                    cmb_CarSize.SelectedIndex = 0;
                    txt_CarInfo.Clear();
                }
                txt_BizNo.ReadOnly = false;
                txt_Name.ReadOnly = false;
                txt_CEO.ReadOnly = false;
                txt_Uptae.ReadOnly = false;
                txt_Upjong.ReadOnly = false;
                txt_CEOBirth.ReadOnly = false;
                txt_Zip.ReadOnly = true;
                txt_State.ReadOnly = true;
                txt_City.ReadOnly = true;
                txt_Street.ReadOnly = false;
                btnFindZip.Enabled = true;
                txt_MobileNo.ReadOnly = false;
                cmb_PayBankName.Enabled = true;
                txt_PayAccountNo.ReadOnly = false;
                txt_AccountOwner.ReadOnly = false;
                cmb_CarType.Enabled = true;
                cmb_CarSize.Enabled = true;
                txt_CarInfo.ReadOnly = false;

                DriverId = 0;
            }
            else
            {
                var _Driver = DriverDataTable[0];
                txt_BizNo.Text = _Driver.BizNo;
                txt_Name.Text = _Driver.Name;
                txt_CEO.Text = _Driver.CEO;
                txt_Uptae.Text = _Driver.Uptae;
                txt_Upjong.Text = _Driver.Upjong;
                txt_CEOBirth.Text = _Driver.CEOBirth;
                txt_Zip.Text = _Driver.Zipcode;
                txt_State.Text = _Driver.AddressState;
                txt_City.Text = _Driver.AddressCity;
                txt_Street.Text = _Driver.AddressDetail;
                btnFindZip.Enabled = true;
                txt_MobileNo.Text = _Driver.MobileNo;


                string ReferralName = "";
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT Top 1 ISNULL(cs.SangHo,N'') as ReferralName FROM ORDERS LEFT JOIN CUSTOMERS ON ORDERS.CustomerId = CUSTOMERS.CustomerId left JOIN Customers as Cs ON cs.CustomerId = orders.ReferralId WHERE Orders.DriverId = @DriverId Order by Orders.CreateTime desc";
                        _Command.Parameters.AddWithValue("@DriverId", _Driver.DriverId);
                        var o = _Command.ExecuteScalar();
                        if (o != null)
                            ReferralName = o.ToString();
                    }
                    _Connection.Close();
                }


                lblReferralName.Text = ReferralName;




                if (String.IsNullOrEmpty(_Driver.PayBankCode))
                {

                    cmb_PayBankName.SelectedValue = 0;
                    txt_PayAccountNo.Text = "";
                    txt_AccountOwner.Text = "";

                    cmb_PayBankName.Enabled = true;
                    txt_PayAccountNo.ReadOnly = false;
                    txt_AccountOwner.ReadOnly = false;
                }
                else
                {
                  
                    string tempString = string.Empty;
                    tempString = _Driver.PayAccountNo;
                    try
                    {
                        txt_PayAccountNo.Text = m_crypt.Decrypt(tempString);
                       
                    }
                    catch
                    {
                        txt_PayAccountNo.Text = tempString;
                       
                    }

                    cmb_PayBankName.SelectedValue = _Driver.PayBankCode;
                    //txt_PayAccountNo.Text = _Driver.PayAccountNo;
                    txt_AccountOwner.Text = _Driver.PayInputName;


                    if (LocalUser.Instance.LogInInformation.Client.CarBankYn)
                    {
                        if (_Driver.ServiceState != 1)
                        {
                            cmb_PayBankName.Enabled = true;
                            txt_PayAccountNo.ReadOnly = false;
                            txt_AccountOwner.ReadOnly = false;
                        }
                        else
                        {
                            cmb_PayBankName.Enabled = false;
                            txt_PayAccountNo.ReadOnly = true;
                            txt_AccountOwner.ReadOnly = true;
                        }
                    }
                    else
                    {
                        cmb_PayBankName.Enabled = false;
                        txt_PayAccountNo.ReadOnly = true;
                        txt_AccountOwner.ReadOnly = true;
                    }
                }

                
                


                cmb_CarType.SelectedValue = _Driver.CarType;
                cmb_CarSize.SelectedValue = _Driver.CarSize;
                txt_CarInfo.Text = _Driver.CarInfo;

                if (_Driver.ServiceState == 1)
                {
                    txt_BizNo.ReadOnly = true;
                }
                else
                {
                    txt_BizNo.ReadOnly = false;

                }
                txt_Name.ReadOnly = false;
                txt_CEO.ReadOnly = false;
                txt_Uptae.ReadOnly = false;
                txt_Upjong.ReadOnly = false;
                txt_CEOBirth.ReadOnly = false;
                txt_Zip.ReadOnly = true;
                txt_State.ReadOnly = true;
                txt_City.ReadOnly = true;
                txt_Street.ReadOnly = false;
                txt_MobileNo.ReadOnly = false;
               

                cmb_CarType.Enabled = true;
                cmb_CarSize.Enabled = true;

                txt_CarInfo.ReadOnly = false;

                DriverId = _Driver.DriverId;
            }
            btnAdd.Enabled = true;
            btnAddClose.Enabled = true;
            ValidateStart = false;
        }

        private void txt_CarNo_TextChanged(object sender, EventArgs e)
        {
            txt_BizNo.ReadOnly = true;
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
            cmb_PayBankName.Enabled = false;
            txt_PayAccountNo.ReadOnly = true;
            txt_AccountOwner.ReadOnly = true;
            cmb_CarType.Enabled = false;
            cmb_CarSize.Enabled = false;
            txt_CarInfo.ReadOnly = true;

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

        private void ShareExcelForm_Click(object sender, EventArgs e)
        {
            string fileString = "";
            byte[] ieExcel = null;

            fileString = "차량관리공유양식.xlsx";
            ieExcel = Properties.Resources.차량관리공유양식;

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

        private void ShareExcel_Click(object sender, EventArgs e)
        {
            using (EXCELINSERT_DRIVER_SHARE _Form = new EXCELINSERT_DRIVER_SHARE
            {
                Owner = this,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                _Form.ShowDialog();
            }
        }

        private void btnMisuExcelUp_Click(object sender, EventArgs e)
        {
            EXCELINSERT_DRIVER_MISU _Form = new EXCELINSERT_DRIVER_MISU();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }
        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }
        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender as Control != null)
                {
                    Button pnShortCut = sender as Control as Button;
                    if (pnShortCut.Enabled)
                    {
                        pnShortCut.BackColor = Color.Green;
                    }
                }
            }
            catch { }
        }


        public void Button_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (sender as Control != null)
                {
                    Button pnShortCut = sender as Control as Button;
                    // pnShortCut.Invalidate();
                    pnShortCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
                }
            }
            catch { }
        }
        private void btnMisuExcel_Click(object sender, EventArgs e)
        {
            var fileString = LocalUser.Instance.LogInInformation.ClientName + "_차량_미수미지급" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.차량_미수미지급;
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
    }
}
