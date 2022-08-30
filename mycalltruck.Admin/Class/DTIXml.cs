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
using mycalltruck.Admin.Class.Common;

namespace mycalltruck.Admin.Class
{
    public class DTIXml
    {

        private static DTIXml _Instance = new DTIXml();
        public static DTIXml Instance { get { return _Instance; } }

        private DTIXml()
        {
            TaxInvoiceDocument = new TaxInvoiceDocument();
          


        }
        [XmlElement]
        public string FileName { get; set; }
        [XmlElement]
        public TaxInvoiceDocument TaxInvoiceDocument { get; set; }

        public void Write()
        {
            //XML 문서 경로
            string folderPath = "C:\\차세로";
            folderPath = Path.Combine(folderPath, "전자세금계산서");
            if (Directory.Exists(folderPath) == false) Directory.CreateDirectory(folderPath);
            if (FileName == null || FileName == string.Empty)
                FileName = "DTIxml"+LocalUser.Instance.LogInInformation.Client.ClientId+".xml";
            string filePath = Path.Combine(folderPath, FileName);
            FileInfo xmlFile = new FileInfo(filePath);
            xmlFile.Delete();
            XmlSerializer serializer = new XmlSerializer(typeof(DTIXml));
            TextWriter writer = new StreamWriter(xmlFile.FullName);
            serializer.Serialize(writer, DTIXml.Instance);
            writer.Close();

            //  X509Certificate2 cert = null;
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = store.Certificates;
           

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

    

    public class TaxInvoiceDocument
    {
        //public TaxInvoiceDocument()
        //{
        //    //MYMID = "C:\\차세로\\MID";
        //    //CUSTOMER = "C:\\차세로\\거래처관리";
        //    //MYBANKNEW = "C:\\차세로\\대량이체";
        //    //MYCAR = "C:\\차세로\\배차관리";
        //    //TAX = "C:\\차세로\\세무";
        //    //DRIVER = "C:\\차세로\\차량관리";
        //    //MYTRADEFILE = "C:\\차세로\\첨부파일";
        //    //TRADE = "C:\\차세로\\매입관리";
        //    //MYBANKNEWRESULT = "C:\\차세로\\이체처리결과";

        //    //MYSTATS = "C:\\차세로\\정산관리";
        //    //MYETC = "C:\\차세로\\ETC";

        //    //FPISTRU = "C:\\FPIS";
        //    //FPISCAR = "C:\\FPIS";
        //    //KICC = "C:\\KICC";
           

        //}


        [XmlElement]
        public string IssueID { get; set; }
        [XmlElement]
        public string IssueDateTime { get; set; }
        [XmlElement]
        public string TypeCode { get; set; }
        [XmlElement]
        public string PurposeCode { get; set; }
        [XmlElement]
        public string AmendmentStatusCode { get; set; }
        [XmlElement]
        public string OriginalIssueID { get; set; }
        [XmlElement]
        public string DescriptionText { get; set; }
        
    }
}
