using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Data;
using Discuz.Entity;
using System.Data;
using System.Collections;
using Discuz.Common;
using System.Web;

namespace Discuz.Forum
{
    public class Stats
    {
        private Stats() { }

        #region Public Methods
        /// <summary>
        /// 更新特定统计
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="count">count增加量</param>
        public static void UpdateStats(string type, string variable, int count)
        {
            DatabaseProvider.GetInstance().UpdateStats(type, variable, count);
        }

        /// <summary>
        /// 更新特定统计
        /// </summary>
        /// <param name="statinfo"></param>
        public static void UpdateStats(StatInfo statinfo)
        {
            UpdateStats(statinfo.Type, statinfo.Variable, statinfo.Count);
        }

        /// <summary>
        /// 更新特定统计
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        public static void UpdateStatVars(string type, string variable, string value)
        {
            DatabaseProvider.GetInstance().UpdateStatVars(type, variable, value);
        }

        /// <summary>
        /// 更新特定统计
        /// </summary>
        /// <param name="statvarinfo"></param>
        public static void UpdateStatVars(StatVarInfo statvarinfo)
        {
            UpdateStatVars(statvarinfo.Type, statvarinfo.Variable, statvarinfo.Value);
        }

        /// <summary>
        /// 获取所有统计数据
        /// </summary>
        /// <returns></returns>
        public static StatInfo[] GetAllStats()
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetAllStats();
#if NET1
            ArrayList list = new ArrayList();
#else
            List<StatInfo> list = new List<StatInfo>();
#endif
            while (reader.Read())
            {
                StatInfo statinfo = LoadSingleStat(reader);

                list.Add(statinfo);
            }
            reader.Close();
#if NET1
            return (StatInfo[])list.ToArray(typeof(StatInfo));
#else
            return list.ToArray();
#endif
        }

        /// <summary>
        /// 获取所有统计数据
        /// </summary>
        /// <returns></returns>
        public static StatVarInfo[] GetAllStatVars()
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetAllStatVars();
#if NET1
            ArrayList list = new ArrayList();
#else
            List<StatVarInfo> list = new List<StatVarInfo>();
#endif
            while (reader.Read())
            {
                StatVarInfo statvarinfo = LoadSingleStatVar(reader);

                list.Add(statvarinfo);
            }
            reader.Close();
#if NET1
            return (statvarinfo[])list.ToArray(typeof(statvarinfo));
#else
            return list.ToArray();
#endif
        }

        /// <summary>
        /// 获取指定类型统计数据
        /// </summary>
        /// <returns></returns>
        public static StatInfo[] GetStatsByType(string type)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetStatsByType(type);
#if NET1
            ArrayList list = new ArrayList();
#else
            List<StatInfo> list = new List<StatInfo>();
#endif
            while (reader.Read())
            {
                StatInfo statinfo = LoadSingleStat(reader);

                list.Add(statinfo);
            }
            reader.Close();
#if NET1
            return (StatInfo[])list.ToArray(typeof(StatInfo));
#else
            return list.ToArray();
#endif
        }

        /// <summary>
        /// 获取指定统计数据
        /// </summary>
        /// <returns></returns>
        public static StatVarInfo[] GetStatVarsByType(string type)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetStatVarsByType(type);
#if NET1
            ArrayList list = new ArrayList();
#else
            List<StatVarInfo> list = new List<StatVarInfo>();
#endif
            while (reader.Read())
            {
                StatVarInfo statvarinfo = LoadSingleStatVar(reader);

                list.Add(statvarinfo);
            }
            reader.Close();
#if NET1
            return (statvarinfo[])list.ToArray(typeof(statvarinfo));
#else
            return list.ToArray();
