using System;
using System.Collections;
using System.Web;
using Discuz.Web.UI;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Common;
using System.Text;

namespace Discuz.Web
{
    /// <summary>
    /// 标签列表页
    /// </summary>
    public partial class stats : PageBase
    {
        #region Fields
        //public Dictionary<string, int> totalstats = new Dictionary<string,int>();
        public Hashtable totalstats = new Hashtable();
        public Hashtable osstats = new Hashtable();
        public Hashtable browserstats = new Hashtable();
        public Hashtable monthstats = new Hashtable();
        public Hashtable weekstats = new Hashtable();
        public Hashtable hourstats = new Hashtable();
        public Hashtable mainstats = new Hashtable();

        public Hashtable daypostsstats = new Hashtable();
        public Hashtable monthpostsstats = new Hashtable();
        public Hashtable forumsrankstats = new Hashtable();
        public Hashtable onlinesstats = new Hashtable();
        public Hashtable postsrankstats = new Hashtable();
        public Hashtable teamstats = new Hashtable();
        public Hashtable teamcategories;
        public Hashtable teamforums;
        public Hashtable teamadmins;
        public Hashtable teammoderators;
        public Hashtable teammembers;
        public Hashtable teamavgoffdays;
        public Hashtable teamsavgthismonthposts;
        public Hashtable teamavgtotalol;
        public Hashtable teamavgthismonthol;
        public Hashtable teamavgmodactions;
        public Hashtable creditsrankstats = new Hashtable();
        public Hashtable tradestats = new Hashtable();

        public string lastupdate = string.Empty;
        public string nextupdate = string.Empty;

        public string type = string.Empty;

        #region Main
        public int members;
        public int mempost;
        public string admins;
        public int memnonpost;
        public string lastmember;
        public double mempostpercent;
        public string bestmem;
        public int bestmemposts;
        public int forums;
        public double mempostavg;
        public double postsaddavg;
        public double membersaddavg;
        public double topicreplyavg;
        public double pageviewavg;
        public ForumInfo hotforum;
        public int topics;
        public int posts;
        public string postsaddtoday;
        public string membersaddtoday;
        public string activeindex;
        public bool statstatus;
        public string monthpostsofstatsbar = string.Empty;
        public string daypostsofstatsbar = string.Empty;
        public string monthofstatsbar = string.Empty;
        public int runtime;
        #endregion

        #region Views
        public string weekofstatsbar = string.Empty;
        public string hourofstatsbar = string.Empty;
        #endregion

        #region Client
        public string browserofstatsbar = string.Empty;
        public string osofstatsbar = string.Empty;
        #endregion

        #region TopicsRank
        public string hotreplytopics;
        public string hottopics;
        #endregion

        #region PostsRank
        public string postsrank;
        public string digestpostsrank;
        public string thismonthpostsrank;
        public string todaypostsrank;
        #endregion

        #region ForumsRank
        public string topicsforumsrank;
        public string postsforumsrank;
        public string thismonthforumsrank;
        public string todayforumsrank;
        #endregion

        #region CreditsRank
        public string[] score;
        public string creditsrank;
        public string extcreditsrank1;
        public string extcreditsrank2;
        public string extcreditsrank3;
        public string extcreditsrank4;
        public string extcreditsrank5;
        public string extcreditsrank6;
        public string extcreditsrank7;
        public string extcreditsrank8;
        #endregion

        #region OnlineRank
        public string totalonlinerank;
        public string thismonthonlinerank;
        #endregion


        public int maxos = 0;
        public int maxbrowser = 0;
        public int maxmonth = 0;
        public int yearofmaxmonth = 0;
        public int monthofmaxmonth = 0;
        public int maxweek = 0;
        public string dayofmaxweek;
        public int maxhour = 0;
        public int maxhourfrom = 0;
        public int maxhourto = 0;

        public int maxmonthposts;
        public int maxdayposts;

        public int statscachelife = 120;

        Dictionary<string, string> statvars = new Dictionary<string, string>();
        #endregion

        public bool needlogin = false;

