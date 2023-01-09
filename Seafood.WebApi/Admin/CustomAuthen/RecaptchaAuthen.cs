using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Amin.CustomAuthen
{
    public class RecaptchaAuthen
    {
        public static string Validate(string EncodedResponse)
        {
            try
            {
                var client = new WebClient();

                WebProxy wp = new WebProxy(System.Configuration.ConfigurationManager.AppSettings["ReCaptCha-Secret-ProxySever"]);
                client.Proxy = wp;
                string PrivateKey = System.Configuration.ConfigurationManager.AppSettings["ReCaptCha-Secret-Key"];
                string url = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse);
                var GoogleReply = client.DownloadString(url);

                var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RecaptchaAuthen>(GoogleReply);

                return captchaResponse.Success.ToLower();
            }
            catch
            {
                return "false";
            }

        }

        public static bool IsReCaptchValid(string EncodedResponse)
        {
            var result = false;
            try
            {
                string PrivateKey = System.Configuration.ConfigurationManager.AppSettings["ReCaptCha-Secret-Key"];
                Debug.WriteLine(PrivateKey);
                var requestUri = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse);
                Debug.WriteLine(requestUri);
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                var proxy_addr = System.Configuration.ConfigurationManager.AppSettings["ReCaptCha-Secret-ProxySever"];
                Debug.WriteLine(proxy_addr);
                if (!string.IsNullOrEmpty(proxy_addr))
                {
                    WebProxy wp = new WebProxy(proxy_addr);
                    request.Proxy = wp;
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        JObject jResponse = JObject.Parse(stream.ReadToEnd());
                        var isSuccess = jResponse.Value<bool>("success");
                        Debug.WriteLine(isSuccess);
                        result = (isSuccess) ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }


        private List<string> m_ErrorCodes;


    }
}