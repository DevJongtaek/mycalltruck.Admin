using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Class.Common;

namespace mycalltruck.Admin
{
    public partial class UCAccount02 : UserControl
    {
        int DriverId = 0;
        int CustomerId = 0;

        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();

        public UCAccount02()
        {
            InitializeComponent();
            SetMouseClick(this);
            Div.SelectedIndex = 0;
        }

        private void UCAcoount02_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
            btn_Search_Click(null, null);
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
            RequestSum.Clear();
            PaySum.Clear();

            DateTime Begin = dtp_Sdate.Value;
            DateTime End = dtp_Edate.Value;
            Schalar Schalar = new Schalar();

            List<UCAccount02Model> T = new List<UCAccount02Model>();
            BindingList<UCAccount02Model> ModelDataSource = new BindingList<UCAccount02Model>();
            if (Div.SelectedIndex == 0)
            {
                foreach (var model in ShareOrderDataSet.Trades.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId
                    && c.PayState == 1 && c.PayDate >= Begin && c.PayDate <= End)
                    .Select(trade => new UCAccount02Model
                    {
                        Date = trade.PayDate.Value,
                        Name = trade.DriverModel.CarNo,
                        TradeAmount = trade.Amount,
                    }))
                {
                    T.Add(model);
                }
                foreach (var model in ShareOrderDataSet.SalesManage.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId
                    && c.PayState == 1 && c.PayDate.HasValue && c.PayDate >= Begin && c.PayDate <= End)
                    .Select(sales => new UCAccount02Model
                    {
                        Date = sales.PayDate.Value,
                        Name = sales.CustomerModel.SangHo,
                        SalesAmount = sales.Amount,
                    }))
                {
                    T.Add(model);
                }
                foreach (var model in T.OrderBy(c => c.Date))
                {
                    ModelDataSource.Add(model);
                }
            }
            if (Div.SelectedIndex == 1)
            {
                foreach (var model in ShareOrderDataSet.SalesManage.Where(c => (CustomerId == 0 || c.CustomerId == CustomerId) && c.ClientId == LocalUser.Instance.LogInInformation.ClientId
                    && c.PayState == 1 && c.PayDate.HasValue && c.PayDate >= Begin && c.PayDate <= End)
                    .Select(sales => new UCAccount02Model
                    {
                        Date = sales.PayDate.Value,
                        Name = sales.CustomerModel.SangHo,
                        SalesAmount = sales.Amount,
                    }))
                {
                    T.Add(model);
                }
                foreach (var model in T.OrderBy(c => c.Date))
                {
                    ModelDataSource.Add(model);
                }
            }
            else if (Div.SelectedIndex == 2)
            {
                foreach (var model in ShareOrderDataSet.Trades.Where(c => (DriverId == 0 || c.DriverId == DriverId) && c.ClientId == LocalUser.Instance.LogInInformation.ClientId
                    && c.PayState == 1 && c.PayDate >= Begin && c.PayDate <= End)
                    .Select(trade => new UCAccount02Model
                    {
                        Date = trade.PayDate.Value,
                        Name = trade.DriverModel.CarNo,
                        TradeAmount = trade.Amount,
                    }))
                {
                    T.Add(model);
                }
                foreach (var model in T.OrderBy(c => c.Date))
                {
                    ModelDataSource.Add(model);
                }
            }
            ModelDataGrid.AutoGenerateColumns = false;
            ModelDataGrid.DataSource = ModelDataSource;
            RequestSum.Text = ModelDataSource.Sum(c => c.TradeAmount).ToString("N0");
            PaySum.Text = ModelDataSource.Sum(c => c.SalesAmount).ToString("N0");
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;

            btn_Search_Click(null, null);
        }



        private void Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            DriverId = 0;
            CustomerId = 0;

            if (Div.SelectedIndex == 1)
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
            else if (Div.SelectedIndex == 2)
            {
                if (e.KeyChar == 13)
                {
                    if (Input.Text.Replace(" ", "").Length < 2)
                        return;
                    DriverSelect.Items.Clear();
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
    }

    class UCAccount02Model
    {
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public decimal TradeAmount { get; set; }
        public decimal SalesAmount { get; set; }
    }
}
