using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ichari.Uow;
using System.Web.Mvc;
using Ichari.Model;
using Ichari.Admin.ViewModel;
using Ichari.Model.Admin;
using Ichari.Model.Dto;

namespace Ichari.Admin
{
    public class CustomerController : BaseController
    {
        private IChariUow _uow;
        private IAdminUow _auow;

        public CustomerController() { }

        public CustomerController(IChariUow uow, IAdminUow auow)
        {
            _uow = uow;
            _auow = auow;
        }
        
        

        public ActionResult Users(string UserName, int? pageIndex)
        {
            var cvm = new CustomerViewModel();
            
            if (pageIndex.HasValue)
                cvm.PageIndex = pageIndex.Value;
            if (string.IsNullOrWhiteSpace(UserName))
            {
                var clist = _uow.UserInfoService.GetQueryList().OrderBy(o => o.Id);
                cvm.CustomerList = new Common.Helper.PageList<UserInfo>(clist, cvm.PageIndex, cvm.PageCount);
            }
            else
            {
                var clist = _uow.UserInfoService.GetQueryList(o => o.UserName.Contains(UserName)).OrderBy(o => o.Id);
                cvm.CustomerList = new Common.Helper.PageList<UserInfo>(clist, cvm.PageIndex, cvm.PageCount);
            }
            cvm.SubMenuList = GetSubMenuList();

            return View(cvm);
        }

        public ActionResult EditUser(string id)
        {
            long usr_id;
            if (long.TryParse(id, out usr_id))
            {
                UserInfo usr = _uow.UserInfoService.Get(o => o.Id == usr_id);
                if (usr != null)
                {
                    CustomerEditModel cem = new CustomerEditModel();
                    cem.Id = usr.Id;
                    cem.UserName = usr.UserName;
                    cem.TrueName = usr.TrueName;
                    cem.Email = usr.Email;
                    cem.IdentityCardNo = usr.IdentityCardNo;
                    cem.Tel = usr.Tel;
                    cem.Phone = cem.Phone;

                    return View(cem);
                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult EditUser(CustomerEditModel usr)
        {
            if (ModelState.IsValid)
            {
                if (usr.Id != 0)
                {
                    UserInfo old_usr = _uow.UserInfoService.Get(o => o.Id == usr.Id);
                    if (old_usr != null && old_usr.Id == usr.Id && !string.IsNullOrWhiteSpace(usr.UserName))
                    {
                        old_usr.UserName = usr.UserName;
                        old_usr.Email = usr.Email;
                        old_usr.TrueName = usr.TrueName;
                        old_usr.IdentityCardNo = usr.IdentityCardNo;
                        old_usr.Tel = usr.Tel;
                        old_usr.Phone = usr.Phone;
                        old_usr.UpdateTime = DateTime.Now;
                        _uow.Commit();
                        return RedirectToAction("Users"); ;
                    }
                }
            }
            ModelState.AddModelError("", "Error Editing User");
            return View(usr);
        }
        #region 账户管理
        public ActionResult Account(int? pageIndex,string userName)
        {
            var model = new BaseViewModel<UserAccountDto>();
            if(pageIndex.HasValue)
                model.PageIndex = pageIndex.Value;
            var list = _uow.GetAccountList(userName);
            model.PageList = new Common.Helper.PageList<UserAccountDto>(list, model.PageIndex, model.PageCount);
            return View(model);
        }

        public ActionResult AccountLog(int id,int? pageIndex)
        {
            var model = new BaseViewModel<AccountLog>();
            if(pageIndex.HasValue)
                model.PageIndex = pageIndex.Value;
            var acc = _uow.GetUserAccountDto(id);
            ViewData["account"] = acc;
            var list = _uow.AccountLogService.GetQueryList(t => t.UserId == acc.UserId).OrderByDescending(t => t.CreateTime).ThenByDescending(t => t.LogId);
            model.PageList = new Common.Helper.PageList<AccountLog>(list, model.PageIndex, model.PageCount);
            return View(model);
        }
        #endregion

    }
}
