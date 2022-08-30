using Microsoft.Reporting.WinForms;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.XML;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Popbill;
using Popbill.Taxinvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0212_SALESMANAGE : Form
    {
        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
            auth = this.GetAuth();

            if (!LocalUser.Instance.LogInInformation.IsClient)
            {
                auth = MenuAuth.Read;

            }


            switch (auth)
            {
                case MenuAuth.None:
                    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                    return;
                case MenuAuth.Read:

                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                    btn_New2.Enabled = false;

                    grid1.ReadOnly = true;
                    btn_Tax.Enabled = false;
                    button2.Enabled = false;

                    button4.Enabled = false;
                    button1.Enabled = false;

                    //grid2.ReadOnly = true;
                    break;
            }


        }


       // List<AddressViewModel> AddressList = new List<AddressViewModel>();
        int GridIndex = 0;
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;

        private const string CRLF = "\r\n";

        string TinfoNtsresult = "";
        string TinfoWriteDate = "";
        string TinfoitemKey = "";
        string TinfostateCode = "";
        string TinfostateName = "";

        int Error = 0;
        int Success = 0;
        bool HideAddSales = false;
        int _StatsGubun = 0;

        #region 전자세금계산서NiceDNR
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
        string linkId = "edubillsys";
        #endregion
        DateTimePicker dtp = new DateTimePicker();
        ContextMenuStrip m = new ContextMenuStrip();


        public FrmMN0212_SALESMANAGE()
        {
            //foreach (var item in SingleDataSet.Instance.AddressReferences)
            //{
            //    if (String.IsNullOrEmpty(item.State) || String.IsNullOrEmpty(item.City) || String.IsNullOrEmpty(item.Street))
            //        continue;
            //    if (!AddressList.Any(c => c.State == item.State && c.City == item.City && c.Street == item.Street))
            //    {
            //        AddressList.Add(new AddressViewModel
            //        {
            //            State = item.State,
            //            City = item.City,
            //            Street = item.Street,
            //        });
            //    }
            //}


            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
               
                ApplyAuth();
            }
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);



            dtp_PayDate.Visible = true;
            label5.Visible = true;
            if (LocalUser.Instance.LogInInformation.ClientId != 290)
            {
                EndDay.Visible = false;
            }

            //InitClientTable();

            LocalUser.Instance.LogInInformation.LoadClient();
            _StatsGubun = LocalUser.Instance.LogInInformation.Client.StatsGubun;


        }



        private void FrmMN0212_SALESMANAGE_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'salesDataSet.SalesManage' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.salesManageTableAdapter.Fill(this.salesDataSet.SalesManage);
            if (LocalUser.Instance.LogInInformation.IsAdmin)
                clientsTableAdapter.Fill(this.cMDataSet.Clients);
            else
                clientsTableAdapter.FillByClientId(cMDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);
            customersTableAdapter.Fill(this.cMDataSet.Customers);

            AdminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
            _InitCmb();

            SetMouseClick(this);

            cmb_Date.SelectedIndex = 0;
            //dtp_Sdate.Value = DateTime.Now.AddMonths(-2);
            //dtp_Edate.Value = DateTime.Now;
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
            cmb_Etax.SelectedIndex = 0;
            cmb_PayState.SelectedIndex = 0;
            cmb_Date.SelectedIndex = 0;

            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;


            LocalUser.Instance.LogInInformation.LoadClient();
            //HideAddSales = LocalUser.Instance.LogInInformation.Client.HideAddSales;

            //if (HideAddSales)
            //{
            //    // btn_New.Enabled = true;
            //    // btn_New2.Enabled = true;

            //}
            //else
            //{
            //    // btn_New.Enabled = false;
            //    // btn_New2.Enabled = false;
            //}

            //dataGridView1.Controls.Add(dtp);
            //dtp.Format = DateTimePickerFormat.Custom;
            //dtp.Visible = false;

            btn_Search_Click(null, null);
        }


        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();
        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }

            public string CMSOwner { get; set; }
            public string CMSBankName { get; set; }
            public string CMSBankCode { get; set; }
            public string CMSAccountNo { get; set; }


        }


        private void InitClientTable()
        {
            _ClientTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = " SELECT ClientId, Code, Name, LoginId, isnull(PhoneNo, ''), isnull(MobileNo, '')" +
                        ", ISNULL(CMSOwner, '')as CMSOwner, ISNULL(CMSBankName, '') as CMSBankName, ISNULL(CMSBankCode, '') as CMSBankCode, ISNULL(CMSAccountNo, '') as CMSAccountNo " +
                        " FROM Clients " +
                        " WHERE ISNULL(CMSOwner,'') != ''  and ISNULL(CMSBankCode,'') != '' and ISNULL(CMSAccountNo,'') != ''" +
                        "  UNION " +
                        " SELECT ClientId, Code, Name, LoginId, isnull(PhoneNo, ''),isnull(MobileNo, '') " +
                        " ,ISNULL(CMSOwner1, ''), ISNULL(CMSBankName1, ''), ISNULL(CMSBankCode1, ''), ISNULL(CMSAccountNo1, '')" +
                        " FROM Clients" +
                        " WHERE ISNULL(CMSOwner1,'') != ''  and ISNULL(CMSBankCode1,'') != '' and ISNULL(CMSAccountNo1,'') != ''" +
                        "  UNION " +
                        " SELECT ClientId, Code, Name, LoginId, isnull(PhoneNo, ''),isnull(MobileNo, '')" +
                        " ,ISNULL(CMSOwner2, ''), ISNULL(CMSBankName2, ''), ISNULL(CMSBankCode2, ''), ISNULL(CMSAccountNo2, '')" +
                        " FROM Clients " +
                        " WHERE ISNULL(CMSOwner2,'') != ''  and ISNULL(CMSBankCode2,'') != '' and ISNULL(CMSAccountNo2,'') != '' " +
                        " UNION " +
                        " SELECT ClientId, Code, Name, LoginId, isnull(PhoneNo, ''),isnull(MobileNo, '')" +
                        " ,ISNULL(CMSOwner3, ''), ISNULL(CMSBankName3, ''), ISNULL(CMSBankCode3, ''), ISNULL(CMSAccountNo3, '')" +
                         " FROM Clients" +
                         " WHERE ISNULL(CMSOwner3,'') != ''  and ISNULL(CMSBankCode3,'') != '' and ISNULL(CMSAccountNo3,'') != ''" +
                           " UNION" +
                          " SELECT ClientId, Code, Name, LoginId, isnull(PhoneNo, ''),isnull(MobileNo, '')" +
                      " ,ISNULL(CMSOwner4, ''), ISNULL(CMSBankName4, ''), ISNULL(CMSBankCode4, ''), ISNULL(CMSAccountNo4, '')" +
                           " FROM Clients" +
                        " WHERE ISNULL(CMSOwner4,'') != ''  and ISNULL(CMSBankCode4,'') != '' and ISNULL(CMSAccountNo4,'') != ''";




                    //commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientTable.Add(
                              new ClientViewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                                  PhoneNo = dataReader.GetString(4),
                                  MobileNo = dataReader.GetString(5),

                                  CMSOwner = dataReader.GetString(6),
                                  CMSBankName = dataReader.GetString(7),
                                  CMSBankCode = dataReader.GetString(8),
                                  CMSAccountNo = dataReader.GetString(9),


                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        private void _InitCmb()
        {
            Dictionary<bool, string> HasACCList = new Dictionary<bool, string>();
            HasACCList.Add(false, "현금");
            HasACCList.Add(true, "카드");
            cmb_HasAcc.DataSource = new BindingSource(HasACCList, null);
            cmb_HasAcc.DisplayMember = "Value";
            cmb_HasAcc.ValueMember = "Key";

            Dictionary<string, string> Smonth = new Dictionary<string, string>
            {

                { "당월", "당월" },
                { "전월", "전월" },
                { "지정", "지정" }
            };

            cmbSMonth.DataSource = new BindingSource(Smonth, null);
            cmbSMonth.DisplayMember = "Value";
            cmbSMonth.ValueMember = "Key";

            cmbSMonth.SelectedIndex = 0;
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0212_SALESMANAGE_ADD _Form = new FrmMN0212_SALESMANAGE_ADD();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();


            if (txt_SangHo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_SangHo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }



            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                return;
            }

            if (txt_CEO.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }


            if (txt_Uptae.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Upjong.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_AddrDetail.Text == "" || txt_Zip.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AddrDetail, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Email.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Email, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            //if (txt_ContRactName.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_ContRactName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return;
            //}


            if (txt_MobileNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }


            if (txt_Item.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }


            if (txt_UnitPrice.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_UnitPrice, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }


            if (txt_Num.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Num, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            UpdateDB();
        }
        private void UpdateDB()
        {
            try
            {
                salesManageBindingSource.EndEdit();
                var Row = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;

                Row.AddressState = txt_State.Text;
                Row.AddressCity = txt_City.Text;
                Row.ZipCode = txt_Zip.Text;
                //if (chk_PayState.Checked)
                //{
                //    Row.PayState = 1;
                //    Row.PayDate = dtp_PayDate.Value;
                //}
                //else
                //{
                //    Row.PayState = 2;
                //}

                Row.UnitPrice = Int64.Parse(txt_UnitPrice.Text.Replace(",", ""));
                Row.Num = Int64.Parse(txt_Num.Text.Replace(",", ""));
                Row.Price = Int64.Parse(txt_Price.Text.Replace(",", ""));
                Row.Vat = Int64.Parse(txt_VAT.Text.Replace(",", ""));
                Row.Amount = Int64.Parse(txt_Amount.Text.Replace(",", ""));

                if (rdb_Tax2.Checked)
                {
                    Row.UseTax = false;
                    Row.Taxgubun = 2;
                }
                else
                {
                    if (rdb_Tax0.Checked)
                    {
                        Row.Taxgubun = 0;
                    }
                    else
                    {
                        Row.Taxgubun = 1;
                    }
                    Row.UseTax = true;
                }

                if (Row.Issue == true)
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {

                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        cmd.CommandText =
                           "UPDATE SalesManage SET RequestDate = @RequestDate,Email = @Email  WHERE    SalesId =" + Row.SalesId + " ";
                        cmd.Parameters.AddWithValue("@RequestDate", dtp_RequestDate.Value);
                        cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                        cmd.ExecuteNonQuery();

                        //if (chk_PayState.Checked)
                        //{
                        //    cmd.Parameters.AddWithValue("@PayState", 1);
                        //    cmd.Parameters.AddWithValue("@PayDate", dtp_PayDate.Value);
                        //}
                        //else
                        //{
                        //    cmd.Parameters.AddWithValue("@PayDate", DBNull.Value);
                        //    cmd.Parameters.AddWithValue("@PayState", 2);
                        //}
                        //cmd.ExecuteNonQuery();
                        //if (chk_PayState.Checked)
                        //{
                        //    using (SqlCommand OrderCommand = cn.CreateCommand())
                        //    {
                        //        OrderCommand.CommandText =
                        //            @"UPDATE Orders
                        //                SET CustomerPay = null
                        //                , CustomerPayDate = CONVERT(NVARCHAR(10),GETDATE(),126)
                        //                , CustomerPayPrice = @CustomerPayPrice
                        //                , CustomerPayVAT = @CustomerPayVAT
                        //                WHERE SalesManageId = @SalesManageId";
                        //        OrderCommand.Parameters.AddWithValue("@CustomerPayPrice", Row.Price);
                        //        OrderCommand.Parameters.AddWithValue("@CustomerPayVAT", Row.Vat);
                        //        OrderCommand.Parameters.AddWithValue("@SalesManageId", Row.SalesId);
                        //        OrderCommand.ExecuteNonQuery();
                        //    }
                        //}
                        //else
                        //{
                        //    using (SqlCommand OrderCommand = cn.CreateCommand())
                        //    {
                        //        OrderCommand.CommandText =
                        //            @"UPDATE Orders
                        //                SET CustomerPay = null
                        //                , CustomerPayDate = null
                        //                , CustomerPayPrice = null
                        //                , CustomerPayVAT = null
                        //                WHERE SalesManageId = @SalesManageId";
                        //        OrderCommand.Parameters.AddWithValue("@SalesManageId", Row.SalesId);
                        //        OrderCommand.ExecuteNonQuery();
                        //    }
                        //}
                        cn.Close();
                    }
                }
                else
                {
                    salesManageTableAdapter.Update(Row);
                }
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "매출관리", 1), "매출관리 수정 성공");

                if (grid1.RowCount > 1)
                {
                    GridIndex = salesManageBindingSource.Position;
                    btn_Search_Click(null, null);
                    grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];


                }
                else
                {
                    btn_Search_Click(null, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "매출관리 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            //List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            //if (dataGridView1.SelectedCells.Count > 0)
            //{
            //    foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            //    {
            //        if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
            //    }
            //    int Customercount = 0;




            //    if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "매출관리", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            //    foreach (DataGridViewRow row in deleteRows)
            //    {
            //        dataGridView1.Rows.Remove(row);
            //    }
            //}
            //Up_Status = "Delete";
            //int _rows = UpdateDB(Up_Status);

            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (grid1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in grid1.SelectedCells)
                {

                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }

                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "매출관리", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                //foreach (DataGridViewRow row in deleteRows)
                //{

                //    dataGridView1.Rows.Remove(row);
                //}


                salesManageBindingSource.EndEdit();

                if (salesManageBindingSource.Current != null)
                {
                    var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;



                    if (Selected != null)
                    {

                        salesManageBindingSource.EndEdit();
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                "Delete SalesManage  WHERE SalesId = @SalesId";

                            cmd.CommandText +=
                              " Delete SalesManageDetail  WHERE SalesId = @SalesId";

                            cmd.Parameters.AddWithValue("@SalesId", Selected.SalesId);
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                        try
                        {
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                SqlCommand cmd = cn.CreateCommand();
                                cmd.CommandText =
                                    "Update Orders SET SalesManageId = NULL WHERE SalesManageId = @SalesManageId";

                                cmd.Parameters.AddWithValue("@SalesManageId", Selected.SalesId);
                                cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                        }
                        catch { }
                    }
                }

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "매출관리", 1), "매출관리 삭제 성공");

                btn_Search_Click(null, null);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        //private void LoadTable()
        //{
        //    if (cmb_Date.SelectedIndex == 0)
        //    {
        //        this.salesManageTableAdapter.FillByCreateDate(this.salesDataSet.SalesManage, LocalUser.Instance.LogInInformation.ClientId, dtp_Sdate.Value.Date, dtp_Edate.Value.Date.AddDays(1));
        //    }
        //    else if (cmb_Date.SelectedIndex == 1)
        //    {
        //        this.salesManageTableAdapter.FillByPayDate(this.salesDataSet.SalesManage, LocalUser.Instance.LogInInformation.ClientId, dtp_Sdate.Value.Date, dtp_Edate.Value.Date.AddDays(1));
        //    }
        //}
        private String GetSelectCommand()
        {

            return @"SELECT AddressCity, AddressDetail, AddressState, Amount, BeginDate, BizNo, REPLACE(BizNo, '-', '') AS BizNo1, CONVERT (varchar, CreateDate, 111) AS CREATE_DATE1, CardPayGubun, Ceo, ClientId, ContRactName, CreateDate, CustomerId, Email, EndDate, HasACC, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, Issue, IssueDate, CONVERT (varchar, IssueDate, 111) AS IssueDate1, IssueState, Item, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Date, LGD_Last_Function, LGD_OID, LGD_Result, MobileNo, Num, PayAccountNo, PayBankCode, PayBankName, PayDate, CONVERT (varchar, PayDate, 111) AS PayDate1, PayInputName, PayState, Price, RequestDate, SalesId, SangHo, SetYN, SourceType, SubClientId, UnitPrice, Upjong, Uptae, UseTax, Vat, ZipCode, invoicerMgtKey,IssueLoginId, IssueGubun , CMSOwner, CMSBankName, CMSAccountNo,EmailCount,TaxGubun,billNo,purposeType,TypeCode FROM SalesManage ";


        }


        private void _Search()
        {
            checkedCodes.Clear();
            salesDataSet.SalesManage.Clear();
            // LoadTable();
            //string _FilterString = string.Empty;
            //string _cmbSearchString = string.Empty;
            //string _DateSearchString = string.Empty;
            //string _StatusSearchString = string.Empty;
            //string _EtaxSearch = string.Empty;
            //string _PayStateSearch = string.Empty;
            //string _EtaxCancle = string.Empty;


            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

                    // 2. 조회 기간
                    // String _DateColumn = "";// = cmb_Date.SelectedIndex == 0 ? " CreateDate" : " PayDate";
                    switch (cmb_Date.SelectedIndex)
                    {
                        case 0:

                            WhereStringList.Add("RequestDate >= @Begin AND RequestDate < @End ");
                            break;
                        case 1:
                            //    _DateColumn = "PayDate";
                            WhereStringList.Add("PayDate >= @Begin AND PayDate < @End ");
                            WhereStringList.Add("PayState  = 1 ");
                            break;


                        case 2:

                            WhereStringList.Add("IssueDate >= @Begin AND IssueDate < @End ");
                            WhereStringList.Add("Issue  = 1 ");
                            break;


                    }


                    _Command.Parameters.AddWithValue("@Begin", dtp_Sdate.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtp_Edate.Value.Date.AddDays(1));
                    // 
                    if (cmb_Search.Text == "사업자번호")
                    {
                        WhereStringList.Add(string.Format("REPLACE(BizNo, '-', '') Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "상호")
                    {
                        //string CustomerIdIN = string.Empty;
                        //var _CustomerId = cMDataSet.Customers.Where(c => c.SangHo.Contains(txt_Search.Text) && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => c.CustomerId).ToArray();

                        //foreach (var code in _CustomerId)
                        //{
                        //    CustomerIdIN += code + ",";
                        //}
                        //CustomerIdIN = CustomerIdIN.Substring(0, CustomerIdIN.Length - 1);

                        //WhereStringList.Add(string.Format("CustomerId IN ({0})", CustomerIdIN));
                        
                        WhereStringList.Add(string.Format("SangHo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "품목명")
                    {
                        WhereStringList.Add(string.Format("Item Like  '%{0}%'", txt_Search.Text));
                    }


                    if (cmb_Etax.SelectedIndex == 1)
                    {
                        WhereStringList.Add(String.Format(" ISNULL(IssueDate,'9999-12-31 00:00:00.000') <>'{0}' ", "9999-12-31 00:00:00.000"));

                    }
                    else if (cmb_Etax.SelectedIndex == 2)
                    {
                        WhereStringList.Add(String.Format(" ISNULL(IssueDate,'9999-12-31 00:00:00.000') = '{0}' ", "9999-12-31 00:00:00.000"));
                    }

                    if (cmb_PayState.SelectedIndex == 1)
                    {
                        WhereStringList.Add(String.Format(" PayState = 1"));
                    }
                    else if (cmb_PayState.SelectedIndex == 2)
                    {
                        WhereStringList.Add(String.Format(" PayState = 2"));
                    }

                    if (!chk_EtaxCancle.Checked)
                    {
                        WhereStringList.Add(String.Format(" (IssueState <> '취소' OR IssueState IS NULL) "));
                    }
                    WhereStringList.Add(String.Format(" ClientId = {0}", LocalUser.Instance.LogInInformation.ClientId));


                    if(!LocalUser.Instance.LogInInformation.IsClient)
                    {
                        WhereStringList.Add(String.Format(" CustomerId = {0} ", LocalUser.Instance.LogInInformation.CustomerId));

                    }

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }

                    if (cmb_Date.SelectedIndex == 0)
                    {
                        SelectCommandText += "Order by RequestDate Desc ";
                    }
                    else if (cmb_Date.SelectedIndex == 1)
                    {
                        SelectCommandText += "Order by PayDate Desc ";

                    }
                    else
                    {
                        SelectCommandText += "Order by IssueDate Desc ";

                    }
                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        salesDataSet.SalesManage.Load(_Reader);


                    }
                }
                _Connection.Close();
            }

            var Query = salesDataSet.SalesManage.ToArray();

            if (Query.Any())
            {

                lblCount.Text = Query.Count().ToString("N0");
                lblPrice.Text = Query.Sum(c => c.Price).ToString("N0");
                lblVat.Text = Query.Sum(c => c.Vat).ToString("N0");
                //lblAmount.Text = Query.Sum(c => c.Amount).ToString("N0");

                decimal _Sugun = Query.Sum(c => c.InputPrice1) + Query.Sum(c => c.InputPrice2) + Query.Sum(c => c.InputPrice3);
                lblSugun.Text = _Sugun.ToString("N0");

                decimal _Misu = Query.Sum(c => c.Amount) - _Sugun;

                lblMisu.Text = _Misu.ToString("N0");

                lblAmount.Text = Query.Sum(c => c.Amount).ToString("N0");

            }
            else
            {
                lblCount.Text = "0";
                lblPrice.Text = "0";
                lblVat.Text = "0";
                lblAmount.Text = "0";
                lblSugun.Text = "0";
                lblMisu.Text = "0";

            }
          
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void cmb_AddressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmb_AddressState.SelectedValue == null)
            //    return;


            //cmb_AddressCity.Enabled = true;


            //var ParkCityDataSource = AddressList.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            //cmb_AddressCity.DataSource = ParkCityDataSource;
            //cmb_AddressCity.DisplayMember = "City";
            //cmb_AddressCity.ValueMember = "City";
        }

        private void btn_CustomerInfo_Click(object sender, EventArgs e)
        {
            if (rdb_Normal.Checked)
            {
                FrmCustomerSearch _frmCustomerSearch = new FrmCustomerSearch("1,3");
                _frmCustomerSearch.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
                {
                    if (ie.KeyCode != Keys.Return) return;
                    if (_frmCustomerSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCustomerSearch.grid1.SelectedCells[0].RowIndex < 0) return;

                    txt_SangHo.Text = _frmCustomerSearch.grid1[0, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCustomerSearch.grid1[1, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCustomerSearch.grid1[2, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCustomerSearch.grid1[3, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCustomerSearch.grid1[4, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_State.Text = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCustomerSearch.grid1[20, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_AddrDetail.Text = _frmCustomerSearch.grid1[7, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCustomerSearch.grid1[8, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCustomerSearch.grid1[9, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCustomerSearch.grid1[16, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CardPayGubun.Text = "N";
                    txt_CustomerId.Text = _frmCustomerSearch.grid1[12, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    _frmCustomerSearch.Close();
                });
                _frmCustomerSearch.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
                {
                    if (_frmCustomerSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCustomerSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                    txt_SangHo.Text = _frmCustomerSearch.grid1[0, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCustomerSearch.grid1[1, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCustomerSearch.grid1[2, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCustomerSearch.grid1[3, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCustomerSearch.grid1[4, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_State.Text = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCustomerSearch.grid1[20, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();


                    txt_AddrDetail.Text = _frmCustomerSearch.grid1[7, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCustomerSearch.grid1[8, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCustomerSearch.grid1[9, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCustomerSearch.grid1[16, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CardPayGubun.Text = "N";
                    txt_CustomerId.Text = _frmCustomerSearch.grid1[12, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    _frmCustomerSearch.Close();
                });






                _frmCustomerSearch.Owner = this;
                _frmCustomerSearch.StartPosition = FormStartPosition.CenterParent;
                _frmCustomerSearch.ShowDialog();
                // txt_AddrDetail.Focus();

            }
            else
            {
                FrmCardSearch _frmCardSearch = new FrmCardSearch();
                _frmCardSearch.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
                {
                    if (ie.KeyCode != Keys.Return) return;
                    if (_frmCardSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCardSearch.grid1.SelectedCells[0].RowIndex < 0) return;

                    txt_SangHo.Text = _frmCardSearch.grid1[0, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCardSearch.grid1[1, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCardSearch.grid1[3, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCardSearch.grid1[4, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_State.Text = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCardSearch.grid1[40, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();


                    txt_AddrDetail.Text = _frmCardSearch.grid1[7, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCardSearch.grid1[8, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCardSearch.grid1[10, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CardPayGubun.Text = "C";
                    txt_CustomerId.Text = _frmCardSearch.grid1[12, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo_Leave(null, null);
                    _frmCardSearch.Close();
                });
                _frmCardSearch.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
                {
                    if (_frmCardSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCardSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                    txt_SangHo.Text = _frmCardSearch.grid1[0, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCardSearch.grid1[1, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCardSearch.grid1[3, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCardSearch.grid1[4, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_State.Text = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCardSearch.grid1[40, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();


                    txt_AddrDetail.Text = _frmCardSearch.grid1[7, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCardSearch.grid1[8, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCardSearch.grid1[10, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CardPayGubun.Text = "C";
                    txt_CustomerId.Text = _frmCardSearch.grid1[12, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo_Leave(null, null);
                    _frmCardSearch.Close();
                });


                _frmCardSearch.Owner = this;
                _frmCardSearch.StartPosition = FormStartPosition.CenterParent;
                _frmCardSearch.ShowDialog();

            }
        }

        String _Sender = "";
        bool MethodProccess = false;
        private void salesManageBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();

            tabControl1.SelectTab(0);
            if (salesManageBindingSource.Current == null)
            {
                txt_UnitPrice.Clear();
                txt_Num.Clear();
                txt_Price.Clear();
                txt_VAT.Clear();
                txt_Amount.Clear();

                txt_Zip.Clear();
                txt_State.Clear();
                txt_City.Clear();
                txt_AddrDetail.Clear();
                return;
            }

            else
            {




                var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
                GridIndex = salesManageBindingSource.Position;
                //if (((DataRowView)salesManageBindingSource.Current).Row is SalesDataSet.SalesManageRow Selected)
                //{
//                salesReportTableAdapter.Fill(this.salesDataSet.SalesReport, Selected.SalesId);



                MethodProccess = true;
                _Sender = "salesManageBindingSource";
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        //_Command.CommandText = "SELECT OrderId, AcceptTime, ISNULL(Drivers.Caryear, N'') AS Customer, drivers.CarNo ,ISNULL(SalesPrice,0) as SalesPrice, Item, StartState, StartCity, StopState, StopCity FROM ORDERS LEFT JOIN drivers ON ORDERS.driverid = Drivers.driverid WHERE SalesManageId = @SalesId";
                        _Command.CommandText = "SELECT OrderId, AcceptTime, ISNULL(Orders.Driver, N'') AS Customer, Orders.DriverCarNo AS CarNo ,ISNULL(SalesPrice,0)+ ISNULL(AlterPrice,0) as SalesPrice, Item, StartState+'/'+ StartCity+'/'+StartStreet as StartName, StopState+'/'+ StopCity+'/'+StopStreet as StopName FROM ORDERS LEFT JOIN drivers ON ORDERS.driverid = Drivers.driverid WHERE SalesManageId = @SalesId Order by AcceptTime ";

                        _Command.Parameters.AddWithValue("@SalesId", Selected.SalesId);
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            var _Table = new DataTable();
                            _Table.Load(_Reader);
                            ordersBindingSource1.DataSource = _Table;
                        }
                    }
                    _Connection.Close();
                }

                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        //_Command.CommandText = "SELECT OrderId, AcceptTime, ISNULL(Drivers.Caryear, N'') AS Customer, drivers.CarNo ,ISNULL(SalesPrice,0) as SalesPrice, Item, StartState, StartCity, StopState, StopCity FROM ORDERS LEFT JOIN drivers ON ORDERS.driverid = Drivers.driverid WHERE SalesManageId = @SalesId";
                        _Command.CommandText = "SELECT c.chasu, c.InputPriceDate,isnull(c.InputPrice,0) as InputPrice ,SalesId  FROM (" +
                            "SELECT a.chasu, InputPriceDate,a.InputPrice1 as InputPrice ,SalesId" +
                            " FROM(" +
                            "   select '1' as chasu, convert(varchar, InputPrice1Date, 111) as InputPriceDate, InputPrice1, SalesId from SalesManage" +
                            "   union" +
                            "   select '2', convert(varchar, InputPrice2Date, 111), InputPrice2, SalesId  from SalesManage" +
                            "   union" +
                            "   select '3', convert(varchar, InputPrice3Date, 111), InputPrice3, SalesId  from SalesManage" +
                            "   ) AS a" +
                            "   WHERE InputPriceDate IS NOT NULL AND InputPrice1 != 0 AND SalesId = @SalesId" +
                            "   UNION select '합계','', SUM(InputPrice1) ,max(SalesId)" +
                            " FROM(" +
                            "   select '1' as chasu, convert(varchar, InputPrice1Date, 111) as InputPriceDate, InputPrice1, SalesId from SalesManage" +
                            "   union" +
                            "   select '2', convert(varchar, InputPrice2Date, 111), InputPrice2, SalesId from SalesManage" +
                            "   union" +
                            "   select '3', convert(varchar, InputPrice3Date, 111), InputPrice3, SalesId from SalesManage" +
                            "     ) AS B" +
                            "   WHERE InputPriceDate IS NOT NULL AND InputPrice1 != 0 AND SalesId = @SalesId" +
                            "   )as C";

                        _Command.Parameters.AddWithValue("@SalesId", Selected.SalesId);
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            var _Table = new DataTable();
                            _Table.Load(_Reader);
                            PaybindingSource.DataSource = _Table;
                        }
                    }
                    _Connection.Close();
                }


                var _Customer = cMDataSet.Customers.FirstOrDefault(c => c.CustomerId == Selected.CustomerId);
                if (Selected.SourceType == 1)
                {

                    if (_Customer == null)
                    {
                        lbl_BizNo.Text = "";
                        lbl_SangHo.Text = "";
                        lbl_RequestDate.Text = "";
                        lbl_Date.Text = "";
                    }
                    else
                    {

                        lbl_BizNo.Text = _Customer.BizNo;
                        lbl_SangHo.Text = _Customer.SangHo;
                        lbl_RequestDate.Text = Selected.RequestDate.ToString("d").Replace("-", "/");
                        lbl_Date.Text = Selected.BeginDate.ToString("d").Replace("-", "/") + " - " + Selected.EndDate.ToString("d").Replace("-", "/");

                    }
                }
                else
                {

                    lbl_BizNo.Text = "";
                    lbl_SangHo.Text = "";
                    lbl_RequestDate.Text = "";
                    lbl_Date.Text = "";
                }




                txt_State.Text = Selected.AddressState;
                txt_City.Text = Selected.AddressCity;
                txt_Zip.Text = Selected.ZipCode;
                //cmb_AddressState.SelectedValue = Selected.AddressState;
                //cmb_AddressCity.SelectedValue = Selected.AddressCity;

                txt_UnitPrice.Text = ((long)Selected.UnitPrice).ToString("N0");
                txt_Num.Text = ((long)Selected.Num).ToString("N0");
                txt_Price.Text = ((long)Selected.Price).ToString("N0");
                if (Selected.UseTax == false)
                {

                    txt_VAT.Text = ((long)Selected.Vat).ToString("N0");
                }
                else
                {
                    txt_VAT.Text = ((long)Selected.Vat).ToString("N0");
                }
                txt_Amount.Text = ((long)Selected.Amount).ToString("N0");

                //수금완료
                if (Selected.PayState == 1)
                {
                    btnPayUpdate.Enabled = false;
                    btnPayCancle.Enabled = true;
                    txt_InputPrice.Enabled = false;
                    dtp_PayDate.Enabled = false;

                    var _totalPrice = Selected.InputPrice1 + Selected.InputPrice2 + Selected.InputPrice3;
                    var _inputPrice = Selected.Amount - _totalPrice;

                    txtTotalInput.Text = Convert.ToInt64(_totalPrice).ToString("N0");

                    txt_InputPrice.Text = String.Format("{0:#,###}", Convert.ToInt64(_inputPrice));
                    dtp_PayDate.Value = Selected.PayDate;

                }
                else
                {
                    //chk_PayState.Checked = false;
                    btnPayUpdate.Enabled = true;
                    btnPayCancle.Enabled = false;
                    txt_InputPrice.Enabled = true;
                    dtp_PayDate.Value = DateTime.Now;
                    dtp_PayDate.Enabled = true;

                    var _totalPrice = Selected.InputPrice1 + Selected.InputPrice2 + Selected.InputPrice3;
                    var _inputPrice = Selected.Amount - _totalPrice;

                    txtTotalInput.Text = Convert.ToInt64(_totalPrice).ToString("N0");


                    txt_InputPrice.Text = Convert.ToInt64(_inputPrice).ToString("N0");
                }

                if (Selected.PayDate1 != "")
                {
                    btnCurrentDelete.Enabled = false;
                    // btnUpdate.Enabled = false;
                }
                else
                {
                    btnCurrentDelete.Enabled = true;
                    //btnUpdate.Enabled = true;

                }
                InputPrice1.Text = Convert.ToInt64(Selected.InputPrice1).ToString();
                InputPrice2.Text = Convert.ToInt64(Selected.InputPrice2).ToString();
                InputPrice3.Text = Convert.ToInt64(Selected.InputPrice3).ToString();
                //if(Selected.SourceType == 1)
                //{
                //    btnUpdate.Enabled = false;


                //}
                //else
                //{

                //    btnUpdate.Enabled = true;

                //}

                // chk_PayState.Enabled = true;

                if (Selected.Issue == true)
                {


                    dtp_RequestDate.Enabled = true;
                    txt_SangHo.Enabled = false;
                    btn_CustomerInfo.Enabled = false;
                    panel5.Enabled = false;
                    txt_BizNo.Enabled = false;
                    txt_CEO.Enabled = false;
                    txt_ContRactName.Enabled = false;
                    txt_Uptae.Enabled = false;
                    txt_Upjong.Enabled = false;
                    txt_Email.Enabled = true;
                    txt_AddrDetail.Enabled = false;
                    btnFindZip.Enabled = false;
                    txt_MobileNo.Enabled = false;
                    txt_Item.Enabled = false;
                    txt_UnitPrice.Enabled = false;
                    txt_Num.Enabled = false;

                    btn_Issue.Enabled = false;


                    //btnCurrentDelete.Enabled = false;
                    //btn_Tax.Enabled = false;


                    if (Selected.IssueState == "취소")
                    {
                        btnCurrentDelete.Enabled = true;
                        btnUpdate.Enabled = false;
                        btn_Tax.Enabled = false;
                    }
                    else
                    {


                        btnCurrentDelete.Enabled = false;
                        btnUpdate.Enabled = true;
                        btn_Tax.Enabled = true;
                    }
                }
                else
                {

                    dtp_RequestDate.Enabled = true;
                    txt_SangHo.Enabled = true;
                    btn_CustomerInfo.Enabled = true;
                    panel5.Enabled = true;
                    txt_BizNo.Enabled = true;
                    txt_CEO.Enabled = true;
                    txt_ContRactName.Enabled = true;
                    txt_Uptae.Enabled = true;
                    txt_Upjong.Enabled = true;
                    txt_Email.Enabled = true;
                    txt_AddrDetail.Enabled = true;
                    btnFindZip.Enabled = true;
                    txt_MobileNo.Enabled = true;
                    txt_Item.Enabled = true;
                    txt_UnitPrice.Enabled = true;
                    txt_Num.Enabled = true;
                    btn_Issue.Enabled = true;

                    btnCurrentDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                    btn_Tax.Enabled = true;

                    if (Selected.SourceType == 1)
                    {
                        dtp_RequestDate.Enabled = true;
                        txt_SangHo.Enabled = false;
                        btn_CustomerInfo.Enabled = false;
                        panel5.Enabled = false;
                        txt_BizNo.Enabled = false;
                        txt_CEO.Enabled = false;
                        txt_ContRactName.Enabled = false;
                        txt_Uptae.Enabled = false;
                        txt_Upjong.Enabled = false;
                        txt_Email.Enabled = true;
                        txt_AddrDetail.Enabled = false;
                        btnFindZip.Enabled = false;
                        txt_MobileNo.Enabled = false;
                        txt_Item.Enabled = false;
                        txt_UnitPrice.Enabled = false;
                        txt_Num.Enabled = false;
                    }
                }
               

                if(Selected.Taxgubun == 0)
                {
                    rdb_Tax0.Checked = true;
                }
                else if(Selected.Taxgubun == 1)
                {
                    rdb_Tax1.Checked = true;
                }
                else
                {
                    rdb_Tax2.Checked = true;
                }

                //GridIndex = salesManageBindingSource.Position;
                _Sender = "";
                MethodProccess = false;

                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    
                    ApplyAuth();
                }

            }
        }

        private void txt_UnitPrice_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_UnitPrice.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice.Text);
                decimal _Num = decimal.Parse(txt_Num.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");

                }
                txt_UnitPrice.Text = _UnitPrice.ToString("N0");
                txt_Num.Text = _Num.ToString("N0");
            }
        }

        private void txt_Num_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_UnitPrice.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice.Text);
                decimal _Num = decimal.Parse(txt_Num.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                txt_UnitPrice.Text = _UnitPrice.ToString("N0");
                txt_Num.Text = _Num.ToString("N0");
            }
        }

        private void txt_UnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
            if (grid1.Columns[e.ColumnIndex] == SalesChk)
            {
                //if (Selected.Issue == true)
                //{
                //    var _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                //    _Cell.Enabled = false;
                //    _Cell.ReadOnly = true;
                //}
                //else
                //{
                //    var _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                //    _Cell.Enabled = true;
                //    _Cell.ReadOnly = false;
                //}
            }
            if (e.ColumnIndex == No.Index)
            {
                grid1[e.ColumnIndex, e.RowIndex].Value = (grid1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            else if (grid1.Columns[e.ColumnIndex] == createDateDataGridViewTextBoxColumn)
            {
                e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
            }
            else if (grid1.Columns[e.ColumnIndex] == requestDateDataGridViewTextBoxColumn)
            {
                e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
            }
            else if (grid1.Columns[e.ColumnIndex] == unitPriceDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == numDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == vatDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == priceDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == AmountDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == issueDateDataGridViewTextBoxColumn)
            {
                if (Selected.Issue == true)
                {
                    grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == CardPayGubun)
            {

                if (Selected.CardPayGubun == "C")
                {
                    e.Value = "차세로 거래처";
                }
                else
                {
                    e.Value = "일반 거래처";
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == bizNoDataGridViewTextBoxColumn)
            {

                if (Selected.BizNo.Length == 10)
                {
                    e.Value = Selected.BizNo.Substring(0, 3) + "-" + Selected.BizNo.Substring(3, 2) + "-" + Selected.BizNo.Substring(5, 5);
                }
                else
                {
                    e.Value = Selected.BizNo;
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == PayDate)
            {

                if (Selected.PayState == 1)
                {
                    e.Value = Selected.PayDate1;
                }
                else
                {
                    e.Value = "";
                }
            }
            else if (e.ColumnIndex == ColumnError.Index)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                if (TaxInvoiceErrorDic.ContainsKey(Selected.SalesId))
                {
                    e.Value = TaxInvoiceErrorDic[Selected.SalesId];
                }
                else
                {
                    e.Value = "";
                }
            }
            else if (e.ColumnIndex == ColumnOrderText.Index)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                if (Selected.SourceType == 1)
                    e.Value = $"{Selected.BeginDate:yyyy/MM/dd}".Replace("-", "/") + "-" + $"{Selected.EndDate:yyyy/MM/dd}".Replace("-", "/");
                else
                    e.Value = "";
            }
            else if (grid1.Columns[e.ColumnIndex] == ColumnAcceptCount)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                int acceptCount = 0;
                try
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        var Command1 = cn.CreateCommand();
                        Command1.CommandText = "SELECT count(*) FROM Orders where SalesManageId = " + Selected.SalesId + "";
                        var o1 = Command1.ExecuteScalar();
                        if (o1 != null)
                        {
                            acceptCount += Convert.ToInt32(o1);
                        }
                        cn.Close();
                    }
                    e.Value = acceptCount.ToString("N0");
                }
                catch
                {
                    e.Value = 0;
                }
            }
            else if (e.ColumnIndex == IssueDate1.Index)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                if (Selected != null)
                {
                    //if (!Selected.Issue)
                    //{
                    //    e.Value = "";
                    //}

                    if (String.IsNullOrEmpty(Selected.IssueLoginId) && String.IsNullOrEmpty(Selected.IssueGubun))
                    {
                        e.Value = "";
                    }

                }
            }
            else if (e.ColumnIndex == IssueState.Index)
            {

                if (e.Value == null || e.Value.ToString() == "0" || String.IsNullOrEmpty(e.Value.ToString()))

                    e.Value = "미발행";
            }
            else if (e.ColumnIndex == EndDay.Index)
            {
                //  var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                if (Selected != null)
                {
                    if (cMDataSet.Customers.Any(c => c.CustomerId == Selected.CustomerId))
                    {
                        e.Value = cMDataSet.Customers.FindByCustomerId(Selected.CustomerId).EndDay;
                    }
                    else
                    {
                        e.Value = "";
                    }
                }
            }
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Search.Clear();
            if (cmb_Search.SelectedIndex == 0)
            {
                txt_Search.ReadOnly = true;
            }
            else
            {
                txt_Search.ReadOnly = false;
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Date.SelectedIndex = 0;

            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
            cmb_Etax.SelectedIndex = 0;
            cmb_PayState.SelectedIndex = 0;
            cmbSMonth.SelectedIndex = 0;

            dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
            dtp_Edate.Value = DateTime.Now;
            _Search();
        }

        bool overhead = false;
        List<string> checkedCodes = new List<string>();

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < grid1.RowCount; i++)
            {
                var cell = grid1[SalesChk.Index, i] as DataGridViewDisableCheckBoxCell;
                if (cell.Enabled)
                    grid1[SalesChk.Index, i].Value = chkAllSelect.Checked;
            }



            //overhead = true;
            //for (int i = 0; i < dataGridView1.RowCount; i++)
            //{
            //    dataGridView1[SalesChk.Index, i].Value = chkAllSelect.Checked;
            //}
            //overhead = false;
            ////dataGridView1_CellValueChanged(null, null);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == SalesChk.Index)
                {
                    object o = grid1[e.ColumnIndex, e.RowIndex].Value;

                    //var IssueDate1 = ((DataRowView)salesManageBindingSource[e.RowIndex])["IssueDate1"].ToString();

                    var Issue1 = ((DataRowView)salesManageBindingSource[e.RowIndex])["Issue"].ToString();
                    var IssueState1 = ((DataRowView)salesManageBindingSource[e.RowIndex])["IssueState"].ToString();

                    string code = ((DataRowView)salesManageBindingSource[e.RowIndex])["SalesId"].ToString();
                    if (Convert.ToBoolean(o))
                    {
                        if (!checkedCodes.Contains(code) && Issue1 == "False")
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

        //private string getFilterString()
        //{
        //    string r = "'0'";
        //    if (checkedCodes.Count > 0)
        //    {
        //        r = String.Join(",", checkedCodes.Select(c => "'" + c + "'").ToArray());
        //    }
        //    return r;
        //}
        public SalesDataSet.SalesManageRow[] Sales { get; set; }


        private Dictionary<int, String> TaxInvoiceErrorDic = new Dictionary<int, string>();
        private void btn_Tax_Click(object sender, EventArgs e)
        {
            LocalUser.Instance.LogInInformation.LoadClient();

            if (LocalUser.Instance.LogInInformation.IsAdmin)
                return;
            var myClient = LocalUser.Instance.LogInInformation.Client;
            string NiceEtaxGubun = "D";
            if (LocalUser.Instance.LogInInformation.Client.EtaxGubun == "N")
            {
                //if (String.IsNullOrEmpty(myClient.userid))
                //{
                SendEtaxDialogMessageBox Smdm = new SendEtaxDialogMessageBox();

                #region 정발행
                if (Smdm.ShowDialog() == DialogResult.Yes)
                {
                    
                    #region Nice전자세금계산서


                    var _Email = myClient.Email.Split('@');

                    var checkIsMember = dTIServiceClass.GetMembJoinInf(linkcd, _Email[0], myClient.BizNo.Replace("-", ""), myClient.CEO, myClient.Email, myClient.MobileNo);

                    if (!String.IsNullOrEmpty(checkIsMember))
                    {
                        var ResultList = checkIsMember.Split('/');

                        try
                        {
                            var _Client = LocalUser.Instance.LogInInformation.Client;

                            var retVal = ResultList[0];
                            var errMsg = ResultList[1];
                            var frnNo = ResultList[2];
                            var userid = ResultList[3];
                            var passwd = ResultList[4];

                            if (retVal == "0")
                            {
                                //return;
                            }
                            else
                            {
                                var NiceMemberJoin = dTIServiceClass.Memberjoin(linkcd, _Email[0], _Client.BizNo.Replace("-", ""), _Client.Name, _Client.CEO, _Client.Uptae, _Client.Upjong, _Client.CEO, _Client.Email, _Client.PhoneNo, _Client.MobileNo, _Client.ZipCode, _Client.AddressState, _Client.AddressCity + _Client.AddressDetail);


                                if (!String.IsNullOrEmpty(NiceMemberJoin))
                                {

                                    var NiceMemberJoinList = NiceMemberJoin.Split('/');

                                    try
                                    {
                                        //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                                        retVal = NiceMemberJoinList[0];
                                        errMsg = NiceMemberJoinList[1];
                                        frnNo = NiceMemberJoinList[2];
                                        userid = NiceMemberJoinList[3];
                                        passwd = NiceMemberJoinList[4];

                                        if (retVal == "0")
                                        {
                                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                            {


                                                cn.Open();
                                                SqlCommand cmd = cn.CreateCommand();



                                                cmd.CommandText =
                                                   "UPDATE Clients SET linkcd = @linkcd,frnNo = @frnNo , userid = @userid, passwd = @passwd WHERE ClientId = @Clientid";



                                                cmd.Parameters.AddWithValue("@linkcd", linkcd);
                                                cmd.Parameters.AddWithValue("@frnNo", frnNo);
                                                cmd.Parameters.AddWithValue("@userid", userid);
                                                cmd.Parameters.AddWithValue("@passwd", passwd);
                                                cmd.Parameters.AddWithValue("@Clientid", LocalUser.Instance.LogInInformation.ClientId);

                                                cmd.ExecuteNonQuery();
                                                cn.Close();


                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show(errMsg);

                                        }



                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }


                                }

                            }



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        LocalUser.Instance.LogInInformation.LoadClient();

                    }
                    #endregion
                    //}




                    #region 나이스 공인인증서
                    AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
                    //var Query = baseDataSet.cl.FirstOrDefault();
                    var _Clients = LocalUser.Instance.LogInInformation.Client;
                    var Result = dTIServiceClass.selectExpireDt("EDB", _Clients.frnNo, _Clients.userid, _Clients.passwd);

                    if (!String.IsNullOrEmpty(Result))
                    {
                        var ResultList = Result.Split('/');

                        try
                        {
                            //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                            var retVal = ResultList[0];
                            var errMsg = ResultList[1];
                            var regYn = ResultList[2];
                            var expireDt = ResultList[3];





                            if (regYn == "N")
                            {
                                MessageBox.Show("공인인증서를 등록하셔야 합니다. 공인인증 등록 페이지로 이동합니다.\r\n인증서 등록후 회원정보에 공인인증서 비밀번호를 입력하세요.");
                                // MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);


                                var Query = baseDataSet.AdminInfoes.FirstOrDefault();
                                string url = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do?";
                                if (Query.IsTest != true)
                                {
                                    url = "http://www.nicedata.co.kr/ti/TI_80101.do?";
                                }


                                string frnNo = "";
                                string userid = "";
                                string passwd = "";
                                string Linkcd = "";

                                try
                                {
                                    frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.frnNo }).ToString();
                                    userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.userid }).ToString();
                                    passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.passwd }).ToString();
                                    Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "EDB" }).ToString();


                                    //MessageBox.Show(Encode.ToString());

                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.Write(ex.Message);

                                }

                                string value = $"frnNo={frnNo}&userId={userid}&passwd={passwd}&linkCd={Linkcd}&retUrl=";

                                //FrmNice f = new FrmNice(value, url);
                                //f.ShowDialog();
                                //Process.Start(url + value);
                                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Internet Explorer\\iexplore.exe", url + value);

                            }
                            else
                            {
                                //  MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);

                            }



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                    }

                    #endregion

                    if (String.IsNullOrEmpty(_Clients.NPKIpasswd))
                    {
                        MessageBox.Show("공인인증서 비밀번호를 등록후 이용하시기 바랍니다.", "공인인증서 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                        FrmMDI.LoadForm("FrmMN0204", "");


                        return;
                    }

                   
                }
                #endregion

                #region 역발행
                else if (Smdm.DialogResult == DialogResult.OK)
                {
                    NiceEtaxGubun = "R";
                }
                #endregion
                else
                { }
            }
            else
            {
                #region 팝빌 회원확인
                var checkIsMember = taxinvoiceService.CheckIsMember(myClient.BizNo.Replace("-", ""), LinkID);
                if (checkIsMember.code == 0)
                {

                    MessageBox.Show("전자세금계산서 발행을 위한 별도 회원가입이 필요 합니다\r\n" +
                        "아래 “확인” 버튼을 눌러, 팝빌 홈페이지에 접속 합니다.\r\n" +
                        " ① “회원가입”->”연동회원” 클릭.\r\n" +
                        " ② “링크아이디”는 “EDUBILL”를 입력\r\n" +
                        $" ③ “아이디”는 “{myClient.LoginId}”를 입력\r\n" +
                        $" ④ “비밀번호”는 “{myClient.Password}”를 입력\r\n" +
                        "※. 위 아이디/비밀번호는 별도 메모 하셔야 합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    FrmPopbillRegister f = new Admin.FrmPopbillRegister();
                    f.Show();
                    return;
                }

                #endregion
            }
            
            string errormessage = string.Empty;

            var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);

            if (_ClientPoint < 150)
            {

                MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                FrmMDI.LoadForm("FrmMN0204", "Point");

                
                return;
            }

            #region 전자세금계산서 체크박스

            var Datas = new List<SalesDataSet.SalesManageRow>();
            for (int i = 0; i < grid1.RowCount; i++)
            {

                var _Cell = grid1[SalesChk.Index, i] as DataGridViewDisableCheckBoxCell;
                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                    var _Row = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                    _Row.RejectChanges();

                    _Row.IssueDate = DateTime.Now;
                    if (_Row.Issue != true)
                    {
                        Datas.Add(_Row);
                    }
                }
            }
            if (Datas.Any())
            {
                //FrmLogin_B f = new FrmLogin_B();
                //f.ShowDialog();
                //if (f.Accepted)
                //{
                if (LocalUser.Instance.LogInInformation.Client.EtaxGubun == "N")
                {
                    if (NiceEtaxGubun == "R")
                    {
                        Dialog_EtaxManageNiceR r = new Dialog_EtaxManageNiceR();

                        r.Sales = Datas.ToArray();

                        r.ShowDialog();
                    }
                    else if (NiceEtaxGubun == "D")
                    {
                        Dialog_EtaxManageNice d = new Dialog_EtaxManageNice();

                        d.Sales = Datas.ToArray();

                        d.ShowDialog();
                    }
                }
                else
                {
                    Dialog_EtaxManage d = new Dialog_EtaxManage();

                    d.Sales = Datas.ToArray();

                    d.ShowDialog();
                }
                btn_Search_Click(null, null);
                //}
            }
            else
            {
                MessageBox.Show("전사세금계산서 발행할 항목들을 선택하여 주십시오.");
            }


            #endregion



          




        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grid1.IsCurrentCellDirty)
            {
                grid1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect1 = grid1.GetColumnDisplayRectangle(SalesChk.Index, true);
            chkAllSelect.Location = new Point(rect1.Location.X + 2, rect1.Location.Y + 4);
            if (rect1.Width == 0) chkAllSelect.Visible = false;
            else chkAllSelect.Visible = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Success = 0;
            Error = 0;
            if (e.RowIndex < 0)
                return;

            if (grid1.Columns[e.ColumnIndex] == IssueDate1 && grid1.Columns[e.ColumnIndex] == Column1 && e.RowIndex >= 0)
                return;
            var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as SalesDataSet.SalesManageRow;
            if (grid1.Columns[e.ColumnIndex] == Column1 && e.RowIndex >= 0)
            {
                if (Selected != null)
                {

                    if (Selected.Issue == true && Selected.IssueState != "취소")
                    {
                        #region 팝빌
                        if (!String.IsNullOrEmpty(Selected.invoicerMgtKey))
                        {
                            if (Selected.HasACC)
                            {
                                var totalsum = Selected.InputPrice1 + Selected.InputPrice2 + Selected.InputPrice3;
                                if (totalsum > 0)
                                {
                                    MessageBox.Show("수금액이 존재하여 발행취소 할 수 없습니다..", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return;
                                }
                                #region 
                                TaxinvoiceInfo taxinvoiceInfo = taxinvoiceService.GetInfo(LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", ""), MgtKeyType.SELL, Selected.invoicerMgtKey);

                                try
                                {
                                    string tmp = null;

                                    tmp += "itemKey : " + taxinvoiceInfo.itemKey + CRLF;
                                    tmp += "taxType : " + taxinvoiceInfo.taxType + CRLF;
                                    tmp += "writeDate : " + taxinvoiceInfo.writeDate + CRLF;
                                    tmp += "regDT : " + taxinvoiceInfo.regDT + CRLF;

                                    tmp += "invoicerCorpName : " + taxinvoiceInfo.invoicerCorpName + CRLF;
                                    tmp += "invoicerCorpNum : " + taxinvoiceInfo.invoicerCorpNum + CRLF;
                                    tmp += "invoicerMgtKey : " + taxinvoiceInfo.invoicerMgtKey + CRLF;
                                    tmp += "invoicerPrintYN : " + taxinvoiceInfo.invoicerPrintYN + CRLF;
                                    tmp += "invoiceeCorpName : " + taxinvoiceInfo.invoiceeCorpName + CRLF;
                                    tmp += "invoiceeCorpNum : " + taxinvoiceInfo.invoiceeCorpNum + CRLF;
                                    tmp += "invoiceeMgtKey : " + taxinvoiceInfo.invoiceeMgtKey + CRLF;
                                    tmp += "invoiceePrintYN : " + taxinvoiceInfo.invoiceePrintYN + CRLF;
                                    tmp += "closeDownState : " + taxinvoiceInfo.closeDownState + CRLF;
                                    tmp += "closeDownStateDate : " + taxinvoiceInfo.closeDownStateDate + CRLF;
                                    tmp += "interOPYN : " + taxinvoiceInfo.interOPYN + CRLF;

                                    tmp += "supplyCostTotal : " + taxinvoiceInfo.supplyCostTotal + CRLF;
                                    tmp += "taxTotal : " + taxinvoiceInfo.taxTotal + CRLF;
                                    tmp += "purposeType : " + taxinvoiceInfo.purposeType + CRLF;
                                    tmp += "modifyCode : " + taxinvoiceInfo.modifyCode.ToString() + CRLF;
                                    tmp += "issueType : " + taxinvoiceInfo.issueType + CRLF;

                                    tmp += "issueDT : " + taxinvoiceInfo.issueDT + CRLF;
                                    tmp += "preIssueDT : " + taxinvoiceInfo.preIssueDT + CRLF;

                                    tmp += "stateCode : " + taxinvoiceInfo.stateCode.ToString() + CRLF;
                                    tmp += "stateDT : " + taxinvoiceInfo.stateDT + CRLF;

                                    tmp += "openYN : " + taxinvoiceInfo.openYN.ToString() + CRLF;
                                    tmp += "openDT : " + taxinvoiceInfo.openDT + CRLF;
                                    tmp += "ntsresult : " + taxinvoiceInfo.ntsresult + CRLF; //국세청전송결과
                                    tmp += "ntsconfirmNum : " + taxinvoiceInfo.ntsconfirmNum + CRLF; //국세청승인번호
                                    tmp += "ntssendDT : " + taxinvoiceInfo.ntssendDT + CRLF; //
                                    tmp += "ntsresultDT : " + taxinvoiceInfo.ntsresultDT + CRLF;
                                    tmp += "ntssendErrCode : " + taxinvoiceInfo.ntssendErrCode + CRLF;
                                    tmp += "stateMemo : " + taxinvoiceInfo.stateMemo;
                                    // MessageBox.Show(tmp, "문서 상태/요약 정보 조회");

                                    if (!string.IsNullOrEmpty(taxinvoiceInfo.ntsresult))
                                    {
                                        TinfoNtsresult = taxinvoiceInfo.ntsresult;
                                    }
                                    TinfoWriteDate = taxinvoiceInfo.writeDate;
                                    TinfoitemKey = taxinvoiceInfo.itemKey;
                                    TinfostateCode = taxinvoiceInfo.stateCode.ToString();

                                    switch (TinfostateCode)
                                    {
                                        case "300":
                                            TinfostateName = "발행완료";
                                            break;
                                        case "310":
                                            TinfostateName = "발행완료";
                                            break;
                                        case "301":
                                            TinfostateName = "전송중";
                                            break;
                                        case "302":
                                            TinfostateName = "전송중";
                                            break;
                                        case "303":
                                            TinfostateName = "전송중";
                                            break;
                                        case "311":
                                            TinfostateName = "전송중";
                                            break;
                                        case "312":
                                            TinfostateName = "전송중";
                                            break;
                                        case "313":
                                            TinfostateName = "전송중";
                                            break;
                                        case "304":
                                            TinfostateName = "전송성공";
                                            break;
                                        case "314":
                                            TinfostateName = "전송성공";
                                            break;
                                        case "305":
                                            TinfostateName = "전송실패";
                                            break;
                                        case "315":
                                            TinfostateName = "전송실패";
                                            break;

                                    }
                                }

                                catch (PopbillException ex)
                                {
                                    MessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                                                    "응답메시지(message) : " + ex.Message, "문서 상태/요약 정보 조회");
                                    return;
                                }
                                #endregion
                                if (Selected.IssueDate.Date == DateTime.Now.Date)
                                {
                                    if (TinfostateName == "발행완료")
                                    {
                                        if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 당일 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {
                                                SendCancelIssue(Selected.SalesId);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }
                                        }
                                    }
                                    else if (TinfostateName == "전송성공")
                                    {
                                        if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {
                                                MinusInvoice(Selected.IssueDate.ToString("yyyyMMdd"), Selected.invoicerMgtKey);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }


                                        }
                                    }
                                    else
                                    {

                                        MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }




                                    //if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 당일 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    //{
                                    //    try
                                    //    {
                                    //        SendCancelIssue(Selected.SalesId);
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    //        return;
                                    //    }
                                    //}
                                }
                                else
                                {

                                    if (TinfostateName == "발행완료")
                                    {
                                        if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {
                                                SendCancelIssue(Selected.SalesId);

                                                //SendMinusInvoice(Selected.DriverId, Selected.TradeId, Selected.ClientId, Selected.trusteeMgtKey);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }
                                        }
                                    }
                                    else if (TinfostateName == "전송성공")
                                    {
                                        if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {

                                                MinusInvoice(Selected.IssueDate.ToString("yyyyMMdd"), Selected.invoicerMgtKey);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }



                                }





                            }
                            else
                            {
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                       $"UPDATE SalesManage SET IssueDate = NULL, Issue = 0, IssueState = '0' ,IssueLoginId = NULL, IssueGubun = NULL  WHERE    SalesId ={Selected.SalesId}";
                                    cmd.Parameters.AddWithValue("@IssueDate", dtp_PayDate.Value);

                                    cmd.ExecuteNonQuery();
                                    cn.Close();
                                }
                                Selected.SetIssueDateNull();
                                Selected.Issue = false;
                                Selected.IssueState = "";
                                salesManageBindingSource.ResetBindings(false);
                            }
                        }
                        #endregion

                        #region 나이스
                        else
                        {


                            var totalsum = Selected.InputPrice1 + Selected.InputPrice2 + Selected.InputPrice3;
                            if (totalsum > 0)
                            {
                                MessageBox.Show("수금액이 존재하여 발행취소 할 수 없습니다..", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                return;
                            }


                            #region 수정 취소
                           
                            #endregion



                            string[] BillStatusList = null;
                            //var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
                            LocalUser.Instance.LogInInformation.LoadClient();
                            var _Clients = LocalUser.Instance.LogInInformation.Client;
                            var _changedStatusReq = "";
                            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
                            var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
                            if (Selected.TypeCode == "위수탁")
                            {
                                
                                _changedStatusReq = dTIServiceClass.changedStatusReqById("EDB", _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, Selected.RequestDate.AddDays(-1).ToString("yyyyMMdd"), Selected.RequestDate.AddMonths(1).ToString("yyyyMMdd"));
                            }
                            else
                            {


                                _changedStatusReq = dTIServiceClass.changedStatusReqById("EDB", _Clients.frnNo, _Clients.userid, _Clients.passwd, Selected.RequestDate.AddDays(-1).ToString("yyyyMMdd"), Selected.RequestDate.AddMonths(1).ToString("yyyyMMdd"));
                            }
                            //var _changedStatusReq = dTIServiceClass.changedStatusReqByLinkCd("EDB",  Selected.RequestDate.AddDays(1).ToString("yyyyMMdd"));
                            

                            try
                            {
                                var _changedStatusReqList = _changedStatusReq.Split('/');

                                var retVal = _changedStatusReqList[0];
                                var errMsg = _changedStatusReqList[1];
                                var statusMsg = _changedStatusReqList[2];

                                if (retVal == "0")
                                {
                                   
                                    var statusMsgList = statusMsg.Split(';');

                                    for (int i = 0; i < statusMsgList.Length; i++)
                                    {
                                        if (statusMsgList[i].Contains(Selected.billNo))
                                        {
                                            BillStatusList = statusMsgList[i].Split(',');
                                            break;
                                        }
                                    }

                                    if (BillStatusList.Length != 0)
                                    {
                                        //승인번호
                                        _billNo = BillStatusList[0];
                                        //1.문서상태변경 2.신고상태변경
                                        _changestatus = BillStatusList[1];
                                        //문서상태 코드 또는 신고상태 코드
                                        _changestatusCode = BillStatusList[2];
                                        //국세청응답코드
                                        _EtaxCode = BillStatusList[3];
                                    }

                                }
                                else
                                {

                                }
                            }
                            catch { }
                           

                            if (_changestatusCode == "A" )
                            {
                                if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 당일 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        NiceEtaxDelete(Selected.SalesId);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }


                                }





                            }
                            else if(_changestatusCode == "B")
                            {
                                MessageBox.Show("국세청 전송중입니다.\r\n잠시후 다시 시도해주세요.", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            else if ( _changestatusCode == "C" || _changestatusCode == "D")
                            {
                                if (Selected.TypeCode == "위수탁" && newDGV1.RowCount == 0)
                                {

                                    #region 세금계산서 수정
                                    NiceEtaxDialogMessageBox Smdm = new NiceEtaxDialogMessageBox();

                                    //수정
                                    if (Smdm.ShowDialog() == DialogResult.Yes)
                                    {
                                        if (newDGV1.RowCount > 0)
                                        {
                                            if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            {
                                                try
                                                {
                                                    NiceEtaxDeleteEdit(Selected.SalesId);
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message, "발행취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                    return;
                                                }


                                            }
                                        }
                                        else
                                        {
                                            FrmNiceEdit frmNiceEdit = new FrmNiceEdit(Selected.SalesId, Selected.billNo);

                                            frmNiceEdit.ShowDialog();
                                           // NiceEtaxEdit(Selected.SalesId);


                                        }




                                    }

                                    //취소
                                    else if (Smdm.DialogResult == DialogResult.OK)
                                    {
                                        if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {
                                                NiceEtaxDeleteEdit(Selected.SalesId);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "발행취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }


                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        try
                                        {
                                            NiceEtaxDeleteEdit(Selected.SalesId);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message, "발행취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                            return;
                                        }


                                    }

                                }
                                

                              

                                
                            }
                            else
                            {
                                if (MessageBox.Show("정말 삭제하시겠습니까?", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        NiceEtaxDelete(Selected.SalesId);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }


                                }


                            }

                        }
                        #endregion
                    }
                   
                }

            }
            else if (grid1.Columns[e.ColumnIndex] == Stats)
            {
                var Query = ShareOrderDataSet.Instance.Orders.Where(c => c.SalesManageId == Selected.SalesId);

                if (Query.Any())
                {
                    var Amount = Selected.Amount;
                    var HAmount = "합계금액 : " + Amount.ToString("N0") + " (금 " + Number2Hangle((long)Amount) + "원정)";

                    FrmTaxSales f = new FrmTaxSales(Selected.SalesId, HAmount);
                    //var RCount = 0;
                    //foreach(var r in Query)
                    //{
                    //    if(!String.IsNullOrEmpty(r.RequestMemo))
                    //    {
                    //        RCount++;
                    //    }
                    //}
                    //if (RCount > 0)
                    //{
                    //    f.PrintClient2();
                    //}
                    //else
                    //{
                    //    f.PrintClient();

                    //}

                    LocalUser.Instance.LogInInformation.LoadClient();
                    _StatsGubun = LocalUser.Instance.LogInInformation.Client.StatsGubun;



                    switch (_StatsGubun)
                    {
                        case 0:
                            f.PrintClientDefault();
                            break;

                        case 1:
                            f.PrintClientDefault1();
                            break;
                        case 2:
                            f.PrintClientRemark();
                            break;
                        case 3:
                            f.PrintClientRemark1();
                            break;
                        case 4:
                            f.PrintClientDriver();
                            break;

                    }

                    f.StartPosition = FormStartPosition.CenterScreen;
                    //f.Size = FormWindowState.Maximized;

                    f.ShowDialog();
                }
                else
                {
                    var Amount = Selected.Amount;
                    var HAmount = "합계금액 : " + Amount.ToString("N0") + " (금 " + Number2Hangle((long)Amount) + "원정)";

                    FrmTaxSales2 f = new FrmTaxSales2(Selected.SalesId, HAmount, Selected.CardPayGubun);


                    f.PrintClient();
                    f.StartPosition = FormStartPosition.CenterScreen;
                    //f.Size = FormWindowState.Maximized;

                    f.ShowDialog();

                }






            }

        }
        public string Number2Hangle(long lngNumber)
        {

            string[] NumberChar = new string[] { "", "일", "이", "삼"
                                               , "사", "오", "육"
                                               , "칠", "팔", "구" };
            string[] LevelChar = new string[] { "", "십", "백", "천" };
            string[] DecimalChar = new string[] { "", "만", "억", "조", "경" };

            string strMinus = string.Empty;

            if (lngNumber < 0)
            {
                strMinus = "마이너스";
                lngNumber *= -1;
            }

            string strValue = string.Format("{0}", lngNumber);
            string NumToKorea = string.Empty;
            bool UseDecimal = false;

            if (lngNumber == 0) return "영";

            for (int i = 0; i < strValue.Length; i++)
            {
                int Level = strValue.Length - i;
                if (strValue.Substring(i, 1) != "0")
                {
                    UseDecimal = true;
                    if (((Level - 1) % 4) == 0)
                    {
                        if (DecimalChar[(Level - 1) / 4] != string.Empty
                           && strValue.Substring(i, 1) == "1")
                            NumToKorea = NumToKorea + DecimalChar[(Level - 1) / 4];
                        else
                            NumToKorea = NumToKorea
                                              + NumberChar[int.Parse(strValue.Substring(i, 1))]
                                              + DecimalChar[(Level - 1) / 4];
                        UseDecimal = false;
                    }
                    else
                    {
                        if (strValue.Substring(i, 1) == "1")
                            NumToKorea = NumToKorea
                                               + LevelChar[(Level - 1) % 4];
                        else
                            NumToKorea = NumToKorea
                                               + NumberChar[int.Parse(strValue.Substring(i, 1))]
                                               + LevelChar[(Level - 1) % 4];
                    }
                }
                else
                {
                    if ((Level % 4 == 0) && UseDecimal)
                    {
                        NumToKorea = NumToKorea + DecimalChar[Level / 4];
                        UseDecimal = false;
                    }
                }
            }
            return strMinus + NumToKorea;
        }


        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
            if (Selected.IssueState == "취소")
            {

                grid1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            }
            //if (grid1.Columns[e.ColumnIndex] == ShowTax && e.RowIndex >= 0)
            //{
            //    // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
            //    if (Selected != null)
            //    {
            //        if (Selected.Issue == false)
            //        {

            //            if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            //            {
            //                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            //                {
            //                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
            //                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
            //                    cellBackground.Dispose();
            //                }
            //                else
            //                {
            //                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
            //                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
            //                    cellBackground.Dispose();
            //                }
            //            }
            //            e.Handled = true;


            //        }
            //    }
            //}


            if (grid1.Columns[e.ColumnIndex] == Stats && e.RowIndex >= 0)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                if (Selected != null)
                {
                    //var Query = ShareOrderDataSet.Instance.Orders.Where(c => c.SalesManageId == Selected.SalesId);


                    //if (!Query.Any() )
                    //{


                    //    if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                    //    {
                    //        if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                    //        {
                    //            SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                    //            e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                    //            cellBackground.Dispose();
                    //        }
                    //        else
                    //        {
                    //            SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                    //            e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                    //            cellBackground.Dispose();
                    //        }
                    //    }
                    //    e.Handled = true;


                    //}
                }
            }
            //발행취소
            if (grid1.Columns[e.ColumnIndex] == Column1 && e.RowIndex >= 0)
            {
                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                if (Selected != null)
                {
                    if (Selected.Issue == false || Selected.IssueState == "취소")
                    {


                        if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                        {
                            if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                            else
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                        }
                        e.Handled = true;


                    }
                }
            }


            //if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            //{

            //    if (Selected != null)
            //    {
            //        if (Selected.Issue == true)
            //        {


            //            if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            //            {
            //                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            //                {
            //                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
            //                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
            //                    cellBackground.Dispose();
            //                }
            //                else
            //                {
            //                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
            //                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
            //                    cellBackground.Dispose();
            //                }
            //            }
            //            e.Handled = true;


            //        }
            //    }
            //}
        }

        private void txt_BizNo_Leave(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(LocalUser.Instance.LogInInformation.Client.ShopID) && !String.IsNullOrEmpty(LocalUser.Instance.LogInInformation.Client.ShopPW))
            {
                FrmPopbill f = new Admin.FrmPopbill();
                f.Show();
            }
            //LocalUser.Instance.LogInInformation.LoadClient();
            //var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;

            //if (Selected != null)
            //{
            //    if (string.IsNullOrEmpty(Selected.invoicerMgtKey) && !string.IsNullOrEmpty(Selected.billNo))
            //    {
            //        string sParameter = "";
            //        string EncodingSparameter = "";


            //        //현재시간(년월일시분초);공급자사업자번호;공급받는자사업자번호;연계코드
            //        sParameter = DateTime.Now.ToString("yyyyMMddHHmmss") + ";" + LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", "") + ";" + "" + ";" + "EDB";
            //        EncodingSparameter = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { sParameter }).ToString();


            //        // url = "http://t-renewal.nicedata.co.kr/ti/TI_80401.do?";
            //        string value = $"data={EncodingSparameter}";

            //        FrmNice f = new FrmNice(value, "http://t-renewal.nicedata.co.kr/ti/TI_80401.do?");
            //        f.Show();




            //    }
            //    else
            //    {
            //        if (!String.IsNullOrEmpty(LocalUser.Instance.LogInInformation.Client.ShopID) && !String.IsNullOrEmpty(LocalUser.Instance.LogInInformation.Client.ShopPW))
            //        {
            //            FrmPopbill f = new Admin.FrmPopbill();
            //            f.Show();
            //        }
            //        else
            //        {
            //            //MessageBox.Show("전자세금관리에 필요한 팝빌 어카운트 정보가 존재하지 않습니다. 확인을 누르면 관련 화면으로 이동합니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            //// Close();
            //            //FrmMDI.LoadForm("FrmMN0204_CARGOOWNERMANAGE", "");
            //        }
            //    }

            //}

        }

        private void rdb_Normal_CheckedChanged(object sender, EventArgs e)
        {
            lbl_HasAcc.Visible = false;
            cmb_HasAcc.Visible = false;
            panel4.Visible = false;


            txt_SangHo.Text = "";
            txt_BizNo.Text = "";
            txt_CEO.Text = "";
            txt_Uptae.Text = "";
            txt_Upjong.Text = "";

            txt_State.Text = "";
            txt_City.Text = "";
            txt_Zip.Text = "";

            txt_AddrDetail.Text = "";
            txt_Email.Text = "";
            txt_ContRactName.Text = "";
            txt_MobileNo.Text = "";
            txt_CardPayGubun.Text = "N";
            txt_CustomerId.Text = "";
            txt_BizNo_Leave(null, null);


            //cmb_HasAcc.SelectedIndex = 0;
        }

        private void rdb_Cardpay_CheckedChanged(object sender, EventArgs e)
        {
            lbl_HasAcc.Visible = true;
            cmb_HasAcc.Visible = true;
            panel4.Visible = true;

            txt_SangHo.Text = "";
            txt_BizNo.Text = "";
            txt_CEO.Text = "";
            txt_Uptae.Text = "";
            txt_Upjong.Text = "";


            txt_State.Text = "";
            txt_City.Text = "";
            txt_Zip.Text = "";

            txt_AddrDetail.Text = "";
            txt_Email.Text = "";
            txt_ContRactName.Text = "";
            txt_MobileNo.Text = "";
            txt_CardPayGubun.Text = "C";
            txt_CustomerId.Text = "";
            txt_BizNo_Leave(null, null);
        }

        private void chk_PayState_CheckedChanged(object sender, EventArgs e)
        {
            //if (chk_PayState.Checked)
            //{
            //    dtp_PayDate.Enabled = true;
            //}
            //else
            //{
            //    dtp_PayDate.Enabled = false;
            //}
        }
        private void SendCancelIssue(int salesid)
        {
            try
            {
                var _Sales = salesDataSet.SalesManage.FirstOrDefault(c => c.SalesId == salesid);
                Response response = taxinvoiceService.CancelIssue(LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", ""), MgtKeyType.SELL, _Sales.invoicerMgtKey, "", LocalUser.Instance.LogInInformation.Client.ShopID);

                SendDelete(salesid);
            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);

                return;

            }
        }

        private void SendDelete(int salesid)
        {

            try
            {
                LocalUser.Instance.LogInInformation.LoadClient();
                //var Query = cMDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
                var Query = LocalUser.Instance.LogInInformation.Client;


                var _Sales = salesDataSet.SalesManage.FirstOrDefault(c => c.SalesId == salesid);
                Response response = taxinvoiceService.Delete(LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", ""), MgtKeyType.SELL, _Sales.invoicerMgtKey, LocalUser.Instance.LogInInformation.Client.ShopID);


                try
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText =
                           $"UPDATE SalesManage SET IssueDate = NULL, Issue = 0, IssueState = '0' ,IssueLoginId = NULL, IssueGubun = NULL WHERE    SalesId ={salesid}";
                        cmd.Parameters.AddWithValue("@IssueDate", dtp_PayDate.Value);

                        cmd.ExecuteNonQuery();


                        SqlCommand Customercmd = cn.CreateCommand();
                        Customercmd.CommandText =
                           " UPDATE Customers " +
                           " SET Misu = Misu - @Misu WHERE CustomerId ='" + _Sales.CustomerId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                        Customercmd.Parameters.AddWithValue("@Misu", _Sales.Amount);
                        Customercmd.ExecuteNonQuery();

                        //SqlCommand OrdersCmd = cn.CreateCommand();
                        //OrdersCmd.CommandText =
                        //   " UPDATE Orders " +
                        //   " SET SalesManageId = NULL WHERE SalesManageId ='" + _Sales.SalesId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";

                        //OrdersCmd.ExecuteNonQuery();


                        cn.Close();


                    }
                    MessageBox.Show("전자세금계산서가 삭제 되었습니다.");
                    //Success++;
                    btn_Search_Click(null, null);
                }

                catch (PopbillException ext)
                {
                    MessageBox.Show(ext.code.ToString() + "\r\n" + ext.Message);
                    return;
                }



            }
            catch (PopbillException ex)
            {

                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return;
            }




        }
        private void MinusInvoice(string Tdtp_Date, string MgtKEy)
        {
            Success = 0;
            Error = 0;


            var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as SalesDataSet.SalesManageRow;
            var Query = cMDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
            //발행취소

            bool forceIssue = false;        // 지연발행 강제여부
            String memo = "";  // 즉시발행 메모 



            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();

            //   Response response2 = taxinvoiceService.GetInfo(Query.First().BizNo.Replace("-", ""),MgtKeyType.SELL, MgtKEy);



            taxinvoice.writeDate = TinfoWriteDate;//Tdtp_Date;                      //필수, 기재상 작성일자
            taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
            taxinvoice.issueType = "정발행";                        //필수, {정발행, 역발행, 위수탁}
            taxinvoice.purposeType = "영수";                        //필수, {영수, 청구}
            taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
            taxinvoice.taxType = "과세";                            //필수, {과세, 영세, 면세}

            taxinvoice.invoicerCorpNum = Query.First().BizNo.Replace("-", "");              //공급자 사업자번호
            taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
            taxinvoice.invoicerCorpName = Query.First().CEO;
            taxinvoice.invoicerMgtKey = "C" + Selected.SalesId.ToString() + "-" + DateTime.Now.ToString("yyMMddhhmmss");           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoicerCEOName = Query.First().CEO;
            taxinvoice.invoicerAddr = Query.First().AddressState + " " + Query.First().AddressCity + " " + Query.First().AddressDetail;
            taxinvoice.invoicerBizClass = Query.First().Upjong;
            taxinvoice.invoicerBizType = Query.First().Uptae;
            taxinvoice.invoicerContactName = Query.First().CEO;
            taxinvoice.invoicerEmail = Query.First().Email;
            taxinvoice.invoicerTEL = Query.First().PhoneNo;
            taxinvoice.invoicerHP = Query.First().MobileNo;
            taxinvoice.invoicerSMSSendYN = false;                    //정발행시(공급자->공급받는자) 문자발송기능 사용시 활용

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            taxinvoice.invoiceeType = "사업자";                     //공급받는자 구분, {사업자, 개인, 외국인}
            taxinvoice.invoiceeCorpNum = Selected.BizNo.Replace("-", "");              //공급받는자 사업자번호
            taxinvoice.invoiceeCorpName = Selected.SangHo;
            taxinvoice.invoiceeMgtKey = "";                         //역발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoiceeCEOName = Selected.Ceo;
            taxinvoice.invoiceeAddr = Selected.AddressState + " " + Selected.AddressCity + " " + Selected.AddressDetail;
            taxinvoice.invoiceeBizClass = Selected.Upjong;
            taxinvoice.invoiceeBizType = Selected.Uptae;
            taxinvoice.invoiceeTEL1 = Selected.MobileNo;
            taxinvoice.invoiceeContactName1 = Selected.Ceo;
            taxinvoice.invoiceeEmail1 = Selected.Email;
            taxinvoice.invoiceeHP1 = Selected.MobileNo;
            taxinvoice.invoiceeSMSSendYN = false;                   //역발행시(공급받는자->공급자) 문자발송기능 사용시 활용



            Int64 MinusPrice = Convert.ToInt64(Selected.Price) * -1;
            Int64 MinusVat = Convert.ToInt64(Selected.Vat) * -1;
            Int64 MinusAmount = Convert.ToInt64(Selected.Amount) * -1;


            taxinvoice.supplyCostTotal = MinusPrice.ToString();                //필수 공급가액 합계"
            taxinvoice.taxTotal = MinusVat.ToString();                      //필수 세액 합계
            taxinvoice.totalAmount = MinusAmount.ToString();                 //필수 합계금액.  공급가액 + 세액

            //수정세금계산서 작성시 1~6까지 선택기재.
            // 1 - 기재사항 착오정정, 2 - 공급가액변동
            // 3 - 환입, 4 - 계약의 해지 
            // 5 - 내국신용장 사후개설
            // 6 - 착오에 의한 이중발행
            taxinvoice.modifyCode = 4;                           //수정세금계산서 작성시 1~6까지 선택기재.

            //수정세금계산서 작성시 원본 세금계산서의 ItemKey기재. 
            taxinvoice.originalTaxinvoiceKey = TinfoitemKey;                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
            taxinvoice.serialNum = "1";
            taxinvoice.cash = "";                                   //현금
            taxinvoice.chkBill = "";                                //수표
            taxinvoice.note = "";                                   //어음
            taxinvoice.credit = "";                                 //외상미수금
            taxinvoice.remark1 = "";
            taxinvoice.remark2 = "";
            taxinvoice.remark3 = "";
            taxinvoice.kwon = 1;
            taxinvoice.ho = 1;

            taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
            taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

            taxinvoice.detailList = new List<TaxinvoiceDetail>();

            TaxinvoiceDetail detail = new TaxinvoiceDetail();

            detail.serialNum = 1;                                   //일련번호
            detail.purchaseDT = Selected.CREATE_DATE1.Replace("/", "");                         //거래일자
            detail.itemName = txt_Item.Text;
            detail.spec = "";
            detail.qty = "1";                                       //수량
            detail.unitCost = MinusPrice.ToString();                            //단가
            detail.supplyCost = MinusPrice.ToString();                           //공급가액
            detail.tax = MinusVat.ToString();                                 //세액
            detail.remark = "";

            taxinvoice.detailList.Add(detail);

            detail = new TaxinvoiceDetail();

            try
            {
                // Response response = taxinvoiceService.RegistIssue(Query.First().BizNo, taxinvoice, forceIssue, memo);
                Response response = taxinvoiceService.RegistIssue(Query.First().BizNo.Replace("-", ""), taxinvoice, forceIssue, memo);

                try
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        cmd.CommandText =
                           "UPDATE SalesManage SET IssueDate = getdate()  , IssueState = '취소' ,invoicerMgtKey = @invoicerMgtKey,IssueLoginId = @IssueLoginId, IssueGubun = @IssueGubun  WHERE    SalesId =" + Selected.SalesId.ToString() + " ";
                        cmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);
                        cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
                        cmd.Parameters.AddWithValue("@IssueGubun", "차세로");
                        cmd.ExecuteNonQuery();

                        SqlCommand Customercmd = cn.CreateCommand();
                        Customercmd.CommandText =
                           " UPDATE Customers " +
                           " SET Misu = Misu- @Misu WHERE CustomerId ='" + Selected.CustomerId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                        Customercmd.Parameters.AddWithValue("@Misu", Selected.Amount);
                        Customercmd.ExecuteNonQuery();





                        cn.Close();

                    }

                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand INcmd = cn.CreateCommand();

                        INcmd.CommandText =
                           "INSERT INTO  SalesManage(RequestDate, SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat, UseTax, Price, Amount, CreateDate, IssueDate, ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, IssueState, ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey)" +
                           " SELECT getdate(), SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat*-1, UseTax, Price*-1, Amount*-1, getdate(), getdate(), ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, '취소', ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey FROM SalesManage WHERE SalesId = @SalesId";
                        // INcmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);
                        INcmd.Parameters.AddWithValue("@SalesId", Selected.SalesId);
                        INcmd.ExecuteNonQuery();
                        cn.Close();

                    }

                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmdOrder = cn.CreateCommand();
                        cmdOrder.CommandText =
                            "Update Orders SET SalesManageId = NULL WHERE SalesManageId = @SalesManageId";

                        cmdOrder.Parameters.AddWithValue("@SalesManageId", Selected.SalesId);
                        cmdOrder.ExecuteNonQuery();
                        cn.Close();
                    }

                    MessageBox.Show("발행 취소 되었습니다.");

                    btn_Search_Click(null, null);
                }
                catch (PopbillException ex)
                {
                    MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                    Error++;
                    btn_Search_Click(null, null);
                }
                //Success++;
            }
            catch (PopbillException ex)
            {
                MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                Error++;
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
            //    txt_AddrDetail.Text = String.Join(" ", ss.Skip(2));
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
                    txt_AddrDetail.Text = String.Join(" ", ss.Skip(2));
                    txt_AddrDetail.Focus();
                }
                else if (f.rdoJibun.Checked)
                {
                    var ss = f.Jibun.Split(' ');
                    txt_Zip.Text = f.Zip;
                    txt_State.Text = ss[0];
                    txt_City.Text = ss[1];
                    txt_AddrDetail.Text = String.Join(" ", ss.Skip(2));

                    txt_AddrDetail.Focus();
                }
            }
        }

        private void btn_New2_Click(object sender, EventArgs e)
        {
            FrmMN0212_SALESMANAGE_ADD2 _Form = new FrmMN0212_SALESMANAGE_ADD2();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var _Row = ((DataRowView)ordersBindingSource1.Current).Row as OrderDataSet.OrdersRow;
            //var _Row = ((DataRowView)ordersBindingSource1.Current).Row;
            if (e.ColumnIndex == ColumnNumber.Index)
            {
                e.Value = e.RowIndex + 1;
            }
            //else if (e.ColumnIndex == Column1Start.Index)
            //{


            //  //  var _Row = ordersBindingSource1[e.RowIndex];

            //    e.Value = String.Join("/", new object[] { _Row.StartState, _Row.StartCity, _Row.StartStreet }.Where(c => c != null && !String.IsNullOrWhiteSpace(c.ToString())));
            //}
            //else if (e.ColumnIndex == Column1Stop.Index)
            //{

            //    e.Value = String.Join("/", new object[] { _Row["StopState"], _Row["StopCity"], _Row["StopStreet"] }.Where(c => c != null && !String.IsNullOrWhiteSpace(c.ToString())));
            //}


        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {

        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();
           
        }

        private void txt_UnitPrice_Enter(object sender, EventArgs e)
        {
            txt_UnitPrice.Text = txt_UnitPrice.Text.Replace(",", "");
        }

        private void txt_Num_Enter(object sender, EventArgs e)
        {
            txt_Num.Text = txt_Num.Text.Replace(",", "");
        }

        private void btn_Issue_Click(object sender, EventArgs e)
        {
            if (salesManageBindingSource.Current == null)
                return;
            if (((DataRowView)salesManageBindingSource.Current).Row is SalesDataSet.SalesManageRow Selected && !Selected.Issue)
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                       $"UPDATE SalesManage SET IssueDate = @IssueDate , Issue = 1  , IssueState = '발행', HasAcc = 0 WHERE    SalesId ={Selected.SalesId}";
                    cmd.Parameters.AddWithValue("@IssueDate", dtp_PayDate.Value);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                Selected.IssueDate = dtp_PayDate.Value;
                Selected.Issue = true;
                Selected.IssueState = "발행";
                Selected.HasACC = false;
                salesManageBindingSource.ResetBindings(false);
            }
        }

        private void btnPayUpdate_Click(object sender, EventArgs e)
        {
            if (salesManageBindingSource.Current == null)
                return;




            var Row = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            if (Row == null)
                return;
            if (MessageBox.Show($"{Row.SangHo}\r{(Convert.ToDecimal(txt_InputPrice.Text)).ToString("N0")} 원\r정말 수금적용 하시겠습니까?", "화주 수금적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var totalsum = Row.InputPrice1 + Row.InputPrice2 + Row.InputPrice3 + Convert.ToDecimal(txt_InputPrice.Text);
            var inputSum = Row.Amount - totalsum;

            if (inputSum < 0)
            {
                MessageBox.Show("초과 수금 할수 없습니다.", "수금적용", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (Row.InputPrice1 != 0 && Row.InputPrice2 != 0 && Row.InputPrice3 == 0)
            {
                if (Row.Amount - totalsum > 0)
                {
                    MessageBox.Show("3차에는\r\n미수금 전액을 수금하여야 합니다.", "수금적용", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (Row.Amount - totalsum < 0)
                {
                    MessageBox.Show("초과 수금 할수 없습니다.", "수금적용", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();


                //  var totalsum = Row.InputPrice1 + Row.InputPrice2 + Row.InputPrice3 + Convert.ToDecimal(txt_InputPrice.Text);

                //   var inputSum = Row.Amount - totalsum;

                using (SqlCommand OrderCommand = cn.CreateCommand())
                {
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                     "UPDATE SalesManage SET PayDate = @PayDate , PayState = @PayState  " +
                    " ,InputPrice1 = @InputPrice1" +
                    " ,InputPrice2 = @InputPrice2" +
                    " ,InputPrice3 = @InputPrice3" +
                    " ,InputPrice1Date = @InputPrice1Date" +
                    " ,InputPrice2Date = @InputPrice2Date" +
                    " ,InputPrice3Date = @InputPrice3Date" +
                    " WHERE SalesId =" + Row.SalesId + " ";
                    cmd.Parameters.Add("@PayDate", SqlDbType.DateTime);
                    cmd.Parameters.Add("@PayState", SqlDbType.Int);
                    cmd.Parameters.Add("@InputPrice1", SqlDbType.Decimal);
                    cmd.Parameters.Add("@InputPrice1Date", SqlDbType.DateTime);
                    cmd.Parameters.Add("@InputPrice2", SqlDbType.Decimal);
                    cmd.Parameters.Add("@InputPrice2Date", SqlDbType.DateTime);
                    cmd.Parameters.Add("@InputPrice3", SqlDbType.Decimal);
                    cmd.Parameters.Add("@InputPrice3Date", SqlDbType.DateTime);

                    // var InputPrice = Row.InputPrice1 + Row.InputPrice2 + Row.InputPrice3;
                    if (Row.InputPrice1 == 0 && Row.InputPrice2 == 0 && Row.InputPrice3 == 0)
                    {
                        cmd.Parameters["@InputPrice1"].Value = Convert.ToDecimal(txt_InputPrice.Text.Replace(",", ""));
                        cmd.Parameters["@InputPrice1Date"].Value = dtp_PayDate.Value;

                        cmd.Parameters["@InputPrice2"].Value = 0;
                        cmd.Parameters["@InputPrice2Date"].Value = dtp_PayDate.Value;

                        cmd.Parameters["@InputPrice3"].Value = 0;
                        cmd.Parameters["@InputPrice3Date"].Value = dtp_PayDate.Value;



                    }
                    else if (Row.InputPrice1 != 0 && Row.InputPrice2 == 0 && Row.InputPrice3 == 0)
                    {
                        cmd.Parameters["@InputPrice1"].Value = Row.InputPrice1;
                        cmd.Parameters["@InputPrice1Date"].Value = Row.InputPrice1Date;

                        cmd.Parameters["@InputPrice2"].Value = Convert.ToDecimal(txt_InputPrice.Text.Replace(",", ""));
                        cmd.Parameters["@InputPrice2Date"].Value = dtp_PayDate.Value;

                        cmd.Parameters["@InputPrice3"].Value = 0;
                        cmd.Parameters["@InputPrice3Date"].Value = dtp_PayDate.Value;




                    }
                    else if (Row.InputPrice1 != 0 && Row.InputPrice2 != 0 && Row.InputPrice3 == 0)
                    {

                        cmd.Parameters["@InputPrice1"].Value = Row.InputPrice1;
                        cmd.Parameters["@InputPrice1Date"].Value = Row.InputPrice1Date;

                        cmd.Parameters["@InputPrice2"].Value = Row.InputPrice2;
                        cmd.Parameters["@InputPrice2Date"].Value = Row.InputPrice2Date;

                        cmd.Parameters["@InputPrice3"].Value = Convert.ToDecimal(txt_InputPrice.Text.Replace(",", ""));
                        cmd.Parameters["@InputPrice3Date"].Value = dtp_PayDate.Value;



                    }

                    if (inputSum == 0)
                    {
                        cmd.Parameters["@PayState"].Value = 1;
                        cmd.Parameters["@PayDate"].Value = dtp_PayDate.Value;

                    }
                    else
                    {
                        cmd.Parameters["@PayState"].Value = 2;
                        cmd.Parameters["@PayDate"].Value = DBNull.Value;

                    }

                    cmd.ExecuteNonQuery();




                }


                if (inputSum == 0)
                {
                    using (SqlCommand OrderCommand = cn.CreateCommand())
                    {
                        OrderCommand.CommandText =
                            @"UPDATE Orders
                                        SET CustomerPay = null
                                        , CustomerPayDate = @PayDate
                                        , CustomerPayPrice = @CustomerPayPrice
                                        , CustomerPayVAT = @CustomerPayVAT
                                        WHERE SalesManageId = @SalesManageId";
                        OrderCommand.Parameters.AddWithValue("@CustomerPayPrice", Row.Price);
                        OrderCommand.Parameters.AddWithValue("@CustomerPayVAT", Row.Vat);
                        OrderCommand.Parameters.AddWithValue("@SalesManageId", Row.SalesId);
                        OrderCommand.Parameters.AddWithValue("@PayDate", dtp_PayDate.Value);
                        
                        OrderCommand.ExecuteNonQuery();
                    }
                }



                using (SqlCommand cmdDriver = cn.CreateCommand())
                {

                    cmdDriver.CommandText =
                                    @" UPDATE Customers SET Misu =Misu -  @Misu WHERE CustomerId = @CustomerId  AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                    cmdDriver.Parameters.AddWithValue("@Misu", Convert.ToDecimal(txt_InputPrice.Text.Replace(",", "")));
                    cmdDriver.Parameters.AddWithValue("@CustomerId", Row.CustomerId);

                    cmdDriver.ExecuteNonQuery();
                }

                cn.Close();
            }

            //   MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "수금적용", 1), "매출관리 수정 성공");

            if (grid1.RowCount > 1)
            {
                GridIndex = salesManageBindingSource.Position;
                btn_Search_Click(null, null);
                grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];
            }
            else
            {
                btn_Search_Click(null, null);
            }


        }

        private void btnPayCancle_Click(object sender, EventArgs e)
        {
            if (salesManageBindingSource.Current == null)
                return;




            var Row = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            if (Row == null)
                return;
            if (MessageBox.Show($"{Row.SangHo}\r{((long)Row.Amount).ToString("N0")} 원\r정말 수금취소 하시겠습니까?", "화주 수금취소", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;




            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                using (SqlCommand OrderCommand = cn.CreateCommand())
                {
                    OrderCommand.CommandText =
                        @"UPDATE Orders
                                        SET CustomerPay = null
                                        , CustomerPayDate = null
                                        , CustomerPayPrice = null
                                        , CustomerPayVAT = null
                                        WHERE SalesManageId = @SalesManageId";
                    OrderCommand.Parameters.AddWithValue("@SalesManageId", Row.SalesId);
                    OrderCommand.ExecuteNonQuery();
                }

                using (SqlCommand cmd = cn.CreateCommand())
                {

                    cmd.CommandText =
                     "UPDATE SalesManage SET PayDate = @PayDate , PayState = @PayState" +
                     " ,InputPrice1 = @InputPrice1" +
                     " ,InputPrice2 = @InputPrice2" +
                     " ,InputPrice3 = @InputPrice3" +
                     " ,InputPrice1Date = @InputPrice1Date" +
                     " ,InputPrice2Date = @InputPrice2Date" +
                     " ,InputPrice3Date = @InputPrice3Date" +
                     "   WHERE    SalesId =" + Row.SalesId + " ";

                    cmd.Parameters.AddWithValue("@PayDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PayState", 2);

                    cmd.Parameters.AddWithValue("@InputPrice1", 0);
                    cmd.Parameters.AddWithValue("@InputPrice1Date", dtp_PayDate.Value);
                    cmd.Parameters.AddWithValue("@InputPrice2", 0);
                    cmd.Parameters.AddWithValue("@InputPrice2Date", dtp_PayDate.Value);
                    cmd.Parameters.AddWithValue("@InputPrice3", 0);
                    cmd.Parameters.AddWithValue("@InputPrice3Date", dtp_PayDate.Value);

                    cmd.ExecuteNonQuery();

                }
                using (SqlCommand cmdDriver = cn.CreateCommand())
                {

                    cmdDriver.CommandText =
                                    @" UPDATE Customers SET Misu =Misu +  @Misu WHERE CustomerId = @CustomerId  AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                    cmdDriver.Parameters.AddWithValue("@Misu", Row.Amount);
                    cmdDriver.Parameters.AddWithValue("@CustomerId", Row.CustomerId);

                    cmdDriver.ExecuteNonQuery();
                }

                cn.Close();
            }

            if (grid1.RowCount > 1)
            {
                GridIndex = salesManageBindingSource.Position;
                btn_Search_Click(null, null);
                grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];
            }
            else
            {
                btn_Search_Click(null, null);
            }
        }

        private void txt_InputPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_InputPrice_Enter(object sender, EventArgs e)
        {
            txt_InputPrice.Text = txt_InputPrice.Text.Replace(",", "");
        }

        private void txt_InputPrice_Leave(object sender, EventArgs e)
        {
            //decimal _InputPrice = decimal.Parse(txt_InputPrice.Text);

            // txtTotalInput.Text = _InputPrice 
            //    txt_Price.Text = _Price.ToString("N0");
            //    txt_VAT.Text = _Vat.ToString("N0");
            //    txt_Amount.Text = _Amount.ToString("N0");

        }

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //if (e.ColumnIndex == dataGridViewTextBoxColumn1.Index)
            //{
            //    e.Value = e.RowIndex + 1;
            //}
        }

        private void chk_EtaxCancle_CheckedChanged(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372110925");
        }
        private void rdoChk()
        {
            if (MethodProccess)
                return;
            if (!string.IsNullOrEmpty(txt_UnitPrice.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice.Text);
                decimal _Num = decimal.Parse(txt_Num.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                txt_UnitPrice.Text = _UnitPrice.ToString("N0");
                txt_Num.Text = _Num.ToString("N0");
            }
        }
        private void rdb_Tax1_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();

        }

        private void rdb_Tax2_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();
        }


        int _SalesId = 0;


        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesManageRow;



            //if(e.Button == MouseButtons.Right)
            //{
            //    if(Selected == null)
            //    {
            //        return;
            //    }
            //    if (Selected.Issue == true && Selected.IssueState != "취소")
            //    {
            //        contextMenuStrip1.Visible = false;
            //        return;
            //    }


            //}
            //if (e.Button == MouseButtons.Right)
            //{
            //    if (Selected.Issue == false || Selected.IssueState == "취소")
            //    {

            //        if (e.ColumnIndex == requestDateDataGridViewTextBoxColumn.Index)
            //        {

            //            Rectangle Rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, salesManageBindingSource.Position, true);
            //            m.Show(dataGridView1, new Point(Rectangle.X, Rectangle.Y));

            //        }
            //    }
            //}
            //else
            //{

            //    if (e.ColumnIndex == requestDateDataGridViewTextBoxColumn.Index)
            //    {
            //        requestDateDataGridViewTextBoxColumn.ReadOnly = true;
            //    }

            //    dtp.Visible = false;
            //}
        }

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMonth.SelectedIndex)
            {


                //당월
                case 0:
                    dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전월
                case 1:
                    dtp_Sdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 2:
                    dtp_Sdate.Value = DateTime.Now;
                    dtp_Edate.Value = DateTime.Now;
                    break;
            }
        }

        private void e세로ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            if (Selected.Issue != true)
            {
                IssueGubunEdit("e-세로", Selected.SalesId);

            }

        }

        private void 종이계산서ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            if (Selected.Issue != true)
            {
                IssueGubunEdit("종이계산서", Selected.SalesId);

            }
        }

        private void 발행안함ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            if (Selected.Issue != true)
            {
                IssueGubunEdit("발행안함", Selected.SalesId);

            }
        }

        private void IssueGubunEdit(string UGubun, int USalesId)
        {
            string _Gubun = UGubun;
            int _SalesId = USalesId;
            //switch (_Gubun)
            //{
            //    case "e-세로":

            //        break;
            //    case "종이계산서":

            //        break;
            //    case "발행안함":

            //        break;
            //}
            if (!String.IsNullOrEmpty(_Gubun))
            {
                if (_Gubun == "발행취소")
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {

                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        cmd.CommandText =
                             "UPDATE SalesManage SET IssueDate = NULL, Issue = 0, IssueState = '0' ,IssueLoginId = NULL, IssueGubun = NULL  WHERE  SalesId =" + _SalesId + " ";
                        //cmd.Parameters.AddWithValue("@IssueDate", dtp_RequestDate.Value);
                        //cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
                        //cmd.Parameters.AddWithValue("@IssueGubun", _Gubun);
                        cmd.ExecuteNonQuery();


                        cn.Close();
                    }

                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {

                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        cmd.CommandText =
                           "UPDATE SalesManage SET IssueDate = @IssueDate ,IssueLoginId = @IssueLoginId, IssueGubun = @IssueGubun,IssueState ='발행' WHERE    SalesId =" + _SalesId + " ";
                        cmd.Parameters.AddWithValue("@IssueDate", dtp_RequestDate.Value);
                        cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
                        cmd.Parameters.AddWithValue("@IssueGubun", _Gubun);
                        cmd.ExecuteNonQuery();


                        cn.Close();
                    }
                }


            }

            try
            {
                if (grid1.RowCount > 1)
                {
                    GridIndex = salesManageBindingSource.Position;
                    btn_Search_Click(null, null);
                    grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];
                }
                else
                {
                    btn_Search_Click(null, null);
                }
            }
            catch { }
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,

                SalesChk

            , No

            , requestDateDataGridViewTextBoxColumn
            , CardPayGubun
            , sangHoDataGridViewTextBoxColumn
            , bizNoDataGridViewTextBoxColumn
            , Stats
            , EmailCount

            , itemDataGridViewTextBoxColumn

            , priceDataGridViewTextBoxColumn
            , vatDataGridViewTextBoxColumn

            , AmountDataGridViewTextBoxColumn

            , IssueState
            , IssueDate1
            , ColumnIssueLoginId
            , ColumnIssueGubun
            , PayDate

            , ColumnAcceptCount
            , ColumnOrderText
            , Column1
            , EndDay

            , ColumnError

           );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);
        }

        private void 임의등록발행취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            if (Selected.Issue != true)
            {
                IssueGubunEdit("발행취소", Selected.SalesId);

            }
        }

        private void 거래명세서계좌변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitClientTable();
            var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            var _C = _ClientTable.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);


            textBox1.Text = Selected.SalesId.ToString();
          
            if (_C.Any())
            {

                var BankName = Filter.Bank.BankList.Where(c => c.Value == _C.First().CMSBankCode).ToArray().First().Text;

                if (!string.IsNullOrEmpty(Selected.CMSOwner) && !string.IsNullOrEmpty(Selected.CMSAccountNo) && !string.IsNullOrEmpty(Selected.CMSBankName))
                {
                    lblNowPayAccount.Text = "현재계좌 : " + Selected.CMSBankName + " / " + Selected.CMSAccountNo + " / " + Selected.CMSOwner;
                }
                else
                {

                    lblNowPayAccount.Text = "현재계좌 : " + BankName + " / " + _C.First().CMSAccountNo + " / " + _C.First().CMSOwner;
                }

                if (!string.IsNullOrEmpty(BankName) && !string.IsNullOrEmpty(_C.First().CMSAccountNo) && !string.IsNullOrEmpty(_C.First().CMSOwner))
                {


                    PayAccountSelect.Items.Clear();
                    int ItemIndex = 0;



                    foreach (var _Client in _C)
                    {
                        var _CMSBankName = Filter.Bank.BankList.Where(c => c.Value == _Client.CMSBankCode).ToArray().First().Text;




                        PayAccountSelect.Items.Add(_CMSBankName);


                        PayAccountSelect.Items[ItemIndex].SubItems.Add(_Client.CMSAccountNo);
                        PayAccountSelect.Items[ItemIndex].SubItems.Add(_Client.CMSOwner);
                        PayAccountSelect.Items[ItemIndex].Tag = _Client.CMSBankCode;
                        ItemIndex++;
                    }


                    if (PayAccountSelect.Items.Count > 0)
                    {
                        PayAccountSelect.Items[0].Selected = true;
                    }

                    
                    PayAccountSelectContainer.Location = new Point(521, 191);
                    PayAccountSelectContainer.Visible = true;
                    PayAccountSelect.Focus();
                }
            }
            else
            {

            }

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (salesManageBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;
            if (Selected.Issue == true)
            {
                MessageBox.Show("세금계산서가 이미 발행 되었습니다.", "매출관리", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //   MessageBox.Show("세금계산서가 이미 발행 되었습니다.");


                e.Cancel = true;

            }




        }

        private void SetMouseClick(Control mControl)
        {
            foreach (Control nControl in mControl.Controls)
            {
                if (nControl == PayAccountSelect)
                {
                    continue;
                }
                nControl.MouseClick += NControl_MouseClick;
                SetMouseClick(nControl);
            }
        }

        private void NControl_MouseClick(object sender, MouseEventArgs e)
        {
            PayAccountSelectContainer.Visible = false;

        }

        private void PayAccountSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (PayAccountSelect.SelectedItems.Count > 0)
                {
                    _PayAccountSelected();
                }
            }
        }

        private void PayAccountSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in PayAccountSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _PayAccountSelected();
                    }
                }
            }
        }

        private void _PayAccountSelected()
        {



            var _PaybankName = PayAccountSelect.SelectedItems[0].Text;
            var _PayAccountNo = PayAccountSelect.SelectedItems[0].SubItems[1].Text;
            var _PayInputName = PayAccountSelect.SelectedItems[0].SubItems[2].Text;
            var _PaybankCode = PayAccountSelect.SelectedItems[0].Tag;



            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {

                cn.Open();
                SqlCommand cmd = cn.CreateCommand();

                cmd.CommandText =
                   "UPDATE SalesManage SET CMSBankName = @CMSBankName,CMSAccountNo = @CMSAccountNo,CMSOwner = @CMSOwner  WHERE    SalesId =" + textBox1.Text + " ";
                cmd.Parameters.AddWithValue("@CMSBankName", _PaybankName);
                cmd.Parameters.AddWithValue("@CMSAccountNo", _PayAccountNo);
                cmd.Parameters.AddWithValue("@CMSOwner", _PayInputName);
                cmd.ExecuteNonQuery();


                cn.Close();
            }

            PayAccountSelectContainer.Visible = false;


            btn_Search_Click(null, null);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            PayAccountSelectContainer.Visible = false;
        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
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
                        if (_S.Length == 9)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                        else
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
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

        private void button2_Click(object sender, EventArgs e)
        {
            #region 전자세금계산서 체크박스
            var _Clients = LocalUser.Instance.LogInInformation.Client;
            var Datas = new List<SalesDataSet.SalesManageRow>();
            for (int i = 0; i < grid1.RowCount; i++)
            {
                var _Cell = grid1[SalesChk.Index, i] as DataGridViewDisableCheckBoxCell;
                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                    var _Row = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                    _Row.RejectChanges();

                   // _Row.IssueDate = DateTime.Now;
                    Datas.Add(_Row);
                }
            }


         
            SendMailDialogMessageBox Smdm = new SendMailDialogMessageBox();

            if (Smdm.ShowDialog() == DialogResult.Yes)
            {
                if (Datas.Any())
                {

                    if (MessageBox.Show($"선택건수 :{Datas.Count()}건\r\n메일로 전송하시겠습니까?", "거래명세서 메일전송", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MailSuccess = 0;
                        MailFail = 0;

                        pnProgress.Visible = true;
                        bar.Value = 0;
                        bar.Maximum = Datas.Count;
                        Thread t = new Thread(new ThreadStart(() =>
                        {
                            foreach (var Stats in Datas.ToArray())
                            {


                                StatsSendEmail(Stats.SalesId);
                            }
                            pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

                            try
                            {
                                pnProgress.Invoke(new Action(() => MessageBox.Show("메일전송이 완료되었습니다 \r\n\r\n성공 : '" + MailSuccess + "'건\r\n실패 : '" + MailFail + "'건", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));
                                pnProgress.Invoke(new Action(() => _Search()));
                            }
                            catch
                            {
                                pnProgress.Invoke(new Action(() => _Search()));
                            }
                        }));
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();




                    }





                }
                else
                {
                    MessageBox.Show("메일로 전송할 거래명세서 항목들을 선택하여 주십시오.", "거래명세서 메일전송");
                }
            }
            else if (Smdm.DialogResult == DialogResult.OK)
            {

                if (Datas.Where(c => c.billNo != "").ToArray().Count() > 0)
                {

                    if (MessageBox.Show($"선택건수 :{Datas.Where(c => c.billNo != "").Count()}건\r\n메일로 전송하시겠습니까?", "전자세금계산서 메일재전송", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string BillNos = "";
                        string Emails = "";

                        MailSuccess = 0;
                        MailFail = 0;

                        pnProgress.Visible = true;
                        bar.Value = 0;
                        bar.Maximum = Datas.Where(c => c.billNo != "").Count();
                        Thread t = new Thread(new ThreadStart(() =>
                        {
                            foreach (var Stats in Datas.Where(c => c.billNo != "").ToArray())
                            {
                                BillNos += Stats.billNo + ";";
                                Emails += Stats.Email + ";";

                                // _Clients.frnNo, _Clients.userid, _Clients.passwd, certPw

                            }

                            dTIServiceClass.sendMultiMail2(linkcd, _Clients.frnNo, _Clients.userid, _Clients.passwd, BillNos, Emails);


                            pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

                            try
                            {
                                pnProgress.Invoke(new Action(() => MessageBox.Show("메일재전송이 완료되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));
                                pnProgress.Invoke(new Action(() => _Search()));
                            }
                            catch
                            {
                                pnProgress.Invoke(new Action(() => _Search()));
                            }
                        }));
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();




                    }





                }
                else
                {
                    MessageBox.Show("메일로 전송할 전자세금계산서를 선택하여 주십시오.", "전자세금계산서 메일재전송");
                }
            }
            else
            {


            }






            #endregion




        }
        int MailSuccess = 0;
        int MailFail = 0;
        private void StatsSendEmail(int SalesId)
        {
            

            salesReportTableAdapter.Fill(this.salesDataSet.SalesReport, SalesId);
            salesReportBindingSource1.DataSource = salesDataSet.SalesReport;
            #region epplus 데이터 내보내기

            string title = string.Empty;
            byte[] ieExcel;
            LocalUser.Instance.LogInInformation.LoadClient();
            _StatsGubun = LocalUser.Instance.LogInInformation.Client.StatsGubun;
            ieExcel = Properties.Resources.SalesListRemarkDriver_blank;


            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);

            if (di.Exists == false)
            {
                di.Create();
            }
            //var fileString = string.Format("Stats{0}-{1}", dtp_Sdate.Text.Replace("/", ""), dtp_Edate.Text.Replace("/", ""))+ "_"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var fileString = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                file.Delete();

            }


            //switch (_StatsGubun)

            //{
            //    case 0:
            //        ieExcel = Properties.Resources.SalesList_blank;
            //        break;
            //    case 1:
            //        ieExcel = Properties.Resources.SalesList_1_blank;
            //        break;
            //    case 2:
            //        ieExcel = Properties.Resources.SalesListRemark_blank;
            //        break;
            //    case 3:
            //        ieExcel = Properties.Resources.SalesListRemark_1_blank;
            //        break;
            //    case 4:
            //        ieExcel = Properties.Resources.SalesListRemarkDriver_blank;
            //        break;
            //}


            //if (_StatsGubun == 5)
            //{
            //    ieExcel = Properties.Resources.SalesListRemarkDriver_blank;
            //}



            try
            {

                #region 엑셀저장
               
                ReportViewer report = new ReportViewer();


                LocalUser.Instance.LogInInformation.LoadClient();
                _StatsGubun = LocalUser.Instance.LogInInformation.Client.StatsGubun;
                var _ReportEmbeddedResource = "";
                switch (_StatsGubun)
                {
                    case 0:
                        _ReportEmbeddedResource = "mycalltruck.Admin.SalesList.rdlc";
                        break;

                    case 1:
                        _ReportEmbeddedResource = "mycalltruck.Admin.SalesList-1.rdlc";
                        break;
                    case 2:
                        _ReportEmbeddedResource = "mycalltruck.Admin.SalesListRemark.rdlc";
                        break;
                    case 3:
                        _ReportEmbeddedResource = "mycalltruck.Admin.SalesListRemark-1.rdlc";
                        break;
                    case 4:
                        _ReportEmbeddedResource = "mycalltruck.Admin.SalesListRemarkDriver.rdlc";
                        break;

                }




                var _Q = salesDataSet.SalesManage.Where(c => c.SalesId == SalesId).ToArray();

                var HAmount = "합계금액 : " + _Q.First().Amount.ToString("N0") + " (금 " + Number2Hangle((long)_Q.First().Amount) + "원정)";

                
                BindingSource tempBindingSource = new BindingSource(salesReportBindingSource1, "");

                Microsoft.Reporting.WinForms.ReportDataSource _Source = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", tempBindingSource);
                report.LocalReport.ReportEmbeddedResource = _ReportEmbeddedResource;
                report.LocalReport.DataSources.Add(_Source);
                report.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("HAmount", HAmount));
                report.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


                report.RefreshReport();




                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = report.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                FileStream fs = new FileStream(FileName, FileMode.Create);
                //Response.Buffer = true;
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                #endregion

                #region 엑셀저장
                //File.WriteAllBytes(FileName, ieExcel);

                //using (OfficeOpenXml.ExcelPackage excelApp = new OfficeOpenXml.ExcelPackage(file, true))
                //{



                //    OfficeOpenXml.ExcelWorksheet ws = excelApp.Workbook.Worksheets[1];




                //    int indexCol = 15;

                //    int linhaCorrente = 0;
                //    int rownum = 0;

                //    switch (_StatsGubun)
                //    {
                //        case 0:
                //            foreach (DataGridViewRow row in newDGV4.Rows)
                //            {


                //                linhaCorrente = 2;
                //                var Query = salesDataSet.SalesReport.ToArray();
                //                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                //                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                //                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                //                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                //                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                //                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                //                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                //                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                //                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                //                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                //                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                //                if (Query.Any())
                //                {
                //                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                //                    ws.Cells[6, 10].Value = Query.First().SangHo;

                //                    ws.Cells[7, 10].Value = Query.First().CCeo;
                //                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                //                    ws.Cells[8, 10].Value = Query.First().CUptae;
                //                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                //                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                //                    foreach (DataGridViewColumn col in newDGV4.Columns)
                //                    {


                //                        if (col.Visible == true)
                //                        {

                //                            decimal decimalresult = 0;


                //                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                //                            if (linhaCorrente == 2)
                //                            {
                //                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                //                            }
                //                            //if (linhaCorrente == 7)
                //                            //{
                //                            //    ws.Cells[indexCol, linhaCorrente].Value =  col.;
                //                            //}
                //                            linhaCorrente++;
                //                        }

                //                    }


                //                    indexCol++;
                //                }


                //            }
                //            break;

                //        case 1:
                //            foreach (DataGridViewRow row in newDGV5.Rows)
                //            {


                //                linhaCorrente = 2;
                //                var Query = salesDataSet.SalesReport.ToArray();
                //                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                //                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                //                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                //                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                //                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                //                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                //                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                //                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                //                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                //                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                //                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                //                if (Query.Any())
                //                {
                //                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                //                    ws.Cells[6, 10].Value = Query.First().SangHo;

                //                    ws.Cells[7, 10].Value = Query.First().CCeo;
                //                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                //                    ws.Cells[8, 10].Value = Query.First().CUptae;
                //                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                //                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                //                    foreach (DataGridViewColumn col in newDGV5.Columns)
                //                    {


                //                        if (col.Visible == true)
                //                        {

                //                            decimal decimalresult = 0;


                //                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                //                            if (linhaCorrente == 2)
                //                            {
                //                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                //                            }

                //                            linhaCorrente++;
                //                        }

                //                    }


                //                    indexCol++;
                //                }


                //            }
                //            break;

                //        case 2:
                //            foreach (DataGridViewRow row in newDGV6.Rows)
                //            {


                //                linhaCorrente = 2;
                //                var Query = salesDataSet.SalesReport.ToArray();
                //                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                //                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                //                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                //                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                //                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                //                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                //                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                //                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                //                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                //                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                //                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                //                if (Query.Any())
                //                {
                //                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                //                    ws.Cells[6, 10].Value = Query.First().SangHo;

                //                    ws.Cells[7, 10].Value = Query.First().CCeo;
                //                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                //                    ws.Cells[8, 10].Value = Query.First().CUptae;
                //                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                //                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                //                    foreach (DataGridViewColumn col in newDGV6.Columns)
                //                    {


                //                        if (col.Visible == true)
                //                        {

                //                            decimal decimalresult = 0;


                //                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                //                            if (linhaCorrente == 2)
                //                            {
                //                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                //                            }

                //                            linhaCorrente++;
                //                        }

                //                    }


                //                    indexCol++;
                //                }


                //            }
                //            break;

                //        case 3:
                //            foreach (DataGridViewRow row in newDGV7.Rows)
                //            {


                //                linhaCorrente = 2;
                //                var Query = salesDataSet.SalesReport.ToArray();
                //                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                //                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                //                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                //                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                //                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                //                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                //                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                //                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                //                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                //                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                //                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                //                if (Query.Any())
                //                {
                //                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                //                    ws.Cells[6, 10].Value = Query.First().SangHo;

                //                    ws.Cells[7, 10].Value = Query.First().CCeo;
                //                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                //                    ws.Cells[8, 10].Value = Query.First().CUptae;
                //                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                //                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                //                    foreach (DataGridViewColumn col in newDGV7.Columns)
                //                    {


                //                        if (col.Visible == true)
                //                        {

                //                            decimal decimalresult = 0;


                //                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                //                            if (linhaCorrente == 2)
                //                            {
                //                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                //                            }

                //                            linhaCorrente++;
                //                        }

                //                    }


                //                    indexCol++;
                //                }


                //            }
                //            break;

                //        case 4:
                //            foreach (DataGridViewRow row in newDGV3.Rows)
                //            {


                //                linhaCorrente = 2;
                //                var Query = salesDataSet.SalesReport.ToArray();
                //                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                //                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                //                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                //                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                //                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                //                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                //                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                //                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                //                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                //                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                //                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                //                if (Query.Any())
                //                {
                //                    ws.Cells[5, 11].Value = Query.First().CBizNo;
                //                    ws.Cells[6, 11].Value = Query.First().SangHo;

                //                    ws.Cells[7, 11].Value = Query.First().CCeo;
                //                    ws.Cells[7, 13].Value = Query.First().CPhoneNo;

                //                    ws.Cells[8, 11].Value = Query.First().CUptae;
                //                    ws.Cells[8, 13].Value = Query.First().CUpjong;
                //                    ws.Cells[9, 11].Value = Query.First().CustomerAddr;
                //                    foreach (DataGridViewColumn col in newDGV3.Columns)
                //                    {


                //                        if (col.Visible == true)
                //                        {

                //                            decimal decimalresult = 0;
                //                            if (linhaCorrente == 10)
                //                                linhaCorrente = linhaCorrente + 1;

                //                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                //                            if (linhaCorrente == 2)
                //                            {
                //                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                //                            }
                //                            //if (linhaCorrente == 1)
                //                            //{
                //                            //    ws.Cells[indexCol, linhaCorrente].Value = row.Cells[35].FormattedValue.ToString()+"/" + row.Cells[36].FormattedValue.ToString()+"/"+ row.Cells[24].FormattedValue.ToString();
                //                            //}
                //                            linhaCorrente++;
                //                        }

                //                    }


                //                    indexCol++;
                //                }


                //            }
                //            break;
                //    }




                //    var Query2 = salesDataSet.SalesReport.ToArray();
                //    ws.Cells[indexCol + 1, 3].Value = "<합계>";
                //    ws.Cells[indexCol + 1, 11].Value = Query2.Sum(c => c.Amont).ToString("N0");

                //    excelApp.Workbook.Properties.Title = title;
                //    excelApp.Workbook.Properties.Author = "edubill";
                //    excelApp.Workbook.Properties.Comments = "";
                //    excelApp.Workbook.Properties.Company = "에듀빌";


                //    excelApp.SaveAs(file);

                //    title = null;



                //}
                //GC.GetTotalMemory(false);
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
                //GC.GetTotalMemory(true);

                #endregion

                System.Net.WebClient wcClient = new System.Net.WebClient();


                wcClient.UploadFile("http://222.231.9.253:8080/Newchasero/FileUpload.aspx", "POST", FileName);

                // System.Diagnostics.Process.Start(FileName);
            }
            catch (Exception E)
            {
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);
            }

            try
            {
                file.Delete();

                var sFile = $"http://222.231.9.253:8080/Newchasero/StatsUpload/" + fileString;
                var sFileName = fileString;
                string sName, sEmail, sMobile, sNote1,sClientName, sgigan,scount,sAmount;
                sClientName = LocalUser.Instance.LogInInformation.Client.Name;
                sgigan = "- 기간 : " + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd");
                scount = "- 건수 : " + newDGV3.RowCount.ToString() + " 건";
               
                var Query = salesDataSet.SalesReport.ToArray();
                //if (Query.Any())
                //{
                //    sName = Query.First().SangHo;

                //    sEmail = txt_Email.Text;
                //    sMobile = Query.First().PhoneNo;
                //    sNote1 = Query.First().RequestMemo;
                //}
                //else
                //{
                var Q = salesDataSet.SalesManage.Where(c => c.SalesId == SalesId).ToArray();
                sName = Q.First().SangHo;
                sEmail = Q.First().Email;
                sMobile = Q.First().MobileNo;
                sNote1 = "";

                //}
                sAmount = "- 청구금액 : " + Query.Sum(c => c.Amont).ToString("N0") +" 원(VAT별도)";
                




                #region
                string Parameter = "";
          

                StringBuilder postParams = new StringBuilder();
                postParams.Append($"sName={sName}");
                postParams.Append($"&sEmail={sEmail}");
                postParams.Append($"&sMobile={sMobile}");
                postParams.Append($"&sFile_url={sFile}");
                postParams.Append($"&sFileName={sFileName}");
                postParams.Append($"&sClientName={sClientName}");
                postParams.Append($"&sgigan={sgigan}");
                postParams.Append($"&scount={scount}");
                postParams.Append($"&sAmount={sAmount}");



                JObject response = null;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://cardpay.kr/directsend_mail_change_word_asp_api.asp?" + postParams);



                request.Method = "GET";
                request.ContentType = "pplication/json;charset=utf-8;";
                request.ContentLength = 0;

                request.Headers.Add("header-staff-api", "value");

                var httpResponse = (HttpWebResponse)request.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = JObject.Parse(streamReader.ReadToEnd());

                  

                }
                #endregion


                if (response["status"].ToString() == "0")
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText =
                            "Update SalesManage SET EmailCount = EmailCount+1 WHERE SalesId = @SalesId";

                        cmd.Parameters.AddWithValue("@SalesId", SalesId);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                    MailSuccess++;
                    //MessageBox.Show("메일전송이 완료되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MailFail++;
                }
              
            }
            catch(Exception ex)
            {

                //MessageBox.Show(ex.Message);
                MailFail++;

            }
            #endregion

            //try { MessageBox.Show("메일전송이 완료되었습니다 \r\n\r\n성공 : '" + MailSuccess + "'건\r\n실패 : '" + MailFail + "'건", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
            //catch { MessageBox.Show("연결된 인터넷전화(LG U+)가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
        }
        private void newDGV3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == ColumnNum.Index)
            {
                newDGV3[e.ColumnIndex, e.RowIndex].Value = (newDGV3.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
        }

        private void SendMail()
        {
            try
            {
                //var Url = $"http://www.juso.go.kr/addrlink/addrLinkApi.do?resultType=json&countPerPage=100&keyword={_Address}&confmKey=U01TX0FVVEgyMDE2MTIyMjE2MTY0NjE3NTky";
                //WebClient _WebClient = new WebClient();
                //_WebClient.Encoding = Encoding.UTF8;
                //var r = _WebClient.DownloadString(Url);
                //dynamic _Array = JsonConvert.DeserializeObject(r);
                //if (_Array.results != null)
                //{
                //    dynamic jusoArray = _Array.results.juso;
                //    foreach (var juso in jusoArray)
                //    {
                //        var roadAddr = juso.roadAddr.ToString();
                //        var zipNo = juso.zipNo.ToString();
                //        var jibunAddr = juso.jibunAddr;
                //        _CoreList.Add(new ModelDefalut
                //        {
                //            Zip = zipNo,
                //            Address = roadAddr,
                //            Jibun = jibunAddr,
                //        });
                //    }
                //}

            }
            catch (Exception)
            {

            }
        }

        private void newDGV4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)newDGV4.Rows[e.RowIndex].DataBoundItem).Row as SalesDataSet.SalesReportRow;

            if (e.ColumnIndex == ColumnNum.Index)
            {
                newDGV4[e.ColumnIndex, e.RowIndex].Value = (newDGV4.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            //if (e.ColumnIndex == dataGridViewTextBoxColumn21.Index)
            //{
            //    newDGV4[e.ColumnIndex, e.RowIndex].Value = Selected.CarsizeName + "/" + Selected.CarTypeName;
            //}
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            salesReportTableAdapter.Fill(this.salesDataSet.SalesReport, 6935);
            salesReportBindingSource1.DataSource = salesDataSet.SalesReport;


            string title = string.Empty;
            byte[] ieExcel;
            LocalUser.Instance.LogInInformation.LoadClient();
            _StatsGubun = LocalUser.Instance.LogInInformation.Client.StatsGubun;
            ieExcel = Properties.Resources.SalesListRemarkDriver_blank;


            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);

            if (di.Exists == false)
            {
                di.Create();
            }
            //var fileString = string.Format("Stats{0}-{1}", dtp_Sdate.Text.Replace("/", ""), dtp_Edate.Text.Replace("/", ""))+ "_"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var fileString = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                file.Delete();

            }


            switch (comboBox1.SelectedIndex)

            {
                case 0:
                    ieExcel = Properties.Resources.SalesList_blank;
                    break;
                case 1:
                    ieExcel = Properties.Resources.SalesList_1_blank;
                    break;
                case 2:
                    ieExcel = Properties.Resources.SalesListRemark_blank;
                    break;
                case 3:
                    ieExcel = Properties.Resources.SalesListRemark_1_blank;
                    break;
                case 4:
                    ieExcel = Properties.Resources.SalesListRemarkDriver_blank;
                    break;
            }



            try
            {




                File.WriteAllBytes(FileName, ieExcel);

                using (OfficeOpenXml.ExcelPackage excelApp = new OfficeOpenXml.ExcelPackage(file, true))
                {



                    OfficeOpenXml.ExcelWorksheet ws = excelApp.Workbook.Worksheets[1];




                    int indexCol = 15;

                    int linhaCorrente = 0;
                    int rownum = 0;

                    switch (comboBox1.SelectedIndex)
                    {
                        case 0:
                            foreach (DataGridViewRow row in newDGV4.Rows)
                            {


                                linhaCorrente = 2;
                                var Query = salesDataSet.SalesReport.ToArray();
                                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                                if (Query.Any())
                                {
                                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                                    ws.Cells[6, 10].Value = Query.First().SangHo;

                                    ws.Cells[7, 10].Value = Query.First().CCeo;
                                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                                    ws.Cells[8, 10].Value = Query.First().CUptae;
                                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                                    foreach (DataGridViewColumn col in newDGV4.Columns)
                                    {


                                        if (col.Visible == true)
                                        {

                                            decimal decimalresult = 0;


                                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                                            if (linhaCorrente == 2)
                                            {
                                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                                            }
                                            //if (linhaCorrente == 7)
                                            //{
                                            //    ws.Cells[indexCol, linhaCorrente].Value =  col.;
                                            //}
                                            linhaCorrente++;
                                        }

                                    }


                                    indexCol++;
                                }


                            }
                            break;

                        case 1:
                            foreach (DataGridViewRow row in newDGV5.Rows)
                            {


                                linhaCorrente = 2;
                                var Query = salesDataSet.SalesReport.ToArray();
                                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                                if (Query.Any())
                                {
                                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                                    ws.Cells[6, 10].Value = Query.First().SangHo;

                                    ws.Cells[7, 10].Value = Query.First().CCeo;
                                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                                    ws.Cells[8, 10].Value = Query.First().CUptae;
                                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                                    foreach (DataGridViewColumn col in newDGV5.Columns)
                                    {


                                        if (col.Visible == true)
                                        {

                                            decimal decimalresult = 0;


                                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                                            if (linhaCorrente == 2)
                                            {
                                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                                            }

                                            linhaCorrente++;
                                        }

                                    }


                                    indexCol++;
                                }


                            }
                            break;

                        case 2:
                            foreach (DataGridViewRow row in newDGV6.Rows)
                            {


                                linhaCorrente = 2;
                                var Query = salesDataSet.SalesReport.ToArray();
                                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                                if (Query.Any())
                                {
                                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                                    ws.Cells[6, 10].Value = Query.First().SangHo;

                                    ws.Cells[7, 10].Value = Query.First().CCeo;
                                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                                    ws.Cells[8, 10].Value = Query.First().CUptae;
                                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                                    foreach (DataGridViewColumn col in newDGV6.Columns)
                                    {


                                        if (col.Visible == true)
                                        {

                                            decimal decimalresult = 0;


                                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                                            if (linhaCorrente == 2)
                                            {
                                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                                            }

                                            linhaCorrente++;
                                        }

                                    }


                                    indexCol++;
                                }


                            }
                            break;

                        case 3:
                            foreach (DataGridViewRow row in newDGV7.Rows)
                            {


                                linhaCorrente = 2;
                                var Query = salesDataSet.SalesReport.ToArray();
                                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                                if (Query.Any())
                                {
                                    ws.Cells[5, 10].Value = Query.First().CBizNo;
                                    ws.Cells[6, 10].Value = Query.First().SangHo;

                                    ws.Cells[7, 10].Value = Query.First().CCeo;
                                    ws.Cells[7, 12].Value = Query.First().CPhoneNo;

                                    ws.Cells[8, 10].Value = Query.First().CUptae;
                                    ws.Cells[8, 12].Value = Query.First().CUpjong;
                                    ws.Cells[9, 10].Value = Query.First().CustomerAddr;
                                    foreach (DataGridViewColumn col in newDGV7.Columns)
                                    {


                                        if (col.Visible == true)
                                        {

                                            decimal decimalresult = 0;


                                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                                            if (linhaCorrente == 2)
                                            {
                                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                                            }

                                            linhaCorrente++;
                                        }

                                    }


                                    indexCol++;
                                }


                            }
                            break;

                        case 4:
                            foreach (DataGridViewRow row in newDGV3.Rows)
                            {


                                linhaCorrente = 2;
                                var Query = salesDataSet.SalesReport.ToArray();
                                ws.Cells[3, 2].Value = "(" + dtp_Sdate.Value.Date.ToString("yyyy-MM-dd") + "~" + dtp_Edate.Value.Date.ToString("yyyy-MM-dd") + ")";
                                ws.Cells[5, 4].Value = LocalUser.Instance.LogInInformation.Client.BizNo;
                                ws.Cells[6, 4].Value = LocalUser.Instance.LogInInformation.Client.Name;
                                ws.Cells[7, 4].Value = LocalUser.Instance.LogInInformation.Client.CEO;
                                ws.Cells[7, 6].Value = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                                ws.Cells[8, 4].Value = LocalUser.Instance.LogInInformation.Client.Uptae;
                                ws.Cells[8, 6].Value = LocalUser.Instance.LogInInformation.Client.Upjong;
                                ws.Cells[9, 4].Value = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity + " " + LocalUser.Instance.LogInInformation.Client.AddressDetail;
                                ws.Cells[11, 2].Value = " 아래와　같이　계산합니다． 팩스번호 : " + LocalUser.Instance.LogInInformation.Client.FaxNo;
                                ws.Cells[12, 2].Value = "합계금액 : " + Query.Sum(c => c.Amont).ToString("N0") + " (금 " + Number2Hangle((long)Query.Sum(c => c.Amont)) + "원정)";

                                ws.Cells[13, 2].Value = LocalUser.Instance.LogInInformation.Client.CMSBankName + "/" + LocalUser.Instance.LogInInformation.Client.CMSAccountNo + "/" + LocalUser.Instance.LogInInformation.Client.CMSOwner;

                                if (Query.Any())
                                {
                                    ws.Cells[5, 11].Value = Query.First().CBizNo;
                                    ws.Cells[6, 11].Value = Query.First().SangHo;

                                    ws.Cells[7, 11].Value = Query.First().CCeo;
                                    ws.Cells[7, 13].Value = Query.First().CPhoneNo;

                                    ws.Cells[8, 11].Value = Query.First().CUptae;
                                    ws.Cells[8, 13].Value = Query.First().CUpjong;
                                    ws.Cells[9, 11].Value = Query.First().CustomerAddr;
                                    foreach (DataGridViewColumn col in newDGV3.Columns)
                                    {


                                        if (col.Visible == true)
                                        {

                                            decimal decimalresult = 0;
                                            if (linhaCorrente > 9)
                                                linhaCorrente = linhaCorrente + 1;

                                            ws.Cells[indexCol, linhaCorrente].Value = row.Cells[col.Index].FormattedValue.ToString();
                                            if (linhaCorrente == 2)
                                            {
                                                ws.Cells[indexCol, linhaCorrente].Value = rownum + 1;
                                            }

                                            linhaCorrente++;
                                        }

                                    }


                                    indexCol++;
                                }


                            }
                            break;
                    }




                    var Query2 = salesDataSet.SalesReport.ToArray();
                    ws.Cells[indexCol + 1, 3].Value = "<합계>";
                    ws.Cells[indexCol + 1, 11].Value = Query2.Sum(c => c.Amont).ToString("N0");

                    excelApp.Workbook.Properties.Title = title;
                    excelApp.Workbook.Properties.Author = "edubill";
                    excelApp.Workbook.Properties.Comments = "";
                    excelApp.Workbook.Properties.Company = "에듀빌";


                    excelApp.SaveAs(file);

                    title = null;



                }
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);



                // System.Net.WebClient wcClient = new System.Net.WebClient();


                // wcClient.UploadFile("http://222.231.9.253/Newchasero/FileUpload.aspx", "POST", FileName);

                System.Diagnostics.Process.Start(FileName);
            }
            catch (Exception E)
            {
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);
            }
        }
        //승인번호
        string _billNo = "";
        //1.문서상태변경 2.신고상태변경
        string _changestatus = "";
        //문서상태 코드 또는 신고상태 코드
        string _changestatusCode = "";
        //국세청응답코드
        string _EtaxCode = "";
        private void button4_Click(object sender, EventArgs e)
        {
            
           
            string sParameter = "";
            string EncodingSparameter = "";

           // var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;



            //if (Selected != null)
            //{
                //현재시간(년월일시분초);공급자사업자번호;공급받는자사업자번호;연계코드
                sParameter = DateTime.Now.ToString("yyyyMMddHHmmss") + ";" + LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", "") + ";" + "" + ";" + "EDB";
                EncodingSparameter = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { sParameter }).ToString();


                // url = "http://t-renewal.nicedata.co.kr/ti/TI_80401.do?";

              
                string value = $"data={EncodingSparameter}";

                AdminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
                var Query = baseDataSet.AdminInfoes.FirstOrDefault();
                var url = "http://t-renewal.nicedata.co.kr/ti/TI_80401.do?";
                if (Query.IsTest != true)
                {
                    url = "http://www.nicedata.co.kr/ti/TI_80401.do?";
                }


            //FrmNice f = new FrmNice(value, url);
            //f.Show();
            
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Internet Explorer\\iexplore.exe", url + value);



            //}


        }

        private void NiceEtaxEdit(int salesid)
        {
            var _Clients = LocalUser.Instance.LogInInformation.Client;
            var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";


            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.NPKIpasswd }).ToString();




            string DutyNum = string.Empty;
            string DescriptionText = "";
            int i = 0;

            var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
            if (_HideAccountNo == 0)
            {
                DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner + "/" + _Clients.CMSAccountNo;
            }
            else
            {
                DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner;

            }
            var _Query = salesDataSet.SalesManage.Where(c => c.SalesId == salesid).FirstOrDefault();

            decimal _Price = 0;
            decimal _Vat = 0;
            decimal _Amont = 0;
            var DetailQuery1 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName1) && c.SalesId == _Query.SalesId).ToArray();
            var DetailQuery2 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName2) && c.SalesId == _Query.SalesId).ToArray();
            var DetailQuery3 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName3) && c.SalesId == _Query.SalesId).ToArray();
            var DetailQuery4 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName4) && c.SalesId == _Query.SalesId).ToArray();
            if (DetailQuery1.Any() && DetailQuery2.Any() && DetailQuery3.Any() && DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2 + DetailQuery3.First().unitCost3 + DetailQuery4.First().unitCost4;
                _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2 + DetailQuery3.First().tax3 + DetailQuery4.First().tax4;
            }
            else if (DetailQuery1.Any() && DetailQuery2.Any() && DetailQuery3.Any() && !DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2 + DetailQuery3.First().unitCost3;
                _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2 + DetailQuery3.First().tax3;
            }
            else if (DetailQuery1.Any() && DetailQuery2.Any() && !DetailQuery3.Any() && !DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2;
                _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2;
            }
            else if (DetailQuery1.Any() && !DetailQuery2.Any() && !DetailQuery3.Any() && !DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1;
                _Vat = DetailQuery1.First().tax1;
            }
            else
            {
                _Price = _Query.Price * -1;
                _Vat = _Query.Vat * -1;
            }
            string _TypeCode = "0201";
            if (_Query.TypeCode == "위수탁")
            {
                if (Convert.ToInt64(_Vat) == 0)
                {
                    _TypeCode = "0205";
                }
                else
                {
                    _TypeCode = "0203";
                }
            }
            else
            {

                if (Convert.ToInt64(_Vat) == 0)
                {
                    _TypeCode = "0202";
                }
                else
                {
                    _TypeCode = "0201";
                }
            }
            string Tdtp_Date = _Query.RequestDate.ToString("yyyyMMdd");

            _Amont = _Price + _Vat;



            //string dtiXml = "";
            //if (_Query.TypeCode == "위수탁")
            //{


            //    dtiXml = "" +
            //"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            //"<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            //" xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            //"   <TaxInvoiceDocument>\r\n" +
            //$"       <TypeCode>{_TypeCode}</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            //$"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            //$"      <IssueDateTime>{Tdtp_Date}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            //$"      <AmendmentStatusCode>01</AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            //$"      <PurposeCode>{_Query.purposeType}</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            //$"      <OriginalIssueID>{_Query.billNo}</OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            //$"  </TaxInvoiceDocument>\r\n" +
            //$"  <TaxInvoiceTradeSettlement>\r\n" +
            //$"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            //$"          <ID>{_Clients.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            //$"          <TypeCode>{_Clients.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            //$"          <NameText>{_Clients.Name}</NameText>	<!-- 상호명 -->\r\n" +
            //$"          <ClassificationCode>{_Clients.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            //$"          <SpecifiedOrganization>\r\n" +
            //$"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            //$"          </SpecifiedOrganization>\r\n" +
            //$"          <SpecifiedPerson>\r\n" +
            //$"              <NameText>{_Clients.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            //$"          </SpecifiedPerson>\r\n" +
            //$"          <DefinedContact>\r\n" +
            //$"              <URICommunication>{_Clients.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            //$"          </DefinedContact>\r\n" +
            //$"          <SpecifiedAddress>\r\n" +
            //$"              <LineOneText>{_Clients.AddressState + " " + _Clients.AddressCity + " " + _Clients.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
            //$"          </SpecifiedAddress>" +
            //$"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
            //$"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
            //$"          <ID>{_Query.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            //$"          <TypeCode>{_Query.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
            //$"          <NameText>{_Query.SangHo}</NameText>	<!-- 상호명 -->\r\n" +
            //$"          <ClassificationCode>{_Query.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            //$"          <SpecifiedOrganization>\r\n" +
            //$"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            //$"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
            //$"          </SpecifiedOrganization>\r\n" +
            //$"          <SpecifiedPerson>\r\n" +
            //$"              <NameText>{_Query.Ceo}</NameText>	<!--  대표자명 -->\r\n" +
            //$"          </SpecifiedPerson>\r\n" +
            //$"          <PrimaryDefinedContact>\r\n" +
            //$"              <PersonNameText>{_Query.Ceo}</PersonNameText>	<!-- 담당자명 -->\r\n" +
            //$"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
            //$"              <TelephoneCommunication>{_Query.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
            //$"              <URICommunication>{_Query.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            //$"          </PrimaryDefinedContact>\r\n" +
            //$"          <SpecifiedAddress>\r\n" +
            //$"              <LineOneText>{_Query.AddressState + " " + _Query.AddressCity + " " + _Query.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
            //$"          </SpecifiedAddress>\r\n" +
            //$"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +

            //        $"      <BrokerParty> <!-- 수탁자정보 --> \r\n" +
            //        $"          <ID>{_Admininfo.BIzNo.Replace("-", "")}</ID>     <!-- 사업자번호 -->\r\n" +
            //        $"          <SpecifiedOrganization>\r\n" +
            //        $"              <TaxRegistrationID></TaxRegistrationID>         <!-- 종사업자번호 -->\r\n" +
            //        $"          </SpecifiedOrganization>\r\n" +

            //        $"          <NameText>{_Admininfo.CustName}</NameText>         <!-- 상호명 -->\r\n" +
            //        $"          <SpecifiedPerson>\r\n" +
            //        $"              <NameText>{_Admininfo.ownerName}</NameText>       <!--  대표자명 -->\r\n" +
            //        $"          </SpecifiedPerson>\r\n" +
            //        $"          <SpecifiedAddress>\r\n" +
            //        $"              <LineOneText>{_Admininfo.addr1 + " " + _Admininfo.addr2}</LineOneText>          <!-- 사업장주소 -->\r\n" +
            //        $"          </SpecifiedAddress>\r\n" +
            //        $"          <TypeCode>{_Admininfo.BizCond}</TypeCode>  <!-- 업태 -->\r\n" +
            //        $"          <ClassificationCode>{_Admininfo.bizItem}</ClassificationCode>   <!-- 종목 -->\r\n" +
            //        $"          <DefinedContact>\r\n" +
            //        $"              <DepartmentNameText>물류팀</DepartmentNameText> <!-- 부서명 -->\r\n" +
            //        $"              <PersonNameText>{_Admininfo.rsbmName}</PersonNameText>      <!-- 담당자명 -->\r\n" +
            //        $"              <TelephoneCommunication>{_Admininfo.hpNo}</TelephoneCommunication>           <!-- 전화번호 -->\r\n" +
            //        $"             	<URICommunication>{_Admininfo.email}</URICommunication>  <!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            //        $"          </DefinedContact>\r\n" +
            //        $"      </BrokerParty> <!-- 수탁자정보 끝 -->\r\n" +

            //    $"          <SpecifiedPaymentMeans>\r\n" +
            //$"              <TypeCode>10</TypeCode> <!-- 10:현금, 20:수표, 30:어음, 40:외상미수금 -->\r\n" +
            //$"              <PaidAmount>{Convert.ToInt64(_Amont)}</PaidAmount> <!-- 부가세가 포함된 금액 -->\r\n" +
            //$"          </SpecifiedPaymentMeans>\r\n" +
            //$"          <SpecifiedMonetarySummation>\r\n" +
            //$"              <ChargeTotalAmount>{Convert.ToInt64(_Price)}</ChargeTotalAmount>	<!-- 총 공급가액 -->\r\n" +
            //$"              <TaxTotalAmount>{Convert.ToInt64(_Vat)}</TaxTotalAmount>	<!-- 총 세액 -->\r\n" +
            //$"              <GrandTotalAmount>{Convert.ToInt64(_Amont)}</GrandTotalAmount>	<!-- 총액(공급가액+세액) -->\r\n" +
            //$"          </SpecifiedMonetarySummation>\r\n" +
            //$"  </TaxInvoiceTradeSettlement>\r\n";

            //}
            //else
            //{
            //    dtiXml = "" +
            //"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            //"<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            //" xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            //"   <TaxInvoiceDocument>\r\n" +
            //$"       <TypeCode>{_TypeCode}</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            //$"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            //$"      <IssueDateTime>{Tdtp_Date}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            //$"      <AmendmentStatusCode>01</AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            //$"      <PurposeCode>{_Query.purposeType}</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            //$"      <OriginalIssueID>{_Query.billNo}</OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            //$"  </TaxInvoiceDocument>\r\n" +
            //$"  <TaxInvoiceTradeSettlement>\r\n" +
            //$"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            //$"          <ID>{_Clients.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            //$"          <TypeCode>{_Clients.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            //$"          <NameText>{_Clients.Name}</NameText>	<!-- 상호명 -->\r\n" +
            //$"          <ClassificationCode>{_Clients.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            //$"          <SpecifiedOrganization>\r\n" +
            //$"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            //$"          </SpecifiedOrganization>\r\n" +
            //$"          <SpecifiedPerson>\r\n" +
            //$"              <NameText>{_Clients.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            //$"          </SpecifiedPerson>\r\n" +
            //$"          <DefinedContact>\r\n" +
            //$"              <URICommunication>{_Clients.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            //$"          </DefinedContact>\r\n" +
            //$"          <SpecifiedAddress>\r\n" +
            //$"              <LineOneText>{_Clients.AddressState + " " + _Clients.AddressCity + " " + _Clients.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
            //$"          </SpecifiedAddress>" +
            //$"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
            //$"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
            //$"          <ID>{_Query.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            //$"          <TypeCode>{_Query.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
            //$"          <NameText>{_Query.SangHo}</NameText>	<!-- 상호명 -->\r\n" +
            //$"          <ClassificationCode>{_Query.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            //$"          <SpecifiedOrganization>\r\n" +
            //$"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            //$"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
            //$"          </SpecifiedOrganization>\r\n" +
            //$"          <SpecifiedPerson>\r\n" +
            //$"              <NameText>{_Query.Ceo}</NameText>	<!--  대표자명 -->\r\n" +
            //$"          </SpecifiedPerson>\r\n" +
            //$"          <PrimaryDefinedContact>\r\n" +
            //$"              <PersonNameText>{_Query.Ceo}</PersonNameText>	<!-- 담당자명 -->\r\n" +
            //$"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
            //$"              <TelephoneCommunication>{_Query.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
            //$"              <URICommunication>{_Query.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            //$"          </PrimaryDefinedContact>\r\n" +
            //$"          <SpecifiedAddress>\r\n" +
            //$"              <LineOneText>{_Query.AddressState + " " + _Query.AddressCity + " " + _Query.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
            //$"          </SpecifiedAddress>\r\n" +
            //$"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +

            //$"          <SpecifiedPaymentMeans>\r\n" +
            //$"              <TypeCode>10</TypeCode> <!-- 10:현금, 20:수표, 30:어음, 40:외상미수금 -->\r\n" +
            //$"              <PaidAmount>{Convert.ToInt64(_Amont)}</PaidAmount> <!-- 부가세가 포함된 금액 -->\r\n" +
            //$"          </SpecifiedPaymentMeans>\r\n" +
            //$"          <SpecifiedMonetarySummation>\r\n" +
            //$"              <ChargeTotalAmount>{Convert.ToInt64(_Price)}</ChargeTotalAmount>	<!-- 총 공급가액 -->\r\n" +
            //$"              <TaxTotalAmount>{Convert.ToInt64(_Vat)}</TaxTotalAmount>	<!-- 총 세액 -->\r\n" +
            //$"              <GrandTotalAmount>{Convert.ToInt64(_Amont)}</GrandTotalAmount>	<!-- 총액(공급가액+세액) -->\r\n" +
            //$"          </SpecifiedMonetarySummation>\r\n" +
            //$"  </TaxInvoiceTradeSettlement>\r\n";
            //}



            //if (DetailQuery1.Any())
            //{
            //    dtiXml += "" +
            //    $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
            //    $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
            //    $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery1.First().unitCost1)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
            //    $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery1.First().qty1)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
            //    $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
            //    $"      <NameText>{DetailQuery1.First().itemName1}</NameText>  <!-- 품목명 -->\r\n" +
            //    $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
            //    $"          <TotalTax>\r\n" +
            //    $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery1.First().tax1)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
            //    $"          </TotalTax>\r\n" +
            //    $"          <UnitPrice>\r\n" +
            //    $"              <UnitAmount>{Convert.ToInt64(DetailQuery1.First().supplyCost1)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
            //    $"          </UnitPrice>\r\n" +
            //    $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
            //    $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            //}
            //else
            //{
            //    dtiXml += "" +
            //      $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
            //      $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
            //      $"      <InvoiceAmount>{Convert.ToInt64(_Price)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
            //      $"      <ChargeableUnitQuantity>1</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
            //      $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
            //      $"      <NameText>{_Query.Item}</NameText>  <!-- 품목명 -->\r\n" +
            //      $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
            //      $"          <TotalTax>\r\n" +
            //      $"              <CalculatedAmount>{Convert.ToInt64(_Vat)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
            //      $"          </TotalTax>\r\n" +
            //      $"          <UnitPrice>\r\n" +
            //      $"              <UnitAmount>{Convert.ToInt64(_Price)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
            //      $"          </UnitPrice>\r\n" +
            //      $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
            //      $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";

            //}
            //if (DetailQuery2.Any())
            //{
            //    dtiXml += "" +
            //    $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
            //    $"      <SequenceNumeric>2</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
            //    $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery2.First().unitCost2)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
            //    $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery2.First().qty2)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
            //    $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
            //    $"      <NameText>{DetailQuery2.First().itemName2}</NameText>  <!-- 품목명 -->\r\n" +
            //    $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
            //    $"          <TotalTax>\r\n" +
            //    $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery2.First().tax2)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
            //    $"          </TotalTax>\r\n" +
            //    $"          <UnitPrice>\r\n" +
            //    $"              <UnitAmount>{Convert.ToInt64(DetailQuery2.First().supplyCost2)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
            //    $"          </UnitPrice>\r\n" +
            //    $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
            //    $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            //}

            //if (DetailQuery3.Any())
            //{
            //    dtiXml += "" +
            //    $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
            //    $"      <SequenceNumeric>3</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
            //    $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery3.First().unitCost3)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
            //    $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery3.First().qty3)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
            //    $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
            //    $"      <NameText>{DetailQuery3.First().itemName3}</NameText>  <!-- 품목명 -->\r\n" +
            //    $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
            //    $"          <TotalTax>\r\n" +
            //    $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery3.First().tax3)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
            //    $"          </TotalTax>\r\n" +
            //    $"          <UnitPrice>\r\n" +
            //    $"              <UnitAmount>{Convert.ToInt64(DetailQuery3.First().supplyCost3)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
            //    $"          </UnitPrice>\r\n" +
            //    $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
            //    $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            //}

            //if (DetailQuery4.Any())
            //{
            //    dtiXml += "" +
            //    $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
            //    $"      <SequenceNumeric>4</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
            //    $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery4.First().unitCost4)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
            //    $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery4.First().qty4)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
            //    $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
            //    $"      <NameText>{DetailQuery4.First().itemName4}</NameText>  <!-- 품목명 -->\r\n" +
            //    $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
            //    $"          <TotalTax>\r\n" +
            //    $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery4.First().tax4)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
            //    $"          </TotalTax>\r\n" +
            //    $"          <UnitPrice>\r\n" +
            //    $"              <UnitAmount>{Convert.ToInt64(DetailQuery4.First().supplyCost4)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
            //    $"          </UnitPrice>\r\n" +
            //    $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
            //    $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            //}
            //dtiXml += "" +
            //"</TaxInvoice>";











            //string Result = "";
            //if (_Query.TypeCode != "위수탁")
            //{
            //    Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Clients.frnNo, _Clients.userid, _Clients.passwd, certPw, dtiXml, "Y", "N", "", "3");
            //}
            //else
            //{
            //    passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            //    certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "dpebqlf54906**" }).ToString();

            //    Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "", "3");
            //}
            //try
            //{
            //    var ResultList = Result.Split('/');

            //    var retVal = ResultList[0];
            //    var errMsg = ResultList[1];
            //    var billNo = ResultList[2];
            //    var gnlPoint = ResultList[3];
            //    var outbnsPoint = ResultList[4];
            //    var totPoint = ResultList[5];
            //    //return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

            //    if (retVal == "0")
            //    {
            //        //승인번호 DB반영

            //        try
            //        {


            //            try
            //            {
            //                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //                {
            //                    cn.Open();
            //                    SqlCommand cmd = cn.CreateCommand();

            //                    cmd.CommandText =
            //                       "UPDATE SalesManage SET IssueDate = getdate()  , IssueState = '취소' ,billNo = @billNo,IssueLoginId = @IssueLoginId, IssueGubun = @IssueGubun  WHERE    SalesId =" + _Query.SalesId.ToString() + " ";
            //                    cmd.Parameters.AddWithValue("@billNo", billNo);
            //                    cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
            //                    cmd.Parameters.AddWithValue("@IssueGubun", "차세로");
            //                    cmd.ExecuteNonQuery();

            //                    SqlCommand Customercmd = cn.CreateCommand();
            //                    Customercmd.CommandText =
            //                       " UPDATE Customers " +
            //                       " SET Misu = Misu- @Misu WHERE CustomerId ='" + _Query.CustomerId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
            //                    Customercmd.Parameters.AddWithValue("@Misu", _Query.Amount);
            //                    Customercmd.ExecuteNonQuery();





            //                    cn.Close();

            //                }

            //                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //                {
            //                    cn.Open();
            //                    SqlCommand INcmd = cn.CreateCommand();

            //                    INcmd.CommandText =
            //                       "INSERT INTO  SalesManage(RequestDate, SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat, UseTax, Price, Amount, CreateDate, IssueDate, ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, IssueState, ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey)" +
            //                       $" SELECT getdate(), SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat*-1, UseTax, Price*-1, Amount*-1, getdate(), getdate(), ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, '취소', ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey FROM SalesManage WHERE SalesId = @SalesId";
            //                    // INcmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);
            //                    INcmd.Parameters.AddWithValue("@SalesId", _Query.SalesId);
            //                    INcmd.ExecuteNonQuery();
            //                    cn.Close();

            //                }

            //                //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //                //{
            //                //    cn.Open();
            //                //    SqlCommand cmdOrder = cn.CreateCommand();
            //                //    cmdOrder.CommandText =
            //                //        "Update Orders SET SalesManageId = NULL WHERE SalesManageId = @SalesManageId";

            //                //    cmdOrder.Parameters.AddWithValue("@SalesManageId", _Query.SalesId);
            //                //    cmdOrder.ExecuteNonQuery();
            //                //    cn.Close();
            //                //}

            //                MessageBox.Show("발행 취소 되었습니다.");

            //                btn_Search_Click(null, null);
            //            }
            //            catch (PopbillException ex)
            //            {
            //                MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
            //                Error++;
            //                btn_Search_Click(null, null);
            //            }
            //        }
            //        catch (PopbillException ex)
            //        {
            //            TaxInvoiceErrorDic.Add(_Query.SalesId, "[ " + ex.code.ToString() + " ] " + ex.Message);
            //            //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
            //            Error++;
            //        }
            //    }
            //    else
            //    {
            //        TaxInvoiceErrorDic.Add(_Query.SalesId, "[ " + retVal + " ] " + errMsg);
            //        //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
            //        Error++;
            //    }

            //}
            //catch
            //{

            //}

            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();

                    cmd.CommandText =
                       "UPDATE SalesManage SET IssueDate = getdate()  , IssueState = '취소' ,IssueLoginId = @IssueLoginId, IssueGubun = @IssueGubun  WHERE    SalesId =" + _Query.SalesId.ToString() + " ";
                    //cmd.Parameters.AddWithValue("@billNo", billNo);
                    cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
                    cmd.Parameters.AddWithValue("@IssueGubun", "차세로");
                    cmd.ExecuteNonQuery();

                    SqlCommand Customercmd = cn.CreateCommand();
                    Customercmd.CommandText =
                       " UPDATE Customers " +
                       " SET Misu = Misu- @Misu WHERE CustomerId ='" + _Query.CustomerId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                    Customercmd.Parameters.AddWithValue("@Misu", _Query.Amount);
                    Customercmd.ExecuteNonQuery();





                    cn.Close();

                }

                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand INcmd = cn.CreateCommand();

                    INcmd.CommandText =
                       "INSERT INTO  SalesManage(RequestDate, SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat, UseTax, Price, Amount, CreateDate, IssueDate, ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, IssueState, ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey)" +
                       $" SELECT getdate(), SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat*-1, UseTax, Price*-1, Amount*-1, getdate(), getdate(), ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, '취소', ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey FROM SalesManage WHERE SalesId = @SalesId";
                    // INcmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);
                    INcmd.Parameters.AddWithValue("@SalesId", _Query.SalesId);
                    INcmd.ExecuteNonQuery();
                    cn.Close();

                }

                //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                //{
                //    cn.Open();
                //    SqlCommand cmdOrder = cn.CreateCommand();
                //    cmdOrder.CommandText =
                //        "Update Orders SET SalesManageId = NULL WHERE SalesManageId = @SalesManageId";

                //    cmdOrder.Parameters.AddWithValue("@SalesManageId", _Query.SalesId);
                //    cmdOrder.ExecuteNonQuery();
                //    cn.Close();
                //}

                MessageBox.Show("발행 취소 되었습니다.");

                btn_Search_Click(null, null);
            }
            catch (PopbillException ex)
            {
                MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                Error++;
                btn_Search_Click(null, null);
            }

        }

        private void NiceEtaxDeleteEdit(int salesid)
        {
            var _Clients = LocalUser.Instance.LogInInformation.Client;
            var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";

           
            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.NPKIpasswd }).ToString();


            

            string DutyNum = string.Empty;
            string DescriptionText = "";
            int i = 0;

            var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
            if (_HideAccountNo == 0)
            {
                DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner + "/" + _Clients.CMSAccountNo;
            }
            else
            {
                DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner;

            }
            var _Query = salesDataSet.SalesManage.Where(c => c.SalesId == salesid).FirstOrDefault();

            decimal _Price = 0;
            decimal _Vat = 0;
            decimal _Amont = 0;
            var DetailQuery1 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName1) && c.SalesId == _Query.SalesId).ToArray();
            var DetailQuery2 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName2) && c.SalesId == _Query.SalesId).ToArray();
            var DetailQuery3 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName3) && c.SalesId == _Query.SalesId).ToArray();
            var DetailQuery4 = salesDataSet.SalesManageDetail.Where(c => !String.IsNullOrEmpty(c.itemName4) && c.SalesId == _Query.SalesId).ToArray();
            if (DetailQuery1.Any() && DetailQuery2.Any() && DetailQuery3.Any() && DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2 + DetailQuery3.First().unitCost3 + DetailQuery4.First().unitCost4;
                _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2 + DetailQuery3.First().tax3 + DetailQuery4.First().tax4;
            }
            else if (DetailQuery1.Any() && DetailQuery2.Any() && DetailQuery3.Any() && !DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2 + DetailQuery3.First().unitCost3;
                _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2 + DetailQuery3.First().tax3;
            }
            else if (DetailQuery1.Any() && DetailQuery2.Any() && !DetailQuery3.Any() && !DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1 + DetailQuery2.First().unitCost2;
                _Vat = DetailQuery1.First().tax1 + DetailQuery2.First().tax2;
            }
            else if (DetailQuery1.Any() && !DetailQuery2.Any() && !DetailQuery3.Any() && !DetailQuery4.Any())
            {
                _Price = DetailQuery1.First().unitCost1;
                _Vat = DetailQuery1.First().tax1;
            }
            else
            {
                _Price = _Query.Price * -1;
                _Vat = _Query.Vat * -1;
            }
            string _TypeCode = "0201";
            if (_Query.TypeCode == "위수탁")
            {
                if (Convert.ToInt64(_Vat) == 0)
                {
                    _TypeCode = "0205";
                }
                else
                {
                    _TypeCode = "0203";
                }
            }
            else
            {
               
                if (Convert.ToInt64(_Vat) == 0)
                {
                    _TypeCode = "0202";
                }
                else
                {
                    _TypeCode = "0201";
                }
            }
            string Tdtp_Date = _Query.RequestDate.ToString("yyyyMMdd");

            _Amont = _Price + _Vat;

          

            string dtiXml = "";
            if (_Query.TypeCode == "위수탁")
            {
                

                dtiXml = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            " xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            "   <TaxInvoiceDocument>\r\n" +
            $"       <TypeCode>{_TypeCode}</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            $"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            $"      <IssueDateTime>{Tdtp_Date}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            $"      <AmendmentStatusCode>01</AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            $"      <PurposeCode>{_Query.purposeType}</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            $"      <OriginalIssueID></OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            $"  </TaxInvoiceDocument>\r\n" +
            $"  <TaxInvoiceTradeSettlement>\r\n" +
            $"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            $"          <ID>{_Clients.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Clients.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            $"          <NameText>{_Clients.Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Clients.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Clients.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <URICommunication>{_Clients.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Clients.AddressState + " " + _Clients.AddressCity + " " + _Clients.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>" +
            $"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
            $"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
            $"          <ID>{_Query.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Query.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
            $"          <NameText>{_Query.SangHo}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Query.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Query.Ceo}</NameText>	<!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <PrimaryDefinedContact>\r\n" +
            $"              <PersonNameText>{_Query.Ceo}</PersonNameText>	<!-- 담당자명 -->\r\n" +
            $"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <TelephoneCommunication>{_Query.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
            $"              <URICommunication>{_Query.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </PrimaryDefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Query.AddressState + " " + _Query.AddressCity + " " + _Query.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +
            
                    $"      <BrokerParty> <!-- 수탁자정보 --> \r\n" +
                    $"          <ID>{_Admininfo.BIzNo.Replace("-", "")}</ID>     <!-- 사업자번호 -->\r\n" +
                    $"          <SpecifiedOrganization>\r\n" +
                    $"              <TaxRegistrationID></TaxRegistrationID>         <!-- 종사업자번호 -->\r\n" +
                    $"          </SpecifiedOrganization>\r\n" +

                    $"          <NameText>{_Admininfo.CustName}</NameText>         <!-- 상호명 -->\r\n" +
                    $"          <SpecifiedPerson>\r\n" +
                    $"              <NameText>{_Admininfo.ownerName}</NameText>       <!--  대표자명 -->\r\n" +
                    $"          </SpecifiedPerson>\r\n" +
                    $"          <SpecifiedAddress>\r\n" +
                    $"              <LineOneText>{_Admininfo.addr1 + " " + _Admininfo.addr2}</LineOneText>          <!-- 사업장주소 -->\r\n" +
                    $"          </SpecifiedAddress>\r\n" +
                    $"          <TypeCode>{_Admininfo.BizCond}</TypeCode>  <!-- 업태 -->\r\n" +
                    $"          <ClassificationCode>{_Admininfo.bizItem}</ClassificationCode>   <!-- 종목 -->\r\n" +
                    $"          <DefinedContact>\r\n" +
                    $"              <DepartmentNameText>물류팀</DepartmentNameText> <!-- 부서명 -->\r\n" +
                    $"              <PersonNameText>{_Admininfo.rsbmName}</PersonNameText>      <!-- 담당자명 -->\r\n" +
                    $"              <TelephoneCommunication>{_Admininfo.hpNo}</TelephoneCommunication>           <!-- 전화번호 -->\r\n" +
                    $"             	<URICommunication>{_Admininfo.email}</URICommunication>  <!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
                    $"          </DefinedContact>\r\n" +
                    $"      </BrokerParty> <!-- 수탁자정보 끝 -->\r\n" +
                
                $"          <SpecifiedPaymentMeans>\r\n" +
            $"              <TypeCode>10</TypeCode> <!-- 10:현금, 20:수표, 30:어음, 40:외상미수금 -->\r\n" +
            $"              <PaidAmount>{Convert.ToInt64(_Amont)}</PaidAmount> <!-- 부가세가 포함된 금액 -->\r\n" +
            $"          </SpecifiedPaymentMeans>\r\n" +
            $"          <SpecifiedMonetarySummation>\r\n" +
            $"              <ChargeTotalAmount>{Convert.ToInt64(_Price)}</ChargeTotalAmount>	<!-- 총 공급가액 -->\r\n" +
            $"              <TaxTotalAmount>{Convert.ToInt64(_Vat)}</TaxTotalAmount>	<!-- 총 세액 -->\r\n" +
            $"              <GrandTotalAmount>{Convert.ToInt64(_Amont)}</GrandTotalAmount>	<!-- 총액(공급가액+세액) -->\r\n" +
            $"          </SpecifiedMonetarySummation>\r\n" +
            $"  </TaxInvoiceTradeSettlement>\r\n";

            }
            else
            {
                dtiXml = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            " xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            "   <TaxInvoiceDocument>\r\n" +
            $"       <TypeCode>{_TypeCode}</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            $"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            $"      <IssueDateTime>{Tdtp_Date}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            $"      <AmendmentStatusCode>01</AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            $"      <PurposeCode>{_Query.purposeType}</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            $"      <OriginalIssueID></OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            $"  </TaxInvoiceDocument>\r\n" +
            $"  <TaxInvoiceTradeSettlement>\r\n" +
            $"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            $"          <ID>{_Clients.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Clients.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            $"          <NameText>{_Clients.Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Clients.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Clients.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <URICommunication>{_Clients.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Clients.AddressState + " " + _Clients.AddressCity + " " + _Clients.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>" +
            $"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
            $"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
            $"          <ID>{_Query.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Query.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
            $"          <NameText>{_Query.SangHo}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Query.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Query.Ceo}</NameText>	<!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <PrimaryDefinedContact>\r\n" +
            $"              <PersonNameText>{_Query.Ceo}</PersonNameText>	<!-- 담당자명 -->\r\n" +
            $"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <TelephoneCommunication>{_Query.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
            $"              <URICommunication>{_Query.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </PrimaryDefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Query.AddressState + " " + _Query.AddressCity + " " + _Query.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +
           
            $"          <SpecifiedPaymentMeans>\r\n" +
            $"              <TypeCode>10</TypeCode> <!-- 10:현금, 20:수표, 30:어음, 40:외상미수금 -->\r\n" +
            $"              <PaidAmount>{Convert.ToInt64(_Amont)}</PaidAmount> <!-- 부가세가 포함된 금액 -->\r\n" +
            $"          </SpecifiedPaymentMeans>\r\n" +
            $"          <SpecifiedMonetarySummation>\r\n" +
            $"              <ChargeTotalAmount>{Convert.ToInt64(_Price)}</ChargeTotalAmount>	<!-- 총 공급가액 -->\r\n" +
            $"              <TaxTotalAmount>{Convert.ToInt64(_Vat)}</TaxTotalAmount>	<!-- 총 세액 -->\r\n" +
            $"              <GrandTotalAmount>{Convert.ToInt64(_Amont)}</GrandTotalAmount>	<!-- 총액(공급가액+세액) -->\r\n" +
            $"          </SpecifiedMonetarySummation>\r\n" +
            $"  </TaxInvoiceTradeSettlement>\r\n";
            }

            

            if (DetailQuery1.Any())
            {
                dtiXml += "" +
                $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery1.First().unitCost1)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery1.First().qty1)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                $"      <NameText>{DetailQuery1.First().itemName1}</NameText>  <!-- 품목명 -->\r\n" +
                $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                $"          <TotalTax>\r\n" +
                $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery1.First().tax1)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                $"          </TotalTax>\r\n" +
                $"          <UnitPrice>\r\n" +
                $"              <UnitAmount>{Convert.ToInt64(DetailQuery1.First().supplyCost1)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                $"          </UnitPrice>\r\n" +
                $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
                $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            }
            else
            {
                dtiXml += "" +
                  $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                  $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                  $"      <InvoiceAmount>{Convert.ToInt64(_Price)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                  $"      <ChargeableUnitQuantity>1</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                  $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                  $"      <NameText>{_Query.Item}</NameText>  <!-- 품목명 -->\r\n" +
                  $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                  $"          <TotalTax>\r\n" +
                  $"              <CalculatedAmount>{Convert.ToInt64(_Vat)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                  $"          </TotalTax>\r\n" +
                  $"          <UnitPrice>\r\n" +
                  $"              <UnitAmount>{Convert.ToInt64(_Price)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                  $"          </UnitPrice>\r\n" +
                  $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
                  $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";

            }
            if (DetailQuery2.Any())
            {
                dtiXml += "" +
                $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                $"      <SequenceNumeric>2</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery2.First().unitCost2)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery2.First().qty2)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                $"      <NameText>{DetailQuery2.First().itemName2}</NameText>  <!-- 품목명 -->\r\n" +
                $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                $"          <TotalTax>\r\n" +
                $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery2.First().tax2)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                $"          </TotalTax>\r\n" +
                $"          <UnitPrice>\r\n" +
                $"              <UnitAmount>{Convert.ToInt64(DetailQuery2.First().supplyCost2)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                $"          </UnitPrice>\r\n" +
                $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
                $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            }

            if (DetailQuery3.Any())
            {
                dtiXml += "" +
                $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                $"      <SequenceNumeric>3</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery3.First().unitCost3)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery3.First().qty3)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                $"      <NameText>{DetailQuery3.First().itemName3}</NameText>  <!-- 품목명 -->\r\n" +
                $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                $"          <TotalTax>\r\n" +
                $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery3.First().tax3)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                $"          </TotalTax>\r\n" +
                $"          <UnitPrice>\r\n" +
                $"              <UnitAmount>{Convert.ToInt64(DetailQuery3.First().supplyCost3)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                $"          </UnitPrice>\r\n" +
                $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
                $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            }

            if (DetailQuery4.Any())
            {
                dtiXml += "" +
                $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                $"      <SequenceNumeric>4</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                $"      <InvoiceAmount>{Convert.ToInt64(DetailQuery4.First().unitCost4)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                $"      <ChargeableUnitQuantity>{Convert.ToInt64(DetailQuery4.First().qty4)}</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                $"      <NameText>{DetailQuery4.First().itemName4}</NameText>  <!-- 품목명 -->\r\n" +
                $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                $"          <TotalTax>\r\n" +
                $"              <CalculatedAmount>{Convert.ToInt64(DetailQuery4.First().tax4)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                $"          </TotalTax>\r\n" +
                $"          <UnitPrice>\r\n" +
                $"              <UnitAmount>{Convert.ToInt64(DetailQuery4.First().supplyCost4)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                $"          </UnitPrice>\r\n" +
                $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
                $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";
            }
            dtiXml += "" +
            "</TaxInvoice>";










            //try
            //{
            //    webBrowser1.Url = new Uri("http://222.231.9.253/NiceEncoding.asp");
            //    //frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.frnNo }).ToString();
            //    //userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.userid }).ToString();
            //    passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            //  //  Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.Linkcd }).ToString();


            //    //MessageBox.Show(Encode.ToString());

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.Write(ex.Message);

            //}

            string Result = "";
            if (_Query.TypeCode != "위수탁")
            {
                Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Clients.frnNo, _Clients.userid, _Clients.passwd, certPw, dtiXml, "Y", "N", "", "3");
            }
            else
            {
                passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
                certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "dpebqlf54906**" }).ToString();

                Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "", "3");
            }
            try
            {
                var ResultList = Result.Split('/');

                var retVal = ResultList[0];
                var errMsg = ResultList[1];
                var billNo = ResultList[2];
                var gnlPoint = ResultList[3];
                var outbnsPoint = ResultList[4];
                var totPoint = ResultList[5];
                //return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

                if (retVal == "0")
                {
                    //승인번호 DB반영

                    try
                    {


                        try
                        {
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                SqlCommand cmd = cn.CreateCommand();

                                cmd.CommandText =
                                   "UPDATE SalesManage SET IssueDate = getdate()  , IssueState = '취소' ,billNo = @billNo,IssueLoginId = @IssueLoginId, IssueGubun = @IssueGubun  WHERE    SalesId =" + _Query.SalesId.ToString() + " ";
                                cmd.Parameters.AddWithValue("@billNo", billNo);
                                cmd.Parameters.AddWithValue("@IssueLoginId", LocalUser.Instance.LogInInformation.LoginId);
                                cmd.Parameters.AddWithValue("@IssueGubun", "차세로");
                                cmd.ExecuteNonQuery();

                                SqlCommand Customercmd = cn.CreateCommand();
                                Customercmd.CommandText =
                                   " UPDATE Customers " +
                                   " SET Misu = Misu- @Misu WHERE CustomerId ='" + _Query.CustomerId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                                Customercmd.Parameters.AddWithValue("@Misu", _Query.Amount);
                                Customercmd.ExecuteNonQuery();





                                cn.Close();

                            }
                            
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                SqlCommand INcmd = cn.CreateCommand();

                                INcmd.CommandText =
                                   "INSERT INTO  SalesManage(RequestDate, SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat, UseTax, Price, Amount, CreateDate, IssueDate, ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, IssueState, ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey)" +
                                   $" SELECT getdate(), SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat*-1, UseTax, Price*-1, Amount*-1, getdate(), getdate(), ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, '취소', ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey FROM SalesManage WHERE SalesId = @SalesId";
                                // INcmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);
                                INcmd.Parameters.AddWithValue("@SalesId", _Query.SalesId);
                                INcmd.ExecuteNonQuery();
                                cn.Close();

                            }

                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                SqlCommand cmdOrder = cn.CreateCommand();
                                cmdOrder.CommandText =
                                    "Update Orders SET SalesManageId = NULL WHERE SalesManageId = @SalesManageId";

                                cmdOrder.Parameters.AddWithValue("@SalesManageId", _Query.SalesId);
                                cmdOrder.ExecuteNonQuery();
                                cn.Close();
                            }

                            MessageBox.Show("발행 취소 되었습니다.");

                            btn_Search_Click(null, null);
                        }
                        catch (PopbillException ex)
                        {
                            MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                            Error++;
                            btn_Search_Click(null, null);
                        }
                    }
                    catch (PopbillException ex)
                    {
                        TaxInvoiceErrorDic.Add(_Query.SalesId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                        //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                        Error++;
                    }
                }
                else
                {
                    TaxInvoiceErrorDic.Add(_Query.SalesId, "[ " + retVal + " ] " + errMsg);
                    //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                    Error++;
                }

            }
            catch
            {

            }

        }

        private void NiceEtaxDelete(int salesid)
        {
            // var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
            LocalUser.Instance.LogInInformation.LoadClient();
            var _Clients = LocalUser.Instance.LogInInformation.Client;
            var Selected = salesDataSet.SalesManage.Where(c => c.SalesId == salesid).FirstOrDefault();
            
            try
            {




                #region 세금계산서 삭제 정발행
                if (Selected.TypeCode != "위수탁")
                {
                    var Result = dTIServiceClass.updateEtaxStatusToZ("EDB", _Clients.frnNo, _Clients.userid, _Clients.passwd, Selected.billNo);

                    try
                    {
                        var ResultList = Result.Split('/');

                        var ResultListretVal = ResultList[0];
                        var ResultListerrMsg = ResultList[1];

                        if (ResultListretVal == "0")
                        {
                            try
                            {
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                       $"UPDATE SalesManage SET IssueDate = NULL, Issue = 0, IssueState = '0' ,IssueLoginId = NULL, IssueGubun = NULL,billNo = NULL WHERE    SalesId ={Selected.SalesId}";
                                    cmd.Parameters.AddWithValue("@IssueDate", dtp_PayDate.Value);

                                    cmd.ExecuteNonQuery();


                                    SqlCommand Customercmd = cn.CreateCommand();
                                    Customercmd.CommandText =
                                       " UPDATE Customers " +
                                       " SET Misu = Misu - @Misu WHERE CustomerId ='" + Selected.CustomerId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                                    Customercmd.Parameters.AddWithValue("@Misu", Selected.Amount);
                                    Customercmd.ExecuteNonQuery();




                                    cn.Close();


                                }
                                MessageBox.Show("전자세금계산서가 삭제 되었습니다.");
                                //Success++;
                                btn_Search_Click(null, null);
                            }

                            catch (PopbillException ext)
                            {
                                MessageBox.Show(ext.code.ToString() + "\r\n" + ext.Message);
                                return;
                            }
                        }
                        else
                        {
                            //NiceEtaxEdit(salesid);
                        }

                    }
                    catch
                    {

                    }
                }
                #endregion


                #region 세금계산서 삭제(위수탁)
                else
                {
                    AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
                    var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

                    var Result = dTIServiceClass.updateEtaxStatusToZ("EDB", _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, Selected.billNo);

                    try
                    {
                        var ResultList = Result.Split('/');

                        var ResultListretVal = ResultList[0];
                        var ResultListerrMsg = ResultList[1];

                        if (ResultListretVal == "0")
                        {
                            try
                            {
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                       $"UPDATE SalesManage SET IssueDate = NULL, Issue = 0, IssueState = '0' ,IssueLoginId = NULL, IssueGubun = NULL,billNo = NULL WHERE    SalesId ={Selected.SalesId}";
                                    cmd.Parameters.AddWithValue("@IssueDate", dtp_PayDate.Value);

                                    cmd.ExecuteNonQuery();


                                    SqlCommand Customercmd = cn.CreateCommand();
                                    Customercmd.CommandText =
                                       " UPDATE Customers " +
                                       " SET Misu = Misu - @Misu WHERE CustomerId ='" + Selected.CustomerId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                                    Customercmd.Parameters.AddWithValue("@Misu", Selected.Amount);
                                    Customercmd.ExecuteNonQuery();




                                    cn.Close();


                                }
                                MessageBox.Show("전자세금계산서가 삭제 되었습니다.");
                                //Success++;
                                btn_Search_Click(null, null);
                            }

                            catch (PopbillException ext)
                            {
                                MessageBox.Show(ext.code.ToString() + "\r\n" + ext.Message);
                                return;
                            }
                        }
                        else
                        {
                            //NiceEtaxEdit(salesid);
                        }

                    }
                    catch
                    {

                    }
                }
                #endregion
            }
            catch
            {

            }
        }
        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }
        private void button5_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
            var _Clients = LocalUser.Instance.LogInInformation.Client;
            // var Result = dTIServiceClass.updateEtaxStatusToZ(_Admininfo.Linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, "202001284100004000000001");
            
            var Result = dTIServiceClass.updateEtaxStatusToZ("EDB", _Clients.frnNo, _Clients.userid, _Clients.passwd, textBox2.Text);


            try
            {
                var ResultList = Result.Split('/');

                var ResultListretVal = ResultList[0];
                var ResultListerrMsg = ResultList[1];
            }
            catch { }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;




            var fileString = "매출정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
                if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { SalesChk, Stats, Column1,ColumnError }.Contains(grid1.Columns[i])))
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { SalesChk, Stats, Column1, ColumnError }.Contains(grid1.Columns[i])))
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

        private void button6_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);


            _FormStyle.WriteFormStyle(this, grid1);

            MessageBox.Show("저장하였습니다.");
        }
    }

    class DirectMail
    {
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE { get; set; }
        public string NOTE { get; set; }
        
    }
}
