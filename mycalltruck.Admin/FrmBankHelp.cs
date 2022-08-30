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
    public partial class FrmBankHelp : Form
    {
        string fileName = string.Empty;
        public string PageAddress { get; set; }
        List<Bitmap> SlideImageList = new List<Bitmap>();
        List<Slide> SlideList = new List<Slide>();
        int Index = 0;
        public FrmBankHelp()
        {
            InitializeComponent();
            //SlideImageList.Add(Properties.Resources.슬라이드1);
            //SlideImageList.Add(Properties.Resources.슬라이드2);
            //SlideImageList.Add(Properties.Resources.슬라이드3);
            //SlideImageList.Add(Properties.Resources.슬라이드4);
            //SlideImageList.Add(Properties.Resources.슬라이드5);
            //SlideImageList.Add(Properties.Resources.슬라이드6);
            //SlideImageList.Add(Properties.Resources.슬라이드7);
            //SlideImageList.Add(Properties.Resources.슬라이드8);
            //SlideImageList.Add(Properties.Resources.슬라이드9);
            //SlideImageList.Add(Properties.Resources.슬라이드10);
            //SlideImageList.Add(Properties.Resources.슬라이드11);
            //SlideImageList.Add(Properties.Resources.슬라이드12);
            //SlideImageList.Add(Properties.Resources.슬라이드13);
            //SlideImageList.Add(Properties.Resources.슬라이드14);
            //SlideImageList.Add(Properties.Resources.슬라이드15);
            //SlideImageList.Add(Properties.Resources.슬라이드16);
            //SlideImageList.Add(Properties.Resources.슬라이드17);
            //SlideImageList.Add(Properties.Resources.슬라이드18);
            //SlideImageList.Add(Properties.Resources.슬라이드19);
            //SlideImageList.Add(Properties.Resources.슬라이드20);
            //SlideImageList.Add(Properties.Resources.슬라이드21);
            //SlideImageList.Add(Properties.Resources.슬라이드22);
            //SlideImageList.Add(Properties.Resources.슬라이드23);
            //SlideImageList.Add(Properties.Resources.슬라이드24);
            //SlideImageList.Add(Properties.Resources.슬라이드25);
            //SlideImageList.Add(Properties.Resources.슬라이드26);
            //SlideImageList.Add(Properties.Resources.슬라이드27);
            //SlideImageList.Add(Properties.Resources.슬라이드28);
            //SlideImageList.Add(Properties.Resources.슬라이드29);
            //SlideImageList.Add(Properties.Resources.슬라이드30);
            //SlideImageList.Add(Properties.Resources.슬라이드31);
            //SlideImageList.Add(Properties.Resources.슬라이드32);
            //SlideImageList.Add(Properties.Resources.슬라이드33);
            //SlideImageList.Add(Properties.Resources.슬라이드34);
            //SlideImageList.Add(Properties.Resources.슬라이드35);
            //SlideImageList.Add(Properties.Resources.슬라이드36);
            //SlideImageList.Add(Properties.Resources.슬라이드37);
            //SlideImageList.Add(Properties.Resources.슬라이드38);
            //SlideImageList.Add(Properties.Resources.슬라이드39);
            //SlideImageList.Add(Properties.Resources.슬라이드40);
            //SlideImageList.Add(Properties.Resources.슬라이드41);
            //SlideImageList.Add(Properties.Resources.슬라이드42);
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 0,
            //    SlidePage = 0,
            //    Text = "0. 시작",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 1,
            //    SlidePage = 2,
            //    Text = "1. 프로그램 설치",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 2,
            //    SlidePage = 3,
            //    Text = "2. 로그인",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 3,
            //    SlidePage = 4,
            //    Text = "3. 거래처 관리",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 4,
            //    SlidePage = 7,
            //    Text = "4. 차량 관리",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 5,
            //    SlidePage = 12,
            //    Text = "5. 배차 관리",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 6,
            //    SlidePage = 16,
            //    Text = "6. 매입 관리",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 7,
            //    SlidePage = 26,
            //    Text = "7. 매출 관리",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 8,
            //    SlidePage = 34,
            //    Text = "8. 통계 관리",
            //});
            //SlideList.Add(new Slide
            //{
            //    SlideIndex = 9,
            //    SlidePage = 37,
            //    Text = "9. 어플 화면설명",
            //});

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

            //cmbNaigation.DataSource = SlideList;
            //cmbNaigation.SelectedIndex = 0;
            cmbNaigation.SelectedIndexChanged += CmbNaigation_SelectedIndexChanged;
        }

        private void CmbNaigation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var SlideIndex = (int)cmbNaigation.SelectedValue;
            //Index = SlideList.FirstOrDefault(c => c.SlideIndex == SlideIndex).SlidePage;
            //mPictureBox.Image = SlideImageList[Index];
        }

        private void FrmHelp_Load(object sender, EventArgs e)
        {
        }

        public void ActivateAndNavitation(string iformName)
        {
            //Activate();
            //switch (iformName)
            //{
            //    case "FrmMN0209_CUSTOMER":
            //        cmbNaigation.SelectedIndex = 3;
            //        break;
            //    case "FrmMN0203_CAROWNERMANAGE":
            //        cmbNaigation.SelectedIndex = 4;
            //        break;
            //    case "FrmMN0301_CARGOACCEPT":
            //        cmbNaigation.SelectedIndex = 5;
            //        break;
            //    case "FrmTrade":
            //        cmbNaigation.SelectedIndex = 6;
            //        break;
            //    case "FrmMN0212_SALESMANAGE":
            //        cmbNaigation.SelectedIndex = 7;
            //        break;
            //    default:
            //        break;
            //}
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
        }

        private void FrmHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                File.Delete(fileName);
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
                    mStream.Write(Properties.Resources.카드페이_운송사_메뉴얼_Ver2_0, 0, Properties.Resources.카드페이_운송사_메뉴얼_Ver2_0.Length);
                }
            }
        }
    }
}
