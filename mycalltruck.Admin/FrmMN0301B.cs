using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static mycalltruck.Admin.CMDataSet;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301B : Form
    {
        private bool ValidateIgnore = false;
        private bool IsCurrentNull = false;
        bool HasLoaded = false;

        int GridIndex = 0;

        DateTimePicker dtp = new DateTimePicker();
        ContextMenuStrip m = new ContextMenuStrip();


        //Action LoadAction = null;
        //0.용차
        //1.지입
        int OrderBubun = 0;
        public FrmMN0301B()
        {
            InitializeComponent();



            DateFilterBegin.Value = DateTime.Now.Date;
            DateFilterEnd.Value = DateTime.Now.Date;
            // dtpRequestDate.Value = DateTime.Now;
           // SearChOrderBubun.SelectedIndex = 0;
            SearchTextFilter.SelectedIndex = 0;


            SetStaticOptions();
            //SetDriverList();
            SetCustomerList();
            SetDriverList();

            //m.Items.Add("배차일자수정");
            //m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
            //dtp.CloseUp += new EventHandler(dtp_CloseUp);
            //dtp.ValueChanged += new EventHandler(dtp_OnTextChange);
            SearChDateGubun.SelectedIndex = 0;

            DateFilterBegin.ValueChanged += (c, d) =>
            {
                _Search();
            };
            DateFilterEnd.ValueChanged += (c, d) =>
            {
                _Search();
            };

        }

        private void FrmMN0301G_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'orderDataSet.OrderBs' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.orderBsTableAdapter.Fill(this.orderDataSet.OrderBs);


            LocalUser.Instance.LogInInformation.LoadClient();

           // OrderBubun = LocalUser.Instance.LogInInformation.Client.OrderBubun;
            DataList.Controls.Add(dtp);
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.Visible = false;
            _Search();

            HasLoaded = true;




        }
        class CustomerModel
        {
            public int CustomerId { get; set; }
            public string SangHo { get; set; }
            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }
            public int PointMethod { get; set; }
            public int Fee { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }
            public string BizNo { get; set; }
            public string CustomerManager { get; set; }
            public int BizGubun { get; set; }
            public string Ceo { get; set; }
        }
        List<CustomerModel> CustomerModelList = new List<CustomerModel>();
        private void SetCustomerList()
        {
            CustomerModelList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT Customers.CustomerId, Customers.SangHo, Customers.PhoneNo, Customers.PointMethod, Customers.Fee, Customers.AddressState, Customers.AddressCity, Customers.AddressDetail, Customers.BizNo,Customers.MobileNo,ISNULL(CustomerManager.ManagerName,''),BizGubun,ISNULL(Customers.Ceo,N'') FROM Customers " +
                        $" LEFT  JOIN CustomerManager ON Customers.CustomerManagerId = CustomerManager.ManagerId" +

                        $" WHERE Customers.BizNo <> '000-00-00000' AND Customers.ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            CustomerModelList.Add(new CustomerModel
                            {
                                CustomerId = _Reader.GetInt32(0),
                                SangHo = _Reader.GetStringN(1),
                                PhoneNo = _Reader.GetStringN(2),
                                PointMethod = _Reader.GetInt32Z(3),
                                Fee = _Reader.GetInt32Z(4),
                                AddressState = _Reader.GetStringN(5),
                                AddressCity = _Reader.GetStringN(6),
                                AddressDetail = _Reader.GetStringN(7),
                                BizNo = _Reader.GetStringN(8),
                                MobileNo = _Reader.GetStringN(9),
                                CustomerManager = _Reader.GetString(10),
                                BizGubun = _Reader.GetInt32(11),
                                Ceo = _Reader.GetString(12),
                            });
                        }
                    }
                }
                _Connection.Close();
            }
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
                    _Command.CommandText = $"SELECT Distinct Drivers.DriverId, CEO, Name, CarNo, MobileNo, CarType, CarSize, CarYear" +
                        $" FROM Drivers JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId WHERE Drivers.ServiceState != 5 AND DriverInstances.ClientId = {LocalUser.Instance.LogInInformation.ClientId} ";
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

                            });
                        }
                    }
                }
                _Connection.Close();
            }
        }


        private void _SetStaticOption(ComboBox mComboBox, String Div)
        {
            mComboBox.DisplayMember = "Name";
            mComboBox.ValueMember = "Value";
            mComboBox.DataSource = new BindingList<StaticOptionsRow>(SingleDataSet.Instance.StaticOptions.Where(c => c.Div == Div).ToList());
            mComboBox.SelectedIndex = 0;
        }
        private void initCmbCustomer()
        {
            Dictionary<int, string> DCustomer = new Dictionary<int, string>();
            customersNewTableAdapter.Fill(clientDataSet.CustomersNew);
            var CustomerDataSource = clientDataSet.CustomersNew.Where(c => c.BizGubun == 5 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();

            DCustomer.Add(0, "거래처선택");

            foreach (var item in CustomerDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }

            Customer.DataSource = new BindingSource(DCustomer, null);
            Customer.DisplayMember = "Value";
            Customer.ValueMember = "Key";
            Customer.SelectedValue = 0;





        }
        private void SetStaticOptions()
        {

            Dictionary<int, string> DItem = new Dictionary<int, string>();
            orderItemTableAdapter.Fill(orderItemDataSet.OrderItems);
            var OrderItemDataSource = orderItemDataSet.OrderItems.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.ItemName, c.OrderItemId }).OrderBy(c => c.OrderItemId).ToArray();

            DItem.Add(0, "품목선택");

            foreach (var item in OrderItemDataSource)
            {
                DItem.Add(item.OrderItemId, item.ItemName);
            }

            Item.DataSource = new BindingSource(DItem, null);
            Item.DisplayMember = "Value";
            Item.ValueMember = "Key";
            Item.SelectedValue = 0;


            Dictionary<int, String> DriverIdDataSource = new Dictionary<int, string>();
            DriverIdDataSource.Add(0, "선택");
            BaseDataSet.DriversDataTable T = new BaseDataSet.DriversDataTable();
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(T);
            foreach (var driverRow in T.OrderBy(c => c.CarYear))
            {
                //DriverIdDataSource.Add(driverRow.DriverId, $"{driverRow.Name}[{driverRow.CarYear}] ({driverRow.CarNo})");
                DriverIdDataSource.Add(driverRow.DriverId, driverRow.CarYear);
            }
            Driver.DataSource = DriverIdDataSource.ToList();
            Driver.ValueMember = "Key";
            Driver.DisplayMember = "Value";
            Driver.SelectedIndex = 0;


            initCmbCustomer();



        }

        private void _Search()
        {
            ValidateIgnore = true;
            DataList.AutoGenerateColumns = false;
            var Now = DateFilterBegin.Value.Date;

            var Tommorow = DateFilterEnd.Value.AddDays(1).Date;
            List<OrderB> DataSource = new List<OrderB>();

            using (ShareOrderBDataSet ShareOrderBDataSet = new ShareOrderBDataSet())
            {
                DataSource = ShareOrderBDataSet.OrderBs.OrderByDescending(c => c.CreateDate).ToList();

                switch (SearChDateGubun.SelectedIndex)
                {
                    case 0:
                        DataSource = DataSource.Where(c => c.CreateDate >= Now && c.CreateDate < Tommorow).ToList();
                        break;

                    case 1:
                        DataSource = DataSource.Where(c => c.BanDate >= Now && c.BanDate < Tommorow).ToList();

                        break;

                }

                if (!String.IsNullOrEmpty(SearchText.Text))
                {
                    switch (SearchTextFilter.SelectedIndex)
                    {

                        case 1:
                            DataSource = DataSource.Where(c => c.ItemName.Contains(SearchText.Text)).ToList();
                            break;
                        case 2:
                            DataSource = DataSource.Where(c => c.CustomerName.Contains(SearchText.Text)).ToList();
                            break;

                        case 3:
                            DataSource = DataSource.Where(c => c.DriverName.Contains(SearchText.Text)).ToList();
                            break;


                    }
                }


                DataSource = DataSource.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToList();

                orderBsBindingSource.DataSource = DataSource;
            }

            ValidateIgnore = false;


            NewOrderB_Click(null, null);



        }
        bool MethodProcess = false;

      

        private void NewOrderB_Click(object sender, EventArgs e)
        {
            DataList.CurrentCell = null;
            IsCurrentNull = true;
            _NewOrder();
            // Driver.Enabled = true;
        }
        private void _NewOrder()
        {

            Customer.SelectedIndex = 0;
            CustomerRemark.Text = "";

            CreateDate.Value = DateTime.Now;
            BanDate.Value = DateTime.Now;

            Item.SelectedIndex = 0;


            BanPLTNum.Text = "0";
            BanBOXNum.Text = "0";
            Remark.Clear();
            Driver.SelectedIndex = 0;
            DriverCarNo.Text = "";
          

        }
        private void SaveOrderB_Click(object sender, EventArgs e)
        {
            //화주정보
            if (Customer.SelectedIndex == 0)
            {
                MessageBox.Show("화주정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Customer.Focus();
                return;
            }

            //화주정보
            if (Item.SelectedIndex == 0)
            {
                MessageBox.Show("품목명은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Item.Focus();
                return;
            }

            //화주정보
            if (Driver.SelectedIndex == 0)
            {
                MessageBox.Show("기사명은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Driver.Focus();
                return;
            }
            OrderB nOrderB = new OrderB();


            nOrderB.CustomerId = (int)Customer.SelectedValue;
            nOrderB.CustomerName = Customer.Text;
            nOrderB.CustomerRemark = CustomerRemark.Text;

            nOrderB.CreateDate = CreateDate.Value;
            nOrderB.BanDate = BanDate.Value;

            nOrderB.ItemId = (int)Item.SelectedValue;
            nOrderB.ItemName = Item.Text;

            nOrderB.BanPLTNum = int.Parse(BanPLTNum.Text.Replace(",", ""));
            nOrderB.BanBOXNum = int.Parse(BanBOXNum.Text.Replace(",", ""));

            nOrderB.Remark = Remark.Text;
            nOrderB.ClientId = LocalUser.Instance.LogInInformation.ClientId;

            nOrderB.DriverId = (int)Driver.SelectedValue;
            nOrderB.DriverCarNo = DriverCarNo.Text;
            nOrderB.DriverName = Driver.Text;


            using (ShareOrderBDataSet ShareOrderBDataSet = new ShareOrderBDataSet())
            {
                ShareOrderBDataSet.OrderBs.Add(nOrderB);
                ShareOrderBDataSet.SaveChanges();
           
            }



            _Search();
            _NewOrder();
        }

        private void UpOrderB_Click(object sender, EventArgs e)
        {
            if (orderBsBindingSource.Current == null)
            {
                return;
            }
            var Current = orderBsBindingSource.Current as OrderB;
            //화주정보
            if (Customer.SelectedIndex == 0)
            {
                MessageBox.Show("거래처정보는 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Customer.Focus();
                return;
            }

            //화주정보
            if (Item.SelectedIndex == 0)
            {
                MessageBox.Show("품목영은 필수 입력사항입니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Item.Focus();
                return;
            }

           
            using (ShareOrderBDataSet ShareOrderBDataSet = new ShareOrderBDataSet())
            {
                OrderB uOrderB = ShareOrderBDataSet.OrderBs.Find(Current.OrderBid);



                uOrderB.CustomerId = (int)Customer.SelectedValue;
                uOrderB.CustomerName = Customer.Text;
                uOrderB.CustomerRemark = CustomerRemark.Text;

                uOrderB.CreateDate = CreateDate.Value;
                uOrderB.BanDate = BanDate.Value;

                uOrderB.ItemId = (int)Item.SelectedValue;
                uOrderB.ItemName = Item.Text;
                uOrderB.BanPLTNum = int.Parse(BanPLTNum.Text.Replace(",", ""));
                uOrderB.BanBOXNum = int.Parse(BanBOXNum.Text.Replace(",", ""));
                uOrderB.Remark = Remark.Text;

                //nOrderG.CreateTime = DateTime.Now;
                uOrderB.DriverId = (int)Driver.SelectedValue;
                uOrderB.DriverCarNo = DriverCarNo.Text;

                uOrderB.DriverName = Driver.Text;

                ShareOrderBDataSet.Entry(uOrderB).State = System.Data.Entity.EntityState.Modified;
                ShareOrderBDataSet.SaveChanges();


                ShareOrderBDataSet.SaveChanges();
            }

            _Search();

        }

        private void DeleteOrderB_Click(object sender, EventArgs e)
        {
            if (orderBsBindingSource.Current == null)
                return;
            var Current = orderBsBindingSource.Current as OrderB;

            if (MessageBox.Show("삭제 하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "DELETE FROM OrderBs WHERE OrderBid = @OrderId";
                    _Command.Parameters.AddWithValue("@OrderId", Current.OrderBid);
                    _Command.ExecuteNonQuery();
                }


            });
            try
            {
                MessageBox.Show("삭제 완료되었습니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _Search();

            }
            catch
            {

                _Search();

            }

        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = "카드페이_지입_화물_반품_내역" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.카드페이_지입_화물_반품_내역))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var ColumnIndexMap = new Dictionary<int, int>();
            var ColumnIndex = 0;
            for (int i = 0; i < DataList.ColumnCount; i++)
            {
                if (DataList.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }
            for (int i = 0; i < DataList.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = DataList[ColumnIndexMap[j], i].FormattedValue?.ToString();
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
        private void Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void Customer_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Customer.SelectedIndex == 0)
            {
                CustomerRemark.Text = "";
            }
            else
            {
                var Query = CustomerModelList.Where(c => c.CustomerId == (int)Customer.SelectedValue).ToArray();

                if (Query.Any())
                {
                    CustomerRemark.Text =Query.First().BizNo + " / "+ Query.First().AddressState + " " + Query.First().AddressCity + " / " + Query.First().Ceo + " / " + "☎ " + Query.First().PhoneNo;
                }
            }
        }

        private void Number_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
            ((TextBox)sender).SelectAll();
        }


        private void Number_Leave(object sender, EventArgs e)
        {
            var Number = sender as TextBox;
            if (String.IsNullOrEmpty(Number.Text))
            {

                Number.Text = "0";

            }
            else
            {

                Number.Text = int.Parse(Number.Text.Replace(",", "")).ToString("N0");

            }
           
        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
        private void Number_MouseClick(object sender, MouseEventArgs e)
        {
            Number_Enter(sender, EventArgs.Empty);
        }


        private void DataList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (IsCurrentNull)
            {
                orderBsBindingSource_CurrentChanged_1(null, null);

            }
        }

        private void DataList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < orderBsBindingSource.Count)
            {
                var Current = orderBsBindingSource[e.RowIndex] as OrderB;

                if (e.ColumnIndex == ColumnNum.Index)
                {
                    DataList[e.ColumnIndex, e.RowIndex].Value = (DataList.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
                }


                if (e.ColumnIndex == orderBidDataGridViewTextBoxColumn.Index)
                {
                    e.Value = Current.OrderBid.ToString("00000000");
                }
                else if (e.ColumnIndex == createDateDataGridViewTextBoxColumn.Index)
                {
                   
                    e.Value = Current.CreateDate.ToString("d").Replace("-", "/");
                }
                else if (e.ColumnIndex == banDateDataGridViewTextBoxColumn.Index)
                {
                   
                    e.Value = Current.BanDate.ToString("d").Replace("-", "/");
                }
                else if (e.ColumnIndex == banPLTNumDataGridViewTextBoxColumn.Index)
                {
                   
                    e.Value = Current.BanPLTNum.ToString("N0").Replace("-", "/");
                }
                else if (e.ColumnIndex == banBOXNumDataGridViewTextBoxColumn.Index)
                {
                   
                    e.Value = Current.BanBOXNum.ToString("N0").Replace("-", "/");
                }

                else if (e.ColumnIndex == CustomerPhoneNoColumns.Index)
                {
                    var Query = CustomerModelList.Where(c => c.CustomerId == Current.CustomerId).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().PhoneNo;
                    }
                }
                else if (e.ColumnIndex == DriverNameColumns.Index)
                {
                    var Query = DriverModelList.Where(c => c.DriverId == Current.DriverId).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().CarYear;
                    }
                }
                
                else if (e.ColumnIndex == DriverPhoneNoColumns.Index)
                {
                    var Query = DriverModelList.Where(c => c.DriverId == Current.DriverId).ToArray();
                    if (Query.Any())
                    {
                        e.Value = Query.First().MobileNo;
                    }
                }

            }
        }

    

        private void SearchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                _Search();
            }
        }

        private void orderBsBindingSource_CurrentChanged_1(object sender, EventArgs e)
        {
            MethodProcess = true;
            _NewOrder();
            if (orderBsBindingSource.Current != null)
            {
                var Current = orderBsBindingSource.Current as OrderB;
                ModelToView(Current);

                if (DataList.RowCount > 0)
                {
                    GridIndex = orderBsBindingSource.Position;
                  
                }

                IsCurrentNull = false;


            }
            MethodProcess = false;
        }
        private void ModelToView(OrderB Current)
        {
            try
            {
                Customer.SelectedValue = Current.CustomerId;
                CustomerRemark.Text = Current.CustomerRemark;

                CreateDate.Value = Current.CreateDate;
                BanDate.Value = Current.BanDate;

                Item.SelectedValue = Current.ItemId;
                BanPLTNum.Text = Current.BanPLTNum.ToString("N0");
                BanBOXNum.Text = Current.BanBOXNum.ToString("N0");
                Remark.Text = Current.Remark;

                Driver.SelectedValue = Current.DriverId;
                DriverCarNo.Text = Current.DriverCarNo;
            }
            catch
            {

            }
        }
        private void rdb_Ban_CheckedChanged(object sender, EventArgs e)
        {

            if (rdb_Car.Checked)
            {

                FrmMDI frmMDI = new FrmMDI();

                //FrmMN0301Default _Form = new FrmMN0301Default();
                FrmMN0301Default _Form = new FrmMN0301Default();
                _Form.MdiParent = this.MdiParent;

                _Form.Show();

                this.Close();


            }
            //else if (rdo_Defalut.Checked)
            //{
            //    FrmMDI frmMDI = new FrmMDI();

            //    FrmMN0301 _Form = new FrmMN0301();
            //    _Form.MdiParent = this.MdiParent;

            //    _Form.Show();

            //    this.Close();
            //}
            else if (rdb_Client.Checked)
            {
                FrmMDI frmMDI = new FrmMDI();

                FrmMN0301G _Form = new FrmMN0301G();
                _Form.MdiParent = this.MdiParent;

                _Form.Show();

                this.Close();
            }
        }

        private void Driver_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Query = DriverModelList.Where(c => c.DriverId == (int)Driver.SelectedValue).ToArray();

            if (Query.Any())
            {
                var SizeName = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == Query.First().CarSize && c.Div == "CarSize").Select(c => new { c.Name }).ToArray();



                var TypeName = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == Query.First().CarType && c.Div == "CarType" && c.Value == Query.First().CarType).Select(c => new { c.Name }).ToArray();


                DriverCarNo.Text = Query.First().CarNo ;
            }
        }
    }
}
