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
using mycalltruck.Admin.DataSets;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using mycalltruck.Admin.Class.Common;

namespace mycalltruck.Admin
{

    public partial class FormImagesDefault : Form
    {
        //  static mycalltruck.Admin.CMDataSet;
        public static OrderDataSet.OrdersRow Order { get; set; }
        List<String> Urls = new List<string>();
        string CarYear = "";
        string MobileNo = "";
        public FormImagesDefault(String[] iUrls)
        {
            InitializeComponent();
            Urls.AddRange(iUrls);

             HttpClient hClient = new HttpClient();
            var Stream = hClient.GetStreamAsync(GetUrl(1)).Result;
            var dImage = Image.FromStream(Stream);
            int Width = dImage.Width;
            int Height = dImage.Height;


            if (dImage.Width > dImage.Height)
            {
                dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }


            Preview.Image = dImage;


            //Preview.ImageLocation = GetUrl(1);
            int _Count = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (!IsEmpty(i))
                    _Count++;
            }
            txtCount.Text = _Count.ToString();
        }

        public bool IsEmpty(int Index)
        {
            if (Index > Urls.Count)
                return true;
            else
                return false;
        }

        private void btnPre_Click(object sender, EventArgs e)
        {if (ImageIndex.Text == "1")
            {
                return;
            }
            int Current = int.Parse(ImageIndex.Text);
            Current--;
            if (!IsEmpty(Current))
            {
                MovePreview(Current);
            }

           
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int Current = int.Parse(ImageIndex.Text);
            Current++;
            if (!IsEmpty(Current))
            {
                MovePreview(Current);
            }
        }

        private string GetUrl(int Index)
        {
            if (Index > Urls.Count)
                return "";
            else
                return Urls[Index - 1];
        }

