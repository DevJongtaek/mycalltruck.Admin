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
    public partial class FRMMNUSELIST : Form
    {
        public FRMMNUSELIST()
        {
            InitClientTable();
            InitializeComponent();
        }


        private void FRMMNUSELIST_Load(object sender, EventArgs e)
        {
            
            // TODO: 이 코드는 데이터를 'useListDataSet.AccountList' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.accountListTableAdapter.Fill(this.useListDataSet.AccountList);

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
            var ClientIdDataSource = _ClientTable.Select(c => new { c.ClientId, c.Name }).ToArray().OrderBy(c=> c.Name);
            


        
            Dictionary<string, string> SearchClients = new Dictionary<string, string>();

            SearchClients.Add("%", "전체");


            foreach (var item in ClientIdDataSource)
            {
                SearchClients.Add(item.ClientId.ToString(), item.Name);
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
            EXCELINSERT_USELIST _Form = new EXCELINSERT_USELIST();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

                );
            btn_Search_Click(null, null);
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
           
            if (cmbClientId.SelectedIndex == 0)
            {
                accountListTableAdapter.FillBySearch(useListDataSet.AccountList, dtp_Sdate.Text, dtp_Edate.Text,txtSearch.Text);
            }
            else
            {
                if (cmbClientId.SelectedValue != null)
                {
                    accountListTableAdapter.FillBySearchClient(useListDataSet.AccountList, dtp_Sdate.Text, dtp_Edate.Text, Convert.ToInt32(cmbClientId.SelectedValue.ToString()), txtSearch.Text);
                }
               
            }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            //{
            //    newDGV1[e.ColumnIndex, e.RowIndex].Value = (newDGV1.Rows.Count - e.RowIndex).ToString();
            //}

            // if (e.ColumnIndex == idx.Index)
           


            if (e.ColumnIndex == idx.Index)
            {
                if (e.RowIndex == newDGV1.RowCount - 1)
                {
                    e.Value = "합계";
                }
                else
                {
                    //e.Value = e.RowIndex + 1;
                    e.Value = (newDGV1.Rows.Count - e.RowIndex-1).ToString();
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
            var fileString = "고객별사용내역_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
