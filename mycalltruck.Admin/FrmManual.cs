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
    public partial class FrmManual : Form
    {
        public FrmManual()
        {
            InitializeComponent();
        }
        FrmHelp_Youtube mFrmHelp_Youtube = null;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            try
            {
                mFrmHelp_Youtube.Close();

            }
            catch
            {

            }

            if (treeView1.SelectedNode.Name == "Customer")
            {


                if (mFrmHelp_Youtube == null)
                {
                    mFrmHelp_Youtube = new FrmHelp_Youtube("Customer");
                    mFrmHelp_Youtube.FormClosing += (a, b) => { mFrmHelp_Youtube = null; };
                    mFrmHelp_Youtube.StartPosition = FormStartPosition.Manual;
                    mFrmHelp_Youtube.Top = 40;
                    mFrmHelp_Youtube.Left = 40;
                    mFrmHelp_Youtube.TopMost = true;
                    mFrmHelp_Youtube.Show();
                }

            }
            else if (treeView1.SelectedNode.Name == "CustomerExcel")
            {

                if (mFrmHelp_Youtube == null)
                {
                    mFrmHelp_Youtube = new FrmHelp_Youtube("CustomerExcel");
                    mFrmHelp_Youtube.FormClosing += (a, b) => { mFrmHelp_Youtube = null; };
                    mFrmHelp_Youtube.StartPosition = FormStartPosition.Manual;
                    mFrmHelp_Youtube.Top = 40;
                    mFrmHelp_Youtube.Left = 40;
                    mFrmHelp_Youtube.TopMost = true;
                    mFrmHelp_Youtube.Show();
                }
            }
        }
    }
}
