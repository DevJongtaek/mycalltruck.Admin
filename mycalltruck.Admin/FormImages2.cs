using mycalltruck.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{

    public partial class FormImages2 : Form
    {
        //  static mycalltruck.Admin.CMDataSet;
        Bitmap _b = null;
        Bitmap _c = null;
        public FormImages2(BaseDataSet.DriversRow iDriver)
        {
            InitializeComponent();

            var Host = "http://m.cardpay.kr/ImageFromAdmin/GetBizPaper?DriverId=";
            var Host2 = "http://m.cardpay.kr/ImageFromAdmin/GetCarPaper?DriverId=";
            //BizPaper.ImageLocation = Host + Driver.DriverId.ToString();

            //if (BizPaper.Image.Width > BizPaper.Image.Height)
            //{

            //    BizPaper.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //}

            //CarPaper.ImageLocation = Host2 + Driver.DriverId.ToString();


            WebClient mWebClient = new WebClient();
            try
            {
                var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetBizPaper?DriverId=" + iDriver.DriverId.ToString());
                MemoryStream ms = new MemoryStream();
                ms.Write(b, 0, b.Length);
                ms.Position = 0;
                _b = new Bitmap(ms);


                var c = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetCarPaper?DriverId=" + iDriver.DriverId.ToString());
                MemoryStream ms2 = new MemoryStream();
                ms2.Write(c, 0, c.Length);
                ms2.Position = 0;
                _c = new Bitmap(ms2);
            }
            catch (Exception e)
            {


            }


            if (_b.Width > _b.Height)
            {
                _b.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            BizPaper.Image = _b;


            if (_c.Width > _c.Height)
            {
                _c.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            CarPaper.Image = _c;


        }

        int PrintIndex = 1;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintIndex = 1;
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;

            PrintDialog pDialog = new PrintDialog();
            pDialog.Document = pd;
            pDialog.AllowSomePages = true;
            pDialog.ShowHelp = true;
            pDialog.ShowNetwork = true;

            if (pDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            //HttpClient hClient = new HttpClient();
            //var Stream = hClient.GetStreamAsync(GetUrl(PrintIndex)).Result;
            //var dImage = Image.FromStream(Stream);
            //int Width = e.MarginBounds.Width;
            //int Height = (int)((double)dImage.Height * (double)Width / (double)dImage.Width);
            //if (Height > e.MarginBounds.Height)
            //{
            //    Height = e.MarginBounds.Height;
            //    Width = (int)((double)dImage.Width * (double)Height / (double)dImage.Height);
            //}
            //e.Graphics.DrawImage(dImage, new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, Width, Height));




            //var Next = PrintIndex + 1;
            //if (IsEmpty(Next))
            //{
            //    e.HasMorePages = false;
            //}
            //else
            //{
            //    e.HasMorePages = true;
            //    PrintIndex = Next;
            //}
        }
    }
}
