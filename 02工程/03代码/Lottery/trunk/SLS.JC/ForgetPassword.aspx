<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgetPassword.aspx.cs" Inherits="ForgetPassword" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>忘记密码</title>
    <meta name="description" content="会员注册，彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。" />
    <meta name="keywords" content="买彩票" />
    <link href="Style/ForgetPassword.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="reg_header">
	    <div class="toplogo_both">
		    <div class="toplogo">
			    <div class="logo">
				    <p><a href="/" ><img src="/images/logo.gif" width="190" height="65" /></a></p>
                    <dl class="reg"></dl>
			    </div>
			    <!--登录前-->
               <div class="top_link"  style="display: block" id="lbefore">
                    <ul>
                        <li class="dl_txt"><a href="/UserLogin.aspx">请登录</a></li>
			            <li class="gray_fg">|</li>
			            <li><a href="/UserReg.aspx" rel="nofollow" title="" class="c_btn" target="_blank" >免费注册</a></li>
                        <li> | <a href="/bbs/">彩民论坛</a> | <a href="/Help/Help_Default.aspx">帮助中心</a> | <a href="BotNav/about1.html">关于我们</a></li>
                    </ul>
                </div>
               <!--登录后-->
               <div class="top_link" style="display: none" id="lafter">
                    <ul>
                        <li>欢迎您，<em><span id="topUserName"></span></em>! &nbsp; <a href="/Home/Room/ViewAccount.aspx">我的账户</a></li>
                        <li>[<a href="/Home/Room/OnlinePay/Alipay02/Send_Default.aspx" class="c_org">充值</a>]</li>
                        <li>[<a href="#" id="topLoginOut">退出</a>]</li>
                        <li> | <a href="/bbs/">彩民论坛</a> | <a href="/Help/">帮助中心</a> | <a href="/BotNav/about1.html">关于我们</a></li>
                    </ul>
                </div>
		    </div>
	    </div>
    </div>
    <!--reg_both-->
    <div class="reg_both">
	<div class="reg_flow">
    	<h2>取回流程</h2>
        <dl><img src="images/reg_pwflow.gif" width="377" height="28" alt="取回流程" /></dl>
    </div>
    <div class="reg_main">
        <div id="top_tishi" runat="server">
    	    <div class="top_tip" id= "top_tip">正确填写通行证和邮箱后，系统会发送一封邮件到您的邮箱，您可以点击邮件中的链接进行修改密码的操作 </div>    	    
    	</div>
		<dl>
        	<dt class="r_name">用户名：</dt>
            <dt class="r_txt">
                <span><asp:TextBox ID="tbFormUserName" runat="server" class="regip" size="25"></asp:TextBox></span>
                <span class="r_tip c_red" id="spCheckResult0" name="spCheckResult0">*</span>
            </dt>
        </dl>
        <dl>
            <dt class="r_name">电子邮箱：</dt>
            <dt class="r_txt">
                <span><asp:TextBox ID="tbEmail" runat="server" class="regip" size="25"></asp:TextBox></span>
                <span class="r_tip c_red">*</span>
                <span class="r_tip c_red" id="spCheckResult3" name="spCheckResult3" style="display:none">*请填写您已激活的邮箱地址，如果邮箱没有激活，请联系客服人员帮你解决，谢谢合作。</span>
            </dt>
        </dl>
        <dl>
            <dt class="r_name">验证码：</dt>
            <dt class="r_txt">
                <span><asp:TextBox ID="tbRegCheckCode" name="tbRegCheckCode" runat="server" MaxLength="6"
                                class="reg_yzip" Width="70px" Height="25px"></asp:TextBox></span>
                <span class="r_yzm"><img src="regcode.aspx" width="50" height="20" id="CheckCode" name="CheckCode" alt="验证码" onclick="refreshVerify()"/></span>
                <span><a href="javascript:refreshVerify()"><img src="images/refresh.gif" width="13" height="13" alt="刷新" style="cursor: hand; border:0px;"/></a></span>
              <span class="r_tip c_999">请输入下图中的字符，不区分大小写</span>
              
          </dt>
        </dl>
        <dl>
        	<dt class="r_name">&nbsp;</dt>
            <dt class="r_txt"><ShoveWebUI:ShoveConfirmButton ID="btnReg" Style="cursor: pointer;" runat="server"
                        Width="120px" Height="36px" CausesValidation="False" Text="取回密码"
                        BorderStyle="None" OnClick="btnReg_Click" OnClientClick="if(!checkReg()){return false;}" class="btn_reg fl"/></dt>
        </dl>
        <div class="cl"></div>
    </div>
