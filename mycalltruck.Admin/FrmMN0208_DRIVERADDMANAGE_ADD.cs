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

namespace mycalltruck.Admin
{
    public partial class FrmMN0208_DRIVERADDMANAGE_ADD : Form
    {
        public FrmMN0208_DRIVERADDMANAGE_ADD()
        {
            InitializeComponent();

            _InitCmb();
        }
        private void FrmMN0208_DRIVERADDMANAGE_ADD_Load(object sender, EventArgs e)
        {

            DriverAddCode_Add();
            // cmb_Gubun.Focus();
            dtp_InDate.Value = DateTime.Now;
            dtp_OutDate.Value = DateTime.Now;
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");

            txt_Name.Focus();
        }
        private void _InitCmb()
        {




        }
        private void DriverAddCode_Add()
        {
            this.driverAddTableAdapter.Fill(cmDataSet.DriverAdd);
            var DriverAdd_code = cmDataSet.DriverAdd.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.DriverAddCode }).OrderByDescending(c => c.DriverAddCode).ToArray();



            if (DriverAdd_code.Count() > 0)
            {
                var DriverAddCodeCode = 1001;
                var DriverAddCandidate = cmDataSet.DriverAdd.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).OrderBy(c => c.DriverAddCode).Select(c => c.DriverAddCode).ToArray();
                while (true)
                {
                    if (!DriverAddCandidate.Any(c => c == DriverAddCodeCode.ToString()))
                    {
                        break;
                    }
                    DriverAddCodeCode++;
                }
                txt_Code.Text = DriverAddCodeCode.ToString();
            }
            else
            {

                txt_Code.Text = "1001";
            }



        }
        public bool IsSuccess = false;
        public CMDataSet.DriverAddRow CurrentCode = null;

        private int _UpdateDB()
        {
            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
            else if (cmDataSet.DriverAdd.Where(c => c.DriverAddCode == txt_Code.Text && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Count() > 0)
            {

                MessageBox.Show("코드가 중복되었습니다.!!", "코드 입력 오류");
                err.SetError(txt_Code, "코드가 중복되었습니다.!!");
                return -1;

            }

            //if (txt_SangHo.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_SangHo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;

            //}
            //else if (SingleDataSet.Instance.Drivers.Where(c => c.Name == txt_SangHo.Text.Trim()).Count() == 0)
            //{

            //    MessageBox.Show("해당 상호로 등록된  차가 없습니다.!!", "상호 입력 오류");
            //    err.SetError(txt_SangHo, "해당 상호가 없습니다.!!");
            //    // tabControl1.SelectTab(1);
            //    return -1;

            //}




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
            CMDataSet.DriverAddRow row = cmDataSet.DriverAdd.NewDriverAddRow();
            CurrentCode = row;

            row.DriverAddCode = txt_Code.Text;

            row.SangHo = txt_SangHo.Text;

            row.Name = txt_Name.Text;

            if (txt_PhoneNo.Text.Length < 8)
            {
                row.PhoneNo = "";
            }
            else
            {
                row.PhoneNo = txt_PhoneNo.Text;
            }
            if (txt_MobileNo.Text.Length < 12)
            {
                row.MobileNo = "";
            }
            else
            {
                row.MobileNo = txt_MobileNo.Text;
            }

            row.InDate = dtp_InDate.Text;
            //   row.OutDate = dtp_InDate.Text;

            row.WriteDate = DateTime.Parse(txt_CreateDate.Text);



            //  row.DriverId = int.Parse(txt_DriverId.Text);
            row.ClientId = LocalUser.Instance.LogInInformation.ClientId;

            row.OutYn = false;


            cmDataSet.DriverAdd.AddDriverAddRow(row);
            try
            {

                driverAddTableAdapter.Update(row);
            }
            catch
            {
                MessageBox.Show("영업딜러추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "기사", 1), "기사정보 추가 성공");


                //  cmb_Gubun.SelectedIndex = 0;
                txt_SangHo.Text = "";
                //   txt_CarNo.Text = "";
                txt_Name.Text = "";
                txt_PhoneNo.Text = "";
                txt_MobileNo.Text = "";
                dtp_InDate.Value = DateTime.Now;
                dtp_OutDate.Value = DateTime.Now;

                DriverAddCode_Add();



            }

            txt_Name.Focus();
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "기사", 1), "기사정보 추가 성공");
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

        private void btn_CarInfo_Click(object sender, EventArgs e)
        {
            FrmCarNumSearch2 _frmCarNumSearch2 = new FrmCarNumSearch2();
            _frmCarNumSearch2.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCarNumSearch2.grid1.SelectedCells.Count == 0) return;
                if (_frmCarNumSearch2.grid1.SelectedCells[0].RowIndex < 0) return;


                txt_SangHo.Text = _frmCarNumSearch2.grid1[1, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
                //txt_CarNo.Text = _frmCarNumSearch2.grid1[2, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txt_DriverId.Text = _frmCarNumSearch2.grid1[6, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();






                _frmCarNumSearch2.Close();
            });
            _frmCarNumSearch2.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCarNumSearch2.grid1.SelectedCells.Count == 0) return;
                if (_frmCarNumSearch2.grid1.SelectedCells[0].RowIndex < 0) return;
                txt_SangHo.Text = _frmCarNumSearch2.grid1[0, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
                // txt_CarNo.Text = _frmCarNumSearch2.grid1[1, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txt_DriverId.Text = _frmCarNumSearch2.grid1[5, _frmCarNumSearch2.grid1.SelectedCells[0].RowIndex].Value.ToString();




                _frmCarNumSearch2.Close();
            });






            _frmCarNumSearch2.Owner = this;
            _frmCarNumSearch2.StartPosition = FormStartPosition.CenterParent;
            _frmCarNumSearch2.ShowDialog();
            txt_Name.Focus();

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


            //pnProgress.Visible = true;
            //bar.Value = 0;
            //Thread t = new Thread(new ThreadStart(() =>
            //{


            //    dataGridView1.ExportExistExcel2_xlsx(title, fileString, bar, true, ieExcel, 3, LocalUser.Instance.LogInInfomation.DRIVER);
            //    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));





            //}));
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();

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
