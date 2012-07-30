using System;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

using Discuz.Cache;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Scoresets
    {

        private static object lockHelper = new object();

        /// <summary>
        /// 获得积分策略
        /// </summary>
        /// <returns>积分策略</returns>
        public static DataTable GetScoreSet()
        {
            lock (lockHelper)
            {
                DNTCache cache = DNTCache.GetCacheService();
                DataTable dt = cache.RetrieveSingleObject("/ScoreSet") as DataTable;

                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config");
                    string[] path = new string[1];
                    path[0] = AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config";
                    cache.AddSingleObject("/ScoreSet", ds.Tables[0], path);
                    return ds.Tables[0];
                }
            }
        }


        /// <summary>
        /// 获得积分策略
        /// </summary>
        /// <returns>积分策略描述</returns>
        public static UserExtcreditsInfo GetScoreSet(int extcredits)
        {
            UserExtcreditsInfo userextcreditsinfo = new UserExtcreditsInfo();
            string extcreditsname = "extcredits" + extcredits;
            DataTable dt = GetScoreSet();

            if (extcredits > 0)
            {
                userextcreditsinfo.Name = dt.Rows[0][extcreditsname].ToString();
                userextcreditsinfo.Unit = dt.Rows[1][extcreditsname].ToString();
                userextcreditsinfo.Rate = Single.Parse(dt.Rows[2][extcreditsname].ToString());
                userextcreditsinfo.Init = Single.Parse(dt.Rows[3][extcreditsname].ToString());
                userextcreditsinfo.Topic = Single.Parse(dt.Rows[4][extcreditsname].ToString());
                userextcreditsinfo.Reply = Single.Parse(dt.Rows[5][extcreditsname].ToString());
                userextcreditsinfo.Digest = Single.Parse(dt.Rows[6][extcreditsname].ToString());
                userextcreditsinfo.Upload = Single.Parse(dt.Rows[7][extcreditsname].ToString());
                userextcreditsinfo.Download = Single.Parse(dt.Rows[8][extcreditsname].ToString());
                userextcreditsinfo.Pm = Single.Parse(dt.Rows[9][extcreditsname].ToString());
                userextcreditsinfo.Search = Single.Parse(dt.Rows[10][extcreditsname].ToString());
                userextcreditsinfo.Pay = Single.Parse(dt.Rows[11][extcreditsname].ToString());
                userextcreditsinfo.Vote = Single.Parse(dt.Rows[12][extcreditsname].ToString());
            }

            return userextcreditsinfo;
        }

        /// <summary>
        /// 获得具有兑换比率的可交易积分策略
        /// </summary>
        /// <returns>兑换比率的可交易积分策略</returns>
        public static DataTable GetScorePaySet(int type)
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = type==0 ?cache.RetrieveObject("/ScorePaySet") as DataTable:cache.RetrieveObject("/ScorePaySet1") as DataTable;
            bool pass = true;
            if (dt != null)
            {
                return dt;
            }
            else
            {
                DataTable dtScoreSet = GetScoreSet();
                DataTable dtScorePaySet = new DataTable();
                dtScorePaySet.Columns.Add("id", Type.GetType("System.Int32"));
                dtScorePaySet.Columns.Add("name", Type.GetType("System.String"));
                dtScorePaySet.Columns.Add("rate", Type.GetType("System.Single"));
                for (int i = 1; i < 9; i++)
                {
                    if (type == 0)
                    {
                        pass = (dtScoreSet.Rows[0]["extcredits" + i.ToString()].ToString().Trim() != "") && (dtScoreSet.Rows[2]["extcredits" + i.ToString()].ToString() != "0");
                    }
                    else
                    {
                        pass = (dtScoreSet.Rows[0]["extcredits" + i.ToString()].ToString().Trim() != "");
   
                    
                    }
                    if (pass)
                    {
                        DataRow dr = dtScorePaySet.NewRow();
                        dr["id"] = i;
                        dr["name"] = dtScoreSet.Rows[0]["extcredits" + i.ToString()].ToString().Trim();
                        dr["rate"] = Utils.StrToFloat(dtScoreSet.Rows[2]["extcredits" + i.ToString()].ToString(), 0);
                        dtScorePaySet.Rows.Add(dr);
                    }
                }
                if (type == 0)
                {
                    cache.AddObject("/ScorePaySet", dtScorePaySet);
                }
                else
                {
                    cache.AddObject("/ScorePaySet1", dtScorePaySet);
                }
                return dtScorePaySet;
            }
        }

        /// <summary>
        /// 获取评分操作专用的积分策略
        /// </summary>
        /// <returns>分操作专用的积分策略</returns>
        public static DataTable GetRateScoreSet()
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/RateScoreSet") as DataTable;

            if (dt != null)
            {
                return dt;
            }
            else
            {
                DataTable dtScoreSet = GetScoreSet();
                DataTable dtRateScoreSet = new DataTable();
                dtRateScoreSet.Columns.Add("id", Type.GetType("System.Int32"));
                dtRateScoreSet.Columns.Add("name", Type.GetType("System.String"));
                dtRateScoreSet.Columns.Add("rate", Type.GetType("System.Single"));

                for (int i = 1; i < 9; i++)
                {
                    DataRow dr = dtRateScoreSet.NewRow();
                    dr["id"] = i;
                    dr["name"] = dtScoreSet.Rows[0]["extcredits" + i.ToString()].ToString().Trim();
                    dr["rate"] = Utils.StrToFloat(dtScoreSet.Rows[2]["extcredits" + i.ToString()].ToString(), 0);
                    dtRateScoreSet.Rows.Add(dr);
                }
                cache.AddObject("/RateScoreSet", dtRateScoreSet);
                return dtRateScoreSet;
            }
        }

        /// <summary>
        /// 返回前台可以使用的扩展字段单位
        /// </summary>
        /// <returns>返回前台可以使用的扩展字段单位</returns>
        public static string[] GetValidScoreUnit()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string[] scoreunit = cache.RetrieveObject("/ValidScoreUnit") as string[];

            if (scoreunit != null)
            {
                return scoreunit;
            }
            else
            {
                // 为了前台模板中的可读性, scoreunit中有效元素也对应extcredits1 - 8字段, score[0]无用
                scoreunit = new string[9];
                scoreunit[0] = string.Empty;
                DataTable dt = GetScoreSet();
                for (int i = 1; i < 9; i++)
                {
                    if (dt.Rows[1]["extcredits" + i.ToString()].ToString() != string.Empty)
                        scoreunit[i] = dt.Rows[1]["extcredits" + i.ToString()].ToString();
                    else
                        scoreunit[i] = string.Empty;
                }
                dt.Dispose();
                cache.AddObject("/ValidScoreUnit", scoreunit);
                return scoreunit;
            }
        }

        /// <summary>
        /// 返回前台可以使用的扩展字段名和显示名称
        /// </summary>
        /// <returns>前台可以使用的扩展字段名名和显示名称</returns>
        public static string[] GetValidScoreName()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string[] score = cache.RetrieveObject("/ValidScoreName") as string[];

            if (score != null)
            {
                return score;
            }
            else
            {
                // 为了前台模板中的可读性, score中有效元素也对应extcredits1 - 8字段, score[0]无用
                score = new string[9];
                score[0] = "";

                DataTable dt = GetScoreSet();
                for (int i = 1; i < 9; i++)
                {
                    if (dt.Rows[0]["extcredits" + i.ToString()].ToString() != "")
                    {
                        score[i] = dt.Rows[0]["extcredits" + i.ToString()].ToString();
                    }
                    else
                    {
                        score[i] = "";
                    }
                }
                dt.Dispose();
                cache.AddObject("/ValidScoreName", score);
                return score;
            }
        }


        /// <summary>
        /// 获得积分规则
        /// </summary>
        /// <returns></returns>
        public static string GetScoreCalFormula()
        {
            return GetScoreCalFormula("/config/scoreset.config");
        }

        /// <summary>
        /// 获得积分规则
        /// </summary>
        /// <param name="configFilePath">积分规则</param>
        /// <returns>积分规则</returns>
        public static string GetScoreCalFormula(string configFilePath)
        {
            DNTCache cache = DNTCache.GetCacheService();
            string formula = cache.RetrieveObject("/Scoreset/Formula") as string;

            if (formula != null)
            {
                return formula;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + configFilePath);
                formula = ds.Tables["formula"].Rows[0]["formulacontext"].ToString();
                cache.AddObject("/Scoreset/Formula", formula);
                return formula;
            }
        }

        /// <summary>
        /// 返回交易积分
        /// </summary>
        /// <returns>交易积分</returns>
        public static int GetCreditsTrans()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string creditstrans = cache.RetrieveObject("/Scoreset/CreditsTrans") as string;

            if (creditstrans != null)
            {
                return Int16.Parse(creditstrans);
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config");
                creditstrans = ds.Tables["formula"].Rows[0]["creditstrans"].ToString();
                cache.AddObject("/Scoreset/CreditsTrans", creditstrans);
                return Int16.Parse(creditstrans);
            }
        }

        /// <summary>
        /// 返回积分交易税
        /// </summary>
        /// <returns>积分交易税</returns>
        public static float GetCreditsTax()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string creditstax = cache.RetrieveObject("/Scoreset/CreditsTax") as string;

            if (creditstax != null)
            {
                return float.Parse(creditstax);
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config");
                creditstax = ds.Tables["formula"].Rows[0]["creditstax"].ToString();
                cache.AddObject("/Scoreset/CreditsTax", creditstax);
                return float.Parse(creditstax);
            }
        }

        /// <summary>
        /// 转账最低余额
        /// </summary>
        /// <returns>转账最低余额</returns>
        public static int GetTransferMinCredits()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string transfermincredits = cache.RetrieveObject("/Scoreset/TransferMinCredits") as string;

            if (transfermincredits != null)
            {
                return Convert.ToInt16(transfermincredits);
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config");
                transfermincredits = ds.Tables["formula"].Rows[0]["transfermincredits"].ToString();
                cache.AddObject("/Scoreset/TransferMinCredits", transfermincredits);
                return Convert.ToInt16(transfermincredits);
            }
        }

        /// <summary>
        /// 返回兑换最低余额
        /// </summary>
        /// <returns>兑换最低余额</returns>
        public static int GetExchangeMinCredits()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string exchangemincredits = cache.RetrieveObject("/Scoreset/ExchangeMinCredits") as string;

            if (exchangemincredits != null)
            {
                return Convert.ToInt16(exchangemincredits);
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config");
                exchangemincredits = ds.Tables["formula"].Rows[0]["exchangemincredits"].ToString();
                cache.AddObject("/Scoreset/ExchangeMinCredits", exchangemincredits);
                return Convert.ToInt16(exchangemincredits);
            }
        }

        /// <summary>
        /// 单主题最高收入
        /// </summary>
        /// <returns></returns>
        public static int GetMaxIncPerTopic()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string maxincperthread = cache.RetrieveObject("/Scoreset/MaxIncPerThread") as string;

            if (maxincperthread != null)
            {
                return Convert.ToInt16(maxincperthread);
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config");
                maxincperthread = ds.Tables["formula"].Rows[0]["maxincperthread"].ToString();
                cache.AddObject("/Scoreset/MaxIncPerThread", maxincperthread);
                return Convert.ToInt16(maxincperthread);
            }
        }


        /// <summary>
        /// 单主题最高出售时限(小时)
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChargeSpan()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string maxchargespan = cache.RetrieveObject("/Scoreset/MaxChargeSpan") as string;

            if (maxchargespan != null)
            {
                return Convert.ToInt16(maxchargespan);
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "config/scoreset.config");
                maxchargespan = ds.Tables["formula"].Rows[0]["maxchargespan"].ToString();
                cache.AddObject("/Scoreset/MaxChargeSpan", maxchargespan);
                return Convert.ToInt16(maxchargespan);
            }
        }

        /// <summary>
        /// 确认当前时间是否在指定的时间列表内
        /// </summary>
        /// <param name="timelist">一个包含多个时间段的列表(格式为hh:mm-hh:mm)</param>
        /// <param name="vtime">输出参数：符合条件的第一个是时间段</param>
        /// <returns>时间段存在则返回true,否则返回false</returns>
        public static bool BetweenTime(string timelist, out string vtime)
        {
            if (!timelist.Equals(""))
            {
                string[] enabledvisittime = Utils.SplitString(timelist, "\n");

                if (enabledvisittime.Length > 0)
                {
                    string starttime = "";
                    int s = 0;
                    string endtime = "";
                    int e = 0;
                    foreach (string visittime in enabledvisittime)
                    {
                        if (
                            Regex.IsMatch(visittime,
                                          @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])-(([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9]))$"))
                        {
                            starttime = visittime.Substring(0, visittime.IndexOf("-"));
                            s = Utils.StrDateDiffMinutes(starttime, 0);

                            endtime =
                                Utils.CutString(visittime, visittime.IndexOf("-") + 1,
                                                visittime.Length - (visittime.IndexOf("-") + 1));
                            e = Utils.StrDateDiffMinutes(endtime, 0);

                            if (DateTime.Parse(starttime) < DateTime.Parse(endtime)) //起始时间小于结束时间,认为未跨越0点
                            {
                                if (s > 0 && e < 0)
                                {
                                    vtime = visittime;
                                    return true;
                                }
                            }
                            else //起始时间大于结束时间,认为跨越0点
                            {
                                if ((s < 0 && e < 0) || (s > 0 && e > 0 && e > s))
                                {
                                    vtime = visittime;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            vtime = "";
            return false;
        }

        /// <summary>
        /// 确认当前时间是否在指定的时间列表内
        /// </summary>
        /// <param name="timelist">一个包含多个时间段的列表(格式为hh:mm-hh:mm)</param>
        /// <returns>时间段存在则返回true,否则返回false</returns>
        public static bool BetweenTime(string timelist)
        {
            string visittime = "";
            return BetweenTime(timelist, out visittime);
        }
    }
}