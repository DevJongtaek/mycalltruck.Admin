using mycalltruck.Admin.Class.Extensions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using mycalltruck.Admin.Class.Common;
using System.Threading.Tasks;

namespace mycalltruck.Admin
{
    public partial class FrmMN0213_PRICEMANAGE : Form
    {
        public FrmMN0213_PRICEMANAGE()
        {
            InitializeComponent();
            InitializeStore();
            InitializeCode();
        }

        #region ACTION
        private void btn_New_Click(object sender, EventArgs e)
        {
            ImportExcel();
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                var _Deleted = _BindingList[dataGridView1.SelectedRows[0].Index];
                var _DeletedCount = _Deleted.Count;
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    var _Command = _Connection.CreateCommand();
                    _Command.CommandText = "DELETE FROM CustomerPriceReferenceMasters WHERE ClientId = @ClientId AND CustomerId = @CustomerId";
                    _Command.Parameters.AddWithValue("ClientId", _Deleted.ClientId);
                    _Command.Parameters.AddWithValue("CustomerId", _Deleted.CustomerId);
                    _Command.ExecuteNonQuery();
                    _Connection.Close();
                }
                MessageBox.Show($"{_DeletedCount.ToString("N0")} 건의 운임료 항목이 삭제 되었습니다.", this.Text, MessageBoxButtons.OK);
                InitializeStore();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region UPDATER
        private void ImportExcel()
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "xlsx";
            d.Filter = "엑셀 (*.xlsx)|*.xlsx";
            d.FilterIndex = 0;
            if(d.ShowDialog() == DialogResult.OK)
            {
                ExcelPackage _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
                if(_Excel.Workbook.Worksheets.Count < 1)
                {
                    MessageBox.Show("엑셀 파일의 쉬트가 없습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                //var CustomerSheet = _Excel.Workbook.Worksheets[1];
                //var BizNo = CustomerSheet.Cells[2, 1].Text;
                //var Name = CustomerSheet.Cells[2, 2].Text;
                //if(String.IsNullOrEmpty(BizNo) || String.IsNullOrEmpty(Name))
                //{
                //    MessageBox.Show("화주 정보가 없습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                //    return;
                //}
                var ClientPriceSheet = _Excel.Workbook.Worksheets[1];
                //var PriceSheet = _Excel.Workbook.Worksheets[3];
                Dictionary<int, HeaderItem> HeaderDictionary = new Dictionary<int, HeaderItem>();
                int Row = 2;
                String State = "";
                while (true)
                {
                    String Text = ClientPriceSheet.Cells[Row, 1].Text;
                    if (!String.IsNullOrEmpty(Text))
                        State = Text;
                    String City = ClientPriceSheet.Cells[Row, 2].Text;
                    if (String.IsNullOrEmpty(City))
                        break;
                    String _ExcelText = $"{State} {City}";
                    String _State = State;
                    String _City = City;
                    if(_State == "광역시")
                    {
                        _State = _City;
                        _City = "";
                    }
                    String _TargetState = "";
                    String _TargetCity = "";
                    int? AddressReferenceId = null;
                    String _Match = "";
                    var _StateMatches = _AddressMatchList.Where(c => c.StateWord == _State);
                    if (_StateMatches.Any())
                    {
                        if (_StateMatches.First().IsAddressReference)
                        {
                            AddressReferenceId = _StateMatches.First().AddressReferenceId;
                            var Address = _AddressList.Find(c => c.Id == AddressReferenceId);
                            _TargetState = Address.State;
                            _TargetCity = Address.City;
                            _AddHeader(HeaderDictionary, Row, new HeaderItem {State = _State, City = _City, TargetState = _TargetState, TargetCity = _TargetCity, IsValid = true });
                            Row++;
                            continue;
                        }
                        else
                        {
                            _Match = _StateMatches.First().StateTarget;
                        }
                    }

                    var Q1 = _AddressList.Where(c => c.State.StartsWith(_State) || c.State == _Match);
                    if (Q1.Any())
                    {
                        _TargetState = Q1.First().State;
                        if (String.IsNullOrEmpty(_City))
                        {
                            _AddHeader(HeaderDictionary, Row, new HeaderItem { State = _State, TargetState = _TargetState, IsValid = true });
                        }
                        else
                        {
                            _Match = null;
                            var _CityMatches = _AddressMatchList.Where(c => c.CityWord == _City);
                            if (_CityMatches.Any())
                                _Match = _CityMatches.First().CityTarget;
                            if(_Match=="")
                            {
                                _TargetCity = _Match;
                                _AddHeader(HeaderDictionary, Row, new HeaderItem { State = _State, City = _City, TargetState = _TargetState, TargetCity = _TargetCity, IsValid = true });
                            }
                            else
                            {
                                var Q2 = _AddressList.Where(c => c.State == _TargetState && c.City.StartsWith(_City) || c.City == _Match);
                                if (Q2.Any())
                                {
                                    _TargetCity = Q2.First().City;
                                    _AddHeader(HeaderDictionary, Row, new HeaderItem { State = _State, City = _City, TargetState = _TargetState, TargetCity = _TargetCity, IsValid = true });
                                }
                                else
                                {
                                    _AddHeader(HeaderDictionary, Row, new HeaderItem { ExcelText = _ExcelText, State = _State, City = _City, TargetState = _TargetState, IsValid = false });
                                }
                            }
                        }
                    }
                    else
                    {
                        _AddHeader(HeaderDictionary, Row, new HeaderItem { ExcelText = _ExcelText, State = _State, City = _City, IsValid = false });
                    }
                    Row++;
                }
                if(HeaderDictionary.Count == 0)
                {
                    MessageBox.Show("추가할 운임정보가 없습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                FrmMN0213_PRICEMANAGE_Header f = new FrmMN0213_PRICEMANAGE_Header();
                f.InitializeHeader(HeaderDictionary.Values.Where(c => !c.IsValid), _AddressList, _AddressMatchList);
                f.InitializeClientAndCustomer(_ClientList);
                f.ShowDialog();
                if (!f.IsValid)
                    return;
                int ClientId = f.ClientId;
                int CustomerId = f.CustomerId;
                int Rate = f.Rate;
                pnProgress.Visible = true;
                Task.Factory.StartNew(() => {
                    List<PriceReference> _PriceReferenceList = new List<PriceReference>();
                    var RowIndexes = HeaderDictionary.Keys.OrderBy(c => c).ToArray();
                    for (int i = 0; i < RowIndexes.Length; i++)
                    {
                        var RowIndex = RowIndexes[i];
                        for (int j = 0; j < RowIndexes.Length; j++)
                        {
                            var ColIndex = RowIndexes[j] + 1;
                            var _ClientPriceText = ClientPriceSheet.Cells[RowIndex, ColIndex].Text.Replace(",", "");
                            int _ClientPrice = 0, _Price = 0;
                            if (int.TryParse(_ClientPriceText, out _ClientPrice))
                            {
                                _Price = Convert.ToInt32(((float)Math.Round((double)(_ClientPrice * Rate / 100d / 100d), 0, MidpointRounding.AwayFromZero) * 100f));
                                var Start = HeaderDictionary[RowIndex];
                                var End = HeaderDictionary[ColIndex - 1];
                                var StartText = $"{Start.State} {Start.City}";
                                var EndText = $"{End.State} {End.City}";
                                var StartState = Start.TargetState;
                                var StartCity = Start.TargetCity;
                                var EndState = End.TargetState;
                                var EndCity = End.TargetCity;
                                _PriceReferenceList.Add(new Admin.FrmMN0213_PRICEMANAGE.PriceReference
                                {
                                    StartText = StartText,
                                    EndText = EndText,
                                    StartState = StartState,
                                    EndState = EndState,
                                    StartCity = StartCity,
                                    EndCity = EndCity,
                                    Price = _Price,
                                    ClientPrice = _ClientPrice,
                                });
                                var OddItem = new PriceReference
                                {
                                    StartText = EndText,
                                    EndText = StartText,
                                    StartState = EndState,
                                    EndState = StartState,
                                    StartCity = EndCity,
                                    EndCity = StartCity,
                                    Price = _Price,
                                    ClientPrice = _ClientPrice,
                                };
                                if (!_PriceReferenceList.Any(c => c.StartState == OddItem.StartState && c.StartCity == OddItem.StartCity && c.EndState == OddItem.EndState && c.EndCity == OddItem.EndCity))
                                {
                                    _PriceReferenceList.Add(OddItem);
                                }
                            }
                        }
                    }
                    if (_PriceReferenceList.Count == 0)
                    {
                        Invoke(new Action(() => {
                            MessageBox.Show("추가할 운임정보가 없습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                            pnProgress.Visible = false;
                        }));
                        return;
                    }
                    try
                    {
                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            using (var _DeleteCommand = _Connection.CreateCommand())
                            using (var _InsertCommand = _Connection.CreateCommand())
                            using (var _BulkCommand = new SqlBulkCopy(_Connection))
                            using (var _DataTable = new DataTable())
                            {
                                _DeleteCommand.CommandText = "DELETE FROM CustomerPriceReferenceMasters WHERE CustomerId = @CustomerId AND ClientId = @ClientId";
                                _DeleteCommand.Parameters.AddWithValue("@CustomerId", CustomerId);
                                _DeleteCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                _DeleteCommand.ExecuteNonQuery();
                                _InsertCommand.CommandText = "INSERT INTO CustomerPriceReferenceMasters (ClientId, CustomerId, Rate, Count) OUTPUT INSERTED.MasterId VALUES (@ClientId, @CustomerId, @Rate, @Count)";
                                _InsertCommand.Parameters.AddWithValue("@CustomerId", CustomerId);
                                _InsertCommand.Parameters.AddWithValue("@ClientId", ClientId);
                                _InsertCommand.Parameters.AddWithValue("@Rate", Rate);
                                _InsertCommand.Parameters.AddWithValue("@Count", _PriceReferenceList.Count);
                                int MasterId = (int)_InsertCommand.ExecuteScalar();
                                _BulkCommand.DestinationTableName = "CustomerPriceReferences";
                                _DataTable.Columns.Add("CustomerPriceId", typeof(int));
                                _DataTable.Columns.Add("Price", typeof(decimal));
                                _DataTable.Columns.Add("ClientPrice", typeof(decimal));
                                _DataTable.Columns.Add("StartState", typeof(string));
                                _DataTable.Columns.Add("StartCity", typeof(string));
                                _DataTable.Columns.Add("EndState", typeof(string));
                                _DataTable.Columns.Add("EndCity", typeof(string));
                                _DataTable.Columns.Add("StartText", typeof(string));
                                _DataTable.Columns.Add("EndText", typeof(string));
                                _DataTable.Columns.Add("MasterId", typeof(int));

                                foreach (var _PriceReference in _PriceReferenceList)
                                {
                                    _DataTable.LoadDataRow(new object[] { 0, _PriceReference.Price, _PriceReference.ClientPrice, _PriceReference.StartState, _PriceReference.StartCity, _PriceReference.EndState, _PriceReference.EndCity, _PriceReference.StartText, _PriceReference.EndText, MasterId }, true);
                                }
                                _BulkCommand.WriteToServer(_DataTable);
                            }
                            _Connection.Close();
                        }
                        Invoke(new Action(() => MessageBox.Show($"{_PriceReferenceList.Count} 건의 운임정보가 추가 되었습니다.", this.Text, MessageBoxButtons.OK)));
                    }
                    catch (Exception)
                    {
                        Invoke(new Action(() => MessageBox.Show("데이터 베이스 작업 중 오류가 발생하였습니다. 잠시 후에 다시 시도바랍니다.", this.Text, MessageBoxButtons.OK)));
                    }
                    Invoke(new Action(() => {
                        pnProgress.Visible = false;
                        InitializeStore();
                    }));
                });
            }
        }
        private void _AddHeader(Dictionary<int, HeaderItem> HeaderDictionary, int Index, HeaderItem _HeaderItem)
        {
            HeaderDictionary.Add(Index, _HeaderItem);
        }
        #endregion

        #region STORE
        private class Model
        {
            public int MasterId { get; set; }
            public int Count { get; set; }
            public int Rate { get; set; }
            public int OriginalRate { get; set; }
            public int ClientId { get; set; }
            public int CustomerId { get; set; }
            public string ClientBizNo { get; set; }
            public string ClientName { get; set; }
            public string CustomerBizNo { get; set; }
            public string CustomerName { get; set; }
        }
        private BindingList<Model> _BindingList = new BindingList<Model>();
        private void InitializeStore()
        {
            _BindingList.Clear();
            string _CommandText =
                @"SELECT 
	                MasterId, Clients.ClientId, Customers.CustomerId, [COUNT], [Rate],
	                Clients.BizNo as ClientBizNo,
	                Clients.Name as ClientName,
	                Customers.BizNo as CustomerBizNo,
	                Customers.SangHo as CustomerName
                FROM
                CustomerPriceReferenceMasters AS A
                JOIN Clients ON A.ClientId = Clients.ClientId
                JOIN Customers ON A.CustomerId = Customers.CustomerId";

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                var _Command = _Connection.CreateCommand();
                _Command.CommandText = _CommandText;
                var _Reader = _Command.ExecuteReader();
                while (_Reader.Read())
                {
                    _BindingList.Add(new Model
                    {
                        MasterId = _Reader.GetInt32(0),
                        ClientId = _Reader.GetInt32(1),
                        CustomerId = _Reader.GetInt32(2),
                        Count = _Reader.GetInt32(3),
                        OriginalRate = _Reader.GetInt32(4),
                        Rate = _Reader.GetInt32(4),
                        ClientBizNo = _Reader.GetString(5),
                        ClientName = _Reader.GetString(6),
                        CustomerBizNo = _Reader.GetString(7),
                        CustomerName = _Reader.GetString(8),
                    });
                }
                _Connection.Close();
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _BindingList;            
        }
        #endregion

        #region HELPER
        public class HeaderItem
        {
            public String ExcelText { get; set; } = "";
            public String State { get; set; } = "";
            public String City { get; set; } = "";
            public String TargetState { get; set; } = "";
            public String TargetCity { get; set; } = "";
            public bool IsValid { get; set; }
        }
        public class Address
        {
            public int Id { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
        }
        public class AddressMatch
        {
            public int Id { get; set; }
            public String StateWord { get; set; }
            public String StateTarget { get; set; }
            public String CityWord { get; set; }
            public String CityTarget { get; set; }
            public int? AddressReferenceId { get; set; }
            public bool IsAddressReference { get; set; }
        }
        public class PriceReference
        {
            public int Id { get; set; }
            public decimal Price { get; set; }
            public decimal ClientPrice { get; set; }
            public string StartState { get; set; }
            public string StartCity { get; set; }
            public string EndState { get; set; }
            public string EndCity { get; set; }
            public string StartText { get; set; }
            public string EndText { get; set; }
        }
        private List<Address> _AddressList = new List<Address>();
        private List<AddressMatch> _AddressMatchList = new List<AddressMatch>();
        private List<BasicModel> _ClientList = new List<BasicModel>();
        private void InitializeCode()
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText = "SELECT AddressReferenceId, State, City, Street FROM AddressReferences";
                IDataReader _Reader = _Command.ExecuteReader();
                while (_Reader.Read())
                {
                    _AddressList.Add(new Address
                    {
                        Id = _Reader.GetInt32(0),
                        State = _Reader.GetString(1),
                        City = _Reader.GetString(2),
                        Street = _Reader.GetString(3),
                    });
                }
                _Connection.Close();
            }
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText = "SELECT MatchId, StateWord, StateTarget, CityWord, CityTarget, AddressReferenceId, IsAddressReference FROM AddressMatches";
                SqlDataReader _Reader = _Command.ExecuteReader();
                while (_Reader.Read())
                {
                    _AddressMatchList.Add(new AddressMatch
                    {
                        Id = _Reader.GetInt32(0),
                        StateWord = _Reader.GetStringN(1),
                        StateTarget = _Reader.GetStringN(2),
                        CityWord = _Reader.GetStringN(3),
                        CityTarget = _Reader.GetStringN(4),
                        AddressReferenceId = _Reader.GetInt32N(5),
                        IsAddressReference = _Reader.GetBoolean(6),
                    });
                }
                _Connection.Close();
            }

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText = "SELECT ClientId, Name, BizNo  FROM Clients ORDER BY Name DESC ";
                SqlDataReader _Reader = _Command.ExecuteReader();
                while (_Reader.Read())
                {
                    int Id = _Reader.GetInt32(0);
                    String _Name = _Reader.GetString(1);
                    String _BizNo = _Reader.GetString(2);
                    _ClientList.Add(new BasicModel
                    {
                        Id=Id,
                        Text = $"{_Name}[{_BizNo}]",
                    });
                }
                _Connection.Close();
            }
        }
        #endregion

