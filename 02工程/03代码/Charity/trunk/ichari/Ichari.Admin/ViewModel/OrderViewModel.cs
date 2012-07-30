using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;


namespace Ichari.Admin.ViewModel
{
    public class OrderViewModel : BaseViewModel
    {
        public Order Order { get; set; }
        /// <summary>
        /// 支付记录列表
        /// </summary>
        public List<PayLog> PayLogList { get; set; }
    }
}
