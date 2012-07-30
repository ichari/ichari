<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="CPS_Login" %>
<%@ Register Src="UserControls/Head.ascx" TagName="Head" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兼职推广联盟</title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Head ID="Head1" runat="server" />
    <table width="981" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;" align="center">
            <tr>
                <td width="220" valign="top">
                    <table width="190" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                        <tr>
                            <td height="32" background="images/index-1_28.gif" bgcolor="#FFFFFF" class="blue12"
                                style="padding-left: 10px;">
                                <strong>申请推广</strong>
                            </td>
                        </tr>
                        <tr>
                            <td height="70" background="images/index-1_32.gif" bgcolor="#FFFFFF">
                                <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="10%" align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td width="90%" height="26" class="hui">
                                        <a href="UserRegCps.aspx">注册推广员</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="UserRegCps.aspx?type=2">注册代理商</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="Login.aspx">推广员登录</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="../Home/Web/UserRegAgree.aspx?type=1" target="_blank">注册协议</a>
                                    </td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td rowspan="2"  align="left" valign="top">
                    <table width="773" border="0" align="left" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC"
                        style="margin-left: 8px;">
                        <tr>
                            <td height="33" background="images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="hui">
                                            推广首页&nbsp;&gt;&nbsp;申请推广&nbsp;&gt;&nbsp;推广员登录
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="193" align="center" bgcolor="#FFFFFF" style="padding-top: 20px; padding-bottom: 8px;
                                background: url(images/index-1_32.gif); background-repeat: repeat-x; background-color: #FFF">
                                <table width="90%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="50" align="center">
                                            <img src="images/user_title_1.gif" width="327" height="33" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="1" bgcolor="#CCCCCC" align="center">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="135" align="center" style="padding-top: 10px; padding-bottom: 10px;">
                                            <table width="95%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding-top: 20px; padding-bottom: 20px;">
                                                        <table width="60%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="24%" height="24" align="right" class="hui">
                                                                    用户名：
                                                                </td>
                                                                <td width="76%" height="30" align="left">
                                                                    <asp:TextBox ID="tbUserName"  runat="server" autocomplete="off"  size="25"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="24" align="right" class="hui">
                                                                    密码：
                                                                </td>
                                                                <td height="30" align="left">
                                                                    <asp:TextBox ID="tbPwd" runat="server" autocomplete="off" TextMode="Password" Width="180"/>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trCheckCode">
                                                                <td height="24" align="right" class="hui">
                                                                    验证码：
                                                                </td>
                                                                <td height="30" align="left">
                                                                    <asp:TextBox ID="tbCheckCode"  runat="server" size="5" autocomplete="off" />
                                                                    <ShoveWebUI:ShoveCheckCode ID="CheckCode" runat="server" BackColor="SeaShell" Charset="All"
                                                                     ForeColor="CornflowerBlue" Height="20px" Width="64px" SupportDir="../ShoveWebUI_client" />
                                                                    <a href="Login.aspx">刷新</a>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="60" colspan="2" align="center">
                                                                    <table width="60%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="39%">
                                                                                <a href="UserRegCPS.aspx">
                                                                                    <img src="images/bt_zhuce.jpg" width="83" height="29" border="0" /></a>
                                                                            </td>
                                                                            <td width="61%" align="center">
                                                                                <asp:ImageButton ID="btnOK" runat="server" ImageUrl="images/bt_denglu.jpg" OnClick="btnOK_Click" />

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
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    &nbsp;
                </td>
            </tr>
        </table>
     <uc2:Foot ID="Foot1" runat="server" />
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
