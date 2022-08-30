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
    public partial class FrmStatsPreview : Form
    {
        string _Gubun = "1";
        public FrmStatsPreview(string Gubun)
        {
            _Gubun = Gubun;
            InitializeComponent();
        }

        private void FrmStatsPreview_Load(object sender, EventArgs e)
        {
            switch(_Gubun)
            {
                case "1":
                    pictureBox1.Image = Properties.Resources.Stats1;
                    break;
                case "2":
                    pictureBox1.Image = Properties.Resources.Stats2;
                    break;
                case "3":
                    pictureBox1.Image = Properties.Resources.Stats3;
                    break;
                case "4":
                    pictureBox1.Image = Properties.Resources.Stats4;
                    break;
                case "5":
                    pictureBox1.Image = Properties.Resources.Stats5;
                    break;
            }
            
        }
    }
}
