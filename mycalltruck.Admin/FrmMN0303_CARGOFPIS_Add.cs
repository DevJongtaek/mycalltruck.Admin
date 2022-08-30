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
    public partial class FrmMN0303_CARGOFPIS_Add : Form
    {

        private int _CustomerId = 0;

        public FrmMN0303_CARGOFPIS_Add()
        {
            InitializeComponent();
           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "의뢰자/계약", 1), "의뢰자/계약정보 추가 성공");

                cmb_CL_COMP_GUBUN.SelectedIndex = 0;
                txt_CL_COMP_NM.Text = "";
                txt_CL_COMP_CORP_NUM.Text = "";
                txt_CL_COMP_BSNS_NUM.Text = "";
                txt_CL_P_TEL.Text = "";
                dtp_CONT_FROM.Value = DateTime.Now;
                dtp_CONT_TO.Value = DateTime.Now;

                cmb_CONT_ITEM.SelectedIndex = 0;
                cmb_CONT_GOODS_FORM.SelectedIndex = 0;

                txt_CONT_GOODS_CNT.Text = "";
                cmb_CONT_GOODS_UNIT.SelectedIndex = 0;

                txtStartAddr.Text = "";
                txtStartZip.Text = "";

                txtEndAddr.Text = "";
                txtEndZip.Text = "";

                txt_CONT_DEPOSIT.Text = "";
                cmb_CONT_MANG_TYPE.SelectedIndex = 1;

                txt_CONT_GROUP.Text = DateTime.Now.ToString("yyMMddhhmmss");

            }

            cmb_CL_COMP_GUBUN.Focus();
        }

      

        private void btnAddClose_Click(object sender, EventArgs e)
        {

            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "의뢰자/계약", 1), "의뢰자/계약정보 추가 성공");
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
            _InitCmb();

            dtp_CONT_FROM.Value = DateTime.Now;
            dtp_CONT_TO.Value = DateTime.Now;

            cmb_CONT_MANG_TYPE.SelectedIndex = 1;

            cmb_CONT_ITEM.SelectedValue = "99";
            cmb_CONT_GOODS_FORM.SelectedValue = "99";

            txt_CONT_GROUP.Text = DateTime.Now.ToString("yyMMddhhmmss");
        }



        public bool IsSuccess = false;
        public CMDataSet.FPIS_CONTRow CurrentCode = null;
        private int _UpdateDB()
        {
            err.Clear();

            if (cmb_CL_COMP_GUBUN.SelectedIndex == 0)
            {
                //if (txt_CL_COMP_NM.Text == "")
                //{
                //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                //    err.SetError(txt_CL_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                //    return -1;

                //}
                if (txt_CL_COMP_NM.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CL_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;

                }


                if (!String.IsNullOrEmpty(txt_CL_COMP_BSNS_NUM.Text))
                {

                    if (txt_CL_COMP_BSNS_NUM.Text.Replace(" ", "").Length != 12 || txt_CL_COMP_BSNS_NUM.Text.Contains(" "))
                    {
                        MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                        err.SetError(txt_CL_COMP_BSNS_NUM, "사업자 번호가 완전하지 않습니다.");

                        return -1;
                    }
                    //else if (SingleDataSet.Instance.FPIS_CONT.Where(c => c.CL_COMP_BSNS_NUM == txt_CL_COMP_BSNS_NUM.Text).Count() > 0)
                    //{

                    //    MessageBox.Show("사업자 번호가 중복되었습니다.!!", "주민 번호 입력 오류");
                    //    err.SetError(txt_CL_COMP_BSNS_NUM, "사업자 번호가 중복되었습니다.!!");
                    //    return -1;

                    //}
                    txt_CL_COMP_BSNS_NUM.Mask = "999-99-99999";

                }

             
            }

            else
            {
                if (txt_CL_COMP_NM.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CL_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;

                }

                if (!String.IsNullOrEmpty(txt_CL_COMP_BSNS_NUM.Text))
                {

                    if (txt_CL_COMP_BSNS_NUM.Text.Replace(" ", "").Length != 12 || txt_CL_COMP_BSNS_NUM.Text.Contains(" "))
                    {
                        MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                        err.SetError(txt_CL_COMP_BSNS_NUM, "사업자 번호가 완전하지 않습니다.");

                        return -1;
                    }
                   
                    txt_CL_COMP_BSNS_NUM.Mask = "999-99-99999";

                }
            }
           
           

            if (dtp_CONT_TO.Value.Date < dtp_CONT_FROM.Value.Date)
            {
                MessageBox.Show("계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                err.SetError(dtp_CONT_TO, "계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                return -1;
            }
           

            //if (txt_CONT_GOODS_CNT.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_CONT_GOODS_CNT, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;

            //}

          

            //if (txtStartAddr.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txtStartAddr, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;

            //}

            //if (txtEndAddr.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txtEndAddr, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;

            //}
            if (txt_CONT_DEPOSIT.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CONT_DEPOSIT, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
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
                if (code == "CONT_DEPOSIT") iName = "계약금액";

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
            CMDataSet.FPIS_CONTRow row = cmDataSet.FPIS_CONT.NewFPIS_CONTRow();
            CurrentCode = row;

            row.CL_COMP_GUBUN = cmb_CL_COMP_GUBUN.SelectedValue.ToString();
            row.CL_COMP_NM = txt_CL_COMP_NM.Text;

            if (txt_CL_COMP_CORP_NUM.Text.Length == 14)
            {
                row.CL_COMP_CORP_NUM = txt_CL_COMP_CORP_NUM.Text;
            }

            if (txt_CL_COMP_BSNS_NUM.Text.Length == 12)
            {
                row.CL_COMP_BSNS_NUM = txt_CL_COMP_BSNS_NUM.Text;
            }
            if (txt_CL_P_TEL.Text.Length > 11)
            {
                row.CL_P_TEL = txt_CL_P_TEL.Text;
            }
            row.CONT_FROM = dtp_CONT_FROM.Text;
            row.CONT_TO = dtp_CONT_TO.Text;
            row.CONT_ITEM = cmb_CONT_ITEM.SelectedValue.ToString();
            row.CONT_GOODS_FORM = cmb_CONT_GOODS_FORM.SelectedValue.ToString();

         
            row.CONT_GOODS_CNT = txt_CONT_GOODS_CNT.Text;
           
            row.CONT_GOODS_UNIT = cmb_CONT_GOODS_UNIT.SelectedValue.ToString();


            row.CONT_START_ADDR = txtStartZip.Text;
            row.CONT_START_ADDR1 = txtStartAddr.Text;
            row.CONT_END_ADDR = txtEndZip.Text;
            row.CONT_END_ADDR1 = txtEndAddr.Text;

            row.CONT_DEPOSIT = txt_CONT_DEPOSIT.Text;
            row.CONT_MANG_TYPE = cmb_CONT_MANG_TYPE.SelectedValue.ToString();


            row.CREATE_DATE = DateTime.Now;

            row.CliendId = LocalUser.Instance.LogInInformation.LoginId;

            row.ONE_GUBUN = "0";
            if (cbo_CONT_YN.Checked == true)
            {
                row.CONT_YN = true;

            }
            else
            {
                row.CONT_YN = false;
            }

            row.CONT_GROUP = txt_CONT_GROUP.Text;

            row.CustomerId = _CustomerId;






            cmDataSet.FPIS_CONT.AddFPIS_CONTRow(row);
            try
            {

                fpiS_CONTTableAdapter.Update(row);
            }
            catch
            {
                MessageBox.Show("의뢰자/계약정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
        private void _InitCmb()
        {
            var CargoItemDataSource = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoItem").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_ITEM.DataSource = CargoItemDataSource;
            cmb_CONT_ITEM.DisplayMember = "Name";
            cmb_CONT_ITEM.ValueMember = "value";


            var CargoFormDataSource = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoForm").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_GOODS_FORM.DataSource = CargoFormDataSource;
            cmb_CONT_GOODS_FORM.DisplayMember = "Name";
            cmb_CONT_GOODS_FORM.ValueMember = "value";


            var CargoSizeDataSource = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoSize").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_GOODS_UNIT.DataSource = CargoSizeDataSource;
            cmb_CONT_GOODS_UNIT.DisplayMember = "Name";
            cmb_CONT_GOODS_UNIT.ValueMember = "value";


            var CargoUseDataSource = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoUse").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_MANG_TYPE.DataSource = CargoUseDataSource;
            cmb_CONT_MANG_TYPE.DisplayMember = "Name";
            cmb_CONT_MANG_TYPE.ValueMember = "value";

            var CargoGubunDataSource = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoGubun").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CL_COMP_GUBUN.DataSource = CargoGubunDataSource;
            cmb_CL_COMP_GUBUN.DisplayMember = "Name";
            cmb_CL_COMP_GUBUN.ValueMember = "value";
        }

        private void btnAddr1_Click(object sender, EventArgs e)
        {
            FrmCargoZip _frmCargoZip = new FrmCargoZip();
            _frmCargoZip.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;

                txtStartAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtStartZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                
                // txt_Addr.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                //txt_ZipCode.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();


                //txt_AddrDetail.Text = "";

                _frmCargoZip.Close();
            });
            _frmCargoZip.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;
                txtStartAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtStartZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

                _frmCargoZip.Close();
            });






            _frmCargoZip.Owner = this;
            _frmCargoZip.StartPosition = FormStartPosition.CenterParent;
            _frmCargoZip.ShowDialog();
           // txt_AddrDetail.Focus();
        }

        private void btnAddr2_Click(object sender, EventArgs e)
        {
            FrmCargoZip _frmCargoZip = new FrmCargoZip();
            _frmCargoZip.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;

                
                txtEndAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtEndZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

                // txt_Addr.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                //txt_ZipCode.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();


                //txt_AddrDetail.Text = "";

                _frmCargoZip.Close();
            });
            _frmCargoZip.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;
                txtEndAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtEndZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

                _frmCargoZip.Close();
            });

            _frmCargoZip.Owner = this;
            _frmCargoZip.StartPosition = FormStartPosition.CenterParent;
            _frmCargoZip.ShowDialog();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txt_CONT_GOODS_CNT.Text);
            if (!ismatch)
            {
                MessageBox.Show("숫자만 입력해 주세요.");


            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txt_CONT_DEPOSIT.Text);
            if (!ismatch)
            {
                MessageBox.Show("숫자만 입력해 주세요.");


            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void btn_Customer_Click(object sender, EventArgs e)
        {
            FrmCustomerSearch2 _frmCustomerSearch = new FrmCustomerSearch2("1,3");
            _frmCustomerSearch.Owner = this;
            _frmCustomerSearch.StartPosition = FormStartPosition.CenterParent;
            if (_frmCustomerSearch.ShowDialog() == DialogResult.OK)
            {
                txt_CL_COMP_NM.Text = _frmCustomerSearch.CL_COMP_NM;
                txt_CL_COMP_BSNS_NUM.Text = _frmCustomerSearch.CL_COMP_BSNS;
                txt_CL_P_TEL.Text = _frmCustomerSearch.CL_P_TEL;
                txt_CL_COMP_CORP_NUM.Text = _frmCustomerSearch.CL_COMP_CORP_NUM;

                _CustomerId = _frmCustomerSearch.CustomerId;
            }
        }

        private void cmb_CL_COMP_GUBUN_SelectedIndexChanged(object sender, EventArgs e)
        {

            err.Clear();
            if (cmb_CL_COMP_GUBUN.SelectedIndex == 0)
            {
                label4.ForeColor = Color.Blue;
                label6.ForeColor = Color.Blue;
                cbo_CONT_YN.Enabled = true;
            }
            else
            {
                label4.ForeColor = Color.Blue;
                label6.ForeColor = Color.Blue;
                cbo_CONT_YN.Checked = false;
              
                cbo_CONT_YN.Enabled = false;
            }
        }

    
      

        
    }
}
