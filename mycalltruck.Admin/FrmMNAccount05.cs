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
    public partial class FrmMNAccount05 : Form
    {
        int DriverId = 0;
        int CustomerId = 0;

        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();

        public FrmMNAccount05()
        {
            InitializeComponent();
            SetMouseClick(this);
            Div.SelectedIndex = 0;
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
                if (nControl == DriverSelect)
                {
                    continue;
                }
                nControl.MouseClick += NControl_MouseClick;
                SetMouseClick(nControl);
            }
        }

        private void NControl_MouseClick(object sender, MouseEventArgs e)
        {
            DriverSelect.Visible = false;
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {

            DateTime Begin = dtp_Sdate.Value;
            DateTime End = dtp_Edate.Value.AddDays(1).AddSeconds(-1);

            List<Account05Model> T = new List<Account05Model>();
            BindingList<Account05Model> ModelDataSource = new BindingList<Account05Model>();
            var Query = ShareOrderDataSet.Orders.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId &&
                (DriverId == 0 || c.DriverId == DriverId) && c.DriverModel != null &&
                c.CreateTime >= Begin && c.CreateTime <= End)
                .Select(c => new
                {
                    Id = c.DriverId.Value,
                    c.DriverModel.CarNo,
                    c.DriverModel.CarYear,
                    c.DriverModel.MobileNo,
                    Date = c.CreateTime,
                    DriverAmount = c.TradePrice ?? 0,
                    CustomerAmount = c.SalesPrice ?? 0,
                    FeeAmount = ((c.SalesPrice ?? 0) - (c.TradePrice ?? 0)),
                    TradePayedAmount = (c.TradeModel != null && c.TradeModel.PayState == 1) ? (c.TradePrice ?? 0) : 0,
                    SalesPayedAmount = (c.SalesManageModel != null && c.SalesManageModel.PayState == 1) ? (c.ClientPrice ?? 0) : 0,
                }).ToArray();

            foreach (var order in Query)
            {
                T.Add(new Account05Model
                {
                    Id = order.Id,
                    Date = order.Date.Date,
                    CarNo = order.CarNo,
                    CarYear = order.CarYear,
                    MobileNo = order.MobileNo,
                    OrderCount = 1,
                    IssuedAmount = order.CustomerAmount,
                    DriverAmount = order.DriverAmount,
                    FeeAmount= order.FeeAmount,
                    SalesPayedAmount = order.SalesPayedAmount,
                    TradePayedAmount = order.TradePayedAmount,
                });
            }


            if (Div.SelectedIndex == 0)
            {
                foreach (var group in T.GroupBy(c => new { c.Id, c.CarNo, c.CarYear, c.MobileNo, c.Date }).OrderBy(c => c.Key.Date).OrderBy(c => c.Key.CarNo))
                {
                    ModelDataSource.Add(new Account05Model
                    {
                        Id = group.Key.Id,
                        CarNo = group.Key.CarNo,
                        CarYear = group.Key.CarYear,
                        MobileNo = group.Key.MobileNo,
                        Date = group.Key.Date,
                        OrderCount = group.Sum(c => c.OrderCount),
                        IssuedAmount = group.Sum(c => c.IssuedAmount),
                        DriverAmount = group.Sum(c => c.DriverAmount),
                        FeeAmount = group.Sum(c => c.FeeAmount),
                        SalesPayedAmount = group.Sum(c => c.SalesPayedAmount),
                        TradePayedAmount = group.Sum(c => c.TradePayedAmount),
                        IsDay = true,
                        FeeRate = group.Sum(c => c.IssuedAmount) == 0?0d:(((double)group.Sum(c => c.FeeAmount)) / ((double)group.Sum(c => c.IssuedAmount))) * 100d,
                    });
                }
            }

            if (Div.SelectedIndex == 1)
            {
                foreach (var group in T.GroupBy(c => new { c.Id, c.CarNo, c.CarYear, c.MobileNo, Date = new DateTime( c.Date.Year, c.Date.Month, 1) }).OrderBy(c => c.Key.Date).OrderBy(c => c.Key.CarNo))
                {
                    ModelDataSource.Add(new Account05Model
                    {
                        Id = group.Key.Id,
                        CarNo = group.Key.CarNo,
                        CarYear = group.Key.CarYear,
                        MobileNo = group.Key.MobileNo,
                        Date = group.Key.Date,
                        OrderCount = group.Sum(c => c.OrderCount),
                        IssuedAmount = group.Sum(c => c.IssuedAmount),
                        DriverAmount = group.Sum(c => c.DriverAmount),
                        FeeAmount = group.Sum(c => c.FeeAmount),
                        SalesPayedAmount = group.Sum(c => c.SalesPayedAmount),
                        TradePayedAmount = group.Sum(c => c.TradePayedAmount),
                        IsMonth = true,
                        FeeRate = group.Sum(c => c.IssuedAmount) == 0 ? 0d : (((double)group.Sum(c => c.FeeAmount)) / ((double)group.Sum(c => c.IssuedAmount))) * 100d,
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
            DriverId = 0;
            if (e.KeyChar == 13 && Input.Text.Replace(" ", "").Length > 1)
            {
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
            if(e.ColumnIndex == ColumnOrderDate.Index)
            {
                var Model = ModelDataGrid.Rows[e.RowIndex].DataBoundItem as Account05Model;
                if (Model.IsMonth)
                {
                    e.Value = ((DateTime)e.Value).ToString("yyyy-MM");
                }
            }
        }
    }
    class Account05Model
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String CarNo { get; set; }
        public String CarYear { get; set; }
        public String MobileNo { get; set; }
        public int OrderCount { get; set; }
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
