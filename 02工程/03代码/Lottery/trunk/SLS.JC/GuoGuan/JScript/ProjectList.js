var Condition = "";
var PlayTypeID = "";
var Name = "";

$().ready(function() {
    $(".sta_nav ul li").click(function() {
        $(".sta_nav ul").children("li").removeClass("curr");
        $(this).addClass("curr");
        PlayTypeID = "";
        if (!isNaN($(this).attr("mid"))) {
            PlayTypeID = "&PlayTypeID=" + $(this).attr("mid");
            $("#hidPlayTypeID").val($(this).attr("mid"));
        }
        Condition = PlayTypeID + Name;

        InitData(0);
    });

    $("#srearch").click(function() {
        Name = "";
        PlayTypeID = "&PlayTypeID=" + $("#hidPlayTypeID").val();
        if (($("#SerachCondition").val() != "" && $("#SerachCondition").val() != "输入用户名或方案号")) {
            Name = "&Name=" + $("#SerachCondition").val();
        }
        Condition = PlayTypeID + Name;

        InitData(0);
    });

    $("#ddl_Day").change(function() {
        PlayTypeID = "&PlayTypeID=" + $("#hidPlayTypeID").val();
        Condition = PlayTypeID;

        InitData(0);
    });
});