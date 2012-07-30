using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Enum
{
    public enum OrderState
    {
        /// <summary>
        /// 交易失败
        /// </summary>
        [Display(Name = "交易失败")]
        Failure = -20,
        /// <summary>
        /// 取消订单
        /// </summary>
        [Display(Name = "取消订单")]
        Cancel = -10,
        /// <summary>
        /// 新订单
        /// </summary>
        [Display(Name = "新订单")]
        Create = 0,
        /// <summary>
        /// 已付款
        /// </summary>
        [Display(Name = "已付款")]
        Paid = 10,
        /// <summary>
        /// 银联扣款成功，针对爱心零钱支付
        /// </summary>
        [Display(Name = "银联扣款成功")]
        UnionPaySuccess = 11,
        /// <summary>
        /// 已发货
        /// </summary>
        [Display(Name = "已发货")]
        Freighted=15,
        /// <summary>
        /// 已完成
        /// </summary>
        [Display(Name = "已完成")]
        Compolete = 20
    }
}
