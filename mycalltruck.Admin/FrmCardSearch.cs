using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace mycalltruck.Admin
{
    public partial class FrmCardSearch : Form
    {
      
        public FrmCardSearch()
        {
            InitializeComponent();
            grid1.AutoGenerateColumns = false;
        }

        private void FrmCustomerSearch_Load(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);
        }

        string Salegubunin = string.Empty;
        private void _Search()
        {
            var _CommandText = "SELECT ClientId, Name, BizNo, CEO FROM Clients WHERE PAYLOGISYN = 1 AND ClientId <> @ClientId";
            this.cMDataSet.Clients.Clear();
            try
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    SqlCommand _Command = _Connection.CreateCommand();
                    if (!String.IsNullOrEmpty(txt_Search.Text))
                    {
                        _CommandText += " AND (REPLACE(BizNo,N'-',N'') LIKE '%' + @Text + '%' OR Name LIKE '%' + @Text + '%' OR CEO LIKE '%' + @Text + '%')";
                        _Command.Parameters.AddWithValue("@Text", txt_Search.Text);
                    }
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    _Command.CommandText = _CommandText;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        cMDataSet.Clients.Load(_Reader);
                    }
                }
            }
            catch
            {
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
