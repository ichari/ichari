var orderby = "";
var num = "";
var Condition = "";
var State = "";
var PlayTypeID = "";
var Name = "";

$().ready(function() {
    InitData(0);

    $("#EachPageNum a").click(function() {
        $("#EachPageNum").children("a").removeClass("current");
        $(this).addClass("current");
        num = $(this).html();
        InitData(0);
    });

    $("#govalue").keyup(function() {
        $(this).val($(this).val().replace(/[^\d]/g, ''));

        if (parseInt($(this).val()) > parseInt($("#last").val())) {
            $(this).val($("#last").val());
        }

        if (parseInt($(this).val()) <= 0) {
            $(this).val(1);
        }
    });

    $("#govalue").keydown(function() {
        if (event.keyCode == 13) { InitData(parseInt($("#govalue").val()) - 1); return false; }
    });

    $("#Btn_Go").click(function() {
        if (isNaN($("#govalue").val())) {
            return false;
        }
        InitData(parseInt($("#govalue").val()) - 1);
    });
});

function pageselectCallback(page_id, jq) {
    InitData(page_id);
}

function InitData(pageindx) {
    var tbody = "";
    var LotteryID = $("#hidLotteryID").val();
    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'SchemeList.ashx', //目标地址
        data: "p=" + (pageindx + 1) + "&orderby=" + orderby + "&EachPageNum=" + num + Condition + "&tbUserName=" + $("#tbUserName").val() + "&tbYser=" + $("#tbYser").val(),
        beforeSend: function() { $("#divload").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function() { $("#divload").hide(); $("#SchemeList").show() }, //接收数据完毕
        success: function(json) {
            $("#SchemeList tr:gt(0)").remove();

            $("#SchemeList").append(json.Content);
            $("#Pagination").html(json.page);

            $("#SchemeList tr:gt(0):odd").attr("class", "sk1");
            $("#SchemeList tr:gt(0):even").attr("class", "sk2");

        }
    });
}

function Sort(ordercolumn, ordertipid) {
    var ordertype = ""; //1:desc,0:asc
    var $orderimg = $("#" + ordertipid);
    if ($orderimg.html() != "") {
        var imgsrc = $("img", $orderimg).attr("src");

        if (imgsrc.indexOf("asc") > -1) {
            $(".ordertip").empty();
            $orderimg.html("&nbsp;<img src=\"Images/sort_desc.gif\" align=\"absmiddle\">");

            ordertype = 1;
        }
        else {
            $(".ordertip").empty();
            $orderimg.html("&nbsp;<img src=\"Images/sort_asc.gif\" align=\"absmiddle\">");

            ordertype = 0;
        }
    }
    else {
        $(".ordertip").empty();
        $orderimg.html("&nbsp;<img src=\"Images/sort_desc.gif\" align=\"absmiddle\">");
        ordertype = 1;
    }

    orderby = ordercolumn + "_" + ordertype;

    InitData(0);
}

function checkTime() {
    
    var date = new Date();
    var time = date.getFullYear() + "-" + add_zero(date.getMonth() + 1) + "-" + add_zero(date.getDate());
    var reg = /^(\d{4})-(\d{2})-(\d{2})$/;
    var val = document.getElementById("tbYser").value;
    var rs = val.match(reg);
    if (rs != null) {
        if (rs[1] < 1900 || rs[1] > 3000 || rs[2] < 0 || rs[2] > 12 || rs[3] < 0 || rs[3] > 31) {
            msg("日期格式不正确，请确认再查询");
            document.getElementById("tbYser").value = time;
        }
    } else {
        msg("日期格式不正确，请确认再查询");
        document.getElementById("tbYser").value = time;
    }
}

function add_zero(temp) {
    if (temp < 10) return "0" + temp;
    else return temp;
}