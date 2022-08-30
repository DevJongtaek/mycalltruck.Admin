using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
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
    public partial class FrmMN0208_DRIVERADDMANAGE : Form
    {
        int GridIndex = 0;

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




                    dataGridView1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }


        public FrmMN0208_DRIVERADDMANAGE()
        {
            InitializeComponent();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }


            _InitCmb();
            _InitCmbSearch();
        }

        private void FrmMN0208_DRIVERADDMANAGE_Load(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_New.Enabled = false;
            }
        }
        private void _InitCmb()
        {

           


        }

        private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
        private void _InitCmbSearch()
        {
            DataGridViewColumn[] cols = new DataGridViewColumn[]{sangHoDataGridViewTextBoxColumn,nameDataGridViewTextBoxColumn,phoneNoDataGridViewTextBoxColumn,mobileNoDataGridViewTextBoxColumn
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








        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0208_DRIVERADDMANAGE_ADD _Form = new FrmMN0208_DRIVERADDMANAGE_ADD();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void driverAddBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            
              if (driverAddBindingSource.Current == null)
                return;

              var Selected = ((DataRowView)driverAddBindingSource.Current).Row as CMDataSet.DriverAddRow;
              if (Selected != null)
              {
                
                  // this.driversTableAdapter.Fill(this.cMDataSet.Drivers);

                   //var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).Select(c => new { c.Name,c.CarNo }).ToArray();

                   //if (Query.Any())
                   //{
                   //    txt_SangHo.Text = Query.First().Name;

                   //    //txt_CarNo.Text = Query.First().CarNo;
                   //}
                  if (Selected.OutYn == true)
                  {
                      
                      chkOut.Checked = true;
                      dtp_OutDate.Visible = true;
                  }
                  else
                  {
                      chkOut.Checked = false;
                      dtp_OutDate.Visible = false;
                  }
                 
                   txt_CreateDate.Text = Selected.WriteDate.ToString("d").Replace("-","/");

            //txt_SangHo.Text = 
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //순번
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
          


            else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                //var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DriverAddRow;
                //var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId);
                


                //if (Query.Any())
                //{
                //    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().Name;
                    
                //}

                //else
                //{
                   
                //}
            }


            else if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
            else if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DriverAddRow;

                if (Selected.OutYn == true)
                {
                    e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
                }
               
            }
            else if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
        }

        private void btn_CarInfo_Click(object sender, EventArgs e)
        {
            FrmCarNumSearch2 _frmCarNumSearch2 = new FrmCarNumSearch2();
            _frmCarNumSearch2.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCarNumSearch2.grid1.SelectedCells.Count == 0) return;
                if (_frmCarNumSearch2.grid1.SelectedCells[0].RowIndex < 0) return;


                txt_SangHo.Text = _frmCarNumSearch2.grid1[1, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
                //txt_CarNo.Text = _frmCarNumSearch2.grid1[2, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txt_DriverId.Text = _frmCarNumSearch2.grid1[6, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();






                _frmCarNumSearch2.Close();
            });
            _frmCarNumSearch2.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCarNumSearch2.grid1.SelectedCells.Count == 0) return;
                if (_frmCarNumSearch2.grid1.SelectedCells[0].RowIndex < 0) return;
                txt_SangHo.Text = _frmCarNumSearch2.grid1[0, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
              //  txt_CarNo.Text = _frmCarNumSearch2.grid1[1, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txt_DriverId.Text = _frmCarNumSearch2.grid1[5, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();




                _frmCarNumSearch2.Close();
            });






            _frmCarNumSearch2.Owner = this;
            _frmCarNumSearch2.StartPosition = FormStartPosition.CenterParent;
            _frmCarNumSearch2.ShowDialog();
            txt_Name.Focus();
        }
        private void _Search()
        {
            if (LocalUser.Instance.LogInInformation.IsAdmin)
                this.driverAddTableAdapter.Fill(this.cMDataSet.DriverAdd);
            else
                this.driverAddTableAdapter.FillByClient(this.cMDataSet.DriverAdd, LocalUser.Instance.LogInInformation.ClientId);
            string _FilterString = string.Empty;
            string _cmbSearchString = string.Empty;
            try
            {
                if (cmb_Search.Text == "구분")
                {
                    var codes = SingleDataSet.Instance.StaticOptions.Where(c => c.Name.Contains(txt_Search.Text) && c.Div == "CarGubun").Select(c => c.Value).ToArray();
                    if (codes.Count() > 0)
                    {
                        string filter = String.Format("Gubun IN ('{0}'", codes[0]);
                        for (int i = 1; i < codes.Count(); i++)
                        {
                            filter += string.Format(", '{0}'", codes[i]);
                        }
                        filter += ")";
                        _cmbSearchString = filter;
                    }
                    else
                        _cmbSearchString = "";
                }
                else if (cmb_Search.Text == "면허증번호")
                {
                  
                        string filter = string.Format("SangHo Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                   
                }
                else if (cmb_Search.Text == "차량번호")
                {
                    //var codes = SingleDataSet.Instance.DriverAdd.Where(c => c.CarNo.Contains(txt_Search.Text));
                    //if (codes.Count() > 0)
                    //{
                    //    string filter = string.Format("CarNo Like  '%{0}%'", txt_Search.Text);
                    //    _cmbSearchString = filter;

                    //}
                    //else
                    //    _cmbSearchString = "CarNo = 'CarNo'";
                }
                else if (cmb_Search.Text == "기사명")
                {
                  
                        string filter = string.Format("Name Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                  
                }
                else if (cmb_Search.Text == "전화번호")
                {
                   
                        string filter = string.Format("PhoneNo1 Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                  
                }
                else if (cmb_Search.Text == "핸드폰번호")
                {
                   
                        string filter = string.Format("MobileNo1 Like  '%{0}%'", txt_Search.Text);
                        _cmbSearchString = filter;

                  
                }

                _FilterString += _cmbSearchString;

                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {

                    if (_FilterString != string.Empty)
                    {
                        _FilterString += " AND Clientid = " + LocalUser.Instance.LogInInformation.ClientId + " ";
                    }
                    else
                    {
                        _FilterString += " Clientid = " + LocalUser.Instance.LogInInformation.ClientId + " ";
                    }
                }
                
                //else
                //{

                //}




                try
                {

                    driverAddBindingSource.Filter = _FilterString;
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
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
          
           
            //if (txt_SangHo.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_SangHo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return ;

            //}
            //else if (SingleDataSet.Instance.Drivers.Where(c => c.Name == txt_SangHo.Text.Trim()).Count() == 0)
            //{

            //    MessageBox.Show("해당 상호로 등록된  차가 없습니다.!!", "상호 입력 오류");
            //    err.SetError(txt_SangHo, "해당 상호가 없습니다.!!");
            //    // tabControl1.SelectTab(1);
            //    return -1;

            //}

           


            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;

            }


            if (txt_PhoneNo.Text.Replace("-", "").Length < 9)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;
            }


            if (txt_MobileNo.Text.Replace("-", "").Length < 10)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;
            }
            UpdateDB();
        }
        private void UpdateDB()
        {
            try
            {
                driverAddBindingSource.EndEdit();
                var Row = ((DataRowView)driverAddBindingSource.Current).Row as CMDataSet.DriverAddRow;
                if (chkOut.Checked == true)
                {
                    Row.OutYn = true;
                    Row.OutDate = dtp_OutDate.Value.Date.ToString("yyyy/MM/dd").Replace("-", "/");
                }
                else
                {
                    Row.OutYn = false;
                    Row.OutDate = DBNull.Value.ToString();
                }
                driverAddTableAdapter.Update(Row);
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "기사", 1), "기사정보 수정 성공");

                if (dataGridView1.RowCount > 1)
                {
                    GridIndex = driverAddBindingSource.Position;
                    btn_Search_Click(null, null);
                    dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                }
                else
                {
                    btn_Search_Click(null, null);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("작업 중 오류가 발생하였습니다.", "기사정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                //int DriverAddcount = 0;
                //foreach (DataGridViewRow dRow in deleteRows)
                //{

                //    DriverAddcount += (int)driverAddTableAdapter.GetByDriverAdd(dRow.Cells["DriverId"].Value.ToString(), dRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString());

                //}
                //if (DriverAddcount > 0)
                //{
                //    MessageBox.Show(string.Format("사용중인 데이터 {0}건이  존재하므로\n이 기사정보는 삭제할 수 없습니다.",
                //        DriverAddcount), "기사정보 삭제 실패");
                //    return;
                //}



                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "기사정보", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
                driverAddTableAdapter.Update(cMDataSet.DriverAdd);
              

               
            }
            //Up_Status = "Delete";
            //int _rows = UpdateDB(Up_Status);
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "기사", 1), "기사정보 삭제 성공");
            btn_Search_Click(null, null);
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
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
           
            _Search();
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkOut_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOut.Checked == true)
            {
                dtp_OutDate.Visible = true;
                dtp_OutDate.Value = DateTime.Now;
            }
            else
            {
                dtp_OutDate.Visible = false;
               // dtp_OutDate.Value = DateTime.Now;
            }
        }
    }
}
