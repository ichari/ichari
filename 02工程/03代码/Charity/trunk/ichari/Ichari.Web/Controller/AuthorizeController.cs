using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ichari.Web.Controller
{
    public class AuthorizeController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Index(string userName,string password)
        {
            if (string.IsNullOrEmpty(userName))
                return;
            if (string.IsNullOrEmpty(password))
                return;
            if (userName.ToLower().Equals("admin") && password.Equals("8u9i7y")) {
                System.Web.Security.FormsAuthentication.SetAuthCookie(userName, false);
                if (Request.QueryString["returnUrl"] != null) {
                    Response.Redirect(Request.QueryString["returnUrl"]);
                }
                Response.Redirect("/");
            }
            Response.Redirect("/authorize");
        }
    }
}
