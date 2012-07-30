<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrontPay.aspx.cs" Inherits="SampleCS.FrontPay" %>

<%@ Import Namespace="com.unionpay.upop.sdk" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<% 
    // **************演示前台交易的请求***********************
  
    // 要使用各种Srv必须先使用LoadConf载入配置
    UPOPSrv.LoadConf(Server.MapPath("./App_Data/conf.xml.config"));

    // 使用Dictionary保存参数
    System.Collections.Generic.Dictionary<string, string> param = new System.Collections.Generic.Dictionary<string, string>();
    
    // 随机构造一个订单号（演示用）
    Random rnd = new Random();
    string orderID = DateTime.Now.ToString("yyyyMMddHHmmss") + (rnd.Next(900) + 100).ToString().Trim();

    // 填写参数
    param["transType"] = UPOPSrv.TransType.CONSUME;                        // 交易类型，前台只支持CONSUME 和 PRE_AUTH
    param["commodityUrl"] = "http://emall.chinapay.com/product?name=商品";  // 商品URL
    param["commodityName"] = "商品名称";                                    // 商品名称
    param["commodityUnitPrice"] = "11000";                                  // 商品单价，分为单位
    param["commodityQuantity"] = "1";                                       // 商品数量
    param["orderNumber"] = orderID;                                         // 订单号，必须唯一
    param["orderAmount"] = "11000";                                         // 交易金额
    param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;                          // 币种
    param["orderTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");           // 交易时间
    param["customerIp"] = "172.17.136.34";                                  // 用户IP
    param["frontEndUrl"] = "http://www.example.com/UPOPSDK/NotifyCallback.aspx";  // 前台回调URL
    param["backEndUrl"] = "http://www.example.com/UPOPSDK/NotifyCallBack.aspx";   // 后台回调URL（前台请求时可为空）

    // 创建前台交易服务对象
    FrontPaySrv srv = new FrontPaySrv(param);

    // 将前台交易服务对象产生的Html文档写入页面，从而引导用户浏览器重定向
    Response.ContentEncoding = srv.Charset; // 指定输出编码
    Response.Write(srv.CreateHtml());       // 写入页面
   
%>

