using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.XML;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301PrevPopup : Form
    {
        string _SangHo = "";
        string _SGubun = "L";
        public int _OrderId;

        public FrmMN0301PrevPopup(string SangHo = "")
        {
            InitializeComponent();

            _SangHo = SangHo;            
            InitDriverTable();

            //grid1
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);
        }
        public FrmMN0301PrevPopup(string SGubun, string SangHo = "")
        {
            InitializeComponent();

            _SangHo = SangHo;
            _SGubun = SGubun;
            InitDriverTable();

            //grid1
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);
        }
        private void FrmMN0301PrevPopup_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'orderDataSet.OrdersPopup' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.ordersPopupTableAdapter.Fill(this.orderDataSet.OrdersPopup);
            dtp_From.Value = DateTime.Now.AddMonths(-3).AddDays(1).Date;
            dtp_To.Value = DateTime.Now.Date;

            txt_Search.Text = _SangHo;

            _Search(_SGubun);

        }
        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();
        class DriverViewModel
        {
            public int DriverId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string BizNo { get; set; }
            public int candidateid { get; set; }
            public bool AppUse { get; set; }
            public string MobileNo { get; set; }
            public string CarNo { get; set; }
            public string CarYear { get; set; }
            public int CarSize { get; set; }
            public int CarType { get; set; }
            public string Password { get; set; }

        }


        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _DriverTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,MobileNo,CarNo,CarYear,CarSize,CarType,Password FROM Drivers ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _DriverTable.Add(
                              new DriverViewModel
                              {
                                  DriverId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                                  BizNo = dataReader.GetString(4),
                                  candidateid = dataReader.GetInt32(5),
                                  AppUse = dataReader.GetBoolean(6),
                                  MobileNo = dataReader.GetString(7),
                                  CarNo = dataReader.GetString(8),
                                  CarYear = dataReader.GetString(9),
                                  CarSize = dataReader.GetInt32(10),
                                  CarType = dataReader.GetInt32(11),
                                  Password = dataReader.GetString(12),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        private String GetSelectCommand()
        {

            return @"SELECT OrderId,OrderStatus,ISNULL(Customer,'')Customer,ISNULL(CustomerId,0) CustomerId,CreateTime,
                    StartState,StartCity,StartStreet,ISNULL(StartDetail,'')StartDetail,ISNULL(StartName,N'') as StartName
                    ,StopState,StopCity,StopStreet,ISNULL(StopDetail,N'')StopDetail,ISNULL(StopName,N'')StopName
                    ,Item ,ISNULL(TradePrice,0)TradePrice, ISNULL(SalesPrice,0) as SalesPrice,ISNULL(AlterPrice,0)AlterPrice
                    ,ISNULL(StartPrice,0) StartPrice,ISNULL(StopPrice,0) StopPrice,ISNULL(DriverPrice,0) DriverPrice,PayLocation
                    ,CarSize,CarType,IsShared, SharedItemLength, SharedItemSize,Remark,isnull(DriverId,0)as DriverId ,ItemSize FROM Orders";


        }

        public void _Search(string _SGubun)
        {
            orderDataSet.OrdersPopup.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

                    // 2. 조회 기간
                    String _DateColumn = "";

                    _DateColumn = "Orders.CreateTime";



                    WhereStringList.Add($"{_DateColumn} >= @Begin AND {_DateColumn} < @End ");
                    _Command.Parameters.AddWithValue("@Begin", dtp_From.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtp_To.Value.Date.AddDays(1));

                    WhereStringList.Add("Orders.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    if (_SGubun == "W")
                    {
                        WhereStringList.Add(string.Format("Orders.Customer =  '{0}'", txt_Search.Text));
                    }
                    else
                    {
                        WhereStringList.Add(string.Format("Orders.Customer Like  '%{0}%'", txt_Search.Text));
                    }


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }





                    SelectCommandText += "Order by Orders.CreateTime Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        orderDataSet.OrdersPopup.Load(_Reader);


                    }
                }
                _Connection.Close();
            }
        }

        private void DataList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as OrderDataSet.OrdersPopupRow;


            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnNum.Index)
            {
                grid1[e.ColumnIndex, e.RowIndex].Value = (grid1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            else if (e.ColumnIndex == orderStatusDataGridViewTextBoxColumn.Index)
            {
                switch (Selected.OrderStatus)
                {
                    case 0:
                        e.Value = "취소";
                        break;
                    case 1:

                        if (Selected.CreateTime.Date > DateTime.Now.Date)
                        {
                            e.Value = "예약";
                        }
                        else
                        {
                            e.Value = "접수";
                        }

                        break;
                    case 2:
                        e.Value = "대기";
                        break;
                    case 3:

                        e.Value = "완료";
                        break;
                }
            }
            else if (e.ColumnIndex == createTimeDataGridViewTextBoxColumn.Index)
            {
                e.Value = Selected.CreateTime.ToString("yyyy-MM-dd HH:mm").Replace("-", "/");
            }
            else if (e.ColumnIndex == payLocationDataGridViewTextBoxColumn.Index)
            {
                if (Selected.PayLocation != 0)
                {
                    if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "PayLocation" && c.Value == Selected.PayLocation))
                        e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "PayLocation" && c.Value == Selected.PayLocation).Name;
                }
            }
            else if (e.ColumnIndex == carTypeDataGridViewTextBoxColumn.Index)
            {
                if (Selected.CarType != 0)
                {
                    if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == Selected.CarType))
                        e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarType" && c.Value == Selected.CarType).Name;
                }
            }
            else if (e.ColumnIndex == carSizeDataGridViewTextBoxColumn.Index)
            {
                if (Selected.CarType != 0)
                {
                    if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == Selected.CarSize))
                        e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == Selected.CarSize).Name;
                }




            }
            else if (e.ColumnIndex == ColumnCarSize.Index)
            {

                //if (Selected.OrderStatus == 3)
                //{
                //    if (Selected.DriverId > 0)
                //    {
                //        var drivers = _DriverTable.Where(c => c.DriverId == Selected.DriverId).ToArray();

                //        if (drivers.Any())
                //        {
                //            if (drivers.First().CarSize != 0)
                //            {
                //                if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == drivers.First().CarSize))
                //                    e.Value = SingleDataSet.Instance.StaticOptions.First(c => c.Div == "CarSize" && c.Value == drivers.First().CarSize).Name;
                //            }

                //        }


                //    }
                //}
                //else
                //{
                //    e.Value = "";
                //}
            }


            else if (e.ColumnIndex == ColumnsShared.Index)
            {
                if (Selected.IsShared)
                {
                    e.Value = "혼적";
                }
                else
                {
                    e.Value = "독차";
                }
            }
            else if (e.ColumnIndex == ColumnsSharedItem.Index)
            {
                e.Value = "";
                if (Selected.IsShared)
                {
                    List<String> ValueTextList = new List<string>();
                    if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "SharedItemLength" && c.Value == Selected.SharedItemLength))
                        ValueTextList.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "SharedItemLength" && c.Value == Selected.SharedItemLength).Name);
                    if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "SharedItemSize" && c.Value == Selected.SharedItemSize))
                        ValueTextList.Add(SingleDataSet.Instance.StaticOptions.First(c => c.Div == "SharedItemSize" && c.Value == Selected.SharedItemSize).Name);
                    e.Value = String.Join("/", ValueTextList);
                }
            }
            else if (e.ColumnIndex == startNameDataGridViewTextBoxColumn.Index)
            {
                // var _Row = ((DataRowView)ordersPopupBindingSource.Current).Row;
                e.Value = String.Join(" ", new object[] { Selected.StartState, Selected.StartCity, Selected.StartStreet, Selected.StartDetail }.Where(c => c != null && !String.IsNullOrWhiteSpace(c.ToString())));
            }
            else if (e.ColumnIndex == stopNameDataGridViewTextBoxColumn.Index)
            {
                // var _Row = ((DataRowView)ordersPopupBindingSource.Current).Row;
                e.Value = String.Join(" ", new object[] { Selected.StopState, Selected.StopCity, Selected.StopStreet, Selected.StopDetail }.Where(c => c != null && !String.IsNullOrWhiteSpace(c.ToString())));
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search("L");
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void DataList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            var _Row = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as OrderDataSet.OrdersPopupRow;


            _Select(_Row.OrderId);
        }

        private void _Select(int OrderId)
        {

            _OrderId = OrderId;


            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,
                  ColumnNum,
                 //orderIdDataGridViewTextBoxColumn,
                 orderStatusDataGridViewTextBoxColumn,
                 customerDataGridViewTextBoxColumn,
                 //customerIdDataGridViewTextBoxColumn,
                 createTimeDataGridViewTextBoxColumn,
                 //startStateDataGridViewTextBoxColumn,
                 //startCityDataGridViewTextBoxColumn,
                 //startStreetDataGridViewTextBoxColumn,
                 //startDetailDataGridViewTextBoxColumn,
                 startNameDataGridViewTextBoxColumn,
                 //stopStateDataGridViewTextBoxColumn,
                 //stopCityDataGridViewTextBoxColumn,
                 //stopStreetDataGridViewTextBoxColumn,
                 //stopDetailDataGridViewTextBoxColumn,
                 stopNameDataGridViewTextBoxColumn,
                 itemDataGridViewTextBoxColumn,
                 ColumnCarSize,
                 salesPriceDataGridViewTextBoxColumn,
                 tradePriceDataGridViewTextBoxColumn,
                 alterPriceDataGridViewTextBoxColumn,
                 startPriceDataGridViewTextBoxColumn,
                 stopPriceDataGridViewTextBoxColumn,
                 driverPriceDataGridViewTextBoxColumn,
                 payLocationDataGridViewTextBoxColumn,
                 carSizeDataGridViewTextBoxColumn,
                 carTypeDataGridViewTextBoxColumn,
                 //isSharedDataGridViewCheckBoxColumn,
                 //sharedItemLengthDataGridViewTextBoxColumn,
                 //sharedItemSizeDataGridViewTextBoxColumn,
                 ColumnsShared,
                 ColumnsSharedItem


                );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);
        }
    }
}
