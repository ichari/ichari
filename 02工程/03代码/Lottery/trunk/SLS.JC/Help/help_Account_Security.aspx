<%@ Page Language="C#" AutoEventWireup="true" CodeFile="help_Account_Security.aspx.cs"
    Inherits="Help_help_Account_Security" %>

<%@ Register Src="../UserControls/HelpCenter.ascx" TagName="Help" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>账户安全-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="keywords" content="<%=_Site.Name %>帮助中心-帐户安全" />
    <meta name="description" content="<%=_Site.Name %>提供：用户登录-账户充值-选择彩种、选择玩法、选号投注 -点击“立即投注”按钮---投注成功-中大奖了，我要提款！的帮助" />
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../favicon.ico" />
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">

    <div id="content">
        <div id="help_left">
            <uc2:Help ID="Help" runat="server" />
        </div>
        <div id="help_right">
            <table border="0" cellpadding="0" cellspacing="0"  width="100%">
                <tr class="bg1x bgp5">
                    
                    <td align="center" width="90" id="tdHelpCenter" style="cursor: hand;" class="fs14 bg1x bgp1 afff fw">
                        <a href="Help_Default.aspx">帮助中心</a>
                    </td>
                    
                    <td align="center" width="90" id="tdPlayType" style="cursor: hand;" class="hui14">
                        <a href="Play_Default.aspx">玩法介绍</a>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td height="32" width="20">
                        &nbsp;
                    </td>
                    <td width="5">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E3E3E4">
                <tr bgcolor="#CCCCCC">
                    <td bgcolor="#ffffff" class="blue14" style="padding: 20px 25px 20px 25px; background-image: url(../images/zfb_bg_blue.jpg);
                        background-repeat: repeat-x; background-position: center top;">
                        <h1>帐户安全</h1>
                        <p>
                            第一步：<span>实名制认证</span><br />
                            本站采用支付宝实名制认证，须填写真实姓名与身份证号码<br /><img src="images/help_20_4.jpg" />
                        </p>
                        <p>
                            第二步：<span>用户昵称</span><br />
                            支付宝用户第一次登录时需填写用户昵称，以保证支付宝账户的安全性。<br />
                            <img src="images/help_20_5.jpg" />
                        </p>
                        <p>
                            第三步：<span>安全保护问题</span><br />
                            支付宝用户第一次登录时需设置安全问题与答案，在修改用户信息以及提款的时候需要验证安全保护问题<br />
                            <img src="images/help_20_6.jpg" />
                        </p>
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
    <!--#includefile="../Html/TrafficStatistics/1.htm"-->
</body>
</html>
