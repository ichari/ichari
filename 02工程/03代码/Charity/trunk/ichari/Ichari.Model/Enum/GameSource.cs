using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Enum
{
    /// <summary>
    /// 抽奖来源
    /// </summary>
    public enum GameSource
    {
        /// <summary>
        /// 爱心零钱
        /// </summary>
        [Display(Name = "银联爱心零钱")]
        ChangeOfCare = 1,
        /// <summary>
        /// 集善网爱心捐赠
        /// </summary>
        [Display(Name = "集善网爱心捐赠")]
        IchariDonation = 2
    }
}
