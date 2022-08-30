using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0207_SALESMANAGE : Form
    {
        string UP_Status = string.Empty;
        int GridIndex = 0;
        public FrmMN0207_SALESMANAGE()
        {
            InitializeComponent();
            _InitCmbSearch();
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0207_SALESMANAGE_Add _Form = new FrmMN0207_SALESMANAGE_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(
               
            );
            btn_Search_Click(null, null);
        }

        private void FrmMN0207_SALESMANAGE_Load(object sender, EventArgs e)
        {
            this.dealersTableAdapter.Fill(this.cMDataSet.Dealers);
            _InitCmb();
        }
        private void _InitCmb()
        {
            var statusSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Status.DataSource = statusSourceDataSource;
            cmb_Status.DisplayMember = "Name";
            cmb_Status.ValueMember = "value";

            var BankSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BankGubun").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Bank.DataSource = BankSourceDataSource;
            cmb_Bank.DisplayMember = "Name";
            cmb_Bank.ValueMember = "Name";

            var BizNameDataSource = cMDataSet.Dealers.Where(c => c.BizNo.Length == 12).Select(c => new { c.DealerId, c.Name }).ToArray();
            cmb_BizName.DataSource = BizNameDataSource;
            cmb_BizName.DisplayMember = "Name";
            cmb_BizName.ValueMember = "DealerId";

             var SearchBizDataSource = cMDataSet.Dealers.Where(c => c.BizNo.Length == 12).Select(c => new { c.DealerId, c.Name }).ToArray();
             //cmb_SearchBiz.DataSource = SearchBizDataSource;
             //cmb_SearchBiz.DisplayMember = "Name";
             //cmb_SearchBiz.ValueMember = "DealerId";

             Dictionary<string, string> SearchBiz = new Dictionary<string, string>();

             SearchBiz.Add("%", "전체");

         
             foreach (var item in SearchBizDataSource)
             {
                 SearchBiz.Add(item.DealerId.ToString(),item.Name);
             }
             cmb_SearchBiz.DataSource = new BindingSource(SearchBiz, null);
             cmb_SearchBiz.DisplayMember = "Value";
             cmb_SearchBiz.ValueMember = "Key";
        }
        private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
        private void _InitCmbSearch()
        {
            DataGridViewColumn[] cols = new DataGridViewColumn[]{codeDataGridViewTextBoxColumn,nameDataGridViewTextBoxColumn,bizNoDataGridViewTextBoxColumn,loginIdDataGridViewTextBoxColumn,cEODataGridViewTextBoxColumn
             };
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


            //var statusSourceSDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status").OrderBy(c => c.Seq).ToArray();

            //cmb_Sstatus.Items.Clear();
            //cmb_Sstatus.Items.Add("전체");
            //cmb_Sstatus.Items.AddRange(statusSourceSDataSource);
            //cmb_Sstatus.DisplayMember = "Name";
            //cmb_Sstatus.ValueMember = "value";

            //if (cmb_Sstatus.Items.Count > 0)
            //{
            //    cmb_Sstatus.SelectedIndex = 0;
            //}


            var statusSourceSDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status" && c.Value != 0).OrderBy(c => c.Seq).ToArray();




            Dictionary<string, string> SearchSstatus = new Dictionary<string, string>();

            SearchSstatus.Add("%", "전체");


            foreach (var item in statusSourceSDataSource)
            {
                SearchSstatus.Add(item.Value.ToString(), item.Name);
            }
            cmb_Sstatus.DataSource = new BindingSource(SearchSstatus, null);
            cmb_Sstatus.DisplayMember = "Value";
            cmb_Sstatus.ValueMember = "Key";



            var statusSourceSDataSource1 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();

            statusDataGridViewTextBoxColumn.DataSource = statusSourceSDataSource1;
            statusDataGridViewTextBoxColumn.DisplayMember = "Name";
            statusDataGridViewTextBoxColumn.ValueMember = "value";


            //var bankSourceSDataSource1 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BankGubun").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //bankNameDataGridViewTextBoxColumn.DataSource = bankSourceSDataSource1;
            //bankNameDataGridViewTextBoxColumn.DisplayMember = "Name";
            //bankNameDataGridViewTextBoxColumn.ValueMember = "value";

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();


            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            //if (txt_BizNo.Text.Length != 12)
            //{
            //    MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
            //    err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");
            //    return;

            //}

            if(!String.IsNullOrEmpty(txt_BizNo.Text))
            {
                var Query1 =
                     "Select Count(*) From Dealers Where BizNo = @BizNo AND DealerId != @DealerId";


                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    cmd1.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                    cmd1.Parameters.AddWithValue("@DealerId", DealerNoId.Value);
                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }
                    //if (!IsDuplicated)
                    //{
                    //    SqlCommand cmd2 = cn.CreateCommand();
                    //    cmd2.CommandText = Query2;
                    //    cmd2.Parameters.AddWithValue("@LoginId", txt_CarNo.Text);
                    //    if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
                    //    {
                    //        IsDuplicated = true;
                    //    }
                    //}
                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show("사업자(주민)번호가 중복되었습니다.!!. 다른사업자(주민)번호를 입력해주십시오.");
                    err.SetError(txt_BizNo, "사업자(주민)번호가 중복되었습니다!!");
                    
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
            if (txt_LoignId.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_LoignId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_Password.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            if (txt_MobileNo.Text.Length < 10)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }
            
            if (String.IsNullOrEmpty(txt_ZipCode.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_ZipCode, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }



            if (String.IsNullOrEmpty(txt_Addr.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Addr, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (String.IsNullOrEmpty(txt_AddrDetail.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AddrDetail, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;
            }
            if (cmb_Bank.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_Bank, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }



            if (txt_AccountOwner.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AccountOwner, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            if (txt_AccountNo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AccountNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;
            }

            UP_Status = "Update";
            int _rows = UpdateDB(UP_Status);
        }

        private int UpdateDB(string Status)
        {
            int _rows = 0;
            try
            {
                
                dealersBindingSource.EndEdit();
              
                
                var Row = ((DataRowView)dealersBindingSource.Current).Row as CMDataSet.DealersRow;

                if (Status == "Update")
                {
                    dealersTableAdapter.Update(Row);
                 


                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "영업딜러", 1), "영업딜러정보 수정 성공");

                    if (dataGridView1.RowCount > 1)
                    {
                        GridIndex = dealersBindingSource.Position;
                        btn_Search_Click(null, null);
                        dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                    }
                    else
                    {
                        btn_Search_Click(null, null);
                    }
                }
                else if (Status == "Delete")
                {

                    dealersTableAdapter.Update(cMDataSet.Dealers);
                   


                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "영업딜러", 1), "영업딜러정보 삭제 성공");

                    btn_Search_Click(null, null);
                }
               

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "영업딜러정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }


        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Code_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txt_Code.Text);
            if (!ismatch)
            {
                MessageBox.Show("숫자만 입력해 주세요.");
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void _Search()
        {


           // dealersTableAdapter.Fill(SingleDataSet.Instance.Dealers);

            dealersTableAdapter.Fill(cMDataSet.Dealers);


            string _FilterString = string.Empty;
            string _cmbSearchString = string.Empty;
        
            string _StatusSearchString = string.Empty;
            string _BizSearchString = string.Empty;
          

            try
            {
                if (cmb_Search.Text == "대리점코드")
                {
                    var codes = cMDataSet.Dealers.Where(c => c.Code.Contains(txt_Search.Text));
                    if (codes.Count() > 0)
                    {
                        string filter = string.Format("Code Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                    }
                    else
                        _cmbSearchString = "Code = 'Code'";
                }

                else if (cmb_Search.Text == "상호(성명)")
                {
                    var codes = cMDataSet.Dealers.Where(c => c.Name.Contains(txt_Search.Text));
                    if (codes.Count() > 0)
                    {
                        string filter = string.Format("Name Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                    }
                    else
                        _cmbSearchString = "Name = 'Name'";
                }
                else if (cmb_Search.Text == "사업자(주민)번호")
                {
                    var codes = cMDataSet.Dealers.Where(c => c.BizNo.Contains(txt_Search.Text));
                    if (codes.Count() > 0)
                    {
                        string filter = string.Format("BizNo Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                    }
                    else
                        _cmbSearchString = "BizNo = 'BizNo'";
                }
                else if (cmb_Search.Text == "아이디")
                {
                    var codes = cMDataSet.Dealers.Where(c => c.LoginId.ToString().Contains(txt_Search.Text));
                    if (codes.Count() > 0)
                    {
                        string filter = string.Format("LoginId Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                    }
                    else
                        _cmbSearchString = "LoginId = 'LoginId'";
                }
                else if (cmb_Search.Text == "대표자")
                {
                    var codes = cMDataSet.Dealers.Where(c => c.CEO.Contains(txt_Search.Text));
                    if (codes.Count() > 0)
                    {
                        string filter = string.Format("CEO Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                    }
                    else
                        _cmbSearchString = "CEO = 'CEO'";
                }
                
                _FilterString += _cmbSearchString;

                try
                {

                    if (cmb_Sstatus.SelectedIndex != 0)
                    {
                        string StatusCode = cmb_Sstatus.SelectedValue.ToString();
                        _StatusSearchString = "Status ='" + StatusCode + "'";
                    }
                }
                catch
                {
                    _StatusSearchString = "";
                }




                if (_FilterString != string.Empty && _StatusSearchString != string.Empty)
                {
                    _FilterString += " AND " + _StatusSearchString;
                }
                else
                {
                    _FilterString += _StatusSearchString;
                }



             

                if(cmb_SearchBiz.SelectedIndex != 0)
                {

                    //var codes = cMDataSet.Dealers.Where(c => c.Name.Contains(txt_SearchBiz.Text)).ToArray();

                    //if (codes.Count() > 0)
                    //{
                    //    string filter = String.Format("BizName IN ('{0}'", codes[0].DealerId);
                    //    for (int i = 1; i < codes.Count(); i++)
                    //    {
                    //        filter += string.Format(", '{0}'", codes[i]);
                    //    }
                    //    filter += ")";
                    //    _BizSearchString = filter;
                    //}

                    //else
                    //    _BizSearchString = "";

                    string BizCode = cmb_SearchBiz.SelectedValue.ToString();
                        _BizSearchString = "BizName = '" + BizCode + "'";
                }
                if (_FilterString != string.Empty && _BizSearchString != string.Empty)
                {
                    _FilterString += " AND " + _BizSearchString;
                }
                else
                {
                    _FilterString += _BizSearchString;
                }
             
                    

                try
                {

                    dealersBindingSource.Filter = _FilterString;
                }
                catch
                {
                    btn_Search_Click(null, null);
                }
            }
            catch
            {
            }


        }

        private void btn_Logo_Click(object sender, EventArgs e)
        {
            //if (clientsBindingSource.Current == null || dataGridView1.SelectedCells.Count == 0 || dataGridView1.SelectedCells[0].RowIndex == -1)
            //{

            //}

            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;
            txt_LogoImage.Text = dlgOpen.FileName;
            picLogo.ImageLocation = dlgOpen.FileName;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                int dealercount = 0;
                int Clientscount = 0;
                foreach (DataGridViewRow dRow in deleteRows)
                {
                    
                    dealercount += (int)dealersTableAdapter.GetBYDealer(dRow.Cells["dealerIdDataGridViewTextBoxColumn"].Value.ToString());
                    Clientscount += (int)dealersTableAdapter.GetBYClients(Int32.Parse(dRow.Cells["dealerIdDataGridViewTextBoxColumn"].Value.ToString()));
                }
                if (dealercount > 0  || Clientscount > 0)
                {
                    MessageBox.Show(string.Format("사용중인 데이터 \n 딜러별잠재고객관리{0}건 \n 회원정보{1}건이  존재하므로\n이 영업딜러는 삭제할 수 없습니다.",
                        dealercount, Clientscount), "영업딜러 삭제 실패");
                    return;
                }


               



                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "영업딜러", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            UP_Status = "Delete";
            int _rows = UpdateDB(UP_Status);
            //MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "영업딜러", deleteRows.Count));
        }

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


        private void btnZip_Click(object sender, EventArgs e)
        {
            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {

                txt_ZipCode.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_Addr.Text = ss[0] + " " + ss[1]+ String.Join(" ", ss.Skip(2));
              
                //txt_AddrDetail.Text = String.Join(" ", ss.Skip(2));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            picLogo.Image = null;
            txt_LogoImage.Text = "";
        }

        private void dealersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            

            DataRowView view = dealersBindingSource.Current as DataRowView;
            if (view == null) return;
            CMDataSet.DealersRow row = view.Row as CMDataSet.DealersRow;
            if (row == null) return;

           // var  BizNo = SingleDataSet.Instance.Dealers.Where(c => c.Code == row.Code).Select(c => c.BizNo);

            var Selected = ((DataRowView)dealersBindingSource.Current).Row as CMDataSet.DealersRow;
            if (Selected.BizNo.Length == 14)
            {

                txt_BizNo.Mask = "999999-9999999";
                cmb_bizStatus.SelectedIndex = 0;
                txt_BizNo.Text = Selected.BizNo;
                cmb_BizName.Visible = true;
              //  cmb_BizName.SelectedValue = Selected.BizName;

            }
            else
            {

                txt_BizNo.Mask = "999-99-99999";
                cmb_bizStatus.SelectedIndex = 1;
                txt_BizNo.Text = Selected.BizNo;
                cmb_BizName.Visible = false;
            }
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            cmb_Sstatus.SelectedIndex = 0;
            cmb_SearchBiz.SelectedIndex = 0;
            _Search();
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void cmb_bizStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_bizStatus.SelectedIndex == 0)
            {

                txt_BizNo.Mask = "999999-9999999";
                cmb_BizName.Visible = true;
            }
            else
            {

                txt_BizNo.Mask = "999-99-99999";
                cmb_BizName.Visible = false;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }

            else if (dataGridView1.Columns[e.ColumnIndex] == BiznameDataGridViewTextBoxColumn)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DealersRow;

                if (Selected.BizNo.Length == 14)
                {
                    var Query = cMDataSet.Dealers.Where(c => c.DealerId == Selected.BizName).ToArray();

                    if (Query.Any())
                    {
                        e.Value = Query.First().Name;
                    }
                    else
                    {
                        e.Value = "";
                    }
                  
                }
            }
        }

        private void txt_SearchBiz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

       

       
    }
}
