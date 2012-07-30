using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;

using Ichari.Uow;
using Ichari.Common;
using Ichari.Common.Helper;
using Ichari.Model;
using Ichari.Model.Enum;
using Ichari.Web.ViewModel;
using Ichari.Cache;
using Ichari.Model.Dto;
using System.Configuration;
using Ichari.Model.Validation;

namespace Ichari.Web
{
    /// <summary>
    /// 爱心捐赠controller
    /// </summary>
    public class CharityController : BaseController
    {
        private IChariUow _uow;

        public CharityController(IChariUow uow)
        {
            _uow = uow;
        }

        public ActionResult Index()
        {
            ViewData[SessionKey.VwCurrentNav] = "charity";
            var model = new ViewModel.CharityViewModel();
            return View(model);
        }

        [LogonFilter]
        [HttpPost]
        public void GoPay(decimal amount,string tradeNo)
        {
            
            if (amount <= 0)
                return;
            //如果tradeno有值，则是从用户中心过来
            if (string.IsNullOrEmpty(tradeNo))
            {
                var user = Session[SessionKey.User] as UserInfo;
                //创建捐款订单
                var order = new Order();
                order.OrderId = System.Guid.NewGuid();
                order.TradeNo = WebUtils.GenTradeNo(new Random());
                order.OrderType = (int)Ichari.Model.Enum.OrderType.Donation;
                if (user != null)
                    order.UserId = user.Id;
                else
                    order.UserId = StaticKey.AnonymousUserId;
                order.Total = amount;
                order.PayWay = (int)Ichari.Model.Enum.PayWay.UnionPay;
                order.Ip = Request.UserHostAddress;
                order.Status = (int)Ichari.Model.Enum.OrderState.Create;
                order.CreateTime = DateTime.Now;
                //订单明细
                order.OrderDetail.Add(new OrderDetail()
                {
                    OrderId = order.OrderId,
                    ProductId = VirtualProduct.Donation,
                    ProductName = StaticKey.VirtualProdDonation,
                    Price = amount,
                    ProductCount = 1
                });

                _uow.OrderService.Add(order);
                _uow.Commit();

                Response.Redirect(string.Format(
                    WebUtils.GetAppSettingValue(StaticKey.PayUrlFormatter)
                        ,order.OrderId,order.TradeNo,PaySource.Donation)
                );
            }
            var oldOrder = _uow.OrderService.Get(t => t.TradeNo == tradeNo);
            if (oldOrder == null)
                return;
            Response.Redirect(string.Format(
                    WebUtils.GetAppSettingValue(StaticKey.PayUrlFormatter)
                        ,oldOrder.OrderId,oldOrder.TradeNo,PaySource.Donation)
                );
        }
        
        public ActionResult ChariDraw()
        {
            ViewData[SessionKey.VwCurrentNav] = "game";
            Session.Remove(SessionKey.DonationId);
            Session.Remove(SessionKey.DonationAmt);
            Session.Remove("ChariGame");
            Session.Add("ChariGame", DataEncryption.HashString("chari.game"));
            ChariWinnerViewModel cwvm = new ChariWinnerViewModel();
            if (Request.Headers["Referer"] != null && Request.Headers["Referer"].Contains(ConfigurationManager.AppSettings["UPOPHost"]))
            {
                cwvm.IsFromChinaUnion = true;
                base.RenderTip("完成抽奖前，请先不要关闭本页面，或点击其他链接",3);
                _log.Info("from china union");
                string oAmt = Request.Form.Get("orderAmount");
                string oQid = Request.Form.Get("qid");
                string svar = "orderAmount=" + oAmt.ToString() + "&orderCurrency=" + Request.Form.Get("orderCurrency") + "&qid=" + oQid;
                string sig_in = Request.Form.Get("signature");
                string sig = DataEncryption.HashUPOPChariOrder(svar).Replace("-", "").ToLower();
                if (sig_in.Equals(sig))
                {
                    _log.Info("china union sign ok");
                    //signatures equal, writes variables to session
                    Session[SessionKey.ID] = oQid;
                    Session[SessionKey.DonationAmt] = decimal.Parse(oAmt) / 100;
                    Session[SessionKey.UPOPChari] = "Verified";                    
                }
            }
            #region Winner List
            
            cwvm.WinList = _uow.GetWinnerPrizeList();

            // pad the winner list to at least 5 winners
            for (int i = cwvm.WinList.Count; i < 10; i++)
            {
                switch (i % 5)
                {
                    case 0: ViewBag.WinList += "<ul><li>wahaha</il><li>4天前</li><li>HTC 手机</li></ul>";
                        break;
                    case 1: ViewBag.WinList += "<ul><li>iamgod</il><li>5天前</li><li>Sony TV</li></ul>";
                        break;
                    case 2: ViewBag.WinList += "<ul><li>willywanker</il><li>7天前</li><li>笔记本</li></ul>";
                        break;
                    case 3: ViewBag.WinList += "<ul><li>seanc</il><li>10天前</li><li>自行车</li></ul>";
                        break;
                    case 4: ViewBag.WinList += "<ul><li>jaschen</il><li>13天前</li><li>iPad</li></ul>";
                        break;
                }
            }
            #endregion

            return View(cwvm);
        }

