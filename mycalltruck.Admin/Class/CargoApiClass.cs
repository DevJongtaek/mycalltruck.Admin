using Newtonsoft.Json.Linq;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;

namespace mycalltruck.Admin.Class
{
 
   class CargoApiClass
    {
        string code = "";
        string msg = "";
        string Key = "783AE1841E52783A";
        string _SHA256HASH = "";
        //string ApiLiveUrl = "http://cargo.api.labbgsoft.kr:8088";
        //string ApiTestUrl = "http://cargo.apitest.labbgsoft.kr:8080";

        string Date = DateTime.Now.ToString("yyyyMMddHHmmss");
        public static String SHA256Hash(String data)
        {
            
            try
            {
                
                SHA256 sha = new SHA256Managed();
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hash)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
                return stringBuilder.ToString();

                
            }
            catch (Exception ex)
            {
                throw new RuntimeException(ex);
            }
        }
        

        public void Cargo_regist(string COMCODE, string ORDERCODE, string LOADAY, string LOACITY, string LOACODE, string POIX, string POIY, string DOWDAY, string DOWCITY, string DOWCODE, string POIX_OUT, string POIY_OUT, string CARTON, string CARTYPE, string LOADTYPE, string OWID, string OWNAME, string PAYMENT, string PAY, string FEE, string INFO, string ETC, string PHONE, string SATYPE, string HATYPE, string WEIGHT, string HASH,string ApiUrl)
        {

            _SHA256HASH = SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);




            string Parameter = "";
            Parameter = String.Format(@"COMCODE={0}&ORDERCODE={1}&LOADAY={2}&LOACITY={3}&LOACODE={4}&POIX={5}&POIY={6}&DOWDAY={7}&DOWCITY={8}&DOWCODE={9}&POIX_OUT={10}&POIY_OUT={11}&CARTON={12}&CARTYPE={13}&LOADTYPE={14}&OWID={15}&OWNAME={16}&PAYMENT={17}&PAY={18}&FEE={19}&INFO={20}&ETC={21}&PHONE={22}&SATYPE={2}&HATYPE={24}&WEIGHT={25}&HASH={26}", COMCODE,
ORDERCODE,LOADAY,LOACITY,LOACODE,POIX,POIY,DOWDAY,DOWCITY,DOWCODE,POIX_OUT,POIY_OUT,CARTON,CARTYPE,LOADTYPE,OWID,OWNAME,PAYMENT,PAY,FEE,INFO,ETC,PHONE,SATYPE,HATYPE,WEIGHT,HASH
);




            JObject response = null;

            var uriBuilder = new UriBuilder($"{ApiUrl}service/cj/set_cargo_regist");


