using mycalltruck.Admin.Class;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.CMDataSetTableAdapters;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using mycalltruck.Admin.Class.Extensions;
using System.Data.SqlClient;
using System.ComponentModel;

namespace mycalltruck.Admin
{
    public partial class FrmMDI : Form
    {

        DataSets.ShareOrderDataSet ShareOrderDataSet = DataSets.ShareOrderDataSet.Instance;
        Helper mHelper = Helper.Instance;
        Timer mTimer = new Timer();
        bool IsLogined = false;
        FrmHelp mFrmHelp = null;
        FrmHelp_Youtube mFrmHelp_Youtube = null;
        private static FrmMDI THIS = null;
        private bool FormClosingBind = false;
        public static Dialog_CustomerImage Dialog_CustomerImage_Instance = new Dialog_CustomerImage();
        Rectangle closeButtonPosition;
        int HoveredTabIndex;
        Bitmap CloseImageDynamic = Properties.Resources.hover;
        bool isMouseLeftDown = false;
        public FrmMDI()
        {
            Properties.Settings.Default.TruckConnectionString = "Data Source=222.231.9.253,2899;Initial Catalog=Truck;Persist Security Info=True;User ID=edubillsys;Password=edubillsysdb2202#$";

            InitializeComponent();


            Opacity = 0;
            ShowInTaskbar = false;


            THIS = this;


        }



        private void InitTimer()
        {
            mTimer.Interval = 1000;
            mTimer.Tick += mTimer_Tick;
            mTimer.Enabled = true;
        }

        private void StopTimer()
        {
            mTimer.Enabled = false;
        }


        void mTimer_Tick(object sender, EventArgs e)
        {
            if (!mHelper.IsLogined)
                return;
            if (mTimer.Enabled == false)
                return;
            mTimer.Enabled = false;
            if (mHelper.IsAuthenticated())
            {
                mTimer.Enabled = true;
            }
            else
            {
                //Invoke(new Action(() =>
                //{
                //    MessageBox.Show("로그 아웃 되어, 종료합니다.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    isClosed = true;
                //    Close();
                //}));
            }
        }

        private void FrmMDI_Shown(object sender, EventArgs e)
        {
            //#if DEBUG
            //            Dialog_DEBUG d = new Dialog_DEBUG();
            //            d.ShowDialog();
            //#endif

            LogOut_Click(null, null);
        }



        bool isClosed = false;
        void FrmMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosed == false)
            {
                try
                {
                    //if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    //{
                    //    DriverRepository mDriverRepository = new DriverRepository();
                    //    if (mDriverRepository.HasUnregistrations())
                    //    {
                    //        MessageBox.Show(
                    //            "기사님의 카드 사용 등록을 위해서, 본인의 약관 동의가 필요합니다." + Environment.NewLine + Environment.NewLine +
                    //            "약관 동의 방법은 차세로 앱을 설치한 후 로그인 하면, 확인 할 수 있습니다." + Environment.NewLine + Environment.NewLine +
                    //            "약관 동의가 필요한 기사님은 차량관리 화면에서 [서비스상태: 등록, 첨부 파일: 있음] 조건으로 조회하시면, 세부항목에서 확인할 수 있습니다.",
                    //            "차세로", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                    //}



                    if (MessageBox.Show("종료버튼을 누르셨습니다.\n정말 종료하시겠습니까?", "차세로", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        e.Cancel = false;
                        isClosed = true;
                    }
                }
                catch { isClosed = true; }
            }
            if (isClosed)
            {
                mHelper.Logoff();
                CallHelper.Instance.Dispose();
            }
        }

        Form _Form = null;
        public void Menu_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem senderAsItem = sender as ToolStripMenuItem;
                if (senderAsItem == null) return;

                MenuAuth auth = MenuAuth.None;