        private void MovePreview(int Index)
        {
            ImageIndex.Text = Index.ToString();
            Preview.ImageLocation = GetUrl(Index);
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
            HttpClient hClient = new HttpClient();
            var Stream = hClient.GetStreamAsync(GetUrl(PrintIndex)).Result;
            var dImage = Image.FromStream(Stream);
            int Width = e.MarginBounds.Width;
            int Height = dImage.Height;

            //int Height = (int)((double)dImage.Height * (double)Width / (double)dImage.Width);
            //if (Height > e.MarginBounds.Height)
            //{
            //    Height = e.MarginBounds.Height;
            //    Width = (int)((double)dImage.Width * (double)Height / (double)dImage.Height);
            //}
            //e.Graphics.DrawImage(dImage, new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, Width, Height));
           



            if (dImage.Width > dImage.Height)
            {
                dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            var width = dImage.Width;
            var height = dImage.Height;
            if (dImage.Width > e.PageBounds.Width)
            {
                width = e.PageBounds.Width;
                height = height * e.PageBounds.Width / dImage.Width;

            }
            else if (dImage.Height > e.PageBounds.Height)
            {
                height = e.PageBounds.Height;
                width = width * e.PageBounds.Height / dImage.Height;
            }

            e.Graphics.DrawImage(dImage, 10, 10, width - 20, height);

            var Next = PrintIndex + 1;
            if (IsEmpty(Next))
            {
                e.HasMorePages = false;
            }
            else
            {
                e.HasMorePages = true;
                PrintIndex = Next;
            }
        }
        private void PrintPage2(object sender, PrintPageEventArgs e)
        {
            //using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //{
            //    _Connection.Open();
            //    using (SqlCommand _Command = _Connection.CreateCommand())
            //    {
            //        _Command.CommandText = $"SELECT DriverId, Name,CarYear,MobileNo FROM Drivers WHERE DriverId = '{Order.DriverId}' ";
            //        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
            //        {
            //            if (_Reader.Read())
            //            {

            //                CarYear = _Reader.GetString(2);
            //                MobileNo = _Reader.GetString(3);


            //            }
            //        }
            //    }
            //    _Connection.Close();
            //}



            HttpClient hClient = new HttpClient();
            var Stream = hClient.GetStreamAsync(GetUrl(Convert.ToInt32(ImageIndex.Text))).Result;
            var dImage = Image.FromStream(Stream);
            int Width = dImage.Width;
            int Height = dImage.Height;



            if (dImage.Width > dImage.Height)
            {
                dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            var width = dImage.Width;
            var height = dImage.Height;
            if (dImage.Width > e.PageBounds.Width)
            {
                width = e.PageBounds.Width;
                height = height * e.PageBounds.Width / dImage.Width;

            }
            else if (dImage.Height > e.PageBounds.Height)
            {
                height = e.PageBounds.Height;
                width = width * e.PageBounds.Height / dImage.Height;
            }
            //_e.Graphics.DrawImage(_b, 0, 0, width, height);
            Font font = new Font("굴림", 12, FontStyle.Regular, GraphicsUnit.Point);


            //String TopText = "";
            //String TopText2 = "";
            //TopText = CarYear + "(" + Trade.DriverCarNo + ") | " + Trade.RequestDate.Date.ToString("d").Replace("-", "/") + " | " + Trade.DriverBizNo + " | " + ImageIndex.Text + "/" + txtCount.Text;

            //if (!String.IsNullOrEmpty(Trade.StartState))
            //{
            //    TopText2 = Trade.TransportDate.ToString("d").Replace("-", "/") + " | " + Trade.StartState + " ▶ " + Trade.StopState + " | " + MobileNo;
            //}
            //if (String.IsNullOrEmpty(Trade.StartState))
            //{
            //    TopText2 = Trade.TransportDate.ToString("d").Replace("-", "/") + " | " + MobileNo;
            //}

            //e.Graphics.DrawString(TopText, font, new SolidBrush(this.ForeColor), 10, 10);
            //e.Graphics.DrawString(TopText2, font, new SolidBrush(this.ForeColor), 10, 30);

            e.Graphics.DrawImage(dImage, 10, 10, width - 20, height);



        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            MessageBox.Show("“미리보기” 창에서는 현재페이지 \r\n한장씩만 인쇄 가능합니다.\r\n첨부파일 전체를 인쇄하시려면,\r\n우측 “인쇄” 버튼을 클릭 하십시오.", "인쇄미리보기", MessageBoxButtons.OK, MessageBoxIcon.Information);
            #region 인쇄 미리보기

            #endregion

            PrintDocument pd = new PrintDocument();
            PrintDialog pDialog = new PrintDialog();

            PrintPreviewDialog ppDoc = new PrintPreviewDialog();

            //pd.PrintPage += new PrintPageEventHandler(PrintPage);
            pd.PrintPage += PrintPage2;



            pDialog.Document = pd;
            pDialog.PrinterSettings = pd.PrinterSettings;
            pDialog.AllowSomePages = true;
            pDialog.ShowHelp = true;

          

            ppDoc.Document = pd;

            ppDoc.WindowState = FormWindowState.Maximized;



            ppDoc.Show();





        }



        private void btnDown_Click(object sender, EventArgs e)
        {


            //using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //{
            //    _Connection.Open();
            //    using (SqlCommand _Command = _Connection.CreateCommand())
            //    {
            //        _Command.CommandText = $"SELECT DriverId, Name,CarYear,MobileNo FROM Drivers WHERE DriverId = '{Order.DriverId}' ";
            //        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
            //        {
            //            if (_Reader.Read())
            //            {

            //                CarYear = _Reader.GetString(2);
            //                MobileNo = _Reader.GetString(3);


            //            }
            //        }
            //    }
            //    _Connection.Close();
            //}


            //DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYTRADEFILE + "\\" + Order.DriverCarNo + "_" + CarYear);

            //if (di.Exists == false)
            //{

            //    di.Create();
            //}

            //if (!String.IsNullOrEmpty(Order.imageaima.Image1))
            //{
            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-(" + Trade.DriverBizNo.Replace("-", "") + ")-" + CarYear + "(" + Trade.DriverCarNo + ")" + "1.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(1)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }


            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}
            //if (!String.IsNullOrEmpty(Trade.Image2))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "2.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(2)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }


            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}
            //if (!String.IsNullOrEmpty(Trade.Image3))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "3.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(3)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }



            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}
            //if (!String.IsNullOrEmpty(Trade.Image4))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "4.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(4)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }



            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}
            //if (!String.IsNullOrEmpty(Trade.Image5))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "5.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(5)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }



            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}



            //if (!String.IsNullOrEmpty(Trade.Image6))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "6.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(6)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }


            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}

            //if (!String.IsNullOrEmpty(Trade.Image7))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "7.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(7)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }



            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}

            //if (!String.IsNullOrEmpty(Trade.Image8))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "8.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(8)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }


            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}
            //if (!String.IsNullOrEmpty(Trade.Image9))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "9.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(9)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }



            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}

            //if (!String.IsNullOrEmpty(Trade.Image10))
            //{

            //    var fileString = Trade.RequestDate.ToString("d").Replace("-", "") + "-" + Trade.DriverBizNo + "-" + CarYear + "(" + Trade.DriverCarNo + ")" + "10.png";
            //    var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //    FileInfo file = new FileInfo(FileName);
            //    if (file.Exists)
            //    {
            //        //if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        //    return;

            //        file.Delete();
            //    }

            //    HttpClient hClient = new HttpClient();
            //    var Stream = hClient.GetStreamAsync(GetUrl(10)).Result;
            //    var dImage = Image.FromStream(Stream);
            //    int Width = dImage.Width;
            //    int Height = dImage.Height;


            //    if (dImage.Width > dImage.Height)
            //    {
            //        dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }



            //    dImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            //}

            //try
            //{
            //    MessageBox.Show("다운로드 완료되었습니다.", "다운로드", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    Process.Start(LocalUser.Instance.PersonalOption.MYTRADEFILE);
            //}
            //catch { }



        }
    }
}
