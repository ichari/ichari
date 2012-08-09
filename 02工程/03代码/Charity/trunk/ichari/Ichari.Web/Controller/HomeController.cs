using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using Ichari.Uow;
using Ichari.Common;
using Ichari.Common.Helper;

using System.Net;
using System.Net.Mail;

namespace Ichari.Web
{
    public class HomeController : BaseController
    {
        private IChariUow _uow;

        public HomeController(IChariUow uow)
        {
            _uow = uow;
        }

        public ActionResult Index()
        {
            ViewData[SessionKey.VwCurrentNav] = "home";
            return View();
        }

        #region Static Pages
        public ActionResult Promotions()
        {
            ViewData[SessionKey.VwCurrentNav] = "promo";
            return View();
        }
        public ActionResult AboutUs()
        {
            ViewData[SessionKey.VwCurrentNav] = "about";
            return View();
        }
        public ActionResult CustomerSupport()
        {
            ViewData[SessionKey.VwCurrentNav] = "about";
            return View();
        }
        public ActionResult Careers()
        {
            ViewData[SessionKey.VwCurrentNav] = "about";
            return View();
        }
        public ActionResult Contact()
        {
            ViewData[SessionKey.VwCurrentNav] = "about";
            return View();
        }

        /// <summary>
        /// 项目介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult Intro()
        { 
            ViewData[SessionKey.VwCurrentNav] = "intro";
            return View();
        }
        #endregion

        #region Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(Model.UserNameEmail fpwd)
        {
            if (ModelState.IsValid)
            {
                Model.UserInfo usr = _uow.UserInfoService.Get(o => o.UserName == fpwd.UserName);
                if (usr != null && usr.UserName == fpwd.UserName && usr.Email == fpwd.Email)
                {
                    SmtpClient smtpm = new SmtpClient();
                    smtpm.Host = "mail.charilot.cc";
                    smtpm.Port = 1025;
                    smtpm.EnableSsl = false;
                    smtpm.Credentials = new NetworkCredential("jay@charilot.cc", "");
                    smtpm.DeliveryMethod = SmtpDeliveryMethod.Network;

                    MailAddress sender = new MailAddress("jay@charilot.cc", "集善网系统", Encoding.UTF8);
                    MailAddress receiver = new MailAddress(fpwd.Email);
                    MailMessage eMailMsg = new MailMessage(sender, receiver);
                    eMailMsg.Subject = "集善网忘记密码，密码重置系统邮件";
                    eMailMsg.Bcc.Add("jay@charilot.cc");
                    //eMailMsg.Bcc.Add("j.ct.sheu@gmail.com");
                    eMailMsg.IsBodyHtml = true;
                    Random rnd = new Random();
                    DateTime dt = DateTime.Now;
                    string resetLink = DataEncryption.EncryptAES(dt.ToBinary().ToString() + "?" + fpwd.Email + "?" + rnd.Next(100, 10000).ToString() + "?" + fpwd.UserName + "?" + dt.ToShortTimeString());
                    eMailMsg.Body = "尊敬的集善网客户: <br />您好! <br />系统已收到您的密码找回申请，请点击链接 <br /><p>" +
                        "http://" + Request.Url.Host + "/Account/ResetPassword?en=" + resetLink +
                        "</p>重设您的密码。<br /><br />为了您的安全，该邮件通知地址将在 48 小时后失效，谢谢合作。<br /><p>--------------------------------------------------<br />" +
                        "此邮件由系统发出，请勿直接回复! <br />集善网 版权所有(C) 2008-2009</p>";
                    eMailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    try
                    {
                        smtpm.Send(eMailMsg);
                    }
                    catch (Exception fre)
                    {
                        ModelState.AddModelError("", fre.Message);
                    }
                }
                else
                    ModelState.AddModelError("", "*UserName/Email not found");
            }
            else
                ModelState.AddModelError("", "Error 99");

            return View();
        }
        #endregion

        #region 公共头底部
        public ActionResult Top(string version)
        { 
            var vwName = "Top";
            if (!string.IsNullOrEmpty(version))
                vwName += "_v" + version;

            var ru = string.Empty;
            var q = Request.UrlReferrer;
            if (q != null)
            {
                var m = new Regex("^[\\?|&]returnUrl=([^&]*|$)");
                var mm = m.Match(q.Query);
                if (mm != null && mm.Groups.Count == 2) {
                    ru = mm.Groups[1].Value;
                }
            }

            if (!string.IsNullOrEmpty(ru)) {
                ViewData["returnUrl"] = ru;
            }
            if (ru.IndexOf("Account/Login", StringComparison.CurrentCultureIgnoreCase) > -1) {
                ViewData["returnUrl"] = null;
            }
            CheckCookie();
            
            return View(vwName);
        }
        private void CheckCookie()
        {
            
            var ck = Request.Cookies[Ichari.Common.WebUtils.GetAppSettingValue(StaticKey.AkSiteCookieName)];
            if (ck != null)
            {
                var userName = ck.Values["userName"];
                var usr = _uow.UserInfoService.Get(t => t.UserName == userName);
                if (usr == null)
                    return;
                var sign = ck.Values["sign"];
                if (sign == Ichari.Common.DataEncryption.HashString(string.Format("{0}{1}{2}", userName, usr.Password, Ichari.Common.WebUtils.GetAppSettingValue(StaticKey.AkUserSalt))))
                {
                    Session[SessionKey.User] = usr;
                }
            }
            else {
                Session.Remove(SessionKey.User);
            }
        }

        public ActionResult Bottom(string version)
        {
            var vwName = "Bottom";
            if (!string.IsNullOrEmpty(version))
                vwName += "_v" + version;

            return View(vwName);
        }

        /// <summary>
        /// 支持单位&合作媒体
        /// </summary>
        /// <returns></returns>
        [OutputCache(Location=System.Web.UI.OutputCacheLocation.Any,Duration=60)]
        public ActionResult Support()
        {
            var list = _uow.ContentService.GetListByTagsWithCache("zcdw_hzmt");
            ViewData["clist"] = list;
            return View();
        }
        #endregion

        #region 检查是否登录，动态更新top区域
        public ActionResult CheckLogin()
        {
            return View();
        }
        #endregion

    }
}
