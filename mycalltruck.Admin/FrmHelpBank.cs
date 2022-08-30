using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Xps.Packaging;

namespace mycalltruck.Admin
{
    public partial class FrmHelpBank : Form
    {
        string fileName = string.Empty;
        public string PageAddress { get; set; }
        List<Bitmap> SlideImageList = new List<Bitmap>();
        List<Slide> SlideList = new List<Slide>();
        int Index = 0;
        private ScrollPanelMessageFilter filter;

        

        public FrmHelpBank()
        {
            InitializeComponent();
            Dictionary<string, string> PayBank = new Dictionary<string, string>();

            PayBank.Add("국민은행", "국민은행");
            PayBank.Add("우리은행", "우리은행");
            PayBank.Add("신한은행", "신한은행");
            PayBank.Add("기업은행", "기업은행");
            PayBank.Add("하나은행", "하나은행");
            PayBank.Add("농협", "농협");

            cmbNaigation.DataSource = new BindingSource(PayBank, null);
            cmbNaigation.DisplayMember = "Value";
            cmbNaigation.ValueMember = "Key";

           
            cmbNaigation.SelectedIndex = 0;

            panel2.AutoScroll = false;
            panel2.AutoScroll = true;

            mPictureBox.Image = Properties.Resources.대량이체;
            mPictureBox.Height = Properties.Resources.대량이체.Height;

            cmbNaigation.SelectedIndexChanged += CmbNaigation_SelectedIndexChanged;
           
        }

        

         private void CmbNaigation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                switch (cmbNaigation.Text)
                {
                    case "국민은행":
                        
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;

                        mPictureBox.Image = Properties.Resources.대량이체;
                        mPictureBox.Height = Properties.Resources.대량이체.Height;
                        

                        break;
                    case "우리은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.우리은행대량이체 ;
                        mPictureBox.Height = Properties.Resources.우리은행대량이체.Height;

                        break;

                    case "기업은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.기업은행대량이체;
                        mPictureBox.Height = Properties.Resources.기업은행대량이체.Height;
                        break;

                    case "하나은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.하나은행대량이체;
                        mPictureBox.Height = Properties.Resources.하나은행대량이체.Height;

                        break;
                    case "농협":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.농협대량이체;
                        mPictureBox.Height = Properties.Resources.농협대량이체.Height;
                        break;

                    case "신한은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.신한은행대량이체;
                        mPictureBox.Height = Properties.Resources.신한은행대량이체.Height;

                        break;
                }
            }
            else
            {
                switch (cmbNaigation.Text)
                {
                    case "국민은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.대량이체결과;
                        mPictureBox.Height = Properties.Resources.대량이체결과.Height;

                        break;
                    case "우리은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.우리은행대량이체결과;
                        mPictureBox.Height = Properties.Resources.우리은행대량이체결과.Height;

                        break;
                    case "기업은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.기업은행대량이체결과;
                        mPictureBox.Height = Properties.Resources.기업은행대량이체결과.Height;

                        break;
                    case "하나은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.하나은행대량이체결과;
                        mPictureBox.Height = Properties.Resources.하나은행대량이체결과.Height;

                        break;
                    case "농협":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.농협이체결과;
                        mPictureBox.Height = Properties.Resources.농협이체결과.Height;

                        break;

                    case "신한은행":
                        panel2.AutoScroll = false;
                        panel2.AutoScroll = true;
                        mPictureBox.Image = Properties.Resources.신한은행대량이체결과;
                        mPictureBox.Height = Properties.Resources.신한은행대량이체결과.Height;

                        break;
                }
                
            }


