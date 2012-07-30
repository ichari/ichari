<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountBuy.aspx.cs" Inherits="Home_Room_BuyAccount" %>

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
     <div id="divBuy" >
                    <asp:DataGrid ID="gHistory" runat="server" Width="100%" BorderStyle="None" AllowPaging="True"
                        PageSize="30" AutoGenerateColumns="False" CellPadding="0" BackColor="#DDDDDD"
                        Font-Names="Tahoma" OnItemDataBound="gHistory_ItemDataBound" CellSpacing="1"
                        GridLines="None" AllowSorting="True" OnSortCommand="g_SortCommand" 
                        BorderColor="#E0E0E0" BorderWidth="2px">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px"
                            CssClass="blue12_2" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle BorderStyle="None" Height="30px" HorizontalAlign="Center"
                            CssClass="black12" />
                        <Columns>
                            <asp:BoundColumn SortExpression="InitiateName" DataField="InitiateName" HeaderText="发起人">
                            </asp:BoundColumn>
                             <asp:BoundColumn SortExpression="LotteryName" DataField="LotteryName" HeaderText="彩种">
                            </asp:BoundColumn>
                            <asp:BoundColumn SortExpression="PlayTypeName" DataField="PlayTypeName" HeaderText="玩法">
                            </asp:BoundColumn>
                            <asp:BoundColumn SortExpression="SchemeShare" DataField="SchemeShare" HeaderText="方案份数">
                            </asp:BoundColumn>
                            <asp:BoundColumn SortExpression="Money" DataField="Money" HeaderText="方案金额"></asp:BoundColumn>
                            <asp:BoundColumn SortExpression="Share" DataField="Share" HeaderText="认购份数"></asp:BoundColumn>
                            <asp:BoundColumn SortExpression="DetailMoney" DataField="DetailMoney" HeaderText="认购金额">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="所占股份"></asp:BoundColumn>
                            <asp:BoundColumn SortExpression="SchemeWinMoney" DataField="SchemeWinMoney" HeaderText="方案奖金">
                            </asp:BoundColumn>
                            <asp:BoundColumn SortExpression="WinMoneyNoWithTax" DataField="WinMoneyNoWithTax"
                                HeaderText="您的奖金"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="是否中奖"></asp:BoundColumn>                           
                                 <asp:HyperLinkColumn DataNavigateUrlField="SchemeID" DataTextField="DateTime" 
                                DataTextFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="发起时间"  DataNavigateUrlFormatString="/Home/Room/Scheme.aspx?id={0}"
                                 SortExpression="DateTime" Target="_blank" 
                                Visible="true"></asp:HyperLinkColumn>
                            <asp:BoundColumn HeaderText="状态"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="SchemeID" HeaderText="SchemeID"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="QuashStatus" HeaderText="QuashedStatus">
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Buyed" HeaderText="Buyed"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="AssureMoney" HeaderText="AssureMoney">
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BuyedShare" HeaderText="BuyedShare">
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPagerHistory" runat="server" width="100%" PageSize="30"
                        ShowSelectColumn="False" DataGrid="gHistory" AlternatingRowColor="#F8F8F8" GridColor="#E0E0E0"
                        HotColor="MistyRose" RowCursorStyle="" OnPageWillChange="gPager_PageWillChange"
                        AllowShorting="true" />
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                        <tr>
                            <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                支出交易笔数： <span class="red12">
                                    <asp:Label ID="lblBuyOutCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                支出金额合计： <span class="red12">
                                    <asp:Label ID="lblBuyOutMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#FFFEDF" class="black12" style="padding: 5px 10px 5px 10px;">
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