#endif
        }

        /// <summary>
        /// 线程更新统计信息
        /// </summary>
        /// <param name="isguest">是否游客</param>
        public static void UpdateStatCount(bool isguest, bool sessionexists)
        {
            string browser = GetClientBrower();
            string os = GetClientOS();
            string visitorsadd = string.Empty;
            if (!sessionexists)
            {
                visitorsadd = "OR ([type]='browser' AND [variable]='" + browser + "') OR ([type]='os' AND [variable]='" + os + "')"
                + (isguest ? " OR ([type]='total' AND [variable]='guests')" : " OR ([type]='total' AND [variable]='members')");
            }
            ProcessStats ps = new ProcessStats(browser, os, visitorsadd);
            ps.Enqueue();
        }


        /// <summary>
        /// 删除过期的日发帖记录
        /// </summary>
        public static void DeleteOldDayposts()
        {
            DatabaseProvider.GetInstance().DeleteOldDayposts();
        }

        /// <summary>
        /// 获得最早帖子时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetPostStartTime()
        {
            return DatabaseProvider.GetInstance().GetPostStartTime();
        }

        /// <summary>
        /// 获得板块总数
        /// </summary>
        /// <returns></returns>
        public static int GetForumCount()
        {
            return DatabaseProvider.GetInstance().GetForumCount();
        }

        /// <summary>
        /// 获得主题总数
        /// </summary>
        /// <returns></returns>
        public static int GetTopicCount()
        {
            return Utils.StrToInt(Statistics.GetStatisticsRowItem("totaltopic"), 0);
        }

        /// <summary>
        /// 获得帖子总数
        /// </summary>
        /// <returns></returns>
        public static int GetPostCount()
        {
            return Utils.StrToInt(Statistics.GetStatisticsRowItem("totalpost"), 0);
        }

        /// <summary>
        /// 获得会员总数
        /// </summary>
        /// <returns></returns>
        public static int GetMemberCount()
        {
            return Utils.StrToInt(Statistics.GetStatisticsRowItem("totalusers"), 0);
        }

        /// <summary>
        /// 获得今日发帖数
        /// </summary>
        /// <returns></returns>
        public static int GetTodayPostCount()
        {
            return DatabaseProvider.GetInstance().GetTodayPostCount(Posts.GetPostTableID());
        }

        /// <summary>
        /// 获得今日新会员数
        /// </summary>
        /// <returns></returns>
        public static int GetTodayNewMemberCount()
        {
            return DatabaseProvider.GetInstance().GetTodayNewMemberCount();
        }

        /// <summary>
        /// 获得管理员数
        /// </summary>
        /// <returns></returns>
        public static int GetAdminCount()
        {
            return DatabaseProvider.GetInstance().GetAdminCount();
        }

        /// <summary>
        /// 获得未发帖会员数
        /// </summary>
        /// <returns></returns>
        public static int GetNonPostMemCount()
        {
            return DatabaseProvider.GetInstance().GetNonPostMemCount();
        }

        /// <summary>
        /// 获得最热论坛
        /// </summary>
        /// <returns></returns>
        public static ForumInfo GetHotForum()
        {
            ForumInfo forum = null;
            int posts = 0;
            foreach (ForumInfo f in Forums.GetForumList())
            {
                if (f.Layer > 0 && f.Status > 0 && f.Posts > posts)
                {
                    posts = f.Posts;
                    forum = f;
                }
            }

            if (posts > 0)
                return forum;
            foreach (ForumInfo f in Forums.GetForumList())
            {
                if (f.Layer > 0 && f.Status > 0)
                {
                    return f;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得今日最佳用户
        /// </summary>
        /// <param name="bestmem"></param>
        /// <param name="bestmemposts"></param>
        public static void GetBestMember(out string bestmem, out int bestmemposts)
        {
            bestmem = "";
            bestmemposts = 0;
            IDataReader reader = DatabaseProvider.GetInstance().GetBestMember(Posts.GetPostTableID());
            if (reader.Read())
            {
                bestmem = reader["poster"].ToString();
                bestmemposts = Utils.StrToInt(reader["posts"], 0);
            }
            reader.Close();

        }

        /// <summary>
        /// 返回运行天数
        /// </summary>
        /// <returns></returns>
        public static int GetRuntime()
        {
            DateTime firstdate = DatabaseProvider.GetInstance().GetPostStartTime();
            return (int)(DateTime.Now - firstdate).TotalDays;
        }

        /// <summary>
        /// 获得每月发帖统计
        /// </summary>
        /// <param name="monthpostsstats"></param>
        /// <returns></returns>
        public static Hashtable GetMonthPostsStats(Hashtable monthpostsstats)
        {
            Hashtable ht = new Hashtable();
            IDataReader reader = DatabaseProvider.GetInstance().GetMonthPostsStats(Posts.GetPostTableID());
            while (reader.Read())
            {
                string key = reader["year"].ToString() + Utils.StrToInt(reader["month"], 1).ToString("00");
                int count = Utils.StrToInt(reader["count"], 0);
                if (!monthpostsstats.ContainsKey(key) || monthpostsstats[key].ToString() != count.ToString())
                {
                    monthpostsstats[key] = count.ToString();
                    ht[key] = count;
                }
                
            }
            reader.Close();

            foreach (string key in ht.Keys)
            {
                UpdateStatVars("monthposts", key, ht[key].ToString());
            }

            ArrayList list = new ArrayList(monthpostsstats.Values);
            list.Sort(new StatVarSorter());
            monthpostsstats["maxcount"] = list.Count > 0 ? Utils.StrToInt(list[list.Count - 1], 0) : 0;

            return monthpostsstats;
        }

        /// <summary>
        /// 获得每日发帖统计
        /// </summary>
        /// <param name="daypostsstats"></param>
        /// <returns></returns>
        public static Hashtable GetDayPostsStats(Hashtable daypostsstats)
        {
            Hashtable ht = new Hashtable();
            IDataReader reader = DatabaseProvider.GetInstance().GetDayPostsStats(Posts.GetPostTableID());
            while (reader.Read())
            {
                string key = reader["year"].ToString() + Utils.StrToInt(reader["month"], 1).ToString("00") + Utils.StrToInt(reader["day"], 1).ToString("00");
                int count = Utils.StrToInt(reader["count"], 0);
                if (!daypostsstats.ContainsKey(key) || daypostsstats[key].ToString() != count.ToString())
                {
                    daypostsstats[key] = count.ToString();
                    ht[key] = count;
                }

            }
            reader.Close();

            foreach (string key in ht.Keys)
            {
                UpdateStatVars("dayposts", key, ht[key].ToString());
            }
            ArrayList list = new ArrayList(daypostsstats.Values);
            list.Sort(new StatVarSorter());
            daypostsstats["maxcount"] = list.Count < 1 ? 0 : Utils.StrToInt(list[list.Count - 1], 0);

            return daypostsstats;
        }

        /// <summary>
        /// 获得热门主题html
        /// </summary>
        /// <returns></returns>
        public static string GetHotTopicsHtml()
        {
            string html = string.Empty;
            IDataReader reader = DatabaseProvider.GetInstance().GetHotTopics(20);
            while (reader.Read())
            {
                html += "<li><em>" + reader["views"].ToString() + "</em><a href=\"" + Urls.ShowTopicAspxRewrite(Convert.ToInt32(reader["tid"]), 0) + "\">" + reader["title"].ToString() + "</a>\r\n";
            }
            reader.Close();
            return html;
        }

        /// <summary>
        /// 获得热门回复主题html
        /// </summary>
        /// <returns></returns>
        public static string GetHotReplyTopicsHtml()
        {
            string html = string.Empty;
            IDataReader reader = DatabaseProvider.GetInstance().GetHotReplyTopics(20);
            while (reader.Read())
            {
                html += "<li><em>" + reader["replies"].ToString() + "</em><a href=\"" + Urls.ShowTopicAspxRewrite(Convert.ToInt32(reader["tid"]), 0) + "\">" + reader["title"].ToString() + "</a>\r\n";
            }
            reader.Close();
            return html;
        }

        /// <summary>
        /// 获得板块列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ForumInfo[] GetForumArray(string type)
        {
            IDataReader reader = null;
            switch (type)
            { 
                case "topics":
                    reader = DatabaseProvider.GetInstance().GetForumsByTopicCount(20);
                    break;
                case "posts":
                    reader = DatabaseProvider.GetInstance().GetForumsByPostCount(20);
                    break;
                case "thismonth":
                    reader = DatabaseProvider.GetInstance().GetForumsByMonthPostCount(20, Posts.GetPostTableID());
                    break;
                case "today":
                    reader = DatabaseProvider.GetInstance().GetForumsByDayPostCount(20, Posts.GetPostTableID());
                    break;
            }
            if (reader == null)
                return new ForumInfo[0];
#if NET1
            ArrayList list = new ArrayList();
#else
            List<ForumInfo> list = new List<ForumInfo>();
#endif
            while (reader.Read())
            {
                ForumInfo f = new ForumInfo();
                f.Fid = Utils.StrToInt(reader["fid"], 0);
                f.Name = reader["name"].ToString();
                if (type == "topics")
                    f.Topics = Utils.StrToInt(reader["topics"], 0);
                else
                    f.Posts = Utils.StrToInt(reader["posts"], 0);

                list.Add(f);
            }
            reader.Close();
#if NET1
            return (ForumInfo[])list.ToArray(typeof(ForumInfo));
#else
            return list.ToArray();
#endif

        }

        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ShortUserInfo[] GetUserArray(string type)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUsersRank(20, Posts.GetPostTableID(), type);
            if (reader == null)
                return new ShortUserInfo[0];
#if NET1
            ArrayList list = new ArrayList();
#else
            List<ShortUserInfo> list = new List<ShortUserInfo>();
#endif
            while (reader.Read())
            {
                ShortUserInfo u = new ShortUserInfo();
                u.Username = reader["username"].ToString();
                u.Uid = Utils.StrToInt(reader["uid"], 0);
                if (type == "digestposts")
                    u.Digestposts = Utils.StrToInt(reader["digestposts"], 0);
                else if (type == "credits")
                    u.Credits = Utils.StrToInt(reader["credits"], 0);
                else
                {
                    switch (type)
                    {
                        case "extcredits1":
                            u.Extcredits1 = Utils.StrToFloat(reader["extcredits1"], 0);
                            break;
                        case "extcredits2":
                            u.Extcredits2 = Utils.StrToFloat(reader["extcredits2"], 0);
                            break;
                        case "extcredits3":
                            u.Extcredits3 = Utils.StrToFloat(reader["extcredits3"], 0);
                            break;
                        case "extcredits4":
                            u.Extcredits4 = Utils.StrToFloat(reader["extcredits4"], 0);
                            break;
                        case "extcredits5":
                            u.Extcredits5 = Utils.StrToFloat(reader["extcredits5"], 0);
                            break;
                        case "extcredits6":
                            u.Extcredits6 = Utils.StrToFloat(reader["extcredits6"], 0);
                            break;
                        case "extcredits7":
                            u.Extcredits7 = Utils.StrToFloat(reader["extcredits7"], 0);
                            break;
                        case "extcredits8":
                            u.Extcredits8 = Utils.StrToFloat(reader["extcredits8"], 0);
                            break;
                        case "oltime":
                            u.Oltime = Utils.StrToInt(reader["oltime"], 0);
                            break;
                        default:
                            u.Posts = Utils.StrToInt(reader["posts"], 0);
                            break;
                    }
                }
                u.Password = string.Empty;
                u.Secques = string.Empty;
                u.Nickname = string.Empty;

                list.Add(u);
            }
            reader.Close();
#if NET1
            return (ShortUserInfo[])list.ToArray(typeof(ShortUserInfo));
#else
            return list.ToArray();
#endif

        }

        /// <summary>
        /// 获得扩展积分排行数组
        /// </summary>
        /// <returns></returns>
        public static ShortUserInfo[][] GetExtsRankUserArray()
        {
#if NET1
            ArrayList list = new ArrayList();
#else
            List<ShortUserInfo[]> list = new List<ShortUserInfo[]>();
#endif
            string[] score = Scoresets.GetValidScoreName();

            for (int i = 1; i < 9; i++)
            {
                if (score[i] == string.Empty)
                    list.Add(new ShortUserInfo[0]);
                else
                    list.Add(GetUserArray("extcredits" + i.ToString()));
            }

#if NET1
            return (ShortUserInfo[][])list.ToArray(typeof(ShortUserInfo[]));
#else
            return list.ToArray();
#endif
        }


        public static ShortUserInfo[] GetUserOnlinetime(string field)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserByOnlineTime(field);
            if (reader == null)
                return new ShortUserInfo[0];
#if NET1
            ArrayList list = new ArrayList();
#else
            List<ShortUserInfo> list = new List<ShortUserInfo>();
#endif
            while (reader.Read())
            {
                ShortUserInfo u = new ShortUserInfo();
                u.Username = reader["username"].ToString();
                u.Uid = Utils.StrToInt(reader["uid"], 0);
                u.Oltime = Utils.StrToInt(reader[field], 0);
                u.Password = string.Empty;
                u.Secques = string.Empty;
                u.Nickname = string.Empty;

                list.Add(u);

            }
            reader.Close();
#if NET1
            return (ShortUserInfo[])list.ToArray(typeof(ShortUserInfo));
#else
            return list.ToArray();
#endif
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 获得浏览器信息
        /// </summary>
        /// <returns></returns>
        private static string GetClientBrower()
        {
            string browser = string.Empty;
            string agent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (agent == null)
                return "Other";

            if (agent.IndexOf("Netscape") > -1)
                browser = "Netscape";
            else if (agent.IndexOf("Lynx") > -1)
                browser = "Lynx";
            else if (agent.IndexOf("Opera") > -1)
                browser = "Opera";
            else if (agent.IndexOf("Konqueror") > -1)
                browser = "Konqueror";
            else if (agent.IndexOf("MSIE") > -1)
                browser = "MSIE";
            else if (agent.IndexOf("Safari") > -1)
                browser = "Safari";
            else if (agent.IndexOf("Firefox") > -1)
                browser = "Firefox";
            else if (agent.Substring(0, 7) == "Mozilla")
                browser = "Mozilla";
            else
                browser = "Other";
            return browser;
        }

        /// <summary>
        /// 获得操作系统信息
        /// </summary>
        /// <returns></returns>
        private static string GetClientOS()
        {
            string os = string.Empty;
            string agent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (agent == null)
                return "Other";

            if (agent.IndexOf("Win") > -1)
                os = "Windows";
            else if (agent.IndexOf("Mac") > -1)
                os = "Mac";
            else if (agent.IndexOf("Linux") > -1)
                os = "Linux";
            else if (agent.IndexOf("FreeBSD") > -1)
                os = "FreeBSD";
            else if (agent.IndexOf("SunOS") > -1)
                os = "SunOS";
            else if (agent.IndexOf("OS/2") > -1)
                os = "OS/2";
            else if (agent.IndexOf("AIX") > -1)
                os = "AIX";
            else if (System.Text.RegularExpressions.Regex.IsMatch(agent, @"(Bot|Crawl|Spider)"))
                os = "Spiders";
            else
                os = "Other";
            return os;
        }
        #endregion

        #region DataHelper
        private static StatInfo LoadSingleStat(IDataReader reader)
        {
            StatInfo statinfo = new StatInfo();
            statinfo.Type = reader["type"].ToString().Trim();
            statinfo.Variable = reader["variable"].ToString().Trim();
            statinfo.Count = Utils.StrToInt(reader["count"], 0);
            return statinfo;
        }

        private static StatVarInfo LoadSingleStatVar(IDataReader reader)
        {
            StatVarInfo statvarinfo = new StatVarInfo();
            statvarinfo.Type = reader["type"].ToString().Trim();
            statvarinfo.Variable = reader["variable"].ToString().Trim();
            statvarinfo.Value = reader["value"].ToString().Trim();
            return statvarinfo;
        }
        #endregion

        #region Helper
        /// <summary>
        /// 获得统计数据html通用方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="statht"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string GetStatsDataHtml(string type, Hashtable statht, int max)
        {
            string statsbar = string.Empty;
            int sum = 0;

            ArrayList list;

            if (type == "os" || type == "browser")
            {
                ArrayList al = new ArrayList(statht);

                al.Sort(new StatSorter());
                al.Reverse();
                list = new ArrayList();
                foreach (DictionaryEntry de in al)
                {
                    list.Add(de.Key);
                }
            }
            else
            {
                list = new ArrayList(statht.Keys);
                list.Sort();
            }
            foreach (string key in list)
            {
                sum += Utils.StrToInt(statht[key], 0);
            }

            foreach (string key in list)
            {
                string variable = "";
                int count = 0;
                switch (type)
                {
                    case "month":
                    case "monthposts":
                        if (key.Length < 4)
                            break;
                        variable = key.Substring(0, 4) + "-" + key.Substring(4);
                        break;
                    case "dayposts":
                        variable = key.Substring(0, 4) + "-" + key.Substring(4, 2) + "-" + key.Substring(6);
                        break;
                    case "week":
                        switch (key)
                        {
                            case "0": variable = "星期日"; break;
                            case "1": variable = "星期一"; break;
                            case "2": variable = "星期二"; break;
                            case "3": variable = "星期三"; break;
                            case "4": variable = "星期四"; break;
                            case "5": variable = "星期五"; break;
                            case "6": variable = "星期六"; break;
                            default: continue;
                        }

                        break;
                    case "hour":
                        variable = key;
                        break;
                    default:
                        variable = "<img src='images/stats/" + key.Replace("/", "") + ".gif ' border='0' alt='" + key + "' title='" + key + "' />&nbsp;" + key;
                        break;
                }
                count = Utils.StrToInt(statht[key], 0);
                int width = (int)(370 * (max == 0 ? 0.0 : ((double)count / (double)max)));
                double percent = ((double)Math.Round((double)count * 100 / (double)(sum == 0 ? 1 : sum), 2));
                if (width <= 0)
                    width = 2;
                variable = count == max ? "<strong>" + variable + "</strong>" : variable;
                string countstr = "<div class='optionbar'><div style='width: " + width.ToString() + "px'>&nbsp;</div></div>&nbsp;<strong>" + count.ToString() + "</strong> (" + percent + "%)";
                statsbar += "<tr><th width=\"100\">" + variable + "</th><td>" + countstr + "</td></tr>\r\n";

            }
            return statsbar;
        }

        /// <summary>
        /// 获得板块排行的html
        /// </summary>
        /// <param name="forums"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetForumsRankHtml(ForumInfo[] forums, string type, int maxrows)
        {
            StringBuilder builder = new StringBuilder();
            int blankrows = maxrows;
            foreach (ForumInfo f in forums)
            {
                builder.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", type == "topics" ? f.Topics : f.Posts, Urls.ShowForumAspxRewrite(f.Fid, 0), f.Name);
                blankrows--;
            }
            for (int i = 0; i < blankrows; i++)
            {
                builder.Append("<li>&nbsp;</li>");
            }

            return builder.ToString();

        }

        /// <summary>
        /// 获得用户排行的html
        /// </summary>
        /// <param name="users"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUserRankHtml(ShortUserInfo[] users, string type, int maxrows)
        {
            StringBuilder builder = new StringBuilder();
            string unit = "";
            int blankrows = maxrows;
            foreach (ShortUserInfo u in users)
            {
                string count = string.Empty;
                switch (type)
                {
                    case "credits":
                        count = u.Credits.ToString();
                        break;
                    case "extcredits1":
                        count = u.Extcredits1.ToString();
                        unit = Scoresets.GetValidScoreUnit()[1];
                        break;
                    case "extcredits2":
                        count = u.Extcredits2.ToString();
                        unit = Scoresets.GetValidScoreUnit()[2];
                        break;
                    case "extcredits3":
                        count = u.Extcredits3.ToString();
                        unit = Scoresets.GetValidScoreUnit()[3];
                        break;
                    case "extcredits4":
                        count = u.Extcredits4.ToString();
                        unit = Scoresets.GetValidScoreUnit()[4];
                        break;
                    case "extcredits5":
                        count = u.Extcredits5.ToString();
                        unit = Scoresets.GetValidScoreUnit()[5];
                        break;
                    case "extcredits6":
                        count = u.Extcredits6.ToString();
                        unit = Scoresets.GetValidScoreUnit()[6];
                        break;
                    case "extcredits7":
                        count = u.Extcredits7.ToString();
                        unit = Scoresets.GetValidScoreUnit()[7];
                        break;
                    case "extcredits8":
                        count = u.Extcredits8.ToString();
                        unit = Scoresets.GetValidScoreUnit()[8];
                        break;
                    case "digestposts":
                        count = u.Digestposts.ToString();
                        break;
                    case "onlinetime":
                        count = Math.Round(((double)u.Oltime) / 60, 2).ToString();
                        unit = "小时";
                        break;
                    default:
                        count = u.Posts.ToString();
                        break;
                }


                builder.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", count + (unit == string.Empty ? string.Empty : " " + unit), Urls.UserInfoAspxRewrite(u.Uid), u.Username);
                blankrows--;
            }
            for (int i = 0; i < blankrows; i++)
            {
                builder.Append("<li>&nbsp;</li>");
            }
            return builder.ToString();
        }

        #endregion

        private class ProcessStats
        {
            public ProcessStats(string browser, string os, string visitorsadd)
            {
                _browser = browser;
                _os = os;
                _visitorsadd = visitorsadd;
            }
            protected string _browser;
            protected string _os;
            protected string _visitorsadd;

            /// <summary>
            /// 执行统计操作
            /// </summary>
            public void Enqueue()
            {
                ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
            }

            /// <summary>
            /// 处理当前操作
            /// </summary>
            /// <param name="state"></param>
            private void Process(object state)
            {
                DatabaseProvider.GetInstance().UpdateStatCount(this._browser, this._os, this._visitorsadd);
            }
        }

    }

    /// <summary>
    /// Stat排序类
    /// </summary>
    class StatSorter : IComparer
    {
        #region IComparer 成员

        public int Compare(object x, object y)
        {
            return (new CaseInsensitiveComparer().Compare(((DictionaryEntry)x).Value, ((DictionaryEntry)y).Value));
        }

        #endregion

    }

    /// <summary>
    /// StatVar排序类
    /// </summary>
    class StatVarSorter : IComparer
    {
        #region IComparer 成员

        public int Compare(object x, object y)
        {
            return new CaseInsensitiveComparer().Compare(Utils.StrToInt(x, 0), Utils.StrToInt(y, 0));
        }

        #endregion

    }

}
