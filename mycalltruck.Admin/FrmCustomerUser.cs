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
    public partial class FrmCustomerUser : Form
    {
        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
           // auth = this.GetAuth();
            //switch (auth)
            //{
            //    case MenuAuth.None:
            //        MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        Close();
            //        return;
            //    case MenuAuth.Read:

            //        btn_New.Enabled = false;
            //        btnUpdate.Enabled = false;
            //        btnCurrentDelete.Enabled = false;




            //        dataGridView1.ReadOnly = true;
            //        //grid2.ReadOnly = true;
            //        break;
            //}
            if(LocalUser.Instance.LogInInformation.IsAdmin || LocalUser.Instance.LogInInformation.IsClient)
            {
                MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
                return;
            }

            //if(LocalUser.Instance.LogInInformation.CustomerUserId != 0)
            //{
            //    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    Close();
            //    return;
            //}

        }
        public FrmCustomerUser()
        {
            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            _InitCmb();
            //txt_CreateTime.DataBindings[0].Format += txt_CreateTimeBinding_Format;
            LocalUser.Instance.LogInInformation.LoadClient();

          
        }


        Dictionary<int, String> SubClientIdDictionary = new Dictionary<int, string>();
        private void _InitCmb()
        {
            


        }

        private void FrmClientUser_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'customerStartManageDataSet.CustomerAddPhone' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.customerAddPhoneTableAdapter.Fill(this.customerStartManageDataSet.CustomerAddPhone, LocalUser.Instance.LogInInformation.CustomerId);

            
            this.dataGridView1.Columns[1].Visible = false;
            this.dataGridView1.Columns[2].Visible = false;
            btn_New.Enabled = true;
            btnUpdate.Enabled = true;
            btnCurrentDelete.Enabled = true;
            cmb_ClientSerach.Visible = false;
            txt_ClientSearch.Visible = false;
            btn_Inew_Click(null, null);
            //btn_Search_Click(null, null);
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmCustomerUser_Add _Form = new FrmCustomerUser_Add(LocalUser.Instance.LogInInformation.CustomerId);
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
            btn_Search_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String r = String.Empty;
            err.Clear();
            var Selected = ((DataRowView)customerAddPhoneBindingSource.Current).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;

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

            r = r.Trim();

            if (!String.IsNullOrEmpty(r))
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
            var Selected = ((DataRowView)customerAddPhoneBindingSource.Current).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (Selected.AddName == "슈퍼관리자")
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
                    customerAddPhoneTableAdapter.Update(customerStartManageDataSet);
                    
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

                var Selected = ((DataRowView)customerAddPhoneBindingSource.Current).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;

                if (Selected.AddName =="슈퍼관리자" )
                {
                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {


                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();



                        cmd.CommandText =
                           "UPDATE Customers SET Password = @Password WHERE CustomerId = @CustomerId ";




                        cmd.Parameters.AddWithValue("@Password", txt_Password.Text);
                        cmd.Parameters.AddWithValue("@CustomerId", LocalUser.Instance.LogInInformation.CustomerId);





                        cmd.ExecuteNonQuery();
                        cn.Close();


                    }
                }
                else
                {

                    customerAddPhoneBindingSource.EndEdit();
                    customerAddPhoneTableAdapter.Update(Selected);
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

            this.customerAddPhoneTableAdapter.FillBySuper(this.customerStartManageDataSet.CustomerAddPhone, LocalUser.Instance.LogInInformation.CustomerId);

            if (String.IsNullOrEmpty(txt_Search.Text))
            {

                customerAddPhoneBindingSource.Filter = string.Format(" idx =  '{0}'", LocalUser.Instance.LogInInformation.CustomerUserId);
            }
            else
            {
                if (cmb_Search.Text == "사용자명")
                {
                    customerAddPhoneBindingSource.Filter = string.Format("Name Like  '%{0}%'", txt_Search.Text);
                }
                else if (cmb_Search.Text == "아이디")
                {
                    customerAddPhoneBindingSource.Filter = string.Format("LoginId Like  '%{0}%'", txt_Search.Text);
                }

                if (LocalUser.Instance.LogInInformation.ClientUserId != 0)
                {
                    if (customerAddPhoneBindingSource.Filter != "")
                    {
                        customerAddPhoneBindingSource.Filter += string.Format("AND idx =  '{0}'", LocalUser.Instance.LogInInformation.CustomerUserId);
                    }
                    else
                    {
                        customerAddPhoneBindingSource.Filter = string.Format(" idx =  '{0}'", LocalUser.Instance.LogInInformation.CustomerUserId);
                    }
                }
            }


        }
        
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;
            if (Selected == null)
                return;


            if (e.ColumnIndex == 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
           
        }

      

       
        void txt_CreateTimeBinding_Format(object sender, System.Windows.Forms.ConvertEventArgs e)
        {
            e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

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
            var _Txt = ((System.Windows.Forms.TextBox)sender);
            if (!String.IsNullOrWhiteSpace(_Txt.Text))
            {
                var _S = _Txt.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
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
                    if (_S.Length == 8)
                    {
                        _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
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
                        if (_S.Length == 12)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 3) + "-" + _S.Substring(6, 4);
                        }
                        else if (_S.Length == 13)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                        }
                        else if (_S.Length > 13)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4) + "-" + _S.Substring(8, 4);
                        }
                    }
                }
                _Txt.Text = _S;
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

        //private void RegisterAdd_Click(object sender, EventArgs e)
        //{
        //    if(LocalUser.Instance.LogInInformation.IsSubClient)
        //    {
        //        MessageBox.Show("본점 아이디로만 추가가 가능합니다.");
        //        return;
        //    }
        //    bool Is = false;
        //    Data.Connection(_Connection =>
        //    {
        //        using (SqlCommand _Command = _Connection.CreateCommand())
        //        {
        //            _Command.CommandText = "SELECT ClientUserId FROM ClientUsers WHERE ClientId = @ClientId AND IsRegister = 1";
        //            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
        //            var _Reader = _Command.ExecuteReader();
        //            if (_Reader.Read())
        //            {
        //                MessageBox.Show("화물 등록 아이디는 오직 하나만 생성할 수 있습니다.");
        //                Is = true;
        //                return;
        //            }
        //        }
        //    });
        //    if (Is)
        //        return;
        //    FrmClientRegisterUser_Add _Form = new FrmClientRegisterUser_Add();
        //    _Form.Owner = this;
        //    _Form.StartPosition = FormStartPosition.CenterParent;
        //    _Form.ShowDialog();
        //    btn_Search_Click(null, null);
        //}

        private void btnUserAuth_Click(object sender, EventArgs e)
        {
        
        }

        private void customerAddPhoneBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            if (customerAddPhoneBindingSource.Current == null)
            {
                return;
            }
            else
            {
                var Selected = ((DataRowView)customerAddPhoneBindingSource.Current).Row as CustomerStartManageDataSet.CustomerAddPhoneRow;
                if (Selected != null)
                {
                    if (Selected.AddName == "슈퍼관리자")
                    {
                        txt_Part.ReadOnly = true;
                        txt_Name.ReadOnly = true;
                        txt_Rank.ReadOnly = true;
                        txt_Email.ReadOnly = true;
                        txt_PhoneNo.ReadOnly = true;
                        txt_MobileNo.ReadOnly = true;

                    }
                    else
                    {
                        txt_Part.ReadOnly = false;
                        txt_Name.ReadOnly = false;
                        txt_Rank.ReadOnly = false;
                        txt_Email.ReadOnly = false;
                        txt_PhoneNo.ReadOnly = false;
                        txt_MobileNo.ReadOnly = false;

                    }

                }
            }
        }
    }
}
