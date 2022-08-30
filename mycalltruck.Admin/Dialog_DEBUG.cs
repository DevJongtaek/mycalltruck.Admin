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
    public partial class Dialog_DEBUG : Form
    {
        public Dialog_DEBUG()
        {
            InitializeComponent();
        }

        private void CARDPAY_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TruckConnectionString = "Data Source=222.231.9.253,2899;Initial Catalog=Truck;Persist Security Info=True;User ID=edubillsys;Password=edubillsysdb2202#$";
            Close();
        }

        private void CARDPAY_HY_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TruckConnectionString = "Data Source=222.231.9.252;Initial Catalog=CARDPAY_HY;Persist Security Info=True;User ID=logison;Password=logisondb2202#$";
            Close();
        }

        private void Btn_Beta_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TruckConnectionString = "Data Source=.;Initial Catalog=CARDPAY_HY;Integrated Security=True";
            Close();
        }
    }
}
