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
    public partial class FrmEDocumentList : Form
    {
        public FrmEDocumentList()
        {
            InitClientTable();
            InitDriverTable();
            

            InitializeComponent();
        }


        private void FRMMNUSELIST_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'eDocumnetDataSet.DocuTable' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.docuTableTableAdapter.Fill(this.eDocumnetDataSet.DocuTable);
            // TODO: 이 코드는 데이터를 'eDocumnet.DocuTable' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
          

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
      


    
        private List<DriverViewModel> _DriverTable = new List<DriverViewModel>();
     
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

     


        private void Fclear()
        {
            dtpStart.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtpEnd.Value = DateTime.Now;

         
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
          
            String SelectCommandText =
                    @"SELECT  TradeId, RequestDateTime, FileDirectory, Image1Name, 
               CASE WHEN SignLocation = 'LU' THEN '좌측상단' WHEN SignLocation = 'LL' THEN '좌측하단' WHEN SignLocation = 'RU' THEN '우측상단' WHEN SignLocation = 'RL' THEN '우측하단' ELSE
                '좌측상단' END AS SignLocationName, SignLocation, PdfFileName, AipId, DocId, 
               CASE WHEN Status = '_10_ready' THEN 'PDF생성 대기' WHEN Status = '_10_prog' THEN 'PDF생성 진행중' WHEN Status = '_10_error' THEN 'PDF생성 오류' WHEN Status = '_20_ready' THEN
                'TimeStamp 대기' WHEN Status = '_20_prog' THEN 'TimeStamp 진행중' WHEN Status = '_20_error' THEN 'TimeStamp 오류' WHEN Status = '_30_ready' THEN '공전소전송 대기' WHEN Status
                = '_30_prog' THEN '공전소전송 중' WHEN Status = '_30_error' THEN '공전소전송 오류' WHEN Status = '_40_ready' THEN '공전소삭제 대기' WHEN Status = '_40_prog' THEN '공전소삭제 중'
                WHEN Status = '_40_error' THEN '공전소삭제 오류' ELSE '' END AS StatusName, Status, ErrMsg ,CONVERT(varchar,UpdateDateTime,20) UpdateDateTime
                FROM     DocuTable";

           

            return SelectCommandText;
        }


        private void DataLoad()
        {
            eDocumnetDataSet.DocuTable.Clear();
            
            
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

                   
                    //// 2. 단어
                    //if (cmb_Search.SelectedIndex > 0)
                    //{
                    //    switch (cmb_Search.Text)
                    //    {
                          
                    //        case "파일명":
                    //            WhereStringList.Add(string.Format("PDfFileName Like  '%{0}%'", txt_Search.Text));
                    //            break;

                    //        case "상태":
                    //            WhereStringList.Add(string.Format("Status Like  '%{0}%'", txt_Search.Text));
                    //            break;
                        
                    //        default:
                    //            break;
                    //    }

                    //}

                    WhereStringList.Add($" RequestDateTime >= @Begin AND RequestDateTime < @End");
                    _Command.Parameters.AddWithValue("@Begin", dtpStart.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtpEnd.Value.Date.AddDays(1));
                    


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText + " order by RequestDateTime desc";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        eDocumnetDataSet.DocuTable.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }
        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
          
            if (e.RowIndex < 0)
                return;
            
            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as EDocumnetDataSet.DocuTableRow;
            if (Selected == null)
                return;
            if (e.ColumnIndex == ColumnNo1.Index)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0").Replace(",","");
            }
            else if (e.ColumnIndex == requestDateTimeDataGridViewTextBoxColumn.Index)
            {
                //var Dattereq = Selected.date_client_req.ToString();
                e.Value = Selected.RequestDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            //else if (e.ColumnIndex == updateDateTimeDataGridViewTextBoxColumn.Index)
            //{
            //    //var Dattereq = Selected.date_client_req.ToString();
            //    e.Value = Selected.UpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            // else if(e.ColumnIndex == smsResultDataGridViewTextBoxColumn.Index)
            //{
            //    //var Dattereq = Selected.date_client_req.ToString();
            //    if(Selected.SmsResult == "성공")
            //    {
            //        e.CellStyle.ForeColor = Color.Blue;
            //    }
            //    else
            //    {

            //        e.Value = "실패";
            //        e.CellStyle.ForeColor = Color.Red;
            //    }

            //}
            //else if (e.ColumnIndex == smsErrorDataGridViewTextBoxColumn.Index)
            //{
            //    //var Dattereq = Selected.date_client_req.ToString();
            //    if (Selected.SmsError != "")
            //    {
            //        e.CellStyle.ForeColor = Color.Red;
            //    }


            //}




        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            Fclear();
            btn_Search_Click(null, null);
        }

     

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }
    }
}
