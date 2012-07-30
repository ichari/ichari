var Condition = "";
var State = "";
var PlayTypeID = "";
var Name = "";

$().ready(function() {
    $("#myTab li").click(function() {
        $("#myTab").children("li").removeClass("active");
        $(this).addClass("active");
        PlayTypeID = "";
        if (!isNaN($(this).attr("mid"))) {
            PlayTypeID = "&PlayTypeID=" + $(this).attr("mid");
        }
        Condition = State + PlayTypeID + Name;

        InitData(0);
    });

    $("#srearch").click(function() {
        Name = "";
        if (($("#SerachCondition").val() != "" && $("#SerachCondition").val() != "请输入用户名")) {
            Name = "&Name=" + $("#SerachCondition").val();
        }
        Condition = State + PlayTypeID + Name;

        InitData(0);
    });

    $(".f_hot a").click(function() {
        Name = "";
        Name = "&Name=" + $(this).html();
        Condition = State + PlayTypeID + Name;
        InitData(0);
    });

    $("#state_term").change(function() {
        State = "";

        if ($(this).val() != "") {
            State = "&State=" + $(this).val();
        }

        Condition = State + PlayTypeID + Name;
        InitData(0);
    });

    $(".f_class span").each(function() {
        var lotId = parseInt($(this).attr("mid"));
        if (lotId == parseInt($("#hidLotteryID").val())) {
            $(this).addClass("curr");
        }
        else {
            $(this).removeClass("curr");
        }
    });

    $("#cpTab0 li").each(function(i) {//check事件
        this.onclick = function() {
            $(this).addClass("active").parent().end().siblings().removeClass("active");
            InitData($(this).attr("mid"));
        } 
    });
});

function join(SchemeID, Share, ShareMoney, LotteryID) {
    if (!confirm("您确认要购买 " + Share + "份，需要支付 " + ShareMoney + " 元?"))
        return;
    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'Join.ashx', //目标地址
        data: "SchemeID=" + SchemeID + "&BuyShare=" + Share + "&ShareMoney=" + ShareMoney + "&LotteryID=" + LotteryID,
        beforeSend: function() { $("#divload").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function() { $("#divload").hide(); $("#SchemeList").show() }, //接收数据完毕
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
