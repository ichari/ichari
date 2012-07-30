using System;
using Ichari.Uow;
using System.Web.Mvc;
using Ichari.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ichari.Web.ViewModel;
using Ichari.Model;

namespace Ichari.Web
{
    [LogonFilter]
    public class AccountController : BaseController
    {
        private IChariUow _uow;

        public AccountController(IChariUow uow)
        {
            _uow = uow;
        }

        #region Captcha related
        [LogonFilter(SkipFilter = true)]
        public string ChangeCaptcha()
        {
            return "<img id=\"capImg\" src=\"\\Account\\CaptchaImg?" + new Random().Next(100).ToString() + "\" alt=\"验证码\" style=\"vertical-align:top;\" />";
        }
        [LogonFilter(SkipFilter = true)]
        public ActionResult CaptchaImg()
        {
            Captcha cap = new Captcha();
            string code = cap.CreateVerifyCode();
            FileContentResult img = null;

            Session[SessionKey.Captcha] = null;
            Session[SessionKey.Captcha] = code.ToLower();
            img = this.File(cap.GetJPEG(code).GetBuffer(), "image/Jpeg");

            return img;
        }
        #endregion

        public ActionResult Index()
        {
            return RedirectToAction("Summary");
        }
        
        #region Donation Related
        /// <summary>
        /// 银联爱心零钱捐赠
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionDonations(int? pageIndex)
        {   
            
            ChariGameViewModel cgvm = new ChariGameViewModel();

            if(pageIndex.HasValue)
                cgvm.PageIndex = pageIndex.Value;
            ViewData[SessionKey.VwUserMenu] = "uniondonation";
            var userId = base.CurrentUser.Id;
            
            var list = _uow.LoveChangeService.GetQueryList(o => o.UserId == userId).OrderByDescending(o => o.CreateTime);
            cgvm.PageList = new Common.Helper.PageList<LoveChange>(list, cgvm.PageIndex, cgvm.PageCount);
            return View(cgvm);
        }

        /// <summary>
        /// 绑定银联爱心零钱捐赠订单号
        /// </summary>
        /// <param name="cgvm"></param>
        /// <returns></returns>
        [HttpPost]        
        public ActionResult UnionDonations(ChariGameViewModel cgvm)
        {
            ViewData[SessionKey.VwUserMenu] = "uniondonation";
            LoveChange old_lc = _uow.LoveChangeService.Get(o => o.UnionOrder == cgvm.DonationId && o.Amount == cgvm.DonationAmount);
            UserInfo usr = base.CurrentUser;
            if (old_lc == null) { 
                ModelState.AddModelError("NotExistDonation", "无法帮定捐赠单");
                var list = _uow.LoveChangeService.GetQueryList(o => o.UserId == usr.Id).OrderByDescending(o => o.CreateTime);
                cgvm.PageList = new Common.Helper.PageList<LoveChange>(list, cgvm.PageIndex, cgvm.PageCount);
                return View(cgvm);
            }
            if (old_lc.UserId != null) { 
                ModelState.AddModelError("BindedDonation", "此捐赠单已经绑定");
                var list = _uow.LoveChangeService.GetQueryList(o => o.UserId == usr.Id).OrderByDescending(o => o.CreateTime);
                cgvm.PageList = new Common.Helper.PageList<LoveChange>(list, cgvm.PageIndex, cgvm.PageCount);
                return View(cgvm);
            }

            old_lc.UserId = usr.Id;
            if(string.IsNullOrWhiteSpace(old_lc.TrueName))
                old_lc.TrueName = usr.TrueName;
            _uow.Commit();
            base.RenderTip(null);
            return RedirectToAction("UnionDonations");
            
        }

        /// <summary>
        /// 集善网爱心捐赠
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDonations(int? pageIndex)
        {
            ViewData[SessionKey.VwUserMenu] = "mydonation";
            var cu = base.CurrentUser;
            var vm = new BaseViewModel<Order>();
            if (pageIndex.HasValue)
                vm.PageIndex = pageIndex.Value;
            var list = _uow.OrderService.GetQueryList(t => t.UserId == cu.Id).OrderByDescending(t => t.CreateTime);
            vm.PageList = new Common.Helper.PageList<Order>(list, vm.PageIndex, vm.PageCount);
            return View(vm);
        }

