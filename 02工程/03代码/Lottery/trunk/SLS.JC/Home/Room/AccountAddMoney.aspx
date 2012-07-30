<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountAddMoney.aspx.cs" Inherits="Home_Room_AccountAddMoney" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server"> <input type="hidden" id="hdCurDiv" runat="server" value="divBuy" />
    <div id="divUserPay">
                    <asp:DataGrid ID="gUserPay" runat="server" width="100%" BorderStyle="None" AllowSorting="True"
                        AllowPaging="True" PageSize="8" AutoGenerateColumns="False" CellPadding="0"
                        OnSortCommand="g_SortCommand" BackColor="#DDDDDD" Font-Names="Tahoma" OnItemDataBound="gUserPay_ItemDataBound"
                        CellSpacing="1" GridLines="None" OnItemCommand="gUserPay_ItemCommand">
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px"
                            CssClass="blue12_2"></HeaderStyle>
                        <AlternatingItemStyle BackColor="#F8F8F8" />
                        <ItemStyle BackColor="White" BorderStyle="None" Height="30px" HorizontalAlign="Center"
                            CssClass="black12" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="订单号">
                                <ItemStyle Width="30%" />
                                <ItemTemplate><%#Eval("ID").ToString()%>
                                    <asp:HiddenField ID="tdUserPayDetail" runat="server" Value='<%#Eval("ID").ToString()%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Money" HeaderText="收入(元)">
                                <ItemStyle Width="20%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FormalitiesFees" HeaderText="(手续费)">
                                <ItemStyle Width="10%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PayType" HeaderText="交易类型">
                                <ItemStyle Width="10%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="DateTime" HeaderText="交易时间">
                                <ItemStyle Width="30%" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPagerUserPay" runat="server" width="100%" PageSize="8"
                        ShowSelectColumn="False" DataGrid="gUserPay" AlternatingRowColor="#F8F8F8" GridColor="#E0E0E0"
                        HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" AllowShorting="true"
                        RowCursorStyle="" />
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                         <tbody id="isShowUserPay" runat="server">
                            <tr>
                                <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                    充值金额： <span class="red12">
                                        <asp:Label ID="lblUserPayMoneys" runat="server" Text=""></asp:Label>
                                    </span>
                                </td>
                                <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                    充值时间： <span class="red12">
                                        <asp:Label ID="lblUserPayTime" runat="server" Text=""></asp:Label>
                                    </span>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;" colspan="2" >
                                    充值方式： <span class="red12">
                                        <asp:Label ID="lblUserPayBank" runat="server" Text=""></asp:Label>
                                    </span>
                                </td>
                            </tr>
                        </tbody>
                        <tr>
                            <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;"
                                colspan="2">
                                充值次数： <span class="red12">
                                    <asp:Label ID="lblUserPayCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;" colspan="2">
                                充值金额合计： <span class="red12">
                                    <asp:Label ID="lblUserPayMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#FFFEDF" class="black12" style="padding: 5px 10px 5px 10px;" colspan="2">
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
