using System;
using System.Text;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Config.Provider;

namespace Discuz.Forum
{
	/// <summary>
	/// Feed操作类
	/// </summary>
	public class Feeds
	{
        private static GeneralConfigInfo config = GeneralConfigs.GetConfig();

		/// <summary>
		/// 获得论坛最新的20个主题的Rss描述
		/// </summary>
		/// <param name="ttl">TTL数值</param>
		/// <returns>Rss描述</returns>
		public static string GetRssXml(int ttl)
		{
		
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

			string str = cache.RetrieveObject("/RSS/Index") as string;
			if (str != null)
			{
				return str;
			}
			ForumInfo[] forumlist = Forums.GetForumList();
			UserGroupInfo guestinfo = UserGroups.GetUserGroupInfo(7);
			StringBuilder sbforumlist = new StringBuilder();//不允许游客访问的板块Id列表
			StringBuilder sbRss = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");

			foreach (ForumInfo f in forumlist)
			{
				if (f.Allowrss == 0)
				{
					sbforumlist.AppendFormat(",{0}", f.Fid);
				}
				else
				{
					if (f.Viewperm == null || f.Viewperm == string.Empty)
					{
						//板块权限设置为空，按照用户组权限走，RSS仅检查游客权限
						if (guestinfo.Allowvisit == 0)
						{
							sbforumlist.AppendFormat(",{0}", f.Fid);
						}
					}
					else
					{
						if (!Utils.InArray("7", f.Viewperm, ","))
						{
							sbforumlist.AppendFormat(",{0}", f.Fid);
						}
					}
				}
			}

			if (sbforumlist.Length > 0)
				sbforumlist.Remove(0,1);
            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + ("/").ToLower();		
            	
			sbRss.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
			sbRss.Append("<rss version=\"2.0\">\r\n");
			sbRss.Append("  <channel>\r\n");
			sbRss.Append("    <title>");
            sbRss.Append(Utils.HtmlEncode(config.Forumtitle));
			sbRss.Append("</title>\r\n");
			sbRss.Append("    <link>");
			sbRss.Append(forumurl);
			sbRss.Append("</link>\r\n");
			sbRss.Append("    <description>Latest 20 threads</description>\r\n");
			sbRss.Append("    <copyright>Copyright (c) ");
            sbRss.Append(Utils.HtmlEncode(config.Forumtitle));
			sbRss.Append("</copyright>\r\n");
			sbRss.Append("    <generator>");
			sbRss.Append("Discuz!NT");
			sbRss.Append("</generator>\r\n");
			sbRss.Append("    <pubDate>");
			//sbRss.Append(DateTime.Now.ToUniversalTime().ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"));
			sbRss.Append(DateTime.Now.ToString("r"));
			sbRss.Append("</pubDate>\r\n");
			sbRss.AppendFormat("    <ttl>{0}</ttl>\r\n", ttl.ToString());


            //声明新的缓存策略接口
            Discuz.Cache.ICacheStrategy ics = new RssCacheStrategy();
            ics.TimeOut = ttl;
            cache.LoadCacheStrategy(ics);

            IDataReader reader = DatabaseProvider.GetInstance().GetNewTopics(sbforumlist.ToString());
			
			if (reader != null)
			{
				while (reader.Read())
				{
					sbRss.Append("    <item>\r\n");
					sbRss.Append("      <title>");
					sbRss.Append(Utils.HtmlEncode(reader["title"].ToString().Trim()));
					sbRss.Append("</title>\r\n");
					sbRss.Append("    <description><![CDATA[");
					if (reader["message"].ToString().IndexOf("[hide]") > -1)
						sbRss.Append("***内容隐藏***");
					else
						sbRss.Append(Utils.HtmlEncode(Utils.GetSubString(UBB.ClearUBB(reader["message"].ToString()), 200, "......")));
					sbRss.Append("]]></description>\r\n");
					sbRss.Append("      <link>");
					sbRss.Append(Utils.HtmlEncode(forumurl));

                    if (config.Aspxrewrite == 1)
                    {
                        sbRss.Append("showtopic-");
                        sbRss.Append(reader["tid"].ToString());
                        sbRss.Append(config.Extname);
                    }
                    else 
                    {
                        sbRss.Append("showtopic.aspx?topicid=");
                        sbRss.Append(reader["tid"].ToString());
                    }
                    
					sbRss.Append("</link>\r\n");
					sbRss.Append("      <category>");
					sbRss.Append(Utils.HtmlEncode(reader["name"].ToString().Trim()));
					sbRss.Append("</category>\r\n");
					sbRss.Append("      <author>");
					sbRss.Append(Utils.HtmlEncode(reader["poster"].ToString().Trim()));
					sbRss.Append("</author>\r\n");
					sbRss.Append("      <pubDate>");
					sbRss.Append(Utils.HtmlEncode(Convert.ToDateTime(reader["postdatetime"]).ToString("r").Trim()));
					sbRss.Append("</pubDate>\r\n");
					sbRss.Append("    </item>\r\n");
				}
				reader.Close();
			}
			else
			{
				sbRss.Length = 0;
				sbRss.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
				sbRss.Append("<Rss>Error</Rss>\r\n");
				cache.AddObject("/RSS/Index", sbRss.ToString());
				cache.LoadDefaultCacheStrategy();
				return sbRss.ToString();
			}
			

			sbRss.Append("  </channel>\r\n");
			sbRss.Append("</rss>");

			cache.AddObject("/RSS/Index", sbRss.ToString());
			cache.LoadDefaultCacheStrategy();
		
			return sbRss.ToString();
		}
		
