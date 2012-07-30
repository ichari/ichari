using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;

namespace SLS.Task
{
    public partial class MainService : ServiceBase
    {
        private string ConnectionString = "";

        private ElectronTicket_HPSH ElectronTicket_HPSH_Task = null;
        private ElectronTicket_HPSH_GP ElectronTicket_HPSH_GP_Task = null;

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

            SystemOptions so = new SystemOptions(ConnectionString);

            // 恒朋上海电子票自动任务
            try
            {
                if (so["ElectronTicket_HPSH_Status_ON"].ToBoolean(false) && (new DAL.Tables.T_Lotteries().GetCount(ConnectionString, "PrintOutType = 102") > 0))
                {
                    ElectronTicket_HPSH_Task = new ElectronTicket_HPSH(ConnectionString);

                    ElectronTicket_HPSH_Task.ElectronTicket_HPSH_Getway = so["ElectronTicket_HPSH_Getway"].ToString("");
                    ElectronTicket_HPSH_Task.ElectronTicket_HPSH_UserName = so["ElectronTicket_HPSH_UserName"].ToString("");
                    ElectronTicket_HPSH_Task.ElectronTicket_HPSH_UserPassword = so["ElectronTicket_HPSH_UserPassword"].ToString("");
                    ElectronTicket_HPSH_Task.ElectronTicket_PrintOut_AlipayName = so["ElectronTicket_PrintOut_AlipayName"].ToString("");
                    ElectronTicket_HPSH_Task.ElectronTicket_PrintOut_IDCardNumber = so["ElectronTicket_PrintOut_IDCardNumber"].ToString("");
                    ElectronTicket_HPSH_Task.ElectronTicket_PrintOut_Mobile = so["ElectronTicket_PrintOut_Mobile"].ToString("");
                    ElectronTicket_HPSH_Task.ElectronTicket_PrintOut_RealityName = so["ElectronTicket_PrintOut_RealityName"].ToString("");
                    ElectronTicket_HPSH_Task.ElectronTicket_PrintOut_Email = so["ElectronTicket_PrintOut_Email"].ToString("");

                    ElectronTicket_HPSH_GP_Task = new ElectronTicket_HPSH_GP(ConnectionString);

                    ElectronTicket_HPSH_GP_Task.ElectronTicket_HPSH_Getway = so["ElectronTicket_HPSH_Getway"].ToString("");
                    ElectronTicket_HPSH_GP_Task.ElectronTicket_HPSH_UserName = so["ElectronTicket_HPSH_UserName"].ToString("");
                    ElectronTicket_HPSH_GP_Task.ElectronTicket_HPSH_UserPassword = so["ElectronTicket_HPSH_UserPassword"].ToString("");
                    ElectronTicket_HPSH_GP_Task.ElectronTicket_PrintOut_AlipayName = so["ElectronTicket_PrintOut_AlipayName"].ToString("");
                    ElectronTicket_HPSH_GP_Task.ElectronTicket_PrintOut_IDCardNumber = so["ElectronTicket_PrintOut_IDCardNumber"].ToString("");
                    ElectronTicket_HPSH_GP_Task.ElectronTicket_PrintOut_Mobile = so["ElectronTicket_PrintOut_Mobile"].ToString("");
                    ElectronTicket_HPSH_GP_Task.ElectronTicket_PrintOut_RealityName = so["ElectronTicket_PrintOut_RealityName"].ToString("");
                    ElectronTicket_HPSH_GP_Task.ElectronTicket_PrintOut_Email = so["ElectronTicket_PrintOut_Email"].ToString("");

                    Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                    ElectronTicket_HPSH_GP_Task.t_Datetime = ini.Read("Options", "DateTime");

                    if ((ElectronTicket_HPSH_Task.ElectronTicket_HPSH_Getway != "") && (ElectronTicket_HPSH_Task.ElectronTicket_HPSH_UserName != "") && (ElectronTicket_HPSH_Task.ElectronTicket_HPSH_UserPassword != ""))
                    {
                        ElectronTicket_HPSH_Task.Run();
                        ElectronTicket_HPSH_GP_Task.Run();
                    }
                }
            }
            catch (Exception e)
            {
                new Log("System").Write("ElectronTicket_HPSH 启动失败：" + e.Message);
            }
        }

        protected override void OnStop()
        {
            while ((ElectronTicket_HPSH_Task != null) && (ElectronTicket_HPSH_Task.StateService != 0)) { System.Threading.Thread.Sleep(500); };
        }
    }
}
