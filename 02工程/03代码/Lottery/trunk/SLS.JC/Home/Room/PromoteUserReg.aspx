<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromoteUserReg.aspx.cs" Inherits="Home_Room_PromoteUserReg" %>
<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register src="UserControls/WebFoot.ascx" tagname="WebFoot" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员注册 -
        <%=_Site.Name %>－买彩票，就上<%=_Site.Name %></title>
    <meta name="description" content="会员注册，<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站，包含足彩等众多彩种的实时开奖信息、图表走势、分析预测等。" />
    <meta name="keywords" content="会员注册，买彩票，就上" />
    <link href="/Style/login.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="/favicon.ico" />
    <link href="/Style/index.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .red14{
            	font-size: 14px;
	            color: #FF0065;
	            font-family: "tahoma";
	            line-height: 20px;
	            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
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
		<a href="/Default.aspx"><img src="/Images/logo_login.gif" width="108" height="57" alt="" style="border:0px" /></a>
	</div>
</div>
<!--header end-->
<!--wrap-->
<div class="reg_wrap">
	<div class="reg_tab">
		<div class="reg_topflow">
			<dl><img src="/Images/reg_flowpic.gif" width="570" height="23" alt="购彩流程" /></dl>
		</div>
		<div class="reg_cont">		
		    <div style="margin-left:14px;">
		        <table width="500" border="0" align="left" cellpadding="0" cellspacing="0" bgcolor="#CCCCCC">
			        <tr>
			            <td height="130" colspan="2" align="left" valign="top" background="/Home/Room/Images/bg_708.gif"
                                    bgcolor="#FFFFFF" class="black14" style="padding: 10px 0px 0px 15px; font-family: 宋体, Arial, Helvetica, sans-serif;
                                    font-size: 14px; font-weight: bold;">
                                    <div style="width: 470px">
                                        <asp:Panel ID="pnlShowInfoWithoutHongbao" runat="server">
                                            &nbsp;&nbsp;&nbsp;&nbsp;<span class="red14"><asp:Label runat="server" ID="lblCommenderName2">xxx</asp:Label>
                                            </span>请求加您为好友，请您接受<span class="red14"><asp:Label runat="server" ID="lblCommenderName1">xxx</asp:Label>
                                            </span>的好友请求,完善您的个人资料，共同在<%=_Site.Name %>上进行彩票合买，共同搏击1000万大奖!</asp:Panel>
                                        <asp:Panel ID="pnlShowInfoWithHongbao" runat="server">
                                            &nbsp;&nbsp;&nbsp;&nbsp;你的好友<span class="red14"><asp:Label runat="server" ID="lblCommenderName3">xxx</asp:Label>
                                            </span>送给你<span class="red14"><asp:Label ID="lblGifMoney1" runat="server">5</asp:Label>
                                            </span>元彩票红包，请完善个人信息领取红包，按受他的邀请，共同合买彩票搏击1000万大奖。</asp:Panel>
                                        <asp:Panel ID="pnlShowInfoPromotion" runat="server">
                                            提示:<asp:Label runat="server" ID="lblShowInfoPromotion" class="red14">xxxxxxxxxxxx.</asp:Label>
                                        </asp:Panel>
                                    </div>
                          </td>
			        </tr>
			    </table>
		    </div>
			<dl>
				<dt class="r_name">用户名：</dt>
				<dt class="r_txt">
					<span><asp:TextBox ID="TBUserName" name="TBUserName" runat="server" class="regip2" size="14" MaxLength="16"></asp:TextBox></span>
					<span  id="TBUserName_tip" name="TBUserName_tip"></span>
					<p>4-16位之间，请用英文小写、数字、下划线，不能全部是数字或下划线。</p>
				</dt>
			</dl>
			<div class="more_name" id="more_name" style="display:none;">
				<h2>您输入的用户名已被注册，为你推荐以下用户名：</h2>
				<ul>
					<li><span>可以注册</span>
					<input name="UserName_radio" type="radio" value="" /><label></label></li>
					<li><span>可以注册</span>
					<input name="UserName_radio" type="radio" value="" /><label></label></li>
					<li><span>可以注册</span>
					<input name="UserName_radio" type="radio" value="" /><label></label></li>
				</ul>
			</div>
			<dl>
				<dt class="r_name">密码：</dt>
				<dt class="r_txt">
					<span><asp:TextBox ID="TBPwdOne" name="TBPwdOne" runat="server" TextMode="Password" class="regip2" size="30" MaxLength="16"></asp:TextBox></span>
					<span class="regpw"><img src="/images/reg_pw01.gif" width="150" height="24" id="img_reg_pw" name="img_reg_pw" style="display:none;" /></span>
					<span  id="TBPwdOne_tip" name="TBPwdOne_tip"></span>
					<p>6-16字符，(字母、数字、符号)，区分大小写。</p>
				</dt>
			</dl>
			<dl>
				<dt class="r_name">确认密码：</dt>
				<dt class="r_txt">
					<span><asp:TextBox ID="TBPwdTwo" name="TBPwdTwo" runat="server" TextMode="Password" class="regip2" size="30" MaxLength="16"></asp:TextBox></span>
					<span  id="TBPwdTwo_tip" name="TBPwdTwo_tip"></span>
				</dt>
			</dl>
			<dl>
				<dt class="r_name">验证码：</dt>
				<dt class="r_txt">
					<span><asp:TextBox ID="tbRegCheckCode" name="tbRegCheckCode" runat="server" MaxLength="6" class="regip2"></asp:TextBox></span>
					<span class="r_yzm"><img src="/regcode.aspx" width="50" height="20" id="CheckCode" name="CheckCode" alt="验证码"/></span>
					<span class="r_refresh"><a href="#" onclick="refreshVerify()" alt="刷新" style="cursor: hand;">看不清</a>？</span>
			  </dt>
			</dl>
			<dl class="r_treaty">
			    <span>
			        <tr>
                        <td height="28" align="right" bgcolor="#FFFFFF" class="hui12">
                            &nbsp;
                        </td>
                        <td bgcolor="#FFFFFF" class="blue12_line">
                                <asp:CheckBox ID="ckbAgree" runat="server" Checked="True" />
                            <asp:Label ID="lblAgreeTip" runat="server">接受好友请求，领取红包！</asp:Label>同意会员<a target="_blank"
                                href="../Web/UserRegAgree.aspx?type=User" style="text-decoration: underline; font-weight: bold;">注册协议</a>。
                        </td>
                    </tr>
			    </span>
			<%--	<span><asp:CheckBox ID="ckbAgree" runat="server" Checked="true" /></span>
				<span>我已阅读并同意<a href="/Home/Web/UserRegAgree.aspx" style="color:Red;" target="_blank">《<%=_Site.Name %>用户协议》</a></span>--%>
			</dl>
			<dl class="r_btn">
				<ShoveWebUI:ShoveConfirmButton ID="ShoveConfirmButton1" Style="cursor: pointer;" runat="server"
                        Width="120px" Height="36px" CausesValidation="False" Text="提交注册"
                        BorderStyle="None" OnClick="btnReg_Click" OnClientClick="if(!checkReg()){return false;}" class="btn_reg"/>
			</dl>
		
		</div>
	</div>
	<div class="cl"></div>

