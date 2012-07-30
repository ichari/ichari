using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Data;
using Discuz.Common;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    public class StatEvent : IEvent
    {
        #region IEvent Members

        void IEvent.Execute(object state)
        {
            //更新昨日发帖，并更新最高日发帖
            DatabaseProvider.GetInstance().UpdateYesterdayPosts(Posts.GetPostTableID());



            //更新缓存
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Statistics");

            //清空onlinetime表的thismonth
            if (DateTime.Today.Day == 1)
            {
                //重置onlinetime表的thismonth(清零)
                DatabaseProvider.GetInstance().ResetThismonthOnlineTime();
                //更新统计
                DatabaseProvider.GetInstance().UpdateStatVars("onlines", "lastupdate", "0");
            }
        }

        #endregion
    }
}
