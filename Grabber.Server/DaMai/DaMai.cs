using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TracySecretTool.Tools;

namespace Grabber.Server.DaMai
{
    public class DaMai
    {
        private List<string> listKeyword = new List<string> { "周杰伦", "梁静茹", "笛子" };
        public Tuple<bool, string> Search()
        {
            HttpStatusCode status = new HttpStatusCode();

            string loginUrl = "https://secure.damai.cn/login.aspx";
            var loginParam = new
              {
                  type = "0",
                  token = DateTime.Now.ToUnixTime13(),
                  nationPerfix = "86",
                  login_email = "13163736313",
                  login_pwd = "Windowsxp3",
                  code = "验证码"
              };

            //登录
            CookieContainer cc = WebRequestHelper.GetCookieContainer(loginUrl, loginParam);

            //访问演唱会列表页
            string strListResponse = WebRequestHelper.HttpGet("http://www.damai.cn/sz/Perform-1/", cc, Encoding.UTF8, out status);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(strListResponse);
            var rootNode = doc.DocumentNode;

            var totalPageNode = rootNode.SelectSingleNode("//em[@id='totalpages']");
            if (totalPageNode == null)
            {
                LogHelper.WriteGrabberLog("大麦网：totalPageNode为null");
            }
            //遍历每个分页
            Random rd=new Random();
            for (int i = 1; i <= Convert.ToInt32(totalPageNode.InnerHtml); i++)
            {
                string pageUrl = "http://www.damai.cn/ajax/cityCategoryProjectList.aspx?pageidx=" + i + "&categoryId=1&host=sz&sortType=0&startDate=&endDate=&sortKey=0&t=" + rd.NextDouble();
                string strPageResponse = WebRequestHelper.HttpGet(pageUrl, cc, Encoding.UTF8, out status);
                var projectList = HtmlNode.CreateNode("<div>" + strPageResponse + "</div>").SelectNodes("//li");
                //遍历每个项目(演唱会)
                foreach (HtmlNode projectNode in projectList)
                {
                    var node = projectNode.SelectSingleNode("//h5/a[1]");
                    string projectName = node.InnerHtml;
                    string projectUrl = node.Attributes["href"].Value;
                    string projectState = projectNode.SelectSingleNode("//p[@class='city-state']/span").InnerHtml;
                    //如果存在需要的项目，则购票
                    if (projectState == "售票中" && listKeyword.Any(p => projectName.Contains(p)))
                    {
                        //todo购票
                    }
                }
            }


            string qiandaoResponse = WebRequestHelper.HttpPost("http://www.itmxc.com/plugin.php?id=dsu_paulsign:sign&operation=qiandao&infloat=1&sign_as=1&inajax=1", Encoding.GetEncoding("gbk"), cc, new { });
            string result = new Regex("(?<=<div class=\"c\">(\r\n)*).+(?=</div>)").Match(qiandaoResponse).Value;
            return Tuple.Create(true, "MXC签到：" + result);
        }
    }
}