        public string FlashGame(string var)
        {
            //Session[SessionKey.DrawId] = 1;
            //Session[SessionKey.PrizeId] = 1;
            //Session[SessionKey.User] = _uow.UserInfoService.Get(o => o.Id == 8);
            //return "{ \"type\" : \"checkLogin\", \"status\" : 1, \"massage\" : \"xx\", \"angle\" : 60, \"url\" : \"win\" }";
                                
            //check if request generated from ChariDraw page
            if (Session["ChariGame"] == null)
                return "{ \"type\" : \"checkLogin\", \"status\" : 0, \"massage\" : \"已超时，请重新刷新。\", \"url\" : \"exp\" }";
            if (Session["ChariGame"].ToString() == DataEncryption.HashString("chari.game"))
            {
                //直接输入捐赠号
                UserInfo usr = (UserInfo)Session[SessionKey.User];
                string DonationId = (string)Session[SessionKey.DonationId];
                decimal DonationAmt = 0;
                if (Session[SessionKey.DonationAmt] != null)
                    DonationAmt = (decimal)Session[SessionKey.DonationAmt];
                
                #region User Logged In
                if (usr != null)
                {
                    //user logged in
                    UserInfo dbusr = _uow.UserInfoService.Get(o => o.Id == usr.Id);
                    if (dbusr != null && usr.Password == dbusr.Password && usr.UserName == dbusr.UserName)
                    {
                        //user valid, get user's donations
                        //List<LoveChange> donations = _uow.LoveChangeService.GetQueryList(o => o.UserId == dbusr.Id  && o.IsGame != true).OrderBy(o => o.CreateTime).ToList();
                        var donation = _uow.LoveChangeService.GetQueryList(o => o.UserId == dbusr.Id && o.IsGame != true).OrderBy(o => o.CreateTime).FirstOrDefault();

                        //int dtype = (int)OrderType.Donation;
                        //List<Order> orderList = _uow.OrderService.GetQueryList(o => o.UserId == dbusr.Id && o.IsGame != true && o.OrderType == dtype && o.Status == 10).OrderBy(o => o.PayTime).ToList();
                        var donationOrder = _uow.OrderService.GetQueryList(t => t.UserId == dbusr.Id
                                                && t.IsGame == false
                                                && t.OrderType == (int)OrderType.Donation
                                                && t.Status >= (int)OrderState.Paid).OrderBy(t => t.PayTime).FirstOrDefault();

                        #region 愛心零錢 UnionPay
                        //if (donations.Count > 0)
                        if (donation != null)
                        {
                            //愛心零錢 available for drawings, select earliest and create drawing entry
                            Drawings NewDraw = new Drawings();
                            NewDraw.Source = (int)GameSource.ChangeOfCare;
                            NewDraw.UserId = dbusr.Id;
                            NewDraw.OrderNo = donation.UnionOrder;
                            
                            //get available prizes
                            List<Prize> przList = _uow.PrizeService.SelectPrizeList(true);
                            if (przList.Count > 0)
                            {
                                //prize available, calculate winning result
                                bool DrawResult = DataEncryption.GetDrawingResult();
                                NewDraw.IsWinner = DrawResult;

                                if (DrawResult)
                                {
                                    //user won a prize, selects a random prize from the list
                                    Prize wPrize = _uow.PrizeService.SelectWinningPrize(przList);
                                    NewDraw.PrizeId = wPrize.Id;
                                    //increase used prize count
                                    wPrize.UsedCount += 1;
                                    NewDraw.CreateTime = DateTime.Now;
                                    donation.GameTime = NewDraw.CreateTime;
                                    donation.IsGame = true;
                                    _uow.DrawingsService.Add(NewDraw);
                                    _uow.Commit();
                                    
                                    int ang = wPrize.Angle + 720;
                                    return "{ \"type\" : \"checkLogin\", \"status\" : 1, \"massage\" : \"" + wPrize.Name + "\", \"angle\" : " + ang.ToString() + ", \"url\" : \"usr\" }";
                                }
                                else
                                {
                                    //user did not win
                                    NewDraw.CreateTime = DateTime.Now;
                                    donation.GameTime = NewDraw.CreateTime;
                                    NewDraw.IsConfirrmed = true;
                                    donation.IsGame = true;
                                    _uow.DrawingsService.Add(NewDraw);
                                    _uow.Commit();
                                    return "{ \"type\" : \"checkLogin\", \"status\" : 2, \"massage\" : \"感谢您对慈善事业的大力支持，谢谢参与。\", \"angle\" : 1050, \"url\" : \"usr\" }";
                                }
                            }
                            else
                            {
                                //no prize available, user set to not win
                                NewDraw.CreateTime = DateTime.Now;
                                donation.GameTime = NewDraw.CreateTime;
                                NewDraw.IsConfirrmed = true;
                                donation.IsGame = true;
                                _uow.DrawingsService.Add(NewDraw);
                                _uow.Commit();
                                return "{ \"type\" : \"checkLogin\", \"status\" : 2, \"massage\" : \"感谢您对慈善事业的大力支持，谢谢参与。\", \"angle\" : 1050, \"url\" : \"usr\" }";
                            }
                        }
                        #endregion
                        #region 愛心捐贈 ichari
                        else if (donationOrder != null)
                        {
                            //愛心捐贈 available for drawings, select earliest and create drawing entry
                            Drawings NewDraw = new Drawings();
                            NewDraw.Source = (int)GameSource.IchariDonation;
                            NewDraw.UserId = dbusr.Id;
                            NewDraw.OrderNo = donationOrder.TradeNo;

                            //get available prizes
                            List<Prize> przList = _uow.PrizeService.SelectRandomPrizeList(11, true);
                            if (przList.Count > 0)
                            {
                                //prize available, calculate winning result
                                bool DrawResult = DataEncryption.GetDrawingResult();
                                NewDraw.IsWinner = DrawResult;
                                if (DrawResult)
                                {
                                    //user won a prize, selects a random prize from the list
                                    Prize wPrize = _uow.PrizeService.SelectWinningPrize(przList);
                                    //end prize selection
                                    NewDraw.PrizeId = wPrize.Id;
                                    //increase used prize count
                                    wPrize.UsedCount += 1;
                                    NewDraw.CreateTime = DateTime.Now;                                    
                                    _uow.DrawingsService.Add(NewDraw);
                                    //更新捐赠订单
                                    donationOrder.UpdateTime = NewDraw.CreateTime;
                                    donationOrder.IsGame = true;

                                    _uow.Commit();
                                    
                                    int ang = wPrize.Angle + 720;
                                    return "{ \"type\" : \"checkLogin\", \"status\" : 1, \"massage\" : \"" + wPrize.Name + "\", \"angle\" : " + ang.ToString() + ", \"url\" : \"usr\" }";
                                }
                                else
                                {
                                    //user did not win
                                    NewDraw.CreateTime = DateTime.Now;                                    
                                    NewDraw.IsConfirrmed = true;
                                    //更新捐赠订单
                                    donationOrder.UpdateTime = NewDraw.CreateTime;
                                    donationOrder.IsGame = true;
                                    _uow.DrawingsService.Add(NewDraw);

                                    _uow.Commit();
                                    
                                    return "{ \"type\" : \"checkLogin\", \"status\" : 2, \"massage\" : \"感谢您对慈善事业的大力支持，谢谢参与。\", \"angle\" : 1050, \"url\" : \"usr\" }";
                                }
                            }
                            else
                            {
                                //no prize available, user set to not win
                                NewDraw.CreateTime = DateTime.Now;
                                donationOrder.UpdateTime = NewDraw.CreateTime;
                                NewDraw.IsConfirrmed = true;
                                donationOrder.IsGame = true;
                                _uow.DrawingsService.Add(NewDraw);
                                _uow.Commit();
                                return "{ \"type\" : \"checkLogin\", \"status\" : 2, \"massage\" : \"感谢您对慈善事业的大力支持，谢谢参与。\", \"angle\" : 1050, \"url\" : \"usr\" }";
                            }
                        }
                        #endregion
                        else
                        {
                            _log.Info("user is logon,but not exist donation order");
                            //no donations available for drawing
                            return "{ \"type\" : \"checkLogin\", \"status\" : 0, \"massage\" : \"对不起，您已经没有抽奖机会了\" }";
                        }
                    }
                }
                #endregion
                #region Donation Entered or from UPOP
                else if (!string.IsNullOrWhiteSpace(DonationId) && DonationAmt > 0)
                {
                    //donation entered
                    LoveChange lc = _uow.LoveChangeService.Get(o => o.UnionOrder == DonationId & o.Amount == DonationAmt);
                    if (lc != null && lc.Amount == DonationAmt && lc.UnionOrder == DonationId)
                    {
                        //donation valid, 
                        if (lc.IsGame != true)
                        {
                            //check if prize available, create drawing entry
                            Drawings NewDraw = new Drawings();
                            NewDraw.OrderNo = DonationId;
                            NewDraw.Source = (int)GameSource.ChangeOfCare;
                            List<Prize> przList = _uow.PrizeService.SelectRandomPrizeList(11, true);
                            if (przList.Count > 0)
                            {
                                //prize available, get drawing result
                                bool drawResult = DataEncryption.GetDrawingResult();
                                NewDraw.IsWinner = drawResult;
                                if (drawResult)
                                {
                                    //user won, randomly selects a prize
                                    Prize wPrize = _uow.PrizeService.SelectWinningPrize(przList);
                                    NewDraw.PrizeId = wPrize.Id;
                                    //increase used count
                                    wPrize.UsedCount += 1;
                                    NewDraw.CreateTime = DateTime.Now;
                                    lc.GameTime = NewDraw.CreateTime;
                                    lc.IsGame = true;
                                    _uow.DrawingsService.Add(NewDraw);
                                    _uow.Commit();
                                    Session[SessionKey.DrawId] = _uow.DrawingsService.Get(o => o.PrizeId == NewDraw.PrizeId && o.OrderNo == NewDraw.OrderNo).Id;
                                    
                                    
                                    int ang = wPrize.Angle + 720;
                                    return "{ \"type\" : \"checkLogin\", \"status\" : 1, \"massage\" : \"" + wPrize.Name + "\", \"angle\" : " + ang.ToString() + ", \"url\" : \"win\" }";
                                }
                                else
                                {
                                    //user did not win
                                    NewDraw.CreateTime = DateTime.Now;
                                    lc.GameTime = NewDraw.CreateTime;
                                    NewDraw.IsConfirrmed = true;
                                    lc.IsGame = true;
                                    _uow.DrawingsService.Add(NewDraw);
                                    _uow.Commit();
                                    return "{ \"type\" : \"checkLogin\", \"status\" : 2, \"massage\" : \"感谢您对慈善事业的大力支持，谢谢参与。\", \"angle\" : 1050, \"url\" : \"lose\" }";
                                }
                            }
                            else
                            {
                                //prize not available, user set to lost
                                NewDraw.CreateTime = DateTime.Now;
                                lc.GameTime = NewDraw.CreateTime;
                                NewDraw.IsConfirrmed = true;
                                lc.IsGame = true;
                                _uow.DrawingsService.Add(NewDraw);
                                _uow.Commit();
                                return "{ \"type\" : \"checkLogin\", \"status\" : 2, \"massage\" : \"感谢您对慈善事业的大力支持，谢谢参与。\", \"angle\" : 1050, \"url\" : \"lose\" }";
                            }
                        }
                        else
                        {
                            //game played
                            return "{ \"type\" : \"checkLogin\", \"status\" : 0, \"massage\" : \"此捐赠单已抽奖，请登录查询中奖记录。\" }";
                        }
                    }
                    else
                    {
                        //donation invalid
                        return "{ \"type\" : \"checkLogin\", \"status\" : 0, \"massage\" : \"请重新确认您的捐赠单号。（或凭证号）。\" }";
                    }
                }
                #endregion
                else
                {
                    //user not logged in and no donation entered, disable flash
                    return "{ \"type\" : \"checkLogin\", \"status\" : 0, \"massage\" : \"《请登陆用户》 或 《填写捐赠单》\", \"url\" : \"login\" }";
                }
            }
            
            return "";
        }
        
