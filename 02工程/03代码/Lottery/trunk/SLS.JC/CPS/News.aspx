<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="CPS_News" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register Src="UserControls/Head.ascx" TagName="Head" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兼职推广联盟</title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Head ID="Head1" runat="server" />
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
                                        推广首页&nbsp;&gt;&nbsp;<asp:Label runat="server" ID="lbType" Text="新闻公告" />&nbsp;&gt;&nbsp;新闻列表
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" bgcolor="#FFFFFF" style="padding: 8px 10px 8px 10px; background-repeat: repeat-x;
                            background-position: center top;" class="hui">
                            <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                <ShoveWebUI:ShoveDataList ID="sdlNewsList" runat="server" AllowPaging="True" NextPageText="下一页"
                                    PageMode="NextPrev" PageSize="10" PrevPageText="上一页" Width="100%" OnPageIndexChanged="sdlNewsList_PageIndexChanged"
                                    OnItemDataBound="sdlNewsList_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td width="3%" height="24" align="center">
                                                <img src="images/dian.jpg" width="3" height="3" />
                                            </td>
                                            <td width="97%" height="24" align="left" class="hui">
                                                <asp:HyperLink ID="hlNews" runat="server" Target="_blank"></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </ShoveWebUI:ShoveDataList>
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
