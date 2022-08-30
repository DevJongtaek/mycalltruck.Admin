using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.UI;
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
    public partial class FrmPayExcel : Form
    {
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

               
                    //grid2.ReadOnly = true;
                    break;
            }


        }

        public FrmPayExcel()
        {
            InitClientTable();
            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                cmbClientId.Visible = false;
                btn_Import.Enabled = false;
            }

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }


        private void FRMMNUSELIST_Load(object sender, EventArgs e)
        {
            _InitCmb();
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

        private void _InitCmb()
        {
            var ClientIdDataSource = _ClientTable.Select(c => new { c.ClientId, c.Name,c.Code }).ToArray().OrderBy(c=> c.Name);
            


        
            Dictionary<int, string> SearchClients = new Dictionary<int, string>();

            SearchClients.Add(0, "전체");


            foreach (var item in ClientIdDataSource)
            {
                SearchClients.Add(item.ClientId, item.Name);
            }
            cmbClientId.DataSource = new BindingSource(SearchClients, null);
            cmbClientId.DisplayMember = "Value";
            cmbClientId.ValueMember = "Key";


        }
         


        private void Fclear()
        {
            dtp_Sdate.Value = DateTime.Now.AddMonths(-6);
            dtp_Edate.Value = DateTime.Now;
            cmbClientId.SelectedIndex = 0;
            txtSearch.Text = "";
            

        }
        private void btn_Import_Click(object sender, EventArgs e)
        {
            EXCELINSERT_PayExcel _Form = new EXCELINSERT_PayExcel();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
            btn_Search_Click(null, null);
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                if (cmbClientId.SelectedIndex == 0)
                {
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
        @"SELECT Models.PayDate as SalesDate, Models.LastDate as PayDate , Models.LGD_MID as ShopID, Drivers.Name as DriverName, Drivers.BizNo as DriverBizNo, Models.Amount, (Models.Amount - Models.PayAmount) as CommSum, Models.PayAmount as PaySum, Clients.Name as ClientName,Drivers.CarYear
FROM
(SELECT [AccountList].PayDate, [AccountList].LastDate, [AccountList].LGD_MID, Trades.DriverId, [AccountList].Amount, [AccountList].PayAmount, Trades.ClientId
	FROM [Truck].[dbo].[AccountList]
	LEFT JOIN AccLogs ON AccountList.LGD_OID = AccLogs.LGD_OID
	LEFT JOIN Trades ON AccLogs.TradeId = Trades.TradeId
	WHERE Trades.DriverId IS NOT NULL AND Trades.ClientId IS NOT NULL
	GROUP BY [AccountList].PayDate, [AccountList].LastDate, [AccountList].LGD_MID, Trades.DriverId, [AccountList].Amount, [AccountList].PayAmount, Trades.ClientId) AS Models
LEFT JOIN Drivers ON Models.DriverId = Drivers.DriverId
LEFT JOIN Clients ON Models.ClientId = Clients.ClientId
WHERE Models.PayDate >= @BEGIN AND Models.PayDate <= @END 
AND Drivers.CarYear Like '%'+ @CarYear +'%'
ORDER BY SalesDate";
                            _Command.Parameters.AddWithValue("@BEGIN", dtp_Sdate.Text);
                            _Command.Parameters.AddWithValue("@END", dtp_Edate.Text);
                            _Command.Parameters.AddWithValue("@CarYear", txtSearch.Text);
                            using (IDataReader _Reader = _Command.ExecuteReader())
                            {
                                payExcelDataSet.PayExcel.Clear();
                                payExcelDataSet.PayExcel.Load(_Reader);
                            }
                        }
                    });
                }
                else
                {
                    Data.Connection(_Connection =>
                    {
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText =
        @"SELECT Models.PayDate as SalesDate, Models.LastDate as PayDate , Models.LGD_MID as ShopID, Drivers.Name as DriverName, Drivers.BizNo as DriverBizNo, Models.Amount, (Models.Amount - Models.PayAmount) as CommSum, Models.PayAmount as PaySum, Clients.Name as ClientName,Drivers.CarYear
FROM
(SELECT [AccountList].PayDate, [AccountList].LastDate, [AccountList].LGD_MID, Trades.DriverId, [AccountList].Amount, [AccountList].PayAmount, Trades.ClientId
	FROM [Truck].[dbo].[AccountList]
	LEFT JOIN AccLogs ON AccountList.LGD_OID = AccLogs.LGD_OID
	LEFT JOIN Trades ON AccLogs.TradeId = Trades.TradeId
	WHERE Trades.DriverId IS NOT NULL AND Trades.ClientId IS NOT NULL
	GROUP BY [AccountList].PayDate, [AccountList].LastDate, [AccountList].LGD_MID, Trades.DriverId, [AccountList].Amount, [AccountList].PayAmount, Trades.ClientId) AS Models
LEFT JOIN Drivers ON Models.DriverId = Drivers.DriverId
LEFT JOIN Clients ON Models.ClientId = Clients.ClientId
WHERE Models.PayDate >= @BEGIN AND Models.PayDate <= @END AND Models.ClientId = @ClientId
AND Drivers.CarYear Like '%'+ @CarYear +'%'
ORDER BY SalesDate";
                            _Command.Parameters.AddWithValue("@BEGIN", dtp_Sdate.Text);
                            _Command.Parameters.AddWithValue("@END", dtp_Edate.Text);
                            _Command.Parameters.AddWithValue("@ClientId", (int)cmbClientId.SelectedValue);
                            _Command.Parameters.AddWithValue("@CarYear", txtSearch.Text);
                            using (IDataReader _Reader = _Command.ExecuteReader())
                            {
                                payExcelDataSet.PayExcel.Clear();
                                payExcelDataSet.PayExcel.Load(_Reader);
                            }
                        }
                    });
                }
            }
            else
            {
                Data.Connection(_Connection =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText =
    @"SELECT Models.PayDate as SalesDate, Models.LastDate as PayDate , Models.LGD_MID as ShopID, Drivers.Name as DriverName, Drivers.BizNo as DriverBizNo, Models.Amount, (Models.Amount - Models.PayAmount) as CommSum, Models.PayAmount as PaySum, Clients.Name as ClientName,Drivers.CarYear
FROM
(SELECT [AccountList].PayDate, [AccountList].LastDate, [AccountList].LGD_MID, Trades.DriverId, [AccountList].Amount, [AccountList].PayAmount, Trades.ClientId
	FROM [Truck].[dbo].[AccountList]
	LEFT JOIN AccLogs ON AccountList.LGD_OID = AccLogs.LGD_OID
	LEFT JOIN Trades ON AccLogs.TradeId = Trades.TradeId
	WHERE Trades.DriverId IS NOT NULL AND Trades.ClientId IS NOT NULL
	GROUP BY [AccountList].PayDate, [AccountList].LastDate, [AccountList].LGD_MID, Trades.DriverId, [AccountList].Amount, [AccountList].PayAmount, Trades.ClientId) AS Models
LEFT JOIN Drivers ON Models.DriverId = Drivers.DriverId
LEFT JOIN Clients ON Models.ClientId = Clients.ClientId
WHERE Models.PayDate >= @BEGIN AND Models.PayDate <= @END AND Models.ClientId = @ClientId
AND Drivers.CarYear Like '%'+ @CarYear +'%'
ORDER BY SalesDate";
                        _Command.Parameters.AddWithValue("@BEGIN", dtp_Sdate.Text);
                        _Command.Parameters.AddWithValue("@END", dtp_Edate.Text);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.Parameters.AddWithValue("@CarYear", txtSearch.Text);
                        using (IDataReader _Reader = _Command.ExecuteReader())
                        {
                            payExcelDataSet.PayExcel.Clear();
                            payExcelDataSet.PayExcel.Load(_Reader);
                        }
                    }
                });
            }

            //if (LocalUser.Instance.LogInInformation.IsAdmin)
            //{
            //    if (cmbClientId.SelectedIndex == 0)
            //    {

            //        payExcelTableAdapter.FillBySearch(payExcelDataSet.PayExcel, dtp_Sdate.Text, dtp_Edate.Text);
            //    }
            //    else
            //    {
            //        if (cmbClientId.SelectedValue != null)
            //        {
            //            payExcelTableAdapter.FillBySearchClient(payExcelDataSet.PayExcel, dtp_Sdate.Text, dtp_Edate.Text, cmbClientId.SelectedValue.ToString());
            //        }

            //    }
            //}
            //else
            //{
            //    payExcelTableAdapter.FillBySearchClient(payExcelDataSet.PayExcel, dtp_Sdate.Text, dtp_Edate.Text, LocalUser.Instance.LogInInformation.Client.Code);
            //}
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == idx.Index)
            {

                //e.Value = e.RowIndex + 1;
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString();

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
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

            if (di.Exists == false)
            {
                di.Create();
            }
            var fileString = "정산요약_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
            {
                if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            ExcelPackage _Excel = null;
            using (MemoryStream _Stream = new MemoryStream(Properties.Resources.정산요약))
            {
                _Stream.Seek(0, SeekOrigin.Begin);
                _Excel = new ExcelPackage(_Stream);
            }
            var _Sheet = _Excel.Workbook.Worksheets[1];
            var RowIndex = 2;
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                var _Model = ((DataRowView)newDGV1.Rows[i].DataBoundItem).Row as PayExcelDataSet.PayExcelRow;
               
                if (_Model.PayDate == "")
                { _Sheet.Cells[RowIndex, 1].Value = "합계"; }
                else
                {
                    //_Sheet.Cells[RowIndex, 1].Value = (i + 1).ToString();
                    _Sheet.Cells[RowIndex, 1].Value = (newDGV1.Rows.Count - i - 1);
                }
                _Sheet.Cells[RowIndex, 1].Value = _Model.PayDate;
                _Sheet.Cells[RowIndex, 2].Value = _Model.SalesDate;
                _Sheet.Cells[RowIndex, 3].Value = _Model.ShopID;
                _Sheet.Cells[RowIndex, 4].Value = _Model.DriverName;
                _Sheet.Cells[RowIndex, 5].Value = _Model.DriverBizNo;
                _Sheet.Cells[RowIndex, 6].Value = _Model.Amount;
                _Sheet.Cells[RowIndex, 7].Value = _Model.CommSum;
                _Sheet.Cells[RowIndex, 8].Value = _Model.PaySum;
                _Sheet.Cells[RowIndex, 9].Value = _Model.ClientName;

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

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("조회된 내용을 삭제하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                    "Delete PayExcel  WHERE SalesDate >= @Sdate AND SalesDate <= @Edate ";

                cmd.Parameters.AddWithValue("@Sdate", dtp_Sdate.Text);
                cmd.Parameters.AddWithValue("@Edate", dtp_Edate.Text);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

            try
            {
                MessageBox.Show("삭제 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { btn_Search_Click(null, null); }

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }
    }
}
