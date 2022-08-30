using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
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
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class FrmClientUser : Form
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
        public FrmClientUser()
        {
            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            _InitCmb();
            txt_CreateTime.DataBindings[0].Format += txt_CreateTimeBinding_Format;
            LocalUser.Instance.LogInInformation.LoadClient();

            InitSubClientTable();
        }


        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        private void _InitCmb()
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                SubClientIdDictionary.Add(0, "본점");
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _AllowSubCommand = _Connection.CreateCommand())
                    {
                        _AllowSubCommand.CommandText = "SELECT AllowSub FROM Clients WHERE ClientId = @ClientId";
                        _AllowSubCommand.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        var _AllowSub = (bool)_AllowSubCommand.ExecuteScalar();
                        if (_AllowSub)
                        {
                            using (SqlCommand _Command = _Connection.CreateCommand())
                            {
                                _Command.CommandText = "SELECT Name, SubClientId FROM SubClients WHERE ClientId = @ClientId";
                                _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                                using (SqlDataReader _Reader = _Command.ExecuteReader())
                                {
                                    while (_Reader.Read())
                                    {
                                        SubClientIdDictionary.Add(_Reader.GetInt32(1), _Reader.GetString(0));
                                    }
                                }
                            }
                            cmbSubClientId.DataSource = SubClientIdDictionary.ToList();
                            cmbSubClientId.ValueMember = "Key";
                            cmbSubClientId.DisplayMember = "Value";
                        }
                        else
                        {
                            lblSubClientId.Visible = false;
                            cmbSubClientId.Visible = false;
                            SubClientId.Visible = false;
                            lbl_IsAgent.Visible = false;
                            IsAgent.Visible = false;
                        }
                    }
                    _Connection.Close();
                }
            }
            else
            {
                SubClientId.Visible = false;
            }
            Dictionary<int, string> DCustomer = new Dictionary<int, string>();
            
            customersNewTableAdapter.Fill(clientDataSet.CustomersNew);
            
            var cmbCustomerMIdDataSource = clientDataSet.CustomersNew.Where(c=> c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.SangHo, c.CustomerId }).OrderBy(c => c.SangHo).ToArray();


            DCustomer.Add(0, "전체");


            foreach (var item in cmbCustomerMIdDataSource)
            {
                DCustomer.Add(item.CustomerId, item.SangHo);
            }


            cmb_Customer.DataSource = new BindingSource(DCustomer, null);
            cmb_Customer.DisplayMember = "Value";
            cmb_Customer.ValueMember = "Key";
            cmb_Customer.SelectedValue = 0;

           

            cmb_Search.SelectedIndex = 0;
            cmb_ClientSerach.SelectedIndex = 0;


        }

        private void FrmClientUser_Load(object sender, EventArgs e)
        {
            if(!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                this.clientUsersTableAdapter.Fill(this.clientDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);
                this.dataGridView1.Columns[1].Visible = false;
                this.dataGridView1.Columns[2].Visible = false;
                btn_New.Enabled = true;
                btnUpdate.Enabled = true;
                btnCurrentDelete.Enabled = true;
                cmb_ClientSerach.Visible = false;
                txt_ClientSearch.Visible = false;
                if(LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {
                    Close();
                }
            }
            else
            {
                btn_New.Enabled = false;
                btnCurrentDelete.Enabled = true;
                btnUpdate.Enabled = true;
                this.clientUsersTableAdapter.FillForAdmin(this.clientDataSet.ClientUsers);
                this.dataGridView1.Columns[1].Visible = true;
                this.dataGridView1.Columns[2].Visible = true;
                cmb_ClientSerach.Visible = true;
                txt_ClientSearch.Visible = true;
            }

            btn_Search_Click(null, null);
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmClientUser_Add _Form = new FrmClientUser_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
            btn_Search_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String r = String.Empty;
            err.Clear();
            var Selected = ((DataRowView)clientUsersBindingSource.Current).Row as ClientDataSet.ClientUsersRow;

            if (!Selected.IsRegister)
            {
                if (String.IsNullOrEmpty(txt_Part.Text))
                {
                    r += "부서명을 입력해주십시오." + Environment.NewLine;
                    err.SetError(txt_Part, "부서명을 입력해주십시오.");
                }
                if (String.IsNullOrEmpty(txt_Name.Text))
                {
                    r += "사용자명을 입력해주십시오." + Environment.NewLine;
                    err.SetError(txt_Name, "사용자명을 입력해주십시오.");
                }
            }
            if (String.IsNullOrEmpty(txt_LoginId.Text))
            {
                r += "아이디를 입력해주십시오." + Environment.NewLine;
                err.SetError(txt_LoginId, "아이디를 입력해주십시오.");
            }
            if (String.IsNullOrEmpty(txt_Password.Text))
            {
                r += "비밀번호를 입력해주십시오." + Environment.NewLine;
                err.SetError(txt_Password, "비밀번호를 입력해주십시오.");
            }
            //if(!String.IsNullOrEmpty(txt_LoginId.Text) && !String.IsNullOrEmpty(txt_Password.Text))
            //{
            //    var Query1 =
            //        "Select Count(*) From ClientUsers Where LoginId = @LoginId AND Password = @Password AND ClientUserId != @ClientUserId";
            //    var Query2 =
            //        "Select Count(*) From Clients Where LoginId = @LoginId AND Password = @Password";

            //    bool IsDuplicated = false;
            //    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            //    {
            //        cn.Open();

            //        SqlCommand cmd1 = cn.CreateCommand();
            //        cmd1.CommandText = Query1;
            //        cmd1.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
            //        cmd1.Parameters.AddWithValue("@Password", txt_Password.Text);
            //        if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
            //        {
            //            IsDuplicated = true;
            //        }
            //        if (!IsDuplicated)
            //        {
            //            SqlCommand cmd2 = cn.CreateCommand();
            //            cmd2.CommandText = Query2;
            //            cmd2.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
            //            cmd2.Parameters.AddWithValue("@Password", txt_Password.Text);
            //            if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
            //            {
            //                IsDuplicated = true;
            //            }
            //        }
            //        cn.Close();
            //    }
            //    if (IsDuplicated)
            //    {
            //        r += "아이디/비밀번호를 확인해주십시오. 아이디/비밀번호는 중복될 수 없습니다." + Environment.NewLine;
            //        err.SetError(txt_LoginId, "아이디를 입력해주십시오.");
            //    }
            //}
            r = r.Trim();

            if(!String.IsNullOrEmpty(r))
            {
                MessageBox.Show(r, "유효성 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var UP_Status = "Update";
            int _rows = UpdateDB(UP_Status);
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();
            var Selected = ((DataRowView)clientUsersBindingSource.Current).Row as ClientDataSet.ClientUsersRow;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (Selected.Name == "슈퍼관리자")
                {
                    MessageBox.Show("슈퍼관리자는 삭제할 수 없습니다", "삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                    {
                        if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                    }


                    if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "아이디", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    foreach (DataGridViewRow row in deleteRows)
                    {
                        dataGridView1.Rows.Remove(row);
                    }
                    clientUsersTableAdapter.Update(clientDataSet);
                }
            }

            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "아이디", 1), "아이디 삭제 성공");
            btn_Search_Click(null, null);
            //var UP_Status = "Delete";
            //int _rows = UpdateDB(UP_Status);
            //MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "영업딜러", deleteRows.Count));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            InitSubClientTable();

            _Search();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            cmb_ClientSerach.SelectedIndex = 0;
            txt_ClientSearch.Text = string.Empty;
            txt_Search.Text = string.Empty;
            _Search();
        }


        private int UpdateDB(string Status)
        {
            int _rows = 0;
            try
            {

                var Selected = ((DataRowView)clientUsersBindingSource.Current).Row as ClientDataSet.ClientUsersRow;

                if (Selected.Name =="슈퍼관리자" )
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {


                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();



                        cmd.CommandText =
                           "UPDATE Clients SET Password = @Password WHERE ClientId = @ClientId ";




                        cmd.Parameters.AddWithValue("@Password", txt_Password.Text);
                        cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);





                        cmd.ExecuteNonQuery();
                        cn.Close();


                    }
                }
                else
                {
                    if (cmbSubClientId.SelectedIndex <= 0)
                        Selected.SetSubClientIdNull();
                    else
                        Selected.SubClientId = (int)cmbSubClientId.SelectedValue;
                    if(Selected.SubClientId > 0 && IsAgent.Checked)
                    {
                        Selected.IsAgent = true;
                    }
                    else
                    {
                        Selected.IsAgent = false;
                    }

                    Selected.CustomerName = cmb_Customer.Text;
                   // Selected.CustomerName = cmb_Customer.Text;
                    clientUsersBindingSource.EndEdit();
                    //clientUsersTableAdapter.Update(Selected);

                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {


                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();



                        cmd.CommandText =
                           "UPDATE ClientUsers" +
                           " SET LoginId = '" + Selected.LoginId + "'" +
                           " ,PassWord ='" + Selected.Password + "'" +
                           " ,Part ='" + Selected.Part + "'" +
                           " ,Name ='" + Selected.Name + "'" +
                           " ,Rank ='" + Selected.Rank + "'" +
                           " ,Email ='" + Selected.Email + "'" +
                           " ,PhoneNo ='" + Selected.PhoneNo + "'" +
                           " ,MobileNo ='" + Selected.MobileNo + "'" +
                           " ,AllowWrite ='" + Selected.AllowWrite + "'" +
                           " ,SubClientId ='" + Selected.SubClientId + "'" +
                           " ,IsRegister ='" + Selected.IsRegister + "'" +
                           " ,CustomerId ='" + (int)cmb_Customer.SelectedValue + "'" +
                           " ,CustomerName ='" + cmb_Customer.Text + "'" +
                          " ,CustomerTeam = '"+ (int)cmbteam.SelectedValue + "'" +
                          ", CustomerTeamName = '" + cmbteam.Text + "'" +
                          ", CargoLoginId = '" + txtCargoLoginId.Text + "'" +
                            " WHERE ClientUserId = @ClientUserId";







                        cmd.Parameters.AddWithValue("@ClientUserId", Selected.ClientUserId);
                        





                        cmd.ExecuteNonQuery();
                        cn.Close();


                    }


                }
                if (Status == "Update")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "아이디", 1), "아이디 수정 성공");

                  
                }
                else if (Status == "Delete")
                {
                    //MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "아이디", 1), "아이디 삭제 성공");
                }
               // btn_Search_Click(null, null);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "아이디 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }

        private void _Search()
        {
            string _AdminSearchString = string.Empty;
            string _SearchString = string.Empty;
            string _FilterString = string.Empty;

            this.clientUsersTableAdapter.FillBySuper(this.clientDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);

            if (String.IsNullOrEmpty(txt_Search.Text))
            {
                clientUsersBindingSource.Filter = "";
            }
            else
            {
                if (cmb_Search.Text == "사용자명")
                {
                    clientUsersBindingSource.Filter = string.Format("Name Like  '%{0}%'", txt_Search.Text);
                }
                else if (cmb_Search.Text == "아이디")
                {
                    clientUsersBindingSource.Filter = string.Format("LoginId Like  '%{0}%'", txt_Search.Text);
                }
            }
        }
        
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as ClientDataSet.ClientUsersRow;
            if (Selected == null)
                return;


            if (e.ColumnIndex == 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == SubClientId.Index)
            {
                if (Selected.SubClientId == 0)
                {
                    e.Value = "본점";
                }
                else
                {
                    var Q = _SubClientTable.Where(c => c.SubClientId == Selected.SubClientId).ToArray();
                    // if (SubClientIdDictionary.ContainsKey(Selected.SubClientId))
                    e.Value = Q.First().Name;
                }
            }
            else if(e.ColumnIndex == rankDataGridViewTextBoxColumn.Index)
            {
                if (Selected.SubClientId == 0)
                    e.Value = "본점용";
                else if(Selected.IsAgent)
                    e.Value = "영업자용";
                else
                    e.Value = "지점용";
                if (Selected.IsRegister)
                    e.Value = "화물등록";
            }
        }

        class SubClientViewModel
        {
            public int SubClientId { get; set; }
          
            public string Name { get; set; }
       
        }
        private List<SubClientViewModel> _SubClientTable = new List<SubClientViewModel>();


        private void InitSubClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT Name, SubClientId FROM SubClients WHERE ClientId = @Clientid ";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _SubClientTable.Add(
                              new SubClientViewModel
                              {
                                  Name = dataReader.GetString(0),
                                  SubClientId = dataReader.GetInt32(1),
                                  
                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        void txt_CreateTimeBinding_Format(object sender, System.Windows.Forms.ConvertEventArgs e)
        {
            e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void clientUsersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            if (clientUsersBindingSource.Current == null)
            {
                return;
            }
            else
            {
                var Selected = ((DataRowView)clientUsersBindingSource.Current).Row as ClientDataSet.ClientUsersRow;
                if (Selected != null)
                {
                    if (Selected.Name == "슈퍼관리자" || Selected.IsRegister)
                    {
                        txt_Part.ReadOnly = true;
                        txt_Name.ReadOnly = true;
                        txt_Rank.ReadOnly = true;
                        txt_Email.ReadOnly = true;
                        txt_PhoneNo.ReadOnly = true;
                        txt_MobileNo.ReadOnly = true;
                        lbl_AllowWrite.Visible = false;
                        chk_AllowWrite.Visible = false;
                    }
                    else
                    {
                        txt_Part.ReadOnly = false;
                        txt_Name.ReadOnly = false;
                        txt_Rank.ReadOnly = false;
                        txt_Email.ReadOnly = false;
                        txt_PhoneNo.ReadOnly = false;
                        txt_MobileNo.ReadOnly = false;
                        lbl_AllowWrite.Visible = true;
                        chk_AllowWrite.Visible = true;
                    }
                    if(Selected.IsAgent && Selected.ClientId > 0)
                    {
                        IsAgent.Checked = true;
                    }
                    else
                    {
                        IsAgent.Checked = false;
                    }

                    cmbSubClientId.SelectedValue = Selected.SubClientId;

                    cmb_Customer.SelectedValue = Selected.CustomerId;


                    if (Selected.CustomerId > 0)
                    {

                        InitCustomerTeam(Selected.CustomerId);
                        if (Selected.CustomerTeam == 0)
                        {
                            cmbteam.SelectedIndex = 0;
                        }
                        else
                        {
                            cmbteam.SelectedValue = Selected.CustomerTeam;
                        }
                    }


                   // cmbteam.SelectedValue = Selected.CustomerTeam;


                }
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void txt_ClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void cmbSubClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubClientId.SelectedIndex == 0)
            {
                IsAgent.Checked = false;
                IsAgent.Enabled = false;
            }
            else
            {
                IsAgent.Enabled = true;
            }
        }

        private void IsAgent_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAgent.Checked)
            {
                chk_AllowWrite.Checked = true;
                chk_AllowWrite.Enabled = false;
            }
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "").Trim();
            _Txt.SelectionStart = _Txt.Text.Length;
        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_PhoneNo.Text))
            {
                var _S = txt_PhoneNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("1"))
                {
                    if (_S.Length > 4)
                    {
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
                    }
                    if (_S.Length > 8)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                    }
                }
                else if (_S.StartsWith("02"))
                {
                    if (_S.Length > 2)
                    {
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                    }
                    if (_S.Length > 6)
                    {
                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                    }
                    if (_S.Length > 11)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                    }
                }
                else
                {
                    if (_S.Length > 3)
                    {
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                    }
                    if (_S.Length > 7)
                    {
                        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                txt_PhoneNo.Text = _S;
            }
        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            _Txt.Text = _Txt.Text.Replace("-", "").Trim();
            _Txt.SelectionStart = _Txt.Text.Length;
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_MobileNo.Text))
            {
                var _S = txt_MobileNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 7)
                {
                    _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                }
                if (_S.Length > 12)
                {
                    _S = _S.Replace("-", "");
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                }
                txt_MobileNo.Text = _S;
            }
        }

        private void RegisterAdd_Click(object sender, EventArgs e)
        {
            if(LocalUser.Instance.LogInInformation.IsSubClient)
            {
                MessageBox.Show("본점 아이디로만 추가가 가능합니다.");
                return;
            }
            bool Is = false;
            Data.Connection(_Connection =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT ClientUserId FROM ClientUsers WHERE ClientId = @ClientId AND IsRegister = 1";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var _Reader = _Command.ExecuteReader();
                    if (_Reader.Read())
                    {
                        MessageBox.Show("화물 등록 아이디는 오직 하나만 생성할 수 있습니다.");
                        Is = true;
                        return;
                    }
                }
            });
            if (Is)
                return;
            FrmClientRegisterUser_Add _Form = new FrmClientRegisterUser_Add();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
            btn_Search_Click(null, null);
        }

        private void btnUserAuth_Click(object sender, EventArgs e)
        {
            if (clientUsersBindingSource.Count > 0)
            {
                FrmUserAuthorityAdd f = new FrmUserAuthorityAdd();

                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("권한을 설정할 사용자가 존재하지 않습니다", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void cmb_Customer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientUsersBindingSource.Current == null)
            {
                return;
            }

            var Selected = ((DataRowView)clientUsersBindingSource.Current).Row as ClientDataSet.ClientUsersRow;
            var _MCustomer = (int)cmb_Customer.SelectedValue;

            InitCustomerTeam(_MCustomer);

            
          //  cmbteam.SelectedValue = Selected.CustomerTeam;


        }

        private void InitCustomerTeam(int customerid)
        {

            

            cmbteam.Enabled = true;


            Dictionary<int, string> DCustomerTeam = new Dictionary<int, string>();

            InitCustomerAndTeamTable(customerid);

            var cmbCustomerTeamDataSource = _CustomerTeamTable.Select(c => new { c.TeamId, c.Part }).OrderBy(c => c.Part).ToList();


            DCustomerTeam.Add(0, "본사");

            foreach (var item in cmbCustomerTeamDataSource)
            {
                DCustomerTeam.Add(item.TeamId, item.Part);
            }


            cmbteam.DataSource = new BindingSource(DCustomerTeam, null);
            cmbteam.DisplayMember = "Value";
            cmbteam.ValueMember = "Key";
            cmbteam.SelectedIndex = 0;
        }
        class CustomerAndTeamViewModel
        {
            public int CustomerId { get; set; }
            public int TeamId { get; set; }
            public string Part { get; set; }


        }
        private List<CustomerAndTeamViewModel> _CustomerTeamTable = new List<CustomerAndTeamViewModel>();
        private void InitCustomerAndTeamTable(int _CustomerId)
        {
            _CustomerTeamTable.Clear();
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT CustomerTeamId as  idx, CustomerId ,TeamName " +
                        " FROM CustomerTeams " +
                        " WHERE(ClientId = @ClientId) AND CustomerId = @CustomerId" +

                        " ORDER BY CreateDate DESC";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    commnad.Parameters.AddWithValue("@CustomerId", _CustomerId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _CustomerTeamTable.Add(
                              new CustomerAndTeamViewModel
                              {
                                  TeamId = dataReader.GetInt32(0),
                                  CustomerId = dataReader.GetInt32(1),

                                  Part = dataReader.GetString(2),


                              });
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
