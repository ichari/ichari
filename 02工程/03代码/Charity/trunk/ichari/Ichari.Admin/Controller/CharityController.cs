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
    public class CharityController : BaseController
    {
        private IChariUow _uow;

        public CharityController() { }

        public CharityController(IChariUow uow)
        {
            _uow = uow;
        }

        #region 爱心零钱
        public ActionResult Change(int? pageIndex,string unionOrder,string trueName)
        {
            var vm = new LoveChangeViewModel();
            if (pageIndex.HasValue)
                vm.PageIndex = pageIndex.Value;

            var list = _uow.LoveChangeService.GetQueryList();
            if (!string.IsNullOrEmpty(unionOrder))
                list = list.Where(t => t.UnionOrder == unionOrder);
            if (!string.IsNullOrEmpty(trueName))
                list = list.Where(t => t.TrueName == trueName);

            vm.RecordList = new Common.Helper.PageList<LoveChange>(list.OrderByDescending(t => t.CreateTime), vm.PageIndex, vm.PageCount);

            return View(vm);
        }
        #endregion

        #region 爱心捐赠
        public ActionResult Donation(int? pageIndex, string trueName,int? status)
        {
            var vm = new DonationViewModel();
            vm.QueryModel = new BaseQueryModel();
            vm.QueryModel.TrueName = trueName;
            vm.QueryModel.Status = status;
            if (pageIndex.HasValue)
                vm.PageIndex = pageIndex.Value;

            var list = _uow.OrderService.GetQueryList(t => t.OrderType == (int)OrderType.Donation);
            //if (!string.IsNullOrEmpty(userName))
            //{
            //    var u = _uow.UserInfoService.Get(t => t.UserName == userName);
            //    if (u != null)
            //    {
            //        list = list.Where(t => t.UserId == u.Id);
            //    }
            //}
            if (!string.IsNullOrEmpty(trueName))
                list = list.Where(t => t.TrueName == trueName);
            if (status.HasValue)
                list = list.Where(t => t.Status == status.Value);

            vm.RecordList = new Common.Helper.PageList<Order>(list.OrderByDescending(t => t.CreateTime), vm.PageIndex, vm.PageCount);
            vm.OrderStatus = Ichari.Common.Helper.EnumHelper.EnumToSelectList<OrderState>(status, true);
            //vm.SubMenuList = GetSubMenuList();
            return View(vm);
        }
        #endregion
    }
}
