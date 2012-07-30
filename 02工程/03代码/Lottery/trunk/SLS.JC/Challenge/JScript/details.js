var index = 0;
var state = false;
window.$ = jQuery;
Changepage = function(pageindex) {
    index = pageindex;    
    if (state) {
        Challenge_ChellengeUserSchemesDetails.GetPage($("#userId").val(), $("#dropYear").val(), $("#dropMonth").val(), pageindex, comeback);
    } else {
        Challenge_ChellengeUserSchemesDetails.GetPage($("#userId").val(),'', '', pageindex, comeback);
    }
}
comeback = function(res) {
    if (res == "" || res == null) {
        $("#productTable tr:gt(2)").remove();
        $("#productTable").append("");
        return;
    }
    $("#divload").hide();
    $("#productTable tr:gt(2)").remove();
    $("#productTable").append(res.value);
    $("#productTable tr:gt(2):odd").attr("class", "sk1");
    $("#productTable tr:gt(2):even").attr("class", "sk2");

}


Search = function() {
    state = true;
    $("#divload").show();
    $("#productTable tr:gt(2)").remove();
    $("#productTable").append("");
    var year = $("#dropYear").val()
    var month = $("#dropMonth").val()
    Challenge_ChellengeUserSchemesDetails.GetPage($("#userId").val(), year, month, 1, comeback);
}

document.getElementById("dropYear").options.add(new Option(new Date().getFullYear(), new Date().getFullYear()));
document.getElementById("dropYear").options.add(new Option(new Date().getFullYear() - 1, new Date().getFullYear() - 1));
document.getElementById("dropMonth").selectedIndex = new Date().getMonth();