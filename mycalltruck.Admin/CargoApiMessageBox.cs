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
    public partial class CargoApiMessageBox : Form
    {
        public CargoApiMessageBox(string Gubun)
        {
            InitializeComponent();

           
                label1.Text = Gubun;
           


        }

        //거래명세서
        private void btn1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        //세금계산서
        private void btnTax_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

       
        private void SendMailDialogMessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
