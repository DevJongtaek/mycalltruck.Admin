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
    public partial class UCSTATS1 : UserControl
    {
        public UCSTATS1()
        {
            InitializeComponent();
        }

        private void UCSTATS1_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;

            stats1TableAdapter.Fill(cMDataSet.Stats1, LocalUser.Instance.LogInInformation.ClientId, dtp_Sdate.Value.Date, dtp_Edate.Value.Date.AddDays(1));
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            stats1TableAdapter.Fill(cMDataSet.Stats1, LocalUser.Instance.LogInInformation.ClientId, dtp_Sdate.Value.Date, dtp_Edate.Value.Date.AddDays(1));
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;

            btn_Search_Click(null, null);
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.Stats1Row;
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

            if (newDGV1.Columns[e.ColumnIndex] == cAmountDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }

            }

            if (newDGV1.Columns[e.ColumnIndex] == SumAmount)
            {
                Decimal SSAmount = Selected.amount + Selected.CAmount;
                e.Value = String.Format("{0:#,##0}", Convert.ToInt64(SSAmount));


            }
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
            }

            f.STATS1(dtp_Sdate.Text, dtp_Edate.Text, order, stats1BindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }
    }
}
