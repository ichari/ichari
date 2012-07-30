using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SLS.Common;
using System.Configuration;

namespace eTicket
{
    public partial class MainService : ServiceBase
    {
        private string ConnectionString = "";
        private eTickets SunLottoTask = null;

        public MainService()
        {
            InitializeComponent();

            //Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            //ConnectionString = ini.Read("Options", "ConnectionString");
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                System.Data.SqlClient.SqlConnection conn = Shove.Database.MSSQL.CreateDataConnection<System.Data.SqlClient.SqlConnection>(ConnectionString);
                while (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    new Log("System").Write("数据库连接失败");
                    System.Threading.Thread.Sleep(1000);
                }
                conn.Close();
            }
            catch (Exception e)
            {
               new Log("System").Write("MainService.OnStart Error " + e.Message);
            }
            new Log("System").Write("Database Connection Opened!");
            SunLottoTask = new eTickets(ConnectionString);
            // 电子票自动任务
            try
            {
                //run service
                SunLottoTask.Run();
                new Log("System").Write("Running SunLotto eTickets");
            }
            catch (Exception e)
            {
                new Log("System").Write("SunLotto eTickets 启动失败：" + e.Message);
            }
        }

        protected override void OnStop()
        {
            while (SunLottoTask.StateService != 0)
            {
                SunLottoTask.Exit();
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}
