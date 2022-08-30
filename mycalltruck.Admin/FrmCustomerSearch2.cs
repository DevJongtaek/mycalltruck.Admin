using mycalltruck.Admin.Class.Common;
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
    public partial class FrmCustomerSearch2 : Form
    {
        public CMDataSet.CustomersRow Selected = null;

        public string CL_COMP_NM { get; set; }
        public string CL_COMP_BSNS { get; set; }
        public string CL_P_TEL { get; set; }
        public string CL_COMP_CORP_NUM { get; set; }
        public int CustomerId { get; set; }

        string S_GubunNum = string.Empty;
        //string S_BizGubun = string.Empty;
        public FrmCustomerSearch2(string GubunNum )
        {
            InitializeComponent();

            S_GubunNum = GubunNum;
            //S_BizGubun = BizGubun;
            DialogResult = DialogResult.Cancel;
        }

        private void FrmCustomerSearch_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.Customers' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.customersTableAdapter.Fill(this.cMDataSet.Customers);

            btn_Search_Click(null, null);
        }

        string Salegubunin = string.Empty;
        string BizGubunin = string.Empty;
        private void _Search()
        {
            this.customersTableAdapter.Fill(this.cMDataSet.Customers);


            try
            {
                Salegubunin = S_GubunNum;




                if (String.IsNullOrEmpty(txt_Search.Text))
                {

                    customersBindingSource.Filter = string.Format("SalesGubun IN ({0}) and  ClientId  = {1}", Salegubunin, LocalUser.Instance.LogInInformation.ClientId);
                }
                else
                {

                    customersBindingSource.Filter = string.Format("(SangHo Like  '%{0}%' or BizNo Like  '%{0}%' or Ceo Like  '%{0}%')and SalesGubun  IN ({1}) and ClientId = {2}", txt_Search.Text, Salegubunin, LocalUser.Instance.LogInInformation.ClientId);

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

        private void btn_new_Click(object sender, EventArgs e)
        {
            txt_Search.Text = "";
            btn_Search_Click(null, null);
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void grid1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            if (grid1.SelectedCells.Count == 0) return;
            if (grid1.SelectedCells[0].RowIndex < 0) return;

            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as CMDataSet.CustomersRow;
            if (Selected == null)
                return;

            CL_COMP_NM = Selected.SangHo;
            CL_COMP_BSNS = Selected.BizNo;
            CL_P_TEL = Selected.PhoneNo;
            CL_COMP_CORP_NUM = Selected.ResgisterNo;
            CustomerId = Selected.CustomerId;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void grid1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (grid1.SelectedCells.Count == 0) return;
            if (grid1.SelectedCells[0].RowIndex < 0) return;

            if (customersBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)customersBindingSource.Current).Row as CMDataSet.CustomersRow;
            if (Selected == null)
                return;

            CL_COMP_NM = Selected.SangHo;
            CL_COMP_BSNS = Selected.BizNo;
            CL_P_TEL = Selected.PhoneNo;
            CL_COMP_CORP_NUM = Selected.ResgisterNo;
            CustomerId = Selected.CustomerId;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
