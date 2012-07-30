using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Web
{
    public static class StaticKey
    {
        #region AppSettings Key
        /// <summary>
        /// 站点的主域名，格式：ichari.com
        /// </summary>
        public const string AkSiteDomainName = "SiteDomainName";
        /// <summary>
        /// 前往支付跳转Url格式化字符串
        /// </summary>
        public const string PayUrlFormatter = "PayUrlFormatter";
        /// <summary>
        /// 静态服务器路径
        /// </summary>
        public const string StaticServer = "StaticServer";

        public const string AkSiteCookieName = "SiteCookieName";
        public const string AkUserSalt = "UserSalt";
        /// <summary>
        /// 彩票频道URL
        /// </summary>
        public const string AkLotteryUrl = "LotteryChannelUrl";
        /// <summary>
        /// 彩票频道api接口url格式化字符串
        /// </summary>
        public const string AkLotteryApiUrlFormatter = "LotteryApiUrlFormatter";
        #endregion

        /// <summary>
        /// 匿名用户ID
        /// </summary>
        public const int AnonymousUserId = -99;
        public const string VirtualProdDonation = "爱心捐赠虚拟产品";

        /// <summary>
        /// 全局提示信息
        /// </summary>
        public const string TmpGlobalInfo = "TmpGlobalInfo";
        /// <summary>
        /// 全局提示信息类型，成功or失败
        /// </summary>
        public const string TmpGlobalInfoType = "TmpGlobalInfoType";
    }

    public struct VirtualProduct
    {
        /// <summary>
        /// 爱心捐赠虚拟产品
        /// </summary>
        public static int Donation = -100;
    }

    
}
