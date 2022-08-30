using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.Extensions;

using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0404_CHAGECARMANAGE : Form
    {
        string Up_Status = string.Empty;
       
        int GridIndex = 0;
        public FrmMN0404_CHAGECARMANAGE()
        {
            InitializeComponent();

            tabControl1.SelectTab(1);
            tabControl1.SelectTab(0);
            _InitCmb();
        }

      
        private void FrmMN0404_CHAGECARMANAGE_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet1.ChargesExcel' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.chargesExcelTableAdapter.Fill(this.cMDataSet.ChargesExcel);
            // TODO: 이 코드는 데이터를 'cMDataSet.Charges1' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.charges1TableAdapter.Fill(this.cMDataSet.Charges1);
            this.chargesTableAdapter.Fill(this.cMDataSet.Charges);
            this.chargeAccountsTableAdapter.Fill(this.cMDataSet.ChargeAccounts);
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(baseDataSet.Drivers);
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {

                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                cmb_ClientSerach.Visible = false;
                txt_ClientSearch.Visible = false;
               // btn_New.Enabled = true;
            }
            else
            {
                this.clientsTableAdapter.Fill(this.cMDataSet.Clients);
                btn_New.Enabled = false;
            }

            Fclear();

            btn_Search_Click(null, null);

            Fclear2();

            btn_Search2_Click(null, null);
        }

        private void Fclear()
        {

            dtp_From.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtp_To.Value = DateTime.Now;
            cmb_SetYnSearch.SelectedIndex = 0;
            cmb_GubunSearch.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
            cmb_ClientSerach.SelectedIndex = 0;
            txt_ClientSearch.Text = "";
        }

        private void Fclear2()
        {

            dtp_From2.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtp_To2.Value = DateTime.Now;
         //   cmb_IssueSearch.SelectedIndex = 0;
            cmb_GubunSearch2.SelectedIndex = 0;
            cmb_Search2.SelectedIndex = 0;
            txt_Search2.Text = "";
        }
        private void _InitCmb()
        {



            var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun3" && (c.Value != 0)).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun.DataSource = GubunSourceDataSource;
            cmb_Gubun.DisplayMember = "Name";
            cmb_Gubun.ValueMember = "Name";



            var GubunSourceDataSource3 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun" && (c.Value != 3 && c.Value != 0)).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun2.DataSource = GubunSourceDataSource3;
            cmb_Gubun2.DisplayMember = "Name";
            cmb_Gubun2.ValueMember = "Name";


            var AccountSourceDataSource = cMDataSet.ChargeAccounts.Where(c => (c.ClientId == LocalUser.Instance.LogInInformation.ClientId.ToString() || c.ClientId == "NULL") && (c.Gubun == cmb_Gubun2.SelectedValue.ToString() || c.Gubun == "수입/지출") && (c.Gubun2 == 2)).Select(c => new { c.ChargeAccountId, c.Name }).OrderBy(c => c.ChargeAccountId).ToArray();
            cmb_ChargeAccountId.DataSource = AccountSourceDataSource;
            cmb_ChargeAccountId.DisplayMember = "Name";
            cmb_ChargeAccountId.ValueMember = "ChargeAccountId";

        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            int Tabindex = 0;
            if (tabControl1.SelectedIndex == 0)
            {
                FrmMN0404_CHAGECARMANAGE_ADD _Form = new FrmMN0404_CHAGECARMANAGE_ADD(Tabindex);
                _Form.Owner = this;
                _Form.StartPosition = FormStartPosition.CenterParent;
                _Form.ShowDialog(

                ); 
                tabControl1.SelectTab(0);
                btn_Search_Click(null, null);
            }
            else
            {
                FrmMN0404_CHAGECARMANAGE_ADD2 _Form = new FrmMN0404_CHAGECARMANAGE_ADD2(Tabindex);
                _Form.Owner = this;
                _Form.StartPosition = FormStartPosition.CenterParent;
                _Form.ShowDialog(

                );
                tabControl1.SelectTab(1);
                btn_Search2_Click(null, null);
            }

          
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {

                if (txt_CustomerName.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CustomerName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }


                if (txt_BizNo.Text.Length != 12 || txt_BizNo.Text.Contains(" "))
                {
                    MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                    err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");
                    tabControl1.SelectTab(0);
                    return;
                }


                if (txt_Item.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }
                if (txt_UnitPrice.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_UnitPrice, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }


                //if (txt_Amount.Text == "")
                //{
                //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                //    err.SetError(txt_Amount, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                //    return -1;
                //}
            }
            else
            {

                if (txt_Num.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Num, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }


                if (txt_UnitPrice2.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_UnitPrice2, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return ;
                }
            }


            if (tabControl1.SelectedIndex == 0)
            {
                Up_Status = "Update";
                int _rows = UpdateDB(Up_Status);

            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Up_Status = "Update";
                int _rows = UpdateDB2(Up_Status);
            }
        }
        private int UpdateDB(string Status)
        {
            int _rows = 0;
            try
            {


                chargesBindingSource.EndEdit();
              //  _rows = SingleDataSet.Instance.Charges.Where(c => c.RowState != DataRowState.Unchanged).Count();


                var Row = ((DataRowView)chargesBindingSource.Current).Row as CMDataSet.ChargesRow;

              
                    Row.Gubun = cmb_Gubun.SelectedValue.ToString();
                    Row.ApplyDate = dtp_ApplyDate.Value;
                    Row.CustomerName = txt_CustomerName.Text;
                    Row.BizNo = txt_BizNo.Text;
                    Row.Item = txt_Item.Text;
                    Row.Price = Int64.Parse(txt_UnitPrice.Text);
                    Row.Tax = Int64.Parse(txt_Tax.Text);
                    Row.Amount = Int64.Parse(txt_Amount.Text);

                    if (chk_IssueGubun.Checked == true)
                    {
                        Row.IsIssued = true;
                    }
                    else
                    {
                        Row.IsIssued = false;
                    }

                    Row.CardName = cmb_CardName.Text;
                    Row.TaxGubun = 1;
                    Row.DriverId = txt_DriverId.Text;
                    Row.Num = 0;
                    Row.UnitPrice = 0;
                    Row.Remark = "";


                  
                //  clientsTableAdapter.Update(SingleDataSet.Instance.Clients);

                if (Status == "Update")
                {

                    chargesTableAdapter.Update(Row);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "기사세무", 1), "기사세무 수정 성공");

                    if (dataGridView1.RowCount > 1)
                    {
                        GridIndex = chargesBindingSource.Position;
                        btn_Search_Click(null, null);
                        dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                    }
                    else
                    {
                        btn_Search_Click(null, null);
                    }
                }
                else  if (Status == "Delete")
                {
                   chargesTableAdapter.Update(cMDataSet.Charges);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "기사세무", 1), "기사세무 삭제 성공");
                    btn_Search_Click(null, null);
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "매출관리 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }

        private int UpdateDB2(string Status)
        {
            int _rows = 0;
            try
            {


                charges1BindingSource.EndEdit();
                //  _rows = SingleDataSet.Instance.Charges.Where(c => c.RowState != DataRowState.Unchanged).Count();


                var Row = ((DataRowView)charges1BindingSource.Current).Row as CMDataSet.Charges1Row;


                Row.Gubun = cmb_Gubun2.SelectedValue.ToString();

                Row.ChargeAccountId =int.Parse(cmb_ChargeAccountId.SelectedValue.ToString());



              

                if (Status == "Update")
                {

                    charges1TableAdapter.Update(Row);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "기사세무", 1), "기사세무 수정 성공");

                    if (newDGV1.RowCount > 1)
                    {
                        GridIndex = charges1BindingSource.Position;
                        btn_Search2_Click(null, null);
                        newDGV1.CurrentCell = newDGV1.Rows[GridIndex].Cells[0];
                    }
                    else
                    {
                        btn_Search2_Click(null, null);
                    }
                }
                else if (Status == "Delete")
                {
                    charges1TableAdapter.Update(cMDataSet.Charges1);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "기사세무", 1), "기사세무 삭제 성공");
                    btn_Search2_Click(null, null);
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "매출관리 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
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



                    if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "기사세무", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    foreach (DataGridViewRow row in deleteRows)
                    {
                        dataGridView1.Rows.Remove(row);
                    }
                }
                Up_Status = "Delete";
                int _rows = UpdateDB(Up_Status);
            }
            else
            {
                List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

                if (newDGV1.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell item in newDGV1.SelectedCells)
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



                    if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "기사세무", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    foreach (DataGridViewRow row in deleteRows)
                    {
                        newDGV1.Rows.Remove(row);
                    }
                }
                Up_Status = "Delete";
                int _rows = UpdateDB2(Up_Status);
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
        private void _Search()
        {
            chkAllSelect.Checked = false;
            this.chargesTableAdapter.FillBy(this.cMDataSet.Charges);
            this.chargesExcelTableAdapter.Fill(this.cMDataSet.ChargesExcel);
            string _DateSearchString = string.Empty;
            string _SetYnSearchString = string.Empty;
            string _GubunSearchString = string.Empty;
            string _cmbSearchString = string.Empty;
            string _FilterString = string.Empty;
            string _ClientString = string.Empty;

            string _AllSearchString = string.Empty;
            string _AdminSearchString = string.Empty;

            _DateSearchString = "ApplyDate1 >= '" + dtp_From.Text + "'  AND ApplyDate1 <= '" + dtp_To.Text + "'";
             _FilterString += _DateSearchString;

             if (cmb_SetYnSearch.SelectedIndex == 1)
             {
                 _SetYnSearchString = "SetYN = 2  ";
             }
             else if (cmb_SetYnSearch.SelectedIndex == 2)
             {
                 _SetYnSearchString = "SetYN in(1,0) ";
             }
             else
             {
                 _SetYnSearchString = "";


             }

             if (_FilterString != string.Empty && _SetYnSearchString != string.Empty)
             {
                 _FilterString += " AND " + _SetYnSearchString;
             }
             else
             {
                 _FilterString += _SetYnSearchString;
             }

             if (cmb_GubunSearch.SelectedIndex == 0)
             {
                 _GubunSearchString = " gubun in('매입','매출')";

             }
             else if(cmb_GubunSearch.SelectedIndex == 1)
             {
                 _GubunSearchString = " gubun in('매입')";
             }
             else if (cmb_GubunSearch.SelectedIndex == 2)
             {
                 _GubunSearchString = " gubun in('매출')";
             }

             if (_FilterString != string.Empty && _GubunSearchString != string.Empty)
             {
                 _FilterString += " AND " + _GubunSearchString;
             }
             else
             {
                 _FilterString += _GubunSearchString;
             }

             if (cmb_Search.Text == "기사명")
             {
                 var codes = baseDataSet.Drivers.Where(c => c.CarYear.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                 if (codes.Count() > 0)
                 {
                     string filter = String.Format("DriverId IN ('{0}'", codes[0]);
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
             else if (cmb_Search.Text == "차량번호")
             {
                 var codes = baseDataSet.Drivers.Where(c => c.CarNo.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                 if (codes.Count() > 0)
                 {
                     string filter = String.Format("DriverId IN ('{0}'", codes[0]);
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

             else if (cmb_Search.Text == "사업자번호")
             {
                 var codes = baseDataSet.Drivers.Where(c => c.BizNo.Replace("-", "").Contains(txt_Search.Text) || c.BizNo.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                 if (codes.Count() > 0)
                 {
                     string filter = String.Format("DriverId IN ('{0}'", codes[0]);
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

             if (_FilterString != string.Empty && _cmbSearchString != string.Empty)
             {
                 _FilterString += " AND " + _cmbSearchString ;
             }
             else
             {
                 _FilterString += _cmbSearchString;
             }


             if (_FilterString != string.Empty && _ClientString != string.Empty)
             {
                 _FilterString += " AND " + _ClientString;
             }
             else
             {
                 _FilterString +=  _ClientString;
             }

             if (!LocalUser.Instance.LogInInformation.IsAdmin)
             {
                 #region 차량

                 List<string> VisibleOrderIds = new List<string>();

                 var DriverCnt = baseDataSet.Drivers.Where(c => c.CandidateId == LocalUser.Instance.LogInInformation.ClientId && c.AccountUse == true).ToArray();

                 //if (DriverGroupsCnt.Any())
                 //{
                 //    var Query = cMDataSet.DriverGroups.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();

                 //    foreach (var item in Query)
                 //    {
                 //        VisibleOrderIds.Add("'" + item.DriverId + "'");
                 //    }




                 //}
                 if (DriverCnt.Any())
                 {
                     var Query = baseDataSet.Drivers.Where(c => c.CandidateId == LocalUser.Instance.LogInInformation.ClientId && c.AccountUse == true).ToArray();

                     foreach (var item in Query)
                     {
                         VisibleOrderIds.Add("'" + item.DriverId + "'");

                     }

                 }
                 VisibleOrderIds.Add("0");

                 if (VisibleOrderIds.Any())
                 {
                     string filter = "DriverId  in  (" + String.Join(",", VisibleOrderIds) + ")";


                     _AllSearchString = filter;
                 }
                 #endregion

                 if (_FilterString != string.Empty && _AllSearchString != string.Empty)
                 {
                     _FilterString += " AND " + _AllSearchString;
                 }
                 else
                 {
                     _FilterString += _AllSearchString;
                 }
             }
             else
             {
                 if (cmb_ClientSerach.Text == "운송사코드")
                 {

                     var codes = cMDataSet.Clients.Where(c => c.Code.ToString().Contains(txt_ClientSearch.Text)).Select(c => c.ClientId).ToArray();



                     if (codes.Count() > 0)
                     {
                         string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                         for (int i = 1; i < codes.Count(); i++)
                         {
                             filter += string.Format(", '{0}'", codes[i]);
                         }
                         filter += ")";
                         _AdminSearchString = filter;
                     }
                     else
                         _AdminSearchString = "";

                    


                 }
                 else if (cmb_ClientSerach.Text == "운송사명")
                 {

                     var codes = cMDataSet.Clients.Where(c => c.Name.Contains(txt_ClientSearch.Text)).Select(c => c.ClientId).ToArray();
                     if (codes.Count() > 0)
                     {
                         string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                         for (int i = 1; i < codes.Count(); i++)
                         {
                             filter += string.Format(", '{0}'", codes[i]);
                         }
                         filter += ")";
                         _AdminSearchString = filter;
                     }
                     else
                         _AdminSearchString = "";

                 }

                 if (_FilterString != string.Empty && _AdminSearchString != string.Empty)
                 {
                     _FilterString += " AND " + _AdminSearchString;
                 }
                 else
                 {
                     _FilterString += _AdminSearchString;
                 }
             }
             try
             {
                 chargesBindingSource.Filter = _FilterString;
               //  chargesBindingSource1.Filter = _FilterString;
                 if (_FilterString != string.Empty)
                 {
                     chargesExcelBindingSource.Filter = _FilterString + " AND SetYN = 2";
                 }
                 else
                 {
                     chargesExcelBindingSource.Filter = " SetYN = 2";
                 }
             }
             catch
             {
             }
        }

        private void _Search2()
        {
            chkAllSelect.Checked = false;
            this.charges1TableAdapter.FillBy(this.cMDataSet.Charges1);
            string _DateSearchString = string.Empty;
            string _SetYnSearchString = string.Empty;
            string _GubunSearchString = string.Empty;
            string _cmbSearchString = string.Empty;
            string _FilterString = string.Empty;

            string _AllSearchString = string.Empty;

            _DateSearchString = "ApplyDate1 >= '" + dtp_From2.Text + "'  AND ApplyDate1 <= '" + dtp_To2.Text + "'";
            _FilterString += _DateSearchString;

            //if (cmb_IssueSearch.SelectedIndex == 1)
            //{
            //    _SetYnSearchString = "((SetYN = 2 AND T_GUBUN = 'B') OR (SetYN = 1 AND T_GUBUN = 'A')) ";
            //}
            //else if (cmb_IssueSearch.SelectedIndex == 2)
            //{
            //    _SetYnSearchString = "((SetYN = 1 AND T_GUBUN = 'B') OR (SetYN = 0 AND T_GUBUN = 'A')) ";
            //}
            //else
            //{
            //    _SetYnSearchString = "";


            //}

            //if (_FilterString != string.Empty && _SetYnSearchString != string.Empty)
            //{
            //    _FilterString += " AND " + _SetYnSearchString;
            //}
            //else
            //{
            //    _FilterString += _SetYnSearchString;
            //}

            if (cmb_GubunSearch2.SelectedIndex == 0)
            {
                _GubunSearchString = " gubun in('수입','지출') AND  TaxGubun = 2 ";

            }
            else if (cmb_GubunSearch2.SelectedIndex == 1)
            {
                _GubunSearchString = " gubun in('수입') AND  TaxGubun = 2 ";
            }
            else if (cmb_GubunSearch2.SelectedIndex == 2)
            {
                _GubunSearchString = " gubun in('지출') AND  TaxGubun = 2 ";
            }

            if (_FilterString != string.Empty && _GubunSearchString != string.Empty)
            {
                _FilterString += " AND " + _GubunSearchString;
            }
            else
            {
                _FilterString += _GubunSearchString;
            }

            if (cmb_Search2.Text == "기사명")
            {
                var codes = baseDataSet.Drivers.Where(c => c.CarYear.Contains(txt_Search2.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
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
            else if (cmb_Search2.Text == "차량번호")
            {
                var codes = baseDataSet.Drivers.Where(c => c.CarNo.Contains(txt_Search2.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
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

            else if (cmb_Search2.Text == "사업자번호")
            {
                var codes = baseDataSet.Drivers.Where(c => c.BizNo.Replace("-", "").Contains(txt_Search2.Text) || c.BizNo.Contains(txt_Search2.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
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

            if (_FilterString != string.Empty && _cmbSearchString != string.Empty)
            {
                _FilterString += " AND " + _cmbSearchString;
            }
            else
            {
                _FilterString += _cmbSearchString;
            }
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                #region 차량

                List<string> VisibleOrderIds = new List<string>();



                // var DriverGroupsCnt = cMDataSet.DriverGroups.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();
                var DriverCnt = baseDataSet.Drivers.Where(c => c.CandidateId == LocalUser.Instance.LogInInformation.ClientId && c.AccountUse == true).ToArray();

                //if (DriverGroupsCnt.Any())
                //{
                //    var Query = cMDataSet.DriverGroups.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();

                //    foreach (var item in Query)
                //    {
                //        VisibleOrderIds.Add("'" + item.DriverId + "'");
                //    }




                //}
                if (DriverCnt.Any())
                {
                    var Query = baseDataSet.Drivers.Where(c => c.CandidateId == LocalUser.Instance.LogInInformation.ClientId && c.AccountUse == true).ToArray();

                    foreach (var item in Query)
                    {
                        VisibleOrderIds.Add("'" + item.DriverId + "'");

                    }

                }
                VisibleOrderIds.Add("0");

                if (VisibleOrderIds.Any())
                {
                    string filter = "DriverId  in  (" + String.Join(",", VisibleOrderIds) + ")";


                    _AllSearchString = filter;
                }
                #endregion

                if (_FilterString != string.Empty && _AllSearchString != string.Empty)
                {
                    _FilterString += " AND " + _AllSearchString;
                }
                else
                {
                    _FilterString += _AllSearchString;
                }
            }
            try
            {
                charges1BindingSource.Filter = _FilterString;


            }

            catch
            {
                btn_Search_Click(null, null);
            }
        }
        private void btn_Inew_Click(object sender, EventArgs e)
        {
            Fclear();

            btn_Search_Click(null, null);
        }

        private void chargesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
           // chkAllSelect.Checked = false;



            if (chargesBindingSource.Current == null)
            {
                txt_Tax.Text = "";
                txt_DriverId.Text = "";

                cmb_Gubun.SelectedValue = 1;
                //  dtp_ApplyDate.Value = Selected.ApplyDate;
                txt_CustomerName.Text = "";
                txt_BizNo.Text = "";
                txt_Item.Text = "";
                txt_UnitPrice.Text = "";
                txt_Amount.Text = "";

                chk_UseTax.Checked = false;


                chk_IssueGubun.Checked = false;


                driversCardTableAdapter.Fill(this.cMDataSet.DriversCard, 0);
                //cmb_CardName.DataSource="";
                txt_CardNo.Text = "";

                //if (!string.IsNullOrEmpty(Selected.CardName))
                //{

                //}


                return;
            }
            else
            {

                var Selected = ((DataRowView)chargesBindingSource.Current).Row as CMDataSet.ChargesRow;
                if (Selected != null)
                {
                    if (Selected.T_Gubun == "A")
                    {
                        if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.ClientUserId > 0)
                        {

                            if (cMDataSet.ClientUsers.Any(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId && !c.AllowWrite))
                            {
                                btnUpdate.Enabled = false;
                                btnCurrentDelete.Enabled = false;
                            }
                            else
                            {
                                btnUpdate.Enabled = true;
                                btnCurrentDelete.Enabled = true;
                            }
                        }
                       
                        
                    }
                    else
                    {
                        
                            btnUpdate.Enabled = false;
                            btnCurrentDelete.Enabled = false;
                      
                       
                    }

                    if (Selected.TaxGubun == 1)
                    {
                        // tabControl1.SelectTab(0);


                        txt_Tax.Text = Selected.Tax.ToString();
                        txt_DriverId.Text = Selected.DriverId.ToString();

                        cmb_Gubun.SelectedValue = Selected.Gubun;
                        dtp_ApplyDate.Value = Selected.ApplyDate;
                        txt_CustomerName.Text = Selected.CustomerName;
                        txt_BizNo.Text = Selected.BizNo;
                        txt_Item.Text = Selected.Item;
                        txt_UnitPrice.Text = Selected.Price.ToString();
                        txt_Amount.Text = Selected.Amount.ToString();
                        if (Selected.UseTax == true)
                        {
                            chk_UseTax.Checked = true;
                        }
                        else
                        {
                            chk_UseTax.Checked = false;
                        }
                        if (Selected.IsIssued == true)
                        {
                            chk_IssueGubun.Checked = true;
                        }
                        else
                        {
                            chk_IssueGubun.Checked = false;
                        }


                        if (Selected.TaxGubun == 1 && Selected.T_Gubun == "A")
                        {
                            driversCardTableAdapter.Fill(this.cMDataSet.DriversCard, int.Parse(Selected.DriverId));



                            if (cmb_CardName.Items.Count > 0)
                            {
                                cmb_CardName.SelectedText = Selected.CardName;
                                txt_CardNo.Text = cmb_CardName.SelectedValue.ToString();
                            }
                            else
                            {
                                //cmb_CardName.Items.Clear();
                                txt_CardNo.Text = "";
                            }
                        }
                        else
                        {
                            driversCardTableAdapter.Fill(this.cMDataSet.DriversCard, 0);
                            //cmb_CardName.DataSource="";
                            txt_CardNo.Text = "";
                        }

                        //if (!string.IsNullOrEmpty(Selected.CardName))
                        //{

                        //}

                    }
                    else
                    {

                    }

                }

            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                if (Selected.T_Gubun == "A")
                {

                    if (Selected.SetYN == 2)
                    {
                        e.Value = "확정";

                    }
                    else
                    {
                        e.Value = "미확정";

                    }
                }
                else if(Selected.T_Gubun == "B")
                {
                    if (Selected.SetYN == 2)
                    {
                        e.Value = "확정";

                    }
                    else
                    {
                        e.Value = "미확정";

                    }
                }
            }

            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {

                if (!String.IsNullOrEmpty(Selected.ClientId))
                {
                    var Query = cMDataSet.Clients.Where(c => c.ClientId == int.Parse(Selected.ClientId));
                    if (Query.Any())
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().Code;

                    }
                    else
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "000000";
                    }
                }
            }
            //운송사명
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(Selected.ClientId))
                {

                    var Query = cMDataSet.Clients.Where(c => c.ClientId == int.Parse(Selected.ClientId));

                    if (Query.Any())
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().Name;
                    }
                    else
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "미지정";
                    }
                }

            }

            //발행일
            else if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
            }
            else if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                if (e.Value.ToString() == "A")
                {
                    e.Value = "추가";
                }
                else
                {
                    e.Value = "원장";
                }
            }

            //기사명
            else if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {

                if (Selected.DriverId != "NULL")
                {
                    var Query = baseDataSet.Drivers.Where(c => c.DriverId == int.Parse(Selected.DriverId));
                    if (Query.Any())
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().CarYear;

                    }
                }



            }

           //차량번호
            else if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {

                if (Selected.DriverId != "NULL")
                {
                    var Query = baseDataSet.Drivers.Where(c => c.DriverId == int.Parse(Selected.DriverId));
                    if (Query.Any())
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().CarNo;

                    }
                }



            }

          
            //전표구분
            else if (e.ColumnIndex == 9 && e.RowIndex >= 0)
            {

                if (Selected.DriverId != "NULL")
                {
                    var Query = baseDataSet.Drivers.Where(c => c.DriverId == int.Parse(Selected.DriverId));
                    if (Query.Any())
                    {
                        e.Value = Query.First().BizNo;

                    }
                }



            }
            else if (e.ColumnIndex == 10 && e.RowIndex >= 0)
            {

                if (e.Value.ToString() == "매출")
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
                else
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }
            //과세유형
            else if (e.ColumnIndex == 11 && e.RowIndex >= 0)
            {

                if (Selected.UseTax == true)
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "과세";
                }
                else
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "비과세";
                }



            }
            //신용카드번호
            else if (e.ColumnIndex == 13 && e.RowIndex >= 0)
            {

                if (Selected.DriverId != "NULL")
                {

                    var Query = baseDataSet.Drivers.Where(c => c.DriverId == int.Parse(Selected.DriverId));
                    if (Query.Any())
                    {
                        if (!String.IsNullOrEmpty(Selected.CardName))
                        {
                            if (Selected.CardName == "신한카드")
                            {
                                dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceShcard;

                            }
                            else if (Selected.CardName == "국민카드")
                            {
                                dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceKbCard;

                            }
                            else if (Selected.CardName == "우리카드")
                            {
                                dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceWrCard;

                            }
                        }

                    }
                }



            }
            //거래처명
            else if (e.ColumnIndex == 15 && e.RowIndex >= 0)
            {

            }
            else if (e.ColumnIndex == 17 && e.RowIndex >= 0)
            {

                e.Value = String.Format("{0:#,##0}", Convert.ToDecimal(e.Value));


            }
            else if (e.ColumnIndex == 18 && e.RowIndex >= 0)
            {

                e.Value = String.Format("{0:#,##0}", Convert.ToDecimal(e.Value));


            }
            //운송사명
            //else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            //{


            //    var Query = SingleDataSet.Instance.Clients.Where(c => c.ClientId == Selected.ClientId);
            //    if (Query.Any())
            //    {
            //        dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().Name;
            //    }
            //}
        }
        private void txt_UnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_UnitPrice_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_UnitPrice.Text))
            {

                if (chk_UseTax.Checked == true)
                {
                    txt_Tax.Text = Math.Truncate((Int64.Parse(txt_UnitPrice.Text)) * 0.1).ToString();
                    txt_Amount.Text = (Int64.Parse(txt_Tax.Text) + (Int64.Parse(txt_UnitPrice.Text))).ToString();

                }
                else
                {
                    txt_Tax.Text = "0";
                    txt_Amount.Text = (Int64.Parse(txt_UnitPrice.Text)).ToString();
                }


            }
        }

        private void chk_UseTax_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_UnitPrice.Text))
            {

                if (chk_UseTax.Checked == true)
                {
                    txt_Tax.Text = Math.Truncate((Int64.Parse(txt_UnitPrice.Text)) * 0.1).ToString();
                    txt_Amount.Text = (Int64.Parse(txt_Tax.Text) + (Int64.Parse(txt_UnitPrice.Text))).ToString();

                }
                else
                {
                    txt_Tax.Text = "0";
                    txt_Amount.Text = (Int64.Parse(txt_UnitPrice.Text)).ToString();
                }


            }
        }
        bool overhead = false;
        List<string> checkedCodes = new List<string>();

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            overhead = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1[CheckBox.Index, i].Value = chkAllSelect.Checked;
            }
            overhead = false;
            dataGridView1_CellValueChanged(null, null);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        object o = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                        string code = ((DataRowView)chargesBindingSource[e.RowIndex])["ChargeId"].ToString();
                        if (Convert.ToBoolean(o))
                        {
                            if (!checkedCodes.Contains(code))
                                checkedCodes.Add(code);
                        }
                        else
                        {
                            if (checkedCodes.Contains(code))
                                checkedCodes.Remove(code);
                        }
                    }
                }
            }
            catch { }
            if (overhead) return;
        }
        private string getFilterString()
        {
            string r = "'0'";
            if (checkedCodes.Count > 0)
            {
                r = String.Join(",", checkedCodes.Select(c => "'" + c + "'").ToArray());
            }
            return r;
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect1 = dataGridView1.GetColumnDisplayRectangle(CheckBox.Index, true);
            chkAllSelect.Location = new Point(rect1.Location.X + 2, rect1.Location.Y + 4);
            if (rect1.Width == 0) chkAllSelect.Visible = false;
            else chkAllSelect.Visible = true;
        }

        private void btn_SetYn_Click(object sender, EventArgs e)
        {

            
            getFilterString();
            string errormessage = string.Empty;
            if (checkedCodes.Count() == 0)
            {
                MessageBox.Show("확정할 건을 선택하십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }




            if (MessageBox.Show("'" +checkedCodes.Count() + "' 건을 확정 하시겠습니까?", "기사세무확정", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {




                #region 확정
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {






                    //원장
                    var TradeIds = dataGridView1.Rows.Cast<DataGridViewRow>().Where(c => c.Visible).Where(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.ChargesRow).T_Gubun == "B" && (((DataRowView)c.DataBoundItem).Row as CMDataSet.ChargesRow).CheckBox).Select(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.ChargesRow).ChargeId).ToArray();


                    //추가
                    var ChargeId = dataGridView1.Rows.Cast<DataGridViewRow>().Where(c => c.Visible).Where(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.ChargesRow).T_Gubun == "A" && (((DataRowView)c.DataBoundItem).Row as CMDataSet.ChargesRow).CheckBox).Select(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.ChargesRow).ChargeId).ToArray();






                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();

                    //cmd.CommandText =
                    //   "UPDATE FPIS_TRU SET FPIS_F_DATE = getdate() WHERE TRU_CONT_FROM >= @TRU_CONT_FROM  AND TRU_CONT_FROM <= @TRU_CONT_FROM2 AND  FPIS_ID IN " + "( select fpis_id from fpis_cont where CliendId in(" + String.Join(",", VisibleOrderIds) + "))  AND ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') ='9999-12-31 00:00:00.000' ";

                    if (TradeIds.Any())
                    {
                        cmd.CommandText = "UPDATE Trades SET SetYN = 2  WHERE TradeId IN " + "(" + String.Join(",", TradeIds) + ")";
                        cmd.ExecuteNonQuery();
                       

                    }
                    if (ChargeId.Any())
                    {
                        cmd.CommandText = "UPDATE Charges SET SetYN = 2 WHERE   ChargeId IN " + "(" + String.Join(",", ChargeId) + ")  ";

                        cmd.ExecuteNonQuery();
                      //  cn.Close();

                    }



                    //cmd.Parameters.AddWithValue("@TRU_CONT_FROM", sDate);
                    //cmd.Parameters.AddWithValue("@TRU_CONT_FROM2", eDate);
                    cn.Close();








                }
                #endregion
            }

            try
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "확정", 1), "기사세무 확정 성공");
                btn_Search_Click(null, null);

            }
            catch
            {

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
            btn_Search_Click(null, null);
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.Charges1Row;

            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                newDGV1[e.ColumnIndex, e.RowIndex].Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
            //기사명
            else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {

                if (Selected.DriverId != "NULL")
                {
                    var Query = baseDataSet.Drivers.Where(c => c.DriverId == int.Parse(Selected.DriverId));
                    if (Query.Any())
                    {
                        newDGV1[e.ColumnIndex, e.RowIndex].Value = Query.First().CarYear;

                    }
                }



            }

           //차량번호
            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {

                if (Selected.DriverId != "NULL")
                {
                    var Query = baseDataSet.Drivers.Where(c => c.DriverId == int.Parse(Selected.DriverId));
                    if (Query.Any())
                    {
                        newDGV1[e.ColumnIndex, e.RowIndex].Value = Query.First().CarNo;

                    }
                }



            }

            //차량번호
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {

                if (Selected.DriverId != "NULL")
                {
                    var Query = baseDataSet.Drivers.Where(c => c.DriverId == int.Parse(Selected.DriverId));
                    if (Query.Any())
                    {
                        e.Value = Query.First().BizNo;

                    }
                }



            }
            else if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                if (e.Value.ToString() == "수입")
                {
                    newDGV1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
                else
                {
                    newDGV1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }

            //차량번호
            else if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {


                var Query = cMDataSet.ChargeAccounts.Where(c => c.ChargeAccountId == Selected.ChargeAccountId);
                if (Query.Any())
                {
                    e.Value = Query.First().Name;

                }



            }

            //else if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            //{

            //    e.Value = String.Format("{0:###0}", Convert.ToDecimal(e.Value));


            //}
            else if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {

                e.Value = String.Format("{0:#,##0}", Convert.ToDecimal(e.Value));


            }

            else if (e.ColumnIndex == 9 && e.RowIndex >= 0)
            {

                e.Value = String.Format("{0:#,##0}", Convert.ToDecimal(e.Value));


            }
            else if (e.ColumnIndex == 10 && e.RowIndex >= 0)
            {

                e.Value = String.Format("{0:#,##0}", Convert.ToDecimal(e.Value));


            }
        }

        private void charges1BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();

            if (charges1BindingSource.Current == null)
            {
                return;
            }
            else
            {
                
                var Selected = ((DataRowView)charges1BindingSource.Current).Row as CMDataSet.Charges1Row;
                if (Selected != null)
                {



                    txt_DriverId1.Text = Selected.DriverId.ToString();

                     cmb_Gubun2.SelectedValue = Selected.Gubun;
                    cmb_ChargeAccountId.SelectedValue = Selected.ChargeAccountId;

                }

            }
        }

        private void btn_Search2_Click(object sender, EventArgs e)
        {
            _Search2();
        }

        private void btn_Inew2_Click(object sender, EventArgs e)
        {
            Fclear2();
            btn_Search2_Click(null, null);
        }

        private void txt_Search2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search2_Click(null, null);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                btnUpdate.Enabled = true;
                btnCurrentDelete.Enabled = true;

                btn_Search2_Click(null, null);
            }
            else
            {
                btn_Search_Click(null,null);
            }
        }

        private void cmb_Gubun2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (cmb_Gubun2.SelectedValue.ToString() == null)
                {
                    return;
                }
                var AccountSourceDataSource1 = cMDataSet.ChargeAccounts.Where(c => (c.ClientId == LocalUser.Instance.LogInInformation.ClientId.ToString() || c.ClientId == "NULL") && c.Gubun == cmb_Gubun2.SelectedValue.ToString()).Select(c => new { c.ChargeAccountId, c.Name }).OrderBy(c => c.ChargeAccountId).ToArray();
                cmb_ChargeAccountId.DataSource = AccountSourceDataSource1;
                cmb_ChargeAccountId.DisplayMember = "Name";
                cmb_ChargeAccountId.ValueMember = "ChargeAccountId";

            }
        }

        private void cmb_Search2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Search2.Clear();
            if (cmb_Search2.SelectedIndex == 0)
            {
                txt_Search2.ReadOnly = true;
            }
            else
            {
                txt_Search2.ReadOnly = false;
            }
        }

        private void txt_Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Num_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {

                if (chk_UseTax2.Checked == true)
                {
                    txt_Tax2.Text = Math.Truncate((Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)) * 0.1).ToString();
                    txt_Amount2.Text = (Int64.Parse(txt_Tax2.Text) + (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text))).ToString();

                }
                else
                {
                    txt_Tax2.Text = "0";
                    txt_Amount2.Text = (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)).ToString();
                }


            }
        }

        private void txt_UnitPrice2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_UnitPrice2_Leave(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {

                if (chk_UseTax2.Checked == true)
                {
                    txt_Tax2.Text = Math.Truncate((Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)) * 0.1).ToString();
                    txt_Amount2.Text = (Int64.Parse(txt_Tax2.Text) + (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text))).ToString();

                }
                else
                {
                    txt_Tax2.Text = "0";
                    txt_Amount2.Text = (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)).ToString();
                }


            }
        }

        private void chk_UseTax2_CheckedChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {

                if (chk_UseTax2.Checked == true)
                {
                    txt_Tax2.Text = Math.Truncate((Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)) * 0.1).ToString();
                    txt_Amount2.Text = (Int64.Parse(txt_Tax2.Text) + (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text))).ToString();

                }
                else
                {
                    txt_Tax2.Text = "0";
                    txt_Amount2.Text = (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)).ToString();
                }


            }
        }

        private void btn_ExcelExport_Click(object sender, EventArgs e)
        {
//            newDGV2_CellFormatting(null, null);

            string title = string.Empty;
            byte[] ieExcel;

            string fileString = string.Empty;


            fileString = "부가가치세_신고_" + DateTime.Now.ToString("yyyyMMdd");
            title = "sheet1";
            ieExcel = Properties.Resources.부가가치세_신고양식;


          
            if (newDGV2.RowCount > 0)
            {
              
                pnProgress.Visible = true;
                bar.Value = 0;
                Thread t = new Thread(new ThreadStart(() =>
                {

                    newDGV2.ExportExistExcel2(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.TAX);
                    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));





                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();


            }
            else
            {
                MessageBox.Show("확정된 정보가 없습니다.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


           

        }

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
          
            if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
                if (Selected.Gubun == "매입")
                {
                    if (!String.IsNullOrEmpty(Selected.CardName))
                    {
                        var Query = baseDataSet.Drivers.Where(c => c.DriverId ==int.Parse(Selected.DriverId)).ToArray();
                        if (Query.Any())
                        {
                            if(Selected.CardName.Contains("신한"))
                            {
                                 e.Value = Query.First().InsuranceShcard;
                            }
                            else if (Selected.CardName.Contains("국민"))
                            {
                                 e.Value = Query.First().InsuranceKbCard;
                            }
                            else if (Selected.CardName.Contains("우리"))
                            {
                                 e.Value = Query.First().InsuranceWrCard;
                            }
                        }
                    }

                }
            }
            //else if (e.ColumnIndex == 9 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
            //     e.Value = Selected.CustomerName;
            //}
            //else if (e.ColumnIndex ==10 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
            //     e.Value = Selected.BizNo;
            //}

            //else if (e.ColumnIndex == 11 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
            //     e.Value = Selected.Price;
            //}

            //else if (e.ColumnIndex == 12 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
            //     e.Value = Selected.Tax;
            //}

            //else if (e.ColumnIndex == 13 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
            //     e.Value = Selected.Item;
            //}
            //else if (e.ColumnIndex == 14 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ChargesRow;
            //    if (Selected.IsIssued == true)
            //    {
            //         e.Value = 1;
            //    }
            //}

        }

        private void txt_ClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }
    }
}
