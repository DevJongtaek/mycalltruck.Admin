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
    public partial class UCSTATS5 : UserControl
    {
        public UCSTATS5()
        {
            InitializeComponent();
        }

        private void UCSTATS5_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
            this.sTATS5TableAdapter.Fill(cMDataSet.STATS5, dtp_Sdate.Value, dtp_Edate.Value, LocalUser.Instance.LogInInformation.ClientId);
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            this.sTATS5TableAdapter.Fill(cMDataSet.STATS5, dtp_Sdate.Value, dtp_Edate.Value, LocalUser.Instance.LogInInformation.ClientId);
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;

            btn_Search_Click(null, null);
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.STATS5Row;
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
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
            if (newDGV1.Columns[e.ColumnIndex] == priceDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }

            }

            if (newDGV1.Columns[e.ColumnIndex] == revenueDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }

            }

            if (newDGV1.Columns[e.ColumnIndex] == orderPerCentDataGridViewTextBoxColumn)
            {

                e.Value = Convert.ToInt64(e.Value);


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
                //if (newDGV1.SortOrder == System.Windows.Forms.SortOrder.Descending)
                //{
                //    order += " Desc";
                //}
            }

            f.STATS5(dtp_Sdate.Text, dtp_Edate.Text, order, sTATS5BindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }
    }
}
