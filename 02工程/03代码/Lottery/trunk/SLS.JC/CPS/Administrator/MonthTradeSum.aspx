<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonthTradeSum.aspx.cs" Inherits="Cps_Administrator_MonthTradeSum" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>佣金发放通知</title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
    <link href="../../Style/main.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../../Components/My97DatePicker/WdatePicker.js"></script>
    <style>
        body
        {
            background-image:none;
        }
    </style>

</head>
<body>
    <div>
        <br />
        <form id="form1" runat="server">
          <table width="100%" border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td>
                    <asp:DataGrid ID="g" runat="server"  BorderStyle="None" AllowSorting="True"
                        AllowPaging="True" PageSize="31" AutoGenerateColumns="False" CellPadding="2"
                        BackColor="#CCCCCC" Font-Names="宋体" CellSpacing="1"
                        GridLines="None"  DataKeyField="AccountMonth" >
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                            BackColor="#E9F1F8" Height="25px" CssClass="black12"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="IsPayOff" HeaderText="是否已经发放" Visible="false">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="AccountMonth" HeaderText="时间(年月)">
                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CpsCount" HeaderText="累计会员数">
                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="MonthNewCpsMemberCount" HeaderText="本月新增会员数">
                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="BuyMoney" HeaderText="本月交易量" DataFormatString="{0:N2}">
                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PayBonus" HeaderText="收入金额" DataFormatString="{0:N2}">
                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            
                            <asp:HyperLinkColumn DataNavigateUrlField="AccountMonth"  HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                                DataNavigateUrlFormatString="PerDayTradeSumOfMonthByType.aspx?YearMonth={0}"  
                                HeaderText="操作"   Target="_self" Text="交易明细">
                            </asp:HyperLinkColumn>
                            
                 
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <table bgcolor="#E9F1F8" 
                        style="　padding: 5px; border-collapse: collapse; font-size: 12px;" 
                        border="0px" cellpadding="2" cellspacing="0">
                        <tr>
                            <td width="102px" align="right"></td>
                            <td width="102px" align="right"></td>
                            <td width="100px" align="right">合计：</td>
                            <td width="100px" ><asp:Label  runat="server" ID="lbTotalBuyMoney" ForeColor="#CC0000"></asp:Label></td>
                            <td width="102px"　><asp:Label  runat="server" ID="lbTotalBonus" ForeColor="#CC0000"></asp:Label></td>
                            <td width="102px"　style="color: #CC0000"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr bgcolor="#f7f7f7" style="display:none">
                <td >
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="31" 
                        ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"    HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore"
                        class="black12" CssClass="black12" SelectRowColor="#FF9933" />
                </td>
            </tr>
         </table>
        </form>
        <br />
    </div>
</body>
</html>
