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

    class Call24ApiClass
    {
       //public static string SecretKey = "76a7c0dac0a8633379fe10b1bd6ee028";
       // public static string EncKey = "76a7c0dac0a86333581fd07f32c9ddbf";
       // public static string IvKey = "7a223c69896bbcf9";

        string ApiUrl = "http://api.15660088.com:18091";
        string code = "";
        string message = "";
        string data = "";
        
        //AES 암호화
        public string AESEncrypt(string input,string Call24SecretKey, string Call24EncKey, string Call24IvKey)
        {


            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256; //AES256으로 사용시 
                                   // aes.KeySize = 128; //AES128로 사용시 
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;

                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(Call24EncKey);
                aes.IV = Encoding.UTF8.GetBytes(Call24IvKey);
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
        public string AESDecrypt(string input, string Call24SecretKey, string Call24EncKey, string Call24IvKey)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                //aes.KeySize = 256; //AES256으로 사용시 
                aes.KeySize = 128; //AES128로 사용시 
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(Call24EncKey);
                aes.IV = Encoding.UTF8.GetBytes(Call24IvKey);
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


        string _data = "";
        string result = "";
        /// <summary>
        /// 주소찾기 
        /// </summary>
        /// <param name="Address"></param>
        public bool get_address_search(string sido, string gugun,string Dong, string ApiUrl, string Call24SecretKey, string Call24EncKey, string Call24IvKey)
        {

            #region

            string str = "{\"sido\":\"" + sido + "\",\"gugun\":\"" + gugun + "\"}";

            _data = AESEncrypt(str, Call24SecretKey, Call24EncKey, Call24IvKey);


            string url = $"{ApiUrl}/api/order/addr";
            string json = "{\"data\":\"" + _data + "\",\"userVal\":\"test\"}";
            JObject response = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
 {
     return true; // **** Always accept
 };


                var uriBuilder = new UriBuilder(url);






                // uriBuilder.Query = Parameter;
                Uri finalUrl = uriBuilder.Uri;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);
                
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Headers.Add("call24-api-key:76a7c0dac0a8633379fe10b1bd6ee028");




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

                            dataArray = AESDecrypt(Convert.ToString(dataArray), Call24SecretKey, Call24EncKey, Call24IvKey);

                            dynamic _dataArray = JsonConvert.DeserializeObject(dataArray);
                            foreach (var data in _dataArray)
                            {
                                var SIDO = data.nm.ToString();

                                _AddrList.Add(new Model
                                {
                                    SIDO = SIDO,


                                });
                            }
                        }
                    }

                }
                if (_AddrList.Count() > 0)
                {
                    //시도
                    if (String.IsNullOrEmpty(gugun))
                    {
                        return true;
                    }

                    //시군구
                    else if(String.IsNullOrEmpty(Dong))
                    {
                        return true;
                    }
                    else
                    {
                        var Query = _AddrList.Where(c => c.SIDO == Dong).ToArray();

                        if (Query.Any())
                        {
                            return true;
                        }
                        else
                        {

                            return false;
                        }
                    }
                   
                }
                else
                {
                    return false;

                }



            }
            catch
            {
                return false;
            }


            #endregion

        }


        BindingList<Model> _AddrList = new BindingList<Model>();

        class Model
        {
            public String SIDO { get; set; }
            //public String Address { get; set; }
            //public String Jibun { get; set; }
        }
    }

}
