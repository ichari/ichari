<%@ Page Language="C#" AutoEventWireup="true" CodeFile="play_61.aspx.cs" Inherits="Help_play_61" %>

<%@ Register Src="~/UserControls/PlayType.ascx" TagName="Help" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>时时彩彩票玩法介绍-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！ </title>
    <meta name="keywords" content="时时彩彩票玩法介绍" />
    <meta name="description" content="<%=_Site.Name %>提供时时彩彩票玩法介绍" />
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
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr class="bg1x bgp5">
                    <td align="center" width="90" style="cursor: hand;" class="hui14">
                        <a href="Help_Default.aspx">帮助中心</a>
                    </td>
                    <td align="center" width="90" style="cursor: hand;" class="fs14 bg1x bgp1 afff fw">
                        <a href="Play_Default.aspx">玩法介绍</a>
                    </td>
                    <td height="32" width="20">&nbsp;</td>
                    <td width="5">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E3E3E4">
                <tr bgcolor="#CCCCCC">
                    <td bgcolor="#ffffff" class="black12" style="padding: 20px 25px 20px 25px; background-image: url('/Home/Room/Images/zfb_bg_blue.jpg');
                        background-repeat: repeat-x; background-position: center top;">
                        <h1 class="tc" style="line-height:45px">时时彩玩法介绍</h1>
                        <p>
                            <span style="font-weight:bold;">一、发行周期和开奖时间</span><br />
                            开奖时间：彩期在每天白天10点至22点，夜场22点至凌晨2点开售；<br />
                            白天10分钟一期，夜场5分钟一期；每日期数：白天72期，夜场48期，共120期。<br />
                            <%= _Site.Name %>将在当期官方开奖前 3分钟 截止投注，官方开奖后 3分钟 开奖派奖。
                        </p>
                        <br />
                        <p>
                            <span style="font-weight:bold;">二、怎么中奖 </span>
                        </p>
                        <p>各种玩法的奖金计算方式如下：</p>
                        <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                            <tr>
                                <td width="552" colspan="6" bgcolor="#E1E1E1">
                                    <p>假设当期的开奖号码是46878</p>
                                </td>
                            </tr>
                            <tr>
                                <td width="552" rowspan="2" align="center" bgcolor="#eaeaea">
                                    <p>奖级设置</p>
                                </td>
                                <td width="552" rowspan="2" align="center" bgcolor="#eaeaea">
                                    <p>中奖条件</p>
                                </td>
                                <td width="552" rowspan="2" align="center" bgcolor="#eaeaea">
                                    <p>中奖金额</p>
                                </td>
                                <td width="552" colspan="3" align="center" bgcolor="#eaeaea">
                                    <p>投注方式</p>
                                </td>
                            </tr>
                            <tr>
                                <td width="552" align="center" bgcolor="#eaeaea">
                                    <p>单选</p>
                                </td>
                                <td width="552" align="center" bgcolor="#eaeaea">
                                    <p>组合复式</p>
                                </td>
                                <td width="552" align="center" bgcolor="#eaeaea">
                                    <p>复选复式</p>
                                </td>
                            </tr>
                            <tr>
                            	<td width="552" align="center" bgcolor="#FAFAFA">
                            		<p>五星</p>
                            	</td>
                            	<td width="552" align="center" bgcolor="#FAFAFA">
                            		<p>5位全中，&nbsp;如46878</p>
                            	</td>
                            	<td width="552" align="center" bgcolor="#FAFAFA">
                            		<p>100000元</p>
                            	</td>
                            	<td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                            		<p>即单注，同排列3或3D的直选单式票一样，如购买46878</p>
                            	</td>
                            	<td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                            		<p>同排列3或3D的直选复式票一样，如购买&nbsp;万位：046&nbsp;千位：967&nbsp;百位：287&nbsp;十位：789&nbsp;个位：08&nbsp;此复式投注共计：324元。投注额=每位所选号个数相乘再&#215;2。</p>
                            	</td>
                            	<td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                            		<p>复选五星，购买法如：4+6+8+7+8&nbsp;，该票共8元，由以下4注购成46878（五星）、<br />878（三星）、<br />78（二星）、<br />8（一星）</p>
                            	</td>
                            	</tr>
                            <tr>
                            	<td width="552" align="center" bgcolor="#FAFAFA">
                            		<p>
                            			三星
                            			</p>
                            		</td>
                            	<td width="552" align="center" bgcolor="#FAFAFA">
                            		<p>
                            			中后三位（百、十、个），如878
                            			</p>
                            		</td>
                            	<td width="552" align="center" bgcolor="#FAFAFA">
                            		<p>
                            			1000元
                            			</p>
                            		</td>
                            	<td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                            		<p>
                            			如购买878
                            			</p>
                            		</td>
                            	<td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                            		<p>
                            			如购买&nbsp;百位：287&nbsp;十位：789&nbsp;个位：08&nbsp;此复式投注共计：36元。
                            			</p>
                            		</td>
                            	<td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                            		<p>
                            			如8+7+8&nbsp;该票共6元，由以下3注购成878（三星）、78（二星）、8（一星）
                            			</p>
                            		</td>
                            	</tr>
                            <tr>
                                <td width="552" align="center" bgcolor="#FAFAFA">
                                    <p>
                                        二星
                                    </p>
                                </td>
                                <td width="552" align="center" bgcolor="#FAFAFA">
                                    <p>
                                        中后两位（十、个），&nbsp;如78
                                    </p>
                                </td>
                                <td width="552" align="center" bgcolor="#FAFAFA">
                                    <p>
                                        100元
                                    </p>
                                </td>
                                <td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                                    <p>
                                        如购买78
                                    </p>
                                </td>
                                <td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                                    <p>
                                        如购买&nbsp;十位：789&nbsp;个位：08&nbsp;此复式投注共计：12元。
                                    </p>
                                </td>
                                <td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                                    <p>
                                        如7+8&nbsp;该票共4元，由以下2注购成78（二星）、8（一星）
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="552" align="center" bgcolor="#FAFAFA">
                                    <p>
                                        一星
                                    </p>
                                </td>
                                <td width="552" align="center" bgcolor="#FAFAFA">
                                    <p>
                                        中最后一位（个），&nbsp;如8
                                    </p>
                                </td>
                                <td width="552" align="center" bgcolor="#FAFAFA">
                                    <p>
                                        10元
                                    </p>
                                </td>
                                <td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                                    <p>
                                        如购买8
                                    </p>
                                </td>
                                <td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                                    <p>
                                        如购买&nbsp;个位：08&nbsp;此复式投注共计：4元。
                                    </p>
                                </td>
                                <td width="552" bgcolor="#FAFAFA" style="padding-left: 10px;">
                                    <p>
                                        无（就是单选）
                                    </p>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <p class="blue14">
                            三、怎么玩</p>
                        <p>
                            &#160;二星组选是&#8220;时时彩二星&#8221;的附加玩法。是指投注者可在从0&#8212;9十个数字中任选两个不同的数字对十位和个位进行投注。所选的号码与当期开奖号码相同（顺序不限），即为中奖。例如：二星组选投注号码为81，开奖号码的十位、个位为18或者81都中奖。&nbsp;
                        </p>
                        <p>
                            二星组选的投注方式：复式投注、单式投注、和值投注
                        </p>
                        <p>
                            3、&#8220;五星通选&#8221;玩法特点
                        </p>
                        <p>
                            &#9312;、玩法简单、中奖容易
                        </p>
                        <p>
                            &#160;&#160;&#160;&#8220;五星通选&#8221;是从0－9任选5个数进行投注，选号时各位置上所选数字可以有相同数。选号简单、玩法灵活、选中5个数的中奖机会是1/100000，选中3个数的中奖机会是1/1000，选中2个数的中奖机会是1/100，属于中奖概率大的游戏。
                        </p>
                        <p>
                            &#9313;、两元一注号码，五次中奖机会
                        </p>
                        <p>
                            &#160;&#160;&#160;&#8220;五星通选&#8221;设3个奖级，全部为固定奖。两元一注号码，三个奖级通吃，五次中奖机会。即使选对５个数，要是选中了&#8220;前三&#8221;、&#8220;后三&#8221;或者&#8220;前对&#8221;、&#8220;后对&#8221;都可中奖。
                        </p>
                        <p>
                            &#9314;、每注号码可兼中兼得，彩民可得更多的实惠。
                        </p>
                        <p>
                            &#160;&#160;&#160;&#8220;五星通选&#8221;游戏规则明确：当期每注号码可兼中兼得。下注2元钱，如果选中一等奖号码，就可以通吃一、二、三等奖，这给彩民朋友带来了更多实惠。这是&#8220;五星通选&#8221;玩法吸引人的最大卖点。
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