        /// <summary>
        /// 我的抽奖记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDrawings(int? pageIndex,bool? isWin)
        { 
            ViewData[SessionKey.VwUserMenu] = "draw";
            ViewData["isWin"] = isWin;
            var cu = base.CurrentUser;
            var vm = new BaseViewModel<Drawings>();
            if (pageIndex.HasValue)
                vm.PageIndex = pageIndex.Value;
            var list = _uow.DrawingsService.GetQueryList(t => t.UserId == cu.Id);
            if (isWin.HasValue)
                list = list.Where(t => t.IsWinner == isWin.Value);

            vm.PageList = new Common.Helper.PageList<Drawings>(list.OrderByDescending(t => t.CreateTime), vm.PageIndex, vm.PageCount);
            
            return View(vm);
        }

        public ActionResult DrawDetail(int id)
        { 
            var usr = base.CurrentUser;
            //check draw id
            var draw = _uow.DrawingsService.Get(t => t.Id == id);
            if (draw == null)
                throw new IchariException(string.Format("未找到相应的抽奖记录：drawId={0}",id));
            if(draw.UserId != usr.Id)
                throw new IchariException(string.Format("抽奖记录异常：drawId={0}",id));
            if(draw.IsHandled == null || !draw.IsHandled.Value)
                throw new IchariException(string.Format("尚未登记领奖信息：drawId={0}",id));

            var vm = new DrawDetailViewModel();
            vm.User = usr;
            vm.Draw = draw;
            if (draw.Source == (int)Ichari.Model.Enum.GameSource.ChangeOfCare) { 
                var order = _uow.LoveChangeService.Get(t => t.UnionOrder == draw.OrderNo);
                if(order == null)
                    throw new IchariException(string.Format("未找到相应的捐赠记录：TradeNo={0}",draw.OrderNo));
                vm.LcModel = order;
            }
            else if (draw.Source == (int)Ichari.Model.Enum.GameSource.IchariDonation) {
                var order = _uow.OrderService.Get(t => t.TradeNo == draw.OrderNo);
                if(order == null)
                    throw new IchariException(string.Format("未找到相应的捐赠记录：TradeNo={0}",draw.OrderNo));
                vm.OrderModel = order;
            }
            vm.DrawDetail = _uow.GetDrawDetail(id);
            if (draw.CardId.HasValue) {
                vm.FreeCard = _uow.FreeCardService.Get(t => t.Id == draw.CardId.Value);
            }
            return View(vm);
        }

        /// <summary>
        /// 删除未支付的订单
        /// </summary>
        /// <param name="tradeNo"></param>
        /// <returns></returns>
        public ActionResult DeleteOrder(string tn)
        {
            var u = base.CurrentUser;
            var order = _uow.OrderService.Get(t => t.TradeNo == tn && t.UserId == u.Id);
            if (order == null)
            {
                base.RenderTip("订单不存在:" + tn,2);
                return RedirectToAction("mydonations");
            }
            if (order.Status != (int)Ichari.Model.Enum.OrderState.Create)
            {
                base.RenderTip("订单状态错误:" + order.Status.ToString(), 2);
                return RedirectToAction("mydonations");
            }
            var details = order.OrderDetail.ToList();
            foreach (var d in details) {
                _uow.OrderDetailService.Delete(d);
            }
            _uow.OrderService.Delete(order);
            _uow.Commit();
            base.RenderTip(string.Empty);
            return RedirectToAction("mydonations");
        }
        #endregion

