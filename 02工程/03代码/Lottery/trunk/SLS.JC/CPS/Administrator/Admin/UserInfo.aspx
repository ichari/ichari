<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="CPS_Administrator_Admin_UserInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image:url(about:blank);">
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
                                                    推广首页&nbsp;&gt;会员管理&nbsp;&gt;&nbsp;用户信息
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
                                        <strong class="blue12">用户信息</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            真实姓名：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                            <asp:TextBox ID="tbRealityName" CssClass="in_text_hui" runat="server" autocomplete="off"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            联系电话：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                            <asp:TextBox ID="tbPhone" CssClass="in_text_hui" runat="server" autocomplete="off"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            手机号码：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                            <asp:TextBox ID="tbMobile" CssClass="in_text_hui" runat="server" autocomplete="off"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            QQ 号码：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                            <asp:TextBox ID="tbQQNum" CssClass="in_text_hui" runat="server" autocomplete="off"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            Email：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                            <asp:TextBox ID="tbEmail" CssClass="in_text_hui" runat="server" autocomplete="off"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            联系地址：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                            <asp:TextBox ID="tbAddress" CssClass="in_text_hui" runat="server" autocomplete="off"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                                       
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
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
