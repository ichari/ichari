using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Sync
{
    public class StaticConfig
    {
        /// <summary>
        /// 是否启用同步数据功能，启用：1，禁用：0
        /// </summary>
        public const string AkIsEnableSyncToLottery = "IsEnableSyncToLottery";
        /// <summary>
        /// 彩票频道api接口url格式化字符串
        /// </summary>
        public const string AkLotteryApiUrlFormatter = "LotteryApiUrlFormatter";
        /// <summary>
        /// 主站api接口url格式化字符串
        /// </summary>
        public const string AkIchariUrlFormatter = "IchariUrlFormatter";
    }

    public static class LotteryApiConfig
    {
        public const string RegisterUser = "reguser";
        public const string UpdatePassword = "updatepwd";
    }
}
