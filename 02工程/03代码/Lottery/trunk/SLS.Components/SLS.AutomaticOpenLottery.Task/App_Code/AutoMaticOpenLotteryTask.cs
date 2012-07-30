using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

using Shove.Database;

namespace SLS.AutomaticOpenLottery.Task
{
    /// <summary>
    ///GetLotteryOpenNumberTask 的摘要说明
    /// </summary>
    public class AutoMaticOpenLotteryTask
    {
        private long gCount1 = 0;

        private SqlConnection conn;
        private static Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");

        private int TimeGap = Shove._Convert.StrToInt(ini.Read("Options", "TimeGap"), 45);
        private string AutomaticOpenLottery = ini.Read("Options", "AutomaticOpenLottery");

        private string CQSSC_Open_URL = ini.Read("Options", "CQSSC_Open_URL");
        private string SHSSL_Open_URL = ini.Read("Options", "SHSSL_Open_URL");
        private string JX11X5_Open_URL = ini.Read("Options", "JX11X5_Open_URL");
        private string JXSSC_Open_URL = ini.Read("Options", "JXSSC_Open_URL");
        private string SYYDJ_Open_URL = ini.Read("Options", "SYYDJ_Open_URL");
        private string FC3D_Open_URL = ini.Read("Options", "FC3D_Open_URL");
        private string SZPL3_Open_URL = ini.Read("Options", "SZPL3_Open_URL");
        private string SZPL5_Open_URL = ini.Read("Options", "SZPL5_Open_URL");

        private string GP_Open_RulSSC = ini.Read("Options", "GP_Open_RulSSC");
        private string GP_Open_RulSSL = ini.Read("Options", "GP_Open_RulSSL");
        private string GP_Open_RulJXSSC = ini.Read("Options", "GP_Open_RulJXSSC");
        private string GP_Open_RulJX11X5 = ini.Read("Options", "GP_Open_RulJX11X5");
        private string GP_Open_RulSYYDJ = ini.Read("Options", "GP_Open_RulSYYDJ");
        private string GP_Open_RulFC3D = ini.Read("Options", "GP_Open_RulFC3D");
        private string GP_Open_RulSZPL3 = ini.Read("Options", "GP_Open_RulSZPL3");
        private string GP_Open_RulSZPL5 = ini.Read("Options", "GP_Open_RulSZPL5");

        private System.Threading.Thread thread;

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public AutoMaticOpenLotteryTask(string connectionstring)
        {
            conn = new SqlConnection(connectionstring);
        }

        public void Run()
        {
            // 已经启动
            if (State == 1)
            {
                return;
            }

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                State = 1;

                gCount1 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                new Log("System").Write("AutoMaticOpenLottery_Task.");
            }
        }

        public void Exit()
        {
            State = 2;
        }