            uriBuilder.Query = Parameter;
            Uri finalUrl = uriBuilder.Uri;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);



            request.Method = "POST";
            request.ContentType = "text/json;";
            request.ContentLength = 0;

            request.Headers.Add("header-staff-api", "value");

            var httpResponse = (HttpWebResponse)request.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = JObject.Parse(streamReader.ReadToEnd());

                //SVC_RT = response["SVC_RT"].ToString();
                //SVC_MSG = response["SVC_MSG"].ToString();

                //if (SVC_RT == "0000")
                //{
                //    SmsResult = "성공";
                //    successcount++;
                //}
                //else
                //{
                //    SmsResult = "실패";
                //    failcount++;
                //    _SVC_RT.Add(SVC_RT);
                //}

            }
        }


       
      
        string result;
       /// <summary>
       /// 주소찾기 문자검색
       /// </summary>
       /// <param name="Address"></param>
        public string get_address_search(string FULLAddress, string AddressStreet,string ApiUrl)
        {

            #region
         




            _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);
            string url = $"{ApiUrl}/service2/csr/get_address_search";
            string json = "{\"COMCODE\":\"207\",\"VALUE\":\""+ AddressStreet + "\",\"HASH\":\""+ _SHA256HASH + "\"}";
            JObject response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                    stream.Flush();
                    stream.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    
                    result = reader.ReadToEnd();
                

                    response = JObject.Parse(result);

                    msg = response["msg"].ToString();
                    code = response["code"].ToString();

                    if (code == "200")
                    {
                        
                        dynamic _Array = JsonConvert.DeserializeObject(result);
                        if (_Array.data != null)
                        {
                            dynamic dataArray = _Array.data;
                            foreach (var data in dataArray)
                            {
                                var GUN = data.GUN.ToString();
                                var CODE = data.CODE.ToString();
                                var DONG = data.DONG;
                                var SIDO = data.SIDO;
                                var POIY = data.POIY;
                                var POIX = data.POIX;
                                var GU = data.GU;
                                var LOCNUM = data.LOCNUM;
                                _AddrList.Add(new Model
                                {
                                    GUN = GUN,
                                    CODE = CODE,
                                    DONG = DONG,
                                    SIDO = SIDO,
                                    POIY = POIY,
                                    POIX = POIX,
                                    GU = GU,
                                    LOCNUM = LOCNUM,
                                    FULLADDRESS = SIDO + "" + GU + "" + GUN + "" + DONG,
                                    BFULLADDRESS = SIDO + "" + GU + "" + "" + DONG,

                                });
                            }
                        }
                    }
                   
                }

                if(FULLAddress.Contains("부천"))
                {
                    var Query = _AddrList.Where(c => c.BFULLADDRESS == FULLAddress).ToArray();

                    if (Query.Any())
                    {
                        return Query.First().CODE + "," + Query.First().POIX + "," + Query.First().POIY + "," + Query.First().SIDO + "," + Query.First().GU + "," + Query.First().GUN + "," + Query.First().DONG;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    var Query = _AddrList.Where(c => c.FULLADDRESS == FULLAddress).ToArray();

                    if (Query.Any())
                    {
                        return Query.First().CODE + "," + Query.First().POIX + "," + Query.First().POIY + "," + Query.First().SIDO + "," + Query.First().GU + "," + Query.First().GUN + "," + Query.First().DONG;
                    }
                    else
                    {
                        return "";
                    }
                }


            }
            catch
            {
                return "";
            }


            #endregion

        }


        /// <summary>
        ///  주소찾기 스텝1
        /// </summary>
        public String Get_addr_search1(string addrstate,string ApiUrl)
        {

            #region

            _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);
            string url = $"{ApiUrl}/service2/csr/get_addr_search";
            string json = "{\"COMCODE\":\"207\",\"STEP\":\"1\",\"HASH\":\"" + _SHA256HASH + "\",\"CODE\":\"\"}";
            JObject response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                    stream.Flush();
                    stream.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    result = reader.ReadToEnd();


                    response = JObject.Parse(result);

                    msg = response["msg"].ToString();
                    code = response["code"].ToString();

                    if (code == "200")
                    {

                        dynamic _Array = JsonConvert.DeserializeObject(result);
                        if (_Array.data != null)
                        {
                            _AddrList1.Clear();
                            dynamic dataArray = _Array.data;
                            foreach (var data in dataArray)
                            {
                                var GUN = data.GUN.ToString();
                                var CODE = data.CODE.ToString();
                                var DONG = data.DONG;
                                var SIDO = data.SIDO;
                                var POIY = data.POIY;
                                var POIX = data.POIX;
                                var GU = data.GU;
                                var LOCNUM = data.LOCNUM;
                                _AddrList1.Add(new Model
                                {
                                    GUN = GUN,
                                    CODE = CODE,
                                    DONG = DONG,
                                    SIDO = SIDO,
                                    POIY = POIY,
                                    POIX = POIX,
                                    GU = GU,
                                    LOCNUM = LOCNUM,

                                });
                            }
                        }
                    }

                }

               var Query = _AddrList1.Where(c => c.SIDO.Contains(addrstate)).ToArray();

                if(Query.Any())
                {
                    return Query.First().CODE;
                }
                else
                {
                    return "";
                }

               
            }
            catch
            {
                return "";
            }


            #endregion
        }

        /// <summary>
        ///  주소찾기 STEP2
        /// </summary>
        /// <param name="_Code"></param>
        public string get_addr_search2(string _Code,string addrCity,string ApiUrl)
        {
            #region





            _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);
            string url = $"{ApiUrl}/service2/csr/get_addr_search";
            string json = "{\"COMCODE\":\"207\",\"STEP\":\"2\",\"HASH\":\"" + _SHA256HASH + "\",\"CODE\":\""+ _Code + "\"}";
            JObject response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                    stream.Flush();
                    stream.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    result = reader.ReadToEnd();


                    response = JObject.Parse(result);

                    msg = response["msg"].ToString();
                    code = response["code"].ToString();

                    if (code == "200")
                    {
                      

                        dynamic _Array = JsonConvert.DeserializeObject(result);
                        if (_Array.data != null)
                        {
                            _AddrList2.Clear();
                            dynamic dataArray = _Array.data;
                            foreach (var data in dataArray)
                            {
                                var GUN = data.GUN.ToString();
                                var CODE = data.CODE.ToString();
                                var DONG = data.DONG;
                                var SIDO = data.SIDO;
                                var POIY = data.POIY;
                                var POIX = data.POIX;
                                var GU = data.GU;
                                var LOCNUM = data.LOCNUM;
                                _AddrList2.Add(new Model
                                {
                                    GUN = GUN,
                                    CODE = CODE,
                                    DONG = DONG,
                                    SIDO = SIDO,
                                    POIY = POIY,
                                    POIX = POIX,
                                    GU = GU,
                                    LOCNUM = LOCNUM,

                                });
                            }
                        }
                    }

                }

                var Query = _AddrList2.Where(c => c.GU.Contains(addrCity)).ToArray();

                if(Query.Any())
                {
                    return Query.First().CODE;
                }
                else
                {
                    return "";
                }
                


            }
            catch
            {
                return "";
            }


            #endregion
        }


        /// <summary>
        ///  주소찾기 STEP3
        /// </summary>
        /// <param name="_Code"></param>
        public string get_addr_search3(string _Code, string addrCity, string addrState,string addrStreet,string ApiUrl)
        {
            #region





            _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);
            string url = $"{ApiUrl}/service2/csr/get_addr_search";
            string json = "{\"COMCODE\":\"207\",\"STEP\":\"3\",\"HASH\":\"" + _SHA256HASH + "\",\"CODE\":\"" + _Code + "\"}";
            JObject response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                    stream.Flush();
                    stream.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    result = reader.ReadToEnd();


                    response = JObject.Parse(result);

                    msg = response["msg"].ToString();
                    code = response["code"].ToString();

                    if (code == "200")
                    {


                        dynamic _Array = JsonConvert.DeserializeObject(result);
                        if (_Array.data != null)
                        {
                            _AddrList3.Clear();
                            dynamic dataArray = _Array.data;
                            foreach (var data in dataArray)
                            {
                                var GUN = data.GUN.ToString();
                                var CODE = data.CODE.ToString();
                                var DONG = data.DONG;
                                var SIDO = data.SIDO;
                                var POIY = data.POIY;
                                var POIX = data.POIX;
                                var GU = data.GU;
                                var LOCNUM = data.LOCNUM;
                                _AddrList3.Add(new Model
                                {
                                    GUN = GUN,
                                    CODE = CODE,
                                    DONG = DONG,
                                    SIDO = SIDO,
                                    POIY = POIY,
                                    POIX = POIX,
                                    GU = GU,
                                    LOCNUM = LOCNUM,

                                });
                            }
                        }
                    }

                }

                var Query = _AddrList3.Where(c => c.GU.Contains(addrCity) && c.SIDO.Contains(addrState) && c.DONG.Contains(addrStreet)).ToArray();

                if (Query.Any())
                {
                    return Query.First().CODE +","+Query.First().POIX+","+Query.First().POIY;
                }
                else
                {
                    return "";
                }


            }
            catch
            {
                return "";
            }


            #endregion
        }

        /// <summary>
        ///  주소찾기 STEP3
        /// </summary>
        /// <param name="_Code"></param>
        public string get_addr_searchname(string _Code, string addrCity, string addrState, string addrStreet,string ApiUrl)
        {
            #region





            _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);
            string url = $"{ApiUrl}/service2/csr/get_addr_search";
            string json = "{\"COMCODE\":\"207\",\"STEP\":\"3\",\"HASH\":\"" + _SHA256HASH + "\",\"CODE\":\"" + _Code + "\"}";
            JObject response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                    stream.Flush();
                    stream.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    result = reader.ReadToEnd();


                    response = JObject.Parse(result);

                    msg = response["msg"].ToString();
                    code = response["code"].ToString();

                    if (code == "200")
                    {


                        dynamic _Array = JsonConvert.DeserializeObject(result);
                        if (_Array.data != null)
                        {
                            _AddrList3.Clear();
                            dynamic dataArray = _Array.data;
                            foreach (var data in dataArray)
                            {
                                var GUN = data.GUN.ToString();
                                var CODE = data.CODE.ToString();
                                var DONG = data.DONG;
                                var SIDO = data.SIDO;
                                var POIY = data.POIY;
                                var POIX = data.POIX;
                                var GU = data.GU;
                                var LOCNUM = data.LOCNUM;
                                _AddrList3.Add(new Model
                                {
                                    GUN = GUN,
                                    CODE = CODE,
                                    DONG = DONG,
                                    SIDO = SIDO,
                                    POIY = POIY,
                                    POIX = POIX,
                                    GU = GU,
                                    LOCNUM = LOCNUM,

                                });
                            }
                        }
                    }

                }

                var Query = _AddrList3.Where(c => c.GU.Contains(addrCity) && c.SIDO.Contains(addrState) && c.DONG.Contains(addrStreet)).ToArray();

                if (Query.Any())
                {
                    return Query.First().SIDO + "," + Query.First().GUN + "," + Query.First().GU + "," + Query.First().DONG;
                }
                else
                {
                    return "";
                }


            }
            catch
            {
                return "";
            }


            #endregion
        }

        /// <summary>
        /// 공통코드 상하차방법
        /// </summary>
        /// <param name="Address"></param>
        public List<TYPEModel> Get_code_config(string Gubun,string ApiUrl)
        {

            #region

            List<TYPEModel> _TYPEModel = new List<TYPEModel>();
            
            _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);
            string url = $"{ApiUrl}/service2/csr/get_code_config";
            string json = "{\"COMCODE\":\"207\",\"HASH\":\"" + _SHA256HASH + "\"}";
            JObject response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                    stream.Flush();
                    stream.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    result = reader.ReadToEnd();


                    response = JObject.Parse(result);

                    msg = response["msg"].ToString();
                    code = response["code"].ToString();

                    if (code == "200")
                    {

                        dynamic _Array = JsonConvert.DeserializeObject(result);
                        if (_Array.data != null)
                        {
                            switch (Gubun)
                            { 

                                case "MOVETYPE":
                                    dynamic MOVETYPEdataArray = _Array.data.MOVETYPE;
                                    foreach (var data in MOVETYPEdataArray)
                                    {
                                        var index = data.index.ToString();
                                        var value = data.value.ToString();
                                        var key = data.key;

                                        _TYPEModel.Add(new TYPEModel
                                        {
                                            INDEX = index,
                                            VALUE = value,
                                            KEY = key,

                                        });
                                    }
                                    break;

                                case "TON":
                                    dynamic TONdataArray = _Array.data.TON;
                                    foreach (var data in TONdataArray)
                                    {
                                        var index = data.index.ToString();
                                        var value = data.value.ToString();
                                        var key = data.key;

                                        _TYPEModel.Add(new TYPEModel
                                        {
                                            INDEX = index,
                                            VALUE = value,
                                            KEY = key,

                                        });
                                    }
                                    break;

                                case "CARTYPE":

                                    dynamic CARTYPEArray = _Array.data.CARTYPE;
                                    foreach (var data in CARTYPEArray)
                                    {
                                        var index = data.index.ToString();
                                        var value = data.value.ToString();
                                        var key = data.key;

                                        _TYPEModel.Add(new TYPEModel
                                        {
                                            INDEX = index,
                                            VALUE = value,
                                            KEY = key,

                                        });
                                    }
                                    break;

                                case "ALONE":

                                    dynamic ALONEArray = _Array.data.ALONE;
                                    foreach (var data in ALONEArray)
                                    {
                                        var index = data.index.ToString();
                                        var value = data.value.ToString();
                                        var key = data.key;

                                        _TYPEModel.Add(new TYPEModel
                                        {
                                            INDEX = index,
                                            VALUE = value,
                                            KEY = key,

                                        });
                                    }
                                    break;

                                case "STATE":

                                    dynamic STATEArray = _Array.data.STATE;
                                    foreach (var data in STATEArray)
                                    {
                                        var index = data.index.ToString();
                                        var value = data.value.ToString();
                                        var key = data.key;

                                        _TYPEModel.Add(new TYPEModel
                                        {
                                            INDEX = index,
                                            VALUE = value,
                                            KEY = key,

                                        });
                                    }
                                    break;

                                case "PAYMETHOD":

                                    dynamic PAYMETHODArray = _Array.data.PAYMETHOD;
                                    foreach (var data in PAYMETHODArray)
                                    {
                                        var index = data.index.ToString();
                                        var value = data.value.ToString();
                                        var key = data.key;

                                        _TYPEModel.Add(new TYPEModel
                                        {
                                            INDEX = index,
                                            VALUE = value,
                                            KEY = key,

                                        });
                                    }
                                    break;
                            }
                                
                          


                            
                         

                        

                           
                        }
                    }

                }
                

            }
            catch
            {

            }
            return _TYPEModel;
            //return MOVETYPEModel;

            #endregion

        }
        BindingList<Model2> _CoreList = new BindingList<Model2>();
        public string roadaddr_trans(string roadaddr)
        {
            _CoreList.Clear();
            string _emdNm = "";
            string _sggNm = "";
            string addrnm = "";
            var Url = $"http://www.juso.go.kr/addrlink/addrLinkApi.do?resultType=json&countPerPage=100&keyword={roadaddr}&confmKey=U01TX0FVVEgyMDE2MTIyMjE2MTY0NjE3NTky";
            WebClient _WebClient = new WebClient();
            _WebClient.Encoding = Encoding.UTF8;
            var r = _WebClient.DownloadString(Url);
            dynamic _Array = JsonConvert.DeserializeObject(r);
            if (_Array.results != null)
            {
                dynamic jusoArray = _Array.results.juso;
                //foreach (var juso in jusoArray)
                //{
                //    var roadAddr = juso.roadAddr.ToString();
                //    var zipNo = juso.zipNo.ToString();
                //    var jibunAddr = juso.jibunAddr;
                //    var emdNm = juso.emdNm;

                //}
                dynamic jusototalcount = _Array.results.common.totalCount;
                var ijusototalcount = jusototalcount.ToString();
                if (ijusototalcount != "0")
                {
                    foreach (var juso in jusoArray)
                    {
                        var roadAddr = juso.roadAddr.ToString();
                        var zipNo = juso.zipNo.ToString();
                        var jibunAddr = juso.jibunAddr.ToString();
                        var emdNm = juso.emdNm.ToString();
                        var sggNm = juso.sggNm.ToString();
                        _CoreList.Add(new Model2
                        {
                            Zip = zipNo,
                            roadAddr = roadAddr,
                            jibunAddr = jibunAddr,
                            emdNm = emdNm,
                            sggNm = sggNm,
                        });
                    }

                    //var roadAddr = jusoArray.First.roadAddr.ToString();

                    //var zipNo = jusoArray.First.zipNo.ToString();
                    //var jibunAddr = jusoArray.First.jibunAddr.ToString();


                    var _Q = _CoreList.Where(c => c.jibunAddr.Contains(roadaddr) || c.roadAddr.Contains(roadaddr)).ToArray();
                    // _emdNm = _CoreList.Where(c => c.jibunAddr == roadaddr || c.roadAddr == roadaddr).First().emdNm;
                    //sggNm = _sggNm;
                    if (_Q.Any())
                    {
                        addrnm = _Q.First().sggNm + " " + _Q.First().emdNm;
                    }
                    
                }
                else
                { addrnm = ""; }

            }
            return addrnm;
        }

        //public static List<SelectModel> MOVETYPELIST
        //{
            
        //    get
        //    {
        //        List<SelectModel> r = new List<SelectModel>();
        //        foreach (var optionRow in SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").OrderBy(c => c.Seq))
        //        {
        //            r.Add(new SelectModel { lue = optionRow.Value, Text = optionRow.Name });
        //        }
        //        return r;
        //    }
        //}
        #region STORAGE
        class Model
        {
            public String GUN { get; set; }
            public String CODE { get; set; }
            public String DONG { get; set; }
            public String SIDO { get; set; }

            public String POIY { get; set; }
            public String POIX { get; set; }
            public String GU { get; set; }
            public String LOCNUM { get; set; }
            public String FULLADDRESS { get; set; }
            public String BFULLADDRESS { get; set; }
        }
        class Model2
        {
            public String Zip { get; set; }
            public String roadAddr { get; set; }
            public String jibunAddr { get; set; }
            public String emdNm { get; set; }
            public String sggNm { get; set; }
        }
        
        public  class SelectModel
        {
            public String INDEX { get; set; }
            public String VALUE { get; set; }
            public String KEY { get; set; }

        }


        class TONModel
        {
            public String INDEX { get; set; }
            public String VALUE { get; set; }
            public String KEY { get; set; }
          
        }
        public  class TYPEModel
        {
            public String INDEX { get; set; }
            public String VALUE { get; set; }
            public String KEY { get; set; }

        }
        class CARTYPEModel
        {
            public String INDEX { get; set; }
            public String VALUE { get; set; }
            public String KEY { get; set; }

        }
        class STATEModel
        {
            public String INDEX { get; set; }
            public String VALUE { get; set; }
            public String KEY { get; set; }

        }

        class PAYMETHODModel
        {
            public String INDEX { get; set; }
            public String VALUE { get; set; }
            public String KEY { get; set; }

        }

        class ALONEModel
        {
            public String INDEX { get; set; }
            public String VALUE { get; set; }
            public String KEY { get; set; }

        }
        #endregion
        BindingList<Model> _AddrList = new BindingList<Model>();
        BindingList<Model> _AddrList1 = new BindingList<Model>();
        BindingList<Model> _AddrList2 = new BindingList<Model>();
        BindingList<Model> _AddrList3 = new BindingList<Model>();

        List<TYPEModel> _MOVETYPELIST = new List<TYPEModel>();
        BindingList<TONModel> _TONLIST = new BindingList<TONModel>();
        BindingList<CARTYPEModel> _CARTYPELIST = new BindingList<CARTYPEModel>();
        BindingList<ALONEModel> _ALONELIST = new BindingList<ALONEModel>();
        BindingList<STATEModel> _STATELIST = new BindingList<STATEModel>();
        BindingList<PAYMETHODModel> _PAYMETHODLIST = new BindingList<PAYMETHODModel>();

    }

}