		/// <summary>
		/// 获得指定版块最新的20个主题的Rss描述
		/// </summary>
		/// <param name="ttl">TTL数值</param>
		/// <param name="fid">版块id</param>
		/// <returns>Rss描述</returns>
		public static string GetForumRssXml(int ttl, int fid)
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

			string str = cache.RetrieveObject("/RSS/Forum" + fid) as string;
			if(str != null)
			{
				return str;
			}

            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + ("/").ToLower();//GeneralConfigs.GetConfig().Forumurl;
            //if (!forumurl.EndsWith("/"))
            //{
            //    forumurl = forumurl + "/";
            //}
			ForumInfo forum = Forums.GetForumInfo(fid);
			if (forum == null)
			{
				
				return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified forum not found</Rss>\r\n";
			}

			if (forum.Viewperm == null || forum.Viewperm == string.Empty)
			{
				//板块权限设置为空，按照用户组权限走，RSS仅检查游客权限
				UserGroupInfo guestinfo = UserGroups.GetUserGroupInfo(7);
				if (guestinfo.Allowvisit == 0)
				{
					return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Guest Denied</Rss>\r\n";
				}
			}
			else
			{
				if (!Utils.InArray("7", forum.Viewperm, ","))
				{
					return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Guest Denied</Rss>\r\n";
				}
				
			}
			StringBuilder sbRss = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
			sbRss.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
			sbRss.Append("<rss version=\"2.0\">\r\n");
			sbRss.Append("  <channel>\r\n");
			sbRss.Append("    <title>");
            sbRss.Append(Utils.HtmlEncode(config.Forumtitle));
			sbRss.Append(" - ");
			sbRss.Append(Utils.HtmlEncode(forum.Name));
			sbRss.Append("</title>\r\n");
			sbRss.Append("    <link>");
			sbRss.Append(forumurl);

            if (config.Aspxrewrite == 1)
            {
                sbRss.Append("showforum-");
                sbRss.Append(fid.ToString());
                sbRss.Append(config.Extname);
            }
            else
            {
                sbRss.Append("showforum.aspx?forumid=");
                sbRss.Append(fid.ToString());
            }

			sbRss.Append("</link>\r\n");
			sbRss.Append("    <description>Latest 20 threads</description>\r\n");
			sbRss.Append("    <copyright>Copyright (c) ");
            sbRss.Append(Utils.HtmlEncode(config.Forumtitle));
			sbRss.Append("</copyright>\r\n");
			sbRss.Append("    <generator>");
			sbRss.Append("Discuz!NT");
			sbRss.Append("</generator>\r\n");
			sbRss.Append("    <pubDate>");
			//sbRss.Append(DateTime.Now.ToUniversalTime().ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"));
			sbRss.Append(DateTime.Now.ToString("r"));
			sbRss.Append("</pubDate>\r\n");
			sbRss.AppendFormat("    <ttl>{0}</ttl>\r\n", ttl.ToString());

