using System;
using System.Data;
using System.Text;
using System.Xml;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 论坛聚合数据类
    /// </summary>
    public class ForumAggregationData : AggregationData
    {

        /// <summary>
        /// 推荐的论坛主题帖
        /// </summary>
        private static PostInfo[] __postinfos;

        /// <summary>
        /// 聚合首页BBS版块(主题)列表
        /// </summary>
        private static DataTable __topicList;

        /// <summary>
        /// 聚合首页版块推荐主题
        /// </summary>
        private static StringBuilder __topicjson;

        private static int[] __recommendForumidArray;

        /// <summary>
        /// 清空数据绑定
        /// </summary>
        public override void ClearDataBind()
        {      
            __postinfos = null;

            __topicList = null;

            __topicjson = null;

            __recommendForumidArray = null;
        }

        /// <summary>
        /// 获取后台推荐的版块id串
        /// </summary>
        /// <returns></returns>
        public int [] GetRecommendForumID()
        {
            if (__recommendForumidArray != null)
            {
                return __recommendForumidArray;
            }

            XmlNodeList xmlnodelist = __xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend");
            if (xmlnodelist.Count > 0)
            {
                string forumidlist = __xmlDoc.GetSingleNodeValue(xmlnodelist[0], "fidlist") == null ? "" : __xmlDoc.GetSingleNodeValue(xmlnodelist[0], "fidlist");
                if (!Utils.StrIsNullOrEmpty(forumidlist))
                {
                    string[] forumidarray = forumidlist.Split(',');
                    if (Utils.IsNumericArray(forumidarray))
                    {
                        __recommendForumidArray = new int[forumidarray.Length];
                        int i = 0;
                        foreach (string forumid in forumidarray)
                        {
                            if (forumid != "")
                            {
                                __recommendForumidArray[i] = Convert.ToInt32(forumid);
                                i++;
                            }
                        }
                    }
                }
            }
            return __recommendForumidArray == null ? new int [] { 0 } : __recommendForumidArray;
        }
         


              

        #region 从XML中检查出指定的主题信息

        /// <summary>
        /// 获得推荐的论坛主题帖对象数组
        /// </summary>
        /// <param name="nodename">节点名称</param>
        /// <returns></returns>
        public PostInfo[] GetPostListFromFile(string nodename)
        {
            if (__postinfos != null)
            {
                return __postinfos;
            }

            XmlNodeList xmlnodelist = __xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodename + "/Topiclist/Topic");
            __postinfos = new PostInfo[xmlnodelist.Count];
            int rowcount = 0;

            foreach (XmlNode xmlnode in xmlnodelist)
            {
                    __postinfos[rowcount] = new PostInfo();
                    __postinfos[rowcount].Tid = (__xmlDoc.GetSingleNodeValue(xmlnode, "topicid") == null) ? 0 : Convert.ToInt32(__xmlDoc.GetSingleNodeValue(xmlnode, "topicid"));
                    __postinfos[rowcount].Title = (__xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : Utils.RemoveHtml(__xmlDoc.GetSingleNodeValue(xmlnode, "title"));
                    __postinfos[rowcount].Poster = (__xmlDoc.GetSingleNodeValue(xmlnode, "poster") == null) ? "" : __xmlDoc.GetSingleNodeValue(xmlnode, "poster");
                    __postinfos[rowcount].Posterid = (__xmlDoc.GetSingleNodeValue(xmlnode, "posterid") == null) ? 0 : Convert.ToInt32(__xmlDoc.GetSingleNodeValue(xmlnode, "posterid"));
                    __postinfos[rowcount].Postdatetime = (__xmlDoc.GetSingleNodeValue(xmlnode, "postdatetime") == null) ? "" : __xmlDoc.GetSingleNodeValue(xmlnode, "postdatetime");
                    __postinfos[rowcount].Message = (__xmlDoc.GetSingleNodeValue(xmlnode, "shortdescription") == null) ? "" : Utils.RemoveHtml(__xmlDoc.GetSingleNodeValue(xmlnode, "shortdescription"));
                    rowcount++;
            }
            return __postinfos;
        }

        public string GetTopicJsonFromFile()
        {
            if (__topicjson != null)
            {
                return __topicjson.ToString();
            }

            __topicjson = new StringBuilder();
            __topicjson.Append("[");
            XmlNodeList xmlnodelist = __xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomendtopiclist/Website_forumrecomendtopic");
            int rowcount = 1;

            foreach (XmlNode xmlnode in xmlnodelist)
            {
                 __topicjson.Append(string.Format("{{'id' : {0}, 'title' : '{1}', 'fid' : {2}, 'img' : '{3}', 'tid' : {4}}},",
                        rowcount,
                        __xmlDoc.GetSingleNodeValue(xmlnode, "title") == null ? "" : __xmlDoc.GetSingleNodeValue(xmlnode, "title"),
                        __xmlDoc.GetSingleNodeValue(xmlnode, "fid") == null ? 0 : Convert.ToInt32(__xmlDoc.GetSingleNodeValue(xmlnode, "fid")),
                        __xmlDoc.GetSingleNodeValue(xmlnode, "img") == null ? "" : __xmlDoc.GetSingleNodeValue(xmlnode, "img"),
                        __xmlDoc.GetSingleNodeValue(xmlnode, "tid") == null ? 0 : Convert.ToInt32(__xmlDoc.GetSingleNodeValue(xmlnode, "tid"))
                        ));
                 rowcount++;
            }
            if (__topicjson.ToString().EndsWith(","))
            {
                __topicjson.Remove(__topicjson.Length - 1, 1);
            }
            __topicjson.Append("]");
            return __topicjson.ToString();
        }
         


        #endregion


        #region 得到主题列表

        /// <summary>
        /// 获得聚合首页BBS版块(主题)列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetForumTopicList()
        {
            if (__topicList != null)
            {
                return __topicList;
            }

            //返回的记录数
            int topnumber = 10;
            XmlNode xmlnode = __xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Bbs").Item(0);

            if (__xmlDoc.GetSingleNodeValue(xmlnode, "Topnumber") == null)
            {
                topnumber = 10;
            }
            else
            {
                try
                {
                    topnumber = Convert.ToInt32(__xmlDoc.GetSingleNodeValue(xmlnode, "Topnumber").ToLower());
                }
                catch
                {
                    topnumber = 10;
                }
            }

            if (__xmlDoc.GetSingleNodeValue(xmlnode, "Showtype") != null)
            {
                __topicList = DatabaseProvider.GetInstance().GetWebSiteAggForumTopicList(__xmlDoc.GetSingleNodeValue(xmlnode, "Showtype").ToLower(), topnumber);
            }
            else
            {
                __topicList = DatabaseProvider.GetInstance().GetWebSiteAggForumTopicList("3", topnumber);
            }
            return __topicList;
        }

        #endregion


        #region 得到热门版块列表

        /// <summary>
        /// 得到热门版块列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetHotForumList(int topnumber)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable __forumList = cache.RetrieveObject("/Aggregation/HotForumList") as DataTable;
            if (__forumList == null)
            {
                __forumList = DatabaseProvider.GetInstance().GetWebSiteAggHotForumList(topnumber <= 0 ? 10 : topnumber);

                //声明新的缓存策略接口
                Discuz.Cache.ICacheStrategy ics = new Discuz.Forum.ForumCacheStrategy();
                ics.TimeOut = 5;
                cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/HotForumList", __forumList);
                cache.LoadDefaultCacheStrategy();
            }
            return __forumList;
        }

        #endregion

   
        #region 得到主题列表

        /// <summary>
        /// 获取论坛主题列表
        /// </summary>
        /// <param name="count">主题数</param>
        /// <param name="views">浏览量</param>
        /// <param name="forumid">版块ID</param>
        /// <param name="timetype">时间类型</param>
        /// <param name="ordertype">排序字段</param>
        /// <param name="isdigest">是否精化</param>
        /// <param name="onlyimg">是否包含附件</param>
        /// <returns></returns>
        public DataTable GetForumTopicList(int count, int views, int forumid, TopicTimeType timetype, TopicOrderType ordertype, bool isdigest, bool onlyimg)
        {
            return Focuses.GetTopicList(count, views, forumid, timetype, ordertype, isdigest, 5, onlyimg);
        }

       

        #endregion

        #region 得到用户列表

        /// <summary>
        /// 获取用户列表信息
        /// </summary>
        /// <param name="topnumber">获取用户数量</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public DataTable GetUserList(int topnumber, string orderby)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable __userList = cache.RetrieveObject("/Aggregation/Users_" + orderby + "List") as DataTable;
            if (__userList != null)
            {
                return __userList;
            }
            else
            {
                __userList = Users.GetUserList(topnumber, 1, orderby, "desc");

                //声明新的缓存策略接口
                Discuz.Cache.ICacheStrategy ics = new AggregationCacheStrategy();
                ics.TimeOut = 5;
                cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/Users_" + orderby + "List", __userList);
                cache.LoadDefaultCacheStrategy();
            }

            return __userList;
        }

        /// <summary>
        /// 获取指定版块下的最新回复
        /// </summary>
        /// <param name="fid">指定的版块</param>
        /// <param name="count">返回记录数</param>
        /// <returns></returns>
        public DataTable GetLastPostList(int fid, int count)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable __postList = cache.RetrieveObject("/Aggregation/lastpostList_" + fid) as DataTable;
            if (__postList != null)
            {
                return __postList;
            }
            else
            {
                __postList = DatabaseProvider.GetInstance().GetLastPostList(fid, count, Posts.GetPostTableName(), Forums.GetVisibleForum());

                //声明新的缓存策略接口
                Discuz.Cache.ICacheStrategy ics = new AggregationCacheStrategy();
                ics.TimeOut = 5;
                cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/lastpostList_" + fid, __postList);
                cache.LoadDefaultCacheStrategy();
            }

            return __postList;
        }

        #endregion
     
    }
}
