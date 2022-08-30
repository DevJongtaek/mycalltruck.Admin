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
    public partial class UCAccount04 : UserControl
    {
        int DriverId = 0;
        int CustomerId = 0;

        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();

        public UCAccount04()
        {
            InitializeComponent();
            SetMouseClick(this);
            Div.SelectedIndex = 0;
        }

        private void UCAccount04_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
            btn_Search_Click(null, null);
        }
        private void SetMouseClick(Control mControl)
        {
            foreach (Control nControl in mControl.Controls)
            {
                if (nControl == CustomerSelect)
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
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {

            DateTime Begin = dtp_Sdate.Value;
            DateTime End = dtp_Edate.Value.AddDays(1).AddSeconds(-1);

            List<UCAccount04Model> T = new List<UCAccount04Model>();
            BindingList<UCAccount04Model> ModelDataSource = new BindingList<UCAccount04Model>();
            var Query = ShareOrderDataSet.Orders.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId &&
                (CustomerId == 0 || c.CustomerId == CustomerId) && c.CustomerModel != null &&
                c.CreateTime >= Begin && c.CreateTime <= End)
                .Select(c => new
                {
                    Id = c.CustomerId.Value,
                    Name = c.CustomerModel.SangHo,
                    Date = c.CreateTime,
                    IsAccepted = c.OrderStatus == 3,
                    IsIssued = c.SalesManageId != null,
                    DriverAmount = c.TradePrice ?? 0,
                    CustomerAmount = c.SalesPrice ?? 0,
                    FeeAmount = ((c.ClientPrice ?? 0) - c.Price),
                    TradePayedAmount = (c.TradeModel != null && c.TradeModel.PayState == 1) ? (c.TradePrice ?? 0) : 0,
                    SalesPayedAmount = (c.SalesManageModel != null && c.SalesManageModel.PayState == 1) ? (c.SalesPrice ?? 0) : 0,
                }).ToArray();

            foreach (var order in Query)
            {
                T.Add(new UCAccount04Model
                {
                    Id = order.Id,
                    Date = order.Date.Date,
                    Name = order.Name,
                    OrderCount = 1,
                    AcceptedCount = order.IsAccepted ? 1 : 0,
                    NotIssuedCount = order.IsIssued ? 0 : 1,
                    NotIssuedAmount = order.IsIssued ? 0 : order.CustomerAmount,
                    IssuedAmount = order.IsIssued == false ? 0 : order.CustomerAmount,
                    DriverAmount = order.DriverAmount,
                    FeeAmount = order.FeeAmount,
                    SalesPayedAmount = order.SalesPayedAmount,
                    TradePayedAmount = order.TradePayedAmount,
                });
            }


            if (Div.SelectedIndex == 0)
            {
                foreach (var group in T.GroupBy(c => new { c.Id, c.Name, c.Date }).OrderBy(c => c.Key.Date).OrderBy(c => c.Key.Name))
                {
                    ModelDataSource.Add(new UCAccount04Model
                    {
                        Id = group.Key.Id,
                        Name = group.Key.Name,
                        Date = group.Key.Date,
                        OrderCount = group.Sum(c => c.OrderCount),
                        AcceptedCount = group.Sum(c => c.AcceptedCount),
                        AcceptedRate = ((group.Sum(c => c.AcceptedCount)) / (group.Sum(c => c.OrderCount))) * 100d,
                        NotIssuedCount = group.Sum(c => c.NotIssuedCount),
                        NotIssuedAmount = group.Sum(c => c.NotIssuedAmount),
                        IssuedAmount = group.Sum(c => c.IssuedAmount),
                        DriverAmount = group.Sum(c => c.DriverAmount),
                        FeeAmount = group.Sum(c => c.FeeAmount),
                        SalesPayedAmount = group.Sum(c => c.SalesPayedAmount),
                        TradePayedAmount = group.Sum(c => c.TradePayedAmount),
                        IsDay = true,
                        FeeRate = (((double)group.Sum(c => c.FeeAmount)) / ((double)group.Sum(c => c.NotIssuedCount) + (double)group.Sum(c => c.IssuedAmount))) * 100d,
                    });
                }
            }

            if (Div.SelectedIndex == 1)
            {
                foreach (var group in T.GroupBy(c => new { c.Id, c.Name, Date = new DateTime(c.Date.Year, c.Date.Month, 1) }).OrderBy(c => c.Key.Date).OrderBy(c => c.Key.Name))
                {
                    ModelDataSource.Add(new UCAccount04Model
                    {
                        Id = group.Key.Id,
                        Name = group.Key.Name,
                        Date = group.Key.Date,
                        OrderCount = group.Sum(c => c.OrderCount),
                        AcceptedCount = group.Sum(c => c.AcceptedCount),
                        AcceptedRate = ((group.Sum(c => c.AcceptedCount)) / (group.Sum(c => c.OrderCount))) * 100d,
                        NotIssuedCount = group.Sum(c => c.NotIssuedCount),
                        NotIssuedAmount = group.Sum(c => c.NotIssuedAmount),
                        IssuedAmount = group.Sum(c => c.IssuedAmount),
                        DriverAmount = group.Sum(c => c.DriverAmount),
                        FeeAmount = group.Sum(c => c.FeeAmount),
                        SalesPayedAmount = group.Sum(c => c.SalesPayedAmount),
                        TradePayedAmount = group.Sum(c => c.TradePayedAmount),
                        IsMonth = true,
                        FeeRate = (((double)group.Sum(c => c.FeeAmount)) / ((double)group.Sum(c => c.NotIssuedCount) + (double)group.Sum(c => c.IssuedAmount))) * 100d,
                    });
                }
            }

            ModelDataGrid.AutoGenerateColumns = false;
            ModelDataGrid.DataSource = ModelDataSource;
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;

            btn_Search_Click(null, null);
        }



        private void Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            CustomerId = 0;
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

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnOrderDate.Index)
            {
                var Model = ModelDataGrid.Rows[e.RowIndex].DataBoundItem as UCAccount04Model;
                if (Model.IsMonth)
                {
                    e.Value = ((DateTime)e.Value).ToString("yyyy-MM");
                }
            }
        }
    }
    class UCAccount04Model
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public int OrderCount { get; set; }
        public int AcceptedCount { get; set; }
        public double AcceptedRate { get; set; }
        public int NotIssuedCount { get; set; }
        public decimal NotIssuedAmount { get; set; }
        public decimal IssuedAmount { get; set; }
        public decimal DriverAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public double FeeRate { get; set; }
        public decimal SalesPayedAmount { get; set; }
        public decimal TradePayedAmount { get; set; }

        public bool IsDay { get; set; }
        public bool IsMonth { get; set; }
    }
}