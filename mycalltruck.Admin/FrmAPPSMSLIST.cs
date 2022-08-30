using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using OfficeOpenXml;
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
    public partial class FrmAPPSMSLIST : Form
    {
        public FrmAPPSMSLIST()
        {
            InitClientTable();
            InitDriverTable();
            InitAppSMSTable();
            InitAppSMSMessageTable();

            InitializeComponent();
        }


        private void FRMMNUSELIST_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'appSMSDataSet1.AppSMSUseList' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.appSMSUseListTableAdapter.Fill(this.appSMSDataSet.AppSMSUseList);

            this.tablesTableAdapter.Fill(baseDataSet.TABLES);

            Fclear();

            btn_Search_Click(null, null);

        }
        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
        }
        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();

        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name, LoginId FROM Clients ";
                   
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientTable.Add(
                              new ClientViewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

      
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
        }
        class AppSMSViewModel
        {
            public int Idx { get; set; }
            public string MobileNo { get; set; }
            public string Name { get; set; }
            public int ClientId { get; set; }
            public DateTime CreateDate { get; set; }
            public string SMSYN { get; set; }
            public string SMSSendDate { get; set; }
        }

        class APPSMSMessageViewModel
        {
            public int Idx { get; set; }

            public int ClientId { get; set; }

            public string Message { get; set; }
        }


    
        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();
        private List<AppSMSViewModel> _AppSMSTable = new List<AppSMSViewModel>();
        private List<APPSMSMessageViewModel> _APPSMSMessageTable = new List<APPSMSMessageViewModel>();

     
        private void InitDriverTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT DriverId, Code, Name, LoginId,BizNo,candidateid, Isnull(Appuse,0) as AppUse,MobileNo FROM Drivers ";

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
                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private void InitAppSMSTable()
        {
            _AppSMSTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT idx, MobileNo,ISNULL(Name,'') AS Name ,ISNULL(ClientId,0) AS ClientId, CreateDate, SMSYN, ISNULL(SMSSendDate,'') SMSSendDate FROM AppSMS ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _AppSMSTable.Add(
                              new AppSMSViewModel
                              {
                                  Idx = dataReader.GetInt32(0),
                                  MobileNo = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  ClientId = dataReader.GetInt32(3),
                                  CreateDate = dataReader.GetDateTime(4),
                                  SMSYN = dataReader.GetString(5),
                                  SMSSendDate = dataReader.GetString(6),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }

        private void InitAppSMSMessageTable()
        {
            _APPSMSMessageTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT idx, ClientId, Message FROM APPSMSMessage ";

                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _APPSMSMessageTable.Add(
                              new APPSMSMessageViewModel
                              {
                                  Idx = dataReader.GetInt32(0),

                                  ClientId = dataReader.GetInt32(1),
                                  Message = dataReader.GetString(2),

                              });
                        }
                    }
                }
                connection.Close();
            }
        }



        private void Fclear()
        {
            dtpStart.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtpEnd.Value = DateTime.Now;

            cmb_SmsState.SelectedIndex = 0;
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
         
            

        }
        private void btn_Import_Click(object sender, EventArgs e)
        {
            EXCELINSERT_USELIST _Form = new EXCELINSERT_USELIST();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
            btn_Search_Click(null, null);
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {

            DataLoad();
        }
        private string GetSelectCommand()
        {
            int sMonth = Convert.ToInt32(dtpStart.Text.Substring(5, 2));
            int lMonth = Convert.ToInt32(dtpEnd.Text.Substring(5, 2));

            int mMonth = lMonth - sMonth;
           appSMSDataSet.AppSMSUseList.Clear();
            String SelectCommandText =
                    @" SELECT  a.date_client_req ,d.Code as ClientCode ,d.Name as ClientName ,e.LoginId ,e.CarYear as DriverName, a.recipient_num as MobileNo " +
                    ", e.CarNo , CASE WHEN mt_report_code_ib = '1000' THEN '성공' ELSE '' END SmsResult, CASE WHEN  mt_report_code_ib = '1000' THEN ''ELSE  c.rslt_pname END AS SmsError " +
                    ", a.mt_refkey  ,'LMS' smsgubun " +
                    " FROM     em_mmt_tran AS a " +
                    " JOIN  em_resultcode AS c ON a.mt_report_code_ib = c.rslt_code " +
                    " join Clients AS D ON a.mt_refkey = d.ClientId " +
                    " left join Drivers as e ON replacE(a.recipient_num,'-','') = e.MobileNo  collate Korean_Wansung_CI_AS";

            // " WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
            for (int i = sMonth; i <= lMonth; i++)
            {
                var Query = baseDataSet.TABLES.Where(c => c.TABLE_NAME == "em_mmt_log_2018" + String.Format("{0:00}", i)).Count();

                if (Query > 0)
                {
                    SelectCommandText += " union all" +
                    "  SELECT a.date_client_req ,d.Code as ClientCode ,d.Name as ClientName ,e.LoginId ,e.CarYear as DriverName, a.recipient_num as MobileNo " +
                    ", e.CarNo , CASE WHEN mt_report_code_ib = '1000' THEN '성공' ELSE '' END SmsResult, CASE WHEN  mt_report_code_ib = '1000' THEN ''ELSE  c.rslt_pname END AS SmsError " +
                    ", a.mt_refkey  ,'LMS' smsgubun " +
                    " FROM em_mmt_log_2018" + String.Format("{0:00}", i) + " AS a " +
                    " JOIN em_resultcode AS c ON a.mt_report_code_ib = c.rslt_code " +
                    " join Clients AS D ON a.mt_refkey = d.ClientId " +
                    " left join Drivers as e ON replacE(a.recipient_num,'-','') = e.MobileNo  collate Korean_Wansung_CI_AS";
                    //" WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
                }


            }
            SelectCommandText += " union all" +
                    @"  SELECT a.date_client_req ,d.Code as ClientCode ,d.Name as ClientName ,e.LoginId ,e.CarYear as DriverName, a.recipient_num as MobileNo " +
                    ", e.CarNo , CASE WHEN mt_report_code_ib = '1000' THEN '성공' ELSE '' END SmsResult, CASE WHEN  mt_report_code_ib = '1000' THEN ''ELSE  c.rslt_pname END AS SmsError " +
                    ", a.mt_refkey   ,'SMS' smsgubun" +
                    " FROM em_smt_tran AS a " +
                    " JOIN em_resultcode AS c ON a.mt_report_code_ib = c.rslt_code " +
                    " join Clients AS D ON a.mt_refkey = d.ClientId " +
                    " left join Drivers as e ON replacE(a.recipient_num,'-','') = e.MobileNo  collate Korean_Wansung_CI_AS";
            // " join em_resultcode c on a.mt_report_code_ib = c.rslt_code  WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
            for (int i = sMonth; i <= lMonth; i++)
            {
                var Query = baseDataSet.TABLES.Where(c => c.TABLE_NAME == "em_smt_log_2018" + String.Format("{0:00}", i)).Count();

                if (Query > 0)
                {
                    SelectCommandText += " union  all" +
                "  SELECT a.date_client_req ,d.Code as ClientCode ,d.Name as ClientName ,e.LoginId ,e.CarYear as DriverName, a.recipient_num as MobileNo " +
                ", e.CarNo , CASE WHEN mt_report_code_ib = '1000' THEN '성공' ELSE '' END SmsResult, CASE WHEN  mt_report_code_ib = '1000' THEN ''ELSE  c.rslt_pname END AS SmsError " +
                ", a.mt_refkey  ,'SMS' smsgubun " +
                "  FROM em_smt_log_2018" + String.Format("{0:00}", i) + " AS a " +
                " JOIN em_resultcode AS c ON a.mt_report_code_ib = c.rslt_code " +
                " join Clients AS D ON a.mt_refkey = d.ClientId " +
                " left join Drivers as e ON replacE(a.recipient_num,'-','') = e.MobileNo  collate Korean_Wansung_CI_AS";
                    //"WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";
                }


            }

            SelectCommandText += " UNION  all" +
                 " SELECT A.CreateDate,a.Code,a.ClientName,ISNULL(a.LoginId, '') as LoginId,ISNULL(a.DriverName, '') as DriverName, a.MobileNo,ISNULL(a.CarNo, '') as CarNo" +
                 " ,'' as SmsResult,a.Remark collate Korean_Wansung_CI_AS as SmsError,a.mt_refkey,'' as smsgubun " +
                 " FROM " +
                 " (SELECT idx, a.CreateDate, b.Code, b.Name as ClientName, c.LoginId, c.CarYear as DriverName, a.MobileNo " +
                 " collate Korean_Wansung_CI_AS MobileNo, c.CarNo, isnull(a.Name, '') Name, Remark, b.ClientId as mt_refkey, '' smsgubun " +
                 " FROM AppSMS as a JOIN Clients as b " +
                 " ON a.ClientId = b.ClientId " +
                 " left JOIN Drivers as c " +
                 " ON a.MobileNo = replace(c.MobileNo, '-', '') AND a.DriverId = c.DriverId " +
                  " WHERE isnull(Remark, '') ! = '' " +
                  " ) as a";

            //  " WHERE isnull(Remark,'') ! = '' ) as a  WHERE a.mt_refkey  = " + LocalUser.Instance.LogInInformation.ClientId + "";

            return SelectCommandText;
        }


        private void DataLoad()
        {
            appSMSDataSet.AppSMSSearch.Clear();
            //clientDataSet.SubClients.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

                    // WhereStringList.Add("mt_refkey = @ClientId");
                    //  _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    // 1. 전송결과 
                    if (cmb_SmsState.SelectedIndex > 0)
                    {
                        switch (cmb_SmsState.Text)
                        {
                            case "성공":
                                WhereStringList.Add(" SmsResult = '성공' ");
                                break;
                            case "실패(어플설치)":
                                WhereStringList.Add(" SmsResult != '성공' ");
                                WhereStringList.Add(" SmsError = '어플설치'");
                                break;
                          
                            case "실패(당일중복전송)":
                                WhereStringList.Add(" SmsResult != '성공' ");
                                WhereStringList.Add(" SmsError = '당일발송'");
                                break;
                            case "실패(전체)":
                                WhereStringList.Add(" SmsResult != '성공' ");
                                //WhereStringList.Add(" SmsError != '어플설치'");
                                //WhereStringList.Add(" SmsError != '당일발송'");
                                break;
                            default:
                                break;
                        }

                    }
                    // 2. 단어
                    if (cmb_Search.SelectedIndex > 0)
                    {
                        switch (cmb_Search.Text)
                        {
                            case "운송사코드":
                                WhereStringList.Add(string.Format("ClientCode Like  '%{0}%'", txt_Search.Text));
                                break;
                            case "운송사명":
                                WhereStringList.Add(string.Format("ClientName Like  '%{0}%'", txt_Search.Text));
                                break;

                            case "차주아이디":
                                WhereStringList.Add(string.Format("LoginId Like  '%{0}%'", txt_Search.Text));
                                break;
                            case "차주명":
                                WhereStringList.Add(string.Format("DriverName Like  '%{0}%'", txt_Search.Text));
                                break;
                            case "핸드폰번호":
                                WhereStringList.Add(string.Format("REPLACE(MobileNo,'-','') Like  '%{0}%'", txt_Search.Text));
                                break;
                            case "차량번호":
                                WhereStringList.Add(string.Format("CarNo Like  '%{0}%'", txt_Search.Text));
                                break;
                            default:
                                break;
                        }

                    }

                    WhereStringList.Add($" date_client_req >= @Begin AND date_client_req < @End");
                    _Command.Parameters.AddWithValue("@Begin", dtpStart.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtpEnd.Value.Date.AddDays(1));
                    


                    SelectCommandText = "SELECT  date_client_req,ClientCode,ClientName,LoginId,DriverName,MobileNo, CarNo ,SmsResult ,SmsError,mt_refkey,smsgubun FROM (" + SelectCommandText + ") as C";
                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText + " order by date_client_req desc";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        appSMSDataSet.AppSMSUseList.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }
        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
          
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as AppSMSDataSet.AppSMSUseListRow;
            if (Selected == null)
                return;
            if (e.ColumnIndex == ColumnNo1.Index)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0").Replace(",","");
            }
            else if(e.ColumnIndex == dateclientreqDataGridViewTextBoxColumn.Index)
            {
                //var Dattereq = Selected.date_client_req.ToString();
                e.Value = Selected.date_client_req.ToString("yyyy-MM-dd HH:mm:ss");
            }
             else if(e.ColumnIndex == smsResultDataGridViewTextBoxColumn.Index)
            {
                //var Dattereq = Selected.date_client_req.ToString();
                if(Selected.SmsResult == "성공")
                {
                    e.CellStyle.ForeColor = Color.Blue;
                }
                else
                {

                    e.Value = "실패";
                    e.CellStyle.ForeColor = Color.Red;
                }
               
            }
            else if (e.ColumnIndex == smsErrorDataGridViewTextBoxColumn.Index)
            {
                //var Dattereq = Selected.date_client_req.ToString();
                if (Selected.SmsError != "")
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
                

            }




        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            Fclear();
            btn_Search_Click(null, null);
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            ExcelExportBasic();
        }

        private void ExcelExportBasic()
        {
            if (newDGV1.RowCount == 0)
            {
                MessageBox.Show("엑셀로 내보낼 항목이 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.TAX);

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = "APP설치 전송문자 사용내역_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.고객별사용내역))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                var _Model = ((DataRowView)newDGV1.Rows[i].DataBoundItem).Row as UseListDataSet.AccountListRow;

                if (_Model.PayDate == "")
                { _Sheet.Cells[RowIndex, 1].Value = "합계"; }
                else
                {
                    //_Sheet.Cells[RowIndex, 1].Value = (i + 1).ToString();
                    _Sheet.Cells[RowIndex, 1].Value = (newDGV1.Rows.Count - i - 1);
                }
                _Sheet.Cells[RowIndex, 2].Value = _Model.PayDate;
                _Sheet.Cells[RowIndex, 3].Value = _Model.ClientCode;
                _Sheet.Cells[RowIndex, 4].Value = _Model.ClientName;
                _Sheet.Cells[RowIndex, 5].Value = _Model.LGD_MID;
                _Sheet.Cells[RowIndex, 6].Value = _Model.DriverName;
                _Sheet.Cells[RowIndex, 7].Value = _Model.PayState;
                _Sheet.Cells[RowIndex, 8].Value = _Model.Amount;
                _Sheet.Cells[RowIndex, 9].Value = _Model.VAT;
                _Sheet.Cells[RowIndex, 10].Value = _Model.PayAmount;
                
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

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }
    }
}
