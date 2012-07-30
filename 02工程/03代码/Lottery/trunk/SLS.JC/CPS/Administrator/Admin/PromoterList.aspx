<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromoterList.aspx.cs" Inherits="CPS_Administrator_Admin_PromoterList" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="770" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr class="table">
                                    <td height="33" background="../../images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="hui">
                                                    推广首页&nbsp;&gt;推广员管理&nbsp;&gt;&nbsp;推广员列表
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px;">
                            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="30" colspan="2" align="center" bgcolor="#EAF2FE">
                                        <strong class="blue12">推广员列表</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                        
                                        <asp:DataGrid ID="g" runat="server" Width="100%" BorderStyle="None" AllowSorting="True"
                                            AllowPaging="True" PageSize="16" AutoGenerateColumns="False" CellPadding="2"
                                            BackColor="#CCCCCC" Font-Names="宋体"  CellSpacing="1"
                                            GridLines="None" >
                                            <ItemStyle HorizontalAlign="Center"  Height="28px" CssClass="hui"/>
                                            <HeaderStyle HorizontalAlign="Center" BackColor="#ECECEC" Height="30px"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="序号">
                                                <HeaderStyle Width="9%" />
                                                    <ItemStyle Width="9%"/>
                                                    <ItemTemplate>
                                                        <%# Container.ItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               <asp:HyperLinkColumn ItemStyle-Width="15%" HeaderStyle-Width="15%" DataNavigateUrlField="CpsID" DataTextField="Name" HeaderText="商家网站名称" DataNavigateUrlFormatString="PromoterInfo.aspx?id={0}" SortExpression="Name"></asp:HyperLinkColumn>
                                                <asp:BoundColumn DataField="Datetime" HeaderText="开通时间" DataFormatString="{0:yy-MM-dd}">
                                                    <HeaderStyle Width="12%" />
                                                    <ItemStyle Width="12%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="LastMonthMoney" HeaderText="上个月交易量" DataFormatString="{0:N}">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%"/>
                                                </asp:BoundColumn>
                                                 <asp:BoundColumn DataField="MonthMoney" HeaderText="当月交易量" DataFormatString="{0:N}">
                                                    <HeaderStyle Width="13%" />
                                                    <ItemStyle Width="13%"/>
                                                    </asp:BoundColumn>
                                                <asp:BoundColumn DataField="SumMoney" HeaderText="交易总额" DataFormatString="{0:N}">
                                                    <HeaderStyle Width="13%" />
                                                    <ItemStyle Width="13%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="BonusMoney" HeaderText="累计分润金额" DataFormatString="{0:N}">
                                                    <HeaderStyle Width="13%" />
                                                    <ItemStyle Width="13%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                
                                            </Columns>
                                            <PagerStyle Visible="False"></PagerStyle>
                                        </asp:DataGrid>
                                        <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="16"
                    ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
                    HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore" SelectRowColor="#FF9933" />
                                       
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
