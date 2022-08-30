using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mycalltruck.Admin.DTIService;

namespace mycalltruck.Admin.Class
{

    public class DTIServiceClass
    {
        DTIServiceService _DTIService = new DTIServiceService();

        /// <summary>
        /// 연계코드
        /// </summary>
        string linkCd = "EDB";
        /// <summary>
        /// 연계사 사용중인 ID
        /// </summary>
        string linkId = "edubillsys";

        /// <summary>
        /// 사업자번호(-제외)
        /// </summary>
        string bizNo = "";
        /// <summary>
        /// 회사명
        /// </summary>
        string custName = "";
        /// <summary>
        /// 대표자명
        /// </summary>
        string ownerName = "";
        /// <summary>
        /// 업태
        /// </summary>
        string bizCond = "";
        /// <summary>
        /// 종목
        /// </summary>
        string bizItem = "";
        /// <summary>
        /// 담당자명
        /// </summary>
        string rsbmName = "";
        /// <summary>
        /// 이메일
        /// </summary>
        string email = "";
        /// <summary>
        /// 전화번호
        /// </summary>
        string telNo = "";
        /// <summary>
        /// 휴대폰번호
        /// </summary>
        string hpNo = "";
        /// <summary>
        /// 우편번호
        /// </summary>
        string zipCode = "";
        /// <summary>
        /// 주소1
        /// </summary>
        string addr1 = "";
        /// <summary>
        /// 주소2
        /// </summary>
        string addr2 = "";

        /// <summary>
        /// 결과코드
        /// </summary>
        string retVal = "";
        //오류메시지
        string errMsg = "";
        /// <summary>
        /// 고객사코드
        /// </summary>
        string frnNo = "";
        /// <summary>
        /// 사용자ID
        /// </summary>
        string userid = "";
        /// <summary>
        /// 사용자 PW
        /// </summary>
        string passwd = "";

        /// <summary>
        /// 인증서비밀번호
        /// </summary>
        private string certPw;

        /// <summary>
        /// 계산서XML
        /// </summary>
        string dTIXml;

        /// <summary>
        /// 메일발송여부(Y/N)
        /// </summary>
        string sendMailYn;
        /// <summary>
        /// SMS발송여부(Y/N)
        /// </summary>
        string sendSmsYn;

        /// <summary>
        /// SMS 내용
        /// </summary>
        string sendSmsMsg;
        /// <summary>
        /// 지연 시간
        /// </summary>
        string delayHour;

        /// <summary>
        /// 승인번호
        /// </summary>
        private string billNo;

        /// <summary>
        /// 일반포인트
        /// </summary>
        private string gnlPoint;
        /// <summary>
        /// 보너스 포인트
        /// </summary>
        private string outbnsPoint;
        /// <summary>
        /// 통합포인트
        /// </summary>
        private string totPoint;
        /// <summary>
        /// 재전송 할 승인번호(세미콜론 구분자로 입력) Ex) xxxxxxx;yyyyyy;zzzzzzzz;
        /// </summary>
        private string billNos;
        /// <summary>
        /// 이메일 (세미콜론구분자로 입력. billNos의 순서와 같게 입력)
        /// </summary>
        private string emails;

        /// <summary>
        /// 조회시작일 (검색일은 30일이내)
        /// </summary>
        private string startYYYYMMDD;
        /// <summary>
        /// 조회종료일 (검색일은 30일이내)
        /// </summary>
        private string endYYYYMMDD;
        /// <summary>
        /// 상태가 변경된 데이터(하기 샘플 참조)
        /// </summary>
        private string statusMsg;

        /// <summary>
        /// 조회일
        /// </summary>
        private string YYYYMMDD;

        /// <summary>
        /// 등록여부(Y/N)
        /// </summary>
        private string regYn;

        /// <summary>
        /// 만료일자(YYYYMMDD)
        /// </summary>
        private string expireDt;

        /// <summary>
        /// 승인번호
        /// </summary>
        private string apprNo;


