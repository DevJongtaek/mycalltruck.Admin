using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.Class.XML;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.UI;
using OfficeOpenXml;
using Popbill;
using Popbill.Taxinvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class;
using OfficeOpenXml.Style;

namespace mycalltruck.Admin
{
    public partial class FrmTrade : Form
    {
        int intTradeIds = 0;


        #region 전자세금계산서NiceDNR
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
        //string linkId = "edubillsys";
        #endregion


        private string LinkID = "edubill";
        //비밀키
        private string SecretKey = "0/hKQssE5RiQOEScVHzWGyITGsfqO2OyjhHCBqBE5V0=";
        private TaxinvoiceService taxinvoiceService;
        private const string CRLF = "\r\n";

        string TinfoNtsresult = "";
        string TinfoWriteDate = "";
        string TinfoitemKey = "";
        string TinfostateCode = "";
        string TinfostateName = "";

        string memo = "";
        internal PrintPreviewDialog PrintPreviewDialog1;
        private System.Drawing.Printing.PrintDocument document = new System.Drawing.Printing.PrintDocument();
        //private int curPageNumber;
        //private int curPageNumber_bak;
        bool HideAddTrade = false;
        bool Smsyn = false;
        DriverRepository mDriverRepository = new DriverRepository();

        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
            auth = this.GetAuth();
            switch (auth)
            {
                case MenuAuth.None:
                    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                    return;
                case MenuAuth.Read:

                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;
                    btn_New2.Enabled = false;
                    btnUpdatePayDate.Enabled = false;
                    btnCanclePayDate.Enabled = false;
                    btnAppSms.Enabled = false;
                    btnBankCreat.Enabled = false;
                    btnBankUpdate.Enabled = false;
                    btn_AcceptAcc.Enabled = false;
                    btn_CancelAcc.Enabled = false;

                    grid1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }

        public FrmTrade()
        {
            InitializeComponent();

            newDGV1.CellBeginEdit += NewDGV1_CellBeginEdit;

           


            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            _FormStyle.SetFormStyle(this, grid1);


            cmb_PayState.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            cmb_Date.SelectedIndex = 0;
            cmb_ClientSerach.SelectedIndex = 0;
            cmb_AllowAcc.SelectedIndex = 0;
            cmb_HasAcc_I.SelectedIndex = 0;
            cmb_LGD_Last_Function.SelectedIndex = 0;
            cmbBasKet.SelectedIndex = 0;
            dtp_From.Value = DateTime.Now;
            dtp_To.Value = DateTime.Now;
            _InitCmb();
            initReferralId();
            btn_AcceptAcc.Enabled = false;
            btn_CancelAcc.Enabled = false;
            btnAppSms.Enabled = false;
            btnBankCreat.Enabled = false;
            btnBankUpdate.Enabled = false;
            btn_Help.Enabled = false;
            // 운송사
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btnUpdate.Enabled = true;
                //label10.Visible = true;
                tableLayoutPanel5.Enabled = false;

                cmb_ClientSerach.Visible = false;
                txt_ClientSearch.Visible = false;
                ClientCode.Visible = false;
                ClientName.Visible = false;
                btn_AcceptAcc.Enabled = true;
                btn_CancelAcc.Enabled = true;
                btnAppSms.Enabled = true;

                btnBankCreat.Enabled = true;
                btnBankUpdate.Enabled = true;
                btn_Help.Enabled = true;

                btn_New2.Enabled = true;
                LocalUser.Instance.LogInInformation.LoadClient();
                if(LocalUser.Instance.LogInInformation.Client.CustomerPay)
                {
                    btn_AcceptAcc.Enabled = false;
                    btn_CancelAcc.Enabled = false;
                }
                clientsTableAdapter.Fill(clientDataSet.Clients);
                AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            }
            // 관리자
            else
            {
                btnUpdate.Enabled = false;
                btnCurrentDelete.Enabled = false;
                btnExcel.Enabled = true;
                // label10.Visible = false;
                // chk_PayState.Visible = false;
                tableLayoutPanel5.Enabled = false;
                btn_New.Visible = true;
                btn_New.Enabled = false;
                btn_New2.Enabled = false;
            }
            memo = "▶운송/주선사用 '차세로' PC프로그램 안내 운송/주선사에서는 http://www.chasero.co.kr 접속하여 프로그램을 설치하시면, 화물차주가 발행한 '전자세금계산서' 및 '첨부화일(인수증/송장 등)'을 조회해 보실 수 있습니다. 문의: 1661 - 6090 ";  // 즉시발행 메모 
                                                                                                                                                                             // InitDriverTable();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }
        private void initReferralId()
        {
            Dictionary<int, string> DCustomer = new Dictionary<int, string>();
            customersNewTableAdapter.Fill(clientDataSet.CustomersNew);
           
            var cmbCustomerMIdDataSource = clientDataSet.CustomersNew.Where(c => c.BizGubun == 4 && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.SangHo).ToArray();

            DCustomer.Add(0, "위탁사 전체");


            foreach (var item in cmbCustomerMIdDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }


            cmb_ReferralId.DataSource = new BindingSource(DCustomer, null);
            cmb_ReferralId.DisplayMember = "Value";
            cmb_ReferralId.ValueMember = "Key";
            cmb_ReferralId.SelectedValue = 0;
        }

        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();
        class DriverViewModel
        {
            public int DriverId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string BizNo { get; set; }
            public int candidateid { get; set; }
            public bool AppUse { get; set; }
            public string MobileNo { get; set; }
            public string CarNo { get; set; }
            public string CarYear { get; set; }
            public int CarSize { get; set; }
            public int CarType { get; set; }
            public string Password { get; set; }

            public string CEO { get; set; }
            public string AddressState { get; set; }
            public string AddressCity { get; set; }
            public string AddressDetail { get; set; }