        /// <summary>
        /// Verifies donation order entered by user for drawing
        /// </summary>
        /// <param name="cdvm"></param>
        /// <returns></returns>
        public ActionResult GameVerify(ChariDrawViewModel cdvm)
        {
            LoveChange lc = _uow.LoveChangeService.Get(o => o.Amount == cdvm.DonAmount && o.UnionOrder == cdvm.DonatioinId);

            if (lc != null)
            {
                Session[SessionKey.DonationId] = cdvm.DonatioinId;
                Session[SessionKey.DonationAmt] = cdvm.DonAmount;
                return RedirectToAction("DonationPanel");
            }
            Response.Close();
            Response.End();
            
            return null;
        }
        /// <summary>
        /// Verifies User Login entered by user for drawing
        /// </summary>
        /// <param name="cdvm"></param>
        /// <returns></returns>
        public ActionResult GameLogin(ChariDrawViewModel cdvm)
        {
            if (Request.IsAjaxRequest())
            {
                if (cdvm != null)
                {
                    UserInfo usr = _uow.UserInfoService.Get(o => o.UserName == cdvm.UserName);
                    if (usr != null && usr.Password == DataEncryption.HashString(cdvm.Password) && usr.UserName == cdvm.UserName)
                    {
                        //user logged in successfully
                        Session[SessionKey.User] = usr;
                        return RedirectToAction("GamePanel");
                    }
                }
            }
            Response.Close();
            Response.End();
            return null;
        }
        /// <summary>
        /// Verifies order redirected from UPOP
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyUPOPOrder()
        {
            if (Session[SessionKey.UPOPChari] != null && Session[SessionKey.ID] != null && Session[SessionKey.DonationAmt] != null)
            {
                //redirected from UPOP
                if (Session[SessionKey.UPOPChari].ToString() == "Verified")
                {
                    string oId = Session[SessionKey.ID].ToString();
                    decimal amt = decimal.Parse(Session[SessionKey.DonationAmt].ToString());
                    if (_uow.LoveChangeService.Get(o => o.UnionOrder == oId && o.Amount == amt) == null)
                    {
                        LoveChange lc = new LoveChange();
                        lc.UnionOrder = oId;
                        lc.Amount = amt;
                        lc.CreateTime = DateTime.Now;
                        lc.IsGame = false;
                        lc.IsSuccess = true;
                        if (base.CurrentUser != null) {
                            lc.UserId = base.CurrentUser.Id;
                        }
                        lc.State = (int)OrderState.Paid;
                        _uow.LoveChangeService.Add(lc);
                        _uow.Commit();
                        _log.Info(string.Format("ChinaUnion: {0}_{1}",lc.UnionOrder,lc.Amount));
                    }
                    Session[SessionKey.UPOPChari] = "Verified";
                    Session[SessionKey.DonationId] = oId;
                    Session[SessionKey.DonationAmt] = amt;
                    _log.Info("upop order verified, redirect to donation panel");
                    return RedirectToAction("DonationPanel", new {t = ( DateTime.Now - new DateTime(1900,1,1)).Ticks });
                }
            }
            
            return RedirectToAction("Error");
        }

