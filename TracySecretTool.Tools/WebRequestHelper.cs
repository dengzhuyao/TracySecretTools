using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace TracySecretTool.Tools
{
    public class WebRequestHelper
    {
        private const string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.87 Safari/537.36";

        #region Post
        public static CookieContainer GetCookieContainer(string url, object paras = null)
        {
            CookieContainer mycookiecontainer = new CookieContainer();
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "post";
                myRequest.CookieContainer = mycookiecontainer;
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.UserAgent = userAgent;

                byte[] payload = null;
                string formData = GetFormData(paras);
                if (!string.IsNullOrEmpty(formData))
                {
                    payload = System.Text.Encoding.UTF8.GetBytes(formData);
                    myRequest.ContentLength = payload.Length;
                }

                Stream myrequeststream = myRequest.GetRequestStream();
                StreamWriter mystreamwriter = new StreamWriter(myrequeststream, Encoding.GetEncoding("gb2312"));
                if (payload != null && payload.Length > 0)
                {
                    //将请求参数写入流
                    myrequeststream.Write(payload, 0, payload.Length);
                }
                mystreamwriter.Close();
                myrequeststream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            }
            catch (Exception msg)
            {
                mycookiecontainer = null;
            }
            return mycookiecontainer;

        }

        public static string HttpPost(string url, CookieContainer mycookiecontainer, object paras = null)
        {
            return HttpPost(url, Encoding.UTF8, mycookiecontainer, paras);
        }

         public static string HttpPost(string url, Encoding encoding, CookieContainer mycookiecontainer, object paras = null)
        {
            return HttpPost(url, encoding, mycookiecontainer, null, paras);
        }
        public static string HttpPost(string url, Encoding encoding, CookieContainer mycookiecontainer,List<string> headers=null ,object paras = null)
        {
            string responseText = "";
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "post";
                myRequest.CookieContainer = mycookiecontainer;
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.UserAgent = userAgent;
                if (headers != null)
                {
                    headers.ForEach(p => myRequest.Headers.Add(p));
                }

                byte[] payload = null;
                string formData = GetFormData(paras);
                if (!string.IsNullOrEmpty(formData))
                {
                    payload = System.Text.Encoding.UTF8.GetBytes(formData);
                    myRequest.ContentLength = payload.Length;
                }

                Stream mySream = myRequest.GetRequestStream();
                if (payload != null && payload.Length > 0)
                {
                    mySream.Write(payload, 0, payload.Length);
                }
                mySream.Close();


                HttpWebResponse myResponse = (System.Net.HttpWebResponse)myRequest.GetResponse();
                StreamReader myreader = new StreamReader(myResponse.GetResponseStream(), encoding);
                responseText = myreader.ReadToEnd();
                myreader.Close();
            }
            catch (Exception ex)
            {
                responseText = ex.Message;
            }
            return responseText;
        }

        public static string HttpPost(string url, Encoding encoding, string strCookie, object paras = null)
        {
            string responseText = "";
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "post";
                myRequest.Headers.Add("Cookie: " + strCookie);
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.UserAgent = userAgent;
                //myRequest.Headers.Add("X-Client: javascript");
                myRequest.Headers.Add("X-CSRF-Token: MvLdmA22-8N2BbvcLNUrwmAph_d-NrlQ0yzo");

                byte[] payload = null;
                string formData = GetFormData(paras);
                if (!string.IsNullOrEmpty(formData))
                {
                    payload = System.Text.Encoding.UTF8.GetBytes(formData);
                    myRequest.ContentLength = payload.Length;
                }

                Stream mySream = myRequest.GetRequestStream();
                if (payload != null && payload.Length > 0)
                {
                    mySream.Write(payload, 0, payload.Length);
                }
                mySream.Close();


                HttpWebResponse myResponse = (System.Net.HttpWebResponse)myRequest.GetResponse();
                StreamReader myreader = new StreamReader(myResponse.GetResponseStream(), encoding);
                responseText = myreader.ReadToEnd();
                myreader.Close();
            }
            catch (Exception ex)
            {
                responseText = ex.Message;
            }
            return responseText;
        }
        #endregion

        #region Get
        public static string HttpGet(string url, CookieContainer cookiecontainer, out HttpStatusCode status)
        {
            return HttpGet(url, cookiecontainer, Encoding.GetEncoding("GB2312"), out status);
        }
        public static string HttpGet(string url, CookieContainer cookiecontainer, Encoding encoding, out HttpStatusCode status)
        {
            status = HttpStatusCode.NotFound;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                myReq.UserAgent = userAgent;
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                if (cookiecontainer != null)
                {
                    myReq.CookieContainer = cookiecontainer;
                }

                HttpWebResponse myResponse = (HttpWebResponse)myReq.GetResponse();
                status = myResponse.StatusCode;
                Stream receviceStream = myResponse.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, encoding);
                string strHTML = readerOfStream.ReadToEnd();

                readerOfStream.Close();
                receviceStream.Close();
                myResponse.Close();
                return strHTML;
            }
            catch (Exception msg)
            {
                return msg.Message;
            }
        }

        public static string HttpGet(string url, NameValueCollection nvc, CookieContainer cookiecontainer, Encoding encoding, out HttpStatusCode status)
        {
            status = HttpStatusCode.NotFound;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                myReq.UserAgent = userAgent;
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                foreach (string key in nvc)
                {
                    myReq.Headers.Add(key, nvc[key]);
                }
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");

                HttpWebResponse myResponse = (HttpWebResponse)myReq.GetResponse();
                status = myResponse.StatusCode;
                Stream receviceStream = myResponse.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, encoding);
                string strHTML = readerOfStream.ReadToEnd();

                readerOfStream.Close();
                receviceStream.Close();
                myResponse.Close();
                return strHTML;
            }
            catch (Exception msg)
            {
                return msg.Message;
            }
        }
        #endregion

        #region cookieContainer
        public static string GetCookie(string cookieName, CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c1 in colCookies) lstCookies.Add(c1);
            }
            var model = lstCookies.Find(p => p.Name.ToLower() == cookieName.ToLower());
            if (model != null)
            {
                return model.Value;
            }
            return "";
        }
        public static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }
        public static void AddCookie(string cookie, CookieContainer cc, string domain)
        {
            string[] tempCookies = cookie.Split(';');
            string tempCookie = null;
            int Equallength = 0;//  =的位置 
            string cookieKey = null;
            string cookieValue = null;
            //qg.gome.com.cn  cookie 
            for (int i = 0; i < tempCookies.Length; i++)
            {
                if (!string.IsNullOrEmpty(tempCookies[i]))
                {
                    tempCookie = tempCookies[i];
                    Equallength = tempCookie.IndexOf("=");
                    if (Equallength != -1)       //有可能cookie 无=，就直接一个cookiename；比如:a=3;ck;abc=; 
                    {
                        cookieKey = tempCookie.Substring(0, Equallength).Trim();
                        //cookie=

                        if (Equallength == tempCookie.Length - 1)    //这种是等号后面无值，如：abc=; 
                        {
                            cookieValue = "";
                        }
                        else
                        {
                            cookieValue = tempCookie.Substring(Equallength + 1, tempCookie.Length - Equallength - 1).Trim();
                        }
                    }
                    else
                    {
                        cookieKey = tempCookie.Trim();
                        cookieValue = "";
                    }
                    cc.Add(new Cookie(cookieKey, cookieValue, "", domain));
                }
            }
        }
        #endregion
        private static string GetFormData(object paras)
        {
            StringBuilder formData = new StringBuilder();
            if (paras != null)
            {
                Type t = paras.GetType();
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    string name = pi.Name;
                    object val = pi.GetValue(paras, null);

                    if (formData.ToString() == "")
                    {
                        formData.Append(name + "=" + val);
                    }
                    else
                    {
                        formData.Append("&" + name + "=" + val);
                    }
                }
            }
            return formData.ToString();
        }
        private static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
    }
}
