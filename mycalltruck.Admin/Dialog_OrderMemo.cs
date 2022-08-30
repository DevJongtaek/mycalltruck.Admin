using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
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
    public partial class Dialog_OrderMemo : Form
    {
        int customerId = 0;
        public Dialog_OrderMemo(int CustomerId)
        {
            InitializeComponent();

            customerId = CustomerId;

        

        }

       

        private void Dialog_OrderMemo_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'customerStartManageDataSet.CustomerAddPhone' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.customerAddPhoneTableAdapter.Fill(this.customerStartManageDataSet.CustomerAddPhone, customerId);
            AccountMemoSelect();
            LoadTable();
        }
        private String GetSelectCommand()
        {
            return @"select idx, AddTeam, AddName, AddPhoneNo, ClientId, CustomerId, CreateTime,ISNULL(Remark,'') AS Remark from CustomerAddPhone ";


        }
        private void LoadTable()
        {
            customerStartManageDataSet.CustomerAddPhone.Clear();
           
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();


                  

                    WhereStringList.Add("ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);


                    WhereStringList.Add("CustomerId = @CustomerId");
                    _Command.Parameters.AddWithValue("@CustomerId", customerId);



                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }


                    SelectCommandText += " Order by CreateTime  Desc ";

                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        customerStartManageDataSet.CustomerAddPhone.Load(_Reader);



                    }
                }


                _Connection.Close();


            }

         
        }
        private void AccountMemoSelect()
        {
            string tAccountMemo = "";
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        @"SELECT  Remark FROM  Customers
                                    WHERE CustomerId = @CustomerId AND ClientId = @ClientId ";
                    _Command.Parameters.AddWithValue("@CustomerId", customerId);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    object O = _Command.ExecuteScalar();
                    if (O != null)
                        tAccountMemo = Convert.ToString(O);
                }
                _Connection.Close();
            }

            txtMemo.Text = tAccountMemo;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "Update Customers " +
                                           " SET Remark = @Remark " +
                                           " WHERE CustomerId = @CustomerId" +
                                           " AND ClientId = @ClientId ";
                    _Command.Parameters.AddWithValue("@Remark", txtMemo.Text);
                    _Command.Parameters.AddWithValue("@CustomerId", customerId);
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.ExecuteNonQuery();
                }
                _Connection.Close();

            });

            try
            {
                MessageBox.Show($"화주메모 수정 완료 되었습니다.", "화주메모", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch(Exception ex)
            {
                MessageBox.Show("화주메모 수정 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

       


            if (grid1.Columns[e.ColumnIndex] == rowNUMDataGridViewTextBoxColumn)
            {
                e.Value = (grid1.Rows.Count - e.RowIndex).ToString("N0");
            }
        }
    }
}
