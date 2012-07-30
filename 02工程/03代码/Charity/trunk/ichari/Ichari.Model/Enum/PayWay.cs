using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Enum
{
    public enum PayWay
    {
        /// <summary>
        /// 账户余额
        /// </summary>
        [Display(Name = "账户余额")]
        Account = 0,
        /// <summary>
        /// 中国银联
        /// </summary>
        [Display(Name = "中国银联")]
        UnionPay = 1,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Display(Name = "支付宝")]
        AliPay = 2
    }
}