                if (LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    auth = MenuAuth.Write;
                }
                else if (LocalUser.Instance.LogInInformation.IsClient)
                {
                    try
                    {

                        string menuCode = senderAsItem.Name;
                        auth = menuCode.GetAuth();

                    }
                    catch { }
                }
                else
                {
                    auth = MenuAuth.Write;
                }
                if (senderAsItem.Name == "mnuHomePage" || senderAsItem.Name == "mnuRemote" || senderAsItem.Name == "mnuGonji")
                {

                }
                else
                {


                    if (auth == MenuAuth.None)
                    {

                        MessageBox.Show("해당 메뉴에 대한 권한이 없습니다.\n관리자에게 사용권한을 얻으신 후 계속하여 주십시오.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }


                Type frmType = null;
                if (senderAsItem.Name == "mnu0101")
                {
                    frmType = typeof(FrmMN0101_EMPTYCARINFOR);
                }
                else if (senderAsItem.Name == "mnu0102")
                {
                    frmType = typeof(FrmMN0102_EMPTYCARNOTICE);
                }
                else if (senderAsItem.Name == "mnu0103")
                {
                    frmType = typeof(FrmMN0103_EMPTYCARHISTORY);
                }
                else if (senderAsItem.Name == "mnu0203")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.IsHolderShop)
                        frmType = typeof(FrmMN0203_CAROWNERMANAGE_SG);
                    else
                        frmType = typeof(FrmMN0203_CAROWNERMANAGE);
                }
                else if (senderAsItem.Name == "mnu0204")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin || LocalUser.Instance.LogInInformation.IsClient)
                    {
                        frmType = typeof(FrmMN0204_CARGOOWNERMANAGE);
                    }
                    else
                    {
                        if (LocalUser.Instance.LogInInformation.CustomerUserId == 0)
                        {
                            frmType = typeof(FrmMN0209_CUSTOMER);
                        }
                        else
                        {
                            MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }

                }
                else if (senderAsItem.Name == "mnu0204_1")
                {
                    frmType = typeof(FrmMN0204_CARGOOWNERMANAGE);
                }
                else if (senderAsItem.Name == "mnu0206")
                {
                    frmType = typeof(FrmMN0206_SETMANAGE);
                }
                else if (senderAsItem.Name == "mnu0207")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmMN0207_SALESMANAGE);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else if (senderAsItem.Name == "mnu0702")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmMN0207_SALESMANAGE);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else if (senderAsItem.Name == "mnu0208")
                {
                    frmType = typeof(FrmMN0208_DRIVERADDMANAGE);
                }
                else if (senderAsItem.Name == "mnu0209")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.IsHolderShop)
                        frmType = typeof(FrmMN0209_CUSTOMER_SG);
                    else
                    {
                        
                            frmType = typeof(FrmMN0209_CUSTOMER);
                        

                    }
                }
                else if (senderAsItem.Name == "mnu0210")
                {
                    frmType = typeof(FrmMN0210_AccountOptions);
                }
                else if (senderAsItem.Name == "mnu0213")
                {
                    frmType = typeof(FrmMN0213_PRICEMANAGE);
                }
                else if (senderAsItem.Name == "mnu0214")
                {
                    frmType = typeof(FrmMN0214_SUBCARGOOWNERMANAGE);
                }
                else if (senderAsItem.Name == "mnu0301")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin)

                        LocalUser.Instance.LogInInformation.LoadClient();

                    if (LocalUser.Instance.LogInInformation.IsClient)
                    {
                        if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 0)
                        {
                            //frmType = typeof(FrmMN0301Default);

                            //아이디로그인일때
                            if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                            {
                                var mClientUsesAdapter = new ClientUsersTableAdapter();
                                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);

                                //담당거래처있을때
                                if (mTable.Any())
                                {
                                    frmType = typeof(FrmMN0301_OverView);
                                }
                                //담당거래처 전체일때
                                else
                                {
                                    frmType = typeof(FrmMN0301);
                                }

                            }
                            else
                            {
                                frmType = typeof(FrmMN0301);
                            }

                        }
                        else if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 1)
                        {
                            //아이디로그인일때
                            if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                            {
                                var mClientUsesAdapter = new ClientUsersTableAdapter();
                                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);
                                //담당거래처있을때
                                if (mTable.Any())
                                {
                                    frmType = typeof(FrmMN0301_OverView);
                                }
                                //담당거래처 전체일때
                                else
                                {
                                    frmType = typeof(FrmMN0301);
                                }

                            }
                            else
                            {
                                frmType = typeof(FrmMN0301);
                            }

                            //frmType = typeof(FrmMN0301);

                        }
                        else if(LocalUser.Instance.LogInInformation.Client.OrderGubun ==2)
                        {
                            frmType = typeof(FrmMN0301G); 
                        }
                        else
                        {
                            frmType = typeof(FrmMN0301_ETC);
                        }


                    }
                    else
                    {
                        if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 3)
                        {
                            if (LocalUser.Instance.LogInInformation.BizGubun == 7)
                            {
                                frmType = typeof(FrmMN0301_ETC_REFERRAL);
                            }
                            else
                            {
                                frmType = typeof(FrmMN0301_ETCCustomer);
                            }
                        }
                        else
                        {
                           
                                frmType = typeof(FrmMN0301Customer);
                           
                        }
                    }
                    //if (LocalUser.Instance.LogInInformation.Client != null && LocalUser.Instance.LogInInformation.Client.AllowOrder)
                    //    frmType = typeof(FrmMN0301);
                    //else
                    //    frmType = typeof(FrmMN0301_CARGOACCEPT);
                }




                else if (senderAsItem.Name == "mnu0304")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        if (LocalUser.Instance.LogInInformation.IsClient)
                        {
                            if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 3)
                            {
                                frmType = typeof(FrmMN0304_ETC);
                            }
                            else
                            {
                                var mClientUsesAdapter = new ClientUsersTableAdapter();
                                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);
                                //담당거래처있을때
                                if (mTable.Any())
                                {
                                    frmType = typeof(FrmMN0304_OverView);
                                }
                                //담당거래처 전체일때
                                else
                                {
                                    frmType = typeof(FrmMN0304);
                                }



                                
                            }
                        }
                        else
                        {

                            if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 3)
                            {
                                if (LocalUser.Instance.LogInInformation.BizGubun == 7)
                                {
                                    frmType = typeof(FrmMN0304_REFERRAL);
                                }
                                else
                                {
                                    frmType = typeof(FrmMN0304_ETCCustomer);
                                }
                            }
                            else
                            {
                                frmType = typeof(FrmMN0304_Customer);
                            }

                        }

                            

                    }

                }
                else if (senderAsItem.Name == "mnu0303")
                {
                    frmType = typeof(FrmMN0303_CARGOFPIS);


                }
                else if (senderAsItem.Name == "mnuClientUser")
                {
                    if (LocalUser.Instance.LogInInformation.ClientUserId == 0)
                    {
                        frmType = typeof(FrmClientUser);
                    }
                    else
                    {
                        MessageBox.Show("이용 권한이 없습니다.\r\n운송사 관리자만 이용 가능 합니다. !!!", "아이디관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }

                else if (senderAsItem.Name == "mnuClientVaccount")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)

                    {
                        frmType = typeof(FrmMNClientVaccount);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }

                else if (senderAsItem.Name == "mnuDriverVaccount")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)

                    {
                        frmType = typeof(FrmMNDriverVaccount);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }

                else if (senderAsItem.Name == "mnuNice")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)

                    {
                        frmType = typeof(FRMMNTAXUSELIST);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }


                else if (senderAsItem.Name == "mnuTrade")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.IsHolderShop)
                        frmType = typeof(FrmTrade_SG);
                    else
                        frmType = typeof(FrmTrade);
                }
                else if (senderAsItem.Name == "mnu0404")
                {
                    frmType = typeof(FrmMN0404_CHAGECARMANAGE);
                }
                else if (senderAsItem.Name == "mnu0401")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.IsHolderShop)
                        frmType = typeof(FrmTrade_CU_SG);
                    else
                        frmType = typeof(FrmMN0212_SALESMANAGE);
                }
                else if (senderAsItem.Name == "mnuFPIS")
                {
                    frmType = typeof(FrmMNFPIS);
                }
                else if (senderAsItem.Name == "mnu0104")
                {
                    frmType = typeof(FrmMN0104_DRIVERRECORDMANAGE);
                }
                else if (senderAsItem.Name == "mnu0701")
                {
                    frmType = typeof(FrmMN0701_DRIVERPAY);
                }
                else if (senderAsItem.Name == "mnuCustomerAcc")
                {
                    frmType = typeof(Frm_MN0702_CustomerAcc);
                }
                else if (senderAsItem.Name == "mnuCustomerAccRead")
                {
                    frmType = typeof(Frm_MN0702_CustomerAcc);
                }
                else if (senderAsItem.Name == "mnu0801")
                {
                    //frmType = typeof(FrmMN0801_Call);
                    frmType = typeof(FrmMN0301_SMS_List);
                }
                else if (senderAsItem.Name == "mnu0301_Call_List")
                {
                    frmType = typeof(FrmMN0301_Call_List);
                }
                else if (senderAsItem.Name == "mnuS01")
                {
                    frmType = typeof(FrmMNSTATS1);
                }
                else if (senderAsItem.Name == "mnuS02")
                {
                    frmType = typeof(FrmMNSTATS2);
                }
                else if (senderAsItem.Name == "mnuS03")
                {
                    frmType = typeof(FrmMNSTATS3);
                }
                else if (senderAsItem.Name == "mnuS04")
                {
                    frmType = typeof(FrmMNSTATS4);
                }
                else if (senderAsItem.Name == "mnuS05")
                {
                    frmType = typeof(FrmMNSTATS5);
                }
                else if (senderAsItem.Name == "mnuS06")
                {
                    frmType = typeof(FrmMNSTATS6);
                }
                else if (senderAsItem.Name == "mnuS07")
                {
                    frmType = typeof(FrmMNSTATS7);
                }
                else if (senderAsItem.Name == "mnuS08")
                {
                    frmType = typeof(FrmMNSTATS8);
                }
                else if (senderAsItem.Name == "mnuSTATS")
                {
                    //frmType = typeof(FrmMNAccount01);
                    frmType = typeof(FrmMNSTATS);
                }
                else if (senderAsItem.Name == "FrmMNAccount02")
                {
                    frmType = typeof(FrmMNAccount02);
                }
                else if (senderAsItem.Name == "FrmMNAccount03")
                {
                    frmType = typeof(FrmMNAccount03);
                }
                else if (senderAsItem.Name == "FrmMNAccount04")
                {
                    frmType = typeof(FrmMNAccount04);
                }
                else if (senderAsItem.Name == "FrmMNAccount05")
                {
                    frmType = typeof(FrmMNAccount05);
                }
                else if (senderAsItem.Name == "FrmMNAccount06")
                {
                    frmType = typeof(FrmMNAccount06);
                }
                else if (senderAsItem.Name == "mnu0803")
                {
                    frmType = typeof(FrmMN0803_Send_Image);
                }
                else if (senderAsItem.Name == "mnuNoticeDriver")
                {
                    frmType = typeof(FRMMNNOTICEDRIVER);
                }
                else if (senderAsItem.Name == "mnuCardAcc")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmMNCardAccTest);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }
                else if (senderAsItem.Name == "mnuAS")
                {
                    frmType = typeof(FrmMN0000_PROGRAMAS);
                }
                else if (senderAsItem.Name == "mnuLog")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FRMMNLog);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else if (senderAsItem.Name == "mnuUseList")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FRMMNUSELIST);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else if (senderAsItem.Name == "mnuAppSmsList")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmAPPSMSLIST);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }

                else if (senderAsItem.Name == "mnuEdocumnet")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmEDocumentList);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else if (senderAsItem.Name == "mnuexcel")
                {
                    if (LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        //frmType = typeof(FrmExcel);
                        frmType = typeof(FrmMN0802_Order_Excel);
                    }
                    else
                    {
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }



                }
                else if (senderAsItem.Name == "mnuPayExcel")
                {
                    frmType = typeof(FrmPayExcel);
                }
                else if (senderAsItem.Name == "mn0804")
                {
                    frmType = typeof(FrmMN0804VAccountPool);
                }
                else if (senderAsItem.Name == "mnuPayExcel")
                {
                    frmType = typeof(FrmPayExcel);
                }
                else if (senderAsItem.Name == "mnu0215")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmMN0215_CUSTOMERMANAGE);
                    }

                }
                else if (senderAsItem.Name == "mnu0216")
                {
                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmMN0216_ORDERITEM);
                    }

                }



                else if (senderAsItem.Name == "mnuHelp")
                {
                    Process.Start("http://222.231.9.253/차세로_주선사용_메뉴얼_V20.pdf");
                }
                else if (senderAsItem.Name == "mnuHomePage")
                {
                    Process.Start("http://www.chasero.co.kr");
                }
                else if (senderAsItem.Name == "mnuRemote")
                {
                    Process.Start("https://939.co.kr/lkw6270/");
                }
                else if (senderAsItem.Name == "mnuCustomerUsers")
                {
                    if (!LocalUser.Instance.LogInInformation.IsClient && !LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        frmType = typeof(FrmCustomerUser);
                    }
                    else
                    {

                       
                        MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }

                else if(senderAsItem.Name == "mnuGonji")
                {
                    int cnt = 0;
                    using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        Connection.Open();
                        SqlCommand Command = Connection.CreateCommand();
                        Command.CommandText =
                            @"SELECT COUNT(*) FROM Gongji WHERE UseYn = 'Y'";

                        var DataReader = Command.ExecuteReader();
                        if (DataReader.Read())
                        {

                            cnt = DataReader.GetInt32(0);

                        }
                        Connection.Close();
                    }
                   
                        if (cnt == 1)
                        {
                            FrmGongJi frmGongJi = new FrmGongJi
                            {
                                StartPosition = FormStartPosition.CenterParent
                            };
                            frmGongJi.Owner = this;
                            frmGongJi.StartPosition = FormStartPosition.CenterParent;
                            frmGongJi.ShowDialog();
                        }
                    
                }

                if (frmType == null) return;
                if (frmType == typeof(FrmMN0301))
                {
                    _Form = new FrmMN0301();
                    //_Form = new FrmMN0301(ShareOrderDataSet);
                }
                else if (frmType == typeof(FrmMN0301Default))
                {
                    _Form = new FrmMN0301Default();
                    //_Form = new FrmMN0301(ShareOrderDataSet);
                }
                else if (frmType == typeof(FrmMN0204_CARGOOWNERMANAGE))
                {
                    _Form = new FrmMN0204_CARGOOWNERMANAGE();
                }
                else if (frmType == typeof(FrmMN0301Customer))
                {
                    _Form = new FrmMN0301Customer();
                }
                else if (frmType == typeof(FrmMN0301_ETC))
                {
                    _Form = new FrmMN0301_ETC();
                }
                else if (frmType == typeof(FrmMN0301_ETCCustomer))
                {
                    _Form = new FrmMN0301_ETCCustomer();
                }
                else if (frmType == typeof(FrmMN0301_ETC_REFERRAL))
                {
                    _Form = new FrmMN0301_ETC_REFERRAL();
                }
                else if (frmType == typeof(FrmMN0301_OverView))
                {
                    _Form = new FrmMN0301_OverView();
                }
                else
                {
                    _Form = frmType.GetConstructor(Type.EmptyTypes).Invoke(null) as Form;
                }
                if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {
                    var mClientUsesAdapter = new ClientUsersTableAdapter();
                    var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);
                    if (mTable.Any(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId && !c.AllowWrite))
                    {
                        mHelper.LockWirte(_Form);
                    }
                }
                try
                {
                    //if (_Form.MaximizeBox)
                    //{

                    //    foreach (var item in MdiChildren)
                    //    {
                    //        item.Close();
                    //    }
                    //    _Form.WindowState = FormWindowState.Maximized;
                    //    _Form.MdiParent = this;
                    //    _Form.Show();
                    //}
                    //else _Form.ShowDialog(this);

                    //if (formIsExist(_Form.GetType()))
                    //{

                    //   // _Form.Dispose();     // 창 리소스 제거

                    //}
                    //else
                    //{
                    //    _Form.WindowState = FormWindowState.Maximized;
                    //    _Form.MdiParent = this;
                    //    _Form.Show();
                    //}

                    bool FrmisExist = new bool();
                    FrmisExist = false;
                    string tabName = "";
                    foreach (Form form1 in Application.OpenForms)
                    {
                        if (form1.GetType() == _Form.GetType())
                        {
                            FrmisExist = true;

                            tabName = _Form.Name;
                        }
                    }
                    // 폼존재여부에 따라서 생성과 파기
                    if (!FrmisExist)
                    {
                        _Form.WindowState = FormWindowState.Maximized;
                        _Form.MdiParent = this;
                        _Form.Show();
                        _Form.Activate();
                    }
                    else
                    {
                        //  _Form.Activate();

                        //  var Tabindex = tabControl1.TabPages.IndexOf(_Form);

                        //string tabName = this.tabControl1.TabPages[e.Index].Text;

                        foreach (TabPage tab in tabControl1.TabPages)
                        {

                            if (_Form.Text.Equals(tab.Text.Replace("    ", "")))
                            {
                                tabControl1.SelectedTab = tab;
                                //found = true;
                            }
                        }

                        _Form.Dispose();
                    }




                }
                catch (Exception) { }
            }
            catch (Exception ex) { }
        }


        // 자식 폼 중복 여부
        private bool formIsExist(Type tp)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form.GetType() == tp)
                {
                    form.Activate();
                    return true;
                }
            }
            return false;
        }

        private void _LoadForm(string FormName, string _Gubun, int _Id, string _CreateTime = "")
        {
            Form _Form = null;
            Type frmType = null;
            switch (FormName)
            {
                case "FrmMN0204_CARGOOWNERMANAGE":
                    frmType = typeof(FrmMN0204_CARGOOWNERMANAGE);
                    break;
                case "FrmMN0301":

                    if (LocalUser.Instance.LogInInformation.IsClient)
                    {
                        if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 0)
                        {
                            //frmType = typeof(FrmMN0301Default);

                            //아이디로그인일때
                            if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                            {
                                var mClientUsesAdapter = new ClientUsersTableAdapter();
                                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);

                                //담당거래처있을때
                                if (mTable.Any())
                                {
                                    frmType = typeof(FrmMN0301_OverView);
                                }
                                //담당거래처 전체일때
                                else
                                {
                                    frmType = typeof(FrmMN0301);
                                }

                            }
                            else
                            {
                                frmType = typeof(FrmMN0301);
                            }

                        }
                        else if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 1)
                        {
                            //아이디로그인일때
                            if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                            {
                                var mClientUsesAdapter = new ClientUsersTableAdapter();
                                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);
                                //담당거래처있을때
                                if (mTable.Any())
                                {
                                    frmType = typeof(FrmMN0301_OverView);
                                }
                                //담당거래처 전체일때
                                else
                                {
                                    frmType = typeof(FrmMN0301);
                                }

                            }
                            else
                            {
                                frmType = typeof(FrmMN0301);
                            }

                            //frmType = typeof(FrmMN0301);

                        }
                        else if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 2)
                        {
                            frmType = typeof(FrmMN0301G);
                        }
                        else
                        {
                            frmType = typeof(FrmMN0301_ETC);
                        }


                    }
                    else
                    {
                        if (LocalUser.Instance.LogInInformation.Client.OrderGubun == 3)
                        {
                            if (LocalUser.Instance.LogInInformation.BizGubun == 7)
                            {
                                frmType = typeof(FrmMN0301_ETC_REFERRAL);
                            }
                            else
                            {
                                frmType = typeof(FrmMN0301_ETCCustomer);
                            }
                        }
                        else
                        {

                            frmType = typeof(FrmMN0301Customer);

                        }
                    }


                    //if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                    //{
                    //    if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                    //    {
                    //        var mClientUsesAdapter = new ClientUsersTableAdapter();
                    //        var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);

                    //        if (mTable.Any())
                    //        {
                    //            frmType = typeof(FrmMN0301_OverView);
                    //        }

                    //        else
                    //        {

                    //            frmType = typeof(FrmMN0301);
                    //        }

                    //    }
                    //}

                    //else
                    //{
                    //    frmType = typeof(FrmMN0301);
                    //}
                    //frmType = typeof(FrmMN0301);
                    break;
                case "FrmMN0204":
                    frmType = typeof(FrmMN0204_CARGOOWNERMANAGE);
                    break;
                default:
                    break;
            }
            if (frmType == null) return;

            if (frmType == typeof(FrmMN0301))
            {
                if (string.IsNullOrEmpty(_CreateTime))
                {
                    _Form = new FrmMN0301(_Gubun, _Id);
                }
                else
                {
                    _Form = new FrmMN0301(_Gubun, _Id, _CreateTime);

                }
            }

           else if (frmType == typeof(FrmMN0301_ETC))
            {
                if (string.IsNullOrEmpty(_CreateTime))
                {
                    _Form = new FrmMN0301_ETC(_Gubun, _Id);
                }
                else
                {
                    _Form = new FrmMN0301_ETC(_Gubun, _Id, _CreateTime);

                }
            }


            else if (frmType == typeof(FrmMN0301_OverView))
            {
                if (string.IsNullOrEmpty(_CreateTime))
                {
                    _Form = new FrmMN0301_OverView(_Gubun, _Id);
                }
                else
                {
                    _Form = new FrmMN0301_OverView(_Gubun, _Id, _CreateTime);

                }
            }
            else if (frmType == typeof(FrmMN0204_CARGOOWNERMANAGE))
            {
                // _Form = frmType.GetConstructor(Type.EmptyTypes).Invoke(null) as Form;
                _Form = new FrmMN0204_CARGOOWNERMANAGE(_Gubun);
            }
            else
            {
                _Form = frmType.GetConstructor(Type.EmptyTypes).Invoke(null) as Form;
            }
            if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.ClientUserId > 0)
            {
                var mClientUsesAdapter = new ClientUsersTableAdapter();
                var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);
                if (mTable.Any(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId && !c.AllowWrite))
                {
                    mHelper.LockWirte(_Form);
                }
            }
            try
            {
                //if (_Form.MaximizeBox)
                //{
                //    foreach (var item in MdiChildren)
                //    {
                //        item.Close();
                //    }
                //    _Form.WindowState = FormWindowState.Maximized;
                //    _Form.MdiParent = this;
                //    _Form.Show();
                //}
                //else _Form.ShowDialog(this);


                if (_Form.Name == "FrmMN0301" || _Form.Name == "FrmMN0301_OverView" || _Form.Name == "FrmMN0301_ETC")
                {
                    bool FrmisExist2 = new bool();
                    FrmisExist2 = false;

                    foreach (Form form1 in Application.OpenForms)
                    {

                        if (form1.GetType() == _Form.GetType())
                        {
                            FrmisExist2 = true;

                        }
                    }

                    if (FrmisExist2 == true)
                    {

                        foreach (TabPage tab in tabControl1.TabPages)
                        {

                            if (_Form.Text.Equals(tab.Text.Replace("    ", "")))
                            {
                                tabControl1.SelectedTab = tab;

                            }
                        }

                     (this.tabControl1.TabPages[tabControl1.SelectedIndex].Tag as Form).Dispose(); //dispose the Form
                        this.tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex); //Remove the Tab

                        //after closing a tab, bring focus to the last tab
                        //The if statement prevents error when there is only a single tab left
                        if (tabControl1.TabCount == 0) return;

                        tabControl1.SelectedTab = this.tabControl1.TabPages[tabControl1.TabCount - 1];
                    }

                }

                bool FrmisExist = new bool();
                FrmisExist = false;
                string tabName = "";
                foreach (Form form1 in Application.OpenForms)
                {

                    if (form1.GetType() == _Form.GetType())
                    {
                        FrmisExist = true;

                        tabName = _Form.Name;


                    }
                }



                // 폼존재여부에 따라서 생성과 파기
                if (!FrmisExist)
                {
                    _Form.WindowState = FormWindowState.Maximized;
                    _Form.MdiParent = this;
                    _Form.Show();
                    _Form.Activate();
                }
                else
                {


                    foreach (TabPage tab in tabControl1.TabPages)
                    {

                        if (_Form.Text.Equals(tab.Text.Replace("    ", "")))
                        {
                            tabControl1.SelectedTab = tab;

                        }
                    }

                    _Form.Dispose();
                }
            }
            catch (Exception) { }
        }
        public static void LoadForm(string FormName, string _Gubun, int _Id = 0, string _CreateTime = "")
        {
            if (THIS != null)
                THIS._LoadForm(FormName, _Gubun, _Id, _CreateTime);
        }
        void pnShortCut_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (sender as Control != null && (sender as Control).Parent as Panel != null)
                {
                    Panel pnShortCut = (sender as Control).Parent as Panel;
                    pnShortCut.Invalidate();
                }
            }
            catch { }
        }
        void pnShortCut_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (sender as Control != null && (sender as Control).Parent as Panel != null)
                {
                    Panel pnShortCut = (sender as Control).Parent as Panel;
                    Pen white = new Pen(Color.Gainsboro);
                    Pen black = new Pen(Color.Black);
                    Rectangle rect = pnShortCut.ClientRectangle;
                    using (Graphics g = pnShortCut.CreateGraphics())
                    {
                        g.DrawLine(black, 1, rect.Height - 1, rect.Width - 1, rect.Height - 1);
                        g.DrawLine(black, rect.Width - 1, 1, rect.Width - 1, rect.Height - 1);
                        g.DrawLine(white, 1, 1, rect.Width - 1, 1);
                        g.DrawLine(white, 1, 1, 1, rect.Height - 1);
                    }
                }
            }
            catch { }
        }
        void pnShortCut_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender as Control != null && (sender as Control).Parent as Panel != null)
                {
                    Panel pnShortCut = (sender as Control).Parent as Panel;
                    Pen white = new Pen(Color.Gainsboro);
                    Pen black = new Pen(Color.Black);
                    Rectangle rect = pnShortCut.ClientRectangle;
                    using (Graphics g = pnShortCut.CreateGraphics())
                    {
                        g.DrawLine(black, 1, rect.Height - 1, rect.Width - 1, rect.Height - 1);
                        g.DrawLine(black, rect.Width - 1, 1, rect.Width - 1, rect.Height - 1);
                        g.DrawLine(white, 1, 1, rect.Width - 1, 1);
                        g.DrawLine(white, 1, 1, 1, rect.Height - 1);
                    }
                }
            }
            catch { }
        }
        void pnShortCut_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Left) return;
                if (sender as Control != null && (sender as Control).Parent as Panel != null)
                {
                    Panel pnShortCut = (sender as Control).Parent as Panel;
                    Pen white = new Pen(Color.Gainsboro);
                    Pen black = new Pen(Color.Black);
                    Rectangle rect = pnShortCut.ClientRectangle;
                    using (Graphics g = pnShortCut.CreateGraphics())
                    {
                        g.DrawLine(white, 1, rect.Height - 1, rect.Width - 1, rect.Height - 1);
                        g.DrawLine(white, rect.Width - 1, 1, rect.Width - 1, rect.Height - 1);
                        g.DrawLine(black, 1, 1, rect.Width - 1, 1);
                        g.DrawLine(black, 1, 1, 1, rect.Height - 1);
                    }
                    if (pnShortCut.Name == "pn0101")
                    {
                        Menu_Click(mnu0101, null);
                    }
                    else if (pnShortCut.Name == "pn0102")
                    {
                        Menu_Click(mnu0102, null);
                    }
                    else if (pnShortCut.Name == "pn0103")
                    {
                        Menu_Click(mnu0103, null);
                    }

                    else if (pnShortCut.Name == "pn0203")
                    {
                        Menu_Click(mnu0203, null);
                    }
                    else if (pnShortCut.Name == "pn0204")
                    {
                        Menu_Click(mnu0204, null);
                    }
                    else if (pnShortCut.Name == "pn0205")
                    {
                        if (LocalUser.Instance.LogInInformation.IsAdmin)
                        {
                            Menu_Click(mnu0205, null);
                        }
                        else
                        {
                            MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else if (pnShortCut.Name == "pn0206")
                    {
                        Menu_Click(mnu0206, null);
                    }

                    else if (pnShortCut.Name == "pn0207")
                    {
                        if (LocalUser.Instance.LogInInformation.IsAdmin)
                        {
                            Menu_Click(mnu0207, null);
                        }
                        else
                        {
                            MessageBox.Show("사용권한이 없습니다.", "권한 필요", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else if (pnShortCut.Name == "pn0208")
                    {

                        Menu_Click(mnu0208, null);

                    }
                    else if (pnShortCut.Name == "pn0301")
                    {

                        Menu_Click(mnu0301, null);


                    }
                    else if (pnShortCut.Name == "pn0304")
                    {

                        Menu_Click(mnu0304, null);


                    }
                    else if (pnShortCut.Name == "pn0303")
                    {
                        Menu_Click(mnuFPIS, null);
                    }
                    else if (pnShortCut.Name == "pnClientUser")
                    {
                        //Menu_Click(아이디관리ToolStripMenuItem, null);

                        if (LocalUser.Instance.LogInInformation.ClientUserId == 0)
                        {
                            Menu_Click(mnuClientUser, null);
                        }
                        else
                        {
                            MessageBox.Show("이용 권한이 없습니다.\r\n운송사 관리자만  이용 가능 합니다. !!!", "아이디관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }




                    }

                    else if (pnShortCut.Name == "pnCustomer")
                    {
                        //if (LocalUser.Instance.LogInInformation.CustomerUserId == 0 && LocalUser.Instance.LogInInformation.IsClient && LocalUser.Instance.LogInInformation.IsAdmin)
                        //{
                            Menu_Click(mnu0209, null);
                        //}
                        //else
                        //{
                        //    MessageBox.Show("이용 권한이 없습니다.\r\n 거래처 관리자만  이용 가능 합니다. !!!", "거래처관리", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //}
                      
                    }
                    else if (pnShortCut.Name == "pnTrade")
                    {
                        Menu_Click(mnuTrade, null);

                    }


                    else if (pnShortCut.Name == "pn0403")
                    {
                        Menu_Click(mnu0403, null);
                    }


                    else if (pnShortCut.Name == "pn0404")
                    {
                        Menu_Click(mnu0404, null);
                    }

                    else if (pnShortCut.Name == "pn0701")
                    {
                        Menu_Click(mnu0701, null);
                    }
                    else if (pnShortCut.Name == "pn0702")
                    {
                        Menu_Click(mnuCustomerAcc, null);
                    }
                    else if (pnShortCut.Name == "pn0801")
                    {
                        Menu_Click(mnu0801, null);
                    }
                    else if (pnShortCut.Name == "pn0401")
                    {
                        Menu_Click(mnu0401, null);
                    }
                    else if (pnShortCut.Name == "pnStats")
                    {
                        Menu_Click(mnuSTATS, null);
                    }
                    else if (pnShortCut.Name == "pn0401New")
                    {
                        Menu_Click(mnu0401, null);
                    }
                    else if (pnShortCut.Name == "pn0302")
                    {
                        Menu_Click(mnuFPIS, null);
                    }

                    else if (pnShortCut.Name == "pnHelp")
                    {
                        Process.Start("http://222.231.9.253/차세로_주선사용_메뉴얼_V20.pdf");
                        // var iFormName = this.MdiChildren[this.MdiChildren.Length - 1].Name;
                        //if (mFrmHelp == null)
                        //{
                        //    mFrmHelp = new FrmHelp();
                        //    mFrmHelp.FormClosing += (a, b) => { mFrmHelp = null; };
                        //    mFrmHelp.StartPosition = FormStartPosition.Manual;
                        //    mFrmHelp.Top = 0;
                        //    mFrmHelp.Left = 0;
                        //    mFrmHelp.Show();
                        //}
                        //mFrmHelp.ActivateAndNavitation(iFormName);
                        //switch (iFormName)
                        //{
                        //    //case "FrmClientUser":
                        //    //    Process.Start("https://www.youtube.com/embed/2O4A79T51dQ?vq=hd720");
                        //    //    break;
                        //    //case "FrmMNSTATS8":
                        //    //    Process.Start("https://www.youtube.com/embed/cWMvUiN3ENE?vq=hd720");
                        //    //    break;
                        //    //case "FrmMN0208_DRIVERADDMANAGE":
                        //    //    Process.Start("https://www.youtube.com/embed/DGGtZg13e0s?vq=hd720");
                        //    //    break;
                        //    //case "FrmMN0303_CARGOFPIS":
                        //    //    Process.Start("https://www.youtube.com/embed/V0u-XNnMAYU?vq=hd720");
                        //    //    break;
                        //    //case "FRMMNNOTICEDRIVER":
                        //    //    Process.Start("https://www.youtube.com/embed/cGHlLQaLJSA?vq=hd720");
                        //    //    break;
                        //    //case "FrmMN0209_CUSTOMER":
                        //    //    Process.Start("https://www.youtube.com/embed/FEicvEEwwTM?vq=hd720");
                        //    //    break;
                        //    //case "FrmMN0203_CAROWNERMANAGE":
                        //    //    Process.Start("https://www.youtube.com/embed/ITL7hycAA0c?vq=hd720");
                        //    //    break;
                        //    //case "FrmMN0301_CARGOACCEPT":
                        //    //    Process.Start("https://www.youtube.com/embed/8CZhZxa-4ss?vq=hd720");
                        //    //    break;
                        //    //case "FrmTrade":
                        //    //    Process.Start("https://www.youtube.com/embed/Ld11DNbrVSY?vq=hd720");
                        //    //    break;
                        //    //case "FrmMN0212_SALESMANAGE":
                        //    //    Process.Start("https://www.youtube.com/embed/QwuOtjA5clg?vq=hd720");
                        //    //    break;

                        //    case "FrmMN0209_CUSTOMER":
                        //        Process.Start("https://blog.naver.com/edubill365/221372085635");

                        //        break;
                        //    case "FrmMN0203_CAROWNERMANAGE":
                        //        Process.Start("https://blog.naver.com/edubill365/221372110162");

                        //        break;
                        //    case "FrmMN0301":
                        //        Process.Start("https://blog.naver.com/edubill365/221372110420");

                        //        break;
                        //    case "FrmTrade":
                        //        Process.Start("https://blog.naver.com/edubill365/221372110738");

                        //        break;
                        //    case "FrmMN0212_SALESMANAGE":
                        //        Process.Start("https://blog.naver.com/edubill365/221372110925");

                        //        break;
                        //    case "FrmMNSTATS":
                        //        Process.Start("https://blog.naver.com/edubill365/221372111212");

                        //        break;
                        //    default:
                        //        break;
                        //}



                        //if (mFrmHelp_Youtube == null)
                        //{
                        //    mFrmHelp_Youtube = new FrmHelp_Youtube();
                        //    mFrmHelp_Youtube.FormClosing += (a, b) => { mFrmHelp_Youtube = null; };
                        //    mFrmHelp_Youtube.StartPosition = FormStartPosition.Manual;
                        //    mFrmHelp_Youtube.Top = 40;
                        //    mFrmHelp_Youtube.Left = 40;
                        //    mFrmHelp_Youtube.Show();
                        //}
                        //mFrmHelp_Youtube.ActivateAndNavitation(iFormName);
                    }
                    else if (pnShortCut.Name == "pnVideoHelp")
                    {
                        FrmManual frmManual = new FrmManual();

                        frmManual.StartPosition = FormStartPosition.CenterParent;
                        frmManual.ShowDialog();
                        //if (mFrmHelp_Youtube == null)
                        //{
                        //    mFrmHelp_Youtube = new FrmHelp_Youtube();
                        //    mFrmHelp_Youtube.FormClosing += (a, b) => { mFrmHelp_Youtube = null; };
                        //    mFrmHelp_Youtube.StartPosition = FormStartPosition.Manual;
                        //    mFrmHelp_Youtube.Top = 40;
                        //    mFrmHelp_Youtube.Left = 40;
                        //    mFrmHelp_Youtube.Show();
                        //}
                    }
                    else if (pnShortCut.Name == "pnHomePage")
                    {
                        System.Diagnostics.Process IEProcess = new System.Diagnostics.Process();
                        IEProcess.StartInfo.FileName = "iexplore.exe";
                        IEProcess.StartInfo.Arguments = "http://chasero.co.kr/";
                        IEProcess.Start();

                        //if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.ClientId == 411)
                        //{
                        //    System.Diagnostics.Process IEProcess = new System.Diagnostics.Process();
                        //    IEProcess.StartInfo.FileName = "iexplore.exe";
                        //    IEProcess.StartInfo.Arguments = "HTTP://www.narmi114.co.kr/";
                        //    IEProcess.Start();
                        //}
                        //else
                        //{
                        //    System.Diagnostics.Process IEProcess = new System.Diagnostics.Process();
                        //    IEProcess.StartInfo.FileName = "iexplore.exe";
                        //    IEProcess.StartInfo.Arguments = "https://cardpay114.modoo.at/";
                        //    IEProcess.Start();
                        //}
                    }
                    else if (pnShortCut.Name == "pnRemote")
                    {
                        mnu0902_Click(null, null);
                    }

                    else if (pnShortCut.Name == "pn0104")
                    {
                        Menu_Click(mnuCustomerAccRead, null);
                    }


                    else if (pnShortCut.Name == "pnTradeNew")
                    {
                        Menu_Click(mnuTrade, null);

                    }
                    else if (pnShortCut.Name == "pn0401New")
                    {
                        Menu_Click(mnu0401, null);
                    }
                    else if (pnShortCut.Name == "pnSubMall")
                    {
                        Menu_Click(mnuSubmall, null);
                    }

                    else if (pnShortCut.Name == "pn0206")
                    {
                        Menu_Click(mnu0206, null);
                    }

                    else if (pnShortCut.Name == "pnAs")
                    {

                        Menu_Click(mnuAS, null);
                    }
                    else if (pnShortCut.Name == "pn0301_Call_List")
                    {
                        Menu_Click(mnu0301_Call_List, null);
                    }

                }
            }
            catch { }
        }

        void pnShortCut_MouseHover(object sender, EventArgs e)
        {
            try
            {
                //if (e.Button != MouseButtons.Left) return;
                if (sender as Control != null && (sender as Control).Parent as Panel != null)
                {
                    Panel pnShortCut = (sender as Control).Parent as Panel;
                    //Pen white = new Pen(Color.White);
                    //Pen black = new Pen(Color.Black);
                    //Rectangle rect = pnShortCut.ClientRectangle;
                    //using (Graphics g = pnShortCut.CreateGraphics())
                    //{
                    //    g.DrawLine(white, 1, rect.Height - 1, rect.Width - 1, rect.Height - 1);
                    //    g.DrawLine(white, rect.Width - 1, 1, rect.Width - 1, rect.Height - 1);
                    //    g.DrawLine(black, 1, 1, rect.Width - 1, 1);
                    //    g.DrawLine(black, 1, 1, 1, rect.Height - 1);
                    //}
                    if (pnShortCut.Name == "pnCustomer")
                    {
                        this.toolTip1.ToolTipTitle = "거래처";
                        // this.toolTip1.IsBalloon = true;
                        this.toolTip1.SetToolTip(this.pictureBox11, "화주(거래처)를 등록, 배차/전표관리/수금관리를 편리하게 함.");
                    }
                    else if (pnShortCut.Name == "pn0203")
                    {
                        this.toolTip1.ToolTipTitle = "차량관리";

                        this.toolTip1.SetToolTip(this.pictureBox3, "차주(차량정보)를 등록, 배차/전표관리/결제관리를 편리하게 함.");
                    }
                    else if (pnShortCut.Name == "pn0301")
                    {
                        this.toolTip1.ToolTipTitle = "배차관리";

                        this.toolTip1.SetToolTip(this.pictureBox5, "화주 화물의뢰 정보를 등록, 문자전송 및 배차관련 통합관리 제공.");
                    }
                    else if (pnShortCut.Name == "pnTrade")
                    {
                        this.toolTip1.ToolTipTitle = "매입관리";

                        this.toolTip1.SetToolTip(this.pictureBox4, "배차 후, 차주가 전송한 전자세금계산서/전자인수증 조회 및 결제관리 제공.");
                    }

                    else if (pnShortCut.Name == "pn0401")
                    {
                        this.toolTip1.ToolTipTitle = "매출관리";

                        this.toolTip1.SetToolTip(this.pictureBox14, "배차 후, 화주별 거래명세서를 집계작성 및 전자세금계산서 발행 및 수금관리 제공.");
                    }

                    else if (pnShortCut.Name == "pnStats")
                    {
                        this.toolTip1.ToolTipTitle = "정산관리";

                        this.toolTip1.SetToolTip(this.pictureBox9, "배차일보 및 차주/화주별 미수금/미지급금을 체계적으로 관리.");
                    }
                    else if (pnShortCut.Name == "pn0801")
                    {
                        this.toolTip1.ToolTipTitle = "녹음내역";

                        this.toolTip1.SetToolTip(this.pictureBox7, "LG U+ 인터넷전화 사용 시, 녹음기능 제공. (옵션)");
                    }

                    else if (pnShortCut.Name == "pn0301_Call_List")
                    {
                        this.toolTip1.ToolTipTitle = "통화내역";

                        this.toolTip1.SetToolTip(this.pictureBox8, "LG U+ 인터넷전화 사용 시, 통화내역 기능 제공. (옵션)");
                    }
                    else if (pnShortCut.Name == "pnHomePage")
                    {
                        this.toolTip1.ToolTipTitle = "홈페이지";

                        this.toolTip1.SetToolTip(this.pictureBox15, "차세로 홈페이지 연결.");
                    }
                    else if (pnShortCut.Name == "pnRemote")
                    {
                        this.toolTip1.ToolTipTitle = "원격지원";

                        this.toolTip1.SetToolTip(this.pictureBox22, "당 사에서 해당 운송/주선사 PC에 원격 접속하여 도우미 기능 제공.");
                    }
                    //else if (pnShortCut.Name == "pnAs")
                    //{
                    //    this.toolTip1.ToolTipTitle = "A/S요청";

                    //    this.toolTip1.SetToolTip(this.pictureBox27, "본 프로그램 사용 시, 수정이나 추가 요청사항이 있는 경우 작성.");
                    //}



                }
            }
            catch { }
        }

        private void mnu0902_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process IEProcess = new System.Diagnostics.Process();
                IEProcess.StartInfo.FileName = "iexplore.exe";
                IEProcess.StartInfo.Arguments = "https://939.co.kr/lkw6270/";
                IEProcess.Start();
            }
            catch { }
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            try
            {
               

                pnTool.Visible = false;
                mnu02.Visible = false;


                //panel2.Visible = true;
                //pn0303.Visible = true;
                pn0303.Visible = false;
                pn0301.Visible = true;
                mnu0208.Visible = true;
                mnu0214.Visible = true;
                mnuNoticeDriver.Visible = false;
                배차관리ToolStripMenuItem.Visible = true;
                mnu0403.Visible = true;
                기타ToolStripMenuItem.Visible = true;
                HoveredTabIndex = 0;

                foreach (System.Windows.Forms.Form TheForm in this.MdiChildren)
                {

                    TheForm.Dispose();
                }

                var TabCount = tabControl1.TabPages.Count;

                for (int i = 0; i < TabCount; i++)
                {
                    tabControl1.TabPages.RemoveAt(0);
                }
              

                FrmLogin fLog = new FrmLogin
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                if (fLog.ShowDialog() != DialogResult.Yes)
                {
                    isClosed = true;
                    Close();
                    return;
                }


                int cnt = 0;

                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    Connection.Open();
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText =
                        @"SELECT COUNT(*) FROM Gongji WHERE UseYn = 'Y'";

                    var DataReader = Command.ExecuteReader();
                    if (DataReader.Read())
                    {

                        cnt = DataReader.GetInt32(0);

                    }
                    Connection.Close();
                }
                if (LocalUser.Instance.PersonalOption.GonjJiNO == true && LocalUser.Instance.PersonalOption.GonjJiDate <= DateTime.Now.Date)
                {

                    

                }
                else
                {
                    if (cnt == 1)
                    {
                        FrmGongJi frmGongJi = new FrmGongJi
                        {
                            StartPosition = FormStartPosition.CenterParent
                        };
                        frmGongJi.Owner = this;
                        frmGongJi.StartPosition = FormStartPosition.CenterParent;
                        frmGongJi.ShowDialog();
                    }
                }


                Opacity = 1;
                ShowInTaskbar = true;


                IsLogined = true;
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    LocalUser.Instance.LogInInformation.LoadClient();
                }

                foreach (ToolStripMenuItem menu in mnuRoot.Items)
                {
                    menu.Visible = true;
                    통계관리ToolStripMenuItem.Visible = false;
                }
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (LocalUser.Instance.LogInInformation.IsClient)
                    {
                        #region 운송수선사
                        LocalUser.Instance.LogInInformation.LoadClient();
                        lbl_Gubun.Text = "운송/주선사";
                        if (!LocalUser.Instance.LogInInformation.AllowFPIS || LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            pictureBox10.Image = Properties.Resources.gray_030;
                            // pictureBox5.Image = Properties.Resources._08_New_disabled;
                            pictureBox10.Enabled = false;
                            label10.Enabled = false;
                            mnu0303.Visible = false;
                           // mnuFPIS.Enabled = false;
                        }
                        else
                        {
                            pictureBox10.Image = Properties.Resources.black_030;
                           
                            pictureBox10.Enabled = true;
                            label10.Enabled = true;
                            mnu0303.Visible = false;
                           // mnuFPIS.Enabled = true;
                            //pictureBox10.Image = Properties.Resources._08_New;
                            //pictureBox10.Enabled = true;
                            //label10.Enabled = true;
                            //mnu0303.Enabled = true;

                        }
                        if (!LocalUser.Instance.LogInInformation.AllowSub)
                        {
                            mnu0214.Visible = false;
                        }
                        else
                        {
                            mnu0214.Visible = true;
                        }

                        if (!LocalUser.Instance.LogInInformation.HideAddTrade)
                        {

                            if (LocalUser.Instance.LogInInformation.SmsYn)
                            {
                                

                                pictureBox7.Enabled = true;
                                label5.Enabled = true;
                                mnu0801.Enabled = true;
                                pn0801.Enabled = true;

                            }
                            else
                            {
                               
                                pictureBox7.Visible = false;
                                label5.Visible = false;


                                mnu0801.Enabled = false;
                                pn0801.Enabled = false;

                            }
                           

                            pictureBox8.Visible = false;
                            label3.Visible = false;


                            mnu0301_Call_List.Enabled = false;
                            pn0301_Call_List.Enabled = false;


                        }
                        else
                        {
                          
                            pictureBox7.Visible = true;
                            label5.Visible = true;

                            mnu0801.Enabled = true;
                            pn0801.Enabled = true;


                           
                            pictureBox8.Visible = true;
                            label3.Visible = true;

                            mnu0301_Call_List.Enabled = true;
                            pn0301_Call_List.Enabled = true;
                        }

                        #endregion

                      
                       

                        lbl_Name.Text = LocalUser.Instance.LogInInformation.ClientName;
                    }
                    else
                    {

                        #region 화주
                        pnCustomer.Visible = false;
                        panel2.Visible = false;
                        pn0203.Visible = false;
                        panel3.Visible = false;
                        pnTrade.Visible = false;
                        panel6.Visible = false;
                        pnStats.Visible = false;
                        panel8.Visible = false;
                        pn0801.Visible = false;
                        panel9.Visible = false;
                        pn0301_Call_List.Visible = false;
                        panel10.Visible = false;
                        mnu0303.Visible = false;
                        pn0206.Visible = false;
                        panel11.Visible = false;
                        pn0207.Visible = false;
                        panel12.Visible = false;
                        pn0204.Visible = false;
                        panel13.Visible = false;

                        pn0401.Visible = false;
                      //  mnu01.Visible = false;

                        mnu0209.Visible = true;
                        mnu0203.Visible = false;
                        mnu0208.Visible = false;
                        mnu0214.Visible = false;
                        mnuClientUser.Visible = false;
                        mnu0216.Visible = false;
                        mnu0215.Visible = false;
                        mnuNoticeDriver.Visible = false;

                        mnu0206.Visible = false;
                        mnuexcel.Visible = false;
                        mnu02.Visible = false;
                        통계관리ToolStripMenuItem.Visible = false;
                        정산통계ToolStripMenuItem.Visible = false;
                        기타ToolStripMenuItem.Visible = false;

                        //기본정보ToolStripMenuItem.Enabled = false;


                        배차관리ToolStripMenuItem.Visible = true;
                        재무관리ToolStripMenuItem.Visible = false;

                        mnu0204_1.Visible = false;

                        lbl_Name.Text = LocalUser.Instance.LogInInformation.ClientName;
                        lbl_Gubun.Text = "화주";
                        #endregion
                    }

                }
                else
                {
                    lbl_Gubun.Text = "관리자";
                    lbl_Name.Text = "";
                    통계관리ToolStripMenuItem.Visible = false;
                }

              

                if (LocalUser.Instance.LogInInformation.IsClient)
                {
                    var _ClientPoint = ShareOrderDataSet.ClientPoints.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();
                    if (_ClientPoint.Any())
                    {
                        lbl_LoginId.Text = "현재충전금 : " + _ClientPoint.Sum(c => c.Amount).ToString("N0") + "원";

                    }
                    else
                    {
                        lbl_LoginId.Text = "현재충전금 : 0원 ";
                    }

                    lbl_LoginId.Text += "   " + LocalUser.Instance.LogInInformation.LoginId;
                }
                else
                {
                    lbl_LoginId.Text = LocalUser.Instance.LogInInformation.LoginId;
                }
                
                //pictureBox14.Image = Properties.Resources.black_070;
                pn0401.Enabled = true;
                //운송사
                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    mnuPayExcel.Visible = false;
                    mnu02.Visible = false;

                    if (LocalUser.Instance.LogInInformation.IsClient)
                    {
                        //pn0303.Visible = true;
                        pn0303.Visible = false;
                        pn0301.Visible = true;
                        pn0401.Visible = true;
                        // pnG1.Visible = true;
                        pn0301.Visible = true;
                        //pnG2.Visible = true;
                        //pnG3.Visible = true;

                        pn0401.Visible = true;

                        pn0206.Visible = false;
                        pn0207.Visible = false;
                        pn0204.Visible = false;


                        //pnG4.Visible = false;
                        //panel4.Visible = false;
                        //pnG5.Visible = true;
                        //pnG6.Visible = true;
                       // mnuFPIS.Visible = true;
                        mnu0401.Visible = true;
                        mnuTrade.Visible = true;
                        mnuClientUser.Visible = true;
                      //  mnu0210.Visible = true;
                        mnu0204_1.Visible = true;
                        mnuSubmall.Visible = false;
                        mnu0403.Visible = false;
                        mnu0208.Visible = true;
                        mnu0203.Visible = true;
                        pn0206.Visible = false;
                       // mnuCustomerAccRead.Visible = true;
                       // mnu0104.Visible = true;
                        mnu0803.Visible = false;
                        mn0804.Visible = false;
                        //if (LocalUser.Instance.LogInInformation.IsSubClient)
                        //{
                        //    foreach (ToolStripMenuItem menu in mnuRoot.Items)
                        //    {
                        //        if (menu == mnu01)
                        //            continue;
                        //        menu.Visible = false;
                        //    }
                        //}
                        if (LocalUser.Instance.LogInInformation.IsSubClient)
                        {
                            //pictureBox14.Image = Properties.Resources.gray_070;
                            pn0401.Enabled = false;
                        }


                        if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                        {
                            var mClientUsesAdapter = new ClientUsersTableAdapter();
                            var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId).Where(c => c.CustomerId > 0 && c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId);

                            //담당거래처있을때
                            if (mTable.Any())
                            {
                                #region 화주
                                
                                pnCustomer.Visible = false;
                                panel2.Visible = false;
                                pn0203.Visible = true;
                                panel3.Visible = true;
                                pnTrade.Visible = false;
                                panel6.Visible = false;
                                pnStats.Visible = false;
                                panel8.Visible = false;
                                pn0801.Visible = false;
                                panel9.Visible = false;
                                pn0301_Call_List.Visible = false;
                                panel10.Visible = false;
                                mnu0303.Visible = false;
                                pn0206.Visible = false;
                                panel11.Visible = false;
                                pn0207.Visible = false;
                                panel12.Visible = false;
                                pn0204.Visible = false;
                                panel13.Visible = false;
                                pn0401.Visible = false;
                                // mnu01.Visible = false;


                                mnu0203.Visible = false;
                                mnu0208.Visible = false;
                                mnu0214.Visible = false;
                                mnuClientUser.Visible = false;
                                mnu0216.Visible = false;
                                mnu0215.Visible = false;
                                mnuNoticeDriver.Visible = false;

                                mnu0206.Visible = false;
                                mnuexcel.Visible = false;
                                mnu02.Visible = false;
                                통계관리ToolStripMenuItem.Visible = false;
                                정산통계ToolStripMenuItem.Visible = false;
                                기타ToolStripMenuItem.Visible = false;

                                //기본정보ToolStripMenuItem.Enabled = false;


                                배차관리ToolStripMenuItem.Visible = true;
                                재무관리ToolStripMenuItem.Visible = false;
                                기본정보ToolStripMenuItem.Visible = false;
                                mnu0204_1.Visible = false;


                                #endregion
                            }
                        }
                    }
                    else
                    {
                        #region 화주
                        pnCustomer.Visible = false;
                        panel2.Visible = false;
                        pn0203.Visible = false;
                        panel3.Visible = false;
                        pnTrade.Visible = false;
                        panel6.Visible = false;
                        pnStats.Visible = false;
                        panel8.Visible = false;
                        pn0801.Visible = false;
                        panel9.Visible = false;
                        pn0301_Call_List.Visible = false;
                        panel10.Visible = false;
                        mnu0303.Visible = false;
                        pn0206.Visible = false;
                        panel11.Visible = false;
                        pn0207.Visible = false;
                        panel12.Visible = false;
                        pn0204.Visible = false;
                        panel13.Visible = false;
                        pn0401.Visible = false;
                       // mnu01.Visible = false;


                        mnu0203.Visible = false;
                        mnu0208.Visible = false;
                        mnu0214.Visible = false;
                        mnuClientUser.Visible = false;
                        mnu0216.Visible = false;
                        mnu0215.Visible = false;
                        mnuNoticeDriver.Visible = false;

                        mnu0206.Visible = false;
                        mnuexcel.Visible = false;
                        mnu02.Visible = false;
                        통계관리ToolStripMenuItem.Visible = false;
                        정산통계ToolStripMenuItem.Visible = false;
                        기타ToolStripMenuItem.Visible = false;

                        //기본정보ToolStripMenuItem.Enabled = false;


                        배차관리ToolStripMenuItem.Visible = true;
                        재무관리ToolStripMenuItem.Visible = false;

                        mnu0204_1.Visible = false;


                        #endregion
                        //pnCustomer.Enabled = false;
                        //pn0203.Enabled = false;
                        //pnTrade.Enabled = false;
                        //pnStats.Enabled = false;
                        //pn0801.Enabled = false;
                        //pn0301_Call_List.Enabled = false;

                        //pn0206.Enabled = false;
                        //pn0207.Enabled = false;
                        //pn0204.Enabled = false;

                        //mnu0203.Enabled = false;
                        //mnu0208.Enabled = false;
                        //mnu0214.Enabled = false;
                        //mnuClientUser.Enabled = false;
                        //mnu0216.Enabled = false;
                        //mnu0215.Enabled = false;
                        //mnuNoticeDriver.Enabled = false;

                        //mnu0206.Enabled = false;
                        //mnuexcel.Enabled = false;
                        //mnu02.Enabled = false;
                        //통계관리ToolStripMenuItem.Enabled = false;
                        //정산통계ToolStripMenuItem.Enabled = false;
                        //기타ToolStripMenuItem.Enabled = false;

                        //mnu01.Enabled = false;
                        ////기본정보ToolStripMenuItem.Enabled = false;
                        //배차관리ToolStripMenuItem.Enabled = false;
                        //재무관리ToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                  
                    pn0301.Visible = false;
                    mnuPayExcel.Visible = true;

                    pn0401.Visible = false;
                    //pnG1.Visible = true;
                    pn0303.Visible = false;
                    pn0301.Visible = false;
                    pn0206.Visible = true;
                    pn0207.Visible = true;
                    pn0204.Visible = true;

                    pn0401.Visible = false;
                    //pnG4.Visible = true;
                    //panel4.Visible = false;
                    //pnG5.Visible = true;
                    //pnG6.Visible = true;
                    //관리자
                    mnu02.Visible = true;
                    //위탁관리
                    mnu0303.Visible = false;
                    //실적신고
                   // mnuFPIS.Visible = false;
                    //경비출납
                    mnu0403.Visible = false;
                    //아이디관리
                    mnuClientUser.Visible = false;
                    //서브몰
                    mnuSubmall.Visible = false;
                    //계정과목
                    //mnu0210.Visible = false;
                    //내정보
                    mnu0204_1.Visible = false;
                    //매출관리
                    mnu0401.Visible = true;
                    //매입관리
                    mnuTrade.Visible = true;
                    //기사세무    
                    mnu0404.Visible = true;
                    //기사관리
                    mnu0208.Visible = true;
                    //차량관리
                    mnu0203.Visible = true;
                    //화주보험관리
                    //mnuCustomerAccRead.Visible = true;
                    //탁송업체운송료등록
                    mnu0213.Visible = true;
                    mnu0213.Enabled = true;
                    mnu0205.Visible = false;
                    //지점관리
                    mnu0214.Visible = false;
                   // mnu0104.Visible = false;
                    //사진전송
                    mnu0803.Visible = false;
                    mn0804.Visible = true;
                }
                InitTimer();
                if (LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    Menu_Click(mnuTrade, null);
                }
                else
                {
                    if (LocalUser.Instance.LogInInformation.IsClient)
                    {
                        if (LocalUser.Instance.LogInInformation.Client.IsHolderShop)
                            Menu_Click(mnu0301, null);
                        else
                            //Menu_Click(mnu0301, null);
                            Menu_Click(mnu0301, null);
                    }
                    else
                    {
                        Menu_Click(mnu0301, null);
                    }
                }

                tabControl1.Visible = true;
                if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.Client.IsHolderShop)
                {
                    lblCustomer.Text = "매출처";
                    lblDriver.Text = "매입처";
                    lblTradeSGCU.Text = "청구관리";

                    mnu0203.Text = "매입처관리";
                    mnu0209.Text = "매출처관리";
                    mnu0401.Text = "청구관리";

                   // panel2.Visible = false;
                    pn0303.Visible = false;
                    pn0301.Visible = false;
                    mnu0208.Visible = false;
                    mnu0214.Visible = false;
                    mnuNoticeDriver.Visible = false;
                    배차관리ToolStripMenuItem.Visible = false;
                    mnu0403.Visible = false;
                    this.mnu0101.Visible = false;
                    this.mnu0102.Visible = false;
                    this.mnu0103.Visible = false;
                   // this.mnuFPIS.Visible = false;
                  //  this.mnu0104.Visible = false;
                   // this.mnuCustomerAccRead.Visible = false;
                   // this.mnu0210.Visible = false;
                    this.mnu0701.Visible = false;
                    this.mnuSubmall.Visible = false;
                    this.mnu0213.Visible = false;
                    this.mnu0404.Visible = false;
                    this.mnu0801.Visible = false;
                    this.mnu0803.Visible = false;
                }
                if (!FormClosingBind)
                {
                    FormClosing += new FormClosingEventHandler(FrmMDI_FormClosing);
                    FormClosingBind = true;
                }
                pnTool.Visible = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void FrmMDI_Load(object sender, EventArgs e)
        {
            CallHelper.Instance = new CallHelper(this);
            CallHelper.Instance.Initialize(this.axLGUBaseOpenApi, lblRecord);
            CallHelper.Instance.Start();
            CallHelper.Instance.OnLogouted += () =>
            {
                MessageBox.Show("입력하신 정보로 로그인 할 수 없습니다. 아이디와 비밀번호를 확인해주세요.", "차세로", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            };
            통계관리ToolStripMenuItem.Visible = false;

            // lblVerSion.Text = DateTime.Now.ToString("yyyy) + " 1.0";
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {

            try
            {
                Close();
            }
            catch { }
        }


        public static void LoadCustomerFromCall(int CustomerId)
        {
            if (THIS._Form is FrmMN0301)
            {
                ((FrmMN0301)THIS._Form).LoadCustomer(CustomerId);
            }
            else
            {
                //THIS._Form = new FrmMN0301(THIS.ShareOrderDataSet);
                THIS._Form = new FrmMN0301();
                if (!LocalUser.Instance.LogInInformation.IsAdmin && LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {
                    var mClientUsesAdapter = new ClientUsersTableAdapter();
                    var mTable = mClientUsesAdapter.GetData(LocalUser.Instance.LogInInformation.ClientId);
                    if (mTable.Any(c => c.ClientUserId == LocalUser.Instance.LogInInformation.ClientUserId && !c.AllowWrite))
                    {
                        THIS.mHelper.LockWirte(THIS._Form);
                    }
                }
                try
                {
                    if (THIS._Form.MaximizeBox)
                    {
                        foreach (var item in THIS.MdiChildren)
                        {
                            item.Close();
                        }
                        THIS._Form.WindowState = FormWindowState.Maximized;
                        THIS._Form.MdiParent = THIS;
                        ((FrmMN0301)THIS._Form).LoadCustomer(CustomerId);
                        THIS._Form.Show();
                    }
                }
                catch (Exception) { }

            }
        }


        //public void LoadOrders(int OrderId)
        //{


        //    THIS._Form = new FrmMN0301(OrderId);


        //    try
        //    {


        //        bool FrmisExist = new bool();
        //        FrmisExist = false;
        //        string tabName = "";
        //        foreach (Form form1 in Application.OpenForms)
        //        {
        //            if (form1.GetType() == THIS._Form.GetType())
        //            {
        //                FrmisExist = true;

        //                tabName = THIS._Form.Name;
        //            }
        //        }
        //        // 폼존재여부에 따라서 생성과 파기
        //        if (!FrmisExist)
        //        {
        //            THIS._Form.WindowState = FormWindowState.Maximized;
        //            THIS._Form.MdiParent = this;
        //            THIS._Form.Show();
        //            THIS._Form.Activate();
        //        }
        //        else
        //        {


        //            foreach (TabPage tab in tabControl1.TabPages)
        //            {

        //                if (THIS._Form.Text.Equals(tab.Text.Replace("    ", "")))
        //                {
        //                    tabControl1.SelectedTab = tab;
        //                    //found = true;
        //                }
        //            }

        //            _Form.Dispose();
        //        }




        //    }
        //    catch (Exception) { }



        //}

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabControl1.SelectedTab != null) && (tabControl1.SelectedTab.Tag != null))
                (tabControl1.SelectedTab.Tag as Form).Select();

            if (tabControl1.SelectedIndex == -1) return;
        }

        private void FrmMDI_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
                tabControl1.Visible = false; // If no any child form, hide tabControl
            else
            {
                this.ActiveMdiChild.WindowState = FormWindowState.Maximized; // Child form always maximized

                // If child form is new and no has tabPage, create new tabPage
                if (this.ActiveMdiChild.Tag == null)
                {
                    // Add a tabPage to tabControl with child form caption
                    TabPage tp = new TabPage(this.ActiveMdiChild.Text + "    ");
                    tp.Tag = this.ActiveMdiChild;
                    tp.Parent = tabControl1;
                    tabControl1.SelectedTab = tp;

                    this.ActiveMdiChild.Tag = tp;
                    this.ActiveMdiChild.FormClosed += new FormClosedEventHandler(ActiveMdiChild_FormClosed);

                    focusCloseButton();
                }

                if (!tabControl1.Visible) tabControl1.Visible = true;
            }
        }

        private void focusCloseButton()
        {
            Rectangle rect = tabControl1.GetTabRect(HoveredTabIndex);
            closeButtonPosition =
                new Rectangle(rect.Right - 15
                , rect.Top + 4
                , CloseImageDynamic.Width,
                CloseImageDynamic.Height);
        }
        private void ActiveMdiChild_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((sender as Form).Tag as TabPage).Dispose();
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Bitmap CloseImageTarget = CloseImageDynamic;
            Brush backBruch;
            if (e.Index != HoveredTabIndex)
                CloseImageTarget = Properties.Resources.hover;

            if (e.Index == this.tabControl1.SelectedIndex)
            {
                // e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds);
                backBruch = new SolidBrush(Color.Silver);

                // foreBruch = Brushes.Blue;

            }
            else
            {
                backBruch = new SolidBrush(Color.Gainsboro);
                // foreBruch = Brushes.Black;

            }
            e.Graphics.FillRectangle(backBruch, e.Bounds);
            Rectangle recTab = e.Bounds;


            #region  DrawTabTitle

            e.Graphics.DrawString(
                this.tabControl1.TabPages[e.Index].Text
                , e.Font, Brushes.Black
                , e.Bounds.Left + 5
                , e.Bounds.Top + 2
                );

            #endregion

            #region  DrawCloseButton

            e.Graphics.DrawImage(
                CloseImageTarget
                , e.Bounds.Right - CloseImageDynamic.Width
                , e.Bounds.Top
                , CloseImageDynamic.Width - 2
                , CloseImageDynamic.Height - 2
                );

            #endregion


        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (closeButtonPosition.Contains(e.Location))
            //&& MessageBox.Show("창을 닫으시겠습니까?"
            //                , "확인"
            //                , MessageBoxButtons.YesNo
            //                , MessageBoxIcon.Question) == DialogResult.Yes
            //)
            {
                (this.tabControl1.TabPages[tabControl1.SelectedIndex].Tag as Form).Dispose(); //dispose the Form
                this.tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex); //Remove the Tab

                //after closing a tab, bring focus to the last tab
                //The if statement prevents error when there is only a single tab left
                if (tabControl1.TabCount == 0) return;

                tabControl1.SelectedTab = this.tabControl1.TabPages[tabControl1.TabCount - 1];
            }
        }

        private void tabControl1_MouseLeave(object sender, EventArgs e)
        {
            CloseImageDynamic = Properties.Resources.hover;
            //tabControl1.Invalidate();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseLeftDown = true;

            //if (closeButtonPosition.Contains(e.Location))
            //    CloseImageDynamic = Properties.Resources.press;
            //else
                CloseImageDynamic = Properties.Resources.hover;

            //tabControl1.Invalidate();
        }

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseLeftDown = false;
        }

        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            //Sets tab index for Close button to reference
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                if (tabControl1.GetTabRect(i).Contains(e.Location))
                {
                    HoveredTabIndex = i;
                    focusCloseButton();
                    break;
                }
            }

            //keep the press down image when moving
            if (isMouseLeftDown) return;

            //redraw to hover when moving onto the button
            //if (closeButtonPosition.Contains(e.Location))
            //    if (CloseImageDynamic != Properties.Resources.hover)
            //        CloseImageDynamic = Properties.Resources.hover;
            //    else return;

            ////redraw to default when moving off the button
            //else
            //    if (CloseImageDynamic != Properties.Resources._default)
            //    CloseImageDynamic = Properties.Resources._default;
            //else return;


            //tabControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FrmMN301_Call_PopupNew f = new FrmMN301_Call_PopupNew("01044402689", "07086806907",DateTime.Now);
            //f.Show();

            DtiTest dtiTest = new DtiTest();
            dtiTest.Show();
        }
    }
    public enum MenuAuth
    {
        None,
        Read,
        Write
    }
}
