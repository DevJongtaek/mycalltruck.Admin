using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN301_Call_PopupSetting : Form
    {
        EncryptSHA encryptSHA = new EncryptSHA();
        public FrmMN301_Call_PopupSetting()
        {
            InitializeComponent();

            txtID.Text = CallHelper.Instance.Id;
            txtPassword.Text = CallHelper.Instance.Password;
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("http://centrex.uplus.co.kr/premium/index.html");
        }

        private void FrmMN301_Call_PopupSetting_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           

            if (!String.IsNullOrEmpty(txtID.Text) && !String.IsNullOrEmpty(txtPassword.Text))
            {
                CallHelper.Instance.Login(txtID.Text, txtPassword.Text);
            }
            string SVC_RT = "";
            string SVC_MSG = "";
            #region
            string sha512password = encryptSHA.EncryptSHA512(CallHelper.Instance.Password);




            string Parameter = "";
            Parameter = String.Format(@"id={0}&pass={1}", CallHelper.Instance.Id, sha512password, txtID.Text, txtPassword.Text);




            JObject response = null;

            var uriBuilder = new UriBuilder("https://centrex.uplus.co.kr/RestApi/userinfo");






            uriBuilder.Query = Parameter;
            Uri finalUrl = uriBuilder.Uri;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);



            request.Method = "POST";
            request.ContentType = "text/json;";
            request.ContentLength = 0;

            request.Headers.Add("header-staff-api", "value");

            var httpResponse = (HttpWebResponse)request.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = JObject.Parse(streamReader.ReadToEnd());

                SVC_RT = response["SVC_RT"].ToString();
                SVC_MSG = response["SVC_MSG"].ToString();

                if (SVC_RT == "0000")
                {
                    MessageBox.Show($"{txtID.Text}\r\n\"로그인\"에 성공 했습니다.\r\n\r\nCRM 및 문자를 사용하실 수 있습니다.","인터넷전화 로그인");

                    CallHelper.Instance.Id = txtID.Text;
                    CallHelper.Instance.Password = txtPassword.Text;
                    var Json = JsonConvert.SerializeObject(new
                    {
                        Id = txtID.Text,
                        Password = txtPassword.Text
                    });
                    File.WriteAllText(CallHelper.Instance._GetConfigFile(), Json, Encoding.UTF8);
                }
                else
                {
                    MessageBox.Show($"{txtID.Text}\r\n\"로그인\"에 실패 했습니다.\r\n\r\nCRM 및 문자를 사용하실 수 없습니다.","인터넷전화 로그인");
                }

            }
            #endregion


          
            // MessageBox.Show("저장이 완료되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);



           
            // 여기서 접속시도
            //  CallHelper.Instance.Login(CallHelper.Instance.Id, CallHelper.Instance.Password);



            //if(CallHelper.Instance.IsLogined)
            //{
            //   // MessageBox.Show("로그인");

            //}
            //else
            //{
            //    //   MessageBox.Show("로그아웃");
            //}



            // 맞으면 파일로 해당정보 저장하고
            // 틀리면 전화번호/비밀번호 클리어 혹은 이전 정보로 돌아가기
        }
        bool isMove = false;
        Point fpt;
        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
        //    isMove = true;
        //    fpt = new Point(e.X, e.Y);
        }

        private void tableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
           // isMove = false;
        }

        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
           // isMove = false;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            //if (isMove && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            //{
            //    Location = new Point(this.Left - (fpt.X - e.X), this.Top - (fpt.Y - e.Y));
            //}
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            //isMove = true;
            //fpt = new Point(e.X, e.Y);
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            fpt = new Point(e.X, e.Y);
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (fpt.X - e.X), this.Top - (fpt.Y - e.Y));
            }
        }
    }
}
