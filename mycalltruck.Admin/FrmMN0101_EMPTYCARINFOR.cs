using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace mycalltruck.Admin
{
    public partial class FrmMN0101_EMPTYCARINFOR : Form
    {
        string ClientAddr = string.Empty;
        public FrmMN0101_EMPTYCARINFOR()
        {
            InitializeComponent();
            _InitCmb();
            dataGridView1.AutoGenerateColumns = false;
            if(LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ColumnLastNoticeTime.Visible = true;
                colLastNoticeGPS.Visible = true;
                colLastNoticeGPSMap.Visible = true;
                ColumnLastNoticeFlag.Visible = true;
            }
        }

        private void _InitCmb()
        {
            cmb_FilterType.DataSource = Filter.PreOrder.FilterTypeList(!LocalUser.Instance.LogInInformation.IsAdmin);
            cmb_FilterType.DisplayMember = "Text";
            cmb_FilterType.ValueMember = "Value";

            cmb_Radius.DataSource = Filter.PreOrder.RadiusList;
            cmb_Radius.DisplayMember = "Text";
            cmb_Radius.ValueMember = "Value";

            cmb_CarType.DataSource = Filter.PreOrder.CarTypeList;
            cmb_CarType.DisplayMember = "Text";
            cmb_CarType.ValueMember = "Value";

            cmb_CarSize.DataSource = Filter.PreOrder.CarSizeList;
            cmb_CarSize.DisplayMember = "Text";
            cmb_CarSize.ValueMember = "Value";

            cmb_PreviewType.DataSource = Filter.PreOrder.PreviewTypeList;
            cmb_PreviewType.DisplayMember = "Text";
            cmb_PreviewType.ValueMember = "Value";


            cmb_DriverSearchType.DataSource = Filter.PreOrder.DriverFilterTypeList;
            cmb_DriverSearchType.DisplayMember = "Text";
            cmb_DriverSearchType.ValueMember = "Value";


            var ParkStateDataSource = (from a in SingleDataSet.Instance.AddressReferences select new { a.State }).Distinct().ToArray();
            cmb_ParkState.DataSource = ParkStateDataSource;
            cmb_ParkState.DisplayMember = "State";
            cmb_ParkState.ValueMember = "State";






            var ParkCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c=>c.State == cmb_ParkState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            cmb_ParkCity.DataSource = ParkCityDataSource;
            cmb_ParkCity.DisplayMember = "City";
            cmb_ParkCity.ValueMember = "City";



            //   .Where(c => c.City == cmb_ParkCity.SelectedValue.ToString()).Where(c => c.Street != "").Select(c => new { c.Street }).Distinct().ToArray();
            var ParkStreetDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()).Where(c => c.City == cmb_ParkCity.SelectedValue.ToString()).Where(c => c.Street != "") select new { a.Street }).Distinct().ToArray();
            cmb_ParkStreet.DataSource = ParkStreetDataSource;
            cmb_ParkStreet.DisplayMember = "Street";
            cmb_ParkStreet.ValueMember = "Street";

            //var NotificationGroupSourceDataSource = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "NotificationGroup").Select(c => new { c.StaticOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            //cmb_NotificationGroup.DataSource = NotificationGroupSourceDataSource;
            //cmb_NotificationGroup.DisplayMember = "Name";
            //cmb_NotificationGroup.ValueMember = "value";

            Dictionary<string, string> NotificationGroup = new Dictionary<string, string>();
            NotificationGroup.Add("0", "그룹전체");
            NotificationGroup.Add("A", "A 그룹");
            NotificationGroup.Add("B", "B 그룹");
            NotificationGroup.Add("C", "C 그룹");
          
            cmb_NotificationGroup.DataSource = new BindingSource(NotificationGroup, null);
            cmb_NotificationGroup.DisplayMember = "Value";
            cmb_NotificationGroup.ValueMember = "Key";







        }

        private void cmb_ParkState_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_ParkCity.Enabled = true;




            var ParkCityDataSource = (from a in SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()) select new { a.City }).Distinct().ToArray();
            cmb_ParkCity.DataSource = ParkCityDataSource;
            cmb_ParkCity.DisplayMember = "City";
            cmb_ParkCity.ValueMember = "City";

            ChangeInfo();
        }

        private void cmb_ParkCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_ParkStreet.Enabled = true;
            var ParkStreetDataSource = SingleDataSet.Instance.AddressReferences.Where(c => c.State == cmb_ParkState.SelectedValue.ToString()).Where(c => c.City == cmb_ParkCity.SelectedValue.ToString()).Where(c => c.Street != "").Select(c => new { c.Street }).Distinct().ToArray();
            cmb_ParkStreet.DataSource = ParkStreetDataSource;
            cmb_ParkStreet.DisplayMember = "Street";
            cmb_ParkStreet.ValueMember = "Street";

            ChangeInfo();
        }


        
        private void cmb_FilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                
                //var ClientPark = SingleDataSet.Instance.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();

                var ClientPark = cMDataSet.ClientAddress.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();


                if (ClientPark.Any())
                {
                    cmb_ParkState.SelectedValue = ClientPark.First().AddressState;
                    cmb_ParkCity.SelectedValue = ClientPark.First().AddressCity;
                    cmb_ParkStreet.SelectedValue = ClientPark.First().AddressStreet;
                }
                else
                {
                    cmb_ParkState.SelectedValue = "서울특별시";
                    cmb_ParkCity.SelectedValue = "종로구";
                    cmb_ParkStreet.SelectedValue = "사직동";
                }


                //cmb_ParkState.SelectedValue = ClientPark.First().AddressState;
                //cmb_ParkCity.SelectedValue = ClientPark.First().AddressCity;
                //cmb_ParkStreet.SelectedValue = ClientPark.First().AddressStreet;
            }
            

            ChangeInfo();

            cmb_Radius.SelectedValue = 30;

        }
        private void ChangeInfo()
        {
            if ((int)cmb_FilterType.SelectedValue == 1)
            {
                cmb_FilterType.Visible = true;
                cmb_PreviewType.Visible = true;
                cmb_Radius.Visible = true;
                cmb_CarType.Visible = true;
                cmb_CarSize.Visible = true;



                cmb_DriverSearchType.Visible = false;
                txt_CarNo.Visible = false;
                cmb_ParkCity.Visible = false;
                cmb_ParkState.Visible = false;
                cmb_ParkStreet.Visible = false;
                cmb_NotificationGroup.Visible = false;

              //  var state = SingleDataSet.Instance.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).Select(c => new { c.AddressState, c.AddressCity, c.AddressStreet }).First();


                var state = cMDataSet.ClientAddress.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.AddressState, c.AddressCity, c.AddressStreet }).ToArray();

                if (state.Any())
                {
                    ClientAddr = state.First().AddressState + " " + state.First().AddressCity + " " + state.First().AddressStreet;
                }
                else
                {
                    ClientAddr = "서울특별시 종로구 사직동";
                }
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                label4.Visible = false;
            
                label1.Text = "[" + ClientAddr + "]";
                label1.ForeColor = Color.Blue;
                label2.Text =  " 반경 " + cmb_Radius.SelectedValue + "km 내 공차를 조회 합니다.";
                label2.ForeColor = Color.Red;

                



            }
            else if ((int)cmb_FilterType.SelectedValue == 2)
            {
                cmb_FilterType.Visible = true;



                cmb_ParkStreet.Visible = true;
                cmb_ParkCity.Visible = true;
                cmb_ParkState.Visible = true;


                cmb_Radius.Visible = true;
                cmb_CarType.Visible = true;
                cmb_CarSize.Visible = true;
                cmb_PreviewType.Visible = true;


                cmb_DriverSearchType.Visible = false;
                txt_CarNo.Visible = false;
                cmb_NotificationGroup.Visible = false;

              //  label1.Text = "[" + cmb_ParkState.SelectedValue + " " + cmb_ParkCity.SelectedValue + " " + cmb_ParkStreet.SelectedValue + "]" + " 반경 " + cmb_Radius.SelectedValue + "km 내 공차를 조회 합니다.";

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                label4.Visible = false;

               

                label1.Text = "[" + cmb_ParkState.SelectedValue + " " + cmb_ParkCity.SelectedValue + " " + cmb_ParkStreet.SelectedValue + "]";
                label1.ForeColor = Color.Blue;
                label2.Text = " 반경 " + cmb_Radius.SelectedValue + "km 내 공차를 조회 합니다.";
                label2.ForeColor = Color.Red;



            }
            else if ((int)cmb_FilterType.SelectedValue == 3)
            {
                cmb_FilterType.Visible = true;
                cmb_PreviewType.Visible = false;
                cmb_Radius.Visible = false;
                cmb_CarType.Visible = true;
                cmb_CarSize.Visible = true;
                cmb_NotificationGroup.Visible = true;


                cmb_ParkCity.Visible = false;
                cmb_ParkState.Visible = false;
                cmb_ParkStreet.Visible = false;

                cmb_DriverSearchType.Visible = false;
                txt_CarNo.Visible = false;

                string GroupName = string.Empty;
                if (cmb_NotificationGroup.SelectedIndex == 0)
                {
                    GroupName = "전체 ";
                }
                else 
                {
                    GroupName = cmb_NotificationGroup.SelectedValue.ToString();
                }

               // label1.Text = "[" + LocalUser.Instance.LogInInfomation.UserName + "] " + GroupName + " 그룹 공차를 조회 합니다.";

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                label4.Visible = false;

                label1.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";
                label1.ForeColor = Color.Blue;
                label2.Text = GroupName + " 그룹 공차를 조회 합니다.";
                label2.ForeColor = Color.Red;


            }
            else if ((int)cmb_FilterType.SelectedValue == 4)
            {
                cmb_FilterType.Visible = true;
                cmb_PreviewType.Visible = true;
                cmb_Radius.Visible = true;
                cmb_CarType.Visible = true;
                cmb_CarSize.Visible = true;
                cmb_NotificationGroup.Visible = true;


                cmb_ParkCity.Visible = false;
                cmb_ParkState.Visible = false;
                cmb_ParkStreet.Visible = false;

                cmb_DriverSearchType.Visible = false;
                txt_CarNo.Visible = false;

               // var state = SingleDataSet.Instance.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).Select(c => new { c.AddressState, c.AddressCity, c.AddressStreet }).First();
                var state = cMDataSet.ClientAddress.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.AddressState, c.AddressCity, c.AddressStreet }).ToArray();

                if (state.Any())
                {
                    ClientAddr = state.First().AddressState + " " + state.First().AddressCity + " " + state.First().AddressStreet;
                }
                else
                {
                    ClientAddr = "서울특별시 종로구 사직동";
                }
              //  ClientAddr = state.AddressState + " " + state.AddressCity + " " + state.AddressStreet;

                string GroupName = string.Empty;

                if (cmb_NotificationGroup.SelectedIndex == 0)
                {
                    GroupName = "전체 ";
                }
                else
                {
                    GroupName = cmb_NotificationGroup.SelectedValue.ToString();
                }

               // label1.Text = "[" + LocalUser.Instance.LogInInfomation.UserName + "] " + GroupName + "그룹과" + "[" + ClientAddr + "]" + " 반경 "  + cmb_Radius.SelectedValue +  "Km 내 공차를 조회 합니다.";


                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;

                label1.Text = "[" + LocalUser.Instance.LogInInformation.ClientName + "] ";
                label1.ForeColor = Color.Blue;
                label2.Text = GroupName + "그룹과 ";
                label2.ForeColor = Color.Red;
                label3.Text = "[" + ClientAddr + "]";
                label3.ForeColor = Color.Blue;
                label4.Text = " 반경 " + cmb_Radius.SelectedValue + "Km 내 공차를 조회 합니다.";
                label4.ForeColor = Color.Red;



            }
            else if ((int)cmb_FilterType.SelectedValue == 5)
            {
                cmb_FilterType.Visible = true;
                cmb_PreviewType.Visible = true;
                cmb_Radius.Visible = false;
                cmb_CarType.Visible = true;
                cmb_CarSize.Visible = true;


                cmb_ParkCity.Visible = false;
                cmb_ParkState.Visible = false;
                cmb_ParkStreet.Visible = false;


                txt_CarNo.Visible = true;
                cmb_DriverSearchType.Visible = true;

                cmb_NotificationGroup.Visible = false;


                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = false;


             //   label1.Text = "입력하신 [검색조건] 으로 공차를 조회합니다.";
                label1.Text = "입력하신 ";
                label1.ForeColor = Color.Red;
                label2.Text = "[검색조건] ";
                label2.ForeColor = Color.Blue;
                label3.Text = "으로 공차를 조회합니다.";
                label3.ForeColor = Color.Red;
            }

        }

        private void cmb_Radius_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void cmb_ParkStreet_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void cmb_NotificationGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            var model = new GetPreOrderViewModel
            {
                FilterType = (int)cmb_FilterType.SelectedValue,
                DriverFilterType = (int)cmb_DriverSearchType.SelectedValue,
                DriverFilterValue = txt_CarNo.Text,
                State = cmb_ParkState.Text,
                City = cmb_ParkCity.Text,
                Street = cmb_ParkStreet.Text,
                Radius = (int)cmb_Radius.SelectedValue,
                CarType = (int)cmb_CarType.SelectedValue,
                CarSize = (int)cmb_CarSize.SelectedValue,
                PreviewType = (int)cmb_PreviewType.SelectedValue,
                GroupName = cmb_NotificationGroup.SelectedValue.ToString(),
            };
            BindingList<PreOrderViewModel> mBindingList = new BindingList<PreOrderViewModel>();
            HttpClient mClient = new HttpClient();
            HttpResponseMessage Response = mClient.PostAsync(Filter.PreOrder.ItemUrl + LocalUser.Instance.LogInInformation.ClientId.ToString(), Serialize(model)).Result;
            if(Response.IsSuccessStatusCode)
            {
               var R = JsonConvert.DeserializeObject(Response.Content.ReadAsStringAsync().Result);
               if(R is JArray)
               {
                   foreach (var item in (JArray)R)
                   {
                       mBindingList.Add(new PreOrderViewModel {
                           PreOrderId = (int)item["PreOrderId"],
                           Seq = (int)item["Seq"],
                           Distance = (double)item["Distance"],
                           CarType = Filter.PreOrder.CarTypeList.First(c=>c.Value== (int)item["CarType"]).Text,
                           CarSize = Filter.PreOrder.CarSizeList.First(c=>c.Value== (int)item["CarSize"]).Text,
                           CarYear = item["CarYear"].ToString(),
                           IsGroup = item["IsGroup"].ToString(),
                           CarNo = item["CarNo"].ToString(),
                           IsPreview = (bool)item["IsPreview"],
                           DriverName = item["DriverName"].ToString(),
                           PhoneNo = item["PhoneNo"].ToString(),
                           IsCustomSelected = (bool)item["IsCustomSelected"],
                           X = (double)item["X"],
                           Y = (double)item["Y"],
                           DriverId = (int)item["DriverId"],
                           Park = item["Park"].ToString(),
                           RouteType = Filter.PreOrder.RouteTypeList.First(c => c.Value == (int)item["RouteType"]).Text,
                           StopTime = (DateTime)item["StopTime"],
                           State = item["State"].ToString(),
                           City = item["City"].ToString(),
                           LastNoticeTime = item["LastNoticeTime"].ToString(),
                           LastNoticeFlag = item["LastNoticeFlag"].ToString(),
                           LastNoticeGPS = item["LastNoticeGPS"].ToString(),
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
                if(Value == null)
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
            else if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {
                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as PreOrderViewModel;
                if (Selected.IsPreview)
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Selected.State + "/" + Selected.City;
                }
            }
            else if (e.ColumnIndex == 9 && e.RowIndex >= 0)
            {
                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as PreOrderViewModel;
                if (Selected.IsPreview)
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Selected.StopTime.ToString("d").Replace("-", "/");
                }
            }


            if (e.RowIndex >= 0)
            {
                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as PreOrderViewModel;
                if (e.ColumnIndex == 1)
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.Font = new System.Drawing.Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                }
                else
                {
                    if (!Selected.IsPreview)
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Blue;
                    }
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 14 && e.RowIndex >= 0)
            {
                if (LocalUser.Instance.LogInInformation.IsAdmin)
                    return;
                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as PreOrderViewModel;
                Selected.IsCustomSelected = !Selected.IsCustomSelected;
                HttpClient mClient = new HttpClient();
                var R = mClient.GetAsync(string.Format(Filter.PreOrder.CustomSelectUrl,LocalUser.Instance.LogInInformation.ClientId,Selected.PreOrderId,Selected.IsCustomSelected)).Result;
            }

            else if (e.ColumnIndex == 18 &&  e.RowIndex >= 0)
            {

                var Selected = dataGridView1.Rows[e.RowIndex].DataBoundItem as PreOrderViewModel;
                if (Selected != null)
                {
                    if (Selected.LastNoticeGPS != "")
                    {
                        string url = "http://local.daum.net/map/look?p=" + Selected.LastNoticeGPS;
                        // System.Diagnostics.Process ie = System.Diagnostics.Process.Start("IExplore.exe", url);


                        System.Diagnostics.Process.Start(url);

                    }
                }
            }
        }

        private void FrmMN0101_EMPTYCARINFOR_Load(object sender, EventArgs e)
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                this.clientAddressTableAdapter.FillByClient(cMDataSet.ClientAddress, LocalUser.Instance.LogInInformation.ClientId);
            }
            else
            {
                this.clientAddressTableAdapter.Fill(cMDataSet.ClientAddress);
            }
            cmb_Radius.SelectedIndex = 5;
            btn_Search_Click(null, null);
        }

        private void txt_CarNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            cmb_FilterType.SelectedIndex = 0;


            ChangeInfo();

            cmb_Radius.SelectedValue = 30;
            if (LocalUser.Instance.LogInInformation.ClientId >0)
            {

                cmb_FilterType.SelectedIndex = 0;
                cmb_PreviewType.SelectedIndex = 0;
               // cmb_Radius.SelectedIndex = 0;
                cmb_CarType.SelectedIndex = 0;
                cmb_CarSize.SelectedIndex = 0;

               





            }
            else
            {
                cmb_ParkStreet.SelectedIndex = 0;
                cmb_ParkCity.SelectedIndex = 0;
                cmb_ParkState.SelectedIndex = 0;



                cmb_CarType.SelectedIndex = 0;
                cmb_CarSize.SelectedIndex = 0;
                cmb_PreviewType.SelectedIndex = 0;
            }
            btn_Search_Click(null, null);

        }


        
    }
}
