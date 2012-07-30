using System;
using System.Web;
using System.Text;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;


namespace Discuz.Forum
{
    /// <summary>
    /// 在线用户操作类
    /// </summary>
    public class OnlineUsers
    {


        private static object SynObject = new object();

        /// <summary>
        /// 获得在线用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public static int GetOnlineAllUserCount()
        {
            int count = DatabaseProvider.GetInstance().GetOnlineAllUserCount();
            return count == 0 ? 1 : count;
        }

        /// <summary>
        /// 返回缓存中在线用户总数
        /// </summary>
        /// <returns>缓存中在线用户总数</returns>
        public static int GetCacheOnlineAllUserCount()
        {

            int count = Utils.StrToInt(Utils.GetCookie("onlineusercount"), 0);
            if (count == 0)
            {
                count = OnlineUsers.GetOnlineAllUserCount();
                Utils.WriteCookie("onlineusercount", count.ToString(), 3);
            }
            return count;

        }

        /// <summary>
        /// 清理之前的在线表记录(本方法在应用程序初始化时被调用)
        /// </summary>
        /// <returns></returns>
        public static int InitOnlineList()
        {
            return DatabaseProvider.GetInstance().CreateOnlineTable();
        }

        /// <summary>
        /// 复位在线表, 如果系统未重启, 仅是应用程序重新启动, 则不会重新创建
        /// </summary>
        /// <returns></returns>
        public static int ResetOnlineList()
        {
            try
            {
                // 取得在线表最后一条记录的tickcount字段 (因为本功能不要求特别精确)
                //int tickcount = DatabaseProvider.GetInstance().GetLastTickCount();
                // 如果距离现在系统运行时间小于10分钟
                if (System.Environment.TickCount < 600000 && System.Environment.TickCount > 0)
                {
                    return InitOnlineList();
                }
                return -1;
            }
            catch
            {
                try
                {
                    return InitOnlineList();
                }
                catch
                {
                    return -1;
                }
            }

        }

        /// <summary>
        /// 获得在线注册用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public static int GetOnlineUserCount()
        {
            return DatabaseProvider.GetInstance().GetOnlineUserCount();
        }





        #region 根据不同条件查询在线用户信息


        /// <summary>
        /// 返回用户在线列表
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <param name="totaluser">全部用户数</param>
        /// <param name="guest">游客数</param>
        /// <param name="user">登录用户数</param>
        /// <param name="invisibleuser">隐身会员数</param>
        /// <returns>用户在线列表DataTable</returns>
        public static DataTable GetForumOnlineUserList(int forumid, out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            DataTable dt = new DataTable();

            lock (SynObject)
            {
                dt = DatabaseProvider.GetInstance().GetForumOnlineUserListTable(forumid);
            }

            totaluser = dt.Rows.Count;
            // 统计游客
            DataRow[] dr = dt.Select("userid=-1");
            if (dr == null)
            {
                guest = 0;
            }
            else
            {
                guest = dr.Length;
            }
            //统计隐身用户
            dr = dt.Select("invisible=1");
            if (dr == null)
            {
                invisibleuser = 0;
            }
            else
            {
                invisibleuser = dr.Length;
            }
            //统计用户
            user = totaluser - guest;
            //返回当前版块的在线用户表
            return dt;
        }

        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="totaluser">全部用户数</param>
        /// <param name="guest">游客数</param>
        /// <param name="user">登录用户数</param>
        /// <param name="invisibleuser">隐身会员数</param>
        /// <returns>线用户列表</returns>
        public static DataTable GetOnlineUserList(int totaluser, out int guest, out int user, out int invisibleuser)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetOnlineUserListTable();

            int highestonlineusercount = Utils.StrToInt(Statistics.GetStatisticsRowItem("highestonlineusercount"), 1);
            if (totaluser > highestonlineusercount)
            {
                if (Statistics.UpdateStatistics("highestonlineusercount", totaluser) > 0)
                {
                    Statistics.UpdateStatistics("highestonlineusertime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Statistics.ReSetStatisticsCache();
                }

            }
            // 统计用户
            DataRow[] dr = dt.Select("userid>0");
            if (dr == null)
            {
                user = 0;
            }
            else
            {
                user = dr.Length;
            }
            //统计隐身用户
            dr = dt.Select("invisible=1");
            if (dr == null)
            {
                invisibleuser = 0;
            }
            else
            {
                invisibleuser = dr.Length;
            }
            //统计游客
            if (totaluser > user)
            {
                guest = totaluser - user;
            }
            else
            {
                guest = 0;
            }
            //返回当前版块的在线用户表
            return dt;
        }



