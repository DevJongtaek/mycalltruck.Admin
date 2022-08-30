using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;

namespace mycalltruck.Admin
{
    public partial class UCSTATS7 : UserControl
    {
        public UCSTATS7()
        {
            InitializeComponent();
        }

        private void UCSTATS7_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
            cmb_Search.SelectedIndex = 0;
            this.sTATS7TableAdapter.Fill(cMDataSet.STATS7, dtp_Sdate.Text, dtp_Edate.Text, LocalUser.Instance.LogInInformation.ClientId);

            btn_Search_Click(null, null);
        }

        private void _Search()
        {
            checkedCodes.Clear();

            //sTATS3TableAdapter.Fill(cMDataSet.STATS3, dtp_Sdate.Text, dtp_Edate.Text, LocalUser.Instance.LogInInfomation.ClientId);
            this.sTATS7TableAdapter.Fill(cMDataSet.STATS7, dtp_Sdate.Text, dtp_Edate.Text, LocalUser.Instance.LogInInformation.ClientId);



            List<String> Filters = new List<string>();
            string _cmbSearchString = string.Empty;


            try
            {

                if (cmb_Search.Text == "사업자번호")
                {

                    string filter = string.Format("CL_COMP_BSNS_NUM1 Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;


                }
                else if (cmb_Search.Text == "상호")
                {

                    string filter = string.Format("CL_COMP_NM Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;


                }

                if (!String.IsNullOrEmpty(_cmbSearchString))
                    Filters.Add(_cmbSearchString);




                try
                {

                    sTATS7BindingSource.Filter = String.Join(" AND ", Filters);

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

        private void btn_Search_Click(object sender, EventArgs e)
        {
            //   this.sTATS7TableAdapter.Fill(cMDataSet.STATS7, dtp_Sdate.Text, dtp_Edate.Text, LocalUser.Instance.LogInInfomation.ClientId);

            _Search();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
            cmb_Search.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.STATS7Row;
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                newDGV1[e.ColumnIndex, e.RowIndex].Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0");
            }

            if (newDGV1.Columns[e.ColumnIndex] == clientPriceDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }

            }
            //if (newDGV1.Columns[e.ColumnIndex] == priceDataGridViewTextBoxColumn)
            //{
            //    if (!String.IsNullOrEmpty(e.Value.ToString()))
            //    {
            //        e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
            //    }

            //}

            //if (newDGV1.Columns[e.ColumnIndex] == revenueDataGridViewTextBoxColumn)
            //{
            //    if (!String.IsNullOrEmpty(e.Value.ToString()))
            //    {
            //        e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
            //    }

            //}

            //if (newDGV1.Columns[e.ColumnIndex] == orderPerCentDataGridViewTextBoxColumn)
            //{

            //    e.Value = Convert.ToInt64(e.Value);


            //}


        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
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
        private void button1_Click(object sender, EventArgs e)
        {
            getFilterString();
            string errormessage = string.Empty;
            if (checkedCodes.Count() == 0)
            {
                MessageBox.Show("출력할 건을 선택하십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }







            var NumIds = newDGV1.Rows.Cast<DataGridViewRow>().Where(c => c.Visible).Where(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.STATS7Row).CheckBox).Select(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.STATS7Row).Num).ToArray();
            FrmTaxNew f = new FrmTaxNew(NumIds, dtp_Sdate.Text, dtp_Edate.Text, cmb_Search.SelectedIndex, txt_Search.Text);


            if (cmb_Search.SelectedIndex == 1)
                f.PrintClient1();
            else if (cmb_Search.SelectedIndex == 2)
                f.PrintClient2();
            else
                f.PrintClient();
            f.ShowDialog();
        }

        bool overhead = false;
        List<string> checkedCodes = new List<string>();

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            overhead = true;
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                newDGV1[StatChk.Index, i].Value = chkAllSelect.Checked;
            }
            overhead = false;
            //dataGridView1_CellValueChanged(null, null);
        }

        private void newDGV1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e == null)
                    return;
                if (e.ColumnIndex == 0)
                {

                    object o = newDGV1[e.ColumnIndex, e.RowIndex].Value;

                    //  var IssueDate1 = ((DataRowView)driversBindingSource[e.RowIndex])["IssueDate1"].ToString();
                    string code = ((DataRowView)sTATS7BindingSource[e.RowIndex])["Num"].ToString();
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
            catch { }
            if (overhead) return;
        }
    }
}
