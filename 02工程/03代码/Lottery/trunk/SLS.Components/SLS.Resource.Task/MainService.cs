using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SLS.Resource.Task
{
    public partial class MainService : ServiceBase
    {
        private string ConnectionString = "";

        private Task task = null;
    
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
                task = new Task(ConnectionString);
                task.Run();
            }
            catch (Exception e)
            {
                new Log("System").Write("Task 启动失败：" + e.Message);
            }

        }

        protected override void OnStop()
        {
            if (task != null)
            {
                task.Exit();
            }

            while ((task != null) && (task.State != 0)) { System.Threading.Thread.Sleep(500); };
        }
    }
}
