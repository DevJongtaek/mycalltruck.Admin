using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using mycalltruck.Admin.Class.Extensions;
using System.IO;
using mycalltruck.Admin.UI;

namespace mycalltruck.Admin
{
    public partial class FrmMNCardAccTest : Form
    {
        DESCrypt m_crypt = null;
        DataSets.AppSMSDataSetTableAdapters.em_mmt_tranTableAdapter em_mmt_tranTableAdapter = new DataSets.AppSMSDataSetTableAdapters.em_mmt_tranTableAdapter();
        DataSets.AppSMSDataSetTableAdapters.em_smt_tranTableAdapter em_smt_tranTableAdapter = new DataSets.AppSMSDataSetTableAdapters.em_smt_tranTableAdapter();
        public FrmMNCardAccTest()
        {
            m_crypt = new DESCrypt("12345678");
            InitializeComponent();
        }

        private void FrmMNCardAccTest_Load(object sender, EventArgs e)
        {
            InitializeStorage();
            GetModels();
        }

        #region VIEW
        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var _Model = _BindingList[e.RowIndex];

            if (newDGV1.Columns[e.ColumnIndex] == CheckBox)
            {
               

                if (!String.IsNullOrWhiteSpace(_Model.SMID))
                {
                    var _Cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = false;
                    _Cell.ReadOnly = true;
                }
                else
                {
                    var _Cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                    _Cell.Enabled = true;
                    _Cell.ReadOnly = false;
                }

            }


            if (e.ColumnIndex == colNumber.Index)
            {
                e.Value = (e.RowIndex + 1).ToString("N0");
            }
            else if(e.ColumnIndex == colServiceState.Index)
            {
                switch ((int)e.Value)
                {
                    case 1:
                        e.Value = "제공";
                        break;
                    case 2:
                        e.Value = "대기";
                        break;
                    case 3:
                        e.Value = "등록";
                        break;
                    case 4:
                        e.Value = "보류";
                        break;
                    case 5:
                        e.Value = "해지";
                        break;
                    case 6:
                        e.Value = "심사요청";
                        break;
                    case 7:
                        e.Value = "심사중";
                        break;
                    default:
                        e.Value = "";
                        break;
                }
            }
            else if(e.ColumnIndex == colTestResult.Index)
            {
                if (e.Value == null)
                    e.Value = "";
                else if ((bool?)e.Value == true)
                    e.Value = "성공";
                else
                    e.Value = "실패";
            }
            else if (e.ColumnIndex == colUpdateResult.Index)
            {
                if (e.Value == null)
                    e.Value = "";
                else if ((bool?)e.Value == true)
                    e.Value = "제공";
                else
                    e.Value = "";
            }

            else if (e.ColumnIndex == LgState.Index)
            {
                switch ((int)e.Value)
                {
                    case 1:
                        e.Value = "등록요청";
                        break;
                    case 2:
                        e.Value = "등록재요청";
                        break;
                    case 3:
                        e.Value = "계약중";
                        break;
                    case 4:
                        e.Value = "Key등록완료";
                        break;
                    case 5:
                        e.Value = "데이터오류";
                        break;
                    case 6:
                        e.Value = "기타";
                        break;
                 
                    default:
                        e.Value = "";
                        break;
                }
            }
        }
        #endregion

        #region Storage
        class Model : INotifyPropertyChanged
        {
            private int _DriverId;
            private string _DriverName;
            private string _LoginId;
            private string _PayInputName;
            private int _Servicestate;
            private int _LGstate;
            private string _Code;
            private string _Name;
            private string _Error;
            private bool? _TestResult;
            private bool? _UpdateResult;

            private string _RequestDate;
            public String MID { get; set; }
            public String ClientMobileNo { get; set; }
            public String PayAccountNo { get; set; }

            public String SMID { get; set; }


            public int DriverId
            {
                get
                {
                    return _DriverId;
                }

                set
                {
                    SetField(ref _DriverId, value);
                }
            }

            public string DriverName
            {
                get
                {
                    return _DriverName;
                }

                set
                {
                    SetField(ref _DriverName, value);
                }
            }

            public string LoginId
            {
                get
                {
                    return _LoginId;
                }

                set
                {
                    SetField(ref _LoginId, value);
                }
            }

            public string PayInputName
            {
                get
                {
                    return _PayInputName;
                }

                set
                {
                    SetField(ref _PayInputName, value);
                }
            }

