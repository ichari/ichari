$().ready(function () {
    $.ajax({
        type: "POST",
        url: "/ajax/UserLogin.aspx",
        timeout: 20000,
        cache: false,
        async: false,
        dataType: "json",
        success: function (json) {
            if (isNaN(json.message) || (parseInt(json.message) < 1)) { return; }
            if (json.ismanager == "True") { $("#user_login_manger").show(); }
            else { $("#user_login_manger").hide(); }
            $("#DivUserinfo input").val(json.message);
            $("#topUserName").text(json.name);
            $("#topUserBala").val(json.Balance);
            $("#topUserDate").val(json.Date);
            $("#topUserTime").val(json.Time);
            if ($("#uInfo") != null || $("#uInfo") != undefined) {
                $("#uLogin").hide();
                $("#uInfo").show();
                $("#uContName").text($("#topUserName").text());
                $("#uContBala").text($("#topUserBala").val());
                $("#uContDate").text($("#topUserDate").val());
                $("#uContTime").text($("#topUserTime").val());
            };
        },
        error: function () { return; }
    });

    if (parseInt($("#DivUserinfo input").val()) > 0) {
        $("#r_login_name").text($("#topUserName").text());
        $("#r_right_dl_q").hide();
        $("#r_right_dl_h").show();
        $("#lbefore").hide();
        $("#lafter").show();
        //user content on default page
        //        $("#uLogin").hide();
        //        $("#uInfo").show();
        //        $("#uContName").text($("#topUserName").text());
        //        $("#uContBala").text($("#topUserBala").val());
        //        $("#uContType").text($("#topUserTime").val());
    }
    else {
        $("#r_right_dl_q").show();
        $("#r_right_dl_h").hide();
        $("#lbefore").show();
        $("#lafter").hide();
        $("#uLogin").show();
        $("#uInfo").hide();
    }
});

