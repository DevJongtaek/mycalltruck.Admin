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
using System.IO;
using OfficeOpenXml;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.UI;

namespace mycalltruck.Admin
{
    public partial class Dialog_BankCreate : Form
    {
        DriverRepository mDriverRepository = new DriverRepository();
        public String CarNo { get; set; }
        public Dialog_BankCreate()
        {
            InitializeComponent();
            clientsTableAdapter.FillByClientId(this.cmDataSet.Clients, LocalUser.Instance.LogInInformation.ClientId);

            Dictionary<string, string> PayBank = new Dictionary<string, string>();
           
            PayBank.Add("국민은행", "국민은행");
            PayBank.Add("우리은행", "우리은행");
            PayBank.Add("신한은행", "신한은행");
            PayBank.Add("기업은행", "기업은행");
            PayBank.Add("하나은행", "하나은행");
            PayBank.Add("농협", "농협");

            cmb_Bank.DataSource = new BindingSource(PayBank, null);
            cmb_Bank.DisplayMember = "Value";
            cmb_Bank.ValueMember = "Key";

            

            var Client = cmDataSet.Clients.FindByClientId(LocalUser.Instance.LogInInformation.ClientId);
           
        }

        public TradeDataSet.TradesBasKetRow[] Trades { get; set; }
        private TradeDataSet.TradesBasKetRow[] AcceptTrades { get; set; }
        private TradeDataSet.TradesBasKetRow[] AcceptTradesList { get; set; }

        private int CurrentStep = 1;
        private bool IsLoadClientAccs = false;
        private bool IsLoadStep2 = false;
        private bool IsLoadStep3 = false;
        private bool IsLoadStep4 = false;
        private bool NeedCheckCustomerAccLimite = false;
        private int Step5Tatal = 0;
        private int Step5Done = 0;
       
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
        
        private void btn_ShowDenyList_Click(object sender, EventArgs e)
        {
           
        }


        private void ShowStep3()
        {
            List<TradeDataSet.TradesBasKetRow> Denyed = new List<TradeDataSet.TradesBasKetRow>();
            List<TradeDataSet.TradesBasKetRow> Accepted = new List<TradeDataSet.TradesBasKetRow>();
            foreach (var trade in Trades)
            {
                if (trade.PayState == 1)
                {
                    Denyed.Add(trade);
                    continue;
                }

                if(trade.Amount == 0)
                {
                    Denyed.Add(trade);
                    continue;
                }
                if (trade.EtaxCanCelYN == "Y")
                {
                    Denyed.Add(trade);
                    continue;
                }
                

                Accepted.Add(trade);
            }

            AcceptTrades = Accepted.ToArray();
            AcceptTradesList = Accepted.ToArray();


            CurrentStep = 3;

            // pnStep2.Visible = false;
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
           // btn_AddList.Visible = false;
            if (AcceptTrades.Count() > 0)
            {

                if (!IsLoadStep3)
                {
                    BindingList<TradeDataSet.TradesBasKetRow> AcceptedItemBindingList = new BindingList<TradeDataSet.TradesBasKetRow>(AcceptTrades);
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

                        AcceptTrades = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true ).Select(c => c.DataBoundItem as TradeDataSet.TradesBasKetRow).ToArray();


                        lbl_TotalMoney.Text = " ■ 총 이체금액 : " + AcceptTrades.Where(c => c.DriverBizNo != "" && c.PayAccountNo != "").Sum(c => c.Amount).ToString("N0") + " 원";
                        //Step4TotalCount.Text = AcceptTrades.Count().ToString();

                        if(AcceptTrades.Where(c => c.DriverBizNo != "" && c.PayAccountNo != "").Sum(c => c.Amount) == 0)
                        {
                            btn_Next.Enabled = false;
                        }
                        else
                        {
                            btn_Next.Enabled = true;
                        }

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

                            AcceptTrades = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as TradeDataSet.TradesBasKetRow).ToArray();


                            lbl_TotalMoney.Text = " ■ 총 결제금액 : " + AcceptTrades.Where(c=> c.DriverBizNo != "" && c.PayAccountNo != "").Sum(c => c.Amount).ToString("N0") + " 원";
                            // Step4TotalCount.Text = AcceptTrades.Count().ToString();
                            btn_Next.Enabled = true;
                        }
                        else
                        {
                            for (int i = 0; i < Step3List.RowCount; i++)
                            {
                                Step3List[Step3Checkbox.Index, i].Value = false;

                            }
                            lbl_TotalMoney.Text = " ■ 총 결제금액 : " + "0";
                            btn_Next.Enabled = false;
                            // Step4TotalCount.Text = AcceptTrades.Count().ToString();
                        }
                    };

