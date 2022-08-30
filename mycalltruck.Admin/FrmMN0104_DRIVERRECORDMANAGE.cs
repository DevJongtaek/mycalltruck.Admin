using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0104_DRIVERRECORDMANAGE : Form
    {
        public FrmMN0104_DRIVERRECORDMANAGE()
        {
            InitializeComponent();
        }

        private void FrmMN0104_DRIVERRECORDMANAGE_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.DTGLog' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.

            this.dTGLogTableAdapter.Fill(this.cMDataSet.DTGLog, LocalUser.Instance.LogInInformation.ClientId);
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {

                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                cmb_ClientSerach.Visible = false;
                txt_ClientSearch.Visible = false;

            }
           
            Fclear();

            btn_Search_Click(null, null);
        }

        private void Fclear()
        {
            cmb_Search.SelectedIndex = 0;
            cmb_SearchDate.SelectedIndex = 0;
            dtpStart.Value = DateTime.Now.AddMonths(-3);
            dtpEnd.Value = DateTime.Now;
            txtSearch.Text = "";
            cmb_ClientSerach.SelectedIndex = 0;
            txt_ClientSearch.Text = "";
            cmb_ClientSerach.SelectedIndex = 0;
            txt_ClientSearch.Text = "";
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }


        private void _Search()
        {
            string _FilterString = string.Empty;
            string _DateSearchString = string.Empty;
            string _cmbSearchString = string.Empty;
            string _AllSearchString = string.Empty;
            string _AdminSearchString = string.Empty;
            if (cmb_SearchDate.SelectedIndex == 0)
            {
                _DateSearchString = "CreateTime1 >= '" + dtpStart.Text + "'  AND CreateTime1 <= '" + dtpEnd.Text + "'";
                

            }
            else
            {
                _DateSearchString = "Date1 >= '" + dtpStart.Text + "'  AND Date1 <= '" + dtpEnd.Text + "'";
            }

            _FilterString += _DateSearchString;

            if (cmb_Search.SelectedIndex == 1)
            {
                string filter = string.Format("CarYear Like  '%{0}%'", txtSearch.Text);
                _cmbSearchString = filter;
            }
            else if (cmb_Search.SelectedIndex == 2)
            {
                string filter = string.Format("CarNo Like  '%{0}%'", txtSearch.Text);
                _cmbSearchString = filter;
            }
            else
            {
                _cmbSearchString = "";
            }

            if (_FilterString != string.Empty && _cmbSearchString != string.Empty)
            {
                _FilterString += " AND " + _cmbSearchString;
            }
            else
            {
                _FilterString +=  _cmbSearchString;
            }



            if (_FilterString != string.Empty && _AllSearchString != string.Empty)
            {
                _FilterString += " AND " + _AllSearchString;
            }
            else
            {
                _FilterString += _AllSearchString;
            }
            //if (cmb_ClientSerach.Text == "운송사코드")
            //{
               
            //    var codes = cMDataSet.Clients.Where(c => c.Code.ToString().Contains(txt_ClientSearch.Text)).Select(c => c.ClientId).ToArray();



            //    if (codes.Count() > 0)
            //    {
            //        string filter = String.Format("CandidateId IN ('{0}'", codes[0]);
            //        for (int i = 1; i < codes.Count(); i++)
            //        {
            //            filter += string.Format(", '{0}'", codes[i]);
            //        }
            //        filter += ")";
            //        _AdminSearchString = filter;
            //    }
            //    else
            //        _AdminSearchString = "";
            //}
            //else if (cmb_ClientSerach.Text == "운송사명")
            //{

            //    var codes = cMDataSet.Clients.Where(c => c.Name.Contains(txt_ClientSearch.Text)).Select(c => c.ClientId).ToArray();
            //    if (codes.Count() > 0)
            //    {
            //        string filter = String.Format("CandidateId IN ('{0}'", codes[0]);
            //        for (int i = 1; i < codes.Count(); i++)
            //        {
            //            filter += string.Format(", '{0}'", codes[i]);
            //        }
            //        filter += ")";
            //        _AdminSearchString = filter;
            //    }
            //    else
            //        _AdminSearchString = "";

            //}

            if (_FilterString != string.Empty && _AdminSearchString != string.Empty)
            {
                _FilterString += " AND " + _AdminSearchString;
            }
            else
            {
                _FilterString += _AdminSearchString;
            }
            try
            {
                dTGLogBindingSource.Filter = _FilterString;
            }
            catch
            {
                btn_Search_Click(null, null);
            }
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            Fclear();

            btn_Search_Click(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else  if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DTGLogRow;
                var Query_Drivers = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId);

                if (Query_Drivers.Any())
                {
                    
                    var Query = cMDataSet.Clients.Where(c => c.ClientId == Query_Drivers.First().CandidateId);
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
            else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DTGLogRow;
                var Query_Drivers = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId);

                   if (Query_Drivers.Any())
                   {
                       var Query = cMDataSet.Clients.Where(c => c.ClientId == Query_Drivers.First().CandidateId);
                       
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

            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
                }

            }
            else if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
                }

            }

            else if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DTGLogRow;


                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = Selected.FilePath.Replace("\\"+Selected.FileName, "");
                }

            }

        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cmb_Search.SelectedIndex == 0)
            {
                txtSearch.ReadOnly = true;
            }
            else
            {
                txtSearch.ReadOnly = false;
            }
        }

        private void txt_ClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void btn_Total_Click(object sender, EventArgs e)
        {
            Frm0104_DTGTOTAL _Form = new Frm0104_DTGTOTAL();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();

            //btn_Search_Click(null, null);
        }
    }
}
