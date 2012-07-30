<%@ Page Language="C#" AutoEventWireup="true" CodeFile="help_Invest.aspx.cs" Inherits="Help_help_Invest" %>

<%@ Register Src="../UserControls/HelpCenter.ascx" TagName="Help" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>如何查看投注记录-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="keywords" content="<%=_Site.Name %>帮助中心-如何查看投注记录" />
    <meta name="description" content="<%=_Site.Name %>提供：用户登录-账户充值-选择彩种、选择玩法、选号投注-点击“立即投注”按钮---投注成功-中大奖了，我要提款！的帮助" />
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
                         <h1>如何查看投注记录</h1>
                        <p>
                            第一步：<span>点击我的账户</span><br /><img src="images/help_5_1.jpg" />
                        </p>
                       <p>
                         第二步：<span>点击投资查询</span><br /><img src="images/help_5_2.jpg" />
                       </p>
                        <p>
                            第三步：<span>选择投注记录</span><br /><img src="images/helps_1_5.jpg" class="img"/><img style="border: 1px solid #DDDDDD;" src="images/help_5_4.jpg" />
                        </p>
                        <p>
                            
                            第四步：<span>选择需要查看的彩种</span><br />
                            <img style="border: 1px solid #DDDDDD;" src="images/help_5_5.jpg" />
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
<!--#includefile="../Html/TrafficStatistics/1.htm"--></body>
</html>
