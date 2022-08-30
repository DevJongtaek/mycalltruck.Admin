using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmTrade_Client : Form
    {
        DESCrypt m_crypt = null;
        int intTradeIds = 0;
        public FrmTrade_Client()
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");
            cmb_PayState.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            cmb_Date.SelectedIndex = 0;
            cmb_ClientSerach.SelectedIndex = 0;
            cmb_AllowAcc.SelectedIndex = 0;
            cmb_HasAcc_I.SelectedIndex = 0;
            cmb_LGD_Last_Function.SelectedIndex = 0;
            dtp_From.Value = DateTime.Now.AddMonths(-1);
            dtp_To.Value = DateTime.Now;
            _InitCmb();
            btn_AcceptAcc.Enabled = false;
            btn_CancelAcc.Enabled = false;
            //화주
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btnUpdate.Enabled = true;
                label10.Visible = true;
                chk_PayState.Visible = true;
                cmb_ClientSerach.Visible = false;
                txt_ClientSearch.Visible = false;
                if (LocalUser.Instance.LogInInformation.ClientUserId == 0)
                {
                    btn_AcceptAcc.Enabled = true;
                    btn_CancelAcc.Enabled = true;
                }
            }
            //관리자
            else
            {
                btnUpdate.Enabled = false;
                btnCurrentDelete.Enabled = false;
                btnExcel.Enabled = false;
                label10.Visible = false;
                chk_PayState.Visible = false;
            }
        }
        private void _InitCmb()
        {

            Dictionary<string, string> PayBank = new Dictionary<string, string>();





            //PayBank.Add(" ", "은행선택");
            //PayBank.Add("88", "신한은행");
            //PayBank.Add("35", "제주은행");
            //PayBank.Add("04", "국민은행");
            //PayBank.Add("11", "농협");
            //PayBank.Add("20", "우리은행");
            //PayBank.Add("81", "하나은행");
            //PayBank.Add("05", "외한은행");
            //PayBank.Add("71", "우체국");
            //PayBank.Add("03", "기업은행");
            //PayBank.Add("23", "SC제일은행");
            //PayBank.Add("53", "시티은행");
            //PayBank.Add("07", "수협");
            //PayBank.Add("02", "산업은행");
            //PayBank.Add("50", "상호저축");
            //PayBank.Add("48", "신협");
            //PayBank.Add("45", "새마을");
            //PayBank.Add("39", "경남은행");
            //PayBank.Add("37", "전북은행");
            //PayBank.Add("34", "광주은행");
            //PayBank.Add("32", "부산은행");
            //PayBank.Add("31", "대구은행");
            //PayBank.Add("54", "HSBC은행");
            //PayBank.Add("12", "단위농협");
            //PayBank.Add("27", "한미은행");
            //PayBank.Add("21", "구(조흥)");


            //PayBank.Add(" ", "은행선택");
            //PayBank.Add("003", "기업은행");
            //PayBank.Add("004", "국민은행");
            //PayBank.Add("011", "농협");
            //PayBank.Add("012", "단위농협");
            //PayBank.Add("020", "우리은행");
            //PayBank.Add("031", "대구은행");
            //PayBank.Add("005", "외한은행");
            //PayBank.Add("023", "SC제일은행");
            //PayBank.Add("032", "부산은행");
            //PayBank.Add("045", "새마을");
            //PayBank.Add("027", "한국시티은행");
            //PayBank.Add("034", "광주은행");
            //PayBank.Add("039", "경남은행");
            //PayBank.Add("007", "수협");
            //PayBank.Add("048", "신협");
            //PayBank.Add("037", "전북은행");
            //PayBank.Add("035", "제주은행");
            //PayBank.Add("064", "산림조합");
            //PayBank.Add("071", "우체국");
            //PayBank.Add("081", "하나은행");
            //PayBank.Add("088", "신한은행");
            //PayBank.Add("209", "동양종금증권");
            //PayBank.Add("243", "한국투자증권");
            //PayBank.Add("240", "삼성증권");
            //PayBank.Add("230", "미래에셋");
            //PayBank.Add("247", "우리투자증권");
            //PayBank.Add("218", "현대증권");
            //PayBank.Add("266", "SK증권");
            //PayBank.Add("278", "신한금융투자");
            //PayBank.Add("262", "하이증권");
            //PayBank.Add("263", "HMC증권");
            //PayBank.Add("267", "대신증권");
            //PayBank.Add("270", "하나대투증권");
            //PayBank.Add("279", "동부증권");
            //PayBank.Add("280", "유진증권");
            //PayBank.Add("287", "메리츠증권");
            //PayBank.Add("291", "신영증권");

            PayBank.Add(" ", "은행선택");
            PayBank.Add("003", "기업은행");
            PayBank.Add("005", "외한은행");
            PayBank.Add("004", "국민은행");
            PayBank.Add("011", "농협");
            PayBank.Add("020", "우리은행");
            PayBank.Add("088", "신한은행");
            PayBank.Add("023", "SC제일");
            PayBank.Add("027", "씨티은행");
            PayBank.Add("032", "부산은행");
            PayBank.Add("039", "경남은행");
            PayBank.Add("031", "대구은행");
            PayBank.Add("071", "우체국");
            PayBank.Add("034", "광주은행");
            PayBank.Add("007", "수협");
            PayBank.Add("022", "상업은행");
            PayBank.Add("030", "대동은행");
            PayBank.Add("033", "충청은행");
            PayBank.Add("035", "제주은행");
            PayBank.Add("036", "경기은행");
            PayBank.Add("037", "전북은행");
            PayBank.Add("038", "강원은행");
            PayBank.Add("040", "충북은행");
            PayBank.Add("081", "하나은행");
            PayBank.Add("082", "보람은행");
            PayBank.Add("002", "산업은행");
            PayBank.Add("045", "새마을금고");
            PayBank.Add("054", "HSBC은행");
            PayBank.Add("048", "신협");
            PayBank.Add("090", "카카오뱅크");
            PayBank.Add("089", "케이뱅크");

            PayBank.Add("S0", "유안타증권");
            PayBank.Add("S1", "미래에셋");
            PayBank.Add("S2", "신한금융투자");
            PayBank.Add("S3", "삼성증권");
            PayBank.Add("S6", "한국투자증권");
            PayBank.Add("SG", "한화증권");


            cmb_PayBankName.DataSource = new BindingSource(PayBank, null);
            cmb_PayBankName.DisplayMember = "Value";
            cmb_PayBankName.ValueMember = "Key";

         


              Dictionary<string, string> HasACCList = new Dictionary<string, string>();

              HasACCList.Add("0", "현금");
              HasACCList.Add("1", "카드");
              cmb_HasAcc.DataSource = new BindingSource(HasACCList, null);
              cmb_HasAcc.DisplayMember = "Value";
              cmb_HasAcc.ValueMember = "Key";

           
            
        }
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void txt_MobileNo_TextChanged(object sender, EventArgs e)
        {

        }


        private void FrmTrade_Load(object sender, EventArgs e)
        {
            this.SalesManage2TableAdapter.Fill(this.cMDataSet.SalesManage2);
            clientsTableAdapter.Fill(cMDataSet.Clients);
            try
            {
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    this.driversInfoTableAdapter.Fill(this.cMDataSet.DriversInfo, LocalUser.Instance.LogInInformation.ClientId);
                }
                else
                {
                    this.driversInfoTableAdapter.FillByAdmin(this.cMDataSet.DriversInfo);
                }
                btn_Search_Click(null, null);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;

        private void btnExcel_Click(object sender, EventArgs e)
        {
            FrmTradeExcel _frmTradeExcel = new FrmTradeExcel();
            _frmTradeExcel.Owner = this;
            _frmTradeExcel.StartPosition = FormStartPosition.CenterParent;
            if (_frmTradeExcel.ShowDialog() == DialogResult.OK)
            {
                fileString = string.Format("세금계산서EXCEL_{0}_{1}", dtp_From.Text.Replace("/", ""), dtp_To.Text.Replace("/", ""));
                title = "세금계산서";
                if (_frmTradeExcel.cmb_Excel.SelectedIndex == 0)
                {
                    ieExcel = Properties.Resources.세금계산서;
                }
                else
                {
                    ieExcel = Properties.Resources.신한은행_양식;
                }

                if (_frmTradeExcel.cmb_Excel.SelectedIndex == 0)
                {
                    if (dataGridView1.RowCount == 0)
                    {
                        MessageBox.Show("내보낼 세금계산서 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (dataGridView1.RowCount > 0)
                    {
                        dataGridView1.Columns[0].Visible = false;
                        pnProgress.Visible = true;
                        bar.Value = 0;
                        Thread t = new Thread(new ThreadStart(() =>
                        {
                            dataGridView1.ExportExistExcel2(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.TAX + "\\" + LocalUser.Instance.LogInInformation.ClientName + "\\VAT");
                            pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                            pnProgress.Invoke(new Action(() => dataGridView1.Columns[0].Visible = true));
                        }));
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                    }
                    else
                    {
                        MessageBox.Show("엑셀로 내보낼 세금계산서정보가 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {

                    if (dataGridView2.RowCount == 0)
                    {
                        MessageBox.Show("내보낼 세금계산서 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (dataGridView2.RowCount > 0)
                    {
                        pnProgress.Visible = true;
                        bar.Value = 0;
                        Thread t = new Thread(new ThreadStart(() =>
                        {
                            dataGridView2.ExportExistExcel2(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.TAX + "\\" + LocalUser.Instance.LogInInformation.ClientName + "\\VAT");
                            pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        }));
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                    }
                    else
                    {
                        MessageBox.Show("엑셀로 내보낼 세금계산서정보가 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
        }

        private void btnAllPrint_Click(object sender, EventArgs e)
        {
            getFilterString();
            string errormessage = string.Empty;
            if (checkedCodes.Count() == 0)
            {
                MessageBox.Show("출력할 건을 선택하십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var TradeIds = dataGridView1.Rows.Cast<DataGridViewRow>().Where(c => c.Visible).Where(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.TradesRow).VAT != 0 &&(((DataRowView)c.DataBoundItem).Row as CMDataSet.TradesRow).SUMYN != 2 && (((DataRowView)c.DataBoundItem).Row as CMDataSet.TradesRow).CheckBox).Select(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.TradesRow).TradeId).ToArray();
            FrmTax f = new FrmTax(TradeIds);
            f.PrintClient();
            f.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


           


        }

        int GridIndex = 0;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
          
                err.Clear();

                if (txt_Item.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Item, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

                }


                if (txt_Price.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }

                if (cmb_PayBankName.SelectedIndex == 0)
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(cmb_PayBankName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }


                if (txt_PayAccountNo.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_PayAccountNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }


                if (txt_PayInputName.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_PayInputName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }


                var PayState = 2;

                if (chk_PayState.Checked == true)
                {
                    PayState = 1;
                }

                if (salesManage2BindingSource.Current != null)
                {
                    var Selected = ((DataRowView)salesManage2BindingSource.Current).Row as CMDataSet.SalesManage2Row;
                    if (Selected != null)
                    {
                        Selected.PayState = PayState;
                        salesManage2BindingSource.EndEdit();
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                "UPDATE SalesManage SET  RequestDate = @RequestDate" +
                                ", Item = @Item " +
                                ", Price = @Price " +
                                ", VAT = @VAT " +
                                ", Amount = @Amount" +
                                ", PayState = @PayState " +
                                ", PayDate = @PayDate " +
                                ", PayBankName = @PayBankName" +
                                ", PayBankCode = @PayBankCode" +
                                ", PayAccountNo = @PayAccountNo" +
                                ", PayInputName = @PayInputName" +

                            //    ", CustomerId = @CustomerId" +
                                ", UseTax = @UseTax" +
                                 ", HasAcc = @HasAcc" +
                                " WHERE SalesId = @SalesId";

                            cmd.Parameters.AddWithValue("@RequestDate", dtp_RequestDate.Value);
                           
                       
                            cmd.Parameters.AddWithValue("@Item", txt_Item.Text);
                            cmd.Parameters.AddWithValue("@Price", txt_Price.Text);
                            cmd.Parameters.AddWithValue("@VAT", txt_VAT.Text);
                            cmd.Parameters.AddWithValue("@Amount", txt_Amount.Text);
                            cmd.Parameters.AddWithValue("@PayBankName", cmb_PayBankName.Text);
                            cmd.Parameters.AddWithValue("@PayBankCode", cmb_PayBankName.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@PayAccountNo", txt_PayAccountNo.Text);
                            cmd.Parameters.AddWithValue("@PayInputName", txt_PayInputName.Text);
                        //    cmd.Parameters.AddWithValue("@DriverId", txt_DriverId.Text);
                    //        cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInfomation.ClientId);

                            if (chk_Vat.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@UseTax", true);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@UseTax", false);
                            }

                            if (cmb_HasAcc.SelectedIndex == 0)
                            {
                                cmd.Parameters.AddWithValue("@HasAcc", false);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@HasAcc", true);
                            }

                            if (chk_PayState.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@PayState", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@PayState", 2);
                            }

                        //    cmd.Parameters.AddWithValue("@PayState", Selected.PayState);
                            cmd.Parameters.AddWithValue("@PayDate", dtp_PayDate.Value);
                            cmd.Parameters.AddWithValue("@SalesId", Selected.SalesId);


                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                }


                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "세금계산서", 1), "세금계산서 수정 성공");

                if (dataGridView1.RowCount > 1)
                {
                    GridIndex = salesManage2BindingSource.Position;
                    btn_Search_Click(null, null);


                    dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                }
                else
                {
                    btn_Search_Click(null, null);
                }

            

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (cmb_LGD_Last_Function.Text == "오류")
            {
                DataTable mDataTable = new DataTable();

                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT * FROM AccLogs Where AccFunction = '오류' And CreateDate >= @StartDate And CreateDate <= @StopDate Order by CreateDate DESC";
                    Command.Parameters.AddWithValue("@StartDate", dtp_From.Value);
                    Command.Parameters.AddWithValue("@StopDate", dtp_To.Value);
                    Connection.Open();
                    mDataTable.Load(Command.ExecuteReader());
                    Connection.Close();
                }
                Dialog_AccLogs d = new Dialog_AccLogs();
                d.SetListDataSource(mDataTable);
                d.ShowDialog();
                return;
            }



            LoadTable();


            try
            {
                var Filters = new List<string>();

                var FilterString = "";
                if (cmb_Search.Text == "승인일")
                {
                    FilterString = string.Format("LGD_Last_Function = '승인' AND LGD_Last_Date =  '{0}'", dtp_Search.Text);
                }
                else if (cmb_Search.Text == "취소일")
                {
                    FilterString = string.Format("LGD_Last_Function = '취소' AND LGD_Last_Date =  '{0}'", dtp_Search.Text);
                }
                else
                {
                    if (String.IsNullOrEmpty(txt_Search.Text))
                    {
                        FilterString = "";
                    }
                    else
                    {
                        if (cmb_Search.Text == "청구항목")
                        {
                            FilterString = string.Format("Item Like  '%{0}%'", txt_Search.Text);
                        }
                        else if (cmb_Search.Text == "계좌번호")
                        {
                            FilterString = string.Format("PayAccountNo Like  '%{0}%'", txt_Search.Text);
                        }
                        else if (cmb_Search.Text == "예금주")
                        {
                            FilterString = string.Format("PayInputName Like  '%{0}%'", txt_Search.Text);
                        }
                        else if (cmb_Search.Text == "청구지사업자번호")
                        {
                            //FilterString = string.Format("DriverBizNo Like  '%{0}%'", txt_Search.Text);
                            var codes = cMDataSet.Clients.Where(c => c.BizNo.Contains(txt_Search.Text)).Select(c => c.ClientId).ToArray();
                            if (codes.Count() > 0)
                            {
                                string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                                for (int i = 1; i < codes.Count(); i++)
                                {
                                    filter += string.Format(", '{0}'", codes[i]);
                                }
                                filter += ")";
                                FilterString = filter;
                            }
                            else
                                FilterString = "";
                        }
                        else if (cmb_Search.Text == "청구지상호")
                        {
                            var codes = cMDataSet.Clients.Where(c => c.Name.Contains(txt_Search.Text)).Select(c => c.ClientId).ToArray();
                            if (codes.Count() > 0)
                            {
                                string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                                for (int i = 1; i < codes.Count(); i++)
                                {
                                    filter += string.Format(", '{0}'", codes[i]);
                                }
                                filter += ")";
                                FilterString = filter;
                            }
                            else
                                FilterString = "";
                        }
                        //else if (txt_Search.Text == "차주아이디")
                        //{
                        //    FilterString = string.Format("DriverLoginId Like  '%{0}%'", txt_Search.Text);
                        //}
                        //else if (txt_Search.Text == "차량번호")
                        //{
                        //    FilterString = string.Format("DriverCarNo Like  '%{0}%'", txt_Search.Text);
                        //}
                        //else if (txt_Search.Text == "화주사업자번호")
                        //{
                        //    FilterString = string.Format("CustomerBizNo Like  '%{0}%'", txt_Search.Text);
                        //}
                        //else if (txt_Search.Text == "화주상호")
                        //{
                        //    FilterString = string.Format("CustomerName Like  '%{0}%'", txt_Search.Text);
                        //}
                        else if (txt_Search.Text == "거래번호")
                        {
                            FilterString = string.Format("LGD_OID Like  '%{0}%'", txt_Search.Text);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(FilterString))
                    Filters.Add(FilterString);

                var PayStateFilterString = "";
                if (cmb_PayState.Text == "결제")
                {
                    PayStateFilterString = "PayState = 1";
                }
                else if (cmb_PayState.Text == "미결제")
                {
                    PayStateFilterString = "PayState = 2";
                }

                if (!String.IsNullOrEmpty(PayStateFilterString))
                    Filters.Add(PayStateFilterString);

                var AllowAccFilterString = "";
                //if (cmb_AllowAcc.Text == "가입")
                //{
                //    AllowAccFilterString = "AllowAcc = 1";
                //}
                //else if (cmb_AllowAcc.Text == "미가입")
                //{
                //    AllowAccFilterString = "AllowAcc = 0";
                //}

                if (cmb_AllowAcc.Text == "현금")
                {
                    AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') = ''";
                    // AllowAccFilterString = "HasAcc = 1";
                }
                else if (cmb_AllowAcc.Text == "카드")
                {
                    AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') <> ''";
                    // AllowAccFilterString = "HasAcc = 0";
                }


                if (!String.IsNullOrEmpty(AllowAccFilterString))
                    Filters.Add(AllowAccFilterString);

                var HasAccFilterString = "";
                if (cmb_HasAcc_I.Text == "신청")
                {
                    HasAccFilterString = "HasAcc = 1";
                }
                else if (cmb_HasAcc_I.Text == "미신청")
                {
                    HasAccFilterString = "HasAcc = 0";
                }

                if (!String.IsNullOrEmpty(HasAccFilterString))
                    Filters.Add(HasAccFilterString);

                var LGD_Last_FunctionFilterString = "";
                if (cmb_LGD_Last_Function.Text == "승인")
                {
                    LGD_Last_FunctionFilterString = "LGD_Last_Function = '승인'";
                }
                else if (cmb_LGD_Last_Function.Text == "취소")
                {
                    LGD_Last_FunctionFilterString = "LGD_Last_Function = '취소'";
                }
                else if (cmb_LGD_Last_Function.Text == "미결")
                {
                    LGD_Last_FunctionFilterString = "ISNULL(LGD_Last_Function, '') = ''";
                }

                if (!String.IsNullOrEmpty(LGD_Last_FunctionFilterString))
                    Filters.Add(LGD_Last_FunctionFilterString);

                //var IssueDate1FilterString = "";

                //IssueDate1FilterString = string.Format("IssueDate1 <>  '{0}'", "NULL");

                //Filters.Add(IssueDate1FilterString);


                var ClientFilterString = "";
                if (!String.IsNullOrEmpty(txt_ClientSearch.Text))
                {
                    ClientFilterString = "";
                }
                else
                {
                    if (cmb_ClientSerach.Text == "운송사코드")
                    {
                        ClientFilterString = string.Format("ClientCode Like  '%{0}%'", txt_ClientSearch.Text);
                    }
                    else if (cmb_ClientSerach.Text == "운송사명")
                    {
                        ClientFilterString = string.Format("ClientName Like  '%{0}%'", txt_ClientSearch.Text);
                    }
                    else
                    {
                        ClientFilterString = "";
                    }
                }



                if (!String.IsNullOrEmpty(ClientFilterString))
                    Filters.Add(ClientFilterString);


                var MIDFilterString = "";

                if (cmb_Search.Text == "가맹점-ID")
                {
                    MIDFilterString = string.Format("MID LIKE  '%{0}%'", dtp_Search.Text);
                }

                if (!String.IsNullOrEmpty(MIDFilterString))
                    Filters.Add(MIDFilterString);

                //var SUMYNFilterString = "";

                //    SUMYNFilterString = string.Format("SUMYN <>  '{0}'",2);


                //if (!String.IsNullOrEmpty(SUMYNFilterString))
                //    Filters.Add(SUMYNFilterString);

                if (Filters.Count == 0)
                    salesManage2BindingSource.Filter = "";
                else
                    salesManage2BindingSource.Filter = String.Join(" AND ", Filters);
            }
            catch
            {
            }
        
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_PayState.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            dtp_From.Value = DateTime.Now.AddMonths(-1);
            dtp_To.Value = DateTime.Now;
            txt_Search.Clear();

            cmb_ClientSerach.SelectedIndex = 0;
            txt_ClientSearch.Clear();
            cmb_Date.SelectedIndex = 0;
            cmb_HasAcc_I.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }

      
        private void UpdateDB()
        {
            Int64 sPrice = 0;
            Int64 Price = 0;
            try
            {

                acceptInfoesBindingSource.EndEdit();
                acceptInfoesTableAdapter.Update(cMDataSet.AcceptInfoes);

                #region 업데이트
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    var Command1 = cn.CreateCommand();
                    Command1.CommandText = "SELECT convert(bigint,Sum(Price))  FROM AcceptInfoes where Tradeid = '" + intTradeIds + "'";
                    var o1 = Command1.ExecuteScalar();
                    if (o1 != null)
                    {
                        sPrice = Int64.Parse(o1.ToString());
                    }


                    cn.Close();



                    Price = sPrice;
                    Int64 VAT = Price / 10;
                    Int64 Amount = Price + VAT;


                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();



                    cmd.CommandText =
                       "UPDATE Trades SET Price = @Price, VAT = @VAT,Amount = @Amount WHERE Tradeid = @Tradeid";



                    cmd.Parameters.AddWithValue("@Tradeid", intTradeIds);
                    cmd.Parameters.AddWithValue("@Price", Price);
                    cmd.Parameters.AddWithValue("@VAT", VAT);
                    cmd.Parameters.AddWithValue("@Amount", Amount);
                    cmd.ExecuteNonQuery();
                    cn.Close();


                }
                #endregion
            }
            catch (DBConcurrencyException)
            {
                MessageBox.Show("다른 사용자에 의해서 해당 데이터가 변경되었습니다. 작업이 취소됩니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "매입정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void dtp_PayDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chk_PayState_CheckedChanged(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)salesManage2BindingSource.Current).Row as CMDataSet.SalesManage2Row;
             if (Selected != null)
             {
                 if (Selected.PayState != 1)
                 {

                     dtp_PayDate.Value = DateTime.Now;
                    
                 }
             }

             if (chk_PayState.Checked)
             {

                 lbl_PayDate.Visible = true;
                 dtp_PayDate.Visible = true;

             }
             else
             {
                 lbl_PayDate.Visible = false;
                 dtp_PayDate.Visible = false;
             }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

          
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.SalesManage2Row;

            if (dataGridView1.Columns[e.ColumnIndex] == salesIdDataGridViewTextBoxColumn)
            {
                e.Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }

            if (dataGridView1.Columns[e.ColumnIndex] == hasACCDataGridViewCheckBoxColumn)
            {
                if (Selected.HasACC)
                {
                    e.Value = "카드";
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
                else
                {
                    e.Value = "현금";
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }
            if (dataGridView1.Columns[e.ColumnIndex] == requestDateDataGridViewTextBoxColumn1)
            {
                try
                {

                    e.Value = Selected.RequestDate.ToString("yyyy-MM-dd");

                }
                catch (Exception)
                {
                    e.Value = "";
                }
            }

            if (dataGridView1.Columns[e.ColumnIndex] == lGDLastDateDataGridViewTextBoxColumn)
            {
                try
                {
                    if (Selected.LGD_Last_Function != "" && Selected.PayState == 1)
                    {
                        e.Value = Selected.LGD_Last_Date.ToString("yyyy-MM-dd");
                    }
                    else if (Selected.PayState == 1 && Selected.LGD_Last_Function == "")
                    {
                        e.Value = Selected.PayDate.ToString("yyyy-MM-dd");
                    }
                }
                catch (Exception)
                {
                    e.Value = "";
                }
            }


            if (dataGridView1.Columns[e.ColumnIndex] == Paygubun2)
            {
                try
                {
                    if (Selected.PayState == 1 && Selected.LGD_Last_Function == "")
                    {
                        e.Value = "현금";

                    }
                    else if (Selected.PayState == 1 && Selected.LGD_Last_Function != "")
                    {
                        e.Value = "카드";
                    }
                    else if (Selected.PayState == 2)
                    {
                        e.Value = "";
                    }

                }
                catch (Exception)
                {
                    e.Value = "";
                }
            }


            if (dataGridView1.Columns[e.ColumnIndex] == priceDataGridViewTextBoxColumn1)
            {
                e.Value = Selected.Price.ToString("N0");
            }

            if (dataGridView1.Columns[e.ColumnIndex] == vatDataGridViewTextBoxColumn1)
            {
                e.Value = Selected.Vat.ToString("N0");
            }
            if (dataGridView1.Columns[e.ColumnIndex] == Amount)
            {
                e.Value = Selected.Amount.ToString("N0");
            }

            if (dataGridView1.Columns[e.ColumnIndex] == PayAccountNo)
            {
                string tempString = string.Empty;
                tempString = Selected.PayAccountNo;
                try
                {
                    byte[] data = Convert.FromBase64String(tempString);


                    e.Value = m_crypt.Decrypt(tempString).Replace("\0", "");
                }
                catch
                {
                    e.Value = tempString;
                }
            }
            //if (dataGridView1.Columns[e.ColumnIndex] == LGD_MID2)
            //{
            //    var Query = cMDataSet.Clients.Where(c => c.ClientId == Selected.ClientId).ToArray();
            //    if(Query.Any())
            //    {
            //        e.Value = Query.First().LGD_MID;
            //    }
            //}

            //}
            if (dataGridView1.Columns[e.ColumnIndex] == bizNoDataGridViewTextBoxColumn1)
            {
               // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.SalesManageRow;

                var Query = cMDataSet.Clients.Where(c => c.ClientId == Selected.ClientId).ToArray();

                if (Query.Any())
                {
                    e.Value = Query.First().BizNo;
                }

            }
            if (dataGridView1.Columns[e.ColumnIndex] == sangHoDataGridViewTextBoxColumn)
            {
               // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.SalesManageRow;

                var Query = cMDataSet.Clients.Where(c => c.ClientId == Selected.ClientId).ToArray();

                if (Query.Any())
                {
                    e.Value = Query.First().Name;
                }

            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.SalesManage2Row;
                if (Selected != null)
                {
                    if (Selected.IssueDate1 == "NULL")
                    {
                        if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                        {
                            SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                            e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                            cellBackground.Dispose();
                        }
                        e.Handled = true;
                    }
                }
            }
           
        }

        private void LoadTable()
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                if (cmb_Date.SelectedIndex == 0)
                {
                    this.SalesManage2TableAdapter.FillByRequestDate(this.cMDataSet.SalesManage2, LocalUser.Instance.LogInInformation.ClientId, dtp_From.Value.Date, dtp_To.Value.Date.AddDays(1));
                }
                else
                {
                    this.SalesManage2TableAdapter.FillByPayDate(this.cMDataSet.SalesManage2, LocalUser.Instance.LogInInformation.ClientId, dtp_From.Value.Date, dtp_To.Value.Date.AddDays(1));
                }
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (dataGridView1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                {

                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }

                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "세금계산", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                //foreach (DataGridViewRow row in deleteRows)
                //{

                //    dataGridView1.Rows.Remove(row);
                //}


                salesManage2BindingSource.EndEdit();

                if (salesManage2BindingSource.Current != null)
                {
                    var Selected = ((DataRowView)salesManage2BindingSource.Current).Row as CMDataSet.SalesManage2Row;



                    if (Selected != null)
                    {

                        salesManage2BindingSource.EndEdit();
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                "Delete SalesManage  WHERE SalesId = @SalesId";

                            cmd.Parameters.AddWithValue("@SalesId", Selected.SalesId);
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                }

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "세금계산서", 1), "세금계산서 삭제 성공");

                btn_Search_Click(null, null);
            }




        }

        private void txt_ClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmTrade_Client_Add _Form = new FrmTrade_Client_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void driversInfoBindingSource_CurrentChanged(object sender, EventArgs e)
        {
           
        }

        private void txt_Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Price_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Price.Text))
            {

                if (chk_Vat.Checked == true)
                {
                    txt_VAT.Text = Math.Truncate((Int64.Parse(txt_Price.Text) * 0.1)).ToString();
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }
                else
                {
                    txt_VAT.Text = "0";
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }


            }
        }

        private void chk_Vat_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Price.Text))
            {

                if (chk_Vat.Checked == true)
                {
                    txt_VAT.Text = Math.Truncate((Int64.Parse(txt_Price.Text) * 0.1)).ToString();
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }
                else
                {
                    txt_VAT.Text = "0";
                    txt_Amount.Text = (Int64.Parse(txt_Price.Text) + Int64.Parse(txt_VAT.Text)).ToString();
                }


            }
        }

       bool overhead = false;
        List<string> checkedCodes = new List<string>();



        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {

            overhead = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1[issueDate1DataGridViewTextBoxColumn.Index, i].Value.ToString() != "")
                {
                    dataGridView1[CheckBox.Index, i].Value = chkAllSelect.Checked;
                }
            }
            overhead = false;
            dataGridView1_CellValueChanged(null, null);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null)
                return;

            if (e.RowIndex < 0)
                return;


            try
            {

                if (e.ColumnIndex == 0)
                {
                    object o = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                    string code = ((DataRowView)salesManage2BindingSource[e.RowIndex])["SalesId"].ToString();
                    if (Convert.ToBoolean(o))
                    {
                        if (!checkedCodes.Contains(code))
                            checkedCodes.Add(code);
                    }
                    else
                    {
                        if (checkedCodes.Contains(code))
                            checkedCodes.Remove(code);
                    }
                }
            }
            catch
            {

            }
            if (overhead) return;
        }

        private string getFilterString()
        {
            string r = "'0'";
            if (checkedCodes.Count > 0)
            {
                r = String.Join(",", checkedCodes.Select(c => "'" + c + "'").ToArray());
            }
            return r;
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect1 = dataGridView1.GetColumnDisplayRectangle(CheckBox.Index, true);
            chkAllSelect.Location = new Point(rect1.Location.X + 2, rect1.Location.Y + 4);
            if (rect1.Width == 0) chkAllSelect.Visible = false;
            else chkAllSelect.Visible = true;
        }

        private void btnExcelDown_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btn_AcceptAcc_Click(object sender, EventArgs e)
        {
                var Datas = cMDataSet.SalesManage2.Where(c => c.CheckBox);
                if (Datas.Any())
                {
                    FrmLogin_A f = new FrmLogin_A();
                    f.ShowDialog();
                    if (f.Accepted)
                    {
                        Dialog_AcceptAcc_C d = new Dialog_AcceptAcc_C();
                        d.Sales = Datas.ToArray();
                        d.AuthKey = f.AuthKey;
                        d.ShowDialog();
                        btn_Search_Click(null, null);
                    }
                }
                else
                {
                    MessageBox.Show("먼저 결제할 항목들을 선택하여 주십시오.");
                }
        }

        private void btn_CancelAcc_Click(object sender, EventArgs e)
        {

            //var Datas = cMDataSet.SalesManage2.Where(c => c.CheckBox);
            //if (Datas.Any())
            //{
            //    FrmLogin_A f = new FrmLogin_A();
            //    f.ShowDialog();
            //    if (f.Accepted)
            //    {
            //        Dialog_CancelAcc_C d = new Dialog_CancelAcc_C();
            //        d.sa = Datas.ToArray();
            //        d.AuthKey = f.AuthKey;
            //        d.ShowDialog();
            //        btn_Search_Click(null, null);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("먼저 취소할 항목들을 선택하여 주십시오.");
            //}

        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Search.Text == "승인일" || cmb_Search.Text == "취소일")
            {
                dtp_Search.Visible = true;
                txt_Search.Visible = false;
            }
            else
            {
                dtp_Search.Visible = false;
                txt_Search.Visible = true;
            }
        }

        private void tradesBindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
        {

        }

        private void newDGV1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateDB();

            if (dataGridView1.RowCount > 1)
            {
                GridIndex = salesManage2BindingSource.Position;
                btn_Search_Click(null, null);


                dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
            }
            else
            {
                btn_Search_Click(null, null);
            }
        }

   

        private void newDGV2Binding(int iTradeID)
        {
          //  var Selected = ((DataRowView)dataGridView1.SelectedRows[0].DataBoundItem).Row as CMDataSet.TradesRow;

            DataTable mDataTable = new DataTable();

            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT AccLogId, AccFunction, AccLogs.BankName, CardNo, AccLogs.TradeId, AccLogs.CreateDate, LGD_TID, clients2.LGD_MID as MID,AccLogs.LGD_OID, LGD_AMOUNT, LGD_RESPCODE, LGD_RESPMSG,Clients.LGD_MID FROM AccLogs join SalesManage on AccLogs.SalesId = SalesManage.salesid join Clients on SalesManage.CustomerId = Clients.ClientId join Clients as clients2 on SalesManage.ClientId = Clients2.ClientId Where AccLogs.SalesId = @TradeId Order by AccLogs.CreateDate DESC";
                Command.Parameters.AddWithValue("@TradeId", iTradeID);
                Connection.Open();
                mDataTable.Load(Command.ExecuteReader());
                Connection.Close();
            }

            newDGV2.AutoGenerateColumns = false;
            newDGV2.DataSource = mDataTable;

        }

      

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (newDGV2.RowCount - e.RowIndex).ToString("N0");
            }
        }
     
        private void TaxSumSearch()
        {
            var Filters = new List<string>();

            var FilterString = "";
            if (cmb_Search.Text == "승인일")
            {
                FilterString = string.Format("LGD_Last_Function = '승인' AND LGD_Last_Date =  '{0}'", dtp_Search.Text);
            }
            else if (cmb_Search.Text == "취소일")
            {
                FilterString = string.Format("LGD_Last_Function = '취소' AND LGD_Last_Date =  '{0}'", dtp_Search.Text);
            }
            else
            {
                if (String.IsNullOrEmpty(txt_Search.Text))
                {
                    FilterString = "";
                }
                else
                {
                    if (cmb_Search.Text == "청구항목")
                    {
                        FilterString = string.Format("Item Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "계좌번호")
                    {
                        FilterString = string.Format("PayAccountNo Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "예금주")
                    {
                        FilterString = string.Format("PayInputName Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "차주사업자번호")
                    {
                        FilterString = string.Format("DriverBizNo Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "차주상호명")
                    {
                        FilterString = string.Format("DriverName Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "차주아이디")
                    {
                        FilterString = string.Format("DriverLoginId Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "차량번호")
                    {
                        FilterString = string.Format("DriverCarNo Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "화주사업자번호")
                    {
                        FilterString = string.Format("CustomerBizNo Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "화주상호")
                    {
                        FilterString = string.Format("CustomerName Like  '%{0}%'", txt_Search.Text);
                    }
                    else if (cmb_Search.Text == "거래번호")
                    {
                        FilterString = string.Format("LGD_OID Like  '%{0}%'", txt_Search.Text);
                    }
                }
            }

            if (!String.IsNullOrEmpty(FilterString))
                Filters.Add(FilterString);

            var PayStateFilterString = "";
            if (cmb_PayState.Text == "결제")
            {
                PayStateFilterString = "PayState = 1";
            }
            else if (cmb_PayState.Text == "미결제")
            {
                PayStateFilterString = "PayState = 2";
            }

            if (!String.IsNullOrEmpty(PayStateFilterString))
                Filters.Add(PayStateFilterString);

            var AllowAccFilterString = "";
            //if (cmb_AllowAcc.Text == "가입")
            //{
            //    AllowAccFilterString = "AllowAcc = 1";
            //}
            //else if (cmb_AllowAcc.Text == "미가입")
            //{
            //    AllowAccFilterString = "AllowAcc = 0";
            //}

            if (cmb_AllowAcc.Text == "현금")
            {
                AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') = ''";
                // AllowAccFilterString = "HasAcc = 1";
            }
            else if (cmb_AllowAcc.Text == "카드")
            {
                AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') <> ''";
                // AllowAccFilterString = "HasAcc = 0";
            }


            if (!String.IsNullOrEmpty(AllowAccFilterString))
                Filters.Add(AllowAccFilterString);

            var HasAccFilterString = "";
            if (cmb_HasAcc_I.Text == "신청")
            {
                HasAccFilterString = "HasAcc = 1";
            }
            else if (cmb_HasAcc_I.Text == "미신청")
            {
                HasAccFilterString = "HasAcc = 0";
            }

            if (!String.IsNullOrEmpty(HasAccFilterString))
                Filters.Add(HasAccFilterString);

            var LGD_Last_FunctionFilterString = "";
            if (cmb_LGD_Last_Function.Text == "승인")
            {
                LGD_Last_FunctionFilterString = "LGD_Last_Function = '승인'";
            }
            else if (cmb_LGD_Last_Function.Text == "취소")
            {
                LGD_Last_FunctionFilterString = "LGD_Last_Function = '취소'";
            }
            else if (cmb_LGD_Last_Function.Text == "미결")
            {
                LGD_Last_FunctionFilterString = "ISNULL(LGD_Last_Function, '') = ''";
            }

            if (!String.IsNullOrEmpty(LGD_Last_FunctionFilterString))
                Filters.Add(LGD_Last_FunctionFilterString);




            var ClientFilterString = "";
            if (!String.IsNullOrEmpty(txt_ClientSearch.Text))
            {
                ClientFilterString = "";
            }
            else
            {
                if (cmb_ClientSerach.Text == "운송사코드")
                {
                    ClientFilterString = string.Format("ClientCode Like  '%{0}%'", txt_ClientSearch.Text);
                }
                else if (cmb_ClientSerach.Text == "운송사명")
                {
                    ClientFilterString = string.Format("ClientName Like  '%{0}%'", txt_ClientSearch.Text);
                }
                else
                {
                    ClientFilterString = "";
                }
            }



            if (!String.IsNullOrEmpty(ClientFilterString))
                Filters.Add(ClientFilterString);


            var MIDFilterString = "";

            if (cmb_Search.Text == "가맹점-ID")
            {
                MIDFilterString = string.Format("MID LIKE  '%{0}%'", dtp_Search.Text);
            }

            if (!String.IsNullOrEmpty(MIDFilterString))
                Filters.Add(MIDFilterString);

            //var SUMYNFilterString = "";

            //    SUMYNFilterString = string.Format("SUMYN <>  '{0}'",2);


            //if (!String.IsNullOrEmpty(SUMYNFilterString))
            //    Filters.Add(SUMYNFilterString);

            if (Filters.Count == 0)
                salesManage2BindingSource.Filter = "";
            else
                salesManage2BindingSource.Filter = String.Join(" AND ", Filters);
        }
        private void btn_TaxSum_Click(object sender, EventArgs e)
        {   //SUMYN 
            // 0 =합치기전
            // 1 = 합치기
            //2 = 합치고 나머지

            FrmTradeSum _frmTradeSum = new FrmTradeSum(dtp_From.Text, dtp_To.Text);

            List<int> SgubunList = new List<int>();
            _frmTradeSum.Owner = this;
            _frmTradeSum.StartPosition = FormStartPosition.CenterParent;
            if (_frmTradeSum.ShowDialog() == DialogResult.OK)
                    {

                        int Sgubun = _frmTradeSum.cmb_SumGubun.SelectedIndex;

                        if (Sgubun == 0)
                        {
                            SgubunList.Add(1);
                            SgubunList.Add(2);
                        }
                        else
                        {
                            SgubunList.Add(Sgubun);
                        }

                        //if (cmb_AllowAcc.Text == "현금")
                        //{
                        //    AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') = ''";
                        //}
                        //else if (cmb_AllowAcc.Text == "카드")
                        //{
                        //    AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') <> ''";
                        //}

                        var Filters = new List<string>();

                        var FilterString = "";
                        if (cmb_Search.Text == "승인일")
                        {
                            FilterString = string.Format("LGD_Last_Function = '승인' AND LGD_Last_Date =  '{0}'", dtp_Search.Text);
                        }
                        else if (cmb_Search.Text == "취소일")
                        {
                            FilterString = string.Format("LGD_Last_Function = '취소' AND LGD_Last_Date =  '{0}'", dtp_Search.Text);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(txt_Search.Text))
                            {
                                FilterString = "";
                            }
                            else
                            {
                                if (cmb_Search.Text == "청구항목")
                                {
                                    FilterString = string.Format("Item Like  '%{0}%'", txt_Search.Text);
                                }
                                else if (cmb_Search.Text == "계좌번호")
                                {
                                    FilterString = string.Format("PayAccountNo Like  '%{0}%'", txt_Search.Text);
                                }
                                else if (cmb_Search.Text == "예금주")
                                {
                                    FilterString = string.Format("PayInputName Like  '%{0}%'", txt_Search.Text);
                                }
                                else if (cmb_Search.Text == "차주사업자번호")
                                {
                                  //  FilterString = string.Format("DriverBizNo Like  '%{0}%'", txt_Search.Text);

                                    var codes = cMDataSet.Drivers.Where(c => c.BizNo.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                                    if (codes.Count() > 0)
                                    {
                                        string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                                        for (int i = 1; i < codes.Count(); i++)
                                        {
                                            filter += string.Format(", '{0}'", codes[i]);
                                        }
                                        filter += ")";
                                        FilterString = filter;
                                    }
                                    else
                                        FilterString = "";


                                }
                                else if (cmb_Search.Text == "차주상호명")
                                {
                                  //  FilterString = string.Format("DriverName Like  '%{0}%'", txt_Search.Text);
                                    var codes = cMDataSet.Drivers.Where(c => c.Name.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                                    if (codes.Count() > 0)
                                    {
                                        string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                                        for (int i = 1; i < codes.Count(); i++)
                                        {
                                            filter += string.Format(", '{0}'", codes[i]);
                                        }
                                        filter += ")";
                                        FilterString = filter;
                                    }
                                    else
                                        FilterString = "";
                                }
                                else if (cmb_Search.Text == "차주아이디")
                                {
                                    //ilterString = string.Format("DriverLoginId Like  '%{0}%'", txt_Search.Text);
                                    var codes = cMDataSet.Drivers.Where(c => c.LoginId.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                                    if (codes.Count() > 0)
                                    {
                                        string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                                        for (int i = 1; i < codes.Count(); i++)
                                        {
                                            filter += string.Format(", '{0}'", codes[i]);
                                        }
                                        filter += ")";
                                        FilterString = filter;
                                    }
                                    else
                                        FilterString = "";
                                }
                                else if (cmb_Search.Text == "차량번호")
                                {
                                    //FilterString = string.Format("DriverCarNo Like  '%{0}%'", txt_Search.Text);
                                    var codes = cMDataSet.Drivers.Where(c => c.CarNo.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                                    if (codes.Count() > 0)
                                    {
                                        string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                                        for (int i = 1; i < codes.Count(); i++)
                                        {
                                            filter += string.Format(", '{0}'", codes[i]);
                                        }
                                        filter += ")";
                                        FilterString = filter;
                                    }
                                    else
                                        FilterString = "";
                                }
                                else if (cmb_Search.Text == "화주사업자번호")
                                {
                                    //FilterString = string.Format("CustomerBizNo Like  '%{0}%'", txt_Search.Text);
                                    var codes = cMDataSet.Clients.Where(c => c.BizNo.Contains(txt_Search.Text)).Select(c => c.ClientId).ToArray();
                                    if (codes.Count() > 0)
                                    {
                                        string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                                        for (int i = 1; i < codes.Count(); i++)
                                        {
                                            filter += string.Format(", '{0}'", codes[i]);
                                        }
                                        filter += ")";
                                        FilterString = filter;
                                    }
                                    else
                                        FilterString = "";
                                }
                                else if (cmb_Search.Text == "화주상호")
                                {
                                   // FilterString = string.Format("CustomerName Like  '%{0}%'", txt_Search.Text);
                                    var codes = cMDataSet.Clients.Where(c => c.Name.Contains(txt_Search.Text)).Select(c => c.ClientId).ToArray();
                                    if (codes.Count() > 0)
                                    {
                                        string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                                        for (int i = 1; i < codes.Count(); i++)
                                        {
                                            filter += string.Format(", '{0}'", codes[i]);
                                        }
                                        filter += ")";
                                        FilterString = filter;
                                    }
                                    else
                                        FilterString = "";
                                }
                                else if (cmb_Search.Text == "거래번호")
                                {
                                    FilterString = string.Format("LGD_OID Like  '%{0}%'", txt_Search.Text);
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(FilterString))
                            Filters.Add(FilterString);

                        var PayStateFilterString = "";
                        if (cmb_PayState.Text == "결제")
                        {
                            PayStateFilterString = "PayState = 1";
                        }
                        else if (cmb_PayState.Text == "미결제")
                        {
                            PayStateFilterString = "PayState = 2";
                        }

                        if (!String.IsNullOrEmpty(PayStateFilterString))
                            Filters.Add(PayStateFilterString);

                        var AllowAccFilterString = "";
                        //if (cmb_AllowAcc.Text == "가입")
                        //{
                        //    AllowAccFilterString = "AllowAcc = 1";
                        //}
                        //else if (cmb_AllowAcc.Text == "미가입")
                        //{
                        //    AllowAccFilterString = "AllowAcc = 0";
                        //}

                        if (cmb_AllowAcc.Text == "현금")
                        {
                            AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') = ''";
                            // AllowAccFilterString = "HasAcc = 1";
                        }
                        else if (cmb_AllowAcc.Text == "카드")
                        {
                            AllowAccFilterString = "PayState = 1 AND isnull(LGD_Last_Function,'') <> ''";
                            // AllowAccFilterString = "HasAcc = 0";
                        }


                        if (!String.IsNullOrEmpty(AllowAccFilterString))
                            Filters.Add(AllowAccFilterString);

                        var HasAccFilterString = "";
                        if (cmb_HasAcc_I.Text == "신청")
                        {
                            HasAccFilterString = "HasAcc = 1";
                        }
                        else if (cmb_HasAcc_I.Text == "미신청")
                        {
                            HasAccFilterString = "HasAcc = 0";
                        }

                        if (!String.IsNullOrEmpty(HasAccFilterString))
                            Filters.Add(HasAccFilterString);

                        var LGD_Last_FunctionFilterString = "";
                        if (cmb_LGD_Last_Function.Text == "승인")
                        {
                            LGD_Last_FunctionFilterString = "LGD_Last_Function = '승인'";
                        }
                        else if (cmb_LGD_Last_Function.Text == "취소")
                        {
                            LGD_Last_FunctionFilterString = "LGD_Last_Function = '취소'";
                        }
                        else if (cmb_LGD_Last_Function.Text == "미결")
                        {
                            LGD_Last_FunctionFilterString = "ISNULL(LGD_Last_Function, '') = ''";
                        }

                        if (!String.IsNullOrEmpty(LGD_Last_FunctionFilterString))
                            Filters.Add(LGD_Last_FunctionFilterString);




                        var ClientFilterString = "";
                        if (!String.IsNullOrEmpty(txt_ClientSearch.Text))
                        {
                            ClientFilterString = "";
                        }
                        else
                        {
                            if (cmb_ClientSerach.Text == "운송사코드")
                            {
                                //ClientFilterString = string.Format("ClientCode Like  '%{0}%'", txt_ClientSearch.Text);
                                var codes = cMDataSet.Clients.Where(c => c.Code.Contains(txt_Search.Text)).Select(c => c.ClientId).ToArray();
                                if (codes.Count() > 0)
                                {
                                    string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                                    for (int i = 1; i < codes.Count(); i++)
                                    {
                                        filter += string.Format(", '{0}'", codes[i]);
                                    }
                                    filter += ")";
                                    ClientFilterString = filter;
                                }
                                else
                                    ClientFilterString = "";
                            }
                            else if (cmb_ClientSerach.Text == "운송사명")
                            {
                                //ClientFilterString = string.Format("ClientName Like  '%{0}%'", txt_ClientSearch.Text);
                                var codes = cMDataSet.Clients.Where(c => c.Name.Contains(txt_Search.Text)).Select(c => c.ClientId).ToArray();
                                if (codes.Count() > 0)
                                {
                                    string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                                    for (int i = 1; i < codes.Count(); i++)
                                    {
                                        filter += string.Format(", '{0}'", codes[i]);
                                    }
                                    ClientFilterString += ")";
                                    FilterString = filter;
                                }
                                else
                                    ClientFilterString = "";
                            }
                            else
                            {
                                ClientFilterString = "";
                            }
                        }



                        if (!String.IsNullOrEmpty(ClientFilterString))
                            Filters.Add(ClientFilterString);


                        var MIDFilterString = "";

                        if (cmb_Search.Text == "가맹점-ID")
                        {
                           // MIDFilterString = string.Format("MID LIKE  '%{0}%'", dtp_Search.Text);

                            var codes = cMDataSet.Drivers.Where(c => c.MID.Contains(txt_Search.Text)).Select(c => c.DriverId).ToArray();
                            if (codes.Count() > 0)
                            {
                                string filter = String.Format("ClientId IN ('{0}'", codes[0]);
                                for (int i = 1; i < codes.Count(); i++)
                                {
                                    filter += string.Format(", '{0}'", codes[i]);
                                }
                                filter += ")";
                                MIDFilterString = filter;
                            }
                            else
                                MIDFilterString = "";
                        }

                        if (!String.IsNullOrEmpty(MIDFilterString))
                        {
                            Filters.Add(MIDFilterString);
                        }

                        if (Filters.Count > 0)
                        {
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {

                                if (cmb_Date.SelectedIndex == 0)
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                       " UPDATE T  " +
                                       " SET T.SumAmount = B.Amount " +
                                       " ,t.SumVAT = b.VAT " +
                                       " ,T.SumPrice = b.price" +
                                       " ,t.SUMYN = 1" +
                                       " ,T.SUMFROMDate = b.FromDate " +
                                       " ,T.SUMToDate = b.ToDate " +
                                       " FROM " +
                                       " trades as T join (select MAX(TradeId) as TradeId,sum(CONVERT(bigint,ROUND(price,0))) as price,sum(CONVERT(bigint,ROUND(VAT,0))) VAT,sum(CONVERT(bigint,ROUND(Amount,0))) Amount" +
                                       ",driverid as driverid,UseTax,convert(varchar,min(RequestDate),111) as FromDate,convert(varchar,max(RequestDate),111) as ToDate from Trades " +
                                       " where RequestDate >= @dtp_From  AND RequestDate <= @dtp_To " +
                                       " and clientid = @ClientId " +
                                       " and PayState IN " + "(" + String.Join(",", SgubunList) + ") " +
                                       " AND SUMYN = 0 "+
                                       " AND " + String.Join(" AND ", Filters) +

                                       " group by DriverId,UseTax) AS B " +
                                       " on T.TradeId = b.tradeid " +
                                       " where T.RequestDate >= @dtp_From  AND T.RequestDate <= @dtp_To " +
                                       " and T.clientid = @ClientId " +
                                       " AND T.SUMYN = 0";

                                    cmd.Parameters.AddWithValue("@dtp_From", dtp_From.Text);
                                    cmd.Parameters.AddWithValue("@dtp_To", dtp_To.Text);
                                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                    // cmd.Parameters.AddWithValue("@PayState", Sgubun);


                                    cmd.ExecuteNonQuery();
                                    cn.Close();
                                }
                                else
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                       " UPDATE T  " +
                                       " SET T.SumAmount = B.Amount " +
                                       " ,t.SumVAT = b.VAT " +
                                       " ,T.SumPrice = b.price" +
                                       " ,t.SUMYN = 1" +
                                       " ,T.SUMFROMDate = b.FromDate " +
                                       " ,T.SUMToDate = b.ToDate " +
                                       " FROM " +
                                       " trades as T join (select MAX(TradeId) as TradeId,sum(CONVERT(bigint,ROUND(price,0))) as price,sum(CONVERT(bigint,ROUND(VAT,0))) VAT,sum(CONVERT(bigint,ROUND(Amount,0))) Amount" +
                                       ",driverid as driverid,UseTax,convert(varchar,min(RequestDate),111) as FromDate,convert(varchar,max(RequestDate),111) as ToDate from Trades " +
                                       " where PayDate >= @dtp_From  AND PayDate <= @dtp_To " +
                                       " and clientid = @ClientId " +
                                      " and PayState IN " + "(" + String.Join(",", SgubunList) + ") " +
                                      " AND SUMYN = 0 " +
                                       " AND " + String.Join(" AND ", Filters) +
                                       " group by DriverId,UseTax) AS B " +
                                       " on T.TradeId = b.tradeid " +
                                       " where T.PayDate >= @dtp_From  AND T.PayDate <= @dtp_To " +
                                       " and T.clientid = @ClientId " +
                                        " AND T.SUMYN = 0";

                                    cmd.Parameters.AddWithValue("@dtp_From", dtp_From.Text);
                                    cmd.Parameters.AddWithValue("@dtp_To", dtp_To.Text);
                                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);



                                    cmd.ExecuteNonQuery();
                                    cn.Close();
                                }
                            }

                        }
                        else
                        {
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {

                                if (cmb_Date.SelectedIndex == 0)
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                       " UPDATE T  " +
                                       " SET T.SumAmount = B.Amount " +
                                       " ,t.SumVAT = b.VAT " +
                                       " ,T.SumPrice = b.price" +
                                       " ,t.SUMYN = 1" +
                                       " ,T.SUMFROMDate = b.FromDate " +
                                       " ,T.SUMToDate = b.ToDate " +
                                       " FROM " +
                                       " trades as T join (select MAX(TradeId) as TradeId,sum(CONVERT(bigint,ROUND(price,0))) as price,sum(CONVERT(bigint,ROUND(VAT,0))) VAT,sum(CONVERT(bigint,ROUND(Amount,0))) Amount" +
                                       ",driverid as driverid,UseTax,convert(varchar,min(RequestDate),111) as FromDate,convert(varchar,max(RequestDate),111) as ToDate from Trades " +
                                       " where RequestDate >= @dtp_From  AND RequestDate <= @dtp_To " +
                                       " and clientid = @ClientId " +
                                        " AND SUMYN = 0" +
                                       " and PayState IN " + "(" + String.Join(",", SgubunList) + ") " +
                                        // " AND " + String.Join(" AND ", Filters) +

                                       " group by DriverId,UseTax) AS B " +
                                       " on T.TradeId = b.tradeid " +
                                       " where T.RequestDate >= @dtp_From  AND T.RequestDate <= @dtp_To " +
                                       " and T.clientid = @ClientId " +
                                       " AND T.SUMYN = 0";

                                    cmd.Parameters.AddWithValue("@dtp_From", dtp_From.Text);
                                    cmd.Parameters.AddWithValue("@dtp_To", dtp_To.Text);
                                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                    // cmd.Parameters.AddWithValue("@PayState", Sgubun);


                                    cmd.ExecuteNonQuery();
                                    cn.Close();
                                }
                                else
                                {
                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                       " UPDATE T  " +
                                       " SET T.SumAmount = B.Amount " +
                                       " ,t.SumVAT = b.VAT " +
                                       " ,T.SumPrice = b.price" +
                                       " ,t.SUMYN = 1" +
                                       " ,T.SUMFROMDate = b.FromDate " +
                                       " ,T.SUMToDate = b.ToDate " +
                                       " FROM " +
                                       " trades as T join (select MAX(TradeId) as TradeId,sum(CONVERT(bigint,ROUND(price,0))) as price,sum(CONVERT(bigint,ROUND(VAT,0))) VAT,sum(CONVERT(bigint,ROUND(Amount,0))) Amount" +
                                       ",driverid as driverid,UseTax,convert(varchar,min(RequestDate),111) as FromDate,convert(varchar,max(RequestDate),111) as ToDate from Trades " +
                                       " where PayDate >= @dtp_From  AND PayDate <= @dtp_To " +
                                       " and clientid = @ClientId " +
                                       " AND SUMYN = 0" +
                                      " and PayState IN " + "(" + String.Join(",", SgubunList) + ") " +
                                       " group by DriverId,UseTax) AS B " +
                                       " on T.TradeId = b.tradeid " +
                                       " where T.PayDate >= @dtp_From  AND T.PayDate <= @dtp_To " +
                                       " and T.clientid = @ClientId " +
                                       " AND T.SUMYN = 0";

                                    cmd.Parameters.AddWithValue("@dtp_From", dtp_From.Text);
                                    cmd.Parameters.AddWithValue("@dtp_To", dtp_To.Text);
                                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);



                                    cmd.ExecuteNonQuery();
                                    cn.Close();
                                }
                            }
                        }
                        LoadTable();


                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {



                            //var TradeId = cMDataSet.Trades.Where(c => c.RequestDate >= dtp_RequestDate.Value).Where(c => c.RequestDate <= dtp_To.Value.AddDays(1)).Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).Where(c => c.SUMYN != 1).Select(c => c.TradeId).ToArray();

                            var TradeId = dataGridView1.Rows.Cast<DataGridViewRow>().Where(c => c.Visible).Where(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.TradesRow).VAT != 0 && (((DataRowView)c.DataBoundItem).Row as CMDataSet.TradesRow).SUMYN ==0  ).Select(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.TradesRow).TradeId).ToArray();

                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            if (TradeId.Any())
                            {
                                cmd.CommandText =
                                    "UPDATE Trades SET sumyn = 2  WHERE  TradeId IN " + "(" + String.Join(",", TradeId) + ") ";


                                cmd.Parameters.AddWithValue("@dtp_From", dtp_From.Text);
                                cmd.Parameters.AddWithValue("@dtp_To", dtp_To.Text);
                                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);



                                cmd.ExecuteNonQuery();
                                cn.Close();
                            }

                        }


                
                        btn_Search_Click(null, null);
                        MessageBox.Show("전표합치기완료");


                    }
            
        }

        private void cmb_HasAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_HasAcc.SelectedIndex == 1)
            {
                chk_PayState.Enabled = false;
            }
            else
            {
                chk_PayState.Enabled = true;
            }
        }

        private void rdb_Car_CheckedChanged(object sender, EventArgs e)
        {


            FrmMDI frmMDI = new FrmMDI();




            FrmTrade _Form = new FrmTrade();
            _Form.MdiParent = this.MdiParent;

            _Form.Show();

            this.Close();
        }

        private void rdb_Client_CheckedChanged(object sender, EventArgs e)
        {
            //FrmMDI frmMDI = new FrmMDI();

            //frmMDI.Menu_Click("mnu0209", null);
        }

        private void salesManage2BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (salesManage2BindingSource.Current == null)
            {
                chk_PayState.Checked = false;
                return;
            }
            else
            {
             //   driversTableAdapter.Fill(this.cMDataSet.Drivers);
                var Selected = ((DataRowView)salesManage2BindingSource.Current).Row as CMDataSet.SalesManage2Row;
                if (Selected != null)
                {
                    if (Selected.PayState == 1)
                    {
                        chk_PayState.Checked = true;
                    }
                    else
                    {
                        chk_PayState.Checked = false;
                    }

                    label83.Text = Selected.SangHo;
                    //txt_DriverId.Text = Selected.DriverId.ToString();
                    //txt_BizNo.Text = Selected.DriverBizNo;
                    //txt_Name.Text = Selected.DriverName;
                    txt_Price.Text = ((long)Selected.Price).ToString();
                    txt_VAT.Text = ((long)Selected.Vat).ToString();
                    txt_Amount.Text = ((long)Selected.Amount).ToString();

                    var Query = cMDataSet.Clients.Where(c => c.ClientId == Selected.ClientId).ToArray();

                    if (Query.Any())
                    {
                        txt_Name.Text = Query.First().Name;
                        txt_BizNo.Text = Query.First().BizNo;
                    }
                    string tempString = string.Empty;
                    tempString = Selected.PayAccountNo;
                    try
                    {
                        byte[] data = Convert.FromBase64String(tempString);


                        txt_PayAccountNo.Text = m_crypt.Decrypt(tempString).Replace("\0", "");
                    }
                    catch
                    {
                        txt_PayAccountNo.Text = tempString;
                    }


                    //var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId);
                    //txt_DriverName.Text = Query.First().CarYear;

                    //  cmb_PayBankName.SelectedValue = Selected.PayBankCode.ToString();
                    if (Selected.HasACC == true)
                    {
                        cmb_HasAcc.SelectedIndex = 1;
                        chk_PayState.Enabled = false;
                    }
                    else
                    {
                        cmb_HasAcc.SelectedIndex = 0;
                        chk_PayState.Enabled = true;
                    }


                    tableLayoutPanel2.Enabled = true;
                    btnCurrentDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                    if (Selected.HasACC)
                    {
                        //  chk_PayState.Enabled = false;
                        if (Selected.PayState == 1)
                        {
                            tableLayoutPanel2.Enabled = false;
                            btnCurrentDelete.Enabled = false;
                            btnUpdate.Enabled = false;
                            chk_PayState.Enabled = false;
                        }
                    }
                    else
                    {
                        chk_PayState.Enabled = true;
                    }


                 //   intTradeIds = Selected.SalesId;

                //    this.acceptInfoesTableAdapter.Fill(this.cMDataSet.AcceptInfoes);

                  //  acceptInfoesBindingSource.Filter = " SalesId = '" + intTradeIds + "'";


                    newDGV2Binding(Selected.SalesId);
                }
                else
                {
                    chk_PayState.Checked = false;
                }

            }
        }

     

      
     

       

        

       
    }
}