        protected override void ShowPage()
        {
            pagetitle = "统计";
            if (usergroupinfo.Allowviewstats == 0)
            {
                AddErrLine("您所在的用户组 ( <b>" + usergroupinfo.Grouptitle + "</b> ) 没有查看统计信息的权限");
                if (userid < 1)
                {
                    needlogin = true;
                }
                return;

            }


            //判断权限


            statscachelife = config.Statscachelife;
            if (statscachelife <= 0)
            {
                statscachelife = 120;
            }
            StatInfo[] stats = Stats.GetAllStats();
            statstatus = config.Statstatus == 1;
            //statstatus = false;

            //initialize
            totalstats["hits"] = 0;
            totalstats["maxmonth"] = 0;
            totalstats["guests"] = 0;
            totalstats["visitors"] = 0;


            foreach (StatInfo stat in stats)
            {
                switch (stat.Type)
                { 
                    case "total":
                        SetValue(stat, totalstats);
                        break;
                    case "os":
                        SetValue(stat, osstats);
                        if (stat.Count > maxos)
                        {
                            maxos = stat.Count;
                        }
                        break;
                    case "browser":
                        SetValue(stat, browserstats);
                        if (stat.Count > maxbrowser)
                        {
                            maxbrowser = stat.Count;
                        }
                        break;
                    case "month":
                        SetValue(stat, monthstats);
                        if (stat.Count > maxmonth)
                        {
                            maxmonth = stat.Count;
                            yearofmaxmonth = Utils.StrToInt(stat.Variable, 0) / 100;
                            monthofmaxmonth = Utils.StrToInt(stat.Variable, 0) - yearofmaxmonth * 100;
                        }
                        break;
                    case "week":
                        SetValue(stat, weekstats);
                        if (stat.Count > maxweek)
                        {
                            maxweek = stat.Count;
                            dayofmaxweek = stat.Variable;
                        }
                        break;
                    case "hour":
                        SetValue(stat, hourstats);
                        if (stat.Count > maxhour)
                        {
                            maxhour = stat.Count;
                            maxhourfrom = Utils.StrToInt(stat.Variable, 0);
                            maxhourto = maxhourfrom + 1;
                        }
                        break;
                }
            }

            StatVarInfo[] statvars = Stats.GetAllStatVars();
            foreach (StatVarInfo statvar in statvars)
            {
                if (statvar.Variable == "lastupdate" && Utils.IsNumeric(statvar.Value))
                    continue;
                switch (statvar.Type)
                {
                    case "dayposts":
                        SetValue(statvar, daypostsstats);
                        break;
                    case "creditsrank":
                        SetValue(statvar, creditsrankstats);
                        break;
                    case "forumsrank":
                        SetValue(statvar, forumsrankstats);
                        break;
                    case "postsrank":
                        SetValue(statvar, postsrankstats);
                        break;
                    case "main":
                        SetValue(statvar, mainstats);
                        break;
                    case "monthposts":
                        SetValue(statvar, monthpostsstats);
                        break;
                    case "onlines":
                        SetValue(statvar, onlinesstats);
                        break;
                    case "team":
                        SetValue(statvar, teamstats);
                        break;
                    case "trade":
                        SetValue(statvar, tradestats);
                        break;
                }
             
            }

            type = DNTRequest.GetString("type");

            if ((type == "" && !statstatus) || type == "posts")
            {
                maxmonthposts = maxdayposts = 0;
                /*
                daypostsstats["starttime"] = DateTime.Now.AddDays(-30);
                */
                //daypostsstats.Add("starttime", DateTime.Now.AddDays(-30));

                Stats.DeleteOldDayposts();

                /*
                if (!monthpostsstats.Contains("starttime"))
                { 
                    DateTime starttime = Stats.GetPostStartTime();
                    monthpostsstats["starttime"] = starttime;
                    Stats.UpdateStatVars("monthposts", "starttime", starttime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                */

                //monthposts
                monthpostsstats = Stats.GetMonthPostsStats(monthpostsstats);
                maxmonthposts = (int)monthpostsstats["maxcount"];
                monthpostsstats.Remove("maxcount");
                //dayposts
                daypostsstats = Stats.GetDayPostsStats(daypostsstats);
                maxdayposts = (int)daypostsstats["maxcount"];
                daypostsstats.Remove("maxcount");



            }



            switch (type)
            { 
                case "views":
                    GetViews();
                    break;
                case "client":
                    GetClient();
                    break;
                case "posts":
                    GetPosts();
                    break;
                case "forumsrank":
                    GetForumsRank();
                    break;
                case "topicsrank":
                    GetTopicsRank();
                    break;
                case "postsrank":
                    GetPostsRank();
                    break;
                case "creditsrank":
                    GetCreditsRank();
                    break;
                case "onlinetime":
                    GetOnlinetime();
                    break;
                case "team":
                    GetTeam();
                    break;
                case "modworks":
                    GetModWorks();
                    break;
                case "": 
                    Default();
                    break;
                default: 
                    AddErrLine("未定义操作请返回");
                    SetShowBackLink(false);
                    return;

            }
        }


        #region Helper
        
        private void SetValue(StatInfo stat, Hashtable ht)
        {
            ht[stat.Variable] = stat.Count;
        }

        private void SetValue(StatVarInfo statvar, Hashtable ht)
        {
            ht[statvar.Variable] = statvar.Value;
        }

        #endregion

