<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CPS_Default" %>

<%@ Register Src="UserControls/Head.ascx" TagName="Head" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兼职推广员联盟|兼职工作|联盟推广|广告联盟</title>
    <meta name="description" content="针对拥有用户访问流量的网站，分配一个指定的用户访问域名链接。网站负责推广此广告链接，联盟通过指定的域名链接产生的用户交易量给网站站长支付分润收益。我们称之为“流量合作分润系统”，以下简称“CPS联盟”。"/>
    <meta name="keywords" content="兼职推广员联盟|兼职工作|联盟推广|广告联盟" />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Head ID="Head1" runat="server" />
     <table width="981" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10PX;" align="center">
            <tr>
                <td width="249" valign="top">
                     <iframe id="Login_IFrame" name="Login_IFrame" width="100%" frameborder="0" scrolling="no" src="UserLogin.aspx"
                                onload="try{document.all['Login_IFrame'].style.height=Login_IFrame.document.body.scrollHeight;}catch(e){}">
                            </iframe>
                </td>
                <td width="405" valign="top">
                    <table width="401" border="0" cellspacing="1" cellpadding="0" bgcolor="#AAB6FF" style="margin-left: 6px;">
                        <tr>
                            <td height="32" background="images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                <table width="95%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="21">
                                            <strong>兼职推广联盟简介</strong>
                                        </td>
                                        <td align="right" class="Arial">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="123" align="center" background="images/index-1_32.gif" bgcolor="#FFFFFF"
                                style="padding-top: 5px;">
                                <table width="95%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" class="hui" style="text-indent: 25px;">
                                            针对拥有用户访问流量的网站，分配一个指定的用户访问域名链接。网站负责推广此广告链接，联盟通过指定的域名链接产生的用户交易量给网站站长支付分润收益。我们称之为“流量合作分润系统”，以下简称“CPS联盟”。
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="327" valign="top">
                    <table width="318" border="0" cellspacing="1" cellpadding="0" bgcolor="#AAB6FF" style="margin-left: 6px;">
                        <tr>
                            <td height="32" background="images/index-1_28.gif" style="padding-left: 10px;" class="blue12">
                                <table width="95%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="21">
                                            <strong>新闻公告</strong>
                                        </td>
                                        <td align="right" class="Arial">
                                            <a href="News.aspx">MORE</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="123" align="center" background="images/index-1_32.gif" bgcolor="#FFFFFF"
                                style="padding-top: 5px;" id="tdXWGG" runat="server" valign="top">
                               
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table width="981" border="0" cellspacing="0" cellpadding="0" style="margin-top: 5px;
                        margin-bottom: 5px;">
                        <tr>
                            <td height="19" background="images/index-1_42.gif">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="249" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                        <tr>
                            <td height="30" bgcolor="#ECECEC" class="hui" style="padding-left: 10px;">
                                <strong>推广员佣金排行</strong>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#FFFFFF" style="padding-top: 5px;" height="216" id="tdUsers" runat="server" valign="top">
                                
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding-left: 6px;">
                    <table width="401" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                        <tr>
                            <td height="30" bgcolor="#ECECEC" style="padding-left: 10px;">
                                <strong class="hui">推广指南</strong>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#FFFFFF" style="padding-top: 12px;" id="tdTGZN" runat="server" height="206" valign="top">
                                
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" valign="top">
                    <table width="318" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <table width="318" cellspacing="1" cellpadding="0" bgcolor="#AAB6FF" style="margin-left: 6px;">
                                    <tr>
                                        <td height="32" align="left" background="images/index-1_28.gif" bgcolor="#FFFFFF"
                                            class="blue12" style="padding-left: 10px;">
                                            <strong>联系我们</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="103" align="center" bgcolor="#FFFFFF" background="images/index-1_32.gif"
                                            style="padding-top: 5px;">
                                            <table width="95%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="22%" class="hui">
                                                        联系电话：
                                                    </td>
                                                    <td width="40%" height="20" align="left" class="hui">
                                                        0755-27759151
                                                    </td>
                                                    <td width="38%" rowspan="4" align="center" valign="bottom" class="hui">
                                                        <a href="javascript:;" onclick="try{parent.closeMini();}catch(e){;}this.newWindow = window.open('http://chat10.live800.com/live800/chatClient/chatbox.jsp?companyID=86584&configID=149924&jid=8794095338&enterurl=http%3A%2F%2Flocalhost%3A2003%2FSLS%2EIcaile%2FHome%2FWeb%2FDefault%2Easpx', 'chatbox86584', 'toolbar=0');this.newWindow.focus();this.newWindow.opener=window;return false;">
                                                            <img src="images/contact.gif" width="68" height="63" border="0" /></a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="hui">
                                                        图文传真：
                                                    </td>
                                                    <td height="20" align="left" class="hui">
                                                        0755-27759020
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="hui">
                                                        推广QQ：
                                                    </td>
                                                    <td height="24" align="left" class="hui">
                                                        372585858
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="hui">
                                                        客服邮箱：
                                                    </td>
                                                    <td height="24" align="left" class="hui">
                                                        services@3gcpw.com
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 8px;">
                                <img src="images/banner_2.jpg" width="318" height="101" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    <uc2:Foot ID="Foot1" runat="server" />
   
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
