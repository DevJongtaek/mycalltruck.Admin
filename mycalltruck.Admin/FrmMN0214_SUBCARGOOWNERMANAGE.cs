using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.IO;

using OfficeOpenXml;
using System.Runtime.CompilerServices;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class FrmMN0214_SUBCARGOOWNERMANAGE : Form
    {
        DESCrypt m_crypt = null;
        string Up_Status = string.Empty;
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
            auth = this.GetAuth();
            switch (auth)
            {
                case MenuAuth.None:
                    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                    return;
                case MenuAuth.Read:

                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                   // btn_Excel.Enabled = false;



                    dataGridView1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }
        public FrmMN0214_SUBCARGOOWNERMANAGE()
        {
            m_crypt = new DESCrypt("12345678");

            foreach (var item in SingleDataSet.Instance.AddressReferences)
            {
                if (String.IsNullOrEmpty(item.State) || String.IsNullOrEmpty(item.City) || String.IsNullOrEmpty(item.Street))
                    continue;
                if (!AddressList.Any(c => c.State == item.State && c.City == item.City && c.Street == item.Street))
                {
                    AddressList.Add(new AddressViewModel
                    {
                        State = item.State,
                        City = item.City,
                        Street = item.Street,
                    });
                }
            }


            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            _InitCmbSearch();
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");

        }

        private void FrmMN0204_CARGOOWNERMANAGE_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'clientDataSet.SubClients' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.subClientsTableAdapter.Fill(this.clientDataSet.SubClients);
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                dataGridView1.Columns[1].Visible = false;
               // dataGridView1.Columns[2].Visible = false;
                btn_Excel.Visible = false;
            }
            else
            {
                dataGridView1.Columns[1].Visible = true;
               // dataGridView1.Columns[2].Visible = true;
                btn_Excel.Visible = true;
            }
          
            btn_Search_Click(null, null);
        }

        private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
        private void _InitCmbSearch()
        {
            DataGridViewColumn[] cols = new DataGridViewColumn[] {  nameDataGridViewTextBoxColumn, bizNoDataGridViewTextBoxColumn, cEODataGridViewTextBoxColumn, };
            cmb_Search.Items.Clear();
            DicSearch.Clear();
            cmb_Search.Items.Add("전체");

            foreach (var item in cols)
            {

                cmb_Search.Items.Add(item.HeaderText);
                if (item.DataPropertyName == null || item.DataPropertyName == "")
                {
                    DicSearch.Add(item.HeaderText, "'" + item.Name);
                }
                else
                {
                    DicSearch.Add(item.HeaderText, item.DataPropertyName);
                }
            }
            if (cmb_Search.Items.Count > 0) cmb_Search.SelectedIndex = 0;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();
            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");
                return;

            }
            else
            {
                var Query1 =
                       "Select Count(*) From Clients Where BizNo = @BizNo AND ClientId != @ClientId";
                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                    cmd1.Parameters.AddWithValue("@ClientId", ClientNoId.Value);
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }
                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show("사업자번호가 중복되었습니다.!!. 다른사업자 사업자번호를 입력해주십시오.");
                    err.SetError(txt_BizNo, "사업자번호가 중복되었습니다!!");
                    return;
                }
            }
            if (txt_CEO.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Uptae.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Upjong.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_PhoneNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Zip.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            Up_Status = "Update";
            int _rows = UpdateDB(Up_Status);
        }

        private int UpdateDB(string Status)
        {
            int _rows = 0;
            try
            {
                subClientsBindingSource.EndEdit();
                var Row = ((DataRowView)subClientsBindingSource.Current).Row as ClientDataSet.SubClientsRow;

                if (Status == "Update")
                {
                    UpdateAndLoad(Row);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "지점", 1), "지점정보 수정 성공");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "지점정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }
        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (dataGridView1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }
                int _DriverCount = 0;
                int _OrderCount = 0;
                int _TradeCount = 0;
                int _SaleManageCount = 0;
                int _UserCount = 0;
                foreach (DataGridViewRow dRow in deleteRows)
                {
                    var _SubClientId = (((DataRowView)dRow.DataBoundItem).Row as ClientDataSet.SubClientsRow).SubClientId;
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _DriverCountCommand = _Connection.CreateCommand())
                        using (SqlCommand _OrderCountCommand = _Connection.CreateCommand())
                        using (SqlCommand _TradeCountCommand = _Connection.CreateCommand())
                        using (SqlCommand _SaleManageCountCommand = _Connection.CreateCommand())
                        using (SqlCommand _UserCountCommand = _Connection.CreateCommand())
                        {
                            _DriverCountCommand.CommandText = "SELECT COUNT(*) FROM Drivers WHERE SubClientId = @SubClientId";
                            _DriverCountCommand.Parameters.AddWithValue("@SubClientId", _SubClientId);
                            _DriverCount += (int)_DriverCountCommand.ExecuteScalar();
                            _OrderCountCommand.CommandText = "SELECT COUNT(*) FROM Orders WHERE SubClientId = @SubClientId";
                            _OrderCountCommand.Parameters.AddWithValue("@SubClientId", _SubClientId);
                            _OrderCount += (int)_OrderCountCommand.ExecuteScalar();
                            _TradeCountCommand.CommandText = "SELECT COUNT(*) FROM Trades WHERE SubClientId = @SubClientId";
                            _TradeCountCommand.Parameters.AddWithValue("@SubClientId", _SubClientId);
                            _TradeCount += (int)_TradeCountCommand.ExecuteScalar();
                            _SaleManageCountCommand.CommandText = "SELECT COUNT(*) FROM SalesManage WHERE SubClientId = @SubClientId";
                            _SaleManageCountCommand.Parameters.AddWithValue("@SubClientId", _SubClientId);
                            _SaleManageCount += (int)_SaleManageCountCommand.ExecuteScalar();
                            _UserCountCommand.CommandText = "SELECT COUNT(*) FROM ClientUsers WHERE SubClientId = @SubClientId";
                            _UserCountCommand.Parameters.AddWithValue("@SubClientId", _SubClientId);
                            _UserCount += (int)_UserCountCommand.ExecuteScalar();
                        }
                        _Connection.Close();
                    }
                }
                if (_DriverCount > 0 || _OrderCount > 0 || _TradeCount > 0 || _SaleManageCount > 0 || _UserCount > 0)
                {
                    MessageBox.Show(string.Format("사용중인 n차주관리 : {0}건 \n배차관리 : {1}건\n매입관리 : {2}건\n매출관리 : {3}건\n사용자ID : {4}건 이  존재하므로\n이 화주정보는 삭제할 수 없습니다.",
                        _DriverCount, _OrderCount, _TradeCount, _SaleManageCount, _UserCount), "지점정보 삭제 실패");
                    return;
                }



                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "지점", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }

                subClientsTableAdapter.Update(clientDataSet);
            }
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "화주", deleteRows.Count), "화주 삭제 성공");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btn_Search_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btn_New_Click(object sender, EventArgs e)
        {

            FrmMN0214_SUBCARGOOWNERMANAGE_Add _Form = new FrmMN0214_SUBCARGOOWNERMANAGE_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();

            btn_Search_Click(null, null);

        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            DataLoad();
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            DataLoad();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == dataGridViewTextBoxColumn1)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.SubClientsRow;
                e.Value = Selected.AddressState + " " + Selected.AddressCity + " " + Selected.AddressDetail;
            }
        }

        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;


        private void btn_Excel_Click(object sender, EventArgs e)
        {
            //fileString = string.Format("회원정보EXCEL");
            //title = "회원정보";

            //ieExcel = Properties.Resources.회원정보1;





            //if (dataGridView1.RowCount == 0)
            //{
            //    MessageBox.Show("내보낼 회원정보 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            //if (dataGridView1.RowCount > 0)
            //{
            //    pnProgress.Visible = true;
            //    bar.Value = 0;
            //    Thread t = new Thread(new ThreadStart(() =>
            //    {
            //        dataGridView1.ExportExistExcel2_xlsx(title, fileString, bar, true, ieExcel, 2, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            //        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

            //        //mycalltruck.Admin.Class.Extensions.Excel_Epplus.Ep_FileExport(dataGridView1, title, fileString, bar, true, ieExcel, 2);
            //        //pnProgress.Invoke(new Action(() => pnProgress.Visible = false));


            //    }));
            //    t.SetApartmentState(ApartmentState.STA);
            //    t.Start();
            //}
            //else
            //{
            //    MessageBox.Show("엑셀로 내보낼 회원정보 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
            //        Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            ExcelExport();

        }
        private void ExcelExport()
        {

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {

                di.Create();
            }
            var fileString = "회원정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }


            List<ExcelModel> _List = new List<ExcelModel>();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
              //  using (SqlCommand _ExtraCommand = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT
                                a.Code,a.Name,a.Status,a.BizNo,a.Ceo,a.PGGubun,a.LGD_MID,a.Email,a.LoginId,a.Password,a.PhoneNo,a.MobileNo,Convert(varchar,a.CreateDate,111) as CreateDate,a.BizType,b.Name,a.Zipcode,a.AddressState +' '+ a.AddressCity + ' ' +a. AddressDetail as Address


                        FROM     Clients AS a join Dealers as b  ON a.Dealerid = b.DealerId
                        WHERE  (a.PGGubun = @PGGubun OR
                       @PGGubun = 0) AND (a.BizType = @BizType OR
                       @BizType = 0) AND (a.Status = @Status OR
                       @Status = 0) AND (@SearchType = 0) OR
                       (@SearchType = 1) AND (a.Code LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 2) AND (a.Name LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 3) AND (REPLACE(a.BizNo, '-', '') LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 4) AND (a.LoginId LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 5) AND (a.CEO LIKE '%' + @SearchText + '%') OR
                       (@SearchType = 6) AND (ISNULL(a.LGD_MID, N'') LIKE '%' + @SearchText + '%')
                        ORDER BY a.ClientId DESC";
                    _Command.Parameters.AddWithValue("@SearchType", cmb_Search.SelectedIndex.ToString());
                    _Command.Parameters.AddWithValue("@SearchText", txt_Search.Text);
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        _List.AddRange(_GetDataForExcelExport(_Reader));
                    }
                   
                }
                _Connection.Close();
            }
            if (_List.Count == 0)
            {
                MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.회원정보1))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            for (int i = 0; i < _List.Count; i++)
            {
                var _Model = _List[i];
                _Sheet.Cells[RowIndex, 1].Value = (_List.Count-i).ToString();
                _Sheet.Cells[RowIndex, 2].Value = _Model.SCdoe;
                _Sheet.Cells[RowIndex, 3].Value = _Model.SName;
                _Sheet.Cells[RowIndex, 4].Value = _Model.SSerivice;
                _Sheet.Cells[RowIndex, 5].Value = _Model.SBizNo;
                _Sheet.Cells[RowIndex, 6].Value = _Model.SCeo;
                _Sheet.Cells[RowIndex, 7].Value = _Model.SPG;
                _Sheet.Cells[RowIndex, 8].Value = _Model.SPgId;
                _Sheet.Cells[RowIndex, 9].Value = _Model.SEmail;
                _Sheet.Cells[RowIndex, 10].Value = _Model.SLoginId;
                _Sheet.Cells[RowIndex, 11].Value = _Model.SPassWord;
                _Sheet.Cells[RowIndex, 12].Value = _Model.SMobileNo;
                _Sheet.Cells[RowIndex, 13].Value = _Model.SPhoneNo;
                _Sheet.Cells[RowIndex, 14].Value = _Model.SCreateDate;
                _Sheet.Cells[RowIndex, 15].Value = _Model.SBizGubun;
                _Sheet.Cells[RowIndex, 16].Value = _Model.SDealerName;
                _Sheet.Cells[RowIndex, 17].Value = _Model.SZipCode;
                _Sheet.Cells[RowIndex, 18].Value = _Model.SAddress;

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
        private List<ExcelModel> _GetDataForExcelExport(SqlDataReader _Reader)
        {
            List<ExcelModel> r = new List<ExcelModel>();
            while (_Reader.Read())
            {
                //SingleDataSet.Instance.StaticOptions
                r.Add(new ExcelModel
                {
                   // S_Idx = _Reader.GetStringN(0),
                    SCdoe = _Reader.GetStringN(0),
                    SName = _Reader.GetStringN(1),
                    SSerivice = GetTextFromStaticOption("Status", _Reader.GetInt32Z(2)),
                    SBizNo = _Reader.GetStringN(3),
                    SCeo = _Reader.GetStringN(4),
                    SPG = GetTextFromStaticOption("PGgubun", _Reader.GetInt32Z(5)),
                    SPgId = _Reader.GetStringN(6),
                    SEmail= _Reader.GetStringN(7),
                    SLoginId = _Reader.GetStringN(8),
                    SPassWord = _Reader.GetStringN(9),
                    SMobileNo = _Reader.GetStringN(10),
                    SPhoneNo = _Reader.GetStringN(11),
                    SCreateDate = _Reader.GetStringN(12),
                    SBizGubun = GetTextFromStaticOption("ClientType", _Reader.GetInt32Z(13)),
                    SDealerName = _Reader.GetStringN(14),
                    SZipCode = _Reader.GetStringN(15),
                    SAddress = _Reader.GetStringN(16),
                  
                });
            }
            return r;
        }
        private String GetTextFromStaticOption(String Div, int Value)
        {
            if (SingleDataSet.Instance.StaticOptions.Any(c => c.Div == Div && c.Value == Value))
            {
                return SingleDataSet.Instance.StaticOptions.First(c => c.Div == Div && c.Value == Value).Name;
            }
            else
            {
                return "";
            }
        }

        class ExcelModel : INotifyPropertyChanged
        {
            private String _S_Idx = "";
            private String _SCdoe = "";
            private String _SName = "";
            private String _SSerivice = "";
            private String _SBizNo = "";
            private String _SCeo = "";
            private String _SPG = "";
            private String _SPgId = "";
            private String _SEmail = "";
            private String _SLoginId = "";
            private String _SPassWord = "";
            private String _SMobileNo = "";
            private String _SPhoneNo = "";
            private String _SCreateDate = "";
            private String _SBizGubun = "";
            private String _SDealerName = "";
            private String _SAddress = "";
            private String _SZipCode = "";

            public string S_Idx
            {
                get
                {
                    return _S_Idx;
                }

                set
                {
                    SetField(ref _S_Idx, value);
                }
            }

            public string  SCdoe
            {
                get
                {
                    return _SCdoe;
                }

                set
                {
                    SetField(ref _SCdoe, value);
                }
            }

            public string SName
            {
                get
                {
                    return _SName;
                }

                set
                {
                    SetField(ref _SName, value);
                }
            }

            public string  SSerivice
            {
                get
                {
                    return _SSerivice;
                }

                set
                {
                    SetField(ref _SSerivice, value);
                }
            }

            public string  SBizNo
            {
                get
                {
                    return _SBizNo;
                }

                set
                {
                    SetField(ref _SBizNo, value);
                }
            }

            public string SCeo
            {
                get
                {
                    return _SCeo;
                }

                set
                {
                    SetField(ref _SCeo, value);
                }
            }

            public string SPG
            {
                get
                {
                    return _SPG;
                }

                set
                {
                    SetField(ref _SPG, value);
                }
            }
            public string SPgId
            {
                get
                {
                    return _SPgId;
                }

                set
                {
                    SetField(ref _SPgId, value);
                }
            }
           
            public string SEmail
            {
                get
                {
                    return _SEmail;
                }

                set
                {
                    SetField(ref _SEmail, value);
                }
            }


            public string SLoginId
            {
                get
                {
                    return _SLoginId;
                }

                set
                {
                    SetField(ref _SLoginId, value);
                }
            }
            public string SPassWord
            {
                get
                {
                    return _SPassWord;
                }

                set
                {
                    SetField(ref _SPassWord, value);
                }
            }
            public string SMobileNo
            {
                get
                {
                    return _SMobileNo;
                }

                set
                {
                    SetField(ref _SMobileNo, value);
                }
            }
            public string SPhoneNo
            {
                get
                {
                    return _SPhoneNo;
                }

                set
                {
                    SetField(ref _SPhoneNo, value);
                }
            }

            public string SCreateDate
            {
                get
                {
                    return _SCreateDate;
                }

                set
                {
                    SetField(ref _SCreateDate, value);
                }
            }

          




            public string SBizGubun
            {
                get
                {
                    return _SBizGubun;
                }

                set
                {
                    SetField(ref _SBizGubun, value);
                }
            }

            public string SDealerName
            {
                get
                {
                    return _SDealerName;
                }

                set
                {
                    SetField(ref _SDealerName, value);
                }
            }
            public string SZipCode
            {
                get
                {
                    return _SZipCode;
                }

                set
                {
                    SetField(ref _SZipCode, value);
                }
            }
            public string SAddress
            {
                get
                {
                    return _SAddress;
                }

                set
                {
                    SetField(ref _SAddress, value);
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                txt_Zip.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_State.Text = ss[0];
                txt_City.Text = ss[1];
                txt_Street.Text = String.Join(" ", ss.Skip(2));
            }
        }

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            txt_MobileNo.Text = txt_MobileNo.Text.Replace(",", "");
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_MobileNo.Text))
            {
                var _S = txt_MobileNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 7)
                {
                    _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                }
                if (_S.Length > 12)
                {
                    _S = _S.Replace("-", "");
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                }
                txt_MobileNo.Text = _S;
            }
        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            txt_PhoneNo.Text = txt_PhoneNo.Text.Replace(",", "");
        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_PhoneNo.Text))
            {
                var _S = txt_PhoneNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
                {
                    if (_S.Length > 2)
                    {
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                    }
                    if (_S.Length > 6)
                    {
                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                    }
                    if (_S.Length > 11)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                    }
                }
                else
                {
                    if (_S.Length > 3)
                    {
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                    }
                    if (_S.Length > 7)
                    {
                        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                txt_PhoneNo.Text = _S;
            }
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        #region STORAGE
        private string GetSelectCommand()
        {
            String SelectCommandText =
                    @"SELECT  SubClientId, PhoneNo, Name, BizNo, CEO, Uptae, Upjong, Email, MobileNo, CreateDate, ZipCode, AddressDetail, ShopID, ShopPW, Code, AddressState, AddressCity, ClientId
                    FROM     SubClients";
            return SelectCommandText;
        }
        private void DataLoad()
        {
            clientDataSet.SubClients.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();
                    // 1. 본점/지사
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        WhereStringList.Add("SubClientId = @SubClientId");
                        _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                    }
                    WhereStringList.Add("ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    // 2. 단어 검색
                    if (cmb_Search.SelectedIndex > 0)
                    {
                        switch (cmb_Search.Text)
                        {
                            case "지점코드":
                                WhereStringList.Add("Code LIKE '%' + @Text + '%'");
                                break;
                            case "지점명":
                                WhereStringList.Add("Name LIKE '%' + @Text + '%'");
                                break;
                            case "사업자번호":
                                WhereStringList.Add("REPLACE(BizNo,N'-',N'') LIKE '%' + @Text + '%'");
                                break;
                            case "대표자":
                                WhereStringList.Add("CEO LIKE '%' + @Text + '%'");
                                break;
                            default:
                                break;
                        }
                        _Command.Parameters.AddWithValue("@Text", txt_Search.Text);
                    }
                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        clientDataSet.SubClients.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }
        private void UpdateAndLoad(ClientDataSet.SubClientsRow Row)
        {
            subClientsTableAdapter.Update(Row);
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();
                    SelectCommandText += Environment.NewLine + "WHERE SubClientId = @SubClientId";
                    _Command.Parameters.AddWithValue("@SubClientId", Row.SubClientId);
                    _Command.CommandText = SelectCommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        clientDataSet.SubClients.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }
        #endregion


    }
}