        /// <summary>
        /// 基本状况
        /// </summary>
        private void Default()
        {
            lastmember = Statistics.GetStatisticsRowItem("lastusername");
            //StatVarInfo[] mainstatvars = Stats.GetStatVarsByType("main");
            foreach (string key in mainstats.Keys)
            {
                statvars[key] = mainstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Stats.UpdateStatVars("main", "lastupdate", statvars["lastupdate"]);
            }

            forums = Stats.GetForumCount();
            topics = Stats.GetTopicCount();
            posts = Stats.GetPostCount();
            members = Stats.GetMemberCount();

            //运行时间 从第一帖到现在
            if (statvars.ContainsKey("runtime"))
            {
                runtime = Utils.StrToInt(statvars["runtime"], 0);
            }
            else 
            {
                runtime = (DateTime.Now - Convert.ToDateTime(monthpostsstats["starttime"])).Days;
                //runtime = Stats.GetRuntime();
                Stats.UpdateStatVars("main", "runtime", runtime.ToString());
            }

            //今日新增帖数
            if (statvars.ContainsKey("postsaddtoday"))
            {
                postsaddtoday = statvars["postsaddtoday"];
            }
            else 
            {
                postsaddtoday = Stats.GetTodayPostCount().ToString();
                Stats.UpdateStatVars("main", "postsaddtoday", postsaddtoday);
            }

            //今日新增会员数
            if (statvars.ContainsKey("membersaddtoday"))
            {
                membersaddtoday = statvars["membersaddtoday"];
            }
            else
            {
                membersaddtoday = Stats.GetTodayNewMemberCount().ToString();
                Stats.UpdateStatVars("main", "membersaddtoday", membersaddtoday);
            }

            //管理人员数
            if (statvars.ContainsKey("admins"))
            {
                admins = statvars["admins"];
            }
            else
            {
                admins = Stats.GetAdminCount().ToString();
                Stats.UpdateStatVars("main", "admins", admins);
            }

            //未发帖会员数
            if (statvars.ContainsKey("memnonpost"))
            {
                memnonpost = Utils.StrToInt(statvars["memnonpost"], 0);
            }
            else
            {
                memnonpost = Stats.GetNonPostMemCount();
                Stats.UpdateStatVars("main", "memnonpost", memnonpost.ToString());
            }

            //热门论坛
            if (statvars.ContainsKey("hotforum"))
            {
                hotforum = (ForumInfo)SerializationHelper.DeSerialize(typeof(ForumInfo), statvars["hotforum"]);
            }
            else
            {
                hotforum = Stats.GetHotForum();
                Stats.UpdateStatVars("main", "hotforum", SerializationHelper.Serialize(hotforum));
            }

            //今日最佳会员及其今日帖数
            if (statvars.ContainsKey("bestmem") && statvars.ContainsKey("bestmemposts"))
            {
                bestmem = statvars["bestmem"];
                bestmemposts = Utils.StrToInt(statvars["bestmemposts"], 0);
            }
            else
            {
                Stats.GetBestMember(out bestmem, out bestmemposts);

                Stats.UpdateStatVars("main", "bestmem", bestmem);
                Stats.UpdateStatVars("main", "bestmemposts", bestmemposts.ToString());

            }
            mempost = members - memnonpost;
            mempostavg = (double)Math.Round((double)posts / (double)members, 2);
            topicreplyavg = (double)Math.Round((double)(posts - topics) / (double)topics, 2);
            mempostpercent = (double)Math.Round((double)(mempost * 100) / (double)members, 2);
            postsaddavg = (double)Math.Round((double)posts / (double)runtime, 2);
            membersaddavg = members / runtime;

            int visitors = Utils.StrToInt(totalstats["members"], 0) + Utils.StrToInt(totalstats["guests"], 0);
            totalstats["visitors"] = visitors;
            pageviewavg = (double)Math.Round((double)Utils.StrToInt(totalstats["hits"], 0) / (double)(visitors == 0 ? 1 : visitors), 2);
            activeindex = ((Math.Round(membersaddavg /(double) (members == 0 ? 1 : members), 2) + Math.Round(postsaddavg /(double) (posts == 0 ? 1 : posts), 2)) * 1500.00 + topicreplyavg * 10.00 + mempostavg + Math.Round(mempostpercent / 10.00, 2) + pageviewavg).ToString();

            if (statstatus)
            {
                monthofstatsbar = Stats.GetStatsDataHtml("month", monthstats, maxmonth);
            }
            else
            {
                monthpostsofstatsbar = Stats.GetStatsDataHtml("monthposts", monthpostsstats, maxmonthposts);
                daypostsofstatsbar = Stats.GetStatsDataHtml("dayposts", daypostsstats, maxdayposts);
            }

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");

            
        }

        /// <summary>
        /// 管理统计
        /// </summary>
        private void GetModWorks()
        {
        }

        /// <summary>
        /// 管理团队
        /// </summary>
        private void GetTeam()
        {
            foreach (string key in teamstats.Keys)
            {
                statvars[key] = teamstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Stats.UpdateStatVars("team", "lastupdate", statvars["lastupdate"]);
            }



            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");

        }

        /// <summary>
        /// 在线时间
        /// </summary>
        private void GetOnlinetime()
        {
            if (config.Oltimespan == 0)
            {
                totalonlinerank = "<li>未开启在线时长统计</li>";
                thismonthonlinerank = "<li></li>";

                return;
            }

            foreach (string key in onlinesstats.Keys)
            {
                statvars[key] = onlinesstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Stats.UpdateStatVars("onlines", "lastupdate", statvars["lastupdate"]);
            }
            ShortUserInfo[] total;
            if (statvars.ContainsKey("total"))
            {
                total = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["total"]);
            }
            else
            {
                total = Stats.GetUserOnlinetime("total");
                Stats.UpdateStatVars("onlines", "total", SerializationHelper.Serialize(total));
            }

            ShortUserInfo[] thismonth;
            if (statvars.ContainsKey("thismonth"))
            {
                thismonth = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["thismonth"]);
            }
            else
            {
                thismonth = Stats.GetUserOnlinetime("thismonth");
                Stats.UpdateStatVars("onlines", "thismonth", SerializationHelper.Serialize(thismonth));
            }

            int maxrows = Math.Max(total.Length, thismonth.Length);

