using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TracySecretTool.Tools;

namespace AutoSignIn.Server
{
    public class MXC
    {
        public Tuple<bool,string> QianDao()
        {
            HttpStatusCode status = new HttpStatusCode();

            string loginUrl = "http://www.itmxc.com/member.php?mod=logging&action=login&loginsubmit=yes&infloat=yes&lssubmit=yes&inajax=1";
            var loginParam = new
              {
                  fastloginfield = "username",
                  username = "dengzhuyao",
                  password = "windowsxp3",
                  quickforward = "yes",
                  handlekey = "ls"
              };

            //登录
            CookieContainer cc = WebRequestHelper.GetCookieContainer(loginUrl, loginParam);

            //访问主页
            string homePageResponse = WebRequestHelper.HttpGet("http://www.itmxc.com/forum.php", cc, out status);
            string formhash = new Regex("formhash=.+(?=['\"])").Match(homePageResponse).Value.Split('=')[1];

            //访问每日签到的选择页面
            WebRequestHelper.HttpGet("http://www.itmxc.com/plugin.php?id=dsu_paulsign:sign&" + formhash + "&infloat=yes&handlekey=dsu_paulsign&inajax=1&ajaxtarget=fwin_content_dsu_paulsign", cc, out status);

            //执行签到操作
            var qiandaoParam = new
              {
                  formhash = formhash,
                  qdxq = "kx",
                  qdmode = "2",
                  todaysay = "",
                  fastreply = "2"
              };  
            string qiandaoResponse= WebRequestHelper.HttpPost("http://www.itmxc.com/plugin.php?id=dsu_paulsign:sign&operation=qiandao&infloat=1&sign_as=1&inajax=1",Encoding.GetEncoding("gbk") ,cc, qiandaoParam);
            string result = new Regex("(?<=<div class=\"c\">(\r\n)*).+(?=</div>)").Match(qiandaoResponse).Value;
            return Tuple.Create(true, "MXC签到：" + result);
        }
    }
}
