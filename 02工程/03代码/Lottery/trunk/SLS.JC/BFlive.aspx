<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BFlive.aspx.cs" Inherits="BFlive" %>
<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register src="Home/Room/UserControls/WebFoot.ascx" tagname="WebFoot" tagprefix="uc1" %>
<%@ Register src="Home/Room/UserControls/WebHead.ascx" tagname="WebHead" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <title><%=_Site.Name %>-竞彩足球 竞彩篮球比分直播</title>
    <meta name="description" content="湖北省体育彩票管理中心-竞彩足球 竞彩篮球比分直播" />
    <meta name="keywords" content="湖北省体育彩票管理中心-竞彩足球 竞彩篮球比分直播" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <uc2:WebHead ID="WebHead1" runat="server" />
    <div style="width:1002px; padding:0; margin:0 auto;">
        <table cellpadding='0' cellspacing="0" border="0" width="100%" style="line-height: 2; margin-top: 5px;">
            <tr>
                <td style="border-bottom: #6db8f3 1px solid;">
                    <iframe src='http://www.spbo.com/bf.htm'
                        width='100%' height='700px'></iframe>
                </td>
            </tr>
        </table>
    </div>
    <div class="cl">
    </div>
    <!--footer-->
    <uc1:WebFoot ID="WebFoot1" runat="server" />
    <!--footer end-->
    </form>
</body>
</html>