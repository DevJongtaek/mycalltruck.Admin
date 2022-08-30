using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace mycalltruck.Admin
{
    public partial class FrmMN0207_SALESMANAGE_Add : Form
    {
        public FrmMN0207_SALESMANAGE_Add()
        {
            InitializeComponent();
            this.dealersTableAdapter.Fill(this.cmDataSet.Dealers);
            _InitCmb();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "영업딜러", 1), "영업딜러정보 추가 성공");

              //  txtidChk.Text = "";

                //txt_Code.Text = "";
                txt_Name.Text = "";
                txt_BizNo.Text = "";
                txt_CEO.Text = "";
                txt_Uptae.Text = "";
                txt_Upjong.Text = "";
                txt_Email.Text = "";
                txt_LoignId.Text = "";
                txt_Password.Text = "";
                txt_MobileNo.Text = "";
                txt_PhoneNo.Text = "";
                txt_FaxNo.Text = "";
                txt_ZipCode.Text = "";
                txt_LogoImage.Text = "";
                cmb_Status.SelectedIndex = 1;
                txt_Addr.Text = "";
                txt_AddrDetail.Text = "";

                cmb_Bank.SelectedIndex = 0;
                txt_LogoImage.Text = "";
                txt_AccountNo.Text = "";
                txt_AccountOwner.Text = "";
                picLogo.Image = null;
                cmb_bizStatus.SelectedIndex = 0;
                cmb_BizName.SelectedIndex = 0;
                DealersCode_Add();

               

            }

            txt_Name.Focus();
        }

        public bool IsSuccess = false;
        public CMDataSet.DealersRow CurrentCode = null;

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "영업딜러", 1), "영업딜러정보 추가 성공");
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

        private void FrmMN0207_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            DealersCode_Add();
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            cmb_Status.SelectedIndex = 1;
            cmb_bizStatus.SelectedIndex = 0;
            txt_BizNo.Mask = "999999-9999999";
            txt_Name.Focus();
        }

        private void DealersCode_Add()
        {
            this.dealersTableAdapter.Fill(this.cmDataSet.Dealers);
            var Dealer_code = cmDataSet.Dealers.Select(c => new { c.Code }).OrderByDescending(c => c.Code).ToArray();

            //int aaa = Dealer_code.Code.Count();
            //int Dealer_codeint = 0;

            //if (Dealer_code.Code.Count() > 0)
            //{
            //    Dealer_codeint = int.Parse(Dealer_code.Code) + 1;
            //    txt_Code.Text = Dealer_codeint.ToString();
            //}
            //else
            //{
            //    txt_Code.Text = "1000";
            //}


            if (Dealer_code.Count() > 0)
            {
                var DealersCode = 1001;
                var DealersCandidate = cmDataSet.Dealers.OrderBy(c => c.Code).Select(c => c.Code).ToArray();
                while (true)
                {
                    if (!DealersCandidate.Any(c => c == DealersCode.ToString()))
                    {
                        break;
                    }
                    DealersCode++;
                }
                txt_Code.Text = DealersCode.ToString();
            }
            else
            {

                txt_Code.Text = "1001";
            }



        }

        private int _UpdateDB()
        {
            this.dealersTableAdapter.Fill(this.cmDataSet.Dealers);
            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
            else if (cmDataSet.Dealers.Where(c => c.Code == txt_Code.Text).Count() > 0) 
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
            if (cmb_bizStatus.SelectedIndex == 0)
            {
                if (txt_BizNo.Text.Length != 14)
                {
                    MessageBox.Show("주민 번호가 완전하지 않습니다.", "주민 번호 불완전");
                    err.SetError(txt_BizNo, "주민 번호가 완전하지 않습니다.");

                    return -1;
                }
                else if (cmDataSet.Dealers.Where(c => c.BizNo == txt_BizNo.Text).Count() > 0)
                {

                    MessageBox.Show("주민 번호가 중복되었습니다.!!", "주민 번호 입력 오류");
                    err.SetError(txt_BizNo, "주민 번호가 중복되었습니다.!!");
                    return -1;

                }

                txt_BizNo.Mask = "999999-9999999";
            }
            else
            {
                if (txt_BizNo.Text.Length != 12)
                {
                    MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                    err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                    return -1;
                }

                else if (cmDataSet.Dealers.Where(c => c.BizNo == txt_BizNo.Text).Count() > 0)
                {

                    MessageBox.Show("사업자 번호가 중복되었습니다.!!", "사업자 번호 입력 오류");
                    err.SetError(txt_BizNo, "사업자 번호가 중복되었습니다.!!");
                    return -1;

                }
                txt_BizNo.Mask = "999-99-99999";
            }

            
            
            if (txt_CEO.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_Uptae.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_Upjong.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            //if (txt_LoignId.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_LoignId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;
            //}

            if (txt_LoignId.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_LoignId, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            else if (cmDataSet.Dealers.Where(c => c.LoginId == txt_LoignId.Text).Count() > 0)
            {

                MessageBox.Show("아이디가 중복되었습니다.!!", "아이디 입력 오류");
                err.SetError(txt_LoignId, "아이디가 중복되었습니다.!!");
                return -1;

            }
            if (txt_Password.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Password, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (txt_MobileNo.Text.Length < 10)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (String.IsNullOrEmpty(txt_ZipCode.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_ZipCode, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            

            if (String.IsNullOrEmpty(txt_Addr.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Addr, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (String.IsNullOrEmpty(txt_AddrDetail.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AddrDetail, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            
            if (cmb_Bank.SelectedIndex == 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(cmb_Bank, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

           

            if (txt_AccountOwner.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AccountOwner, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            if (txt_AccountNo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AccountNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
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
                if (code == "Name") iName = "상호";
                if (code == "ID_Code") iName = "사업자 번호";
                if (code == "CEO_Name") iName = "대표자명";
                if (code == "Zip_Code" || code == "Addr" || code == "Addr_Detail") iName = "주소";
                if (code == "Uptae") iName = "업태";
                if (code == "Upjong") iName = "업종";
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
            CMDataSet.DealersRow row = cmDataSet.Dealers.NewDealersRow();
            CurrentCode = row;

            row.Code = txt_Code.Text;
            row.Name = txt_Name.Text;
            row.BizNo = txt_BizNo.Text;
            row.CEO = txt_CEO.Text;
            row.Uptae = txt_Uptae.Text;
            row.Upjong = txt_Upjong.Text;
            row.Email = txt_Email.Text;
            row.LoginId = txt_LoignId.Text;
            row.Password = txt_Password.Text;

            if (txt_MobileNo.Text.Length  < 12)
            {
                row.MobileNo = "";
            }
            else
            {
                row.MobileNo = txt_MobileNo.Text;
            }

            if (txt_PhoneNo.Text.Length < 8)
            {
                row.PhoneNo = "";
            }
            else
            {
                row.PhoneNo = txt_PhoneNo.Text;
            }
            if (txt_FaxNo.Text.Length < 12)
            {
                row.FaxNo = "";
            }

            else
            {
                row.FaxNo = txt_FaxNo.Text;
            }

            row.CreateDate = DateTime.Parse(txt_CreateDate.Text);
            row.ZipCode = txt_ZipCode.Text;
            row.Address = txt_Addr.Text;
            row.AddressDetail = txt_AddrDetail.Text;
            row.Status = int.Parse(cmb_Status.SelectedValue.ToString());
            row.BankName =cmb_Bank.SelectedValue.ToString();
            row.AccountNo = txt_AccountNo.Text;
            row.AccountOwner = txt_AccountOwner.Text;
            
            if (txt_LogoImage.Text != "" || txt_LogoImage.Text != "" || txt_LogoImage.Text != "")
            {
                FileStream fs = new FileStream(txt_LogoImage.Text, FileMode.Open, FileAccess.Read);
                  byte[] bImage = new byte[fs.Length];
                  fs.Read(bImage, 0, (int)fs.Length);


            

              
                row.LogoImage = bImage;
            }




            if (cmb_bizStatus.SelectedIndex == 0)
            {
                row.BizName = int.Parse(cmb_BizName.SelectedValue.ToString());
            }
            else
            {
                row.BizName = 0;
            }




            cmDataSet.Dealers.AddDealersRow(row);
            try
            {

                dealersTableAdapter.Update(row);
            }
            catch
            {
                MessageBox.Show("영업딜러추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
        private void _InitCmb()
        {


            var statusSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "Status").Select(c => new { c.StaticOptionId, c.Name, c.Seq,c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Status.DataSource = statusSourceDataSource;
            cmb_Status.DisplayMember = "Name";
            cmb_Status.ValueMember = "Value";



            var BankSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "BankGubun").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Bank.DataSource = BankSourceDataSource;
            cmb_Bank.DisplayMember = "Name";
            cmb_Bank.ValueMember = "Name";


            var BizNameDataSource = cmDataSet.Dealers.Where(c => c.BizNo.Length == 12).Select(c => new { c.DealerId, c.Name}).ToArray();
            cmb_BizName.DataSource = BizNameDataSource;
            cmb_BizName.DisplayMember = "Name";
            cmb_BizName.ValueMember = "DealerId";
        }

        private void txt_Code_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txt_Code.Text);
            if (!ismatch)
            {
                MessageBox.Show("숫자만 입력해 주세요.");
            }



        }
        class ErrorModel
        {
            public ErrorModel(Control iControl)
            {
                _Color = iControl.BackColor;
                _Control = iControl;
            }
            public Control _Control { get; set; }
            public Color _Color { get; set; }
        }
        private List<ErrorModel> _ErrorModelList = new List<ErrorModel>();
        private void SetError(Control _Control, String _ErrorText)
        {
            if (!_ErrorModelList.Any(c => c._Control == _Control))
            {
                _ErrorModelList.Add(new ErrorModel(_Control));
                _Control.BackColor = Color.FromArgb(0xff, 0xff, 0xf9, 0xc4);
                if (!(_Control.Tag is ToolTip))
                {
                    var _ToolTip = new ToolTip();
                    _ToolTip.ShowAlways = true;
                    _ToolTip.AutomaticDelay = 0;
                    _ToolTip.InitialDelay = 0;
                    _ToolTip.ReshowDelay = 0;
                    _ToolTip.ForeColor = Color.Red;
                    _ToolTip.BackColor = Color.White;
                    _Control.Tag = new ToolTip();
                }
                ((ToolTip)_Control.Tag).SetToolTip(_Control, _ErrorText);
                ((ToolTip)_Control.Tag).Show(_ErrorText, _Control);
            }
        }
        private void ClearError()
        {
            foreach (var _Model in _ErrorModelList)
            {
                _Model._Control.BackColor = _Model._Color;
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            _ErrorModelList.Clear();
        }
        private void RemoveError(Control _Control)
        {
            if (_ErrorModelList.Any(c => c._Control == _Control))
            {
                var _Model = _ErrorModelList.First(c => c._Control == _Control);
                _Model._Control.BackColor = _Model._Color;
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
                _ErrorModelList.Remove(_Model);
            }
        }


        private void btnZip_Click(object sender, EventArgs e)
        {

            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {

                txt_ZipCode.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_Addr.Text = ss[0] + " " + ss[1] + String.Join(" ", ss.Skip(2));

                //txt_AddrDetail.Text = String.Join(" ", ss.Skip(2));
            }

        }

        private void btn_Logo_Click(object sender, EventArgs e)
        {

            //if (clientsBindingSource.Current == null || dataGridView1.SelectedCells.Count == 0 || dataGridView1.SelectedCells[0].RowIndex == -1)
            //{

            //}
            
            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;
            txt_LogoImage.Text = dlgOpen.FileName;
            picLogo.ImageLocation = dlgOpen.FileName;


        }

        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void cmb_bizStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_bizStatus.SelectedIndex == 0)
            {
               
                txt_BizNo.Mask = "999999-9999999";

                cmb_BizName.Visible = true;
            }
            else
            {

                txt_BizNo.Mask = "999-99-99999";

                cmb_BizName.Visible = false;

            }
        }
    }
}
