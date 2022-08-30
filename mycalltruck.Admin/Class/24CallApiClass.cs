using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace mycalltruck.Admin.Class
{
    class _24CallApiClass
    {
        //인증키
        public static readonly string SecretKey = "76a7c0dac0a8633379fe10b1bd6ee028"; //
        //암호화키
        public static readonly string key = "76a7c0dac0a86333581fd07f32c9ddbf";
        //IV키
        public static readonly string iv = "7a223c69896bbcf9";
        private string result;
        private string message;
        private string code;



        //AES 암호화
        public static string AESEncrypt(string input)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256; //AES256으로 사용시 
               // aes.KeySize = 128; //AES128로 사용시 
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buf = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }
                    buf = ms.ToArray();
                }
                string Output = Convert.ToBase64String(buf);
                return Output;
            }
            catch (Exception ex)
            {
               // Debug.LogError(ex.Message);
                return ex.Message;
            }
        }

        //AES 복호화
        public static string AESDecrypt(string input)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                //aes.KeySize = 256; //AES256으로 사용시 
                aes.KeySize = 128; //AES128로 사용시 
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                var decrypt = aes.CreateDecryptor();
                byte[] buf = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }
                    buf = ms.ToArray();
                }
                string Output = Encoding.UTF8.GetString(buf);
                return Output;
            }
            catch (Exception ex)
            {
                
                return ex.Message;
            }
        }


        public string get_address_search(string sido, string gugun, string ApiUrl)
        {
            BindingList<Model> _AddrList = new BindingList<Model>();

            #region

            string str = "{\"sido\":\"" + sido + "\",\"gugun\":\"" + gugun + "\"}";
            string _data = AESEncrypt(str);
            string url = $"{ApiUrl}/api/order/addr";
            string json = "{\"data\":\"" + _data + "\",\"userVal\":\"test\"}";
            JObject response = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                      
                       | SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
 delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                         System.Security.Cryptography.X509Certificates.X509Chain chain,
                         System.Net.Security.SslPolicyErrors sslPolicyErrors)
 {
     return true; // **** Always accept
 };


                var uriBuilder = new UriBuilder(url);






                // uriBuilder.Query = Parameter;
                Uri finalUrl = uriBuilder.Uri;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);



                // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var aaa = request.ProtocolVersion;

                //  request.ProtocolVersion = HttpVersion.Version11;

                request.Method = "POST";
                request.ContentType = "application/json";
                //string call24_api_key = "call24-api-key:76a7c0dac0a8633379fe10b1bd6ee028";
                request.Headers.Add("call24-api-key", "76a7c0dac0a8633379fe10b1bd6ee028");
                request.Headers.Add("Cache-Control", "no-cache");

                //request.Headers.Add("Method", "POST");
                //request.Headers.Add("ContentType", "application/json");
                //request.Headers.Add("Cache-Control", "no-cache");

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

                    message = response["message"].ToString();
                    code = response["code"].ToString();

                    if (code == "1")
                    {

                        dynamic _Array = JsonConvert.DeserializeObject(result);
                        if (_Array.data != null)
                        {
                            string dataArray = _Array.data;

                            dataArray = AESDecrypt(Convert.ToString(dataArray));

                            dynamic _dataArray = JsonConvert.DeserializeObject(dataArray);
                            foreach (var data in _dataArray)
                            {
                                var SIDO = data.nm.ToString();

                                _AddrList.Add(new Model
                                {
                                    Result = SIDO,


                                });
                            }
                        }
                    }

                }

                var Query = _AddrList.Where(c => c.Result == sido).ToArray();

                if (Query.Any())
                {
                    return Query.First().Result;
                }
                else
                {
                    return "";
                }



            }
            catch (Exception ex)
            {
                return "";
            }


            #endregion

        }
        class Model
        {
            public String Result { get; set; }
            //public String Address { get; set; }
            //public String Jibun { get; set; }
        }
    }
}
