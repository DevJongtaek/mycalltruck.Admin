using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace mycalltruck.Admin.Class
{
    class BizNoInfoClass
    {
    }

   public class CRNRequest
    {
        private static CrnWeb crnWeb = new CrnWeb();
        //static void Main(string[] args)
        //{
        //    StringBuilder result = new StringBuilder("");
        //    int index = 0;
        //    foreach (String arg in args)
        //    {
        //        if (index == 0)
        //        {
        //            index++;
        //            continue;
        //        }
        //        result.Append(call(arg));
        //        result.AppendLine();
        //        index++;
        //    }
        //    Console.WriteLine(result.ToString());
        //}

        public static String call(String crn)
        {
            String result = crnWeb.postCRN(crn);
            result = result.Replace("\n", "").Replace("\t", " ");
            //return crn + "\t" + result;
            return  result;
        }
    }
    class CrnWeb
    {
        private readonly String postUrl = "https://teht.hometax.go.kr/wqAction.do?actionId=ATTABZAA001R08&screenId=UTEABAAA13&popupYn=false&realScreenId=";
        private String xmlRaw = "<map id=\"ATTABZAA001R08\"><pubcUserNo/><mobYn>N</mobYn><inqrTrgtClCd>1</inqrTrgtClCd><txprDscmNo>{CRN}</txprDscmNo><dongCode>15</dongCode><psbSearch>Y</psbSearch><map id=\"userReqInfoVO\"/></map>";

        public String postCRN(String crn)
        {
            byte[] contents = System.Text.Encoding.ASCII.GetBytes(xmlRaw.Replace("{CRN}", crn));
            HttpWebRequest request = createHttpWebRequest();
            setContentStream(request, contents);

            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                String resString = new StreamReader(responseStream).ReadToEnd();
                responseStream.Close();
                response.Close();
                return getCRNresultFromXml(resString);
            }
            response.Close();
            return "???";
        }
        private HttpWebRequest createHttpWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.Method = "POST";
            return request;
        }

        private void setContentStream(HttpWebRequest request, byte[] contents)
        {
            request.ContentLength = contents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(contents, 0, contents.Length);
            requestStream.Close();
        }
        private String getCRNresultFromXml(String xmlData)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            String crnResult = xmlDocument.SelectNodes("//trtCntn").Item(0).InnerText;
            return crnResult;
        }
    }
}
