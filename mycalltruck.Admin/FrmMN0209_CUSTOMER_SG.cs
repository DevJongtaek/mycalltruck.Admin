using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using mycalltruck.Admin.Class.DataSet;

namespace mycalltruck.Admin
{
    public partial class FrmMN0209_CUSTOMER_SG : Form
    {
        int GridIndex = 0;
        Dictionary<int, Boolean> AccDictionary = new Dictionary<int, bool>();


        public FrmMN0209_CUSTOMER_SG()
        {
            InitializeComponent();
            Image1.Tag = ImageName1;
            Image2.Tag = ImageName2;
            Image3.Tag = ImageName3;
            Image4.Tag = ImageName4;
            Image5.Tag = ImageName5;
        }

        private void FrmMN0209_CARGOCLIENT_Load(object sender, EventArgs e)
        {

            _InitCmb();
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_New.Enabled = false;
            }
            // 운창로지텍만 마감일이 표시된다.
            if(LocalUser.Instance.LogInInformation.ClientId !=290)
            {
                lbl_EndDay.Visible = false;
                cmb_EndDay.Visible = false;
            }
            cmb_Search.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0209_CUSTOMER_Add_SG _Form = new FrmMN0209_CUSTOMER_Add_SG();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
             err.Clear();
            LocalUser.Instance.LogInInformation.LoadClient();
            var Row = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Row == null)
                return;

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }


            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                return;
            }
            else if(!LocalUser.Instance.LogInInformation.Client.AllowMultiCustomer)
            {
                var Query1 =
                      "Select Count(*) From Customers Where BizNo = @BizNo AND CustomerId != @CustomerId AND ClientId = @ClientId";


                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                    cmd1.Parameters.AddWithValue("@CustomerId", Row.CustomerId);
                    cmd1.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
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
            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;

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

            if (txt_Street.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Street, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_PhoneNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (!Regex.Match(txt_Email.Text, @"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$").Success)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Email, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (!String.IsNullOrEmpty(txt_LoginId.Text) && !String.IsNullOrEmpty(txt_Password.Text))
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    _Command.CommandText = "SELECT CustomerId FROM Customers WHERE LoginId = @LoginId AND CustomerId <> @CustomerId";
                    _Command.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                    _Command.Parameters.AddWithValue("@CustomerId", Row.CustomerId);
                    if(_Command.ExecuteScalar() != null)
                    {
                        MessageBox.Show("아이디가 중복되었습니다.");
                        err.SetError(txt_LoginId, "아이디가 중복되었습니다");
                        return;
                    }
                }
            }
            if(!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.CustomerPay)
            {
                int t = 0;
                if(!int.TryParse(txt_Fee.Text, out t) || t < 0 || t > 100)
                {
                    MessageBox.Show("수수료는 0~100 사이의 값을 입력해주십시오.");
                    err.SetError(txt_Fee, "수수료는 0~100 사이의 값을 입력해주십시오.");
                    return;
                }
            }
            UpdateDB();
        }
        private void UpdateDB()
        {
            try
            {

                customersBindingSource.EndEdit();


                var Row = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;


                if(cmb_SalesGubun.SelectedIndex == 1)
                {
                    Row.EndDay = 0;
                }
                else
                {
                    Row.EndDay = (int)cmb_EndDay.SelectedValue;
                }
                customersTableAdapter.Update(Row);
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "거래처", 1), "거래처정보 수정 성공");

                if (dataGridView1.RowCount > 1)
                {
                    GridIndex = customersBindingSource.Position;
                    btn_Search_Click(null, null);
                    dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                }
                else
                {
                    btn_Search_Click(null, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "거래처정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
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
                int Ordercount = 0;
                int Salescount = 0;
                int CustomerPriceCount = 0;
                foreach (DataGridViewRow dRow in deleteRows)
                {
                    //Ordercount += (int)customersTableAdapter.OrdersQuery(int.Parse(dRow.Cells["customerIdDataGridViewTextBoxColumn"].Value.ToString()));
                    //Salescount += (int)customersTableAdapter.SalesQuery(int.Parse(dRow.Cells["customerIdDataGridViewTextBoxColumn"].Value.ToString()));
                    //CustomerPriceCount += (int)customersTableAdapter.CustomerPriceQuery(int.Parse(dRow.Cells["customerIdDataGridViewTextBoxColumn"].Value.ToString()));
                }
                if (Ordercount > 0 || Salescount > 0 || CustomerPriceCount > 0)
                {
                    MessageBox.Show(string.Format("사용중인 데이터 배차관리 : {0}건 \n매출관리 : {1}건 \n탁송업에 : {2}건 이  존재하므로\n이 거래처정보는 삭제할 수 없습니다.",
                        Ordercount, Salescount, CustomerPriceCount), "거래처정보 삭제 실패");
                    return;
                }

                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "거래처", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            try
            {
                customersTableAdapter.Update(clientDataSet.Customers);
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "거래처", 1), "거래처정보 삭제 성공");
                btn_Search_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "거래처정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void _Search()
        {
            try
            {
                var _SelectCommandText =
                @"SELECT  CustomerId, Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, 
                        CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId, PointMethod, Image1, Image2, Image3, Image4, Image5, ISNULL(DriverId, 0) AS DriverId, ISNULL(Fee, 8) AS Fee, LoginId, Password
                        FROM     Customers ";
                clientDataSet.Customers.Clear();
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        String SelectCommandText = _SelectCommandText;

                        List<String> WhereStringList = new List<string>();
                        // 1. 본점/지사
                        if (LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            if (LocalUser.Instance.LogInInformation.IsAgent)
                            {
                                WhereStringList.Add("ClientUserId = @ClientUserId");
                                _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                            }
                            else
                            {
                                WhereStringList.Add("SubClientId = @SubClientId");
                                _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                            }
                        }
                        else if (!LocalUser.Instance.LogInInformation.IsAdmin)
                        {
                            WhereStringList.Add("ClientId = @ClientId");
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        }
                        // 2. 단어 검색
                        if (cmb_Search.SelectedIndex > 0)
                        {
                            switch (cmb_Search.Text)
                            {
                                case "사업자번호":
                                    WhereStringList.Add("REPLACE(BizNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "전화번호":
                                    WhereStringList.Add("REPLACE(PhoneNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "팩스번호":
                                    WhereStringList.Add("REPLACE(FaxNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "핸드폰번호":
                                    WhereStringList.Add("REPLACE(MobileNo,N'-',N'') LIKE '%' + @Text + '%'");
                                    break;
                                case "상호":
                                    WhereStringList.Add("SangHo LIKE '%' + @Text + '%'");
                                    break;
                                case "대표자":
                                    WhereStringList.Add("Ceo LIKE '%' + @Text + '%'");
                                    break;
                                case "담당자":
                                    WhereStringList.Add("ChargeName LIKE '%' + @Text + '%'");
                                    break;
                                default:
                                    break;
                            }
                            _Command.Parameters.AddWithValue("@Text", txt_Search.Text.Replace("-", ""));
                        }
                        if (WhereStringList.Count > 0)
                        {
                            var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                            SelectCommandText += Environment.NewLine + WhereString;
                        }
                        _Command.CommandText = SelectCommandText + " Order by CreateTime DESC";
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            clientDataSet.Customers.Load(_Reader);
                        }
                    }
                    _Connection.Close();
                }
            }
            catch
            {
                MessageBox.Show("조회가 실패하였습니다. 잠시 후에 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            else if (e.ColumnIndex == createTimeDataGridViewTextBoxColumn.Index && e.RowIndex >= 0)
            {

                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Search.Clear();
            if (cmb_Search.SelectedIndex == 0)
            {
                txt_Search.ReadOnly = true;
            }
            else
            {
                txt_Search.ReadOnly = false;
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
            _Search();
        }

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.CUSTOMER);

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = "거래처관리입력양식_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.거래처관리입력양식_Blank))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var _Index = 1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var _Model = ((DataRowView)row.DataBoundItem).Row as ClientDataSet.CustomersRow;
                var _RegNo = _Model.ResgisterNo.Replace("-", "").Trim();
                if(_RegNo.Length > 6)
                {
                    _RegNo = _RegNo.Substring(0, 6) + "-" + _RegNo.Substring(6);
                }
                _Sheet.Cells[RowIndex, 1].Value = _Index.ToString();
                _Sheet.Cells[RowIndex, 2].Value = _Model.BizNo;
                _Sheet.Cells[RowIndex, 3].Value = _Model.SangHo;
                _Sheet.Cells[RowIndex, 4].Value = _Model.Ceo;
                _Sheet.Cells[RowIndex, 5].Value = _Model.Uptae;
                _Sheet.Cells[RowIndex, 6].Value = _Model.Upjong;
                _Sheet.Cells[RowIndex, 7].Value = _Model.Zipcode;
                _Sheet.Cells[RowIndex, 8].Value = _Model.AddressState;
                _Sheet.Cells[RowIndex, 9].Value = _Model.AddressCity;
                _Sheet.Cells[RowIndex, 10].Value = _Model.AddressDetail;
                _Sheet.Cells[RowIndex, 11].Value = _RegNo;
                _Sheet.Cells[RowIndex, 12].Value = _Model.SalesGubun;
                _Sheet.Cells[RowIndex, 13].Value = _Model.Email;
                _Sheet.Cells[RowIndex, 14].Value = _Model.PhoneNo;
                _Sheet.Cells[RowIndex, 15].Value = _Model.ChargeName;
                _Sheet.Cells[RowIndex, 16].Value = _Model.MobileNo;
                RowIndex++;
                _Index++;
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

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == 20)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var Data = ((DataRowView)row.DataBoundItem).Row as ClientDataSet.CustomersRow;
                if (Data != null)
                {
                    if (AccDictionary.ContainsKey(Data.CustomerId) && AccDictionary[Data.CustomerId])
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "O";
                    }
                }
            }
        }

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
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
        #region HELP
        class ErrorModel
        {
            public ErrorModel(Control iControl)
            {
                _Color = iControl.BackColor;
                _Control = iControl;
            }
            public Control _Control { get; set; }
            public Color _Color { get; set; }
        }
        private List<ErrorModel> _ErrorModelList = new List<ErrorModel>();
        private void SetError(Control _Control, String _ErrorText)
        {
            if (!_ErrorModelList.Any(c => c._Control == _Control))
            {
                _ErrorModelList.Add(new ErrorModel(_Control));
                _Control.BackColor = Color.FromArgb(0xff, 0xff, 0xf9, 0xc4);
                if (!(_Control.Tag is ToolTip))
                {
                    var _ToolTip = new ToolTip();
                    _ToolTip.ShowAlways = true;
                    _ToolTip.AutomaticDelay = 0;
                    _ToolTip.InitialDelay = 0;
                    _ToolTip.ReshowDelay = 0;
                    _ToolTip.ForeColor = Color.Red;
                    _ToolTip.BackColor = Color.White;
                    _Control.Tag = new ToolTip();
                }
                ((ToolTip)_Control.Tag).SetToolTip(_Control, _ErrorText);
                ((ToolTip)_Control.Tag).Show(_ErrorText, _Control);
            }
        }
        private void ClearError()
        {
            foreach (var _Model in _ErrorModelList)
            {
                _Model._Control.BackColor = _Model._Color;
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            _ErrorModelList.Clear();
        }
        private void RemoveError(Control _Control)
        {
            if (_ErrorModelList.Any(c => c._Control == _Control))
            {
                var _Model = _ErrorModelList.First(c => c._Control == _Control);
                _Model._Control.BackColor = _Model._Color;
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
                _ErrorModelList.Remove(_Model);
            }
        }
        private void CustomerCode_Add()
        {
            int CustomerCode = 1001;
            while (true)
            {
                if (!CodeCompareTable.Any(c => c == CustomerCode.ToString()))
                {
                    break;
                }
                CustomerCode++;
            }
            txt_Code.Text = CustomerCode.ToString();
        }
        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        private void _InitCmb()
        {
            Dictionary<int, int> EndDay = new Dictionary<int, int>();
            for (int i = 1; i <= 31; i++)
            {


                EndDay.Add(i, i);

            }

            cmb_EndDay.DataSource = new BindingSource(EndDay, null);
            cmb_EndDay.DisplayMember = "Value";
            cmb_EndDay.ValueMember = "Key";


            var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CustomerGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun.DataSource = GubunSourceDataSource;
            cmb_Gubun.DisplayMember = "Name";
            cmb_Gubun.ValueMember = "Value";

            var SalesSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "SalesGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_SalesGubun.DataSource = SalesSourceDataSource;
            cmb_SalesGubun.DisplayMember = "Name";
            cmb_SalesGubun.ValueMember = "Value";

        }
        #endregion

        private List<String> BizNoCompareTable = new List<String>();
        private List<String> CodeCompareTable = new List<String>();

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            //txt_PhoneNo.Text = txt_PhoneNo.Text.Replace("-", "");
            //txt_PhoneNo.SelectionStart = txt_PhoneNo.Text.Length;
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;

        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
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
                _Txt.Text = _S;
            }
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_FaxNo_Leave(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
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
                _Txt.Text = _S;
            }
        }

        private void txt_FaxNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_FaxNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;
        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "");
            _Txt.SelectionStart = _Txt.Text.Length;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmb_SalesGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(LocalUser.Instance.LogInInformation.ClientId == 290)
            {
                if (cmb_SalesGubun.SelectedIndex == 1)
                {
                    lbl_EndDay.Visible = false;
                    cmb_EndDay.Visible = false;

                }
                else
                {
                    lbl_EndDay.Visible = true;
                    cmb_EndDay.Visible = true;

                }
            }
        }

        private void customersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            tabContainer.SelectedTab = BasicPage;

            if (customersBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected != null)
            {
                if(LocalUser.Instance.LogInInformation.ClientId == 290)
                {
                    if (Selected.SalesGubun != 2)
                    {
                        cmb_EndDay.Visible = true;
                        lbl_EndDay.Visible = true;
                        cmb_EndDay.SelectedValue = Selected.EndDay;
                    }
                    else
                    {
                        cmb_EndDay.Visible = false;
                        lbl_EndDay.Visible = false;
                    }
                }
            }
        }

        private void Image1_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected == null)
                return;
            var _sender = ((PictureBox)sender).Name;
            var Id = 0;
            if (Selected[_sender] != DBNull.Value)
                Id = Convert.ToInt32(Selected[_sender]);
            if(Id == 0)
            {
                OpenFileDialog d = new OpenFileDialog();
                d.DefaultExt = "png";
                if(d.ShowDialog() == DialogResult.OK)
                {
                    Bitmap b;
                    Bitmap t;
                    try
                    {
                        b = new Bitmap(d.FileName);
                        if (b.Width < 190 && b.Height < 190)
                        {
                            t = (Bitmap)b.Clone();
                        }
                        else
                        {
                            if (b.Width > b.Height)
                            {
                                if (b.Width > 1000)
                                {
                                    int s = (int)Math.Floor((double)b.Height * 1000d / (double)b.Width);
                                    b = new Bitmap(b, new Size(1000, s));
                                }
                                int s1 = (int)Math.Floor((double)b.Height * 190d / (double)b.Width);
                                t = new Bitmap(b, new Size(190, s1));
                            }
                            else
                            {
                                if (b.Height > 1000)
                                {
                                    int s = (int)Math.Floor((double)b.Width * 1000d / (double)b.Height);
                                    b = new Bitmap(b, new Size(s, 1000));
                                }
                                int s1 = (int)Math.Floor((double)b.Width * 190d / (double)b.Height);
                                t = new Bitmap(b, new Size(s1, 190));
                            }
                        }
                        var Value = new ImageViewModel()
                        {
                            Name = ((TextBox)((PictureBox)sender).Tag).Text,
                        };
                        using (MemoryStream _Stream = new MemoryStream())
                        {
                            EncoderParameters mEncoderParameters = new EncoderParameters(1);
                            mEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                            b.Save(_Stream, GetEncoder(ImageFormat.Png), mEncoderParameters);
                            Value.ImageData64String = Convert.ToBase64String(_Stream.ToArray());
                        }
                        using (MemoryStream _Stream = new MemoryStream())
                        {
                            EncoderParameters mEncoderParameters = new EncoderParameters(1);
                            mEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                            t.Save(_Stream, GetEncoder(ImageFormat.Png), mEncoderParameters);
                            Value.ThumnailData64String = Convert.ToBase64String(_Stream.ToArray());
                        }
                        if (b != null)
                            b.Dispose();

                        var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetCustomerImage"));
                        // var http = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/ImageFromApp/SetCustomerImage"));
                        http.Accept = "application/json";
                        http.ContentType = "application/json; charset=utf-8";
                        http.Method = "POST";

                        string parsedContent = JsonConvert.SerializeObject(Value);
                        UTF8Encoding encoding = new UTF8Encoding();
                        Byte[] buffer = encoding.GetBytes(parsedContent);

                        Stream newStream = http.GetRequestStream();
                        newStream.Write(buffer, 0, buffer.Length);
                        newStream.Close();

                        var response = http.GetResponse();

                        var stream = response.GetResponseStream();
                        var sr = new StreamReader(stream);
                        var content = sr.ReadToEnd();
                        Selected[_sender] = Convert.ToInt32(((JObject)JsonConvert.DeserializeObject(content)).GetValue("ImageReferenceId"));
                        customersTableAdapter.Update(Selected);
                        ((PictureBox)sender).Image = t;
                        if (sender == Image1)
                        {
                            ImageDelete1.Enabled = true;
                            ImageUpdate1.Enabled = true;
                            Image1.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image2)
                        {
                            ImageDelete2.Enabled = true;
                            ImageUpdate2.Enabled = true;
                            Image2.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image3)
                        {
                            ImageDelete3.Enabled = true;
                            ImageUpdate3.Enabled = true;
                            Image3.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image4)
                        {
                            ImageDelete4.Enabled = true;
                            ImageUpdate4.Enabled = true;
                            Image4.Cursor = Cursors.Arrow;
                        }
                        else if (sender == Image5)
                        {
                            ImageDelete5.Enabled = true;
                            ImageUpdate5.Enabled = true;
                            Image5.Cursor = Cursors.Arrow;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("올바른 사진(그림) 파일을 선택하여 주십시오.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void tabContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabContainer.SelectedTab == ImagePage)
            {
                Image1.Image = Properties.Resources.ic_photo_white_48dp_2x;
                Image2.Image = Properties.Resources.ic_photo_white_48dp_2x;
                Image3.Image = Properties.Resources.ic_photo_white_48dp_2x;
                Image4.Image = Properties.Resources.ic_photo_white_48dp_2x;
                Image5.Image = Properties.Resources.ic_photo_white_48dp_2x;
                Image1.Cursor = Cursors.Hand;
                Image2.Cursor = Cursors.Hand;
                Image3.Cursor = Cursors.Hand;
                Image4.Cursor = Cursors.Hand;
                Image5.Cursor = Cursors.Hand;
                ImageName1.Clear();
                ImageName2.Clear();
                ImageName3.Clear();
                ImageName4.Clear();
                ImageName5.Clear();
                ImageDelete1.Enabled = false;
                ImageDelete2.Enabled = false;
                ImageDelete3.Enabled = false;
                ImageDelete4.Enabled = false;
                ImageDelete5.Enabled = false;
                ImageUpdate1.Enabled = false;
                ImageUpdate2.Enabled = false;
                ImageUpdate3.Enabled = false;
                ImageUpdate4.Enabled = false;
                ImageUpdate5.Enabled = false;
                if (customersBindingSource.Current == null)
                    return;
                var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
                if (Selected == null)
                    return;
                if(Selected.Image1 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId="+Selected.Image1.ToString());
                        if(r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName1.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image1.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete1.Enabled = true;
                    ImageUpdate1.Enabled = true;
                    Image1.Cursor = Cursors.Arrow;
                }
                if (Selected.Image2 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image2.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName2.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image2.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete2.Enabled = true;
                    ImageUpdate2.Enabled = true;
                    Image2.Cursor = Cursors.Arrow;
                }
                if (Selected.Image3 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image3.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName3.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image3.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete3.Enabled = true;
                    ImageUpdate3.Enabled = true;
                    Image3.Cursor = Cursors.Arrow;
                }
                if (Selected.Image4 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image4.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName4.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image4.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete4.Enabled = true;
                    ImageUpdate4.Enabled = true;
                    Image4.Cursor = Cursors.Arrow;
                }
                if (Selected.Image5 > 0)
                {
                    using (WebClient mWebClient = new WebClient())
                    {
                        var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/GetThumnail?ImageReferenceId=" + Selected.Image5.ToString());
                        if (r != null)
                        {
                            var J = ((JObject)JsonConvert.DeserializeObject(r));
                            ImageName5.Text = Encoding.UTF8.GetString(Convert.FromBase64String(J.GetValue("Name").ToString()));
                            var Buffer = Convert.FromBase64String(J.GetValue("Thumnail").ToString());
                            using (MemoryStream _Stream = new MemoryStream(Buffer))
                            {
                                Image5.Image = new Bitmap(_Stream);
                            }
                        }
                    }
                    ImageDelete5.Enabled = true;
                    ImageUpdate5.Enabled = true;
                    Image5.Cursor = Cursors.Arrow;
                }
            }
        }

        private void ImageDelete1_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected == null)
                return;
            var ImageReferenceId = 0;
            if (sender == ImageDelete1)
                ImageReferenceId = Selected.Image1;
            if (sender == ImageDelete2)
                ImageReferenceId = Selected.Image2;
            if (sender == ImageDelete3)
                ImageReferenceId = Selected.Image3;
            if (sender == ImageDelete4)
                ImageReferenceId = Selected.Image4;
            if (sender == ImageDelete5)
                ImageReferenceId = Selected.Image5;
            if (ImageReferenceId == 0)
                return;
            if (MessageBox.Show("정말 사진(그림)을 삭제하겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (WebClient mWebClient = new WebClient())
                {
                    var r = mWebClient.DownloadString("http://m.cardpay.kr/ImageFromApp/DeleteCustomImagel?CustomerId=" + Selected.CustomerId.ToString() + "&ImageReferenceId=" + ImageReferenceId.ToString());
                }
                if (sender == ImageDelete1)
                {
                    Selected.Image1 = 0;
                    Image1.Image = Properties.Resources.ic_photo_white_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName1.Clear();
                    ImageDelete1.Enabled = false;
                    ImageUpdate1.Enabled = false;
                }
                if (sender == ImageDelete2)
                {
                    Selected.Image2 = 0;
                    Image2.Image = Properties.Resources.ic_photo_white_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName2.Clear();
                    ImageDelete2.Enabled = false;
                    ImageUpdate2.Enabled = false;
                }
                if (sender == ImageDelete3)
                {
                    Selected.Image3 = 0;
                    Image3.Image = Properties.Resources.ic_photo_white_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName3.Clear();
                    ImageDelete3.Enabled = false;
                    ImageUpdate3.Enabled = false;
                }
                if (sender == ImageDelete4)
                {
                    Selected.Image4 = 0;
                    Image4.Image = Properties.Resources.ic_photo_white_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName4.Clear();
                    ImageDelete4.Enabled = false;
                    ImageUpdate4.Enabled = false;
                }
                if (sender == ImageDelete5)
                {
                    Selected.Image5 = 0;
                    Image5.Image = Properties.Resources.ic_photo_white_48dp_2x;
                    Image1.Cursor = Cursors.Hand;
                    ImageName5.Clear();
                    ImageDelete5.Enabled = false;
                    ImageUpdate5.Enabled = false;
                }
                customersTableAdapter.Update(Selected);
            }
        }

        private void ImageUpdate1_Click(object sender, EventArgs e)
        {
            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as ClientDataSet.CustomersRow;
            if (Selected == null)
                return;
            if (sender == ImageUpdate1)
            {
                if(Selected.Image1 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName1.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image1);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate2)
            {
                if (Selected.Image2 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName2.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image2);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate3)
            {
                if (Selected.Image3 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName3.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image3);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate4)
            {
                if (Selected.Image4 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName4.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image4);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
            else if (sender == ImageUpdate5)
            {
                if (Selected.Image5 > 0)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        SqlCommand _Command = _Connection.CreateCommand();
                        _Command.CommandText = "UPDATE ImageReferences SET Name = @Name WHERE ImageReferenceId = @ImageReferenceId";
                        _Command.Parameters.AddWithValue("@Name", ImageName5.Text);
                        _Command.Parameters.AddWithValue("@ImageReferenceId", Selected.Image5);
                        _Command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void txt_BizNo_Enter(object sender, EventArgs e)
        {
            txt_BizNo.Text = txt_BizNo.Text.Replace("-", "");
        }

        private void txt_BizNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_BizNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_BizNo.Text))
            {
                var _S = txt_BizNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 6)
                {
                    _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                }
                txt_BizNo.Text = _S;
            }
        }

        private void txt_RegisterNo_Enter(object sender, EventArgs e)
        {
            txt_RegisterNo.Text = txt_RegisterNo.Text.Replace("-", "");
        }

        private void txt_RegisterNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_RegisterNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_RegisterNo.Text))
            {
                var _S = txt_RegisterNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 6)
                {
                    _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                }
                txt_RegisterNo.Text = _S;
            }
        }

        private void txt_Fee_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
    }
}
