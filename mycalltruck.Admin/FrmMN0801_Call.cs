using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0801_Call : Form
    {
        private Dictionary<String, String> CustomerDictionary = new Dictionary<string, string>();
        private Dictionary<String, String> DriverDictionary = new Dictionary<string, string>();
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

                    btn_Centrix.Enabled = false;
                    btn_Setting.Enabled = false;
                    


                    dataGridView1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }
        public FrmMN0801_Call()
        {
            InitializeComponent();
            dtpFrom.Value = DateTime.Now.Date.AddDays(-7);
            dtpTo.Value = DateTime.Now.Date;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _InnerDataSource;

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }

        private void FrmMN0801_Call_Load(object sender, EventArgs e)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        $"SELECT Sangho, ISNULL(PhoneNo, ''), ISNULL(MobileNo, '') FROM Customers WHERE ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var Sangho = _Reader.GetString(0);
                            var PhoneNo = _Reader.GetString(1).Replace("-","");
                            var MobileNo = _Reader.GetString(2).Replace("-", "");
                            if (!String.IsNullOrEmpty(PhoneNo))
                            {
                                if (!CustomerDictionary.ContainsKey(PhoneNo))
                                    CustomerDictionary.Add(PhoneNo, Sangho);
                            }
                            if (!String.IsNullOrEmpty(MobileNo))
                            {
                                if (!CustomerDictionary.ContainsKey(MobileNo))
                                    CustomerDictionary.Add(MobileNo, Sangho);
                            }
                        }
                    }
                }
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText =
                        $"SELECT Name, ISNULL(PhoneNo, ''), ISNULL(MobileNo, '') FROM Drivers JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId WHERE DriverInstances.ClientId = {LocalUser.Instance.LogInInformation.ClientId}";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var Name = _Reader.GetString(0);
                            var PhoneNo = _Reader.GetString(1).Replace("-", "");
                            var MobileNo = _Reader.GetString(2).Replace("-", "");
                            if (!String.IsNullOrEmpty(PhoneNo))
                            {
                                if (!DriverDictionary.ContainsKey(PhoneNo))
                                    DriverDictionary.Add(PhoneNo, Name);
                            }
                            if (!String.IsNullOrEmpty(MobileNo))
                            {
                                if (!DriverDictionary.ContainsKey(MobileNo))
                                    DriverDictionary.Add(MobileNo, Name);
                            }
                        }
                    }
                }
                _Connection.Close();
            }

            txtID.Text = CallHelper.Instance.Id;
            txtPassword.Text = CallHelper.Instance.Password;

            Btn_Search_Click(null, null);
            btn_Search.Focus();
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            _InnerDataSource.Clear();
            var _Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CardPay");
            if (Directory.Exists(_Directory) == false)
                Directory.CreateDirectory(_Directory);
            var Date = dtpFrom.Value;
            List<Model> T = new List<Model>();
            while (Date <= dtpTo.Value)
            {
                if (Directory.Exists(Path.Combine(_Directory, Date.ToString("yyyy-MM-dd"))))
                {
                    foreach (var filePath in Directory.GetFiles(Path.Combine(_Directory, Date.ToString("yyyy-MM-dd"))))
                    {
                        if (Path.GetExtension(filePath).ToLower().EndsWith("wav"))
                        {
                            T.Add(new Model
                            {
                                FilePath = filePath
                            }.Parse());
                        }
                    }
                }
                Date = Date.AddDays(1);
            }
            foreach (var Added in T.OrderByDescending(c=>c.RecordTime))
            {
                _InnerDataSource.Add(Added);
            }
        }
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == No.Index)
            {
                e.Value = (e.RowIndex + 1).ToString("N0");
            }
            else if(e.ColumnIndex == ColumnName.Index)
            {
                var CurrentModel = _InnerDataSource[e.RowIndex];
                if (CustomerDictionary.ContainsKey(CurrentModel.PhoneNo))
                    e.Value = CustomerDictionary[CurrentModel.PhoneNo];
                else if (DriverDictionary.ContainsKey(CurrentModel.PhoneNo))
                    e.Value = DriverDictionary[CurrentModel.PhoneNo];
            }
            else if (e.ColumnIndex == ColumnDiv.Index)
            {
                var CurrentModel = _InnerDataSource[e.RowIndex];
                if (CustomerDictionary.ContainsKey(CurrentModel.PhoneNo))
                    e.Value = "화주";
                else if (DriverDictionary.ContainsKey(CurrentModel.PhoneNo))
                    e.Value = "차주";
            }
        }

        BindingList<Model> _InnerDataSource = new BindingList<Model>();
        class Model
        {
            public DateTime RecordTime { get; set; }
            public String Id { get; set; }
            public string PhoneNo { get; set; }
            public string FilePath { get; set; }
            public string Memo { get; set; }

            public Model Parse()
            {
                try
                {
                    var FileName = Path.GetFileNameWithoutExtension(FilePath);
                    var s = FileName.Split('_');
                    //Id = s[0];
                    PhoneNo = s[0].Split('-')[1];
                    RecordTime = DateTime.ParseExact(s[1], "yyyyMMddHHmmss", null);
                }
                catch (Exception)
                {
                }
                return this;
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == Play.Index)
            {
                Process.Start(_InnerDataSource[e.RowIndex].FilePath);
            }
            else if(e.ColumnIndex == Download.Index)
            {
                SaveFileDialog d = new SaveFileDialog();
                d.AddExtension = true;
                d.DefaultExt = "wav";
                if(d.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(_InnerDataSource[e.RowIndex].FilePath, d.FileName);
                    Process.Start(Path.GetDirectoryName(d.FileName));
                }
            }
        }

        private void Btn_Setting_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(txtID.Text) && !String.IsNullOrEmpty(txtPassword.Text))
            {
                CallHelper.Instance.Login(txtID.Text, txtPassword.Text);
            }
            // 여기서 접속시도
            // 맞으면 파일로 해당정보 저장하고
            // 틀리면 전화번호/비밀번호 클리어 혹은 이전 정보로 돌아가기
        }

        private void btn_Centrix_Click(object sender, EventArgs e)
        {
            Process.Start("http://centrex.uplus.co.kr/premium/index.html");
        }
    }
}