                    Step3List.CellPainting += (sender, e) =>
                    {
                        if (e.RowIndex < 0)
                            return;
                        var Selected = AcceptTradesList[e.RowIndex];

                        if (Selected.DriverBizNo == "" || Selected.PayAccountNo == "")
                        {

                            Step3List.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        }
                    };

                    Step3AllSelect.Checked = true;
                    IsLoadStep3 = true;
                }
            }
            else
            {
                btn_Next.Enabled = false;

            }
        }
      

        private void ShowStep4()
        {
            CurrentStep = 4;

            //pnStep2.Visible = false;
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
            btn_AddList.Visible = false;




            if (!IsLoadStep4)
            {
                //Step4TotalAmount.Text = lbl_TotalMoney.Text;


                IsLoadStep4 = true;







            }

            btn_Next.Enabled = true;


           


        }

        private void Step4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void ShowStep5()
        {
            CurrentStep = 5;

            //pnStep2.Visible = false;
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
            btn_AddList.Visible = false;
            AcceptTrades = Step3List.Rows.Cast<DataGridViewRow>().Where(c => (bool)Step3List[Step3Checkbox.Index, c.Index].Value == true).Select(c => c.DataBoundItem as TradeDataSet.TradesBasKetRow).ToArray();
            AcceptTrades = AcceptTrades.Where(c => c.PayAccountNo != "" && c.DriverBizNo != "").ToArray();
            Step5Tatal = AcceptTrades.Length;
            Step5Progress.Maximum = Step5Tatal;
            Step5Progress.Value = Step5Done;

            Step5Display(true);
            
            Step5Work();
        }
        private void ExcelExportBasic()
        {
            
            //if (newDGV1.RowCount == 0)
            //{
            //    MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYBANKNEW);

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = cmb_Bank.Text + "_" + DateTime.Now.ToString("yyyyMMdd") +"_UP";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName+".xls");
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                { 
                    return;
                }
                else
                {
                    file.Delete();

                }
            }


           // ExcelPackage _Excel = null;
            // MemoryStream _Stream = null;
            string title = string.Empty;
            byte[] ieExcel;

           
            switch (cmb_Bank.Text)
            {
                case "국민은행":

                    title = "대량이체";
                    ieExcel = Properties.Resources.국민_대량이체_업로드;


                    pnProgress.Visible = true;
                    bar.Value = 0;
                    Thread t = new Thread(new ThreadStart(() =>
                    {

                      //  newDGV1.ExportExistExcelBank(title, fileString, bar, true, ieExcel, 1, LocalUser.Instance.PersonalOption.MYBANKNEW);
                   
                     

                        newDGV1.ExportExistExcelBank2(newDGV7, title, fileString, bar, true, ieExcel, 1,8, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        pnProgress.Invoke(new Action(() => btn_Next.Enabled = true));


                    }));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    break;
                case "우리은행":

                    title = "Sheet1";
                    ieExcel = Properties.Resources.우리_대량이체_업로드;


                    pnProgress.Visible = true;
                    bar.Value = 0;
                    Thread j = new Thread(new ThreadStart(() =>
                    {

                      //  newDGV2.ExportExistExcelBank(title, fileString, bar, true, ieExcel, 1, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        newDGV2.ExportExistExcelBank2(newDGV7, title, fileString, bar, true, ieExcel, 1, 8, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        pnProgress.Invoke(new Action(() => btn_Next.Enabled = true));




                    }));
                    j.SetApartmentState(ApartmentState.STA);
                    j.Start();







                    break;
                case "기업은행":
                    title = "Sheet1";
                    ieExcel = Properties.Resources.기업은행_대량이체_업로드;


                    pnProgress.Visible = true;
                    bar.Value = 0;
                    Thread k = new Thread(new ThreadStart(() =>
                    {

                        //newDGV3.ExportExistExcelBank(title, fileString, bar, true, ieExcel, 1, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        newDGV3.ExportExistExcelBank2(newDGV7, title, fileString, bar, true, ieExcel, 1, 8, LocalUser.Instance.PersonalOption.MYBANKNEW);

                        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        pnProgress.Invoke(new Action(() => btn_Next.Enabled = true));




                    }));
                    k.SetApartmentState(ApartmentState.STA);
                    k.Start();
                    break;
                case "신한은행":
                    title = "Sheet1";
                    ieExcel = Properties.Resources.신한은행_대량이체_업로드;


                    pnProgress.Visible = true;
                    bar.Value = 0;
                    Thread m = new Thread(new ThreadStart(() =>
                    {

                        // newDGV6.ExportExistExcelBank(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        newDGV6.ExportExistExcelBank2(newDGV7, title, fileString, bar, true, ieExcel, 2, 8, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        pnProgress.Invoke(new Action(() => btn_Next.Enabled = true));




                    }));
                    m.SetApartmentState(ApartmentState.STA);
                    m.Start();
                    break;

                case "하나은행":
                    title = "Sheet1";
                    ieExcel = Properties.Resources.하나_대량이체_업로드;


                    pnProgress.Visible = true;
                    bar.Value = 0;
                    Thread q = new Thread(new ThreadStart(() =>
                    {

                        //newDGV4.ExportExistExcelBank(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        newDGV4.ExportExistExcelBank2(newDGV7, title, fileString, bar, true, ieExcel, 2, 8, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        pnProgress.Invoke(new Action(() => btn_Next.Enabled = true));




                    }));
                    q.SetApartmentState(ApartmentState.STA);
                    q.Start();
                    break;
                case "농협":
                    title = "Sheet1";
                    ieExcel = Properties.Resources.농협_대량이체_업로드;


                    pnProgress.Visible = true;
                    bar.Value = 0;
                    Thread w= new Thread(new ThreadStart(() =>
                    {

                        //newDGV5.ExportExistExcelBank(title, fileString, bar, true, ieExcel, 1, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        newDGV5.ExportExistExcelBank2(newDGV7, title, fileString, bar, true, ieExcel, 1, 8, LocalUser.Instance.PersonalOption.MYBANKNEW);
                        pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        pnProgress.Invoke(new Action(() => btn_Next.Enabled = true));




                    }));
                    w.SetApartmentState(ApartmentState.STA);
                    w.Start();
                    break;
            }


            try
            {

                //_Excel.SaveAs(file);
               

                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    foreach (var Tradeid in AcceptTrades)
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "UPDATE Trades SET ExcelExportYN = @ExcelExportYN WHERE TradeId = @TradeId";
                            _Command.Parameters.AddWithValue("@ExcelExportYN", DateTime.Now.ToString("d").Replace("-", "/"));
                            _Command.Parameters.AddWithValue("@TradeId", Tradeid.TradeId);
                            _Command.ExecuteNonQuery();
                        }
                    }
                    _Connection.Close();

                    Step5Complete();
                    //System.Diagnostics.Process.Start(FileName + ".xls");
                }

            }
            catch (Exception ex)
            {
              
               MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }

           

            try { }
            catch
            {

               
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);
            }
           
        }

       


        private void Step5Work()
        {
            if (IsClosed)
                return;

            try
            {
               
                ExcelExportBasic();
                btn_BankHome.Text = cmb_Bank.Text + " 홈페이지";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }

           
        }
        

        private void Step5Complete()
        {
            Step5Info.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            Step5Info.Font = new Font(Step5Info.Font, FontStyle.Bold);
            Step5Info.Text = "내보내기 완료 되었습니다.";
            btn_Next.Text = "완료";
            btn_Close.Enabled = false;
            //btn_Next.Enabled = true;
        }

        BindingList<BankExport> AccLogsItemBindingList = new BindingList<BankExport>();
        BindingList<BankExportPayment> PaymentBindingList = new BindingList<BankExportPayment>();
        private void Step5Display(bool first, int _TradeId = 0)
        {
            newDGV1.DataSource = null;
            switch (cmb_Bank.Text)
            {
                case "국민은행":
                    newDGV1.Visible = true;
                    newDGV2.Visible = false;
                    newDGV3.Visible = false;
                    newDGV4.Visible = false;
                    newDGV5.Visible = false;
                    newDGV6.Visible = false;
                    foreach (var trade in AcceptTrades)
                    {
                        var Added = new BankExport
                        {
                            BankCode = trade.PayBankCode,
                            BankName = trade.PayBankName,
                            PayAccountNo = trade.PayAccountNo,
                            Amount = trade.Amount.ToString("N0"),
                            DriverName = trade.PayInputName,
                            Remark = trade.ClientName,
                          
                        };
                        AccLogsItemBindingList.Add(Added);
                    }
                    Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptTrades.Count());

                    newDGV1.AutoGenerateColumns = false;
                    newDGV1.DataSource = AccLogsItemBindingList;
                    Step5Progress.Value = Step5Done;
                   
                    break;

                case "우리은행":

                    newDGV1.Visible = false;
                    newDGV2.Visible = true;
                    newDGV3.Visible = false;
                    newDGV4.Visible = false;
                    newDGV5.Visible = false;
                    newDGV6.Visible = false;
                    foreach (var trade in AcceptTrades)
                    {
                        var Added = new BankExport
                        {
                            
                            BankName = trade.PayBankName,
                            PayAccountNo = trade.PayAccountNo,
                            Amount = trade.Amount.ToString("N0"),
                            DriverName = trade.PayInputName,
                            Remark = trade.ClientName,

                        };
                        AccLogsItemBindingList.Add(Added);
                    }
                    Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptTrades.Count());

                    newDGV2.AutoGenerateColumns = false;
                    newDGV2.DataSource = AccLogsItemBindingList;
                    Step5Progress.Value = Step5Done;

                   
                    break;
                case "기업은행":

                    newDGV1.Visible = false;
                    newDGV2.Visible = false;
                    newDGV3.Visible = true;
                    newDGV4.Visible = false;
                    newDGV5.Visible = false;
                    newDGV6.Visible = false;
                    foreach (var trade in AcceptTrades)
                    {
                        var Added = new BankExport
                        {

                            BankName = trade.PayBankName,
                            PayAccountNo = trade.PayAccountNo,
                            Amount = trade.Amount.ToString("N0"),
                            PayInputName = trade.PayInputName,
                            Remark = trade.ClientName,
                             
                        };
                        AccLogsItemBindingList.Add(Added);
                    }
                    Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptTrades.Count());

                    newDGV3.AutoGenerateColumns = false;
                    newDGV3.DataSource = AccLogsItemBindingList;
                    Step5Progress.Value = Step5Done;


                    break;
                case "하나은행":

                    newDGV1.Visible = false;
                    newDGV2.Visible = false;
                    newDGV3.Visible = false;
                    newDGV4.Visible = true;
                    newDGV5.Visible = false;
                    newDGV6.Visible = false;

                    foreach (var trade in AcceptTrades)
                    {
                        var Added = new BankExport
                        {

                            BankName = trade.PayBankName,
                            PayAccountNo = trade.PayAccountNo,
                            Amount = trade.Amount.ToString("N0"),
                            PayInputName = trade.PayInputName,
                            Remark = trade.ClientName,


                        };
                        AccLogsItemBindingList.Add(Added);
                    }
                    Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptTrades.Count());

                    newDGV4.AutoGenerateColumns = false;
                    newDGV4.DataSource = AccLogsItemBindingList;
                    Step5Progress.Value = Step5Done;


                    break;
                case "농협":

                    newDGV1.Visible = false;
                    newDGV2.Visible = false;
                    newDGV3.Visible = false;
                    newDGV4.Visible = false;
                    newDGV5.Visible = true;
                    newDGV6.Visible = false;

                    foreach (var trade in AcceptTrades)
                    {
                        var Added = new BankExport
                        {

                            BankCode = trade.PayBankCode,
                            PayAccountNo = trade.PayAccountNo,
                            Amount = trade.Amount.ToString("N0"),

                            Remark = trade.ClientName,

                        };
                        AccLogsItemBindingList.Add(Added);
                    }
                    Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptTrades.Count());

                    newDGV5.AutoGenerateColumns = false;
                    newDGV5.DataSource = AccLogsItemBindingList;
                    Step5Progress.Value = Step5Done;


                    break;

                case "신한은행":

                    newDGV1.Visible = false;
                    newDGV2.Visible = false;
                    newDGV3.Visible = false;
                    newDGV4.Visible = false;
                    newDGV5.Visible = false;
                    newDGV6.Visible = true;

                    foreach (var trade in AcceptTrades)
                    {
                        var Added = new BankExport
                        {

                            BankCode = trade.PayBankCode,
                            PayAccountNo = trade.PayAccountNo,
                            PayInputName = trade.PayInputName,
                            Amount = trade.Amount.ToString("N0"),
                            
                            Remark = trade.ClientName,
                            TradeId = trade.TradeId.ToString(),

                        };
                        AccLogsItemBindingList.Add(Added);
                    }
                    Step5TotalCount.Text = String.Format("{0:N0}  건", AcceptTrades.Count());

                    newDGV6.AutoGenerateColumns = false;
                    newDGV6.DataSource = AccLogsItemBindingList;
                    Step5Progress.Value = Step5Done;


                    break;
            }


            //newDGV1.Visible = false;
            //newDGV2.Visible = false;
            //newDGV3.Visible = false;
            //newDGV4.Visible = false;
            //newDGV5.Visible = false;
            //newDGV6.Visible = false;
            //newDGV7.Visible = true;
            int i = 1;
            foreach (var trade in AcceptTrades)
            {

                var Added = new BankExportPayment
                {
                    DTransportDate = trade.TransportDate.ToString("d"),
                    DStartState = trade.StartState,
                    DStopState = trade.StopState,
                    DCarNo = trade.DriverCarNo,
                    DBizNo = trade.DriverBizNo,
                    DPayInputName = trade.PayInputName,
                    DPayBankName = trade.PayBankName,
                    DPayAccountNo = trade.PayAccountNo,
                    DPrice = trade.Price.ToString("N0"),
                    DVAT = trade.VAT.ToString("N0"),
                    DAmount = trade.Amount.ToString("N0"),
                    TradeId = trade.TradeId,
                    DIdx = i++.ToString(),

                };
                
                PaymentBindingList.Add(Added);
            }


            var Added2 = new BankExportPayment
            {
                DTransportDate = "",
                DStartState = "",
                DStopState = "",
                DCarNo = "",
                DBizNo = "",
                DPayInputName = "",
                DPayBankName = "",
                DPayAccountNo = "",
                DPrice = AcceptTrades.Sum(c => c.Price).ToString("N0"),
                DVAT = AcceptTrades.Sum(c => c.VAT).ToString("N0"),
                DAmount = AcceptTrades.Sum(c => c.Amount).ToString("N0"),
                TradeId = 0,
                DIdx = "합계",
            };
            PaymentBindingList.Add(Added2);
            


            newDGV7.AutoGenerateColumns = false;
            newDGV7.DataSource = PaymentBindingList;

        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 2)
                ShowStep3();
            else if (CurrentStep == 3)
                ShowStep4();
            else if (CurrentStep == 4)
            {

                //if (MessageBox.Show("정말 카드결제를 하시겠습니까?", "카드결제 확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                if (MessageBox.Show("" + cmb_Bank.Text + " 데이터를 생성 하시겠습니까?\r※ “C:\\차세로\\대량이체” 폴더에 자동저장\r※ “Excel 97-2003 통합문서” 로 저장 됨.", "대량이체 확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    ShowStep5();

            }
            else if (CurrentStep == 5)
            {
                DialogResult = DialogResult.Yes;
                Close();
                


            }
        }

        private void btn_Pre_Click(object sender, EventArgs e)
        {
            if (CurrentStep == 4)
                ShowStep3();
            //else if (CurrentStep == 3)
            //    ShowStep2();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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

        class BankExport : INotifyPropertyChanged
        {
            private int _Id;
            private String _DriverLoginId;
            private String _DriverCarNo;
            private String _DriverName;
            private String _Amount;
            private String _BankName;
            private String _PayAccountNo;
            private String _BankCode;
            private String _Remark;
            private String _TradeId;
            private String _PayInputName;
            //

            public int Id { get { return _Id; } set { SetField(ref _Id, value); } }
            public string DriverLoginId { get { return _DriverLoginId; } set { SetField(ref _DriverLoginId, value); } }
            public string DriverCarNo { get { return _DriverCarNo; } set { SetField(ref _DriverCarNo, value); } }
            public string DriverName { get { return _DriverName; } set { SetField(ref _DriverName, value); } }
            public string Amount { get { return _Amount; } set { SetField(ref _Amount, value); } }
            public string BankName { get { return _BankName; } set { SetField(ref _BankName, value); } }
            public string PayAccountNo { get { return _PayAccountNo; } set { SetField(ref _PayAccountNo, value); } }
            public string BankCode { get { return _BankCode; } set { SetField(ref _BankCode, value); } }
            public string Remark { get { return _Remark; } set { SetField(ref _Remark, value); } }
            public string TradeId { get { return _TradeId; } set { SetField(ref _TradeId, value); } }
            public string PayInputName { get { return _PayInputName; } set { SetField(ref _PayInputName, value); } }
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

        class BankExportPayment : INotifyPropertyChanged
        {
            private int _Id;
            private String _DTransportDate;
            private String _DStartState;
            private String _DStopState;
            private String _DCarNo;
            private String _DBizNo;
            private String _DPayInputName;
            private String _DPayBankName;
            private String _DPayAccountNo;
            private String _DPrice;
            private String _DVAT;
            private String _DAmount;
            private int _TradeId;
            private string _DIdx;



            public int Id { get { return _Id; } set { SetField(ref _Id, value); } }
            public string DTransportDate { get { return _DTransportDate; } set { SetField(ref _DTransportDate, value); } }
            public string DStartState { get { return _DStartState; } set { SetField(ref _DStartState, value); } }
            public string DStopState { get { return _DStopState; } set { SetField(ref _DStopState, value); } }
            public string DCarNo { get { return _DCarNo; } set { SetField(ref _DCarNo, value); } }
            public string DBizNo { get { return _DBizNo; } set { SetField(ref _DBizNo, value); } }
            public string DPayInputName { get { return _DPayInputName; } set { SetField(ref _DPayInputName, value); } }
            public string DPayBankName { get { return _DPayBankName; } set { SetField(ref _DPayBankName, value); } }
            public string DPayAccountNo { get { return _DPayAccountNo; } set { SetField(ref _DPayAccountNo, value); } }
            public string DPrice { get { return _DPrice; } set { SetField(ref _DPrice, value); } }
            public string DVAT { get { return _DVAT; } set { SetField(ref _DVAT, value); } }
            public string DAmount { get { return _DAmount; } set { SetField(ref _DAmount, value); } }
            public int TradeId { get { return _TradeId; } set { SetField(ref _TradeId, value); } }
            public string DIdx { get { return _DIdx; } set { SetField(ref _DIdx, value); } }
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





       

        private void Dialog_AcceptAcc_Load(object sender, EventArgs e)
        {
            ShowStep3();
        }



      

        private void btn_BankHome_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process IEProcess = new System.Diagnostics.Process();
            IEProcess.StartInfo.FileName = "iexplore.exe";
            switch(cmb_Bank.Text)
            {
                case "국민은행":
                    IEProcess.StartInfo.Arguments = " https://www.kbstar.com/";
                    break;
                case "우리은행":
                    IEProcess.StartInfo.Arguments = " https://www.wooribank.com/";
                    break;
                case "신한은행":
                    IEProcess.StartInfo.Arguments = " https://www.shinhan.com/";
                    break;
                case "기업은행":
                    IEProcess.StartInfo.Arguments = " https://www.ibk.co.kr/";
                    break;
                case "하나은행":
                    IEProcess.StartInfo.Arguments = " https://www.kebhana.com/";
                    break;
                case "농협":
                    IEProcess.StartInfo.Arguments = " https://banking.nonghyup.com/";
                    break;

            }
            
            IEProcess.Start();
           
        }

        private void newDGV7_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == DIdx.Index)
            {
                //if (e.RowIndex == newDGV7.RowCount - 1)
                //{
                //    e.Value = "합계";
                //}
                //else
                //{
                //   // e.Value = (newDGV7.Rows.Count - e.RowIndex).ToString("N0");
                   
                //}
            }




        }

        private void Step3List_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void Step3List_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (Step3List.RowCount - e.RowIndex).ToString("N0");
            }
            if (e.ColumnIndex == Step3Checkbox.Index)
            {
                var _Row = AcceptTradesList[e.RowIndex];

                if (_Row.PayAccountNo == "" || _Row.DriverBizNo == "")
                {
                    var _Cell = Step3List[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = false;
                    _Cell.ReadOnly = true;
                }
                else
                {
                    var _Cell = Step3List[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = true;
                    _Cell.ReadOnly = false;
                }
            }
            else if (e.ColumnIndex == Step3CarYear.Index)
            {
                var _Row = AcceptTradesList[e.RowIndex];



                var _Driver = mDriverRepository.GetDriver(_Row.DriverCarNo);


                e.Value = _Driver.CarYear;
            }
        }

        private void Step3List_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;


            var _Row = AcceptTradesList[e.RowIndex];

            _Select(_Row.DriverCarNo);
        }

        private void _Select(string _CarNo)
        {
          
            CarNo = _CarNo;
        

            DialogResult = DialogResult.OK;
            Close();
        }
    }

}
