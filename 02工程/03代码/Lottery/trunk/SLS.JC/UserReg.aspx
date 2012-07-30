<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserReg.aspx.cs" Inherits="UserReg" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员注册 - <%=_Site.Name %>－</title>
    <meta name="description" content="会员注册，<%=_Site.Name %> 彩票，双色球、时时乐、时时彩" />
    <meta name="keywords" content="会员注册，双色球开奖，双色球走势图，3d走势图，福彩3d，时时彩" />
    <link href="Style/register.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="favicon.ico" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <!--reg_both-->
    <div class="reg_both">
        <div class="reg_flow">
            <h2>
                购彩流程</h2>
            <dl>
                <img src="images/reg_flow.gif" width="615" height="28" alt="购彩流程" /></dl>
            <dt>已经是会员？请直接<a href="/UserLogin.aspx">登录</a></dt>
        </div>
        <div class="reg_main">
            <dl>
                <dt class="r_name">用户名：</dt>
                <dt class="r_txt">
                    <span><asp:TextBox ID="TBUserName" name="TBUserName" runat="server" class="regip" size="14" MaxLength="16"></asp:TextBox></span> 
                    <span id="TBUserName_tip" name="TBUserName_tip"></span>
                </dt>
            </dl>
            <dl>
                <dt class="r_name">登录密码：</dt>
                <dt class="r_txt">
                    <span><asp:TextBox ID="TBPwdOne" name="TBPwdOne" runat="server" TextMode="Password" class="regip" size="30" MaxLength="16"></asp:TextBox></span>
                    <span id="TBPwdOne_tip" name="TBPwdOne_tip"></span>
                </dt>
            </dl>
            <div class="r_name">
            </div>
            <div class="r_txt" style="display: none;" id="DivDengji">
                <span>弱</span>
                <img src="images/Star1.gif" width="17" height="17" />
                <img src="images/satr2.gif" width="17" height="17" />
                <img src="images/Star3.gif" width="17" height="17" />
                <img src="images/Star3.gif" width="17" height="17" />
                <img src="images/Star3.gif" width="17" height="17" />
            </div>
            <dl>
                <dt class="r_name">确认密码：</dt>
                <dt class="r_txt"><span>
                    <asp:TextBox ID="TBPwdTwo" name="TBPwdTwo" runat="server" TextMode="Password" class="regip"
                        size="30" MaxLength="16"></asp:TextBox></span><span id="TBPwdTwo_tip" name="TBPwdTwo_tip"></span></dt>
            </dl>
            <dl>
                <dt class="r_name">电子邮箱：</dt>
                <dt class="r_txt"><span>
                    <asp:TextBox ID="TBUserMail" name="TBUserMail" runat="server" class="regip" size="30"></asp:TextBox></span><span
                        id="TBUserMail_tip" name="TBUserMail_tip"></span></dt>
            </dl>
            <dl>
                <dt class="r_name">手机号码：</dt>
                <dt class="r_txt"><span>
                    <asp:TextBox ID="TBMobile" name="TBMobile" runat="server" class="regip" size="30"></asp:TextBox></span><span
                        id="TBMobile_tip" name="TBMobile_tip"></span></dt>
            </dl>
            <dl>
                <dt class="r_name">真实姓名：</dt>
                <dt class="r_txt"><span>
                    <asp:TextBox ID="tbRealityName" name="tbRealityName" runat="server" class="regip"
                        size="30" onblur="isUserRealityName($(this));"></asp:TextBox></span><span id="tbRealityName_tip"
                            name="tbRealityName_tip"></span></dt>
            </dl>
            <dl>
                <dt class="r_name">验证码：</dt>
                <dt class="r_txt"><span>
                    <asp:TextBox ID="tbRegCheckCode" name="tbRegCheckCode" runat="server" MaxLength="6"
                        class="reg_yzip" Width="70px" Height="25px"></asp:TextBox></span> <span class="r_yzm">
                            <img src="regcode.aspx" width="50" height="20" id="CheckCode" name="CheckCode" alt="验证码" /></span>
                    <span class="r_refresh" onclick="refreshVerify()">
                        <img src="images/refresh.gif" width="13" height="13" onclick="refreshVerify()" alt="刷新"
                            style="cursor: hand;" /></span> <span class="r_tip c_999">请输入下图中的字符，不区分大小写</span>
                </dt>
            </dl>
            <dl class="r_treaty">
                <span>
                    <asp:CheckBox ID="ckbAgree" runat="server" Checked="true" /></span> <span>同我已阅读并同意<a
                        href="Home/Web/UserRegAgree.aspx" style="color: Red;" target="_blank">《<%=_Site.Name %>用户协议》</a></span>
            </dl>
            <dl class="r_btn">
                <ShoveWebUI:ShoveConfirmButton ID="btnReg" Style="cursor: pointer;" runat="server"
                    Width="120px" Height="36px" CausesValidation="False" Text="提交注册" BorderStyle="None"
                    OnClick="btnReg_Click" OnClientClick="if(!checkReg()){return false;}" class="btn_reg" />
            </dl>
        </div>
        <div class="cl">
        </div>
    </div>
