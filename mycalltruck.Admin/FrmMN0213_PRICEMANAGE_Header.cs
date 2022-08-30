using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;

namespace mycalltruck.Admin
{
    public partial class FrmMN0213_PRICEMANAGE_Header : Form
    {
        public bool IsValid { get; set; }
        public int ClientId { get; set; }
        public int CustomerId { get; set; }
        public int Rate { get; set; }
        private BindingList<FrmMN0213_PRICEMANAGE.HeaderItem> _BindingList = new BindingList<FrmMN0213_PRICEMANAGE.HeaderItem>();
        private List<BasicModel> ClientList = new List<BasicModel>();
        public FrmMN0213_PRICEMANAGE_Header()
        {
            InitializeComponent();
            IsValid = false;
        }

        private List<FrmMN0213_PRICEMANAGE.Address> _AddressList = new List<FrmMN0213_PRICEMANAGE.Address>();
        private List<FrmMN0213_PRICEMANAGE.AddressMatch> _AddressMatchList = new List<FrmMN0213_PRICEMANAGE.AddressMatch>();
        private bool InitializeHeaderComplete = false;

        public void InitializeHeader(IEnumerable<FrmMN0213_PRICEMANAGE.HeaderItem> _Header, List<FrmMN0213_PRICEMANAGE.Address> _AddressList, List<FrmMN0213_PRICEMANAGE.AddressMatch> _AddressMatchList)
        {
            this._AddressList = _AddressList;
            this._AddressMatchList = _AddressMatchList;

            foreach (var Value in _Header)
            {
                _BindingList.Add(Value);
            }
            headerGridView.AutoGenerateColumns = false;
            headerGridView.DataSource = _BindingList;
            InitializeHeaderComplete = true;

        }

