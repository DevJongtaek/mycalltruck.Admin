using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using System.Runtime.CompilerServices;
using System.Net.Sockets;

namespace mycalltruck.Admin
{
    public partial class Dialog_AcceptBank : Form
    {
        public Dialog_AcceptBank()
        {
            InitializeComponent();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);
            Dictionary<string, string> CardGubun = new Dictionary<string, string>();
            CardGubun.Add("0", "개인");
            CardGubun.Add("1", "법인");
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
            //Step4CardMonth.DataSource = new BindingSource(Month, null);
            //Step4CardMonth.DisplayMember = "Value";
            //Step4CardMonth.ValueMember = "Key";
            Dictionary<string, string> Year = new Dictionary<string, string>();
            Year.Add("", "");
            for (int i = DateTime.Now.Year; i <= DateTime.Now.AddYears(10).Year; i++)
            {
                Year.Add(i.ToString(), i.ToString());
            }
            //Step4CardYear.DataSource = new BindingSource(Year, null);
            //Step4CardYear.DisplayMember = "Value";
            //Step4CardYear.ValueMember = "Key";
            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
            Step4CMSBankName.Text = Client.CMSBankName;
            Step4CMSAccountNo.Text = Client.CMSAccountNo;
            Step4CMSOwner.Text = Client.CMSOwner;
            PGGubun = Client.PGGubun;
        }

        public TradeDataSet.TradesRow[] Trades { get; set; }
        private TradeDataSet.TradesRow[] AcceptTrades { get; set; }

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


        private void ShowStep2()
        {
            CurrentStep = 2;

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
            List<TradeDataSet.TradesRow> Denyed = new List<TradeDataSet.TradesRow>();

            if (!IsLoadStep2)
            {
                Step2TotalCount.Text = Trades.Length.ToString("N0") + " 건";
                List<TradeDataSet.TradesRow> Accepted = new List<TradeDataSet.TradesRow>();
                foreach (var trade in Trades)
                {
                    if (trade.PayState == 1)
                    {
                        Denyed.Add(trade);
                        continue;
                    }

                    if (trade.HasAcc)
                    {
                        Denyed.Add(trade);
                        continue;
                    }

                    if (trade.ServiceState != 1)
                    {
                        Denyed.Add(trade);
                        continue;
                    }
                    Accepted.Add(trade);
                }
                Step2AllowCount.Text = Accepted.Count.ToString("N0") + " 건";
                Step2AllowAmount.Text = "(" + Accepted.Sum(c => c.Amount).ToString("N0") + " 원)";
                Step2DenyCount.Text = Denyed.Count.ToString("N0") + " 건";
                AcceptTrades = Accepted.ToArray();

                BindingList<DenyedItem> DenyedItemBindingList = new BindingList<DenyedItem>();
                foreach (var trade in Denyed)
                {

                    var Added = new DenyedItem
                    {
                        DriverLoginId = trade.DriverLoginId,
                        DriverCarNo = trade.DriverCarNo,
                        DriverName = trade.DriverName,
                        CheckHasAcc = trade.HasAcc ? "신청안함" : "신청",
                        CheckSetYN = trade.PayState == 1 ? "결제" : "결제안함",


                        //MID = trade.MID != "" ? trade.MID:"없음",
                        MID = trade.ServiceState == 1 ? "O" : "X",




                    };
                    DenyedItemBindingList.Add(Added);
                }
                Step2DenyList.AutoGenerateColumns = false;
                Step2DenyList.DataSource = DenyedItemBindingList;
                IsLoadStep2 = true;
            }
            if (AcceptTrades.Length == 0)
            {
                btn_Next.Enabled = false;
            }
            if (Denyed.Count == 0)
                ShowStep3();
        }

        private void btn_ShowDenyList_Click(object sender, EventArgs e)
        {
            Step2DenyList.Visible = true;
        }


