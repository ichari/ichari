using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Notify
{
    /// <summary>
    /// 期次状态
    /// </summary>
    public class IssueState
    {
        /// <summary>
        /// 0-未开始
        /// </summary>
        public const string NotStart = "0";
        /// <summary>
        /// 1-已开新期
        /// </summary>
        public const string NewIssue = "1";
        /// <summary>
        /// 2-暂停奖期
        /// </summary>
        public const string PauseIssue = "2";
        /// <summary>
        /// 3-截止投注
        /// </summary>
        public const string Stop = "3";
        /// <summary>
        /// 4-摇出奖号
        /// </summary>
        public const string Drawing = "4";
        /// <summary>
        /// 5-兑奖中
        /// </summary>
        public const string Open = "5";
        /// <summary>
        /// 6-结期兑奖
        /// </summary>
        public const string End = "6";
    }
}
