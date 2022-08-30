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
    public partial class FrmETAXREIN : Form
    {
        public FrmETAXREIN(string Ddate, string SangHo, string Item)
        {
            InitializeComponent();

            label2.Text = Ddate;
            lblItem.Text = Item;
            lbl_SangHo.Text = SangHo;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
