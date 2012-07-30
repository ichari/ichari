<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditPassWord.aspx.cs" Inherits="Home_Room_EditPassWord" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_Site.Name %>中修改我的密码-我的资料-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acPwd" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" cellspacing="0" style="border:#E4E4E5 1px solid;" cellpadding="0" style="margin-top: 10px;">
            <tr>
                <td width="40" height="30" align="right" valign="middle" class="red14">
                    <img src="images/user_icon_man.gif" width="19" height="16" />
                </td>
                <td valign="middle" class="red14" style="padding-left: 10px;">
                    我的资料
                </td>
            </tr>
        </table>
            <table width="100%" border="0" cellspacing="0" class="bg1x bgp5" cellpadding="0" style="margin-top: 10px;">
            <tr>
           
                <td width="100" align="center" background="images/admin_qh_100_1.jpg" class="bg1x bgp1 afff fs14">
                    <a href="EditPassword.aspx"><strong>修改密码</strong></a>
                </td>
                <td width="20" height="32">
                    &nbsp;
                </td>
                <td width="6">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
            <table width="750" border="0" cellpadding="0" cellspacing="0" bgcolor="#C0DBF9">
                <tr>
                    <td width="100%" height="30" align="center" bgcolor="#FFFFFF" class="black12" style="padding: 10px">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr bgcolor="#C0DBF9">
                                <td width="20%" height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    用户名：<span class="red12"></span>
                                </td>
                                <td width="80%" align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                                    <asp:Label ID="tbName" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    原投注密码：
                                </td>
                                <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                                    <label>
                                        <asp:TextBox ID="tbOldPassWord" runat="server" CssClass="in_text_hui" Width="140px"
                                            TextMode="Password"></asp:TextBox>
                                    </label>
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    新投注密码：
                                </td>
                                <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                                    <asp:TextBox ID="tbNewPassWord" runat="server" CssClass="in_text_hui" TextMode="Password"
                                        Width="140px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    确认新投注密码：
                                </td>
                                <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                                    <asp:TextBox ID="tbRePassWord" runat="server" TextMode="Password" CssClass="in_text_hui"
                                        Width="140px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="20" colspan="2" align="right" bgcolor="#FFFFFF" class="black12">
                                    <div id="hr">
                                    </div>
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    &nbsp;
                                </td>
                                <td align="left" bgcolor="#FFFFFF" class="red12" style="padding-left: 10px;">
                                    （为了您的账户安全，请您输入以下信息进行确认）
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    安全保护问题：
                                </td>
                                <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                                    <asp:Label ID="lbQuestion" runat="server"></asp:Label>
                                    &nbsp;&nbsp;<span class="blue12"><a href="SafeSet.aspx?FromUrl=EditPassWord.aspx">
                                        <asp:Label ID="lbQuestionInfo" runat="server"></asp:Label>
                                    </a><span class="blue12">
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    答案：
                                </td>
                                <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                                    <asp:TextBox ID="tbMyA" runat="server"></asp:TextBox>
                                    <span class="red12">*</span>
                                </td>
                            </tr>
                            <tr bgcolor="#C0DBF9">
                                <td height="20" colspan="2" align="right" bgcolor="#FFFFFF" class="black12">
                                    <div id="hr">
                                    </div>
                                </td>
                            </tr>
                            <%-- <tr bgcolor="#C0DBF9">
                       <td height="30" align="right" bgcolor="#FFFFFF" class="black12">&nbsp;</td>
                       <td align="left" bgcolor="#FFFFFF" class="red12" style="padding-left:10px;">为了您的账户安全，请您输入以下信息进行确认：</td>
                     </tr>
                     <tr bgcolor="#C0DBF9">
                       <td height="30" align="right" bgcolor="#FFFFFF" class="black12">安全保护问题：</td>
                       <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left:10px;"><select name="select3" id="select3">
                         <option>我爸爸妈妈的名字各是什么？</option>
                         <option>我爸爸的出生地在哪里？</option>
                         <option>我妈妈的出生地在哪里？</option>
                       </select> 
                         <span class="red12">*</span></td>
                     </tr>
                     <tr bgcolor="#C0DBF9">
                       <td height="30" align="right" bgcolor="#FFFFFF" class="black12">问题答案：</td>
                       <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left:10px;">
                         <input name="textfield6" type="text" id="textfield8" size="20" />
                         <span class="red12">*</span></td>
                     </tr>
                     <tr bgcolor="#C0DBF9">
                       <td height="20" colspan="2" align="right" bgcolor="#FFFFFF" class="black12"><div id="hr"></div></td>
                     </tr>--%>
                            <tr bgcolor="#C0DBF9">
                                <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                                    &nbsp;
                                </td>
                                <td align="left" bgcolor="#FFFFFF" class="black12" style="padding: 10px;">
                                    <label>
                                        <ShoveWebUI:ShoveConfirmButton ID="btnOK" runat="server" Text="确定修改" AlertText="确信输入的资料无误，并立即保存资料吗？"
                                            OnClick="btnOK_Click" Style="cursor: pointer;" />
                                        <input type="reset" name="button2" id="button2" value="重填" />
                                    </label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="798" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
            <tr>
                <td width="798" bgcolor="#FFFEDF" class="blue14" style="padding: 5px 10px 5px 10px;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <span class="blue14" style="padding: 5px 10px 5px 10px;">如有其他问题，请联系网站客服：<span class="red14"><%= _Site.ServiceTelephone %>
                                </span></span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </div>
</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>
<asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
</form>
</body>
</html>