</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>
<asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
<asp:HiddenField ID="hidCity" runat="server" />
</form>
</body>
</html>
<script type="text/javascript" language="javascript">
    $().ready(function() {
        $.ajax({
            type: "POST",
            url: "/ajax/UserLogin.ashx",
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            error: callError,
            success: callSuccess
        });

        function callSuccess(json, textStatus) {
            if (isNaN(json.message)) {
                return;
            }

            if (parseInt(json.message) < 1) {
                return;
            }

            window.location.href = "/";

            $("#topUserName").text(json.name);
            $("#lafter").show();
            $("#lbefore").hide();
        }

        function callError() {
            alert("登录异常，请重试一次，谢谢。可能是网络延时原因。");
        }
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
        focus: function() {
            inPut("TBUserName", "4-16个字符，可以使用汉字、数字、字母");
        },
        blur: function() {
            isRegisterUserName();
        }
    })

    $("#TBPwdOne").bind({
        focus: function() {
            inPut("TBPwdOne", "6-15个字符，建议使用字母、数字、符号组合");
        },
        blur: function() {
            isRegisterPwd1();
        }
    })

    $("#TBPwdTwo").bind({
        focus: function() {
            inPut("TBPwdTwo", "请再次输入您以上填写的登录密码");
        },
        blur: function() {
            isRegisterPwd2();
        }
    })

    $("#TBUserMail").bind({
        focus: function() {
            inPut("TBUserMail", "如：12345@qq.com");
        },
        blur: function() {
            isUserMail();
        }
    })

    $("#TBMobile").bind({
        focus: function() {
            inPut("TBMobile", "为了您中奖后及时通知您，请如实填写");
        },
        blur: function() {
            isUserMobile();
        }
    })

    $("#tbRealityName").bind({
        focus: function() {
            inPut("tbRealityName", "非常重要，和您的提款相关，请务必填写");
        },
        blur: function() {
            isUserRealityName();
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
        $("#DivDengji").css("display", "Block");

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

    function isUserMail() {
        var patrn = /^(\w)+[-]*(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$/;
        if (!patrn.test($("#TBUserMail").val())) {
            showErr("TBUserMail", "对不起，您输入的邮箱格式不正确");
            return false;
        }
        showOK("TBUserMail");
        return true;
    }

    function isUserMobile() {
        var patrn = /^(1(3|4|5|8)[0-9]{9})$/;
        if (!patrn.test($("#TBMobile").val())) {
            showErr("TBMobile", "对不起，您输入的手机号码不正确");
            return false;
        }
        showOK("TBMobile");
        return true;
    }

    function isUserRealityName() {
        var NameLen = StringLength($("#tbRealityName").val());

        if (NameLen < 2) {
            showErr("tbRealityName", "对不起，真实姓名长度请不要少于2个字符");
            return false;
        }

        var patrn = /[^0-9A-Za-z\u4e00-\u9fa5]/;
        if (patrn.test($("#tbRealityName").val())) {
            showErr("tbRealityName", "对不起，请使用汉字，数字或字母");
            return false;
        }
        showOK("tbRealityName");
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
            result = json.message
        }

        function callError() {
            alert("检验失败，请重试一次，谢谢。可能是网络延时原因。");
        }

        if (Number(result) < 0) {
            if (Number(result) == -1) {
                showErr("TBUserName", "对不起用户名中含有禁止使用的字符");
                return false;
            }

            if (Number(result) == -2) {
                showErr("TBUserName", "用户名 " + $("#TBUserName").val() + " 已被占用，请重新输入一个");
                return false;
            }

            if (Number(result) == -3) {
                showErr("TBUserName", "用户名长度在 4-16 个英文字符或数字、中文 2-8 之间");
                return false;
            }
        }
        else {
            showOK("TBUserName");
        }
        return true;
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

        $("#DivDengji").html(ShowLeave(PwdLeave));
    }

    function ShowLeave(i) {
        var s0 = "<img src=\"Images/Star3.gif\" width=\"17\" height=\"17\" />";
        var s1 = "<img src=\"Images/satr2.gif\" width=\"17\" height=\"17\" />";
        var s2 = "<img src=\"Images/Star1.gif\" width=\"17\" height=\"17\" />";
        var sp1 = "<span>弱</span>";
        var sp2 = "<span>中</span>";
        var sp3 = "<span>强</span>";
        var s = "";

        switch (i) {
            case (0):
                s = sp1 + s0 + s0 + s0 + s0 + s0;
                break;
            case (1):
                s = sp1 + s1 + s0 + s0 + s0 + s0;
                break;
            case (2):
                s = sp1 + s2 + s0 + s0 + s0 + s0;
                break;
            case (3):
                s = sp1 + s2 + s1 + s0 + s0 + s0;
                break;
            case (4):
                s = sp2 + s2 + s2 + s0 + s0 + s0;
                break;
            case (5):
                s = sp2 + s2 + s2 + s1 + s0 + s0;
                break;
            case (6):
                s = sp2 + s2 + s2 + s2 + s0 + s0;
                break;
            case (7):
                s = sp3 + s2 + s2 + s2 + s1 + s0;
                break;
            case (8):
                s = sp3 + s2 + s2 + s2 + s2 + s0;
                break;
            case (9):
                s = sp3 + s2 + s2 + s2 + s2 + s1;
                break;
            case (10):
                s = sp3 + s2 + s2 + s2 + s2 + s2;
                break;
            default:
                break;
        }
        return s;
    }

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
        if (!isRegisterUserName() | !isRegisterPwd1() | !isRegisterPwd2() | !isUserMail() | !isUserMobile() | !isUserRealityName()) {
            return false;
        }

        if (IsAgree) {
            alert("必须同意注册协议才能注册");
            return false;
        }
        return true;
    }
</script>
<%--
<script type="text/javascript" src="http://fw.qq.com/ipaddress" charset="gb2312"></script>
<script type="text/javascript">
    $("#hidCity").val(IPData[3])
</script>
--%>
