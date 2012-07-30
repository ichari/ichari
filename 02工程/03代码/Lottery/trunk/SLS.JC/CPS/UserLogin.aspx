<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserLogin.aspx.cs" Inherits="CPS_UserLogin" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="NoLogin" runat="server">
        <table width="249" border="0" cellspacing="1" cellpadding="0" bgcolor="#AAB6FF">
            <tr>
                <td height="32" background="images/index-1_28.gif" bgcolor="#FFFFFF" class="blue12"
                    style="padding-left: 10px;">
                    <strong>推广员登录</strong>
                </td>
            </tr>
            <tr>
                <td height="108" align="center" valign="top" background="images/index-1_32.gif" bgcolor="#FFFFFF"
                    style="padding-top: 20px;">
                    <table width="90%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="28%" align="right" class="hui">
                                用户名：
                            </td>
                            <td width="72%" height="30" align="left">
                                <asp:TextBox ID="tbUserName" CssClass="table" runat="server" autocomplete="off"  Width="150"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="hui">
                                密&nbsp;&nbsp;&nbsp;码：
                            </td>
                            <td height="30" align="left">
                                <asp:TextBox ID="tbPwd" CssClass="table" runat="server" autocomplete="off" TextMode="Password" Width="150"/>
                            </td>
                        </tr>
                        <tr>
                            <td height="43" colspan="2" align="right" class="hui">
                                <table width="88%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <a href="UserRegCPS.aspx" target="_top">
                                                <img src="images/index-1_34.gif" width="76" height="28" border="0" /></a>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="btnOK" runat="server" ImageUrl="images/index-1_36.gif" OnClick="btnOK_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Longining" runat="server" Visible="false">
        <table width="249" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#AAB6FF">
            <tr>
                <td align="center" class="blue12" background="images/index-1_32.gif" bgcolor="#FFFFFF" height="32">
                    <asp:Label ID="lbUserName" runat="server" CssClass="red"></asp:Label>,您好!
                    级别:<asp:Label ID="lbUserType" runat="server" CssClass="red" Text="推广员"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 207px; height: 73px" class="blue12" align="center" background="images/index-1_32.gif" bgcolor="#FFFFFF">
                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="0" style="text-align: center">
                        <tr runat="server" id="trCpsLogin" visible="false">
                            <td>
                                <a target="_blank" href="Admin/">商家管理后台</a>
                            </td>
                            <td>
                                <a target="_blank" href="Admin/Default.aspx?SubPage=AdvermentLink.aspx">
                                    商家资料</a>
                            </td>
                        </tr>
                        <tr style="padding-top: 5px;" id="trSupperManager" runat="server" visible="false">
                            <td >
                                <a target="_blank" href="Administrator/">超级管理后台</a>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr style="padding-top: 5px;" id="trApply" runat="server" visible="false">
                            <td >
                                <a  target="_top" href="UserRegCPS.aspx">申请推广</a>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr style="padding-top: 5px;" id="trCheck" runat="server" visible="false">
                            <td >
                                状态：审核中
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" background="images/index-1_32.gif" bgcolor="#FFFFFF" height="54">
                    <asp:ImageButton ID="imgbtnLogout" runat="server" ImageUrl="images/exit.gif" OnClick="imgbtnLogout_Click" />
                    
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
