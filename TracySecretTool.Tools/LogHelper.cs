using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TracySecretTool.Tools
{
    public class LogHelper
    {
        private readonly static string Path = ConfigHelper.LogPath;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void WriteMVCLog(string url, string queryString, string formData, string msg,string trace)
        {
            string strPath = Path + "Exception/";
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            using (StreamWriter sw = File.AppendText(strPath + "/" + DateTime.Now.ToString("yyyyMMdd") + ".Log"))
            {
                sw.WriteLine("---------------------------------------------------------------------------------------");
                sw.WriteLine();
                sw.WriteLine("Time:\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("Url:\t" + url);
                sw.WriteLine("Query:\t" + queryString);
                sw.WriteLine("Form:\t" + formData);
                sw.WriteLine("Msg:\t" + msg);
                sw.WriteLine("Trace:\t" + trace);
                sw.WriteLine();
            }
        }
        public static void WriteLog(string msg)
        {
            string path = Path + "General/";
            WriteLog(msg, path);
        }
        public static void WriteGrabberLog(string msg)
        {
            string path = Path + "Grabber/";
            WriteLog(msg, path);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void WriteLog(string msg,string path)
        {
            string strPath = path;
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            using (StreamWriter sw = File.AppendText(strPath + "/" + DateTime.Now.ToString("yyyyMMdd") + ".Log"))
            {
                sw.WriteLine("---------------------------------------------------------------------------------------");
                sw.WriteLine();
                sw.WriteLine("Time:\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("Msg:\t" + msg);
                sw.WriteLine();
            }
        }
    }
}
