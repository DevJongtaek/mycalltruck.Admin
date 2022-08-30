using mycalltruck.Admin.Class.Common;
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
    public partial class FrmMN0209_CUSTOMER_Add_SG : Form
    {
        public FrmMN0209_CUSTOMER_Add_SG()
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
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            _InitCmb();
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
            var fileString = "거래처관리입력양식_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.거래처관리입력양식;
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnFindZip_Click(object sender, EventArgs e)
        {
            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                txt_Zip.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_State.Text = ss[0];
                txt_City.Text = ss[1];
                txt_Street.Text = String.Join(" ", ss.Skip(2));
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
                txt_BizNo.Focus();
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
            if (!Regex.Match(txt_PhoneNo.Text, @"^0\d{1,2}-\d{3,4}-\d{4}$").Success)
            {
                var Error = "전화번호를 입력해주세요.";
                ErrorArray.Add(Error);
                SetError(txt_PhoneNo, Error);
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
            Task.Factory.StartNew(() => {
                try
                {
                    InitCompareTable();
                    var InsertQuery =
                        @"INSERT INTO Customers (Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Zipcode, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, CreateTime, ClientId, SubClientId, EndDay, ClientUserId, LoginId, Password)
                        VALUES (@Code, @BizNo, @SangHo, @Ceo, @Uptae, @Upjong, @AddressState, @AddressCity, @AddressDetail, @Zipcode, @BizGubun, @ResgisterNo, @SalesGubun, @Email, @PhoneNo, @FaxNo, @ChargeName, @MobileNo, @CreateTime, @ClientId, @SubClientId, @EndDay, @ClientUserId, @LoginId, @Password)";
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
                                _Command.Parameters["@EndDay"].Value =(int)cmb_EndDay.SelectedValue;
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
                                _Command.Parameters.AddWithValue("@LoginId", DBNull.Value);
                                _Command.Parameters.AddWithValue("@Password", DBNull.Value);
                            }
                            _Command.ExecuteNonQuery();
                        }
                        _Connection.Close();
                    }
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
            var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CustomerGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun.DataSource = GubunSourceDataSource;
            cmb_Gubun.DisplayMember = "Name";
            cmb_Gubun.ValueMember = "Value";

            var SalesSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "SalesGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_SalesGubun.DataSource = SalesSourceDataSource;
            cmb_SalesGubun.DisplayMember = "Name";
            cmb_SalesGubun.ValueMember = "Value";


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
    }
}
