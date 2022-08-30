
using mycalltruck.Admin.Class.Extensions;

using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMNCarFPIS : Form
    {
        string Gubun = string.Empty;
        string sDate = string.Empty;
        string eDate = string.Empty;
        string FPIS_FILE_INFO = string.Empty;
        string FPIS_ID_IN = string.Empty;
        int R_index = 0;
        public FrmMNCarFPIS()
        {
            InitializeComponent();

          
        }

        private void FrmMNCarFPIS_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.FPIS_FILE_CAR' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.fPIS_FILE_CARTableAdapter.Fill(this.cMDataSet.FPIS_FILE_CAR);
           // this.driversTableAdapter.Fill(this.cMDataSet.Drivers);
            this.driver1TableAdapter.Fill(this.cMDataSet.Driver1, LocalUser.Instance.LogInInformation.ClientId);
          
            _InitCmb();


            sDate = DateTime.Now.ToString("yyyy") + "/01/01";
            eDate = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/");
        }

        private void _InitCmb()
        {

            var DealersDataSource = cMDataSet.Driver1.ToArray();

            cmb_Drivers.DataSource = DealersDataSource;
            cmb_Drivers.DisplayMember = "Name";
            cmb_Drivers.ValueMember = "DriverId";
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_FILE_CARRow;


                dataGridView1[e.ColumnIndex, e.RowIndex].Value = Selected.CONT_FROM + "-" + Selected.CONT_TO;
            }

            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_FILE_CARRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId);

                if (Query.Any())
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().Name;

                }
            }
        }

        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;


        string _FpisSearch = string.Empty;
        string _DateSearchString2 = string.Empty;
        string _FilterString2 = string.Empty;
        private void btn_File_Click(object sender, EventArgs e)
        {

           
            //lbl_Title.Text = "차량별 직접운송 생성";
            //btn_File.Text = "직접운송 화일생성";


          //  dataGridView1.Columns[1].HeaderText = "계약일(FROM)";
            var DriverQuery = cMDataSet.Driver1.Where(c => c.DriverId != 0).ToArray();
            foreach (var item in DriverQuery)
            {
                Gubun = "CAR";
                R_index = 0;



                fPISFILECARBindingSource.Filter = "ClientId = " + LocalUser.Instance.LogInInformation.ClientId + " AND FPIS_GUBUN = 'CAR' AND RE_GUBUN = '" + R_index.ToString() + "' ";



                FPIS_FILE_INFO = LocalUser.Instance.PersonalOption.FPISCAR + "\\" + item.Name + "\\운송";

                _FpisSearch = string.Empty;
                _DateSearchString2 = string.Empty;
                _FilterString2 = string.Empty;

                if (R_index == 1)
                {
                    //sDate = dtp_Sdate.Text;
                    //eDate = dtp_Edate.Text;
                }


                fPIS_CAR_EXCEL_ATableAdapter.FillByDrivers(cMDataSet.FPIS_CAR_EXCEL_A, DateTime.Parse(sDate), DateTime.Parse(eDate).AddDays(1), item.DriverId);

                if (R_index == 0)
                {
                    _FpisSearch = String.Format(" ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') ='{0}' ", "9999-12-31 00:00:00.000");
                }
                else if (R_index == 1)
                {
                    _FpisSearch = String.Format(" ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') <> '{0}' ", "9999-12-31 00:00:00.000");
                }

                fPISCAREXCELABindingSource.Filter = _FpisSearch;



                if (R_index == 0)
                {

                    fileString = string.Format("{0}_운송_{1}_{2}{3}", item.Name, sDate.Replace("/", "").Substring(2, 6), eDate.Replace("/", "").Substring(2, 6), DateTime.Now.ToString("hh:mm:ss").Replace(":", ""));
                }
                else
                {
                    fileString = string.Format("{0}_운송_{1}_{2}{3}_1", item.Name, sDate.Replace("/", "").Substring(2, 6), eDate.Replace("/", "").Substring(2, 6), DateTime.Now.ToString("hh:mm:ss").Replace(":", ""));
                }
                title = "실적데이터";
                ieExcel = Properties.Resources.일괄입력운송신고양식;















                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (newDGV2.RowCount == 0)
                    {
                      //  MessageBox.Show("내보낼 운송정보 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {

                        #region 화일생성일
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            if (!LocalUser.Instance.LogInInformation.IsAdmin)
                            {
                                


                                cn.Open();
                                SqlCommand cmd = cn.CreateCommand();

                                if (R_index == 0)
                                {

                                    //cmd.CommandText =
                                    //   "UPDATE Orders SET FPIS_F_DATE = getdate() WHERE convert(varchar(10),StopTime,111) >= @CAR_END_TIME  AND convert(varchar(10),StopTime,111) <= @CAR_END_TIME2 AND OrderStatus = '3' and   FPIS_ID IN " + "( select fpis_id from fpis_cont where CliendId in(" + String.Join(",", VisibleOrderIds) + ")) AND ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') ='9999-12-31 00:00:00.000'    ";

                                    cmd.CommandText =
                                     "UPDATE Orders SET FPIS_F_DATE = getdate() WHERE convert(varchar(10),StopTime,111) >= @CAR_END_TIME  AND convert(varchar(10),StopTime,111) <= @CAR_END_TIME2 AND OrderStatus = '3' and   DriverId =@DriverId  AND ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') ='9999-12-31 00:00:00.000'    ";


                                }
                                else
                                {
                                    cmd.CommandText =
                                      "UPDATE Orders SET FPIS_F_DATE = getdate() WHERE convert(varchar(10),StopTime,111) >= @CAR_END_TIME  AND convert(varchar(10),StopTime,111) <= @CAR_END_TIME2 AND OrderStatus = '3' and   DriverId = @DriverId   AND ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') !='9999-12-31 00:00:00.000' ";
                                }

                                cmd.Parameters.AddWithValue("@CAR_END_TIME", sDate);
                                cmd.Parameters.AddWithValue("@CAR_END_TIME2", eDate);
                                cmd.Parameters.AddWithValue("@DriverId", item.DriverId);
                                cmd.ExecuteNonQuery();
                                cn.Close();

                                //fPIS_FILETableAdapter.Insert(sDate, eDate, newDGV2.RowCount, FPIS_FILE_INFO, fileString, eDate, DateTime.Now, "CAR", R_index.ToString(), LocalUser.Instance.LogInInfomation.ClientId);

                                fPIS_FILE_CARTableAdapter.Insert(sDate, eDate, newDGV2.RowCount, FPIS_FILE_INFO, fileString, eDate, DateTime.Now, "CAR", R_index.ToString(), LocalUser.Instance.LogInInformation.ClientId, item.DriverId, "");
                            }

                        }
                        #endregion



                    }
                }




                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (newDGV2.RowCount > 0)
                    {





                        pnProgress.Visible = true;
                        bar.Value = 0;
                        Thread t = new Thread(new ThreadStart(() =>
                        {

                            newDGV2.ExportExistExcel3(title, fileString, bar, true, ieExcel, 3, FPIS_FILE_INFO);
                            pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                        }));
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();


                    }
                    else
                    {
                        //MessageBox.Show("엑셀로 내보낼 운송정보가 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
                        //    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
                
                fPISFILECARBindingSource.Filter = "FPIS_GUBUN = 'CAR' AND RE_GUBUN = '" + R_index.ToString() + "' ";
                //  this.fPIS_FILETableAdapter.Fill(this.cMDataSet.FPIS_FILE);
            }

            this.fPIS_FILE_CARTableAdapter.Fill(this.cMDataSet.FPIS_FILE_CAR);
        }

        private void cmb_Drivers_SelectedIndexChanged(object sender, EventArgs e)
        {
            sDate = DateTime.Now.ToString("yyyy") + "/01/01";
            eDate = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/");

           // fPIS_CAR_EXCEL_ATableAdapter.FillByDrivers(cMDataSet.FPIS_CAR_EXCEL_A, DateTime.Parse(sDate), DateTime.Parse(eDate).AddDays(1), 308);
        }
    }
}
