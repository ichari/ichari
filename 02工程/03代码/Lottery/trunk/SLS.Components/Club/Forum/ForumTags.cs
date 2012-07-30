using System;
using System.Text;
using System.IO;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using System.Data;
using Discuz.Data;
using Discuz.Forum.ScheduledEvents;
using Discuz.Cache;

namespace Discuz.Forum
{
    /// <summary>
    /// 论坛标签(Tag)操作类
    /// </summary>
    public class ForumTags
    {
        /// <summary>
        /// 论坛热门标签缓存文件(json格式)文件路径
        /// </summary>
        public const string ForumHotTagJSONCacheFileName = "cache\\tag\\hottags_forum_cache_json.txt";
        /// <summary>
        /// 论坛热门标签缓存文件(jsonp格式)文件路径
        /// </summary>
        public const string ForumHotTagJSONPCacheFileName = "cache\\tag\\hottags_forum_cache_jsonp.txt";
      
        /// <summary>
        /// 写入热门标签缓存文件(json格式)
        /// </summary>
        /// <param name="count">数量</param>
        public static void WriteHotTagsListForForumCacheFile(int count)
        {
            string filename = EventManager.RootPath + ForumHotTagJSONCacheFileName;
            List<TagInfo> tags = GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, tags, string.Empty, true);
        }

        /// <summary>
        /// 写入热门标签缓存文件(jsonp格式)
        /// </summary>
        /// <param name="count"></param>
        public static void WriteHotTagsListForForumJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + ForumHotTagJSONPCacheFileName;
            List<TagInfo> tags = GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, tags, "forumhottag_callback", true);
        }

        /// <summary>
        /// 写入主题标签缓存文件
        /// </summary>
        /// <param name="tagsArray">标签数组</param>
        /// <param name="topicid">主题Id</param>
        public static void WriteTopicTagsCacheFile(int topicid)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append("/cache/topic/magic/");
            dir.Append((topicid / 1000 + 1).ToString());
            dir.Append("/");
            string filename = Utils.GetMapPath(dir.ToString() + topicid.ToString() + "_tags.config");
            List<TagInfo> tags = GetTagsListByTopic(topicid);
            Tags.WriteTagsCacheFile(filename, tags, string.Empty, false);
        }

        /// <summary>
        /// 获取主题所包含的Tag
        /// </summary>
        /// <param name="topicid">主题Id</param>
        /// <returns>List</returns>
        public static List<TagInfo> GetTagsListByTopic(int topicid)
        {
            List<TagInfo> tags = new List<TagInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetTagsListByTopic(topicid);

            while (reader.Read())
            {
                tags.Add(Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();

            return tags;
        }

        /// <summary>
        /// 获取60个论坛热门标签
        /// </summary>
        /// <returns>List</returns>
        public static List<TagInfo> GetHotTagsListForForum()
        {
            return GetHotTagsListForForum(60);
        }

        /// <summary>
        /// 获取论坛热门标签
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns>List</returns>
        public static List<TagInfo> GetHotTagsListForForum(int count)
        {
            List<TagInfo> tags = new List<TagInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetHotTagsListForForum(count);

            while (reader.Read())
            {
                tags.Add(Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();

            return tags;
        }    

        /// <summary>
        /// 热门标签
        /// </summary>
        /// <param name="count">标签数</param>
        /// <returns>TagInfo</returns>
        public static TagInfo[] GetCachedHotForumTags(int count)
        {
            TagInfo[] tags;
            DNTCache cache = DNTCache.GetCacheService();
            tags = cache.RetrieveObject("/Tag/Hot-" + count) as TagInfo[];
            if (tags != null)
            {
                return tags;
            }

#if NET1
            System.Collections.ArrayList tagList = new System.Collections.ArrayList();
#else
            List<TagInfo> tagList = new List<TagInfo>();
#endif
            

            IDataReader reader = DatabaseProvider.GetInstance().GetHotTagsListForForum(count);

            while (reader.Read())
            {
                tagList.Add(Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();
#if NET1
            tags = (TagInfo[])tagList.ToArray(typeof(TagInfo));
#else
            tags = tagList.ToArray();
#endif


            Discuz.Cache.ICacheStrategy ics = new ForumCacheStrategy();
            ics.TimeOut = 360;
            cache.LoadCacheStrategy(ics);
            cache.AddObject("/Tag/Hot-" + count, tags);
            cache.LoadDefaultCacheStrategy();    

            return tags;
        }
        /// <summary>
        /// 删除主题标题
        /// </summary>
        /// <param name="topicid">主题ID</param>
        public static void DeleteTopicTags(int topicid)
        {
            DatabaseProvider.GetInstance().DeleteTopicTags(topicid);
        }



        public static void CreateTopicTags(string[] tagArray, int topicid, int userid, string curdatetime)
        {
            DatabaseProvider.GetInstance().CreateTopicTags(string.Join(" ", tagArray), topicid, userid, curdatetime);
            ForumTags.WriteTopicTagsCacheFile(topicid);
        }
    }
}
