using MessageBomber.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageBomber
{
    class Program
    {
        static void Main(string[] args)
        {
            string phone = "13163736313";

            //Console.WriteLine(new MeiTuan().SendMsg(phone));


            string msg = "";
            int i = 0;
            var _796 = new _796JiaoYiSuo();
            if (_796.CallPhone(phone))
            {
                i++;
                msg += "796电话\n";
            }
            if (_796.SendMsg(phone))
            {
                i++;
                msg += "796短信\n";
            }

            if (new ChinaUnicom().SendMsg(phone))
            {
                i++;
                msg += "联通短信\n";
            }

            if (new DuoZhuan().SendMsg(phone))
            {
                i++;
                msg += "多赚短信\n";
            }
            
            if (new LianZhongYouXi().SendMsg(phone))
            {
                i++;
                msg += "联众短信\n";
            }

            if (new MeiTuan().SendMsg(phone))
            {
                i++;
                msg += "美团短信\n";
            }

            Console.WriteLine(i + "次成功，成功的有：");
            Console.WriteLine(msg);
            Console.ReadKey();
        }
    }
}
