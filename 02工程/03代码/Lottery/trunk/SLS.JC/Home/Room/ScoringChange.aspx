<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScoringChange.aspx.cs" Inherits="Home_Room_ScoringChange" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>积分兑换-我的积分-<%= _Site.Name %> ！</title>
    <meta name="description" content="<%= _Site.Name %> 是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JScript/Public.js"></script>
    <script type="text/javascript">
		<!--
		function btnGoClick(){
			var o_labScoring = window.document.all["labScoring"];
			var o_tbScoring = window.document.all["tbScoring"];
            var Opt_ScoringExchangeRate = <%=Opt_ScoringExchangeRate.ToString()%>;
			var Scoring = o_tbScoring.value;
			if (Scoring < Opt_ScoringExchangeRate){
				alert("请输入正确的兑换积分(积分必须为 " + Opt_ScoringExchangeRate + " 的整数倍)。");
				o_tbScoring.focus();
				return false;
			}
			
			if (Math.floor(Scoring / Opt_ScoringExchangeRate) * Opt_ScoringExchangeRate != Scoring){
				alert("请输入正确的兑换积分(积分必须为 " + Opt_ScoringExchangeRate + " 的整数倍)。");
				o_tbScoring.focus();
				return false;
			}

			if (Scoring > StrToFloat(o_labScoring.innerText)){
				alert("您账户的可兑换积分不足，请重新输入。");
				o_tbScoring.focus();				
				return false;
			}

			if (!confirm("您确信输入无误并要兑换积分吗？")){
				return false;
			}
			return true;
		}
		-->
    </script>
</head>
<body class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="ptRedeem" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" background="images/admin_qh_100_2.jpg" >
                <tr>                    
                    <td width="120" id="tdHistory" align="center" background="images/admin_qh_100_1.jpg" style="cursor: pointer;" class="afff fs14">
                       我的积分兑换
                    </td><td width="10" height="32" align="left">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0" ">
                <tr style="background-color: White;">
                    <td class="hei12" style="padding: 10px; height: 61px;" align="left">
                        您现在账户上的积分为
                        <asp:Label ID="labScoringSum" runat="server" ForeColor="Red"></asp:Label>&nbsp;分，可兑换的积分为
                        <asp:Label ID="labScoring" runat="server" ForeColor="Red"></asp:Label>&nbsp;分。
                        <br />
                        现想兑换
                        <asp:TextBox ID="tbScoring" runat="server" Width="72px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;分&nbsp;(<font
                            color="#ff0000"><%=Opt_ScoringExchangeRate.ToString()%></font>&nbsp;的整数倍)&nbsp;&nbsp;&nbsp;&nbsp;
                        <ShoveWebUI:ShoveConfirmButton ID="btnOK" runat="server" BackColor="Transparent"
                            BackgroupImage="Images/buttbg.gif" Style="font-size: 9pt; cursor:pointer; border-top-style: none; font-family: 宋体; border-right-style: none; border-left-style: none;
                            border-bottom-style: none;" Width="60px" Text="立即兑换" OnClick="btnOK_Click" OnClientClick="if (!btnGoClick()) return false; this.disabled=true;"
                            Height="20px" />
                    </td>
                </tr>
                <tr style="background-color: White;">
                    <td style="height:1px">
                        &nbsp;
                    </td>
                </tr>
                <tr style="background-color: White;">
                    <td style="height: 200px; padding: 10px;" align="left" class="black12">
                        <span class="red12">我的积分规则</span> ：<br />
                        <br />
                        1. 我的积分是对用户参与本站彩票代购、合买进行奖励的积分机制。<br />
                        <br />
                        2. 我的购彩积分：<br />
                        <font face="宋体">&nbsp;&nbsp;&nbsp; 在本站参与代购、合买彩票的用户，每成功购买满 <font color="#ff0000">1</font>
                            元（撤单、方案未成功不积分），积 <font color="#ff0000">
                                <%= ScoringOfSelfBuy.ToString()%>
                            </font>分，单次投注不满 <font color="#ff0000">1</font> 元不积分。<br />
                            <br />
                            3.我的推荐积分：<br />
                            &nbsp;&nbsp;&nbsp; 本站会员用一个属于自己专用的链接地址
                            <asp:HyperLink ID="hlCommend" runat="server" CssClass="li3" NavigateUrl="Commend.aspx">推荐他人注册</asp:HyperLink>&nbsp;成为本站会员，并在本站参与代购、合买彩票，系统将奖励推荐积分，积分规则同“购彩积分”，即：每成功购买满
                            <font color="#ff0000">1</font> 元钱（撤单、方案未成功不积分），积 <font color="#ff0000">
                                <%= ScoringOfCommendBuy.ToString()%>
                            </font>分，单次投注不满 <font color="#ff0000">1</font> 元不积分。<br />
                            <br />
                            4.积分兑换：<br />
                            &nbsp;&nbsp;&nbsp; 积分满 <font color="#ff0000">
                                <%=Opt_ScoringExchangeRate.ToString()%></font> 分，用户可以进行积分兑换，兑换以 <font color="#ff0000">
                                    <%=Opt_ScoringExchangeRate.ToString()%></font> 分为一个兑换单位，超过 <font color="#ff0000">
                                        <%=Opt_ScoringExchangeRate.ToString()%></font> 分兑换奖励的用户，兑换积分必须是 <font color="#ff0000">
                                            <%=Opt_ScoringExchangeRate.ToString()%></font> 的整数倍，不够 <font color="#ff0000">
                                                <%=Opt_ScoringExchangeRate.ToString()%></font> 分不能兑换。<font color="#ff0000"><%=Opt_ScoringExchangeRate.ToString()%></font>
                            分兑换 <font color="#ff0000">1</font> 元，兑换后此款项自动增加到用户的可用资金中。</font>
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
