<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BackPay.aspx.cs" Inherits="SampleCS.BackPay" %>
<%@ Import Namespace="com.unionpay.upop.sdk" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
        // **************演示后台交易的请求***********************

        // 要使用各种Srv必须先使用LoadConf载入配置
        UPOPSrv.LoadConf(Server.MapPath("./App_Data/conf.xml.config"));

        // 使用Dictionary保存参数
        System.Collections.Generic.Dictionary<string, string> param = new System.Collections.Generic.Dictionary<string, string>();

        // 随机构造一个订单号和订单时间（演示用）
        Random rnd = new Random();
        DateTime orderTime = DateTime.Now;
        string orderID = orderTime.ToString("yyyyMMddHHmmss") + (rnd.Next(900) + 100).ToString().Trim();
        

        // 填写参数
        param["transType"] = UPOPSrv.TransType.CONSUME;                             // 交易类型
        param["commodityUrl"] = "http://emall.chinapay.com/product?name=商品";      // 商品URL
        param["commodityName"] = "商品名称";                                        // 商品名称
        param["commodityUnitPrice"] = "11000";                                      // 商品单价，分为单位
        param["commodityQuantity"] = "1";                                           // 商品数量
        param["orderNumber"] = orderID;                                             // 订单号，必须唯一
        param["orderAmount"] = "11000";                                             // 交易金额
        param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;                              // 币种
        param["orderTime"] = orderTime.ToString("yyyyMMddHHmmss");                  // 交易时间
        param["customerIp"] = "172.17.136.34";                                      // 用户IP
        param["frontEndUrl"] = "http://www.example.com/UPOPSDK/NotifyCallback.aspx";      // 前台回调URL（后台请求时可为空）
        param["backEndUrl"] = "http://www.example.com/UPOPSDK/NotifyCallBack.aspx";       // 后台回调URL
        param["transTimeout"] = "300000";                                           // 交易超时时间，毫秒
        param["cardNumber"] = "6212341111111111111";                                // 卡号
        param["cardCvn2"] = "123";                                                  // CVN2号
        param["cardExpire"] = "1212";                                               // 信用卡过期时间

        // 创建后台交易服务对象
        BackSrv srv = new BackPaySrv(param);
        
        // 请求服务，得到SrvResponse回应对象
        SrvResponse resp = srv.Request();
            
        Response.Write ("<h1>");
        Response.Write("order Number:" + orderID + "<br/>");
        Response.Write("order Time:" + orderTime.ToString()  + "<br/>");

        // 根据回应对象的ResponseCode判断是否请求成功
        //（请求成功但交易不一定处理完成，需用Query接口查询交易状态或等Notify回调通知）
        if(resp.ResponseCode != SrvResponse.RESP_SUCCESS )
        {
            string msg = resp.HasField ("respMsg") ? resp.Fields["respMsg"] : "";
            Response.Write(String.Format("Pay Failed! Error:[{0}] : {1} ", resp.Fields["respCode"], msg));
        }else
        {
            Response.Write( "Pay Success!");
        } 
        Response.Write("</h1><br/>");
        Response.Write ( "post string:" + resp.OrigPostString);
        
        %>
    </div>
    </form>
</body>
</html>
