using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.XML;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0203_CAROWNERMANAGE : Form
    {
        bool Account_Result = false;
        DESCrypt m_crypt = null;
        string UP_Status = string.Empty;
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
        int GridIndex = 0;
        string _DcarNo = "";

        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
            auth = this.GetAuth();
            switch (auth)
            {
                case MenuAuth.None:
                    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                    return;
                case MenuAuth.Read:
                    //btnAdd.Enabled = false;
                    //btnImport.Enabled = false;
                    //btnAppend.Enabled = false;
                    //btnRequest.Enabled = false;
                    //btnCurrentDelete.Enabled = false;
                    btnExcelExport.Enabled = false;
                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                    btn_SMS.Enabled = false;



                    grid1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }
        public FrmMN0203_CAROWNERMANAGE()
        {
           // _DcarNo = DCarNo;
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
                ApplyAuth();
            }
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                grid1.Columns["ClientCode"].Visible = false;
                grid1.Columns["ClientName"].Visible = false;



                //grid1.Columns[2].Visible = false;
                //grid1.Columns[3].Visible = false;
                cmb_ClientSerach.Visible = false;
                txt_ClientSearch.Visible = false;
                cmb_AdminI.Visible = false;
                btn_Inew.Enabled = true;
                lblClient.Visible = false;
                cmb_CandidateGubun.Visible = false;
                cmb_UsePayNow.Enabled = false;
                btn_ServiceState.Visible = false;
                cmb_ServiceState.Enabled = false;
            }
            else
            {
                btn_New.Enabled = false;
                cmb_Group.Enabled = false;
                lblClient.Visible = true;
                cmb_CandidateGubun.Visible = true;
                cmb_UsePayNow.Enabled = true;
                btn_ServiceState.Visible = true;
            }

            

        }

        public FrmMN0203_CAROWNERMANAGE(String DCarNo = "")
        {
            _DcarNo = DCarNo;
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
                grid1.Columns["ClientCode"].Visible = false;
                grid1.Columns["ClientName"].Visible = false;

                //grid1.Columns[2].Visible = false;
                //grid1.Columns[3].Visible = false;
                cmb_ClientSerach.Visible = false;
                txt_ClientSearch.Visible = false;
                cmb_AdminI.Visible = false;
                btn_Inew.Enabled = true;
                lblClient.Visible = false;
                cmb_CandidateGubun.Visible = false;
                cmb_UsePayNow.Enabled = false;
                btn_ServiceState.Visible = false;
                cmb_ServiceState.Enabled = false;
            }
            else
            {
                btn_New.Enabled = false;
                cmb_Group.Enabled = false;
                lblClient.Visible = true;
                cmb_CandidateGubun.Visible = true;
                cmb_UsePayNow.Enabled = true;
                btn_ServiceState.Visible = true;
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
            InitAccLogTable();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                LocalUser.Instance.LogInInformation.LoadClient();
                if (LocalUser.Instance.LogInInformation.Client.HasPoint)
                {
                    lbl_VAccount.Visible = true;
                    txt_VAcount.Visible = true;
                }
            }
            if(_DcarNo != "")
            {
                cmb_Search.Text = "차량번호";
                txt_Search.Text = _DcarNo; 
            }
           
            _Search();
            //btn_Search_Click(null, null);

        }
        private List<AccLogsViewModel> _AccLogTable = new List<AccLogsViewModel>();
        class AccLogsViewModel
        {
            public int AccLogId { get; set; }
            public string AccFunction { get; set; }
            public string BankName { get; set; }
            public string CardNo { get; set; }
            public int TradeId { get; set; }
            public DateTime CreateDate { get; set; }
            public string LGD_TID { get; set; }
            public string LGD_MID { get; set; }
            public string LGD_OID { get; set; }
            public decimal LGD_AMOUNT { get; set; }
            public string LGD_RESPCODE { get; set; }
            public string LGD_RESPMSG { get; set; }
            





        }


        private void InitAccLogTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _AccLogTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT AccLogId, AccFunction, BankName, CardNo, TradeId, CreateDate, LGD_TID, LGD_MID, LGD_OID, LGD_AMOUNT, LGD_RESPCODE, LGD_RESPMSG  FROM AccLogs ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _AccLogTable.Add(
                              new AccLogsViewModel
                              {
                                  AccLogId = dataReader.GetInt32(0),
                                  AccFunction = dataReader.GetString(1),
                                  BankName = dataReader.GetString(2),
                                  CardNo = dataReader.GetString(3),
                                  TradeId = dataReader.GetInt32(4),
                                  CreateDate = dataReader.GetDateTime(5),
                                  LGD_TID = dataReader.GetString(6),
                                  LGD_MID = dataReader.GetString(7),
                                  LGD_OID = dataReader.GetString(8),
                                  LGD_AMOUNT = dataReader.GetDecimal(9),
                                  LGD_RESPCODE = dataReader.GetString(10),
                                  LGD_RESPMSG = dataReader.GetString(11),
                                 
                                 
                              });
                        }
                    }
                }
                connection.Close();
            }
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

                        _Command.CommandText += Environment.NewLine +
                                @"WHERE ClientId = @ClientId";
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        //if (LocalUser.Instance.LogInInformation.IsSubClient)
                        //{
                        //    _Command.CommandText += Environment.NewLine +
                        //        @"WHERE SubClientId = @SubClientId";
                        //    _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        //}
                        //else
                        //{
                        //    _Command.CommandText += Environment.NewLine +
                        //        @"WHERE ClientId = @ClientId";
                        //    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        //}
                    }
                    _Connection.Close();
                }
            }
        }

        string Account_Name = "";

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (driversBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null)
                return;
            err.Clear();
            DriverRepository mDriverRepository = new DriverRepository();
            //if (mDriverRepository.IsMyCar(Selected.DriverId) && !mDriverRepository.IsAnotherCar(Selected.CarNo))
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
                    MessageBox.Show("대표자 생년월일 6자리를 입력해주세요.");
                    err.SetError(txt_Birthday, "대표자 생년월일이 완전하지 않습니다.");
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
                if (txt_MobileNo.Text.Length < 10 || txt_MobileNo.Text.Contains(" "))
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                else
                {
                    if (!Regex.Match(txt_MobileNo.Text, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}").Success)
                    {
                        MessageBox.Show("핸드폰번호를 올바르게 입력해주세요.");
                        //  err.SetError(txt_MobileNo, "차량번호가 중복되었습니다.!!");
                        var Error = "핸드폰번호를 입력해주세요.";
                        err.SetError(txt_MobileNo, Error);
                        tabControl1.SelectTab(0);
                        return;
                    }

                    //MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    //err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    //tabControl1.SelectTab(0);
                    //return;


                }
                if (txt_LoignId.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_LoignId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_Password.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(0);
                    return;
                }
                if (txt_CarNo.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CarNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(1);
                    return;
                }
                else
                {
                    var Query1 =
                         "Select Count(*) From Drivers Where CarNo = @CarNo AND DriverId != @DriverId AND ServiceState <> 5";
                    bool IsDuplicated = false;
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();

                        SqlCommand cmd1 = cn.CreateCommand();
                        cmd1.CommandText = Query1;
                        cmd1.Parameters.AddWithValue("@CarNo", txt_CarNo.Text.Trim().Replace(" ", ""));
                        cmd1.Parameters.AddWithValue("@DriverId", DirverNoId.Value);
                        //  cmd1.Parameters.AddWithValue("@CandidateId", LocalUser.Instance.LogInInfomation.ClientId);
                        if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                        {
                            IsDuplicated = true;
                        }
                        //if (!IsDuplicated)
                        //{
                        //    SqlCommand cmd2 = cn.CreateCommand();
                        //    cmd2.CommandText = Query2;
                        //    cmd2.Parameters.AddWithValue("@LoginId", txt_CarNo.Text);
                        //    if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
                        //    {
                        //        IsDuplicated = true;
                        //    }
                        //}
                        cn.Close();
                    }
                    if (IsDuplicated)
                    {
                        MessageBox.Show("차량번호가 중복되었습니다.!!. 다른차량번호를 입력해주십시오.");
                        err.SetError(txt_Code, "차량번호가 중복되었습니다.!!");
                        tabControl1.SelectTab(1);
                        return;
                    }





                }
                if (cmb_CarType.SelectedIndex == 0)
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(cmb_CarType, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(1);
                    return;
                }
                if (cmb_CarSize.SelectedIndex == 0)
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(cmb_CarSize, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    tabControl1.SelectTab(1);
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
                       // var _Result = mWebClient.DownloadString(new Uri("http://localhost/Pay/AccCert" + Parameter));

                        //_Result = HttpUtility.UrlDecode(_Result);
                        //if (_Result == "오류")
                        //{
                        //    throw new Exception("올바른 계좌가 아닙니다.");
                        //}
                        //else
                        //{
                        //    Account_Name = _Result;
                        //}

                        if (!bool.TryParse(_Result, out _o) || !_o)
                        {
                            var _Result2 = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/AccCertFirst" + Parameter));
                            _Result2 = HttpUtility.UrlDecode(_Result2);
                            if (_Result2 == "오류")
                            {
                                throw new Exception("올바른 계좌가 아닙니다.");
                            }
                            else
                            {
                                if (MessageBox.Show($"{cmb_PayBankName.Text} / {txt_PayAccountNo.Text} / {_Result2}\r\n위 계좌가 맞습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                    return;
                               
                                Account_Name = _Result2;
                            }
                        }
                        else
                        {
                            Account_Name = txt_PayInputName.Text;
                        }

                        //if (!bool.TryParse(_Result, out _o) || !_o)
                        //{
                        //    throw new Exception("올바른 계좌가 아닙니다.");
                        //}
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("계좌정보를 다시 확인해 주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    Account_Name = txt_PayInputName2.Text;
                }
            }

            UP_Status = "Update";
            int _rows = UpdateDB(UP_Status, Account_Name);
        }
        private void MWebClient_DownloadStringCompleted2(object sender, DownloadStringCompletedEventArgs e)
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
                    //  Account_Result = true;

                    //UP_Status = "Update";
                    //int _rows = UpdateDB(UP_Status);
                    MessageBox.Show("카드결제가\r\n취소 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _Search();
                }
                else
                {
                  //  Account_Result = false;




                    MessageBox.Show("카드결제취소\r\n 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    
                }
            }
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

        private int UpdateDB(string Status,string _Account_Name = "")
        {
            int _rows = 0;
            try
            {
                driversBindingSource.EndEdit();
                if (Status == "Update")
                {
                    var Row = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;

                    Row.CarNo = txt_CarNo.Text.Trim().Replace(" ", "");
                    Row.ParkState = cmb_ParkState.Text;
                    Row.ParkCity = cmb_ParkCity.Text;
                    Row.ParkStreet = cmb_ParkStreet.Text;
                    Row.BizNo = txt_BizNo.Text;
                    Row.MobileNo = txt_MobileNo.Text;
                    Row.CEOBirth = txt_Birthday.Text;


                    Row.AccountPrice = int.Parse(txt_AccountPrice.Text);
                    Row.DTGPrice = int.Parse(txt_DTGPrice.Text);
                    Row.FPISPrice = int.Parse(txt_FPISPrice.Text);
                    Row.MyCallPrice = int.Parse(txt_MycallPrice.Text);

                    if (txt_InsuranceShcard.Text.Length != 19)
                    {
                        Row.InsuranceShcard = DBNull.Value.ToString();
                    }
                    else
                    {
                        Row.InsuranceShcard = txt_InsuranceShcard.Text.Replace("-", "");
                    }
                    if (txt_InsuranceKbCard.Text.Length != 19)
                    {
                        Row.InsuranceKbCard = DBNull.Value.ToString();
                    }
                    else
                    {
                        Row.InsuranceKbCard = txt_InsuranceKbCard.Text.Replace("-", "");
                    }
                    if (txt_InsuranceWrCard.Text.Length != 19)
                    {
                        Row.InsuranceWrCard = DBNull.Value.ToString();
                    }
                    else
                    {
                        Row.InsuranceWrCard = txt_InsuranceWrCard.Text.Replace("-", "");
                    }

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
                    
                    Row.PayInputName = _Account_Name;
                    
                    //if (LocalUser.Instance.LogInInformation.IsAdmin)
                    //{
                    //    if (cmb_CandidateGubun.SelectedValue != null)
                    //        Row.CandidateId = int.Parse(cmb_CandidateGubun.SelectedValue.ToString());
                    //}
                    Row.ServiceState = int.Parse(cmb_ServiceState.SelectedValue.ToString());
                    Row.DealerId = int.Parse(cmb_Admin.SelectedValue.ToString());
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
            InitAccLogTable();
            try
            {
                List<String> _WhereStringList = new List<string>();
                //// 1. 본점/지사
                //if (LocalUser.Instance.LogInInformation.IsSubClient)
                //{
                //    if (LocalUser.Instance.LogInInformation.IsAgent)
                //    {
                //        _WhereStringList.Add("DriverInstances.ClientUserId = " + LocalUser.Instance.LogInInformation.ClientUserId.ToString());
                //    }
                //    else
                //    {
                //        _WhereStringList.Add("DriverInstances.SubClientId = " + LocalUser.Instance.LogInInformation.SubClientId.ToString());
                //    }
                //}
                //else if (!LocalUser.Instance.LogInInformation.IsAdmin)
                //{
                //    // 1.1 본지점구분
                //    if (cmb_SubClientId.SelectedIndex > 0)
                //    {
                //        var _SubClientId = (int)cmb_SubClientId.SelectedValue;
                //        if (_SubClientId == 0)
                //        {
                //            _WhereStringList.Add("DriverInstances.SubClientId IS NULL");
                //        }
                //        else
                //        {
                //            _WhereStringList.Add("DriverInstances.SubClientId = " + _SubClientId.ToString());
                //        }
                //    }
                //}

                if (LocalUser.Instance.LogInInformation.IsAdmin && cmb_AdminI.SelectedIndex > 0 )
                {
                   
                   
                        _WhereStringList.Add($"Drivers.DealerId  = " + cmb_AdminI.SelectedValue.ToString() + " ");
                   

                }
                // 2.운송사 검색
                if (LocalUser.Instance.LogInInformation.IsAdmin && cmb_ClientSerach.SelectedIndex > 0 && !String.IsNullOrEmpty(txt_ClientSearch.Text))
                {
                    int[] ClientID = new int[0];
                    if (cmb_ClientSerach.Text == "운송사코드")
                    {
                        _WhereStringList.Add($"Clients.Code Like '%"+ txt_ClientSearch.Text + "%'");
                    }
                    else if (cmb_ClientSerach.Text == "운송사명")
                    {
                        _WhereStringList.Add($"Clients.Name Like '%"+ txt_ClientSearch.Text + "%'");
                    }
                }
                // 3. 차량 구분
                if (cmb_CarSearchGubun.SelectedIndex > 0)
                {
                    if (cmb_CarSearchGubun.Text == "직영")
                    {
                        _WhereStringList.Add("DriverInstances.CarGubun = 1");
                    }
                    else if (cmb_CarSearchGubun.Text == "위수탁(지입)")
                    {
                        _WhereStringList.Add("DriverInstances.CarGubun = 2");
                    }
                    else if (cmb_CarSearchGubun.Text == "장기용차")
                    {
                        _WhereStringList.Add("DriverInstances.CarGubun = 3");
                    }
                    else if (cmb_CarSearchGubun.Text == "단기용차")
                    {
                        _WhereStringList.Add("DriverInstances.CarGubun = 4");
                    }
                    //else if (cmb_CarSearchGubun.Text == "직영/위수탁(지입)/장기/단가")
                    //{
                    //    _WhereStringList.Add("Drivers.CarGubun in(1,2,3,4) AND Drivers.CandidateId = " + LocalUser.Instance.LogInInformation.ClientId + " ");
                    //}
                    //else if (cmb_CarSearchGubun.Text == "타사차")
                    //{
                    //    _WhereStringList.Add($"Drivers.CandidateId <> {LocalUser.Instance.LogInInformation.ClientId}");

                    //}
                }
                // 4. 단어 검색
                if (cmb_Search.SelectedIndex > 0)
                {
                    switch (cmb_Search.Text)
                    {
                        case "차주 상호":
                            _WhereStringList.Add("Drivers.Name LIKE '%" + txt_Search.Text.Replace("-", "") + "%'");
                            break;
                        case "차주 사업자번호":
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
                                _WhereStringList.Add("DriverInstances.GroupName = '" + txt_Search.Text.ToUpper() + "'");
                            }
                            break;
                        default:
                            break;
                    }
                }
                // 5. 차종 / 톤수 / 상태
                if (cmb_CarTypeS.SelectedIndex > 0)
                {
                    _WhereStringList.Add("Drivers.CarType = " + cmb_CarTypeS.SelectedValue);
                }
                if (cmb_CarSizeS.SelectedIndex > 0)
                {
                    _WhereStringList.Add("Drivers.CarSize = "+ cmb_CarSizeS.SelectedValue);
                }
                if (cmb_ServiceState_I.SelectedIndex > 0)
                {
                    _WhereStringList.Add("Drivers.ServiceState = " + cmb_ServiceState_I.SelectedValue);
                }
                // 6. 첨부파일
                if (cmb_File.SelectedIndex > 0)
                {
                    if (cmb_File.Text == "있음")
                        _WhereStringList.Add("Drivers.HasBizPaper = 1 AND Drivers.HasCarPaper = 1");
                    else if (cmb_File.Text == "없음")
                        _WhereStringList.Add("(Drivers.HasBizPaper = 0 or Drivers.HasBizPaper IS NULL  OR Drivers.HasCarPaper = 0 or Drivers.HasCarPaper IS NULL )");
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
            DataGridViewColumn[] cols = new DataGridViewColumn[]{nameDataGridViewTextBoxColumn,bizNoDataGridViewTextBoxColumn,loginIdDataGridViewTextBoxColumn,cEODataGridViewTextBoxColumn,carNoDataGridViewTextBoxColumn,mobileNoDataGridViewTextBoxColumn,Group,carYearDataGridViewTextBoxColumn
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
            if (cmb_Search.Items.Count > 0) cmb_Search.SelectedIndex = 0;
            cmb_ClientSerach.SelectedIndex = 0;
            cmb_CarSearchGubun.SelectedIndex = 0;
            cmb_ServiceState_I.SelectedIndex = 0;
            cmb_File.SelectedIndex = 0;

            var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CarTypeS.DataSource = CarTypeDataSource;
            cmb_CarTypeS.DisplayMember = "Name";
            cmb_CarTypeS.ValueMember = "value";

            var CarSizeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CarSizeS.DataSource = CarSizeDataSource;
            cmb_CarSizeS.DisplayMember = "Name";
            cmb_CarSizeS.ValueMember = "value";

            cmb_AdminI.DisplayMember = "Text";
            cmb_AdminI.ValueMember = "Value";
            cmb_AdminI.DataSource = Filter.Dealer.DealerList;
            cmb_AdminI.SelectedIndex = 0;
        }


        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        List<BasicModel> _ClientDataSource = new List<BasicModel>();
        private void _InitCmb()
        {
            cmb_Admin.DisplayMember = "Text";
            cmb_Admin.ValueMember = "Value";
            cmb_Admin.DataSource = Filter.Dealer.DealerList;

          

            var CarstateDataSource = (from a in AddressList select new { a.State }).Distinct().ToArray();
            cmb_ParkState.DataSource = CarstateDataSource;
            cmb_ParkState.DisplayMember = "State";
            cmb_ParkState.ValueMember = "State";

            var BizTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BizType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_BizType.DataSource = BizTypeDataSource;
            cmb_BizType.DisplayMember = "Name";
            cmb_BizType.ValueMember = "value";

            var RouteTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "RouteType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_RouteType.DataSource = RouteTypeDataSource;
            cmb_RouteType.DisplayMember = "Name";
            cmb_RouteType.ValueMember = "value";

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

            var InsuranceTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "InsuranceType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_InsuranceType.DataSource = InsuranceTypeDataSource;
            cmb_InsuranceType.DisplayMember = "Name";
            cmb_InsuranceType.ValueMember = "value";

            var ServiceStateDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_ServiceState.DataSource = ServiceStateDataSource;
            cmb_ServiceState.DisplayMember = "Name";
            cmb_ServiceState.ValueMember = "value";

            var ServiceStateDataSource1 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_ServiceState_I.DataSource = ServiceStateDataSource1;
            cmb_ServiceState_I.DisplayMember = "Name";
            cmb_ServiceState_I.ValueMember = "value";

            var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CarType.DataSource = CarTypeDataSource;
            cmb_CarType.DisplayMember = "Name";
            cmb_CarType.ValueMember = "value";

            var CarSizeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CarSize.DataSource = CarSizeDataSource;
            cmb_CarSize.DisplayMember = "Name";
            cmb_CarSize.ValueMember = "value";

            cmb_ParkCity.DisplayMember = "City";
            cmb_ParkCity.ValueMember = "City";

            cmb_ParkStreet.DisplayMember = "Street";
            cmb_ParkStreet.ValueMember = "Street";

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT Name, Code, ClientId FROM Clients";
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            while (_Reader.Read())
                            {
                                _ClientDataSource.Add(new BasicModel
                                {
                                    Text = _Reader.GetString(0),
                                    TextOption1 = _Reader.GetString(1),
                                    Id = _Reader.GetInt32(2),
                                });
                            }
                        }
                        cmb_CandidateGubun.DataSource = _ClientDataSource;
                        cmb_CandidateGubun.DisplayMember = "Text";
                        cmb_CandidateGubun.ValueMember = "Id";
                    }
                    _Connection.Close();
                }
            }

            var UsePayNowDatasource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "UsePay").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_UsePayNow.DataSource = UsePayNowDatasource;
            cmb_UsePayNow.DisplayMember = "Name";
            cmb_UsePayNow.ValueMember = "Value";

            Dictionary<string, string> NotificationGroup = new Dictionary<string, string>();

            NotificationGroup.Add("0", "미설정");
            NotificationGroup.Add("A", "A");
            NotificationGroup.Add("B", "B");
            NotificationGroup.Add("C", "C");

            cmb_Group.DataSource = new BindingSource(NotificationGroup, null);
            cmb_Group.DisplayMember = "Value";
            cmb_Group.ValueMember = "Key";

            var CarGubunDatasource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarGubun").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CarGubun.DataSource = CarGubunDatasource;
            cmb_CarGubun.DisplayMember = "Name";
            cmb_CarGubun.ValueMember = "Value";

            var FPISCarGubunDatasource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "FPisCarType").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_FPIS_CarType.DataSource = FPISCarGubunDatasource;
            cmb_FPIS_CarType.DisplayMember = "Name";
            cmb_FPIS_CarType.ValueMember = "Value";
            if (!LocalUser.Instance.LogInInformation.IsAdmin && !LocalUser.Instance.LogInInformation.IsSubClient)
            {
                SubClientIdDictionary.Add(-1, "본지점구분");
                SubClientIdDictionary.Add(0, "본점");
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _AllowSubCommand = _Connection.CreateCommand())
                    {
                        _AllowSubCommand.CommandText = "SELECT AllowSub FROM Clients WHERE ClientId = @ClientId";
                        _AllowSubCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        var _AllowSub = (bool)_AllowSubCommand.ExecuteScalar();
                        if (_AllowSub)
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT Name, SubClientId FROM SubClients WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader())
                                {
                                    while (_Reader.Read())
                                    {
                                        SubClientIdDictionary.Add(_Reader.GetInt32(1), _Reader.GetString(0));
                                    }
                                }
                            }
                            cmb_SubClientId.DataSource = SubClientIdDictionary.ToList();
                            cmb_SubClientId.DisplayMember = "Value";
                            cmb_SubClientId.ValueMember = "Key";
                        }
                        else
                        {
                            SubClientId.Visible = false;
                            cmb_SubClientId.Visible = false;
                        }
                    }
                    _Connection.Close();
                }
            }
            else
            {
                SubClientId.Visible = false;
                cmb_SubClientId.Visible = false;
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0203_CAROWNERMANAGE_FAULT_Add _Form = new FrmMN0203_CAROWNERMANAGE_FAULT_Add();
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

                //매입,배차 있을경우 return;

                var Query1 =
                         "Select Count(*) From Orders Where DriverId = @DriverId ";
                bool IsOrders = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;

                    cmd1.Parameters.AddWithValue("@DriverId", Selected.DriverId);

                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsOrders = true;
                    }

                    cn.Close();
                }

                var Query2 =
                        "Select Count(*) From Trades Where DriverId = @DriverId ";
                bool IsTrades = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query2;

                    cmd1.Parameters.AddWithValue("@DriverId", Selected.DriverId);

                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsTrades = true;
                    }

                    cn.Close();
                }

                if (LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (IsOrders && IsTrades)
                    {
                        if (MessageBox.Show("매입관리 ,배차관리 내역이 존재합니다.\r\n그래도 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            // MessageBox.Show("매입관리 ,배차관리 내역이 존재합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                    }
                    else if (IsOrders)
                    {
                        if (MessageBox.Show("배차관리 내역이 존재합니다.\r\n그래도 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            //  MessageBox.Show("배차관리 내역이 존재합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                    }
                    else if (IsTrades)
                    {
                        if (MessageBox.Show("매입관리 내역이 존재합니다.\r\n그래도 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            // MessageBox.Show("매입관리 내역이 존재합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                    }


                    DriverRepository mDriverRepository = new DriverRepository();
                    mDriverRepository.DeleteDriver(Selected.DriverId);
                    _Search();
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "차량", 1), "차량정보 삭제 성공");
                }
                else
                {
                    if (IsOrders && IsTrades)
                    {

                        MessageBox.Show("매입관리 ,배차관리 내역이 존재하여 삭제 할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    DriverRepository mDriverRepository = new DriverRepository();
                    mDriverRepository.DeleteDriver(Selected.DriverId);
                    _Search();
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "차량", 1), "차량정보 삭제 성공");

                }







               
            }
        }

        private void cmb_ParkState_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime Start = DateTime.Now;
            if (cmb_ParkState.SelectedValue == null)
                return;
            cmb_ParkCity.Enabled = true;
            var ParkCityDataSource = AddressList.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            cmb_ParkCity.DataSource = ParkCityDataSource;
            cmb_ParkCity.DisplayMember = "City";
            cmb_ParkCity.ValueMember = "City";
            System.Diagnostics.Debug.WriteLine((DateTime.Now - Start).ToString());
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
            pic_BizPaper.Visible = true;
            DriverId.HasBizPaper = true;
            txt_BizPaper.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.UpdateDriverSetBizPaper(DriverId.DriverId);
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
            txt_BizPaper.Text = "";
            pic_BizPaper.Visible = false;
            DriverId.HasBizPaper = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        BaseDataSet.DriversRow PreviewItem = null;
        bool MethodChange = false;
        private void driversBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            lblInfo.Text = string.Empty;
            if (PreviewItem != null && PreviewItem.RowState == DataRowState.Modified)
            {
                PreviewItem.RejectChanges();
                PreviewItem = null;
            }
            if (driversBindingSource.Current == null)
                return;
            if (((DataRowView)driversBindingSource.Current).Row is BaseDataSet.DriversRow Selected)
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

                txt_CarNo.Text = Selected.CarNo;
                txt_BizPaper.Clear();
                if (Selected.HasBizPaper)
                {
                    pic_BizPaper.Visible = true;
                }
                else
                {
                    pic_BizPaper.Visible = false;
                }
                txt_CarPaper.Clear();
                if (Selected.HasCarPaper)
                {
                    pic_CarPaper.Visible = true;
                }
                else
                {
                    pic_CarPaper.Visible = false;
                }
                txt_CarReg.Clear();
                if (Selected.HasCarReg)
                {
                    pic_CarReg.Visible = true;
                }
                else
                {
                    pic_CarReg.Visible = false;
                }

                if (Selected.HasCarEtc)
                {
                    pic_CarEtc.Visible = true;
                }
                else
                {
                    pic_CarEtc.Visible = false;
                }

                if (LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    cmb_CandidateGubun.SelectedValue = Selected.CandidateId;
                }
                cmb_ServiceState.SelectedValue = Selected.ServiceState;
                cmb_ParkState.SelectedValue = Selected.ParkState;
                cmb_ParkCity.SelectedValue = Selected.ParkCity;
                cmb_ParkStreet.SelectedValue = Selected.ParkStreet;
                txt_AccountPrice.Text = Selected.AccountPrice.ToString();
                txt_DTGPrice.Text = Selected.DTGPrice.ToString();
                txt_FPISPrice.Text = Selected.FPISPrice.ToString();
                txt_MycallPrice.Text = Selected.MyCallPrice.ToString();
                txt_MobileNo.Text = Selected.MobileNo;

                if (!String.IsNullOrEmpty(Selected.BizNo))
                {
                    txt_BizNo.Text = Selected.BizNo.Replace("-", "").Substring(0, 3) + "-" + Selected.BizNo.Replace("-", "").Substring(3, 2) + "-" + Selected.BizNo.Replace("-", "").Substring(5, 5);
                }

                if (!String.IsNullOrEmpty(Selected.MobileNo))
                {
                    string _S = Selected.MobileNo.Replace("-", "");

                    if (_S.Length == 8)
                    {
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
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
                        if (_S.Length == 12)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 3) + "-" + _S.Substring(6, 4);
                        }
                        else if (_S.Length == 13)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                        }
                        else if (_S.Length > 13)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4) + "-" + _S.Substring(8, 4);
                        }
                    }

                    txt_MobileNo.Text = _S;
                }

                cmb_Admin.SelectedValue = Selected.DealerId.ToString() ;
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
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
                    //cmb_PayBankName.Enabled = true;
                    //txt_PayAccountNo.ReadOnly = false;
                    //txt_PayInputName.ReadOnly = false;


                    cmb_BizType.Enabled = true;
                    cmb_RouteType.Enabled = true;
                    cmb_InsuranceType.Enabled = true;
                    cmb_UsePayNow.Enabled = true;
                    txt_CarNo.Enabled = true;
                    cmb_CarType.Enabled = true;
                    cmb_CarSize.Enabled = true;
                    txt_CarInfo.Enabled = true;
                    cmb_CarGubun.Enabled = true;
                    dtp_RequestFrom.Enabled = true;
                    dtp_RequestTo.Enabled = true;
                    txt_CarYear.Enabled = true;
                    cmb_ParkState.Enabled = true;
                    cmb_ParkCity.Enabled = true;
                    cmb_ParkStreet.Enabled = true;
                    cmb_FPIS_CarType.Enabled = true;
                    pnDetail3.Enabled = true;
                    btn_BizPaperAdd.Enabled = true;
                    btn_BizPaperDelete.Enabled = true;
                    btn_CarPaperAdd.Enabled = true;
                    btn_CarPaperDelete.Enabled = true;
                    cmb_PayBankName.Enabled = true;
                    txt_PayAccountNo.Enabled = true;
                    txt_PayInputName.Enabled = true;
                    txt_BizNo.ReadOnly = false;
                    btn_BizPaperAdd.Enabled = true;
                    btn_BizPaperDelete.Enabled = true;
                    btn_CarPaperAdd.Enabled = true;
                    btn_CarPaperDelete.Enabled = true;
                    btn_CarRegAdd.Enabled = true;
                    btn_CarRegDelete.Enabled = true;
                    btn_ImageView.Enabled = true;
                    btn_BizPaperShow.Enabled = true;
                    btn_CarPaperShow.Enabled = true;
                    btn_ShowCarReg.Enabled = true;
                    pic_BizPaper.Enabled = true;
                    pic_CarPaper.Enabled = true;
                    pic_CarReg.Enabled = true;
                    if (Selected.ServiceState != 3)
                    {

                        cmb_PayBankName.Enabled = false;
                        txt_PayAccountNo.Enabled = false;
                        txt_PayInputName.Enabled = false;

                        txt_BizNo.ReadOnly = true;

                        btn_BizPaperAdd.Enabled = false;
                        btn_BizPaperDelete.Enabled = false;
                        btn_CarPaperAdd.Enabled = false;
                        btn_CarPaperDelete.Enabled = false;
                    }



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

                }
                if (Selected.ServiceState == 1)
                {
                    txt_BizNo.ReadOnly = true;
                    txt_CarNo.ReadOnly = true;
                    txt_PayAccountNo.ReadOnly = true;
                    cmb_PayBankName.Enabled = false;
                    txt_PayInputName.ReadOnly = true;
                }

                //제공이 아닐때
                else
                {
                    //관리자
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        if (String.IsNullOrEmpty(Selected.PayAccountNo) || String.IsNullOrEmpty(Selected.PayBankCode) || String.IsNullOrEmpty(Selected.PayInputName))
                        {

                            cmb_PayBankName.Enabled = true;
                            txt_PayAccountNo.Enabled = true;
                            txt_PayInputName.Enabled = true;
                            txt_PayAccountNo.ReadOnly = false;
                            txt_PayInputName.ReadOnly = false;

                        }
                        else
                        {
                            //첨부파일 있을때
                            if (Selected.HasCarPaper == true && Selected.HasBizPaper == true)
                            {
                                cmb_PayBankName.Enabled = false;
                                txt_PayAccountNo.Enabled = false;
                                txt_PayInputName.Enabled = false;
                                txt_PayAccountNo.ReadOnly = true;
                                txt_PayInputName.ReadOnly = true;
                            }
                            else
                            {

                                cmb_PayBankName.Enabled = true;
                                txt_PayAccountNo.Enabled = true;
                                txt_PayInputName.Enabled = true;
                                txt_PayAccountNo.ReadOnly = false;
                                txt_PayInputName.ReadOnly = false;

                            }
                        }

                    }
                    else
                    {
                        //계좌번호 수정옵션
                        if (LocalUser.Instance.LogInInformation.Client.CarBankYn)
                        {
                            cmb_PayBankName.Enabled = true;
                            txt_PayAccountNo.Enabled = true;
                            txt_PayInputName.Enabled = true;
                            txt_PayAccountNo.ReadOnly = false;
                            txt_PayInputName.ReadOnly = false;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(Selected.PayAccountNo) || String.IsNullOrEmpty(Selected.PayBankCode) || String.IsNullOrEmpty(Selected.PayInputName))
                            {

                                cmb_PayBankName.Enabled = true;
                                txt_PayAccountNo.Enabled = true;
                                txt_PayInputName.Enabled = true;
                                txt_PayAccountNo.ReadOnly = false;
                                txt_PayInputName.ReadOnly = false;

                            }
                            else
                            {
                                //첨부파일 있을때
                                if (Selected.HasCarPaper == true && Selected.HasBizPaper == true)
                                {
                                    cmb_PayBankName.Enabled = false;
                                    txt_PayAccountNo.Enabled = false;
                                    txt_PayInputName.Enabled = false;
                                    txt_PayAccountNo.ReadOnly = true;
                                    txt_PayInputName.ReadOnly = true;
                                }
                                else
                                {
                                    cmb_PayBankName.Enabled = false;
                                    txt_PayAccountNo.Enabled = false;
                                    txt_PayInputName.Enabled = false;
                                    txt_PayAccountNo.ReadOnly = true;
                                    txt_PayInputName.ReadOnly = true;
                                }
                            }
                        }

                    }


                    
                }
                //lbl_VAccount_Helper.Visible = false;
                //if (!String.IsNullOrEmpty(Selected.VAccount))
                //    lbl_VAccount_Helper.Visible = true;
                if (!String.IsNullOrEmpty(Selected.VBankName) && !String.IsNullOrEmpty(Selected.VAccount))
                {
                    VAccountInfo.Text = $"가상계좌번호 : {Selected.VAccount} ({Selected.VBankName})";
                }
                else
                {
                    VAccountInfo.Text = "";
                }
                txtMisu.Text = Selected.Misu.ToString("N0");
                txtMizi.Text = Selected.Mizi.ToString("N0");

                string ReferralName = "";
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT Top 1 ISNULL(cs.SangHo,N'') as ReferralName FROM ORDERS LEFT JOIN CUSTOMERS ON ORDERS.CustomerId = CUSTOMERS.CustomerId left JOIN Customers as Cs ON cs.CustomerId = orders.ReferralId WHERE Orders.DriverId = @DriverId Order by Orders.CreateTime desc";
                        _Command.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                        var o = _Command.ExecuteScalar();
                        if (o != null )
                            ReferralName = o.ToString();
                    }
                    _Connection.Close();
                }


                lblReferralName.Text = ReferralName;

                MethodChange = false;
                if (tabControl1.SelectedTab == tabPagePoint)
                {
                    tabControl1.SelectedTab = tabPageBase;
                }

            }
            else
            {
                txt_MobileNo.Clear();
            }
        }

        private void cmb_AddressState_SelectedValueChanged(object sender, EventArgs e)
        {

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
            pic_CarPaper.Visible = true;
            DriverId.HasCarPaper = true;
            txt_CarPaper.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.UpdateDriverSetCarPaper(DriverId.DriverId);
            DriverId.HasCarPaper = true;
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
            txt_CarPaper.Text = "";
            pic_CarPaper.Visible = false;
            DriverId.HasCarPaper = false;
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            cmb_CarSizeS.SelectedIndex = 0;
            cmb_CarTypeS.SelectedIndex = 0;
            cmb_ClientSerach.SelectedIndex = 0;
            txt_ClientSearch.Text = string.Empty;
            cmb_CarSearchGubun.SelectedIndex = 0;
            cmb_ServiceState_I.SelectedIndex = 0;
            cmb_File.SelectedIndex = 0;
            if (cmb_SubClientId.Items.Count > 0)
                cmb_SubClientId.SelectedIndex = 0;
            cmb_AdminI.SelectedIndex = 0;
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
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as BaseDataSet.DriversRow;
            if (Selected == null)
                return;
            if (e.ColumnIndex == rowNUMDataGridViewTextBoxColumn.Index)
            {
                grid1[e.ColumnIndex, e.RowIndex].Value = (grid1.Rows.Count - e.RowIndex).ToString();
            }
            else if (grid1.Columns[e.ColumnIndex] == Gubun)
            {
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            if (Selected.ClientUserId != LocalUser.Instance.LogInInformation.ClientUserId)
                            {
                                e.Value = "타사차";
                                return;
                            }
                        }
                        if (Selected.SubClientId != LocalUser.Instance.LogInInformation.SubClientId)
                        {
                            e.Value = "타사차";
                            return;
                        }
                    }
                    if (Selected.CandidateId != LocalUser.Instance.LogInInformation.ClientId)
                    {
                        e.Value = "타사차";
                        return;
                    }
                }
                var StaticOptionsQuery = SingleDataSet.Instance.StaticOptions.ToArray();
                var Query = StaticOptionsQuery.Where(c => c.Div == "CarGubun" && c.Value == Selected.CarGubun);
                if (Query.Any())
                    e.Value = Query.First().Name;
                else
                    e.Value = "";
            }
            else if (grid1.Columns[e.ColumnIndex] == bizNoDataGridViewTextBoxColumn)
            {
                if (Selected.BizNo.Length == 10)
                    e.Value = Selected.BizNo.Substring(0, 3) + "-" + Selected.BizNo.Substring(3, 2) + "-" + Selected.BizNo.Substring(5, 5);
                else
                    e.Value = Selected.BizNo;
            }
            else if (grid1.Columns[e.ColumnIndex] == mobileNoDataGridViewTextBoxColumn)
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
            else if (grid1.Columns[e.ColumnIndex] == Column1)
                grid1[e.ColumnIndex, e.RowIndex].Value = Selected.ParkState + "/" + Selected.ParkCity + "/" + Selected.ParkStreet;
            else if (grid1.Columns[e.ColumnIndex] == createDateDataGridViewTextBoxColumn)
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            else if (grid1.Columns[e.ColumnIndex] == RouteType)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "RouteType").Where(c => c.Value == Selected.RouteType).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
            else if (grid1.Columns[e.ColumnIndex] == carTypeDataGridViewTextBoxColumn)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Where(c => c.Value == Selected.CarType).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
            else if (grid1.Columns[e.ColumnIndex] == carSizeDataGridViewTextBoxColumn)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").Where(c => c.Value == Selected.CarSize).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
            else if (grid1.Columns[e.ColumnIndex] == UsePayNow)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "UsePay").Where(c => c.Value == Selected.UsePayNow).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
            else if (grid1.Columns[e.ColumnIndex] == AppUse)
            {
                if (Selected.AppUse == true)
                    e.Value = "○";
                else
                    e.Value = "X";
            }
            else if (grid1.Columns[e.ColumnIndex] == ServiceState)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState" && c.Value == Selected.ServiceState).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
            else if (grid1.Columns[e.ColumnIndex] == FileBiz)
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
            else if (e.ColumnIndex == SubClientId.Index)
            {
                if(Selected.CandidateId != LocalUser.Instance.LogInInformation.ClientId)
                {
                    e.Value = "타사차";
                }
                else
                {
                    if (SubClientIdDictionary.ContainsKey(Selected.SubClientId))
                        e.Value = SubClientIdDictionary[Selected.SubClientId];
                }
            }
            else if(e.ColumnIndex == Group.Index)
            {
                if (e.Value.ToString() == "0")
                    e.Value = "미설정";
            }
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                if(e.ColumnIndex == ClientCode.Index)
                {
                    var Query = _ClientDataSource.Where(c => c.Id == Selected.CandidateId).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().TextOption1;
                    }
                }
                else if(e.ColumnIndex == ClientName.Index)
                {
                    var Query = _ClientDataSource.Where(c => c.Id == Selected.CandidateId).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().Text;
                    }
                }
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void cmb_ParkCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime Start = DateTime.Now;
            if (cmb_ParkState.SelectedValue == null)
                return;
            if (cmb_ParkCity.SelectedValue == null)
                return;
            cmb_ParkStreet.Enabled = true;
            var CarParkStreetDataSource = AddressList.Where(c => c.City == cmb_ParkCity.SelectedValue.ToString()).Where(c => c.Street != "").Select(c => new { c.Street }).Distinct().ToArray();
            cmb_ParkStreet.DataSource = CarParkStreetDataSource;
            cmb_ParkStreet.DisplayMember = "Street";
            cmb_ParkStreet.ValueMember = "Street";
            System.Diagnostics.Debug.WriteLine((DateTime.Now - Start).ToString());
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

        private void cmb_CarGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (driversBindingSource.Current != null)
            {
                var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
                if (Selected != null)
                {
                    if (Selected.CandidateId == LocalUser.Instance.LogInInformation.ClientId)
                    {
                        if (cmb_CarGubun.SelectedIndex == 3)
                        {
                            pnRequestPeriod.Visible = true;
                        }
                        else
                        {
                            pnRequestPeriod.Visible = false;
                        }
                    }
                }
            }
        }

        private void btn_driverInfo_Click(object sender, EventArgs e)
        {
            FrmDriverSearch _frmDriverSearch = new FrmDriverSearch();
            _frmDriverSearch.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmDriverSearch.grid1.SelectedCells.Count == 0) return;
                if (_frmDriverSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                txt_CarYear.Text = _frmDriverSearch.grid1[0, _frmDriverSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                _frmDriverSearch.Close();
            });
            _frmDriverSearch.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmDriverSearch.grid1.SelectedCells.Count == 0) return;
                if (_frmDriverSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                txt_CarYear.Text = _frmDriverSearch.grid1[0, _frmDriverSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                _frmDriverSearch.Close();
            });
            _frmDriverSearch.Owner = this;
            _frmDriverSearch.StartPosition = FormStartPosition.CenterParent;
            _frmDriverSearch.ShowDialog();
        }

        private void dtp_InsuranceNowDate_ValueChanged(object sender, EventArgs e)
        {
            dtp_InsuranceNextDate.Value = dtp_InsuranceNowDate.Value.AddMonths(6);
        }

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            //int _DriverType = 1;
            //if (!LocalUser.Instance.LogInInformation.IsAdmin)
            //{
            //    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //    {
            //        _Connection.Open();
            //        using (SqlCommand _ClientsCommand = _Connection.CreateCommand())
            //        {
            //            _ClientsCommand.CommandText = "SELECT DriverType FROM Clients WHERE ClientId = @ClientId";
            //            _ClientsCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
            //            var o = _ClientsCommand.ExecuteScalar();
            //            if (o == null || !int.TryParse(o.ToString(), out _DriverType))
            //            {
            //                MessageBox.Show("항목을 가져오는 중 오류가 발생하였습니다. 잠시 후에 다시 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                return;
            //            }
            //        }
            //        _Connection.Close();
            //    }
            //}
            //if (_DriverType == 1)
            //{
            //    ExcelExportBasic();
            //}
            //else
            //{
            //    ExcelExport();
            //}

            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;




            var fileString = "차량정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.CustomerOrderList))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var ColumnIndexMap = new Dictionary<int, int>();

            List<string> ColumnHeaderMap = new List<string>();
            var ColumnIndex = 0;
            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { checkbox }.Contains(grid1.Columns[i])))
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { checkbox }.Contains(grid1.Columns[i])))
                {
                    ColumnHeaderMap.Add(grid1.Columns[i].HeaderText);
                }

            }


            for (int i = 0; i < ColumnHeaderMap.Count; i++)
            {

                _Sheet.Cells[1, i + 1].Value = ColumnHeaderMap[i];

                _Sheet.Cells[RowIndex, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            }
            for (int i = 0; i < grid1.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = grid1[ColumnIndexMap[j], i].FormattedValue?.ToString();

                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    _Sheet.Cells[1, j + 1].AutoFitColumns();
                }

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
            for (int i = 0; i < grid1.RowCount; i++)
            {
                var _Model = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as BaseDataSet.DriversRow;
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

                _Sheet.Cells[RowIndex, 31].Value = _Model.LoginId;

                var Q = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState" && c.Value == _Model.ServiceState).ToArray();
                _Sheet.Cells[RowIndex, 32].Value = Q.First().Name;
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
            if (grid1.RowCount == 0)
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
            for (int i = 0; i < grid1.RowCount; i++)
            {
                var _Model = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as BaseDataSet.DriversRow;
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
                _Sheet.Cells[RowIndex, 11].Value = _MobleNo.Replace("-","");
                _Sheet.Cells[RowIndex, 12].Value = _Model.CarNo;
                _Sheet.Cells[RowIndex, 13].Value = _Model.CarType;
                _Sheet.Cells[RowIndex, 14].Value = _Model.CarSize;
                _Sheet.Cells[RowIndex, 15].Value = _Model.CarInfo;
                _Sheet.Cells[RowIndex, 16].Value = _Model.PayBankName;
                _Sheet.Cells[RowIndex, 17].Value = m_crypt.Decrypt(_Model.PayAccountNo);
                _Sheet.Cells[RowIndex, 18].Value = _Model.PayInputName;
                _Sheet.Cells[RowIndex, 19].Value = _Model.VAccount;
                _Sheet.Cells[RowIndex, 20].Value = _Model.Misu;
                _Sheet.Cells[RowIndex, 21].Value = _Model.Mizi;
                _Sheet.Cells[RowIndex, 23].Value = _Model.LoginId;

                var Q = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState" && c.Value == _Model.ServiceState).ToArray();
                _Sheet.Cells[RowIndex, 24].Value = Q.First().Name;
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


        private void chk_AccountUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_AccountUse.Checked == true)
            {
                txt_AccountPrice.Text = (15000 - int.Parse(txt_DTGPrice.Text)).ToString();

            }
            else
            {
                txt_AccountPrice.Text = "0";
            }

            DTGPrice = int.Parse(txt_DTGPrice.Text);
            AccountPrice = int.Parse(txt_AccountPrice.Text);

            ServicePrice = AccountPrice + DTGPrice;


            //if (ServicePrice >= 18000)
            //{
            //    ServicePrice = 17000;
            //}

            if (chk_useTax.Checked == true)
            {
                txt_ServicePrice.Text = ((ServicePrice * 0.1) + ServicePrice).ToString();
            }
            else
            {
                txt_ServicePrice.Text = (ServicePrice).ToString();
            }
        }

        private void chk_DTGUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DTGUse.Checked == true)
            {
                txt_DTGPrice.Text = "5000";
                // DTGPrice = 3000;
            }
            else
            {
                txt_DTGPrice.Text = "0";
            }



            DTGPrice = int.Parse(txt_DTGPrice.Text);
            AccountPrice = int.Parse(txt_AccountPrice.Text);

            ServicePrice = AccountPrice + DTGPrice;


            //if (ServicePrice >= 18000)
            //{
            //    ServicePrice = 17000;
            //}

            if (chk_useTax.Checked == true)
            {
                txt_ServicePrice.Text = ((ServicePrice * 0.1) + ServicePrice).ToString();
            }
            else
            {
                txt_ServicePrice.Text = (ServicePrice).ToString();
            }
        }

        private void chk_FPISUse_CheckedChanged(object sender, EventArgs e)
        {
            // fSum();
        }

        private void chk_MyCallUSe_CheckedChanged(object sender, EventArgs e)
        {
            //fSum();
        }
        int AccountPrice = 0;
        int DTGPrice = 0;
        int FPISPrice = 0;
        int MyCallPrice = 0;
        int ServicePrice = 0;

        private void fSum()
        {



            //if (chk_FPISUse.Checked == true)
            //{
            //    FPISPrice = 0;
            //}
            //else
            //{
            //    FPISPrice = 0;
            //}

            //if (chk_MyCallUSe.Checked == true)
            //{
            //    MyCallPrice = 15000;
            //}
            //else
            //{
            //    MyCallPrice = 0;
            //}


        }

        private void chk_useTax_CheckedChanged(object sender, EventArgs e)
        {
            DTGPrice = int.Parse(txt_DTGPrice.Text);
            AccountPrice = int.Parse(txt_AccountPrice.Text);

            ServicePrice = AccountPrice + DTGPrice;


            //if (ServicePrice >= 18000)
            //{
            //    ServicePrice = 17000;
            //}

            if (chk_useTax.Checked == true)
            {
                txt_ServicePrice.Text = ((ServicePrice * 0.1) + ServicePrice).ToString();
            }
            else
            {
                txt_ServicePrice.Text = (ServicePrice).ToString();
            }


            //fSum();
        }
        bool overhead = false;
        List<string> checkedCodes = new List<string>();


        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            overhead = true;
            for (int i = 0; i < grid1.RowCount; i++)
            {
                grid1[checkbox.Index, i].Value = chkAllSelect.Checked;
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
                if (e.ColumnIndex == 0)
                {

                    object o = grid1[e.ColumnIndex, e.RowIndex].Value;

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
      
        string SmsResult;


        private void btn_SMS_Click(object sender, EventArgs e)

        {
            string SVC_RT;
            string SVC_MSG;
            List<string> _SVC_RT = new List<string>() ;
            if (!LocalUser.Instance.LogInInformation.Client.HideAddTrade && !LocalUser.Instance.LogInInformation.Client.SmsYn)
            {
                MessageBox.Show("본 서비스의 “CRM 연동”과 “문자 전송”은 \r\nLG U+ 인터넷전화기를 통해서만 가능 합니다.\r\n귀 사는\r\nLG U+ 미 사용자이므로 사용하실 수 없습니다.\r\n\r\nLG U+인터넷전화 신청\r\n문의 : ☎ 1833-2363  엑티브아이티㈜", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
            
              
            }

            int successcount = 0;
            int failcount = 0;
            if (LocalUser.Instance.LogInInformation.IsAdmin)
                return;
            //bool AllowSMS = false;
            //using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //{
            //    _Connection.Open();
            //    using (SqlCommand _Command = _Connection.CreateCommand())
            //    {
            //        _Command.CommandText = "SELECT AllowSMS FROM Clients WHERE ClientId = @ClientId";
            //        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
            //        var o = _Command.ExecuteScalar();
            //        if (o != null && (bool)o == true)
            //            AllowSMS = true;
            //    }
            //    _Connection.Close();
            //}
            //if (!AllowSMS)
            //{
            //    MessageBox.Show("SMS 전송 서비스를 신청하지 않았습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
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
                SMSText = _FrmSMS.txt_SMS.Text;//.Replace("\r\n"," ");


                bar.Value = 0;
                bar.Maximum = checkedCodes.Count();
                bar.Visible = true;
                pnProgress.Visible = true;
                //   string title = string.Format("세금계산서 ({0}-{1})", dtp_Sdate.Text.Replace("/", "-"), dtp_Edate.Text.Replace("/", "-"));
                List<BaseDataSet.DriversRow> drivers = new List<BaseDataSet.DriversRow>();
                foreach (DataGridViewRow row in grid1.Rows)
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
                            
                            string sha512password = EncryptSHA512(CallHelper.Instance.Password);

                          


                            string Parameter = "";
                            Parameter = String.Format(@"id={0}&pass={1}&destnumber={2}&smsmsg={3}", CallHelper.Instance.Id, sha512password, item.MobileNo.Replace("-", ""), SMSText);
                            



                            JObject response = null;

                            var uriBuilder = new UriBuilder("https://centrex.uplus.co.kr/RestApi/smssend");
                           





                            uriBuilder.Query = Parameter;
                            Uri finalUrl = uriBuilder.Uri;


                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);
                          


                            request.Method = "POST";
                            request.ContentType = "text/json;";
                            request.ContentLength = 0;

                            request.Headers.Add("header-staff-api", "value");

                            var httpResponse = (HttpWebResponse)request.GetResponse();

                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                response = JObject.Parse(streamReader.ReadToEnd());

                                SVC_RT = response["SVC_RT"].ToString();
                                SVC_MSG = response["SVC_MSG"].ToString();

                                if (SVC_RT == "0000")
                                {
                                    SmsResult = "성공";
                                    successcount++;
                                }
                                else
                                {
                                    SmsResult = "실패";
                                    failcount++;
                                    _SVC_RT.Add(SVC_RT);
                                }

                            }

                            
                            Data.Connection((_Connection) =>
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText =
                                    "INSERT INTO CallSms (CTime, OriginalPhoneNo, SmsResult, ResultMessage, ClientId,LoginId,CustomerId,DriverId,MSG) VALUES (@CTime, @OriginalPhoneNo, @SmsResult, @ResultMessage, @ClientId,@LoginId,@CustomerId,@DriverId,@Msg)";

                                    _Command.Parameters.AddWithValue("@CTime", DateTime.Now);
                                    _Command.Parameters.AddWithValue("@OriginalPhoneNo", item.MobileNo.Replace("-", ""));
                                    _Command.Parameters.AddWithValue("@SmsResult", SmsResult);
                                    _Command.Parameters.AddWithValue("@ResultMessage", SVC_MSG);
                                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                    _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);
                                    _Command.Parameters.AddWithValue("@CustomerId", 0);
                                    _Command.Parameters.AddWithValue("@DriverId", item.DriverId);
                                    _Command.Parameters.AddWithValue("@Msg", SMSText);
                                    _Command.ExecuteNonQuery();
                                    //CallId = Convert.ToInt32(_Command.ExecuteScalar());
                                }
                            });

                        }
                    }
                    try {
                        
                       

                            if (_SVC_RT.Contains("1004"))
                            {
                                MessageBox.Show("전화번호(LG U+)로 로그인에 실패 했습니다.\r\n\r\n“통화내역” 메뉴에서 “전화번호 설정”을 클릭,\r\n“비밀번호 초기화”를 한 후에\r\n다시 사용하시면 됩니다.", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            }

                             MessageBox.Show("SMS 전송 \r\n\r\n성공 : '" + successcount + "'건\r\n실패 : '" + failcount + "'건", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


                    }
                    catch { MessageBox.Show("연결된 인터넷전화(LG U+)가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
                    
                    
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

        public static string EncryptSHA512(string Data)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        private void txt_DTGPrice_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_DTGPrice_Leave(object sender, EventArgs e)
        {
            DTGPrice = int.Parse(txt_DTGPrice.Text);
            AccountPrice = int.Parse(txt_AccountPrice.Text);

            ServicePrice = AccountPrice + DTGPrice;


            //if (ServicePrice >= 18000)
            //{
            //    ServicePrice = 17000;
            //}

            if (chk_useTax.Checked == true)
            {
                txt_ServicePrice.Text = ((ServicePrice * 0.1) + ServicePrice).ToString();
            }
            else
            {
                txt_ServicePrice.Text = (ServicePrice).ToString();
            }
        }

        private void txt_AccountPrice_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_AccountPrice_Leave(object sender, EventArgs e)
        {
            DTGPrice = int.Parse(txt_DTGPrice.Text);
            AccountPrice = int.Parse(txt_AccountPrice.Text);

            ServicePrice = AccountPrice + DTGPrice;


            //if (ServicePrice >= 18000)
            //{
            //    ServicePrice = 17000;
            //}

            if (chk_useTax.Checked == true)
            {
                txt_ServicePrice.Text = ((ServicePrice * 0.1) + ServicePrice).ToString();
            }
            else
            {
                txt_ServicePrice.Text = (ServicePrice).ToString();
            }
        }

        private void chk_OTGUSe_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_OTGUSe.Checked == true)
            {
                txt_OTGPrice.Text = "15000";
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab == tabPagePoint)
            {
                if(!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("주선사에서는\r\n조회하실 수 없습니다.", "차량관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    
                  
                    return;
                }
                baseDataSet.DriverPoints.Clear();
                //Load DriverPoints
                if (driversBindingSource.Current as DataRowView == null)
                    return;
                var _Selected = (driversBindingSource.Current as DataRowView).Row as BaseDataSet.DriversRow;
                if (_Selected == null)
                    return;
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText =
                          //@"select * from DriverPoints where DriverId = @DriverId
                          //union
                          //select 0, AcceptTime, -1 * DriverPoint, OrderId, DriverId, ClientId, '',0,''
                          //from Orders
                          //where DriverPoint IS NOT NULL and DriverId = @DriverId
                          //order by CDate DESC";
                          @"select * from DriverPoints where DriverId = @DriverId
                       
                        order by CDate DESC";

                    _Command.Parameters.AddWithValue("@DriverId", _Selected.DriverId);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        baseDataSet.DriverPoints.Load(_Reader);
                    }
                }
                txt_Point.Text = baseDataSet.DriverPoints.Sum(c => c.Amount).ToString("N0");
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
                    if(_S.Length == 8)
                    {
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
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
                        if (_S.Length == 12)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 3) + "-" + _S.Substring(6, 4);
                        }
                        else if (_S.Length == 13)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                        }
                        else if (_S.Length > 13)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4) + "-" + _S.Substring(8, 4);
                        }
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
            if (driversBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null || !Selected.HasBizPaper || !Selected.HasCarPaper)
                return;
            FormImages2 f = new FormImages2(Selected);
            f.Show();
        }

        private void btn_ServiceState_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말 심사 의뢰하시겠습니까?", "차량관리", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)DataListDriverPoint.Rows[e.RowIndex].DataBoundItem).Row as BaseDataSet.DriverPointsRow;

           // InitAccLogTable();
            var Q = _AccLogTable.Where(c => c.LGD_TID == Selected.LGD_Result && c.LGD_RESPCODE == "0000" && c.LGD_MID.Contains("chasero")).ToArray();
            if (e.ColumnIndex == driverPointIdDataGridViewTextBoxColumn.Index)
                e.Value = e.RowIndex + 1;
            else if(e.ColumnIndex == ColumnSum.Index)
            {
                e.Value = baseDataSet.DriverPoints.Skip(e.RowIndex).Sum(c => c.Amount);
            }
            else if (e.ColumnIndex == cDateDataGridViewTextBoxColumn.Index)
            {
                //e.Value =
            }

            

            //충전수단
             if (e.ColumnIndex == ColumnCardGubun.Index)
            { 
                if(Q.Any())
                {
                    e.Value = "카드";
                }

               
            }

            //유형
             if (e.ColumnIndex == ColumnLGDFunction.Index)
            {
                if (Q.Any())
                {
                    if (Selected.PointItem.Trim() != "취소")
                    {
                        e.Value = "승인";
                    }
                    else
                    {
                        e.Value = "취소";
                    }
                    
                }


            }
            //거래번호
             if (e.ColumnIndex == ColumnLGDTID.Index)
            {
                if (Q.Any())
                {
                    e.Value = Q.First().LGD_TID;

                }


            }

            //카드번호
            if (e.ColumnIndex == ColumnCardNo.Index)
            {
                if (Q.Any())
                {
                    e.Value = Q.First().CardNo.Substring(0, Q.First().CardNo.Length - 4) + "****";

                }


            }
            //코드
            if (e.ColumnIndex == ColumnLGDRESPCODE.Index)
            {
                if (Q.Any())
                {
                    e.Value = Q.First().LGD_RESPCODE;

                }


            }

            //코드
            if (e.ColumnIndex == ColumnLGD_RESPMSG.Index)
            {
                if (Q.Any())
                {
                    if (Selected.PointItem.Trim() != "취소")
                    {
                        var m = Q.Where(c => c.AccFunction != "취소");
                        e.Value = m.First().LGD_RESPMSG;
                    }
                    else
                    {
                        var m = Q.Where(c => c.AccFunction == "취소");
                        e.Value = m.First().LGD_RESPMSG;
                        if(m.First().LGD_RESPMSG == "취소성공")
                        {
                            e.Value = "취소";
                        }

                    }

                }


            }
        }

        private void btn_ChangePoint_Click(object sender, EventArgs e)
        {
            if (driversBindingSource.Current == null)
                return;
            if (((DataRowView)driversBindingSource.Current).Row is BaseDataSet.DriversRow Selected)
            {
                var Current = baseDataSet.DriverPoints.Sum(c => c.Amount);
                if (!decimal.TryParse(txt_Point.Text.Replace(",", ""), out decimal Changed))
                {
                    txt_Point.Text = Current.ToString("N0");
                    return;
                }
                if (Current != Changed)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "INSERT INTO DriverPoints (CDate, Amount, OrderId, DriverId, Remark,PointItem) VALUES (GETDATE(), @Amount, 0, @DriverId, @Remark,@PointItem)";
                        _Command.Parameters.AddWithValue("@Amount", Changed - Current);
                        _Command.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                        _Command.Parameters.AddWithValue("@PointItem", "수정");
                        if (Changed - Current > 0)
                            _Command.Parameters.AddWithValue("@Remark", String.Format("임의변경: {0:N0}원 추가", Changed - Current));
                        else
                            _Command.Parameters.AddWithValue("@Remark", String.Format("임의변경: {0:N0}원 감소", Current - Changed));

                        _Command.ExecuteNonQuery();
                        tabControl1_SelectedIndexChanged(null, null);
                    }
                }
            }
        }

        private void txt_Point_Enter(object sender, EventArgs e)
        {
            txt_Point.Text = txt_Point.Text.Replace(",", "");
        }

        private void txt_Point_Leave(object sender, EventArgs e)
        {
            decimal _Point = 0;
            if(decimal.TryParse(txt_Point.Text, out _Point))
            {
                txt_Point.Text = _Point.ToString("N0");
            }
            else
            {
                txt_Point.Clear();
            }
        }

        private void btn_CarRegAdd_Click(object sender, EventArgs e)
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

                var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetCarReg"));
                // var http = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/ImageFromApp/SetCarReg"));
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
            pic_CarReg.Visible = true;
            DriverId.HasCarReg = true;
            txt_CarReg.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
            MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_CarRegDelete_Click(object sender, EventArgs e)
        {
            BaseDataSet.DriversRow DriverId = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (DriverId == null)
                return;
            if (MessageBox.Show("정말 이미지를 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                string sqlString = "UPDATE Drivers Set HasCarReg = 0 WHERE DriverId = @DriverId";
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
            txt_CarReg.Text = "";
            pic_CarReg.Visible = false;
            DriverId.HasCarReg = false;
        }

        private void btn_ShowCarReg_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null || !Selected.HasCarReg)
                return;
            Bitmap _b = null;
            PrintDocument pDoc = new PrintDocument();
            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(10, 10, 10, 10);
            pDoc.DefaultPageSettings = ps;
            PrintPreviewDialog ppDoc = new PrintPreviewDialog();

            ppDoc.ClientSize = new System.Drawing.Size(500, 500);

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
                    var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarReg?DriverId=" + Selected.DriverId.ToString());
                    //   var b = mWebClient.DownloadData("http://localhost/ImageFromAdmin/GetCarReg?DriverId=" + Selected.DriverId.ToString());
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
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == '\u0003') )
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
            //if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == Convert.ToChar(Keys.Back)))
            //{
            //    e.Handled = true;
            //}
        }

        private void driverPointsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            if (driverPointsBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)driverPointsBindingSource.Current).Row as BaseDataSet.DriverPointsRow;

            if (Selected.CDate.Date != DateTime.Now.Date)
            {
                button1.Enabled = false;
            }
            if (Selected.PointItem =="취소" || String.IsNullOrEmpty(Selected.LGD_OID) || String.IsNullOrEmpty(Selected.LGD_Result))
            {
                button1.Enabled = false;
            }
            var Query = baseDataSet.DriverPoints.Where(c => c.LGD_OID == Selected.LGD_OID).ToArray();

            if (Query.Count() > 1)
            {
                button1.Enabled = false;
            }
       
        }

        private void txtMisu_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtMizi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372110162");
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as BaseDataSet.DriversRow;
            if (Selected == null)
                return;

           // var _Row = baseDataSet.Drivers[e.RowIndex];

            if (String.IsNullOrEmpty(Selected.BizNo) || Selected.BizNo.Substring(0, 3) == "000" || Selected.BizNo.Substring(0, 3) == "999" || String.IsNullOrEmpty(Selected.Name) || Selected.Name == "." || String.IsNullOrEmpty(Selected.PayBankName) || String.IsNullOrEmpty(Selected.PayAccountNo) || String.IsNullOrEmpty(Selected.PayInputName))
            {

                grid1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;

            }
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,
               checkbox
               , rowNUMDataGridViewTextBoxColumn
               ,ClientCode
               ,ClientName
               ,SubClientId
               ,codeDataGridViewTextBoxColumn
              // ,Gubun
               ,nameDataGridViewTextBoxColumn
               ,bizNoDataGridViewTextBoxColumn
               ,cEODataGridViewTextBoxColumn
               ,AppUse
               ,carYearDataGridViewTextBoxColumn
               ,Group
               ,ServiceState
               ,FileBiz
               ,loginIdDataGridViewTextBoxColumn
               ,passwordDataGridViewTextBoxColumn
               ,mobileNoDataGridViewTextBoxColumn
               ,carTypeDataGridViewTextBoxColumn
               ,carSizeDataGridViewTextBoxColumn
               ,CarInfo
               ,carNoDataGridViewTextBoxColumn
              // ,Column1
               //,RouteType
               ,UsePayNow
               ,VAccount
               ,createDateDataGridViewTextBoxColumn




                );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (driverPointsBindingSource.Current == null)
                return;

         

            var Selected = ((DataRowView)driverPointsBindingSource.Current).Row as BaseDataSet.DriverPointsRow;
            DriverRepository mDriverRepository = new DriverRepository();
            var _Driver = mDriverRepository.GetDriver(Selected.DriverId);
           
            if (MessageBox.Show($"차량번호 : {_Driver.CarNo}\r\n차주성명 : {_Driver.CarYear}\r\n카드취소 : {Selected.Amount.ToString("N0")}원\r\n위 금액을 카드취소 하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            if (Selected.CDate.Date != DateTime.Now.Date)
            {
                MessageBox.Show("당일 충전건만 취소 가능합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (Selected.PointItem == "취소" || String.IsNullOrEmpty(Selected.LGD_OID) || String.IsNullOrEmpty(Selected.LGD_Result))
            {
                MessageBox.Show("카드 충전건만 취소 가능합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            WebClient mWebClient = new WebClient();
            string Parameter = "?sPrameter=" + String.Join("^", new object[] { Selected.DriverPointId });
            mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted2;
            mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/DriverCardCancel" + Parameter));
            //mWebClient.DownloadStringAsync(new Uri("http://localhost/Pay/DriverCardCancel" + Parameter));
        }

        private void btn_CarEtcAdd_Click(object sender, EventArgs e)
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

                var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetCarEtc"));
                //var http = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/ImageFromApp/SetCarEtc"));
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
            pic_CarEtc.Visible = true;
            DriverId.HasCarReg = true;
            txt_CarEtc.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
            MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_CarEtcDelete_Click(object sender, EventArgs e)
        {
            BaseDataSet.DriversRow DriverId = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (DriverId == null)
                return;
            if (MessageBox.Show("정말 이미지를 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                string sqlString = "UPDATE Drivers Set HasCarEtc = 0 WHERE DriverId = @DriverId";
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
            txt_CarEtc.Text = "";
            pic_CarEtc.Visible = false;
            DriverId.HasCarEtc = false;
        }

        private void btn_ShowCarEtc_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null || !Selected.HasCarReg)
                return;
            Bitmap _b = null;
            PrintDocument pDoc = new PrintDocument();
            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(10, 10, 10, 10);
            pDoc.DefaultPageSettings = ps;
            PrintPreviewDialog ppDoc = new PrintPreviewDialog();

            ppDoc.ClientSize = new System.Drawing.Size(500, 500);

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
                    var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarEtc?DriverId=" + Selected.DriverId.ToString());
                    //var b = mWebClient.DownloadData("http://localhost/ImageFromAdmin/GetCarEtc?DriverId=" + Selected.DriverId.ToString());
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
        public void Button_MouseMove(object sender, MouseEventArgs e)
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

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }

        private void pic_CarEtc_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected == null || !Selected.HasCarReg)
                return;
            Bitmap _b = null;
            PrintDocument pDoc = new PrintDocument();
            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(10, 10, 10, 10);
            pDoc.DefaultPageSettings = ps;
            PrintPreviewDialog ppDoc = new PrintPreviewDialog();

            ppDoc.ClientSize = new System.Drawing.Size(500, 500);

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
                    var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarEtc?DriverId=" + Selected.DriverId.ToString());
                    //var b = mWebClient.DownloadData("http://localhost/ImageFromAdmin/GetCarEtc?DriverId=" + Selected.DriverId.ToString());
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

        private void button3_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);


            _FormStyle.WriteFormStyle(this, grid1);

            MessageBox.Show("저장하였습니다.");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            lblInfo.Text = string.Empty;
            if(String.IsNullOrEmpty(txt_BizNo.Text))
            {
                return;
            }
            var aa = CRNRequest.call(txt_BizNo.Text.Replace("-", ""));

            lblInfo.Text = aa;
        }
    }

    public class AndroidImageViewModel
    {
        public int DriverId { get; set; }
        public String ImageData64String { get; set; }
    }


}
