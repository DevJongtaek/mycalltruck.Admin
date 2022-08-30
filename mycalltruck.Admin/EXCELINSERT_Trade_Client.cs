using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;
using mycalltruck.Admin.Class.Common;
using System.Data.SqlClient;

namespace mycalltruck.Admin
{
    public partial class EXCELINSERT_Trade_Client : Form
    {
        string FileName = string.Empty;
        DateTime O_StopDate, O_RequestDate;
        Int64 O_Price = 0;
        Int64 O_Deposit = 0;
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        public EXCELINSERT_Trade_Client()
        {
            InitializeComponent();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");

            this.staticOptionsTableAdapter.Fill(this.cMDataSet.StaticOptions);
            this.drivers_Car2TableAdapter.Fill(this.cMDataSet.Drivers_Car2);
            this.clientsTableAdapter.Fill(this.cMDataSet.Clients);
            this.clientUsersTableAdapter.Fill(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);
            this.salesManageTableAdapter.Fill(this.cMDataSet.SalesManage);
            //dtp_BeginDate.Value = DateTime.Parse(Todate).AddMonths(-1);
            //dtp_EndDate.Value = DateTime.Parse(Todate).AddDays(-1);
        }

        private void ExcelTest()
        {

            newDGV1.DataSource = "";
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.TRADE);

            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.TRADE;
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
                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn);
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
                catch (Exception e)
                {
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
            OleDbCommand cmdf = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", connf);
            OleDbDataAdapter adptf = new OleDbDataAdapter(cmdf);


            DataSet dsf = new DataSet();
            adptf.Fill(dsf);


            // 엑셀 데이타 갱신
            cmdf = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='' ", connf);



            cmdf.ExecuteNonQuery();
            connf.Close();

            List<DataGridViewRow> dataRows = new List<DataGridViewRow>();
            int errCount = 0;
            int RowErrorCount = 0;
            int FCount = 0;

            foreach (DataGridViewRow row in newDGV1.Rows)
            {




                CMDataSet.SalesManageRow Row = cMDataSet.SalesManage.NewSalesManageRow();
                RowErrorCount = 0;
                dataRows.Add(row);



                Idx = row.Cells[0].Value.ToString();

                if (Idx != "")
                {
                    FCount++;
                }
                string S_Idx = row.Cells[0].Value.ToString().Trim();
                string SBiz_NO = row.Cells[1].Value.ToString().Trim().Replace("-", "");
                string SSangHo = row.Cells[2].Value.ToString().Trim();
                string SItem = row.Cells[3].Value.ToString().Trim();
                if (String.IsNullOrEmpty(S_Idx))
                {


                    // OLEDB를 이용한 엑셀 연결
                    string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                    OleDbConnection conn = new OleDbConnection(szConn);
                    conn.Open();

                    // 엑셀로부터 데이타 읽기
                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn);
                    OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                    DataSet ds = new DataSet();
                    adpt.Fill(ds);

                    if (RowErrorCount > 0)
                    {
                        ErrorText = ",IDX 빈값";
                    }
                    else
                    {
                        ErrorText = "IDX 빈값";

                    }
                    // 엑셀 데이타 갱신
                    cmd = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                    cmd.ExecuteNonQuery();
                    conn.Close();


                    RowErrorCount++;


                }
                //사업자등록번호 없으면 에러
                else if (String.IsNullOrEmpty(SBiz_NO) || SBiz_NO.Length != 10)
                {


                    // OLEDB를 이용한 엑셀 연결
                    string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                    OleDbConnection conn = new OleDbConnection(szConn);
                    conn.Open();

                    // 엑셀로부터 데이타 읽기
                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn);
                    OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                    DataSet ds = new DataSet();
                    adpt.Fill(ds);

                    if (RowErrorCount > 0)
                    {
                        ErrorText = ",사업자번호 형식 또는 빈값";
                    }
                    else
                    {
                        ErrorText = "사업자번호 형식 또는 빈값";

                    }
                    // 엑셀 데이타 갱신
                    cmd = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                    cmd.ExecuteNonQuery();
                    conn.Close();


                    RowErrorCount++;


                }
                //else if (String.IsNullOrEmpty(SSangHo))
                //{
                //    // OLEDB를 이용한 엑셀 연결
                //    string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                //    OleDbConnection conn = new OleDbConnection(szConn);
                //    conn.Open();

                //    // 엑셀로부터 데이타 읽기
                //    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn);
                //    OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                //    DataSet ds = new DataSet();
                //    adpt.Fill(ds);

                //    if (RowErrorCount > 0)
                //    {
                //        ErrorText = ",협력업체상호 없습니다.";
                //    }
                //    else
                //    {
                //        ErrorText = "협력업체상호 없습니다.";

