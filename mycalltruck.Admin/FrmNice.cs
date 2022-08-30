using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;

namespace mycalltruck.Admin
{
    public partial class FrmNice : Form
    {
        string _Value = "";
        string _url = "";
        public FrmNice(string Value,string url)
        {
            InitializeComponent();
            //AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfo);
            _Value = Value;
            _url = url;
        }

        private void FrmPopbill_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
          // string url = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do?";


            Web.Navigate(_url + _Value);


           
            Cursor = Cursors.Arrow;
        }
    }
}
