using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Ichari.Uow;
using Ichari.Model.Admin;
using Ichari.Common;
using Ichari.Common.Extensions;

namespace Ichari.Admin
{
    public class HomeController : BaseController
    {
        private IChariUow _uow;

        public HomeController() { }

        public HomeController(IChariUow uow)
        {
            _uow = uow;
        }

        public ActionResult Index()
        {
            var vm = new ViewModel.IndexViewModel();
            var c = _uow.LoveChangeService.GetQueryList().Sum(t => t.Amount);
            var d = _uow.OrderService.GetQueryList(t => t.OrderType == (int)Ichari.Model.Enum.OrderType.Donation
                                    && t.Status >= (int)Ichari.Model.Enum.OrderState.Paid)
                                    .Sum(t => t.Total);
            vm.AllDonationAmount = c + d;
           
            vm.ChangeOfCareCount = _uow.LoveChangeService.GetQueryList().Count();
            vm.ChangeOfCareAmt = c;
            vm.DonationCount = _uow.OrderService.GetQueryList(t => t.OrderType == (int)Ichari.Model.Enum.OrderType.Donation
                                    && t.Status >= (int)Ichari.Model.Enum.OrderState.Paid).Count();
            vm.DonationAmt = d;
            return View(vm);
        }
    }
}
