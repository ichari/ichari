using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// 有关标签的计划任务
    /// </summary>
    public class TagsEvent : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            ForumTags.WriteHotTagsListForForumCacheFile(60);
            ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);
        }

        #endregion
    }
}
