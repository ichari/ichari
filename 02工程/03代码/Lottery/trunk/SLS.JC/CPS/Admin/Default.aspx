<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CPS_Admin_Default" %>

<%@ Register Src="../UserControls/Head.ascx" TagName="Head" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兼职推广联盟</title>
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />

    <script src="../../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Head ID="Head1" runat="server" />
    <table width="981" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;" align="center">
        <tr>
            <td width="200" valign="top">
                <table width="200" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="200" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="32" background="../images/index-1_28.gif" bgcolor="#FFFFFF" class="blue12"
                                        style="padding-left: 10px;">
                                        <strong>客户资料</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="70" align="center" background="../images/index-1_32.gif" bgcolor="#FFFFFF">
                                        <table width="90%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td colspan="2">
                                                   <asp:Label ID="lbUserName" runat="server"  CssClass="red"></asp:Label>,您好！
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                   级别：<asp:Label ID="lbUserType" runat="server" CssClass="red" Text="推广员"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <hr style="border: 1px dashed #cccccc; width: 100%" size="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="46%" height="26" align="center">
                                                    <span class="hui"><a href="BaseInfo.aspx" target="contentFrame">基本信息</a></span>
                                                </td>
                                                <td width="54%" height="26" align="center">
                                                    <span class="hui"><a href="../../Home/Room/UserEdit.aspx" target="_blank">个人资料</a></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="26" align="center">
                                                    <span class="hui"><a href="../../Home/Room/EditPassword.aspx" target="_blank">修改密码</a></span>
                                                </td>
                                                <td height="26" align="center">
                                                    <asp:LinkButton ID="lbExit" runat="server"  CssClass="hui" OnClick="lbExit_Click">安全退出</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5PX;">
                            <table width="200" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="32" background="../images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                        <strong><a href="AdvermentLink.aspx" target="contentFrame">广告链接</a></strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5PX;">
                            <table width="200" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="32" background="../images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                        <strong><a href="javascript:" onclick="if(document.getElementById('trSRTJ').style.display=='none'){document.getElementById('trSRTJ').style.display=''}else{document.getElementById('trSRTJ').style.display='none'}">收入统计</a></strong>
                                    </td>
                                </tr>
                                <tr id="trSRTJ" style="display:none">
                                    <td height="16" align="center" bgcolor="#FFFFFF" class="blue12" style="padding-top: 10px;
                                        padding-bottom: 10px;">
                                        <table width="75%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td height="30" class="hui">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td height="24" align="left" class="hui" style="padding-left: 10px;">
                                                                <a href="IncomeList.aspx" target="contentFrame">收入明细</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="30" align="left" class="hui">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td height="24" align="left" class="hui" style="padding-left: 10px;">
                                                                <a href="IncomeListByMonth.aspx" target="contentFrame">月度结算表</a>
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
                     <tr id="trPromoter" runat="server" visible="false">
                        <td style="padding-top: 5PX;">
                            <table width="200" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="32" background="../images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                        <strong><a href="javascript:" onclick="if(document.getElementById('trTGY').style.display=='none'){document.getElementById('trTGY').style.display=''}else{document.getElementById('trTGY').style.display='none'}">推广员管理</a></strong>
                                    </td>
                                </tr>
                                <tr id="trTGY" style="display:none">
                                    <td height="16" align="center" bgcolor="#FFFFFF" class="blue12" style="padding-top: 10px;
                                        padding-bottom: 10px;">
                                        <table width="75%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td height="30" class="hui">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td height="24" align="left" class="hui" style="padding-left: 10px;">
                                                                <a href="PromoterList.aspx" target="contentFrame">推广员列表</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="30" align="left" class="hui">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td height="24" align="left" class="hui" style="padding-left: 10px;">
                                                                <a href="AddPromoter.aspx" target="contentFrame">增加推广员</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td height="30" align="left" class="hui">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td height="24" align="left" class="hui" style="padding-left: 10px;">
                                                                <a href="LinkList.aspx" target="contentFrame">PID推广链接列表</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td height="30" align="left" class="hui">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td height="24" align="left" class="hui" style="padding-left: 10px;">
                                                                <a href="LinkBonusScale.aspx" target="contentFrame">PID推广链接佣金设置</a>
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
                        <td style="padding-top: 5PX;">
                            <table width="200" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="32" background="../images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                        <strong><a href="MemberList.aspx" target="contentFrame">会员管理</a></strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5PX;">
                            <table width="200" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="32" background="../images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                        <strong><a href="../../Home/Room/Distill.aspx?IsCps=1" target="_blank">提款管理</a></strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5PX;">
                            <table width="200" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="32" background="../images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                        <strong><a href="http://www.3gcpw.com/Club/showforum-14.aspx" target="_blank">在线提问</a></strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <iframe id="contentFrame" src="<%=SubPage %>"  frameborder="no" width="100%" onload="try{document.all['contentFrame'].style.height=contentFrame.document.body.scrollHeight;}catch(e){}" name="contentFrame"
                        border="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="yes">
                    </iframe>
            </td>
        </tr>
    </table>
    <uc2:Foot ID="Foot1" runat="server" />
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