        #endregion


        /// <summary>
        /// 返回在线用户图例
        /// </summary>
        /// <returns>在线用户图例</returns>
        private static DataTable GetOnlineGroupIconTable()
        {

            lock (SynObject)
            {
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                DataTable dt = cache.RetrieveObject("/OnlineIconTable") as DataTable;
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    dt = DatabaseProvider.GetInstance().GetOnlineGroupIconTable();
                    cache.AddObject("/OnlineIconTable", dt);
                    return dt;
                }
            }
        }
        /// <summary>
        /// 返回用户组图标
        /// </summary>
        /// <param name="groupid">用户组</param>
        /// <returns>用户组图标</returns>
        public static string GetGroupImg(int groupid)
        {
            string img = "";
            DataTable dt = GetOnlineGroupIconTable();
            // 如果没有要显示的图例类型则返回""
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    // 图例类型初始为:普通用户
                    // 如果有匹配的则更新为匹配的图例
                    if ((int.Parse(dr["groupid"].ToString()) == 0 && img == "") || (int.Parse(dr["groupid"].ToString()) == groupid))
                    {
                        img = "<img src=\"images\\groupicons\\" + dr["img"].ToString() + "\" />";
                    }
                }
            }
            return img;
        }


        #region 查看指定的某一用户的详细信息
        public static OnlineUserInfo GetOnlineUser(int olid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetOnlineUser(olid);
            OnlineUserInfo onlineuserinfo = null;
            if (reader.Read())
            {
                onlineuserinfo = LoadSingleOnlineUser(reader);
            }
            reader.Close();
            return onlineuserinfo;
        }

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userid">在线用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>用户的详细信息</returns>
        private static OnlineUserInfo GetOnlineUser(int userid, string password)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetOnlineUser(userid, password);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                OnlineUserInfo onlineuserinfo = LoadSingleOnlineUser(dr);
                dt.Dispose();
                return onlineuserinfo;
            }
            return null;


        }


        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <returns>用户的详细信息</returns>
        private static OnlineUserInfo GetOnlineUserByIP(int userid, string ip)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetOnlineUserByIP(userid, ip);
            if (dt.Rows.Count > 0)
            {
                OnlineUserInfo oluser = LoadSingleOnlineUser(dt.Rows[0]);
                dt.Dispose();
                return oluser;

            }
            return null;


        }



        /// <summary>
        /// 检查在线用户验证码是否有效
        /// </summary>
        /// <param name="olid">在组用户ID</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>在组用户ID</returns>
        public static bool CheckUserVerifyCode(int olid, string verifycode)
        {
            return DatabaseProvider.GetInstance().CheckUserVerifyCode(olid, verifycode, ForumUtils.CreateAuthStr(5, false));
        }



        #endregion


        #region 添加新的在线用户
        /// <summary>
        /// 执行在线用户向表及缓存中添加的操作。
        /// </summary>
        /// <param name="onlineuserinfo">在组用户信息内容</param>
        /// <returns>添加成功则返回刚刚添加的olid,失败则返回0</returns>
        private static int Add(OnlineUserInfo onlineuserinfo, int timeout)
        {
            return DatabaseProvider.GetInstance().AddOnlineUser(onlineuserinfo, timeout);
        }


        /// <summary>
        /// Cookie中没有用户ID或则存的的用户ID无效时在在线表中增加一个游客.
        /// </summary>
        public static OnlineUserInfo CreateGuestUser(int timeout)
        {


            OnlineUserInfo onlineuserinfo = new OnlineUserInfo();

            onlineuserinfo.Userid = -1;
            onlineuserinfo.Username = "游客";
            onlineuserinfo.Nickname = "游客";
            onlineuserinfo.Password = "";
            onlineuserinfo.Groupid = 7;
            onlineuserinfo.Olimg = GetGroupImg(7);
            onlineuserinfo.Adminid = 0;
            onlineuserinfo.Invisible = 0;
            onlineuserinfo.Ip = DNTRequest.GetIP();
            onlineuserinfo.Lastposttime = "1900-1-1 00:00:00";
            onlineuserinfo.Lastpostpmtime = "1900-1-1 00:00:00";
            onlineuserinfo.Lastsearchtime = "1900-1-1 00:00:00";
            onlineuserinfo.Lastupdatetime = Utils.GetDateTime();
            onlineuserinfo.Action = 0;
            onlineuserinfo.Lastactivity = 0;
            onlineuserinfo.Verifycode = ForumUtils.CreateAuthStr(5, false);

            int olid = Add(onlineuserinfo, timeout);
            onlineuserinfo.Olid = olid;

            return onlineuserinfo;

        }


        /// <summary>
        /// 增加一个会员信息到在线列表中。用户login.aspx或在线用户信息超时,但用户仍在线的情况下重新生成用户在线列表
        /// </summary>
        /// <param name="uid"></param>
        private static OnlineUserInfo CreateUser(int uid, int timeout)
        {
            OnlineUserInfo onlineuserinfo = new OnlineUserInfo();
            if (uid > 0)
            {
                ShortUserInfo ui = Users.GetShortUserInfo(uid);
                if (ui != null)
                {
                    onlineuserinfo.Userid = uid;
                    onlineuserinfo.Username = ui.Username.Trim();
                    onlineuserinfo.Nickname = ui.Nickname.Trim();
                    onlineuserinfo.Password = ui.Password.Trim();
                    onlineuserinfo.Groupid = short.Parse(ui.Groupid.ToString());
                    onlineuserinfo.Olimg = GetGroupImg(short.Parse(ui.Groupid.ToString()));
                    onlineuserinfo.Adminid = short.Parse(ui.Adminid.ToString());
                    onlineuserinfo.Invisible = short.Parse(ui.Invisible.ToString());


                    onlineuserinfo.Ip = DNTRequest.GetIP();
                    onlineuserinfo.Lastposttime = "1900-1-1 00:00:00";
                    onlineuserinfo.Lastpostpmtime = "1900-1-1 00:00:00";
                    onlineuserinfo.Lastsearchtime = "1900-1-1 00:00:00";
                    onlineuserinfo.Lastupdatetime = Utils.GetDateTime();
                    onlineuserinfo.Action = 0;
                    onlineuserinfo.Lastactivity = 0;
                    onlineuserinfo.Verifycode = ForumUtils.CreateAuthStr(5, false);


                    int olid = Add(onlineuserinfo, timeout);
                    DatabaseProvider.GetInstance().SetUserOnlineState(uid, 1);
                    onlineuserinfo.Olid = olid;

                    HttpCookie cookie = HttpContext.Current.Request.Cookies["dnt"];
                    if (cookie != null)
                    {
                        cookie.Values["tpp"] = ui.Tpp.ToString();
                        cookie.Values["ppp"] = ui.Ppp.ToString();
                        if (HttpContext.Current.Request.Cookies["dnt"]["expires"] != null)
                        {
                            int expires = Utils.StrToInt(HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString(), 0);
                            if (expires > 0)
                            {
                                cookie.Expires = DateTime.Now.AddMinutes(Utils.StrToInt(HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString(), 0));
                            }
                        }
                    }

                    try
                    {
                        string cookieDomain = GeneralConfigs.GetConfig().CookieDomain.Trim();
                        if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain) > -1 && ForumUtils.IsValidDomain(HttpContext.Current.Request.Url.Host))
                            cookie.Domain = cookieDomain;
                        HttpContext.Current.Response.AppendCookie(cookie);
                    }
                    catch { }
                    //catch(Exception ex)
                    //{
                    //    Discuz.Forum.ScheduledEvents.EventLogs.WriteFailedLog("OnlineUsers.cs(440)," + ex.Message);
                    //}
                }
            }
            else
            {
                onlineuserinfo = CreateGuestUser(timeout);
            }
            return onlineuserinfo;

        }




        /// <summary>
        /// 用户在线信息维护。判断当前用户的身份(会员还是游客),是否在在线列表中存在,如果存在则更新会员的当前动,不存在则建立.
        /// </summary>
        /// <param name="passwordkey">论坛passwordkey</param>
        /// <param name="timeout">在线超时时间</param>
        /// <param name="passwd">用户密码</param>
        public static OnlineUserInfo UpdateInfo(string passwordkey, int timeout, int uid, string passwd)
        {

            lock (SynObject)
            {
                OnlineUserInfo onlineuser = new OnlineUserInfo();


                string ip = DNTRequest.GetIP();
                int userid = Discuz.Forum.Users.GetUserIDFromCookie();

                if (userid != -1)
                {
                    
                    onlineuser = GetOnlineUser(userid, Users.GetUserInfo(userid).Password);

                    //更新流量统计
                    if (!DNTRequest.GetPageName().EndsWith("ajax.aspx") && GeneralConfigs.GetConfig().Statstatus == 1)
                    {
                        Stats.UpdateStatCount(false, onlineuser != null);
                    }

                    if (onlineuser != null)
                    {

                        if (onlineuser.Ip != ip)
                        {
                            UpdateIP(onlineuser.Olid, ip);

                            onlineuser.Ip = ip;

                            return onlineuser;
                        }
                    }
                    else
                    {

                        // 判断密码是否正确
                        userid = Users.CheckPassword(userid, Users.GetUserInfo(userid).Password, false);
                        if (userid != -1)
                        {
                            DeleteRowsByIP(ip);
                            return CreateUser(userid, timeout);
                        }
                        else
                        {
                            // 如密码错误则在在线表中创建游客
                            onlineuser = GetOnlineUserByIP(-1, ip);
                            if (onlineuser == null)
                            {
                                return CreateGuestUser(timeout);
                            }
                        }
                    }

                }
                else
                {
                    onlineuser = GetOnlineUserByIP(-1, ip);
                    //更新流量统计
                    if (!DNTRequest.GetPageName().EndsWith("ajax.aspx") && GeneralConfigs.GetConfig().Statstatus == 1)
                    {
                        Stats.UpdateStatCount(true, onlineuser != null);
                    }

                    if (onlineuser == null)
                    {
                        return CreateGuestUser(timeout);
                    }

                }

                //UpdateLastTime(onlineuser.Olid);

                onlineuser.Lastupdatetime = Utils.GetDateTime();
                return onlineuser;

            }

        }

        /// <summary>
        /// 用户在线信息维护。判断当前用户的身份(会员还是游客),是否在在线列表中存在,如果存在则更新会员的当前动,不存在则建立.
        /// </summary>
        /// <param name="timeout">在线超时时间</param>
        /// <param name="passwd">用户密码</param>
        public static OnlineUserInfo UpdateInfo(string passwordkey, int timeout)
        {
            return UpdateInfo(passwordkey, timeout, -1, "");
        }

        #endregion


        #region 在组用户信息更新

        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        /// <param name="timeout">过期时间</param>
        public static void UpdateAction(int olid, int action, int inid, int timeout)
        {

            // 如果上次刷新cookie间隔小于5分钟, 则不刷新数据库最后活动时间
            if ((timeout < 0) && (Environment.TickCount - Utils.StrToInt(Utils.GetCookie("lastolupdate"), Environment.TickCount) < 300000))
            {
                Utils.WriteCookie("lastolupdate", Environment.TickCount.ToString());
            }
            else
            {
                UpdateAction(olid, action, inid);
            }

        }
        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        public static void UpdateAction(int olid, int action, int inid)
        {
            DatabaseProvider.GetInstance().UpdateAction(olid, action, inid);
        }


        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作id</param>
        /// <param name="fid">版块id</param>
        /// <param name="forumname">版块名</param>
        /// <param name="tid">主题id</param>
        /// <param name="topictitle">主题名</param>
        /// <param name="timeout">超时时间</param>
        public static void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle, int timeout)
        {
            // 如果上次刷新cookie间隔小于5分钟, 则不刷新数据库最后活动时间
            if ((timeout < 0) && (System.Environment.TickCount - Utils.StrToInt(Utils.GetCookie("lastolupdate"), System.Environment.TickCount) < 300000))
            {
                Utils.WriteCookie("lastolupdate", System.Environment.TickCount.ToString());
            }
            else
            {
                if ((action == UserAction.ShowForum.ActionID) || (action == UserAction.PostTopic.ActionID) || (action == UserAction.ShowTopic.ActionID) || (action == UserAction.PostReply.ActionID))
                {
                    forumname = forumname.Length > 40 ? forumname.Substring(0, 37) + "..." : forumname;
                }

                if ((action == UserAction.ShowTopic.ActionID) || (action == UserAction.PostReply.ActionID))
                {
                    topictitle = topictitle.Length > 40 ? topictitle.Substring(0, 37) + "..." : topictitle;
                }
                DatabaseProvider.GetInstance().UpdateAction(olid, action, fid, forumname, tid, topictitle);
            }

        }

        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="timeout">超时时间</param>
        private static void UpdateLastTime(int olid, int timeout)
        {

            // 如果上次刷新cookie间隔小于5分钟, 则不刷新数据库最后活动时间
            if ((timeout < 0) && (System.Environment.TickCount - Utils.StrToInt(Utils.GetCookie("lastolupdate"), System.Environment.TickCount) < 300000))
            {
                Utils.WriteCookie("lastolupdate", System.Environment.TickCount.ToString());
            }
            else
            {
                DatabaseProvider.GetInstance().UpdateLastTime(olid);
            }
        }

        /// <summary>
        /// 更新用户最后发帖时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public static void UpdatePostTime(int olid)
        {
            DatabaseProvider.GetInstance().UpdatePostTime(olid);
        }

        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public static void UpdatePostPMTime(int olid)
        {
            DatabaseProvider.GetInstance().UpdatePostPMTime(olid);
        }

        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        public static void UpdateInvisible(int olid, int invisible)
        {
            DatabaseProvider.GetInstance().UpdateInvisible(olid, invisible);
        }

        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="password">用户密码</param>
        public static void UpdatePassword(int olid, string password)
        {
            DatabaseProvider.GetInstance().UpdatePassword(olid, password);
        }


        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="ip">ip地址</param>
        public static void UpdateIP(int olid, string ip)
        {
            DatabaseProvider.GetInstance().UpdateIP(olid, ip);
        }

        /// <summary>
        /// 更新用户最后搜索时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public static void UpdateSearchTime(int olid)
        {
            DatabaseProvider.GetInstance().UpdateSearchTime(olid);
        }



        /// <summary>
        /// 更新用户的用户组
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="groupid">组名</param>
        public static void UpdateGroupid(int userid, int groupid)
        {
            DatabaseProvider.GetInstance().UpdateGroupid(userid, groupid);
        }

        #endregion


        #region 删除符合条件的用户信息

        /// <summary>
        /// 删除符合条件的一个或多个用户信息
        /// </summary>
        /// <returns>删除行数</returns>
        private static int DeleteRowsByIP(string ip)
        {
            return DatabaseProvider.GetInstance().DeleteRowsByIP(ip);
        }

        /// <summary>
        /// 删除在线表中指定在线id的行
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        public static int DeleteRows(int olid)
        {
            return DatabaseProvider.GetInstance().DeleteRows(olid);
        }


        #endregion


        #region 条件编译的方法

        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="totaluser">全部用户数</param>
        /// <param name="guest">游客数</param>
        /// <param name="user">登录用户数</param>
        /// <param name="invisibleuser">隐身会员数</param>
        /// <returns></returns>
