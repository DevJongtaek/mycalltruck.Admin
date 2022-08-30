using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0404_CHAGECARMANAGE_ADD2 : Form
    {
        int tabindex = 0;
        public FrmMN0404_CHAGECARMANAGE_ADD2(int TabIndex)
        {
            InitializeComponent();

            tabindex = TabIndex;
        }

        private void FrmMN0404_CHAGECARMANAGE_ADD_Load(object sender, EventArgs e)
        {
            this.driversInfo2TableAdapter.Fill(this.cMDataSet.DriversInfo2, LocalUser.Instance.LogInInformation.ClientId);

            this.chargeAccountsTableAdapter.Fill(this.cMDataSet.ChargeAccounts);
            btn_InfoSearch_Click(null, null);
         
            _InitCmb();
        }


        private void _Fclear()
        {
            cmb_Gubun2.SelectedIndex = 0;
            dtp_ApplyDate2.Value = DateTime.Now;
            

        }
        private void _InitCmb()
        {



            //var GubunSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun3" && (c.Value != 0)).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_Gubun.DataSource = GubunSourceDataSource;
            //cmb_Gubun.DisplayMember = "Name";
            //cmb_Gubun.ValueMember = "Name";

           

            var GubunSourceDataSource3 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "AccountGubun" && (c.Value != 3 && c.Value != 0)).Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_Gubun2.DataSource = GubunSourceDataSource3;
            cmb_Gubun2.DisplayMember = "Name";
            cmb_Gubun2.ValueMember = "Name";


            var AccountSourceDataSource = cMDataSet.ChargeAccounts.Where(c => (c.ClientId == LocalUser.Instance.LogInInformation.ClientId.ToString() || c.ClientId == "NULL") && (c.Gubun == cmb_Gubun2.SelectedValue.ToString() || c.Gubun == "수입/지출")&&(c.Gubun2 == 2)).Select(c => new { c.ChargeAccountId, c.Name }).OrderBy(c => c.ChargeAccountId).ToArray();
            cmb_ChargeAccountId.DataSource = AccountSourceDataSource;
            cmb_ChargeAccountId.DisplayMember = "Name";
            cmb_ChargeAccountId.ValueMember = "ChargeAccountId";

        }

        private void btn_InfoSearch_Click(object sender, EventArgs e)
        {
            driversInfo2BindingSource.Filter = string.Format("AccountUse = 1 AND (Name Like  '%{0}%' or CarNo Like  '%{0}%' or CarYear Like  '%{0}%' )", txt_InfoSearch.Text);
        }

        private void btn_InfoSearch_Clear_Click(object sender, EventArgs e)
        {

            txt_InfoSearch.Text = "";

            btn_InfoSearch_Click(null, null);
        }

        private void txt_InfoSearch_KeyUp(object sender, KeyEventArgs e)
        {
            btn_InfoSearch_Click(null, null);
        }

        private void driversInfo1BindingSource_CurrentChanged(object sender, EventArgs e)
        {
           
        }
        private int _UpdateDB()
        {
            err.Clear();





            if (txt_Num.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Num, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }


            if (txt_UnitPrice2.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_UnitPrice2, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
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
                if (code == "Remark") iName = "적요";
                if (code == "Num") iName = "수량";
                if (code == "Price") iName = "단가";

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        public CMDataSet.ChargesRow CurrentCode = null;
        private void _AddClient()
        {
            CMDataSet.ChargesRow row = cMDataSet.Charges.NewChargesRow();
            CurrentCode = row;


            row.Gubun = cmb_Gubun2.SelectedValue.ToString();
            row.ApplyDate = dtp_ApplyDate2.Value;

            row.ChargeAccountId = int.Parse(cmb_ChargeAccountId.SelectedValue.ToString());

            var Query = cMDataSet.ChargeAccounts.Where(c => c.NeedCardInfo == true && c.ChargeAccountId == int.Parse(cmb_ChargeAccountId.SelectedValue.ToString())).ToArray();
            if (Query.Any())
            {
                row.Gubun = "매입";
                row.TaxGubun = 1;
                row.Price = Int64.Parse(txt_UnitPrice2.Text);
                row.UnitPrice = 0;

            }
            else
            {
                row.Gubun = cmb_Gubun2.SelectedValue.ToString();
                row.TaxGubun = 2;

                row.Price = 0;
                row.UnitPrice = Int64.Parse(txt_UnitPrice2.Text);
            }

            row.Remark = txt_Remark.Text;
            row.Num = int.Parse(txt_Num.Text);


            row.Tax = Int64.Parse(txt_Tax2.Text);

            if (chk_UseTax2.Checked == true)
            {
                row.UseTax = true;
            }
            else
            {
                row.UseTax = false;
            }
            row.DriverId = txt_DriverId.Text;
            row.Amount = Int64.Parse(txt_Amount2.Text);
            row.CardName = cmb_CardName.Text;



            row.SetYN = 0;
            row.IsIssued = true;


            row.IsIssued = false;

            row.ClientId = LocalUser.Instance.LogInInformation.ClientId.ToString();






            cMDataSet.Charges.AddChargesRow(row);
            try
            {
                chargesTableAdapter.Update(row);

            }
            catch
            {
                MessageBox.Show("경비출납 정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (_UpdateDB() > 0)
            {





                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "기사세무", 1), "기사세무 추가 성공");


             
                    cmb_Gubun2.SelectedIndex = 0;
                    dtp_ApplyDate2.Value = DateTime.Now;
                    cmb_ChargeAccountId.SelectedIndex = 0;
                    txt_Remark.Text = "";


                    txt_Num.Text = "";
                    txt_UnitPrice2.Text = "";
                    txt_Amount2.Text = "";
                    
                    txt_Tax2.Text = "";
                    //  chk_HasTax.Checked = false;
                    chk_UseTax2.Checked = false;

                    txt_CardNo.Text = "";
              

            }

            cmb_Gubun2.Focus();
        }

        public bool IsSuccess = false;
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {


                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "기사세무", 1), "기사세무 정보 추가 성공");
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

        private void cmb_CardName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_CardName.Items.Count > 0)
            {
                txt_CardNo.Text = cmb_CardName.SelectedValue.ToString();

            }
        }

        private void cmb_Gubun_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmb_Gubun2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Gubun2.SelectedValue.ToString() == null)
            {
                return;
            }
            var AccountSourceDataSource1 = cMDataSet.ChargeAccounts.Where(c => (c.ClientId == LocalUser.Instance.LogInInformation.ClientId.ToString() || c.ClientId == "NULL") && (c.Gubun == cmb_Gubun2.SelectedValue.ToString() || c.Gubun == "수입/지출") && (c.Gubun2 == 2)).Select(c => new { c.ChargeAccountId, c.Name }).OrderBy(c => c.ChargeAccountId).ToArray();
            cmb_ChargeAccountId.DataSource = AccountSourceDataSource1;
            cmb_ChargeAccountId.DisplayMember = "Name";
            cmb_ChargeAccountId.ValueMember = "ChargeAccountId";
        }

        private void txt_UnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

     

        private void driversInfo2BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (driversInfo2BindingSource.Current == null) return;



            var Selected = ((DataRowView)driversInfo2BindingSource.Current).Row as CMDataSet.DriversInfo2Row;


            if (Selected != null)
            {


                //  label83.Text = Selected.Name;
                txt_DriverId.Text = Selected.DriverId.ToString();
                txt_ChargeGubun.Text = Selected.Gubun;
                //   txt_CarNo.Text = Selected.CarNo;
                //   txt_DriverName.Text = Selected.CarYear;

                driversCardTableAdapter.Fill(cMDataSet.DriversCard, Selected.DriverId);
                // cmb_CardName.DataSource = driversInfo1BindingSource;

                if (cmb_CardName.Items.Count > 0)
                {
                    cmb_CardName.SelectedIndex = 0;
                    txt_CardNo.Text = cmb_CardName.SelectedValue.ToString();
                }
                else
                {
                    txt_CardNo.Text = "";
                }
                ////cmb_Gubun2.DataSource = driversCard;
                // cmb_CardName.DisplayMember = "CardName";
                // cmb_CardName.ValueMember = "CardNo";
            }
        }

        private void txt_Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Num_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {

                if (chk_UseTax2.Checked == true)
                {
                    txt_Tax2.Text = Math.Truncate((Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)) * 0.1).ToString();
                    txt_Amount2.Text = (Int64.Parse(txt_Tax2.Text) + (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text))).ToString();

                }
                else
                {
                    txt_Tax2.Text = "0";
                    txt_Amount2.Text = (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)).ToString();
                }


            }
        }

        private void txt_UnitPrice2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_UnitPrice2_Leave(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {

                if (chk_UseTax2.Checked == true)
                {
                    txt_Tax2.Text = Math.Truncate((Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)) * 0.1).ToString();
                    txt_Amount2.Text = (Int64.Parse(txt_Tax2.Text) + (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text))).ToString();

                }
                else
                {
                    txt_Tax2.Text = "0";
                    txt_Amount2.Text = (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)).ToString();
                }


            }
        }

        private void chk_UseTax2_CheckedChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num.Text))
            {

                if (chk_UseTax2.Checked == true)
                {
                    txt_Tax2.Text = Math.Truncate((Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)) * 0.1).ToString();
                    txt_Amount2.Text = (Int64.Parse(txt_Tax2.Text) + (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text))).ToString();

                }
                else
                {
                    txt_Tax2.Text = "0";
                    txt_Amount2.Text = (Int64.Parse(txt_UnitPrice2.Text) * Int64.Parse(txt_Num.Text)).ToString();
                }


            }
        }
    }
}
