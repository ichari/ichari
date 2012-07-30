$(function() {
    window.onscroll = (function() {
        if ($("#tz").length == 1) {
            var offset = $("#tz").offset();
            var p = offset.top;
            var left = offset.left;
            if (document.documentElement.scrollTop < p) {
                $("#scroll").hide();
            } else {
                $("#scroll").show();
                $("#scroll").css({ position: "fixed", top: "0px", margin: "0 auto", width: "1000px" });
                $("#scroll").css("left", left);
                $("#selectSpValueOne").val($("#selectSpValue").val());
            }
        }
    });
});