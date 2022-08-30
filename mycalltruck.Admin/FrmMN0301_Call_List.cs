using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0301_Call_List : Form
    {
        BindingList<Model> DataListSource = new BindingList<Model>();
        Dictionary<int, Rectangle> RowBoundDictionary = new Dictionary<int, Rectangle>();

        public FrmMN0301_Call_List()
        {
            InitializeComponent();
            CTimeSFilter.Value = DateTime.Now.Date;
            cmbCallGubun.SelectedIndex = 0;
            cmbSearch.SelectedIndex = 0;
            cmb_Gubun.SelectedIndex = 0;
            Search();
        }

        private void Search()
        {
            var CTimeBegin = CTimeSFilter.Value.Date;
            var CTimeEnd = CTimeEFilter.Value.Date.AddDays(1);
          
            DataListSource.Clear();
            RowBoundDictionary.Clear();
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = "SELECT Calls.CallId, Calls.OriginalPhoneNo, Calls.Target, Calls.Div, Calls.CustomerId, Calls.DriverId, Calls.CTime, Calls.ClientId, Calls.ClientPhoneNo, ISNULL(Calls.Memo,N''), Calls.LoginId,Calls.Gubun ,ISNULL(Drivers.CarNo,N'') as CarNo FROM Calls left JOIN Drivers ON Calls.DriverId = Drivers.DriverId";

                    List<String> WhereStringList = new List<string>();
                    WhereStringList.Add("Calls.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    WhereStringList.Add("Calls.LoginId = @LoginId");
                    _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);

                    WhereStringList.Add(" Calls.CTime >= @CTimeBegin");
                    _Command.Parameters.AddWithValue("@CTimeBegin", CTimeBegin);

                    WhereStringList.Add(" Calls.CTime < @CTimeEnd");
                    _Command.Parameters.AddWithValue("@CTimeEnd", CTimeEnd);

                    if(cmb_Gubun.Text =="거래처")
                    {
                        WhereStringList.Add(" Calls.Div = @Div ");
                        _Command.Parameters.AddWithValue("@Div", "거래처");
                    }
                    else if (cmb_Gubun.Text == "차주")
                    {
                        WhereStringList.Add(" Calls.Div = @Div ");
                        _Command.Parameters.AddWithValue("@Div", "차주");
                    }

                    if (cmbCallGubun.Text == "발신")
                    {
                        WhereStringList.Add(" Calls.Gubun = @Gubun ");
                        _Command.Parameters.AddWithValue("@Gubun", "발신");
                    }
                    else if (cmbCallGubun.Text == "수신")
                    {
                        WhereStringList.Add(" Calls.Gubun = @Gubun ");
                        _Command.Parameters.AddWithValue("@Gubun", "수신");
                    }
                    else if (cmbCallGubun.Text == "부재중")
                    {
                        WhereStringList.Add(" Calls.Gubun = @Gubun ");
                        _Command.Parameters.AddWithValue("@Gubun", "부재중");
                    }

                    if (cmbSearch.Text == "상호/이름")
                    {
                        WhereStringList.Add(string.Format("Calls.Target Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmbSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("Drivers.CarNo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmbSearch.Text == "전화번호")
                    {
                        WhereStringList.Add(string.Format("REPLACE(Calls.OriginalPhoneNo,'-','') Like  '%{0}%'", txt_Search.Text.Replace("-", "")));
                    }


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }




                    SelectCommandText += " ORDER BY CTime DESC ";

                    
                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            Call call = new Call
                            {
                                CallId = _Reader.GetInt32(0),
                                OriginalPhoneNo = _Reader.GetString(1),
                                Target = _Reader.GetString(2),
                                Div = _Reader.GetString(3),
                                CustomerId = _Reader.GetInt32(4),
                                DriverId = _Reader.GetInt32(5),
                                CTime = _Reader.GetDateTime(6),
                                ClientId = _Reader.GetInt32(7),
                                ClientPhoneNo = _Reader.GetString(8),
                                LoginId = _Reader.GetString(10),
                                Gugun = _Reader.GetString(11),
                                Memo = _Reader.GetString(9),
                        };
                         
                          
                            DataListSource.Add(new Model { Call = call });
                       
                        }
                    }
                }
            });
            DataList.AutoGenerateColumns = false;
            DataList.DataSource = DataListSource;
        }

        #region
     
        #endregion
        private void DataList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Model = DataListSource[e.RowIndex];
            if (Model.Call != null)
            {
                if (e.ColumnIndex == ColumnNo.Index)
                    e.Value = (DataListSource.Count - e.RowIndex).ToString("N0");

                //e.Value = Math.Round((double)(DataListSource.Count - e.RowIndex) / 2).ToString("N0");
                else if (e.ColumnIndex == ColumnCTime.Index)
                    e.Value = Model.Call.CTime.ToString("yyyy-MM-dd HH:mm:ss");
                else if (e.ColumnIndex == ColumnOriginalPhoneNo.Index)
                {
                    string _S = Model.Call.OriginalPhoneNo;
                    if (_S.StartsWith("1"))
                    {
                        if (_S.Length > 4)
                        {
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4);
                        }
                        else if (_S.Length > 8)
                        {
                            _S = _S.Substring(0, 4) + "-" + _S.Substring(4, 4);
                        }
                    }
                    else if (_S.StartsWith("02"))
                    {
                        if (_S.Length > 2)
                        {
                            _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                        }
                        if (_S.Length > 6)
                        {
                            _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                        }
                        if (_S.Length > 11)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                        }
                    }
                    else if (_S.StartsWith("01"))
                    {
                        if (_S.Length > 3)
                        {
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                        }
                        if (_S.Length > 7)
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                        if (_S.Length > 12)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                        }
                    }
                    else
                    {
                        if (_S.Length > 3)
                        {
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                        }
                        if (_S.Length > 7)
                        {
                            _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                        }
                        if (_S.Length > 12)
                        {
                            _S = _S.Replace("-", "");
                            _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                        }
                    }
                    e.Value = _S;
                }
                else if (e.ColumnIndex == ColumnTarget.Index)
                    e.Value = Model.Call.Target;
                else if (e.ColumnIndex == ColumnGubun.Index)
                {
                    if (Model.Call.Gugun == "수신")
                    {
                        Image _Image = mycalltruck.Admin.Properties.Resources.icon_receive_gray;

                        e.Value = _Image;
                    }
                    else if (Model.Call.Gugun == "발신")
                    {

                        Image _Image = mycalltruck.Admin.Properties.Resources.icon_send_gray;

                        e.Value = _Image;

                        
                    }
                    else if (Model.Call.Gugun == "부재중")
                    {

                        Image _Image = mycalltruck.Admin.Properties.Resources.icon_Empty_gray;

                        e.Value = _Image;

                        
                    }
                }
                else if (e.ColumnIndex == ColumnDiv.Index)
                {
                    if (Model.Call.CustomerId > 0)
                        e.Value = "거래처";
                    else if (Model.Call.DriverId > 0)
                        e.Value = "차주";
                    else
                        e.Value = "미등록";
                }

                else if (e.ColumnIndex == ColumnMemoText.Index)
                {

                    e.Value = Model.Call.Memo.Replace("\r\n", " ");

                }

                else if (e.ColumnIndex == ColumnCallStatus.Index)
                {
                    if (Model.Call.Gugun == "수신")
                    {
                        e.Value = "수신";
                    }
                    else if (Model.Call.Gugun == "발신")
                    {
                        
                        e.Value = "발신";
                    }
                    else if (Model.Call.Gugun == "부재중")
                    {

                        e.Value = "부재중";
                        e.CellStyle.ForeColor = Color.Red;
                    }


                }
            }
        }

        private void DataList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //var Model = DataListSource[e.RowIndex];

            //if (Model.Call == null)
            //    return;
            //if (e.ColumnIndex == ColumnGubun.Index)
            //{
            //    if (Model.Call.Gugun == "부재중")
            //    {
            //        ColumnGubun.Image = mycalltruck.Admin.Properties.Resources.icon_Empty_gray;

            //    }
            //    if (Model.Call.Gugun == "발신")
            //    {
            //        ColumnGubun.Image = mycalltruck.Admin.Properties.Resources.icon_send_gray;
            //    }

            //    if (Model.Call.Gugun == "수신")
            //    {

            //        ColumnGubun.Image = mycalltruck.Admin.Properties.Resources.icon_receive_gray;
            //    }
            //}


        }

      
        private void DoSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        String Id = "";
        String Password = "";
        private void DataList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //var ConfigFile = CallHelper.Instance._GetConfigFile();
            //if (File.Exists(ConfigFile))
            //{
            //    try
            //    {
            //        var Json = File.ReadAllText(ConfigFile, Encoding.UTF8);
            //        var jObject = (JObject)JsonConvert.DeserializeObject(Json);
            //        if (jObject.GetValue("Id") != null && jObject.GetValue("Password") != null)
            //        {
            //            Id = jObject.GetValue("Id").ToString();
            //            Password = jObject.GetValue("Password").ToString();
                        
            //        }
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}


            var Model = DataListSource[e.RowIndex];
            if (Model.Call != null)
            {
                FrmMN301_Call_PopupNew f = new FrmMN301_Call_PopupNew(Model.Call.OriginalPhoneNo, Id, DateTime.Now,"Click");

                if (e.ColumnIndex == ColumnSendCall.Index)
                {

                    f.SendCall();
                    f.Show();
                      


                    // CallHelper.Instance.Call(Model.Call.OriginalPhoneNo);
                }
                else if (e.ColumnIndex == ColumnSendSMS.Index)
                {


                    f.SendSMS();

                    f.Show();



                }
                else if (e.ColumnIndex == ColumnMemo.Index)
                {

                    f.SendMemo(Model.Call.Memo,Model.Call.CallId);

                    f.Show();

                }
                else if (e.ColumnIndex == ColumnShowImage.Index)
                {
                    if (Model.Call.CustomerId > 0)
                    {
                        FrmMDI.Dialog_CustomerImage_Instance.LoadCutomer(Model.Call.CustomerId);
                    }
                }
                else if (e.ColumnIndex == ColumnLoadCutomer.Index)
                {
                    if (Model.Call.CustomerId > 0)
                    {
                        FrmMDI.LoadCustomerFromCall(Model.Call.CustomerId);
                    }
                }

                else if (e.ColumnIndex == ColumnCallCustomer.Index)
                {
                    if (Model.Call.CustomerId > 0)
                    {
                        FrmMDI.LoadForm("FrmMN0301", "Customer", Model.Call.CustomerId);
                    }
                }
            }
        }

        class Model
        {
            public Call Call { get; set; } = null;
          //  public TextBox InnerTextBox { get; set; } = null;
        }

        private void btn_Centrix_Click(object sender, EventArgs e)
        {
            // Process.Start("http://centrex.uplus.co.kr/premium/index.html");

            FrmMN301_Call_PopupSetting f = new FrmMN301_Call_PopupSetting();


            f.Show();



        }

        private void btn_Setting_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtID.Text) && !String.IsNullOrEmpty(txtPassword.Text))
            {
                CallHelper.Instance.Login(txtID.Text, txtPassword.Text);
            }
            // 여기서 접속시도
            // 맞으면 파일로 해당정보 저장하고
            // 틀리면 전화번호/비밀번호 클리어 혹은 이전 정보로 돌아가기
        }

        private void DataList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex < 0)
            //        return;
            //    var Model = DataListSource[e.RowIndex];
            //    if (Model.Call != null)
            //    {
            //        if (e.ColumnIndex == ColumnMemoText.Index)
            //        {
                        
            //            object o = DataList[e.ColumnIndex, e.RowIndex].Value;
            //            if (Model.Call.Memo != o.ToString())
            //            {
            //                Data.Connection((_Connection) =>
            //                {
            //                    using (SqlCommand _Command = _Connection.CreateCommand())
            //                    {
            //                        _Command.CommandText =
            //                        "UPDATE Calls " +
            //                        "SET Memo = @Memo " +
            //                        "WHERE CallId = @CallId";
            //                        _Command.Parameters.AddWithValue("@Memo", o.ToString());
            //                        _Command.Parameters.AddWithValue("@CallId", Model.Call.CallId);
            //                        _Command.ExecuteNonQuery();
            //                    }
            //                });

            //            }

            //            Search();
                          
            //        }

            //    }
            //}
            //catch { }
            
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            Search();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FrmPdfViewer f = new FrmPdfViewer();
            f.ShowDialog();
        }
    }
}
