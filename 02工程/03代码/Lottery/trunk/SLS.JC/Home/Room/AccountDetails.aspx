<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountDetails.aspx.cs" Inherits="Home_Room_AccountDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
        <link type="text/css" rel="stylesheet" href="/Style/public.css" />
       <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
            <input type="hidden" id="hdCurDiv" runat="server" value="divBuy" />
    <div id="divAccountDetail">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-left: 1px solid #C0DBF9;
                        border-right: 1px solid #C0DBF9;">
                        <tr>
                            <td height="35" colspan="8" align="left" bgcolor="#F3F3F3" style="padding: 5px 10px 5px 2px;">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="black12">
                                            开始时间：
                                            <asp:DropDownList ID="ddlYear" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                                <asp:ListItem Value="1">1 月</asp:ListItem>
                                                <asp:ListItem Value="2">2 月</asp:ListItem>
                                                <asp:ListItem Value="3">3 月</asp:ListItem>
                                                <asp:ListItem Value="4">4 月</asp:ListItem>
                                                <asp:ListItem Value="5">5 月</asp:ListItem>
                                                <asp:ListItem Value="6">6 月</asp:ListItem>
                                                <asp:ListItem Value="7">7 月</asp:ListItem>
                                                <asp:ListItem Value="8">8 月</asp:ListItem>
                                                <asp:ListItem Value="9">9 月</asp:ListItem>
                                                <asp:ListItem Value="10">10月</asp:ListItem>
                                                <asp:ListItem Value="11">11月</asp:ListItem>
                                                <asp:ListItem Value="12">12月</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlDay" runat="server">
                                            </asp:DropDownList>
                                            结束时间：
                                            <asp:DropDownList ID="ddlYear1" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlMonth1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                                <asp:ListItem Value="1">1 月</asp:ListItem>
                                                <asp:ListItem Value="2">2 月</asp:ListItem>
                                                <asp:ListItem Value="3">3 月</asp:ListItem>
                                                <asp:ListItem Value="4">4 月</asp:ListItem>
                                                <asp:ListItem Value="5">5 月</asp:ListItem>
                                                <asp:ListItem Value="6">6 月</asp:ListItem>
                                                <asp:ListItem Value="7">7 月</asp:ListItem>
                                                <asp:ListItem Value="8">8 月</asp:ListItem>
                                                <asp:ListItem Value="9">9 月</asp:ListItem>
                                                <asp:ListItem Value="10">10月</asp:ListItem>
                                                <asp:ListItem Value="11">11月</asp:ListItem>
                                                <asp:ListItem Value="12">12月</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlDay1" runat="server">
                                            </asp:DropDownList>
                                            <ShoveWebUI:ShoveConfirmButton ID="btnGO" runat="server" CssClass="btn_s" OnClick="btnGO_Click" Text="查询" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:DataGrid ID="g" runat="server" width="100%" BorderStyle="None" AllowPaging="True"
                        PageSize="30" AutoGenerateColumns="False" CellPadding="0" BackColor="#DDDDDD"
                        Font-Names="Tahoma" OnItemDataBound="g_ItemDataBound" CellSpacing="1" GridLines="None"
                        OnSortCommand="g_SortCommand">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px"
                            CssClass="blue12_2"></HeaderStyle>
                        <ItemStyle BackColor="White" BorderStyle="None" Height="30px" HorizontalAlign="Center"
                            CssClass="black12" />
                        <Columns>
                            <asp:BoundColumn DataField="DateTime" SortExpression="DateTime" HeaderText="交易时间">
                                <ItemStyle Width="17%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Memo" SortExpression="Memo" HeaderText="摘要">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="MoneyAdd" SortExpression="MoneyAdd" HeaderText="收入(元)">
                                <ItemStyle Width="8%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="MoneySub" SortExpression="MoneySub" HeaderText="支出(元)">
                                <ItemStyle Width="8%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FormalitiesFees" SortExpression="FormalitiesFees" HeaderText="(手续费)">
                                <ItemStyle Width="8%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Balance" SortExpression="Balance" HeaderText="余额">
                                <ItemStyle Width="8%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Reward" SortExpression="Reward" HeaderText="中奖金额">
                                <ItemStyle Width="8%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SumReward" SortExpression="SumReward" HeaderText="中奖总金额">
                                <ItemStyle Width="8%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="SchemeID" HeaderText="SchemeID"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" width="100%" PageSize="30" ShowSelectColumn="False"
                        DataGrid="g" AlternatingRowColor="#F8F8F8" HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange"
                        Visible="False" RowCursorStyle="" />
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                        <tr>
                            <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                支出交易笔数： <span class="red12">
                                    <asp:Label ID="lblOutCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                            <td width="390" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                收入交易笔数： <span class="red12">
                                    <asp:Label ID="lblInCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                支出金额合计： <span class="red12">
                                    <asp:Label ID="lblOutMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                收入金额合计： <span class="red12">
                                    <asp:Label ID="lblInMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" bgcolor="#FFFEDF" class="black12" style="padding: 5px 10px 5px 10px;">
                                <span class="blue12">说明：</span><br />
                                1、您可以查询您的帐户在一段时间内的所有交易流水。<br />
                                2、如果你已经充值，银行账户钱扣了，而您的账户还没有加上，请点击<a style="color: Red;" href="javascript:;"><span
                                        class="blue12_2">在线客服</span></a>告诉我们，我们将第一时间为您处理！<br />
                                3、如有其他问题，请联系网站客服：<%= _Site.ServiceTelephone %>。
                            </td>
                        </tr>
                    </table>
                </div>
    </form>
</body>
</html>