        #region COMMENT
        /*
        1. 1~2 열의 텍스트를 읽어 Header를 채운다.
        2. 행의 Header는 열의 Header와 동일하다.
        3. 읽은 텍스트중 알 수 없는 것은 사용자가 선택할 수 있는 UI를 표시한다.
        */
        #endregion

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            int colNumberIndex = colNumber.DisplayIndex;
            int colItemCountIndex = colItemCount.DisplayIndex;
            if(e.ColumnIndex == colNumberIndex)
            {
                e.Value = (e.RowIndex + 1).ToString("N0");
            }
            else if(e.ColumnIndex == colItemCount.DisplayIndex)
            {
                e.Value = ((int)e.Value).ToString("N0");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == ColumnAction.Index)
            {
                var Selected = _BindingList[e.RowIndex];
                if(Selected.OriginalRate != Selected.Rate)
                {
                    if (MessageBox.Show($"{Selected.ClientName}/{Selected.CustomerName} 차주 운송료 비율을 {Selected.OriginalRate}% 에서 {Selected.Rate}% 으로 변경하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    pnProgress.Visible = true;
                    Task.Factory.StartNew(() => {
                        try
                        {
                            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                _Connection.Open();
                                using (SqlCommand _MasterCommand = _Connection.CreateCommand())
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _MasterCommand.CommandText = "UPDATE CustomerPriceReferenceMasters SET Rate = @Rate WHERE MasterId = @MasterId";
                                    _Command.CommandText = "UPDATE CustomerPriceReferences SET Price = (ROUND(ClientPrice * @Rate * 0.01 * 0.01, 0) * 100 ) WHERE MasterId = @MasterId";
                                    _MasterCommand.Parameters.AddWithValue("@Rate", Selected.Rate);
                                    _MasterCommand.Parameters.AddWithValue("@MasterId", Selected.MasterId);
                                    _Command.Parameters.AddWithValue("@Rate", Selected.Rate);
                                    _Command.Parameters.AddWithValue("@MasterId", Selected.MasterId);
                                    _Command.ExecuteNonQuery();
                                    _MasterCommand.ExecuteNonQuery();
                                }
                                _Connection.Close();
                            }
                            Invoke(new Action(() => MessageBox.Show("화주운행료 비율 변경 작업이 완료되었습니다.", this.Text, MessageBoxButtons.OK)));
                        }
                        catch
                        {
                            Invoke(new Action(() => MessageBox.Show("데이터 베이스 작업 중 오류가 발생하였습니다. 잠시 후에 다시 시도바랍니다.", this.Text, MessageBoxButtons.OK)));
                        }
                        Invoke(new Action(() => {
                            pnProgress.Visible = false;
                            InitializeStore();
                        }));
                    });
                }
            }
        }
    }
}