        public ActionResult LoginPanel(string id)
        {
            UserInfo usr = (UserInfo)Session[SessionKey.User];
            //if (usr != null)
            //{
            //    UserInfo dbusr = _uow.UserInfoService.Get(o => o.Id == usr.Id);
            //    if (dbusr != null && dbusr.UserName == usr.UserName && dbusr.Password == usr.Password)
            //    {
            //        return RedirectToAction("GamePanel");
            //    }
            //}
            //else if (Session[SessionKey.UPOPChari] != null && Session[SessionKey.ID] != null && Session[SessionKey.DonationAmt] != null)
            //{
            //    //redirected from UPOP
            //    if (Session[SessionKey.UPOPChari].ToString() == "Verified")
            //    {
            //        return RedirectToAction("VerifyUPOPOrder");
            //    }
            //}

            if (Session[SessionKey.UPOPChari] != null && Session[SessionKey.ID] != null
                && Session[SessionKey.DonationAmt] != null
                && Session[SessionKey.UPOPChari].ToString() == "Verified")
            {
                _log.Info("from china union : redirect to verifyUpopOrder");
                //redirected from UPOP
                return RedirectToAction("VerifyUPOPOrder");
            }

            if (usr == null)
            {                
                return View();
            }
            else { 
                //不是从银联跳转
                return RedirectToAction("GamePanel", new { t = new Random().NextDouble().ToString() });
            }
            
        }