            IDataReader reader = DatabaseProvider.GetInstance().GetForumNewTopics(fid);
			if (reader != null)
			{
				while (reader.Read())
				{
					sbRss.Append("    <item>\r\n");
					sbRss.Append("      <title>");
					sbRss.Append(Utils.HtmlEncode(reader["title"].ToString().Trim()));
					sbRss.Append("</title>\r\n");
                    sbRss.Append("      <description><![CDATA[");
					if (reader["message"].ToString().IndexOf("[hide]") > -1)
						sbRss.Append("***内容隐藏***");
					else
						sbRss.Append(Utils.HtmlEncode(Utils.GetSubString(UBB.ClearUBB(reader["message"].ToString()), 200, "......")));
					sbRss.Append("]]></description>\r\n");
					sbRss.Append("      <link>");
					sbRss.Append(Utils.HtmlEncode(forumurl));

                    if (config.Aspxrewrite == 1)
                    {
                        sbRss.Append("showtopic-");
                        sbRss.Append(reader["tid"].ToString());
                        sbRss.Append(config.Extname);
                    }
                    else
                    {
                        sbRss.Append("showtopic.aspx?topicid=");
                        sbRss.Append(reader["tid"].ToString());
                    }

					sbRss.Append("</link>\r\n");
					sbRss.Append("      <category>");
					sbRss.Append(Utils.HtmlEncode(forum.Name));
					sbRss.Append("</category>\r\n");
					sbRss.Append("      <author>");
					sbRss.Append(Utils.HtmlEncode(reader["poster"].ToString().Trim()));
					sbRss.Append("</author>\r\n");
					sbRss.Append("      <pubDate>");
					sbRss.Append(Utils.HtmlEncode(Convert.ToDateTime(reader["postdatetime"]).ToString("r").Trim()));
					sbRss.Append("</pubDate>\r\n");
					sbRss.Append("    </item>\r\n");
				}
				reader.Close();
			}
			sbRss.Append("  </channel>\r\n");
			sbRss.Append("</rss>");


            Discuz.Cache.ICacheStrategy ics = new RssCacheStrategy();
            ics.TimeOut = ttl;
            cache.LoadCacheStrategy(ics);
			cache.AddObject("/RSS/Forum" + fid, sbRss.ToString());
			cache.LoadDefaultCacheStrategy();
		
			return sbRss.ToString();
		}


        /// <summary>
        /// 获得百度论坛收录协议xml
        /// </summary>
        /// <param name="ttl"></param>
        /// <returns></returns>
        public static string GetBaiduSitemap(int ttl)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
          
            string str = cache.RetrieveObject("/Sitemap/Baidu") as string;
            if (str != null)
            {
                return str;
            }
            ForumInfo[] forumlist = Forums.GetForumList();
            UserGroupInfo guestinfo = UserGroups.GetUserGroupInfo(7);
            StringBuilder sbforumlist = new StringBuilder();//不允许游客访问的板块Id列表
            StringBuilder sitemapBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");

            foreach (ForumInfo f in forumlist)
            {
                if (f.Allowrss == 0)
                {
                    sbforumlist.AppendFormat(",{0}", f.Fid);
                }
                else
                {
                    if (f.Viewperm == null || f.Viewperm == string.Empty)
                    {
                        //板块权限设置为空，按照用户组权限走，RSS仅检查游客权限
                        if (guestinfo.Allowvisit == 0)
                        {
                            sbforumlist.AppendFormat(",{0}", f.Fid);
                        }
                    }
                    else
                    {
                        if (!Utils.InArray("7", f.Viewperm, ","))
                        {
                            sbforumlist.AppendFormat(",{0}", f.Fid);
                        }
                    }
                }
            }

            if (sbforumlist.Length > 0)
                sbforumlist.Remove(0, 1);

            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + ("/").ToLower();
            ShortUserInfo master = Users.GetShortUserInfo(BaseConfigs.GetFounderUid);
            string masteremail = "";
            if (master != null)
            {
                masteremail = master.Email;
            }

