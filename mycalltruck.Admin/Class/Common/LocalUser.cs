using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;



namespace mycalltruck.Admin.Class.Common
{
    public class LocalUser
    {
        private static LocalUser _Instance = new LocalUser();
        public static LocalUser Instance { get { return _Instance; } }

        private LocalUser()
        {
            PersonalOption = new PersonlOption();
            LogInInformation = new LogInInformation();
        }
        //어플리케이션 시동시 인증서를 검색하여 인증서 목록을 반환한다.
        public static LocalUser[] GetLocalUsers()
        {
            List<LocalUser> localUsers = new List<LocalUser>();
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath = Path.Combine(folderPath, Application.ProductName);
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if (folder.Exists == false) folder.Create();
            FileInfo[] certDocs = folder.GetFiles("LocalUser*.Xml");
            FileInfo tempFile = new FileInfo(Path.Combine(folderPath, "Temp.xml"));
            foreach (var xmlFile in certDocs)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlFile.FullName);
                    EncryptedXml encryptedXML = new EncryptedXml(doc);
                    encryptedXML.DecryptDocument();
                    doc.Save(tempFile.FullName);
                    XmlReader reader = XmlReader.Create(tempFile.FullName);
                    XmlSerializer serializer = new XmlSerializer(typeof(LocalUser));
                    LocalUser tThis = serializer.Deserialize(reader) as LocalUser;
                    reader.Close();
                    if (tThis != null)
                    {
                        localUsers.Add(tThis);
                    }
                    tempFile.Delete();
                }
                //실패하면 인증 화면 
                catch (Exception E)
                {
                    try
                    {
                        if (xmlFile.Exists)
                            xmlFile.Delete();
                    }
                    catch
                    {
                    }
                }
            }
            return localUsers.ToArray();
        }
        //캡슐화된 Instance 값할당
        public static void SetInstance(LocalUser value)
        {
            _Instance = value;
        }
        public static void NewInstance()
        {
            _Instance = new LocalUser();
        }

        [XmlElement]
        public string FileName { get; set; }
        [XmlElement]
        public PersonlOption PersonalOption { get; set; }
        [XmlIgnore]
        public LogInInformation LogInInformation { get; set; }

        public void Write()
        {
            //XML 문서 경로
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath = Path.Combine(folderPath, Application.ProductName);
            if (Directory.Exists(folderPath) == false) Directory.CreateDirectory(folderPath);
            if (FileName == null || FileName == string.Empty)
                FileName = "LocalUser.Xml";
            string filePath = Path.Combine(folderPath, FileName);
            FileInfo xmlFile = new FileInfo(filePath);
            xmlFile.Delete();
            XmlSerializer serializer = new XmlSerializer(typeof(LocalUser));
            TextWriter writer = new StreamWriter(xmlFile.FullName);
            serializer.Serialize(writer, LocalUser.Instance);
            writer.Close();

          //  X509Certificate2 cert = null;
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = store.Certificates;
            //foreach (X509Certificate2 item in collection)
            //{
            //    if (item.Subject == "CN=EDUBILL_BADME")
            //    {
            //        cert = item;
            //        break;
            //    }
            //}

            //암호화
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile.FullName);
            EncryptedXml encrytedXml = new EncryptedXml(doc);
         //   EncryptedData encryptedData = encrytedXml.Encrypt(doc.DocumentElement, cert);
         //   EncryptedXml.ReplaceElement(doc.DocumentElement, encryptedData, false);
            doc.Save(xmlFile.FullName);

        }
        public void Clear()
        {
            //XML 문서 경로
           
            Instance.Write();
        }
    }

    public class LogInInformation
    {
        public LogInInformation()
        {

        }
        public LogInInformation(bool _IsLogin, bool _IsAdmin, string _LoginId, int _UserId)
        {
            IsLogin = _IsLogin;
            IsAdmin = _IsAdmin;
            LoginId = _LoginId;
            UserId = _UserId;
        }
        public LogInInformation(bool _IsLogin, bool _IsAdmin, string _LoginId
            , int _ClientId, int _SubClientId, int _ClientUserId, bool _IsSubClient, bool _IsAgent, String _ClientName
            , int _ExcelType, int _DriverType, int _ContType, int _OrderType, int _NoticeDriver, int _NoticeCnt
            , bool _AllowFPIS_In, bool _AllowSMS, bool _AllowOrder, bool _AllowFPIS, bool _AllowSub, bool _AllowMultiCustomer, bool _AllowTax,bool _HideAddTrade, bool _HideAddSales,bool _SmsYn,bool _IsClient,int _CustomerId,int _CustomerUserId,int _CustomerTeamId,int _BizGubun,string _CargoLoginId)
        {
            IsLogin = _IsLogin;
            IsAdmin = _IsAdmin;
            LoginId = _LoginId;
            ClientId = _ClientId;
            SubClientId = _SubClientId;
            ClientUserId = _ClientUserId;
            IsSubClient = _IsSubClient;
            IsAgent = _IsAgent;
            ClientName = _ClientName;
            ExcelType = _ExcelType;
            DriverType = _DriverType;
            ContType = _ContType;
            OrderType = _OrderType;
            NoticeDriver = _NoticeDriver;
            NoticeCnt = _NoticeCnt;
            AllowFPIS_In = _AllowFPIS_In;
            AllowSMS = _AllowSMS;
            AllowOrder = _AllowOrder;
            AllowFPIS = _AllowFPIS;
            AllowSub = _AllowSub;
            AllowMultiCustomer = _AllowMultiCustomer;
            AllowTax = _AllowTax;
            HideAddTrade = _HideAddTrade;
            HideAddSales = _HideAddSales;
            SmsYn = _SmsYn;
            IsClient = _IsClient;
            CustomerId = _CustomerId;
            CustomerUserId = _CustomerUserId;
            CustomerTeamId = _CustomerTeamId;
            BizGubun =   _BizGubun;
            CargoLoginId = _CargoLoginId;
        }

        // 사용자
        public bool IsLogin { get; private set; }
        public bool IsAdmin { get; private set; }
        public string LoginId { get; private set; }
        // 관리자
        public int UserId { get; private set; }
        // 운송사
        public int ClientId { get; private set; }
        public int SubClientId { get; private set; }
        public int ClientUserId { get; private set; }
        public bool IsSubClient { get; private set; }
        public bool IsAgent { get; private set; }
        public String ClientName { get; set; }

        public int ExcelType { get; private set; }
        public int DriverType { get; private set; }
        public int ContType { get; private set; }
        public int OrderType { get; private set; }

        public int NoticeDriver { get; private set; }
        public int NoticeCnt { get; private set; }

        public bool AllowFPIS_In { get; private set; }
        public bool AllowSMS { get; private set; }
        public bool AllowOrder { get; private set; }
        public bool AllowFPIS { get; private set; }
        public bool AllowSub { get; private set; }
        public bool AllowMultiCustomer { get; private set; }
        public bool AllowTax { get; private set; }
        public bool HideAddTrade { get; private set; }
        public bool HideAddSales { get; private set; }
        public bool SmsYn { get; private set; }
        public bool IsClient { get; set; }
        public int CustomerId { get; set; }
        public int CustomerUserId { get; set; }
        public int CustomerTeamId { get; set; }
        public int BizGubun { get; set; }
        public string CargoLoginId { get; set; }
        public DataSets.ClientDataSet.ClientsRow Client { get { return _ClientTable?.First(); } }
        public DataSets.ClientDataSet.ClientsDataTable _ClientTable;

        public Guid SessionId { get; set; }

        public void LoadClient()
        {
            if (!IsAdmin)
            {
                var clientsDataAdapter = new DataSets.ClientDataSetTableAdapters.ClientsTableAdapter();
                _ClientTable = clientsDataAdapter.Find(ClientId);
            }
        }
    }

    public class PersonlOption
    {
        public PersonlOption()
        {
            MYMID = "C:\\차세로\\MID";
            CUSTOMER = "C:\\차세로\\거래처관리";
            MYBANKNEW = "C:\\차세로\\대량이체";
            MYCAR = "C:\\차세로\\배차관리";
            TAX = "C:\\차세로\\세무";
            DRIVER = "C:\\차세로\\차량관리";
            MYTRADEFILE = "C:\\차세로\\첨부파일";
            TRADE = "C:\\차세로\\매입관리";
            MYBANKNEWRESULT = "C:\\차세로\\이체처리결과";

            MYSTATS = "C:\\차세로\\정산관리";
            MYETC = "C:\\차세로\\ETC";

            FPISTRU = "C:\\FPIS";
            FPISCAR = "C:\\FPIS";
            KICC = "C:\\KICC";
        }
        [XmlElement]
        public bool IDSave { get; set; }

        [XmlElement]
        public bool GonjJiNO { get; set; }

        [XmlElement]
        public DateTime GonjJiDate { get; set; }

        [XmlElement]
        public string UserID { get; set; }
        [XmlElement]
        public int UserGubun { get; set; }
        [XmlElement]
        public string FPISTRU { get; set; }
        [XmlElement]
        public string FPISCAR { get; set; }
        [XmlElement]
        public string TAX { get; set; }
        [XmlElement]
        public string MYCAR { get; set; }
        [XmlElement]
        public string CUSTOMER { get; set; }
        [XmlElement]
        public string DRIVER { get; set; }
        [XmlElement]
        public string TRADE { get; set; }
        [XmlElement]
        public string KICC { get; set; }
        [XmlElement]
        public bool CardNumSave { get; set; }
        [XmlElement]
        public string CardNum { get; set; }
        [XmlElement]
        public int CardMonth { get; set; }
        [XmlElement]
        public int CardYear { get; set; }

        [XmlElement]
        public string MYBANKNEW { get; set; }

        [XmlElement]
        public string MYBANKNEWRESULT { get; set; }

        [XmlElement]
        public string MYMID { get; set; }

        [XmlElement]
        public string MYTRADEFILE { get; set; }

        [XmlElement]
        public string MYSTATS { get; set; }

        [XmlElement]
        public string MYETC { get; set; }
    }

}
