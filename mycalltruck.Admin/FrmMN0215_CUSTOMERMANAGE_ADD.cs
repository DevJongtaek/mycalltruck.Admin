using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.IO;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class FrmMN0215_CUSTOMERMANAGE_ADD : Form
    {
        public FrmMN0215_CUSTOMERMANAGE_ADD()
        {
            InitializeComponent();

            _InitCmb();
        }
        private void FrmMN0208_DRIVERADDMANAGE_ADD_Load(object sender, EventArgs e)
        {

            DriverAddCode_Add();
            // cmb_Gubun.Focus();        
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");

            txt_Name.Focus();
        }
        private void _InitCmb()
        {




        }
        private void DriverAddCode_Add()
        {
            
            this.customerManagerTableAdapter.Fill(customerManagerDataSet.CustomerManager);
            var ManagerCode = customerManagerDataSet.CustomerManager.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.ManagerCode }).OrderByDescending(c => c.ManagerCode).ToArray();



            if (ManagerCode.Count() > 0)
            {
                var sManagerCode = 1001;
                var ManagerCodeCandidate = customerManagerDataSet.CustomerManager.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).OrderBy(c => c.ManagerCode).Select(c => c.ManagerCode).ToArray();
                while (true)
                {
                    if (!ManagerCodeCandidate.Any(c => c == sManagerCode.ToString()))
                    {
                        break;
                    }
                    sManagerCode++;
                }
                txt_Code.Text = sManagerCode.ToString();
            }
            else
            {

                txt_Code.Text = "1001";
            }



        }
        public bool IsSuccess = false;
        public CustomerManagerDataSet.CustomerManagerRow CurrentCode = null;

        private int _UpdateDB()
        {
            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
            else if (customerManagerDataSet.CustomerManager.Where(c => c.ManagerCode == txt_Code.Text && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Count() > 0)
            {

                MessageBox.Show("코드가 중복되었습니다.!!", "코드 입력 오류");
                err.SetError(txt_Code, "코드가 중복되었습니다.!!");
                return -1;

            }

            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }


            if (txt_PhoneNo.Text.Replace("-", "").Length < 9)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }


            if (txt_MobileNo.Text.Replace("-", "").Length < 10)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
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
            CustomerManagerDataSet.CustomerManagerRow row = customerManagerDataSet.CustomerManager.NewCustomerManagerRow();
                
            CurrentCode = row;

            row.ManagerCode = txt_Code.Text;

            row.ManagerName = txt_Name.Text;

            if (txt_PhoneNo.Text.Length < 8)
            {
                row.ManagerPhoneNo = "";
            }
            else
            {
                row.ManagerPhoneNo = txt_PhoneNo.Text;
            }
            if (txt_MobileNo.Text.Length < 12)
            {
                row.ManagerMobileNo = "";
            }
            else
            {
                row.ManagerMobileNo = txt_MobileNo.Text;
            }

          

            row.CreateDate = DateTime.Parse(txt_CreateDate.Text);



            //  row.DriverId = int.Parse(txt_DriverId.Text);
            row.ClientId = LocalUser.Instance.LogInInformation.ClientId;


            customerManagerDataSet.CustomerManager.AddCustomerManagerRow(row);
          
            try
            {
                customerManagerTableAdapter.Update(row);
            }
            catch
            {
                MessageBox.Show("화주담당자 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "화주담당자", 1), "화주담당자 추가 성공");


           
                txt_Name.Text = "";
                txt_PhoneNo.Text = "";
                txt_MobileNo.Text = "";
              

                DriverAddCode_Add();



            }

            txt_Name.Focus();
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "화주담당자", 1), "화주담당자 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void btnExcelImport_Click(object sender, EventArgs e)
        {
            EXCELINSERT_DRIVERMANAGE _Form = new EXCELINSERT_DRIVERMANAGE();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
        }

        private void btn_DriverExcel_Click(object sender, EventArgs e)
        {
            string title = string.Empty;
            byte[] ieExcel;

            string fileString = string.Empty;


            fileString = "기사관리입력양식_" + DateTime.Now.ToString("yyyyMMdd")+".xlsx";
            title = "sheet1";
            ieExcel = Properties.Resources.기사관리입력양식;


         

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {

                di.Create();
            }
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                file.Delete();
            }
            File.WriteAllBytes(FileName, ieExcel);
            System.Diagnostics.Process.Start(FileName);


        }


    }
}
