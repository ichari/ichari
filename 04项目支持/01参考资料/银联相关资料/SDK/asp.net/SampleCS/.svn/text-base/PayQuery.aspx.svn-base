<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayQuery.aspx.cs" Inherits="SampleCS.PayQuery" %>
<%@ Import Namespace="com.unionpay.upop.sdk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        请输入要查询的订单号：<input type ="text" name="txtOrderID" size="20" /><br />
        请输入要查询的订单时间：<input type ="text" name="txtOrderTime" size="20" /><br />
        <input type="submit" value=" 确定" />
        
    </div>
    </form>
    <div>
    <%
        // **************演示交易查询接口***********************
        
        if (Request.HttpMethod == "POST")
        {
            // 要使用各种Srv必须先使用LoadConf载入配置
            UPOPSrv.LoadConf(Server.MapPath("./App_Data/conf.xml.config"));
            
            //获取订单号和订单时间
            string orderNumber = Request.Form["txtOrderID"];
            DateTime orderTime = DateTime.Parse(Request.Form["txtOrderTime"]);

            // 使用Dictionary保存参数
            System.Collections.Generic.Dictionary<string,string> param = new System.Collections.Generic.Dictionary<string,string> ();

            // 填写参数
            param["transType"] = UPOPSrv.TransType.CONSUME ;
            param["orderNumber"] = orderNumber;
            param["orderTime"] = orderTime.ToString("yyyyMMddHHmmss");
            
            // 创建后台交查询务对象
            QuerySrv srv = new QuerySrv (param);

            // 请求查询服务，得到SrvResponse回应对象
            SrvResponse resp = srv.Request() ;

            Response.Write("<h1>");
            
            string queryStat = "";
            if (resp.ResponseCode == SrvResponse.RESP_SUCCESS ) 
            {
                switch (resp.Fields["queryResult"].Trim()) // 根据queryResult字段来判断交易状态
                {
                    case QuerySrv.QUERY_SUCCESS:
                        queryStat = "交易成功";
                        break;
                    case QuerySrv.QUERY_WAIT:
                        queryStat = "交易正在进行中";
                        break;
                    default:
                        queryStat = "未知状态";
                        break;
                }
                
            }
            else // respCode 不为 RESP_SUCCESS 时，则可能是交易失败，也可能是本次查询请求出错
            {
                // queryResult == QUERY_FAIL 时，代表交易失败。此时ResponseCode表示失败原因
                if (resp.HasField ("queryResult") && resp.Fields["queryResult"].Trim() == QuerySrv.QUERY_FAIL) 
                {
                    queryStat = string.Format ("交易失败  <h3>ErrorCode=[{0}]</h3>", resp.ResponseCode );
                }
                else // 否则则为本次查询请求出错
                {
                    string msg = resp.HasField("respMsg") ? resp.Fields["respMsg"] : "";
                    Response.Write(String.Format("Error [{0}] : {1} ", resp.Fields["respCode"], msg));
                }
            }

            if (queryStat != "") { Response.Write("交易状态：" + queryStat); }
            Response.Write("</h1><br/>");
            Response.Write("post string:" + resp.OrigPostString);
            
        }
            
    %>
    </div>
</body>
</html>
