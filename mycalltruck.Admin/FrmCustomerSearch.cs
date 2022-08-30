using mycalltruck.Admin.Class.Common;
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
    public partial class FrmCustomerSearch : Form
    {
        string S_GubunNum = string.Empty;
        public FrmCustomerSearch(string GubunNum)
        {
            InitializeComponent();

            S_GubunNum = GubunNum;
        }

        private void FrmCustomerSearch_Load(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);
        }

        string Salegubunin = string.Empty;
        private void _Search()
        {
            try
            {
                var _SelectCommandText =
                @"SELECT  CustomerId, Code, BizNo, SangHo, Ceo, Uptae, Upjong, AddressState, AddressCity, AddressDetail, BizGubun, ResgisterNo, SalesGubun, Email, PhoneNo, FaxNo, ChargeName, MobileNo, 
                        CreateTime, ClientId, Zipcode, SubClientId, EndDay, ClientUserId
                        FROM     Customers ";
                cMDataSet.Customers.Clear();
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        String SelectCommandText = _SelectCommandText;

                        List<String> WhereStringList = new List<string>();
                        // 1. 본점/지사
                        if (LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            if (LocalUser.Instance.LogInInformation.IsAgent)
                            {
                                WhereStringList.Add("ClientUserId = @ClientUserId");
                                _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                            }
                            else
                            {
                                WhereStringList.Add("SubClientId = @SubClientId");
                                _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                            }
                        }
                        else if (!LocalUser.Instance.LogInInformation.IsAdmin)
                        {
                            WhereStringList.Add("ClientId = @ClientId");
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        }
                        // 2. 단어 검색
                        if (!String.IsNullOrEmpty(txt_Search.Text))
                        {
                            WhereStringList.Add("( REPLACE(BizNo,N'-',N'') LIKE '%' + @Text + '%' OR SangHo LIKE '%'+@Text+'%')  ");
                            _Command.Parameters.AddWithValue("@Text", txt_Search.Text.Replace("-", ""));
                        }
                        if (WhereStringList.Count > 0)
                        {
                            var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                            SelectCommandText += Environment.NewLine + WhereString;
                        }
                        _Command.CommandText = SelectCommandText + " Order by CreateTime DESC";
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            cMDataSet.Customers.Load(_Reader);
                        }
                    }
                    _Connection.Close();
                }
            }
            catch
            {
                MessageBox.Show("조회가 실패하였습니다. 잠시 후에 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            txt_Search.Text = "";
            btn_Search_Click(null, null);
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }
    }
}
