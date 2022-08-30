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
    public partial class FrmMN0303_CARGOFPIS_Add2_Default : Form
    {
        string SFpisId = string.Empty;
        string SGoodsUnit = string.Empty;
        public FrmMN0303_CARGOFPIS_Add2_Default(string FpisId,string GoodsUnit)
        {
          
            InitializeComponent();

            SFpisId = FpisId;
            SGoodsUnit = GoodsUnit;
            
           

        }

        private void F_GoodsUnit()
        {
            var codes = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoSize" && c.Value == SGoodsUnit).Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).ToArray();

           // lbl_TRU_GOODS_UNIT.Text = "(" + codes.First().Name + ")";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "위탁", 1), "위탁정보 추가 성공");


              

                cmb_TRU_MANG_TYPE.SelectedIndex = 1;

                txt_TRU_COMP_BSNS_NUM.Text = "";

                txt_TRU_COMP_NM.Text = "";



            //    txt_TRU_COMP_CORP_NUM.Text = "";



                dtp_TRU_CONT_FROM.Value = DateTime.Now;
                dtp_TRU_CONT_TO.Value = DateTime.Now;

              //  cmb_TRU_TRANS_TYPE.SelectedIndex = 0;
                txt_TRU_DEPOSIT.Text = "";
               // cmb_TRU_CONT_ITEM.SelectedText= "기타";
               // cmb_TRU_GOODS_FORM.SelectedText = "기타";
               // txt_TRU_GOODS_CNT.Text = "";

                //lbl_TRU_GOODS_UNIT.Text = "";

                txtStartZip.Text = "";
               // txtStartAddr.Text = "";

                txtEndZip.Text = "";
               // txtStartAddr.Text = "";





            }

            txt_TRU_COMP_NM.Focus();
          //  cmb_TRU_TRANS_TYPE.Focus();
        }

      

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "위탁", 1), "위탁정보 추가 성공");
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
        string I_CONT_GOODS_CNT, I_CONT_DEPOSIT, I_TRU_GOODS_CNT, I_TRU_DEPOSIT, I_CAR_GOODS_CNT, I_CAR_CHARGE, I_ORDER_GOODS_CNT, I_ORDER_CHARGE;
        private void FrmMN0207_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            this.fpiS_CONTTableAdapter.Fill(this.cmDataSet.FPIS_CONT);
            _InitCmb();
            F_GoodsUnit();
            dtp_TRU_CONT_FROM.Value = DateTime.Now;
            dtp_TRU_CONT_TO.Value = DateTime.Now;
            cmb_TRU_MANG_TYPE.SelectedIndex = 1;

          

        //    cmb_TRU_CONT_ITEM.SelectedValue = "99" ;
        //    cmb_TRU_GOODS_FORM.SelectedValue = "99";

             var Query  = cmDataSet.FPIS_CONT.Where(c=> c.FPIS_ID == int.Parse(SFpisId));
             lbl_name.Text = Query.First().CL_COMP_NM;
             lbl_Date.Text = Query.First().CONT_TO;
             lbl_SignMoney.Text = String.Format("{0:#,##0}", Convert.ToInt64(Query.First().CONT_DEPOSIT));

             using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
             {

                 this.clientUsersTableAdapter.FillForAdmin(this.cmDataSet.ClientUsers);

                 #region 배차

                 SqlCommand selectCmd1 = new SqlCommand(

                @"   select sum(CONVERT(bigint,ISNULL(Price,0))) as CAR_CHARGE  from orders left join drivers  on orders.driverid = drivers.DriverId
                            WHERE orders.FPIS_ID in( @fpisid ) AND OrderStatus = 3  AND convert(varchar(10), StopTime,120) >=  @Sdate AND convert(varchar(10), StopTime,120) <= @Edate
                             and ((drivers.CarGubun = 1) or (drivers.cargubun = 2 and drivers.Car_ContRact = 1) or (drivers.cargubun = 3 and drivers.Car_ContRact = 1))"

                , cn);

                 selectCmd1.Parameters.Add(new SqlParameter("@fpisid", SFpisId));

                 selectCmd1.Parameters.Add(new SqlParameter("@Sdate", DateTime.Now.Year.ToString() + "-01-01"));
                 selectCmd1.Parameters.Add(new SqlParameter("@Edate", DateTime.Now.ToString("yyyy-MM-dd")));

                 cn.Open();

                 SqlDataReader reader1 = selectCmd1.ExecuteReader();
                 I_ORDER_CHARGE = string.Empty;

                 try
                 {
                     while (reader1.Read())
                     {

                         I_ORDER_CHARGE = reader1["CAR_CHARGE"].ToString();

                     }
                     if (String.IsNullOrEmpty(I_ORDER_CHARGE))
                     {
                         I_ORDER_CHARGE = "0";

                     }

                 }
                 catch { }

                 cn.Close();



                 I_CAR_CHARGE = "0";

                 string CarPer = string.Format("{0:#,###0}", Int64.Parse(I_CAR_CHARGE) + Int64.Parse(I_ORDER_CHARGE));
                 #endregion

                 #region 위탁
                
                 SqlCommand selectCmd3 = new SqlCommand(
                        @" select sum(CONVERT(bigint,ISNULL(TRU_DEPOSIT,0))) AS TRU_DEPOSIT from FPIS_TRU
                            JOIN FPIS_CONT on FPIS_TRU.FPIS_ID = FPIS_CONT.FPIS_ID where FPIS_CONT.FPIS_ID in( @fpisid )AND  REPLACE(FPIS_TRU.TRU_CONT_FROM,'/','-') >=  @Sdate AND REPLACE(FPIS_TRU.TRU_CONT_FROM,'/','-') <= @Edate", cn);
                 // selectCmd3.Parameters.Add(new SqlParameter("@ClientId", LocalUser.Instance.LogInInfomation.UserID));


                 selectCmd3.Parameters.Add(new SqlParameter("@fpisid", SFpisId));
                 selectCmd3.Parameters.Add(new SqlParameter("@Sdate", DateTime.Now.Year.ToString() + "-01-01"));
                 selectCmd3.Parameters.Add(new SqlParameter("@Edate", DateTime.Now.ToString("yyyy/MM/dd")));


                 cn.Open();

                 SqlDataReader reader3 = selectCmd3.ExecuteReader();

                 I_TRU_DEPOSIT = string.Empty;

                 try
                 {
                     while (reader3.Read())
                     {
                         I_TRU_DEPOSIT = reader3["TRU_DEPOSIT"].ToString();
                     }

                     if (String.IsNullOrEmpty(I_TRU_DEPOSIT))
                     {
                         I_TRU_DEPOSIT = "0";

                     }
                 }
                 catch { }

               //  string TruPer = string.Format("{0:#,###0}", Int64.Parse(I_TRU_DEPOSIT));



                 cn.Close();
                 #endregion


                 ////string S_CarPer = string.Format("{0:#,###0.##}", ((double.Parse(I_CAR_CHARGE) + double.Parse(I_ORDER_CHARGE)) / (double.Parse(I_CAR_CHARGE) + double.Parse(I_ORDER_CHARGE) + double.Parse(I_TRU_DEPOSIT))) * 100);

                 //string S_TruPer = string.Format("{0:#,###0.##}", ((double.Parse(I_TRU_DEPOSIT) / (double.Parse(I_CAR_CHARGE) + double.Parse(I_ORDER_CHARGE))) * 100));


                 //string S_TruPer2 = string.Format("{0:#,###0}", ((double.Parse(I_TRU_DEPOSIT) / (double.Parse(I_CAR_CHARGE) + double.Parse(I_ORDER_CHARGE))) * 100));


                 //string S_CarPer = string.Format("{0:#,###0.##}", double.Parse("100") - double.Parse(S_TruPer));

              //   string S_TruWon = string.Format("{0:#,###0}", (double.Parse(I_TRU_DEPOSIT)));
                 string S_CarWon = string.Format("{0:#,###0}", (double.Parse(lbl_SignMoney.Text) - double.Parse(I_TRU_DEPOSIT) + double.Parse(I_ORDER_CHARGE)));


                 lbl_ReMoney.Text = string.Format("{0:#,###0}", (double.Parse(lbl_SignMoney.Text) - double.Parse(I_TRU_DEPOSIT) + double.Parse(I_ORDER_CHARGE)));
             }

        }


        public bool IsSuccess = false;
        public CMDataSet.FPIS_TRURow CurrentCode = null;
        private int _UpdateDB()
        {
            err.Clear();

          

            if (txt_TRU_COMP_BSNS_NUM.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_TRU_COMP_BSNS_NUM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }
            else if (txt_TRU_COMP_BSNS_NUM.Text.Replace(" ","").Length != 12 || txt_TRU_COMP_BSNS_NUM.Text.Contains(" "))
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_TRU_COMP_BSNS_NUM, "사업자 번호가 완전하지 않습니다.");
                return -1;
            }

            if (txt_TRU_COMP_NM.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_TRU_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }

            //if (txt_TRU_COMP_CORP_NUM.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_TRU_COMP_CORP_NUM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;

            //}
            //else if (txt_TRU_COMP_CORP_NUM.TextLength != 14)
            //{
            //    MessageBox.Show("법인등록번호가 완전하지 않습니다.", "법인등록번호 불완전");
            //    err.SetError(txt_TRU_COMP_CORP_NUM, "법인등록번호가 완전하지 않습니다.");
            //    return -1;
            //}




            //if (dtp_TRU_CONT_FROM.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(dtp_TRU_CONT_FROM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;

            //}


            if (dtp_TRU_CONT_TO.Value.Date < dtp_TRU_CONT_FROM.Value.Date)
            {
                MessageBox.Show("계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                err.SetError(dtp_TRU_CONT_TO, "계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                return -1;
            }
            
            var Query = cmDataSet.FPIS_CONT.Where(c => c.FPIS_ID ==int.Parse(SFpisId));
            DateTime dContFrom = DateTime.Parse(Query.First().CONT_FROM);

            if (dtp_TRU_CONT_FROM.Value.Date < dContFrom)
            {
                MessageBox.Show("계약시작일은 위탁받은날 이후로 설정하셔야 합니다.");
                err.SetError(dtp_TRU_CONT_TO, "계약시작일은 위탁받은날 이후로 설정하셔야 합니다.");
                return -1;
            }

            if (txt_TRU_DEPOSIT.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_TRU_DEPOSIT, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }

            //if (txt_TRU_GOODS_CNT.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_TRU_GOODS_CNT, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
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


            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "TRU_DEPOSIT") iName = "계약금액";
                if (code == "TRU_COMP_BSNS_NUM") iName = "사업자등록번호";
                if (code == "ID_Code") iName = "위탁계약금";

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

            CMDataSet.FPIS_TRURow row = cmDataSet.FPIS_TRU.NewFPIS_TRURow();
            CurrentCode = row;

            row.TRU_TRANS_TYPE = "1";
         

            if (txt_TRU_COMP_BSNS_NUM.Text.Length == 12)
            {
                row.TRU_COMP_BSNS_NUM = txt_TRU_COMP_BSNS_NUM.Text;
            }
            row.TRU_COMP_NM = txt_TRU_COMP_NM.Text;



            //if (txt_TRU_COMP_CORP_NUM.Text.Length == 14)
            //{
            //    row.TRU_COMP_CORP_NUM = txt_TRU_COMP_CORP_NUM.Text;
            //}


            row.TRU_CONT_FROM = dtp_TRU_CONT_FROM.Text;
            row.TRU_CONT_TO = dtp_TRU_CONT_TO.Text;

            row.TRU_MANG_TYPE = cmb_TRU_MANG_TYPE.SelectedValue.ToString();
            row.TRU_DEPOSIT = txt_TRU_DEPOSIT.Text;
            row.TRU_CONT_ITEM = "99";
            row.TRU_GOODS_FORM = "99";
            row.TRU_GOODS_CNT = "";

            row.TRU_GOODS_UNIT = SGoodsUnit;

            row.TRU_START_ADDR = "";
            row.TRU_START_ADDR1 = "";

            row.TRU_END_ADDR = "";
            row.TRU_END_ADDR1 = "";

          
            row.CREATE_DATE =DateTime.Parse(DateTime.Now.ToString("d"));

            row.FPIS_ID = int.Parse(SFpisId);












            cmDataSet.FPIS_TRU.AddFPIS_TRURow(row);
            try
            {
                fpiS_TRUTableAdapter.Update(row);
            }
            catch
            {
                MessageBox.Show("위탁정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }
   
        
        private void _InitCmb()
        {
           

            //var CargoItemDataSource2 = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoItem").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_TRU_CONT_ITEM.DataSource = CargoItemDataSource2;
            //cmb_TRU_CONT_ITEM.DisplayMember = "Name";
            //cmb_TRU_CONT_ITEM.ValueMember = "value";


         
            //var CargoFormDataSource2 = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoForm").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_TRU_GOODS_FORM.DataSource = CargoFormDataSource2;
            //cmb_TRU_GOODS_FORM.DisplayMember = "Name";
            //cmb_TRU_GOODS_FORM.ValueMember = "value";



            var CargoUseDataSource2 = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoUse").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_TRU_MANG_TYPE.DataSource = CargoUseDataSource2;
            cmb_TRU_MANG_TYPE.DisplayMember = "Name";
            cmb_TRU_MANG_TYPE.ValueMember = "value";

            //var CargoCarryDataSource = SingleDataSet.Instance.FPISOptions.Where(c => c.Div == "CargoCarry").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_TRU_TRANS_TYPE.DataSource = CargoCarryDataSource;
            //cmb_TRU_TRANS_TYPE.DisplayMember = "Name";
            //cmb_TRU_TRANS_TYPE.ValueMember = "value";

      

        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Regex emailregex = new Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(txt_TRU_DEPOSIT.Text);
            if (!ismatch)
            {
                MessageBox.Show("숫자만 입력해 주세요.");


            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

      

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

     

     

    
      

        
    }
}
