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
    public partial class FrmMNSTATS3 : Form
    {
        public FrmMNSTATS3()
        {
            InitializeComponent();
        }

        private void FrmMNSTATS1_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;

         //   sTATS3TableAdapter.Fill(cMDataSet.STATS3, dtp_Sdate.Text, dtp_Edate.Text, LocalUser.Instance.LogInInfomation.ClientId);
            btn_Inew_Click(null, null);
          
        }
        private void _Search()
        {
            sTATS3TableAdapter.Fill(cMDataSet.STATS3, LocalUser.Instance.LogInInformation.ClientId, dtp_Sdate.Value.Date, dtp_Edate.Value.Date.AddDays(1));




            List<String> Filters = new List<string>();
            string _cmbSearchString = string.Empty;
            string _CarTypeSearchString = string.Empty;
            string _CarSizeSearchString = string.Empty;


            try
            {

                if (cmb_Search.Text == "아이디")
                {
                    string filter = string.Format("LoginId Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }
                else if (cmb_Search.Text == "사업자번호")
                {
                    string filter = string.Format("BizNo1 Like  '%{0}%'", txt_Search.Text.Replace("-",""));
                    _cmbSearchString = filter;
                }
                else if (cmb_Search.Text == "차주명")
                {
                    string filter = string.Format("CarYear Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }
                else if (cmb_Search.Text == "차량번호")
                {
                    string filter = string.Format("CarNo Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }
                if (!String.IsNullOrEmpty(_cmbSearchString))
                    Filters.Add(_cmbSearchString);
                try
                {
                    sTATS3BindingSource.Filter = String.Join(" AND ", Filters);
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
            _Search();

           
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
            cmb_Search.SelectedIndex = 0;
          //  cmb_SalesSearch.SelectedIndex = 0;
            txt_Search.Text = "";
          
            btn_Search_Click(null, null);
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.Stats11Row;
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                newDGV1[e.ColumnIndex, e.RowIndex].Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0");
            }

            if (newDGV1.Columns[e.ColumnIndex] == amountDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }

            }

            //if (newDGV1.Columns[e.ColumnIndex] == cAmountDataGridViewTextBoxColumn)
            //{
            //    if (!String.IsNullOrEmpty(e.Value.ToString()))
            //    {
            //        e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
            //    }

            //}

           
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {

            if (newDGV1.RowCount < 1)
            {
                MessageBox.Show("출력할 내용이 없습니다. 먼저 데이터를 조회하신 후 출력 버튼을 눌려주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            FrmStatistics f = new FrmStatistics();
            string order = string.Empty;
            if (newDGV1.SortedColumn != null)
            {
                order = newDGV1.SortedColumn.DataPropertyName;
                //if (newDGV1.SortOrder == System.Windows.Forms.SortOrder.Descending)
                //{
                //    order += " Desc";
                //}
            }

            f.STATS3(dtp_Sdate.Text, dtp_Edate.Text, order, sTATS3BindingSource,cmb_Search.Text,txt_Search.Text);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }
    }
}
