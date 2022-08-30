using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
namespace mycalltruck.Admin
{
    public partial class Dialog_AcceptAcc_C : Form
    {
        DESCrypt m_crypt = null;

        public Dialog_AcceptAcc_C()
        {
            InitializeComponent();
            m_crypt = new DESCrypt("12345678");
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);
            ShowStep1();
            Dictionary<string, string> CardGubun = new Dictionary<string, string>();
            CardGubun.Add("0", "개인");
            CardGubun.Add("1", "법인");



            cmb_CardGubun.DataSource = new BindingSource(CardGubun, null);
            cmb_CardGubun.DisplayMember = "Value";
            cmb_CardGubun.ValueMember = "Key";

            cmb_CardGubun.SelectedIndex = 0;




            Dictionary<string, string> Month = new Dictionary<string, string>();

            Month.Add("", "");
            Month.Add("1", "01");
            Month.Add("2", "02");
            Month.Add("3", "03");
            Month.Add("4", "04");
            Month.Add("5", "05");
            Month.Add("6", "06");
            Month.Add("7", "07");
            Month.Add("8", "08");
            Month.Add("9", "09");
            Month.Add("10", "10");
            Month.Add("11", "11");
            Month.Add("12", "12");

            //for (int i = 1; i <= 12; i++)
            //{

            //    Month.Add(i.ToString(),i.ToString());

            //}
           

            Step4CardMonth.DataSource = new BindingSource(Month, null);
            Step4CardMonth.DisplayMember = "Value";
            Step4CardMonth.ValueMember = "Key";


            Dictionary<string, string> Year = new Dictionary<string, string>();


            Year.Add("","");
            for (int i = DateTime.Now.Year; i <= DateTime.Now.AddYears(10).Year; i++)

            {

                Year.Add(i.ToString(), i.ToString());

            }

          

            Step4CardYear.DataSource = new BindingSource(Year, null);
            Step4CardYear.DisplayMember = "Value";
            Step4CardYear.ValueMember = "Key";


            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
            Step4LGD_MID.Text = Client.LGD_MID;
            PGGubun = Client.PGGubun;

          //  Step4CardMonth.SelectedIndex = DateTime.Now.Month - 1;

            
        }

        public CMDataSet.SalesManage2Row[] Sales { get; set; }
        private CMDataSet.SalesManage2Row[] AcceptSales { get; set; }

