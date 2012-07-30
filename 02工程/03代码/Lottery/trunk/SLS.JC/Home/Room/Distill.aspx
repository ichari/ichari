<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Distill.aspx.cs" Inherits="Home_Room_Distill" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_Site.Name %>中奖奖金提款-我的账户-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
    <script type="text/javascript" language="javascript" src="../../JScript/Public.js"></script>
    <script language="javascript" type="text/javascript">
        function clickLottery(obj) {
            ChangeBackgroundImg();
            switch (obj.id) {
                case 'PayByBank':
                    document.getElementById("PayByBank").className = 'afff fs14';
                    document.getElementById("PayByBank").style.fontWeight = "bold";
                    document.getElementById("PayByBank").style.backgroundImage = "url('images/admin_qh_100_1.jpg')";
                    if (document.getElementById("hdStep").value == "3") {
                        distillFrame.location.href = "DistillBybank.aspx?Step=3&IsCps=<%=IsCps %>";
                    }
                    else {
                        distillFrame.location.href = "DistillBybank.aspx?IsCps=<%=IsCps %>";
                    }
                    break;
                case 'PayFee':
                    document.getElementById("PayFee").className = 'afff fs14';
                    document.getElementById("PayFee").style.fontWeight = "bold";
                    document.getElementById("PayFee").style.backgroundImage = "url('images/admin_qh_100_1.jpg')";
                    distillFrame.location.href = "DistillFee.aspx";
                    break;
                case 'PayDistill':
                    document.getElementById("PayDistill").className = 'afff fs14';
                    document.getElementById("PayDistill").style.fontWeight = "bold";
                    document.getElementById("PayDistill").style.backgroundImage = "url('images/admin_qh_100_1.jpg')";
                    distillFrame.location.href = "DistillDetail.aspx";
                    break;

            }
        }

        function ChangeBackgroundImg() {
            var arrCells = new Array('PayByBank', 'PayFee', 'PayDistill');
            var length = arrCells.length;

            for (var i = 0; i < length; i++) {
                document.getElementById(arrCells[i]).style.backgroundImage = "url('images/admin_qh_100_2.jpg')";
                document.getElementById(arrCells[i]).className = 'fs14';
                document.getElementById(arrCells[i]).style.fontWeight = "normal";
            }
        }

        function mOver(obj, type) {
            if (type == 1) {
                obj.style.textDecoration = "underline";
            }
            else {
                obj.style.textDecoration = "none";
            }
        }

        function reinitIframe() {
            var iframe = document.getElementById("distillFrame");
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
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acWidthdraw" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#9FC8EA">
                <tr>
                    <td valign="top" bgcolor="#FFFFFF">
                        <table width="100%" border="0" style="border:#E4E4E5 1px solid;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="32">
                                    <%--  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <uc1:Bottom ID="Bottom" runat="server" />
                                    </td>
                                </tr>
                            </table>--%>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="40" height="32" align="right" valign="middle" class="red14">
                                                <img src="images/icon_5.gif" width="19" height="20" runat="server"  id="imgDistill"/>
                                            </td>
                                            <td valign="middle" class="red14" style="padding-left: 10px;" runat="server" id="tdDistill">
                                                我的账户
                                            </td>
                                            <td width="11">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <%--<tr>
                                    <td height="2" colspan="3" bgcolor="#cc0000">
                                    </td>
                                </tr>--%>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" class="bg1x bgp5" border="0" cellspacing="0" cellpadding="0"style="margin-top: 10px;">
                            <tr>
                                
                                <td id="PayByBank" style="cursor: pointer;" width="100" align="center" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" onclick="clickLottery(this)">银行卡提款</td>
                                <td id="PayFee" style="cursor: pointer;" width="100" align="center" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" onclick="clickLottery(this)">
                                    资费标准
                                </td>
                                <td id="PayDistill" style="cursor: pointer;" width="100" align="center" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" onclick="clickLottery(this)">
                                    提款明细
                                </td>
                                <td width="20" height="32" align="left">&nbsp;</td>
                                <td align="center" class="blue14">&nbsp;</td>
                            </tr>
                        </table>
                        <table id="myIcaileTab" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table id="tb_mains" width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#E4E4E5">
                                        <tbody id="td_Tips">
                                            <tr>
                                                <td height="30" colspan="2" align="left" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                                    尊敬的会员 <span class="red12">
                                                        <%=_User.Name%>
                                                    </span>，为了您的提款能及时安全到帐，请如实填写以下资料进行核实：
                                                </td>
                                            </tr>
                                        </tbody>
                                        <tr>
                                            <td height="30" colspan="2" align="center" bgcolor="#FFFFFF" class="black12">
                                                <iframe id="distillFrame" name="distillFrame" runat="server" border="0" frameborder="0"
                                                    marginwidth="0" noresize scrolling="no" width="100%"></iframe>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;" id="tb_times">
                <tr>
                    <td width="24">
                        <img src="images/icon_taihao.gif" width="18" height="18" />
                    </td>
                    <td width="754" class="black12">
                        提款处理需知：<span class="red12">（周一至周日每日处理提款，除法定节日）</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="black12">
                        <p>
                            <span class="blue12">&#9733;</span> 当日16点前申请提款：当日处理 <span class="blue12">&#9733;</span>
                            当日16点后申请提款：次日处理
                        </p>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;"
                id="tb_remind">
                <tr>
                    <td bgcolor="#FFFEDF" class="black12" style="padding: 5px 10px 5px 10px;">
                        <span class="red12">温馨提醒：</span><br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 为了保障您的资金安全，您的提款申请成功，并经核对无误后，将通过人工处理到银行及第三方支付平台再汇到您的账户：<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1.针对用户正常提款申请，根据不同银行或者第三方支付平台处理情况，请1-5个工作日后查询。<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2.针对极个别用户的异常提款申请，请15个工作日后查询。<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3.如果账户资金低于10元，仍需提款，请一次性提清。
                        <!--为了保障您在本站的资金安全，您的取款要求首先要经过我们财务人员核对，核对无误后通过人工办理汇款，因为需要手工处理您的提款要求，10元以下的提款我们暂不接受。如果帐户资金低于10元，仍需提款，请一次性提清.对有明显套现或洗钱意图（充值不消费即提款）的用户本站将采用原路退回的原则处理，由于受银行处理时间的制约到帐时间将有可能超过半月。-->
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdStep" runat="server" value="PayFee" />
         </div>
    </div>


</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>


    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>

    <script type="text/javascript" language="javascript">
        var fromUrl = location.search.toString();
        var index = fromUrl.indexOf("Type") + 5;
        var type = fromUrl.substring(index, index + 1);
        if (type == "2") {
            document.getElementById("hdStep").value = "3";
            clickLottery(document.getElementById('PayByBank'));
        }
        else if (type == "1") {
            document.getElementById("hdStep").value = "3";
            clickLottery(document.getElementById('PayFee'));
        }
        else {
            clickLottery(document.getElementById('PayFee'));
        }
    </script>

    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
