<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForeCast.aspx.cs" Inherits="ForeCast" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register Src="Home/Room/UserControls/Index_banner.ascx" TagName="Index_banner"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=_Site.Name %>网-资讯标题</title>
    <meta name="description" content="<%=_Site.Name %>为广大彩民提供竞彩足球，竞彩篮球，胜负彩，排列3/5，大乐透等彩票开奖号码预测分析。" />
    <meta name="keywords" content="彩票预测，彩票分析" />
    <link href="Style/dahecp.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 331px;
        }
    </style>
</head>
<link rel="shortcut icon" href="/favicon.ico" />
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <div id="content">
        <table width="1002" border="0" cellspacing="0" cellpadding="0" style="margin-bottom: 10px;">
            <tr>
                <td>
                    <img src="/home/room/images/222.jpg" width="1002" height="66" />
                </td>
            </tr>
        </table>
        <table width="1002" border="0" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
            <tr>
                <td bgcolor="#FFFFFF" style="padding-left:4px; padding-top:4px; padding-right:4px; padding-bottom:4px;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="vertical-align:top; text-align:left;">
                        <tr valign="top">
                            <td>
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 3px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" bgcolor="#FFFFFF" class="blue12" style="background: url(home/room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table border="0" cellspacing="0" cellpadding="0" style="width: 100%">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">
                                                            竞彩足球</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=jczq" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="tdjczq" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 3px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" bgcolor="#FFFFFF" class="blue12" style="background: url(home/room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">
                                                            竞彩篮球</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=jclq" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px;" bgcolor="#FFFFFF" runat="server" id="tdjclq" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" class="style1">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 3px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" align="left" bgcolor="#FFFFFF" class="blue12" style="background: url(Home/Room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">超级大乐透</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=dlt" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="tdCJDLT" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" bgcolor="#FFFFFF" class="blue12" style="background: url(Home/Room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                       <h1 class='blue12' style="display: inline;">排列3/5</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=szpl" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="tdPL" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" valign="bottom">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" align="left" bgcolor="#FFFFFF" class="blue12" style="background: url(Home/Room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">七星彩</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=qxc" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"  height="360px" bgcolor="#FFFFFF" runat="server" id="tdqxc" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" class="style1">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" align="left" bgcolor="#FFFFFF" class="blue12" style="background: url(Home/Room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">22选5</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=22x5" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="td22x5" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr valign="top">
                            <td>
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" bgcolor="#FFFFFF" class="blue12" style="background: url(home/room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">
                                                            足彩资讯</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=ctzc" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="td1" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" bgcolor="#FFFFFF" class="blue12" style="background: url(home/room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">
                                                            欧冠资讯</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=ogzx" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="td2" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" class="style1">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" align="left" bgcolor="#FFFFFF" class="blue12" style="background: url(Home/Room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">英超资讯</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=yczx" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="td3" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr valign="top">
                            <td>
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" bgcolor="#FFFFFF" class="blue12" style="background: url(home/room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">西甲资讯</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=xjzx" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"  height="360px" bgcolor="#FFFFFF" runat="server" id="td4" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" bgcolor="#FFFFFF" class="blue12" style="background: url(home/room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">
                                                            意甲资讯</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=yjsz" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"  height="360px" bgcolor="#FFFFFF" runat="server" id="td5" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" class="style1">
                                <table width="328px" border="1" style="border-style:solid;border-color:#BCD2E9;margin-top: 6px;" cellspacing="1" cellpadding="0" bgcolor="#BCD2E9">
                                    <tr>
                                        <td height="31" align="left" bgcolor="#FFFFFF" class="blue12" style="background: url(Home/Room/images/cpyc.jpg);
                                            padding-left: 10px;">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="83%">
                                                        <h1 class='blue12' style="display: inline;">德甲资讯</h1>
                                                    </td>
                                                    <td width="17%" align="left">
                                                        <a href="/ZX.aspx?NewsName=djsz" target="_blank">更多&gt;&gt;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="360px" bgcolor="#FFFFFF" runat="server" id="td6" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
