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
    public partial class FrmMNDriverVaccount : Form
    {
        public FrmMNDriverVaccount()
        {
            InitializeComponent();
        }

        private void FrmMNDriverVaccount_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'pointsDataSet.DriverPointsList' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.driverPointsListTableAdapter.Fill(this.pointsDataSet.DriverPointsList);
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
                //{ "배차일시", "배차일시" },
                { "아이디", "아이디" },
                { "차량번호", "차량번호" },
                { "차주명", "차주명" },


            };


            cmbSSearch.DataSource = new BindingSource(Ssearch, null);
            cmbSSearch.DisplayMember = "Value";
            cmbSSearch.ValueMember = "Key";

            cmbSSearch.SelectedIndex = 0;


        }
        private String GetSelectCommand()
        {
            return @"SELECT DP.CDate,DP.Amount, D.LoginId,d.CarNo,d.CarYear,'카드' AS ChargeGubun,CASE WHEN DP.PointItem ='충전' THEN'승인'else '취소' end as  AccFunction,ac.LGD_TID,ac.CardNo,ac.LGD_RESPCODE
                    ,CASE WHEN DP.PointItem ='충전' and AC.LGD_RESPCODE='0000' THEN ac.LGD_RESPMSG else '취소성공' end as LGD_RESPMSG   FROM DriverPoints as DP
                    JOIN AccLogs as  AC on DP.DriverId = ac.TradeId and AC.LGD_MID = 'chasero' and DP.LGD_Result = ac.LGD_TID
                    JOIN Drivers as D on dp.DriverId = D.DriverId   ";


        }
        private void LoadTable()
        {
            pointsDataSet.DriverPointsList.Clear();


            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();

               

                    WhereStringList.Add("CONVERT(VARCHAR(10),DP.CDate,111) >= @Sdate AND CONVERT(VARCHAR(10),DP.CDate,111) <= @Edate");
                    _Command.Parameters.AddWithValue("@Sdate", dtp_Sdate.Text);
                    _Command.Parameters.AddWithValue("@edate", dtp_Edate.Text);

                    if (cmbSSearch.Text == "아이디")
                    {
                        WhereStringList.Add(string.Format(" D.LoginId Like '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("d.CarNo Like  '%{0}%'", txtSText.Text));
                    }
                    else if (cmbSSearch.Text == "차주명")
                    {
                        WhereStringList.Add(string.Format("d.CarYear Like  '%{0}%'", txtSText.Text));
                    }



                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by DP.CDate  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        pointsDataSet.DriverPointsList.Load(_Reader);



                    }
                }


                _Connection.Close();


            }

            var Query = pointsDataSet.DriverPointsList.ToArray();

            if (Query.Any())
            {
                lblApprove.Text = Query.Where(c => c.AccFunction == "승인").Sum(c => c.Amount).ToString("N0");
                lblCancle.Text = Query.Where(c => c.AccFunction == "취소").Sum(c =>-c.Amount).ToString("N0");
                lblAmout.Text = Query.Sum(c => c.Amount).ToString("N0");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTable();
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

        private void txtSText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            LoadTable();
        }

        private void grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as PointsDataSet.DriverPointsListRow;


            if (grid1.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (grid1.Rows.Count - e.RowIndex).ToString("N0");
            }

            if (grid1.Columns[e.ColumnIndex] == cDateDataGridViewTextBoxColumn)
            {
                e.Value = Selected.CDate.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if(e.ColumnIndex == cardNoDataGridViewTextBoxColumn.Index)
            {

                e.Value = Selected.CardNo.Substring(0, Selected.CardNo.Length - 4) + "****";
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
            var fileString = "차주(카드)충전내역" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
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
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.DriverVaccount))
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
                _Sheet.Cells[RowIndex, 4].Value = grid1[loginIdDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 5].Value = grid1[carNoDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 6].Value = grid1[carYearDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 7].Value = grid1[chargeGubunDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 8].Value = grid1[accFunctionDataGridViewTextBoxColumn.Index, i].FormattedValue;

                _Sheet.Cells[RowIndex, 9].Value = grid1[lGDTIDDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 10].Value = grid1[cardNoDataGridViewTextBoxColumn.Index, i].FormattedValue;

                _Sheet.Cells[RowIndex, 11].Value = grid1[lGDRESPCODEDataGridViewTextBoxColumn.Index, i].FormattedValue;
                _Sheet.Cells[RowIndex, 12].Value = grid1[lGDRESPMSGDataGridViewTextBoxColumn.Index, i].FormattedValue;
                


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
    }
}
