<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserRegChecking.aspx.cs" Inherits="UserRegChecking" %>

<%@ Register Src="Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc1" %>
<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员注册 - <%=_Site.Name %>-竞彩足球，竞彩足球，足彩，数字彩-买彩票就上<%=_Site.Name %></title>
    <meta name="description" content="会员注册，<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。" />
    <meta name="keywords" content="会员注册，竞彩开奖，彩票走势图，超级大乐透，排列3/5" />
    <link href="Style/login.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="favicon.ico" />
    <link href="/Style/index.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form runat="server" id="form1">
    <!--header-->
<div class="login_header">
	<div class="login_topbar">
    	<div class="t_leftnav">
			<ul>
				<li><a href="/ForeCast.aspx">彩票资讯</a></li>
				<li><a href="/WinQuery/OpenInfoList.aspx">全国开奖</a></li>
				<li><a href="/GuoGuan/">过关统计</a></li>
				<li><a href="/BFlive.aspx">即时比分</a></li>
				<li class="noline"><a href="/TrendCharts/">数据图表</a></li>
			</ul>
		</div>
		<div class="login_nav">
			<dl><a href="/UserLogin.aspx">登录</a> <a href="#">注册</a></dl>
			<dl class="ico_home"><a href="/JCZC/buy_spf.aspx">购买彩票</a> <a href="/Join/">合买大厅</a></dl>
		</div>
    </div>
	<div class="reg_logo">
		<a href="/Default.aspx"><img src="images/logo_login.gif" width="108" height="57" alt="" style="border:0px" /></a>
	</div>
</div>
<!--header end-->
<!--wrap-->
<div class="reg_wrap">
	<div class="reg_tab">
		<div class="reg_topflow">
			<dl><img src="images/reg_flowpic2.gif" width="570" height="23" alt="购彩流程" /></dl>
		</div>
		<div class="reg_rzcont">
			<dl class="tipwin">恭喜您以<em>注册成功</em>，为了您能获得更好的服务请您做以下验证。以后在说 &nbsp;&nbsp; <a href="/Home/Room/ViewAccount.aspx">用户中心</a> &nbsp;&nbsp; <a href="/JCZC/Buy_SPF.aspx">购买彩票</a></dl>
			<div class="rzTabTitle">
				<ul id="rzTab">
					<li class="active" id="ntEmai" onclick="nTabs(this,0);"><span class="email">电子邮箱认证</span></li>
					<li class="normal" id="ntMobile" onclick="nTabs(this,1);"><span class="phone">手机号码认证</span></li>
				</ul>
			</div>
			<div class="rzTabContent">
				<div id="rzTab_Content0" runat="server">
					<div class="send_tip">
						<dl>
							<span>Email 认证:							
							    <asp:TextBox runat="server" id="tbEmail" name="tbEmail" class="regip"></asp:TextBox>							
							</span>							
							<span class="r_tip c_red" id="tip_Eamil" style="display:none"><span class="icon_wrong"></span>Email格式不正确，请确认。</span>
						</dl>
						<dl><span class="c_999" style="padding-left:60px;">请输入您常用的Email，以便获免费的专家分析和中奖提醒。</span></dl>
					</div>
					<div class="rz_repeat">
						<span>
						    <asp:Button runat="server" id="btnEmail" class="btn_rzsend" Text="发送验证邮件" 
                            onclick="btnEmail_Click" />
						</span>
					</div>
				</div>
				<div id="rzTab_Content1" class="none">
				    <div id="pnMobile" runat="server">
					    <div class="send_tip">
						    <dl>
							    <span>手机认证:&nbsp;
							        <asp:TextBox ID="tbMobile" runat="server" class="regip"></asp:TextBox>							    
							    </span>
							    <span class="r_tip c_red" id="tip_Mobile" style="display:none"><span class="icon_wrong"></span>手机号不正确，请确认。</span>
						    </dl>
						    <dl><span class="c_999" style="padding-left:60px;">请输入您的手机号，以便获及时的中奖通知。</span></dl>
					    </div>
					    <div class="rz_repeat">
						    <span>
						        <ShoveWebUI:ShoveConfirmButton ID="btnMobile" Style="cursor: pointer;" runat="server" class="btn_rzsend"
                         CausesValidation="False" Text="发送验证" BorderStyle="None" OnClick="btnMobile_Click" OnClientClick="if(!checkReg()){return false;}"/>
						    </span>
					    </div>
					</div>
					<div id="pnInputMobile" runat="server" visible="false">
					    <div id="Tip_Mobile" runat="server"></div>
					    <!--输入验证码-->
					    <div class="send_tip">
						    <dl>
							    <span>输入验证码:<asp:TextBox ID="tbValidPassword" runat="server" class="regip"></asp:TextBox></span>
						    </dl>
						    <dl><span class="c_999" style="padding-left:60px;">请输入您的手机号获取的验证码。</span></dl>
					    </div>
					    <div class="rz_repeat">
						    <span>
						    <ShoveWebUI:ShoveConfirmButton ID="btnGO" runat="server" Text=" 确定 " OnClick="btnGO_Click" class="btn_rzsend" />
						    </span>
					    </div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="cl"></div>

</div>
<!--login_footer-->
<div class="footer_login">
	<uc1:WebFoot ID="WebFoot1" runat="server"/>
</div>
<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<input id="hidType" name="hidType" type="hidden" value="0" runat="server" />
</form>
</body>
</html>
<script type="text/javascript">    
    
    function nTabs(thisObj, Num) {
        if (Num == 1) {
            $("#hidType").val(1); 
            document.getElementById("ntEmai").className = "normal";
            document.getElementById("ntMobile").className = "active";
            document.getElementById("rzTab_Content1").style.display = "block";
            document.getElementById("rzTab_Content0").style.display = "none";
        }
        else {
            $("#hidType").val(0);
            document.getElementById("ntEmai").className = "active";
            document.getElementById("ntMobile").className = "normal";
            document.getElementById("rzTab_Content1").style.display = "none";
            document.getElementById("rzTab_Content0").style.display = "block";
        }
    }

    $("#btnEmail").click(function() {
        isRegeditEmail();
    });

    function checkReg() {
        var mobile = $("#tbMobile").val();
        var isMobile = (/^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/);
        if (!isMobile.test(mobile)) {
            $("#tip_Mobile").show();
            $('#tbMobile').attr("class", "regip3");
            return false;
        } else {
            $("#tip_Mobile").hide();
            $('#tbMobile').attr("class", "regip");
            return true;
        }
    }

    if ($("#hidType").val() == 1) {
        nTabs($("#ntEmai"), 1);
    }
    else {
        nTabs($("#ntMobile"), 0);
    }

    $("#tbEmail").bind({
        blur: function() {
            isRegeditEmail();
        },
        focus: function() {
            $(this).attr("class", "regip2");
        }
    })

    $("#tbMobile").bind({
        blur: function() {
            checkReg();
        },
        focus: function() {
            $(this).attr("class", "regip2");
        }
    })

    function isRegeditEmail() {
        var email = $("#tbEmail").val();
        var isemail = (/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/);
        if (!isemail.test(email) || email.length > 30) {
            $("#tip_Eamil").show();
            $('#tbEmail').attr("class", "regip3");
            return false;
        } else {
            $("#tip_Eamil").hide();
            $('#tbEmail').attr("class", "regip");
            return true;
        }
    }
</script>