window.$ = jQuery;
var boxCount = 0;

$(document).ready(function () {
    var SDmodel = new scrollDoor();
    SDmodel.sd(["b_tab01", "b_tab02"], ["b_tabC01", "b_tabC02"], "c", "", "mouseover");
    SDmodel.sd(["b_tab11", "b_tab12", "b_tab13", "b_tab14"], ["b_tabC11", "b_tabC12", "b_tabC13", "b_tabC14"], "c", "", "click");
    $("#uLoginPn").dialog({
        width: 410,
        height: 335,
        modal: true,
        title: "",
        autoOpen: false
    });
    $("#uContLogin").click(function () {
        $("#uLoginPn").dialog("open");
    });

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

$.ajax({
    type: "POST",
    url: "/ajax/UserLogin.ashx",
    timeout: 20000,
    cache: false,
    async: false,
    dataType: "json",
    error: function() {
        msg("登录异常，请重试一次，谢谢。可能是网络延时原因。");
        return;
    },
    success: function(json) {
        if (isNaN(json.message)) {
            return;
        }

        if (parseInt(json.message) < 1) {
            return;
        }

        $("#DivUserinfo input").val(json.message);
        $("#topUserName").text(json.name);

        if (json.ismanager == "True") {
            $("#r_login_manger").show();
        }
        else {
            $("#r_login_manger").hide();
        }
    }
});

if (parseInt($("#DivUserinfo input").val()) > 0) {
    $("#r_login_name").text($("#topUserName").text());
    $("#r_right_dl_q").hide();
    $("#r_right_dl_h").show();
    $("#lbefore").hide();
    $("#lafter").show();
}
else {
    $("#r_right_dl_q").show();
    $("#r_right_dl_h").hide();
    $("#lbefore").show();
    $("#lafter").hide();
}

//$().ready(function() {
//    $("#r_login_crefresh").click(function() {
//        $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
//    });

//    $("#r_login_cimg").click(function() {
//        $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
//    });

//    $("#kaijiang li").mouseover(function() {
//        $(this).addClass("cur").siblings().removeClass("cur");

//        if (!isNaN($(this).attr("mid"))) {
//            $("#scroll_con").children("div").hide().eq(parseInt($(this).attr("mid"))).show();
//            boxCount = parseInt($(this).attr("mid"));
//        }
//    });

//    $("#gonggao li").mouseover(function() {
//        $(this).addClass("cur").siblings().removeClass("cur");

//        if (!isNaN($(this).attr("mid"))) {
//            $("#wzgg_con").children("div").hide().eq(parseInt($(this).attr("mid"))).show();
//        }
//    });

//    $("#cpTab0 li").each(function(i) {//check事件
//        this.onclick = function() {
//            $(this).addClass("active").parent().end().siblings().removeClass("active");
//            InitData($(this).attr("mid"));
//        }
//    });

//    $(".anyClass").jCarouselLite({
//        btnNext: ".next",
//        btnPrev: ".prev",
//        visible: 2
//    });

//    $("img", "#slide_div").each(function(index, domEle) {
//        $(domEle).attr("src", $(domEle).attr("url"));
//    });

//    $("#Image93").unbind("mouseover");
//    $("#Image93").unbind("mouseout");
//    $("#top_Gmcpcon").unbind("mouseover");
//    $("#top_Gmcpcon").unbind("mouseout");
//})

$("#r_login_btn").click(function() {
    $("#r_login_tip").show();
    if ($("#r_login_u").val() == "") {
        $("#r_login_tip").html("请输入合法的用户名!");
        $("#r_login_u").focus();
        $("#r_login_tip").css({ "top": "-30px" });
    } else if ($("#r_login_p").val() == "") {
        $("#r_login_tip").html("请输入合法的密码!");
        $("#r_login_p").focus();
        $("#r_login_tip").css({ "top": "0px" });
    } else if ($("#r_login_c").val() == "") {
        $("#r_login_tip").html("请输入正确的验证码!");
        $("#r_login_c").focus();
        $("#r_login_tip").css({ "top": "25px" });
    } else {
        $("#r_login_tip").hide();

        $.ajax({
            type: "POST",
            url: "/ajax/UserLogin.ashx",
            data: "UserName=" + $("#r_login_u").val() + "&PassWord=" + $("#r_login_p").val() + "&RegCode=" + $("#r_login_c").val(),
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            success: callSuccesslogin_btn,
            error: callErrorlogin_btn
        });
    }
});

function callSuccesslogin_btn(json) {
    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    $("#r_login_p").val('');
    if (isNaN(json.message)) {
        msg(json.message);
        return;
    }

    if (parseInt(json.message) < 1) {
        return;
    }

    if (json.ismanager == "True") {
        $("#r_login_manger").show();
        $("#user_login_manger").show();
    }
    else {
        $("#r_login_manger").hide();
        $("#user_login_manger").hide();
    }

    $("#DivUserinfo input").val(json.message);
    $("#r_login_name").text(json.name);
    $("#topUserName").text(json.name)
    $("#r_right_dl_q").hide();
    $("#r_right_dl_h").show();
    $("#lbefore").hide();
    $("#lafter").show();
}

function callErrorlogin_btn(a, b, c) {
    msg("登录异常，请重试一次，谢谢。可能是网络延时原因。");
}

$("#r_login_close").click(function() {
    $.ajax({
        type: "POST",
        url: "/ajax/UserLogin.ashx",
        data: "action=loginout",
        timeout: 20000,
        cache: false,
        async: false,
        dataType: "json",
        error: callErrorlogin,
        success: callSuccesslogin
    });
});

function callSuccesslogin(json, textStatus) {
    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    if (isNaN(json.message)) {
        msg(json.message);

        return;
    }
    // 清空
    $("#r_login_u").val("");
    $("#r_login_p").val("");
    $("#r_login_c").val("");

    $("#DivUserinfo input").val(json.message);
    $("#r_right_dl_q").show();
    $("#r_right_dl_h").hide();
    $("#r_login_manger").hide();
    $("#lbefore").show();
    $("#lafter").hide();
}

function callErrorlogin() {
    msg("登录异常，请重试一次，谢谢。可能是网络延时原因。");
}

function join(SchemeID, Share, ShareMoney, LotteryID) {
    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'Join/Join.ashx', //目标地址
        data: "SchemeID=" + SchemeID + "&BuyShare=" + Share + "&ShareMoney=" + ShareMoney + "&LotteryID=" + LotteryID,
        beforeSend: function() { $("#divload").show(); $("#Pagination").hide(); }, //发送数据之前
        complete: function() { $("#divload").hide(); $("#Pagination").show() }, //接收数据完毕
        success: function(json) {
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

var imgcounts = $("#imgCount").val();

$.fn.hideFocus = function() {
    $(this).focus(function() { this.blur() })
};

(function(m, M, w, o, T, h, ul, bt, a, t, d) {
    $(function(p, n) {
        ul = $('#slide_div ul');
        bt = $('#slide_btn .btn02');
        li = $('li', ul);
        a = $('a', ul);
        t = $('#slide_a');
        var g = $("img", a);
        g.attr('src', g.attr('url'));
        p = bt.last().next();
        n = p.next();
        for (i = 1; i < M; i++) {
            li.eq(i).remove()
        }
        d = auto();
        p.click(prev);
        n.click(next);
        $('a', p).hideFocus();
        $('a', n).hideFocus();
        bt.click(pos)
    });
    function auto() {
        return setTimeout(next, o)
    }
    function pos(n, t) {
        if (h) {
            n = m; m = $(this).attr('target') - 0;
            if (m != n) {
                set(m - n)
            }
        }
    }
    function prev() {
        if (h) {
            m--;
            m = m < 0 ? M - 1 : m;
            set(-1)
        }
    }
    function next() {
        if (h) {
            m++;
            m = m >= M ? 0 : m;
            set(1)
        }
    }
    function set(b, e) {
        h = 0; clearTimeout(d);
        ul.prepend(li.eq(m));
        b = ul.find('li');
        b.eq(1).fadeOut(T);
        b.eq(0).fadeIn(T, function() { b.eq(1).remove(); h = 1; d = auto() });
        bt.removeClass('btn01');
        bt.eq(m).addClass('btn01');
        e = a.eq(m);
        t.attr('href', e.attr('href'));
        t.html(e.attr('txt'));
        var g = $("img", e);
        g.attr('src', g.attr('url'));
    }
})(0, imgcounts, 450, 1000 * imgcounts, '', 1);

var myTimer;
var speed = 100; //速度毫秒 值越小速度越快
var stepSpeed = 4; //值越大越快
$(function() {
    var mybox = $(".scroll_box");
    //向上
    $(".scroll_up").bind("mouseover", function() {
        var nowPos = mybox[boxCount].scrollTop; //当前值
        changePos(mybox, nowPos, 0);
    }).bind("mouseout", function() {
        if (myTimer) { window.clearInterval(myTimer); }
    });
    //向下
    $(".scroll_down").bind("mouseover", function() {
        var nowPos = mybox[boxCount].scrollTop; //当前值
        var maxPos = mybox[boxCount].scrollHeight - mybox.outerHeight(); //最大值
        changePos(mybox, nowPos, maxPos);
    }).bind("mouseout", function() {
        if (myTimer) { window.clearInterval(myTimer); }
    });
});

function changePos(box, from, to) {
    if (myTimer) { window.clearInterval(myTimer); }
    var temStepSpeed = stepSpeed;
    if (from > to) {
        myTimer = window.setInterval(function() {
            if (box[boxCount].scrollTop > to) { box[boxCount].scrollTop -= (5 + temStepSpeed); temStepSpeed += temStepSpeed; }
            else { window.clearInterval(myTimer); }
        }, speed);
    } else if (from < to) {
        myTimer = window.setInterval(function() {
            if (box[boxCount].scrollTop < to) { box[boxCount].scrollTop += (5 + temStepSpeed); temStepSpeed += temStepSpeed; }
            else { window.clearInterval(myTimer); }
        }, speed);
    }
}

$(".butterL").click(function() {
    $(".JScon").scrollLeft(100);
});

$(".butterR").click(function() {
    $(".JScon").scrollLeft(-100);
});

//鼠标经过变色
$(function() {
    $("tr").hover(function() {
        $(this).addClass("th_on");
    }, function() {
        $(this).removeClass("th_on");
    });
});


var loadState = true;
$(window).bind("scroll", function(event) {
    var top = $(document).scrollTop();
    if (loadState) {
        if (parseInt(top) > 201) {
            // 加载合买方案
//            InitData(5);
            $("#frmmain").attr("src", "/Lottery/Quick_SFC.aspx");
            loadState = false;
        }
    }
});