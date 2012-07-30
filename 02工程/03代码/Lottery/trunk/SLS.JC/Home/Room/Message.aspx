<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Message.aspx.cs" Inherits="Home_Room_Message" %>
<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>我的站内信-我的购彩纪录-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acMessage" />
        </div>
        <div id="menu_right">
            <table width="100%" style="border:#E4E4E5 1px solid;"  border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="32" align="right" valign="middle" class="red14">
                        <img src="images/icon_6.gif" width="19" height="16" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的彩票记录
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" class="bg1x bgp5 mt10" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="100" id="tdHistory" align="center" background="images/admin_qh_100_1.jpg"
                        style="cursor: pointer;" class="afff fs14">
                        我的站内信
                    </td>
                    <td width="10" height="29" align="left">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table id="myIcaileTab" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#DDDDDD">
                <tr>
                    <td valign="top" style="background-color: White;">
                        <asp:DataGrid ID="g1" runat="server" GridLines="None" CellPadding="2" BackColor="#DDDDDD"
                            BorderStyle="None" Width="100%" AutoGenerateColumns="False" 
                            CellSpacing="1" onitemcommand="g1_ItemCommand" 
                            onitemdatabound="g1_ItemDataBound">
                            <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                            <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px" CssClass="blue12_2"></HeaderStyle>
                            <AlternatingItemStyle BackColor="#F8F8F8" />
                            <ItemStyle BackColor="White" BorderStyle="None" Height="30px" HorizontalAlign="Center" CssClass="black12" />
                            <Columns>
                                <asp:BoundColumn DataField="SourceUserName" HeaderText="发言人">
                                    <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Content" HeaderText="内容">
                                    <HeaderStyle HorizontalAlign="Center" Width="40%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="是否已读取">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                    <asp:CheckBox ID="cbisRead" runat="server" Enabled="False" OnCheckedChanged="cbisRead_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="DateTime" HeaderText="发言时间">
                                    <HeaderStyle HorizontalAlign="Center" Width="18%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="删除">
                                    <HeaderStyle HorizontalAlign="Center" Width="17%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnDel" Text="删除" style="cursor: hand;" AlertText="确定要删除此条短消息吗？" Height="16px" OnClientClick="return confirm('是否确定删除?');" CommandName="Del" ></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnView" Text="查看" style="cursor: hand;" Height="16px" CommandName="View" ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="ID" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="isRead" Visible="false"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#99CCCC" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                        <ShoveWebUI:ShoveGridPager ID="gPager1" runat="server" Width="100%" PageSize="20"
                            ShowSelectColumn="False" DataGrid="g1" AlternatingRowColor="#F8F8F8" GridColor="#E0E0E0"
                            HotColor="MistyRose" Visible="False" OnPageWillChange="gPager1_PageWillChange"
                            AllowShorting="False" RowCursorStyle="" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>
<asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
</form>
</body>
</html>