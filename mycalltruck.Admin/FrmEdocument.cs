using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmEdocument : Form
    {
        // private File _b;
        public static TradeDataSet.TradesRow Trade { get; set; }
        string CarYear = "";
        string MobileNo = "";
        public FrmEdocument(TradeDataSet.TradesRow itrade)
        {
            InitializeComponent();
            Trade = itrade;

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT DriverId, Name,CarYear,MobileNo FROM Drivers WHERE DriverId = '{Trade.DriverId}' ";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {

                            CarYear = _Reader.GetString(2);
                            MobileNo = _Reader.GetString(3);


                        }
                    }
                }
                _Connection.Close();
            }


            WindowState = FormWindowState.Normal;
        }

        private void FrmEdocument_Load(object sender, EventArgs e)
        {
            FView();
            
        }


        private PdfDocument OpenDocument(string fileName)
        {
            try
            {
                return PdfDocument.Load(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void FView()
        {
            var url = "http://m.cardpay.kr/Cardpay-Data/Tax_Images/" + Trade.DriverId.ToString() + "/" + Trade.PdfFileName;

            lblKeyword.Text = Trade.PdfFileName.Replace("signed_", "").Replace(".pdf", "");


             DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYTRADEFILE + "\\" + Trade.DriverCarNo + "_" + CarYear);

            if (di.Exists == false)
            {

                di.Create();
            }
           

            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, di +@"\" + Trade.PdfFileName);

            


            pdfViewer1.Document?.Dispose();
            pdfViewer1.Document = OpenDocument(di + @"\" + Trade.PdfFileName);



            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var form = new PrintPreviewDialog())
            {
                form.Document = pdfViewer1.Document.CreatePrintDocument();
                form.ShowDialog(this);
            }
        }

        private void btnFitWidth_Click(object sender, EventArgs e)
        {
            FitPage(PdfViewerZoomMode.FitWidth);
        }
        private void FitPage(PdfViewerZoomMode zoomMode)
        {
            int page = pdfViewer1.Renderer.Page;
            pdfViewer1.ZoomMode = zoomMode;
            pdfViewer1.Renderer.Zoom = 1;
            pdfViewer1.Renderer.Page = page;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FitPage(PdfViewerZoomMode.FitHeight);
        }

        private void btnFitHeight_Click(object sender, EventArgs e)
        {
            FitPage(PdfViewerZoomMode.FitHeight);
        }

        private void btnFitBest_Click(object sender, EventArgs e)
        {
            FitPage(PdfViewerZoomMode.FitHeight);
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            pdfViewer1.Renderer.RotateLeft();
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            pdfViewer1.Renderer.RotateRight();

        }

        private void FrmEdocument_FormClosing(object sender, FormClosingEventArgs e)
        {
            pdfViewer1.Document.Dispose();
        }
    }
}