#if NET1
        public static OnlineUserInfoCollection GetForumOnlineUserCollection(int forumid, out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            OnlineUserInfoCollection coll = new OnlineUserInfoCollection();
#else
        public static Discuz.Common.Generic.List<OnlineUserInfo> GetForumOnlineUserCollection(int forumid, out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            Discuz.Common.Generic.List<OnlineUserInfo> coll = new Discuz.Common.Generic.List<OnlineUserInfo>();
#endif
            //在线游客
            guest = 0;
            //在线隐身用户
            invisibleuser = 0;
            //用户总数
            totaluser = 0;

            IDataReader reader = DatabaseProvider.GetInstance().GetForumOnlineUserList(forumid);
            while (reader.Read())
            {
                OnlineUserInfo info = LoadSingleOnlineUser(reader);
                //当前版块在线总用户数
                totaluser++;
                if (info.Userid == -1)
                {
                    guest++;
                }
                if (info.Invisible == 1)
                {
                    invisibleuser++;
                }
                coll.Add(info);
            }
            reader.Close();

            //统计用户
            user = totaluser - guest;
            //返回当前版块的在线用户表
            return coll;
        }


        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="totaluser">全部用户数</param>
        /// <param name="guest">游客数</param>
        /// <param name="user">登录用户数</param>
        /// <param name="invisibleuser">隐身会员数</param>
        /// <returns></returns>
#if NET1
        public static OnlineUserInfoCollection GetOnlineUserCollection(out int totaluser, out int guest, out int user, out int invisibleuser)
		{
			OnlineUserInfoCollection coll = new OnlineUserInfoCollection();
#else
        public static Discuz.Common.Generic.List<OnlineUserInfo> GetOnlineUserCollection(out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            Discuz.Common.Generic.List<OnlineUserInfo> coll = new Discuz.Common.Generic.List<OnlineUserInfo>();
#endif
            //在线注册用户数
            user = 0;
            //在线隐身用户数
            invisibleuser = 0;
            //在线总用户数
            totaluser = 0;
            IDataReader reader = DatabaseProvider.GetInstance().GetOnlineUserList();
            while (reader.Read())
            {
                OnlineUserInfo info = LoadSingleOnlineUser(reader);
                //
                if (info.Userid > 0)
                {
                    user++;
                }
                if (info.Invisible == 1)
                {
                    invisibleuser++;
                }
                totaluser++;
                if (info.Userid > 0 || (info.Userid == -1 && GeneralConfigs.GetConfig().Whosonlinecontract == 0))
                {
                    info.Actionname = UserAction.GetActionDescriptionByID((int)(info.Action));
                    coll.Add(info);
                }
            }
            reader.Close();
            int highestonlineusercount = Utils.StrToInt(Statistics.GetStatisticsRowItem("highestonlineusercount"), 1);
            if (totaluser > highestonlineusercount)
            {
                if (Statistics.UpdateStatistics("highestonlineusercount", totaluser) > 0)
                {
                    Statistics.UpdateStatistics("highestonlineusertime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Statistics.ReSetStatisticsCache();
                }

            }

            //统计游客
            if (totaluser > user)
            {
                guest = totaluser - user;
            }
            else
            {
                guest = 0;
            }
            //返回当前版块的在线用户集合
            return coll;
        }


        public static void UpdateOnlineTime(int oltimespan, int uid)
        {
            //为0代表关闭统计功能
            if (oltimespan == 0)
            {
                return;
            }
            if (Utils.GetCookie("lastactivity", "onlinetime") == string.Empty)
            {
                Utils.WriteCookie("lastactivity", "onlinetime", System.Environment.TickCount.ToString());
            }

            if ((System.Environment.TickCount - Utils.StrToInt(Utils.GetCookie("lastactivity", "onlinetime"), System.Environment.TickCount) >= oltimespan * 60 * 1000))
            {
                DatabaseProvider.GetInstance().UpdateOnlineTime(oltimespan, uid);
                Utils.WriteCookie("lastactivity", "onlinetime", System.Environment.TickCount.ToString());

                //判断是否同步oltime (登录后的第一次onlinetime更新的时候或者在线超过1小时)
                if (Utils.GetCookie("lastactivity", "oltime") == string.Empty || (System.Environment.TickCount - Utils.StrToInt(Utils.GetCookie("lastactivity", "onlinetime"), System.Environment.TickCount) >= 60 * 60 * 1000))
                {
                    DatabaseProvider.GetInstance().SynchronizeOltime(uid);
                    Utils.WriteCookie("lastactivity", "oltime", System.Environment.TickCount.ToString());
                }
            }


        }

        #endregion

        #region Helper
        private static OnlineUserInfo LoadSingleOnlineUser(IDataReader reader)
        {
            OnlineUserInfo info = new OnlineUserInfo();
            info.Olid = Int32.Parse(reader["olid"].ToString());
            info.Userid = Int32.Parse(reader["userid"].ToString());
            info.Ip = reader["ip"].ToString();
            info.Username = reader["username"].ToString();
            //info.Tickcount = Int32.Parse(reader["tickcount"].ToString());
            info.Nickname = reader["nickname"].ToString();
            info.Password = reader["password"].ToString();
            info.Groupid = Int16.Parse(reader["groupid"].ToString() == "" ? "10" : reader["groupid"].ToString());
            info.Adminid = Int16.Parse(reader["adminid"].ToString() == "" ? "1" : reader["adminid"].ToString());

            switch(info.Adminid)
            {
                case 1:
                    info.Olimg = "<img src=\"images/groupicons/admin.gif\" />";
                    break;
                case 2:
                    info.Olimg = "<img src=\"images/groupicons/supermoder.gif\" />";
                    break;
                case 3:
                    info.Olimg = "<img src=\"images/groupicons/moder.gif\" />";
                    break;
            }

            switch (info.Groupid)
            {
                case 10:
                    info.Olimg = "<img src=\"images/groupicons/member.gif\" />";
                    break;
                case 7:
                    info.Olimg = "<img src=\"images/groupicons/guest.gif\" />";
                    break;
            }

            if (info.Olimg == null || info.Olimg == "")
            {
                info.Olimg = "<img src=\"images/groupicons/member.gif\" />";
            }

            info.Invisible = Int16.Parse(reader["invisible"].ToString());
            info.Action = Int16.Parse(reader["action"].ToString());
            info.Lastactivity = Int16.Parse(reader["lastactivity"].ToString());
            info.Lastposttime = reader["lastposttime"].ToString();
            info.Lastpostpmtime = reader["lastpostpmtime"].ToString();
            info.Lastsearchtime = reader["lastsearchtime"].ToString();
            info.Lastupdatetime = reader["lastupdatetime"].ToString();
            info.Forumid = Int32.Parse(reader["forumid"].ToString());
            if (reader["forumname"] != DBNull.Value)
            {
                info.Forumname = reader["forumname"].ToString();
            }
            info.Titleid = Int32.Parse(reader["titleid"].ToString());
            if (reader["title"] != DBNull.Value)
            {
                info.Title = reader["title"].ToString();
            }
            info.Verifycode = reader["verifycode"].ToString();
            return info;
        }

        private static OnlineUserInfo LoadSingleOnlineUser(DataRow dr)
        {
            OnlineUserInfo info = new OnlineUserInfo();
            info.Olid = Int32.Parse(dr["olid"].ToString());
            info.Userid = Int32.Parse(dr["userid"].ToString());
            info.Ip = dr["ip"].ToString();
            info.Username = dr["username"].ToString();
            //info.Tickcount = Int32.Parse(reader["tickcount"].ToString());
            info.Nickname = dr["nickname"].ToString();
            info.Password = dr["password"].ToString();
            info.Groupid = Int16.Parse(dr["groupid"].ToString());
            info.Olimg = dr["olimg"].ToString();
            info.Adminid = Int16.Parse(dr["adminid"].ToString());
            info.Invisible = Int16.Parse(dr["invisible"].ToString());
            info.Action = Int16.Parse(dr["action"].ToString());
            info.Lastactivity = Int16.Parse(dr["lastactivity"].ToString());
            info.Lastposttime = dr["lastposttime"].ToString();
            info.Lastpostpmtime = dr["lastpostpmtime"].ToString();
            info.Lastsearchtime = dr["lastsearchtime"].ToString();
            info.Lastupdatetime = dr["lastupdatetime"].ToString();
            info.Forumid = Int32.Parse(dr["forumid"].ToString());
            if (dr["forumname"] != DBNull.Value)
            {
                info.Forumname = dr["forumname"].ToString();
            }
            info.Titleid = Int32.Parse(dr["titleid"].ToString());
            if (dr["title"] != DBNull.Value)
            {
                info.Title = dr["title"].ToString();
            }
            info.Verifycode = dr["verifycode"].ToString();
            return info;
        }
        #endregion


        /// <summary>
        /// 根据Uid获得Olid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetOlidByUid(int uid)
        {
            return DatabaseProvider.GetInstance().GetOlidByUid(uid);
        }

        /// <summary>
        /// 删除在线表中Uid的用户
        /// </summary>
        /// <param name="uid">要删除用户的Uid</param>
        /// <returns></returns>
        public static int DeleteUserByUid(int uid)
        {
            int olid = GetOlidByUid(uid);
            return DeleteRows(olid);
        }
    }//class end
}