        public void Do()
        {
            while (true)
            {
                if (State == 2)
                {
                    State = 0;
                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                #region Config.ini 配置时间单位：秒, 取一次开奖号码并开奖,默认设置为45秒

                if (gCount1 >= 45)
                {
                    gCount1 = 0;

                    try
                    {
                        //抓取开奖号码并开奖
                        GetLotteryOpenNumber();
                        new Log("System").Write("GetLotteryOpenNumber success");

                        //Open();

                        new Log("System").Write("Open success");
                    }
                    catch (Exception e)
                    {
                        new Log("System").Write("GetLotteryOpenNumber Fail:" + e.Message);
                    }

                    try
                    {
                        SystemEndSchemePrintOut();

                        new Log("System").Write("SystemEndSchemePrintOut success");
                    }
                    catch (Exception e)
                    {
                        new Log("System").Write("SystemEndSchemePrintOut Fail:" + e.Message);
                    }
                }

                #endregion
              
            }
        }

        private void Stop()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        #region 定时器执行的事件

        private void GetLotteryOpenNumber()	//获取开奖号码
        {
            //查询自动开奖的彩种的期号
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            if ((AutomaticOpenLottery == null) || (AutomaticOpenLottery == ""))
            {
                new Log("SystemGetLotteryOpenNumber").Write("没有可自动开奖的彩种");

                return;
            }

            DataTable dt = null;
            try
            {
                dt = t_Isuses.Open(conn, "distinct LotteryID", "LotteryID in (" + AutomaticOpenLottery + ") and LotteryID not in (28, 61) and IsOpened = 0 and EndTime < Getdate() and DAY(EndTime) = DAY(GETDATE()) and month(getdate()) = MONTH(StartTime) and YEAR(GETDATE()) = YEAR(StartTime) and isnull(WinLotteryNumber, '') = ''", "");
            }
            catch (Exception e)
            {
                new Log("SystemGetLotteryOpenNumber").Write(e.Message);
                return;
            }

            if (dt == null)
            {
                new Log("SystemGetLotteryOpenNumber").Write("数据读取错误." + AutomaticOpenLottery);

                return;
            }

            int LotteryID = -1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LotteryID = Shove._Convert.StrToInt(dt.Rows[i]["LotteryID"].ToString(), -1);

                new Log("SystemGetLotteryOpenNumber").Write("开奖的彩种" + LotteryID.ToString());

                switch (LotteryID)
                {
                    case 6:
                        try
                        {
                            GetLotteryOpenNumberForFC3D(LotteryID);
                        }
                        catch (Exception EM)
                        {
                            new Log("SystemGetLotteryOpenNumber").Write("开奖的彩种" + LotteryID.ToString() + ",  " + EM.Message);
                            break;
                        }
                        break;
                        
                    case 29:
                        try
                        {
                            GetLotteryOpenNumberForSHSSL(LotteryID);
                        }
                        catch (Exception EM)
                        {
                            new Log("SystemGetLotteryOpenNumber").Write("开奖的彩种" + LotteryID.ToString() + ",  " + EM.Message);
                            break;
                        }
                        break;

                    case 62:
                        try
                        {
                            GetLotteryOpenNumberForSYYDJ(LotteryID);
                        }
                        catch (Exception EM)
                        {
                            new Log("SystemGetLotteryOpenNumber").Write("开奖的彩种" + LotteryID.ToString() + ",  " + EM.Message);
                            break;
                        }
                        break;

                    case 63:
                        try
                        {
                            GetLotteryOpenNumberForSZPL3(LotteryID);
                        }
                        catch (Exception EM)
                        {
                            new Log("SystemGetLotteryOpenNumber").Write("开奖的彩种" + LotteryID.ToString() + ",  " + EM.Message);
                            break;
                        }
                        break;

                    case 64:
                        try
                        {
                            GetLotteryOpenNumberForSZPL5(LotteryID);
                        }
                        catch (Exception EM)
                        {
                            new Log("SystemGetLotteryOpenNumber").Write("开奖的彩种" + LotteryID.ToString() + ",  " + EM.Message);
                            break;
                        }
                        break;

                    case 70:
                        try
                        {
                            GetLotteryOpenNumberForJX11X5(LotteryID);
                        }
                        catch (Exception EM)
                        {
                            new Log("SystemGetLotteryOpenNumber").Write("开奖的彩种" + LotteryID.ToString() + ",  " + EM.Message);
                            break;
                        }
                        break;
                }
            }
        }

