using Grabber.Server.DaMai;
using Grabber.Server.TrainTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TracySecretTool.Server;
using TracySecretTool.Tools;

namespace Grabber
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    TTrain_StationBLL stationBll = new TTrain_StationBLL();
        //    DateTime dt = DateTime.Parse("2016-10-23");
        //    string startCode = stationBll.GetStationByName("深圳北").StationCode;
        //    string endCode = stationBll.GetStationByName("武汉").StationCode;

        //    List<string> listCanBuy = new TrainTicket().Search(dt, startCode, endCode);
        //    listCanBuy.ForEach(p => Console.WriteLine(p));
        //    Console.ReadKey();
        //}
        static void Main(string[] args)
        {
            string url = "http://211.154.153.150:6688/dparty/getwifiinfo?wifi=mac:f0-b0-52-27-74-08|ssid:PRO-GS|ss:-70&wifi=mac:f0-b4-29-51-1c-f5|ssid:KCT_FRONT|ss:-71&wifi=mac:f0-b4-29-51-11-19|ssid:KCT_OFFICE|ss:-56";

            HttpStatusCode status=new HttpStatusCode();
            string result = WebRequestHelper.HttpGet(url, new System.Net.CookieContainer(), Encoding.Default, out status);

            Console.Write(result);
            Console.ReadKey();
        }
    }
}