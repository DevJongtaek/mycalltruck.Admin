using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Models;
using mycalltruck.Admin.UI;
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
    public partial class FRMMNNOTICEDRIVER : Form
    {
        string Up_Status = string.Empty;
        List<AddressViewModel> AddressList = new List<AddressViewModel>();
        int GridIndex = 0;
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
        public FRMMNNOTICEDRIVER()
        {
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
            InitializeComponent();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            _InitCmb();
        }
        private void FRMMNNOTICEDRIVER_Load(object sender, EventArgs e)
        {
            
            // TODO: 이 코드는 데이터를 'cMDataSet.NOTICEDRIVER' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.nOTICEDRIVERTableAdapter.Fill(this.cMDataSet.NOTICEDRIVER);
            InitCompareTable();
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
            cmb_CarSizeS.SelectedIndex = 0;
            cmb_CarTypeS.SelectedIndex = 0;
            btn_Search_Click(null, null);

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {

                btn_New.Enabled = true;
                

            }
            else
            {
                btn_New.Enabled = false;

            }

        }

        private List<CompareItem> CompareTable = new List<CompareItem>();
        private void InitCompareTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand commnad = connection.CreateCommand();
                commnad.CommandText = "SELECT ClientId, Code, Name, BizNo, BizType FROM Clients";
                connection.Open();
                var dataReader = commnad.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    CompareTable.Add(
                      new CompareItem
                      {
                          ClientId = dataReader.GetInt32(0),
                          Code = dataReader.GetString(1),
                          Name = dataReader.GetString(2),
                          BizNo = dataReader.GetString(3),
                          BizType = dataReader.GetInt32(4),
                      });
                }
            }
        }

        class CompareItem
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string BizNo { get; set; }
            public int BizType { get; set; }
        }


        private void btn_New_Click(object sender, EventArgs e)
        {
            //  clientsTableAdapter.Fill(this.cMDataSet.Clients);

            FRMMNNOTICEDRIVER_ADD _Form = new FRMMNNOTICEDRIVER_ADD();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();

            btn_Search_Click(null, null);

        }
        private void _InitCmb()
        {

            var AddressStateDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.State }).Distinct().ToArray();
            cmb_AddressState.DataSource = AddressStateDataSource;
            cmb_AddressState.DisplayMember = "State";
            cmb_AddressState.ValueMember = "State";

            //var AddressCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            //cmb_AddressCity.DataSource = AddressCityDataSource;

            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";


            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
               

                SqlDataAdapter da = new SqlDataAdapter("select ' 시도'  as state union select distinct state from AddressReferences", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmb_AddressState_I.DataSource = dt;
                cmb_AddressState_I.DisplayMember = "State";
                cmb_AddressState_I.ValueMember = "State";
            }


            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                SqlDataAdapter da = new SqlDataAdapter("select ' 시군구'  as City union select distinct City from AddressReferences WHERE State = '" + cmb_AddressState_I.SelectedValue.ToString() + "'", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmb_AddressCity_I.DataSource = dt;
                cmb_AddressCity_I.DisplayMember = "City";
                cmb_AddressCity_I.ValueMember = "City";
            }



            //var AddressCityDataSource_I = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState_I.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            //cmb_AddressCity_I.DataSource = AddressCityDataSource_I;
            //cmb_AddressCity_I.DisplayMember = "City";
            //cmb_AddressCity_I.ValueMember = "City";




            cmb_CarType.DisplayMember = "Name";
            cmb_CarType.ValueMember = "Value";


            var CarSizeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value != 0 && c.Value != 99).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarSize.DataSource = CarSizeDataSource;
            cmb_CarSize.DisplayMember = "Name";
            cmb_CarSize.ValueMember = "Value";
            cmb_CarSize.SelectedIndex = 0;
            cmb_CarSize_SelectedIndexChanged(null, null);

            var CarTypeSDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarTypeS.DataSource = CarTypeSDataSource;
            cmb_CarTypeS.DisplayMember = "Name";
            cmb_CarTypeS.ValueMember = "Value";


            var CarSizeSDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
            cmb_CarSizeS.DataSource = CarSizeSDataSource;
            cmb_CarSizeS.DisplayMember = "Name";
            cmb_CarSizeS.ValueMember = "Value";


        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();
            
            var Row = ((DataRowView)nOTICEDRIVERBindingSource.Current).Row as CMDataSet.NOTICEDRIVERRow;
            if (txt_MobileNo.Text.Replace("-","") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_MobileNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
               // return -1;



            }
            else
            {


                var Query1 =
                      "Select Count(*) From NOTICEDRIVER WHERE Replace(MobileNo,'-','') = @MobileNo AND ClientId = @ClientId AND NoticeIdx != @NoticeIdx  ";


                bool IsDuplicated = false;
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = Query1;
                    
                    cmd1.Parameters.AddWithValue("@MobileNo", txt_MobileNo.Text.Trim().Replace("-", ""));
                    cmd1.Parameters.AddWithValue("@NoticeIdx", Row.NoticeIdx);
                    cmd1.Parameters.AddWithValue("@ClientId", Row.ClientId);

                    if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                    {
                        IsDuplicated = true;
                    }
                   
                    cn.Close();
                }
                if (IsDuplicated)
                {
                    MessageBox.Show(" 핸드폰번호가 중복되었습니다.!!. 다른핸드폰번호를 입력해주십시오.");
                    err.SetError(txt_MobileNo, "핸드폰번호가 중복되었습니다.!!");
                   
                    return;
                }
            }


            Up_Status = "Update";
            UpdateDB(Up_Status);
        }
        private void UpdateDB(string Status)
        {
            try
            {

                if (Status == "Update")
                {
                    nOTICEDRIVERBindingSource.EndEdit();
                    var Row = ((DataRowView)nOTICEDRIVERBindingSource.Current).Row as CMDataSet.NOTICEDRIVERRow;
                    Row.State = cmb_AddressState.Text;
                    Row.City = cmb_AddressCity.Text;

                    nOTICEDRIVERTableAdapter.Update(Row);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "카톡배차차량", 1), "카톡배차차량 수정 성공");

                    if (dataGridView1.RowCount > 1)
                    {
                        GridIndex = nOTICEDRIVERBindingSource.Position;
                        btn_Search_Click(null, null);
                        dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                    }
                    else
                    {
                        btn_Search_Click(null, null);
                    }
                }
                else if (Status == "Delete")
                {
                    nOTICEDRIVERTableAdapter.Update(cMDataSet.NOTICEDRIVER);
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "카톡배차차량", 1), "카톡배차차량 삭제 성공");
                    btn_Search_Click(null, null);
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "카톡배차차량 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (dataGridView1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }
              


                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "카톡배차차량", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            Up_Status = "Delete";
            UpdateDB(Up_Status);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void _Search()
        {



            nOTICEDRIVERTableAdapter.Fill(cMDataSet.NOTICEDRIVER);




            List<String> Filters = new List<string>();
            string _cmbSearchString = string.Empty;
            string _CarTypeSearchString = string.Empty;
            string _CarSizeSearchString = string.Empty;
            string _StateSearchString = string.Empty;
            string _CitySearchString = string.Empty;


            try
            {

                if (cmb_Search.Text == "핸드폰번호")
                {

                    string filter = string.Format("MobileNo1 Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;


                }

                if (!String.IsNullOrEmpty(_cmbSearchString))
                    Filters.Add(_cmbSearchString);


                if (cmb_CarTypeS.SelectedIndex != 0)
                {
                    _CarTypeSearchString = string.Format("CarType =  '{0}'", cmb_CarTypeS.SelectedValue.ToString());
                }


                if (!String.IsNullOrEmpty(_CarTypeSearchString))
                    Filters.Add(_CarTypeSearchString);

                if (cmb_CarSizeS.SelectedIndex != 0)
                {
                    _CarSizeSearchString = string.Format("CarSize =  '{0}'", cmb_CarSizeS.SelectedValue.ToString());
                }

                if (!String.IsNullOrEmpty(_CarSizeSearchString))
                    Filters.Add(_CarSizeSearchString);

                

                if (cmb_AddressState_I.SelectedIndex != 0)
                {
                    _StateSearchString = string.Format("State =  '{0}'", cmb_AddressState_I.SelectedValue.ToString());
                }
                if (!String.IsNullOrEmpty(_StateSearchString))
                    Filters.Add(_StateSearchString);


                
                if (cmb_AddressCity_I.SelectedIndex != 0)
                {
                    _CitySearchString = string.Format("State =  '{0}' AND City =  '{1}'", cmb_AddressState_I.SelectedValue.ToString(), cmb_AddressCity_I.SelectedValue.ToString());
                }
                if (!String.IsNullOrEmpty(_CitySearchString))
                    Filters.Add(_CitySearchString);



                try
                {
                    //if (LocalUser.Instance.LogInInfomation.UserGubun == 2)
                    //{
                    //    if (Filters.Count == 0)
                    //    {
                    //        customersBindingSource.Filter = null;
                    //    }
                    //    else
                    //    {
                    //        customersBindingSource.Filter = String.Join(" AND ", Filters);
                    //    }
                    //}
                    //else
                    //{
                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        Filters.Add(String.Format("ClientId = '{0}'", LocalUser.Instance.LogInInformation.ClientId));
                    }
                    nOTICEDRIVERBindingSource.Filter = String.Join(" AND ", Filters);
                    // }
                }
                catch
                {
                    btn_Search_Click(null, null);
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

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = "";
            cmb_CarSizeS.SelectedIndex = 0;
            cmb_CarTypeS.SelectedIndex = 0;
            cmb_AddressState_I.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }

        private void nOTICEDRIVERBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();
            //DataRowView view = driversBindingSource.Current as DataRowView;
            //if (view == null) return;
            //CMDataSet.DriversRow row = view.Row as CMDataSet.DriversRow;


            //  if (row == null) return;
            if (nOTICEDRIVERBindingSource.Current == null)
            {
                txt_CreateDate.Clear();
                cmb_AddressState.SelectedItem = null;
                cmb_AddressCity.DataSource = null;
                txt_ClientCode.Clear();
                txt_ClientName.Clear();
                return;
            }

            var Selected = ((DataRowView)nOTICEDRIVERBindingSource.Current).Row as CMDataSet.NOTICEDRIVERRow;
            if (Selected != null)
            {


                txt_CreateDate.Text = Selected.CreateDate.ToString("d").Replace("-", "/");
                cmb_AddressState.SelectedValue = Selected.State;
                cmb_AddressCity.SelectedValue = Selected.City;

                //    var Query = cMDataSet.Clients.Where(c => c.ClientId == Selected.ClientId).ToArray();
                var Query = CompareTable.Where(c => c.ClientId == Selected.ClientId).ToArray();
                if (Query.Any())
                {
                    txt_ClientCode.Text = Query.First().Code;
                    txt_ClientName.Text = Query.First().Name;
                }
            }
        }

        private void cmb_AddressState_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.NOTICEDRIVERRow;
            if (Selected == null)
                return;
            if (e.ColumnIndex == 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString();
            }

            if (dataGridView1.Columns[e.ColumnIndex] == ClientCodeTextBoxColumn)
            {
                var CodeQuery = CompareTable.Where(c => c.ClientId == Selected.ClientId).ToArray();

                if(CodeQuery.Any())
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = CodeQuery.First().Code;


                }
            }
            if (dataGridView1.Columns[e.ColumnIndex] == ClientNameTextBoxColumn)
            {
                var CodeQuery = CompareTable.Where(c => c.ClientId == Selected.ClientId).ToArray();

                if (CodeQuery.Any())
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = CodeQuery.First().Name;


                }
            }

            if(dataGridView1.Columns[e.ColumnIndex] == mobileNoDataGridViewTextBoxColumn)
            {
                if (Selected.MobileNo.Replace("-","").Length == 10)
                {
                    e.Value = Selected.MobileNo.Replace("-", "").Substring(0, 3) + "-" + Selected.MobileNo.Replace("-", "").Substring(3, 3) + "-" + Selected.MobileNo.Replace("-", "").Substring(6);
                }
                else if (Selected.MobileNo.Replace("-", "").Length == 11)
                {
                    e.Value = Selected.MobileNo.Replace("-", "").Substring(0, 3) + "-" + Selected.MobileNo.Replace("-", "").Substring(3, 4) + "-" + Selected.MobileNo.Replace("-", "").Substring(7);
                }


            }
            if (dataGridView1.Columns[e.ColumnIndex] == createDateDataGridViewTextBoxColumn)
            {
                e.Value = Selected.CreateDate.ToString("d").Replace("-", "/");
            }
            if (dataGridView1.Columns[e.ColumnIndex] == carTypeDataGridViewTextBoxColumn)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").Where(c => c.Value == Selected.CarType).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
            if (dataGridView1.Columns[e.ColumnIndex] == carSizeDataGridViewTextBoxColumn)
            {
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").Where(c => c.Value == Selected.CarSize).ToArray();
                if (Query.Any())
                    e.Value = Query.First().Name;
            }
        }

        private void cmb_AddressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_AddressState.SelectedValue == null)
                return;
            cmb_AddressCity.Enabled = true;

            cmb_AddressCity.DataSource = null;
            var CarCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            cmb_AddressCity.DataSource = CarCityDataSource;

            cmb_AddressCity.DisplayMember = "City";
            cmb_AddressCity.ValueMember = "City";
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void cmb_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Search.Clear();
            if (cmb_Search.SelectedIndex == 0)
            {
                txt_Search.ReadOnly = true;
            }
            else
            {
                txt_Search.ReadOnly = false;
            }
        }

        private void cmb_AddressState_I_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_AddressCity_I.Enabled = true;

            cmb_AddressCity_I.DataSource = null;

            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {


                SqlDataAdapter da = new SqlDataAdapter("select ' 시군구'  as City union select distinct City from AddressReferences WHERE State = '" + cmb_AddressState_I.SelectedValue.ToString() + "'", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmb_AddressCity_I.DataSource = dt;
                cmb_AddressCity_I.DisplayMember = "City";
                cmb_AddressCity_I.ValueMember = "City";
            }


            //var CarCityDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_AddressState_I.SelectedValue.ToString()).Select(c => new { c.City }).Distinct().ToArray();
            //cmb_AddressCity_I.DataSource = CarCityDataSource;

            //cmb_AddressCity_I.DisplayMember = "City";
            //cmb_AddressCity_I.ValueMember = "City";
        }

        private void cmb_CarSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] SmallCarTypeValues = new int[]{
                2, 3
            };
            int[] MediumCarTypeValues = new int[]{
                0,1,4,5,6,8,9,10,14,16,18,20,21,60,61,62,22,63,64,65,66,67,27,68,69,33,34,70,71
            };
            int[] FiveCarTypeValues = new int[]{
                0,1,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59
            };
            int[] LargeCarTypeValues = new int[]{
                0,1,4,5,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59
            };
            if (cmb_CarSize.SelectedValue == null)
                return;
            int CarSizeValue = (int)cmb_CarSize.SelectedValue;
            if (CarSizeValue == 1)
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 2;
            }
            else if (CarSizeValue == 2)
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && SmallCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 3;
            }
            else if (new int[] { 3, 4, 5, 6 }.Contains(CarSizeValue))
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && MediumCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 0;
            }
            else if (new int[] { 7 }.Contains(CarSizeValue))
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && FiveCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 0;
            }
            else if (CarSizeValue > 7)
            {
                var CarTypeDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType" && LargeCarTypeValues.Contains(c.Value)).Select(c => new { c.Value, c.Name, c.Seq }).OrderBy(c => c.Seq).ToArray();
                cmb_CarType.DataSource = CarTypeDataSource;
                cmb_CarType.SelectedValue = 0;
            }
        }
    }
}
