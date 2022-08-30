using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class Dialog_BankResult : Form
    {
        int SbumId = 0;
        public Dialog_BankResult()
        {
            InitializeComponent();
         

            dtpStart.Value = DateTime.Now.AddMonths(-1).AddDays(1);
            dtpEnd.Value = DateTime.Now;

            ShowList();
        }

        private void Dialog_BankResult_Load(object sender, EventArgs e)
        {
            
            // TODO: 이 코드는 데이터를 'bankDataSet.BankUploadDetail' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.bankUploadDetailTableAdapter.Fill(this.bankDataSet.BankUploadDetail);
            // TODO: 이 코드는 데이터를 'bankDataSet.BankUploadMaster' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.bankUploadMasterTableAdapter.Fill(this.bankDataSet.BankUploadMaster);

            btn_Search_Click(null, null);
        }
        private string GetSelectCommand()
        {




            String SelectCommandText =
                    @" SELECT  BUMId, RequestDate, FileName, TotalCount, SuccessCount, FailedCount, TotalAmount, SuccessAmount, FailAmount, ClientId FROM   BankUploadMaster ";
                   


            return SelectCommandText;
        }

        private String GetDetailSelectCommand()
        {
            return @"SELECT  BUDId, BUMId, ExcelIdx, BankName, BankAccount, BankAccountName, Amount, Result, Remark, CreateDate FROM     BankUploadDetail ";


        }


        private void DataLoad()
        {
           bankDataSet.BankUploadMaster.Clear();
            //clientDataSet.SubClients.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                   
                    WhereStringList.Add($" RequestDate >= @Begin AND RequestDate < @End");
                    _Command.Parameters.AddWithValue("@Begin", dtpStart.Value.Date);
                    _Command.Parameters.AddWithValue("@End", dtpEnd.Value.Date.AddDays(1));

                    WhereStringList.Add($" ClientId = " + LocalUser.Instance.LogInInformation.ClientId + "");

                    
                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText + " order by RequestDate desc";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        bankDataSet.BankUploadMaster.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }


        private void DetailDataLoad()
        {
            bankDataSet.BankUploadDetail.Clear();
            //clientDataSet.SubClients.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetDetailSelectCommand();

                    List<String> WhereStringList = new List<string>();

                    WhereStringList.Add($" BUMId = @BUMId ");
                    _Command.Parameters.AddWithValue("@BUMId", SbumId);

                    if (cmb_Search.Text == "은행명")
                    {
                        WhereStringList.Add(string.Format("BankName LIKE '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "계좌번호")
                    {
                        WhereStringList.Add(string.Format("BankAccount LIKE '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "예금주")
                    {
                        WhereStringList.Add(string.Format("BankAccountName Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "적용결과")
                    {
                        WhereStringList.Add(string.Format("Result  =   '{0}'", txt_Search.Text));
                    }

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText + " order by BudId desc";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        bankDataSet.BankUploadDetail.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }


        private void btn_Search_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void newDGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //if (e.RowIndex < 0)
            //    return;
          
            //var Selected = ((DataRowView)bankUploadMasterBindingSource.Current).Row as BankDataSet.BankUploadMasterRow;
            //ShowDetail(Selected.BUMId,Selected.FileName,Selected.RequestDate);
        }
        private void ShowDetail(int IbumId,string IFileName,DateTime IRequestDate)
        {
            pnMaster.Visible = false;
            pnDeatail.Visible = true;
            LayoutRoot.RowStyles[0].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[0].Height = 0F;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[1].Height = 100F;

            SbumId = IbumId;
            lblFileName.Text ="파일명 : "+ IFileName;
            lblRequestDate.Text = "일자 : " + IRequestDate.ToString("d");
            cmb_Search.SelectedIndex = 0;
            DetailDataLoad();
            //bankUploadMasterBindingSource.



        }

        private void ShowList()
        {
            pnMaster.Visible = true;
            pnDeatail.Visible = false;

            LayoutRoot.RowStyles[0].SizeType = SizeType.Percent;
            LayoutRoot.RowStyles[0].Height = 100F;
            LayoutRoot.RowStyles[1].SizeType = SizeType.Absolute;
            LayoutRoot.RowStyles[1].Height = 0F;

            btn_Search_Click(null, null);
        }
        private void btn_List_Click(object sender, EventArgs e)
        {
            ShowList();
        }

        private void btnBankUpdate_Click(object sender, EventArgs e)
        {
            Dialog_BankUpload _Dialog_BankUpload = new Dialog_BankUpload();

            if (_Dialog_BankUpload.ShowDialog() == DialogResult.OK)
            {

                btn_Search_Click(null, null);
            }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as BankDataSet.BankUploadMasterRow;

            if (newDGV1.Columns[e.ColumnIndex] == ColumnNo1)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0");

            }
            else if (newDGV1.Columns[e.ColumnIndex] == requestDateDataGridViewTextBoxColumn)
            {
                e.Value = Selected.RequestDate.ToString("d");

            }

        }

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)newDGV2.Rows[e.RowIndex].DataBoundItem).Row as BankDataSet.BankUploadDetailRow;

            if (newDGV2.Columns[e.ColumnIndex] == ColNo)
            {
                e.Value = (newDGV2.Rows.Count - e.RowIndex).ToString("N0");

            }
        }

        private void btnInfo2_Click(object sender, EventArgs e)
        {
            DetailDataLoad();
        }

        private void cmb_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btnInfo2_Click(null, null);
        }

        private void newDGV1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)bankUploadMasterBindingSource.Current).Row as BankDataSet.BankUploadMasterRow;
            ShowDetail(Selected.BUMId, Selected.FileName, Selected.RequestDate);
        }

        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;
        string FolderPath = string.Empty;

        private void btnSave_Click(object sender, EventArgs e)
        {

            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYBANKNEWRESULT);

            title = "적용상세내역";

            ieExcel = Properties.Resources.적용상세내역;

            if (di.Exists == false)
            {
                di.Create();
            }
            fileString = string.Format("적용상세내역") + DateTime.Now.ToString("yyyyMMdd");
            var FileName = System.IO.Path.Combine(di.FullName, fileString);
            FileInfo file = new FileInfo(FileName + ".xls");
           


            if (newDGV2.RowCount == 0)
            {
                MessageBox.Show("내보낼 적용상세내역 자료가 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (newDGV2.RowCount > 0)
            {
                if (file.Exists)
                {
                    if (MessageBox.Show($"{fileString} 파일이 이미 존재 합니다. 이 파일을 덮어쓰시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        file.Delete();

                    }
                }


                pnProgress.Visible = true;
                bar.Value = 0;
                Thread t = new Thread(new ThreadStart(() =>
                {
                    newDGV2.ExportExistExcel2(title, fileString, bar, true, ieExcel, 2, LocalUser.Instance.PersonalOption.MYBANKNEWRESULT);
                    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));



                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            else
            {
                MessageBox.Show("엑셀로 내보낼 실패내역 없습니다. 먼저 원하시는 자료를 검색하신 후, 진행해주십시오",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void Dialog_BankResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK; 
        }
    }
}