            public string Upjong { get; set; }
            public string Uptae { get; set; }
            public string Email { get; set; }
            public string PhoneNo { get; set; }
          

      


        }


        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _DriverTable.Clear();
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,ISNULL(MobileNo,N'')as MobileNo,CarNo,ISNULL(CarYear,'N')CarYear,CarSize,CarType,Password,isnull(CEO,N'') as CEO,isnull(AddressState,N'')as AddressState,ISNULL(AddressCity,N'')as AddressCity,ISNULL(AddressDetail,N'')as AddressDetail,ISNULL(Upjong,N'')as Upjong,ISNULL(Uptae,N'')as Uptae,ISNULL(Email,N'')as Email,ISNULL(PhoneNo,N'')as PhoneNo FROM Drivers with (nolock)";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _DriverTable.Add(
                              new DriverViewModel
                              {
                                  DriverId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                                  BizNo = dataReader.GetString(4),
                                  candidateid = dataReader.GetInt32(5),
                                  AppUse = dataReader.GetBoolean(6),
                                  MobileNo = dataReader.GetString(7),
                                  CarNo = dataReader.GetString(8),
                                  CarYear = dataReader.GetString(9),
                                  CarSize = dataReader.GetInt32(10),
                                  CarType = dataReader.GetInt32(11),
                                  Password = dataReader.GetString(12),
                                  CEO = dataReader.GetString(13),

                                  AddressState = dataReader.GetString(14),
                                  AddressCity = dataReader.GetString(15),
                                  AddressDetail = dataReader.GetString(16),

                                  Upjong = dataReader.GetString(17),
                                  Uptae = dataReader.GetString(18),
                                  Email = dataReader.GetString(19),
                                  PhoneNo = dataReader.GetString(20),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }


        
        private void NewDGV1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(e.ColumnIndex != Column1Price.Index)
            {
                e.Cancel = true;
            }
        }

        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        private void _InitCmb()
        {
            Dictionary<string, string> PayBank = new Dictionary<string, string>();
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

            if(!LocalUser.Instance.LogInInformation.IsAdmin && !LocalUser.Instance.LogInInformation.IsSubClient)
            {
                SubClientIdDictionary.Add(-1, "본지점구분");
                SubClientIdDictionary.Add(0, "본점");
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _AllowSubCommand = _Connection.CreateCommand())
                    {
                        _AllowSubCommand.CommandText = "SELECT AllowSub FROM Clients WHERE ClientId = @ClientId";
                        _AllowSubCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        var _AllowSub = (bool)_AllowSubCommand.ExecuteScalar();
                        if (_AllowSub)
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT Name, SubClientId FROM SubClients WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader())
                                {
                                    while (_Reader.Read())
                                    {
                                        SubClientIdDictionary.Add(_Reader.GetInt32(1), _Reader.GetString(0));
                                    }
                                }
                            }
                            cmb_SubClientId.DataSource = SubClientIdDictionary.ToList();
                            cmb_SubClientId.DisplayMember = "Value";
                            cmb_SubClientId.ValueMember = "Key";
                        }
                        else
                        {
                            SubClientId.Visible = false;
                            cmb_SubClientId.Visible = false;
                        }
                    }
                    _Connection.Close();
                }
            }
            else
            {
                SubClientId.Visible = false;
                cmb_SubClientId.Visible = false;
            }

            Dictionary<string, string> Smonth = new Dictionary<string, string>
            {
              
                { "당월", "당월" },
                { "전월", "전월" },
                { "지정", "지정" }
            };

            cmbSMonth.DataSource = new BindingSource(Smonth, null);
            cmbSMonth.DisplayMember = "Value";
            cmbSMonth.ValueMember = "Key";

            cmbSMonth.SelectedIndex = 2;
        }
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void txt_MobileNo_TextChanged(object sender, EventArgs e)
        {

        }
     

        private void FrmTrade_Load(object sender, EventArgs e)
        {
            try
            {
               
                //전자세금계산서 모듈 초기화
                taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

                //연동환경 설정값, 테스트용(true), 상업용(false)
                taxinvoiceService.IsTest = false;
                //DriverRepository mDriverRepository = new DriverRepository();
                //mDriverRepository.Select(baseDataSet.Drivers);
             
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    LocalUser.Instance.LogInInformation.LoadClient();
                    HideAddTrade = LocalUser.Instance.LogInInformation.Client.HideAddTrade;
                    Smsyn = LocalUser.Instance.LogInInformation.Client.SmsYn;
                    btn_New.Enabled = true;
                    btn_New2.Enabled = true;
                }

                button1_Click(null,null);
              //  btn_Search_Click(null, null);

                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    ApplyAuth();
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        string fileString = string.Empty;
        string title = string.Empty;
        string FolderPath = string.Empty;


        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {

            ExcelDialogMessageBox printDialog = new ExcelDialogMessageBox();

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);

            if (di.Exists == false)
            {
                di.Create();
            }


            ExcelPackage _Excel = null;




            var fileString = "매입정보_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.CustomerOrderList))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            var ColumnIndexMap = new Dictionary<int, int>();

            List<string> ColumnHeaderMap = new List<string>();
            var ColumnIndex = 0;
            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { CheckBox, SelectTax,ShowTaxOut,ShowTax, Edocument }.Contains(grid1.Columns[i])))
                {
                    ColumnIndexMap.Add(ColumnIndex, i);
                    ColumnIndex++;
                }
            }

            for (int i = 0; i < grid1.ColumnCount; i++)
            {
                if (grid1.Columns[i].Visible && !(new DataGridViewColumn[] { CheckBox, SelectTax, ShowTaxOut, ShowTax , Edocument }.Contains(grid1.Columns[i])))
                {
                    ColumnHeaderMap.Add(grid1.Columns[i].HeaderText);
                }

            }


            for (int i = 0; i < ColumnHeaderMap.Count; i++)
            {

                _Sheet.Cells[1, i + 1].Value = ColumnHeaderMap[i];

                _Sheet.Cells[RowIndex, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                _Sheet.Cells[RowIndex, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            }
            for (int i = 0; i < grid1.RowCount; i++)
            {
                for (int j = 0; j < ColumnIndexMap.Count; j++)
                {

                    _Sheet.Cells[RowIndex, j + 1].Value = grid1[ColumnIndexMap[j], i].FormattedValue?.ToString();

                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    _Sheet.Cells[RowIndex, j + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    _Sheet.Cells[1, j + 1].AutoFitColumns();
                }

                RowIndex++;
            }


            try
            {

                _Excel.SaveAs(file);
            }
            catch (Exception)
            {
                MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                return;
            }

            System.Diagnostics.Process.Start(FileName);


            //DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            //if (di.Exists == false)
            //{

            //    di.Create();
            //}
            //var fileString = "매입양식" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //FileInfo file = new FileInfo(FileName);
            //if (file.Exists)
            //{
            //    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //        return;
            //}

            //ExcelPackage _Excel = null;
            //using (MemoryStream _Stream = new MemoryStream(Properties.Resources.지입차일괄역발행양식_Blank))
            //{
            //    _Stream.Seek(0, SeekOrigin.Begin);
            //    _Excel = new ExcelPackage(_Stream);
            //}
            //var _Sheet = _Excel.Workbook.Worksheets[1];
            //var RowIndex = 2;
            //for (int i = 0; i < grid1.RowCount; i++)
            //{
            //    _Sheet.Cells[RowIndex, 1].Value = grid1[tradeIdDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 2].Value = grid1[ColumnPayState.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 3].Value = grid1[TransportDate.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 4].Value = grid1[requestDateDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 5].Value = grid1[ColumnHasAcc.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 6].Value = grid1[Code.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 7].Value = grid1[ServiceState.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 8].Value = grid1[CarNo.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 9].Value = grid1[bizNoDataGridViewTextBoxColumn.Index, i].FormattedValue;


            //    _Sheet.Cells[RowIndex, 10].Value = grid1[StartState.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 11].Value = grid1[StopState.Index, i].FormattedValue;

            //    _Sheet.Cells[RowIndex, 12].Value = grid1[nameDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 13].Value = grid1[DriverPhoneNo.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 14].Value = grid1[priceDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 15].Value = grid1[vATDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 16].Value = grid1[amountDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 17].Value = grid1[ColumnAcceptCount.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 18].Value = grid1[ColumnOrderText.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 19].Value = grid1[payBankNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 20].Value = grid1[payAccountNoDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 21].Value = grid1[payInputNameDataGridViewTextBoxColumn.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 22].Value = grid1[LGD_Last_Function.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 23].Value = grid1[ColumnLGD_Last_Date.Index, i].FormattedValue;
            //    _Sheet.Cells[RowIndex, 24].Value = grid1[ColumnHasETax.Index, i].FormattedValue;
            //    RowIndex++;
            //}

            //try
            //{
            //    _Excel.SaveAs(file);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("엑셀 파일을 저장 할 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
            //    return;
            //}
            //System.Diagnostics.Process.Start(FileName);
        }

        private void btnAllPrint_Click(object sender, EventArgs e)
        {
            PrintDialogMessageBox printDialog = new PrintDialogMessageBox();
            if (printDialog.ShowDialog() == DialogResult.Yes)
            {
                List<int> TradeIdList = new List<int>();
                for (int i = 0; i < grid1.Rows.Count; i++)
                {
                    if (((DataGridViewDisableCheckBoxCell)grid1[SelectTax.Index, i]).Enabled && grid1[SelectTax.Index, i].Value != null && (bool)grid1[SelectTax.Index, i].Value == true)
                    {
                        var _Model = (grid1.Rows[i].DataBoundItem as DataRowView).Row as TradeDataSet.TradesRow;
                        TradeIdList.Add(_Model.TradeId);
                    }
                }
                FrmTax f = new FrmTax(TradeIdList.ToArray(), "New");
                f.StartPosition = FormStartPosition.CenterScreen;
                f.WindowState = FormWindowState.Maximized;
                f.PrintOut();
                f.ShowDialog();

            }
            else if(printDialog.DialogResult == DialogResult.OK)
            {
                List<int> TradeIdList = new List<int>();
                for (int i = 0; i < grid1.Rows.Count; i++)
                {
                    if (((DataGridViewDisableCheckBoxCell)grid1[SelectTax.Index, i]).Enabled && grid1[SelectTax.Index, i].Value != null && (bool)grid1[SelectTax.Index, i].Value == true)
                    {
                        var _Model = (grid1.Rows[i].DataBoundItem as DataRowView).Row as TradeDataSet.TradesRow;
                        TradeIdList.Add(_Model.TradeId);
                    }
                }
                FrmTax f = new FrmTax(TradeIdList.ToArray());
                f.StartPosition = FormStartPosition.CenterScreen;
                f.WindowState = FormWindowState.Maximized;
                f.PrintClient();
                f.ShowDialog();

               
            }
            else
            {
                

            }
        }
        public bool IsEmpty(int Index)
        {
            bool r = true;

            if (Index == 1)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage1);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 2)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage2);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 3)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage3);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 4)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage4);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 5)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage5);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 6)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage6);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 7)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage7);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 8)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage8);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 9)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage9);
                }
                catch (Exception)
                {
                }
            }
            else if (Index == 10)
            {
                try
                {
                    r = String.IsNullOrEmpty(TImage10);
                }
                catch (Exception)
                {
                }
            }


            return r;
        }
        int PrintIndex = 1;
        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            HttpClient hClient = new HttpClient();
            var Stream = hClient.GetStreamAsync(GetUrl(PrintIndex)).Result;
            var dImage = Image.FromStream(Stream);
            int Width = dImage.Width;
            int Height = dImage.Height;

            //e.Graphics.DrawImage(dImage, new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, Width, Height));


            if (dImage.Width > dImage.Height)
            {
                dImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            var width = dImage.Width;
            var height = dImage.Height;
            if (dImage.Width > e.MarginBounds.Width)
            {
                width = e.MarginBounds.Width;
                height = height * e.MarginBounds.Width / dImage.Width;
            }
            else if (dImage.Height > e.MarginBounds.Height)
            {
                height = e.MarginBounds.Height;
                width = width * e.MarginBounds.Height / dImage.Height;
            }
            //_e.Graphics.DrawImage(_b, 0, 0, width, height);
            e.Graphics.DrawImage(dImage, 50, 70, width, height);



            var Next = PrintIndex + 1;
            if (IsEmpty(Next))
            {
                e.HasMorePages = false;
            }
            else
            {
                e.HasMorePages = true;
                PrintIndex = Next;
            }
        }

        string TImage1 = "";
        string TImage2 = "";
        string TImage3 = "";
        string TImage4 = "";
        string TImage5 = "";
        string TImage6 = "";
        string TImage7 = "";
        string TImage8 = "";
        string TImage9 = "";
        string TImage10 = "";

        private string GetUrl(int Index)
        {
            var Host = "http://m.cardpay.kr";
            //var Host = "http://localhost";
            var Url = "";

            if (Index == 1)
            {
                Url = Host + TImage1;
            }
            else if (Index == 2)
            {
                Url = Host + TImage2;
            }
            else if (Index == 3)
            {
                Url = Host + TImage3;
            }
            else if (Index == 4)
            {
                Url = Host + TImage4;
            }
            else if (Index == 5)
            {
                Url = Host + TImage5;
            }
            else if (Index == 6)
            {
                Url = Host + TImage6;
            }
            else if (Index == 7)
            {
                Url = Host + TImage7;
            }
            else if (Index == 8)
            {
                Url = Host + TImage8;
            }
            else if (Index == 9)
            {
                Url = Host + TImage9;
            }
            else if (Index == 10)
            {
                Url = Host + TImage10;
            }

            return Url;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == CheckBox.Index)
            {
                grid1.RefreshEdit();
                var _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
              
                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                    var _Row = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (!BankBasKetList.Contains(_Row.TradeId))
                    {
                        BankBasKetList.Add(_Row.TradeId);
                    }
                   
                }
                else
                {
                    var _Row = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (BankBasKetList.Contains(_Row.TradeId))
                    {
                        BankBasKetList.Remove(_Row.TradeId);
                    }
                }

                lblBankBasketCount.Text = BankBasKetList.Count().ToString("N0");


            }
            if (e.ColumnIndex == SelectTax.Index)
            {
                grid1.RefreshEdit();
            }
            if (grid1.Columns[e.ColumnIndex] == ShowTax)
            {

                
                var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    //if (Selected.HasETax == true && Selected.AllowAcc == false)
                    //{


                    //string url = taxinvoiceService.GetEPrintURL("1148654906", MgtKeyType.TRUSTEE, Selected.trusteeMgtKey, "edubillsys");

                    //Process.Start(url);
                    //}
                    //else
                    //{

                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        var uOrder = ShareOrderDataSet.Orders.Where(c => c.TradeId == Selected.TradeId).Count();

                        if (Selected.VAT != 0 && Selected.SUMYN != 2)
                        {

                            FrmTax f = new FrmTax(new int[] { Selected.TradeId });

                            f.PrintClient();
                            f.StartPosition = FormStartPosition.CenterScreen;

                            f.ShowDialog();


                        }
                    }
                    //}
                }
            }

            if (grid1.Columns[e.ColumnIndex] == ShowTaxOut)
            {
                var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        var uOrder = ShareOrderDataSet.Orders.Where(c => c.TradeId == Selected.TradeId).Count();

                        if (Selected.VAT != 0 && Selected.SUMYN != 2 )
                        {

                            FrmTax f = new FrmTax(new int[] { Selected.TradeId },"New");

                            f.PrintOut();
                            f.StartPosition = FormStartPosition.CenterScreen;

                            f.ShowDialog();


                        }
                    }
                }
            }
            if (grid1.Columns[e.ColumnIndex] == ColumnImage1)
            {
                var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    if (!String.IsNullOrEmpty(Selected.Image1))
                    {
                        #region 인쇄 미리보기
                        // 
                        //     if (!String.IsNullOrEmpty(Selected.Image1))
                        //     {
                        //         TImage1 = Selected.Image1;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image2))
                        //     {
                        //         TImage2 = Selected.Image2;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image3))
                        //     {
                        //         TImage3 = Selected.Image3;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image4))
                        //     {
                        //         TImage4 = Selected.Image1;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image5))
                        //     {
                        //         TImage5 = Selected.Image5;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image6))
                        //     {
                        //         TImage6 = Selected.Image6;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image7))
                        //     {
                        //         TImage7 = Selected.Image7;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image8))
                        //     {
                        //         TImage8 = Selected.Image8;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image9))
                        //     {
                        //         TImage9 = Selected.Image9;
                        //     }
                        //     if (!String.IsNullOrEmpty(Selected.Image10))
                        //     {
                        //         TImage10 = Selected.Image10;
                        //     }


                        //     PrintIndex = 1;
                        //     PrintDocument pd = new PrintDocument();
                        //     pd.PrintPage += PrintPage;

                        //     PrintDialog pDialog = new PrintDialog();

                        //     PrintPreviewDialog ppDoc = new PrintPreviewDialog();

                        //     pDialog.Document = pd;
                        //     pDialog.AllowSomePages = true;
                        //     pDialog.ShowHelp = true;
                        //     pDialog.ShowNetwork = true;

                        //     ppDoc.Document = pd;
                        //     ppDoc.WindowState = FormWindowState.Normal;

                        //     ppDoc.Show();

                        #endregion

                        FormImages f = new FormImages(Selected);

                        f.Owner = this;
                        f.StartPosition = FormStartPosition.CenterParent;
                        f.ShowDialog();

                        btn_Search_Click(null, null);
                    }




                }
            }

            if (grid1.Columns[e.ColumnIndex] == ColumnAccLog)
            {
                var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    if (Selected.HasAcc)
                    {
                        DataTable mDataTable = new DataTable();

                        using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            SqlCommand Command = Connection.CreateCommand();
                            Command.CommandText = "SELECT AccLogId, AccFunction, AccLogs.BankName, CardNo, AccLogs.TradeId, AccLogs.CreateDate, LGD_TID, LGD_MID,AccLogs.LGD_OID, LGD_AMOUNT, LGD_RESPCODE, LGD_RESPMSG,drivers.MID FROM AccLogs join trades on AccLogs.TradeId = trades.TradeId join drivers on trades.driverid = drivers.DriverId Where AccLogs.TradeId = @TradeId Order by AccLogs.CreateDate DESC";
                            Command.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                            Connection.Open();
                            mDataTable.Load(Command.ExecuteReader());
                            Connection.Close();
                        }
                        Dialog_AccLogs d = new Dialog_AccLogs();
                        d.SetListDataSource(mDataTable);
                        d.ShowDialog();
                    }
                }
            }

            if (grid1.Columns[e.ColumnIndex] == Edocument)
            {
                var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    if (!String.IsNullOrEmpty(Selected.PdfFileName) && Selected.HasETax == true)
                    {


                        FrmEdocument f = new FrmEdocument(Selected);
                        f.ShowDialog();

                    }
                    else if (!String.IsNullOrEmpty(Selected.Image1) && Selected.HasETax == false)
                    {


                        FormImages f = new FormImages(Selected);
                        f.Owner = this;
                        f.StartPosition = FormStartPosition.CenterParent;
                        f.ShowDialog();

                        btn_Search_Click(null, null);

                    }


                }
            }

        }

        int GridIndex = 0;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정질문, "세금계산", 1), "선택항목 수정 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            tradesBindingSource.EndEdit();

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
           
            var _d = 0m;
            if (!decimal.TryParse(txt_Price.Text.Replace(",", ""), out _d) || !decimal.TryParse(txt_VAT.Text.Replace(",", ""), out _d) || !decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _d))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Price, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;

            }
            else
            {
                decimal _Pirce = Convert.ToDecimal(txt_Price.Text.Replace(",", ""));
                if(_Pirce > 10000000)
                {
                    MessageBox.Show("10,000,000 원 까지만 입력 가능합니다.", "매입관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    err.SetError(txt_Price,"10,000,000 원 까지만 입력 가능합니다.");
                    return;
                }

            }


            if (cmb_PayBankName.SelectedIndex == 0 || cmb_PayBankName.SelectedIndex == -1)
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


            //var PayState = 2;

            //if (chk_PayState.Checked == true)
            //{
            //    PayState = 1;
            //}

            if (tradesBindingSource.Current != null)
            {
                var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                   // Selected.PayState = PayState;
                    tradesBindingSource.EndEdit();

                    //if (Selected.EtaxCanCelYN == "Y" && Selected.HasETax)
                    //{
                    //    MessageBox.Show("취소건 은 수정할 수 없습니다", "삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    return;
                    //}


                    if (Selected.HasETax && Selected.PayState == 2)
                    {

                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                "UPDATE Trades SET  HasAcc = @HasAcc " +
                                ", PayDate = @PayDate " +
                                //", PayState = @PayState "+                              
                                ", TransportDate = @TransportDate " +
                                ", StartState = @StartState " +
                                ", StopState = @StopState " +
                                " WHERE TradeId = @TradeId";

                            //cmd.Parameters.AddWithValue("@PayState", chk_PayState);
                            cmd.Parameters.AddWithValue("@PayDate", dtp_PayDate.Value);
                            cmd.Parameters.AddWithValue("@TransportDate", dtpTransportDate.Value);
                            cmd.Parameters.AddWithValue("@StartState", txtStartState.Text);
                            cmd.Parameters.AddWithValue("@StopState", txtStopState.Text);
                      
                            if (cmb_HasAcc.SelectedIndex == 0)
                            {
                                cmd.Parameters.AddWithValue("@HasAcc", false);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@HasAcc", true);
                            }
                            cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();
                            cmd.CommandText =
                                "UPDATE Trades SET  RequestDate = @RequestDate" +
                                ", BeginDate = @BeginDate" +
                                ", EndDate = @EndDate" +
                                ", DriverName = @DriverName" +
                                ", Item = @Item " +
                                ", Price = @Price " +
                                ", VAT = @VAT " +
                                ", Amount = @Amount" +
                                ", PayBankName = @PayBankName" +
                                ", PayBankCode = @PayBankCode" +
                                ", PayAccountNo = @PayAccountNo" +
                                ", PayInputName = @PayInputName" +
                                ", ClientId = @ClientId" +
                                ", UseTax = @UseTax" +
                                ", HasAcc = @HasAcc " +
                                ", TransportDate = @TransportDate " +
                                ", StartState = @StartState " +
                                ", StopState = @StopState " +
                                //", AllowAcc = 1 "+
                                " WHERE TradeId = @TradeId";

                            cmd.Parameters.AddWithValue("@RequestDate", dtp_RequestDate.Value);
                            cmd.Parameters.AddWithValue("@BeginDate", dtp_BeginDate.Value);
                            cmd.Parameters.AddWithValue("@EndDate", dtp_EndDate.Value);
                            cmd.Parameters.AddWithValue("@DriverName",txt_DriverName.Text );
                            cmd.Parameters.AddWithValue("@Item", txt_Item.Text);
                            cmd.Parameters.AddWithValue("@Price", txt_Price.Text.Replace(",", ""));
                            cmd.Parameters.AddWithValue("@VAT", txt_VAT.Text.Replace(",", ""));
                            cmd.Parameters.AddWithValue("@Amount", txt_Amount.Text.Replace(",", ""));
                            cmd.Parameters.AddWithValue("@PayBankName", cmb_PayBankName.Text);
                            cmd.Parameters.AddWithValue("@PayBankCode", cmb_PayBankName.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@PayAccountNo", txt_PayAccountNo.Text);
                            cmd.Parameters.AddWithValue("@PayInputName", txt_PayInputName.Text);
                            cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                            cmd.Parameters.AddWithValue("@TransportDate", dtpTransportDate.Value);
                            cmd.Parameters.AddWithValue("@StartState", txtStartState.Text);
                            cmd.Parameters.AddWithValue("@StopState", txtStopState.Text);
                            cmd.Parameters.AddWithValue("@UseTax", true);
                            if (cmb_HasAcc.SelectedIndex == 0)
                            {
                                cmd.Parameters.AddWithValue("@HasAcc", false);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@HasAcc", true);
                            }
                            cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                }
            }
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "세금계산서", 1), "세금계산서 수정 성공");
            if (grid1.RowCount > 1)
            {
                GridIndex = tradesBindingSource.Position;
                btn_Search_Click(null, null);
                grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];
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
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
          
            cmb_PayState.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            dtp_From.Value = DateTime.Now;
            dtp_To.Value = DateTime.Now;
            txt_Search.Clear();

            cmb_ClientSerach.SelectedIndex = 0;
            txt_ClientSearch.Clear();
            cmb_Date.SelectedIndex = 0;
            cmb_HasAcc_I.SelectedIndex = 0;
            cmb_AllowAcc.SelectedIndex = 0;
            cmb_LGD_Last_Function.SelectedIndex = 0;
            cmbSMonth.SelectedIndex = 2;
            chk_EtaxCancle.Checked = false;
            cmbBasKet.SelectedIndex = 0;
            if (cmb_SubClientId.Items.Count > 0)
                cmb_SubClientId.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }
        bool MethodProcess = false;
        object Old = null;
        private void tradesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            MethodProcess = true;
            if (tradesBindingSource.Current == null)
            {
                //tableLayoutPanel5.Visible = false;
                // chk_PayState.Checked = false;
                btnUpdatePayDate.Enabled = false;
                btnCanclePayDate.Enabled = false;
                
            }
            else
            {
                if(Old != null && (Old as DataRow).RowState == DataRowState.Modified)
                {
                    (Old as DataRow).RejectChanges();
                }
                var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
                Old = Selected;
                if (Selected != null)
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "SELECT OrderId, AcceptTime, ISNULL(Customers.SangHo, N'') AS Customer, ISNULL(TradePrice, 0) as TradePrice, Item, StartState, StartCity, StartStreet,StopState, StopCity,StopStreet,ISNULL(cs.SangHo,N'') as ReferralName FROM ORDERS LEFT JOIN CUSTOMERS ON ORDERS.CustomerId = CUSTOMERS.CustomerId left JOIN Customers as Cs ON cs.CustomerId = orders.ReferralId WHERE TradeId = @TradeId";
                            _Command.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                            using (SqlDataReader _Reader = _Command.ExecuteReader())
                            {
                                
                                var _Table = new DataTable();
                                _Table.Load(_Reader);
                                ordersBindingSource.DataSource = _Table;
                            }
                        }
                        _Connection.Close();
                    }
                   // _DriverTable.Where(c => c.DriverId == Selected.DriverId);

                  //  var _Driver = _DriverTable.Where(c => c.DriverId == Selected.DriverId);

                    var _Driver = mDriverRepository.NoGetDriver(Selected.DriverId);
                    if (_Driver == null)
                    {
                        lbl_CarNo.Text = "";
                        lbl_DriverName.Text = "";
                        lbl_RequestDate.Text = "";
                        lbl_Date.Text = "";
                    }
                    else
                    {
                        lbl_CarNo.Text = _Driver.CarNo;
                        lbl_DriverName.Text = _Driver.CarYear;
                        lbl_RequestDate.Text = Selected.RequestDate.ToString("d").Replace("-", "/");
                        lbl_Date.Text = Selected.BeginDate.ToString("d").Replace("-", "/") + " - " + Selected.EndDate.ToString("d").Replace("-", "/");
                    }
                    rdb_Tax0.Checked = true;
                    txt_Price.ReadOnly = false;
                    txt_Amount.ReadOnly = true;
                    AllowAcc.Checked = Selected.AllowAcc;
                    // 상태에 따라서 수정/삭제 가능 여부 변경
                    // 0. 카드는 기본정보를 수정할 수 없다.
                    // 1. 결제 건은 수정/삭제가 안된다.
                    // 2. 현금은 결제여부와 결제일자 변경이 가능하다.
                    // 3. 결제여부와 결제일자는 수정버튼을 누르지 않고 변경하도록 하자.
                    // 4. 외부계산서 체크는 결제여부와 관련없이 언제나 변경가능(수정버튼 누르지 않고 수정)
                    //카드
                    if (Selected.HasAcc)
                    {
                        // chk_PayState.Enabled = false;
                        tableLayoutPanel5.Enabled = false;

                        dtp_PayDate.Enabled = false;
                      //  btnUpdatePayDate.Enabled = false;
                        cmb_HasAcc.SelectedIndex = 1;
                        btnUpdatePayDate.Enabled = false;
                        btnCanclePayDate.Enabled = false;

                    }
                    //현금
                    else
                    {
                        tableLayoutPanel5.Enabled = true;
                        // chk_PayState.Enabled = true;
                        dtp_PayDate.Enabled = true;
                      //  btnUpdatePayDate.Enabled = true;
                        cmb_HasAcc.SelectedIndex = 0;

                        btnUpdatePayDate.Enabled = true;
                        btnCanclePayDate.Enabled = true;


                    }

                    //결제
                    if (Selected.PayState == 1)
                    {

                        btnCurrentDelete.Enabled = false;
                        btnUpdate.Enabled = false;

                        //카드
                        if (Selected.HasAcc)
                        {
                            tableLayoutPanel5.Enabled = false;
                        }
                        //현금
                        else
                        {
                            dtp_PayDate.Enabled = false;
                            tableLayoutPanel5.Enabled = true;
                            btnCanclePayDate.Enabled = true;
                            btnUpdatePayDate.Enabled = false;

                            txtPayAmount.Text = Selected.Amount.ToString("N0");
                            txtPayDate.Text = Selected.PayDate.ToString("d");
                        }
                        // chk_PayState.Checked = true;
                        // tableLayoutPanel5.Visible = true;

                        //lbl_PayDate.Visible = true;
                        //dtp_PayDate.Visible = true;
                        //  btnUpdatePayDate.Visible = true;

                        newDGV1.ReadOnly = true;

                        dtp_BeginDate.Enabled = false;
                        dtp_EndDate.Enabled = false;
                        txt_Item.Enabled = false;
                        dtp_RequestDate.Enabled = false;
                        txt_Price.Enabled = false;
                        rdb_Tax0.Enabled = false;
                        rdb_Tax1.Enabled = false;
                        txt_Amount.Enabled = false;
                        cmb_HasAcc.Enabled = false;

                        if (Selected.HasETax)
                        {
                            AllowAcc.Visible = false;

                        }
                        else
                        {
                            AllowAcc.Visible = true;

                        }
                    }
                    else
                    {
                        btnCurrentDelete.Enabled = true;
                        btnUpdate.Enabled = true;

                        //카드
                        if (Selected.HasAcc)
                        {
                            tableLayoutPanel5.Enabled = false;
                        }
                        //현금
                        else
                        {
                            tableLayoutPanel5.Enabled = true;
                            btnUpdatePayDate.Enabled = true;
                            btnCanclePayDate.Enabled = false;
                            dtp_PayDate.Enabled = true;
                            dtp_PayDate.Value = DateTime.Now;
                            txtPayAmount.Text = "";
                            txtPayDate.Text = "";
                        }





                        // chk_PayState.Checked = false;
                        //tableLayoutPanel5.Visible = true;
                        //lbl_PayDate.Visible = false;
                        //dtp_PayDate.Visible = false;
                        //  btnUpdatePayDate.Visible = false;

                        if (Selected.HasETax)
                        {

                            newDGV1.ReadOnly = false;

                            dtp_BeginDate.Enabled = false;
                            dtp_EndDate.Enabled = false;
                            txt_Item.Enabled = false;
                            dtp_RequestDate.Enabled = false;
                            txt_Price.Enabled = false;
                            rdb_Tax0.Enabled = false;
                            rdb_Tax1.Enabled = false;
                            txt_Amount.Enabled = false;
                            cmb_HasAcc.Enabled = true;
                            //  AllowAcc.Enabled =false;
                            AllowAcc.Visible = false;
                        }
                        else
                        {



                            newDGV1.ReadOnly = false;

                            dtp_BeginDate.Enabled = true;
                            dtp_EndDate.Enabled = true;
                            txt_Item.Enabled = true;
                            dtp_RequestDate.Enabled = true;
                            txt_Price.Enabled = true;
                            rdb_Tax0.Enabled = true;
                            rdb_Tax1.Enabled = true;
                            txt_Amount.Enabled = true;
                            cmb_HasAcc.Enabled = true;
                            AllowAcc.Visible = true;
                            //AllowAcc.Enabled = true;
                        }

                    }

                   
                    
                    //label83.Text = Selected.DriverName;
                    txt_BizNo.Text = Selected.DriverBizNo;
                    txt_Name.Text = Selected.DriverName;
                    txt_Price.Text = ((long)Selected.Price).ToString("N0");
                    txt_VAT.Text = ((long)Selected.VAT).ToString("N0");
                    txt_Amount.Text = ((long)Selected.Amount).ToString("N0");
                    if (!Selected.HasAcc)
                    {
                        if(Selected.PayState != 1)
                        {
                            dtp_PayDate.Value = DateTime.Now;
                        }
                        else
                        {
                            dtp_PayDate.Value = Selected.PayDate;
                        }
                    }

                    // var Query = _DriverTable.Where(c => c.DriverId == Selected.DriverId).ToArray();
                    //var _Driver = mDriverRepository.NoGetDriver(Selected.DriverId);
                    //if (Query.Any())
                    //{
                    //    txt_DriverName.Text = Query.First().CarYear;
                    //}
                   
                        txt_DriverName.Text = _Driver.CarYear;
                   
                    intTradeIds = Selected.TradeId;
                    //acceptInfoesBindingSource.Filter = " TradeId = '" + intTradeIds + "'";
                    newDGV2Binding(Selected.TradeId);
                    if(Selected.SourceType == 1)
                    {
                        txt_Price.ReadOnly = true;
                    }
                    else
                    {
                        txt_Price.ReadOnly = false;
                    }


                    //if(Selected.HasETax)
                    //{
                    //    btnUpdate.Enabled = false;

                    //}
                }
                else
                {
                    btnUpdatePayDate.Enabled = false;
                    btnCanclePayDate.Enabled = false;
                    //tableLayoutPanel5.Visible = false;
                    //chk_PayState.Checked = false;
                }
            }

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btnCurrentDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }

            MethodProcess = false;

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }
        private int UpdateDB()
        {
            return 0;
            Int64 sPrice = 0;
            Int64 Price = 0;
            int _rows = 0;
            try
            {
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
                //if (LocalUser.Instance.LogInInfomation.UseItemJisa)
                //    SingleDataSet.Instance.TblItemTableAdapter.FillJisa(SingleDataSet.Instance.TblItem, LocalUser.Instance.LogInInfomation.JisaCode);
                //else
                //    SingleDataSet.Instance.TblItemTableAdapter.Fill(SingleDataSet.Instance.TblItem);
                _rows = 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "상품정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }
        private void chk_PayState_CheckedChanged(object sender, EventArgs e)
        {
            //if (tradesBindingSource.Current == null)
            //    return;
            //var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            //if (Selected == null)
            //    return;
            //if (Selected != null)
            //{
            //    if (Selected.PayState != 1)
            //    {
            //        dtp_PayDate.Value = DateTime.Now;
            //    }
            //}
            //if (!MethodProcess)
            //{
            //    Selected.RejectChanges();
            //    if (Selected.HasAcc)
            //        return;
            //    var PayState = 2;
            //    if (chk_PayState.Checked == true)
            //    {
            //        PayState = 1;
            //    }
            //    // 여기서 데이터 적용한다.
            //    var TradeId = Selected.TradeId;
            //    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //    {
            //        cn.Open();
            //        SqlCommand cmd = cn.CreateCommand();
            //        cmd.CommandText =
            //            "UPDATE Trades SET  PayState = @PayState " +
            //            "WHERE TradeId = @TradeId AND HasAcc = 0";
            //        cmd.Parameters.AddWithValue("@PayState", PayState);
            //        cmd.Parameters.AddWithValue("@TradeId", TradeId);
            //        int _R = cmd.ExecuteNonQuery();
            //        if (_R > 0)
            //        {
            //            if (PayState == 1)
            //            {
            //                cmd.CommandText =
            //                    @"UPDATE Orders
            //                SET DriverPayDate = CONVERT(NVARCHAR(10),GETDATE(),126)
            //                    ,DriverPayPrice = @DriverPayPrice
            //                    ,DriverPayVAT = @DriverPayVAT
            //                    ,UseCardPay = 0
            //                    ,DriverPoint = null
            //                WHERE TradeId = @TradeId";
            //                cmd.Parameters.AddWithValue("@DriverPayPrice", Selected.Price);
            //                cmd.Parameters.AddWithValue("@DriverPayVAT", Selected.VAT);
            //                cmd.ExecuteNonQuery();
            //            }
            //            else
            //            {
            //                cmd.CommandText =
            //                    @"UPDATE Orders
            //                SET DriverPayDate = null
            //                    ,DriverPayPrice = null
            //                    ,DriverPayVAT = null
            //                    ,UseCardPay = null
            //                    ,DriverPoint = null
            //                WHERE TradeId = @TradeId";
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //        cn.Close();
            //    }
            //    LoadTableOne(TradeId);
            //}
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;

            if (grid1.Columns[e.ColumnIndex] == CheckBox)
            {
                if (Selected.PayState == 1 && !Selected.HasAcc)
                {
                    var _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = false;
                    _Cell.ReadOnly = true;
                }
                else
                {
                    var _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = true;
                    _Cell.ReadOnly = false;

                }

                if (BankBasKetList.Contains(Selected.TradeId))
                {
                    e.Value = true;


                }

            }
            else if (e.ColumnIndex == SelectTax.Index)
            {
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    var uOrder = ShareOrderDataSet.Orders.Where(c => c.TradeId == Selected.TradeId).Count();

                    if (Selected.AllowAcc)
                    {
                        if (uOrder == 0)
                        {
                            var _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                            _Cell.Enabled = false;
                            _Cell.ReadOnly = true;
                        }
                    }
                    else
                    {
                        var _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                        _Cell.Enabled = true;
                        _Cell.ReadOnly = false;
                    }
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == ColumnPayState)
            {
                // 결제된것은 무조건 결제완료
                if (Selected.PayState == 1)
                {
                    e.Value = "결제완료";
                    if (Selected.EtaxCanCelYN == "Y")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Gray;
                    }
                }
                else
                {
                    if (Selected.HasAcc)
                    {
                        if (Selected.ServiceState != 1)
                        {
                            e.Value = "심사중";
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else if (Selected.ServiceState == 1 && Selected.LGD_Last_Function != "승인")
                        {
                            e.Value = "결제가능";

                            if (Selected.EtaxCanCelYN == "Y")
                            {
                                e.CellStyle.ForeColor = Color.Red;
                            }
                            else
                            {
                                e.CellStyle.ForeColor = Color.Blue;
                            }
                        }
                    }
                    else
                    {
                        e.Value = "";
                    }
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == tradeIdDataGridViewTextBoxColumn)
            {
                e.Value = (grid1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (grid1.Columns[e.ColumnIndex] == ColumnHasAcc)
            {
                if (Selected.HasAcc)
                {
                    e.Value = "카드";

                    if (Selected.EtaxCanCelYN == "Y")
                    {
                        grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                    }


                }
                else
                {
                    e.Value = "현금";
                    grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == CarNo)
            {
                if (BankBasKetList.Contains(Selected.TradeId))
                {
                    grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
            }

            else if (grid1.Columns[e.ColumnIndex] == ColumnHasETax)
            {
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    var uOrder = ShareOrderDataSet.Orders.Where(c => c.TradeId == Selected.TradeId).Count();

                    if (uOrder > 0)
                    {
                        if (String.IsNullOrEmpty(Selected.TaxGubun))
                        {
                            e.Value = "외부계산서";
                        }
                        else
                        {
                            e.Value = Selected.TaxGubun;
                        }
                    }

                    if (Selected.AllowAcc)
                    {

                        if (uOrder > 0)
                        {
                            if (String.IsNullOrEmpty(Selected.TaxGubun))
                            {
                                e.Value = "외부계산서";
                            }
                            else
                            {
                                e.Value = Selected.TaxGubun;
                            }
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                    else
                    {
                        if (Selected.HasETax)
                        {
                           
                            e.Value = "전자";

                            if (uOrder > 0)
                            {
                                e.Value += "(배차)";

                            }
                            else
                            {
                                e.Value += "(작성발행)";
                            }

                            if (Selected.SourceType == 0)
                            {
                                e.Value = "전자(역발행)";

                            }
                        }


                    }
                }
                //역발행
                //if (Selected.SourceType == 0)
                //{
                //    //e.Value += "(역)";
                //    e.Value = "";
                //}
            }
            else if (grid1.Columns[e.ColumnIndex] == ColumnLGD_Last_Date)
            {
                try
                {
                    if (Selected.HasAcc && Selected.PayState == 1 && Selected.LGD_Last_Function != "")
                    {
                        e.Value = Selected.LGD_Last_Date.ToString("yyyy-MM-dd");
                    }
                    else if (Selected.PayState == 1)
                    {
                        e.Value = Selected.PayDate.ToString("yyyy-MM-dd");
                    }
                }
                catch (Exception)
                {
                    e.Value = "";
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == ExcelExportYN)
            {
                try
                {
                    e.Value = Selected.ExcelExportYN;
                }
                catch (Exception)
                {
                    e.Value = "";
                }
            }

            else if (grid1.Columns[e.ColumnIndex] == ServiceState)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ServiceState" && c.Value == Selected.ServiceState).ToArray();

                if (Query.Any())
                {
                    e.Value = Query.First().Name;
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == DriverPhoneNo)
            {
                String SMobileNo = string.Empty;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    var Command1 = cn.CreateCommand();
                    Command1.CommandText = "SELECT MobileNo  FROM Drivers where driverid = '" + Selected.DriverId + "'";
                    var o1 = Command1.ExecuteScalar();
                    if (o1 != null)
                    {
                        SMobileNo = o1.ToString();
                    }
                    cn.Close();
                }
                if (!String.IsNullOrWhiteSpace(SMobileNo))
                {
                    if (SMobileNo.Length == 11)
                    {
                        e.Value = SMobileNo.Substring(0, 3) + "-" + SMobileNo.Substring(3, 4) + "-" + SMobileNo.Substring(7, 4);
                    }
                    else if (SMobileNo.Length == 10)
                    {
                        e.Value = SMobileNo.Substring(0, 3) + "-" + SMobileNo.Substring(3, 3) + "-" + SMobileNo.Substring(6, 4);
                    }
                    else
                    {
                        e.Value = SMobileNo;
                    }
                }

            }
            else if (e.ColumnIndex == ColumnOrderText.Index)
            {
                if (Selected.SourceType == 1 && Selected.AcceptCount > 1)
                    e.Value = $"{Selected.BeginDate:yyyy/MM/dd}".Replace("-", "/") + "-" + $"{Selected.EndDate:yyyy/MM/dd}".Replace("-", "/");
                else
                    e.Value = "";
            }
            else if (e.ColumnIndex == SubClientId.Index)
            {
                if (SubClientIdDictionary.ContainsKey(Selected.SubClientId))
                    e.Value = SubClientIdDictionary[Selected.SubClientId];
            }
            else if (e.ColumnIndex == LGD_Last_Function.Index)
            {
                if (Selected.HasAcc)
                {
                    if (Selected.LGD_Last_Function == "승인")
                    {
                        e.Value = "승인";
                        e.CellStyle.ForeColor = Color.Blue;
                    }
                    else if (Selected.LGD_Last_Function == "취소")
                    {
                        e.Value = "취소";
                        e.CellStyle.ForeColor = Color.Red;
                    }
                }
                else
                {
                    if (Selected.PayState == 1)
                    {
                        e.Value = "현금";
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.Value = "";
                    }
                }
            }
            else if (e.ColumnIndex == HasETaxGubun.Index)
            {
                if (Selected.EtaxCanCelYN == "Y" && Selected.HasETax)
                {
                    if (Selected.Amount > 0)
                    {
                        e.Value = "발행취소";
                    }
                    else if (Selected.Amount < 0)
                    {
                        e.Value = "취소";
                    }


                }
                else if (Selected.EtaxCanCelYN == "N")
                {
                    e.Value = "외부발행";
                   

                }
                //else
                //{
                //    e.Value = "";
                //}

            }
            else if (grid1.Columns[e.ColumnIndex] == TransportDate)
            {
                e.Value = Selected.TransportDate.ToString("d").Replace("-", "/");
            }

            else if (grid1.Columns[e.ColumnIndex] == GroupName)
            {
                String SGroupName = string.Empty;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    var Command1 = cn.CreateCommand();
                    Command1.CommandText = "SELECT ISNULL(GroupName,'미설정') FROM  DriverInstances where Driverid = '" + Selected.DriverId + "' AND DriverInstances.ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                    var o1 = Command1.ExecuteScalar();
                    if (o1 != null)
                    {
                        SGroupName = o1.ToString();
                    }
                    cn.Close();
                }
                if (!String.IsNullOrWhiteSpace(SGroupName))
                {

                    e.Value = SGroupName;

                }
                else
                {

                    e.Value = "미설정";
                }

                if (BankBasKetList.Contains(Selected.TradeId))
                {
                    grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }

            }

            else if (grid1.Columns[e.ColumnIndex] == ColumnsReferralId)
            {
                String SangHo = string.Empty;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    var Command1 = cn.CreateCommand();
                    Command1.CommandText = "SELECT ISNULL(SangHo,'') FROM  Customers where CustomerId = '" + Selected.ReferralId + "' AND ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "";
                    var o1 = Command1.ExecuteScalar();
                    if (o1 != null)
                    {
                        SangHo = o1.ToString();
                    }
                    cn.Close();
                }
                if (!String.IsNullOrWhiteSpace(SangHo))
                {

                    e.Value = SangHo;

                }
                else
                {

                    e.Value = "";
                }
                if (BankBasKetList.Contains(Selected.TradeId))
                {
                    grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }

            }


            else if (grid1.Columns[e.ColumnIndex] == bizNoDataGridViewTextBoxColumn)
            {
                if (BankBasKetList.Contains(Selected.TradeId))
                {
                    grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
            }
            else if (grid1.Columns[e.ColumnIndex] == nameDataGridViewTextBoxColumn)
            {
                if (BankBasKetList.Contains(Selected.TradeId))
                {
                    grid1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                }
            }

            else if(e.ColumnIndex == ShowTax.Index)
            {

                if (Selected.HasETax == true && Selected.AllowAcc == false)
                {
                   
                    e.Value= "전자";
                }
                else
                {
                    e.Value = "종이";
                }
            }

            else if (e.ColumnIndex ==Edocument.Index)
            {

                if (Selected.HasETax == true && Selected.AllowAcc == false)
                {

                    e.Value = "전자";

                }
                else
                {
                    e.Value = "종이";
                }
            }

            else if(e.ColumnIndex == ColumnDriverName.Index)
            {
                var _Driver = mDriverRepository.NoGetDriver(Selected.DriverId);
                if(_Driver != null)
                e.Value = _Driver.CarYear;
            }

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
            if (Selected.EtaxCanCelYN == "Y")
            {
                grid1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            }
           
            if (grid1.Columns[e.ColumnIndex] == ShowTax)
            {
              //  var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                    {
                        var uOrder = ShareOrderDataSet.Orders.Where(c => c.TradeId == Selected.TradeId).Count();

                        if (Selected.VAT == 0 || Selected.SUMYN == 2 )
                        {
                            
                            if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                            {
                                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                                else
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                            }
                            e.Handled = true;
                        }
                    }



                }
            }


            if (grid1.Columns[e.ColumnIndex] == ColumnImage1)
            {
               // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    try
                    {
                        if (String.IsNullOrEmpty(Selected.Image1))
                        {
                            if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                            {
                                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                                else
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                            }
                            e.Handled = true;
                        }
                    }
                    catch (Exception)
                    {
                        if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                        {
                            if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                            else
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                        }
                        e.Handled = true;
                    }
                }
            }


            if (grid1.Columns[e.ColumnIndex] == Edocument)
            {
               
                if (Selected != null)
                {
                    try
                    {
                        if (String.IsNullOrEmpty(Selected.PdfFileName) &&Selected.HasETax == true)
                        {
                            if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                            {
                                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                 
                                    cellBackground.Dispose();
                                    
                                }
                                else
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                            }
                            e.Handled = true;
                        }

                        if(String.IsNullOrEmpty(Selected.Image1) && Selected.HasETax == false)
                        {
                            if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                            {
                                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                                else
                                {
                                    SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                    e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                    cellBackground.Dispose();
                                }
                            }
                            e.Handled = true;
                        }

                    }
                    catch (Exception)
                    {
                        if ((e.PaintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                        {
                            if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.SelectionBackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                            else
                            {
                                SolidBrush cellBackground = new SolidBrush(e.CellStyle.BackColor);
                                e.Graphics.FillRectangle(cellBackground, e.CellBounds);
                                cellBackground.Dispose();
                            }
                        }
                        e.Handled = true;
                    }
                }
            }


            if (grid1.Columns[e.ColumnIndex] == ColumnAccLog)
            {
               // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as TradeDataSet.TradesRow;
                if (Selected != null)
                {
                    if (String.IsNullOrEmpty(Selected.LGD_Last_Function))
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

            if (Selected.ServiceState == 5)
            {
                grid1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                grid1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;
            }
        }
        private String GetSelectCommand()
        {
           
            return @"SELECT distinct Trades.TradeId, Trades.RequestDate, BeginDate, EndDate, 
                trades.Item, Trades.Price, VAT, Amount, PayState, PayDate, Trades.PayBankName, Trades.PayBankCode, Trades.PayAccountNo, Trades.PayInputName, 
                Trades.DriverId, Trades.ClientId, Trades.UseTax, LGD_OID, 
                LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, AllowAcc, HasAcc, ClientAccId, 
                SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, 
                Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, 
                HasETax, Trades.SourceType,
                Drivers.LoginId AS DriverLoginId, Drivers.CarNo AS DriverCarNo, Drivers.BizNo AS DriverBizNo, Drivers.Name AS DriverName, 
                Drivers.ServiceState, Drivers.MID, 
                Clients.Code AS ClientCode, Clients.Name AS ClientName, Trades.AcceptCount, Trades.SubClientId, Trades.ClientUserId
                ,(SELECT ISNULL(GroupName,'미설정') FROM  DriverInstances WHERE DriverId = Trades.DriverId and ClientId = Trades.ClientId) as GroupName
                ,Trades.ExcelExportYN,ISNULL(Trades.EtaxCanCelYN,'N') AS EtaxCanCelYN,trusteeMgtKey,TransportDate,Trades.StartState,Trades.StopState,Trades.PdfFileName,Trades.AipId,Trades.DocId,Trades.DeleteYn,Trades.UpdateDateTime
				,Orders.TradeId AS OTradeId,Orders.ReferralId
                ,ISNULL(Trades.TaxGubun,'') as TaxGubun,Trades.billNo
                FROM     Trades with (nolock)
                JOIN Drivers with (nolock) ON Trades.DriverId = Drivers.DriverId
                JOIN Clients with (nolock) ON Trades.ClientId = Clients.ClientId 
			    LEFT JOIN Orders with (nolock) ON Trades.TradeId = Orders.TradeId ";


        }



        private void LoadTable()
        {
            tradeDataSet.Trades.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();
                    // 1. 본점/지사
                    if (LocalUser.Instance.LogInInformation.IsSubClient)
                    {
                        if (LocalUser.Instance.LogInInformation.IsAgent)
                        {
                            WhereStringList.Add("Trades.ClientUserId = @ClientUserId");
                            _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        }
                        else
                        {
                            WhereStringList.Add("Trades.SubClientId = @SubClientId");
                            _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        }
                    }
                    else if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        WhereStringList.Add("Trades.ClientId = @ClientId");
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        // 1.1 본지점구분
                        if (cmb_SubClientId.SelectedIndex > 0)
                        {
                            var _SubClientId = (int)cmb_SubClientId.SelectedValue;
                            if (_SubClientId == 0)
                            {
                                WhereStringList.Add("Trades.SubClientId IS NULL");
                            }
                            else
                            {
                                WhereStringList.Add("Trades.SubClientId = @SubClientId");
                                _Command.Parameters.AddWithValue("@SubClientId", _SubClientId);
                            }
                        }
                    }
                    // 2. 조회 기간
                    String _DateColumn = "";
                    if (cmb_Date.SelectedIndex == 0)
                    {
                        _DateColumn = "Trades.RequestDate";
                    }
                    else if (cmb_Date.SelectedIndex == 1)
                    {

                        _DateColumn = "Trades.PayDate";

                    }
                    else if (cmb_Date.SelectedIndex == 2)
                    {

                        _DateColumn = "Orders.CreateTime";

                    }
                    else if (cmb_Date.SelectedIndex == 3)
                    {

                        _DateColumn = "Trades.ExcelExportYN";

                    }

                    WhereStringList.Add($"{_DateColumn} >= @Begin AND {_DateColumn} < @End ");
                    _Command.Parameters.AddWithValue("@Begin", dtp_From.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtp_To.Value.Date.AddDays(1));
                    // 
                    if (cmb_Search.Text == "승인일")
                    {
                        WhereStringList.Add(string.Format("LGD_Last_Function = '승인' AND LGD_Last_Date = '{0}'", dtp_Search.Text));
                    }
                    else if (cmb_Search.Text == "취소일")
                    {
                        WhereStringList.Add(string.Format("LGD_Last_Function = '취소' AND LGD_Last_Date = '{0}'", dtp_Search.Text));
                    }
                    else if (cmb_Search.Text == "계좌번호")
                    {
                        WhereStringList.Add(string.Format("Trades.PayAccountNo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "예금주")
                    {
                        WhereStringList.Add(string.Format("Trades.PayInputName Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "차주사업자번호")
                    {
                        WhereStringList.Add(string.Format("REPLACE(Drivers.BizNo,N'-',N'') Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "차주상호")
                    {
                        WhereStringList.Add(string.Format("Drivers.Name Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "차주아이디")
                    {
                        WhereStringList.Add(string.Format("Drivers.LoginId Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("Drivers.CarNo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "거래번호")
                    {
                        WhereStringList.Add(string.Format("LGD_OID Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "핸드폰번호")
                    {
                        WhereStringList.Add(string.Format("Drivers.MobileNo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "가맹점-ID")
                    {
                        WhereStringList.Add(string.Format("Drivers.MID Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "그룹설정")
                    {
                      
                        if (!LocalUser.Instance.LogInInformation.IsAdmin && (txt_Search.Text.ToUpper() == "A" || txt_Search.Text.ToUpper() == "B" || txt_Search.Text.ToUpper() == "C"))
                        {
                            WhereStringList.Add("(SELECT ISNULL(GroupName,'미설정') FROM  DriverInstances WHERE DriverId = Trades.DriverId and ClientId = Trades.ClientId) = '" + txt_Search.Text.ToUpper() + "'");
                        }
                    }

                    if (cmb_Date.SelectedIndex == 1)
                    {
                        WhereStringList.Add("PayState = 1");

                    }
                    else
                    {
                        if (cmb_PayState.Text == "결제")
                        {
                            WhereStringList.Add("PayState = 1");
                        }

                        else if (cmb_PayState.Text == "미결제")
                        {
                            WhereStringList.Add("PayState = 2");
                        }
                    }
                    if (cmb_AllowAcc.Text == "현금")
                    {
                        WhereStringList.Add("HasAcc = 0");
                    }
                    else if (cmb_AllowAcc.Text == "카드")
                    {
                        WhereStringList.Add("HasAcc = 1");
                    }
                    if (cmb_HasAcc_I.Text == "외부계산서")
                    {
                        //WhereStringList.Add("AllowAcc = 1 AND Orders.TradeId IS NOT NULL AND HasEtax != 1");

                       
                        //WhereStringList.Add("((Orders.TradeId IS NOT NULL AND HasETax != 1) OR(Orders.TradeId IS NOT NULL AND AllowAcc = 1))");
                        WhereStringList.Add("((ISNULL(Orders.TradeId, 0) != 0 AND HasETax != 1) OR( ISNULL(Orders.TradeId, 0) != 0 AND AllowAcc = 1))");
                    }
                    else if (cmb_HasAcc_I.Text == "전자")
                    {
                        WhereStringList.Add("HasETax = 1 AND AllowAcc = 0");
                    }
                    else if (cmb_HasAcc_I.Text == "없음")
                    {

                      
                        //WhereStringList.Add(" AllowAcc = 1 AND Orders.TradeId IS NULL");
                        WhereStringList.Add(" AllowAcc = 1 AND   ISNULL(Orders.TradeId, 0) = 0");
                        // WhereStringList.Add("HasETax = 0 AND AllowAcc = 0");
                    }
                    if (cmb_LGD_Last_Function.Text == "승인")
                    {
                        WhereStringList.Add("LGD_Last_Function = '승인'");
                    }
                    else if (cmb_LGD_Last_Function.Text == "취소")
                    {
                        WhereStringList.Add("LGD_Last_Function = '취소'");
                    }
                    else if (cmb_LGD_Last_Function.Text == "미결")
                    {
                        WhereStringList.Add("ISNULL(LGD_Last_Function, '') = ''");
                    }

                    if (!String.IsNullOrEmpty(txt_ClientSearch.Text))
                    {
                        if (cmb_ClientSerach.Text == "운송사코드")
                        {
                            WhereStringList.Add(string.Format("Clients.Code Like  '%{0}%'", txt_ClientSearch.Text));
                        }
                        else if (cmb_ClientSerach.Text == "운송사명")
                        {
                            WhereStringList.Add(string.Format("Clients.Name Like  '%{0}%'", txt_ClientSearch.Text));
                        }
                    }

                    if(!chk_EtaxCancle.Checked)
                    {
                        WhereStringList.Add("EtaxCanCelYN != 'Y'");

                    }

                    if(cmbBasKet.Text =="체크" )
                    {
                        if (BankBasKetList.Any())
                        {
                            WhereStringList.Add(string.Format("Trades.Tradeid IN ({0})", String.Join(",", BankBasKetList)));

                            WhereStringList.Add("PayState = 2");
                        }
                    }
                    else if(cmbBasKet.Text=="미체크")
                    {
                        if (BankBasKetList.Any())
                        {
                            WhereStringList.Add(string.Format("Trades.Tradeid NOT IN ({0})", String.Join(",", BankBasKetList)));

                            WhereStringList.Add("PayState = 2");
                        }
                    }


                    if (cmb_ReferralId.SelectedIndex != 0)
                    {
                        WhereStringList.Add(string.Format("Orders.ReferralId ={0}", cmb_ReferralId.SelectedValue));
                    }


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString ;

                    }

                    


                    if (cmb_Date.SelectedIndex == 0)
                    {
                        SelectCommandText += "Order by Trades.RequestDate Desc ";
                    }
                    else
                    {
                        SelectCommandText += "Order by Trades.PayDate Desc ";

                    }
                    _Command.CommandText = SelectCommandText ;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        tradeDataSet.Trades.Load(_Reader);

                       
                    }
                }
                _Connection.Close();
            }

            var Query = tradeDataSet.Trades.ToArray();

            if(Query.Any())
            {

                lblCount.Text = Query.Count().ToString("N0");
                lblPrice.Text = Query.Sum(c=> c.Price).ToString("N0");
                lblVat.Text = Query.Sum(c=>c.VAT).ToString("N0");
                lblAmount.Text = Query.Sum(c=> c.Amount).ToString("N0");
            }
            else
            {
                lblCount.Text = "0";
                lblPrice.Text = "0";
                lblVat.Text = "0";
                lblAmount.Text = "0";
            }
        }

        private void LoadTableOne(int TradeId)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();
                    List<String> WhereStringList = new List<string>();
                    WhereStringList.Add("Trades.TradeId = @TradeId");
                    _Command.Parameters.AddWithValue("@TradeId", TradeId);
                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        tradeDataSet.Trades.Load(_Reader);

                        //tradeDataSet.Trades.OrderByDescending(c => c.TradeId);
                    }
                }
                _Connection.Close();
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }
        private void SendCancelIssue(int tradeId)
        {
            try
            {
                var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
                Response response = taxinvoiceService.CancelIssue("1148654906", MgtKeyType.TRUSTEE, _Trade.trusteeMgtKey, memo, "edubillsys");

                SendDelete(tradeId);
            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);

                return;
                
            }
        }

        private void SendDelete(int tradeId)
        {

            try
            {
                var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
                Response response = taxinvoiceService.Delete("1148654906", MgtKeyType.TRUSTEE, _Trade.trusteeMgtKey, "edubillsys");


                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "Delete Trades  WHERE TradeId = @TradeId";
                    cmd.Parameters.AddWithValue("@TradeId", tradeId);
                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                      " UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";
                   // cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                     " UPDATE Orders SET TaxTradeId = null  WHERE TaxTradeId = @TradeId";
                   
                    cmd.ExecuteNonQuery();

                    // cmd.CommandText =
                    //" UPDATE Drivers SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId";
                    using (SqlCommand _DICommand = cn.CreateCommand())
                    {
                        _DICommand.CommandText =
                        " UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId ";
                        _DICommand.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                        _DICommand.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                        _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _DICommand.ExecuteNonQuery();
                    }

                    // cmd.ExecuteNonQuery();

                    if (!String.IsNullOrEmpty(_Trade.PdfFileName))
                    {
                        if (!String.IsNullOrEmpty(_Trade.AipId))
                        {
                            SqlCommand cmdDocument = cn.CreateCommand();
                            cmdDocument.CommandText =
                                " INSERT INTO DocuTable(TradeId,PdfFileName,AipId,DocId,Status,RequestDateTime) " +
                                " Values(@TradeId,@PdfFileName,@AipId,@DocId,@Status,getdate())";
                            cmdDocument.Parameters.AddWithValue("@Status", "_40_ready");
                            cmdDocument.Parameters.AddWithValue("@TradeId", tradeId);
                            cmdDocument.Parameters.AddWithValue("@PdfFileName", _Trade.PdfFileName);
                            cmdDocument.Parameters.AddWithValue("@AipId", _Trade.AipId);
                            cmdDocument.Parameters.AddWithValue("@DocId", _Trade.DocId);
                            cmdDocument.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand cmdDocument = cn.CreateCommand();
                            cmdDocument.CommandText =
                                "Delete DocuTable  WHERE TradeId = @TradeId";
                            cmdDocument.Parameters.AddWithValue("@TradeId", tradeId);
                            cmdDocument.ExecuteNonQuery();

                        }
                    }
                    cn.Close();
                }

                MessageBox.Show("삭제 되었습니다.");
                btn_Search_Click(null, null);
            }
            catch (PopbillException ex)
            {
              
                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return;
            }


          

        }

        private void SendMinusInvoice(int driverId, int tradeId, int clientId,string MgtKEy)
        {
            InitDriverTable();

            //var _Driver = baseDataSet.Drivers.FirstOrDefault(c => c.DriverId == driverId);
            var _Driver = _DriverTable.Where(c => c.DriverId == driverId);
            var _Client = clientDataSet.Clients.FirstOrDefault(c => c.ClientId == clientId);
            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            bool forceIssue = false;        // 지연발행 강제여부
            var TPrice = _Trade.Price * -1;
            var TAmount = _Trade.Amount * -1;
            var TVat = _Trade.VAT * -1;

           

            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();


            taxinvoice.writeDate = TinfoWriteDate;//DateTime.Now.ToString("yyyyMMdd");                      //필수, 기재상 작성일자
            taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
            taxinvoice.issueType = "위수탁";                        //필수, {정발행, 역발행, 위수탁}
            taxinvoice.purposeType = "청구";                        //필수, {영수, 청구}
            taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
            taxinvoice.taxType = "과세";                            //필수, {과세, 영세, 면세}

            taxinvoice.invoicerCorpNum = _Driver.First().BizNo.Replace("-", "");             //공급자 사업자번호
            taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
            taxinvoice.invoicerCorpName = _Driver.First().Name;
            taxinvoice.invoicerMgtKey = tradeId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoicerCEOName = _Driver.First().CEO;
            taxinvoice.invoicerAddr = _Driver.First().AddressState + " " + _Driver.First().AddressCity + " " + _Driver.First().AddressDetail;
            taxinvoice.invoicerBizClass = _Driver.First().Upjong;
            taxinvoice.invoicerBizType = _Driver.First().Uptae;
            taxinvoice.invoicerContactName = _Driver.First().CEO;
            taxinvoice.invoicerEmail = _Driver.First().Email;
            taxinvoice.invoicerTEL = _Driver.First().PhoneNo;
            taxinvoice.invoicerHP = _Driver.First().MobileNo;
            taxinvoice.invoicerSMSSendYN = false;                    //정발행시(공급자->공급받는자) 문자발송기능 사용시 활용

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            taxinvoice.invoiceeType = "사업자";                     //공급받는자 구분, {사업자, 개인, 외국인}
            taxinvoice.invoiceeCorpNum = _Client.BizNo.Replace("-", "");              //공급받는자 사업자번호
            taxinvoice.invoiceeCorpName = _Client.Name;
            // taxinvoice.invoiceeMgtKey = "";                         //역발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoiceeCEOName = _Client.CEO;
            taxinvoice.invoiceeAddr = _Client.AddressState + " " + _Client.AddressCity + " " + _Client.AddressDetail;
            taxinvoice.invoiceeBizClass = _Client.Upjong;
            taxinvoice.invoiceeBizType = _Client.Uptae;
            taxinvoice.invoiceeTEL1 = _Client.MobileNo;
            taxinvoice.invoiceeContactName1 = _Client.CEO;
            taxinvoice.invoiceeEmail1 = _Client.Email;
            taxinvoice.invoiceeHP1 = _Client.MobileNo;
            taxinvoice.invoiceeSMSSendYN = false;                   //역발행시(공급받는자->공급자) 문자발송기능 사용시 활용

            taxinvoice.supplyCostTotal = TPrice.ToString().Replace(".00","");                  //필수 공급가액 합계"
            taxinvoice.taxTotal = TVat.ToString().Replace(".00", "");                      //필수 세액 합계
            taxinvoice.totalAmount = TAmount.ToString().Replace(".00", "");                  //필수 합계금액.  공급가액 + 세액

            taxinvoice.modifyCode = 4;                           //수정세금계산서 작성시 1~6까지 선택기재.
            taxinvoice.originalTaxinvoiceKey = TinfoitemKey;                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
            taxinvoice.serialNum = "1";
            taxinvoice.cash = "";                                   //현금
            taxinvoice.chkBill = "";                                //수표
            taxinvoice.note = "";                                   //어음
            taxinvoice.credit = "";                                 //외상미수금
            taxinvoice.remark1 = _Trade.TransportDate.ToString("d").Replace("-", "/") + " " + _Trade.StartState + " → " + _Trade.StopState + " ( ☎ " + _Driver.First().MobileNo + " )";
            taxinvoice.remark2 = "";
            taxinvoice.remark3 = "";
            taxinvoice.kwon = 1;
            taxinvoice.ho = 1;

            taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
            taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

            string CtrusteeMgtKey = "t" + _Trade.TradeId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            #region 수탁자
            //수탁자 문서관리번호
            taxinvoice.trusteeMgtKey = CtrusteeMgtKey; //_Trade.trusteeMgtKey;
            //사업자번호
            taxinvoice.trusteeCorpNum = "1148654906";
            //상호
            taxinvoice.trusteeCorpName = "(주)에듀빌시스템";
            //대표자
            taxinvoice.trusteeCEOName = "박양우";

            //주소
            taxinvoice.trusteeAddr = "서울특별시 금천구 디지털로9길 65, 408호(가산동,백상스타타워1차)";

            //업태
            taxinvoice.trusteeBizType = "";

            //업종
            taxinvoice.trusteeBizClass = "";


            //담당자성명
            taxinvoice.trusteeContactName = "박양우";

            #endregion


            taxinvoice.detailList = new List<TaxinvoiceDetail>();

            TaxinvoiceDetail detail = new TaxinvoiceDetail();

            detail.serialNum = 1;                                   //일련번호
            detail.purchaseDT = DateTime.Now.ToString("yyyyMMdd");                         //거래일자
            detail.itemName = "화물운송료(" + _Driver.First().CarNo + ")";
            detail.spec = "";
            detail.qty = "1";                                       //수량
            detail.unitCost = TPrice.ToString().Replace(".00", ""); ;                             //단가
            detail.supplyCost  = TPrice.ToString().Replace(".00", ""); ;                           //공급가액
            detail.tax = TVat.ToString().Replace(".00", ""); ;                                   //세액
            detail.remark = "";

            taxinvoice.detailList.Add(detail);

            detail = new TaxinvoiceDetail();

            try
            {
                Response response = taxinvoiceService.RegistIssue("1148654906", taxinvoice, forceIssue, memo);
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                   
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "INSERT INTO  Trades" +
                        "(RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, trusteeMgtKey, EtaxCanCelYN, ExcelExportYN, TransportDate, StartState, StopState)  " +
                        " SELECT getdate(), BeginDate, EndDate, DriverName, Item, Price *-1, VAT*-1, Amount*-1, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, '" + CtrusteeMgtKey + "', 'Y', ExcelExportYN, TransportDate, StartState, StopState FROM Trades" +
                        " WHERE TradeId = @TradeId" +
                        " UPDATE Trades SET EtaxCanCelYN = 'Y' " +
                        " WHERE TradeId = @TradeId";



                    cmd.Parameters.AddWithValue("@TradeId", tradeId);
                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                   " UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";

                    cmd.ExecuteNonQuery();


                    cmd.CommandText =
                    " UPDATE Orders SET TaxTradeId = null  WHERE TaxTradeId = @TradeId";

                    cmd.ExecuteNonQuery();


                    // cmd.CommandText =
                    //" UPDATE Drivers SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId";
                    // cmd.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                    // cmd.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                    // cmd.ExecuteNonQuery();
                    using (SqlCommand _DICommand = cn.CreateCommand())
                    {
                        _DICommand.CommandText =
                        " UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId ";
                        _DICommand.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                        _DICommand.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                        _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _DICommand.ExecuteNonQuery();
                    }

                }

                MessageBox.Show("발행 취소 되었습니다.");

                btn_Search_Click(null, null);


            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                MessageBox.Show(ex.code.ToString()+"\r\n"+ ex.Message);
                return; 
            }
        }
        string _billNo = "";
        string _changestatus = "";
        string _changestatusCode = "";
        string _EtaxCode = "";
        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);

            if (grid1.SelectedCells.Count > 0)
            {
                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "세금계산", 1), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                tradesBindingSource.EndEdit();
                if (tradesBindingSource.Current != null)
                {
                    if (((DataRowView)tradesBindingSource.Current).Row is TradeDataSet.TradesRow Selected)
                    {
                        tradesBindingSource.EndEdit();
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            //전자계산서
                            if (Selected.HasETax)
                            {
                                if (!String.IsNullOrEmpty(Selected.trusteeMgtKey))
                                {
                                    //if (Selected.EtaxCanCelYN == "Y")
                                    //{
                                    //    MessageBox.Show("취소건 은 삭제할 수 없습니다", "삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    //    return;
                                    //}
                                    #region
                                    TaxinvoiceInfo taxinvoiceInfo = taxinvoiceService.GetInfo("1148654906", MgtKeyType.TRUSTEE, Selected.trusteeMgtKey);

                                try
                                {
                                    string tmp = null;

                                    tmp += "itemKey : " + taxinvoiceInfo.itemKey + CRLF;
                                    tmp += "taxType : " + taxinvoiceInfo.taxType + CRLF;
                                    tmp += "writeDate : " + taxinvoiceInfo.writeDate + CRLF;
                                    tmp += "regDT : " + taxinvoiceInfo.regDT + CRLF;

                                    tmp += "invoicerCorpName : " + taxinvoiceInfo.invoicerCorpName + CRLF;
                                    tmp += "invoicerCorpNum : " + taxinvoiceInfo.invoicerCorpNum + CRLF;
                                    tmp += "invoicerMgtKey : " + taxinvoiceInfo.invoicerMgtKey + CRLF;
                                    tmp += "invoicerPrintYN : " + taxinvoiceInfo.invoicerPrintYN + CRLF;
                                    tmp += "invoiceeCorpName : " + taxinvoiceInfo.invoiceeCorpName + CRLF;
                                    tmp += "invoiceeCorpNum : " + taxinvoiceInfo.invoiceeCorpNum + CRLF;
                                    tmp += "invoiceeMgtKey : " + taxinvoiceInfo.invoiceeMgtKey + CRLF;
                                    tmp += "invoiceePrintYN : " + taxinvoiceInfo.invoiceePrintYN + CRLF;
                                    tmp += "closeDownState : " + taxinvoiceInfo.closeDownState + CRLF;
                                    tmp += "closeDownStateDate : " + taxinvoiceInfo.closeDownStateDate + CRLF;
                                    tmp += "interOPYN : " + taxinvoiceInfo.interOPYN + CRLF;

                                    tmp += "supplyCostTotal : " + taxinvoiceInfo.supplyCostTotal + CRLF;
                                    tmp += "taxTotal : " + taxinvoiceInfo.taxTotal + CRLF;
                                    tmp += "purposeType : " + taxinvoiceInfo.purposeType + CRLF;
                                    tmp += "modifyCode : " + taxinvoiceInfo.modifyCode.ToString() + CRLF;
                                    tmp += "issueType : " + taxinvoiceInfo.issueType + CRLF;

                                    tmp += "issueDT : " + taxinvoiceInfo.issueDT + CRLF;
                                    tmp += "preIssueDT : " + taxinvoiceInfo.preIssueDT + CRLF;

                                    tmp += "stateCode : " + taxinvoiceInfo.stateCode.ToString() + CRLF;
                                    tmp += "stateDT : " + taxinvoiceInfo.stateDT + CRLF;

                                    tmp += "openYN : " + taxinvoiceInfo.openYN.ToString() + CRLF;
                                    tmp += "openDT : " + taxinvoiceInfo.openDT + CRLF;
                                    tmp += "ntsresult : " + taxinvoiceInfo.ntsresult + CRLF; //국세청전송결과
                                    tmp += "ntsconfirmNum : " + taxinvoiceInfo.ntsconfirmNum + CRLF; //국세청승인번호
                                    tmp += "ntssendDT : " + taxinvoiceInfo.ntssendDT + CRLF; //
                                    tmp += "ntsresultDT : " + taxinvoiceInfo.ntsresultDT + CRLF;
                                    tmp += "ntssendErrCode : " + taxinvoiceInfo.ntssendErrCode + CRLF;
                                    tmp += "stateMemo : " + taxinvoiceInfo.stateMemo;
                                    // MessageBox.Show(tmp, "문서 상태/요약 정보 조회");

                                    if (!string.IsNullOrEmpty(taxinvoiceInfo.ntsresult))
                                    {
                                        TinfoNtsresult = taxinvoiceInfo.ntsresult;
                                    }
                                    TinfoWriteDate = taxinvoiceInfo.writeDate;
                                    TinfoitemKey = taxinvoiceInfo.itemKey;
                                    TinfostateCode = taxinvoiceInfo.stateCode.ToString();

                                    switch (TinfostateCode)
                                    {
                                        case "300":
                                            TinfostateName = "발행완료";
                                            break;
                                        case "310":
                                            TinfostateName = "발행완료";
                                            break;
                                        case "301":
                                            TinfostateName = "전송중";
                                            break;
                                        case "302":
                                            TinfostateName = "전송중";
                                            break;
                                        case "303":
                                            TinfostateName = "전송중";
                                            break;
                                        case "311":
                                            TinfostateName = "전송중";
                                            break;
                                        case "312":
                                            TinfostateName = "전송중";
                                            break;
                                        case "313":
                                            TinfostateName = "전송중";
                                            break;
                                        case "304":
                                            TinfostateName = "전송성공";
                                            break;
                                        case "314":
                                            TinfostateName = "전송성공";
                                            break;
                                        case "305":
                                            TinfostateName = "전송실패";
                                            break;
                                        case "315":
                                            TinfostateName = "전송실패";
                                            break;

                                    }
                                }

                                catch (PopbillException ex)
                                {
                                    MessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                                                    "응답메시지(message) : " + ex.Message, "문서 상태/요약 정보 조회");
                                }
                                #endregion

                                
                                    //당일일경우 삭제
                                    if (Selected.RequestDate.Date == DateTime.Now.Date)
                                    {

                                        try
                                        {
                                            if (TinfostateName == "발행완료")
                                            {
                                                if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 당일 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    SendCancelIssue(Selected.TradeId);
                                                }
                                            }
                                            else if (TinfostateName == "전송성공")
                                            {
                                                if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    SendMinusInvoice(Selected.DriverId, Selected.TradeId, Selected.ClientId, Selected.trusteeMgtKey);
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                            return;
                                        }

                                    }
                                    else
                                    {
                                        if (TinfostateName == "발행완료")
                                        {
                                            if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            {
                                                try
                                                {
                                                    SendCancelIssue(Selected.TradeId);

                                                    //SendMinusInvoice(Selected.DriverId, Selected.TradeId, Selected.ClientId, Selected.trusteeMgtKey);
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                    return;
                                                }
                                            }
                                        }
                                        else if (TinfostateName == "전송성공")
                                        {
                                            if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            {
                                                try
                                                {
                                                    SendMinusInvoice(Selected.DriverId, Selected.TradeId, Selected.ClientId, Selected.trusteeMgtKey);
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    string[] BillStatusList = null;
                                    var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
                                    LocalUser.Instance.LogInInformation.LoadClient();
                                    var _Clients = LocalUser.Instance.LogInInformation.Client;
                                    var _changedStatusReq = dTIServiceClass.changedStatusReq("EDB", _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, Selected.BeginDate.ToString("yyyyMMdd"), Selected.EndDate.ToString("yyyyMMdd"));

                                    try
                                    {
                                        var _changedStatusReqList = _changedStatusReq.Split('/');

                                        var retVal = _changedStatusReqList[0];
                                        var errMsg = _changedStatusReqList[1];
                                        var statusMsg = _changedStatusReqList[2];

                                        if (retVal == "0")
                                        {

                                            var statusMsgList = statusMsg.Split(';');

                                            for (int i = 0; i < statusMsgList.Length; i++)
                                            {
                                                if (statusMsgList[i].Contains(Selected.billNo))
                                                {
                                                    BillStatusList = statusMsgList[i].Split(',');
                                                    break;
                                                }
                                            }

                                            if (BillStatusList.Length != 0)
                                            {
                                                //승인번호
                                                _billNo = BillStatusList[0];
                                                //1.문서상태변경 2.신고상태변경
                                                _changestatus = BillStatusList[1];
                                                //문서상태 코드 또는 신고상태 코드
                                                _changestatusCode = BillStatusList[2];
                                                //국세청응답코드
                                                _EtaxCode = BillStatusList[3];
                                            }

                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch { }


                                    if (_changestatusCode == "A" )
                                    {
                                        if (MessageBox.Show("정말 삭제하시겠습니까?", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {
                                                NiceEtaxDelete(Selected.TradeId);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }


                                        }
                                    }
                                    else if (_changestatusCode == "B")
                                    {
                                        MessageBox.Show("국세청 전송중입니다.\r\n잠시후 다시 시도해주세요.", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }
                                    else if (_changestatusCode == "C" || _changestatusCode == "D")
                                    {
                                        if (MessageBox.Show("정말 삭제하시겠습니까?", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {
                                                NiceEtaxEdit(Selected.TradeId);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }


                                        }


                                    }
                                    else
                                    {
                                        if (MessageBox.Show("정말 삭제하시겠습니까?", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            try
                                            {
                                                NiceEtaxDelete(Selected.TradeId);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                return;
                                            }


                                        }

                                    }
                                }

                            }
                            else
                            {
                                cn.Open();
                                SqlCommand cmd = cn.CreateCommand();
                               


                                cmd.CommandText =
                                  "UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";
                                cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                                cmd.ExecuteNonQuery();



                                cmd.CommandText =
                                    "Delete Trades  WHERE TradeId = @TradeId";
                               // cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                                cmd.ExecuteNonQuery();



                                SqlCommand cmdDriver = cn.CreateCommand();

                                cmdDriver.CommandText =
                                                @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                                cmdDriver.Parameters.AddWithValue("@Mizi", Selected.Amount);
                                cmdDriver.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                                cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                                cmdDriver.ExecuteNonQuery();



                                cn.Close();

                                try
                                {

                                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "세금계산서", 1), "세금계산서 삭제 성공");
                                    btn_Search_Click(null, null);
                                }
                                catch { }
                            }
                        }
                    }
                }

               
            }
        }

        private void txt_ClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmTrade_Add _Form = new FrmTrade_Add();
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
            var AllowTaxBool = !rdb_Tax0.Checked;
            if (!AllowTaxBool)
            {
                decimal _Price = 0;
                if (decimal.TryParse(txt_Price.Text.Replace(",", ""), out _Price))
                {
                    decimal _Vat = Math.Floor(_Price * 0.1m);
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    var _Amount = (_Price + _Vat);
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }
        }

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grid1.RowCount; i++)
            {
                var cell = grid1[CheckBox.Index, i] as DataGridViewDisableCheckBoxCell;
                if (cell.Enabled)
                    grid1[CheckBox.Index, i].Value = chkAllSelect.Checked;

                if (cell.Value != null && (bool)cell.Value)
                {
                    var _Row = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (!BankBasKetList.Contains(_Row.TradeId))
                    {
                        BankBasKetList.Add(_Row.TradeId);
                    }
                }
                else
                {
                    var _Row = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (BankBasKetList.Contains(_Row.TradeId))
                    {
                        BankBasKetList.Remove(_Row.TradeId);
                    }
                }

            }

            lblBankBasketCount.Text = BankBasKetList.Count().ToString("N0");



        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect1 = grid1.GetColumnDisplayRectangle(CheckBox.Index, true);
            chkAllSelect.Location = new Point(rect1.Location.X + 2, rect1.Location.Y + 4);
            if (rect1.Width == 0)
                chkAllSelect.Visible = false;
            else
                chkAllSelect.Visible = true;
            Rectangle rect2 = grid1.GetColumnDisplayRectangle(SelectTax.Index, true);
            chkAllSelectTax.Location = new Point(rect2.Location.X + 2, rect2.Location.Y + 4);
            if (rect2.Width == 0)
                chkAllSelectTax.Visible = false;
            else
                chkAllSelectTax.Visible = true;
        }

        private void btnExcelDown_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btn_AcceptAcc_Click(object sender, EventArgs e)
        {
            var Datas = new List<TradeDataSet.TradesRow>();
            for (int i = 0; i < grid1.RowCount; i++)
            {
                var _Cell = grid1[CheckBox.Index, i] as DataGridViewDisableCheckBoxCell;
                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                    var _Row = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    // && _Row.HasETax == true && _Row.EtaxCanCelYN == "N"
                    if (_Row.ServiceState == 1 && _Row.HasAcc == true )
                        Datas.Add(_Row);
                }
            }
            if (Datas.Any())
            {
                FrmLogin_B f = new FrmLogin_B();
                f.ShowDialog();
                if (f.Accepted)
                {
                    Dialog_AcceptAcc d = new Dialog_AcceptAcc();
                    d.Trades = Datas.ToArray();
                    d.AuthKey = f.AuthKey;
                    d.AuthKey = Guid.NewGuid().ToString();
                    d.ShowDialog();
                    btn_Search_Click(null, null);
                }
            }
            else
            {
                MessageBox.Show("카드로 결제할 건이 없습니다.");
            }

        }

        private void btn_CancelAcc_Click(object sender, EventArgs e)
        {
            var Datas = new List<TradeDataSet.TradesRow>();
            var Datas2 = new List<TradeDataSet.TradesRow>();
            var Datas3 = new List<TradeDataSet.TradesRow>();
            for (int i = 0; i < grid1.RowCount; i++)
            {
                var _Cell = grid1[CheckBox.Index, i] as DataGridViewDisableCheckBoxCell;
                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                   
                    var _Row = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (_Row.ServiceState == 1  && _Row.PayDate.ToString("d").Replace("-", "/") == DateTime.Now.ToString("d").Replace("-", "/"))
                        Datas.Add(_Row);

                    if (_Row.ServiceState == 1 && _Row.PayDate.ToString("d").Replace("-", "/") != DateTime.Now.ToString("d").Replace("-", "/"))
                        Datas2.Add(_Row);
                    

                }
            }

            if (Datas.Any())
            {
                FrmLogin_B f = new FrmLogin_B();
                f.ShowDialog();
                if (f.Accepted)
                {
                    Dialog_CancelAcc d = new Dialog_CancelAcc();
                    d.Trades = Datas.ToArray();
                    d.AuthKey = Guid.NewGuid().ToString();
                    d.ShowDialog();
                    btn_Search_Click(null, null);
                }
            }
            else
            {

                if (Datas2.Any())
                {
                    MessageBox.Show("신용카드 취소는 당일 승인 건만 가능 합니다.");
                }
                else
                {
                    

                    MessageBox.Show("먼저 취소할 항목들을 선택하여 주십시오.");
                }
               
            }

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
            if (tradesBindingSource.Current == null)
                return;
            if (newDGV1.RowCount == 0)
                return;
            if (e.RowIndex < 0)
                return;

            var _Trade = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                var _TradePrice = 0m;
                foreach (var rowView in ordersBindingSource)
                {
                    var _Row = ((DataRowView)rowView).Row;
                    var _OrderId = Convert.ToInt32(_Row["OrderId"]);
                    var _Price = Convert.ToDecimal(_Row["Price"]);
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "UPDATE Orders SET Price = @Price WHERE OrderId = @OrderId";
                        _Command.Parameters.AddWithValue("@Price", _Price);
                        _Command.Parameters.AddWithValue("@OrderId", _OrderId);
                        _Command.ExecuteNonQuery();
                    }
                    _TradePrice += _Price;
                }
                _Trade.Price = _TradePrice;
                decimal _Vat = Math.Floor(_TradePrice * 0.1m);
                txt_Price.Text = _TradePrice.ToString("N0");
                txt_VAT.Text = _Vat.ToString("N0");
                var _Amount = (_TradePrice + _Vat);
                txt_Amount.Text = _Amount.ToString("N0");
                _Trade.VAT = _Vat;
                _Trade.Amount = _Amount;
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "UPDATE Trades SET Price = @Price, VAT = @VAT, Amount = @Amount WHERE TradeId = @TradeId";
                    _Command.Parameters.AddWithValue("@Price", _Trade.Price);
                    _Command.Parameters.AddWithValue("@VAT", _Trade.VAT);
                    _Command.Parameters.AddWithValue("@Amount", _Trade.Amount);
                    _Command.Parameters.AddWithValue("@TradeId", _Trade.TradeId);
                    _Command.ExecuteNonQuery();
                }
                _Connection.Close();
            }
            
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var _Row = ((DataRowView)ordersBindingSource.Current).Row;
            if (e.ColumnIndex == ColumnNumber.Index)
            {
                e.Value = e.RowIndex + 1;
            }
            else if(e.ColumnIndex == Column1Start.Index)
            {
              
                e.Value = String.Join("/", new object[] { _Row["StartState"], _Row["StartCity"],_Row["StartStreet"] }.Where(c => c != null && !String.IsNullOrWhiteSpace(c.ToString())));
            }
            else if (e.ColumnIndex == Column1Stop.Index)
            {
               // var _Row = ((DataRowView)ordersBindingSource.Current).Row;
                e.Value = String.Join("/", new object[] { _Row["StopState"], _Row["StopCity"], _Row["StopStreet"] }.Where(c => c != null && !String.IsNullOrWhiteSpace(c.ToString())));
            }
            else if (e.ColumnIndex == ColumnsReferralName.Index)
            {
               // var _Row = ((DataRowView)ordersBindingSource.Current).Row;
                e.Value = _Row["ReferralName"];
            }
        }


        private void newDGV2Binding(int iTradeID)
        {
            DataTable mDataTable = new DataTable();

            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT AccLogId, AccFunction, AccLogs.BankName, CardNo, AccLogs.TradeId, AccLogs.CreateDate, LGD_TID, LGD_MID,AccLogs.LGD_OID, LGD_AMOUNT, LGD_RESPCODE, LGD_RESPMSG,drivers.MID FROM AccLogs join trades on AccLogs.TradeId = trades.TradeId join drivers on trades.driverid = drivers.DriverId Where AccLogs.TradeId = @TradeId Order by AccLogs.CreateDate DESC";
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
            if(e.ColumnIndex == Column7.Index)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = e.Value.ToString().Substring(0, e.Value.ToString().Length - 4) + "****";
                }

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
                    //else if (cmb_Search.Text == "화주사업자번호")
                    //{
                    //    FilterString = string.Format("CustomerBizNo Like  '%{0}%'", txt_Search.Text);
                    //}
                    //else if (cmb_Search.Text == "화주상호")
                    //{
                    //    FilterString = string.Format("CustomerName Like  '%{0}%'", txt_Search.Text);
                    //}
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
                tradesBindingSource.Filter = "";
            else
                tradesBindingSource.Filter = String.Join(" AND ", Filters);
        }

        private void cmb_HasAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_HasAcc.SelectedIndex == 1)
            {
                //chk_PayState.Enabled = false;
                tableLayoutPanel5.Enabled = false;
            }
            else
            {
                // chk_PayState.Enabled = true;
                tableLayoutPanel5.Enabled = true;
            }
            if (tradesBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            if (Selected == null)
                return;
            if (cmb_HasAcc.SelectedIndex == 1)
            {
                Selected.HasAcc = true;
            }
            else
            {
                Selected.HasAcc = false;
            }
            if (!MethodProcess)
            {
                //Selected.RejectChanges();
                // 여기서 데이터 적용한다.
                var TradeId = Selected.TradeId;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "UPDATE Trades SET  HasAcc = @HasAcc " +
                        "WHERE TradeId = @TradeId AND PayState <> 1";
                    cmd.Parameters.AddWithValue("@HasAcc", Selected.HasAcc);
                    cmd.Parameters.AddWithValue("@TradeId", TradeId);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                LoadTableOne(TradeId);
            }
        }

        private void rdb_Car_CheckedChanged(object sender, EventArgs e)
        {


            FrmMDI frmMDI = new FrmMDI();




            FrmTrade_Client _Form = new FrmTrade_Client();
            _Form.MdiParent = this.MdiParent;

            _Form.Show();

            this.Close();
        }

        private void rdb_Client_CheckedChanged(object sender, EventArgs e)
        {
            //FrmMDI frmMDI = new FrmMDI();

            //frmMDI.Menu_Click("mnu0209", null);
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (dataGridView3.Columns[e.ColumnIndex] == Memo)
            //{
            //    e.Value = "화물운송료";
            //}
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_DriverId_TextChanged(object sender, EventArgs e)
        {


        }

        private void chkAllSelectTax_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grid1.RowCount; i++)
            {
                var cell = grid1[SelectTax.Index, i] as DataGridViewDisableCheckBoxCell;
                if (cell.Enabled)
                    grid1[SelectTax.Index, i].Value = chkAllSelectTax.Checked;
            }

        }

        private void txt_Price_Enter(object sender, EventArgs e)
        {
            txt_Price.Text = txt_Price.Text.Replace(",", "");
        }

        private void btn_New2_Click(object sender, EventArgs e)
        {
            FrmTradeNew2 _Form = new FrmTradeNew2();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            string DCarNo2 = "";
            if (_Form.ShowDialog() == DialogResult.OK)
            {
                lblBankBasketCount.Text = "0";
                BankBasKetList.Clear();


              //  DCarNo2 = _Form._CarNo;

                //if (DCarNo2 == "")
                //    return;

                //FrmMDI frmMDI = new FrmMDI();

                //FrmMN0203_CAROWNERMANAGE _f = new FrmMN0203_CAROWNERMANAGE(DCarNo2);
                //_f.MdiParent = this.MdiParent;

                //_f.Show();

                //this.Close();

            }
            else
            {
                btn_Search_Click(null, null);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == CheckBox.Index )
            {
                
                DataGridViewCheckBoxCell _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                _Cell.Value = _Cell.Value == null || !((bool)_Cell.Value);
                grid1.RefreshEdit();
                grid1.NotifyCurrentCellDirty(true);

                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                    var _Row = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (!BankBasKetList.Contains(_Row.TradeId))
                    {
                        BankBasKetList.Add(_Row.TradeId);
                    }

                }
                else
                {
                    var _Row = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as TradeDataSet.TradesRow;
                    _Row.RejectChanges();
                    if (BankBasKetList.Contains(_Row.TradeId))
                    {
                        BankBasKetList.Remove(_Row.TradeId);
                    }
                }

                lblBankBasketCount.Text = BankBasKetList.Count().ToString("N0");
            }

            if(e.ColumnIndex == SelectTax.Index)
            {
                  DataGridViewCheckBoxCell _Cell = grid1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                _Cell.Value = _Cell.Value == null || !((bool)_Cell.Value);
                grid1.RefreshEdit();
                grid1.NotifyCurrentCellDirty(true);
            }
        }

        private void AllowAcc_CheckedChanged(object sender, EventArgs e)
        {
            //if (tradesBindingSource.Current == null)
            //    return;
            //var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            //if (Selected == null)
            //    return;
            //if (!MethodProcess)
            //{
            //    // 여기서 데이터 적용한다.
            //    var TradeId = Selected.TradeId;
            //    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //    {
            //        cn.Open();
            //        SqlCommand cmd = cn.CreateCommand();
            //        cmd.CommandText =
            //            "UPDATE Trades SET  AllowAcc = @AllowAcc " +
            //            "WHERE TradeId = @TradeId";
            //        cmd.Parameters.AddWithValue("@AllowAcc", AllowAcc.Checked);
            //        cmd.Parameters.AddWithValue("@TradeId", TradeId);
            //        cmd.ExecuteNonQuery();
            //        cn.Close();
            //    }
            //    LoadTableOne(TradeId);
            //}

            AllowAcc.Checked = true;
        }

        private void rdb_Tax0_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Tax0.Checked)
            {
                txt_Amount.ReadOnly = true;
                txt_Price.ReadOnly = false;
                lbl_Price.ForeColor = Color.Blue;
                lbl_Amt.ForeColor = Color.Black;
                txt_Price.Focus();
            }
            else
            {
                txt_Amount.ReadOnly = false;
                txt_Price.ReadOnly = true;
                lbl_Price.ForeColor = Color.Black;
                lbl_Amt.ForeColor = Color.Blue;
                txt_Amount.Focus();
            }
        }

        private void txt_Amount_Enter(object sender, EventArgs e)
        {
            var AllowTaxBool = !rdb_Tax0.Checked;
            if (AllowTaxBool == true)
            {
                decimal _Price = 0;
                decimal _Vat = 0;
                decimal _Amount = 0;
                if (decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _Amount))
                {

                    _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }
        }

        private void txt_Amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_Amount_Leave(object sender, EventArgs e)
        {
            if (rdb_Tax1.Checked)
            {
                decimal _Price = 0;
                decimal _Vat = 0;
                decimal _Amount = 0;
                if (decimal.TryParse(txt_Amount.Text.Replace(",", ""), out _Amount))
                {
                    _Vat = Math.Floor(_Amount - (_Amount / 1.1m));
                    _Price = _Amount - _Vat;
                    txt_Price.Text = _Price.ToString("N0");
                    txt_VAT.Text = _Vat.ToString("N0");
                    txt_Amount.Text = _Amount.ToString("N0");
                }
                else
                {
                    txt_VAT.Text = "";
                    txt_Amount.Text = "";
                }
            }


            txt_Amount.Text = txt_Amount.Text.Replace(",", "");
        }

        private void btnUpdatePayDate_Click(object sender, EventArgs e)
        {

            if (tradesBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            if (Selected == null)
                return;
            //if (Selected != null)
            //{
            //    if (Selected.PayState != 1)
            //    {
            //        dtp_PayDate.Value = DateTime.Now;
            //    }
            //}
            if (MessageBox.Show($"{Selected.DriverName}\r{((long)Selected.Amount).ToString("N0")} 원\r정말 결제적용 하시겠습니까?", "차주 결제적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!MethodProcess)
            {
                Selected.RejectChanges();
                if (Selected.HasAcc)
                    return;
                var PayState = 1;
                //if (chk_PayState.Checked == true)
                //{
                //    PayState = 1;
                //}
                // 여기서 데이터 적용한다.
                var TradeId = Selected.TradeId;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "UPDATE Trades SET  PayState = @PayState,PayDate = @PayDate " +
                        "WHERE TradeId = @TradeId AND HasAcc = 0";
                    cmd.Parameters.AddWithValue("@PayState", PayState);
                    cmd.Parameters.AddWithValue("@PayDate", dtp_PayDate.Text);
                    cmd.Parameters.AddWithValue("@TradeId", TradeId);
                    int _R = cmd.ExecuteNonQuery();
                    if (_R > 0)
                    {
                        cmd.CommandText =
                            @" UPDATE Orders
                            SET DriverPayDate = @PayDate
                                ,DriverPayPrice = @DriverPayPrice
                                ,DriverPayVAT = @DriverPayVAT
                                ,UseCardPay = 0
                                ,DriverPoint = null
                            WHERE TradeId = @TradeId";
                        cmd.Parameters.AddWithValue("@DriverPayPrice", Selected.Price);
                        cmd.Parameters.AddWithValue("@DriverPayVAT", Selected.VAT);
                        //cmd.Parameters.AddWithValue("@PayDate", dtp_PayDate.Value);
                        cmd.ExecuteNonQuery();


                        SqlCommand cmdDriver = cn.CreateCommand();

                        cmdDriver.CommandText =
                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                        cmdDriver.Parameters.AddWithValue("@Mizi", Selected.Amount);
                        cmdDriver.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                        cmdDriver.ExecuteNonQuery();

                    }
                    cn.Close();
                }
                LoadTableOne(TradeId);
            }





            //if (tradesBindingSource.Current == null)
            //    return;
            //var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            //if (Selected == null)
            //    return;
            //if (!Selected.HasAcc || Selected.PayState == 1)
            //{
            //    // 여기서 데이터 적용한다.
            //    var TradeId = Selected.TradeId;
            //    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //    {
            //        cn.Open();
            //        SqlCommand cmd = cn.CreateCommand();
            //        cmd.CommandText =
            //            "UPDATE Trades SET  PayDate = @PayDate " +
            //            "WHERE TradeId = @TradeId";
            //        cmd.Parameters.AddWithValue("@PayDate", dtp_PayDate.Value);
            //        cmd.Parameters.AddWithValue("@TradeId", TradeId);
            //        cmd.ExecuteNonQuery();
            //        cn.Close();
            //    }
            //    LoadTableOne(TradeId);
            //}
        }


        private void btnCanclePayDate_Click(object sender, EventArgs e)
        {
            
            if (tradesBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            if (Selected == null)
                return;
            //if (Selected != null)
            //{
            //    if (Selected.PayState != 1)
            //    {
            //        dtp_PayDate.Value = DateTime.Now;
            //    }
            //}

            if (MessageBox.Show($"{Selected.DriverName}\r{((long)Selected.Amount).ToString("N0")} 원\r정말 결제취소 하시겠습니까?", "차주 결제취소", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;


            if (!MethodProcess)
            {
                Selected.RejectChanges();
                if (Selected.HasAcc)
                    return;
                var PayState = 2;
                
                // 여기서 데이터 적용한다.
                var TradeId = Selected.TradeId;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText =
                        "UPDATE Trades SET  PayState = @PayState " +
                        "WHERE TradeId = @TradeId AND HasAcc = 0";
                    cmd.Parameters.AddWithValue("@PayState", PayState);
                    cmd.Parameters.AddWithValue("@TradeId", TradeId);
                    int _R = cmd.ExecuteNonQuery();
                    if (_R > 0)
                    {
                        cmd.CommandText =
                          @"UPDATE Orders
                            SET DriverPayDate = null
                                ,DriverPayPrice = null
                                ,DriverPayVAT = null
                                ,UseCardPay = null
                                ,DriverPoint = null
                            WHERE TradeId = @TradeId";
                      //  cmd.Parameters.AddWithValue("@TradeId", TradeId);
                        cmd.ExecuteNonQuery();



                        SqlCommand cmdDriver = cn.CreateCommand();

                        cmdDriver.CommandText =
                                        @" UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId";
                        cmdDriver.Parameters.AddWithValue("@Mizi", Selected.Amount);
                        cmdDriver.Parameters.AddWithValue("@DriverId", Selected.DriverId);
                        cmdDriver.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                        cmdDriver.ExecuteNonQuery();

                    }
                    cn.Close();
                }
                LoadTableOne(TradeId);
            }

        }


        private void Btn_Stat8_Click(object sender, EventArgs e)
        {
            FrmMNSTATS8 f = new FrmMNSTATS8();
            f.ShowDialog();
        }

        private void btnAppSms_Click(object sender, EventArgs e)
        {

            //MessageBox.Show("준비중입니다.", btnAppSms.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (!LocalUser.Instance.LogInInformation.Client.HideAddTrade && !LocalUser.Instance.LogInInformation.Client.SmsYn)
            {
                MessageBox.Show("본 서비스의 “CRM 연동”과 “문자 전송”은 \r\nLG U+ 인터넷전화기를 통해서만 가능 합니다.\r\n귀 사는\r\nLG U+ 미 사용자이므로 사용하실 수 없습니다.\r\n\r\nLG U+인터넷전화 신청\r\n문의 : ☎ 1833-2363  엑티브아이티㈜", "문자전송", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {



                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    FrmAppSms d = new FrmAppSms();
                    d.StartPosition = FormStartPosition.CenterParent;
                    d.ShowDialog();
                }
            }


        }

        private void btn_AcceptBank_Click(object sender, EventArgs e)
        {
            //var Datas = new List<TradeDataSet.TradesRow>();
            //for (int i = 0; i < dataGridView1.RowCount; i++)
            //{
            //    var _Cell = dataGridView1[CheckBox.Index, i] as DataGridViewDisableCheckBoxCell;
            //    if (_Cell.Value != null && (bool)_Cell.Value)
            //    {
            //        var _Row = ((DataRowView)dataGridView1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
            //        _Row.RejectChanges();
            //        if (_Row.ServiceState == 1  && _Row.HasAcc == false)
            //            Datas.Add(_Row);
            //    }
            //}
            //if (Datas.Any())
            //{
            //    //FrmLogin_B f = new FrmLogin_B();
            //    //f.ShowDialog();
            //    //if (f.Accepted)
            //    //{
            //        Dialog_AcceptBank d = new Dialog_AcceptBank();
            //        d.Trades = Datas.ToArray();
            //        //d.AuthKey = f.AuthKey;
            //        d.ShowDialog();
            //        btn_Search_Click(null, null);
            //    //}
            //}
            //else
            //{
            //    MessageBox.Show("먼저 결제할 항목들을 선택하여 주십시오.");
            //}
        }
       private void TradebasketBinding()
        { }
        private void btnBankCreat_Click(object sender, EventArgs e)
        {
            var Datas = new List<TradeDataSet.TradesBasKetRow>();

            if(!BankBasKetList.Any())
            {
                MessageBox.Show("먼저 생성할 항목들을 선택하여 주십시오.");
                return;
            }
         

          
                BankBaseketInsert();




            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                tradeDataSet.TradesBasKet.Clear();
                cn.Open();



                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText =
                        $@"SELECT distinct Trades.TradeId, Trades.RequestDate, BeginDate, EndDate, trades.Item, Trades.Price, VAT, Amount, PayState, PayDate, Trades.PayBankName, Trades.PayBankCode, Trades.PayAccountNo, Trades.PayInputName, Trades.DriverId, Trades.ClientId, Trades.UseTax, LGD_OID, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, AllowAcc, HasAcc, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, Trades.SourceType, Drivers.LoginId AS DriverLoginId, Drivers.CarNo AS DriverCarNo, Drivers.BizNo AS DriverBizNo, Drivers.Name AS DriverName, Drivers.ServiceState, Drivers.MID, 
                Clients.Code AS ClientCode, Clients.Name AS ClientName, Trades.AcceptCount, Trades.SubClientId, Trades.ClientUserId,(SELECT ISNULL(GroupName, '미설정') FROM DriverInstances WHERE DriverId = Trades.DriverId and ClientId = Trades.ClientId) as GroupName ,Trades.ExcelExportYN,ISNULL(Trades.EtaxCanCelYN, 'N') AS EtaxCanCelYN, trusteeMgtKey, TransportDate, Trades.StartState,Trades.StopState,Trades.PdfFileName,Trades.AipId,Trades.DocId,Trades.DeleteYn,Trades.UpdateDateTime--,Orders.TradeId AS OTradeId
                FROM     Trades
                JOIN Drivers ON Trades.DriverId = Drivers.DriverId
                JOIN Clients ON Trades.ClientId = Clients.ClientId

                LEFT JOIN Orders ON Trades.TradeId = Orders.TradeId 
                WHERE Trades.ClientId = {LocalUser.Instance.LogInInformation.ClientId} AND Trades.Tradeid IN({ String.Join(",", BankBasKetList)})";

                    using (SqlDataReader _Reader = cmd.ExecuteReader())
                    {
                        tradeDataSet.TradesBasKet.Load(_Reader);


                    }
                }
                cn.Close();

            }

            tradesBasKetbindingSource.DataSource = tradeDataSet.TradesBasKet;

            for (int i = 0; i < tradesBasKetbindingSource.Count ; i++)
            {
                var _Row = ((DataRowView)tradesBasKetbindingSource[i]).Row as TradeDataSet.TradesBasKetRow;

                _Row.RejectChanges();

                Datas.Add(_Row);

            }


            if (Datas.Any())
            {

                Dialog_BankCreate d = new Dialog_BankCreate();
                d.Trades = Datas.ToArray();

                String DCarNo = "";
                if (d.ShowDialog() == DialogResult.OK)
                {
                    //lblBankBasketCount.Text = "0";
                    //BankBasKetList.Clear();


                    DCarNo = d.CarNo;

                    if (DCarNo == "")
                        return;

                    FrmMDI frmMDI = new FrmMDI();

                    FrmMN0203_CAROWNERMANAGE _Form = new FrmMN0203_CAROWNERMANAGE(DCarNo);
                    _Form.MdiParent = this.MdiParent;

                    _Form.Show();

                    this.Close();


                }
                else if (d.DialogResult == DialogResult.Yes)
                {
                    BankBasKetList.Clear();
                    lblBankBasketCount.Text = "0";

                    btn_Search_Click(null, null);

                    FrmHelpBank Fhd = new FrmHelpBank();


                    Fhd.ShowDialog();
                }
                else
                {
                    //BankBasKetList.Clear();
                    //lblBankBasketCount.Text = "0";

                }


            }
            else
            {
                MessageBox.Show("먼저 생성할 항목들을 선택하여 주십시오.");
            }
        }

        //이체결과적용
        private void btnBankUpdate_Click(object sender, EventArgs e)
        {
            //FrmLogin_B f = new FrmLogin_B();
            //f.ShowDialog();
            //if (f.Accepted)
            //{
            //var Datas = new List<TradeDataSet.TradesRow>();
            //for (int i = 0; i < tradeDataSet.Trades.Count; i++)
            //{

            //    var _Row = ((DataRowView)dataGridView1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
            //    _Row.RejectChanges();
            //    //if (_Row.HasAcc == false)
            //        Datas.Add(_Row);

            //}


            //Dialog_BankUpload d = new Dialog_BankUpload();
            //d.Trades = Datas.ToArray();
            ////d.AuthKey = f.AuthKey;
            //d.ShowDialog();
            //btn_Search_Click(null, null);
            //}

            Dialog_BankResult d = new Dialog_BankResult();
            

            if (d.ShowDialog() == DialogResult.OK)
            {

                btn_Search_Click(null, null);

                lblBankBasketCount.Text = "0";
                BankBasKetList.Clear();
            }
        }

        private void btn_Help_Click(object sender, EventArgs e)
        {
            FrmHelpBank d = new FrmHelpBank();
           
           
            d.ShowDialog();
            
        }

        private void chk_EtaxCancle_CheckedChanged(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.naver.com/edubill365/221372110738");
        }

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DateTime dateNow = System.DateTime.Now;
            //DateTime firstDayOfMonth = System.DateTime.Parse(dateNow.ToString("yyy-MM-01"));
            //string lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            



            switch (cmbSMonth.SelectedIndex)
            {
              
              
                //당월
                case 0:
                    dtp_From.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_To.Value = DateTime.Now;
                    break;
                //전월
                case 1:
                    dtp_From.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_To.Value =DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 2:
                    dtp_From.Value = DateTime.Now;
                    dtp_To.Value = DateTime.Now;
                    break;
            }
        }

        List<int> BankBasKetList = new List<int>();
        private void btnBankBasKet_Click(object sender, EventArgs e)
        {
            //TradeDataSet.TradesBasKetRow row = tradeDataSet.TradesBasKet.NewTradesBasKetRow();
            //for (int i = 0; i < dataGridView1.RowCount; i++)
            //{
            //    var _Cell = dataGridView1[CheckBox.Index, i] as DataGridViewDisableCheckBoxCell;
            //    if (_Cell.Value != null && (bool)_Cell.Value)
            //    {
            //        var _Row = ((DataRowView)dataGridView1.Rows[i].DataBoundItem).Row as TradeDataSet.TradesRow;
            //        _Row.RejectChanges();
            //        if (!BankBasKetList.Contains(_Row.TradeId))
            //        {
            //            BankBasKetList.Add(_Row.TradeId);
                       
            //        }
            //    }
            //}

           
          


            //lblBankBasketCount.Text = BankBasKetList.Count().ToString("N0");
        }

        private void BankBaseketInsert()
        {

            if (BankBasKetList.Any())
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    using (SqlCommand Deletecmd = cn.CreateCommand())
                    {
                        Deletecmd.CommandText =
                           "DELETE  TradesBasKet" +
                           " WHERE ClientId = @ClientId";

                        Deletecmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        Deletecmd.ExecuteNonQuery();
                    }
                    cn.Close();
                }

                pnProgress.Visible = true;
                bar.Value = 0;
                bar.Maximum = tradesBasKetbindingSource.Count;
                Thread t = new Thread(new ThreadStart(() =>
                {
                    foreach (var bankbasket in BankBasKetList)
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {

                            cn.Open();



                            using (SqlCommand cmd = cn.CreateCommand())
                            {
                                cmd.CommandText =
                                    "INSERT INTO  TradesBasKet" +
                                    "(TradeId, RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, trusteeMgtKey, EtaxCanCelYN, ExcelExportYN, TransportDate, StartState, StopState, PdfFileName, AipId, DocId, DeleteYn, UpdateDateTime)  " +
                                    " SELECT TradeId, RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, trusteeMgtKey, EtaxCanCelYN, ExcelExportYN, TransportDate, StartState, StopState, PdfFileName, AipId, DocId, DeleteYn, UpdateDateTime FROM Trades" +
                                    " WHERE TradeId = @TradeId";

                                cmd.Parameters.AddWithValue("@TradeId", bankbasket);
                                cmd.ExecuteNonQuery();
                            }
                            cn.Close();

                        }
                    }
                    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));



                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();




            }


            //tradesBasKetTableAdapter.Fill(tradeDataSet.TradesBasKet);




            lblBankBasketCount.Text = BankBasKetList.Count().ToString("N0");
        }
        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = sender as Control as Button;


            btn.ForeColor = btn.Enabled == false ? Color.White : Color.White;
            btn.BackColor = btn.Enabled == false ? System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(194))))) : System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                using (SqlCommand Deletecmd = cn.CreateCommand())
                {
                    Deletecmd.CommandText =
                       "DELETE  TradesBasKet" +
                       " WHERE ClientId = @ClientId";

                    Deletecmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    Deletecmd.ExecuteNonQuery();
                }
                cn.Close();
            }

            BankBasKetList.Clear();

            lblBankBasketCount.Text = BankBasKetList.Count().ToString("N0");

            btn_Search_Click(null, null);

        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);
            FrmGridProperty _frmProperty = new FrmGridProperty(grid1,

             CheckBox
            , tradeIdDataGridViewTextBoxColumn
            , SubClientId
            , ColumnPayState
            , ClientCode
            , ClientName
            , TransportDate
            , requestDateDataGridViewTextBoxColumn
            , ColumnHasAcc
            //, Code
            , ServiceState
            , CarNo
            , ColumnDriverName
            , GroupName
            , bizNoDataGridViewTextBoxColumn
            , nameDataGridViewTextBoxColumn
            , StartState
            , StopState
            , DriverPhoneNo
            //, ColumnLGD_Last_Function
            , priceDataGridViewTextBoxColumn
            , vATDataGridViewTextBoxColumn
            , amountDataGridViewTextBoxColumn
            , ColumnAcceptCount
            , ColumnOrderText
            , payBankNameDataGridViewTextBoxColumn
            , payAccountNoDataGridViewTextBoxColumn
            , payInputNameDataGridViewTextBoxColumn
            , ColumnsReferralId
            , LGD_Last_Function
            , ExcelExportYN
            , ColumnLGD_Last_Date
           // , HasETaxGubun
            , ColumnHasETax
            , SelectTax
            , ShowTaxOut
            , ShowTax
            , Edocument
           // , ColumnImage1
          //  , ColumnAccLog_


           );
            _frmProperty.ShowDialog();
            _FormStyle.WriteFormStyle(this, grid1);
        }
        private void NiceEtaxEdit(int tradeId)
        {
            InitDriverTable();
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);

            var _Clients = LocalUser.Instance.LogInInformation.Client;
            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            // var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";


            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Clients.NPKIpasswd }).ToString();


            string DutyNum = string.Empty;
            string DescriptionText = "";
            int i = 0;

            //var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
            //if (_HideAccountNo == 0)
            //{
                DescriptionText = "▶운송 / 주선사用 '차세로' PC프로그램 안내 운송 / 주선사에서는 http://www.chasero.co.kr 접속하여 프로그램을 설치하시면, 화물차주가 발행한 '전자세금계산서' 및 '첨부화일(인수증/송장 등)'을 조회해 보실 수 있습니다. 문의: 1661 - 6090";
            //}
            //else
            //{
            //    DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner;

            //}
            var _Query = tradeDataSet.Trades.Where(c => c.TradeId == tradeId).FirstOrDefault();
            var _Driver = _DriverTable.Where(c => c.DriverId == _Query.DriverId).FirstOrDefault();
            var _Admininfo = baseDataSet.AdminInfoes.FirstOrDefault();
            decimal _Price = 0;
            decimal _Vat = 0;
            decimal _Amont = 0;

            _Price = _Query.Price * -1;
            _Vat = _Query.VAT * -1;
            _Amont = _Query.Amount * -1;
           
            string _TypeCode = "0203";
            if (Convert.ToInt64(_Vat) == 0)
            {
                _TypeCode = "0205";
            }
            string Tdtp_Date = DateTime.Now.ToString("yyyyMMdd");

            _Amont = _Price + _Vat;


            string dtiXml = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            " xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            "   <TaxInvoiceDocument>\r\n" +
            $"       <TypeCode>{_TypeCode}</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            $"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            $"      <IssueDateTime>{Tdtp_Date}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            $"      <AmendmentStatusCode>01</AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            $"      <PurposeCode>02</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            $"      <OriginalIssueID>{_Query.billNo}</OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            $"  </TaxInvoiceDocument>\r\n" +
            $"  <TaxInvoiceTradeSettlement>\r\n" +
            $"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            $"          <ID>{_Driver.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Driver.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            $"          <NameText>{_Driver.Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Driver.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Driver.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <URICommunication>{_Driver.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Driver.AddressState + " " + _Driver.AddressCity + " " + _Driver.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>" +
            $"      </InvoicerParty>	<!-- 공급자정보 끝 -->\r\n" +
            $"      <InvoiceeParty>	<!-- 공급받는자정보 시작 -->\r\n" +
            $"          <ID>{_Clients.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Clients.Uptae}</TypeCode>	<!-- 업태 -->\r\n" +
            $"          <NameText>{_Clients.Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Clients.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"              <BusinessTypeCode>01</BusinessTypeCode>	<!-- 사업자등록번호 구분코드 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Clients.CEO}</NameText>	<!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <PrimaryDefinedContact>\r\n" +
            $"              <PersonNameText>{_Clients.CEO}</PersonNameText>	<!-- 담당자명 -->\r\n" +
            $"              <DepartmentNameText></DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <TelephoneCommunication>{_Clients.MobileNo}</TelephoneCommunication>	<!-- 전화번호 -->\r\n" +
            $"              <URICommunication>{_Clients.Email}</URICommunication>	<!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </PrimaryDefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Clients.AddressState + " " + _Clients.AddressCity + " " + _Clients.AddressDetail}</LineOneText>	<!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"      </InvoiceeParty>	<!-- 공급받는자정보 끝 -->\r\n" +
            $"      <BrokerParty> <!-- 수탁자정보 --> \r\n" +
            $"          <ID>{_Admininfo.BIzNo.Replace("-", "")}</ID>     <!-- 사업자번호 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>         <!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +

            $"          <NameText>{_Admininfo.CustName}</NameText>         <!-- 상호명 -->\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Admininfo.ownerName}</NameText>       <!--  대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Admininfo.addr1 + " " + _Admininfo.addr2}</LineOneText>          <!-- 사업장주소 -->\r\n" +
            $"          </SpecifiedAddress>\r\n" +
            $"          <TypeCode>{_Admininfo.BizCond}</TypeCode>  <!-- 업태 -->\r\n" +
            $"          <ClassificationCode>{_Admininfo.bizItem}</ClassificationCode>   <!-- 종목 -->\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <DepartmentNameText>물류팀</DepartmentNameText> <!-- 부서명 -->\r\n" +
            $"              <PersonNameText>{_Admininfo.rsbmName}</PersonNameText>      <!-- 담당자명 -->\r\n" +
            $"              <TelephoneCommunication>{_Admininfo.hpNo}</TelephoneCommunication>           <!-- 전화번호 -->\r\n" +
            $"             	<URICommunication>{_Admininfo.email}</URICommunication>  <!-- 담당자 이메일(이메일 발송할 주소) -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"      </BrokerParty> <!-- 수탁자정보 끝 -->\r\n" +
            $"          <SpecifiedPaymentMeans>\r\n" +
            $"              <TypeCode>10</TypeCode> <!-- 10:현금, 20:수표, 30:어음, 40:외상미수금 -->\r\n" +
            $"              <PaidAmount>{Convert.ToInt64(_Amont)}</PaidAmount> <!-- 부가세가 포함된 금액 -->\r\n" +
            $"          </SpecifiedPaymentMeans>\r\n" +
            $"          <SpecifiedMonetarySummation>\r\n" +
            $"              <ChargeTotalAmount>{Convert.ToInt64(_Price)}</ChargeTotalAmount>	<!-- 총 공급가액 -->\r\n" +
            $"              <TaxTotalAmount>{Convert.ToInt64(_Vat)}</TaxTotalAmount>	<!-- 총 세액 -->\r\n" +
            $"              <GrandTotalAmount>{Convert.ToInt64(_Amont)}</GrandTotalAmount>	<!-- 총액(공급가액+세액) -->\r\n" +
            $"          </SpecifiedMonetarySummation>\r\n" +
            $"  </TaxInvoiceTradeSettlement>\r\n";

           
                dtiXml += "" +
                  $"  <TaxInvoiceTradeLineItem>	<!-- 품목정보 시작 -->\r\n" +
                  $"      <SequenceNumeric>1</SequenceNumeric>           <!-- 일련번호 -->\r\n" +
                  $"      <InvoiceAmount>{Convert.ToInt64(_Price)}</InvoiceAmount>   <!-- 물품공급가액 -->\r\n" +
                  $"      <ChargeableUnitQuantity>1</ChargeableUnitQuantity>       <!-- 물품수량 -->\r\n" +
                  $"      <InformationText></InformationText> <!-- 규격 -->\r\n" +
                  $"      <NameText>{_Query.Item}</NameText>  <!-- 품목명 -->\r\n" +
                  $"      <PurchaseExpiryDateTime>{Tdtp_Date.Replace("/", "")}</PurchaseExpiryDateTime>\r\n" +
                  $"          <TotalTax>\r\n" +
                  $"              <CalculatedAmount>{Convert.ToInt64(_Vat)}</CalculatedAmount>       <!-- 물품세액 -->\r\n" +
                  $"          </TotalTax>\r\n" +
                  $"          <UnitPrice>\r\n" +
                  $"              <UnitAmount>{Convert.ToInt64(_Price)}</UnitAmount>          <!-- 물품단가 -->\r\n" +
                  $"          </UnitPrice>\r\n" +
                  $"          <DescriptionText></DescriptionText>       <!-- 품목비고 -->\r\n" +
                  $"  </TaxInvoiceTradeLineItem>         <!-- 품목정보 끝 --> \r\n";


            dtiXml += "" +
            "</TaxInvoice>";










            //try
            //{
            //    webBrowser1.Url = new Uri("http://222.231.9.253/NiceEncoding.asp");
            //    //frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.frnNo }).ToString();
            //    //userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.userid }).ToString();
            //    passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
            //  //  Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.Linkcd }).ToString();


            //    //MessageBox.Show(Encode.ToString());

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.Write(ex.Message);

            //}



            var Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, _Admininfo.certpwEnc, dtiXml, "Y", "N", "", "3");

            try
            {
                var ResultList = Result.Split('/');

                var retVal = ResultList[0];
                var errMsg = ResultList[1];
                var billNo = ResultList[2];
                var gnlPoint = ResultList[3];
                var outbnsPoint = ResultList[4];
                var totPoint = ResultList[5];
                //return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

                if (retVal == "0")
                {
                    //승인번호 DB반영

                    try
                    {


                        try
                        {
                            try
                            {
                              
                                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                                {

                                    cn.Open();
                                    SqlCommand cmd = cn.CreateCommand();
                                    cmd.CommandText =
                                        "INSERT INTO  Trades" +
                                        "(RequestDate, BeginDate, EndDate, DriverName, Item, Price, VAT, Amount, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, billNo, EtaxCanCelYN, ExcelExportYN, TransportDate, StartState, StopState)  " +
                                        " SELECT getdate(), BeginDate, EndDate, DriverName, Item, Price *-1, VAT*-1, Amount*-1, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, DriverId, ClientId, PayResult, UseTax, SetYN, LGD_OID, AllowAcc, HasAcc, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, CustomerAccId, ClientAccId, SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, ExcelFile, Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, HasETax, CEO, Uptae, Upjong, Email, Name, BizNo, Address, CName, CBizNo, SourceType, AcceptCount, SubClientId, ClientUserId, Fee, Cost, PointMethod, Fee_VAT, CustomerId, '" + billNo + "', 'Y', ExcelExportYN, TransportDate, StartState, StopState FROM Trades" +
                                        " WHERE TradeId = @TradeId" +
                                        " UPDATE Trades SET EtaxCanCelYN = 'Y' " +
                                        " WHERE TradeId = @TradeId";



                                    cmd.Parameters.AddWithValue("@TradeId", tradeId);
                                    cmd.ExecuteNonQuery();


                                    cmd.CommandText =
                                   " UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";

                                    cmd.ExecuteNonQuery();


                                    cmd.CommandText =
                                    " UPDATE Orders SET TaxTradeId = null  WHERE TaxTradeId = @TradeId";

                                 
                                    using (SqlCommand _DICommand = cn.CreateCommand())
                                    {
                                        _DICommand.CommandText =
                                        " UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId ";
                                        _DICommand.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                                        _DICommand.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                                        _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                        _DICommand.ExecuteNonQuery();
                                    }

                                }

                                MessageBox.Show("발행 취소 되었습니다.");

                                btn_Search_Click(null, null);


                            }
                            catch (PopbillException ex)
                            {
                                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                                return;
                            }

                            MessageBox.Show("발행 취소 되었습니다.");

                            btn_Search_Click(null, null);
                        }
                        catch (PopbillException ex)
                        {
                            MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                           
                        }
                    }
                    catch (PopbillException ex)
                    {
                      
                    }
                }
                else
                {
                  
                }

            }
            catch
            {

            }

        }

        private void NiceEtaxDelete(int tradeId)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);

            var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
            LocalUser.Instance.LogInInformation.LoadClient();
            var _Clients = LocalUser.Instance.LogInInformation.Client;
            var Selected = tradeDataSet.Trades.Where(c => c.TradeId == tradeId).FirstOrDefault();
            var _Trade = tradeDataSet.Trades.FirstOrDefault(c => c.TradeId == tradeId);
            try
            {
               



                #region 세금계산서 삭제
                var Result = dTIServiceClass.updateEtaxStatusToZ("EDB", _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, _Trade.billNo);

                try
                {
                    var ResultList = Result.Split('/');

                    var ResultListretVal = ResultList[0];
                    var ResultListerrMsg = ResultList[1];

                    if (ResultListretVal == "0")
                    {
                        try
                        {

                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                SqlCommand cmd = cn.CreateCommand();
                                cmd.CommandText =
                                    "Delete Trades  WHERE TradeId = @TradeId";
                                cmd.Parameters.AddWithValue("@TradeId", tradeId);
                                cmd.ExecuteNonQuery();


                                cmd.CommandText =
                                  " UPDATE Orders SET TradeId = null  WHERE TradeId = @TradeId";
                                // cmd.Parameters.AddWithValue("@TradeId", Selected.TradeId);
                                cmd.ExecuteNonQuery();


                           

                                // cmd.CommandText =
                                //" UPDATE Drivers SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId";
                                using (SqlCommand _DICommand = cn.CreateCommand())
                                {
                                    _DICommand.CommandText =
                                    " UPDATE DriverInstances  SET Mizi = Mizi-@Mizi  WHERE DriverId = @DriverId AND ClientId = @ClientId ";
                                    _DICommand.Parameters.AddWithValue("@DriverId", _Trade.DriverId);
                                    _DICommand.Parameters.AddWithValue("@Mizi", _Trade.Amount);
                                    _DICommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                    _DICommand.ExecuteNonQuery();
                                }

                                // cmd.ExecuteNonQuery();

                                if (!String.IsNullOrEmpty(_Trade.PdfFileName))
                                {
                                    if (!String.IsNullOrEmpty(_Trade.AipId))
                                    {
                                        SqlCommand cmdDocument = cn.CreateCommand();
                                        cmdDocument.CommandText =
                                            " INSERT INTO DocuTable(TradeId,PdfFileName,AipId,DocId,Status,RequestDateTime) " +
                                            " Values(@TradeId,@PdfFileName,@AipId,@DocId,@Status,getdate())";
                                        cmdDocument.Parameters.AddWithValue("@Status", "_40_ready");
                                        cmdDocument.Parameters.AddWithValue("@TradeId", tradeId);
                                        cmdDocument.Parameters.AddWithValue("@PdfFileName", _Trade.PdfFileName);
                                        cmdDocument.Parameters.AddWithValue("@AipId", _Trade.AipId);
                                        cmdDocument.Parameters.AddWithValue("@DocId", _Trade.DocId);
                                        cmdDocument.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        SqlCommand cmdDocument = cn.CreateCommand();
                                        cmdDocument.CommandText =
                                            "Delete DocuTable  WHERE TradeId = @TradeId";
                                        cmdDocument.Parameters.AddWithValue("@TradeId", tradeId);
                                        cmdDocument.ExecuteNonQuery();

                                    }
                                }
                                cn.Close();
                            }

                           

                            MessageBox.Show("전자세금계산서가 삭제 되었습니다.");
                            //Success++;
                            btn_Search_Click(null, null);
                        }

                        catch (PopbillException ext)
                        {
                            MessageBox.Show(ext.code.ToString() + "\r\n" + ext.Message);
                            return;
                        }
                    }
                    else
                    {

                    }

                }
                catch
                {

                }
                #endregion

            }
            catch
            {

            }
        }
        private void 종이인수증등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tradesBindingSource.Current != null)
            {
                var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;

                if (Selected == null)
                    return;
                if (Selected.HasETax == true && Selected.AllowAcc == false)
                {
                    MessageBox.Show("전자세금계산서에는 종이인수증을 등록할 수 없습니다.", "종이인수증등록", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                FrmTradePaperAdd _Form = new FrmTradePaperAdd(Selected.DriverId,Selected.TradeId);

                _Form.Owner = this;
                _Form.StartPosition = FormStartPosition.CenterParent;
                _Form.ShowDialog();

                btn_Search_Click(null, null);
            }
        }
        private void TaxGubunEdit(string UGubun, int UTradeId)
        {
            string _Gubun = UGubun;
            int _TradeId = UTradeId;

            if (!String.IsNullOrEmpty(_Gubun))
            {

                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {

                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();

                    cmd.CommandText =
                       "UPDATE Trades SET TaxLoginId = @TaxLoginId, TaxGubun = @TaxGubun  WHERE    TradeId =" + _TradeId + " ";
                    //cmd.Parameters.AddWithValue("@IssueDate", dtp_RequestDate.Value);
                    cmd.Parameters.AddWithValue("@TaxLoginId", LocalUser.Instance.LogInInformation.LoginId);
                    cmd.Parameters.AddWithValue("@TaxGubun", _Gubun);
                    cmd.ExecuteNonQuery();


                    cn.Close();
                }



            }

            try
            {
                if (grid1.RowCount > 1)
                {
                    GridIndex = tradesBindingSource.Position;
                    btn_Search_Click(null, null);
                    grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];
                }
                else
                {
                    btn_Search_Click(null, null);
                }
            }
            catch { }
        }
        private void 외부종이ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;

            if (Selected.HasETax == true && Selected.AllowAcc == false)
            {
                return;
            }

            if (Selected.AllowAcc == true && String.IsNullOrEmpty(Selected.TradeId.ToString()))
            {
                return;
            }
            TaxGubunEdit("외부(종이)", Selected.TradeId);
               
           
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tradesBindingSource.Current == null)
                return;
            

            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;
            if (Selected.HasETax == true && Selected.AllowAcc == false)
            {
                MessageBox.Show("외부계산서만 수정 가능합니다.", "매입관리", MessageBoxButtons.OK, MessageBoxIcon.Information);

              

                e.Cancel = true;

            }
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                var uOrder = ShareOrderDataSet.Orders.Where(c => c.TradeId == Selected.TradeId).Count();

                if (Selected.AllowAcc == true && uOrder ==0)
                {
                    MessageBox.Show("외부계산서만 수정 가능합니다.", "매입관리", MessageBoxButtons.OK, MessageBoxIcon.Information);



                    e.Cancel = true;
                }
            }
        }

        private void 외부e세로ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;

            if (Selected.HasETax == true && Selected.AllowAcc == false)
            {
                return;
            }

            if (Selected.AllowAcc == true && String.IsNullOrEmpty(Selected.TradeId.ToString()))
            {
                return;
            }
            TaxGubunEdit("외부(e-세로)", Selected.TradeId);
               

        }

        private void 외부24시콜ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;

            if (Selected.HasETax == true && Selected.AllowAcc == false)
            {
                return;
            }

            if (Selected.AllowAcc == true && String.IsNullOrEmpty(Selected.TradeId.ToString()))
            {
                return;
            }
            TaxGubunEdit("외부(24시콜)", Selected.TradeId);
                
        }

        private void 외부화물맨ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;

            if (Selected.HasETax == true && Selected.AllowAcc == false)
            {
                return;
            }

            if (Selected.AllowAcc == true && String.IsNullOrEmpty(Selected.TradeId.ToString()))
            {
                return;
            }
            TaxGubunEdit("외부(화물맨)", Selected.TradeId);


        }

        private void 외부원콜ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;

            if (Selected.HasETax == true && Selected.AllowAcc == false)
            {
                return;
            }

            if (Selected.AllowAcc == true && String.IsNullOrEmpty(Selected.TradeId.ToString()))
            {
                return;
            }

            TaxGubunEdit("외부(원콜)", Selected.TradeId);


        }



        private void button2_Click(object sender, EventArgs e)
        {
            if(LocalUser.Instance.LogInInformation.IsAdmin)
            {
                return;
            }
            string sParameter = "";
            string EncodingSparameter = "";

            //var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;



            //if (Selected != null)

            //{
            //현재시간(년월일시분초);공급자사업자번호;공급받는자사업자번호;연계코드
            sParameter = DateTime.Now.ToString("yyyyMMddHHmmss") + ";" + "" + ";" + LocalUser.Instance.LogInInformation.Client.BizNo.Replace("-", "") + ";" + "EDB";
            EncodingSparameter = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { sParameter }).ToString();



            string value = $"data={EncodingSparameter}";
            var Query = baseDataSet.AdminInfoes.FirstOrDefault();
            var url = "http://t-renewal.nicedata.co.kr/ti/TI_80401.do?";
            if (Query.IsTest != true)
            {
                url = "http://www.nicedata.co.kr/ti/TI_80401.do?";
            }

            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Internet Explorer\\iexplore.exe", url + value);

            //FrmNice f = new FrmNice(value, url);
            //f.Show();



            //
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormStyle _FormStyle = new mycalltruck.Admin.Class.XML.FormStyle(this, grid1);


            _FormStyle.WriteFormStyle(this, grid1);

            MessageBox.Show("저장하였습니다.");
        }

        private void 차량계좌번호변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tradesBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)tradesBindingSource.Current).Row as TradeDataSet.TradesRow;

            if (Selected == null)
            {
                return;
            }
            else
            {
                var _Driver = mDriverRepository.NoGetDriver(Selected.DriverId);

                if (_Driver.ServiceState == 1)
                {
                    MessageBox.Show("서비스 상태가 제공상태일때는 계좌번호를 변경 할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                FrmBank frmBank = new FrmBank(Selected.DriverId, Selected.TradeId);

                frmBank.StartPosition = FormStartPosition.CenterScreen;
                frmBank.Show();
            }
        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
          //  NiceEtaxEdit(57161);
        }
    }
}
