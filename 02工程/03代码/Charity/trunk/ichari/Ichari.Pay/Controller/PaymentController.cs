using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Ichari.Model;
using Ichari.Model.Enum;
using Ichari.Uow;

using com.unionpay.upop.sdk;

namespace Ichari.Pay
{
    public class PaymentController : BaseController
    {
        private IChariUow _uow;

        public PaymentController(IChariUow uow)
        {
            this._uow = uow;
        }

        public ActionResult Index(ViewModel.PaymentViewModel model)
        {
            //todo:还需要处理session信息
            var order = _uow.OrderService.Get(t => t.OrderId == model.OrderId && t.TradeNo == model.TradeNo);
            if (order == null)
                return RedirectToAction("PayError");
            if(order.Status >= (int)Ichari.Model.Enum.OrderState.Paid)
                return RedirectToAction("PayError");
            model.OrderModel = order;
            return View(model);
        }

        [HttpPost]
        public void Submit(ViewModel.PaymentViewModel model)
        {
            var order = _uow.OrderService.Get(t => t.OrderId == model.OrderId && t.TradeNo == model.TradeNo);
            if (order == null)
                Response.Redirect("PayError");
            model.OrderModel = order;

            model.FrontCallbackUrl = Ichari.Common.WebUtils.GetAppSettingValue("FrontPayUrl");
            model.BackCallbackUrl = Ichari.Common.WebUtils.GetAppSettingValue("BackPayUrl");

            if (model.Source == Model.Enum.PaySource.Donation)
            {
                model.ProdUrl = Ichari.Common.WebUtils.GetAppSettingValue("DonateUrl");
                model.ProdName = Ichari.Common.WebUtils.GetAppSettingValue("DonationName");
            }
            switch (model.PayWayType)
            {
                case PayWay.UnionPay :
                    // 要使用各种Srv必须先使用LoadConf载入配置
                    UPOPSrv.LoadConf(Server.MapPath("~/conf.xml.config"));

                    var p = GenParams(model);
                    var srv = new FrontPaySrv(p);
                    //写入订单支付记录
                    SavePayLog(model);

                    Response.ContentEncoding = srv.Charset;
                    _log.Info(srv.CreateHtml());
                    Response.Write(srv.CreateHtml());
                    break;
            }            
        }

        private Dictionary<string, string> GenParams(ViewModel.PaymentViewModel model)
        {
            var dict = new Dictionary<string, string>();
            // 交易类型，前台只支持CONSUME 和 PRE_AUTH
            dict.Add("transType", UPOPSrv.TransType.CONSUME);
            // 商品URL
            dict.Add("commodityUrl",model.ProdUrl);
            // 商品名称
            dict.Add("commodityName",model.ProdName);
            // 商品单价，分为单位
            dict.Add("commodityUnitPrice",Ichari.Common.WebUtils.ConvertToFen(model.OrderModel.Total));
            // 商品数量
            dict.Add("commodityQuantity",model.OrderModel.OrderDetail.Sum(t => t.ProductCount).ToString());
            // 订单号，必须唯一
            dict.Add("orderNumber",model.OrderModel.TradeNo);
            // 交易金额
            dict.Add("orderAmount", Ichari.Common.WebUtils.ConvertToFen(model.OrderModel.Total));
            // 币种
            dict.Add("orderCurrency",UPOPSrv.CURRENCY_CNY);
            // 交易时间
            dict.Add("orderTime",model.OrderModel.CreateTime.ToString("yyyyMMddHHmmss"));
            // 用户IP
            dict.Add("customerIp",Request.UserHostAddress);
            // 前台回调URL
            dict.Add("frontEndUrl",model.FrontCallbackUrl);
            // 后台回调URL（前台请求时可为空）
            dict.Add("backEndUrl",model.BackCallbackUrl);

            return dict;
        }
        private void SavePayLog(ViewModel.PaymentViewModel model)
        {
            var paylog = new PayLog();
            paylog.OrderId = model.OrderModel.OrderId;
            //paylog.TransactionId = model.OrderModel.TradeNo;
            paylog.UserId = model.OrderModel.UserId;
            paylog.PayWay = (int)PayWay.UnionPay;
            paylog.PayMoney = model.OrderModel.Total;
            paylog.PayUrl = "";
            paylog.BackUrl = model.BackCallbackUrl;
            paylog.PayResult = "前往支付";
            paylog.CreateTime = DateTime.Now;

            _uow.PayLogService.Add(paylog);
            _uow.Commit();
        }
        /// <summary>
        /// 前台同步通知
        /// </summary>
        /// <returns></returns>
        public ActionResult PayOk()
        {
            //UPOPSrv.LoadConf(Server.MapPath("~/conf.xml.config"));
            // 使用Post过来的内容构造SrvResponse
            SrvResponse resp = new SrvResponse(Util.NameValueCollection2StrDict(Request.Form));
            
            var tradeNo = resp.Fields["orderNumber"];
            var order = _uow.OrderService.Get(t => t.TradeNo == tradeNo);
            if (order == null)
                return RedirectToAction("PayError");

            //if (order.Status == (int)OrderState.Create)
            //{
            //    OnAfterPay(order, resp);
            //}

            return View(order);
        }

