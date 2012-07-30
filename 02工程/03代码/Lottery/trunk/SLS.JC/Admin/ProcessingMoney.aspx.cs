using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Security.Cryptography;
using System.Text;


using System.Reflection;
using System.Web.Services;
using System.ServiceModel;
//using System.ServiceModel.Description;
using System.Web.Services.Description;
using System.Net;
using System.IO;
public partial class Admin_ProcessingMoney : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        frmmain.Attributes.Add("src", "");

        if (!IsPostBack)
        {
            tbBeginTime.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            tbEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = "Admin/ProcessingMoney.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberManagement);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DateTime fromDate = DateTime.Now;
        DateTime toDate = DateTime.Now;
        if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) || !DateTime.TryParse(tbEndTime.Text, out toDate))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期范围!");
            return;
        }


        string sql = @"SELECT a.ID,a.SiteID,a.UserID,a.[DateTime], a.PayType,a.[Money],a.FormalitiesFees,a.Result, 
			                        b.Name, b.RealityName, b.Sex, b.IDCardNumber, b.[Address], b.Email, b.QQ, b.Telephone, b.Mobile
                        FROM dbo.T_UserPayDetails a INNER JOIN dbo.T_Users b ON a.UserID = b.ID 
                        where  a.[DateTime]>=@fromDate and a.[DateTime]<=@toDate  and Result = 0";

        Shove.Database.MSSQL.Parameter[] paras = new Shove.Database.MSSQL.Parameter[3];
        paras[0] = new Shove.Database.MSSQL.Parameter("@fromDate", SqlDbType.DateTime, 8, ParameterDirection.Input, fromDate);
        paras[1] = new Shove.Database.MSSQL.Parameter("@toDate", SqlDbType.DateTime, 8, ParameterDirection.Input, toDate);

        DataTable dt = null;
        if (!string.IsNullOrEmpty(tbName.Text.Trim()))
        {
            sql += " and Name like '%" + Shove._Web.Utility.FilteSqlInfusion(tbName.Text.Trim()) + "%'";
            
            dt = Shove.Database.MSSQL.Select(sql, paras);
        }
        else
        {
            dt = Shove.Database.MSSQL.Select(sql, paras);
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_UserForInitiateFollowSchemeTrys");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }
    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            int Result = -9999;

            long ID = Shove._Convert.StrToLong(e.Item.Cells[9].Text, 0);
            string bankPay = e.Item.Cells[10].Text;
            double Money = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            double FormalitiesFees = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            string PayNumber = e.Item.Cells[8].Text;
            string PayBank = getBankName(bankPay);

            DateTime payDateTime = Shove._Convert.StrToDateTime(e.Item.Cells[2].Text, DateTime.Now.ToString("yyyyMMdd"));
            string payDate = Shove._Convert.StrToDateTime(e.Item.Cells[2].Text, DateTime.Now.ToString("yyyyMMdd")).ToString("yyyyMMdd");

            int ReturnValue = -1;
            string ReturnDescription = "";

            if (e.CommandName == "Query")
            {
                //要区分充值记录是通过什么接口(财付通、支付宝、51支付卡)
                switch(bankPay.Split('_')[0].ToUpper())
                {
                    case "ALIPAY":
                        {
                            //支付宝接口
                            string AlipayPaymentNumber = "";

                            Alipay.Gateway.OnlinePay onlinepay = new Alipay.Gateway.OnlinePay();

                            try
                            {
                                Result = onlinepay.Query(e.Item.Cells[10].Text.Trim(), PayNumber, ref AlipayPaymentNumber, ref ReturnDescription);
                            }
                            catch
                            {
                                Shove._Web.JavaScript.Alert(this.Page, "查询失败，可能是网络通讯故障。请重试一次。");

                                return;
                            }

                            if (Result < 0)
                            {
                                Shove._Web.JavaScript.Alert(this.Page, "支付号为 " + PayNumber + " 的支付记录没有充值成功，描述：" + ReturnDescription);

                                return;
                            }

                            string Memo = "系统交易号：" + PayNumber + ",支付宝交易号：" + AlipayPaymentNumber;
                            ReturnDescription = "";
                            int Results = -1;
                            Results = DAL.Procedures.P_UserAddMoney(_Site.ID, ID, Money, FormalitiesFees, PayNumber, PayBank, Memo, ref ReturnValue, ref ReturnDescription);

                            if (Results < 0)
                            {
                                Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误");

                                return;
                            }
                            else
                            {
                                if (ReturnValue < 0)
                                {
                                    Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
                                    return;
                                }

                                Shove._Web.JavaScript.Alert(this.Page, "此笔充值记录已到帐并已处理成功！");
                            }
                        }

                        break;
                    case "51ZFK":
                        {
                            //51支付卡、神州行充值卡
                            frmmain.Attributes.Add("src", "../Home/Room/OnlinePay/ZhiFuKa/PayQuery.aspx?sdcustomno=" + PayNumber);
                        }
                        break;
                    case "TENPAY":
                        {
                            //财付通接口
                            frmmain.Attributes.Add("src", "../Home/Room/OnlinePay/Tenpay/PayQuery.aspx?sp_billno=" + PayNumber + "&date=" + payDate);
                        }
                        break;
                    case "007KA":
                        {
                            frmmain.Attributes.Add("src", "../Home/Room/OnlinePay/007ka/PayQuery.aspx?OrderID="+PayNumber);
                        }
                        break;
                    case "99BILL"://快钱冲值
                        {
                            string dealID="";
                            string errorMsg="";
                            if (this.Check99BillPay(long.Parse(PayNumber),ref dealID, ref errorMsg) && errorMsg=="")
                            {
                                string Memo = "系统交易号：" + PayNumber + ",快钱交易号：" + dealID;
                                ReturnDescription = "";
                                ReturnValue = -1;
                                int result = -1;
                                result = DAL.Procedures.P_UserAddMoney(_Site.ID, ID, Money, FormalitiesFees, PayNumber, PayBank, Memo, ref ReturnValue, ref ReturnDescription);

                                if (result < 0)
                                {
                                    Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误:" + _Site.ID+" , "+ID+" , "+Money+" , "+ FormalitiesFees+" , "+ PayNumber+" , "+ PayBank+" , "+ Memo );

                                    return;
                                }
                                else
                                {
                                    if (ReturnValue < 0)
                                    {
                                        Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
                                        return;
                                    }

                                    Shove._Web.JavaScript.Alert(this.Page, "此笔充值记录已到帐并已处理成功！");
                                    BindData();
                                }
                            }
                            else
                            {
                                Shove._Web.JavaScript.Alert(this.Page, errorMsg);
                            }
                            
                        }
                        break;
                }

            }

            if (e.CommandName == "Accept")
            {
                string Memo = "手动处理充值" + ((TextBox)e.Item.Cells[4].FindControl("tbDescription")).Text.Trim();

                Result = -1;

                Result = DAL.Procedures.P_UserAddMoney(_Site.ID, ID, Money, FormalitiesFees, PayNumber, PayBank, Memo, ref ReturnValue, ref ReturnDescription);

                if (Result < 0)
                {
                    ReturnDescription = "数据库读写错误";

                    return;
                }

                if (ReturnValue < 0)
                {
                    Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
                }

                Shove._Web.JavaScript.Alert(this.Page, "此笔充值处理成功！");
            }

            if (e.CommandName == "Del")
            {
                try
                {
                    new DAL.Tables.T_UserPayDetails().Delete("ID=" + PayNumber + " and Result = 0");
                }
                catch
                {
                    Shove._Web.JavaScript.Alert(this.Page, "此笔充值删除失败！");
                }

                Shove._Web.JavaScript.Alert(this.Page, "此笔充值删除成功！");
            }

            BindData();
        }
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {

            e.Item.Cells[4].Text = getBankName(e.Item.Cells[10].Text);

            e.Item.Cells[3].Text = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0).ToString("N");
            e.Item.Cells[5].Text = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0).ToString("N");
        }
    }

    protected void btn_Search_Click(object sender, EventArgs e)
    {
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) && !DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
                return;
            }
        }
        BindData();
    }


    //根据支付方式来获取相应的中文说明
    private string getBankName(string bankCode)
    {

        string bankName = "";
        string[] banks = bankCode.Split('_');

        if (banks.Length < 2)
        {
            return "未知银行";
        }

        if (banks[0].ToUpper() == "ALIPAY")
        {
            switch (banks[1].ToUpper())
            {
                case "ALIPAY":
                    bankName = "支付宝";
                    break;

                case "ICBCB2C":
                    bankName = "中国工商银行";
                    break;
                case "GDB":
                    bankName = "广东发展银行";
                    break;
                case "CEBBANK":
                    bankName = "中国光大银行";
                    break;
                case "CCB":
                    bankName = "中国建设银行";
                    break;
                case "COMM":
                    bankName = "中国交通银行";
                    break;
                case "ABC":
                    bankName = "中国农业银行";
                    break;
                case "SPDB":
                    bankName = "上海浦发银行";
                    break;
                case "SDB":
                    bankName = "深圳发展银行";
                    break;
                case "CIB":
                    bankName = "兴业银行";
                    break;
                case "HZCBB2C":
                    bankName = "杭州银行";
                    break;
                case "CMBC":
                    bankName = "杭州银行";
                    break;
                case "BOCB2C":
                    bankName = "中国银行";
                    break;
                case "CMB":
                    bankName = "中国招商银行";
                    break;
                case "CITIC":
                    bankName = "中信银行";
                    break;
                default:
                    bankName = "支付宝";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "TENPAY")
        {
            switch (banks[1].ToUpper())
            {
                case "0":
                    bankName = "财付通";

                    break;
                case "1001":
                    bankName = "招商银行";

                    break;
                case "1002":
                    bankName = "中国工商银行";

                    break;
                case "1003":
                    bankName = "中国建设银行";

                    break;
                case "1004":
                    bankName = "上海浦东发展银行";

                    break;
                case "1005":
                    bankName = "中国农业银行";

                    break;
                case "1006":
                    bankName = "中国民生银行";

                    break;
                case "1008":
                    bankName = "深圳发展银行";

                    break;
                case "1009":
                    bankName = "兴业银行";

                    break;
                case "1028":
                    bankName = "广州银联";

                    break;
                case "1032":
                    bankName = "   北京银行";

                    break;
                case "1020":
                    bankName = "   中国交通银行";

                    break;
                case "1022":
                    bankName = "   中国光大银行";

                    break;
                default:
                    bankName = "财付通";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "51ZFK")
        {
            switch (banks[1].ToUpper())
            {
                case "SZX":
                    bankName = "神州行充值卡";
                    break;

                case "ZFK":
                    bankName = "51支付卡";
                    break;
                default:
                    bankName = "神州行充值卡";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "007KA")
        {
            bankName = "007KA支付";
        }
        else if (banks[0].ToUpper() == "99BILL")
        {
            bankName = "快钱支付";
        }

        

   

        return bankName;

    }


    /// <summary>
    /// 查询快钱冲记录是否冲值成功
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected bool   Check99BillPay(long payNumber,ref string DealID, ref string  ErrorMessage)
    {
        SystemOptions so = new SystemOptions();
        string key = so["OnlinePay_99Bill_QueryMD5Key"].Value.ToString();
        string myMerchantAcctId = so["OnlinePay_99Bill_UserNumber"].Value.ToString();
        
        #region 构造请求付款数组
        //生成请求对象
        //GatewayOrderQueryRequest orderQueryRequest = new GatewayOrderQueryRequest();
        Interface99Bill.GatewayOrderQueryRequest orderQueryRequest = new Interface99Bill.GatewayOrderQueryRequest();
        //orderQueryRequest.

        //字符集
        //固定值：1
        //1代表UTF-8 
        orderQueryRequest.inputCharset = "1";

        //查询接口版本
        //固定值：v2.0
        //注意为小写字母
        orderQueryRequest.version = "v2.0";

        //签名类型
        //固定值：1
        //1代表MD5加密签名方式
        orderQueryRequest.signType = 1;

        //人民币账号
        //数字串
        //本参数用来指定接收款项的快钱用户的人民币账号
        orderQueryRequest.merchantAcctId = myMerchantAcctId;

        //查询方式
        //固定选择值：0、1
        //0按商户订单号单笔查询（返回该订单信息）
        //1按交易结束时间批量查询（只返回成功订单）
        orderQueryRequest.queryType = 0;

        //查询模式
        //固定值：1
        //1代表简单查询（返回基本订单信息）
        orderQueryRequest.queryMode = 1;

        //交易开始时间
        //数字串，一共14位
        //格式为：年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]，例如：20071117020101
        //orderQueryRequest.startTime ="";// PayDateTime.ToString("yyyyMMddHHmmss");// "20080303000000";

        //交易结束时间
        //数字串，一共14位
        //格式为：年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]，例如：20071117020101
        //orderQueryRequest.endTime = "";// PayDateTime.AddDays(1).ToString("yyyyMMddHHmmss");// 查询从冲时间起一天内的

        //请求记录集页码
        //数字串
        //在查询结果数据总量很大时，快钱会将支付结果分多次返回。本参数表示商户需要得到的记录集页码。
        //默认为1，表示第1页。
        orderQueryRequest.requestPage = "1";

        //商户订单号
        //字符串
        //只允许使用字母、数字、- 、_,并以字母或数字开头
        orderQueryRequest.orderId = payNumber.ToString();

        //构造签名字符串
        string tempMac = "inputCharset=" + orderQueryRequest.inputCharset +
                        "&version=" + orderQueryRequest.version +
                        "&signType=" + orderQueryRequest.signType +
                        "&merchantAcctId=" + orderQueryRequest.merchantAcctId +
                        "&queryType=" + orderQueryRequest.queryType +
                        "&queryMode=" + orderQueryRequest.queryMode +
                        //"&startTime=" + orderQueryRequest.startTime +
                        //"&endTime=" + orderQueryRequest.endTime +
                        "&requestPage=" + orderQueryRequest.requestPage +
                        "&orderId=" + orderQueryRequest.orderId +
                        "&key=" + key;

        orderQueryRequest.signMsg = GetMD5(tempMac, "utf-8").ToUpper();

        #endregion 构造请求付款数组

        //GatewayOrderQueryService orderQueryService = new GatewayOrderQueryService();
        Interface99Bill.GatewayOrderQueryService  orderQueryService = new Interface99Bill.GatewayOrderQueryService();


        //调用gatewayOrderQuery()方法
        Interface99Bill.GatewayOrderQueryResponse orderQueryResponse = orderQueryService.gatewayOrderQuery(orderQueryRequest);

        if (orderQueryResponse.errCode != "")//返回接口的错误码
        {

            //return false;
        }

        //打印支付结果数据
        if (orderQueryResponse != null)
        {

            string msgSginSource = "";

            //网关版本
            //固定值：v2.0
            //与提交时的查询版本号保持一致
            msgSginSource += "version=" + orderQueryResponse.version;

            //签名类型
            //固定值：1
            //与提交时的签名类型保持一致
            msgSginSource += "&signType=" + orderQueryResponse.signType;

            //人民币账号
            msgSginSource += "&merchantAcctId=" + orderQueryResponse.merchantAcctId;
            //错误代码
            msgSginSource +=string.IsNullOrEmpty(orderQueryResponse.errCode)? "": "&errCode=" + orderQueryResponse.errCode;
            //记录集当前页码
            msgSginSource += string.IsNullOrEmpty(orderQueryResponse.currentPage) ? "" : "&currentPage=" + orderQueryResponse.currentPage;
            //记录集总页码
            msgSginSource += string.IsNullOrEmpty(orderQueryResponse.pageCount) ? "" : "&pageCount=" + orderQueryResponse.pageCount;
            //记录集当页条数
            msgSginSource += string.IsNullOrEmpty(orderQueryResponse.pageSize) ? "" : "&pageSize=" + orderQueryResponse.pageSize;
            //记录集总条数
            msgSginSource += string.IsNullOrEmpty(orderQueryResponse.recordCount) ? "" : "&recordCount=" + orderQueryResponse.recordCount;
            msgSginSource += "&key="+key;

            string myMsgSign = GetMD5(msgSginSource, "utf-8").ToUpper();
            //签名验证			
            if (myMsgSign != orderQueryResponse.signMsg)
            {
                ErrorMessage= "[快钱]查询结果数据签名验证失败.";

                return false;
            }

            if (orderQueryResponse.orders == null || orderQueryResponse.orders.Length == 0)
            {
                ErrorMessage = "[快钱]没有此支付成功记录.";

                return false;
            }

            if(orderQueryResponse.orders.Length==1)
            {
                //商户订单号
                string msgSginSource2="";
                msgSginSource2+="orderId="+orderQueryResponse.orders[0].orderId;

                //商户订单金额,(整型数字)以分为单位
                msgSginSource2+="&orderAmount="+orderQueryResponse.orders[0].orderAmount;


                //商户订单提交时间, 数字串, 与提交订单时的商户订单提交时间保持一致
                msgSginSource2+="&orderTime="+orderQueryResponse.orders[0].orderTime;

                //快钱交易时间//数字串//快钱接收该笔交易并进行处理的最后时间。//格式为：年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]，例如：20071117020101
                msgSginSource2+="&dealTime="+orderQueryResponse.orders[0].dealTime;

                //处理结果//固定选择值：10、11、20//10：支付成功//只返回支付成功的记录
                 msgSginSource2+="&payResult="+orderQueryResponse.orders[0].payResult;

                //支付方式//固定选择值：00、10、11、12、13、14//10：银行卡支付//11：电话银行支付//12：快钱人民币账户支付//13：线下支付//14：B2B支付
                msgSginSource2+="&payType="+orderQueryResponse.orders[0].payType;

                //订单实际支付金额 整型数字  返回在使用优惠券等情况后，用户实际支付的金额  以分为单位。比方10元，提交时金额应为1000
                msgSginSource2+="&payAmount="+orderQueryResponse.orders[0].payAmount;

                //费用  数字串  快钱收取商户的手续费，单位为分。
                msgSginSource2+="&fee="+orderQueryResponse.orders[0].fee;

                //快钱交易号 数字串 该交易在快钱系统中对应的交易号d;
                msgSginSource2 +="&dealId="+orderQueryResponse.orders[0].dealId;

                msgSginSource2 += "&key="+key;

                string myMsgInfo = GetMD5(msgSginSource2, "utf-8").ToUpper();
                //签名验证			
                if (myMsgInfo != orderQueryResponse.orders[0].signInfo)
                {
                    ErrorMessage = "[快钱]查询支付记录的签名验证失败." + msgSginSource2;

                    return false;
                }
                
                if(orderQueryResponse.orders[0].orderId != payNumber.ToString())//比较返回的结果支付号与当前支付号
                {
                    ErrorMessage = "[快钱]返回的订单号不一致." + msgSginSource2;

                    return false;
                }

                if (orderQueryResponse.orders[0].payResult != "10")
                {
                    ErrorMessage = "[快钱]此支付记录没有成功.";
                    return false;
                }
                else
                {
                    DealID = orderQueryResponse.orders[0].dealId;//快钱交易号
                    return true;
                }
            }

            
        }

        ErrorMessage = "[快钱]未知原因";
        return false;
    }



    //快钱 功能函数。将字符串进行编码格式转换，并进行MD5加密，然后返回。开始
    private static string GetMD5(string dataStr, string codeType)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(codeType).GetBytes(dataStr));
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        for (int i = 0; i < t.Length; i++)
        {
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        }
        return sb.ToString();
    }
    //功能函数。将字符串进行编码格式转换，并进行MD5加密，然后返回。结束

}
