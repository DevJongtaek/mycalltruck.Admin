using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Popbill.Cashbill
{
 
    public class CashbillService : BaseService
    {
        public CashbillService(String LinkID, String SecretKey)
            : base(LinkID, SecretKey)
        {
            this.AddScope("140");
        }

        public ChargeInfo GetChargeInfo(String CorpNum)
        {
            return GetChargeInfo(CorpNum, null);
        }


        public ChargeInfo GetChargeInfo(String CorpNum, String UserID)
        {
            ChargeInfo response = httpget<ChargeInfo>("/Cashbill/ChargeInfo/", CorpNum, UserID);

            return response;
        }

        public Single GetUnitCost(String CorpNum)
        {
            UnitCostResponse response = httpget<UnitCostResponse>("/Cashbill?cfg=UNITCOST", CorpNum, null);

            return response.unitCost;
        }

        public String GetURL(String CorpNum, String UserID, String TOGO)
        {
            URLResponse response = httpget<URLResponse>("/Cashbill?TG=" + TOGO, CorpNum, UserID);

            return response.url;
        }

        public bool CheckMgtKeyInUse(String CorpNum, String MgtKey)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            try
            {
                CashbillInfo response = httpget<CashbillInfo>("/Cashbill/" + MgtKey, CorpNum, null);

                return string.IsNullOrEmpty(response.itemKey) == false;
            }
            catch (PopbillException pe)
            {
                if (pe.code == -14000003) return false;

                throw pe;
            }

        }

        public Response Register(String CorpNum, Cashbill cashbill)
        {
            return Register(CorpNum, cashbill, null);
        }
        public Response Register(String CorpNum, Cashbill cashbill, String UserID)
        {
            if (cashbill == null) throw new PopbillException(-99999999, "현금영수증 정보가 입력되지 않았습니다.");

            String PostData = toJsonString(cashbill);

            return httppost<Response>("/Cashbill", CorpNum, UserID, PostData, null);
        }

        public Response Update(String CorpNum, String MgtKey, Cashbill cashbill, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            if (cashbill == null) throw new PopbillException(-99999999, "현금영수증 정보가 입력되지 않았습니다.");

            String PostData = toJsonString(cashbill);

            return httppost<Response>("/Cashbill/" + MgtKey , CorpNum, UserID, PostData, "PATCH");
        }

        public Response Delete(String CorpNum, String MgtKey, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, UserID, null, "DELETE");
        }

        public Response Issue(String CorpNum, String MgtKey, String Memo, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            MemoRequest request = new MemoRequest();

            request.memo = Memo;
         
            String PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, UserID, PostData, "ISSUE");
        }

        public Response CancelIssue(String CorpNum, String MgtKey, String Memo, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            MemoRequest request = new MemoRequest();

            request.memo = Memo;

            String PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, UserID, PostData, "CANCELISSUE");
        }

        public Response SendEmail(String CorpNum, String MgtKey, String Receiver, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.receiver = Receiver;

            String PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, UserID, PostData, "EMAIL");
        }

        public Response SendSMS(String CorpNum, String MgtKey,String Sender, String Receiver, String Contents, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.sender = Sender;
            request.receiver = Receiver;
            request.contents = Contents;

            String PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, UserID, PostData, "SMS");
        }

        public Response SendFAX(String CorpNum, String MgtKey, String Sender, String Receiver, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.sender = Sender;
            request.receiver = Receiver;
         
            String PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, UserID, PostData, "FAX");
        }

        public Cashbill GetDetailInfo(String CorpNum, String MgtKey)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            return httpget<Cashbill>("/Cashbill/" + MgtKey + "?Detail", CorpNum, null);
        }

        public CashbillInfo GetInfo(String CorpNum, String MgtKey)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            return httpget<CashbillInfo>("/Cashbill/" + MgtKey, CorpNum, null);
        }

        public List<CashbillInfo> GetInfos(String CorpNum, List<String> MgtKeyList)
        {
            if(MgtKeyList == null || MgtKeyList.Count == 0) throw new PopbillException(-99999999,"관리번호 목록이 입력되지 않았습니다.");

            String PostData = toJsonString(MgtKeyList);

            return httppost<List<CashbillInfo>>("/Cashbill/States", CorpNum, null, PostData, null);
        }

        public List<CashbillLog> GetLogs(String CorpNum, string MgtKey)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            return httpget<List<CashbillLog>>("/Cashbill/" + MgtKey + "/Logs", CorpNum, null);
        }

        public String GetPopUpURL(String CorpNum, String MgtKey, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=POPUP", CorpNum, UserID);

            return response.url;

        }

        public String GetMailURL(String CorpNum, String MgtKey, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=MAIL", CorpNum, UserID);

            return response.url;

        }
        public String GetPrintURL(String CorpNum,String MgtKey, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=PRINT", CorpNum, UserID);

            return response.url;

        }
        public String GetEPrintURL(String CorpNum, String MgtKey, String UserID)
        {
            if (String.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "관리번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=EPRINT", CorpNum, UserID);

            return response.url;

        }
        public String GetMassPrintURL(String CorpNum, List<String> MgtKeyList, String UserID)
        {
            if (MgtKeyList == null || MgtKeyList.Count == 0) throw new PopbillException(-99999999, "관리번호 목록이 입력되지 않았습니다.");

            String PostData = toJsonString(MgtKeyList);

            URLResponse response = httppost<URLResponse>("/Cashbill/Prints", CorpNum, UserID, PostData, null);

            return response.url;
        }

        public Response RegistIssue(String CorpNum, Cashbill cashbill, String Memo)
        {
            return RegistIssue(CorpNum, cashbill, Memo, null);
        }

        public Response RegistIssue(String CorpNum, Cashbill cashbill, String Memo, String UserID)
        {
            if (cashbill == null) throw new PopbillException(-99999999, "현금영수증 정보가 입력되지 않았습니다.");

            cashbill.memo = Memo;

            String PostData = toJsonString(cashbill);

            return httppost<Response>("/Cashbill", CorpNum, UserID, PostData, "ISSUE");
        }

        public CBSearchResult Search(String CorpNum, String DType, String SDate, String EDate, String[] State, String[] TradeType, String[] TradeUsage, String[] TaxationType, String Order, int Page, int PerPage)
        {
            if (String.IsNullOrEmpty(DType)) throw new PopbillException(-99999999, "검색일자 유형이 입력되지 않았습니다.");
            if (String.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (String.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            String uri = "/Cashbill/Search";
            uri += "?DType=" + DType;
            uri += "&SDate=" + SDate;
            uri += "&EDate=" + EDate;
            uri += "&State=" + String.Join(",", State);
            uri += "&TradeType=" + String.Join(",", TradeType);
            uri += "&TradeUsage=" + String.Join(",", TradeUsage);
            uri += "&TaxationType=" + String.Join(",", TaxationType);
            uri += "&Order=" + Order;
            uri += "&Page=" + Page.ToString();
            uri += "&PerPage=" + PerPage.ToString();

            return httpget<CBSearchResult>(uri, CorpNum, null);
        }
       
        [DataContract]
        private class MemoRequest
        {
            [DataMember]
            public String memo;
        }

        [DataContract]
        private class ResendRequest
        {
            [DataMember]
            public String receiver;
            [DataMember]
            public String sender = null;
            [DataMember]
            public String contents = null;
        }

    }

}
