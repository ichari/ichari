<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserEmailBind.aspx.cs" Inherits="Home_Room_UserEmailBind" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>绑定Email-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。"/>
    <meta name="keywords" content="绑定Email" />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="/favicon.ico" />
    </head>
<body class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acEmail" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" style="border:#E4E4E5 1px solid;" cellspacing="0" cellpadding="0">
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
                    <td width="100" align="center" class="afff fs14 " style="background-image:url('images/admin_qh_100_1.jpg');">
                        <a href="UserEdit.aspx"><strong>激活电子邮箱</strong></a>
                    </td>
                    <td height="32">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:10px;">
                <tr>
                    <td style="width:20%;height:30px;text-align:right;" class="black12">邮箱激活：</td>
                    <td style="padding-left: 10px;"><div class="blue12">密码找回、身份验证时需要</div></td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">用户名：</td>
                    <td style="padding-left: 10px;">
                        <div>
                            <asp:Label ID="labName" runat="server" CssClass="black12"></asp:Label>
                            <span class="red">
                                （<asp:Label ID="labUserType" runat="server"></asp:Label>
                                等级 <asp:Label ID="labLevel" runat="server"></asp:Label>）
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" class="black12">
                        真实姓名：
                    </td>
                    <td style="padding-left:10px;">
                        <asp:Label ID="tbRealityName" runat="server" Text="Label" CssClass="black12"></asp:Label>
                    </td>
                </tr>
                <%--
                <tr>
                    <td width="18%" height="30" align="right" bgcolor="f7f7f7" class="black12">
                        安全问题：
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 15px;">
                        <asp:Label ID="lblQuestion" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="18%" height="30" align="right" bgcolor="f7f7f7" class="black12">
                        问题答案：
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 15px;">
                        <asp:TextBox runat="server" ID="tbAnswer"/>
                    </td>
                </tr>
                --%>
                <tr>
                    <td height="30" align="right" class="black12">
                        Email：
                    </td>
                    <td class="black12" style="padding-left: 10px;">
                        <label>
                            <asp:TextBox ID="tbEmail" runat="server" />
                        </label>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" class="black12">
                        状态：
                    </td>
                    <td class="black12" style="padding-left: 10px;">
                        <asp:Label ID="labIsEmailVailded" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td height="30"></td>
                    <td style="padding-left: 10px;">
                        <asp:Button ID="btnBind" runat="server" CssClass="btn_s" Text="申请激活" OnClick="btnBind_Click" />
                        <asp:Button ID="btnReBind" runat="server" CssClass="btn_s" Text="重新激活" OnClick="btnBind_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;"></td>
                    <td style="padding-left: 10px;">
                        <asp:Label ID="Label1" runat="server" style="color:Red;"></asp:Label>
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
