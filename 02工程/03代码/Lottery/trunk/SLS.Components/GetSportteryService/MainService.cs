using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Shove;
using System.Configuration;

namespace GetSportteryService
{
    partial class MainService : ServiceBase
    {

        private string ConnectionString = "";
        private string ConnectionStringInformation = "";
        private string Source = "";
        public string PathName = "";

        private int RateSpaceTime = 0;
        private int MatchSpaceTime = 0;
        private Main main = null;

        public MainService()
        {
            InitializeComponent();

            Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            try
            {
                ConnectionString = ini.Read("Config", "ConnectionString").Replace("\"","");
                ConnectionStringInformation = ini.Read("Config", "ConnectionStringInformation").Replace("\"", "");

                MatchSpaceTime = Shove._Convert.StrToInt(ini.Read("Config", "MatchSpaceTime"), 1);
                RateSpaceTime = Shove._Convert.StrToInt(ini.Read("Config", "RateSpaceTime"), 1);                  //抓取赔率间隔时间
                Source = ini.Read("Config", "Source");
                PathName = ini.Read("Config", "PathName");


            }
            catch(Exception e)
            {
                new Log("Sporttery").Write("配置文件错误:" + e.Message);

                return;
            }
        }

        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // 自动任务
            try
            {
                Clutch.ConnectionString = ConnectionString;
                Clutch.ConnectionStringInformation = ConnectionStringInformation;
                MsSql.ConnectionString = ConnectionString;
                main = new Main(ConnectionString,
                    MatchSpaceTime,
                    RateSpaceTime,
                    Source,
                    PathName);
                main.Run();
            }
            catch (Exception e)
            {
                new Log("Sporttery").Write("Main 启动失败：" + e.Message);
            }
        }

        protected override void OnStop()
        {
            if (main != null)
            {
                main.Exit();
            }

            while ((main != null) && (main.State != 0)) { System.Threading.Thread.Sleep(500); };

        }



        static void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                Exception ex = e.ExceptionObject as Exception;
                
                new Log("Sporttery").Write("失败：" + ex.Message);
            }
        }

    }
}
