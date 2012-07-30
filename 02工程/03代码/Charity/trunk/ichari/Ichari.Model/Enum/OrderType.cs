using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Model.Enum
{
    public enum OrderType
    {
        /// <summary>
        /// 爱心捐赠产生的订单
        /// </summary>
        Donation = 1,
        /// <summary>
        /// 银联爱心零钱产生的订单
        /// </summary>
        UnionChangeOfCare = 2
    }
}
