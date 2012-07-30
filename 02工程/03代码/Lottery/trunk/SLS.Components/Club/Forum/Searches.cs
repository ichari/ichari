using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin;

namespace Discuz.Forum
{
	/// <summary>
	/// 搜索操作类
	/// </summary>
	public class Searches
	{
        private static Regex regexForumTopics = new Regex(@"<ForumTopics>([\s\S]+?)</ForumTopics>");
        
        /// <summary>
		/// 创建搜索缓存
		/// </summary>
		/// <param name="cacheinfo">搜索缓存信息</param>
		/// <returns>搜索缓存id</returns>
		public static int CreateSearchCache(SearchCacheInfo cacheinfo)
		{
            return DatabaseProvider.GetInstance().CreateSearchCache(cacheinfo);
		}
		
		/// <summary>
		/// 根据指定条件进行搜索
		/// </summary>
		/// <param name="posttableid">帖子表id</param>
		/// <param name="userid">用户id</param>
		/// <param name="usergroupid">用户组id</param>
		/// <param name="keyword">关键字</param>
		/// <param name="posterid">发帖者id</param>
		/// <param name="type">搜索类型</param>
		/// <param name="searchforumid">搜索版块id</param>
		/// <param name="keywordtype">关键字类型</param>
		/// <param name="searchtime">搜索时间</param>
		/// <param name="searchtimetype">搜索时间类型</param>
		/// <param name="resultorder">结果排序方式</param>
		/// <param name="resultordertype">结果类型类型</param>
		/// <returns>如果成功则返回searchid, 否则返回-1</returns>
		public static int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, string type, string searchforumid, int keywordtype, int searchtime, int searchtimetype, int resultorder, int resultordertype)
		{
            if (posttableid == 0)
            { 
                posttableid = Utils.StrToInt(Posts.GetPostTableID(), 1);
            }
            return DatabaseProvider.GetInstance().Search(posttableid, userid, usergroupid, keyword, posterid, type, searchforumid, keywordtype, searchtime, searchtimetype, resultorder, resultordertype);
		}

		/// <summary>
		/// 获指定的搜索缓存的DataTable
		/// </summary>
		/// <param name="posttableid">帖子分表id</param>
		/// <param name="searchid">搜索缓存的searchid</param>
		/// <param name="pagesize">每页的记录数</param>
		/// <param name="pageindex">当前页码</param>
		/// <param name="topiccount">主题记录数</param>
		/// <param name="type">搜索类型</param>
		/// <returns>搜索缓存的DataTable</returns>
		public static DataTable GetSearchCacheList(int posttableid, int searchid, int pagesize, int pageindex, out int topiccount,string type)
		{
			topiccount = 0;
            DataTable dt = DatabaseProvider.GetInstance().GetSearchCache(searchid);
			if (dt.Rows.Count == 0)
			{
				return new DataTable();
			}
            string cachedidlist = dt.Rows[0][0].ToString();

            Match m;

            #region 搜索论坛

            m = regexForumTopics.Match(cachedidlist);

            if (m.Success)
            {
                string tids = GetCurrentPageTids(m.Groups[1].Value, out topiccount, pagesize, pageindex);

                if (tids == string.Empty)
                {
                    return new DataTable();
                }

                if (type == "digest")
                {
                    return DatabaseProvider.GetInstance().GetSearchDigestTopicsList(pagesize, tids);
                }
                if (type == "post")
                {
                    return DatabaseProvider.GetInstance().GetSearchPostsTopicsList(pagesize, tids, Posts.GetPostTableName());
                }
                else
                {
                    return DatabaseProvider.GetInstance().GetSearchTopicsList(pagesize, tids);
                }
            }
            #endregion

            return new DataTable();
		}

        /// <summary>
        /// 获得当前页的Tid列表
        /// </summary>
        /// <param name="tids">全部Tid列表</param>
        /// <returns></returns>
        private static string GetCurrentPageTids(string tids, out int topiccount, int pagesize, int pageindex)
        {
            string[] tid = Utils.SplitString(tids, ",");
            topiccount = tid.Length;
            int pagecount = topiccount % pagesize == 0 ? topiccount / pagesize : topiccount / pagesize + 1;
            if (pagecount < 1)
            {
                pagecount = 1;
            }
            if (pageindex  > pagecount)
            {
                pageindex = pagecount;
            }
            int startindex = pagesize * (pageindex - 1);
            StringBuilder strTids = new StringBuilder();
            for (int i = startindex; i < topiccount; i++)
            {
                if (i > startindex + pagesize)
                {
                    break;
                }
                else
                {
                    strTids.Append(tid[i]);
                    strTids.Append(",");
                }
            }
            strTids.Remove(strTids.Length - 1, 1);

            return strTids.ToString();
        }
		
		/// <summary>
		/// 开启全文索引
		/// </summary>
		public static void ConfirmFullTextEnable()
		{
            DatabaseProvider.GetInstance().ConfirmFullTextEnable();
		}

	}
}