</div>
<!--login_footer-->
<div class="footer_login">
    <uc1:WebFoot ID="WebFoot1" runat="server" />
</div>
    </form>
</body>
</html>
<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    window.$ = jQuery;
    
    $().ready(function() {
        $("#more_name input").each(function(i) {//check事件
            this.onclick = function() {
                $("#TBUserName").val($(this).parent().children("label").html());
                $("#more_name").hide();
                showOK("TBUserName");
            }
        });
    })
    
    function showErr(input, msg) {
        $('#' + input + "_tip").text(msg);
        $('#' + input + "_tip")[0].className = 'r_tip c_red';
        return false;
    }

    function showOK(input) {
        $('#' + input + "_tip").text('');
        $('#' + input + "_tip")[0].className = 'icon_correct';
        return true;
    }

    function inPut(input, msg) {
        var NameLen = StringLength($('#' + input).val());
        if (NameLen < 1) {
            $('#' + input + "_tip").text(msg);
            $('#' + input + "_tip")[0].className = 'r_tip c_999';
        }
    }

    $("#TBUserName").bind({
        blur: function() {
            isRegisterUserName();
        }
    })

    $("#TBPwdOne").bind({
        blur: function() {
            isRegisterPwd1();
        }
    })

    $("#TBPwdTwo").bind({
        blur: function() {
            isRegisterPwd2();
        }
    })

    function isRegisterUserName() {
        var NameLen = StringLength($("#TBUserName").val());
        if (NameLen > 16) {
            showErr("TBUserName", "对不起，用户名长度请不要超过16个字符");
            return false;
        }
        if (NameLen < 4) {
            showErr("TBUserName", "对不起，用户名长度请不要少于4个字符");
            return false;
        }

        var patrn = /[^0-9A-Za-z\u4e00-\u9fa5]/;
        if (patrn.test($("#TBUserName").val())) {
            showErr("TBUserName", "对不起，请使用汉字，数字或字母");
            return false;
        } else {
            if (checkUserName()) {
                showOK("TBUserName");
            }
        }

        return true;
    }

    function isRegisterPwd1() {
        var PwdLen = $("#TBPwdOne").val().length;
        if (PwdLen > 15) {
            showErr("TBPwdOne", "对不起，登录密码长度请不要超过15个字符");
            return false;
        }
        if (PwdLen < 6) {
            showErr("TBPwdOne", "对不起，登录密码长度请不要少于6个字符");
            return false;
        }

        var patrn = /\s/;
        if (patrn.test($("#TBPwdOne").val())) {
            showErr("TBPwdOne", "对不起，登录密码请不要使用空格");
            return false;
        }

        GetLeave($("#TBPwdOne").val());

        if ($("#TBPwdTwo").val().length != 0) {
            if (isPwdSame()) {
                showOK("TBPwdTwo");
            }
            else {
                return false;
            }
        }

        showOK("TBPwdOne");
        return true;
    }

    function isRegisterPwd2() {
        var PwdLen = $("#TBPwdTwo").val().length;
        if (PwdLen > 15) {
            showErr("TBPwdTwo", "对不起，登录密码长度请不要超过15个字符");
            return false;
        }
        if (PwdLen < 6) {
            showErr("TBPwdTwo", "对不起，登录密码长度请不要少于6个字符");
            return false;
        }
        var patrn = /\s/;
        if (patrn.test($("#TBPwdTwo").val())) {
            showErr("TBPwdTwo", "对不起，登录密码请不要使用空格");
            return false;
        }

        if (isPwdSame()) {
            showOK("TBPwdTwo");
        }
        else {
            showErr("TBPwdTwo", "对不起，您两次输入的密码不一致");
            return false;
        }

        showOK("TBPwdTwo");
        return true;
    }

    function checkUserName() {
        if ($("#TBUserName").val() == "") {
            showErr("TBUserName", "用户名不能为空");
        }

        var result = 0;
        
        $.ajax({
            type: "POST",
            url: "/ajax/CheckName.ashx", //发送请求的地址
            timeout: 20000,
            cache: false,
            async: false,
            data: "UserName=" + $("#TBUserName").val(),
            dataType: "json",
            error: callError,
            success: callSuccess
        });

        function callSuccess(json, textStatus) {
            result = json.message;
            
            if (Number(result) < 0) {
                if (Number(result) == -1) {
                    showErr("TBUserName", "对不起用户名中含有禁止使用的字符");
                    return false;
                }

                if (Number(result) == -2) {
                    $("#more_name").show();
                    $("#more_name label").eq(0).text(json.UserName1);
                    $("#more_name label").eq(1).text(json.UserName2);
                    $("#more_name label").eq(2).text(json.UserName3);
                    return false;
                }

                if (Number(result) == -3) {
                    showErr("TBUserName", "用户名长度在 4-16 个英文字符或数字、中文 2-8 之间");
                    return false;
                }
            }
            else {
                showOK("TBUserName");

                return true;
            }
        }

        function callError() {
            alert("检验失败，请重试一次，谢谢。可能是网络延时原因。");
        }
    }

    function GetLeave(str) {
        var PwdLeave = 0;

        var RegNum = /[0-9]/;
        var Regaz = /[a-z]/;
        var RegAZ = /[A-Z]/;
        var RegNot = /[^0-9A-Za-z]/;

        if (RegNum.test(str)) {
            PwdLeave += 1;
        }

        if (Regaz.test(str)) {
            PwdLeave += 1;
        }

        if (RegAZ.test(str)) {
            PwdLeave += 1;
        }

        if (RegNot.test(str)) {
            PwdLeave += 2;
        }
        
        if (str.length >= 6)
            PwdLeave += 1;

        if (str.length >= 10)
            PwdLeave += 1;

        if (str.length >= 12)
            PwdLeave += 1;

        if (str.length >= 15)
            PwdLeave += 1;

        if (PwdLeave > 3) {
            $("#img_reg_pw").attr("src", "/images/reg_pw02.gif");
        }

        if (PwdLeave > 6) {
            $("#img_reg_pw").attr("src", "/images/reg_pw03.gif");
        }

        $("#img_reg_pw").show();
    }
   
    function refreshVerify() {
        $("#CheckCode").attr("src", "regcode.aspx?rnd=" + Math.random());
    }

    function StringLength(str) {
        return str.replace(/[^\x00-\xff]/g, "**").length
    }

    function isPwdSame() {
        var passwordOne = $("#TBPwdOne").val();
        var passwordTwo = $("#TBPwdTwo").val();
        if (passwordOne == passwordTwo) {
            return true;
        } else {
            return false;
        }
    }

    function checkReg() {
        var IsAgree = $("#CKBAgree").checked;
        if (!isRegisterUserName() | !isRegisterPwd1() | !isRegisterPwd2()) {
            return false;
        }
        
        if (IsAgree) {
            alert("必须同意注册协议才能注册");
            return false;
        }
        return true;
    }
</script>

