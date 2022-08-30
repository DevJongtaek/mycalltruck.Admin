using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMNFPISCONT : Form
    {
        public FrmMNFPISCONT()
        {
            InitializeComponent();
        }

        private void FrmMNFPISCONT_Load(object sender, EventArgs e)
        {
            _InitCmb();
        }

        private void _InitCmb()
        {
          

            var CargoGubunDataSource = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoGubun").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CL_COMP_GUBUN.DataSource = CargoGubunDataSource;
            cmb_CL_COMP_GUBUN.DisplayMember = "Name";
            cmb_CL_COMP_GUBUN.ValueMember = "value";
        }

        private void cmb_CL_COMP_GUBUN_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
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

            else
            {
                if (txt_CL_COMP_NM.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CL_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;

                }
            }



            //if (dtp_CONT_TO.Value.Date < dtp_CONT_FROM.Value.Date)
            //{
            //    MessageBox.Show("계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
            //    err.SetError(dtp_CONT_TO, "계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
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

        
            if (txt_CL_COMP_BSNS_NUM.Text.Length == 12)
            {
                row.CL_COMP_BSNS_NUM = txt_CL_COMP_BSNS_NUM.Text;
            }
          
            row.CONT_FROM = dtp_CONT_FROM.Text;
            row.CONT_TO = dtp_CONT_FROM.Value.AddDays(1).ToString("yyyy/MM/dd");


            //row.CONT_ITEM = cmb_CONT_ITEM.SelectedValue.ToString();
            //row.CONT_GOODS_FORM = cmb_CONT_GOODS_FORM.SelectedValue.ToString();


            //row.CONT_GOODS_CNT = txt_CONT_GOODS_CNT.Text;

            //row.CONT_GOODS_UNIT = cmb_CONT_GOODS_UNIT.SelectedValue.ToString();


            //row.CONT_START_ADDR = txtStartZip.Text;
            //row.CONT_START_ADDR1 = txtStartAddr.Text;
            //row.CONT_END_ADDR = txtEndZip.Text;
            //row.CONT_END_ADDR1 = txtEndAddr.Text;

            row.CONT_DEPOSIT = txt_CONT_DEPOSIT.Text;
            row.CONT_MANG_TYPE = "N";


            row.CREATE_DATE = DateTime.Now;

            row.CliendId = LocalUser.Instance.LogInInformation.LoginId;


            //if (cbo_CONT_YN.Checked == true)
            //{
            //    row.CONT_YN = true;

            //}
            //else
            //{
            //    row.CONT_YN = false;
            //}


            row.CONT_YN = false;

            row.CONT_GOODS_UNIT = "99";

            row.CONT_ITEM = "99";
            

            // 한건등록 신규 --2,닫기 또는 등록후닫기일때는 1
            row.ONE_GUBUN = "2";

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
    }
}
