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
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.IO;

namespace mycalltruck.Admin
{
    public partial class FRMMNNOTICEDRIVER_ADD : Form
    {

        bool Account_Result = false;
        public FRMMNNOTICEDRIVER_ADD()
        {
            InitializeComponent();

            _InitCmb();
        }

        private void FRMMNNOTICEDRIVER_ADD_Load(object sender, EventArgs e)
        {
            clientsTableAdapter.Fill(cmDataSet.Clients);
            
            var Query = cmDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();


            txt_CreateDate.Text = DateTime.Now.ToString("yyy-MM-dd").Replace("-", "/");
            txt_ClientCode.Text = Query.First().Code;
            txt_ClientName.Text = Query.First().Name;
        }
        private void _InitCmb()
        {

            var AddressStateDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.State }).Distinct().ToArray();
            cmb_AddressState.DataSource = AddressStateDataSource;
            cmb_AddressState.DisplayMember = "State";
            cmb_AddressState.ValueMember = "State";


            var AddressCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            cmb_AddressCity.DataSource = AddressCityDataSource;
            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";

          


            //var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            //cmb_CarType.DataSource = CarTypeDataSource;
            cmb_CarType.DisplayMember = "Name";
            cmb_CarType.ValueMember = "Value";


            var CarSizeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value != 0 && c.Value != 99).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarSize.DataSource = CarSizeDataSource;
            cmb_CarSize.DisplayMember = "Name";
            cmb_CarSize.ValueMember = "Value";
            cmb_CarSize.SelectedIndex = 0;
            cmb_CarSize_SelectedIndexChanged(null, null);
        }

        public bool IsSuccess = false;
        public CMDataSet.NOTICEDRIVERRow CurrentCode = null;

        private int _UpdateDB()
        {
            err.Clear();

            noticedriverTableAdapter.Fill(this.cmDataSet.NOTICEDRIVER);



            if (txt_MobileNo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }
            else
            {
                var Query = cmDataSet.NOTICEDRIVER.Where(c => c.MobileNo.Replace("-", "") == txt_MobileNo.Text.Replace("-", "") && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

                if(Query.Count() > 0)
                {
                    MessageBox.Show("이미 등록된 핸드폰번호 입니다.!!", "핸드폰번호 입력 오류");
                    err.SetError(txt_MobileNo, "핸드폰번호가 중복");
                    return -1;

                }
                
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
            CMDataSet.NOTICEDRIVERRow row = cmDataSet.NOTICEDRIVER.NewNOTICEDRIVERRow();
            CurrentCode = row;

            row.State = cmb_AddressState.SelectedValue.ToString();
            row.City = cmb_AddressCity.SelectedValue.ToString();
            row.MobileNo = txt_MobileNo.Text;
            row.CarType = int.Parse(cmb_CarType.SelectedValue.ToString());
            row.CarSize = int.Parse(cmb_CarSize.SelectedValue.ToString());

            row.ClientId = LocalUser.Instance.LogInInformation.ClientId;
            row.CreateDate = DateTime.Now;




            cmDataSet.NOTICEDRIVER.AddNOTICEDRIVERRow(row);
            try
            {
                noticedriverTableAdapter.Update(row);
            }
            catch
            {
                MessageBox.Show("카톡배차 차량추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "카톡배차 차량", 1), "카톡배차 차량 추가 성공");



                cmb_AddressState.SelectedIndex = 0;

                cmb_AddressCity.SelectedIndex = 0;
                txt_MobileNo.Clear();
                cmb_CarSize.SelectedIndex = 0;
                cmb_CarSize_SelectedIndexChanged(null, null);
                txt_CreateDate.Text = DateTime.Now.ToString();
            }

            txt_MobileNo.Focus();
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "카톡배차 차량", 1), "카톡배차 차량 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExcelImport_Click(object sender, EventArgs e)
        {
            EXCELINSERT_NOTICEDRIVER _Form = new EXCELINSERT_NOTICEDRIVER();
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


            fileString = "알림톡일괄등록양식" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            title = "sheet1";
            ieExcel = Properties.Resources.알림톡일괄등록양식;



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

        private void cmb_AddressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_AddressCity.Enabled = true;

            cmb_AddressCity.DataSource = null;
            var CarCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            cmb_AddressCity.DataSource = CarCityDataSource;

            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";

        }

        private void cmb_CarSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] SmallCarTypeValues = new int[]{
                2, 3
            };
            int[] MediumCarTypeValues = new int[]{
                0,1,4,5,6,8,9,10,14,16,18,20,21,60,61,62,22,63,64,65,66,67,27,68,69,33,34,70,71
            };
            int[] FiveCarTypeValues = new int[]{
                0,1,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59
            };
            int[] LargeCarTypeValues = new int[]{
                0,1,4,5,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59
            };

            int CarSizeValue = (int)cmb_CarSize.SelectedValue;
            if (CarSizeValue == 1)
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 2;
            }
            else if (CarSizeValue == 2)
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 3;
            }
            else if (new int[] { 3, 4, 5, 6 }.Contains(CarSizeValue))
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && MediumCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 0;
            }
            else if (new int[] { 7 }.Contains(CarSizeValue))
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && FiveCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 0;
            }
            else if (CarSizeValue > 7)
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && LargeCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 0;
            }
        }
    }
}
