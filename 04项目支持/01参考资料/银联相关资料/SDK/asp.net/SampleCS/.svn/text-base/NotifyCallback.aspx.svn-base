<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotifyCallback.aspx.cs" Inherits="SampleCS.NotifyCallback" %>
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
        // 要使用各种Srv必须先使用LoadConf载入配置
        UPOPSrv.LoadConf(Server.MapPath("./App_Data/conf.xml.config"));
        
        // 使用Post过来的内容构造SrvResponse
        SrvResponse resp = new SrvResponse (Util.NameValueCollection2StrDict(Request.Form ));
        
        // 收到回应后做后续处理（这里写入文件，仅供演示）
        System.IO.StreamWriter sw = new System.IO.StreamWriter(Server.MapPath ("./notify_data.txt"));
        
        if(resp.ResponseCode != SrvResponse.RESP_SUCCESS )
        {
            sw.WriteLine("error in parsing response message! code:" + resp.ResponseCode ); 
        }
        else
        {
            foreach (string k in resp.Fields.Keys  )
            {
                sw.WriteLine (k + "\t = " + resp.Fields[k]);
            }
        }
        
        sw.Close (); 
        
        
        
         %>
    </div>
    </form>
</body>
</html>
