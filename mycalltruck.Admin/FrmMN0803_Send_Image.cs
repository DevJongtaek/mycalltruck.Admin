using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
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

namespace mycalltruck.Admin
{
    public partial class FrmMN0803_Send_Image : Form
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

                    btn_Delete.Enabled = false;

                    mDataGridView.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }
        public FrmMN0803_Send_Image()
        {
            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            InnerList.Clear();
            List<String> WhereList = new List<string>();
            WhereList.Add($"SendImages.ClientId = '{LocalUser.Instance.LogInInformation.ClientId}'");
            WhereList.Add($"SendImages.CreateDate >= '{dtp_Sdate.Value.ToString("yyyy-MM-dd")}'");
            WhereList.Add($"SendImages.CreateDate < '{dtp_Edate.Value.AddDays(1).ToString("yyyy-MM-dd")}'");
            switch (cmb_Search.SelectedIndex)
            {
                case 1:
                    WhereList.Add($"Customers.SangHo LIKE '%{txt_Search.Text}%'");
                    break;
                case 2:
                    WhereList.Add($"Drivers.LoginId LIKE '%{txt_Search.Text}%'");
                    break;
                case 3:
                    WhereList.Add($"Drivers.Name LIKE '%{txt_Search.Text}%'");
                    break;
                default:
                    break;
            }
            var _Query = $"{QueryText}WHERE {String.Join(" AND ", WhereList)} ORDER BY SendImages.SendImageId DESC";
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = _Query;
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var Added = new Model();
                            Added.SendImageId = _Reader.GetInt32(_Reader.GetOrdinal("SendImageId"));
                            Added.CreateTime = _Reader.GetDateTime(_Reader.GetOrdinal("CreateTime"));
                            Added.Customer = _Reader.GetString(_Reader.GetOrdinal("Customer"));
                            Added.Driver = _Reader.GetString(_Reader.GetOrdinal("Driver"));
                            Added.DriverId = _Reader.GetString(_Reader.GetOrdinal("DriverId"));
                            Added.Image1 = _Reader.GetString(_Reader.GetOrdinal("Image1"));
                            Added.Image2 = _Reader.GetString(_Reader.GetOrdinal("Image2"));
                            Added.Image3 = _Reader.GetString(_Reader.GetOrdinal("Image3"));
                            Added.Image4 = _Reader.GetString(_Reader.GetOrdinal("Image4"));
                            Added.Image5 = _Reader.GetString(_Reader.GetOrdinal("Image5"));
                            Added.Image6 = _Reader.GetString(_Reader.GetOrdinal("Image6"));
                            Added.Image7 = _Reader.GetString(_Reader.GetOrdinal("Image7"));
                            Added.Image8 = _Reader.GetString(_Reader.GetOrdinal("Image8"));
                            Added.Image9 = _Reader.GetString(_Reader.GetOrdinal("Image9"));
                            Added.Image10 = _Reader.GetString(_Reader.GetOrdinal("Image10"));
                            Added.ImageCount = new string[] { Added.Image1, Added.Image2, Added.Image3, Added.Image4, Added.Image5, Added.Image6, Added.Image7, Added.Image8, Added.Image9, Added.Image10 }.Count(c => !String.IsNullOrEmpty(c));

                            InnerList.Add(Added);
                        }
                    }
                }
                _Connection.Close();
            }
            mDataGridView.AutoGenerateColumns = false;
            mDataGridView.DataSource = InnerList;
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Clear();
            btn_Search_Click(null, null);
        }

        private void FrmMN0803_Send_Image_Load(object sender, EventArgs e)
        {
            dtp_Edate.Value = DateTime.Now;
            dtp_Sdate.Value = DateTime.Now.AddMonths(-3);
            cmb_Search.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }

        private void mDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == ColumnImage.Index)
            {
                var model = InnerList[e.RowIndex];
                TradeDataSet.TradesRow Trade = new TradeDataSet.TradesDataTable().NewTradesRow();
                Trade.Image1 = model.Image1;
                Trade.Image2 = model.Image2;
                Trade.Image3 = model.Image3;
                Trade.Image4 = model.Image4;
                Trade.Image5 = model.Image5;
                Trade.Image6 = model.Image6;
                Trade.Image7 = model.Image7;
                Trade.Image8 = model.Image8;
                Trade.Image9 = model.Image9;
                Trade.Image10 = model.Image10;
                FormImages f = new FormImages(Trade);
                f.ShowDialog();
            }
        }

        private void mDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnNo.Index)
            {
                e.Value = (e.RowIndex + 1).ToString();
            }
        }


        private string QueryText =
            @"SELECT SendImages.SendImageId, SendImages.CreateDate as CreateTime, Customers.SangHo as Customer, Drivers.LoginId as DriverId, Drivers.Name as Driver,  
ISNULL(SendImages.Image1, '') AS Image1, ISNULL(SendImages.Image2, '') AS Image2, ISNULL(SendImages.Image3, '') AS Image3, ISNULL(SendImages.Image4, '') AS Image4, ISNULL(SendImages.Image5, '') AS Image5, 
ISNULL(SendImages.Image6, '') AS Image6, ISNULL(SendImages.Image7, '') AS Image7, ISNULL(SendImages.Image8, '') AS Image8, ISNULL(SendImages.Image9, '') AS Image9, ISNULL(SendImages.Image10, '') AS Image10 
FROM SendImages JOIN Customers ON SendImages.CustomerId = Customers.CustomerId JOIN Drivers ON SendImages.DriverId = Drivers.DriverId" + Environment.NewLine;

        BindingList<Model> InnerList = new BindingList<Model>();

        class Model
        {
            public int SendImageId { get; set; }
            public DateTime CreateTime { get; set; }
            public string Customer { get; set; }
            public string DriverId { get; set; }
            public string Driver { get; set; }
            public string Image1 { get; set; }
            public string Image2 { get; set; }
            public string Image3 { get; set; }
            public string Image4 { get; set; }
            public string Image5 { get; set; }
            public string Image6 { get; set; }
            public string Image7 { get; set; }
            public string Image8 { get; set; }
            public string Image9 { get; set; }
            public string Image10 { get; set; }
            public int ImageCount { get; set; }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if(mDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("먼저 삭제할 항목을 선택하여 주십시오.");
                return;
            }
            if(mDataGridView.SelectedRows[0].Index < 0)
            {
                MessageBox.Show("먼저 삭제할 항목을 선택하여 주십시오.");
                return;
            }
            var Selected = InnerList[mDataGridView.SelectedRows[0].Index];
            if (MessageBox.Show($"[{Selected.CreateTime.ToString("yyyy-MM-dd")}] {Selected.Driver} => {Selected.Customer} 사진({Selected.ImageCount}장)을 정말 삭제 하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"DELETE FROM SendImages WHERE SendImageId = {Selected.SendImageId}";
                    _Command.ExecuteNonQuery();
                }
                _Connection.Close();
            }
            if (mDataGridView.SelectedRows[0].Index < 0)
            {
                MessageBox.Show("삭제 하였습니다.");
                return;
            }
            btn_Search_Click(null, null);
        }
    }
}
