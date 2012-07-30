<%@ Page Language="C#" AutoEventWireup="true" CodeFile="play_5.aspx.cs" Inherits="Help_help_5" %>

<%@ Register Src="~/UserControls/PlayType.ascx" TagName="Help" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>双色球彩票玩法介绍-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！ </title>
    <meta name="keywords" content="双色球彩票玩法介绍" />
    <meta name="description" content="<%=_Site.Name %>提供双色球彩票玩法介绍" />
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="/favicon.ico" />
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
            <table border="0" cellpadding="0" cellspacing="0" width="842">
                <tr class="bg1x bgp5">
                    <td align="center" width="90" id="tdHelpCenter" style="cursor: hand; background-color: #E4E4E4;"
                        class="hui14">
                        <a href="Help_Default.aspx">帮助中心</a>
                    </td>
                    <td align="center" width="90" id="tdPlayType" style="cursor: hand;" class="fs14 bg1x bgp1 afff fw">
                        <a href="Play_Default.aspx">玩法介绍</a>
                    </td>
                    <td height="32" width="20">
                        &nbsp;
                    </td>
                    <td width="5">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E3E3E4">
                <tr bgcolor="#CCCCCC">
                    <td bgcolor="#ffffff" class="black12" style="padding: 20px 25px 20px 25px; background-image: url('/Home/Room/Images/zfb_bg_blue.jpg');
                        background-repeat: repeat-x; background-position: center top;">
                        <h1 class="tc" style="line-height:45px;">双色球玩法介绍</h1>
                        <p>
                            <span class="blue14">一、发行周期和开奖时间：</span><br />
                            双色球每周发行三期，分别在每周二、四、日三天晚上20:45开奖，中国教育电视台（CETV-1）对开奖进行现场直播。<br />
                            <br />
                            <span class="blue14">二、怎么玩</span><br />
                            双色球投注分为红球和蓝球，红球号码范围为01～33，蓝球号码范围为01～16，每期开出6个红球和1个蓝球作为中奖号码。<br />
                            双色球玩法即是竞猜开奖号码的6个红球号码和1个蓝球号码，顺序不限：
                            <br />
                        </p>
                        <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#EBEBEB">
                                    奖级
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#EBEBEB">
                                    <p>
                                        红球
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#EBEBEB">
                                    <p>
                                        蓝球
                                    </p>
                                </td>
                                <td colspan="2" align="center" valign="center" bgcolor="#EBEBEB">
                                    <p>
                                        中奖条件</p>
                                </td>
                                <td width="290" colspan="2" align="center" valign="center" bgcolor="#EBEBEB">
                                    <p>
                                        奖金
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#FFFFFF" class="blue12">
                                    <p>
                                        一等奖
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;&#9679;&#9679;&#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                                <td colspan="2" align="center" valign="center" bgcolor="#F4F9FF" class="blue">
                                    <p>
                                        选6+1中6+1
                                    </p>
                                </td>
                                <td colspan="2" align="center" valign="center" bgcolor="#FFFFFF" class="blue">
                                    <p>
                                        当奖池资金低于1亿元时，单注最高限额封顶500万元。当奖池资金高于1亿元（含）时，单注最高可达1000万元。
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#FFFFFF" class="blue12">
                                    <p>
                                        二等奖
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;&#9679;&#9679;&#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                    </p>
                                </td>
                                <td colspan="2" align="center" valign="center" bgcolor="#F4F9FF" class="blue">
                                    <p>
                                        选6+1中6+0
                                    </p>
                                </td>
                                <td colspan="2" align="center" valign="center" bgcolor="#FFFFFF" class="blue">
                                    <p>
                                        高等奖奖金的30%除以中奖注数。
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#FFFFFF" class="blue12">
                                    <p>
                                        三等奖
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;&#9679;&#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                                <td colspan="2" align="center" valign="center" bgcolor="#F4F9FF" class="blue">
                                    <p>
                                        选6+1中5+1
                                    </p>
                                </td>
                                <td colspan="2" align="center" valign="center" bgcolor="#FFFFFF" class="blue">
                                    <p>
                                        3000元
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#FFFFFF" class="blue12">
                                    <p>
                                        四等奖
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;&#9679;&#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                    </p>
                                </td>
                                <td colspan="2" rowspan="2" align="center" valign="center" bgcolor="#F4F9FF" class="blue">
                                    <p>
                                        选6+1中5+0或中4+1
                                    </p>
                                </td>
                                <td colspan="2" rowspan="2" align="center" valign="center" bgcolor="#FFFFFF" class="blue">
                                    <p>
                                        200元
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#FFFFFF" class="blue12">
                                    <p>
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;&#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#FFFFFF" class="blue12">
                                    <p>
                                        五等奖
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;&#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                    </p>
                                </td>
                                <td colspan="2" rowspan="2" align="center" valign="center" bgcolor="#F4F9FF" class="blue">
                                    <p>
                                        选6+1中4+0或中3+1
                                    </p>
                                </td>
                                <td colspan="2" rowspan="2" align="center" valign="center" bgcolor="#FFFFFF" class="blue">
                                    <p>
                                        10元
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" align="center" valign="center" bgcolor="#FFFFFF" class="blue12">
                                    <p>
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="68" height="25" rowspan="3" align="center" valign="center" bgcolor="#FFFFFF"
                                    class="blue12">
                                    <p>
                                        六等奖
                                    </p>
                                </td>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;&#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                                <td colspan="2" rowspan="3" align="center" valign="center" bgcolor="#F4F9FF" class="blue">
                                    <p>
                                        选6+1中2+1或中1+1或中0+1
                                    </p>
                                </td>
                                <td colspan="2" rowspan="3" align="center" valign="center" bgcolor="#FFFFFF" class="blue">
                                    <p>
                                        5元
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="102" align="center" valign="center" bgcolor="#FFFFFF" class="red14">
                                    <p>
                                    </p>
                                </td>
                                <td width="52" align="center" valign="center" bgcolor="#FFFFFF" class="blue14">
                                    <p>
                                        &#9679;
                                    </p>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <p>
                            高等奖奖金：双色球一、二等奖为高等奖，三至六等奖为低等奖。高等奖奖金为当期奖金减去当期低等奖奖金。</p>
                        <p>
                            &nbsp;
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
