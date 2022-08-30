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
    public partial class FrmSMSResult : Form
    {
        int _CallSmsId = 0;
        string _Msg = "";
        public FrmSMSResult(int CallSmsId,string Msg)
        {
            InitializeComponent();

            _Msg = Msg;
            txt_SMS.Text = _Msg;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmSMSResult_Load(object sender, EventArgs e)
        {


            textBox1.Focus();
        }
    }
}