        public ActionResult GamePanel()
        {
            UserInfo usr = (UserInfo)Session[SessionKey.User];
            if (usr != null && !string.IsNullOrWhiteSpace(usr.UserName) && usr.Id > 0)
            {
                ChariGameViewModel cgvm = new ChariGameViewModel();
                cgvm.DList = _uow.LoveChangeService.GetQueryList(o => o.UserId == usr.Id && o.IsGame == false).OrderByDescending(o => o.CreateTime).ToList();
                int dtype = (int)OrderType.Donation;
                cgvm.OList = _uow.OrderService.GetQueryList(o => o.UserId == usr.Id && o.IsGame != true && o.OrderType == dtype && o.Status == 10).OrderBy(o => o.PayTime).ToList();
                cgvm.Count = cgvm.DList.Count + cgvm.OList.Count;
                return View(cgvm);
            }
            return View();
        }

        public ActionResult DonationPanel()
        {
            string donId = Session[SessionKey.DonationId].ToString();
            decimal donAmt = 0;
            _log.Info(string.Format("SessionKey.DonationId : {0} , SessionKey.DonationAmt : {1}",donId,Session[SessionKey.DonationAmt]));
            if (!string.IsNullOrWhiteSpace(donId) && decimal.TryParse(Session[SessionKey.DonationAmt].ToString(), out donAmt))
            {
                LoveChange lc = _uow.LoveChangeService.Get(o => o.UnionOrder == donId && o.Amount == donAmt);
                if (lc != null)
                {
                    ChariGameViewModel cgvm = new ChariGameViewModel();
                    cgvm.DonationId = donId;
                    cgvm.DonationAmount = donAmt;
                    ViewBag.DonPlayed = lc.IsGame;
                    _log.Info("love change query ok");
                    return View(cgvm);
                }
            }
            
            return RedirectToAction("LoginPanel");
        }

