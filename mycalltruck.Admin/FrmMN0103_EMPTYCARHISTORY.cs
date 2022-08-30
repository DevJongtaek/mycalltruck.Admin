using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0103_EMPTYCARHISTORY : Form
    {
        public FrmMN0103_EMPTYCARHISTORY()
        {
            InitializeComponent();

            _InitCmb();

            dtpStart.Value = DateTime.Now.AddMonths(-1);
            dtpEnd.Value = DateTime.Now;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ReadOnly = false;
            this.SeqColumn.ReadOnly = true;
            this.CreateTimeColumn.ReadOnly = true;
            this.CarTypeColumn.ReadOnly = true;
            this.CarSizeColumn.ReadOnly = true;
            this.CarYearColumn.ReadOnly = true;
            this.IsGroupColumn.ReadOnly = true;
            this.CarNoColumn.ReadOnly = true;
            this.ParkColumn.ReadOnly = true;
            this.RouteTypeColumn.ReadOnly = true;
            this.DriverNameColumn.ReadOnly = true;
            this.PhoneNoColumn.ReadOnly = true;

        }

        private void _InitCmb()
        {
            cmb_CarType.DisplayMember = "Text";
            cmb_CarType.ValueMember = "Value";
            cmb_CarType.DataSource = Filter.PreviewPreOrder.CarTypeList;

            cmb_CarSize.DisplayMember = "Text";
            cmb_CarSize.ValueMember = "Value";
            cmb_CarSize.DataSource = Filter.PreviewPreOrder.CarSizeList;

            cmb_DriverSearchType.DisplayMember = "Text";
            cmb_DriverSearchType.ValueMember = "Value";
            cmb_DriverSearchType.DataSource = Filter.PreOrder.DriverFilterTypeList;

            //var NotificationGroupSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NotificationGroup2").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_NotificationGroup.DataSource = NotificationGroupSourceDataSource;
            //cmb_NotificationGroup.DisplayMember = "Name";
            //cmb_NotificationGroup.ValueMember = "value";


            Dictionary<string, string> NotificationGroup = new Dictionary<string, string>();
            NotificationGroup.Add("0", "그룹전체");
            NotificationGroup.Add("A", "A 그룹");
            NotificationGroup.Add("B", "B 그룹");
            NotificationGroup.Add("C", "C 그룹");
            NotificationGroup.Add("My", "A+B+C");
            NotificationGroup.Add("Other", "기타");


            cmb_NotificationGroup.DataSource = new BindingSource(NotificationGroup, null);
            cmb_NotificationGroup.DisplayMember = "Value";
            cmb_NotificationGroup.ValueMember = "Key";




        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            var model = new GetCustomSelectViewModel
            {
                FromDate = dtpStart.Value,
                ToDate = dtpEnd.Value,
                DriverFilterType = (int)cmb_DriverSearchType.SelectedValue,
                DriverFilterValue = txt_CarNo.Text,
                CarType = (int)cmb_CarType.SelectedValue,
                CarSize = (int)cmb_CarSize.SelectedValue,
                GroupName = cmb_NotificationGroup.SelectedValue.ToString(),
            };
            BindingList<CustomSelectViewModel> mBindingList = new BindingList<CustomSelectViewModel>();
            HttpClient mClient = new HttpClient();
            HttpResponseMessage Response = mClient.PostAsync(Filter.CustomSelecte.ItemUrl + LocalUser.Instance.LogInInformation.ClientId.ToString(), Serialize(model)).Result;
            if (Response.IsSuccessStatusCode)
            {
                var R = JsonConvert.DeserializeObject(Response.Content.ReadAsStringAsync().Result);
                if (R is JArray)
                {
                    foreach (var item in (JArray)R)
                    {
                        mBindingList.Add(new CustomSelectViewModel
                        {
                            CustomSelectId = (int)item["CustomSelectId"],
                            CreateTime = (DateTime)item["CreateTime"],
                            CarType = Filter.PreOrder.CarTypeList.First(c => c.Value == (int)item["CarType"]).Text,
                            CarSize = Filter.PreOrder.CarSizeList.First(c => c.Value == (int)item["CarSize"]).Text,
                            CarYear = item["CarYear"].ToString(),
                            GroupName = item["GroupName"].ToString(),
                            CarNo = item["CarNo"].ToString(),
                            DriverName = item["DriverName"].ToString(),
                            DriverMobileNo = item["DriverMobileNo"].ToString(),
                            Remark = item["Remark"].ToString(),
                            Park = item["Park"].ToString(),
                            RouteType = Filter.PreOrder.RouteTypeList.First(c => c.Value == (int)item["RouteType"]).Text,
                        });
                    }
                }
            }

            dataGridView1.DataSource = mBindingList;
        }

        private FormUrlEncodedContent Serialize(object model)
        {
            List<KeyValuePair<String, String>> Data = new List<KeyValuePair<string, string>>();

            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var Name = propertyInfo.Name;
                var Value = propertyInfo.GetValue(model,null);
                if (Value == null)
                {
                    Data.Add(new KeyValuePair<string, string>(Name, ""));
                }
                else
                {
                    Data.Add(new KeyValuePair<string, string>(Name, Value.ToString()));

                }
            }

            return new FormUrlEncodedContent(Data);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            else if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as CustomSelectViewModel;
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = Selected.CreateTime.ToString("yyyy-MM-dd HH:ss").Replace("-", "/");
            }
            else if (e.ColumnIndex == 11 && e.RowIndex >= 0)
            {
                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as CustomSelectViewModel;
                if (Selected.DriverMobileNo.Length > 9)
                {
                    if (Selected.DriverMobileNo.Length == 10)
                    {
                        e.Value = Selected.DriverMobileNo.Substring(0, 3) + "-" + Selected.DriverMobileNo.Substring(3, 3) + "-" + Selected.DriverMobileNo.Substring(6, 4);
                    }
                    else if (Selected.DriverMobileNo.Length == 11)
                    {
                        e.Value = Selected.DriverMobileNo.Substring(0, 3) + "-" + Selected.DriverMobileNo.Substring(3, 4) + "-" + Selected.DriverMobileNo.Substring(7, 4);
                    }

                }


            }
            else if (e.ColumnIndex == 12 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.Font = new System.Drawing.Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
            }
           
        }


        private void FrmMN0103_EMPTYCARHISTORY_Load(object sender, EventArgs e)
        {
            //this.clientAddressTableAdapter.FillByClient(cMDataSet.ClientAddress);


            btn_Search_Click(null, null);

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dataGridView1.CurrentCell.ColumnIndex != 12)
                return;
            var Tbx = e.Control as TextBox;
            if (Tbx != null)
                Tbx.BorderStyle = BorderStyle.Fixed3D;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 12 && e.RowIndex >= 0)
            {
                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as CustomSelectViewModel;

                using (SqlConnection cn = new  SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    var Query =
                        "UPDATE CustomSelects SET Remark = @Remark WHERE CustomSelectId = @CustomSelectId";

                    SqlCommand Cmd = cn.CreateCommand();
                    Cmd.CommandText = Query;
                    Cmd.Parameters.AddWithValue("@CustomSelectId", Selected.CustomSelectId);
                    Cmd.Parameters.AddWithValue("@Remark", Selected.Remark);
                    Cmd.ExecuteNonQuery();


                    cn.Close();
                }

            }
        }

        private void txt_CarNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.AddDays(-1);
            dtpEnd.Value = DateTime.Now;
            cmb_CarType.SelectedIndex = 0;
            cmb_CarSize.SelectedIndex = 0;
            cmb_NotificationGroup.SelectedIndex = 0;
            cmb_DriverSearchType.SelectedIndex = 0;
            txt_CarNo.Text = string.Empty;
            btn_Search_Click(null, null);
        }
       

       
    }
}
