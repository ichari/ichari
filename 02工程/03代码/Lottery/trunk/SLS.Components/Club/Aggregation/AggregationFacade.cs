using System;
using System.Text;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 聚合类对象Facade类
    /// </summary>
    public class AggregationFacade
    {
        private static AggregationData baseAggregationData;

        private static ForumAggregationData forumAggregationData;

        static AggregationFacade()
        {
            baseAggregationData = new AggregationData();

            forumAggregationData = new ForumAggregationData();

            //加载要通知的聚合数据对象
            AggregationDataSubject.Attach(baseAggregationData);

            AggregationDataSubject.Attach(forumAggregationData);
        }

        public static AggregationData BaseAggregation
        {
            get
            {
                return baseAggregationData;
            }
        }


        public static ForumAggregationData ForumAggregation
        {
              get
              {
                  return forumAggregationData;
              }
        }
    }
}
