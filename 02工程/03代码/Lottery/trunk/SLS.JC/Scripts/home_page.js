$(document).ready(function () {
    var SDmodel = new scrollDoor();
    SDmodel.sd(["b_tab01", "b_tab02"], ["b_tabC01", "b_tabC02"], "c", "", "mouseover");
    SDmodel.sd(["b_tab11", "b_tab12", "b_tab13", "b_tab14"], ["b_tabC11", "b_tabC12", "b_tabC13", "b_tabC14"], "c", "", "click");

    //    $("#uLoginPn").dialog({
    //        width: 410,
    //        height: 335,
    //        modal: true,
    //        title: "",
    //        autoOpen: false
    //    });

    //    $("#uContLogin").click(function () {
    //        $("#uLoginPn").dialog("open");
    //    });

    //    $("#r_login_btn").click(function () {
    //        $("#r_login_tip").show();
    //        if ($("#r_login_u").val() == "") {
    //            $("#r_login_tip").html("请输入合法的用户名!");
    //            $("#r_login_u").focus();
    //            $("#r_login_tip").css({ "top": "-30px" });
    //        } else if ($("#r_login_p").val() == "") {
    //            $("#r_login_tip").html("请输入合法的密码!");
    //            $("#r_login_p").focus();
    //            $("#r_login_tip").css({ "top": "0px" });
    //        } else {
    //            $("#r_login_tip").hide();

    //            $.ajax({
    //                type: "POST",
    //                url: "/ajax/UserLogin.ashx",
    //                data: "UserName=" + $("#r_login_u").val() + "&PassWord=" + $("#r_login_p").val() + "&RegCode=", 
    //                timeout: 20000,
    //                cache: false,
    //                async: false,
    //                dataType: "json",
    //                success: callSuccesslogin_btn,
    //                error: callErrorlogin_btn
    //            });
    //        }
    //    });

    $("#gbTab > span").each(function () {//check事件
        this.onclick = function () {
            $(this).addClass("c").parent().end().siblings().removeClass("c");
            InitData($(this).attr("mid"));
        }
    });

    InitData(5);
});

function InitData(LotteryID) {
    var tbody = "";

    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'Join/SchemeList.ashx', //目标地址
        data: "LotteryID=" + LotteryID + "&TopNum=5",
        beforeSend: function () { $("#gbLoad").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function () { $("#gbLoad").hide(); $("#SchemeList").show(); }, //接收数据完毕
        success: function (json) {
            $("#SchemeList tr:gt(0)").remove();
            try {
                $("#SchemeList").append(json[0][0]["Content"]);
            } catch (e) { };

            $("#SchemeList tr:gt(0):odd").attr("class", "");
            $("#SchemeList tr:gt(0):even").attr("class", "th_even");
            $("#SchemeList tr:gt(0)").hover(function () {
                $(this).addClass('th_on');
            }, function () {
                $(this).removeClass('th_on');
            });
            $(".Share").keyup(function () {
                if (/\D/.test(this.value))
                    this.value = parseInt(this.value) || 1;
            });
            $(".join img").click(function () {
                if (!CreateLogin(this)) {
                    return;
                }
                join($(this).attr("mid"), $(this).parent().parent().prev().children().val(), $(this).parent().parent().prev().prev().prev().prev().text().replace(/[^\d]/g, ''), LotteryID);
            });
            $(".Share").blur(function () {
                $(this).val($(this).val().replace(/[^\d]/g, ''));
                if (parseInt($(this).val()) > parseInt($(this).parent().prev().children().html())) {
                    $(this).val($(this).parent().prev().children().html());
                }
                if (parseInt($(this).val()) <= 0) {
                    $(this).val(1);
                }
            });
        }
    });
}

//$.ajax({
//    type: "POST",
//    url: "/ajax/UserLogin.ashx",
//    timeout: 20000,
//    cache: false,
//    async: false,
//    dataType: "json",
//    error: function () {
//        msg("登录异常，请重试一次，谢谢。可能是网络延时原因。");
//        return;
//    },
//    success: function (json) {
//        if (isNaN(json.message)) {
//            return;
//        }

//        if (parseInt(json.message) < 1) {
//            return;
//        }

//        $("#DivUserinfo input").val(json.message);
//        $("#topUserName").text(json.name);

//        if (json.ismanager == "True") {
//            $("#r_login_manger").show();
//        }
//        else {
//            $("#r_login_manger").hide();
//        }
//    }
//});

//if (parseInt($("#DivUserinfo input").val()) > 0) {
//    $("#r_login_name").text($("#topUserName").text());
//    $("#lbefore").hide();
//    $("#lafter").show();

//    $("#uLogin").hide();
//    $("#uInfo").show();
//    $("#uContName").text($("#topUserName").text());
//    $("#uContBala").text($("#topUserBala").val());
//    $("#uContType").text($("#topUserType").val());
//}
//else {
//    $("#lbefore").show();
//    $("#lafter").hide();

//    $("#uLogin").show();
//    $("#uInfo").hide();
//}

//function callSuccesslogin_btn(json) {
//    $("#r_login_u").val('');
//    $("#r_login_p").val('');
//    if (isNaN(json.message)) {
//        msg(json.message);
//        return;
//    }

//    if (parseInt(json.message) < 1) {
//        return;
//    }

//    if (json.ismanager == "True") {
//        $("#r_login_manger").show();
//        $("#user_login_manger").show();
//    }
//    else {
//        $("#r_login_manger").hide();
//        $("#user_login_manger").hide();
//    }

//    $("#DivUserinfo input").val(json.message);
//    $("#r_login_name").text(json.name);
//    $("#topUserName").text(json.name)

//    $("#lbefore").hide();
//    $("#lafter").show();
//    $("#uLogin").hide();
//    $("#uInfo").show();
//    
//    $("#uContName").text(json.name);
//    $("#uContBala").text(json.Balance);
//    $("#uContType").text($("#topUserType").val());
//    $("#uLoginPn").dialog("close");
//}

//function callErrorlogin_btn(a, b, c) {
//    msg("登录异常，请重试一次，谢谢。可能是网络延时原因。");
//}

function join(SchemeID, Share, ShareMoney, LotteryID) {
    if (!confirm("您确认要购买 " + Share + "份，需要支付 " + ShareMoney + " 元?"))
        return;
    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'Join/Join.ashx', //目标地址
        data: "SchemeID=" + SchemeID + "&BuyShare=" + Share + "&ShareMoney=" + ShareMoney + "&LotteryID=" + LotteryID,
        beforeSend: function () { $("#divload").show(); $("#Pagination").hide(); }, //发送数据之前
        complete: function () { $("#divload").hide(); $("#Pagination").show(); }, //接收数据完毕
        success: function (json) {
            msg(json.message);
            if ($("#hidLotteryID").val()) {
                InitData(0);
            }
            else {
                InitData(LotteryID);
            }
        }
    });
}
