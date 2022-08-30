using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMNAccount01 : Form
    {
        int DriverId = 0;
        int CustomerId = 0;

        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();

        public FrmMNAccount01()
        {
            InitializeComponent();
            SetMouseClick(this);
            Div.SelectedIndex = 0;
        }

        private void FrmMNSTATS1_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
        }

        private void SetMouseClick(Control mControl)
        {
            foreach (Control nControl in mControl.Controls)
            {
                if (nControl == CustomerSelect || nControl == DriverSelect)
                {
                    continue;
                }
                nControl.MouseClick += NControl_MouseClick;
                SetMouseClick(nControl);
            }
        }

        private void NControl_MouseClick(object sender, MouseEventArgs e)
        {
            CustomerSelect.Visible = false;
            DriverSelect.Visible = false;
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if(Div.SelectedIndex == 0 && CustomerId == 0)
            {
                MessageBox.Show("먼저 거래처를 선택해주세요.");
                return;
            }
            if (Div.SelectedIndex == 1 && DriverId == 0)
            {
                MessageBox.Show("먼저 차량을 선택해주세요.");
                return;
            }

            Misu.Clear();
            RequestSum.Clear();
            PaySum.Clear();

            DateTime Begin = dtp_Sdate.Value;
            DateTime End = dtp_Edate.Value;
            Schalar Schalar = new Schalar();

            BindingList<Account01ParentModel> ParentDataSource = new BindingList<Account01ParentModel>();
            BindingList<Account01Model> ModelDataSource = new BindingList<Account01Model>();
            if (Div.SelectedIndex == 1)
            {
                var Query = ShareOrderDataSet.Trades.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.DriverId == DriverId)
                    .Select(c => new Account01ParentModel
                    {
                        Id = c.TradeId,
                        Date = c.RequestDate,
                        Amount = c.Amount,
                        IsTrade = true,
                    }).OrderBy(c => c.Date).ToArray();
                foreach (var item in Query)
                {
                    ParentDataSource.Add(item);
                }
                var SubQuery = ShareOrderDataSet.Orders.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.DriverId == DriverId)
                    .ToArray()
                    .Select(c => new Account01Model
                    {
                        OrderId = c.OrderId,
                        OrderDate = c.AcceptTime.Value,
                        RequestAmount = c.Price,
                        PayedAmount = (c.TradeId != null && c.TradeModel.PayState == 1) ? c.Price : 0,
                        HasImage = c.ImageA != null,
                        HasParent = c.TradeId != null,
                        IssuedDate = (c.TradeId != null) ? c.TradeModel.RequestDate.ToString("yyyy-MM-dd") : "",
                        PayedDate = (c.TradeId != null && c.TradeModel.PayState == 1) ? c.TradeModel.RequestDate.ToString("yyyy-MM-dd") : "",
                    }).OrderBy(c => c.OrderId).ToArray();
                foreach (var item in SubQuery)
                {
                    ModelDataSource.Add(item);
                }
                ParentDataGrid.AutoGenerateColumns = false;
                ModelDataGrid.AutoGenerateColumns = false;
                ParentDataGrid.DataSource = ParentDataSource;
                ModelDataGrid.DataSource = ModelDataSource;
                ParentDataGrid.CurrentCell = null;
                Misu.Text = Schalar.GetMisubyDriverId(DriverId).ToString("N0");
                RequestSum.Text = ModelDataSource.Sum(c => c.RequestAmount).ToString("N0");
                PaySum.Text = ModelDataSource.Sum(c => c.PayedAmount).ToString("N0");
            }
            else if (Div.SelectedIndex == 0)
            {
                var Query = ShareOrderDataSet.SalesManage.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == CustomerId)
                    .Select(c => new Account01ParentModel
                    {
                        Id = c.SalesId,
                        Date = c.RequestDate,
                        Amount = c.Amount,
                        IsSales = true,
                    }).OrderBy(c => c.Date).ToArray();
                foreach (var item in Query)
                {
                    ParentDataSource.Add(item);
                }
                var SubQuery = ShareOrderDataSet.Orders.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == CustomerId)
                    .ToArray()
                    .Select(c => new Account01Model
                    {
                        OrderId = c.OrderId,
                        OrderDate = c.CreateTime,
                        RequestAmount = c.Price,
                        PayedAmount = (c.SalesManageId != null && c.SalesManageModel.PayState == 1) ? c.Price : 0,
                        HasImage = c.ImageA != null,
                        HasParent = c.SalesManageId != null,
                        IssuedDate = (c.SalesManageId != null) ? c.SalesManageModel.RequestDate.ToString("yyyy-MM-dd") : "",
                        PayedDate = (c.SalesManageId != null && c.SalesManageModel.PayState == 1) ? c.SalesManageModel.RequestDate.ToString("yyyy-MM-dd") : "",
                    }).OrderBy(c => c.OrderId).ToArray();
                foreach (var item in SubQuery)
                {
                    ModelDataSource.Add(item);
                }
                ParentDataGrid.AutoGenerateColumns = false;
                ModelDataGrid.AutoGenerateColumns = false;
                ParentDataGrid.DataSource = ParentDataSource;
                ModelDataGrid.DataSource = ModelDataSource;
                ParentDataGrid.CurrentCell = null;
                Misu.Text = Schalar.GetMisubyCustomerId(CustomerId).ToString("N0");
                RequestSum.Text = ModelDataSource.Sum(c => c.RequestAmount).ToString("N0");
                PaySum.Text = ModelDataSource.Sum(c => c.PayedAmount).ToString("N0");
            }
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;

            btn_Search_Click(null, null);
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {

            if (ParentDataGrid.RowCount < 1)
            {
                MessageBox.Show("출력할 내용이 없습니다. 먼저 데이터를 조회하신 후 출력 버튼을 눌려주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            FrmStatistics f = new FrmStatistics();
            string order = string.Empty;
            if (ParentDataGrid.SortedColumn != null)
            {
                order = ParentDataGrid.SortedColumn.DataPropertyName;
            }

            f.STATS1(dtp_Sdate.Text, dtp_Edate.Text, order, stats1BindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }

        private void Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            DriverId = 0;
            CustomerId = 0;

            if (Div.SelectedIndex == 0)
            {
                if (e.KeyChar == 13)
                {
                    CustomerSelect.Items.Clear();
                    int ItemIndex = 0;
                    foreach (var _Customer in ShareOrderDataSet.Customers.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.SangHo.Contains(Input.Text) || c.PhoneNo.Replace("-", "").Contains(Input.Text.Replace("-", ""))))
                    {
                        CustomerSelect.Items.Add(_Customer.SangHo);
                        CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.PhoneNo);
                        CustomerSelect.Items[ItemIndex].SubItems.Add(_Customer.AddressState + " " + _Customer.AddressCity);
                        CustomerSelect.Items[ItemIndex].Tag = _Customer.CustomerId;
                        ItemIndex++;
                    }
                    if (CustomerSelect.Items.Count > 0)
                    {
                        CustomerSelect.Items[0].Selected = true;
                        CustomerSelect.Visible = true;
                        CustomerSelect.Focus();
                    }
                    else
                    {
                        CustomerId = 0;
                    }
                }
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    if (Input.Text.Replace(" ", "").Length < 2)
                        return;
                    int ItemIndex = 0;
                    foreach (var _Driver in ShareOrderDataSet.Drivers.Where(c => c.CarNo.Contains(Input.Text) || c.CarYear.Contains(Input.Text) || c.MobileNo.Replace("-", "").Contains(Input.Text.Replace("-", ""))))
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
                    if (DriverSelect.Items.Count > 0)
                    {
                        DriverSelect.Items[0].Selected = true;
                        DriverSelect.Visible = true;
                        DriverSelect.Focus();
                    }
                    else
                    {
                        DriverId = 0;
                    }
                }
            }
        }

        private void CustomerSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (CustomerSelect.SelectedItems.Count > 0)
                {
                    _CustomerSelected();
                }
            }
        }

        private void CustomerSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem nListViewItem in CustomerSelect.Items)
                {
                    if (nListViewItem.GetBounds(ItemBoundsPortion.Entire).Contains(e.X, e.Y))
                    {
                        _CustomerSelected();
                    }
                }
            }
        }

        private void _CustomerSelected()
        {
            var _Customer = ShareOrderDataSet.Customers.Find((int)CustomerSelect.SelectedItems[0].Tag);
            Input.Text = _Customer.SangHo;
            CustomerId = _Customer.CustomerId;
            CustomerSelect.Visible = false;
            Input.Focus();
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

            var _Driver = ShareOrderDataSet.Drivers.Find((int)DriverSelect.SelectedItems[0].Tag);

            Input.Text = _Driver.CarYear;
            DriverId = _Driver.DriverId;

            DriverSelect.Visible = false;
            Input.Focus();
        }

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnHasImage.Index)
            {
                if ((bool)e.Value)
                    e.Value = "발송";
                else
                    e.Value = "";
            }
            if (e.ColumnIndex == ColumnHasParent.Index)
            {
                if ((bool)e.Value)
                    e.Value = "발행";
                else
                    e.Value = "";
            }
        }

        private void ParentDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            RequestSum.Clear();
            PaySum.Clear();
            Account01ParentModel ParentModel = ParentDataGrid.Rows[e.RowIndex].DataBoundItem as Account01ParentModel;
            BindingList<Account01Model> ModelDataSource = new BindingList<Account01Model>();
            if (ParentModel.IsTrade)
            {
                var SubQuery = ShareOrderDataSet.Orders.Where(c => c.TradeId == ParentModel.Id)
                    .ToArray()
                    .Select(c => new Account01Model
                    {
                        OrderId = c.OrderId,
                        OrderDate = c.AcceptTime.Value,
                        RequestAmount = c.Price,
                        PayedAmount = (c.TradeId != null && c.TradeModel.PayState == 1) ? c.Price : 0,
                        HasImage = c.ImageA != null,
                        HasParent = c.TradeId != null,
                        IssuedDate = (c.TradeId != null) ? c.TradeModel.RequestDate.ToString("yyyy-MM-dd") : "",
                        PayedDate = (c.TradeId != null && c.TradeModel.PayState == 1) ? c.TradeModel.RequestDate.ToString("yyyy-MM-dd") : "",
                    }).OrderBy(c => c.OrderId).ToArray();
                foreach (var item in SubQuery)
                {
                    ModelDataSource.Add(item);
                }
                ModelDataGrid.AutoGenerateColumns = false;
                ModelDataGrid.DataSource = ModelDataSource;
                RequestSum.Text = ModelDataSource.Sum(c => c.RequestAmount).ToString("N0");
                PaySum.Text = ModelDataSource.Sum(c => c.PayedAmount).ToString("N0");
            }
            if (ParentModel.IsSales)
            {
                var SubQuery = ShareOrderDataSet.Orders.Where(c => c.SalesManageId == ParentModel.Id)
                    .ToArray()
                    .Select(c => new Account01Model
                    {
                        OrderId = c.OrderId,
                        OrderDate = c.AcceptTime.Value,
                        RequestAmount = c.Price,
                        PayedAmount = (c.SalesManageId != null && c.SalesManageModel.PayState == 1) ? c.Price : 0,
                        HasImage = c.ImageA != null,
                        HasParent = c.SalesManageId != null,
                        IssuedDate = (c.SalesManageId != null) ? c.SalesManageModel.RequestDate.ToString("yyyy-MM-dd") : "",
                        PayedDate = (c.SalesManageId != null && c.SalesManageModel.PayState == 1) ? c.SalesManageModel.RequestDate.ToString("yyyy-MM-dd") : "",
                    }).OrderBy(c => c.OrderId).ToArray();
                foreach (var item in SubQuery)
                {
                    ModelDataSource.Add(item);
                }
                ModelDataGrid.AutoGenerateColumns = false;
                ModelDataGrid.DataSource = ModelDataSource;
                RequestSum.Text = ModelDataSource.Sum(c => c.RequestAmount).ToString("N0");
                PaySum.Text = ModelDataSource.Sum(c => c.PayedAmount).ToString("N0");
            }
        }
    }
    class Account01Model
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal RequestAmount { get; set; }
        public decimal PayedAmount { get; set; }
        public bool HasImage { get; set; }
        public bool HasParent { get; set; }
        public String IssuedDate { get; set; }
        public String PayedDate { get; set; }
    }
    class Account01ParentModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool IsTrade { get; set; }
        public bool IsSales { get; set; }
    }
}
