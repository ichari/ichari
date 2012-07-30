<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailReg.aspx.cs" Inherits="Home_Room_EmailReg" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=_Site.Name %>购彩激活邮箱-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
     
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <table width="1000px" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8"
        style="margin-top: 20px; margin:0 auto;" align="center">
        <tr>
            <td align="left" bgcolor="#FFFFDD" class="black12" style="padding: 5px 10px 5px 30px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center" >
                    <tbody id="tbOk" runat ="server">
                        <tr>
                            <td width="9%" style="height:45px">
                                <img src="images/icon_cg222.jpg" width="73" height="52" />
                            </td>
                            <td width="91%" class="red14_2">
                                邮箱激活成功！
                            </td>
                        </tr>
                    </tbody>
                    <tbody id="tbFailure" runat="server">
                        <tr>
                            <td width="91%" class="red14_2" colspan="2" style="height:45px ;">
                                邮箱激活失败！
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#FFFFFF" style="height:60px;padding-left:30px">
            
               <asp:Label ID="labValided" runat="server" />
            </td>
        </tr>
    </table>
    <table width="1000px" border="0" cellspacing="0" cellpadding="0" align="center">
         <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
        <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
