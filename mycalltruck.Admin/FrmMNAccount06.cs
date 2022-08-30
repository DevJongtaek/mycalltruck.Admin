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
    public partial class FrmMNAccount06 : Form
    {
        int DriverId = 0;
        int CustomerId = 0;

        ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet();

        public FrmMNAccount06()
        {
            InitializeComponent();
            SetMouseClick(this);
        }

        private void FrmMNSTATS1_Load(object sender, EventArgs e)
        {
            Year.Value = DateTime.Now.Year;
            Month.SelectedIndex = DateTime.Now.Month - 1;
            var Date = new DateTime((int)Year.Value, Month.SelectedIndex + 1, 1);
            ColumnMisu1.HeaderText = Date.Month.ToString("00") + "월";
            ColumnMisu2.HeaderText = Date.AddMonths(-1).Month.ToString("00") + "월";
            ColumnMisu3.HeaderText = Date.AddMonths(-2).Month.ToString("00") + "월";
            ColumnMisu4.HeaderText = Date.AddMonths(-3).Month.ToString("00") + "월";
            ColumnMisu5.HeaderText = Date.AddMonths(-4).Month.ToString("00") + "월";
            ColumnMisu6.HeaderText = Date.AddMonths(-5).Month.ToString("00") + "월";
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
            var Date = new DateTime((int)Year.Value, Month.SelectedIndex + 1, 1);
            ColumnMisu1.HeaderText = Date.Month.ToString("00") + "월";
            ColumnMisu2.HeaderText = Date.AddMonths(-1).Month.ToString("00") + "월";
            ColumnMisu3.HeaderText = Date.AddMonths(-2).Month.ToString("00") + "월";
            ColumnMisu4.HeaderText = Date.AddMonths(-3).Month.ToString("00") + "월";
            ColumnMisu5.HeaderText = Date.AddMonths(-4).Month.ToString("00") + "월";
            ColumnMisu6.HeaderText = Date.AddMonths(-5).Month.ToString("00") + "월";

            var Begin = new DateTime((int)Year.Value, Month.SelectedIndex + 1, 1).AddMonths(-6);
            var End = new DateTime((int)Year.Value, Month.SelectedIndex + 1, 1).AddMonths(1).AddSeconds(-1);

            List<Account06Model> T = new List<Account06Model>();
            BindingList<Account06Model> ModelDataSource = new BindingList<Account06Model>();
            var Query = ShareOrderDataSet.SalesManage.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId &&
                (CustomerId == 0 || c.CustomerId == CustomerId) && c.CustomerModel != null &&
                c.RequestDate >= Begin && c.RequestDate <= End)
                .Select(c => new
                {
                    Id = c.CustomerId.Value,
                    Name = c.CustomerModel.SangHo,
                    Date = c.RequestDate,
                    MisuAmount = c.PayState == 1?c.Amount:0,
                }).ToArray();

            foreach (var order in Query)
            {
                T.Add(new Account06Model
                {
                    Id = order.Id,
                    Date = order.Date.Date,
                    Name = order.Name,
                    MisuAmount = order.MisuAmount,
                });
            }

            var Date1 = new DateTime((int)Year.Value, Month.SelectedIndex + 1, 1);
            var Date2 = Date1.AddMonths(-1);
            var Date3 = Date1.AddMonths(-2);
            var Date4 = Date1.AddMonths(-3);
            var Date5 = Date1.AddMonths(-4);
            var Date6 = Date1.AddMonths(-5);

            foreach (var group in T.GroupBy(c=>new {c.Id, c.Name }).OrderBy(c=>c.Key.Name))
            {
                ModelDataSource.Add(new Account06Model
                {
                    Id = group.Key.Id,
                    Name = group.Key.Name,
                    TotalMisu = group.Sum(c=>c.MisuAmount),
                    Misu1 = group.Where(c=>new DateTime(c.Date.Year, c.Date.Month, 1) == Date1).Sum(c => c.MisuAmount),
                    Misu2 = group.Where(c => new DateTime(c.Date.Year, c.Date.Month, 1) == Date2).Sum(c => c.MisuAmount),
                    Misu3 = group.Where(c => new DateTime(c.Date.Year, c.Date.Month, 1) == Date3).Sum(c => c.MisuAmount),
                    Misu4 = group.Where(c => new DateTime(c.Date.Year, c.Date.Month, 1) == Date4).Sum(c => c.MisuAmount),
                    Misu5 = group.Where(c => new DateTime(c.Date.Year, c.Date.Month, 1) == Date5).Sum(c => c.MisuAmount),
                    Misu6 = group.Where(c => new DateTime(c.Date.Year, c.Date.Month, 1) == Date6).Sum(c => c.MisuAmount),
                });
            }
            ModelDataSource.Add(new Account06Model
            {
                Id = 0,
                Name = $"총 {ModelDataSource.Count}건",
                TotalMisu = ModelDataSource.Sum(c => c.TotalMisu),
                Misu1 = ModelDataSource.Sum(c => c.Misu1),
                Misu2 = ModelDataSource.Sum(c => c.Misu2),
                Misu3 = ModelDataSource.Sum(c => c.Misu3),
                Misu4 = ModelDataSource.Sum(c => c.Misu4),
                Misu5 = ModelDataSource.Sum(c => c.Misu5),
                Misu6 = ModelDataSource.Sum(c => c.Misu6),
            });
            ModelDataGrid.AutoGenerateColumns = false;
            ModelDataGrid.DataSource = ModelDataSource;
            var LastRow = ModelDataGrid.Rows[ModelDataGrid.RowCount - 1];
            LastRow.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.DarkSlateGray,
                SelectionForeColor = Color.White,
                BackColor = Color.DarkSlateGray,
                ForeColor = Color.White,
                Font = new Font(ModelDataGrid.DefaultCellStyle.Font, FontStyle.Bold),
            };
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

        }
    }
    class Account06Model
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public decimal MisuAmount { get; set; }
        public decimal TotalMisu { get; set; }
        public decimal Misu1 { get; set; }
        public decimal Misu2 { get; set; }
        public decimal Misu3 { get; set; }
        public decimal Misu4 { get; set; }
        public decimal Misu5 { get; set; }
        public decimal Misu6 { get; set; }
    }
}
