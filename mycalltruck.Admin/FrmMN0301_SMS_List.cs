using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
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
    public partial class FrmMN0301_SMS_List : Form
    {
        BindingList<Model> DataListSource = new BindingList<Model>();

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





                    DataList.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }
        public FrmMN0301_SMS_List()
        {
            InitializeComponent();
            CTimeSFilter.Value = DateTime.Now.Date;
            cmbCallGubun.SelectedIndex = 0;
            cmbSearch.SelectedIndex = 0;
            cmb_Gubun.SelectedIndex = 0;
            Search();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
        }

        private void Search()
        {
            var CTimeBegin = CTimeSFilter.Value.Date;
            var CTimeEnd = CTimeEFilter.Value.Date.AddDays(1);
           
            DataListSource.Clear();
           
            Data.Connection((_Connection) =>
            {
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = "select CallSmsId,CTime,OriginalPhoneNo,SmsResult,ResultMessage,ClientId,LoginId,CustomerId,DriverId,CarNo,SangHo,Msg FROM(" +
                    " select CallSms.CallSmsId, CallSms.CTime, CallSms.OriginalPhoneNo, CallSms.SmsResult, CallSms.ResultMessage, CallSms.ClientId, CallSms.LoginId, CallSms.CustomerId, CallSms.DriverId, ISNULL(Drivers.CarNo, N'') as CarNo, CASE WHEN CallSms.CustomerId > 0 THEN ISNULL(Customers.SangHo,N'') ELSE ISNULL(Drivers.Name, '') END as SangHo, CallSms.Msg from CallSms" +
                    " LEFT JOIN Drivers ON CallSms.DriverId = Drivers.DriverId " +
                    " LEFT JOIN Customers ON CallSms.CustomerId = Customers.CustomerId)a ";

                    List<String> WhereStringList = new List<string>();
                    WhereStringList.Add("ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);

                    WhereStringList.Add("LoginId = @LoginId");
                    _Command.Parameters.AddWithValue("@LoginId", LocalUser.Instance.LogInInformation.LoginId);

                    WhereStringList.Add(" CTime >= @CTimeBegin");
                    _Command.Parameters.AddWithValue("@CTimeBegin", CTimeBegin);

                    WhereStringList.Add(" CTime < @CTimeEnd");
                    _Command.Parameters.AddWithValue("@CTimeEnd", CTimeEnd);

                    if(cmb_Gubun.Text =="거래처")
                    {
                        WhereStringList.Add(" CustomerId > 0 ");
                        
                    }
                    else if (cmb_Gubun.Text == "차주")
                    {
                        WhereStringList.Add(" DriverId > 0 ");
                        
                    }

                    if (cmbCallGubun.Text == "성공")
                    {
                        WhereStringList.Add(" SmsResult = @Gubun ");
                        _Command.Parameters.AddWithValue("@Gubun", "성공");
                    }
                    else if (cmbCallGubun.Text == "실패")
                    {
                        WhereStringList.Add(" SmsResult != @Gubun ");
                        _Command.Parameters.AddWithValue("@Gubun", "성공");
                    }


                    if (cmbSearch.Text == "상호/이름")
                    {
                        WhereStringList.Add(string.Format("SangHo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmbSearch.Text == "차량번호")
                    {
                        WhereStringList.Add(string.Format("CarNo Like  '%{0}%'", txt_Search.Text));
                    }
                    else if (cmbSearch.Text == "핸드폰번호")
                    {
                        WhereStringList.Add(string.Format("REPLACE(OriginalPhoneNo,'-','') Like  '%{0}%'", txt_Search.Text.Replace("-", "")));
                    }


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }




                    SelectCommandText += " ORDER BY CallSmsId DESC ";

                    
                    _Command.CommandText = SelectCommandText;



                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            CallSms call = new CallSms
                            {
                                CallSmsId = _Reader.GetInt32(0),
                                CTime = _Reader.GetDateTime(1),
                                OriginalPhoneNo = _Reader.GetString(2),
                                SmsResult = _Reader.GetString(3),
                                ResultMessage = _Reader.GetString(4),
                                ClientId = _Reader.GetInt32(5),
                                LoginId = _Reader.GetString(6),
                                CustomerId = _Reader.GetInt32(7),
                                DriverId = _Reader.GetInt32(8),

                                CarNo = _Reader.GetString(9),
                                SangHo = _Reader.GetString(10),
                                Msg = _Reader.GetString(11),

                            };

                            DataListSource.Add(new Model { CallSms = call });


                        }
                    }
                }
            });
            DataList.AutoGenerateColumns = false;
            DataList.DataSource = DataListSource;
        }

       

        private void DataList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var Model = DataListSource[e.RowIndex];
            if (Model.CallSms != null)
            {
                if (e.ColumnIndex == ColumnNo.Index)
                    e.Value = (DataListSource.Count - e.RowIndex).ToString("N0");
                else if (e.ColumnIndex == ColumnCTime.Index)
                    e.Value = Model.CallSms.CTime.ToString("yyyy-MM-dd HH:mm:ss");
                else if (e.ColumnIndex == ColumnDiv.Index)
                {
                    if (Model.CallSms.CustomerId > 0)
                        e.Value = "거래처";
                    else if (Model.CallSms.DriverId > 0)
                        e.Value = "차주";
                    else
                        e.Value = "";
                }
                else if (e.ColumnIndex == ColumnTarget.Index)
                    e.Value = Model.CallSms.SangHo;
                else if (e.ColumnIndex == ColumnCarNo.Index)
                    e.Value = Model.CallSms.CarNo;

                else if (e.ColumnIndex == ColumnOriginalPhoneNo.Index)
                {
                    string _S = Model.CallSms.OriginalPhoneNo;
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
              
                else if (e.ColumnIndex == ColumnMessage.Index)
                    e.Value = Model.CallSms.Msg;
                else if (e.ColumnIndex == ColumnSmsResult.Index)
                    e.Value = Model.CallSms.SmsResult;


            }
        }

        private void DataList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
          
        }

       
        class Model
        {
            public CallSms CallSms { get; set; } = null;
           
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

            var Model = DataListSource[e.RowIndex];



            if (e.ColumnIndex == ColumnBtnSms.Index)
            {

                FrmSMSResult f = new FrmSMSResult(Model.CallSms.CallSmsId,Model.CallSms.Msg);

                f.Show();



            }


        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            Search();
        }

        private void btn_Centrix_Click(object sender, EventArgs e)
        {
            FrmMN301_Call_PopupSetting f = new FrmMN301_Call_PopupSetting();


            f.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FrmPdfViewer f = new FrmPdfViewer();
            f.ShowDialog();
        }
    }
}
