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
    public partial class EXCELINSERT_DRIVER_LG : Form
    {
        string FileName = string.Empty;
        DateTime O_StopDate,O_RequestDate;
        Int64 O_Price = 0;
        Int64 O_Deposit = 0;
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        public EXCELINSERT_DRIVER_LG()
        {
            InitializeComponent();
        }


     
        private void EXCELINSERT_LG_Load(object sender, EventArgs e)
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
                    OleDbCommand cmd = new OleDbCommand("SELECT no,상점ID,머트키 FROM [청약일괄등록$A1:AB] WHERE 상점 <> '' ", conn);
                    OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                    DataSet ds = new DataSet();
                    adpt.Fill(ds);

                    DataTable dTable = new DataTable();
                    adpt.Fill(dTable);


                    newDGV1.DataSource = dTable;


                    conn.Close();

                    
                 

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
            OleDbCommand cmdf = new OleDbCommand("SELECT no,상점ID,머트키 FROM [청약일괄등록$A1:AB] WHERE 상점 <> '' ", connf);
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



                string No = row.Cells[0].Value.ToString().Trim();
                string SDriverCode = row.Cells[1].Value.ToString().Trim();
                string SMID = row.Cells[27].Value.ToString().Trim();
                //string SMID = row.Cells[0].Value.ToString().Trim();
                //string SDriverCode= row.Cells[1].Value.ToString().Trim();
                //string SClientCode = row.Cells[2].Value.ToString().Trim();
                //string SClientName = row.Cells[3].Value.ToString().Trim();


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
                else if (String.IsNullOrEmpty(No))
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

                    //if (SClientCode != "")
                    //{

                    //    var Query = cMDataSet.Clients.Where(c => c.Code == SClientCode).ToArray();

                    //    if (Query.Any())
                    //    {

                    //        Row.CandidateId = Query.First().ClientId;
                    //    }

                    //    else
                    //    {
                          

                    //        RowErrorCount++;


                    //        //  return;
                    //    }


                    //}

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


                        driversTableAdapter.LGMIDUpdateQuery(Type_B_lists.MID, Type_B_lists.Code);
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
