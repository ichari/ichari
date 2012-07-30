using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Enum
{
    /// <summary>
    /// 支付请求来源
    /// </summary>
    public enum PaySource
    {
        /// <summary>
        /// 爱心捐赠
        /// </summary>
        [Display(Name = "爱心捐赠")]
        Donation = 1
        /*
         * Hotel = 2
         * */
    }
}
