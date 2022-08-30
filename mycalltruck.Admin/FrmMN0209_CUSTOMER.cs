using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.XML;
using mycalltruck.Admin.Class.Extensions;
using OfficeOpenXml.Style;

namespace mycalltruck.Admin
{
    public partial class FrmMN0209_CUSTOMER : Form
    {
        int GridIndex = 0;
        Dictionary<int, Boolean> AccDictionary = new Dictionary<int, bool>();

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

                    btnStartNew.Enabled = false;
                    btnStartDelete.Enabled = false;
                    btnStartUpdate.Enabled = false;

                    btnStopNew.Enabled = false;
                    btnStopDelete.Enabled = false;
                    btnStopUpdate.Enabled = false;

                    btnPNew.Enabled = false;
                    btnPDelete.Enabled = false;
                    btnPUpdate.Enabled = false;

                    grid1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }
        }

        public FrmMN0209_CUSTOMER()
        {
            InitializeComponent();


            //this.Font = new Font(FontLibrary.Families[0], 10, System.Drawing.FontStyle.Regular);
            //if (!LocalUser.Instance.LogInInformation.IsAdmin)
            if (LocalUser.Instance.LogInInformation.IsClient)
            {
                ApplyAuth();
            }
            Image1.Tag = ImageName1;
            Image2.Tag = ImageName2;
            Image3.Tag = ImageName3;
            Image4.Tag = ImageName4;
            Image5.Tag = ImageName5;

            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);

            // if (!LocalUser.Instance.LogInInformation.IsAdmin)
            if (LocalUser.Instance.LogInInformation.IsClient)
            {
                grid1.Columns["ColumnClientCode"].Visible = false;
                grid1.Columns["ColumnClientName"].Visible = false;
                
                //grid1.Columns[2].Visible = false;
                //grid1.Columns[3].Visible = false;
            }
        }

        private void FrmMN0209_CARGOCLIENT_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'customerUserDataSet.CustomerTeams' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.customerTeamsTableAdapter.Fill(this.customerUserDataSet.CustomerTeams, LocalUser.Instance.LogInInformation.ClientId);
            // TODO: 이 코드는 데이터를 'customerStartManageDataSet.CustomerStopManage' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.customerStopManageTableAdapter.Fill(this.customerStartManageDataSet.CustomerStopManage);
            // TODO: 이 코드는 데이터를 'customerStartManageDataSet1.CustomerAddPhone' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.customerAddPhoneTableAdapter.FillByClient(this.customerStartManageDataSet.CustomerAddPhone);
            // TODO: 이 코드는 데이터를 'customerStartManageDataSet.CustomerStartManage' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.customerStartManageTableAdapter.Fill(this.customerStartManageDataSet.CustomerStartManage);

            _InitCmb();
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_New.Enabled = false;
            }
            else if (!LocalUser.Instance.LogInInformation.IsClient)
            {
                btn_New.Enabled = false;
                btnUpdate.Enabled = false;
                btnCurrentDelete.Enabled = false;


                //btnPNew.Enabled = false;
                //btnPDelete.Enabled = false;
                //btnPUpdate.Enabled = false;
            }

            if (LocalUser.Instance.LogInInformation.CustomerUserId != 0)
            {
                btnPNew.Enabled = false;
                btnPDelete.Enabled = false;
            }
            else
            {
                btnPNew.Enabled = true;
                btnPDelete.Enabled = true;
            }

            // 운창로지텍만 마감일이 표시된다.
            if(LocalUser.Instance.LogInInformation.ClientId !=290)
            {
                lbl_EndDay.Visible = false;
                cmb_EndDay.Visible = false;
            }
            cmb_Search.SelectedIndex = 0;
            cmb_SalesSearch.SelectedIndex = 0;
            cmb_IGubun.SelectedIndex = 0;

            if (!LocalUser.Instance.LogInInformation.IsAdmin && !LocalUser.Instance.LogInInformation.IsSubClient && !LocalUser.Instance.LogInInformation.IsAgent)
            {
                LocalUser.Instance.LogInInformation.LoadClient();
                if (LocalUser.Instance.LogInInformation.Client.CustomerPay)
                {
                    lblPointMethod.Visible = true;
                    cmb_PointMethod.Visible = true;
                    var PointMethodDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "PointMethod").Select(c => new { c.Name, c.Value, c.Seq }).OrderBy(c => c.Seq).ToArray();
                    cmb_PointMethod.DataSource = PointMethodDataSource;
                    cmb_PointMethod.DisplayMember = "Name";
                    cmb_PointMethod.ValueMember = "Value";

                    lbl_LoginId.Visible = true;
                    lbl_Password.Visible = true;
                    txt_LoginId.Visible = true;
                    txt_Password.Visible = true;
                    txt_LoginId.Enabled = true;
                    txt_Password.Enabled = true;
                    lblDriverId.Visible = true;
                    cmb_DriverId.Visible = true;

                    Dictionary<int, String> DriverIdDataSource = new Dictionary<int, string>();
                    DriverIdDataSource.Add(0, "[선택 안함]");
                    BaseDataSet.DriversDataTable T = new BaseDataSet.DriversDataTable();
                    DriverRepository mDriverRepository = new DriverRepository();
                    mDriverRepository.Select(T);
                    foreach (var driverRow in T)
                    {
                        DriverIdDataSource.Add(driverRow.DriverId, $"{driverRow.Name}[{driverRow.CarYear}] ({driverRow.CarNo})");
                    }
                    cmb_DriverId.DataSource = DriverIdDataSource.ToList();
                    cmb_DriverId.ValueMember = "Key";
                    cmb_DriverId.DisplayMember = "Value";

                    lbl_Fee.Visible = true;
                    txt_Fee.Visible = true;
                }
            }
            btn_Search_Click(null, null);

            
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0209_CUSTOMER_Add _Form = new FrmMN0209_CUSTOMER_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
             err.Clear();
            LocalUser.Instance.LogInInformation.LoadClient();
            var Row = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Row == null)
                return;

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }


            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                return;
            }
            else /*if(!LocalUser.Instance.LogInInformation.Client.AllowMultiCustomer)*/
            {
                var Query1 =
                      "Select Count(*) From Customers Where BizNo = @BizNo AND CustomerId != @CustomerId AND ClientId = @ClientId";


                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                    cmd1.Parameters.AddWithValue("@CustomerId", Row.CustomerId);
                    cmd1.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }

                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show("사업자번호가 중복되었습니다.!!. 다른사업자 사업자번호를 입력해주십시오.");
                    err.SetError(txt_BizNo, "사업자번호가 중복되었습니다!!");

                    return;
                }

            }
            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
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

            if (txt_Street.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Street, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_Zip.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Zip, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_PhoneNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_MobileNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (!Regex.Match(txt_Email.Text, @"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$").Success)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Email, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (!String.IsNullOrEmpty(txt_LoginId.Text) && !String.IsNullOrEmpty(txt_Password.Text))
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "SELECT COUNT(*) FROM " +
                        " (Select LoginId, ClientId From ClientUsers " +
                        " union " +
                        " Select LoginId, ClientId From clients " +
                        " union " +
                        " Select LoginId, customerid From Customers " +
                        " union " +
                        " Select LoginId, customerid From CustomerAddPhone) as a " +
                        " WHERE Loginid  = @LoginId AND ClientId <> @CustomerId";


                    _Command.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    _Command.Parameters.AddWithValue("@CustomerId", Row.CustomerId);
                    if (Convert.ToInt32(_Command.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("아이디가 중복되었습니다.");
                        err.SetError(txt_LoginId, "아이디가 중복되었습니다");
                        return;
                    }
                }
            }
            if(!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                int t = 0;
                if(!int.TryParse(txt_Fee.Text, out t) || t < 0 || t > 100)
                {
                    MessageBox.Show("수수료는 0~100 사이의 값을 입력해주십시오.");
                    err.SetError(txt_Fee, "수수료는 0~100 사이의 값을 입력해주십시오.");
                    return;
                }
            }
            UpdateDB();
        }
        private void UpdateDB()
        {
            try
            {

                customersBindingSource.EndEdit();


                var Row = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;

                Row.Remark = textBox1.Text;

                if(cmb_SalesGubun.SelectedIndex == 1)
                {
                    Row.EndDay = 0;
                }
                else
                {
                    Row.EndDay = (int)cmb_EndDay.SelectedValue;
                }
                // customersTableAdapter.Update(Row);


                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                        @" UPDATE Customers
                            SET BizNo = @BizNo
                          ,SangHo = @SangHo
                          ,Ceo = @Ceo
                          ,Uptae = @Uptae
                          ,Upjong = @Upjong
                          ,AddressState = @AddressState
                          ,AddressCity = @AddressCity
                          ,AddressDetail = @AddressDetail
                          ,BizGubun = @BizGubun
                          ,ResgisterNo = @ResgisterNo
                          ,SalesGubun = @SalesGubun
                          ,Email = @Email
                          ,PhoneNo = @PhoneNo
                          ,FaxNo = @FaxNo
                          ,ChargeName = @ChargeName
                          ,MobileNo = @MobileNo
                         
                          ,Zipcode = @Zipcode
                    
                          ,EndDay = @EndDay
                          ,ClientUserId = @ClientUserId
                          ,PointMethod = @PointMethod
                       
                          ,DriverId = @DriverId
                          ,Fee = @Fee
                          ,LoginId = @LoginId
                          ,Password = @Password
                          ,Remark = @Remark
                          ,CustomerManagerId = @CustomerManagerId
                          ,Misu = @Misu
                          ,Mizi = @Mizi
                          ,SalesDay = @SalesDay
                            WHERE CustomerId = @CustomerId ";
                        _Command.Parameters.AddWithValue("@BizNo", Row.BizNo);
                        _Command.Parameters.AddWithValue("@SangHo", Row.SangHo);
                        _Command.Parameters.AddWithValue("@Ceo", Row.Ceo);
                        _Command.Parameters.AddWithValue("@Uptae", Row.Uptae);
                        _Command.Parameters.AddWithValue("@Upjong", Row.Upjong);
                        _Command.Parameters.AddWithValue("@AddressState", Row.AddressState);
                        _Command.Parameters.AddWithValue("@AddressCity", Row.AddressCity);
                        _Command.Parameters.AddWithValue("@AddressDetail", Row.AddressDetail);
                        _Command.Parameters.AddWithValue("@BizGubun", Row.BizGubun);
                        _Command.Parameters.AddWithValue("@ResgisterNo", Row.ResgisterNo);
                        _Command.Parameters.AddWithValue("@SalesGubun", Row.SalesGubun);

                        _Command.Parameters.AddWithValue("@Email", Row.Email);
                        _Command.Parameters.AddWithValue("@PhoneNo", Row.PhoneNo);
                        _Command.Parameters.AddWithValue("@FaxNo", Row.FaxNo);
                        _Command.Parameters.AddWithValue("@ChargeName", Row.ChargeName);
                        _Command.Parameters.AddWithValue("@MobileNo", Row.MobileNo);

                        
                        _Command.Parameters.AddWithValue("@Zipcode", Row.Zipcode);
                        _Command.Parameters.AddWithValue("@EndDay", Row.EndDay);
                        _Command.Parameters.AddWithValue("@ClientUserId", Row.ClientUserId);
                        _Command.Parameters.AddWithValue("@PointMethod", Row.PointMethod);

                        _Command.Parameters.AddWithValue("@DriverId", Row.DriverId);
                        _Command.Parameters.AddWithValue("@Fee", Row.Fee);
                        _Command.Parameters.AddWithValue("@LoginId", Row.LoginId);

                        _Command.Parameters.AddWithValue("@Password", Row.Password);
                        _Command.Parameters.AddWithValue("@Remark", Row.Remark);
                        _Command.Parameters.AddWithValue("@CustomerManagerId", Row.CustomerManagerId);
                        _Command.Parameters.AddWithValue("@Misu", Row.Misu);
                        _Command.Parameters.AddWithValue("@Mizi", Row.Mizi);
                        _Command.Parameters.AddWithValue("@CustomerId", Row.CustomerId);
                        _Command.Parameters.AddWithValue("@SalesDay", Row.SalesDay);
                        
                        _Command.ExecuteNonQuery();
                    }

                    using (SqlCommand _CCommand = _Connection.CreateCommand())
                    {
                        _CCommand.CommandText =
                        @" UPDATE Orders
                            SET 
                          customer = @customer
                         
                            WHERE CustomerId = @CustomerId
                            AND ClientId = @ClientId
                            AND (SalesManageId IS NULL)";

                        _CCommand.Parameters.AddWithValue("@customer", Row.SangHo);

                        _CCommand.Parameters.AddWithValue("@CustomerId", Row.CustomerId);
                        _CCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                        _CCommand.ExecuteNonQuery();
                    }
                });



                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "거래처", 1), "거래처정보 수정 성공");

                if (grid1.RowCount > 1)
                {
                    GridIndex = customersBindingSource.Position;
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
                MessageBox.Show(e.Message, "거래처정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {

            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

           

            if (grid1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in grid1.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }
                int Ordercount = 0;
                int Salescount = 0;
                int CustomerPriceCount = 0;
                foreach (DataGridViewRow dRow in deleteRows)
                {
                    Ordercount += (int)customersTableAdapter.OrdersQuery(int.Parse(dRow.Cells["customerIdDataGridViewTextBoxColumn"].Value.ToString()));
                    Salescount += (int)customersTableAdapter.SalesQuery(int.Parse(dRow.Cells["customerIdDataGridViewTextBoxColumn"].Value.ToString()));
                    //CustomerPriceCount += (int)customersTableAdapter.CustomerPriceQuery(int.Parse(dRow.Cells["customerIdDataGridViewTextBoxColumn"].Value.ToString()));
                }
                if (Ordercount > 0 || Salescount > 0 || CustomerPriceCount > 0)
                {
                    MessageBox.Show(string.Format("사용중인 데이터 배차관리 : {0}건 \n매출관리 : {1}건 이  존재하므로\n이 거래처정보는 삭제할 수 없습니다.",
                        Ordercount, Salescount, CustomerPriceCount), "거래처정보 삭제 실패");
                    return;
                }

                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "거래처", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
               
            }
            try
            {
                var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;

                //int _CCount = 0;


                Data.Connection(_Connection =>
                {

                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {

                        _Command.CommandText = " Delete Customers Where CustomerId = @CustomerId AND ClientId = @ClientId";
                        _Command.Parameters.AddWithValue("@CustomerId", Selected.CustomerId);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }

                });


                MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "거래처", 1), "거래처정보 삭제 성공");
                btn_Search_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "거래처정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void _Search()
        {
            string _SelectCommandText = "";
            try
            {
                if(LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    _SelectCommandText =
               @"		SELECT  Customers.CustomerId, Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, 
                        CreateTime, Customers.ClientId  AS ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, ISNULL(DriverId, 0) AS DriverId, ISNULL(Fee, 8) AS Fee, LoginId, Password,Customers.Remark AS Remark ,CustomerManagerId ,ISNULL(Customers.Misu,0) AS Misu,ISNULL(Customers.Mizi,0) as Mizi, Customers.ClientId as Ccid,SalesDay
                        FROM     Customers  ";
                }
                else
                {
                    _SelectCommandText =
               @"		SELECT  Customers.CustomerId, Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, 
                        CreateTime, Customers.ClientId  AS ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, ISNULL(DriverId, 0) AS DriverId, ISNULL(Fee, 8) AS Fee, LoginId, Password,Customers.Remark AS Remark,CustomerManagerId ,ISNULL(Customers.Misu,0) AS Misu,ISNULL(Customers.Mizi,0) as Mizi, Customers.ClientId as Ccid,SalesDay
                        FROM     Customers  ";
                }
               
                clientDataSet.Customers.Clear();
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        String SelectCommandText = _SelectCommandText;

                        List<String> WhereStringList = new List<string>();
                        // 1. 본점/지사
                        //if (LocalUser.Instance.LogInInformation.IsSubClient)
                        //{
                        //    if (LocalUser.Instance.LogInInformation.IsAgent)
                        //    {
                        //        WhereStringList.Add("ClientUserId = @ClientUserId");
                        //        _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        //    }
                        //    else
                        //    {
                        //        WhereStringList.Add("SubClientId = @SubClientId");
                        //        _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        //    }
                        //}
                        //else
                        if (!LocalUser.Instance.LogInInformation.IsAdmin)
                        {
                            if (LocalUser.Instance.LogInInformation.IsClient)
                            {
                                WhereStringList.Add("Customers.ClientId = @ClientId");
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                // 1.1 본지점구분
                                if (cmb_SubClientId.SelectedIndex > 0)
                                {
                                    var _SubClientId = (int)cmb_SubClientId.SelectedValue;
                                    if (_SubClientId == 0)
                                    {
                                        WhereStringList.Add("SubClientId IS NULL");
                                    }
                                    else
                                    {
                                        WhereStringList.Add("SubClientId = @SubClientId");
                                        _Command.Parameters.AddWithValue("@SubClientId", _SubClientId);
                                    }
                                }
                            }
                            else
                            {
                                WhereStringList.Add("Customers.CustomerId = @CustomerId");
                                _Command.Parameters.AddWithValue("@CustomerId", LocalUser.Instance.LogInInformation.CustomerId);
                            }
                        }
                        // 2. 단어 검색
                        if (cmb_Search.SelectedIndex > 0)
                        {
                            switch (cmb_Search.Text)
                            {
                                case "사업자번호":
                                    WhereStringList.Add("REPLACE(BizNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "전화번호":
                                    WhereStringList.Add("REPLACE(PhoneNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "팩스번호":
                                    WhereStringList.Add("REPLACE(FaxNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "핸드폰번호":
                                    WhereStringList.Add("REPLACE(MobileNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "상호":
                                    WhereStringList.Add("SangHo LIKE '%' + @Text + '%'");
                                    break;
                                case "대표자":
                                    WhereStringList.Add("Ceo LIKE '%' + @Text + '%'");
                                    break;
                                case "담당자":
                                    WhereStringList.Add("ChargeName LIKE '%' + @Text + '%'");
                                    break;
                                default:
                                    break;
                            }
                            _Command.Parameters.AddWithValue("@Text", txt_Search.Text.Replace("-", ""));
                        }
                        // 사업구분
                        if (cmb_SalesSearch.SelectedIndex > 0)
                        {
                            if (cmb_SalesSearch.Text == "매출처")
                            {
                                WhereStringList.Add("SalesGubun = 1");
                            }
                            else if (cmb_SalesSearch.Text == "매입처")
                            {
                                WhereStringList.Add("SalesGubun = 2");
                            }
                            else if (cmb_SalesSearch.Text == "매입/매출")
                            {
                                WhereStringList.Add("SalesGubun = 3");
                            }
                        }

                        if(cmb_IGubun.SelectedIndex > 0)
                        {
                            if (cmb_IGubun.Text == "화주")
                            {
                                WhereStringList.Add("BizGubun = 1");
                            }
                            else if (cmb_IGubun.Text == "운송")
                            {
                                WhereStringList.Add("BizGubun = 2");
                            }
                            else if (cmb_IGubun.Text == "주선")
                            {
                                WhereStringList.Add("BizGubun = 3");
                            }
                            else if (cmb_IGubun.Text == "위탁사")
                            {
                                WhereStringList.Add("BizGubun = 4");
                            }
                            else if (cmb_IGubun.Text == "기타")
                            {
                                WhereStringList.Add("BizGubun = 5");
                            }

                        }

                        if (WhereStringList.Count > 0)
                        {
                            var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                            SelectCommandText += Environment.NewLine + WhereString;
                        }
                        _Command.CommandText = SelectCommandText + " Order by CreateTime DESC";
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            clientDataSet.Customers.Load(_Reader);
                        }
                    }
                    _Connection.Close();
                }
            }
            catch
            {
                MessageBox.Show("조회가 실패하였습니다. 잠시 후에 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }
        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.ColumnIndex == No.Index && e.RowIndex >= 0)
            {
                grid1[e.ColumnIndex, e.RowIndex].Value = (grid1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            else if (e.ColumnIndex == salesGubunDataGridViewTextBoxColumn.Index && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.CustomersRow;

                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "SalesGubun" && c.Value == Selected.SalesGubun);

                if (Query.Any())
                {
                    e.Value = Query.First().Name;
                }
            }
            else if (e.ColumnIndex == createTimeDataGridViewTextBoxColumn.Index && e.RowIndex >= 0)
            {

                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
            else if (e.ColumnIndex == SubClientId.Index)
            {
                var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.CustomersRow;
                if (SubClientIdDictionary.ContainsKey(Selected.SubClientId))
                    e.Value = SubClientIdDictionary[Selected.SubClientId];
            }
           
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                 var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.CustomersRow;
                if (e.ColumnIndex == ColumnClientCode.Index)
                {
                    var Query = _ClientDataSource.Where(c => c.Id ==Convert.ToInt32(Selected.ClientId)).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().TextOption1;
                    }
                }
                else if (e.ColumnIndex == ColumnClientName.Index)
                {
                    var Query = _ClientDataSource.Where(c => c.Id == Convert.ToInt32(Selected.ClientId)).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().Text;
                    }
                }
            }
            else if (e.ColumnIndex == bizGubunDataGridViewTextBoxColumn.Index)
            {
                var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.CustomersRow;
                e.Value = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NCustomerGubun" && c.Value == Selected.BizGubun).First().Name;

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
            cmb_Search.SelectedIndex = 0;
            cmb_SalesSearch.SelectedIndex = 0;
            cmb_IGubun.SelectedIndex = 0;
            txt_Search.Text = "";
            if (cmb_SubClientId.Items.Count > 0)
                cmb_SubClientId.SelectedIndex = 0;
            _Search();
        }

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {

            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;

            var fileString = "거래처정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
                if (grid1.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible)
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



            //if (grid1.Rows.Count == 0)
            //{
            //    MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

            //DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);

            //if (di.Exists == false)
            //{
            //    di.Create();
            //}
            //var fileString = "거래처정보_일괄등록_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //FileInfo file = new FileInfo(FileName);
            //if (file.Exists)
            //{
            //    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        return;
            //}
            //ExcelPackage _Excel = null;
            //using (MemoryStream _Stream = new MemoryStream(Properties.Resources.Customer11))
            //{
            //    _Stream.Seek(0, SeekOrigin.Begin);
            //    _Excel = new ExcelPackage(_Stream);
            //}
            //var _Sheet = _Excel.Workbook.Worksheets[1];
            //var RowIndex = 2;
            //var _Index = 1;
            //foreach (DataGridViewRow row in grid1.Rows)
            //{
            //    var _Model = ((DataRowView)row.DataBoundItem).Row as ClientDataSet.CustomersRow;
            //    var _RegNo = _Model.ResgisterNo.Replace("-", "").Trim();
            //    if(_RegNo.Length > 6)
            //    {
            //        _RegNo = _RegNo.Substring(0, 6) + "-" + _RegNo.Substring(6);
            //    }


            //    var _BizGubin = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NCustomerGubun" && c.Value == _Model.BizGubun).First().Name;


            //    _Sheet.Cells[RowIndex, 1].Value = _Index.ToString();
            //    _Sheet.Cells[RowIndex, 2].Value = _Model.BizNo;
            //    _Sheet.Cells[RowIndex, 3].Value = _Model.SangHo;
            //    _Sheet.Cells[RowIndex, 4].Value = _Model.Ceo;
            //    _Sheet.Cells[RowIndex, 5].Value = _Model.Uptae;
            //    _Sheet.Cells[RowIndex, 6].Value = _Model.Upjong;
            //    _Sheet.Cells[RowIndex, 7].Value = _Model.Zipcode;
            //    _Sheet.Cells[RowIndex, 8].Value = _Model.AddressState;
            //    _Sheet.Cells[RowIndex, 9].Value = _Model.AddressCity;
            //    _Sheet.Cells[RowIndex, 10].Value = _Model.AddressDetail;
            //    _Sheet.Cells[RowIndex, 11].Value = _BizGubin;

            //    _Sheet.Cells[RowIndex, 12].Value = _RegNo;
            //    _Sheet.Cells[RowIndex, 13].Value = _Model.SalesGubun;
            //    _Sheet.Cells[RowIndex, 14].Value = _Model.Email;
            //    _Sheet.Cells[RowIndex, 15].Value = _Model.PhoneNo;
            //    _Sheet.Cells[RowIndex, 16].Value = _Model.ChargeName;
            //    _Sheet.Cells[RowIndex, 17].Value = _Model.MobileNo;
            //    _Sheet.Cells[RowIndex, 18].Value = _Model.Misu;
            //    _Sheet.Cells[RowIndex, 19].Value = _Model.Mizi;
            //    _Sheet.Cells[RowIndex, 20].Value = _Model.Remark;

            //    RowIndex++;
            //    _Index++;
            //}
            //try
            //{
            //    _Excel.SaveAs(file);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
            //    return;
            //}
            //System.Diagnostics.Process.Start(FileName);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!base.ProcessCmdKey(ref msg, keyData))
            {
                if (keyData.Equals(Keys.F1))
                {
                    //MessageBox.Show("F1");

                   // return true;
                }
                else
                {
                   // MessageBox.Show("false");
                  //  return false;
                }
            }
            else
            {
               // return true;

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

           
            //  var row = grid1.Rows[e.RowIndex];
            if (e.ColumnIndex == 20)
            {
                var row = grid1.Rows[e.RowIndex];

                var Data = ((DataRowView)row.DataBoundItem).Row as ClientDataSet.CustomersRow;
                if (Data != null)
                {
                    if (AccDictionary.ContainsKey(Data.CustomerId) && AccDictionary[Data.CustomerId])
                    {
                        grid1[e.ColumnIndex, e.RowIndex].Value = "O";
                    }
                }
            }

            var _Row = clientDataSet.Customers[e.RowIndex];
            if (String.IsNullOrEmpty(_Row.BizNo) || _Row.BizNo.Substring(0,3) =="000" || _Row.BizNo.Substring(0, 3) == "999" || String.IsNullOrEmpty(_Row.Ceo) || String.IsNullOrEmpty(_Row.Uptae) || String.IsNullOrEmpty(_Row.Upjong) || _Row.Ceo == "." || _Row.Upjong == "." || _Row.Uptae == "." || String.IsNullOrEmpty(_Row.AddressState) || String.IsNullOrEmpty(_Row.AddressCity) || String.IsNullOrEmpty(_Row.AddressDetail))
            {

                grid1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;

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
        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        List<BasicModel> _ClientDataSource = new List<BasicModel>();
        private void _InitCmb()
        {
            Dictionary<int, int> EndDay = new Dictionary<int, int>();
            for (int i = 1; i <= 31; i++)
            {


                EndDay.Add(i, i);

            }

            cmb_EndDay.DataSource = new BindingSource(EndDay, null);
            cmb_EndDay.DisplayMember = "Value";
            cmb_EndDay.ValueMember = "Key";


            var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NCustomerGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun.DataSource = GubunSourceDataSource;
            cmb_Gubun.DisplayMember = "Name";
            cmb_Gubun.ValueMember = "Value";


            var IGubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NCustomerGubun").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
          
          
            cmb_IGubun.DataSource = IGubunSourceDataSource;
            cmb_IGubun.DisplayMember = "Name";
            cmb_IGubun.ValueMember = "Value";

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
                     
                    }
                    _Connection.Close();
                }
            }
        }
        #endregion

        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmb_SalesGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void customersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
           // tabContainer.SelectedTab = BasicPage;

            if (customersBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected != null)
            {

                var Query1 =
                      "Select Count(*) From Orders Where ReferralId = @ReferralId ";


                bool IsDuplicated = false;
                bool IsDuplicated2 = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@ReferralId", Selected.CustomerId);
                   
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }

                    cn.Close();
                }


                var Query2 =
                     "Select Count(*) From ReferralAccount Where CustomerId = @CustomerId ";


             
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query2;
                    cmd1.Parameters.AddWithValue("@CustomerId", Selected.CustomerId);

                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated2 = true;
                    }

                    cn.Close();
                }


                if (IsDuplicated || IsDuplicated2)
                {
                  //  btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                }
                else
                {
                   // btnUpdate.Enabled = true;
                    btnCurrentDelete.Enabled = true;
                }
                if (!LocalUser.Instance.LogInInformation.IsClient)
                {
                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;


                }

                //if(Selected.Ccid == LocalUser.Instance.LogInInformation.ClientId.ToString())
                //{
                //    btnCurrentDelete.Enabled = true;
                //}
                //else
                //{
                //    btnCurrentDelete.Enabled = false;

                //}

                // cmbCustomerMId.SelectedValue = Selected.CustomerManagerId.ToString();
                if (LocalUser.Instance.LogInInformation.ClientId == 290)
                {
                    if (Selected.SalesGubun != 2)
                    {
                        cmb_EndDay.Visible = true;
                        lbl_EndDay.Visible = true;
                        cmb_EndDay.SelectedValue = Selected.EndDay;
                    }
                    else
                    {
                        cmb_EndDay.Visible = false;
                        lbl_EndDay.Visible = false;
                    }
                }

                txtMisu.Text = Selected.Misu.ToString("N0");
                txtMizi.Text = Selected.Mizi.ToString("N0");
                textBox1.Text = Selected.Remark;

               


                newDGV1Binding(Selected.CustomerId);
                newDGV2Binding(Selected.CustomerId);
                newDGV4Binding(Selected.CustomerId);
                newDGV5Binding(Selected.CustomerId);
                ImagePageView();

                
            }
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }
      
        private void Image1_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected == null)
                return;
            var _sender = ((PictureBox)sender).Name;
            var Id = 0;
            if (Selected[_sender] != DBNull.Value)
                Id = Convert.ToInt32(Selected[_sender]);
            if(Id == 0)
            {
                OpenFileDialog d = new OpenFileDialog();
                d.DefaultExt = "png";
                if(d.ShowDialog() == DialogResult.OK)
                {
                    Bitmap b;
                    Bitmap t;
                    try
                    {
                        b = new Bitmap(d.FileName);
                        if (b.Width < 190 && b.Height < 190)
                        {
                            t = (Bitmap)b.Clone();
                        }
                        else
                        {
                            if (b.Width > b.Height)
                            {
                                if (b.Width > 1000)
                                {
                                    int s = (int)Math.Floor((double)b.Height * 1000d / (double)b.Width);
                                    b = new Bitmap(b, new Size(1000, s));
                                }
                                int s1 = (int)Math.Floor((double)b.Height * 190d / (double)b.Width);
                                t = new Bitmap(b, new Size(190, s1));
                            }
                            else
                            {
                                if (b.Height > 1000)
                                {
                                    int s = (int)Math.Floor((double)b.Width * 1000d / (double)b.Height);
                                    b = new Bitmap(b, new Size(s, 1000));
                                }
                                int s1 = (int)Math.Floor((double)b.Width * 190d / (double)b.Height);
                                t = new Bitmap(b, new Size(s1, 190));
                            }
                        }
                        var Value = new ImageViewModel()
                        {
                            Name = ((TextBox)((PictureBox)sender).Tag).Text,
                        };
                        using (MemoryStream _Stream = new MemoryStream())
                        {
                            EncoderParameters mEncoderParameters = new EncoderParameters(1);
                            mEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                            b.Save(_Stream, GetEncoder(ImageFormat.Png), mEncoderParameters);
                            Value.ImageData64String = Convert.ToBase64String(_Stream.ToArray());
                        }
                        using (MemoryStream _Stream = new MemoryStream())
                        {
                            EncoderParameters mEncoderParameters = new EncoderParameters(1);
                            mEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                            t.Save(_Stream, GetEncoder(ImageFormat.Png), mEncoderParameters);
                            Value.ThumnailData64String = Convert.ToBase64String(_Stream.ToArray());
                        }
                        if (b != null)
                            b.Dispose();

                        var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetCustomerImage"));
                        // var http = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/ImageFromApp/SetCustomerImage"));
                        http.Accept = "application/json";
                        http.ContentType = "application/json; charset=utf-8";
                        http.Method = "POST";

                        string parsedContent = JsonConvert.SerializeObject(Value);
                        UTF8Encoding encoding = new UTF8Encoding();
                        Byte[] buffer = encoding.GetBytes(parsedContent);

                        Stream newStream = http.GetRequestStream();
                        newStream.Write(buffer, 0, buffer.Length);
                        newStream.Close();

                        var response = http.GetResponse();

                        var stream = response.GetResponseStream();
                        var sr = new StreamReader(stream);
                        var content = sr.ReadToEnd();

                        Selected[_sender] = Convert.ToInt32(((JObject)JsonConvert.DeserializeObject(content)).GetValue("ImageReferenceId"));

                       
                        if (sender == Image1)
                        {
                            Selected.Image1 = Convert.ToInt32(((JObject)JsonConvert.DeserializeObject(content)).GetValue("ImageReferenceId"));
                        }
                        if (sender == Image2)
                        {
                            Selected.Image2 = Convert.ToInt32(((JObject)JsonConvert.DeserializeObject(content)).GetValue("ImageReferenceId"));
                        }
                        if (sender == Image3)
                        {
                            Selected.Image3 = Convert.ToInt32(((JObject)JsonConvert.DeserializeObject(content)).GetValue("ImageReferenceId"));
                        }
                        if (sender == Image4)
                        {
                            Selected.Image4 = Convert.ToInt32(((JObject)JsonConvert.DeserializeObject(content)).GetValue("ImageReferenceId"));
                        }
                        if (sender == Image5)
                        {
                            Selected.Image5 = Convert.ToInt32(((JObject)JsonConvert.DeserializeObject(content)).GetValue("ImageReferenceId"));
                        }

                        Data.Connection(_Connection =>
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText =
                                @" UPDATE Customers
                            SET Image1 =@Image1
                                ,Image2 =@Image2
                                ,Image3 =@Image3
                                ,Image4 =@Image4
                                ,Image5 =@Image5

                            WHERE CustomerId = @CustomerId ";
                                _Command.Parameters.AddWithValue("@Image1", Selected.Image1);

                                if (Selected.Image2 == 0)
                                {
                                    _Command.Parameters.AddWithValue("@Image2", DBNull.Value);
                                }
                                else
                                {
                                    _Command.Parameters.AddWithValue("@Image2", Selected.Image2);
                                }

                                if (Selected.Image3 == 0)
                                {
                                    _Command.Parameters.AddWithValue("@Image3", DBNull.Value);
                                }
                                else
                                {
                                    _Command.Parameters.AddWithValue("@Image3", Selected.Image3);
                                }
                                if (Selected.Image4 == 0)
                                {
                                    _Command.Parameters.AddWithValue("@Image4", DBNull.Value);
                                }
                                else
                                {
                                    _Command.Parameters.AddWithValue("@Image4", Selected.Image4);
                                }
                                if (Selected.Image5 == 0)
                                {
                                    _Command.Parameters.AddWithValue("@Image5", DBNull.Value);
                                }
                                else
                                {
                                    _Command.Parameters.AddWithValue("@Image5", Selected.Image5);
                                }
                                //_Command.Parameters.AddWithValue("@Image3", Selected.Image3);
                                //_Command.Parameters.AddWithValue("@Image4", Selected.Image4);
                                //_Command.Parameters.AddWithValue("@Image5", Selected.Image5);

                                _Command.Parameters.AddWithValue("@CustomerId", Selected.CustomerId);

                                _Command.ExecuteNonQuery();
                            }


                        });


                       // customersTableAdapter.Update(Selected);
                        ((PictureBox)sender).Image = t;
                        if (sender == Image1)
                        {
                            ImageDelete1.Enabled = true;
                            ImageUpdate1.Enabled = true;
                            Image1.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image2)
                        {
                            ImageDelete2.Enabled = true;
                            ImageUpdate2.Enabled = true;
                            Image2.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image3)
                        {
                            ImageDelete3.Enabled = true;
                            ImageUpdate3.Enabled = true;
                            Image3.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image4)
                        {
                            ImageDelete4.Enabled = true;
                            ImageUpdate4.Enabled = true;
                            Image4.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image5)
                        {
                            ImageDelete5.Enabled = true;
                            ImageUpdate5.Enabled = true;
                            Image5.Cursor = Cursors.Arrow;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("올바른 사진(그림) 파일을 선택하여 주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private void newDGV1Binding(int CustomerId)
        {
            customerStartManageDataSet.CustomerStartManage.Clear();

            //Load DriverPoints
            if (customersBindingSource.Current as DataRowView == null)
                return;
            var _Selected = (customersBindingSource.Current as DataRowView).Row as ClientDataSet.CustomersRow;
            if (_Selected == null)
                return;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText =

                      @"select * from customerstartmanage where CustomerId = @CustomerId AND SGubun = 'S'"+
                       " AND (StartState like '%'+@SearchText+'%'  OR StartCity like '%'+@SearchText+'%' OR StartStreet like '%'+@SearchText+'%' OR StartDetail like '%'+@SearchText+'%' OR StartName like '%'+@SearchText+'%') " +
                        "order by CreateTime DESC";

              
                _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                _Command.Parameters.AddWithValue("@SearchText", txtStartSearch.Text);
                using (SqlDataReader _Reader = _Command.ExecuteReader())
                {
                    customerStartManageDataSet.CustomerStartManage.Load(_Reader);
                }
            }
        }

        private void newDGV2Binding(int CustomerId)
        {
            customerStartManageDataSet.CustomerAddPhone.Clear();

            //Load DriverPoints
            if (customersBindingSource.Current as DataRowView == null)
                return;
            var _Selected = (customersBindingSource.Current as DataRowView).Row as ClientDataSet.CustomersRow;
            if (_Selected == null)
                return;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();

                if (LocalUser.Instance.LogInInformation.CustomerUserId != 0)
                {
                    _Command.CommandText =

                          @"select idx, AddTeam, AddName, AddPhoneNo, ClientId, CustomerId, CreateTime,Remark, LoginId, Password, Rank, Email, MobilePhoneNo,TeamId from CustomerAddPhone where CustomerId = @CustomerId and TeamId = @TeamId                       
                        order by CreateTime DESC";

                    _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                    _Command.Parameters.AddWithValue("@TeamId", LocalUser.Instance.LogInInformation.CustomerTeamId);
                }
                else
                {
                    _Command.CommandText =

                         @"select idx, AddTeam, AddName, AddPhoneNo, ClientId, CustomerId, CreateTime,Remark, LoginId, Password, Rank, Email, MobilePhoneNo,TeamId from CustomerAddPhone where CustomerId = @CustomerId
                       
                        order by CreateTime DESC";

                    _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                }

               
                using (SqlDataReader _Reader = _Command.ExecuteReader())
                {
                    customerStartManageDataSet.CustomerAddPhone.Load(_Reader);
                }

                
            }
        }
        private void tabContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabContainer.SelectedTab == ImagePage)
            {
                Image1.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                Image2.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                Image3.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                Image4.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                Image5.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                Image1.Cursor = Cursors.Hand;
                Image2.Cursor = Cursors.Hand;
                Image3.Cursor = Cursors.Hand;
                Image4.Cursor = Cursors.Hand;
                Image5.Cursor = Cursors.Hand;
                ImageName1.Clear();
                ImageName2.Clear();
                ImageName3.Clear();
                ImageName4.Clear();
                ImageName5.Clear();
                ImageDelete1.Enabled = false;
                ImageDelete2.Enabled = false;
                ImageDelete3.Enabled = false;
                ImageDelete4.Enabled = false;
                ImageDelete5.Enabled = false;
                ImageUpdate1.Enabled = false;
                ImageUpdate2.Enabled = false;
                ImageUpdate3.Enabled = false;
                ImageUpdate4.Enabled = false;
                ImageUpdate5.Enabled = false;
                if (customersBindingSource.Current == null)
                    return;
                var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
                if (Selected == null)
                    return;
                if (Selected.Image1 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image1.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName1.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image1.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete1.Enabled = true;
                    ImageUpdate1.Enabled = true;
                    Image1.Cursor = Cursors.Arrow;
                }
                if (Selected.Image2 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image2.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName2.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image2.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete2.Enabled = true;
                    ImageUpdate2.Enabled = true;
                    Image2.Cursor = Cursors.Arrow;
                }
                if (Selected.Image3 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image3.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName3.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image3.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete3.Enabled = true;
                    ImageUpdate3.Enabled = true;
                    Image3.Cursor = Cursors.Arrow;
                }
                if (Selected.Image4 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image4.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName4.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image4.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete4.Enabled = true;
                    ImageUpdate4.Enabled = true;
                    Image4.Cursor = Cursors.Arrow;
                }
                if (Selected.Image5 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image5.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName5.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image5.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete5.Enabled = true;
                    ImageUpdate5.Enabled = true;
                    Image5.Cursor = Cursors.Arrow;
                }
            }

            else if(tabContainer.SelectedTab == tabPage2)
            {
                if (customersBindingSource.Current == null)
                    return;

                var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;



                Dictionary<int, string> DCustomer = new Dictionary<int, string>();

                customerTeamsTableAdapter.Fill(customerUserDataSet.CustomerTeams, LocalUser.Instance.LogInInformation.ClientId);
                var cmbCustomerTeamataSource = customerUserDataSet.CustomerTeams.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == Selected.CustomerId).Select(c => new { c.TeamName, c.CustomerTeamId }).OrderBy(c => c.CustomerTeamId).ToArray();

                DCustomer.Add(0, "부서선택");


                foreach (var item in cmbCustomerTeamataSource)
                {
                    DCustomer.Add(item.CustomerTeamId, item.TeamName);
                }


                cmbAddTeam.DataSource = new BindingSource(DCustomer, null);
                cmbAddTeam.DisplayMember = "Value";
                cmbAddTeam.ValueMember = "Key";

                cmbAddTeam.SelectedValue = 0;

                newDGV2Binding(Selected.CustomerId);

            }
            else if(tabContainer.SelectedTab == tabPage4 )
            {
                if (customersBindingSource.Current == null)
                    return;

                var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;

                newDGV5Binding(Selected.CustomerId);

            }


        }
        private void ImagePageView()
        {


            Image1.Image = Properties.Resources.ic_photo_gray_48dp_2x;
            Image2.Image = Properties.Resources.ic_photo_gray_48dp_2x;
            Image3.Image = Properties.Resources.ic_photo_gray_48dp_2x;
            Image4.Image = Properties.Resources.ic_photo_gray_48dp_2x;
            Image5.Image = Properties.Resources.ic_photo_gray_48dp_2x;
            Image1.Cursor = Cursors.Hand;
            Image2.Cursor = Cursors.Hand;
            Image3.Cursor = Cursors.Hand;
            Image4.Cursor = Cursors.Hand;
            Image5.Cursor = Cursors.Hand;
            ImageName1.Clear();
            ImageName2.Clear();
            ImageName3.Clear();
            ImageName4.Clear();
            ImageName5.Clear();
            ImageDelete1.Enabled = false;
            ImageDelete2.Enabled = false;
            ImageDelete3.Enabled = false;
            ImageDelete4.Enabled = false;
            ImageDelete5.Enabled = false;
            ImageUpdate1.Enabled = false;
            ImageUpdate2.Enabled = false;
            ImageUpdate3.Enabled = false;
            ImageUpdate4.Enabled = false;
            ImageUpdate5.Enabled = false;
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected == null)
                return;
            if (Selected.Image1 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image1.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName1.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image1.Image = new Bitmap(_Stream);
                        }
                    }
                }
                ImageDelete1.Enabled = true;
                ImageUpdate1.Enabled = true;
                Image1.Cursor = Cursors.Arrow;
            }
            if (Selected.Image2 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image2.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName2.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image2.Image = new Bitmap(_Stream);
                        }
                    }
                }
                ImageDelete2.Enabled = true;
                ImageUpdate2.Enabled = true;
                Image2.Cursor = Cursors.Arrow;
            }
            if (Selected.Image3 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image3.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName3.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image3.Image = new Bitmap(_Stream);
                        }
                    }
                }
                ImageDelete3.Enabled = true;
                ImageUpdate3.Enabled = true;
                Image3.Cursor = Cursors.Arrow;
            }
            if (Selected.Image4 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image4.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName4.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image4.Image = new Bitmap(_Stream);
                        }
                    }
                }
                ImageDelete4.Enabled = true;
                ImageUpdate4.Enabled = true;
                Image4.Cursor = Cursors.Arrow;
            }
            if (Selected.Image5 > 0)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image5.ToString());
                    if (r != null)
                    {
                        var J = ((JObject)JsonConvert.DeserializeObject(r));
                        ImageName5.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                        var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                        using (MemoryStream _Stream = new MemoryStream(Buffer))
                        {
                            Image5.Image = new Bitmap(_Stream);
                        }
                    }
                }
                ImageDelete5.Enabled = true;
                ImageUpdate5.Enabled = true;
                Image5.Cursor = Cursors.Arrow;
            }


        }
        private void ImageDelete1_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected == null)
                return;
            var ImageReferenceId = 0;
            if (sender == ImageDelete1)
                ImageReferenceId = Selected.Image1;
            if (sender == ImageDelete2)
                ImageReferenceId = Selected.Image2;
            if (sender == ImageDelete3)
                ImageReferenceId = Selected.Image3;
            if (sender == ImageDelete4)
                ImageReferenceId = Selected.Image4;
            if (sender == ImageDelete5)
                ImageReferenceId = Selected.Image5;
            if (ImageReferenceId == 0)
                return;
            if (MessageBox.Show("정말 사진(그림)을 삭제하겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/DeleteCustomImagel?CustomerId=" + Selected.CustomerId.ToString() + "&ImageReferenceId=" + ImageReferenceId.ToString());
                }
                if (sender == ImageDelete1)
                {
                    Selected.Image1 = 0;
                    Image1.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName1.Clear();
                    ImageDelete1.Enabled = false;
                    ImageUpdate1.Enabled = false;
                }
                if (sender == ImageDelete2)
                {
                    Selected.Image2 = 0;
                    Image2.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName2.Clear();
                    ImageDelete2.Enabled = false;
                    ImageUpdate2.Enabled = false;
                }
                if (sender == ImageDelete3)
                {
                    Selected.Image3 = 0;
                    Image3.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName3.Clear();
                    ImageDelete3.Enabled = false;
                    ImageUpdate3.Enabled = false;
                }
                if (sender == ImageDelete4)
                {
                    Selected.Image4 = 0;
                    Image4.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName4.Clear();
                    ImageDelete4.Enabled = false;
                    ImageUpdate4.Enabled = false;
                }
                if (sender == ImageDelete5)
                {
                    Selected.Image5 = 0;
                    Image5.Image = Properties.Resources.ic_photo_gray_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName5.Clear();
                    ImageDelete5.Enabled = false;
                    ImageUpdate5.Enabled = false;
                }
                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
                        @" UPDATE Customers
                            SET Image1 =@Image1
                                ,Image2 =@Image2
                                ,Image3 =@Image3
                                ,Image4 =@Image4
                                ,Image5 =@Image5

                            WHERE CustomerId = @CustomerId ";
                        if (Selected.Image1 == 0)
                        {
                            _Command.Parameters.AddWithValue("@Image1", DBNull.Value);
                        }
                        else
                        {
                            _Command.Parameters.AddWithValue("@Image1", Selected.Image1);
                        }

                        if (Selected.Image2 == 0)
                        {
                            _Command.Parameters.AddWithValue("@Image2", DBNull.Value);
                        }
                        else
                        {
                            _Command.Parameters.AddWithValue("@Image2", Selected.Image2);
                        }

                        if (Selected.Image3 == 0)
                        {
                            _Command.Parameters.AddWithValue("@Image3", DBNull.Value);
                        }
                        else
                        {
                            _Command.Parameters.AddWithValue("@Image3", Selected.Image3);
                        }
                        if (Selected.Image4 == 0)
                        {
                            _Command.Parameters.AddWithValue("@Image4", DBNull.Value);
                        }
                        else
                        {
                            _Command.Parameters.AddWithValue("@Image4", Selected.Image4);
                        }
                        if (Selected.Image5 == 0)
                        {
                            _Command.Parameters.AddWithValue("@Image5", DBNull.Value);
                        }
                        else
                        {
                            _Command.Parameters.AddWithValue("@Image5", Selected.Image5);
                        }
                   

                        _Command.Parameters.AddWithValue("@CustomerId", Selected.CustomerId);

                        _Command.ExecuteNonQuery();
                    }


                });

                // customersTableAdapter.Update(Selected);
            }
        }

        private void ImageUpdate1_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected == null)
                return;
            if (sender == ImageUpdate1)
            {
                if(Selected.Image1 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName1.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image1);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate2)
            {
                if (Selected.Image2 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName2.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image2);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate3)
            {
                if (Selected.Image3 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName3.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image3);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate4)
            {
                if (Selected.Image4 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName4.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image4);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate5)
            {
                if (Selected.Image5 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName5.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image5);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void txt_BizNo_Enter(object sender, EventArgs e)
        {
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

        private void txt_Fee_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
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
            Process.Start("https://blog.naver.com/edubill365/221372085635");
        }

        private void StartPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void StartPhoneNo_Leave(object sender, EventArgs e)
        {
            //if (!String.IsNullOrWhiteSpace(StartPhoneNo.Text))
          
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

        private void btnStartFindZip_Click(object sender, EventArgs e)
        {
            FindZipNew f = new Admin.FindZipNew();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                if (f.rdoRoad.Checked)
                {
                    // txt_Zip.Text = f.Zip;
                    var ss = f.Address.Split(' ');
                    StartState.Text = ss[0];
                    StartCity.Text = ss[1];
                    StartStreet.Text = ss[2];
                    Start.Text = String.Join(" ", ss.Skip(3));

                    //StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                    Start.Focus();
                }
                else if (f.rdoJibun.Checked)
                {
                    var ss = f.Jibun.Split(' ');
                    StartState.Text = ss[0];
                    StartCity.Text = ss[1];
                    StartStreet.Text = ss[2];
                    Start.Text = String.Join(" ", ss.Skip(3));
                    //StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                    Start.Focus();
                }
            }
        }

        private string _AddressStateParse(String _Value)
        {
            string R = "";
            switch (_Value)
            {
                case "강원도":
                    R = "강원";
                    break;
                case "경기도":
                    R = "경기";
                    break;
                case "경상남도":
                    R = "경남";
                    break;
                case "경상북도":
                    R = "경북";
                    break;
                case "광주광역시":
                    R = "광주";
                    break;
                case "대구광역시":
                    R = "대구";
                    break;
                case "대전광역시":
                    R = "대전";
                    break;
                case "부산광역시":
                    R = "부산";
                    break;
                case "서울특별시":
                    R = "서울";
                    break;
                case "세종특별자치시":
                    R = "세종";
                    break;
                case "울산광역시":
                    R = "울산";
                    break;
                case "인천광역시":
                    R = "인천";
                    break;
                case "전라남도":
                    R = "전남";
                    break;
                case "전라북도":
                    R = "전북";
                    break;
                case "제주특별자치도":
                    R = "제주";
                    break;
                case "충청남도":
                    R = "충남";
                    break;
                case "충청북도":
                    R = "충북";
                    break;
                default:
                    break;
            }
            return R;
        }

        private void btnStartNew_Click(object sender, EventArgs e)
        {
            txtIdx.Clear();
            StartState.Text = "";
            StartCity.Text = "";
            StartStreet.Text = "";
            Start.Text = "";
            StartName.Text = "";
            StartPhoneNo.Text = "";
            
            btnStartUpdate.Text = "저 장";

        }

        private void btnStartUpdate_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;

            if(String.IsNullOrEmpty(StartState.Text) || String.IsNullOrEmpty(StartCity.Text))
            {
                MessageBox.Show("상차지 주소를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var _Current = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;

            if (!String.IsNullOrWhiteSpace(txtIdx.Text))
            {
               
                //수정
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "Update customerstartmanage SET StartState = @StartState" +
                        ", StartCity = @StartCity" +
                        ", StartStreet = @StartStreet" +
                        ", StartDetail = @StartDetail" +
                        ", StartName = @StartName" +
                        ", StartPhoneNo = @StartPhoneNo" +
                        
                        " WHERE idx = @idx ";
                    _Command.Parameters.AddWithValue("@StartState", StartState.Text);
                    _Command.Parameters.AddWithValue("@StartCity", StartCity.Text);
                    _Command.Parameters.AddWithValue("@StartStreet", StartStreet.Text);
                    _Command.Parameters.AddWithValue("@StartDetail", Start.Text);
                    _Command.Parameters.AddWithValue("@StartName", StartName.Text);
                    _Command.Parameters.AddWithValue("@StartPhoneNo", StartPhoneNo.Text);

                    _Command.Parameters.AddWithValue("@idx", txtIdx.Text);
                   // _Command.Parameters.AddWithValue("@CustomerId", _Current.CustomerId);

                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV1Binding(_Current.CustomerId);
                    //tabContainer_SelectedIndexChanged(null, null);
                }

            }
            else
            {
                //신규
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "INSERT INTO customerstartmanage (StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,SGubun) VALUES (@StartState, @StartCity, @StartStreet, @StartDetail, @StartName, @StartPhoneNo, @ClientId, @CustomerId, getdate(),'S')";
                    _Command.Parameters.AddWithValue("@StartState", StartState.Text);
                    _Command.Parameters.AddWithValue("@StartCity", StartCity.Text);
                    _Command.Parameters.AddWithValue("@StartStreet", StartStreet.Text);
                    _Command.Parameters.AddWithValue("@StartDetail", Start.Text);
                    _Command.Parameters.AddWithValue("@StartName", StartName.Text);
                    _Command.Parameters.AddWithValue("@StartPhoneNo", StartPhoneNo.Text);

                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@CustomerId", _Current.CustomerId);

                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV1Binding(_Current.CustomerId);
                    // tabContainer_SelectedIndexChanged(null, null);
                }
            }
        }

        private void btnStartDelete_Click(object sender, EventArgs e)
        {
            if (customerStartManageBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)customerStartManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStartManageRow;
            if (MessageBox.Show("정말 상차지정보를 삭제하겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "DELETE customerstartmanage WHERE  Idx = @Idx";
                    _Command.Parameters.AddWithValue("@Idx", Selected.idx);


                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV1Binding(Selected.CustomerId);
                    // tabContainer_SelectedIndexChanged(null, null);
                }
            }
        }

        private void customerStartManageBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (customerStartManageBindingSource.Current == null)
            {
                txtIdx.Clear();
                StartState.Text = "";
                StartCity.Text = "";
                StartStreet.Text = "";
                Start.Text = "";
                StartName.Text = "";
                StartPhoneNo.Text = "";
                btnStartUpdate.Text = "저 장";
                return;
            }

            var Selected = ((DataRowView)customerStartManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStartManageRow;
            if (Selected != null)
            {
                txtIdx.Text = Selected.idx.ToString();
                StartState.Text = Selected.StartState;
                StartCity.Text = Selected.StartCity;
                StartStreet.Text = Selected.StartStreet;
                Start.Text = Selected.StartDetail;
                StartName.Text = Selected.StartName;
                StartPhoneNo.Text = Selected.StartPhoneNo;

                btnStartUpdate.Text = "수 정";

            }
            else
            {
                txtIdx.Clear();
                StartState.Text = "";
                StartCity.Text = "";
                StartStreet.Text = "";
                Start.Text = "";
                StartName.Text = "";
                StartPhoneNo.Text = "";
                btnStartUpdate.Text = "저 장";
            }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
          //  var Selected = ((DataRowView)customerStartManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStartManageRow;

            if (e.ColumnIndex == dataGridViewTextBoxColumn1.Index)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString();
            }
            if(e.ColumnIndex == Column1.Index)
            {
                var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CustomerStartManageDataSet.CustomerStartManageRow;
                e.Value = Selected.StartState + " " + Selected.StartCity + " " + Selected.StartStreet + " " + Selected.StartDetail;
            }
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,
                No,
                //customerIdDataGridViewTextBoxColumn,
               //ColumnClientCode,
               //ColumnClientName,
               codeDataGridViewTextBoxColumn,
               bizGubunDataGridViewTextBoxColumn,
               //SubClientId,
               salesGubunDataGridViewTextBoxColumn,
               bizNoDataGridViewTextBoxColumn,
               sangHoDataGridViewTextBoxColumn,
               ceoDataGridViewTextBoxColumn,
               //uptaeDataGridViewTextBoxColumn,
               //upjongDataGridViewTextBoxColumn,
               //addressStateDataGridViewTextBoxColumn,
               //addressCityDataGridViewTextBoxColumn,
               //addressDetailDataGridViewTextBoxColumn,
               //resgisterNoDataGridViewTextBoxColumn,
               emailDataGridViewTextBoxColumn,
               phoneNoDataGridViewTextBoxColumn,
               faxNoDataGridViewTextBoxColumn,
               chargeNameDataGridViewTextBoxColumn,
               mobileNoDataGridViewTextBoxColumn,
               createTimeDataGridViewTextBoxColumn




                );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);
        }

        private void txtAddPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txtAddPhoneNo_Leave(object sender, EventArgs e)
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

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            //  var Selected = ((DataRowView)customerStartManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStartManageRow;

            if (e.ColumnIndex == dataGridViewTextBoxColumn2.Index)
            {
                e.Value = (newDGV2.Rows.Count - e.RowIndex).ToString();
            }
        }

        private void btnPNew_Click(object sender, EventArgs e)
        {

            newDGV2.CurrentCell = null;
            txtPIdx.Clear();
            txtAddTeam.Text = "";
            txtAddName.Text = "";
            txtAddPhoneNo.Text = "";
            txtRemark.Text = "";
            cmbAddTeam.SelectedValue = 0;
            txt_PRank.Text = "";
            txt_PEmail.Text = "";
            txt_UserLoginId.Enabled = true;
            txt_UserPassword.Enabled = true;
            txt_UserLoginId.Text = "";
            txt_UserPassword.Text = "";
            txtCMobilePhoneNo.Text = "";
            btnPUpdate.Text = "저 장";
        }

        private void btnPUpdate_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;

            //if (String.IsNullOrEmpty(txtAddTeam.Text))
            //{
            //    MessageBox.Show("부서를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtAddTeam.Focus();
            //    return;
            //}
            if (String.IsNullOrEmpty(txtAddName.Text))
            {
                MessageBox.Show("성명을 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddName.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtAddPhoneNo.Text))
            {
                MessageBox.Show("전화번호를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddPhoneNo.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtCMobilePhoneNo.Text))
            {
                MessageBox.Show("핸드폰번호를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCMobilePhoneNo.Focus();
                return;
            }
            var _Current = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
           
            
            if (!String.IsNullOrWhiteSpace(txtPIdx.Text))
            {
                var Selected = ((DataRowView)customerAddPhoneBindingSource.Current).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;
                if (String.IsNullOrEmpty(Selected.LoginId) && !String.IsNullOrEmpty(txt_UserLoginId.Text))
                {

                    //var Query1 = "Select Count(*) From CustomerAddPhone Where LoginId = @LoginId";
                    //var Query2 =
                    //    "Select Count(*) From Customers Where LoginId = @LoginId";
                    var Query1 = "SELECT COUNT(*) FROM " +
                     "(Select LoginId From ClientUsers " +
                     " union " +
                     " Select LoginId From clients " +
                     " union " +
                     " Select LoginId From Customers " +
                     " union " +
                     " Select LoginId From CustomerAddPhone) as a" +
                     " Where LoginId = @LoginId ";

                    bool IsDuplicated = false;
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();

                        SqlCommand cmd1 = cn.CreateCommand();
                        cmd1.CommandText = Query1;
                        cmd1.Parameters.AddWithValue("@LoginId", txt_UserLoginId.Text);
                        if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                        {
                            IsDuplicated = true;
                        }
                        //if (!IsDuplicated)
                        //{
                        //    SqlCommand cmd2 = cn.CreateCommand();
                        //    cmd2.CommandText = Query2;
                        //    cmd2.Parameters.AddWithValue("@LoginId", txt_UserLoginId.Text);
                        //    if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
                        //    {
                        //        IsDuplicated = true;
                        //    }
                        //}
                        cn.Close();
                    }
                    if (IsDuplicated)
                    {
                        MessageBox.Show("아이디가 중복되었습니다.!!", "아이디 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        err.SetError(txt_UserLoginId, "아이디가 중복되었습니다.!!");
                        txt_UserLoginId.Clear();
                        txt_UserLoginId.Focus();
                        return;
                    }

                    if (String.IsNullOrEmpty(txt_UserPassword.Text))
                    {
                        MessageBox.Show("비밀번호를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_UserPassword.Focus();
                        return;
                    }

                }

                //수정
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "Update CustomerAddPhone SET AddTeam = @AddTeam" +
                        ", AddName = @AddName" +
                        ", AddPhoneNo = @AddPhoneNo" +
                        ",MobilePhoneNo = @MobilePhoneNo"+
                        " ,Remark =  @Remark" +
                        " ,Rank =  @Rank" +
                        " ,Email =  @Email" +
                        ",TeamId = @TeamId " +
                        ",LoginId = @LoginId"+
                        ",Password = @Password"+
                        " WHERE idx = @idx ";
                    _Command.Parameters.AddWithValue("@AddTeam", cmbAddTeam.Text);
                    _Command.Parameters.AddWithValue("@AddName", txtAddName.Text);
                    _Command.Parameters.AddWithValue("@AddPhoneNo", txtAddPhoneNo.Text);
                    _Command.Parameters.AddWithValue("@MobilePhoneNo", txtCMobilePhoneNo.Text);
                    _Command.Parameters.AddWithValue("@Remark", txtRemark.Text);

                    _Command.Parameters.AddWithValue("@Rank", txt_PRank.Text);

                    _Command.Parameters.AddWithValue("@Email", txt_PEmail.Text);
                    _Command.Parameters.AddWithValue("@TeamId", cmbAddTeam.SelectedValue.ToString());
                    _Command.Parameters.AddWithValue("@LoginId", txt_UserLoginId.Text);

                    _Command.Parameters.AddWithValue("@Password", txt_UserPassword.Text);

                    _Command.Parameters.AddWithValue("@idx", txtPIdx.Text);
                   

                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV2Binding(_Current.CustomerId);
                    
                }

            }
            else
            {

                if (!String.IsNullOrEmpty(txt_UserLoginId.Text))
                {

                    //var Query1 = "Select Count(*) From CustomerAddPhone Where LoginId = @LoginId";
                    //var Query2 =
                    //    "Select Count(*) From Customers Where LoginId = @LoginId";

                    var Query1 = "SELECT COUNT(*) FROM " +
                 "(Select LoginId From ClientUsers " +
                 " union " +
                 " Select LoginId From clients " +
                 " union " +
                 " Select LoginId From Customers " +
                 " union " +
                 " Select LoginId From CustomerAddPhone) as a" +
                 " Where LoginId = @LoginId ";

                    bool IsDuplicated = false;
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();

                        SqlCommand cmd1 = cn.CreateCommand();
                        cmd1.CommandText = Query1;
                        cmd1.Parameters.AddWithValue("@LoginId", txt_UserLoginId.Text);
                        if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                        {
                            IsDuplicated = true;
                        }
                        //if (!IsDuplicated)
                        //{
                        //    SqlCommand cmd2 = cn.CreateCommand();
                        //    cmd2.CommandText = Query2;
                        //    cmd2.Parameters.AddWithValue("@LoginId", txt_UserLoginId.Text);
                        //    if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
                        //    {
                        //        IsDuplicated = true;
                        //    }
                        //}
                        cn.Close();
                    }
                    if (IsDuplicated)
                    {
                        MessageBox.Show("아이디가 중복되었습니다.!!", "아이디 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        err.SetError(txt_UserLoginId, "아이디가 중복되었습니다.!!");
                        txt_UserLoginId.Clear();
                        txt_UserLoginId.Focus();
                        return;
                    }

                    if(String.IsNullOrEmpty(txt_UserPassword.Text))
                    {
                        MessageBox.Show("비밀번호를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_UserPassword.Focus();
                        return;
                    }

                }
                //신규
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "INSERT INTO CustomerAddPhone (AddTeam, AddName, AddPhoneNo, ClientId, CustomerId, CreateTime,Remark,LoginId,Password,Rank,Email,MobilePhoneNo,TeamId) " +
                        " VALUES ( @AddTeam, @AddName, @AddPhoneNo, @ClientId, @CustomerId, getdate(),@Remark,@LoginId,@Password,@Rank,@Email,@MobilePhoneNo,@TeamId)";
                    _Command.Parameters.AddWithValue("@AddTeam", cmbAddTeam.Text);
                    _Command.Parameters.AddWithValue("@AddName", txtAddName.Text);
                    _Command.Parameters.AddWithValue("@AddPhoneNo", txtAddPhoneNo.Text);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@CustomerId", _Current.CustomerId);
                    _Command.Parameters.AddWithValue("@Remark", txtRemark.Text);
                    _Command.Parameters.AddWithValue("@LoginId", txt_UserLoginId.Text);
                    _Command.Parameters.AddWithValue("@Password", txt_UserPassword.Text);
                    _Command.Parameters.AddWithValue("@Rank", txt_PRank.Text);
                    _Command.Parameters.AddWithValue("@Email", txt_PEmail.Text);
                    _Command.Parameters.AddWithValue("@MobilePhoneNo", txtCMobilePhoneNo.Text);
                    _Command.Parameters.AddWithValue("@TeamId", cmbAddTeam.SelectedValue.ToString());

                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV2Binding(_Current.CustomerId);
                    // tabContainer_SelectedIndexChanged(null, null);
                }
            }
        }

        private void btnPDelete_Click(object sender, EventArgs e)
        {
            if (customerAddPhoneBindingSource.Current == null)
                return;
            
            var Selected = ((DataRowView)customerAddPhoneBindingSource.Current).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;
            if (MessageBox.Show("정말 담당자정보를 삭제하겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "DELETE CustomerAddPhone WHERE  Idx = @Idx";
                    _Command.Parameters.AddWithValue("@Idx", Selected.idx);


                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV2Binding(Selected.CustomerId);
                    // tabContainer_SelectedIndexChanged(null, null);
                }
            }
        }

        private void customerAddPhoneBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            

            if (customerAddPhoneBindingSource.Current == null)
            {
                txtPIdx.Clear();
                txtAddTeam.Text = "";
                txtAddName.Text = "";
                txtAddPhoneNo.Text = "";
                txtRemark.Text = "";
                btnPUpdate.Text = "저 장";
                cmbAddTeam.SelectedValue = 0;
                txt_UserLoginId.Text = "";
                txt_UserPassword.Text = "";
                txtCMobilePhoneNo.Text = "";
                txt_PRank.Text = "";
                txt_PEmail.Text = "";

                txt_UserLoginId.Enabled = true;
               // txt_UserPassword.Enabled = true;


                return;
            }
            
            var Selected = ((DataRowView)customerAddPhoneBindingSource.Current).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;
            if (Selected != null)
            {

                Dictionary<int, string> DCustomer = new Dictionary<int, string>();

                customerTeamsTableAdapter.Fill(customerUserDataSet.CustomerTeams, LocalUser.Instance.LogInInformation.ClientId);
                var cmbCustomerTeamataSource = customerUserDataSet.CustomerTeams.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == Selected.CustomerId).Select(c => new { c.TeamName, c.CustomerTeamId }).OrderBy(c => c.CustomerTeamId).ToArray();

                DCustomer.Add(0, "부서선택");


                foreach (var item in cmbCustomerTeamataSource)
                {
                    DCustomer.Add(item.CustomerTeamId, item.TeamName);
                }


                cmbAddTeam.DataSource = new BindingSource(DCustomer, null);
                cmbAddTeam.DisplayMember = "Value";
                cmbAddTeam.ValueMember = "Key";
                cmbAddTeam.SelectedValue = 0;

                txtPIdx.Text = Selected.idx.ToString();
                txtAddTeam.Text = Selected.AddTeam;
                txtAddName.Text = Selected.AddName;
                txtAddPhoneNo.Text = Selected.AddPhoneNo;
                txtRemark.Text = Selected.Remark;
                txt_UserLoginId.Text = Selected.LoginId;
                txt_UserPassword.Text = Selected.Password;
                txtCMobilePhoneNo.Text = Selected.MobilePhoneNo;
                

                cmbAddTeam.SelectedValue = Selected.TeamId;
                if (string.IsNullOrEmpty(Selected.LoginId))
                {
                    txt_UserLoginId.Enabled = true;
                    //txt_UserPassword.Enabled = true;
                }
                else
                {
                    txt_UserLoginId.Enabled = false;
                    //txt_UserPassword.Enabled = false;
                }
                txt_PRank.Text = Selected.Rank;
                txt_PEmail.Text = Selected.Email;

                btnPUpdate.Text = "수 정";

            }
            else
            {
                txtPIdx.Clear();
                txtAddTeam.Text = "";
                txtAddName.Text = "";
                txtAddPhoneNo.Text = "";
                txtRemark.Text = "";
                cmbAddTeam.SelectedValue = 0;
                btnPUpdate.Text = "저 장";
                txt_UserLoginId.Text = "";
                txt_UserPassword.Text = "";
                txtCMobilePhoneNo.Text = "";

                txt_UserLoginId.Enabled = true;
               // txt_UserPassword.Enabled = true;

                txt_PRank.Text = "";
                txt_PEmail.Text = "";


                return;

            }
        }
        bool IsCurrentNull = false;
        private void newDGV2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            customerAddPhoneBindingSource_CurrentChanged(null, null);

        }

        private void newDGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            
            customerStartManageBindingSource_CurrentChanged(null, null);
        }

        private void btnStopNew_Click(object sender, EventArgs e)
        {
            txtStopIdx.Clear();
            StopState.Text = "";
            StopCity.Text = "";
            StopStreet.Text = "";
            Stop.Text = "";
            StopName.Text = "";
            StopPhoneNo.Text = "";

            btnStopUpdate.Text = "저 장";
        }

        private void btnStopUpdate_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;

            if (String.IsNullOrEmpty(StopState.Text) || String.IsNullOrEmpty(StopCity.Text))
            {
                MessageBox.Show("하차지 주소를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var _Current = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;

            if (!String.IsNullOrWhiteSpace(txtStopIdx.Text))
            {

                //수정
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "Update customerstartmanage SET StartState = @StartState" +
                        ", StartCity = @StartCity" +
                        ", StartStreet = @StartStreet" +
                        ", StartDetail = @StartDetail" +
                        ", StartName = @StartName" +
                        ", StartPhoneNo = @StartPhoneNo" +

                        " WHERE idx = @idx ";
                    _Command.Parameters.AddWithValue("@StartState", StopState.Text);
                    _Command.Parameters.AddWithValue("@StartCity", StopCity.Text);
                    _Command.Parameters.AddWithValue("@StartStreet", StopStreet.Text);
                    _Command.Parameters.AddWithValue("@StartDetail", Stop.Text);
                    _Command.Parameters.AddWithValue("@StartName", StopName.Text);
                    _Command.Parameters.AddWithValue("@StartPhoneNo", StopPhoneNo.Text);

                    _Command.Parameters.AddWithValue("@idx", txtStopIdx.Text);
                    // _Command.Parameters.AddWithValue("@CustomerId", _Current.CustomerId);

                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV4Binding(_Current.CustomerId);
                    //tabContainer_SelectedIndexChanged(null, null);
                }

            }
            else
            {
                //신규
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "INSERT INTO customerstartmanage (StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,SGubun) VALUES (@StartState, @StartCity, @StartStreet, @StartDetail, @StartName, @StartPhoneNo, @ClientId, @CustomerId, getdate(),'E')";
                    _Command.Parameters.AddWithValue("@StartState", StopState.Text);
                    _Command.Parameters.AddWithValue("@StartCity", StopCity.Text);
                    _Command.Parameters.AddWithValue("@StartStreet", StopStreet.Text);
                    _Command.Parameters.AddWithValue("@StartDetail", Stop.Text);
                    _Command.Parameters.AddWithValue("@StartName", StopName.Text);
                    _Command.Parameters.AddWithValue("@StartPhoneNo", StopPhoneNo.Text);

                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@CustomerId", _Current.CustomerId);

                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV4Binding(_Current.CustomerId);
                    // tabContainer_SelectedIndexChanged(null, null);
                }
            }
        }
        private void newDGV4Binding(int CustomerId)
        {
            customerStartManageDataSet.CustomerStopManage.Clear();

            //Load DriverPoints
            if (customersBindingSource.Current as DataRowView == null)
                return;
            var _Selected = (customersBindingSource.Current as DataRowView).Row as ClientDataSet.CustomersRow;
            if (_Selected == null)
                return;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText =

                      @"select * from customerstartmanage where CustomerId = @CustomerId AND SGubun = 'E'
                       AND (StartState like '%'+@SearchText+'%'  OR StartCity like '%'+@SearchText+'%' OR StartStreet like '%'+@SearchText+'%' OR StartDetail like '%'+@SearchText+'%' OR StartName like '%'+@SearchText+'%') 
                        order by CreateTime DESC";


                _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                _Command.Parameters.AddWithValue("@SearchText", txtStopSearch.Text);
                using (SqlDataReader _Reader = _Command.ExecuteReader())
                {
                    customerStartManageDataSet.CustomerStopManage.Load(_Reader);
                }
            }
        }
        private void btnStopDelete_Click(object sender, EventArgs e)
        {
            if (customerStopManageBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)customerStopManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStopManageRow;
            if (MessageBox.Show("정말 하차지정보를 삭제하겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "DELETE customerstartmanage WHERE  Idx = @Idx";
                    _Command.Parameters.AddWithValue("@Idx", Selected.idx);


                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV4Binding(Selected.CustomerId);
                    // tabContainer_SelectedIndexChanged(null, null);
                }
            }
        }

        private void newDGV4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            customerStopManageBindingSource_CurrentChanged(null, null);
        }

        private void newDGV4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //  var Selected = ((DataRowView)customerStartManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStartManageRow;

            if (e.ColumnIndex == dataGridViewTextBoxColumn25.Index)
            {
                e.Value = (newDGV4.Rows.Count - e.RowIndex).ToString();
            }
            if (e.ColumnIndex == dataGridViewTextBoxColumn26.Index)
            {
                var Selected = ((DataRowView)newDGV4.Rows[e.RowIndex].DataBoundItem).Row as CustomerStartManageDataSet.CustomerStopManageRow;
                e.Value = Selected.StartState + " " + Selected.StartCity + " " + Selected.StartStreet + " " + Selected.StartDetail;
            }
        }

        private void customerStopManageBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (customerStopManageBindingSource.Current == null)
            {
                txtStopIdx.Clear();
                StopState.Text = "";
                StopCity.Text = "";
                StopStreet.Text = "";
                Stop.Text = "";
                StopName.Text = "";
                StopPhoneNo.Text = "";
                btnStopUpdate.Text = "저 장";
                return;
            }

            var Selected = ((DataRowView)customerStopManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStopManageRow;
            if (Selected != null)
            {
                txtStopIdx.Text = Selected.idx.ToString();
                StopState.Text = Selected.StartState;
                StopCity.Text = Selected.StartCity;
                StopStreet.Text = Selected.StartStreet;
                Stop.Text = Selected.StartDetail;
                StopName.Text = Selected.StartName;
                StopPhoneNo.Text = Selected.StartPhoneNo;
                btnStopUpdate.Text = "수 정";

            }
            else
            {
                txtStopIdx.Clear();
                StopState.Text = "";
                StopCity.Text = "";
                StopStreet.Text = "";
                Stop.Text = "";
                StopName.Text = "";
                StopPhoneNo.Text = "";
                btnStopUpdate.Text = "저 장";
                return;
            }
        }

        private void btnStopFindZip_Click(object sender, EventArgs e)
        {
            FindZipNew f = new Admin.FindZipNew();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                if (f.rdoRoad.Checked)
                {
                   
                    var ss = f.Address.Split(' ');
                    StopState.Text = ss[0];
                    StopCity.Text = ss[1];
                    StopStreet.Text = ss[2];
                    Stop.Text = String.Join(" ", ss.Skip(3));

                    //StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                    Stop.Focus();
                }
                else if (f.rdoJibun.Checked)
                {
                    var ss = f.Jibun.Split(' ');
                    StopState.Text = ss[0];
                    StopCity.Text = ss[1];
                    StopStreet.Text = ss[2];
                    Stop.Text = String.Join(" ", ss.Skip(3));
                    //StartName.Text = _AddressStateParse(StartState.Text) + " " + StartCity.Text;
                    Stop.Focus();
                }
            }
        }

        private void btnStartSearch_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            newDGV1Binding(Selected.CustomerId);
        }

        private void btnStoptSearch_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            newDGV4Binding(Selected.CustomerId);
        }

        private void txtStartSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            newDGV1Binding(Selected.CustomerId);
        }

        private void txtStopSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            newDGV4Binding(Selected.CustomerId);
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

        private void FrmMN0209_CUSTOMER_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F4)
            //{
            //    MessageBox.Show("qq");
            //}
        }

        private void btnTeam_New_Click(object sender, EventArgs e)
        {
            txt_TeamName.Text = "";
            txt_PartMaster.Text = "";
            txt_TCreateDate.Text = DateTime.Now.ToString("d").Replace("-", "/");
            txtCustomerTeamId.Text = "";
            btnTeam_Save.Text = "저 장";
        }
        private void newDGV5Binding(int CustomerId)
        {
            
            customerUserDataSet.CustomerTeams.Clear();
            //Load DriverPoints
            if (customersBindingSource.Current as DataRowView == null)
                return;
            var _Selected = (customersBindingSource.Current as DataRowView).Row as ClientDataSet.CustomersRow;
            if (_Selected == null)
                return;

            //if (customersBindingSource.Current == null)
            //    return;

            //var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;



           



            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText =

                      @"select CustomerTeamId, TeamName, CustomerId, ClientId, PartMaster, CreateDate from CustomerTeams where CustomerId = @CustomerId and Clientid = @Clientid
                       
                        order by CreateDate DESC";


                _Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                _Command.Parameters.AddWithValue("@Clientid", LocalUser.Instance.LogInInformation.ClientId);
                using (SqlDataReader _Reader = _Command.ExecuteReader())
                {
                    customerUserDataSet.CustomerTeams.Load(_Reader);
                }
            }
        }
        private void btnTeam_Save_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;

            if (String.IsNullOrEmpty(txt_TeamName.Text))
            {
                MessageBox.Show("부서명를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddTeam.Focus();
                return;
            }

            var _Current = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;

            if (!String.IsNullOrWhiteSpace(txtCustomerTeamId.Text))
            {

                //수정
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "Update CustomerTeams SET TeamName = @TeamName" +
                        ", PartMaster = @PartMaster " +

                        " WHERE CustomerTeamId = @CustomerTeamId ";
                    _Command.Parameters.AddWithValue("@TeamName", txt_TeamName.Text);
                    _Command.Parameters.AddWithValue("@PartMaster", txt_PartMaster.Text);
                    _Command.Parameters.AddWithValue("@CustomerTeamId", txtCustomerTeamId.Text);
                    


                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV5Binding(_Current.CustomerId);

                }

            }
            else
            {
                //신규
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "INSERT INTO CustomerTeams (TeamName, CustomerId, ClientId, PartMaster, CreateDate) " +
                        " VALUES (@TeamName, @CustomerId, @ClientId, @PartMaster, getdate())";
                    _Command.Parameters.AddWithValue("@TeamName", txt_TeamName.Text);
                    _Command.Parameters.AddWithValue("@CustomerId", _Current.CustomerId);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    _Command.Parameters.AddWithValue("@PartMaster", txt_PartMaster.Text);
                   

                    _Command.ExecuteNonQuery();

                    _Connection.Close();
                    newDGV5Binding(_Current.CustomerId);
                    // tabContainer_SelectedIndexChanged(null, null);
                }
            }
        }

        private void customerTeamsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            

            if (customerTeamsBindingSource.Current == null)
            {
                txtCustomerTeamId.Text = "";
                txt_TeamName.Text = "";
                txt_PartMaster.Text = "";
                txt_CreateDate.Text = DateTime.Now.ToString("d").Replace("-", "/");
                btnPUpdate.Text = "저 장";
                return;
            }

            var Selected = ((DataRowView)customerTeamsBindingSource.Current).Row as CustomerUserDataSet.CustomerTeamsRow;
            if (Selected != null)
            {
                txtCustomerTeamId.Text = Selected.CustomerTeamId.ToString();
                txt_TeamName.Text = Selected.TeamName;
                txt_PartMaster.Text = Selected.PartMaster;
                txt_CreateDate.Text = Selected.CreateDate.ToString("d").Replace("-", "/");
                btnTeam_Save.Text = "수 정";

            }
            else
            {
                txtCustomerTeamId.Text = "";
                txt_TeamName.Text = "";
                txt_PartMaster.Text = "";
                txt_CreateDate.Text = DateTime.Now.ToString("d").Replace("-", "/");
                btnPUpdate.Text = "저 장";
                return;
            }
        }

        private void newDGV5_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //  var Selected = ((DataRowView)customerStartManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStartManageRow;

            if (e.ColumnIndex == dataGridViewTextBoxColumn27.Index)
            {
                e.Value = (newDGV5.Rows.Count - e.RowIndex).ToString();
            }
        }

        private void newDGV5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            customerTeamsBindingSource_CurrentChanged(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
           FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
          

            _FormStyle.WriteFormStyle(this, grid1);

            MessageBox.Show("저장하였습니다.");

        }
        private DataGridView Grid;
        private DataGridViewColumn[] DefaultColumns;
        private DataGridViewColumn SelecteColumn;
        private Dictionary<int, DataGridViewColumn> DicCols = new Dictionary<int, DataGridViewColumn>();
        private void FrmGridWidth(DataGridView iGrid, params DataGridViewColumn[] iDefaultColumns)
        {
            InitializeComponent();

            Grid = iGrid;
            DefaultColumns = iDefaultColumns;
           
            InitListBox();

            var cond = System.Windows.Markup.XmlLanguage.GetLanguage
                    (System.Globalization.CultureInfo.CurrentUICulture.Name);

     

        }
        private void InitListBox()
        {
        
            DicCols.Clear();
            DataGridViewColumn[] _Cols = new DataGridViewColumn[Grid.ColumnCount];
            Grid.Columns.CopyTo(_Cols, 0);
            foreach (DataGridViewColumn item in _Cols.OrderBy(c => c.DisplayIndex))
            {
               // DicCols.Add(item.HeaderText, item);
        

            }
        }
    }
    
}
