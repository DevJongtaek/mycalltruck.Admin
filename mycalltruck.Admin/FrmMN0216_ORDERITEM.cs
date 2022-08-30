using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0216_ORDERITEM : Form
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

                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;




                    dataGridView1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }

        int GridIndex = 0;
        public FrmMN0216_ORDERITEM()
        {
            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }

        private void FrmMN0216_ORDERITEM_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'orderItemDataSet.OrderItem' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.orderItemsTableAdapter.Fill(this.orderItemDataSet.OrderItems);

            cmb_Search.SelectedIndex = 0;

            btn_Inew_Click(null, null);

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_New.Enabled = false;
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0216_ORDERITEM_ADD _Form = new FrmMN0216_ORDERITEM_ADD();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }
        private String GetSelectCommand()
        {
            return @"SELECT OrderItemId, ItemCode, ItemName, CreateDate, ClientId, Remark from OrderItems ";


        }
        private void LoadTable()
        {
            orderItemDataSet.OrderItems.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                    WhereStringList.Add("ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    if (cmb_Search.Text == "품목코드")
                    {
                        WhereStringList.Add(string.Format("ItemCode Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "품목명")
                    {
                        WhereStringList.Add(string.Format("ItemName Like '%{0}%'", txt_Search.Text));
                    }
                  


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by ItemCode  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        orderItemDataSet.OrderItems.Load(_Reader);

                    }
                }
                _Connection.Close();
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();

            if (ItemCode.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(ItemCode, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }

           else if (ItemName.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(ItemName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return;

            }


           
            UpdateDB();
        }
        private void UpdateDB()
        {
            try
            {

                
                orderItemBindingSource.EndEdit();
                var Row = ((DataRowView)orderItemBindingSource.Current).Row as OrderItemDataSet.OrderItemsRow;

                orderItemsTableAdapter.Update(Row);
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "품목코드", 1), "품목코드 수정 성공");

                if (dataGridView1.RowCount > 1)
                {
                    GridIndex = orderItemBindingSource.Position;
                    btn_Search_Click(null, null);
                    dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                }
                else
                {
                    btn_Search_Click(null, null);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("작업 중 오류가 발생하였습니다.", "화주담당자 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (dataGridView1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }


                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "품목코드", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }


                orderItemsTableAdapter.Update(orderItemDataSet.OrderItems);

            }
           
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "품목코드", 1), "품목코드 삭제 성공");
            btn_Search_Click(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Clear();
            btn_Search_Click(null, null);
        }

        private void orderItemBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (orderItemBindingSource.Current == null)
            {
                CreateDate.Text = "";
                return;
            }
            var Selected = ((DataRowView)orderItemBindingSource.Current).Row as OrderItemDataSet.OrderItemsRow;
            if (Selected != null)
            {

                CreateDate.Text = Selected.CreateDate.ToString("d").Replace("-", "/");

            }
            else
            {
                CreateDate.Text = "";
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            LoadTable();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as OrderItemDataSet.OrderItemsRow;


            if (dataGridView1.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }

            else if (e.ColumnIndex == createDateDataGridViewTextBoxColumn.Index)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
        }
    }
}