            //var SlideIndex = (int)cmbNaigation.SelectedValue;
            //Index = SlideList.FirstOrDefault(c => c.SlideIndex == SlideIndex).SlidePage;
            //mPictureBox.Image = SlideImageList[Index];
        }

        private void FrmHelp_Load(object sender, EventArgs e)
        {
           
        }

        public void ActivateAndNavitation(string iformName)
        {
            Activate();
            switch (iformName)
            {
                case "FrmMN0209_CUSTOMER":
                    cmbNaigation.SelectedIndex = 3;
                    break;
                case "FrmMN0203_CAROWNERMANAGE":
                    cmbNaigation.SelectedIndex = 4;
                    break;
                case "FrmMN0301_CARGOACCEPT":
                    cmbNaigation.SelectedIndex = 5;
                    break;
                case "FrmTrade":
                    cmbNaigation.SelectedIndex = 6;
                    break;
                case "FrmMN0212_SALESMANAGE":
                    cmbNaigation.SelectedIndex = 7;
                    break;
                default:
                    break;
            }
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
        }

        private void FrmHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
               // File.Delete(fileName);
            }
            catch (Exception)
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (Index > 0)
                Index--;
            mPictureBox.Image = SlideImageList[Index];
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (Index < SlideImageList.Count -1)
                Index++;
            mPictureBox.Image = SlideImageList[Index];
        }

        class Slide
        {
            public int SlideIndex { get; set; }
            public int SlidePage { get; set; }
            public string Text { get; set; }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            System.Windows.Controls.PrintDialog mDialog = new System.Windows.Controls.PrintDialog();
            if (mDialog.ShowDialog() == true)
            {
                PrintQueue mPrintQueue = mDialog.PrintQueue;
                PrintSystemJobInfo mXPSPrintJob = mPrintQueue.AddJob();

                using (var mStream = mXPSPrintJob.JobStream)
                {

                    if (radioButton1.Checked)
                    {
                        switch (cmbNaigation.Text)
                        {
                            case "국민은행":
                                mStream.Write(Properties.Resources.국민_대량이체, 0, Properties.Resources.국민_대량이체.Length);

                                break;
                            case "우리은행":
                                mStream.Write(Properties.Resources.우리_대량이체, 0, Properties.Resources.우리_대량이체.Length);

                                break;
                            case "기업은행":
                                mStream.Write(Properties.Resources.기업_대량이체, 0, Properties.Resources.기업_대량이체.Length);

                                break;
                            case "하나은행":
                                mStream.Write(Properties.Resources.하나은행_대량이체, 0, Properties.Resources.하나은행_대량이체.Length);

                                break;
                            case "농협":
                                mStream.Write(Properties.Resources.농협_대량이체, 0, Properties.Resources.농협_대량이체.Length);

                                break;

                            case "신한은행":
                                mStream.Write(Properties.Resources.신한_대량이체, 0, Properties.Resources.신한_대량이체.Length);

                                break;
                        }
                    }
                    else
                    {
                        switch (cmbNaigation.Text)
                        {
                            case "국민은행":
                                mStream.Write(Properties.Resources.국민_이체처리결과, 0, Properties.Resources.국민_이체처리결과.Length);

                                break;
                            case "우리은행":
                                mStream.Write(Properties.Resources.우리_이체처리결과, 0, Properties.Resources.우리_이체처리결과.Length);

                                break;
                            case "기업은행":
                                mStream.Write(Properties.Resources.기업_이체처리결과, 0, Properties.Resources.기업_이체처리결과.Length);

                                break;
                            case "하나은행":
                                mStream.Write(Properties.Resources.하나은행_이체처리결과, 0, Properties.Resources.하나은행_이체처리결과.Length);

                                break;
                            case "농협":
                                mStream.Write(Properties.Resources.농협_이체처리결과, 0, Properties.Resources.농협_이체처리결과.Length);

                                break;

                            case "신한은행":
                                mStream.Write(Properties.Resources.신한_이체처리결과, 0, Properties.Resources.신한_이체처리결과.Length);

                                break;
                        }

                    }



                }

            }
            //System.Windows.Controls.PrintDialog mDialog = new System.Windows.Controls.PrintDialog();
            //if (mDialog.ShowDialog() == true)
            //{
            //    PrintQueue mPrintQueue = mDialog.PrintQueue;
            //    PrintSystemJobInfo mXPSPrintJob = mPrintQueue.AddJob();
            //    using (var mStream = mXPSPrintJob.JobStream)
            //    {

            //        if (radioButton1.Checked)
            //        {
            //            switch (cmbNaigation.Text)
            //            {
            //                case "국민은행":
            //                    mStream.Write(Properties.Resources.국민_대량이체, 0, Properties.Resources.국민_대량이체.Length);

            //                    break;
            //                case "우리은행":
            //                    mStream.Write(Properties.Resources.우리_대량이체, 0, Properties.Resources.우리_대량이체.Length);

            //                    break;
            //                case "기업은행":
            //                    mStream.Write(Properties.Resources.기업_대량이체, 0, Properties.Resources.기업_대량이체.Length);

            //                    break;
            //                case "하나은행":
            //                    mStream.Write(Properties.Resources.KEB하나_대량이체, 0, Properties.Resources.KEB하나_대량이체.Length);

            //                    break;
            //                case "농협":
            //                    mStream.Write(Properties.Resources.농협_대량이체, 0, Properties.Resources.농협_대량이체.Length);

            //                    break;

            //                case "신한은행":
            //                    mStream.Write(Properties.Resources.신한_대량이체, 0, Properties.Resources.신한_대량이체.Length);

            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            switch (cmbNaigation.Text)
            //            {
            //                case "국민은행":
            //                    mStream.Write(Properties.Resources.국민_이체처리결과, 0, Properties.Resources.국민_이체처리결과.Length);

            //                    break;
            //                case "우리은행":
            //                    mStream.Write(Properties.Resources.우리_이체처리결과, 0, Properties.Resources.우리_이체처리결과.Length);

            //                    break;
            //                case "기업은행":
            //                    mStream.Write(Properties.Resources.기업_이체처리결과, 0, Properties.Resources.기업_이체처리결과.Length);

            //                    break;
            //                case "하나은행":
            //                    mStream.Write(Properties.Resources.KEB하나_이체처리결과, 0, Properties.Resources.KEB하나_이체처리결과.Length);

            //                    break;
            //                case "농협":
            //                    mStream.Write(Properties.Resources.농협_이체처리결과, 0, Properties.Resources.농협_이체처리결과.Length);

            //                    break;

            //                case "신한은행":
            //                    mStream.Write(Properties.Resources.신한_이체처리결과, 0, Properties.Resources.신한_이체처리결과.Length);

            //                    break;
            //            }

            //        }



            //    }      
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void FrmHelpBank_Activated(object sender, EventArgs e)
        {
            filter = new ScrollPanelMessageFilter(panel2);
            Application.AddMessageFilter(filter);

        }

        private void FrmHelpBank_Deactivate(object sender, EventArgs e)
        {
            Application.AddMessageFilter(filter);

            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
                CmbNaigation_SelectedIndexChanged(null, null);
           
        }
    }
}