$().ready(function () {
    $('#' + $("#WebHead1_currentMenu").val()).addClass("current").siblings().removeClass("current");
    $('#Image93,#top_Gmcpcon').bind({
        mouseover: function() {
            $("#top_Gmcpcon").show();
            $("#Image93").attr("src", "/Home/Room/Images/menudh2.jpg");
        },
        mouseout: function() {
            $("#top_Gmcpcon").hide();
            $("#Image93").attr("src", "/Home/Room/Images/menudh.jpg");
        }
    });

    $('#lu, #lp').bind({
        mouseover: function() {
            $(this).addClass("text_on");
        },
        mouseout: function() {
            $(this).removeClass("text_on");
        },
        focus: function() {
            $(this).addClass("text_on");
        },
        blur: function() {
            $(this).removeClass("text_on");
        }
    });

    $("#yzmup").click(function() {
        $("#yzmimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    });

    $("#yzmimg").click(function() {
        $("#yzmimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    });

    $("#topLoginOut").click(function() {
        $.ajax({
            type: "POST",
            url: "/ajax/UserLogin.aspx",
            data: "action=loginout",
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            error: callErrortopLoginOut,
            success: callSuccesstopLoginOut
        });
    });

    function callSuccesstopLoginOut(json, textStatus) {
        if (isNaN(json.message)) {
            msg(json.message);

            return;
        }
        window.location.href = "/";
        $("#DivUserinfo input").val(json.message);
        $("#user_login_manger").hide();
        $("#r_right_dl_q").show();
        $("#r_right_dl_h").hide();
        $("#lbefore").show();
        $("#lafter").hide();
    }

    function callErrortopLoginOut() {
        msg("登录异常，请重试一次，谢谢。可能是网络延时原因!");
    }

    $("#floginbtn").click(function() {
        $("#error_tips").show();
        if ($("#lu").val() == "") {
            $("#error_tips").html("请输入合法的用户名!");
            $("#lu").focus();
        } else if ($("#lp").val() == "") {
            $("#error_tips").html("请输入合法的密码!");
            $("#lp").focus();
        } else if ($("#yzmtext").val() == "") {
            $("#error_tips").html("请输入正确的验证码!");
            $("#yzmtext").focus();
        } else {
            $("#error_tips").hide();

            $.ajax({
                type: "POST",
                url: "/ajax/UserLogin.aspx",
                data: "UserName=" + $("#lu").val() + "&PassWord=" + $("#lp").val() + "&RegCode=" + $("#yzmtext").val(),
                timeout: 20000,
                cache: false,
                async: false,
                dataType: "json",
                error: callErrorfloginbtn,
                success: callSuccessfloginbtn
            });
        }
    });

    function callSuccessfloginbtn(json, textStatus) {
        $("#lp").val('');
        $("#error_tips").hide();
        if (isNaN(json.message)) { $("#error_tips").text(json.message); $("#error_tips").show(); return; }
        if (parseInt(json.message) < 1) { return; }
        if (json.ismanager == "True") { $("#user_login_manger").show(); }
        else { $("#user_login_manger").hide(); }

        $("#DivUserinfo input").val(json.message);
        $("#topUserName").text(json.name);
        $("#r_login_name").text($("#topUserName").text());
        $("#r_right_dl_q").hide();
        $("#r_right_dl_h").show();
        $("#lbefore").hide();
        $("#lafter").show();
        tb_remove();
        if (window.evalscript != "") { eval(window.evalscript); } else { location.href = location.href; }
    }

    function callErrorfloginbtn() {
        msg("登录异常，请重试一次，谢谢。可能是网络延时原因!");
    }

    $("#info_dlg_ok").click(function () {
        tb_remove();
    });
})

function CreateLogin(script) {
    if (parseInt($("#DivUserinfo input").val()) > 0) {
        eval(script);
        return true;
    }

    $("#yzmimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    tb_show("用户登录", "#TB_inline?width=360&amp;inlineId=loginLay", "");

    $("#floginbtn").focus();
    
    window.evalscript = script;

    return false;
}

$.fn.numeral = function() {
    $(this).css("ime-mode", "disabled");
    this.bind("keypress", function(event) {
        var keyCode = 0;
        if (event.charCode != undefined) {
            keyCode = event.charCode;
        } else {
            keyCode = event.keyCode;
        }

        if (keyCode == 46) {
            if (this.value.indexOf(".") != -1) {
                return false;
            }
        } else {
            return (keyCode >= 46 && keyCode <= 57) || keyCode == 0;
        }
    });
    this.bind("blur", function() {
        if (this.value.lastIndexOf(".") == (this.value.length - 1)) {
            this.value = this.value.substr(0, this.value.length - 1);
        } else if (isNaN(this.value)) {
            this.value = "";
        }
    });
    this.bind("paste", function() {
        var s = clipboardData.getData('text');
        if (!/\D/.test(s));
        value = s.replace(/^0*/, '');
        return false;
    });
    this.bind("dragenter", function() {
        return false;
    });
    this.bind("keyup", function() {
        if (/(^0+)/.test(this.value)) this.value = this.value.replace(/^0*/, '');
    });
};

msg = function(message) {
    $("#info_dlg_content").html(message);
    tb_show("温馨提示", "#TB_inline?width=470&amp;inlineId=info_dlg", "");
    $("#info_dlg_ok").focus();
}

msg = function(message, isreload) {
    $("#info_dlg_content").html(message);
    tb_show("温馨提示", "#TB_inline?width=470&amp;inlineId=info_dlg", "", isreload);
    $("#info_dlg_ok").focus();
}

confirmTip = function(message, fn) {
    $("#confirm_dlg_content").html(message);
    tb_show("温馨提示", "#TB_inline?width=470&amp;inlineId=confirm_dlg", "");
    $("#confirm_dlg_yes").focus();
}

// /**/   scorll
if ($("#scro_tops").length > 0) {
    $("#scro_tops").innerfade({
        animationtype: "slide",
        speed: 1750,
        timeout: 2000,
        type: "random",
        containerheight: "28px"
    });
}