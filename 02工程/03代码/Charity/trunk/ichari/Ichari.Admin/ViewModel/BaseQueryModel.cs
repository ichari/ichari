using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Admin.ViewModel
{
    public class BaseQueryModel
    {
        public int? Status { get; set; }
        public string TrueName { get; set; }

        /// <summary>
        /// 集善网订单流水号
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 银联订单流水号
        /// </summary>
        public string UnionOrderNo { get; set; }
    }
}
