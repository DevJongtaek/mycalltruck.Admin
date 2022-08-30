using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmHelp_Youtube : Form
    {
        string _FormName = string.Empty;
        public string PageAddress { get; set; }

        public FrmHelp_Youtube(string FormName)
        {
            InitializeComponent();
            _FormName = FormName;
        }

        private void FrmHelp_Load(object sender, EventArgs e)
        {
           // base.OnLoad(e);
            var embed = "<html><head>" +
            "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
            "</head><body>" +
            "<iframe style=\"display:block; width:99vw; height: 98vh\" src=\"{0}\"" +
            //"frameborder = \"0\" allow = \"accelerometer; autoplay; encrypted-media; playsinline:0;gyroscope; picture-in-picture\" allowfullscreen='allowFullScreen'></iframe>" +
            "frameborder=\"0\" allowfullscreen=\"\" style=\"position: absolute; position: absolute; top: 0; left: 0; width: 100 %; height: 100 %; \">" +
            "</body></html>";
            var url = "https://www.youtube.com/embed/M5AAJU0hhd8";
            switch (_FormName)
            {

                case "Customer":
                    url = "https://www.youtube.com/embed/M5AAJU0hhd8";
                    lblTitle.Text = " 1-1.거래처(화주)한 건 등록";
                    break;
                case "CustomerExcel":
                    url = "https://www.youtube.com/embed/BanTUeSApNw";
                    lblTitle.Text = " 1-2.화주 엑셀 일괄등록";
                    break;

                    


            }
           
            
            this.webBrowser.DocumentText = string.Format(embed, url);
        }

        public void ActivateAndNavitation(string iformName)
        {
            //Activate();
            //switch (iformName)
            //{
            //    case "FrmMN0209_CUSTOMER":
            //        webBrowser.DocumentText = @"<iframe width='100%' height='100%' rel='0' src='https://www.youtube.com/embed/FEicvEEwwTM?vq=hd720' frameborder='0' allowfullscreen></iframe>";
            //        break;
            //    case "FrmMN0203_CAROWNERMANAGE":
            //        webBrowser.DocumentText = @"<iframe width='100%' height='100%' rel='0' src='https://www.youtube.com/embed/ITL7hycAA0c?vq=hd720' frameborder='0' allowfullscreen></iframe>";
            //        break;
            //    case "FrmMN0301_CARGOACCEPT":
            //        webBrowser.DocumentText = @"<iframe width='100%' height='100%' rel='0' src='https://www.youtube.com/embed/c7A-SaWt0TA?vq=hd720' frameborder='0' allowfullscreen></iframe>";
            //        break;
            //    case "FrmTrade":
            //        webBrowser.DocumentText = @"<iframe width='100%' height='100%' rel='0' src='https://www.youtube.com/embed/Ld11DNbrVSY?vq=hd720' frameborder='0' allowfullscreen></iframe>";
            //        break;
            //    case "FrmMN0212_SALESMANAGE":
            //        webBrowser.DocumentText = @"<iframe width='100%' height='100%' rel='0' src='https://www.youtube.com/embed/QwuOtjA5clg?vq=hd720' frameborder='0' allowfullscreen></iframe>";
            //        break;
            //    default:
            //        break;
            //}
            //if (WindowState == FormWindowState.Minimized)
            //    WindowState = FormWindowState.Normal;
        }

        private void FrmHelp_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