                //    }
                //    // 엑셀 데이타 갱신
                //    cmd = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                //    cmd.ExecuteNonQuery();
                //    conn.Close();


                //    RowErrorCount++;
                //}
                else if (String.IsNullOrEmpty(SItem))
                {
                    // OLEDB를 이용한 엑셀 연결
                    string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                    OleDbConnection conn = new OleDbConnection(szConn);
                    conn.Open();

                    // 엑셀로부터 데이타 읽기
                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn);
                    OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                    DataSet ds = new DataSet();
                    adpt.Fill(ds);

                    if (RowErrorCount > 0)
                    {
                        ErrorText = ",청구항목이 없습니다.";
                    }
                    else
                    {
                        ErrorText = "청구항목이 없습니다.";

                    }
                    // 엑셀 데이타 갱신
                    cmd = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                    cmd.ExecuteNonQuery();
                    conn.Close();


                    RowErrorCount++;
                }
                //있으면  찾기
                else
                {
                    #region 운송사정보

                    if (row.Cells[1].Value.ToString().Trim().Replace("-", "") != "" || row.Cells[1].Value.ToString().Trim().Replace("-", "").Length == 10)
                    {





                        var Query = cMDataSet.Clients.Where(c => c.BizNo.Replace("-","") == SBiz_NO.Replace("-", "")).ToArray();

                        if (Query.Any())
                        {



                            Row.Uptae = Query.First().Uptae;
                            Row.Upjong = Query.First().Upjong;
                            Row.AddressState = Query.First().AddressState;
                            Row.AddressCity = Query.First().AddressCity;
                            Row.AddressDetail = Query.First().AddressDetail;
                            Row.Email = Query.First().Email;
                            Row.ContRactName = Query.First().CEO;
                            Row.Ceo = Query.First().CEO;
                            Row.MobileNo = Query.First().MobileNo;
                            Row.CustomerId = Query.First().ClientId;
                          //  Row.ClientId = Query.First().ClientId;

                        }

                        else
                        {
                            string szConn1 = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                            OleDbConnection conn1 = new OleDbConnection(szConn1);
                            conn1.Open();

                            // 엑셀로부터 데이타 읽기
                            OleDbCommand cmd1 = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn1);
                            OleDbDataAdapter adpt1 = new OleDbDataAdapter(cmd1);


                            DataSet ds1 = new DataSet();
                            adpt1.Fill(ds1);

                            if (RowErrorCount > 0)
                            {
                                ErrorText = ",등록된 사업자 없음.";
                            }
                            else
                            {
                                ErrorText = "등록된 사업자 없음.";

                            }
                            // 엑셀 데이타 갱신
                            cmd1 = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn1);



                            cmd1.ExecuteNonQuery();
                            conn1.Close();


                            RowErrorCount++;


                            //  return;
                        }


                    }

                    #endregion
                    #region 직접배송정보
                    string D_date = string.Empty;
                    string D_dateString = row.Cells[4].Value.ToString().Replace(".0", "");

                    if (D_dateString.Length > 0)
                    {
                        if (D_dateString.Length == 8)
                        {
                            D_date = D_dateString.Substring(0, 4) + "-" + D_dateString.Substring(4, 2) + "-" + D_dateString.Substring(6, 2);
                        }
                        else if (D_dateString.Length == 10)
                        {
                            D_date = D_dateString.Substring(0, 4) + "-" + D_dateString.Substring(5, 2) + "-" + D_dateString.Substring(8, 2);
                        }
                    }



                    if (!DateTime.TryParse(D_date, out O_StopDate))
                    {
                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);
                        if (RowErrorCount > 0)
                        {
                            ErrorText += " ,청구일 형식틀림 ";
                        }
                        else
                        {
                            ErrorText = " 청구일 형식틀림";
                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();

                        RowErrorCount++;



                        // return;
                    }
                    else
                    {
                        Row.RequestDate = DateTime.Parse(D_date);


                    }
                    if (Int64.TryParse(row.Cells[5].Value.ToString().Replace(",", ""), out O_Price))
                    {
                        Row.UnitPrice = O_Price;
                        Row.Price = O_Price;



                        Row.Vat = long.Parse(Math.Truncate((O_Price * 0.1)).ToString());
                        Row.Amount = Row.Price + Row.Vat;

                    }
                    else
                    {
                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$A1:G] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        ErrorText = "청구금액형식오류";
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [Sheet1$A1:G] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();

                        RowErrorCount++;
                        // return;

                    }





                    Row.PayDate = DateTime.Now;
                    Row.PayState = 2;
                    Row.PayBankName = "";

                    Row.ClientId = LocalUser.Instance.LogInInformation.ClientId;

                    Row.UseTax = true;
                    Row.SetYN = 0;

                    Row.CardPayGubun = "C";
                    Row.CreateDate = DateTime.Now;

                   // cMDataSet.SalesManage.AddSalesManageRow(Row);
                    #endregion


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
                    label4.Text = FCount.ToString() + " 건";
                    label5.Text = (FCount - errCount).ToString() + " 건";
                    label6.Text = errCount.ToString() + " 건";
                    label7.Visible = false;
                    btn_Update.Enabled = true;
                }
                else
                {
                    label4.Text = FCount.ToString() + " 건";
                    label5.Text = (FCount - errCount).ToString() + " 건";
                    label6.Text = errCount.ToString() + " 건";
                    label7.Visible = true;
                    btn_Update.Enabled = false;
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

        private void btn_Update_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> dataRows = new List<DataGridViewRow>();
            int errCount = 0;
            int RowErrorCount = 0;
            int FCount = 0;

            foreach (DataGridViewRow row in newDGV1.Rows)
            {




                CMDataSet.SalesManageRow Row = cMDataSet.SalesManage.NewSalesManageRow();
                RowErrorCount = 0;
                dataRows.Add(row);



                Idx = row.Cells[0].Value.ToString();

                if (Idx != "")
                {
                    FCount++;
                }
                string S_Idx = row.Cells[0].Value.ToString().Trim();
                string SBiz_NO = row.Cells[1].Value.ToString().Trim().Replace("-", "");
                string SSangHo = row.Cells[2].Value.ToString().Trim();
                string SItem = row.Cells[3].Value.ToString().Trim();



               

                #region 운송사정보

                if (row.Cells[1].Value.ToString().Trim().Replace("-", "") != "" || row.Cells[1].Value.ToString().Trim().Replace("-", "").Length == 10)
                {


                    Row.BizNo = SBiz_NO.Substring(0, 3) + "-" + SBiz_NO.Substring(3, 2) +"-"+ SBiz_NO.Substring(5, 5);
                  
                    Row.Item = SItem;


                    var Query = cMDataSet.Clients.Where(c => c.BizNo.Replace("-","") == SBiz_NO.Replace("-", "")).ToArray();

                    if (Query.Any())
                    {

                        Row.ClientId = Query.First().ClientId;                       
                    }

                    var Query2 = cMDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

                    if (Query2.Any())
                    {
                        Row.PayAccountNo = Query2.First().CMSAccountNo;
                        Row.PayBankName = Query2.First().CMSBankName;
                        Row.PayBankCode = Query2.First().CMSBankCode;
                        Row.PayInputName = Query2.First().CMSOwner;

                        Row.SangHo = Query2.First().Name;
                        Row.Uptae = Query2.First().Uptae;
                        Row.Upjong = Query2.First().Upjong;
                        Row.AddressState = Query2.First().AddressState;
                        Row.AddressCity = Query2.First().AddressCity;
                        Row.AddressDetail = Query2.First().AddressDetail;
                        Row.Email = Query2.First().Email;
                        Row.ContRactName = Query2.First().CEO;
                        Row.Ceo = Query2.First().CEO;
                        Row.MobileNo = Query2.First().MobileNo;
                       
                    }
                   


                }

                #endregion
                #region 직접배송정보
                string D_date = string.Empty;
                string D_dateString = row.Cells[4].Value.ToString().Replace(".0", "");

                if (D_dateString.Length > 0)
                {
                    if (D_dateString.Length == 8)
                    {
                        D_date = D_dateString.Substring(0, 4) + "-" + D_dateString.Substring(4, 2) + "-" + D_dateString.Substring(6, 2);
                    }
                    else if (D_dateString.Length == 10)
                    {
                        D_date = D_dateString.Substring(0, 4) + "-" + D_dateString.Substring(5, 2) + "-" + D_dateString.Substring(8, 2);
                    }
                }




                Row.RequestDate = DateTime.Parse(D_date);


                if (Int64.TryParse(row.Cells[5].Value.ToString().Replace(",", ""), out O_Price))
                {
                    Row.UnitPrice = O_Price;
                    Row.Price = O_Price;



                    Row.Vat = long.Parse(Math.Truncate((O_Price * 0.1)).ToString());
                    Row.Amount = Row.Price + Row.Vat;
                    Row.Num = 1;
                }




                Row.CreateDate = DateTime.Now;
                Row.HasACC = true;
                Row.CardPayGubun = "C";
                //Row.ClientId = LocalUser.Instance.LogInInfomation.ClientId;
                Row.CustomerId = LocalUser.Instance.LogInInformation.ClientId;


             
                Row.PayState = 2;
              

               

                Row.UseTax = true;
                Row.SetYN = 0;

              
              

                cMDataSet.SalesManage.AddSalesManageRow(Row);
                #endregion 

            }

            salesManageTableAdapter.Update(cMDataSet.SalesManage);


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
       
       


    }
}