            public int Servicestate
            {
                get
                {
                    return _Servicestate;
                }

                set
                {
                    SetField(ref _Servicestate, value);
                }
            }

            public int LGState
            {
                get
                {
                    return _LGstate;
                }

                set
                {
                    SetField(ref _LGstate, value);
                }
            }

            public string Code
            {
                get
                {
                    return _Code;
                }

                set
                {
                    SetField(ref _Code, value);
                }
            }

            public string Name
            {
                get
                {
                    return _Name;
                }

                set
                {
                    SetField(ref _Name, value);
                }
            }
            public string RequestDate
            {
                get
                {
                    return _RequestDate;
                }

                set
                {
                    SetField(ref _RequestDate, value);
                }
            }
            public string Error
            {
                get
                {
                    return _Error;
                }

                set
                {
                    SetField(ref _Error, value);
                }
            }

            public bool? TestResult
            {
                get
                {
                    return _TestResult;
                }

                set
                {
                    SetField(ref _TestResult, value);
                }
            }

            public bool? UpdateResult
            {
                get
                {
                    return _UpdateResult;
                }

                set
                {
                    SetField(ref _UpdateResult, value);
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

        }
        BindingList<Model> _BindingList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _BindingList;
        }
        #endregion

        #region UPDATE
        private void GetModels()
        {
            _BindingList.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _TradeCommand = _Connection.CreateCommand())
                {
                    //_TradeCommand.CommandText = "SELECT Drivers.DriverId, Drivers.Name as DriverName, Drivers.LoginId, Drivers.PayInputName, Drivers.Servicestate, Clients.Code, Clients.Name, Drivers.LoginId, Drivers.LGState, Clients.MobileNo as ClientMobileNo, Drivers.PayAccountNo FROM Drivers JOIN Clients ON Drivers.CandidateId = Clients.ClientId  WHERE Drivers.Servicestate = 7 AND Drivers.LGState = 4";
                    _TradeCommand.CommandText = "SELECT Drivers.DriverId, Drivers.Name as DriverName, Drivers.LoginId, Drivers.PayInputName, Drivers.Servicestate, Clients.Code, Clients.Name, Drivers.LoginId, Drivers.LGState, Clients.MobileNo as ClientMobileNo, Drivers.PayAccountNo ,Drivers.RequestDate,ISNULL(Drivers.Mid,N'') FROM Drivers JOIN Clients ON Drivers.CandidateId = Clients.ClientId  WHERE Drivers.Servicestate = 7";
                    using (SqlDataReader _Reader = _TradeCommand.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            var DriverId = _Reader.GetInt32(0);
                            if (_BindingList.Any(c => c.DriverId == DriverId))
                                continue;
                            var Added = new Model
                            {
                                DriverId = DriverId,
                                DriverName = _Reader.GetStringN(1),
                                LoginId = _Reader.GetStringN(2),
                                PayInputName = _Reader.GetStringN(3),
                                Servicestate = _Reader.GetInt32(4),
                                Code = _Reader.GetStringN(5),
                                Name = _Reader.GetStringN(6),
                                MID = _Reader.GetStringN(7),
                                LGState = _Reader.GetInt32(8),
                                ClientMobileNo = _Reader.GetStringN(9),
                                PayAccountNo = _Reader.GetStringN(10),
                                RequestDate = _Reader.GetDateTime(11).ToString("d").Replace("-","/"),
                                SMID = _Reader.GetStringN(12),
                            };
                            _BindingList.Add(Added);
                        }
                    }
                }
                _Connection.Close();
            }
        }
        #endregion

        #region ACTION

        private void btnTest_Click(object sender, EventArgs e)
        {
            pnProgress.Visible = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _AuthKeyCommand = _Connection.CreateCommand())
                        using (SqlCommand _UpdateCommand = _Connection.CreateCommand())
                        {
                            _AuthKeyCommand.CommandText = "UPDATE Drivers SET AuthKey = @AuthKey WHERE DriverId = @DriverId";
                            _AuthKeyCommand.Parameters.Add("@AuthKey", SqlDbType.NVarChar);
                            _AuthKeyCommand.Parameters.Add("@DriverId", SqlDbType.Int);
                            _UpdateCommand.CommandText = "UPDATE Drivers SET AuthKey = N'', TESTError = @Error WHERE DriverId = @DriverId";
                            _UpdateCommand.Parameters.Add("@Error", SqlDbType.NVarChar);
                            _UpdateCommand.Parameters.Add("@DriverId", SqlDbType.Int);
                            foreach (var _Model in _BindingList.Reverse())
                            {
                                if (_Model.Servicestate != 7 )
                                    //if (_Model.Servicestate != 7 || _Model.LGState != 4)
                                        continue;
                                var _AuthKey = Guid.NewGuid().ToString().Substring(0, 10);
                                var _DriverId = _Model.DriverId;
                                var _CardNo = "5584204000616426";
                                var _CardDate = "2106";
                                var _CardPan = "52";
                                var _MID = _Model.MID;
                                _AuthKeyCommand.Parameters["@AuthKey"].Value = _AuthKey;
                                _AuthKeyCommand.Parameters["@DriverId"].Value = _DriverId;
                                _AuthKeyCommand.ExecuteNonQuery();
                                WebClient mWebClient = new WebClient();
                                mWebClient.Encoding = Encoding.UTF8;
                                String _AccountNo = m_crypt.Decrypt(_Model.PayAccountNo).Replace("\0", "");
                                string Parameter = "?sPrameter=" + String.Join("^", new object[] { _DriverId, _AuthKey, _CardNo, _CardDate, _CardPan, _MID, _AccountNo });
                                var r = mWebClient.DownloadString(new Uri("http://m.cardpay.kr/Pay/CardTEST" + Parameter));

                               // var r = mWebClient.DownloadString(new Uri("http://localhost/Pay/CardTEST" + Parameter));
                                bool _r = false;
                                if (bool.TryParse(r, out _r))
                                {
                                    if (_r)
                                    {
                                        _Model.TestResult = true;
                                        _Model.Error = "";
                                    }
                                    else
                                    {
                                        _Model.TestResult = false;
                                        _Model.Error = "알 수 없는 오류로 테스트를 실패하였습니다.";
                                    }
                                }
                                else
                                {
                                    _Model.TestResult = false;
                                    _Model.Error = r;
                                }
                                _UpdateCommand.Parameters["@Error"].Value = _Model.Error;
                                _UpdateCommand.Parameters["@DriverId"].Value = _DriverId;
                                _UpdateCommand.ExecuteNonQuery();
                            }
                        }
                        _Connection.Close();
                    }
                    Invoke(new Action(() => MessageBox.Show("승인테스트를 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                }
                catch (Exception ex)
                {
                   // Invoke(new Action(() => MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                   Invoke(new Action(() => MessageBox.Show("승인테스트 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                }
                Invoke(new Action(() => pnProgress.Visible = false));
            });
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
         

            //DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYMID);

            //if (di.Exists == false)
            //{
            //    di.Create();
            //}
            //var fileString = DateTime.Now.ToString("yyyyMMddhhmmss");
            //var FileName = System.IO.Path.Combine(di.FullName, fileString);
            //FileInfo file = new FileInfo(FileName + ".txt");
            //if (!file.Exists)
            //{
            //    FileStream fs = file.Create();
            //    fs.Close();
                     
               
            //}




            pnProgress.Visible = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        _Connection.Open();
                        using (SqlCommand _Command = _Connection.CreateCommand())
                        {
                            _Command.CommandText = "UPDATE Drivers SET ServiceState = 1 WHERE DriverId = @DriverId";
                            _Command.Parameters.AddWithValue("@RequestDate", DateTime.Now.ToString("yyyy-MM-dd"));
                            _Command.Parameters.Add("@DriverId", SqlDbType.Int);
                            foreach (var _Model in _BindingList)
                            {
                                if (_Model.TestResult == true && _Model.Servicestate != 1)
                                {
                                    _Command.Parameters["@DriverId"].Value = _Model.DriverId;
                                    _Command.ExecuteNonQuery();
                                    _Model.Servicestate = 1;
                                    _Model.UpdateResult = true;
                                    if (!String.IsNullOrWhiteSpace(_Model.ClientMobileNo) && Regex.IsMatch(_Model.ClientMobileNo.Replace("-", ""), @"^01[0,1,6,7,8,9]\d{3,4}\d{4}$"))
                                    {
                                        var Message = $"'{_Model.DriverName}'님 등록심사 완료. 차세로 결제가능 합니다.";
                                        em_smt_tranTableAdapter.Insert(DateTime.Now, Message, "028535111", "0", _Model.ClientMobileNo.Replace("-", ""), "0");
                                    }


                                    //FileStream fs = new FileStream(file.ToString(), FileMode.Append, FileAccess.Write);
                                    ////FileMode중 append는 이어쓰기. 파일이 없으면 만든다.
                                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                                    //sw.WriteLine(_Model.LoginId + " = e62974de0fdf4f98b9edaa4428eec39a");
                                    //sw.Flush();
                                    //sw.Close();
                                    //fs.Close();

                                

                                }





                            }
                        }
                        _Connection.Close();



                    }
                    Invoke(new Action(() => MessageBox.Show("제공적용을 완료하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    Invoke(new Action(() => GetModels()));
                }
                catch (Exception)
                {
                    Invoke(new Action(() => MessageBox.Show("제공적용 중 오류가 발생하였습니다. 잠시 후 다시 한번 시도해 주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                }
                Invoke(new Action(() => pnProgress.Visible = false));
            });
        }
        #endregion

        private void btnMID_Click(object sender, EventArgs e)
        {
            BindingList<Model> Datas = new BindingList<Model>();

            for (int i = 0; i < _BindingList.Count(); i++)
            {
                var _Cell = newDGV1[CheckBox.Index, i] as DataGridViewDisableCheckBoxCell;
                if (_Cell.Value != null && (bool)_Cell.Value)
                {
                    Datas.Add(_BindingList[i]);


                }


            }

            if (Datas.Any())
            {

                DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYMID);

                if (di.Exists == false)
                {
                    di.Create();
                }
                var fileString = DateTime.Now.ToString("yyyyMMddHHmmss");
                var FileName = System.IO.Path.Combine(di.FullName, fileString);
                FileInfo file = new FileInfo(FileName + ".txt");
                if (!file.Exists)
                {
                    FileStream fs2 = file.Create();
                    fs2.Close();


                }




                foreach (var _Model in Datas)
                {
                    FileStream fs = new FileStream(file.ToString(), FileMode.Append, FileAccess.Write);
                    //FileMode중 append는 이어쓰기. 파일이 없으면 만든다.
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                    sw.WriteLine(_Model.LoginId + " = e62974de0fdf4f98b9edaa4428eec39a");
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText =
                            "UPDATE Drivers SET  Mid = @Mid " +
                            "WHERE LoginId = @LoginId ";
                        cmd.Parameters.AddWithValue("@Mid", "e62974de0fdf4f98b9edaa4428eec39a");
                        cmd.Parameters.AddWithValue("@LoginId", _Model.LoginId);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }

                }
                try
                {
                    for (int i = 0; i < newDGV1.RowCount; i++)
                    {

                        newDGV1[CheckBox.Index, i].Value = false;
                    }
                    chkAllSelect.Checked = false;

                    System.Diagnostics.Process.Start(FileName+".txt");
                }
                catch { }
            }
            else
            {
                MessageBox.Show("먼저 생성할 항목들을 선택하여 주십시오.");
            }

        }

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < newDGV1.RowCount; i++)
            {
                var cell = newDGV1[CheckBox.Index, i] as DataGridViewDisableCheckBoxCell;
                if (cell.Enabled)
                    newDGV1[CheckBox.Index, i].Value = chkAllSelect.Checked;
            }


         
        }

        private void newDGV1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect1 = newDGV1.GetColumnDisplayRectangle(CheckBox.Index, true);
            chkAllSelect.Location = new Point(rect1.Location.X + 2, rect1.Location.Y + 4);
            if (rect1.Width == 0)
                chkAllSelect.Visible = false;
            else
                chkAllSelect.Visible = true;
        }

        private void newDGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == CheckBox.Index)
            {
                DataGridViewCheckBoxCell _Cell = newDGV1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableCheckBoxCell;
                _Cell.Value = _Cell.Value == null || !((bool)_Cell.Value);
                newDGV1.RefreshEdit();
                newDGV1.NotifyCurrentCellDirty(true);
            }
        }

        private void btnLinkTest_Click(object sender, EventArgs e)
        {
            FrmMNTest frmMNTest = new FrmMNTest();

            frmMNTest.Show();
        }
    }
}
