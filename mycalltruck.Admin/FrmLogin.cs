using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace mycalltruck.Admin
{
    public partial class FrmLogin : Form
    {
        Helper mHelper = Helper.Instance;
        LocalUser[] _LocalUsers = null;
        public String AuthKey { get; set; }
        public FrmLogin()
        {
            InitializeComponent();
            mHelper.Logoff();
            this.DialogResult = DialogResult.No;
            //인증서 목록을 불러온다.
            _LocalUsers = LocalUser.GetLocalUsers();
            if (_LocalUsers.Length == 0)
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                folderPath = Path.Combine(folderPath, Application.ProductName);
                if (Directory.Exists(folderPath) == false) Directory.CreateDirectory(folderPath);
                //만약에 이전 인증서가 있으면 지우지 않고 새로 만든다
                //어짜피 인증서들을 검색해서, 로그인한다.
                //새로 인스톨하는 경우에는 이전의 인증서를 다지운다.
                DirectoryInfo folder = new DirectoryInfo(folderPath);
                int no = folder.GetFiles("LocalUser*.Xml").Length;
                string fileName = string.Empty;
                fileName = "LocalUser.Xml";
                LocalUser.Instance.FileName = fileName;
                string filePath = Path.Combine(folderPath, fileName);
                FileInfo xmlFile = new FileInfo(filePath);
                //XML 문서 만들기
                xmlFile.Delete();
                XmlSerializer serializer = new XmlSerializer(typeof(LocalUser));
                TextWriter writer = new StreamWriter(xmlFile.FullName);
                serializer.Serialize(writer, LocalUser.Instance);
                writer.Close();
            }
            else
            {
                try
                {
                    LocalUser.SetInstance(_LocalUsers[0]);
                }
                catch { }
            }
           // cmb_UserGubun.SelectedIndex = LocalUser.Instance.PersonalOption.UserGubun;
            if (LocalUser.Instance.PersonalOption.IDSave)
            {
                txtID.Text = LocalUser.Instance.PersonalOption.UserID;
                //if(LocalUser.Instance.PersonalOption.UserGubun == 2)
                //{
                //    rdo_Customer.Checked = true;
                //}
                //else
                //{
                //    rdo_Client.Checked = true;
                //}
                chkIDSave.Checked = true;
                txtPassword.Focus();
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                (this as Form).ThisMessageBox("아이디를 입력해주세요.");
                return;
            }
            if (txtPassword.Text == "")
            {
                (this as Form).ThisMessageBox("비밀번호를 입력해주세요.");
                return;
            }
            #region Login
            bool IsLogin = false;
            int Status = 0;
            // 운수사
            if (txtID.Text != "root")
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    int ClientId = 0;
                    int SubClientId = 0;
                    int ClientUserId = 0;
                    bool IsSubClient = false;
                    bool IsAgent = false;
                    int CustomerId = 0;
                    int CustomerUserId = 0;
                    int CustomerTeamId = 0;
                    int BizGubun = 0;
                    string CargoLoginId = "";

                    var Query1 = "SELECT COUNT(*) FROM " +
                "(Select LoginId From ClientUsers " +
                " union " +
                " Select LoginId From clients ) as a" +
                " Where LoginId = @LoginId ";


                    bool IsDuplicated = false;
                    
                        cn.Open();

                        SqlCommand cmd1 = cn.CreateCommand();
                        cmd1.CommandText = Query1;
                        cmd1.Parameters.AddWithValue("@LoginId", txtID.Text);
                       
                        if (Convert.ToInt32(cmd1.ExecuteScalar()) > 0)
                        {
                            IsDuplicated = true;
                        }
                        //if (!IsDuplicated)
                        //{
                        //    SqlCommand cmd2 = cn.CreateCommand();
                        //    cmd2.CommandText = Query2;
                        //    cmd2.Parameters.AddWithValue("@LoginId", txt_LoginId.Text);
                        //    cmd2.Parameters.AddWithValue("@ClientId", txt_ClientId.Text);
                        //    if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0)
                        //    {
                        //        IsDuplicated = true;
                        //    }
                        //}
                        cn.Close();



                    #region 운송주선사
                    //if (rdo_Client.Checked)
                    if (IsDuplicated)
                    
                   
                    {


                        using (SqlCommand ClientIdCommand = cn.CreateCommand())
                        using (SqlCommand Command = cn.CreateCommand())
                        {
                            cn.Open();
                            ClientIdCommand.CommandText =
                                @"SELECT ClientId, 0, 0, CONVERT(bit, 0) ,Status,ISNULL(CargoLoginId,'') FROM Clients WHERE LoginId = @UserID AND Password = @UserPassword UNION SELECT ClientUsers.ClientId, ISNULL(SubClientId, 0), ClientUserId, IsAgent,clients.Status,ISNULL(ClientUsers.CargoLoginId,ISNULL(Clients.CargoLoginId,'')) FROM ClientUsers JOIN Clients ON Clients.ClientId = ClientUsers.ClientId  WHERE ClientUsers.LoginId = @UserID AND ClientUsers.Password = @UserPassword AND ISNULL(IsRegister, 0) <> 1";
                            ClientIdCommand.Parameters.Add(new SqlParameter("@UserID", txtID.Text));
                            ClientIdCommand.Parameters.Add(new SqlParameter("@UserPassword", txtPassword.Text));
                            using (SqlDataReader Reader = ClientIdCommand.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                if (Reader.Read())
                                {
                                    IsLogin = true;
                                    ClientId = Reader.GetInt32(0);
                                    SubClientId = Reader.GetInt32(1);
                                    ClientUserId = Reader.GetInt32(2);
                                    IsSubClient = SubClientId > 0;
                                    IsAgent = !Reader.IsDBNull(3) && Reader.GetBoolean(3);
                                    Status = Reader.GetInt32(4);
                                    CargoLoginId = Reader.GetStringN(5);
                                }
                            }
                            if (IsLogin)
                            {
                                Command.CommandText =
                                    @"SELECT ExcelType, DriverType, ContType, OrderType, NoticeDriver, NoticeCnt
                                    , AllowFPIS_In, AllowSMS, AllowOrder, AllowFPIS, AllowSub, AllowMultiCustomer, AllowTax, Name,HideAddTrade,HideAddSales,SmsYn
                                FROM Clients WHERE ClientId = @ClientId";
                                Command.Parameters.AddWithValue("@ClientId", ClientId);
                                using (SqlDataReader Reader = Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (Reader.Read())
                                    {
                                        LocalUser.Instance.LogInInformation = new LogInInformation(IsLogin, false, txtID.Text, ClientId, SubClientId, ClientUserId, IsSubClient, IsAgent,
                                            _ExcelType: Reader.GetInt32Z(0),
                                            _DriverType: Reader.GetInt32Z(1),
                                            _ContType: Reader.GetInt32Z(2),
                                            _OrderType: Reader.GetInt32Z(3),
                                            _NoticeDriver: Reader.GetInt32Z(4),
                                            _NoticeCnt: Reader.GetInt32Z(5),
                                            _AllowFPIS_In: Reader.GetBooleanZ(6),
                                            _AllowSMS: Reader.GetBooleanZ(7),
                                            _AllowOrder: Reader.GetBooleanZ(8),
                                            _AllowFPIS: Reader.GetBooleanZ(9),
                                            _AllowSub: Reader.GetBooleanZ(10),
                                            _AllowMultiCustomer: Reader.GetBooleanZ(11),
                                            _AllowTax: Reader.GetBooleanZ(12),
                                            _ClientName: Reader.GetStringN(13),
                                           _HideAddTrade: Reader.GetBooleanZ(14),
                                            _HideAddSales: Reader.GetBooleanZ(15),
                                            _SmsYn: Reader.GetBooleanZ(16),

                                            _IsClient: true,
                                            _CustomerId: 0,
                                            _CustomerUserId: 0,
                                            _CustomerTeamId : 0,
                                            _BizGubun : 0,
                                            _CargoLoginId: CargoLoginId


                                            );

                                    }
                                }
                            }
                            cn.Close();
                        }
                    }

                    #endregion
                    #region 화주
                    else
                    {
                        using (SqlCommand CustomerCommand = cn.CreateCommand())
                        using (SqlCommand Command = cn.CreateCommand())
                        {
                            cn.Open();

                            CustomerCommand.CommandText =
                                @"SELECT ClientId, 0, 0, CONVERT(bit, 0),CustomerId,0 ,BizGubun FROM Customers WHERE LoginId = @UserID AND Password = @UserPassword 
                                UNION SELECT CustomerAddPhone.ClientId, 0, idx,clients.Status,CustomerAddPhone.CustomerId,ISNULL(TeamId,0),Customers.BizGubun FROM CustomerAddPhone 
                                JOIN Clients ON Clients.ClientId = CustomerAddPhone.ClientId 
                                JOIN Customers ON CustomerAddPhone.CustomerId = Customers.CustomerId
                                WHERE CustomerAddPhone.LoginId = @UserID AND CustomerAddPhone.Password = @UserPassword  ";
                            CustomerCommand.Parameters.Add(new SqlParameter("@UserID", txtID.Text));
                            CustomerCommand.Parameters.Add(new SqlParameter("@UserPassword", txtPassword.Text));
                            using (SqlDataReader Reader = CustomerCommand.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                if (Reader.Read())
                                {
                                    IsLogin = true;
                                    ClientId = Reader.GetInt32(0);
                                    SubClientId = Reader.GetInt32(1);
                                    CustomerUserId = Reader.GetInt32(2);
                                    IsSubClient = false;
                                    IsAgent = false;
                                    CustomerId = Reader.GetInt32(4);
                                    CustomerTeamId = Reader.GetInt32(5);
                                    BizGubun = Reader.GetInt32(6);
                                    //Status = Reader.GetInt32(4);
                                }
                            }
                            if (IsLogin)
                            {
                                Command.CommandText =
                                    @"SELECT 0, 0, 0, 0, customerId, 0
                                    , 0, 0, 0, 0, 0, 0, 0, SangHo,0,0,0
                                FROM Customers WHERE CustomerId = @CustomerId";
                                Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                                using (SqlDataReader Reader = Command.ExecuteReader(CommandBehavior.SingleRow))
                                {
                                    if (Reader.Read())
                                    {
                                        LocalUser.Instance.LogInInformation = new LogInInformation(IsLogin, false, txtID.Text, ClientId, SubClientId, 0, IsSubClient, IsAgent,
                                            _ExcelType: 3,
                                            _DriverType: 1,
                                            _ContType: 1,
                                            _OrderType: 1,
                                            _NoticeDriver: 1,
                                            _NoticeCnt: 0,
                                            _AllowFPIS_In: false,
                                            _AllowSMS: false,
                                            _AllowOrder: true,
                                            _AllowFPIS: false,
                                            _AllowSub: false,
                                            _AllowMultiCustomer: false,
                                            _AllowTax: false,
                                            _ClientName: Reader.GetStringN(13),
                                           _HideAddTrade: false,
                                            _HideAddSales: false,
                                            _SmsYn: false,

                                            _IsClient: false,
                                            _CustomerId : Reader.GetInt32(4),
                                            _CustomerUserId : CustomerUserId,
                                            _CustomerTeamId : CustomerTeamId,
                                            _BizGubun : BizGubun,
                                              _CargoLoginId: ""
                                            );

                                    }
                                }
                            }
                            cn.Close();
                        }
                    }
                    #endregion
                }
            }
            // 관리자
            else
            {
                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    Connection.Open();
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText =
                        @"SELECT UserId FROM Users WHERE LoginId = @LoginId AND Password = @Password";
                    Command.Parameters.AddWithValue("@LoginId", txtID.Text);
                    Command.Parameters.AddWithValue("@Password", txtPassword.Text);
                    var DataReader = Command.ExecuteReader();
                    if (DataReader.Read())
                    {
                        IsLogin = true;
                        bool IsAdmin = true;
                        String LoginId = txtID.Text;
                        int UserId = DataReader.GetInt32(0);
                        LocalUser.Instance.LogInInformation = new LogInInformation(IsLogin, IsAdmin, LoginId, UserId);
                    }
                    Connection.Close();
                }
            }
            if (IsLogin)
            {
                int _MenuCount = 0;
                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    Connection.Open();
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText =
                        @"SELECT COUNT(*) FROM UserAuthority WHERE UserId = @UserId ";
                    Command.Parameters.AddWithValue("@UserId", txtID.Text);
                 
                    var DataReader = Command.ExecuteReader();
                    if (DataReader.Read())
                    {
                        _MenuCount = DataReader.GetInt32(0);

                    }
                    Connection.Close();
                    Connection.Open();
                    if (_MenuCount != 35)
                    {
                        using (SqlCommand _Command = Connection.CreateCommand())
                        {
                            _Command.CommandText = "DELETE UserAuthority WHERE UserId = @LoginId ";
                            _Command.CommandText += " Insert UserAuthority (UserId, MenuCode, ReadAuth, WriteAuth, Memo, MenuName)" +
                                                  "SELECT @LoginId ,MenuCode,0,1,'' ,MenuName FROM MenuList";
                            _Command.Parameters.AddWithValue("@LoginId", txtID.Text);

                            _Command.ExecuteNonQuery();
                        }

                    }

                    Connection.Close();



                }

                if (chkIDSave.Checked)
                {
                    LocalUser.Instance.PersonalOption.IDSave = true;
                    LocalUser.Instance.PersonalOption.UserID = txtID.Text;
                }
                else
                {
                    LocalUser.Instance.PersonalOption.IDSave = false;
                    LocalUser.Instance.PersonalOption.UserID = "";
                }
                if(txtID.Text == "root")
                {
                    LocalUser.Instance.PersonalOption.UserGubun = 1;
                }
                else
                {
                    //if (rdo_Client.Checked)
                    //{
                    //    LocalUser.Instance.PersonalOption.UserGubun = 0;
                    //}
                    //else
                    //{
                    //    LocalUser.Instance.PersonalOption.UserGubun = 2;

                    //}
                }
                
                LocalUser.Instance.Write();



                LocalUser.Instance.PersonalOption.CUSTOMER = "C:\\차세로\\거래처관리";
                LocalUser.Instance.PersonalOption.MYMID = "C:\\차세로\\MID";
                LocalUser.Instance.PersonalOption.MYBANKNEW = "C:\\차세로\\대량이체";
                LocalUser.Instance.PersonalOption.MYCAR = "C:\\차세로\\배차관리";
                LocalUser.Instance.PersonalOption.TAX = "C:\\차세로\\세무";
                LocalUser.Instance.PersonalOption.DRIVER = "C:\\차세로\\차량관리";
                LocalUser.Instance.PersonalOption.MYTRADEFILE = "C:\\차세로\\첨부파일";



                LocalUser.Instance.Write();


                if (LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    mHelper.AdminLogin();
                }
                else
                {
                    if (LocalUser.Instance.LogInInformation.IsClient)
                    {
                        if (Status != 2)
                        {
                            (this as Form).ThisMessageBox($"\r\n· 상호 : {LocalUser.Instance.LogInInformation.ClientName}\r\n· 아이디 : {LocalUser.Instance.LogInInformation.LoginId}\r\n\r\n위 회원은 현재 대기상태 입니다.\r\n정회원으로 등록하시려면, 아래로 문의바랍니다.\r\n\r\n☎ 콜 센터 : 1661-6090");
                            return;

                        }

                        //#if DEBUG
                        //                    Helper.Instance.IsLogined = true;
                        //                    DialogResult = DialogResult.Yes;
                        //                    Close();
                        //                    return;
                        //#endif
                        //if (mHelper.IsAnotherLoginedForClient())
                        //{
                        //    if (MessageBox.Show("같은 아이디로 다른 사람이 사용중에 있습니다.\n강제로 로그아웃 하시겠습니까?", "카드페이", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        //    {
                        //        mHelper.ForceLogoutAnotherUserForClient();
                        //    }
                        //    else
                        //    {
                        //        return;
                        //    }
                        //}qwe

                        
                        mHelper.ClientLogin();
                    }
                }
                DialogResult = DialogResult.Yes;
                Close();
            }
            else
            {
                (this as Form).ThisMessageBox("사용자 아이디와 비밀번호를 확인해 주십시오. \r(* 앱 화물등록 아이디로는 로그인 할 수 없습니다.)");
            }
            #endregion
        }

        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
           
            FrmMNRegister _Form = new FrmMNRegister();
            //_Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void lbl_register_Click(object sender, EventArgs e)
        {
            FrmMNRegister _Form = new FrmMNRegister();
            //_Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            FrmMNRegister _Form = new FrmMNRegister();
            //_Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnLogIn_Click(null, null);
            }
        }
        private Point mousePoint;
        private void FrmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void FrmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void FrmLogin_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
