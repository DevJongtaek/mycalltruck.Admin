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
namespace mycalltruck.Admin
{
    public partial class Dialog_CancelAccTest : Form
    {
        public Dialog_CancelAccTest()
        {
            InitializeComponent();
            CMDataSetTableAdapters.ClientsTableAdapter clientsTableAdapter = new CMDataSetTableAdapters.ClientsTableAdapter();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);
            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
            PGGubun = Client.PGGubun;
        }

        public CMDataSet.TradesRow[] Trades { get; set; }
        private CMDataSet.TradesRow[] AcceptTrades { get; set; }

        private int CurrentStep = 1;
        private bool IsLoadStep1 = false;
        private int Step5Tatal = 0;
        private int Step5Done = 0;
        private int Step5Success = 0;
        private int Step5Failed = 0;
        private bool IsClosed = false;
        private int PGGubun = 0;
        //Parameter
        public String AuthKey { get; set; }
        List<CMDataSet.TradesRow> Accepted = new List<CMDataSet.TradesRow>();
        List<CMDataSet.TradesRow> Denyed = new List<CMDataSet.TradesRow>();
        BindingList<DenyedItem> DenyedItemBindingList = new BindingList<DenyedItem>();
        private void ShowStep1()
        {
            CurrentStep = 1;

            pnStep1.Visible = true;
            pnStep2.Visible = false;

            LayoutRoot.RowStyles[1].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[1].Height = 100F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[2].Height = 0F;

            lbl_Step1.Font = new Font(lbl_Step1.Font, FontStyle.Bold);
            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Regular);
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            //   btn_ShowDenyList.Visible = true;

            //Load ClientAccs
            if (!IsLoadStep1)
            {
                Step4LGD_MID.Text = LocalUser.Instance.LogInInformation.LoginId;
                Step4Name.Text = LocalUser.Instance.LogInInformation.ClientName;
                Step2TotalCount.Text = Trades.Length.ToString("N0") + " 건";
                //List<CMDataSet.TradesRow> Accepted = new List<CMDataSet.TradesRow>();
                //List<CMDataSet.TradesRow> Denyed = new List<CMDataSet.TradesRow>();
                //BindingList<DenyedItem> DenyedItemBindingList = new BindingList<DenyedItem>();
                foreach (var trade in Trades)
                {
                    if (trade.PayState == 2)
                    {
                        Denyed.Add(trade);
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

                    if (!trade.HasAcc)
                    {
                        Denyed.Add(trade);
                        continue;
                    }
                    if (trade.PayDate.Date != DateTime.Now.Date)
                    {
                        Denyed.Add(trade);
                        continue;
                    }
                    if (String.IsNullOrEmpty(trade.AccCheck))
                    {
                        Denyed.Add(trade);
                        continue;
                    }
                    if (trade.AccCheck.Contains("화주") && trade.CustomerAccId == 0)
                    {
                        Denyed.Add(trade);
                        continue;
                    }
                    trade.CheckBox = true;
                    Accepted.Add(trade);
                }
                Step2AllowCount.Text = Accepted.Count.ToString("N0") + " 건";
                Step2AllowAmount.Text = "(" + Accepted.Sum(c => c.Amount).ToString("N0") + " 원)";
                Step2DenyCount.Text = (Trades.Length - Accepted.Count).ToString("N0") + " 건";
                AcceptTrades = Accepted.ToArray();

                foreach (var trade in Denyed)
                {
                    var Added = new DenyedItem
                    {
                        DriverLoginId = trade.DriverLoginId,
                        DriverCarNo = trade.DriverCarNo,
                        //CheckPayState = "O",
                        CheckPayState = "결제완료",
                        //CheckPayDate = "O",
                        CheckPayDate = "예",
                        CheckClientAccId = "O",
                        CheckCustomerAccId = "O",
                    };
                    if (!trade.AllowAcc || !trade.HasAcc)
                    {
                        Added.CheckPayState = "X";
                        Added.CheckPayState = "미결제";
                        //Added.CheckPayDate = "X";
                        Added.CheckPayDate = "아니오";
                        Added.CheckClientAccId = "X";
                        Added.CheckCustomerAccId = "X";
                    }
                    if (trade.PayState == 2)
                    {
                        // Added.CheckPayState = "X";
                        Added.CheckPayState = "미결제";

                    }
                    if (trade.PayDate.Date != DateTime.Now.Date)
                    {
                        // Added.CheckPayDate = "X";
                        Added.CheckPayDate = "아니오";
                    }
                    if (String.IsNullOrEmpty(trade.AccCheck))
                    {
                        Added.CheckClientAccId = "X";
                    }
                    if (trade.AccCheck.Contains("화주") && trade.CustomerAccId == 0)
                    {
                        Added.CheckCustomerAccId = "X";
                    }
                    DenyedItemBindingList.Add(Added);
                }
                Step2DenyList.DataSource = DenyedItemBindingList;


                IsLoadStep1 = true;
            }
            if (AcceptTrades.Length == 0)
            {
                btn_Next.Enabled = false;
            }
            else
            {
                btn_Next.Enabled = true;
            }
        }


