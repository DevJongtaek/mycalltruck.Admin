using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0203_CAROWNERMANAGE_SG : Form
    {
        bool Account_Result = false;
        DESCrypt m_crypt = null;
        string UP_Status = string.Empty;
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
        int GridIndex = 0;
        public FrmMN0203_CAROWNERMANAGE_SG()
        {
            m_crypt = new DESCrypt("12345678");
            foreach (var item in SingleDataSet.Instance.AddressReferences)
            {
                if (String.IsNullOrEmpty(item.State) || String.IsNullOrEmpty(item.City) || String.IsNullOrEmpty(item.Street))
                    continue;
                if (!AddressList.Any(c => c.State == item.State && c.City == item.City && c.Street == item.Street))
                {
                    AddressList.Add(new AddressViewModel
                    {
                        State = item.State,
                        City = item.City,
                        Street = item.Street,
                    });
                }
            }
            InitializeComponent();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_Inew.Enabled = true;
                cmb_ServiceState.Enabled = false;
            }
            else
            {
                btn_New.Enabled = false;
            }
        }

        private void FrmMN0203_CAROWNERMANAGE_Load(object sender, EventArgs e)
        {
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            tabControl1.SelectedIndex = 1;
            tabControl1.SelectedIndex = 0;
            _InitCmb();
            _InitCmbSearch();
            _InitGroup();

            btn_Search_Click(null, null);
        }

        private void _InitGroup()
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                            @"SELECT  DriverGroupId, ClientId, DriverId, Name, Cont, RequestFrom, RequestTo, SubClientId
                        FROM DriverGroups";
                        if (LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            _Command.CommandText += Environment.NewLine +
                                @"WHERE SubClientId = @SubClientId";
                            _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        }
                        else
                        {
                            _Command.CommandText += Environment.NewLine +
                                @"WHERE ClientId = @ClientId";
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        }
                    }
                    _Connection.Close();
                }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (driversBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null)
                return;
            err.Clear();
            DriverRepository mDriverRepository = new DriverRepository();
            if (mDriverRepository.IsMyCar(Selected.DriverId) && !mDriverRepository.IsAnotherCar(Selected.CarNo))
            {
                if (txt_Name.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_BizNo.Text.Length != 12 || txt_BizNo.Text.Contains(" "))
                {
                    MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                    err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_Birthday.Text.Length != 6 || txt_Birthday.Text.Contains(" "))
                {
                    MessageBox.Show("개인사업자는 생년월일을 법인사업자는 법인번호 앞 6자리를 입력해주세요.");
                    err.SetError(txt_Birthday, "법인번호(생년월일)가 완전하지 않습니다.");
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_CEO.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_Uptae.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_Upjong.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_MobileNo.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_Email.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Email, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (cmb_PayBankName.Text != cmb_PayBankName2.Text || txt_PayAccountNo.Text != txt_PayAccountNo2.Text || txt_PayInputName.Text != txt_PayInputName2.Text)
                {
                    string Parameter = "";
                    WebClient mWebClient = new WebClient();
                    string Mid = "edubill";
                    Parameter = String.Format(@"?BanKCode={0}&Account_No={1}&Account_Name={2}&BankName={3},&MID={4}", cmb_PayBankName.SelectedValue.ToString(), txt_PayAccountNo.Text, txt_PayInputName.Text, cmb_PayBankName.Text, Mid);
                    mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                    try
                    {
                        var _o = false;
                        var _Result = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCert" + Parameter));
                        //var _Result = mWebClient.DownloadString(new Uri("http://localhost/Pay/AccCert" + Parameter));
                        if (!bool.TryParse(_Result, out _o) || !_o)
                        {
                            throw new Exception("올바른 계좌가 아닙니다.");
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("계좌인증 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }

            UP_Status = "Update";
            int _rows = UpdateDB(UP_Status);
        }
        private void MWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
            }
            else if (e.Error != null)
            {
            }
            else
            {
                if (e.Result != null && e.Result.ToLower() == "true")
                {
                    Account_Result = true;

                    UP_Status = "Update";
                    int _rows = UpdateDB(UP_Status);
                }
                else
                {
                    Account_Result = false;


                    //Up_Status = "Update";
                    //int _rows = UpdateDB(Up_Status);


                    MessageBox.Show("계좌인증 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // _UpdateDB();
                }
            }
        }

        private int UpdateDB(string Status)
        {
            int _rows = 0;
            try
            {
                driversBindingSource.EndEdit();
                if (Status == "Update")
                {
                    var Row = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;

                    Row.BizNo = txt_BizNo.Text;
                    Row.MobileNo = txt_MobileNo.Text;
                    Row.PayBankName = cmb_PayBankName.Text;

                    if (txt_PayAccountNo.Text == "" && txt_PayAccountNo2.Text == "")
                    {

                    }
                    else
                    {
                        string tempString = string.Empty;
                        //DES 암호화
                        tempString = m_crypt.Encrypt(txt_PayAccountNo.Text);
                        Row.PayAccountNo = tempString;
                    }
                    DriverRepository mDriverRepository = new DriverRepository();
                    mDriverRepository.UpdateDriver(Row, Row.DriverId);
                    //driversTableAdapter.Update(Row);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "차량", 1), "차량정보 수정 성공");
                    // 조회
                    btn_Search_Click(null, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "차량정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }

        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Code_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txt_Code.Text);
            if (!ismatch)
            {
                MessageBox.Show("숫자만 입력해 주세요.");
            }



        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }
        private void _Search()
        {
            try
            {
                List<String> _WhereStringList = new List<string>();
                // 1. 본점/지사
                if (LocalUser.Instance.LogInInformation.IsSubClient)
                {
                    if (LocalUser.Instance.LogInInformation.IsAgent)
                    {
                        _WhereStringList.Add("DriverInstances.ClientUserId = " + LocalUser.Instance.LogInInformation.ClientUserId.ToString());
                    }
                    else
                    {
                        _WhereStringList.Add("DriverInstances.SubClientId = " + LocalUser.Instance.LogInInformation.SubClientId.ToString());
                    }
                }
                // 4. 단어 검색
                if (cmb_Search.SelectedIndex > 0)
                {
                    switch (cmb_Search.Text)
                    {
                        case "상호":
                            _WhereStringList.Add("Drivers.Name LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "사업자번호":
                            _WhereStringList.Add("REPLACE(Drivers.BizNo,N'-',N'') LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "아이디":
                            _WhereStringList.Add("Drivers.LoginId LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "대표자":
                            _WhereStringList.Add("Drivers.Ceo LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "차량번호":
                            _WhereStringList.Add("Drivers.CarNo LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "핸드폰번호":
                            _WhereStringList.Add("REPLACE(Drivers.MobileNo,N'-',N'') LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "기사명":
                            _WhereStringList.Add("Drivers.CarYear LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "그룹설정":
                            if (!LocalUser.Instance.LogInInformation.IsAdmin && (txt_Search.Text.ToUpper() == "A" || txt_Search.Text.ToUpper() == "B" || txt_Search.Text.ToUpper() == "C"))
                            {
                                _WhereStringList.Add("DriverInstances.GroupName = " + txt_Search.Text.ToUpper());
                            }
                            break;
                        default:
                            break;
                    }
                }
                DriverRepository mDriverRepository = new DriverRepository();
                mDriverRepository.Select(baseDataSet.Drivers, _WhereStringList);
            }
            catch
            {
                MessageBox.Show("조회가 실패하였습니다. 잠시 후에 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
        private void _InitCmbSearch()
        {
            DataGridViewColumn[] cols = new DataGridViewColumn[]{nameDataGridViewTextBoxColumn,bizNoDataGridViewTextBoxColumn,loginIdDataGridViewTextBoxColumn,cEODataGridViewTextBoxColumn,mobileNoDataGridViewTextBoxColumn,
             };
            cmb_Search.Items.Clear();
            DicSearch.Clear();
            cmb_Search.Items.Add("전체");

            foreach (var item in cols)
            {

                cmb_Search.Items.Add(item.HeaderText);
                if (item.DataPropertyName == null || item.DataPropertyName == "")
                {
                    DicSearch.Add(item.HeaderText, "'" + item.Name);
                }
                else
                {
                    DicSearch.Add(item.HeaderText, item.DataPropertyName);
                }
            }
            if (cmb_Search.Items.Count > 0)
                cmb_Search.SelectedIndex = 0;
        }

        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        List<BasicModel> _ClientDataSource = new List<BasicModel>();
        private void _InitCmb()
        {
            Dictionary<string, string> PayBank = new Dictionary<string, string>();
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

            cmb_PayBankName2.DataSource = new BindingSource(PayBank, null);
            cmb_PayBankName2.DisplayMember = "Value";
            cmb_PayBankName2.ValueMember = "Key";

            var ServiceStateDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_ServiceState.DataSource = ServiceStateDataSource;
            cmb_ServiceState.DisplayMember = "Name";
            cmb_ServiceState.ValueMember = "value";
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0203_CAROWNERMANAGE_FAULT_Add_SG _Form = new FrmMN0203_CAROWNERMANAGE_FAULT_Add_SG();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
            _InitGroup();
            btn_Search_Click(null, null);
            //if (LocalUser.Instance.LogInInformation.DriverType == 1)
            //{
            //    FrmMN0203_CAROWNERMANAGE_FAULT_Add _Form = new FrmMN0203_CAROWNERMANAGE_FAULT_Add();
            //    _Form.Owner = this;
            //    _Form.StartPosition = FormStartPosition.CenterParent;
            //    _Form.ShowDialog();
            //    _InitGroup();
            //    btn_Search_Click(null, null);
            //}
            //else
            //{
            //    FrmMN0203_CAROWNERMANAGE_Add _Form = new FrmMN0203_CAROWNERMANAGE_Add();
            //    _Form.Owner = this;
            //    _Form.StartPosition = FormStartPosition.CenterParent;
            //    _Form.ShowDialog();
            //    _InitGroup();
            //    btn_Search_Click(null, null);

            //}
        }

        private void cmb_CarState_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            if (driversBindingSource.Current != null)
            {
                var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
                if (Selected.RowState == DataRowState.Modified)
                    Selected.RejectChanges();
                if (MessageBox.Show("차량을 삭제 하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                DriverRepository mDriverRepository = new DriverRepository();
                mDriverRepository.DeleteDriver(Selected.DriverId);
                _Search();
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "차량", 1), "차량정보 삭제 성공");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        BaseDataSet.DriversRow PreviewItem = null;
        bool MethodChange = false;
        private void driversBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            tabControl1.SelectedTab = BasicPage;
            if (PreviewItem != null && PreviewItem.RowState == DataRowState.Modified)
            {
                PreviewItem.RejectChanges();
                PreviewItem = null;
            }
            if (driversBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected != null)
            {
                PreviewItem = Selected;
                MethodChange = true;
                // 화면구성
                if (String.IsNullOrEmpty(Selected.PayAccountNo))
                {
                    btn_Decrypt.Visible = false;
                }
                else
                {
                    btn_Decrypt.Visible = true;
                }
                txt_PayAccountNo.Text = "";
                txt_PayAccountNo2.Text = "";

                cmb_ServiceState.SelectedValue = Selected.ServiceState;
                txt_MobileNo.Text = Selected.MobileNo;
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (Selected.CandidateId != LocalUser.Instance.LogInInformation.ClientId)
                    {
                        txt_BizNo.ReadOnly = true;
                        txt_Name.ReadOnly = true;
                        txt_CEO.ReadOnly = true;
                        txt_Uptae.ReadOnly = true;
                        txt_Upjong.ReadOnly = true;
                        txt_Email.ReadOnly = true;
                        txt_Password.ReadOnly = true;
                        txt_MobileNo.ReadOnly = true;
                        txt_PhoneNo.ReadOnly = true;
                        txt_FaxNo.ReadOnly = true;
                        txt_Birthday.ReadOnly = true;
                        txt_Street.ReadOnly = true;
                        btnFindZip.Enabled = false;
                        cmb_PayBankName.Enabled = false;
                        txt_PayAccountNo.ReadOnly = true;
                        txt_PayInputName.ReadOnly = true;

                        pic_BizPaper.Enabled = false;
                        pic_CarPaper.Enabled = false;
                        btn_ImageView.Enabled = false;
                    }
                    else
                    {
                        txt_BizNo.ReadOnly = false;
                        txt_Name.ReadOnly = false;
                        txt_CEO.ReadOnly = false;
                        txt_Uptae.ReadOnly = false;
                        txt_Upjong.ReadOnly = false;
                        txt_Email.ReadOnly = false;
                        txt_Password.ReadOnly = false;
                        txt_MobileNo.ReadOnly = false;
                        txt_PhoneNo.ReadOnly = false;
                        txt_FaxNo.ReadOnly = false;
                        txt_Birthday.ReadOnly = false;
                        txt_Street.ReadOnly = false;
                        btnFindZip.Enabled = true;
                        cmb_PayBankName.Enabled = true;
                        txt_PayAccountNo.ReadOnly = false;
                        txt_PayInputName.ReadOnly = false;

                        cmb_PayBankName.Enabled = true;
                        txt_PayAccountNo.Enabled = true;
                        txt_PayInputName.Enabled = true;
                        txt_BizNo.ReadOnly = false;
                        btn_ImageView.Enabled = true;
                        pic_BizPaper.Enabled = true;
                        pic_CarPaper.Enabled = true;
                        if (Selected.ServiceState != 3)
                        {
                            cmb_PayBankName.Enabled = false;
                            txt_PayAccountNo.Enabled = false;
                            txt_PayInputName.Enabled = false;

                            txt_BizNo.ReadOnly = true;
                        }
                    }

                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        if (LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            if (LocalUser.Instance.LogInInformation.IsAgent)
                            {
                                if (Selected.ClientUserId != LocalUser.Instance.LogInInformation.ClientUserId)
                                {
                                    txt_MobileNo.Text = Selected.MobileNo.Substring(0, 4) + "****-****";
                                }
                            }
                            if (Selected.SubClientId != LocalUser.Instance.LogInInformation.SubClientId)
                            {
                                txt_MobileNo.Text = Selected.MobileNo.Substring(0, 4) + "****-****";
                            }
                        }
                        //if (Selected.CandidateId != LocalUser.Instance.LogInInformation.ClientId)
                        //{
                        //    txt_MobileNo.Text = Selected.MobileNo.Substring(0, 4) + "****-****";
                        //}
                    }
                }
                if (Selected.ServiceState == 1)
                {
                    txt_BizNo.ReadOnly = true;
                    txt_PayAccountNo.ReadOnly = true;
                    cmb_PayBankName.Enabled = false;
                    txt_PayInputName.ReadOnly = true;
                }
                MethodChange = false;
            }
            else
            {
                txt_MobileNo.Clear();
            }
        }

        private void cmb_AddressState_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            btn_Search_Click(null, null);
        }

        void pDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (pic_BizPaper.Image != null)
            {
                if (pic_BizPaper.Image.Width > pic_BizPaper.Image.Height)
                {
                    pic_BizPaper.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                e.Graphics.DrawImage(pic_BizPaper.Image, 0, 0, pic_BizPaper.Image.Width, pic_BizPaper.Image.Height);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null || !Selected.HasBizPaper)
                return;
            Bitmap _b = null;
            PrintDocument pDoc = new PrintDocument();
            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(10, 10, 10, 10);
            pDoc.DefaultPageSettings = ps;
            PrintPreviewDialog ppDoc = new PrintPreviewDialog();

            ppDoc.ClientSize = new System.Drawing.Size(500, 500);

            //   ppDoc.ClientSize = new System.Drawing.Size(pic_BizPaper.Image.Width, pic_BizPaper.Image.Height);
            ppDoc.UseAntiAlias = true;
            pDoc.PrintPage += new PrintPageEventHandler((_sender, _e) =>
            {
                if (_b.Width > _b.Height)
                {
                    _b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                var width = _b.Width;
                var height = _b.Height;
                if (_b.Width > _e.MarginBounds.Width)
                {
                    width = _e.MarginBounds.Width;
                    height = height * _e.MarginBounds.Width / _b.Width;
                }
                else if (_b.Height > _e.MarginBounds.Height)
                {
                    height = _e.MarginBounds.Height;
                    width = width * _e.MarginBounds.Height / _b.Height;
                }
                _e.Graphics.DrawImage(_b, 0, 0, width, height);

            });

            ppDoc.Document = pDoc;
            ((Form)ppDoc).WindowState = FormWindowState.Maximized;
            ((Form)ppDoc).TopMost = true;
            Task.Factory.StartNew(() =>
            {
                WebClient mWebClient = new WebClient();
                try
                {
                    var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetBizPaper?DriverId=" + Selected.DriverId.ToString());
                    //   var b = mWebClient.DownloadData("http://localhost/ImageFromAdmin/GetBizPaper?DriverId=" + Selected.DriverId.ToString());
                    MemoryStream ms = new MemoryStream();
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    Invoke(new Action(() => MessageBox.Show("이미지를 가져오는 중 오류가 발생하였습니다. 잠시 후 다시 이용바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                    return;
                }
                Invoke(new Action(() => ppDoc.Show()));
                //  ppDoc.Show();


            });
        }

        void pDoc_PrintPage2(object sender, PrintPageEventArgs e)
        {
            if (pic_CarPaper.Image != null)
            {
                if (pic_CarPaper.Image.Width > pic_CarPaper.Image.Height)
                {
                    pic_CarPaper.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }

                e.Graphics.DrawImage(pic_CarPaper.Image, 0, 0, pic_CarPaper.Image.Width, pic_CarPaper.Image.Height);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null || !Selected.HasCarPaper)
                return;
            Bitmap _b = null;
            PrintDocument pDoc = new PrintDocument();
            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(10, 10, 10, 10);
            pDoc.DefaultPageSettings = ps;
            PrintPreviewDialog ppDoc = new PrintPreviewDialog();
            ppDoc.ClientSize = new System.Drawing.Size(500, 500);

            //   ppDoc.ClientSize = new System.Drawing.Size(pic_BizPaper.Image.Width, pic_BizPaper.Image.Height);
            ppDoc.UseAntiAlias = true;
            pDoc.PrintPage += new PrintPageEventHandler((_sender, _e) =>
            {
                if (_b.Width > _b.Height)
                {
                    _b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                var width = _b.Width;
                var height = _b.Height;
                if (_b.Width > _e.MarginBounds.Width)
                {
                    width = _e.MarginBounds.Width;
                    height = height * _e.MarginBounds.Width / _b.Width;
                }
                else if (_b.Height > _e.MarginBounds.Height)
                {
                    height = _e.MarginBounds.Height;
                    width = width * _e.MarginBounds.Height / _b.Height;
                }
                _e.Graphics.DrawImage(_b, 0, 0, width, height);

            });
            ppDoc.Document = pDoc;
            ((Form)ppDoc).WindowState = FormWindowState.Maximized;
            Task.Factory.StartNew(() =>
            {
                WebClient mWebClient = new WebClient();
                try
                {
                    var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarPaper?DriverId=" + Selected.DriverId.ToString());
                    // var b = mWebClient.DownloadData("http://localhost/ImageFromAdmin/GetCarPaper?DriverId=" + Selected.DriverId.ToString());
                    MemoryStream ms = new MemoryStream();
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    Invoke(new Action(() => MessageBox.Show("이미지를 가져오는 중 오류가 발생하였습니다. 잠시 후 다시 이용바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                    return;
                }
                Invoke(new Action(() => ppDoc.Show()));
            });
        }

        private void btn_Group_Click(object sender, EventArgs e)
        {
            FrmMN0203_CAROWNERMANAGE_Add2 _Form = new FrmMN0203_CAROWNERMANAGE_Add2();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
            _InitGroup();
            btn_Search_Click(null, null);
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as BaseDataSet.DriversRow;
            if (Selected == null)
                return;
            if (e.ColumnIndex == 1)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == bizNoDataGridViewTextBoxColumn)
            {
                if (Selected.BizNo.Length == 10)
                    e.Value = Selected.BizNo.Substring(0, 3) + "-" + Selected.BizNo.Substring(3, 2) + "-" + Selected.BizNo.Substring(5, 5);
                else
                    e.Value = Selected.BizNo;
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == mobileNoDataGridViewTextBoxColumn)
            {
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            if (Selected.ClientUserId != LocalUser.Instance.LogInInformation.ClientUserId)
                            {
                                e.Value = Selected.MobileNo.Substring(0, 4) + "****-****";
                                return;
                            }
                        }
                        if (Selected.SubClientId != LocalUser.Instance.LogInInformation.SubClientId)
                        {
                            e.Value = Selected.MobileNo.Substring(0, 4) + "****-****";
                            return;
                        }
                    }
                    //if (Selected.CandidateId != LocalUser.Instance.LogInInformation.ClientId)
                    //{
                    //    e.Value = Selected.MobileNo.Substring(0, 4) + "****-****";
                    //    return;
                    //}
                }
                e.Value = Selected.MobileNo;
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == ServiceState)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState" && c.Value == Selected.ServiceState).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == FileBiz)
            {
                if (Selected.HasCarPaper && Selected.HasBizPaper)
                {
                    e.Value = "있음";
                }
                else
                {
                    e.Value = "없음";
                }
            }
            else if(dataGridView1.Columns[e.ColumnIndex] == AddressState)
            {
                e.Value = Selected.AddressState + " " + Selected.AddressCity + " " + Selected.AddressDetail;
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void txt_ClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
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

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            int _DriverType = 1;
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
                ExcelExportBasic();
            }
            else
            {
                ExcelExport();
            }
        }

        private void ExcelExport()
        {
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {

                di.Create();
            }
            var fileString = "차량관리입력양식(표준)_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.차량관리입력양식_표준__Blank))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 3;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                var _Model = ((DataRowView)dataGridView1.Rows[i].DataBoundItem).Row as BaseDataSet.DriversRow;
                var _MobleNo = _Model.MobileNo;
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            if (_Model.ClientUserId != LocalUser.Instance.LogInInformation.ClientUserId)
                            {
                                _MobleNo = _Model.MobileNo.Substring(0, 4) + "****-****";
                            }
                        }
                        if (_Model.SubClientId != LocalUser.Instance.LogInInformation.SubClientId)
                        {
                            _MobleNo = _Model.MobileNo.Substring(0, 4) + "****-****";
                        }
                    }
                    //if (_Model.CandidateId != LocalUser.Instance.LogInInformation.ClientId)
                    //{
                    //    _MobleNo = _Model.MobileNo.Substring(0, 4) + "****-****";
                    //}
                }

                _Sheet.Cells[RowIndex, 1].Value = (i + 1).ToString();
                _Sheet.Cells[RowIndex, 2].Value = _Model.BizNo;
                _Sheet.Cells[RowIndex, 3].Value = _Model.Name;
                _Sheet.Cells[RowIndex, 4].Value = _Model.Uptae;
                _Sheet.Cells[RowIndex, 5].Value = _Model.Upjong;
                _Sheet.Cells[RowIndex, 6].Value = _Model.CEO;
                _Sheet.Cells[RowIndex, 7].Value = "";
                _Sheet.Cells[RowIndex, 8].Value = _MobleNo;
                _Sheet.Cells[RowIndex, 9].Value = _Model.PhoneNo;
                _Sheet.Cells[RowIndex, 10].Value = _Model.FaxNo;
                _Sheet.Cells[RowIndex, 11].Value = _Model.Email;
                _Sheet.Cells[RowIndex, 12].Value = _Model.AddressState;
                _Sheet.Cells[RowIndex, 13].Value = _Model.AddressCity;
                _Sheet.Cells[RowIndex, 14].Value = _Model.AddressDetail;
                _Sheet.Cells[RowIndex, 15].Value = "";
                _Sheet.Cells[RowIndex, 16].Value = _Model.RouteType;
                _Sheet.Cells[RowIndex, 17].Value = _Model.InsuranceType;
                _Sheet.Cells[RowIndex, 18].Value = _Model.CarNo;
                _Sheet.Cells[RowIndex, 19].Value = _Model.CarType;
                _Sheet.Cells[RowIndex, 20].Value = _Model.CarSize;
                _Sheet.Cells[RowIndex, 21].Value = _Model.CarGubun;
                _Sheet.Cells[RowIndex, 22].Value = _Model.CarYear;
                _Sheet.Cells[RowIndex, 23].Value = _Model.PayBankName;
                _Sheet.Cells[RowIndex, 24].Value = _Model.PayAccountNo;
                _Sheet.Cells[RowIndex, 25].Value = _Model.PayInputName;
                _Sheet.Cells[RowIndex, 26].Value = _Model.ParkState;
                _Sheet.Cells[RowIndex, 27].Value = _Model.ParkCity;
                _Sheet.Cells[RowIndex, 28].Value = _Model.ParkStreet;
                _Sheet.Cells[RowIndex, 29].Value = _Model.FpisCarType;
                RowIndex++;
            }
            try
            {
                _Excel.SaveAs(file);
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            System.Diagnostics.Process.Start(FileName);

        }
        private List<ExcelModel> _GetDataForExcelExport(SqlDataReader _Reader)
        {
            List<ExcelModel> r = new List<ExcelModel>();
            while (_Reader.Read())
            {
                //SingleDataSet.Instance.StaticOptions
                r.Add(new ExcelModel
                {
                    SBiz_NO = _Reader.GetStringN(0),
                    SName = _Reader.GetStringN(1),
                    SUptae = _Reader.GetStringN(2),
                    SUpjong = _Reader.GetStringN(3),
                    SCeo = _Reader.GetStringN(4),
                    SCeoBirth = _Reader.GetStringN(5),
                    SMobileNo = _Reader.GetStringN(6),
                    SPhoneNo = _Reader.GetStringN(7),
                    SFaxNo = _Reader.GetStringN(8),
                    SEmail = _Reader.GetStringN(9),
                    SState = _Reader.GetStringN(10),
                    SCity = _Reader.GetStringN(11),
                    SStreet = _Reader.GetStringN(12),
                    SBizGubun = GetTextFromStaticOption("BizType", _Reader.GetInt32Z(13)),
                    SRouteType = GetTextFromStaticOption("RouteType", _Reader.GetInt32Z(14)),
                    SInsurance = GetTextFromStaticOption("InsuranceType", _Reader.GetInt32Z(15)),
                    SCarNo = _Reader.GetStringN(16),
                    SCarType = GetTextFromStaticOption("CarType", _Reader.GetInt32Z(17)),
                    SCarSize = GetTextFromStaticOption("CarSize", _Reader.GetInt32Z(18)),
                    SCarGubun = GetTextFromStaticOption("CarGubun", _Reader.GetInt32Z(19)),
                    SCarYear = _Reader.GetStringN(20),
                    SPayBankName = _Reader.GetStringN(21),
                    SPayAccountNo = _Reader.GetStringN(22),
                    SInputName = _Reader.GetStringN(23),
                    SCarstate = _Reader.GetStringN(24),
                    SCarcity = _Reader.GetStringN(25),
                    SCarStreet = _Reader.GetStringN(26),
                    SfpisCartype = GetTextFromStaticOption("FPisCarType", _Reader.GetInt32Z(27)),
                });
            }
            return r;
        }

        private String GetTextFromStaticOption(String Div, int Value)
        {
            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == Div && c.Value == Value))
            {
                return SingleDataSet.Instance.StaticOptions.First(c => c.Div == Div && c.Value == Value).Name;
            }
            else
            {
                return "";
            }
        }

        private void ExcelExportBasic()
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = "차량관리입력양식(기본)_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.차량관리입력양식_기본__Blank))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 3;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                var _Model = ((DataRowView)dataGridView1.Rows[i].DataBoundItem).Row as BaseDataSet.DriversRow;
                var _MobleNo = _Model.MobileNo;
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            if (_Model.ClientUserId != LocalUser.Instance.LogInInformation.ClientUserId)
                            {
                                _MobleNo = _Model.MobileNo.Substring(0, 4) + "****-****";
                            }
                        }
                        if (_Model.SubClientId != LocalUser.Instance.LogInInformation.SubClientId)
                        {
                            _MobleNo = _Model.MobileNo.Substring(0, 4) + "****-****";
                        }
                    }
                    //if (_Model.CandidateId != LocalUser.Instance.LogInInformation.ClientId)
                    //{
                    //    _MobleNo = _Model.MobileNo.Substring(0, 4) + "****-****";
                    //}
                }
                _Sheet.Cells[RowIndex, 1].Value = (i + 1).ToString();
                _Sheet.Cells[RowIndex, 2].Value = _Model.BizNo.Replace("-", "");
                _Sheet.Cells[RowIndex, 3].Value = _Model.Name;
                _Sheet.Cells[RowIndex, 4].Value = _Model.Uptae;
                _Sheet.Cells[RowIndex, 5].Value = _Model.Upjong;
                _Sheet.Cells[RowIndex, 6].Value = _Model.Zipcode;
                _Sheet.Cells[RowIndex, 7].Value = _Model.AddressState;
                _Sheet.Cells[RowIndex, 8].Value = _Model.AddressCity;
                _Sheet.Cells[RowIndex, 9].Value = _Model.AddressDetail;
                _Sheet.Cells[RowIndex, 10].Value = _Model.CEO;
                _Sheet.Cells[RowIndex, 11].Value = _MobleNo.Replace("-", "");
                _Sheet.Cells[RowIndex, 12].Value = _Model.CarNo;
                _Sheet.Cells[RowIndex, 13].Value = _Model.CarType;
                _Sheet.Cells[RowIndex, 14].Value = _Model.CarSize;
                _Sheet.Cells[RowIndex, 15].Value = _Model.PayBankName;
                _Sheet.Cells[RowIndex, 16].Value = m_crypt.Decrypt(_Model.PayAccountNo);
                _Sheet.Cells[RowIndex, 17].Value = _Model.PayInputName;
                _Sheet.Cells[RowIndex, 18].Value = _Model.VAccount;
                RowIndex++;
            }

            try
            {
                _Excel.SaveAs(file);
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }
            System.Diagnostics.Process.Start(FileName);
        }

        private List<ExcelBasicModel> _GetDataForExcelExportBasic(SqlDataReader _Reader)
        {
            List<ExcelBasicModel> r = new List<ExcelBasicModel>();
            while (_Reader.Read())
            {
                //SingleDataSet.Instance.StaticOptions
                r.Add(new ExcelBasicModel
                {
                    SBiz_NO = _Reader.GetStringN(0),
                    SName = _Reader.GetStringN(1),
                    SUptae = _Reader.GetStringN(2),
                    SUpjong = _Reader.GetStringN(3),
                    SState = _Reader.GetStringN(4),
                    SCity = _Reader.GetStringN(5),
                    SStreet = _Reader.GetStringN(6),
                    SZip = _Reader.GetStringN(7),
                    SCeo = _Reader.GetStringN(8),
                    SMobileNo = _Reader.GetStringN(9),
                    SCarNo = _Reader.GetStringN(10),
                    SCarType = _Reader.GetInt32Z(11).ToString(),
                    SCarSize = _Reader.GetInt32Z(12).ToString(),
                    SPayBankName = _Reader.GetStringN(13),
                    SPayAccountNo = _Reader.GetStringN(14),
                    SInputName = _Reader.GetStringN(15),
                });
            }
            return r;
        }

        bool overhead = false;
        List<string> checkedCodes = new List<string>();


        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            overhead = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1[checkbox.Index, i].Value = chkAllSelect.Checked;
            }
            overhead = false;
            dataGridView1_CellValueChanged(null, null);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e == null)
                    return;
                if (e.ColumnIndex == checkbox.Index)
                {

                    object o = dataGridView1[e.ColumnIndex, e.RowIndex].Value;

                    //  var IssueDate1 = ((DataRowView)driversBindingSource[e.RowIndex])["IssueDate1"].ToString();
                    string code = ((DataRowView)driversBindingSource[e.RowIndex])["DriverId"].ToString();
                    if (Convert.ToBoolean(o))
                    {
                        if (!checkedCodes.Contains(code))
                            checkedCodes.Add(code);
                    }
                    else
                    {
                        if (checkedCodes.Contains(code))
                            checkedCodes.Remove(code);
                    }
                }
            }
            catch { }
            if (overhead) return;

        }


        private string getFilterString()
        {
            string r = "'0'";
            if (checkedCodes.Count > 0)
            {
                r = String.Join(",", checkedCodes.Select(c => "'" + c + "'").ToArray());
            }
            return r;
        }
        private void btn_SMS_Click(object sender, EventArgs e)
        {
            if (LocalUser.Instance.LogInInformation.IsAdmin)
                return;
            bool AllowSMS = false;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT AllowSMS FROM Clients WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var o = _Command.ExecuteScalar();
                    if (o != null && (bool)o == true)
                        AllowSMS = true;
                }
                _Connection.Close();
            }
            if (!AllowSMS)
            {
                MessageBox.Show("SMS 전송 서비스를 신청하지 않았습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string SMSText;
            getFilterString();
            string errormessage = string.Empty;
            if (checkedCodes.Count() == 0)
            {
                MessageBox.Show("적용할 건을 선택하십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrmSMS _FrmSMS = new FrmSMS(checkedCodes.Count());
            _FrmSMS.Owner = this;
            _FrmSMS.StartPosition = FormStartPosition.CenterParent;
            if (_FrmSMS.ShowDialog() == DialogResult.OK)
            {
                if (_FrmSMS.txt_SMS.Text == "") return;

                _FrmSMS.Close();
                SMSText = _FrmSMS.txt_SMS.Text;

                bar.Value = 0;
                bar.Maximum = checkedCodes.Count();
                bar.Visible = true;
                pnProgress.Visible = true;
                //   string title = string.Format("세금계산서 ({0}-{1})", dtp_Sdate.Text.Replace("/", "-"), dtp_Edate.Text.Replace("/", "-"));
                List<BaseDataSet.DriversRow> drivers = new List<BaseDataSet.DriversRow>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[checkbox.Index].Value is bool && (bool)row.Cells[checkbox.Index].Value == true)
                    {
                        drivers.Add(((DataRowView)row.DataBoundItem).Row as BaseDataSet.DriversRow);
                    }
                }
                if (drivers.Count() == 0)
                {
                    MessageBox.Show("SMS전송할 건을 선택하십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bar.Visible = false;
                    pnProgress.Visible = false;
                    return;
                }
                Thread t = new Thread(new ThreadStart(() =>
                {
                    foreach (var item in drivers)
                    {
                        if (item.MobileNo.Replace("-", "").Length >= 10)
                        {
                            string Parameter = "";
                            WebClient mWebClient = new WebClient();
                            Parameter = String.Format(@"?SessionId={0}&PhoneNo={1}&Message={2}", LocalUser.Instance.LogInInformation.SessionId, item.MobileNo.Replace("-", ""), SMSText);
                            try
                            {
                                var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/SMS/Send" + Parameter));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    MessageBox.Show("SMS 전송 '" + drivers.Count() + "' 건  성공하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                    pnProgress.Invoke(new Action(() => btn_Search_Click(null, null)));
                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            else
            {
                btn_Search_Click(null, null);
            }
            checkedCodes.Clear();
        }

        private void cmb_ClientSerach_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == ImagePage)
            {
                pic_BizPaper.Image = Properties.Resources.ic_photo_white_48dp_2x;
                pic_CarPaper.Image = Properties.Resources.ic_photo_white_48dp_2x;
                pic_BizPaper.Cursor = Cursors.Arrow;
                pic_CarPaper.Cursor = Cursors.Arrow;
                if (driversBindingSource.Current == null)
                    return;
                var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
                if (Selected == null)
                    return;
                if (Selected.HasBizPaper)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetBizPaper?DriverId=" + Selected.DriverId.ToString());
                        MemoryStream ms = new MemoryStream();
                        ms.Write(b, 0, b.Length);
                        ms.Position = 0;
                        pic_BizPaper.Image = new Bitmap(ms);
                    }
                    pic_BizPaper.Cursor = Cursors.Hand;
                }
                if (Selected.HasCarPaper)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarPaper?DriverId=" + Selected.DriverId.ToString());
                        MemoryStream ms = new MemoryStream();
                        ms.Write(b, 0, b.Length);
                        ms.Position = 0;
                        pic_CarPaper.Image = new Bitmap(ms);
                    }
                    pic_CarPaper.Cursor = Cursors.Hand;
                }
            }
        }


        string fileString = string.Empty;
        string title = string.Empty;
        string FolderPath = string.Empty;

        private void btnExcelExport2_Click(object sender, EventArgs e)
        {
            EXCELINSERT_DRIVER2 _Form = new EXCELINSERT_DRIVER2();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
        }

        private void btnExcelIMportLG_Click(object sender, EventArgs e)
        {

        }

        private void cmb_ServiceState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (driversBindingSource.Current != null)
            {
                var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            }
        }

        private void btn_Decrypt_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            string tempString = string.Empty;
            tempString = Selected.PayAccountNo;
            try
            {
                txt_PayAccountNo.Text = m_crypt.Decrypt(tempString);
                txt_PayAccountNo2.Text = m_crypt.Decrypt(tempString);
            }
            catch
            {
                txt_PayAccountNo.Text = tempString;
                txt_PayAccountNo2.Text = tempString;
            }

        }

        class ExcelBasicModel : INotifyPropertyChanged
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
            private String _SPayBankName = "";
            private String _SPayAccountNo = "";
            private String _SInputName = "";
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

        class ExcelModel : INotifyPropertyChanged
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

        private void btnFindZip_Click(object sender, EventArgs e)
        {
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

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void btn_ImageView_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null || !Selected.HasBizPaper || !Selected.HasCarPaper)
                return;
            FormImages2 f = new FormImages2(Selected);
            f.Show();
        }

        private void btn_ServiceState_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Drivers SET ServiceState = 7 , RequestDate = GetDate()  WHERE DriverId = @DriverId";
                cmd.Parameters.AddWithValue("@DriverId", DirverNoId.Value);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            _Search();
        }

        private void txt_Birthday_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_BizNo_Enter(object sender, EventArgs e)
        {
            if (txt_BizNo.ReadOnly)
                return;
            txt_BizNo.Text = txt_BizNo.Text.Replace("-", "");
        }

        private void txt_BizNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == '\u0003'))
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
        }

        private void txt_CarInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void btn_BizPaperAdd_Click(object sender, EventArgs e)
        {
            BaseDataSet.DriversRow DriverId = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (DriverId == null)
                return;

            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;

            Bitmap _b = null;
            try
            {
                var b = File.ReadAllBytes(dlgOpen.FileName);
                MemoryStream ms = new MemoryStream();
                ms.Write(b, 0, b.Length);
                ms.Position = 0;
                _b = new Bitmap(ms);
            }
            catch (Exception)
            {
                MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                MemoryStream mm = new MemoryStream();
                _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bytes = new byte[mm.Length];
                mm.Seek(0, SeekOrigin.Begin);
                mm.Read(bytes, 0, bytes.Length);
                mm.Close();

                AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                i.DriverId = DriverId.DriverId;
                i.ImageData64String = System.Convert.ToBase64String(bytes);

                var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetBizPaper"));
                // var http = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/ImageFromApp/SetBizPaper"));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";

                string parsedContent = JsonConvert.SerializeObject(i);
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] b = encoding.GetBytes(parsedContent);

                Stream newStream = http.GetRequestStream();
                newStream.Write(b, 0, b.Length);
                newStream.Close();

                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
            }
            catch (Exception)
            {
                MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            pic_BizPaper.Image = _b;
            DriverId.HasBizPaper = true;
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.UpdateDriverSetBizPaper(DriverId.DriverId);
            DriverId.AcceptChanges();
            MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_BizPaperDelete_Click(object sender, EventArgs e)
        {
            BaseDataSet.DriversRow DriverId = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (DriverId == null)
                return;
            if (MessageBox.Show("정말 이미지를 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                string sqlString = "UPDATE Drivers Set HasBizPaper = 0 WHERE DriverId = @DriverId";
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand updateCmd = new SqlCommand(sqlString, cn);
                    updateCmd.Parameters.Add(new SqlParameter("@DriverId", DriverId.DriverId));
                    cn.Open();
                    updateCmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("이미지를 삭제 하는 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            pic_BizPaper.Image = Properties.Resources.ic_photo_white_48dp_2x;
            DriverId.HasBizPaper = false;
            DriverId.AcceptChanges();
        }


        private void btn_CarPaperAdd_Click(object sender, EventArgs e)
        {
            BaseDataSet.DriversRow DriverId = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (DriverId == null)
                return;

            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;

            Bitmap _b = null;
            try
            {
                var b = File.ReadAllBytes(dlgOpen.FileName);
                MemoryStream ms = new MemoryStream();
                ms.Write(b, 0, b.Length);
                ms.Position = 0;
                _b = new Bitmap(ms);
            }
            catch (Exception)
            {
                MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                MemoryStream mm = new MemoryStream();
                _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bytes = new byte[mm.Length];
                mm.Seek(0, SeekOrigin.Begin);
                mm.Read(bytes, 0, bytes.Length);
                mm.Close();

                AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                i.DriverId = DriverId.DriverId;
                i.ImageData64String = System.Convert.ToBase64String(bytes);

                var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetCarPaper"));
                //    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/ImageFromApp/SetCarPaper"));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";

                string parsedContent = JsonConvert.SerializeObject(i);
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] b = encoding.GetBytes(parsedContent);

                Stream newStream = http.GetRequestStream();
                newStream.Write(b, 0, b.Length);
                newStream.Close();

                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
            }
            catch (Exception)
            {
                MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            pic_CarPaper.Image = _b;
            DriverId.HasCarPaper = true;
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.UpdateDriverSetCarPaper(DriverId.DriverId);
            DriverId.HasCarPaper = true;
            DriverId.AcceptChanges();
            MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_CarPaperDelete_Click(object sender, EventArgs e)
        {
            BaseDataSet.DriversRow DriverId = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (DriverId == null)
                return;
            if (MessageBox.Show("정말 이미지를 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                string sqlString = "UPDATE Drivers Set HasCarPaper = 0 WHERE DriverId = @DriverId";
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand updateCmd = new SqlCommand(sqlString, cn);
                    updateCmd.Parameters.Add(new SqlParameter("@DriverId", DriverId.DriverId));
                    cn.Open();
                    updateCmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("이미지를 삭제 하는 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            pic_CarPaper.Image = Properties.Resources.ic_photo_white_48dp_2x;
            DriverId.HasCarPaper = false;
            DriverId.AcceptChanges();
        }
    }

}
