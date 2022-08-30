using mycalltruck.Admin.DataSets;
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
    public partial class FRMMNLog : Form
    {
        public FRMMNLog()
        {
            InitializeComponent();
        }

        private void FRMMNLog_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'loginLog._LoginLog' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.loginLogTableAdapter.Fill(this.loginLog._LoginLog,dtp_Sdate.Text,dtp_Edate.Text);

            FClear();
            btn_Search_Click(null, null);

        }

        private void FClear()
        {


            cmb_Search.SelectedIndex = 0;
            dtp_Sdate.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtp_Edate.Value = DateTime.Now;
            txt_Search.Text = "";
        }
        private void _Search()
        {

            loginLogTableAdapter.Fill(loginLog._LoginLog, dtp_Sdate.Text, dtp_Edate.Text);



            List<String> Filters = new List<string>();
            string _cmbSearchString = string.Empty;


            try
            {

                if (cmb_Search.Text == "상호")
                {
                    string filter = string.Format("ClientName Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }
                else if (cmb_Search.Text == "아이디")
                {
                    string filter = string.Format("LoginId Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }
                else if (cmb_Search.Text == "사용자")
                {
                    string filter = string.Format("UserName Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }
                else if (cmb_Search.Text == "구분")
                {
                    string filter = string.Format("Gubun Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }

                if (!String.IsNullOrEmpty(_cmbSearchString))
                    Filters.Add(_cmbSearchString);

                loginLogBindingSource.Filter = String.Join(" AND ", Filters);
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
            FClear();
            btn_Search_Click(null, null);
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as LoginLog.LoginLogRow;
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                newDGV1[e.ColumnIndex, e.RowIndex].Value = (newDGV1.Rows.Count - e.RowIndex).ToString();
            }

            if (newDGV1.Columns[e.ColumnIndex] == logDateDataGridViewTextBoxColumn)
            {

                e.Value = Selected.LogDate.ToString("yyyy-MM-dd HH:mm:ss").Replace("-","/");
               

            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }
    }
}
