using AutoSignIn.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AutoSignIn
{
    class Program

    {
        static void Main(string[] args)
        {
            new IReader().QianDao();
            Console.WriteLine();

            new MXC().QianDao();
            Console.ReadLine();
        }
    }
}
