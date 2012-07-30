using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ichari.Pay
{
    public class BaseController : System.Web.Mvc.Controller
    {
        public log4net.ILog _log;

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
            catch (Exception ex)
            {
                //跳转错误页
                Response.Write(ex.Message);
                Response.Write("<br />");
                Response.Write(exceptionStr);
            }
            _log.Fatal("[Error]",filterContext.Exception);
        }

        
    }
}
