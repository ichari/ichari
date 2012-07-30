<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Jczcdg.aspx.cs" Inherits="Admin_Jczcdg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </br>
        <table cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td align="center">
                    <asp:DataList ID="g" runat="server" Width="100%" GridLines="Horizontal">
                        <HeaderTemplate>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="td4" width="10%">
                                        <strong>赛事编号</strong>
                                    </td>
                                    <td class="td4" width="10%">
                                        <strong>联赛</strong>
                                    </td>
                                    <td class="td4" width="20%">
                                        <strong>截止时间</strong>
                                    </td>
                                    <td class="td4" width="15%">
                                        <strong>主队</strong>
                                    </td>
                                    <td class="td4" width="10%">
                                        <strong>让球</strong>
                                    </td>
                                    <td class="td4" width="15%">
                                        <strong>客队</strong>
                                    </td>
                                    <td class="td4" width="20%">
                                        <strong>修改</strong>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="10%">
                                        <%# DataBinder.Eval(Container.DataItem, "MatchNumber")%>
                                    </td>
                                    <td width="10%">
                                        <%# DataBinder.Eval(Container.DataItem, "Game")%>
                                    </td>
                                    <td width="20%">
                                        <%# DataBinder.Eval(Container.DataItem, "StopSellTime")%>
                                    </td>
                                    <td width="15%">
                                        <%# DataBinder.Eval(Container.DataItem, "MainTeam")%>
                                    </td>
                                    <td width="10%">
                                        <%# DataBinder.Eval(Container.DataItem, "MainLoseball")%>
                                    </td>
                                    <td width="15%">
                                        <%# DataBinder.Eval(Container.DataItem, "GuestTeam")%>
                                    </td>
                                    <td width="20%">
                                        <a href='JczcdgEdit.aspx?ID=<%# DataBinder.Eval(Container.DataItem,"ID")%>'>
                                            修改</a>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
