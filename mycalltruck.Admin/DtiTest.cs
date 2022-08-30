using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;


namespace mycalltruck.Admin
{
    public partial class DtiTest : Form
    {
        DTIServiceClass DTIService = new DTIServiceClass();
        

        /// <summary>
        /// 연계코드
        /// </summary>
        string linkCd = "EDB";
        /// <summary>
        /// 연계사 사용중인 ID
        /// </summary>
        string linkId = "edubillsys";    
        /// <summary>
        /// 사업자번호(-제외)
        /// </summary>
        string bizNo = "";
        /// <summary>
        /// 회사명
        /// </summary>
        string custName = "";
        /// <summary>
        /// 대표자명
        /// </summary>
        string ownerName = "";
        /// <summary>
        /// 업태
        /// </summary>
        string bizCond = "";
        /// <summary>
        /// 종목
        /// </summary>
        string bizItem = "";
        /// <summary>
        /// 담당자명
        /// </summary>
        string rsbmName = "";
        /// <summary>
        /// 이메일
        /// </summary>
        string email = "";
        /// <summary>
        /// 전화번호
        /// </summary>
        string telNo = "";
        /// <summary>
        /// 휴대폰번호
        /// </summary>
        string hpNo = "";
        /// <summary>
        /// 우편번호
        /// </summary>
        string zipCode = "";
        /// <summary>
        /// 주소1
        /// </summary>
        string addr1 = "";
        /// <summary>
        /// 주소2
        /// </summary>
        string addr2 = "";

        /// <summary>
        /// 결과코드
        /// </summary>
        string retVal = "";
        //오류메시지
        string errMsg = "";
        /// <summary>
        /// 고객사코드
        /// </summary>
        string frnNo = "1480606";
        /// <summary>
        /// 사용자ID
        /// </summary>
        string userid = "";
        /// <summary>
        /// 사용자 PW
        /// </summary>
        string passwd = "";


        


        public DtiTest()
        {
            InitializeComponent();

            LocalUser.Instance.LogInInformation.LoadClient();
        }

        private void DtiTest_Load(object sender, EventArgs e)
        {
            bizNo = LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", "");
            custName = LocalUser.Instance.LogInInformation.Client.Name;
            ownerName = LocalUser.Instance.LogInInformation.Client.CEO;
            bizCond = LocalUser.Instance.LogInInformation.Client.Uptae;
            bizItem = LocalUser.Instance.LogInInformation.Client.Upjong;
            rsbmName = LocalUser.Instance.LogInInformation.Client.CEO;
            email = LocalUser.Instance.LogInInformation.Client.Email;
            telNo = LocalUser.Instance.LogInInformation.Client.PhoneNo;
            hpNo = LocalUser.Instance.LogInInformation.Client.MobileNo;
            zipCode = LocalUser.Instance.LogInInformation.Client.ZipCode.Replace("-", "");
            addr1 = LocalUser.Instance.LogInInformation.Client.AddressState + " " + LocalUser.Instance.LogInInformation.Client.AddressCity;
            addr2 = LocalUser.Instance.LogInInformation.Client.AddressDetail;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            lblErrorMessage.Text = "";


            var Result = DTIService.Memberjoin(linkCd, linkId, bizNo, custName, ownerName, bizCond, bizItem, rsbmName, email, telNo, hpNo, zipCode, addr1, addr2);

            if (!String.IsNullOrEmpty(Result))
            {
                var ResultList = Result.Split('/');

                try
                {
                    lblErrorMessage.Text = ResultList[0] + " / " + ResultList[1] + " / " + ResultList[2] + " / " + ResultList[3] + " / " + ResultList[4];

                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }


        }
        


        //가입정보조회
        private void button2_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            var Result = DTIService.GetMembJoinInf(txtLinkCd.Text, txtLinkId.Text, txtBizNo.Text, rsbmName, email, hpNo);

            if(!String.IsNullOrEmpty(Result))
            {
                var ResultList =  Result.Split('/');

                try
                {
                    lblErrorMessage.Text = ResultList[0] + " / " + ResultList[1] + " / " + ResultList[2] + " / " + ResultList[3] + " / " + ResultList[4];

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            var Result = DTIService.SelectMembInfo(txtLinkCd.Text, frnNo, txtLinkId.Text, txtPassword.Text);

            if (!String.IsNullOrEmpty(Result))
            {
                var ResultList = Result.Split('/');

                try
                {
                    lblErrorMessage.Text = ResultList[0] + " / " + ResultList[1] + " / " + ResultList[2] + " / " + ResultList[3] + " / " + ResultList[4];

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DTIXml.Instance.Write();
        }
    }
}
