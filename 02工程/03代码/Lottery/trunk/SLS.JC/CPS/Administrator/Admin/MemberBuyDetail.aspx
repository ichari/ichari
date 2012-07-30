<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MemberBuyDetail.aspx.cs" Inherits="CPS_Administrator_Admin_MemberBuyDetail " %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <div style="text-align:right">
        
        <asp:DataGrid ID="g" runat="server" BorderColor="Silver" BorderStyle="None" BackColor="White" Font-Names="宋体" AutoGenerateColumns="False" CellPadding="1"
            Width="95%">
            <ItemStyle Height="28px" HorizontalAlign="Center" />
            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" BackColor="WhiteSmoke" Height="30px"></HeaderStyle>
            <Columns>
                <asp:BoundColumn DataField="Name" HeaderText="会员">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="IsuseName" HeaderText="期号">
                </asp:BoundColumn>
                <asp:HyperLinkColumn DataNavigateUrlField="SchemeID" DataTextField="SchemeNumber" Target="_blank" 
                    HeaderText="方案号" DataNavigateUrlFormatString="../../Home/Room/Scheme.aspx?id={0}" ItemStyle-ForeColor="Blue">
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="LotteryName" HeaderText="彩种">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="PlayTypeName" HeaderText="方案类型">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Money" HeaderText="方案金额" DataFormatString="{0:N}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="DetailMoney" HeaderText="购买金额" DataFormatString="{0:N}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="DateTime" HeaderText="购买时间" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                </asp:BoundColumn>
               
            </Columns>
            <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
            </PagerStyle>
        </asp:DataGrid>
        <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="16"
            ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
            HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore"
            class="black12" CssClass="black12" SelectRowColor="#FF9933" />
    </div>
   
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
