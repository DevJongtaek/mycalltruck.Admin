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
    public partial class FrmETAXIN : Form
    {
        string DateS = string.Empty;
        public FrmETAXIN(string Ddate)
        {
            InitializeComponent();


            label2.Text = Ddate;

           // dtp_Date.Value = DateTime.Now;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
