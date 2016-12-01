using MessageBomber;
using MessageBomber.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TracySecretTool.Tools;

namespace Grabber.Server.TrainTicket
{
    public class TrainTicket
    {
        public List<string> Search(DateTime dt, string startCode, string endCode)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;//important!    所有证书验证都通过，无论证书是否有效

            CookieContainer cc = new CookieContainer();
            HttpStatusCode status = new HttpStatusCode();
            string searchUrl = "https://kyfw.12306.cn/otn/lcxxcx/query?purpose_codes=ADULT&queryDate=" + dt.ToString("yyyy-MM-dd") + "&from_station=" + startCode + "&to_station=" + endCode;

            string searchResponse = WebRequestHelper.HttpGet(searchUrl, cc, Encoding.UTF8, out status);

            List<string> listTrain = new List<string>();

            if (status == HttpStatusCode.OK)
            {
                JObject jobj = JObject.Parse(searchResponse);
                if (jobj["data"]["flag"].Value<bool>() == true)
                {
                    var list = jobj["data"]["datas"].ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i]["canWebBuy"].ToString() == "Y")
                        {
                            string trainCode = list[i]["station_train_code"].Value<string>();
                            string startTime = list[i]["start_time"].Value<string>();
                            string msg = string.Format("{0}次列车可以购买", trainCode.PadLeft(3));
                            if (new string[] { "G", "D" }.Contains(trainCode.Substring(0, 1)))
                            {
                                string ydzNum = list[i]["zy_num"].Value<string>();
                                string edzNum = list[i]["ze_num"].Value<string>();
                                int num = 0;
                                if (int.TryParse(ydzNum, out num))
                                {
                                    msg += string.Format("，一等座还剩{0}张", num.ToString().PadLeft(3));
                                }
                                if (int.TryParse(edzNum, out num))
                                {
                                    msg += string.Format("，二等座还剩{0}张", num.ToString().PadLeft(3));
                                }
                                msg += "，发车时间为" + dt.ToString("yyyy-MM-dd") + " " + startTime;
                            }
                            else
                            {
                                string rw_num = list[i]["rw_num"].Value<string>();
                                string yw_num = list[i]["yw_num"].Value<string>();
                                string yz_num = list[i]["yz_num"].Value<string>();
                                string wz_num = list[i]["wz_num"].Value<string>();
                                int num = 0;
                                if (int.TryParse(rw_num, out num))
                                {
                                    msg += string.Format("，软卧还剩{0}张", num.ToString().PadLeft(3));
                                }
                                if (int.TryParse(yw_num, out num))
                                {
                                    msg += string.Format("，硬卧还剩{0}张", num.ToString().PadLeft(3));
                                }
                                if (int.TryParse(yz_num, out num))
                                {
                                    msg += string.Format("，硬座还剩{0}张", num.ToString().PadLeft(3));
                                }
                                if (int.TryParse(wz_num, out num))
                                {
                                    msg += string.Format("，无座还剩{0}张", num.ToString().PadLeft(3));
                                }
                                msg += "，发车时间为" + dt.ToString("yyyy-MM-dd") + " " + startTime;
                            }
                            listTrain.Add(msg);
                        }
                    }
                }
            }
            return listTrain;
        }
    }
}
