using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.Threading;

namespace mycalltruck.Admin
{
    public partial class FrmUserAuthorityAdd : Form
    {
        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
            auth = this.GetAuth();

            switch (auth)
            {
                case MenuAuth.None:
                    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                    return;
                case MenuAuth.Read:
                    btnDefault.Enabled = false;
                    btnOK.Enabled = false;
                    newDGV1.ReadOnly = true;
                    dataGridView1.ReadOnly = true;
                    break;
            }
        }


        public FrmUserAuthorityAdd()
        {
            InitializeComponent();
            LocalUser.Instance.LogInInformation.LoadClient();
        }
        private bool ingSelect = false;
        private void FrmUserAuthorityAdd_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'baseDataSet.UserAuthority' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.userAuthorityTableAdapter.Fill(this.baseDataSet.UserAuthority);
            this.clientUsersTableAdapter.Fill(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);//.FillBySuper(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);


            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                this.clientUsersTableAdapter.Fill(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);

                ingSelect = true;
                this.userAuthorityTableAdapter.Fill(this.baseDataSet.UserAuthority);
                foreach (var item in baseDataSet.UserAuthority)
                {
                    if (item.ReadAuth == false && item.WriteAuth == false)
                        item.NoneAuth = true;
                    else item.NoneAuth = false;
                }
                ingSelect = false;
            }


            _Search();

        }

        private void clientUsersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (clientUsersBindingSource.Current == null)
            {
                return;
            }
            ingSelect = true;
            var Selected = ((DataRowView)clientUsersBindingSource.Current).Row as CMDataSet.ClientUsersRow;

            int _MenuCount = 0;
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                Connection.Open();
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText =
                    @"SELECT COUNT(*) FROM UserAuthority WHERE UserId = @UserId ";
                Command.Parameters.AddWithValue("@UserId", Selected.LoginId);

                var DataReader = Command.ExecuteReader();
                if (DataReader.Read())
                {
                    _MenuCount = DataReader.GetInt32(0);

                }
                Connection.Close();
                Connection.Open();
                if (_MenuCount != 35)
                {
                    using (SqlCommand _Command = Connection.CreateCommand())
                    {
                        _Command.CommandText = "DELETE UserAuthority WHERE UserId = @LoginId ";
                        _Command.CommandText += " Insert UserAuthority (UserId, MenuCode, ReadAuth, WriteAuth, Memo, MenuName)" +
                                              "SELECT @LoginId ,MenuCode,0,1,'' ,MenuName FROM MenuList";
                        _Command.Parameters.AddWithValue("@LoginId", Selected.LoginId);

                        _Command.ExecuteNonQuery();
                    }

                }

                Connection.Close();



            }



            ingSelect = true;
            try
            {
                CMDataSet.ClientUsersRow user = ((DataRowView)clientUsersBindingSource.Current).Row as CMDataSet.ClientUsersRow;
                lblUserID.Text = user.LoginId;
                lblUserName.Text = user.Name;

                //foreach (var item in baseDataSet.UserAuthority.Where(c => c.UserId == user.LoginId))
                //{
                //    item.ReadAuth = false;
                //    item.WriteAuth = true;


                //}
                this.userAuthorityTableAdapter.Fill(this.baseDataSet.UserAuthority);
                foreach (var item in baseDataSet.UserAuthority)
                {
                    if (item.ReadAuth == false && item.WriteAuth == false)
                        item.NoneAuth = true;
                    else item.NoneAuth = false;
                }
                //int count = (int)userAuthorityTableAdapter.GetExist(user.LoginId);


                userAuthorityBindingSource.Filter = string.Format("UserId  =   '{0}'", Selected.LoginId);
            }
            catch { }
            ingSelect = false;

        }
        private void _Search()
        {
            string _AdminSearchString = string.Empty;
            string _SearchString = string.Empty;
            string _FilterString = string.Empty;

            //this.clientUsersTableAdapter.FillBySuper(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);
            this.clientUsersTableAdapter.Fill(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);

            //if (String.IsNullOrEmpty(txt_Search.Text))
            //{
            //    clientUsersBindingSource.Filter = "";
            //}
            //else
            //{
            //    if (cmb_Search.Text == "사용자명")
            //    {
            //        clientUsersBindingSource.Filter = string.Format("Name Like  '%{0}%'", txt_Search.Text);
            //    }
            //    else if (cmb_Search.Text == "아이디")
            //    {
            //        clientUsersBindingSource.Filter = string.Format("LoginId Like  '%{0}%'", txt_Search.Text);
            //    }
            //}
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.ClientUsersRow;
            if (Selected == null)
                return;


            if (e.ColumnIndex == 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }

            else if (e.ColumnIndex == rankDataGridViewTextBoxColumn.Index)
            {
                if (Selected.SubClientId == 0)
                    e.Value = "본점용";
                else if (Selected.IsAgent)
                    e.Value = "영업자용";
                else
                    e.Value = "지점용";
                if (Selected.IsRegister)
                    e.Value = "화물등록";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            pnProgress.Visible = true;
            bar.Value = 0;
            pnProgress.Invoke(new Action(() => this.Enabled = false));
            Thread t = new Thread(new ThreadStart(() =>
            {
                bar.Invoke(new Action(() => {
                    bar.Value++;
                }));

                userAuthorityTableAdapter.Update(baseDataSet.UserAuthority);
                pnProgress.Invoke(new Action(() => this.clientUsersTableAdapter.Fill(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId)));
                //this.clientUsersTableAdapter.Fill(this.cMDataSet.ClientUsers, LocalUser.Instance.LogInInformation.ClientId);
                pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
                pnProgress.Invoke(new Action(() => this.Enabled = true));
                pnProgress.Invoke(new Action(() => MessageBox.Show("권한설정 변경이 성공하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));

            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();


           


          

            //MessageBox.Show("권한설정 변경이 성공하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void newDGV1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            newDGV1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void newDGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool value = false;
            try
            {
                value = !(bool)newDGV1[e.ColumnIndex, e.RowIndex].Value;
            }
            catch { }
            if (value)
            {
                try
                {

                    BaseDataSet.UserAuthorityRow auth = ((DataRowView)userAuthorityBindingSource.Current).Row as BaseDataSet.UserAuthorityRow;
                    if (e.ColumnIndex == NoneCheckBoxColumn.Index)
                    {
                        auth.ReadAuth = false;
                        auth.WriteAuth = false;
                    }
                    else if (e.ColumnIndex == readAuthDataGridViewCheckBoxColumn.Index)
                    {
                        auth.NoneAuth = false;
                        auth.WriteAuth = false;
                    }
                    else if (e.ColumnIndex == writeAuthDataGridViewCheckBoxColumn.Index)
                    {
                        auth.NoneAuth = false;
                        auth.ReadAuth = false;
                    }

                    userAuthorityBindingSource.EndEdit();
                    newDGV1.Refresh();
                }
                catch { }
            }
        }

        private void newDGV1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void newDGV1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (clientUsersBindingSource.Current == null)
            {
                return;
            }
            ingSelect = true;
            var Selected = ((DataRowView)clientUsersBindingSource.Current).Row as CMDataSet.ClientUsersRow;

            //  int _MenuCount = 0;
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {

                Connection.Open();

                using (SqlCommand _Command = Connection.CreateCommand())
                {
                    _Command.CommandText = "DELETE UserAuthority WHERE UserId = @LoginId ";
                    _Command.CommandText += " Insert UserAuthority (UserId, MenuCode, ReadAuth, WriteAuth, Memo, MenuName)" +
                                          "SELECT @LoginId ,MenuCode,0,1,'' ,MenuName FROM MenuList";
                    _Command.Parameters.AddWithValue("@LoginId", Selected.LoginId);

                    _Command.ExecuteNonQuery();
                }



                Connection.Close();

            }

            ingSelect = true;
            try
            {
                CMDataSet.ClientUsersRow user = ((DataRowView)clientUsersBindingSource.Current).Row as CMDataSet.ClientUsersRow;
                lblUserID.Text = user.LoginId;
                lblUserName.Text = user.Name;

                this.userAuthorityTableAdapter.Fill(this.baseDataSet.UserAuthority);
                foreach (var item in baseDataSet.UserAuthority)
                {
                    if (item.ReadAuth == false && item.WriteAuth == false)
                        item.NoneAuth = true;
                    else item.NoneAuth = false;
                }
               


                userAuthorityBindingSource.Filter = string.Format("UserId  =   '{0}'", Selected.LoginId);
            }
            catch { }
            ingSelect = false;
        }
    }
}