            sitemapBuilder.Append("<document xmlns:bbs=\"http://www.baidu.com/search/bbs_sitemap.xsd\">\r\n");
            sitemapBuilder.Append("  <webSite>");
            sitemapBuilder.Append(forumurl);
            sitemapBuilder.Append("</webSite>\r\n");
            sitemapBuilder.Append("  <webMaster>");
            sitemapBuilder.Append(masteremail);
            sitemapBuilder.Append("</webMaster>\r\n");
            sitemapBuilder.Append("  <updatePeri>");
            sitemapBuilder.Append(config.Sitemapttl);
            sitemapBuilder.Append("</updatePeri>\r\n");
            sitemapBuilder.Append("  <updatetime>");
            sitemapBuilder.Append(DateTime.Now.ToString("r"));
            sitemapBuilder.Append("</updatetime>\r\n");
            sitemapBuilder.Append("  <version>");
            sitemapBuilder.Append("Discuz!NT " + Utils.GetAssemblyVersion());
            sitemapBuilder.Append("</version>\r\n");

            IDataReader reader = DatabaseProvider.GetInstance().GetSitemapNewTopics(sbforumlist.ToString());

            if (reader != null)
            {
                while (reader.Read())
                {
                    sitemapBuilder.Append("    <item>\r\n");
                    sitemapBuilder.Append("      <link>");
                    sitemapBuilder.Append(Utils.HtmlEncode(forumurl));
                    if (config.Aspxrewrite == 1)
                    {
                        sitemapBuilder.Append("showtopic-");
                        sitemapBuilder.Append(reader["tid"].ToString());
                        sitemapBuilder.Append(config.Extname);
                    }
                    else
                    {
                        sitemapBuilder.Append("showtopic-");
                        sitemapBuilder.Append(reader["tid"].ToString());
                    }

                    sitemapBuilder.Append("</link>\r\n");
                    sitemapBuilder.Append("      <title>");
                    sitemapBuilder.Append(Utils.HtmlEncode(reader["title"].ToString().Trim()));
                    sitemapBuilder.Append("</title>\r\n");
                    sitemapBuilder.Append("      <pubDate>");
                    sitemapBuilder.Append(Utils.HtmlEncode(reader["postdatetime"].ToString().Trim()));
                    sitemapBuilder.Append("</pubDate>\r\n");
                    sitemapBuilder.Append("      <bbs:lastDate>");
                    sitemapBuilder.Append(reader["lastpost"].ToString());
                    sitemapBuilder.Append("</bbs:lastDate>\r\n");
                    sitemapBuilder.Append("      <bbs:reply>");
                    sitemapBuilder.Append(reader["replies"].ToString().Trim());
                    sitemapBuilder.Append("</bbs:reply>\r\n");
                    sitemapBuilder.Append("      <bbs:hit>");
                    sitemapBuilder.Append(reader["views"].ToString().Trim());
                    sitemapBuilder.Append("</bbs:hit>\r\n");
                    sitemapBuilder.Append("      <bbs:boardid>");
                    sitemapBuilder.Append(reader["fid"].ToString().Trim());
                    sitemapBuilder.Append("</bbs:boardid>\r\n");
                    sitemapBuilder.Append("      <bbs:pick>");
                    sitemapBuilder.Append(reader["digest"].ToString().Trim());
                    sitemapBuilder.Append("</bbs:pick>\r\n");
                    sitemapBuilder.Append("    </item>\r\n");
                }
                reader.Close();
            }
            else
            {
                sitemapBuilder.Length = 0;
                sitemapBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                sitemapBuilder.Append("<document>Error</document>\r\n");

                //声明新的缓存策略接口
                Discuz.Cache.ICacheStrategy ics = new SitemapCacheStrategy();
                ics.TimeOut = ttl * 60;
                cache.LoadCacheStrategy(ics);
                cache.AddObject("/Sitemap/Baidu", sitemapBuilder.ToString());
                cache.LoadDefaultCacheStrategy();
                return sitemapBuilder.ToString();
            }


            sitemapBuilder.Append("</document>");

            cache.AddObject("/Sitemap/Baidu", sitemapBuilder.ToString());
            cache.LoadDefaultCacheStrategy();

            return sitemapBuilder.ToString();
        }

	}
}
