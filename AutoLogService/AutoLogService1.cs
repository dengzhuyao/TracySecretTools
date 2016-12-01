using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TracySecretTool.Tools;

namespace AutoLogService
{
    partial class AutoLogService1 : ServiceBase
    {
        public AutoLogService1()
        {
            base.ServiceName = "AutoLogService";
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.WriteLog("服务开启");
            System.Timers.Timer tt = new System.Timers.Timer();
            tt.Interval = 1000 * 60; //毫秒
            tt.Elapsed += new System.Timers.ElapsedEventHandler(ChkSrv);//到达间隔时执行的事件
            tt.AutoReset = true;//是否循环执行Elapsed
            tt.Enabled = true;//是否执行Elapsed
        }

        protected override void OnStop()
        {
            LogHelper.WriteLog("服务关闭");
        }

        protected override void OnPause()
        {
            LogHelper.WriteLog("服务暂停");
            base.OnPause();
        }

        protected override void OnContinue()
        {
            LogHelper.WriteLog("服务继续");
            base.OnContinue();
        }
        protected void ChkSrv(object source, System.Timers.ElapsedEventArgs e)
        {
            if (e.SignalTime.ToString("HH:mm") == "10:30")
            {
                try
                {
                    System.Timers.Timer tt = source as System.Timers.Timer;
                    tt.Enabled = false;

                    SignIn();

                    tt.Enabled = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                }
            }
        }
        private void SignIn()
        {
            Tuple<bool, string> tIReader = new AutoSignIn.Server.IReader().QianDao();
            LogHelper.WriteLog(tIReader.Item2);

            Tuple<bool, string> tMXC = new AutoSignIn.Server.MXC().QianDao();
            LogHelper.WriteLog(tMXC.Item2);
        }
    }
}
