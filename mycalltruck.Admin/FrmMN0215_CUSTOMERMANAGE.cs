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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0215_CUSTOMERMANAGE : Form
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
        public FrmMN0215_CUSTOMERMANAGE()
        {
            InitializeComponent();
            _InitCmb();
            _InitCmbSearch();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }

        private void FrmMN0208_DRIVERADDMANAGE_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'customerManagerDataSet.CustomerManager' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.customerManagerTableAdapter.Fill(this.customerManagerDataSet.CustomerManager);
          
            btn_Search_Click(null, null);

            if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_New.Enabled = false;
            }
        }
        private void _InitCmb()
        {

           


        }

        private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
        private void _InitCmbSearch()
        {
            DataGridViewColumn[] cols = new DataGridViewColumn[]{managerCodeDataGridViewTextBoxColumn,managerNameDataGridViewTextBoxColumn,managerPhoneNoDataGridViewTextBoxColumn,managerMobileNoDataGridViewTextBoxColumn
             };
            cmb_Search.Items.Clear();
            DicSearch.Clear();
            cmb_Search.Items.Add("전체");

            foreach (var item in cols)
            {

                cmb_Search.Items.Add(item.HeaderText);
                if (item.DataPropertyName == null || item.DataPropertyName == "")
                {
                    DicSearch.Add(item.HeaderText, "'" + item.Name);
                }
                else
                {
                    DicSearch.Add(item.HeaderText, item.DataPropertyName);
                }
            }
            if (cmb_Search.Items.Count > 0) cmb_Search.SelectedIndex = 0;

        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0215_CUSTOMERMANAGE_ADD _Form = new FrmMN0215_CUSTOMERMANAGE_ADD();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void customerManagerBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            
            
              if (customerManagerBindingSource.Current == null)
                return;
            
              var Selected = ((DataRowView)customerManagerBindingSource.Current).Row as CMDataSet.DriverAddRow;
              if (Selected != null)
              {
                
                  
                 
                   txt_CreateDate.Text = Selected.WriteDate.ToString("d").Replace("-","/");

         
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CustomerManagerDataSet.CustomerManagerRow;

           
             if (dataGridView1.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }

            else if (e.ColumnIndex == createDateDataGridViewTextBoxColumn.Index)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");
            }
        }
        private String GetSelectCommand()
        {
            return @"SELECT  ManagerId, ManagerCode, ManagerName, ManagerPhoneNo, ManagerMobileNo, CreateDate, ClientId FROM     CustomerManager ";


        }
        private void LoadTable()
        {
            customerManagerDataSet.CustomerManager.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();
                  
                   
                    WhereStringList.Add("ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        
                  
                    if (cmb_Search.Text == "코드")
                    {
                        WhereStringList.Add(string.Format("ManagerCode Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "화주담당자")
                    {
                        WhereStringList.Add(string.Format("ManagerName Like '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "전화번호")
                    {
                        WhereStringList.Add(string.Format("ManagerPhoneNo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmb_Search.Text == "핸드폰번호")
                    {
                        WhereStringList.Add(string.Format("ManagerMobileNo Like  '%{0}%'", txt_Search.Text));
                    }
                    
                   

                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }

                   
                    SelectCommandText += " Order by ManagerCode  Desc ";
                  
                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        customerManagerDataSet.CustomerManager.Load(_Reader);

                    }
                }
                _Connection.Close();
            }
        }
       
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();

            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
          
      


            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;

            }


            if (txt_PhoneNo.Text.Replace("-", "").Length < 9)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;
            }


            if (txt_MobileNo.Text.Replace("-", "").Length < 10)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return ;
            }
            UpdateDB();
        }
        private void UpdateDB()
        {
            try
            {

                customerManagerBindingSource.EndEdit();
                var Row = ((DataRowView)customerManagerBindingSource.Current).Row as CMDataSet.DriverAddRow;

                customerManagerTableAdapter.Update(Row);
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "화주담당자", 1), "화주담당자 수정 성공");

                if (dataGridView1.RowCount > 1)
                {
                    GridIndex = customerManagerBindingSource.Position;
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
            

                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "화주담당자", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
                

                customerManagerTableAdapter.Update(customerManagerDataSet.CustomerManager);
               
            }
            //Up_Status = "Delete";
            //int _rows = UpdateDB(Up_Status);
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "화주담당자", 1), "화주담당자 삭제 성공");
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
            txt_Search.Text = string.Empty;

            LoadTable();
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            LoadTable();
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      
    }
}
