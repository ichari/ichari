using System;
using System.Text;
using Discuz.Data;
using System.Data;
using Discuz.Entity;
using System.Reflection;
using Discuz.Config;
using Discuz.Common;
using System.Web;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    public class Debates
    {


        /// <summary>
        /// 获取帖子观点
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>Dictionary泛型</returns>
        public static Dictionary<int, int> GetPostDebateList(int tid)
        {
            Dictionary<int, int> debateList = new Dictionary<int, int>();
            IDataReader reader = DatabaseProvider.GetInstance().GetPostDebate(tid);
            while (reader.Read())
            {
                debateList.Add(Utils.StrToInt(reader["pid"], 0), Utils.StrToInt(reader["opinion"], 0));
            }
            reader.Close();
            return debateList;
        }

        /// <summary>
        /// 获取辩论的扩展信息
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>辩论主题扩展信息</returns>
        public static DebateInfo GetDebateTopic(int tid)
        {
            DebateInfo topicexpand = new DebateInfo();
            IDataReader debatetopic = DatabaseProvider.GetInstance().GetDebateTopic(tid);
            if (debatetopic.Read())
            {
                topicexpand.Positiveopinion = debatetopic["positiveopinion"].ToString();
                topicexpand.Negativeopinion = debatetopic["negativeopinion"].ToString();
                //topicexpand.Positivecolor = debatetopic["positivecolor"].ToString();
                //topicexpand.Negativecolor = debatetopic["negativecolor"].ToString();
                topicexpand.Terminaltime = DateTime.Parse(debatetopic["terminaltime"].ToString());
                topicexpand.Positivediggs = int.Parse(debatetopic["positivediggs"].ToString());
                topicexpand.Negativediggs = int.Parse(debatetopic["negativediggs"].ToString());
                //topicexpand.Positivebordercolor = debatetopic["positivebordercolor"].ToString();
                //topicexpand.Negativebordercolor = debatetopic["negativebordercolor"].ToString();
                topicexpand.Tid = tid;
            }
            debatetopic.Close();
            return topicexpand;
        }

        /// <summary>
        /// 返回调用的JSON数据
        /// </summary>
        /// <param name="callback">JS回调函数</param>
        /// <param name="tidlist">主题ID列表</param>
        /// <returns>JS数据</returns>
        public static string GetDebatesJsonList(string callback, string tidlist)
        {

            IDataReader reader = null;
            switch (callback)
            {
                case "gethotdebatetopic":
                    string[] debatesrule;
                    int listlength = tidlist.Split(',').Length;
                    if (listlength < 3)
                    {
                        break;
                    }
                    else
                    {
                        debatesrule = tidlist.Split(',');
                        if (debatesrule[0] != "views" && debatesrule[0] != "replies" && Utils.IsNumeric(debatesrule[1]) && Utils.IsNumeric(debatesrule[2]))
                            break;
                    }
                    reader = DatabaseProvider.GetInstance().GetHotDebatesList(debatesrule[0], Utils.StrToInt(debatesrule[1], 0), Utils.StrToInt(debatesrule[2], 0));
                    break;
                case "recommenddebates":
                    if (tidlist == string.Empty)
                    {
                        tidlist = GeneralConfigs.GetConfig().Recommenddebates;
                    }
                    else
                    {
                        if (!Utils.IsNumericList(tidlist))
                        {
                            break;
                        }
                    }

                    reader = DatabaseProvider.GetInstance().GetRecommendDebates(tidlist);
                    break;
                default:
                    break;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(callback);
            sb.Append("([");
                if (reader.Read())
                {
                    string str = string.Format("{{'title':'{0}','tid','{1}'}},", reader["title"].ToString(), reader["tid"].ToString());
                    sb.Append(str);
                    
                }
                else
                {

                    return "0";
                }

            while (reader.Read())
            {
                string str = string.Format("{{'title':'{0}','tid','{1}'}},", reader["title"].ToString(), reader["tid"].ToString());
                sb.Append(str);
            }
            reader.Close();
            return sb.ToString().Remove(sb.Length - 1) + "])";
        }
        /// <summary>
        /// 添加点评
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="message">点评内容</param>
        public static void CommentDabetas(int tid, string message)
        {
            DatabaseProvider.GetInstance().AddCommentDabetas(tid, int.Parse(Posts.GetPostTableID(tid)), message);

        }


        /// <summary>
        /// 验证用户组是否允许顶
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="tips">提示信息</param>
        /// <returns>是否可以顶,布尔类型</returns>
        public static bool AllowDiggs(int userid, out string tips)
        {
            tips = "您所在的用户组不允许此操作";
            if (UserGroups.GetUserGroupInfo(7).Allowdiggs == 0)
            {
                if (userid == -1)
                {
                  
                    return false;
                }
            
            }
            
            UserInfo userinfo = Users.GetUserInfo(userid);
            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(userinfo.Groupid);
            if (usergroupinfo.Allowdiggs == 0)
            {
                //tips = "您所在的用户组不允许此操作";
                return false;
            }
            return true;
        }


        /// <summary>
        /// 增加Digg
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="type">正反方观点</param>
        /// <param name="userid">用户ID</param>
        public static void AddDebateDigg(int tid, int pid, int type, int userid)
        {
            UserInfo userinfo = Users.GetUserInfo(userid);
            DatabaseProvider.GetInstance().AddDebateDigg(tid, pid, type,Utils.GetRealIP(), userinfo);

        }

        /// <summary>
        /// 判断是否顶过
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <param name="userid">用户ID</param>
        /// <returns>判断是否顶过</returns>
        public static bool IsDigged(int pid, int userid)
        {
            //开放游客后，验证方式为松散验证,24小时内只能顶一次
            if (UserGroups.GetUserGroupInfo(7).Allowdiggs != 1)
            {
                return !DatabaseProvider.GetInstance().AllowDiggs(pid, userid);
            }
            else
            {
                if (Utils.GetCookie("debatedigged") == string.Empty)
                {
                    return false;
                }
                string[] pidlist = Utils.GetCookie("debatedigged").Split(',');
                foreach (string s in pidlist)
                {
                    if (pid == Utils.StrToInt(s, 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 写入已顶的COOKIES
        /// </summary>
        /// <param name="pid">帖子ID</param>
        public static void WriteCookies(int pid)
        {

            if (Utils.GetCookie("debatedigged") == string.Empty)
            {
                Utils.WriteCookie("debatedigged", pid.ToString(), 1440);
            }
            else
            {
                string newlist = Utils.GetCookie("debatedigged") + "," + pid.ToString();
                Utils.WriteCookie("debatedigged", newlist, 1440);
            }
        }

        /// <summary>
        /// 返回辩论主题的帖子一方的帖子数
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="debateOpinion">帖子观点</param>
        /// <returns>帖子数</returns>
        public static int GetDebatesPostCount(PostpramsInfo postpramsInfo, int debateOpinion)
        {
            return DatabaseProvider.GetInstance().GetDebatesPostCount(postpramsInfo.Tid, debateOpinion);
        
        }
        /// <summary>
        /// 获取辩论正方帖子列表
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="attachmentlist">附件列表</param>
        /// <param name="ismoder">是否有管理权限</param>
        /// <returns>正方帖子列表</returns>
        public static List<ShowtopicPagePostInfo> GetPositivePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 1, new PostOrderType());
        }

        private static List<ShowtopicPagePostInfo> GetDebatePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder, int debateOpinion, PostOrderType postOrderType)
        {
            List<ShowtopicPagePostInfo> postcoll = new List<ShowtopicPagePostInfo>();
            attachmentlist = new List<ShowtopicPageAttachmentInfo>();
            StringBuilder attachmentpidlist = new StringBuilder();
            StringBuilder pidlist = new StringBuilder();
            IDataReader reader = DatabaseProvider.GetInstance().GetDebatePostList(postpramsInfo.Tid, debateOpinion, postpramsInfo.Pagesize, postpramsInfo.Pageindex, Posts.GetPostTableName(postpramsInfo.Tid), postOrderType);

            postcoll = Posts.ParsePostList(postpramsInfo, attachmentlist, ismoder, postcoll, reader, attachmentpidlist);

            //当因冗余字段不准导致未取得分页信息时，修正冗余字段，并取最后一页
            if (postcoll.Count == 0 && postpramsInfo.Pageindex > 1)
            {
                int postcount = DatabaseProvider.GetInstance().ReviseDebateTopicDiggs(postpramsInfo.Tid, debateOpinion);

                postpramsInfo.Pageindex = postcount % postpramsInfo.Pagesize == 0 ? postcount / postpramsInfo.Pagesize : postcount / postpramsInfo.Pagesize + 1;

                reader = DatabaseProvider.GetInstance().GetDebatePostList(postpramsInfo.Tid, debateOpinion, postpramsInfo.Pagesize, postpramsInfo.Pageindex, Posts.GetPostTableName(postpramsInfo.Tid), postOrderType);

                postcoll = Posts.ParsePostList(postpramsInfo, attachmentlist, ismoder, postcoll, reader, attachmentpidlist);
            }

           

            foreach (ShowtopicPagePostInfo post in postcoll)
            {
                pidlist.AppendFormat("{0},", post.Pid);
            }

            Dictionary<int, int> postdiggs = GetPostDiggs(pidlist.ToString().Trim(','));
            foreach (ShowtopicPagePostInfo post in postcoll)
            {
                if (postdiggs.ContainsKey(post.Pid))
                {
                    post.Diggs = postdiggs[post.Pid];
                }
            }
            return postcoll;
        }
        /// <summary>
        /// 返回帖子被顶数
        /// </summary>
        /// <param name="pidlist">帖子ID数组</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<int, int> GetPostDiggs(string pidlist)
        {
            Dictionary<int, int> result = new Dictionary<int,int>();
            IDataReader reader = DatabaseProvider.GetInstance().GetDebatePostDiggs(pidlist);
            if (reader != null)
            {
                while (reader.Read())
                {
                    result[Convert.ToInt32(reader["pid"])] = Convert.ToInt32(reader["diggs"]);
                }
                reader.Close();
            }
           
            return result;
        }

        /// <summary>
        /// 反方的帖子列表
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="attachmentlist">附件列表</param>
        /// <param name="ismoder">是否有管理权限</param>
        /// <returns>反方帖子列表</returns>
        public static List<ShowtopicPagePostInfo> GetNegativePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 2, new PostOrderType());
        }

        /// <summary>
        /// 得到用户已顶过帖子PID
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>用户已顶过帖子PID</returns>
        public static string GetUesrDiggs(int tid, int uid)
        {
            //string userdiggs = "";
            StringBuilder sb = new StringBuilder();
            IDataReader reader = DatabaseProvider.GetInstance().GetUesrDiggs(tid, uid);
            while (reader.Read())
            {
                //userdiggs = userdiggs + "|" + reader["pid"].ToString();
                //sb.Append(userdiggs);
                sb.Append("|");
                sb.Append(reader["pid"].ToString());

            }
            reader.Close();
            //return userdiggs;
            return sb.ToString();
        }
    }
}
