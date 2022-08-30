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
    public partial class FrmMN0210_AccountOptions_Add : Form
    {
        public FrmMN0210_AccountOptions_Add()
        {
            InitializeComponent();
        }

        private void FrmMN0210_AccountOptions_Add_Load(object sender, EventArgs e)
        {
            _InitCmb();
         
            cmb_Gubun.Focus();
        }

        private void _InitCmb()
        {


            var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun.DataSource = GubunSourceDataSource;
            cmb_Gubun.DisplayMember = "Name";
            cmb_Gubun.ValueMember = "Name";

            var GubunSourceDataSource2 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun2" && c.Seq != 0).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun2.DataSource = GubunSourceDataSource2;
            cmb_Gubun2.DisplayMember = "Name";
            cmb_Gubun2.ValueMember = "Value";


        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {





                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "계정과목", 1), "계정과목 추가 성공");


                
                cmb_Gubun.SelectedIndex = 0;
                txt_Name.Text = "";
                txt_Remark.Text = "";



            }

            cmb_Gubun.Focus();
        }
        private int _UpdateDB()
        {
            err.Clear();

           
            
            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }


            //if (txt_Remark.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_Remark, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;
            //}
           






            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
               
                if (code == "Gubun") iName = "구분";
                if (code == "Name") iName = "비고";
               
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        public CMDataSet.ChargeAccountsRow CurrentCode = null;
        private void _AddClient()
        {
            CMDataSet.ChargeAccountsRow row = cmDataSet.ChargeAccounts.NewChargeAccountsRow();
            CurrentCode = row;

           
            row.Gubun = cmb_Gubun.SelectedValue.ToString();
            row.Name = txt_Name.Text;
            row.Remark = txt_Remark.Text;


            row.ClientId = LocalUser.Instance.LogInInformation.ClientId.ToString();

            row.Gubun2 = int.Parse(cmb_Gubun2.SelectedValue.ToString());
            row.NeedCardInfo = false;
            cmDataSet.ChargeAccounts.AddChargeAccountsRow(row);
            try
            {
                chargeAccountsTableAdapter.Update(row);
             
            }
            catch
            {
                MessageBox.Show("계정과목 정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
        public bool IsSuccess = false;


        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {


                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "계정과목", 1), "계정과목 정보 추가 성공");
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
    }
}
