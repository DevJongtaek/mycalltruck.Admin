using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using System.IO;
using OfficeOpenXml;
using mycalltruck.Admin.Class.DataSet;
using System.Linq.Expressions;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301_CARGOACCEPT : Form
    {
        #region FLAG
        private bool AllowOrder = false;
        private bool HasPoint = false;
        private void InitializeFlag()
        {
            if (LocalUser.Instance.LogInInformation.IsAdmin)
                return;
            LocalUser.Instance.LogInInformation.LoadClient();
            AllowOrder = LocalUser.Instance.LogInInformation.Client.AllowOrder;
            HasPoint = LocalUser.Instance.LogInInformation.Client.HasPoint;
        }
        private bool AutoLoad = true;
        Timer mTimer = new Timer();
        #endregion

        private void InitializeControl()
        {
            dtp_Setdate.Value = DateTime.Now;
            dtp_Getdate.Value = DateTime.Now;
            dtpStart.Value = DateTime.Now.AddMonths(-2);
            dtpEnd.Value = DateTime.Now;
            cmb_Search.SelectedIndex = 0;
            NotificationRadius.SelectedValue = 5;
            cmb_CarSearch.SelectedIndex = 0;
            cmb_I_Gubun.SelectedIndex = 0;

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                fPISIDDataGridViewTextBoxColumn.Visible = false;
                FPISNameColumn.Visible = false;
                cmb_FPISSearch.Visible = false;
                txt_FPISSearch.Visible = false;
                if (!AllowOrder)
                {
                    btnOrderBeforeAdd.Enabled = false;
                }
            }
            //관리자
            else
            {
                btnOrderBeforeAdd.Enabled = false;
                btnOrderAfterAdd.Enabled = false;
                btn_ClearDriverId.Enabled = false;
                btn_DriverCancel.Enabled = false;
                btnCurrentDelete.Enabled = false;
            }

            cmb_LocationSearch.SelectedIndex = 0;
            cmb_FPISSearch.SelectedIndex = 0;

            tabControl1.SelectedIndex = 1;
            tabControl1.SelectedIndex = 0;

            mTimer.Interval = 10 * 1000;
            mTimer.Tick += MTimer_Tick;
        }

        private void MTimer_Tick(object sender, EventArgs e)
        {
            DataLoad();
        }

        private BaseDataSet.DriversDataTable TTable = new BaseDataSet.DriversDataTable();

        #region ACTION

        public FrmMN0301_CARGOACCEPT()
        {
            InitializeComponent();
        }

        private void FrmMN0301_CARGOACCEPT_Load(object sender, EventArgs e)
        {
            InitializeFlag();
            InitializeControl();
            if(LocalUser.Instance.LogInInformation.IsAdmin)
            {
                InitClientTable();
            }
            InitCustomerTable();
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = driversBindingSource;
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(baseDataSet.Drivers);
            InitializeDataSource();
            ChangeInfo();
            DataLoad();
            newDGV1.CurrentCell = null;
            if(!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                ClientPrice.Enabled = false;
            }
        }

        // 상단 버튼
        private void btnOrderBeforeAdd_Click(object sender, EventArgs e)
        {
            FrmMN0301_CARGOACCEPT_Add _Form = new FrmMN0301_CARGOACCEPT_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();

            DataLoad();
        }

        private void btnOrderAfterAdd_Click(object sender, EventArgs e)
        {
            FrmMN0303_CARGOFPIS_Add3 _Form = new FrmMN0303_CARGOFPIS_Add3();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(baseDataSet.Drivers);
            DataLoad();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // 조회 관련
        private void cmb_FPISSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_FPISSearch.Clear();
            if (cmb_FPISSearch.SelectedIndex == 0)
            {
                txt_FPISSearch.ReadOnly = true;
            }
            else
            {
                txt_FPISSearch.ReadOnly = false;
            }
        }

        private void btn_AfterDate_Click(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.AddDays(1);
            dtpEnd.Value = DateTime.Now.AddYears(2);
            cmb_Search.SelectedIndex = 1;
            DataLoad();
        }

        private void cmb_LocactionSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_LocationSearch.Clear();
            if (cmb_LocationSearch.SelectedIndex == 0)
            {
                txt_LocationSearch.ReadOnly = true;
            }
            else
            {
                txt_LocationSearch.ReadOnly = false;
            }
        }

        private void onNeedDataLoad(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_FPISSearch.SelectedIndex = 0;
            txt_FPISSearch.Clear();
            cmb_LocationSearch.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            cmb_CarTypeS.SelectedIndex = 0;
            cmb_StatusS.SelectedIndex = 0;
            dtpStart.Value = DateTime.Now.AddMonths(-2);
            dtpEnd.Value = DateTime.Now;

            cmb_I_Gubun.SelectedIndex = 0;
            if (cmb_SubClientId.Items.Count > 0)
                cmb_SubClientId.SelectedIndex = 0;
            DataLoad();
        }

        // 상세 내역
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmb_StartState_Click(object sender, EventArgs e)
        {
            if (cmb_StartState.SelectedValue == null)
                return;
            cmb_StartCity.DataSource = null;
        }

        private void cmb_EndState_Click(object sender, EventArgs e)
        {
            if (cmb_EndState.SelectedValue == null)
                return;
            cmb_EndCity.DataSource = null;
        }

        private void cmb_StartCity_DropDown(object sender, EventArgs e)
        {
            var StartCityDataSource = AddressList.Where(c => c.State == cmb_StartState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            cmb_StartCity.DataSource = StartCityDataSource;
            cmb_StartCity.ValueMember = "City";
            cmb_StartCity.DisplayMember = "City";
        }

        private void cmb_EndCity_DropDown(object sender, EventArgs e)
        {
            var EndCityDataSource = AddressList.Where(c => c.State == cmb_EndState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            cmb_EndCity.DataSource = EndCityDataSource;
            cmb_EndCity.ValueMember = "City";
            cmb_EndCity.DisplayMember = "City";
        }

        private void onNeedChangeInfo(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void Price_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
        }

        private void Price_Leave(object sender, EventArgs e)
        {
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Selected == null)
                return;
            decimal _Fee = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT ISNULL(Fee, 8) FROM Customers WHERE CustomerId = {Selected.CustomerId}";
                    var o = _Command.ExecuteScalar();
                    if(o != null)
                    {
                        _Fee = Convert.ToInt32(o);
                    }
                }
                _Connection.Close();
            }
            _Fee += 100;
            _Fee *= 0.01m;
            decimal _d = 0;
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
            if (decimal.TryParse(((TextBox)sender).Text, out _d))
            {
                ((TextBox)sender).Text = _d.ToString("N0");
                if (sender == Price)
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
                    {
                        ClientPrice.Text = Math.Floor(_d * _Fee).ToString("N0");
                    }
                }
            }
        }

        private void Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        // 배차 내역
        private void btn_CarSearch_Click(object sender, EventArgs e)
        {
            //상호
            //기사명
            //차량번호
            if(cmb_CarSearch.SelectedIndex > 0 && !String.IsNullOrWhiteSpace(txt_CarSearch.Text))
            {
                switch (cmb_CarSearch.Text)
                {
                    case "상호":
                        driversBindingSource.Filter = "Name LIKE '%"+txt_CarSearch.Text+"%'";
                        break;
                    case "기사명":
                        driversBindingSource.Filter = "CarYear LIKE '%" + txt_CarSearch.Text + "%'";
                        break;
                    case "차량번호":
                        driversBindingSource.Filter = "CarNo LIKE '%" + txt_CarSearch.Text + "%'";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                driversBindingSource.Filter = "";
            }
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Selected == null)
                return;
            if(!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.HasPoint && Selected.PointMethod == 2)
            {
                var Fee = Selected.ClientPrice - Selected.Price;
                if (driversBindingSource.Filter != "")
                {
                    driversBindingSource.Filter += "AND POINT > " + Fee;
                }
                else
                {
                    driversBindingSource.Filter = "POINT > "+Fee;
                }
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            cmb_CarSearch.SelectedIndex = 0;
            txt_CarSearch.Text = string.Empty;
            driversBindingSource.Filter = "";
        }

        private void btn_ClearDriverId_Click(object sender, EventArgs e)
        {
            ClearDriver();
        }

        private void btn_DriverCancel_Click(object sender, EventArgs e)
        {
            RemoveDriver();
        }

        private void newDGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Selected == null)
                return;
            if (Selected.OrderId == 0)
                return;
            if (driversBindingSource.Current == null)
                return;
            var _Model = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (_Model == null)
                return;
            DriverName.Text = _Model.CarYear;
            DriverBizNo.Text = _Model.BizNo;
            DriverCarType.SelectedValue = _Model.CarType;
            DriverCarNo.Text = _Model.CarNo;
            DriverMobileNo.Text = _Model.MobileNo;

            DriverName.ReadOnly = true;
            DriverBizNo.ReadOnly = true;
            DriverCarType.Enabled = false;
            DriverCarNo.ReadOnly = true;
            DriverMobileNo.ReadOnly = true;
        }

        // 리스트

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.RowCount)
                return;
            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as OrderDataSet.OrdersRow;
            if (Selected == null)
                return;
            // 관리자 로그인시 운송사코드/운송사명 출력
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                if (dataGridView1.Columns[e.ColumnIndex] == fPISIDDataGridViewTextBoxColumn)
                {
                    var Query = _ClientTable.Where(c => c.ClientId == Selected.ClientId);
                    if (Query.Any())
                    {
                        e.Value = Query.First().Code;
                    }
                    return;
                }
                //운송사명
                if (dataGridView1.Columns[e.ColumnIndex] == FPISNameColumn)
                {
                    //  var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.OrdersRow;
                    var Query = _ClientTable.Where(c => c.ClientId == Selected.ClientId);
                    if (Query.Any())
                    {
                        e.Value = Query.First().Name;
                    }
                    return;
                }
            }
            //순번
            if (e.ColumnIndex == 0)
            {
                e.Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            //차량구분
            else if (dataGridView1.Columns[e.ColumnIndex] == Column7)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                }
                else
                {
                    var _DriverId = (int)e.Value;
                    if (_DriverId > 0)
                    {
                        var _DriverViewModel = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _DriverId);
                        if(_DriverViewModel == null)
                            _DriverViewModel = TTable.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel != null)
                        {
                            var _Option = SingleDataSet.Instance.StaticOptions.FirstOrDefault(c => c.Div == "CarGubun" && c.Value == _DriverViewModel.CarGubun);
                            if (_Option != null)
                            {
                                e.Value = _Option.Name;
                            }
                            else
                            {
                                e.Value = "";
                            }
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                }
            }
            // 차량번호
            else if (dataGridView1.Columns[e.ColumnIndex] == Column_DriverCarNo)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                }
                else
                {
                    var _DriverId = (int)e.Value;
                    if (_DriverId > 0)
                    {
                        var _DriverViewModel = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel == null)
                            _DriverViewModel = TTable.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel != null)
                        {
                            e.Value = _DriverViewModel.CarNo;
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                }
            }
            // 차주아이디
            else if (dataGridView1.Columns[e.ColumnIndex] == Column_DriverLoginId)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                }
                else
                {
                    var _DriverId = (int)e.Value;
                    if (_DriverId > 0)
                    {
                        var _DriverViewModel = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel == null)
                            _DriverViewModel = TTable.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel != null)
                        {
                            e.Value = _DriverViewModel.LoginId;
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                }
            }
            // 기사명
            else if (dataGridView1.Columns[e.ColumnIndex] == Column_DriverName)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                }
                else
                {
                    var _DriverId = (int)e.Value;
                    if (_DriverId > 0)
                    {
                        var _DriverViewModel = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel == null)
                            _DriverViewModel = TTable.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel != null)
                        {
                            e.Value = _DriverViewModel.CarYear;
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                }
            }
            // 연락처
            else if (dataGridView1.Columns[e.ColumnIndex] == Column_DriverPhoneNo)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                }
                else
                {
                    var _DriverId = (int)e.Value;
                    if (_DriverId > 0)
                    {
                        var _DriverViewModel = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel == null)
                            _DriverViewModel = TTable.FirstOrDefault(c => c.DriverId == _DriverId);
                        if (_DriverViewModel != null)
                        {
                            e.Value = _DriverViewModel.MobileNo;
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                }
            }
            // 등록구분
            else if (dataGridView1.Columns[e.ColumnIndex] == Column10)
            {
                if ((int)e.Value == 1)
                {
                    e.Value = "어플";
                }
                else if ((int)e.Value == 2)
                {
                    e.Value = "PC(1건)";
                }
                else if ((int)e.Value == 3)
                {
                    e.Value = "PC(일괄)";
                }
                else
                {
                    e.Value = "";
                }
            }
            // 화물의뢰자
            else if (e.ColumnIndex == ColumnCustomerId.Index)
            {
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";
                    return;
                }
                var _CustomerViewModel = _CustomerViewModelList.FirstOrDefault(c => c.CustomerId == (int)e.Value);
                if (_CustomerViewModel == null)
                    e.Value = "";
                else
                    e.Value = _CustomerViewModel.Name;
            }
            // 출발지
            else if (dataGridView1.Columns[e.ColumnIndex] == startDataGridViewTextBoxColumn)
            {
                e.Value = String.Join("/", new string[] { Selected.StartState, Selected.StartCity }.Where(c => !String.IsNullOrWhiteSpace(c)));
            }
            // 도착지
            else if (dataGridView1.Columns[e.ColumnIndex] == stopDataGridViewTextBoxColumn)
            {
                e.Value = String.Join("/", new string[] { Selected.StopState, Selected.StopCity }.Where(c => !String.IsNullOrWhiteSpace(c)));
            }
            // 화주발행
            else if (e.ColumnIndex == SalesManageId.Index)
            {
                String SIssueState = string.Empty;
                bool _HasSales = false;
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";

                }
                else if ((int)e.Value > 0)
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        var Command1 = cn.CreateCommand();
                        Command1.CommandText = "SELECT ISNULL(IssueState,N'')  FROM SalesManage where SalesId = '" + Selected.SalesManageId + "'";
                        var o1 = Command1.ExecuteScalar();
                        if (o1 != null)
                        {
                            _HasSales = true;
                            SIssueState = o1.ToString();
                        }
                        cn.Close();

                    }
                    if(SIssueState == "발행" || SIssueState =="재발행")
                    {

                        e.Value = "발행";
                    }
                    else if(_HasSales)
                    {
                        e.Value = "미전송";

                    }
                }
                else
                {
                    e.Value = "";
                }
            }
            else if(e.ColumnIndex == SaleManageDate.Index)
            {
                String SIssueState = string.Empty;
                int _PayState = 0;
                DateTime _PayDate = DateTime.Now;
                if (e.Value == DBNull.Value)
                {
                    e.Value = "";

                }
                else if ((int)e.Value > 0)
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        var Command1 = cn.CreateCommand();
                        Command1.CommandText = "SELECT IssueState, PayState, PayDate FROM SalesManage where SalesId = '" + Selected.SalesManageId + "'";
                        using (SqlDataReader _Reader = Command1.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                if (!_Reader.IsDBNull(0))
                                {
                                    SIssueState = _Reader.GetString(0);
                                    _PayState = _Reader.GetInt32(1);
                                }
                                if(!_Reader.IsDBNull(2))
                                    _PayDate = _Reader.GetDateTime(2);
                            }
                        }
                        cn.Close();
                    }
                    if ((SIssueState == "발행" || SIssueState == "재발행") && (_PayState == 1))
                    {
                        e.Value = _PayDate.ToString("yyy-MM-dd");
                    }
                    else
                    {
                        e.Value = "";
                    }
                }
                else
                {
                    e.Value = "";
                }
            }
            // 차주청구
            else if (e.ColumnIndex == TradeId.Index)
            {
                if (e.Value == DBNull.Value)
                    e.Value = "";
                else if ((int)e.Value > 0)
                    e.Value = "청구";
                else
                    e.Value = "";
            }
            // 배송상태
            else if (dataGridView1.Columns[e.ColumnIndex] == orderStatusDataGridViewTextBoxColumn)
            {
                var _T = Filter.Order.StatusList.FirstOrDefault(c => c.Value == (int)e.Value);
                if (_T != null)
                    e.Value = _T.Text;
                if (e.Value.ToString() != "배차완료")
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
                else
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Gray;
                }
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.Font = new System.Drawing.Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
            }
            // 차종
            else if (dataGridView1.Columns[e.ColumnIndex] == carTypeDataGridViewTextBoxColumn)
            {
                try
                {
                    e.Value = Filter.Order.CarTypeList.First(c => c.Value == (int)e.Value).Text;
                }
                catch (Exception)
                {
                    e.Value = string.Empty;
                }
            }
            // 톤수
            else if (dataGridView1.Columns[e.ColumnIndex] == carSizeDataGridViewTextBoxColumn)
            {
                try
                {
                    e.Value = Filter.Order.CarSizeList.First(c => c.Value == (int)e.Value).Text;
                }
                catch (Exception)
                {
                    e.Value = string.Empty;
                }
            }
            // 알림조건
            else if (dataGridView1.Columns[e.ColumnIndex] == notificationFilterTypeDataGridViewTextBoxColumn)
            {
                if (Selected.SourceType == 0)
                {
                    e.Value = "";
                }
                else
                {
                    if (Selected.NotificationFilterType == 1)
                    {
                        var ClientAddr = String.Join("/", new string[] { Selected.StartState, Selected.StartCity }.Where(c => !String.IsNullOrWhiteSpace(c)));
                        e.Value = ClientAddr + " " + Selected.NotificationRadius.ToString("N0") + "Km";
                    }
                    else if (Selected.NotificationFilterType == 3)
                    {
                        string GroupName = string.Empty;
                        if (Selected.NotificationGroupName == "0")
                        {
                            GroupName = "그룹 전체 ";
                        }
                        else
                        {
                            GroupName = Selected.NotificationGroupName + " 그룹";
                        }
                        e.Value = GroupName;
                    }
                    else if (Selected.NotificationFilterType == 4)
                    {
                        var ClientAddr = String.Join("/", new string[] { Selected.StartState, Selected.StartCity }.Where(c => !String.IsNullOrWhiteSpace(c)));
                        string GroupName = string.Empty;
                        if (Selected.NotificationGroupName == "0")
                        {
                            GroupName = "그룹 전체 ";
                        }
                        else
                        {
                            GroupName = Selected.NotificationGroupName + " 그룹";
                        }
                        e.Value = GroupName + " + " + ClientAddr + " " + Selected.NotificationRadius.ToString("N0") + "Km";
                    }
                }
            }
            // 본지점구분
            else if (e.ColumnIndex == SubClientId.Index)
            {
                if (SubClientIdDictionary.ContainsKey(Selected.SubClientId))
                    e.Value = SubClientIdDictionary[Selected.SubClientId];
            }
        }

        private void ordersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Selected != null)
            {
                // Initialize Controls State
                btnUpdate.Enabled = true;
                btnCurrentDelete.Enabled = true;

                dtp_Setdate.Enabled = true;
                dtp_Getdate.Enabled = true;
                cmb_StartState.Enabled = true;
                cmb_StartCity.Enabled = true;
                cmb_EndState.Enabled = true;
                cmb_EndCity.Enabled = true;
                Item.ReadOnly = false;
                ItemSize.ReadOnly = false;
                CarCount.ReadOnly = false;
                Price.ReadOnly = false;
                ClientPrice.ReadOnly = false;
                Etc.ReadOnly = false;

                btn_SetDriver.Enabled = true;
                btn_DriverCancel.Enabled = true;
                btn_ClearDriverId.Enabled = true;
                cmb_CarSearch.Enabled = true;
                txt_CarSearch.Enabled = true;
                btn_CarSearch.Enabled = true;
                btn_Clear.Enabled = true;
                newDGV1.Enabled = true;

                CarType.Enabled = true;
                CarSize.Enabled = true;
                PayLocation.Enabled = true;
                IsShared.Enabled = true;
                NotificationFilterType.Enabled = true;
                NotificationRadius.Enabled = true;
                NotificationGroupName.Enabled = true;

                if(LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    btn_ClearDriverId.Enabled = false;
                    btn_DriverCancel.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                }

                // Set Controls Value
                cmb_StartCity.DataSource = new String[] { Selected.StartCity }.Select(c => new { City = c }).ToList();
                cmb_EndCity.DataSource = new String[] { Selected.StopCity }.Select(c => new { City = c }).ToList();

                cmb_StartState.SelectedValue = Selected.StartState;
                cmb_EndState.SelectedValue = Selected.StopState;
                cmb_StartCity.SelectedValue = Selected.StartCity;
                cmb_EndCity.SelectedValue = Selected.StopCity;

                txt_OrderStatus.Text = Filter.Order.StatusList.FirstOrDefault(c => c.Value == Selected.OrderStatus).Text;

                Price.Text = Selected.Price.ToString("N0");
                ClientPrice.Text = Selected.ClientPrice.ToString("N0");
                CarCount.Text = Selected.CarCount.ToString("N0");
                if (_CustomerViewModelList.Any(c => c.CustomerId == Selected.CustomerId))
                {
                    CustomerName.Text = _CustomerViewModelList.FirstOrDefault(c => c.CustomerId == Selected.CustomerId).Name;
                    CustomerPhoneNo.Text = _CustomerViewModelList.FirstOrDefault(c => c.CustomerId == Selected.CustomerId).PhoneNo;
                }
                else
                {
                    CustomerName.Clear();
                    CustomerPhoneNo.Clear();
                }
                // 배차내역
                if (Selected.DriverId > 0)
                {
                    var _Driver = baseDataSet.Drivers.FindByDriverId(Selected.DriverId);
                    if(_Driver == null)
                        _Driver = TTable.FindByDriverId(Selected.DriverId);
                    if (_Driver != null)
                    {
                        DriverName.Text = _Driver.CarYear;
                        DriverBizNo.Text = _Driver.BizNo;
                        DriverCarType.SelectedValue = _Driver.CarType;
                        DriverCarNo.Text = _Driver.CarNo;
                        DriverMobileNo.Text = _Driver.MobileNo;
                    }
                    else
                    {
                        DriverName.Clear();
                        DriverBizNo.Clear();
                        DriverCarType.SelectedIndex = 0;
                        DriverCarNo.Clear();
                        DriverMobileNo.Clear();
                    }

                    DriverName.ReadOnly = true;
                    DriverBizNo.ReadOnly = true;
                    DriverCarType.Enabled = false;
                    DriverCarNo.ReadOnly = true;
                    DriverMobileNo.ReadOnly = true;

                    btn_SetDriver.Enabled = false;
                    btn_DriverCancel.Enabled = true;
                    btn_ClearDriverId.Enabled = false;
                    cmb_CarSearch.Enabled = false;
                    txt_CarSearch.Enabled = false;
                    btn_CarSearch.Enabled = false;
                    btn_Clear.Enabled = false;
                    newDGV1.Enabled = false;

                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                    newDGV1.AutoGenerateColumns = false;
                    newDGV1.DataSource = null;
                }
                else
                {
                    DriverName.Clear();
                    DriverBizNo.Clear();
                    DriverCarType.SelectedIndex = 0;
                    DriverCarNo.Clear();
                    DriverMobileNo.Clear();

                    DriverName.ReadOnly = false;
                    DriverBizNo.ReadOnly = false;
                    DriverCarType.Enabled = true;
                    DriverCarNo.ReadOnly = false;
                    DriverMobileNo.ReadOnly = false;

                    btn_SetDriver.Enabled = true;
                    btn_DriverCancel.Enabled = false;
                    btn_ClearDriverId.Enabled = true;
                    cmb_CarSearch.Enabled = true;
                    txt_CarSearch.Enabled = true;
                    btn_CarSearch.Enabled = true;
                    btn_Clear.Enabled = true;
                    newDGV1.Enabled = true;
                    cmb_CarSearch.SelectedIndex = 0;
                    txt_CarSearch.Clear();
                    btn_CarSearch_Click(null, null);
                    btn_ClearDriverId.Visible = true;
                    if (HasPoint && Selected.PointMethod == 2)
                    {
                        btn_ClearDriverId.Visible = false;
                    }
                    newDGV1.AutoGenerateColumns = false;
                    newDGV1.DataSource = driversBindingSource;
                    newDGV1.CurrentCell = null;
                }

                // Set Notification Info
                if (Selected.SourceType == 0 || Selected.DriverId > 0)
                {
                    CarType.Enabled = false;
                    CarSize.Enabled = false;
                    PayLocation.Enabled = false;
                    IsShared.Enabled = false;
                    NotificationFilterType.Enabled = false;
                    NotificationRadius.Enabled = false;
                    NotificationGroupName.Enabled = false;
                }
                else
                {
                    CarType.Enabled = true;
                    CarSize.Enabled = true;
                    PayLocation.Enabled = true;
                    IsShared.Enabled = true;
                    NotificationFilterType.Enabled = true;
                    NotificationRadius.Enabled = true;
                    NotificationGroupName.Enabled = true;
                }
                ChangeInfo();

                // Set Controls State
                if (Selected.TradeId > 0 || Selected.SalesManageId > 0)
                {
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;

                    dtp_Setdate.Enabled = false;
                    dtp_Getdate.Enabled = false;
                    cmb_StartState.Enabled = false;
                    cmb_StartCity.Enabled = false;
                    cmb_EndState.Enabled = false;
                    cmb_EndCity.Enabled = false;
                    Item.ReadOnly = true;
                    ItemSize.ReadOnly = true;
                    CarCount.ReadOnly = true;
                    Price.ReadOnly = true;
                    ClientPrice.ReadOnly = true;
                    Etc.ReadOnly = true;

                    btn_SetDriver.Enabled = false;
                    btn_DriverCancel.Enabled = false;
                    btn_ClearDriverId.Enabled = false;
                    cmb_CarSearch.Enabled = false;
                    txt_CarSearch.Enabled = false;
                    btn_CarSearch.Enabled = false;
                    btn_Clear.Enabled = false;
                    newDGV1.Enabled = false;

                    CarType.Enabled = false;
                    CarSize.Enabled = false;
                    PayLocation.Enabled = false;
                    IsShared.Enabled = false;
                    NotificationFilterType.Enabled = false;
                    NotificationRadius.Enabled = false;
                    NotificationGroupName.Enabled = false;
                }
            }
        }

        private void onNeedDataLoad(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            DataLoad();
        }

        private void btn_SetDriver_Click(object sender, EventArgs e)
        {
            UpdateData(() => { SetDriver(); });
        }

        private void DriverBizNo_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace("-", "");
        }

        private void DriverBizNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(DriverBizNo.Text))
            {
                var _S = DriverBizNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 6)
                {
                    _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                }
                DriverBizNo.Text = _S;
            }

        }

        private void DriverMobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(DriverMobileNo.Text))
            {
                var _S = DriverMobileNo.Text.Replace("-", "").Replace(" ", "");
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
                DriverMobileNo.Text = _S;
            }

        }

        private void FrmMN0301_CARGOACCEPT_Shown(object sender, EventArgs e)
        {
            newDGV1.ClearSelection();
        }

        #endregion

        #region UPDATE

        private void UpdateData(Action Done = null)
        {
            if (ordersBindingSource == null || ordersBindingSource.Current == null)
                return;
            Int64 iPrice = 0;
            Int32 iCarCount = 0;
            Int64 iClientPrice = 0;
            // Data Test
            if (dtp_Getdate.Value < dtp_Setdate.Value)
            {
                MessageBox.Show("도착지 날짜는 반드시 출발지 이후로 설정하셔야 합니다.");
                return;
            }

            if (String.IsNullOrEmpty(Item.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            var Row = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Row.TradeId > 0 || Row.SalesManageId > 0)
                return;
            if (Row.SourceType == 1)
            {
                if (CarType.SelectedIndex == 0)
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(CarType, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }
                if (CarSize.SelectedIndex == 0)
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(CarSize, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }
            }

            if (String.IsNullOrEmpty(Price.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;

            }
            else if (!Int64.TryParse(Price.Text.Replace(",", ""), out iPrice))
            {
                MessageBox.Show("차주운송료를 숫자로 입력해 주십시오.");
                return;
            }

            if (String.IsNullOrEmpty(CarCount.Text) &&  !Int32.TryParse(CarCount.Text, out iCarCount))
            {
                MessageBox.Show("배차횟수를  숫자로 입력해 주십시오.");
                return;
            }
            else
            {
                iCarCount = Int32.Parse(CarCount.Text.Replace(",", ""));

            }

            if (String.IsNullOrEmpty(ClientPrice.Text) && !Int64.TryParse(ClientPrice.Text, out iClientPrice))
            {
                MessageBox.Show("화주운송료를  숫자로 입력해 주십시오.");
                return;
            }
            else
            {
                iClientPrice = Int64.Parse(ClientPrice.Text.Replace(",",""));

            }

            // Update
            ordersBindingSource.EndEdit();
            Row.StartTime = dtp_Setdate.Value;
            Row.StopTime = dtp_Getdate.Value;
            Row.Price = iPrice;
            Row.ClientPrice = iClientPrice;
            Row.CarCount = iCarCount;
            //출발지 도착지
            Row.StartState = cmb_StartState.Text;
            Row.StartCity = cmb_StartCity.Text;
            Row.StopState = cmb_EndState.Text;
            Row.StopCity = cmb_EndCity.Text;
            // 배차 전 화물등록(배차중계) 이며 배차 전 상태 일때만, 좌표 등록한다.
            if (Row.SourceType == 1 && Row.OrderStatus == 1)
            {
                //좌표
                var Address = SingleDataSet.Instance.AddressReferences.First(c => c.State == Row.StartState && c.City == Row.StartCity);
                var rPoint = mLocationHelper.GetLocactionFromAddress(Address);
                Row.X = rPoint.X;
                Row.Y = rPoint.Y;
            }
            else
            {
                Row.X = 0;
                Row.Y = 0;
            }

            if (String.IsNullOrEmpty(ItemSize.Text))
            {
                Row.ItemSize = "0";
            }
            else
            {
                Row.ItemSize = ItemSize.Text;
            }

            // 외래키 제약이 걸려 있어서, 0으로 입력하면 데이터베이스에서 에러가 발생한다.
            //ClientId
            if (Row.ClientId == 0)
                Row.SetClientIdNull();
            //CustomerId
            if (Row.CustomerId == 0)
                Row.SetCustomerIdNull();
            //TradeId
            if (Row.TradeId == 0)
                Row.SetTradeIdNull();
            //SalesManageId
            if (Row.SalesManageId == 0)
                Row.SetSalesManageIdNull();
            //DriverId
            if (Row.DriverId == 0)
                Row.SetDriverIdNull();

            ordersTableAdapter.Update(Row);

            // Notification
            // 배차 전 화물등록(배차중계) 이며 배차전 상태일 때만, 알림 등록한다.
            if (Row.SourceType == 0 && Row.OrderStatus == 1)
            {
                List<int> DriverIdList = new List<int>();
                if (Row.OrderStatus == 1)
                {
                    //행정구역 +- 10Km
                    if (Row.NotificationFilterType == 1 || Row.NotificationFilterType == 4)
                    {
                        var x1 = Row.X;
                        var y1 = Row.Y;
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            var Query =
                                   @"SELECT PreOrders.DriverId, X, Y
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            WHERE PreOrders.StopTime > GetDate() AND PreOrders.IsPreview = 0
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            ORDER BY ABS(@x1 - PreOrders.X) + ABS(@y1 - PreOrders.Y)";
                            var Commad = cn.CreateCommand();
                            Commad.CommandText = Query;
                            Commad.Parameters.AddWithValue("@x1", x1);
                            Commad.Parameters.AddWithValue("@y1", y1);
                            Commad.Parameters.AddWithValue("@CarType", Row.CarType);
                            Commad.Parameters.AddWithValue("@CarSize", Row.CarSize);
                            var Reader = Commad.ExecuteReader();
                            while (Reader.Read())
                            {
                                var X = Reader.GetDouble(1);
                                var Y = Reader.GetDouble(2);
                                var DriverId = Reader.GetInt32(0);
                                var Distance = mLocationHelper.DistanceFromGPS(X, Y, x1, y1);
                                if (Distance > Row.NotificationRadius)
                                    break;
                                DriverIdList.Add(DriverId);
                            }
                            cn.Close();
                        }
                    }
                    //그룹
                    if (Row.NotificationFilterType == 3 || Row.NotificationFilterType == 4)
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();

                            var Query =
                                @"SELECT PreOrders.DriverId
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            JOIN DriverGroups ON Drivers.DriverId = DriverGroups.DriverId
                            WHERE StopTime > GetDate() AND IsPreview = 0
                            AND DriverGroups.ClientId = @ClientId
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            AND (@iNotificationGroupName = '0' OR DriverGroups.NAME = @iNotificationGroupName)";
                            var Commad = cn.CreateCommand();
                            Commad.CommandText = Query;
                            Commad.Parameters.AddWithValue("@ClientId", Row.ClientId);
                            Commad.Parameters.AddWithValue("@iNotificationGroupName", Row.NotificationGroupName);
                            Commad.Parameters.AddWithValue("@CarType", Row.CarType);
                            Commad.Parameters.AddWithValue("@CarSize", Row.CarSize);
                            var Reader = Commad.ExecuteReader();
                            while (Reader.Read())
                            {
                                var DriverId = Reader.GetInt32(0);
                                DriverIdList.Add(DriverId);
                            }
                            cn.Close();
                        }
                    }
                }
                if (DriverIdList.Count > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlBulkCopy _Bulk = new SqlBulkCopy(_Connection))
                        {
                            var _Table = new OrderDataSet.NotificationsDataTable();
                            foreach (var driverId in DriverIdList.Distinct())
                            {
                                var NRow = _Table.NewNotificationsRow();
                                NRow.FilterType = Row.NotificationFilterType;
                                NRow.State = Row.StartState;
                                NRow.City = Row.StartCity;
                                NRow.Radius = Row.NotificationRadius;
                                NRow.GroupName = Row.NotificationGroupName;
                                NRow.IsChecked = false;
                                NRow.CreateTime = DateTime.Now;
                                NRow.DriverId = driverId;
                                NRow.ClientId = Row.ClientId;
                                NRow.OrderId = Row.OrderId;
                                _Table.AddNotificationsRow(NRow);
                            }
                            _Bulk.WriteToServer(_Table);
                        }
                        _Connection.Close();
                    }
                }
            }

            if (Row.SourceType == 1)
            {
                //Notification 추가
                #region Notification 추가
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    var iNotificationFilterType = Row.NotificationFilterType;
                    var iNotificationRadius = Row.NotificationRadius;
                    var iOrderStatus = Row.OrderStatus;
                    var DistanceLimite = iNotificationRadius;
                    var iNotificationGroupName = Row.NotificationGroupName;
                    var iStartState = Row.StopState;
                    var iStartCity = Row.StartCity;
                    var iCarType = Row.CarType;
                    var iCarSize = Row.CarSize;
                    var iStopState = Row.StopState;
                    var iStopCity = Row.StopCity;

                    if (iOrderStatus == 1)
                    {
                        List<int> DriverIdList = new List<int>();
                        //행정구역 +- 10Km
                        if (iNotificationFilterType == 1 || iNotificationFilterType == 4)
                        {
                            var x1 = Row.X;
                            var y1 = Row.Y;
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                var Query =
                                    @"SELECT PreOrders.DriverId, X, Y
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            WHERE PreOrders.StopTime > GetDate() AND PreOrders.IsPreview = 0
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            ORDER BY ABS(@x1 - PreOrders.X) + ABS(@y1 - PreOrders.Y)";
                                var Commad = cn.CreateCommand();
                                Commad.CommandText = Query;
                                Commad.Parameters.AddWithValue("@x1", x1);
                                Commad.Parameters.AddWithValue("@y1", y1);

                                Commad.Parameters.AddWithValue("@CarType", iCarType);
                                Commad.Parameters.AddWithValue("@CarSize", iCarSize);

                                var Reader = Commad.ExecuteReader();
                                while (Reader.Read())
                                {
                                    var X = Reader.GetDouble(1);
                                    var Y = Reader.GetDouble(2);
                                    var DriverId = Reader.GetInt32(0);

                                    var Distance = mLocationHelper.DistanceFromGPS(X, Y, x1, y1);
                                    if (Distance > DistanceLimite)
                                        break;
                                    DriverIdList.Add(DriverId);
                                }
                                cn.Close();
                            }
                        }
                        //그룹
                        if (iNotificationFilterType == 3 || iNotificationFilterType == 4)
                        {
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();

                                var Query =
                                    @"SELECT PreOrders.DriverId
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            JOIN DriverGroups ON Drivers.DriverId = DriverGroups.DriverId
                            WHERE StopTime > GetDate() AND IsPreview = 0
                            AND DriverGroups.ClientId = @ClientId
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            AND (@iNotificationGroupName = '0' OR DriverGroups.NAME = @iNotificationGroupName)";
                                var Commad = cn.CreateCommand();
                                Commad.CommandText = Query;
                                Commad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                Commad.Parameters.AddWithValue("@iNotificationGroupName", iNotificationGroupName);

                                Commad.Parameters.AddWithValue("@CarType", iCarType);
                                Commad.Parameters.AddWithValue("@CarSize", iCarSize);


                                var Reader = Commad.ExecuteReader();
                                while (Reader.Read())
                                {
                                    var DriverId = Reader.GetInt32(0);
                                    DriverIdList.Add(DriverId);
                                }
                                cn.Close();
                            }
                        }
                        if (DriverIdList.Count > 0)
                        {
                            var Table = new CMDataSet.NotificationsDataTable();
                            foreach (var driverId in DriverIdList.Distinct())
                            {
                                var Noti = Table.NewNotificationsRow();
                                Noti.FilterType = iNotificationFilterType;
                                Noti.State = iStartState;
                                Noti.City = iStartCity;
                                Noti.Street = "";
                                Noti.Radius = iNotificationRadius;
                                Noti.GroupName = iNotificationGroupName;
                                Noti.IsChecked = false;
                                Noti.CreateTime = DateTime.Now;
                                Noti.DriverId = driverId;
                                Noti.ClientId = LocalUser.Instance.LogInInformation.ClientId;
                                Noti.OrderId = Row.OrderId;
                                Table.AddNotificationsRow(Noti);
                            }
                            var Adapter = new CMDataSetTableAdapters.NotificationsTableAdapter();
                            Adapter.Update(Table);
                        }
                    }
                }
                #endregion
            }

            if(Done != null)
            {
                Done();
                return;
            }

            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "화물접수", 1), "화물접수 수정 성공");

            if (dataGridView1.RowCount > 1)
            {
                int _RowIndex = ordersBindingSource.Position;
                DataLoad();
                dataGridView1.CurrentCell = dataGridView1.Rows[_RowIndex].Cells[0];
            }
            else
            {
                DataLoad();
            }
        }

        private void DeleteData()
        {
            if (ordersBindingSource == null || ordersBindingSource.Current == null)
                return;
            // Data Test
            var Row = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            int? _OrderStatus = null;

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT OrderStatus FROM Orders WHERE OrderId = @OrderId";
                    _Command.Parameters.AddWithValue("@OrderId", Row.OrderId);
                    var _o = _Command.ExecuteScalar();
                    if (_o != null)
                    {
                        _OrderStatus = Convert.ToInt32(_o);
                    }
                }
                _Connection.Close();
            }

            if (_OrderStatus == null)
            {
                orderDataSet.Orders.Rows.Remove(Row);
                return;
            }
            else if (_OrderStatus == 3)
            {
                int _OrderId = Row.OrderId;
                MessageBox.Show("배차완료 건은 삭제할 수 없습니다.", "화물접수삭제");
                DataLoad();
                foreach (DataGridViewRow _ViewRow in dataGridView1.Rows)
                {
                    var _Row = ((OrderDataSet.OrdersRow)_ViewRow.DataBoundItem);
                    if (_Row.OrderId == _OrderId)
                    {
                        _ViewRow.Selected = true;
                        break;
                    }
                }
                return;
            }

            // 삭제 하기 전 NULL 값 설정 - 왜 필요한지 이해가 안감
            ////ClientId
            //if (Row.ClientId == 0)
            //    Row.SetClientIdNull();
            ////CustomerId
            //if (Row.CustomerId == 0)
            //    Row.SetCustomerIdNull();
            ////TradeId
            //if (Row.TradeId == 0)
            //    Row.SetTradeIdNull();
            ////SalesManageId
            //if (Row.SalesManageId == 0)
            //    Row.SetSalesManageIdNull();
            ////DriverId
            //if (Row.DriverId == 0)
            //    Row.SetDriverIdNull();
            if (MessageBox.Show("화물을 삭제하시겠습니까?", "화물접수", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Row.Delete();
            ordersTableAdapter.Update(orderDataSet.Orders);

            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "화물접수", 1), "화물접수 삭제 성공");
        }

        // 배차 내역

        private int AppendDriver()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(DriverCarNo.Text))
                {
                    MessageBox.Show("차량을 우측 표에서 선택하거나, 직접 입력 한 후 배차완료 해주세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return 0;
                }
                if (DriverCarNo.Text.Length < 2)
                {
                    MessageBox.Show("차량 번호는 최소 2자 이상 입력해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return 0;
                }
                bool _IsValid = false;
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT DriverId FROM Drivers WHERE CarNo = @CarNo AND CandidateId = @ClientId";
                        _Command.Parameters.AddWithValue("@CarNo", DriverCarNo.Text);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        var o = _Command.ExecuteScalar();
                        _IsValid = (o == null);
                    }
                    _Connection.Close();
                }
                if (!_IsValid)
                {
                    if (String.IsNullOrWhiteSpace(DriverCarNo.Text))
                    {
                        MessageBox.Show($"차량번호 {DriverCarNo.Text}는 이미 등록되어 있는 차량입니다. 해당 차량을 선택하여 배차완료 하여주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return 0;
                    }
                }
                if (MessageBox.Show($"차량번호 {DriverCarNo.Text}로 차량을 새로 추가하고 배차완료하겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return 0;

                var _Name = DriverName.Text;
                var _BizNo = DriverBizNo.Text;
                var _CarType = (int)DriverCarType.SelectedValue;
                var _CarNo = DriverCarNo.Text;
                var _MobileNo = DriverMobileNo.Text;
                if (String.IsNullOrWhiteSpace(_Name))
                    _Name = _CarNo;
                if (!Regex.Match(_BizNo, @"^\d{3}-\d{2}-\d{5}$").Success)
                {
                    _BizNo = "000-00-00000";
                }
                if (!Regex.Match(_MobileNo, @"^01[0,1,6,7,8,9]-\d{3,4}-\d{4}$").Success)
                {
                    _MobileNo = "010-0000-0000";
                }
                DriverRepository mDriverRepository = new DriverRepository();
                int _DriverId = mDriverRepository.CreateDriver(_BizNo, _Name, _Name, "", "", "", "", "", "", "", _MobileNo, "", "", "", "", _CarNo, _CarType, 0, "",0,0,0);
                mDriverRepository.Select(baseDataSet.Drivers);
                return _DriverId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 작업 중 오류가 발생하였습니다. 잠시 후에 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return 0;
            }
        }

        private void SetDriver()
        {
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Selected == null)
                return;
            if (Selected.OrderStatus != 1)
                return;
            if(Selected.Price <=0 || Selected.ClientPrice <= 0)
            {
                MessageBox.Show("차주운송료/화주운송료를 확인해주십시오. 금액이 없는 배차는 할 수 없습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                if (Selected.Price < 10000)
                {
                    MessageBox.Show("차주운송료는 최소금액이 10,000원 입니다. 금액을 확인 부탁드립니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            if (newDGV1.SelectedRows.Count == 0)
            {
                var _DriverId = AppendDriver();
                if (_DriverId == 0)
                    return;
                Selected.DriverId = _DriverId;
            }
            else
            {
                if (driversBindingSource.Current == null)
                    return;
                var Driver = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
                if (Driver == null)
                    return;
                Selected.DriverId = Driver.DriverId;
            }
            Selected.OrderStatus = 3;
            Selected.AcceptTime = DateTime.Now;
            Selected.Wgubun = "PC";
            #region 미수금 추가
            if (Selected.PointMethod == 2)
            {
                decimal _Amount = 0;
                _Amount = Math.Floor((Selected.Price) * 0.077m) * -1m;
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "INSERT INTO DriverPoints (CDate, Amount, OrderId, DriverId, ClientId, Remark,PointItem) VALUES (GETDATE(), @Amount, @OrderId, @DriverId, @ClientId, @Remark,@PointItem)";
                    _Command.Parameters.AddWithValue("@Amount", _Amount);
                    _Command.Parameters.AddWithValue("@OrderId", Selected.OrderId);
                    _Command.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.Parameters.AddWithValue("@Remark", String.Format("배차 수수료: {0}({1:N0})", CustomerName.Text, _Amount));
                    _Command.Parameters.AddWithValue("@PointItem", "사용");
                    _Command.ExecuteNonQuery();
                }
            }
            #endregion
            UpdateAndLoad(Selected);
        }

        private void RemoveDriver()
        {
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Selected == null)
            {
                return;
            }
            else
            {
                if (Selected.Wgubun == "APP")
                {
                    MessageBox.Show("차주가 직접 배차한 건은 차주만 취소 할 수 있습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    if (Selected.OrderStatus == 3)
                    {
                        var _Driver = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == Selected.DriverId);
                        if(_Driver == null)
                        {
                            _Driver = TTable.FirstOrDefault(c => c.DriverId == Selected.DriverId);
                            if(_Driver != null)
                            {
                                if (MessageBox.Show($"{_Driver.Name}기사님은 등록되어 있지 않습니다. 배차를 취소하고 난 후, 동일한 기사로 다시 배차 하기 힘듭니다. 배차를 취소하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                    return;
                            }
                        }
                        else
                        {
                            if (MessageBox.Show($"{_Driver.Name}기사님 배차를 취소하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                return;
                        }
                        if(_Driver != null)
                        {
                            #region 포인트 취소 추가
                            if (Selected.PointMethod == 2)
                            {
                                decimal _Amount = 0;
                                _Amount = Math.Floor((Selected.Price) * 0.077m) * 1m;
                                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {
                                    _Connection.Open();
                                    SqlCommand _Command = _Connection.CreateCommand();
                                    _Command.CommandText = "INSERT INTO DriverPoints (CDate, Amount, OrderId, DriverId, ClientId, Remark,PointItem) VALUES (GETDATE(), @Amount, @OrderId, @DriverId, @ClientId, @Remark,@PointItem)";
                                    _Command.Parameters.AddWithValue("@Amount", _Amount);
                                    _Command.Parameters.AddWithValue("@OrderId", Selected.OrderId);
                                    _Command.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                    _Command.Parameters.AddWithValue("@Remark", String.Format("배차 취소: {0}({1:N0})", CustomerName.Text, _Amount));
                                    _Command.Parameters.AddWithValue("@PointItem", "사용");
                                    _Command.ExecuteNonQuery();
                                }
                            }
                            #endregion
                        }


                        Selected.SetDriverIdNull();
                        Selected.OrderStatus = 1;
                        Selected.SetAcceptTimeNull();
                        Selected.Wgubun = "";


                        if(Selected.SourceType == 1)
                        {
                            //Notification 추가
                            #region Notification 추가
                            if (!LocalUser.Instance.LogInInformation.IsAdmin)
                            {



                                var iNotificationFilterType = Selected.NotificationFilterType;
                                var iNotificationRadius = Selected.NotificationRadius;
                                var iOrderStatus = Selected.OrderStatus;
                                var DistanceLimite = iNotificationRadius;
                                var iNotificationGroupName = Selected.NotificationGroupName;
                                var iStartState = Selected.StopState;
                                var iStartCity = Selected.StartCity;
                                var iCarType = Selected.CarType;
                                var iCarSize = Selected.CarSize;


                                var iStopState = Selected.StopState;
                                var iStopCity = Selected.StopCity;
                                var iPrice = Price.Text;


                                if (iOrderStatus == 1)
                                {
                                    List<int> DriverIdList = new List<int>();
                                    //행정구역 +- 10Km
                                    if (iNotificationFilterType == 1 || iNotificationFilterType == 4)
                                    {
                                        var x1 = Selected.X;
                                        var y1 = Selected.Y;
                                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                        {
                                            cn.Open();

                                            var Query =
                                                @"SELECT PreOrders.DriverId, X, Y
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            WHERE PreOrders.StopTime > GetDate() AND PreOrders.IsPreview = 0
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            ORDER BY ABS(@x1 - PreOrders.X) + ABS(@y1 - PreOrders.Y)";
                                            var Commad = cn.CreateCommand();
                                            Commad.CommandText = Query;
                                            Commad.Parameters.AddWithValue("@x1", x1);
                                            Commad.Parameters.AddWithValue("@y1", y1);

                                            Commad.Parameters.AddWithValue("@CarType", iCarType);
                                            Commad.Parameters.AddWithValue("@CarSize", iCarSize);

                                            var Reader = Commad.ExecuteReader();
                                            while (Reader.Read())
                                            {
                                                var X = Reader.GetDouble(1);
                                                var Y = Reader.GetDouble(2);
                                                var DriverId = Reader.GetInt32(0);

                                                var Distance = mLocationHelper.DistanceFromGPS(X, Y, x1, y1);
                                                if (Distance > DistanceLimite)
                                                    break;
                                                DriverIdList.Add(DriverId);
                                            }
                                            cn.Close();
                                        }
                                    }
                                    //그룹
                                    if (iNotificationFilterType == 3 || iNotificationFilterType == 4)
                                    {
                                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                        {
                                            cn.Open();

                                            var Query =
                                                @"SELECT PreOrders.DriverId
                            FROM PreOrders 
                            JOIN Drivers ON PreOrders.DriverId = Drivers.DriverId
                            JOIN DriverGroups ON Drivers.DriverId = DriverGroups.DriverId
                            WHERE StopTime > GetDate() AND IsPreview = 0
                            AND DriverGroups.ClientId = @ClientId
                            AND Drivers.CarType = @CarType AND Drivers.CarSize >= @CarSize
                            AND (@iNotificationGroupName = '0' OR DriverGroups.NAME = @iNotificationGroupName)";
                                            var Commad = cn.CreateCommand();
                                            Commad.CommandText = Query;
                                            Commad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                            Commad.Parameters.AddWithValue("@iNotificationGroupName", iNotificationGroupName);

                                            Commad.Parameters.AddWithValue("@CarType", iCarType);
                                            Commad.Parameters.AddWithValue("@CarSize", iCarSize);


                                            var Reader = Commad.ExecuteReader();
                                            while (Reader.Read())
                                            {
                                                var DriverId = Reader.GetInt32(0);
                                                DriverIdList.Add(DriverId);
                                            }
                                            cn.Close();
                                        }
                                    }
                                    if (DriverIdList.Count > 0)
                                    {
                                        var Table = new CMDataSet.NotificationsDataTable();
                                        foreach (var driverId in DriverIdList.Distinct())
                                        {
                                            var Row = Table.NewNotificationsRow();
                                            Row.FilterType = iNotificationFilterType;
                                            Row.State = iStartState;
                                            Row.City = iStartCity;
                                            Row.Street = "";
                                            Row.Radius = iNotificationRadius;
                                            Row.GroupName = iNotificationGroupName;
                                            Row.IsChecked = false;
                                            Row.CreateTime = DateTime.Now;
                                            Row.DriverId = driverId;
                                            Row.ClientId = LocalUser.Instance.LogInInformation.ClientId;
                                            Row.OrderId = Selected.OrderId;
                                            Table.AddNotificationsRow(Row);
                                        }
                                        var Adapter = new CMDataSetTableAdapters.NotificationsTableAdapter();
                                        Adapter.Update(Table);
                                    }
                                }
                            }
                            #endregion

                            //카카오톡배차
                            #region 카카오톡배차
                            //var _NoticeDriver = 0;
                            //var _NoticeCnt = 0;
                            //using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            //{
                            //    _Connection.Open();
                            //    using (SqlCommand _Command = _Connection.CreateCommand())
                            //    {
                            //        _Command.CommandText = "SELECT NoticeDriver, NoticeCnt FROM Clients WHERE ClientId = @ClientId";
                            //        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInfomation.ClientId);
                            //        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                            //        {
                            //            if (_Reader.Read())
                            //            {
                            //                _NoticeDriver = _Reader.GetInt32(0);
                            //                _NoticeCnt = _Reader.GetInt32(1);
                            //            }
                            //        }
                            //    }
                            //    _Connection.Close();
                            //}
                            //if (_NoticeDriver == 1)
                            //{

                            //    if (LocalUser.Instance.LogInInfomation.ClientId > 0)
                            //    {
                            //        var iCarType = Selected.CarType;
                            //        var iCarSize = Selected.CarSize;
                            //        var iOrderStatus = Selected.OrderStatus;
                            //        if (iOrderStatus == 1)
                            //        {
                            //            List<string> DriverMobileNoList = new List<string>();

                            //            if (DriverMobileNoList.Count < _NoticeCnt)
                            //            {
                            //                int DriverIdCnt = _NoticeCnt - DriverMobileNoList.Count;
                            //                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            //                {
                            //                    cn.Open();
                            //                    var Commad = cn.CreateCommand();
                            //                    Commad.CommandText = " SELECT Top " + _NoticeCnt + " NoticeIdx ,MobileNo" +
                            //                    " FROM NOTICEDRIVER " +
                            //                    " WHERE CarType = @CarType AND CarSize >= @CarSize AND ClientId = @ClientId" +
                            //                    " ORDER BY NEWID()";

                            //                    Commad.Parameters.AddWithValue("@DriverIdCnt", DriverIdCnt);
                            //                    Commad.Parameters.AddWithValue("@CarType", iCarType);
                            //                    Commad.Parameters.AddWithValue("@CarSize", iCarSize);
                            //                    Commad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInfomation.ClientId);

                            //                    var Reader = Commad.ExecuteReader();
                            //                    while (Reader.Read())
                            //                    {

                            //                        var DriverId = Reader.GetInt32(0);
                            //                        var MobileNo = Reader.GetString(1);

                            //                        DriverMobileNoList.Add(MobileNo);
                            //                    }
                            //                    cn.Close();
                            //                }

                            //            }

                            //            if (DriverMobileNoList.Count > 0)
                            //            {


                            //                var Table = new CMDataSet.MSG_DATADataTable();
                            //                foreach (var phoneNo in DriverMobileNoList.Distinct())
                            //                {
                            //                    var Row = Table.NewMSG_DATARow();


                            //                    Row.SENDER_KEY = "045bc76bbd0af10b869e9f7b388bb73d4870a3d8";
                            //                    Row.PHONE = phoneNo.Replace("-", "");
                            //                    Row.TMPL_CD = "K18-0005";
                            //                    Row.SEND_MSG = "배차를 위한 화물정보 입니다." + "\n" +
                            //                                    "배차를 받으시려면 하단 배차받기" + "\n" +
                            //                                    "버튼을 클릭 하십시오." + "\n\n" +

                            //                                    " * 상차 : " + Selected.StartTime.ToString("yyyy-MM-dd") + " " + Selected.StartTime.ToString("HH") + ":" + Selected.StartTime.ToString("mm") + " " + "\n" +
                            //                                    " * 출발 : " + Selected.StartState + " " + Selected.StartCity + " " + "\n" +
                            //                                    " * 도착 : " + Selected.StopState + " " + Selected.StopCity + " " + "\n" +
                            //                                    " * 차종 : " + Selected.CarType + "" + "\n" +
                            //                                    " * 톤수 : " + Selected.CarSize + "" + "\n" +
                            //                                    " * 화물 : " + Selected.Item + "" + "\n" +
                            //                                    " * 금액 : " + Selected.Price.ToString("N0") + "" + "\n\n" +

                            //                                    " * 운송회사 : " + LocalUser.Instance.LogInInfomation.UserName + "" + "\n" +
                            //                                    " * 문의전화 : " + LocalUser.Instance.LogInInfomation.PhoneNo + "";

                            //                    Row.REQ_DATE = DateTime.Now;
                            //                    Row.CUR_STATE = "0";
                            //                    Row.SMS_TYPE = "N";
                            //                    Row.ATTACHMENT_TYPE = "button";
                            //                    Row.ATTACHMENT_NAME = "배차받기";
                            //                    Row.ATTACHMENT_URL = $"http://m.cardpay.kr/Link?Id={Selected.OrderId}";
                            //                    Table.AddMSG_DATARow(Row);


                            //                }

                            //                var Adapter = new CMDataSetTableAdapters.MSG_DATATableAdapter();
                            //                Adapter.Update(Table);


                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                        UpdateAndLoad(Selected);
                    }
                }

            }
        }

        private void ClearDriver()
        {
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;
            if (Selected == null)
                return;
            if (Selected.OrderStatus != 1)
                return;

            newDGV1.ClearSelection();
            Selected.SetDriverIdNull();

            DriverName.Clear();
            DriverBizNo.Clear();
            DriverCarType.SelectedIndex = 0;
            DriverCarNo.Clear();
            DriverMobileNo.Clear();

            DriverName.ReadOnly = false;
            DriverBizNo.ReadOnly = false;
            DriverCarType.Enabled = true;
            DriverCarNo.ReadOnly = false;
            DriverMobileNo.ReadOnly = false;
        }
        private void ChangeInfo()
        {
            if (ordersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)ordersBindingSource.Current).Row as OrderDataSet.OrdersRow;

            if (Selected.SourceType == 1)
            {
                if (NotificationFilterType.SelectedIndex == 0)
                {
                    NotificationRadius.Enabled = true;
                    NotificationGroupName.Enabled = false;

                    string ClientAddr;

                    ClientAddr = cmb_StartState.Text + " " + cmb_StartCity.Text;

                    labelA.Visible = true;
                    labelB.Visible = true;
                    labelC.Visible = false;
                    labelD.Visible = false;
                    labelE.Visible = true;


                    labelE.Text = "[" + CarType.Text + "]" + "[" + CarSize.Text + "] 이상";
                    labelA.Text = "[" + ClientAddr + "]";
                    labelA.ForeColor = Color.Blue;
                    labelB.Text = " 반경 " + NotificationRadius.SelectedValue + "km 내 공차로 알림전송 합니다.";
                    labelB.ForeColor = Color.Red;
                }

                else if (NotificationFilterType.SelectedIndex == 1)
                {
                    NotificationRadius.Enabled = false;
                    NotificationGroupName.Enabled = true;

                    string GroupName = string.Empty;
                    if (NotificationGroupName.SelectedIndex == 0)
                    {
                        GroupName = "전체 ";
                    }
                    else
                    {
                        if (NotificationGroupName.SelectedValue != null)
                            GroupName = NotificationGroupName.SelectedValue.ToString();
                        else
                            GroupName = "";
                    }

                    // label1.Text = "[" + LocalUser.Instance.LogInInfomation.UserName + "] " + GroupName + " 그룹 공차를 조회 합니다.";

                    labelA.Visible = true;
                    labelB.Visible = true;
                    labelC.Visible = false;
                    labelD.Visible = false;
                    labelE.Visible = true;


                    labelE.Text = "[" + CarType.Text + "]" + "[" + CarSize.Text + "] 이상";
                    labelA.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";
                    labelA.ForeColor = Color.Blue;
                    labelB.Text = GroupName + " 그룹 공차로 알림전송 합니다.";
                    labelB.ForeColor = Color.Red;
                }
                else if (NotificationFilterType.SelectedIndex == 2)
                {
                    NotificationRadius.Enabled = true;
                    NotificationGroupName.Enabled = true;

                    labelA.Visible = true;
                    labelB.Visible = true;
                    labelC.Visible = true;
                    labelD.Visible = true;
                    labelE.Visible = true;


                    labelE.Text = "[" + CarType.Text + "]" + "[" + CarSize.Text + "] 이상";
                    string GroupName = string.Empty;
                    if (NotificationGroupName.SelectedIndex == 0)
                    {
                        GroupName = "전체 ";
                    }
                    else
                    {
                        GroupName = NotificationGroupName.SelectedValue.ToString();
                    }
                    string ClientAddr;

                    ClientAddr = cmb_StartState.Text + " " + cmb_StartCity.Text;

                    labelA.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";
                    labelA.ForeColor = Color.Blue;
                    labelB.Text = GroupName + "그룹과 ";
                    labelB.ForeColor = Color.Red;
                    labelC.Text = "[" + ClientAddr + "]";
                    labelC.ForeColor = Color.Blue;
                    labelD.Text = " 반경 " + NotificationRadius.SelectedValue + "Km 내 공차로 알림전송 합니다.";
                    labelD.ForeColor = Color.Red;
                }
            }
            else
            {
                labelA.Text = "";
                labelB.Text = "";
                labelC.Text = "";
                labelD.Text = "";
                labelE.Text = "";
            }
        }

        #endregion

        #region STORAGE
        private string GetSelectCommand()
        {
            String SelectCommandText =
                    @"SELECT  Orders.OrderId, Orders.DriverId, Orders.ClientId, Orders.CustomerId, Orders.TradeId, Orders.SalesManageId, 
                        Orders.StartTime, Orders.StartState, Orders.StartCity, Orders.StopTime, Orders.StopState, Orders.StopCity, Orders.ItemSize, 
                        Orders.Item, Orders.AcceptTime, Orders.Price, Orders.ClientPrice, Orders.CarCount, Orders.Etc, 
                        Orders.IsShared, Orders.CarType, Orders.CarSize, Orders.PayLocation, Orders.NotificationFilterType, Orders.NotificationRadius, Orders.NotificationGroupName, 
                        Orders.X, Orders.Y, Orders.OrderStatus, Orders.Wgubun, Orders.CreateTime, Orders.SourceType, Orders.Agubun,
						ISNULL(CONVERT(nvarchar(10), Trades.PayDate,126),N'') as PayDate,
                        ISNULL(CONVERT(nvarchar(10), SalesManage.PayDate,126),N'') as SalesManageDate, Orders.SubClientId, Orders.ClientUserId, ISNULL(Orders.HasPoint, 0) as HasPoint, Orders.StartStreet, Orders.StopStreet,
                        ISNULL(Orders.PointMethod, 0) as PointMethod, ISNULL(Orders.FPIS_ID, 0) as FPIS_ID
                    FROM    Orders
                    LEFT JOIN Drivers ON Orders.DriverId = Drivers.DriverId
                    LEFT JOIN (SELECT TradeId, PayDate FROM Trades WHERE PayState = 1) AS Trades ON Trades.TradeId = Orders.TradeId
                    LEFT JOIN (SELECT SalesId, PayDate FROM SalesManage WHERE PayState = 1) AS SalesManage ON SalesManage.SalesId = Orders.SalesManageId";
            return SelectCommandText;
        }
        private void DataLoad()
        {
            Debug.WriteLine(DateTime.Now);
            mTimer.Enabled = false;
            orderDataSet.Orders.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();
                    // 1. 화주/관리자
                    if(LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            WhereStringList.Add("Orders.ClientUserId = @ClientUserId");
                            _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        }
                        else
                        {
                            WhereStringList.Add("Orders.SubClientId = @SubClientId");
                            _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        }
                    }
                    else if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        WhereStringList.Add("Orders.ClientId = @ClientId");
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        // 1.1 본지점구분
                        if (cmb_SubClientId.SelectedIndex > 0)
                        {
                            var _SubClientId = (int)cmb_SubClientId.SelectedValue;
                            if (_SubClientId == 0)
                            {
                                WhereStringList.Add("Orders.SubClientId IS NULL");
                            }
                            else
                            {
                                WhereStringList.Add("Orders.SubClientId = @SubClientId");
                                _Command.Parameters.AddWithValue("@SubClientId", _SubClientId);
                            }
                        }
                    }
                    // 2. 조회 기간
                    String _DateColumn = cmb_Search.SelectedIndex == 0 ? "Orders.CreateTime" : cmb_Search.SelectedIndex == 1 ? "Orders.StartTime" : "Orders.StopTime";
                    WhereStringList.Add($"{_DateColumn} >= @Begin AND {_DateColumn} < @End");
                    _Command.Parameters.AddWithValue("@Begin", dtpStart.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtpEnd.Value.Date.AddDays(1));
                    // 3. FPIS 
                    if (cmb_FPISSearch.SelectedIndex > 0 && !String.IsNullOrWhiteSpace(txt_FPISSearch.Text))
                    {
                        SelectCommandText += Environment.NewLine + "JOIN Customers ON Customers.CustomerId = Orders.CustomerId";
                        if (cmb_FPISSearch.SelectedIndex == 1)
                        {
                            WhereStringList.Add("Customers.Code = @Customer");
                        }
                        else
                        {
                            WhereStringList.Add("Customers.SangHo Like '%' + @Customer + '%'");
                        }
                        _Command.Parameters.AddWithValue("@Customer", txt_FPISSearch.Text);
                    }
                    // 4. PC/APP
                    if(cmb_I_Gubun.SelectedIndex > 0)
                    {
                        WhereStringList.Add($"Orders.Wgubun LIKE '%{cmb_I_Gubun.Text}'");
                    }
                    // 5. 단어 검색
                    if (cmb_LocationSearch.SelectedIndex > 0)
                    {
                        switch (cmb_LocationSearch.SelectedIndex)
                        {
                            case 1:
                                WhereStringList.Add("(Orders.StartState LIKE '%' + @Text + '%' OR Orders.StartCity LIKE '%' + @Text + '%')");
                                break;
                            case 2:
                                WhereStringList.Add("(Orders.StopState LIKE '%' + @Text + '%' OR Orders.StopCity LIKE '%' + @Text + '%')");
                                break;
                            case 3:
                                WhereStringList.Add("Orders.Item LIKE '%' + @Text + '%'");
                                break;
                            case 4:
                                if (cmb_FPISSearch.SelectedIndex == 0)
                                {
                                    SelectCommandText += Environment.NewLine + "JOIN Customers ON Customers.CustomerId = Orders.CustomerId";
                                    WhereStringList.Add("Customers.SangHo Like '%' + @Text + '%'");
                                }
                                break;
                            case 5:
                                WhereStringList.Add("Drivers.CarNo LIKE '%' + @Text + '%'");
                                break;
                            case 6:
                                WhereStringList.Add("Drivers.CarYear LIKE '%' + @Text + '%'");
                                break;
                            case 7:
                                WhereStringList.Add("Replace(Drivers.MobileNo,'-','') LIKE '%' + @Text + '%'");
                                break;
                            case 8:
                                WhereStringList.Add("Drivers.LoginId LIKE '%' + @Text + '%'");
                                break;
                            default:
                                break;
                        }
                        _Command.Parameters.AddWithValue("@Text", txt_LocationSearch.Text);
                    }
                    // 6 차종
                    if(cmb_CarTypeS.SelectedIndex > 0)
                    {
                        WhereStringList.Add("Drivers.CarType = @CarType");
                        _Command.Parameters.AddWithValue("@CarType", (int)cmb_CarTypeS.SelectedValue);
                    }
                    // 7 배송상태
                    if (cmb_StatusS.SelectedIndex == 1)
                    {
                        WhereStringList.Add("Orders.OrderStatus <> 3");
                    }
                    else if (cmb_StatusS.SelectedIndex == 2)
                    {
                        WhereStringList.Add("Orders.OrderStatus = 3");
                    }
                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        try
                        {
                            orderDataSet.Orders.Load(_Reader);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                _Connection.Close();
            }
            DriverRepository mDriverRepository = new DriverRepository();
            foreach (var order in orderDataSet.Orders)
            {
                if(order.DriverId > 0)
                {
                    if(!baseDataSet.Drivers.Any(c=>c.DriverId == order.DriverId) && !TTable.Any(c => c.DriverId == order.DriverId))
                    {
                        mDriverRepository.SelectOne(TTable, order.DriverId);
                    }
                }
            }
            mTimer.Enabled = chk_AutoLoad.Checked;
        }
        private void UpdateAndLoad(OrderDataSet.OrdersRow Row)
        {
            ordersTableAdapter.Update(Row);
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();
                    SelectCommandText += Environment.NewLine + "WHERE OrderId = @OrderId";
                    _Command.Parameters.AddWithValue("@OrderId", Row.OrderId);
                    _Command.CommandText = SelectCommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        orderDataSet.Orders.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }
        #endregion

        #region Helper
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
        LocationHelper mLocationHelper = new LocationHelper();
        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        private void InitializeDataSource()
        {
            // 조회
            cmb_StatusS.DisplayMember = "Text";
            cmb_StatusS.ValueMember = "Value";
            cmb_StatusS.DataSource = Filter.Order.StatusListWithAll;

            cmb_CarTypeS.DisplayMember = "Text";
            cmb_CarTypeS.ValueMember = "Value";
            cmb_CarTypeS.DataSource = Filter.Order.CarTypeList;

            // 상세 정보
            foreach (var item in SingleDataSet.Instance.AddressReferences)
            {
                if (String.IsNullOrEmpty(item.State) || String.IsNullOrEmpty(item.City))
                    continue;
                if (!AddressList.Any(c => c.State == item.State && c.City == item.City))
                {
                    AddressList.Add(new AddressViewModel
                    {
                        State = item.State,
                        City = item.City,
                    });
                }
            }

            var StartStateDataSource = (from a in AddressList select new { a.State }).Distinct().ToArray();
            cmb_StartState.DataSource = StartStateDataSource;
            cmb_StartState.DisplayMember = "State";
            cmb_StartState.ValueMember = "State";

            var EndStateDataSource = (from a in AddressList select new { a.State }).Distinct().ToArray();
            cmb_EndState.DataSource = EndStateDataSource;
            cmb_EndState.DisplayMember = "State";
            cmb_EndState.ValueMember = "State";

            cmb_StartCity.DisplayMember = "City";
            cmb_StartCity.ValueMember = "City";

            cmb_EndCity.DisplayMember = "City";
            cmb_EndCity.ValueMember = "City";

            IsShared.DisplayMember = "Text";
            IsShared.ValueMember = "BoolValue";
            IsShared.DataSource = Filter.Order.IsSharedList;

            CarType.DisplayMember = "Text";
            CarType.ValueMember = "Value";
            CarType.DataSource = Filter.Order.CarTypeList;

            DriverCarType.DisplayMember = "Text";
            DriverCarType.ValueMember = "Value";
            DriverCarType.DataSource = Filter.Order.CarTypeList;


            CarSize.DisplayMember = "Text";
            CarSize.ValueMember = "Value";
            CarSize.DataSource = Filter.Order.CarSizeList;

            PayLocation.DisplayMember = "Text";
            PayLocation.ValueMember = "Value";
            PayLocation.DataSource = Filter.Order.LocationList;

            NotificationFilterType.DisplayMember = "Text";
            NotificationFilterType.ValueMember = "Value";
            NotificationFilterType.DataSource = Filter.Order.FilterTypeList;

            NotificationRadius.DisplayMember = "Text";
            NotificationRadius.ValueMember = "Value";
            NotificationRadius.DataSource = Filter.Order.RadiusList;

            Dictionary<string, string> NotificationGroup = new Dictionary<string, string>();
            NotificationGroup.Add("0", "그룹전체");
            NotificationGroup.Add("A", "A 그룹");
            NotificationGroup.Add("B", "B 그룹");
            NotificationGroup.Add("C", "C 그룹");

            NotificationGroupName.DataSource = new BindingSource(NotificationGroup, null);
            NotificationGroupName.DisplayMember = "Value";
            NotificationGroupName.ValueMember = "Key";

            if(!LocalUser.Instance.LogInInformation.IsAdmin && !LocalUser.Instance.LogInInformation.IsSubClient)
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

        class CustomerViewModel
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string PhoneNo { get; set; }

            public string CBizNo { get; set; }
        }
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>();
        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }
        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();
        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name FROM Clients";
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
                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        private void InitCustomerTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT CustomerId, SangHo, PhoneNo,BizNo FROM Customers WHERE ClientId = @ClientId";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var dataReader = commnad.ExecuteReader();
                    while (dataReader.Read())
                    {
                        _CustomerViewModelList.Add(
                          new CustomerViewModel
                          {
                              CustomerId = dataReader.GetInt32(0),
                              Name = dataReader.GetStringN(1),
                              PhoneNo = dataReader.GetStringN(2),
                              CBizNo = dataReader.GetStringN(3),
                          });
                    }
                }
                connection.Close();
            }
        }
        #endregion

        private void newDGV1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void txt_CarSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_CarSearch_Click(null, null);
        }

        private void box4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.box4.ClientRectangle, box4.BorderColor, ButtonBorderStyle.Dotted);
        }

        private void box4_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void box5_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.box5.ClientRectangle, box5.BorderColor, ButtonBorderStyle.Dotted);
        }

        private void box5_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {

                di.Create();
            }
            var fileString = "회원가입_배차정보입력양식_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.회원가입_배차정보입력양식_Blank))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 3;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                var _Model = ((DataRowView)dataGridView1.Rows[i].DataBoundItem).Row as OrderDataSet.OrdersRow;
              

                _Sheet.Cells[RowIndex, 1].Value = (i + 1).ToString();


                if (_Model.CustomerId != 0)
                {

               
                    _Sheet.Cells[RowIndex, 2].Value = _CustomerViewModelList.FirstOrDefault(c => c.CustomerId == _Model.CustomerId).Name;
                    _Sheet.Cells[RowIndex, 3].Value = _CustomerViewModelList.FirstOrDefault(c => c.CustomerId == _Model.CustomerId).CBizNo.Replace("-", "");
                }
                else
                {
                    _Sheet.Cells[RowIndex, 2].Value = "";
                    _Sheet.Cells[RowIndex, 3].Value = "";

                }
                _Sheet.Cells[RowIndex, 4].Value = _Model.StartTime.ToString("yyyyMMdd");
                _Sheet.Cells[RowIndex, 5].Value = _Model.StopTime.ToString("yyyyMMdd");
                _Sheet.Cells[RowIndex, 6].Value = _Model.StartState;
                _Sheet.Cells[RowIndex, 7].Value = _Model.StartCity;
                _Sheet.Cells[RowIndex, 8].Value = _Model.StopState;
                _Sheet.Cells[RowIndex, 9].Value = _Model.StopCity;
                _Sheet.Cells[RowIndex, 10].Value = _Model.ItemSize;
                _Sheet.Cells[RowIndex, 11].Value = _Model.ClientPrice;
                _Sheet.Cells[RowIndex, 12].Value = _Model.Price;
                _Sheet.Cells[RowIndex, 13].Value = _Model.Etc;

                if (_Model.DriverId != 0)
                {
                    _Sheet.Cells[RowIndex, 14].Value = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _Model.DriverId).CarNo;
                    _Sheet.Cells[RowIndex, 15].Value = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _Model.DriverId).CarYear;
                    _Sheet.Cells[RowIndex, 16].Value = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _Model.DriverId).BizNo;
                    _Sheet.Cells[RowIndex, 17].Value = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == _Model.DriverId).MobileNo;
                }
                else
                {
                    _Sheet.Cells[RowIndex, 14].Value = "";
                    _Sheet.Cells[RowIndex, 15].Value = "";
                    _Sheet.Cells[RowIndex, 16].Value = "";
                    _Sheet.Cells[RowIndex, 17].Value = "";

                }
                _Sheet.Cells[RowIndex, 18].Value = _Model.CarType;
                _Sheet.Cells[RowIndex, 19].Value = _Model.CarSize;
               
               
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

        private void cmb_StartCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void cmb_EndCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void NotificationFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void chk_AutoLoad_CheckedChanged(object sender, EventArgs e)
        {
            mTimer.Enabled = chk_AutoLoad.Checked;
        }

        private void FrmMN0301_CARGOACCEPT_FormClosing(object sender, FormClosingEventArgs e)
        {
            mTimer.Enabled = false;
        }
    }
}
