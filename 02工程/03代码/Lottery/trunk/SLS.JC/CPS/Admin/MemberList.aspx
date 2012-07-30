<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MemberList.aspx.cs" Inherits="CPS_Admin_MemberList" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />

    <script src="../../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

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
                                    <td height="33" background="../images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="hui">
                                                    推广首页&nbsp;&gt;会员管理&nbsp;&gt;&nbsp;会员列表
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
                                        <strong class="blue12">会员列表</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="104" height="30" align="right" class="hui_2">
                                                    按消费日期查询：
                                                </td>
                                                <td width="75" align="right">
                                                    开始日期：
                                                </td>
                                                <td width="120">
                                                    <asp:TextBox runat="server" ID="tbBeginTime" CssClass="hui_cc" onblur="if(this.value=='') this.value=document.getElementById('hBeginTime').value"
                                                        name="tbBeginTime" onFocus="WdatePicker({el:'tbBeginTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                                </td>
                                                <td width="83" align="right">
                                                    截止时间：
                                                </td>
                                                <td width="137">
                                                    <asp:TextBox runat="server" ID="tbEndTime" CssClass="hui_cc" onblur="if(this.value=='') this.value=document.getElementById('hBeginTime').value"
                                                        name="tbEndTime" onFocus="WdatePicker({el:'tbEndTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                                </td>
                                                <td width="229">
                                                    <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="g" runat="server" Width="100%" BorderStyle="None" AllowSorting="True"
                                            AllowPaging="True" PageSize="16" AutoGenerateColumns="False" CellPadding="2"
                                            BackColor="#CCCCCC" Font-Names="宋体"  CellSpacing="1"
                                            GridLines="None" OnItemCommand="g_ItemCommand">
                                            <ItemStyle HorizontalAlign="Center"  Height="28px"/>
                                            <HeaderStyle HorizontalAlign="Center" BackColor="#ECECEC" Height="30px"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="序号">
                                                <HeaderStyle Width="9%" />
                                                    <ItemStyle Width="9%" CssClass="black12" />
                                                    <ItemTemplate>
                                                        <%# Container.ItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Name" HeaderText="会员ID">
                                                <HeaderStyle Width="15%" />
                                                     <ItemStyle Width="15%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="RegisterTime" HeaderText="注册时间" DataFormatString="{0:yy-MM-dd}">
                                                    <HeaderStyle Width="12%" />
                                                    <ItemStyle Width="12%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="RealityName" HeaderText="真实姓名">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Width="10%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="联系方式">
                                                    <HeaderStyle Width="13%" />
                                                    <ItemStyle Width="13%" CssClass="red" />
                                                    <ItemTemplate>
                                                        <shovewebui:shovelinkbutton id="btnLook" runat="server" commandname="Look">查看</shovewebui:shovelinkbutton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="TradeMoney" HeaderText="消费总额" DataFormatString="{0:N}">
                                                    <HeaderStyle Width="13%" />
                                                    <ItemStyle Width="13%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="BonusMoney" HeaderText="分润总额" DataFormatString="{0:N}">
                                                    <HeaderStyle Width="13%" />
                                                    <ItemStyle Width="13%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="查看">
                                                    <HeaderStyle Width="15%" />
                                                   <ItemStyle Width="15%" CssClass="red" />
                                                    <ItemTemplate>
                                                        <shovewebui:shovelinkbutton id="btnDetaile" runat="server" commandname="BuyDetail">查看</shovewebui:shovelinkbutton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                                
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
    <asp:HiddenField ID="hBeginTime" runat="server" />
    <asp:HiddenField ID="hEndTime" runat="server" />
    </form>
</body>
</html>
