using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Ichari.Model.Admin;

namespace Ichari.Admin
{
    [AuthorizationFilter]
    public class BaseController : System.Web.Mvc.Controller
    {
        //private log4net.ILog _log;

        public BaseController()
        {
            //_log = log4net.LogManager.GetLogger(this.GetType());
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
            catch (Exception ex)
            {
                //跳转错误页
                //throw ex;
                Response.Write(ex.Message);
                Response.Write(exceptionStr);
            }
            //_log.Fatal("[Error]",filterContext.Exception);
        }

        public string GetMenuCode(Controller c)
        {
            string controllerName = c.RouteData.GetRequiredString("controller");
            string actionName = c.RouteData.GetRequiredString("action");
            return string.Format("{0}_{1}", controllerName, actionName);
        }

        protected List<Actions> GetSubMenuList()
        {
            var uow = DependencyResolver.Current.GetService<Ichari.Uow.IAdminUow>();
            var menuCode = GetMenuCode(ControllerContext.Controller as Controller);
            var ca = uow.ActionsService.GetQueryList(t => t.MenuCode == menuCode).OrderByDescending(t => t.ID).FirstOrDefault();
            if (ca != null)
            {
                return uow.ActionsService.GetQueryList(t => t.ParentID == ca.ParentID).ToList();
            }
            return null;
        }

        protected void SetAlertMsg(string msg)
        {
            TempData[Ichari.Admin.StaticKey.TempGlobalInfo] = msg;
        }
    }
}
