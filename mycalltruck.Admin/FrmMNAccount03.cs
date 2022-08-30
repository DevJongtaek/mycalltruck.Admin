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
    public partial class FrmMNAccount03 : Form
    {
        int DriverId = 0;
        int CustomerId = 0;

        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();

        public FrmMNAccount03()
        {
            InitializeComponent();
            Div.SelectedIndex = 0;
            SetMouseClick(this);
        }

        private void FrmMNSTATS1_Load(object sender, EventArgs e)
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
            DateTime End = dtp_Edate.Value.AddDays(1).AddSeconds(-1);
            Schalar Schalar = new Schalar();

            List<Account03Model> T = new List<Account03Model>();
            BindingList<Account03Model> ModelDataSource = new BindingList<Account03Model>();
            if(Div.SelectedIndex == 0)
            {
                if (LocalUser.Instance.LogInInformation.Client.StaticFromOrder)
                {
                    var Query =  ShareOrderDataSet.Orders.Where(c =>c.AcceptTime >= Begin && c.AcceptTime <=End &&  c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && (c.TradeModel == null || c.TradeModel.PayState != 1)).GroupBy(c=>c.DriverModel);
                    foreach (var model in Query
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.CarNo,
                            TradeAmount = group.Sum(c => (c.TradePrice??0)),
                            IsDriver = true,
                            DriverId = group.Key.DriverId,
                        }))
                    {
                        T.Add(model);
                    }
                }
                else
                {
                    var Query = ShareOrderDataSet.Trades.Where(c => c.RequestDate >= Begin && c.RequestDate <= End && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.PayState != 1).GroupBy(c => c.DriverModel);
                    foreach (var model in Query
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.CarNo,
                            TradeAmount = group.Sum(c => c.Amount),
                            IsDriver = true,
                            DriverId = group.Key.DriverId,
                        }))
                    {
                        T.Add(model);
                    }
                }

                if (LocalUser.Instance.LogInInformation.Client.StaticFromOrder)
                {
                    var Query = ShareOrderDataSet.Orders.Where(c => c.AcceptTime >= Begin && c.AcceptTime <= End && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && (c.SalesManageModel == null || c.SalesManageModel.PayState != 1)).GroupBy(c => c.CustomerModel);
                    foreach (var model in Query
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.SangHo,
                            SalesAmount = group.Sum(c => (c.SalesPrice ?? 0)),
                            IsCustomer = true,
                            CustomerId = group.Key.CustomerId,
                        }))
                    {
                        T.Add(model);
                    }
                }
                else
                {
                    var Query = ShareOrderDataSet.SalesManage.Where(c => c.RequestDate >= Begin && c.RequestDate <= End && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.PayState != 1 && c.CustomerModel != null).GroupBy(c => c.CustomerModel);
                    foreach (var model in Query.Where(group => group.Key != null)
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.SangHo,
                            SalesAmount = group.Sum(c => c.Amount),
                            IsCustomer = true,
                            CustomerId = group.Key.CustomerId,
                        }))
                    {
                        T.Add(model);
                    }
                }
                foreach (var model in T.OrderBy(c=>c.Name))
                {
                    if (model.IsDriver)
                        model.TTradeAmount = Schalar.GetMisubyDriverId(model.DriverId);
                    if (model.IsCustomer)
                        model.TSalesAmount = Schalar.GetMisubyCustomerId(model.CustomerId);

                    ModelDataSource.Add(model);
                }
            }
            if (Div.SelectedIndex == 1)
            {
                if (LocalUser.Instance.LogInInformation.Client.StaticFromOrder)
                {
                    var Query = ShareOrderDataSet.Orders.Where(c => (CustomerId == 0 || c.CustomerId == CustomerId) && c.AcceptTime >= Begin && c.AcceptTime <= End && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && (c.SalesManageModel == null || c.SalesManageModel.PayState != 1)).GroupBy(c => c.CustomerModel);
                    foreach (var model in Query
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.SangHo,
                            SalesAmount = group.Sum(c => (c.SalesPrice ?? 0)),
                            IsCustomer = true,
                            CustomerId = group.Key.CustomerId,
                        }))
                    {
                        T.Add(model);
                    }
                }
                else
                {
                    var Query = ShareOrderDataSet.SalesManage.Where(c => (CustomerId == 0 || c.CustomerId == CustomerId) && c.RequestDate >= Begin && c.RequestDate <= End && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.PayState != 1).GroupBy(c => c.CustomerModel);
                    foreach (var model in Query.Where(group => group.Key != null)
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.SangHo,
                            SalesAmount = group.Sum(c => c.Amount),
                            IsCustomer = true,
                            CustomerId = group.Key.CustomerId,
                        }))
                    {
                        T.Add(model);
                    }
                }
                foreach (var model in T.OrderBy(c => c.Name))
                {
                    if (model.IsDriver)
                        model.TTradeAmount = Schalar.GetMisubyDriverId(model.DriverId);
                    if (model.IsCustomer)
                        model.TSalesAmount = Schalar.GetMisubyCustomerId(model.CustomerId);

                    ModelDataSource.Add(model);
                }
            }
            else if (Div.SelectedIndex == 2)
            {
                if (LocalUser.Instance.LogInInformation.Client.StaticFromOrder)
                {
                    var Query = ShareOrderDataSet.Orders.Where(c => (DriverId == 0 || c.DriverId == DriverId) && c.AcceptTime >= Begin && c.AcceptTime <= End && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && (c.TradeModel == null || c.TradeModel.PayState != 1)).GroupBy(c => c.DriverModel);
                    foreach (var model in Query
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.CarNo,
                            TradeAmount = group.Sum(c => (c.TradePrice ?? 0)),
                            IsDriver = true,
                            DriverId = group.Key.DriverId,
                        }))
                    {
                        T.Add(model);
                    }
                }
                else
                {
                    var Query = ShareOrderDataSet.Trades.Where(c => (DriverId == 0 || c.DriverId == DriverId) && c.RequestDate >= Begin && c.RequestDate <= End && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.PayState != 1).GroupBy(c => c.DriverModel);
                    foreach (var model in Query
                        .Select(group => new Account03Model
                        {
                            Name = group.Key.CarNo,
                            TradeAmount = group.Sum(c => c.Amount),
                            IsDriver = true,
                            DriverId = group.Key.DriverId,
                        }))
                    {
                        T.Add(model);
                    }
                }


                foreach (var model in T.OrderBy(c => c.Name))
                {
                    if (model.IsDriver)
                        model.TTradeAmount = Schalar.GetMisubyDriverId(model.DriverId);
                    if (model.IsCustomer)
                        model.TSalesAmount = Schalar.GetMisubyCustomerId(model.CustomerId);

                    ModelDataSource.Add(model);
                }
            }
            ModelDataGrid.AutoGenerateColumns = false;
            ModelDataGrid.DataSource = ModelDataSource;
            RequestSum.Text = ModelDataSource.Sum(c => c.SalesAmount).ToString("N0");
            PaySum.Text = ModelDataSource.Sum(c => c.TradeAmount).ToString("N0");
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
            else if(Div.SelectedIndex == 2)
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
    }
    class Account03Model
    {
        public String Name { get; set; }
        public decimal TradeAmount { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal Calculated { get; set; }
        public decimal TTradeAmount { get; set; }
        public decimal TSalesAmount { get; set; }
        public bool IsDriver { get; set; }
        public bool IsCustomer { get; set; }
        public int DriverId { get; set; }
        public int CustomerId { get; set; }
    }
}