        public void InitializeClientAndCustomer(List<BasicModel> _ClientList)
        {
            cmbClient.DisplayMember = "Text";
            cmbClient.ValueMember = "Id";
            ClientList.AddRange(_ClientList);
            cmbClient.DataSource = ClientList;
            if (cmbClient.Items.Count > 0)
                cmbClient.SelectedIndex = 0;
            ClientId = Convert.ToInt32(cmbClient.SelectedValue);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (cmbClient.SelectedIndex < 0 || cmbCustomer.SelectedIndex < 0)
                return;
            ClientId = Convert.ToInt32(cmbClient.SelectedValue);
            CustomerId = Convert.ToInt32(cmbCustomer.SelectedValue);
            Rate = (int)nmRate.Value;

            if (_BindingList.Any(c => !c.IsValid))
            {
                if(MessageBox.Show("주소가 수정되지 않은 건이 있습니다. 해당 건의 운임정보는 입력되지 않습니다. 그래도 진행하시겠습니까?",this.Text, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            }
            IsValid = true;
            Close();
        }

        private void headerGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //TODO AddressReferenceId 지정
            //TODO Start/City Word 분리

            var cell = headerGridView[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if(cell != null)
            {
                var value = cell.Value == null ? "" : cell.Value.ToString();
                cell.Items.Clear();
                cell.Items.Add(value);
                cell.Value = cell.Items[0];
                int colStateIndex = ColState.DisplayIndex;
                int colCityIndex = ColCity.DisplayIndex;
                var Model = _BindingList[e.RowIndex];
                if(e.ColumnIndex == colStateIndex)
                {
                    if(_AddressList.Any(c => c.State == value))
                    {
                        Model.TargetState = value;
                        if(String.IsNullOrEmpty(Model.City))
                        {
                            Model.IsValid = true;
                        }
                        else
                        {
                            var cityCell = headerGridView[colCityIndex, e.RowIndex] as DataGridViewComboBoxCell;
                            if (cityCell.Value != null)
                            {
                                if (_AddressList.Any(c => c.State == value && c.City == cityCell.Value.ToString()))
                                {
                                    Model.TargetCity = cityCell.Value.ToString();
                                    Model.IsValid = true;
                                }
                            }
                        }
                        TryAppendAddressMatchState(Model.State, Model.TargetState);
                    }
                }
                else if(e.ColumnIndex == colCityIndex)
                {
                    var stateCell = headerGridView[colStateIndex, e.RowIndex] as DataGridViewComboBoxCell;
                    if(stateCell.Value != null)
                    {
                        if(String.IsNullOrEmpty(value))
                        {
                            Model.TargetCity = value;
                            Model.IsValid = true;
                            TryAppendAddressMatchCity(Model.City, Model.TargetCity);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(Model.City))
                            {
                                if (_AddressList.Any(c => c.State == stateCell.Value.ToString() && c.City == value && String.IsNullOrEmpty(c.Street)))
                                {
                                    Model.TargetCity = value;
                                    Model.IsValid = true;
                                    int Id = _AddressList.First(c => c.State == stateCell.Value.ToString() && c.City == value && String.IsNullOrEmpty(c.Street)).Id;
                                    TryAppendAddressMatchState(Model.State, Id);
                                }
                            }
                            else
                            {
                                if (_AddressList.Any(c => c.State == stateCell.Value.ToString() && c.City == value))
                                {
                                    Model.TargetCity = value;
                                    Model.IsValid = true;
                                    TryAppendAddressMatchCity(Model.City, Model.TargetCity);
                                }
                            }
                        }
                    }
                }
                headerGridView.Update();
                headerGridView.Refresh();
            }
        }

        private void headerGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var cell = headerGridView.CurrentCell as DataGridViewComboBoxCell;
            var Model = _BindingList[cell.RowIndex];
            int colStateIndex = ColState.DisplayIndex;
            int colCityIndex = ColCity.DisplayIndex;
            var comboBox = e.Control as ComboBox;
            if (comboBox == null)
                return;
            String value = cell.Value == null ? "" : cell.Value.ToString();
            comboBox.Items.Clear();
            int SelectedIndex = 0;
            if (cell.ColumnIndex == colStateIndex)
            {
                if (String.IsNullOrEmpty(Model.TargetState))
                {
                    foreach (var state in _AddressList.Select(c=>c.State).Distinct())
                    {
                        comboBox.Items.Add(state);
                    }
                    comboBox.Items.Add(Model.State);
                    comboBox.SelectedIndex = comboBox.Items.Count - 1;
                }
                else
                {
                    foreach (var state in _AddressList.Select(c => c.State).Distinct())
                    {
                        comboBox.Items.Add(state);
                        if(state == Model.TargetState)
                        {
                            SelectedIndex = comboBox.Items.Count - 1;
                        }
                    }
                    comboBox.SelectedIndex = SelectedIndex;
                }
            }
            else if (cell.ColumnIndex == colCityIndex)
            {
                if (Model.IsValid)
                {
                    foreach (var city in _AddressList.Where(c => c.State == Model.TargetState).Select(c => c.City).Distinct())
                    {
                        comboBox.Items.Add(city);
                        if(city == value)
                        {
                            SelectedIndex = comboBox.Items.Count - 1;
                        }
                    }
                    if(SelectedIndex == 0)
                    {
                        comboBox.Items.Add(value);
                        SelectedIndex = comboBox.Items.Count - 1;
                    }
                    comboBox.SelectedIndex = SelectedIndex;
                }
                else
                {
                    if (!String.IsNullOrEmpty(Model.TargetState))
                    {
                        comboBox.Items.Add("");
                        foreach (var city in _AddressList.Where(c => c.State == Model.TargetState).Select(c => c.City).Distinct())
                        {
                            comboBox.Items.Add(city);
                            if (city == value)
                            {
                                SelectedIndex = comboBox.Items.Count - 1;
                            }
                        }
                        if (SelectedIndex == 0 & !String.IsNullOrEmpty(value))
                        {
                            comboBox.Items.Add(value);
                            SelectedIndex = comboBox.Items.Count - 1;
                        }
                        if (String.IsNullOrEmpty(value))
                        {
                            comboBox.SelectedIndex = 0;
                        }
                        else
                        {
                            comboBox.SelectedIndex = SelectedIndex;
                        }
                    }
                    else
                    {
                        comboBox.Items.Add(value);
                        comboBox.SelectedIndex = 0;
                    }
                }
            }
            e.CellStyle.BackColor = headerGridView.DefaultCellStyle.BackColor;
            // ColOrigin CellFormatting Fire
            Model.ExcelText = Model.ExcelText + " ";
            Model.ExcelText = Model.ExcelText.Substring(0,Model.ExcelText.Length-1);

        }

