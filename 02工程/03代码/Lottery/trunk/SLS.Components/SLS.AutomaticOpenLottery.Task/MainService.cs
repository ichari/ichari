using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace SLS.AutomaticOpenLottery.Task
{
    public partial class MainService : ServiceBase
    {
        private string ConnectionString = "";

        private AutoMaticOpenLotteryTask AutoMaticOpenLottery_Task = null;

        private OpenTask OpenTask_Task = null;

        public MainService()
        {
            InitializeComponent();

            Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            ConnectionString = ini.Read("Options", "ConnectionString");
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
                new Log("System").Write(e.Message);
            }

            // 自动任务
            try
            {
                AutoMaticOpenLottery_Task = new AutoMaticOpenLotteryTask(ConnectionString);
                AutoMaticOpenLottery_Task.Run();
            }
            catch (Exception e)
            {
                new Log("System").Write("AutoMaticOpenLottery_Task 启动失败：" + e.Message);
            }

            try
            {
                OpenTask_Task = new OpenTask(ConnectionString);
                OpenTask_Task.Run();
            }
            catch (Exception e)
            {
                new Log("System").Write("OpenTask_Task 启动失败：" + e.Message);
            }
        }

        protected override void OnStop()
        {
            if (AutoMaticOpenLottery_Task != null)
            {
                AutoMaticOpenLottery_Task.Exit();
            }
        }
    }
}
