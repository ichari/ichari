using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Configuration;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Ichari.Model;

namespace Ichari.Web
{
    public class LogonFilterAttribute : ActionFilterAttribute
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
            //是否登录
            var u = context.HttpContext.Session[SessionKey.User] as UserInfo;           

            if (u == null)
            {
                string rawUrl = context.HttpContext.Request.Url.PathAndQuery;
                //                

                context.Result = new RedirectResult(
                            string.Format("/Account/Login?returnUrl={0}", context.HttpContext.Server.UrlEncode(rawUrl))
                );
            }
        }
    }
}
