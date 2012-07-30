<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IncomeDetail.aspx.cs" Inherits="Cps_Admin_IncomeDetail" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <div style="text-align:right">
        <asp:DataGrid ID="g" runat="server" BorderColor="Silver" BorderStyle="None" BackColor="White" Font-Names="宋体" AutoGenerateColumns="False" CellPadding="1"
            Width="99%" onitemdatabound="g_ItemDataBound" 
            onitemcommand="g_ItemCommand">
            <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
            <SelectedItemStyle ForeColor="#663399"></SelectedItemStyle>
            <HeaderStyle  HorizontalAlign="Center" Height="30" VerticalAlign="Middle" BackColor="WhiteSmoke"></HeaderStyle>
            <ItemStyle Height="28" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundColumn DataField="Name" HeaderText="会员">
                    <HeaderStyle Width="100px"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="IsuseName" HeaderText="期号">
                    <HeaderStyle Width="120px"></HeaderStyle>
                </asp:BoundColumn>
                 <asp:BoundColumn DataField="SchemeNumber" HeaderText="方案号" Visible="false" >
                    <HeaderStyle  Width="120px"></HeaderStyle>
                </asp:BoundColumn> 
                <asp:TemplateColumn HeaderText="方案号">
                    <ItemTemplate>
                         <asp:LinkButton  ID="lbtnViewScheme" runat="server" CommandName="ViewScheme"  Text=""></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="LotteryName" HeaderText="彩种">
                    <HeaderStyle  Width="80px"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="PlayTypeName" HeaderText="方案类型">
                    <HeaderStyle Width="100px"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Money" HeaderText="方案金额" DataFormatString="{0:N}">
                    <HeaderStyle Width="80px"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="DetailMoney" HeaderText="购买金额" DataFormatString="{0:N}">
                    <HeaderStyle  Width="80px"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="DateTime" HeaderText="购买时间">
                    <HeaderStyle  Width="120px"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="SchemeID"  HeaderText="方案ID" Visible="false" >
                </asp:BoundColumn>
            </Columns>
            <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
            </PagerStyle>
        </asp:DataGrid>
        <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="16"
            ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
            HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore" SelectRowColor="#FF9933" />
    </div>
   
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