        /// <summary>
        /// 회원가입
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="linkId"></param>
        /// <param name="bizNo"></param>
        /// <param name="custName"></param>
        /// <param name="ownerName"></param>
        /// <param name="bizCond"></param>
        /// <param name="bizItem"></param>
        /// <param name="rsbmName"></param>
        /// <param name="email"></param>
        /// <param name="telNo"></param>
        /// <param name="hpNo"></param>
        /// <param name="zipCode"></param>
        /// <param name="addr1"></param>
        /// <param name="addr2"></param>
        /// <returns></returns>
        public string Memberjoin(string linkCd, string linkId, string bizNo, string custName, string ownerName, string bizCond, string bizItem, string rsbmName, string email, string telNo, string hpNo, string zipCode, string addr1, string addr2)
        {
            _DTIService.membJoin(linkCd, linkId, bizNo, custName, ownerName, bizCond, bizItem, rsbmName, email, telNo, hpNo, zipCode, addr1, addr2, out retVal, out errMsg, out frnNo, out userid, out passwd);

            try
            {
                return retVal + "/" + errMsg + "/" + frnNo + "/" + userid + "/" + passwd;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }

        /// <summary>
        /// 세금계산서발행
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <param name="certPw"></param>
        /// <param name="dTIXml"></param>
        /// <param name="sendMailYn"></param>
        /// <param name="sendSmsYn"></param>
        /// <param name="sendSmsMsg"></param>
        /// <returns></returns>
        public string makeAndPublishSign(string linkCd, string frnNo, string userid, string passwd, string certPw, string dTIXml, string sendMailYn, string sendSmsYn, string sendSmsMsg)
        {
            _DTIService.makeAndPublishSign(linkCd, frnNo, userid, passwd, certPw, dTIXml, sendMailYn, sendSmsYn, sendSmsMsg, out retVal, out errMsg, out billNo, out gnlPoint, out outbnsPoint, out totPoint);
            try
            {
                // var Results = bizNo + ":" + custName + ":" + ownerName + ":" + bizCond + ":" + bizItem + ":" + rsbmName + ":" + email + ":" + telNo + ":" + hpNo + ":" + zipCode + ":" + addr1 + ":" + addr2;

                return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// 이메일전송
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <param name="billNos"></param>
        /// <param name="emails"></param>
        /// <returns></returns>
        public string sendMultiMail2(string linkCd,string frnNo, string userid, string passwd, string billNos, string emails)
        {
            _DTIService.sendMultiMail2(linkCd, frnNo, userid, passwd, billNos, emails, out retVal, out errMsg);

            try
            {
                return retVal + "/" + errMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        /// <summary>
        /// 회원정보수정
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <param name="custName"></param>
        /// <param name="ownerName"></param>
        /// <param name="bizCond"></param>
        /// <param name="bizItem"></param>
        /// <param name="rsbmName"></param>
        /// <param name="email"></param>
        /// <param name="telNo"></param>
        /// <param name="hpNo"></param>
        /// <param name="zipCode"></param>
        /// <param name="addr1"></param>
        /// <param name="addr2"></param>
        /// <returns></returns>
        public string updateMembInfo(string linkCd, string frnNo, string userid, string passwd, string custName, string ownerName, string bizCond, string bizItem, string rsbmName, string email, string telNo, string hpNo, string zipCode, string addr1, string addr2)
        {
            _DTIService.updateMembInfo(linkCd, frnNo, userid, passwd, custName, ownerName, bizCond, bizItem, rsbmName, email, telNo, hpNo, zipCode, addr1, addr2, out retVal, out errMsg);
            try
            {
                return retVal + "/" + errMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// 회원정보조회
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public string SelectMembInfo(string linkCd, string frnNo, string userid, string passwd)
        {
            _DTIService.selectMembInfo(linkCd, frnNo, userid, passwd, out retVal, out errMsg, out bizNo, out custName, out ownerName, out bizCond, out bizItem, out rsbmName, out email, out telNo, out hpNo, out zipCode, out addr1, out addr2);
            try
            {
                var Results = bizNo + ":" + custName + ":" + ownerName + ":" + bizCond + ":" + bizItem + ":" + rsbmName + ":" + email + ":" + telNo + ":" + hpNo + ":" + zipCode + ":" + addr1 + ":" + addr2;

                return retVal + "/" + errMsg + "/" + frnNo + "/" + userid + "/" + passwd + "/" + Results;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
        /// <summary>
        /// 상태변경내역 요청(사업자번호 기준)
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <param name="startYYYYMMDD"></param>
        /// <param name="endYYYYMMDD"></param>
        /// <returns></returns>
        public string changedStatusReq(string linkCd, string frnNo, string userid, string passwd, string startYYYYMMDD, string endYYYYMMDD)
        {
            _DTIService.changedStatusReq(linkCd, frnNo, userid, passwd, startYYYYMMDD, endYYYYMMDD, out retVal, out errMsg, out statusMsg);
            try
            {
                return retVal + "/" + errMsg + "/" + statusMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 상태변경내역 요청(사용자 ID 기준)
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <param name="startYYYYMMDD"></param>
        /// <param name="endYYYYMMDD"></param>
        /// <returns></returns>
        public string changedStatusReqById(string linkCd, string frnNo, string userid, string passwd, string startYYYYMMDD, string endYYYYMMDD)
        {

            _DTIService.changedStatusReqById(linkCd, frnNo, userid, passwd, startYYYYMMDD, endYYYYMMDD, out retVal, out errMsg, out statusMsg);
            try
            {
                return retVal + "/" + errMsg + "/" + statusMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 상태변경내역 응답
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <param name="statusMsg"></param>
        /// <returns></returns>
        public string changedStatusRes(string linkCd,string frnNo,string userid,string passwd,string statusMsg)
        {
            _DTIService.changedStatusRes(linkCd, frnNo, userid, passwd, statusMsg, out retVal, out errMsg);

            try
            {
                return retVal + "/" + errMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        /// <summary>
        /// 상태변경내역 요청(연계코드 기준)
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="YYYYMMDD"></param>
        /// <returns></returns>
        public string changedStatusReqByLinkCd(string linkCd,string YYYYMMDD)
        {
            _DTIService.changedStatusReqByLinkCd(linkCd, YYYYMMDD, out retVal, out errMsg, out statusMsg);

            try
            {
                return retVal + "/" + errMsg + "/" + statusMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 상태변경내역 응답(연계코드 기준)
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="statusMsg"></param>
        /// <returns></returns>
        public string changedStatusResByLinkCd(string linkCd,string statusMsg)
        {
            _DTIService.changedStatusResByLinkCd(linkCd, statusMsg, out retVal, out errMsg);

            try
            {
                return retVal + "/" + errMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 인증서 등록여부 및 만료일자 조회
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public string selectExpireDt(string linkCd, string frnNo, string userid, string passwd)
        {
            _DTIService.selectExpireDt(linkCd,frnNo,userid,passwd,out retVal, out errMsg,out regYn,out expireDt);

            try
            {
                return retVal + "/" + errMsg + "/" + regYn + "/" + expireDt;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 가입정보조회
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="linkId"></param>
        /// <param name="bizNo"></param>
        /// <param name="rsbmName"></param>
        /// <param name="email"></param>
        /// <param name="hpNo"></param>
        /// <returns></returns>
        public string GetMembJoinInf(string linkCd, string linkId, string bizNo, string rsbmName, string email, string hpNo)
        {
            _DTIService.getMembJoinInf(linkCd, linkId, bizNo, rsbmName, email, hpNo, out retVal, out errMsg, out frnNo, out userid, out passwd);

            try
            {
                return retVal + "/" + errMsg + "/" + frnNo + "/" + userid + "/" + passwd;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
        /// <summary>
        /// 세금계산서 지연발행
        /// </summary>
        /// <param name="linkCd"></param>
        /// <param name="frnNo"></param>
        /// <param name="userid"></param>
        /// <param name="passwd"></param>
        /// <param name="certPw"></param>
        /// <param name="dTIXml"></param>
        /// <param name="sendMailYn"></param>
        /// <param name="sendSmsYn"></param>
        /// <param name="sendSmsMsg"></param>
        /// <param name="delayHour"></param>
        /// <returns></returns>
        public string makeAndPublishSignDealy(string linkCd, string frnNo, string userid, string passwd, string certPw, string dTIXml, string sendMailYn, string sendSmsYn, string sendSmsMsg, string delayHour)
        {
            _DTIService.makeAndPublishSignDelay(linkCd, frnNo, userid, passwd, certPw, dTIXml, sendMailYn, sendSmsYn, sendSmsMsg, delayHour, out retVal, out errMsg, out billNo, out gnlPoint, out outbnsPoint, out totPoint);
            try
            {
                

                return retVal + "/" + errMsg + "/" + billNo + "/" + gnlPoint + "/" + outbnsPoint + "/" + totPoint;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string updateEtaxStatusToZ(string linkCd, string frnNo, string userid, string passwd, string apprNo)
        {
            _DTIService.updateEtaxStatusToZ(linkCd, frnNo, userid, passwd, apprNo, out retVal, out errMsg);

            try
            {
                return retVal + "/" + errMsg;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        







    }
}
