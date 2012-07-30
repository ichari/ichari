<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserEdit.aspx.cs" Inherits="Home_Room_UserEdit" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改用户资料-<%=_Site.Name %></title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。"/>
    <meta name="keywords" content="修改用户资料" />
    <link rel="stylesheet" type="text/css" href="Style/css.css" />
    <link rel="Shortcut Icon" href="/favicon.ico" />
    <style type="text/css">
        .style1
        {
            font-size: 12px;
            color: #000000;
            font-family: tahoma;
            line-height: 18px;
            height: 20px;
        }
    </style>
    <script type="text/javascript" src="/Components/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" language="javascript">
        function showSameHeight() {
            if (document.getElementById("menu_left").clientHeight < document.getElementById("menu_right").clientHeight) {
                document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
            }
            else {
                if (document.getElementById("menu_right").offsetHeight >= 960) {
                    document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
                }
                else {
                    document.getElementById("menu_left").style.height = "960px";
                }
            }
        }
    </script>
</head>
<body onload="showSameHeight();" class="gybg">
<form id="form1" runat="server">
<input id="hdIDCardNumber" runat="server" type="hidden" />
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acInfo" />
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
                    <td class="afff fs14" style="width:100px;height:32px;text-align:center;background-image:url('images/admin_qh_100_1.jpg');">
                        <a href="UserEdit.aspx"><strong>个人基本资料</strong></a>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:10px;">
                <tr>
                    <td style="width:20%;height:30px;text-align:right;" class="black12">
                        真实姓名：<span class="red12"></span>
                    </td>
                    <td style="width:80%;padding-left: 10px;" class="black12">
                        <asp:TextBox ID="tbRealityName" runat="server" Width="160px"></asp:TextBox>
                        <asp:Label ID="lbIsRealityNameValided" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;">
                        &nbsp;
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <span class="red12">非常重要，您提款的重要依据，提款时银行卡的户名必须是这里填写的真实姓名，一旦提交将不可更改！</span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        身份证号码：<span class="red12"></span>
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:TextBox ID="tbIDCardNumber" Text="" runat="server" Width="160px"></asp:TextBox>
                        <asp:Label ID="lbIsIdCardNumberValided" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;">
                        &nbsp;
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <span class="red12">填写成功后用户不能自行修改，如需修改，请联系网站客服。</span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        用户名：<span class="red12"></span>
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:Label ID="lbUserName" runat="server" Text=""></asp:Label>
                        <%--<asp:TextBox ID="txtName" runat="server" Width="160px"></asp:TextBox>
                        &nbsp;&nbsp;<span class="blue12"><a style="cursor: hand;" onclick="return checkUserName();">检测用户名</a></span>
                        <span id="spCheckResult"></span>--%>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        性别：<span class="red12"></span>
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:RadioButton ID="rbSexM" runat="server" GroupName="rbSex" Text="男" />
                        <asp:RadioButton ID="rbSexW" runat="server" GroupName="rbSex" Text="女" />
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        手机号码：
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:TextBox ID="tbMobile" runat="server" Width="160" />
                        <%-- <asp:Label ID="lbMobile" runat="server" Text="" Width="160px"></asp:Label>--%>
                        <%--<asp:Label ID="labIsMobileVailded" runat="server"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        E-mail：
                    </td>
                    <td style="padding-left:10px;">
                        <asp:TextBox ID="tbEmail" runat="server" CssClass="in_text_hui black12" Width="160px" MaxLength="50"></asp:TextBox>
                        <asp:Label ID="lbIsEmailValided" runat="server"></asp:Label>&nbsp;&nbsp;
                        <a href="UserEmailBind.aspx" class="blue12">申请激活或修改激活</a>
                        <%--<input name="textfield" type="text" id="textfield3" value="test001@163.com" size="20" />--%>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        城市：
                    </td>
                    <td style="padding-left:10px;">
                        <ShoveWebUI:ShoveProvinceCityInput ID="ddlCity" runat="server" SupportDir="~/ShoveWebUI_client" />
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        出生日期：
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:TextBox ID="tbBirthday" CssClass="in_text_hui" runat="server" MaxLength="10" onFocus="WdatePicker({el:'tbBirthday',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})"></asp:TextBox>
                        <span class="black12">如：1990-9-1</span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        联系地址：
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:TextBox ID="tbAddress" CssClass="in_text_hui" Width="260px" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <!--
                <tr bgcolor="#C0DBF9">
                    <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                        邮政编码：
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <input name="textfield5" type="text" id="textfield6" size="20" />
                    </td>
                </tr>
                -->
                <tr>
                    <td style="height:30px;text-align:right;">
                        &nbsp;
                    </td>
                    <td style="padding-left:10px;" class="red12">
                        （为了您的账户安全，请确实设置安全问题）
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" bgcolor="#FFFFFF" class="black12">
                        安全保护问题：
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <asp:Label ID="lbQuestion" runat="server"></asp:Label>&nbsp;&nbsp;
                        <span class="blue12">
                            <a href="SafeSet.aspx?FromUrl='UserEdit.aspx'">设置安全问题</a>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">答案：</td>
                    <td style="padding-left: 10px;"><asp:Label ID="lbSecAns" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="height:30px;">
                        <div class="hr_bar"></div>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;">
                        &nbsp;
                    </td>
                    <td style="padding-left:10px;" class="red12">
                        （为了您的账户安全，请您输入正确账户密码进行确认）
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        账户密码：
                    </td>
                    <td style="padding-left:10px;">
                        <asp:TextBox ID="tbVerPwd" runat="server" CssClass="in_text_hui" Width="160" MaxLength="20" TextMode="Password" />
                        <asp:Label ID="lbErrPwd" runat="server" CssClass="red12" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height:30px;">
                        <div class="hr_bar"></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="" style="padding: 10px;">
                        <ShoveWebUI:ShoveConfirmButton ID="btnOK" runat="server" Text="确定修改" Style="cursor: pointer;"
                            AlertText="确信输入的资料无误，并立即保存资料吗？" OnClick="btnOK_Click" CssClass="btn_s" />
                        <a href="UserEdit.aspx" class="btn_s" style="vertical-align:middle;">取消</a>
                    </td>
                </tr>
            </table>
            <div style="padding:5px 10px 5px 10px;background-color:#FFFEDF;border:1px solid #D8D8D8;">
                <span class="blue14" style="padding: 5px 10px 5px 10px;">如有其他问题，请联系网站客服：
                    <span class="red14"><%= _Site.ServiceTelephone %></span>
                </span>
            </div>
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
<script type="text/javascript" language="javascript">
 // ClickLottery(document.getElementById('tdLottery5'),0);
  //ClickTC(document.getElementById('tdTC1'),0);
</script>
