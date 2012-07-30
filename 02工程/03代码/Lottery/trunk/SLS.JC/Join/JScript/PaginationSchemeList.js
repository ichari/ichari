$().ready(function() {
    InitData(5);
});

function InitData(LotteryID) {
    var tbody = "";

    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'SchemeList.ashx', //目标地址
        data: "LotteryID=" + LotteryID,
        beforeSend: function() { $("#divload").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function() { $("#divload").hide(); $("#SchemeList").show() }, //接收数据完毕
        success: function(json) {
            $("#SchemeList tr:gt(0)").remove();
            
            try
            {
                $("#SchemeList").append(json[0][0]["Content"]);
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