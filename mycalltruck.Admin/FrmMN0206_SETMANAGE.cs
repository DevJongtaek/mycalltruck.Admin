using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;
using System.Data.SqlClient;
using mycalltruck.Admin.UI;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class;
using System.Diagnostics;

namespace mycalltruck.Admin
{
    public partial class FrmMN0206_SETMANAGE : Form
    {
        string SLoginID;
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
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
                    tableLayoutPanel2.Enabled = false;



                    dataGridView1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }

        public FrmMN0206_SETMANAGE()
        {
            InitializeComponent();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }

            foreach (var item in SingleDataSet.Instance.AddressReferences)
            {
                if (String.IsNullOrEmpty(item.State) || String.IsNullOrEmpty(item.City) || String.IsNullOrEmpty(item.Street))
                    continue;
                if (!AddressList.Any(c => c.State == item.State && c.City == item.City && c.Street == item.Street))
                {
                    AddressList.Add(new AddressViewModel
                    {
                        State = item.State,
                        City = item.City,
                        Street = item.Street,
                    });
                }
            }


            if (!String.IsNullOrEmpty(LocalUser.Instance.PersonalOption.FPISTRU))
            {
                txtFpisTruFolder.Text = LocalUser.Instance.PersonalOption.FPISTRU;
            }
            else
            {
                backUpValueChanged(null, null);
            }

            if (!String.IsNullOrEmpty(LocalUser.Instance.PersonalOption.FPISCAR))
            {
                txtFpisCarFolder.Text = LocalUser.Instance.PersonalOption.FPISCAR;
            }
            else
            {
                backUpValueChanged1(null, null);
            }
            if (!String.IsNullOrEmpty(LocalUser.Instance.PersonalOption.TAX))
            {
                txtTaxFolder.Text = LocalUser.Instance.PersonalOption.TAX;
            }
            else
            {
                backUpValueChanged2(null, null);
            }

            if (!String.IsNullOrEmpty(LocalUser.Instance.PersonalOption.MYCAR))
            {
                txtMyCarFolder.Text = LocalUser.Instance.PersonalOption.MYCAR;
            }
            else
            {
                backUpValueChanged4(null, null);
            }

            if (!String.IsNullOrEmpty(LocalUser.Instance.PersonalOption.KICC))
            {

                txt_KICC.Text = LocalUser.Instance.PersonalOption.KICC;
            }
            else
            {
                backUpValueChanged(null, null);
            }


            cmb_MenuGubun.SelectedIndex = 0;

            //LogStore

            String LogStorePath = string.Empty;
            //List<String> LogStoreDriverLoginIDLIst = new List<string>();
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();

                var Command1 = cn.CreateCommand();
                Command1.CommandText = "SELECT [DTGFolderPath]  FROM [DTGFolder]";
                var o1 = Command1.ExecuteScalar();
                if (o1 != null)
                {
                    LogStorePath = o1.ToString();
                }

