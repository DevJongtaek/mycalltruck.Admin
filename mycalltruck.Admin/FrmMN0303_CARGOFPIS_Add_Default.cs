using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;

namespace mycalltruck.Admin
{
    public partial class FrmMN0303_CARGOFPIS_Add_Default : Form
    {
        private int _CustomerId = 0;

        public FrmMN0303_CARGOFPIS_Add_Default()
        {
            InitializeComponent();
        }

        private void FrmMN0303_CARGOFPIS_Add_Default_Load(object sender, EventArgs e)
        {
            dtp_CONT_FROM.Value = DateTime.Now;
            dtp_CONT_TO.Text = DateTime.Now.ToString("yyyy-12-31");
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            _UpdateDB(() => {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "의뢰자/계약", 1), "의뢰자/계약정보 추가 성공");
                txt_CL_COMP_NM.Text = "";
                txt_CL_COMP_BSNS_NUM.Text = "";
                txt_CL_P_TEL.Text = "";
                txt_CONT_DEPOSIT.Text = "";
                dtp_CONT_FROM.Value = DateTime.Now;
                dtp_CONT_TO.Text = DateTime.Now.ToString("yyyy-12-31");
                txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            });
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            _UpdateDB(() => {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "의뢰자/계약", 1), "의뢰자/계약정보 추가 성공");
                IsSuccess = true;
                Close();
            });
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        public bool IsSuccess = false;
        private void btn_Customer_Click(object sender, EventArgs e)
        {
            ClearError();
            FrmCustomerSearch2 _frmCustomerSearch = new FrmCustomerSearch2("1,3");
            _frmCustomerSearch.Owner = this;
            _frmCustomerSearch.StartPosition = FormStartPosition.CenterParent;
            if (_frmCustomerSearch.ShowDialog() == DialogResult.OK)
            {
                txt_CL_COMP_NM.Text = _frmCustomerSearch.CL_COMP_NM;
                txt_CL_COMP_BSNS_NUM.Text = _frmCustomerSearch.CL_COMP_BSNS;
                txt_CL_P_TEL.Text = _frmCustomerSearch.CL_P_TEL;
               // txt_CL_COMP_CORP_NUM.Text = _frmCustomerSearch.CL_COMP_CORP_NUM;

                _CustomerId = _frmCustomerSearch.CustomerId;
            }
        }
        private void btnExcelImport_Click(object sender, EventArgs e)
        {
            ClearError();
            EXCELINSERT_CARGOFPIS _Form = new EXCELINSERT_CARGOFPIS();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void btn_DriverExcel_Click(object sender, EventArgs e)
        {
            byte[] ieExcel;
            string fileString = string.Empty;
            fileString = "위수탁관리입력양식_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            ieExcel = Properties.Resources.위수탁관리입력양식;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.FPISTRU);
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
        #region UPDATE
        private void _UpdateDB(Action Done)
        {
            bool HasError = false;
            List<String> ErrorArray = new List<string>();
            ClearError();
            if (String.IsNullOrWhiteSpace(txt_CL_COMP_NM.Text))
            {
                var Error = "상호/성명을 검색해주세요.";
                ErrorArray.Add(Error);
                SetError(btn_Customer, Error);
                HasError = true;
            }
            fpiS_CONTTableAdapter.Fill(cmDataSet.FPIS_CONT);
            var QueryContTO = cmDataSet.FPIS_CONT.Where(c => c.CustomerId == _CustomerId);
            if (QueryContTO.Any())
            {
                if (QueryContTO.First().CONT_FROM.Substring(1, 7) == dtp_CONT_FROM.Text.Substring(1, 7))
                {

                    var Error = "해당기간에 계약정보가 존재합니다.";
                    ErrorArray.Add(Error);
                    SetError(dtp_CONT_TO, Error);
                    HasError = true;
                }

            }

            if (dtp_CONT_TO.Value < dtp_CONT_FROM.Value)
            {
                var Error = "계약기간은 시작일이 종료일 보다 나중일 수 없습니다.";
                ErrorArray.Add(Error);
                SetError(dtp_CONT_TO, Error);
                HasError = true;
            }
            decimal _d = 0;
            if (!decimal.TryParse(txt_CONT_DEPOSIT.Text, out _d))
            {
                var Error = "계약금을 을 입력해 주세요.";
                ErrorArray.Add(Error);
                SetError(txt_CONT_DEPOSIT, Error);
                HasError = true;
            }
            if (HasError)
            {
                return;
            }
            CMDataSet.FPIS_CONTRow row = cmDataSet.FPIS_CONT.NewFPIS_CONTRow();
            row.CL_COMP_GUBUN = "1"; //운수사업자
            row.CL_COMP_NM = txt_CL_COMP_NM.Text;
            row.CL_COMP_BSNS_NUM = txt_CL_COMP_BSNS_NUM.Text;
            row.CL_COMP_CORP_NUM = 
            row.CL_P_TEL = txt_CL_P_TEL.Text;
            row.CONT_FROM = dtp_CONT_FROM.Text;
            row.CONT_TO = dtp_CONT_TO.Text;
            row.CONT_ITEM = "99";
            row.CONT_GOODS_FORM = "99";
            row.CONT_GOODS_CNT = "0";
            row.CONT_GOODS_UNIT = "99";
            row.CONT_START_ADDR = "";
            row.CONT_START_ADDR1 = "";
            row.CONT_END_ADDR = "";
            row.CONT_END_ADDR1 = "";
            row.CONT_DEPOSIT = txt_CONT_DEPOSIT.Text.Replace(",", "");
            row.CONT_MANG_TYPE = "N";
            row.CREATE_DATE = DateTime.Now;
            row.CliendId = LocalUser.Instance.LogInInformation.LoginId;
            row.ONE_GUBUN = "0";
            row.CONT_YN = false;
            row.CONT_GROUP = DateTime.Now.ToString("yyMMddhhmmss");
            row.CustomerId = _CustomerId;
            cmDataSet.FPIS_CONT.AddFPIS_CONTRow(row);
            try
            {
                fpiS_CONTTableAdapter.Update(row);
            }
            catch
            {
                Invoke(new Action(() => MessageBox.Show("데이터베이스 작업 중 문제가 발생하였습니다. 잠시 후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                return;
            }
            Done();
        }
        #endregion


        #region HELP
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
        bool ValidateStart = false;
        #endregion

        private void Control_Enter(object sender, EventArgs e)
        {
            RemoveError(sender as Control);
            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
        }

        private void txt_CONT_DEPOSIT_Enter(object sender, EventArgs e)
        {
            Control_Enter(sender, e);
            txt_CONT_DEPOSIT.Text = txt_CONT_DEPOSIT.Text.Replace(",", "");
        }

        private void txt_CONT_DEPOSIT_Leave(object sender, EventArgs e)
        {
            decimal _d = 0;
            txt_CONT_DEPOSIT.Text = txt_CONT_DEPOSIT.Text.Replace(",", "");
            if (!decimal.TryParse(txt_CONT_DEPOSIT.Text, out _d))
            {
                var Error = "계약금을 을 입력해 주세요.";
                SetError(txt_CONT_DEPOSIT, Error);
            }
            else
            {
                txt_CONT_DEPOSIT.Text = _d.ToString("N0");
            }
        }

        private void dtp_CONT_FROM_Leave(object sender, EventArgs e)
        {
            if (dtp_CONT_TO.Value < dtp_CONT_FROM.Value)
            {
                var Error = "계약기간은 시작일이 종료일 보다 나중일 수 없습니다.";
                SetError(dtp_CONT_TO, Error);
            }
        }

        private void txt_CONT_DEPOSIT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
    }
}
