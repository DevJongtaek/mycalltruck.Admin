using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.IO;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class;
using System.Data.SqlClient;
using OfficeOpenXml;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301Add : Form
    {
        public FrmMN0301Add()
        {
            InitializeComponent();

            _InitCmb();
        }
        private void FrmMN0208_DRIVERADDMANAGE_ADD_Load(object sender, EventArgs e)
        {



            cmbAccGubun.SelectedIndex = 0;
            cmbCustomer.SelectedIndex = 0;
            cmbDriverCarNo.SelectedIndex = 0;
            txtDriverName.Text = "";
            dtpStartDate.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtpStopDate.Value = DateTime.Now;
            dtpCreateDate.Value = DateTime.Now;
            AcceptTime.Value = DateTime.Now;
            txtSalesPrice.Text = "0";
            txtTradePrice.Text = "0";


        }
        private void _InitCmb()
        {
            Dictionary<int, string> AccGubun = new Dictionary<int, string>();
            var AccGubunDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccGubun" && c.Value != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            AccGubun.Add(0, "선택");

            foreach (var item in AccGubunDataSource.OrderBy(c => c.Seq))
            {
                AccGubun.Add(item.Value, item.Name);
            }


            cmbAccGubun.DataSource = new BindingSource(AccGubun, null);
            cmbAccGubun.DisplayMember = "Value";
            cmbAccGubun.ValueMember = "Key";
            cmbAccGubun.SelectedIndex = 0;

            Dictionary<int, string> DCustomer = new Dictionary<int, string>();
            customersNewTableAdapter.Fill(clientDataSet.CustomersNew);
            var CustomerDataSource = clientDataSet.CustomersNew.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.CustomerId).ToArray();


            DCustomer.Add(0, "선택");

            foreach (var item in CustomerDataSource.OrderBy(c=> c.SangHo))
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }


            cmbCustomer.DataSource = new BindingSource(DCustomer, null);
            cmbCustomer.DisplayMember = "Value";
            cmbCustomer.ValueMember = "Key";
            cmbCustomer.SelectedIndex = 0;


           

            Dictionary<int, String> DriverIdDataSource = new Dictionary<int, string>();
            DriverIdDataSource.Add(0, "선택");
            BaseDataSet.DriversDataTable T = new BaseDataSet.DriversDataTable();
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(T);
            foreach (var driverRow in T.OrderBy(c=> c.CarNo))
            {
                //DriverIdDataSource.Add(driverRow.DriverId, $"{driverRow.Name}[{driverRow.CarYear}] ({driverRow.CarNo})");
                DriverIdDataSource.Add(driverRow.DriverId, driverRow.CarNo);
            }
            cmbDriverCarNo.DataSource = DriverIdDataSource.ToList();
            cmbDriverCarNo.ValueMember = "Key";
            cmbDriverCarNo.DisplayMember = "Value";
            cmbDriverCarNo.SelectedIndex = 0;





        }
     
        public bool IsSuccess = false;
       

        private int _UpdateDB()
        {
            err.Clear();

            if (cmbAccGubun.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmbAccGubun, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }


            else if (cmbCustomer.SelectedIndex == 0 && cmbDriverCarNo.SelectedIndex == 0)
            {
                MessageBox.Show("화주명,차량번호 둘중 한가지는 입력해야 합니다.", "필수항목누락");
                
                err.SetError(cmbCustomer, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }

            if (cmbDriverCarNo.SelectedIndex != 0)
            {
                var _d = 0m;
                if (!decimal.TryParse(txtTradePrice.Text.Replace(",", ""), out _d))
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txtTradePrice, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;

                }
                else
                {
                    decimal _Pirce = Convert.ToDecimal(txtTradePrice.Text.Replace(",", ""));
                    if (_Pirce > 1000000)
                    {
                        MessageBox.Show("1,000,000 원 까지만 입력 가능합니다.", "매입관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        err.SetError(txtTradePrice, "1,000,000 원 까지만 입력 가능합니다.");
                        return -1;
                    }

                }
            }
            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
             

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
           {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        private void _AddClient()
        {
            BaseDataSet.DriversDataTable T = new BaseDataSet.DriversDataTable();
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(T);

            int _CarSize = 1;
            int _CarType = 2;
           
            var _Driver = T.Where(c => c.CarNo == cmbDriverCarNo.Text).FirstOrDefault();

            if(cmbDriverCarNo.SelectedIndex != 0)
            {
                _CarSize = (int)_Driver.CarSize;
                _CarType = (int)_Driver.CarType;


            }

            string _OrderPhoneNo = "";
            string _CustomerName = "";
            var _Customer = clientDataSet.CustomersNew.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

            if (cmbCustomer.SelectedIndex != 0)
            {
                _OrderPhoneNo = _Customer.Where(c=> c.CustomerId ==(int)cmbCustomer.SelectedValue).First().PhoneNo;
            }
            if(cmbCustomer.SelectedIndex == 0)
            {
                _CustomerName = "";
            }
            else
            {
                _CustomerName = cmbCustomer.Text;
            }
            Order nOrder = new Order
            {
                CreateTime = Convert.ToDateTime(dtpCreateDate.Value.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")),

                ClientId = LocalUser.Instance.LogInInformation.ClientId,
                StartState = " ",
                StartCity = " ",
                StartStreet = "",
                StartDetail = "",
                StopState = " ",
                StopCity = " ",
                StopStreet = "",
                StopDetail = "",
                CarSize = _CarSize,
                CarType = _CarType,
                StopDateHelper = 0,
                CarCount = 1,
                PayLocation = 5,
                Item = cmbAccGubun.Text,
                Remark = cmbAccGubun.Text,
                ItemSize = "0",
                ItemSizeInclude = false,
                IsShared = false,
                SharedItemLength = 0,
                SharedItemSize = 0,
                Emergency = false,
                Round = false,
                Reservation = false,
                Customer = _CustomerName,
                OrderPhoneNo = _OrderPhoneNo,
                StopPhoneNo = "",
                StartPhoneNo = "",
                StartDate = DateTime.Now.Date,
                StartTime = DateTime.Now.Date,
                StopTime = DateTime.Now.Date,


                StartName = "",
                StopName = "",
                StartMemo = "",
                StopMemo = "",
                RequestMemo = "",

                ReferralId = 0,
                MyCarOrder = false,
            };
            nOrder.TradePrice = 0;
            nOrder.SalesPrice = 0;
            nOrder.AlterPrice = 0;
            nOrder.StartPrice = 0;
            nOrder.StopPrice = 0;
            nOrder.DriverPrice = 0;
            nOrder.Price = 0;
            nOrder.ClientPrice = 0;

           

            nOrder.TradePrice = int.Parse(txtTradePrice.Text.Replace(",", ""));
            nOrder.SalesPrice = int.Parse(txtSalesPrice.Text.Replace(",", ""));
            nOrder.AlterPrice = 0;
            nOrder.Price = nOrder.TradePrice.Value;
            nOrder.ClientPrice = nOrder.SalesPrice;
            nOrder.StopMulti = false;
            nOrder.StartMulti = false;
            nOrder.OrderClientId = LocalUser.Instance.LogInInformation.ClientId;
            nOrder.OrdersAcceptId = LocalUser.Instance.LogInInformation.LoginId;
            //nOrder.StartInfo


            //string StartTime = "";
            //if (StartTimeType1.Checked)
            //{
            //    nOrder.StartTimeType = 1;
            //    nOrder.StartDate = DateTime.Now.Date;
            //    nOrder.StartTime = DateTime.Now;
            //    StartTime = "지금상/";
            //}
            //else if (StartTimeType2.Checked)
            //{
            //    nOrder.StartTimeType = 2;
            //    nOrder.StartDate = DateTime.Now.Date;
            //    nOrder.StartTime = DateTime.Now.Date;
            //    if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
            //    {
            //        nOrder.StartTimeHour = StartTimeHour2.SelectedIndex + (StartTimeHour1.SelectedIndex - 1) * 12;
            //        nOrder.StartTime = nOrder.StartTime.AddHours(nOrder.StartTimeHour.Value);
            //        nOrder.StartTimeHalf = StartTimeHalf.Checked;
            //        if (StartTimeHalf.Checked)
            //        {
            //            nOrder.StartTime = nOrder.StartTime.AddMinutes(30);
            //        }
            //    }
            //    StartTime = "당상/";
            //}
            //else if (StartTimeType3.Checked)
            //{
            //    nOrder.StartTimeType = 3;
            //    nOrder.StartDate = DateTime.Now.Date;
            //    nOrder.StartTime = DateTime.Now.Date;
            //    if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
            //    {
            //        nOrder.StartTimeHour = StartTimeHour2.SelectedIndex + (StartTimeHour1.SelectedIndex - 1) * 12;
            //        nOrder.StartTime = nOrder.StartTime.AddHours(nOrder.StartTimeHour.Value);
            //        nOrder.StartTimeHalf = StartTimeHalf.Checked;
            //        if (StartTimeHalf.Checked)
            //        {
            //            nOrder.StartTime = nOrder.StartTime.AddMinutes(30);
            //        }
            //    }
            //    StartTime = "내상/";
            //}
            //else if (!String.IsNullOrEmpty(StartTimeType4.Text))
            //{
            //    nOrder.StartTimeType = 1000 + int.Parse(StartTimeType4.Text);
            //    nOrder.StartDate = DateTime.Now.Date;
            //    nOrder.StartTime = DateTime.Now.Date;
            //    if (StartTimeHour1.SelectedIndex > 0 && StartTimeHour2.SelectedIndex > 0)
            //    {
            //        nOrder.StartTimeHour = StartTimeHour2.SelectedIndex + (StartTimeHour1.SelectedIndex - 1) * 12;
            //        nOrder.StartTime = nOrder.StartTime.AddHours(nOrder.StartTimeHour.Value);
            //        nOrder.StartTimeHalf = StartTimeHalf.Checked;
            //        if (StartTimeHalf.Checked)
            //        {
            //            nOrder.StartTime = nOrder.StartTime.AddMinutes(30);
            //        }
            //    }
            //    StartTime = $"{StartTimeType4.Text}일상/";
            //}
            //if (StopInfo1.Checked)
            //    nOrder.StopInfo = StopInfo1.Text;
            //else if (StopInfo2.Checked)
            //    nOrder.StopInfo = StopInfo2.Text;
            //else if (StopInfo3.Checked)
            //    nOrder.StopInfo = StopInfo3.Text;
            //else if (StopInfo4.Checked)
            //    nOrder.StopInfo = StopInfo4.Text;
            //else if (StopInfo5.Checked)
            //    nOrder.StopInfo = StopInfo5.Text;
            //string StopTime = "";
            //if (StopTimeType1.Checked)
            //{
            //    nOrder.StopTimeType = 1;
            //    nOrder.StopTime = DateTime.Now.Date;
            //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
            //    {
            //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
            //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
            //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
            //        if (StopTimeHalf.Checked)
            //        {
            //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
            //        }
            //    }
            //    StopTime = "당착";
            //}
            //else if (StopTimeType2.Checked)
            //{
            //    nOrder.StopTimeType = 2;
            //    nOrder.StopTime = DateTime.Now.Date;
            //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
            //    {
            //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
            //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
            //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
            //        if (StopTimeHalf.Checked)
            //        {
            //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
            //        }
            //    }
            //    StopTime = "내착";
            //}
            //else if (StopTimeType3.Checked)
            //{
            //    nOrder.StopTimeType = 3;
            //    nOrder.StopTime = DateTime.Now.Date;
            //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
            //    {
            //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
            //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
            //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
            //        if (StopTimeHalf.Checked)
            //        {
            //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
            //        }
            //    }
            //    StopTime = "월착";
            //}
            //else if (StopTimeType4.Checked)
            //{
            //    nOrder.StopTimeType = 4;
            //    nOrder.StopTime = DateTime.Now.Date;
            //    StopTime = "당착/내착";
            //}
            //else if (!String.IsNullOrEmpty(StopTimeType5.Text))
            //{
            //    nOrder.StopTimeType = 1000 + int.Parse(StopTimeType5.Text);
            //    nOrder.StopTime = DateTime.Now.Date;
            //    if (StopTimeHour1.SelectedIndex > 0 && StopTimeHour2.SelectedIndex > 0)
            //    {
            //        nOrder.StopTimeHour = StopTimeHour2.SelectedIndex + (StopTimeHour1.SelectedIndex - 1) * 12;
            //        nOrder.StopTime = nOrder.StopTime.AddHours(nOrder.StopTimeHour.Value);
            //        nOrder.StopTimeHalf = StopTimeHalf.Checked;
            //        if (StopTimeHalf.Checked)
            //        {
            //            nOrder.StopTime = nOrder.StopTime.AddMinutes(30);
            //        }
            //    }
            //    StopTime = $"{StopTimeType5.Text}일착";
            //}
            if (String.IsNullOrEmpty(nOrder.OrderPhoneNo))
            {

                nOrder.OrderPhoneNo = "";
            }
            if (cmbCustomer.SelectedIndex != 0)
            {
                nOrder.CustomerId = (int)cmbCustomer.SelectedValue;
            }
            if (cmbDriverCarNo.SelectedIndex != 0)
            {
                nOrder.DriverId = _Driver.DriverId;
                nOrder.Driver = _Driver.CarYear;
                nOrder.DriverCarModel = _Driver.Name;
                nOrder.DriverCarNo = _Driver.CarNo;
                nOrder.DriverPhoneNo = _Driver.MobileNo;
                
            }
            nOrder.OrderStatus = 3;
            nOrder.AcceptTime = AcceptTime.Value.Date;
            nOrder.Wgubun = "PC";

            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                try
                {


                    ShareOrderDataSet.Orders.Add(nOrder);
                    ShareOrderDataSet.SaveChanges();
                }

                catch (Exception ex)
                {

                }
            }
            //if (nOrder.CustomerId == null && !String.IsNullOrEmpty(nOrder.OrderPhoneNo) && !String.IsNullOrEmpty(nOrder.Customer))
            //{
            //    Data.Connection(_Connection =>
            //    {
            //        using (SqlCommand _Command = _Connection.CreateCommand())
            //        {
            //            _Command.CommandText = "SELECT ContractId FROM Contracts WHERE PhoneNo = @PhoneNo";
            //            _Command.Parameters.AddWithValue("@PhoneNo", nOrder.OrderPhoneNo);
            //            if (_Command.ExecuteScalar() != null)
            //                return;
            //        }
            //        using (SqlCommand _Command = _Connection.CreateCommand())
            //        {
            //            _Command.CommandText = "INSERT INTO Contracts (ClientId, PhoneNo, Name) VALUES (@ClientId, @PhoneNo, @Name)";
            //            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
            //            _Command.Parameters.AddWithValue("@PhoneNo", nOrder.OrderPhoneNo);
            //            _Command.Parameters.AddWithValue("@Name", nOrder.Customer);
            //            _Command.ExecuteNonQuery();
            //        }
            //    });
            //}
            //직배송

            //if (cmbDriverCarNo.SelectedIndex != 0)
            //{
            //    nOrder.DriverId = _Driver.DriverId;
            //    nOrder.Driver = _Driver.CarYear;
            //    nOrder.DriverCarModel = _Driver.Name;
            //    nOrder.DriverCarNo = _Driver.CarNo;
            //    nOrder.DriverPhoneNo = _Driver.MobileNo;
            //    nOrder.OrderStatus = 3;
            //}
          
            //nOrder.AcceptTime = DateTime.Now;
            //nOrder.Wgubun = "PC";
           
            //using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            //{
            //    ShareOrderDataSet.Entry(nOrder).State = System.Data.Entity.EntityState.Modified;
            //    ShareOrderDataSet.SaveChanges();
            //}
            //if (LocalUser.Instance.LogInInformation.Client.AllowSMS)
            //{

            //}



            try
            {
               
            }
            catch
            {
                MessageBox.Show("화주담당자 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "지입배차", 1), "지입배차 추가 성공");

                cmbAccGubun.SelectedIndex = 0;
                cmbCustomer.SelectedIndex = 0;
                cmbDriverCarNo.SelectedIndex = 0;
                txtDriverName.Text = "";
                dtpStartDate.Value = DateTime.Now.AddMonths(-1).AddDays(1);
                dtpStopDate.Value = DateTime.Now;
                dtpCreateDate.Value = DateTime.Now;
                AcceptTime.Value = DateTime.Now;
                txtSalesPrice.Text = "0";
                txtTradePrice.Text = "0";



            }

            cmbAccGubun.Focus();

           
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "지입배차 추가", 1), "지입배차 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void btnExcelImport_Click(object sender, EventArgs e)
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

        private void btn_DriverExcel_Click(object sender, EventArgs e)
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
                string fileString = "배차내역_일괄등록_표준_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                byte[] ieExcel = Properties.Resources.Order_Default;
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
            else if (iExcelType == 4)
            {
                MessageBox.Show("화주 업체에서 부터 양식을 다운로드 하십시오.", Text);
                return;
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
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void cmbDriverCarNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbDriverCarNo.SelectedIndex != 0)
            {
                BaseDataSet.DriversDataTable T = new BaseDataSet.DriversDataTable();
                DriverRepository mDriverRepository = new DriverRepository();
                mDriverRepository.Select(T);

                var Query = T.Where(c => c.CarNo == cmbDriverCarNo.Text).FirstOrDefault();
                
                txtDriverName.Text = Query.CarYear;

                txtTradePrice.Enabled = true;
            }
            else
            {
                txtTradePrice.Enabled = false;
            }
        }

        private void btnExcelImportNew_Click(object sender, EventArgs e)
        {
            EXCELINSERT5 _Form = new EXCELINSERT5();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
        }

        private void btn_DriverExcelNew_Click(object sender, EventArgs e)
        {
            string fileString = "배차내역_일괄등록_간편_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            byte[] ieExcel = Properties.Resources.Order_Mini;
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

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbCustomer.SelectedIndex == 0)
            {
                txtSalesPrice.Enabled = false;
            }
            else
            {
                txtSalesPrice.Enabled = true;
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EXCELINSERT6 _Form = new EXCELINSERT6();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
        }

        private void btnTax_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.No;
        }

        public void Button_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender as Control != null)
                {
                    Button pnShortCut = sender as Control as Button;
                    if (pnShortCut.Enabled)
                    {
                        pnShortCut.BackColor = Color.Green;
                    }
                }
            }
            catch { }
        }
        

        public void Button_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (sender as Control != null)
                {
                    Button pnShortCut = sender as Control as Button;
                    // pnShortCut.Invalidate();
                    pnShortCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
                }
            }
            catch { }

        }
    }
}
