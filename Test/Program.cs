using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using TracySecretTool.Tools;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    new Thread(() =>
                    {
                        LogHelper.WriteLog(DateTime.Now.ToString("mm:ss.fff"));
                    }).Start();
                }
            }
            catch (Exception ex)
            {

            }
            Console.WriteLine("over");
            Console.Read();
        }
    }
}
