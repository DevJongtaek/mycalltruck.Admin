using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using System.IO;
using OfficeOpenXml;
using System.Diagnostics;
using mycalltruck.Admin.CMDataSetTableAdapters;
using mycalltruck.Admin.UI;
using OfficeOpenXml.Style;

namespace mycalltruck.Admin
{
    public partial class UCNSTATS3 : UserControl
    {
        public UCNSTATS3()
        {
            InitializeComponent();

            dtp_Sdate.ValueChanged += (c, d) =>
            {
                cmbsChange();
            };
            dtp_Edate.ValueChanged += (c, d) =>
            {
                cmbsChange();
            };
        }

        private void UCNSTATS1_Load(object sender, EventArgs e)
        {
            //dtp_Sdate.Value = DateTime.Now;
            //dtp_Edate.Value = DateTime.Now;
            InitCmb();
            //LoadTable();
        }

        public void InitCmb()
        {
            Dictionary<string, string> Smonth = new Dictionary<string, string>
            {
                { "당일", "당일" },
                { "전일", "전일" },
                { "금주", "금주" },
                { "금월", "금월" },
                { "전월", "전월" },
                { "지정", "지정" }
            };

            cmbSMonth.DataSource = new BindingSource(Smonth, null);
            cmbSMonth.DisplayMember = "Value";
            cmbSMonth.ValueMember = "Key";

            cmbSMonth.SelectedIndex = 4;
             



        }
        private String GetSelectCommand()
        {

            return @" SELECT a.CreateTime,a.ReferralId,a.SangHo,a.PayLocation,a.PayLocationName,a.StartName,a.StopName,a.Item,a.StartPrice,a.StopPrice,a.DriverPrice,a.Insu,a.PayDate,a.ClientName,a.OrderStatus,a.DriverCarNo,a.AlterPrice,a.ReferralAccountYN,a.Customer,ClientId,OrderId,ReferralAccountId ,SugumCheck,Driver FROM (SELECT
                        Orders.CreateTime
                        ,Orders.ReferralId
                        ,Customers.SangHo
                        ,Orders.PayLocation -- 2인것만,배차완료인것만
                        ,StaticOptions.Name as PayLocationName
                        ,Orders.StartName
                        ,Orders.StopName
                        ,Orders.Item
                        ,ISNULL(Orders.StartPrice,0) StartPrice
                        ,ISNULL(Orders.StopPrice,0)StopPrice
                        ,ISNULL(Orders.DriverPrice,0)DriverPrice

                        ,'' as Insu
                        ,'' as PayDate
                        ,case when orders.OrdersLoginId IS NULL THEN  clients.LoginId else OrdersLoginId END as ClientName
                        ,Orders.OrderStatus
                        ,Orders.DriverCarNo
                        ,ISNULL(Orders.AlterPrice,0)AlterPrice 
                        , ISNULL(Orders.ReferralAccountYN,'N')  as ReferralAccountYN
                        ,Orders.ClientId
                        ,ISNULL(Orders.Customer,N'') as Customer
                        , Orders.OrderId
                        , orders.ReferralAccountId
                        , Orders.SugumCheck
                        ,Orders.Driver
                        FROM
                        Orders
                         JOIN Customers

                        ON Orders.ReferralId = Customers.CustomerId
                        JOIN StaticOptions ON Orders.PayLocation = StaticOptions.Value AND StaticOptions.Div = 'PayLocation' AND Orders.PayLocation  in (2 ,4)
                        JOIN Clients ON Orders.ClientId = Clients.ClientId
                        ) A";


