using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Notify
{
    /// <summary>
    /// 奖期查询/通知返回的XML格式定义
    /// </summary>
	public class SunlotXmlDefin
	{
        //属性名称
        public const string LotoId = "lotoid";
        public const string Issue = "issue";
        public const string StartTime = "starttime";
        public const string EndTime = "endtime";
        public const string Status = "status";
        public const string BonusCode = "bonuscode";

        public const string Money = "money";
        public const string BonusCls = "bonuscls";
        public const string TicketId = "ticketid";
        //tag名称
        public const string TagBody = "body";

        public const string TagIssue = "issue";
        public const string TagBonusQuery = "bonusquery";
        public const string TagBonusItem = "bonusItem";

        //开奖查询
        public const string TagLotId = "lotId";
        public const string TagLotIssue = "lotIssue";
        public const string TagSt = "startTime";
        public const string TagEt = "endTime";
        public const string TagWinNumber = "kjCode";
        public const string TagSaleAmount = "sale";
        public const string TagBonusBalance = "bonusBalance";
        public const string TagWinName = "winName";
        public const string TagWinMoney = "winMoney";
        public const string TagWinCount = "winCount";
	}
}