<div class="cl"></div>
</div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <!--#includefile="Html/TrafficStatistics/1.htm"-->
</body>
</html>
<script src="JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">

    $("#topLoginOut").click(function() {
        $.ajax({
            type: "POST",
            url: "/ajax/UserLogin.ashx",
            data: "action=loginout",
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            error: callError,
            success: callSuccess
        });

        function callSuccess(json, textStatus) {
            if (isNaN(json.message)) {
                alert(json.message);

                return;
            }

            $("#lbefore").show();
            $("#lafter").hide();
        }

        function callError() {
            alert("登录异常，请重试一次，谢谢。可能是网络延时原因。");
        }
    });

    function refreshVerify() {
        $("#CheckCode").attr("src", "regcode.aspx?rnd=" + Math.random()); 
    }
    
    function StringLength(str) {
        return str.replace(/[^\x00-\xff]/g, "**").length
    }

    function isRegisterUserName(TbObj) {

        var NameLen = StringLength($("#tbFormUserName").val());
        if (NameLen > 16) {
            $("#spCheckResult0").removeClass("r_tip c_999");
            $("#spCheckResult0").addClass("r_tip c_red");
            $("#spCheckResult0").text("* 对不起，用户名长度请不要超过16个字符");
            return false;
        }
        if (NameLen < 4) {
            $("#spCheckResult0").removeClass("r_tip c_999");
            $("#spCheckResult0").addClass("r_tip c_red");
            $("#spCheckResult0").text("* 对不起，用户名长度请不要少于4个字符");
            return false;
        }

        var patrn = /[^0-9A-Za-z\u4e00-\u9fa5]/;
        if (patrn.test(TbObj.val())) {
            $("#spCheckResult0").text("* 对不起，请使用汉字，数字或字母");
            return false
        } else {
            if (checkUserName()) {
                return true
            }
        }
    }

    function isUserMail(TbObj) {
        var patrn = /^(\w)+[-]*(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$/;
        if (!patrn.test(TbObj.val())) {
            $("#spCheckResult3").show();
            $("#spCheckResult3").removeClass("r_tip c_999");
            $("#spCheckResult3").addClass("r_tip c_red");
            $("#spCheckResult3").text("对不起，您输入的邮箱格式不正确"); ;
            return false;
        }
        return true;
    }

    function checkUserName() {
        var result = 0;
        var userName = $("#tbFormUserName").val();
        // 去除空格
        userName = userName.replace(/[ ]/g, ""); 
        if (userName == "") {
            $("#spCheckResult0").removeClass("r_tip c_999");
            $("#spCheckResult0").addClass("r_tip c_red");
            $("#spCheckResult0").text("* 用户名不能为空");
            return false;
        }
        
        $.ajax({
            type: "POST",
            url: "/ajax/CheckName.ashx", //发送请求的地址
            timeout: 20000,
            cache: false,
            async: false,
            data: "UserName=" + userName,
            dataType: "json",
            error: callError,
            success: callSuccess
        });

        function callSuccess(json, textStatus) {
            result = json.message
        }

        function callError() {
            alert("检验失败，请重试一次，谢谢。可能是网络延时原因。");
        }

        if (Number(result) < 0) {
            return true;
        }
        else {
            $("#spCheckResult0").removeClass("r_tip c_999");
            $("#spCheckResult0").addClass("r_tip c_red");
            $('#spCheckResult0').text("* 用户名不存在");
            return false;
        }
        return true;
    }

    function checkReg() {
        var userName = $("#tbFormUserName");
        var email = $("#tbEmail");
        var CheckCode = $("#tbRegCheckCode");
        if (isRegisterUserName(userName)) {
            $('#spCheckResult0').text("");
        }
        else {
            return;
        }
        if (!isUserMail(email)) {
            return false;
        }
        $("#spCheckResult3").text("");
        return true;
    }
</script>

