<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageCode.aspx.cs" Inherits="CPS_ImageCode" %>

<%@ Register Src="UserControls/Head.ascx" TagName="Head" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兼职推广联盟</title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Head ID="Head1" runat="server" />
    <table width="981" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;"
        align="center">
        <tr>
            <td width="220" valign="top">
                <table width="220" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                    <tr>
                        <td height="32" background="images/index-1_28.gif" bgcolor="#FFFFFF" class="blue12"
                            style="padding-left: 10px;">
                            <strong>广告样式</strong>
                        </td>
                    </tr>
                    <tr>
                        <td height="70" background="images/index-1_32.gif" bgcolor="#FFFFFF" style="padding: 5px;">
                            <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="10%" align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td width="90%" height="26" class="hui">
                                        <a href="ImageCode.aspx">图片广告样式</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="WordCode.aspx">文字标题广告样式</a>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="CustomCode.aspx">定制广告商务合作</a>
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td rowspan="2" valign="top">
                <table width="754" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                    <tr>
                        <td height="33" background="images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                            <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td class="hui">
                                        推广首页&nbsp;&gt;&nbsp;广告链接&nbsp;&gt;&nbsp;图文广告样式
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" bgcolor="#FFFFFF" style="padding-top: 8px; padding-bottom: 8px;">
                            <table width="96%" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="30" colspan="3" align="center" bgcolor="#EAF2FE">
                                        <strong class="blue12">广告样式一</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center" bgcolor="#FFFFFF" style="padding-bottom: 5px; padding-top: 5px;">
                                        <img src="images/ad_tg_2.jpg" width="619" height="60" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="19%" height="30" align="center" bgcolor="#FCF5E1" class="hui">
                                        广告代码
                                    </td>
                                    <td width="66%" height="30" bgcolor="#FFFFFF" style="padding: 5px;">
                                         <asp:TextBox ID="tbImageCode1" TextMode="MultiLine" runat="server" ReadOnly="true"
                                Rows="5" Columns="55"></asp:TextBox>
                                    </td>
                                    <td width="15%" bgcolor="#FFFFFF">
                                        <input  type="button" onclick="doufucopy('tbImageCode1')" value="复制代码" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="center" bgcolor="#FCF5E1">
                                        <span class="hui">广告形式</span>
                                    </td>
                                    <td height="30" colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding-left: 10px;">
                                        图片广告
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="center" bgcolor="#FCF5E1" class="hui" style="padding-left: 10px;">
                                        图片大小
                                    </td>
                                    <td height="30" colspan="2" align="left" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        619*60
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="center" bgcolor="#FCF5E1" class="hui">
                                        使用方法
                                    </td>
                                    <td height="30" colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding-left: 10px;">
                                        复制广告代码，粘贴在你要宣传的位置。
                                    </td>
                                </tr>
                            </table>
                            <table width="96%" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC" style="margin-top: 10px;">
                                <tr>
                                    <td height="30" colspan="3" align="center" bgcolor="#EAF2FE">
                                        <strong class="blue12">广告样式二</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center" bgcolor="#FFFFFF" style="padding-bottom: 5px; padding-top: 5px;">
                                        <img src="images/ad_tg_2.jpg" width="619" height="60" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="19%" height="30" align="center" bgcolor="#FCF5E1" class="hui">
                                        广告代码
                                    </td>
                                    <td width="66%" height="30" bgcolor="#FFFFFF" style="padding: 5px;">
                                        <asp:TextBox ID="tbImageCode2" TextMode="MultiLine" runat="server" ReadOnly="true"
                                Rows="5" Columns="55"></asp:TextBox>
                                    </td>
                                    <td width="15%" bgcolor="#FFFFFF">
                                        <input  type="button" onclick="doufucopy('tbImageCode2')" value="复制代码" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="center" bgcolor="#FCF5E1">
                                        <span class="hui">广告形式</span>
                                    </td>
                                    <td height="30" colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding-left: 10px;">
                                        图片广告
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="center" bgcolor="#FCF5E1" class="hui">
                                        图片大小
                                    </td>
                                    <td height="30" colspan="2" align="left" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        619*60
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="center" bgcolor="#FCF5E1" class="hui">
                                        使用方法
                                    </td>
                                    <td height="30" colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding-left: 10px;">
                                        复制广告代码，粘贴在你要宣传的位置。
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" style="padding-top: 5px;">
                &nbsp;
            </td>
        </tr>
    </table>
    <uc2:Foot ID="Foot1" runat="server" />
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
<script type="text/javascript">
    function doufucopy(text) {
        text = document.getElementById(text);
        textRange = text.createTextRange();
        textRange.execCommand("Copy");
        alert("已复制到剪粘板上");

        return false;
    }
</script>