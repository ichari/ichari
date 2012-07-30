<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileReg.aspx.cs" Inherits="Home_Room_MobileReg" %>
<%@ Register src="UserControls/UserMyIcaile.ascx" tagname="UserMyIcaile" tagprefix="uc2" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我的手机号码验证-用户资料-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
     
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
<link rel="shortcut icon" href="../../favicon.ico" /></head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">

    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" />
        </div>
        <div id="menu_right">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td valign="top" bgcolor="#FFFFFF">

                <table width="100%" border="0" class="bg1x bgp5"  bgcolor="#E4E4E5" cellpadding="1" cellspacing="0" background="images/bg_blue_30.jpg">
                        <tr>
                            <td height="32" width="120" align="center" class="bg1x bgp1 afff fs14" >
                            手机号码验证<input id="Hidden1" runat="server" name="tbLotteryID" style="width: 50px"
                                                type="hidden" /><input id="Hidden2" runat="server" name="tbPlayTypeID" style="width: 50px"
                                                    type="hidden" /><input id="Hidden3" runat="server" name="tbBuyFileName" style="width: 50px"
                                                        type="hidden" />
                            </td>
                            <td>                            
                            </td>
                        </tr>
                    </table>
 

                                <table width="100%" border="0" align="center" cellpadding="1" bgcolor="#E4E4E5">
                                    <tr style="background-color: White;">
                                        <td class="td6" valign="middle" style="width: 184px" align="right">
                                            <asp:Label ID="Label2" runat="server">　用户名：</asp:Label>&nbsp;
                                        </td>
                                        <td height="34">
                                            &nbsp;<asp:TextBox ID="tbUserName" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="background-color: White;">
                                        <td style="width: 184px" align="right">
                                            &nbsp;<asp:Label ID="Label10" runat="server">手机号码：</asp:Label>&nbsp;
                                        </td>
                                        <td height="34">
                                            &nbsp;<asp:TextBox ID="tbUserMobile" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                            <ShoveWebUI:ShoveConfirmButton ID="btnMobileValid" runat="server" Text="立即验证" AlertText="确信要立即验证您的手机吗？" OnClick="btnMobileValid_Click" />
                                        </td>
                                    </tr>
                                    <asp:Panel ID="panelValid" runat="server" Visible="false">
                                        <tr style="background-color: White;">
                                            <td style="height: 25px; width: 184px;" align="right">
                                                <asp:Label ID="Label15" runat="server">验证密码：</asp:Label>&nbsp;
                                            </td>
                                            <td style="height: 25px">
                                                &nbsp;<asp:TextBox ID="tbValidPassword" runat="server" Width="150px"></asp:TextBox>
                                                <ShoveWebUI:ShoveConfirmButton ID="btnGO" runat="server" Text=" 确定 " OnClick="btnGO_Click" />
                                            </td>
                                        </tr>
                                        <tr style="background-color: White;">
                                            <td align="right" class="style1">
                                            </td>
                                            <td class="style2">
                                                <asp:Label ID="Label3" Style="left: 322px; top: 203px;" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </asp:Panel>
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
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
