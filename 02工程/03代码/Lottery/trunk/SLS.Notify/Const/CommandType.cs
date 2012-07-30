using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Notify
{
    /// <summary>
    /// 出票商接口命令分类
    /// </summary>
	public class CommandType
	{
        /// <summary>
        /// 奖期通知
        /// </summary>
        public const string LotterySchema = "1000";
        /// <summary>
        /// 出票状态通知
        /// </summary>
        public const string TicketStatus = "1001";
        /// <summary>
        /// 兑奖通知
        /// </summary>
        public const string LotteryOpen = "1002";
        /// <summary>
        /// 奖期查询
        /// </summary>
        public const string IssueQuery = "2000";
        /// <summary>
        /// 兑奖查询
        /// </summary>
        public const string OpenIssueQuery = "2013";
	}
}
