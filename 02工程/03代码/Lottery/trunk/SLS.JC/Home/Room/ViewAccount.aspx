<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewAccount.aspx.cs" Inherits="Home_Room_ViewAccount" %>
<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>账户全览-我的账号-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
<link rel="shortcut icon" href="/favicon.ico" /></head>
<body  class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acView" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" style="border:#E4E4E5 1px solid;" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="32" align="right" valign="middle" class="red14">
                        <img src="images/icon_5.gif" width="19" height="20" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的账户
                    </td>
                </tr>
            </table>
            <table width="100%"  class="bg1x bgp5" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                <tr>
                    <td class="bg1x bgp1 afff fs14" width="100" height="32" align="center">
                        <a href="ViewAccount.aspx"><strong>帐户全览</strong></a>
                    </td>
                    <td width="4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E4E4E5">
                <tr>
                    <td width="17%" height="30" align="right" bgcolor="#F8F8F8" class="black12">
                        用户类型：<span class="red12"></span>
                    </td>
                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <asp:Label ID="labUserType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" bgcolor="#F8F8F8" class="black12">
                        账户余额：
                    </td>
                    <td width="22%" align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <span class="red12"><asp:Label ID="labBalance" runat="server"></asp:Label></span> 元
                    </td>
                    <td width="61%" align="left" bgcolor="#FFFFFF" class="blue12" style="padding-left: 10px;">
                        <a href="Distill.aspx" target="_self">【我要提款】</a>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" bgcolor="#F8F8F8" class="black12">
                        已冻结金额：
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <span class="red12"><asp:Label ID="labFreeze" runat="server" CssClass="zw9"></asp:Label></span>元
                    </td>
                    <td align="left" bgcolor="#FFFFFF" style="padding-left: 10px;">
                        <a href="AccountFreezeDetail.aspx" style="text-decoration: none" target="_self" class="blue12" runat="server" id="lbFreezDetail" visible="false">【冻结明细】</a>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" bgcolor="#F8F8F8" class="black12">
                        可投注金额：
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <span class="red12"><asp:Label ID="labBalanceDo" runat="server"></asp:Label></span> 元
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="blue12" style="padding-left: 10px;">
                        <a href="../../Default.aspx" target="_blank">【我要购彩】</a> <a href="/Join/Default.aspx" target="_blank">【我要合买】</a>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" bgcolor="#F8F8F8" class="black12">
                        我的积分：
                    </td>
                    <td align="left" bgcolor="#FFFFFF" class="blue12" style="padding-left: 10px;" colspan="2">
                        <asp:Label ID="labScoring" runat="server"></asp:Label><span class="black12">&nbsp;分</span>
                    </td>
                    <!--
                        <td align="left" bgcolor="#FFFFFF" class="blue12" style="padding-left: 10px;">
                            <a href="#">【积分兑换礼品】</a> <a href="#">【积分抽奖】</a> <a href="#">【积分换金币】</a>
                        </td>
                    -->
                </tr>
                <tr>
                    <td height="30" align="right" bgcolor="#F8F8F8" class="black12">
                        &nbsp;
                    </td>
                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="black12" style="padding: 10px;">
                        如果你已经充值，银行账户钱扣了，而您的账户还没有加上，请点击
                        <span class="blue12_2">
                        <%--<script type='text/javascript' src='http://www.365webcall.com/IMMe1.aspx?settings=mw7m6m7mIbI06Nz3A0Nbbbz3A00wb0z3AN6mmNm&LL=0'></script>--%>
                        </span> 告诉我们，我们将第一时间为您处理！<br />
                        <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#FFCD33" style="margin-top: 10px;">
                            <tr>
                                <td bgcolor="#FFFFE1" class="red12" style="padding: 10px;">
                                    资金冻结的原因：<span class="black12"><br />
                                    1、方案发起人保底<br />
                                    2、用户取款<br />
                                    3、追号冻结<br />
                                    4、自动跟单冻结 </span>
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
