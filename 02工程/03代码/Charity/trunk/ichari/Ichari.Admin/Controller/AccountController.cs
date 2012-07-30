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
    [AuthorizationFilter(SkipFilter = true)]
    public class AccountController : BaseController
    {
        private IAdminUow _uow;

        public AccountController() { }

        public AccountController(IAdminUow uow)
        {
            _uow = uow;
        }

        public ActionResult Login(string returnUrl)
        {
            var u = new SysUser();
            u.LogonName = "root";
            u.Password = "aaa";
            ViewData["returnUrl"] = returnUrl;
            return View(u);
            
        }

        [HttpPost]
        public ActionResult Login(SysUser model,string returnUrl)
        {
            if (
                model.LogonName.Equals(WebUtils.GetAppSettingValue(StaticKey.AppSuperUserName))
                && model.Password.ToMd5().Equals(WebUtils.GetAppSettingValue("SuperUserPwd"))
                )
            {
                Session[StaticKey.IsSuper] = true;
                Session[StaticKey.SessionUser] = new SysUser() { 
                    LogonName = model.LogonName
                };
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("index", "home");
            }
            else
            {
                var u = _uow.SysUserService.Get(t => t.LogonName == model.LogonName);
                if (u == null)
                {
                    TempData[StaticKey.TempGlobalError] = StaticKey.ErrorLogon;
                    return RedirectToAction("Login");
                }
                if(model.Password.ToMd5() != u.Password)
                {
                    TempData[StaticKey.TempGlobalError] = StaticKey.ErrorLogon;
                    return RedirectToAction("Login");
                }

                Session[StaticKey.SessionUser] = u;
                Session[StaticKey.SessionUserActionsList] =
                    _uow.GetUserActionList(u.Id).ToDictionary(t => t.ID);

                return RedirectToAction("index","home");

            }
            
        }

       

        public ActionResult Menu()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Menu(Actions model)
        {
            
            var old = _uow.ActionsService.Get(t => t.ID == model.ID);
            if (old == null)
            {
                _uow.ActionsService.Add(model);
            }
            else
            {
                old.MenuCode = model.MenuCode;
                old.MenuName = model.MenuName;
                old.ParentID = model.ParentID;
                old.IsMenu = model.IsMenu;
                old.IsAjax = model.IsAjax;
                old.ResourceKey = model.ResourceKey;
                old.IsNeedAuth = model.IsNeedAuth;
                old.SortNumber = model.SortNumber;
            }
            _uow.Commit();
            return RedirectToAction("Menu");
        }

        [AuthorizationFilter]
        public ActionResult ChangePwd()
        {
            var vm = Session[StaticKey.SessionUser];
            return View(vm);
        }

        public void LogOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("/account/login");
        }
    }
}