        #region Registration
        [LogonFilter(SkipFilter = true)]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [LogonFilter(SkipFilter = true)]
        public ActionResult Register(Model.UserInfo usr)
        {
            if (Session[SessionKey.Captcha] == null || Session[SessionKey.Captcha].ToString() != usr.Captcha)
            {
                ModelState.AddModelError("Captcha", "验证码不正确");
                return View(usr);
            }
            if (ModelState.IsValid)
            {
                var isExistUserName = _uow.UserInfoService.GetQueryList(t => t.UserName == usr.UserName).Count() > 0;
                if (isExistUserName) { 
                    ModelState.AddModelError("UserName", "用户名已使用");
                    return View(usr);
                }
                //var isExistEmail = string.IsNullOrEmpty(usr.Email);
                var isExistEmail = !string.IsNullOrEmpty(usr.Email) && _uow.UserInfoService.GetQueryList(t => t.Email == usr.Email).Count() > 0;
                
                if (isExistEmail) { 
                    ModelState.AddModelError("Email",string.Format("此邮箱已被注册：{0}",usr.Email));
                    return View(usr);
                }

                string pwdplain = usr.Password;
                usr.RegFrom = 1;
                usr.CreateTime = DateTime.Now;
                usr.UpdateTime = DateTime.Now;
                usr.Password = DataEncryption.HashString(pwdplain);
                //sync to lottery                   
                var uid = new Ichari.Sync.UserSync().RegistToLottery(usr.UserName, pwdplain, usr.Email, usr.TrueName); //SyncUserToLottery(usr.UserName,pwdplain,usr.Email,usr.TrueName);
                usr.LotteryUserId = uid;
                _uow.UserInfoService.Add(usr);
                _uow.Commit();
                //Registration on both end successful, auto login
                var ckDomain = string.Format(".{0}",WebUtils.GetAppSettingValue(StaticKey.AkSiteDomainName));
                DataEncryption.SaveToCookies(DataEncryption.EncryptAES(usr.Id.ToString()),usr.UserName, uid,ckDomain,false);
                Session[SessionKey.User] = usr;
                if (Request.QueryString["r"] == "ClaimPrize")
                {
                    return RedirectToAction("ClaimPrizeStep2", "Charity");
                }
                return RedirectToAction("UserInfo", "Account");
            }

            return View(usr);
        }
        

        public string CheckUserName(string id)
        {
            if (_uow.UserInfoService.Get(o => o.UserName == id) == null)
                return "用户名可使用";
            return "用户名已被使用";
        }

        [LogonFilter(SkipFilter = true)]
        public JsonResult CheckUn(string un)
        {
            var u = _uow.UserInfoService.Get(t => t.UserName == un);
            if (u != null) {
                return Json(new ReturnJsonResult { 
                    IsSuccess = false,
                    ErrorMessage = "此用户名已经被使用"                    
                });
            }
            return Json(new ReturnJsonResult { 
                    IsSuccess = true                   
                });
        }
        #endregion

        #region User Information
        public ActionResult Summary()
        {            
            ViewData[SessionKey.VwUserMenu] = "summary";
            return View(Session[SessionKey.User]);
        }
        [HttpPost]
        public ActionResult Summary(Model.UserInfo usr)
        {
            ViewData[SessionKey.VwUserMenu] = "summary";
            Model.UserInfo susr = (Model.UserInfo)Session[SessionKey.User];
            if (susr == null)
                return RedirectToAction("Login", "Account");

            var old_usr = _uow.UserInfoService.Get(o => o.Id == susr.Id);
            if (old_usr == null || old_usr.Id != susr.Id)
            {
                return RedirectToAction("Error", "Home", new { em = "101", ef = "Summary" });
            }
            old_usr.Email = usr.Email == null ? "" : usr.Email;
            old_usr.Phone = usr.Phone == null ? "" : usr.Phone;
            old_usr.Tel = usr.Tel == null ? "" : usr.Tel;
            _uow.Commit();
            Session[SessionKey.User] = old_usr;

            return View(usr);
        }
        
