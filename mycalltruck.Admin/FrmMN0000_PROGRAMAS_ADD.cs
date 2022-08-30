using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0000_PROGRAMAS_ADD : Form
    {
        public FrmMN0000_PROGRAMAS_ADD()
        {
            InitializeComponent();
            _InitCmb();
        }

        private void FrmMN0000_PROGRAMAS_ADD_Load(object sender, EventArgs e)
        {
            LocalUser.Instance.LogInInformation.LoadClient();
            dtp_RequestDate.Value = DateTime.Now;
            txt_ClientName.Text = LocalUser.Instance.LogInInformation.Client.Name;
            txt_PhoneNo.Focus();
            CurrentCode = cmDataSet.CardPayAS.NewCardPayASRow();
        }
        private void _InitCmb()
        {




        }
        public bool IsSuccess = false;
         public CMDataSet.CardPayASRow CurrentCode = null;

        private int _UpdateDB()
        {
            err.Clear();


            if (txt_PhoneNo.Text.Replace("-", "").Length < 9)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }




            if (txt_Contents.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Contents, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }












            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Code") iName = "코드";
                if (code == "Name") iName = "기사명";
                if (code == "SangHo") iName = "상호";
                if (code == "CarNo") iName = "차량번호";


                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        private void _AddClient()
        {

            CurrentCode.CreateDate = DateTime.Now;
            CurrentCode.RequestDate = dtp_RequestDate.Value;
            CurrentCode.ClientName = txt_ClientName.Text;
            CurrentCode.ClientId = LocalUser.Instance.LogInInformation.ClientId;
            if (txt_PhoneNo.Text.Length < 8)
            {
                CurrentCode.PhoneNo = "";
            }
            else
            {
                CurrentCode.PhoneNo = txt_PhoneNo.Text;
            }



            CurrentCode.Contents = txt_Contents.Text;

            CurrentCode.ProcessState = "접수";
            cmDataSet.CardPayAS.AddCardPayASRow(CurrentCode);
            try
            {

                cardPayASTableAdapter.Update(CurrentCode);
            }
            catch
            {
                MessageBox.Show("프로그램A/S 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
    private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "프로그램 A/S", 1), "프로그램A/S 추가 성공");

                dtp_RequestDate.Value = DateTime.Now;
                txt_PhoneNo.Clear();
                txt_FileName1.Clear();
                txt_FileName2.Clear();
                txt_Contents.Clear();
                CurrentCode = cmDataSet.CardPayAS.NewCardPayASRow();
            }

            txt_PhoneNo.Focus();
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "프로그램 A/S", 1), "프로그램 A/S 추가 성공");
                IsSuccess = true;
                Close();
            }
            else { }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_File1_Click(object sender, EventArgs e)
        {
            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;



            using (MemoryStream ms = new MemoryStream())
            using (MemoryStream mm = new MemoryStream())
            {
                Bitmap _b = null;
                try
                {
                    var b = File.ReadAllBytes(dlgOpen.FileName);
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                try
                {
                    _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = mm.ToArray();

                    AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                    i.DriverId = 0;
                    i.ImageData64String = System.Convert.ToBase64String(bytes);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetASPaper"));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string parsedContent = JsonConvert.SerializeObject(i);
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] b = encoding.GetBytes(parsedContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(b, 0, b.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    var o =  (dynamic)JsonConvert.DeserializeObject(content);
                    if ((bool)o.Result)
                    {
                        txt_FileName1.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
                        CurrentCode.FileName1 = txt_FileName1.Text;
                        CurrentCode.FilePath1 = (string)o.FilePath;
                        MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }


        }

        private void btn_File2_Click(object sender, EventArgs e)
        {
            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;



            using (MemoryStream ms = new MemoryStream())
            using (MemoryStream mm = new MemoryStream())
            {
                Bitmap _b = null;
                try
                {
                    var b = File.ReadAllBytes(dlgOpen.FileName);
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                try
                {
                    _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = mm.ToArray();

                    AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                    i.DriverId = 0;
                    i.ImageData64String = System.Convert.ToBase64String(bytes);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetASPaper"));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string parsedContent = JsonConvert.SerializeObject(i);
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] b = encoding.GetBytes(parsedContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(b, 0, b.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    var o = (dynamic)JsonConvert.DeserializeObject(content);
                    if ((bool)o.Result)
                    {
                        txt_FileName2.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
                        CurrentCode.FileName2 = txt_FileName2.Text;
                        CurrentCode.FilePath2 = (string)o.FilePath;
                        MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        private void txt_Contents_TextChanged(object sender, EventArgs e)
        {
            #region 텍스트 길이 체크
            try
            {
                char[] msg_chars = this.txt_Contents.Text.ToCharArray();
                int len = 0;
                foreach (char msg_char in msg_chars)
                {
                    if (char.IsDigit(msg_char) || char.IsWhiteSpace(msg_char) || char.IsUpper(msg_char) || char.IsLower(msg_char))
                    {
                        len++;
                    }
                    else
                    {
                        len += 2;
                    }
                }
                this.lbl_txtLengh.Text = " " + len.ToString() + " / 5000";
                if (len > 5000)
                {
                    MessageBox.Show("5000 byte를 초과하여 입력할 수 없습니다.");
                    this.txt_Contents.Text = this.txt_Contents.Text.Remove(txt_Contents.Text.Length - 1);
                }
            }
            catch (Exception ex)
            {
                // MessageManager.ShowMessage(MessageType.Error, "오류가 발생했습니다.", ex, false);
            }
            #endregion
        }
    }


}