        private int CurrentStep = 1;
        private bool IsLoadClientAccs = false;
        private bool IsLoadStep2 = false;
        private bool IsLoadStep3 = false;
        private bool IsLoadStep4 = false;
        private bool NeedCheckCustomerAccLimite = false;
        private int Step5Tatal = 0;
        private int Step5Done = 0;
        private Int64 Step5TatalMoney = 0;
        private int Step5Success = 0;
        private Int64 Step5SuccessMoney = 0;
        private int Step5Failed = 0;
        private Int64 Step5FailedMoney = 0;
        private bool IsClosed = false;
        //Parameter
        public String AuthKey { get; set; }
        private int ClientAccId = 0;
        private string CardNo = "";
        private string CardDate = "";
        private string CardPan = "";
        private string UserType = "";
        private string BizNo = "";
        private int PGGubun = 0;
        private void ShowStep1()
        {
            CurrentStep = 1;

            pnStep1.Visible = true;
            pnStep2.Visible = false;
            pnStep3.Visible = false;
            pnStep4.Visible = false;
            pnStep5.Visible = false;

            LayoutRoot.RowStyles[1].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[1].Height = 100F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[4].Height = 0F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[5].Height = 0F;

            lbl_Step1.Font = new Font(lbl_Step1.Font, FontStyle.Bold);
            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Regular);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Regular);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Regular);
            lbl_Step2.ForeColor = Color.FromArgb(137, 206, 250);
            lbl_Step3.ForeColor = Color.FromArgb(137, 206, 250);
            lbl_Step4.ForeColor = Color.FromArgb(137, 206, 250);
            lbl_Step5.ForeColor = Color.FromArgb(137, 206, 250);
            bar_Step2.BackColor = Color.FromArgb(19, 124, 192);
            bar_Step3.BackColor = Color.FromArgb(19, 124, 192);
            bar_Step4.BackColor = Color.FromArgb(19, 124, 192);
            bar_Step5.BackColor = Color.FromArgb(19, 124, 192);
            btn_Pre.Enabled = false;
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            btn_ShowDenyList.Visible = false;

            //Load ClientAccs
            if(!IsLoadClientAccs)
            {
                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "Select * From ClientAccs Where ClientId = @ClientId";
                    Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    Connection.Open();
                    var DataReader = Command.ExecuteReader();
                    cmDataSet.ClientAccs.Load(DataReader, LoadOption.OverwriteChanges);
                    Connection.Close();
                }
                foreach (var row in cmDataSet.ClientAccs)
                {
                    Step1ClientAccBank.Items.Add(row);
                }
                Step1ClientAccBank.SelectedValueChanged += Step1ClientAccBank_SelectedValueChanged;
                Step1ClientAccBank.SelectedIndex = 0;
                IsLoadClientAccs = true;
            }

            if (Step1ClientAccBank.SelectedIndex == 0)
            {
                btn_Next.Enabled = false;
            }
            else
            {
                var row = Step1ClientAccBank.SelectedItem as CMDataSet.ClientAccsRow;
                //if ((row.AccLimite - row.AccCurrent) < Trades.Sum(c => c.Amount))
                //{
                //    btn_Next.Enabled = false;
                //}
                //else
                //{
                //    btn_Next.Enabled = true;
                //}
                btn_Next.Enabled = true;
                NeedCheckCustomerAccLimite = row.AccCheck.Contains("화주");
            }
        }

        private void Step1ClientAccBank_SelectedValueChanged(object sender, EventArgs e)
        {
            if (Step1ClientAccBank.SelectedIndex == 0)
            {
                btn_Next.Enabled = false;
                Step1ClientAccLimite.Text = "0";
                Step1ClientAccCurrent.Text = "0";
                Step1ClientAccLast.Text = "0";
                ClientAccId = 0;
            }
            else
            {
                var row = Step1ClientAccBank.SelectedItem as CMDataSet.ClientAccsRow;
                ClientAccId = row.ClientAccId;
                Step1ClientAccLimite.Text = row.AccLimite.ToString("N0");
                Step1ClientAccCurrent.Text = Sales.Sum(c => c.Amount).ToString("N0");
                Step1ClientAccLast.Text = (row.AccLimite - row.AccCurrent).ToString("N0");
                //if ((row.AccLimite - row.AccCurrent) < Trades.Sum(c => c.Amount))
                //{
                //    btn_Next.Enabled = false;
                //}
                //else
                //{
                //    btn_Next.Enabled = true;
                //}

                btn_Next.Enabled = true;

                NeedCheckCustomerAccLimite = row.AccCheck.Contains("화주");
            }

            Step4CardName.Text = Step1ClientAccBank.Text;
        }

        private void ShowStep2()
        {
            CurrentStep = 2;

            pnStep1.Visible = false;
            pnStep2.Visible = true;
            pnStep3.Visible = false;
            pnStep4.Visible = false;
            pnStep5.Visible = false;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[2].Height = 100F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[4].Height = 0F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[5].Height = 0F;

            lbl_Step1.Font = new Font(lbl_Step1.Font, FontStyle.Regular);
            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Bold);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Regular);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Regular);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Regular);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step3.ForeColor = Color.FromArgb(137, 206, 250);
            lbl_Step4.ForeColor = Color.FromArgb(137, 206, 250);
            lbl_Step5.ForeColor = Color.FromArgb(137, 206, 250);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step3.BackColor = Color.FromArgb(19, 124, 192);
            bar_Step4.BackColor = Color.FromArgb(19, 124, 192);
            bar_Step5.BackColor = Color.FromArgb(19, 124, 192);
            btn_Pre.Enabled = true;
            btn_Close.Enabled = true;
            btn_Next.Enabled = true;
            btn_ShowDenyList.Visible = false;

            if(!IsLoadStep2)
            {
                List<int> DenyedCustomerIdList = new List<int>();
                //foreach (var cGroup in Sales.Where(c => !c.IsCustomerAccIdNull() && c.CustomerAccId > 0).GroupBy(c => c.CustomerAccId))
                //{
                //    var cLast = cGroup.First().AccLimite - cGroup.First().AccCurrent;
                //    if (cLast < cGroup.Sum(c => c.Amount))
                //    {
                //        DenyedCustomerIdList.Add(cGroup.Key);
                //    }
                //}





                Step2TotalCount.Text = Sales.Length.ToString("N0") + " 건";
                List<CMDataSet.SalesManage2Row> Accepted = new List<CMDataSet.SalesManage2Row>();
                List<CMDataSet.SalesManage2Row> Denyed = new List<CMDataSet.SalesManage2Row>();
                foreach (var sales in Sales)
                {
                    if (sales.PayState == 1)
                    {
                        Denyed.Add(sales);
                        continue;
                    }
                    //if(trade.IsCustomerAccIdNull())
                    //{
                    //    Denyed.Add(trade);
                    //    continue;
                    //}


                    //if (!trade.AllowAcc || !trade.HasAcc)
                    //{
                    //    Denyed.Add(trade);
                    //    continue;
                    //}

                    if (!sales.HasACC)
                    {
                        Denyed.Add(sales);
                        continue;
                    }

                    if (sales.MID == "")
                    {
                        Denyed.Add(sales);
                        continue;
                    }

                    if(NeedCheckCustomerAccLimite)
                    {
                        //if (sales.IsCustomerAccIdNull())
                        //{
                        //    Denyed.Add(sales);
                        //    continue;
                        //}
                        ////if (trade.AccState != "제공")
                        ////{
                        ////    Denyed.Add(trade);
                        ////    continue;
                        ////}
                        //if (DenyedCustomerIdList.Contains(trade.CustomerId))
                        //{
                        //    Denyed.Add(trade);
                        //    continue;
                        //}
                    }
                    sales.CheckBox = true;
                    Accepted.Add(sales);
                }
                Step2AllowCount.Text = Accepted.Count.ToString("N0")+" 건";
                Step2AllowAmount.Text = "(" + Accepted.Sum(c => c.Amount).ToString("N0") + " 원)";
                Step2DenyCount.Text = Denyed.Count.ToString("N0") + " 건";
                AcceptSales = Accepted.ToArray();

                BindingList<DenyedItem> DenyedItemBindingList = new BindingList<DenyedItem>();
                foreach (var trade in Denyed)
                {
                   
                    var Added = new DenyedItem
                    {
                        DriverLoginId = trade.LoginId,
                        DriverCarNo = trade.BizNo,
                        DriverName = trade.SangHo,
                      //  CheckAllowAcc = trade.AllowAcc ? "O" : "X",
                        CheckHasAcc = trade.HasACC ? "신청" : "신청안함",
                        CheckSetYN = trade.PayState == 1?"결제":"결제안함",

                     
                        MID = trade.MID != "" ? trade.MID:"없음",

                       
                  
                       
                    };
                    if (NeedCheckCustomerAccLimite)
                    {
                        //if (trade.IsCustomerAccIdNull())
                        //{
                        //    Added.CheckAccState = "X";
                        //    Added.CheckAccLimite = "X";
                        //}
                        //else
                        //{
                        //    //if (trade.AccState != "제공")
                        //    //{
                        //    //    Added.CheckAccState = "X";
                        //    //}
                        //    //else
                        //    //{
                        //    //    Added.CheckAccState = "O";
                        //    //}
                        //    //if (DenyedCustomerIdList.Contains(trade.CustomerId))
                        //    //{
                        //    //    Added.CheckAccLimite = "X";
                        //    //}
                        //    //else
                        //    //{
                        //    //    Added.CheckAccLimite = "O";
                        //    //}
                        //}
                    }
                    else
                    {
                        Added.CheckAccState = "O";
                        Added.CheckAccLimite = "O";
                    }
                    DenyedItemBindingList.Add(Added);
                }
                Step2DenyList.DataSource = DenyedItemBindingList;
                IsLoadStep2 = true;
            }
            if(AcceptSales.Length == 0)
            {
                btn_Next.Enabled = false;
            }
        }

        private void btn_ShowDenyList_Click(object sender, EventArgs e)
        {
            Step2DenyList.Visible = true;
        }


        private void ShowStep3()
        {
            CurrentStep = 3;

            pnStep1.Visible = false;
            pnStep2.Visible = false;
            pnStep3.Visible = true;
            pnStep4.Visible = false;
            pnStep5.Visible = false;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[3].Height = 100F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[4].Height = 0F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[5].Height = 0F;

            lbl_Step1.Font = new Font(lbl_Step1.Font, FontStyle.Regular);
            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Bold);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Regular);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Regular);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step3.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step4.ForeColor = Color.FromArgb(137, 206, 250);
            lbl_Step5.ForeColor = Color.FromArgb(137, 206, 250);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step3.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step4.BackColor = Color.FromArgb(19, 124, 192);
            bar_Step5.BackColor = Color.FromArgb(19, 124, 192);
            btn_Pre.Enabled = true;
            btn_Close.Enabled = true;
            btn_Next.Enabled = true;
            btn_ShowDenyList.Visible = false;

            if (!IsLoadStep3)
            {
                BindingList<CMDataSet.SalesManage2Row> AcceptedItemBindingList = new BindingList<CMDataSet.SalesManage2Row>(AcceptSales);
                Step3List.AutoGenerateColumns = false;
                Step3List.DataSource = AcceptedItemBindingList;
                Step3List.CellContentClick += (sender, e) =>
                {
                    Step3List.EndEdit();
                    if (Step3List.Rows.Cast<DataGridViewRow>().Any(c=>(bool)Step3List[Step3Checkbox.Index,c.Index].Value == true))
                    {
                        btn_Next.Enabled = true;
                        //lbl_TotalMoney.Text = AcceptedItemBindingList.Where(c => c.CheckBox == true).Sum(c => c.Amount).ToString();
                        
                    }
                    else
                    {
                        btn_Next.Enabled = false;
                    }

                    AcceptSales = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as CMDataSet.SalesManage2Row).ToArray();

                
                    lbl_TotalMoney.Text =" ■ 총 결제금액 : " + AcceptSales.Sum(c => c.Amount).ToString("N0") + " 원";       

                };
                Step3AllSelect.CheckedChanged += (sender, e) =>
                  {
                      List<string> Totalmoney = new List<string>();
                      if(Step3AllSelect.Checked)
                      {
                          for (int i = 0; i < Step3List.RowCount; i++)
                          {
                              Step3List[Step3Checkbox.Index, i].Value = true;
                             
                          }

                          AcceptSales = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as CMDataSet.SalesManage2Row).ToArray();


                          lbl_TotalMoney.Text = " ■ 총 결제금액 : " + AcceptSales.Sum(c => c.Amount).ToString("N0") + " 원";       
                          
                      }
                      else
                      {
                          for (int i = 0; i < Step3List.RowCount; i++)
                          {
                              Step3List[Step3Checkbox.Index, i].Value = false;
                              
                          }
                          lbl_TotalMoney.Text = " ■ 총 결제금액 : " + "0";
                      }
                  };
                Step3AllSelect.Checked = true;
                IsLoadStep3 = true;
            }
        }

        private void ShowStep4()
        {
            CurrentStep = 4;

            pnStep1.Visible = false;
            pnStep2.Visible = false;
            pnStep3.Visible = false;
            pnStep4.Visible = true;
            pnStep5.Visible = false;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[4].Height = 100F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[5].Height = 0F;

            lbl_Step1.Font = new Font(lbl_Step1.Font, FontStyle.Regular);
            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Regular);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Bold);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Regular);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step3.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step4.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step5.ForeColor = Color.FromArgb(137, 206, 250);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step3.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step4.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step5.BackColor = Color.FromArgb(19, 124, 192);
            btn_Pre.Enabled = true;
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            btn_ShowDenyList.Visible = false;
          



            if (!IsLoadStep4)
            {
                if (LocalUser.Instance.PersonalOption.CardNumSave)
                {
                    chkCardNumSave.Checked = true;
                    Step4CardNo.Text = LocalUser.Instance.PersonalOption.CardNum.Substring(0, 15);
                }
                else
                {
                    chkCardNumSave.Checked = false;
                    cmb_CardGubun.SelectedIndex = 0;
                    Step4CardNo.Text = "";
                }

                Step4LGD_MID.Text = LocalUser.Instance.LogInInformation.LoginId;
                Step4Name.Text = LocalUser.Instance.LogInInformation.ClientName;
                IsLoadStep4 = true;
                Step4CardNo.TextChanged += Step4_TextChanged;
                Step4CardPassword.TextChanged += Step4_TextChanged;
                Step4CardMonth.SelectedIndexChanged += Step4_TextChanged;
                Step4CardYear.SelectedIndexChanged += Step4_TextChanged;
            }
            if (Step4CardNo.MaskFull &&  Step4CardPassword.MaskFull && (!Step4BizNo.Visible || Step4BizNo.MaskFull) && Step4CardMonth.Text != "" && Step4CardYear.Text!="")
            {
                btn_Next.Enabled = true;
            }
            if(PGGubun == 1)
            {
                Step4BizNo.TextChanged += Step4_TextChanged;
            }
            else if(PGGubun == 2)
            {
                lbl_CardGubun.Visible = false;
                Lbl_Step4BizNo.Visible = false;
                Info_Step4BizNo.Visible = false;
                cmb_CardGubun.Visible = false;
                Step4BizNo.Visible = false;
            }
           


        }

        private void Step4_TextChanged(object sender, EventArgs e)
        {
            //if (Step4CardNo.MaskFull && Step4CardDate.MaskFull && Step4CardPassword.MaskFull && (!Step4BizNo.Visible || Step4BizNo.MaskFull))
            //{
            //    btn_Next.Enabled = true;
            //}
            //else
            //{
            //    btn_Next.Enabled = false;
            //}

            if (Step4CardNo.MaskFull && Step4CardPassword.MaskFull && (!Step4BizNo.Visible || Step4BizNo.MaskFull) && Step4CardMonth.Text != "" && Step4CardYear.Text != "")
            {
                btn_Next.Enabled = true;

                if (chkCardNumSave.Checked)
                {


                    LocalUser.Instance.PersonalOption.CardNumSave = true;
                    LocalUser.Instance.PersonalOption.CardNum = Step4CardNo.Text;
                    LocalUser.Instance.Write();

                }
                else
                {

                    LocalUser.Instance.PersonalOption.CardNumSave = false;
                    LocalUser.Instance.PersonalOption.CardNum = "";
                    LocalUser.Instance.Write();

                }
            }
            else
            {
                btn_Next.Enabled = false;
            }
        }

        private void ShowStep5()
        {
            CurrentStep = 5;

            pnStep1.Visible = false;
            pnStep2.Visible = false;
            pnStep3.Visible = false;
            pnStep4.Visible = false;
            pnStep5.Visible = true;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;
            LayoutRoot.RowStyles[3].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[3].Height = 0F;
            LayoutRoot.RowStyles[4].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[4].Height = 0F;
            LayoutRoot.RowStyles[5].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[5].Height = 100F;

            lbl_Step1.Font = new Font(lbl_Step1.Font, FontStyle.Regular);
            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            lbl_Step3.Font = new Font(lbl_Step3.Font, FontStyle.Regular);
            lbl_Step4.Font = new Font(lbl_Step4.Font, FontStyle.Regular);
            lbl_Step5.Font = new Font(lbl_Step5.Font, FontStyle.Bold);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step3.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step4.ForeColor = Color.FromKnownColor(KnownColor.White);
            lbl_Step5.ForeColor = Color.FromKnownColor(KnownColor.White);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step3.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step4.BackColor = Color.FromKnownColor(KnownColor.White);
            bar_Step5.BackColor = Color.FromKnownColor(KnownColor.White);
            btn_Pre.Enabled = false;
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            btn_ShowDenyList.Visible = false;
            AcceptSales = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as CMDataSet.SalesManage2Row).ToArray();
            Step5Tatal = AcceptSales.Length;
            Step5Progress.Maximum = Step5Tatal;
            Step5Progress.Value = Step5Done;
            Step5Display();

            CardNo = Step4CardNo.Text.Replace("-", "");
            //CardDate = Step4CardDate.Text.Replace("-", "");
            CardDate = Step4CardYear.Text.Substring(2,2) + Step4CardMonth.Text;
            CardPan = Step4CardPassword.Text;
            UserType = cmb_CardGubun.SelectedValue.ToString();
            BizNo = Step4BizNo.Text;
            
           

            Step5Work();
        }
        private void Step5Work()
        {
            if (IsClosed)
                return;
            Step5Display();
            if (Step5Done < AcceptSales.Length)
            {
                ////KICC
                //if (PGGubun == 1)
                //{

                //    var CurrentTrade = AcceptSales[Step5Done];

                //    WebClient mWebClient = new WebClient();
                //    string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.SalesId, AuthKey, ClientAccId, CardNo, CardDate, CardPan, UserType, BizNo,CurrentTrade.MID });
                //    mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                //    mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/KCCCardAuth_C" + Parameter));
                ////    mWebClient.DownloadStringAsync(new Uri("http://m.mycalltruck.co.kr/Pay/KCCCardAuth" + Parameter));
                // //   mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/KCCCardAuth_C" + Parameter));
                //}
                ////LGU+
                //else if (PGGubun == 2)
                //{
                //    var CurrentTrade = AcceptSales[Step5Done];
                //    WebClient mWebClient = new WebClient();
                //    string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.SalesId, AuthKey, ClientAccId, CardNo, CardDate, CardPan });
                //    mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                //    //mWebClient.DownloadStringAsync(new Uri("http://localhost/Truck.MVC/Pay/CardAuth" + Parameter));
                //    mWebClient.DownloadStringAsync(new Uri("http://m.mycalltruck.co.kr/Pay/CardAuth" + Parameter));
                //}

                var CurrentTrade = AcceptSales[Step5Done];
                WebClient mWebClient = new WebClient();
                string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.SalesId, AuthKey, ClientAccId, CardNo, CardDate, CardPan });
                mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                //mWebClient.DownloadStringAsync(new Uri("http://localhost/Truck.MVC/Pay/CardAuth" + Parameter));
                mWebClient.DownloadStringAsync(new Uri("http://m.mycalltruck.co.kr/Pay/CardAuth" + Parameter));
            }
            else
            {
                Step5Complete();
            }
        }
        List<Int64> FAmount = new List<Int64>();
        List<Int64> SAmount = new List<Int64>();
        List<Int64> TAmount = new List<Int64>();
        private void MWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
         
            if (e.Cancelled)
            {
                Step5Failed++;
                var CurrentTrade = AcceptSales[Step5Done];
                string IMount =  CurrentTrade.Amount.ToString("N0").Replace(",","");
                FAmount.Add(Int64.Parse(IMount));
            }
            else if (e.Error != null)
            {
                Step5Failed++;
                var CurrentTrade = AcceptSales[Step5Done];
                string IMount = CurrentTrade.Amount.ToString("N0").Replace(",", "");
                FAmount.Add(Int64.Parse(IMount));
            }
            else
            {
                if (e.Result != null && e.Result.ToLower() == "true")
                {
                    Step5Success++;
                    var CurrentTrade = AcceptSales[Step5Done];
                    string IMount = CurrentTrade.Amount.ToString("N0").Replace(",", "");
                    SAmount.Add(Int64.Parse(IMount));
                }
                else
                {
                    Step5Failed++;
                    var CurrentTrade = AcceptSales[Step5Done];
                    string IMount = CurrentTrade.Amount.ToString("N0").Replace(",", "");
                    FAmount.Add(Int64.Parse(IMount));
                }
            }

            var TCurrentTrade = AcceptSales[Step5Done];
            string TMount = TCurrentTrade.Amount.ToString("N0").Replace(",", "");
            TAmount.Add(Int64.Parse(TMount));


            Step5Done++;
            Step5Work();
        }


        private void Step5Complete()
        {
            Step5Info.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            Step5Info.Font = new Font(Step5Info.Font, FontStyle.Bold);
            Step5Info.Text = "결제가 완료 되었습니다.";
            btn_Next.Text = "완료";
            btn_Close.Enabled = false;
            btn_Next.Enabled = true;
        }

        private void Step5Display()
        {

            Step5TatalMoney = TAmount.Sum();
            Step5SuccessMoney = SAmount.Sum();
            Step5FailedMoney = FAmount.Sum();

            Step5TotalCount.Text = String.Format("{0:N0} / {1:N0} 건", Step5Done, Step5Tatal);
            Step5TotalMoney.Text = String.Format(": {0:N0} 원", Step5TatalMoney);

            Step5SuccessCount.Text = String.Format("{0:N0} 건", Step5Success);
            lbl_Step5SuccessMoney.Text = String.Format(": {0:N0} 원", Step5SuccessMoney);

            Step5FailedCount.Text = String.Format("{0:N0} 건", Step5Failed);
            lbl_Step5FailedMoney.Text = String.Format(": {0:N0} 원", Step5FailedMoney);


         //   List<CMDataSet.TradesRow> AccLogs = new List<CMDataSet.TradesRow>();

            string LGD_RESPMSG = "";
            BindingList<ACCLOGITEM> AccLogsItemBindingList = new BindingList<ACCLOGITEM>();
            foreach (var trade in AcceptSales)
            {
                accLogsTableAdapter.Fill(cmDataSet.AccLogs);
                var Query = cmDataSet.AccLogs.Where(c => c.SalesId == trade.SalesId).ToArray().OrderByDescending(c => c.AccLogId);
                if (Query.Any())
                {
                    LGD_RESPMSG = Query.First().LGD_RESPMSG;
                }
                else
                {
                    LGD_RESPMSG = "";
                }

                var Added = new ACCLOGITEM
                {
                    DriverLoginId = trade.LoginId,
                    DriverCarNo = trade.BizNo,
                    DriverName = trade.SangHo,
                    Amount = trade.Amount.ToString("N0"),
                    BankName = trade.PayBankName,
                    PayAccountNo = trade.PayAccountNo,

                    RESPMSG = LGD_RESPMSG,
                       
                    //  MID = trade.MID != "" ? trade.MID:"없음",




                };

                AccLogsItemBindingList.Add(Added);

            }
            newDGV1.DataSource = AccLogsItemBindingList;


            // Step5Progress.Value = Step5Done;
        }


        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 1)
                ShowStep2();
            else if (CurrentStep == 2)
                ShowStep3();
            else if (CurrentStep == 3)
                ShowStep4();
            else if (CurrentStep == 4)
            {
                if (MessageBox.Show("정말 카드결제를 하시겠습니까?", "카드결제 확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    ShowStep5();
            }
            else if (CurrentStep == 5)
                Close();
        }

        private void btn_Pre_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 4)
                ShowStep3();
            else if (CurrentStep == 3)
                ShowStep2();
            else if (CurrentStep == 2)
                ShowStep1();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Step3List_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

           
         
            
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == 0)
            {
                e.Value = (Step3List.RowCount - e.RowIndex).ToString("N0");
            }

            if (Step3List.Columns[e.ColumnIndex] == Step3ColAccountNo)
            {
                string tempString = string.Empty;
                tempString = Step3List.Rows[e.RowIndex].Cells["Step3ColAccountNo"].Value.ToString();
                try
                {
                    byte[] data = Convert.FromBase64String(tempString);


                    e.Value = m_crypt.Decrypt(tempString);
                }
                catch
                {
                    e.Value = tempString;
                }
            }
        }

        private void Dialog_AcceptAcc_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsClosed = true;
        }
        class DenyedItem
        {
            public String DriverLoginId { get; set; }
            public String DriverCarNo { get; set; }
            public String DriverName { get; set; }

            //화주보험가입
            public String CheckAllowAcc { get; set; }

            //바로결제신청
            public String CheckHasAcc { get; set; }

            //화주서비스제공
            public String CheckAccState { get; set; }
            //화주한도
            public String CheckAccLimite { get; set; }
            public String CheckSetYN { get; set; }
            public String MID { get; set; }
        }

        class ACCLOGITEM
        {
            public String DriverLoginId { get; set; }
            public String DriverCarNo { get; set; }
            public String DriverName { get; set; }

            //결제금액
            public String Amount { get; set; }

            //은행명
            public String BankName { get; set; }

            //계좌번호
            public String PayAccountNo { get; set; }
            
            public String RESPMSG { get; set; }
        }

        private void Step2DenyList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == 0)
            {
                e.Value = (Step2DenyList.RowCount - e.RowIndex).ToString("N0");
            }

            if (e.ColumnIndex == 5)
            {
                // if (CurrentTrade..CheckPayState == "미결제")
                if (e.Value.ToString() == "신청")
                {
                    //  e.Value = "미결제";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
                else
                {
                    // e.Value = "결제완료";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }
            if (e.ColumnIndex == 8)
            {
                if (e.Value.ToString() == "결제")
                {
                    //e.Value = "아니오";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
                else
                {
                    //  e.Value = "예";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }

            if (e.ColumnIndex == 9)
            {
                if (e.Value.ToString() == "없음")
                {
                    //e.Value = "아니오";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
                else
                {
                    //  e.Value = "예";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
            }
        }

        private void cmb_CardGubun_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
          
          
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (newDGV1.RowCount - e.RowIndex).ToString("N0");
            }
            
        }


        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;
        private void btnExcelExport_Click(object sender, EventArgs e)
        {

            fileString = string.Format("승인결과") + DateTime.Now.ToString("yyyyMMdd");
            title = "승인결과";

            ieExcel = Properties.Resources.승인결과;





            if (newDGV1.RowCount == 0)
            {
                MessageBox.Show("내보낼 승인결과 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (newDGV1.RowCount > 0)
            {
                pnProgress.Visible = true;
                bar.Value = 0;
                Thread t = new Thread(new ThreadStart(() =>
                {
                    newDGV1.ExportExistExcel2(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.KICC);
                    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));

                    //mycalltruck.Admin.Class.Extensions.Excel_Epplus.Ep_FileExport(dataGridView1, title, fileString, bar, true, ieExcel, 2);
                    //pnProgress.Invoke(new Action(() => pnProgress.Visible = false));


                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            else
            {
                MessageBox.Show("엑셀로 내보낼 승인결과 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        



        private void chkCardGubunSave_CheckedChanged(object sender, EventArgs e)
        {

            if (chkCardGubunSave.Checked)
            {
                
               
                //LocalUser.Instance.LogInInfomation.CardGubunSave = true;
                //LocalUser.Instance.LogInInfomation.CardGubun = cmb_CardGubun.SelectedIndex.ToString();
                //LocalUser.Instance.LogInInfomation.CardBizNo = Step4BizNo.Text;
                //LocalUser.Instance.Write();

            }
            else
            {
               
                //LocalUser.Instance.LogInInfomation.CardGubunSave = false;
                //LocalUser.Instance.LogInInfomation.CardGubun = "0";

                //LocalUser.Instance.LogInInfomation.CardBizNo = "";
                //LocalUser.Instance.Write();


                Step4BizNo.Text = "";
            }


           
        }

        private void chkCardNumSave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCardNumSave.Checked)
            {


                //LocalUser.Instance.LogInInfomation.CardNumSave = true;
                //LocalUser.Instance.LogInInfomation.CardNum = Step4CardNo.Text;
                //LocalUser.Instance.Write();

            }
            else
            {
                Step4CardNo.Text = "";
                //LocalUser.Instance.LogInInfomation.CardNumSave = false;
                //LocalUser.Instance.LogInInfomation.CardNum = "";
                //LocalUser.Instance.Write();

            }
        }

      
    }

}
