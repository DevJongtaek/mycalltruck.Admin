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
    public partial class UCSTATS4 : UserControl
    {
        public UCSTATS4()
        {
            InitializeComponent();
        }

        private void UCSTATS4_Load(object sender, EventArgs e)
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            dtp_Edate.Value = DateTime.Now;
            sTATS4TableAdapter.Fill(cMDataSet.STATS4, LocalUser.Instance.LogInInformation.ClientId, dtp_Sdate.Value.ToString("yyyy-MM-dd"), dtp_Edate.Value.ToString("yyyy-MM-dd"));

            btn_Inew_Click(null, null);
        }

        private void _Search()
        {
            //sTATS3TableAdapter.Fill(cMDataSet.STATS3, dtp_Sdate.Text, dtp_Edate.Text, LocalUser.Instance.LogInInfomation.ClientId);
            sTATS4TableAdapter.Fill(cMDataSet.STATS4, LocalUser.Instance.LogInInformation.ClientId, dtp_Sdate.Value.ToString("yyyy-MM-dd"), dtp_Edate.Value.ToString("yyyy-MM-dd"));



            List<String> Filters = new List<string>();
            string _cmbSearchString = string.Empty;


            try
            {

                if (cmb_Search.Text == "사업자번호")
                {

                    string filter = string.Format("BizNo1 Like  '%{0}%'", txt_Search.Text.Replace("-", ""));
                    _cmbSearchString = filter;


                }
                else if (cmb_Search.Text == "상호")
                {

                    string filter = string.Format("SangHo Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;


                }

                if (!String.IsNullOrEmpty(_cmbSearchString))
                    Filters.Add(_cmbSearchString);




                try
                {

                    sTATS4BindingSource.Filter = String.Join(" AND ", Filters);

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

            f.STATS4(dtp_Sdate.Text, dtp_Edate.Text, order, sTATS4BindingSource);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }
    }
}