        private void headerGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (InitializeHeaderComplete)
            {
                InitializeHeaderComplete = false;
                int colStateIndex = ColState.DisplayIndex;
                int colCityIndex = ColCity.DisplayIndex;
                for (int i = 0; i < headerGridView.Rows.Count; i++)
                {
                    FrmMN0213_PRICEMANAGE.HeaderItem Model = _BindingList[i];
                    if (String.IsNullOrEmpty(Model.TargetState))
                    {
                        (headerGridView[colStateIndex, i] as DataGridViewComboBoxCell).Items.Add(_BindingList[i].State);
                        (headerGridView[colStateIndex, i] as DataGridViewComboBoxCell).Value = (headerGridView[colStateIndex, i] as DataGridViewComboBoxCell).Items[0];
                    }
                    else
                    {
                        (headerGridView[colStateIndex, i] as DataGridViewComboBoxCell).Items.Add(_BindingList[i].TargetState);
                        (headerGridView[colStateIndex, i] as DataGridViewComboBoxCell).Value = (headerGridView[colStateIndex, i] as DataGridViewComboBoxCell).Items[0];
                    }
                    if (!String.IsNullOrEmpty(Model.City))
                    {
                        if (String.IsNullOrEmpty(Model.TargetCity))
                        {
                            (headerGridView[colCityIndex, i] as DataGridViewComboBoxCell).Items.Add(_BindingList[i].City);
                            (headerGridView[colCityIndex, i] as DataGridViewComboBoxCell).Value = (headerGridView[colCityIndex, i] as DataGridViewComboBoxCell).Items[0];
                        }
                        else
                        {
                            (headerGridView[colCityIndex, i] as DataGridViewComboBoxCell).Items.Add(_BindingList[i].TargetCity);
                            (headerGridView[colCityIndex, i] as DataGridViewComboBoxCell).Value = (headerGridView[colCityIndex, i] as DataGridViewComboBoxCell).Items[0];
                        }
                    }
                }
            }
        }

        private void headerGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            int colStateIndex = ColState.DisplayIndex;
            int colCityIndex = ColCity.DisplayIndex;

