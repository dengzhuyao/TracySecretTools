using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TracySecretTool.Tools
{
    public class ConfigHelper
    {
        public readonly static string ConnString = ConfigurationManager.ConnectionStrings["TracySecretTool"].ConnectionString;

        public readonly static string LogPath = ConfigurationManager.AppSettings["logPath"];
    }
}
