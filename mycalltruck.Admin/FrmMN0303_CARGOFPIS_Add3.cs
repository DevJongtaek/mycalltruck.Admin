using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using static mycalltruck.Admin.CMDataSet;

namespace mycalltruck.Admin
{
    public partial class FrmMN0303_CARGOFPIS_Add3 : Form
    {
        private decimal iPrice = 0;
        private decimal iClientPrice = 0;
        private int iCarCount = 0;
        private int iDriverId = 0;
        public String iCEO { get; set; }
        public String iName { get; set; }
        public String iCarNo { get; set; }
        public String iMobileNo { get; set; }
        private int iCarType = 0;
        private int iCarSize = 0;
        ShareOrderDataSet ShareOrderDataSet = ShareOrderDataSet.Instance;
        DriverModel mDriverModel = null;

        public FrmMN0303_CARGOFPIS_Add3()
        {
            InitializeComponent();

            StartTime.Value = DateTime.Now;
            CarCount.Text = "1";
            LocalUser.Instance.LogInInformation.LoadClient();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "배차정보", 1), "배차정보 추가 성공");
                DriverName.Text = "";
                Driver.Text = "";
                StartTime.Value = DateTime.Now;
                DriverPhoneNo.Text = "";
                ItemSize.Text = "";
                Price.Text = "";
                ClientPrice.Text = "";
                CarCount.Text = "1";
            }
        }
        public bool IsSuccess = false;
        private int _UpdateDB()
        {
            err.Clear();

            if(mDriverModel == null)
            {
                MessageBox.Show("배차 차량이 없습니다.");
                return -1;
            }

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("해당하는 화물 의뢰자가 없습니다.");
                return -1;
            }

            if (Driver.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(Driver, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (String.IsNullOrEmpty(Price.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            else if (!decimal.TryParse(Price.Text.Replace(",", ""), out iPrice))
            {
                MessageBox.Show("차주운송료를 숫자로 입력해 주십시오.");
                return -1;
            }

            if (String.IsNullOrEmpty(ClientPrice.Text))
            {
                ClientPrice.Text = Price.Text;
            }
            else if (!decimal.TryParse(ClientPrice.Text.Replace(",", ""), out iClientPrice))
            {
                MessageBox.Show("화주운송료를 숫자로 입력해 주십시오.");
                return -1;
            }
            Int32.TryParse(CarCount.Text.Replace(",", ""), out iCarCount);
            try
            {
                return _AddClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        private int _AddClient()
        {
            var SelectedCustomer = _CustomerViewModelList[dataGridView2.SelectedRows[0].Index];
            try
            {
                Order nOrder = new Order
                {
                    CreateTime = StartTime.Value,
                    ClientId = LocalUser.Instance.LogInInformation.ClientId,
                    StartState = StartState.Text,
                    StartCity = StartCity.Text,
                    StartStreet = StartStreet.Text,
                    StartDetail = Start.Text,
                    StopState = StopState.Text,
                    StopCity = StopCity.Text,
                    StopStreet = StopStreet.Text,
                    StopDetail = Stop.Text,
                    CarCount = int.Parse(CarCount.Text),
                    Price = int.Parse(Price.Text.Replace(",", "")),
                    ClientPrice = int.Parse(ClientPrice.Text.Replace(",", "")),
                    Item = Item.Text,
                    Remark = Item.Text,
                    ItemSize = ItemSize.Text,
                    StartDate = StartTime.Value,
                    StartTime = StartTime.Value,
                    StopTime = StartTime.Value,
                    CarSize = (int)CarSize.SelectedValue,
                    CarType = (int)CarType.SelectedValue,
                };
                nOrder.OrderPhoneNo = SelectedCustomer.PhoneNo;
                nOrder.CustomerId = SelectedCustomer.CustomerId;
                nOrder.Customer = SelectedCustomer.Name;
                if (String.IsNullOrEmpty(nOrder.OrderPhoneNo))
                {
                    string PhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo.Replace("-", "");
                    nOrder.OrderPhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;
                }
                nOrder.DriverId = mDriverModel.DriverId;
                nOrder.Driver = mDriverModel.CarYear;
                nOrder.DriverCarModel = mDriverModel.Name;
                nOrder.DriverCarNo = mDriverModel.CarNo;
                nOrder.DriverPhoneNo = mDriverModel.MobileNo;
                nOrder.OrderStatus = 3;
                nOrder.AcceptTime = DateTime.Now;
                nOrder.Wgubun = "PC";

                ShareOrderDataSet.Orders.Add(nOrder);
                ShareOrderDataSet.SaveChanges();
                return 1;
            }
            catch(Exception ex)
            {
                MessageBox.Show("배차정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return -1;
            }

        }
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "배차", 1), "배차정보 추가 성공");
                IsSuccess = true;
                Close();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmMN0207_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            InitCustomerTable();
            SetDriverList();
            CarSize.DisplayMember = "Name";
            CarSize.ValueMember = "Value";
            CarType.DisplayMember = "Name";
            CarType.ValueMember = "Value";
            CarSize.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value != 0 && c.Value != 99).ToList());
            CarSize.SelectedValue = 1;
            CarSize.SelectedIndexChanged += CarSize_SelectedIndexChanged;
            CarSize_SelectedIndexChanged(null, null);
        }


        private void FrmMN0303_CARGOFPIS_Add3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                btnAdd_Click(null, null);
            else if (e.KeyCode == Keys.F6)
                btnAddClose_Click(null, null);
        }
        class ExcelItem
        {

            public string CONT_YN { get; set; }
            public string C_Gubun { get; set; }
            public string SangHo { get; set; }
            public string BizNo { get; set; }
            public string ContDate { get; set; }
            public string ContMoney { get; set; }
            public string CarNo { get; set; }
            public string StopDate { get; set; }

            public string Money { get; set; }

            public string Error { get; set; }
        }
        private void btnExcelInsert_Click(object sender, EventArgs e)
        {
            if (LocalUser.Instance.LogInInformation.ExcelType == 2)
            {
                EXCELINSERT2 _Form = new EXCELINSERT2();
                _Form.Owner = this;
                _Form.StartPosition = FormStartPosition.CenterParent;
                _Form.ShowDialog(

                    );
            }
            else if (LocalUser.Instance.LogInInformation.ExcelType == 3)
            {
                EXCELINSERT3 _Form = new EXCELINSERT3();
                _Form.Owner = this;
                _Form.StartPosition = FormStartPosition.CenterParent;
                _Form.ShowDialog(

                    );
            }
            else if (LocalUser.Instance.LogInInformation.ExcelType == 4)
            {
                EXCELINSERT4 _Form = new EXCELINSERT4();
                _Form.Owner = this;
                _Form.StartPosition = FormStartPosition.CenterParent;
                _Form.ShowDialog(

                    );
            }


        }

        private void btnExcelDown_Click(object sender, EventArgs e)
        {
            int iExcelType = 0;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT ExcelType FROM Clients WHERE ClientId = @ClientId";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    iExcelType = Convert.ToInt32(_Command.ExecuteScalar());
                }
                _Connection.Close();
            }
            if (iExcelType == 2)
            {
                string fileString = "엑셀_배차관리_외부양식_#1_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                byte[] ieExcel = Properties.Resources.엑셀_배차관리_외부양식__1;
                DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

                if (di.Exists == false)
                {
                    di.Create();
                }
                var FileName = System.IO.Path.Combine(di.FullName, fileString);
                FileInfo file = new FileInfo(FileName);
                try
                {
                    if (file.Exists)
                    {
                        if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            return;
                        file.Delete();
                    }
                    File.WriteAllBytes(FileName, ieExcel);
                }
                catch
                {
                    MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 이름의 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                System.Diagnostics.Process.Start(FileName);
            }
            else if (iExcelType == 3)
            {
                string fileString = "회원가입_배차정보입력양식_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                byte[] ieExcel = Properties.Resources.회원가입_배차정보입력양식;
                DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

                if (di.Exists == false)
                {
                    di.Create();
                }
                var FileName = System.IO.Path.Combine(di.FullName, fileString);
                FileInfo file = new FileInfo(FileName);
                try
                {
                    if (file.Exists)
                    {
                        if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            return;
                        file.Delete();
                    }
                    File.WriteAllBytes(FileName, ieExcel);
                }
                catch
                {
                    MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 이름의 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                System.Diagnostics.Process.Start(FileName);
            }
            else if(iExcelType == 4)
            {
                MessageBox.Show("화주 업체에서 부터 양식을 다운로드 하십시오.", Text);
                return;
            }
        }

        private void txt_CAR_GOODS_CNT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void btn_FpisContInsert_Click(object sender, EventArgs e)
        {
            FrmMNFPISCONT _Form = new FrmMNFPISCONT();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );

            Price.Text = _Form.txt_CONT_DEPOSIT.Text;
        }

        private void Price_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
        }

        private void Price_Leave(object sender, EventArgs e)
        {
            decimal _d = 0;
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
            if (decimal.TryParse(((TextBox)sender).Text, out _d))
            {
                ((TextBox)sender).Text = _d.ToString("N0");
            }
        }

        private void Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        #region CUSTOMER
        class CustomerViewModel
        {
            public int? CustomerId { get; set; }
            public string Name { get; set; }
            public string PhoneNo { get; set; }
        }
        private List<CustomerViewModel> _CustomerViewModelList = new List<CustomerViewModel>
        {
            new CustomerViewModel
            {
                Name = LocalUser.Instance.LogInInformation.Client.Name,
                PhoneNo = LocalUser.Instance.LogInInformation.Client.PhoneNo,
            }
        };
        private void InitCustomerTable()
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    var _CommandText = "SELECT CustomerId, SangHo, PhoneNo FROM Customers WHERE ClientId = @ClientId"; ;
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            _CommandText += " AND ClientUserId = @ClientUserId";
                            _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        }
                        else
                        {
                            _CommandText += " AND SubClientId = @SubClientId";
                            _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        }
                    }
                    _Command.CommandText = _CommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _CustomerViewModelList.Add(
                              new CustomerViewModel
                              {
                                  CustomerId = _Reader.GetInt32(0),
                                  Name = _Reader.GetStringN(1),
                                  PhoneNo = _Reader.GetStringN(2),
                              });
                        }
                    }
                }
                _Connection.Close();
            }
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = _CustomerViewModelList;
        }
        #endregion

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            lbl_Customer.Text = "";
            if (dataGridView2.CurrentCell == null)
            {
                return;
            }
            if (dataGridView2.CurrentCell.RowIndex < 0)
            {
                return;
            }
            var SelectedCustomer = _CustomerViewModelList[dataGridView2.CurrentCell.RowIndex];
            lbl_Customer.Text = SelectedCustomer.Name;
        }

        private void btnExcelInsert_24_New_Click(object sender, EventArgs e)
        {
            EXCELINSERT3 _Form = new EXCELINSERT3(1);
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void btnExcelInsert_24_Old_Click(object sender, EventArgs e)
        {
            EXCELINSERT3 _Form = new EXCELINSERT3(2);
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void btnExcelInsert_Man_Click(object sender, EventArgs e)
        {
            EXCELINSERT3 _Form = new EXCELINSERT3(3);
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void btnExcelInsert_One_Click(object sender, EventArgs e)
        {
            EXCELINSERT3 _Form = new EXCELINSERT3(4);
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void Start_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Start.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    StartSelect.Items.Clear();
                    int ItemIndex = 0;
                    foreach (var _Address in SingleDataSet.Instance.AddressReferences.Where(c => c.State.Contains(Start.Text) || c.City.Contains(Start.Text) || c.Street.Contains(Start.Text)))
                    {
                        StartSelect.Items.Add(_Address.State);
                        StartSelect.Items[ItemIndex].SubItems.Add(_Address.City);
                        StartSelect.Items[ItemIndex].SubItems.Add(_Address.Street);
                        ItemIndex++;
                    }
                    if (StartSelect.Items.Count > 0)
                    {
                        StartSelect.Items[0].Selected = true;
                        StartSelect.Visible = true;
                        StartSelect.Focus();
                    }
                    else
                    {
                        Start.Clear();
                    }
                }
            }
        }

        private void StartSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (StartSelect.SelectedItems.Count > 0)
                {
                    _StartSelected();
                }
            }
        }

        private void StartSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in StartSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _StartSelected();
                    }
                }
            }
        }

        private void _StartSelected()
        {
            Start.Clear();
            StartState.Text = StartSelect.SelectedItems[0].Text;
            StartCity.Text = StartSelect.SelectedItems[0].SubItems[1].Text;
            StartStreet.Text = StartSelect.SelectedItems[0].SubItems[2].Text;
            StartSelect.Visible = false;
            Start.Focus();
        }

        private void Stop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Stop.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    StopSelect.Items.Clear();
                    int ItemIndex = 0;
                    foreach (var _Address in SingleDataSet.Instance.AddressReferences.Where(c => c.State.Contains(Stop.Text) || c.City.Contains(Stop.Text) || c.Street.Contains(Stop.Text)))
                    {
                        StopSelect.Items.Add(_Address.State);
                        StopSelect.Items[ItemIndex].SubItems.Add(_Address.City);
                        StopSelect.Items[ItemIndex].SubItems.Add(_Address.Street);
                        ItemIndex++;
                    }
                    if (StopSelect.Items.Count > 0)
                    {
                        StopSelect.Items[0].Selected = true;
                        StopSelect.Visible = true;
                        StopSelect.Focus();
                    }
                    else
                    {
                        Stop.Clear();
                    }
                }
            }
        }

        private void StopSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (StopSelect.SelectedItems.Count > 0)
                {
                    _StopSelected();
                }
            }
        }

        private void StopSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in StopSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _StopSelected();
                    }
                }
            }
        }

        private void _StopSelected()
        {
            Stop.Clear();
            StopState.Text = StopSelect.SelectedItems[0].Text;
            StopCity.Text = StopSelect.SelectedItems[0].SubItems[1].Text;
            StopStreet.Text = StopSelect.SelectedItems[0].SubItems[2].Text;
            StopSelect.Visible = false;
            Stop.Focus();
        }

        private void Driver_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Driver.Text.Length > 1)
            {
                if (e.KeyChar == 13)
                {
                    if (LocalUser.Instance.LogInInformation.Client.NeedCustomerId && Price.Text == "0")
                    {
                        MessageBox.Show("배차를 입력하기 전, 먼저 운송료를 설정하여 주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    DriverSelect.Items.Clear();
                    int ItemIndex = 0;
                    foreach (var _Driver in DriverModelList.Where(c => c.CarNo.Contains(Driver.Text) || c.CarYear.Contains(Driver.Text) || c.MobileNo.Replace("-", "").Contains(Driver.Text.Replace("-", ""))))
                    {
                        DriverSelect.Items.Add(_Driver.CarYear);
                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == _Driver.CarType))
                        {
                            DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == _Driver.CarType).Name);
                        }
                        else
                        {
                            DriverSelect.Items[ItemIndex].SubItems.Add("");
                        }
                        if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == _Driver.CarSize))
                        {
                            DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == _Driver.CarSize).Name);
                        }
                        else
                        {
                            DriverSelect.Items[ItemIndex].SubItems.Add("");
                        }

                        DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.MobileNo);
                        DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.CarNo);

                        DriverSelect.Items[ItemIndex].Tag = _Driver.DriverId;
                        ItemIndex++;
                    }
                    if (DriverSelect.Items.Count == 0)
                    {
                        SetDriverList();
                        foreach (var _Driver in DriverModelList.Where(c => c.CarYear.Contains(Driver.Text) || c.CarNo.Contains(Driver.Text) || c.MobileNo.Replace("-", "").Contains(Driver.Text.Replace("-", ""))))
                        {
                            DriverSelect.Items.Add(_Driver.CarYear);
                            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == _Driver.CarType))
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == _Driver.CarType).Name);
                            }
                            else
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add("");
                            }
                            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == _Driver.CarSize))
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == _Driver.CarSize).Name);
                            }
                            else
                            {
                                DriverSelect.Items[ItemIndex].SubItems.Add("");
                            }

                            DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.MobileNo);
                            DriverSelect.Items[ItemIndex].SubItems.Add(_Driver.CarNo);
                            DriverSelect.Items[ItemIndex].Tag = _Driver.DriverId;
                            ItemIndex++;
                        }
                    }
                    if (DriverSelect.Items.Count > 0)
                    {
                        DriverSelect.Items[0].Selected = true;
                        DriverSelect.Visible = true;
                        DriverSelect.Focus();
                    }
                    else
                    {
                        Driver.Clear();
                    }
                }
            }
        }

        private void DriverSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (DriverSelect.SelectedItems.Count > 0)
                {
                    _DriverSelected();
                }
            }
        }

        private void DriverSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in DriverSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _DriverSelected();
                    }
                }
            }
        }

        private void _DriverSelected()
        {
            mDriverModel = DriverModelList.Find(c => c.DriverId == (int)DriverSelect.SelectedItems[0].Tag);
            DriverName.Text = mDriverModel.CarYear;
            DriverPhoneNo.Text = mDriverModel.MobileNo;
            Driver.Text = mDriverModel.CarNo;
            DriverSelect.Visible = false;
            
        }
        class DriverModel
        {
            public int DriverId { get; set; }
            public string Name { get; set; }
            public string CarNo { get; set; }
            public string MobileNo { get; set; }
            public string CarYear { get; set; }
            public int CarType { get; set; }
            public int CarSize { get; set; }
            public decimal DriverPoint { get; set; }
        }
        List<DriverModel> DriverModelList = new List<DriverModel>();
        private void SetDriverList()
        {
            DriverModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Drivers.DriverId, CEO, Name, CarNo, MobileNo, CarType, CarSize, CarYear, dbo._GetDriverPoint(Drivers.DriverId) as DriverPoint FROM Drivers JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId  WHERE DriverInstances.ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            DriverModelList.Add(new DriverModel
                            {
                                DriverId = _Reader.GetInt32(0),
                                Name = _Reader.GetStringN(2),
                                CarNo = _Reader.GetStringN(3),
                                MobileNo = _Reader.GetStringN(4),
                                CarType = _Reader.GetInt32(5),
                                CarSize = _Reader.GetInt32(6),
                                CarYear = _Reader.GetStringN(7),
                                DriverPoint = _Reader.GetDecimal(8),
                            });
                        }
                    }
                }
                _Connection.Close();
            }
        }

        private void CarSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] SmallCarTypeValues = new int[]{
                2, 3
            };
            int[] MediumCarTypeValues = new int[]{
                0,1,4,5,6,8,9,10,14,16,18,20,21,60,61,62,22,63,64,65,66,67,27,68,69,33,34,70,71
            };
            int[] FiveCarTypeValues = new int[]{
                0,1,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59
            };
            int[] LargeCarTypeValues = new int[]{
                0,1,4,5,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59
            };

            int CarSizeValue = (int)CarSize.SelectedValue;
            if (CarSizeValue == 1)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 2;
            }
            else if (CarSizeValue == 2)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 3;
            }
            else if (new int[] { 3, 4, 5, 6 }.Contains(CarSizeValue))
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && MediumCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 0;
            }
            else if (new int[] { 7 }.Contains(CarSizeValue))
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && FiveCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 0;
            }
            else if (CarSizeValue > 7)
            {
                CarType.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && LargeCarTypeValues.Contains(c.Value)).OrderBy(c => c.Seq).ToList());
                CarType.SelectedValue = 0;
            }
        }


    }
}