            if(e.ColumnIndex == colStateIndex || e.ColumnIndex == colCityIndex)
            {
                var Model = _BindingList[e.RowIndex];
                var topLeftPoint = new Point(e.CellBounds.Left, e.CellBounds.Top);
                var topRightPoint = new Point(e.CellBounds.Right - 1, e.CellBounds.Top);
                var bottomRightPoint = new Point(e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                var bottomleftPoint = new Point(e.CellBounds.Left, e.CellBounds.Bottom - 1);
                var backGroundPen = new Pen(e.CellStyle.BackColor, 1);
                var gridlinePen = new Pen(headerGridView.GridColor, 1);
                var errorPen = new Pen(Color.FromArgb(0xFF, 0xCA, 0x28), 1);

                e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);
                if (e.ColumnIndex == colStateIndex)
                {
                    if (String.IsNullOrEmpty(Model.TargetState))
                    {
                        e.Graphics.DrawRectangle(errorPen, new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height - 1));
                    }
                    else
                    {
                        e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);
                        if (e.RowIndex == 0)
                            e.Graphics.DrawLine(backGroundPen, topLeftPoint, topRightPoint);
                        if (e.ColumnIndex == 0)
                            e.Graphics.DrawLine(backGroundPen, topLeftPoint, bottomleftPoint);
                        if (e.RowIndex == headerGridView.RowCount - 1)
                            e.Graphics.DrawLine(gridlinePen, bottomRightPoint, bottomleftPoint);
                        else  
                            e.Graphics.DrawLine(backGroundPen, bottomRightPoint, bottomleftPoint);
                        if (e.ColumnIndex == headerGridView.ColumnCount - 1)
                            e.Graphics.DrawLine(gridlinePen, bottomRightPoint, topRightPoint);
                        else 
                            e.Graphics.DrawLine(backGroundPen, bottomRightPoint, topRightPoint);
                        if (e.RowIndex > 0)
                            e.Graphics.DrawLine(gridlinePen, topLeftPoint, topRightPoint);
                        if (e.ColumnIndex > 0)
                            e.Graphics.DrawLine(gridlinePen, topLeftPoint, bottomleftPoint);
                    }
                }
                else if (e.ColumnIndex == colCityIndex)
                {
                    if (!String.IsNullOrEmpty(Model.City))
                    {
                        if (String.IsNullOrEmpty(Model.TargetCity))
                        {
                            e.Graphics.DrawRectangle(errorPen, new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height - 1));
                        }
                        else
                        {
                            e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);
                            if (e.RowIndex == 0)
                                e.Graphics.DrawLine(backGroundPen, topLeftPoint, topRightPoint);
                            if (e.ColumnIndex == 0)
                                e.Graphics.DrawLine(backGroundPen, topLeftPoint, bottomleftPoint);
                            if (e.RowIndex == headerGridView.RowCount - 1)
                                e.Graphics.DrawLine(gridlinePen, bottomRightPoint, bottomleftPoint);
                            else
                                e.Graphics.DrawLine(backGroundPen, bottomRightPoint, bottomleftPoint);
                            if (e.ColumnIndex == headerGridView.ColumnCount - 1)
                                e.Graphics.DrawLine(gridlinePen, bottomRightPoint, topRightPoint);
                            else
                                e.Graphics.DrawLine(backGroundPen, bottomRightPoint, topRightPoint);
                            if (e.RowIndex > 0)
                                e.Graphics.DrawLine(gridlinePen, topLeftPoint, topRightPoint);
                            if (e.ColumnIndex > 0)
                                e.Graphics.DrawLine(gridlinePen, topLeftPoint, bottomleftPoint);
                        }

                    }
                }

                e.Handled = true;
            }
        }

        private void TryAppendAddressMatchState(String StateWord, String StateTarget)
        {
            if(!_AddressMatchList.Any(c=>c.StateWord == StateWord && c.StateTarget == StateTarget))
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    var DeleteCommand = _Connection.CreateCommand();
                    DeleteCommand.CommandText = "DELETE FROM AddressMatches WHERE StateWord = @StateWord";
                    DeleteCommand.Parameters.AddWithValue("@StateWord", StateWord);
                    DeleteCommand.ExecuteNonQuery();
                    var InsertCommand = _Connection.CreateCommand();
                    InsertCommand.CommandText = "INSERT INTO AddressMatches (StateWord, StateTarget) output INSERTED.MatchId VALUES (@StateWord, @StateTarget)";
                    InsertCommand.Parameters.AddWithValue("@StateWord", StateWord);
                    InsertCommand.Parameters.AddWithValue("@StateTarget", StateTarget);
                    int Id = (int)InsertCommand.ExecuteScalar();
                    _Connection.Close();

                    _AddressMatchList.RemoveAll(c => c.StateWord == StateWord);
                    _AddressMatchList.Add(new FrmMN0213_PRICEMANAGE.AddressMatch
                    {
                        Id = Id,
                        StateWord = StateWord,
                        StateTarget = StateTarget,
                    });
                }

                int colStateIndex = ColState.DisplayIndex;
                int colCityIndex = ColCity.DisplayIndex;
                for (int i = 0; i < _BindingList.Count; i++)
                {
                    var Model = _BindingList[i];
                    if(Model.State == StateWord && Model.TargetState != StateTarget)
                    {
                        var cell = headerGridView[colStateIndex, i] as DataGridViewComboBoxCell;
                        cell.Items.Clear();
                        cell.Items.Add(StateTarget);
                        cell.Value = cell.Items[0];
                        Model.TargetState = StateTarget;
                        if (!String.IsNullOrEmpty(Model.City))
                        {
                            if(_AddressList.Any(c=>c.State == Model.TargetState && c.City == Model.City))
                            {
                                Model.TargetCity = Model.City;
                                Model.IsValid = true;
                            }
                        }
                        else
                        {
                            Model.IsValid = true;
                        }
                    }
                }
            }
        }

        private void TryAppendAddressMatchState(String StateWord, int AddressReferenceId)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                var DeleteCommand = _Connection.CreateCommand();
                DeleteCommand.CommandText = "DELETE FROM AddressMatches WHERE StateWord = @StateWord";
                DeleteCommand.Parameters.AddWithValue("@StateWord", StateWord);
                DeleteCommand.ExecuteNonQuery();
                var InsertCommand = _Connection.CreateCommand();
                InsertCommand.CommandText = "INSERT INTO AddressMatches (StateWord, AddressReferenceId, IsAddressReference) output INSERTED.MatchId VALUES (@StateWord, @AddressReferenceId, 1)";
                InsertCommand.Parameters.AddWithValue("@StateWord", StateWord);
                InsertCommand.Parameters.AddWithValue("@AddressReferenceId", AddressReferenceId);
                int Id = (int)InsertCommand.ExecuteScalar();
                _Connection.Close();

                _AddressMatchList.RemoveAll(c => c.StateWord == StateWord);
                _AddressMatchList.Add(new FrmMN0213_PRICEMANAGE.AddressMatch
                {
                    Id = Id,
                    StateWord = StateWord,
                    IsAddressReference = true,
                    AddressReferenceId = AddressReferenceId,
                });
            }
        }
        
        private void TryAppendAddressMatchCity(String CityWord, String CityTarget)
        {
            if (!_AddressMatchList.Any(c => c.StateWord == CityWord && c.StateTarget == CityTarget))
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    var DeleteCommand = _Connection.CreateCommand();
                    DeleteCommand.CommandText = "DELETE FROM AddressMatches WHERE CityWord = @CityWord";
                    DeleteCommand.Parameters.AddWithValue("@CityWord", CityWord);
                    DeleteCommand.ExecuteNonQuery();
                    var InsertCommand = _Connection.CreateCommand();
                    InsertCommand.CommandText = "INSERT INTO AddressMatches (CityWord, CityTarget) output INSERTED.MatchId VALUES (@CityWord, @CityTarget)";
                    InsertCommand.Parameters.AddWithValue("@CityWord", CityWord);
                    InsertCommand.Parameters.AddWithValue("@CityTarget", CityTarget);
                    int Id = (int)InsertCommand.ExecuteScalar();
                    _Connection.Close();

                    _AddressMatchList.RemoveAll(c => c.CityWord == CityWord);
                    _AddressMatchList.Add(new FrmMN0213_PRICEMANAGE.AddressMatch
                    {
                        Id = Id,
                        CityWord = CityWord,
                        CityTarget = CityTarget,
                    });
                }
            }
        }

        private void headerGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            int colOriginIndex = ColOrigin.DisplayIndex;
            int colNmberIndex = colNumber.DisplayIndex;
            if(e.ColumnIndex == colNmberIndex)
            {
                e.Value = (e.RowIndex + 1).ToString("N0");
            }
            else if(e.ColumnIndex == colOriginIndex)
            {
                var Model = _BindingList[e.RowIndex];
                if (Model.IsValid)
                {
                    e.CellStyle.ForeColor = headerGridView.DefaultCellStyle.ForeColor;
                    e.CellStyle.SelectionForeColor = headerGridView.DefaultCellStyle.SelectionForeColor;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.OrangeRed;
                    e.CellStyle.SelectionForeColor = Color.OrangeRed;
                }
            }
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClient.SelectedValue == null)
                return;
            Dictionary<int, String> _Source = new Dictionary<int, string>();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT CustomerId, BizNo, SangHo FROM Customers WHERE ClientId = @ClientId AND SalesGubun IN (1, 3)";
                    _Command.Parameters.AddWithValue("@ClientId", (int)cmbClient.SelectedValue);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            _Source.Add(_Reader.GetInt32Z(0), $"{_Reader.GetStringN(2)}[{_Reader.GetStringN(1)}]");
                        }
                    }
                }
                _Connection.Close();
            }
            cmbCustomer.DataSource = new BindingSource(_Source, null);
            cmbCustomer.DisplayMember = "Value";
            cmbCustomer.ValueMember = "Key";
            if (_Source.Count > 0)
                cmbCustomer.SelectedIndex = 0;
        }

        private void txt_ClientSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_ClientSearch.Text) || !ClientList.Any(c => c.Text.Contains(txt_ClientSearch.Text)))
            {
                cmbClient.DataSource = ClientList;
                cmbClient.Update();
                cmbClient.DroppedDown = false;
            }
            else
            {
                cmbClient.DataSource = ClientList.Where(c => c.Text.Contains(txt_ClientSearch.Text)).ToList();
                cmbClient.Update();
                cmbClient.DroppedDown = true;
            }
        }

        private void txt_ClientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (!cmbClient.DroppedDown)
                return;
            if(e.KeyCode == Keys.Up)
            {
                int SelectedIndex = cmbClient.SelectedIndex - 1;
                if (SelectedIndex < 0)
                    SelectedIndex = 0;
                cmbClient.SelectedIndex = SelectedIndex;
                e.Handled = true;
            }
            else if(e.KeyCode == Keys.Down)
            {
                int SelectedIndex = cmbClient.SelectedIndex + 1;
                if (SelectedIndex > cmbClient.Items.Count -1)
                    SelectedIndex = cmbClient.Items.Count - 1;
                cmbClient.SelectedIndex = SelectedIndex;
                e.Handled = true;
            }
            else if(e.KeyCode == Keys.Return)
            {
                var Selected = cmbClient.SelectedItem;
                txt_ClientSearch.Clear();
                cmbClient.SelectedItem = Selected;
                cmbClient.DroppedDown = false;
                e.Handled = true;
            }
        }
    }
}
