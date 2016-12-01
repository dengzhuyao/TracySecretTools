using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TracySecretTool.Tools;

namespace AutoSignIn.Server
{
    public class IReader
    {
        public Tuple<bool, string> QianDao()
        {
            string uid = "i721885931";//我的
            //string uid = "i258346629";//调试的
            HttpStatusCode status = new HttpStatusCode();
            CookieContainer cc = new CookieContainer();

            //点击进入抽奖之前的页面
            string qiandaoBtnUrl = "http://ah2.zhangyue.com/zybook3/app/app.php?pk=BEqiandao&usr=" + uid + "&rgt=7&ca=sign.index";
            string btnResponse = WebRequestHelper.HttpGet(qiandaoBtnUrl, cc, Encoding.UTF8, out status);

            //正则找到抽奖页面
            string choosePrizeUrl = new Regex(@"(?<=checkGo\(').+(?='\))").Match(btnResponse).Value;
            if (choosePrizeUrl == "")
            {
                choosePrizeUrl = new Regex(@"(?<=gotoUrlWithSeedValidate\(').+(?='\))").Match(btnResponse).Value;
            }
            string seed = new Regex("(?<=var seed = ').+(?=';)").Match(btnResponse).Value;
            choosePrizeUrl = getRebuildUrl(choosePrizeUrl, seed);
            if (choosePrizeUrl == "")
            {
                return Tuple.Create(false, "ireader签到：抽奖页面地址为空");
            }

            //循环10次找到最大概率,10次都不行，就认命取最后一次吧
            string prizeResponse = "";
            for (int i = 0; i < 10; i++)
            {
                prizeResponse = WebRequestHelper.HttpGet(choosePrizeUrl, cc, Encoding.UTF8, out status);
                string strRate = new Regex(@"(?<=超越了)\d+(?=%)").Match(prizeResponse).Value;
                float rate = string.IsNullOrEmpty(strRate) ? 0 : float.Parse(strRate) / 100;
                if (rate > 0.45)
                {
                    break;
                }
            }
            string doQiandaoUrl = new Regex("(?<=var url = \").+(?=\";)").Match(prizeResponse).Value;
            string qiandaoResponse = WebRequestHelper.HttpGet(doQiandaoUrl, cc, Encoding.UTF8, out status);
            if (qiandaoResponse == "无效的 URI: 此 URI 为空。")
            {
                return Tuple.Create(false, "ireader签到：无结果");
            }
            else
            {
                JObject jo = JObject.Parse(qiandaoResponse);
                if (jo["status"].Value<string>() == "0")
                {
                    return Tuple.Create(true, "ireader签到：成功");
                }
                else
                {
                    return Tuple.Create(false, "ireader签到：失败");
                }
            }
        }
        private string getRebuildUrl(string url, string seed)
        {
            var hashCode = getHashCode(seed);
            url = url + "&_s_3d=" + seed + "&_s_3c=" + hashCode;
            return url;
        }

        private string getHashCode(string seed)
        {
            var data = seed.Split('|');
            int a = Convert.ToInt32(data[0]);
            var b = data[1];
            var name = data[2];
            var time = data[3];
            var param1 = getPart(b, a);
            var param2 = getPart(time, a);
            var param3 = getPart(name, a);
            var hashCode = param1 + "zY." + param2 + param3;
            int start = getStartIndex(hashCode, a);
            if (start < 0)
                start = hashCode.Length + start;
            int len = hashCode.Length - start;
            hashCode = hashCode.Substring(start, len > 8 ? 8 : len); ;
            return hashCode;
        }

        private string getPart(string b, int a)
        {
            var start = getStartIndex(b, a);
            if (start < 0)
                start = b.Length + start;
            int len = b.Length - start;
            return b.Substring(start, len > 3 ? 3 : len);
        }

        private int getStartIndex(string str, int a)
        {
            var length = str.Length;
            var start = a % length;
            return start < (float)length / 2f ? start : -start;
        }        
    }
}