        public void CallBack()
        {
            _log.Info("--start recieve notification--");
            //UPOPSrv.LoadConf(Server.MapPath("~/conf.xml.config"));
            
            // 使用Post过来的内容构造SrvResponse
            SrvResponse resp = new SrvResponse(Util.NameValueCollection2StrDict(Request.Form));

            
            if (resp.ResponseCode == SrvResponse.RESP_SUCCESS)
            {
                var tradeNo = resp.Fields["orderNumber"];
                var order = _uow.OrderService.Get(t => t.TradeNo == tradeNo);
                if (order == null)
                    return;

                if (order.Status == (int)OrderState.Create)
                {
                    OnAfterPay(order, resp);
                }
            }
            _log.Info("--notify success--");
        }

        /// <summary>
        /// 支付成功的后续操作
        /// </summary>
        private void OnAfterPay(Order order,SrvResponse resp)
        {   
            var paylog = new PayLog();
            paylog.OrderId = order.OrderId;
            paylog.TransactionId = resp.Fields["qid"];
            paylog.UserId = order.UserId;
            paylog.PayWay = (int)PayWay.UnionPay;
            paylog.PayMoney = order.Total;
            paylog.PayUrl = resp.OrigPostString;
            paylog.BackUrl = Request.RawUrl;
            paylog.CreateTime = DateTime.Now;
            paylog.PayResult = "支付成功";
            //如果支付失败
            if (resp.ResponseCode != SrvResponse.RESP_SUCCESS)
                paylog.PayResult = "支付失败";
            else { 
                //账户操作
                var acc = _uow.AccountService.Get(t => t.UserId == order.UserId);
                if (acc == null)
                {
                    acc = new Account() { 
                        UserId = order.UserId,
                        Amount = 0,
                        IsStop = false,
                        CreateTime = DateTime.Now,
                        FrozenAmount = 0
                    };
                    //新增账户
                    _uow.AccountService.Add(acc);
                }
                //账户日志--充值
                var acclog = new AccountLog();
                acclog.UserId = order.UserId;
                acclog.OrderId = order.OrderId;
                acclog.Amount = order.Total;
                acclog.AccountWay = (int)AccountWay.In;
                acclog.Ip = Request.UserHostAddress;
                acclog.CreateTime = DateTime.Now;
                acclog.PayWay = (int)PayWay.UnionPay;
                _uow.AccountLogService.Add(acclog);
                //充值
                acc.Amount += order.Total;
                //账户日志--扣款
                var acclog2 = new AccountLog();
                acclog2.UserId = order.UserId;
                acclog2.OrderId = order.OrderId;
                acclog2.Amount = order.Total;
                acclog2.AccountWay = (int)AccountWay.Out;
                acclog2.Ip = Request.UserHostAddress;
                acclog2.CreateTime = DateTime.Now;
                acclog2.PayWay = (int)PayWay.UnionPay;
                _uow.AccountLogService.Add(acclog2);
                //修改账户余额
                acc.Amount -= order.Total;
                acc.UpdateTime = DateTime.Now;
                //写支付日志
                _uow.PayLogService.Add(paylog);
                //修改订单状态
                order.Status = (int)OrderState.Paid;
                order.UpdateTime = DateTime.Now;
                order.PayTime = DateTime.Now;

            }
            _uow.Commit();
        }

  
        public ActionResult PayError()
        {
            return View();
        }
    }
}
