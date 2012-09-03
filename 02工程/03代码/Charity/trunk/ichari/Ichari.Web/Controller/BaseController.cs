using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using Ichari.Uow;

namespace Ichari.Web
{
    [Authorize]
    public class BaseController : System.Web.Mvc.Controller
    {
        protected log4net.ILog _log;

        public BaseController()
        {
            _log = log4net.LogManager.GetLogger(this.GetType());
        }

        protected override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            string exceptionStr = string.Empty;
            try
            {
                if (filterContext.Exception != null)
                {
                    exceptionStr = filterContext.Exception.StackTrace;
                    throw filterContext.Exception;
                }
            }
            catch (HttpRequestValidationException)
            {
                if (filterContext.HttpContext.Request.UrlReferrer != null)
                {
                    System.Web.HttpContext.Current.Response.Redirect(filterContext.HttpContext.Request.UrlReferrer.ToString());
                }
            }
            catch (Exception ex)
            {

                ViewData["errMsg"] = ex.Message;
                ViewData["internalErrStackTrace"] = exceptionStr;
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = base.ViewData
                };
            }
            _log.Fatal("[Error]",filterContext.Exception);
            _log.Fatal(string.Format("[ErrorStackTrace]:{0}",exceptionStr));
        }

        

        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected Ichari.Model.UserInfo CurrentUser
        {
            get {
                return Session[SessionKey.User] as Ichari.Model.UserInfo;
            }
        }
        /// <summary>
        /// 显示操作提示信息
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="type">1-成功，2-失败，3-提醒</param>
        protected void RenderTip(string msg,int type = 1)
        {
            TempData[StaticKey.TmpGlobalInfo] = msg == null ? string.Empty : msg;
            TempData[StaticKey.TmpGlobalInfoType] = type;
        }
    }
}
