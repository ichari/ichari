using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Web
{
    public static class SessionKey
    {
        public const string ID = "UserID";
        public const string User = "User";
        public const string Captcha = "iCaptcha";
        public const string DonationId = "DonaID";
        public const string DonationAmt = "DonaAmt";
        public const string UPOPChari = "UPOPChari";
        public const string DrawId = "DrawingID";
        public const string PrizeId = "WinPrzID";
        /// <summary>
        /// 领取电子优惠劵的ID
        /// </summary>
        public const string DeliveryFreeCard = "DeliveryFreeCard";

        #region ViewData Key
        /// <summary>
        /// 当前导航
        /// </summary>
        public const string VwCurrentNav = "VwCurrentNav";
        public const string VwUserMenu = "VwUserMenu";
        #endregion
    }

   

    
}
