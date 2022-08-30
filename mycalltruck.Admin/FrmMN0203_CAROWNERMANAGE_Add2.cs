using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace mycalltruck.Admin
{
    public partial class FrmMN0203_CAROWNERMANAGE_Add2 : Form
    {
        public FrmMN0203_CAROWNERMANAGE_Add2()
        {
            InitializeComponent();

            _InitCmb();
            cmb_Search.SelectedIndex = 0;
            cmb_Group.SelectedIndex = 0;

            

            dtp_RequestFrom.Value = DateTime.Now;
            dtp_RequestTo.Value = DateTime.Now.AddMonths(3);
        }
        private void FrmMN0203_CAROWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            LoadAllDriver();
            btn_Search_Click(null, null);
        }

        private void LoadAllDriver()
        {
            String _SelectCommandText =
                @"SELECT    Drivers.DriverId, Drivers.Name, Drivers.MobileNo, Drivers.CarYear,
                        Drivers.CarNo, Drivers.CarType, Drivers.CarSize, Drivers.ParkState, Drivers.ParkCity, Drivers.ParkStreet
                    FROM    Drivers INNER JOIN Clients ON Drivers.CandidateId = Clients.ClientId";
            try
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        String SelectCommandText = _SelectCommandText;

                        List<String> WhereStringList = new List<string>();
                        //// 1. 본점/지사
                        //if (LocalUser.Instance.LogInInformation.IsSubClient)
                        //{
                        //    if (LocalUser.Instance.LogInInformation.IsAgent)
                        //    {
                        //        WhereStringList.Add("Drivers.ClientUserId <> @ClientUserId  OR Drivers.ClientUserId IS NULL");
                        //        _Command.Parameters.AddWithValue("@ClientUserId", LocalUser.Instance.LogInInformation.ClientUserId);
                        //    }
                        //    else
                        //    {
                        //        WhereStringList.Add("Drivers.SubClientId <> @SubClientId  OR Drivers.SubClientId IS NULL");
                        //        _Command.Parameters.AddWithValue("@SubClientId", LocalUser.Instance.LogInInformation.SubClientId);
                        //    }
                        //}
                        //else 
                        
                        if (!LocalUser.Instance.LogInInformation.IsAdmin)
                        {
                            // 타사공유
                            WhereStringList.Add("Drivers.CandidateId <> @ClientId");
                            _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        }
                        if (WhereStringList.Count > 0)
                        {
                            var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                            SelectCommandText += Environment.NewLine + WhereString;
                        }
                        _Command.CommandText = SelectCommandText + " Order by Drivers.CreateDate DESC";
                        using (SqlDataReader _Reader = _Command.ExecuteReader())
                        {
                            try
                            {
                                baseDataSet.EnforceConstraints = false;
                                baseDataSet.Drivers.Load(_Reader);
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }
                    }
                    _Connection.Close();
                }
            }
            catch
            {
                MessageBox.Show("조회가 실패하였습니다. 잠시 후에 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
       
        private void _InitCmb()
        {
            Dictionary<string, string> NotificationGroup = new Dictionary<string, string>();
            NotificationGroup.Add("0", "미설정");
            NotificationGroup.Add("A", "A");
            NotificationGroup.Add("B", "B");
            NotificationGroup.Add("C", "C");

            cmb_Group.DataSource = new BindingSource(NotificationGroup, null);
            cmb_Group.DisplayMember = "Value";
            cmb_Group.ValueMember = "Key";
        }

        private void driversBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (driversBindingSource == null) return;
                if (driversBindingSource.Current == null) return;
                DataRowView view = driversBindingSource.Current as DataRowView;

                if (view == null) return;
                BaseDataSet.DriversRow row = view.Row as BaseDataSet.DriversRow;
                if (row == null) return;

                var SizeName = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == row.CarSize && c.Div == "CarSize").Select(c => new { c.Name }).ToArray();

                txt_CarSize.Text = SizeName.First().Name;

                var TypeName = SingleDataSet.Instance.StaticOptions.Where(c => c.Value == row.CarType && c.Div == "CarType").Select(c => new { c.Name }).ToArray();

                txt_CarType.Text = TypeName.First().Name;

                txt_ParkState.Text = row.ParkState + " " + row.ParkCity + " " + row.ParkStreet;

                chk_Cont.Checked = false;

                txt_MobileNo.Text = row.MobileNo.Substring(0, 4) + "****-****";
                ;
            }
            catch { }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void _Search()
        {
            try
            {
                String FilterString = "";
                if (!String.IsNullOrEmpty(txt_Search.Text))
                {
                    String _SearchFitlerString = "";
                    if (cmb_Search.Text == "차량번호")
                    {
                        _SearchFitlerString = $"CarNo Like  '%{txt_Search.Text}%'";
                    }
                    else if (cmb_Search.Text == "기사명")
                    {
                        _SearchFitlerString = $"CarYear Like  '%{txt_Search.Text}%'";
                    }
                    else if (cmb_Search.Text == "상호")
                    {
                        _SearchFitlerString = $"Name Like  '%{txt_Search.Text}%'";
                    }
                    if (String.IsNullOrEmpty(FilterString))
                        FilterString = _SearchFitlerString;
                    else
                        FilterString += $" AND {_SearchFitlerString}";
                }
                driversBindingSource.Filter = FilterString;
            }
            catch
            {
            }
        }

        public bool IsSuccess = false;

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            
        }

        private int _UpdateDB()
        {
            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Group") iName = "그룹설정";
               
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }

        private void _AddClient()
        {
            if (driversBindingSource == null) return;
            if (driversBindingSource.Current == null) return;
            DataRowView view = driversBindingSource.Current as DataRowView;

            if (view == null) return;
            BaseDataSet.DriversRow driver = view.Row as BaseDataSet.DriversRow;
            if (driver == null) return;


        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            btn_Search_Click(null, null);
        }

        private void btnAddClose_Click_1(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "그룹설정", 1), "그룹설정 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "그룹설정", 1), "그룹설정 성공");
                IsSuccess = true;

                cmb_Group.SelectedIndex = 0;
                btn_new_Click(null, null);
            }
            else
            {

            }
        }

        private void chk_Cont_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Cont.Checked == true)
            {
                dtp_RequestFrom.Visible = true;
                dtp_RequestTo.Visible = true;
            }
            else
            {
                dtp_RequestFrom.Visible = false;
                dtp_RequestTo.Visible = false;
            }
        }
    }
}