        private void ShowStep2()
        {
            CurrentStep = 2;

            pnStep1.Visible = false;
            pnStep2.Visible = true;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;
            LayoutRoot.RowStyles[2].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[2].Height = 100F;

            lbl_Step1.Font = new Font(lbl_Step1.Font, FontStyle.Regular);
            lbl_Step2.Font = new Font(lbl_Step2.Font, FontStyle.Bold);
            lbl_Step2.ForeColor = Color.FromKnownColor(KnownColor.White);
            bar_Step2.BackColor = Color.FromKnownColor(KnownColor.White);
            btn_Close.Enabled = true;
            btn_Next.Enabled = false;
            btn_ShowDenyList.Visible = false;
            Step5Tatal = AcceptTrades.Length;
            Step5Progress.Maximum = Step5Tatal;
            Step5Progress.Value = Step5Done;
            Step5Display();


            Step5Work();
        }
        private void Step5Work()
        {
            if (IsClosed)
                return;
            Step5Display();
            if (Step5Done < AcceptTrades.Length)
            {
                //if(PGGubun == 1)
                //{
                //    var CurrentTrade = AcceptTrades[Step5Done];
                //    WebClient mWebClient = new WebClient();
                //    //   string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.TradeId, AuthKey,CurrentTrade.MID });DriverLoginId
                //    string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.TradeId, AuthKey, CurrentTrade.DriverLoginId });
                //    mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                // //   mWebClient.DownloadStringAsync(new Uri("http://m.mycalltruck.co.kr/Pay/KCCCardCancel" + Parameter));
                //    mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/KCCCardCancel" + Parameter));
                //    //mWebClient.DownloadStringAsync(new Uri("http://localhost/Truck.MVC/Pay/KCCCardCancel" + Parameter));
                //}
                //else
                //{
                //    var CurrentTrade = AcceptTrades[Step5Done];
                //    WebClient mWebClient = new WebClient();
                //    string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.TradeId, AuthKey });
                //    mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                //    mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/CardCancel" + Parameter));
                //}

                var CurrentTrade = AcceptTrades[Step5Done];
                WebClient mWebClient = new WebClient();
                string Parameter = "?sPrameter=" + String.Join("^", new object[] { CurrentTrade.TradeId, AuthKey, CurrentTrade.DriverLoginId });
                mWebClient.DownloadStringCompleted += MWebClient_DownloadStringCompleted;
                mWebClient.DownloadStringAsync(new Uri("http://m.cardpay.kr/Pay/CardCancel" + Parameter));
            }
            else
            {
                Step5Complete();
            }
        }

        private void MWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
                Step5Failed++;
            else if (e.Error != null)
                Step5Failed++;
            else
            {
                if (e.Result != null && e.Result.ToLower() == "true")
                    Step5Success++;
                else
                    Step5Failed++;
            }
            Step5Done++;
            Step5Work();
        }


        private void Step5Complete()
        {
            Step5Info.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            Step5Info.Font = new Font(Step5Info.Font, FontStyle.Bold);
            Step5Info.Text = "취소가 완료 되었습니다.";
            btn_Next.Text = "완료";
            btn_Close.Enabled = false;
            btn_Next.Enabled = true;
        }

        private void Step5Display()
        {
            Step5TotalCount.Text = String.Format("{0:N0} / {1:N0} 건", Step5Done, Step5Tatal);
            Step5SuccessCount.Text = String.Format("{0:N0} 건", Step5Success);
            Step5FailedCount.Text = String.Format("{0:N0} 건", Step5Failed);
            Step5Progress.Value = Step5Done;


            string LGD_RESPMSG = "";
            BindingList<ACCLOGITEM> AccLogsItemBindingList = new BindingList<ACCLOGITEM>();
            foreach (var trade in AcceptTrades)
            {
                accLogsTableAdapter.Fill(cmDataSet.AccLogs);
                var Query = cmDataSet.AccLogs.Where(c => c.TradeId == trade.TradeId).ToArray().OrderByDescending(c => c.AccLogId);
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
                    DriverLoginId = trade.DriverLoginId,
                    DriverCarNo = trade.DriverCarNo,
                    DriverName = trade.DriverName,
                    Amount = trade.Amount.ToString("N0"),
                    BankName = trade.PayBankName,
                    PayAccountNo = trade.PayAccountNo,

                    RESPMSG = LGD_RESPMSG,

                    //  MID = trade.MID != "" ? trade.MID:"없음",




                };

                AccLogsItemBindingList.Add(Added);

            }
            newDGV1.DataSource = AccLogsItemBindingList;


        }


        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 1)
            {
                if (MessageBox.Show("정말 카드취소를 하시겠습니까?", "카드취소 확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    ShowStep2();

            }
            else if (CurrentStep == 2)
                Close();
        }



        private void btn_Close_Click(object sender, EventArgs e)
        {
          
            Close();
        }

        private void Dialog_AcceptAcc_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsClosed = true;
        }

        private void Dialog_CancelAcc_Load(object sender, EventArgs e)
        {
            ShowStep1();

        }

        private void btn_ShowDenyList_Click(object sender, EventArgs e)
        {
            Step2DenyList.Visible = true;
        }

        private void Step2DenyList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (Step2DenyList.RowCount - e.RowIndex).ToString("N0");
            }


            if (e.ColumnIndex == 3)
            {
                // if (CurrentTrade..CheckPayState == "미결제")
                if (e.Value.ToString() == "미결제")
                {
                    //  e.Value = "미결제";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
                else
                {
                    // e.Value = "결제완료";
                    Step2DenyList[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
            }
            if (e.ColumnIndex == 4)
            {
                if (e.Value.ToString() == "아니오")
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

        class DenyedItem
        {
            public String DriverLoginId { get; set; }
            public String DriverCarNo { get; set; }
            public String CheckPayState { get; set; }
            public String CheckPayDate { get; set; }
            public String CheckClientAccId { get; set; }
            public String CheckCustomerAccId { get; set; }
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

        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;
        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            fileString = string.Format("취소결과") + DateTime.Now.ToString("yyyyMMdd");
            title = "취소결과";

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

    }
}
