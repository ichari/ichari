<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsShow.aspx.cs" Inherits="CPS_NewsShow" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register Src="UserControls/Head.ascx" TagName="Head" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>兼职推广联盟</title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Head ID="Head2" runat="server" />
    <table width="981" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;"
        align="center">
        <tr>
            <td width="220" valign="top">
                <table width="220" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                    <tr>
                        <td height="32" background="images/index-1_28.gif" bgcolor="#FFFFFF" class="blue12"
                            style="padding-left: 10px;">
                            <strong>新闻公告</strong>
                        </td>
                    </tr>
                    <tr>
                        <td height="70" background="images/index-1_32.gif" bgcolor="#FFFFFF">
                            <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="10%" align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td width="90%" height="26" class="hui">
                                        <a href="News.aspx">新闻公告</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="News.aspx?TypeID=2">推广指南</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td rowspan="2" valign="top">
                <table width="754" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                    <tr>
                        <td height="33" background="images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                            <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td class="hui">
                                        <a href="Default.aspx" style="text-decoration: none">CPS 联盟首页</a> &gt;<asp:Label
                                            ID="lblType" runat="server" Text="Label"></asp:Label>
                                        &gt;新闻详细
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top" bgcolor="#FFFFFF" class="black12" style="padding: 8px 10px 8px 10px;
                            background-image: url(images/bg_lan.jpg); background-repeat: repeat-x; background-position: center top;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td height="30" align="center" class="red14">
                                        <asp:Label ID="lbTitle" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="20" align="right" background="images/bg_line.gif" class="hui">
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" align="right" class="blue" style="padding-right: 20px;">
                                        <asp:Label ID="lbDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="20" background="images/bg_line.gif" class="black12">
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" align="left" class="black12">
                                        <asp:Label ID="lbDetail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5px;">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <uc2:Foot ID="Foot1" runat="server" />
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
    </form>
</body>
</html>
