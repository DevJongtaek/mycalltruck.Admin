using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using OfficeOpenXml;
using Popbill;
using Popbill.Taxinvoice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMNClientVaccount : Form
    {
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

        #region 전자세금계산서NiceDNR
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        string linkcd = "EDB";
        string linkId = "edubillsys";
        #endregion


        public FrmMNClientVaccount()
        {
            InitializeComponent();
        }

        private void FrmMNClientVaccount_Load(object sender, EventArgs e)
        {
            //전자세금계산서 모듈 초기화
            taxinvoiceService = new TaxinvoiceService(LinkID, SecretKey);

            //연동환경 설정값, 테스트용(true), 상업용(false)
            taxinvoiceService.IsTest = false;


            // TODO: 이 코드는 데이터를 'pointsDataSet.ClientPointsList' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.clientPointsListTableAdapter.Fill(this.pointsDataSet.ClientPointsList);
            dtp_Sdate.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtp_Edate.Value = DateTime.Now;
            InitCmb();
          
            btn_Inew_Click(null, null);
        }

        public void InitCmb()
        {
            Dictionary<string, string> Smonth = new Dictionary<string, string>
            {
                { "당일", "당일" },
                { "전일", "전일" },
                { "금주", "금주" },
                { "금월", "금월" },
                { "전월", "전월" },
                { "지정", "지정" }
            };

            cmbSMonth.DataSource = new BindingSource(Smonth, null);
            cmbSMonth.DisplayMember = "Value";
            cmbSMonth.ValueMember = "Key";

            cmbSMonth.SelectedIndex = 0;








            Dictionary<string, string> Ssearch = new Dictionary<string, string>
            {
                { "전체", "전체" },
                { "사업자번호", "사업자번호" },
                { "상호", "상호" },
                { "대표자", "대표자" },

            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


        }
        private String GetSelectCommand()
        {
            return @"SELECT   CP.ClientPointId, CP.CDate, CP.Amount, C.Code, C.Name, C.BizNo, C.CEO, C.VBankName, C.VAccount, CP.ClientId, CP.EtaxCanCelYN, CP.EtaxWriteDate, CP.invoicerMgtKey, CP.IssueState,  CP.Issue
                  FROM     ClientPoints AS CP INNER JOIN
                    Clients AS C ON CP.ClientId = C.ClientId
                  ";


        }
        private void LoadTable()
        {
            pointsDataSet.ClientPointsList.Clear();
            

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

                    WhereStringList.Add("CP.Remark = '가상계좌 입금'");

                   

                  //  WhereStringList.Add("CP.ClientId = @ClientId");
                 //   _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    WhereStringList.Add("CONVERT(VARCHAR(10),CP.CDate,111) >= @Sdate AND CONVERT(VARCHAR(10),CP.CDate,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate", dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    if (cmbSSearch.Text == "상호")
                    {
                        WhereStringList.Add(string.Format("C.Name Like '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "사업자번호")
                    {
                        WhereStringList.Add(string.Format("REPLACE(C.BizNo,'-','') Like  '%{0}%'", txtSText.Text.Replace("-","")));
                    }
                    else if (cmbSSearch.Text == "대표자")
                    {
                        WhereStringList.Add(string.Format("C.CEO Like  '%{0}%'", txtSText.Text));
                    }

                

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by CP.CDate  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        pointsDataSet.ClientPointsList.Load(_Reader);


                        //if (grid1.RowCount > 0)
                        //{
                           
                        //    grid1.CurrentCell = grid1.Rows[GridIndex].Cells[0];

                        //    nSTATS1BindingSource_CurrentChanged(null, null);
                        //}


                    }
                }


                _Connection.Close();


            }

          
        }


        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmbSMonth.SelectedIndex = 0;
            dtp_Sdate.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtp_Edate.Value = DateTime.Now;

           
            cmbSSearch.SelectedIndex = 0;
            txtSText.Clear();
            btnSearch_Click(null, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void txtSText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            LoadTable();
        }

        private void grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as PointsDataSet.ClientPointsListRow;


            if (grid1.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (grid1.Rows.Count - e.RowIndex).ToString("N0");
            }

            if (grid1.Columns[e.ColumnIndex] == cDateDataGridViewTextBoxColumn)
            {
                e.Value = Selected.CDate.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (grid1.Columns[e.ColumnIndex] == ColumnEtaxWriteDate)
            {
                if (Selected.IssueState != "발행")
                {
                    e.Value = "";
                }
                else
                {
                    e.Value = Selected.EtaxWriteDate.ToString("yyyy-MM-dd");
                }
            }
        }

        private void cmbSMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSMonth.SelectedIndex)
            {
                //당일
                case 0:
                    dtp_Sdate.Value = DateTime.Now;
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전일
                case 1:
                    dtp_Sdate.Value = DateTime.Now.AddDays(-1);
                    dtp_Edate.Value = DateTime.Now.AddDays(-1);
                    break;
                //금주
                case 2:
                    dtp_Sdate.Value = DateTime.Now.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(DateTime.Today.DayOfWeek));
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //금월
                case 3:
                    dtp_Sdate.Text = DateTime.Now.ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Now;
                    break;
                //전월
                case 4:
                    dtp_Sdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01");
                    dtp_Edate.Value = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1);
                    break;
                case 5:
                    dtp_Sdate.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                    dtp_Edate.Value = DateTime.Now;
                    break;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYSTATS);

            if (di.Exists == false)
            {

                di.Create();
            }
            var fileString = "주선사(가상계좌)충전내역" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;


                }


            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.ClientVaccountList))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            for (int i = 0; i < grid1.RowCount; i++)
            {
                _Sheet.Cells[RowIndex, 1].Value = grid1[rowNUMDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 2].Value = grid1[cDateDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 3].Value = grid1[amountDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 4].Value = grid1[codeDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 5].Value = grid1[nameDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 6].Value = grid1[bizNoDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 7].Value = grid1[cEODataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 8].Value = grid1[vBankNameDataGridViewTextBoxColumn.Index, i].FormattedValue;

                _Sheet.Cells[RowIndex, 9].Value = grid1[vAccountDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 10].Value = grid1[ColumnEtaxWriteDate.Index, i].FormattedValue;


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
        }

        private void btnEtax_Click(object sender, EventArgs e)
        {
            var Datas = new List<PointsDataSet.ClientPointsListRow>();
            for (int i = 0; i < grid1.RowCount; i++)
            {
                //var _Cell = grid1[SalesChk.Index, i] as DataGridViewDisableCheckBoxCell;
                //if (_Cell.Value != null && (bool)_Cell.Value)
                //{

                var _Row = ((DataRowView)grid1.Rows[i].DataBoundItem).Row as PointsDataSet.ClientPointsListRow;
                _Row.RejectChanges();

                _Row.EtaxWriteDate = DateTime.Now;
                Datas.Add(_Row);
                //}
            }
            if (Datas.Any())
            {
                //FrmLogin_B f = new FrmLogin_B();
                //f.ShowDialog();
                //if (f.Accepted)
                //{
                FrmClientVaccountEtax d = new FrmClientVaccountEtax(dtp_Sdate.Text, dtp_Edate.Text);
                d.Owner = this;
                d.StartPosition = FormStartPosition.CenterParent;
                d.Cpoints = Datas.Where(c=> c.Issue != true).ToArray();

                d.ShowDialog();
                btnSearch_Click(null, null);
                //}
            }
            else
            {
                MessageBox.Show("전사세금계산서 발행할 항목들을 선택하여 주십시오.");
            }




            //FrmClientVaccountEtax _Form = new FrmClientVaccountEtax(dtp_Sdate.Text, dtp_Edate.Text);
            //_Form.Owner = this;
            //_Form.StartPosition = FormStartPosition.CenterParent;
            
            //if (_Form.ShowDialog() == DialogResult.OK)
            //{
                


            //}
            //else
            //{
            //    btnSearch_Click(null, null);
            //}
        }

        private void grid1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as PointsDataSet.ClientPointsListRow;

            if (grid1.Columns[e.ColumnIndex] == ColumnEtaxCancle && e.RowIndex >= 0)
            {

                if (Selected != null)
                {
                    if (!Selected.Issue == true && Selected.IssueState != "발행")
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
        int Error = 0;
        int Success = 0;
        //승인번호
        string _billNo = "";
        //1.문서상태변경 2.신고상태변경
        string _changestatus = "";
        //문서상태 코드 또는 신고상태 코드
        string _changestatusCode = "";
        //국세청응답코드
        string _EtaxCode = "";
        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Success = 0;
            Error = 0;


            if (grid1.Columns[e.ColumnIndex] != ColumnEtaxCancle && e.RowIndex >= 0)
                return;
            var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as PointsDataSet.ClientPointsListRow;
            if (grid1.Columns[e.ColumnIndex] == ColumnEtaxCancle && e.RowIndex >= 0)
            {
                if (Selected != null)
                {
                    if (Selected.Issue == true && Selected.IssueState != "취소")
                    {


                        #region
                        //TaxinvoiceInfo taxinvoiceInfo = taxinvoiceService.GetInfo("1148654906", MgtKeyType.SELL, Selected.invoicerMgtKey);

                        //try
                        //{
                        //    string tmp = null;

                        //    tmp += "itemKey : " + taxinvoiceInfo.itemKey + CRLF;
                        //    tmp += "taxType : " + taxinvoiceInfo.taxType + CRLF;
                        //    tmp += "writeDate : " + taxinvoiceInfo.writeDate + CRLF;
                        //    tmp += "regDT : " + taxinvoiceInfo.regDT + CRLF;

                        //    tmp += "invoicerCorpName : " + taxinvoiceInfo.invoicerCorpName + CRLF;
                        //    tmp += "invoicerCorpNum : " + taxinvoiceInfo.invoicerCorpNum + CRLF;
                        //    tmp += "invoicerMgtKey : " + taxinvoiceInfo.invoicerMgtKey + CRLF;
                        //    tmp += "invoicerPrintYN : " + taxinvoiceInfo.invoicerPrintYN + CRLF;
                        //    tmp += "invoiceeCorpName : " + taxinvoiceInfo.invoiceeCorpName + CRLF;
                        //    tmp += "invoiceeCorpNum : " + taxinvoiceInfo.invoiceeCorpNum + CRLF;
                        //    tmp += "invoiceeMgtKey : " + taxinvoiceInfo.invoiceeMgtKey + CRLF;
                        //    tmp += "invoiceePrintYN : " + taxinvoiceInfo.invoiceePrintYN + CRLF;
                        //    tmp += "closeDownState : " + taxinvoiceInfo.closeDownState + CRLF;
                        //    tmp += "closeDownStateDate : " + taxinvoiceInfo.closeDownStateDate + CRLF;
                        //    tmp += "interOPYN : " + taxinvoiceInfo.interOPYN + CRLF;

                        //    tmp += "supplyCostTotal : " + taxinvoiceInfo.supplyCostTotal + CRLF;
                        //    tmp += "taxTotal : " + taxinvoiceInfo.taxTotal + CRLF;
                        //    tmp += "purposeType : " + taxinvoiceInfo.purposeType + CRLF;
                        //    tmp += "modifyCode : " + taxinvoiceInfo.modifyCode.ToString() + CRLF;
                        //    tmp += "issueType : " + taxinvoiceInfo.issueType + CRLF;

                        //    tmp += "issueDT : " + taxinvoiceInfo.issueDT + CRLF;
                        //    tmp += "preIssueDT : " + taxinvoiceInfo.preIssueDT + CRLF;

                        //    tmp += "stateCode : " + taxinvoiceInfo.stateCode.ToString() + CRLF;
                        //    tmp += "stateDT : " + taxinvoiceInfo.stateDT + CRLF;

                        //    tmp += "openYN : " + taxinvoiceInfo.openYN.ToString() + CRLF;
                        //    tmp += "openDT : " + taxinvoiceInfo.openDT + CRLF;
                        //    tmp += "ntsresult : " + taxinvoiceInfo.ntsresult + CRLF; //국세청전송결과
                        //    tmp += "ntsconfirmNum : " + taxinvoiceInfo.ntsconfirmNum + CRLF; //국세청승인번호
                        //    tmp += "ntssendDT : " + taxinvoiceInfo.ntssendDT + CRLF; //
                        //    tmp += "ntsresultDT : " + taxinvoiceInfo.ntsresultDT + CRLF;
                        //    tmp += "ntssendErrCode : " + taxinvoiceInfo.ntssendErrCode + CRLF;
                        //    tmp += "stateMemo : " + taxinvoiceInfo.stateMemo;
                        //    // MessageBox.Show(tmp, "문서 상태/요약 정보 조회");

                        //    if (!string.IsNullOrEmpty(taxinvoiceInfo.ntsresult))
                        //    {
                        //        TinfoNtsresult = taxinvoiceInfo.ntsresult;
                        //    }
                        //    TinfoWriteDate = taxinvoiceInfo.writeDate;
                        //    TinfoitemKey = taxinvoiceInfo.itemKey;
                        //    TinfostateCode = taxinvoiceInfo.stateCode.ToString();

                        //    switch (TinfostateCode)
                        //    {
                        //        case "300":
                        //            TinfostateName = "발행완료";
                        //            break;
                        //        case "310":
                        //            TinfostateName = "발행완료";
                        //            break;
                        //        case "301":
                        //            TinfostateName = "전송중";
                        //            break;
                        //        case "302":
                        //            TinfostateName = "전송중";
                        //            break;
                        //        case "303":
                        //            TinfostateName = "전송중";
                        //            break;
                        //        case "311":
                        //            TinfostateName = "전송중";
                        //            break;
                        //        case "312":
                        //            TinfostateName = "전송중";
                        //            break;
                        //        case "313":
                        //            TinfostateName = "전송중";
                        //            break;
                        //        case "304":
                        //            TinfostateName = "전송성공";
                        //            break;
                        //        case "314":
                        //            TinfostateName = "전송성공";
                        //            break;
                        //        case "305":
                        //            TinfostateName = "전송실패";
                        //            break;
                        //        case "315":
                        //            TinfostateName = "전송실패";
                        //            break;

                        //    }
                        //}

                        //catch (PopbillException ex)
                        //{
                        //    MessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                        //                    "응답메시지(message) : " + ex.Message, "문서 상태/요약 정보 조회");
                        //    return;
                        //}
                        #endregion

                        #region
                        //if (Selected.EtaxWriteDate.Date == DateTime.Now.Date)
                        //    {
                        //        if (TinfostateName == "발행완료")
                        //        {
                        //            if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 당일 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //            {
                        //                try
                        //                {
                        //                    SendCancelIssue(Selected.ClientPointId);
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //                    return;
                        //                }
                        //            }
                        //        }
                        //        else if (TinfostateName == "전송성공")
                        //        {
                        //            if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //            {
                        //                try
                        //                {
                        //                    MinusInvoice(Selected.EtaxWriteDate.ToString("yyyyMMdd"), Selected.invoicerMgtKey);
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //                    return;
                        //                }


                        //            }
                        //        }
                        //        else
                        //        {

                        //            MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //            return;
                        //        }



                        //    }
                        //    else
                        //    {

                        //        if (TinfostateName == "발행완료")
                        //        {
                        //            if (MessageBox.Show("정말 삭제하시겠습니까?\r\n( ※.국세청 미 전송, 발행 건이므로\r\n전표 자체가 삭제 됩니다. ", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //            {
                        //                try
                        //                {
                        //                    SendCancelIssue(Selected.ClientPointId);

                        //                    //SendMinusInvoice(Selected.DriverId, Selected.TradeId, Selected.ClientId, Selected.trusteeMgtKey);
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //                    return;
                        //                }
                        //            }
                        //        }
                        //        else if (TinfostateName == "전송성공")
                        //        {
                        //            if (MessageBox.Show("정말 발행취소 하시겠습니까?\r\n( ※.국세청에 기 전송된 건 입니다.\r\n같은 금액의 마이너스(-) 전표 발행으로취소 됩니다. )", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //            {
                        //                try
                        //                {

                        //                    MinusInvoice(Selected.EtaxWriteDate.ToString("yyyyMMdd"), Selected.invoicerMgtKey);
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //                    return;
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            MessageBox.Show("세금계산서가 " + TinfostateName + " 상태입니다.\r\n팝빌 홈페이지를 통해 확인 하시기 바랍니다. ", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //            return;
                        //        }



                        //    }
                        #endregion

                        #region 나이스


                       

                        string[] BillStatusList = null;
                        //var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
                        clientsTableAdapter.Fill(clientDataSet.Clients);
                        var _Clients = clientDataSet.Clients.FindByClientId(377);
                        var _changedStatusReq = "";
                        AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
                        var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();



                        _changedStatusReq = dTIServiceClass.changedStatusReqById("EDB", _Clients.frnNo, _Clients.userid, _Clients.passwd, Selected.EtaxWriteDate.AddDays(-1).ToString("yyyyMMdd"), Selected.EtaxWriteDate.AddMonths(1).ToString("yyyyMMdd"));
                        
                       


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
                                    if (statusMsgList[i].Contains(Selected.invoicerMgtKey))
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


                        if (_changestatusCode == "A")
                        {
                            if (MessageBox.Show("정말 삭제하시겠습니까?", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    NiceEtaxDelete(Selected.ClientPointId);
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
                                    NiceEtaxEdit(Selected.ClientPointId);
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
                                    NiceEtaxDelete(Selected.ClientPointId);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }


                            }

                        }


                        #endregion


                    }
                }

            }
        }
        private Dictionary<int, String> TaxInvoiceErrorDic = new Dictionary<int, string>();
        private void NiceEtaxDelete(int clientPointId)
        {
            // var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();
            LocalUser.Instance.LogInInformation.LoadClient();
            var _Clients = clientDataSet.Clients.FindByClientId(377);
            var Selected = pointsDataSet.ClientPointsList.FirstOrDefault(c => c.ClientPointId == clientPointId);

            try
            {




                #region 세금계산서 삭제 정발행

                var Result = dTIServiceClass.updateEtaxStatusToZ("EDB", _Clients.frnNo, _Clients.userid, _Clients.passwd, Selected.invoicerMgtKey);

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
                                       $"UPDATE ClientPoints SET EtaxWriteDate = @EtaxWriteDate, Issue = 0, IssueState = '취소'   WHERE    ClientPointId ={clientPointId}";
                                cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);

                                cmd.ExecuteNonQuery();





                                cn.Close();


                            }
                            MessageBox.Show("전자세금계산서가 삭제 되었습니다.");
                            //Success++;
                            btnSearch_Click(null, null);
                        }

                        catch (PopbillException ext)
                        {
                            MessageBox.Show(ext.code.ToString() + "\r\n" + ext.Message);
                            return;
                        }
                    }
                    else
                    {
                        //NiceEtaxEdit(salesid);
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
        private void NiceEtaxEdit(int clientPointId)
        {
            LocalUser.Instance.LogInInformation.LoadClient();
            var _Query = pointsDataSet.ClientPointsList.FirstOrDefault(c => c.ClientPointId == clientPointId);
            var _Clients = clientDataSet.Clients.FindByClientId(_Query.ClientId);
            var _Admins = clientDataSet.Clients.FindByClientId(377);
            var _Admininfo = baseDataSet.AdminInfoes.ToArray().FirstOrDefault();

            string passwd = "";
            string certPw = "";


            passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admins.passwd }).ToString();
            certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admins.NPKIpasswd }).ToString();




            string DutyNum = string.Empty;
            string DescriptionText = "";
            int i = 0;

            var _HideAccountNo = LocalUser.Instance.LogInInformation.Client.HideAccountNo;
            if (_HideAccountNo == 0)
            {
                DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner + "/" + _Clients.CMSAccountNo;
            }
            else
            {
                DescriptionText = _Clients.CMSBankName + "/" + _Clients.CMSOwner;

            }
         

            decimal _Price = 0;
            decimal _Vat = 0;
            decimal _Amont = 0;

            _Price = (_Query.Amount / 1.1m) * -1;
            _Vat = _Query.Amount - _Price - 1;

            string _TypeCode = "0201";


            if (Convert.ToInt64(_Vat) == 0)
            {
                _TypeCode = "0202";
            }
            else
            {
                _TypeCode = "0201";
            }
          
            string Tdtp_Date = _Query.EtaxWriteDate.ToString("yyyyMMdd");

            _Amont = _Price + _Vat;



            string dtiXml = "";
          
                dtiXml = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<TaxInvoice xmlns=\"urn: kr: or: kec: standard: Tax: ReusableAggregateBusinessInformationEntitySchemaModule: 1:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
            " xsi:schemaLocation=\"urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd\">\r\n" +
            "   <TaxInvoiceDocument>\r\n" +
            $"       <TypeCode>{_TypeCode}</TypeCode>		<!-- 세금계산서종류(코드값은 전자세금계산서_항목표.xls 파일 참조) -->\r\n" +
            $"      <DescriptionText>{DescriptionText}</DescriptionText> <!-- 비고 -->\r\n" +
            $"      <IssueDateTime>{Tdtp_Date}</IssueDateTime>	<!-- 작성일자(거래일자) -->\r\n" +
            $"      <AmendmentStatusCode>01</AmendmentStatusCode>	<!-- 수정사유(수정세금계산서의 경우 필수값.코드값은 전자세금계산서_항목표.xls 파일 참조)-->\r\n" +
            $"      <PurposeCode>01</PurposeCode>	<!-- 영수/청구구분(01 : 영수, 02 : 청구) -->\r\n" +
            $"      <OriginalIssueID></OriginalIssueID> <!-- 당초 전자(세금)계산서 승인번호 -->\r\n" +
            $"  </TaxInvoiceDocument>\r\n" +
            $"  <TaxInvoiceTradeSettlement>\r\n" +
            $"      <InvoicerParty>	<!-- 공급자정보 시작-->\r\n" +
            $"          <ID>{_Admins.BizNo.Replace("-", "")}</ID>	<!-- 사업자번호 -->\r\n" +
            $"          <TypeCode>{_Admins.Uptae}</TypeCode>		<!-- 업태 -->\r\n" +
            $"          <NameText>{_Admins.Name}</NameText>	<!-- 상호명 -->\r\n" +
            $"          <ClassificationCode>{_Admins.Upjong}</ClassificationCode>	<!-- 종목 -->\r\n" +
            $"          <SpecifiedOrganization>\r\n" +
            $"              <TaxRegistrationID></TaxRegistrationID>	<!-- 종사업자번호 -->\r\n" +
            $"          </SpecifiedOrganization>\r\n" +
            $"          <SpecifiedPerson>\r\n" +
            $"              <NameText>{_Admins.CEO}</NameText>	<!-- 대표자명 -->\r\n" +
            $"          </SpecifiedPerson>\r\n" +
            $"          <DefinedContact>\r\n" +
            $"              <URICommunication>{_Admins.Email}</URICommunication> <!-- 담당자 이메일 -->\r\n" +
            $"          </DefinedContact>\r\n" +
            $"          <SpecifiedAddress>\r\n" +
            $"              <LineOneText>{_Admins.AddressState + " " + _Admins.AddressCity + " " + _Admins.AddressDetail}</LineOneText> <!-- 사업장주소 -->\r\n" +
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
                  $"      <NameText>주선사 충전금</NameText>  <!-- 품목명 -->\r\n" +
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










            string Result = "";
            
                passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { _Admininfo.passwd }).ToString();
                certPw = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { "dpebqlf54906**" }).ToString();

                Result = dTIServiceClass.makeAndPublishSignDealy(linkcd, _Admininfo.frnNo, _Admininfo.userid, _Admininfo.passwd, certPw, dtiXml, "Y", "N", "", "3");
            
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
                            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                            {
                                cn.Open();
                                SqlCommand cmd = cn.CreateCommand();

                                cmd.CommandText =
                                   $"UPDATE ClientPoints SET EtaxWriteDate = @EtaxWriteDate, Issue = 0, IssueState = '취소'   WHERE    ClientPointId ={clientPointId}";
                                cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                                cmd.ExecuteNonQuery();



                                cn.Close();

                            }

                            MessageBox.Show("발행 취소 되었습니다.");

                            btnSearch_Click(null, null);
                        }
                        catch (PopbillException ex)
                        {
                            MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                            Error++;
                            btnSearch_Click(null, null);
                        }
                    }
                    catch (PopbillException ex)
                    {
                        TaxInvoiceErrorDic.Add(_Query.ClientPointId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                        //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                        Error++;
                    }
                }
                else
                {
                    TaxInvoiceErrorDic.Add(_Query.ClientPointId, "[ " + retVal + " ] " + errMsg);
                    //MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                    Error++;
                }

            }
            catch
            {

            }

        }
        private void SendCancelIssue(int clientPointId)
        {
            try
            {
                var _Sales = pointsDataSet.ClientPointsList.FirstOrDefault(c => c.ClientPointId == clientPointId);

                Response response = taxinvoiceService.CancelIssue("1148654906", MgtKeyType.SELL, _Sales.invoicerMgtKey, "", "edubillsys");

                SendDelete(clientPointId);
            }
            catch (PopbillException ex)
            {
                //TaxInvoiceErrorDic.Add(tradeId, "[ " + ex.code.ToString() + " ] " + ex.Message);
                ////MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);

                return;

            }
        }

        private void SendDelete(int clientPointId)
        {

            try
            {
                clientsTableAdapter.Fill(clientDataSet.Clients);
                var Query = clientDataSet.Clients.Where(c => c.ClientId == 377);


                var _Sales = pointsDataSet.ClientPointsList.FirstOrDefault(c => c.ClientPointId == clientPointId);
                Response response = taxinvoiceService.Delete("1148654906", MgtKeyType.SELL, _Sales.invoicerMgtKey, "edubillsys");


                try
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText =
                               $"UPDATE ClientPoints SET EtaxWriteDate = @EtaxWriteDate, Issue = 0, IssueState = '취소'   WHERE    ClientPointId ={clientPointId}";
                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);

                        cmd.ExecuteNonQuery();


                     


                        cn.Close();


                    }
                    MessageBox.Show("전자세금계산서가 삭제 되었습니다.");
                    //Success++;
                    btnSearch_Click(null, null);
                }

                catch (PopbillException ext)
                {
                    MessageBox.Show(ext.code.ToString() + "\r\n" + ext.Message);
                    return;
                }



            }
            catch (PopbillException ex)
            {

                MessageBox.Show(ex.code.ToString() + "\r\n" + ex.Message);
                return;
            }




        }
        private void MinusInvoice(string Tdtp_Date, string MgtKEy)
        {
            Success = 0;
            Error = 0;


            var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as PointsDataSet.ClientPointsListRow;
            clientsTableAdapter.Fill(clientDataSet.Clients);
            var Query = clientDataSet.Clients.Where(c => c.ClientId == 377);
            var Query2 = clientDataSet.Clients.Where(c => c.ClientId == Selected.ClientId).FirstOrDefault();
            //발행취소

            bool forceIssue = false;        // 지연발행 강제여부
            String memo = "";  // 즉시발행 메모 



            // 세금계산서 정보 객체 
            Taxinvoice taxinvoice = new Taxinvoice();

            //   Response response2 = taxinvoiceService.GetInfo(Query.First().BizNo.Replace("-", ""),MgtKeyType.SELL, MgtKEy);



            taxinvoice.writeDate = TinfoWriteDate;//Tdtp_Date;                      //필수, 기재상 작성일자
            taxinvoice.chargeDirection = "정과금";                  //필수, {정과금, 역과금}
            taxinvoice.issueType = "정발행";                        //필수, {정발행, 역발행, 위수탁}
            taxinvoice.purposeType = "영수";                        //필수, {영수, 청구}
            taxinvoice.issueTiming = "직접발행";                    //필수, {직접발행, 승인시자동발행}
            taxinvoice.taxType = "과세";                            //필수, {과세, 영세, 면세}

            taxinvoice.invoicerCorpNum = Query.First().BizNo.Replace("-", "");              //공급자 사업자번호
            taxinvoice.invoicerTaxRegID = "";                       //종사업자 식별번호. 필요시 기재. 형식은 숫자 4자리.
            taxinvoice.invoicerCorpName = Query.First().CEO;
            taxinvoice.invoicerMgtKey = "C" + Selected.ClientPointId.ToString() + "-" + DateTime.Now.ToString("yyMMddhhmmss");           //정발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoicerCEOName = Query.First().CEO;
            taxinvoice.invoicerAddr = Query.First().AddressState + " " + Query.First().AddressCity + " " + Query.First().AddressDetail;
            taxinvoice.invoicerBizClass = Query.First().Upjong;
            taxinvoice.invoicerBizType = Query.First().Uptae;
            taxinvoice.invoicerContactName = Query.First().CEO;
            taxinvoice.invoicerEmail = Query.First().Email;
            taxinvoice.invoicerTEL = Query.First().PhoneNo;
            taxinvoice.invoicerHP = Query.First().MobileNo;
            taxinvoice.invoicerSMSSendYN = false;                    //정발행시(공급자->공급받는자) 문자발송기능 사용시 활용

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            taxinvoice.invoiceeType = "사업자";                     //공급받는자 구분, {사업자, 개인, 외국인}
            taxinvoice.invoiceeCorpNum = Selected.BizNo.Replace("-", "");              //공급받는자 사업자번호
            taxinvoice.invoiceeCorpName = Selected.Name;
            taxinvoice.invoiceeMgtKey = "";                         //역발행시 필수, 문서관리번호 1~24자리까지 공급자사업자번호별 중복없는 고유번호 할당
            taxinvoice.invoiceeCEOName = Selected.CEO;
            taxinvoice.invoiceeAddr = Query2.AddressState + " " + Query2.AddressCity + " " + Query2.AddressDetail;
            taxinvoice.invoiceeBizClass = Query2.Upjong;
            taxinvoice.invoiceeBizType = Query2.Uptae;
            taxinvoice.invoiceeTEL1 = Query2.MobileNo;
            taxinvoice.invoiceeContactName1 = Query2.CEO;
            taxinvoice.invoiceeEmail1 = Query2.Email;
            taxinvoice.invoiceeHP1 = Query2.MobileNo;
            taxinvoice.invoiceeSMSSendYN = false;                   //역발행시(공급받는자->공급자) 문자발송기능 사용시 활용


            var _Amount = Selected.Amount;
            var _Price = (_Amount / 1.1m);
            var _VAT = _Amount - _Price;


            Int64 MinusPrice = Convert.ToInt64(_Price) * -1;
            Int64 MinusVat = Convert.ToInt64(_VAT) * -1;
            Int64 MinusAmount = Convert.ToInt64(Selected.Amount) * -1;


            taxinvoice.supplyCostTotal = MinusPrice.ToString();                //필수 공급가액 합계"
            taxinvoice.taxTotal = MinusVat.ToString();                      //필수 세액 합계
            taxinvoice.totalAmount = MinusAmount.ToString();                 //필수 합계금액.  공급가액 + 세액

            //수정세금계산서 작성시 1~6까지 선택기재.
            // 1 - 기재사항 착오정정, 2 - 공급가액변동
            // 3 - 환입, 4 - 계약의 해지 
            // 5 - 내국신용장 사후개설
            // 6 - 착오에 의한 이중발행
            taxinvoice.modifyCode = 4;                           //수정세금계산서 작성시 1~6까지 선택기재.

            //수정세금계산서 작성시 원본 세금계산서의 ItemKey기재. 
            taxinvoice.originalTaxinvoiceKey = TinfoitemKey;                  //수정세금계산서 작성시 원본세금계산서의 ItemKey기재. ItemKey는 문서확인.
            taxinvoice.serialNum = "1";
            taxinvoice.cash = "";                                   //현금
            taxinvoice.chkBill = "";                                //수표
            taxinvoice.note = "";                                   //어음
            taxinvoice.credit = "";                                 //외상미수금
            taxinvoice.remark1 = "";
            taxinvoice.remark2 = "";
            taxinvoice.remark3 = "";
            taxinvoice.kwon = 1;
            taxinvoice.ho = 1;

            taxinvoice.businessLicenseYN = false;                   //사업자등록증 이미지 첨부시 설정.
            taxinvoice.bankBookYN = false;                          //통장사본 이미지 첨부시 설정.

            taxinvoice.detailList = new List<TaxinvoiceDetail>();

            TaxinvoiceDetail detail = new TaxinvoiceDetail();

            detail.serialNum = 1;                                   //일련번호
            detail.purchaseDT = Selected.EtaxWriteDate.ToString("yyyyMMdd");                         //거래일자
            detail.itemName = 
            detail.spec = "";
            detail.qty = "1";                                       //수량
            detail.unitCost = MinusPrice.ToString();                            //단가
            detail.supplyCost = MinusPrice.ToString();                           //공급가액
            detail.tax = MinusVat.ToString();                                 //세액
            detail.remark = "";

            taxinvoice.detailList.Add(detail);

            detail = new TaxinvoiceDetail();

            try
            {
                // Response response = taxinvoiceService.RegistIssue(Query.First().BizNo, taxinvoice, forceIssue, memo);
                Response response = taxinvoiceService.RegistIssue(Query.First().BizNo.Replace("-", ""), taxinvoice, forceIssue, memo);

                try
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        cmd.CommandText =
                           $"UPDATE ClientPoints SET EtaxWriteDate = @EtaxWriteDate, Issue = 0, IssueState = '취소'   WHERE    ClientPointId ={Selected.ClientPointId}";
                        cmd.Parameters.AddWithValue("@EtaxWriteDate", DateTime.Now);
                        cmd.ExecuteNonQuery();

                    

                        cn.Close();

                    }

                    //using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    //{
                    //    cn.Open();
                    //    SqlCommand INcmd = cn.CreateCommand();

                    //    INcmd.CommandText =
                    //       "INSERT INTO  SalesManage(RequestDate, SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat, UseTax, Price, Amount, CreateDate, IssueDate, ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, IssueState, ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey)" +
                    //       " SELECT getdate(), SangHo, BizNo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, Email, ContRactName, MobileNo, Item, UnitPrice, Num, Vat*-1, UseTax, Price*-1, Amount*-1, getdate(), getdate(), ClientId, Issue, HasACC, LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, LGD_OID, SetYN, PayState, PayDate, PayBankName, PayBankCode, PayAccountNo, PayInputName, CardPayGubun, CustomerId, ClientAccId, '취소', ZipCode, BeginDate, EndDate, SourceType, SubClientId, InputPrice1, InputPrice1Date, InputPrice2, InputPrice2Date, InputPrice3, InputPrice3Date, invoicerMgtKey FROM SalesManage WHERE SalesId = @SalesId";
                    //    // INcmd.Parameters.AddWithValue("@invoicerMgtKey", taxinvoice.invoicerMgtKey);
                    //    INcmd.Parameters.AddWithValue("@SalesId", Selected.SalesId);
                    //    INcmd.ExecuteNonQuery();
                    //    cn.Close();

                    //}

                   

                    MessageBox.Show("발행 취소 되었습니다.");

                    btnSearch_Click(null, null);
                }
                catch (PopbillException ex)
                {
                    MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                    Error++;
                    btnSearch_Click(null, null);
                }
                //Success++;
            }
            catch (PopbillException ex)
            {
                MessageBox.Show("[ " + ex.code.ToString() + " ] " + ex.Message, "즉시발행");
                Error++;
            }


        }
    }
}
