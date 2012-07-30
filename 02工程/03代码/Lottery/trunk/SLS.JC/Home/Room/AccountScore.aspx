<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountScore.aspx.cs" Inherits="Home_Room_AccountScore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="hdCurDiv" runat="server" value="divBuy" />
   <div id="divScoring">
                    <asp:DataGrid ID="gScoring" runat="server" width="100%" BorderStyle="None" AllowSorting="True"
                        AllowPaging="True" PageSize="30" AutoGenerateColumns="False" CellPadding="0"
                        OnSortCommand="g_SortCommand" BackColor="#DDDDDD" Font-Names="Tahoma" OnItemDataBound="gScoring_ItemDataBound"
                        CellSpacing="1" GridLines="None">
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px"
                            CssClass="blue12_2"></HeaderStyle>
                        <AlternatingItemStyle BackColor="#F8F8F8" />
                        <ItemStyle BackColor="White" BorderStyle="None" Height="30px" HorizontalAlign="Center"
                            CssClass="black12" />
                        <Columns>
                            <asp:BoundColumn DataField="ID" HeaderText="订单号">
                                <ItemStyle Width="17%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Scoring" HeaderText="收入">
                                <ItemStyle Width="9%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="支出">
                                <ItemStyle Width="10%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="OperatorType" HeaderText="交易类型">
                                <ItemStyle Width="11%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="DateTime" HeaderText="交易时间">
                                <ItemStyle Width="17%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Scoring" HeaderText="积分">
                                <ItemStyle Width="17%" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPagerScoring" runat="server" width="100%" PageSize="30"
                        ShowSelectColumn="False" DataGrid="gScoring" AlternatingRowColor="#F8F8F8" GridColor="#E0E0E0"
                        HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" AllowShorting="true"
                        RowCursorStyle="" />
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                        <tr>
                            <td width="368" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                支出交易笔数： <span class="red12">
                                    <asp:Label ID="lblScoreOutCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                            <td width="407" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                收入交易笔数： <span class="red12">
                                    <asp:Label ID="lblScoreInCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                支出积分合计： <span class="red12">
                                    <asp:Label ID="lblScoreOutMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                收入积分总计： <span class="red12">
                                    <asp:Label ID="lblScoreInMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" bgcolor="#FFFEDF" class="black12" style="padding: 5px 10px 5px 10px;">
                                <span class="blue12">说明：</span><br />
                                1、您可以查询您的帐户在一段时间内的所有交易流水。<br />
                                2、如果你已经充值，银行账户钱扣了，而您的账户还没有加上，请点击<span class="blue12_2">在线客服</span>告诉我们，我们将第一时间为您处理！<br />
                                3、如有其他问题，请联系网站客服：<%= _Site.ServiceTelephone %>。
                            </td>
                        </tr>
                    </table>
                </div>
    </form>
</body>
</html>
