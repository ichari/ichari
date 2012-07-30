<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountDetail.aspx.cs" Inherits="Home_Room_AccountDetail" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我的购彩账户交易明细-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %>
        ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="" />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
    <script type="text/javascript" language="javascript">
        var lastCheckMenu = null;
        function ClickShow(url, o) {
            ClearBag();
            if (url == null || url == "") {
                document.getElementById("iframe_playtypes").src = "AccountBuy.aspx";
                return;
            }
             document.getElementById(o).className = 'afff fs14';
             document.getElementById(o).style.background = "url(images/admin_qh_100_1.jpg)";
             document.getElementById("iframe_playtypes").src = url;
         }

         function ClearBag() {
             document.getElementById("tdAccountDetail").style.background = "url(images/admin_qh_100_2.jpg)"
             document.getElementById("tdBuy").style.background = "url(images/admin_qh_100_2.jpg)";
             document.getElementById("tdReward").style.background = "url(images/admin_qh_100_2.jpg)";

             document.getElementById("tdUserPay").style.background = "url(images/admin_qh_100_2.jpg)";
             document.getElementById("tdUserDistills").style.background = "url(images/admin_qh_100_2.jpg)";
             document.getElementById("tdScoring").style.background = "url(images/admin_qh_100_2.jpg)";

             document.getElementById("tdAccountDetail").className = '';
             document.getElementById("tdBuy").className = '';
             document.getElementById("tdReward").className = '';

             document.getElementById("tdUserPay").className = '';
             document.getElementById("tdUserDistills").className = '';
             document.getElementById("tdScoring").className = '';

         }
     
         function mOver(obj, styleClass) {
             if (obj != lastCheckMenu) {
                 //obj.className = '';
             }
         }

         function mOut(obj, styleClass) {
             if (obj != lastCheckMenu) {
                 //obj.className = '';
             }
         }

         function reinitIframe() {
             var iframe = document.getElementById("iframe_playtypes");
             try {
                 var bHeight = iframe.contentWindow.document.body.scrollHeight;
                 var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
                 var height = Math.max(bHeight, dHeight);
                 iframe.height = height;
             } catch (ex) { }
         }
         window.setInterval("reinitIframe()", 200);
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
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acTrans" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" style="border:#E4E4E5 1px solid;" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="32" align="right"align="middle" class="red14">
                        <img src="images/icon_5.gif" width="19" height="20" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的账户
                    </td>
                </tr>
            </table>

            <!--头部选项-->
            <table width="100%" border="0" class="bg1x bgp5" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                <tr>
                    
                    <td width="100" id="tdBuy" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" background="images/admin_qh_100_1.jpg" class="afff fs14" style="cursor: pointer;"
                        onclick="ClickShow('AccountBuy.aspx','tdBuy')">
                        购彩记录
                    </td>
                    <td width="6">
                        &nbsp;
                    </td>
                    <td width="100" id="tdAccountDetail" align="center" background="images/admin_qh_100_2.jpg"
                        onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" class="blue12" style="cursor: pointer;"
                        onclick="ClickShow('AccountDetails.aspx','tdAccountDetail')">
                        账户明细
                    </td>
                    <td width="6">
                        &nbsp;
                    </td>
                    <td width="100" id="tdReward" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" background="images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"
                        onclick="ClickShow('AccountWinMoney.aspx','tdReward')">
                        奖金派发
                    </td>
                    <td width="6">
                        &nbsp;
                    </td>
                    <td width="100" id="tdUserPay" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" background="images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"
                        onclick="ClickShow('AccountAddMoney.aspx','tdUserPay')">
                        充值记录
                    </td>
                    <td width="5">
                        &nbsp;
                    </td>
                    <td width="100" id="tdUserDistills" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" background="images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"
                        onclick="ClickShow('AccountDrawMoney.aspx','tdUserDistills')">
                        提款记录
                    </td>
                    <td width="5">
                        &nbsp;
                    </td>
                    <td width="100" id="tdScoring" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" background="images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"
                        onclick="ClickShow('AccountScore.aspx','tdScoring')">
                        积分明细
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td width="20" height="32" align="left">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table id="myIcaileTab" runat="server" width="100%" height="100%" border="0" cellpadding="0"
                cellspacing="0">
                <tr>
                    <td align="center">
                        <iframe id="iframe_playtypes" name="iframe_playtypes" width="100%" height="100%"
                            frameborder="0" scrolling="no" src="AccountBuy.aspx" onload="document.getElementById('iframe_playtypes').style.height=iframe_playtypes.document.body.scrollHeight;">
                        </iframe>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdCurDiv" runat="server" value="divBuy" />
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
