using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0210_AccountOptions : Form
    {

        string Up_Status = string.Empty;
      
        int GridIndex = 0;



        public FrmMN0210_AccountOptions()
        {
            InitializeComponent();
        }

        private void FrmMN0210_AccountOptions_Load(object sender, EventArgs e)
        {
            _InitCmb();
            cmb_Search.SelectedIndex = 0;
            cmb_gubunS.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }
        private void _InitCmb()
        {


            var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun.DataSource = GubunSourceDataSource;
            cmb_Gubun.DisplayMember = "Name";
            cmb_Gubun.ValueMember = "Name";


            var GubunSourceDataSource2 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun2" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun2.DataSource = GubunSourceDataSource2;
            cmb_Gubun2.DisplayMember = "Name";
            cmb_Gubun2.ValueMember = "Value";




        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0210_AccountOptions_Add _Form = new FrmMN0210_AccountOptions_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();

         

            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;

            }


            //if (txt_Remark.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_Remark, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return;
            //}
           

            Up_Status = "Update";
            UpdateDB(Up_Status);
        }
        private void UpdateDB(string Status)
        {
            try
            {
                chargeAccountsBindingSource.EndEdit();
                var Row = ((DataRowView)chargeAccountsBindingSource.Current).Row as CMDataSet.ChargeAccountsRow;
                if (Status == "Update")
                {

                    chargeAccountsTableAdapter.Update(Row);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "계정과목", 1), "계정과목 수정 성공");

                    if (dataGridView1.RowCount > 1)
                    {
                        GridIndex = chargeAccountsBindingSource.Position;
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
                    chargeAccountsTableAdapter.Update(cMDataSet.ChargeAccounts);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "계정과목", 1), "계정과목 삭제 성공");
                    btn_Search_Click(null, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "계정과목 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                int Customercount = 0;
                //foreach (DataGridViewRow dRow in deleteRows)
                //{

                //  //  Customercount += (int)driverAddTableAdapter.GetByDriverAdd(dRow.Cells["DriverId"].Value.ToString(), dRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString());

                //}
                //if (Customercount > 0)
                //{
                //    MessageBox.Show(string.Format("사용중인 데이터 {0}건이  존재하므로\n이 기사정보는 삭제할 수 없습니다.",
                //        DriverAddcount), "기사정보 삭제 실패");
                //    return;
                //}



                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "계정과목", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            Up_Status = "Delete";
            UpdateDB(Up_Status);
        }
        private void _Search()
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                chargeAccountsTableAdapter.FillByClient(cMDataSet.ChargeAccounts, LocalUser.Instance.LogInInformation.ClientId);
            }
            else
            {
                chargeAccountsTableAdapter.Fill(cMDataSet.ChargeAccounts);
            }
            string _FilterString = string.Empty;
            string _cmbSearchString = string.Empty;

            string _GubunSearchString = string.Empty;


            try
            {

                if (cmb_Search.Text == "계정과목")
                {

                    string filter = string.Format("Name Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;


                }

              

                _FilterString += _cmbSearchString;



                if (cmb_gubunS.Text == "수입")
                {
                    _GubunSearchString = " Gubun = '수입'";
                }
                else if (cmb_gubunS.Text == "지출")
                {
                    _GubunSearchString = " Gubun = '지출'";
                }

                else if (cmb_gubunS.Text == "수입/지출")
                {
                    _GubunSearchString = " Gubun = '수입/지출'";
                }

                if (_FilterString != string.Empty && _GubunSearchString != string.Empty)
                {
                    _FilterString += " AND " + _GubunSearchString;
                }
                else
                {
                    _FilterString +=  _GubunSearchString;
                }

                try
                {

                    chargeAccountsBindingSource.Filter = _FilterString;
                    //if (LocalUser.Instance.LogInInfomation.UserGubun == 1)
                    //{

                    //    chargeAccountsBindingSource.Filter = _FilterString;
                    //}
                    //else
                    //{
                    //    if (_FilterString != string.Empty)
                    //    {
                    //         chargeAccountsBindingSource.Filter = _FilterString + " AND ClientId = '" + LocalUser.Instance.LogInInfomation.ClientId + "'";
                    //    }
                    //    else
                    //    {
                    //         chargeAccountsBindingSource.Filter = " ClientId = '" + LocalUser.Instance.LogInInfomation.ClientId + "'";
                    //    }
                    //}
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_gubunS.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
            _Search();
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargeAccountsRow;
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == Selected.Gubun2 && c.Div == "AccountGubun2");
                e.Value = Query.First().Name;

            }
            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargeAccountsRow;
                //var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == Selected.Gubun && c.Div == "AccountGubun");
                if (Selected.ClientId == "NULL")
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "공통";
            
                }
                else
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "추가";
            
                }

            }
        }

        private void chargeAccountsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

            err.Clear();


            if (chargeAccountsBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)chargeAccountsBindingSource.Current).Row as CMDataSet.ChargeAccountsRow;
            if (Selected != null)
            {

                if (Selected.ClientId == "NULL")
                {
                    cmb_Gubun.Enabled = false;
                    txt_Name.ReadOnly = true;
                    txt_Remark.ReadOnly = true;
                }
                else
                {
                    cmb_Gubun.Enabled = true;
                    txt_Name.ReadOnly = false;
                    txt_Remark.ReadOnly = false;
                }

            }
        }
    }
}