        public ActionResult UserInfo()
        {
            if (Session[SessionKey.User] == null)
                return RedirectToAction("Login");

            Model.UserInfo usr = new Model.UserInfo();
            usr = (Model.UserInfo)Session[SessionKey.User];
            if (!string.IsNullOrWhiteSpace(usr.TrueName) && !string.IsNullOrWhiteSpace(usr.IdentityCardNo) && !string.IsNullOrWhiteSpace(usr.Phone))
            {
                return RedirectToAction("Summary");
            }
            Model.UserAdditionalInfo uai = new Model.UserAdditionalInfo();
            uai.UserName = usr.UserName;
            uai.TrueName = usr.TrueName;
            uai.IdentityCardNo = usr.IdentityCardNo;
            uai.Phone = usr.Phone;
            return View(uai);
        }
        [HttpPost]
        public ActionResult UserInfo(Model.UserAdditionalInfo usr)
        {
            if (Session[SessionKey.User] == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                //save changes and commit to db
                Model.UserInfo susr = (Model.UserInfo)Session[SessionKey.User];
                var old_usr = _uow.UserInfoService.Get(u => u.Id == susr.Id);
                if (old_usr == null || (old_usr.Id != susr.Id))
                    return RedirectToAction("Error", "Home");
                old_usr.TrueName = usr.TrueName == null ? "" : usr.TrueName;
                old_usr.IdentityCardNo = usr.IdentityCardNo == null ? "" : usr.IdentityCardNo;
                old_usr.Phone = usr.Phone == null ? "" : usr.Phone;
                old_usr.UpdateTime = DateTime.Now;
                _uow.Commit();
                //susr = old_usr;
                Session[SessionKey.User] = old_usr;
                return RedirectToAction("Summary");
            }
            else
                ModelState.AddModelError("", "Error 101");

            return View();
        }
        #endregion

        #region Login / Logout
        [LogonFilter(SkipFilter = true)]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [LogonFilter(SkipFilter = true)]
        public ActionResult Login(Model.UserInfo usr)
        {                
            var x = _uow.UserInfoService.Get(m => m.UserName == usr.UserName);
            string hpwd = DataEncryption.HashString(usr.Password);
            if ((x != null) && (x.UserName == usr.UserName) && (x.Password == hpwd))
            {               
                
                // user authenticated login and store in session
                Session[SessionKey.User] = x;
                string returnUrl = Request["returnUrl"];

                var str = Ichari.Common.DataEncryption.HashString(string.Format("{0}{1}{2}",x.UserName,x.Password,Ichari.Common.WebUtils.GetAppSettingValue("UserSalt")));
                var ckDomain = string.Format(".{0}",WebUtils.GetAppSettingValue(StaticKey.AkSiteDomainName));
                if (Request.IsLocal)
                    ckDomain = null;
                DataEncryption.SaveToCookies(str, usr.UserName,x.LotteryUserId,ckDomain,usr.IsSaveCookie);

                if (Request.IsAjaxRequest()) {
                    return Json(new ReturnJsonResult() { 
                        IsSuccess = true
                    });
                }
                
                
                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);
                else if (string.IsNullOrWhiteSpace(x.TrueName) || string.IsNullOrWhiteSpace(x.IdentityCardNo) || string.IsNullOrWhiteSpace(x.Phone))
                    return RedirectToAction("UserInfo");
                return RedirectToAction("Summary");
            }
            else
                ModelState.AddModelError("", "用户/密码错误");

