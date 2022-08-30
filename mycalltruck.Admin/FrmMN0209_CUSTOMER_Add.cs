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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0209_CUSTOMER_Add : Form
    {
        int CustomerId = 0;
        public FrmMN0209_CUSTOMER_Add()
        {
            InitializeComponent();
            if (LocalUser.Instance.LogInInformation.ClientId != 290)
            {
                lbl_EndDay.Visible = false;
                cmb_EndDay.Visible = false;
            }
        }

        private void FrmMN0209_CUSTOMER_Add_Load(object sender, EventArgs e)
        {
              ValidateStart = true;
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            _InitCmb();
            SetCustomerList();
            CustomerCode_Add();
            txt_BizNo.Focus();
            ValidateStart = true;
        }

        #region ACTION
        private void btnExcelImport_Click(object sender, EventArgs e)
        {
            EXCELINSERT_CUSTOMER _Form = new EXCELINSERT_CUSTOMER();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }
        private void btn_DriverExcel_Click(object sender, EventArgs e)
        {
            var fileString = "거래처정보_일괄등록_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.Customer11;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);

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

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnFindZip_Click(object sender, EventArgs e)
        {
            //foreach (var _Model in _ErrorModelList)
            //{
            //    ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            //}
            //FindZip f = new Admin.FindZip();
            //f.ShowDialog();
            //if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            //{
            //    txt_Zip.Text = f.Zip;
            //    var ss = f.Address.Split(' ');
            //    txt_State.Text = ss[0];
            //    txt_City.Text = ss[1];
            //    txt_Street.Text = String.Join(" ", ss.Skip(2));
            //}
            FindZipNew f = new Admin.FindZipNew();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                if (f.rdoRoad.Checked)
                {
                    txt_Zip.Text = f.Zip;
                    var ss = f.Address.Split(' ');
                    txt_State.Text = ss[0];
                    txt_City.Text = ss[1];
                    txt_Street.Text = String.Join(" ", ss.Skip(2));
                    txt_Street.Focus();
                }
                else if (f.rdoJibun.Checked)
                {
                    var ss = f.Jibun.Split(' ');
                    txt_Zip.Text = f.Zip;
                    txt_State.Text = ss[0];
                    txt_City.Text = ss[1];
                    txt_Street.Text = String.Join(" ", ss.Skip(2));

                    txt_Street.Focus();
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            _UpdateDB(() => {
                CustomerCode_Add();
                txt_BizNo.Text = "";
                txt_Name.Text = "";
                txt_CEO.Text = "";
                txt_Uptae.Text = "";
                txt_Upjong.Text = "";
                txt_Zip.Clear();
                txt_State.Clear();
                txt_City.Clear();
                txt_Street.Clear();

                cmb_Gubun.SelectedIndex = 0;
                txt_RegisterNo.Text = "";
                cmb_SalesGubun.SelectedIndex = 0;

                txt_Email.Text = "";
                txt_PhoneNo.Text = "";
                txt_FaxNo.Text = "";
                txt_ChargeName.Text = "";
                txt_MobileNo.Text = "";

                txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
                txtRemark.Text = "";

                txt_LoginId.Text = "";
                txt_Password.Text = "";
                cmbCustomerMId.SelectedIndex = 0;
                txt_BizNo.Focus();

                txt_Name.ReadOnly = true;
                txt_CEO.ReadOnly = true;
                txt_Uptae.ReadOnly = true;
                txt_Upjong.ReadOnly = true;
                //txt_Zip.ReadOnly = true;
                txt_State.ReadOnly = true;
                txt_City.ReadOnly = true;
                txt_Street.ReadOnly = true;
                btnFindZip.Enabled = false;
              //  cmb_Gubun.Enabled = false;
                txt_RegisterNo.ReadOnly = true;
                cmb_SalesGubun.Enabled = false;
                txt_Email.ReadOnly = true;
                txt_PhoneNo.ReadOnly = true;
                txt_FaxNo.ReadOnly = true;
                txt_ChargeName.ReadOnly = true;
                txt_MobileNo.ReadOnly = true;
                txtRemark.ReadOnly = true;
                txt_LoginId.ReadOnly = true;
                txt_Password.ReadOnly = true;

                btnFindZip.Enabled = false;
                btnAdd.Enabled = false;
                btnAddClose.Enabled = false;
                CustomerId = 0;
            });
        }
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            _UpdateDB(() => Close());
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
                var Error = "성명/상호를 입력해주세요.";
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

       
        private void cmb_SalesGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ValidateStart)
                return;
            //if (cmb_SalesGubun.SelectedIndex < 1)
            //{
            //    var Error = "거래처구분을 선택해주세요.";
            //    SetError(cmb_SalesGubun, Error);
            //}
            //else
            //{
            //    RemoveError(cmb_SalesGubun);
            //}
            if(LocalUser.Instance.LogInInformation.ClientId == 290)
            {
                if (cmb_SalesGubun.SelectedIndex == 1)
                {
                    lbl_EndDay.Visible = false;
                    cmb_EndDay.Visible = false;

                }
                else
                {
                    lbl_EndDay.Visible = true;
                    cmb_EndDay.Visible = true;

                }
            }
        }

        private void txt_Email_Leave(object sender, EventArgs e)
        {
            if (!Regex.Match(txt_Email.Text, @"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$").Success)
            {
                var Error = "e-메일을 입력해주세요.";
                SetError(txt_Email, Error);
                return;
            }
        }

        #endregion

        #region UPDATE
        private void _UpdateDB(Action Done)
        {
            bool HasError = false;
            List<String> ErrorArray = new List<string>();
            ClearError();
            InitCompareTable();

            if (CustomerId > 0)
            {
                //try
                //{
                //    CustomerRepository mCustomerRepository = new CustomerRepository();
                //    mCustomerRepository.ConnectCustomer(CustomerId);
                //}
                //catch (Exception ex)
                //{
                //    pnProgress.Visible = false;
                //    MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //    return;
                //}
                //pnProgress.Visible = false;
                //Done();
                //return;
            }
            else
            {
                if (!Regex.Match(txt_BizNo.Text, @"^\d{3}-\d{2}-\d{5}").Success)
                {
                    var Error = "바른 사업자 번호를 입력해주세요.";
                    ErrorArray.Add(Error);
                    SetError(txt_BizNo, Error);
                    HasError = true;
                }
                else
                {
                    if (!LocalUser.Instance.LogInInformation.AllowMultiCustomer)
                    {
                        if (BizNoCompareTable.Contains(txt_BizNo.Text))
                        {
                            var Error = "등록되어 있는 사업자 번호입니다.";
                            ErrorArray.Add(Error);
                            SetError(txt_BizNo, Error);
                            HasError = true;
                        }
                    }
                }
                if (String.IsNullOrWhiteSpace(txt_Name.Text))
                {
                    var Error = "성명/상호를 입력해주세요.";
                    ErrorArray.Add(Error);
                    SetError(txt_Name, Error);
                    HasError = true;
                }
                else if (LocalUser.Instance.LogInInformation.AllowMultiCustomer)
                {
                    if (SanghoCompareTable.Contains(txt_Name.Text))
                    {
                        var Error = "등록되어 있는 성명/상호입니다.";
                        ErrorArray.Add(Error);
                        SetError(txt_Name, Error);
                        HasError = true;
                    }
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
                if (txt_PhoneNo.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    var Error = "전화번호를 입력해주세요.";
                    ErrorArray.Add(Error);
                    SetError(txt_PhoneNo, Error);
                    HasError = true;
                }
                if (txt_MobileNo.Text.Replace(" ", "").Replace("-", "") == "")
                {
                    var Error = "핸드폰번호를 입력해주세요.";
                    ErrorArray.Add(Error);
                    SetError(txt_MobileNo, Error);
                    HasError = true;
                }

                if (!Regex.Match(txt_Email.Text, @"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$").Success)
                {
                    var Error = "e-메일을 입력해주세요.";
                    ErrorArray.Add(Error);
                    SetError(txt_Email, Error);
                    HasError = true;
                }
                //if (cmb_SalesGubun.SelectedIndex < 1)
                //{
                //    var Error = "거래처구분을 선택해주세요.";
                //    ErrorArray.Add(Error);
                //    SetError(cmb_SalesGubun, Error);
                //    HasError = true;
                //}
                if (HasError)
                {
                    return;
                }
                pnProgress.Visible = true;
                string SBiz_NO = txt_BizNo.Text.Trim();
                string SName = txt_Name.Text.Trim();
                string SUptae = txt_Uptae.Text.Trim();
                string SUpjong = txt_Upjong.Text.Trim();
                string SCeo = txt_CEO.Text.Trim();
                string SCharge = txt_ChargeName.Text.Trim();
                string SRegisterNo = txt_RegisterNo.Text.Trim();
                string SMobileNo = txt_MobileNo.Text.Trim();
                string SPhoneNo = txt_PhoneNo.Text.Trim();
                string SFaxNo = txt_FaxNo.Text.Trim();
                string SEmail = txt_Email.Text.Trim();
                string SState = txt_State.Text.Trim();
                string SCity = txt_City.Text.Trim();
                string SStreet = txt_Street.Text.Trim();
                string SZip = txt_Zip.Text.Trim();
                int _Gubun = (int)cmb_Gubun.SelectedValue;
                int _SalesGubun = (int)cmb_SalesGubun.SelectedValue;
                string SRemark = txtRemark.Text;
                string LoginId = txt_LoginId.Text;
                string Password = txt_Password.Text;

                int _SalesDay = (int)nudSalesDay.Value;
                int _CustomerManagerId = (int)cmbCustomerMId.SelectedValue;
                Task.Factory.StartNew(() => {
                    try
                    {
                        InitCompareTable();
                        var InsertQuery =
                            @"INSERT INTO Customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Zipcode, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, SubClientId, EndDay, ClientUserId, LoginId, Password,CustomerManagerId,Remark,SalesDay)
                        VALUES (@Code, @BizNo, @SangHo, @Ceo, @Uptae, @Upjong, @AddressState, @AddressCity, @AddressDetail, @Zipcode, @BizGubun, @ResgisterNo, @SalesGubun, @Email, @PhoneNo, @FaxNo, @ChargeName, @MobileNo, @CreateTime, @ClientId, @SubClientId, @EndDay, @ClientUserId, @LoginId, @Password,@CustomerManagerId,@Remark,@SalesDay) SELECT @@identity";
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
                                _Command.Parameters.Add("@Zipcode", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@BizGubun", SqlDbType.Int);
                                _Command.Parameters.Add("@ResgisterNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@SalesGubun", SqlDbType.Int);
                                _Command.Parameters.Add("@Email", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@PhoneNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@FaxNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@ChargeName", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@MobileNo", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@CreateTime", SqlDbType.DateTime);
                                _Command.Parameters.Add("@ClientId", SqlDbType.Int);
                                _Command.Parameters.Add("@SubClientId", SqlDbType.Int);
                                _Command.Parameters.Add("@EndDay", SqlDbType.Int);
                                _Command.Parameters.Add("@ClientUserId", SqlDbType.Int);
                                _Command.Parameters.Add("@Remark", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@CustomerManagerId", SqlDbType.Int);
                                _Command.Parameters.Add("@SalesDay", SqlDbType.Int);
                                //_Command.Parameters.Add("@LoginId", SqlDbType.NVarChar);
                                //_Command.Parameters.Add("@Password", SqlDbType.NVarChar);

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
                                _Command.Parameters["@BizNo"].Value = SBiz_NO;
                                _Command.Parameters["@SangHo"].Value = SName;
                                _Command.Parameters["@Ceo"].Value = SCeo;
                                _Command.Parameters["@Uptae"].Value = SUptae;
                                _Command.Parameters["@Upjong"].Value = SUpjong;
                                _Command.Parameters["@AddressState"].Value = SState;
                                _Command.Parameters["@AddressCity"].Value = SCity;
                                _Command.Parameters["@AddressDetail"].Value = SStreet;
                                _Command.Parameters["@Zipcode"].Value = SZip;
                                _Command.Parameters["@BizGubun"].Value = _Gubun;
                                _Command.Parameters["@ResgisterNo"].Value = SRegisterNo;
                                _Command.Parameters["@SalesGubun"].Value = _SalesGubun;
                                _Command.Parameters["@Email"].Value = SEmail;
                                _Command.Parameters["@PhoneNo"].Value = SPhoneNo;
                                _Command.Parameters["@FaxNo"].Value = SFaxNo;
                                _Command.Parameters["@ChargeName"].Value = SCharge;
                                _Command.Parameters["@MobileNo"].Value = SMobileNo;
                                _Command.Parameters["@CreateTime"].Value = DateTime.Now;
                                _Command.Parameters["@ClientId"].Value = LocalUser.Instance.LogInInformation.ClientId;
                                _Command.Parameters["@SubClientId"].Value = DBNull.Value;
                                _Command.Parameters["@ClientUserId"].Value = DBNull.Value;
                                _Command.Parameters["@Remark"].Value = SRemark;
                                _Command.Parameters["@CustomerManagerId"].Value = _CustomerManagerId;
                                _Command.Parameters["@SalesDay"].Value = _SalesDay;
                                //_Command.Parameters["@LoginId"].Value = LoginId;
                                //_Command.Parameters["@Password"].Value = Password;
                                if (LocalUser.Instance.LogInInformation.IsSubClient)
                                {
                                    _Command.Parameters["@SubClientId"].Value = LocalUser.Instance.LogInInformation.SubClientId;
                                    if (LocalUser.Instance.LogInInformation.IsAgent)
                                    {
                                        _Command.Parameters["@ClientUserId"].Value = LocalUser.Instance.LogInInformation.ClientUserId;
                                    }
                                }


                                if (_SalesGubun != 2)
                                {
                                    _Command.Parameters["@EndDay"].Value = (int)cmb_EndDay.SelectedValue;
                                }
                                else
                                {
                                    _Command.Parameters["@EndDay"].Value = 0;
                                }
                                if (LocalUser.Instance.LogInInformation.Client.CustomerPay)
                                {
                                    _Command.Parameters.AddWithValue("@LoginId", "c" + SBiz_NO.Replace("-", ""));
                                    _Command.Parameters.AddWithValue("@Password", SBiz_NO.Replace("-", "").Substring(5));
                                }
                                else
                                {
                                    _Command.Parameters.AddWithValue("@LoginId", LoginId);
                                    _Command.Parameters.AddWithValue("@Password", Password);
                                }
                                CustomerId = Convert.ToInt32(_Command.ExecuteScalar());
                             
                            }
                            _Connection.Close();
                        }

                        //CustomerRepository mCustomerRepository = new CustomerRepository();
                        //mCustomerRepository.ConnectCustomer(CustomerId);


                        try
                        {
                            //Data.Connection(_Connection =>
                            //{
                             

                            //    using (SqlCommand _CCommand = _Connection.CreateCommand())
                            //    {
                            //        _CCommand.CommandText =
                            //        @" UPDATE CustomerInstances
                            //        SET 
                            //      NRemark = @Remark
                         
                            //        WHERE CustomerId = @CustomerId
                            //        AND ClientId = @ClientId";

                            //        _CCommand.Parameters.AddWithValue("@Remark", txtRemark.Text);

                            //        _CCommand.Parameters.AddWithValue("@CustomerId", CustomerId);
                            //        _CCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                            //        _CCommand.ExecuteNonQuery();
                            //    }
                            //});

                        }
                        catch { }
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => pnProgress.Visible = false));
                        Invoke(new Action(() => MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                        return;
                    }
                    Invoke(new Action(() => pnProgress.Visible = false));
                    Invoke(Done);
                });
            }

            
        }
        #endregion

        #region STORAGE
        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();
        private List<String> SanghoCompareTable = new List<String>();
        private void InitCompareTable()
        {
            CodeCompareTable.Clear();
            BizNoCompareTable.Clear();
            SanghoCompareTable.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (var _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT Code,BizNo,Sangho FROM Customers WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            CodeCompareTable.Add(_Reader.GetString(0));
                            BizNoCompareTable.Add(_Reader.GetString(1));
                            SanghoCompareTable.Add(_Reader.GetString(2));
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
        #endregion

        #region HELP
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
            if (_ErrorModelList.Any(c => c._Control == _Control))
            {
                var _Model = _ErrorModelList.First(c => c._Control == _Control);
                _Model._Control.BackColor = _Model._Color;
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
                _ErrorModelList.Remove(_Model);
            }
        }
        private void CustomerCode_Add()
        {
            InitCompareTable();
            int CustomerCode = 1001;
            while (true)
            {
                if (!CodeCompareTable.Any(c => c == CustomerCode.ToString()))
                {
                    break;
                }
                CustomerCode++;
            }
            txt_Code.Text = CustomerCode.ToString();
        }
        private void _InitCmb()
        {
            var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NCustomerGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun.DataSource = GubunSourceDataSource;
            cmb_Gubun.DisplayMember = "Name";
            cmb_Gubun.ValueMember = "Value";

            var SalesSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "SalesGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_SalesGubun.DataSource = SalesSourceDataSource;
            cmb_SalesGubun.DisplayMember = "Name";
            cmb_SalesGubun.ValueMember = "Value";

            Dictionary<int, string> DCustomer = new Dictionary<int, string>();

            customerManagerTableAdapter.Fill(customerManagerDataSet.CustomerManager);
            var cmbCustomerMIdDataSource = customerManagerDataSet.CustomerManager.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.ManagerName, c.ManagerId }).OrderBy(c => c.ManagerId).ToArray();

            DCustomer.Add(0, "전체");


            foreach (var item in cmbCustomerMIdDataSource)
            {
                DCustomer.Add(item.ManagerId, item.ManagerName);
            }


            cmbCustomerMId.DataSource = new BindingSource(DCustomer, null);
            cmbCustomerMId.DisplayMember = "Value";
            cmbCustomerMId.ValueMember = "Key";


            Dictionary<int, int> EndDay = new Dictionary<int, int>();
            for (int i = 1; i <= 31; i++)
            {


                EndDay.Add(i, i);

            }

            cmb_EndDay.DataSource = new BindingSource(EndDay, null);
            cmb_EndDay.DisplayMember = "Value";
            cmb_EndDay.ValueMember = "Key";


        }
        bool ValidateStart = false;
        #endregion

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            //txt_PhoneNo.Text = txt_PhoneNo.Text.Replace("-", "");
            //txt_PhoneNo.SelectionStart = txt_PhoneNo.Text.Length;
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;

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

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_FaxNo_Leave(object sender, EventArgs e)
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

        private void txt_FaxNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_FaxNo_Enter(object sender, EventArgs e)
        {
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

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;
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

        private void txt_BizNo_Enter(object sender, EventArgs e)
        {
            RemoveError(txt_BizNo);
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
                var Error = "바른 사업자 번호를 입력해주세요.";
                SetError(txt_BizNo, Error);
                return;
            }
        }

        private void txt_RegisterNo_Enter(object sender, EventArgs e)
        {
            txt_RegisterNo.Text = txt_RegisterNo.Text.Replace("-", "");
        }

        private void txt_RegisterNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_RegisterNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_RegisterNo.Text))
            {
                var _S = txt_RegisterNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 6)
                {
                    _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                }
                txt_RegisterNo.Text = _S;
            }
        }

        //미수금 일괄등록
        private void btnMisuExcelUp_Click(object sender, EventArgs e)
        {
            EXCELINSERT_CUSTOMER_MISU _Form = new EXCELINSERT_CUSTOMER_MISU();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        //미수금다운
        private void btnMisuExcel_Click(object sender, EventArgs e)
        {
            var fileString = "거래처_미수금_일괄등록_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.Customer2;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);

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

        private void btnCustomerExist_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_BizNo.Text))
            {
                MessageBox.Show("사업자번호를 입력한 후 확인을 눌려주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var CustomersDataTable = new BaseDataSet.CustomersDataTable();
            var _Repository = new CustomerRepository();
            var IsMyCustomer = _Repository.IsMyCustomer(txt_BizNo.Text);
            if (IsMyCustomer)
            {
                MessageBox.Show("이미 등록되어 있는 사업자번호입니다..", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            _Repository.FindbyBizNo(CustomersDataTable, txt_BizNo.Text);
            ValidateStart = true;

            if (CustomersDataTable.Rows.Count == 0)
            {
                if (CustomerId != 0)
                {
                    txt_Name.Clear();
                    txt_CEO.Clear();
                    txt_Uptae.Clear();
                    txt_Upjong.Clear();
                    txt_Zip.Clear();
                    txt_State.Clear();
                    txt_City.Clear();
                    txt_Street.Clear();
                    cmb_Gubun.SelectedIndex = 0;
                    txt_RegisterNo.Clear();
                    cmb_SalesGubun.SelectedIndex = 0;
                    txt_Email.Clear();
                    txt_PhoneNo.Clear();
                    txt_FaxNo.Clear();
                    txt_ChargeName.Clear();
                    txt_MobileNo.Clear();
                    txtRemark.Clear();
                    txt_LoginId.Clear();
                    txt_Password.Clear();

                }
                txt_Name.ReadOnly = false;
                txt_CEO.ReadOnly = false;
                txt_Uptae.ReadOnly = false;
                txt_Upjong.ReadOnly = false;
               // txt_Zip.ReadOnly = false;
                txt_State.ReadOnly = false;
                txt_City.ReadOnly = false;
                txt_Street.ReadOnly = false;
                btnFindZip.Enabled = true;
              //  cmb_Gubun.Enabled = true;
                txt_RegisterNo.ReadOnly = false;
                cmb_SalesGubun.Enabled = true;
                txt_Email.ReadOnly = false;
                txt_PhoneNo.ReadOnly = false;
                txt_FaxNo.ReadOnly = false;
                txt_ChargeName.ReadOnly = false;
                txt_MobileNo.ReadOnly = false;
                txtRemark.ReadOnly = false;
                txt_LoginId.ReadOnly = false;
                txt_Password.ReadOnly = false;
                CustomerId = 0;
            }
            else
            {
                var _Customer = CustomersDataTable[0];

                txt_Name.Text = _Customer.SangHo;
                txt_CEO.Text = _Customer.Ceo;
                txt_Uptae.Text = _Customer.Uptae;
                txt_Upjong.Text = _Customer.Upjong;
                txt_Zip.Text = _Customer.Zipcode;
                txt_State.Text = _Customer.AddressState;
                txt_City.Text = _Customer.AddressCity;
                txt_Street.Text = _Customer.AddressDetail;
                btnFindZip.Enabled = false;
                cmb_Gubun.SelectedValue = _Customer.BizGubun;
                txt_RegisterNo.Text = _Customer.ResgisterNo;
                cmb_SalesGubun.SelectedValue = _Customer.SalesGubun;
                txt_Email.Text = _Customer.Email;
                txt_PhoneNo.Text = _Customer.PhoneNo;
                txt_FaxNo.Text = _Customer.FaxNo;
                txt_ChargeName.Text = _Customer.ChargeName;
                txt_MobileNo.Text = _Customer.MobileNo;
               // txtRemark.Text = _Customer.Remark;



                txt_Name.ReadOnly = true;
                txt_CEO.ReadOnly = true;
                txt_Uptae.ReadOnly = true;
                txt_Upjong.ReadOnly = true;
               // txt_Zip.ReadOnly = true;
                txt_State.ReadOnly = true;
                txt_City.ReadOnly = true;
                txt_Street.ReadOnly = true;
                btnFindZip.Enabled = false;
               // cmb_Gubun.Enabled = false;
                txt_RegisterNo.ReadOnly = true;
                cmb_SalesGubun.Enabled = false;
                txt_Email.ReadOnly = true;
                txt_PhoneNo.ReadOnly = true;
                txt_FaxNo.ReadOnly = true;
                txt_ChargeName.ReadOnly = true;
                txt_MobileNo.ReadOnly = true;
                txtRemark.ReadOnly = true;
                txt_LoginId.ReadOnly = true;
                txt_Password.ReadOnly = true;



                CustomerId = _Customer.CustomerId;
            }
            btnAdd.Enabled = true;
            btnAddClose.Enabled = true;
            ValidateStart = false;
        }

        private void txt_BizNo_TextChanged(object sender, EventArgs e)
        {

            txt_Name.ReadOnly = true;
            txt_CEO.ReadOnly = true;
            txt_Uptae.ReadOnly = true;
            txt_Upjong.ReadOnly = true;
           // txt_Zip.ReadOnly = true;
            txt_State.ReadOnly = true;
            txt_City.ReadOnly = true;
            txt_Street.ReadOnly = true;
            btnFindZip.Enabled = false;
           // cmb_Gubun.Enabled = false;
            txt_RegisterNo.ReadOnly = true;
            cmb_SalesGubun.Enabled = false;
            txt_Email.ReadOnly = true;
            txt_PhoneNo.ReadOnly = true;
            txt_FaxNo.ReadOnly = true;
            txt_ChargeName.ReadOnly = true;
            txt_MobileNo.ReadOnly = true;
            txtRemark.ReadOnly = true;
            txt_LoginId.ReadOnly = true;
            txt_Password.ReadOnly = true;


            btnFindZip.Enabled = false;
            btnAdd.Enabled = false;
            btnAddClose.Enabled = false;
          
        }

        private void cmb_Gubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int64 BizNo = 9999900001;

            var Query = CustomerModelList.Where(c => c.BizGubun == 5).OrderByDescending(c => c.BizNo.Replace("-", "")).ToArray();

            if (Query.Any())
            {


                BizNo = Convert.ToInt64(Query.First().BizNo.Replace("-", "")) + 1;
            }

            if (cmb_Gubun.Text == "도착지")
            {
                txt_BizNo.Text = BizNo.ToString();
                txt_BizNo_Leave(null, null);
                txt_CEO.Text = ".";
                txt_Uptae.Text = ".";
                txt_Upjong.Text = ".";
                txt_Email.Text = "111@naver.com";
                txt_MobileNo.Text = "010-0000-0000";

                btnCustomerExist_Click(null, null);

            }
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    if (sender as Control != null)
            //    {
            //        Button pnShortCut = sender as Control as Button;
            //        if (pnShortCut.Enabled)
            //        {
            //            pnShortCut.BackColor = Color.Green;
            //        }
            //    }
            //}
            //catch { }
        }


        public void Button_MouseLeave(object sender, EventArgs e)
        {
            //try
            //{
            //    if (sender as Control != null)
            //    {
            //        Button pnShortCut = sender as Control as Button;
            //        // pnShortCut.Invalidate();
            //        pnShortCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
            //    }
            //}
            //catch { }

        }
    }
}
