<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserRegAgree.aspx.cs" Inherits="Home_Web_UserRegAgree" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户注册协议-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。" />
    <meta name="keywords" content="用户注册协议" />
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .pass_password_strong
        {
            border-right: #bebebe 1px solid;
            border-left: #ffffff 1px solid;
            color: #999999;
            border-bottom: #bebebe 1px solid;
            background-color: #ebebeb;
            padding: 5px;
        }
    </style>
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body style="text-align: center">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <div>
        <asp:Panel ID="pStep1" runat="server" EnableViewState="false">
            <table width="1000px" border="0" cellpadding="0" cellspacing="0" bgcolor="#9BBFCA" style=" margin:0 auto;">
                <tr>
                    <td valign="top" bgcolor="#FFFFFF" class="bg_x" style="padding: 12px 20px 12px 20px;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <img src="images/user_title_1.gif" width="327" height="33" />
                                </td>
                            </tr>
                            <tr>
                                <td height="12">
                                </td>
                            </tr>
                            <tr>
                                <td height="1" bgcolor="#CCCCCC">
                                </td>
                            </tr>
                            <tr>
                                <td height="12">
                                </td>
                            </tr>
                        </table>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="2" class="hui12">
                                    <div style="height: 100%; width: 100%; text-align:left;">
                                        <asp:Label ID="labAgreement" runat="server" EnableViewState="false"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                            <td>
                                <a onclick="window.opener=null" href="javascript:window.close()" style="color:Red;">我已了解，关闭此页</a>
                            </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
