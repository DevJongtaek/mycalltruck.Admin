using EP_CLI64_COMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.Common
{
    class KCCValidateAccount
    {
        public bool KCCValidateAccount_A(String BankCode, String Account_No, String Account_Name, String PrivateId, String BankName)
        {




            //인증 시작
            //  var LGD_BUYERIP = Request.UserHostAddress;
            var LGD_BUYERIP = "::1";
            var LGD_OID = "edu_bank_" + DateTime.Now.ToString("yyyyMMddHHmmss");


            var g_cert_file = "C:\\kicc\\cert\\pg_cert.pem";
            var g_log_dir = "C:\\kicc\\log";
            var g_log_level = 1;

            //* ============================================================================== */ 
            //* =   02. 쇼핑몰 지불 정보 설정                                                = */
            //* = -------------------------------------------------------------------------- = */ 
            //  var g_gw_url = "testgw.easypay.co.kr"; // 테스트 Gateway URL
            //static internal System.String g_gw_url    = "gw.easypay.co.kr"; // 리얼 Gateway URL
            var g_gw_url = "gw.easypay.co.kr";
            var g_gw_port = "80";                   // 포트번호(변경불가)

            //static internal System.String g_mall_id   = "05112221";             // 리얼 반영시 KICC에 발급된 mall_id 사용
            //  var g_mall_id = "T5102001";             // 리얼 반영시 KICC에 발급된 mall_id 사용
            var g_mall_id = "05527386";
            //* ============================================================================== */ 


            KICC64Class Easypay = new KICC64Class();
            Easypay.EP_CLI_COM__init(g_gw_url, g_gw_port, g_cert_file, g_log_dir, g_log_level);


            var acnt_txtype = "10";     // [필수]계좌이체 처리종류 10
            var bank_cd = BankCode;         // [필수]은행코드
            var account_no = Account_No;      // [필수]계좌번호
            var account_nm = Account_Name;      // [필수]계좌소유주명
            var resident_no = PrivateId;     // [필수]계좌소유주 주민번호(사업자번호)


            System.String res_cd = "";
            System.String res_msg = "";

            EasyPay_Client cLib = new EasyPay_Client();
            cLib.InitMsg();


            System.String TRAN_CD_NOR_PAYMENT = "00101000";   // 승인
            System.String TRAN_CD_NOR_MGR = "00201000";   // 변경

            var tr_cd = TRAN_CD_NOR_PAYMENT;

            //계좌인증    
            var tx_req_data = "";

            // 주문정보
            tx_req_data = cLib.SetEntry("order_data");
            //tx_req_data = cLib.SetValue("user_type", user_type, Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("order_no", LGD_OID, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("user_id", user_id, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("user_nm", user_nm, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("user_mail", user_mail, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("user_phone1", user_phone1, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("user_phone2", user_phone2, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("user_addr", user_addr, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("product_type", product_type, Convert.ToChar(31).ToString());
            //tx_req_data = cLib.SetValue("product_nm", product_nm, Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("product_amt", "0", Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetDelim(Convert.ToChar(28).ToString());

            //결제정보
            tx_req_data = cLib.SetEntry("pay_data");
            tx_req_data = cLib.SetEntry("common");
            tx_req_data = cLib.SetValue("tot_amt", "0", Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("currency", "00", Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("client_ip", LGD_BUYERIP, Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("escrow_yn", "N", Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("complex_yn", "N", Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetDelim(Convert.ToChar(30).ToString());


            tx_req_data = cLib.SetEntry("acnt");
            tx_req_data = cLib.SetValue("acnt_txtype", acnt_txtype, Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("bank_cd", bank_cd, Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("account_no", account_no, Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("account_nm", account_nm, Convert.ToChar(31).ToString());
            tx_req_data = cLib.SetValue("resident_no", resident_no, Convert.ToChar(31).ToString());

            tx_req_data = cLib.SetDelim(Convert.ToChar(30).ToString());

            tx_req_data = cLib.SetDelim(Convert.ToChar(28).ToString());

            cLib.InitMsg();

            Easypay.EP_CLI_COM__set_plan_data(tx_req_data);

            Easypay.EP_CLI_COM__proc(tr_cd, g_mall_id, LGD_BUYERIP, LGD_OID);
            res_cd = Easypay.EP_CLI_COM__get_value("res_cd");    // 응답코드
            res_msg = Easypay.EP_CLI_COM__get_value("res_msg");    // 응답메시지

            /* -------------------------------------------------------------------------- */
            /* ::: 결과 처리                                                              */
            /* -------------------------------------------------------------------------- */
            System.String r_cno = Easypay.EP_CLI_COM__get_value("cno");    // PG거래번호 
            System.String r_amount = Easypay.EP_CLI_COM__get_value("amount");    //총 결제금액
            System.String r_auth_no = Easypay.EP_CLI_COM__get_value("auth_no");    //승인번호
            System.String r_tran_date = Easypay.EP_CLI_COM__get_value("tran_date");    //승인일시
            System.String r_pnt_auth_no = Easypay.EP_CLI_COM__get_value("pnt_auth_no");    //포인트승인번호
            System.String r_pnt_tran_date = Easypay.EP_CLI_COM__get_value("pnt_tran_date");    //포인트승인일시
            System.String r_cpon_auth_no = Easypay.EP_CLI_COM__get_value("cpon_auth_no");    //쿠폰승인번호
            System.String r_cpon_tran_date = Easypay.EP_CLI_COM__get_value("cpon_tran_date");    //쿠폰승인일시
            System.String r_card_no = Easypay.EP_CLI_COM__get_value("card_no");    //카드번호
            System.String r_issuer_cd = Easypay.EP_CLI_COM__get_value("issuer_cd");    //발급사코드
            System.String r_issuer_nm = Easypay.EP_CLI_COM__get_value("issuer_nm");    //발급사명
            System.String r_acquirer_cd = Easypay.EP_CLI_COM__get_value("acquirer_cd");    //매입사코드
            System.String r_acquirer_nm = Easypay.EP_CLI_COM__get_value("acquirer_nm");    //매입사명
            System.String r_install_period = Easypay.EP_CLI_COM__get_value("install_period");    //할부개월
            System.String r_noint = Easypay.EP_CLI_COM__get_value("noint");    //무이자여부
            System.String r_bank_cd = Easypay.EP_CLI_COM__get_value("bank_cd");    //은행코드
            System.String r_bank_nm = Easypay.EP_CLI_COM__get_value("bank_nm");    //은행명
            System.String r_account_no = Easypay.EP_CLI_COM__get_value("account_no");    //계좌번호
            System.String r_deposit_nm = Easypay.EP_CLI_COM__get_value("deposit_nm");    //입금자명
            System.String r_expire_date = Easypay.EP_CLI_COM__get_value("expire_date");    //계좌사용만료일
            System.String r_cash_res_cd = Easypay.EP_CLI_COM__get_value("cash_res_cd");    //현금영수증 결과코드
            System.String r_cash_res_msg = Easypay.EP_CLI_COM__get_value("cash_res_msg");    //현금영수증 결과메세지
            System.String r_cash_auth_no = Easypay.EP_CLI_COM__get_value("cash_auth_no");    //현금영수증 승인번호
            System.String r_cash_tran_date = Easypay.EP_CLI_COM__get_value("cash_tran_date");    //현금영수증 승인일시
            System.String r_auth_id = Easypay.EP_CLI_COM__get_value("auth_id");    //PhoneID
            System.String r_billid = Easypay.EP_CLI_COM__get_value("billid");    //인증번호
            System.String r_mobile_no = Easypay.EP_CLI_COM__get_value("mobile_no");    //휴대폰번호
            System.String r_ars_no = Easypay.EP_CLI_COM__get_value("ars_no");    //전화번호
            System.String r_cp_cd = Easypay.EP_CLI_COM__get_value("cp_cd");    //포인트사/쿠폰사
            System.String r_used_pnt = Easypay.EP_CLI_COM__get_value("used_pnt");    //사용포인트
            System.String r_remain_pnt = Easypay.EP_CLI_COM__get_value("remain_pnt");    //잔여한도
            System.String r_pay_pnt = Easypay.EP_CLI_COM__get_value("pay_pnt");    //할인/발생포인트
            System.String r_accrue_pnt = Easypay.EP_CLI_COM__get_value("accrue_pnt");    //누적포인트
            System.String r_remain_cpon = Easypay.EP_CLI_COM__get_value("remain_cpon");    //쿠폰잔액
            System.String r_used_cpon = Easypay.EP_CLI_COM__get_value("used_cpon");    //쿠폰 사용금액
            System.String r_mall_nm = Easypay.EP_CLI_COM__get_value("mall_nm");    //제휴사명칭
            System.String r_escrow_yn = Easypay.EP_CLI_COM__get_value("escrow_yn");    //에스크로 사용유무
            System.String r_complex_yn = Easypay.EP_CLI_COM__get_value("complex_yn");    //복합결제 유무
            System.String r_canc_acq_date = Easypay.EP_CLI_COM__get_value("canc_acq_date");    //매입취소일시
            System.String r_canc_date = Easypay.EP_CLI_COM__get_value("canc_date");    //취소일시
            System.String r_refund_date = Easypay.EP_CLI_COM__get_value("refund_date");    //환불예정일시    



            if ("0000".Equals(res_cd))
            {
                //var DriverId = AccountHelper.GetDriverId(Request);
                //var Driver = ApplicationDbContext.Drivers.Find(DriverId);
                //Driver.PayBankCode = BankCode;
                //Driver.PayBankName = BankName;
                //Driver.PayInputName = Account_Name;
                //Driver.PayAccountNo = Account_No;
                //ApplicationDbContext.Entry(Driver).State = System.Data.Entity.EntityState.Modified;
                //ApplicationDbContext.SaveChanges();

                //ViewBag.PayBankCode = BankCode;
                //ViewBag.PayBankName = BankName;
                //ViewBag.PayAccountNo = Account_No;
                // ViewBag.
                // ViewBag.KCCValidate = "0";

                return true;
            }
            else
            {

                //   ViewBag.KCCValidate = "1";
                //   ModelState.AddModelError("", "화물량은 필수항목입니다.");
                return false;
            }
        }

        class EasyPay_Client
        {
            private System.String m_retData;    //처리 결과 값

            public EasyPay_Client()
            {
            }

            public void InitMsg()
            {
                m_retData = "";
            }

            public System.String SetEntry(System.String entry)
            {
                if (m_retData == "")
                    m_retData = entry + "=";
                else
                    m_retData = m_retData + entry + "=";

                return m_retData;
            }

            public System.String SetValue(System.String name, System.String value, System.String delim)
            {
                if (m_retData != "")
                {
                    if (value != "")
                        m_retData = m_retData + name + "=" + value + delim;
                }

                return m_retData;
            }

            public System.String SetDelim(System.String delim)
            {
                if (m_retData != "")
                    m_retData = m_retData + delim;

                return m_retData;
            }
        }
    }
}
