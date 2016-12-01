using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TracySecretTool.Tools;

namespace MessageBomber.Server
{
    public class LianZhongYouXi
    {
        public bool SendMsg(string phone)
        {
            HttpStatusCode status = new HttpStatusCode();
            string url = "http://id.ourgame.com/sjyzm!getMobileYzm.do?passport=" + phone;
            string response = WebRequestHelper.HttpGet(url, new CookieContainer(), out status);
            if (response == "0")
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
