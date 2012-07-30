
BindUserName();
BindPwd();

var _stateName = false;
var _statePwd = false;

$("#tbFormUserName").focus();


$(document).keydown(function(event) {
    if (event.keyCode == 13) {
        $("#btnOk").click();
    }
});

$("#btnOk").click(function() {
    var userName = $("#tbFormUserName").val();
    if (userName == "") {
        $(".tip").html("用户名不能为空");
        $("#tbFormUserName").val("");
        $("#tbFormUserName").focus();
        $("#tbFormUserName").focus(function() { });
        $("#tbFormUserName").attr("class", "input_red");
        $("#tbFormUserName").unbind();
        _stateName = true;
        $("#tbFormUserName").bind("change", changeValue);
        return;
    } else {
        $(".tip").html("");
    }

    var pwd = $("#tbFormUserPassword").val();
    if (pwd == "") {
        $(".tip").html("密码不能为空");
        $("#tbFormUserPassword").val("");
        $("#tbFormUserPassword").focus();
        $("#tbFormUserPassword").focus(function() { });
        $("#tbFormUserPassword").attr("class", "input_red");
        $("#tbFormUserPassword").unbind();
        _statePwd = true;
        $("#tbFormUserPassword").bind("change", changeValue);
        return;
    } else {
        $(".tip").html("");
    }

    $.ajax({
        type: "POST",
        url: "/ajax/UserLogin.ashx",
        data: "UserName=" + userName + "&PassWord=" + pwd,
        timeout: 20000,
        cache: false,
        async: false,
        dataType: "json",
        error: callErrorloginbtn,
        success: callSuccessloginbtn
    });
});

callSuccessloginbtn = function(json, textStatus) {
    if (isNaN(json.message)) {
        $(".tip").html(json.message);
        if (json.message == "用户不存在") {
            $("#tbFormUserName").focus();
            $("#tbFormUserName").focus(function() { });
            $("#tbFormUserName").attr("class", "input_red");
            $("#tbFormUserName").unbind();
            _stateName = true;
            $("#tbFormUserName").bind("change", changeValue);
        } else if (json.message == "密码错误") {
            $("#tbFormUserPassword").focus();
            $("#tbFormUserPassword").focus(function() { });
            $("#tbFormUserPassword").attr("class", "input_red");
            $("#tbFormUserPassword").unbind();
            _statePwd = true;
            $("#tbFormUserPassword").bind("change", changeValue);
        } else {
            $("#tbFormUserName").focus();
            $("#tbFormUserName").attr("class", "input_red");
            $("#tbFormUserName").unbind();
            _stateName = true;
            $("#tbFormUserName").bind("change", changeValue);

            $("#tbFormUserPassword").focus();
            $("#tbFormUserPassword").attr("class", "input_red");
            $("#tbFormUserPassword").unbind();
            _statePwd = true;
            $("#tbFormUserPassword").bind("change", changeValue);
        }
        return;
    }
    if (parseInt(json.message) < 1) {
        $(".tip").html("登录异常，请重试一次，谢谢。可能是网络延时原因。");
        _statePwd = false;
        _stateUserName = false;
        return;
    }
    $(".tip").html("");
    location.href = $("#hid_fromUrl").val();
}

callErrorloginbtn = function() {
    $(".tip").html("登录异常，请重试一次，谢谢。可能是网络延时原因。");
}

function BindUserName() {
    //--- 用户名
    $("#tbFormUserName").focus(function() {
        this.className = "input_move";
    });
    $("#tbFormUserName").blur(function() {
    this.className = "input_on";
    });

    $("#tbFormUserName").mousemove(function() {
        this.className = "input_move"
    });
    $("#tbFormUserName").mouseout(function() {
    this.className = "input_on";
    });
}

function BindPwd() {
    //--- 密码
    $("#tbFormUserPassword").focus(function() {
        this.className = "input_move";
    });
    $("#tbFormUserPassword").blur(function() {
    this.className = "input_on";
    });

    $("#tbFormUserPassword").mousemove(function() {
        this.className = "input_move"
    });
    $("#tbFormUserPassword").mouseout(function() {
    this.className = "input_on";
    });
}

function changeValue() {
    if (_stateName) {
        BindUserName();
        _stateName = false;
    }
    if (_statePwd) {
        BindPwd();
        _statePwd = false;
    }
}