        //福彩3D
        private void GetLotteryOpenNumberForFC3D(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(FC3D_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("福彩3D获取开奖号码页面异常");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("福彩3D获取开奖号码页面异常");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName = dr["expect"].ToString();

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString().Replace(",","");

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;

                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulFC3D == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("福彩3D获取开奖号码异常：" + e.Message);
                }
            }
        }

        //重庆时时彩
        private void GetLotteryOpenNumberForCQSSC(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(CQSSC_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("重庆时时彩获取开奖号码页面异常1");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("重庆时时彩获取开奖号码页面异常2");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName = "20" + dr["expect"].ToString().Insert(dr["expect"].ToString().Length - 3, "-");

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString().Replace(",","");

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;
                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulSSC == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("重庆时时彩获取开奖号码异常：" + e.Message);
                }
            }
        }

        //江西时时彩
        private void GetLotteryOpenNumberForJXSSC(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(JXSSC_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("江西时时彩获取开奖号码页面异常");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("江西时时彩获取开奖号码页面异常");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName = dr["expect"].ToString().Substring(4);

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString().Replace(",", "");

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;
                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulSYYDJ == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("江西时时彩获取开奖号码异常：" + e.Message);
                }
            }
        }

        //上海时时乐
        private void GetLotteryOpenNumberForSHSSL(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(SHSSL_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("上海时时乐获取开奖号码页面异常");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("上海时时乐获取开奖号码页面异常");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName = dr["expect"].ToString().Replace("-", "");

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString();

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;
                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulSSL == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("上海时时乐获取开奖号码异常：" + e.Message);
                }
            }
        }

        //十一运夺金
        private void GetLotteryOpenNumberForSYYDJ(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(SYYDJ_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("十一运夺金获取开奖号码页面异常");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("十一运夺金获取开奖号码页面异常");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName = dr["expect"].ToString();

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString().Replace(","," ");

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;
                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulSYYDJ == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("十一运夺金获取开奖号码异常：" + e.Message);
                }
            }
        }

        //数字排列三
        private void GetLotteryOpenNumberForSZPL3(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(SZPL3_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("数字排列三获取开奖号码页面异常");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("数字排列三获取开奖号码页面异常");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName =  "20" + dr["expect"].ToString();

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString().Replace(",","").Substring(0,3);

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;
                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulSZPL3 == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("数字排列三获取开奖号码异常：" + e.Message);
                }
            }
        }

        //数字排列五
        private void GetLotteryOpenNumberForSZPL5(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(SZPL5_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("数字排列五获取开奖号码页面异常");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("数字排列五获取开奖号码页面异常");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName = "20" + dr["expect"].ToString();

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString().Replace(",","");

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;
                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulSZPL5 == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("数字排列五获取开奖号码异常：" + e.Message);
                }
            }
        }

        //江西十一选五
        private void GetLotteryOpenNumberForJX11X5(int LotteryID)
        {
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml(JX11X5_Open_URL);
            }
            catch
            {
                new Log("SystemGetLotteryOpenNumber").Write("江西11选5获取开奖号码页面异常");

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[0].Rows.Count < 1))
            {
                new Log("SystemGetLotteryOpenNumber").Write("江西11选5获取开奖号码页面异常");

                return;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //期数
                string t_IsuseName = dr["expect"].ToString();

                //开奖号码
                string t_winLotteryNumber = dr["opencode"].ToString().Replace(","," ");

                if (t_Isuses.GetCount(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }

                try
                {
                    t_Isuses.WinLotteryNumber.Value = t_winLotteryNumber;
                    t_Isuses.Update(conn, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + t_IsuseName + "' and isnull(WinLotteryNumber, '') = '' and year(StartTime) = YEAR(GETDATE())");

                    if (GP_Open_RulJX11X5 == "1")
                    {
                        //开奖
                        DrawingLottery(LotteryID, t_IsuseName, t_winLotteryNumber);
                    }
                }
                catch (Exception e)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("江西11选5获取开奖号码异常：" + e.Message);
                }
            }
        }

        //开奖派奖
        private void DrawingLottery(int LotteryID, string IsuseName, string WinNumber)
        {
            Log log = new Log("SystemGetLotteryOpenNumber");

            if (!new SLS.Lottery()[LotteryID].AnalyseWinNumber(WinNumber))
            {
                log.Write("开奖号码不正确！");

                return;
            }

            int ReturnValue = 0;
            string ReturnDescription = "";

            DataTable dtIsuse = new DAL.Tables.T_Isuses().Open(conn, "top 1 [ID], IsOpened", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + IsuseName + "' and IsOpened=0 and year(StartTime) = YEAR(GETDATE())", "");
            
            if (dtIsuse == null)
            {
                log.Write("数据读写错误001");

                return;
            }

            if (dtIsuse.Rows.Count <= 0)
            {
                //log.Write("暂无对应期号信息，彩种ID：" + LotteryID.ToString() + "， 期号：" + IsuseName);

                return;
            }

            if (Shove._Convert.StrToBool(dtIsuse.Rows[0]["IsOpened"].ToString(), false))
            {
                //log.Write("彩种ID：" + LotteryID + "第" + IsuseName + "期已开奖");

                return;
            }

            long IsuseID = Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), -1);

            DataTable dtWin = null;
            
            dtWin = new DAL.Tables.T_Schemes().Open(conn, "ID", "IsuseID = " + IsuseID.ToString() + " and isOpened = 0", "");

            if (dtWin == null)
            {
                log.Write("数据读写错误002");

                return;
            }

            // 准备开奖，开奖之前，对出票不完整的方案进行出票处理
            ReturnValue = 0;
            ReturnDescription = "";

            DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open(conn, "[DefaultMoney],[DefaultMoneyNoWithTax]", "LotteryID = " + LotteryID.ToString(), "[Order]");

            if (dtWinTypes == null)
            {
                log.Write("奖金读取数据读写错误");

                return;
            }

            double[] WinMoneyList = new double[dtWinTypes.Rows.Count * 2];

            for (int y = 0; y < dtWinTypes.Rows.Count; y++)
            {
                WinMoneyList[y * 2] = Shove._Convert.StrToDouble(dtWinTypes.Rows[y]["DefaultMoney"].ToString(), 0);
                WinMoneyList[y * 2 + 1] = Shove._Convert.StrToDouble(dtWinTypes.Rows[y]["DefaultMoneyNoWithTax"].ToString(), 0);

                if (WinMoneyList[y * 2] < 0)
                {
                    log.Write("第 " + (y + 1).ToString() + " 项奖金输入错误！");

                    return;
                }
            }

            dtWin = null;

            #region 开奖第一步

            dtWin = new DAL.Tables.T_Schemes().Open(conn, "LotteryNumber,PlayTypeID,Multiple,ID", "isOpened = 0 and IsuseID = " + IsuseID.ToString(), "[ID]");

            if (dtWin == null)
            {
                log.Write("方案数据读取错误");

                return;
            }

            for (int y = 0; y < dtWin.Rows.Count; y++)
            {
                string LotteryNumber = "";

                try
                {
                    LotteryNumber = dtWin.Rows[y]["LotteryNumber"].ToString();
                }
                catch
                { }

                string Description = "";
                double WinMoneyNoWithTax = 0;

                try
                {

                    double WinMoney = new SLS.Lottery()[LotteryID].ComputeWin(LotteryNumber, WinNumber, ref Description, ref WinMoneyNoWithTax, int.Parse(dtWin.Rows[y]["PlayTypeID"].ToString()), WinMoneyList);

                    if (WinMoney > 0)
                    {
                        log.Write("方案ID:" + dtWin.Rows[y]["ID"].ToString() + " 中奖金额:" + WinMoney.ToString());
                    }

                    Shove.Database.MSSQL.ExecuteNonQuery(conn, "update T_Schemes set EditWinMoney = @p1, EditWinMoneyNoWithTax = @p2, WinDescription = @p3 where [ID] = " + dtWin.Rows[y]["ID"].ToString(),
                        new Shove.Database.MSSQL.Parameter("p1", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney * Shove._Convert.StrToInt(dtWin.Rows[y]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p2", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax * Shove._Convert.StrToInt(dtWin.Rows[y]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p3", SqlDbType.VarChar, 0, ParameterDirection.Input, Description));
                }
                catch
                {
                    log.Write("方案ID:" + dtWin.Rows[y]["ID"].ToString() + " 开奖号码出现错误");

                    continue;
                }
            }

            #endregion

            log.Write("开奖-----------------------------4");

            #region 开奖第三步

            string OpenAffiche = new OpenAfficheTemplates()[LotteryID];

            int SchemeCount, QuashCount, WinCount, WinNoBuyCount;
            bool isEndOpen = false;

            while (!isEndOpen)
            {
                SchemeCount = 0;
                QuashCount = 0;
                WinCount = 0;
                WinNoBuyCount = 0;
                //  总方案数，处理时撤单数，中奖数，中奖但未成功数

                ReturnValue = 0;
                ReturnDescription = "";
                DataSet dsWin = null;

                P_Win(conn, ref dsWin,
                     IsuseID,
                     WinNumber,
                     OpenAffiche,
                     1,
                     true,
                     ref SchemeCount, ref QuashCount, ref WinCount, ref WinNoBuyCount,
                     ref isEndOpen,
                     ref ReturnValue, ref ReturnDescription);

                if ((dsWin == null) || (ReturnDescription != ""))
                {
                    log.Write(ReturnDescription);

                    return;
                }

                string Message = "彩种ID：{0},开奖成功，开奖号码：{1},总方案 {2} 个，撤单未满员或未出票方案 {3} 个，有效中奖方案 {4} 个，中奖但未成功方案 {5} 个。本期开奖还未全部完成, 请继续操作第三步。";

                if (isEndOpen)
                {
                    Message = "彩种ID：{0},开奖成功，开奖号码：{1},总方案 {2} 个，撤单未满员或未出票方案 {3} 个，有效中奖方案 {4} 个，中奖但未成功方案 {5} 个。本期开奖已全部完成。";
                }

                log.Write(String.Format(Message,LotteryID,WinNumber, SchemeCount, QuashCount, WinCount, WinNoBuyCount));
            }

            #endregion

            log.Write("开奖-----------------------------5");
        }

        private int P_Win(SqlConnection conn, ref DataSet ds, long IsuseID, string WinLotteryNumber, string OpenAffiche, long OpenOperatorID, bool isEndTheIsuse, ref int SchemeCount, ref int QuashCount, ref int WinCount, ref int WinNoBuyCount, ref bool isEndOpen, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Win", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("WinLotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, WinLotteryNumber),
                new MSSQL.Parameter("OpenAffiche", SqlDbType.VarChar, 0, ParameterDirection.Input, OpenAffiche),
                new MSSQL.Parameter("OpenOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OpenOperatorID),
                new MSSQL.Parameter("isEndTheIsuse", SqlDbType.Bit, 0, ParameterDirection.Input, isEndTheIsuse),
                new MSSQL.Parameter("SchemeCount", SqlDbType.Int, 4, ParameterDirection.Output, SchemeCount),
                new MSSQL.Parameter("QuashCount", SqlDbType.Int, 4, ParameterDirection.Output, QuashCount),
                new MSSQL.Parameter("WinCount", SqlDbType.Int, 4, ParameterDirection.Output, WinCount),
                new MSSQL.Parameter("WinNoBuyCount", SqlDbType.Int, 4, ParameterDirection.Output, WinNoBuyCount),
                new MSSQL.Parameter("isEndOpen", SqlDbType.Bit, 0, ParameterDirection.Output, isEndOpen),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                SchemeCount = System.Convert.ToInt32(Outputs["SchemeCount"]);
            }
            catch { }

            try
            {
                QuashCount = System.Convert.ToInt32(Outputs["QuashCount"]);
            }
            catch { }

            try
            {
                WinCount = System.Convert.ToInt32(Outputs["WinCount"]);
            }
            catch { }

            try
            {
                WinNoBuyCount = System.Convert.ToInt32(Outputs["WinNoBuyCount"]);
            }
            catch { }

            try
            {
                isEndOpen = System.Convert.ToBoolean(Outputs["isEndOpen"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        private void Open()
        {
            //查询自动开奖的彩种的期号
            DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

            if ((AutomaticOpenLottery == null) || (AutomaticOpenLottery == ""))
            {
                new Log("SystemGetLotteryOpenNumber").Write("没有可自动开奖的彩种");

                return;
            }

            DataTable dt = null;
            try
            {
                dt = t_Isuses.Open(conn, "[ID], LotteryID, [Name], WinLotteryNumber", "LotteryID in (" + AutomaticOpenLottery + ") and IsOpened = 0 and EndTime < Getdate() and isnull(WinLotteryNumber, '') <> ''", "[EndTime] asc");
            }
            catch (Exception e)
            {
                new Log("SystemGetLotteryOpenNumber").Write(e.Message);
                return;
            }

            if (dt == null)
            {
                new Log("SystemGetLotteryOpenNumber").Write("数据读取错误." + AutomaticOpenLottery);

                return;
            }

            int LotteryID = -1;
            string IsuseName = "";
            string WinLotteryNumber = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LotteryID = Shove._Convert.StrToInt(dt.Rows[i]["LotteryID"].ToString(), -1);
                IsuseName = dt.Rows[i]["Name"].ToString();
                WinLotteryNumber = dt.Rows[i]["WinLotteryNumber"].ToString();

                try
                {
                    DrawingLottery(LotteryID, IsuseName, WinLotteryNumber);
                }
                catch(Exception EX)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("开奖有错误." + EX.Message);
                    continue;
                }
            }
        }

        private void SystemEndSchemePrintOut()
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_SystemEndSchemePrintOut(conn, ref ReturnValue, ref ReturnDescription) < 0)
            {
                new Log("SystemGetLotteryOpenNumber").Write("Exec SystemEndSchemePrintOut: Procedure \"P_SystemEndSchemePrintOut\" Fail.");

                return;
            }

            if (ReturnValue < 0)
            {
                new Log("SystemGetLotteryOpenNumber").Write("Exec SystemEndSchemePrintOut: Procedure \"P_SystemEndSchemePrintOut\" Return: " + ReturnDescription);

            }
        }
        #endregion
    }
}