            totalonlinerank = Stats.GetUserRankHtml(total, "onlinetime", maxrows);
            thismonthonlinerank = Stats.GetUserRankHtml(thismonth, "onlinetime", maxrows);

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");

        }

        /// <summary>
        /// 信用记录
        /// </summary>
        private void GetCreditsRank()
        {
            score = Scoresets.GetValidScoreName();
            foreach (string key in creditsrankstats.Keys)
            {
                statvars[key] = creditsrankstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Stats.UpdateStatVars("creditsrank", "lastupdate", statvars["lastupdate"]);
            }

            ShortUserInfo[] credits;
            ShortUserInfo[][] extendedcredits;
            if (statvars.ContainsKey("credits"))
            {
                credits = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["credits"]);
            }
            else
            {
                credits = Stats.GetUserArray("credits");
                Stats.UpdateStatVars("creditsrank", "credits", SerializationHelper.Serialize(credits));
            }

            if (statvars.ContainsKey("extendedcredits"))
            {
                extendedcredits = (ShortUserInfo[][])SerializationHelper.DeSerialize(typeof(ShortUserInfo[][]), statvars["extendedcredits"]);
            }
            else
            {
                extendedcredits = Stats.GetExtsRankUserArray();
                Stats.UpdateStatVars("creditsrank", "extendedcredits", SerializationHelper.Serialize(extendedcredits));
            }

            int maxrows = 0;
            maxrows = Math.Max(credits.Length, maxrows);
            maxrows = Math.Max(extendedcredits[0].Length, maxrows);
            maxrows = Math.Max(extendedcredits[1].Length, maxrows);
            maxrows = Math.Max(extendedcredits[2].Length, maxrows);
            maxrows = Math.Max(extendedcredits[3].Length, maxrows);
            maxrows = Math.Max(extendedcredits[4].Length, maxrows);
            maxrows = Math.Max(extendedcredits[5].Length, maxrows);
            maxrows = Math.Max(extendedcredits[6].Length, maxrows);
            maxrows = Math.Max(extendedcredits[7].Length, maxrows);

            creditsrank = Stats.GetUserRankHtml(credits, "credits", maxrows);
            extcreditsrank1 = Stats.GetUserRankHtml(extendedcredits[0], "extcredits1", maxrows);
            extcreditsrank2 = Stats.GetUserRankHtml(extendedcredits[1], "extcredits2", maxrows);
            extcreditsrank3 = Stats.GetUserRankHtml(extendedcredits[2], "extcredits3", maxrows);
            extcreditsrank4 = Stats.GetUserRankHtml(extendedcredits[3], "extcredits4", maxrows);
            extcreditsrank5 = Stats.GetUserRankHtml(extendedcredits[4], "extcredits5", maxrows);
            extcreditsrank6 = Stats.GetUserRankHtml(extendedcredits[5], "extcredits6", maxrows);
            extcreditsrank7 = Stats.GetUserRankHtml(extendedcredits[6], "extcredits7", maxrows);
            extcreditsrank8 = Stats.GetUserRankHtml(extendedcredits[7], "extcredits8", maxrows);


            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");



        }

        /// <summary>
        /// 发帖排行
        /// </summary>
        private void GetPostsRank()
        {
            foreach (string key in postsrankstats.Keys)
            {
                statvars[key] = postsrankstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Stats.UpdateStatVars("postsrank", "lastupdate", statvars["lastupdate"]);
            }

            ShortUserInfo[] posts;
            ShortUserInfo[] digestposts;
            ShortUserInfo[] thismonth;
            ShortUserInfo[] today;

            if (statvars.ContainsKey("posts"))
            {
                posts = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["posts"]);
            }
            else
            {
                posts = Stats.GetUserArray("posts");
                Stats.UpdateStatVars("postsrank", "posts", SerializationHelper.Serialize(posts));
            }

            if (statvars.ContainsKey("digestposts"))
            {
                digestposts = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["digestposts"]);
            }
            else
            {
                digestposts = Stats.GetUserArray("digestposts");
                Stats.UpdateStatVars("postsrank", "digestposts", SerializationHelper.Serialize(digestposts));
            }

            if (statvars.ContainsKey("thismonth"))
            {
                thismonth = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["thismonth"]);
            }
            else
            {
                thismonth = Stats.GetUserArray("thismonth");
                Stats.UpdateStatVars("postsrank", "thismonth", SerializationHelper.Serialize(thismonth));
            }

            if (statvars.ContainsKey("today"))
            {
                today = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["today"]);
            }
            else
            {
                today = Stats.GetUserArray("today");
                Stats.UpdateStatVars("postsrank", "today", SerializationHelper.Serialize(today));
            }

            int maxrows = 0;
            maxrows = Math.Max(posts.Length, maxrows);
            maxrows = Math.Max(digestposts.Length, maxrows);
            maxrows = Math.Max(thismonth.Length, maxrows);
            maxrows = Math.Max(today.Length, maxrows);

            postsrank = Stats.GetUserRankHtml(posts, "posts", maxrows);
            digestpostsrank = Stats.GetUserRankHtml(digestposts, "digestposts", maxrows);
            thismonthpostsrank = Stats.GetUserRankHtml(thismonth, "thismonth", maxrows);
            todaypostsrank = Stats.GetUserRankHtml(today, "today", maxrows);

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 主题排行
        /// </summary>
        private void GetTopicsRank()
        {
            hottopics = Stats.GetHotTopicsHtml();
            hotreplytopics = Stats.GetHotReplyTopicsHtml();
        }

        /// <summary>
        /// 板块排行
        /// </summary>
        private void GetForumsRank()
        {
            foreach (string key in forumsrankstats.Keys)
            {
                statvars[key] = forumsrankstats[key].ToString();
            }
            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Stats.UpdateStatVars("forumsrank", "lastupdate", statvars["lastupdate"]);
            }

            ForumInfo[] topics;
            ForumInfo[] posts;
            ForumInfo[] thismonth;
            ForumInfo[] today;

            int maxrows = 0;

            if (statvars.ContainsKey("topics"))
            {
                topics = (ForumInfo[])SerializationHelper.DeSerialize(typeof(ForumInfo[]), statvars["topics"]);
            }
            else
            {
                topics = Stats.GetForumArray("topics"); 
                Stats.UpdateStatVars("forumsrank", "topics", SerializationHelper.Serialize(topics));
            }

            if (statvars.ContainsKey("posts"))
            {
                posts = (ForumInfo[])SerializationHelper.DeSerialize(typeof(ForumInfo[]), statvars["posts"]);
            }
            else
            {
                posts = Stats.GetForumArray("posts"); 
                Stats.UpdateStatVars("forumsrank", "posts", SerializationHelper.Serialize(posts));
            }

            if (statvars.ContainsKey("thismonth"))
            {
                thismonth = (ForumInfo[])SerializationHelper.DeSerialize(typeof(ForumInfo[]), statvars["thismonth"]);
            }
            else
            {
                thismonth = Stats.GetForumArray("thismonth"); 
                Stats.UpdateStatVars("forumsrank", "thismonth", SerializationHelper.Serialize(thismonth));
            }

            if (statvars.ContainsKey("today"))
            {
                today = (ForumInfo[])SerializationHelper.DeSerialize(typeof(ForumInfo[]), statvars["today"]);
            }
            else
            {
                today = Stats.GetForumArray("today");
                Stats.UpdateStatVars("forumsrank", "today", SerializationHelper.Serialize(today));
            }

            maxrows = Math.Max(topics.Length, maxrows);
            maxrows = Math.Max(posts.Length, maxrows);
            maxrows = Math.Max(thismonth.Length, maxrows);
            maxrows = Math.Max(today.Length, maxrows);


            topicsforumsrank = Stats.GetForumsRankHtml(topics, "topics", maxrows);
            postsforumsrank = Stats.GetForumsRankHtml(posts, "posts", maxrows);
            thismonthforumsrank = Stats.GetForumsRankHtml(thismonth, "thismonth", maxrows);
            todayforumsrank = Stats.GetForumsRankHtml(today, "today", maxrows);

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");

        }

        /// <summary>
        /// 发帖量记录
        /// </summary>
        private void GetPosts()
        {
            monthpostsofstatsbar = Stats.GetStatsDataHtml("monthposts", monthpostsstats, maxmonthposts);
            daypostsofstatsbar = Stats.GetStatsDataHtml("dayposts", daypostsstats, maxdayposts);
        }

        /// <summary>
        /// 客户软件
        /// </summary>
        private void GetClient()
        {
            if (!statstatus)
                return;
            browserofstatsbar = Stats.GetStatsDataHtml("browser", browserstats, maxbrowser);
            osofstatsbar = Stats.GetStatsDataHtml("os", osstats, maxos);
        }

        /// <summary>
        /// 流量统计
        /// </summary>
        private void GetViews()
        {
            if (!statstatus)
                return;
            weekofstatsbar = Stats.GetStatsDataHtml("week", weekstats, maxweek);
            hourofstatsbar = Stats.GetStatsDataHtml("hour", hourstats, maxhour);
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:46.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:46. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<!--header end-->\r\n");
            templateBuilder.Append("	<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("		<div id=\"nav\">\r\n");
            templateBuilder.Append("			<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"stats.aspx\">统计</a>  &raquo; <strong>\r\n");

            if (type == "")
            {

                templateBuilder.Append("		基本概况\r\n");

            }
            else if (type == "views")
            {

                templateBuilder.Append("		流量统计\r\n");

            }
            else if (type == "client")
            {

                templateBuilder.Append("		客户软件\r\n");

            }
            else if (type == "posts")
            {

                templateBuilder.Append("		发帖量记录\r\n");

            }
            else if (type == "forumsrank")
            {

                templateBuilder.Append("		版块排行\r\n");

            }
            else if (type == "topicsrank")
            {

                templateBuilder.Append("		主题排行\r\n");

            }
            else if (type == "postsrank")
            {

                templateBuilder.Append("		发帖排行\r\n");

            }
            else if (type == "creditsrank")
            {

                templateBuilder.Append("		金币排行\r\n");

            }
            else if (type == "onlinetime")
            {

                templateBuilder.Append("		在线时间\r\n");

            }
            else if (type == "trade")
            {

                templateBuilder.Append("		交易排行\r\n");

            }
            else if (type == "team")
            {

                templateBuilder.Append("		管理团队\r\n");

            }
            else if (type == "modworks")
            {

                templateBuilder.Append("		管理统计\r\n");

            }	//end if

            templateBuilder.Append("</strong>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("		function changeTab(obj)\r\n");
            templateBuilder.Append("		{\r\n");
            templateBuilder.Append("			if (obj.className == 'currenttab')\r\n");
            templateBuilder.Append("			{\r\n");
            templateBuilder.Append("				obj.className = '';\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("			else\r\n");
            templateBuilder.Append("			{\r\n");
            templateBuilder.Append("				obj.className = 'currenttab';\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("		}\r\n");
            templateBuilder.Append("	</" + "script>\r\n");
            templateBuilder.Append("	<div class=\"statstab\">\r\n");
            templateBuilder.Append("		<a id=\"tab_main\" class=\"currenttab\" onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\" href=\"stats.aspx\">基本状况</a>\r\n");

            if (statstatus)
            {

                templateBuilder.Append("		<a id=\"tab_views\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=views\">流量统计</a>\r\n");
                templateBuilder.Append("		<a id=\"tab_client\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=client\">客户软件</a>\r\n");

            }	//end if

            templateBuilder.Append("		<a id=\"tab_posts\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=posts\">发帖量记录</a>\r\n");
            templateBuilder.Append("		<a id=\"tab_forumsrank\"   onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=forumsrank\">版块排行</a>\r\n");
            templateBuilder.Append("		<a id=\"tab_topicsrank\"   onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=topicsrank\">主题排行</a>\r\n");
            templateBuilder.Append("		<a id=\"tab_postsrank\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=postsrank\">发帖排行</a>\r\n");
            templateBuilder.Append("		<a id=\"tab_creditsrank\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=creditsrank\">金币排行</a>\r\n");
            templateBuilder.Append("		<!--\r\n");
            templateBuilder.Append("		<a id=\"tab_trade\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=trade\">交易排行</a>\r\n");
            templateBuilder.Append("		-->\r\n");

            if (config.Oltimespan > 0)
            {

                templateBuilder.Append("		<a id=\"tab_onlinetime\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=onlinetime\">在线时间</a>\r\n");

            }	//end if

            templateBuilder.Append("		<!--\r\n");
            templateBuilder.Append("		<a id=\"tab_team\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\"  href=\"?type=team\">管理团队</a>\r\n");
            templateBuilder.Append("		<a id=\"tab_modworks\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\" href=\"?type=modworks\">管理统计</a>\r\n");
            templateBuilder.Append("		-->\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("	try{\r\n");
            templateBuilder.Append("		$(\"tab_main\").className = \"\";\r\n");
            templateBuilder.Append("		$(\"tab_\" + '" + type.ToString() + "').className = \"currenttab\";\r\n");
            templateBuilder.Append("	}catch(e){\r\n");
            templateBuilder.Append("		$(\"tab_main\").className = \"currenttab\";\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("	</" + "script>\r\n");

            if (page_err == 0)
            {


                if (type == "")
                {

                    templateBuilder.Append("		<div class=\"mainbox viewsstats\">\r\n");
                    templateBuilder.Append("			<h3>基本状况</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">注册会员</td>\r\n");
                    templateBuilder.Append("					<td>" + members.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">发帖会员</td>\r\n");
                    templateBuilder.Append("					<td>" + mempost.ToString() + "</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">管理成员</td>\r\n");
                    templateBuilder.Append("					<td>" + admins.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">未发帖会员</td>\r\n");
                    templateBuilder.Append("					<td>" + memnonpost.ToString() + "</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">新会员</td>\r\n");
                    templateBuilder.Append("					<td>" + lastmember.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">发帖会员占总数</td>\r\n");
                    templateBuilder.Append("					<td>" + mempostpercent.ToString() + "%</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">今日论坛之星</td>\r\n");
                    templateBuilder.Append("					<td>\r\n");

                    if (bestmem != "")
                    {

                        templateBuilder.Append("<a href=\"userinfo.aspx?username=" + bestmem.ToString() + "\">" + bestmem.ToString() + "</a>(" + bestmemposts.ToString() + ")\r\n");

                    }	//end if

                    templateBuilder.Append("</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">平均每人发帖数</td>\r\n");
                    templateBuilder.Append("					<td>" + mempostavg.ToString() + "</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>	\r\n");
                    templateBuilder.Append("		<div class=\"mainbox viewsstats\">\r\n");
                    templateBuilder.Append("			<h3>论坛统计</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">版块数</td>\r\n");
                    templateBuilder.Append("					<td style=\"width:15%\">" + forums.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">平均每日新增帖子数</td>\r\n");
                    templateBuilder.Append("					<td style=\"width:15%\">" + postsaddavg.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">最热门版块</td>\r\n");
                    templateBuilder.Append("					<td><a href=\"" + ShowForumAspxRewrite(hotforum.Fid, 0).ToString().Trim() + "\" target=\"_blank\">" + hotforum.Name.ToString().Trim() + "</a></td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">主题数</td>\r\n");
                    templateBuilder.Append("					<td>" + topics.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">平均每日注册会员数</td>\r\n");
                    templateBuilder.Append("					<td>" + membersaddavg.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">主题数</td>\r\n");
                    templateBuilder.Append("					<td>" + hotforum.Topics.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">帖子数</td>\r\n");
                    templateBuilder.Append("					<td>" + posts.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">最近24小时新增帖子数</td>\r\n");
                    templateBuilder.Append("					<td>" + postsaddtoday.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">帖子数</td>\r\n");
                    templateBuilder.Append("					<td>" + hotforum.Posts.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">平均每个主题被回复次数</td>\r\n");
                    templateBuilder.Append("					<td>" + topicreplyavg.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">今日新增会员数</td>\r\n");
                    templateBuilder.Append("					<td>" + membersaddtoday.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"statsitem\">论坛活跃指数</td>\r\n");
                    templateBuilder.Append("					<td>" + activeindex.ToString() + "</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                    if (statstatus)
                    {

                        templateBuilder.Append("		<div class=\"mainbox viewsstats\">\r\n");
                        templateBuilder.Append("			<h3>流量概况</h3>\r\n");
                        templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">总页面流量</td>\r\n");
                        templateBuilder.Append("					<td>" + totalstats["hits"].ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">访问量最多的月份</td>\r\n");
                        templateBuilder.Append("					<td>" + yearofmaxmonth.ToString() + " 年 " + monthofmaxmonth.ToString() + " 月</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">共计来访</td>\r\n");
                        templateBuilder.Append("					<td>" + totalstats["visitors"].ToString().Trim() + " 人次</td>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">月份总页面流量</td>\r\n");
                        templateBuilder.Append("					<td>" + maxmonth.ToString() + "</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">会员</td>\r\n");
                        templateBuilder.Append("					<td>" + totalstats["members"].ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">时段</td>\r\n");
                        templateBuilder.Append("					<td>" + maxhourfrom.ToString() + " - " + maxhourto.ToString() + "</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">游客</td>\r\n");
                        templateBuilder.Append("					<td>" + totalstats["guests"].ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">时段总页面流量</td>\r\n");
                        templateBuilder.Append("					<td>" + maxhour.ToString() + "</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">平均每人浏览</td>\r\n");
                        templateBuilder.Append("					<td>" + pageviewavg.ToString() + "</td>\r\n");
                        templateBuilder.Append("					<td class=\"statsitem\">&nbsp;</td>\r\n");
                        templateBuilder.Append("					<td>&nbsp;</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</table>\r\n");
                        templateBuilder.Append("		</div>\r\n");

                    }	//end if

                    templateBuilder.Append("		<div class=\"mainbox viewsstats\">\r\n");
                    templateBuilder.Append("			<h3>月份流量</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");

                    if (statstatus)
                    {

                        templateBuilder.Append("					" + monthofstatsbar.ToString() + "\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("					<thead>\r\n");
                        templateBuilder.Append("						<td colspan=\"2\">每月新增帖子记录</td>\r\n");
                        templateBuilder.Append("					</thead>\r\n");
                        templateBuilder.Append("					" + monthpostsofstatsbar.ToString() + "\r\n");
                        templateBuilder.Append("					<thead>\r\n");
                        templateBuilder.Append("						<td colspan=\"2\">每日新增帖子记录</td>\r\n");
                        templateBuilder.Append("					</thead>\r\n");
                        templateBuilder.Append("					" + daypostsofstatsbar.ToString() + "\r\n");

                    }	//end if

                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "views")
                {

                    templateBuilder.Append("		<div class=\"mainbox viewsstats\">\r\n");
                    templateBuilder.Append("			<h3>流量统计</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td colspan=\"2\">星期流量</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				" + weekofstatsbar.ToString() + "\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td colspan=\"2\">时段流量</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				" + hourofstatsbar.ToString() + "\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "client")
                {

                    templateBuilder.Append("		<div class=\"mainbox viewsstats\">\r\n");
                    templateBuilder.Append("			<h3>客户软件</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td colspan=\"2\">操作系统</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				" + osofstatsbar.ToString() + "\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td colspan=\"2\">浏览器</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				" + browserofstatsbar.ToString() + "\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "posts")
                {

                    templateBuilder.Append("		<div class=\"mainbox viewsstats\">\r\n");
                    templateBuilder.Append("			<h3>发帖量记录</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td colspan=\"2\">每月新增帖子记录</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				" + monthpostsofstatsbar.ToString() + "\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td colspan=\"2\">每日新增帖子记录</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				" + daypostsofstatsbar.ToString() + "\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "forumsrank")
                {

                    templateBuilder.Append("		<div class=\"mainbox topicstats\">\r\n");
                    templateBuilder.Append("			<h3>版块排行</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">发帖 排行榜</td>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">回复 排行榜</td>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">最近 30 天发帖 排行榜</td>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">最近 24 小时发帖 排行榜</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td><ul>" + topicsforumsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + postsforumsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + thismonthforumsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + todayforumsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "topicsrank")
                {

                    templateBuilder.Append("		<div class=\"mainbox topicstats\">\r\n");
                    templateBuilder.Append("			<h3>主题排行</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td width=\"50%\">被浏览最多的主题</td>\r\n");
                    templateBuilder.Append("						<td>被回复最多的主题</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td><ul>" + hottopics.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + hotreplytopics.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "postsrank")
                {

                    templateBuilder.Append("		<div class=\"mainbox topicstats\">\r\n");
                    templateBuilder.Append("			<h3>发帖排行</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">发帖 排行榜</td>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">精华帖 排行榜</td>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">最近 30 天发帖 排行榜</td>\r\n");
                    templateBuilder.Append("						<td width=\"25%\">最近 24 小时发帖 排行榜</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td><ul>" + postsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + digestpostsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + thismonthpostsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + todaypostsrank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "creditsrank")
                {

                    templateBuilder.Append("		<div class=\"mainbox topicstats\">\r\n");
                    templateBuilder.Append("			<h3>金币排行</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    //templateBuilder.Append("						<td>金币 排行榜</td>\r\n");

                    if (score[1].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[1].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if


                    if (score[2].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[2].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if


                    if (score[3].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[3].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if


                    if (score[4].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[4].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if


                    if (score[5].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[5].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if


                    if (score[6].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[6].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if


                    if (score[7].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[7].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if


                    if (score[8].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td>" + score[8].ToString().Trim() + " 排行榜</td>\r\n");

                    }	//end if

                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    //templateBuilder.Append("						<td><ul>" + creditsrank.ToString() + "</ul></td>\r\n");

                    if (score[1].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank1.ToString() + "</ul></td>\r\n");

                    }	//end if


                    if (score[2].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank2.ToString() + "</ul></td>\r\n");

                    }	//end if


                    if (score[3].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank3.ToString() + "</ul></td>\r\n");

                    }	//end if


                    if (score[4].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank4.ToString() + "</ul></td>\r\n");

                    }	//end if


                    if (score[5].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank5.ToString() + "</ul></td>\r\n");

                    }	//end if


                    if (score[6].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank6.ToString() + "</ul></td>\r\n");

                    }	//end if


                    if (score[7].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank7.ToString() + "</ul></td>\r\n");

                    }	//end if


                    if (score[8].ToString().Trim() != "")
                    {

                        templateBuilder.Append("							<td><ul>" + extcreditsrank8.ToString() + "</ul></td>\r\n");

                    }	//end if

                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (type == "onlinetime")
                {

                    templateBuilder.Append("		<div class=\"mainbox topicstats\">\r\n");
                    templateBuilder.Append("			<h3>主题排行</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<thead>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td width=\"50%\">总在线时间排行(小时)</td>\r\n");
                    templateBuilder.Append("						<td>本月在线时间排行(小时)</td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</thead>\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("					<tr>\r\n");
                    templateBuilder.Append("						<td><ul>" + totalonlinerank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("						<td><ul>" + thismonthonlinerank.ToString() + "</ul></td>\r\n");
                    templateBuilder.Append("					</tr>\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


                if (lastupdate != "" && nextupdate != "")
                {

                    templateBuilder.Append("		<div class=\"hintinfo notice\">统计数据已被缓存，上次于 " + lastupdate.ToString() + " 被更新，下次将于 " + nextupdate.ToString() + " 进行更新</div>\r\n");

                }	//end if


            }
            else
            {


                if (needlogin)
                {


                    templateBuilder.Append("<div class=\"box message\">\r\n");
                    templateBuilder.Append("	<h1>" + config.Forumtitle.ToString().Trim() + " 提示信息</h1>\r\n");
                    templateBuilder.Append("	<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n");
                    templateBuilder.Append("	<p><b>" + msgbox_text.ToString() + "</b></p>\r\n");
                    templateBuilder.Append("	<p>您还没有登录，请填写下面的登录表单后再尝试访问。</p>\r\n");
                    templateBuilder.Append("	<form id=\"formlogin\" name=\"formlogin\" method=\"post\" action=\"login.aspx\" onsubmit=\"submitLogin(this);\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" value=\"2592000\" name=\"cookietime\"/>\r\n");
                    templateBuilder.Append("	<div class=\"box\" style=\"margin: 10px auto; width: 60%;\">\r\n");
                    templateBuilder.Append("		<table cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">\r\n");
                    templateBuilder.Append("		<thead>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td colspan=\"2\">会员登录</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</thead>\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td>用户名</td>\r\n");
                    templateBuilder.Append("				<td><input type=\"text\" id=\"username\" name=\"username\" size=\"25\" maxlength=\"40\" tabindex=\"2\" />  <a href=\"register.aspx\" tabindex=\"-1\" accesskey=\"r\" title=\"立即注册 (ALT + R)\">立即注册</a>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td>密码</td>\r\n");
                    templateBuilder.Append("				<td><input type=\"password\" name=\"password\" size=\"25\" tabindex=\"3\" /> <a href=\"getpassword.aspx\" tabindex=\"-1\" accesskey=\"g\" title=\"忘记密码 (ALT + G)\">忘记密码</a>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");

                    if (config.Secques == 1)
                    {

                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<td>安全问题</td>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<select name=\"questionid\" tabindex=\"4\">\r\n");
                        templateBuilder.Append("					<option value=\"0\">&nbsp;</option>\r\n");
                        templateBuilder.Append("					<option value=\"1\">母亲的名字</option>\r\n");
                        templateBuilder.Append("					<option value=\"2\">爷爷的名字</option>\r\n");
                        templateBuilder.Append("					<option value=\"3\">父亲出生的城市</option>\r\n");
                        templateBuilder.Append("					<option value=\"4\">您其中一位老师的名字</option>\r\n");
                        templateBuilder.Append("					<option value=\"5\">您个人计算机的型号</option>\r\n");
                        templateBuilder.Append("					<option value=\"6\">您最喜欢的餐馆名称</option>\r\n");
                        templateBuilder.Append("					<option value=\"7\">驾驶执照的最后四位数字</option>\r\n");
                        templateBuilder.Append("					</select>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<td>答案</td>\r\n");
                        templateBuilder.Append("				<td><input type=\"text\" name=\"answer\" size=\"25\" tabindex=\"5\" /></td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");

                    }	//end if

                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td>&nbsp;</td>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<button class=\"submit\" type=\"submit\" name=\"loginsubmit\" id=\"loginsubmit\" value=\"true\" tabindex=\"6\">会员登录</button>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		</table>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	</form>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("	document.getElementById(\"username\").focus();\r\n");
                    templateBuilder.Append("	function submitLogin(loginForm)\r\n");
                    templateBuilder.Append("	{\r\n");
                    templateBuilder.Append("		loginForm.action = 'login.aspx?loginsubmit=true&reurl=' + escape(window.location);\r\n");
                    templateBuilder.Append("		loginForm.submit();\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("</" + "script>\r\n");
                    templateBuilder.Append("</div>\r\n");



                }
                else
                {


                    templateBuilder.Append("<div class=\"box message\">\r\n");
                    templateBuilder.Append("	<h1>出现了" + page_err.ToString() + "个错误</h1>\r\n");
                    templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");
                    templateBuilder.Append("	<p class=\"errorback\">\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("			if(" + msgbox_showbacklink.ToString() + ")\r\n");
                    templateBuilder.Append("			{\r\n");
                    templateBuilder.Append("				document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n");
                    templateBuilder.Append("			}\r\n");
                    templateBuilder.Append("		</" + "script>\r\n");
                    templateBuilder.Append("		<a href=\"forumindex.aspx\">论坛首页</a>\r\n");

                    if (usergroupid == 7)
                    {

                        templateBuilder.Append("		 &nbsp; &nbsp|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

                    }	//end if

                    templateBuilder.Append("	</p>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("</div>\r\n");



                }	//end if


            }	//end if

            templateBuilder.Append("	</div>\r\n");


            if (footerad != "")
            {

                templateBuilder.Append("<!--底部广告显示-->\r\n");
                templateBuilder.Append("<div id=\"ad_footerbanner\">" + footerad.ToString() + "</div>\r\n");
                templateBuilder.Append("<!--底部广告结束-->\r\n");

            }	//end if

            templateBuilder.Append(Discuz.Web.UI.PageElement.Bottom);
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"stats_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx\">基本状况</a></li>\r\n");

            if (config.Statstatus == 1)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=views\">流量统计</a></li>\r\n");
                templateBuilder.Append("	<li><a href=\"stats.aspx?type=client\">客户软件</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<li><a href=\"stats.aspx?type=posts\">发帖量记录</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=forumsrank\">版块排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=topicsrank\">主题排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=postsrank\">发帖排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=creditsrank\">金币排行</a></li>\r\n");

            if (config.Oltimespan > 0)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=onlinetime\">在线时间</a></li>\r\n");

            }	//end if

            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"my_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"mytopics.aspx\">我的主题</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myposts.aspx\">我的帖子</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"search.aspx?posterid=" + userid.ToString() + "&amp;type=digest\">我的精华</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myattachment.aspx\">我的附件</a></li>\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("	<li><a href=\"usercpsubscribe.aspx\">我的收藏</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/mymenu.js\"></" + "script>\r\n");
            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup\" id=\"viewpro_menu\" style=\"display: none\">\r\n");

            if (useravatar != "")
            {

                templateBuilder.Append("		<img src=\"" + useravatar.ToString() + "\"/>\r\n");

            }	//end if

            aspxrewriteurl = this.UserInfoAspxRewrite(userid);

            templateBuilder.Append("	<li class=\"popuser\"><a href=\"" + aspxrewriteurl.ToString() + "\">我的资料</a></li>\r\n");


            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<div id=\"quicksearch_menu\" class=\"searchmenu\" style=\"display: none;\">\r\n");
            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='0';document.getElementById('quicksearch').innerHTML='帖子标题';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">帖子标题</div>\r\n");

            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='8';document.getElementById('quicksearch').innerHTML='作&nbsp;&nbsp;者';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">作&nbsp;&nbsp;者</div>\r\n");
            templateBuilder.Append("</div>\r\n");



            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }

    }
}