                //var Command2 = cn.CreateCommand();
                //Command2.CommandText = "SELECT [LoginId] FROM [Drivers] WHERE [NeedLogStore] = 1";
                //var mReader = Command2.ExecuteReader();
                //while (mReader.Read())
                //{
                //    LogStoreDriverLoginIDLIst.Add(mReader.GetString(0));
                //}
                cn.Close();
            }

            txtDTGFolderPath.Text = LogStorePath;
            //foreach (var driverLoginID in LogStoreDriverLoginIDLIst.OrderBy(c=>c))
            //{
            //    lbxLoginID.Items.Add(driverLoginID);
            //}

            string _Contents = "";
            string _UseYn = "";
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                Connection.Open();
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText =
                    @"SELECT Contents,UseYn FROM Gongji";

                var DataReader = Command.ExecuteReader();
                if (DataReader.Read())
                {
                    //IsLogin = true;
                    //bool IsAdmin = true;
                    //String LoginId = txtID.Text;
                    _Contents = DataReader.GetString(0);
                    _UseYn = DataReader.GetString(1);
                    // LocalUser.Instance.LogInInformation = new LogInInformation(IsLogin, IsAdmin, LoginId, UserId);
                }
                Connection.Close();
            }

            if (!string.IsNullOrEmpty(_Contents))
            {

                txtGonji.Text = _Contents;
                if(_UseYn =="Y")
                {
                    chkUseYn.Checked = true;
                }
                else
                {
                    chkUseYn.Checked = false;
                }
                
            }
        }


        private void _InitCmb()
        {



            var CarstateDataSource = (from a in AddressList select new { a.State }).Distinct().ToArray();
            cmb_AddressState.DataSource = CarstateDataSource;
            cmb_AddressState.DisplayMember = "State";
            cmb_AddressState.ValueMember = "State";



            var AddressCityDataSource = SingleDataSet.Instance.AddressReferences.Select(c => new { c.City }).Distinct().ToArray();
            cmb_AddressCity.DataSource = AddressCityDataSource;
            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";

            var ParkStreetDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Where(c => c.City == cmb_AddressCity.SelectedValue.ToString()).Where(c => c.Street != "") select new { a.Street }).Distinct().ToArray();
            cmb_AddressStreet.DataSource = ParkStreetDataSource;
            cmb_AddressStreet.DisplayMember = "Street";
            cmb_AddressStreet.ValueMember = "Street";


            Dictionary<bool, string> IsTest = new Dictionary<bool, string>();

            IsTest.Add(true, "테스트");
            IsTest.Add(false, "라이브");


            cmbNiceTest.DataSource = new BindingSource(IsTest, null);
            cmbNiceTest.DisplayMember = "Value";
            cmbNiceTest.ValueMember = "Key";


        }
        private void btnAutoFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtFpisTruFolder.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtFpisTruFolder.Text = folderBrowserDialog.SelectedPath;
                backUpValueChanged(null, null);
            }
            else
                MessageBox.Show("사용자에의해 취소되었습니다.", Text);
        }

        void backUpValueChanged(object sender, EventArgs e)
        {
            LocalUser.Instance.PersonalOption.FPISTRU = txtFpisTruFolder.Text;
            LocalUser.Instance.Write();

        }

        private void btnAutoFolder1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtFpisTruFolder.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtFpisCarFolder.Text = folderBrowserDialog.SelectedPath;
                backUpValueChanged1(null, null);
            }
            else
                MessageBox.Show("사용자에의해 취소되었습니다.", Text);
        }

        void backUpValueChanged1(object sender, EventArgs e)
        {
            LocalUser.Instance.PersonalOption.FPISCAR = txtFpisCarFolder.Text;
            LocalUser.Instance.Write();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtFpisTruFolder.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtTaxFolder.Text = folderBrowserDialog.SelectedPath;
                backUpValueChanged2(null, null);
            }
            else
                MessageBox.Show("사용자에의해 취소되었습니다.", Text);
        }

        void backUpValueChanged2(object sender, EventArgs e)
        {
            LocalUser.Instance.PersonalOption.TAX = txtTaxFolder.Text;
            LocalUser.Instance.Write();

        }

        void backUpValueChanged4(object sender, EventArgs e)
        {
            LocalUser.Instance.PersonalOption.MYCAR = txtMyCarFolder.Text;
            LocalUser.Instance.Write();

        }

        void backUpValueChanged5(object sender, EventArgs e)
        {
            LocalUser.Instance.PersonalOption.KICC = txt_KICC.Text;
            LocalUser.Instance.Write();

        }

        void backUpValueChanged6(object sender, EventArgs e)
        {
            LocalUser.Instance.PersonalOption.MYBANKNEW = txtBankFolder.Text;
            LocalUser.Instance.Write();

        }

        private void FrmMN0206_SETMANAGE_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'baseDataSet.StaticOptions' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.staticOptionsTableAdapter.Fill(this.baseDataSet.StaticOptions);
            // TODO: 이 코드는 데이터를 'cmDataSet.AndroidTime' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.androidTimeTableAdapter.Fill(this.cmDataSet.AndroidTime);
            this.androidUpdateInfoesTableAdapter.Fill(this.cmDataSet.AndroidUpdateInfoes);
            this.clientsTableAdapter.Fill(this.cmDataSet.Clients);
            this.clientAddressTableAdapter.Fill(this.cmDataSet.ClientAddress);
            this.clientsTableAdapter1.Fill(this.clientDataSet.Clients);
            this.cargoApiURLTableAdapter.Fill(this.cargoDataSet.CargoApiURL);
            this.AdminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
            var Query = cmDataSet.AndroidUpdateInfoes;
            txt_Version.Text = Query.First().Version.ToString();

            var Query2 = cmDataSet.AndroidTime;
            txt_Time.Text = Query2.First().Time.ToString();

            var CargoQuery = cargoDataSet.CargoApiURL.ToArray();


            if(CargoQuery.Any())
            {
                TestUrl.Text = CargoQuery.Where(c => c.Gubun == "Test").First().ApiUrl;
                LiveUrl.Text = CargoQuery.Where(c => c.Gubun == "Live").First().ApiUrl;

                var _Gubun = CargoQuery.Where(c => c.UseYN == "Y").First().Gubun;
                if(_Gubun == "Live")
                {
                    rdoLive.Checked = true;
                }
                else
                {
                    rdoTest.Checked = true;
                }
            }

            var _Clients = clientDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

            if (_Clients.Any())
            {
                if (_Clients.First().HideMyCarOrder == true)
                {
                    cmbHideMyCarOrder.SelectedIndex = 0;
                }
                else
                {
                    cmbHideMyCarOrder.SelectedIndex = 1;
                }

                if(_Clients.First().OrderGubun == 0)
                {
                    cmbOrderGubun.SelectedIndex = 0;
                }
                else
                {
                    cmbOrderGubun.SelectedIndex = 1;
                }



                comboBox1.SelectedIndex = _Clients.First().OrderGubun;




                cmbStatsGubun.SelectedIndex = _Clients.First().StatsGubun;


                cmbHideAccountNo.SelectedIndex = _Clients.First().HideAccountNo;

            }

            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now;

            _InitCmb();

            var Query3 = cmDataSet.ClientAddress.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
            if (Query3.Any())
            {
                cmb_AddressState.Text = Query3.First().AddressState;
                cmb_AddressCity.Text = Query3.First().AddressCity;
                cmb_AddressStreet.Text = Query3.First().AddressStreet;
            }
            else
            {


            }
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            var AdminInfoesQ = baseDataSet.AdminInfoes.FirstOrDefault();

            cmbNiceTest.SelectedValue = AdminInfoesQ.IsTest;

        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                SqlCommand cmd = cn.CreateCommand();



                cmd.CommandText =
                   "UPDATE AndroidUpdateInfoes SET Version = @Version , CreateTime  =getdate()";




                cmd.Parameters.AddWithValue("@Version", txt_Version.Text);

                cmd.ExecuteNonQuery();
                cn.Close();


            }

            this.androidUpdateInfoesTableAdapter.Fill(this.cmDataSet.AndroidUpdateInfoes);


            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "어플버전", 1), "어플버전정보 수정 성공");
        }
        private string GetSelectCommand()
        {

            String SelectCommandText =
                    @"select StaticOptionId, Div, Name, Seq, Value, Number from StaticOptions ";



            return SelectCommandText;
        }
        private void DataLoad()
        {
            baseDataSet.StaticOptions.Clear();

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();



                    WhereStringList.Add($" Div = @Div");
                    _Command.Parameters.AddWithValue("@Div", "AccGubun");

                    WhereStringList.Add($" Value != 0");


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;
                    }
                    _Command.CommandText = SelectCommandText + " order by Seq";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        baseDataSet.StaticOptions.Load(_Reader);
                    }
                }
                _Connection.Close();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    tableLayoutPanel3.Visible = false;
                    MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    tabControl1.SelectTab(0);
                }
                else
                {
                    tableLayoutPanel3.Visible = true;
                }
            }

            else if (tabControl1.SelectedIndex == 2)
            {
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    tableLayoutPanel3.Visible = false;
                    MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    tabControl1.SelectTab(0);
                }
                else
                {
                    DataLoad();


                }
            }
        }

        private void txt_ClientName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            ClientName_Serach(txt_ClientCode.Text);
        }

        private void ClientName_Serach(string SName)
        {
            var Query = cmDataSet.Clients.Where(c => c.Code == SName);

            if (Query.Any())
            {
                lbl_ClientName.Text = Query.First().Name;
                lbl_ClientId.Text = Query.First().ClientId.ToString();
                SLoginID = Query.First().LoginId;
            }
        }

        private void cmb_MenuGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_MenuGubun.SelectedIndex > 5 || cmb_MenuGubun.SelectedIndex == 0)
            {
                dtpStart.Enabled = true;
                dtpEnd.Enabled = true;
            }
            else
            {
                dtpStart.Enabled = false;
                dtpEnd.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClientName_Serach(txt_ClientCode.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_ClientCode.Text) || String.IsNullOrEmpty(lbl_ClientName.Text))
            {
                MessageBox.Show("운송사코드가 입력되지않았습니다.", "운송사코드 입력.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txt_ClientCode.Focus();
                return;
            }

            if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문2, cmb_MenuGubun.Text, 0), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;




            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                SqlCommand cmd = cn.CreateCommand();

                switch (cmb_MenuGubun.Text)
                {

                    case "차주관리":
                        DriverRepository mDriverRepository = new DriverRepository();
                        mDriverRepository.AllDelete(int.Parse(lbl_ClientId.Text));
                        break;

                    case "ID관리":
                        cmd.CommandText =
                                "DELETE ClientUsers WHERE ClientId = @ClientId";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);
                        break;

                    case "화물접수":
                        cmd.CommandText =
                                "DELETE  Orders WHERE ClientId= @ClientId and CONVERT(varchar(10),CreateTime,111) >= @Sdate AND CONVERT(varchar(10),CreateTime,111) <= @Edate ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);
                        cmd.Parameters.AddWithValue("@Sdate", dtpStart.Text);
                        cmd.Parameters.AddWithValue("@Edate", dtpEnd.Text);
                        break;
                    case "기사관리":
                        cmd.CommandText =
                                "DELETE  DriverAdd WHERE ClientId= @ClientId ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);

                        break;

                    case "거래처관리":
                        cmd.CommandText =
                                "DELETE  Customers WHERE ClientId= @ClientId ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);

                        break;

                    case "계정과목":
                        cmd.CommandText =
                                "DELETE  ChargeAccounts WHERE ClientId= @ClientId ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);

                        break;


                    case "매입관리":
                        cmd.CommandText =
                                "DELETE  Trades WHERE ClientId= @ClientId and CONVERT(varchar(10),RequestDate,111) >= @Sdate AND CONVERT(varchar(10),RequestDate,111) <= @Edate ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);
                        cmd.Parameters.AddWithValue("@Sdate", dtpStart.Text);
                        cmd.Parameters.AddWithValue("@Edate", dtpEnd.Text);
                        break;

                    case "매출관리":
                        cmd.CommandText =
                                "DELETE  SalesManage WHERE ClientId= @ClientId and CONVERT(varchar(10),RequestDate,111) >= @Sdate AND CONVERT(varchar(10),RequestDate,111) <= @Edate ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);
                        cmd.Parameters.AddWithValue("@Sdate", dtpStart.Text);
                        cmd.Parameters.AddWithValue("@Edate", dtpEnd.Text);
                        break;


                    case "실적신고":
                        cmd.CommandText =
                                "DELETE  FPIS_FILE WHERE ClientId= @ClientId and CONVERT(varchar(10),CREATE_DATE,111) >= @Sdate AND CONVERT(varchar(10),CREATE_DATE,111) <= @Edate " +
                                "DELETE  FPIS_FILE_CAR WHERE ClientId= @ClientId and CONVERT(varchar(10),CREATE_DATE,111) >= @Sdate AND CONVERT(varchar(10),CREATE_DATE,111) <= @Edate ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);
                        cmd.Parameters.AddWithValue("@Sdate", dtpStart.Text);
                        cmd.Parameters.AddWithValue("@Edate", dtpEnd.Text);
                        break;

                    case "경비출납":
                        cmd.CommandText =
                                "DELETE  Charges_Clients WHERE ClientId= @ClientId and CONVERT(varchar(10),ApplyDate,111) >= @Sdate AND CONVERT(varchar(10),ApplyDate,111) <= @Edate ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);
                        cmd.Parameters.AddWithValue("@Sdate", dtpStart.Text);
                        cmd.Parameters.AddWithValue("@Edate", dtpEnd.Text);
                        break;

                    case "전체":
                        cmd.CommandText =
                                //" DELETE Drivers WHERE CandidateId = @ClientId " +
                                " UPDATE Drivers SET ServiceState = 5 WHERE CandidateId = @ClientId" +
                                " DELETE DriverGroups WHERE  ClientId = @ClientId " +
                                " UPDATE VAccountPools SET DriverId = NULL WHERE ClientId = @ClientId ";

                        cmd.CommandText += " DELETE FROM DriverInstances WHERE ClientId = @ClientId ";

                        cmd.CommandText +=
                               "  DELETE ClientUsers WHERE ClientId = @ClientId";
                        cmd.CommandText +=
                              "  DELETE  Orders WHERE ClientId= @ClientId and CONVERT(varchar(10),CreateTime,111) >= @Sdate AND CONVERT(varchar(10),CreateTime,111) <= @Edate ";
                        cmd.CommandText +=
                                "  DELETE  Trades WHERE ClientId= @ClientId and CONVERT(varchar(10),RequestDate,111) >= @Sdate AND CONVERT(varchar(10),RequestDate,111) <= @Edate ";
                        cmd.CommandText +=
                                "  DELETE  DriverAdd WHERE ClientId= @ClientId ";
                        cmd.CommandText +=
                                "  DELETE  Customers WHERE ClientId= @ClientId ";
                        cmd.CommandText +=
                                "  DELETE  ChargeAccounts WHERE ClientId= @ClientId ";
                        cmd.CommandText +=
                                "  DELETE  SalesManage WHERE ClientId= @ClientId and CONVERT(varchar(10),RequestDate,111) >= @Sdate AND CONVERT(varchar(10),RequestDate,111) <= @Edate ";
                        cmd.CommandText +=
                                "  DELETE  FPIS_FILE WHERE ClientId= @ClientId and CONVERT(varchar(10),CREATE_DATE,111) >= @Sdate AND CONVERT(varchar(10),CREATE_DATE,111) <= @Edate " +
                                "  DELETE  FPIS_FILE_CAR WHERE ClientId= @ClientId and CONVERT(varchar(10),CREATE_DATE,111) >= @Sdate AND CONVERT(varchar(10),CREATE_DATE,111) <= @Edate ";
                        cmd.Parameters.AddWithValue("@ClientId", lbl_ClientId.Text);
                        cmd.Parameters.AddWithValue("@Sdate", dtpStart.Text);
                        cmd.Parameters.AddWithValue("@Edate", dtpEnd.Text);

                        break;
                }



                cmd.ExecuteNonQuery();
                cn.Close();


            }
            try
            {



                MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공2, cmb_MenuGubun.Text, 0), cmb_MenuGubun.Text + " 삭제 성공");
            }
            catch
            {

            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtFpisTruFolder.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtMyCarFolder.Text = folderBrowserDialog.SelectedPath;
                backUpValueChanged4(null, null);
            }
            else
                MessageBox.Show("사용자에의해 취소되었습니다.", Text);
        }



        private void btnAcceptStorePath_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDTGFolderPath.Text))
            {
                MessageBox.Show("저장폴더를 입력해주십시오.");
                return;
            }
            String LogPath = txtDTGFolderPath.Text;
            if (LogPath.EndsWith(@"\"))
            {
                LogPath = LogPath.Substring(0, LogPath.Length - 1);
            }

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();

                bool IsExist = false;
                var Command1 = cn.CreateCommand();
                Command1.CommandText = @"SELECT COUNT(*) FROM [DTGFolder]";
                var o1 = Command1.ExecuteScalar();
                if (o1 != null)
                {
                    if (Convert.ToInt32(o1) > 0)
                    {
                        IsExist = true;
                    }
                }
                if (!IsExist)
                {
                    var Command3 = cn.CreateCommand();
                    Command3.CommandText = String.Format("INSERT INTO [DTGFolder] VALUES ('{0}')", LogPath);
                    Command3.ExecuteNonQuery();

                }
                else
                {
                    var Command3 = cn.CreateCommand();
                    Command3.CommandText = String.Format("UPDATE [DTGFolder] SET [DTGFolderPath] = '{0}'", LogPath);
                    Command3.ExecuteNonQuery();

                }
                cn.Close();
                MessageBox.Show("저장되었습니다.");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtDTGFolderPath.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDTGFolderPath.Text = folderBrowserDialog.SelectedPath;
                // backUpValueChanged(null, null);
            }
            else
                MessageBox.Show("사용자에의해 취소되었습니다.", Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txt_KICC.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txt_KICC.Text = folderBrowserDialog.SelectedPath;

            }
            else
                MessageBox.Show("사용자에의해 취소되었습니다.", Text);





        }

        private void button7_Click(object sender, EventArgs e)
        {
            backUpValueChanged5(null, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                SqlCommand cmd = cn.CreateCommand();



                cmd.CommandText =
                   "UPDATE AndroidTime SET Time = @Version , CreateTime  =getdate()";




                cmd.Parameters.AddWithValue("@Version", txt_Time.Text);

                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void cmb_AddressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_AddressCity.Enabled = true;




            var ParkCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            cmb_AddressCity.DataSource = ParkCityDataSource;
            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";
        }

        private void cmb_AddressCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_AddressStreet.Enabled = true;
            var ParkStreetDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Where(c => c.City == cmb_AddressCity.SelectedValue.ToString()).Where(c => c.Street != "").Select(c => new { c.Street }).Distinct().ToArray();
            cmb_AddressStreet.DataSource = ParkStreetDataSource;
            cmb_AddressStreet.DisplayMember = "Street";
            cmb_AddressStreet.ValueMember = "Street";
        }

        private void btn_AddressOk_Click(object sender, EventArgs e)
        {

            var Query = cmDataSet.ClientAddress.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

            if (Query.Any())
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {


                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();



                    cmd.CommandText =
                       "UPDATE ClientAddress SET AddressState = @AddressState , AddressCity  =@AddressCity, AddressStreet = @AddressStreet" +
                       " WHERE Clientid = @ClientId";




                    cmd.Parameters.AddWithValue("@AddressState", cmb_AddressState.SelectedValue);
                    cmd.Parameters.AddWithValue("@AddressCity", cmb_AddressCity.SelectedValue);
                    cmd.Parameters.AddWithValue("@AddressStreet", cmb_AddressStreet.SelectedValue);
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

                try
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "행정구역", 1), "행정구역 수정 성공");
                }

                catch (Exception ed)
                {
                    MessageBox.Show(ed.Message, "행정구역 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {


                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();



                    cmd.CommandText =
                       "INSERT INTO ClientAddress (AddressState,AddressCity,AddressStreet,ClientId)" +
                       " VALUES(@AddressState,@AddressCity,@AddressStreet,@ClientId)";

                    cmd.Parameters.AddWithValue("@AddressState", cmb_AddressState.SelectedValue);
                    cmd.Parameters.AddWithValue("@AddressCity", cmb_AddressCity.SelectedValue);
                    cmd.Parameters.AddWithValue("@AddressStreet", cmb_AddressStreet.SelectedValue);
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);



                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

                try
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "행정구역", 1), "행정구역 등록 성공");
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "행정구역 등록 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtBankFolder.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtBankFolder.Text = folderBrowserDialog.SelectedPath;
                backUpValueChanged4(null, null);
            }
            else
                MessageBox.Show("사용자에의해 취소되었습니다.", Text);
        }

        string SaveGubun = "";
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            if (dataGridView1.RowCount > 0)
            {
                var Query = baseDataSet.StaticOptions.Where(c => c.Value != 0).OrderByDescending(c => c.Seq);

                nudSeq.Value = Query.First().Seq + 1;
            }

            btnSave.Text = "저장";
            SaveGubun = "NEW";
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("계정과목을 입력하세요.", "계정과목", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtName.Focus();
                return;
            }
            if (nudSeq.Value == 0)
            {
                MessageBox.Show("0이 아닌 순번을 입력하세요.", "계정과목", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                nudSeq.Focus();
                return;
            }
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                if (SaveGubun == "NEW")
                {
                    SqlCommand cmd = cn.CreateCommand();



                    cmd.CommandText =
                       "INSERT INTO StaticOptions (Div, Name, Seq, Value)" +
                       " VALUES(@Div, @Name, @Seq, @Value)";

                    cmd.Parameters.AddWithValue("@Div", "AccGubun");
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Seq", nudSeq.Value);
                    cmd.Parameters.AddWithValue("@Value", nudSeq.Value);



                    cmd.ExecuteNonQuery();
                }
                else if (SaveGubun == "UPDATE")
                {
                    if (staticOptionsBindingSource.Current != null)
                    {

                        var Selected = ((DataRowView)staticOptionsBindingSource.Current).Row as BaseDataSet.StaticOptionsRow;



                        SqlCommand Upcmd = cn.CreateCommand();



                        Upcmd.CommandText =
                           "Update StaticOptions SET Name = @Name, Seq = @Seq " +
                           " WHERE StaticOptionId = @StaticOptionId AND Div = @Div";

                        Upcmd.Parameters.AddWithValue("@Div", "AccGubun");
                        Upcmd.Parameters.AddWithValue("@Name", txtName.Text);
                        Upcmd.Parameters.AddWithValue("@Seq", nudSeq.Value);

                        Upcmd.Parameters.AddWithValue("@StaticOptionId", Selected.StaticOptionId);



                        Upcmd.ExecuteNonQuery();
                    }
                }

                cn.Close();
            }

            try
            {
                MessageBox.Show("저장되었습니다.", "계정과목", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void staticOptionsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            SaveGubun = "UPDATE";
            btnSave.Text = "수정";
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;



            if (e.ColumnIndex == No.Index)
            {
                e.Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (LocalUser.Instance.LogInInformation.IsAdmin)
                return;



            var _HideMyCarOrder = false;
            if (cmbHideMyCarOrder.SelectedIndex == 0)
            {
                _HideMyCarOrder = true;
            }
            else
            {
                _HideMyCarOrder = false;
            }


            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET HideMyCarOrder = @HideMyCarOrder" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@HideMyCarOrder", _HideMyCarOrder);
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

        }



        private void cmbOrderGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (LocalUser.Instance.LogInInformation.IsAdmin)
            //    return;



            var _OrderGubun = 0;
            if (cmbOrderGubun.SelectedIndex == 0)
            {
                _OrderGubun = 0;
            }
            else
            {
                _OrderGubun = 1;
            }


            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET OrderGubun = @OrderGubun" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@OrderGubun", _OrderGubun);
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

        }

        private void cmbStatsGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _StatsGubun = 0;
            _StatsGubun = cmbStatsGubun.SelectedIndex;
           

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET StatsGubun = @StatsGubun" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@StatsGubun", _StatsGubun);
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

        }

        private void btnStatsPreview_Click(object sender, EventArgs e)
        {
            string _Gubun = "1";

            switch (cmbStatsGubun.SelectedIndex)
            {
                case 0:
                    _Gubun = "1";
                    break;
                case 1:
                    _Gubun = "2";
                    break;
                case 2:
                    _Gubun = "3";
                    break;
                case 3:
                    _Gubun = "4";
                    break;
                case 4:
                    _Gubun = "5";
                    break;

            }
            FrmStatsPreview f = new FrmStatsPreview(_Gubun);
            f.Owner = this;
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var _OrderGubun = 0;
            if (comboBox1.SelectedIndex == 0)
            {
                _OrderGubun = 0;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                _OrderGubun = 1;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                _OrderGubun = 2;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                _OrderGubun = 3;
            }

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET OrderGubun = @OrderGubun" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@OrderGubun", _OrderGubun);
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void cmbHideAccountNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _HideAccountNo = 1;
            if (cmbHideAccountNo.SelectedIndex == 0)
            {
                _HideAccountNo = 0;
            }
            else
            {
                _HideAccountNo = 1;
            }


            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE Clients SET HideAccountNo = @HideAccountNo" +
                   " WHERE Clientid = @ClientId";
                cmd.Parameters.AddWithValue("@HideAccountNo", _HideAccountNo);
                cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

        }

        //TestUrl
        private void button11_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                SqlCommand cmd = cn.CreateCommand();



                cmd.CommandText =
                   "UPDATE CargoApiURL SET ApiUrl = @ApiUrl WHERE Gubun = 'Test'";




                cmd.Parameters.AddWithValue("@ApiUrl", TestUrl.Text);

                cmd.ExecuteNonQuery();
                cn.Close();


            }

            this.cargoApiURLTableAdapter.Fill(this.cargoDataSet.CargoApiURL);


            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "화물맨API", 1), "화물맨API TestUrl 수정 성공");
        }
        //LiveUrl
        private void button12_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                SqlCommand cmd = cn.CreateCommand();



                cmd.CommandText =
                   "UPDATE CargoApiURL SET ApiUrl = @ApiUrl WHERE Gubun = 'Live'";




                cmd.Parameters.AddWithValue("@ApiUrl", LiveUrl.Text);

                cmd.ExecuteNonQuery();
                cn.Close();


            }

            this.cargoApiURLTableAdapter.Fill(this.cargoDataSet.CargoApiURL);


            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "화물맨API", 1), "화물맨API LiveUrl 수정 성공");
        }

        private void rdoLive_CheckedChanged(object sender, EventArgs e)
        {
           
           
            string _LiveYN = "Y";
            string _TestYN = "N";
            if (rdoLive.Checked)
            {
                rdoTest.Checked = false;
                _LiveYN = "Y";
                _TestYN = "N";
            }
            else
            {
                rdoLive.Checked = false;
                _LiveYN = "N";
                _TestYN = "Y";
            }
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText =
                   "UPDATE CargoApiURL SET UseYN = @LiveYN" +
                   " WHERE Gubun = 'Live'";

                cmd.CommandText +=
                  " UPDATE CargoApiURL SET UseYN = @TestYN" +
                  " WHERE Gubun = 'Test'";

                cmd.Parameters.AddWithValue("@LiveYN", _LiveYN);
                cmd.Parameters.AddWithValue("@TestYN", _TestYN);

                cmd.ExecuteNonQuery();
                cn.Close();
            }


        }

        private void button13_Click(object sender, EventArgs e)
        {

            string _UseYn = "Y";

           if(chkUseYn.Checked)
            {
                _UseYn = "Y";

            }
           else
            {
                _UseYn = "N";
            }
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                SqlCommand cmd = cn.CreateCommand();



                cmd.CommandText =
                   "UPDATE Gongji SET Contents = @Contents,UseYn = @UseYn";




                cmd.Parameters.AddWithValue("@Contents", txtGonji.Text);



                cmd.Parameters.AddWithValue("@UseYn", _UseYn);

                cmd.ExecuteNonQuery();
                cn.Close();


            }

            try
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "공지사항수정", 1), "공지사항 수정 성공");
            }
            catch
            {

            }
        }
        DTIServiceClass dTIServiceClass = new DTIServiceClass();
        //NiceDnr
        private void button14_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);

            var Query = baseDataSet.AdminInfoes.FirstOrDefault();



            //var Query = 
            var Result = dTIServiceClass.Memberjoin(Query.Linkcd, Query.LinkId, Query.BIzNo, Query.CustName, Query.ownerName, Query.BizCond, Query.bizItem, Query.rsbmName, Query.email, Query.telNo, Query.hpNo, Query.zipCoe, Query.addr1, Query.addr2);

            if (!String.IsNullOrEmpty(Result))
            {
                var ResultList = Result.Split('/');

                try
                {
                    //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                    var retVal = ResultList[0];
                    var errMsg = ResultList[1];
                    var frnNo = ResultList[2];
                    var userid = ResultList[3];
                    var passwd = ResultList[4];
                    
                    if(retVal == "0")
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {


                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();



                            cmd.CommandText =
                               "UPDATE AdminInfoes SET frnNo = @frnNo , userid  =@userid,passwd = @passwd";




                            cmd.Parameters.AddWithValue("@frnNo", frnNo);
                            cmd.Parameters.AddWithValue("@userid", userid);
                            cmd.Parameters.AddWithValue("@passwd", passwd);

                            cmd.ExecuteNonQuery();
                            cn.Close();


                        }
                    }
                    else
                    {
                        MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);

                    }
                        


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }


        }

        private void button15_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);

            var Query = baseDataSet.AdminInfoes.FirstOrDefault();

            var Result = dTIServiceClass.SelectMembInfo(Query.Linkcd, Query.frnNo, Query.userid, Query.passwd);

            if (!String.IsNullOrEmpty(Result))
            {
                var ResultList = Result.Split('/');

                try
                {
                    //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                    var retVal = ResultList[0];
                    var errMsg = ResultList[1];
                    var frnNo = ResultList[2];
                    var userid = ResultList[3];
                    var passwd = ResultList[4];
                    var Results = ResultList[5];

                    if (retVal == "0")
                    {
                        txtfrnNo.Text = frnNo;
                        txtuserId.Text = userid;
                        txtpasswd.Text = passwd;
                        textBox1.Text = Results;
                    }
                    else
                    {
                        MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);

                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            var Query = baseDataSet.AdminInfoes.FirstOrDefault();

            var Result = dTIServiceClass.GetMembJoinInf(Query.Linkcd, Query.LinkId, Query.BIzNo, Query.rsbmName, Query.email, Query.hpNo);

            if (!String.IsNullOrEmpty(Result))
            {
                var ResultList = Result.Split('/');

                try
                {
                    //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                    var retVal = ResultList[0];
                    var errMsg = ResultList[1];
                    var frnNo = ResultList[2];
                    var userid = ResultList[3];
                    var passwd = ResultList[4];

                    if (retVal == "0")
                    {
                        txtfrnNo.Text = frnNo;
                        txtuserId.Text = userid;
                        txtpasswd.Text = passwd;
                    }
                    else
                    {
                        MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);

                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }
        //인증서등록
        private void button18_Click(object sender, EventArgs e)
        {



            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            var Query = baseDataSet.AdminInfoes.FirstOrDefault();
            string url = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do?";
            if (Query.IsTest != true)
            {
                url = "http://www.nicedata.co.kr/ti/TI_80101.do?";
            }
            
            string frnNo = "";
            string userid = "";
            string passwd = "";
            string Linkcd = "";

            try
            {
                frnNo = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.frnNo }).ToString();
                userid = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.userid }).ToString();
                passwd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.passwd }).ToString();
                Linkcd = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { Query.Linkcd }).ToString();


                //MessageBox.Show(Encode.ToString());

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);

            }

            string value = $"frnNo={frnNo}&userId={userid}&passwd={passwd}&linkCd={Linkcd}&retUrl=";

            FrmNice f = new FrmNice(value, url);
            f.Show();


        }

        private void button19_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            var Query = baseDataSet.AdminInfoes.FirstOrDefault();

            var Result = dTIServiceClass.selectExpireDt(Query.Linkcd, Query.frnNo, Query.userid, Query.passwd);

            if (!String.IsNullOrEmpty(Result))
            {
                var ResultList = Result.Split('/');

                try
                {
                    //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                    var retVal = ResultList[0];
                    var errMsg = ResultList[1];
                    var regYn = ResultList[2];
                    var expireDt = ResultList[3];
                   

                    if (retVal == "0")
                    {
                        //txtfrnNo.Text = frnNo;
                        //txtuserId.Text = userid;
                        //txtpasswd.Text = passwd;
                        MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);
                    }
                    else
                    {
                        MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);

                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            AdminInfoesTableAdapter.Fill(baseDataSet.AdminInfoes);
            var Query = baseDataSet.AdminInfoes.FirstOrDefault();

            var _Q = clientDataSet.Clients.Where(c => c.ClientId == 377).FirstOrDefault();

            var Result = dTIServiceClass.updateMembInfo(Query.Linkcd, Query.frnNo, Query.userid, Query.passwd,_Q.Name,_Q.CEO,_Q.Uptae,_Q.Upjong, textBox3.Text,_Q.Email,_Q.PhoneNo,_Q.MobileNo,_Q.ZipCode,_Q.AddressState +" "+ _Q.AddressCity,_Q.AddressDetail);

            if (!String.IsNullOrEmpty(Result))
            {
                var ResultList = Result.Split('/');

                try
                {
                    //return retVal + " / " + errMsg + " / " + frnNo + " / " + userid + " / " + passwd;

                    var retVal = ResultList[0];
                    var errMsg = ResultList[1];
               


                    if (retVal == "0")
                    {
                        using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {


                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();



                            cmd.CommandText =
                               "UPDATE AdminInfoes SET BizNo = @BizNo ,  CustName = @CustName, ownerName = @ownerName, BizCond = @BizCond, bizItem = @bizItem, rsbmName = @rsbmName, email = @email, telNo = @telNo, hpNo = @hpNo, zipCoe = @zipCoe, addr1 = @addr1, addr2 = @addr2 ";




                            cmd.Parameters.AddWithValue("@BizNo", _Q.BizNo);
                            cmd.Parameters.AddWithValue("@CustName", _Q.Name);
                            cmd.Parameters.AddWithValue("@ownerName", _Q.CEO);

                            cmd.Parameters.AddWithValue("@BizCond", _Q.Uptae);
                            cmd.Parameters.AddWithValue("@bizItem", _Q.Upjong);
                            cmd.Parameters.AddWithValue("@rsbmName", textBox3.Text);

                            cmd.Parameters.AddWithValue("@email", _Q.Email);
                            cmd.Parameters.AddWithValue("@telNo", _Q.PhoneNo);
                            cmd.Parameters.AddWithValue("@hpNo", _Q.MobileNo);

                            cmd.Parameters.AddWithValue("@zipCoe", _Q.ZipCode);
                            cmd.Parameters.AddWithValue("@addr1", _Q.AddressState + " " + _Q.AddressCity);
                            cmd.Parameters.AddWithValue("@addr2", _Q.AddressDetail);

                            cmd.ExecuteNonQuery();
                            cn.Close();


                        }
                    }
                    else
                    {
                        MessageBox.Show("retVal  : " + retVal + "\r\nerrMsg" + errMsg);

                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                cn.Open();
                SqlCommand cmd = cn.CreateCommand();



                cmd.CommandText =
                   "UPDATE AdminInfoes SET IsTest = @IsTest";




               
                cmd.Parameters.AddWithValue("@IsTest", cmbNiceTest.SelectedValue);

                cmd.ExecuteNonQuery();
                cn.Close();


            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(textBox2.Text))
            {
                return;
            }
            string sParameter = "";
            string EncodingSparameter = "";

            // var Selected = ((DataRowView)salesManageBindingSource.Current).Row as SalesDataSet.SalesManageRow;



            //if (Selected != null)
            //{
            //현재시간(년월일시분초);공급자사업자번호;공급받는자사업자번호;연계코드
            sParameter = DateTime.Now.ToString("yyyyMMddHHmmss") + ";" + textBox2.Text.Replace("-", "") + ";" + "" + ";" + "EDB";
            EncodingSparameter = webBrowser1.Document.InvokeScript("niceEncodingString", new Object[] { sParameter }).ToString();


            // url = "http://t-renewal.nicedata.co.kr/ti/TI_80401.do?";


            string value = $"data={EncodingSparameter}";

            AdminInfoesTableAdapter.Fill(this.baseDataSet.AdminInfoes);
            var Query = baseDataSet.AdminInfoes.FirstOrDefault();
            var url = "http://t-renewal.nicedata.co.kr/ti/TI_80401.do?";
            if (Query.IsTest != true)
            {
                url = "http://www.nicedata.co.kr/ti/TI_80401.do?";
            }


            //FrmNice f = new FrmNice(value, url);
            //f.Show();

            Process.Start(url + value);

        }
    }
}