        private void ShowStep3()
        {
            CurrentStep = 3;

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
                BindingList<TradeDataSet.TradesRow> AcceptedItemBindingList = new BindingList<TradeDataSet.TradesRow>(AcceptTrades);
                Step3List.AutoGenerateColumns = false;
                Step3List.DataSource = AcceptedItemBindingList;
                Step3List.CellContentClick += (sender, e) =>
                {
                    Step3List.EndEdit();
                    if (Step3List.Rows.Cast<DataGridViewRow>().Any(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true))
                    {
                        btn_Next.Enabled = true;
                        //lbl_TotalMoney.Text = AcceptedItemBindingList.Where(c => c.CheckBox == true).Sum(c => c.Amount).ToString();

                    }
                    else
                    {
                        btn_Next.Enabled = false;
                    }

                    AcceptTrades = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as TradeDataSet.TradesRow).ToArray();


                    lbl_TotalMoney.Text = " ■ 총 결제금액 : " + AcceptTrades.Sum(c => c.Amount).ToString("N0") + " 원";
                    Step4TotalCount.Text = AcceptTrades.Count().ToString();

                };
                Step3AllSelect.CheckedChanged += (sender, e) =>
                {
                    List<string> Totalmoney = new List<string>();
                    if (Step3AllSelect.Checked)
                    {
                        for (int i = 0; i < Step3List.RowCount; i++)
                        {
                            Step3List[Step3Checkbox.Index, i].Value = true;

                        }

                        AcceptTrades = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as TradeDataSet.TradesRow).ToArray();


                        lbl_TotalMoney.Text = " ■ 총 결제금액 : " + AcceptTrades.Sum(c => c.Amount).ToString("N0") + " 원";
                        Step4TotalCount.Text = AcceptTrades.Count().ToString();

                    }
                    else
                    {
                        for (int i = 0; i < Step3List.RowCount; i++)
                        {
                            Step3List[Step3Checkbox.Index, i].Value = false;

                        }
                        lbl_TotalMoney.Text = " ■ 총 결제금액 : " + "0";
                        Step4TotalCount.Text = AcceptTrades.Count().ToString();
                    }
                };
                Step3AllSelect.Checked = true;
                IsLoadStep3 = true;
            }
        }
        private TcpClient server;
        private NetworkStream ns;
        private bool isRunning = true;

        private void ShowStep4()
        {
            CurrentStep = 4;

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
                Step4TotalAmount.Text = lbl_TotalMoney.Text;
                
              
                IsLoadStep4 = true;


              


        
               
            }
           
            //if (Step4CardNo1.Text.Length == 4 && Step4CardNo2.Text.Length == 4 && Step4CardNo3.Text.Length == 4 && Step4CardNo4.Text.Length == 4 && Step4CardPassword.MaskFull && Step4CardMonth.Text != "" && Step4CardYear.Text != "")
            //{
            //    btn_Next.Enabled = true;
            //}


           
        }

        private void Step4_TextChanged(object sender, EventArgs e)
        {
            //if (Step4CardNo1.Text.Length == 4 && Step4CardNo2.Text.Length == 4 && Step4CardNo3.Text.Length == 4 && Step4CardNo4.Text.Length == 4 && Step4CardPassword.MaskFull && Step4CardMonth.Text != "" && Step4CardYear.Text != "")
            //{
            //    btn_Next.Enabled = true;
            //    if (chkCardNumSave.Checked)
            //    {
            //        LocalUser.Instance.PersonalOption.CardNumSave = true;
            //        LocalUser.Instance.PersonalOption.CardNum = Step4CardNo1.Text + "-" + Step4CardNo2.Text + "-" + Step4CardNo3.Text + "-" + Step4CardNo4.Text ;
            //        LocalUser.Instance.PersonalOption.CardMonth = Step4CardMonth.SelectedIndex;
            //        LocalUser.Instance.PersonalOption.CardYear = Step4CardYear.SelectedIndex;
            //        LocalUser.Instance.Write();
            //    }
            //    else
            //    {
            //        LocalUser.Instance.PersonalOption.CardNumSave = false;
            //        LocalUser.Instance.PersonalOption.CardNum = "";
            //        LocalUser.Instance.PersonalOption.CardMonth = 0;
            //        LocalUser.Instance.PersonalOption.CardYear = 0;
            //        LocalUser.Instance.Write();
            //    }
            //}
            //else
            //{
            //    btn_Next.Enabled = false;
            //}
        }

        private void ShowStep5()
        {
            CurrentStep = 5;

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
            AcceptTrades = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as TradeDataSet.TradesRow).ToArray();
            Step5Tatal = AcceptTrades.Length;
            Step5Progress.Maximum = Step5Tatal;
            Step5Progress.Value = Step5Done;

            Step5Display(true);
            //CardNo = Step4CardNo1.Text+ Step4CardNo2.Text+ Step4CardNo3.Text+ Step4CardNo4.Text;
            //CardDate = Step4CardYear.Text.Substring(2, 2) + Step4CardMonth.Text;
            //CardPan = Step4CardPassword.Text;
            Step5Work();
        }
        private void Step5Work()
        {
            if (IsClosed)
                return;
            if (Step5Done < AcceptTrades.Length)
            {
                var CurrentTrade = AcceptTrades[Step5Done];
                WebClient mWebClient = new WebClient();
                string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.TradeId, AuthKey, ClientAccId, CardNo, CardDate, CardPan, CurrentTrade.DriverLoginId });
                mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/CardAuth" + Parameter));
                //mWebClient.DownloadStringAsync(new Uri("http://localhost/Pay/CardAuth" + Parameter));
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
                var CurrentTrade = AcceptTrades[Step5Done];
                string IMount = CurrentTrade.Amount.ToString("N0").Replace(",", "");
                FAmount.Add(Int64.Parse(IMount));
            }
            else if (e.Error != null)
            {
                Step5Failed++;
                var CurrentTrade = AcceptTrades[Step5Done];
                string IMount = CurrentTrade.Amount.ToString("N0").Replace(",", "");
                FAmount.Add(Int64.Parse(IMount));
            }
            else
            {
                if (e.Result != null && e.Result.ToLower() == "true")
                {
                    Step5Success++;
                    var CurrentTrade = AcceptTrades[Step5Done];
                    string IMount = CurrentTrade.Amount.ToString("N0").Replace(",", "");
                    SAmount.Add(Int64.Parse(IMount));
                }
                else
                {
                    Step5Failed++;
                    var CurrentTrade = AcceptTrades[Step5Done];
                    string IMount = CurrentTrade.Amount.ToString("N0").Replace(",", "");
                    FAmount.Add(Int64.Parse(IMount));
                }
            }

            var TCurrentTrade = AcceptTrades[Step5Done];
            string TMount = TCurrentTrade.Amount.ToString("N0").Replace(",", "");
            TAmount.Add(Int64.Parse(TMount));


            Step5Display(false, AcceptTrades[Step5Done].TradeId);
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

        BindingList<ACCLOGITEM> AccLogsItemBindingList = new BindingList<ACCLOGITEM>();
        private void Step5Display(bool first, int _TradeId = 0)
        {
            if(!first)
                Step5Done++;
            Step5TatalMoney = TAmount.Sum();
            Step5SuccessMoney = SAmount.Sum();
            Step5FailedMoney = FAmount.Sum();

            Step5TotalCount.Text = String.Format("{0:N0} / {1:N0} 건", Step5Done, Step5Tatal);
            Step5TotalMoney.Text = String.Format(": {0:N0} 원", Step5TatalMoney);

            Step5SuccessCount.Text = String.Format("{0:N0} 건", Step5Success);
            lbl_Step5SuccessMoney.Text = String.Format(": {0:N0} 원", Step5SuccessMoney);

            Step5FailedCount.Text = String.Format("{0:N0} 건", Step5Failed);
            lbl_Step5FailedMoney.Text = String.Format(": {0:N0} 원", Step5FailedMoney);

            if (first)
            {
                foreach (var trade in AcceptTrades)
                {
                    var Added = new ACCLOGITEM
                    {
                        Id = trade.TradeId,
                        DriverLoginId = trade.DriverLoginId,
                        DriverCarNo = trade.DriverCarNo,
                        DriverName = trade.DriverName,
                        Amount = trade.Amount.ToString("N0"),
                        BankName = trade.PayBankName,
                        PayAccountNo = trade.PayAccountNo,
                    };
                    AccLogsItemBindingList.Add(Added);
                }
            }
            else
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = @"SELECT  LGD_RESPMSG
                                                            FROM     AccLogs
                                                            WHERE TradeId = @TradeId
                                                            ORDER BY AccLogId DESC";
                        _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                var LGD_RESPMSG = _Reader.GetString(0);
                                if (AccLogsItemBindingList.Any(c => c.Id == _TradeId))
                                    AccLogsItemBindingList.First(c => c.Id == _TradeId).RESPMSG = LGD_RESPMSG;
                            }
                        }
                    }
                    _Connection.Close();

                }
            }

            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = AccLogsItemBindingList;
            Step5Progress.Value = Step5Done;
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
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Step3List_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (Step3List.RowCount - e.RowIndex).ToString("N0");
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

            //바로결제신청
            public String CheckHasAcc { get; set; }

            public String CheckSetYN { get; set; }
            public String MID { get; set; }
        }

        class ACCLOGITEM : INotifyPropertyChanged
        {
            private int _Id;
            private String _DriverLoginId;
            private String _DriverCarNo;
            private String _DriverName;
            private String _Amount;
            private String _BankName;
            private String _PayAccountNo;
            private String _RESPMSG;

            public int Id { get { return _Id; } set { SetField(ref _Id, value); } }
            public string DriverLoginId { get { return _DriverLoginId; } set { SetField(ref _DriverLoginId, value); } }
            public string DriverCarNo { get { return _DriverCarNo; } set { SetField(ref _DriverCarNo, value); } }
            public string DriverName { get { return _DriverName; } set { SetField(ref _DriverName, value); } }
            public string Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            public string BankName { get { return _BankName; } set { SetField(ref _BankName, value); } }
            public string PayAccountNo { get { return _PayAccountNo; } set { SetField(ref _PayAccountNo, value); } }
            public string RESPMSG { get { return _RESPMSG; } set { SetField(ref _RESPMSG, value); } }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }

        private void Step2DenyList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
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





        private void chkCardNumSave_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkCardNumSave.Checked)
            //{


            //    //LocalUser.Instance.LogInInfomation.CardNumSave = true;
            //    //LocalUser.Instance.LogInInfomation.CardNum = Step4CardNo.Text;
            //    //LocalUser.Instance.Write();

            //}
            //else
            //{
            //    Step4CardNo.Text = "";
            //    //LocalUser.Instance.LogInInfomation.CardNumSave = false;
            //    //LocalUser.Instance.LogInInfomation.CardNum = "";
            //    //LocalUser.Instance.Write();

            //}
        }

        private void Dialog_AcceptAcc_Load(object sender, EventArgs e)
        {
            ShowStep2();
        }

      

        private void Step4CardNo1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Step4CardNo1.Text.Length == 4)
            //{
            //    Step4CardNo2.Text = "";
            //    Step4CardNo2.Focus();

            //}
        }
        private void Step4CardNo2_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Step4CardNo2.Text.Length == 4)
            //{
            //    Step4CardNo3.Text = "";
            //    Step4CardNo3.Focus();

            //}
        }
        private void Step4CardNo3_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Step4CardNo3.Text.Length == 4)
            //{
            //    Step4CardNo4.Text = "";
            //    Step4CardNo4.Focus();

            //}
        }

        private void Step4CardNo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void Step4CardNo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void Step4CardNo4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void Step4CardNo3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
    }

}
