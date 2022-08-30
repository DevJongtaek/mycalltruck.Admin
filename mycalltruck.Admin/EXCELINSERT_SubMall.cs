using mycalltruck.Admin.Class;
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
    public partial class EXCELINSERT_SubMall : Form
    {
        string FileName = string.Empty;
        DateTime O_StopDate,O_RequestDate;
        Int64 O_Price = 0;
        Int64 O_Deposit = 0;
        string ErrorText = string.Empty;
        string FPIS_id = string.Empty;
        string Idx = string.Empty;
        string ERROR=string.Empty;
        int FCount = 0;
        int errCount = 0;
        public EXCELINSERT_SubMall()
        {
            InitializeComponent();
        }


        private void EXCELINSERT_Load(object sender, EventArgs e)
        {
            string Todate = DateTime.Now.ToString("yyyy/MM/01");
          
            this.staticOptionsTableAdapter.Fill(this.cMDataSet.StaticOptions);
            this.driversTableAdapter.Fill(this.cMDataSet.Drivers);
          
            cmb_Savegubun.SelectedIndex = 0;
          
        }

        private void ExcelTest()
        {

            newDGV1.DataSource = "";
            label7.Visible = false;
            OpenFileDialog d = new OpenFileDialog();
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.DRIVER);

            if (di.Exists == false)
            {

                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.DRIVER;
            d.Filter = "Excel2003-2007 (*.xls)|*.xls|Excel통합문서 (*.xlsx)|*.xlsx";
            d.FilterIndex = 1;

          
            

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                FileName = d.FileName;


                // OLEDB를 이용한 엑셀 연결
                string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                //string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES;ImportMixedTypes=Text;IMEX=1'";
                OleDbConnection conn = new OleDbConnection(szConn);
                try
                {
                    conn.Open();

                    // 엑셀로부터 데이타 읽기
                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
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
            FCount = 0;
            errCount = 0;


            string szConnf = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
            OleDbConnection connf = new OleDbConnection(szConnf);
            connf.Open();

            // 엑셀로부터 데이타 읽기
            OleDbCommand cmdf = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", connf);
            OleDbDataAdapter adptf = new OleDbDataAdapter(cmdf);


            DataSet dsf = new DataSet();
            adptf.Fill(dsf);

           
            // 엑셀 데이타 갱신
            cmdf = new OleDbCommand("UPDATE [입력사항$A2:Q] SET ERROR='' ", connf);



            cmdf.ExecuteNonQuery();
            connf.Close();

            List<DataGridViewRow> dataRows = new List<DataGridViewRow>();
           
            int RowErrorCount = 0;

          
            foreach (DataGridViewRow row in newDGV1.Rows)
            {
                


                RowErrorCount = 0;
                dataRows.Add(row);

            


                Idx = row.Cells[0].Value.ToString();

                if (Idx != "")
                {
                    FCount++;

                    string S_Idx = row.Cells[0].Value.ToString().Trim();
                    string SBiz_NO = row.Cells[1].Value.ToString().Trim().Replace("-", "");
                    string SName = row.Cells[2].Value.ToString().Trim();

                    string SUptae = row.Cells[3].Value.ToString().Trim();
                    string SUpjong = row.Cells[4].Value.ToString().Trim();


                    string SCeo = row.Cells[5].Value.ToString().Trim();
                    string SCeoBirth = row.Cells[6].Value.ToString().Trim();
              //      string SMobileNo = row.Cells[7].Value.ToString().Trim();
                    string SPhoneNo = row.Cells[7].Value.ToString().Trim();
                    string SFaxNo = row.Cells[8].Value.ToString().Trim();

                    string SEmail = row.Cells[9].Value.ToString().Trim();



                    string SState = row.Cells[10].Value.ToString().Trim();
                    string SCity = row.Cells[11].Value.ToString().Trim();
                    string SStreet = row.Cells[12].Value.ToString().Trim();

                 //   string SBizGubun = row.Cells[14].Value.ToString().Trim();

             //       string SRouteType = row.Cells[15].Value.ToString().Trim();

               //     string SInsurance = row.Cells[16].Value.ToString().Trim();

              //      string SCarNo = row.Cells[17].Value.ToString().Trim();

              //      string SCarType = row.Cells[18].Value.ToString().Trim();

              //      string SCarSize = row.Cells[19].Value.ToString().Trim();

              //      string SCarGubun = row.Cells[20].Value.ToString().Trim();
              //      string SCarYear = row.Cells[21].Value.ToString().Trim();

                    string SPayBankName = row.Cells[13].Value.ToString().Trim();
                    string SPayAccountNo = row.Cells[14].Value.ToString().Trim();
                    string SInputName = row.Cells[15].Value.ToString().Trim();


                    string SCarstate = row.Cells[10].Value.ToString().Trim();
                    string SCarcity = row.Cells[11].Value.ToString().Trim();
                 //   string SCarStreet = row.Cells[27].Value.ToString().Trim();

                //    string SfpisCartype = row.Cells[28].Value.ToString().Trim();

                    if (String.IsNullOrEmpty(S_Idx))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
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
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



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
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
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
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //상호 없으면 에러
                    else if (String.IsNullOrEmpty(SName))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",상호 빈값";
                        }
                        else
                        {
                            ErrorText = "상호 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }

                    //업태
                    else if (String.IsNullOrEmpty(SUptae))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",업태 빈값";
                        }
                        else
                        {
                            ErrorText = "업태 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //종목
                    else if (String.IsNullOrEmpty(SUpjong))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",종목 빈값";
                        }
                        else
                        {
                            ErrorText = "종목 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }

                         //대표자
                    else if (String.IsNullOrEmpty(SCeo))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",상호 빈값";
                        }
                        else
                        {
                            ErrorText = "상호 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //대표자생년월일
                    else if (String.IsNullOrEmpty(SCeoBirth))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",대표자생년월일 빈값 ";
                        }
                        else
                        {
                            ErrorText = "대표자생년월일 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //핸드폰번호
                    else if (String.IsNullOrEmpty(SCeoBirth))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",핸드폰번호 빈값 ";
                        }
                        else
                        {
                            ErrorText = "핸드폰번호 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //시도
                    else if (String.IsNullOrEmpty(SState))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",시도 빈값";
                        }
                        else
                        {
                            ErrorText = "시도 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //시군구
                    else if (String.IsNullOrEmpty(SCity))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",시군구 빈값";
                        }
                        else
                        {
                            ErrorText = "시군구 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //상세주소
                    else if (String.IsNullOrEmpty(SStreet))
                    {


                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",상세주소 빈값";
                        }
                        else
                        {
                            ErrorText = "상세주소 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;


                    }
                    //차량번호
                    
                    //은행
                    else if (String.IsNullOrEmpty(SPayBankName))
                    {
                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",은행 빈값";
                        }
                        else
                        {
                            ErrorText = "은행 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;
                    }
                    //계좌번호
                    else if (String.IsNullOrEmpty(SPayAccountNo))
                    {
                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",계좌번호 빈값";
                        }
                        else
                        {
                            ErrorText = "계좌번호 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;
                    }
                    else if (String.IsNullOrEmpty(SInputName))
                    {
                        // OLEDB를 이용한 엑셀 연결
                        string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                        OleDbConnection conn = new OleDbConnection(szConn);
                        conn.Open();

                        // 엑셀로부터 데이타 읽기
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                        OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                        DataSet ds = new DataSet();
                        adpt.Fill(ds);

                        if (RowErrorCount > 0)
                        {
                            ErrorText = ",예금주 빈값";
                        }
                        else
                        {
                            ErrorText = "예금주 빈값";

                        }
                        // 엑셀 데이타 갱신
                        cmd = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn);



                        cmd.ExecuteNonQuery();
                        conn.Close();


                        RowErrorCount++;
                    }
                   
                    //있으면  찾기
                    else
                    {
                        


                        #region 은행명

                        if (SPayBankName != "")
                        {

                            var BankName = Filter.Bank.BankList.Where(c => c.Text == SPayBankName).ToArray();
                            //  var BankCode = Filter.Bank.BankList.Where(c => c. == SPayBankName).ToArray();

                            if (BankName.Any())
                            {

                                //Row.PayBankName = BankName.First().Text;
                               // Row.PayBankCode = BankName.First().Value;
                               // Row.PayInputName = SInputName;
                               // Row.PayAccountNo = SPayAccountNo;
                            }

                            else
                            {
                                string szConn1 = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
                                OleDbConnection conn1 = new OleDbConnection(szConn1);
                                conn1.Open();

                                // 엑셀로부터 데이타 읽기
                                OleDbCommand cmd1 = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn1);
                                OleDbDataAdapter adpt1 = new OleDbDataAdapter(cmd1);


                                DataSet ds1 = new DataSet();
                                adpt1.Fill(ds1);

                                if (RowErrorCount > 0)
                                {
                                    ErrorText = ",등록된 은행 없음.";
                                }
                                else
                                {
                                    ErrorText = "등록된  은행 없음.";

                                }
                                // 엑셀 데이타 갱신
                                cmd1 = new OleDbCommand("UPDATE [입력사항$A2:Q] SET error='" + ErrorText + "' where IDX = " + Idx + " ", conn1);



                                cmd1.ExecuteNonQuery();
                                conn1.Close();


                                RowErrorCount++;


                                //  return;
                            }


                        }
                        else
                        {
                           // Row.PayBankName = "국민은행";
                           // Row.PayBankCode = "004";
                           // Row.PayInputName = " ";
                           // Row.PayAccountNo = "0";
                        }
                        #endregion





                      





                        string sCode;

                        var Driver_code = cMDataSet.Drivers.Select(c => new { c.Code }).OrderByDescending(c => c.Code).ToArray();
                        if (Driver_code.Count() > 0)
                        {
                            var DriverCode = 100001;
                            var DriverCodeCandidate = cMDataSet.Drivers.OrderBy(c => c.Code).Select(c => c.Code).ToArray();
                            while (true)
                            {
                                if (!DriverCodeCandidate.Any(c => c == DriverCode.ToString()))
                                {
                                    break;
                                }
                                DriverCode++;
                            }
                            sCode = DriverCode.ToString();
                        }
                        else
                        {

                            sCode = "100001";
                        }

                      //  Row.Code = sCode;


                        string S_BizNo = string.Empty;
                        string BiznoId = string.Empty;
                        string sPassword = string.Empty;

                        if (SBiz_NO.Length == 10)
                        {
                            S_BizNo = SBiz_NO.Substring(5, 5);
                        }
                        else if (SBiz_NO.Length == 12)
                        {
                            S_BizNo = SBiz_NO.Substring(7, 5);
                        }



                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            cn.Open();

                            SqlCommand selectCmd = new SqlCommand(
                                @"SELECT top 1 convert(int,right(LoginId,3)+1) as LoginId FROM Drivers Where right(BizNo,5) = @Bizno order by loginid desc", cn);
                            selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
                            var Reader = selectCmd.ExecuteReader();
                            while (Reader.Read())
                            {

                                BiznoId = Reader["LoginId"].ToString();

                            }
                            if (String.IsNullOrEmpty(BiznoId))
                            {
                                BiznoId = "";

                            }
                            cn.Close();
                        }
                        string BizNo = string.Empty;
                        if (BiznoId == "")
                        {
                            BizNo = "001";

                        }
                        else if (BiznoId.Length == 2)
                        {
                            BizNo = "0" + BiznoId;
                        }
                        else if (BiznoId.Length == 1)
                        {
                            BizNo = "00" + BiznoId;
                        }
                        else if (BiznoId.Length == 3)
                        {
                            BizNo = BiznoId;
                        }


                        string sLoginId = "m" + SBiz_NO.Substring(5, 5) + BizNo;

                        if (SPhoneNo.Length > 8)
                        {
                            sPassword = SPhoneNo.Substring(SPhoneNo.Length - 4, 4);
                        }


                   

                        DateTime sCreateDate = DateTime.Now;
                        string sUsePayNow = "0";
                        int sCandidateId = LocalUser.Instance.LogInInformation.ClientId;
                 



                    }










                    if (RowErrorCount > 0)
                    {
                        errCount++;
                    }

                }
            }
            if (errCount == 0)
            {
                label4.Text = FCount.ToString() + " 건";
                label5.Text = (FCount - errCount).ToString() + " 건";
                label6.Text = errCount.ToString() + " 건";

                label7.Visible = false;
                btn_Update.Enabled = true;
              
                //label8.Visible = true;
                //cmb_Savegubun.Visible = true;

               

            }
            else
            {

                label4.Text = FCount.ToString() + " 건";
                label5.Text = (FCount - errCount).ToString() + " 건";
                label6.Text = errCount.ToString() + " 건";

                label7.Visible = true;

               // btn_Update.Enabled = false;

                //label8.Visible = false;
                //cmb_Savegubun.Visible = false;
            }
           
           
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
            FCount = 0;
            errCount = 0;


            if (newDGV1.Rows.Count == 0)
            {
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;

            }

            string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES'";
            //string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties='Excel 8.0;HDR=YES;ImportMixedTypes=Text;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(szConn);
            try
            {
                conn.Open();

                // 엑셀로부터 데이타 읽기
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [입력사항$A2:Q] ", conn);
                OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);


                DataSet ds = new DataSet();
                adpt.Fill(ds);

                DataTable dTable = new DataTable();
                adpt.Fill(dTable);


                newDGV1.DataSource = dTable;


                conn.Close();


             

            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.ToString());
            }


            List<DataGridViewRow> dataRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in newDGV1.Rows)
            {



             

                CMDataSet.DriversRow Row = cMDataSet.Drivers.NewDriversRow();
                //   RowErrorCount = 0;
                dataRows.Add(row);

            




                Idx = row.Cells[0].Value.ToString();
                ERROR = row.Cells[16].Value.ToString();
                if (Idx != "" && ERROR == "" )
                {

                    string S_Idx = row.Cells[0].Value.ToString().Trim();
                    string SBiz_NO = row.Cells[1].Value.ToString().Trim().Replace("-", "");
                    string SName = row.Cells[2].Value.ToString().Trim();

                    string SUptae = row.Cells[3].Value.ToString().Trim();
                    string SUpjong = row.Cells[4].Value.ToString().Trim();


                    string SCeo = row.Cells[5].Value.ToString().Trim();
                    string SCeoBirth = row.Cells[6].Value.ToString().Trim();
                 //   string SMobileNo = row.Cells[7].Value.ToString().Trim();
                     string SPhoneNo = row.Cells[7].Value.ToString().Trim();
                    string SFaxNo = row.Cells[8].Value.ToString().Trim();

                    string SEmail = row.Cells[9].Value.ToString().Trim();



                    string SState = row.Cells[10].Value.ToString().Trim();
                    string SCity = row.Cells[11].Value.ToString().Trim();
                    string SStreet = row.Cells[12].Value.ToString().Trim();

                    //string SBizGubun = row.Cells[14].Value.ToString().Trim();

                    //string SRouteType = row.Cells[15].Value.ToString().Trim();

                    //string SInsurance = row.Cells[16].Value.ToString().Trim();

                    //string SCarNo = row.Cells[17].Value.ToString().Trim();

                    //string SCarType = row.Cells[18].Value.ToString().Trim();

                    //string SCarSize = row.Cells[19].Value.ToString().Trim();

                    //string SCarGubun = row.Cells[20].Value.ToString().Trim();
                    //string SCarYear = row.Cells[21].Value.ToString().Trim();

                    string SPayBankName = row.Cells[13].Value.ToString().Trim();
                    string SPayAccountNo = row.Cells[14].Value.ToString().Trim();
                    string SInputName = row.Cells[15].Value.ToString().Trim();


                    string SCarstate = row.Cells[10].Value.ToString().Trim();
                    string SCarcity = row.Cells[11].Value.ToString().Trim();
               //     string SCarStreet = row.Cells[27].Value.ToString().Trim();

                  //  string SfpisCartype = row.Cells[28].Value.ToString().Trim();


                    #region 은행명

                    if (SPayBankName != "")
                    {

                        var BankName = Filter.Bank.BankList.Where(c => c.Text == SPayBankName).ToArray();
                        //  var BankCode = Filter.Bank.BankList.Where(c => c. == SPayBankName).ToArray();

                        if (BankName.Any())
                        {

                            Row.PayBankName = BankName.First().Text;
                            Row.PayBankCode = BankName.First().Value;
                            Row.PayInputName = SInputName;
                            Row.PayAccountNo = SPayAccountNo;
                        }




                    }
                    else
                    {
                        Row.PayBankName = "국민은행";
                        Row.PayBankCode = "004";
                        Row.PayInputName = " ";
                        Row.PayAccountNo = "0";
                    }
                    #endregion


                


                    Row.BizNo = SBiz_NO;
                    Row.Name = SName;
                    Row.CEO = SCeo;
                    Row.Uptae = SUptae;
                    Row.Upjong = SUpjong;




                    Row.Email = SEmail;

             //       Row.MobileNo = SMobileNo;

                    Row.PhoneNo = SPhoneNo;
                    Row.MobileNo = SPhoneNo;
                    Row.FaxNo = SFaxNo;
                    Row.AddressState = SState;
                    Row.AddressCity = SCity;
                    Row.AddressDetail = SStreet;

                    Row.CEOBirth = SCeoBirth;

                
                    Row.ParkState = SCarstate;
                    Row.ParkCity = SCarcity;
                    Row.ParkStreet = "";



                    string sCode;

                    var Driver_code = cMDataSet.Drivers.Select(c => new { c.Code }).OrderByDescending(c => c.Code).ToArray();
                    if (Driver_code.Count() > 0)
                    {
                        var DriverCode = 100001;
                        var DriverCodeCandidate = cMDataSet.Drivers.OrderBy(c => c.Code).Select(c => c.Code).ToArray();
                        while (true)
                        {
                            if (!DriverCodeCandidate.Any(c => c == DriverCode.ToString()))
                            {
                                break;
                            }
                            DriverCode++;
                        }
                        sCode = DriverCode.ToString();
                    }
                    else
                    {

                        sCode = "100001";
                    }

                    Row.Code = sCode;


                    string S_BizNo = string.Empty;
                    string BiznoId = string.Empty;
                    string sPassword = string.Empty;

                    if (SBiz_NO.Length == 10)
                    {
                        S_BizNo = SBiz_NO.Substring(5, 5);
                    }
                    else if (SBiz_NO.Length == 12)
                    {
                        S_BizNo = SBiz_NO.Substring(7, 5);
                    }



                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();

                        SqlCommand selectCmd = new SqlCommand(
                            @"SELECT top 1 convert(int,right(LoginId,3)+1) as LoginId FROM Drivers Where right(BizNo,5) = @Bizno order by loginid desc", cn);
                        selectCmd.Parameters.Add(new SqlParameter("@Bizno", S_BizNo));
                        var Reader = selectCmd.ExecuteReader();
                        while (Reader.Read())
                        {

                            BiznoId = Reader["LoginId"].ToString();

                        }
                        if (String.IsNullOrEmpty(BiznoId))
                        {
                            BiznoId = "";

                        }
                        cn.Close();
                    }
                    string BizNo = string.Empty;
                    if (BiznoId == "")
                    {
                        BizNo = "001";

                    }
                    else if (BiznoId.Length == 2)
                    {
                        BizNo = "0" + BiznoId;
                    }
                    else if (BiznoId.Length == 1)
                    {
                        BizNo = "00" + BiznoId;
                    }
                    else if (BiznoId.Length == 3)
                    {
                        BizNo = BiznoId;
                    }


                    string sLoginId = "m" + SBiz_NO.Substring(5, 5) + BizNo;

                    if (SPhoneNo.Length > 8)
                    {
                        sPassword = SPhoneNo.Substring(SPhoneNo.Length - 4, 4);
                    }


                    Row.LoginId = sLoginId;
                    Row.Password = sPassword;


                    DateTime sCreateDate = DateTime.Now;
                    string sUsePayNow = "0";
                    int sCandidateId = LocalUser.Instance.LogInInformation.ClientId;
                    Row.CreateDate = sCreateDate;

                    Row.AccountOwner = "";
                    Row.AccountRegNo = "";
                    Row.BankName = "";
                    Row.AccountNo = "";
                    Row.AccountExtra = "";


                    if (String.IsNullOrEmpty(sUsePayNow))
                    {
                        Row.UsePayNow = 2;
                    }
                    else
                    {
                        Row.UsePayNow = int.Parse(sUsePayNow);
                    }
                    Row.ClientBizType = 0;
                    //     row.CandidateId = int.Parse(cmb_CandidateGubun.SelectedValue.ToString());
                    Row.CandidateId = LocalUser.Instance.LogInInformation.ClientId;

                    Row.Car_ContRact = false;



                    Row.BizType = 1;
                    Row.RouteType = 1;
                    Row.InsuranceType = 1;
                    Row.CarNo = "1";
                    Row.CarType = 1;
                    Row.CarSize = 1;

                 


                    Row.UsePayNow = 0;
                    Row.ClientBizType = 0;
                   


                    Row.AccountUse = false;
                    Row.DTGUse = true;
                    Row.FPISUse = true;
                    Row.MyCallUSe = true;
                    Row.OTGUse = false;
                    Row.ServicePrice = "5500";
                    Row.useTax = true;
                    Row.OTGPrice = 0;
                    Row.AccountPrice = 0;
                    Row.FPISPrice = 0;
                    Row.MyCallPrice = 0;
                    Row.DTGPrice = 5000;

                    Row.LG_MertKeyYn = false;
                    Row.ServiceState = 3;
                    Row.CarYear = SCeo;
                  

                    cMDataSet.Drivers.AddDriversRow(Row);

                    FCount++;
                }
            }

            driversTableAdapter.Update(cMDataSet.Drivers);

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

        private void cmb_Savegubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Savegubun.SelectedIndex == 0)
            {
                btn_Update.Enabled = true;
            }
            else
            {
                btn_Update.Enabled = false;
            }
        }
    }
}
