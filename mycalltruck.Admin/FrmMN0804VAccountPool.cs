using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0804VAccountPool : Form
    {
        public FrmMN0804VAccountPool()
        {
            InitializeComponent();
        }

        private void mDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            (int VAccountPoolId, String VAccountNo, String VBankName, String ClientCode, String ClientName, String DriverCode, String DriverName, int DriverId) = DataListSource[e.RowIndex];
            if (e.ColumnIndex == ColumnNo.Index)
            {
                e.Value = (mDataGridView.RowCount - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == ColumnVAccountNo.Index)
                e.Value = VAccountNo;
            else if (e.ColumnIndex == ColumnVBankName.Index)
                e.Value = VBankName;
            else if (e.ColumnIndex == ColumnClientCode.Index)
                e.Value = ClientCode;
            else if (e.ColumnIndex == ColumnClientName.Index)
                e.Value = ClientName;
            else if (e.ColumnIndex == ColumnDriverCode.Index)
                e.Value = DriverCode;
            else if (e.ColumnIndex == ColumnDriverName.Index)
                e.Value = DriverName;
        }

        private void FrmMN0804VAccountPool_Load(object sender, EventArgs e)
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                Close();
                return;
            }
            Search();
        }
        BindingList<(int VAccountPoolId, String VAccountNo, String VBankName, String ClientCode, String ClientName, String DriverCode, String DriverName, int DriverId)> DataListSource = new BindingList<(int , string, string, string, string, string, string, int)>();

        private void Search()
        {
            DataListSource.Clear();
            mDataGridView.AutoGenerateColumns = false;
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        "SELECT VAccountPoolId, VAccountPools.VAccount, VAccountPools.VBankName, ISNULL(Clients.Code, N'') as ClientCode, ISNULL(Clients.Name, N'') as ClientName, ISNULL(Drivers.Code, N'') as DriverCode, ISNULL(Drivers.Name, N'') as DriverName, ISNULL(VAccountPools.DriverId, 0) as DriverId " +
                        "FROM VAccountPools LEFT JOIN Clients ON VAccountPools.ClientId = Clients.ClientId LEFT JOIN Drivers ON VAccountPools.DriverId = Drivers.DriverId " +
                        "ORDER BY VAccountPoolId DESC";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            DataListSource.Add((_Reader.GetInt32(0), _Reader.GetString(1), _Reader.GetString(2), _Reader.GetString(3), _Reader.GetString(4), _Reader.GetString(5), _Reader.GetString(6), _Reader.GetInt32(7)));
                        }
                    }
                }
            });
            mDataGridView.DataSource = DataListSource;
            DataListSource.ResetBindings();
        }

        private void SaveForm_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog
            {
                DefaultExt = "xlsx",
                FileName = "가상계좌번호입력.xlsx"
            };
            if(d.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(d.FileName, Properties.Resources.가상계좌번호입력);
                Process.Start(d.FileName);
            }
        }

        private async void ImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog
            {
                DefaultExt = "xlsx",
                FileName = "가상계좌번호입력.xlsx"
            };
            if(d.ShowDialog() == DialogResult.OK)
            {
                int TotalCount = 0;
                int AddedCount = 0;
                Task SqlTask = new Task(() =>
                {
                    List<(string VAccount, string VBankName)> ExcelList = new List<(string VAccount, string VBankName)>();
                    ExcelPackage _Excel = null;
                    try
                    {
                        _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                        return;
                    }
                    if (_Excel.Workbook.Worksheets.Count < 1)
                    {
                        MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                        return;
                    }
                    var _Sheet = _Excel.Workbook.Worksheets[1];
                    int RowIndex = 1;
                    while (true)
                    {
                        RowIndex++;
                        if (String.IsNullOrEmpty(_Sheet.Cells[RowIndex, 1].Text) || String.IsNullOrEmpty(_Sheet.Cells[RowIndex, 2].Text))
                            break;
                        TotalCount++;
                        if (!DataListSource.Any(c => c.VAccountNo == _Sheet.Cells[RowIndex, 1].Text.Trim()))
                        {
                            ExcelList.Add((_Sheet.Cells[RowIndex, 1].Text.Trim(), _Sheet.Cells[RowIndex, 2].Text.Trim()));
                            AddedCount++;
                        }
                    }
                    Data.Connection((_Connection) =>
                    {
                        DateTime Start = DateTime.Now;
                        using (SqlTransaction _Transaction = _Connection.BeginTransaction())
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.Transaction = _Transaction;
                                _Command.CommandText
                                = "INSERT INTO VAccountPools (VAccount, VBankName) VALUES (@VAccount, @VBankName)";
                                _Command.Parameters.Add("@VAccount", SqlDbType.NVarChar);
                                _Command.Parameters.Add("@VBankName", SqlDbType.NVarChar);
                                foreach (var (VAccount, VBankName) in ExcelList)
                                {
                                    _Command.Parameters["@VAccount"].Value = VAccount;
                                    _Command.Parameters["@VBankName"].Value = VBankName;
                                    _Command.ExecuteNonQuery();
                                }
                            }
                            _Transaction.Commit();
                        }
                        Debug.WriteLine((DateTime.Now - Start).TotalSeconds);
                    });
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"{TotalCount}건의 정보 중, 중복 정보를 제외한 {AddedCount}건의 정보를 입력하였습니다.");
                        Search();
                        this.Cursor = Cursors.Arrow;
                    }));

                });
                this.Cursor = Cursors.WaitCursor;
                SqlTask.Start();
            }
        }

        private void AllSelect_Click(object sender, EventArgs e)
        {
            if(AllSelect.CheckState == CheckState.Checked)
            {
                foreach (DataGridViewRow Row in mDataGridView.Rows)
                {
                    Row.Cells[ColumnSelect.Index].Value = true;
                }
            }
            else if(AllSelect.CheckState == CheckState.Unchecked)
            {
                foreach (DataGridViewRow Row in mDataGridView.Rows)
                {
                    Row.Cells[ColumnSelect.Index].Value = false;
                }
            }
        }

        private void mDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            mDataGridView.EndEdit();
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnSelect.Index)
            {
                if (mDataGridView.Rows.Cast<DataGridViewRow>().Any(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                {
                    if (mDataGridView.Rows.Cast<DataGridViewRow>().All(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                    {
                        AllSelect.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        AllSelect.CheckState = CheckState.Indeterminate;
                    }
                }
                else
                {
                    AllSelect.CheckState = CheckState.Unchecked;
                }
            }

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말 삭제 하시겠습니까?", "가상계좌 삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {


                int Count = 0;
                Data.Connection((_Connection) =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "UPDATE Drivers SET VAccount = null , VBankName = null WHERE DriverId = @DriverId; DELETE FROM VAccountPools WHERE VAccountPoolId = @VAccountPoolId";
                        _Command.Parameters.Add("@DriverId", SqlDbType.Int);
                        _Command.Parameters.Add("@VAccountPoolId", SqlDbType.Int);
                        foreach (DataGridViewRow Row in mDataGridView.Rows.Cast<DataGridViewRow>().Where(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                        {
                            (int VAccountPoolId, String VAccountNo, String VBankName, String ClientCode, String ClientName, String DriverCode, String DriverName, int DriverId) = DataListSource[Row.Index];
                            _Command.Parameters["@DriverId"].Value = DriverId;
                            _Command.Parameters["@VAccountPoolId"].Value = VAccountPoolId;
                            if (_Command.ExecuteNonQuery() > 0)
                                Count++;
                        }

                        _Command.CommandText = "DELETE vacs_vact WHERE acct_no = @acct_no AND bank_cd = @bank_cd";
                        _Command.Parameters.Add("@acct_no", SqlDbType.VarChar);
                        _Command.Parameters.Add("@bank_cd", SqlDbType.VarChar);
                        foreach (DataGridViewRow Row in mDataGridView.Rows.Cast<DataGridViewRow>().Where(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                        {
                            (int VAccountPoolId, String VAccountNo, String VBankName, String ClientCode, String ClientName, String DriverCode, String DriverName, int DriverId) = DataListSource[Row.Index];

                            string bank_cd = "";
                            switch (VBankName)
                            {
                                case "기업":
                                    bank_cd = "003";
                                    break;
                                case "국민":
                                    bank_cd = "004";
                                    break;
                                case "농협":
                                    bank_cd = "011";
                                    break;
                                case "우리":
                                    bank_cd = "020";
                                    break;
                                case "신한":
                                    bank_cd = "088";
                                    break;

                            }
                            _Command.Parameters["@acct_no"].Value = VAccountNo;
                            _Command.Parameters["@bank_cd"].Value = bank_cd;
                            _Command.ExecuteNonQuery();
                            //    Count++;
                        }
                    }
                });
                MessageBox.Show($"{Count}건의 정보가 삭제되었습니다.");
                Search();
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말 해지 하시겠습니까?", "가상계좌 해지", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int Count = 0;
                Data.Connection((_Connection) =>
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "UPDATE Drivers SET VAccount = null , VBankName = null WHERE DriverId = @DriverId; UPDATE VAccountPools SET DriverId = null, ClientId = null WHERE VAccountPoolId = @VAccountPoolId AND @DriverId is not null";
                        _Command.Parameters.Add("@DriverId", SqlDbType.Int);
                        _Command.Parameters.Add("@VAccountPoolId", SqlDbType.Int);
                        foreach (DataGridViewRow Row in mDataGridView.Rows.Cast<DataGridViewRow>().Where(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                        {
                            (int VAccountPoolId, String VAccountNo, String VBankName, String ClientCode, String ClientName, String DriverCode, String DriverName, int DriverId) = DataListSource[Row.Index];
                            _Command.Parameters["@DriverId"].Value = DriverId;
                            _Command.Parameters["@VAccountPoolId"].Value = VAccountPoolId;
                            if (_Command.ExecuteNonQuery() > 0)
                                Count++;
                        }

                        _Command.CommandText = "DELETE vacs_vact WHERE acct_no = @acct_no AND bank_cd = @bank_cd";
                        _Command.Parameters.Add("@acct_no", SqlDbType.VarChar);
                        _Command.Parameters.Add("@bank_cd", SqlDbType.VarChar);
                        foreach (DataGridViewRow Row in mDataGridView.Rows.Cast<DataGridViewRow>().Where(c => ((bool?)(c.Cells[ColumnSelect.Index] as DataGridViewCheckBoxCell).Value) == true))
                        {
                            (int VAccountPoolId, String VAccountNo, String VBankName, String ClientCode, String ClientName, String DriverCode, String DriverName, int DriverId) = DataListSource[Row.Index];

                            string bank_cd = "";
                            switch (VBankName)
                            {
                                case "기업":
                                    bank_cd = "003";
                                    break;
                                case "국민":
                                    bank_cd = "004";
                                    break;
                                case "농협":
                                    bank_cd = "011";
                                    break;
                                case "우리":
                                    bank_cd = "020";
                                    break;
                                case "신한":
                                    bank_cd = "088";
                                    break;

                            }
                            _Command.Parameters["@acct_no"].Value = VAccountNo;
                            _Command.Parameters["@bank_cd"].Value = bank_cd;
                            _Command.ExecuteNonQuery();
                            //    Count++;
                        }

                    }
                });
                MessageBox.Show($"{Count}건의 정보가 해제되었습니다.");
                Search();
            }
        }
    }
}
