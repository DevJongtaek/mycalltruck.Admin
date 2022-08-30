using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmETAXRERESULT : Form
    {
        string SuccessCnt = string.Empty;
        string sdatevalue = string.Empty;
        public FrmETAXRERESULT(string Success, string Sdate)
        {
            InitializeComponent();

            SuccessCnt = Success;
            sdatevalue = Sdate;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //var Query = SingleDataSet.Instance.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId);

            //ProcessStartInfo web = new ProcessStartInfo("iexplore");
            //web.Arguments = String.Format(@"http://www.truebiz.co.kr/edubill.asp?cid=e{0}&pwd={1}&part=edubill", Query.First().BizNo.Replace("-", "")
            //    , Query.First().BizNo.Substring(7));
            //Process.Start(web);

            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void FrmETAXRERESULT_Load(object sender, EventArgs e)
        {
            label2.Text = SuccessCnt;
            label6.Text = sdatevalue;
        }
    }
}
