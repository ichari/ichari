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
        url: 'ProjectList.ashx', //目标地址
        data: "p=" + (pageindx + 1) + "&LotteryID=" + LotteryID + "&orderby=" + orderby + "&EachPageNum=" + num + Condition,
        beforeSend: function() { $("#divload").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function() { $("#divload").hide(); $("#SchemeList").show() }, //接收数据完毕
        success: function(json) {
            $("#SchemeList tr:gt(0)").remove();
            try {
                $("#SchemeList").append(json[0][0]["Content"]);
                $("#Pagination").html(json[1][0]["page"]);
            } catch (e) { };

            $("#SchemeList tr:gt(0):odd").attr("class", "");
            $("#SchemeList tr:gt(0):even").attr("class", "th_even");

            $("#SchemeList tr:gt(0)").hover(function() {
                $(this).addClass('th_on');
            }, function() {
                $(this).removeClass('th_on');
            });

            $(".Share").keyup(function() {
                if (/\D/.test(this.value))
                    this.value = parseInt(this.value) || 1;
            });

            $(".join img").click(function() {
                join($(this).attr("mid"), $(this).parent().parent().prev().children().val(), $(this).parent().parent().prev().prev().prev().prev().text().replace(/[^\d]/g, ''), LotteryID);
            });

            $(".Share").blur(function() {
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