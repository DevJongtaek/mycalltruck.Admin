using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.UI;
using Popbill.Taxinvoice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0212_SALESMANAGE_ADD : Form
    {
        decimal _VAT = 0, _VAT1 = 0, _VAT2 = 0, _VAT3 = 0, _VAT4 = 0;

        decimal _PRICE = 0, _PRICE1 = 0, _PRICE2 = 0, _PRICE3 = 0, _PRICE4 = 0;
        decimal _UNITPRICE = 0, _UNITPRICE1, _UNITPRICE2, _UNITPRICE3, _UNITPRICE4;
        decimal _NUM = 0, _NUM1 = 0, _NUM2 = 0, _NUM3 = 0, _NUM4 = 0;
        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;

        #region 전자세금계산서NiceDNR
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
     //   string linkId = "edubillsys";
        #endregion

        public FrmMN0212_SALESMANAGE_ADD()
        {
            InitializeComponent();
        }

        private void FrmMN0212_SALESMANAGE_ADD_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'salesDataSet.SalesManage' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.salesManageTableAdapter1.Fill(this.salesDataSet.SalesManage);
            clientsTableAdapter.Fill(this.cmDataSet.Clients);

            adminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;


            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            _InitCmb();

            txt_SangHo.Focus();


        }

        private void _InitCmb()
        {






            //var CarstateDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.State }).Distinct().ToArray();
            //cmb_AddressState.DataSource = CarstateDataSource;
            //cmb_AddressState.DisplayMember = "State";
            //cmb_AddressState.ValueMember = "State";


            //var CarCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            //cmb_AddressCity.DataSource = CarCityDataSource;
            //cmb_AddressCity.DisplayMember = "City";
            //cmb_AddressCity.ValueMember = "City";


            Dictionary<string, string> HasACCList = new Dictionary<string, string>();

            HasACCList.Add("0", "현금");
            HasACCList.Add("1", "카드");
            cmb_HasAcc.DataSource = new BindingSource(HasACCList, null);
            cmb_HasAcc.DisplayMember = "Value";
            cmb_HasAcc.ValueMember = "Key";


        


        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                dtp_RequestDate.Value = DateTime.Now;
                txt_SangHo.Text = "";
                txt_BizNo.Text = "";
                txt_CEO.Text = "";
                txt_Uptae.Text = "";
                txt_Upjong.Text = "";
                //cmb_AddressState.SelectedIndex = 0;
                //cmb_AddressCity.SelectedIndex = 0;

                txt_Zip.Text = "";
                txt_State.Text = "";
                txt_City.Text = "";

                txt_AddrDetail.Text = "";
                txt_Email.Text = "";
                txt_ContRactName.Text = "";
                txt_MobileNo.Text = "";


                txt_Item1.Text = "";
                txt_UnitPrice1.Text = "";
                txt_Num1.Text = "";
                txt_VAT1.Text = "";
                txt_Price1.Text = "";

                txt_Item2.Text = "";
                txt_UnitPrice2.Text = "";
                txt_Num2.Text = "";
                txt_VAT2.Text = "";
                txt_Price2.Text = "";


                txt_Item3.Text = "";
                txt_UnitPrice3.Text = "";
                txt_Num3.Text = "";
                txt_VAT3.Text = "";
                txt_Price3.Text = "";

                txt_Item4.Text = "";
                txt_UnitPrice4.Text = "";
                txt_Num4.Text = "";
                txt_VAT4.Text = "";
                txt_Price4.Text = "";


                rdb_Tax0.Checked = true;
             

                txt_Amount.Text = "";
                txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
                lbl_HasAcc.Visible = false;
                cmb_HasAcc.Visible = false;

                txt_CardPayGubun.Text = "";
                panel4.Visible = false;

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "배차 외 추가", 1), "배차 외 추가 성공");



                

               



            }

            txt_SangHo.Focus();
        }


        private int _UpdateDB()
        {
            err.Clear();

            if (txt_SangHo.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_SangHo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            


            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");

                return -1;
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

            if (txt_AddrDetail.Text == "" || txt_State.Text == "" || txt_City.Text == "" || txt_Zip.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_AddrDetail, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_Email.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Email, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            //if (txt_ContRactName.Text == "")
            //{
            //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
            //    err.SetError(txt_ContRactName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            //    return -1;
            //}

           
            if (txt_MobileNo.Text.Replace(" ", "").Replace("-", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }


          


            if (txt_Item1.Text.Replace(" ", "") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Item1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }

            
                if (txt_UnitPrice1.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_UnitPrice1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }


                if (txt_Num1.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Num1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }
            

            if (!String.IsNullOrEmpty(txt_Item2.Text))
            {
                if (txt_UnitPrice2.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_UnitPrice1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }


                if (txt_Num2.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Num1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }
            }

            if (!String.IsNullOrEmpty(txt_Item3.Text))
            {
                if (txt_UnitPrice3.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_UnitPrice1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }


                if (txt_Num3.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Num1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }
            }

            if (!String.IsNullOrEmpty(txt_Item4.Text))
            {
                if (txt_UnitPrice4.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_UnitPrice1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }


                if (txt_Num4.Text.Replace(" ", "").Replace(",", "") == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Num1, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return -1;
                }
            }





            try
            {
                if (!String.IsNullOrEmpty(txt_VAT1.Text))
                {
                    _VAT1 = decimal.Parse(txt_VAT1.Text);
                }
                if (!String.IsNullOrEmpty(txt_VAT2.Text))
                {
                    _VAT2 = decimal.Parse(txt_VAT2.Text);
                }
                if (!String.IsNullOrEmpty(txt_VAT3.Text))
                {
                    _VAT3 = decimal.Parse(txt_VAT3.Text);
                }
                if (!String.IsNullOrEmpty(txt_VAT4.Text))
                {
                    _VAT4 = decimal.Parse(txt_VAT4.Text);

                }
                _VAT = _VAT1 + _VAT2 + _VAT3 + _VAT4;

                if (!String.IsNullOrEmpty(txt_Price1.Text))
                {
                    _PRICE1 = decimal.Parse(txt_Price1.Text);
                }
                if (!String.IsNullOrEmpty(txt_Price2.Text))
                {
                    _PRICE2 = decimal.Parse(txt_Price2.Text);
                }
                if (!String.IsNullOrEmpty(txt_Price3.Text))
                {
                    _PRICE3 = decimal.Parse(txt_Price3.Text);
                }
                if (!String.IsNullOrEmpty(txt_Price4.Text))
                {
                    _PRICE4 = decimal.Parse(txt_Price4.Text);

                }
                _PRICE = _PRICE1 + _PRICE2 + _PRICE3 + _PRICE4;


                if (!String.IsNullOrEmpty(txt_UnitPrice1.Text))
                {
                    _UNITPRICE1 = decimal.Parse(txt_UnitPrice1.Text);
                }
                if (!String.IsNullOrEmpty(txt_UnitPrice2.Text))
                {
                    _UNITPRICE2 = decimal.Parse(txt_UnitPrice2.Text);
                }
                if (!String.IsNullOrEmpty(txt_UnitPrice3.Text))
                {
                    _UNITPRICE3 = decimal.Parse(txt_UnitPrice3.Text);
                }
                if (!String.IsNullOrEmpty(txt_UnitPrice4.Text))
                {
                    _UNITPRICE4 = decimal.Parse(txt_UnitPrice4.Text);

                }
                _UNITPRICE = _UNITPRICE1 + _UNITPRICE2 + _UNITPRICE3 + _UNITPRICE4;

                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Code") iName = "코드";
                if (code == "Name") iName = "상호";
                if (code == "BizNo") iName = "사업자 번호";
                if (code == "CEO") iName = "대표자명";
                if (code == "Addr") iName = "주소";
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
       public CMDataSet.SalesManageRow CurrentCode = null;
        int _salesId = 0;
        private void _AddClient()
        {
            CMDataSet.SalesManageRow row = cmDataSet.SalesManage.NewSalesManageRow();
            CurrentCode = row;
            row.RequestDate = dtp_RequestDate.Value;
            row.SangHo = txt_SangHo.Text;
            row.BizNo = txt_BizNo.Text;
            row.Ceo = txt_CEO.Text;
            row.Uptae = txt_Uptae.Text;
            row.Upjong = txt_Upjong.Text;
            row.AddressState = txt_State.Text;
            row.AddressCity = txt_City.Text;
            row.ZipCode = txt_Zip.Text;
            row.AddressDetail = txt_AddrDetail.Text;
            row.Email = txt_Email.Text;
            row.ContRactName = txt_ContRactName.Text;
            row.MobileNo = txt_MobileNo.Text;

            row.Item = txt_Item1.Text;

            row.UnitPrice = _UNITPRICE;
            if (_NUM == 0)
            {
                row.Num = 1;
            }
            else
            {
                row.Num = Int64.Parse(_NUM.ToString());
            }
            row.Vat = Int64.Parse(_VAT.ToString());
            if (rdb_Tax2.Checked)
            {

                row.UseTax = false;
            }
            else
            {
                row.UseTax = true;
            }
            row.Price = Int64.Parse(_PRICE.ToString());
            row.Amount = Int64.Parse(txt_Amount.Text.Replace(",", ""));
            row.CreateDate = DateTime.Parse(txt_CreateDate.Text);
            if (LocalUser.Instance.LogInInformation.IsSubClient)
            {
                row.SubClientId = LocalUser.Instance.LogInInformation.SubClientId;
            }
            else
            {
                row.SetSubClientIdNull();
            }
            if (cmb_HasAcc.SelectedIndex == 0)
            {
                row.HasACC = false;
            }
            else
            {
                row.HasACC = true;
            }
            if (String.IsNullOrEmpty(txt_CardPayGubun.Text))
            {
                row.CardPayGubun = "N";
            }
            else
            {
                row.CardPayGubun = txt_CardPayGubun.Text;
            }
            if (!String.IsNullOrEmpty(txt_CustomerId.Text))
            {
                row.CustomerId = int.Parse(txt_CustomerId.Text);
            }
            row.ClientId = LocalUser.Instance.LogInInformation.ClientId;
            if (row.CardPayGubun == "N")
            {
                row.PayAccountNo = "";
                row.PayBankCode = "";
                row.PayBankName = "";
                row.PayInputName = "";
            }
            else
            {
                var Query = cmDataSet.Clients.Where(c => c.ClientId == int.Parse(txt_CustomerId.Text)).ToArray();
                if (Query.Any())
                {
                    row.PayAccountNo = Query.First().CMSAccountNo.ToString();
                    row.PayBankCode = Query.First().CMSBankCode.ToString();
                    row.PayBankName = Query.First().CMSBankName.ToString();
                    row.PayInputName = Query.First().CMSOwner.ToString();
                }
            }
            row.SetYN = 0;
            row.PayState = 2;
            row.Issue = false;
            row.SourceType = 0;
             
            cmDataSet.SalesManage.AddSalesManageRow(row);
            try
            {
                salesManageTableAdapter.Update(row);
                _salesId = row.SalesId;

                int _Taxgubun = 0;

                if (rdb_Tax0.Checked == true)
                {
                    _Taxgubun = 0;
                }
                else if (rdb_Tax1.Checked == true)
                {
                    _Taxgubun = 1;
                }
                else
                {
                    _Taxgubun = 2;
                }

                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {


                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();



                    cmd.CommandText =
                       "UPDATE SalesManage SET Taxgubun = @Taxgubun WHERE SalesId = @SalesId AND ClientId = @Clientid";



                    cmd.Parameters.AddWithValue("@Taxgubun", _Taxgubun);
                    cmd.Parameters.AddWithValue("@SalesId", _salesId);
                  
                    cmd.Parameters.AddWithValue("@Clientid", LocalUser.Instance.LogInInformation.ClientId);

                    cmd.ExecuteNonQuery();
                    cn.Close();


                }


                SalesDataSet.SalesManageDetailRow row2 = salesDataSet.SalesManageDetail.NewSalesManageDetailRow();
                row2.SalesId = _salesId;
                row2.itemName1 = txt_Item1.Text;
                row2.qty1 = txt_Num1.Text;

                if (rdb_Tax1.Checked)
                {
                    row2.unitCost1 = Convert.ToDecimal(txt_Price1.Text);
                }
                else
                {
                    row2.unitCost1 = Convert.ToDecimal(txt_UnitPrice1.Text);
                }
                row2.supplyCost1 =Convert.ToDecimal(txt_Price1.Text);
                row2.tax1 =Convert.ToDecimal(txt_VAT1.Text);

                if(!String.IsNullOrEmpty(txt_Item2.Text))
                { 
                    row2.itemName2 = txt_Item2.Text;
                    row2.qty2 = txt_Num2.Text;
                    if (rdb_Tax1.Checked)
                    {
                        row2.unitCost2 = Convert.ToDecimal(txt_Price2.Text);
                    }
                    else
                    {
                        row2.unitCost2 = Convert.ToDecimal(txt_UnitPrice2.Text);
                    }
                       
                    row2.supplyCost2 = Convert.ToDecimal(txt_Price2.Text);
                    row2.tax2 = Convert.ToDecimal(txt_VAT2.Text);
                }

                if (!String.IsNullOrEmpty(txt_Item3.Text))
                {
                    row2.itemName3 = txt_Item3.Text;
                    row2.qty3 = txt_Num3.Text;
                    if (rdb_Tax1.Checked)
                    {
                        row2.unitCost3 = Convert.ToDecimal(txt_Price3.Text);
                    }
                    else
                    {
                        row2.unitCost3 = Convert.ToDecimal(txt_UnitPrice3.Text);
                    }
                    row2.supplyCost3 = Convert.ToDecimal(txt_Price3.Text);
                    row2.tax3 = Convert.ToDecimal(txt_VAT3.Text);
                }

                if (!String.IsNullOrEmpty(txt_Item4.Text))
                {
                    row2.itemName4 = txt_Item4.Text;
                    row2.qty4 = txt_Num4.Text;
                    if (rdb_Tax1.Checked)
                    {
                        row2.unitCost4 = Convert.ToDecimal(txt_Price4.Text);
                    }
                    else
                    {
                        row2.unitCost4 = Convert.ToDecimal(txt_UnitPrice4.Text);
                    }
                    row2.supplyCost4 = Convert.ToDecimal(txt_Price4.Text);
                    row2.tax4 = Convert.ToDecimal(txt_VAT4.Text);
                }
                salesDataSet.SalesManageDetail.AddSalesManageDetailRow(row2);
                try
                {
                    salesManageDetailTableAdapter.Update(row2);
                }
                catch
                {

                }

            }
            catch (Exception E)
            {
                MessageBox.Show("매출관리 정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
        public bool IsSuccess = false;


        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {


                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "배차 외 추가", 1), "배차 외 추가 성공");
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

        private void txt_UnitPrice_Leave(object sender, EventArgs e)
        {
            decimal Amount1 = 0;
            decimal Amount2 = 0;
            decimal Amount3 = 0;
            decimal Amount4 = 0;

            if (!string.IsNullOrEmpty(txt_UnitPrice1.Text) && !string.IsNullOrEmpty(txt_Num1.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice1.Text);
                decimal _Num = decimal.Parse(txt_Num1.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                txt_UnitPrice1.Text = _UnitPrice.ToString("N0");
                txt_Num1.Text = _Num.ToString("N0");
            }

            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num2.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice2.Text);
                decimal _Num = decimal.Parse(txt_Num2.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                txt_UnitPrice2.Text = _UnitPrice.ToString("N0");
                txt_Num2.Text = _Num.ToString("N0");
            }


            if (!string.IsNullOrEmpty(txt_UnitPrice3.Text) && !string.IsNullOrEmpty(txt_Num3.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice3.Text);
                decimal _Num = decimal.Parse(txt_Num3.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                txt_UnitPrice3.Text = _UnitPrice.ToString("N0");
                txt_Num3.Text = _Num.ToString("N0");
            }

            if (!string.IsNullOrEmpty(txt_UnitPrice4.Text) && !string.IsNullOrEmpty(txt_Num4.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice4.Text);
                decimal _Num = decimal.Parse(txt_Num4.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                txt_UnitPrice4.Text = _UnitPrice.ToString("N0");
                txt_Num4.Text = _Num.ToString("N0");
            }

            txt_Amount.Text = (Amount1 + Amount2 + Amount3 + Amount4).ToString("N0");
        }

        private void txt_Num_Leave(object sender, EventArgs e)
        {
            decimal Amount1 = 0;
            decimal Amount2 = 0;
            decimal Amount3 = 0;
            decimal Amount4 = 0;

            if (!string.IsNullOrEmpty(txt_UnitPrice1.Text) && !string.IsNullOrEmpty(txt_Num1.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice1.Text);
                decimal _Num = decimal.Parse(txt_Num1.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                txt_UnitPrice1.Text = _UnitPrice.ToString("N0");
                txt_Num1.Text = _Num.ToString("N0");
            }

            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num2.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice2.Text);
                decimal _Num = decimal.Parse(txt_Num2.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                txt_UnitPrice2.Text = _UnitPrice.ToString("N0");
                txt_Num2.Text = _Num.ToString("N0");
            }


            if (!string.IsNullOrEmpty(txt_UnitPrice3.Text) && !string.IsNullOrEmpty(txt_Num3.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice3.Text);
                decimal _Num = decimal.Parse(txt_Num3.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                txt_UnitPrice3.Text = _UnitPrice.ToString("N0");
                txt_Num3.Text = _Num.ToString("N0");
            }

            if (!string.IsNullOrEmpty(txt_UnitPrice4.Text) && !string.IsNullOrEmpty(txt_Num4.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice4.Text);
                decimal _Num = decimal.Parse(txt_Num4.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                txt_UnitPrice4.Text = _UnitPrice.ToString("N0");
                txt_Num4.Text = _Num.ToString("N0");
            }

            txt_Amount.Text = (Amount1 + Amount2 + Amount3 + Amount4).ToString("N0");
        }

        private void txt_UnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == 45))


                //if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void btn_CustomerInfo_Click(object sender, EventArgs e)
        {
            if (rdb_Normal.Checked)
            {
                FrmCustomerSearch _frmCustomerSearch = new FrmCustomerSearch("1,3");
                _frmCustomerSearch.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
                {
                    if (ie.KeyCode != Keys.Return) return;
                    if (_frmCustomerSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCustomerSearch.grid1.SelectedCells[0].RowIndex < 0) return;

                    txt_SangHo.Text = _frmCustomerSearch.grid1[0, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCustomerSearch.grid1[1, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCustomerSearch.grid1[2, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCustomerSearch.grid1[3, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCustomerSearch.grid1[4, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_State.Text = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCustomerSearch.grid1[20, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_AddrDetail.Text = _frmCustomerSearch.grid1[7, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCustomerSearch.grid1[8, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCustomerSearch.grid1[9, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCustomerSearch.grid1[16, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CardPayGubun.Text = "N";
                    txt_CustomerId.Text = _frmCustomerSearch.grid1[12, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo_Leave(null, null);
                    _frmCustomerSearch.Close();
                });
                _frmCustomerSearch.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
                {
                    if (_frmCustomerSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCustomerSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                    txt_SangHo.Text = _frmCustomerSearch.grid1[0, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCustomerSearch.grid1[1, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCustomerSearch.grid1[2, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCustomerSearch.grid1[3, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCustomerSearch.grid1[4, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_State.Text = _frmCustomerSearch.grid1[5, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCustomerSearch.grid1[6, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCustomerSearch.grid1[20, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_AddrDetail.Text = _frmCustomerSearch.grid1[7, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCustomerSearch.grid1[8, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCustomerSearch.grid1[9, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCustomerSearch.grid1[16, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CustomerId.Text = _frmCustomerSearch.grid1[12, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_CardPayGubun.Text = "N";
                    txt_BizNo_Leave(null, null);
                    _frmCustomerSearch.Close();
                });

               




                _frmCustomerSearch.Owner = this;
                _frmCustomerSearch.StartPosition = FormStartPosition.CenterParent;
                _frmCustomerSearch.ShowDialog();

            }
            else
            {
                FrmCardSearch _frmCardSearch = new FrmCardSearch();
                _frmCardSearch.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
                {
                    if (ie.KeyCode != Keys.Return) return;
                    if (_frmCardSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCardSearch.grid1.SelectedCells[0].RowIndex < 0) return;

                    txt_SangHo.Text = _frmCardSearch.grid1[0, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCardSearch.grid1[1, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCardSearch.grid1[3, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCardSearch.grid1[4, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_State.Text = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCardSearch.grid1[40, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_AddrDetail.Text = _frmCardSearch.grid1[7, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCardSearch.grid1[8, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCardSearch.grid1[10, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CardPayGubun.Text = "C";
                    txt_CustomerId.Text = _frmCardSearch.grid1[12, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo_Leave(null, null);
                    _frmCardSearch.Close();
                });
                _frmCardSearch.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
                {
                    if (_frmCardSearch.grid1.SelectedCells.Count == 0) return;
                    if (_frmCardSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                    txt_SangHo.Text = _frmCardSearch.grid1[0, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo.Text = _frmCardSearch.grid1[1, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CEO.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Uptae.Text = _frmCardSearch.grid1[3, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Upjong.Text = _frmCardSearch.grid1[4, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    //cmb_AddressState.SelectedValue = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    //cmb_AddressCity.SelectedValue = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_State.Text = _frmCardSearch.grid1[5, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_City.Text = _frmCardSearch.grid1[6, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Zip.Text = _frmCardSearch.grid1[40, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();

                    txt_AddrDetail.Text = _frmCardSearch.grid1[7, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_Email.Text = _frmCardSearch.grid1[8, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_ContRactName.Text = _frmCardSearch.grid1[2, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_MobileNo.Text = _frmCardSearch.grid1[10, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_CardPayGubun.Text = "C";
                    txt_CustomerId.Text = _frmCardSearch.grid1[12, _frmCardSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                    txt_BizNo_Leave(null, null);
                    _frmCardSearch.Close();
                });






                _frmCardSearch.Owner = this;
                _frmCardSearch.StartPosition = FormStartPosition.CenterParent;
                _frmCardSearch.ShowDialog();


            }
            // txt_AddrDetail.Focus();
        }
      //  public SalesDataSet.SalesManageRow[] Sales { get; set; }
        private void btnEtax_Click(object sender, EventArgs e)
        {
            LocalUser.Instance.LogInInformation.LoadClient();

            var _Q = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

            if(!_Q.Any())
            {
                MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                FrmMDI.LoadForm("FrmMN0204", "Point");


                return;
            }

            var _ClientPoint = ShareOrderDataSet.Instance.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Sum(c => c.Amount);


            if (_ClientPoint < 150)
            {

                MessageBox.Show("충전금이 부족하여\r\n알림톡/계산서 발행을 하실 수 없습니다.\r\n해당 가상계좌로 충전하신 후, 이용하시기 바랍니다.", "충전금 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                FrmMDI.LoadForm("FrmMN0204", "Point");


                return;
            }

            if (_UpdateDB() > 0)
            {
                var myClient = LocalUser.Instance.LogInInformation.Client; //cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
                //_salesId
                if (LocalUser.Instance.LogInInformation.Client.EtaxGubun == "P")
                {
                   
                    var checkIsMember = taxinvoiceService.CheckIsMember(myClient.BizNo.Replace("-", ""), LinkID);
                    if (checkIsMember.code == 0)
                    {
                        // MessageBox.Show("먼저 전자세금계산서 연동서비스를 신청해주시기바랍니다..", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        MessageBox.Show("전자세금계산서 발행을 위한 별도 회원가입이 필요 합니다\r\n" +
                            "아래 “확인” 버튼을 눌러, 팝빌 홈페이지에 접속 합니다.\r\n" +
                            " ① “회원가입”->”연동회원” 클릭.\r\n" +
                            " ② “링크아이디”는 “EDUBILL”를 입력\r\n" +
                            $" ③ “아이디”는 “{myClient.LoginId}”를 입력\r\n" +
                            $" ④ “비밀번호”는 “{myClient.Password}”를 입력\r\n" +
                            "※. 위 아이디/비밀번호는 별도 메모 하셔야 합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        FrmPopbillRegister f = new Admin.FrmPopbillRegister();
                        f.Show();
                        return;
                    }
                }
                else
                {
                    //if (String.IsNullOrEmpty(myClient.userid))
                    //{

                    if (rdo_D.Checked)
                    {
                        #region Nice전자세금계산서
                        var _Email = myClient.Email.Split('@');
                        var checkIsMember = dTIServiceClass.GetMembJoinInf(linkcd, _Email[0], myClient.BizNo.Replace("-", ""), myClient.CEO, myClient.Email, myClient.MobileNo);

                        if (!String.IsNullOrEmpty(checkIsMember))
                        {
                            var ResultList = checkIsMember.Split('/');

                            try
                            {
                                var _Client = LocalUser.Instance.LogInInformation.Client;

                                var retVal = ResultList[0];
                                var errMsg = ResultList[1];
                                var frnNo = ResultList[2];
                                var userid = ResultList[3];
                                var passwd = ResultList[4];

                                if (retVal == "0")
                                {
                                    //return;
                                }
                                else
                                {
                                    var NiceMemberJoin = dTIServiceClass.Memberjoin(linkcd, _Email[0], _Client.BizNo.Replace("-", ""), _Client.Name, _Client.CEO, _Client.Uptae, _Client.Upjong, _Client.CEO, _Client.Email, _Client.PhoneNo, _Client.MobileNo, _Client.ZipCode, _Client.AddressState, _Client.AddressCity + _Client.AddressDetail);


                                    if (!String.IsNullOrEmpty(NiceMemberJoin))
                                    {

                                        var NiceMemberJoinList = NiceMemberJoin.Split('/');

                                        try
                                        {
                                            //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                                            retVal = NiceMemberJoinList[0];
                                            errMsg = NiceMemberJoinList[1];
                                            frnNo = NiceMemberJoinList[2];
                                            userid = NiceMemberJoinList[3];
                                            passwd = NiceMemberJoinList[4];

                                            if (retVal == "0")
                                            {
                                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                                {


                                                    cn.Open();
                                                    SqlCommand cmd = cn.CreateCommand();



                                                    cmd.CommandText =
                                                       "UPDATE Clients SET linkcd = @linkcd,frnNo = @frnNo , userid = @userid, passwd = @passwd WHERE ClientId = @Clientid";



                                                    cmd.Parameters.AddWithValue("@linkcd", linkcd);
                                                    cmd.Parameters.AddWithValue("@frnNo", frnNo);
                                                    cmd.Parameters.AddWithValue("@userid", userid);
                                                    cmd.Parameters.AddWithValue("@passwd", passwd);
                                                    cmd.Parameters.AddWithValue("@Clientid", LocalUser.Instance.LogInInformation.ClientId);

                                                    cmd.ExecuteNonQuery();
                                                    cn.Close();


                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(errMsg);

                                            }



                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());
                                        }


                                    }

                                }



                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                            LocalUser.Instance.LogInInformation.LoadClient();

                        }
                        #endregion
                        




                        #region 나이스 공인인증서
                        adminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
                        //var Query = baseDataSet.cl.FirstOrDefault();
                        var _Clients = LocalUser.Instance.LogInInformation.Client;
                        var Result = dTIServiceClass.selectExpireDt("EDB", _Clients.frnNo, _Clients.userid, _Clients.passwd);

                        if (!String.IsNullOrEmpty(Result))
                        {
                            var ResultList = Result.Split('/');

                            try
                            {
                                //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                                var retVal = ResultList[0];
                                var errMsg = ResultList[1];
                                var regYn = ResultList[2];
                                var expireDt = ResultList[3];


                                if (regYn == "N")
                                {
                                    MessageBox.Show("공인인증서를 등록하셔야 합니다. 공인인증 등록 페이지로 이동합니다.\r\n인증서 등록후 회원정보에 공인인증서 비밀번호를 입력하세요.");
                                    // MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);


                                    var Query = baseDataSet.AdminInfoes.FirstOrDefault();
                                    string url = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do?";
                                    if (Query.IsTest != true)
                                    {
                                        url = "http://www.nicedata.co.kr/ti/TI_80101.do?";
                                    }


                                    string frnNo = "";
                                    string userid = "";
                                    string passwd = "";
                                    string Linkcd = "";

                                    try
                                    {
                                        frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.frnNo }).ToString();
                                        userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.userid }).ToString();
                                        passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.passwd }).ToString();
                                        Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "EDB" }).ToString();


                                        //MessageBox.Show(Encode.ToString());

                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.Write(ex.Message);

                                    }

                                    string value = $"frnNo={frnNo}&userId={userid}&passwd={passwd}&linkCd={Linkcd}&retUrl=";

                                    FrmNice f = new FrmNice(value, url);
                                    f.ShowDialog();


                                }
                                else
                                {
                                    //  MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);

                                }



                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }

                        #endregion

                        if (String.IsNullOrEmpty(_Clients.NPKIpasswd))
                        {
                            MessageBox.Show("공인인증서 비밀번호를 등록후 이용하시기 바랍니다.", "공인인증서 확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                            FrmMDI.LoadForm("FrmMN0204", "");


                            return;
                        }
                    }
                    
                }
                newDGV1.DataSource = null;
                string errormessage = string.Empty;

                this.salesManageTableAdapter1.Fill(this.salesDataSet.SalesManage);
                salesManageBindingSource.Filter = string.Format("salesId =  '{0}'", _salesId);

                newDGV1.DataSource = salesManageBindingSource;

                var Datas = new List<SalesDataSet.SalesManageRow>();
                for (int i = 0; i < newDGV1.RowCount; i++)
                {
                    newDGV1.RefreshEdit();

                    var _Row = ((DataRowView)newDGV1.Rows[i].DataBoundItem).Row as SalesDataSet.SalesManageRow;
                    _Row.RejectChanges();

                    _Row.IssueDate = DateTime.Now;
                    Datas.Add(_Row);

                }

                string _rdopurposeType = "";
                if (radioButton1.Checked)
                {
                    _rdopurposeType = "영수";

                }
                else
                {
                    _rdopurposeType = "청구";
                }

                string _staxtype = "";
              
               if (rdb_Tax2.Checked)
                {
                    _staxtype = "면세";
                }
                else
                {
                    _staxtype = "과세";

                }

                if (LocalUser.Instance.LogInInformation.Client.EtaxGubun == "P")
                {
                    Dialog_EtaxManage d = new Dialog_EtaxManage(_rdopurposeType, _staxtype, "Direct");
                    d.Sales = Datas.ToArray();

                    d.ShowDialog();
                }
                else
                {
                    if(rdo_D.Checked)
                    {
                        Dialog_EtaxManageNice d = new Dialog_EtaxManageNice(_rdopurposeType, _staxtype, "Direct");
                        d.Sales = Datas.ToArray();

                        d.ShowDialog();
                    }
                    else
                    {
                        Dialog_EtaxManageNiceR d = new Dialog_EtaxManageNiceR(_rdopurposeType, _staxtype, "Direct");
                        d.Sales = Datas.ToArray();

                        d.ShowDialog();

                    }

                }
                this.Close();

            }


        }

        private void cmb_AddressState_SelectedIndexChanged(object sender, EventArgs e)
        {
          ////  cmb_AddressState.Enabled = true;
          //  cmb_AddressCity.Enabled = true;



          //  var ParkCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
          //  cmb_AddressCity.DataSource = ParkCityDataSource;
          //  cmb_AddressCity.DisplayMember = "City";
          //  cmb_AddressCity.ValueMember = "City";
        }

        private void txt_BizNo_Leave(object sender, EventArgs e)
        {
            if (txt_BizNo.Text.Length != 12)
            {
                lbl_HasAcc.Visible = false;
                cmb_HasAcc.Visible = false;
                panel4.Visible = false;
                cmb_HasAcc.SelectedIndex = 0;
                return;
            }

            var Query = cmDataSet.Clients.Where(c=> c.BizNo.Replace("-","") == txt_BizNo.Text.Replace("-","")).Where(c=> c.PAYLOGISYN == 1).ToArray();

            if (Query.Any())
            {
               // lbl_HasAcc.Visible = true;
               // cmb_HasAcc.Visible = true;
               // panel4.Visible = true;
            }
            else
            {
                lbl_HasAcc.Visible = false;
                cmb_HasAcc.Visible = false;
                panel4.Visible = false;
                cmb_HasAcc.SelectedIndex = 0;
            }


        }

        private void rdb_Normal_CheckedChanged(object sender, EventArgs e)
        {
            lbl_HasAcc.Visible = false;
            cmb_HasAcc.Visible = false;
            panel4.Visible = false;



            txt_SangHo.Text = "";
            txt_BizNo.Text ="";
            txt_CEO.Text = "";
            txt_Uptae.Text = "";
            txt_Upjong.Text = "";

            txt_State.Text = "";
            txt_City.Text = "";
            txt_Zip.Text = "";

            txt_AddrDetail.Text = "";
            txt_Email.Text = "";
            txt_ContRactName.Text = "";
            txt_MobileNo.Text = "";
            txt_CardPayGubun.Text = "N";
            txt_CustomerId.Text = "";
            txt_BizNo_Leave(null, null);

        }

        private void rdb_Cardpay_CheckedChanged(object sender, EventArgs e)
        {
            //lbl_HasAcc.Visible = true;
            //cmb_HasAcc.Visible = true;
           // panel4.Visible = true;


            txt_SangHo.Text = "";
            txt_BizNo.Text = "";
            txt_CEO.Text = "";
            txt_Uptae.Text = "";
            txt_Upjong.Text = "";


            txt_State.Text = "";
            txt_City.Text = "";
            txt_Zip.Text = "";

            txt_AddrDetail.Text = "";
            txt_Email.Text = "";
            txt_ContRactName.Text = "";
            txt_MobileNo.Text = "";
            txt_CardPayGubun.Text = "C";
            txt_CustomerId.Text = "";
            txt_BizNo_Leave(null, null);
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
        private void btnFindZip_Click(object sender, EventArgs e)
        {
            foreach (var _Model in _ErrorModelList)
            {
                ((ToolTip)_Model._Control.Tag).Hide(_Model._Control);
            }
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                txt_Zip.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_State.Text = ss[0];
                txt_City.Text = ss[1];
                txt_AddrDetail.Text = String.Join(" ", ss.Skip(2));
            }
        }

        private void txt_UnitPrice_Enter(object sender, EventArgs e)
        {
            txt_UnitPrice1.Text = txt_UnitPrice1.Text.Replace(",", "");
        }

        private void txt_Num_Enter(object sender, EventArgs e)
        {
            txt_Num1.Text = txt_Num1.Text.Replace(",", "");
        }

        private void rdoChk()
        {
            decimal Amount1 = 0;
            decimal Amount2 = 0;
            decimal Amount3 = 0;
            decimal Amount4 = 0;

            if (!string.IsNullOrEmpty(txt_UnitPrice1.Text) && !string.IsNullOrEmpty(txt_Num1.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice1.Text);
                decimal _Num = decimal.Parse(txt_Num1.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price1.Text = _Price.ToString("N0");
                    txt_VAT1.Text = _Vat.ToString("N0");
                    Amount1 = _Amount;
                }
                txt_UnitPrice1.Text = _UnitPrice.ToString("N0");
                txt_Num1.Text = _Num.ToString("N0");
            }

            if (!string.IsNullOrEmpty(txt_UnitPrice2.Text) && !string.IsNullOrEmpty(txt_Num2.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice2.Text);
                decimal _Num = decimal.Parse(txt_Num2.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price2.Text = _Price.ToString("N0");
                    txt_VAT2.Text = _Vat.ToString("N0");
                    Amount2 = _Amount;
                }
                txt_UnitPrice2.Text = _UnitPrice.ToString("N0");
                txt_Num2.Text = _Num.ToString("N0");
            }


            if (!string.IsNullOrEmpty(txt_UnitPrice3.Text) && !string.IsNullOrEmpty(txt_Num3.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice3.Text);
                decimal _Num = decimal.Parse(txt_Num3.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price3.Text = _Price.ToString("N0");
                    txt_VAT3.Text = _Vat.ToString("N0");
                    Amount3 = _Amount;
                }
                txt_UnitPrice3.Text = _UnitPrice.ToString("N0");
                txt_Num3.Text = _Num.ToString("N0");
            }

            if (!string.IsNullOrEmpty(txt_UnitPrice4.Text) && !string.IsNullOrEmpty(txt_Num4.Text))
            {
                decimal _UnitPrice = decimal.Parse(txt_UnitPrice4.Text);
                decimal _Num = decimal.Parse(txt_Num4.Text);
                if (rdb_Tax0.Checked)
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = Math.Floor(_Price * 0.1m);
                    _Amount = (_Price + _Vat);
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                else if (rdb_Tax1.Checked)
                {
                    decimal _Amount = _UnitPrice * _Num;
                    decimal _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    decimal _Price = _Amount - _Vat;
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                else
                {
                    decimal _Price = _UnitPrice * _Num;
                    decimal _Vat = 0;
                    decimal _Amount = 0;
                    _Vat = 0;
                    _Amount = (_Price + _Vat);
                    txt_Price4.Text = _Price.ToString("N0");
                    txt_VAT4.Text = _Vat.ToString("N0");
                    Amount4 = _Amount;
                }
                txt_UnitPrice4.Text = _UnitPrice.ToString("N0");
                txt_Num4.Text = _Num.ToString("N0");
            }

            txt_Amount.Text = (Amount1 + Amount2 + Amount3 + Amount4).ToString("N0");
        }
        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();
        }

        private void rdb_Tax1_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();
        }

        private void rdb_Tax2_CheckedChanged(object sender, EventArgs e)
        {
            rdoChk();
        }
    }
}
