using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Config;

namespace Discuz.Forum
{
    public class Urls
    {
        #region aspxrewrite 配置

        /// <summary>
        /// 设置关于showforum页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowForumAspxRewrite(int forumid, int pageid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 0)
                {
                    return "showforum-" + forumid + "-" + pageid + config.Extname;
                }
                else
                {
                    return "showforum-" + forumid + config.Extname;
                }
            }
            else
            {
                if (pageid > 0)
                {
                    return "showforum.aspx?forumid=" + forumid + "&page=" + pageid;
                }
                else
                {
                    return "showforum.aspx?forumid=" + forumid;
                }
            }

        }

        /// <summary>
        /// 设置关于showtopic页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowTopicAspxRewrite(int topicid, int pageid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 0)
                {
                    return "showtopic-" + topicid + "-" + pageid + config.Extname;
                }
                else
                {
                    return "showtopic-" + topicid + config.Extname;
                }
            }
            else
            {
                if (pageid > 0)
                {
                    return "showtopic.aspx?topicid=" + topicid + "&page=" + pageid;
                }
                else
                {
                    return "showtopic.aspx?topicid=" + topicid;
                }
            }
        }

        public static string ShowDebateAspxRewrite(int topicid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            if (config.Aspxrewrite == 1)
            {
                return string.Format("showdebate-{0}{1}", topicid, config.Extname);
            }
            else
            {
                return string.Format("showdebate.aspx?topicid={0}", topicid);
            }
        }

        /// <summary>
        /// 设置关于showbonus页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowBonusAspxRewrite(int topicid, int pageid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 0)
                {
                    return "showbonus-" + topicid + "-" + pageid + config.Extname;
                }
                else
                {
                    return "showbonus-" + topicid + config.Extname;
                }
            }
            else
            {
                if (pageid > 0)
                {
                    return "showbonus.aspx?topicid=" + topicid + "&page=" + pageid;
                }
                else
                {
                    return "showbonus.aspx?topicid=" + topicid;
                }
            }
        }


        public static string UserInfoAspxRewrite(int userid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                return "userinfo-" + userid + config.Extname;
            }
            else
            {
                return "userinfo.aspx?userid=" + userid;
            }

        }

        /// <summary>
        /// 设置关于userinfo页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string RssAspxRewrite(int forumid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                return "rss-" + forumid + config.Extname;
            }
            else
            {
                return "rss.aspx?forumid=" + forumid;
            }

        }


        /// <summary>
        /// 设置关于showgoods页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowGoodsAspxRewrite(int goodsid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                return "showgoods-" + goodsid + config.Extname;

            }
            else
            {
                return "showgoods.aspx?goodsid=" + goodsid;
            }
        }


        /// <summary>
        /// 设置关于showgoods页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowGoodsListAspxRewrite(int categoryid, int pageid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 0)
                {
                    return "showgoodslist-" + categoryid + "-" + pageid + config.Extname;
                }
                else
                {
                    return "showgoodslist-" + categoryid + config.Extname;
                }
            }
            else
            {
                if (pageid > 0)
                {
                    return "showgoodslist.aspx?categoryid=" + categoryid + "&page=" + pageid;
                }
                else
                {
                    return "showgoodslist.aspx?categoryid=" + categoryid;
                }
            }
        }
        #endregion

    }
}
