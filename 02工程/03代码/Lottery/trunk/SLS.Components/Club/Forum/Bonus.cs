using System;
using System.Text;
using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common.Generic;
using System.Data;

namespace Discuz.Forum
{
    /// <summary>
    /// 悬赏操作类
    /// </summary>
    public class Bonus
    {
        /// <summary>
        /// 添加悬赏日志
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="authorid">悬赏者Id</param>
        /// <param name="winerid">获奖者Id</param> 
        /// <param name="winnerName">获奖者用户名</param>
        /// <param name="postid">帖子Id</param>
        /// <param name="bonus">奖励积分</param>
        /// <param name="isbest">是否是最佳答案，0=不是,1=是较好的答案,2=最佳答案</param>
        public static void AddLog(int tid, int authorid, int winerid, string winnerName, int postid, int bonus, int isbest)
        {
            DatabaseProvider.GetInstance().AddBonusLog(tid, authorid, winerid, winnerName, postid, bonus, Scoresets.GetCreditsTrans(), isbest);
        }

        /// <summary>
        /// 结束悬赏并给分
        /// </summary>
        /// <param name="topicinfo">主题信息</param>
        /// <param name="userid">当前执行此操作的用户Id</param>
        /// <param name="postIdArray">帖子Id数组</param>
        /// <param name="winerIdArray">获奖者Id数组</param>
        /// <param name="winnerNameArray">获奖者的用户名数组</param>
        /// <param name="costBonusArray">奖励积分数组</param>
        /// <param name="valuableAnswerArray">有价值答案的pid数组</param>
        /// <param name="bestAnswer">最佳答案的pid</param>
        public static void CloseBonus(TopicInfo topicinfo, int userid, int[] postIdArray, int[] winerIdArray, string[] winnerNameArray, string[] costBonusArray, string[] valuableAnswerArray, int bestAnswer)
        {
            topicinfo.Special = 3;

            Topics.UpdateSpecial(topicinfo);//更新标志位为已结帖状态

            //开始给分和记录
            int winerId = 0, isbest = 0, postid = 0, bonus=0;
            string winnerName = string.Empty;
            for (int i = 0; i < winerIdArray.Length; i++)
            {
                winerId = winerIdArray[i];
                bonus = Utils.StrToInt(costBonusArray[i], 0);
                winnerName = winnerNameArray[i];
                postid = postIdArray[i];

                if (winerId > 0 && bonus > 0)
                {
                    Users.UpdateUserExtCredits(winerId, Scoresets.GetCreditsTrans(), bonus);
                }
                if (Utils.InArray(postid.ToString(), valuableAnswerArray))
                {
                    isbest = 1;
                }
                if (postid == bestAnswer)
                {
                    isbest = 2;
                }


                AddLog(topicinfo.Tid, topicinfo.Posterid, winerId, winnerName, postid, bonus, isbest);
            }
        }

        /// <summary>
        /// 获取指定主题的给分记录
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>悬赏日志集合</returns>
        public static List<BonusLogInfo> GetLogs(int tid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicBonusLogs(tid);
            List<BonusLogInfo> blis = new List<BonusLogInfo>();

            while (reader.Read())
            {
                BonusLogInfo bli = new BonusLogInfo();
                bli.Tid = Utils.StrToInt(reader["tid"], 0);
                bli.Answerid = Utils.StrToInt(reader["answerid"], 0);
                bli.Authorid = Utils.StrToInt(reader["authorid"], 0);
                bli.Answername = reader["answername"].ToString();
                bli.Bonus = Utils.StrToInt(reader["bonus"], 0);
                bli.Extid = Convert.ToByte(reader["extid"]);
                blis.Add(bli);
            }
            reader.Close();

            return blis;
        }

        /// <summary>
        /// 获取指定主题的给分记录
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public static Dictionary<int, BonusLogInfo> GetLogsForEachPost(int tid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicBonusLogsByPost(tid);
            Dictionary<int, BonusLogInfo> blis = new Dictionary<int, BonusLogInfo>();

            while (reader.Read())
            {
                BonusLogInfo bli = new BonusLogInfo();
                bli.Pid = Utils.StrToInt(reader["pid"], 0);
                bli.Bonus = Utils.StrToInt(reader["bonus"], 0);
                bli.Isbest = Utils.StrToInt(reader["isbest"], 0);
                bli.Extid = (byte)reader["extid"];
                blis[bli.Pid] = bli;
            }
            reader.Close();
            return blis;
        }
    }
}
