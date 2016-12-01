using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TracySecretTool.Tools;

namespace MessageBomber.Server
{
    public class _796JiaoYiSuo
    {
        public bool CallPhone(string phone)
        {
            string url = "https://796.com:8080/api/v1.1/verify/voice/86:" + phone;
            string response = WebRequestHelper.HttpPost(url, new CookieContainer());
            JObject jobj = JObject.Parse(response);
            if (jobj != null && jobj["Status"].Value<string>() == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendMsg(string phone)
        {
            string url = "https://796.com:8080/api/v1.1/verify/mobile/86:" + phone;
            string response = WebRequestHelper.HttpPost(url, new CookieContainer());
            JObject jobj = JObject.Parse(response);
            if (jobj != null && jobj["Status"].Value<string>() == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