        public ActionResult ClaimPrize()
        {
            if (Session[SessionKey.User] != null)
            {
                UserInfo usr = (UserInfo)Session[SessionKey.User];

                if (_uow.UserInfoService.Get(o => o.Id == usr.Id && o.UserName == usr.UserName && o.Password == usr.Password) != null)
                {
                    return RedirectToAction("ClaimPrizeStep2");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult ClaimPrize(WinningRegistration winReg)
        {
            if (Session[SessionKey.Captcha] == null || Session[SessionKey.Captcha].ToString() != winReg.Captcha)
            {
                ModelState.AddModelError("Captcha", "验证码不正确");
            }
            if (Session[SessionKey.DrawId] == null)
            {
                return RedirectToAction("Error");
            }
            if (ModelState.IsValid)
            {
                List<UserInfo> eusr = _uow.UserInfoService.GetList(t => t.UserName == winReg.UserName).ToList();
                if (eusr.Count == 0)
                {
                    UserInfo usr = new UserInfo();
                    string pwdplain = winReg.Password;
                    usr.RegFrom = 1;
                    usr.UserName = winReg.UserName;
                    usr.Password = DataEncryption.HashString(pwdplain);
                    usr.TrueName = winReg.Name;
                    usr.Email = winReg.Email;
                    usr.Phone = winReg.Cell;
                    usr.Tel = winReg.Tel;
                    usr.CreateTime = DateTime.Now;
                    usr.UpdateTime = DateTime.Now;
                    _uow.UserInfoService.Add(usr);
                    _uow.Commit();

                    UserInfo u = _uow.UserInfoService.Get(o => o.UserName == o.UserName && o.Password == usr.Password);
                    Address addr = new Address();
                    addr.UserId = u.Id;
                    addr.Address1 = winReg.Street;
                    addr.City = winReg.City;
                    addr.Email = winReg.Email;
                    addr.Mobile = winReg.Cell;
                    addr.PostCode = winReg.Postal;
                    addr.Province = winReg.Province;
                    addr.Tel = winReg.Tel;
                    addr.TrueName = winReg.Name;
                    _uow.AddressService.Add(addr);
                    _uow.Commit();

                    long drawId = long.Parse((string)Session[SessionKey.DrawId]);
                    Drawings dr = _uow.DrawingsService.Get(o => o.Id == drawId);
                    Address ad = _uow.AddressService.Get(o => o.UserId == u.Id && o.Address1 == winReg.Street && o.City == winReg.City && o.PostCode == winReg.Postal);
                    dr.IsConfirrmed = true;
                    dr.AddressId = ad.Id;
                    
                    _uow.Commit();
                    Session[SessionKey.User] = usr;
                    Session[SessionKey.DrawId] = dr.Id;
                    Session[SessionKey.PrizeId] = dr.PrizeId;

                    return RedirectToAction("ClaimConfirmation");
                }
                else
                {
                    ModelState.AddModelError("UserName", "用户名已使用");
                }
            }

            return View(winReg);
        }

        [LogonFilter]
        public ActionResult ClaimPrizeStep2(int? drawId)
        {
            drawId = drawId ?? (int)Session[SessionKey.DrawId];
            var usr = base.CurrentUser;
            //check draw id
            var draw = _uow.DrawingsService.Get(t => t.Id == drawId.Value);
            if (draw == null)
                throw new IchariException(string.Format("未找到相应的抽奖记录：drawId={0}",drawId));
            if(draw.UserId != usr.Id)
                throw new IchariException(string.Format("抽奖记录异常：drawId={0}",drawId));
            if(draw.IsHandled != null && draw.IsHandled.Value)
                throw new IchariException(string.Format("已经登记领奖信息：drawId={0}",drawId));
            //对应的奖品
            var prize = _uow.PrizeService.Get(t => t.Id == draw.PrizeId.Value);
            ViewData["prize"] = prize;
            if (prize.IsVirtual) {
                if (Session[SessionKey.DeliveryFreeCard] == null)
                {
                    var fc = _uow.FreeCardService.Delivery(FreeCardType.UnionCard);
                    draw.CardId = fc.Id; 
                    ViewData["freeCard"] = fc;
                    Session[SessionKey.DeliveryFreeCard] = fc;
                    _uow.Commit();
                }
                else {
                    ViewData["freeCard"] = Session[SessionKey.DeliveryFreeCard] as FreeCard;
                }
                return View();
            }
            if (_uow.UserInfoService.Get(o => o.Id == usr.Id && o.Password == usr.Password) != null)
            {
                var addrList = _uow.AddressService.GetQueryList(t => t.UserId == usr.Id).ToList();
                ViewData["addrList"] = addrList;
                Address addr = addrList.FirstOrDefault(t => t.IsDefault == true);
                if (addr != null)
                {
                    AddressValidation av = new AddressValidation();
                    av.Area = addr.Area;
                    av.Cell = addr.Mobile;
                    av.City = addr.City;
                    av.DefaultAddr = true;
                    av.Email = addr.Email;
                    av.Name = addr.TrueName;
                    av.Postal = addr.PostCode;
                    av.Province = addr.Province;
                    av.Street = addr.Address1;
                    av.Tel = addr.Tel;
                    av.DrawId = drawId ?? 0;
                    
                    return View(av);
                }
                
                return View();
            }
            return RedirectToAction("Login", "Account", new { returnUrl = "/Account/MyDrawings" });
        }

        [HttpPost]
        [LogonFilter]
        public ActionResult ClaimPrizeStep2(AddressValidation av,int? selectedAddr)
        {
            
            if (selectedAddr.HasValue) { 
                var dr = _uow.DrawingsService.Get(o => o.Id == av.DrawId.Value);
                if (dr == null)
                    throw new IchariException(string.Format("未找到相应的抽奖记录：drawId={0}",av.DrawId));
                dr.AddressId = selectedAddr.Value;
                
                _uow.Commit();
                return RedirectToAction("DrawDetail", "Account", new { id = dr.Id });
            }

            if (ModelState.IsValid)
            {
                var usr = base.CurrentUser;
                if (_uow.UserInfoService.Get(o => o.Id == usr.Id && o.UserName == usr.UserName && o.Password == usr.Password) != null)
                {
                    Address addr = new Address();
                    addr.UserId = usr.Id;
                    addr.Address1 = av.Street;
                    addr.Area = av.Area;
                    addr.City = av.City;
                    addr.Email = av.Email;
                    addr.Mobile = av.Cell;
                    addr.PostCode = av.Postal;
                    addr.Province = av.Province;
                    addr.Tel = av.Tel;
                    addr.TrueName = av.Name;
                    _uow.AddressService.Add(addr);
                    _uow.Commit();

                    long drId = av.DrawId  ?? 0; //long.Parse((string)Session[SessionKey.DrawId]);
                    Drawings dr = _uow.DrawingsService.Get(o => o.Id == drId);
                    dr.AddressId = addr.Id;
                    dr.IsHandled = true;
                    _uow.Commit();
                    return RedirectToAction("DrawDetail", "Account", new { id = dr.Id });
                }
            }
            return View();
        }

        [HttpPost]
        [LogonFilter]
        public ActionResult SendCard(int drawId,string email)
        {
            if (!string.IsNullOrEmpty(email)) {
                var u = base.CurrentUser;
                var dbUsr = _uow.UserInfoService.Get(t => t.Id == u.Id);
                dbUsr.Email = email;
                dbUsr.UpdateTime = DateTime.Now;
                Session[SessionKey.User] = dbUsr;
            }
            var dr = _uow.DrawingsService.Get(t => t.Id == drawId);
            dr.IsHandled = true;
            var fc = Session[SessionKey.DeliveryFreeCard] as FreeCard;
            var fcdb = _uow.FreeCardService.Get(t => t.Id == fc.Id);
            fcdb.DeliverTime = DateTime.Now;
            fcdb.IsDelivery = true;
            _uow.Commit();
            Session.Remove(SessionKey.DeliveryFreeCard);
            base.RenderTip(null);
            return RedirectToAction("DrawDetail", "Account", new { id = dr.Id });
        }
        
    }
}
