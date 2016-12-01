using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TracySecretTool.Tools;

namespace MessageBomber.Server
{
    public class MeiTuan
    {
        public bool SendMsg(string phone)
        {
            HttpStatusCode status = new HttpStatusCode();
            string logUrl = "https://passport.meituan.com/account/unitivesignup";
            CookieContainer cc = new CookieContainer();
            string logResponse = WebRequestHelper.HttpGet(logUrl, cc, out status);
            string token = new Regex("(?<=<span id=\"csrf\" style=\"display:none\">).+(?=</span>)").Match(logResponse).Value;

            string url = "https://passport.meituan.com/account/signupcheck";
            var sendParam = new
            {
                mobile = phone
            };

            List<string> sendHead = new List<string> { "X-CSRF-Token: " + token };
            string response = WebRequestHelper.HttpPost(url, Encoding.UTF8, cc, sendHead, sendParam);

            if (response.Contains("\"success\":0"))
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
