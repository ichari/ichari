<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LinkBonusScale.aspx.cs" Inherits="CPS_Admin_LinkBonusScale" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />
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
                                                    推广首页&nbsp;&gt;会员管理&nbsp;&gt;&nbsp;PID推广链接佣金设置
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
                                        <strong class="blue12">PID推广链接佣金设置</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="75" align="right">
                                                    链接PID：
                                                </td>
                                                <td width="120">
                                                    <asp:TextBox runat="server" ID="tbPID" CssClass="hui_cc" />
                                                </td>
                                                <td width="229">
                                                    <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="g" runat="server" Width="100%" BorderStyle="None" AllowSorting="True"
                                            AllowPaging="True" PageSize="16" AutoGenerateColumns="False" CellPadding="2"
                                            BackColor="#CCCCCC" Font-Names="宋体" CellSpacing="1" GridLines="None" DataKeyField="ID">
                                            <ItemStyle HorizontalAlign="Center" Height="28px" />
                                            <HeaderStyle HorizontalAlign="Center" BackColor="#ECECEC" Height="30px"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="SiteLinkPID" HeaderText="链接PID">
                                                    <HeaderStyle Width="19%" />
                                                    <ItemStyle Width="19%" CssClass="hui" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="分润比例">
                                                    <ItemStyle CssClass="hui"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tbBonus" runat="server" onkeyup="value=value.replace(/[^\d\.]/g,'');"
                                                            Text='<%#DataBinder.Eval(Container.DataItem,"BonusScale")%>' Width="50" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle Visible="False"></PagerStyle>
                                        </asp:DataGrid>
                                        <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="16"
                                            ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
                                            HotColor="MistyRose" SelectRowColor="#FF9933" />
                                           <div  style="text-align:center"><asp:Button ID="btnSave" Text="保 存"  runat="server" onclick="btnSave_Click" /></div> 
                                    </td>
                                    
                                </tr>
                                 <tr>
                                    <td colspan="2" align="left" style="padding-left:10px; padding-bottom:10px; padding-top:10px" bgcolor="#EAF2FE">
                                        PID为代理商家系统的站长标识ID (可以为数字或字符串).通过http://www.3gcpw.Com/Default.aspx?Cpsid=1&PID=12345链接注册的会员由联盟指定的站长12345推广的会员,12345就是代理商分配给哪个站长的标记,目的是用这个标记来区别哪些员会是属于哪个站长推广的.
                                        PID的值由商家自由分配,<%=_Site.Name %>系统会自动记录用此PID推广注册的会员
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
