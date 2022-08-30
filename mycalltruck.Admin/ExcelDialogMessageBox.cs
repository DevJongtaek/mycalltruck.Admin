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
    public partial class ExcelDialogMessageBox : Form
    {
        public ExcelDialogMessageBox()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void btnTax_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void PrintDialogMessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }
    }
}
