using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.unionpay.upop.sdk;

public partial class Home_Room_OnlinePay_UnionPay_FrontSend : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        double dMoney = double.Parse(Request.QueryString["m"].ToString());
        double dFee = double.Parse(Request.QueryString["f"].ToString());
        long NewPayNumber = -1;
        string ReturnDescription = "";

        if(DAL.Procedures.P_GetNewPayNumber(_Site.ID, _User.ID, "UPOP", (dMoney - dFee), dFee, ref NewPayNumber, ref ReturnDescription) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if ((NewPayNumber < 0) || (ReturnDescription != ""))
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

            return;
        }
        // **************演示前台交易的请求***********************

        // 要使用各种Srv必须先使用LoadConf载入配置
        UPOPSrv.LoadConf(Server.MapPath("~/App_Data/conf.xml.config"));

        // 使用Dictionary保存参数
        System.Collections.Generic.Dictionary<string, string> param = new System.Collections.Generic.Dictionary<string, string>();

        // 随机构造一个订单号（演示用）
        //Random rnd = new Random();
        //string orderID = DateTime.Now.ToString("yyyyMMddHHmmss") + (rnd.Next(900) + 100).ToString().Trim();

        // 填写参数
        param["transType"] = UPOPSrv.TransType.CONSUME;                         // 交易类型，前台只支持CONSUME 和 PRE_AUTH
        param["commodityUrl"] = "http://emall.chinapay.com/product?name=商品";  // 商品URL
        param["commodityName"] = "帐户充值";                                    // 商品名称
        param["commodityUnitPrice"] = (dMoney * 100).ToString();                // 商品单价，分为单位
        param["commodityQuantity"] = "1";                                       // 商品数量
        param["orderNumber"] = NewPayNumber.ToString();                         // 订单号，必须唯一
        param["orderAmount"] = (dMoney * 100 + dFee * 100).ToString();          // 交易金额
        param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;                          // 币种
        param["orderTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");           // 交易时间
        param["customerIp"] = "172.17.136.34";                                  // 用户IP
        param["frontEndUrl"] = "http://219.143.228.243:81/Home/Room/OnlinePay/UnionPay/FrontRecieve.aspx";  // 前台回调URL
        // 后台回调URL（前台请求时可为空）used to recieve data from unionpay automatically, use this data as the primary source
        param["backEndUrl"] = "";   

        // 创建前台交易服务对象
        FrontPaySrv srv = new FrontPaySrv(param);

        // 将前台交易服务对象产生的Html文档写入页面，从而引导用户浏览器重定向
        Response.ContentEncoding = srv.Charset; // 指定输出编码
        Response.Write(srv.CreateHtml());       // 写入页面
    }
}