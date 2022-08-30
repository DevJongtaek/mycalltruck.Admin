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
    public partial class FrmETAXCanCle : Form
    {
        string IssueDate1 = "";
        public FrmETAXCanCle(string Ddate, string SangHo, string Item,string IssuDate)
        {
            InitializeComponent();

            label2.Text = Ddate;
            lblItem.Text = Item;
            lbl_SangHo.Text = SangHo;
            IssueDate1 = IssuDate;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            DateTime DDate = DateTime.Parse(dtp_Date.Text);
            DateTime IDate = DateTime.Parse(IssueDate1);
            if (DDate < IDate)
            {
                MessageBox.Show("세금계산서 발행일 보다 이전일수 없습니다.","오류", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