            if (Request.IsAjaxRequest()) {
                    return Json(new ReturnJsonResult() { 
                        IsSuccess = false
                    });
            }
            return View(usr);
        }
        [LogonFilter(SkipFilter = true)]
        public void Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            //清除cookie
            System.Web.HttpCookie ck = new System.Web.HttpCookie(Ichari.Common.WebUtils.GetAppSettingValue(StaticKey.AkSiteCookieName));
            ck.Domain = string.Format(".{0}",Ichari.Common.WebUtils.GetAppSettingValue(StaticKey.AkSiteDomainName));
            if (Request.IsLocal)
                ck.Domain = null;
            ck.Path = "/";
            ck.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(ck);
            Response.Redirect("/home");
        }
        #endregion

        #region Password related
        public ActionResult ResetPassword()
        {
            string str_ver = Request.QueryString["en"];
            if (str_ver == null)
                return RedirectToAction("Index", "Home", new { ver = "nil" });
            string str_msg = "";
            try
            {
                str_msg = DataEncryption.DecryptAES(str_ver);
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { err = "888" });
            }
            char[] sp = new char[] { '?' };
            string[] str_var = str_msg.Split(sp);
            if(str_var.Length != 5)
                return RedirectToAction("Index", "Home", new { len = "!5" });
            DateTime dt = DateTime.FromBinary(long.Parse(str_var[0]));
            if (dt < DateTime.Now.AddDays(-2))
                return RedirectToAction("Index", "Home", new { tm = "expired" });
            if (dt.ToShortTimeString() != str_var[4])
                return RedirectToAction("Index", "Home", new { st = "different" });
            
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(Model.UserResetPassword resPwd)
        {
            string str_ver = Request.QueryString["en"];
            if (str_ver == null)
                return RedirectToAction("Index", "Home", new { ver = "nil" });
            string str_msg = DataEncryption.DecryptAES(str_ver);
            char[] sp = new char[] { '?' };
            string[] str_var = str_msg.Split(sp);
            if(str_var.Length != 5)
                return RedirectToAction("Index", "Home", new { len = "!5" });
            DateTime dt = DateTime.FromBinary(long.Parse(str_var[0]));
            if (dt < DateTime.Now.AddDays(-2))
                return RedirectToAction("Index", "Home", new { tm = "expired" });
            if (dt.ToShortTimeString() != str_var[4])
                return RedirectToAction("Index", "Home", new { st = "different" });
            string uemail = str_var[1];
            string uname = str_var[3];
            Model.UserInfo old_usr = _uow.UserInfoService.Get(m => m.UserName == uname && m.Email == uemail);
            if (old_usr == null || old_usr.Email != str_var[1] || old_usr.UserName != str_var[3])
                return RedirectToAction("Error", "Home", new { err = "User_Not_Found" });
            //if (usr.UserName != resPwd.UserName || usr.Email != resPwd.Email)
            //    ModelState.AddModelError("", "UserName/Email not verified");
            else
            {
                old_usr.Password = DataEncryption.HashString(resPwd.NewPassword);
                old_usr.UpdateTime = DateTime.Now;
                _uow.Commit();
                Session.Abandon();
                Session.RemoveAll();
                return RedirectToAction("Login", "Account", new { mg = "Password_reset_successfully" });
            }

           // return View();
        }

        public ActionResult ChangePassword()
        {
            if (Session[SessionKey.User] == null)
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.AbsoluteUri });
            ViewData[SessionKey.VwUserMenu] = "changepwd";
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(Model.UserChangePassword ucp)
        {
            if (Session[SessionKey.User] == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "" });

            Model.UserInfo usr = (Model.UserInfo)Session[SessionKey.User];
            Model.UserInfo old_usr = _uow.UserInfoService.Get(o => o.Id == usr.Id);
            if (old_usr == null || old_usr.Id != usr.Id)
                return RedirectToAction("Error", "Home", "155");
            if (old_usr.Password != DataEncryption.HashString(ucp.CurrentPwd))
                ModelState.AddModelError("CurrentPwd", "原密码不正确");
            else
            {
                old_usr.Password = DataEncryption.HashString(ucp.NewPassword);
                old_usr.UpdateTime = DateTime.Now;
                _uow.Commit();
                //Session.Abandon();
                //Session.RemoveAll();
                Session[SessionKey.User] = null;
                //同步修改彩票频道密码
                try
                {
                    var r = new Ichari.Sync.UserSync().UpdatePassword(old_usr.LotteryUserId, ucp.NewPassword);
                    _log.Error(string.Format("同步修改密码执行结果：{0}",r));
                }
                catch {
                    _log.Error(string.Format("同步修改密码失败，userId={0},lotteryUserId={1}",old_usr.Id,old_usr.LotteryUserId));
                }
                base.RenderTip("请重新登录");
                return RedirectToAction("Login", new { m = "rspw" });
            }

            return View();
        }

        #endregion

        public ActionResult Claim(string id)
        {
            long drawID = 0;
            if (id != null)
                drawID = long.Parse(id);
            UserInfo usr = (UserInfo)Session[SessionKey.User];
            if(usr == null)
                return RedirectToAction("Login", "Account");

            Drawings dr = _uow.DrawingsService.Get(o => o.Id == drawID && o.UserId == usr.Id);
            if (dr == null)
                return RedirectToAction("MyDrawings", "Account");
            Session[SessionKey.DrawId] = dr.Id;
            return RedirectToAction("ClaimPrizeStep2", "Drawings");
        }
    }

    
}
