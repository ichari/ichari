using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Configuration;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Ichari.Model.Admin;

namespace Ichari.Admin
{
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public bool SkipFilter { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IsLogon(filterContext);
        }

        private void IsLogon(ActionExecutingContext context)
        {
            if (SkipFilter)
                return;
            //是否超级管理员
            if (context.HttpContext.Session[StaticKey.IsSuper] != null)
                return;

            var u = context.HttpContext.Session[StaticKey.SessionUser] as SysUser;

            string ca = GetActionName((Controller)context.Controller);


            var uow = DependencyResolver.Current.GetService<Ichari.Uow.IAdminUow>();
            var a = uow.ActionsService.Get(t => t.MenuCode == ca.ToLower());

            if (u == null)
            {
                string rawUrl = context.HttpContext.Request.Url.PathAndQuery;
                //                
                #region 判断是否ajax方法
                bool isAjax = false;
                string ajaxParam = context.RequestContext.HttpContext.Request.Headers["X-Requested-With"];
                if (!string.IsNullOrEmpty(ajaxParam))
                {
                    if (ajaxParam.IndexOf("XMLHttpRequest") > -1)
                    {
                        isAjax = true;
                    }
                }
                #endregion                

                if (!isAjax)
                {
                    if (a != null && a.IsAjax)
                    {
                        context.Result = new System.Web.Mvc.EmptyResult();
                        context.HttpContext.Response.Write(StaticKey.InfoTimeOut);
                    }
                    else
                    {
                        context.Result = new RedirectResult(
                            string.Format("/Account/Login?returnUrl={0}", context.HttpContext.Server.UrlEncode(rawUrl))
                            );
                    }
                }
                else
                {
                    context.Result = new System.Web.Mvc.EmptyResult();
                    context.HttpContext.Response.Write(StaticKey.InfoTimeOut);
                }
            }
            else
            {                
                if (a != null)
                {
                    if (!a.IsNeedAuth)
                    {
                        return;
                    }
                    if (!CheckAuth(a.ID))
                    {
                        if (a.IsAjax)
                        {
                            context.Result = new System.Web.Mvc.EmptyResult();
                            context.HttpContext.Response.Write(StaticKey.InfoNoAuth);
                        }
                        else
                        {
                            context.Controller.TempData["auth_url"] = context.HttpContext.Request.RawUrl;
                            if (context.HttpContext.Request.UrlReferrer != null)
                            {
                                context.Controller.TempData["auth_referrer"] = context.HttpContext.Request.UrlReferrer.AbsoluteUri;
                            }
                            context.Result = new System.Web.Mvc.RedirectResult("/Home/Error", true);
                        }
                    }
                }
            }
        }

        private string GetActionName(Controller c)
        {
            string controllerName = c.RouteData.GetRequiredString("controller");
            string actionName = c.RouteData.GetRequiredString("action");
            return string.Format("{0}_{1}", controllerName, actionName);
        }
        /// <summary>
        /// 校验是否具有访问权限
        /// </summary>
        /// <param name="ca">controller_action 格式</param>
        /// <returns></returns>
        private bool CheckAuth(int actionId)
        {
            if (HttpContext.Current.Session == null) return false;
            if (HttpContext.Current.Session[StaticKey.SessionIsSuper] != null)
                return true;

            var user = HttpContext.Current.Session[StaticKey.SessionUser] as SysUser;
            if (user == null) return false;
            
            if (HttpContext.Current.Session[StaticKey.SessionUserActionsList] == null) return false;

            HashSet<int> permissions = (HashSet<int>)HttpContext.Current.Session[StaticKey.SessionUserActionsList];
            return permissions.Contains(actionId);
        }

        
    }
}
