using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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

namespace mycalltruck.Admin
{
    public partial class FRMMNTAXUSELIST : Form
    {
        int GridIndex = 0;
        int GridIndex2 = 0;

        public FRMMNTAXUSELIST()
        {
            InitClientTable();
            InitializeComponent();
        }


        private void FRMMNUSELIST_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'useListDataSet.DriverTaxList' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.driverTaxListTableAdapter.Fill(this.useListDataSet.DriverTaxList);
            // TODO: 이 코드는 데이터를 'useListDataSet.ClientsTaxList' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.clientsTaxListTableAdapter.Fill(this.useListDataSet.ClientsTaxList);

           

            _InitCmb();
            Fclear();
            Fclear2();
            btn_Search_Click(null, null);
            button3_Click(null, null);
        }
        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
        }
        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();

        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name, LoginId FROM Clients ";
                   
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientTable.Add(
                              new ClientViewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        public void _InitCmb()
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

            cmbSMonth.SelectedIndex = 0;

            cmbSMontDriver.DataSource = new BindingSource(Smonth, null);
            cmbSMontDriver.DisplayMember = "Value";
            cmbSMontDriver.ValueMember = "Key";

            cmbSMontDriver.SelectedIndex = 0;
            



            //Dictionary<string, string> SpayLocation = new Dictionary<string, string>
            //{
            //    { "전체", "전체" },
            //    { "인수증", "인수증" },
            //    { "선/착불", "선/착불" },
            //    { "수수료확인", "수수료확인" }
            //};


            //cmbSPayLocation.DataSource = new BindingSource(SpayLocation, null);
            //cmbSPayLocation.DisplayMember = "Value";
            //cmbSPayLocation.ValueMember = "Key";

            //cmbSPayLocation.SelectedIndex = 0;


            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {
                { "전체", "전체" },
                //{ "배차일시", "배차일시" },
                { "거래처명", "거래처명" },
                { "차량번호", "차량번호" },
                { "기사명", "기사명" },
                { "핸드폰번호", "핸드폰번호" },
                { "화주담당자", "화주담당자" }

            };


            //cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            //cmbSSearch.DisplayMember = "Value";
            //cmbSSearch.ValueMember = "Key";

            //cmbSSearch.SelectedIndex = 0;


        }



        private void Fclear()
        {
            dtp_Sdate.Value = DateTime.Now;
            dtp_Edate.Value = DateTime.Now;
           // cmbClientId.SelectedIndex = 0;
            //txtSearch.Text = "";
            

        }


        private void Fclear2()
        {
            dtp_Sdate2.Value = DateTime.Now;
            dtp_Edate2.Value = DateTime.Now;
            // cmbClientId.SelectedIndex = 0;
            //txtSearch2.Text = "";


        }


        private void btn_Import_Click(object sender, EventArgs e)
        {
            EXCELINSERT_USELIST _Form = new EXCELINSERT_USELIST();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
            btn_Search_Click(null, null);
        }


        private String GetSelectCommand()
        {
            return @"SELECT CDate,Amount,ClientPoints.Clientid,Clients.MobileNo,Clients.Name,Clients.CEO,MethodType,Remark FROM ClientPoints
JOIN Clients ON ClientPoints.ClientId = Clients.ClientId";



        }
        private void LoadTable()
        {
            useListDataSet.ClientsTaxList.Clear();

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                    //if (cmbSReferralId.SelectedIndex > 0)
                    //{
                    //    WhereStringList.Add("Customers.SangHo = @ReferralName ");
                    //    _Command.Parameters.AddWithValue("@ReferralName", cmbSReferralId.Text);
                    //}

                    //if (cmbCustomers.SelectedIndex > 0)
                    //{
                    //    WhereStringList.Add("Orders.CustomerId = @CustomerId ");
                    //    _Command.Parameters.AddWithValue("@CustomerId", cmbCustomers.SelectedValue.ToString());
                    //}


                    //if (cmbSPayLocation.SelectedIndex > 0)
                    //{
                    //    WhereStringList.Add("PayLocation.Name = @PayLocation ");
                    //    _Command.Parameters.AddWithValue("@PayLocation", cmbSPayLocation.Text);
                    //}

                    //if (cmbTeam.SelectedIndex > 0)
                    //{
                    //    WhereStringList.Add("Orders.CustomerTeam = @CustomerTeam ");
                    //    _Command.Parameters.AddWithValue("@CustomerTeam", cmbTeam.SelectedValue.ToString());
                    //}




                    WhereStringList.Add("CONVERT(VARCHAR(10), CDate,111) >= @Sdate AND CONVERT(VARCHAR(10),CDate,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate", dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    WhereStringList.Add("MethodType = '전자 세금계산서'  ");


                    //if (cmbSSearch.Text == "거래처명")
                    //{
                    //    WhereStringList.Add(string.Format("Orders.Customer Like '%{0}%'", txtSText.Text));
                    //}
                    //else if (cmbSSearch.Text == "차량번호")
                    //{
                    //    WhereStringList.Add(string.Format("DriverCarNo Like  '%{0}%'", txtSText.Text));
                    //}
                    //else if (cmbSSearch.Text == "기사명")
                    //{
                    //    WhereStringList.Add(string.Format("Driver Like  '%{0}%'", txtSText.Text));
                    //}
                    //else if (cmbSSearch.Text == "핸드폰번호")
                    //{
                    //    WhereStringList.Add(string.Format("DriverPhoneNo Like  '%{0}%'", txtSText.Text));
                    //}

                    //else if (cmbSSearch.Text == "화주담당자")
                    //{
                    //    WhereStringList.Add(string.Format("CustomerManager Like  '%{0}%'", txtSText.Text));
                    //}






                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }

                    SelectCommandText += " UNION ALL SELECT GETDATE(),sum(-amount),0,'','','','',''  FROM ClientPoints";

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }
                    //  SelectCommandText += " Order by orders.CreateTime  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        useListDataSet.ClientsTaxList.Load(_Reader);


                        if (newDGV1.RowCount > 0)
                        {
                            // GridIndex = ModelDataGrid.Position;
                            //  _Search();
                            newDGV1.CurrentCell = newDGV1.Rows[GridIndex].Cells[0];

                            clientsTaxListBindingSource_CurrentChanged(null, null);
                        }


                    }
                }


                _Connection.Close();


            }
            var Query = useListDataSet.ClientsTaxList.ToArray();

            if (Query.Any())
            {
                var _cnt = Query.Count()-1;
                lblClientCnt.Text = _cnt.ToString("N0") + "건 조회되었습니다.";
            }
            
            //var Query = nSTATSDataSet.NSTATS1.ToArray();

            //if (Query.Any())
            //{
            //    lblSalesPrice.Text = Query.Sum(c => c.SalesPrice).ToString("N0");
            //    lblTradePrice.Text = Query.Sum(c => c.TradePrice).ToString("N0");
            //    lblSTPrice.Text = (Query.Sum(c => c.SalesPrice) - Query.Sum(c => c.TradePrice)).ToString("N0");
            //    lblAlterPrice.Text = Query.Sum(c => c.AlterPrice).ToString("N0");
            //    lblStartPrice.Text = Query.Sum(c => c.StartPrice).ToString("N0");
            //    lblStopPrice.Text = Query.Sum(c => c.StopPrice).ToString("N0");
            //    lblDriverPrice.Text = Query.Sum(c => c.DriverPrice).ToString("N0");
            //}
        }

        private String GetSelectCommand2()
        {
            return @"SELECT CDate,Amount,DriverPoints.DriverId,Drivers.MobileNo,Drivers.CarNo,Drivers.CarYear,Remark,PointItem FROM DriverPoints
JOIN Drivers ON Driverpoints.DriverId = Drivers.DriverId ";



        }
        private void LoadTable2()
        {
            useListDataSet.DriverTaxList.Clear();

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand2();

                    List<String> WhereStringList = new List<string>();


                 


                    WhereStringList.Add("CONVERT(VARCHAR(10), CDate,111) >= @Sdate AND CONVERT(VARCHAR(10),CDate,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate", dtp_Sdate2.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate2.Text);

                    WhereStringList.Add("Remark = '전자 세금계산서 발행 수수료'  ");







                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }

                    SelectCommandText += " UNION ALL " +
                        " SELECT getdate(),SUM(-amount),0,'','','','','' FROM DriverPoints ";

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }
                    //  SelectCommandText += " Order by orders.CreateTime  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        useListDataSet.DriverTaxList.Load(_Reader);


                        if (newDGV1.RowCount > 0)
                        {
                            // GridIndex = ModelDataGrid.Position;
                            //  _Search();
                            newDGV2.CurrentCell = newDGV1.Rows[GridIndex2].Cells[0];

                            driverTaxListBindingSource_CurrentChanged(null, null);
                        }


                    }
                }


                _Connection.Close();

                var Query = useListDataSet.DriverTaxList.ToArray();

                if (Query.Any())
                {
                    var _cnt = Query.Count() - 1;
                    int cnt2 = 0;
                    using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        Connection.Open();
                        SqlCommand Command = Connection.CreateCommand();
                        Command.CommandText =
                            @"SELECT COUNT(*) FROM trades " +
                        " WHERE billNo is not null " +
                        " AND etaxcancelyn = 'N' " +
                        $" AND convert(varchar(10),RequestDate,111) >= '{dtp_Sdate2}' AND convert(varchar(10),RequestDate,111) <= '{dtp_Edate2}' ";

                        var DataReader = Command.ExecuteReader();
                        if (DataReader.Read())
                        {

                            cnt2 = DataReader.GetInt32(0);

                        }
                        Connection.Close();
                    }



               


                    lblDriverCnt.Text = "취소건포함 "+_cnt.ToString("N0") + "건 조회되었습니다.  취소건 제외"+ cnt2 +"건";
                }
            }

           
        }


        private void btn_Search_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            //{
            //    newDGV1[e.ColumnIndex, e.RowIndex].Value = (newDGV1.Rows.Count - e.RowIndex).ToString();
            //}

            // if (e.ColumnIndex == idx.Index)
           


            if (e.ColumnIndex == idx.Index)
            {
                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    e.Value = "합계";
                }
                else
                {
                    //e.Value = e.RowIndex + 1;
                    e.Value = (newDGV1.Rows.Count - e.RowIndex-1).ToString();
                }
            }
            if (e.ColumnIndex == cDateDataGridViewTextBoxColumn.Index)
            {
                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    e.Value = "";
                }
                else
                {
                    //e.Value = e.RowIndex + 1;
                    //e.Value = (newDGV1.Rows.Count - e.RowIndex - 1).ToString();
                }
            }

        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            Fclear();
            btn_Search_Click(null, null);
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {
            if (newDGV1.RowCount == 0)
            {
                MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;

            var fileString = "운송주선사세금계산서발행내역_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
            for (int i = 0; i < newDGV1.ColumnCount; i++)
            {
                if (newDGV1.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < newDGV1.ColumnCount; i++)
            {
                if (newDGV1.Columns[i].Visible)
                {
                    ColumnHeaderMap.Add(newDGV1.Columns[i].HeaderText);
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
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = newDGV1[ColumnIndexMap[j], i].FormattedValue?.ToString();

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

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void clientsTaxListBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (clientsTaxListBindingSource.Current == null)
            {
                return;
            }
            else
            {
                var Selected = ((DataRowView)clientsTaxListBindingSource.Current).Row as UseListDataSet.ClientsTaxListRow;

                GridIndex = clientsTaxListBindingSource.Position;
                //_SOrderId = Selected.OrderId;
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

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

          

            if (e.ColumnIndex == dataGridViewTextBoxColumn1.Index)
            {
                if (e.RowIndex == newDGV2.RowCount - 1)
                {
                    e.Value = "합계";
                }
                else
                {
                    //e.Value = e.RowIndex + 1;
                    e.Value = (newDGV2.Rows.Count - e.RowIndex - 1).ToString();
                }
            }
            if (e.ColumnIndex == cDateDataGridViewTextBoxColumn1.Index)
            {
                if (e.RowIndex == newDGV2.RowCount - 1)
                {
                    e.Value = "";
                }
                else
                {
                    //e.Value = e.RowIndex + 1;
                    //e.Value = (newDGV1.Rows.Count - e.RowIndex - 1).ToString();
                }
            }
        }

        private void cmbSMontDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMontDriver.SelectedIndex)
            {
                //당일
                case 0:
                    dtp_Sdate2.Value = DateTime.Now;
                    dtp_Edate2.Value = DateTime.Now;
                    break;
                //전일
                case 1:
                    dtp_Sdate2.Value = DateTime.Now.AddDays(-1);
                    dtp_Edate2.Value = DateTime.Now.AddDays(-1);
                    break;
                //금주
                case 2:
                    dtp_Sdate2.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    dtp_Edate2.Value = DateTime.Now;
                    break;
                //금월
                case 3:
                    dtp_Sdate2.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_Edate2.Value = DateTime.Now;
                    break;
                //전월
                case 4:
                    dtp_Sdate2.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_Edate2.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 5:
                    dtp_Sdate2.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                    dtp_Edate2.Value = DateTime.Now;
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadTable2();
        }

        private void driverTaxListBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (driverTaxListBindingSource.Current == null)
            {
                return;
            }
            else
            {
                var Selected = ((DataRowView)driverTaxListBindingSource.Current).Row as UseListDataSet.DriverTaxListRow;

                GridIndex2 = driverTaxListBindingSource.Position;
                //_SOrderId = Selected.OrderId;
            }
        }

        private void ExcelExportBasic2()
        {
            if (newDGV2.RowCount == 0)
            {
                MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;

            var fileString = "차주세금계산서발행내역_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
            for (int i = 0; i < newDGV2.ColumnCount; i++)
            {
                if (newDGV2.Columns[i].Visible)
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < newDGV2.ColumnCount; i++)
            {
                if (newDGV2.Columns[i].Visible)
                {
                    ColumnHeaderMap.Add(newDGV2.Columns[i].HeaderText);
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
            for (int i = 0; i < newDGV2.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = newDGV2[ColumnIndexMap[j], i].FormattedValue?.ToString();

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

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelExportBasic2();
        }
    }
}