   //         return @"SELECT a.CreateTime,a.ReferralId,a.SangHo,a.PayLocation,a.PayLocationName,a.StopName,a.StopName,a.Item,a.StartPrice,a.StopPrice,a.DriverPrice,a.Insu,a.PayDate,a.ClientName,a.OrderStatus,a.DriverCarNo,a.AlterPrice,a.ReferralAccountYN FROM (
   //           SELECT
   //             Orders.CreateTime
   //             ,Orders.ReferralId
   //             ,Customers.SangHo
   //             ,Orders.PayLocation -- 2인것만,배차완료인것만
   //             ,StaticOptions.Name as PayLocationName
   //             ,Orders.StartName
   //             ,Orders.StopName
   //             ,Orders.Item
   //             ,ISNULL(Orders.StartPrice,0) StartPrice
   //             ,ISNULL(Orders.StopPrice,0)StopPrice
   //             ,ISNULL(Orders.DriverPrice,0)DriverPrice
               
   //             ,'' as Insu
   //             ,'' as PayDate
   //             ,'' as ClientName
   //             ,Orders.OrderStatus
   //             ,Orders.DriverCarNo
   //             ,ISNULL(Orders.AlterPrice,0)AlterPrice 
   //             , ISNULL(Orders.ReferralAccountYN,'N')  as ReferralAccountYN
			//	,Orders.ClientId
   //             FROM
   //             Orders
   //              JOIN Customers
				
   //             ON Orders.ReferralId = Customers.CustomerId
   //             JOIN StaticOptions ON Orders.PayLocation = StaticOptions.Value AND StaticOptions.Div = 'PayLocation' AND Orders.PayLocation  in (2 ,4)
				
			
			//union all


			//	SELECT
   //             Orders.CreateTime
   //             ,Orders.ReferralId
   //             ,Customers.SangHo
   //             ,Orders.PayLocation -- 2인것만,배차완료인것만
   //             ,StaticOptions.Name as PayLocationName
   //             ,Orders.StartName
   //             ,Orders.StopName
   //             ,Orders.Item
   //             ,0 StartPrice
   //             ,0 StopPrice
   //             ,CASE WHEN Orders.ReferralId != 0 THEN ISNULL(Orders.DriverPrice,0) ELSE  ISNULL(Orders.AlterPrice,0) END DriverPrice 
               
   //             ,'' as Insu
   //             ,'' as PayDate
   //             ,'' as ClientName
   //             ,Orders.OrderStatus
   //             ,Orders.DriverCarNo
   //             ,ISNULL(Orders.AlterPrice,0)AlterPrice 
   //             , ISNULL(Orders.ReferralAccountYN,'N')  as ReferralAccountYN
			//	,Orders.ClientId
   //             FROM
   //             Orders
   //              JOIN Customers
			
   //             ON Orders.ReferralId = Customers.CustomerId
   //             JOIN StaticOptions ON Orders.PayLocation = StaticOptions.Value AND StaticOptions.Div = 'PayLocation' AND Orders.PayLocation  in(1,4,5)
			//	WHERE MyCarOrder = 1
				
			//	union all

			//	SELECT
   //             Orders.CreateTime
   //             ,Orders.ReferralId
   //             ,Customers.SangHo
   //             ,Orders.PayLocation -- 2인것만,배차완료인것만
   //             ,StaticOptions.Name as PayLocationName
   //             ,Orders.StartName
   //             ,Orders.StopName
   //             ,Orders.Item
   //             ,0 StartPrice
   //             ,0 StopPrice
   //             ,ISNULL(Orders.DriverPrice,0)  DriverPrice 
               
   //             ,'' as Insu
   //             ,'' as PayDate
   //             ,'' as ClientName
   //             ,Orders.OrderStatus
   //             ,Orders.DriverCarNo
   //             ,ISNULL(Orders.AlterPrice,0)AlterPrice 
   //             , ISNULL(Orders.ReferralAccountYN,'N')  as ReferralAccountYN
			//	,Orders.ClientId
   //             FROM
   //             Orders
   //              JOIN Customers
             
