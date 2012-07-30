using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Enum;

namespace Ichari.Pay.ViewModel
{
    public class PaymentViewModel : BaseViewModel
    {
        public Order OrderModel { get; set; }
        public Guid OrderId { get; set; }
        public string TradeNo { get; set; }
        public decimal Amount { get; set; }
        /// <summary>
        /// 支付请求来源
        /// </summary>
        public PaySource Source { get; set; }

        /// <summary>
        /// 当前商品的URL
        /// </summary>
        public string ProdUrl { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProdName { get; set; }

        public string FrontCallbackUrl { get; set; }

        public string BackCallbackUrl { get; set; }

        public PayWay PayWayType { get; set; }
    }
}
