using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Ichari.Uow;
using Ichari.Admin.ViewModel;
using Ichari.Model;
using Ichari.Model.Admin;
using Ichari.Model.Enum;
using Ichari.Common;
using Ichari.Common.Extensions;

namespace Ichari.Admin
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderController : BaseController
    {
        private IChariUow _uow;

        public OrderController() { }

        public OrderController(IChariUow uow)
        {
            _uow = uow;
        }

        public ActionResult Detail(string tradeNo)
        {
            var order = _uow.OrderService.Get(t => t.TradeNo == tradeNo);
            if (order == null)
                return RedirectToAction("Error");
            var model = new OrderViewModel();
            model.Order = order;
            model.PayLogList = _uow.PayLogService.GetList(t => t.OrderId == order.OrderId).ToList();
            return View(model);
        }
    }
}
