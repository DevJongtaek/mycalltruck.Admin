using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_DRIVER2 : Form
    {
        string FileName = string.Empty;
        DateTime O_StopDate,O_RequestDate;
        Int64 O_Price = 0;
        Int64 O_Deposit = 0;
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        public EXCELINSERT_DRIVER2()
        {
            InitializeComponent();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
          
            this.staticOptionsTableAdapter.Fill(this.cMDataSet.StaticOptions);
            this.driversTableAdapter.Fill(this.cMDataSet.Drivers);
            //this.drivers_Car2TableAdapter.Fill(this.cMDataSet.Drivers_Car2);
            this.clientsTableAdapter.Fill(this.cMDataSet.Clients);
            //this.clientUsersTableAdapter.Fill(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInfomation.ClientId);
            //this.driversTableAdapter.Fill(this.cMDataSet.Drivers);
            //this.trades1TableAdapter.Fill(this.cMDataSet.Trades1);
          
        }

        private void ExcelTest()
        {

            newDGV1.DataSource = "";
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.KICC);

            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.KICC;
            d.Filter = "Excel2003-2007 (*.xls)|*.xls|Excel통합문서 (*.xlsx)|*.xlsx";
            d.FilterIndex = 1;

          
            

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                FileName = d.FileName;


                // OLEDB를 이용한 엑셀 연결
                string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES;ImportMixedTypes=Text;IMEX=1'";
                OleDbConnection conn = new OleDbConnection(szConn);
                try
                {
                    conn.Open();

                    // 엑셀로부터 데이타 읽기
                    OleDbCommand cmd = new OleDbCommand("SELECT MID,차주코드,운송사코드,운송사명 FROM [Sheet1$A1:R] WHERE 차주코드 <> '' ", conn);
                    OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                    DataSet ds = new DataSet();
                    adpt.Fill(ds);

                    DataTable dTable = new DataTable();
                    adpt.Fill(dTable);


                    newDGV1.DataSource = dTable;


                    conn.Close();

                    
                    //ExcelOpen excel = new ExcelOpen(d.FileName);
                    //DataTable table = excel.Open();
                    //Dictionary<string, int> dic = new Dictionary<string, int>();
                    //dic.Add("사업자등록번호", -1);
                    //dic.Add("기사명", -1);
                    //dic.Add("차량번호", -1);
                    //dic.Add("청구항목", -1);
                    //dic.Add("청구일", -1);
                    //dic.Add("청구금액(VAT별도)", -1);
                    //for (int i = 0; i < table.Columns.Count; i++)
                    //{
                    //    string key = table.Columns[i].ColumnName;
                    //    if (dic.Keys.Contains(key))
                    //        dic[key] = i;
                    //}
                    //if (dic.Values.Count(c => c == -1) > 0)
                    //{
                    //    MessageBox.Show("엑셀 첫행은 각 열의 이름을 쓰여져야 하며, 열 중에 '사업자등록번호', '기사명', '청구항목','청구일','청구금액' 의 열들이 모두 포함되어 있어야 합니다."
                    //        , Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //    return;
                    //}
                    //else
                    //{
                    //    newDGV1.DataSource = table;
                    //}

                    label4.Text = "0";
                    label5.Text = "0";
                    label6.Text = "0";
                }
                catch(Exception e)
                {
                    conn.Close();
                    MessageBox.Show(e.ToString());
                }

            }
        }

        private void btn_Info_Click(object sender, EventArgs e)
        {
            ExcelTest();

            
        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            //bar.Value = 0;
            List<string> VisibleOrderIds = new List<string>();
            if (newDGV1.Rows.Count == 0)
            {
                MessageBox.Show("검증할 데이터가 없습니다.");
                return;

            }
            

            string szConnf = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
            OleDbConnection connf = new OleDbConnection(szConnf);
            connf.Open();

            // 엑셀로부터 데이타 읽기
            OleDbCommand cmdf = new OleDbCommand("SELECT MID,차주코드,운송사코드,운송사명 FROM [Sheet1$A1:R] ", connf);
            OleDbDataAdapter adptf = new OleDbDataAdapter(cmdf);


            DataSet dsf = new DataSet();
            adptf.Fill(dsf);

           
            // 엑셀 데이타 갱신
            //cmdf = new OleDbCommand("UPDATE [Sheet1$A1:Z] SET error='' ", connf);



            //cmdf.ExecuteNonQuery();
            connf.Close();

            List<DataGridViewRow> dataRows = new List<DataGridViewRow>();

            var ITEM_Dirvers = new List<CMDataSet.DriversRow>();


            int errCount = 0;
            int RowErrorCount = 0;
            int FCount = 0;
        
            foreach(DataGridViewRow row in newDGV1.Rows)
            {


                FCount++;

                CMDataSet.DriversRow Row = cMDataSet.Drivers.NewDriversRow();
                RowErrorCount = 0;
                dataRows.Add(row);

         
               


                string SMID = row.Cells[0].Value.ToString().Trim();
                string SDriverCode= row.Cells[1].Value.ToString().Trim();
                string SClientCode = row.Cells[2].Value.ToString().Trim();
                string SClientName = row.Cells[3].Value.ToString().Trim();


                //MID 없으면 에러
                if (String.IsNullOrEmpty(SMID))
                {



                    RowErrorCount++;


                }
              
               //차주코드 없으면 에러
                else if (String.IsNullOrEmpty(SDriverCode))
                {

                    RowErrorCount++;
                }
                //운송사코드 없으면 에러
                else if (String.IsNullOrEmpty(SClientCode))
                {
                    
                    RowErrorCount++;

                }
               
              
                //있으면  찾기
                else
                {
                     
                    //FCount++;
                
                    Row.MID = SMID;
                    Row.Code = SDriverCode;

                    #region 구분

                    if (SClientCode != "")
                    {

                        var Query = cMDataSet.Clients.Where(c => c.Code == SClientCode).ToArray();

                        if (Query.Any())
                        {

                            Row.CandidateId = Query.First().ClientId;
                        }

                        else
                        {
                          

                            RowErrorCount++;


                            //  return;
                        }


                    }

                    #endregion

                //    cMDataSet.Drivers.AddDriversRow(Row);
                    ITEM_Dirvers.Add(Row);
                }


                if (RowErrorCount > 0)
                {
                    errCount++;
                }
                
            }


            try
            {
                if (errCount == 0)
                {
                   // driversTableAdapter.Update(cMDataSet.Drivers);

                    foreach (var Type_B_lists in ITEM_Dirvers)
                    {

                      //  TempCMDataSet.TblItem.AddTblItemRow(Type_B_lists);

                        driversTableAdapter.UpdateQuery(Type_B_lists.MID, Type_B_lists.Code,Type_B_lists.CandidateId);
                        //tblItemTableAdapter.StockUseUpdateQuery(Type_B_lists.StockUse, Type_B_lists.Code);

                    }


                
                    label4.Text = FCount.ToString() + " 건";
                    label5.Text = (FCount - errCount).ToString() + " 건";
                    label6.Text = errCount.ToString() + " 건";

                    EXCELRESULT _Form = new EXCELRESULT(label4.Text.Replace("건", ""), label5.Text.Replace("건", ""));
                    _Form.Owner = this;
                    _Form.StartPosition = FormStartPosition.CenterParent;
                    _Form.ShowDialog(

                        );
                    this.Close();
                    
                }
                else
                {

                    label4.Text = FCount.ToString() + " 건";
                    label5.Text = (FCount - errCount).ToString()+" 건";
                    label6.Text = errCount.ToString() + " 건";

                    label7.Visible = true;


                }

            }

            catch { }


           // conn.Close();
           
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (newDGV1.Rows.Count == 0)
            {
                MessageBox.Show("확인할 데이터가 없습니다.");
                return;

            }
            System.Diagnostics.Process.Start(FileName);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
