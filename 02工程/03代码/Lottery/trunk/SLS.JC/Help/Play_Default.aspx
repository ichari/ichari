<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Play_Default.aspx.cs" Inherits="Help_Play_Default" %>

<%@ Register Src="../UserControls/PlayType.ascx" TagName="Help" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>彩票玩法介绍-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！ </title>
    <meta name="keywords" content="彩票玩法介绍" />
    <meta name="description" content="<%=_Site.Name %>提供彩票玩法介绍">
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../favicon.ico" />
</head>

<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">

    <div id="user">
        <div id="user_l">
            <uc2:Help ID="Help" runat="server" />
        </div>
        <div id="user_r">
            <table border="0" cellpadding="0" cellspacing="0"width="100%">
                <tr class="bg1x bgp5">
                    <td align="center" width="90" id="tdHelpCenter" style="cursor: hand;" class="hui14">
                        <a href="Help_Default.aspx">帮助中心</a>
                    </td>                    
                    <td align="center" width="90" id="tdPlayType" style="cursor: hand;" class="fs14 bg1x bgp1 afff fw">
                        <a href="Play_Default.aspx">玩法介绍</a>
                    </td>
                    <td width="5">&nbsp;</td>
                    <td height="32" width="20">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E3E3E4">
                <tr bgcolor="#CCCCCC">
                    <td bgcolor="#ffffff" class="black12" style="padding: 20px 25px 20px 25px; background-image: url(../images/zfb_bg_blue.jpg);
                        background-repeat: repeat-x; background-position: center top;">
                        <h1 class="tc" style="line-height:45px;">胜负彩玩法介绍</h1>
                        <p>
                            <span class="blue14">一、关于投注</span><br />
                            胜负彩每期竞猜14场比赛，竞猜内容为主队在90分钟内的比赛结果，主队获胜则正确的竞猜结果为3；主队打平则竞猜结果为1；主队输则竞猜结果为0。
                            <br />
                            <br />
                            <span class="blue14">二、设奖</span><br />
                            “胜负游戏”只设2个浮动奖级，分别为一等奖和二等奖。
                            <br />
                            <br />
                            <br />
                            <span class="blue14">三、中奖</span><br />
                            一等奖 猜中全部14场比赛的胜平负结果；
                            <br />
                            二等奖 猜中其中13场比赛的胜平负结果。<br />
                            <br />
                            <br />
                            <span class="blue14">四、奖金</span><br />
                            一等奖为当期奖金总额的70%，及奖池和调节基金转入部分；（最高限额封顶500万）<br />
                            二等奖为当期奖金总额的30%。（最高限额封顶500万）<br />
                            <br />
                            <br />
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