   //             ON Orders.ReferralId = Customers.CustomerId
   //             JOIN StaticOptions ON Orders.PayLocation = StaticOptions.Value AND StaticOptions.Div = 'PayLocation' AND Orders.PayLocation in(1,4,5)
			//	WHERE Orders.FOrderId != 0 AND MyCarOrder != 1 and Orders.ReferralId != 0

				
				
   //         )as A";


        }
        private void LoadTable()
        {
            nSTATSDataSet.NSTATS3.Clear();
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                    


                    WhereStringList.Add("CONVERT(VARCHAR(10),CreateTime,111) >= @Sdate AND CONVERT(VARCHAR(10),CreateTime,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate",dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    WhereStringList.Add("OrderStatus = 3");
                    

                    WhereStringList.Add("ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    if (cmbSReferralId.SelectedIndex > 0)
                    {
                        WhereStringList.Add("SangHo = @ReferralName ");
                        _Command.Parameters.AddWithValue("@ReferralName", cmbSReferralId.Text);
                    }
                    if (cmbOrderLogin.SelectedIndex > 0)
                    {
                        WhereStringList.Add("ClientName = @ClientName ");
                        _Command.Parameters.AddWithValue("@ClientName", cmbOrderLogin.SelectedValue);
                    }
                    if (cmbPayState.SelectedIndex > 0)
                    {
                        string _PayState = "";
                        WhereStringList.Add("ReferralAccountYN = @ReferralAccountYN");

                        if(cmbPayState.SelectedIndex == 1)
                        {
                            _PayState = "Y";
                        }
                        else
                        {
                            _PayState = "N";

                        }
                        _Command.Parameters.AddWithValue("@ReferralAccountYN", _PayState);
                    }

                    if (cmbSSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("DriverCarNo Like '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "차주명")
                    {
                        WhereStringList.Add(string.Format("DriverCarNo Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "화주명")
                    {
                        WhereStringList.Add(string.Format("Customer Like  '%{0}%'", txtSText.Text));
                    }
                   

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by CreateTime  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        nSTATSDataSet.NSTATS3.Load(_Reader);

                    }
                }

                
                _Connection.Close();    


            }
            var sdate = dtp_Sdate.Value.Date;
            var edate = dtp_Edate.Value.Date;
            var Query = nSTATSDataSet.NSTATS3.ToArray();

            if (Query.Any())
            {



                lblStartPrice.Text = Query.Sum(c => c.StartPrice).ToString("N0");
                lblStopPrice.Text = Query.Sum(c => c.StopPrice).ToString("N0");
                lblDriverPrice.Text = Query.Sum(c => c.DriverPrice).ToString("N0");
                nstatS6TableAdapter.Fill(nSTATSDataSet.NSTATS6);
               
                if(cmbSReferralId.SelectedIndex  > 0)
                {
                    var Query2 = nSTATSDataSet.NSTATS6.Where(c => c.OrderSdate.Date <= sdate && c.OrderEdate <= edate && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CustomerId == (int)cmbSReferralId.SelectedValue).ToArray().OrderByDescending(c => c.PayDate);

                    lblSugum.Text = Query.Where(c => c.ReferralAccountYN == "Y" && c.ReferralId == (int)cmbSReferralId.SelectedValue).Sum(c => c.DriverPrice).ToString("N0");
                    lblmisu.Text = Query.Where(c => c.ReferralAccountYN != "Y").Sum(c => c.DriverPrice).ToString("N0");

                }
                else
                {
                    //if (cmbSReferralId.SelectedIndex == 0)
                    //{


                        var Query2 = nSTATSDataSet.NSTATS6.Where(c => c.OrderSdate.Date <= sdate && c.OrderEdate <= edate && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray().OrderByDescending(c => c.PayDate);

                        lblSugum.Text = Query.Where(c => c.ReferralAccountYN == "Y").Sum(c => c.DriverPrice).ToString("N0");

                        lblmisu.Text = Query.Where(c => c.ReferralAccountYN != "Y").Sum(c => c.DriverPrice).ToString("N0");


                    //}
                }

               
            }
            else
            {

                lblStartPrice.Text = "0";
                lblStopPrice.Text = "0";
                lblDriverPrice.Text = "0";
                lblSugum.Text = "0";
                lblmisu.Text = "0";

            }
          

        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

      
      

        private void ModelDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS3Row;


            if (ModelDataGrid.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (ModelDataGrid.Rows.Count - e.RowIndex).ToString("N0");
            }
           
            else if (ModelDataGrid.Columns[e.ColumnIndex] == acceptTimeDataGridViewTextBoxColumn)
            {
                e.Value = Selected.CreateTime.ToString("d").Replace("-", "/");
            }

            else if (e.ColumnIndex == clientNameDataGridViewTextBoxColumn.Index)
            {
                var mClientUsesAdapter = new ClientUsersTableAdapter();
                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);

                if (mTable.Any(c => c.LoginId == e.Value.ToString()))
                {
                    e.Value = mTable.Where(c => c.LoginId == e.Value.ToString()).First().Name;
                }
                else
                {
                    e.Value = LocalUser.Instance.LogInInformation.Client.CEO;
                }



                // e.Value = LocalUser.Instance.LogInInformation.Client.CEO;
            }
            else if (e.ColumnIndex == Sugum.Index)
            {
                // this.nstatS6TableAdapter.Fill(this.nSTATSDataSet.NSTATS6);


                var Query = nSTATSDataSet.NSTATS6.Where(c => c.idx == Selected.ReferralAccountId).ToArray();

                // ReferralAccountYN
                //var Query = nSTATSDataSet.NSTATS6.Where(c => c.CustomerId == Selected.ReferralId && c.ClientId==LocalUser.Instance.LogInInformation.ClientId && (c.OrderSdate.Date <= Selected.CreateTime.Date && c.OrderEdate >=Selected.CreateTime.Date )).ToArray();

                if (Selected.ReferralAccountYN == "Y")
                {
                    if (Query.Any())
                    {
                        e.Value = Query.First().PayDate.ToString("d").Replace("-", "/");
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

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMonth.SelectedIndex)
            {
                //당일
                case 0:
                    dtp_Sdate.Value = DateTime.Now;
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전일
                case 1:
                    dtp_Sdate.Value = DateTime.Now.AddDays(-1);
                    dtp_Edate.Value = DateTime.Now.AddDays(-1);
                    break;
                //금주
                case 2:
                    dtp_Sdate.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //금월
                case 3:
                    dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전월
                case 4:
                    dtp_Sdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 5:
                    dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                    dtp_Edate.Value = DateTime.Now;
                    break;
            }
        }

        private void txtSText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            LoadTable();
        }

        public void btn_Inew_Click(object sender, EventArgs e)
        {
            cmbSMonth.SelectedIndex = 4;
            //dtp_Sdate.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            //dtp_Edate.Value = DateTime.Now;
            cmbSReferralId.SelectedIndex = 0;
            cmbPayState.SelectedIndex = 0;
            cmbOrderLogin.SelectedIndex = 0;
            cmbSSearch.SelectedIndex = 0;
            txtSText.Clear();
            btnSearch_Click(null, null);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {

            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;

            var fileString = "위탁사수수료정산내역_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.CustomerOrderList))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var ColumnIndexMap = new Dictionary<int, int>();

            List<string> ColumnHeaderMap = new List<string>();
            var ColumnIndex = 0;
            for (int i = 0; i < ModelDataGrid.ColumnCount; i++)
            {
                if (ModelDataGrid.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < ModelDataGrid.ColumnCount; i++)
            {
                if (ModelDataGrid.Columns[i].Visible)
                {
                    ColumnHeaderMap.Add(ModelDataGrid.Columns[i].HeaderText);
                }

            }


            for (int i = 0; i < ColumnHeaderMap.Count; i++)
            {

                _Sheet.Cells[1, i + 1].Value = ColumnHeaderMap[i];

                _Sheet.Cells[RowIndex, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            }
            for (int i = 0; i < ModelDataGrid.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = ModelDataGrid[ColumnIndexMap[j], i].FormattedValue?.ToString();

                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    _Sheet.Cells[1, j + 1].AutoFitColumns();
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
        //private void ExcelExportBasic()
        //{
        //    DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

        //    if (di.Exists == false)
        //    {

        //        di.Create();
        //    }
        //    var fileString = "매출주선위탁사정산" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
        //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
        //    FileInfo file = new FileInfo(FileName);
        //    if (file.Exists)
        //    {
        //        if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        //        {
        //            return;


        //        }


        //    }

        //    ExcelPackage _Excel = null;
        //    using (MemoryStream _Stream = new MemoryStream(Properties.Resources.매출주선위탁사정산_Blank))
        //    {
        //        _Stream.Seek(0, SeekOrigin.Begin);
        //        _Excel = new ExcelPackage(_Stream);
        //    }
        //    var _Sheet = _Excel.Workbook.Worksheets[1];
        //    var RowIndex = 2;
        //    for (int i = 0; i < ModelDataGrid.RowCount; i++)
        //    {
        //        _Sheet.Cells[RowIndex, 1].Value = ModelDataGrid[rowNUMDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 2].Value = ModelDataGrid[acceptTimeDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 3].Value = ModelDataGrid[sangHoDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 4].Value = ModelDataGrid[payLocationNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 5].Value = ModelDataGrid[DriverCarNo.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 6].Value = ModelDataGrid[startNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 7].Value = ModelDataGrid[stopNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 8].Value = ModelDataGrid[itemDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 9].Value = ModelDataGrid[startPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 10].Value = ModelDataGrid[stopPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 11].Value = ModelDataGrid[driverPriceDataGridViewTextBoxColumn.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 12].Value = ModelDataGrid[Sugum.Index, i].FormattedValue;
        //        _Sheet.Cells[RowIndex, 13].Value = ModelDataGrid[insuDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 14].Value = ModelDataGrid[payDateDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        _Sheet.Cells[RowIndex, 15].Value = ModelDataGrid[clientNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

        //        RowIndex++;
        //    }

        //    try
        //    {
        //        _Excel.SaveAs(file);
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
        //        return;
        //    }
        //    System.Diagnostics.Process.Start(FileName);
        //}

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372111212");
        }

        private void cmbSReferralId_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void cmbsChange()
        {


            var _Sdate = dtp_Sdate.Value.Date;
            var _Edate = dtp_Edate.Value.Date.AddDays(1);


            Dictionary<int, string> DCustomerI = new Dictionary<int, string>();
            Dictionary<string, string> DLoginI = new Dictionary<string, string>();
            DLoginI.Clear();
            DCustomerI.Clear();
            cmbOrderLogin.DataSource = null;
            cmbSReferralId.DataSource = null;

            var cmbCustomerMIdDataSource2 = nSTATSDataSet.NSTATS3.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.CreateTime >= _Sdate && c.CreateTime < _Edate).Select(c => new { c.SangHo, c.ReferralId,c.ClientName }).OrderBy(c => c.ReferralId).ToArray();

          
            DCustomerI.Add(0, "전체");
            DLoginI.Add("0", "배차자전체");
            
            DLoginI.Add(LocalUser.Instance.LogInInformation.Client.LoginId, LocalUser.Instance.LogInInformation.Client.CEO);
            if (cmbCustomerMIdDataSource2.Any())
            {


                foreach (var item in cmbCustomerMIdDataSource2.GroupBy(c => c.ReferralId))
                {


                    DCustomerI.Add((int)item.First().ReferralId, item.First().SangHo);


                }


                foreach (var item in cmbCustomerMIdDataSource2.GroupBy(c => c.ClientName))
                {
                    var mClientUsesAdapter = new ClientUsersTableAdapter();
                    var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);

                    if (mTable.Any(c => c.LoginId == item.First().ClientName))
                    {

                        DLoginI.Add(item.First().ClientName, mTable.Where(c => c.LoginId == item.First().ClientName).First().Name);


                    }
                    else
                    {

                    }





                }

            }
        
            if (DCustomerI.Any())
            {

                cmbSReferralId.DataSource = new BindingSource(DCustomerI, null);
                cmbSReferralId.DisplayMember = "Value";
                cmbSReferralId.ValueMember = "Key";
                cmbSReferralId.SelectedIndex = 0;
            }
            if (DLoginI.Any())
            {

                cmbOrderLogin.DataSource = new BindingSource(DLoginI, null);
                cmbOrderLogin.DisplayMember = "Value";
                cmbOrderLogin.ValueMember = "Key";
                cmbOrderLogin.SelectedIndex = 0;
            }



        }

        private void dtp_Edate_ValueChanged(object sender, EventArgs e)
        {
            cmbsChange();
        }

        private void dtp_Sdate_ValueChanged(object sender, EventArgs e)
        {
            cmbsChange();
        }

        private void 수수료정산취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ModelDataGrid.SelectedCells.Count > 0)
            {
                nSTATS3BindingSource.EndEdit();
                if (nSTATS3BindingSource.Current != null)
                {
                    
                    if (((DataRowView)nSTATS3BindingSource.Current).Row is NSTATSDataSet.NSTATS3Row Selected)
                    {
                        if(Selected.ReferralAccountYN != "Y")
                        {
                            MessageBox.Show("수금이 완료된 건을 선택하세요", "수수료 정산취소", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        nSTATS3BindingSource.EndEdit();
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {

                            if (MessageBox.Show($"위탁사   : {Selected.SangHo}\r\n차량번호 : {Selected.DriverCarNo}\r\n수수료   : {Selected.DriverPrice.ToString("N0")}\r\n\r\n 정말 취소하시겠습니까?", "수수료 정산취소", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {

                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();



                                    cmd.CommandText =
                                    "Update Orders SET ReferralAccountYN = 'N',ReferralAccountId = 0 WHERE Orderid = @OrderId  ";
                                    cmd.Parameters.AddWithValue("@OrderId", Selected.OrderId);
                                    cmd.ExecuteNonQuery();


                                    cmd.CommandText =
                                        "Update ReferralAccount SET Amount = Amount-@Amount,ReferralCount = ReferralCount-1  WHERE idx = @idx";
                                    cmd.Parameters.AddWithValue("@Amount", Selected.DriverPrice);
                                    cmd.Parameters.AddWithValue("@idx", Selected.ReferralAccountId);

                                    cmd.ExecuteNonQuery();

                                    cn.Close();

                                    try
                                    {
                                        MessageBox.Show("취소되었습니다.", "수수료 정산취소", MessageBoxButtons.OK);
                                        //  MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "수수료 ", 1), "세금계산서 삭제 성공");
                                        btnSearch_Click(null, null);
                                    }
                                    catch { }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                            }


                        }
                    }
                }


            }
        }
        int _SOrderId = 0;
        private void ModelDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)ModelDataGrid.Rows[e.RowIndex].DataBoundItem).Row as NSTATSDataSet.NSTATS3Row;
            _SOrderId = Selected.OrderId;




            if (_SOrderId == 0)
                return;

            FrmMDI.LoadForm("FrmMN0301", "Order", _SOrderId);
        }

        public void 수수료확인취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ModelDataGrid.SelectedCells.Count > 0)
            {
                nSTATS3BindingSource.EndEdit();
                if (nSTATS3BindingSource.Current != null)
                {

                    if (((DataRowView)nSTATS3BindingSource.Current).Row is NSTATSDataSet.NSTATS3Row Selected)
                    {
                        //if (Selected.ReferralAccountYN != "Y")
                        //{
                        //    MessageBox.Show("수금이 완료된 건을 선택하세요", "수수료 정산취소", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    return;
                        //}
                        nSTATS3BindingSource.EndEdit();
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {

                            if (MessageBox.Show($"상호   : {Selected.SangHo}\r\n운송일   : {Selected.CreateTime.ToString("yyyy-MM-dd").Replace("-","/")}\r\n차량번호 : {Selected.DriverCarNo}\r\n수수료   : {Selected.DriverPrice.ToString("N0")}\r\n\r\n 위 금액을 확인(취소)하시겠습니까?", "수수료 확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {

                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();


                                    if (Selected.SugumCheck == true)
                                    {
                                        cmd.CommandText =
                                        "Update Orders SET SugumCheck =  0  WHERE Orderid = @OrderId  ";
                                    }
                                    else
                                    {
                                        cmd.CommandText =
                                        "Update Orders SET SugumCheck =  1  WHERE Orderid = @OrderId  ";
                                    }
                                    cmd.Parameters.AddWithValue("@OrderId", Selected.OrderId);
                                    cmd.ExecuteNonQuery();


                                    //cmd.CommandText =
                                    //    "Update ReferralAccount SET Amount = Amount-@Amount  WHERE idx = @idx";
                                    //cmd.Parameters.AddWithValue("@Amount", Selected.DriverPrice);
                                    //cmd.Parameters.AddWithValue("@idx", Selected.ReferralAccountId);

                                    //cmd.ExecuteNonQuery();

                                    cn.Close();

                                    try
                                    {
                                       // MessageBox.Show("확인(취소)되었습니다.", "수수료 확인", MessageBoxButtons.OK);
                                        //  MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "수수료 ", 1), "세금계산서 삭제 성공");
                                        btnSearch_Click(null, null);
                                    }
                                    catch { }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                            }


                        }
                    }
                }


            }
        }
        List<int> OrderIds = new List<int>();
        List<int> OrderIdsNo = new List<int>();
       
        public void 선택건수수료합계ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderIds.Clear();
            OrderIdsNo.Clear();


            ////if (e.RowIndex < 0)
            ////    return;
            int DriverPrice = 0;
            int DriverNoPrice = 0;
            int DriverCount = 0;
            int DriverCountNo = 0;
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (ModelDataGrid.SelectedCells.Count > 0)
            {

                foreach (DataGridViewCell item in ModelDataGrid.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }






                foreach (DataGridViewRow dRow in deleteRows)
                {

                    if (dRow.Cells["ReferralAccountYN"].Value.ToString() == "N")
                    {
                        DriverNoPrice += int.Parse(dRow.Cells["driverPriceDataGridViewTextBoxColumn"].Value.ToString());
                        DriverCountNo++;
                        OrderIds.Add(int.Parse(dRow.Cells["OrderId"].Value.ToString()));
                    }
                    else
                    {
                        DriverPrice += int.Parse(dRow.Cells["driverPriceDataGridViewTextBoxColumn"].Value.ToString());
                        DriverCount++;
                        OrderIdsNo.Add(int.Parse(dRow.Cells["OrderId"].Value.ToString()));
                    }

                }


                MessageBox.Show($"\r\n       수금건수 : {DriverCount.ToString("N0")} 건                      \r\n       수금금액 : {DriverPrice.ToString("N0")} 원\r\n\r\n       미수금건수 : {DriverCountNo.ToString("N0")} 건\r\n       미수금액 : {DriverNoPrice.ToString("N0")} 원\r\n\r\n       합계금액 : {(DriverNoPrice+ DriverPrice).ToString("N0")} 원","선택 건 수수료 합계");
                //lblCount.Text = DriverCount.ToString("N0");
                //lblSSum.Text = DriverPrice.ToString("N0");

            }
        }
    }
}
